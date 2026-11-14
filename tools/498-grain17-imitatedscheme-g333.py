# -*- coding: utf-8 -*-
"""#498 grain 17 — groupe 3.3.3 « Opération inappropriée » : DESCENTE sous la tête, enfant par enfant.

Cadrage : docs/ontology/498-grain9-cadrage-scheme-imite-2026-11-10.md (§3.3.3).
Arbitrage ai-01 du 13/11 (PR #2246, c.6850786269) : « le verdict épinglé prime » — PK 690
reste attack-columns-only (ancre RATIFIÉE, `FallacyAttackOnlyStopGateTests`), et la ligne
§3.3.3 du cadrage n'est PAS appliquée telle quelle : une référence posée sur la tête serait
héritée par ses 5 descendants, or le document de cluster
(`docs/taxonomy/498-aif-operation-inappropriee-cluster.md`) sépare le sous-arbre en deux
mécaniques. « Donc 3.3.3 descend. Les classes se proposent enfant par enfant. »

Répartition, par sous-groupe (la mécanique se pose au plus haut nœud où elle est vraie) :

  3.3.3.1 (PK 691, Sophisme mathématique)      → référence `Deductive_Inference`
        couvre aussi 692 Division par zéro et 693 Pseudo démonstration d'égalité :
        une erreur de calcul cachée corrompt l'étape d'inférence — même mécanique.
  3.3.3.2 (PK 694, Détournement de symbole)    → référence `Deductive_Inference`
        l'équivoque sur le symbole casse la dérivation (branche « corrompt
        l'inférence » du document de cluster ; tête de sous-groupe, pas de descendant).
  3.3.3.3 (PK 695, Pseudo-science)             → PHRASE (colonne `AIF_imitatedScheme`)
        « fabrique une prémisse » : elle imite l'appel à une connaissance
        scientifique établie, sans qu'AIF nomme ce scheme — aucun jeton natif
        (le fail-loud du document de cluster porte sur la couche skos, pas sur (a)).

Représentation (GO ai-01 sur #2229) : référence → `AIF_skosExceptionRef` jeton nu +
`AIF_skosMappingType='skos:broadMatch'` (doctrine grain 10 : avec une ExceptionRef la
cible est la classe des conflits du scheme, le nœud est plus étroit) ; phrase → colonne
dédiée `AIF_imitatedScheme`, quotée, français, comme PK 244/299/837.

⛔ PK 690 JAMAIS touchée : les 4 colonnes skos restent vides et les colonnes d'attaque
gardent `undercut`/`RA-node` (égalité épinglée par `FallacyAttackOnlyStopGateTests`).
⛔ 692/693 restent byte-identiques (couvertes par héritage du plus proche ancêtre porteur).

Lecture seule par défaut ; écriture uniquement avec --write. Verify AVANT l'écriture ;
re-vérification byte-exact + decoded-equal APRÈS. Base : ce checkout (master 60442818).

    python tools/498-grain17-imitatedscheme-g333.py            # dry-run
    python tools/498-grain17-imitatedscheme-g333.py --write    # écrit
"""
import io
import os
import sys

import csv  # decoded-equality only, never for writing

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # tools/x.py -> <repo>
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

BOM = b"\xef\xbb\xbf"

REF_EDITS = {  # PK -> (ExceptionRef, MappingType) — jetons nus (pas de virgule)
    "691": ("Deductive_Inference", "skos:broadMatch"),
    "694": ("Deductive_Inference", "skos:broadMatch"),
}
PHRASE_EDITS = {  # PK -> prose française (colonne AIF_imitatedScheme)
    "695": ("imite l'appel à une connaissance scientifique établie — "
            "le statut de fait scientifique est fabriqué pour tenir lieu de preuve"),
}
PATHS = {"691": "3.3.3.1", "694": "3.3.3.2", "695": "3.3.3.3"}
# 3.3 = tête de groupe hétérogène (cadrage §1) ; 3.3.3 = ancre RATIFIÉE (PK 690), non écrite.
NAKED_HEADS = ("3.3", "3.3.3")
# Ancre ratifiée : (attackType, attackedNode) — les 4 colonnes skos (dont imitated) restent vides.
ANCHOR = ("690", "undercut", "RA-node")
# Couvertes par héritage de 691 : aucune cellule propre, contrôle explicite de non-écriture.
INHERITED_UNTOUCHED = ("692", "693")

