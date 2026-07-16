#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Byte-check contamination/BOM on Rules + Scenarii CSVs (8 lang cols).

Reports: BOM, CRLF, cell-level contamination (FR rows in _en/_ru/etc.), and
count of populated vs empty per lang column. Read-only, no write.
"""
import csv, sys, collections, os

# (label, path, lang-fields-to-audit)
TARGETS = [
    ("Rules", r"Cards\Rules\Argumentum Rules - Cards.csv"),
    ("Rules PnP", r"Cards\Rules\Argumentum Rules - Cards Print and Play.csv"),
    ("Scenarii", r"Cards\Scenarii\Argumentum Scenarii - Cards.csv"),
]

# Per-lang script-range expectations (Unicode blocks)
# FR contamination = characters from Latin-1 range (à, é, è, ê, ç) appearing
# in lang columns where the expected script is Cyrillic, Arabic, or CJK.
LANG_SCRIPT_RANGES = {
    "ru": (0x0400, 0x04FF),    # Cyrillic
    "ar": (0x0600, 0x06FF),    # Arabic
    "fa": (0x0600, 0x06FF),    # Arabic (Farsi reuses Arabic block + extensions)
    "zh": (0x4E00, 0x9FFF),    # CJK Unified Ideographs (main block)
}
# FR-specific Latin characters that should NEVER appear in ru/ar/fa/zh columns
FR_LATIN_MARKERS = set("àâäçéèêëîïôùûÿœæÀÂÄÇÉÈÊËÎÏÔÙÛŸŒÆ")


def detect_encoding(path):
    with open(path, "rb") as f:
        raw = f.read(64)
    bom = "BOM" if raw[:3] == b"\xef\xbb\xbf" else "no-BOM"
    crlf = "CRLF" if b"\r\n" in raw else "LF"
    return bom, crlf


def script_coverage(text):
    """Return dict {script_name: count} for characters in text."""
    out = collections.Counter()
    for ch in text:
        cp = ord(ch)
        if 0x41 <= cp <= 0x5A or 0x61 <= cp <= 0x7A:
            out["latin"] += 1
        elif 0x00C0 <= cp <= 0x024F:
            out["latin_ext"] += 1
        elif 0x0400 <= cp <= 0x04FF:
            out["cyrillic"] += 1
        elif 0x0600 <= cp <= 0x06FF:
            out["arabic"] += 1
        elif 0x4E00 <= cp <= 0x9FFF:
            out["cjk"] += 1
        elif cp < 0x20 or 0x7F <= cp <= 0xA0:
            out["ctrl"] += 1
        else:
            out["other"] += 1
    return out


def main():
    findings = []
    print("## Byte-check — Rules + Scenarii (read-only audit, 0 write)\n")
    for label, path in TARGETS:
        if not os.path.exists(path):
            print(f"{label} | MISSING: {path}")
            continue
        bom, eol = detect_encoding(path)
        with open(path, encoding="utf-8-sig", newline="") as f:
            rows = list(csv.reader(f))
        header = rows[0]
        print(f"### {label} | `{path}`")
        print(f"  encoding: {bom} | {eol} | rows={len(rows)-1} cols={len(header)}\n")

        # For each lang col, check script coverage + FR markers
        for lang in ["en", "ru", "pt", "es", "ar", "fa", "zh"]:
            lang_cols = [(i, c) for i, c in enumerate(header) if c.endswith(f"_{lang}")]
            if not lang_cols:
                continue
            populated = 0
            empty = 0
            fr_marker_hits = []
            script_totals = collections.Counter()
            for r in rows[1:]:
                for i, col_name in lang_cols:
                    v = r[i].strip() if i < len(r) else ""
                    if not v:
                        empty += 1
                        continue
                    populated += 1
                    script_totals.update(script_coverage(v))
                    # FR-specific markers (à é è ç...) → contamination in cyrillic/arabic/cjk
                    if lang in LANG_SCRIPT_RANGES:
                        bad_chars = [c for c in v if c in FR_LATIN_MARKERS]
                        if bad_chars:
                            fr_marker_hits.append((r[0], col_name, "".join(sorted(set(bad_chars))), v[:60]))
            total_chars = sum(script_totals.values())
            script_pct = {k: f"{v/total_chars*100:.0f}%" for k, v in script_totals.items() if v > 0} if total_chars else {}
            fr_marker_n = len(fr_marker_hits)
            line = f"  _{lang} ({len(lang_cols)} cols): populated={populated} empty={empty}"
            if script_pct:
                line += f" | scripts={script_pct}"
            if fr_marker_n:
                line += f" | FR_MARKERS={fr_marker_n}"
            print(line)
            if fr_marker_hits:
                findings.append((label, lang, fr_marker_hits[:5]))

    print("\n## FR-marker contamination findings (top 5 per (dataset, lang))\n")
    if not findings:
        print("**0 findings.** FR-specific Latin characters (à, é, ç, etc.) absent from all cyrillic/arabic/cjk columns.\n")
    else:
        print(f"**{len(findings)} (dataset, lang) pairs with potential FR contamination.**\n")
        for label, lang, samples in findings:
            print(f"### {label} / _{lang}")
            for pk, col, chars, preview in samples:
                print(f"  - PK {pk} | {col} | markers={chars!r} | preview: `{preview}`")
            print()


if __name__ == "__main__":
    main()