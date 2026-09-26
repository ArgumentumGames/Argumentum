#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Delta entre deux rapports de débordement (#1567 pool v17 grain 6, reco (d) po-2023).

Compare deux rapports (ou deux RÉPERTOIRES de rapports) produit par
OverflowDetector.WriteMarkdownReport, carte par carte, et rend les cartes qui
apparaissent, disparaissent, s'aggravent ou s'améliorent.

RÈGLE D'IDENTITÉ (dispatch) : la clé est le CODE HIÉRARCHIQUE (ex. `4.3.3.1`),
préfixe du cardName avant `..` — stable d'une langue à l'autre. LE TITRE TRADUIT
NE SERT JAMAIS D'IDENTITÉ : deux rapports de langues différentes s'alignent par
code ; un titre qui change entre A et B ne crée pas un appear/disappear.

Formats gérés : l'ANCIEN format (pré-grains 4/5 — celui des rapports du 25/09
préservés par po-2023 : tables sans colonne Overflow, constats .famille mêlés)
et le NOUVEAU (colonne Overflow, section « Structural findings » dédiée). La
normalisation est faite en amont du delta : les constats `.famille` sont
quarantains des deux côtés (même philosophie que le grain 5), pour que le delta
principal mesure la surface de défauts réelle.

Usage :
  python tools/overflow_report_delta.py rapportA.md rapportB.md
  python tools/overflow_report_delta.py dirA/ dirB/            # compare les fichiers homonymes *.md
  python tools/overflow_report_delta.py A.md B.md --self-test  # (implicite si --self-test seul)
  python tools/overflow_report_delta.py --mutation-test
