#!/usr/bin/env python3
"""#458 grain 24b - 2sxc EAV content export + dry-run verifier (READ-ONLY).

Exports the *displayed* 2sxc content types of the Argumentum test site to a CSV
with one row per (entity, field) and one column per language, then verifies that
re-importing that CSV unchanged would change **zero** value.

WHY A WHITELIST
    The repository is public. The same database also holds member data
    (orders, clients, user data), technical settings and legacy content that no
    page renders. Only the measured "displayed content" set is exported - see
    the grain 24b step-1 measurement on issue #458 (comment 6301456941):
    apps mounted on portal pages, editorial / label / credit content types.

WHY THE KEY IS `entity_guid|field`
    One row per field of one entity. `EntityGuid` alone is NOT unique database
    wide (9493 entities, 5822 distinct GUIDs - the same content type is copied
    across apps), but it IS unique inside this whitelist (rows == distinct GUIDs
    for every whitelisted type, measured 2026-10-20). The verifier asserts that
    uniqueness and fails loud if it ever stops holding.

WHY NOT `FOR JSON PATH`
    SQL Server truncates a `FOR JSON` result at exactly 2033 characters (measured
    2026-10-20 on this base: 2033/4067/6101 line breaks, identical through
    sqlcmd AND through ADO.NET `GetString`). The same rows read one by one
    through `SqlDataReader` come back whole (13314-char value verified). This
    tool therefore streams rows: PowerShell writes one JSON object per line,
    Python pivots nothing - the SQL already returns one line per (entity, field).

READ-ONLY
    The tool runs SELECTs only. The connection string is read from
    DNNPlatform/web.config at runtime and is never printed.

USAGE
    python tools/458-eav-content-export.py export --out docs/dnn-localization/eav-content.csv
    python tools/458-eav-content-export.py verify --csv docs/dnn-localization/eav-content.csv
    python tools/458-eav-content-export.py selftest

    export    write the CSV (deterministic: sorted, LF, UTF-8 no BOM)
    verify    re-read the DB and compare every cell of the CSV; exit 1 on any diff
    verify --classify [--out-classified F]
              bucket every diff cell into the three classes of decision 3 on
              #458 (c.6405473398): registre_avance (CSV filled, DB empty ->
              injection candidate), base_avance (CSV empty, DB filled ->
              pull-back-into-registry-by-PR candidate), conflit (both filled,
              different -> no write, list for ai-01). --out-classified also
              writes the full cell values per class as CSV, for the injection
              and pull-back work of the following ticks.
    selftest  inverse control: export -> verify (0 diff) -> mutate one cell ->
              verify (exactly 1 diff) -> classify the three mutation shapes
              (one per class, exactly). Proves the verifier - and the
              classifier - can see a difference, in the right bucket.
"""
from __future__ import annotations

import argparse
import csv
import json
import os
import pathlib
import re
import subprocess
import sys
import tempfile

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent
DEFAULT_CONFIG = REPO_ROOT / "DNNPlatform" / "web.config"

# ---------------------------------------------------------------------------
# Language dimensions. The site's primary culture is fr-FR (Portals.DefaultLanguage),
# so the FRENCH source is the value stored with NO dimension row; translations
# live in the culture dimensions below. Measured 2026-10-20: inside the whitelist
# scope only dims 7-13 carry values (no fr-FR overlay), so "no dimension" == fr.
# ---------------------------------------------------------------------------
LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
DIM_EXTERNAL_KEY = {
    "en": "en-US",
    "ru": "ru-RU",
    "pt": "pt-BR",
    "es": "es-ES",
    "ar": "ar-SA",
    "fa": "fa-IR",
    "zh": "zh-CN",
}

