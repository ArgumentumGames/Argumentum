#!/usr/bin/env python3
"""#1502/G7 — Règles : site publié vs cartes du dépôt (instrument, lecture seule).

Télécharge les 6 pages publiées (landing + 5 détails de variantes), en extrait
les sections du volet de contenu DNN, les apparie INTRA-variante aux sections
markdown des 15 cartes de « Argumentum Rules - Cards.csv » (colonne Text, fr),
et MESURE la matrice de similarité + les deltas de contenu nominatifs
(compteurs joueurs par variante, historique git -S). Chaque verdict est
CALCULÉ — rien d'imprimé en chaîne fixe (#1517 v1 : T4 avait traversé le rc=0
parce que sa conclusion était un print figé).

Référence de rédaction : docs/quality/1502-g7-regles-site-vs-cartes-2026-09-23.md.

Gardes :
- lecture seule stricte (GET sur les pages publiques, aucun POST/auth/écriture) ;
- aucune écriture CSV/gabarit — ceci est un instrument de dossier d'arbitrage.

Usage :
  python docs/corpus/rules-site-vs-cards.py [csv_path]
    csv_path optionnel : copie du CSV pour le CONTRÔLE INVERSE par mutation
    (ex. compteur de Rules_09 changé ⇒ verdict « beau » doit basculer à ECART).

Codes de sortie :
  0  extraction complète et structurelle cohérente (les écarts site↔cartes
     sont des RÉSULTATS, pas des erreurs) ;
  2  erreur structurelle : page inaccessible, appariement insuffisant (<30
     paires), compteur introuvable sur une carte attendue, compteur carte
     divergeant de la valeur décidée par l'owner (EXPECTED_CARD_COUNTERS),
     ou git indisponible.
"""
import csv
import html
import os
import re
import subprocess
import sys
import unicodedata
import urllib.request
from difflib import SequenceMatcher

BASE = "https://www.argumentum.games/R%C3%A8gles"
VARIANTS = {
    "l-ecole-des-menteurs": ("ecole", ["Rules_01", "Rules_02", "Rules_03", "Rules_04", "Rules_05", "Rules_06"], "Rules_02"),
    "le-bingo-mixologie-argumentative": ("bingo", ["Rules_07", "Rules_08"], "Rules_07"),
    "le-dernier-beau-parleur": ("beau", ["Rules_09", "Rules_10"], "Rules_09"),
    "le-moulin-a-baratin": ("moulin", ["Rules_11", "Rules_12"], "Rules_11"),
    "la-parlote-coinchee": ("parlote", ["Rules_13", "Rules_14", "Rules_15"], "Rules_13"),
}
SITE_VARIANT_LABELS = [
    ("ecole", "L'école des menteurs"),
    ("bingo", "Le Bingo mixologie argumentative"),
    ("moulin", "Le moulin à baratin"),
    ("beau", "Le dernier beau parleur"),
    ("parlote", "La parlote coinchée"),
]
TIMEOUT = 30
RE_COUNTER_CARD = re.compile(r"R[èe]gles du jeu\s*:\s*(de\s+(\d+)\s+[àa]\s+(\d+)|(\d+))\s*joueurs")
RE_COUNTER_SITE = re.compile(r"(de\s+(\d+)\s+[àa]\s+(\d+)|(?<!\d)(\d+))\s*joueurs")
# Décision owner 23/09 (#1502 c.5794914387, « monter à 10 joueurs, ça passe avec le
# nouveau deck ») : la carte École annonce « de 3 à 10 joueurs ». Le site publié
# reste à 4-10 jusqu'à sa re-synchronisation — l'écart site/carte est ATTENDU.
EXPECTED_CARD_COUNTERS = {"ecole": (3, 10)}


def fetch(url):
    req = urllib.request.Request(url, headers={"User-Agent": "argumentum-diff/1502 (read-only)"})
    try:
        with urllib.request.urlopen(req, timeout=TIMEOUT) as r:
            return r.read().decode("utf-8", errors="replace")
    except Exception as e:
        print(f"ERREUR: page inaccessible {url} — {e}")
        return None


