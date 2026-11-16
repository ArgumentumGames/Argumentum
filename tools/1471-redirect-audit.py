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
    python tools/1471-redirect-audit.py --classes      # + classe de chaque groupe
    python tools/1471-redirect-audit.py --self-test    # logique hors réseau

P24-23 (décision de coordination du 15/11, règle par connexité du 15/11 soir) —
`--classes` classe chaque groupe de collision par la parenté MÉCANIQUE de ses
nœuds, et liste à part les groupes « autres », seuls à juger un par un dans les
lots. Un groupe est accepté si ses membres forment UN SEUL bloc quand on les
relie deux à deux par les relations acceptées :
  ascendance   : un chemin préfixe strict de l'autre, à frontière de segment
                 (`5.1` est préfixe de `5.1.2`, pas de `5.10`) ;
  fratrie      : même parent de chemin ;
  même libellé : text_fr OU text_en normalisés égaux (casefold, _ -> espace).
La classe d'un groupe accepté suit la précédence carte-ancetre > freres >
meme-sophisme. Un groupe dont un membre n'est relié à aucun autre — même si
d'autres paires du groupe sont apparentées — est « autres » : c'est précisément
ce membre isolé que les lots doivent juger (HOLD du 15/11 : « une paire suffit »
laissait passer 6 tels groupes, dont PK 2 « Argument bâclé », dont le link_en
est resté `Hasty_generalization`, le titre anglais de son ancien nom d'avant
#1522). La classe décrit la relation des NŒUDS ; le nombre de cartes du deck
par groupe (colonne `carte` non vide) est imprimé à part — il sert au tri des
jugements, pas à la classe.

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


def classify_group(pks, by_pk):
    """Classe mécanique d'un groupe de collision (P24-23, règle par connexité).

    Un groupe est accepté si ses membres forment UN SEUL bloc quand on les
    relie deux à deux par les relations acceptées (ascendance, même parent,
    même libellé normalisé) ; sinon « autres ». La classe d'un groupe accepté
    garde la précédence carte-ancetre > freres > meme-sophisme. Voir l'en-tête
    du module (HOLD du 15/11 : la version « une paire suffit » acceptait des
    groupes contenant un membre isolé).
    """
    paths = {pk: by_pk[pk]["path"] for pk in pks}
    members = sorted(pks)

    def is_prefix(a, b):
        return b.startswith(a + ".")

    def parent(p):
        return p.rsplit(".", 1)[0] if "." in p else ""

    def edge(a, b):
        """Relation acceptée entre deux membres : 'anc', 'fre', 'sos' ou None."""
        if is_prefix(paths[a], paths[b]) or is_prefix(paths[b], paths[a]):
            return "anc"
        if parent(paths[a]) == parent(paths[b]):
            return "fre"
        for col in ("text_fr", "text_en"):
            la = norm(by_pk[a].get(col) or "")
            if la and la == norm(by_pk[b].get(col) or ""):
                return "sos"
        return None

    # connexité : union-find sur les seules arêtes acceptées
    comp = {m: m for m in members}

    def find(x):
        while comp[x] != x:
            comp[x] = comp[comp[x]]
            x = comp[x]
        return x

    kinds = set()
    for i, a in enumerate(members):
        for b in members[i + 1:]:
            k = edge(a, b)
            if k is not None:
                kinds.add(k)
                comp[find(a)] = find(b)
    if len({find(m) for m in members}) > 1:
        return "autres"  # au moins un membre isolé : à juger, pas à présumer
    if "anc" in kinds:
        return "carte-ancetre"
    if "fre" in kinds:
        return "freres"
    return "meme-sophisme"


def deck_count(pks, by_pk):
    """Cartes du deck parmi les PK du groupe (colonne `carte` non vide)."""
    return sum(1 for pk in pks if (by_pk[pk].get("carte") or "").strip())


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

    # classification P24-23 : chaque classe sur des nœuds synthétiques
    cls_rows = [
        {"PK": "1", "path": "5.1", "text_fr": "X", "text_en": "X", "carte": ""},
        {"PK": "2", "path": "5.1.2", "text_fr": "Y", "text_en": "Y", "carte": "1"},
        {"PK": "3", "path": "6.2.1", "text_fr": "P", "text_en": "P", "carte": ""},
        {"PK": "4", "path": "6.2.2", "text_fr": "Q", "text_en": "Q", "carte": ""},
        {"PK": "5", "path": "7.1.1", "text_fr": "Même", "text_en": "Same", "carte": ""},
        {"PK": "6", "path": "8.2.2", "text_fr": "Même", "text_en": "Same", "carte": ""},
        {"PK": "7", "path": "9.1.1", "text_fr": "R", "text_en": "R", "carte": ""},
        {"PK": "8", "path": "10.2.2", "text_fr": "S", "text_en": "S", "carte": ""},
        {"PK": "9", "path": "11.1", "text_fr": "T", "text_en": "T", "carte": ""},
        # témoins de la règle par connexité (HOLD du 15/11)
        {"PK": "10", "path": "3.1", "text_fr": "U", "text_en": "U", "carte": ""},
        {"PK": "11", "path": "3.1.1", "text_fr": "V", "text_en": "V", "carte": ""},
        {"PK": "12", "path": "1.1", "text_fr": "W", "text_en": "W", "carte": ""},
        {"PK": "13", "path": "3.2.3.2.1", "text_fr": "X1", "text_en": "X1", "carte": ""},
        {"PK": "14", "path": "3.2.3.2.2", "text_fr": "X2", "text_en": "X2", "carte": ""},
        {"PK": "15", "path": "9.9.1", "text_fr": "Jumeau", "text_en": "Twin", "carte": ""},
        {"PK": "16", "path": "12.3.4", "text_fr": "Jumeau", "text_en": "Twin", "carte": ""},
        {"PK": "17", "path": "3.2.1", "text_fr": "Y1", "text_en": "Y1", "carte": ""},
        {"PK": "18", "path": "3.2.1.5", "text_fr": "Y2", "text_en": "Y2", "carte": ""},
        {"PK": "19", "path": "5.5.5", "text_fr": "Y2", "text_en": "Y2", "carte": ""},
    ]
    cbp = {r["PK"]: r for r in cls_rows}
    assert classify_group(["1", "2"], cbp) == "carte-ancetre"
    assert classify_group(["3", "4"], cbp) == "freres"
    assert classify_group(["5", "6"], cbp) == "meme-sophisme"
    assert classify_group(["7", "8"], cbp) == "autres"
    # témoin 1 du HOLD : {3.1, 3.1.1, 1.1} — une paire apparentée (ascendance)
    # + un membre isolé doit sortir « autres » (la règle « une paire suffit »
    # y rendait « carte-ancetre » : cette assertion était ROUGE avant fix)
    assert classify_group(["10", "11", "12"], cbp) == "autres"
    # témoin 2 du HOLD : deux paires apparentées sans lien entre elles
    # (forme 662/664 frères + 979/1349 jumeaux) doit sortir « autres »
    assert classify_group(["13", "14", "15", "16"], cbp) == "autres"
    # face stricte de la connexité : relations mixtes en chaîne (forme
    # 633/642/1085 : ancêtre + jumeau) = UN seul bloc → accepté, classe
    # par précédence (une règle « tous liés à tous » l'excluait à tort)
    assert classify_group(["17", "18", "19"], cbp) == "carte-ancetre"
    # précédence : l'ascendance prime sur le libellé partagé
    cbp["2"]["text_fr"] = "X"
    assert classify_group(["1", "2"], cbp) == "carte-ancetre"
    # frontière de segment : 11.1 n'est PAS un préfixe de 11.10
    cbp["9"]["path"] = "11.10"
    assert classify_group(["9", "8"], cbp) == "autres"
    assert deck_count(["1", "2"], cbp) == 1
    print("SELF-TEST OK (collisions / fragments distincts / libellé autre nœud / parse URL / classes P24-23 par connexité)")


def main():
    if "--self-test" in sys.argv:
        self_test()
        return
    langs = LANGS
    if "--lang" in sys.argv:
        langs = (sys.argv[sys.argv.index("--lang") + 1],)
    classes_mode = "--classes" in sys.argv
    raw = open(CSV_PATH, "rb").read().decode("utf-8-sig")
    rows = list(csv.DictReader(io.StringIO(raw)))
    assert len(rows) == 1408, "rangées %d != 1408" % len(rows)
    by_pk = {r["PK"]: r for r in rows}
    resolver = Resolver()
    resolver._load_cache()
    total_col = total_sec = total_lab = 0
    class_totals = {}
    autres_all = []
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
            tag = ""
            if classes_mode:
                pks = sorted({l["pk"] for l in grp}, key=int)
                cls = classify_group(pks, by_pk)
                class_totals[cls] = class_totals.get(cls, 0) + 1
                tag = "[%s] " % cls
                if cls == "autres":
                    autres_all.append((lang, grp, pks))
            print("  - %s%s -> « %s »%s" % (tag, fmt_group(grp), art,
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
    if classes_mode:
        print("CLASSES : " + " · ".join("%s %d" % (c, class_totals.get(c, 0))
                                        for c in ("carte-ancetre", "freres", "meme-sophisme", "autres")))
        print()
        print("## AUTRES — groupes sans parenté mécanique, à juger un par un : %d" % len(autres_all))
        for lang, grp, pks in autres_all:
            art = grp[0]["final"]
            titles = " ; ".join("%s p=%s deck=%d « %s » / « %s »" % (
                pk, by_pk[pk]["path"], 1 if (by_pk[pk].get("carte") or "").strip() else 0,
                (by_pk[pk].get("text_fr") or "").strip(), (by_pk[pk].get("text_en") or "").strip())
                for pk in pks)
            print("- %s · %s · deck=%d -> « %s »%s · %s" % (
                lang, " ".join(pks), deck_count(pks, by_pk), art,
                " [ROUGE]" if art in resolver.missing.get(grp[0]["wiki"], ()) else "", titles))


if __name__ == "__main__":
    main()