# ---------------------------------------------------------------------------
# Displayed content whitelist (app_id, content_type, kind).
# `kind`: editorial = page prose | label = visible app label | credit = credits.
# Deliberately EXCLUDED: Fallacy (1013 entities, CSV-sourced and not displayed -
# the only mounted module renders _RulesExplorer_RuleList), all *Settings,
# PresSetText, Effect, VideoPresentation, PresSetMap,
# LayoutElement, TestContent (demo), Comment/Scenario (empty), and every
# NBrightBuy table (orders/clients/user data - never exported).
# ViewConfiguration is whitelisted for app 52 only, and only for its `Title`
# field (see FIELD_ALLOW): that is where the /xx/Actus h1 lives.
# ---------------------------------------------------------------------------
WHITELIST = [
    (33, "Content", "editorial"),
    (33, "Link", "editorial"),
    (33, "Video", "editorial"),
    (33, "Location", "editorial"),
    (33, "Person", "editorial"),
    (33, "Image", "editorial"),
    (33, "App-Resources", "label"),
    (35, "Accordion", "editorial"),
    (35, "App-Resources", "label"),
    (44, "Person", "editorial"),
    (47, "Link", "editorial"),
    (47, "App-Resources", "label"),
    # Newsletter (home page, accordion item 10172): the served Sendinblue form is
    # app 48 E10181 `Html` - 13314 chars, dim 0 only, reached through
    # ContentGroupReference E10179 -> E10180 -> E10181 and rendered by
    # SnippetInject3/_SnippetInject.cshtml as `@Html.Raw(Content.Html)`
    # (docs/dnn-localization/458-newsletter-store-chain-2026-10-26.md). Only
    # `Html` is exported (FIELD_ALLOW); the form skeleton inside it (action,
    # hidden fields, input names) is frozen by the translation grain, not here.
    (48, "HtmlFragment", "editorial"),
    (49, "Slider", "editorial"),
    (49, "Slide", "editorial"),
    # Contact form (home page, accordion item 10182): MobiusForms5 reads every
    # visible string from `Resources.*`, i.e. app 50 App-Resources (E9169, all
    # values dim 0 only). Only the fields the contact form's views read are
    # exported (FIELD_ALLOW). The same app stores visitor submissions as
    # `RawData` entities - personal data, see FORBIDDEN_TYPES.
    (50, "App-Resources", "label"),
    (52, "News", "editorial"),
    (52, "Category", "editorial"),
    (52, "App-Resources", "label"),
    # The h1 of /xx/Actus lives in the *view configuration* of the mounted list
    # module, NOT in a `Content` entity: app 52 has no content type named
    # `Content` at all (measured 2026-10-25: its types are App-Resources,
    # App-Settings, Category, News, ViewConfiguration). The earlier entry
    # `(52, "Content")` matched zero rows and is replaced by this one.
    # Carrier: EntityGuid 438836CC-C11A-4B56-B81C-9919E8BD44F1, field `Title`,
    # value "Dernières nouvelles", **0 dimension row** -> translatable and
    # untranslated. Two independent instruments agree: the DB value, and the
    # served page (h1 "Dernières nouvelles" on 8/8 while `<title>` is localized
    # 8/8: News / Novedades / Novidades / Новости / 新闻 / الأخبار / اخبار).
    (52, "ViewConfiguration", "editorial"),
    (59, "Term", "editorial"),
    (59, "App-Resources", "label"),
    (60, "Game Rule", "editorial"),
    (60, "Album", "credit"),
    (60, "License", "credit"),
    (60, "Contributor", "credit"),
    (60, "ImageMetadata", "credit"),
    (60, "AlbumPresentation", "credit"),
    (60, "App-Resources", "label"),
]

KIND_BY_TYPE = {(app, ct): kind for app, ct, kind in WHITELIST}
ALLOWED_TYPES = set(KIND_BY_TYPE)

# Per-type field allowlist, applied after the SQL read. Used where a whitelisted
# type mixes displayed prose with settings: (52, ViewConfiguration) carries the
# Actus h1 in `Title` and also ItemsInPage / ShowPagination, which are settings
# and stay out by the same rule that excludes the *Settings types. Types absent
# from this dict are exported field for field, as before.
FIELD_ALLOW = {
    (48, "HtmlFragment"): {"Html"},
    # Derived from the consumer, not from the database: every `Resources.<Field>`
    # read by MobiusForms5/live/_Contact Form.cshtml and the files it pulls in
    # (Html.Partial / CreateInstance, comments ignored), plus "Label" + <id> for
    # each FieldBuilder call. The Mail* / Owner* / Customer* mail templates of the
    # same entity are not read by the form and stay out.
    # tools/test_458-eav-export-guards.py re-derives this set from the views.
    (50, "App-Resources"): {
        "ButtonSend", "GdprLabel", "LabelMessage", "LabelRequired",
        "LabelSenderMail", "LabelSenderName", "LabelSubject", "LabelValidEmail",
        "LabelValidFile", "MessageContainsRecaptcha", "MessageCustomerMailSend",
        "MessageDefaultMailChimpKey", "MessageEnableMailchimp",
        "MessageFormIncomplete", "MessageMailFromError", "MessageMailOwnerError",
        "MessageNewsletterSuccess", "MessageNewsletterUnexpectedError",
        "MessageRecaptchaMissing", "MessageRecaptchaWarning", "MessageSendError",
        "MessageSendOk", "MessageSending", "TermsLabel",
    },
    (52, "ViewConfiguration"): {"Title"},
}

