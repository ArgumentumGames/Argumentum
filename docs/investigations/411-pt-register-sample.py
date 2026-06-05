#!/usr/bin/env python3
"""
#411 PT Register Sample: gpt-5.5 modernization of 5 Fallacies records.

Sample records: PK=139, 176, 360, 459, 817
Represents Low/Medium/High severity archaisms.

Usage:
  python3 411-pt-register-sample.py           # dry-run
  python3 411-pt-register-sample.py --apply   # write CSV
"""

import csv
import json
import sys
import io
import re
import time
from collections import Counter
from openai import OpenAI

# ── Config ──────────────────────────────────────────────────────────
KEY_PATH = r"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt"
MODEL = "gpt-5.5"
CSV_PATH = r"..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv"

# Sample PKs representing the range
SAMPLE_PKS = ["139", "176", "360", "459", "817"]
PT_FIELDS = ["desc_pt", "example_pt"]

# ── Load key ────────────────────────────────────────────────────────
with open(KEY_PATH, "r", encoding="utf-8") as f:
    api_key = f.read().strip()

client = OpenAI(api_key=api_key)

# ── Load CSV ────────────────────────────────────────────────────────
with open(CSV_PATH, "r", encoding="utf-8-sig") as f:
    rows = list(csv.DictReader(f, delimiter=","))
    fieldnames = list(rows[0].keys())

pk_index = {row["PK"]: row for row in rows}

apply_mode = "--apply" in sys.argv
print(f"Mode: {'APPLY' if apply_mode else 'DRY-RUN'} | Model: {MODEL}")
print(f"Sample PKs: {SAMPLE_PKS}")
print()

results = []
for pk in SAMPLE_PKS:
    row = pk_index.get(pk)
    if not row:
        print(f"PK={pk}: NOT FOUND")
        continue

    name = row.get("nom_vulgarisé", "")
    print(f"--- PK={pk} | {name} ---")

    # Build the PT text to modernize
    texts = {}
    for field in PT_FIELDS:
        val = row.get(field, "").strip()
        if val:
            texts[field] = val

    if not texts:
        print("  No PT text to process")
        continue

    # Show original
    for field, val in texts.items():
        print(f"  {field} (original): {val[:150]}...")

    prompt = f"""You are a professional Portuguese translator. Modernize the following Brazilian Portuguese text by replacing ALL archaic European Portuguese forms with their modern Brazilian equivalents.

Rules:
- Replace "vós" → "vocês" (or restructure naturally)
- Replace "vosso/vossa" → "seu/sua" (agreeing with the following noun gender)
- Replace archaic verb forms (dizeis, sois, sabeis, podeis, vedes, lembrais) → modern 3rd person plural
- Preserve the EXACT meaning and register of the original
- Keep the same sentence structure where possible
- Do NOT change any other words

Texts to modernize:
"""
    for i, (field, val) in enumerate(texts.items(), 1):
        prompt += f"\n[{i}] {val}\n"

    prompt += "\nRespond in JSON:\n{\"modernized\": {"
    for i, field in enumerate(texts.keys(), 1):
        if i > 1:
            prompt += ", "
        prompt += f"\"{field}\": \"the modernized text\""
    prompt += "}}"

    try:
        response = client.chat.completions.create(
            model=MODEL,
            messages=[{"role": "user", "content": prompt}],
            response_format={"type": "json_object"},
            temperature=0.1,
            max_tokens=1000,
        )
        result = json.loads(response.choices[0].message.content)
        modernized = result.get("modernized", {})

        for field, val in texts.items():
            new_val = modernized.get(field, val)
            changed = new_val != val
            marker = "✅ CHANGED" if changed else "⚠️ UNCHANGED"
            print(f"  {field} ({marker}): {new_val[:150]}...")

        results.append({
            "PK": pk,
            "name": name,
            "original": texts,
            "modernized": modernized,
        })

    except Exception as e:
        print(f"  ❌ API error: {e}")
        results.append({"PK": pk, "error": str(e)})

    print()
    time.sleep(1)

# ── Save results ───────────────────────────────────────────────────
results_path = "411-pt-register-sample-results.json"
with open(results_path, "w", encoding="utf-8") as f:
    json.dump(results, f, ensure_ascii=False, indent=2)
print(f"Results saved to {results_path}")

# ── Apply if requested ─────────────────────────────────────────────
if apply_mode:
    changes = 0
    for r in results:
        if "error" in r or "modernized" not in r:
            continue
        row = pk_index.get(r["PK"])
        if not row:
            continue
        for field, new_val in r["modernized"].items():
            if row.get(field, "").strip() != new_val.strip():
                row[field] = new_val
                changes += 1

    output = io.StringIO()
    writer = csv.DictWriter(output, fieldnames=fieldnames, lineterminator="\n")
    writer.writeheader()
    writer.writerows(rows)
    with open(CSV_PATH, "w", encoding="utf-8") as f:
        f.write(output.getvalue())
    print(f"\nApplied {changes} changes to CSV")
else:
    print("\nDry-run mode. Use --apply to write changes.")
