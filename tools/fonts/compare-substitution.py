"""#1485 — comparatif de substitution de polices sur faces REELLES.

Compose la carte depuis le gabarit commite (mustache + css) avec une vraie
ligne du CSV et rend chaque variante dans Chromium headless. Deux instruments :

1. METRIQUES DOM — hauteurs de blocs texte (repli/wrapping), compteur de
   rognage, familles declarees. Le pixel-diff pleine page n'est qu'un
   indicateur faible : la face Fallacies place son texte en absolu et Chromium
   ne charge paresseusement une fonte qu'a la peinture -> pas de repeint
   post-swap dans ce cas (limite documentee, ne pas s'y fier).
2. RASTER GLYPHE — texte reel de la carte trace sur canvas apres
   document.fonts.load() FORCE (aucun race possible), meme echelle dans
   toutes les variantes -> diff pixel + delta de largeur = distance de
   substitution.

Chaque famille embarque un TEMOIN NEGATIF (Lobster, script display) : si le
candidat et le temoin negatif rendent des ecarts similaires, l'instrument est
casse, pas la police.

Les substitutions s'injectent AU RENDU uniquement : le nom de famille est
conserve, seul le src du @font-face change — simulation fidele de la
migration future (post-tag). Cards/ reste en LECTURE SEULE.

Substituts :
  DINPro 300/500/700/900 (cdnfonts, sans licence claire) -> Barlow (OFL)
  TrendSlabW00-Four 400 (onlinewebfonts, alias de licence douteuse) -> Zilla Slab (OFL)

Dependances : playwright (python), PIL. Reseau requis (Google Fonts css2,
cdnfonts, onlinewebfonts).

Usage:
  python compare-substitution.py --repo <racine Argumentum> [--outdir DIR]
"""
import io
import json
import re
import sys
import csv
import urllib.request
from pathlib import Path

# ---------------------------------------------------------------- repo/outdir
def _find_repo() -> Path:
    if "--repo" in sys.argv:
        return Path(sys.argv[sys.argv.index("--repo") + 1]).resolve()
    for base in (Path(__file__).resolve().parent, Path.cwd()):
        _p = base
        while _p != _p.parent and not (_p / "Cards").is_dir():
            _p = _p.parent
        if (_p / "Cards").is_dir():
            return _p
    raise SystemExit("racine du repo introuvable (Cards/ absent) — passer --repo <path>")

REPO = _find_repo()
OUT = Path(sys.argv[sys.argv.index("--outdir") + 1]) if "--outdir" in sys.argv else Path("font-compare-out")
OUT.mkdir(exist_ok=True)

FALLACY_CSV = REPO / "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
RULES_CSV = REPO / "Cards/Rules/Argumentum Rules - Cards.csv"
GABARITS = {
    "fallacies": REPO / "Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
    "rules_cover": REPO / "Cards/Rules/Argumentum_Rules_fr.json",
    "rules_body": REPO / "Cards/Rules/Argumentum_Rules_fr.json",
}

# ---------------------------------------------------------------- mini-mustache
def _em(s: str) -> str:
    return re.sub(r"\*([^*]+)\*", r"<em>\1</em>", s)

def _md(text: str) -> str:
    out, in_ul = [], False

    def close_ul():
        nonlocal in_ul
        if in_ul:
            out.append("</ul>")
            in_ul = False

    for ln in text.split("\n"):
        s = ln.strip()
        if s.startswith("### "):
            close_ul(); out.append(f"<h3>{_em(s[4:])}</h3>")
        elif s.startswith("## "):
            close_ul(); out.append(f"<h2>{_em(s[3:])}</h2>")
        elif s.startswith("# "):
            close_ul(); out.append(f"<h1>{_em(s[2:])}</h1>")
        elif s.startswith("* ") or s.startswith("- "):
            if not in_ul:
                out.append("<ul>"); in_ul = True
            out.append(f"<li>{_em(s[2:])}</li>")
        elif s == "":
            close_ul()
        else:
            close_ul(); out.append(f"<p>{_em(s)}</p>")
    close_ul()
    return "".join(out)

def render_mustache(html: str, ctx: dict) -> str:
    html = re.sub(
        r"\{\{#if ([^}]+)\}\}(.*?)\{\{/if\}\}",
        lambda m: m.group(2) if (ctx.get(m.group(1).strip()) or "").strip() else "",
        html, flags=re.S)

    def repl(m):
        inner = m.group(1).strip()
        if inner.startswith("breaklines "):
            return (ctx.get(inner[11:].strip()) or "").replace("\n", "<br>")
        if inner.startswith("markdown "):
            return _md(ctx.get(inner[9:].strip()) or "")
        return ctx.get(inner) or ""

    return re.sub(r"\{\{([^#/][^}]*)\}\}", repl, html)

