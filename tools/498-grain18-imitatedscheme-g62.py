# -*- coding: utf-8 -*-
"""#498 grain 18 — groupe 6.2 « Changement de cap » : 6 références + 6 phrases, 12 rangées.

Cadrage : docs/ontology/498-grain9-cadrage-scheme-imite-2026-11-10.md (§6.2.1, §6.2.2,
§6.2.3, partition §4). Base : origin/master 5cc2e4f0. Sans arbitrage nouveau : le grain
applique les tables du cadrage telles quelles (aucune descente à trancher ici).

Re-dérivation du périmètre (définition (a) du cadrage du 06/11 : ExceptionRef non vide,
OU DirectRef avec cible en _Inference/_Scheme, héritée du plus proche ancêtre porteur) :
49 nœuds sans-(a) dans 6.2 — la tête 973 (1) + 6.2.1 (18) + 6.2.2 (20) + 6.2.3 (10).
Couverture existante : 1020 (ER Waste_Inference) + 1021 (héritée). Les 12 écritures
couvrent 47 nœuds ; restent sans écriture VOLONTAIRE les deux têtes 973 et 1011.

Répartition (la mécanique se pose au plus haut nœud où elle est vraie) :

  6.2.1 Exigence renforcée (18 sans-(a) couverts par 7 écritures) :
    PK 974  tête        → PHRASE « imite la fixation légitime d'un standard de preuve
                          avant acceptation — l'exigence est déplacée après coup »
                          (AIF n'a pas de scheme de charge de preuve)
    PK 975  .1 (7 n.)   → PHRASE « imite l'examen critique exhaustif — l'itération sans
                          fin d'objections tient lieu de réfutation »
    PK 984  .2 (3 n.)   → PHRASE « imite la réfutation — une explication concurrente est
                          présentée comme une défaite de la thèse »
    PK 987  .3 (2 n.)   → référence EvidenceToHypothesis_Inference (porteurs 3.1/3.2 :
                          exiger un seul type de preuve imite la convergence des preuves)
    PK 989  .4 (2 n.)   → référence Ignorance_Inference (porteur 1.1.1.1 : renverser la
                          charge = absence de preuve tenue pour preuve d'absence)
    PK 977  .1.1.1.1(2) → référence PracticalReasoning_Inference (porteurs 1.2/4.1 : le
                          nirvana exige l'option parfaite — jumeau de 7.2.2.2, §7.2.2)
    PK 991  .5 (1 n.)   → PHRASE « imite la précision technique — l'obscurité
                          terminologique tient lieu de rigueur »
  6.2.2 Beurre et argent du beurre (20 sans-(a), UNE écriture) :
    PK 992  tête        → référence Commitment_Inference (porteurs 4.3.2/7.3.1/1.3.2 :
                          tout le sous-arbre dévoie l'engagement de position)
  6.2.3 Exigence relâchée (9 écritures couvrent 9 sans-(a) ; tête non écrite) :
    PK 1012 .1 (1 n.)   → référence Ignorance_Inference (le silence comme preuve)
    PK 1013 .2 (2 n.)   → PHRASE « imite la tolérance aux incohérences mineures — tout
                          admettre tient lieu de souplesse »
    PK 1015 .3 (5 n.)   → référence Waste_Inference (porteur le frère 6.2.3.4/1020 :
                          « on a tant investi » comme raison de continuer)
    PK 1022 .5 (1 n.)   → PHRASE « imite la pertinence de l'ancienneté — l'âge d'un
                          fait tient lieu d'objection »

Représentation (GO ai-01 sur #2229) : référence → AIF_skosExceptionRef jeton nu +
AIF_skosMappingType='skos:broadMatch' (doctrine grain 10 : avec une ExceptionRef la cible
est la classe des conflits du scheme, le nœud est plus étroit) ; phrase → colonne dédiée
AIF_imitatedScheme, quotée, français, comme PK 244/299/837/695.

⛔ PK 973 (tête de groupe hétérogène) JAMAIS touchée : elle porte déjà son mapping (c)
  (DirectRef=ConflictingGoals_Conflict + relatedMatch + undermine/I-node) — les colonnes
  (a) (ExceptionRef/imitatedScheme/skosOther) restent vides, tout le reste byte-identique.
⛔ PK 1011 (tête intermédiaire 6.2.3, enfants hétérogènes) non écrite : 5 colonnes skos
  vides + attack undercut/RA-node, inchangés (même statut que 2.1.2/2.3.2/7.2.2, §4).
⛔ 1020/1021 byte-identiques (couverture (a) existante du sous-arbre 6.2.3.4).

Lecture seule par défaut ; écriture uniquement avec --write. Verify AVANT l'écriture ;
re-vérification byte-exact + decoded-equal APRÈS. Base : ce checkout (origin/master
5cc2e4f0).

    python tools/498-grain18-imitatedscheme-g62.py            # dry-run
    python tools/498-grain18-imitatedscheme-g62.py --write    # écrit
"""
import io
import os
import sys

