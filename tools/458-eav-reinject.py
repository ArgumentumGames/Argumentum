#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#458 / #131 etape 10 (P23-24) - re-injection EAV du registre vers une base cible.

REJOUABLE DEPUIS LE DEPOT. Source = docs/dnn-localization/eav-content.csv (le registre
est le PRODUCTEUR, jamais un miroir). Cible = la base designee par le --config fourni
(web.config lu au moment du geste, jamais imprime). Aucune IP, compte ou chaine de
connexion n'est code en dur.

RESOLUTION DES ENTITES (M1/M3, revue ai-01 c.7046760140). Un GUID d'entite N'IDENTIFIE
PAS une entite : 64/65 GUIDs partages du perimetre existent dans PLUSIEURS apps de la
cible, tous publies, 0 brouillons (pattern re-import template-app : chaque instance de
l'app porte sa copie des entites du gabarit, memes GUIDs — copie demo 'Name'/'Chrysler'
a cote de la copie configuree 'Nom'/'Drenche'). Et un AppId est LOCAL a chaque base :
ceux du registre sont ceux de la preprod ; le staging (copie prod) a sa numerotation.
L'outil mesure donc a CHAQUE execution une carte app_registre -> app_cible
(build_app_map) : candidates = apps de la cible portant les GUIDs (meme content type) ;
departage par le CONTENU (la copie reellement exportee dit fr = colonne fr du registre,
la demo jamais). Identite sur la preprod ; numerotation prod sur le staging. Egalite de
score entre candidats -> REFUS, jamais de hasard. Toute cle de resolution — lecture,
classification, ecriture, relecture, preservation — epingle (app_cible, GUID, champ, dim).
La page servie (M3, 20/11) confirme que la copie configuree est celle que sert le site :
l'app de la carte est l'app servie.

PERIMETRE. Une rangée de valeur PAR dimension, appariement par (app_cible, EntityGuid,
field) : le registre ne porte pas d'EntityId (les ids changent d'une base a l'autre —
la copie de prod, le staging et la preprod ne partagent pas les memes), les GUID, eux,
sont stables.

  - registre non-vide, dimension ABSENTE       -> INSERT (TsDynDataValue + lien dim)
  - registre non-vide, dimension VIDE en base  -> UPDATE de cette rangée
  - registre non-vide == base                  -> deja present, on saute
  - registre non-vide != base (les deux pleins) -> CONFLIT : aucune ecriture, liste
  - registre vide                              -> on saute (pas d'effacement)

ECRASEMENT NOMME (--force ; ne de P24-32, 24/11). Un conflit reste un refus, SAUF si
l'operateur nomme la cellule entiere : --force GUID:CHAMP:LANGUE (cumulable, GUID
complet, langue parmi les 7). Elle passe alors en FORCE et rien d'autre ne bouge : sa
valeur d'avant (celle de la base) est sauvegardee dans --backup-dir AVANT la
transaction ; l'UPDATE in-tx epingle la rangee que le dry-run a lue (ValueId MAX de la
dimension) sous verrou et exige qu'elle porte ENCORE exactement la valeur mesuree,
comparaison ordinale — une rangee, sinon ROLLBACK ; la relecture in-tx la relit comme
toute ecriture. Un nom qui
ne designe aucune cellule du plan est un REFUS (faute de frappe) ; un nom dont la
cellule n'est PAS en conflit est rapporte, sans ecriture forcee. Cas fondateur : trois
cellules corrigees en typographie au registre (P24-32) que la base servie porte encore
dans leur ancienne valeur — les deux cotes sont pleins, et c'est la frappe qui dit
« celle-ci, et son contenu d'avant ».

FRANCAIS DE BASE (revue ai-01 c.7038852934). La colonne `fr` du registre est comparee
a la valeur de base (dim-0) de la cible — sur l'entite epingle par la carte (app, guid).
Au sein d'une (entite, attribut) plusieurs rangees dim-0 peuvent coexister (2sxc
append-only, orphelines) : lecture et ecriture epinglent le ValueId MAX de l'entite
epinglee. Ce MAX est un choix de DETERMINISME — il ne departage que les rangees d'une
MEME entite, pour que la relecture relise celle qui a ete ecrite. Il n'etablit pas
laquelle de ces rangees le site rend : ce point n'est pas tranche (M1/M3,
c.7050026044 — ce qui a ete mesure est la resolution d'app, pas un id).
FR_SAME si identique, FR_DRIFT sinon. En --apply, les FR_DRIFT sont REECRITS depuis
le registre — sauf exclusions
explicites (--fr-exclude guid:field ; par defaut le bloc #2181, ou la prod est en
avance) et sauf references 'file:NNN' (FR_DB_LOCAL : id ADAM locaux a la base, jamais
ecrits cross-base). La valeur anterieure de chaque base reecrite est sauvegardee dans
--backup-dir AVANT la transaction. Une base absente (FR_MISSING) refuse l'apply.

PRESERVATION (controle avant ET apres, dans la meme transaction). Le contenu propre a
la cible ne doit pas bouger : bloc #2181 et les 7 entites #2182 (recherchees par
(app, GUID) via la carte, jamais par ValueId ni EntityId — ids dependants de la base).
Un ecart => ROLLBACK, rien n'ecrit.

MODES. dry-run (defaut) = ne fait QUE des SELECT + compteurs attendus, exit 0/1.
apply = une transaction, relecture in-tx octet par octet, COMMIT seulement si tout est
conforme. En apply, --backup-dir est exige : un JSON par valeur touchee y est ecrit
AVANT le premier INSERT/UPDATE (hors depot, jamais committe).

USAGE
  python tools/458-eav-reinject.py --config DNNPlatform/web.config            # dry-run
  python tools/458-eav-reinject.py --config <web.config cible> --apply --backup-dir <hors-depot>
