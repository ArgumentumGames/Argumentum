#!/usr/bin/env python
"""Signature page-par-page d'un PDF par ses images EMBARQUEES (+ geometrie de page).

Pourquoi pas un md5 du fichier : deux rendus du meme contenu different (dates, ids).
Pourquoi pas un md5 des PNG harvest inter-sessions : non deterministe (Playwright).
Les images embarquees, elles, sont deterministes depuis les PNG sources : un changement
de contenu change la signature de la page concernee, et seulement elle.

Etalonnage (21/09/2026, pre-depôt) :
  - temoin identique (bundle fr vs arbre de regen)      -> 379/379 identiques, 0 differentes
  - temoin different (TarotCards vs TarotCards_Virtues) -> 262/262 differentes + pagination
  - reproductibilite : 2 runs independants (arbres D:\\A9 / D:\\A10, harvest rejoue)
    -> 403/403 pages identiques sur les 6 documents fr hors-Tarot

Usage :
  python tools/pdf-page-signature.py ANCIEN.pdf NOUVEAU.pdf    # comparaison
  python tools/pdf-page-signature.py --self-test REF.pdf       # temoin + mutation falsifiante

Codes de sortie (comparaison) : 0 = pages toutes identiques, 1 = au moins une page
differente ou pagination differente, 2 = erreur.
Self-test : 0 = PASS (les deux controles tiennent), 1 = FAIL, 2 = erreur.
"""
import hashlib
import shutil
import sys
import tempfile
import os

import fitz  # PyMuPDF


def page_sig(doc, page):
    h = hashlib.sha256()
    try:
        for img in page.get_images(full=True):
            xref = img[0]
            try:
                data = doc.extract_image(xref).get("image", b"")
            except Exception as exc:  # xref illisible
                data = f"ERR{exc}".encode()
            h.update(hashlib.sha256(data).digest())
    except Exception as exc:
        h.update(f"IMGERR{exc}".encode())
    # geometrie : un changement de gabarit doit sortir, meme a images egales
    r = page.rect
    h.update(f"{round(r.width, 2)}x{round(r.height, 2)}|{page.rotation}".encode())
    return h.hexdigest()


def sig(path):
    doc = fitz.open(path)
    try:
        return doc.page_count, [page_sig(doc, p) for p in doc]
    finally:
        doc.close()


def compare(a_path, b_path, max_listed=60):
    na, sa = sig(a_path)
    nb, sb = sig(b_path)
    print(f"A {a_path}")
    print(f"  pages={na}")
    print(f"B {b_path}")
    print(f"  pages={nb}")
    if na != nb:
        print(f"!! NOMBRE DE PAGES DIFFERENT ({na} vs {nb}) -- comparaison limitee au min")
    n = min(na, nb)
    diff = [i + 1 for i in range(n) if sa[i] != sb[i]]
    print(f"pages_signature_identique={n - len(diff)}/{n}")
    print(f"pages_differentes={len(diff)}")
    if diff:
        head = diff[:max_listed]
        print("  " + ", ".join(str(x) for x in head) + (" ..." if len(diff) > len(head) else ""))
    return na == nb and not diff


def tiny_png():
    """8x8 RGB solide -> octets PNG (bytes differents de toute image de cartes)."""
    pix = fitz.Pixmap(fitz.csRGB, fitz.IRect(0, 0, 8, 8))
    pix.set_rect(pix.irect, (255, 0, 255))
    return pix.tobytes("png")


def self_test(ref_path):
    """Les deux controles exigés par la doctrine (temoin vivant + mutation falsifiante).

    1. TEMoin : deux fichiers distincts de memes octets -> 0 page differente.
    2. CONTROLE INVERSE : une image ajoutée sur UNE page (page du milieu) ->
       l'instrument doit rapporter exactement cette page, et aucune autre.
       insert_image cree un xref neuf utilise par cette seule page : les dos de
       cartes PARTAGES entre pages ne sont pas touches.
    """
    ok = True
    with tempfile.TemporaryDirectory() as td:
        # --- 1. temoin : copie octet a octet
        copy_path = os.path.join(td, "temoin.pdf")
        shutil.copyfile(ref_path, copy_path)
        print("== TEMOIN (copie octet a octet) : attendu 0 page differente")
        if not compare(ref_path, copy_path):
            print("FAIL : la copie identique presente des differences")
            ok = False
        else:
            print("   PASS")

        # --- 2. mutation falsifiante : image ajoutee sur la page du milieu
        n_pages = fitz.open(ref_path).page_count
        k = n_pages // 2
        mut_path = os.path.join(td, "mute.pdf")
        shutil.copyfile(ref_path, mut_path)
        doc = fitz.open(mut_path)
        try:
            page = doc[k]
            page.insert_image(fitz.Rect(0, 0, 20, 20), stream=tiny_png())
            doc.saveIncr()
        finally:
            doc.close()
        print(f"== CONTROLE INVERSE (image ajoutee page {k + 1}/{n_pages}) : attendu 1 page differente, exactement elle")
        na, sa = sig(ref_path)
        nb, sb = sig(mut_path)
        diff = [i + 1 for i in range(min(na, nb)) if sa[i] != sb[i]]
        print(f"pages_differentes={len(diff)} {diff[:10]}")
        if diff == [k + 1]:
            print("   PASS")
        else:
            print(f"FAIL : attendu [{k + 1}], obtenu {diff[:10]}{'...' if len(diff) > 10 else ''}")
            ok = False

    print("SELF-TEST " + ("PASS" if ok else "FAIL"))
    return ok


def main():
    args = sys.argv[1:]
    if len(args) == 2 and args[0] == "--self-test":
        sys.exit(0 if self_test(args[1]) else 1)
    if len(args) != 2:
        print(__doc__)
        sys.exit(2)
    try:
        sys.exit(0 if compare(args[0], args[1]) else 1)
    except Exception as exc:
        print(f"ERREUR : {exc}", file=sys.stderr)
        sys.exit(2)


if __name__ == "__main__":
    main()
