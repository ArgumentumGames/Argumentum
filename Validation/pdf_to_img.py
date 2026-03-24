"""
Convertit des PDFs en images PNG pour inspection visuelle.
"""
import sys
from pathlib import Path

try:
    import fitz  # PyMuPDF
except ImportError:
    print("Installation de PyMuPDF...")
    import subprocess
    subprocess.check_call([sys.executable, "-m", "pip", "install", "pymupdf"])
    import fitz

def pdf_to_png(pdf_path: str, output_dir: str = "comparison_output"):
    """Convertit la première page d'un PDF en PNG."""
    pdf_path = Path(pdf_path)
    output_dir = Path(output_dir)
    output_dir.mkdir(exist_ok=True)

    doc = fitz.open(str(pdf_path))
    page = doc[0]  # Première page

    # Zoom pour meilleure qualité
    mat = fitz.Matrix(2, 2)
    pix = page.get_pixmap(matrix=mat)

    output_path = output_dir / f"{pdf_path.stem}.png"
    pix.save(str(output_path))

    doc.close()
    print(f"  {pdf_path.name} -> {output_path.name}")

    return output_path

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python pdf_to_img.py <pdf_pattern>")
        print("Exemple: python pdf_to_img.py comparison_output/*_page*.pdf")
        sys.exit(1)

    import glob
    pattern = sys.argv[1]
    pdf_files = sorted(glob.glob(pattern))

    print(f"Conversion de {len(pdf_files)} PDFs en PNG...")
    for pdf_file in pdf_files:
        try:
            pdf_to_png(pdf_file)
        except Exception as e:
            print(f"  Erreur: {e}")