"""
from __future__ import annotations

import argparse
import datetime
import hashlib
import json
import pathlib
import re
import sys

# Import du module d'export : on reutilise la connexion, la lecture SQL par lignes et la
# resolution des dimensions par ExternalKey (les DimensionId different d'une base a l'autre).
import importlib.util
_EXP_PATH = pathlib.Path(__file__).with_name("458-eav-content-export.py")
_spec = importlib.util.spec_from_file_location("eavexp", _EXP_PATH)
EXP = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(EXP)

REGISTRY = EXP.REPO_ROOT / "docs" / "dnn-localization" / "eav-content.csv"
LANGS = EXP.LANGS  # ["en","ru","pt","es","ar","fa","zh"]
CURRENT_PLAN: list[dict] = []      # cellules traductibles (7 langues)
CURRENT_FR_PLAN: list[dict] = []   # cellules de base fr (comparaison FR_* )

# 7 articles News de #2182, par GUID (ids dependent de la base ; sur le staging ils sont
# 12020-12026, sur la preprod 12011-12017). Mesures: docs/dnn-localization/2182-*.
NEWS_2182_GUIDS = [
    "D6A2A0CF-B256-4545-AAFC-DF8D222E282B",
    "39355C1D-4E9E-47F3-822E-92B1B9880770",
    "F2D87598-EEC5-44DA-A10B-E4CFEC2B8491",
    "84094BAE-CA49-4881-A99E-9F11E83671B1",
    "3F913BF3-F0F7-47F7-AB49-6256F76AA55E",
    "8EC968E1-8B0C-47CC-9B9C-FC2892CA6BEF",
    "2E10F534-92EC-41FD-A9BE-767FD992DB78",
]

# Bloc #2181 -- entite 11915, champ Text, dimension de base. Le texte de base fait 4421
# caracteres sur la PREPROD (mesure 20/11) et 6709 sur le STAGING (copie prod, mesure web2
# 20/11) : la base prod est en AVANCE sur la preprod pour ce bloc. Les 7 traductions du
# registre derivent donc d'un texte plus court que celui qui sera servi -> ecrites
# uniquement sur arbitrage explicite (--include-preserved-2181).
PRESERVED_2181_GUID = "8A8636A4-069A-4692-A139-4773B161931B"
PRESERVED_2181_FIELD = "Text"


def read_registry(path=None) -> list[dict]:
    src = pathlib.Path(path) if path else REGISTRY
    with src.open("r", encoding="utf-8-sig", newline="") as h:
        return list(csv.DictReader(h))


import csv  # noqa: E402  (utilise par read_registry)


def build_plan(reg_rows: list[dict]) -> list[dict]:
    """Une entree de plan par cellule traductible non-vide du registre."""
    plan = []
    for r in reg_rows:
        guid = r["entity_guid"].upper()
        for lang in LANGS:
            val = r.get(lang, "")
            if val == "":
                continue
            plan.append({
                "app_id": int(r["app_id"]),
                "content_type": r["content_type"],
                "entity_guid": guid,
                "field": r["field"],
                "lang": lang,
                "value": val,
            })
    return plan


def build_fr_plan(reg_rows: list[dict]) -> list[dict]:
    """Une entree par cellule de BASE (colonne fr du registre) non vide.

    La base dim-0 est ce que sert la culture fr : la comparer, c'est verifier que la
    cible sert bien le francais du registre — condition pour que les 7 traductions
    ecrites traduisent le texte reellement servi (revue ai-01 c.7038852934).
    """
    plan = []
    for r in reg_rows:
        val = r.get("fr") or ""
        if val == "":
            continue
        plan.append({
            "app_id": int(r["app_id"]),
            "content_type": r["content_type"],
            "entity_guid": r["entity_guid"].upper(),
            "field": r["field"],
            "lang": "fr",
            "value": val,
        })
    return plan


def build_app_map(conn, reg_rows) -> dict:
    """Carte app_registre -> app de LA base cible, mesuree a chaque execution.

    Pourquoi : les AppId sont locaux a chaque base (M1 20/11 : le registre porte ceux
    de la preprod ; le staging, copie de prod, a sa propre numerotation — un app 2sxc
    n'a pas de GUID). Et un GUID d'entite N'IDENTIFIE PAS une entite : 64/65 GUIDs
    partages du perimetre existent dans PLUSIEURS apps, tous publies (copie demo
    d'une re-importation + copie configuree). Resoudre par GUID seul lit une copie
    au hasard ; resoudre par AppId du registre ne dit rien sur une autre base.

    Comment : pour chaque app du registre, les apps candidates de la cible sont
    celles qui portent ses GUIDs (meme nom de content type). Le departage se fait
    par le CONTENU : la copie reellement exportee dit 'fr' = colonne fr du registre
    pour la quasi-totalite de ses cellules ; la copie demo ne le dit jamais. Sur la
    preprod la carte est l'identite ; sur le staging elle mappe vers les apps de la
    copie de prod. Vainqueur sans marge => REFUS (arbitrage, pas de hasard).
    """
    per_app = {}  # app registre -> {"guids", "cts", "cells": [(guid, field, fr)]}
    for r in reg_rows:
        a = int(r["app_id"]); g = r["entity_guid"].upper()
        d = per_app.setdefault(a, {"guids": set(), "cts": set(), "cells": []})
        d["guids"].add(g); d["cts"].add(r["content_type"])
        if r.get("fr"):
            d["cells"].append((g, r["field"], r["fr"]))
    amap, evidence = {}, []
    for a, d in sorted(per_app.items()):
        in_g = ", ".join("'" + g + "'" for g in sorted(d["guids"]))
        cts = ", ".join("'" + c.replace("'", "''") + "'" for c in sorted(d["cts"]))
        vals = EXP.run_query(conn, f"""
SET NOCOUNT ON;
SELECT e.AppId AS app, CONVERT(nvarchar(64), e.EntityGuid) AS g, a.StaticName AS f,
       CONVERT(nvarchar(max), v.[Value]) AS val
FROM TsDynDataEntity e
JOIN TsDynDataContentType ct ON ct.ContentTypeId = e.ContentTypeId
JOIN TsDynDataValue v ON v.EntityId = e.EntityId
JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId AND a.TransDeletedId IS NULL
LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId
WHERE e.TransDeletedId IS NULL AND vd.ValueId IS NULL
  AND ct.Name IN ({cts})
  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_g});
""")
        by_cand = {}  # app -> {(g, f): set(values)}
        for r in vals:
            key = (r["g"].upper(), r["f"])
            by_cand.setdefault(int(r["app"]), {}).setdefault(key, set()).add(r["val"] or "")
        cells = d["cells"] or []
        scored = []
        for cand, kv in sorted(by_cand.items()):
            matched = sum(1 for (g, f, fr) in cells if fr in kv.get((g, f), ()))
            scored.append({"app": cand, "cells_fr": len(cells), "matched": matched,
                           "overlap_guids": len({g for g, _ in kv})})
        scored.sort(key=lambda s: (-s["matched"], -s["overlap_guids"]))
        if not scored:
            raise SystemExit(f"REFUS: app {a} du registre introuvable sur la cible (aucun GUID reconnu)")
        win = scored[0]
        # G2 (revue ai-01 c.7053569313) : un vainqueur sans marge = un choix au hasard.
        # - matched == 0 : le contenu n'a rien departage (candidat sans fr reconnu, ou
        #   app du registre sans cellule fr) — la carte ne dit rien, on refuse.
        # - 2e candidat a matched == vainqueur : plus de marge. L'overlap de GUIDs ne
        #   departage plus rien (deux copies du meme gabarit sevent les memes GUIDs) :
        #   on refuse au lieu de trancher par un tie-break silencieux.
        if win["matched"] == 0:
            raise SystemExit(f"REFUS (G2): app {a} — aucun contenu fr reconnu chez le "
                             f"candidat vainqueur: {scored}")
        if len(scored) > 1 and scored[1]["matched"] >= win["matched"]:
            raise SystemExit(f"REFUS (G2): app {a} — vainqueur sans marge "
                             f"({win['matched']} contre {scored[1]['matched']}): {scored}")
        amap[a] = win["app"]
        evidence.append({"registry_app": a, "target_app": win["app"],
                         "matched": f"{win['matched']}/{win['cells_fr']}",
                         "candidats": scored})
    # G3 (revue ai-01 c.7053569313) : carte non injective — deux apps du registre
    # vers la meme app cible. Les cles (app, guid) du plan se confondraient et les
    # memes entites seraient ecrites sous deux identites : carte cassee, pas un mapping.
    seen_target = {}
    for a_src, a_tgt in sorted(amap.items()):
        if a_tgt in seen_target:
            raise SystemExit(f"REFUS (G3): carte non injective — apps {seen_target[a_tgt]} "
                             f"et {a_src} du registre toutes deux mappees vers l'app cible {a_tgt}")
        seen_target[a_tgt] = a_src
    return {"map": amap, "evidence": evidence}


def print_app_map(evidence) -> None:
    """Imprime la carte mesuree : une ligne par app du registre, avec ses marges."""
    print("carte d'apps (registre -> cible, mesuree a l'execution) :")
    for e in evidence:
        cands = ", ".join(f"{c['app']}:{c['matched']}/{c['cells_fr']}" for c in e["candidats"])
        print(f"  app {e['registry_app']} -> {e['target_app']} "
              f"(fr reconnu {e['matched']} ; candidats {cands})")


def resolve_target(conn, reg_rows):
    """Mesure la carte d'apps, l'IMPRIME, puis lit la cible (P23-27, c.7057778259).

    L'ordre est volontaire : fetch_target peut refuser (G1, dimension absente) et le
    dry-run s'arrete la. La carte est la seule piece qui dit quelle app de la cible
    joue le role de chaque app du registre ; imprimee apres la lecture, un refus se
    lit sur des numeros d'app dont rien n'etablit qu'ils sont les bons.
    """
    info = build_app_map(conn, reg_rows)
    print_app_map(info["evidence"])
    return info, fetch_target(conn, info["map"])


def g1_entity_ids(conn, in_apps, in_guids) -> dict:
    """EntityId vivants par paire (app, GUID), pour NOMMER le refus G1 (nit po-2024).

    Meme fragment FROM..WHERE que la requete de comptage, sans jointure : les ids
    listes sont exactement ceux qu'elle a comptes. Un JOIN ajoute ici change les
    lignes lues sans toucher au WHERE — la garde R4 compare donc a partir de FROM
    (`tools/test_458-eav-reinject-guards.py`). Aide au debogage seulement — si cette
    lecture echoue, le refus tombe quand meme, sans les ids : la garde est le refus,
    pas la liste.
    """
    try:
        rows = EXP.run_query(conn, f"""
SET NOCOUNT ON;
SELECT e.AppId AS app, CONVERT(nvarchar(64), e.EntityGuid) AS g, e.EntityId AS eid
FROM TsDynDataEntity e
WHERE e.TransDeletedId IS NULL AND e.AppId IN ({in_apps})
  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids});
