#!/usr/bin/env python3
"""#682 — compute the Rule content-type field delta for the owner window, and verify it after.

READ-ONLY. This never opens a database and never writes a schema. It reads an inventory in the
committed export format (docs/dnn-localization/release-validation/exports/.../11-game-rule-schema.json)
and reports which of the 49 prepared lang-suffixed attributes the measured schema is missing.

The same instrument runs BEFORE the window (expect 49 additions) and AFTER it (expect 0 missing),
so the window's exit criterion is measured rather than asserted. The owner decision of 2026-09-14
(#682 c.5665864947) fixes the field model: the real `Title` attribute, suffixed `Title_<lang>` --
never a second `EntityTitle`.

Deliberately NOT predicted: `AttributeID`. The database assigns it. Inventing IDs would make the
plan look precise while being fiction, and a fabricated ID is worse than an absent one because it
reads as measured.

Usage
  tools/dnn-682-schema-delta.py [SCHEMA.json] [--expect N] [--quiet]
  tools/dnn-682-schema-delta.py --self-test

Exit: 0 expectations met · 1 not met · 2 environment problem.
"""
import argparse
import json
import os
import sys

# The 7 prose fields of the `Game Rule` content-type (setID 377). The other 8 measured attributes
# are not localizable prose: Parent/Author/Licence/Original (Entity), MinNbPlayers/MaxNbPlayers
# (Number), Date (DateTime), UrlKey (slug).
FIELDS = ["Title", "Summary", "Material", "Installation", "Content", "Variants", "Memo"]

# `fr` is the canonical, unsuffixed value already in the DB. The 7 targets are the release set.
LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]

FIRST_SORT_ORDER = 15  # the measured schema occupies 0..14

DEFAULT_SCHEMA = (
    "docs/dnn-localization/release-validation/exports/"
    "DNN-Argumentum-export-2026-07-07/11-game-rule-schema.json"
)

# The inferred type of the 49 additions. All 7 source fields are String in the measured schema, so
# the suffix inherits String -- high confidence, but an INFERENCE, not a measurement (dossier §7).
INFERRED_TYPE = "String"


def expected_names():
    """The 49 suffixed StaticNames, in the SortOrder they are planned to receive."""
    return [(f"{field}_{lang}", field, lang) for field in FIELDS for lang in LANGS]


def load_schema(path):
    with open(path, encoding="utf-8") as fh:
        return json.load(fh)


def analyse(schema):
    """Return the measured facts. No writes, no inference beyond what is labelled as such."""
    attrs = schema.get("attributes", [])
    names = {a["StaticName"] for a in attrs}
    plan = expected_names()

    missing = [(n, field, lang, FIRST_SORT_ORDER + i)
               for i, (n, field, lang) in enumerate(plan) if n not in names]
    present = [n for n, _f, _l in plan if n in names]

    # Guard 1 -- option (B) was discarded by the owner: a second title attribute must never appear.
    forbidden = sorted(n for n in names if n.startswith("EntityTitle"))

    # Guard 2 -- scope: no suffixed attribute outside the 7 fields (a suffix typo'd onto another
    # attribute would be provisioned, translated and then never read).
    suffixed = sorted(n for n in names
                      if "_" in n and n.rsplit("_", 1)[1] in LANGS
                      and n.rsplit("_", 1)[0] not in FIELDS)

    # Guard 3 -- exactly one title attribute, and it is `Title`.
    titles = sorted(a["StaticName"] for a in attrs if a.get("IsTitle"))

    return {
        "count": len(attrs),
        "missing": missing,
        "present": present,
        "forbidden": forbidden,
        "offscope": suffixed,
        "titles": titles,
    }


def report(schema, res, quiet=False):
    print(f"content-type : {schema.get('contentType')}  setID {schema.get('attributeSetId')}  "
          f"app {schema.get('appId')}")
    print(f"measured     : {res['count']} attributs")
    print(f"title attrs  : {res['titles']}")
    print(f"plan         : {len(res['present'])}/{len(expected_names())} des 49 deja presents, "
          f"{len(res['missing'])} a provisionner")
    if res["missing"] and not quiet:
        print(f"\n{'SortOrder':>9}  {'StaticName':<20} {'base':<14} Type")
        for name, field, lang, order in res["missing"]:
            print(f"{order:>9}  {name:<20} {field:<14} {INFERRED_TYPE}  (lang={lang})")
    if res["present"] and not quiet:
        print(f"\ndeja presents : {', '.join(sorted(res['present']))}")
    return res