def norm(s):
    s = unicodedata.normalize("NFKD", s)
    s = "".join(c for c in s if not unicodedata.combining(c))
    s = s.replace("’", "'").replace(" ", " ")
    s = re.sub(r"[*_#>`]+", "", s)
    return re.sub(r"\s+", " ", s).strip().lower()


def sim(a, b):
    return round(SequenceMatcher(None, a, b).ratio(), 3) if a and b else 0.0


def text_of(fragment):
    f = re.sub(r"<script.*?</script>", "", fragment, flags=re.S | re.I)
    f = re.sub(r"<style.*?</style>", "", f, flags=re.S | re.I)
    f = re.sub(r"<br\s*/?>", "\n", f, flags=re.I)
    f = re.sub(r"</(p|li|h[1-6]|tr|div)>", "\n", f, flags=re.I)
    f = re.sub(r"<li[^>]*>", "- ", f, flags=re.I)
    f = re.sub(r"<[^>]+>", "", f)
    f = html.unescape(f)
    lines = [re.sub(r"\s+", " ", ln).strip() for ln in f.split("\n")]
    return "\n".join(ln for ln in lines if ln)


def site_sections(raw):
    m = re.search(r'id="dnn_ContentPane"(.*?)(?:<footer|id="dnn_FooterPane")', raw, re.S)
    pane = m.group(1) if m else raw
    parts = re.split(r"(<h[23][^>]*>.*?</h[23]>)", pane, flags=re.S | re.I)
    doc, heading = [], None
    for p in parts:
        if re.match(r"<h[23]", p, re.I):
            heading = text_of(p)
            doc.append((heading, ""))
        elif heading is not None and doc:
            doc[-1] = (heading, doc[-1][1] + text_of(p) + "\n")
    return doc


def nkey(s):
    n = norm(s)
    n = re.sub(r"^(\d+\.\s*)", "", n)
    n = n.replace(":", "").replace("de la manche", "").strip()
    return n.replace("resume du jeu", "resume")


def card_sections(text):
    parts = re.split(r"(^#{1,3}\s+.*$)", text, flags=re.M)
    cur, out = None, {}
    for p in parts:
        if p and re.match(r"^#{1,3}\s", p) and "\n" not in p.strip():
            cur = re.sub(r"^#+\s*", "", p.strip())
            out[cur] = ""
        elif cur is not None:
            out[cur] += p + "\n"
    return out


def counter_card(text):
    """(min, max) du compteur d'une carte, None si absent."""
    m = RE_COUNTER_CARD.search(text)
    if not m:
        return None
    if m.group(2):
        return (int(m.group(2)), int(m.group(3)))
    return (int(m.group(4)), int(m.group(4)))


def counter_site(label, landing_text):
    """(min, max) du compteur annoncé sur la landing pour la variante, None si absent."""
    idx = landing_text.find(label)
    if idx < 0:
        return None
    m = RE_COUNTER_SITE.search(landing_text, idx + len(label))
    if not m:
        return None
    if m.group(2):
        return (int(m.group(2)), int(m.group(3)))
    return (int(m.group(4)), int(m.group(4)))


def git_s_count(needle):
    """Compte les commits touchant l'occurrence — EXÉCUTÉ, pas imprimé."""
    ref = "origin/master"
    try:
        r = subprocess.run(["git", "log", "--oneline", "-S", needle, ref, "--", "Cards/Rules/"],
                           capture_output=True, text=True, timeout=30)
        return len([l for l in r.stdout.splitlines() if l.strip()])
    except Exception:
        return None