# Per-type entity allowlist (EntityId), applied after the SQL read like
# FIELD_ALLOW. HtmlFragment holds arbitrary HTML, so a field bound is not enough:
# any other fragment of app 48 (draft, unserved, demo) would be published with
# it. Bounded to the measured carrier E10181 (store-chain doc, link 6); the
# WITNESSES entry proves at export that this id is the Sendinblue form, so a
# wrong id fails loud instead of exporting nothing. EntityId, not EntityGuid:
# the GUID is not in the measurement docs - it lands in the registry key with
# the row, and a later change can pin it there. App 50 is not entity-bounded:
# after FIELD_ALLOW it carries labels only, and which of its App-Resources
# entities is served is settled at injection time, not at export.
ENTITY_ALLOW = {
    (48, "HtmlFragment"): {10181},
}

# Visitor submissions of MobiusForms5 (app 50) are stored as `RawData` entities:
# personal data, never exported, whatever the whitelist says. Enforced three
# times: at import (no whitelist entry may name it), in the SQL (excluded by
# name), and on every row read (assert_export_safe). Compared case-folded: the
# base collation is French_CI_AS, so SQL would match `rawdata` as `RawData`.
FORBIDDEN_TYPES = frozenset({"RawData"})

# Defense in depth: whatever happens, never emit a row for a private/store table.
FORBIDDEN_TOKENS = (
    "ORDER", "CLIENT", "USERDATA", "NBrightBuy", "AspNetUsers", "RawData",
)

# Positive controls. A whitelist entry that matches nothing exports nothing and
# stays silent - the earlier `(52, "Content")` entry did exactly that. Each
# witness is the start of a value served on the home page (measured 2026-10-27:
# the Sendinblue block is 13314 chars, the length of E10181 `Html`; the contact
# button renders `@Resources.ButtonSend` as "Envoyer"); export and selftest fail
# if one is missing. (app, content_type, field, fr prefix)
WITNESSES = [
    (48, "HtmlFragment", "Html", "<!-- Begin Sendinblue Form -->"),
    (50, "App-Resources", "ButtonSend", "Envoyer"),
]


def assert_whitelist_safe(whitelist=None, field_allow=None, entity_allow=None) -> None:
    """Fail at import if the configuration itself names a forbidden type."""
    whitelist = WHITELIST if whitelist is None else whitelist
    field_allow = FIELD_ALLOW if field_allow is None else field_allow
    entity_allow = ENTITY_ALLOW if entity_allow is None else entity_allow
    folded = {t.casefold() for t in FORBIDDEN_TYPES}
    named = sorted({ct for _app, ct, _kind in whitelist if ct.casefold() in folded})
    if named:
        raise SystemExit(f"type interdit dans la liste blanche: {', '.join(named)}")
    allowed = {(app, ct) for app, ct, _kind in whitelist}
    for name, table in (("FIELD_ALLOW", field_allow), ("ENTITY_ALLOW", entity_allow)):
        orphans = sorted(set(table) - allowed)
        if orphans:
            raise SystemExit(f"{name} hors liste blanche: {orphans}")


assert_whitelist_safe()

COLUMNS = [
    "key", "app_id", "content_type", "entity_guid", "field", "field_type",
    "value_kind", "fr", *LANGS,
]

# Streams rows as JSON lines. No `FOR JSON`, no scalar aggregate: SQL Server caps
# a FOR JSON result at 2033 characters, a per-row reader does not.
PS_READER = r"""
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Data
$sql = [System.IO.File]::ReadAllText($env:EAV_SQL_FILE)
$enc = New-Object System.Text.UTF8Encoding($false)
$writer = New-Object System.IO.StreamWriter($env:EAV_JSONL, $false, $enc)
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($env:EAV_CS)
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandTimeout = 600
    $cmd.CommandText = $sql
    $reader = $cmd.ExecuteReader()
    $names = @()
    for ($i = 0; $i -lt $reader.FieldCount; $i++) { $names += $reader.GetName($i) }
    while ($reader.Read()) {
        $row = [ordered]@{}
        for ($i = 0; $i -lt $names.Count; $i++) {
            if ($reader.IsDBNull($i)) { $row[$names[$i]] = "" }
            else { $row[$names[$i]] = [string]$reader.GetValue($i) }
        }
        $writer.WriteLine(($row | ConvertTo-Json -Compress))
    }
    $reader.Close()
    $conn.Close()
}
finally { $writer.Close() }
"""


