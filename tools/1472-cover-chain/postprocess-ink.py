#!/usr/bin/env python3
"""#1472 grain ③ — deterministic ink post-process for generated cover art.

The model is prompted to draw BLACK INK ON WHITE. This script turns that into
a transparent, tinted plate whose ink colour is exact BY CONSTRUCTION — no
sampling, no averaging, no per-run drift:

    1. L        = luminance (ITU-R 601, on 8-bit RGB)
    2. alpha    = clamp((white - L) / (white - black), 0, 1)   [soft threshold]
    3. RGB      = the --ink colour, uniformly, on every pixel
    4. out      = RGBA PNG

Step 2 is the "seuil, alpha" of the recipe (c.5830131319 §2): `--black` and
`--white` bracket the ink/paper range measured on the source, and everything
between them is a linear ramp — one knob pair decides how much grey survives
as partial alpha, and nothing else in the pipeline touches it.

Invariants:
    - pure function of (input bytes, --ink, --black, --white): same inputs ⇒
      identical PIXELS on any machine (no dithering, no resampling); identical
      BYTES at equal Pillow/zlib versions — the PNG encoder is not part of
      the invariant.
    - the source is NEVER overwritten; .png in, -ink.png out, side by side —
      and src == dst is REFUSED outright (exit, no write), **including the
      same file written with another letter case on Windows** (`IN.PNG` and
      `in.png` are one file there — measured T7d): samefile when both exist,
      normcase(realpath) otherwise.
    - --stats prints the alpha histogram deciles so a calibration run can be
      judged numerically instead of by eye (a blank or all-grey plate reads
      immediately as a collapsed histogram).

Usage:
    python postprocess-ink.py IN.png OUT.png --ink 1A1A1A [--black 40 --white 235]
    python postprocess-ink.py IN.png OUT.png --ink 1A1A1A --stats
"""

import argparse
import os
import sys

try:
    from PIL import Image
except ImportError:  # pragma: no cover - environment guard
    sys.exit("Pillow is required: pip install pillow")


def parse_hex(value):
    v = value.strip().lstrip("#")
    if len(v) != 6:
        raise argparse.ArgumentTypeError("expected RRGGBB")
    return tuple(int(v[i:i + 2], 16) for i in (0, 2, 4))


def same_underlying_path(a, b):
    """Windows compares paths case-insensitively: `IN.PNG` and `in.png` name
    ONE file (measured T7d), so abspath equality alone misses the case and
    the run would overwrite its source. samefile settles it by identity when
    both paths exist; normcase(realpath) covers a dst that does not exist
    yet (realpath also resolves junctions and symlinks)."""
    try:
        return os.path.samefile(a, b)
    except OSError:
        return os.path.normcase(os.path.realpath(a)) == \
            os.path.normcase(os.path.realpath(b))


def main():
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("src")
    ap.add_argument("dst")
    ap.add_argument("--ink", type=parse_hex, required=True,
                    help="ink colour RRGGBB (exact family hex)")
    ap.add_argument("--black", type=int, default=40,
                    help="luminance mapped to alpha=255 (default 40)")
    ap.add_argument("--white", type=int, default=235,
                    help="luminance mapped to alpha=0 (default 235)")
    ap.add_argument("--stats", action="store_true",
                    help="print alpha deciles and ink coverage")
    args = ap.parse_args()

    if same_underlying_path(args.src, args.dst):
        sys.exit("refusing: dst is the same file as src (case-insensitively "
                 "— Windows resolves IN.PNG and in.png to one file) — "
                 "the source must not be overwritten")

    if args.black >= args.white:
        sys.exit("--black must be < --white")

    img = Image.open(args.src).convert("RGB")
    w, h = img.size
    px = img.load()

    out = Image.new("RGBA", (w, h))
    opx = out.load()
    span = float(args.white - args.black)
    r, g, b = args.ink

    hist = [0] * 256
    for y in range(h):
        for x in range(w):
            pr, pg, pb = px[x, y]
            lum = 0.299 * pr + 0.587 * pg + 0.114 * pb
            a = (args.white - lum) / span
            a = 0.0 if a < 0.0 else (1.0 if a > 1.0 else a)
            ai = int(a * 255 + 0.5)
            hist[ai] += 1
            opx[x, y] = (r, g, b, ai)

    out.save(args.dst, "PNG")

    if args.stats:
        total = w * h
        print(f"size {w}x{h}  ink {args.ink[0]:02X}{args.ink[1]:02X}"
              f"{args.ink[2]:02X}  black={args.black} white={args.white}")
        print("alpha deciles:", end=" ")
        acc = 0
        marks = [total * i // 10 for i in range(1, 11)]
        mi = 0
        for ai in range(256):
            acc += hist[ai]
            while mi < 10 and acc >= marks[mi]:
                print(ai, end=(" " if mi < 9 else "\n"))
                mi += 1
        opaque = sum(hist[224:])
        clear = hist[0]
        print(f"fully opaque: {opaque/total:6.1%}   fully clear: {clear/total:6.1%}")
        # A plate that is >99% one bucket is blank or un-détouré — say so.
        if clear / total > 0.99:
            print("WARN: >99% fully transparent — the source is blank", file=sys.stderr)
        elif opaque / total > 0.99:
            print("WARN: >99% fully opaque — threshold not separating ink from paper",
                  file=sys.stderr)


if __name__ == "__main__":
    main()