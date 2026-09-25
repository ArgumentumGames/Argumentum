#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1497 grain 1 — tiering des ~465 cellules B (ARTICLE_PAS_LANGUE) du deck.

Issue #1497 (decision owner 22/09) : les 465 cellules B sont aujourd'hui
INDECIDABLES (« 20-40 h, rendement inconnu »). Ce grain les caracterise en
TRANCHES mesurees, par langue, AVANT tout arbitrage de budget. ⛔ AUCUNE
ECRITURE — mesure seule.

Trois tranches (noms de l'issue) :
  T1_COMPLETABLE_DIRECTEMENT     une page wiki PERTINENTE existe dans la langue
                                 cible malgre l'absence de langlink (page
                                 non-liee, ou titre traduit que le graphe
                                 interwiki ne voit pas). Ecrire = poser l'URL.
  T2_SOURCE_ALTERNATIVE_IDENTIFIEE  aucune page pertinente dans la langue, MAIS
                                 au moins un article de LANGUE PROCHE existe
                                 (romane<->romane, ru<->slave, fa<->ar...) :
                                 la lacune est ciblee, la traduction a une
                                 source identifiee — arbitrage owner.
  T3_LACUNE_IRREDUCTIBLE         aucun voisin linguistique. Sous-split :
                                 sujet_mince (article présent dans <=2 langues
                                 au total) vs sujet_etabli_hors_famille (>2) —
                                 une lacune sur un sujet etabli est plus
                                 « réparable » qu'une lacune sur un sujet que
                                 personne n'a encyclopédisé.

