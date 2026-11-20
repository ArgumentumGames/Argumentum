#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#458 / #131 etape 10 (P23-24) - re-injection EAV du registre vers une base cible.

REJOUABLE DEPUIS LE DEPOT. Source = docs/dnn-localization/eav-content.csv (le registre
est le PRODUCTEUR, jamais un miroir). Cible = la base designee par le --config fourni
(web.config lu au moment du geste, jamais imprime). Aucune IP, compte ou chaine de
connexion n'est code en dur.

PERIMETRE. Une rangée de valeur PAR dimension, appariement par (EntityGuid, field) :
le registre ne porte pas d'EntityId (les ids changent d'une base a l'autre — la copie
de prod, le staging et la preprod ne partagent pas les memes), les GUID, eux, sont
stables. fr (dim-0) n'est JAMAIS ecrit : la base dim-0 est ce que sert la culture fr.

  - registre non-vide, dimension ABSENTE       -> INSERT (TsDynDataValue + lien dim)
  - registre non-vide, dimension VIDE en base  -> UPDATE de cette rangée
  - registre non-vide == base                  -> deja present, on saute
  - registre non-vide != base (les deux pleins) -> CONFLIT : aucune ecriture, liste
  - registre vide                              -> on saute (pas d'effacement)

PRESERVATION (controle avant ET apres, dans la meme transaction). Le contenu propre a
la cible ne doit pas bouger : bloc #2181 (ValueID 27942) et les 7 entites #2182
(recherchees par GUID, ids dependants de la base). Un ecart => ROLLBACK, rien n'ecrit.

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


def q(s: str) -> str:
    """Litteral SQL nvarchar, apostrophes doublees."""
    return "N'" + s.replace("'", "''") + "'"


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


def fetch_target(conn) -> dict:
    """Lit la base cible : dimensions, valeurs existantes du perimetre, contenu preserve."""
    # La meme culture existe dans plusieurs zones 2sxc (zone 2 = defaut DNN, zone 3 =
    # apps Argumentum). Les docs d'injection (app 59/35, inject42) utilisent les dims
    # 7-13 = zone 3. On resout la dimension PAR ZONE : la zone de l'app de la cellule,
    # mesuree sur TsDynDataApp.ZoneId. Ne jamais prendre le premier en-US venu.
    app_ids = sorted({p["app_id"] for p in CURRENT_PLAN})
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

    guids = sorted({p["entity_guid"] for p in CURRENT_PLAN})
    in_guids = ", ".join("'" + g + "'" for g in guids)
    sql = f"""
SET NOCOUNT ON;
SELECT CONVERT(nvarchar(64), e.EntityGuid) AS entity_guid,
       e.EntityId AS entity_id,
       a.StaticName AS field,
       vd.DimensionId AS dimension_id,
       CONVERT(nvarchar(max), v.Value) AS val
FROM TsDynDataEntity e
JOIN TsDynDataContentType ct ON ct.ContentTypeId = e.ContentTypeId
JOIN TsDynDataValue v ON v.EntityId = e.EntityId
JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId AND a.TransDeletedId IS NULL
LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId
WHERE e.TransDeletedId IS NULL
  AND CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids})
  AND ct.Name NOT IN ('RawData')
ORDER BY entity_guid, field, dimension_id;
"""
    rows = EXP.run_query(conn, sql)
    # index: (GUID, field, dimId-or-0) -> value
    existing = {}
    entities = {}
    for row in rows:
        g = row["entity_guid"].upper()
        entities[g] = int(row["entity_id"])
        dim = int(row["dimension_id"]) if str(row["dimension_id"]).strip() not in ("", "None") else 0
        existing[(g, row["field"], dim)] = row["val"] or ""
    return {"lang_dim_by_app": lang_dim_by_app, "existing": existing, "entities": entities}


def preservation_probe(conn) -> dict:
    """Capture l'etat du contenu preserve (AVANT et APRES doivent etre identiques)."""
    out = {}
    r1 = EXP.run_query(conn,
        "SET NOCOUNT ON; SELECT ValueId AS vid, LEN(CAST(Value AS nvarchar(max))) AS len "
        "FROM TsDynDataValue WHERE ValueId = 27942;")
    out["block_2181_valueid_27942"] = [
        {"value_id": int(x["vid"]), "len": int(x["len"])} for x in r1]
    in_guids = ", ".join("'" + g + "'" for g in NEWS_2182_GUIDS)
    r2 = EXP.run_query(conn,
        "SET NOCOUNT ON; SELECT CONVERT(nvarchar(64), e.EntityGuid) AS g, e.EntityId AS id "
        f"FROM TsDynDataEntity e WHERE CONVERT(nvarchar(64), e.EntityGuid) IN ({in_guids});")
    out["news_2182"] = sorted(
        ({"guid": x["g"].upper(), "entity_id": int(x["id"])} for x in r2),
        key=lambda d: d["guid"])
    return out


def compute_actions(plan, target):
    existing = target["existing"]
    lang_dim_by_app = target["lang_dim_by_app"]
    actions = []
    for p in plan:
        g, field, lang = p["entity_guid"], p["field"], p["lang"]
        dim = lang_dim_by_app[p["app_id"]][lang]
        base = existing.get((g, field, 0))
        if base is None:
            actions.append({**p, "op": "MISSING_ENTITY_OR_FIELD"})
            continue
        cur = existing.get((g, field, dim))
        if cur is None:
            actions.append({**p, "op": "INSERT", "dim": dim})
        elif cur == "":
            actions.append({**p, "op": "UPDATE", "dim": dim})
        elif cur == p["value"]:
            actions.append({**p, "op": "SKIP_SAME", "dim": dim})
        else:
            actions.append({**p, "op": "CONFLICT", "dim": dim, "db_value": cur})
    return actions


def main(argv=None) -> int:
    global CURRENT_PLAN
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--config", default=str(EXP.DEFAULT_CONFIG))
    ap.add_argument("--apply", action="store_true", help="ecrit (defaut: dry-run)")
    ap.add_argument("--backup-dir", default=None, help="requis en --apply, hors depot")
    ap.add_argument("--registry", default=None,
                    help="chemin du registre CSV (defaut: docs/dnn-localization/eav-content.csv du depot)")
    args = ap.parse_args(argv)

    reg = read_registry(args.registry)
    CURRENT_PLAN = build_plan(reg)
    conn = EXP.read_connection_string(pathlib.Path(args.config))
    target = fetch_target(conn)
    actions = compute_actions(CURRENT_PLAN, target)

    counts = {}
    for a in actions:
        counts[a["op"]] = counts.get(a["op"], 0) + 1
    print(f"registre: {len(reg)} lignes, {len(CURRENT_PLAN)} cellules traductibles non vides")
    print(f"entites resolues sur la cible: {len(target['entities'])}")
    for op in sorted(counts):
        print(f"  {op}: {counts[op]}")

    pres_before = preservation_probe(conn)
    print(f"preservation AVANT: #2181 ValueId27942={pres_before['block_2181_valueid_27942']}, "
          f"#2182 entites={len(pres_before['news_2182'])}/7")

    conflicts = [a for a in actions if a["op"] == "CONFLICT"]
    missing = [a for a in actions if a["op"] == "MISSING_ENTITY_OR_FIELD"]
    if missing:
        print(f"\n{len(missing)} cellule(s) sans entite/champ resolu sur la cible (extrait):")
        for a in missing[:15]:
            print(f"  - {a['entity_guid'][:8]} {a['content_type']}.{a['field']} [{a['lang']}]")
    if conflicts:
        print(f"\n{len(conflicts)} CONFLIT(S) (les deux pleins, differents) — aucune ecriture pour celles-ci:")
        for a in conflicts[:15]:
            print(f"  - {a['entity_guid'][:8]} {a['field']} [{a['lang']}] "
                  f"db={len(a['db_value'])}c registre={len(a['value'])}c")

    writes = [a for a in actions if a["op"] in ("INSERT", "UPDATE")]
    if not args.apply:
        print(f"\nDRY-RUN: {len(writes)} ecriture(s) prevues "
              f"({counts.get('INSERT',0)} INSERT + {counts.get('UPDATE',0)} UPDATE). "
              f"Aucune ecriture effectuee.")
        print("VERDICT: " + ("ECHEC (conflits ou cibles manquantes a arbitrer)" if (conflicts or missing)
                             else "plan coherent, pret pour --apply"))
        return 1 if (conflicts or missing) else 0

    if conflicts or missing:
        print("REFUS d'appliquer: conflits ou cibles manquantes non arbitres.")
        return 1
    if not args.backup_dir:
        print("REFUS: --apply exige --backup-dir (hors depot).")
        return 1
    backup_dir = pathlib.Path(args.backup_dir)
    backup_dir.mkdir(parents=True, exist_ok=True)
    stamp = datetime.datetime.now(datetime.UTC).strftime("%Y%m%dT%H%M%SZ")
    (backup_dir / f"reinject-plan-{stamp}.json").write_text(
        json.dumps({"writes": [{k: a[k] for k in ('entity_guid','field','lang','op','dim','value')} for a in writes],
                    "preservation_before": pres_before}, ensure_ascii=False, indent=1),
        encoding="utf-8")
    print(f"plan + preservation AVANT sauvegardes dans {backup_dir} ({len(writes)} ecritures)")
    return run_writer(conn, writes, pres_before, backup_dir, stamp)


# ---------------------------------------------------------------------------
# Moteur d'ecriture : un script PowerShell genere, UNE transaction, relecture in-tx.
# Modele app 59 / inject42 : INSERT d'une rangee de valeur par dimension + lien
# TsDynDataValueDimension ; UPDATE des rangees dimensionnees vides. Appariement par
# (EntityId resolu du GUID, AttributeId resolu du (ContentType, field), DimensionId).
# ---------------------------------------------------------------------------
def run_writer(conn, writes, pres_before, backup_dir, stamp) -> int:
    import os
    import subprocess
    import tempfile

    payload_file = backup_dir / f"reinject-payload-{stamp}.json"
    payload_file.write_text(json.dumps({"writes": writes}, ensure_ascii=False), encoding="utf-8")

    ps = r'''
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Data
$writes = (Get-Content -LiteralPath $env:REINJECT_PAYLOAD -Raw -Encoding UTF8 | ConvertFrom-Json).writes
if ($writes -isnot [System.Array]) { $writes = @($writes) }
$conn = New-Object System.Data.SqlClient.SqlConnection($env:REINJECT_CS)
$conn.Open()
$tx = $conn.BeginTransaction()
try {
    $cmd = $conn.CreateCommand(); $cmd.Transaction = $tx; $cmd.CommandTimeout = 600
    $log = New-Object System.Collections.Generic.List[object]
    $inserted = 0; $updated = 0
    foreach ($w in $writes) {
        $guid = $w.entity_guid; $field = $w.field; $ct = $w.content_type; $val = $w.value
        # Resoudre EntityId + AttributeId (UPDLOCK/HOLDLOCK) a l'interieur de la transaction.
        $cmd.Parameters.Clear()
        $cmd.CommandText = @"
SELECT e.EntityId, a.AttributeId FROM TsDynDataEntity e WITH (UPDLOCK,HOLDLOCK)
JOIN TsDynDataContentType c ON c.ContentTypeId = e.ContentTypeId
JOIN TsDynDataAttribute a WITH (UPDLOCK,HOLDLOCK) ON a.ContentTypeId = c.ContentTypeId
WHERE e.EntityGuid = @g AND c.Name = @ct AND a.StaticName = @f AND a.TransDeletedId IS NULL AND e.TransDeletedId IS NULL
"@
        [void]$cmd.Parameters.Add("@g", [System.Data.SqlDbType]::NVarChar, 64); $cmd.Parameters["@g"].Value = $guid
        [void]$cmd.Parameters.Add("@ct", [System.Data.SqlDbType]::NVarChar, 256); $cmd.Parameters["@ct"].Value = $ct
        [void]$cmd.Parameters.Add("@f", [System.Data.SqlDbType]::NVarChar, 256); $cmd.Parameters["@f"].Value = $field
        $r = $cmd.ExecuteReader()
        if (-not $r.Read()) { $r.Close(); throw "entite/attribut introuvable: $guid $ct.$field" }
        $eid = $r.GetInt32(0); $aid = $r.GetInt32(1); $r.Close()

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
        }
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
WHERE e.EntityGuid=@g AND c.Name=@ct AND a.StaticName=@f AND vd.DimensionId=@d AND v.[Value]=@v
"@
        [void]$cmd.Parameters.Add("@g",[System.Data.SqlDbType]::NVarChar,64); $cmd.Parameters["@g"].Value=$w.entity_guid
        [void]$cmd.Parameters.Add("@ct",[System.Data.SqlDbType]::NVarChar,256); $cmd.Parameters["@ct"].Value=$w.content_type
        [void]$cmd.Parameters.Add("@f",[System.Data.SqlDbType]::NVarChar,256); $cmd.Parameters["@f"].Value=$w.field
        [void]$cmd.Parameters.Add("@d",[System.Data.SqlDbType]::Int); $cmd.Parameters["@d"].Value=[int]$w.dim
        [void]$cmd.Parameters.Add("@v",[System.Data.SqlDbType]::NVarChar,-1); $cmd.Parameters["@v"].Value=$w.value
        $c = [int]$cmd.ExecuteScalar()
        if ($c -lt 1) { $bad++; Write-Host "RELECTURE ECHEC: $($w.entity_guid) $($w.field) dim$($w.dim)" }
    }
    if ($bad -gt 0) { throw "relecture in-tx: $bad valeur(s) non conformes - ROLLBACK" }
    # Preservation #2181 : ValueId 27942 present, longueur inchangee.
    $cmd.Parameters.Clear()
    $cmd.CommandText = "SELECT LEN(CAST([Value] AS nvarchar(max))) FROM TsDynDataValue WHERE ValueId=27942"
    $len2181 = $cmd.ExecuteScalar()
    if ($null -eq $len2181 -or [int]$len2181 -ne [int]$env:REINJECT_2181_LEN) { throw "preservation #2181: ValueId27942 len=$len2181 attendu $env:REINJECT_2181_LEN - ROLLBACK" }
    # Preservation #2182 : les 7 entites News toujours presentes.
    $cmd.Parameters.Clear()
    $cmd.CommandText = "SELECT COUNT(*) FROM TsDynDataEntity WHERE EntityGuid IN ($env:REINJECT_2182_GUIDS)"
    $n2182 = [int]$cmd.ExecuteScalar()
    if ($n2182 -ne 7) { throw "preservation #2182: $n2182/7 entites News - ROLLBACK" }

    $tx.Commit()
    Write-Host "COMMIT OK: INSERT=$inserted UPDATE=$updated ; relecture in-tx conforme ; #2181 len=$len2181 ; #2182=$n2182/7"
    $conn.Close()
}
catch {
    try { $tx.Rollback() } catch {}
    Write-Host "ROLLBACK: $($_.Exception.Message)"
    $conn.Close()
    exit 1
}
'''
    env = os.environ.copy()
    env["REINJECT_CS"] = conn
    env["REINJECT_PAYLOAD"] = str(payload_file)
    len2181 = pres_before["block_2181_valueid_27942"][0]["len"] if pres_before["block_2181_valueid_27942"] else -1
    env["REINJECT_2181_LEN"] = str(len2181)
    env["REINJECT_2182_GUIDS"] = ", ".join("'" + g + "'" for g in NEWS_2182_GUIDS)
    proc = subprocess.run(["powershell", "-NoProfile", "-NonInteractive", "-Command", ps],
                          capture_output=True, text=True, encoding="utf-8", errors="replace", env=env)
    sys.stdout.write(proc.stdout or "")
    if proc.returncode != 0:
        sys.stderr.write((proc.stderr or "")[:800] + "\n")
        print("VERDICT: ECHEC apply (ROLLBACK)")
        return 1

    pres_after = preservation_probe(conn)
    same = (pres_after["block_2181_valueid_27942"] == pres_before["block_2181_valueid_27942"]
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


if __name__ == "__main__":
    sys.exit(main())
