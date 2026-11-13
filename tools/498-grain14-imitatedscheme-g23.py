# -*- coding: utf-8 -*-
"""#498 grain 14 — groupe 2.3 « Manipulation mentale » : écritures adjudiquées du cadrage mergé.

Cadrage : docs/ontology/498-grain9-cadrage-scheme-imite-2026-11-10.md (§2.3.2, §2.3.3).
Représentation (GO ai-01 #2229) : référence → AIF_skosExceptionRef (+ skos:broadMatch) ;
lacune déclarée → AIF_skosOther, prose anglaise au préfixe du marqueur existant (PK 247/834).
Les têtes 2.3 et 2.3.2 restent NUES (documentées pour (c), cadrage §1) — jamais touchées.

Cibles (6 rangées, 9 cellules) :
  PK 421 2.3.2.1 Charme personnel      → Exc=Ethotic_Inference       + Map=skos:broadMatch
  PK 431 2.3.2.2 Prise de contrôle     → Other=lacune ( contrôle s'exerce, ne se déduit pas )
  PK 456 2.3.2.3 Jeu psychologique     → Other=lacune ( mécanismes sans contenu propositionnel )
  PK 497 2.3.2.4 Conditionnement social→ Exc=PopularOpinion_Inference+ Map=skos:broadMatch
  PK 511 2.3.3   Communication non verbale (tête) → Other=lacune ( signaux, pas de propositions )
  PK 590 2.3.3.5 Apparence             → Exc=Sign_Inference          + Map=skos:broadMatch

Les libellés anglais des lacunes reflètent les justifications FR du cadrage (même convention
que le grain 12 : PK 247 — le cadrage cite en FR, le corpus porte la prose EN au préfixe
« No native AIF Inference scheme for … »).

Lecture seule par défaut ; écriture uniquement avec --write. Verify AVANT l'écriture ;
re-vérification byte-exact + decoded-equal APRÈS. Base : ce checkout (92076717)."""
import io
import os
import sys

import csv  # decoded-equality only, never for writing

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # tools/x.py -> <repo>
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

BOM = b"\xef\xbb\xbf"

REF_EDITS = {  # PK -> (ExceptionRef, MappingType) — jetons nus (pas de virgule)
    "421": ("Ethotic_Inference", "skos:broadMatch"),
    "497": ("PopularOpinion_Inference", "skos:broadMatch"),
    "590": ("Sign_Inference", "skos:broadMatch"),
}
LACUNE_EDITS = {  # PK -> prose AIF_skosOther (anglaise, préfixe du marqueur existant)
    "431": "No native AIF Inference scheme for coercion and domination; control is exerted, not inferred",
    "456": "No native AIF Inference scheme for psychological manipulation; the mechanisms (guilt-tripping, implicit threat) carry no imitated propositional content",
    "511": "No native AIF Inference scheme for non-verbal channels; they transmit signals, not propositions",
}
PATHS = {"421": "2.3.2.1", "431": "2.3.2.2", "456": "2.3.2.3", "497": "2.3.2.4",
         "511": "2.3.3", "590": "2.3.3.5"}
NAKED_HEADS = ("2.3", "2.3.2")  # jamais touchées (cadrage §1)


def split_logical_rows(text):
    """RFC4180 : séparateur = CRLF hors guillemets ; guillemets doublés dedans."""
    rows, cur, i, inq = [], [], 0, False
    while i < len(text):
        c = text[i]
        if inq:
            if c == '"':
                if i + 1 < len(text) and text[i + 1] == '"':
                    cur.append('""'); i += 2; continue
                inq = False; cur.append(c)  # le fermant fait partie du champ sérialisé
            else:
                cur.append(c)
        else:
            if c == '"':
                inq = True; cur.append(c)
            elif c == "\r" and i + 1 < len(text) and text[i + 1] == "\n":
                rows.append("".join(cur)); cur = []; i += 2; continue
            else:
                cur.append(c)
        i += 1
    if cur or (text and not text.endswith("\r\n")):
        rows.append("".join(cur))
    return rows


def split_fields(row):
    fields, cur, i, inq = [], [], 0, False
    while i < len(row):
        c = row[i]
        if inq:
            cur.append(c)
            if c == '"':
                if i + 1 < len(row) and row[i + 1] == '"':
                    cur.append('"'); i += 2; continue
                inq = False
        else:
            if c == '"':
                inq = True; cur.append(c)
            elif c == ",":
                fields.append("".join(cur)); cur = []
            else:
                cur.append(c)
        i += 1
    fields.append("".join(cur))
    return fields


def decode(f):
    """Valeur décodée d'un champ sérialisé."""
    if len(f) >= 2 and f[0] == '"' and f[-1] == '"':
        return f[1:-1].replace('""', '"')
    return f