def main(argv):
    sys.stdout.reconfigure(encoding="utf-8")
    csv_path = argv[1] if len(argv) > 1 else os.path.join("Cards", "Rules", "Argumentum Rules - Cards.csv")
    rows = {r["pk"].strip(): r.get("Text") or "" for r in csv.DictReader(open(csv_path, encoding="utf-8-sig"))}
    if len(rows) != 15:
        print(f"ERREUR: 15 cartes attendues, {len(rows)} lues dans {csv_path}")
        return 2

    # ── Compteurs joueurs : MESURÉS des deux côtés, verdict CALCULÉ ──
    landing = fetch(BASE)
    if landing is None:
        return 2
    landing_txt = re.sub(r"\s+", " ", text_of(landing))
    print("=== Compteurs joueurs par variante — site (landing) vs carte (ligne « Règles du jeu ») ===")
    structural_fail = False
    for vk, label in SITE_VARIANT_LABELS:
        s = counter_site(label, landing_txt)
        card_pk = next(c for slug, (v, pks, c) in VARIANTS.items() if v == vk)
        c = counter_card(rows.get(card_pk, ""))
        if s is None or c is None:
            fmt = lambda t: "?" if t is None else f"{t[0]}-{t[1]}"
            print(f"  {vk:8s} site={fmt(s)} carte({card_pk})={fmt(c)}  [STRUCTURE: compteur introuvable]")
            structural_fail = True
            continue
        verdict = "identique" if s == c else "ECART"
        expected = EXPECTED_CARD_COUNTERS.get(vk)
        if expected is not None:
            if c != expected:
                print(f"  {vk:8s} site={s[0]}-{s[1]}  carte({card_pk})={c[0]}-{c[1]}  "
                      f"[STRUCTURE: compteur carte != {expected[0]}-{expected[1]} attendu "
                      f"(décision owner #1502)]")
                structural_fail = True
                continue
            if s != c:
                verdict = "ECART attendu (site à re-synchroniser)"
        print(f"  {vk:8s} site={s[0]}-{s[1]}  carte({card_pk})={c[0]}-{c[1]}  -> {verdict}")

    # ── Historique git -S : EXÉCUTÉ ──
    print("\n=== Historique git -S (exécuté, ref origin/master, chemin Cards/Rules/) ===")
    n3 = git_s_count("3 ou 4 joueurs")
    n5 = git_s_count("5 joueurs et plus")
    if n3 is None or n5 is None:
        print("  ERREUR: git indisponible")
        structural_fail = True
    else:
        print(f"  \"3 ou 4 joueurs\"  : {n3} commit(s)")
        print(f"  \"5 joueurs et plus\": {n5} commit(s) (0 = jamais existé dans la lignée cartes)")

    # ── Matrice de similarité ──
    print("\n=== Matrice site↔cartes (appariement intra-variante, similarité SequenceMatcher sur texte normalisé) ===")
    paired = 0
    for slug, (vk, pks, _) in VARIANTS.items():
        raw = fetch(f"{BASE}/details/{slug}/mid/602")
        if raw is None:
            return 2
        secs = site_sections(raw)
        print(f"--- {vk} ({len(secs)} sections site) ---")
        card_pool = {}
        for pk in pks:
            card_pool.update(card_sections(rows[pk]))
        for name, text in secs:
            k = nkey(name)
            cands = [(ck, cv) for ck, cv in card_pool.items() if nkey(ck) == k]
            if not cands:
                print(f"  {name[:34]:36s} <-> (aucune carte)")
                continue
            paired += 1
            ns, nc = norm(text), norm(cands[0][1])
            s = sim(nc, ns)
            if len(ns) < 5 and len(nc) < 5:
                tag = "ancre"  # titre de niveau intermédiaire sans corps des deux côtés
            elif s >= 0.985:
                tag = "IDENT"
            elif s >= 0.85:
                tag = "proche"
            else:
                tag = "reecrit"
            print(f"  {name[:34]:36s} <-> {cands[0][0][:30]:32s} sim={s:5.3f}  {tag}")

    if paired < 30:
        print(f"\nERREUR: appariement structurel insuffisant ({paired} paires < 30)")
        structural_fail = True
    if structural_fail:
        return 2
    print(f"\n[{paired} paires] Instrument cohérent — les écarts ci-dessus sont des résultats.")
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
