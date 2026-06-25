#!/usr/bin/env python3
"""
#192 — terminology harmonization APPLIER (dry-run by default, gated --apply).

Counterpart to 192-terminology-audit.py (which DETECTS multi-variant groups).
This script APPLIES the ratified harmonization, converting jsboige's
ratification (Decision A register/cap + Decision B lexical picks) into a
drift-free CSV edit — the same method as #595 (QUOTE_MINIMAL + CRLF, UTF-8
no-BOM, byte-identical round-trip on unchanged cells).

SAFETY MODEL (HARD):
  - Default = DRY-RUN. Writes NOTHING under Cards/. Prints the exact diff
    (row, field, lang: old -> new) split into WOULD-CHANGE (auto-applicable)
    and PENDING (flagged for jsboige/native judgment — left untouched).
  - --apply actually rewrites the CSV. Intended POST-ratification only.
  - --verify-drift rewrites the CSV with ZERO cell changes and asserts the
    result is byte-identical to the source. Run this FIRST on any machine:
    if it reports drift, the writer dialect is wrong for this CSV and
    --apply must NOT be used (it would silently reformat every line).

PICK LOGIC (mirrors docs/taxonomy/192-terminology-glossary-register.md):
  For each detected multi-variant group (FR source label with >1 translation
  across rows), the winner is the majority variant. Default pick = winner.
  An EXCEPTION TABLE (below) encodes the doc's per-case guidance:
    - OVERRIDE  : apply a contra-majority recommended value (S6 meaning,
                  S7 number — the doc argues these beat the majority).
    - FLAG      : do NOT auto-apply; list as PENDING. Covers S2 scope,
                  S3 regional (jsboige judgment) and V1-V6 RTL/CJK near-ties
                  (native-required, per the doc's LOW confidence).
  Decision A (sentence-case register) is NOT reimplemented as a caser here
  (casing Portuguese proper nouns like "Idade Media" is error-prone). The
  cap-only cases (S4/S5/S8-cap) resolve by picking the winner variant AS-IS,
  so the ratified register surfaces through the chosen variant string, not a
  transform. jsboige ratifying sentence-case + majority = the winner wins.

Detection + dialect are runtime-derived (no hardcoded variant strings), so
this stays correct as the CSV evolves. NO WRITE under Cards/ unless --apply.
"""
import csv
import sys

MAJ = 0.80  # not used for picking (we pick majority regardless), kept for parity

# (dataset, path, [(fr_field, en_base_stem)], [target langs])
DATASETS = [
    ("Virtues", "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv", [
        ("family_fr", "family"),
        ("subfamily_fr", "subfamily"),
        ("subsubfamily_fr", "subsubfamily"),
    ], ["en", "ru", "pt", "es", "ar", "zh", "fa"]),
    ("Fallacies", "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv", [
        ("Famille", "Family"),
        ("Sous-Famille", "Subfamily"),
        ("Soussousfamille", "Subsubfamily"),
    ], ["en", "ru", "pt", "es", "ar", "zh", "fa"]),
    ("Scenarii", "Cards/Scenarii/Argumentum Scenarii - Cards.csv", [
        ("catégorie", "category"),
        ("sous-catégorie", "subcategory"),
    ], ["en", "ru", "pt", "es", "ar", "zh", "fa"]),
]

