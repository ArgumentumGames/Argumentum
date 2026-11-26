"""#1485 -- couverture de jeu de caracteres des polices, sur le corpus REEL.

Repond a une seule question, mais celle qui decide d'une substitution : pour une
police DONNEE, quels caracteres du corpus ne peuvent pas etre rendus, et le
navigateur bascule-t-il alors silencieusement sur la famille suivante ?

Le corpus est lu dans l'arbre (`git show <ref>:<fichier>`), pas dans le worktree,
pour que la mesure soit reproductible quelle que soit la branche courante.

Seaux -- un caractere n'est teste que dans sa propre ecriture, partout ou il
apparait :
  ru-cyr  lettres cyrilliques rencontrees dans une colonne `*_ru`
  ru-oth  le reste des colonnes `*_ru` (chiffres, ponctuation, mots latins)
  la-let  lettres latines des colonnes `_fr/_en/_es/_pt`, des colonnes NUES
          qu'un gabarit rend en `{{…}}`, et du texte statique des gabarits
  la-oth  le reste de ces memes colonnes et textes

Une colonne nue n'est lue que si le mustache d'un gabarit rend une variable de ce
nom : c'est le mustache qui decide qu'un caractere est imprime. Sans cette regle,
`È` (Scenarii, « Adam et Ève », rendu par `{{enjeu}}` / `{{contexte}}`) manquait
au seau latin, alors qu'a l'inverse `Remarques` ou `image`, qu'aucun gabarit ne
rend, gonfleraient le seau d'un texte jamais imprime.

⚠️ Pourquoi ce decoupage : un verdict de substitution mesure sur `fr` ne dit RIEN
du deck `ru`. Les gabarits sont uniques (`*_fr.json`) et servent les 8 langues ;
`ARGU_LANG_MARKER` (`AssetConverterConfig.cs`) pose `argu-lang-<langue>` sur la
carte, et les seules regles de police portees par une classe de langue sont
`.argu-lang-ar`, `.argu-lang-fa` et `.argu-lang-zh` — il n'en existe aucune pour
fr/en/ru/pt/es (`font-selectors.py`, compagnon de cet outil, le mesure gabarit par
gabarit). Un `@font-face` remplace se paie donc en cyrillique aussi, et le repli
est muet.

Metriques verticales : `x/em` et `cap/em` (OS/2 sxHeight / sCapHeight). Deux
familles differentes sur une meme ligne se trahissent la : une ligne russe ou
les mots latins viennent d'une famille et les lettres cyrilliques d'une autre se
voit au decalage des hauteurs. `axes` dit quelles graisses une police variable
peut atteindre -- un axe `wght 100..900` couvre les 300/500/700/900 declares par
les gabarits, un axe `wght 400..900` ne couvre PAS le 300.

Dependances : aucune (stdlib seule). WOFF 1.0 = tables zlib, lisible sans brotli.
WOFF2 est REFUSE avec une erreur nommee (il faudrait le module `brotli`) : mieux
vaut un echec bruyant qu'une absence de mesure lue comme une couverture.

Reseau : les polices et les `METADATA.pb` sont telecharges puis caches dans
`--cache` (defaut : %TEMP%/argumentum-fontcache).

Une source en echec -- URL injoignable, `family=` inconnue (404) -- donne une
ligne `ERREUR` a sa place, et les autres polices du meme appel restent mesurees.
Seul un conteneur illisible (WOFF2) sort en erreur nommee, expres.

Usage:
  python -I font-coverage.py --repo <racine Argumentum> [--ref origin/master] \\
      [--out rapport.md] <police...>

  <police> = une URL https, un chemin local, `etiquette=url`, ou
             `family=<dossier de google/fonts>` qui deplie les statiques
             Light/Medium/Bold/Black de la famille (italiques ecartees), et a
             defaut sa police variable.

Exemple -- la decision de #1485 (les 4 graisses declarees par les gabarits et
les candidates qui couvrent les deux ecritures) :
  python -I font-coverage.py --repo . family=barlow family=rubik \\
      "ref DINPro-Light=https://fonts.cdnfonts.com/s/18774/DINPro-Light%20tr.woff"

Lecture seule sur le depot.
"""
import argparse
import csv
import io
import json
import re
import struct
import subprocess
import tempfile
import urllib.request
import zlib
from pathlib import Path

UA = ("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
      "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36")

