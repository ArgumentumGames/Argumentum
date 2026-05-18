#!/usr/bin/env python3
"""
Cleanup whitespace in CSV cells across Argumentum datasets (surgical).

Surgical approach: parse CSV to identify cells needing change, then do
byte-level string replacement on the original file. This preserves original
quoting, line endings, and minimizes diff churn (per memory:
feedback_csv_surgical_edits).

Rules:
- Collapse runs of horizontal whitespace ([ \\t]+) into single space.
- Trim leading/trailing horizontal whitespace per line.
- Preserve newlines and paragraph structure.

Rules CSV excluded: Text_* columns contain markdown with intentional
two-space line-breaks (21 cells) that would be destroyed.

Usage:
    python tools/cleanup-whitespace.py [--check]
"""
import csv
import io
import re
import sys
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent

DATASETS = {
    'Fallacies': {
        'path': REPO_ROOT / 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv',
        'text_cols': None,  # auto: suffix _fr/_en/_ru/_pt
    },
    'Virtues': {
        'path': REPO_ROOT / 'Cards/Fallacies/Argumentum Virtues - Taxonomy.csv',
        'text_cols': None,
    },
    'Scenarii': {
        'path': REPO_ROOT / 'Cards/Scenarii/Argumentum Scenarii - Cards.csv',
        'text_cols': [
            'catégorie', 'sous-catégorie', 'titre', 'baratineur', 'piocheur',
            'contexte', 'enjeu', 'suggestion',
            'category', 'subcategory', 'title', 'smoothTalker', 'drawer',
            'context', 'issue', 'suggestion_en',
            'category_ru', 'subcategory_ru', 'title_ru', 'smoothTalker_ru',
            'drawer_ru', 'context_ru', 'issue_ru', 'suggestion_ru',
            'category_pt', 'subcategory_pt', 'title_pt', 'smoothTalker_pt',
            'drawer_pt', 'context_pt', 'issue_pt', 'suggestion_pt',
        ],
    },
}

LANG_SUFFIXES = ('_fr', '_en', '_ru', '_pt')


def detect_text_cols(cols):
    out = []
    for c in cols:
        if any(c.endswith(s) for s in LANG_SUFFIXES) and 'camelcase' not in c.lower():
            out.append(c)
    return out


def normalize_cell(val: str) -> str:
    if not val:
        return val
    new = re.sub(r'[ \t]+', ' ', val)
    new = '\n'.join(line.strip(' \t') for line in new.split('\n'))
    return new


def find_replacements(path: Path, text_cols_override):
    """Return list of (old_quoted_field_bytes, new_quoted_field_bytes) tuples."""
    raw = path.read_bytes()
    text = raw.decode('utf-8-sig')
    reader = csv.reader(io.StringIO(text))
    header = next(reader)
    text_cols = text_cols_override or detect_text_cols(header)
    col_idx = [header.index(c) for c in text_cols if c in header]

    pairs = []  # (old_val, new_val) raw cell strings
    seen = set()  # avoid dup replacements
    for row in reader:
        for idx in col_idx:
            if idx >= len(row):
                continue
            old = row[idx]
            new = normalize_cell(old)
            if old != new:
                key = (old, new)
                if key not in seen:
                    seen.add(key)
                    pairs.append((old, new))
    return pairs, raw


def quote_field(val: str) -> str:
    """Quote a CSV field as csv module would when writing."""
    out = io.StringIO()
    csv.writer(out, quoting=csv.QUOTE_MINIMAL).writerow([val])
    # writerow appends \r\n; trim
    return out.getvalue().rstrip('\r\n')


def apply_replacements(raw: bytes, pairs, dry_run=False):
    """Apply byte-level replacements. Returns (new_bytes, applied_count)."""
    text = raw.decode('utf-8-sig')
    applied = 0
    for old, new in pairs:
        # Build candidate quoted forms for old and new
        old_q = quote_field(old)
        new_q = quote_field(new)
        # Surround by field boundary chars to be safe: comma or newline before/after
        # Try the most-specific patterns first:
        # CASE 1: ,<quoted>,  → between commas
        # CASE 2: ,<quoted>\n → end of line
        # CASE 3: \n<quoted>, → start of line (rare for our text fields)
        patterns_old = [
            ',' + old_q + ',',
            ',' + old_q + '\n',
            ',' + old_q + '\r\n',
        ]
        patterns_new = [
            ',' + new_q + ',',
            ',' + new_q + '\n',
            ',' + new_q + '\r\n',
        ]
        for po, pn in zip(patterns_old, patterns_new):
            count = text.count(po)
            if count > 0:
                text = text.replace(po, pn)
                applied += count
    return text.encode('utf-8'), applied


def process_dataset(name, cfg, check_only=False):
    path = cfg['path']
    if not path.exists():
        print(f'[SKIP] {name}: file not found ({path})')
        return

    pairs, raw = find_replacements(path, cfg.get('text_cols'))
    if not pairs:
        print(f'{name}: no changes needed.')
        return

    # Quick cell-level count
    text = raw.decode('utf-8-sig')
    reader = csv.reader(io.StringIO(text))
    header = next(reader)
    text_cols = cfg.get('text_cols') or detect_text_cols(header)
    col_idx = [header.index(c) for c in text_cols if c in header]
    cell_changes = 0
    for row in reader:
        for idx in col_idx:
            if idx < len(row) and normalize_cell(row[idx]) != row[idx]:
                cell_changes += 1

    print(f'{name}: {cell_changes} cells need cleanup ({len(pairs)} unique old→new pairs)')

    if check_only:
        return

    # Iterate until convergence (rare cells where multiple values collide on
    # the same row need a second pass).
    total_applied = 0
    iterations = 0
    while pairs and iterations < 5:
        new_bytes, applied = apply_replacements(raw, pairs)
        path.write_bytes(new_bytes)
        total_applied += applied
        iterations += 1
        pairs, raw = find_replacements(path, cfg.get('text_cols'))

    print(f'  applied: {total_applied} replacement(s) over {iterations} pass(es)')
    print(f'  written: {path.relative_to(REPO_ROOT)}')


def main():
    check_only = '--check' in sys.argv
    print(f'Mode: {"CHECK (no write)" if check_only else "APPLY"}\n')
    for name, cfg in DATASETS.items():
        process_dataset(name, cfg, check_only=check_only)
        print()


if __name__ == '__main__':
    main()
