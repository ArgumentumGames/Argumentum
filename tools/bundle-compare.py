#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""Comparateur de paquets : diff manifeste ↔ manifeste + rapport « où regarder ».

Driver du grain 2 du pool c.5993735448 (05/10) : prépare le verdict visuel d'ai-01
après la régénération post-verdict. Les manifestes sont produits par
tools/pdf-page-signature-batch.py (une entrée par PDF : sha256_pdf, pages,
page_signatures). Ce comparateur NE RE-SIGNE RIEN : il diff deux manifestes,
agrège, et rend un rapport lisible — ai-01 ouvre les pages listées, pas 80 PDF.

Ce que le rapport répond (dans l'ordre) :
  1. COMPLÉTUDE   : mêmes 80 fichiers (8 langues × 10 documents), rien en plus/moins.
  2. PAGINATION   : même nombre de pages par document — les corrections de la
                    campagne sont textuelles : TOUTE dérive de pagination est rouge.
  3. PÉRIMÈTRE    : par document, pages au signature différente (contenu d'images
                    embarquées + géométrie, cf. tools/pdf-page-signature.py), avec
                    zones de deck pour TarotCards (p.1-15 Rules, 16-29 Memo,
                    30-379 Fallacies — mesuré 07/09, runbook §E).
  4. TÉMOIN INVERSE : --expect-deck lang=préfixe — un deck attendu touché dont
                    0 page bouge = défaillance silencieuse (leçon clobber : le
                    run peut « réussir » en réutilisant l'existant). Chaque entrée
                    attendue doit avoir ≥ 1 page différente.

Codes de sortie : 0 = rapport écrit, complétude ET pagination tenues (des pages
différentes ne sont PAS un échec : c'est le produit) ; 1 = anomalie structurelle
(fichier manquant/surnuméraire, pagination dérive, ou témoin inverse mort) ;
2 = erreur.

Usage :
  python tools/bundle-compare.py ANCIEN.json NOUVEAU.json --out rapport.md [--json diff.json]
      [--expect-deck fr=TarotCards] [--expect-deck es=TarotCards ...]
      [--baseline-release NOTE] [--max-listed 80]

  --self-test   mutations falsifiantes sur manifestes de fixture (sans PDF) :
                le diff doit nommer exactement le fichier/page muté ; un fichier
                retiré doit sortir en complétude ; une page changée en plus doit
                rester nommée ; --expect-deck sur un deck intact doit échouer.

Étalonnage (05/10) : témoin réel = baseline v2.0.0 vs elle-même → 80/80
identiques, 0 page différente, sortie 0 (contrôle positif sur données réelles).
"""
import argparse
import json
import sys
from datetime import datetime, timezone

# Zones de deck pour TarotCards_{lang} (runbook §E, mesuré 07/09 page par page) :
# p.1-15 Rules (15 faces sans dos) · p.16-29 Memo ×7 (recto-verso) · p.30-379 Fallacies ×175.
TAROT_ZONES = (
    (1, 15, "Rules"),
    (16, 29, "Memo"),
    (30, 10**9, "Fallacies"),
)


def zone_of(doc, page):
    """Nom de zone de deck pour une page d'un document (best effort)."""
    base = doc.rsplit("_", 1)[0] if doc.count("_") else doc
    if "TarotCards" in doc and "Virtues" not in doc and "Print" not in doc:
        for lo, hi, name in TAROT_ZONES:
            if lo <= page <= hi:
                return name
    if "Virtues" in doc:
        return "Virtues"
    if "Poker" in doc:
        return "Scenarii"
    return "autre"


def load_manifest(path):
    with open(path, "r", encoding="utf-8") as fh:
        m = json.load(fh)
    if "files" not in m:
        raise SystemExit(f"ERREUR : {path} n'est pas un manifeste (clé 'files' absente)")
    return m


def key_of(e):
    return (e["lang"], e["file"])


def compare(old, new):
    """Diff complet : renvoie (rows, completeness_problems, pagination_problems)."""
    old_by = {key_of(e): e for e in old["files"]}
    new_by = {key_of(e): e for e in new["files"]}

    completeness = []
    for k in sorted(set(old_by) - set(new_by)):
        completeness.append(f"absent du nouveau : {k[0]}/{k[1]}")
    for k in sorted(set(new_by) - set(old_by)):
        completeness.append(f"surnuméraire dans le nouveau : {k[0]}/{k[1]}")

    rows = []
    pagination = []
    for k in sorted(set(old_by) & set(new_by)):
        a, b = old_by[k], new_by[k]
        row = {"lang": k[0], "file": k[1], "pages_old": a["pages"], "pages_new": b["pages"],
               "bytes_old": a.get("bytes"), "bytes_new": b.get("bytes"),
               "sha256_identical": a.get("sha256_pdf") == b.get("sha256_pdf"),
               "changed_pages": [], "zones": {}}
        if a["pages"] != b["pages"]:
            pagination.append(f"{k[0]}/{k[1]} : {a['pages']} -> {b['pages']} pages")
        n = min(a["pages"], b["pages"])
        changed = [i + 1 for i in range(n)
                   if a["page_signatures"][i] != b["page_signatures"][i]]
        row["changed_pages"] = changed
        for p in changed:
            z = zone_of(k[1], p)
            row["zones"][z] = row["zones"].get(z, 0) + 1
        rows.append(row)
    return rows, completeness, pagination


def render_report(old, new, rows, completeness, pagination, expect_decks, note,
                  max_listed):
    lines = []
    total_changed = sum(len(r["changed_pages"]) for r in rows)
    docs_changed = [r for r in rows if r["changed_pages"]]
    identical_docs = len(rows) - len(docs_changed)

    lines.append("# Comparaison de paquets — rapport « où regarder »")
    lines.append("")
    lines.append(f"- Ancien : {old.get('bundle_dir', '?')} (capturé {old.get('captured_utc', '?')}, "
                 f"{old.get('pdf_count', '?')} PDF)")
    lines.append(f"- Nouveau : {new.get('bundle_dir', '?')} (capturé {new.get('captured_utc', '?')}, "
                 f"{new.get('pdf_count', '?')} PDF)")
    if note:
        lines.append(f"- Note : {note}")
    lines.append(f"- Instrument : signatures page-par-page (images embarquées + géométrie), "
                 f"tools/pdf-page-signature.py via -batch")
    lines.append("")
    lines.append(f"**Documents comparés : {len(rows)} · documents au contenu changé : "
                 f"{len(docs_changed)} · pages différentes au total : {total_changed}**")
    lines.append("")

    # 1. Complétude
    lines.append("## 1. Complétude")
    if completeness:
        lines.append(f"**{len(completeness)} anomalie(s) :**")
        lines.extend(f"- {c}" for c in completeness)
    else:
        lines.append(f"- OK : {len(rows)} documents appariés, rien en plus ni en moins.")
    lines.append("")

    # 2. Pagination
    lines.append("## 2. Pagination (contractuelle : les corrections sont textuelles)")
    if pagination:
        lines.append(f"**{len(pagination)} dérive(s) de pagination :**")
        lines.extend(f"- {p}" for p in pagination)
    else:
        lines.append(f"- OK : nombre de pages identique sur les {len(rows)} documents.")
    lines.append("")

    # 3. Périmètre du changement
    lines.append("## 3. Périmètre du changement (par langue)")
    lines.append("")
    lines.append("| Langue | Document | Pages diff. | Zones | Pages (liste) |")
    lines.append("|---|---|---:|---|---|")
    by_lang = {}
    for r in docs_changed:
        by_lang.setdefault(r["lang"], []).append(r)
    for lang in sorted(by_lang):
        for r in sorted(by_lang[lang], key=lambda x: x["file"]):
            pages = ", ".join(str(p) for p in r["changed_pages"][:max_listed])
            if len(r["changed_pages"]) > max_listed:
                pages += f" … (+{len(r['changed_pages']) - max_listed})"
            zones = ", ".join(f"{z}:{n}" for z, n in sorted(r["zones"].items()))
            lines.append(f"| {lang} | {r['file']} | {len(r['changed_pages'])} | {zones} | {pages} |")
    if not docs_changed:
        lines.append("| — | (aucun document au contenu changé) | 0 | — | — |")
    lines.append("")

    # 3bis. Résumé deck × langue (croisement census)
    lines.append("## 3bis. Résumé par deck (à croiser avec le census de la tête de lancement)")
    lines.append("")
    lines.append("| Langue | Deck | Documents touchés | Pages diff. totales |")
    lines.append("|---|---|---|---:|")
    deck_agg = {}
    for r in docs_changed:
        decks = set(r["zones"].keys()) or {"autre"}
        for d in decks:
            k = (r["lang"], d)
            agg = deck_agg.setdefault(k, {"docs": 0, "pages": 0})
            agg["docs"] += 1
            agg["pages"] += r["zones"].get(d, 0)
    for (lang, deck) in sorted(deck_agg):
        agg = deck_agg[(lang, deck)]
        lines.append(f"| {lang} | {deck} | {agg['docs']} | {agg['pages']} |")
    if not deck_agg:
        lines.append("| — | — | 0 | 0 |")
    lines.append("")

    # 4. Témoin inverse
    lines.append("## 4. Témoin inverse (decks attendus touchés)")
    if not expect_decks:
        lines.append("- Aucune attente fournie (--expect-deck lang=préfixe).")
    else:
        for spec in expect_decks:
            lang, prefix = spec.split("=", 1)
            hit = [r for r in rows
                   if r["lang"] == lang and prefix in r["file"]
                   and r["changed_pages"]]
            n_pages = sum(len(r["changed_pages"]) for r in hit)
            if hit:
                lines.append(f"- OK : {lang}/{prefix}* → {len(hit)} document(s), "
                             f"{n_pages} page(s) différente(s).")
            else:
                lines.append(f"- **ROUGE : {lang}/{prefix}* attendu touché, 0 page différente "
                             f"— défaillance silencieuse probable (clobber incomplet ?)**")
    lines.append("")
    lines.append(f"*Généré le {datetime.now(timezone.utc).isoformat(timespec='seconds')} "
                 f"par tools/bundle-compare.py*")
    return "\n".join(lines)


def self_test():
    """Mutations falsifiantes sur manifestes de fixture — aucun PDF requis.

    1. TÉMOIN      : ancien vs copie de lui-même → 0 page différente, exit-vue 0.
    2. MUTATION    : une signature de page retournée dans UN document → le diff
                     nomme exactement ce document et cette page, aucune autre.
    3. COMPLÉTUDE  : un document retiré du nouveau → l'anomalie le nomme.
    4. PAGINATION  : pages 379 → 380 dans un document → la dérive est nommée.
    5. TÉMOIN INVERSE : --expect-deck sur un deck intact → ROUGE rendu.
    """
    import copy
    import tempfile
    import os

    def fixture():
        return {
            "bundle_dir": "temoin", "captured_utc": "2026-10-05T00:00:00+00:00",
            "pdf_count": 2,
            "files": [
                {"lang": "fr", "file": "Argumentum_TarotCards_fr.pdf", "bytes": 100,
                 "sha256_pdf": "a" * 64, "pages": 379,
                 "page_signatures": [f"s{i}" for i in range(379)]},
                {"lang": "es", "file": "Argumentum_TarotCards_es.pdf", "bytes": 100,
                 "sha256_pdf": "b" * 64, "pages": 379,
                 "page_signatures": [f"t{i}" for i in range(379)]},
            ],
        }

    ok = True
    with tempfile.TemporaryDirectory() as td:
        old = fixture()

        # 1. témoin
        rows, comp, pag = compare(old, copy.deepcopy(old))
        changed = sum(len(r["changed_pages"]) for r in rows)
        print(f"1. TÉMOIN           : pages diff={changed}, complétude={len(comp)}, pagination={len(pag)}")
        if changed or comp or pag:
            print("   FAIL : le témoin identique présente des différences")
            ok = False
        else:
            print("   PASS")

        # 2. mutation : page 200 (indice 199) du doc fr retournée
        mut = copy.deepcopy(old)
        mut["files"][0]["page_signatures"][199] = "MUTATED"
        rows, comp, pag = compare(old, mut)
        hit = [r for r in rows if r["changed_pages"]]
        print(f"2. MUTATION         : docs touchés={len(hit)}, pages={hit[0]['changed_pages'] if hit else None}, "
              f"zones={hit[0]['zones'] if hit else None}")
        if len(hit) == 1 and hit[0]["file"].endswith("_fr.pdf") and hit[0]["changed_pages"] == [200]:
            print("   PASS")
        else:
            print("   FAIL : la mutation n'est pas rapportée exactement")
            ok = False

        # 3. complétude : document es retiré
        mut = copy.deepcopy(old)
        del mut["files"][1]
        rows, comp, pag = compare(old, mut)
        print(f"3. COMPLÉTUDE       : anomalies={comp}")
        if len(comp) == 1 and "absent du nouveau" in comp[0] and "_es.pdf" in comp[0]:
            print("   PASS")
        else:
            print("   FAIL : le document retiré n'est pas nommé")
            ok = False

        # 4. pagination : 379 -> 380
        mut = copy.deepcopy(old)
        mut["files"][0]["pages"] = 380
        mut["files"][0]["page_signatures"].append("extra")
        rows, comp, pag = compare(old, mut)
        print(f"4. PAGINATION       : dérives={pag}")
        if len(pag) == 1 and "379 -> 380" in pag[0]:
            print("   PASS")
        else:
            print("   FAIL : la dérive de pagination n'est pas nommée")
            ok = False

        # 5. témoin inverse : deck es attendu touché, corpus intact -> ROUGE
        rep = render_report(old, copy.deepcopy(old), *compare(old, copy.deepcopy(old)),
                            expect_decks=["es=TarotCards"], note=None, max_listed=10)
        print("5. TÉMOIN INVERSE   : ROUGE rendu =", "ROUGE" in rep)
        if "ROUGE : es/TarotCards" in rep:
            print("   PASS")
        else:
            print("   FAIL : le témoin inverse intact ne rend pas ROUGE")
            ok = False

        # zone_of : bornes mesurées 07/09
        checks = [(("Argumentum_TarotCards_fr.pdf", 15), "Rules"),
                  (("Argumentum_TarotCards_fr.pdf", 16), "Memo"),
                  (("Argumentum_TarotCards_fr.pdf", 30), "Fallacies"),
                  (("Argumentum_TarotCards_Virtues_fr.pdf", 1), "Virtues"),
                  (("Argumentum_PokerCards_fr.pdf", 1), "Scenarii")]
        zok = all(zone_of(d, p) == want for (d, p), want in checks)
        print(f"6. ZONES TAROT      : bornes 15/16/30 + Virtues/Poker = {'PASS' if zok else 'FAIL'}")
        if not zok:
            ok = False

    print("SELF-TEST " + ("PASS" if ok else "FAIL"))
    return ok


def main(argv=None):
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    # positionnels optionnels : --self-test se joue sans manifestes
    ap.add_argument("old_manifest", nargs="?")
    ap.add_argument("new_manifest", nargs="?")
    ap.add_argument("--out", required=False, help="rapport markdown de sortie")
    ap.add_argument("--json", help="diff machine-readable de sortie")
    ap.add_argument("--expect-deck", action="append", default=[],
                    metavar="LANG=PREFIX",
                    help="deck attendu touché (ex. fr=TarotCards) — 0 page = ROUGE")
    ap.add_argument("--baseline-release", help="note sur la référence (ex. release v2.0.0-review)")
    ap.add_argument("--max-listed", type=int, default=80)
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args(argv)

    if args.self_test:
        return 0 if self_test() else 1
    if not (args.old_manifest and args.new_manifest):
        ap.error("old_manifest et new_manifest requis (sauf --self-test)")

    try:
        old = load_manifest(args.old_manifest)
        new = load_manifest(args.new_manifest)
    except Exception as exc:
        print(f"ERREUR : {exc}", file=sys.stderr)
        return 2

    rows, completeness, pagination = compare(old, new)
    report = render_report(old, new, rows, completeness, pagination,
                           args.expect_deck, args.baseline_release, args.max_listed)

    if args.json:
        payload = {
            "what": "diff de paquets pour verdict visuel",
            "old": {"bundle_dir": old.get("bundle_dir"), "captured_utc": old.get("captured_utc")},
            "new": {"bundle_dir": new.get("bundle_dir"), "captured_utc": new.get("captured_utc")},
            "generated_utc": datetime.now(timezone.utc).isoformat(timespec="seconds"),
            "completeness_problems": completeness,
            "pagination_problems": pagination,
            "documents": rows,
        }
        with open(args.json, "w", encoding="utf-8") as fh:
            json.dump(payload, fh, ensure_ascii=False, indent=1)

    out = args.out or "bundle-compare-report.md"
    with open(out, "w", encoding="utf-8", newline="\n") as fh:
        fh.write(report + "\n")
    print(report)
    print(f"\nrapport : {out}" + (f" · json : {args.json}" if args.json else ""))

    structural = bool(completeness or pagination)
    dead_witness = False
    for spec in args.expect_deck:
        lang, prefix = spec.split("=", 1)
        if not any(r["lang"] == lang and prefix in r["file"] and r["changed_pages"]
                   for r in rows):
            dead_witness = True
    return 1 if (structural or dead_witness) else 0


if __name__ == "__main__":
    sys.exit(main())
