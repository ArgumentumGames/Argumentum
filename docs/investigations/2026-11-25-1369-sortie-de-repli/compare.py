# -*- coding: utf-8 -*-
"""P23-08 (#1369) — comparaison avant/après des rendus Rules.

Oracle = PIXELS, jamais md5 : deux sessions CardPen donnent des PNG
pixel-identiques mais aux octets différents (piège mesuré le 18/09 sur cette
issue même). Trois niveaux, du plus autoritaire au plus livré :

  1. RÉCOLTE  — ce que CardPen a rendu : dataUrl du `*_harvest_<lang>.json`,
                décodés et comparés pixel à pixel (jamais copiés de `Images/`,
                dont l'écrivain saute les fichiers existants, #1250/#1295) ;
  2. DISQUE   — les PNG de `Images/density-0/<jeu>/` : pixels ET égalité des
                chunks IDAT ;
  3. CARTE 7  — où est l'écart : bande basse (38 px CSS × dpi/96) contre reste
                de la carte, amplitudes, lignes changées.

⚠️ Alpha : une différence calculée en RGBA a un canal alpha nul PARTOUT, et
`getbbox()` (Pillow >= 11, alpha_only=True par défaut) la voit VIDE. Première
version de cet instrument : « 22 identiques » sur une carte qui avait changé,
dénoncé par son propre contrôle inverse (07 contre 08 rendu IDENTIQUE). Ici
les images sont APLATIES sur blanc puis comparées en RVB, masque exact par
numpy (un canal non nul suffit).

Trois contrôles sont joués à chaque exécution :
  - un fichier contre lui-même            => doit rendre IDENTIQUE ;
  - Rules_07 contre Rules_08 (avant)       => doit rendre DIFFÉRENT ;
  - une copie de Rules_08 avec 1 niveau ajouté sur 1 pixel => doit être vue.

    python compare.py <avant/Target> <apres/Target> [lang]
"""
import base64
import hashlib
import io
import json
import pathlib
import struct
import sys

import numpy as np
from PIL import Image, ImageStat


def dataurl_bytes(s):
    head, _, payload = s.partition(",")
    return head, base64.b64decode(payload)


def flat(img):
    """RVB aplati sur blanc (l'alpha ne doit masquer aucune différence)."""
    if "A" in img.getbands() or img.mode == "P":
        img = img.convert("RGBA")
        bg = Image.new("RGBA", img.size, (255, 255, 255, 255))
        img = Image.alpha_composite(bg, img)
    return img.convert("RGB")


def diff(a, b):
    """(identique, nb pixels différents, % , bbox, écart max de canal, masque)."""
    if a.size != b.size:
        return False, a.size[0] * a.size[1], 100.0, None, 255, None
    arr = np.asarray(_absdiff(a, b))
    nz = (arr != 0).any(axis=2)
    n = int(nz.sum())
    if n == 0:
        return True, 0, 0.0, None, 0, nz
    ys, xs = np.nonzero(nz)
    bbox = (int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1)
    return False, n, 100.0 * n / nz.size, bbox, int(arr.max()), nz


def _absdiff(a, b):
    d = np.abs(np.asarray(a, dtype=np.int16) - np.asarray(b, dtype=np.int16)).astype(np.uint8)
    return Image.fromarray(d, "RGB")


def maxdiff(a, b, rows=None):
    """Écart max de canal sur une bande de lignes (slice). ou toute l'image."""
    x = np.asarray(_absdiff(a, b))
    return int(x[rows].max()) if rows is None or x[rows].size else 0


def png_idat(b):
    """sha256 des données IDAT concaténées, dimensions."""
    assert b[:8] == b"\x89PNG\r\n\x1a\n", "pas un PNG"
    pos, idat = 8, hashlib.sha256()
    while pos < len(b):
        (ln,) = struct.unpack(">I", b[pos:pos + 4])
        typ = b[pos + 4:pos + 8]
        if typ == b"IDAT":
            idat.update(b[pos + 8:pos + 8 + ln])
        pos += 12 + ln
    return idat.hexdigest()


def harvests(root, lang):
    d = root / lang / "Harvest"
    return sorted(p for p in d.glob("*_harvest_*.json")) if d.is_dir() else []


def faces(root, lang):
    """[(jeu, image, Image RVB)] pour toutes les faces de toutes les récoltes."""
    out = []
    for f in harvests(root, lang):
        j = json.loads(f.read_text(encoding="utf-8"))
        for k, v in sorted(j.get("Faces", {}).get("Images", {}).items()):
            out.append((f.name, k, flat(Image.open(io.BytesIO(dataurl_bytes(v)[1])))))
    return out


