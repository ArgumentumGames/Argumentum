#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Mesure 0-écriture des marqueurs du portugais continental (pt-PT) dans le corpus Argumentum.

Grain ②.3 du dispatch ai-01 10/10 12:47Z (#458 c.6096790964) : compter, par corpus
(Fallacies, Scenarii, Virtues ; Rules = témoin aligné #1095), les marqueurs continentaux
avec témoins, puis livrer volume + proposition. AUCUNE écriture : lecture seule.

Tiers (le même motif peut être légitime dans l'autre variante — jamais totaliser sans lire):
  A = lexical non-ambigu (camião n'existe pas au Brésil)
  B = lexical/grammatical fortement penché (secção, contacto, estar a + INF, clitique + a + INF)
  C = contextuel (facto, registo, castanho, time — attestés dans l'autre variante)

Direction inverse (témoins BR) comptée aussi : caminhão, celular, ônibus, você…
Le témoin ABSOLU : PK 621 Fallacies « camião » et PK 1361 « vi-o a » (relevé 03/10,
docs/corpus/458-v23-g31-examples-pt-fidelite-2026-10-03.md) doivent être VUS.

Auto-test (--self-test) : motifs sur fixtures embarquées + garde d'en-tête par corpus.
Contrôle inverse : une rangée synthétique doit lever les motifs A et B (dans le self-test).

v1 -> v2 (leçons de la lecture intégrale des hits v1, consignées dans le rapport) :
  le motif v1 « clitique + a + INF » attrapait la PRÉPOSITION (« convencê-lo a continuar »
  existe à l'identique au Brésil) — v2 le restreint aux verbes de perception (vi-o a…).
  « Vossa Majestade » (honorifique figé des deux variantes) exclu par lookahead.
  Ajout : « estar a » après modal (deve estar a…). dossiê retieré C (les deux l'emploient).

Rejouer depuis l'emplacement committé : python docs/corpus/pt-continental-markers.py
(le dépôt est déduit de __file__ ; --repo=... pour surcharger).
"""
import csv
import io
import re
import sys
from pathlib import Path

REPO_DEFAULT = Path(__file__).resolve().parents[2]
CORPORA = None  # filled in main

# --- Marqueurs continentaux (forme pt-PT -> équivalent BR, tier) -------------------
MARKERS_PT = [
    # tier A — lexical non-ambigu
    (r"\bcami[ãa]o(?:es|s)?\b", "camião", "caminhão", "A"),
    (r"\btelem[óo]ve(?:l|is|es)\b", "telemóvel", "celular", "A"),
    (r"\bautocarros?\b", "autocarro", "ônibus", "A"),
    (r"\bcomboios?\b", "comboio", "trem", "A"),
    (r"\bpequeno-almo(ç|c)o\b", "pequeno-almoço", "café da manhã", "A"),
    (r"\becr[ãa]s?\b", "ecrã", "tela", "A"),
    (r"\bficheiros?\b", "ficheiro", "arquivo", "A"),
    (r"\butilizador(?:es)?\b", "utilizador", "usuário", "A"),
    (r"\bequipas?\b", "equipa", "equipe", "A"),
    (r"\bcasa de banho\b", "casa de banho", "banheiro", "A"),
    (r"\bdesportos?\b", "desporto", "esporte", "A"),
    # tier B — fortement penché continental
    (r"\bsec[çc][ãa]o\b", "secção", "seção", "B"),
    (r"\brece[çc][ãa]o\b", "receção", "recepção", "B"),
    (r"\bcontactos?\b", "contacto", "contato", "B"),
    (r"\bplaneamento\b", "planeamento", "planejamento", "B"),
    (r"\bdossi[êe]s?\b", "dossiê", "(les deux variantes l'emploient)", "C"),
    (r"\b(?:v[óo]s|voss[oa]s?|convosco)\b(?!\s+(?:Majestade|Excelência|Santidade|Senhoria|Alteza|Eminência))",
     "vós/vosso (arch.)", "vocês", "B"),
    (r"\best(?:ou|á|ás|amos|ão|ava|avas|ávamos|avam|arei|arás|ará|aremos|arão|eja|ejas|ejamos|ejam|ivesse|ivessem|iver|ivermos|iverem)\s+a\s+(?!partir\b)\w+",
     "estar a + INF (conjugué)", "estar + gerúndio", "B"),
    (r"\bestar\s+a\s+(?!partir\b)\w+",
     "estar a + INF (après modal, ex. devem estar a)", "estar + gerúndio", "B"),
    (r"\b(?:vi|via|viu|vejo|vemos|ouvi|ouviu|ouviram|encontrei|encontrou|apanhou|apanhei|deparou|surpreendi)-?(?:o|a|os|as|lo|la|los|las|no|na|nos|nas)\s+a\s+\w+r\b",
     "verbe de perception + clitique + a + INF", "clitique + gerúndio", "B"),
    # tier C — contextuel
    (r"\bfactos?\b", "facto", "fato", "C"),
    (r"\bregistos?\b", "registo", "registro", "C"),
    (r"\bcastanhos?\b", "castanho", "marrom", "C"),
]

# --- Témoins de la direction BR (le corpus penche-t-il continental ?) --------------
MARKERS_BR = [
    (r"\bcaminh[ãa]o\b", "caminhão"),
    (r"\bcelulares?\b", "celular"),
    (r"\b[ôo]nibus\b", "ônibus"),
    (r"\bbanheiros?\b", "banheiro"),
    (r"\busu[áa]rios?\b", "usuário"),
    (r"\bequipes?\b", "equipe"),
    (r"\btelas?\b", "tela"),
    (r"\bcafé da manhã\b", "café da manhã"),
    (r"\besportes?\b", "esporte"),
    (r"\bvoc[êe]s?\b", "você"),
    (r"\btime\b", "time (equipe)"),
]

CORPUS_SPECS = {
    "fallacies": dict(path="Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
                      id_col="pk", min_pt_cols=3,
                      expect=["text_pt", "desc_pt", "example_pt"]),
    "scenarii": dict(path="Cards/Scenarii/Argumentum Scenarii - Cards.csv",
                     id_col="path", min_pt_cols=8,
                     expect=["context_pt", "issue_pt", "suggestion_pt"]),
    "virtues": dict(path="Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
                    id_col="pk", min_pt_cols=6,
                    expect=["title_pt", "description_pt", "remark_pt"]),
    "rules": dict(path="Cards/Rules/Argumentum Rules - Cards.csv",
                  id_col="pk", min_pt_cols=1, expect=["Text_pt"]),
}

FIXTURES = [
    # (texte, motifs attendus — par nom de marqueur PT)
    ("O camião consome mais combustível que um automóvel.", ["camião"]),
    ("O telemóvel dele não para de tocar no autocarro.", ["telemóvel", "autocarro"]),
    ("Estou a pensar na secção do dossiê.", ["estar a + INF (conjugué)", "secção", "dossiê"]),
    ("Vi-o a aproveitar a deixa do adversário.", ["verbe de perception + clitique + a + INF"]),
    ("Ele deve convencê-lo a continuar.", []),  # « a » préposition : les deux variantes
    ("Senhor, que Vossa Majestade não se zangue.", []),  # honorifique figé : les deux variantes
    ("Eles devem estar a confundir as coisas.", ["estar a + INF (après modal, ex. devem estar a)"]),
    ("Eu adoraria um pequeno-almoço com casa de banho própria.", ["pequeno-almoço", "casa de banho"]),
    ("É um facto: o ficheiro foi enviado ao utilizador.", ["facto", "ficheiro", "utilizador"]),
    ("O caminhão e o celular brasileiros, no ônibus com você.", []),  # direction BR : zéro marqueur PT attendu
    ("A equipa vai viajar de comboio para Lisboa.", ["equipa", "comboio"]),
]

def compile_all():
    pt = [(re.compile(p, re.IGNORECASE), name, br, tier) for p, name, br, tier in MARKERS_PT]
    br = [(re.compile(p, re.IGNORECASE), name) for p, name in MARKERS_BR]
    return pt, br

def header_ok(row, spec):
    """Garde d'en-tête : les colonnes attendues doivent exister, sinon mesure invalide."""
    missing = [c for c in spec["expect"] if c not in row]
    pt_cols = [c for c in row if c.lower().endswith("_pt")]
    return missing, pt_cols

def scan(text, pt_res, br_res):
    hits_pt, hits_br = [], []
    for rx, name, br_eq, tier in pt_res:
        for m in rx.finditer(text):
            hits_pt.append((tier, name, m.group(0), text[max(0, m.start()-40):m.end()+40].replace("\n", " ⏎ ")))
    for rx, name in br_res:
        for m in rx.finditer(text):
            hits_br.append((name, m.group(0)))
    return hits_pt, hits_br

def run_corpus(repo, key, pt_res, br_res, synthetic_extra=None):
    spec = CORPUS_SPECS[key]
    path = repo / spec["path"]
    if not path.exists():
        return dict(error=f"ABSENT: {path}", key=key)
    with open(path, encoding="utf-8-sig", newline="") as f:
        reader = csv.reader(f)
        rows = list(reader)
    if not rows:
        return dict(error="EMPTY", key=key)
    header = rows[0]
    missing, pt_cols = header_ok(header, spec)
    if missing:
        return dict(error=f"HEADER: colonnes absentes {missing}", key=key)
    if len(pt_cols) < spec["min_pt_cols"]:
        return dict(error=f"HEADER: {len(pt_cols)} colonnes _pt < {spec['min_pt_cols']}", key=key)
    id_idx = None
    for i, c in enumerate(header):
        if c.strip().lower() == spec["id_col"]:
            id_idx = i
            break
    out = dict(key=key, rows=0, pt_cols=pt_cols, hits=[], br_hits=[], data_rows=len(rows) - 1)
    for row in rows[1:]:
        if not any(cell.strip() for cell in row):
            continue
        rid = row[id_idx] if id_idx is not None and id_idx < len(row) else "?"
        for ci, col in enumerate(header):
            if not col.lower().endswith("_pt"):
                continue
            cell = row[ci] if ci < len(row) else ""
            if not cell.strip():
                continue
            h_pt, h_br = scan(cell, pt_res, br_res)
            for tier, name, form, ctx in h_pt:
                out["hits"].append((rid, col, tier, name, form, ctx))
            for name, form in h_br:
                out["br_hits"].append((rid, col, name, form))
        out["rows"] += 1
    if synthetic_extra:
        rid, col, text = synthetic_extra
        h_pt, h_br = scan(text, pt_res, br_res)
        for tier, name, form, ctx in h_pt:
            out["hits"].append((rid, col, tier, name, form, "[SYNTH] " + ctx))
    return out

def self_test():
    pt_res, br_res = compile_all()
    failures = []
    for text, expected in FIXTURES:
        hits, brs = scan(text, pt_res, br_res)
        names = {n for _, n, _, _ in hits}
        for e in expected:
            if e not in names:
                failures.append(f"SELF-TEST: « {e} » NON VU dans « {text} »")
        if not expected and hits:
            failures.append(f"SELF-TEST: faux positifs {[(t,n) for t,n,_,_ in hits]} dans « {text} »")
    # contrôle inverse : rangée synthétique
    syn = scan("O autocarro estava a chegar quando o vi a sair.", *compile_all())
    tiers = {t for t, _, _, _ in syn[0]}
    if not {"A", "B"} <= tiers:
        failures.append(f"CONTROL: la rangée synthétique ne lève pas A et B (levé: {tiers})")
    return failures

def main():
    args = sys.argv[1:]
    if "--self-test" in args:
        fails = self_test()
        for f in fails:
            print("FAIL", f)
        print("SELF-TEST:", "FAIL" if fails else "OK (fixtures + contrôle inverse)")
        sys.exit(1 if fails else 0)
    repo = None
    for a in args:
        if a.startswith("--repo="):
            repo = Path(a[len("--repo="):])
    if repo is None:
        repo = REPO_DEFAULT
    pt_res, br_res = compile_all()
    total = {"A": 0, "B": 0, "C": 0}
    for key in CORPUS_SPECS:
        r = run_corpus(repo, key, pt_res, br_res,
                       synthetic_extra=None)
        if "error" in r:
            print(f"== {key}: ERREUR {r['error']}")
            sys.exit(2)
        for rid, col, tier, name, form, ctx in r["hits"]:
            print(f"{key}|{rid}|{col}|{tier}|{name}|{form}|{ctx}")
        for rid, col, name, form in r["br_hits"]:
            print(f"{key}|{rid}|{col}|BR|{name}|{form}|")
        for t in total:
            total[t] += sum(1 for h in r["hits"] if h[2] == t)
        print(f"== {key}: {r['data_rows']} rangées, {len(r['pt_cols'])} col _pt, "
              f"PT {sum(1 for h in r['hits'])} hits, BR {len(r['br_hits'])} hits")
    print(f"== TOTAL tiers PT: A={total['A']} B={total['B']} C={total['C']}")

if __name__ == "__main__":
    main()