BUCKETS = ("ru-cyr", "ru-oth", "la-let", "la-oth")

# `{{nom}}`, `{{{nom}}}`, `{{#nom}}`, `{{/nom}}`, `{{^nom}}`, `{{&nom}}` : le nom
# d'une variable, sans ses accolades ni son marqueur. Un helper Handlebars
# (`{{#ifCond a b}}`) est capture aussi -- aucun CSV ne porte une telle colonne.
VARIABLE = re.compile(r"\{\{\{?\s*[#^/>!&]?\s*([A-Za-z0-9_.-]+)")


# ------------------------------------------------------------------ telechargement
def fetch(url: str, label: str, cache: Path) -> bytes:
    cache.mkdir(parents=True, exist_ok=True)
    hit = cache / re.sub(r"[^A-Za-z0-9._-]+", "_", label)[:80]
    if hit.exists():
        return hit.read_bytes()
    req = urllib.request.Request(url, headers={"User-Agent": UA})
    with urllib.request.urlopen(req, timeout=90) as resp:
        data = resp.read()
    hit.write_bytes(data)
    return data


# ------------------------------------------------------------- conteneur sfnt
def sfnt_tables(data: bytes) -> dict:
    """tag -> octets de la table, pour un sfnt nu ou un conteneur WOFF 1.0."""
    if data[:4] == b"wOFF":
        (num, ) = struct.unpack(">H", data[12:14])
        tables = {}
        for i in range(num):
            off = 44 + i * 20
            tag = data[off:off + 4].decode("latin-1")
            toff, comp, orig = struct.unpack(">III", data[off + 4:off + 16])
            blob = data[toff:toff + comp]
            tables[tag] = zlib.decompress(blob) if comp < orig else blob
        return tables
    if data[:4] == b"wOF2":
        raise SystemExit("WOFF2 demande le module brotli ; fournir la source .ttf ou .woff")
    if data[:4] not in (b"\x00\x01\x00\x00", b"OTTO", b"true", b"ttcf"):
        raise SystemExit(f"conteneur sfnt non reconnu : premiers octets {data[:8]!r}")
    if data[:4] == b"ttcf":                       # collection : premiere police
        (first, ) = struct.unpack(">I", data[12:16])
        data = data[first:]
    (num, ) = struct.unpack(">H", data[4:6])
    tables = {}
    for i in range(num):
        off = 12 + i * 16
        tag = data[off:off + 4].decode("latin-1")
        toff, tlen = struct.unpack(">II", data[off + 8:off + 16])
        tables[tag] = data[toff:toff + tlen]
    return tables


def _score(plat: int, enc: int) -> int:
    """Quelle sous-table croire : une Unicode complete plutot qu'une BMP seule."""
    return {(3, 10): 6, (0, 6): 6, (0, 4): 5, (3, 1): 4,
            (0, 3): 3, (0, 2): 2, (0, 1): 2, (0, 0): 2}.get((plat, enc), 1)


def cmap_codepoints(tables: dict) -> set:
    """Points de code qui rendent un glyphe non nul, d'apres la meilleure sous-table."""
    cmap = tables["cmap"]
    (n, ) = struct.unpack(">H", cmap[2:4])
    chosen, best = None, -1
    for i in range(n):
        plat, enc, sub_off = struct.unpack(">HHI", cmap[4 + i * 8:12 + i * 8])
        (fmt, ) = struct.unpack(">H", cmap[sub_off:sub_off + 2])
        if fmt in (4, 12) and _score(plat, enc) > best:
            chosen, best = (sub_off, fmt), _score(plat, enc)
    if chosen is None:
        raise SystemExit("aucune sous-table cmap de format 4 ou 12")
    sub_off, fmt = chosen
    covered = set()
    if fmt == 4:
        (segx2, ) = struct.unpack(">H", cmap[sub_off + 6:sub_off + 8])
        seg = segx2 // 2
        ends = struct.unpack(f">{seg}H", cmap[sub_off + 14:sub_off + 14 + segx2])
        starts_off = sub_off + 14 + segx2 + 2                 # + reservedPad
        starts = struct.unpack(f">{seg}H", cmap[starts_off:starts_off + segx2])
        deltas_off = starts_off + segx2
        deltas = struct.unpack(f">{seg}h", cmap[deltas_off:deltas_off + segx2])
        ros_off = deltas_off + segx2
        ranges = struct.unpack(f">{seg}H", cmap[ros_off:ros_off + segx2])
        for i in range(seg):
            if starts[i] == 0xFFFF:
                continue
            for c in range(starts[i], ends[i] + 1):
                if ranges[i] == 0:
                    gid = (c + deltas[i]) & 0xFFFF
                else:
                    gpos = ros_off + i * 2 + ranges[i] + (c - starts[i]) * 2
                    if gpos + 2 > len(cmap):
                        continue
                    (gid, ) = struct.unpack(">H", cmap[gpos:gpos + 2])
                    if gid:
                        gid = (gid + deltas[i]) & 0xFFFF
                if gid:
                    covered.add(c)
    else:
        (ngroups, ) = struct.unpack(">I", cmap[sub_off + 12:sub_off + 16])
        p = sub_off + 16
        for _ in range(ngroups):
            start, end, _start_gid = struct.unpack(">III", cmap[p:p + 12])
            covered.update(range(start, end + 1))             # format 12 : pas de trou dans un groupe
            p += 12
    return covered


