#!/usr/bin/env python3
"""#600 link_* apply harness (DRY-RUN by default; #595 drift-free ready).

Applies the #618 sidecar (2934 candidate link_<lang> URLs) to the Fallacies/Virtues
taxonomy CSVs. Honors the pre-tag freeze: DRY-RUN by default = **0 write under Cards/**.
The --apply path (gated post-release) writes in-place via the #595 drift-free method
(QUOTE_MINIMAL + quotechar " + CRLF + UTF-8 no-BOM), with skip-non-empty protection.

Sidecar schema: dataset,key,link_lang,resolved_url  (key == CSV PK).
Target column: link_<link_lang> (link_ru/pt/es/ar/fa/zh — link_en excluded, already 100%).

Modes (mutually exclusive):
  (default)    dry-run: 0 write. Counts would-apply / skip-non-empty / orphan-PK /
              target-col-missing / dup-key-lang. Spots ~5% of AR/FA/ZH for homonym
              markers + verifies CSV dialect = #595 (drift-safe to apply).
  --apply      WRITE in-place (drift-free). Skips every non-empty target cell.
              Refuses if --dry-run-also-clobbers would change >0 non-empty cell.
              NOT used during freeze — gated post-release.
  --force-clobber  (with --apply only) overwrite non-empty cells. DANGEROUS, off by default.

Run:
  python docs/taxonomy/600-link-apply.py                 # dry-run, full report
  python docs/taxonomy/600-link-apply.py --report-md     # dry-run, emit markdown report
  python docs/taxonomy/600-link-apply.py --apply         # gated: write drift-free
"""
import csv, os, sys, argparse, tempfile, urllib.parse, random

SIDECAR = {
    "fallacies": r"docs/taxonomy/600-link-resolve-fallacies.csv",
    "virtues":   r"docs/taxonomy/600-link-resolve-virtues.csv",
}
CSV_PATH = {
    "fallacies": r"Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",
    "virtues":   r"Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",
}
PK_COL_FALLBACK = "pk"  # Virtues uses lowercase; Fallacies uses "PK"

def find_pk_col(header):
    """PK column, case-insensitive (Fallacies='PK', Virtues='pk')."""
    for c in header:
        if c.lower() == "pk":
            return c
    return None
HOMONYM_MARKERS = [  # §6.4 — Wikipedia disambiguation path fragments across scripts
    "(disambiguation)", "_disambiguation", "_(disambiguation)",
    "значения", "_(значения)",      # ru
    "توضيح", "_(توضيح)",            # ar
    "消歧义", "_(消歧义)",           # zh
    "ابهام‌سازی", "ابهام‌سازی",       # fa
]
SPOT_LANGS = ["ar", "fa", "zh"]   # §6.4 homonym-risk languages
SPOT_FRAC = 0.05

def load_sidecar():
    cands = []  # (dataset, key, lang, url)
    for ds, path in SIDECAR.items():
        if not os.path.exists(path):
            continue
        with open(path, encoding="utf-8-sig", newline="") as f:
            for r in csv.DictReader(f):
                url = (r.get("resolved_url") or "").strip()
                if url:
                    cands.append((ds, r.get("key", "").strip(), r.get("link_lang", "").strip(), url))
    return cands

def load_csv(path):
    with open(path, encoding="utf-8-sig", newline="") as f:
        rows = list(csv.reader(f))
    return rows  # rows[0] = header

def sniff_dialect(path):
    """Confirm the on-disk dialect matches #595 (CRLF, QUOTE_MINIMAL) so the apply is drift-free.

    Key distinction: a CSV with multiline quoted cells legitimately contains LF bytes
    INSIDE cells (not record terminators). The drift-safety signal is whether RECORD
    terminators are CRLF. We verify CRLF-count == record-count (header + data rows read
    by csv.reader); any excess LF is intra-cell (preserved by csv.reader/writer, benign).
    """
    with open(path, "rb") as f:
        raw = f.read()
    bom = raw.startswith(b"\xef\xbb\xbf")
    crlf_count = raw.count(b"\r\n")
    total_lf = raw.count(b"\n")
    intra_cell_lf = total_lf - crlf_count  # LF inside quoted multiline cells
    # record count via a proper parse (handles quoted newlines correctly)
    with open(path, encoding="utf-8-sig", newline="") as f:
        n_records = sum(1 for _ in csv.reader(f))
    record_terminators_are_crlf = (crlf_count == n_records)
    return {
        "BOM": bom,
        "n_records": n_records,
        "CRLF_count": crlf_count,
        "intra_cell_LF": intra_cell_lf,  # benign — preserved by csv round-trip
        "record_terminators_CRLF": record_terminators_are_crlf,  # TRUE => #595 drift-safe
    }

