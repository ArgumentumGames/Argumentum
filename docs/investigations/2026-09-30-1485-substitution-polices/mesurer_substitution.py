"""Substitution de police : construire le CSS candidat, rendre, mesurer.

Trois modes, dans l'ordre ou l'on s'en sert :

  css      construit un bloc @font-face de substitution a partir de Google Fonts,
           woff2 embarques en base64 -> le rendu ne depend d'aucun reseau.
  rendre   rend une face REELLE via le vrai moteur CardPen, et dit quelles polices
           ont effectivement servi a la peindre.
  mesurer  compare plusieurs configurations de police sur un echantillon de cartes
           et chiffre ce qui bouge (hauteur des blocs, nombre de lignes, debordement).

Le protocole de pilotage de CardPen est celui de
`Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Cardpen/HarvestManager.cs`
(attendre `typeof cardpen`, `cardpen.form.set`, `cardpen.write.generate`, puis dans
l'iframe `#cpOutput` : `#generateButton` -> `generateImages()` -> `#zipButton`).

Pourquoi garder le nom d'origine dans le bloc de substitution
------------------------------------------------------------
Le bloc candidat redeclare `font-family: 'DINpro'` (ou `'TrendSlabW00-Four'`), donc
AUCUN selecteur du gabarit n'est touche : seuls les blocs @font-face changent. Le
diff est minimal et se defait en retirant le bloc.

Dependances : playwright (chromium installe) ; fontTools seulement pour `--couverture`.
    pip install playwright fonttools && playwright install chromium
"""

import argparse
import base64
import csv as _csv
import io
import json
import re
import time
import urllib.parse
import urllib.request
from pathlib import Path

CARDEN_URL = "https://argumentum.myia.io/index.html"
RAW_BASE = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/"
UA = ("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
      "(KHTML, like Gecko) Chrome/126.0 Safari/537.36")

# Poids declares par les gabarits Fallacies : 300/500/700/900. Le navigateur choisit
# le plus proche, donc les preserver tous les quatre reproduit le comportement
# d'origine (`.exemple_fr` demande 400, absent du jeu -> recoit 500).
POIDS_GABARIT = [300, 500, 700, 900]


def telecharger(url: str) -> bytes:
    req = urllib.request.Request(url, headers={"User-Agent": UA})
    with urllib.request.urlopen(req, timeout=60) as r:
        return r.read()


def rewrite_image_paths(mustache: str) -> str:
    """Reproduit HarvestManager.RewriteImagePathsToAbsoluteUrls : les `src="../../..."`
    des gabarits deviennent des URL raw-master, sans quoi les illustrations 404."""
    def sub(m):
        q, chemin = m.group(1), m.group(2)
        chemin = re.sub(r"^(\.\./)+", "", chemin)
        return f"src={q}{RAW_BASE}{chemin}{q}"
    return re.sub(r'src=(["\'])((?:\.\./)+[^"\']+)\1', sub, mustache)


# --------------------------------------------------------------------------- css

def _bloc_latin(css: str, poids: int):
    """URL du woff2 du sous-ensemble latin pour ce poids (Google Fonts css2)."""
    for m in re.finditer(r"/\*\s*([a-z\-]+)\s*\*/\s*@font-face\s*\{(.*?)\}", css, re.S):
        sous_ensemble, corps = m.group(1), m.group(2)
        if sous_ensemble != "latin":
            continue
        mw = re.search(r"font-weight\s*:\s*(\d+)", corps)
        if not mw or int(mw.group(1)) != poids:
            continue
        mu = re.search(r"url\((https://[^)]+\.woff2)\)", corps)
        if mu:
            return mu.group(1)
    return None


def mode_css(args):
    famille, nom, poids = args.famille, args.nom, args.poids
    url = ("https://fonts.googleapis.com/css2?family="
           f"{urllib.parse.quote(famille)}:wght@{';'.join(str(p) for p in poids)}"
           "&display=swap")
    css = telecharger(url).decode("utf-8")

    urls = {}
    for p in poids:
        u = _bloc_latin(css, p)
        if not u:
            raise SystemExit(f"poids {p} introuvable pour {famille}")
        urls[p] = u

    lignes = [f"/* Substitution : {famille} -> @font-face '{nom}' */"]
    if len(set(urls.values())) == 1:
        # police variable : un seul fichier couvre la plage, une seule declaration
        data = telecharger(next(iter(urls.values())))
        axe = str(poids[0]) if len(poids) == 1 else f"{poids[0]} {poids[-1]}"
        lignes.append(
            "@font-face {\n"
            f"    font-family: '{nom}';\n    font-style: normal;\n"
            f"    font-weight: {axe};\n    font-display: swap;\n"
            f"    src: url(data:font/woff2;base64,"
            f"{base64.b64encode(data).decode('ascii')}) format('woff2');\n}}")
        print(f"  {famille} variable w={axe} ({len(data)} o)")
    else:
        for p in poids:
            data = telecharger(urls[p])
            lignes.append(
                "@font-face {\n"
                f"    font-family: '{nom}';\n    font-style: normal;\n"
                f"    font-weight: {p};\n    font-display: swap;\n"
                f"    src: url(data:font/woff2;base64,"
                f"{base64.b64encode(data).decode('ascii')}) format('woff2');\n}}")
            print(f"  {famille} w={p} ({len(data)} o)")
    Path(args.sortie).write_text("\n".join(lignes) + "\n", encoding="utf-8")
    print(f"OK -> {args.sortie}")


