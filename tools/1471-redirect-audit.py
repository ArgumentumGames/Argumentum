# -*- coding: utf-8 -*-
"""#1471 P24-22 — audit des redirections par colonne link_* : collisions d'article d'arrivée.

Règle ratifiée du 15/11 (#1471) : « le titre de l'article est comparé aux libellés des
1 408 nœuds, en portugais et en anglais, après résolution des redirections » —
l'unicité d'URL est aveugle aux redirections, une collision se juge sur l'article
d'ARRIVÉE. Deux lectures (mesure-témoin ai-01, contre-revue #2273, c.6890284753) :

  1. COLISION (à corriger) : ≥ 2 cartes dont le lien mène au MÊME article, MÊME
     fragment (ou sans fragment). Témoin pt : 13 groupes sur 191 liens link_pt
     (p. ex. 172 & 1083 -> Apofenia, 0 & 696 -> Falácia) — l'instrument doit les
     retrouver.
  2. Même article, FRAGMENTS DISTINCTS : accepté (1024 & 1174 -> Novum Organum,
     sections « Os ídolos da tribo » / « da caverna » distinctes) — listé à part,
     jamais compté comme collision.
  3. TITRE = LIBELLÉ d'un AUTRE nœud : le titre d'arrivée (résolu) est le libellé
     (colonne text_<langue du lien> ou text_en) d'un nœud différent — le lecteur
     croit lire la fiche de ce nœud-là.

⛔ 0 écriture CSV : lecture seule. Sortie = rapport markdown sur stdout.

Périmètre : les 8 colonnes link_fr, link_en, link_ru, link_pt, link_es, link_ar,
link_fa, link_zh. Seuls les liens <langue>.wikipedia.org sont résolus (API
action=query&redirects=1, lots de 50 titres) ; les autres hôtes sont inventoriés.

Résolution : l'API rend « normalized » (forme canonique du titre demandé) et
« redirects » (from -> to) ; on suit la chaîne (max 5 sauts), puis on lit le titre
final et le drapeau « missing » (article rouge : titre conservé, drapeau signalé).
Le fragment (#section, éventuellement percent-encodé) est dissocié AVANT la
résolution et re-rattaché après, décodé et comparé sans casse.

Usage :
    python tools/1471-redirect-audit.py                # audit complet (réseau)
    python tools/1471-redirect-audit.py --lang pt      # une seule colonne
    python tools/1471-redirect-audit.py --self-test    # logique hors réseau

La normalisation des titres/libellés : casefold, _ -> espace, espaces collapsés.
C'est la même des deux côtés de la comparaison ; les formes diacritiques
différentes ne sont PAS rapprochées (aucune translittération inventée).
"""
import csv
import io
import json
import os
import sys
import time
import urllib.error
import urllib.parse
import urllib.request

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")

LANGS = ("fr", "en", "ru", "pt", "es", "ar", "fa", "zh")
BATCH = 50
API_DELAY_S = 2.0
CACHE = os.path.join(ROOT, ".tmp", "1471-redirect-audit-cache.json")
UA = ("Argumentum-1471-redirect-audit/1.0 (corpus maintenance; "
      "https://github.com/ArgumentumGames/Argumentum)")


def norm(s):
    """Normalisation titre/libellé : casefold, _ -> espace, espaces collapsés."""
    return " ".join((s or "").replace("_", " ").split()).casefold()


def parse_url(url):
    """(host, title, fragment) d'une URL wiki, ou None hors wikipedia."""
    try:
        parts = urllib.parse.urlsplit(url.strip())
    except ValueError:
        return None
    host = (parts.hostname or "").lower()
    if not host.endswith(".wikipedia.org"):
        return None
    wiki = host.split(".")[0]
    raw = parts.path
    if not raw.startswith("/wiki/"):
        return None
    title = urllib.parse.unquote(raw[len("/wiki/"):]).replace("_", " ")
    frag = urllib.parse.unquote(parts.fragment).replace("_", " ") if parts.fragment else ""
    return wiki, title, frag