def axes(tables: dict) -> str:
    """'static', ou les plages d'axes fvar -- donc les graisses atteignables."""
    if "fvar" not in tables:
        return "static"
    fv = tables["fvar"]
    (axes_off, ) = struct.unpack(">H", fv[4:6])
    (count, ) = struct.unpack(">H", fv[8:10])
    (size, ) = struct.unpack(">H", fv[10:12])
    out = []
    for i in range(count):
        off = axes_off + i * size
        tag = fv[off:off + 4].decode("latin-1")
        mn, df, mx = (v / 65536.0 for v in struct.unpack(">iii", fv[off + 4:off + 16]))
        out.append(f"{tag} {mn:g}..{mx:g}")
    return "variable " + ", ".join(out)


def wght_reach(tables: dict) -> str:
    """Les 300/500/700/900 declares par les gabarits sont-ils atteignables ?

    Pour une statique, la graisse declaree ; pour une variable, la plage de l'axe
    `wght`. Les deux sont mesures, jamais supposes par le nom du fichier.
    """
    want = (300, 500, 700, 900)
    if "fvar" in tables:
        fv = tables["fvar"]
        (axes_off, ) = struct.unpack(">H", fv[4:6])
        (count, ) = struct.unpack(">H", fv[8:10])
        (size, ) = struct.unpack(">H", fv[10:12])
        for i in range(count):
            off = axes_off + i * size
            if fv[off:off + 4] == b"wght":
                mn, _df, mx = (v / 65536.0 for v in struct.unpack(">iii", fv[off + 4:off + 16]))
                missing = [w for w in want if not mn <= w <= mx]
                return "300/500/700/900" if not missing else "manque " + ", ".join(map(str, missing))
    (weight, ) = struct.unpack(">H", tables["OS/2"][4:6])
    return f"statique {weight}"


def os2_metrics(tables: dict) -> tuple:
    """(graisse, hauteur d'x / em, hauteur de capitale / em) -- les proportions
    verticales qu'affiche une ligne mixte."""
    os2, head = tables["OS/2"], tables["head"]
    (upem, ) = struct.unpack(">H", head[18:20])
    (ver, ) = struct.unpack(">H", os2[0:2])
    (weight, ) = struct.unpack(">H", os2[4:6])
    xh = ch = None
    if ver >= 2 and len(os2) >= 90:
        xh, ch = struct.unpack(">hh", os2[86:90])
    return weight, (xh / upem if xh else None), (ch / upem if ch else None)


def resolve(spec: str, cache: Path):
    """`family=<dossier google/fonts>` deplie les statiques Light/Medium/Bold/Black
    (italiques ecartees), sinon la police variable. Le reste est un couple
    (etiquette, source) : `etiquette=url`, ou URL / chemin nu."""
    if spec.startswith("family="):
        fam = spec[len("family="):]
        meta = fetch(f"https://raw.githubusercontent.com/google/fonts/main/ofl/{fam}/METADATA.pb",
                     f"meta_{fam}", cache)
        files = re.findall(r'filename:\s*"([^"]+)"', meta.decode("utf-8", errors="replace"))
        base = f"https://raw.githubusercontent.com/google/fonts/main/ofl/{fam}/"
        static = [f for f in files
                  if "Italic" not in f and "[" not in f
                  and re.search(r"(Light|Medium|Bold|Black)", f)]
        if static:
            return [(f"{fam} {f.rsplit('-', 1)[-1][:-4]}", base + f.replace(" ", "%20"))
                    for f in static]
        var = [f for f in files if "[" in f]
        return [(f"{fam} [variable]", base + var[0].replace(" ", "%20"))] if var else []
    label, sep, src = spec.partition("=")
    return [(label, src)] if sep else [(src, src)]