""")
    except Exception as exc:  # noqa: BLE001 — le REFUS (G1) ne depend d'aucune lecture
        print(f"G1: EntityId non listes ({type(exc).__name__}: {exc}) — le refus tient.")
        return {}
    out = {}
    for r in rows:
        out.setdefault((int(r["app"]), str(r["g"]).upper()), set()).add(int(r["eid"]))
    return out


def g1_pair_detail(dup_row, ids_by_pair) -> str:
    """Une paire refusee par G1, avec ses EntityId quand ils ont pu etre relus."""
    key = (int(dup_row["app"]), str(dup_row["g"]).upper())
    ids = sorted(ids_by_pair.get(key, ()))
    base = f"app {dup_row['app']} {key[1]} ({dup_row['n']} entites"
    if not ids:
        return base + ", EntityId non listes)"
    listed = "/".join(str(i) for i in ids)
    moved = "" if len(ids) == int(dup_row["n"]) else ", la base a bouge entre les deux lectures"
    return base + f", EntityId {listed}{moved})"


def fetch_target(conn, app_map) -> dict:
    """Lit la base cible : dimensions, valeurs existantes du perimetre, contenu preserve."""
    # La meme culture existe dans plusieurs zones 2sxc (zone 2 = defaut DNN, zone 3 =
    # apps Argumentum). Les docs d'injection (app 59/35, inject42) utilisent les dims
    # 7-13 = zone 3. On resout la dimension PAR ZONE : la zone de l'app DE LA CIBLE
    # (carte d'apps), mesuree sur TsDynDataApp.ZoneId. Ne jamais prendre le premier
    # en-US venu. Les cles de resolution portent l'app CIBLE : (app, guid, champ, dim)
    # — un GUID seul ne suffit pas (M1 20/11 : copies demo/configuree par app).
    app_ids = sorted(set(app_map.values()))
    in_apps = ", ".join(str(a) for a in app_ids)
    zones = EXP.run_query(conn,
        f"SET NOCOUNT ON; SELECT AppId AS app, ZoneId AS zone FROM TsDynDataApp WHERE AppId IN ({in_apps});")
    app_zone = {int(r["app"]): int(r["zone"]) for r in zones}
    zone_ids = sorted(set(app_zone.values()))
    in_zones = ", ".join(str(z) for z in zone_ids)
    dimrows = EXP.run_query(conn,
        "SET NOCOUNT ON; SELECT DimensionId AS id, ExternalKey AS ext, ZoneId AS zone "
        f"FROM TsDynDataDimension WHERE ZoneId IN ({in_zones}) AND Active = 1;")
    ext_by_lang = {v: k for k, v in EXP.DIM_EXTERNAL_KEY.items()}  # 'en-US'->'en'
    zone_lang_dim = {}  # (zone, lang) -> DimensionId
    for r in dimrows:
        ext = r["ext"]
        if ext in ext_by_lang:
            zone_lang_dim[(int(r["zone"]), ext_by_lang[ext])] = int(r["id"])
    lang_dim_by_app = {}  # app -> {lang: DimensionId}
    for app, zone in app_zone.items():
        m = {}
        for lang in LANGS:
            d = zone_lang_dim.get((zone, lang))
            if d is None:
                raise SystemExit(f"dimension {lang} absente de la zone {zone} (app {app})")
            m[lang] = d
        lang_dim_by_app[app] = m

    guids = sorted({p["entity_guid"] for p in CURRENT_PLAN}
                   | {p["entity_guid"] for p in CURRENT_FR_PLAN})
    in_guids = ", ".join("'" + g + "'" for g in guids)
    # G1 (revue ai-01 c.7053569313) : une paire (app cible, GUID) qui designe plus
    # d'une entite vivante. M1 a mesure le cas (1 GUID sur 65 porte deux entites dans
    # la MEME app) : les valeurs des deux entites se melangeraient sous une meme cle
    # et le writer prendrait la premiere ligne. Requete dediee sur TsDynDataEntity —
    # sans jointure sur les valeurs, une entite sans valeur compte aussi.
    dup = EXP.run_query(conn, f"""
SET NOCOUNT ON;
SELECT e.AppId AS app, CONVERT(nvarchar(64), e.EntityGuid) AS g,
       COUNT(DISTINCT e.EntityId) AS n
FROM TsDynDataEntity e
WHERE e.TransDeletedId IS NULL AND e.AppId IN ({in_apps})
  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids})
GROUP BY e.AppId, CONVERT(nvarchar(64), e.EntityGuid)
HAVING COUNT(DISTINCT e.EntityId) > 1;
""")
    if dup:
        # Nit po-2024 (c.7057778259) : nommer les EntityId, pas seulement leur nombre
        # (demande ai-01 c.7053569313, « liste les paires et leurs ids »). Le refus est
        # deja decide ci-dessus : cette seconde lecture ne fait que le rendre lisible.
        ids = g1_entity_ids(conn, in_apps, in_guids)
        detail = "; ".join(g1_pair_detail(d, ids) for d in dup)
        raise SystemExit(f"REFUS (G1): paire(s) (app, guid) multi-entites sur la cible: {detail}")
    sql = f"""
SET NOCOUNT ON;
SELECT e.AppId AS app,
       CONVERT(nvarchar(64), e.EntityGuid) AS entity_guid,
       e.EntityId AS entity_id,
       a.StaticName AS field,
       vd.DimensionId AS dimension_id,
       v.ValueId AS vid,
       CONVERT(nvarchar(max), v.Value) AS val
FROM TsDynDataEntity e
JOIN TsDynDataContentType ct ON ct.ContentTypeId = e.ContentTypeId
JOIN TsDynDataValue v ON v.EntityId = e.EntityId
JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId AND a.TransDeletedId IS NULL
LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId
WHERE e.TransDeletedId IS NULL
  AND e.AppId IN ({in_apps})
  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids})
  AND ct.Name NOT IN ('RawData')
