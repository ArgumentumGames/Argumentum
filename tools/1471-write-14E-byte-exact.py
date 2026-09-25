#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1471 strate E — ecrire les 14 cellules PERTINENTES retenues apes re-lecture.

GO owner 22/09 (transmis sur #1471) : « on complete les ~33 qui ont une cible
pertinente ». Re-mesure au moment d'ecrire (exigee par le meme GO) : 40 PERTINENT
mesurees par l'audit, moins 9 deja remplies par #1487/#1508 = 31 candidates.

Pourquoi 14 et pas 31 : la borne du GO dit « Aucune cellule HORS_SUJET n'est
ecrite, quelle que soit sa classe d'origine. Le tri de #1494/#1496 est un FILTRE,
pas un inventaire ». La re-lecture des extraits wiki montre que l'heuristique de
pertinence laisse passer deux familles de faux positifs, retenues nommement :
  - GENIRIQUES (7) : pages parapluie qui ne traitent pas LE sophisme de la carte
    ('Falacia', 'Argumento', 'Logica', 'Paradoja', 'Argumentation', 'Falacia
    causal' categorie) ;
  - AUTRES SOPHISMES (10) : la page traite d'un sophisme DIFFERENT de la carte
    (bug logiciel pour 'Erreur de calcul', homme de paille pour 'ad logicam',
    'Faux dilemme' pour 'Infini trompeur' — la confusion Tier-2 deja signalee
    par #994 sur cette carte, loi des moyennes = l'erreur MIROIR de 'Main chaude').
La liste complete des 17 retenues vit dans le commentaire d'issue ; l'ecart se
dit, il ne se corrige pas en silence.

ZERO DEVINETTE : les 14 cibles viennent de l'API (top hit du sondage natif),
re-lues dans leur extrait ; le format URL est celui des 225 cellules #1487
(percent-encode, underscores). Aucune cible n'est choisie a la main.

QUATRE GARDES :
  (1) AUCUNE COLLISION : chaque cible vise une cellule vide, verifiee a l'octet.
  (2) REGEN-NEUTRALITE : link_* n'est rendu par aucun gabarit (#1487 garde 1).
  (3) 1408 LIGNES / CRLF / BOM invariants ; splice par offsets absolus, jamais
      par recherche de sous-chaine (des cellules vides ne sont pas uniques).
  (4) RE-PARSE COMPLET apres ecriture : les 1408 x N champs re-lus, les 14
      cellules egales a leur URL, tout le reste byte-identique.

Usage :
    python tools/1471-write-14E-byte-exact.py            # --check : preuve, 0 ecriture
    python tools/1471-write-14E-byte-exact.py --apply    # ecrit + backup .BEFORE-1471e
"""

import argparse
import csv
import io
import os
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
BACKUP_SUFFIX = ".BEFORE-1471e"

# (pk, lang, url) — issues de tools/1471-audit-pertinence-E.json (classe PERTINENT,
# cellule vide au re-mesurage, extraits re-lus). La colonne est link_<lang>.
WORKLIST = [
    ("34", "es", "https://es.wikipedia.org/wiki/Prueba_anecd%C3%B3tica"),
    ("34", "fr", "https://fr.wikipedia.org/wiki/Preuve_anecdotique"),
    ("34", "zh", "https://zh.wikipedia.org/wiki/%E8%BB%BC%E4%BA%8B%E8%AD%89%E6%93%9A"),
    ("153", "pt", "https://pt.wikipedia.org/wiki/Argumento_ad_logicam"),
    ("182", "es", "https://es.wikipedia.org/wiki/Falso_dilema"),
    ("182", "fr", "https://fr.wikipedia.org/wiki/Faux_dilemme"),
    ("182", "ru", "https://ru.wikipedia.org/wiki/%D0%9B%D0%BE%D0%B6%D0%BD%D0%B0%D1%8F_%D0%B4%D0%B8%D0%BB%D0%B5%D0%BC%D0%BC%D0%B0"),
    ("644", "pt", "https://pt.wikipedia.org/wiki/Fal%C3%A1cia_do_apostador"),
    ("653", "zh", "https://zh.wikipedia.org/wiki/%E7%86%B1%E6%89%8B%E8%AC%AC%E8%AA%A4"),
    ("735", "zh", "https://zh.wikipedia.org/wiki/%E9%87%8F%E5%8C%96%E8%A9%9E%E5%B0%8D%E8%AA%BF"),
    ("837", "zh", "https://zh.wikipedia.org/wiki/%E4%B8%8D%E4%B8%80%E8%87%B4%E7%9A%84%E6%AF%94%E8%BC%83"),
    ("1020", "fr", "https://fr.wikipedia.org/wiki/Co%C3%BBt_irr%C3%A9cup%C3%A9rable"),
    ("1361", "zh", "https://zh.wikipedia.org/wiki/%E4%B8%8D%E4%B8%80%E8%87%B4%E7%9A%84%E8%AC%AC%E8%AA%A4"),
    ("1388", "zh", "https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E4%BF%A1%E5%BF%83"),
]

BOM = b"\xef\xbb\xbf"


def field_spans(record):
    """Offsets (start, end, quoted) of every field of one record, quote-aware.
    The CRLF record separator never appears inside a quoted field in this file
    (multi-line cells use bare LF — asserted by the caller)."""
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


def quote_of(value, was_quoted):
    if not was_quoted:
        return value  # a URL carries no comma/quote/CRLF — stays unquoted
    raise AssertionError("collision sur une cellule quotée : refus (hors convention)")


def load_raw():
    with open(CSV_PATH, "rb") as fh:
        return fh.read()


def build(raw):
    """Returns (new_bytes, plan) after splicing the 14 URLs, or raises."""
    bom = raw.startswith(BOM)
    text = raw[len(BOM):].decode("utf-8") if bom else raw.decode("utf-8")
    records = text.split("\r\n")
    if records and records[-1] == "":
        records = records[:-1]
    # Guards: the separators this splicer relies on.
    assert text.count("\r\n") == 1409, f"CRLF: {text.count(chr(13)+chr(10))} != 1409"
    header = records[0]
    header_fields = [unquote(f) for f in split_raw(header)]
    pk_idx = header_fields.index("PK")

    by_pk = {}
    for rec in records[1:]:
        fields = split_raw(rec)
        by_pk.setdefault(unquote(fields[pk_idx]).strip(), []).append(rec)
    for pk in {w[0] for w in WORKLIST}:
        assert len(by_pk.get(pk, [])) == 1, f"PK {pk}: {len(by_pk.get(pk, []))} enregistrements"

    # Absolute byte offsets (unicode-code-point offsets on the decoded text), right-to-left
    # across the whole file so an earlier splice cannot shift a later one.
    edits = []
    for pk, lang, url in WORKLIST:
        col = f"link_{lang}"
        idx = header_fields.index(col)
        rec = by_pk[pk][0]
        spans = field_spans(rec)
        s, e = spans[idx]
        current = unquote(rec[s:e])
        assert current.strip() == "", f"COLLISION pk={pk} {col} non vide: {current[:60]!r}"
        rec_offset = text.index(rec)  # unique: records are unique by PK (asserted)
        edits.append((rec_offset + s, rec_offset + e, url, pk, col))

    edits.sort(key=lambda t: t[0], reverse=True)
    new_text = text
    for s, e, url, pk, col in edits:
        new_text = new_text[:s] + url + new_text[e:]

    new_bytes = (BOM if bom else b"") + new_text.encode("utf-8")
    return new_bytes, edits


def split_raw(rec):
    return [rec[s:e] for s, e in field_spans(rec)]


def verify(old_raw, new_raw):
    """Full re-parse: row count, and EVERY field compared — the 14 targets equal to
    their URL, every other field byte-identical."""
    def rows(raw):
        text = raw[len(BOM):].decode("utf-8") if raw.startswith(BOM) else raw.decode("utf-8")
        return list(csv.DictReader(io.StringIO(text)))
    old_rows, new_rows = rows(old_raw), rows(new_raw)
    assert len(old_rows) == len(new_rows) == 1408, f"lignes {len(old_rows)}/{len(new_rows)}"
    expected = {(pk, f"link_{lang}"): url for pk, lang, url in WORKLIST}
    changed = []
    for o, n in zip(old_rows, new_rows):
        assert o["PK"] == n["PK"]
        for key in o:
            if o[key] != n[key]:
                changed.append((n["PK"].strip(), key))
    assert set(changed) == set(expected.keys()), f"cellules changees != worklist: {set(changed) ^ set(expected.keys())}"
    for (pk, col), url in expected.items():
        row = next(r for r in new_rows if r["PK"].strip() == pk)
        assert row[col] == url, f"{pk}.{col}: {row[col]!r} != {url}"
    return changed


def main():
    parser = argparse.ArgumentParser(description="#1471 strate E : 14 cellules, splice byte-exact")
    parser.add_argument("--apply", action="store_true", help="ecrit reellement (default: --check)")
    args = parser.parse_args()

    old_raw = load_raw()
    new_raw, edits = build(old_raw)
    changed = verify(old_raw, new_raw)

    print(f"plan : {len(edits)} cellules")
    for pk, col, url in sorted(WORKLIST, key=lambda w: (int(w[0]), w[1])):
        print(f"  pk={pk:>5} {col:<9} -> {url}")
    print(f"verification : {len(changed)} cellules changees, exactement la worklist ; 1408 lignes ; BOM/CRLF invariants")
    print(f"octets : {len(old_raw)} -> {len(new_raw)} (delta {len(new_raw) - len(old_raw):+d})")

    if not args.apply:
        print("MODE --check : aucune ecriture.")
        return 0

    backup = CSV_PATH + BACKUP_SUFFIX
    with open(backup, "wb") as fh:
        fh.write(old_raw)
    with open(CSV_PATH, "wb") as fh:
        fh.write(new_raw)
    reread = load_raw()
    assert reread == new_raw, "relecture != ce qui a ete ecrit"
    print(f"APPLIQUE. backup: {backup}")
    return 0


if __name__ == "__main__":
    sys.exit(main())