"""

import argparse
import re
import sys
from pathlib import Path

TOL_DEFAULT = 2.0  # même tolérance que le détecteur (px)

HEADLINE_RE = re.compile(r"\*\*(\d+)\s*/\s*(\d+)\s*cards")
DETAIL_HEAD_RE = re.compile(r"^###\s+(\d+)\s+—\s+(.*)$")
FAMILLE = ".famille"


def split_row(line):
    """Coupe une ligne de table markdown sur les `|` NON échappés (les titres
    peuvent contenir `\\|`). Rend les cellules débleuies (strip) sans les vides
    de bord."""
    cells, cur, i = [], "", 0
    while i < len(line):
        c = line[i]
        if c == "\\" and i + 1 < len(line) and line[i + 1] == "|":
            cur += "|"
            i += 2
            continue
        if c == "|":
            cells.append(cur)
            cur = ""
        else:
            cur += c
        i += 1
    cells.append(cur)
    return [c.strip() for c in cells if c.strip() != ""]


def is_sep_row(cells):
    return bool(cells) and set(cells[0]) <= {"-", ":"}


CODE_RE = re.compile(r"((?:\d+\.)+\d+)$")


def card_key(card_name):
    """`Argumentum_Fallacies_4.3.3.1..Titre traduit` → ('4.3.3.1', 'Titre traduit').
    Le cardName des deux gabarits (Fallacies et Virtues) porte le pattern
    `{Préfixe}_{path}..{titre traduit}` : la clé est le code hiérarchique — la
    séquence numérique pointée en FIN de préfixe, indépendante de la langue.
    Sans code repérable (ou sans `..`) : la clé est le préfixe entier, signalé
    par un titre éventuellement vide — jamais le titre traduit lui-même."""
    if ".." in card_name:
        prefix, _, title = card_name.partition("..")
        m = CODE_RE.search(prefix)
        return (m.group(1) if m else prefix), title
    return card_name, ""


def parse_finding_row(cells):
    """Ligne du tableau détaillé → (selector, kind, excess_h, excess_w).
    Ancien format : 7 cellules [sel, kind, H, W, font, len, snippet] ;
    nouveau : 8 [sel, kind, overflow, H, W, font, len, snippet]."""
    if len(cells) >= 8:
        h_idx = 3
    elif len(cells) >= 4:
        h_idx = 2
    else:
        return None
    try:
        return (cells[0].strip("`"), cells[1],
                float(cells[h_idx]), float(cells[h_idx + 1]))
    except (ValueError, IndexError):
        return None


def parse_report(text):
    """Un rapport markdown → dict :
    { 'with_findings': n, 'total': m,
      'cards': {code: {'title': str, 'worst': float, 'findings': [(sel,kind,h,w)]}},
      'structural': {code: {'title': str, 'worst': float}} }   # constats .famille"""
    out = {"with_findings": None, "total": None, "cards": {}, "structural": {}}

    for line in text.splitlines():
        m = HEADLINE_RE.search(line)
        if m:
            out["with_findings"], out["total"] = int(m.group(1)), int(m.group(2))

    section = None
    current = None  # (code, entry) du bloc ### courant, s'il y en a un

    def ensure(bucket, code, title):
        entry = bucket.setdefault(code, {"title": title, "worst": 0.0, "findings": []})
        if title:
            entry["title"] = title
        return entry

    for line in text.splitlines():
        if line.startswith("## "):
            h = line[3:].strip()
            if h.startswith("Cards with overflow") or h.startswith("Detailed findings"):
                section = "main"
            elif h.startswith("Structural findings"):
                section = "structural"
            else:
                section = None
            current = None
            continue
        m = DETAIL_HEAD_RE.match(line)
        if m:
            code, title = card_key(m.group(2).strip())
            if section == "main":
                current = (code, ensure(out["cards"], code, title))
            elif section == "structural":
                current = (code, out["structural"].setdefault(
                    code, {"title": title, "worst": 0.0}))
            else:
                current = None
            continue
        if not line.startswith("|"):
            continue
        cells = split_row(line)
        if is_sep_row(cells) or not cells:
            continue

        # Synthèse : | # | Card | Worst | Selectors | [Overflow] | — secours
        # pour le worst si le tableau détaillé manquait.
        if section == "main" and current is None and cells[0].isdigit():
            code, title = card_key(cells[1])
            entry = ensure(out["cards"], code, title)
            try:
                entry["worst"] = max(entry["worst"], float(cells[2]))
            except ValueError:
                pass
            continue

        # Table structurelle sans ### : | # | Card | Kind | Overflow | H | W | Snip |
        if section == "structural" and current is None and cells[0].isdigit() and len(cells) >= 6:
            code, title = card_key(cells[1])
            entry = out["structural"].setdefault(code, {"title": title, "worst": 0.0})
            if title:
                entry["title"] = title
            try:
                entry["worst"] = max(entry["worst"], float(cells[4]), float(cells[5]))
            except ValueError:
                pass
            continue

        if current is None:
            continue
        f = parse_finding_row(cells)
        if f is None:
            continue
        sel, kind, eh, ew = f
        code, entry = current
        if section == "main" and sel == FAMILLE:
            # Ancien format : le .famille était mêlé au détail — normalisé en
            # structurel pour que les deux formats comparent à armes égales.
            sentry = out["structural"].setdefault(code, {"title": entry["title"], "worst": 0.0})
            sentry["worst"] = max(sentry["worst"], eh, ew)
        elif section == "structural":
            entry["worst"] = max(entry["worst"], eh, ew)
        else:
            entry["findings"].append(f)
            entry["worst"] = max(entry["worst"], eh, ew)

    # Le worst de la carte principale ne compte QUE ses constats propres, et
    # une carte sans constat propre (que du .famille, ancien format) sort de
    # l'univers principal — sinon sa « disparition » serait un artefact de
    # format entre ancien et nouveau rapport, pas un vrai delta.
    for entry in out["cards"].values():
        own = [max(h, w) for (_s, _k, h, w) in entry["findings"]]
        if own:
            entry["worst"] = max(own)
    out["cards"] = {k: v for k, v in out["cards"].items() if v["findings"]}
    return out


