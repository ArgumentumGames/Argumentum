# -*- coding: utf-8 -*-
"""#498 grain 20 — groupe 7.2 « Sabotage du débat », partie 7.2.3 « Empoisonner le puits » :
DESCENTE ENFANT PAR ENFANT — 3 références + 4 phrases, 7 rangées ; l'ancre 1352 JAMAIS écrite.

Arbitrage ai-01 du 16/11 (P24-28, dashboard) : « (a) enfant par enfant, 1352 jamais écrite.
1353/1355 (+1356 si vrai) = disqualifier la source, jeton à choisir et justifier entre
Ethotic_Inference (1360) et DirectAdHominem_Inference (1398). 1354 et 1357 (1358, 1359) :
chacun sa classe d'après sa carte (référence > phrase > lacune). » Base : origin/master
554aa80a (grain 19 mergé).

POURQUOI 1352 N'EST PAS ÉCRITE (verdict ratifié, refus CI du 16/11) : la tête 1352 est une
ancre attaque-seule épinglée par FallacyAttackOnlyStopGateTests ; le doc
docs/taxonomy/498-aif-empoisonner-le-puits-cluster.md la sérialise attack-columns-only et
flaggue l'état skos vide comme « modelling gap » (§5). Même traitement que PK 690 au grain
17 / 3.3.3 : la TÊTE reste sans-(a), les DESCENDANTS reçoivent chacun leur représentation
(précédent de descente 3.3.3, c.6850786269 ; ici arbitrage P24-28).

JETON RETENU POUR 1353/1355/1356 : Ethotic_Inference (mesuré porteur : PK 1360 « Ad
hominem », 7.3, ExceptionRef + skos:broadMatch). Motifs, tous MESURÉS à la base :
  - 1398 « Attaque personnelle » porte DirectAdHominem_Inference (DirectRef + broadMatch),
    avec carte « vous attaquez DIRECTEMENT la personne » ; or 1353 dit elle-même « sans la
    nommer DIRECTEMENT » (insinuation), 1355 est une attribution détournée, 1356 un
    retournement de rôles — aucun n'est l'attaque frontale de 1398 ;
  - les trois cartes disqualifient la SOURCE par son ethos (faute insinuée, défauts
    attribués, crédibilité de la victime) : c'est le scheme dont l'abus est l'ad hominem —
    Ethotic_Inference, celui que le corpus emploie pour la tête de famille 7.3 (1360) ;
  - l'argument du « jumeau » 1402 (Diffamation) avancé par le doc cluster est
    PROPOSITIONNEL : mesuré, 1402 ne porte AUCUN jeton (5 cellules skos vides). Il ne peut
    donc fonder le choix de DirectAdHominem_Inference — écart documenté, arbitrage rendu.
  Le cadrage §7.2.3 proposait déjà Ethotic_Inference pour ce sous-arbre (son écriture sur
    la TÊTE est caduque face au verdict ; son argument « l'ethos waltonien » reste juste
    pour les enfants).

CLASSES DES 4 AUTRES, d'après leur carte (règle « référence > phrase > lacune » ; inventaire
des 68 jetons du corpus mesuré — aucun jeton natif ne les couvre, 0 fabrication #677) :
  1354 Convaincre le public plutôt que l'adversaire → PHRASE (audience appeal ; le seul
       candidat voisin, PopularOpinion_Inference, a pour prémisse l'acceptation populaire
       comme preuve — absente de la carte : mis-fit refusé).
  1357 Arguments factices → PHRASE (straw man ; aucun jeton, corroboré par le finding du
       doc #845 pour son jumeau 1365 « non sérialisé »).
  1358 Utiliser des arguments absurdes → PHRASE (idée absurde en guise d'épreuve).
  1359 Répondre par de mauvais arguments → PHRASE (contre-argumentation en guise de
       réfutation ; aucun jeton two-wrongs dans l'inventaire).
Le finding §4 du doc cluster (les 4 rangées sont hors-mécanisme, reclassification GATED
jsboige) n'est PAS touché : ce grain n'écrit que la couche (a) de chaque mécanisme tel que
sa carte le décrit, sans valider le regroupement.

Confrontation phrases ↔ cartes (règle ai-01 du 15/11, c.6894233749) : chaque phrase est
re-dérivée de desc_fr/desc_en de SA rangée, mesurée à la base — voir PHRASE_EDITS ci-dessous.

Ancres / têtes jamais écrites, contrôlées byte-identiques avant ET après :
  1352 (undermine/I-node, verdict ratifié, skos vides pour toujours),
  1345 (undercut/RA-node, ancre ratifiée 7.2.2), 1312 (7.2, tête de groupe hétérogène).

Représentation (GO ai-01 sur #2229, même que grains 18-19) : référence → AIF_skosExceptionRef
jeton nu + AIF_skosMappingType='skos:broadMatch' ; phrase → colonne dédiée
AIF_imitatedScheme, quotée, français, « imite … ».

Lecture seule par défaut ; écriture uniquement avec --write. Anti-drift : chaque cellule
cible n'accepte QUE l'état vide de la base — un re-jeu sur CSV déjà écrit échoue (contrôle
négatif préservé).

    python tools/498-grain20-imitatedscheme-g723.py            # dry-run
    python tools/498-grain20-imitatedscheme-g723.py --write    # écrit
"""
import io
import os
import sys