def main():
    write = "--write" in sys.argv
    raw = open(CSV_PATH, "rb").read()
    assert raw[:3] == BOM, "BOM absent"
    assert raw.endswith(b"\r\n"), "dernière rangée non CRLF"
    text = raw.decode("utf-8")
    assert text[0] == "﻿", "BOM absente du texte décodé"
    text = text[1:]  # la BOM vit dans les octets, jamais dans les champs
    rows = split_logical_rows(text)
    header = split_fields(rows[0])
    assert len(header) == 106, f"header {len(header)} != 106"
    I_EXC = header.index("AIF_skosExceptionRef")
    I_MAP = header.index("AIF_skosMappingType")
    I_OTH = header.index("AIF_skosOther")
    I_PATH = header.index("path")
    I_PK = header.index("PK")
    assert header[I_EXC + 1] == "AIF_imitatedScheme", "position colonne inattendue"

    by_pk = {}
    for r in rows[1:]:
        f = split_fields(r)
        assert len(f) == 106, f"rangée à {len(f)} champs"
        by_pk[f[I_PK]] = f

    # --- garde : chemins + naked heads intouchés + cellules cibles VIDES ---
    for pk, path in PATHS.items():
        assert by_pk[pk][I_PATH] == path, f"PK {pk} path {by_pk[pk][I_PATH]!r} != {path!r}"
    for pk in by_pk:
        if by_pk[pk][I_PATH] in NAKED_HEADS:
            assert pk not in REF_EDITS and pk not in LACUNE_EDITS
    for pk, (exc, _m) in REF_EDITS.items():
        assert by_pk[pk][I_EXC] == "", f"anti-overwrite: PK {pk} Exc non vide"
        assert by_pk[pk][I_MAP] == "", f"anti-overwrite: PK {pk} Map non vide"
    for pk, prose in LACUNE_EDITS.items():
        assert by_pk[pk][I_OTH] == "", f"anti-overwrite: PK {pk} Other non vide"
    # précédents du cadrage, présents sur l'arbre
    assert decode(by_pk["1360"][I_EXC]) == "Ethotic_Inference"
    assert "PopularOpinion_Inference" in decode(by_pk["3"][I_EXC])
    assert decode(by_pk["357"][I_EXC]) == "Sign_Inference"
    # le porteur interne de 2.3.2.2 (le lacune parent ne le contredit pas : plus proche ancêtre)
    assert decode(by_pk["432"][I_EXC]) == "Waste_Inference"

    filled_other_before = sum(1 for r in rows[1:] if split_fields(r)[I_OTH] != "")
    exc_before = sum(1 for r in rows[1:] if split_fields(r)[I_EXC] != "")

    # --- application ---
    new_rows = [rows[0]]
    n_cells = 0
    for r in rows[1:]:
        f = split_fields(r)
        pk = f[I_PK]
        if pk in REF_EDITS:
            exc, m = REF_EDITS[pk]
            f[I_EXC] = exc          # jeton nu (pas de virgule)
            f[I_MAP] = m            # jeton nu
            n_cells += 2
        elif pk in LACUNE_EDITS:
            prose = LACUNE_EDITS[pk]
            assert '"' not in prose  # virgules légales dans la cellule quotée (RFC4180)
            f[I_OTH] = f'"{prose}"'  # prose → quotée RFC4180
            n_cells += 1
        new_rows.append(",".join(f))
    assert n_cells == 9, f"cellules écrites {n_cells} != 9"
    new_text = "\r\n".join(new_rows) + "\r\n"

    # --- verify (sur le texte NOUVEAU, avant toute écriture disque) ---
    vrows = split_logical_rows(new_text)
    vheader = split_fields(vrows[0])
    assert vheader == header
    vby = {split_fields(r)[I_PK]: split_fields(r) for r in vrows[1:]}
    for pk, (exc, m) in REF_EDITS.items():
        assert decode(vby[pk][I_EXC]) == exc and vby[pk][I_EXC] == exc, f"{pk} Exc"
        assert decode(vby[pk][I_MAP]) == m, f"{pk} Map"
    for pk, prose in LACUNE_EDITS.items():
        assert decode(vby[pk][I_OTH]) == prose, f"{pk} Other"
    # toutes les AUTRES cellules byte-identiques ET decoded-equal
    o_parsed = list(csv.reader(io.StringIO(text, newline="")))
    n_parsed = list(csv.reader(io.StringIO(new_text, newline="")))
    assert len(o_parsed) == len(n_parsed) == 1409
    expected = {}
    for pk, (exc, m) in REF_EDITS.items():
        expected[(pk, "AIF_skosExceptionRef")] = exc
        expected[(pk, "AIF_skosMappingType")] = m
    for pk, prose in LACUNE_EDITS.items():
        expected[(pk, "AIF_skosOther")] = prose
    for o_row, n_row in zip(o_parsed[1:], n_parsed[1:]):
        pk = o_row[I_PK]
        for ci, col in enumerate(header):
            want = expected.get((pk, col), o_row[ci])
            assert n_row[ci] == want, f"delta inattendu PK {pk} {col}"
    filled_other_after = sum(1 for r in vrows[1:] if split_fields(r)[I_OTH] != "")
    exc_after = sum(1 for r in vrows[1:] if split_fields(r)[I_EXC] != "")
    assert exc_after == exc_before + 3, f"Exc remplis {exc_before}->{exc_after}"
    assert filled_other_after == filled_other_before + 3

    if write:
        tmp = CSV_PATH + ".new14"
        with open(tmp, "wb") as fh:
            fh.write(BOM + new_text.encode("utf-8"))
        os.replace(tmp, CSV_PATH)
        print(f"ECRIT: 9 cellules / 6 rangées ; Exc remplis {exc_before}->{exc_after} ; "
              f"Other remplis {filled_other_before}->{filled_other_after}")
    else:
        print(f"DRY-RUN OK: 9 cellules / 6 rangées seraient écrites "
              f"(Exc {exc_before}->{exc_after}, Other {filled_other_before}->{filled_other_after})")


if __name__ == "__main__":
    main()