# ------------------------------------------------------------------------- rendre

def selection_lignes(csv_texte: str, pk=None, ligne=None, n=None) -> str:
    """Garde l'en-tete + les lignes demandees, via le lecteur CSV et jamais
    splitlines() : un champ peut contenir des newlines, qu'un decoupage ligne a
    ligne couperait en plein champ."""
    rdr = list(_csv.reader(io.StringIO(csv_texte)))
    if not rdr:
        return csv_texte
    entete, corps = rdr[0], rdr[1:]
    if pk is not None:
        garde = [r for r in corps if r and r[0] == str(pk)]
    elif ligne is not None:
        garde = [corps[ligne - 1]] if 1 <= ligne <= len(corps) else []
    else:
        garde = corps[: (n or 1)]
    out = io.StringIO()
    w = _csv.writer(out, lineterminator="\n")
    w.writerow(entete)
    w.writerows(garde)
    return out.getvalue()


def preparer_gabarit(chemin: str, css_police=None):
    """Charge le gabarit, absolutise les images, et remplace le CSS de police par
    les seules familles que le bloc candidat redeclare."""
    payload = json.loads(Path(chemin).read_text(encoding="utf-8-sig"))
    payload["mustache"] = rewrite_image_paths(payload.get("mustache", ""))
    if not css_police:
        return payload
    bloc = Path(css_police).read_text(encoding="utf-8")
    css = payload["css"]
    familles = set(re.findall(r"font-family\s*:\s*['\"]([^'\"]+)['\"]", bloc))
    for fam in sorted(familles):
        # insensible a la casse : les gabarits declarent 'DINPro' alors que les
        # selecteurs consomment 'DINpro', et le nom du bloc candidat suit les seconds
        motif = r"@font-face\s*\{[^}]*" + re.escape(fam) + r"[^}]*\}"
        n = len(re.findall(motif, css, re.S | re.I))
        css = re.sub(motif, "", css, flags=re.S | re.I)
        print(f"  @font-face {fam} remplaces : {n}")
    payload["css"] = css + "\n" + bloc
    return payload


SONDE_POLICES = """() => {
  const out = {chargees: [], checks: {}};
  try {
    for (const f of document.fonts)
      if (f.status === 'loaded') out.chargees.push(`${f.family} | w=${f.weight}`);
  } catch (e) { out.err = String(e); }
  for (const s of ["300 16px 'DINpro'", "500 16px 'DINpro'",
                   "700 16px 'DINpro'", "900 16px 'DINpro'"])
    out.checks[s] = document.fonts.check(s);
  return out;
}"""

SONDE_MISE_EN_PAGE = """() => {
  const bloc = (sel) => {
    const e = document.querySelector(sel);
    if (!e) return null;
    const rects = (() => {
      const r = document.createRange(); r.selectNodeContents(e);
      return new Set(Array.from(r.getClientRects())
        .filter(x => x.width > 1).map(x => Math.round(x.top))).size;
    })();
    return { h: Math.round(e.getBoundingClientRect().height * 10) / 10,
             lignes: rects || 1,
             deborde: e.scrollHeight - e.clientHeight > 1 };
  };
  const racine = document.querySelector('card');
  return { desc: bloc('.desc_fr'), ex: bloc('.exemple_fr'), h1: bloc('h1'),
           racine_deborde: racine ? racine.scrollHeight - racine.clientHeight > 1 : null };
}"""


def _piloter(page, payload):
    """Joue une carte et rend l'iframe de sortie, prete a interroger."""
    page.evaluate("(j) => cardpen.form.set(JSON.parse(j))", json.dumps(payload))
    page.evaluate("cardpen.write.generate(cardpen.form.get(), 'image')")
    el = page.query_selector("#cpOutput")
    if el is None:
        raise RuntimeError("#cpOutput introuvable")
    iframe = el.content_frame()
    if iframe is None:
        raise RuntimeError("contenu de #cpOutput inaccessible")
    iframe.locator("#generateButton").wait_for(state="visible", timeout=120000)
    iframe.evaluate("generateImages()")
    iframe.locator("#zipButton").wait_for(state="visible", timeout=300000)
    return iframe


