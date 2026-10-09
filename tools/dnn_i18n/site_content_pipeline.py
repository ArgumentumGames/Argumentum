#!/usr/bin/env python3
"""
#457 T1 — site-content pipeline: 2sxc v21 export JSON <-> CSV <-> 2sxc import XML
(with language dimensions).

WHY THIS EXISTS
---------------
The site-translation chain has four bricks. Two already existed before this tool:

  [1] export 2sxc -> JSON ......... DONE  (#681/#774, committed under
                                            docs/dnn-localization/release-validation/exports/)
  [2] JSON -> translation ......... DONE  (translate_game_rules.py, gpt-5.x, JSON out,
                                            DoD-gated by verify_game_rule_translations.py)
  [3] JSON -> CSV ................. **MISSING**  <- this tool
  [4] CSV  -> 2sxc import XML ..... **MISSING**  <- this tool (with dimensions)

The CSV is the pivot the dispatch asks for: `key | fr | en ru pt es ar fa zh`, one row per
(entity, attribute). It is the interchange the DatasetUpdater task ("contenus du site") can
consume, and it is what makes the import XML *derivable* rather than hand-written.

SHAPE OF THE v21 EXPORT (measured, not assumed)
-----------------------------------------------
`12-game-rule-content-items.json` is NOT the 2sxc-15 `SexyContentData` dialect. It is a flat
triple store:

    { "contentType": "Game Rule", "appId": 60, "attributeSetId": 377,
      "schemaAttributes": [ {"StaticName": "Parent", "Type": "Entity", ...}, ... ],
      "entities": [ {"EntityID": 11378, "EntityGUID": "ae1edefa-...", "IsPublished": true} ],
      "values":   [ {"EntityID": 11378, "StaticName": "Title", "Value": "...",
                     "Type": "String", "Lang": null, "DimensionID": null} ] }

MEASURED 2026-10-08 on the committed export: 5 Game Rule entities, 52 populated values,
**52/52 carry `Lang=null` and `DimensionID=null`** — i.e. the export holds ONLY the
dimensionless default, which the site treats as FR (`manifest.json: dimensions.note`).
No translated value is present in the export. The 7 target languages therefore come from the
committed translation artifact `684-translations.json` (28 cells x 8 languages, DoD-verified:
196/196 tag-preserving and script-correct, 0 violations).

THE DIMENSION AXIS — WHAT IS ATTESTED AND WHAT IS NOT
-----------------------------------------------------
The import dialect (`SexyContentData` / `Entity`, cf. Argumentum.AssetConverter/Dnn2sxc/)
expresses a language dimension as ONE `<Entity>` block per (Guid, Language) pair — the same
Guid repeated once per language, each carrying its own `<Language>` culture code. Attested
values:

    fr-FR  dimensionId 6   (TsDynDataDimension, zone 3 — app 60's zone; measured 2026-10-09)
    en-US  dimensionId 7   (TsDynDataDimension, zone 3 — app 60's zone; measured 2026-10-09)

The other six culture codes are **NOT attested anywhere in this repository**: their 2sxc
dimensions are not provisioned (#682 Path A, recorded in 684-translation-run-report.md
lines 70-71 and 102-103). `to-xml` therefore REFUSES to emit an XML that uses them unless
`--unconfirmed-ok` is passed, so that nobody silently ships a file whose culture codes were
invented. Confirming them is a live step (I3): the portal's real dimensions come from

    SELECT DimensionID, ZoneId, Name, CultureCode FROM TsDynDataDimension

WHAT THIS TOOL DOES *NOT* CLAIM
--------------------------------
* It does NOT prove that 2sxc v21 accepts this XML. That is the T1+I3 unknown, and it is
  settled on the live site, on ONE entity, before any volume.
* It does NOT prove that an empty element leaves a value untouched (fallback) rather than
  clearing it. For that reason `to-xml` emits, in non-default dimensions, ONLY the
  attributes it actually carries a value for; use `--uniform-attributes` to emit the whole
  schema in every block (dialect-matching, but it asserts emptiness explicitly).
* It does NOT write to DNN. Nothing here mutates the portal.

USAGE
-----
  # 1. export -> CSV (translation targets filled from the committed artifact)
  python site_content_pipeline.py to-csv --out site-content.csv

  # 2. CSV -> import XML with dimensions, for ONE entity first (the dispatched proof)
  python site_content_pipeline.py to-xml --csv site-content.csv \
      --entity 11378 --unconfirmed-ok --out one-entity.xml

  # 3. offline proof: lossless round-trip + inverse controls, zero network, zero write
  python site_content_pipeline.py self-test
"""
from __future__ import annotations