SKOS_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther",
             "AIF_skosMappingType", "AIF_imitatedScheme")


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


def census(field_rows, I):
    """Les 4 buckets de `FallacySkosCensusTests`, même précédence : mapped > gap > imitation > untouched."""
    mapped = gap = imit = untouched = 0
    for f in field_rows:
        refs = f[I["AIF_skosDirectRef"]] != "" or f[I["AIF_skosExceptionRef"]] != ""
        if refs:
            mapped += 1
        elif f[I["AIF_skosOther"]] != "":
            gap += 1
        elif f[I["AIF_imitatedScheme"]] != "":
            imit += 1
        elif all(f[I[c]] == "" for c in SKOS_COLS):
            untouched += 1
    return mapped, gap, imit, untouched


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
    I = {name: header.index(name) for name in
         ("PK", "path", "AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_imitatedScheme",
          "AIF_skosOther", "AIF_skosMappingType", "AIF_attackType", "AIF_attackedNode")}
    assert header[I["AIF_skosExceptionRef"] + 1] == "AIF_imitatedScheme", "position colonne inattendue"

    field_rows = [split_fields(r) for r in rows]
    by_pk = {f[I["PK"]]: f for f in field_rows[1:]}
    assert len(field_rows) == 1409 and len(by_pk) == 1408, "rangées %d / PK %d" % (len(field_rows), len(by_pk))

    # --- gardes ---
    for pk, path in PATHS.items():
        assert by_pk[pk][I["path"]] == path, "PK %s path %r != %r" % (pk, by_pk[pk][I["path"]], path)
    for pk in by_pk:
        if by_pk[pk][I["path"]] in NAKED_HEADS:
            assert pk not in REF_EDITS and pk not in PHRASE_EDITS, "tête nue visée : PK %s" % pk
    for pk in REF_EDITS:
        assert by_pk[pk][I["AIF_skosExceptionRef"]] == "", "anti-overwrite: PK %s Exc non vide" % pk
        assert by_pk[pk][I["AIF_skosMappingType"]] == "", "anti-overwrite: PK %s Map non vide" % pk
        assert by_pk[pk][I["AIF_imitatedScheme"]] == "", "anti-overwrite: PK %s imitated non vide" % pk
    for pk in PHRASE_EDITS:
        assert by_pk[pk][I["AIF_imitatedScheme"]] == "", "anti-overwrite: PK %s imitated non vide" % pk
        assert by_pk[pk][I["AIF_skosExceptionRef"]] == "", "anti-overwrite: PK %s Exc non vide" % pk
    # ancre ratifiée : état épinglé par FallacyAttackOnlyStopGateTests
    apk, atype, anode = ANCHOR
    assert by_pk[apk][I["AIF_attackType"]] == atype and by_pk[apk][I["AIF_attackedNode"]] == anode, \
        "ancre %s : attaque %r/%r != %r/%r" % (apk, by_pk[apk][I["AIF_attackType"]],
                                               by_pk[apk][I["AIF_attackedNode"]], atype, anode)
    for c in SKOS_COLS:
        assert by_pk[apk][I[c]] == "", "ancre %s : %s non vide — la contreviendrait au verdict" % (apk, c)
    # couvertes par héritage : aucune cellule propre, avant
    for pk in INHERITED_UNTOUCHED:
        assert all(by_pk[pk][I[c]] == "" for c in SKOS_COLS), "PK %s devrait rester sans cellule" % pk
    # précédent de représentation : une phrase « imite… » est déjà portée par PK 244/837
    assert decode(by_pk["244"][I["AIF_imitatedScheme"]]).startswith("imite "), "précédent PK 244 absent"
    assert decode(by_pk["837"][I["AIF_imitatedScheme"]]).startswith("imite "), "précédent PK 837 absent"
    # précédents de la référence : Deductive_Inference est DÉJÀ un porteur du corpus (grain 15)
    assert decode(by_pk["681"][I["AIF_skosExceptionRef"]]) == "Deductive_Inference"
    assert by_pk["681"][I["AIF_skosMappingType"]] == "skos:broadMatch"

    before = census(field_rows[1:], I)
    assert sum(before) == 1408, "census avant %r ne somme pas à 1408" % (before,)

    # --- application ---
    new_rows = [rows[0]]
    n_cells = 0
    for r, f in zip(rows[1:], field_rows[1:]):
        pk = f[I["PK"]]
        if pk in REF_EDITS:
            exc, m = REF_EDITS[pk]
            f[I["AIF_skosExceptionRef"]] = exc  # jeton nu
            f[I["AIF_skosMappingType"]] = m     # jeton nu
            n_cells += 2
        elif pk in PHRASE_EDITS:
            prose = PHRASE_EDITS[pk]
            assert '"' not in prose  # une prose à guillemets demanderait un doublage
            f[I["AIF_imitatedScheme"]] = '"%s"' % prose  # quotée, comme PK 244/837
            n_cells += 1
        new_rows.append(",".join(f))
    assert n_cells == 5, "cellules écrites %d != 5" % n_cells
    new_text = "\r\n".join(new_rows) + "\r\n"
    assert new_text.count("\r\n") == text.count("\r\n"), "CRLF modifiés"

    # --- verify (sur le texte NOUVEAU, avant toute écriture disque) ---
    vrows = split_logical_rows(new_text)
    vheader = split_fields(vrows[0])
    assert vheader == header
    vfields = [split_fields(r) for r in vrows[1:]]
    vby = {f[I["PK"]]: f for f in vfields}
    for pk, (exc, m) in REF_EDITS.items():
        assert vby[pk][I["AIF_skosExceptionRef"]] == exc, "%s Exc" % pk
        assert decode(vby[pk][I["AIF_skosMappingType"]]) == m, "%s Map" % pk
    for pk, prose in PHRASE_EDITS.items():
        assert decode(vby[pk][I["AIF_imitatedScheme"]]) == prose, "%s imitated" % pk
    # toutes les AUTRES cellules decoded-equal
    o_parsed = list(csv.reader(io.StringIO(text, newline="")))
    n_parsed = list(csv.reader(io.StringIO(new_text, newline="")))
    assert len(o_parsed) == len(n_parsed) == 1409
    expected = {}
    for pk, (exc, m) in REF_EDITS.items():
        expected[(pk, "AIF_skosExceptionRef")] = exc
        expected[(pk, "AIF_skosMappingType")] = m
    for pk, prose in PHRASE_EDITS.items():
        expected[(pk, "AIF_imitatedScheme")] = prose
    for o_row, n_row in zip(o_parsed[1:], n_parsed[1:]):
        pk = o_row[I["PK"]]
        for ci, col in enumerate(header):
            want = expected.get((pk, col), o_row[ci])
            assert n_row[ci] == want, "delta inattendu PK %s %s" % (pk, col)
    # les 3 rangées cibles sont les SEULES différentes, octet pour octet
    diff_rows = [i for i, (a, b) in enumerate(zip(rows, new_rows)) if a != b]
    targets = {i for i, f in enumerate(field_rows) if f[I["PK"]] in set(REF_EDITS) | set(PHRASE_EDITS)}
    assert diff_rows == sorted(targets), "rangées modifiées inattendues : %r != %r" % (diff_rows, sorted(targets))
    # 692/693 byte-identiques
    for pk in INHERITED_UNTOUCHED:
        assert by_pk[pk] == vby[pk], "PK %s a bougé (héritage attendu, pas d'écriture)" % pk
    # ancre intacte après
    for c in SKOS_COLS:
        assert vby[apk][I[c]] == "", "ancre %s : %s écrite" % (apk, c)

    after = census(vfields, I)
    assert sum(after) == 1408, "census après %r ne somme pas à 1408" % (after,)
    assert after[0] == before[0] + 2, "mapped %d->%d (attendu +2)" % (before[0], after[0])
    assert after[2] == before[2] + 1, "declared-imitation %d->%d (attendu +1)" % (before[2], after[2])
    assert after[3] == before[3] - 3, "untouched %d->%d (attendu -3)" % (before[3], after[3])

    if write:
        tmp = CSV_PATH + ".new17"
        with open(tmp, "wb") as fh:
            fh.write(BOM + new_text.encode("utf-8"))
        os.replace(tmp, CSV_PATH)
        print("ECRIT: 5 cellules / 3 rangées (691, 694 ref ; 695 phrase).")
    else:
        print("DRY-RUN OK: 5 cellules / 3 rangées seraient écrites (691, 694 ref ; 695 phrase).")
    print("census avant: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (before + (sum(before),)))
    print("census après: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (after + (sum(after),)))
    print("ancre 690: %s/%s, 5 colonnes skos vides (inchangee)" % (atype, anode))


if __name__ == "__main__":
    main()