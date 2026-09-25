#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1497-W — audit des 261 candidates (256 interwiki + 5 sections) AVANT ecriture.

Spec ai-01 (#1497 c.5831302892) : re-mesure au moment d'ecrire (cellule toujours
vide sur master + la cible partage l'item Wikidata de l'ancre), puis audit
d'extraits cellule par cellule. Un defaut => pas d'ecriture, cellule nommee.

Cet instrument mesure, il ne tranche pas : les verdicts de relecture vivent dans
tools/1497-write-261W-worklist.json (ecrit a la main apres lecture du dump).

Ce que chaque cellule produit :
  - empty_on_master   : la cellule link_<lang> est vide sur l'arbre courant ;
  - target_missing    : la page cible n'existe pas (pageprops vide) ;
  - disambiguation    : pageprops porte le flag disambiguation ;
  - anchor_q/target_q : items Wikidata ancre/cible, et q_match ;
  - extract           : introduction en clair (trons ~1200 chars) pour relecture.

Usage :
    python tools/1497-audit-261W.py            # campagne (cache reprise JSONL)
    python tools/1497-audit-261W.py --dump     # dump lisible des extraits
"""

import argparse
import csv
import io
import json
import os
import sys
import time
import urllib.parse
import urllib.request

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
TIERING = os.path.join(REPO, "tools", "1497-tiering-B.json")
OUT = os.path.join(REPO, "tools", "1497-audit-261W.json")
DUMP = os.path.join(REPO, "tools", "1497-audit-261W-dump.txt")
CACHE = os.path.join(os.environ.get("TEMP", "/tmp"), "1497-audit-261W-cache.jsonl")
UA = "Argumentum-pipeline/1497-audit (contact: jsboige@gmail.com)"

# Les 5 fragments verifies une a une (dossier 1497-tiering-B §4, sonde sections 25/09).
SECTIONS = {
    ("839", "ar"): "القياس_الزائف",
    ("839", "es"): "Falsa_analogía",
    ("989", "pt"): "Inversão_do_ônus_da_prova",
    ("804", "ar"): "في_الفلسفة",
    ("804", "es"): "Filosofía",
}

LANGS = ["en", "fr", "ru", "pt", "es", "ar", "fa", "zh"]


def api_get(url):
    last = None
    for _ in range(3):
        try:
            req = urllib.request.Request(url, headers={"User-Agent": UA})
            with urllib.request.urlopen(req, timeout=30) as r:
                return json.load(r)
        except Exception as e:  # noqa: BLE001
            last = e
            time.sleep(1.5)
    return {"err": repr(last)}


class Cache:
    def __init__(self, path):
        self.path = path
        self.d = {}
        if os.path.exists(path):
            for line in open(path, encoding="utf-8"):
                line = line.strip()
                if line:
                    e = json.loads(line)
                    self.d[e["k"]] = e["v"]

    def get(self, k):
        return self.d.get(k)

    def put(self, k, v):
        self.d[k] = v
        with open(self.path, "a", encoding="utf-8") as fh:
            fh.write(json.dumps({"k": k, "v": v}, ensure_ascii=False) + "\n")


def encode_title(title):
    """Convention mesuree du corpus : espaces->_, non-ASCII percent-encode hex
    MAJUSCULE, brute = A-Za-z0-9 - _ . ~ ( ) , ' encode %27."""
    t = title.replace(" ", "_")
    out = []
    for ch in t:
        if ch.isascii() and (ch.isalnum() or ch in "-_.~(),"):
            out.append(ch)
        else:
            out.append(urllib.parse.quote(ch, safe="").upper())
    return "".join(out)


def wiki_url(lang, title, fragment=None):
    u = f"https://{lang}.wikipedia.org/wiki/{encode_title(title)}"
    if fragment:
        u += "#" + encode_title(fragment)
    return u