# ---------------------------------------------------------------- fixtures
def fallacy_row() -> dict:
    """Ligne profonde : Famille + Sous-Famille + Soussousfamille + exemple non vides."""
    with io.open(FALLACY_CSV, encoding="utf-8-sig") as f:
        for row in csv.DictReader(f):
            if all((row.get(k) or "").strip() for k in
                   ("Famille", "Sous-Famille", "Soussousfamille", "desc_fr", "example_fr", "text_fr")):
                return row
    raise SystemExit("aucune ligne Fallacies complete")

def rules_row(pk: str) -> dict:
    with io.open(RULES_CSV, encoding="utf-8-sig") as f:
        for row in csv.DictReader(f):
            if row.get("pk") == pk:
                return row
    raise SystemExit(f"{pk} introuvable dans Rules CSV")

def fixture(kind: str):
    """-> (card_class, ctx)"""
    if kind == "fallacies":
        row = fallacy_row()
        ctx = {k: (row.get(k) or "") for k in
               ("Famille", "Sous-Famille", "Soussousfamille", "text_fr", "desc_fr",
                "example_fr", "path", "depth_max4")}
        return "1", ctx
    if kind == "rules_cover":
        row = rules_row("Rules_01")
        return "1", {"Text": row.get("Text") or "", "cardIndex": "1"}
    if kind == "rules_body":
        row = rules_row("Rules_02")
        return "2", {"Text": row.get("Text") or "", "cardIndex": "2"}
    raise ValueError(kind)

# ---------------------------------------------------------------- substitutions
def css2(family_query: str) -> str:
    url = f"https://fonts.googleapis.com/css2?{family_query}&display=swap"
    req = urllib.request.Request(url, headers={
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
                      "(KHTML, like Gecko) Chrome/126.0 Safari/537.36"})
    return urllib.request.urlopen(req, timeout=30).read().decode("utf-8")

def rename_face(css_text: str, new_family: str) -> str:
    return re.sub(r"(font-family:\s*)(['\"])[^'\"]+\2", rf"\g<1>'{new_family}'", css_text)

SUBSTITUTIONS = {
    "DINPro": [
        ("temoin-cdnfonts", None),
        ("Barlow-OFL", rename_face(css2("family=Barlow:wght@300;500;700;900"), "DINPro")),
        # temoin negatif d'instrument : une script display ne peut pas rendre comme DINPro
        ("Lobster-NEGATIF", rename_face(css2("family=Lobster"), "DINPro")),
    ],
    "TrendSlabW00-Four": [
        ("temoin-onlinewebfonts", None),
        ("ZillaSlab-OFL", rename_face(css2("family=Zilla+Slab:wght@400"), "TrendSlabW00-Four")),
        ("Lobster-NEGATIF", rename_face(css2("family=Lobster"), "TrendSlabW00-Four")),
    ],
}

# cas de rendu : (nom, gabarit, famille substituee, fixture-kind)
CASES = [
    ("fallacies__face_fr", "fallacies", "DINPro"),
    ("rules__cover_h1", "rules_cover", "TrendSlabW00-Four"),
    ("rules__body_h2h3", "rules_body", "DINPro"),
]

def build_doc(kind: str, family: str, sub_css):
    d = json.load(io.open(GABARITS[kind], encoding="utf-8-sig"))
    css = re.sub(r"/\*.*?\*/", "", d["css"], flags=re.S)
    if sub_css is not None:
        css = re.sub(r"@font-face\s*\{[^}]*font-family:\s*['\"]?" + re.escape(family) + r"[^}]*\}", "", css)
        css = sub_css + "\n" + css
    # le gabarit fixe une hauteur de carte avec overflow hidden (rognage du texte
    # hors de CardPen) ; pour COMPARER les glyphes on laisse le contenu couler,
    # a l'identique dans toutes les variantes — la question "tient-il dans la
    # carte" reste portee par les metriques clipped/hauteurs de blocs
    css += ("\ncard, card * { overflow: visible !important; }\n"
            "card { height: auto !important; min-height: 0 !important; max-height: none !important; }\n")
    card_class, ctx = fixture(kind)
    body = render_mustache(d["mustache"], ctx)
    return (f"<!doctype html><html><head><meta charset='utf-8'>"
            f"<style>{css}</style></head><body>"
            f"<card class='{card_class}'>{body}</card></body></html>")