# ---------------------------------------------------------------------------
# Connection (never printed)
# ---------------------------------------------------------------------------
def read_connection_string(config_path: pathlib.Path) -> str:
    """Return the SiteSqlServer connection string. Never logged, never written."""
    if not config_path.is_file():
        raise SystemExit(f"web.config introuvable: {config_path}")
    text = config_path.read_text(encoding="utf-8", errors="replace")
    m = re.search(
        r'<add\s+name="SiteSqlServer"[^>]*connectionString="([^"]+)"', text
    )
    if not m:
        raise SystemExit("connectionString SiteSqlServer introuvable dans web.config")
    return m.group(1)


def run_query(connection_string: str, sql: str) -> list[dict]:
    """Run one SELECT through SqlDataReader; return one dict per row."""
    with tempfile.TemporaryDirectory(prefix="eav-query-") as tmp:
        sql_file = pathlib.Path(tmp) / "q.sql"
        jsonl_file = pathlib.Path(tmp) / "rows.jsonl"
        sql_file.write_text(sql, encoding="utf-8")
        env = os.environ.copy()
        env.update(
            EAV_CS=connection_string,
            EAV_SQL_FILE=str(sql_file),
            EAV_JSONL=str(jsonl_file),
        )
        proc = subprocess.run(
            ["powershell", "-NoProfile", "-NonInteractive", "-Command", PS_READER],
            capture_output=True,
            text=True,
            encoding="utf-8",
            errors="replace",
            env=env,
        )
        if proc.returncode != 0:
            # Never echo the connection string; only the SQL error text.
            raise SystemExit(
                f"lecture SQL échouée (rc={proc.returncode}): "
                f"{(proc.stderr or proc.stdout)[:600]}"
            )
        rows: list[dict] = []
        if jsonl_file.is_file():
            with jsonl_file.open("r", encoding="utf-8") as handle:
                for line in handle:
                    line = line.strip()
                    if not line:
                        continue
                    try:
                        rows.append(json.loads(line))
                    except json.JSONDecodeError as exc:
                        raise SystemExit(
                            f"ligne JSON invalide ({exc}): {line[:200]!r}"
                        ) from exc
        return rows


def language_dimensions(connection_string: str) -> dict[str, list[int]]:
    """Resolve culture dimensions by ExternalKey (ids differ across installs)."""
    names = "', '".join(DIM_EXTERNAL_KEY.values())
    rows = run_query(
        connection_string,
        "SET NOCOUNT ON; SELECT DimensionId AS id, ExternalKey AS ext "
        "FROM TsDynDataDimension "
        f"WHERE ExternalKey IN ('{names}') AND Active = 1 "
        "ORDER BY DimensionId;",
    )
    by_ext: dict[str, list[int]] = {}
    for row in rows:
        by_ext.setdefault(row["ext"], []).append(int(row["id"]))
    missing = [lang for lang, ext in DIM_EXTERNAL_KEY.items() if ext not in by_ext]
    if missing:
        raise SystemExit(f"dimensions de culture absentes: {', '.join(missing)}")
    return {lang: by_ext[DIM_EXTERNAL_KEY[lang]] for lang in LANGS}


def where_clause() -> str:
    grouped: dict[int, list[str]] = {}
    for app, content_type, _kind in WHITELIST:
        grouped.setdefault(app, []).append(content_type)
    parts = []
    for app, names in sorted(grouped.items()):
        quoted = ", ".join("'" + n.replace("'", "''") + "'" for n in names)
        parts.append(f"(ct.AppId = {app} AND ct.Name IN ({quoted}))")
    forbidden = ", ".join(
        "'" + n.replace("'", "''") + "'" for n in sorted(FORBIDDEN_TYPES)
    )
    return f"({' OR '.join(parts)}) AND ct.Name NOT IN ({forbidden})"


def apply_field_allow(rows: list[dict]) -> list[dict]:
    """Keep, for the types listed in FIELD_ALLOW, only their allowed fields."""
    return [
        row
        for row in rows
        if (int(row["app_id"]), row["content_type"]) not in FIELD_ALLOW
        or row["field"] in FIELD_ALLOW[(int(row["app_id"]), row["content_type"])]
    ]


