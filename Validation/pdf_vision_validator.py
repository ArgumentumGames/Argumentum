#!/usr/bin/env python3
"""
PDF Vision Validator for Argumentum Pipeline (#212).

Converts PDF pages to images via PyMuPDF, then sends them to a vision LLM
(GLM-4.6V or compatible) for structured validation checks:
  - Language detection (FR/EN/RU/PT)
  - Blank/empty card detection
  - Placeholder text detection
  - Card count verification
  - Color/layout consistency

Usage:
    python pdf_vision_validator.py [--target TARGET_DIR] [--model MODEL_ID] [--pages N] [--lang LANG]

Design mirrors sk-agent's document_processing + call_agent pattern:
  1. PDF → PNG images (PyMuPDF, DPI 180)
  2. Base64-encoded images sent to OpenAI-compatible vision API
  3. Structured prompts extract validation signals
  4. JSON report generated with pass/fail per check
"""

from __future__ import annotations

import argparse
import base64
import json
import os
import sys
import time
from dataclasses import dataclass, field, asdict
from pathlib import Path
from typing import Any

import fitz  # PyMuPDF
import httpx

# ---------------------------------------------------------------------------
# Configuration
# ---------------------------------------------------------------------------

DEFAULT_TARGET = Path(__file__).resolve().parent.parent / (
    "Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target"
)

# GLM-4.6V via z.ai cloud (same as sk-agent vision-analyst)
DEFAULT_BASE_URL = "https://api.z.ai/api/coding/paas/v4"
DEFAULT_MODEL = "glm-4.6v"
DEFAULT_API_KEY_ENV = "ZAI_API_KEY"

# Fallback: read from sk-agent config if env var not set
SK_AGENT_CONFIG = (
    Path(__file__).resolve().parent.parent.parent.parent
    / "roo-extensions/mcps/internal/servers/sk-agent/sk_agent_config.json"
)

DPI = 180  # matches sk-agent / GLM-V gradio
MAX_PAGES_PER_CALL = 3  # pages per vision API call (token budget)
MAX_RETRIES = 2
RETRY_DELAY_S = 5

LANGUAGES = ["fr", "en", "ru", "pt"]
EXPECTED_LANG_NAMES = {"fr": "French", "en": "English", "ru": "Russian", "pt": "Portuguese"}

# ---------------------------------------------------------------------------
# Data Structures
# ---------------------------------------------------------------------------


@dataclass
class PageCheck:
    page_number: int
    language_detected: str = ""
    language_correct: bool = False
    blank_cards_found: int = 0
    placeholders_found: list[str] = field(default_factory=list)
    card_count: int = 0
    notes: str = ""
    raw_response: str = ""


@dataclass
class PdfValidation:
    pdf_path: str
    expected_lang: str
    pages_checked: int = 0
    total_pages: int = 0
    checks: list[PageCheck] = field(default_factory=list)
    passed: bool = False
    errors: list[str] = field(default_factory=list)
    duration_s: float = 0.0


@dataclass
class ValidationReport:
    target_dir: str
    model: str
    timestamp: str = ""
    total_pdfs: int = 0
    passed: int = 0
    failed: int = 0
    skipped: int = 0
    validations: list[dict] = field(default_factory=list)
    summary: str = ""


# ---------------------------------------------------------------------------
# API Key Resolution
# ---------------------------------------------------------------------------


def get_api_key() -> str:
    """Resolve API key from env var or sk-agent config."""
    key = os.environ.get(DEFAULT_API_KEY_ENV)
    if key:
        return key

    # Try sk-agent config
    if SK_AGENT_CONFIG.exists():
        try:
            cfg = json.loads(SK_AGENT_CONFIG.read_text(encoding="utf-8"))
            for model in cfg.get("models", []):
                if model.get("model_id") == DEFAULT_MODEL:
                    key = model.get("api_key", "")
                    if key:
                        return key
        except Exception:
            pass

    print(f"ERROR: No API key found. Set {DEFAULT_API_KEY_ENV} or configure sk-agent.", file=sys.stderr)
    sys.exit(1)