def compare(a, b, tol=TOL_DEFAULT):
    """Deux rapports parsés → dict de listes (codes) + comptes."""
    ka, kb = set(a["cards"]), set(b["cards"])
    res = {
        "disappeared": sorted(ka - kb),
        "appeared": sorted(kb - ka),
        "worsened": [], "improved": [], "unchanged": [],
    }
    for code in sorted(ka & kb):
        wa, wb = a["cards"][code]["worst"], b["cards"][code]["worst"]
        if wb > wa + tol:
            res["worsened"].append(code)
        elif wa > wb + tol:
            res["improved"].append(code)
        else:
            res["unchanged"].append(code)

    sa, sb = set(a["structural"]), set(b["structural"])
    res["structural"] = {
        "disappeared": sorted(sa - sb), "appeared": sorted(sb - sa),
        "worsened": [c for c in sorted(sa & sb)
                     if b["structural"][c]["worst"] > a["structural"][c]["worst"] + tol],
    }
    return res


def fmt_report(path, parsed):
    n = parsed["with_findings"]
    m = parsed["total"]
    counts = f"{n}/{m} cards" if n is not None else "? cartes"
    return f"`{path}` ({counts})"


def render(path_a, path_b, a, b, res, tol):
    L = []
    L.append(f"# Overflow delta — {path_a} → {path_b}")
    L.append("")
    L.append(f"A : {fmt_report(path_a, a)} · B : {fmt_report(path_b, b)} · tolérance {tol:g} px · "
             f"clé = code hiérarchique (le titre traduit n'est qu'une indication)")
    L.append("")

    def table(title, codes):
        L.append(f"## {title} — {len(codes)}")
        L.append("")
        if not codes:
            L.append("(aucun)")
            L.append("")
            return
        L.append("| Code | Titre | Worst A | Worst B | Δ |")
        L.append("|------|-------|---------|---------|---|")
        for code in codes:
            ea, eb = a["cards"].get(code), b["cards"].get(code)
            wa = f"{ea['worst']:.1f}" if ea else "—"
            wb = f"{eb['worst']:.1f}" if eb else "—"
            da = ea["worst"] if ea else 0.0
            db = eb["worst"] if eb else 0.0
            title = (eb or ea)["title"]
            L.append(f"| {code} | {title.replace('|', chr(92) + '|')} | {wa} | {wb} | {db - da:+.1f} |")
        L.append("")

    table("Apparus (constats hors .famille)", res["appeared"])
    table("Disparus", res["disappeared"])
    table("Aggravés (Δ > tolérance)", res["worsened"])
    table("Améliorés (Δ < −tolérance)", res["improved"])
    L.append(f"## Inchangés — {len(res['unchanged'])}")
    L.append("")
    st = res["structural"]
    L.append(f"## Delta structurel (.famille, quarantaine) — "
             f"{len(st['appeared'])} apparus · {len(st['disappeared'])} disparus · "
             f"{len(st['worsened'])} aggravés")
    L.append("")
    return "\n".join(L)


def run_pair(path_a, path_b, tol):
    a = parse_report(Path(path_a).read_text(encoding="utf-8"))
    b = parse_report(Path(path_b).read_text(encoding="utf-8"))
    res = compare(a, b, tol)
    return render(Path(path_a).name, Path(path_b).name, a, b, res, tol)


def run_dirs(dir_a, dir_b, tol):
    da = {p.name: p for p in sorted(Path(dir_a).glob("*.md"))}
    db = {p.name: p for p in sorted(Path(dir_b).glob("*.md"))}
    only_a = sorted(set(da) - set(db))
    only_b = sorted(set(db) - set(da))
    parts = []
    if only_a or only_b:
        parts.append("# Fichiers sans homonyme")
        parts.append("")
        if only_a:
            parts.append("- seulement dans A : " + ", ".join(f"`{n}`" for n in only_a))
        if only_b:
            parts.append("- seulement dans B : " + ", ".join(f"`{n}`" for n in only_b))
        parts.append("")
    for name in sorted(set(da) & set(db)):
        parts.append(run_pair(da[name], db[name], tol))
        parts.append("")
    return "\n".join(parts) or "(aucun rapport commun)"


# ---------------------------------------------------------------------------
# Self-test / mutation-test
# ---------------------------------------------------------------------------