import csv  # decoded-equality only, never for writing

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))  # tools/x.py -> <repo>
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

BOM = b"\xef\xbb\xbf"

REF_EDITS = {  # PK -> (ExceptionRef, MappingType) — jetons nus (pas de virgule)
    "1353": ("Ethotic_Inference", "skos:broadMatch"),  # 7.2.3.1 insinuation
    "1355": ("Ethotic_Inference", "skos:broadMatch"),  # 7.2.3.3 projection
    "1356": ("Ethotic_Inference", "skos:broadMatch"),  # 7.2.3.3.1 DARVO
}
PHRASE_EDITS = {  # PK -> prose (AIF_imitatedScheme), re-dérivée de la carte de SA rangée
    "1354": ("imite le débat public — influencer l'auditoire tient lieu de convaincre "
             "l'interlocuteur"),                                       # 7.2.3.2 audience
    "1357": ("imite la réfutation — des arguments fabriqués tiennent lieu de thèse "
             "adverse"),                                              # 7.2.3.4 straw man
    "1358": ("imite la mise à l'épreuve de la position adverse — une idée absurde "
             "tient lieu de test"),                                   # 7.2.3.4.1 absurdité
    "1359": ("imite la réfutation — une contre-argumentation tout aussi discutable "
             "tient lieu de réponse"),                                # 7.2.3.4.2 mauvais contre-args
}
PATHS = {
    "1353": "7.2.3.1", "1354": "7.2.3.2", "1355": "7.2.3.3", "1356": "7.2.3.3.1",
    "1357": "7.2.3.4", "1358": "7.2.3.4.1", "1359": "7.2.3.4.2",
}
# Têtes non écrites : 1312 (7.2, groupe hétérogène) et 1345 (7.2.2, intermédiaire).
NAKED_HEADS = ("7.2", "7.2.2")
NAKED_HEAD_PKS = ("1312", "1345")
# Ancres d'attaque RATIFIÉES (jamais touchées par un write (a), skos vides pour toujours) :
# 1345 = 498-aif-complication-exageree-cluster.md ; 1352 = 498-aif-empoisonner-le-puits-
# cluster.md (tête du sous-arbre de ce grain — verdict final, non écrite, P24-28).
ANCHOR_PINS = {
    "1345": ("undercut", "RA-node"),
    "1352": ("undermine", "I-node"),
}
# Couverture par héritage (aucune cellule propre) : descendants de 1346 (phrase, grain 19).
INHERITED_UNTOUCHED = ("1347", "1348", "1349")
# Sans-(a) AVANT écriture (définition cadrage 06/11 : ExceptionRef non vide, ou DirectRef
# _Inference/_Scheme, héritée du plus proche ancêtre porteur — la phrase ne compte PAS :
# les rangées phrase du grain 19 (1346/1351, et 1347-1349 par non-héritage) restent
# sans-(a) ; seul 1350 en est sorti par sa référence).
EXPECTED_SANS_A_PRE = frozenset(("1312", "1345", "1346", "1347", "1348", "1349", "1351",
                                 "1352", "1353", "1354", "1355", "1356",
                                 "1357", "1358", "1359"))
