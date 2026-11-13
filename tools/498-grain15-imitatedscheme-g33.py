# -*- coding: utf-8 -*-
"""#498 grain 15 — groupe 3.3 « Résultat invalide » : références aux têtes 3.3.1 et 3.3.2.

Cadrage : docs/ontology/498-grain9-cadrage-scheme-imite-2026-11-10.md (§3.3).
Représentation (GO ai-01 #2229) : référence → AIF_skosExceptionRef jeton nu
+ AIF_skosMappingType='skos:broadMatch' (pattern grains 12-14).
La tête de groupe 3.3 (PK 666) reste NUE (hétérogène, documentée pour (c)) — jamais touchée.

⚠️ ÉCART DE SPÉC DECLARÉ — PK 690 (3.3.3 Opération inappropriée) retiré de l'écriture :
le cadrage §3.3.3 prescrit une référence Deductive_Inference sur la tête 3.3.3, mais
`FallacyAttackOnlyStopGateTests` épingle PK 690 comme ancre RATIFIÉE « sans scheme AIF
natif », attack-columns-only ÉTANT l'état final (498-aif-operation-inappropriee-cluster.md) :
« reopen the arbitration BEFORE writing, do not route around this gate ». Les 6 nœuds de
3.3.3 restent donc sans-(a) en attendant la réouverture de l'arbitrage (signalé à ai-01).
Rouge constaté sur la tête 3-rangées (base master 3/3 vert) avant retrait.

Cibles (2 rangées, 4 cellules) :
  PK 667 3.3.1 Imprécision      → EvidenceToHypothesis_Inference (porté par 3.1 PK 595 et 3.2 PK 632)
  PK 681 3.3.2 Erreur de calcul → Deductive_Inference            (porté par 4.2 PK 726 et 4.3 PK 758)

Porteur interne : PK 677 @ 3.3.1.3.1 (slippery slopes, skos:narrowMatch) — coexiste par
plus-proche-ancêtre ; son sous-arbre garde 677 comme porteur.

Lecture seule par défaut ; écriture uniquement avec --write. Verify AVANT l'écriture ;
re-vérification byte-exact + decoded-equal APRÈS. Base : ce checkout (47d70f15)."""
import io
import os
import sys

import csv  # decoded-equality only, never for writing

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # tools/x.py -> <repo>
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

BOM = b"\xef\xbb\xbf"

REF_EDITS = {  # PK -> (ExceptionRef, MappingType) — jetons nus (pas de virgule)
    "667": ("EvidenceToHypothesis_Inference", "skos:broadMatch"),
    "681": ("Deductive_Inference", "skos:broadMatch"),
}
PATHS = {"667": "3.3.1", "681": "3.3.2"}
NAKED_HEADS = ("3.3", "3.3.3")  # 3.3 hétérogène (cadrage §1) ; 3.3.3 = ancre RATIFIÉE attack-only (écart déclaré)


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
    I_PATH = header.index("path")
    I_PK = header.index("PK")
    assert header[I_EXC + 1] == "AIF_imitatedScheme", "position colonne inattendue"

    by_pk = {}
    for r in rows[1:]:
        f = split_fields(r)
        assert len(f) == 106, f"rangée à {len(f)} champs"
        by_pk[f[I_PK]] = f

    # --- garde : chemins + naked head intouchée + cellules cibles VIDES ---
    for pk, path in PATHS.items():
        assert by_pk[pk][I_PATH] == path, f"PK {pk} path {by_pk[pk][I_PATH]!r} != {path!r}"
    for pk in by_pk:
        if by_pk[pk][I_PATH] in NAKED_HEADS:
            assert pk not in REF_EDITS, f"tête nue {pk} ne s'écrit pas"
    for pk, (exc, _m) in REF_EDITS.items():
        assert by_pk[pk][I_EXC] == "", f"anti-overwrite: PK {pk} Exc non vide"
        assert by_pk[pk][I_MAP] == "", f"anti-overwrite: PK {pk} Map non vide"
    # précédents du cadrage, présents sur l'arbre
    assert "EvidenceToHypothesis_Inference" in decode(by_pk["595"][I_EXC])  # 3.1 (liste multi-jetons)
    assert decode(by_pk["632"][I_EXC]) == "EvidenceToHypothesis_Inference"  # 3.2 (jeton seul)
    assert "Deductive_Inference" in decode(by_pk["726"][I_EXC])             # 4.2 (liste)
    assert decode(by_pk["758"][I_EXC]) == "Deductive_Inference"             # 4.3 (jeton seul)
    # porteur interne sous 3.3.1 : son sous-arbre garde 677 comme plus proche ancêtre porteur
    assert decode(by_pk["677"][I_EXC]).startswith("PrecedentSlipperySlope")

    exc_before = sum(1 for r in rows[1:] if split_fields(r)[I_EXC] != "")

    # --- application ---
    new_rows = [rows[0]]
    n_cells = 0
    for r in rows[1:]:
        f = split_fields(r)
        pk = f[I_PK]
        if pk in REF_EDITS:
            exc, m = REF_EDITS[pk]
            f[I_EXC] = exc          # jeton nu
            f[I_MAP] = m            # jeton nu
            n_cells += 2
        new_rows.append(",".join(f))
    assert n_cells == 4, f"cellules écrites {n_cells} != 4"
    new_text = "\r\n".join(new_rows) + "\r\n"

    # --- verify (sur le texte NOUVEAU, avant toute écriture disque) ---
    vrows = split_logical_rows(new_text)
    vheader = split_fields(vrows[0])
    assert vheader == header
    vby = {split_fields(r)[I_PK]: split_fields(r) for r in vrows[1:]}
    for pk, (exc, m) in REF_EDITS.items():
        assert decode(vby[pk][I_EXC]) == exc and vby[pk][I_EXC] == exc, f"{pk} Exc"
        assert decode(vby[pk][I_MAP]) == m, f"{pk} Map"
    # toutes les AUTRES cellules byte-identiques ET decoded-equal
    o_parsed = list(csv.reader(io.StringIO(text, newline="")))
    n_parsed = list(csv.reader(io.StringIO(new_text, newline="")))
    assert len(o_parsed) == len(n_parsed) == 1409
    expected = {}
    for pk, (exc, m) in REF_EDITS.items():
        expected[(pk, "AIF_skosExceptionRef")] = exc
        expected[(pk, "AIF_skosMappingType")] = m
    for o_row, n_row in zip(o_parsed[1:], n_parsed[1:]):
        pk = o_row[I_PK]
        for ci, col in enumerate(header):
            want = expected.get((pk, col), o_row[ci])
            assert n_row[ci] == want, f"delta inattendu PK {pk} {col}"
    exc_after = sum(1 for r in vrows[1:] if split_fields(r)[I_EXC] != "")
    assert exc_after == exc_before + 2, f"Exc remplis {exc_before}->{exc_after}"

    if write:
        tmp = CSV_PATH + ".new15"
        with open(tmp, "wb") as fh:
            fh.write(BOM + new_text.encode("utf-8"))
        os.replace(tmp, CSV_PATH)
        print(f"ECRIT: 4 cellules / 2 rangées ; Exc remplis {exc_before}->{exc_after}")
    else:
        print(f"DRY-RUN OK: 4 cellules / 2 rangées seraient écrites (Exc {exc_before}->{exc_after})")


if __name__ == "__main__":
    main()