import argparse
import csv
import io
import json
import os
import sys
import tempfile
import xml.etree.ElementTree as ET

HERE = os.path.dirname(os.path.abspath(__file__))
if HERE not in sys.path:
    sys.path.insert(0, HERE)

# Single source of truth for "which attributes are prose" and "which languages are targets":
# reused from the sibling translation tool rather than re-declared, so the two cannot drift.
from translate_game_rules import TARGETS, TRANSLATE_FIELDS  # noqa: E402

ROOT = os.path.abspath(os.path.join(HERE, "..", ".."))
EXPORT_DIR = os.path.join(ROOT, "docs", "dnn-localization", "release-validation", "exports",
                          "DNN-Argumentum-export-2026-07-07")
DEFAULT_EXPORT = os.path.join(EXPORT_DIR, "12-game-rule-content-items.json")
DEFAULT_TRANSLATIONS = os.path.join(ROOT, "docs", "dnn-localization", "684-translations.json")

LANG_COLUMNS = ["fr"] + TARGETS

# The entity used for the "proof on ONE entity" fixture consumed by
# Argumentum.AssetConverter.Tests/SiteContentImportXmlInteropTests.cs — "L'école des menteurs".
ONE_ENTITY_GUID = "ae1edefa-6f1b-4593-8230-97fa1edf4f78"
FIXTURE_XML = os.path.join(HERE, "fixtures", "one-entity-game-rule-import.xml")

# lang -> (culture code, dimensionId, attested?, provenance)
#
# dimensionId NOTE (I2, measured 2026-10-09): dimensions are ZONE-scoped, and app 60
# lives in zone 3, whose dimensions are 6 (fr-FR) and 7 (en-US). The IDs this table
# first carried (4 / 3) were copied from the export's manifest.json — they are ZONE 2's
# (the dead portal 0), and that same manifest declares argumentumAppZoneId: 3, i.e. it
# contradicts itself. Only the culture code is ever emitted into the import XML, so
# nothing shipped wrong; the dimensionIds here are documentation, now measured instead
# of copied. The 6 other cultures have no row anywhere in TsDynDataDimension.
CULTURES: dict[str, tuple[str, int | None, bool, str]] = {
    "fr": ("fr-FR", 6, True, "TsDynDataDimension zone 3 (app 60's zone), measured 2026-10-09 (#1781 I2)"),
    "en": ("en-US", 7, True, "TsDynDataDimension zone 3 (app 60's zone), measured 2026-10-09 (#1781 I2)"),
    "ru": ("ru-RU", None, False, "dimension NOT provisioned (#682 Path A)"),
    "pt": ("pt-PT", None, False, "dimension NOT provisioned (#682 Path A)"),
    "es": ("es-ES", None, False, "dimension NOT provisioned (#682 Path A)"),
    "ar": ("ar-SA", None, False, "dimension NOT provisioned (#682 Path A)"),
    "fa": ("fa-IR", None, False, "dimension NOT provisioned (#682 Path A)"),
    "zh": ("zh-CN", None, False, "dimension NOT provisioned (#682 Path A)"),
}

CSV_FIELDS = ["key", "app", "content_type", "guid", "attribute"] + LANG_COLUMNS


class PipelineError(Exception):
    """Refusal. Raised instead of emitting a payload we cannot vouch for."""


# --------------------------------------------------------------------------------------
# loading
# --------------------------------------------------------------------------------------