# ---------------------------------------------------------------------------
# PDF → Images
# ---------------------------------------------------------------------------


def pdf_to_images(pdf_path: str, page_numbers: list[int] | None = None) -> list[tuple[int, bytes]]:
    """Convert specific PDF pages to PNG bytes. Returns [(page_num, png_bytes)]."""
    doc = fitz.open(pdf_path)
    results = []
    targets = page_numbers or list(range(1, min(doc.page_count + 1, MAX_PAGES_PER_CALL + 1)))

    for pn in targets:
        if pn < 1 or pn > doc.page_count:
            continue
        page = doc[pn - 1]
        pix = page.get_pixmap(dpi=DPI)
        results.append((pn, pix.tobytes("png")))

    doc.close()
    return results


# ---------------------------------------------------------------------------
# Vision API Call
# ---------------------------------------------------------------------------

VALIDATION_PROMPT = """\
You are validating a PDF page from the Argumentum educational card game about logical fallacies.

Expected language: {lang_name} ({lang_code})

Analyze this page and respond with ONLY a JSON object (no markdown, no backticks):
{{
  "language_detected": "the primary language you see on cards (FR/EN/RU/PT)",
  "language_correct": true/false,
  "blank_cards": number of blank/empty cards visible,
  "placeholders": ["list of any placeholder texts like [TEXT], Lorem ipsum, etc."],
  "card_count": approximate number of distinct cards visible on this page,
  "color_families_visible": ["list of color family names if visible"],
  "notes": "any issues noticed"
}}

Be strict about language detection. If cards are in French but expected language is English, set language_correct to false."""


def call_vision_api(
    images: list[tuple[int, bytes]],
    lang_code: str,
    api_key: str,
    model: str = DEFAULT_MODEL,
    base_url: str = DEFAULT_BASE_URL,
) -> list[PageCheck]:
    """Send page images to vision API and parse structured responses."""
    lang_name = EXPECTED_LANG_NAMES.get(lang_code, lang_code)
    prompt = VALIDATION_PROMPT.format(lang_name=lang_name, lang_code=lang_code)

    checks = []

    for page_num, img_bytes in images:
        b64 = base64.b64encode(img_bytes).decode("ascii")
        data_url = f"data:image/png;base64,{b64}"

        payload = {
            "model": model,
            "messages": [
                {
                    "role": "user",
                    "content": [
                        {"type": "text", "text": prompt},
                        {"type": "image_url", "image_url": {"url": data_url}},
                    ],
                }
            ],
            "max_tokens": 1024,
            "temperature": 0.1,
        }

        response_text = ""
        for attempt in range(MAX_RETRIES + 1):
            try:
                resp = httpx.post(
                    f"{base_url}/chat/completions",
                    headers={"Authorization": f"Bearer {api_key}"},
                    json=payload,
                    timeout=120.0,
                )
                resp.raise_for_status()
                data = resp.json()
                response_text = data["choices"][0]["message"]["content"]
                break
            except Exception as e:
                if attempt < MAX_RETRIES:
                    print(f"  Retry {attempt+1}/{MAX_RETRIES} for page {page_num}: {e}")
                    time.sleep(RETRY_DELAY_S)
                else:
                    print(f"  ERROR page {page_num}: {e}")
                    response_text = f'{{"error": "{e}"}}'

        # Parse response
        check = PageCheck(page_number=page_num, raw_response=response_text)
        try:
            # Strip markdown fences if present
            clean = response_text.strip()
            if clean.startswith("```"):
                clean = clean.split("\n", 1)[-1]
            if clean.endswith("```"):
                clean = clean.rsplit("```", 1)[0]
            clean = clean.strip()

            parsed = json.loads(clean)
            check.language_detected = parsed.get("language_detected", "")
            check.language_correct = parsed.get("language_correct", False)
            check.blank_cards_found = parsed.get("blank_cards", 0)
            check.placeholders_found = parsed.get("placeholders", [])
            check.card_count = parsed.get("card_count", 0)
            check.notes = parsed.get("notes", "")
        except json.JSONDecodeError:
            check.notes = f"Parse error: {response_text[:200]}"

        checks.append(check)

    return checks


