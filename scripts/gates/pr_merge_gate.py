#!/usr/bin/env python3
"""Executable merge gate for pull requests (std-lib only, read-only).

Mechanises the checklist a merger runs before `gh pr merge`: PR state, pinned
head, merge state, CI, reviews, inline threads, lane signature, counter-review,
closing keywords, diff hygiene, protected paths, single taxonomy-CSV PR.
It never merges and never writes to GitHub.

Exit codes: 0 PASS, 3 HOLD, 2 BLOCK, 1 blind instrument (API failure,
incomplete pagination, missing core data). A failure never reads as PASS.

Architecture: collect() does all the I/O and returns a JSON-serialisable
snapshot; evaluate() is pure. `--self-test` runs offline.
See scripts/gates/README.md.
"""
from __future__ import annotations

import argparse
import copy
import io
import json
import os
import re
import subprocess
import sys
import tempfile
import time
from datetime import datetime, timezone

SCHEMA = 1
DEFAULT_REPO = "ArgumentumGames/Argumentum"
DEFAULT_MERGER_LANE = "ai-01"
MERGE_IDENTITY = "jsboige"
FALLBACK_CONTEXTS = ["build (Debug)", "build (Release)", "guard", "behaviour"]
TAXONOMY_CSV = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"

LEVELS = {"OK": 0, "INFO": 1, "HOLD": 2, "BLOCK": 3}
EXIT_CODES = {"PASS": 0, "HOLD": 3, "BLOCK": 2}
EXIT_BLIND = 1

LANE_TOKEN = r"ai-01|po-2023|po-2024|web2|cloud-[a-z0-9-]+"
LANE_RE = re.compile(r"^\*(" + LANE_TOKEN + r")\*$")
LANE_MENTION_RE = re.compile(r"(?<![A-Za-z0-9-])(" + LANE_TOKEN + r")(?![A-Za-z0-9])")
ATTRIB_RE = re.compile(r"^(?:\U0001F916\s*Generated with\b.*|Co-Authored-By:\s.*)$", re.I)
ACKABLE_READ = ("R5", "R10")


class Blind(Exception):
    """The instrument could not see: never a PASS, always exit code 1."""


# --------------------------------------------------------------------------
# I/O side: gh runner and collector
# --------------------------------------------------------------------------

def gh_run(args, timeout=120):
    """Run `gh <args>`; return (returncode, stdout, stderr) as text."""
    try:
        proc = subprocess.run(["gh"] + list(args), capture_output=True, timeout=timeout)
    except (OSError, subprocess.TimeoutExpired) as exc:
        return 1, "", "%s: %s" % (type(exc).__name__, exc)
    return (proc.returncode,
            proc.stdout.decode("utf-8", "replace"),
            proc.stderr.decode("utf-8", "replace"))


def _first_line(text, limit=160):
    for line in (text or "").replace("\r", "").split("\n"):
        if line.strip():
            return line.strip()[:limit]
    return ""


def _api_json(runner, path, what, extra=()):
    rc, out, err = runner(["api"] + list(extra) + [path])
    if rc != 0:
        raise Blind("%s : appel API en échec (rc=%s) -- %s" % (what, rc, _first_line(err) or "sans message"))
    try:
        return json.loads(out)
    except ValueError:
        raise Blind("%s : réponse non JSON" % what)


def _api_pages(runner, path, what, key=None, max_pages=40):
    """Read every page (100/page). Returns (items, total_count or None)."""
    items, total = [], None
    for page in range(1, max_pages + 1):
        sep = "&" if "?" in path else "?"
        data = _api_json(runner, "%s%sper_page=100&page=%d" % (path, sep, page), what)
        if key is not None:
            if not isinstance(data, dict) or not isinstance(data.get(key), list):
                raise Blind("%s : forme de réponse inattendue" % what)
            if total is None:
                total = data.get("total_count")
            chunk = data[key]
        else:
            chunk = data
        if not isinstance(chunk, list):
            raise Blind("%s : forme de réponse inattendue" % what)
        items.extend(chunk)
        if len(chunk) < 100:
            return items, total
    raise Blind("%s : pagination non terminée après %d pages" % (what, max_pages))


def _expect_count(what, got, want):
    if want is None or got != want:
        raise Blind("%s : %s lus pour %s annoncés -- pagination incomplète ou lecture concurrente"
                    % (what, got, want))


def _slim_user(user):
    user = user or {}
    return {"login": user.get("login") or "?", "type": user.get("type") or "?"}


def _slim_pr(p):
    out = {k: p.get(k) for k in (
        "number", "title", "body", "state", "draft", "merged", "mergeable", "mergeable_state",
        "comments", "review_comments", "commits", "additions", "deletions", "changed_files",
        "html_url")}
    out["user"] = _slim_user(p.get("user"))
    out["head"] = {"sha": p["head"]["sha"], "ref": p["head"].get("ref")}
    out["base"] = {"ref": p["base"]["ref"]}
    return out


_THREADS_QUERY = (
    "query($o:String!,$r:String!,$n:Int!,$c:String){repository(owner:$o,name:$r){"
    "pullRequest(number:$n){reviewThreads(first:100,after:$c){totalCount "
    "pageInfo{hasNextPage endCursor} nodes{isResolved isOutdated path line}}}}}")


def _collect_threads(runner, repo, pr):
    owner, _, name = repo.partition("/")
    nodes, cursor, total = [], None, None
    for _ in range(40):
        args = ["api", "graphql", "-f", "query=" + _THREADS_QUERY,
                "-f", "o=" + owner, "-f", "r=" + name, "-F", "n=%d" % pr]
        if cursor:
            args += ["-f", "c=" + cursor]
        rc, out, err = runner(args)
        if rc != 0:
            raise Blind("fils de revue : appel GraphQL en échec -- %s" % (_first_line(err) or "sans message"))
        try:
            data = json.loads(out)
            if data.get("errors"):
                raise Blind("fils de revue : GraphQL rend des erreurs")
            block = data["data"]["repository"]["pullRequest"]["reviewThreads"]
            total = block["totalCount"]
            nodes.extend(block["nodes"])
            more, cursor = block["pageInfo"]["hasNextPage"], block["pageInfo"]["endCursor"]
        except (ValueError, KeyError, TypeError):
            raise Blind("fils de revue : forme de réponse GraphQL inattendue")
        if not more:
            _expect_count("fils de revue", len(nodes), total)
            return nodes
    raise Blind("fils de revue : pagination non terminée")


def _required_contexts(runner, base, ref):
    rc, out, err = runner(["api", "%s/rules/branches/%s" % (base, ref)])
    if rc != 0:
        why = "lecture du ruleset en échec : %s" % (_first_line(err) or "sans message")
    else:
        try:
            rules = json.loads(out)
        except ValueError:
            rules = None
        if not isinstance(rules, list):
            why = "réponse du ruleset illisible"
        else:
            ctxs = []
            for rule in rules:
                if isinstance(rule, dict) and rule.get("type") == "required_status_checks":
                    for chk in (rule.get("parameters") or {}).get("required_status_checks") or []:
                        if chk.get("context") and chk["context"] not in ctxs:
                            ctxs.append(chk["context"])
            if ctxs:
                return {"source": "ruleset lu en direct (rules/branches/%s)" % ref, "contexts": ctxs}
            why = "le ruleset lu en direct ne déclare aucun check requis"
    return {"source": "REPLI NOMMÉ (%s)" % why, "contexts": list(FALLBACK_CONTEXTS)}