def controls(a_dir, lang):
    res = []
    fa = faces(a_dir, lang)
    A = {(g, k): im for g, k, im in fa}
    same = diff(A[("Rules_harvest_fr.json", "Rules_01")], A[("Rules_harvest_fr.json", "Rules_01")])[0]
    res.append(("Rules_01 (avant) contre elle-même", "IDENTIQUE" if same else "DIFFÉRENT", "OK" if same else "DÉFAUT"))
    d = diff(A[("Rules_harvest_fr.json", "Rules_07")], A[("Rules_harvest_fr.json", "Rules_08")])
    res.append(("Rules_07 contre Rules_08 (avant)", "DIFFÉRENT" if not d[0] else "IDENTIQUE",
                "OK" if not d[0] else "DÉFAUT DE L'INSTRUMENT"))
    probe = A[("Rules_harvest_fr.json", "Rules_08")].copy()
    px = probe.load()
    x0, y0 = probe.size[0] // 2, probe.size[1] // 2
    r, g, bl = px[x0, y0]
    # XOR 1 sur chaque canal : change TOUJOURS de 1 niveau (un +1 sur un blanc saturé ne change rien)
    px[x0, y0] = (r ^ 1, g ^ 1, bl ^ 1)
    d1 = diff(A[("Rules_harvest_fr.json", "Rules_08")], probe)
    res.append(("Rules_08 contre une copie à ±1 niveau sur 1 pixel",
                f"DIFFÉRENT ({d1[1]} px, écart max {d1[4]})" if not d1[0] else "IDENTIQUE",
                "OK" if (not d1[0] and d1[1] == 1 and d1[4] == 1) else "DÉFAUT DE L'INSTRUMENT"))
    return res


def compare_harvests(a_dir, b_dir, lang, report):
    out = []
    for fa in harvests(a_dir, lang):
        fb = b_dir / lang / "Harvest" / fa.name
        if not fb.exists():
            out.append((fa.name, "ABSENT côté après", "", "", "", ""))
            continue
        ja, jb = json.loads(fa.read_text(encoding="utf-8")), json.loads(fb.read_text(encoding="utf-8"))
        for side in ("Faces", "Backs"):
            ia, ib = ja.get(side, {}).get("Images", {}), jb.get(side, {}).get("Images", {})
            if sorted(ia) != sorted(ib) and (ia or ib):
                out.append((f"{fa.name}:{side}", f"JEUX DE CLÉS DIFFÉRENTS {len(ia)}/{len(ib)}", "", "", "", ""))
                continue
            for k in sorted(ia):
                ba, bb = dataurl_bytes(ia[k])[1], dataurl_bytes(ib[k])[1]
                A, B = flat(Image.open(io.BytesIO(ba))), flat(Image.open(io.BytesIO(bb)))
                same, n, pct, bbox, mx, _ = diff(A, B)
                out.append((f"{fa.name}:{side}:{k}", f"{A.size[0]}×{A.size[1]}",
                            "IDENTIQUE" if same else f"DIFFÉRENT {n} px ({pct:.3f} %)",
                            "oui" if png_idat(ba) == png_idat(bb) else "non",
                            "oui" if hashlib.sha256(ba).digest() == hashlib.sha256(bb).digest() else "non",
                            "-" if bbox is None else f"bbox={bbox} max={mx}"))
                report["pairs"].append({"key": out[-1][0], "identical": same, "n": n, "bbox": bbox, "max": mx,
                                        "dpi_a": ja.get(side, {}).get("Dpi"), "dpi_b": jb.get(side, {}).get("Dpi")})
    return out


def compare_disk(a_dir, b_dir, lang):
    out = []
    base_a, base_b = a_dir / lang / "Images", b_dir / lang / "Images"
    for pa in sorted(base_a.rglob("*.png")):
        rel = pa.relative_to(base_a)
        pb = base_b / rel
        if not pb.exists():
            out.append((f"Images/{rel}".replace("\\", "/"), "ABSENT côté après", "", "", ""))
            continue
        ba, bb = pa.read_bytes(), pb.read_bytes()
        A, B = flat(Image.open(io.BytesIO(ba))), flat(Image.open(io.BytesIO(bb)))
        same, n, pct, bbox, mx, _ = diff(A, B)
        out.append((f"Images/{rel}".replace("\\", "/"), f"{A.size[0]}×{A.size[1]}",
                    "IDENTIQUE" if same else f"DIFFÉRENT {n} px ({pct:.3f} %)",
                    "oui" if png_idat(ba) == png_idat(bb) else "non", "oui" if ba == bb else "non"))
    return out