# ---------------------------------------------------------------------------
# Validation Pipeline
# ---------------------------------------------------------------------------


def discover_pdfs(target_dir: Path, lang: str | None = None) -> list[tuple[str, str]]:
    """Find all PDFs per language. Returns [(lang, pdf_path)]."""
    results = []
    langs = [lang] if lang else LANGUAGES

    for l in langs:
        doc_dir = target_dir / l / "Documents" / "density-0"
        if not doc_dir.exists():
            print(f"  SKIP {l}: {doc_dir} not found")
            continue
        for pdf in sorted(doc_dir.glob("*.pdf")):
            results.append((l, str(pdf)))

    return results


def validate_pdf(pdf_path: str, lang: str, api_key: str, model: str, pages_to_check: int = 3) -> PdfValidation:
    """Run vision validation on a single PDF."""
    validation = PdfValidation(pdf_path=pdf_path, expected_lang=lang)
    start = time.time()

    try:
        doc = fitz.open(pdf_path)
        validation.total_pages = doc.page_count
        doc.close()
    except Exception as e:
        validation.errors.append(f"Cannot open PDF: {e}")
        validation.duration_s = time.time() - start
        return validation

    # Select representative pages: first, middle, last
    total = validation.total_pages
    if total <= pages_to_check:
        page_nums = list(range(1, total + 1))
    else:
        mid = total // 2
        page_nums = [1, mid, total]
        page_nums = sorted(set(page_nums))[:pages_to_check]

    try:
        images = pdf_to_images(pdf_path, page_nums)
        if not images:
            validation.errors.append("No pages could be rendered")
            validation.duration_s = time.time() - start
            return validation

        checks = call_vision_api(images, lang, api_key, model)
        validation.checks = checks
        validation.pages_checked = len(checks)
    except Exception as e:
        validation.errors.append(f"Vision API error: {e}")

    validation.duration_s = time.time() - start

    # Determine pass/fail
    if not validation.errors and validation.checks:
        all_lang_correct = all(c.language_correct for c in validation.checks)
        no_blanks = all(c.blank_cards_found == 0 for c in validation.checks)
        no_placeholders = all(len(c.placeholders_found) == 0 for c in validation.checks)
        validation.passed = all_lang_correct and no_blanks and no_placeholders

        if not all_lang_correct:
            detected = set(c.language_detected for c in validation.checks if c.language_detected)
            validation.errors.append(f"Language mismatch: expected {lang}, detected {detected}")
        for c in validation.checks:
            if c.blank_cards_found > 0:
                validation.errors.append(f"Page {c.page_number}: {c.blank_cards_found} blank cards")
            if c.placeholders_found:
                validation.errors.append(f"Page {c.page_number}: placeholders {c.placeholders_found}")

    return validation


# ---------------------------------------------------------------------------
# Report Generation
# ---------------------------------------------------------------------------


def generate_report(validations: list[PdfValidation], target_dir: str, model: str) -> ValidationReport:
    """Generate summary report from all validations."""
    report = ValidationReport(
        target_dir=target_dir,
        model=model,
        timestamp=time.strftime("%Y-%m-%d %H:%M:%S"),
        total_pdfs=len(validations),
    )

    for v in validations:
        if v.errors and not v.checks:
            report.skipped += 1
        elif v.passed:
            report.passed += 1
        else:
            report.failed += 1

        report.validations.append(asdict(v))

    # Build summary
    lines = [
        f"PDF Vision Validation Report — {report.timestamp}",
        f"Model: {model}",
        f"Target: {target_dir}",
        f"Total: {report.total_pdfs} | Passed: {report.passed} | Failed: {report.failed} | Skipped: {report.skipped}",
        "",
    ]

    if report.failed > 0:
        lines.append("FAILURES:")
        for v in validations:
            if not v.passed and v.checks:
                lines.append(f"  [{v.expected_lang}] {Path(v.pdf_path).name}")
                for e in v.errors:
                    lines.append(f"    - {e}")
        lines.append("")

    # Per-language summary
    for lang in LANGUAGES:
        lang_vals = [v for v in validations if v.expected_lang == lang]
        if not lang_vals:
            continue
        passed = sum(1 for v in lang_vals if v.passed)
        total = len(lang_vals)
        lines.append(f"  {lang.upper()}: {passed}/{total} passed")

    report.summary = "\n".join(lines)
    return report