def entity_allowed(row: dict) -> bool:
    """False for a row of an ENTITY_ALLOW type whose EntityId is absent or not listed."""
    ctype = (int(row["app_id"]), row["content_type"])
    if ctype not in ENTITY_ALLOW:
        return True
    eid = str(row.get("entity_id") or "")
    return eid.isdigit() and int(eid) in ENTITY_ALLOW[ctype]


def apply_entity_allow(rows: list[dict]) -> list[dict]:
    """Keep, for the types listed in ENTITY_ALLOW, only their allowed entities."""
    return [row for row in rows if entity_allowed(row)]


def fetch_rows(connection_string: str) -> list[dict]:
    """Query the whitelisted content and return one dict per (entity, field)."""
    dims = language_dimensions(connection_string)
    cols = []
    for lang in LANGS:
        cond = " OR ".join(f"vd.DimensionId = {i}" for i in dims[lang])
        cols.append(
            f"MAX(CASE WHEN ({cond}) THEN CAST(v.Value AS nvarchar(max)) END) AS {lang}"
        )
    pivot_cols = ",\n               ".join(cols)
    selected_langs = ", ".join(f"ISNULL(p.{lang}, '') AS {lang}" for lang in LANGS)
    sql = f"""
SET NOCOUNT ON;
SELECT ct.AppId AS app_id,
       ct.Name AS content_type,
       e.EntityId AS entity_id,
       CONVERT(nvarchar(64), e.EntityGuid) AS entity_guid,
       a.StaticName AS field,
       a.Type AS field_type,
       a.SortOrder AS sort_order,
       ISNULL(p.fr, '') AS fr,
       {selected_langs}
FROM (
    SELECT v.EntityId,
           v.AttributeId,
           MAX(CASE WHEN vd.ValueId IS NULL THEN CAST(v.Value AS nvarchar(max)) END) AS fr,
           {pivot_cols}
    FROM TsDynDataValue v
    LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId
    GROUP BY v.EntityId, v.AttributeId
) AS p
JOIN TsDynDataEntity e ON e.EntityId = p.EntityId AND e.TransDeletedId IS NULL
JOIN TsDynDataContentType ct ON ct.ContentTypeId = e.ContentTypeId
JOIN TsDynDataAttribute a ON a.AttributeId = p.AttributeId
     AND a.TransDeletedId IS NULL
     AND a.Type IN ('String', 'Hyperlink')
WHERE {where_clause()}
ORDER BY ct.AppId, ct.Name, CONVERT(nvarchar(64), e.EntityGuid), a.SortOrder;
"""
    return finish_rows(run_query(connection_string, sql))


def finish_rows(rows: list[dict]) -> list[dict]:
    """Derive kind and key, then apply the post-read bounds (no base access)."""
    for row in rows:
        row["value_kind"] = KIND_BY_TYPE.get(
            (int(row["app_id"]), row["content_type"]), ""
        )
        row["key"] = f'{row["entity_guid"]}|{row["field"]}'
    # Applied here, not in the SQL, so that export and verify see the same rows.
    return apply_field_allow(apply_entity_allow(rows))


# ---------------------------------------------------------------------------
# Guardrails
# ---------------------------------------------------------------------------
def assert_no_forbidden(rows: list[dict]) -> None:
    """No forbidden type, no forbidden token - for DB rows and registry rows alike."""
    folded = {t.casefold() for t in FORBIDDEN_TYPES}
    for row in rows:
        if row["content_type"].casefold() in folded:
            raise SystemExit(
                f"type interdit dans l'export: {(int(row['app_id']), row['content_type'])}"
            )
    blob = json.dumps(rows, ensure_ascii=False)
    for token in FORBIDDEN_TOKENS:
        if token in blob:
            raise SystemExit(f"jeton interdit dans l'export: {token}")


def assert_export_safe(rows: list[dict]) -> None:
    # First, so a forbidden type is refused for its own reason, not as "hors liste".
    assert_no_forbidden(rows)
    seen: dict[str, str] = {}
    for row in rows:
        ctype = (int(row["app_id"]), row["content_type"])
        if ctype not in ALLOWED_TYPES:
            raise SystemExit(f"type hors liste blanche: {ctype}")
        # DB rows always carry entity_id (selected by the SQL): an absent id on
        # an ENTITY_ALLOW type is a refusal, never a pass.
        if not entity_allowed(row):
            raise SystemExit(
                f"entité hors liste: {ctype} EntityId={row.get('entity_id')!r}"
            )
        if row["key"] in seen:
            raise SystemExit(f"clé dupliquée (EntityGuid non unique): {row['key']}")
        seen[row["key"]] = row["content_type"]


