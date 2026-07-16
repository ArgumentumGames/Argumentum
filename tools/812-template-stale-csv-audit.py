#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audit templates Face à csv embeddé stale — issue #812 (dispatch ai-01, 2026-07-16).

Méthode code=truth (mémoire mt-garbage-sweep-false-zero) :
  1. Pour chaque CardSet Face ayant DataSet non-None + SkipDataUpdate!=true,
     le rendu = CSV source injecté par HarvestManager.cs:342-363.
  2. Le `csv` embeddé du template est overridden au runtime.
  3. Si template embeddé ≠ CSV source → "stale latent" (no current impact, mais
     piège si SkipDataUpdate est mis à true un jour, ou si comparaison QA
     cible le mauvais fichier).

Livrable : commentaire structuré sur issue #812 (rapport markdown).

Usage : python tools/audit_template_stale_csv.py [--out report.md]
"""
import csv, io, json, re, sys, collections, argparse, os

BASE = r"c:\dev\Argumentum"

# ── CardSet registry (mirror WebBasedGeneratorConfig.cs lignes 105-456) ──────
# (name, face_dataset, back_dataset, face_template_relpath, back_template_relpath_or_None)
# Source de vérité = production config (master 72c408ec).
CARDSETS = [
    # (name, face_DS, back_DS, face_json, back_json)
    ("Rules",                "Rules",             "None", "Cards/Rules/Argumentum_Rules_fr.json", None),
    ("Fallacies",            "FallaciesTaxonomy", "None", "Cards/Fallacies/Argumentum_Fallacies_Face_fr.json", "Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"),
    ("Virtues",              "VirtuesTaxonomy",   "None", "Cards/Fallacies/Argumentum_Virtues_Face_fr.json", "Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"),
    ("Scenarii",             "Scenarii",          "Scenarii", "Cards/Scenarii/Argumentum_Scenarii_Face_fr.json", "Cards/Scenarii/Argumentum_Scenarii_Back_fr.json"),
    ("Fallacies2",           "FallaciesTaxonomy", "None", "Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json", "Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"),
    ("Fallacies3",           "FallaciesTaxonomy", "None", "Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json", "Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"),
    ("RulesPrintAndPlay",    "RulesPrintAndPlay", None,  "Cards/Rules/Argumentum_Rules_fr.json", None),
    ("FallaciesPrintAndPlay","FallaciesTaxonomy", "None", "Cards/Fallacies/Argumentum_Fallacies_Face_fr.json", "Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"),
    ("FallaciesPrintAndPlayLight", "FallaciesTaxonomy", "None", "Cards/Fallacies/Argumentum_Fallacies_Face_fr.json", "Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"),
    ("VirtuesPrintAndPlayLight",   "VirtuesTaxonomy",   "None", "Cards/Fallacies/Argumentum_Virtues_Face_fr.json", "Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"),
    ("Memo",                 "FallaciesTaxonomy", "FallaciesTaxonomy", "Cards/Memo/Argumentum_Memo_Face_fr.json", "Cards/Memo/Argumentum_Memo_Back_fr.json"),
    ("ScenariiPrintAndPlay", "Scenarii",          "Scenarii", "Cards/Scenarii/Argumentum_Scenarii_Face_fr.json", "Cards/Scenarii/Argumentum_Scenarii_Back_fr.json"),
    ("ScenariiPrintAndPlayFull", "Scenarii",      "Scenarii", "Cards/Scenarii/Argumentum_Scenarii_Face_fr.json", "Cards/Scenarii/Argumentum_Scenarii_Back_fr.json"),
    ("MemoPrintAndPlay",     "FallaciesTaxonomy", "FallaciesTaxonomy", "Cards/Memo/Argumentum_Memo_Face_fr.json", "Cards/Memo/Argumentum_Memo_Back_fr.json"),
    ("FallaciesWeb",         "FallaciesTaxonomy", "None", "Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json", None),
]

# CSV source paths per DataSet (mirror AssetConverterConfig.cs DataSets).
DS_TO_CSV = {
    "Rules":             r"Cards\Rules\Argumentum Rules - Cards.csv",
    "RulesPrintAndPlay": r"Cards\Rules\Argumentum_Rules_Francais_edition_fevrier_2022_Print_and_Play.json",  # template (?)
    "FallaciesTaxonomy": r"Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv",
    "VirtuesTaxonomy":   r"Cards\Fallacies\Argumentum Virtues - Taxonomy.csv",
    "Scenarii":          r"Cards\Scenarii\Argumentum Scenarii - Cards.csv",
}

# A subset uses a non-CSV source — log those as "template-based DS" so we don't compare.

def normalize_csv_text(t: str) -> str:
    """Trim BOM, normalize CRLF → LF, strip trailing whitespace per line."""
    if not t:
        return ""
    t = t.lstrip("﻿")
    t = t.replace("\r\n", "\n").replace("\r", "\n")
    return "\n".join(line.rstrip() for line in t.split("\n"))

def read_template_csv(path):
    """Return the 'csv' string embedded in a CardPen template JSON, or None."""
    try:
        with open(path, encoding="utf-8-sig") as f:
            doc = json.load(f)
    except (FileNotFoundError, json.JSONDecodeError):
        return None
    csd = doc.get("CardSetDocument") or doc
    return csd.get("csv")

def read_source_csv(path):
    """Read raw CSV file content as text (preserve encoding BOM + CRLF)."""
    try:
        with open(path, "rb") as f:
            raw = f.read()
    except FileNotFoundError:
        return None
    # decode: utf-8-sig then fallback latin-1 (CSV can be 8-bit clean)
    try:
        return raw.decode("utf-8-sig")
    except UnicodeDecodeError:
        return raw.decode("latin-1", errors="replace")

def stats_for_csv(text: str):
    """Crude stats for divergence characterization."""
    if not text:
        return {"chars": 0, "lines": 0, "cols_max": 0, "rows": 0}
    reader = csv.reader(io.StringIO(text))
    rows = list(reader)
    cols_max = max((len(r) for r in rows), default=0)
    return {
        "chars": len(text),
        "lines": text.count("\n") + (0 if text.endswith("\n") else 1),
        "cols_max": cols_max,
        "rows": len(rows),
    }

def diff_samples(src_text, tmpl_text, n=3):
    """Return first N diverging rows (or 'identical')."""
    src_rows = list(csv.reader(io.StringIO(src_text)))
    tmpl_rows = list(csv.reader(io.StringIO(tmpl_text)))
    n_src = len(src_rows)
    n_tmpl = len(tmpl_rows)
    ncols_src = len(src_rows[0]) if src_rows else 0
    ncols_tmpl = len(tmpl_rows[0]) if tmpl_rows else 0
    summary = {
        "rows_src": n_src,
        "rows_tmpl": n_tmpl,
        "cols_src_header": ncols_src,
        "cols_tmpl_header": ncols_tmpl,
        "row_delta": n_src - n_tmpl,
        "col_delta": ncols_src - ncols_tmpl,
        "identical": normalize_csv_text(src_text) == normalize_csv_text(tmpl_text),
    }
    return summary

def classify(face_ds, summary, src_stats, tmpl_stats):
    """Suggest risk: HIGH (rows diverged → template outdated), MEDIUM (cosmetic),
    LOW (header-only), NONE (identical)."""
    if summary["identical"]:
        return "NONE", "identical"
    if abs(summary["row_delta"]) > 5:
        return "HIGH", f"row count delta {summary['row_delta']:+d} (template carries stale snapshot)"
    if summary["col_delta"] != 0:
        return "HIGH", f"header column delta {summary['col_delta']:+d}"
    if abs(summary["rows_src"] - summary["rows_tmpl"]) <= 2 and summary["cols_src_header"] == summary["cols_tmpl_header"]:
        return "LOW", "minor row edits (e.g. typo fix, casing), low latent risk"
    return "MEDIUM", "non-trivial row diff (template snapshot drifted)"

# ── Quality analysis (tells us if the stale `csv` embeddé is harmless or harmful) ─
# Mémoire mt-garbage-sweep-false-zero : on regarde ce qui a CHANGÉ depuis le template,
# pas seulement ce qui est différent.
FLAG_TOKENS = [
    "english channel", "peacher", "facility", "have randomly divide",
    "has them by ensuring", "lays them out while",
    "randomly divide", "have them by", "them in mind",
]
FR_STOPWORDS = {"le","la","les","un","une","des","du","de","dans","pour","avec","sans",
                "vers","par","qui","que","quoi","dont","mais","donc","car","être","avoir",
                "cette","ces","son","sa","ses","notre","votre","leur","ils","elles","sont",
                "était","été","quand","alors","puis","aussi","encore","très","peu","chaque"}
LOWER_HEADING_RE = re.compile(r'(?:^|\n)(#{1,6})\s+([a-zàâäéèêëïîôöùûüç])')

def quality_scan(text: str):
    """Return count of MT-garbage tells in the embedded `csv` (rows that would
    render badly IF the runtime ever read this template)."""
    if not text:
        return {"flag_tokens": 0, "lower_headings": 0, "fr_stopword_cells": 0, "examples": []}
    ft = sum(text.lower().count(t) for t in FLAG_TOKENS)
    lh = len(LOWER_HEADING_RE.findall(text))
    # Find FR-stopword-heavy "EN" cells (rough heuristic: any cell with 4+ FR stopwords)
    examples = []
    fr_cells = 0
    for row in csv.reader(io.StringIO(text)):
        for cell in row:
            words = re.findall(r"[a-zàâäéèêëïîôöùûüç]+", cell.lower())
            if len([w for w in words if w in FR_STOPWORDS]) >= 4:
                fr_cells += 1
                if len(examples) < 3:
                    examples.append(cell[:80])
    return {"flag_tokens": ft, "lower_headings": lh, "fr_stopword_cells": fr_cells, "examples": examples}

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--out", default=None, help="Markdown output file (default: stdout)")
    ap.add_argument("--scope", choices=["faces","backs","all"], default="faces")
    args = ap.parse_args()

    out_lines = []
    out_lines.append("# Audit templates CardPen — `csv` embeddé stale vs CSV source (#812)")
    out_lines.append("")
    out_lines.append("**Generated** : 2026-07-16 (auto, read-only) — issue #812 dispatch ai-01")
    out_lines.append("**Base** : master `72c408ec`")
    out_lines.append("**Method** : code=truth — `HarvestManager.cs:342-363` injecte le CSV source au runtime,")
    out_lines.append("             écrasant le `csv` embeddé du template pour toute carte Face.")
    out_lines.append("**Implication** : le `csv` embeddé est mort pour les Faces — un delta = risque **latent**, pas un bug de rendu actuel.")
    out_lines.append("")
    out_lines.append("**Rappel** : corrigé via #803/#805 (cette fois sur le bon fichier), puis contredit par généralisation.")
    out_lines.append("**Statut rendu actuel** : OK (relecture T&A confirme) — seul le `csv` embeddé est stale.")
    out_lines.append("")

    counts = collections.Counter()
    findings = []

    for name, fds, bds, fjson, bjson in CARDSETS:
        if args.scope == "faces":
            sides = [("Face", fds, fjson)]
        elif args.scope == "backs":
            sides = [("Back", bds, bjson)] if bjson else []
        else:
            sides = [("Face", fds, fjson), ("Back", bds, bjson)] if bjson else [("Face", fds, fjson)]

        for side, ds, jpath in sides:
            counts["total"] += 1
            base = {"cardset": name, "side": side, "ds": ds, "template": jpath, "source_csv": None, "summary": None}
            if ds == "None" or jpath is None:
                counts["skip-template-only"] += 1
                findings.append({**base,
                    "template": jpath or "(none)",
                    "risk": "N/A",
                    "note": "template embeddé = source de vérité (DataSet=None)",
                    "reason": "template = render truth"})
                continue
            src_rel = DS_TO_CSV.get(ds)
            if src_rel is None:
                counts["skip-unknown-ds"] += 1
                findings.append({**base,
                    "source_csv": None,
                    "risk": "REVIEW",
                    "note": f"DataSet '{ds}' non mappé — vérifier manuellement",
                    "reason": "unmapped DS"})
                continue
            src_path = os.path.join(BASE, src_rel)
            tmpl_path = os.path.join(BASE, jpath)
            src_text = read_source_csv(src_path)
            tmpl_csv = read_template_csv(tmpl_path)
            base["source_csv"] = src_rel
            if src_text is None:
                counts["skip-missing-source"] += 1
                findings.append({**base,
                    "risk": "ERROR",
                    "note": "CSV source introuvable",
                    "reason": "missing source CSV"})
                continue
            if tmpl_csv is None:
                counts["skip-missing-template"] += 1
                findings.append({**base,
                    "risk": "REVIEW",
                    "note": "template sans clé 'csv' (non-CardPen ?)",
                    "reason": "no csv key"})
                continue
            summary = diff_samples(src_text, tmpl_csv)
            src_stats = stats_for_csv(src_text)
            tmpl_stats = stats_for_csv(tmpl_csv)
            tmpl_quality = quality_scan(tmpl_csv)
            src_quality = quality_scan(src_text)
            risk, reason = classify(ds, summary, src_stats, tmpl_stats)
            counts[f"risk-{risk}"] += 1
            findings.append({**base,
                "risk": risk, "reason": reason,
                "note": f"diff detected — see stats below",
                "summary": summary,
                "src_stats": src_stats, "tmpl_stats": tmpl_stats,
                "tmpl_quality": tmpl_quality, "src_quality": src_quality,
            })

    # ── Report ────────────────────────────────────────────────────────────
    out_lines.append("## Summary")
    out_lines.append("")
    out_lines.append("| CardSets scoped | templates Face/Back analysés | NONE | LOW | MEDIUM | HIGH | REVIEW | skip |")
    out_lines.append("|---:|---:|---:|---:|---:|---:|---:|---:|")
    out_lines.append(f"| {sum(1 for c in CARDSETS)} | {counts['total']} "
                     f"| {counts['risk-NONE']} | {counts['risk-LOW']} | {counts['risk-MEDIUM']} "
                     f"| {counts['risk-HIGH']} | {counts['REVIEW']} "
                     f"| {counts['skip-template-only'] + counts['skip-unknown-ds'] + counts['skip-missing-source'] + counts['skip-missing-template']} |")
    out_lines.append("")

    # Per-finding
    out_lines.append("## Findings (per CardSet × side)")
    out_lines.append("")
    by_risk = {"HIGH": [], "MEDIUM": [], "LOW": [], "NONE": [], "REVIEW": [], "ERROR": [], "N/A": []}
    for f in findings:
        by_risk.setdefault(f["risk"], []).append(f)

    for risk in ["HIGH", "MEDIUM", "LOW", "NONE", "REVIEW", "ERROR", "N/A"]:
        items = by_risk.get(risk, [])
        if not items:
            continue
        out_lines.append(f"### {risk} — {len(items)} finding(s)")
        out_lines.append("")
        for f in items:
            out_lines.append(f"#### `{f['cardset']}` ({f['side']}) — DataSet={f['ds']}")
            out_lines.append(f"- **Template** : `{f['template']}`")
            if f['source_csv']:
                out_lines.append(f"- **CSV source (render truth)** : `{f['source_csv']}`")
            out_lines.append(f"- **Risk** : **{f['risk']}**")
            if "reason" in f and f["reason"]:
                out_lines.append(f"- **Why** : {f['reason']}")
            if f.get("summary"):
                s = f["summary"]
                out_lines.append(f"- **Stats** : rows src={s['rows_src']} / tmpl={s['rows_tmpl']} (Δ{s['row_delta']:+d}) "
                                 f"· cols src={s['cols_src_header']} / tmpl={s['cols_tmpl_header']} (Δ{s['col_delta']:+d}) "
                                 f"· identical={s['identical']}")
            if f.get("tmpl_quality") and (f["tmpl_quality"]["flag_tokens"] or f["tmpl_quality"]["lower_headings"] or f["tmpl_quality"]["fr_stopword_cells"]):
                q = f["tmpl_quality"]
                sq = f["src_quality"]
                out_lines.append(f"- **Stale `csv` quality** : flag_tokens={q['flag_tokens']} (src={sq['flag_tokens']}) "
                                 f"· lower_headings={q['lower_headings']} (src={sq['lower_headings']}) "
                                 f"· fr_stopword_cells={q['fr_stopword_cells']} (src={sq['fr_stopword_cells']})")
                if q["examples"]:
                    out_lines.append(f"  - examples: {' | '.join(q['examples'])}")
            out_lines.append(f"- **Note** : {f['note']}")
            out_lines.append("")

    # Recommendations
    out_lines.append("## Recommendations")
    out_lines.append("")
    out_lines.append("**Échelle** : HIGH > MEDIUM > LOW > NONE")
    out_lines.append("")
    out_lines.append("- **HIGH** : `csv` embeddé a un delta de lignes ou colonnes significatif. → resync **ou** marqueur sibling.")
    out_lines.append("  - Marqueur = ajouter dans le JSON template un champ `_csv_note: \"stale since YYYY-MM-DD, render truth = <csv_path>\"` (subtractif, non-permissif).")
    out_lines.append("  - Resync = regénérer la clé `csv` du template depuis le CSV source (risque drift, large diff).")
    out_lines.append("- **MEDIUM** : à examiner au cas par cas (delta row 1-5, headers OK).")
    out_lines.append("- **LOW** : 0-2 lignes éditées (typo/casing), latent quasi nul. → simple note commit.")
    out_lines.append("- **NONE** : template aligné sur CSV source (peu probable, à confirmer).")
    out_lines.append("- **N/A** : Back/DataSet=None → template = source, rien à auditer.")
    out_lines.append("")
    out_lines.append("**Décision par défaut (ai-01 penche)** : **marqueur** plutôt que resync, car la clé `csv` est morte")
    out_lines.append("pour les Faces — pas de raison de la maintenir fidèle. Le marqueur documente l'intention.")
    out_lines.append("")
    out_lines.append("## Hors-scope (rappel)")
    out_lines.append("")
    out_lines.append("- ⛔ Pas de resync ni write prod ce tick (post-tag, gated jsboige).")
    out_lines.append("- ⛔ #415 history-rewrite INTERDIT en autonome.")
    out_lines.append("- ⛔ #202 écriture sans GO registre jsboige (mais ce ticket ne touche pas #202).")
    out_lines.append("- Le présent audit est **read-only** (lit fichiers, écrit rapport markdown optionnel).")
    out_lines.append("")

    text = "\n".join(out_lines)
    if args.out:
        out_path = os.path.join(BASE, args.out)
        with open(out_path, "w", encoding="utf-8") as f:
            f.write(text)
        print(f"Written: {out_path}")
    else:
        print(text)

if __name__ == "__main__":
    main()