# ---------------------------------------------------------------- raster canvas
# Instrument glyphe : le rendu DOM de la face Fallacies ne repeint pas apres le
# swap (texte en position absolue hors zone de peinture -> chargement paresseux
# jamais declenche). Le canvas force le chargement (document.fonts.load) puis
# trace de facon synchrone : aucun race. Tailles = taille DOM du gabarit x4
# (echelle identique dans toutes les variantes).
RASTER_JS = """async (samples) => {
    for (const s of samples)
        await document.fonts.load(s.weight + ' ' + s.size + 'px "' + s.family + '"');
    const W = 2600;
    let y = 0; const rows = [];
    for (const s of samples) rows.push(y = y + Math.ceil(s.size * 1.6) + 24);
    const c = document.createElement('canvas');
    c.width = W; c.height = y + 24;
    const g = c.getContext('2d');
    g.fillStyle = '#fff'; g.fillRect(0, 0, c.width, c.height);
    g.fillStyle = '#000'; g.textBaseline = 'alphabetic';
    const widths = [];
    samples.forEach((s, i) => {
        g.font = s.weight + ' ' + s.size + 'px "' + s.family + '"';
        const t = s.text.slice(0, 46);
        g.fillText(t, 16, rows[i] - Math.ceil(s.size * 0.6));
        widths.push(Math.round(g.measureText(t).width * 10) / 10);
    });
    document.body.innerHTML = '';
    document.body.style.margin = '0';
    document.body.appendChild(c);
    return {widths, w: c.width, h: c.height};
}"""

def raster_samples(kind: str, ctx: dict):
    """Textes REELS de la carte, taille x4, graisses telles que posees par le CSS."""
    x4 = lambda v: round(v * 4, 2)
    if kind == "fallacies":
        return [
            {"family": "DINPro", "weight": "900", "size": x4(11.52), "text": ctx["desc_fr"]},
            {"family": "DINPro", "weight": "400", "size": x4(11.52), "text": ctx["example_fr"]},
        ]
    if kind == "rules_cover":
        # le sous-titre h2 rend en Bebas Neue (locale), hors perimetre TrendSlab
        return [{"family": "TrendSlabW00-Four", "weight": "500", "size": x4(48), "text": "Argumentum"}]
    if kind == "rules_body":
        body = re.sub(r"[#*]", "", ctx["Text"]).split("\n")
        body = [l.strip() for l in body if len(l.strip()) > 30][:1]
        return [
            {"family": "DINPro", "weight": "700", "size": x4(13.6), "text": "Matériel"},
            {"family": "DINPro", "weight": "400", "size": x4(8), "text": body[0] if body else "Corps de règle"},
        ]
    raise ValueError(kind)

# ---------------------------------------------------------------- rendu + mesure
from playwright.sync_api import sync_playwright  # noqa: E402

MEASURE_JS = """() => {
    const bad = new Set(['STYLE','SCRIPT','META','LINK','TITLE','HEAD']);
    const vis = [...document.querySelectorAll('body *')].filter(e =>
        !bad.has(e.tagName) && e.children.length === 0 &&
        e.textContent.trim().length > 0 &&
        e.getBoundingClientRect().width > 5 && e.getBoundingClientRect().height > 3);
    const byFont = {};
    for (const e of vis) {
        const cs = getComputedStyle(e);
        const b = e.getBoundingClientRect();
        const key = cs.fontFamily.split(',')[0] + ' w' + cs.fontWeight + ' ' + cs.fontSize;
        byFont[key] = (byFont[key] || 0) + Math.round(b.width * b.height);
    }
    const clipped = vis.filter(e =>
        e.scrollWidth > e.clientWidth + 1 || e.scrollHeight > e.clientHeight + 1).length;
    const blocks = vis
        .sort((a, b) => b.getBoundingClientRect().width * b.getBoundingClientRect().height -
                       a.getBoundingClientRect().width * a.getBoundingClientRect().height)
        .slice(0, 4)
        .map(r => { const b = r.getBoundingClientRect(); const cs = getComputedStyle(r);
            return {txt: r.textContent.trim().slice(0, 22),
                    w: Math.round(b.width * 10) / 10, h: Math.round(b.height * 10) / 10,
                    font: cs.fontFamily.split(',')[0], size: cs.fontSize, weight: cs.fontWeight}; });
    return {nText: vis.length, clipped, byFont, blocks,
            fonts: [...document.fonts].filter(f => f.status === 'loaded')
                   .map(f => f.family + ':' + f.weight),
            probe: (() => {
                const c = document.createElement('canvas').getContext('2d');
                const out = {};
                const fams = [...new Set(vis.map(e => getComputedStyle(e).fontFamily))];
                for (const fam of fams.slice(0, 3)) {
                    const sample = vis.find(e => getComputedStyle(e).fontFamily === fam);
                    const cs = getComputedStyle(sample);
                    c.font = cs.fontWeight + ' ' + cs.fontSize + ' ' + fam;
                    const wWeb = c.measureText(sample.textContent.trim().slice(0, 20)).width;
                    c.font = cs.fontWeight + ' ' + cs.fontSize + ' serif';
                    const wRef = c.measureText(sample.textContent.trim().slice(0, 20)).width;
                    out[fam + ' ' + cs.fontWeight + '/' + cs.fontSize] = {
                        wWeb: Math.round(wWeb * 10) / 10, wSerifRef: Math.round(wRef * 10) / 10 };
                }
                return out;
            })()};
}"""