def assert_witnesses(rows: list[dict]) -> None:
    """Every WITNESSES carrier must come back from the base, or nothing is written."""
    for app, ctype, field, prefix in WITNESSES:
        if not any(
            int(row["app_id"]) == app
            and row["content_type"] == ctype
            and row["field"] == field
            and (row.get("fr") or "").startswith(prefix)
            for row in rows
        ):
            raise SystemExit(
                f"témoin absent: ({app}, {ctype}).{field} commençant par {prefix!r}"
            )


# ---------------------------------------------------------------------------
# export
# ---------------------------------------------------------------------------
def write_csv(rows: list[dict], out_path: pathlib.Path) -> None:
    out_path.parent.mkdir(parents=True, exist_ok=True)
    rows = sorted(
        rows,
        key=lambda r: (
            int(r["app_id"]),
            r["content_type"],
            r["entity_guid"],
            int(r["sort_order"]),
        ),
    )
    with out_path.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(
            handle, fieldnames=COLUMNS, lineterminator="\n", extrasaction="ignore"
        )
        writer.writeheader()
        for row in rows:
            writer.writerow({col: row.get(col, "") for col in COLUMNS})


def read_csv(csv_path: pathlib.Path) -> list[dict]:
    if not csv_path.is_file():
        raise SystemExit(f"CSV introuvable: {csv_path}")
    with csv_path.open("r", encoding="utf-8", newline="") as handle:
        reader = csv.DictReader(handle)
        missing = [c for c in COLUMNS if c not in (reader.fieldnames or [])]
        if missing:
            raise SystemExit(f"colonnes absentes du CSV: {', '.join(missing)}")
        return list(reader)


# ---------------------------------------------------------------------------
# verify
# ---------------------------------------------------------------------------
def compare(csv_rows: list[dict], db_rows: list[dict]) -> tuple[list[str], int]:
    """Compare CSV against DB. Returns (problems, diff_count)."""
    problems: list[str] = []
    csv_by_key = {row["key"]: row for row in csv_rows}
    db_by_key = {row["key"]: row for row in db_rows}

    for row in csv_rows:
        ctype = (int(row["app_id"]), row["content_type"])
        if ctype not in ALLOWED_TYPES:
            problems.append(f"type hors liste blanche dans le CSV: {ctype}")

    missing = sorted(set(db_by_key) - set(csv_by_key))
    extra = sorted(set(csv_by_key) - set(db_by_key))
    for key in missing[:20]:
        problems.append(f"clé absente du CSV: {key}")
    for key in extra[:20]:
        problems.append(f"clé absente de la base: {key}")

    diffs = 0
    for key in sorted(set(csv_by_key) & set(db_by_key)):
        csv_row, db_row = csv_by_key[key], db_by_key[key]
        for lang in ["fr", *LANGS]:
            if csv_row.get(lang, "") != db_row.get(lang, ""):
                diffs += 1
                if diffs <= 20:
                    problems.append(
                        f"écart {key} [{lang}]: CSV={csv_row.get(lang, '')!r} "
                        f"DB={db_row.get(lang, '')!r}"
                    )
    return problems, diffs


# ---------------------------------------------------------------------------
# classify (decision 3 on #458, c.6405473398: cell-by-cell, three buckets)
# ---------------------------------------------------------------------------
# Direction rule, from the decision itself: "Registre en avance -> on injecte ;
# base en avance -> on rapatrie au registre par PR ; conflit -> aucune ecriture,
# liste vers ai-01." An empty cell carries no editorial signal, so the only
# machine-decidable "ahead" is against an empty opposite side; two filled,
# different values need a human - that is the conflit bucket, on purpose.
CLASS_ORDER = ["registre_avance", "base_avance", "conflit"]


def classify_diffs(
    csv_rows: list[dict], db_rows: list[dict]
) -> dict[str, list[dict]]:
    """Bucket every cell-level diff into the three decision-3 classes.

    Structural key mismatches (rows present on one side only) stay in
    `compare`'s problems: a cell cannot be classified where one side has no
    row at all, and those need ai-01 regardless.
    """
    csv_by_key = {row["key"]: row for row in csv_rows}
    db_by_key = {row["key"]: row for row in db_rows}
    buckets: dict[str, list[dict]] = {name: [] for name in CLASS_ORDER}
    for key in sorted(set(csv_by_key) & set(db_by_key)):
        csv_row, db_row = csv_by_key[key], db_by_key[key]
        for lang in ["fr", *LANGS]:
            csv_value = csv_row.get(lang, "")
            db_value = db_row.get(lang, "")
            if csv_value == db_value:
                continue
            cell = {
                "class": "",
                "key": key,
                "lang": lang,
                "csv_value": csv_value,
                "db_value": db_value,
                "app_id": csv_row.get("app_id", db_row.get("app_id", "")),
                "content_type": csv_row.get(
                    "content_type", db_row.get("content_type", "")
                ),
                "field": csv_row.get("field", db_row.get("field", "")),
            }
            if csv_value and not db_value:
                cell["class"] = "registre_avance"
            elif db_value and not csv_value:
                cell["class"] = "base_avance"
            else:
                cell["class"] = "conflit"
            buckets[cell["class"]].append(cell)
    return buckets


