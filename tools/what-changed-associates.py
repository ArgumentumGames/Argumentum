#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""« Quoi de neuf » pour les associés — générateur (jamais de chiffres gelés).

Grain 3 du pool c.5993735448 (05/10) : quand le paquet régénéré part aux associés,
chacun doit voir, DANS SA LANGUE, quelles cartes ont changé depuis ce qu'il a
relisé (v2.0.0-review, base bundle `89f78bcd`). Le census dérive de ~40
cellules/jour tant que les pools vivent — un document à chiffres fixés serait
périmé avant d'être lu : ce générateur re-mesure à chaque appel.

Méthode (identique au census manuel n°7, mêmes discriminants) :
  git show BASE:csv / HEAD:csv -> parse csv -> diff par pk -> pour chaque
  cellule changée, colonne -> langue. Carte touchée d'une langue = rangée avec
  ≥ 1 cellule de cette langue changée.

Colonnes -> langue, par deck (mesuré sur les en-têtes master 05/10) :
  Fallacies  : suffixe _fr/_en/... (text_fr, desc_en, LTru...) ; sans suffixe
               = colonnes structurelles (PK, carte, nom_vulgarisé) — hors langues.
  Virtues    : idem (title_fr, description_pt...). family_* compte pour sa langue.
  Rules      : Text = fr ; Text_xx = langue.
  Scenarii   : colonnes FR nommées en français (titre, contexte, enjeu,
               suggestion...) = fr ; suffixe _en/_ru/... = langue ; colonnes
               anglaises sans suffixe (title, context, issue, smoothTalker,
               drawer, category, subcategory) = en.

