# -*- coding: utf-8 -*-
"""P23-08 (#1369) — planches de preuve pour la sortie de repli.

⚠️ Ce siège n'a PAS la vision (le proxy retire les images) : les planches sont
donc validées PROGRAMMATIQUEMENT, jamais « regardées » :
  - chaque libellé doit tenir dans son panneau (textlength mesuré, sinon échec) ;
  - la bande du pilote doit être SOMBre dans un panneau « avant » et CLAIRE dans
    un panneau « après » (luminance moyenne de la zone mesurée) ;
  - le PDF publié est mesuré page par page (luminance de la bande 88–94 % de la
    hauteur) : la page 7 seule doit être sombre — contrôle inverse compris.

Aucun tracé par-dessus une carte : réduction LANCZOS ou recadrage brut.

    python make_sheets.py <racine>     (racine = dossier des passes render-avant/ et
                                        render-apres/, du PDF publié dans pub/ ; les
                                        planches sortent dans <racine>/sheets/)
"""
import glob
import json
import pathlib
import shutil
import subprocess
import sys

from PIL import Image, ImageDraw, ImageFont

assert len(sys.argv) == 2, __doc__
P = pathlib.Path(sys.argv[1])
OUT = P / "sheets"
PDF = P / "pub" / "Argumentum_TarotCards_fr.pdf"
TITLE = ImageFont.truetype(r"C:\Windows\Fonts\arial.ttf", 22)
LABEL = ImageFont.truetype(r"C:\Windows\Fonts\arial.ttf", 17)
PAD, TOP, BOTTOM = 18, 62, 14
# PNG disque 826x1417 (300 dpi) : bande = lignes 1236-1348 (mesuré, compare.py)
BAND_DISK = (1236, 1349)


def card(side, n):
    """PNG de `Images/density-0/Rules/` du rendu <side> (avant | apres), en RVB."""
    rel = P / f"render-{side}" / "Target/fr/Images/density-0/Rules" / f"rules_{n:02d}_face.png"
    return Image.open(rel).convert("RGB")


def lum(img, box):
    """Luminance moyenne (0-255) d'un rectangle, sur une image RVB."""
    c = img.crop(box).convert("L")
    h = c.histogram()
    return sum(i * n for i, n in enumerate(h)) / (c.width * c.height)


def canvas(items, title, notes, width):
    """Feuille blanche : titre, libellés, images, notes (une phrase par ligne)."""
    notes_h = 26 * len(notes)
    h = TOP + max(im.height for _, im in items) + BOTTOM + notes_h
    out = Image.new("RGB", (width, h), "white")
    d = ImageDraw.Draw(out)
    assert d.textlength(title, font=TITLE) <= width - 2 * PAD, "titre trop long"
    d.text((PAD, 12), title, fill="black", font=TITLE)
    for i, n in enumerate(notes):
        assert d.textlength(n, font=LABEL) <= width - 2 * PAD, f"note trop longue : {n!r}"
        d.text((PAD, h - notes_h - BOTTOM + 6 + 26 * i), n, fill=(70, 70, 70), font=LABEL)
    return out, d


def sheet(items, name, title, notes=()):
    """items = [(libelle, image)] côte à côte, échelle 1:1."""
    w = PAD + sum(im.width for _, im in items) + PAD * len(items)
    out, d = canvas(items, title, notes, w)
    x = PAD
    for l, im in items:
        assert d.textlength(l, font=LABEL) <= im.width, f"libellé trop large : {l!r}"
        d.text((x, TOP - 24), l, fill=(70, 70, 70), font=LABEL)
        out.paste(im, (x, TOP))
        d.rectangle([x - 1, TOP - 1, x + im.width, TOP + im.height], outline=(190, 190, 190))
        x += im.width + PAD
    return save(out, name)


def stacked(items, name, title, notes=()):
    """items = [(libelle, image)] empilées, échelle 1:1."""
    w = PAD + max(im.width for _, im in items) + PAD
    notes_h = 26 * len(notes)
    h = TOP + sum(26 + im.height for _, im in items) + BOTTOM + notes_h
    out = Image.new("RGB", (w, h), "white")
    d = ImageDraw.Draw(out)
    assert d.textlength(title, font=TITLE) <= w - 2 * PAD, "titre trop long"
    d.text((PAD, 12), title, fill="black", font=TITLE)
    y = TOP
    for l, im in items:
        assert d.textlength(l, font=LABEL) <= im.width, f"libellé trop large : {l!r}"
        d.text((PAD, y), l, fill=(70, 70, 70), font=LABEL)
        y += 26
        out.paste(im, (x := PAD, y))
        d.rectangle([x - 1, y - 1, x + im.width, y + im.height], outline=(190, 190, 190))
        y += im.height
    for i, n in enumerate(notes):
        assert d.textlength(n, font=LABEL) <= w - 2 * PAD, f"note trop longue : {n!r}"
        d.text((PAD, h - notes_h - BOTTOM + 6 + 26 * i), n, fill=(70, 70, 70), font=LABEL)
    return save(out, name)