def guards(res):
    ok = True
    if res["forbidden"]:
        print(f"GUARD FAIL: attribut(s) EntityTitle* detecte(s) : {res['forbidden']}")
        print("            option (B) ecartee par la decision owner du 14/09 -- a retirer.")
        ok = False
    if res["offscope"]:
        print(f"GUARD FAIL: attribut(s) suffixe(s) hors des 7 champs : {res['offscope']}")
        ok = False
    if res["titles"] != ["Title"]:
        print(f"GUARD FAIL: attribut de titre attendu ['Title'], mesure {res['titles']}")
        ok = False
    return ok


def self_test():
    """Mutation controls on a synthetic schema -- no fixture on disk, no database."""
    print("=== self-test (controles sur schema synthetique) ===")
    base = [{"SortOrder": i, "AttributeID": 1932 + i, "StaticName": n, "Group": "Default",
             "IsTitle": n == "Title", "Type": "String"}
            for i, n in enumerate(["Parent", "Title", "Summary", "Material", "MinNbPlayers",
                                   "MaxNbPlayers", "Installation", "Content", "Variants", "Memo",
                                   "Date", "Author", "Licence", "Original", "UrlKey"])]
    shell = {"contentType": "Game Rule", "attributeSetId": 377, "appId": 60}

    failures = 0

    def check(label, cond):
        nonlocal failures
        print(f"  {'OK  ' if cond else 'FAIL'} {label}")
        if not cond:
            failures += 1

    # control 0 -- the pre-window state must report all 49
    res = analyse({**shell, "attributes": base})
    check("pre-fenetre : 15 attributs -> 49 manquants", len(res["missing"]) == 49)

    # control 1 -- post-window state must report none
    full = base + [{"SortOrder": FIRST_SORT_ORDER + i, "StaticName": n, "Group": "Default",
                    "IsTitle": False, "Type": INFERRED_TYPE}
                   for i, (n, _f, _l) in enumerate(expected_names())]
    res = analyse({**shell, "attributes": full})
    check("post-fenetre : 64 attributs -> 0 manquant", len(res["missing"]) == 0)
    check("post-fenetre : guards verts", guards(res) and True)

    # control 2 -- a single dropped field must be caught (the instrument must discriminate)
    res = analyse({**shell, "attributes": [a for a in full if a["StaticName"] != "Memo_zh"]})
    check("mutation : retirer Memo_zh -> 1 manquant", len(res["missing"]) == 1)

    # control 3 -- the discarded option (B) must be caught
    res = analyse({**shell, "attributes": full + [
        {"SortOrder": 64, "StaticName": "EntityTitle_en", "Group": "Default",
         "IsTitle": False, "Type": "String"}]})
    check("mutation : EntityTitle_en injecte -> guard rouge", bool(res["forbidden"]))

    # control 4 -- an off-scope suffix must be caught
    res = analyse({**shell, "attributes": full + [
        {"SortOrder": 65, "StaticName": "UrlKey_en", "Group": "Default",
         "IsTitle": False, "Type": "String"}]})
    check("mutation : UrlKey_en (hors 7 champs) -> guard rouge", bool(res["offscope"]))

    # control 5 -- a second title attribute must be caught
    res = analyse({**shell, "attributes": full + [
        {"SortOrder": 66, "StaticName": "TitleBis", "Group": "Default",
         "IsTitle": True, "Type": "String"}]})
    check("mutation : second IsTitle -> guard rouge", res["titles"] != ["Title"])

    print(f"\nself-test : {'VERT' if failures == 0 else f'ROUGE ({failures})'}")
    return 1 if failures else 0


def main():
    ap = argparse.ArgumentParser(add_help=True)
    ap.add_argument("schema", nargs="?", default=DEFAULT_SCHEMA)
    ap.add_argument("--expect", type=int, default=None,
                    help="nombre de manquants attendu (avant fenetre : 49 · apres : 0)")
    ap.add_argument("--quiet", action="store_true", help="compteurs seulement, sans le plan")
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()

    if args.self_test:
        return self_test()

    if not os.path.exists(args.schema):
        print(f"FATAL: schema introuvable : {args.schema}", file=sys.stderr)
        print("       (executer depuis la racine du depot)", file=sys.stderr)
        return 2

    schema = load_schema(args.schema)
    res = report(schema, analyse(schema), quiet=args.quiet)
    ok = guards(res)

    if args.expect is not None:
        met = len(res["missing"]) == args.expect
        print(f"\nattendu : {args.expect} manquant(s) · mesure : {len(res['missing'])} "
              f"-> {'CONFORME' if met else 'ECART'}")
        ok = ok and met

    return 0 if ok else 1


if __name__ == "__main__":
    sys.exit(main())
