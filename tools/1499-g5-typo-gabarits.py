#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 grain 5 — typographie des textes ÉCRITS EN DUR dans les gabarits de carte.

Dispatch pool v16 grain 5 ([#458 c.5842223755](...)) : « mesurer les chaînes écrites en
dur dans la clé `mustache` des gabarits `Cards/**/*.json` qui alimentent un CardSet
(libellés fixes, en-têtes). **Parser le JSON, ne pas le greper** (#1485). Appliquer la
norme de #1562 par langue. »

⛔ MESURE SEULE ici : aucune écriture de gabarit, aucune régénération.
⛔ `Archive/` est EXCLU du balayage (dispatch).

LA NORME (#1562, `tools/994-apostrophe-dryrun.py`, `TARGET`) :
    fr et toutes les langues hors EN -> apostrophe COURBE ’   (la droite est le défaut)
    en                               -> apostrophe DROITE '   (la courbe est le défaut)
    toutes langues                   -> aucun guillemet droit " dans le TEXTE RENDU
`count_wrong` ne compte la mauvaise apostrophe qu'**intra-mot** (les deux voisins sont
alphabétiques) — c'est la même règle ici.

CE QUE L'INSTRUMENT COMPTE, ET CE QU'IL EXCLUT (le point où un grep se trompe) :
    compté   : les NŒUDS DE TEXTE du `mustache` (ce qui s'affiche hors balises),
               et les chaînes de `content:` du `css` (rendues par ::before/::after) ;
    exclu    : les balises et leurs ATTRIBUTS (`class="titre"` est du balisage, pas du
               texte — un comptage brut de `"` y voit des milliers de faux défauts),
               les blocs `<style>`/`<script>`, les commentaires HTML, les expressions
               `{{…}}`/`{{{…}}}` (ce sont des DONNÉES, injectées depuis le CSV).

PREUVE DE NON-AVEUGLEMENT (`--naif`) : le même comptage, **sans** le dépouillement, sur
le même arbre. Il DOIT rendre > 0 — sinon le 0 du mode normal ne prouve rien.

Usage :
    python tools/1499-g5-typo-gabarits.py            # balayage dépouillé (le chiffre qui compte)
    python tools/1499-g5-typo-gabarits.py --naif     # contrôle de non-aveuglement (doit être > 0)
    python tools/1499-g5-typo-gabarits.py --self-test
    python tools/1499-g5-typo-gabarits.py --all      # tout Cards/**/*.json (hors Archive/)
"""
import argparse
import json
import os
import re
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CARDS = os.path.join(REPO, "Cards")
STRAIGHT, DQUOTE, CURVED = "'", '"', "’"

# Les gabarits qui ALIMENTENT un CardSet : noms dérivés de `WebBasedGeneratorConfig.cs`
# (JsonFilePathDebug/Release). Les autres JSON de `Cards/` sont des gabarits d'archive
# ou d'édition et ne sont pas mesurés par défaut.
CARD_SETS = [
    "Rules/Argumentum_Rules_fr.json",
    "Fallacies/Argumentum_Fallacies_Face_fr.json",
    "Fallacies/Argumentum_Fallacies_Face_2_fr.json",
    "Fallacies/Argumentum_Fallacies_Face_3_fr.json",
    "Fallacies/Argumentum_Fallacies_Face_Web_fr.json",
    "Fallacies/Argumentum_Fallacies_Back_fr.json",
    "Fallacies/Argumentum_Virtues_Face_fr.json",
    "Scenarii/Argumentum_Scenarii_Face_fr.json",
    "Scenarii/Argumentum_Scenarii_Back_fr.json",
    "Memo/Argumentum_Memo_Face_fr.json",
    "Memo/Argumentum_Memo_Back_fr.json",
]

RE_STYLE = re.compile(r"<style\b.*?</style>", re.S | re.I)
RE_SCRIPT = re.compile(r"<script\b.*?</script>", re.S | re.I)
RE_COMMENT = re.compile(r"<!--.*?-->", re.S)
RE_TAG = re.compile(r"<[^>]*>", re.S)
RE_EXPR = re.compile(r"\{\{\{?.*?\}?\}\}", re.S)
RE_CONTENT = re.compile(r"content\s*:\s*([^;]+);")


def lang_of(path):
    """Langue du gabarit, dérivée du NOM (et non supposée)."""
    b = os.path.basename(path)
    if re.search(r"(_En|English|_en)\b", b) or b.endswith("_En.json"):
        return "en"
    return "fr"


def text_nodes(html):
    """Nœuds de texte du `mustache` : balisage, attributs, blocs et {{…}} retirés."""
    t = RE_STYLE.sub(" ", html)
    t = RE_SCRIPT.sub(" ", t)
    t = RE_COMMENT.sub(" ", t)
    t = RE_TAG.sub(" ", t)
    t = RE_EXPR.sub(" ", t)
    out = []
    for piece in t.split("\n"):
        p = piece.strip()
        if p and not p.isspace():
            out.append(p)
    return out


def css_content(css):
    """Chaînes littérales de `content:` — rendues par ::before/::after, donc du texte."""
    out = []
    for m in RE_CONTENT.finditer(css or ""):
        for lit in re.findall(r'"([^"]*)"|\'([^\']*)\'', m.group(1)):
            v = lit[0] if lit[0] else lit[1]
            if v and v.strip() and v not in ("", " "):
                out.append(v)
    return out


def wrong(lang):
    return STRAIGHT if lang == "fr" or lang not in ("en",) else CURVED


def defects(value, lang):
    """(P, A) — guillemet droit ; apostrophe intra-mot de la mauvaise sorte."""
    p = value.count(DQUOTE)
    w, a = wrong(lang), 0
    for i, ch in enumerate(value):
        if ch == w and (value[i - 1] if i else "").isalpha() \
                and (value[i + 1] if i + 1 < len(value) else "").isalpha():
            a += 1
    return p, a


def scan(path, naif=False):
    d = json.load(open(path, encoding="utf-8"))
    lang = lang_of(path)
    rows = []
    if naif:
        for key in ("mustache", "css"):
            v = d.get(key) or ""
            if v:
                p, a = defects(v, lang)
                rows.append((f"{key} (BRUT)", p, a, len(v)))
        return lang, rows
    tn = text_nodes(d.get("mustache") or "")
    cc = css_content(d.get("css") or "")
    for label, values in (("mustache/texte", tn), ("css/content:", cc)):
        p = sum(defects(v, lang)[0] for v in values)
        a = sum(defects(v, lang)[1] for v in values)
        rows.append((f"{label} ({len(values)} chaîne(s))", p, a, sum(len(v) for v in values)))
    return lang, rows


def all_templates(include_all):
    if include_all:
        out = []
        for root, dirs, files in os.walk(CARDS):
            dirs[:] = [x for x in dirs if x != "Archive"]
            for f in sorted(files):
                if f.endswith(".json"):
                    out.append(os.path.join(root, f))
        return sorted(out)
    return [os.path.join(CARDS, p) for p in CARD_SETS]


def report(include_all, naif):
    total = [0, 0]
    print(f"{'gabarit':52s} {'lang':4s} {'P':>4s} {'A':>4s} {'chars':>8s}")
    print("-" * 78)
    for path in all_templates(include_all):
        rel = os.path.relpath(path, REPO).replace("\\", "/")
        try:
            lang, rows = scan(path, naif)
        except json.JSONDecodeError as e:
            print(f"{rel:52s} !! JSON invalide : {e}")
            continue
        for label, p, a, n in rows:
            total[0] += p
            total[1] += a
            print(f"{rel:52s} {lang:4s} {p:4d} {a:4d} {n:8d}   {label}")
    print("-" * 78)
    print(f"TOTAL  P={total[0]} (guillemet droit)   A={total[1]} (apostrophe de la mauvaise sorte, intra-mot)")
    return total


def self_test():
    cases = [
        ("balisage seul : 0 texte, 0 défaut",
         defects("".join(text_nodes("<div class=\"titre\" data-x='y'>   </div>")), "fr"), (0, 0)),
        ("attribut HTML à guillemets droits : PAS du texte",
         sum(defects(v, "fr")[0] for v in text_nodes("<div class=\"a\" id=\"b\"></div>")), 0),
        ("apostrophe droite dans un nœud de texte (fr) : DÉFAUT",
         defects("l'argument est faux", "fr")[1], 1),
        ("apostrophe courbe dans un nœud de texte (fr) : cible, pas un défaut",
         defects("l’argument est faux", "fr")[1], 0),
        ("apostrophe courbe dans un nœud de texte (en) : DÉFAUT",
         defects("it’s wrong", "en")[1], 1),
        ("guillemet droit dans un nœud de texte : DÉFAUT",
         defects("il dit \"oui\"", "fr")[0], 2),
        ("expression {{…}} : DONNÉE, pas du texte",
         "".join(text_nodes("<div>{{Text}}</div>")), ""),
        ("bloc <style> retiré du texte (le CSS est lu par content:)",
         "".join(text_nodes("<style>a{content:'x'}</style><p>ok</p>")).strip(), "ok"),
        ("content: du CSS : lu",
         css_content("a::before{content:'Famille';}"), ["Famille"]),
        ("TÉMOIN : texte propre",
         (sum(defects(v, "fr")[0] for v in text_nodes("<p>Texte propre, sans marque.</p>")),
          sum(defects(v, "fr")[1] for v in text_nodes("<p>Texte propre, sans marque.</p>"))), (0, 0)),
    ]
    ok = True
    for name, got, want in cases:
        good = got == want
        ok &= good
        print(f"  [{'PASS' if good else 'FAIL'}] {name}" + ("" if good else f" — lu {got!r}, attendu {want!r}"))
    print("SELF-TEST " + ("OK" if ok else "ROUGE"))
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--self-test", action="store_true")
    ap.add_argument("--naif", action="store_true")
    ap.add_argument("--all", action="store_true")
    args = ap.parse_args()
    if args.self_test:
        return self_test()
    mode = "BRUT (contrôle de non-aveuglement)" if args.naif else "dépouillé (nœuds de texte)"
    scope = "TOUT Cards/**/*.json hors Archive/" if args.all else f"{len(CARD_SETS)} gabarits qui alimentent un CardSet"
    print(f"balayage {mode} · périmètre : {scope}")
    print()
    report(args.all, args.naif)
    return 0


if __name__ == "__main__":
    sys.exit(main())
