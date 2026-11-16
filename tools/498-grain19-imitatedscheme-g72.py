# -*- coding: utf-8 -*-
"""#498 grain 19 — groupe 7.2 « Sabotage du débat » (partie 7.2.2/7.2.3) : 2 références + 2 phrases, 4 rangées.

Cadrage : docs/ontology/498-grain9-cadrage-scheme-imite-2026-11-10.md (§7.2.2, §7.2.3,
partition §4). Base : origin/master 64307b98. Sans arbitrage nouveau : le grain applique
les tables du cadrage telles quelles.

Re-dérivation du périmètre (définition (a) du cadrage du 06/11 : AIF_skosExceptionRef
non vide, OU AIF_skosDirectRef avec cible en _Inference/_Scheme, héritée du plus proche
ancêtre porteur) : 48 nœuds dans le sous-arbre 7.2, dont 16 sans-(a) — la tête 1312 (7.2,
groupe hétérogène), la tête intermédiaire 1345 (7.2.2, enfants hétérogènes), 7.2.2.1
(1346+1347+1348+1349), 7.2.2.2 (1350), 7.2.2.3 (1351) et 7.2.3 (1352+1353…1359).
Les 4 écritures couvrent 14 nœuds ; restent sans écriture VOLONTAIRE les deux têtes
1312 et 1345 (cadrage §4 : « même statut que les têtes de groupe »).

  PK 1346  7.2.2.1 (4 n.) → PHRASE « imite la rigueur méthodique — la complication du
                          protocole tient lieu d'examen »
  PK 1350  7.2.2.2 (1 n.) → référence PracticalReasoning_Inference (porteurs 697/977 :
                          le nirvana exige l'option parfaite — jumeau du PK 977, §6.2.1)
  PK 1351  7.2.2.3 (1 n.) → PHRASE « imite la réfutation par détail — un défaut mineur
                          tient lieu de réfutation du tout »
  PK 1352  7.2.3 tête (8) → référence Ethotic_Inference (porteur 7.3/1360 : discréditer
                          la source à l'avance imite l'examen éthique légitime ; le
                          « +liens transverses: ad hominem » de la rangée corrobore)

Confrontation phrases↔cartes (règle ai-01 du 15/11, c.6894233749 : chaque phrase du
cadrage se confronte à desc_fr/desc_en AVANT écriture — mesuré à la base 64307b98) :
  1346 « critique disproportionnée pour embarrasser… éviter le fond » / « criticizing
       disproportionately… avoid the substance » — la phrase rend le mécanisme (le fonds
       est évité par un protocole compliqué qui se donne pour examen rigoureux). FIDÈLE.
  1351 « objections mineures pour affaiblir les arguments » / « raising minor objections
       to undermine significant arguments » — la phrase dit exactement cela (le détail
       mineur tenu pour réfutation du tout). FIDÈLE.
Aucune correction de phrase nécessaire ce grain (différence d'avec 984/991, 15/11).

Ancre 1352 : la rangée porte AIF_attackType=undermine + AIF_attackedNode=I-node
(aif-attack-edges.csv l.142) et « +liens transverses: ad hominem ». Le cadrage §7.2.3
pose la référence (a) SUR la tête (8 nœuds couverts par héritage) — les colonnes
d'attaque et tout le reste de la rangée restent byte-identiques (seules les deux
cellules (a) vides sont remplies). Idem 1345 : undercut/RA-node épinglé, non écrite.

Représentation (GO ai-01 sur #2229, même que grain 18) : référence → AIF_skosExceptionRef
jeton nu + AIF_skosMappingType='skos:broadMatch' ; phrase → colonne dédiée
AIF_imitatedScheme, quotée, français.

Lecture seule par défaut ; écriture uniquement avec --write. Verify AVANT l'écriture ;
re-vérification byte-exact + decoded-equal APRÈS. Anti-drift : chaque cellule cible
n'accepte QUE l'état vide de la base — un re-jeu sur CSV déjà écrit échoue (contrôle
négatif préservé).

    python tools/498-grain19-imitatedscheme-g72.py            # dry-run
    python tools/498-grain19-imitatedscheme-g72.py --write    # écrit
"""
import io
import os
import sys

import csv  # decoded-equality only, never for writing

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # tools/x.py -> <repo>
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

BOM = b"\xef\xbb\xbf"