def fetch_batch(wiki, titles):
    """Résout un lot de titres ; rend (map from->to chaînée, set(titres finaux manquants))."""
    base = "https://%s.wikipedia.org/w/api.php" % wiki
    params = {"action": "query", "format": "json", "redirects": "1",
              "titles": "|".join(titles)}
    url = base + "?" + urllib.parse.urlencode(params)
    req = urllib.request.Request(url, headers={"User-Agent": UA})
    last = None
    waits = (5.0, 15.0, 45.0, 90.0)  # 429/transitoire — Retry-After d'abord, puis backoff
    for w in waits:
        try:
            with urllib.request.urlopen(req, timeout=25) as resp:
                data = json.loads(resp.read().decode("utf-8"))
            break
        except urllib.error.HTTPError as e:  # 429 : honorer Retry-After si présent
            last = e
            ra = e.headers.get("Retry-After") if e.headers else None
            time.sleep(float(ra) if ra and ra.replace(".", "").isdigit() else w)
        except Exception as e:  # transient network — retry then die loudly
            last = e
            time.sleep(w)
    else:
        raise RuntimeError("API %s injoignable: %r" % (wiki, last))
    q = data.get("query", {})
    mapping = {}
    for m in q.get("normalized", []):
        mapping[m["from"]] = m["to"]
    for r in q.get("redirects", []):
        # l'API rend le from déjà normalisé ; les deux clés mènent au même to
        mapping[r["from"]] = r["to"]
        mapping.setdefault(r["from"], r["to"])
    missing = set()
    for p in q.get("pages", {}).values():
        if "missing" in p:
            missing.add(p["title"])
    return mapping, missing


class Resolver(object):
    def __init__(self):
        self.maps = {}   # wiki -> {from: to}
        self.missing = {}  # wiki -> set(final titles)

    def _load_cache(self):
        """Cache disque (scratch .tmp/, hors dépôt) : reprise sans re-payer les wikis résolus."""
        if not os.path.exists(CACHE):
            return
        try:
            with open(CACHE, "r", encoding="utf-8") as fh:
                blob = json.load(fh)
            for wiki, mp in blob.get("maps", {}).items():
                self.maps.setdefault(wiki, {}).update(mp)
            for wiki, miss in blob.get("missing", {}).items():
                self.missing.setdefault(wiki, set()).update(miss)
        except (ValueError, OSError):
            pass  # cache corrompu -> re-résoudre, jamais bloquer

    def _save_cache(self):
        os.makedirs(os.path.dirname(CACHE), exist_ok=True)
        blob = {"maps": self.maps,
                "missing": {w: sorted(s) for w, s in self.missing.items()}}
        tmp = CACHE + ".tmp"
        with open(tmp, "w", encoding="utf-8") as fh:
            json.dump(blob, fh, ensure_ascii=False)
        os.replace(tmp, CACHE)

    def prime(self, wiki, titles):
        mp = self.maps.setdefault(wiki, {})
        miss = self.missing.setdefault(wiki, set())
        # déjà résolu (clé ou valeur déjà connue) -> pas de re-requête
        known_vals = set(mp.values())
        todo = [t for t in titles if t not in mp and t not in known_vals]
        for i in range(0, len(todo), BATCH):
            chunk = todo[i:i + BATCH]
            mapping, missing = fetch_batch(wiki, chunk)
            mp.update(mapping)
            miss |= missing
            time.sleep(API_DELAY_S)
        self._save_cache()

    def resolve(self, wiki, title):
        """Titre d'arrivée après la chaîne de redirection (max 5 sauts)."""
        cur = title
        mp = self.maps.get(wiki, {})
        for _ in range(5):
            nxt = mp.get(cur)
            if not nxt or nxt == cur:
                break
            cur = nxt
        return cur


def collect(rows, lang):
    """Liens wikipedia de la colonne link_<lang> : liste de dicts."""
    col = "link_" + lang
    out = []
    for r in rows:
        url = (r.get(col) or "").strip()
        if not url:
            continue
        parsed = parse_url(url)
        if parsed is None:
            out.append({"pk": r["PK"], "path": r["path"], "url": url,
                        "wiki": None, "title": None, "frag": None})
            continue
        wiki, title, frag = parsed
        out.append({"pk": r["PK"], "path": r["path"], "url": url,
                    "wiki": wiki, "title": title, "frag": frag})
    return out