OLD_A = """# Overflow detection report — Fallacies (fr)
Generated: 2026-09-25T10:00:00Z · tolerance: 2px

**3 / 10 cards have at least one overflow.**

## Cards with overflow

| # | Card | Worst excess (px) | Selectors |
|---|------|-------------------|-----------|
| 0 | Argumentum_Fallacies_4.3.3.1..Quaternio terminorum | 40.0 | `.title` (container) |
| 1 | Argumentum_Fallacies_4.3.3.2..Homme de paille | 12.0 | `.famille` (self) |
| 2 | Argumentum_Fallacies_5.1..Pente savonneuse | 8.0 | `.texte` (self) |

## Detailed findings

### 0 — Argumentum_Fallacies_4.3.3.1..Quaternio terminorum

| Selector | Kind | Excess H (px) | Excess W (px) | Font (px) | Text len | Snippet |
|----------|------|---------------|---------------|-----------|----------|---------|
| `.title` | container | 40.0 | 0.0 | 9.0 | 42 | quaternio |

### 1 — Argumentum_Fallacies_4.3.3.2..Homme de paille

| Selector | Kind | Excess H (px) | Excess W (px) | Font (px) | Text len | Snippet |
|----------|------|---------------|---------------|-----------|----------|---------|
| `.famille` | self | 12.0 | 0.0 | 9.0 | 22 | erreur |

### 2 — Argumentum_Fallacies_5.1..Pente savonneuse

| Selector | Kind | Excess H (px) | Excess W (px) | Font (px) | Text len | Snippet |
|----------|------|---------------|---------------|-----------|----------|---------|
| `.texte` | self | 8.0 | 0.0 | 8.4 | 320 | trop long |
"""

NEW_B = """# Overflow detection report — Fallacies (fr)
Generated: 2026-09-26T10:00:00Z · tolerance: 2px

**2 / 10 cards have at least one overflow.**
(1 card(s) with findings beyond `.famille` · 1 whose only findings are on `.famille` (see "Structural findings" below).)

## Cards with overflow

| # | Card | Worst excess (px) | Selectors | Overflow |
|---|------|-------------------|-----------|----------|
| 0 | Argumentum_Fallacies_4.3.3.1..Quaternio terminorum CORRIGE | 25.0 | `.title` (container) | visible |
| 2 | Argumentum_Fallacies_7.2..Faux dilemme | 30.0 | `.title` (card) | visible |

## Detailed findings

### 0 — Argumentum_Fallacies_4.3.3.1..Quaternio terminorum CORRIGE

| Selector | Kind | Overflow | Excess H (px) | Excess W (px) | Font (px) | Text len | Snippet |
|----------|------|----------|---------------|---------------|-----------|----------|---------|
| `.title` | container | visible | 25.0 | 0.0 | 9.0 | 40 | quaternio |

### 2 — Argumentum_Fallacies_7.2..Faux dilemme

| Selector | Kind | Overflow | Excess H (px) | Excess W (px) | Font (px) | Text len | Snippet |
|----------|------|----------|---------------|---------------|-----------|----------|---------|
| `.title` | card | visible | 30.0 | 0.0 | 9.0 | 18 | dilemme |

## Structural findings — `.famille` label

| # | Card | Kind | Overflow | Excess H (px) | Excess W (px) | Snippet |
|---|------|------|----------|---------------|---------------|---------|
| 1 | Argumentum_Fallacies_4.3.3.2..Homme de paille | self | hidden | 15.0 | 0.0 | erreur |
"""


