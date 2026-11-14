# -*- coding: utf-8 -*-
"""#498 grain 16 — groupe 5.2 « Comparaison fallacieuse » : écriture adjudiquée du cadrage mergé.

Cadrage : docs/ontology/498-grain9-cadrage-scheme-imite-2026-11-10.md (§5.2.1, lignes 97-103).
Arbitrage ai-01 du 10/11 (verbatim #498) : « 5.2.1.1 reste sous la lacune de sa tête » —
donc AUCUNE référence `Analogy_Inference` et AUCUNE cellule sur PK 835 : le nœud est
tranché, pas écrit. La tête 5.2.1 (PK 834) porte déjà sa lacune en prose (`skosOther`),
la tête de groupe 5.2 (PK 833) reste nue (tête hétérogène, cadrage §1).

Une seule cellule :
  PK 837 5.2.1.3 Comparaison incohérente
      → AIF_imitatedScheme = phrase du cadrage (français, comme PK 244)
        « imite la comparaison complète et à critères stables — l'incohérence des
          critères tient lieu de mesure »

Représentation (décision d'ai-01 sur #2229) : une phrase « imite… » va dans la colonne
dédiée `AIF_imitatedScheme`, qui compte comme porteur (a) pour l'instrument.

Lecture seule par défaut ; écriture uniquement avec --write. Verify AVANT l'écriture ;
re-vérification byte-exact + decoded-equal APRÈS. Base : ce checkout (5.2 → 1 rangée).

    python tools/498-grain16-imitatedscheme-g52.py            # dry-run
    python tools/498-grain16-imitatedscheme-g52.py --write    # écrit
"""
import io
import os
import sys

import csv  # decoded-equality only, never for writing

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # tools/x.py -> <repo>
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

BOM = b"\xef\xbb\xbf"

PHRASE_EDITS = {  # PK -> prose française (colonne AIF_imitatedScheme)
    "837": ("imite la comparaison complète et à critères stables — "
            "l'incohérence des critères tient lieu de mesure"),
}
PATHS = {"837": "5.2.1.3"}
# Têtes jamais touchées : 5.2 (hétérogène, cadrage §1) et 5.2.1 (lacune déjà en place).
NAKED_HEADS = ("5.2", "5.2.1")
# Tranché sans écriture : PK 835 (5.2.1.1) — « reste sous la lacune de sa tête ».
ADJUDICATED_NO_WRITE = ("835",)


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
    assert len(header) == 106, "header %d != 106" % len(header)
    I_EXC = header.index("AIF_skosExceptionRef")
    I_IMIT = header.index("AIF_imitatedScheme")
    I_OTH = header.index("AIF_skosOther")
    I_PATH = header.index("path")
    I_PK = header.index("PK")
    assert header[I_EXC + 1] == "AIF_imitatedScheme", "position colonne inattendue"

    by_pk = {}
    for r in rows[1:]:
        f = split_fields(r)
        assert len(f) == 106, "rangée à %d champs" % len(f)
        by_pk[f[I_PK]] = f

    # --- gardes ---
    for pk, path in PATHS.items():
        assert by_pk[pk][I_PATH] == path, "PK %s path %r != %r" % (pk, by_pk[pk][I_PATH], path)
    for pk in by_pk:
        if by_pk[pk][I_PATH] in NAKED_HEADS:
            assert pk not in PHRASE_EDITS, "tête nue visée : PK %s" % pk
    for pk in PHRASE_EDITS:
        assert by_pk[pk][I_IMIT] == "", "anti-overwrite: PK %s imitated non vide" % pk
    # les trois nœuds du cadrage, dans l'état où il les décrit
    assert decode(by_pk["834"][I_OTH]).startswith("No native AIF Inference scheme for non-faulty comparison")
    for pk in ADJUDICATED_NO_WRITE:
        f = by_pk[pk]
        assert f[I_EXC] == "" and f[I_IMIT] == "" and f[I_OTH] == "", \
            "PK %s devrait rester sans cellule (tranché sans écriture)" % pk
    # précédent de représentation : une phrase « imite… » est déjà portée par PK 244
    assert decode(by_pk["244"][I_IMIT]).startswith("imite "), "précédent PK 244 absent"

    imit_before = sum(1 for r in rows[1:] if split_fields(r)[I_IMIT] != "")

    # --- application ---
    new_rows = [rows[0]]
    n_cells = 0
    for r in rows[1:]:
        f = split_fields(r)
        pk = f[I_PK]
        if pk in PHRASE_EDITS:
            prose = PHRASE_EDITS[pk]
            assert '"' not in prose  # une prose à guillemets demanderait un doublage
            f[I_IMIT] = '"%s"' % prose  # quotée, comme PK 244
            n_cells += 1
        new_rows.append(",".join(f))
    assert n_cells == 1, "cellules écrites %d != 1" % n_cells
    new_text = "\r\n".join(new_rows) + "\r\n"

    # --- verify (sur le texte NOUVEAU, avant toute écriture disque) ---
    vrows = split_logical_rows(new_text)
    vheader = split_fields(vrows[0])
    assert vheader == header
    vby = {split_fields(r)[I_PK]: split_fields(r) for r in vrows[1:]}
    for pk, prose in PHRASE_EDITS.items():
        assert decode(vby[pk][I_IMIT]) == prose, "%s imitated" % pk
    # toutes les AUTRES cellules decoded-equal
    o_parsed = list(csv.reader(io.StringIO(text, newline="")))
    n_parsed = list(csv.reader(io.StringIO(new_text, newline="")))
    assert len(o_parsed) == len(n_parsed) == 1409
    expected = {(pk, "AIF_imitatedScheme"): prose for pk, prose in PHRASE_EDITS.items()}
    for o_row, n_row in zip(o_parsed[1:], n_parsed[1:]):
        pk = o_row[I_PK]
        for ci, col in enumerate(header):
            want = expected.get((pk, col), o_row[ci])
            assert n_row[ci] == want, "delta inattendu PK %s %s" % (pk, col)
    imit_after = sum(1 for r in vrows[1:] if split_fields(r)[I_IMIT] != "")
    assert imit_after == imit_before + 1, "imitated %d->%d" % (imit_before, imit_after)
    # la rangée écrite est la SEULE différente, octet pour octet
    diff_rows = [i for i, (a, b) in enumerate(zip(rows, new_rows)) if a != b]
    assert diff_rows == [i for i, r in enumerate(rows) if split_fields(r)[I_PK] == "837"], \
        "rangées modifiées inattendues : %r" % diff_rows

    if write:
        tmp = CSV_PATH + ".new16"
        with open(tmp, "wb") as fh:
            fh.write(BOM + new_text.encode("utf-8"))
        os.replace(tmp, CSV_PATH)
        print("ECRIT: 1 cellule / 1 rangée (PK 837) ; imitated remplis %d->%d" % (imit_before, imit_after))
    else:
        print("DRY-RUN OK: 1 cellule / 1 rangée serait écrite (PK 837) ; "
              "imitated %d->%d" % (imit_before, imit_after))


if __name__ == "__main__":
    main()