# ------------------------------------------------------------------ corpus
def _dialect(text: str):
    try:
        return csv.Sniffer().sniff(text[:4096], delimiters=",;\t")
    except csv.Error:
        return csv.excel


def corpus(repo: Path, ref: str) -> dict:
    """Caracteres des CSV vivants et des gabarits, par seau. Lus dans <ref>.

    Deux passages : les gabarits d'abord, pour connaitre les noms de variables
    qu'un mustache rend ; puis les CSV, ou une colonne nue n'est lue que si son
    nom figure parmi eux.
    """
    out = subprocess.run(["git", "-C", str(repo), "ls-tree", "-r", "--name-only", ref, "Cards/"],
                         check=True, capture_output=True).stdout.decode("utf-8").split("\n")
    living = [f for f in out if f and "/Archive/" not in f and f.endswith((".csv", ".json"))]

    def show(rel: str) -> bytes:
        return subprocess.run(["git", "-C", str(repo), "show", f"{ref}:{rel}"],
                              capture_output=True).stdout

    buckets = {k: set() for k in BUCKETS}
    rendered = set()                        # noms de variables qu'un mustache rend
    n_csv = n_tpl = n_cells = 0
    for rel in (r for r in living if r.endswith(".json")):
        n_tpl += 1
        try:
            doc = json.loads(show(rel).decode("utf-8-sig"))
        except json.JSONDecodeError:
            continue
        for key in ("mustache", "notes"):
            val = doc.get(key) or ""
            if not isinstance(val, str):
                continue
            if key == "mustache":
                rendered.update(v.lower() for v in VARIABLE.findall(val))
            buckets["la-let"].update(c for c in val if c.isalpha())
            buckets["la-oth"].update(c for c in val if not c.isalpha())
    for rel in (r for r in living if r.endswith(".csv")):
        n_csv += 1
        text = show(rel).decode("utf-8-sig", errors="replace")
        for row in csv.DictReader(io.StringIO(text), dialect=_dialect(text)):
            for key, val in row.items():
                if not val or key is None:
                    continue
                k = key.strip()
                if k.endswith("_ru"):
                    n_cells += 1
                    buckets["ru-cyr"].update(c for c in val if 0x0400 <= ord(c) <= 0x052F)
                    buckets["ru-oth"].update(c for c in val if not 0x0400 <= ord(c) <= 0x052F)
                elif re.search(r"_(fr|en|es|pt)$", k) or k.lower() in rendered:
                    buckets["la-let"].update(c for c in val if c.isalpha())
                    buckets["la-oth"].update(c for c in val if not c.isalpha())
    for name in buckets:                    # controles et fins de ligne : aucun glyphe requis
        buckets[name] = {c for c in buckets[name] if ord(c) >= 0x20 and c not in "\t\r\n"}
    return {"buckets": buckets, "csv": n_csv, "tpl": n_tpl, "ru_cells": n_cells}


def describe(chars: set, limit: int = 48) -> str:
    ordered = sorted(chars, key=ord)
    shown = " ".join(f"U+{ord(c):04X}" for c in ordered[:limit])
    return shown + (f" …(+{len(ordered) - limit})" if len(ordered) > limit else "")


def both(cover: dict) -> str:
    """Les deux ecritures sont-elles servies par cette seule police ?"""
    return "oui" if not cover["ru-cyr"] and not cover["la-let"] else "NON"