Sortie : Markdown par langue (section de l'associé), deck par deck, cartes
listées avec pk + ancre FR + champs modifiés ; + tableau de reconciliation
census en tête (à croiser avec le census du plan de régén).

Usage :
  python tools/what-changed-associates.py BASE_REF HEAD_REF [--out f.md]
  python tools/what-changed-associates.py --self-test

Codes : 0 = rapport écrit (un périmètre vide est un résultat, pas une erreur) ;
1 = self-test rouge ; 2 = erreur (réf git inconnue, CSV illisible).
"""
import argparse
import csv
import io
import subprocess
import sys
from collections import defaultdict

LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
LANG_LABEL = {"fr": "Français", "en": "English", "ru": "Русский", "pt": "Português",
              "es": "Español", "ar": "العربية", "fa": "فارسی", "zh": "中文"}
# Champs affichés sous leur nom court (le reste : nom de colonne brut)
FIELD_LABEL = {"text": "texte", "desc": "description", "example": "exemple",
               "LT": "titre latin", "L": "ligne", "link": "lien", "title": "titre",
               "description": "description", "remark": "remarque", "Text": "texte",
               "titre": "titre", "contexte": "contexte", "enjeu": "enjeu",
               "suggestion": "suggestion", "context": "contexte", "issue": "enjeu",
               "smoothTalker": "baratineur", "drawer": "piocheur"}

DECKS = [
    {"name": "Sophismes (Tarot + Web)", "file": "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
     "pk": "PK", "card_col": "carte", "anchor": "nom_vulgarisé", "style": "suffix"},
    {"name": "Vertus (Tarot)", "file": "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
     "pk": "pk", "card_col": "card", "anchor": "title_fr", "style": "suffix"},
    {"name": "Scénarios (Poker)", "file": "Cards/Scenarii/Argumentum Scenarii - Cards.csv",
     "pk": "path", "card_col": None, "anchor": "titre", "style": "scenarii"},
    {"name": "Règles (Tarot)", "file": "Cards/Rules/Argumentum Rules - Cards.csv",
     "pk": "pk", "card_col": None, "anchor": "pk", "style": "rules"},
]
# Scenarii : colonnes FR sans suffixe (mesuré, en-tête master 05/10)
SCENARII_FR_EXPLICIT = {"catégorie", "sous-catégorie", "titre", "baratineur", "piocheur",
                        "contexte", "enjeu", "suggestion", "coordonnées"}
SCENARII_EN_EXPLICIT = {"title", "context", "issue", "smoothTalker", "drawer",
                        "category", "subcategory"}


def lang_of_column(deck, col):
    """Langue d'une colonne, ou None si structurelle."""
    low = col.rsplit("_", 1)
    if deck["style"] == "rules":
        if col == "Text":
            return "fr"
        if low[-1] in LANGS and len(low) == 2:
            return low[-1]
        return None
    if deck["style"] == "scenarii":
        if col in SCENARII_FR_EXPLICIT:
            return "fr"
        if col in SCENARII_EN_EXPLICIT:
            return "en"
        if len(low) == 2 and low[-1] in LANGS:
            return low[-1]
        return None
    # suffix (Fallacies/Virtues) : _fr/_en/... (règle census : préfixe fr_/en_/
    # ... accepté aussi) ; sans marque de langue = structurel (LTfr, Lfr115...)
    if len(low) == 2 and low[-1] in LANGS:
        return low[-1]
    if col.split("_", 1)[0] in LANGS:
        return col.split("_", 1)[0]
    return None


def field_label(col, lang):
    base = col[: -len(lang) - 1] if col.endswith("_" + lang) else col
    return FIELD_LABEL.get(base, base or col)


def git_show(ref, path):
    r = subprocess.run(["git", "show", f"{ref}:{path}"], capture_output=True)
    if r.returncode != 0:
        raise SystemExit(f"ERREUR : git show {ref}:{path} -> {r.returncode}\n{r.stderr.decode(errors='replace')[:300]}")
    return r.stdout.decode("utf-8-sig")  # BOM : le CSV Rules en porte un


def parse_rows(text, pk_col):
    rows = {}
    for row in csv.DictReader(io.StringIO(text)):
        pk = (row.get(pk_col) or "").strip()
        if pk:
            rows[pk] = row
    return rows


def diff_deck_rows(base, head, deck):
    """Comparaison pure de deux jeux de rangées déjà lues — AUCUNE lecture git.

    C'est LE chemin de production : `diff_deck` lit les deux révisions puis
    l'appelle, et le `--self-test` l'appelle directement sur ses fixtures. Un
    self-test qui **recopie** cette logique au lieu de l'appeler ne valide que sa
    copie — celle-ci avait divergé sur deux discriminants (`is_card(h) or
    is_card(b)` vs tête seule ; cellules comptées sous la garde vs hors d'elle),
    et son assertion passait *grâce à* l'écart. Mesuré les 08-09/10/2026.
    """
    # cartes = rangées porteuses (discriminant census) dans l'un ou l'autre
    def is_card(row):
        if not deck["card_col"]:
            return True  # Scenarii/Rules : toutes les rangées sont des cartes
        return bool((row.get(deck["card_col"]) or "").strip())
    changed = {}  # lang -> {pk: [champs]}
    cards_touched = defaultdict(set)
    cells = defaultdict(int)
    for pk in sorted(set(base) | set(head), key=str):
        b, h = base.get(pk), head.get(pk)
        if b is None or h is None:
            continue  # ajout/suppression de rangée : hors périmètre texte, à signaler à part
        card = is_card(h) or is_card(b)
        for col in b:
            if col not in h or b[col] == h.get(col):
                continue
            lang = lang_of_column(deck, col)
            if not lang:
                continue
            # Sémantique census : les CELLULES se comptent sur les rangées cartes
            # uniquement (les rangées taxonomiques sans carte ne partent pas au deck).
            if card:
                cells[lang] += 1
                cards_touched[lang].add(pk)
                changed.setdefault(lang, {}).setdefault(pk, []).append(field_label(col, lang))
    return {"cells": dict(cells),
            "cards": {l: sorted(v) for l, v in cards_touched.items()},
            "detail": changed,
            "base_rows": len(base), "head_rows": len(head)}


def diff_deck(ref_base, ref_head, deck):
    """Lit les deux révisions (git) puis délègue la comparaison à `diff_deck_rows`."""
    base = parse_rows(git_show(ref_base, deck["file"]), deck["pk"])
    head = parse_rows(git_show(ref_head, deck["file"]), deck["pk"])
    return diff_deck_rows(base, head, deck)


def self_test():
    """Mutations falsifiantes sur CSV de fixture — aucun dépôt requis pour la logique.

    ⚠️ Les points 2 à 5 exercent **`diff_deck_rows`**, le chemin de production :
    le self-test ne recopie **aucune** logique. Il l'a fait jusqu'au 09/10/2026, et
    sa copie avait divergé sur deux discriminants — ses assertions passaient alors
    *grâce à* l'écart.

    1. CLASSIFICATION : une cellule _es -> espagnol ; une FR explicite Scenarii -> fr ;
       une colonne structurelle (carte/nom_vulgarisé) -> ignorée.
    2. DIFF          : une cellule changée -> exactement 1 carte, 1 langue, 1 champ.
    3. TEMOIN        : rangée intacte -> absente de tout listing.
    4. DISCRIMINANT  : rangée sans `carte` changée -> ni cellule ni carte comptée
                       (sémantique production ; la copie, elle, comptait la cellule).
    5. DISCRIMINANT  : « carte » = porteuse en BASE **ou** en TÊTE — une rangée dont
                       la tête a vidé `carte` reste comptée (la copie lisait la tête seule).
    """
    import tempfile
    import os
    ok = True
    fal = next(d for d in DECKS if d["name"].startswith("Sophismes"))
    sce = next(d for d in DECKS if d["name"].startswith("Scénarios"))

    def assert_(cond, name, detail=""):
        nonlocal ok
        print(f"  {'PASS' if cond else 'FAIL'}  {name}" + (f" -- {detail}" if not cond else ""))
        if not cond:
            ok = False

    print("== 1. classification colonne -> langue")
    assert_(lang_of_column(fal, "text_es") == "es", "Fallacies text_es -> es")
    assert_(lang_of_column(fal, "carte") is None, "Fallacies carte -> structurel")
    assert_(lang_of_column(fal, "nom_vulgarisé") is None, "Fallacies nom_vulgarisé -> structurel")
    assert_(lang_of_column(sce, "contexte") == "fr", "Scenarii contexte -> fr")
    assert_(lang_of_column(sce, "issue") == "en", "Scenarii issue -> en")
    assert_(lang_of_column(sce, "category_ru") == "ru", "Scenarii category_ru -> ru")
    rules = next(d for d in DECKS if d["name"].startswith("Règles"))
    assert_(lang_of_column(rules, "Text") == "fr", "Rules Text -> fr")
    assert_(lang_of_column(rules, "Text_fa") == "fa", "Rules Text_fa -> fa")

    print("== 2-4. diff sur fixtures")
    header = "PK,path,carte,nom_vulgarisé,text_fr,text_es\n"
    base_txt = header + "1,1.1,oui,Paille-man,A1,B1\n2,2.1,oui,Faux Dilemme,A2,B2\n"
    head_txt = (header + "1,1.1,oui,Paille-man,A1,B1-mute\n"      # es muté
                + "2,2.1,oui,Faux Dilemme-meme,A2,B2\n")            # ancre FR mutée = structurel, ignorée
    header_nc = header.replace("oui", "")  # fixture discriminant : rangée sans carte
    base_nc = header_nc + "3,3.1,,NoCard,C1,D1\n"
    head_nc = header_nc + "3,3.1,,NoCard,C1-mute,D1\n"
    with tempfile.TemporaryDirectory() as td:
        paths = {}
        for tag, txt in (("b", base_txt + base_nc.split("\n", 1)[1]), ("h", head_txt + head_nc.split("\n", 1)[1])):
            p = os.path.join(td, f"{tag}.csv")
            with open(p, "w", encoding="utf-8-sig", newline="") as fh:
                fh.write(txt)
            paths[tag] = p
        b = parse_rows(open(paths["b"], encoding="utf-8-sig").read(), fal["pk"])
        h = parse_rows(open(paths["h"], encoding="utf-8-sig").read(), fal["pk"])
        # LE CHEMIN DE PRODUCTION : les fixtures passent par la fonction que le
        # rapport utilise réellement. Recopier la logique ici validerait une copie.
        res = diff_deck_rows(b, h, fal)
        cells, cards, detail = res["cells"], res["cards"], res["detail"]
        # L'assertion qui casse si l'on ressort le comptage des cellules de la garde.
        # La version antérieure tolérait `{"es":1,"fr":1}` en alternative (« ou ») :
        # elle ne pouvait donc PAS échouer sur ce point.
        assert_(cells == {"es": 1},
                "cellules = {'es': 1} seul : rangée carte comptée, rangée SANS carte ignorée",
                f"cells={cells}")
        assert_("1" in cards.get("es", []), "carte 1 listée en es")
        assert_("2" not in cards.get("es", []) and "2" not in cards.get("fr", []), "rangée 2 : ancre FR mutée = structurelle, pas listée")
        assert_("3" not in {p for v in cards.values() for p in v}, "rangée sans carte absente des deux listings")
        assert_(detail.get("es", {}).get("1") == ["texte"], f"champ affiché = 'texte' (lu {detail.get('es', {}).get('1')})")

    print("== 5. discriminant « carte » : porteuse en BASE **ou** en TÊTE")
    hdr = "PK,path,carte,nom_vulgarisé,text_fr,text_es\n"
    b2 = parse_rows(hdr + "4,4.1,oui,Porteuse,C4,D4\n", fal["pk"])
    h2 = parse_rows(hdr + "4,4.1,,Porteuse,C4,D4-mute\n", fal["pk"])
    r2 = diff_deck_rows(b2, h2, fal)
    assert_(r2["cells"] == {"es": 1},
            "cellule comptée : la rangée était porteuse en BASE",
            f"cells={r2['cells']}")
    assert_("4" in r2["cards"].get("es", []),
            "carte 4 listée : porteuse en base, même si la tête a vidé `carte`",
            f"cards={r2['cards']}")

    print("SELF-TEST " + ("PASS" if ok else "FAIL"))
    return ok


def main(argv=None):
    ap = argparse.ArgumentParser(description="« Quoi de neuf » associés — générateur (census vivant)")
    ap.add_argument("base", nargs="?")
    ap.add_argument("head", nargs="?")
    ap.add_argument("--out")
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args(argv)

    if args.self_test:
        return 0 if self_test() else 1
    if not (args.base and args.head):
        ap.error("base et head requis (ex. 89f78bcd origin/master) — ou --self-test")

    results = [diff_deck(args.base, args.head, d) for d in DECKS]
    # ancre FR par deck pour le listing (lue sur head)
    anchors = {}
    for d in DECKS:
        rows = parse_rows(git_show(args.head, d["file"]), d["pk"])
        anchors[d["name"]] = {pk: (row.get(d["anchor"]) or pk).strip() for pk, row in rows.items()}

    lines = ["# Quoi de neuf — cartes dont le texte change dans votre édition", "",
             f"*Mesuré `{args.base} → {args.head}` par `tools/what-changed-associates.py` — "
             f"généré à chaque appel, jamais de chiffres gelés. Ancres = titre FR de la tête.*",
             "", "## Vue d'ensemble (reconciliation census)", "",
             "| Deck | Cellules | Cartes | Par langue (cellules/cartes) |", "|---|---:|---:|---|"]
    total_cells, total_cards = 0, set()
    per_lang_all = defaultdict(lambda: [0, set()])
    for d, res in zip(DECKS, results):
        n_cells = sum(res["cells"].values())
        deck_cards = set()
        for lang, pks in res["cards"].items():
            per_lang_all[lang][0] += res["cells"].get(lang, 0)
            per_lang_all[lang][1].update((d["name"], p) for p in pks)
            deck_cards.update(pks)
        total_cells += n_cells
        per = " · ".join(f"{l} {res['cells'][l]}/{len(res['cards'].get(l, []))}" for l in sorted(res["cells"]))
        lines.append(f"| {d['name']} | {n_cells} | {len(deck_cards)} | {per or '—'} |")
        total_cards.update((d["name"], p) for p in deck_cards)
    lines.append(f"| **TOTAL** | **{total_cells}** | **{len(total_cards)}** | "
                 + " · ".join(f"{l} {per_lang_all[l][0]}/{len(per_lang_all[l][1])}" for l in sorted(per_lang_all)) + " |")
    lines.append("")
    for lang in LANGS:
        sections = [(d, res["detail"].get(lang, {})) for d, res in zip(DECKS, results)
                    if res["detail"].get(lang)]
        if not sections:
            continue
        n = len({(d["name"], pk) for d, det in sections for pk in det})
        lines.append(f"## {LANG_LABEL[lang]} — {n} carte(s) touchée(s)")
        lines.append("")
        for d, det in sections:
            lines.append(f"### {d['name']} ({len(det)} carte(s))")
            lines.append("")
            for pk in sorted(det):
                anchor = anchors[d["name"]].get(pk, pk)
                lines.append(f"- **{anchor}** ({d['pk']} {pk}) : {', '.join(sorted(det[pk]))}")
            lines.append("")
    out = args.out or "quoi-de-neuf-associes.md"
    with open(out, "w", encoding="utf-8", newline="\n") as fh:
        fh.write("\n".join(lines) + "\n")
    print("\n".join(lines[:14]))
    print(f"…\nrapport complet : {out}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