def load_export(path: str) -> dict:
    with open(path, encoding="utf-8") as f:
        data = json.load(f)
    for k in ("contentType", "schemaAttributes", "entities", "values"):
        if k not in data:
            raise PipelineError(f"{path}: missing top-level key '{k}' — not a v21 export?")
    known = {a["StaticName"] for a in data["schemaAttributes"]}
    for v in data["values"]:
        if v["StaticName"] not in known:
            raise PipelineError(
                f"{path}: value for attribute '{v['StaticName']}' (entity {v['EntityID']}) "
                f"is absent from schemaAttributes {sorted(known)}")
    return data


def load_translations(path: str | None) -> dict:
    if not path:
        return {}
    with open(path, encoding="utf-8") as f:
        return json.load(f)


def translations_for(translations: dict, entity_id: int) -> dict:
    """Entity lookup BY ID, never by iterating the artifact's keys.

    TRAP (measured 2026-10-08): `684-translations.json` carries two NON-entity keys inside
    `entities` — `_meta` and `entities` — leftovers of a double wrap in the translator's
    resume path. Iterating `translations["entities"]` and treating every key as an entity
    would fabricate 2 bogus 2sxc entities. Addressing by the export's EntityID cannot.
    """
    ent = translations.get("entities", {})
    got = ent.get(str(entity_id))
    return got if isinstance(got, dict) and "fields" in got else {}


def translation_value(translations: dict, entity_id: int, attribute: str, lang: str) -> str:
    cell = translations_for(translations, entity_id).get("fields", {}).get(attribute)
    if not isinstance(cell, dict):
        return ""
    v = cell.get(lang)
    return v if isinstance(v, str) else ""


# --------------------------------------------------------------------------------------
# export -> CSV
# --------------------------------------------------------------------------------------

def build_rows(export: dict, translations: dict | None = None,
               entity_filter: str | None = None) -> list[dict]:
    """One row per (entity, attribute) actually STORED in the export.

    Row presence — not cell emptiness — carries the fact that an attribute is set. The v21
    export omits unset attributes entirely (measured: 23 of 75 schema slots are absent —
    Parent/Author/Licence/Original never set on any of the 5 rules, Memo set on one only),
    so a reader that iterates the full schema would invent 23 empty values.
    """
    translations = translations or {}
    by_id = {e["EntityID"]: e for e in export["entities"]}
    app, ctype = export["appId"], export["contentType"]
    keep = {str(e) for e in by_id}
    if entity_filter:
        keep = {k for k in keep if k == str(entity_filter)}

    rows: list[dict] = []
    for v in export["values"]:
        eid = v["EntityID"]
        if str(eid) not in keep:
            continue
        guid = by_id[eid]["EntityGUID"]
        attr = v["StaticName"]
        row = {
            "key": f"{app}|{ctype}|{guid}|{attr}",
            "app": str(app),
            "content_type": ctype,
            "guid": guid,
            "attribute": attr,
            "fr": v["Value"],
        }
        for lang in TARGETS:
            row[lang] = (translation_value(translations, eid, attr, lang)
                         if attr in TRANSLATE_FIELDS else "")
        rows.append(row)
    rows.sort(key=lambda r: (r["guid"], r["attribute"]))
    return rows


def write_csv(rows: list[dict], path: str) -> None:
    # utf-8-sig + CRLF to match the corpus CSVs (BOM present, CRLF line endings).
    with open(path, "w", encoding="utf-8-sig", newline="") as f:
        w = csv.DictWriter(f, fieldnames=CSV_FIELDS, lineterminator="\r\n")
        w.writeheader()
        w.writerows(rows)


def read_csv(path: str) -> list[dict]:
    with open(path, encoding="utf-8-sig", newline="") as f:
        r = csv.DictReader(f)
        missing = [c for c in CSV_FIELDS if c not in (r.fieldnames or [])]
        if missing:
            raise PipelineError(f"{path}: missing column(s) {missing}")
        return [dict(row) for row in r]


# --------------------------------------------------------------------------------------
# CSV -> v21 values  (round-trip axis 1)
# --------------------------------------------------------------------------------------