def mode_rendre(args):
    from playwright.sync_api import sync_playwright

    payload = preparer_gabarit(args.gabarit, args.css_police)
    payload["csv"] = selection_lignes(payload["csv"], args.pk, args.ligne, args.n)
    sortie = Path(args.sortie)
    sortie.mkdir(parents=True, exist_ok=True)

    with sync_playwright() as p:
        nav = p.chromium.launch(headless=True)
        page = nav.new_page(viewport={"width": 1600, "height": 1200})
        page.set_default_timeout(120000)
        page.goto(CARDEN_URL, timeout=120000)
        page.wait_for_function(
            "() => typeof cardpen !== 'undefined' && cardpen.form && cardpen.write",
            timeout=120000)
        t0 = time.time()
        iframe = _piloter(page, payload)
        if args.sonde:
            info = iframe.evaluate(SONDE_POLICES)
            print(f"  polices chargees : {sorted(set(info['chargees']))}")
            print(f"  check() : {json.dumps(info['checks'])}")
        srcs = iframe.evaluate(
            "() => Array.from(document.querySelectorAll('img'))"
            ".map(i => i.getAttribute('src') || '')"
            ".filter(s => s.startsWith('data:image'))")
        ecrites = []
        for i, s in enumerate(srcs):
            chemin = sortie / f"{args.tag}_{i:03d}.png"
            chemin.write_bytes(base64.b64decode(s.split(",", 1)[1]))
            ecrites.append(str(chemin))
        nav.close()
    print(f"OK {len(ecrites)} image(s) en {time.time() - t0:.1f}s")
    for c in ecrites:
        print("  ", c)


# ------------------------------------------------------------------------ mesurer

def mode_mesurer(args):
    from playwright.sync_api import sync_playwright

    base = preparer_gabarit(args.gabarit, None)
    css_reference = base["css"]
    configs = []
    for c in args.config:
        nom, _, chemin = c.partition("=")
        configs.append((nom, preparer_gabarit(args.gabarit, chemin)["css"]
                        if chemin else css_reference))

    pks = [x.strip() for x in args.pks.split(",") if x.strip()]
    resultats = []
    with sync_playwright() as p:
        nav = p.chromium.launch(headless=True)
        page = nav.new_page(viewport={"width": 1600, "height": 1200})
        page.set_default_timeout(120000)
        page.goto(CARDEN_URL, timeout=120000)
        page.wait_for_function(
            "() => typeof cardpen !== 'undefined' && cardpen.form && cardpen.write",
            timeout=120000)
        for nom, css in configs:
            t0 = time.time()
            for pk in pks:
                payload = dict(base)
                payload["css"] = css
                payload["csv"] = selection_lignes(base["csv"], pk=pk)
                iframe = _piloter(page, payload)
                m = iframe.evaluate(SONDE_MISE_EN_PAGE)
                m["pk"], m["config"] = pk, nom
                resultats.append(m)
            print(f"  {nom}: {len(pks)} cartes en {time.time() - t0:.1f}s")
        nav.close()

    Path(args.sortie).write_text(json.dumps(resultats, ensure_ascii=False, indent=1),
                                 encoding="utf-8")

    par_config = {}
    for r in resultats:
        par_config.setdefault(r["config"], []).append(r)
    reference = par_config[configs[0][0]]

    def moy(rs, cle, champ="h"):
        vals = [r[cle][champ] for r in rs if r.get(cle)]
        return sum(vals) / len(vals) if vals else 0.0

    idx = {(r["config"], r["pk"]): r for r in resultats}
    print(f"\n{'config':<14}{'desc(px)':>10}{'ex(px)':>9}{'lignes desc':>13}"
          f"{'cartes qui bougent':>20}")
    for nom, _ in configs:
        rs = par_config[nom]
        if nom == configs[0][0]:
            bouge = "(reference)"
        else:
            n = 0
            for r in reference:
                c = idx.get((nom, r["pk"]))
                if not c:
                    continue
                if any(r.get(k) and c.get(k) and (
                        abs(c[k]["h"] - r[k]["h"]) > 1
                        or c[k]["lignes"] != r[k]["lignes"])
                       for k in ("desc", "ex", "h1")):
                    n += 1
            bouge = f"{n}/{len(reference)}"
        print(f"{nom:<14}{moy(rs, 'desc'):>10.1f}{moy(rs, 'ex'):>9.1f}"
              f"{moy(rs, 'desc', 'lignes'):>13.2f}{bouge:>20}")

    print("\n=== detail des cartes qui bougent ===")
    for nom, _ in configs[1:]:
        lignes = []
        for r in reference:
            c = idx.get((nom, r["pk"]))
            if not c:
                continue
            ecarts = []
            for k in ("desc", "ex", "h1"):
                if r.get(k) and c.get(k):
                    dh = round(c[k]["h"] - r[k]["h"], 1)
                    dl = c[k]["lignes"] - r[k]["lignes"]
                    if abs(dh) > 1 or dl:
                        ecarts.append(f"{k}: {dh:+.1f}px ({dl:+d} ligne)")
            if ecarts:
                lignes.append(f"     PK={r['pk']:<6} " + " | ".join(ecarts))
        print(f"  {nom}: {len(lignes)}/{len(reference)}")
        for l in lignes[:15]:
            print(l)