def audit_lang(rows, lang, resolver):
    links = collect(rows, lang)
    wiki_links = [l for l in links if l["wiki"]]
    other = [l for l in links if not l["wiki"]]

    per_wiki_titles = {}
    for l in wiki_links:
        per_wiki_titles.setdefault(l["wiki"], set()).add(l["title"])
    for wiki, titles in per_wiki_titles.items():
        resolver.prime(wiki, titles)

    for l in wiki_links:
        l["final"] = resolver.resolve(l["wiki"], l["title"])
        l["redir"] = norm(l["final"]) != norm(l["title"])

    # 1. collisions : même (wiki, article d'arrivée, fragment) — fragments distincts séparés
    by_dest = {}
    for l in wiki_links:
        by_dest.setdefault((l["wiki"], norm(l["final"]), norm(l["frag"])), []).append(l)
    collisions = {k: v for k, v in by_dest.items() if len({x["pk"] for x in v}) >= 2}

    by_article = {}
    for l in wiki_links:
        by_article.setdefault((l["wiki"], norm(l["final"])), []).append(l)
    section_split = []
    for k, v in sorted(by_article.items()):
        if len({x["pk"] for x in v}) >= 2:
            frags = {norm(x["frag"]) for x in v}
            if len(frags) >= 2 and (k[0], k[1], sorted(frags)[0]) not in collisions \
               and all((k[0], k[1], f) not in collisions for f in frags):
                section_split.append((k, v))

    # 3. titre d'arrivée = libellé d'un AUTRE nœud (langue du lien + anglais)
    labels = {}
    for r in rows:
        for lc in (lang, "en"):
            v = (r.get("text_" + lc) or "").strip()
            if v:
                labels.setdefault(norm(v), []).append(r["PK"])
    label_hits = []
    for l in wiki_links:
        key = norm(l["final"])
        for pk in labels.get(key, ()):
            if pk != l["pk"]:
                label_hits.append((l, pk, key))
                break

    redir_count = sum(1 for l in wiki_links if l["redir"])
    missing = [l for l in wiki_links if l["final"] in resolver.missing.get(l["wiki"], ())]
    return {"links": links, "wiki_links": wiki_links, "other": other,
            "collisions": collisions, "section_split": section_split,
            "label_hits": label_hits, "redir_count": redir_count, "missing": missing}


def fmt_group(links):
    return ", ".join("%s (%s)" % (l["pk"], l["path"]) for l in sorted(links, key=lambda x: int(x["pk"])))


def self_test():
    """Logique hors réseau : collisions, fragments distincts, libellés."""
    rows = [
        {"PK": "1", "path": "1.1", "link_pt": "https://pt.wikipedia.org/wiki/Apofenia", "text_pt": "A", "text_en": "AA"},
        {"PK": "2", "path": "1.2", "link_pt": "https://pt.wikipedia.org/wiki/Apofhenia", "text_pt": "B", "text_en": "BB"},
        {"PK": "3", "path": "1.3", "link_pt": "https://pt.wikipedia.org/wiki/Novum_Organum#Sec1", "text_pt": "C", "text_en": "CC"},
        {"PK": "4", "path": "1.4", "link_pt": "https://pt.wikipedia.org/wiki/Novum_Organum#Sec2", "text_pt": "D", "text_en": "DD"},
        {"PK": "5", "path": "1.5", "link_pt": "https://pt.wikipedia.org/wiki/Fal%C3%A1cia", "text_pt": "Falácia", "text_en": "EE"},
        {"PK": "6", "path": "1.6", "link_pt": "https://pt.wikipedia.org/wiki/Falacia", "text_pt": "F", "text_en": "Falácia"},
        {"PK": "7", "path": "1.7", "link_pt": "https://example.com/x", "text_pt": "G", "text_en": "GG"},
    ]
    res = Resolver()
    res.maps["pt"] = {"Apofhenia": "Apofenia", "Falacia": "Falácia"}
    res.missing["pt"] = set()
    a = audit_lang(rows, "pt", res)
    assert len(a["collisions"]) == 2, a["collisions"].keys()
    keys = {tuple(k) for k in a["collisions"]}
    assert ("pt", "apofenia", "") in keys and ("pt", "falácia", "") in keys, keys
    # même article, fragments distincts : 3+4 -> Novum Organum, pas une collision
    assert len(a["section_split"]) == 1 and a["section_split"][0][0][1] == "novum organum", a["section_split"]
    # libellé d'un AUTRE nœud : 6 arrive sur « Falácia » = text_pt de 5 ;
    # et 5 arrive sur « Falácia » = text_en de 6 — les DEUX sont des hits
    # (la règle compare langue du lien ET anglais, autre nœud) ; le PROPRE
    # libellé pt de 5 ne compte pas (self exclu)
    pks = {l["pk"] for l, _pk, _k in a["label_hits"]}
    assert pks == {"5", "6"}, pks
    assert len(a["other"]) == 1 and a["other"][0]["pk"] == "7"
    # URL parsing : décodage percent + fragment dissocié
    w, t, f = parse_url("https://pt.wikipedia.org/wiki/Novum_Organum#Os_%C3%ADdolos_da_tribo")
    assert (w, t, f) == ("pt", "Novum Organum", "Os ídolos da tribo"), (w, t, f)
    assert parse_url("https://fallaeasies.com/x") is None
    print("SELF-TEST OK (collisions / fragments distincts / libellé autre nœud / parse URL)")