def card7(a_dir, b_dir, lang):
    """Où est l'écart, pour chaque face qui diffère : amplitude et lignes."""
    rows = []
    A = {(g, k): im for g, k, im in faces(a_dir, lang)}
    B = {(g, k): im for g, k, im in faces(b_dir, lang)}
    for (g, k), a in sorted(A.items()):
        b = B.get((g, k))
        if b is None:
            continue
        same, n, pct, bbox, mx, nz = diff(a, b)
        w, h = a.size                                  # PIL : (largeur, hauteur)
        if same:
            rows.append((k, "oui", "0", "-", "-", "-", "-"))
            continue
        arr = np.asarray(_absdiff(a, b)).max(axis=2)    # écart max de canal, par pixel
        strong = arr >= 32                             # visible à l'oeil
        n_strong = int(strong.sum())
        ys = np.nonzero(nz.any(axis=1))[0]
        ys_s = np.nonzero(strong.any(axis=1))[0]
        rows.append((
            k, "non", f"{n} px ({pct:.2f} %)", f"{mx}",
            f"{n - n_strong} px, écart {arr[arr < 32].max() if n - n_strong else 0}",
            f"{n_strong} px, lignes {int(ys_s[0])}–{int(ys_s[-1])}" if n_strong else "0 px",
            f"lignes {int(ys[0])}–{int(ys[-1])} sur {h}"))
    return rows


def profile(a_dir, b_dir, lang, key="Rules_07", step=100):
    """Profil par tranches de lignes : pixels différents et écart max."""
    A = {(g, k): im for g, k, im in faces(a_dir, lang)}
    B = {(g, k): im for g, k, im in faces(b_dir, lang)}
    a, b = A[("Rules_harvest_fr.json", key)], B[("Rules_harvest_fr.json", key)]
    arr = np.asarray(_absdiff(a, b)).max(axis=2)
    nz = arr != 0
    w, h = a.size
    out = []
    for y0 in range(0, h, step):
        s = slice(y0, min(h, y0 + step))
        out.append((f"{y0}–{min(h, y0 + step) - 1}", int(nz[s].sum()), int(arr[s].max())))
    return out, (w, h)


def table(headers, rows):
    def cell(x):
        return str(x).replace("|", "\\|")
    lines = ["| " + " | ".join(headers) + " |", "|" + "|".join("---" for _ in headers) + "|"]
    lines += ["| " + " | ".join(cell(c) for c in r) + " |" for r in rows]
    return "\n".join(lines)


def main():
    if len(sys.argv) < 3:
        print(__doc__)
        sys.exit(2)
    a_dir, b_dir = pathlib.Path(sys.argv[1]), pathlib.Path(sys.argv[2])
    lang = sys.argv[3] if len(sys.argv) > 3 else "fr"
    report = {"avant": str(a_dir), "apres": str(b_dir), "lang": lang, "pairs": []}

    print("## Contrôles de l'instrument\n")
    print(table(["contrôle", "mesuré", "verdict"], controls(a_dir, lang)))

    print(f"\n## 1. Récolte — ce que CardPen a rendu (`{lang}/Harvest`)\n")
    h = compare_harvests(a_dir, b_dir, lang, report)
    print(table(["récolte:côté:image", "taille", "pixels", "IDAT égaux", "octets égaux", "écart"], h))

    print(f"\n## 2. Disque — PNG de `Images/`\n")
    d = compare_disk(a_dir, b_dir, lang)
    print(table(["fichier", "taille", "pixels", "IDAT égaux", "octets égaux"], d))

    print("\n## 3. Où est l'écart (faces, récolte)\n")
    print(table(["image", "identique", "pixels différents", "écart max", "faible (< 32 niveaux)",
                 "fort (>= 32 niveaux)", "lignes changées"], card7(a_dir, b_dir, lang)))

    prof, size = profile(a_dir, b_dir, lang)
    print(f"\n## 4. Profil de Rules_07 par tranches de 100 lignes (image {size[0]}×{size[1]})\n")
    print(table(["lignes", "pixels différents", "écart max"], prof))
    dpi = json.loads((a_dir / lang / "Harvest" / "Rules_harvest_fr.json").read_text(encoding="utf-8"))["Faces"].get("Dpi")
    print(f"\nDpi déclaré par la récolte : {dpi} => 38 px CSS = {38 * (dpi or 300) / 96:.1f} px de rendu")

    ident = sum(1 for p in report["pairs"] if p["identical"])
    diffn = len(report["pairs"]) - ident
    ctl = controls(a_dir, lang)
    instrument_ok = all(r[2].startswith("OK") for r in ctl)
    print(f"\n**Instrument {'SAIN' if instrument_ok else 'DÉFAILLANT — résultats non recevables'} | "
          f"Bilan récolte : {len(report['pairs'])} images — {ident} identiques, {diffn} différentes.**")
    for p in report["pairs"]:
        if not p["identical"]:
            print(f"- différente : {p['key']} — {p['n']} px, bbox {p['bbox']}, écart max {p['max']}")
    out = pathlib.Path(__file__).with_name(f"compare-{lang}.json")
    out.write_text(json.dumps(report, ensure_ascii=False, indent=1), encoding="utf-8")


if __name__ == "__main__":
    main()
