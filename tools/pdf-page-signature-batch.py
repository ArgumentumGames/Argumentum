#!/usr/bin/env python
"""Capture de ligne de base : signatures page-par-page de TOUS les PDF d'un bundle.

Driver du grain 0 du pool v2 (#458, c.5773133629) : l'instrument de comparaison
(tools/pdf-page-signature.py) compare DEUX régénérations ; ce driver capture la
PREMIÈRE. Il IMPORTE page_sig depuis l'instrument livré — aucune redéfinition, la
signature produite ici est exactement celle que la comparaison produira demain.

Sortie JSON : une entrée par PDF (organisé par langue), avec à côté le sha256 du
PDF SOURCE — pour savoir de quoi la signature est la signature (DoD du grain).

Usage :
  python tools/pdf-page-signature-batch.py <bundle_dir> <out.json>

  --check <out.json> <pdf1> <pdf2> ..   re-signe les PDF nommés et vérifie que
                                        leurs entrées sont identiques au JSON
                                        (contrôle du grain : rejeu sur 2 PDF).

Codes de sortie --check : 0 = toutes identiques, 1 = au moins une divergence,
2 = erreur. Sans --check : 0 = manifeste écrit, 2 = erreur.
"""
import hashlib
import importlib.util
import json
import os
import sys
from datetime import datetime, timezone

BUNDLE_ENV = "ARGU_BASELINE_BUNDLE"


def load_page_sig():
    """Importe page_sig depuis l'instrument livré, dans le MÊME répertoire."""
    here = os.path.dirname(os.path.abspath(__file__))
    src = os.path.join(here, "pdf-page-signature.py")
    spec = importlib.util.spec_from_file_location("pdf_page_signature", src)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod.page_sig


def sha256_file(path):
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def sign_pdf(page_sig, path):
    import fitz  # PyMuPDF — même dépendance que l'instrument livré

    doc = fitz.open(path)
    try:
        return doc.page_count, [page_sig(doc, p) for p in doc]
    finally:
        doc.close()


def entry(page_sig, path, lang):
    n, sigs = sign_pdf(page_sig, path)
    return {
        "lang": lang,
        "file": os.path.basename(path),
        "bytes": os.path.getsize(path),
        "sha256_pdf": sha256_file(path),
        "pages": n,
        "page_signatures": sigs,
    }


def main(argv):
    if len(argv) < 3:
        print(__doc__)
        return 2
    page_sig = load_page_sig()
    here = os.path.dirname(os.path.abspath(__file__))
    instrument_sha = sha256_file(os.path.join(here, "pdf-page-signature.py"))

    if argv[1] == "--check":
        manifest_path, pdfs = argv[2], argv[3:]
        with open(manifest_path, "r", encoding="utf-8") as fh:
            manifest = json.load(fh)
        by_file = {e["file"]: e for e in manifest["files"]}
        rc = 0
        for pdf in pdfs:
            name = os.path.basename(pdf)
            if name not in by_file:
                print(f"ABSENT DU MANIFESTE : {name}")
                rc = max(rc, 1)
                continue
            e = entry(page_sig, pdf, by_file[name]["lang"])
            ref = by_file[name]
            same = (e["sha256_pdf"] == ref["sha256_pdf"]
                    and e["pages"] == ref["pages"]
                    and e["page_signatures"] == ref["page_signatures"])
            print(f"{'IDENTIQUE' if same else 'DIVERGE   '} {name} "
                  f"(sha256_pdf={'=' if e['sha256_pdf'] == ref['sha256_pdf'] else '!'}, "
                  f"{e['pages']}/{ref['pages']} pages)")
            if not same:
                rc = 1
        return rc

    bundle, out = argv[1], argv[2]
    if not os.path.isdir(bundle):
        print(f"RÉPERTOIRE ABSENT : {bundle}")
        return 2

    files = []
    for root, _dirs, names in os.walk(bundle):
        for name in sorted(names):
            if not name.lower().endswith(".pdf"):
                continue
            path = os.path.join(root, name)
            lang = os.path.basename(root)
            if lang == os.path.basename(bundle.rstrip("\\/")):
                lang = "."
            print(f"signature {lang}/{name} ...", file=sys.stderr)
            files.append(entry(page_sig, path, lang))

    files.sort(key=lambda e: (e["lang"], e["file"]))
    manifest = {
        "what": "ligne de base de régénération — signatures page-par-page du bundle de recette",
        "instrument": "tools/pdf-page-signature.py (page_sig importé, non redéfini)",
        "instrument_sha256": instrument_sha,
        "driver": "tools/pdf-page-signature-batch.py",
        "bundle_dir": bundle,
        "captured_utc": datetime.now(timezone.utc).isoformat(timespec="seconds"),
        "pdf_count": len(files),
        "pages_total": sum(e["pages"] for e in files),
        "files": files,
    }
    with open(out, "w", encoding="utf-8") as fh:
        json.dump(manifest, fh, ensure_ascii=False, indent=1)
    langs = sorted({e["lang"] for e in files})
    print(f"{len(files)} PDF · {manifest['pages_total']} pages · langues={','.join(langs)}")
    print(f"manifeste : {out}")
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