def main():
    if "--self-test" in sys.argv:
        self_test()
        return
    langs = LANGS
    if "--lang" in sys.argv:
        langs = (sys.argv[sys.argv.index("--lang") + 1],)
    raw = open(CSV_PATH, "rb").read().decode("utf-8-sig")
    rows = list(csv.DictReader(io.StringIO(raw)))
    assert len(rows) == 1408, "rangées %d != 1408" % len(rows)
    resolver = Resolver()
    resolver._load_cache()
    total_col = total_sec = total_lab = 0
    print("# #1471 audit redirections — collisions d'article d'arrivée (lecture seule)")
    print()
    for lang in langs:
        a = audit_lang(rows, lang, resolver)
        n_col = len(a["collisions"])
        total_col += n_col
        total_sec += len(a["section_split"])
        total_lab += len(a["label_hits"])
        print("## link_%s : %d liens (%d wikipedia, %d autres hôtes), %d redirections, %d articles rouges"
              % (lang, len(a["links"]), len(a["wiki_links"]), len(a["other"]),
                 a["redir_count"], len(a["missing"])))
        hosts = {}
        for l in a["other"]:
            h = urllib.parse.urlsplit(l["url"]).hostname or "?"
            hosts[h] = hosts.get(h, 0) + 1
        if hosts:
            print("- autres hôtes (non résolus) : %s" % ", ".join("%s ×%d" % kv for kv in sorted(hosts.items())))
        print("- **collisions (même article, même fragment) : %d groupes**" % n_col)
        for k in sorted(a["collisions"], key=lambda k: min(int(x["pk"]) for x in a["collisions"][k])):
            grp = a["collisions"][k]
            art = grp[0]["final"]
            print("  - %s -> « %s »%s" % (fmt_group(grp), art,
                                          " [ROUGE]" if art in resolver.missing.get(grp[0]["wiki"], ()) else ""))
        if a["section_split"]:
            print("- même article, fragments distincts (acceptés, non collisions) : %d" % len(a["section_split"]))
            for k, v in a["section_split"]:
                print("  - %s -> « %s » (sections : %s)" % (
                    fmt_group(v), v[0]["final"],
                    ", ".join(sorted({x["frag"] for x in v}))[:120]))
        print("- titres d'arrivée = libellé d'un AUTRE nœud (text_%s / text_en) : %d"
              % (lang, len(a["label_hits"])))
        for l, pk, key in a["label_hits"]:
            print("  - %s (%s) -> « %s » = libellé de %s" % (l["pk"], l["path"], l["final"], pk))
        if a["missing"]:
            print("- articles rouges (missing) : %s"
                  % ", ".join("%s -> « %s »" % (l["pk"], l["final"]) for l in a["missing"]))
        print()
    print("TOTAL : %d groupes en collision, %d paires même-article-sections-distinctes, %d titres = libellé d'un autre nœud"
          % (total_col, total_sec, total_lab))


if __name__ == "__main__":
    main()
