#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Epic #1471 grain 1 — rendement REEL de l'API langlinks.

Deux modes :
  --calibrate   local seul : reproduire les chiffres d'ai-01 (112/72/23 wikipedia,
                125 cartes ancrees, 728 visees). Si ca ne colle pas, l'instrument
                est faux et le rendement qu'il rendra ne vaut rien.
  --measure     reseau : interroge l'API par lots de 50 titres, par langue source.

AUCUNE ecriture CSV, aucun fichier du corpus touche.
"""
import argparse
import importlib.util
import json
import os
import re
import sys
import time
import urllib.parse
import urllib.request

WT = r"D:\Dev\Argumentum"
FAL = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
UA = "Argumentum-AssetConverter/2.0 (https://argumentum.games; jsboige@gmail.com) langlinks-yield-probe"

spec = importlib.util.spec_from_file_location(
    "instr", os.path.join(WT, "tools", "994-apostrophe-dryrun.py"))
instr = importlib.util.module_from_spec(spec)
spec.loader.exec_module(instr)

WIKI_RE = re.compile(r"^https?://([a-z\-]+)\.(?:m\.)?wikipedia\.org/wiki/(.+)$")


def split_urls(cell):
    """Une cellule peut porter PLUSIEURS URLs (separees par un retour a la ligne).

    Mesure : PK 108 `link_en` = une URL wikipedia + une URL utep.edu. Un regex
    ancre sur la cellule entiere la rejette => 111 au lieu de 112. Corrige.
    """
    if not cell:
        return []
    return [u for u in re.split(r"[\s]+", cell.strip()) if u]


def parse_wiki(url):
    """-> (lang, titre) si l'URL est un article wikipedia, sinon None."""
    m = WIKI_RE.match(url.strip())
    if not m:
        return None
    lang, raw = m.group(1), m.group(2)
    raw = raw.split("#")[0]
    title = urllib.parse.unquote(raw).replace("_", " ").strip()
    if not title:
        return None
    return (lang, title)


def parse_wikis(cell):
    """Toutes les ancres wikipedia d'une cellule (0, 1 ou plusieurs)."""
    out = []
    for u in split_urls(cell):
        w = parse_wiki(u)
        if w:
            out.append(w)
    return out


def load_deck():
    corp = instr.load(WT, FAL)
    cols = {c: i for i, c in enumerate(corp["header"])}
    # ETALONNAGE : les colonnes doivent EXISTER. Un nom tape au lieu d'etre
    # derive de l'en-tete a deja effondre une mesure (cf. #1471).
    for need in ["PK", "carte", "link_fr", "link_en"] + [f"link_{l}" for l in LANGS]:
        if need not in cols:
            sys.exit(f"COLONNE ABSENTE : {need!r} — instrument invalide")
        assert need in corp["header"], need
    cards = []
    for row, _t in corp["rows"][1:]:
        f = [instr.unquote(x) for x in instr.split_fields(row)]
        if cols["carte"] >= len(f) or not f[cols["carte"]].strip():
            continue                      # hors deck
        pk = f[cols["PK"]].strip()
        title = f[cols["carte"]].strip()
        links = {}
        for l in LANGS:
            v = f[cols[f"link_{l}"]] if cols[f"link_{l}"] < len(f) else ""
            links[l] = v.strip()
        cards.append({"pk": pk, "title": title, "links": links,
                      "name": f[cols["title"]] if "title" in cols and cols["title"] < len(f) else ""})
    return cards


def calibrate(cards):
    print(f"=== ETALONNAGE LOCAL ===")
    print(f"  cartes du deck = {len(cards)}   (ai-01 : 175)")
    print()
    print(f"  {'lang':5} {'natif':>6} {'dont wiki':>10} {'attendus':>10}")
    expect_nat = {"en": 161, "fr": 89, "ru": 27, "pt": 9, "es": 8, "ar": 8, "zh": 8, "fa": 7}
    expect_wiki = {"en": 112, "fr": 72, "ru": 23, "pt": 9, "es": 8, "ar": 8, "zh": 8, "fa": 7}
    ok = True
    multi = 0
    for l in ["en", "fr", "ru", "pt", "es", "ar", "zh", "fa"]:
        nat = sum(1 for c in cards if c["links"][l])
        wiki = sum(1 for c in cards if parse_wikis(c["links"][l]))
        nmulti = sum(1 for c in cards if len(split_urls(c["links"][l])) > 1)
        multi += nmulti
        e1, e2 = expect_nat[l], expect_wiki[l]
        flag = "OK" if (nat == e1 and wiki == e2) else f"ECART (attendu {e1}/{e2})"
        if nat != e1 or wiki != e2:
            ok = False
        print(f"  {l:5} {nat:>6} {wiki:>10} {e1:>6}/{e2:<4} {flag}"
              + (f"   [dont {nmulti} cellule(s) multi-URL]" if nmulti else ""))

    anchored = sum(1 for c in cards if any(parse_wikis(c["links"][l]) for l in LANGS))
    print()
    print(f"  cellules multi-URL (toutes langues) = {multi}")
    print(f"  cartes ancrees wikipedia = {anchored}   (ai-01 : 125)")
    at_least_one = sum(1 for c in cards if any(c["links"][l] for l in LANGS))
    print(f"  cartes avec >=1 lien     = {at_least_one}   (ai-01 : 165)")

    vise = 0
    for c in cards:
        if not any(parse_wikis(c["links"][l]) for l in LANGS):
            continue
        for l in LANGS:
            if not c["links"][l]:
                vise += 1
    print(f"  cellules VISEES          = {vise}   (ai-01 : 728)")
    if anchored != 125 or at_least_one != 165 or vise != 728:
        ok = False
    print()
    print("  => INSTRUMENT " + ("ETALONNE" if ok else "NON ETALONNE — ne rien publier"))
    return ok


