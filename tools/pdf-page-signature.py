#!/usr/bin/env python
"""Signature page-par-page d'un PDF par ses images EMBARQUEES (+ geometrie de page).

Pourquoi pas un md5 du fichier : deux rendus du meme contenu different (dates, ids).
Pourquoi pas un md5 des PNG harvest inter-sessions : non deterministe (Playwright).
Les images embarquees, elles, sont deterministes depuis les PNG sources : un changement
de contenu change la signature de la page concernee, et seulement elle.

PORTEE EXACTE (ecrite ici depuis le grain 7bis, #1493 c.5773088200) :

  LA signature couvre, par page :
    - le sha256 des octets de CHAQUE image embarquee (page.get_images(full=True)),
      xref par xref ;
    - le sha256 des octets du SMASK de chaque image (img[1]) quand il existe --
      deux cartes de meme bitmap mais d'alpha different sont DES cartes differentes ;
    - largeur x hauteur arrondies au 1/100 et rotation de la page (changement de
      gabarit visible meme a images egales).

  ELLE NE COUVRE PAS (declares non-objectifs, a ne pas « corriger » en silence) :
    - le texte vectoriel et les tracés (polices, calques vectoriels) ;
    - la PLACEMENT des images (get_image_rects) : deux pages aux memes images
      disposees differemment ont la MEME signature -- limitation assumee, le
      gabarit (format+rotation de page) reste couvert ;
    - les metadonnees, dates, identifiants d'objet et numeros xref.

Etalonnage (21/09/2026, pre-depôt) :
  - temoin identique (bundle fr vs arbre de regen)      -> 379/379 identiques, 0 differentes
  - temoin different (TarotCards vs TarotCards_Virtues) -> 262/262 differentes + pagination
  - reproductibilite : 2 runs independants (arbres D:\\A9 / D:\\A10, harvest rejoue)
    -> 403/403 pages identiques sur les 6 documents fr hors-Tarot
  - grain 7bis (22/09/2026) : mutation SMASK-SEUL -> exactement les pages qui
    referencent le smask mute, et aucune autre (voir self-test, controle 3).
    AVANT le fix, cette mutation rendait 0 page differente (angle mort).

Usage :
  python tools/pdf-page-signature.py ANCIEN.pdf NOUVEAU.pdf    # comparaison
  python tools/pdf-page-signature.py --self-test REF.pdf       # 3 controles

Codes de sortie (comparaison) : 0 = pages toutes identiques, 1 = au moins une page
differente ou pagination differente, 2 = erreur.
Self-test : 0 = PASS (les trois controles tiennent), 1 = FAIL, 2 = erreur.
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
            xref, smask = img[0], img[1]
            try:
                data = doc.extract_image(xref).get("image", b"")
            except Exception as exc:  # xref illisible
                data = f"ERR{exc}".encode()
            h.update(hashlib.sha256(data).digest())
            # 7bis : le smask (img[1]) fait partie du contenu de la carte.
            # Un alpha different = une carte differente, meme bitmap.
            if smask:
                try:
                    sdata = doc.extract_image(smask).get("image", b"")
                except Exception as exc:  # smask illisible : l'erreur EST un contenu
                    sdata = f"ERR{exc}".encode()
                h.update(hashlib.sha256(b"SMASK:" + sdata).digest())
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


def first_smask(path):
    """(page_index_0b, smask_xref, pages_0b_referencant_ce_smask) ou None."""
    doc = fitz.open(path)
    try:
        pages_using = {}
        found = None
        for p in range(doc.page_count):
            for img in doc[p].get_images(full=True):
                smask = img[1]
                if smask:
                    found = found or (p, smask)
                    pages_using.setdefault(smask, set()).add(p)
        if not found:
            return None
        p0, xref = found
        return p0, xref, sorted(pages_using[xref])
    finally:
        doc.close()


def self_test(ref_path):
    """Les TROIS controles exigés par la doctrine (temoin + 2 mutations falsifiantes).

    1. TEMOIN : deux fichiers distincts de memes octets -> 0 page differente.
    2. CONTROLE INVERSE (image ajoutee) : une image de plus sur UNE page ->
       l'instrument doit rapporter exactement cette page, et aucune autre.
       insert_image cree un xref neuf utilise par cette seule page : les dos de
       cartes PARTAGES entre pages ne sont pas touches.
    3. CONTROLE INVERSE 7bis (SMASK SEUL) : le smask d'une image existante est
       remplace, sans toucher ni la bitmap ni aucune autre page -> l'instrument
       doit rapporter exactement les pages qui referencent ce smask, et aucune
       autre. AVANT le fix, ce controle echouait silencieusement (0 page).
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

        # --- 3. mutation falsifiante 7bis : SMASK SEUL
        target = first_smask(ref_path)
        smask_ref = ref_path
        if not target:
            # Le temoin n'a aucune image a alpha (mesure : le bundle 2026-09-21
            # porte 0 smask sur ses 1 709 images fr). Le controle doit TOUJOURS
            # pouvoir s'exercer : on construit un temoin synthetique de 2 pages
            # dont la page 2 porte une PNG AVEC alpha (PyMuPDF la scinde en
            # image + /SMask), et on mute son smask.
            print("== CONTROLE 7bis : aucun smask dans le temoin -> temoin synthetique construit")
            smask_ref = os.path.join(td, "smask_witness.pdf")
            pix = fitz.Pixmap(fitz.csRGB, fitz.IRect(0, 0, 32, 32), True)
            for yy in range(32):
                for xx in range(32):
                    pix.set_pixel(xx, yy, (200, 30, 40, 255 if xx < 16 else 0))
            doc = fitz.open()
            doc.new_page(width=200, height=200)
            page2 = doc.new_page(width=200, height=200)
            page2.insert_image(fitz.Rect(20, 20, 120, 120), stream=pix.tobytes("png"))
            doc.save(smask_ref)
            doc.close()
            target = first_smask(smask_ref)
        if not target:
            print("== CONTROLE 7bis : SKIP -- pas de smask obtainable (synthetique echoue)")
            ok = False
        else:
            p0, xref, expected_pages = target
            mut2 = os.path.join(td, "mute_smask.pdf")
            shutil.copyfile(smask_ref, mut2)
            doc = fitz.open(mut2)
            try:
                raw = doc.xref_stream(xref)
                # meme longueur, octets differents : le contenu du masque change,
                # sa structure (dictionnaire, dimensions, filtre) ne change pas.
                doc.update_stream(xref, bytes(b ^ 0x5A for b in raw), compress=1)
                doc.saveIncr()
            finally:
                doc.close()
            want = sorted(p + 1 for p in expected_pages)
            print(f"== CONTROLE 7bis (smask seul, xref {xref}) : attendu exactement {want}")
            na, sa = sig(smask_ref)
            nb, sb = sig(mut2)
            diff = sorted(i + 1 for i in range(min(na, nb)) if sa[i] != sb[i])
            print(f"pages_differentes={len(diff)} {diff[:10]}")
            if diff == want:
                print("   PASS")
            else:
                print(f"FAIL : attendu {want}, obtenu {diff[:10]}{'...' if len(diff) > 10 else ''}")
                ok = False

    print("SELF-TEST " + ("PASS" if ok else "FAIL"))
    return ok


def main():
    args = sys.argv[1:]
    if len(args) == 2 and args[0] == "--self-test":
        # 7bis : le self-test est DANS le try -- une exception dans un temoin
        # doit rendre 2 (erreur), pas une traceback nue ni un faux 0.
        try:
            sys.exit(0 if self_test(args[1]) else 1)
        except Exception as exc:
            print(f"ERREUR : {exc}", file=sys.stderr)
            sys.exit(2)
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