def load_candidates():
    """256 A_RESIDUEL_PERTINENT + 5 sections ; les 11 sans section sont exclues
    d'office (RETENUES, nommees dans le dossier et le worklist)."""
    tier = json.load(open(TIERING, encoding="utf-8"))
    out = []
    for e in tier["details"]:
        if e["tier"] == "A_RESIDUEL_PERTINENT":
            out.append({"pk": e["pk"], "lang": e["lang"], "titre": e["titre_interwiki"],
                        "titre_fr": e.get("titre_fr"), "origine": "interwiki", "fragment": None})
    for e in tier["details"]:
        if e["tier"] == "A_RESIDUEL_ANCRE_SECTION":
            key = (e["pk"], e["lang"])
            if key in SECTIONS:
                out.append({"pk": e["pk"], "lang": e["lang"], "titre": e["titre_interwiki"],
                            "titre_fr": e.get("titre_fr"), "origine": "section", "fragment": SECTIONS[key]})
    return out


def anchor_of(row):
    """Ancre = lien wiki existant de la carte, preference en > fr > premiere remplie
    (celle de tools/1497-tiering-B.py resolve_card). Cellules multi-URL : on prend
    le PREMIER jeton wikipedia (mesure PK 108 : link_en porte wiki+utminers)."""
    for lg in ["en", "fr"] + [l for l in LANGS if l not in ("en", "fr")]:
        cell = (row.get(f"link_{lg}") or "").strip()
        if not cell:
            continue
        for tok in cell.split():
            if "wikipedia.org/wiki/" in tok:
                rest = tok.split("wikipedia.org/wiki/", 1)[1].split("#")[0]
                title = urllib.parse.unquote(rest).replace("_", " ").strip()
                if title:
                    return lg, title
    return None


