# Card fonts — vendored, embedded at harvest (#1485)

The card templates render with three free families in place of DINPro. Their binaries live
here, in the repository, and the pipeline inlines them into the payload as `data:` URIs
(`EmbeddedFontFaces`), so a harvest fetches none of these three from a third party. The
other families the templates use are still remote — see [What is NOT
embedded](#what-is-not-embedded-measured-2026-11-26).

| Family | Files | Weights | Subsets | Used for |
| --- | --- | --- | --- | --- |
| **Barlow** | `barlow/Barlow-<weight>-<subset>.woff2` | 300 Light, 500 Medium, 700 Bold, 900 Black | `latin`, `latin-ext` | every latin-script card (`fr`, `en`, `pt`, `es`) — the substituted family |
| **Rubik** | `rubik/Rubik-cyrillic{,-ext}.woff2` | variable, `wght` 300–900 | `cyrillic`, `cyrillic-ext` | the Cyrillic glyphs of `ru` cards, per-glyph fallback |
| **Alfa Slab One** | `alfaslabone/AlfaSlabOne-Regular-latin.woff2` | 400 | `latin` | the Rules masthead (moved here by #1707/#1716) |

## Why the stack is `'Barlow', 'Rubik', sans-serif`

CSS font matching is **per glyph**: Barlow is asked first, and a character it does not
carry falls through to Rubik, then to the system sans-serif. Barlow has **no Cyrillic at
all** (measured: 62/62 Russian letters missing on all seven weights of the family, #2389),
so `ru` cards take their Latin from Barlow and their Cyrillic from Rubik without a single
`lang`-scoped rule. The two families share `cap/em` 0.700 and their x-heights sit within
0.016 em of each other (Barlow 0.504–0.520 by weight, Rubik 0.520), so a Cyrillic letter
set in Rubik has the size of the Barlow around it.

Their descents do not match — Barlow 0.200 em, Rubik 0.250 — so the line box of a line
that mixes both is taller than a Latin-only line. Measured in Chromium (100 px, `line-height:
normal`, the eleven faces below declared): Latin-only 120.00 px, Cyrillic-only 125.00 px,
mixed 125.00 px, and 125.00 px at each of the four weights. The Cyrillic advance comes from
Rubik's variable axis, which responds to `font-weight` (`АБВ` = 30.97 / 33.23 / 34.58 px at
300 / 700 / 900). A card whose lines are all Cyrillic, or all Latin, does not move.

## Provenance

Downloaded from the Google Fonts CSS API (woff2, Chrome UA — the same files
`fonts.gstatic.com` serves) on **2026-11-26**. The `unicode-range` of each face is copied
verbatim from that response: without it, two faces of the same family and weight would
replace each other instead of splitting the code space.

| File | Bytes | sha256 |
| --- | ---: | --- |
| `barlow/Barlow-Light-latin.woff2` | 22076 | `889347d7bc19d53d984a12f413457fbc8b3723757464cf514121534f626dc4db` |
| `barlow/Barlow-Light-latin-ext.woff2` | 14224 | `5d396229b78ff1a9abba40931592ce7dd5b67c63ec6137d3a7a165b2356e903e` |
| `barlow/Barlow-Medium-latin.woff2` | 22008 | `cd759df8ef9efc98fee14307b4eb5ba27f08b1f8f2f3ad2872432e25c89907a8` |
| `barlow/Barlow-Medium-latin-ext.woff2` | 14416 | `3facdf66b60778c7e64693013be7799e6686b2488dad65525b4036e211ab03e0` |
| `barlow/Barlow-Bold-latin.woff2` | 22788 | `2d797dd8b35dcb3413e1af9d7052b3f4f8c341a147cdcb01f4f06af80db53289` |
| `barlow/Barlow-Bold-latin-ext.woff2` | 14836 | `88c786d5fa8700a201615bc7b8ede09d209ec5d08cc3dd0a30ea1c4baff3476f` |
| `barlow/Barlow-Black-latin.woff2` | 22368 | `6f5d995ccc4d8ffd9e8cb4c303fc1f3646d6c62d40269ef59463332f12d5b897` |
| `barlow/Barlow-Black-latin-ext.woff2` | 15076 | `ff236c2a65201d5d0213138c1721fdc7075d5691e556fda9e0a129b826dd0a85` |
| `rubik/Rubik-cyrillic.woff2` | 15112 | `bdb2b33ab916e9e7b42edb61f8aea5f5dd83ad773d6ee4e9b55665daf8954ce8` |
| `rubik/Rubik-cyrillic-ext.woff2` | 12388 | `35e30483bb79be6fd8de0fec4e038fda9e637fc7337f6ff61418c7d08077878b` |
| `alfaslabone/AlfaSlabOne-Regular-latin.woff2` | 19052 | `48a110c7fda81f9921a6437e1a813dcea56df5a9f52f778c7b2f2151e36e4f2c` |

194 344 bytes of woff2 in total; replayed with `sha256sum Cards/fonts/*/*.woff2`.

## Licences

All three families are **SIL Open Font License 1.1** — the licence text travels with the
binaries under `licences/`, as the OFL requires. Each file also carries the licence in its
own `name` table (IDs 13/14 = `http://scripts.sil.org/OFL`), which is what a font-tool
audit reads:

- `licences/Barlow-OFL.txt` — Copyright 2017 The Barlow Project Authors
- `licences/Rubik-OFL.txt` — Copyright 2015 The Rubik Project Authors
- `licences/AlfaSlabOne-OFL.txt` — Copyright 2016 The Alfa Slab One Project Authors

OFL permits embedding and redistribution of the binaries inside a larger work, which is
what both the repository and the generated PDFs do. It forbids selling the fonts on their
own — not applicable here.

## What is NOT embedded (measured 2026-11-26)

The twelve living card templates still load **one Google Fonts stylesheet each** through
their `extCSS` key, which CardPen turns into `<link rel="stylesheet" crossorigin="anonymous">`
in the output document (`Generation/CardPen/js/main.js`, "Summon the goog"). Five distinct
URLs carry the families below. File counts and bytes are what `fonts.gstatic.com` serves
for the same request (css2 API, Chrome UA, `display=swap`; bytes = sum of the
`Content-Length` of every woff2 of the response):

| Family (as requested) | Templates | woff2 files | Bytes | Note |
| --- | ---: | ---: | ---: | --- |
| `Noto Sans SC` 400, 700 | 11 | **101** | **4 500 544** | the `zh` glyph font, served in one slice per `unicode-range` and per weight |
| `Noto Naskh Arabic` 400, 700 | 11 | 5 | 147 812 | second in the `ar`/`fa` stack |
| `Vazirmatn` 400, 700 | 11 | 3 | 102 692 | first in the `ar`/`fa` stack |
| `Bebas Neue` | 10 | 2 | 22 696 | brand mark and headings |
| `PT Sans` 700 | 9 | 4 | 146 552 | body text |
| `Poppins`, `Raleway` (18 weights each) | 1 | not measured | not measured | Fallacies-3 document only |
| `Oswald`, `PT Sans Narrow`, `Dosis`, `Noto Sans JP` | 1 | not measured | not measured | the Memo back only |

Those loads are load-bearing — the ar/fa/zh scoped rules name exactly the three glyph
fonts — and they are why this directory is not the whole story: a harvest is offline **for
the three substituted families**, not offline.

`ExternalFontHostRatchetContractTests` pins the remainder URL by URL, and fails if any
template names a font host (`googleapis.com`, `gstatic.com`, `cdnfonts.com`,
`onlinewebfonts.com`, …) **outside** its `extCSS`, or declares a `@font-face` with a remote
`src` in any key. Embedding the rest is a separate decision with a measured price: the
brand pair alone (Bebas Neue + PT Sans 700) is 6 files for 169 248 bytes, the `zh` glyph
font is 4.5 MB — twenty-three times the whole of this directory — in every payload that
carries it.

## Replacing or adding a family

1. Fetch the woff2 subsets and the `OFL.txt` next to them.
2. Drop them under a family directory, keeping `Barlow-<weight>-<subset>.woff2` naming.
3. Declare the face in `EmbeddedFontFaces.Faces` (family, weight or weight range, file,
   `unicode-range` verbatim from the CSS that served the file).
4. Reference the family from the template's `font-family` list, **after** Barlow so the
   latin path does not change.

Nothing may be loaded from `url(http…)` in a template's `@font-face`: no living template
declares one any more, and `ExternalFontHostRatchetContractTests` fails if one comes back.
