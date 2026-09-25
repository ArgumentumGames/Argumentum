#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 grain ② — PK 636 « Sophisme de regression » : 8 cellules desc_* splice byte-exact.

Dispatch [#1499 c.5837035919] : retour a la definition de 2022 (tranche par ai-01,
Regle C — l'actuelle introduit une erreur de SENS : « attribuer un effet a X » se lit
comme « X produit l'effet », alors que le sophisme est d'INVENTER UNE CAUSE).

  fr, en, ru : repris de l'archive du depot (Cards/Fallacies/Archive/v3/) ;
  pt, es, ar, fa, zh : retraduits dans le REGISTRE ACTUEL du deck (vouvoiement).
  ⛔ Aucun appel payant. Seules les 8 cellules desc_* sont dans le perimetre.

GARDES
  (1) PERIMETRE : la diff de valeurs == exactement les 8 cellules desc_* de PK 636 ;
  (2) DERIVATION : fr == archive (' -> ’) ; en == archive a l'octet ; ru == archive
      espace final retire (et l'espace final est DECLARE, pas tu) ; pt porte les
      ancres de l'archive (sinon la retraduction a derive de sa source) ;
  (3) structure : nb lignes, BOM, terminaison de chaque ligne INCHANGES ;
  (4) RE-PARSE complet : les cellules changees == exactement la worklist ;
  (5) POST-ETAT : norme typographique #994 (livree en #1562) — 0 ' ou " droit en
      fr/ru/pt/es/fa/zh/ar, 0 ’ courbe en en ; pas d'espace parasite en tete/queue ;
  (6) MUTATION : une cellule remise a son ancienne valeur doit faire ROUGIR (1).

Usage :
    python tools/1499-write-636-desc-byte-exact.py             # --check : plan + preuve, 0 ecriture
    python tools/1499-write-636-desc-byte-exact.py --apply     # ecrit + backup .BEFORE-636
    python tools/1499-write-636-desc-byte-exact.py --mutation-test
"""

import argparse
import csv
import io
import json
import os
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_REL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
ARCHIVE_REL = "Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv"
WORKLIST = os.path.join(REPO, "tools", "1499-636-desc-worklist.json")
BACKUP_SUFFIX = ".BEFORE-636"
BOM = b"\xef\xbb\xbf"
PK = "636"
ARCHIVE_PK = "524"
N_ROWS = 1408
STRAIGHT, DQUOTE, CURVED = "'", '"', "’"
ELISION_LANGS = {"fr", "ru", "pt", "es", "fa"}
LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]


# ── splitters byte-exacts (meme famille que 1471/1497/994) ────────────────────
def field_spans(record):
    spans, i, start, quoted = [], 0, 0, False
    while i < len(record):
        ch = record[i]
        if ch == '"':
            if quoted and i + 1 < len(record) and record[i + 1] == '"':
                i += 2
                continue
            quoted = not quoted
        elif ch == "," and not quoted:
            spans.append((start, i))
            start = i + 1
        i += 1
    spans.append((start, len(record)))
    return spans


def unquote(field):
    if len(field) >= 2 and field.startswith('"') and field.endswith('"'):
        return field[1:-1].replace('""', '"')
    return field


def split_raw(rec):
    return [rec[s:e] for s, e in field_spans(rec)]


def parse_raw(raw):
    """(bom, [(record_brut, terminaison)], header) — UN SEUL parseur pour tout le fichier.

    CRLF preserve ligne a ligne, guillemets doubles echappes geres : un second parseur
    ecrit ailleurs dans ce fichier pourrait diverger silencieusement du premier.
    """
    bom = raw.startswith(BOM)
    text = raw[len(BOM):].decode("utf-8") if bom else raw.decode("utf-8")
    rows, cur, in_q, i, n = [], [], False, 0, len(text)
    while i < n:
        ch = text[i]
        if ch == '"':
            if in_q and i + 1 < n and text[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch in "\r\n" and not in_q:
            if ch == "\r" and i + 1 < n and text[i + 1] == "\n":
                rows.append(("".join(cur), "\r\n")); cur = []; i += 2
            else:
                rows.append(("".join(cur), "\n")); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    if cur:
        rows.append(("".join(cur), ""))
    return bom, rows, [unquote(f) for f in split_raw(rows[0][0])]


def read_rows(rel):
    return parse_raw(open(os.path.join(REPO, rel), "rb").read())


def archive_value(lang):
    _b, rows, header = read_rows(ARCHIVE_REL)
    pk_i, col = header.index("PK"), f"desc_{lang}"
    for rec, _t in rows[1:]:
        f = split_raw(rec)
        if unquote(f[pk_i]).strip() == ARCHIVE_PK:
            return unquote(f[header.index(col)])
    raise AssertionError(f"{ARCHIVE_REL}: PK {ARCHIVE_PK} introuvable")


def load_worklist():
    w = json.load(open(WORKLIST, encoding="utf-8"))
    cells = {c["lang"]: c for c in w["cellules"]}
    assert set(cells) == set(LANGS), f"worklist incomplete : {sorted(cells)}"
    return w, cells


def check_derivation(cells):
    """Garde (2) : les valeurs fr/en/ru sont DERIVEES de l'archive, pas recitees."""
    fr_a = archive_value("fr")
    assert cells["fr"]["valeur"] == fr_a.replace(STRAIGHT, CURVED), \
        f"fr != archive aux apostrophes pres :\n  worklist {cells['fr']['valeur']!r}\n  archive  {fr_a!r}"
    assert cells["en"]["valeur"] == archive_value("en"), "en != archive a l'octet"
    ru_a = archive_value("ru")
    assert cells["ru"]["valeur"] == ru_a.rstrip(), "ru != archive (espace final retire)"
    assert ru_a != ru_a.rstrip(), \
        "l'archive ru n'a PAS d'espace final : la ligne de declaration du dossier est a corriger"
    pt_a = archive_value("pt")
    # ⚠️ DEUX controles, pas un : la liste d'ancres doit etre portee par l'archive
    # (elle est DERIVEE de la source, pas inventee) ET par la valeur retenue (la
    # retraduction n'a pas derive). Ne verifier que la premiere etait VACUE : elle
    # teste une constante et passait pour n'importe quelle valeur — vue par la
    # mutation « ancre perdue », qui restait verte.
    for a in cells["pt"]["ancres_archive"]:
        assert a in pt_a, f"ancre '{a}' absente de l'ARCHIVE pt : la liste d'ancres est inventee"
        assert a in cells["pt"]["valeur"], \
            f"ancre '{a}' absente de la valeur RETENUE : la retraduction pt a derive de l'archive"
    return {"fr": fr_a, "en": archive_value("en"), "ru": ru_a, "pt": pt_a}


def residual_of(lang, v):
    """Garde (5) : norme typographique #994 (CURVED en en, STRAIGHT interdit ailleurs)."""
    errs = []
    if DQUOTE in v:
        errs.append('" droit')
    if lang == "en":
        if CURVED in v:
            errs.append("’ courbe")
    elif STRAIGHT in v:
        errs.append("' droit")
    if v != v.strip():
        errs.append("espace en tete/queue")
    if v.endswith("..") or "  " in v:
        errs.append("ponctuation/espace double")
    return errs


def reserialize(raw_field, new_value):
    """Re-serialise en PRESERVANT la decision de quotage d'origine du champ.

    ⚠️ Sans cela, ecrire une valeur contenant une virgule dans un champ NU (les 7
    autres) ou retirer les guillemets d'un champ QUOTE (desc_ru en porte un)
    deplace la frontiere des champs : la ligne passe de 104 a 105 champs et toutes
    les colonnes suivantes glissent d'un cran. Mesure : desc_ru est le seul champ
    quote des 8, et la seule valeur neuve a porter une virgule U+002C.
    """
    was_wrapped = len(raw_field) >= 2 and raw_field[0] == '"' and raw_field[-1] == '"'
    if was_wrapped:
        return '"' + new_value.replace('"', '""') + '"'
    assert not any(c in new_value for c in ',"\r\n'), \
        f"champ non quote qui deviendrait special : {new_value[:60]!r}"
    return new_value


def plan(cells):
    """Construit la nouvelle valeur de chaque cellule cible + les edits par offsets."""
    bom, rows, header = read_rows(CSV_REL)
    assert len(rows) - 1 == N_ROWS, f"lignes {len(rows) - 1} != {N_ROWS}"
    text = "".join(r + t for r, t in rows)
    edits, avant = [], {}
    pk_i = header.index("PK")
    hits = [i for i, (rec, _t) in enumerate(rows) if i and unquote(split_raw(rec)[pk_i]).strip() == PK]
    assert len(hits) == 1, f"PK {PK} : {len(hits)} enregistrements"
    row_idx = hits[0]
    rec = rows[row_idx][0]
    rec_offset = text.index(rec)
    spans = field_spans(rec)
    for lang in LANGS:
        col = f"desc_{lang}"
        idx = header.index(col)
        s, e = spans[idx]
        old = unquote(rec[s:e])
        new = cells[lang]["valeur"]
        errs = residual_of(lang, new)
        assert not errs, f"garde (5) [{lang}] {col} : {errs} — {new[:60]!r}"
        assert old != new, f"[{lang}] {col} : valeur identique, rien a ecrire"
        avant[lang] = old
        edits.append((rec_offset + s, rec_offset + e, reserialize(rec[s:e], new), col))
    edits.sort(key=lambda t: t[0], reverse=True)
    new_text = text
    for s, e, new, _col in edits:
        new_text = new_text[:s] + new + new_text[e:]
    new_raw = (BOM if bom else b"") + new_text.encode("utf-8")
    return new_raw, sorted(edits, key=lambda t: t[3]), avant


def verify(old_raw, new_raw, cells):
    """Gardes (1) et (3) : la diff de VALEURS == la worklist, structure inchangee."""
    def rows_of(raw):
        text = raw[len(BOM):].decode("utf-8") if raw.startswith(BOM) else raw.decode("utf-8")
        return list(csv.DictReader(io.StringIO(text)))

    o, n = rows_of(old_raw), rows_of(new_raw)
    assert len(o) == len(n) == N_ROWS, f"lignes {len(o)}/{len(n)}"
    expected = {(PK, f"desc_{lg}") for lg in LANGS}
    changed = []
    for ro, rn in zip(o, n):
        assert ro["PK"] == rn["PK"], f"ordre des lignes modifie : {ro['PK']} != {rn['PK']}"
        for k in ro:
            if ro[k] != rn[k]:
                changed.append((rn["PK"].strip(), k))
    assert set(changed) == expected, f"cellules changees != worklist : {set(changed) ^ expected}"
    bo, ro_, ho = parse_raw(old_raw)
    bn, rn_, hn = parse_raw(new_raw)
    assert ho == hn, "en-tete modifie"
    assert bo == bn, "BOM modifie"
    assert [t for _r, t in ro_] == [t for _r, t in rn_], "terminaisons de ligne modifiees"
    assert len(ro_) == len(rn_), "nb de lignes modifie"
    # garde de FRONTIERE : le nombre de champs de CHAQUE ligne est inchange. Un champ
    # nu qui recoit une virgule, ou un champ quote qui perd ses guillemets, deplace la
    # frontiere et fait glisser toutes les colonnes suivantes d'un cran — le nombre de
    # champs le dit en une ligne, la diff de valeurs le dit en 46 cles.
    for i, ((rec_o, _t), (rec_n, _u)) in enumerate(zip(ro_, rn_)):
        no, nn = len(split_raw(rec_o)), len(split_raw(rec_n))
        assert no == nn, f"ligne {i}: {no} -> {nn} champs (frontiere de champ deplacee)"
    for lg in LANGS:
        row = next(r for r in n if r["PK"].strip() == PK)
        assert row[f"desc_{lg}"] == cells[lg]["valeur"], f"desc_{lg} non ecrite"
    return changed, (bn, len(rn_))


def mutation_test(cells):
    """Garde (6) : controle falsifiant sur LITTERAUX — ne lit PAS l'arbre.

    ⚠️ Une premiere version lisait la valeur courante DANS L'ARBRE : apres --apply,
    la valeur courante EST la valeur neuve, la mutation ne mutait donc rien et le
    controle se declarait « aveugle » a tort (rc=1 sur un instrument correct).
    Un controle qui depend de l'etat qu'il valide ne se rejoue pas.
    """
    ANCIENNE_FR = ("Vous attribuez à tort un effet particulier à ce qui n’est qu’un "
                   "retour à la normale après une fluctuation.")
    cas = [
        ("fr = archive SANS courbure (apostrophes droites)",
         {"fr": archive_value("fr")}, "derivation (2)", True),
        ("fr = ancienne valeur (sens d'origine, celui que la Règle C retire)",
         {"fr": ANCIENNE_FR}, "derivation (2)", True),
        ("ru = archive AVEC son espace final 2022",
         {"ru": archive_value("ru")}, "derivation (2)", True),
        ("pt = ancre d'archive perdue (mot 'flutuação' retire)",
         {"pt": cells["pt"]["valeur"].replace("flutuação", "oscilação")}, "ancre pt (2)", True),
    ]
    ok = True
    for nom, patch, garde, attendu in [(n, p, g, a) for n, p, g, a in cas] + \
            [("TEMOIN : worklist inchangee", {}, "aucune", False)]:
        mut = {lg: dict(cells[lg]) for lg in LANGS}
        for lg, val in patch.items():
            mut[lg]["valeur"] = val
        rougi = False
        try:
            check_derivation(mut)
        except AssertionError:
            rougi = True
        if rougi != attendu:
            ok = False
        print(f"  [{'PASS' if rougi == attendu else 'FAIL'}] {nom} -> "
              f"{'ROUGE' if rougi else 'vert'} (attendu {'ROUGE' if attendu else 'vert'} : {garde})")
    print("MUTATION " + ("OK (garde (2) rougit sur les 4 mutations, et pas sur le temoin)"
                         if ok else "NON PROUVEE"))
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    ap.add_argument("--mutation-test", action="store_true")
    args = ap.parse_args()

    w, cells = load_worklist()
    if args.mutation_test:
        return mutation_test(cells)

    deriv = check_derivation(cells)
    print("garde (2) derivation : fr/en/ru == archive v3 (PK 524) ; ancres pt presentes")
    for lg in ("fr", "en", "ru"):
        src = deriv[lg]
        note = " (espace final retire)" if lg == "ru" else (" (' -> ’)" if lg == "fr" else " (a l'octet)")
        print(f"  {lg}: archive PK {ARCHIVE_PK} {len(src)} car.{note}")

    old_raw = open(os.path.join(REPO, CSV_REL), "rb").read()
    new_raw, edits, avant = plan(cells)
    changed, struct = verify(old_raw, new_raw, cells)
    print(f"plan : {len(edits)} cellules, {len(changed)} cellules changees, "
          f"{struct[1]} lignes, BOM={struct[0]}, octets {len(old_raw)} -> {len(new_raw)} "
          f"({len(new_raw) - len(old_raw):+d})")
    for _s, _e, _raw, col in edits:
        lg = col.split("_")[-1]
        print(f"  {col:10s} {avant[lg][:52]!r}\n             -> {cells[lg]['valeur'][:70]!r}")

    if not args.apply:
        print("MODE --check : aucune ecriture.")
        return 0

    path = os.path.join(REPO, CSV_REL)
    with open(path + BACKUP_SUFFIX, "wb") as fh:
        fh.write(old_raw)
    with open(path, "wb") as fh:
        fh.write(new_raw)
    assert open(path, "rb").read() == new_raw, "relecture != ecrit"
    print(f"APPLIQUE. backup: {CSV_REL}{BACKUP_SUFFIX}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