import csv  # decoded-equality only, never for writing

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # tools/x.py -> <repo>
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

BOM = b"\xef\xbb\xbf"

REF_EDITS = {  # PK -> (ExceptionRef, MappingType) — jetons nus (pas de virgule)
    "987": ("EvidenceToHypothesis_Inference", "skos:broadMatch"),   # 6.2.1.3
    "989": ("Ignorance_Inference", "skos:broadMatch"),              # 6.2.1.4
    "977": ("PracticalReasoning_Inference", "skos:broadMatch"),     # 6.2.1.1.1.1
    "992": ("Commitment_Inference", "skos:broadMatch"),             # 6.2.2 tête
    "1012": ("Ignorance_Inference", "skos:broadMatch"),             # 6.2.3.1
    "1015": ("Waste_Inference", "skos:broadMatch"),                 # 6.2.3.3
}
PHRASE_EDITS = {  # PK -> prose française (colonne AIF_imitatedScheme), cadrage §6.2 verbatim
    "974": ("imite la fixation légitime d'un standard de preuve avant acceptation — "
            "l'exigence est déplacée après coup"),                                  # 6.2.1 tête
    "975": ("imite l'examen critique exhaustif — l'itération sans fin d'objections "
            "tient lieu de réfutation"),                                            # 6.2.1.1
    "984": ("imite la réfutation — une explication concurrente est présentée comme "
            "une défaite de la thèse"),                                             # 6.2.1.2
    "991": ("imite la précision technique — l'obscurité terminologique tient lieu "
            "de rigueur"),                                                          # 6.2.1.5
    "1013": ("imite la tolérance aux incohérences mineures — tout admettre tient "
             "lieu de souplesse"),                                                  # 6.2.3.2
    "1022": ("imite la pertinence de l'ancienneté — l'âge d'un fait tient lieu "
             "d'objection"),                                                        # 6.2.3.5
}
PATHS = {
    "974": "6.2.1", "975": "6.2.1.1", "984": "6.2.1.2", "987": "6.2.1.3",
    "989": "6.2.1.4", "991": "6.2.1.5", "977": "6.2.1.1.1.1", "992": "6.2.2",
    "1012": "6.2.3.1", "1013": "6.2.3.2", "1015": "6.2.3.3", "1022": "6.2.3.5",
}
# Têtes non écrites (cadrage §4) : 973 tête de groupe hétérogène (mais déjà (c)-mappée),
# 1011 tête intermédiaire 6.2.3 (enfants hétérogènes).
NAKED_HEADS = ("6.2", "6.2.3")
# État (c) épinglé de 973 — mapping DirectRef préexistant, préservé byte-exact.
HEAD_C_MAPPING = ("973", "ConflictingGoals_Conflict", "skos:relatedMatch", "undermine", "I-node")
# Couvertes par héritage (écriture d'un ancêtre ou porteur existant) : aucune cellule
# propre, contrôle explicite de non-écriture — un enfant par référence posée.
INHERITED_UNTOUCHED = ("976", "993", "978", "1016", "1021")
# 1020 = porteur (a) existant du sous-arbre 6.2.3.4 : ER Waste_Inference déjà en place.
EXISTING_CARRIER = ("1020", "Waste_Inference")

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
    # tête 973 : mapping (c) préexistant épinglé — aucun write (a) ne le touche
    hpk, hdr, hmt, hat, han = HEAD_C_MAPPING
    assert decode(by_pk[hpk][I["AIF_skosDirectRef"]]) == hdr, "973 DirectRef inattendu"
    assert decode(by_pk[hpk][I["AIF_skosMappingType"]]) == hmt, "973 MappingType inattendu"
    assert by_pk[hpk][I["AIF_attackType"]] == hat and by_pk[hpk][I["AIF_attackedNode"]] == han, \
        "973 attaque %r/%r != %r/%r" % (by_pk[hpk][I["AIF_attackType"]],
                                        by_pk[hpk][I["AIF_attackedNode"]], hat, han)
    for c in ("AIF_skosExceptionRef", "AIF_imitatedScheme", "AIF_skosOther"):
        assert by_pk[hpk][I[c]] == "", "973 : %s non vide — tête hétérogène, pas de (a)" % c
    # tête intermédiaire 1011 : 5 colonnes skos vides + ancre d'attaque, rien n'écrit
    for c in SKOS_COLS:
        assert by_pk["1011"][I[c]] == "", "1011 : %s non vide (tête intermédiaire)" % c
    assert by_pk["1011"][I["AIF_attackType"]] == "undercut" and by_pk["1011"][I["AIF_attackedNode"]] == "RA-node", \
        "1011 : colonnes d'attaque inattendues"
    # couvertes par héritage : aucune cellule propre, avant
    for pk in INHERITED_UNTOUCHED:
        assert all(by_pk[pk][I[c]] == "" for c in SKOS_COLS), "PK %s devrait rester sans cellule" % pk
    # porteur existant 1020 : Waste_Inference déjà en place (anti-drift)
    cpk, cer = EXISTING_CARRIER
    assert decode(by_pk[cpk][I["AIF_skosExceptionRef"]]) == cer, "1020 : porteur Waste absent"
    assert decode(by_pk[cpk][I["AIF_skosMappingType"]]) == "skos:broadMatch", "1020 : MT inattendu"
    # précédent de représentation : une phrase « imite… » est déjà portée par PK 244/837/695
    for ppk in ("244", "837", "695"):
        assert decode(by_pk[ppk][I["AIF_imitatedScheme"]]).startswith("imite "), "précédent PK %s absent" % ppk
    # précédents de la référence : chaque IRI est DÉJÀ un porteur du corpus (jeton nu + broadMatch)
    for ppk, tok in (("632", "EvidenceToHypothesis_Inference"), ("750", "Ignorance_Inference"),
                     ("697", "PracticalReasoning_Inference"), ("153", "Commitment_Inference"),
                     ("432", "Waste_Inference")):
        assert decode(by_pk[ppk][I["AIF_skosExceptionRef"]]) == tok, "porteur %s != %s" % (ppk, tok)
        assert decode(by_pk[ppk][I["AIF_skosMappingType"]]) == "skos:broadMatch", "porteur %s MT" % ppk

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
    assert n_cells == 18, "cellules écrites %d != 18" % n_cells
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
    # les 12 rangées cibles sont les SEULES différentes, octet pour octet
    diff_rows = [i for i, (a, b) in enumerate(zip(rows, new_rows)) if a != b]
    targets = {i for i, f in enumerate(field_rows) if f[I["PK"]] in set(REF_EDITS) | set(PHRASE_EDITS)}
    assert diff_rows == sorted(targets), "rangées modifiées inattendues : %r != %r" % (diff_rows, sorted(targets))
    # héritage : les descendants et le porteur existant restent byte-identiques
    for pk in INHERITED_UNTOUCHED:
        assert by_pk[pk] == vby[pk], "PK %s a bougé (héritage attendu, pas d'écriture)" % pk
    assert by_pk[cpk] == vby[cpk], "PK %s (porteur existant) a bougé" % cpk
    # têtes intactes après
    assert vby[hpk] == by_pk[hpk], "973 a bougé — le mapping (c) devait rester byte-exact"
    assert vby["1011"] == by_pk["1011"], "1011 a bougé (tête intermédiaire non écrite)"

    after = census(vfields, I)
    assert sum(after) == 1408, "census après %r ne somme pas à 1408" % (after,)
    assert after[0] == before[0] + 6, "mapped %d->%d (attendu +6)" % (before[0], after[0])
    assert after[2] == before[2] + 6, "declared-imitation %d->%d (attendu +6)" % (before[2], after[2])
    assert after[3] == before[3] - 12, "untouched %d->%d (attendu -12)" % (before[3], after[3])

    if write:
        tmp = CSV_PATH + ".new18"
        with open(tmp, "wb") as fh:
            fh.write(BOM + new_text.encode("utf-8"))
        os.replace(tmp, CSV_PATH)
        print("ECRIT: 18 cellules / 12 rangées (6 ref: 987/989/977/992/1012/1015 ; 6 phrases: 974/975/984/991/1013/1022).")
    else:
        print("DRY-RUN OK: 18 cellules / 12 rangées seraient écrites (6 ref ; 6 phrases).")
    print("census avant: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (before + (sum(before),)))
    print("census après: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (after + (sum(after),)))
    print("têtes non écrites: 973 (mapping (c) %s/%s intact), 1011 (5 skos vides, %s/%s)"
          % (hdr, hmt, "undercut", "RA-node"))


if __name__ == "__main__":
    main()