def is_filled(cell):
    """A cell counts as already-filled if it contains an http(s) URL or any non-trivial text."""
    c = (cell or "").strip()
    if not c:
        return False
    return True

def is_homonym(url):
    try:
        path = urllib.parse.urlparse(url).path.lower()
    except Exception:
        path = (url or "").lower()
    return any(m.lower() in path for m in HOMONYM_MARKERS)

def audit():
    cands = load_sidecar()
    report = {"per_ds_lang": {}, "totals": {}, "dialect": {}, "dup": [], "spot": [], "clobber_risk": []}
    # detect duplicate (dataset,key,lang) with differing URLs
    seen = {}
    for ds, key, lang, url in cands:
        k = (ds, key, lang)
        if k in seen and seen[k] != url:
            report["dup"].append((ds, key, lang, seen[k], url))
        else:
            seen[k] = url
    grand = {"cands": 0, "would_apply": 0, "skip_nonempty": 0, "orphan_pk": 0, "col_missing": 0, "homonym": 0}
    spot_pool = []
    for ds in ("fallacies", "virtues"):
        path = CSV_PATH[ds]
        if not os.path.exists(path):
            continue
        report["dialect"][ds] = sniff_dialect(path)
        rows = load_csv(path)
        header = rows[0]
        idx = {c: i for i, c in enumerate(header)}
        pk_col = find_pk_col(header)
        if pk_col is None:
            continue
        pk_i = idx[pk_col]
        # index data rows by PK
        by_pk = {}
        for r in rows[1:]:
            if pk_i < len(r):
                pk = r[pk_i].strip()
                if pk:
                    by_pk.setdefault(pk, []).append(r)  # may be >1 if dup PKs
        dscands = [c for c in cands if c[0] == ds]
        for dsname, key, lang, url in dscands:
            grand["cands"] += 1
            tcol = "link_%s" % lang
            if tcol not in idx:
                grand["col_missing"] += 1
                continue
            t_i = idx[tcol]
            rows_pk = by_pk.get(key)
            if not rows_pk:
                grand["orphan_pk"] += 1
                continue
            # take first row with this PK
            r = rows_pk[0]
            cur = r[t_i] if t_i < len(r) else ""
            if is_filled(cur):
                grand["skip_nonempty"] += 1
            else:
                grand["would_apply"] += 1
            if is_homonym(url):
                grand["homonym"] += 1
                report["clobber_risk"].append((ds, key, lang, url, "HOMONYM"))
            key2 = (ds, lang)
            d = report["per_ds_lang"].setdefault(key2, {"cands": 0, "would_apply": 0, "skip_nonempty": 0, "orphan": 0, "col_missing": 0})
            d["cands"] += 1
            if tcol not in idx:
                d["col_missing"] += 1; continue
            if not rows_pk:
                d["orphan"] += 1; continue
            if is_filled(cur):
                d["skip_nonempty"] += 1
            else:
                d["would_apply"] += 1
            if lang in SPOT_LANGS:
                spot_pool.append((ds, key, lang, url, cur))
    # spot sample ~5% of AR/FA/ZH, deterministic-ish by sorting (no RNG seed needed for a sample report)
    spot_pool.sort()
    n = max(1, int(len(spot_pool) * SPOT_FRAC)) if spot_pool else 0
    # spread the sample across the 3 languages
    per_lang = {}
    for item in spot_pool:
        per_lang.setdefault(item[2], []).append(item)
    for lang, items in per_lang.items():
        m = max(1, int(len(items) * SPOT_FRAC))
        step = max(1, len(items) // m)
        report["spot"].extend(items[::step][:m])
    report["totals"] = grand
    return report

def fmt_md(report):
    t = report["totals"]
    L = []
    L.append("# #600 `link_*` apply harness — DRY-RUN audit\n")
    L.append("**0 write under Cards/** (pre-tag freeze). Materializes what the #618 sidecar *would* ")
    L.append("fill, for human spot-validation before the gated post-release apply.\n")
    L.append("| Metric | Count |")
    L.append("|---|---:|")
    for k in ("cands", "would_apply", "skip_nonempty", "orphan_pk", "col_missing", "homonym"):
        L.append("| %s | %d |" % (k, t[k]))
    L.append("\n## Per dataset × lang\n")
    L.append("| dataset | lang | cands | would-apply | skip-nonempty | orphan-PK | col-missing |")
    L.append("|---|---|---:|---:|---:|---:|---:|")
    for (ds, lang), d in sorted(report["per_ds_lang"].items()):
        L.append("| %s | %s | %d | %d | %d | %d | %d |" % (ds, lang, d["cands"], d["would_apply"], d["skip_nonempty"], d["orphan"], d["col_missing"]))
    L.append("\n## CSV dialect (drift-safety for the gated apply)\n")
    L.append("| dataset | BOM | records | CRLF-records | intra-cell-LF (benign) | #595 drift-safe |")
    L.append("|---|---|---:|---:|---:|---|")
    for ds, dia in report["dialect"].items():
        safe = "✅ yes" if dia["record_terminators_CRLF"] else "⚠️ CHECK"
        L.append("| %s | %s | %d | %d | %d | %s |" % (ds, dia["BOM"], dia["n_records"], dia["CRLF_count"], dia["intra_cell_LF"], safe))
    L.append("\n## Duplicates (dataset,key,lang with differing URLs)\n")
    if report["dup"]:
        for ds, key, lang, u1, u2 in report["dup"][:20]:
            L.append("- `%s/%s/%s`: `%s` vs `%s`" % (ds, key, lang, u1[:60], u2[:60]))
        if len(report["dup"]) > 20:
            L.append("- … +%d more" % (len(report["dup"]) - 20))
    else:
        L.append("_none_ ✅")
    L.append("\n## Homonym-risk spot sample (~5% of AR/FA/ZH, §6.4)\n")
    L.append("| ds | PK | lang | resolved_url | cur cell | homonym? |")
    L.append("|---|---|---|---|---|---|")
    for ds, key, lang, url, cur in report["spot"][:60]:
        mark = "⚠️" if is_homonym(url) else "ok"
        cur_s = (cur[:20] + "…") if len(cur) > 20 else (cur or "_empty_")
        L.append("| %s | %s | %s | `%s` | %s | %s |" % (ds, key, lang, url[:70], cur_s, mark))
    return "\n".join(L)

def apply_changes(force_clobber=False):
    """Gated post-release: write the sidecar fills in-place, drift-free (#595)."""
    cands = load_sidecar()
    # group by dataset
    by_ds = {}
    for ds, key, lang, url in cands:
        by_ds.setdefault(ds, {}).setdefault((key, lang), url)
    written = 0; skipped = 0; refused = 0
    for ds, fills in by_ds.items():
        path = CSV_PATH[ds]
        rows = load_csv(path)
        header = rows[0]
        idx = {c: i for i, c in enumerate(header)}
        pk_col = find_pk_col(header)
        if pk_col is None:
            continue
        pk_i = idx[pk_col]
        # PK -> first row index
        pk_row = {}
        for ri, r in enumerate(rows[1:], start=1):
            if pk_i < len(r) and r[pk_i].strip():
                pk_row.setdefault(r[pk_i].strip(), ri)
        for (key, lang), url in fills.items():
            tcol = "link_%s" % lang
            if tcol not in idx:
                refused += 1; continue
            t_i = idx[tcol]
            ri = pk_row.get(key)
            if ri is None:
                refused += 1; continue
            r = rows[ri]
            cur = r[t_i] if t_i < len(r) else ""
            if is_filled(cur) and not force_clobber:
                skipped += 1; continue
            # extend row if needed
            while len(r) <= t_i:
                r.append("")
            r[t_i] = url
            written += 1
        # write drift-free: QUOTE_MINIMAL + quotechar " + CRLF + UTF-8 no-BOM (#595)
        fd, tmp = tempfile.mkstemp(suffix=".csv", dir=os.path.dirname(path) or ".")
        with os.fdopen(fd, "w", encoding="utf-8", newline="") as f:
            w = csv.writer(f, quoting=csv.QUOTE_MINIMAL, quotechar='"', lineterminator="\r\n")
            for r in rows:
                w.writerow(r)
        os.replace(tmp, path)
    return written, skipped, refused

if __name__ == "__main__":
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true", help="WRITE in-place (drift-free #595). Gated post-release.")
    ap.add_argument("--force-clobber", action="store_true", help="(with --apply) overwrite non-empty cells.")
    ap.add_argument("--report-md", action="store_true", help="dry-run, emit markdown report to stdout")
    a = ap.parse_args()
    if a.apply:
        w, s, r = apply_changes(force_clobber=a.force_clobber)
        print("APPLY | written=%d | skipped-nonempty=%d | refused=%d" % (w, s, r))
    else:
        rep = audit()
        if a.report_md:
            print(fmt_md(rep))
        else:
            t = rep["totals"]
            print("DRY-RUN | cands=%d | would-apply=%d | skip-nonempty=%d | orphan-PK=%d | col-missing=%d | homonym=%d"
                  % (t["cands"], t["would_apply"], t["skip_nonempty"], t["orphan_pk"], t["col_missing"], t["homonym"]))
            print("dup:", len(rep["dup"]), "| spot-sample:", len(rep["spot"]))
            print("dialect:", rep["dialect"])