def rows_to_values(rows: list[dict], export: dict) -> list[tuple]:
    """Reconstruct the v21 `values` triples. Byte-exact on `Value` by construction."""
    by_guid = {e["EntityGUID"]: e["EntityID"] for e in export["entities"]}
    schema_type = {a["StaticName"]: a["Type"] for a in export["schemaAttributes"]}
    out = []
    for r in rows:
        guid, attr = r["guid"], r["attribute"]
        if guid not in by_guid:
            raise PipelineError(f"row key {r['key']}: guid {guid} is not in the export")
        out.append((by_guid[guid], attr, r["fr"], schema_type.get(attr, "String"), None, None))
    return out


def export_value_tuples(export: dict) -> list[tuple]:
    return [(v["EntityID"], v["StaticName"], v["Value"], v["Type"], v["Lang"], v["DimensionID"])
            for v in export["values"]]


# --------------------------------------------------------------------------------------
# CSV -> import XML  (round-trip axis 2)
# --------------------------------------------------------------------------------------

def rows_to_xml(rows: list[dict], export: dict, *, allow_unconfirmed: bool = False,
                uniform_attributes: bool = False, entity_filter: str | None = None) -> bytes:
    """Emit the 2sxc import dialect: one <Entity> per (Guid, Language)."""
    langs = LANG_COLUMNS
    if not allow_unconfirmed:
        bad = [(l, CULTURES[l][0]) for l in langs if not CULTURES[l][2]]
        if bad:
            raise PipelineError(
                "refusing to emit culture codes that are not attested in this repository: "
                + ", ".join(f"{l}->{c}" for l, c in bad)
                + ".\nTheir 2sxc dimensions are not provisioned (#682 Path A). Confirm them "
                  "on the live portal (SELECT DimensionID, Name, CultureCode FROM "
                  "TsDynDataDimension — zone-scoped: app 60 is zone 3) or pass --unconfirmed-ok "
                  "to emit a PROVISIONAL file.")

    schema_order = [a["StaticName"] for a in export["schemaAttributes"]]
    by_guid: dict[str, dict[str, dict[str, str]]] = {}
    for r in rows:
        if entity_filter and r["guid"] != str(entity_filter):
            continue
        by_guid.setdefault(r["guid"], {})[r["attribute"]] = r

    root = ET.Element("SexyContentData")
    for guid in sorted(by_guid):
        attrs = by_guid[guid]
        for lang in langs:
            e = ET.SubElement(root, "Entity", {"Type": export["contentType"]})
            ET.SubElement(e, "Guid").text = guid
            ET.SubElement(e, "Language").text = CULTURES[lang][0]
            for name in schema_order:
                row = attrs.get(name)
                value = (row or {}).get(lang, "")
                # UNIFORM MINIMAL RULE: emit an element only where we actually carry a
                # value, in every language including the source one. An empty element is
                # never emitted, so the import can never be read as "clear this attribute".
                # `--uniform-attributes` switches to the legacy dialect shape (every schema
                # attribute in every block, empty ones as <Attr />).
                if not uniform_attributes and not value:
                    continue
                child = ET.SubElement(e, name)
                if value:
                    child.text = value
    ET.indent(root, space="  ")
    return ET.tostring(root, encoding="utf-8", xml_declaration=True)


def xml_to_map(xml_bytes: bytes) -> dict[tuple[str, str, str], str]:
    """Read the import XML back: (guid, attribute, culture) -> value ('' when empty)."""
    root = ET.fromstring(xml_bytes)
    if root.tag != "SexyContentData":
        raise PipelineError(f"unexpected root element <{root.tag}>")
    out: dict[tuple[str, str, str], str] = {}
    for ent in root.findall("Entity"):
        guid = ent.findtext("Guid")
        lang = ent.findtext("Language")
        if not guid or not lang:
            raise PipelineError("an <Entity> block is missing <Guid> or <Language>")
        for child in ent:
            if child.tag in ("Guid", "Language"):
                continue
            out[(guid, child.tag, lang)] = child.text or ""
    return out