def run():
    results = {}
    with sync_playwright() as p:
        browser = p.chromium.launch()
        page = browser.new_page(viewport={"width": 1000, "height": 1450}, device_scale_factor=2)
        for case_name, kind, family in CASES:
            for label, sub_css in SUBSTITUTIONS[family]:
                doc = build_doc(kind, family, sub_css)
                page.set_content(doc, wait_until="networkidle")
                # le fetch des webfonts ne bloque pas networkidle : attendre le
                # swap reel (status passe a 'loaded' une fois les fontes utilisees
                # chargees), sinon la capture montre le fallback d'avant-swap
                try:
                    page.wait_for_function("() => document.fonts.status === 'loaded'", timeout=20000)
                except Exception:
                    pass  # fontes mortes -> fallback assume, temoin negatif le detectera
                page.wait_for_timeout(600)
                png = OUT / f"{case_name}__{label}.png"
                # capture PLEINE PAGE : le clip sur la boite card exclurait le
                # texte qui deborde (la carte gabarit est en hauteur fixe)
                page.screenshot(path=str(png), full_page=True)
                m = page.evaluate(MEASURE_JS)
                results[(case_name, label)] = {"png": str(png), **m}
                print(f"[{case_name}] {label}: textLeaves={m['nText']} clipped={m['clipped']}")
                print(f"   fonts loaded: {m['fonts']}")
                print(f"   byFont(px2): {m['byFont']}")
                for b in m["blocks"]:
                    print(f"   block: {b}")
                # raster glyphe (chargement force, tracé synchrone)
                _, fctx = fixture(kind)
                samples = raster_samples(kind, fctx)
                rinfo = page.evaluate(RASTER_JS, samples)
                rpng = OUT / f"{case_name}__{label}__raster.png"
                cv = page.query_selector("canvas")
                if cv:
                    cv.screenshot(path=str(rpng))
                results[(case_name, label)]["raster"] = str(rpng)
                results[(case_name, label)]["raster_widths"] = rinfo["widths"]
                print(f"   raster: {rinfo['w']}x{rinfo['h']} widths={rinfo['widths']}")
        browser.close()
    return results

# ---------------------------------------------------------------- diff
from PIL import Image, ImageChops  # noqa: E402

def diff(a: str, b: str):
    ia, ib = Image.open(a).convert("RGB"), Image.open(b).convert("RGB")
    if ia.size != ib.size:
        return {"note": f"tailles differentes {ia.size} vs {ib.size}"}
    d = ImageChops.difference(ia, ib)
    hist = d.convert("L").histogram()
    changed = sum(hist[8:])
    total = ia.size[0] * ia.size[1]
    return {"bbox": d.getbbox(), "pct": round(100 * changed / total, 2)}

def main():
    results = run()
    print("\n===== DIFFS (variant vs temoin) =====")
    for case_name, kind, family in CASES:
        base = results[(case_name, SUBSTITUTIONS[family][0][0])]
        for label, _ in SUBSTITUTIONS[family][1:]:
            sub = results[(case_name, label)]
            dd = diff(base["png"], sub["png"])
            print(f"\n[{case_name}] {family}: {label} vs temoin")
            print(f"  DOM pixel diff (pleine page): {dd}")
            rd = diff(base["raster"], sub["raster"])
            dw = [round(b2 - b1, 1) for b1, b2 in zip(base["raster_widths"], sub["raster_widths"])]
            print(f"  RASTER glyphe diff: {rd} | delta largeur texte (px @x4): {dw}")
            for i, (b1, b2) in enumerate(zip(base["blocks"], sub["blocks"])):
                dwh = round(b2["h"] - b1["h"], 1)
                if dwh:
                    print(f"  block{i} «{b1['txt']}» h {b1['h']}->{b2['h']} ({dwh:+})")
            print(f"  clipped: temoin {base['clipped']} -> {sub['clipped']}")

if __name__ == "__main__":
    main()