def cmd_verify(args: argparse.Namespace) -> int:
    conn = read_connection_string(pathlib.Path(args.config))
    db_rows = fetch_rows(conn)
    csv_rows = read_csv(pathlib.Path(args.csv))
    problems, diffs = compare(csv_rows, db_rows)
    print(f"CSV   : {len(csv_rows)} lignes")
    print(f"Base  : {len(db_rows)} lignes")
    print(f"Écarts: {diffs}")
    if problems:
        for problem in problems:
            print(f"  - {problem}")
    if getattr(args, "classify", False):
        buckets = classify_diffs(csv_rows, db_rows)
        for name in CLASS_ORDER:
            cells = buckets[name]
            print(f"  {name}: {len(cells)}")
            for cell in cells[:5]:
                print(
                    f"    - {cell['key']} [{cell['lang']}] "
                    f"csv={len(cell['csv_value'])}c db={len(cell['db_value'])}c"
                )
        if len(buckets) and len(buckets["conflit"]) > 5:
            print(f"    … +{len(buckets['conflit']) - 5} autres conflits")
        out = getattr(args, "out_classified", None)
        if out:
            out_path = pathlib.Path(out)
            out_path.parent.mkdir(parents=True, exist_ok=True)
            fields = [
                "class", "key", "lang", "app_id", "content_type", "field",
                "csv_value", "db_value",
            ]
            with out_path.open("w", encoding="utf-8", newline="") as handle:
                writer = csv.DictWriter(
                    handle, fieldnames=fields, lineterminator="\n"
                )
                writer.writeheader()
                for name in CLASS_ORDER:
                    for cell in buckets[name]:
                        writer.writerow(cell)
            total = sum(len(buckets[n]) for n in CLASS_ORDER)
            print(f"  classés -> {out_path} ({total} cellules)")
    if diffs or any(p.startswith(("clé", "type")) for p in problems):
        print("VERDICT: ÉCHEC")
        return 1
    print("VERDICT: réimport à blanc = 0 valeur changée")
    return 0


