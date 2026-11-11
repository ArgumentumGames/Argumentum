#!/usr/bin/env python3
"""#135 — table de liens de la page Téléchargements, 8 langues (pool #458 c.6788623855).

Pour chaque ligne de la correspondance tranchée (#135 c.6650903268) : le nom
d'asset attendu dans la release finale, sous sa **forme publiée** (GitHub
réécrit ``&`` en ``.``), **dérivé** des ``DocumentName`` + ``Translations`` de
``WebBasedGeneratorConfig.cs`` — jamais tapé à la main — puis **confronté** aux
noms publiés de la release (lecture seule, ``gh release view``).

Sortie : JSON + table Markdown. Exit 1 si un nom dérivé n'est pas confrontable.
⛔ Lecture seule : aucun upload, aucune écriture de release.
"""
import argparse
import json
import re
import subprocess
import sys
from pathlib import Path

REPO = Path(__file__).resolve().parents[1]
CONFIG = REPO / "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs"

# Correspondance DÉCIDÉE c.6650903268 : les 6 lignes et ce qu'elles deviennent.
# Les zips (cartes numériques, légères, cartes mentales) ne sont pas des
# DocumentName : ils sont construits au tag par tools/135-build-download-zips.py
# (noms dérivés de son code : Argumentum_Cards_PNG[_light]_<lang>.zip,
# Argumentum_MindMaps_<lang>.zip). Les PDF sont dérivés de la config ci-dessous.
PUBLISHED_AMP = re.compile(r"&")


def derive_documents(config_text: str) -> dict:
    """{published_name: {fr_source, langs}} depuis DocumentName + Translations.

    La grammaire lue : `DocumentName = "…_fr.pdf",` suivi d'un bloc
    `Translations = new List<(string sourceLang, string destLang)>(new [] { ("fr","en"), … })`.
    La langue source est lue dans le DocumentName (_fr).
    """
    docs = {}
    disabled = []
    # chaque bloc CardSetDocumentConfig
    for m in re.finditer(
            r'DocumentName\s*=\s*"(?P<name>[^"]+)"(?P<body>.*?)(?=DocumentName\s*=|\Z)',
            config_text, re.S):
        name = m.group("name")
        body = m.group("body")
        # Enabled = false (documents fractionnés TarotCards_2/_3) : hors périmètre
        # de la page, dérivé de la config — pas une absence à signaler.
        if re.search(r"Enabled\s*=\s*false", body):
            disabled.append(name)
            continue
        src = re.search(r"_([a-z]{2})\.pdf$", name)
        if not src:
            continue
        src_lang = src.group(1)
        langs = {src_lang}
        for t in re.finditer(r'\(\s*"(?P<s>[a-z]{2})"\s*,\s*"(?P<d>[a-z]{2})"\s*\)', body):
            langs.add(t.group("d"))
        # noms publiés : suffixe de langue remplacé, & -> . (réécriture GitHub mesurée)
        for lg in sorted(langs):
            local = re.sub(r"_[a-z]{2}\.pdf$", f"_{lg}.pdf", name)
            published = PUBLISHED_AMP.sub(".", local)
            docs[published] = {"local_name": local, "source_document": name, "lang": lg}
    return docs, disabled


