#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1499 grain 4 — PK 636 « Sophisme de régression » : cohérence terminologique.

Dispatch pool v16 grain 4 ([#458 c.5842223755](...)) : « Pour chaque langue : le terme
clé (retour à la moyenne / régression) est-il celui qu'emploient **le titre de la même
carte** et **ses sœurs de famille** ? »

⛔ MESURE SEULE. Aucune écriture CSV, aucun appel API, aucune régénération.

Ce que l'instrument mesure, et rien de plus :

  §A  PK 636, les 8 langues : quelles familles de termes porte le TITRE (`text_*`),
      quelles familles porte la DESCRIPTION (`desc_*`). Deux familles :
        REG  — « régression » et ses cognats/emprunts ;
        MEAN — « retour à la moyenne » (la moyenne nommée).
  §B  la fratrie : `Soussousfamille` de PK 636 (« Relation infondée », 11 cartes) —
      qui, dans le groupe, emploie REG ou MEAN, et où.
  §C  la PRATIQUE DU DECK, mesurée (pas supposée) : sur les 175 cartes, quelle part
      des `desc_*` reprend un mot du titre de la même carte ? Sans ce chiffre, on ne
      peut pas dire si une desc qui ne reprend pas son titre est un écart ou la norme.

⚠️ LIMITES DÉCLARÉES (elles bornent ce que les chiffres peuvent établir) :
  · `en` — « mean » est ambigu (nom « moyenne » / verbe « signifier ») : MEAN_en
    cherche `average`, jamais `mean`. Une desc qui dirait « the mean » n'est PAS vue.
  · `pt` — `média` est ambigu (« moyenne » / « médias », singulier de media) ;
    `es` — `media` idem. Les occurrences sont donc **imprimées avec leur phrase**
    pour lecture, jamais comptées seules.
  · §C compare des MOTS, pas des sens : c'est une borne basse de la reprise
    terminologique (une desc peut dire la même chose sans réutiliser le mot).

Usage :
    python tools/1499-g636-coherence-termino.py              # §A §B §C
    python tools/1499-g636-coherence-termino.py --self-test   # cas littéraux
"""
import argparse
import csv
import io
import os
import re
import sys
import unicodedata

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_REL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
PK = "636"
GROUP = {"Famille": "Erreur mathématique", "Sous-Famille": "Mauvaise interprétation",
         "Soussousfamille": "Relation infondée"}

# ── deux familles de termes, une par langue ───────────────────────────────────
REG = {
    "fr": [r"régression"], "en": [r"regression"], "ru": [r"регресс"],
    "pt": [r"regress"], "es": [r"regres"], "ar": [r"انحدار"],
    "fa": [r"رگرسیون"], "zh": [r"回归"],
}
MEAN = {
    "fr": [r"moyenne"], "en": [r"average"], "ru": [r"средн"],
    "pt": [r"m[ée]dia"], "es": [r"media", r"promedio"], "ar": [r"متوسط"],
    "fa": [r"میانگین"], "zh": [r"平均"],
}
FAMILIES = {"REG": REG, "MEAN": MEAN}


def load():
    path = os.path.join(REPO, CSV_REL)
    return list(csv.DictReader(io.StringIO(open(path, "rb").read().decode("utf-8-sig"))))


def fams(lang, text):
    """Familles de termes présentes dans `text` pour `lang`."""
    if not text:
        return []
    low = text.lower()
    out = []
    for name, pats in FAMILIES.items():
        if any(re.search(p, low) for p in pats[lang]):
            out.append(name)
    return out


def occ(lang, text, fam):
    """Occurrences citées (famille, terme, extrait) — pour lecture, jamais comptées seules."""
    if not text:
        return []
    low = text.lower()
    out = []
    for p in FAMILIES[fam][lang]:
        for m in re.finditer(p, low):
            s = max(0, m.start() - 34)
            e = min(len(text), m.end() + 34)
            out.append((m.group(0), ("…" if s else "") + text[s:e].replace("\n", " ") + ("…" if e < len(text) else "")))
    return out


STOP = set("""les des une dans pour avec sans que qui quoi dont vers entre mais comme plus moins tout tous
toute toutes est sont être avoir fait faire cela celui celle ceux aux par sur son sa ses leur leurs
this that with from into which whose their there where when what have been being does done""".split())


def tokens(text, lang):
    """Mots porteurs : ≥5 signes pour les écritures à mots, ≥2 idéogrammes pour le zh."""
    if not text:
        return set()
    out = set()
    for w in re.findall(r"[^\W\d_]+", text, flags=re.UNICODE):
        wl = w.lower()
        if wl in STOP:
            continue
        cjk = any("一" <= ch <= "鿿" for ch in wl)
        if (cjk and len(wl) >= 2) or (not cjk and len(wl) >= 5):
            out.add(wl)
    return out


def baseline(rows, lang):
    """§C : part des `desc_<lang>` qui reprennent ≥1 mot (≥5) du titre de la même carte."""
    cards = [r for r in rows if (r.get("carte") or "").strip()]
    n = len(cards)
    hits = 0
    for r in cards:
        t = tokens(r.get("text_" + lang, ""), lang)
        d = tokens(r.get("desc_" + lang, ""), lang)
        if t & d:
            hits += 1
    return hits, n


def group_of(rows):
    return [r for r in rows if all((r.get(k) or "").strip() == v for k, v in GROUP.items())]


def subfamily_of(rows):
    """Les sœurs de SOUS-FAMILLE — c'est là que deux sens de « régression » peuvent coexister."""
    return [r for r in rows
            if (r.get("Famille") or "").strip() == GROUP["Famille"]
            and (r.get("Sous-Famille") or "").strip() == GROUP["Sous-Famille"]]


def report(rows, out=print):
    out("═" * 78)
    out("§A — PK 636, titre vs description")
    out("═" * 78)
    r = next(x for x in rows if (x.get("PK") or "").strip() == PK)
    for lang in LANGS:
        t = ("text_" + lang)
        d = ("desc_" + lang)
        tf, df = fams(lang, r[t]), fams(lang, r[d])
        out(f"  {lang:3s} titre  {'+'.join(tf) if tf else '—':9s} {r[t]}")
        out(f"      desc   {'+'.join(df) if df else '—':9s} {r[d][:110]}")
        for f in df:
            for term, ctx in occ(lang, r[d], f):
                out(f"        · {f} {term!r} : {ctx}")
    out("")
    out("═" * 78)
    out(f"§B — fratrie `{GROUP['Soussousfamille']}` ({len(group_of(rows))} cartes)")
    out("═" * 78)
    grp = group_of(rows)
    for lang in LANGS:
        out(f"  ── {lang}")
        for x in sorted(grp, key=lambda y: int(y["PK"])):
            tf, df = fams(lang, x.get("text_" + lang, "")), fams(lang, x.get("desc_" + lang, ""))
            if tf or df:
                mark = "  <— PK 636" if x["PK"] == PK else ""
                out(f"     PK {x['PK']:>4s} titre {','.join(tf) or '—':9s} desc {','.join(df) or '—':9s} "
                    f"| {x.get('text_' + lang, '')[:60]}{mark}")
        n_t = sum(1 for x in grp if fams(lang, x.get("text_" + lang, "")))
        n_d = sum(1 for x in grp if fams(lang, x.get("desc_" + lang, "")))
        out(f"     → {n_t}/{len(grp)} titres et {n_d}/{len(grp)} descs portent REG ou MEAN")
    out("")
    out("═" * 78)
    sf = subfamily_of(rows)
    out(f"§D — collision de sens : REG dans la SOUS-famille `{GROUP['Sous-Famille']}` "
        f"({len(sf)} cartes)")
    out("═" * 78)
    out("  Hors PK 636, REG y désigne-t-il « régression vers la moyenne » ou le *regressus*")
    out("  de l'infini (« régression infinie ») ? — deux concepts, un même mot.")
    for lang in LANGS:
        others = [x for x in sorted(sf, key=lambda y: int(y["PK"]))
                  if x["PK"] != PK and fams(lang, x.get("text_" + lang, ""))]
        if not others:
            out(f"  {lang:3s} — aucune autre carte de la sous-famille ne porte REG")
            continue
        out(f"  {lang:3s} {len(others)} carte(s) hors 636 :")
        for x in others:
            where = x.get("Soussousfamille") or "(pas de sous-sous-famille)"
            out(f"       PK {x['PK']:>4s} [{where}] {x.get('text_' + lang, '')}")
    out("")
    out("═" * 78)
    out("§C — pratique du deck : `desc` reprend-elle un mot du titre ? (175 cartes)")
    out("═" * 78)
    for lang in LANGS:
        h, n = baseline(rows, lang)
        out(f"  {lang:3s} {h:3d}/{n} = {100.0 * h / n:5.1f} %")
    out("")
    out("  (borne basse : compare des MOTS, pas des sens ; le zh ne partage pas de")
    out("   radical latin, et une desc peut dire la même chose sans réutiliser le mot.)")


def self_test():
    cases = []

    def chk(name, got, want):
        cases.append((name, got == want, got, want))

    # (1) titre REG, desc sans aucun terme — le cas de PK 636 en fr
    chk("fr : titre REG, desc sans terme", fams("fr", "Sophisme de régression"), ["REG"])
    chk("fr : desc 2022 ne porte aucun terme",
        fams("fr", "Vous attribuez une cause erronée à ce qui n'est que le résultat d'une fluctuation tout à fait normale."), [])
    # (2) titre REG, desc qui DÉFINIT par l'autre famille — le cas en
    chk("en : desc porte MEAN ('average')",
        fams("en", "Attributing a specific cause to the natural return from an extreme to the average."), ["MEAN"])
    chk("en : 'mean' seul n'est PAS compté (limite déclarée)",
        fams("en", "Regression fallacy, the mean of a sample"), ["REG"])
    # (3) témoin : une desc qui reprend le terme du titre
    chk("fr : desc qui reprend le titre", fams("fr", "La régression vers la moyenne est mal comprise."), ["REG", "MEAN"])
    # (4) fa : titre MEAN, desc sans terme
    chk("fa : titre MEAN", fams("fa", "مغالطهٔ بازگشت به میانگین"), ["MEAN"])
    chk("fa : desc sans terme", fams("fa", "شما علتی نادرست را به چیزی نسبت می‌دهید که صرفاً نتیجه یک نوسان کاملاً طبیعی است."), [])
    # (5) ar : titre REG+MEAN
    chk("ar : titre REG+MEAN", fams("ar", "مغالطة الانحدار نحو المتوسط"), ["REG", "MEAN"])
    # (6) zh
    chk("zh : titre REG", fams("zh", "回归谬误"), ["REG"])
    chk("zh : desc sans terme", fams("zh", "您把某种错误的原因归因于仅仅是完全正常波动所造成的结果。"), [])
    # (7) faux positif possible : 'média' en pt hors sens « moyenne »
    chk("pt : 'média' nu compte (ambiguïté déclarée)", fams("pt", "Os média falam disso"), ["MEAN"])
    # (8) la famille du voisin « Infini trompeur » : régression au sens de regressus
    chk("fr : 'régression infinie' compte comme REG (collision de sens)",
        fams("fr", "Mauvais argument de la régression infinie"), ["REG"])

    ok = True
    for name, good, got, want in cases:
        ok &= good
        print(f"  [{'PASS' if good else 'FAIL'}] {name}" + ("" if good else f" — lu {got!r}, attendu {want!r}"))
    print("SELF-TEST " + ("OK" if ok else "ROUGE"))
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()
    if args.self_test:
        return self_test()
    rows = load()
    print(f"deck : {len(rows)} lignes · cartes marquées : "
          f"{sum(1 for r in rows if (r.get('carte') or '').strip())}")
    report(rows)
    return 0


if __name__ == "__main__":
    sys.exit(main())