# Sans-(a) APRÈS : les 3 références sortent de l'ensemble ; les 4 phrases RESTENT sans-(a)
# par définition (elles sont dans le bucket « imitation » du census, pas « mapped »).
EXPECTED_SANS_A_POST = frozenset(("1312", "1345", "1346", "1347", "1348", "1349", "1351",
                                  "1352", "1354", "1357", "1358", "1359"))

SKOS_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther",
             "AIF_skosMappingType", "AIF_imitatedScheme")
# Définition attaque-seule de FallacyAttackOnlyStopGateTests (4 colonnes skos, PAS imitatedScheme).
ATTACK_SKOS_COLS = ("AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther",
                    "AIF_skosMappingType")


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


def attack_only(field_rows, I):
    """Même définition que FallacyAttackOnlyStopGateTests (attaque non vide + 4 skos vides)."""
    return {f[I["PK"]] for f in field_rows
            if (f[I["AIF_attackType"]].strip() or f[I["AIF_attackedNode"]].strip())
            and all(not f[I[c]].strip() for c in ATTACK_SKOS_COLS)}


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

    def sans_a(field_rows_):
        return {f[I["PK"]] for f in field_rows_
                if path_of(f) == "7.2" or path_of(f).startswith("7.2.")
                if not (decode(f[I["AIF_skosExceptionRef"]]) != "" or has_direct_a(f)
                        or ancestor_a(f))}

    sans_before = sans_a(field_rows[1:])
    assert sans_before == EXPECTED_SANS_A_PRE, "sans-(a) re-dérivés %r != attendus %r" % (
        sorted(sans_before), sorted(EXPECTED_SANS_A_PRE))

    # --- gardes ---
    for pk, path in PATHS.items():
        assert by_pk[pk][I["path"]] == path, "PK %s path %r != %r" % (pk, by_pk[pk][I["path"]], path)
    for f in field_rows[1:]:
        if path_of(f) in NAKED_HEADS:
            pk = f[I["PK"]]
            assert pk in NAKED_HEAD_PKS, "tête nue inattendue : %s" % pk
            assert pk not in REF_EDITS and pk not in PHRASE_EDITS, "tête nue visée : PK %s" % pk
    # anti-overwrite + anti-drift : les 7 cibles n'ont AUCUNE cellule skos à la base
    for pk in tuple(REF_EDITS) + tuple(PHRASE_EDITS):
        f = by_pk[pk]
        for c in SKOS_COLS:
            if f[I[c]] != "":
                raise AssertionError("anti-drift: PK %s %s=%r non vide, base attendue vide "
                                     "(re-jeu interdit)" % (pk, c, f[I[c]]))
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
    # le jeton RETENU est mesuré porteur : 1360 (exception) ; le jeton ÉCARTÉ aussi (1398,
    # direct) — et le « jumeau » 1402 qui appuyait ce dernier n'a AUCUN jeton (proposition).
    assert decode(by_pk["1360"][I["AIF_skosExceptionRef"]]) == "Ethotic_Inference", "porteur 1360"
    assert decode(by_pk["1360"][I["AIF_skosMappingType"]]) == "skos:broadMatch", "porteur 1360 MT"
    assert decode(by_pk["1398"][I["AIF_skosDirectRef"]]) == "DirectAdHominem_Inference", "porteur 1398"
    assert decode(by_pk["1398"][I["AIF_skosMappingType"]]) == "skos:broadMatch", "porteur 1398 MT"
    assert all(by_pk["1402"][I[c]] == "" for c in SKOS_COLS), \
        "PK 1402 (jumeau cité par le doc) porte un jeton — le motif du choix est à revoir"
    # précédents de représentation : une phrase « imite… » est déjà portée par 244/837/695
    for ppk in ("244", "837", "695"):
        assert decode(by_pk[ppk][I["AIF_imitatedScheme"]]).startswith("imite "), "précédent PK %s absent" % ppk

    before = census(field_rows[1:], I)
    assert sum(before) == 1408, "census avant %r ne somme pas à 1408" % (before,)
    assert before == (132, 4, 12, 1260), "census base %r != 132/4/12/1260" % (before,)
    ao_before = attack_only(field_rows[1:], I)
    assert "1352" in ao_before, "l'ancre 1352 a quitté l'ensemble attaque-seule"

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
    assert n_cells == 10, "cellules écrites %d != 10" % n_cells
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
    # les 7 rangées cibles sont les SEULES différentes, octet pour octet
    diff_rows = [i for i, (a, b) in enumerate(zip(rows, new_rows)) if a != b]
    targets = {i for i, f in enumerate(field_rows) if f[I["PK"]] in set(REF_EDITS) | set(PHRASE_EDITS)}
    assert diff_rows == sorted(targets), "rangées modifiées inattendues : %r != %r" % (diff_rows, sorted(targets))
    # héritage : les descendants restent byte-identiques
    for pk in INHERITED_UNTOUCHED:
        assert by_pk[pk] == vby[pk], "PK %s a bougé (héritage attendu, pas d'écriture)" % pk
    # têtes et ancres intactes après (rangées byte-identiques)
    for pk in NAKED_HEAD_PKS:
        assert vby[pk] == by_pk[pk], "PK %s a bougé (tête nue non écrite)" % pk
    for pk in ANCHOR_PINS:
        assert vby[pk] == by_pk[pk], "ancre %s a bougé — verdict ratifié, rangée intouchable" % pk
        assert all(vby[pk][I[c]] == "" for c in SKOS_COLS), "ancre %s : skos non vides" % pk
    # sans-(a) APRÈS : les 3 références sortent, les phrases restent (définition (a))
    sans_after = sans_a(vfields)
    assert sans_after == EXPECTED_SANS_A_POST, "sans-(a) après %r != attendus %r" % (
        sorted(sans_after), sorted(EXPECTED_SANS_A_POST))

    after = census(vfields, I)
    assert sum(after) == 1408, "census après %r ne somme pas à 1408" % (after,)
    assert after[0] == before[0] + 3, "mapped %d->%d (attendu +3)" % (before[0], after[0])
    assert after[2] == before[2] + 4, "declared-imitation %d->%d (attendu +4)" % (before[2], after[2])
    assert after[3] == before[3] - 7, "untouched %d->%d (attendu -7)" % (before[3], after[3])
    assert after[1] == before[1], "gap %d->%d (attendu inchangé)" % (before[1], after[1])
    # ensemble attaque-seule INCHANGÉ (les cibles n'ont pas de colonnes d'attaque ; 1352 intacte)
    ao_after = attack_only(vfields, I)
    assert ao_after == ao_before, "ensemble attaque-seule modifié : %r" % (
        sorted(ao_after ^ ao_before),)

    if write:
        tmp = CSV_PATH + ".new20"
        with open(tmp, "wb") as fh:
            fh.write(BOM + new_text.encode("utf-8"))
        os.replace(tmp, CSV_PATH)
        print("ECRIT: 10 cellules / 7 rangées (3 ref Ethotic_Inference: 1353/1355/1356 ; "
              "4 phrases: 1354/1357/1358/1359). 1352 non écrite.")
    else:
        print("DRY-RUN OK: 10 cellules / 7 rangées seraient écrites (3 refs ; 4 phrases). "
              "1352 non écrite.")
    print("census avant: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (before + (sum(before),)))
    print("census après: mapped=%d gap=%d imitation=%d untouched=%d (somme %d)"
          % (after + (sum(after),)))
    print("attaque-seule: %d rangées avant et après (1352 incluse)" % len(ao_before))
    print("sans-(a) après: %s" % ", ".join(sorted(sans_after)))
    print("non écrites: 1352 (ancre du sous-arbre, verdict ratifié), 1345 (7.2.2), 1312 (7.2)")


if __name__ == "__main__":
    main()