# -------------------------------------------------------------------- main
def main() -> None:
    ap = argparse.ArgumentParser()
    ap.add_argument("--repo", required=True)
    ap.add_argument("--ref", default="origin/master")
    ap.add_argument("--out", help="rapport markdown")
    ap.add_argument("--cache", default=str(Path(tempfile.gettempdir()) / "argumentum-fontcache"))
    ap.add_argument("fonts", nargs="+")
    a = ap.parse_args()
    repo, cache = Path(a.repo), Path(a.cache)

    corp = corpus(repo, a.ref)
    b = corp["buckets"]
    print(f"corpus @ {a.ref} : {corp['csv']} CSV vivants ({corp['ru_cells']} cellules `*_ru`), "
          f"{corp['tpl']} JSON vivants")
    for name in BUCKETS:
        print(f"  {name} : {len(b[name])} caracteres  {describe(b[name])}")

    rows = []
    for spec in a.fonts:
        try:                                  # `resolve` est resolue par le reseau : un
            pairs = resolve(spec, cache)      # `family=` inconnu (404) ne doit pas perdre le tableau
        except Exception as exc:
            rows.append((spec, 0, 0, f"ERREUR {type(exc).__name__}: {str(exc)[:70]}",
                         None, None, "-"))
            continue
        for label, src in pairs:
            try:
                data = fetch(src, label, cache) if src.startswith("http") else Path(src).read_bytes()
                tables = sfnt_tables(data)
                cov = cmap_codepoints(tables)
            except Exception as exc:          # une source cassee ne doit pas perdre le tableau
                rows.append((label, 0, 0, f"ERREUR {type(exc).__name__}: {str(exc)[:70]}",
                             None, None, "-"))
                continue
            missing = {n: {c for c in b[n] if ord(c) not in cov} for n in BUCKETS}
            rows.append((label, len(data), len(cov), axes(tables), missing,
                         os2_metrics(tables), wght_reach(tables)))

    print()
    print(f"{'police':30s} {'octets':>8s} {'cmap':>5s}  "
          + " ".join(f"{k:>6s}" for k in BUCKETS)
          + f"  {'les 2':>5s}  {'x/em':>5s} {'cap':>5s}  graisses sur 300/500/700/900, axes")
    print("-" * 150)
    for label, size, n, ax, missing, m, reach in rows:
        if missing is None:
            print(f"{label:30s} {ax}")
            continue
        cells = " ".join(f"{len(missing[k]):6d}" for k in BUCKETS)
        _w, xh, ch = m or (None, None, None)
        fmt = lambda v: "-" if v is None else f"{v:.3f}"
        print(f"{label:30s} {size:8d} {n:5d}  {cells}  {both(missing):>5s}  "
              f"{fmt(xh):>5s} {fmt(ch):>5s}  {reach} | {ax}")
    print("  (tailles des seaux : " + " ".join(f"{k} {len(b[k])}" for k in BUCKETS) + ")")
    print()
    for label, size, n, ax, missing, m, reach in rows:
        print(f"[{label}] {ax}")
        if missing:
            for k in BUCKETS:
                if missing[k]:
                    print(f"   {k} manquants ({len(missing[k])}/{len(b[k])}) : {describe(missing[k])}")

    if a.out:
        with open(a.out, "w", encoding="utf-8", newline="\n") as fh:
            fh.write(f"corpus @ {a.ref} : {corp['csv']} CSV vivants ({corp['ru_cells']} cellules "
                     f"`*_ru`), {corp['tpl']} JSON vivants. Tailles des seaux : "
                     + ", ".join(f"{k} {len(b[k])}" for k in BUCKETS) + ".\n\n")
            fh.write("| police | octets | cmap | " + " | ".join(BUCKETS)
                     + " | les deux écritures | x/em | cap/em | graisses | axes |\n")
            fh.write("|---|---:|---:|" + "---:|" * len(BUCKETS) + ":-:|---:|---:|---:|---|\n")
            for label, size, n, ax, missing, m, reach in rows:
                if missing is None:
                    fh.write(f"| {label} | - | - | " + " | ".join(["-"] * len(BUCKETS))
                             + f" | - | - | - | - | {ax} |\n")
                    continue
                _w, xh, ch = m or (None, None, None)
                fmt = lambda v: "-" if v is None else f"{v:.3f}"
                fh.write(f"| {label} | {size} | {n} | "
                         + " | ".join(f"{len(missing[k])}/{len(b[k])}" for k in BUCKETS)
                         + f" | {both(missing)} | {fmt(xh)} | {fmt(ch)} | {reach} | {ax} |\n")
            for label, size, n, ax, missing, m, reach in rows:
                if not missing:
                    continue
                for k in BUCKETS:
                    if missing[k]:
                        fh.write(f"\n`{label}` -- {k} manquants ({len(missing[k])}/{len(b[k])}) : "
                                 f"{describe(missing[k], 400)}\n")


if __name__ == "__main__":
    main()