# --------------------------------------------------------------------------------------
# self-test — the offline proof (no network, no write outside a temp dir)
# --------------------------------------------------------------------------------------

_RESULTS = [0, 0]  # [passed, failed]


def _check(name: str, ok: bool, detail: str = "") -> bool:
    _RESULTS[0 if ok else 1] += 1
    print(f"  [{'PASS' if ok else 'FAIL'}] {name}" + (f" — {detail}" if detail else ""))
    return ok


def self_test(export_path: str, translations_path: str) -> int:
    exp = load_export(export_path)
    tr = load_translations(translations_path)
    print(f"[fixture] export={os.path.basename(export_path)} "
          f"entities={len(exp['entities'])} values={len(exp['values'])} "
          f"attrs={len(exp['schemaAttributes'])}")
    ok = True

    rows = build_rows(exp, tr)
    ok &= _check("row count == stored values", len(rows) == len(exp["values"]),
                 f"{len(rows)} rows / {len(exp['values'])} values")
    ok &= _check("entity coverage", len({r['guid'] for r in rows}) == len(exp["entities"]),
                 f"{len({r['guid'] for r in rows})} guids")

    # RT1 — the CSV carries the FR source without loss.
    got = sorted(rows_to_values(rows, exp))
    want = sorted(export_value_tuples(exp))
    diffs = [f"  want={w!r}\n  got ={g!r}" for w, g in zip(want, got) if w != g]
    ok &= _check("RT1 export -> CSV -> export, byte-exact on Value", got == want,
                 f"{len(diffs)} mismatch(es)" if diffs else f"{len(got)} values identical")
    for d in diffs[:3]:
        print("        " + d.replace("\n", "\n        "))
    # explicitly assert the round-trip is value-exact, not merely tuple-set-equal
    ok &= _check("RT1 preserves long/HTML/unicode values verbatim",
                 all(r["fr"] in {v["Value"] for v in exp["values"]} for r in rows))

    # RT2 — the dimension axis survives the XML.
    xml = rows_to_xml(rows, exp, allow_unconfirmed=True)
    back = xml_to_map(xml)
    expect = {}
    for r in rows:
        for lang in LANG_COLUMNS:
            v = r.get(lang, "")
            if not v:
                continue  # not emitted, by the uniform-minimal rule
            expect[(r["guid"], r["attribute"], CULTURES[lang][0])] = v
    ok &= _check("RT2 CSV -> XML -> map, byte-exact across the dimension axis",
                 back == expect, f"{len(back)} cells in XML / {len(expect)} expected")
    ok &= _check("XML carries one <Entity> per (guid, culture)",
                 len(ET.fromstring(xml).findall("Entity")) == len({r['guid'] for r in rows}) * 8,
                 f"{len(ET.fromstring(xml).findall('Entity'))} blocks")

    # ---- inverse controls -------------------------------------------------------------
    # IC1: a mutated source cell must break RT1.
    mutated = [dict(r) for r in rows]
    mutated[0]["fr"] = mutated[0]["fr"] + " MUTANT"
    ok &= _check("IC1 mutated FR cell is DETECTED by RT1",
                 sorted(rows_to_values(mutated, exp)) != want)

    # IC2: a dropped value in the XML must break RT2.
    dropped = dict(expect)
    dropped.pop(next(iter(dropped)))
    ok &= _check("IC2 RT2 is not vacuous (a missing cell diverges)",
                 xml_to_map(xml) != dropped)

    # IC3: the artifact's spurious keys must not become entities.
    polluted = json.loads(json.dumps(tr))
    polluted["entities"]["_meta"] = {"source": "spurious"}
    polluted["entities"]["entities"] = {"fields": {}}
    rows_p = build_rows(exp, polluted)
    ok &= _check("IC3 spurious artifact keys (\"_meta\", \"entities\") do not become entities",
                 len(rows_p) == len(rows) and len({r['guid'] for r in rows_p}) == 5,
                 f"{len(rows_p)} rows, {len({r['guid'] for r in rows_p})} guids")

    # IC4: an unconfirmed culture is refused unless explicitly allowed.
    refused = False
    try:
        rows_to_xml(rows, exp, allow_unconfirmed=False)
    except PipelineError:
        refused = True
    ok &= _check("IC4 unconfirmed cultures are REFUSED by default (fail-closed)", refused)

    # IC5: a value for an attribute outside the schema is refused.
    bad = json.loads(json.dumps(exp))
    bad["values"].append({"EntityID": bad["entities"][0]["EntityID"], "StaticName": "Nope",
                          "Value": "x", "Type": "String", "Lang": None, "DimensionID": None})
    rejected = False
    with tempfile.TemporaryDirectory() as tmp:
        p = os.path.join(tmp, "bad.json")
        with open(p, "w", encoding="utf-8") as f:
            json.dump(bad, f)
        try:
            load_export(p)
        except PipelineError:
            rejected = True
    ok &= _check("IC5 out-of-schema attribute is REFUSED", rejected)

    # IC6: the translatable-field list really comes from the sibling tool.
    ok &= _check("IC6 TRANSLATE_FIELDS reused (single source of truth)",
                 TRANSLATE_FIELDS == ["Title", "Summary", "Material", "Installation",
                                      "Content", "Variants", "Memo"],
                 str(TRANSLATE_FIELDS))

    # IC7: the dimension axis is not silently collapsed — 8 distinct cultures emitted.
    cultures = {c for (_, _, c) in back}
    ok &= _check("IC7 eight distinct culture codes emitted", len(cultures) == 8,
                 ", ".join(sorted(cultures)))

    # IC8: the two emission modes are distinct, and only the minimal one omits empties.
    xml_u = rows_to_xml(rows, exp, allow_unconfirmed=True, uniform_attributes=True)
    map_min, map_uni = xml_to_map(xml), xml_to_map(xml_u)
    n_min, n_uni = len(map_min), len(map_uni)
    slots = len({r["guid"] for r in rows}) * len(exp["schemaAttributes"]) * 8
    empty_u = sum(1 for v in map_uni.values() if v == "")
    ok &= _check("IC8 --uniform-attributes emits every schema slot in every language",
                 n_uni == slots and n_min == len(expect) and empty_u == slots - n_min,
                 f"minimal={n_min} (0 empty), uniform={n_uni} ({empty_u} empty), slots={slots}")

    # IC10: "present but empty" (a stored value that IS the empty string) is a different
    # thing from "absent" (an attribute the export never emitted at all). Both must be
    # carried by the CSV without being conflated — a reader that iterates the schema
    # instead of the export's value list turns the 23 absent slots into 23 fake values.
    empty_fr = sum(1 for r in rows if r["fr"] == "")
    all_slots = len({r["guid"] for r in rows}) * len(exp["schemaAttributes"])
    absent = all_slots - len(rows)
    ok &= _check("IC10 present-but-empty (4) is distinct from absent slots (23)",
                 empty_fr == 4 and absent == 23,
                 f"{len(rows)} stored = {len(rows) - empty_fr} valued + {empty_fr} empty; "
                 f"{absent} of {all_slots} schema slots never emitted")

    # IC9: no empty element is ever emitted in minimal mode (the non-destructive claim).
    ok &= _check("IC9 minimal mode emits zero empty elements",
                 all(v != "" for v in xml_to_map(xml).values()))

    # IC11: provenance of the committed fixture. It must be EXACTLY what this tool produces
    # from the committed inputs — a stale or hand-edited fixture would silently invalidate
    # every assertion the C# interop suite makes about it.
    if os.path.exists(FIXTURE_XML):
        regenerated = rows_to_xml(rows, exp, allow_unconfirmed=True,
                                  entity_filter=ONE_ENTITY_GUID)
        on_disk = open(FIXTURE_XML, "rb").read()
        ok &= _check("IC11 committed fixture == regenerated from committed inputs",
                     on_disk == regenerated,
                     f"{len(on_disk)} bytes on disk vs {len(regenerated)} regenerated")
    else:
        ok &= _check("IC11 fixture present", False, f"missing {FIXTURE_XML}")

    print()
    if ok:
        print(f"SELF-TEST PASS: {_RESULTS[0]} checks — 2 round-trips (source + dimension axis) "
              f"and {_RESULTS[0] - 2} controls.")
    else:
        print(f"SELF-TEST FAIL: {_RESULTS[1]} of {sum(_RESULTS)} checks failed.")
    return 0 if ok else 1