# EXCEPTION TABLE — keys = (dataset, fr_field, lang, fr_source_label_stripped).
# Each entry: ("OVERRIDE", "<value>")  -> apply this value (contra-majority recommend)
#             ("FLAG", "<reason>")      -> do not auto-apply; list as PENDING
# Anything NOT in this table = default majority pick.
# Encoding is faithful to 192-terminology-glossary-register.md (Scenarii S1-S8,
# Virtues V1-V6). Verified 2026-06-25 against bef3bc6c.
EXCEPTIONS = {
    # --- Scenarii PT : the 8 ARBITRARY/OBVIOUS groups ---
    ("Scenarii", "sous-catégorie", "pt", "contes"):
        ("FLAG", "S2 scope: 'contes' vs 'Contos e literatura' — jsboige decides tales-only vs tales+lit"),
    ("Scenarii", "sous-catégorie", "pt", "drague et séduction"):
        ("FLAG", "S3 regional: BR 'Paquera' vs PT-PT 'engate' — target audience decision (jsboige)"),
    ("Scenarii", "sous-catégorie", "pt", "gestion et administration"):
        ("OVERRIDE", "gestão e administração"),  # S4 cap-only, sentence-case (Decision A)
    ("Scenarii", "sous-catégorie", "pt", "moyen-âge et temps modernes"):
        ("OVERRIDE", "Idade Média e era moderna"),  # S5 OBVIOUS 83%, 'Idade Média' proper noun stays
    ("Scenarii", "sous-catégorie", "pt", "relations au travail"):
        ("OVERRIDE", "relações no trabalho"),  # S6 meaning: interpersonal (at-work), contra-majority legal
    ("Scenarii", "sous-catégorie", "pt", "religions"):
        ("OVERRIDE", "Religiões"),  # S7 number: plural = matches FR source, contra-majority singular
    ("Scenarii", "catégorie", "pt", "relation intime"):
        ("OVERRIDE", "relação íntima"),  # S1 majority + sentence-case (matches FR 'relation')
    ("Scenarii", "sous-catégorie", "pt", "vie de couple"):
        ("OVERRIDE", "vida de casal"),  # S8 majority + sentence-case (literal to FR)
    # --- Virtues V1-V6 RTL/CJK near-ties : native-required -> FLAG ---
    # All flagged per doc LOW confidence; majority only safe as interim w/ native ack.
    # NOTE: keys use the ACTUAL CSV column name (family_fr / subsubfamily_fr), not the
    # en_base stem (family / subsubfamily) — that's what detection produces in g["fr_field"].
    ("Virtues", "family_fr", "fa", "Raisonnement valide"):
        ("FLAG", "V1 fa near-tie 58% — native-required (inference vs argument nuance)"),
    ("Virtues", "subsubfamily_fr", "ru", "Objectif non complaisant"):
        ("FLAG", "V2 ru calque vs paraphrase, n=3 tiny — native-required"),
    ("Virtues", "subsubfamily_fr", "ar", "Mise à distance des idéologies"):
        ("FLAG", "V3 ar literal vs idiomatic, 3v2 — native-required"),
    ("Virtues", "subsubfamily_fr", "zh", "Raisonnement concluant"):
        ("FLAG", "V4 zh 3-way nuance — native-required"),
    ("Virtues", "subsubfamily_fr", "fa", "Mise à distance des idéologies"):
        ("FLAG", "V5 fa near-synonym 3v2 — native-required"),
    ("Virtues", "subsubfamily_fr", "fa", "Raisonnement concluant"):
        ("FLAG", "V6 fa 3-way register 63% — native-required"),
}