REF_EDITS = {  # PK -> (ExceptionRef, MappingType) — jetons nus (pas de virgule)
    "1350": ("PracticalReasoning_Inference", "skos:broadMatch"),  # 7.2.2.2
    "1352": ("Ethotic_Inference", "skos:broadMatch"),             # 7.2.3 tête
}
PHRASE_EDITS = {  # PK -> prose française (colonne AIF_imitatedScheme), cadrage §7.2 verbatim
    "1346": ("imite la rigueur méthodique — la complication du protocole tient lieu "
             "d'examen"),                                          # 7.2.2.1
    "1351": ("imite la réfutation par détail — un défaut mineur tient lieu "
             "de réfutation du tout"),                             # 7.2.2.3
}
PATHS = {
    "1346": "7.2.2.1", "1350": "7.2.2.2", "1351": "7.2.2.3", "1352": "7.2.3",
}
# Têtes non écrites (cadrage §4) : 1312 tête de groupe hétérogène, 1345 tête
# intermédiaire 7.2.2 (enfants hétérogènes — même statut que 2.1.2/2.3.2/6.2.3).
NAKED_HEADS = ("7.2", "7.2.2")
NAKED_HEAD_PKS = ("1312", "1345")
# Ancres d'attaque épinglées (jamais touchées par un write (a)) : 1345 et 1352.
ANCHOR_PINS = {
    "1345": ("undercut", "RA-node"),   # aif-attack-edges + colonnes AIF_attackType/attackedNode
    "1352": ("undermine", "I-node"),
}
# Couvertes par héritage (écriture d'un ancêtre) : aucune cellule propre, contrôle
# explicite de non-écriture — tout le reste du sous-arbre sans-(a).
INHERITED_UNTOUCHED = ("1347", "1348", "1349",                       # sous 1346
                       "1353", "1354", "1355", "1356", "1357", "1358", "1359")  # sous 1352
