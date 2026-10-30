#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""#458 9-bis — construit List_Country.{ar-SA,fa-IR,zh-CN}.resx depuis le neutre + CLDR.

Methode identique aux packs AGR precedents : le .resx n'est PAS recree. On part du
fichier NEUTRE (coquille : 256 cles ISO alpha-2, valeurs toutes vides) et on ne
remplace que le texte des <value>. En-tete, resheader, ordre des cles, BOM UTF-8
et fins de ligne LF sont preserves octet par octet.

Source des valeurs : 458-item9bis-list-country-cldr-47.json (CLDR release-47,
254/256 par locale + AN/CS codes abroges declares [K], meme convention que le
precedent ru-RU #1907).

Idempotent : rejouer sur le meme neutre + JSON produit des octets identiques.
"""
import io
import json
import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

REPO = Path(__file__).resolve().parents[2]
if "--repo" in sys.argv:  # hors de son emplacement commite (scratchpad)
    REPO = Path(sys.argv[sys.argv.index("--repo") + 1])
AGR = REPO / "DNNPlatform" / "App_GlobalResources"
JSON_PATH = Path(__file__).resolve().parent / "458-item9bis-list-country-cldr-47.json"
CULTURES = ("ar-SA", "fa-IR", "zh-CN")

# <data name="XX.Text">\n    <value></value>  — bloc vide du neutre, compte = 1 par cle
EMPTY_BLOCK = '<data name="{name}">\n    <value></value>\n  </data>'


def xml_escape(s: str) -> str:
    return s.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")


def build(culture: str, table: dict, verify_only: bool) -> None:
    neutral_path = AGR / "List_Country.resx"
    raw = neutral_path.read_bytes()
    assert raw.startswith(b"\xef\xbb\xbf"), "le neutre doit porter un BOM UTF-8"
    text = raw.decode("utf-8-sig")
    assert "\r\n" not in text, "le neutre doit etre LF seul"

    neutral_keys = [d.get("name") for d in ET.parse(str(neutral_path)).getroot().findall("data")]
    assert len(neutral_keys) == 256, len(neutral_keys)

    out = text
    for key in neutral_keys:
        block = EMPTY_BLOCK.format(name=key)
        assert text.count(block) == 1, "bloc vide introuvable ou non unique: %s" % key
        filled = ('<data name="%s">\n    <value>%s</value>\n  </data>'
                  % (key, xml_escape(table[key])))
        out = out.replace(block, filled, 1)

    payload = b"\xef\xbb\xbf" + out.encode("utf-8")

    # controle independant : re-parse du XML produit, egalite cle par cle avec le JSON
    root = ET.fromstring(out)
    got = [(d.get("name"), (d.find("value").text or "")) for d in root.findall("data")]
    assert [k for k, _ in got] == neutral_keys, "l'ordre des cles a change"
    for k, v in got:
        assert v == table[k], "valeur re-parse != JSON: %s" % k
        assert v.strip(), "valeur vide: %s" % k

    dest = AGR / ("List_Country.%s.resx" % culture)
    if verify_only:
        current = dest.read_bytes()
        status = "OK byte-identical" if current == payload else "MISMATCH"
        print("%s verify: %s (%d bytes)" % (culture, status, len(payload)))
        if current != payload:
            sys.exit(1)
    else:
        dest.write_bytes(payload)
        print("%s: ecrit %d octets (256 cles)" % (dest.name, len(payload)))


def main() -> None:
    verify_only = "--verify" in sys.argv
    doc = json.loads(io.open(JSON_PATH, encoding="utf-8").read())
    for culture in CULTURES:
        table = doc[culture]
        assert len(table) == 256, (culture, len(table))
        build(culture, table, verify_only)


if __name__ == "__main__":
    main()