METHODE (leçon du grain ⑤ : un hit n'est pas une cible) :
  Par CARTE : 1 appel langlinks+pageprops sur l'ancre (preferee en, puis fr)
    -> ensemble exists_in (langues ou l'article existe selon l'interwiki)
  Par CELLULE B : 2 sondes —
    (a) existence exacte du TITRE D'ANCRE dans le wiki cible ;
    (b) recherche native srsearch=text_<lang>, top-1.
    Chaque page trouvee est CLASSEE (heuristique stricte v2 du grain ⑤,
    reprise telle quelle) : seule une page PERTINENTE ouvre la tranche T1.
    Une page existante mais HORS_SUJET (page-liste, homonymie — l'exact defaut
    mesure sur PK 128 ru) est comptee et LAISSEE : elle n'est pas une cible.

CONTROLES INVERSES (DoD de l'issue) :
  - 3 titres inventes -> 0 hit de recherche chacun (l'instrument peut rendre 0) ;
  - sonde positive (fr 'Sophisme' -> PERTINENT) et negative (en 'Infinity
    Within', album -> HORS_SUJET) sur le classement de pertinence.

RESUME/CACHE : chaque sonde est checkpointee dans --cache (JSONL, reprenable) ;
le resultat final va dans --json-out. Aucune ecriture dans le CSV.

Usage :
    python tools/1497-tiering-B.py --limit 5        # validation sur 5 cartes
    python tools/1497-tiering-B.py                  # passe complete
    python tools/1497-tiering-B.py --json-out tools/1497-tiering-B.json
"""

import argparse
import collections
import csv
import json
import os
import re
import sys
import time
import urllib.parse
import urllib.request

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
LANGS = ("ar", "en", "es", "fa", "fr", "pt", "ru", "zh")
UA = "Argumentum-AssetConverter/2.0 (https://argumentum.games; jsboige@gmail.com) 1497-tieringB"
WIKI_RE = re.compile(r"^https?://([a-z\-]+)\.(?:m\.)?wikipedia\.org/wiki/(.+)$")

# Familles linguistiques (cible -> codes wiki voisins). La cible elle-meme y
# figure : la couverture se compte sur la famille PRIVEE de la cible.
FAMILY = {
    "fr": ["fr", "es", "pt", "it", "ca", "ro", "oc"],
    "es": ["es", "fr", "pt", "it", "ca", "ro", "oc"],
    "pt": ["pt", "es", "fr", "it", "ca", "ro", "oc"],
    "ru": ["ru", "uk", "be", "bg", "sr", "sh", "cs", "sk", "sl", "mk"],
    "fa": ["fa", "ar", "az", "ckb"],
    "ar": ["ar", "fa", "he", "ur", "ckb"],
    "zh": ["zh", "ja", "ko", "yue"],
    "en": ["en", "de", "nl", "sv", "da", "no", "is"],
}

LOGIC_FALLACY_TERMS = (
    "fallacy", "fallacies", "falacia", "falacias", "sophisme", "sofisma",
    "sophism", "argumentum", "argumento",
    "erroneous reasoning", "erreur de raisonnement", "raisonnement invalide",
    "invalid argument", "invalid reasoning", "mauvais raisonnement",
    "biais cognitif", "cognitive bias", "biais de raisonnement",
    "sophistical", "fallacieux",
    "софизм", "ошибка", "заблуждение", "логическая ошибка", "многословие",
    "مغالطه", "سفسطه", "مغالطه‌", "سفسطه‌",
    "谬误", "謬誤", "詭辯",
)
FALLACY_TITLE_TERMS = (
    "fallacy", "fallacies", "falacia", "sophism", "sofisma",
    "argumentum", "sophisme", "مغالطه", "سفسطه", "谬误", "謬誤", "софизм",
)


def api_get(host, **params):
    params.update({"action": "query", "format": "json", "formatversion": "2"})
    url = f"https://{host}/w/api.php?" + urllib.parse.urlencode(params)
    for attempt in range(3):
        try:
            req = urllib.request.Request(url, headers={"User-Agent": UA})
            with urllib.request.urlopen(req, timeout=20) as r:
                return json.loads(r.read().decode("utf-8")), None
        except Exception as e:
            if attempt == 2:
                return None, f"ERR_{type(e).__name__}"
            time.sleep(2 * (attempt + 1))
    return None, "ERR_EXHAUSTED"


def normalize(s):
    s = (s or "").lower()
    repl = str.maketrans({
        "à": "a", "â": "a", "ä": "a", "á": "a",
        "è": "e", "ê": "e", "ë": "e", "é": "e",
        "î": "i", "ï": "i", "í": "i",
        "ô": "o", "ö": "o", "ó": "o",
        "ù": "u", "û": "u", "ü": "u", "ú": "u",
        "ç": "c", "ñ": "n", "ß": "ss",
    })
    return s.translate(repl)


def classify(extract, wikibase, top_title):
    """Heuristique stricte v2 du grain ⑤ — reprise telle quelle."""
    if not extract or len(extract) < 30:
        return "HOMONYMIE"
    if not wikibase:
        return "HOMONYMIE"
    norm = normalize(extract)
    title_norm = normalize(top_title)
    extract_has_fallacy = any(t in norm for t in LOGIC_FALLACY_TERMS)
    title_is_fallacy = any(t in title_norm for t in FALLACY_TITLE_TERMS)
    starts_with_fallacy = norm.startswith((
        "a fallacy", "the fallacy", "es una falacia", "é uma falácia",
        "un sophisme", "le sophisme", "словосочетание", "софизм",
        "это логическая"))
    if extract_has_fallacy or title_is_fallacy or starts_with_fallacy:
        return "PERTINENT"
    return "HORS_SUJET"


def read_deck():
    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    header = rows[0]
    idx = {h: i for i, h in enumerate(header)}
    deck = []
    for r in rows[1:]:
        if len(r) > idx["carte"] and r[idx["carte"]].strip():
            deck.append(r)
    return header, idx, deck


def wiki_titles_per_card(row, idx):
    out = []
    for lg in LANGS:
        val = row[idx[f"link_{lg}"]] if idx.get(f"link_{lg}") is not None else ""
        if not val:
            continue
        for url in re.split(r"\s+", val.strip()):
            m = WIKI_RE.match(url)
            if m:
                raw = urllib.parse.unquote(m.group(2))
                title = raw.split("#")[0]
                if title:
                    out.append((m.group(1), title.replace("_", " "), "#" in raw))
    return out


class Cache:
    def __init__(self, path):
        self.path = path
        self.data = {}
        if path and os.path.exists(path):
            with open(path, encoding="utf-8") as fh:
                for line in fh:
                    line = line.strip()
                    if not line:
                        continue
                    rec = json.loads(line)
                    self.data[rec["k"]] = rec["v"]
        self.fh = open(path, "a", encoding="utf-8") if path else None

    def get(self, k):
        return self.data.get(k)

    def put(self, k, v):
        self.data[k] = v
        if self.fh:
            self.fh.write(json.dumps({"k": k, "v": v}, ensure_ascii=False) + "\n")
            self.fh.flush()


def resolve_card(anchors, cache):
    """1 appel : langlinks + pageprops de l'ancre preferentielle (en, fr, autre).
    Retour : dict(wikibase, exists_in=[codes wiki], target_titles={lang: titre})."""
    order = {"en": 0, "fr": 1}
    anchor = sorted(anchors, key=lambda a: order.get(a[0], 2))[0]
    alang, atitle, afrag = anchor
    key = f"card|{alang}|{atitle}"
    hit = cache.get(key)
    if hit is not None:
        return hit
    d, err = api_get(f"{alang}.wikipedia.org", prop="langlinks|pageprops",
                     titles=atitle, lllimit="max", redirects=1)
    if err or not d.get("query", {}).get("pages"):
        res = {"wikibase": None, "exists_in": [alang], "target_titles": {},
               "anchor": [alang, atitle], "anchor_fragment": afrag, "err": err}
    else:
        p = d["query"]["pages"][0]
        wikibase = (p.get("pageprops") or {}).get("wikibase_item")
        ll = p.get("langlinks") or []
        exists_in = sorted({alang} | {l["lang"] for l in ll})
        titles = {l["lang"]: l["title"] for l in ll}
        res = {"wikibase": wikibase, "exists_in": exists_in,
               "target_titles": titles, "anchor": [alang, atitle],
               "anchor_fragment": afrag, "err": None}
    cache.put(key, res)
    time.sleep(0.3)
    return res


def classify_identity(extract, wikibase, top_title, expect_q):
    """Classement IDENTITE D'ABORD : la page partage l'item Wikidata de l'ancre
    (que le corpus utilise deja pour une autre langue) => meme sujet, pertinent
    par construction — un article de sujet (ex. 'evidencia anedotica') ne
    s'autodecrit pas forcement comme un sophisme, l'heuristique mot-cle du
    grain ⑤ le rejetait a tort. Q different => sujet different, hors sujet
    (cas vivant : la page-liste ru Q87248 pour l'ancre Q2861095). Q absent de
    la page ou de l'ancre : repli sur l'heuristique mot-cle."""
    if expect_q and wikibase:
        return "PERTINENT_IDENTITE" if wikibase == expect_q else "Q_DIFFERENT"
    return classify(extract, wikibase, top_title)


def probe_exact(lang, title, cache, expect_q=None):
    """(a) la page nommee <title> existe-t-elle dans le wiki cible ? + classement."""
    key = f"exact|{lang}|{title}"
    hit = cache.get(key)
    if hit is not None:
        return hit
    d, err = api_get(f"{lang}.wikipedia.org", prop="extracts|pageprops",
                     titles=title, explaintext=1, exintro=1, redirects=1)
    if err:
        res = {"exists": None, "class": "ERREUR_API", "wikibase": None, "err": err}
    else:
        p = d["query"]["pages"][0]
        missing = p.get("missing", False)
        extract = (p.get("extract") or "").strip()[:500]
        wikibase = (p.get("pageprops") or {}).get("wikibase_item")
        if missing:
            res = {"exists": False, "class": "ABSENT", "wikibase": None, "err": None}
        else:
            res = {"exists": True,
                   "class": classify_identity(extract, wikibase, title, expect_q),
                   "wikibase": wikibase, "err": None,
                   "extract_preview": extract[:200]}
    cache.put(key, res)
    time.sleep(0.3)
    return res


def probe_search(lang, query, cache, expect_q=None):
    """(b) recherche native top-1 + classement de pertinence du hit."""
    key = f"search|{lang}|{query}"
    hit = cache.get(key)
    if hit is not None:
        return hit
    d, err = api_get(f"{lang}.wikipedia.org", list="search", srsearch=query,
                     srlimit=3, srprop="wordcount")
    if err:
        res = {"n_hits": None, "top": None, "class": "ERREUR_API", "err": err}
        cache.put(key, res)
        time.sleep(0.3)
        return res
    hits = d["query"]["search"]
    res = {"n_hits": len(hits), "hits": [h["title"] for h in hits], "err": None}
    if hits:
        top = hits[0]["title"]
        d2, err2 = api_get(f"{lang}.wikipedia.org", prop="extracts|pageprops",
                           titles=top, explaintext=1, exintro=1, redirects=1)
        if err2:
            res.update({"top": top, "class": "ERREUR_API"})
        else:
            p = d2["query"]["pages"][0]
            extract = (p.get("extract") or "").strip()[:500]
            wikibase = (p.get("pageprops") or {}).get("wikibase_item")
            res.update({"top": top,
                        "class": classify_identity(extract, wikibase, top, expect_q),
                        "wikibase": wikibase, "extract_preview": extract[:200]})
    else:
        res.update({"top": None, "class": "ZERO_HIT"})
    cache.put(key, res)
    time.sleep(0.3)
    return res


INVENTED_TITLES = [
    ("fr", "Zorblaxion infundibulaire du troisième ordre"),
    ("ru", "Мнемозинальный дрейф кактуса"),
    ("ar", "قياس الزبركانة الشمالية الغربية"),
]
POSITIVE_CONTROL = ("fr", "Sophisme")
NEGATIVE_CONTROL = ("en", "Infinity Within")


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--limit", type=int, default=0, help="limite de cartes (0=toutes)")
    ap.add_argument("--cache", default=os.path.join(
        os.environ.get("TEMP", "/tmp"), "1497-tiering-B-cache.jsonl"))
    ap.add_argument("--json-out")
    a = ap.parse_args()

    print("=== #1497 grain 1 — tiering des cellules B (mesure, 0 ecriture) ===", flush=True)
    header, idx, deck = read_deck()
    print(f"deck : {len(deck)} cartes (colonne 'carte' non vide)", flush=True)

    cache = Cache(a.cache)
    print(f"cache : {a.cache} ({len(cache.data)} sondes deja en cache)", flush=True)

    # ---- Controles inverses (avant tout) -------------------------------
    print("\n--- Controles inverses ---", flush=True)
    inv_ok = 0
    for lg, title in INVENTED_TITLES:
        r = probe_search(lg, title, cache)
        ok = (r["n_hits"] == 0)
        inv_ok += ok
        print(f"  invente {lg} {title[:40]!r} -> {r['n_hits']} hit(s) "
              f"{'OK' if ok else 'ECHEC'}", flush=True)
    pc = probe_exact(*POSITIVE_CONTROL, cache)
    nc = probe_exact(*NEGATIVE_CONTROL, cache)
    pos_ok = pc["class"] == "PERTINENT"
    neg_ok = nc["class"] == "HORS_SUJET"
    print(f"  positif fr 'Sophisme' -> {pc['class']} {'OK' if pos_ok else 'ECHEC'}", flush=True)
    print(f"  negatif en 'Infinity Within' -> {nc['class']} {'OK' if neg_ok else 'ECHEC'}",
          flush=True)
    if inv_ok < len(INVENTED_TITLES) or not pos_ok or not neg_ok:
        print("  ⛔ CONTROLES ECHOUENT — mesure non fiable", flush=True)
        return 2

    # ---- Passe principale ----------------------------------------------
    cards = deck if not a.limit else deck[:a.limit]
    details = []
    counters = collections.Counter()
    by_lang_tier = collections.Counter()
    for ci, row in enumerate(cards, 1):
        pk = row[idx["PK"]].strip()
        anchors = wiki_titles_per_card(row, idx)
        if not anchors:
            continue  # classe E : hors perimetre #1497
        card_info = resolve_card(anchors, cache)
        for lg in LANGS:
            if row[idx[f"link_{lg}"]].strip():
                continue  # cellule deja remplie
            if lg in card_info["exists_in"]:
                # l'article existe via interwiki -> A, pas B. Langlinks =
                # meme item Wikidata que l'ancre (que le corpus utilise deja
                # pour une autre langue) : sujet identique par construction.
                # Exception nommee : ancre a fragment '#' (cas PK 839 — l'ancre
                # en vise une SECTION, l'interwiki cible l'article general) ->
                # verification manuelle de section, comptee a part.
                target = card_info["target_titles"].get(lg)
                if card_info.get("anchor_fragment"):
                    sub = "A_RESIDUEL_ANCRE_SECTION"
                else:
                    sub = "A_RESIDUEL_PERTINENT"
                counters[sub] += 1
                details.append({"pk": pk, "lang": lg, "tier": sub,
                                "titre_interwiki": target})
                continue
            # cellule B apparente : deux sondes de completabilite
            q = card_info["wikibase"]
            exact = probe_exact(lg, card_info["anchor"][1], cache, expect_q=q)
            native = row[idx[f"text_{lg}"]].strip() if idx.get(f"text_{lg}") is not None else ""
            search = probe_search(lg, native, cache, expect_q=q) if native else \
                {"class": "NON_QUERABLE", "n_hits": None, "top": None}
            pertinent = None
            for probe in (exact, search):
                if probe.get("exists") and probe.get("class") in ("PERTINENT",
                                                                  "PERTINENT_IDENTITE"):
                    pertinent = {"via": "titre exact d'ancre",
                                 "titre": card_info["anchor"][1]} if probe is exact else \
                                {"via": "recherche native top-1", "titre": probe.get("top")}
                    break
            fam = [c for c in FAMILY[lg] if c != lg and c in card_info["exists_in"]]
            if pertinent:
                tier = "T1_COMPLETABLE_DIRECTEMENT"
            elif fam:
                tier = "T2_SOURCE_ALTERNATIVE_IDENTIFIEE"
            elif len(card_info["exists_in"]) > 2:
                tier = "T3_LACUNE_IRREDUCTIBLE_sujet_etabli"
            else:
                tier = "T3_LACUNE_IRREDUCTIBLE_sujet_mince"
            base_tier = tier.rsplit("_", 1)[0] if tier.startswith("T3") else tier
            counters[tier] += 1
            by_lang_tier[(lg, base_tier)] += 1
            details.append({
                "pk": pk, "lang": lg, "titre_fr": row[idx["text_fr"]][:60],
                "tier": tier,
                "exists_in": card_info["exists_in"],
                "famille_presents": fam,
                "exact_class": exact.get("class"),
                "search_class": search.get("class"), "search_top": search.get("top"),
                "pertinent": pertinent,
            })
        if ci % 10 == 0 or ci == len(cards):
            print(f"  [{ci:3}/{len(cards)}] cartes traitees", flush=True)

    # ---- Synthese --------------------------------------------------------
    total = sum(v for k, v in counters.items() if k.startswith(("T1", "T2", "T3")))
    a_res = counters.get("A_RESIDUEL_PERTINENT", 0) + counters.get("A_RESIDUEL_ANCRE_SECTION", 0)
    print(f"\nCellules B caracterisees : {total} (+{a_res} A residuels interwiki, hors perimetre B)")
    for k in sorted(counters):
        print(f"  {k:<45} : {counters[k]:>4}")
    print("\nPar langue (T1/T2/T3) :")
    for lg in LANGS:
        t1 = by_lang_tier.get((lg, "T1_COMPLETABLE_DIRECTEMENT"), 0)
        t2 = by_lang_tier.get((lg, "T2_SOURCE_ALTERNATIVE_IDENTIFIEE"), 0)
        t3 = sum(v for (l, t), v in by_lang_tier.items() if l == lg and t.startswith("T3"))
        if t1 + t2 + t3:
            print(f"  {lg:<4} : T1={t1:>3}  T2={t2:>3}  T3={t3:>3}   (total {t1+t2+t3})")

    if a.json_out:
        with open(a.json_out, "w", encoding="utf-8") as f:
            json.dump({
                "total_B": total,
                "counters": dict(counters),
                "by_lang": {f"{l}_{t}": v for (l, t), v in by_lang_tier.items()},
                "controls": {
                    "invented": [{"lang": lg, "title": t,
                                  "n_hits": probe_search(lg, t, cache)["n_hits"]}
                                 for lg, t in INVENTED_TITLES],
                    "positive": {"probe": POSITIVE_CONTROL, "class": pc["class"]},
                    "negative": {"probe": NEGATIVE_CONTROL, "class": nc["class"]},
                },
                "details": details,
            }, f, ensure_ascii=False, indent=1)
        print(f"\nResultat -> {a.json_out}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
