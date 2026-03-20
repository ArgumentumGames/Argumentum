"""
Script simple pour extraire les premières pages d'un PDF et les nommer séquentiellement.
Usage: python extract_pdf_pages.py <pdf_path> <num_pages>
"""
import sys
from pathlib import Path

try:
    from pypdf import PdfReader, PdfWriter
except ImportError:
    print("Installation de pypdf...")
    import subprocess
    subprocess.check_call([sys.executable, "-m", "pip", "install", "pypdf"])
    from pypdf import PdfReader, PdfWriter

def extract_first_pages(pdf_path: str, num_pages: int, output_dir: str = "comparison_output"):
    """Extrait les N premières pages d'un PDF."""
    pdf_path = Path(pdf_path)
    output_dir = Path(output_dir)
    output_dir.mkdir(exist_ok=True)

    print(f"Lecture de {pdf_path}...")
    reader = PdfReader(str(pdf_path))
    total_pages = len(reader.pages)
    print(f"Total pages: {total_pages}")

    pages_to_extract = min(num_pages, total_pages)

    for i in range(pages_to_extract):
        page_num = i + 1
        writer = PdfWriter()
        writer.add_page(reader.pages[i])

        output_path = output_dir / f"{pdf_path.stem}_page{page_num}.pdf"
        with open(output_path, "wb") as f:
            writer.write(f)

        print(f"  Page {page_num} -> {output_path}")

    print(f"\nExtrait {pages_to_extract} pages vers {output_dir}/")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python extract_pdf_pages.py <pdf_path> [num_pages=10]")
        sys.exit(1)

    pdf_path = sys.argv[1]
    num_pages = int(sys.argv[2]) if len(sys.argv) > 2 else 10

    extract_first_pages(pdf_path, num_pages)