# ---------------------------------------------------------------------------
# selftest (inverse control)
# ---------------------------------------------------------------------------
def cmd_selftest(args: argparse.Namespace) -> int:
    conn = read_connection_string(pathlib.Path(args.config))
    db_rows = fetch_rows(conn)
    assert_export_safe(db_rows)
    assert_witnesses(db_rows)
    with tempfile.TemporaryDirectory(prefix="eav-selftest-") as tmp:
        clean = pathlib.Path(tmp) / "clean.csv"
        write_csv(db_rows, clean)

        csv_rows = read_csv(clean)
        problems, diffs = compare(csv_rows, db_rows)
        if diffs or problems:
            print(f"ÉCHEC étape 1: réexport non stable ({diffs} écarts)")
            for problem in problems[:10]:
                print("  -", problem)
            return 1
        print(f"PASS étape 1: export -> verify = 0 écart ({len(csv_rows)} lignes)")

        mutated = pathlib.Path(tmp) / "mutated.csv"
        with clean.open("r", encoding="utf-8", newline="") as src, mutated.open(
            "w", encoding="utf-8", newline=""
        ) as dst:
            reader = csv.DictReader(src)
            writer = csv.DictWriter(
                dst, fieldnames=reader.fieldnames, lineterminator="\n"
            )
            writer.writeheader()
            for index, row in enumerate(reader):
                if index == 0:
                    row["fr"] = row["fr"] + " [mutation]"
                writer.writerow(row)

        mutated_rows = read_csv(mutated)
        problems, diffs = compare(mutated_rows, db_rows)
        if diffs != 1:
            print(f"ÉCHEC étape 2: mutation d'une cellule -> {diffs} écart(s), attendu 1")
            for problem in problems[:10]:
                print("  -", problem)
            return 1
        print("PASS étape 2: mutation d'une cellule -> exactement 1 écart détecté")

        # Step 3 - classify inverse control: one mutation per decision-3 class,
        # on three distinct keys, must land one cell in each bucket and nowhere
        # else. A classifier that put everything in one bucket, or dropped a
        # class, would pass a count-only check - the per-key assertion closes
        # both ways it could lie.
        clean_rows = read_csv(clean)
        filled_fr = [r for r in clean_rows if r["fr"]]
        if len(filled_fr) < 2:
            print("ÉCHEC étape 3: moins de 2 lignes fr non vides pour muter")
            return 1
        conflit_row = filled_fr[0]
        base_row = next(r for r in filled_fr if r["key"] != conflit_row["key"])
        try:
            reg_row = next(
                r
                for r in clean_rows
                if r["key"] not in (conflit_row["key"], base_row["key"])
                and r["en"] == ""
            )
        except StopIteration:
            print("ÉCHEC étape 3: aucune ligne en vide pour la classe registre_avance")
            return 1

        classified = pathlib.Path(tmp) / "classified.csv"
        with clean.open("r", encoding="utf-8", newline="") as src, classified.open(
            "w", encoding="utf-8", newline=""
        ) as dst:
            reader = csv.DictReader(src)
            writer = csv.DictWriter(
                dst, fieldnames=reader.fieldnames, lineterminator="\n"
            )
            writer.writeheader()
            for row in reader:
                if row["key"] == conflit_row["key"]:
                    row["fr"] = row["fr"] + " [conflit]"
                elif row["key"] == base_row["key"]:
                    row["fr"] = ""
                elif row["key"] == reg_row["key"]:
                    row["en"] = "x-registre"
                writer.writerow(row)

        buckets = classify_diffs(read_csv(classified), db_rows)
        expected = {
            "registre_avance": reg_row["key"],
            "base_avance": base_row["key"],
            "conflit": conflit_row["key"],
        }
        for name in CLASS_ORDER:
            cells = buckets[name]
            if len(cells) != 1 or cells[0]["key"] != expected[name]:
                print(f"ÉCHEC étape 3: {name} -> {len(cells)} cellule(s) "
                      f"{[c['key'] for c in cells]}, attendu 1x {expected[name]}")
                return 1
        print("PASS étape 3: 3 mutations -> 1 cellule par classe, la bonne clé")
        print("VERDICT: instrument validé (il voit un 1, dans le bon seau)")
    return 0


# ---------------------------------------------------------------------------
# CLI
# ---------------------------------------------------------------------------
def cmd_export(args: argparse.Namespace) -> int:
    conn = read_connection_string(pathlib.Path(args.config))
    rows = fetch_rows(conn)
    assert_export_safe(rows)
    assert_witnesses(rows)
    out_path = pathlib.Path(args.out)
    write_csv(rows, out_path)
    filled = sum(1 for row in rows if row.get("fr"))
    print(f"{len(rows)} lignes écrites dans {out_path}")
    print(f"  dont {filled} avec une valeur de base non vide")
    print(f"  types: {len({(r['app_id'], r['content_type']) for r in rows})}")
    return 0


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        "--config",
        default=str(DEFAULT_CONFIG),
        help="chemin de web.config (défaut: DNNPlatform/web.config)",
    )
    sub = parser.add_subparsers(dest="command", required=True)

    export = sub.add_parser("export", help="écrit le CSV (lecture seule)")
    export.add_argument(
        "--out",
        default=str(REPO_ROOT / "docs" / "dnn-localization" / "eav-content.csv"),
    )
    export.set_defaults(func=cmd_export)

    verify = sub.add_parser("verify", help="compare un CSV à la base (à blanc)")
    verify.add_argument(
        "--csv",
        default=str(REPO_ROOT / "docs" / "dnn-localization" / "eav-content.csv"),
    )
    verify.add_argument(
        "--classify",
        action="store_true",
        help="bucket les écarts cellule par cellule (décision 3, #458)",
    )
    verify.add_argument(
        "--out-classified",
        default=None,
        help="écrit les cellules classées avec leurs valeurs (CSV)",
    )
    verify.set_defaults(func=cmd_verify, classify=False, out_classified=None)

    selftest = sub.add_parser("selftest", help="contrôle inverse sur une copie temporaire")
    selftest.set_defaults(func=cmd_selftest)

    args = parser.parse_args(argv)
    return args.func(args)


if __name__ == "__main__":
    sys.exit(main())