def detect_groups(name, rel, fields, langs):
    """Return list of groups: (fr_field, lang, tcol, fr_source, Counter, winner)."""
    with open(rel, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    header = rows[0]
    idx = {c: i for i, c in enumerate(header)}
    groups = []
    for fr_field, en_base in fields:
        if fr_field not in idx:
            continue
        fr_ci = idx[fr_field]
        for lang in langs:
            tcol = "%s_%s" % (en_base, lang)
            if tcol not in idx:
                continue
            t_ci = idx[tcol]
            fr2cnt = {}
            for r in rows[1:]:
                fr = r[fr_ci].strip() if fr_ci < len(r) else ""
                t = r[t_ci].strip() if t_ci < len(r) else ""
                if fr and t:
                    fr2cnt.setdefault(fr, {}).setdefault(t, 0)
                    fr2cnt[fr][t] += 1
            for fr_term, cnt in fr2cnt.items():
                if len(cnt) <= 1:
                    continue  # already consistent
                winner = max(cnt, key=lambda k: (cnt[k], k))
                groups.append({
                    "fr_field": fr_field, "lang": lang, "tcol": tcol, "t_ci": t_ci,
                    "fr_ci": fr_ci, "fr": fr_term, "cnt": cnt, "winner": winner,
                })
    return rows, header, idx, groups


def plan_changes(rows, groups):
    """Return (would_change, pending). would_change = list of (row_idx, tcol, old, new, reason).
    pending = list of group dicts with the flag reason (no cell changes)."""
    would_change = []
    pending = []
    for g in groups:
        key = (None, g["fr_field"], g["lang"], g["fr"])  # dataset filled by caller
        exc = None
        # match exception loosely on (fr_field, lang, fr) — dataset added by caller
        for k, v in EXCEPTIONS.items():
            if k[1] == g["fr_field"] and k[2] == g["lang"] and k[3] == g["fr"]:
                exc = v
                break
        if exc and exc[0] == "FLAG":
            pending.append({**g, "reason": exc[1]})
            continue
        target = exc[1] if (exc and exc[0] == "OVERRIDE") else g["winner"]
        # record every row of THIS group (same FR source label) whose value != target.
        # MUST scope by g["fr"] — else rows from other families get wrongly "harmonized"
        # to this group's winner (e.g. Argument-pertinent rows dragged into Raisonnement-valide).
        n = 0
        for ri, r in enumerate(rows[1:], start=1):
            row_fr = r[g["fr_ci"]].strip() if g["fr_ci"] < len(r) else ""
            if row_fr != g["fr"]:
                continue  # belongs to a different FR label — not this group
            cur = r[g["t_ci"]].strip() if g["t_ci"] < len(r) else ""
            if not cur:
                continue
            if cur != target:
                reason = "OVERRIDE" if (exc and exc[0] == "OVERRIDE") else "MAJORITY"
                would_change.append((ri, g["tcol"], g["fr_field"], g["lang"], cur, target, reason))
                n += 1
        if n == 0:
            # consistent already after pick; nothing to do (not pending either)
            pass
    return would_change, pending


def rewrite_csv(rel, rows, changes_by_rowcol):
    """Rewrite the CSV applying changes_by_rowcol={(row_idx,col_idx): new_value}.
    Dialect: QUOTE_MINIMAL, quotechar \", CRLF, UTF-8 no-BOM (matches #595)."""
    import tempfile, os
    out_rows = [list(r) for r in rows]
    for (ri, ci), val in changes_by_rowcol.items():
        out_rows[ri][ci] = val
    fd, tmp = tempfile.mkstemp(suffix=".csv", dir=os.path.dirname(rel) or ".")
    with os.fdopen(fd, "w", encoding="utf-8", newline="") as f:
        w = csv.writer(f, quoting=csv.QUOTE_MINIMAL, quotechar='"', lineterminator="\r\n")
        w.writerows(out_rows)
    return tmp


def main():
    mode = "dry-run"
    if "--apply" in sys.argv:
        mode = "apply"
    elif "--verify-drift" in sys.argv:
        mode = "verify-drift"

    if mode == "verify-drift":
        print("=== VERIFY-DRIFT (byte-identical round-trip with ZERO cell changes) ===")
        drift = False
        for name, rel, fields, langs in DATASETS:
            try:
                with open(rel, encoding="utf-8-sig", newline="") as f:
                    rows = list(csv.reader(f))
            except FileNotFoundError:
                print("  %s: file not found, skip" % rel)
                continue
            tmp = rewrite_csv(rel, rows, {})
            with open(rel, "rb") as a:
                orig = a.read()
            with open(tmp, "rb") as b:
                new = b.read()
            import os as _os
            _os.remove(tmp)
            ok = orig == new
            drift = drift or not ok
            print("  %-55s %s" % (rel, "BYTE-IDENTICAL ✓" if ok else "DRIFT ✗ (%d -> %d bytes)" % (len(orig), len(new))))
        print("\n" + ("DRIFT DETECTED — writer dialect wrong for at least one CSV; do NOT --apply." if drift
                      else "All CSVs byte-identical on zero-change round-trip. Writer dialect is safe."))
        sys.exit(1 if drift else 0)

    # dry-run / apply
    all_change = []
    all_pending = []
    apply_map = {}  # (row_idx, col_idx) -> new_value  (global across datasets; row indices reset per file)
    for name, rel, fields, langs in DATASETS:
        try:
            rows, header, idx, groups = detect_groups(name, rel, fields, langs)
        except FileNotFoundError:
            print("== %s: %s not found, skip ==\n" % (name, rel))
            continue
        # add dataset to groups for exception matching
        for g in groups:
            g["dataset"] = name
        wc, pend = plan_changes(rows, groups)
        all_change.extend([(name,) + tuple(c) for c in wc])
        all_pending.extend(pend)
        if mode == "apply":
            for (ri, tcol, frf, lang, old, new, reason) in wc:
                apply_map[(rel, ri, idx[tcol])] = new
        print("== %s (%s) ==" % (name, rel))
        print("  multi-variant groups detected: %d" % len(groups))
        print("  would-change cells: %d" % len(wc))
        print("  pending (flagged for judgment): %d" % len(pend))

    print("\n" + "=" * 78)
    print("WOULD-CHANGE (auto-applicable, applies ratified majority + overrides):")
    print("=" * 78)
    if not all_change:
        print("  (none)")
    for c in all_change:
        ds, ri, tcol, frf, lang, old, new, reason = c
        print("  [%s] row=%d %s.%s  '%s' -> '%s'  (%s)" % (ds, ri, frf, lang, old[:40], new[:40], reason))

    print("\n" + "=" * 78)
    print("PENDING (NOT auto-applied — needs jsboige / native ratification):")
    print("=" * 78)
    if not all_pending:
        print("  (none)")
    for p in all_pending:
        print("  [%s] %s.%s  FR='%s'  winner='%s'  -- %s" % (
            p["dataset"], p["fr_field"], p["lang"], p["fr"][:40], p["winner"][:40], p["reason"]))

    print("\n" + "-" * 78)
    print("SUMMARY: %d would-change cells, %d pending groups across %d datasets." % (
        len(all_change), len(all_pending), len([d for d in DATASETS])))
    if mode == "dry-run":
        print("Mode: DRY-RUN (0 write under Cards/). Re-run with --apply to write (POST-ratification only).")
    else:
        # apply
        n_written = 0
        files = {}
        for (rel, ri, ci), val in apply_map.items():
            files.setdefault(rel, []).append((ri, ci, val))
        for rel, edits in files.items():
            with open(rel, encoding="utf-8-sig", newline="") as f:
                rows = list(csv.reader(f))
            cmap = {(ri, ci): v for ri, ci, v in edits}
            tmp = rewrite_csv(rel, rows, cmap)
            import shutil
            shutil.move(tmp, rel)
            n_written += len(edits)
        print("Mode: APPLY — wrote %d cells across %d file(s). Re-run --verify-drift to confirm dialect intact." % (
            n_written, len(files)))


if __name__ == "__main__":
    main()