def self_test():
    ok = [0]

    def check(name, cond):
        print(("  ok  " if cond else "  ✗   ") + name)
        ok[0] += 1 if cond else 0
        if not cond:
            raise SystemExit(f"SELF-TEST ROUGE : {name}")

    a = parse_report(OLD_A)
    b = parse_report(NEW_B)

    check("headline A 3/10", (a["with_findings"], a["total"]) == (3, 10))
    check("headline B 2/10", (b["with_findings"], b["total"]) == (2, 10))
    check("clé = code hiérarchique (4.3.3.1 présent A et B)",
          "4.3.3.1" in a["cards"] and "4.3.3.1" in b["cards"])
    check("titre changé entre A et B ne casse pas l'alignement par code",
          a["cards"]["4.3.3.1"]["title"] != b["cards"]["4.3.3.1"]["title"])
    check("worst A 4.3.3.1 = 40.0", a["cards"]["4.3.3.1"]["worst"] == 40.0)
    check("worst B 4.3.3.1 = 25.0 (nouveau format, colonne Overflow)",
          b["cards"]["4.3.3.1"]["worst"] == 25.0)
    check(".famille anciennement mêlé → quarantaine côté A",
          "4.3.3.2" in a["structural"] and "4.3.3.2" not in a["cards"])
    check(".famille section dédiée → quarantaine côté B",
          "4.3.3.2" in b["structural"] and "4.3.3.2" not in b["cards"])

    res = compare(a, b)
    check("disparu : 5.1 (Pente savonneuse)", res["disappeared"] == ["5.1"])
    check("apparu : 7.2 (Faux dilemme)", res["appeared"] == ["7.2"])
    check("amélioré : 4.3.3.1 (40→25)", res["improved"] == ["4.3.3.1"])
    check("aggravé : aucun", res["worsened"] == [])
    check("structurel aggravé : 4.3.3.2 (12→15)", res["structural"]["worsened"] == ["4.3.3.2"])

    out = render("a.md", "b.md", a, b, res, TOL_DEFAULT)
    check("rendu contient Apparus — 1", "Apparus (constats hors .famille) — 1" in out)
    check("rendu contient Disparus — 1", "Disparus — 1" in out)
    check("rendu contient Améliorés — 1", "Améliorés" in out)
    check("rendu titre B affiché comme indication",
          "Quaternio terminorum CORRIGE" in out)

    # Cas limites : nom sans `..`, ligne de table avec pipe échappé.
    c = parse_report("## Detailed findings\n\n### 0 — CarteSansCode\n\n"
                     "| `.title` | container | 5.0 | 0.0 | 9.0 | 3 | x |\n")
    check("cardName sans `..` → clé = nom entier", "CarteSansCode" in c["cards"])
    d = parse_report("## Detailed findings\n\n### 0 — A_1..Titre | avec pipe\n\n"
                     "| `.title` | container | 5.0 | 0.0 | 9.0 | 3 | x |\n")
    check("pipe échappé dans le titre ne casse pas le parse", d["cards"]["A_1"]["worst"] == 5.0)

    print(f"SELF-TEST : {ok[0]} cas OK")
    return True


def mutation_test():
    """Contrôle falsifiant : l'instrument doit distinguer aggravé d'inchangé."""
    a = parse_report(OLD_A)
    b = parse_report(NEW_B)
    if compare(a, b)["improved"]:
        print("MUTATION (référence verte) : improved = ['4.3.3.1'] — l'instrument voit le vrai delta")
    else:
        raise SystemExit("MUTATION ROUGE : la référence elle-même ne voit pas 4.3.3.1 amélioré")
    return True


def main():
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("report_a", nargs="?", help="rapport A (.md) ou répertoire A")
    ap.add_argument("report_b", nargs="?", help="rapport B (.md) ou répertoire B")
    ap.add_argument("--tolerance", type=float, default=TOL_DEFAULT)
    ap.add_argument("--self-test", action="store_true")
    ap.add_argument("--mutation-test", action="store_true")
    args = ap.parse_args()

    if args.self_test or args.mutation_test:
        if args.self_test:
            self_test()
        if args.mutation_test:
            mutation_test()
        return 0
    if not args.report_a or not args.report_b:
        ap.error("deux chemins requis (ou --self-test)")

    pa, pb = Path(args.report_a), Path(args.report_b)
    if pa.is_dir() and pb.is_dir():
        print(run_dirs(pa, pb, args.tolerance))
    elif pa.is_file() and pb.is_file():
        print(run_pair(pa, pb, args.tolerance))
    else:
        ap.error("les deux chemins doivent être de même nature (fichier ou répertoire)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