def collect(pr, repo=DEFAULT_REPO, runner=gh_run, sleep=time.sleep):
    """Read everything the rules need. Raises Blind on any core failure."""
    base = "repos/%s" % repo
    pr_path = "%s/pulls/%d" % (base, pr)
    prj = _api_json(runner, pr_path, "PR")
    tries = 0
    while (isinstance(prj, dict) and prj.get("state") == "open"
           and prj.get("mergeable_state") in (None, "unknown") and tries < 3):
        sleep(2)  # GitHub computes mergeability lazily; bounded retry
        prj = _api_json(runner, pr_path, "PR")
        tries += 1
    try:
        slim = _slim_pr(prj)
    except (KeyError, TypeError, AttributeError):
        raise Blind("PR : objet sans head.sha / base.ref")
    head = slim["head"]["sha"]
    snap = {"schema": SCHEMA, "repo": repo, "number": pr, "pr": slim,
            "collected_at": datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")}

    hc = _api_json(runner, "%s/commits/%s" % (base, head), "commit de tête")
    try:
        snap["head_commit"] = {"sha": hc["sha"], "date": hc["commit"]["committer"]["date"]}
    except (KeyError, TypeError):
        raise Blind("commit de tête : date absente")

    raw, _ = _api_pages(runner, "%s/issues/%d/comments" % (base, pr), "commentaires")
    _expect_count("commentaires", len(raw), slim.get("comments"))
    snap["issue_comments"] = [{"id": c.get("id"), "user": _slim_user(c.get("user")),
                               "created_at": c.get("created_at"), "body": c.get("body") or ""} for c in raw]

    raw, _ = _api_pages(runner, pr_path + "/reviews", "revues")
    snap["reviews"] = [{"id": r.get("id"), "user": _slim_user(r.get("user")), "state": r.get("state"),
                        "submitted_at": r.get("submitted_at"), "commit_id": r.get("commit_id"),
                        "body": r.get("body") or ""} for r in raw]

    raw, _ = _api_pages(runner, pr_path + "/comments", "commentaires inline")
    _expect_count("commentaires inline", len(raw), slim.get("review_comments"))
    snap["inline_comments"] = [{"id": c.get("id"), "user": _slim_user(c.get("user")), "path": c.get("path"),
                                "line": c.get("line"), "created_at": c.get("created_at"),
                                "body": c.get("body") or ""} for c in raw]

    # Both listings are capped by GitHub. Past the cap the gate cannot read every
    # path (R11) or every commit message (R9): say so, instead of "pagination".
    for what, field, cap in (("fichiers", "changed_files", MAX_LISTED_FILES),
                             ("commits", "commits", MAX_LISTED_COMMITS)):
        if isinstance(slim.get(field), int) and slim[field] > cap:
            raise Blind("%s : %d annoncés, l'API GitHub n'en liste que %d -- le gate ne peut pas tout lire, "
                        "il ne rend aucun verdict sur cette PR" % (what, slim[field], cap))

    raw, _ = _api_pages(runner, pr_path + "/files", "fichiers")
    _expect_count("fichiers", len(raw), slim.get("changed_files"))
    snap["files"] = [{k: f.get(k) for k in ("filename", "previous_filename", "status", "additions",
                                            "deletions", "patch")} for f in raw]

    raw, _ = _api_pages(runner, pr_path + "/commits", "commits")
    _expect_count("commits", len(raw), slim.get("commits"))
    snap["commits"] = [{"sha": c.get("sha"), "message": (c.get("commit") or {}).get("message") or ""}
                       for c in raw]

    rc, out, err = runner(["api", "-H", "Accept: application/vnd.github.v3.diff", pr_path])
    if rc == 0 and (out.startswith("diff --git ") or (out == "" and not snap["files"])):
        snap["diff_text"], snap["diff_error"] = out, None
    else:
        snap["diff_text"] = None
        snap["diff_error"] = _first_line(err) or "réponse inattendue"

    raw, total = _api_pages(runner, "%s/commits/%s/check-runs" % (base, head), "check-runs", key="check_runs")
    _expect_count("check-runs", len(raw), total)
    snap["checks"] = [{"name": c.get("name"), "status": c.get("status"), "conclusion": c.get("conclusion"),
                       "app": {"slug": (c.get("app") or {}).get("slug")}} for c in raw]

    st = _api_json(runner, "%s/commits/%s/status?per_page=100" % (base, head), "statuts")
    if not isinstance(st, dict) or not isinstance(st.get("statuses"), list):
        raise Blind("statuts : forme de réponse inattendue")
    if (st.get("total_count") or 0) > len(st["statuses"]):
        raise Blind("statuts : pagination incomplète")
    snap["statuses"] = [{"context": s.get("context"), "state": s.get("state")} for s in st["statuses"]]

    snap["threads"] = _collect_threads(runner, repo, pr)
    snap["required_contexts"] = _required_contexts(runner, base, slim["base"]["ref"])

    touched = set()
    for f in snap["files"]:
        touched.update(p for p in (f.get("filename"), f.get("previous_filename")) if p)
    snap["csv_taxonomy_other_prs"] = None
    if TAXONOMY_CSV in touched:
        opens, _ = _api_pages(runner, "%s/pulls?state=open" % base, "PR ouvertes")
        others = []
        for other in opens:
            num = other.get("number")
            if num == pr or ((other.get("user") or {}).get("login") or "").startswith("dependabot"):
                continue
            ofiles, _ = _api_pages(runner, "%s/pulls/%d/files" % (base, num), "fichiers de #%s" % num)
            if any(TAXONOMY_CSV in (f.get("filename"), f.get("previous_filename")) for f in ofiles):
                others.append({"number": num, "title": other.get("title")})
        snap["csv_taxonomy_other_prs"] = others
    return snap


# --------------------------------------------------------------------------
# Pure side: helpers (no I/O below this line until the CLI)
# --------------------------------------------------------------------------

def _ts(value):
    try:
        return datetime.fromisoformat(str(value).replace("Z", "+00:00"))
    except (TypeError, ValueError):
        return None


def _signature_lines(body):
    """Body lines with trailing blank and attribution lines removed."""
    lines = (body or "").replace("\r\n", "\n").split("\n")
    while lines and (not lines[-1].strip() or ATTRIB_RE.match(lines[-1].strip())):
        lines.pop()
    return lines


def lane_signature(body):
    """Lane named by the literal signature on the last meaningful line, else None."""
    lines = _signature_lines(body)
    if not lines:
        return None
    m = LANE_RE.match(lines[-1].strip())
    return m.group(1) if m else None


def _glob_re(glob, flags=0):
    out, i = "", 0
    while i < len(glob):
        if glob.startswith("**/", i):
            out, i = out + "(?:.*/)?", i + 3
        elif glob.startswith("**", i):
            out, i = out + ".*", i + 2
        elif glob[i] == "*":
            out, i = out + "[^/]*", i + 1
        elif glob[i] == "?":
            out, i = out + "[^/]", i + 1
        else:
            out, i = out + re.escape(glob[i]), i + 1
    return re.compile("^" + out + "$", flags)


def _glob_any(path, globs, flags=0):
    return any(_glob_re(g, flags).match(path) for g in globs)


def _protected(path, globs):
    # A protected path is matched whatever its case: `Web.config` and
    # `web.config` are one file on a Windows checkout, and the repository tracks
    # both spellings. Acknowledgement globs stay case-exact (the narrower reading).
    return _glob_any(path, globs, re.I)


def _rx(*parts, **kw):
    # Patterns are assembled from fragments so that this file never contains a
    # string its own rules (or an external secret scanner) would flag.
    return re.compile("".join(parts), kw.get("flags", 0))


HARD_PATTERNS = (
    ("jeton GitHub", _rx(r"(?<![A-Za-z0-9_])gh", r"[pousr]_[A-Za-z0-9]{36,}")),
    ("jeton GitHub à portée fine", _rx(r"(?<![A-Za-z0-9_])github", r"_pat_[A-Za-z0-9_]{22,}")),
    ("clé de forme sk", _rx(r"(?<![A-Za-z0-9_-])sk", r"-(?=[A-Za-z0-9_-]*[0-9])(?=[A-Za-z0-9_-]*[A-Za-z])",
                            r"[A-Za-z0-9_-]{20,}")),
    ("identifiant de clé AWS", _rx(r"(?<![A-Z0-9])AK", r"IA[0-9A-Z]{16}(?![0-9A-Z])")),
    ("jeton Slack", _rx(r"xox", r"[bap]-[A-Za-z0-9-]{10,}")),
    ("clé privée PEM", _rx(r"-----BEGIN ", r"[A-Z ]*PRIVATE ", r"KEY-----")),
)
_KW = r"(?i)(?:api[_-]?key|token|secret|pass" + r"word|passwd|pwd)\s*[:=]\s*"
_VAL = (r"(?:\"[^\"\s]{12,}\"|'[^'\s]{12,}'"
        r"|(?=[A-Za-z0-9_\-+/=]*[0-9])[A-Za-z0-9_\-+/=]{12,}(?=\s|$|[;\"'&]))")
SOFT_PATTERNS = (
    ("affectation d'une valeur à un nom de secret", re.compile(_KW + _VAL)),
    ("chemin de profil machine", _rx(r"[A-Za-z]:[\\/](?i:users)[\\/]", r"|/home/[A-Za-z0-9._-]+/",
                                     r"|/(?i:users)/[A-Za-z0-9._-]+/")),
    ("mention du dossier de clés", _rx(r"(?i:\.keys)", r"[\\/]")),
)
def _looks_like_address(dotted):
    # A taxonomy decimal path ("2.3.2.1") has the shape of an address. Keep only
    # quads whose first part is >= 10 or that hold a part >= 20 (measured on a
    # real taxonomy PR: 17 occurrences of 5 section numbers were otherwise
    # reported as addresses). Known cost: 1.1.1.1-style quads are not noted.
    parts = [int(p) for p in dotted.split(".")]
    return parts[0] >= 10 or max(parts) >= 20


NOTE_PATTERNS = (
    ("chemin de lecteur", _rx(r"(?<![A-Za-z])[A-Za-z]:", r"\\(?![Uu]sers[\\/])"), None),
    ("adresse IPv4", re.compile(r"(?<![\d.])(?:(?:25[0-5]|2[0-4]\d|1?\d?\d)\.){3}"
                                r"(?:25[0-5]|2[0-4]\d|1?\d?\d)(?![\d.])"), _looks_like_address),
)
PROSE_MARKERS = (
    ("CHANGES_REQUESTED", re.compile(r"CHANGES[_ ]REQUESTED", re.I)),
    ("renvoy", re.compile(r"renvoy", re.I)),
    ("à corriger avant", re.compile(r"[àa] corriger avant", re.I)),
    ("ne pas merger", re.compile(r"ne pas merger", re.I)),
    ("do not merge", re.compile(r"do not merge", re.I)),
    ("ligne ouverte par le signe sens interdit", re.compile(r"(?m)^[ \t>*-]*⛔")),
)
# GitHub closes an issue on `<keyword> #N`, `<keyword> owner/repo#N`, `<keyword> GH-N`
# and `<keyword> <issue URL>`. The URL form is how another repository's issue gets closed.
CLOSING_RE = re.compile(
    r"(?i)\b(?:close[sd]?|fix(?:e[sd])?|resolve[sd]?)\s*:?\s+"
    r"(?:(?P<repo>[\w.-]+/[\w.-]+)?#|GH-"
    r"|https?://(?:www\.)?github\.com/(?P<urlrepo>[\w.-]+/[\w.-]+)/(?:issues|pull)/)(?P<num>\d+)")
ARCHIVE_GLOBS = ("Cards/*/Archive", "Cards/*/Archive/**",
                 "Generation/CardPen/examples", "Generation/CardPen/examples/**")
SENSITIVE_GLOBS = (".github/workflows/**", ".claude/**", "AGENTS.md", "CLAUDE.md",
                   "scripts/gates/**", "**/web.config")
_KEYS_DIR = "." + "keys"
_ENV_RE = re.compile(r"^\.env(?:rc)?(?:$|[._-])")
ENV_MODELS = tuple(".env" + sfx for sfx in (".example", ".sample", ".template", ".dist"))
BAD_CONCLUSIONS = ("failure", "timed_out", "cancelled", "action_required", "startup_failure")
HUNK_RE = re.compile(r"^@@ -\d+(?:,(\d+))? \+(\d+)(?:,(\d+))? @@")
MAX_LISTED_COMMITS, MAX_LISTED_FILES = 250, 3000  # caps of the two GitHub listings


def _env_file(name):
    """'secret' for an environment file, 'model' for its committed template, else None.

    `.env`, `.env.local`, `.env-prod`, `.envrc` are secrets; `.env.example` and
    its three usual spellings are models; `.environment.md` is neither.
    """
    name = name.lower()
    if not _ENV_RE.match(name):
        return None
    return "model" if name in ENV_MODELS else "secret"


def _is_secret_path(path):
    segs = [seg.lower() for seg in path.split("/")]
    return _env_file(segs[-1]) == "secret" or segs[-1].endswith(".pfx") or _KEYS_DIR in segs


def _unquote(text):
    body, out, i = text[1:-1], bytearray(), 0
    while i < len(body):
        ch = body[i]
        if ch == "\\" and i + 1 < len(body):
            nxt = body[i + 1]
            if nxt in "01234567":
                out.append(int(body[i + 1:i + 4], 8) & 0xFF)
                i += 4
                continue
            out.extend({"n": b"\n", "t": b"\t"}.get(nxt, nxt.encode("utf-8")))
            i += 2
            continue
        out.extend(ch.encode("utf-8"))
        i += 1
    return out.decode("utf-8", "replace")


def _diff_path(text):
    text = text.rstrip("\r")
    if text.startswith('"') and text.endswith('"') and len(text) >= 2:
        text = _unquote(text)
    else:
        text = text.split("\t")[0]
    if text == "/dev/null":
        return None
    return text[2:] if text.startswith("b/") else text


def _parse_diff(text, single_path=None):
    """Added lines of a unified diff: {path: [(new_line_number, text)]}.

    Hunk bodies are consumed by the counts of their header, so an added line
    whose own text starts with '+++' or 'diff --git' is never read as a header.
    """
    added, path = {}, single_path
    old_left = new_left = lineno = 0
    for raw in text.split("\n"):
        if old_left > 0 or new_left > 0:
            tag = raw[:1]
            if tag == "+":
                added.setdefault(path, []).append((lineno, raw[1:]))
                lineno += 1
                new_left -= 1
            elif tag == "-":
                old_left -= 1
            elif tag != "\\":
                old_left -= 1
                new_left -= 1
                lineno += 1
            continue
        m = HUNK_RE.match(raw)
        if m:
            old_left = 1 if m.group(1) is None else int(m.group(1))
            lineno = int(m.group(2))
            new_left = 1 if m.group(3) is None else int(m.group(3))
        elif single_path is None:
            if raw.startswith("diff --git "):
                path = None
            elif raw.startswith("+++ "):
                path = _diff_path(raw[4:])
    return added


def _added_lines(s):
    """-> (added, blind, mode). `blind` names every file the scan could not fully read."""
    files = s["files"]
    if s.get("diff_text") is not None:
        added, mode = _parse_diff(s["diff_text"]), "diff unifié"
    else:
        added = {}
        for f in files:
            if f.get("patch"):
                added.update(_parse_diff(f["patch"], single_path=f["filename"]))
        mode = "REPLI sur les patchs par fichier (diff unifié indisponible : %s)" % (
            s.get("diff_error") or "raison inconnue")
    blind, names = [], set()
    for f in files:
        names.add(f["filename"])
        want, got = f.get("additions") or 0, len(added.get(f["filename"], []))
        if got != want:
            blind.append("%s (%d lignes ajoutées annoncées, %d lues)" % (f["filename"], want, got))
    for extra in sorted(str(p) for p in set(added) - names):
        blind.append("%s (dans le diff, absent de la liste des fichiers)" % extra)
    return added, blind, mode


def _context(s):
    return {"head": s["pr"]["head"]["sha"], "head_dt": _ts(s["head_commit"]["date"]),
            "lane": lane_signature(s["pr"].get("body")), "author": s["pr"]["user"]["login"]}


def _short(when):
    return str(when or "date inconnue")[:16].replace("T", " ")


def _cap(items, limit=6):
    items = list(items)
    more = len(items) - limit
    return ", ".join(items[:limit]) + (" (+%d autres)" % more if more > 0 else "")


# --------------------------------------------------------------------------
# Rules. Each returns (reasons, ok_text); a reason is (level, text).
# --------------------------------------------------------------------------

def r1_state(s, o, c):
    pr, reasons = s["pr"], []
    if pr.get("state") != "open":
        reasons.append(("BLOCK", "PR non ouverte (state=%s, merged=%s)" % (pr.get("state"), pr.get("merged"))))
    if pr.get("draft"):
        reasons.append(("BLOCK", "PR en brouillon"))
    if pr["base"]["ref"] != "master":
        reasons.append(("BLOCK", "base = %s, attendu master" % pr["base"]["ref"]))
    return reasons, "PR ouverte, non brouillon, base master"


def r2_head(s, o, c):
    want, head = o["expect_head"], c["head"]
    if not want:
        return [("INFO", "tête non épinglée par l'appelant (--expect-head absent) ; tête lue = %s" % head[:12])], ""
    if len(want) >= 7 and head.lower().startswith(want.lower()):
        return [], "tête = %s, conforme à --expect-head" % head[:12]
    return [("BLOCK", "tête lue %s différente de --expect-head %s (préfixe d'au moins 7 caractères exigé)"
             % (head[:12], want[:12]))], ""


def r3_mergeable(s, o, c):
    state, reasons = s["pr"].get("mergeable_state"), []
    if state == "dirty" or s["pr"].get("mergeable") is False:
        reasons.append(("BLOCK", "conflit avec la base (mergeable_state=%s)" % state))
    elif state in (None, "unknown"):
        reasons.append(("HOLD", "état de fusion non calculé par GitHub (unknown) : relancer"))
    elif state == "behind":
        reasons.append(("INFO", "en retard sur la base (behind) : toléré, le ruleset n'est pas strict"))
    elif state == "blocked":
        # GitHub does not say WHY. If R4, R5 or R6 hold or block, the cause is in view.
        # Otherwise it lies where the gate does not look (an approval rule of the
        # ruleset, an unattributed push) and a PASS would print a command GitHub refuses.
        seen = any(level in ("HOLD", "BLOCK") for fn in (r4_ci, r5_reviews, r6_threads)
                   for level, _ in fn(s, o, c)[0])
        if seen:
            reasons.append(("INFO", "GitHub déclare la fusion bloquée : cause en vue dans R4, R5 ou R6"))
        else:
            reasons.append(("HOLD", "GitHub déclare la fusion bloquée et ni R4 (checks), ni R5 (revues), ni R6 "
                            "(fils) n'en voient la cause : règle du ruleset que le gate ne lit pas "
                            "(approbation exigée, poussée non attribuée), ou état pas encore recalculé -- "
                            "relancer, puis lire le ruleset"))
    elif state not in ("clean", "has_hooks"):
        reasons.append(("INFO", "mergeable_state=%s : voir R4" % state))
    return reasons, "mergeable_state=%s" % state


def r4_ci(s, o, c):
    runs, reasons = {}, []
    for chk in s["checks"]:
        runs.setdefault(chk.get("name") or "?", []).append((chk.get("status"), chk.get("conclusion")))
    for st in s["statuses"]:
        state = st.get("state")
        conv = ("in_progress", None) if state == "pending" else (
            "completed", "success" if state == "success" else "failure")
        runs.setdefault(st.get("context") or "?", []).append(conv)
    required = s["required_contexts"]["contexts"]
    for name in required:
        if name not in runs:
            reasons.append(("HOLD", "check requis absent sur la tête : %s" % name))
    for name in sorted(runs):
        for status, concl in runs[name]:
            if status != "completed":
                reasons.append(("HOLD", "CI en cours, pas verte : %s (%s)" % (name, status)))
            elif concl in BAD_CONCLUSIONS:
                reasons.append(("BLOCK", "check en échec : %s (%s)" % (name, concl)))
            elif concl != "success" and (name in required or concl not in ("neutral", "skipped")):
                reasons.append(("HOLD", "check non concluant : %s (%s)" % (name, concl)))
    return reasons, "%d checks requis verts (%s) ; %d exécutions lues, aucune en échec ni en cours" % (
        len(required), ", ".join(required), sum(len(v) for v in runs.values()))


def r5_reviews(s, o, c):
    reasons, last = [], {}
    for rv in sorted(s["reviews"], key=lambda r: r.get("submitted_at") or ""):
        if rv.get("state") in ("CHANGES_REQUESTED", "APPROVED", "DISMISSED"):
            last[rv["user"]["login"]] = rv
    for login in sorted(last):
        if last[login]["state"] == "CHANGES_REQUESTED":
            reasons.append(("BLOCK", "CHANGES_REQUESTED de %s (%s) non levée par le même auteur"
                            % (login, _short(last[login].get("submitted_at")))))
    items = [("revue", r, r.get("submitted_at"), r.get("commit_id") == c["head"]) for r in s["reviews"]]
    items += [("commentaire", m, m.get("created_at"), False) for m in s["issue_comments"]]
    items += [("commentaire inline %s" % m.get("path"), m, m.get("created_at"), False)
              for m in s["inline_comments"]]
    for kind, item, when, at_head in items:
        names = [name for name, rx in PROSE_MARKERS if rx.search(item.get("body") or "")]
        if not names:
            continue
        dt = _ts(when)
        after = at_head or dt is None or dt > c["head_dt"]
        text = "%s de %s (%s) porte un marqueur bloquant [%s]" % (
            kind, item["user"]["login"], _short(when), ", ".join(names))
        if not after:
            reasons.append(("INFO", text + " -- antérieur à la tête"))
        elif "R5" in o["ack_read"]:
            reasons.append(("INFO", text + " -- postérieur à la tête, acquitté par --ack-read R5"))
        else:
            reasons.append(("HOLD", text + " -- postérieur à la tête : à lire (heuristique de prose, "
                            "levée par --ack-read R5 après lecture)"))
    return reasons, "%d revues lues, aucune CHANGES_REQUESTED en vigueur, aucun marqueur bloquant en prose" % len(
        s["reviews"])


def r6_threads(s, o, c):
    reasons = []
    live = [t for t in s["threads"] if not t.get("isResolved") and not t.get("isOutdated")]
    stale = [t for t in s["threads"] if not t.get("isResolved") and t.get("isOutdated")]
    if live:
        reasons.append(("BLOCK", "%d fil(s) inline non résolu(s) : %s" % (
            len(live), _cap("%s:%s" % (t.get("path"), t.get("line")) for t in live))))
    if stale:
        reasons.append(("INFO", "%d fil(s) non résolu(s) mais périmé(s) (outdated) : %s" % (
            len(stale), _cap(str(t.get("path")) for t in stale))))
    return reasons, "%d fil(s) inline, tous résolus" % len(s["threads"])


def r7_signature(s, o, c):
    if c["lane"]:
        return [], "corps signé *%s* en dernière ligne" % c["lane"]
    body = s["pr"].get("body") or ""
    seen = sorted(set(LANE_MENTION_RE.findall(body)))
    clue = ("mention(s) non littérale(s) dans le corps : %s" % ", ".join(seen)) if seen else (
        "aucune mention de lane dans le corps")
    return [("HOLD", "pas de signature littérale *lane* en dernière ligne du corps : provenance par "
             "recoupement (%s)" % clue)], ""


def r8_counter_review(s, o, c):
    lane, author, found = c["lane"], c["author"], []

    def consider(kind, item, when, at_head):
        if not at_head:
            return
        login, sig = item["user"]["login"], lane_signature(item.get("body"))
        if sig and lane and sig != lane:
            found.append("%s signée *%s* (%s)" % (kind, sig, _short(when)))
        elif login != author and item["user"]["type"] != "Bot" and not login.endswith("[bot]"):
            found.append("%s du compte %s (%s)" % (kind, login, _short(when)))

    for rv in s["reviews"]:
        consider("revue", rv, rv.get("submitted_at"), rv.get("commit_id") == c["head"])
    for cm in s["issue_comments"]:
        dt = _ts(cm.get("created_at"))
        consider("commentaire", cm, cm.get("created_at"), dt is not None and dt > c["head_dt"])
    if found:
        return [], "contre-revue présente sur la tête (présence, pas teneur) : %s" % _cap(found, 3)
    if o["require_counter_review"] or (lane is not None and lane == o["merger_lane"]):
        why = "--require-counter-review" if o["require_counter_review"] else (
            "la PR est de la lane du merger (*%s*)" % lane)
        return [("HOLD", "aucune contre-revue sur la tête par une autre lane ni un autre compte, or %s : "
                 "producteur et vérificateur doivent différer (revue rattachée au commit de tête, ou "
                 "commentaire postérieur à sa date -- approximation)" % why)], ""
    if lane is None:
        return [("INFO", "aucune contre-revue trouvée, et lane de la PR inconnue (R7) : le gate ne peut pas "
                 "établir que le merger n'est pas le producteur")], ""
    return [("INFO", "aucune contre-revue ; PR de *%s*, merger *%s* : le merger est le vérificateur"
             % (lane, o["merger_lane"]))], ""


def r9_closing(s, o, c):
    found, reasons = {}, []
    sources = [("corps", s["pr"].get("body") or ""), ("titre", s["pr"].get("title") or "")]
    sources += [("commit %s" % (cm.get("sha") or "")[:7], cm.get("message") or "") for cm in s["commits"]]
    here = (s.get("repo") or "").lower()
    for where, text in sources:
        for m in CLOSING_RE.finditer(text):
            repo = m.group("repo") or m.group("urlrepo") or ""
            target = "#%s" % m.group("num") if repo.lower() in ("", here) else "%s#%s" % (repo, m.group("num"))
            found.setdefault((int(m.group("num")), target), set()).add(where)
    for num, target in sorted(found):
        where = _cap(sorted(found[(num, target)]), 3)
        if num in o["ack_closes"]:
            reasons.append(("INFO", "le merge fermera %s (%s) -- acquitté par --ack-closes" % (target, where)))
        else:
            reasons.append(("HOLD", "le merge fermera %s (%s) : confronter à la DoD de l'issue, puis "
                            "--ack-closes %d" % (target, where, num)))
    return reasons, "aucun mot de fermeture dans le corps, le titre ni les messages de commit"


def r10_diff(s, o, c):
    added, blind, mode = _added_lines(s)
    groups, reasons = {}, []
    for path in sorted(added, key=str):
        for lineno, text in added[path]:
            for level, patterns in (("BLOCK", HARD_PATTERNS), ("HOLD", SOFT_PATTERNS)):
                for name, rx in patterns:
                    if rx.search(text):
                        groups.setdefault((level, name), []).append("%s:%d" % (path, lineno))
            for name, rx, accept in NOTE_PATTERNS:
                if any(accept is None or accept(m.group(0)) for m in rx.finditer(text)):
                    groups.setdefault(("INFO", name), []).append("%s:%d" % (path, lineno))
    for level, name in sorted(groups, key=lambda k: (-LEVELS[k[0]], k[1])):
        text = "%s : %s" % (name, _cap(groups[(level, name)]))  # location and pattern name, never the value
        if level == "HOLD" and "R10" in o["ack_read"]:
            level, text = "INFO", text + " -- acquitté par --ack-read R10"
        elif level == "HOLD":
            text += " (heuristique : lire, puis --ack-read R10)"
        reasons.append((level, text))
    if blind:
        reasons.append(("HOLD", "scan AVEUGLE sur %d fichier(s), sans dérogation : %s" % (len(blind), _cap(blind, 4))))
    if s.get("diff_text") is None:
        reasons.append(("INFO", mode))
    return reasons, "%d ligne(s) ajoutée(s) lue(s) dans %d fichier(s) (%s), aucun motif" % (
        sum(len(v) for v in added.values()), len(added), mode)


def r11_paths(s, o, c):
    seen, secret, archive, shared, acked = set(), [], [], [], []
    for f in s["files"]:
        for path in (f.get("filename"), f.get("previous_filename")):
            if not path or path in seen:
                continue
            seen.add(path)
            if _is_secret_path(path):
                secret.append(path)
            elif _protected(path, ARCHIVE_GLOBS):
                (acked if _glob_any(path, o["ack_paths"]) else archive).append(path)
            elif _protected(path, SENSITIVE_GLOBS) or _env_file(path.split("/")[-1]) == "model":
                (acked if _glob_any(path, o["ack_paths"]) else shared).append(path)
    reasons = []
    if secret:
        reasons.append(("BLOCK", "chemin de secret, sans dérogation : %s" % _cap(secret)))
    if archive:
        reasons.append(("BLOCK", "référence imprimée (Archive / examples), levée seulement par --ack-path : %s"
                        % _cap(archive)))
    if shared:
        reasons.append(("HOLD", "fichier partagé ou modèle d'environnement, intention à confirmer "
                        "(--ack-path, à la casse exacte) : %s" % _cap(shared)))
    if acked:
        reasons.append(("INFO", "chemin protégé acquitté par --ack-path : %s" % _cap(acked)))
    return reasons, "%d chemin(s), aucun protégé" % len(seen)


def _touched(s):
    out = set()
    for f in s["files"]:
        out.update(p for p in (f.get("filename"), f.get("previous_filename")) if p)
    return out


def r12_csv(s, o, c):
    touched, reasons = _touched(s), []
    if TAXONOMY_CSV in touched and s["csv_taxonomy_other_prs"]:
        reasons.append(("BLOCK", "une seule PR à la fois sur le CSV de taxonomie ; autre(s) PR ouverte(s) : %s"
                        % _cap("#%s" % p.get("number") for p in s["csv_taxonomy_other_prs"])))
    csvs = sorted(p for p in touched if p.startswith("Cards/") and p.lower().endswith(".csv"))
    if csvs:
        reasons.append(("INFO", "CSV touché(s) : %s -- re-parse BOM/CRLF/colonnes à faire "
                        "(tools/bytecheck-datasets.py) ; le gate ne re-parse pas" % _cap(csvs, 3)))
    return reasons, "aucun CSV sous Cards/ touché"


RULES = (
    ("R1", "état", r1_state), ("R2", "tête épinglée", r2_head), ("R3", "état de fusion", r3_mergeable),
    ("R4", "CI", r4_ci), ("R5", "revues", r5_reviews), ("R6", "fils inline", r6_threads),
    ("R7", "signature de lane", r7_signature), ("R8", "contre-revue", r8_counter_review),
    ("R9", "mots de fermeture", r9_closing), ("R10", "hygiène du diff", r10_diff),
    ("R11", "chemins protégés", r11_paths), ("R12", "CSV de taxonomie", r12_csv),
)


def normalize_options(options=None):
    o = dict(options or {})
    return {"expect_head": o.get("expect_head") or None,
            "merger_lane": o.get("merger_lane") or DEFAULT_MERGER_LANE,
            "require_counter_review": bool(o.get("require_counter_review")),
            "ack_closes": frozenset(int(n) for n in (o.get("ack_closes") or ())),
            "ack_paths": tuple(o.get("ack_paths") or ()),
            "ack_read": frozenset(o.get("ack_read") or ())}


def _validate(s):
    def need(cond, what):
        if not cond:
            raise Blind("instantané incomplet ou incohérent : %s" % what)

    need(isinstance(s, dict), "ce n'est pas un objet")
    need(s.get("schema") == SCHEMA, "schéma %r inattendu" % (s.get("schema"),))
    pr = s.get("pr")
    need(isinstance(pr, dict), "objet PR absent")
    try:
        head, _, _ = pr["head"]["sha"], pr["base"]["ref"], pr["user"]["login"]
    except (KeyError, TypeError):
        raise Blind("instantané incomplet ou incohérent : PR sans head.sha / base.ref / user.login")
    need(isinstance(head, str) and len(head) == 40, "head.sha n'est pas un SHA complet")
    need(isinstance(pr.get("number"), int), "numéro de PR absent")
    for key in ("issue_comments", "reviews", "inline_comments", "threads", "files", "commits",
                "checks", "statuses"):
        need(isinstance(s.get(key), list), "%s absent" % key)
    hc = s.get("head_commit") or {}
    need(hc.get("sha") == head and _ts(hc.get("date")) is not None, "commit de tête absent ou non daté")
    rq = s.get("required_contexts") or {}
    need(isinstance(rq.get("contexts"), list) and rq["contexts"] and rq.get("source"), "contextes requis absents")
    for key, field in (("files", "changed_files"), ("issue_comments", "comments"),
                       ("inline_comments", "review_comments"), ("commits", "commits")):
        need(len(s[key]) == pr.get(field), "%s : %d lus pour %s annoncés" % (key, len(s[key]), pr.get(field)))
    need(all(isinstance(f, dict) and f.get("filename") for f in s["files"]), "fichier sans nom")
    need("diff_text" in s, "diff absent")
    if TAXONOMY_CSV in _touched(s):
        need(isinstance(s.get("csv_taxonomy_other_prs"), list),
             "CSV de taxonomie touché mais les autres PR ouvertes n'ont pas été lues")


def merge_command(s):
    repo_opt = "" if s.get("repo") == DEFAULT_REPO else " -R %s" % s.get("repo")
    return ("GH_TOKEN=$(gh auth token --user %s) gh pr merge %d%s --squash --delete-branch "
            "--match-head-commit %s" % (MERGE_IDENTITY, s["pr"]["number"], repo_opt, s["pr"]["head"]["sha"]))


def evaluate(snapshot, options=None, disabled=frozenset()):
    """Pure: snapshot + options -> result. Raises Blind when the snapshot cannot be trusted.

    `disabled` exists for the self-test's falsifying mutations only; a result
    obtained with a rule disabled never carries a merge command.
    """
    o = normalize_options(options)
    _validate(snapshot)
    c = _context(snapshot)
    findings = []
    for rid, title, fn in RULES:
        if rid in disabled:
            continue
        reasons, ok_text = fn(snapshot, o, c)
        reasons = sorted(reasons or [("OK", ok_text)], key=lambda r: -LEVELS[r[0]])
        findings.append({"rule": rid, "title": title, "level": reasons[0][0],
                         "reasons": [list(r) for r in reasons]})
    worst = max([LEVELS[f["level"]] for f in findings] or [0])
    verdict = "BLOCK" if worst == 3 else "HOLD" if worst == 2 else "PASS"
    pr = snapshot["pr"]
    surface = {"body_chars": len(pr.get("body") or ""),
               "comments": [len(m.get("body") or "") for m in snapshot["issue_comments"]],
               "reviews": [[r.get("state"), len(r.get("body") or "")] for r in snapshot["reviews"]],
               "inline_comments": [len(m.get("body") or "") for m in snapshot["inline_comments"]],
               "threads": len(snapshot["threads"]),
               "threads_unresolved": sum(1 for t in snapshot["threads"] if not t.get("isResolved")),
               "files": len(snapshot["files"]), "additions": pr.get("additions"),
               "deletions": pr.get("deletions")}
    return {"verdict": verdict, "exit_code": EXIT_CODES[verdict], "findings": findings, "surface": surface,
            "head": c["head"], "lane": c["lane"], "required_contexts": snapshot["required_contexts"],
            "disabled": sorted(disabled),
            "merge_command": merge_command(snapshot) if verdict == "PASS" and not disabled else None}


NOT_ESTABLISHED = (
    "il ne lit pas le fond à la place du merger : corps, commentaires, revues et diff restent à lire ;",
    "il ne re-parse aucun CSV (BOM, CRLF, colonnes) : c'est le rôle de tools/bytecheck-datasets.py ;",
    "il ne juge aucun verdict visuel (rendu de carte, PDF, mindmap) ;",
    "une signature de lane est déclarative : plusieurs lanes écrivent par le même compte, le gate "
    "n'authentifie pas qui a écrit -- R7 et R8 voient l'oubli, pas l'usurpation ;",
    "R5 (prose), R8 (dates) et R10 (motifs) sont des heuristiques : leur silence n'est pas une preuve.",
)


def render(s, result, origin="lecture en direct"):
    pr, sf, out = s["pr"], result["surface"], []
    out.append("Gate de merge -- PR #%d (%s)" % (pr["number"], s.get("repo")))
    out.append("  titre : %s" % pr.get("title"))
    out.append("  tête  : %s" % result["head"])
    out.append("  lane  : %s   données : %s, collectées le %s" % (
        result["lane"] or "inconnue", origin, s.get("collected_at")))
    out.append("")
    for f in result["findings"]:
        out.append("%-4s %-5s %s -- %s" % (f["rule"], f["level"], f["title"], f["reasons"][0][1]))
        for level, text in f["reasons"][1:]:
            out.append("           [%s] %s" % (level, text))
    out.append("")
    rq = result["required_contexts"]
    out.append("Contextes requis : %s -- source : %s" % (", ".join(rq["contexts"]), rq["source"]))
    out.append("VERDICT : %s (code de sortie %d)" % (result["verdict"], result["exit_code"]))
    if result["merge_command"]:
        out.append("")
        out.append("Commande de merge, à lancer par le merger après lecture du fond (le gate ne merge jamais) :")
        out.append("  " + result["merge_command"])
    elif result["verdict"] == "PASS":
        out.append("  (règles désactivées : %s -- aucune commande de merge)" % ", ".join(result["disabled"]))
    out.append("")
    out.append("Ce que ce gate n'établit pas :")
    out.extend("  - " + line for line in NOT_ESTABLISHED)
    out.append("")
    out.append("Surface de lecture (ce que le merger doit avoir lu) :")
    out.append("  corps : %d caractères" % sf["body_chars"])
    out.append("  commentaires : %d%s" % (len(sf["comments"]), _sizes(sf["comments"])))
    out.append("  revues : %d%s" % (len(sf["reviews"]), _sizes(["%s %d" % (st, n) for st, n in sf["reviews"]])))
    out.append("  commentaires inline : %d%s ; fils : %d (%d non résolus)" % (
        len(sf["inline_comments"]), _sizes(sf["inline_comments"]), sf["threads"], sf["threads_unresolved"]))
    out.append("  fichiers : %d (+%s / -%s)" % (sf["files"], sf["additions"], sf["deletions"]))
    return "\n".join(out) + "\n"


def _sizes(values):
    return (" (tailles : %s)" % _cap((str(v) for v in values), 12)) if values else ""


# --------------------------------------------------------------------------
# CLI
# --------------------------------------------------------------------------

class _Parser(argparse.ArgumentParser):
    def error(self, message):
        # argparse exits with 2 by default, which means BLOCK here: use the blind code instead.
        self.print_usage(sys.stderr)
        sys.stderr.write("%s : arguments invalides : %s\n" % (self.prog, message))
        sys.exit(EXIT_BLIND)


def build_parser():
    p = _Parser(prog="pr_merge_gate.py",
                description="Gate de merge exécutable : lit une PR, rend PASS (0), HOLD (3), BLOCK (2) "
                            "ou instrument aveugle (1). Ne merge jamais.")
    p.add_argument("pr", nargs="?", type=int, help="numéro de la PR")
    p.add_argument("--repo", default=DEFAULT_REPO)
    p.add_argument("--expect-head", metavar="SHA", help="tête attendue (préfixe d'au moins 7 caractères)")
    p.add_argument("--merger-lane", default=DEFAULT_MERGER_LANE, help="lane qui merge (défaut : %(default)s)")
    p.add_argument("--require-counter-review", action="store_true",
                   help="exiger une contre-revue même si la PR vient d'une autre lane")
    p.add_argument("--ack-closes", default="", metavar="N[,N]",
                   help="issues dont la fermeture par le merge a été confrontée à leur DoD")
    p.add_argument("--ack-path", action="append", default=[], metavar="GLOB",
                   help="chemin protégé dont la modification est voulue (répétable)")
    p.add_argument("--ack-read", default="", metavar="R5[,R10]",
                   help="lever les HOLD heuristiques de R5 (prose) et R10 (motifs) après lecture ; "
                        "ne lève aucun BLOCK ni le scan aveugle")
    p.add_argument("--snapshot-out", metavar="FILE", help="écrire l'instantané collecté (JSON)")
    p.add_argument("--from-snapshot", metavar="FILE", help="rejouer un instantané, sans réseau")
    p.add_argument("--json", action="store_true", help="sortie JSON")
    p.add_argument("--self-test", action="store_true", help="auto-test hors ligne")
    return p


def _options_from_args(args):
    try:
        closes = [int(n) for n in args.ack_closes.replace("#", "").split(",") if n.strip()]
    except ValueError:
        raise Blind("--ack-closes attend des numéros séparés par des virgules")
    reads = [r.strip().upper() for r in args.ack_read.split(",") if r.strip()]
    if any(r not in ACKABLE_READ for r in reads):
        raise Blind("--ack-read n'accepte que %s" % " et ".join(ACKABLE_READ))
    return {"expect_head": args.expect_head, "merger_lane": args.merger_lane,
            "require_counter_review": args.require_counter_review, "ack_closes": closes,
            "ack_paths": args.ack_path, "ack_read": reads}


def _write_json(path, data):
    tmp = path + ".tmp"
    with open(tmp, "w", encoding="utf-8", newline="\n") as fh:
        json.dump(data, fh, ensure_ascii=False, indent=1)
    os.replace(tmp, path)


def main(argv=None, runner=gh_run, out=None, sleep=time.sleep):
    out = out or sys.stdout
    args = build_parser().parse_args(argv)
    if args.self_test:
        return self_test(out)
    try:
        options = _options_from_args(args)
        if args.from_snapshot:
            origin = "instantané rejoué"
            try:
                with open(args.from_snapshot, encoding="utf-8") as fh:
                    snap = json.load(fh)
            except (OSError, ValueError) as exc:
                raise Blind("instantané illisible : %s" % exc)
            if args.pr is not None and isinstance(snap, dict) and snap.get("number") != args.pr:
                raise Blind("l'instantané porte la PR #%s, pas #%d" % (snap.get("number"), args.pr))
        else:
            if args.pr is None:
                raise Blind("numéro de PR absent")
            origin = "lecture en direct"
            snap = collect(args.pr, args.repo, runner=runner, sleep=sleep)
        if args.snapshot_out:
            _write_json(args.snapshot_out, snap)
        result = evaluate(snap, options)
    except Exception as exc:  # Blind, or anything unexpected: never a verdict, never 0
        reason = str(exc) if isinstance(exc, Blind) else "erreur interne %s : %s" % (type(exc).__name__, exc)
        if args.json:
            out.write(json.dumps({"verdict": "AVEUGLE", "exit_code": EXIT_BLIND, "error": reason},
                                 ensure_ascii=False) + "\n")
        else:
            out.write("INSTRUMENT AVEUGLE (code de sortie %d) : %s\n" % (EXIT_BLIND, reason))
            out.write("Aucun verdict n'est rendu : une lecture en échec n'est ni un PASS ni un "
                      "\"rien à signaler\".\n")
        return EXIT_BLIND
    if args.json:
        payload = dict(result, pr=snap["pr"]["number"], repo=snap.get("repo"), origin=origin)
        out.write(json.dumps(payload, ensure_ascii=False, indent=2) + "\n")
    else:
        out.write(render(snap, result, origin))
    return result["exit_code"]


# --------------------------------------------------------------------------
# Offline self-test: fixtures
# --------------------------------------------------------------------------

_FX_HEAD = "a1b2c3d4" * 5
_FX_ATTRIB = "\U0001F916 Generated with [Claude Code](https://claude.com/claude-code)"
_FX_AFTER, _FX_BEFORE = "2026-01-01T12:00:00Z", "2025-12-31T12:00:00Z"
# Secret-shaped strings are assembled at run time: none exists literally in this file.
_FX_HARD = {
    "jeton GitHub": "gh" + "p_" + "a1B2" * 9,
    "jeton GitHub à portée fine": "github" + "_pat_" + "a1B2" * 6,
    "clé de forme sk": "sk" + "-" + "a1B2" * 6,
    "identifiant de clé AWS": "AK" + "IA" + "A1B2" * 4,
    "jeton Slack": "xox" + "b-" + "a1b2" * 4,
    "clé privée PEM": "-----BEGIN " + "RSA PRIVATE " + "KEY-----",
}
_FX_SOFT = {
    "affectation d'une valeur à un nom de secret": "pass" + "word" + ' = "' + "x9" * 8 + '"',
    "chemin de profil machine": "C:" + "\\Users\\" + "someone\\notes.txt",
    "mention du dossier de clés": "see ." + "keys" + "/deploy.txt",
}
_FX_SOFT_MORE = ("api" + "_key: " + "abcd1234efgh5678", "/ho" + "me/someone/x", "/Us" + "ers/someone/x",
                 "c:" + "\\users\\" + "someone\\notes.txt", "see ." + "KEYS" + "/deploy.txt")
_FX_NOTE = ("D:" + "\\data\\file.txt", "host 10.1." + "2.3 here")
_FX_PROSE = ("le secret des mots", "mot de passe oublié", "pass" + "word: voir le gestionnaire",
             "GH_" + "TOKEN=$(gh auth status)", "mcp__sk" + "-agent__run_conversation_with_tools_v2",
             "secret = compute_secret_from_vault(name)", "version 1.43.0 of the tool",
             "decimal path 2.3.2.1 then 1.1.10.12 in the taxonomy")


def _fx_snapshot():
    user = {"login": "owner", "type": "User"}
    checks = [{"name": n, "status": "completed", "conclusion": "success", "app": {"slug": "github-actions"}}
              for n in FALLBACK_CONTEXTS]
    checks.append({"name": "External Security Checks", "status": "completed", "conclusion": "success",
                   "app": {"slug": "external"}})
    snap = {
        "schema": SCHEMA, "repo": DEFAULT_REPO, "number": 4242, "collected_at": "2026-01-02T03:04:05Z",
        "pr": {"number": 4242, "title": "feat(x): sample", "body": "Objet de la PR.\n\n*po-2024*\n",
               "state": "open", "draft": False, "merged": False, "mergeable": True,
               "mergeable_state": "clean", "comments": 0, "review_comments": 0, "commits": 1,
               "additions": 0, "deletions": 0, "changed_files": 0, "html_url": "",
               "user": dict(user), "head": {"sha": _FX_HEAD, "ref": "feat/x"}, "base": {"ref": "master"}},
        "head_commit": {"sha": _FX_HEAD, "date": "2026-01-01T10:00:00Z"},
        "issue_comments": [], "inline_comments": [], "threads": [],
        "reviews": [{"id": 1, "user": dict(user), "state": "COMMENTED", "submitted_at": "2026-01-01T11:00:00Z",
                     "commit_id": _FX_HEAD, "body": "Relu.\n\n*po-2023*"}],
        "files": [{"filename": "tools/sample.py", "previous_filename": None, "status": "modified",
                   "additions": 2, "deletions": 0,
                   "patch": "@@ -1,2 +1,4 @@\n line one\n+added alpha\n+added beta\n line two"}],
        "commits": [{"sha": _FX_HEAD, "message": "feat(x): sample"}],
        "checks": checks, "statuses": [],
        "required_contexts": {"source": "auto-test", "contexts": list(FALLBACK_CONTEXTS)},
        "csv_taxonomy_other_prs": None,
    }
    return _fx_sync(snap)


def _fx_sync(snap):
    """Rebuild the unified diff and the PR counters from the file list."""
    parts = []
    for f in snap["files"]:
        if f.get("patch"):
            name = f["filename"]
            parts.append("diff --git a/%s b/%s\nindex 1111111..2222222 100644\n--- a/%s\n+++ b/%s\n%s\n"
                         % (name, name, name, name, f["patch"]))
    snap["diff_text"], snap["diff_error"] = "".join(parts), None
    snap["pr"]["changed_files"] = len(snap["files"])
    snap["pr"]["additions"] = sum(f["additions"] for f in snap["files"])
    return snap


def _fx_add_file(snap, path, lines):
    snap["files"].append({"filename": path, "previous_filename": None, "status": "added",
                          "additions": len(lines), "deletions": 0,
                          "patch": "@@ -0,0 +1,%d @@\n%s" % (len(lines), "\n".join("+" + ln for ln in lines))})
    return _fx_sync(snap)


def _fx_comment(snap, body, when, login="owner"):
    snap["issue_comments"].append({"id": len(snap["issue_comments"]) + 1, "created_at": when, "body": body,
                                   "user": {"login": login, "type": "User"}})
    snap["pr"]["comments"] = len(snap["issue_comments"])


def _fx_review(state, when=_FX_AFTER, body="", login="reviewer", commit=_FX_HEAD):
    return {"id": 0, "user": {"login": login, "type": "User"}, "state": state, "submitted_at": when,
            "commit_id": commit, "body": body}


def _fx_runner(snap, fail=None, short_files=False, tamper=None, unknown_reads=0):
    """Fake `gh`: serves a snapshot back through the API shapes collect() reads.

    `fail` makes one endpoint exit non-zero. `tamper` makes one endpoint ANSWER
    (exit 0, well-formed) while announcing one item more than it serves, or, for
    "graphql-errors", while carrying an `errors` envelope next to complete data --
    the partial reads a guard on the return code alone would wave through.
    `unknown_reads` serves that many first PR reads with an uncomputed merge state.
    """
    pr, head, num = snap["pr"], snap["pr"]["head"]["sha"], snap["pr"]["number"]
    prefix = "repos/%s/" % snap["repo"]
    seen = {"pr_reads": 0}

    def more(what):
        return 1 if tamper == what else 0

    def run(args):
        if fail and fail in " ".join(args):
            return 1, "", "HTTP 500: simulated failure"
        if "graphql" in args:
            block = {"totalCount": len(snap["threads"]) + more("threads"), "nodes": snap["threads"],
                     "pageInfo": {"hasNextPage": False, "endCursor": None}}
            reply = {"data": {"repository": {"pullRequest": {"reviewThreads": block}}}}
            if tamper == "graphql-errors":
                reply["errors"] = [{"message": "simulated partial failure"}]
            return 0, json.dumps(reply), ""
        path, _, query = args[-1].partition("?")
        later = "page=" in query and "page=1" not in query.split("&")
        tail = path[len(prefix):] if path.startswith(prefix) else None

        def listing(items):
            return 0, json.dumps([] if later else items), ""

        if tail == "pulls/%d" % num:
            if any("diff" in a for a in args[:-1]):
                return 0, snap["diff_text"], ""
            seen["pr_reads"] += 1
            if seen["pr_reads"] <= unknown_reads:
                return 0, json.dumps(dict(pr, mergeable_state="unknown", mergeable=None)), ""
            return 0, json.dumps(pr), ""
        if tail == "issues/%d/comments" % num:
            return listing(snap["issue_comments"])
        if tail == "pulls/%d/reviews" % num:
            return listing(snap["reviews"])
        if tail == "pulls/%d/comments" % num:
            return listing(snap["inline_comments"])
        if tail == "pulls/%d/files" % num:
            return listing(snap["files"][:-1] if short_files else snap["files"])
        if tail == "pulls/%d/commits" % num:
            return listing([{"sha": c["sha"], "commit": {"message": c["message"]}} for c in snap["commits"]])
        if tail == "commits/%s/check-runs" % head:
            return 0, json.dumps({"total_count": len(snap["checks"]) + more("check-runs"),
                                  "check_runs": [] if later else snap["checks"]}), ""
        if tail == "commits/%s/status" % head:
            return 0, json.dumps({"state": "pending", "total_count": len(snap["statuses"]) + more("statuses"),
                                  "statuses": snap["statuses"]}), ""
        if tail == "commits/%s" % head:
            return 0, json.dumps({"sha": head, "commit": {"committer": {"date": snap["head_commit"]["date"]}}}), ""
        if tail == "rules/branches/master":
            ctxs = [{"context": c} for c in snap["required_contexts"]["contexts"]]
            return 0, json.dumps([{"type": "pull_request"}, {"type": "required_status_checks",
                                  "parameters": {"required_status_checks": ctxs}}]), ""
        if tail == "pulls":
            return listing([{"number": num, "user": {"login": "owner"}, "title": "self"}])
        return 1, "", "HTTP 404: unrouted %s" % path

    run.seen = seen
    return run


# --------------------------------------------------------------------------
# Offline self-test: cases
# --------------------------------------------------------------------------

def self_test(out):
    results = []
    base = _fx_snapshot()

    def check(label, cond):
        results.append((bool(cond), label))

    def level_of(res, rid):
        return next((f["level"] for f in res["findings"] if f["rule"] == rid), None)

    def nop(s):
        return None

    def case(label, rid, verdict, mutate, opts=None):
        """Defect injected -> expected verdict carried by `rid`; rule disabled -> PASS again."""
        snap = copy.deepcopy(base)
        mutate(snap)
        res = evaluate(snap, opts)
        check("%s %s -> %s" % (rid, label, verdict), res["verdict"] == verdict and level_of(res, rid) == verdict
              and res["merge_command"] is None)
        again = evaluate(snap, opts, disabled=frozenset([rid]))
        check("%s %s : règle désactivée -> PASS (mutation falsifiante)" % (rid, label),
              again["verdict"] == "PASS" and again["merge_command"] is None)
        return snap, res

    def control(label, mutate, opts=None, rid=None, level=None):
        """Inverse control: the gate must NOT fire."""
        snap = copy.deepcopy(base)
        mutate(snap)
        res = evaluate(snap, opts)
        check("contrôle : %s -> PASS%s" % (label, " (%s %s)" % (rid, level) if rid else ""),
              res["verdict"] == "PASS" and (rid is None or level_of(res, rid) == level)
              and bool(res["merge_command"]))
        return snap, res

    def blind(label, mutate):
        snap = copy.deepcopy(base)
        mutate(snap)
        try:
            evaluate(snap)
            check("aveugle : %s" % label, False)
        except Blind:
            check("aveugle : %s" % label, True)

    def run_main(argv, runner):
        buf = io.StringIO()
        return main(argv, runner=runner, out=buf, sleep=lambda _s: None), buf.getvalue()

    # --- baseline
    res = evaluate(base)
    check("base -> PASS", res["verdict"] == "PASS" and res["exit_code"] == 0)
    text = render(base, res)
    check("base : la commande épingle la tête complète", ("--match-head-commit " + _FX_HEAD) in text)
    check("codes de sortie 0/3/2/1", EXIT_CODES == {"PASS": 0, "HOLD": 3, "BLOCK": 2} and EXIT_BLIND == 1)

    # --- R1 / R2 / R3
    case("PR fermée", "R1", "BLOCK", lambda s: s["pr"].update(state="closed", merged=True))
    case("brouillon", "R1", "BLOCK", lambda s: s["pr"].update(draft=True))
    case("base autre que master", "R1", "BLOCK", lambda s: s["pr"]["base"].update(ref="develop"))
    case("tête différente", "R2", "BLOCK", nop, {"expect_head": "f" * 40})
    case("préfixe trop court", "R2", "BLOCK", nop, {"expect_head": _FX_HEAD[:5]})
    control("tête conforme", nop, {"expect_head": _FX_HEAD[:12]}, "R2", "OK")
    control("tête non épinglée", nop, None, "R2", "INFO")
    case("conflit (dirty)", "R3", "BLOCK", lambda s: s["pr"].update(mergeable_state="dirty", mergeable=False))
    case("fusion non calculée (unknown)", "R3", "HOLD",
         lambda s: s["pr"].update(mergeable_state="unknown", mergeable=None))
    control("behind", lambda s: s["pr"].update(mergeable_state="behind"), None, "R3", "INFO")
    case("mergeable=false sous un état clean", "R3", "BLOCK", lambda s: s["pr"].update(mergeable=False))
    case("blocked sans cause vue par R4, R5 ni R6", "R3", "HOLD",
         lambda s: s["pr"].update(mergeable_state="blocked"))

    def blocked_by_ci(s):
        s["pr"]["mergeable_state"] = "blocked"
        s["checks"][0].update(status="in_progress", conclusion=None)
    snap = copy.deepcopy(base)
    blocked_by_ci(snap)
    res = evaluate(snap)
    check("R3 blocked, cause en vue dans R4 -> R3 INFO, R4 HOLD",
          level_of(res, "R3") == "INFO" and level_of(res, "R4") == "HOLD" and res["verdict"] == "HOLD")

    # --- R4
    case("check requis absent", "R4", "HOLD", lambda s: s["checks"].pop(0))
    case("check requis en cours", "R4", "HOLD",
         lambda s: s["checks"][0].update(status="in_progress", conclusion=None))
    case("check requis en échec", "R4", "BLOCK", lambda s: s["checks"][1].update(conclusion="failure"))
    case("check requis skipped", "R4", "HOLD", lambda s: s["checks"][2].update(conclusion="skipped"))
    case("check non requis en échec", "R4", "BLOCK",
         lambda s: s["checks"][-1].update(conclusion="action_required"))
    case("check non requis en cours", "R4", "HOLD",
         lambda s: s["checks"][-1].update(status="queued", conclusion=None))
    case("statut de commit en échec", "R4", "BLOCK",
         lambda s: s["statuses"].append({"context": "ext", "state": "failure"}))
    case("statut de commit pending", "R4", "HOLD",
         lambda s: s["statuses"].append({"context": "ext", "state": "pending"}))
    control("check non requis neutral", lambda s: s["checks"][-1].update(conclusion="neutral"), None, "R4", "OK")

    # --- R5
    cr = "CHANGES_REQUESTED"
    case("CHANGES_REQUESTED en vigueur", "R5", "BLOCK", lambda s: s["reviews"].append(_fx_review(cr)))
    case("CHANGES_REQUESTED puis COMMENTED du même auteur", "R5", "BLOCK", lambda s: s["reviews"].extend(
        [_fx_review(cr), _fx_review("COMMENTED", "2026-01-01T13:00:00Z")]))
    case("CHANGES_REQUESTED puis APPROVED d'un AUTRE auteur", "R5", "BLOCK", lambda s: s["reviews"].extend(
        [_fx_review(cr), _fx_review("APPROVED", "2026-01-01T13:00:00Z", login="someone-else")]))
    case("CHANGES_REQUESTED : --ack-read R5 ne lève pas", "R5", "BLOCK",
         lambda s: s["reviews"].append(_fx_review(cr)), {"ack_read": ["R5"]})
    control("CHANGES_REQUESTED puis APPROVED du même auteur", lambda s: s["reviews"].extend(
        [_fx_review(cr), _fx_review("APPROVED", "2026-01-01T13:00:00Z")]), None, "R5", "OK")
    control("CHANGES_REQUESTED puis DISMISSED", lambda s: s["reviews"].extend(
        [_fx_review(cr), _fx_review("DISMISSED", "2026-01-01T13:00:00Z")]), None, "R5", "OK")
    case("marqueur de prose après la tête", "R5", "HOLD",
         lambda s: _fx_comment(s, "Ne pas merger en l'état.", _FX_AFTER))
    case("ligne ouverte par le signe sens interdit, après la tête", "R5", "HOLD",
         lambda s: _fx_comment(s, "Constat :\n⛔ lot incomplet", _FX_AFTER))
    case("marqueur dans une revue rattachée à la tête", "R5", "HOLD", lambda s: s["reviews"].append(
        _fx_review("COMMENTED", _FX_BEFORE, "PR renvoyée : à corriger avant merge.", login="owner")))
    control("marqueur de prose avant la tête", lambda s: _fx_comment(s, "Do not merge yet.", _FX_BEFORE),
            None, "R5", "INFO")
    control("marqueur après la tête, acquitté", lambda s: _fx_comment(s, "Ne pas merger en l'état.", _FX_AFTER),
            {"ack_read": ["R5"]}, "R5", "INFO")
    control("prose neutre après la tête", lambda s: _fx_comment(s, "Relu, merge propre.", _FX_AFTER),
            None, "R5", "OK")

    # --- R6
    def thread(resolved, outdated):
        return lambda s: s["threads"].append({"isResolved": resolved, "isOutdated": outdated,
                                              "path": "tools/sample.py", "line": 3})
    case("fil non résolu actif", "R6", "BLOCK", thread(False, False))
    control("fil non résolu périmé", thread(False, True), None, "R6", "INFO")
    control("fil résolu", thread(True, False), None, "R6", "OK")

    # --- R7
    case("corps non signé", "R7", "HOLD", lambda s: s["pr"].update(body="Objet."))
    case("corps vide", "R7", "HOLD", lambda s: s["pr"].update(body=None))
    case("signature ailleurs qu'en dernière ligne", "R7", "HOLD",
         lambda s: s["pr"].update(body="*po-2024*\n\nObjet."))
    case("signature décorée, non littérale", "R7", "HOLD",
         lambda s: s["pr"].update(body="Objet.\n\n-- *po-2024 (worker lane) -- signale*"))
    case("lane inconnue", "R7", "HOLD", lambda s: s["pr"].update(body="Objet.\n\n*po-9999*"))
    control("attribution finale tolérée", lambda s: s["pr"].update(
        body="Objet.\r\n\r\n*po-2024*\r\n\r\n" + _FX_ATTRIB + "\r\nCo-Authored-By: X <x@example.invalid>\r\n"),
        None, "R7", "OK")
    control("lane cloud", lambda s: s["pr"].update(body="Objet.\n\n*cloud-codex-1*"), None, "R7", "OK")

    # --- R8
    def own(s):
        s["pr"]["body"] = "Objet.\n\n*ai-01*"

    def own_with(*reviews):
        def mutate(s):
            own(s)
            s["reviews"][:] = list(reviews)
        return mutate

    case("PR de la lane du merger, sans contre-revue", "R8", "HOLD", own_with())
    case("revue signée de la MÊME lane", "R8", "HOLD",
         own_with(_fx_review("COMMENTED", body="Relu.\n\n*ai-01*", login="owner")))
    case("contre-revue sur un commit antérieur", "R8", "HOLD",
         own_with(_fx_review("COMMENTED", body="Relu.\n\n*po-2023*", login="owner", commit="0" * 40)))
    case("compte robot non compté", "R8", "HOLD", own_with(_fx_review("COMMENTED", login="scanner[bot]")))
    case("--require-counter-review, aucune revue", "R8", "HOLD", lambda s: s["reviews"].clear(),
         {"require_counter_review": True})
    case("commentaire d'une autre lane ANTÉRIEUR à la tête", "R8", "HOLD",
         lambda s: (own_with()(s), _fx_comment(s, "Relu.\n\n*web2*", _FX_BEFORE)))
    control("lane du merger, contre-revue d'une autre lane", own, None, "R8", "OK")
    control("lane du merger, contre-revue d'un autre compte", own_with(_fx_review("COMMENTED")), None, "R8", "OK")
    control("lane du merger, commentaire d'une autre lane après la tête",
            lambda s: (own_with()(s), _fx_comment(s, "Relu.\n\n*web2*", _FX_AFTER)), None, "R8", "OK")
    control("autre lane, aucune revue, sans drapeau", lambda s: s["reviews"].clear(), None, "R8", "INFO")

    # --- R9
    def closes(s):
        s["pr"]["body"] = "Closes #12\n\n*po-2024*"
    case("mot de fermeture dans le corps", "R9", "HOLD", closes)
    case("acquittement d'un autre numéro", "R9", "HOLD", closes, {"ack_closes": [13]})
    case("mot de fermeture dans un message de commit", "R9", "HOLD",
         lambda s: s["commits"][0].update(message="feat: x\n\nfixes: #7"))
    case("mot de fermeture dans le titre", "R9", "HOLD", lambda s: s["pr"].update(title="Resolved #5"))
    issue_url = "https://github.com/%s/issues/12" % DEFAULT_REPO
    case("mot de fermeture suivi de l'URL de l'issue", "R9", "HOLD",
         lambda s: s["pr"].update(body="Closes %s\n\n*po-2024*" % issue_url))
    case("mot de fermeture, forme GH-N", "R9", "HOLD",
         lambda s: s["commits"][0].update(message="feat: x\n\nFixed GH-31"))
    snap, res = case("fermeture d'une issue d'un AUTRE dépôt, par URL", "R9", "HOLD", lambda s: s["pr"].update(
        body="Resolves https://github.com/other-org/other-repo/issues/7\n\n*po-2024*"))
    check("R9 autre dépôt : la cible est nommée avec son dépôt", "other-org/other-repo#7" in render(snap, res))
    control("fermeture acquittée", closes, {"ack_closes": [12]}, "R9", "INFO")
    control("URL d'issue citée sans mot de fermeture",
            lambda s: s["pr"].update(body="Voir %s et GH-12.\n\n*po-2024*" % issue_url), None, "R9", "OK")
    control("prose sans mot de fermeture",
            lambda s: s["pr"].update(body="Le suffix #12, voir #12, prefixes #3.\n\n*po-2024*"), None, "R9", "OK")

    # --- R10
    for name in sorted(_FX_HARD):
        value = _FX_HARD[name]
        snap, res = case("motif dur : %s" % name, "R10", "BLOCK",
                         lambda s, v=value: _fx_add_file(s, "docs/n.md", ["intro", "x " + v + " y"]))
        shown = render(snap, res)
        check("R10 %s : emplacement donné, valeur jamais réimprimée" % name,
              "docs/n.md:2" in shown and name in shown and value not in shown
              and value not in json.dumps(res, ensure_ascii=False))
        case("motif dur : %s : --ack-read R10 ne lève pas" % name, "R10", "BLOCK",
             lambda s, v=value: _fx_add_file(s, "docs/n.md", [v]), {"ack_read": ["R10"]})
    for name in sorted(_FX_SOFT):
        value = _FX_SOFT[name]
        snap, res = case("motif souple : %s" % name, "R10", "HOLD",
                         lambda s, v=value: _fx_add_file(s, "docs/n.md", [v]))
        check("R10 %s : nommé, valeur jamais réimprimée" % name,
              name in render(snap, res) and value not in render(snap, res))
        control("motif souple acquitté : %s" % name, lambda s, v=value: _fx_add_file(s, "docs/n.md", [v]),
                {"ack_read": ["R10"]}, "R10", "INFO")
    for value in _FX_SOFT_MORE:
        case("motif souple, variante %d" % _FX_SOFT_MORE.index(value), "R10", "HOLD",
             lambda s, v=value: _fx_add_file(s, "docs/n.md", [v]))
    control("chemin de lecteur et IPv4", lambda s: _fx_add_file(s, "docs/n.md", list(_FX_NOTE)), None, "R10", "INFO")
    control("prose et code qui ne doivent rien déclencher",
            lambda s: _fx_add_file(s, "docs/n.md", list(_FX_PROSE)), None, "R10", "OK")

    def removed_only(s):
        s["files"][0].update(additions=1, deletions=1,
                             patch="@@ -1,1 +1,1 @@\n-%s\n+clean line" % _FX_HARD["jeton GitHub"])
        _fx_sync(s)
    control("motif dur dans une ligne SUPPRIMÉE", removed_only, None, "R10", "OK")

    def header_lookalikes(s):
        _fx_add_file(s, "docs/n.md", ["++ b/elsewhere.md", "diff --git a/x b/y", _FX_HARD["jeton Slack"]])
    snap, res = case("ligne ajoutée imitant un en-tête de diff", "R10", "BLOCK", header_lookalikes)
    check("R10 imitation d'en-tête : numéro de ligne exact", "docs/n.md:3" in render(snap, res))

    def no_patch(s):
        s["files"][0]["patch"] = None
        s["diff_text"], s["diff_error"] = None, "HTTP 406"
    case("fichier sans patch (repli) : jamais PASS", "R10", "HOLD", no_patch)
    case("fichier sans patch : --ack-read R10 ne lève pas", "R10", "HOLD", no_patch, {"ack_read": ["R10"]})
    case("diff incomplet (lignes annoncées != lues)", "R10", "HOLD", lambda s: s["files"][0].update(additions=5))

    def fallback_ok(s):
        s["diff_text"], s["diff_error"] = None, "HTTP 406"
    control("repli sur les patchs, tous présents", fallback_ok, None, "R10", "INFO")
    check("diff : numéros de ligne", _parse_diff(base["diff_text"]) == {
        "tools/sample.py": [(2, "added alpha"), (3, "added beta")]})
    check("diff : chemin avec espaces", _diff_path("b/" + TAXONOMY_CSV + "\t") == TAXONOMY_CSV)
    check("diff : chemin cité", _diff_path('"b/caf\\303\\251 x.txt"') == "café x.txt")

    # --- R11
    def add(path):
        return lambda s: _fx_add_file(s, path, ["x"])
    case("référence imprimée (Archive)", "R11", "BLOCK", add("Cards/Fallacies/Archive/sub/ref.txt"))
    case("exemples CardPen", "R11", "BLOCK", add("Generation/CardPen/examples/a.json"))
    case("renommage hors d'Archive", "R11", "BLOCK",
         lambda s: s["files"][0].update(previous_filename="Cards/Rules/Archive/old.csv"))
    case("fichier d'environnement", "R11", "BLOCK", add("site/.env.local"))
    case("fichier d'environnement : --ack-path ne lève pas", "R11", "BLOCK", add(".env"), {"ack_paths": ["**"]})
    case("certificat pfx", "R11", "BLOCK", add("certs/site.PFX"))
    case("dossier de clés", "R11", "BLOCK", add("." + "keys" + "/a.txt"), {"ack_paths": ["**"]})
    case("workflow", "R11", "HOLD", add(".github/workflows/x.yml"))
    case("configuration d'agent", "R11", "HOLD", add(".claude/skills/x/SKILL.md"))
    case("point d'entrée des agents", "R11", "HOLD", add("AGENTS.md"))
    case("le gate lui-même", "R11", "HOLD", add("scripts/gates/x.py"))
    case("web.config imbriqué", "R11", "HOLD", add("site/app/web.config"))
    # Case: the repository tracks `Web.config` AND `web.config`; on Windows they are one file.
    case("Web.config, majuscule (forme réelle du dépôt)", "R11", "HOLD", add("DNNPlatform/Install/Web.config"))
    case("workflow, autre casse", "R11", "HOLD", add(".GitHub/Workflows/x.yml"))
    case("référence imprimée, autre casse", "R11", "BLOCK", add("cards/Fallacies/archive/ref.txt"))
    case("fichier d'environnement, autre casse", "R11", "BLOCK", add("site/.ENV"))
    case("dossier de clés, autre casse", "R11", "BLOCK", add("." + "KEYS" + "/a.txt"))
    case("acquittement à la mauvaise casse : ne lève pas", "R11", "HOLD", add("DNNPlatform/Install/Web.config"),
         {"ack_paths": ["**/web.config"]})
    case("le dossier de référence lui-même (lien, sous-module)", "R11", "BLOCK", add("Cards/Fallacies/Archive"))
    case(".envrc", "R11", "BLOCK", add("deploy/.envrc"), {"ack_paths": ["**"]})
    case("fichier d'environnement à tiret", "R11", "BLOCK", add(".env-prod"))
    case("modèle d'environnement (.env.example)", "R11", "HOLD", add(".env.example"))
    control("modèle d'environnement acquitté", add("site/.env.example"), {"ack_paths": ["site/.env.example"]},
            "R11", "INFO")
    control("référence imprimée acquittée", add("Cards/Fallacies/Archive/ref.txt"),
            {"ack_paths": ["Cards/*/Archive/**"]}, "R11", "INFO")
    control("workflow acquitté", add(".github/workflows/x.yml"), {"ack_paths": [".github/workflows/**"]},
            "R11", "INFO")
    control("chemins voisins non protégés",
            lambda s: (add("Cards/Fallacies/ArchiveNotes/x.txt")(s), add("docs/AGENTS.md")(s),
                       add("docs/environment.md")(s), add("docs/.environment.md")(s),
                       add("docs/web.config.md")(s)), None, "R11", "OK")

    # --- R12
    def taxonomy(others):
        def mutate(s):
            _fx_add_file(s, TAXONOMY_CSV, ["a,b"])
            s["csv_taxonomy_other_prs"] = others
        return mutate
    case("autre PR ouverte sur le CSV de taxonomie", "R12", "BLOCK", taxonomy([{"number": 99, "title": "t"}]))
    control("CSV de taxonomie, seule PR ouverte", taxonomy([]), None, "R12", "INFO")
    control("autre CSV sous Cards/", add("Cards/Rules/x.csv"), None, "R12", "INFO")

    # --- blind snapshots: never a verdict
    blind("instantané sans liste de fichiers", lambda s: s.pop("files"))
    blind("fichiers lus != annoncés", lambda s: s["pr"].update(changed_files=7))
    blind("commentaires lus != annoncés", lambda s: s["pr"].update(comments=3))
    blind("commit de tête d'un autre SHA", lambda s: s["head_commit"].update(sha="0" * 40))
    blind("contextes requis vides", lambda s: s["required_contexts"].update(contexts=[]))
    blind("schéma inconnu", lambda s: s.update(schema=99))
    blind("CSV de taxonomie touché, autres PR non lues", taxonomy(None))

    # --- collector, through a fake gh
    rc, text = run_main(["4242"], _fx_runner(base))
    check("collecte factice -> PASS, code 0", rc == 0 and "VERDICT : PASS" in text
          and ("--match-head-commit " + _FX_HEAD) in text)
    replay = json.loads(json.dumps(collect(4242, DEFAULT_REPO, runner=_fx_runner(base), sleep=lambda _s: None)))
    check("instantané sérialisable, même verdict rejoué", evaluate(replay)["verdict"] == "PASS")
    for needle in ("pulls/4242", "issues/4242/comments", "/reviews", "pulls/4242/comments", "/files",
                   "pulls/4242/commits", "check-runs", "/status", "commits/" + _FX_HEAD, "graphql"):
        rc, text = run_main(["4242"], _fx_runner(base, fail=needle))
        check("échec API simulé (%s) -> code 1, aucun verdict" % needle.replace(_FX_HEAD, "<tête>"),
              rc == 1 and "AVEUGLE" in text and "VERDICT" not in text and "gh pr merge" not in text)
    rc, text = run_main(["4242"], _fx_runner(base, short_files=True))
    check("pagination incomplète (fichiers) -> code 1", rc == 1 and "VERDICT" not in text)
    # An endpoint that ANSWERS (exit 0, well-formed) but serves less than it announces.
    # No count in the PR object backs these three up: the collector guard is the only one.
    for what in ("check-runs", "threads", "statuses"):
        rc, text = run_main(["4242"], _fx_runner(base, tamper=what))
        check("lecture partielle annoncée par le serveur (%s) -> code 1, aucun verdict" % what,
              rc == 1 and "AVEUGLE" in text and "VERDICT" not in text and "gh pr merge" not in text)
    rc, text = run_main(["4242"], _fx_runner(base, tamper="graphql-errors"))
    check("GraphQL : enveloppe d'erreurs à côté de données complètes -> code 1",
          rc == 1 and "AVEUGLE" in text and "VERDICT" not in text)
    many = copy.deepcopy(base)
    many["pr"]["commits"] = MAX_LISTED_COMMITS + 1
    rc, text = run_main(["4242"], _fx_runner(many))
    check("plus de commits que l'API n'en liste -> code 1, et la cause est dite",
          rc == 1 and ("n'en liste que %d" % MAX_LISTED_COMMITS) in text)
    lazy = _fx_runner(base, unknown_reads=2)
    rc, text = run_main(["4242"], lazy)
    check("état de fusion calculé à la 3e lecture -> PASS (relance bornée)",
          rc == 0 and lazy.seen["pr_reads"] == 3)
    never = _fx_runner(base, unknown_reads=99)
    rc, text = run_main(["4242"], never)
    check("état de fusion jamais calculé -> HOLD après 3 relances, pas de boucle",
          rc == 3 and never.seen["pr_reads"] == 4 and "R3   HOLD" in text)
    rc, text = run_main(["4242"], _fx_runner(base, fail="rules/branches"))
    check("ruleset illisible -> repli nommé, dit dans la sortie", rc == 0 and "REPLI NOMMÉ" in text)
    rc, text = run_main(["4242"], _fx_runner(base, fail="v3.diff"))
    check("diff refusé, patchs présents -> repli dit dans la sortie", rc == 0 and "REPLI sur les patchs" in text)
    blinded = copy.deepcopy(base)
    blinded["files"][0]["patch"] = None
    rc, text = run_main(["4242"], _fx_runner(blinded, fail="v3.diff"))
    check("diff refusé, fichier sans patch -> HOLD, code 3, aucune commande", rc == 3 and "gh pr merge" not in text)
    with_tax = copy.deepcopy(base)
    taxonomy(None)(with_tax)
    rc, text = run_main(["4242"], _fx_runner(with_tax))
    check("CSV de taxonomie : PR ouvertes lues par la collecte", rc == 0 and "R12  INFO" in text)
    rc, text = run_main(["4242"], _fx_runner(with_tax, fail="state=open"))
    check("CSV de taxonomie : lecture des PR ouvertes en échec -> code 1", rc == 1)
    rc, text = run_main(["4242", "--json"], _fx_runner(base, fail="/reviews"))
    check("échec en sortie JSON -> code 1, verdict AVEUGLE", rc == 1 and json.loads(text)["verdict"] == "AVEUGLE")
    rc, text = run_main(["--from-snapshot", os.path.join("no-such-dir", "no-such-snapshot.json")], _fx_runner(base))
    check("instantané illisible -> code 1", rc == 1 and "AVEUGLE" in text)
    rc, text = run_main(["4242", "--ack-read", "R4"], _fx_runner(base))
    check("--ack-read hors R5/R10 -> code 1", rc == 1)
    rc, text = run_main(["4242", "--ack-closes", "douze"], _fx_runner(base))
    check("--ack-closes non numérique -> code 1, argument nommé", rc == 1 and "--ack-closes attend" in text)
    fd, replay_path = tempfile.mkstemp(suffix=".json", prefix="pr-merge-gate-selftest-")
    os.close(fd)
    try:
        _write_json(replay_path, base)
        rc, text = run_main(["--from-snapshot", replay_path], _fx_runner(base, fail="repos/"))
        check("instantané rejoué sans réseau -> même verdict", rc == 0 and "instantané rejoué" in text)
        rc, text = run_main(["4243", "--from-snapshot", replay_path], _fx_runner(base, fail="repos/"))
        check("instantané d'une AUTRE PR que celle demandée -> code 1", rc == 1 and "#4242" in text)
    finally:
        os.remove(replay_path)
    check("commande de merge : dépôt nommé seulement s'il n'est pas le dépôt par défaut",
          " -R " not in merge_command(base) and " -R other/repo " in merge_command(dict(base, repo="other/repo")))
    blind("tête abrégée dans l'instantané", lambda s: (s["pr"]["head"].update(sha=_FX_HEAD[:7]),
                                                       s["head_commit"].update(sha=_FX_HEAD[:7])))
    check("diff : fichier supprimé, aucune ligne attribuée", _diff_path("/dev/null") is None and _parse_diff(
        "diff --git a/gone.txt b/gone.txt\n--- a/gone.txt\n+++ /dev/null\n@@ -1,1 +0,0 @@\n-gone\n") == {})
    check("limite déclarée dans la sortie : signature de lane déclarative",
          "déclarative" in render(base, evaluate(base)))
    hold_snap, hold_res = case("sortie HOLD", "R7", "HOLD", lambda s: s["pr"].update(body="Objet."))
    check("sortie non PASS : aucune commande de merge", "gh pr merge" not in render(hold_snap, hold_res))
    check("globs", _glob_any("web.config", ["**/web.config"]) and _glob_any("a/b/web.config", ["**/web.config"])
          and not _glob_any("Cards/Fallacies/Archivex/a", ARCHIVE_GLOBS)
          and not _glob_any("x/Cards/Fallacies/Archive/a", ARCHIVE_GLOBS))

    failed = [label for ok, label in results if not ok]
    for ok, label in results:
        out.write("%s  %s\n" % ("ok   " if ok else "ECHEC", label))
    out.write("auto-test : %d/%d\n" % (len(results) - len(failed), len(results)))
    return 0 if not failed and results else 1


if __name__ == "__main__":
    for _stream in (sys.stdout, sys.stderr):
        try:
            _stream.reconfigure(encoding="utf-8", errors="replace")
        except (AttributeError, ValueError):
            pass
    sys.exit(main())
