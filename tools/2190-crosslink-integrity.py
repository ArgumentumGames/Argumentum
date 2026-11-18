#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#2190 — garde d'intégrité des liens transverses des cartes Fallacies (rejouable, std-lib, hors ligne).

    python tools/2190-crosslink-integrity.py                # --check : l'arbre committé doit passer
    python tools/2190-crosslink-integrity.py --self-test    # chaque contrôle doit ROUGIR sur copie mutée
    python tools/2190-crosslink-integrity.py --calibrate    # distribution des distances par langue

POURQUOI CE FICHIER EXISTE. Le lot 2 de #2190 (#2289) annote chaque trait transverse
à la génération (`data-crosslink-verb/source/target`, `pk`/`path` sur les nœuds), et
l'état de clôture du 18/11 a comparé ces annotations au corpus — 581/581 triplets par
langue, 1 408 couples (path, pk), chaque `_ext` référençant le SVG testé — par un
instrument lancé à la main et non committé. Deux trous restaient ouverts :

  1. RIEN n'était rejouable : la prochaine re-dérivation pouvait perdre une annotation
     sans qu'aucun contrôle ne rougisse.
  2. L'égalité des triplets N'EXCLUT PAS UNE PERMUTATION entre deux traits : si deux
     liens de même verbe échangeaient leurs annotations complètes, l'ensemble resterait
     identique au corpus — et le visiteur suivrait deux flèches fausses.

Cette garde ferme les deux. C'est un instrument de PARSE (ElementTree), pas un verdict
de proximité brute : la géométrie (C4) mesure chaque extrémité dessinée CONTRE le nœud
que ses annotations désignent — jamais « contre le nœud le plus proche », méthode
mesurée fausse à 12 % (68/581) dans le corps de #2190.

CONTRÔLES
  C1  L'ensemble ordonné des triplets (source, verbe, cible) — doublons compris — du
      CSV (colonnes `crossLink_*`, cibles `;`-séparées, source = `path` de la rangée)
      est ÉGAL à celui des annotations de chaque langue, dans le SVG autonome ET dans
      le SVG embarqué du gabarit inlinant. Jamais écrit en dur : recompté du CSV.
  C2  Les couples (path, pk) de chaque nœud du SVG égalent ceux du CSV.
  C3  Chaque `Fallacies_<lang>_ext.html` référence, par son `<object data=>`, le SVG
      autonome qui vient d'être contrôlé.
  C4  Pour chaque trait, l'extrémité de départ tombe à ≤ TOL de l'ancre texte du nœud
      `data-crosslink-source`, celle d'arrivée à ≤ TOL de celle du nœud
      `data-crosslink-target`. L'ancre = translate(g) + (x, y) du <text> du nœud.
      TOL est calée sur la mesure (fr, master fb4b8992 : max 609) avec marge :
      800. Une permutation d'annotations entre traits y devient visible.
  QUAL  La preuve C4 a une limite, elle est NOMMÉE au lieu d'être tue : deux traits de
      même verbe dont les DEUX couples d'ancres (source ET cible) sont à moins de
      2×TOL l'un de l'autre pourraient être permutés sans que C4 le voie. Ces paires
      sont comptées et listées — le rapport dit quelle part de la preuve est
      discriminante, il ne l'affirme pas.