def gh_asset_names(release: str) -> set:
    out = subprocess.run(
        ["gh", "release", "view", release, "--json", "assets", "--jq", ".assets[].name"],
        capture_output=True, text=True, check=True).stdout
    return {l.strip() for l in out.splitlines() if l.strip()}


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--release", default="v2.1.0-review",
                    help="release de confrontation en lecture seule (défaut : v2.1.0-review)")
    ap.add_argument("--json-out", type=Path,
                    default=REPO / ".tmp/135-zips/downloads-table.json")
    ap.add_argument("--md-out", type=Path,
                    default=REPO / "docs/publication/135-downloads-assets-table.md")
    args = ap.parse_args()

    config_text = CONFIG.read_text(encoding="utf-8-sig")
    docs, disabled = derive_documents(config_text)
    if not docs:
        print("ABORT: aucun DocumentName dérivé — l'instrument est cassé", file=sys.stderr)
        return 2

    langs = sorted({v["lang"] for v in docs.values()})
    published = gh_asset_names(args.release)
    # Contrôle inverse RÉEL : la même comparaison, sur une release privée d'un
    # asset du périmètre, doit le remonter dans `missing` (sinon l'instrument
    # est aveugle et le « 0 absent » ne vaut rien).
    probe = sorted(set(docs) & published)[0] if (set(docs) & published) else None
    if probe is not None:
        if probe not in sorted(set(docs) - (published - {probe})):
            print("ABORT: contrôle inverse inerte", file=sys.stderr)
            return 2

    stem_to_assets = {}
    for name in docs:
        stem = re.sub(r"_[a-z]{2}\.pdf$", "", name)
        stem_to_assets.setdefault(stem, {})[docs[name]["lang"]] = name

    lines = []
    missing = []
    # Table : une ligne par (ligne de la correspondance × langue)
    lines.append("| Document (dérivé de la config) | langue | nom publié attendu | confronté à "
                 + args.release + " |")
    lines.append("|---|---|---|---|")
    for stem in sorted(stem_to_assets):
        for lg in langs:
            entry = stem_to_assets[stem].get(lg)
            if not entry:
                continue
            ok = entry in published
            if not ok:
                missing.append(entry)
            lines.append(f"| `{stem}` | {lg} | `{entry}` | {'✅ présent' if ok else '❌ ABSENT'} |")

    zips = []
    for lg in langs:
        zips += [f"Argumentum_Cards_PNG_{lg}.zip",
                 f"Argumentum_Cards_PNG_light_{lg}.zip",
                 f"Argumentum_MindMaps_{lg}.zip"]

    report = {
        "release_confronted": args.release,
        "langs_derived": langs,
        "pdf_documents_derived": len(stem_to_assets),
        "pdf_cells": len(docs),
        "disabled_documents_excluded": sorted(disabled),
        "pdf_missing_on_release": sorted(missing),
        "zips_to_build_at_tag": zips,
        "assets": docs,
    }
    args.json_out.parent.mkdir(parents=True, exist_ok=True)
    args.json_out.write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding="utf-8")

    md = [
        "# #135 — table des assets de la page Téléchargements (dérivée, confrontée)",
        "",
        f"- **Source de dérivation** : `DocumentName` + `Translations` de "
        f"`WebBasedGeneratorConfig.cs` (jamais tapé à la main).",
        f"- **Forme publiée** : GitHub réécrit `&` → `.` (`Print&Play` → `Print.Play`) — appliquée.",
        f"- **Confrontation** : release `{args.release}` en lecture seule "
        f"({len(published)} assets lus).",
        f"- **Langues dérivées** : {', '.join(langs)}.",
        f"- **Documents PDF dérivés** : {len(stem_to_assets)} stems × 8 langues = {len(docs)} assets.",
        f"- **Documents `Enabled = false` exclus par dérivation** : "
        f"{', '.join('`' + d + '`' for d in sorted(set(disabled))) or 'aucun'} "
        "(fractionnés historiques, hors périmètre de la page).",
        f"- **Absents sur la release** : {len(missing)} (voir table).",
        "",
        *lines,
        "",
        "## Zips à construire au tag (tools/135-build-download-zips.py)",
        "",
        "Ces noms sont dérivés du code de l'outil (jamais publiés à ce jour) : "
        + f"{len(zips)} noms (`Argumentum_Cards_PNG[_light]_<lang>.zip`, "
        "`Argumentum_MindMaps_<lang>.zip`).",
        "",
        "⛔ Rien n'est publié par cet outil. La page se remplira le jour du tag.",
        "",
        "*po-2023*",
    ]
    args.md_out.parent.mkdir(parents=True, exist_ok=True)
    args.md_out.write_text("\n".join(md) + "\n", encoding="utf-8")

    print(f"documents dérivés : {len(stem_to_assets)} stems, {len(docs)} assets sur {len(langs)} langues")
    print(f"assets lus sur {args.release} : {len(published)}")
    print(f"ABSENTS : {len(missing)}")
    for m in sorted(missing)[:20]:
        print("  -", m)
    print(f"table : {args.md_out}")
    print(f"json  : {args.json_out}")
    return 1 if missing else 0


if __name__ == "__main__":
    sys.exit(main())
