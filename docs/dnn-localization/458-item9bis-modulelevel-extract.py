#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Mesure de la famille MODULE-LEVEL de DNN (App_LocalResources) : combien de
fichiers, combien de cles, et ce qui manque pour ar-SA / fa-IR / zh-CN.

LECTURE SEULE : n'ecrit rien, ne touche aucun .resx.

Usage :
    python 458-item9bis-modulelevel-extract.py [--root <racine du depot>] [--json <sortie>]
"""
from __future__ import annotations

import argparse
import json
import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

LOCALE_RE = re.compile(r"^(?P<base>.+)\.(?P<loc>[A-Za-z]{2,3}(-[A-Za-z]{2,4})?)\.resx$")

TARGETS = ("ar-SA", "fa-IR", "zh-CN")
REFERENCES = ("ru-RU", "pt-BR", "es-ES", "fr-FR")
# le "coeur" DNN : les familles que dnnsoftware livre dans les 4 langues de reference
CORE_LOCALES = ("ru-RU", "pt-BR", "es-ES", "fr-FR")
PLACEHOLDER = re.compile(r"\{\d+\}")


def neutral_key(rel: str) -> str | None:
    """Renvoie la cle de famille (base sans culture) si le nom porte une culture."""
    m = LOCALE_RE.match(rel)
    return m.group("base") if m else None


def classify(rel: str) -> str:
    parts = rel.replace("/", "\\").split("\\")
    if parts and parts[0] == "App_GlobalResources":
        return "agr-core"
    if "App_LocalResources" in parts:
        return "module-level"
    return "autre"


def read_keys(path: Path) -> tuple[int, int, int, list[str]]:
    """(nombre de <data>, non vides, cles avec placeholder, liste des cles)."""
    try:
        root = ET.parse(path).getroot()
    except ET.ParseError as exc:
        raise SystemExit(f"resx illisible : {path} ({exc})")
    data = root.findall("data")
    non_empty = 0
    with_placeholder = 0
    names: list[str] = []
    for d in data:
        v = d.find("value")
        text = (v.text or "") if v is not None else ""
        if text.strip():
            non_empty += 1
        if PLACEHOLDER.search(text):
            with_placeholder += 1
        names.append(d.get("name") or "")
    return len(data), non_empty, with_placeholder, names


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--root", default=".", help="racine du depot (contient DNNPlatform/)")
    ap.add_argument("--json", default="", help="ecrire le rapport JSON dans ce fichier")
    args = ap.parse_args()

    root = Path(args.root).resolve()
    dnn = root / "DNNPlatform"
    if not dnn.is_dir():
        raise SystemExit(f"DNNPlatform introuvable sous {root}")

    families: dict[str, dict] = {}
    for path in dnn.rglob("*.resx"):
        rel = path.relative_to(dnn).as_posix()
        m = LOCALE_RE.match(rel)
        if m:
            base_rel = m.group("base")
            loc = m.group("loc")
        else:
            # le neutre a la meme cle de famille que ses versions culturelles :
            # le suffixe ".resx" ne fait pas partie de la cle
            base_rel = rel[: -len(".resx")] if rel.endswith(".resx") else rel
            loc = ""
        fam = families.setdefault(
            base_rel,
            {"base": base_rel, "kind": classify(base_rel), "locales": {}, "neutral": None},
        )
        if loc:
            fam["locales"][loc] = path
        else:
            fam["neutral"] = path

    report: dict = {"root": str(root), "categories": {}, "module_level": {}}

    # --- categorie "autre" : uniquement un compte, pas un detail fichier a fichier
    for kind in ("agr-core", "module-level", "autre"):
        fams = [f for f in families.values() if f["kind"] == kind]
        report["categories"][kind] = {"familles": len(fams)}

    # --- famille module-level : detail
    mod = [f for f in families.values() if f["kind"] == "module-level"]
    rows = []
    for f in sorted(mod, key=lambda x: x["base"]):
        n_keys = n_nonempty = n_ph = 0
        neutral_names: list[str] = []
        if f["neutral"] is not None:
            n_keys, n_nonempty, n_ph, neutral_names = read_keys(f["neutral"])
        locs = sorted(f["locales"])
        missing_targets = [t for t in TARGETS if t not in locs]
        is_core = all(c in locs for c in CORE_LOCALES)
        # parite du jeu de cles avec la reference ru-RU livree par dnnsoftware
        ru_names: list[str] = []
        if "ru-RU" in f["locales"]:
            ru_names = read_keys(f["locales"]["ru-RU"])[3]
        ru_same = bool(ru_names) and set(ru_names) == set(neutral_names)
        rows.append(
            {
                "base": f["base"],
                "a_neutre": f["neutral"] is not None,
                "neutral_cles": n_keys,
                "neutral_non_vides": n_nonempty,
                "neutral_avec_placeholder": n_ph,
                "est_core": is_core,
                "parite_cles_ru": ru_same,
                "cles_ru": len(ru_names),
                "dossier": f["base"].split("/")[0],
                "locales": locs,
                "cibles_manquantes": missing_targets,
            }
        )

    with_neutral = [r for r in rows if r["a_neutre"]]
    par_dossier: dict[str, dict] = {}
    for r in with_neutral:
        dossier = r["base"].split("/")[0]
        d = par_dossier.setdefault(dossier, {"fichiers": 0, "cles": 0, "non_vides": 0, "cibles_manquantes": 0})
        d["fichiers"] += 1
        d["cles"] += r["neutral_cles"]
        d["non_vides"] += r["neutral_non_vides"]
        if r["cibles_manquantes"]:
            d["cibles_manquantes"] += 1

    report["module_level"] = {
        "familles": len(mod),
        "avec_neutre": len(with_neutral),
        "total_cles_neutres": sum(r["neutral_cles"] for r in with_neutral),
        "total_non_vides": sum(r["neutral_non_vides"] for r in with_neutral),
        "familles_avec_une_cible_ar_fa_zh": [r["base"] for r in rows if r["base"] and not r["cibles_manquantes"]],
        "par_dossier": par_dossier,
        "fichiers": rows,
    }

    # --- le "coeur" DNN : les familles que dnnsoftware livre en ru+pt+es+fr.
    # C'est le perimetre que le dispatch nomme ("~94 fichiers sur 97 core") ; le
    # reste (PersonaBar, NBright, ...) a ses propres locales et n'est pas du core.
    core = [r for r in with_neutral if r["est_core"]]
    core_restant = [r for r in core if r["cibles_manquantes"]]
    core_par_dossier: dict[str, dict] = {}
    for r in core_restant:
        d = core_par_dossier.setdefault(
            r["dossier"], {"fichiers": 0, "cles": 0, "non_vides": 0, "placeholders": 0}
        )
        d["fichiers"] += 1
        d["cles"] += r["neutral_cles"]
        d["non_vides"] += r["neutral_non_vides"]
        d["placeholders"] += r["neutral_avec_placeholder"]
    report["module_level"]["core"] = {
        "definition": "familles portant ru-RU + pt-BR + es-ES + fr-FR (pack dnnsoftware)",
        "familles": len(core),
        "familles_sans_ar_fa_zh": len(core_restant),
        "cles_neutres": sum(r["neutral_cles"] for r in core_restant),
        "cles_non_vides": sum(r["neutral_non_vides"] for r in core_restant),
        "cles_avec_placeholder": sum(r["neutral_avec_placeholder"] for r in core_restant),
        "parite_jeu_de_cles_ru_identique": sum(1 for r in core_restant if r["parite_cles_ru"]),
        "par_dossier": core_par_dossier,
    }

    if args.json:
        Path(args.json).write_text(json.dumps(report, ensure_ascii=False, indent=1), encoding="utf-8")
        print(f"rapport JSON ecrit : {args.json}")

    print(f"== {dnn} ==")
    for kind, c in report["categories"].items():
        print(f"  {kind:14s} : {c['familles']} familles")
    ml = report["module_level"]
    print(f"\n== module-level ==")
    print(f"  familles                : {ml['familles']} (avec neutre : {ml['avec_neutre']})")
    print(f"  cles neutres            : {ml['total_cles_neutres']}")
    print(f"  valeurs non vides       : {ml['total_non_vides']}")
    print(f"  familles deja couvertes : {len(ml['familles_avec_une_cible_ar_fa_zh'])}")
    for b in ml["familles_avec_une_cible_ar_fa_zh"]:
        print(f"      - {b}")
    print("\n  par dossier :")
    for dossier, d in sorted(par_dossier.items()):
        print(
            f"      {dossier:22s} fichiers={d['fichiers']:3d} cles={d['cles']:5d} "
            f"non_vides={d['non_vides']:5d} cibles_manquantes={d['cibles_manquantes']:3d}"
        )

    core = report["module_level"]["core"]
    print(f"\n== CORE (ru+pt+es+fr, {core['definition']}) ==")
    print(f"  familles core                : {core['familles']}")
    print(f"  core sans ar/fa/zh           : {core['familles_sans_ar_fa_zh']}")
    print(f"  cles a produire / langue     : {core['cles_non_vides']} (neutres {core['cles_neutres']})")
    print(f"  cles portant un placeholder  : {core['cles_avec_placeholder']}")
    print(f"  parite du jeu de cles avec ru: {core['parite_jeu_de_cles_ru_identique']}"
          f" / {core['familles_sans_ar_fa_zh']}")
    print("  par dossier (decoupe proposee) :")
    for dossier, d in sorted(core["par_dossier"].items(), key=lambda kv: -kv[1]["non_vides"]):
        print(
            f"      {dossier:22s} fichiers={d['fichiers']:3d} cles={d['cles']:5d} "
            f"non_vides={d['non_vides']:5d} placeholders={d['placeholders']:4d}"
        )
    return 0


if __name__ == "__main__":
    sys.exit(main())