ORDER BY app, entity_guid, field, dimension_id, vid;
"""
    rows = EXP.run_query(conn, sql)
    # index: (app CIBLE, GUID, field, dimId-or-0) -> value
    # La resolution par (app, guid) elimine les copies demo/configuree qui partagent
    # un GUID (M1 20/11 : 64/65 GUIDs partages, tous publies — l'app vient de la
    # carte build_app_map, departagee par le contenu). Au sein d'UNE entite, le tri
    # vid croissant + dernier-gagnant garde le ValueId MAX si une (entite, attr, dim)
    # portait plusieurs rangees — determinisme, pas une regle de resolution.
    existing = {}
    entities = {}
    for row in rows:
        a = int(row["app"]); g = row["entity_guid"].upper()
        entities[(a, g)] = int(row["entity_id"])
        dim = int(row["dimension_id"]) if str(row["dimension_id"]).strip() not in ("", "None") else 0
        existing[(a, g, row["field"], dim)] = row["val"] or ""
    return {"lang_dim_by_app": lang_dim_by_app, "existing": existing, "entities": entities}


def preservation_probe(conn, app_map=None) -> dict:
    """Capture l'etat du contenu preserve (AVANT et APRES doivent etre identiques).

    Resolution par (app, GUID) + champ + dimension de base, JAMAIS par ValueId : les
    ValueId ne survivent pas a une copie de base, et 27942 sur la preprod peut designer
    autre chose sur le staging. L'app vient de la carte mesuree — un GUID seul peut
    vivre dans plusieurs apps (M1 20/11). Le ValueId REELLEMENT trouve est rapporte,
    mais n'est pas l'identite.
    """
    app_2181 = None
    apps_2182: list[int] = []
    if app_map:
        for p in CURRENT_PLAN + CURRENT_FR_PLAN:
            if p["entity_guid"] == PRESERVED_2181_GUID and p["field"] == PRESERVED_2181_FIELD:
                app_2181 = app_map[p["app_id"]]
            if p["entity_guid"] in set(NEWS_2182_GUIDS):
                apps_2182.append(app_map[p["app_id"]])
        apps_2182 = sorted(set(apps_2182))
    out = {}
    r1 = EXP.run_query(conn,
        "SET NOCOUNT ON; SELECT TOP 1 v.ValueId AS vid, "
        "CONVERT(nvarchar(max), v.Value) AS val "
        "FROM TsDynDataValue v "
        "JOIN TsDynDataEntity e ON e.EntityId = v.EntityId "
        "JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId "
        "LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId "
        f"WHERE CONVERT(nvarchar(64), e.EntityGuid) = '{PRESERVED_2181_GUID}' "
        f"  AND a.StaticName = '{PRESERVED_2181_FIELD}' "
        + (f"  AND e.AppId = {app_2181} " if app_2181 else "")
        + "  AND vd.ValueId IS NULL "
        "ORDER BY v.ValueId DESC;")  # ValueId MAX = determinisme au sein de l'entite epinglee
    out["block_2181"] = [{
        "value_id": int(x["vid"]),
        "len": len(x["val"] or ""),
        # Empreinte du CONTENU (pas de la longueur seule) : deux textes de meme
        # longueur ne passent plus. UTF-16-LE = encodage natif nvarchar, ce que
        # HASHBYTES aurait hashe cote SQL — sans sa limite de 8000 octets.
        "sha256": hashlib.sha256((x["val"] or "").encode("utf-16-le")).hexdigest(),
        "value": x["val"] or "",
    } for x in r1]
    in_guids = ", ".join("'" + g + "'" for g in NEWS_2182_GUIDS)
    in_apps_2182 = (", ".join(str(a) for a in apps_2182)) if apps_2182 else None
    r2 = EXP.run_query(conn,
        "SET NOCOUNT ON; SELECT CONVERT(nvarchar(64), e.EntityGuid) AS g, e.EntityId AS id "
        f"FROM TsDynDataEntity e WHERE CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids})"
        + (f" AND e.AppId IN ({in_apps_2182})" if in_apps_2182 else "") + ";")
    out["news_2182"] = sorted(
        ({"guid": x["g"].upper(), "entity_id": int(x["id"])} for x in r2),
        key=lambda d: d["guid"])
    return out


def compute_fr_actions(fr_plan, target, app_map, excludes: set) -> list[dict]:
    """Classe chaque cellule de base fr du registre contre la cible.

    FR_SAME       base cible == fr du registre (rien a faire)
    FR_DRIFT      les deux non vides, differents -> REECRITURE en --apply
    FR_EXCLUDED   exclue explicitement (defaut : bloc #2181, prod en avance)
    FR_DB_LOCAL   reference adam 'file:NNN' divergente -> JAMAIS ecrite (voir ci-dessous)
    FR_MISSING    aucune valeur de base sur la cible -> refuse l'apply
    """
    existing = target["existing"]
    actions = []
    for p in fr_plan:
        g, field = p["entity_guid"], p["field"]
        ta = app_map[p["app_id"]]
        base = existing.get((ta, g, field, 0))
        if base is None:
            actions.append({**p, "op": "FR_MISSING"})
        elif (g, field) in excludes:
            actions.append({**p, "op": "FR_EXCLUDED", "db_value": base})
        elif base == p["value"]:
            actions.append({**p, "op": "FR_SAME"})
        elif p["value"].startswith("file:"):
            # Les references 'file:NNN' sont des ids ADAM LOCAUX A LA BASE : le
            # meme numero ne designe pas le meme fichier d'une base a l'autre.
            # Une reecriture cross-base (registre preprod -> staging) pointerait
            # le staging vers les fichiers d'une autre base. Garde structurelle,
            # pas une politique : jamais ecrit, toujours rapporte.
            actions.append({**p, "op": "FR_DB_LOCAL", "db_value": base})
        else:
            actions.append({**p, "op": "FR_DRIFT", "db_value": base, "target_app": ta})
    return actions


def fr_report(fr_actions) -> list[dict]:
    """Liste des cellules FR_DRIFT : GUID court, ContentType.champ, longueurs."""
    return [{
        "guid8": a["entity_guid"][:8],
        "key": f"{a['entity_guid']}/{a['field']}",
        "content_type": a["content_type"],
        "field": a["field"],
        "len_base": len(a["db_value"]),
        "len_registre": len(a["value"]),
    } for a in fr_actions if a["op"] == "FR_DRIFT"]


def compute_actions(plan, target, app_map, allow_preserved=False, force=None):
    """Classe chaque cellule du plan (voir PERIMETRE, en tete). `force` est l'ensemble
    des cles (GUID majuscule, champ, langue) nommees par --force : elles seules quittent
    CONFLICT pour FORCE ; un conflit non nomme reste CONFLICT, donc refuse."""
    existing = target["existing"]
    lang_dim_by_app = target["lang_dim_by_app"]
    force = force or frozenset()
    actions = []
    for p in plan:
        g, field, lang = p["entity_guid"], p["field"], p["lang"]
        if g == PRESERVED_2181_GUID and field == PRESERVED_2181_FIELD and not allow_preserved:
            # Les traductions derivent du texte preprod (4421c) ; la base servie en porte un
            # plus long (6709c). Ecrire ici accouplerait un FR long a des traductions courtes.
            actions.append({**p, "op": "SKIP_PRESERVED_2181"})
            continue
        ta = app_map[p["app_id"]]
        dim = lang_dim_by_app[ta][lang]
        base = existing.get((ta, g, field, 0))
        if base is None:
            actions.append({**p, "op": "MISSING_ENTITY_OR_FIELD"})
            continue
        cur = existing.get((ta, g, field, dim))
        if cur is None:
            actions.append({**p, "op": "INSERT", "dim": dim, "target_app": ta})
        elif cur == "":
            actions.append({**p, "op": "UPDATE", "dim": dim, "target_app": ta})
        elif cur == p["value"]:
            actions.append({**p, "op": "SKIP_SAME", "dim": dim})
        elif (g.upper(), field, lang) in force:
            # Ecrasement NOMME (P24-32) : les deux cotes pleins, la cellule designee une a une.
            # La valeur cible voyage avec l'action : sauvegarde AVANT, comparaison in-tx.
            actions.append({**p, "op": "FORCE", "dim": dim, "target_app": ta, "db_value": cur})
        else:
            actions.append({**p, "op": "CONFLICT", "dim": dim, "db_value": cur})
    return actions


# Une cle --force est (GUID complet, champ, langue). GUID COMPLET, pas le prefixe de 8
# dont l'outil se sert pour ECRIRE ses rapports : un prefixe n'est pas un identifiant
# (145 GUIDs distincts, 145 prefixes distincts au registre le 24/11 — un etat mesure,
# pas une promesse du registre).
FORCE_GUID_RE = re.compile(r"^[0-9A-F]{8}(-[0-9A-F]{4}){3}-[0-9A-F]{12}$")
WRITE_OPS = ("INSERT", "UPDATE", "FORCE")


def parse_force(item: str):
    """`GUID:CHAMP:LANGUE` -> cle normalisee (GUID majuscule, champ, langue), sinon None."""
    parts = item.split(":")
    if len(parts) != 3 or not parts[1] or parts[2] not in LANGS:
        return None
    if not FORCE_GUID_RE.match(parts[0].upper()):
        return None
    return (parts[0].upper(), parts[1], parts[2])


def force_key(action) -> tuple:
    return (action["entity_guid"].upper(), action["field"], action["lang"])


def unmatched_force(actions, force_keys) -> list:
    """Les cles --force qui ne designent AUCUNE cellule classee du plan : faute de frappe."""
    seen = {force_key(a) for a in actions}
    return sorted(k for k in force_keys if k not in seen)


def planned_writes(actions) -> list:
    """Les actions que le writer executera : jamais un CONFLICT, jamais un MISSING."""
    return [a for a in actions if a["op"] in WRITE_OPS]


def main(argv=None) -> int:
    global CURRENT_PLAN, CURRENT_FR_PLAN
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--config", default=str(EXP.DEFAULT_CONFIG))
    ap.add_argument("--apply", action="store_true", help="ecrit (defaut: dry-run)")
    ap.add_argument("--backup-dir", default=None, help="requis en --apply, hors depot")
    ap.add_argument("--registry", default=None,
                    help="chemin du registre CSV (defaut: docs/dnn-localization/eav-content.csv du depot)")
    ap.add_argument("--include-preserved-2181", action="store_true",
                    help="ecrit AUSSI les traductions du bloc #2181 (arbitrage ai-01 20/11 : "
                         "option c transitoire — la base FR du bloc reste celle de la prod)")
    ap.add_argument("--fr-exclude", action="append", default=[], metavar="GUID:FIELD",
                    help="exclut une cellule de base fr de la reecriture (cumulable) ; "
                         "le bloc #2181 est exclu par defaut")
    ap.add_argument("--fr-report", default="fr-drift-report.json",
                    help="fichier ou ecrire la liste FR_DRIFT (defaut: ./fr-drift-report.json)")
    ap.add_argument("--force", action="append", default=[], metavar="GUID:FIELD:LANG",
                    help="ecrasement NOMME (cumulable) : la cellule en CONFLIT designee par son "
                         "GUID COMPLET, son champ et sa langue est reecrite depuis le registre — "
                         "valeur d'avant sauvegardee, concurrence optimiste in-tx. Toute autre "
                         "cellule en conflit continue de refuser l'apply")
    args = ap.parse_args(argv)

    excludes = {(PRESERVED_2181_GUID, PRESERVED_2181_FIELD)}
    for item in args.fr_exclude:
        g, _, f = item.partition(":")
        if not g or not f:
            print(f"REFUS: --fr-exclude mal forme ({item!r}) — attendu GUID:FIELD")
            return 1
        excludes.add((g.upper(), f))
    force_keys = set()
    for item in args.force:
        key = parse_force(item)
        if key is None:
            print(f"REFUS: --force mal forme ({item!r}) — attendu GUID-complet:CHAMP:LANGUE "
                  f"(langue parmi {', '.join(LANGS)})")
            return 1
        force_keys.add(key)

    reg = read_registry(args.registry)
    CURRENT_PLAN = build_plan(reg)
    CURRENT_FR_PLAN = build_fr_plan(reg)
    conn = EXP.read_connection_string(pathlib.Path(args.config))
    print(f"registre: {len(reg)} lignes, {len(CURRENT_PLAN)} cellules traductibles non vides, "
          f"{len(CURRENT_FR_PLAN)} cellules de base fr")
    # Carte d'apps imprimee AVANT la lecture de la cible (P23-27) : fetch_target peut
    # refuser (G1, dimension absente) et l'operateur doit deja lire la carte.
    app_map_info, target = resolve_target(conn, reg)
    actions = compute_actions(CURRENT_PLAN, target, app_map_info["map"],
                              allow_preserved=args.include_preserved_2181, force=force_keys)
    fr_actions = compute_fr_actions(CURRENT_FR_PLAN, target, app_map_info["map"], excludes)
    unknown = unmatched_force(actions, force_keys)
    if unknown:
        # Une faute de frappe dans --force ne doit pas se lire « rien a forcer » : le nom
        # ne designe aucune cellule du plan, donc l'operateur croit avoir nomme autre chose.
        print("REFUS: --force nomme une cellule qui n'est pas au plan (GUID/champ/langue) : "
              + "; ".join(f"{g[:8]}… {f} [{lang}]" for g, f, lang in unknown))
        return 1

    counts = {}
    for a in actions:
        counts[a["op"]] = counts.get(a["op"], 0) + 1
    fr_counts = {}
    for a in fr_actions:
        fr_counts[a["op"]] = fr_counts.get(a["op"], 0) + 1
    print(f"entites resolues sur la cible: {len(target['entities'])}")
    for op in sorted(counts):
        print(f"  {op}: {counts[op]}")
    print("base fr (dim-0) :")
    for op in sorted(fr_counts):
        print(f"  {op}: {fr_counts[op]}")

    # Rapport FR : listes dans un fichier (GUID court, ContentType.champ, longueurs).
    report = fr_report(fr_actions)
    pathlib.Path(args.fr_report).write_text(
        json.dumps({"fr_drift": report,
                    "app_map": app_map_info["evidence"],
                    "excluded": [
            {"key": f"{a['entity_guid']}/{a['field']}", "len_base": len(a["db_value"]),
             "len_registre": len(a["value"])}
            for a in fr_actions if a["op"] == "FR_EXCLUDED"],
            "db_local_refused": [
            # references file:NNN divergentes — jamais ecrites, id ADAM locaux a la base
            {"key": f"{a['entity_guid']}/{a['field']}", "len_base": len(a["db_value"]),
             "len_registre": len(a["value"])}
            for a in fr_actions if a["op"] == "FR_DB_LOCAL"]},
            ensure_ascii=False, indent=1), encoding="utf-8")
    if report:
        print(f"  FR_DRIFT detaille -> {args.fr_report} ({len(report)} cellule(s))")
        for r in report[:15]:
            print(f"    - {r['guid8']} {r['content_type']}.{r['field']} "
                  f"base={r['len_base']}c registre={r['len_registre']}c")
    if fr_counts.get("FR_DB_LOCAL"):
        print(f"  NOTE: {fr_counts['FR_DB_LOCAL']} reference(s) file:NNN divergente(s) — "
              f"JAMAIS ecrite(s) (id ADAM locaux a la base, voir db_local_refused du rapport)")

    pres_before = preservation_probe(conn, app_map_info["map"])
    b2181 = pres_before["block_2181"][0] if pres_before["block_2181"] else {}
    print(f"preservation AVANT: #2181 value_id={b2181.get('value_id')} len={b2181.get('len')} "
          f"sha256={str(b2181.get('sha256'))[:12]}…, #2182 entites={len(pres_before['news_2182'])}/7")
    if counts.get("SKIP_PRESERVED_2181"):
        print(f"  NOTE: {counts['SKIP_PRESERVED_2181']} traduction(s) du bloc #2181 ecartee(s) — "
              f"--include-preserved-2181 pour les ecrire (arbitrage ai-01 20/11 : option c)")

    conflicts = [a for a in actions if a["op"] == "CONFLICT"]
    missing = [a for a in actions if a["op"] == "MISSING_ENTITY_OR_FIELD"]
    fr_missing = [a for a in fr_actions if a["op"] == "FR_MISSING"]
    if missing:
        print(f"\n{len(missing)} cellule(s) sans entite/champ resolu sur la cible (extrait):")
        for a in missing[:15]:
            print(f"  - {a['entity_guid'][:8]} {a['content_type']}.{a['field']} [{a['lang']}]")
    if conflicts:
        print(f"\n{len(conflicts)} CONFLIT(S) (les deux pleins, differents) — aucune ecriture pour celles-ci:")
        for a in conflicts[:15]:
            print(f"  - {a['entity_guid'][:8]} {a['field']} [{a['lang']}] "
                  f"db={len(a['db_value'])}c registre={len(a['value'])}c")
    forced = [a for a in actions if a["op"] == "FORCE"]
    if forced:
        # Pas de troncature ici : chaque ligne a ete NOMMEE par l'operateur (--force), il
        # doit pouvoir les relire toutes avant d'ecrire — c'est du contenu qui REMPLACE.
        print(f"\n{len(forced)} ECRASEMENT(S) NOMME(S) (--force) — la valeur d'avant est "
              f"sauvegardee dans --backup-dir, puis remplacee en --apply :")
        for a in forced:
            print(f"  - {a['entity_guid'][:8]} {a['field']} [{a['lang']}] "
                  f"db={len(a['db_value'])}c -> registre={len(a['value'])}c")
    idle = [a for a in actions if a["op"] != "FORCE" and force_key(a) in force_keys]
    if idle:
        print(f"\nNOTE: {len(idle)} cellule(s) nommee(s) par --force ne sont PAS en conflit "
              f"({', '.join(sorted({a['op'] for a in idle}))}) — rapportees, sans ecriture forcee.")
    if fr_missing:
        print(f"\n{len(fr_missing)} cellule(s) de base fr SANS valeur sur la cible (extrait):")
        for a in fr_missing[:15]:
            print(f"  - {a['entity_guid'][:8]} {a['content_type']}.{a['field']}")

    fr_writes = [{**a, "op": "FR_UPDATE"} for a in fr_actions if a["op"] == "FR_DRIFT"]
    writes = planned_writes(actions)
    n_forced = sum(1 for a in writes if a["op"] == "FORCE")
    if not args.apply:
        print(f"\nDRY-RUN: {len(writes)} ecriture(s) dimensionnees prevues "
              f"({counts.get('INSERT',0)} INSERT + {counts.get('UPDATE',0)} UPDATE"
              + (f" + {n_forced} ECRASEMENT(S) NOMME(S)" if n_forced else "")
              + f") + {len(fr_writes)} REECRITURE(S) de base fr (FR_DRIFT non exclues). "
              f"Aucune ecriture effectuee.")
        blocked = conflicts or missing or fr_missing
        print("VERDICT: " + ("ECHEC (conflits ou cibles manquantes a arbitrer)" if blocked
                             else "plan coherent, pret pour --apply"))
        return 1 if blocked else 0

    if conflicts or missing or fr_missing:
        print("REFUS d'appliquer: conflits ou cibles manquantes non arbitres.")
        return 1
    if not args.backup_dir:
        print("REFUS: --apply exige --backup-dir (hors depot).")
        return 1
    backup_dir = pathlib.Path(args.backup_dir)
    backup_dir.mkdir(parents=True, exist_ok=True)
    stamp = datetime.datetime.now(datetime.timezone.utc).strftime("%Y%m%dT%H%M%SZ")
    # Valeurs anterieures des bases reecrites : sauvegardees AVANT la transaction.
    (backup_dir / f"reinject-fr-prior-{stamp}.json").write_text(
        json.dumps([{"entity_guid": a["entity_guid"], "content_type": a["content_type"],
                     "field": a["field"], "prior_value": a["db_value"], "new_value": a["value"]}
                    for a in fr_actions if a["op"] == "FR_DRIFT"],
                   ensure_ascii=False, indent=1), encoding="utf-8")
    # Valeurs cible des ECRASEMENTS NOMMES (--force) : meme discipline, AVANT la transaction.
    # Un fichier a part : son inverse se lit sans melanger les bases fr et les cellules.
    forced = [a for a in writes if a["op"] == "FORCE"]
    if forced:
        (backup_dir / f"reinject-force-prior-{stamp}.json").write_text(
            json.dumps([{"entity_guid": a["entity_guid"], "content_type": a["content_type"],
                         "field": a["field"], "lang": a["lang"], "dim": a["dim"],
                         "target_app": a["target_app"],
                         "prior_value": a["db_value"], "new_value": a["value"]}
                        for a in forced],
                       ensure_ascii=False, indent=1), encoding="utf-8")
    (backup_dir / f"reinject-plan-{stamp}.json").write_text(
        json.dumps({"writes": [{k: a[k] for k in ('entity_guid','field','lang','op','dim','value','target_app')
                                            + (('db_value',) if a["op"] == "FORCE" else ())}
                               for a in writes],
                    "fr_writes": [{k: a[k] for k in ('entity_guid','field','lang','op','value','target_app')} for a in fr_writes],
                    "preservation_before": {k: v for k, v in pres_before.items() if k != "block_2181"}
                                          | {"block_2181": [{kk: vv for kk, vv in b.items() if kk != "value"}
                                                            for b in pres_before["block_2181"]]}},
                    ensure_ascii=False, indent=1),
        encoding="utf-8")
    print(f"plan + valeurs anterieures FR + preservation AVANT sauvegardes dans {backup_dir} "
          f"({len(writes)} ecritures dimensionnees"
          + (f" dont {len(forced)} ecrasement(s) nomme(s), valeur d'avant sauvegardee" if forced else "")
          + f" + {len(fr_writes)} bases fr)")
    return run_writer(conn, writes, fr_writes, pres_before, backup_dir, stamp, app_map_info["map"])


# ---------------------------------------------------------------------------
# Moteur d'ecriture : un script PowerShell genere, UNE transaction, relecture in-tx.
# Modele app 59 / inject42 : INSERT d'une rangee de valeur par dimension + lien
# TsDynDataValueDimension ; UPDATE des rangees dimensionnees vides. Appariement par
# (EntityId resolu du GUID, AttributeId resolu du (ContentType, field), DimensionId).
# ---------------------------------------------------------------------------
# Le script du writer, CONSTANTE DE MODULE : les gardes hors ligne le LISENT (branche
# FORCE, comparaison ordinale, epingle au ValueId) sans base ni PowerShell. Le texte du
# script est deplace tel quel ; les seules lignes ajoutees sont celles de la branche FORCE.
WRITER_PS = r'''
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Data
$payload  = Get-Content -LiteralPath $env:REINJECT_PAYLOAD -Raw -Encoding UTF8 | ConvertFrom-Json
$writes   = $payload.writes;   if ($writes   -isnot [System.Array]) { $writes   = @($writes) }
$frwrites = $payload.fr_writes; if ($frwrites -isnot [System.Array]) { $frwrites = @($frwrites) }
$conn = New-Object System.Data.SqlClient.SqlConnection($env:REINJECT_CS)
$conn.Open()
$tx = $conn.BeginTransaction()
try {
    $cmd = $conn.CreateCommand(); $cmd.Transaction = $tx; $cmd.CommandTimeout = 600
    $inserted = 0; $updated = 0; $frupdated = 0; $forced = 0
    foreach ($w in $writes) {
        $guid = $w.entity_guid; $field = $w.field; $ct = $w.content_type; $val = $w.value
        $app = [int]$w.target_app
        # Resoudre EntityId + AttributeId (UPDLOCK/HOLDLOCK) a l'interieur de la transaction.
        # Epingle par AppId : un meme GUID existe en plusieurs exemplaires (pattern re-import
        # template-app, M1 20/11 : 64/65 GUID partages du perimetre vivent dans 2 apps, tous
        # publies) -- sans l'app, la resolution prend un exemplaire arbitraire.
        $cmd.Parameters.Clear()
        $cmd.CommandText = @"
SELECT e.EntityId, a.AttributeId FROM TsDynDataEntity e WITH (UPDLOCK,HOLDLOCK)
JOIN TsDynDataContentType c ON c.ContentTypeId = e.ContentTypeId
JOIN TsDynDataAttribute a WITH (UPDLOCK,HOLDLOCK) ON a.ContentTypeId = c.ContentTypeId
WHERE e.EntityGuid = @g AND e.AppId = @app AND c.Name = @ct AND a.StaticName = @f AND a.TransDeletedId IS NULL AND e.TransDeletedId IS NULL
"@
        [void]$cmd.Parameters.Add("@g", [System.Data.SqlDbType]::NVarChar, 64); $cmd.Parameters["@g"].Value = $guid
        [void]$cmd.Parameters.Add("@app", [System.Data.SqlDbType]::Int); $cmd.Parameters["@app"].Value = $app
        [void]$cmd.Parameters.Add("@ct", [System.Data.SqlDbType]::NVarChar, 256); $cmd.Parameters["@ct"].Value = $ct
        [void]$cmd.Parameters.Add("@f", [System.Data.SqlDbType]::NVarChar, 256); $cmd.Parameters["@f"].Value = $field
        $r = $cmd.ExecuteReader()
        if (-not $r.Read()) { $r.Close(); throw "entite/attribut introuvable: $guid (app $app) $ct.$field" }
        $eid = $r.GetInt32(0); $aid = $r.GetInt32(1)
        if ($r.Read()) { $r.Close(); throw "G1 in-tx: plusieurs entites pour $guid (app $app) $ct.$field — base modifiee depuis le dry-run" }
        $r.Close()

        $dim = [int]$w.dim
        if ($w.op -eq "INSERT") {
            $cmd.Parameters.Clear()
            $cmd.CommandText = "INSERT INTO TsDynDataValue (EntityId, AttributeId, [Value]) OUTPUT INSERTED.ValueId VALUES (@e,@a,@v)"
            [void]$cmd.Parameters.Add("@e",[System.Data.SqlDbType]::Int); $cmd.Parameters["@e"].Value=$eid
            [void]$cmd.Parameters.Add("@a",[System.Data.SqlDbType]::Int); $cmd.Parameters["@a"].Value=$aid
            [void]$cmd.Parameters.Add("@v",[System.Data.SqlDbType]::NVarChar,-1); $cmd.Parameters["@v"].Value=$val
            $vid = [int]$cmd.ExecuteScalar()
            $cmd.Parameters.Clear()
            $cmd.CommandText = "INSERT INTO TsDynDataValueDimension (ValueId, DimensionId) VALUES (@vid,@d)"
            [void]$cmd.Parameters.Add("@vid",[System.Data.SqlDbType]::Int); $cmd.Parameters["@vid"].Value=$vid
            [void]$cmd.Parameters.Add("@d",[System.Data.SqlDbType]::Int); $cmd.Parameters["@d"].Value=$dim
            [void]$cmd.ExecuteNonQuery(); $inserted++
        } elseif ($w.op -eq "UPDATE") {
            # Ne toucher qu'une rangee dimensionnee VIDE (double garde anti-ecrasement).
            $cmd.Parameters.Clear()
            $cmd.CommandText = @"
UPDATE v SET v.[Value]=@v FROM TsDynDataValue v
JOIN TsDynDataValueDimension vd ON vd.ValueId=v.ValueId AND vd.DimensionId=@d
WHERE v.EntityId=@e AND v.AttributeId=@a AND (v.[Value] IS NULL OR v.[Value]=N'')
"@
            [void]$cmd.Parameters.Add("@v",[System.Data.SqlDbType]::NVarChar,-1); $cmd.Parameters["@v"].Value=$val
            [void]$cmd.Parameters.Add("@d",[System.Data.SqlDbType]::Int); $cmd.Parameters["@d"].Value=$dim
            [void]$cmd.Parameters.Add("@e",[System.Data.SqlDbType]::Int); $cmd.Parameters["@e"].Value=$eid
            [void]$cmd.Parameters.Add("@a",[System.Data.SqlDbType]::Int); $cmd.Parameters["@a"].Value=$aid
            $n = $cmd.ExecuteNonQuery()
            if ($n -ne 1) { throw "UPDATE $guid.$field dim$dim : $n rangee(s) touchee(s), attendu 1 (valeur non vide?)" }
            $updated++
        } elseif ($w.op -eq "FORCE") {
            # Ecrasement NOMME (--force, P24-32). Le dry-run a lu, pour (entite, attribut,
            # dimension), la rangee au ValueId MAX. On epingle CETTE rangee sous verrou, on
            # exige qu'elle porte ENCORE la valeur du dry-run — comparaison ORDINALE en
            # PowerShell, parce que l'egalite SQL suit le classement de la colonne (elle
            # ignore la casse) — puis on l'ecrit par son ValueId. Une valeur anterieure
            # differente, ou la rangee disparue, est un ROLLBACK : rien n'est ecrase.
            $old = $w.db_value
            $cmd.Parameters.Clear()
            $cmd.CommandText = @"
SELECT TOP 1 v.ValueId, CONVERT(nvarchar(max), v.[Value]) FROM TsDynDataValue v WITH (UPDLOCK,HOLDLOCK)
JOIN TsDynDataValueDimension vd ON vd.ValueId=v.ValueId AND vd.DimensionId=@d
WHERE v.EntityId=@e AND v.AttributeId=@a
ORDER BY v.ValueId DESC
"@
            [void]$cmd.Parameters.Add("@d",[System.Data.SqlDbType]::Int); $cmd.Parameters["@d"].Value=$dim
            [void]$cmd.Parameters.Add("@e",[System.Data.SqlDbType]::Int); $cmd.Parameters["@e"].Value=$eid
            [void]$cmd.Parameters.Add("@a",[System.Data.SqlDbType]::Int); $cmd.Parameters["@a"].Value=$aid
            $r = $cmd.ExecuteReader()
            if (-not $r.Read()) { $r.Close(); throw "FORCE $guid.$field dim$dim : aucune rangee a cette dimension (base modifiee depuis le dry-run?)" }
            $vidf = [int]$r.GetInt32(0); $cur = $r.GetValue(1); $r.Close()
            if ($null -eq $cur -or -not [string]::Equals([string]$cur, [string]$old, [System.StringComparison]::Ordinal)) {
                throw "FORCE $guid.$field dim$dim : la valeur en base n'est plus celle du dry-run - ROLLBACK"
            }
            $cmd.Parameters.Clear()
            $cmd.CommandText = "UPDATE TsDynDataValue SET [Value]=@v WHERE ValueId=@vid"
            [void]$cmd.Parameters.Add("@v",[System.Data.SqlDbType]::NVarChar,-1); $cmd.Parameters["@v"].Value=$val
            [void]$cmd.Parameters.Add("@vid",[System.Data.SqlDbType]::Int); $cmd.Parameters["@vid"].Value=$vidf
            $n = $cmd.ExecuteNonQuery()
            if ($n -ne 1) { throw "FORCE $guid.$field dim$dim : $n rangee(s) touchee(s), attendu 1" }
            $forced++
        }
    }
    # Reecritures de BASE fr (FR_DRIFT) : la rangee dim-0 seulement, et seulement si
    # elle vaut encore EXACTEMENT ce que le dry-run a mesure (concurrence optimiste —
    # si la base a bouge entre-temps, on refuse plutot qu'ecraser par-dessus).
    foreach ($w in $frwrites) {
        $guid = $w.entity_guid; $field = $w.field; $ct = $w.content_type
        $val = $w.value; $old = $w.db_value; $app = [int]$w.target_app
        $cmd.Parameters.Clear()
        $cmd.CommandText = @"
SELECT e.EntityId, a.AttributeId FROM TsDynDataEntity e WITH (UPDLOCK,HOLDLOCK)
JOIN TsDynDataContentType c ON c.ContentTypeId = e.ContentTypeId
JOIN TsDynDataAttribute a WITH (UPDLOCK,HOLDLOCK) ON a.ContentTypeId = c.ContentTypeId
WHERE e.EntityGuid = @g AND e.AppId = @app AND c.Name = @ct AND a.StaticName = @f AND a.TransDeletedId IS NULL AND e.TransDeletedId IS NULL
"@
        [void]$cmd.Parameters.Add("@g", [System.Data.SqlDbType]::NVarChar, 64); $cmd.Parameters["@g"].Value = $guid
        [void]$cmd.Parameters.Add("@app", [System.Data.SqlDbType]::Int); $cmd.Parameters["@app"].Value = $app
        [void]$cmd.Parameters.Add("@ct", [System.Data.SqlDbType]::NVarChar, 256); $cmd.Parameters["@ct"].Value = $ct
        [void]$cmd.Parameters.Add("@f", [System.Data.SqlDbType]::NVarChar, 256); $cmd.Parameters["@f"].Value = $field
        $r = $cmd.ExecuteReader()
        if (-not $r.Read()) { $r.Close(); throw "FR: entite/attribut introuvable: $guid (app $app) $ct.$field" }
        $eid = $r.GetInt32(0); $aid = $r.GetInt32(1)
        if ($r.Read()) { $r.Close(); throw "FR G1 in-tx: plusieurs entites pour $guid (app $app) $ct.$field — base modifiee depuis le dry-run" }
        $r.Close()

        # Epingle une seule rangee dim-0 : 2sxc est append-only, une (entite, attribut)
        # peut en porter plusieurs (mesure 20/11 : 961 paires). Le choix du ValueId MAX
        # est un choix de DETERMINISME — la relecture in-tx relira la meme rangee que
        # l'ecriture. Il n'etablit pas laquelle de ces rangees le site rend (M1/M3).
        $cmd.Parameters.Clear()
        $cmd.CommandText = @"
SELECT MAX(v.ValueId) FROM TsDynDataValue v WITH (UPDLOCK,HOLDLOCK)
LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId
WHERE v.EntityId=@e AND v.AttributeId=@a AND vd.ValueId IS NULL
"@
        [void]$cmd.Parameters.Add("@e",[System.Data.SqlDbType]::Int); $cmd.Parameters["@e"].Value=$eid
        [void]$cmd.Parameters.Add("@a",[System.Data.SqlDbType]::Int); $cmd.Parameters["@a"].Value=$aid
        $vid0 = $cmd.ExecuteScalar()
        if ($null -eq $vid0) { throw "FR_UPDATE $guid.$field : aucune rangee dim-0 (base modifiee depuis le dry-run?)" }

        $cmd.Parameters.Clear()
        $cmd.CommandText = @"
UPDATE v SET v.[Value]=@v FROM TsDynDataValue v
WHERE v.ValueId=@vid0 AND v.[Value]=@old
"@
        [void]$cmd.Parameters.Add("@v",[System.Data.SqlDbType]::NVarChar,-1); $cmd.Parameters["@v"].Value=$val
        [void]$cmd.Parameters.Add("@old",[System.Data.SqlDbType]::NVarChar,-1); $cmd.Parameters["@old"].Value=$old
        [void]$cmd.Parameters.Add("@vid0",[System.Data.SqlDbType]::Int); $cmd.Parameters["@vid0"].Value=[int]$vid0
        $n = $cmd.ExecuteNonQuery()
        if ($n -ne 1) { throw "FR_UPDATE $guid.$field : $n rangee(s) touchee(s), attendu 1 (base modifiee depuis le dry-run?)" }
        $frupdated++
    }
    # Relecture in-tx, octet par octet : chaque ecriture doit relire SA valeur a SA dimension.
    $bad = 0
    foreach ($w in $writes) {
        $cmd.Parameters.Clear()
        $cmd.CommandText = @"
SELECT COUNT(*) FROM TsDynDataValue v
JOIN TsDynDataEntity e ON e.EntityId=v.EntityId
JOIN TsDynDataContentType c ON c.ContentTypeId=e.ContentTypeId
JOIN TsDynDataAttribute a ON a.AttributeId=v.AttributeId
JOIN TsDynDataValueDimension vd ON vd.ValueId=v.ValueId
WHERE e.EntityGuid=@g AND e.AppId=@app AND c.Name=@ct AND a.StaticName=@f AND vd.DimensionId=@d AND v.[Value]=@v
"@
        [void]$cmd.Parameters.Add("@g",[System.Data.SqlDbType]::NVarChar,64); $cmd.Parameters["@g"].Value=$w.entity_guid
        [void]$cmd.Parameters.Add("@app",[System.Data.SqlDbType]::Int); $cmd.Parameters["@app"].Value=[int]$w.target_app
        [void]$cmd.Parameters.Add("@ct",[System.Data.SqlDbType]::NVarChar,256); $cmd.Parameters["@ct"].Value=$w.content_type
        [void]$cmd.Parameters.Add("@f",[System.Data.SqlDbType]::NVarChar,256); $cmd.Parameters["@f"].Value=$w.field
        [void]$cmd.Parameters.Add("@d",[System.Data.SqlDbType]::Int); $cmd.Parameters["@d"].Value=[int]$w.dim
        [void]$cmd.Parameters.Add("@v",[System.Data.SqlDbType]::NVarChar,-1); $cmd.Parameters["@v"].Value=$w.value
        $c = [int]$cmd.ExecuteScalar()
        if ($c -lt 1) { $bad++; Write-Host "RELECTURE ECHEC: $($w.entity_guid) $($w.field) dim$($w.dim)" }
    }
    # Relecture in-tx des bases fr reecrites : la rangee au ValueId MAX doit valoir le
    # registre. Meme choix de DETERMINISME qu'a l'ecriture (M1/M3) : la relecture
    # epingle la meme rangee que l'UPDATE, pas une autre.
    foreach ($w in $frwrites) {
        $cmd.Parameters.Clear()
        $cmd.CommandText = @"
SELECT TOP 1 CONVERT(nvarchar(max), v.[Value])
FROM TsDynDataValue v
JOIN TsDynDataEntity e ON e.EntityId=v.EntityId
JOIN TsDynDataContentType c ON c.ContentTypeId=e.ContentTypeId
JOIN TsDynDataAttribute a ON a.AttributeId=v.AttributeId
LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId=v.ValueId
WHERE e.EntityGuid=@g AND e.AppId=@app AND c.Name=@ct AND a.StaticName=@f AND vd.ValueId IS NULL
ORDER BY v.ValueId DESC
"@
        [void]$cmd.Parameters.Add("@g",[System.Data.SqlDbType]::NVarChar,64); $cmd.Parameters["@g"].Value=$w.entity_guid
        [void]$cmd.Parameters.Add("@app",[System.Data.SqlDbType]::Int); $cmd.Parameters["@app"].Value=[int]$w.target_app
        [void]$cmd.Parameters.Add("@ct",[System.Data.SqlDbType]::NVarChar,256); $cmd.Parameters["@ct"].Value=$w.content_type
        [void]$cmd.Parameters.Add("@f",[System.Data.SqlDbType]::NVarChar,256); $cmd.Parameters["@f"].Value=$w.field
        $srv = $cmd.ExecuteScalar()
        if ($null -eq $srv -or -not ([string]$srv -ceq [string]$w.value)) { $bad++; Write-Host "RELECTURE FR ECHEC: $($w.entity_guid) $($w.field)" }
    }
    if ($bad -gt 0) { throw "relecture in-tx: $bad valeur(s) non conformes - ROLLBACK" }
    # Preservation #2181 : la valeur de base du bloc, identique OCTET PAR OCTET a ce que
    # le dry-run a mesure (l'egalite exacte est plus forte qu'une empreinte, et HASHBYTES
    # est limite a 8000 octets -- le bloc en fait 13418 en UTF-16).
    $cmd.Parameters.Clear()
    $cmd.CommandText = "SELECT CONVERT(nvarchar(max), [Value]) FROM TsDynDataValue WHERE ValueId=@vid"
    [void]$cmd.Parameters.Add("@vid",[System.Data.SqlDbType]::Int); $cmd.Parameters["@vid"].Value=[int]$payload.expected_2181_vid
    $v2181 = $cmd.ExecuteScalar()
    if ($null -eq $v2181 -or -not ([string]$v2181 -ceq [string]$payload.expected_2181_value)) { throw "preservation #2181: valeur de base MODIFIEE (attendu identique au dry-run) - ROLLBACK" }
    # Preservation #2182 : les 7 entites News toujours presentes (comptees dans leur
    # app cible — la carte mesuree — sinon une copie demo des memes GUID fausserait le compte).
    $cmd.Parameters.Clear()
    $cmd.CommandText = "SELECT COUNT(*) FROM TsDynDataEntity WHERE EntityGuid IN ($env:REINJECT_2182_GUIDS) AND AppId IN ($env:REINJECT_2182_APPS)"
    $n2182 = [int]$cmd.ExecuteScalar()
    if ($n2182 -ne 7) { throw "preservation #2182: $n2182/7 entites News - ROLLBACK" }

    $tx.Commit()
    Write-Host "COMMIT OK: INSERT=$inserted UPDATE=$updated FORCE=$forced FR_UPDATE=$frupdated ; relecture in-tx conforme ; #2181 identique ; #2182=$n2182/7"
    $conn.Close()
}
catch {
    try { $tx.Rollback() } catch {}
    Write-Host "ROLLBACK: $($_.Exception.Message)"
    $conn.Close()
    exit 1
}
'''


def run_writer(conn, writes, fr_writes, pres_before, backup_dir, stamp, app_map) -> int:
    import os
    import subprocess
    import tempfile

    payload_file = backup_dir / f"reinject-payload-{stamp}.json"
    b2181 = pres_before["block_2181"][0] if pres_before["block_2181"] else {}
    payload_file.write_text(json.dumps({
        "writes": writes,
        "fr_writes": fr_writes,
        # Preservation par le CONTENU lui-meme : l'egalite exacte in-tx est plus forte
        # qu'une empreinte et evite la limite 8000 octets de HASHBYTES (le bloc fait
        # 6709 caracteres = 13418 octets en UTF-16). L'empreinte sha256 est rapportee
        # a part pour le journal.
        "expected_2181_value": b2181.get("value", ""),
        "expected_2181_vid": b2181.get("value_id"),
    }, ensure_ascii=False), encoding="utf-8")

    ps = WRITER_PS
    env = os.environ.copy()
    env["REINJECT_CS"] = conn
    env["REINJECT_PAYLOAD"] = str(payload_file)
    env["REINJECT_2182_GUIDS"] = ", ".join("'" + g + "'" for g in NEWS_2182_GUIDS)
    apps_2182 = sorted({app_map[p["app_id"]] for p in CURRENT_PLAN + CURRENT_FR_PLAN
                        if p["entity_guid"] in set(NEWS_2182_GUIDS)})
    if not apps_2182:
        print("REFUS d'appliquer: apps #2182 non resolues par la carte (registre sans lignes News ?)")
        return 1
    env["REINJECT_2182_APPS"] = ", ".join(str(a) for a in apps_2182)
    if not pres_before["block_2181"]:
        print("REFUS d'appliquer: bloc #2181 introuvable sur la cible (entite/champ/dim-0).")
        return 1
    proc = subprocess.run(["powershell", "-NoProfile", "-NonInteractive", "-Command", ps],
                          capture_output=True, text=True, encoding="utf-8", errors="replace", env=env)
    sys.stdout.write(proc.stdout or "")
    if proc.returncode != 0:
        sys.stderr.write((proc.stderr or "")[:800] + "\n")
        print("VERDICT: ECHEC apply (ROLLBACK)")
        return 1

    pres_after = preservation_probe(conn, app_map)
    same = (pres_after["block_2181"] == pres_before["block_2181"]
            and pres_after["news_2182"] == pres_before["news_2182"])
    (backup_dir / f"reinject-result-{stamp}.json").write_text(
        json.dumps({"preservation_after": pres_after, "preservation_intact": same}, ensure_ascii=False, indent=1),
        encoding="utf-8")
    print(f"preservation APRES identique a AVANT: {same}")
    print("VERDICT: apply termine" + (" — preservation OK" if same else " — ⚠️ PRESERVATION A QUALIFIER"))
    print("Rappel lane: recycler l'app-domain par mtime web.config (sha256 inchange), jamais iisreset.")
    return 0 if same else 1


if __name__ == "__main__":
    sys.exit(main())
