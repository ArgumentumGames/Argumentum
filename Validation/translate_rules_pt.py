#!/usr/bin/env python3
"""
Translate Argumentum Rules from FR to PT using OpenAI GPT-5.4-mini.

Issue #211: PT Rules have catastrophic MT errors that need full retranslation.
This script:
1. Reads Cards/Rules/Argumentum Rules - Cards.csv
2. Calls OpenAI API to translate each row's Text (FR) → Text_pt (PT)
3. Saves the updated CSV
4. Preserves all other columns (Text_en, Text_ru, print_and_play)

Usage:
    python Validation/translate_rules_pt.py [--dry-run] [--rows 3]
"""

import argparse
import csv
import os
import sys
import time
from pathlib import Path

from openai import OpenAI

CSV_PATH = Path(__file__).resolve().parent.parent / "Cards/Rules/Argumentum Rules - Cards.csv"
KEY_PATH = Path(r"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt")
MODEL = "gpt-5.4-mini"

SYSTEM_PROMPT = """\
You are a professional translator specializing in board game rulebooks.
Translate the following French game rules text to European Portuguese.

Requirements:
- Preserve ALL markdown formatting (headers, lists, bold, italic, separators)
- Use European Portuguese (pt-PT), NOT Brazilian Portuguese
- Keep game-specific terms consistent:
  - "Argumentum" stays as-is (brand name)
  - "Baratineur" stays as-is (game role)
  - "Bingo argumentatif" → "Bingo argumentativo"
  - "Mixologue" stays as-is (game role)
  - Card family names in French are proper nouns — keep them in French
- Translate naturally and idiomatically, NOT word-by-word
- Preserve emoji and symbols exactly (🎲, 🃏, 🎭, etc.)
- Output ONLY the translated Portuguese text, no explanations
"""

USER_PROMPT_TEMPLATE = """\
Translate this French board game rule text to European Portuguese:

---
{french_text}
---

Portuguese translation:"""


def main():
    parser = argparse.ArgumentParser(description="Translate Argumentum Rules FR→PT")
    parser.add_argument("--dry-run", action="store_true", help="Show what would be translated without calling API")
    parser.add_argument("--rows", type=int, default=0, help="Only translate first N rows (0=all)")
    parser.add_argument("--model", default=MODEL, help=f"OpenAI model (default: {MODEL})")
    parser.add_argument("--start-row", type=int, default=0, help="Start from row index (0-based)")
    args = parser.parse_args()

    # Load API key
    if not KEY_PATH.exists():
        print(f"ERROR: API key file not found: {KEY_PATH}", file=sys.stderr)
        sys.exit(1)
    api_key = KEY_PATH.read_text(encoding="utf-8").strip()

    # Read CSV
    if not CSV_PATH.exists():
        print(f"ERROR: CSV file not found: {CSV_PATH}", file=sys.stderr)
        sys.exit(1)

    with open(CSV_PATH, encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f)
        fieldnames = reader.fieldnames
        rows = list(reader)

    total = len(rows)
    end_row = min(args.rows, total) if args.rows > 0 else total
    rows_to_translate = range(args.start_row, end_row)

    print(f"CSV: {CSV_PATH.name}")
    print(f"Total rows: {total}")
    print(f"Rows to translate: {len(list(rows_to_translate))} (indices {args.start_row}-{end_row - 1})")
    print(f"Model: {args.model}")
    print()

    if args.dry_run:
        for i in rows_to_translate:
            text_fr = rows[i].get("Text", "")
            text_pt_current = rows[i].get("Text_pt", "")
            print(f"--- Row {i+1} ---")
            print(f"FR (first 100 chars): {text_fr[:100]}...")
            print(f"Current PT (first 100 chars): {text_pt_current[:100]}...")
            print()
        print("DRY RUN - no API calls made.")
        return

    client = OpenAI(api_key=api_key)
    translated = 0
    errors = 0

    for i in rows_to_translate:
        text_fr = rows[i].get("Text", "").strip()
        if not text_fr:
            print(f"Row {i+1}: EMPTY - skipping")
            continue

        print(f"Row {i+1}/{total}: Translating ({len(text_fr)} chars)...", end=" ", flush=True)
        start = time.time()

        try:
            response = client.chat.completions.create(
                model=args.model,
                messages=[
                    {"role": "system", "content": SYSTEM_PROMPT},
                    {"role": "user", "content": USER_PROMPT_TEMPLATE.format(french_text=text_fr)},
                ],
                temperature=0.3,
                max_completion_tokens=4096,
            )
            translated_text = response.choices[0].message.content.strip()
            elapsed = time.time() - start

            rows[i]["Text_pt"] = translated_text
            translated += 1
            print(f"OK ({elapsed:.1f}s, {len(translated_text)} chars)")

            # Small delay to avoid rate limits
            if i < end_row - 1:
                time.sleep(1)

        except Exception as e:
            errors += 1
            print(f"ERROR: {e}")

    if errors > 0:
        print(f"\n{errors} errors occurred. Review before saving.")

    if translated == 0:
        print("No rows translated. Exiting.")
        return

    # Save backup
    backup_path = CSV_PATH.with_suffix(".csv.bak")
    if not backup_path.exists():
        import shutil
        shutil.copy2(CSV_PATH, backup_path)
        print(f"\nBackup saved: {backup_path}")

    # Write updated CSV
    with open(CSV_PATH, "w", encoding="utf-8-sig", newline="") as f:
        writer = csv.DictWriter(f, fieldnames=fieldnames)
        writer.writeheader()
        writer.writerows(rows)

    print(f"\nDone! {translated} rows translated, {errors} errors.")
    print(f"Updated CSV: {CSV_PATH}")


if __name__ == "__main__":
    main()
