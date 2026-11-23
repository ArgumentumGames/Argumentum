"""Locate cascade drift in a translation corpus: the French cell was tightened,
the translations were left on the longer wording.

The defect was measured on Virtues (pass 3): 23 of 24 kept cells were exactly
this shape. It is *measurable* rather than a matter of taste, so it needs a
detector instead of a "least good cell" heuristic — and the detector must not
confuse two very different things:

  * CASCADE DRIFT — the FR cell HAD that content in an earlier commit and lost it;
    the translation kept it. The fix is to follow the current French.
  * LEGITIMATE LOCALIZATION — the target adds an explanation the French does not
    need (a pun that dies in translation: "avocat" = lawyer/avocado). The
    translation is *longer by design*; shortening it destroys the card.

Only the first is in scope here, and git history is what tells them apart: a
translation can only be stale with respect to content the source once carried.

A third shape was measured on 2026-11-23 (report §3, PK 1282): the French cell can
make a ROUND TRIP — A -> A' -> A. The translation aligned on A is then correct,
yet the return to A looks like a tightening when the scan reaches that commit. The
rule that tells the two apart is historical: the candidate is a round trip when
`fr_new` already appeared BEFORE `fr_old`. Those are dropped by default
(`--keep-round-trips` reproduces the pre-2026-11-23 behaviour: 45 candidates where
26 are actionable).

Usage
  python -I tools/458-cascade-drift-scan.py --csv "Cards/Fallacies/..." --out worklist.csv
"""
import argparse
import csv
import importlib.util
import io
import subprocess
import sys

LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
FIELDS = ["text", "desc", "example"]
# A "tightening" is a loss the translation could carry. Below this, the FR change
# is a typo fix or a comma; treating it as drift would swamp the worklist.
MIN_FR_SHRINK = 12


def git(*args):
    p = subprocess.run(["git"] + list(args), capture_output=True)
    if p.returncode != 0:
        raise SystemExit("git %s failed: %s" % (" ".join(args), p.stderr.decode("utf-8", "replace")[:300]))
    return p.stdout