def api(lang, titles):
    """Un appel : prop=langlinks|pageprops, redirects resolus."""
    params = {
        "action": "query", "format": "json", "formatversion": "2",
        "prop": "langlinks|pageprops", "lllimit": "500", "redirects": "1",
        "titles": "|".join(titles),
    }
    url = f"https://{lang}.wikipedia.org/w/api.php?" + urllib.parse.urlencode(params)
    req = urllib.request.Request(url, headers={"User-Agent": UA})
    with urllib.request.urlopen(req, timeout=45) as r:
        return json.loads(r.read().decode("utf-8"))


def resolve(cards):
    """Interroge l'API pour chaque ancre unique. -> {(lang,titre): infos}"""
    anchors = {}
    for c in cards:
        for l in LANGS:
            for w in parse_wikis(c["links"][l]):
                anchors.setdefault(w, None)
    by_lang = {}
    for (lg, t) in anchors:
        by_lang.setdefault(lg, []).append(t)
    out = {}
    for lg, titles in sorted(by_lang.items()):
        titles = sorted(set(titles))
        print(f"  [{lg}] {len(titles)} ancres -> {-(-len(titles)//50)} appel(s)")
        for i in range(0, len(titles), 50):
            batch = titles[i:i + 50]
            try:
                d = api(lg, batch)
            except Exception as e:
                print(f"      ERREUR lot {i//50}: {e}")
                for t in batch:
                    out[(lg, t)] = {"error": str(e)}
                continue
            q = d.get("query", {})
            norm = {n["from"]: n["to"] for n in q.get("normalized", [])}
            redir = {r["from"]: r["to"] for r in q.get("redirects", [])}
            pages = {p.get("title"): p for p in q.get("pages", [])}
            for t in batch:
                key = redir.get(norm.get(t, t), norm.get(t, t))
                p = pages.get(key)
                if p is None:
                    out[(lg, t)] = {"missing": True}
                    continue
                # formatversion=2 nomme la cle 'title' ; formatversion=1 utilise '*'
                ll = {x["lang"]: (x.get("title") or x.get("*"))
                      for x in p.get("langlinks", [])}
                dab = "disambiguation" in (p.get("pageprops") or {})
                out[(lg, t)] = {
                    "missing": "missing" in p,
                    "redirected_to": redir.get(norm.get(t, t)),
                    "disambiguation": dab,
                    "langlinks": ll,
                    "title": p.get("title"),
                }
            time.sleep(0.6)                      # courtoisie API
    return out


def measure(cards, resolved):
    print()
    print("=== RENDEMENT REEL ===")
    print(f"  {'lang':5} {'vise':>5} {'trouve':>7} {'manquant':>9} {'taux':>7}")
    total = {"vise": 0, "trouve": 0}
    worklist = {}
    found_pairs = []
    candidates = []           # [(pk, lang_source, lang_cible, titre_cible, url_cible), ...]
    for l in LANGS:
        vise = trouve = 0
        manque = []
        for c in cards:
            if c["links"][l]:
                continue                          # deja natif
            anchors = []
            for x in LANGS:
                anchors.extend(parse_wikis(c["links"][x]))
            if not anchors:
                continue                          # pas d'ancre -> 275
            vise += 1
            got = None
            won = None
            for (alg, at) in anchors:
                info = resolved.get((alg, at)) or {}
                if info.get("disambiguation"):
                    continue
                if l in (info.get("langlinks") or {}):
                    got = info["langlinks"][l]
                    won = (alg, at)
                    break
            if got:
                trouve += 1
                found_pairs.append((l, got))
                url = f"https://{l}.wikipedia.org/wiki/" + urllib.parse.quote(got.replace(" ", "_"))
                candidates.append({
                    "pk": c["pk"],
                    "card": c.get("name") or c["title"],
                    "lang_source": won[0],
                    "ancre_source": won[1],
                    "lang_cible": l,
                    "titre_cible": got,
                    "url_cible": url,
                    "colonne_csv": f"link_{l}",
                })
            else:
                manque.append((c["pk"], c.get("name") or c["title"]))
        total["vise"] += vise
        total["trouve"] += trouve
        worklist[l] = manque
        taux = (100.0 * trouve / vise) if vise else 0.0
        print(f"  {l:5} {vise:>5} {trouve:>7} {len(manque):>9} {taux:>6.1f}%")
    taux = (100.0 * total["trouve"] / total["vise"]) if total["vise"] else 0.0
    print(f"  {'TOT':5} {total['vise']:>5} {total['trouve']:>7} {total['vise']-total['trouve']:>9} {taux:>6.1f}%")
    # echantillon : un trouve par langue cible, pour la verification DoD
    sample = []
    for l in LANGS:
        for (tl, tt) in found_pairs:
            if tl == l:
                sample.append((tl, tt))
                break
    return total, worklist, sample, candidates