# Périmètre re-dérivé attendu : les 16 sans-(a) de 7.2 à la base (anti-dérive path).
EXPECTED_SANS_A = frozenset(NAKED_HEAD_PKS + ("1346", "1347", "1348", "1349", "1350",
                                              "1351", "1352", "1353", "1354", "1355",
                                              "1356", "1357", "1358", "1359"))

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

    # --- périmètre re-dérivé : les sans-(a) de 7.2 ---
    def path_of(f):
        return f[I["path"]]

    by_path = {path_of(f): f for f in field_rows[1:]}
    subtree = [f for f in field_rows[1:] if path_of(f) == "7.2" or path_of(f).startswith("7.2.")]
    assert len(subtree) == 48, "sous-arbre 7.2 : %d != 48" % len(subtree)

    def has_direct_a(f):
        v = decode(f[I["AIF_skosDirectRef"]])
        return v.endswith("_Inference") or v.endswith("_Scheme")

    def ancestor_a(f):
        parts = path_of(f).split(".")
        for k in range(len(parts) - 1, 0, -1):
            anc = by_path.get(".".join(parts[:k]))
            if anc is None:
                continue
            if decode(anc[I["AIF_skosExceptionRef"]]) != "" or has_direct_a(anc):
                return True
        return False

    sans_a = {f[I["PK"]] for f in subtree
              if not (decode(f[I["AIF_skosExceptionRef"]]) != "" or has_direct_a(f) or ancestor_a(f))}
    assert sans_a == EXPECTED_SANS_A, "sans-(a) re-dérivés %r != attendus %r" % (
        sorted(map(int, sans_a - EXPECTED_SANS_A)) + sorted(map(int, EXPECTED_SANS_A - sans_a)), None)

    # --- gardes ---
    for pk, path in PATHS.items():
        assert by_pk[pk][I["path"]] == path, "PK %s path %r != %r" % (pk, by_pk[pk][I["path"]], path)
    for f in field_rows[1:]:
        if path_of(f) in NAKED_HEADS:
            pk = f[I["PK"]]
            assert pk in NAKED_HEAD_PKS, "tête nue inattendue : %s" % pk
            assert pk not in REF_EDITS and pk not in PHRASE_EDITS, "tête nue visée : PK %s" % pk
    for pk, (exc, m) in REF_EDITS.items():
        cur_exc = decode(by_pk[pk][I["AIF_skosExceptionRef"]])
        cur_mt = decode(by_pk[pk][I["AIF_skosMappingType"]])
        if not (cur_exc == "" and cur_mt == ""):
            raise AssertionError("anti-drift: PK %s Exc/Map %r/%r — cellule non vide, base attendue vide" % (pk, cur_exc, cur_mt))
        assert by_pk[pk][I["AIF_imitatedScheme"]] == "", "anti-overwrite: PK %s imitated non vide" % pk
    for pk, prose in PHRASE_EDITS.items():
        cur = decode(by_pk[pk][I["AIF_imitatedScheme"]])
        if cur != "":
            raise AssertionError("anti-drift: PK %s imitated %r non vide (re-jeu interdit)" % (pk, cur))
        assert by_pk[pk][I["AIF_skosExceptionRef"]] == "", "anti-overwrite: PK %s Exc non vide" % pk
    # ancres d'attaque épinglées : 1345/1352 portent leurs colonnes d'attaque, jamais (a)
    for pk, (at, an) in ANCHOR_PINS.items():
        f = by_pk[pk]
        assert f[I["AIF_attackType"]] == at and f[I["AIF_attackedNode"]] == an, \
            "ancre %s : attaque %r/%r != %r/%r" % (pk, f[I["AIF_attackType"]], f[I["AIF_attackedNode"]], at, an)
        for c in SKOS_COLS:
            assert f[I[c]] == "", "ancre %s : %s non vide" % (pk, c)
    # têtes nues : rien d'écrit, avant
    for pk in NAKED_HEAD_PKS:
        assert all(by_pk[pk][I[c]] == "" for c in SKOS_COLS), "PK %s (tête nue) : colonne skos non vide" % pk
    # couvertes par héritage : aucune cellule propre, avant
    for pk in INHERITED_UNTOUCHED:
        assert all(by_pk[pk][I[c]] == "" for c in SKOS_COLS), "PK %s devrait rester sans cellule" % pk
    # précédent de représentation : une phrase « imite… » est déjà portée par PK 244/837/695
    for ppk in ("244", "837", "695"):
        assert decode(by_pk[ppk][I["AIF_imitatedScheme"]]).startswith("imite "), "précédent PK %s absent" % ppk
    # précédents de la référence : chaque IRI est DÉJÀ un porteur du corpus (jeton nu + broadMatch)
    for ppk, tok in (("697", "PracticalReasoning_Inference"),
                     ("1360", "Ethotic_Inference")):
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
    assert n_cells == 6, "cellules écrites %d != 6" % n_cells
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
    # les 4 rangées cibles sont les SEULES différentes, octet pour octet
    diff_rows = [i for i, (a, b) in enumerate(zip(rows, new_rows)) if a != b]
    targets = {i for i, f in enumerate(field_rows) if f[I["PK"]] in set(REF_EDITS) | set(PHRASE_EDITS)}
    assert diff_rows == sorted(targets), "rangées modifiées inattendues : %r != %r" % (diff_rows, sorted(targets))
    # héritage : les descendants restent byte-identiques
    for pk in INHERITED_UNTOUCHED:
        assert by_pk[pk] == vby[pk], "PK %s a bougé (héritage attendu, pas d'écriture)" % pk
    # têtes et ancres intactes après (rangées byte-identiques SAUF les cellules (a) posées)
    for pk in NAKED_HEAD_PKS:
        assert vby[pk] == by_pk[pk], "PK %s a bougé (tête nue non écrite)" % pk
    for pk in ANCHOR_PINS:
        changed = [header[i] for i, (a, b) in enumerate(zip(by_pk[pk], vby[pk])) if a != b]
        assert set(changed) <= {"AIF_skosExceptionRef", "AIF_skosMappingType"}, \
            "ancre %s : colonnes bougées %r" % (pk, changed)
        assert vby[pk][I["AIF_attackType"]] == by_pk[pk][I["AIF_attackType"]], "ancre %s attackType" % pk
        assert vby[pk][I["AIF_attackedNode"]] == by_pk[pk][I["AIF_attackedNode"]], "ancre %s attackedNode" % pk

    after = census(vfields, I)
    assert sum(after) == 1408, "census après %r ne somme pas à 1408" % (after,)
    assert after[0] == before[0] + 2, "mapped %d->%d (attendu +2)" % (before[0], after[0])
    assert after[2] == before[2] + 2, "declared-imitation %d->%d (attendu +2)" % (before[2], after[2])
    assert after[3] == before[3] - 4, "untouched %d->%d (attendu -4)" % (before[3], after[3])
    assert after[1] == before[1], "gap %d->%d (attendu inchangé)" % (before[1], after[1])

    if write:
        tmp = CSV_PATH + ".new19"
        with open(tmp, "wb") as fh:
            fh.write(BOM + new_text.encode("utf-8"))
        os.replace(tmp, CSV_PATH)
        print("ECRIT: 6 cellules / 4 rangées (2 ref: 1350 PracticalReasoning, 1352 Ethotic ; 2 phrases: 1346, 1351).")
    else:
        print("DRY-RUN OK: 6 cellules / 4 rangées seraient écrites (2 ref ; 2 phrases).")
    print("census avant: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (before + (sum(before),)))
    print("census après: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (after + (sum(after),)))
    print("têtes non écrites: 1312 (7.2, groupe), 1345 (7.2.2, intermédiaire, ancre undercut/RA-node intacte)")
    print("ancre 1352: undermine/I-node + liens transverses intacts, seules les cellules (a) posées")


if __name__ == "__main__":
    main()