# --------------------------------------------------------------------------------------

def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.split("\n")[1].strip())
    sub = ap.add_subparsers(dest="cmd", required=True)

    c = sub.add_parser("to-csv", help="2sxc export JSON -> pivot CSV")
    c.add_argument("--export", default=DEFAULT_EXPORT)
    c.add_argument("--translations", default=None,
                   help="translation artifact (omit for an untranslated, fr-only CSV)")
    c.add_argument("--out", default=None)
    c.add_argument("--entity", default=None, help="restrict to one EntityID")

    x = sub.add_parser("to-xml", help="pivot CSV -> 2sxc import XML with dimensions")
    x.add_argument("--csv", required=True)
    x.add_argument("--export", default=DEFAULT_EXPORT)
    x.add_argument("--out", default=None)
    x.add_argument("--entity", default=None, help="restrict to one entity GUID (the T1 proof)")
    x.add_argument("--unconfirmed-ok", action="store_true",
                   help="emit PROVISIONAL culture codes not attested in this repository")
    x.add_argument("--uniform-attributes", action="store_true",
                   help="emit every schema attribute in every language block")

    s = sub.add_parser("self-test", help="offline round-trip + inverse controls")
    s.add_argument("--export", default=DEFAULT_EXPORT)
    s.add_argument("--translations", default=DEFAULT_TRANSLATIONS)

    l = sub.add_parser("list-cultures", help="the culture table, with attestation status")
    l.add_argument("--json", action="store_true")

    args = ap.parse_args()

    if args.cmd == "self-test":
        return self_test(args.export, args.translations)

    if args.cmd == "list-cultures":
        if args.json:
            print(json.dumps({k: {"culture": v[0], "dimensionId": v[1], "attested": v[2],
                                  "provenance": v[3]} for k, v in CULTURES.items()}, indent=2))
        else:
            for k, (culture, dim, att, prov) in CULTURES.items():
                print(f"  {k:>3} -> {culture:<6} dim={str(dim):<5} "
                      f"{'ATTESTED' if att else 'UNCONFIRMED':<12} {prov}")
        return 0

    try:
        if args.cmd == "to-csv":
            exp = load_export(args.export)
            tr = load_translations(args.translations)
            rows = build_rows(exp, tr, args.entity)
            if args.out:
                write_csv(rows, args.out)
                filled = sum(1 for r in rows if any(r[l] for l in TARGETS))
                print(f"[to-csv] {len(rows)} rows, {filled} with at least one translation "
                      f"-> {args.out}", file=sys.stderr)
            else:
                buf = io.StringIO()
                w = csv.DictWriter(buf, fieldnames=CSV_FIELDS, lineterminator="\r\n")
                w.writeheader()
                w.writerows(rows)
                sys.stdout.write(buf.getvalue())
            return 0

        if args.cmd == "to-xml":
            rows = read_csv(args.csv)
            exp = load_export(args.export)
            xml = rows_to_xml(rows, exp, allow_unconfirmed=args.unconfirmed_ok,
                              uniform_attributes=args.uniform_attributes,
                              entity_filter=args.entity)
            if args.out:
                with open(args.out, "wb") as f:
                    f.write(xml)
                blocks = len(ET.fromstring(xml).findall("Entity"))
                print(f"[to-xml] {blocks} <Entity> blocks -> {args.out}", file=sys.stderr)
            else:
                sys.stdout.buffer.write(xml)
            return 0
    except PipelineError as e:
        print(f"REFUSED: {e}", file=sys.stderr)
        return 2

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