def quality(resolved):
    """Faux positifs mesures : homonymies, redirections, pages absentes."""
    dab = [k for k, v in resolved.items() if v and v.get("disambiguation")]
    redir = [k for k, v in resolved.items() if v and v.get("redirected_to")]
    miss = [k for k, v in resolved.items() if v and v.get("missing")]
    err = [k for k, v in resolved.items() if v and v.get("error")]
    print()
    print("=== QUALITE DES ANCRES (faux positifs mesures) ===")
    print(f"  ancres uniques resolues   = {len(resolved)}")
    print(f"  dont pages d'HOMONYMIE    = {len(dab)}   (exclues du rendement)")
    print(f"  dont REDIRECTIONS         = {len(redir)}   (suivies, cible reelle interrogee)")
    print(f"  dont pages ABSENTES       = {len(miss)}")
    print(f"  dont ERREURS reseau/API   = {len(err)}")
    for k in dab:
        print(f"      homonymie : {k[0]}:{k[1]!r}")
    return {"dab": len(dab), "redirects": len(redir), "missing": len(miss), "errors": len(err)}


def verify_sample(resolved, targets, n=8):
    """DoD #1471 : la langue doit etre CONSTATEE, pas deduite du domaine."""
    picked = targets[:n]
    print()
    print("=== VERIFICATION ECHANTILLONNEE (HTTP 200 + langue CONSTATEE) ===")
    okc = 0
    for (tlang, ttitle) in picked:
        url = f"https://{tlang}.wikipedia.org/wiki/" + urllib.parse.quote(ttitle.replace(" ", "_"))
        try:
            req = urllib.request.Request(url, headers={"User-Agent": UA})
            with urllib.request.urlopen(req, timeout=30) as r:
                code = r.status
                html = r.read(4096).decode("utf-8", "ignore")
        except Exception as e:
            print(f"  {tlang}:{ttitle[:38]:40} ERREUR {e}")
            continue
        m = re.search(r'<html[^>]*\blang="([^"]+)"', html, re.I)
        got = (m.group(1) if m else "?")[:2].lower()
        good = (code == 200 and got == tlang)
        okc += 1 if good else 0
        print(f"  {tlang}:{ttitle[:38]:40} HTTP {code}  constate lang={got!r}  {'OK' if good else 'ECART'}")
    print(f"  => {okc}/{len(picked)} conformes")
    return okc, len(picked)


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--calibrate", action="store_true")
    ap.add_argument("--measure", action="store_true")
    ap.add_argument("--out")
    ap.add_argument("--emit-cands",
                    help="ecrit la liste des cibles candidates au format JSONL "
                         "(pk, lang_source, ancre_source, lang_cible, titre_cible, url_cible)")
    a = ap.parse_args()
    cards = load_deck()
    if not calibrate(cards):
        print("\n  ⛔ ARRET : instrument non etalonne.")
        return 2
    if not a.measure:
        return 0
    resolved = resolve(cards)
    q = quality(resolved)
    total, worklist, found_pairs, candidates = measure(cards, resolved)
    verify_sample(resolved, found_pairs)
    if a.out:
        with open(a.out, "w", encoding="utf-8") as fh:
            json.dump({"total": total, "quality": q,
                       "worklist": worklist,
                       "resolved_keys": len(resolved),
                       "candidates": candidates,
                       "candidates_total": len(candidates)}, fh, ensure_ascii=False, indent=1)
        print(f"\n  worklist ecrite -> {a.out}")
    if a.emit_cands:
        with open(a.emit_cands, "w", encoding="utf-8") as fh:
            for c in candidates:
                fh.write(json.dumps(c, ensure_ascii=False) + "\n")
        print(f"\n  candidats ecrits -> {a.emit_cands}  ({len(candidates)} lignes)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