def save(out, name):
    path = OUT / name
    out.save(path, "PNG", optimize=True)
    print(f"  {name:34} {out.width}x{out.height}  {path.stat().st_size:>8} o")
    return path


def gs_exe():
    w = shutil.which("gswin64c") or shutil.which("gswin64")
    if w:
        return w
    c = sorted(glob.glob(r"C:\Program Files\gs\gs*\bin\gswin64c.exe"))
    return c[-1] if c else None


def published_pages(dpi, first, last):
    """Pages du PDF PUBLIÉ (asset du 30/09), rendues par Ghostscript."""
    exe = gs_exe()
    assert exe, "Ghostscript introuvable"
    dst = P / "pub" / f"pages-{dpi}dpi"
    dst.mkdir(exist_ok=True)
    cmd = [exe, "-q", "-dNOPAUSE", "-dBATCH", "-dSAFER", "-sDEVICE=png16m", f"-r{dpi}",
           f"-dFirstPage={first}", f"-dLastPage={last}",
           "-sOutputFile=" + str(dst / "p%d.png"), str(PDF)]
    r = subprocess.run(cmd, capture_output=True, text=True)
    assert r.returncode == 0, r.stderr[:300]
    if first != 1:                                   # Ghostscript numérote depuis 1
        for k in range(1, last - first + 2):
            (dst / f"p{k}.png").replace(dst / f"p{first + k - 1}.png")
    return dst


def measure_published(dpi=120):
    """Pages 1–15 : luminance de la bande du pilote (88–94 % de la hauteur) et de la
    zone juste au-dessus (82–86 %). Le pilote se reconnaît à une bande SOMBRE
    surmontée de BLANC ; la couverture a un pied sombre continu, donc elle ne
    compte pas (son « au-dessus » est sombre aussi)."""
    d = published_pages(dpi, 1, 15)
    res = []
    for i in range(1, 16):
        im = Image.open(d / f"p{i}.png").convert("RGB")
        w, h = im.size
        bande = round(lum(im, (0, int(h * 0.88), w, int(h * 0.94))), 1)
        dessus = round(lum(im, (0, int(h * 0.82), w, int(h * 0.86))), 1)
        rgb = [round(float(x), 1) for x in __import__("numpy").asarray(
            im.crop((0, int(h * 0.88), w, int(h * 0.94)))).reshape(-1, 3).mean(axis=0)]
        res.append((i, bande, dessus, rgb, im.size))
    return res