# ---------------------------------------------------------------------- couverture

def mode_couverture(args):
    """Quels caracteres du corpus peint chaque famille couvre-t-elle ?"""
    from fontTools.ttLib import TTFont

    payload = json.loads(Path(args.gabarit).read_text(encoding="utf-8-sig"))
    mustache, csv_texte = payload.get("mustache", ""), payload.get("csv", "")
    rdr = list(_csv.reader(io.StringIO(csv_texte)))
    entete, corps = rdr[0], rdr[1:]

    rendues = [h for h in entete
               if re.search(r"\{\{[^}]*" + re.escape(h.strip()) + r"[^}]*\}\}", mustache)]
    caracteres = set()
    for r in corps:
        for i, h in enumerate(entete):
            if h in rendues and i < len(r):
                caracteres.update(r[i])
    caracteres = {c for c in caracteres if c not in "\r\n\t"}
    print(f"colonnes rendues ({len(rendues)}/{len(entete)}) : {rendues}")
    print(f"corpus peint : {len(caracteres)} caracteres distincts\n")

    for famille in args.familles:
        url = ("https://fonts.googleapis.com/css2?family="
               f"{urllib.parse.quote(famille)}:wght@300;500;700;900&display=swap")
        css = telecharger(url).decode("utf-8")
        cmap, n = set(), 0
        for u in set(re.findall(r"url\((https://[^)]+\.woff2)\)", css)):
            try:
                f = TTFont(io.BytesIO(telecharger(u)))
            except Exception:
                continue
            n += 1
            for tb in f["cmap"].tables:
                cmap.update(tb.cmap.keys())
        manquants = sorted(c for c in caracteres if ord(c) not in cmap)
        etat = "".join(manquants) if manquants else "(aucun)"
        print(f"{famille:<14} {n:>2} sous-ensembles, {len(cmap):>5} points de code | "
              f"manquants : {len(manquants)} {etat}")


def main():
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    sous = ap.add_subparsers(dest="mode", required=True)

    c = sous.add_parser("css", help="construire un bloc @font-face de substitution")
    c.add_argument("--famille", required=True, help="nom Google Fonts, ex: Barlow")
    c.add_argument("--nom", required=True, help="@font-face de sortie, ex: DINpro")
    c.add_argument("--poids", default=",".join(str(p) for p in POIDS_GABARIT))
    c.add_argument("--sortie", required=True)
    c.set_defaults(fonction=lambda a: mode_css(
        argparse.Namespace(**{**vars(a), "poids": [int(x) for x in a.poids.split(",")]})))

    r = sous.add_parser("rendre", help="rendre une face reelle")
    r.add_argument("--gabarit", required=True)
    r.add_argument("--css-police", default=None)
    r.add_argument("--pk", default=None)
    r.add_argument("--ligne", type=int, default=None, help="index 1-based (CSV sans PK)")
    r.add_argument("--n", type=int, default=1)
    r.add_argument("--sortie", required=True)
    r.add_argument("--tag", required=True)
    r.add_argument("--sonde", action="store_true")
    r.set_defaults(fonction=mode_rendre)

    m = sous.add_parser("mesurer", help="chiffrer l'effet sur un echantillon")
    m.add_argument("--gabarit", required=True)
    m.add_argument("--pks", required=True)
    m.add_argument("--config", action="append", required=True, metavar="NOM=chemin.css")
    m.add_argument("--sortie", required=True)
    m.set_defaults(fonction=mode_mesurer)

    o = sous.add_parser("couverture", help="couverture des glyphes du corpus peint")
    o.add_argument("--gabarit", required=True)
    o.add_argument("--familles", nargs="+", required=True)
    o.set_defaults(fonction=mode_couverture)

    args = ap.parse_args()
    args.fonction(args)


if __name__ == "__main__":
    main()