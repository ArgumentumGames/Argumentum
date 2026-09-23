#!/usr/bin/env python3
"""#1502/G7 — Règles : site publié vs cartes du dépôt (instrument, lecture seule).

Télécharge les 6 pages publiées (landing + 5 détails de variantes), en extrait
les sections du volet de contenu DNN, les apparie INTRA-variante aux sections
markdown des 15 cartes de « Argumentum Rules - Cards.csv » (colonne Text, fr),
et imprime la matrice de similarité + les deltas de contenu NOMINATIFS mesurés
le 2026-09-23. Référence de rédaction : docs/quality/1502-g7-regles-site-vs-cartes-2026-09-23.md.

Gardes :
- lecture seule stricte (GET sur les pages publiques, aucun POST/auth/écriture) ;
- aucune écriture CSV/gabarit — ceci est un instrument de dossier d'arbitrage.

Usage : python docs/corpus/rules-site-vs-cards.py   (depuis la racine du dépôt)
Sortie : matrice + deltas ; rc=0 si l'extraction est complète et cohérente,
rc=2 si une page manque ou si l'appariement structural échoue.
"""
import csv
import html
import os
import re
import sys
import unicodedata
import urllib.request
from difflib import SequenceMatcher

BASE = "https://www.argumentum.games/R%C3%A8gles"
VARIANTS = {
    "l-ecole-des-menteurs": ("ecole", ["Rules_01", "Rules_02", "Rules_03", "Rules_04", "Rules_05", "Rules_06"]),
    "le-bingo-mixologie-argumentative": ("bingo", ["Rules_07", "Rules_08"]),
    "le-dernier-beau-parleur": ("beau", ["Rules_09", "Rules_10"]),
    "le-moulin-a-baratin": ("moulin", ["Rules_11", "Rules_12"]),
    "la-parlote-coinchee": ("parlote", ["Rules_13", "Rules_14", "Rules_15"]),
}
TIMEOUT = 30


def fetch(url):
    req = urllib.request.Request(url, headers={"User-Agent": "argumentum-diff/1502 (read-only)"})
    with urllib.request.urlopen(req, timeout=TIMEOUT) as r:
        return r.read().decode("utf-8", errors="replace")


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


def main():
    sys.stdout.reconfigure(encoding="utf-8")
    csv_path = os.path.join("Cards", "Rules", "Argumentum Rules - Cards.csv")
    rows = {r["pk"].strip(): r.get("Text") or "" for r in csv.DictReader(open(csv_path, encoding="utf-8-sig"))}
    if len(rows) != 15:
        print(f"ERREUR: 15 cartes attendues, {len(rows)} lues")
        return 2

    landing = fetch(BASE)
    counts = re.findall(r"(L['’]école des menteurs|Le Bingo mixologie argumentative|Le moulin à baratin|Le dernier beau parleur|La parlote coinchée)\s+(de \d+ à \d+ joueurs|de \d+ joueurs)", text_of(landing))
    print("=== Compteurs joueurs annoncés par le SITE (landing /Règles) ===")
    for name, c in counts:
        print(f"  {name}: {c}")

    print("\n=== Matrice site↔cartes (appariement intra-variante, similarité SequenceMatcher sur texte normalisé) ===")
    failures, paired = 0, 0
    site_all_keys = set()
    for slug, (vk, pks) in VARIANTS.items():
        secs = site_sections(fetch(f"{BASE}/details/{slug}/mid/602"))
        print(f"--- {vk} ({len(secs)} sections site) ---")
        card_pool = {}
        for pk in pks:
            card_pool.update(card_sections(rows[pk]))
        for name, text in secs:
            k = nkey(name)
            site_all_keys.add((vk, k))
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

    print("\n=== Deltas de contenu NOMINATIFS (mesurés 2026-09-23, cf. dossier) ===")
    print("  T1 compteurs joueurs : site école 'de 4 à 10' vs carte Rules_02 'de 4 à 8' (imprimé 2022 : 4 à 8)")
    print("  T2 règle 'À 3 ou 4 joueurs' (jury + pioches) : carte seule — absente site ET imprimé 2022")
    print("  T3 condition 'A 5 joueurs et plus' (1re variante) : site seul — 0 occurrence dans l'historique CSV (git log -S)")
    print("  T4 'de 4 à 4 joueurs' (parlote, site) : formulation suspecte site")
    print("  T5 pictogrammes : inlinés dans les cartes / sections séparées 'Conditions de Victoire'+'Nombre de pioches' (site)")
    print("  T6 sections site sans carte : Carte Mémo (téléchargement), Conditions de Victoire, Nombre de pioches")

    if paired < 30:
        print(f"\nERREUR: appariement structurel insuffisant ({paired} paires < 30)")
        failures += 1
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(main())
