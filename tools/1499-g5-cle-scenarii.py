#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#1499 grain G5 — build and VALIDATE a join key for the Scenarii corpus.

Why this exists (source of the requirement, measured 2026-09-22 by ai-01 on #1499):
  Join Scenarii 2022-archive against the current corpus on `path` and the instrument
  reports divergences that DO NOT EXIST: `path` was RE-ATTRIBUTED (Napoleon did not
  get rewritten — it MOVED from 1.3.1 to 1.2.3). The candidate artifact carrying
  "308 changed cells" was never published for that reason.
  ⭐ "Un instrument de comparaison doit prouver qu'il compare les MEMES OBJETS avant de
  rapporter qu'ils diffèrent."

The delivered key (two stages, second stage gated by an independent control):
  E1  normalized title  — casefold + NFKD accent fold + apostrophe unification +
      punctuation stripped. Recovers a card wherever it MOVED.
  E2  `path` AND content-similarity >= the q99 noise floor — recovers a card that was
      RENAMED. Without the control, E2 is exactly the join that was shown to be false.

The independent control (what makes E1/E2 falsifiable rather than asserted):
  Content similarity = Jaccard over the word sets of (contexte + enjeu + suggestion).
  Its NOISE FLOOR is calibrated EXHAUSTIVELY — every one of the 77x167 pairs, q99 —
  never from a sample. A correct pairing must sit above that floor; a re-keyed one
  cannot. This is deliberately a *content* control, not a taxonomy one:
  `catégorie` agrees 100% on the KNOWN-FALSE `path` join (all cards of a block share
  it), so it carries no discriminating power at all — a control must be able to
  return a 1 before its 0 means anything.

⛔ NO CHIFFRAGE. This instrument reports the KEY's coverage and its control verdicts.
   It never counts changed cells: that is grain G6, gated on this key being validated.

Usage:
    python tools/1499-g5-cle-scenarii.py                 # verdict (rc=0 pass / 2 fail)
    python tools/1499-g5-cle-scenarii.py --report        # full tables
    python tools/1499-g5-cle-scenarii.py --key path      # FALSIFYING MUTATION (must go red)
    python tools/1499-g5-cle-scenarii.py --corpus DIR    # alternate repo root
    python tools/1499-g5-cle-scenarii.py --self-test     # throwaway-fixture suite
"""

import argparse
import csv
import io
import os
import sys
import unicodedata

ARCHIVE_REL = os.path.join("Cards", "Scenarii", "Archive", "2022",
                           "Argumentum Scenarii - Cards fevrier 2022.csv")
CURRENT_REL = os.path.join("Cards", "Scenarii", "Argumentum Scenarii - Cards.csv")

# Columns the instrument cannot work without. A renamed/absent column must fail LOUD:
# a missing column reads as '' through DictReader, which silently yields 0 matches —
# the "a 0 is only an absence if the instrument could have returned a 1" trap.
REQUIRED = ("path", "titre", "contexte", "enjeu", "suggestion")

# Words carrying no discriminating signal in a French card template.
STOPWORDS = set(
    "le la les un une des du de d a au aux et ou en dans sur pour par ce cette ces "
    "son sa ses il elle on nous vous ils elles est sont que qui quoi dont y l s n "
    "plus moins tout tous toute toutes mais si comme aussi".split()
)


def load_csv(path):
    """Rows + header, decoding the BOM if present. Never silently drops a header."""
    with open(path, "rb") as fh:
        raw = fh.read()
    text = raw.decode("utf-8-sig")
    rows = list(csv.DictReader(io.StringIO(text)))
    header = list(rows[0].keys()) if rows else []
    return rows, header


def header_ok(header, where):
    missing = [c for c in REQUIRED if c not in header]
    if missing:
        print(f"FATAL [{where}]: colonnes absentes apres renommage ? {missing}", file=sys.stderr)
        print(f"       en-tete lue : {header}", file=sys.stderr)
        return False
    return True


def fold(text):
    text = (text or "").strip()
    for ch in ("’", "‘", "ʼ"):
        text = text.replace(ch, "'")
    text = " ".join(text.split()).casefold()
    return "".join(c for c in unicodedata.normalize("NFKD", text)
                   if not unicodedata.combining(c))


def key_title(row):
    """E1 — the validated key. Punctuation is stripped: 'Maréchal nous voilà' and
    'Maréchal, nous voilà' are the same card, and a comma is not a rename."""
    folded = fold(row.get("titre", ""))
    cleaned = "".join(c if (c.isalnum() or c == " ") else " " for c in folded)
    return " ".join(cleaned.split())


def key_path(row):
    """The key that was shown FALSE. Kept in the instrument so the control can be
    asked to condemn it on demand (falsifying mutation), never to be used for real."""
    return (row.get("path") or "").strip()


def content_tokens(row):
    joined = " ".join(row.get(c, "") or "" for c in ("contexte", "enjeu", "suggestion"))
    cleaned = "".join(c if (c.isalnum() or c == " ") else " " for c in fold(joined))
    return {w for w in cleaned.split() if len(w) > 2 and w not in STOPWORDS}


def jaccard(left, right):
    if not left or not right:
        return 0.0
    return len(left & right) / len(left | right)


def noise_floor(archive, current):
    """q99 over EVERY archive x current pair — never a sample. Lipschitz-free and
    cheap at this size (77 x 167); a sample would make the floor itself an estimate,
    and the whole point is that the floor is the falsifiable part."""
    scores = sorted(jaccard(content_tokens(a), content_tokens(c))
                    for a in archive for c in current)
    return scores[int(0.99 * len(scores))], len(scores)


def pairs_for(keyfn, archive, current):
    index = {}
    for row in current:
        index.setdefault(keyfn(row), row)
    return [(a, index[keyfn(a)]) for a in archive if keyfn(a) in index]


def judge(keyfn, archive, current, threshold):
    """A key passes when NO pair it produces falls at noise level. One sub-threshold
    pair is a pairing of two different cards and is enough to condemn the key."""
    pairs = pairs_for(keyfn, archive, current)
    scored = [(a, c, jaccard(content_tokens(a), content_tokens(c))) for a, c in pairs]
    below = [t for t in scored if t[2] < threshold]
    return scored, below


def stage2_recovery(archive, current, threshold):
    """E2 — cards the title key cannot see, recovered by `path` ONLY where the content
    control clears the floor STRICTLY: a pair sitting exactly ON the q99 floor is
    indistinguishable from the noise it was calibrated on (the self-test fixture
    produces exactly one such coincidence, which is how the rule was found). Each
    recovery carries its own score, so a weak one is visible as weak instead of being
    averaged into a total."""
    confident_bar = 2.0 * threshold
    idx_path = {}
    for row in current:
        idx_path.setdefault(key_path(row), row)
    idx_title = {}
    for row in current:
        idx_title.setdefault(key_title(row), row)
    recovered, unresolved = [], []
    for row in archive:
        if key_title(row) in idx_title:
            continue
        candidate = idx_path.get(key_path(row))
        score = jaccard(content_tokens(row), content_tokens(candidate)) if candidate else -1.0
        if score > threshold:
            recovered.append((row, candidate, score, "CONFORTABLE" if score >= confident_bar else "LIMITE"))
        else:
            unresolved.append((row, candidate, score))
    return recovered, unresolved


def analyse(corpus):
    archive, header_a = load_csv(os.path.join(corpus, ARCHIVE_REL))
    current, header_c = load_csv(os.path.join(corpus, CURRENT_REL))
    if not header_ok(header_a, "archive 2022") or not header_ok(header_c, "courant"):
        return None
    if not archive or not current:
        print("FATAL: corpus vide — l'instrument n'a rien a comparer.", file=sys.stderr)
        return None
    threshold, n_pairs = noise_floor(archive, current)
    e1, e1_below = judge(key_title, archive, current, threshold)
    e2_candidates, e2_below = judge(key_path, archive, current, threshold)
    recovered, unresolved = stage2_recovery(archive, current, threshold)
    return {
        "threshold": threshold, "floor_pairs": n_pairs,
        "archive_n": len(archive), "current_n": len(current),
        "e1": e1, "e1_below": e1_below,
        "path": e2_candidates, "path_below": e2_below,
        "recovered": recovered, "unresolved": unresolved,
    }


def report(result):
    total = result["archive_n"]
    e1_n = len(result["e1"])
    rec_n = len(result["recovered"])
    print(f"plancher de bruit (q99, {result['floor_pairs']} paires exhaustives) = {result['threshold']:.3f}")
    print(f"archive 2022 : {total} cartes  |  courant : {result['current_n']} cartes")
    print()
    print("=== VERDICT DU CONTROLE (une paire sous le plancher condamne la cle) ===")
    for label, scored, below in (("E1 TITRE normalise", result["e1"], result["e1_below"]),
                                 ("PATH (cle connue fausse)", result["path"], result["path_below"])):
        lo = min((t[2] for t in scored), default=float("nan"))
        print(f"  {label:<28} n={len(scored):>3}  min={lo:.3f}  sous le plancher: {len(below):>2}")
        for a, c, score in sorted(below, key=lambda t: t[2])[:15]:
            print(f"        {score:.3f}  {key_path(a):<7} {a['titre'][:34]!r:<38} -> {key_path(c):<7} {c['titre'][:34]!r}")
    print()
    print("=== COUVERTURE DE LA CLE ===")
    print(f"  E1 titre normalise                                  : {e1_n:>3}/{total}")
    print(f"  E2 path + controle > seuil (renommages recuperes)   : {rec_n:>3}")
    print(f"  NON RESOLUES (la cle seule ne decide pas)           : {len(result['unresolved']):>3}")
    print(f"  COUVERTURE                                          : {e1_n + rec_n:>3}/{total} = {100 * (e1_n + rec_n) / total:.1f}%")
    print()
    print("  E2 du plus faible au plus fort (CONFORTABLE = >= 2x le plancher) :")
    for a, c, score, label in sorted(result["recovered"], key=lambda t: t[2]):
        print(f"    {score:.3f} {label:<11} {key_path(a):<7} {a['titre'][:34]!r:<38} -> {c['titre'][:34]!r}")
    print()
    print("  NON RESOLUES, nommes :")
    for a, c, score in result["unresolved"]:
        shown = repr(c["titre"][:32]) if c else "AUCUN au meme path"
        print(f"    {key_path(a):<7} {a['titre'][:40]!r:<44} path courant: {shown}")


def verdict(result, key_under_test):
    """rc=0 only when the key under test passes AND the control is shown to
    discriminate (it must still condemn `path`). A control that passes everything
    is not a control."""
    ok_key = not (result["e1_below"] if key_under_test == "title" else result["path_below"])
    discriminates = bool(result["path_below"])
    print(f"cle testee = {key_under_test}")
    print(f"  passe le controle                : {'OUI' if ok_key else 'NON'}")
    print(f"  le controle discrimine (path KO) : {'OUI' if discriminates else 'NON'}")
    if ok_key and discriminates:
        print("VERDICT: OK")
        return 0
    print("VERDICT: ECHEC — la cle ne tient pas le controle, ou le controle ne discrimine plus.")
    return 2


def build_fixture(tmp):
    """Deterministic corpus, sized so the q99 floor lands in the NOISE and not among the
    true pairs — a 16-pair fixture would put q99 ON a true pair (similarity 1.0), raise
    the bar to 1.0 and condemn every key: the fixture lying, not the key.

    Three true pairs, each exercising one branch:
      T1 same title, same path      -> E1 (the ordinary case)
      T2 same title, MOVED path     -> E1 must still see it (this is what `path` misses)
      T3 RENAMED title, same path   -> E2, and only because the control clears the floor
    Every other archive path is absent from the current corpus (no pair at all), except
    T2's, which is occupied by a DIFFERENT card on purpose: that single poisoned pair is
    the one the control must condemn."""
    import random
    rng = random.Random(20260925)
    pool = [f"mot{i:03d}" for i in range(160)]

    def text():
        return " ".join(rng.sample(pool, 8))

    n_arch, n_cur = 24, 30
    archive, current = [], []
    for i in range(n_arch):
        archive.append({"path": f"1.{i + 1:02d}", "titre": f"Titre unique {i:02d}",
                        "contexte": text(), "enjeu": "", "suggestion": ""})
    # The current corpus holds three of the archive cards, at paths 1.01 / 1.02 / 1.03.
    archive[0]["path"], archive[1]["path"], archive[2]["path"] = "1.01", "1.02", "1.03"
    archive[0]["titre"] = "Titre garde"
    archive[2]["titre"] = "Titre renomme"
    current.append(dict(archive[0], path="1.01"))          # T1 — same title, same path
    current.append(dict(archive[1], path="1.20"))          # T2 — same title, MOVED
    current.append({**archive[2], "path": "1.03",
                    "titre": "Titre renomme actuel"})      # T3 — renamed, same path
    current.append({"path": "1.02", "titre": "Occupant sans rapport",        # poisons T2's
                    "contexte": text(), "enjeu": "", "suggestion": ""})      # archive path
    while len(current) < n_cur:
        current.append({"path": f"2.{len(current):02d}", "titre": f"Autre {len(current):02d}",
                        "contexte": text(), "enjeu": "", "suggestion": ""})

    for rel, rows in ((ARCHIVE_REL, archive), (CURRENT_REL, current)):
        dest = os.path.join(tmp, rel)
        os.makedirs(os.path.dirname(dest), exist_ok=True)
        with open(dest, "w", encoding="utf-8", newline="") as fh:
            writer = csv.DictWriter(fh, fieldnames=list(REQUIRED))
            writer.writeheader()
            writer.writerows(rows)
    return archive, current


def self_test():
    """Throwaway fixtures only — the corner cases the corpus cannot supply on demand:
    a re-attribution, a rename, a poisoned path, and a renamed column (the guard)."""
    import tempfile
    failures = []

    def check(name, condition):
        print(f"  [{'ok' if condition else 'KO'}] {name}")
        if not condition:
            failures.append(name)

    with tempfile.TemporaryDirectory() as tmp:
        archive, current = build_fixture(tmp)
        res = analyse(tmp)
        check("la fixture se charge", res is not None)
        if res:
            check("le plancher est calibre sur TOUTES les paires",
                  res["floor_pairs"] == len(archive) * len(current))
            check("le seuil reste dans le BRUIT (sinon la fixture est fausse)",
                  res["threshold"] < 0.9)
            check("E1 titre retrouve T1 et T2 (la carte deplacee incluse)", len(res["e1"]) == 2)
            check("E2 recupere T3 (renommee) par le controle",
                  len(res["recovered"]) == 1 and res["recovered"][0][0]["titre"] == "Titre renomme")
            check("PATH est condamne par le controle", len(res["path_below"]) == 1)
            check("verdict global OK sur la fixture", verdict(res, "title") == 0)
            check("MUTATION: la cle path echoue le controle", verdict(res, "path") == 2)

    # Guard: a renamed column must fail loud, not yield a silent 0.
    with tempfile.TemporaryDirectory() as tmp:
        build_fixture(tmp)
        dest = os.path.join(tmp, ARCHIVE_REL)
        rows, _ = load_csv(dest)
        renamed = [{"path": r["path"], "titre_renomme": r["titre"], "contexte": r["contexte"],
                    "enjeu": r.get("enjeu", ""), "suggestion": r.get("suggestion", "")}
                   for r in rows]
        with open(dest, "w", encoding="utf-8", newline="") as fh:
            writer = csv.DictWriter(fh, fieldnames=["path", "titre_renomme", "contexte",
                                                    "enjeu", "suggestion"])
            writer.writeheader()
            writer.writerows(renamed)
        _, header = load_csv(dest)
        check("colonne renommee => header_ok FAUX", not header_ok(header, "fixture"))

    print()
    print(f"self-test: {len(failures)} echec(s)")
    return 2 if failures else 0


def main():
    parser = argparse.ArgumentParser(description="G5 #1499 — cle de jointure Scenarii (lecture seule)")
    parser.add_argument("--corpus", default=".", help="racine du depot (defaut: .)")
    parser.add_argument("--report", action="store_true", help="tables completes")
    parser.add_argument("--key", choices=("title", "path"), default="title",
                        help="cle testee ; --key path est la MUTATION FALSIFIANTE")
    parser.add_argument("--self-test", action="store_true", help="suite sur fixtures jetables")
    args = parser.parse_args()

    if args.self_test:
        return self_test()

    result = analyse(args.corpus)
    if result is None:
        return 2
    if args.report:
        report(result)
        print()
    return verdict(result, args.key)


if __name__ == "__main__":
    sys.exit(main())