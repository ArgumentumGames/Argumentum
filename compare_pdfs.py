#!/usr/bin/env python3
"""
Compare Argumentum PDFs with reference images.
"""
import fitz  # PyMuPDF
import os
from pathlib import Path

def extract_page_as_image(pdf_path, page_num, output_dir):
    """Extract a single page from PDF as PNG."""
    doc = fitz.open(pdf_path)
    page = doc[page_num]

    # Render page to image
    pix = page.get_pixmap()  # Default resolution

    # Save as PNG
    output_path = Path(output_dir) / f"{Path(pdf_path).stem}_page{page_num}.png"
    pix.save(output_path)
    print(f"Extracted: {output_path}")
    return output_path

def main():
    # Reference images provided by user
    reference_images = {
        "rules": "d:/Dev/Argumentum/reference-ru-2-rules.png",
        "rules_face": "d:/Dev/Argumentum/rules_face_01.png",
        "rules_back": "d:/Dev/Argumentum/rules_back_default.png",
        "ru-page1": "d:/Dev/Argumentum/current-ru-1-page1.png",
        "ru-page2": "d:/Dev/Argumentum/current-ru-2-page1.png",
    }

    # Current RU PDFs
    ru_pdfs_dir = Path("d:/Dev/Argumentum/Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/ru/Documents/density-0")

    # Output directory for extracted pages
    output_dir = Path("d:/Dev/Argumentum/comparison_output")
    output_dir.mkdir(exist_ok=True)

    # PDF files to check
    pdfs_to_check = [
        "Argumentum_TarotCards_ru-1.pdf",  # Cover + rules
        "Argumentum_PokerCards_ru-1.pdf",  # Scenarii
    ]

    print("=" * 60)
    print("PDF Comparison Tool for Argumentum")
    print("=" * 60)

    for pdf_name in pdfs_to_check:
        pdf_path = ru_pdfs_dir / pdf_name
        if not pdf_path.exists():
            print(f"\n[SKIP] {pdf_name} not found")
            continue

        print(f"\n[INFO] Processing: {pdf_name}")
        doc = fitz.open(pdf_path)
        print(f"  Pages: {doc.page_count}")

        # Extract first 3 pages for comparison
        for page_num in range(min(3, doc.page_count)):
            output_path = extract_page_as_image(str(pdf_path), page_num, str(output_dir))
            print(f"  Page {page_num}: {output_path}")

if __name__ == "__main__":
    main()