def load_module(path="tools/458-pass-fix-cells.py"):
    """Load the writer tool for its `key_of`.

    The row-key resolution lived as two identical copies (scan here, writer in
    the pass tool) — the #1954 review flagged the drift risk of two copies with
    no equality assertion, so the definition lives once in the pass tool and
    every consumer loads it (the counter-review `458-counterreview-cells.py`
    already did). Fallback semantics and error messages are therefore shared
    by construction, not by discipline.
    """
    spec = importlib.util.spec_from_file_location("pf", path)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def parse_at(rev, path, keycol):
    raw = git("show", "%s:%s" % (rev, path))
    if raw[:3] == b"\xef\xbb\xbf":
        raw = raw[3:]
    return {r[keycol]: r for r in csv.DictReader(io.StringIO(raw.decode("utf-8"), newline=""))}


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--csv", required=True)
    ap.add_argument("--langs", default=",".join(LANGS))
    ap.add_argument("--fields", default=",".join(FIELDS))
    ap.add_argument("--min-shrink", type=int, default=MIN_FR_SHRINK)
    ap.add_argument("--keep-round-trips", action="store_true",
                    help="keep candidates whose fr_new wording already existed before "
                         "fr_old (the FR came back to an earlier value: the translation "
                         "is aligned). Default: dropped, per report §3 of P24-07.")
    ap.add_argument("--cards-only", action="store_true",
                    help="keep rows the deck actually prints (deck column resolved)")
    ap.add_argument("--key-column", default=None,
                    help="row key column (default: resolved among PK/pk/Id/id)")
    ap.add_argument("--deck-column", default=None,
                    help="deck membership column (default: 'carte', else 'card')")
    ap.add_argument("--out", default=None)
    a = ap.parse_args()

    langs = [x.strip() for x in a.langs.split(",") if x.strip()]
    fields = [x.strip() for x in a.fields.split(",") if x.strip()]
    head_header = next(csv.reader(io.StringIO(
        git("show", "HEAD:%s" % a.csv).decode("utf-8-sig").split("\r\n")[0])))
    keycol = load_module().key_of(head_header, a.key_column)
    # Fallacies marks deck membership `carte`, Virtues `card` (measured 23/10).
    # A hardcoded name silently filtered nothing on the wrong corpus.
    deckcol = a.deck_column
    if a.cards_only and not deckcol:
        deckcol = next((c for c in ("carte", "card") if c in head_header), None)
        if not deckcol:
            raise SystemExit("--cards-only: no deck column found (pass --deck-column)")
    print("row key: %r%s" % (keycol, " | deck column: %r" % deckcol if deckcol else ""))
    shas = git("log", "--no-merges", "--format=%H", "--", a.csv).decode().split()
    print("commits touching the corpus (no merges): %d" % len(shas))

    hits = {}
    scanned = 0
    # Chronological FR history, recorded only where the cell CHANGES (rare), plus the
    # value each pair starts with. It is what tells a tightening apart from a round
    # trip without re-walking the corpus. The rank is a step counter, not the commit
    # index: at one commit the value can both start and change, and the starting value
    # is older — a shared index would make the two ranks equal and hide the return.
    fr_changes = {}  # (pk, base) -> [(step, value)]
    step = 0
    for sha in reversed(shas):
        try:
            old = parse_at(sha + "^", a.csv, keycol)
            new = parse_at(sha, a.csv, keycol)
        except SystemExit:
            continue  # root commit, or the file did not exist yet
        scanned += 1
        for pk, nr in new.items():
            orow = old.get(pk)
            if not orow or (deckcol and not (nr.get(deckcol) or "").strip()):
                continue
            for base in fields:
                frc = "%s_fr" % base
                fr_old, fr_new = (orow.get(frc) or ""), (nr.get(frc) or "")
                seq = fr_changes.get((pk, base))
                if seq is None:
                    seq = fr_changes[(pk, base)] = [(step, fr_old)]
                    step += 1
                if fr_old != fr_new and seq[-1][1] != fr_new:
                    seq.append((step, fr_new))
                    step += 1
                if len(fr_old) - len(fr_new) < a.min_shrink:
                    continue
                for lang in langs:
                    col = "%s_%s" % (base, lang)
                    if col not in nr:
                        continue
                    t_old, t_new = (orow.get(col) or ""), (nr.get(col) or "")
                    if not t_new.strip() or t_old != t_new:
                        continue  # translation was updated with the FR, or empty
                    key = (pk, col)
                    # Keep the tightest evidence: the largest FR loss for that cell.
                    loss = len(fr_old) - len(fr_new)
                    cur = hits.get(key)
                    if cur is None or loss > cur["loss"]:
                        hits[key] = dict(pk=pk, col=col, field=base, lang=lang, sha=sha[:8],
                                         loss=loss, fr_old=fr_old, fr_new=fr_new,
                                         target=t_new)
    print("commits scanned: %d | cascade-drift candidates (as of their commit): %d"
          % (scanned, len(hits)))
    # A candidate is only actionable if the two cells still hold at HEAD: the scan
    # records the values AT THE COMMIT, and a later pass may already have repaired
    # the translation (measured 23/10: 57 of 69 proposals were stale this way).
    head = parse_at("HEAD", a.csv, keycol)
    stale = 0
    for key in list(hits):
        pk, col = key
        base = hits[key]["field"]
        hrow = head.get(pk)
        if hrow is None:
            continue
        if (hrow.get(col) or "") != hits[key]["target"] or (hrow.get("%s_fr" % base) or "") != hits[key]["fr_new"]:
            stale += 1
            del hits[key]
    print("dropped as already repaired since: %d | actionable at HEAD: %d" % (stale, len(hits)))
    # Round trips (report §3): the FR returned to a wording it already carried, so the
    # translation that stayed put is aligned with the CURRENT cell, not stale. Rank by
    # first appearance; when fr_new predates fr_old, the change is a return.
    roundtrips = 0
    rt_groups = []
    if not a.keep_round_trips:
        for key in list(hits):
            h = hits[key]
            first = {}
            for i, v in fr_changes.get((h["pk"], h["field"]), []):
                first.setdefault(v, i)
            if first.get(h["fr_new"], 1 << 30) < first.get(h["fr_old"], 1 << 30):
                roundtrips += 1
                g = (h["pk"], h["field"])
                if g not in rt_groups:
                    rt_groups.append(g)
                del hits[key]
    print("dropped as FR round-trips: %d%s | actionable: %d"
          % (roundtrips,
             " (groups: %s)" % ", ".join("%s %s" % g for g in sorted(rt_groups)) if rt_groups else "",
             len(hits)))
    rows = sorted(hits.values(), key=lambda h: (-h["loss"], h["pk"]))
    by_field = {}
    for h in rows:
        by_field.setdefault(h["field"], 0)
        by_field[h["field"]] += 1
    print("by field: %s" % by_field)
    by_lang = {}
    for h in rows:
        by_lang.setdefault(h["lang"], 0)
        by_lang[h["lang"]] += 1
    print("by language: %s" % by_lang)
    for h in rows[:10]:
        print("  PK %-5s %-12s loss=%3d  FR now: %r" % (h["pk"], h["col"], h["loss"], h["fr_new"][:60]))
    if a.out:
        with io.open(a.out, "w", encoding="utf-8", newline="") as f:
            w = csv.DictWriter(f, fieldnames=["pk", "col", "field", "lang", "sha", "loss",
                                              "fr_old", "fr_new", "target"], lineterminator="\n")
            w.writeheader()
            w.writerows(rows)
        print("written", a.out)
    return 0


if __name__ == "__main__":
    sys.exit(main())