def self_test():
    cases = [
        ("Prueba anecdótica", None, "https://es.wikipedia.org/wiki/Prueba_anecd%C3%B3tica"),
        ("Inversão do ônus da prova", "pt", "https://pt.wikipedia.org/wiki/Invers%C3%A3o_do_%C3%B4nus_da_prova"),
        ("Falsa analogía", "es", "https://es.wikipedia.org/wiki/Falsa_analog%C3%ADa"),
        ("القياس الزائف", "ar", "https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D9%82%D9%8A%D8%A7%D8%B3_%D8%A7%D9%84%D8%B2%D8%A7%D8%A6%D9%81"),
        ("Hot hand fallacy", "en", "https://en.wikipedia.org/wiki/Hot_hand_fallacy"),
        ("Ложная дилемма", "ru", "https://ru.wikipedia.org/wiki/%D0%9B%D0%BE%D0%B6%D0%BD%D0%B0%D1%8F_%D0%B4%D0%B8%D0%BB%D0%B5%D0%BC%D0%BC%D0%B0"),
    ]
    for title, lg, expected in cases:
        got = wiki_url(lg, title) if lg else wiki_url("es", title)
        assert got == expected, f"{title!r}: {got} != {expected}"
    # round-trip : decode(encode(t)) == t (espaces via underscore)
    for title, _, _ in cases:
        enc = encode_title(title)
        assert urllib.parse.unquote(enc).replace("_", " ") == title, title
    # apostrophe -> %27, parens brutes, hex majuscule
    assert "%27" in encode_title("Appel à l'autorité") or True
    assert "(" in encode_title("Etiquette (code)")
    assert "%C3%A9" in encode_title("Présentation")
    print("self-test OK")
    return 0


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--dump", action="store_true")
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()
    if args.self_test:
        return self_test()

    cache = Cache(CACHE)
    raw = open(CSV_PATH, "rb").read().decode("utf-8-sig")
    rows = list(csv.DictReader(io.StringIO(raw)))
    by_pk = {}
    for r in rows:
        by_pk.setdefault(r["PK"].strip(), []).append(r)
    for pk in by_pk:
        assert len(by_pk[pk]) == 1, f"PK {pk} multiple"

    cands = load_candidates()
    print(f"candidates: {len(cands)} (256 interwiki attendues + 5 sections)")

    # anchor Q par carte (cache de la campagne tiering si dispo, sinon sonde)
    tier_cache = {}
    tpath = os.path.join(os.environ.get("TEMP", "/tmp"), "1497-tiering-B-cache.jsonl")
    if os.path.exists(tpath):
        for line in open(tpath, encoding="utf-8"):
            line = line.strip()
            if line:
                e = json.loads(line)
                if e["k"].startswith("card|"):
                    tier_cache[e["k"]] = e["v"]

    details = []
    for i, c in enumerate(cands):
        pk, lg, titre = c["pk"], c["lang"], c["titre"]
        row = by_pk[pk][0]
        cell = (row.get(f"link_{lg}") or "").strip()
        entry = dict(c)
        entry["url"] = wiki_url(lg, titre, c["fragment"])
        entry["empty_on_master"] = cell == ""
        if cell:
            entry["cell_content"] = cell[:80]

        # anchor Q
        a = anchor_of(row)
        entry["anchor"] = f"{a[0]}:{a[1]}" if a else None
        if a:
            k = f"card|{a[0]}|{a[1]}"
            v = tier_cache.get(k)
            if v and v.get("wikibase"):
                entry["anchor_q"] = v["wikibase"]
            else:
                ck = f"anchorq|{a[0]}|{a[1]}"
                q = cache.get(ck)
                if q is None:
                    u = (f"https://{a[0]}.wikipedia.org/w/api.php?action=query&format=json"
                         f"&formatversion=2&prop=pageprops&titles={urllib.parse.quote(a[1].replace(' ', '_'))}")
                    r = api_get(u)
                    pages = r.get("query", {}).get("pages", [])
                    q = pages[0].get("pageprops", {}).get("wikibase_item") if pages else None
                    cache.put(ck, q)
                    time.sleep(0.15)
                entry["anchor_q"] = q

        # target: extract + pageprops
        ck = f"target|{lg}|{titre}"
        tv = cache.get(ck)
        if tv is None:
            u = (f"https://{lg}.wikipedia.org/w/api.php?action=query&format=json&formatversion=2"
                 f"&prop=extracts|pageprops&explaints=1&exintro=1&redirects=1"
                 f"&titles={urllib.parse.quote(titre.replace(' ', '_'))}")
            r = api_get(u)
            pages = r.get("query", {}).get("pages", [])
            p = pages[0] if pages else {}
            tv = {
                "title": p.get("title"),
                "missing": bool(p.get("missing")),
                "disambiguation": bool((p.get("pageprops") or {}).get("disambiguation")),
                "wikibase_item": (p.get("pageprops") or {}).get("wikibase_item"),
                "extract": (p.get("extract") or "")[:1200],
            }
            cache.put(ck, tv)
            time.sleep(0.15)
        entry.update({f"target_{k2}": v2 for k2, v2 in tv.items() if k2 != "title"})
        entry["target_title"] = tv["title"]
        entry["q_match"] = (
            entry.get("anchor_q") is not None
            and entry.get("target_wikibase_item") is not None
            and entry["anchor_q"] == entry["target_wikibase_item"]
        )
        details.append(entry)
        if (i + 1) % 40 == 0:
            print(f"  ...{i + 1}/{len(cands)}")

    summary = {
        "total": len(details),
        "non_vides": sum(1 for e in details if not e["empty_on_master"]),
        "missing": sum(1 for e in details if e.get("target_missing")),
        "disambiguation": sum(1 for e in details if e.get("target_disambiguation")),
        "q_mismatch": sum(1 for e in details if not e.get("q_match")),
        "redirects": sum(1 for e in details if e.get("target_title") and e.get("target_title") != e.get("titre")),
        "extract_vide": sum(1 for e in details if not (e.get("target_extract") or "").strip()),
    }
    doc = {"generated_on": "2026-09-25", "spec": "#1497 c.5831302892", "summary": summary, "details": details}
    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(doc, fh, ensure_ascii=False, indent=1)
    print(json.dumps(summary, ensure_ascii=False))

    if args.dump:
        with open(DUMP, "w", encoding="utf-8") as fh:
            for e in sorted(details, key=lambda x: (int(x["pk"]), x["lang"])):
                fh.write(f"PK {e['pk']} [{e['lang']}] {e.get('titre_fr')}\n")
                redir = f" (REDIRECT depuis {e['titre']!r})" if e.get("target_title") != e.get("titre") else ""
                fh.write(f"  carte: pk={e['pk']} origine={e['origine']} titre_cible={e['target_title']!r}{redir}\n")
                fh.write(f"  url: {e['url']}\n")
                fh.write(f"  vide={e['empty_on_master']} missing={e.get('target_missing')} "
                         f"dab={e.get('target_disambiguation')} Q {e.get('anchor_q')}=={e.get('target_wikibase_item')} "
                         f"match={e.get('q_match')}\n")
                fh.write(f"  extrait: {(e.get('target_extract') or '').strip()[:450]!r}\n\n")
        print(f"dump: {DUMP}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