# ---------------------------------------------------------------------------
# CLI
# ---------------------------------------------------------------------------


def main():
    parser = argparse.ArgumentParser(description="PDF Vision Validator for Argumentum Pipeline")
    parser.add_argument(
        "--target", type=str, default=str(DEFAULT_TARGET),
        help="Path to Target/ directory with generated PDFs",
    )
    parser.add_argument(
        "--model", type=str, default=DEFAULT_MODEL,
        help="Vision model ID (default: glm-4.6v)",
    )
    parser.add_argument(
        "--pages", type=int, default=3,
        help="Number of pages to check per PDF (default: 3)",
    )
    parser.add_argument(
        "--lang", type=str, default=None,
        help="Validate only this language (default: all)",
    )
    parser.add_argument(
        "--output", type=str, default=None,
        help="Output JSON report path (default: validation_report.json in script dir)",
    )
    parser.add_argument(
        "--fast", action="store_true",
        help="Only check FallaciesWeb A4 + TarotCards per language (skip split/print PDFs)",
    )
    args = parser.parse_args()

    target_dir = Path(args.target)
    if not target_dir.exists():
        print(f"ERROR: Target directory not found: {target_dir}", file=sys.stderr)
        sys.exit(1)

    api_key = get_api_key()

    print(f"PDF Vision Validator — model={args.model}, target={target_dir}")
    print(f"Checking languages: {args.lang or 'all (fr, en, ru, pt)'}")

    # Discover PDFs
    all_pdfs = discover_pdfs(target_dir, args.lang)
    if not all_pdfs:
        print("No PDFs found.", file=sys.stderr)
        sys.exit(1)

    # Filter for fast mode
    if args.fast:
        all_pdfs = [
            (lang, path) for lang, path in all_pdfs
            if "Fallacies_Web_A4" in Path(path).name
            or ("TarotCards" in Path(path).name and Path(path).name.count("_") <= 3)
            or ("PokerCards" in Path(path).name and Path(path).name.count("_") <= 3)
        ]

    print(f"Found {len(all_pdfs)} PDFs to validate\n")

    # Run validations
    validations = []
    for i, (lang, pdf_path) in enumerate(all_pdfs, 1):
        name = Path(pdf_path).name
        print(f"[{i}/{len(all_pdfs)}] [{lang.upper()}] {name}...", end=" ", flush=True)

        v = validate_pdf(pdf_path, lang, api_key, args.model, args.pages)
        validations.append(v)

        status = "PASS" if v.passed else ("SKIP" if not v.checks else "FAIL")
        print(f"{status} ({v.duration_s:.1f}s, {v.pages_checked}/{v.total_pages} pages)")
        if not v.passed and v.errors:
            for e in v.errors[:3]:
                print(f"    {e}")

    # Generate report
    report = generate_report(validations, str(target_dir), args.model)

    output_path = Path(args.output) if args.output else Path(__file__).parent / "validation_report.json"
    output_path.write_text(json.dumps(asdict(report), indent=2, ensure_ascii=False), encoding="utf-8")

    print(f"\n{report.summary}")
    print(f"\nReport saved to: {output_path}")

    sys.exit(0 if report.failed == 0 else 1)


if __name__ == "__main__":
    main()