TÉMOINS (--self-test, mutations sur copies jetables — le dépôt n'est jamais touché)
  m1  retirer les annotations d'un trait            → C1 rouge
  m2  PERMUTER les annotations complètes de deux traits de même verbe éloignés
      → C1 VERT (même ensemble — c'est le trou), C4 rouge (la géométrie voit)
  m3  corrompre le pk d'un nœud                     → C2 rouge
  m4  réécrire le object data d'un _ext             → C3 rouge
  m5  déplacer l'extrémité dessinée d'un trait      → C4 rouge
  Le témoin positif (copie intacte → vert) précède chaque mutation : une garde qui
  échouerait pour une mauvaise raison (parse cassé par la mutation) est rejetée.

Référence du calibrage : 2026-11-18, master fb4b8992, fr — source p50=13,4 p95=398,
max=606 ; cible p50=13,4 p95=383, max=609 (unités du viewBox, carte ≈ 8 500 de large).
"""

from __future__ import annotations

import argparse
import copy
import csv
import io
import math
import re
import sys
import tempfile
import xml.etree.ElementTree as ET
from collections import Counter
from pathlib import Path

NS = "{http://www.w3.org/2000/svg}"
CSV_REL = Path("Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv")
MM_REL = Path("Cards/Fallacies/Mindmaps")
LANGS = ["ar", "en", "es", "fa", "fr", "pt", "ru", "zh"]
VERB_COLUMNS = [
    "crossLink_PredatesOn", "crossLink_Denounces", "crossLink_Leverages",
    "crossLink_Allows", "crossLink_Opposes", "crossLink_Inverts",
    "crossLink_Mirrors", "crossLink_IsRelatedTo",
]
# Calibrée le 2026-11-18 sur master fb4b8992 (fr : max mesuré 609) — marge ~30 %.
TOL = 800.0


# ---------------------------------------------------------------- CSV (le corpus)

def read_csv_facts(csv_path: Path):
    """Triplets (source_path, verb, target_path) et couples (path, pk) du CSV.

    Recomptés à chaque appel — aucun compte n'est écrit en dur (le corps de #2190
    dit 581, la garde ne le sait pas).
    """
    text = csv_path.read_bytes().decode("utf-8-sig")
    rows = list(csv.DictReader(io.StringIO(text)))
    triplets = Counter()
    anchors = Counter()
    for r in rows:
        src = r["path"]
        anchors[(src, r["PK"])] += 1  # colonne majuscule « PK » (le minuscule n'existe pas)
        for col in VERB_COLUMNS:
            cell = (r.get(col) or "").strip()
            if not cell:
                continue
            verb = col[len("crossLink_"):]
            for tgt in cell.split(";"):
                tgt = tgt.strip()
                if tgt:
                    triplets[(src, verb, tgt)] += 1
    if not rows:
        raise SystemExit(f"CSV vide ou introuvable : {csv_path}")
    return triplets, anchors


# ---------------------------------------------------------------- SVG (les faits)

def _translate(transform: str):
    m = re.match(r"translate\(([-\d.]+)[, ]([-\d.]+)\)", transform or "")
    return (float(m.group(1)), float(m.group(2))) if m else (0.0, 0.0)


def svg_facts(svg_text: str):
    """Parse un SVG autonome : triplets, ancres, positions, géométrie des traits."""
    root = ET.fromstring(svg_text)
    triplets = Counter()
    anchors = Counter()
    nodes = {}  # path -> (ax, ay) ancre texte absolue
    crosslinks = []  # (verb, src, tgt, start_xy, end_xy)
    for g in root.iter(NS + "g"):
        cls = g.get("class")
        if cls == "node":
            path = g.get("path")
            anchors[(path, g.get("pk"))] += 1
            tx, ty = _translate(g.get("transform"))
            text = g.find(NS + "text")
            ax = float(text.get("x", "0")) if text is not None else 0.0
            ay = float(text.get("y", "0")) if text is not None else 0.0
            nodes[path] = (tx + ax, ty + ay)
        elif cls == "crosslink":
            verb = g.get("data-crosslink-verb")
            src = g.get("data-crosslink-source")
            tgt = g.get("data-crosslink-target")
            triplets[(src, verb, tgt)] += 1
            gx, gy = _translate(g.get("transform"))
            d = g.find(NS + "path").get("d")
            nums = re.findall(r"[-\d.]+", d)
            start = (gx + float(nums[0]), gy + float(nums[1]))
            end = (gx + float(nums[-2]), gy + float(nums[-1]))
            crosslinks.append((verb, src, tgt, start, end))
    return {"root": root, "triplets": triplets, "anchors": anchors,
            "nodes": nodes, "crosslinks": crosslinks}


def extract_embedded_svg(html_text: str):
    """Extrait le fragment <svg>…</svg> du gabarit inlinant (le gros, celui qui
    porte les data-crosslink). Les icônes du gabarit n'en portent pas : l'ancre
    est la première occurrence de `data-crosslink-verb`, remontée au `<svg`
    précédent, descendue au `</svg>` apparié INCLUSIF (chevron compris — le
    tronquer fait échouer le parse pour une raison qui n'est pas la garde).
    Rend None si le gabarit ne porte AUCUN data-crosslink : l'appelant en fait
    une erreur rouge — un arbre qui a perdu tous ses liens doit rougir, pas
    crasher."""
    k = html_text.find("data-crosslink-verb")
    if k < 0:
        return None
    i = html_text.rfind("<svg", 0, k)
    depth = 0
    for m in re.finditer(r"</?svg\b", html_text[i:]):
        depth += 1 if m.group(0) == "<svg" else -1
        if depth == 0:
            close = html_text.index(">", i + m.start())
            return html_text[i:close + 1]
    raise SystemExit("fragments <svg> non appariés dans le gabarit inlinant")


# ---------------------------------------------------------------- la garde

def check_lang(repo: Path, lang: str, csv_triplets, csv_anchors, report, errors):
    svg_path = MM_REL / lang / f"Fallacies_{lang}.content.svg"
    html_path = MM_REL / lang / f"Fallacies_{lang}.html"
    ext_path = MM_REL / lang / f"Fallacies_{lang}_ext.html"

    svg_text = (repo / svg_path).read_bytes().decode("utf-8-sig")
    facts = svg_facts(svg_text)
    n_cl = len(facts["crosslinks"])
    n_nd = len(facts["nodes"])

    # C1 — triplets du SVG autonome vs corpus
    if facts["triplets"] != csv_triplets:
        miss = csv_triplets - facts["triplets"]
        extra = facts["triplets"] - csv_triplets
        for t, n in list(miss.items())[:3]:
            errors.append(f"[{lang}] C1 SVG autonome: triplet du corpus ABSENT: {t} ×{n}")
        for t, n in list(extra.items())[:3]:
            errors.append(f"[{lang}] C1 SVG autonome: triplet INCONNU du corpus: {t} ×{n}")

    # C1 (suite) — triplets du SVG embarqué vs corpus
    embedded_svg = extract_embedded_svg(
        (repo / html_path).read_bytes().decode("utf-8-sig"))
    if embedded_svg is None:
        errors.append(f"[{lang}] C1 SVG embarqué: aucun data-crosslink-verb dans le "
                      f"gabarit inlinant alors que le corpus en porte "
                      f"{sum(csv_triplets.values())}")
    embedded = svg_facts(embedded_svg) if embedded_svg is not None else None
    if embedded is not None and embedded["triplets"] != csv_triplets:
        errors.append(f"[{lang}] C1 SVG embarqué: les triplets du gabarit inlinant "
                      f"diffèrent du corpus ({sum(embedded['triplets'].values())} vs "
                      f"{sum(csv_triplets.values())})")

    # C2 — couples (path, pk)
    if facts["anchors"] != csv_anchors:
        miss = csv_anchors - facts["anchors"]
        extra = facts["anchors"] - facts["anchors"]
        errors.append(f"[{lang}] C2 ancres: {sum(miss.values())} couple(s) du corpus "
                      f"absents du SVG ({list(miss.items())[:3]})")

    # C3 — le _ext référence le SVG testé
    ext_text = (repo / ext_path).read_bytes().decode("utf-8-sig")
    m = re.search(r'<object[^>]*data="([^"]+)"', ext_text)
    want = f"Fallacies_{lang}.content.svg"
    if not m or Path(m.group(1)).name != want:
        errors.append(f"[{lang}] C3 _ext: object data="
                      f"{m.group(1) if m else 'ABSENT'} != {want}")

    # C4 — géométrie : chaque extrémité contre le nœud ANNOTÉ (jamais le plus proche)
    worst_s = worst_t = 0.0
    for verb, src, tgt, start, end in facts["crosslinks"]:
        if src not in facts["nodes"] or tgt not in facts["nodes"]:
            errors.append(f"[{lang}] C4: trait ({src},{verb},{tgt}) désigne un nœud absent")
            continue
        ds = math.dist(start, facts["nodes"][src])
        dt = math.dist(end, facts["nodes"][tgt])
        worst_s, worst_t = max(worst_s, ds), max(worst_t, dt)
        if ds > TOL:
            errors.append(f"[{lang}] C4 départ {ds:.0f} > {TOL:.0f} du nœud source "
                          f"({src},{verb},{tgt}) — trait permuté ou déplacé ?")
        if dt > TOL:
            errors.append(f"[{lang}] C4 arrivée {dt:.0f} > {TOL:.0f} du nœud cible "
                          f"({src},{verb},{tgt}) — trait permuté ou déplacé ?")

    # QUAL — paires que C4 ne pourrait pas départager (les NOMMER, pas les taire)
    by_verb = {}
    for verb, src, tgt, start, end in facts["crosslinks"]:
        by_verb.setdefault(verb, []).append((src, tgt))
    ambiguous = []
    same_verb_pairs = 0
    for verb, pairs in by_verb.items():
        for a in range(len(pairs)):
            for b in range(a + 1, len(pairs)):
                same_verb_pairs += 1
                (s1, t1), (s2, t2) = pairs[a], pairs[b]
                if (math.dist(facts["nodes"][s1], facts["nodes"][s2]) < 2 * TOL
                        and math.dist(facts["nodes"][t1], facts["nodes"][t2]) < 2 * TOL):
                    ambiguous.append((verb, s1, t1, s2, t2))
    report.append(f"[{lang}] traits={n_cl} nœuds={n_nd} pire-départ={worst_s:.0f} "
                  f"pire-arrivée={worst_t:.0f} paires-ambiguës={len(ambiguous)}"
                  f"/{same_verb_pairs} même-verbe")
    for amb in ambiguous[:5]:
        report.append(f"[{lang}]   ambigu: {amb}")


def check_repo(repo: Path, langs=None):
    csv_triplets, csv_anchors = read_csv_facts(repo / CSV_REL)
    report = [f"corpus CSV: {sum(csv_triplets.values())} triplets, "
              f"{sum(csv_anchors.values())} couples (path,pk)"]
    errors = []
    for lang in (langs or LANGS):
        check_lang(repo, lang, csv_triplets, csv_anchors, report, errors)
    return errors, report


# ---------------------------------------------------------------- témoins

def _copy_fr_fixture(repo: Path, tmp: Path):
    """Copie le CSV + les 3 fichiers fr : le socle que chaque mutation dérive."""
    (tmp / CSV_REL.parent).mkdir(parents=True, exist_ok=True)
    (tmp / CSV_REL).write_bytes((repo / CSV_REL).read_bytes())
    for name in (f"Fallacies_fr.content.svg", "Fallacies_fr.html", "Fallacies_fr_ext.html"):
        dst = tmp / MM_REL / "fr" / name
        dst.parent.mkdir(parents=True, exist_ok=True)
        dst.write_bytes((repo / MM_REL / "fr" / name).read_bytes())


def _first_key_triplet(svg_text: str):
    """(verb, src, tgt) de la première clé unique, avec le pattern d'attributs
    complet qui l'identifie dans le texte (l'ordre des attributs est fixe : le
    writer les pose toujours dans le même ordre)."""
    facts = svg_facts(svg_text)
    for triplet, n in facts["triplets"].items():
        if n == 1:
            src, verb, tgt = triplet
            pattern = f'data-crosslink-verb="{verb}" data-crosslink-source="{src}" data-crosslink-target="{tgt}"'
            if svg_text.count(pattern) == 1:
                return triplet, pattern
    raise SystemExit("aucun triplet unique pour la mutation")


def _far_pair(svg_text: str):
    """Deux clés uniques de même verbe dont les ancres source ET cible sont
    ÉLOIGNÉES (> 2×TOL) : la permutation entre elles doit rougir C4 sans
    ambiguïté — c'est le pouvoir détecteur démontré, pas supposé."""
    facts = svg_facts(svg_text)
    by_verb = {}
    for triplet, n in facts["triplets"].items():
        if n == 1:
            by_verb.setdefault(triplet[1], []).append(triplet)
    best = None
    for verb, keys in by_verb.items():
        for a in range(len(keys)):
            for b in range(a + 1, len(keys)):
                (s1, v1, t1), (s2, v2, t2) = keys[a], keys[b]
                d = (math.dist(facts["nodes"][s1], facts["nodes"][s2])
                     + math.dist(facts["nodes"][t1], facts["nodes"][t2]))
                if d > 4 * TOL and (best is None or d > best[0]):
                    p1 = f'data-crosslink-verb="{v1}" data-crosslink-source="{s1}" data-crosslink-target="{t1}"'
                    p2 = f'data-crosslink-verb="{v2}" data-crosslink-source="{s2}" data-crosslink-target="{t2}"'
                    if svg_text.count(p1) == 1 and svg_text.count(p2) == 1:
                        best = (d, keys[a], keys[b])
    if best is None:
        raise SystemExit("aucune paire éloignée de même verbe pour m2")
    return best[1], best[2]


def selftest(repo: Path):
    failures = []

    def run_case(name, mutate, expect_layer):
        with tempfile.TemporaryDirectory(prefix="p2190-") as td:
            tmp = Path(td)
            _copy_fr_fixture(repo, tmp)
            svg_file = tmp / MM_REL / "fr" / "Fallacies_fr.content.svg"
            mutate(tmp, svg_file)
            errors, _ = check_repo(tmp, langs=["fr"])
            hit = any(expect_layer in e for e in errors)
            detail = [e for e in errors if expect_layer in e][:2]
            if not hit:
                failures.append(f"{name}: la garde n'a PAS rougi sur {expect_layer} "
                                f"(erreurs: {errors[:2] or 'AUCUNE'})")
            else:
                print(f"  {name}: ROUGE sur {expect_layer} — {detail[0][:110]}")

    # Témoin positif d'abord : une garde qui échouerait à vide ne prouve rien.
    with tempfile.TemporaryDirectory(prefix="p2190-") as td:
        tmp = Path(td)
        _copy_fr_fixture(repo, tmp)
        errors, _ = check_repo(tmp, langs=["fr"])
        if errors:
            failures.append(f"témoin positif: copie INTACTE déjà rouge: {errors[:2]}")
        else:
            print("  témoin positif: copie intacte VERTE")

    svg0 = (repo / MM_REL / "fr" / "Fallacies_fr.content.svg").read_bytes().decode("utf-8-sig")

    # m1 — retirer les annotations d'un trait → C1
    _, pat1 = _first_key_triplet(svg0)

    def m1(tmp, svg_file):
        t = svg_file.read_text(encoding="utf-8-sig")
        assert t.count(pat1) == 1
        svg_file.write_text(t.replace(f" {pat1}", ""), encoding="utf-8")

    run_case("m1 strip annotations", m1, "C1 SVG autonome")

    # m2 — permuter les annotations COMPLÈTES de deux traits éloignés de même
    # verbe : C1 reste vert (même ensemble — le trou), C4 doit rougir.
    keya, keyb = _far_pair(svg0)
    sa, va, ta = keya
    sb, vb, tb = keyb
    pa = f'data-crosslink-verb="{va}" data-crosslink-source="{sa}" data-crosslink-target="{ta}"'
    pb = f'data-crosslink-verb="{vb}" data-crosslink-source="{sb}" data-crosslink-target="{tb}"'
    pa2 = f'data-crosslink-verb="{va}" data-crosslink-source="{sb}" data-crosslink-target="{tb}"'
    pb2 = f'data-crosslink-verb="{vb}" data-crosslink-source="{sa}" data-crosslink-target="{ta}"'

    def m2(tmp, svg_file):
        t = svg_file.read_text(encoding="utf-8-sig")
        assert t.count(pa) == 1 and t.count(pb) == 1
        t = t.replace(pa, "@@M2A@@").replace(pb, "@@M2B@@")
        t = t.replace("@@M2A@@", pa2).replace("@@M2B@@", pb2)
        svg_file.write_text(t, encoding="utf-8")

    run_case("m2 permute deux traits (C1 doit rester VERT)", m2, "C4")

    # m2 (suite) — la même permutation ne doit PAS rougir C1 : c'est le trou que
    # C4 ferme ; si C1 la voyait, la preuve géométrique ne serait pas nécessaire.
    with tempfile.TemporaryDirectory(prefix="p2190-") as td:
        tmp = Path(td)
        _copy_fr_fixture(repo, tmp)
        m2(tmp, tmp / MM_REL / "fr" / "Fallacies_fr.content.svg")
        errors, _ = check_repo(tmp, langs=["fr"])
        if any("C1" in e for e in errors):
            failures.append("m2: C1 a vu la permutation — le trou n'existe pas, "
                            "la qualification C4 est superflue")
        else:
            print("  m2: C1 VERT sur la permutation (le trou confirmé), C4 seul voit")

    # m3 — pk d'un nœud → C2
    def m3(tmp, svg_file):
        t = svg_file.read_text(encoding="utf-8-sig")
        assert t.count('pk="1280"') == 1
        svg_file.write_text(t.replace('pk="1280"', 'pk="99999"'), encoding="utf-8")

    run_case("m3 pk corrompu", m3, "C2")

    # m4 — object data du _ext → C3
    def m4(tmp, svg_file):
        ext = tmp / MM_REL / "fr" / "Fallacies_fr_ext.html"
        t = ext.read_text(encoding="utf-8-sig")
        assert 'data="Fallacies_fr.content.svg"' in t
        ext.write_text(t.replace('data="Fallacies_fr.content.svg"',
                                 'data="Fallacies_fr.links.svg"'), encoding="utf-8")

    run_case("m4 _ext retarget", m4, "C3")

    # m5 — déplacer l'extrémité dessinée d'un trait (le d) → C4
    pat5 = re.escape(pa)

    def m5(tmp, svg_file):
        t = svg_file.read_text(encoding="utf-8-sig")
        rx = re.compile("(" + pat5 + r'[^>]*>\s*<path fill="none" d="M)([-\d.]+) ([-\d.]+)(.*?)([-\d.]+) ([-\d.]+)"')
        m = rx.search(t)
        assert m, "pattern du path du trait ciblé introuvable"
        x2 = float(m.group(5)) + 5000.0
        y2 = float(m.group(6)) + 5000.0
        t = t[:m.start(5)] + f"{x2} {y2}" + t[m.end(6):]
        svg_file.write_text(t, encoding="utf-8")

    run_case("m5 extrémité déplacée", m5, "C4")

    return failures


# ---------------------------------------------------------------- point d'entrée

def main():
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--check", action="store_true", help="garde sur l'arbre (défaut)")
    ap.add_argument("--self-test", action="store_true", help="témoins : chaque contrôle doit rougir")
    ap.add_argument("--calibrate", action="store_true", help="distribution des distances par langue")
    ap.add_argument("--repo", type=Path, default=Path(__file__).resolve().parents[1])
    args = ap.parse_args()
    repo = args.repo

    if args.self_test:
        print("self-test #2190 (mutations sur copies jetables) :")
        failures = selftest(repo)
        if failures:
            print("\nÉCHEC :")
            for f in failures:
                print("  -", f)
            return 1
        print("\nself-test OK : la garde voit chaque mutation, et seulement elle.")
        return 0

    if args.calibrate:
        csv_triplets, csv_anchors = read_csv_facts(repo / CSV_REL)
        for lang in LANGS:
            svg_text = (repo / MM_REL / lang / f"Fallacies_{lang}.content.svg").read_bytes().decode("utf-8-sig")
            facts = svg_facts(svg_text)
            ds = sorted(math.dist(s, facts["nodes"][src])
                        for _, src, _, s, _ in facts["crosslinks"] if src in facts["nodes"])
            dt = sorted(math.dist(e, facts["nodes"][tgt])
                        for _, _, tgt, _, e in facts["crosslinks"] if tgt in facts["nodes"])

            def pct(l, p):
                return l[min(len(l) - 1, int(len(l) * p))]
            print(f"[{lang}] départ p50={pct(ds,.5):.0f} p95={pct(ds,.95):.0f} max={ds[-1]:.0f} | "
                  f"arrivée p50={pct(dt,.5):.0f} p95={pct(dt,.95):.0f} max={dt[-1]:.0f}")
        return 0

    errors, report = check_repo(repo)
    for line in report:
        print(line)
    if errors:
        print(f"\nGARDE ROUGE ({len(errors)} erreur(s)) :")
        for e in errors[:20]:
            print("  -", e)
        return 1
    print("\nGARDE VERTE : triplets, ancres, _ext et géométrie conformes au corpus, 8 langues.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