def main():
    OUT.mkdir(exist_ok=True)
    a, b = card("avant", 7), card("apres", 7)
    y0, y1 = BAND_DISK
    s = 0.5
    print("planches :")
    p1 = sheet([("Rules_07 — AVANT", a.resize((int(a.width * s), int(a.height * s)), Image.LANCZOS)),
                ("Rules_07 — APRÈS", b.resize((int(b.width * s), int(b.height * s)), Image.LANCZOS))],
               "regles-07-avant-apres.png",
               "#1369 — Rules_07, rendu Debug 300 dpi (fr), réduit à 50 %",
               ["AVANT : gabarit de master, plus le retrait du @font-face mort qui bloquait la récolte.",
                "APRÈS : bloc card.cover-bingo retiré. Bande photo de 38 px CSS absente ; reste de la carte identique."])
    s3 = 0.4
    p2 = sheet([(f"Rules_0{n} — après", card("apres", n).resize(
        (int(826 * s3), int(1417 * s3)), Image.LANCZOS)) for n in (6, 7, 8)],
        "regles-06-07-08-apres.png",
        "#1369 — Rules_06 / 07 / 08 après le retrait, rendu Debug 300 dpi (fr), réduit à 40 %",
        ["Rules_06 et Rules_08 : pixels identiques avant et après (mesuré, voir README)."])
    p3 = stacked([("AVANT — lignes 1180 à 1416, échelle 1:1", a.crop((0, 1180, a.width, a.height))),
                  ("APRÈS — mêmes lignes, échelle 1:1", b.crop((0, 1180, b.width, b.height)))],
                 "regles-07-bande-100pct.png",
                 "#1369 — bas de Rules_07 à 100 % (PNG de Images/density-0/Rules, 300 dpi)",
                 ["Bande changée : lignes 1236–1348 (113 lignes). Le texte au-dessus est inchangé."])

    # ---- contrôles programmatiques des planches
    print("contrôles :")
    ok = True
    im = Image.open(p1).convert("RGB")
    w2 = int(a.width * s)
    by0, by1 = TOP + int(y0 * s), TOP + int(y1 * s)
    la = lum(im, (PAD, by0, PAD + w2, by1))
    lb = lum(im, (PAD + w2 + PAD, by0, PAD + w2 + PAD + w2, by1))
    print(f"  planche 1, bande : avant={la:.1f} (attendu < 90) | après={lb:.1f} (attendu > 240)")
    ok &= la < 90 and lb > 240
    im = Image.open(p2).convert("RGB")
    w3 = int(826 * s3)
    p3y0, p3y1 = TOP + int(y0 * s3), TOP + int(y1 * s3)
    vals = [lum(im, (PAD + i * (w3 + PAD), p3y0, PAD + i * (w3 + PAD) + w3, p3y1)) for i in range(3)]
    print(f"  planche 2, bande après : 06={vals[0]:.1f} 07={vals[1]:.1f} 08={vals[2]:.1f} (attendu > 240 ×3)")
    ok &= all(v > 240 for v in vals)
    im = Image.open(p3).convert("RGB")
    ya, yb = TOP + 26, TOP + 26 + (a.height - 1180) + 26
    va, vb = lum(im, (PAD, ya + y0 - 1180, PAD + a.width, ya + y1 - 1180)), lum(im, (PAD, yb + y0 - 1180, PAD + a.width, yb + y1 - 1180))
    print(f"  planche 3, bande 100 % : avant={va:.1f} (attendu < 90) | après={vb:.1f} (attendu > 240)")
    ok &= va < 90 and vb > 240

    pub = measure_published(120)
    pilote = [i for i, b, a, _, _ in pub if b < 90 and a > 240]
    print("  PDF publié, pages 1–15 : bande (88–94 %) / au-dessus (82–86 %)")
    print("    " + "  ".join(f"p{i:02d}={b}/{a}" for i, b, a, _, _ in pub))
    print(f"    pages portant la bande du pilote (sombre sous blanc) : {pilote} (attendu [7])")
    for i, b, a, rgb, _ in pub:
        if i in (1, 7):
            print(f"    p{i:02d} : RVB moyen de la bande = {rgb}")
    ok &= pilote == [7]
    d = published_pages(90, 6, 8)
    p4 = sheet([(f"PDF publié — page {i}", Image.open(d / f"p{i}.png").convert("RGB")) for i in (6, 7, 8)],
               "pdf-publie-p06-a-p08.png",
               "TarotCards_fr.pdf publié (asset du 30/09) — pages 6 à 8, 90 dpi",
               ["Page 7 = Rules_07 : seule page des 15 règles à porter la bande sombre (mesuré, voir README)."])
    im = Image.open(p4).convert("RGB")
    pw, ph = Image.open(d / "p6.png").size
    v4 = [lum(im, (PAD + k * (pw + PAD), TOP + int(ph * 0.88), PAD + k * (pw + PAD) + pw, TOP + int(ph * 0.94)))
          for k in range(3)]
    print(f"  planche 4, bande : p06={v4[0]:.1f} p07={v4[1]:.1f} p08={v4[2]:.1f} (attendu clair / sombre / clair)")
    ok &= v4[0] > 240 and v4[1] < 90 and v4[2] > 240
    (OUT / "controles.json").write_text(json.dumps(
        {"planches": {"p1_avant_bande": la, "p1_apres_bande": lb, "p2_apres": vals, "p3_avant": va, "p3_apres": vb,
                      "p4": v4},
         "publie": [{"page": i, "bande": b, "dessus": a, "rvg_bande": rgb} for i, b, a, rgb, _ in pub]},
        ensure_ascii=False, indent=1), encoding="utf-8")
    print("VERDICT :", "OK — planches cohérentes avec la mesure" if ok else "INCOHÉRENT — ne pas livrer")
    raise SystemExit(0 if ok else 2)


if __name__ == "__main__":
    main()
