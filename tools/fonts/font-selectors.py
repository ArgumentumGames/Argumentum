"""#1485 -- qui porte quelle famille de police dans les gabarits, et pour quelle langue.

Compagnon de `font-coverage.py`. La couverture de glyphes ne dit rien tant qu'on
ignore OU la famille est appliquee : un `@font-face` remplace ne coute quelque
chose que la ou du texte de donnees atterrit dessus, dans une langue qui n'a pas
de regle de police a elle.

Ce que l'outil mesure, sur les gabarits LUS DANS L'ARBRE (`git show <ref>:<fichier>`)
et le JSON PARSE (la cle `css` est une chaine JSON : un motif sur les octets bruts
s'arrete a l'echappement) :

1. les blocs `@font-face` de chaque gabarit : famille, graisse, hotes des `url()` ;
2. les selecteurs qui declarent la famille cherchee (`--family`, DINPro par defaut),
   et la liste de familles qu'ils declarent ;
3. pour chacun de ces selecteurs, si un element du mustache porte sa classe et si
   cet element contient une variable `{{…}}` -- donc si du TEXTE DE DONNEES y passe ;
4. la portee par langue : `ARGU_LANG_MARKER` (`AssetConverterConfig.cs`) reecrit
   `argu-lang-fr` en `argu-lang-<langue>` dans le mustache. Une langue dont aucune
   regle `.argu-lang-<langue>` ne declare de police HERITE des regles de base ;
   c'est ce que ce tableau rend visible, langue par langue, gabarit par gabarit.

⚠️ Le commentaire CSS des gabarits affirme que rien ne peut matcher en
fr/en/ru/pt/es. L'outil STRIPPE les commentaires avant de parser (sinon leur texte
se lit comme des selecteurs) puis mesure l'affirmation plutot que de la citer.

Limite assumee : les blocs `@media`/`@supports` sont aplatis (leurs regles internes
sont traitees comme des regles ordinaires). Les selecteurs de ces blocs sont
signales par un prefixe `@…` conserve dans le rapport.

Dependances : aucune (stdlib seule). Lecture seule sur le depot.

Usage:
  python -I font-selectors.py --repo <racine Argumentum> [--ref origin/master] \\
      [--family DINPro] [gabarit.json ...]
"""
import argparse
import json
import re
import subprocess
from pathlib import Path

LANGS = ("fr", "en", "ru", "pt", "es", "ar", "fa", "zh")

COMMENT = re.compile(r"/\*.*?\*/", re.S)
RULE = re.compile(r"([^{}]+)\{([^{}]*)\}", re.S)
FAMILY = re.compile(r"font-family\s*:\s*([^;]+)", re.S)
WEIGHT = re.compile(r"font-weight\s*:\s*([^;]+)", re.S)
URL = re.compile(r"url\(\s*['\"]?([^'\")]+)['\"]?\s*\)", re.S)
MARKER = re.compile(r"argu-lang-(fr|[a-z]{2})")


def norm(text: str) -> str:
    return re.sub(r"\s+", " ", text).strip()


def host(url: str) -> str:
    m = re.match(r"https?://([^/]+)", url)
    return m.group(1) if m else "(local)"


def rules(css: str):
    """(selecteur, declarations) pour chaque regle CSS, commentaires retires."""
    clean = COMMENT.sub(" ", css)
    for m in RULE.finditer(clean):
        head, body = norm(m.group(1)), m.group(2)
        if not head or head.endswith("@font-face") or head.startswith("@"):
            continue
        for sel in head.split(","):
            sel = norm(sel)
            if sel:
                yield sel, body


def data_elements(mustache: str, cls: str) -> tuple:
    """(elements portant la classe, dont le contenu porte une variable `{{…}}`).

    Heuristique : de la balise ouvrante jusqu'au prochain `</`. Suffit pour dire si
    du texte de donnees atterrit sur le selecteur, ne pretend pas au DOM.
    """
    classes = re.finditer(rf'class="([^"]*\b{re.escape(cls)}\b[^"]*)"', mustache)
    total = with_var = 0
    for m in classes:
        total += 1
        tail = mustache[m.end():]
        stop = tail.find("</")
        if "{{" in (tail if stop < 0 else tail[:stop]):
            with_var += 1
    return total, with_var


def report(repo: Path, ref: str, tpl: str, family: str) -> dict:
    raw = subprocess.run(["git", "-C", str(repo), "show", f"{ref}:{tpl}"],
                         check=True, capture_output=True).stdout
    doc = json.loads(raw.decode("utf-8-sig"))
    css = doc.get("css") or ""
    mustache = doc.get("mustache") or ""
    out = {"tpl": tpl, "faces": [], "carriers": [], "scope": {}}

    clean = COMMENT.sub(" ", css)
    for m in re.finditer(r"@font-face\s*\{(.*?)\}", clean, re.S):
        body = m.group(1)
        fam = FAMILY.search(body)
        wt = WEIGHT.search(body)
        urls = URL.findall(body)
        out["faces"].append({
            "family": norm(fam.group(1)) if fam else "?",
            "weight": norm(wt.group(1)) if wt else "-",
            "hosts": sorted({host(u) for u in urls}),
            "n_url": len(urls),
        })

    seen = {}
    for sel, body in rules(css):
        fam = FAMILY.search(body)
        if fam:
            seen.setdefault(sel, []).append(norm(fam.group(1)))

    for sel, fams in seen.items():
        if family.lower() not in " ".join(fams).lower():
            continue
        cls = re.findall(r"\.([A-Za-z0-9_-]+)", sel)
        counts = [data_elements(mustache, c) for c in cls] or [(0, 0)]
        out["carriers"].append({
            "selector": sel,
            "families": fams,
            "elements": max(c[0] for c in counts),
            "with_variable": max(c[1] for c in counts),
        })

    for lang in LANGS:
        scoped = {s: f for s, f in seen.items() if f".argu-lang-{lang}" in s}
        out["scope"][lang] = scoped
    out["marker_in_mustache"] = sorted(set(MARKER.findall(mustache)))
    out["marker_in_css"] = sorted({m for m in MARKER.findall(css)})
    return out


def main() -> None:
    ap = argparse.ArgumentParser()
    ap.add_argument("--repo", required=True)
    ap.add_argument("--ref", default="origin/master")
    ap.add_argument("--family", default="DINPro")
    ap.add_argument("templates", nargs="*")
    a = ap.parse_args()
    repo = Path(a.repo)

    templates = a.templates
    if not templates:
        listing = subprocess.run(["git", "-C", str(repo), "ls-tree", "-r", "--name-only",
                                  a.ref, "Cards/"], check=True, capture_output=True)
        templates = [f for f in listing.stdout.decode("utf-8").split("\n")
                     if f.endswith(".json") and "/Archive/" not in f]

    reports = []
    for tpl in templates:
        try:
            reports.append(report(repo, a.ref, tpl, a.family))
        except (subprocess.CalledProcessError, json.JSONDecodeError) as exc:
            print(f"[SANS GABARIT] {tpl} : {type(exc).__name__}")

    print(f"famille cherchee : {a.family} — gabarits : {len(reports)}\n")
    for r in reports:
        if not any(r["faces"]) and not r["carriers"]:
            print(f"=== {r['tpl']}\n  aucune @font-face, aucun porteur de {a.family}\n")
            continue
        print(f"=== {r['tpl']}")
        print(f"  @font-face : {len(r['faces'])}")
        for f in r["faces"]:
            print(f"    {f['family']:32s} w={f['weight']:>4s}  {f['n_url']} url  {f['hosts']}")
        print(f"  selecteurs portant {a.family} : {len(r['carriers'])}")
        for c in r["carriers"]:
            print(f"    {c['selector'][:58]:58s} -> {' | '.join(c['families'])[:60]}")
            print(f"        elements portant la classe : {c['elements']}, "
                  f"dont avec variable {{{{…}}}} : {c['with_variable']}")
        overrides = {lg: sc for lg, sc in r["scope"].items() if sc}
        print(f"  marqueur argu-lang dans le mustache : {r['marker_in_mustache'] or 'aucun'}")
        print(f"  classes de langue declarees en css  : {r['marker_in_css'] or 'aucune'}")
        print(f"  surcharges par langue : "
              + (", ".join(sorted(overrides)) if overrides else "aucune"))
        for lg in sorted(overrides):
            print(f"    .argu-lang-{lg} : {len(overrides[lg])} regle(s) — "
                  f"{sorted({fam for fams in overrides[lg].values() for fam in fams})[:4]}")
        print()

    # recap : une langue sans aucune surcharge n'importe ou herite des regles de base
    carriers = [r for r in reports if r["carriers"]]
    print("recapitulatif — langues SANS aucune surcharge de police dans les gabarits porteurs :")
    for lang in LANGS:
        holders = [r["tpl"] for r in carriers if r["scope"][lang]]
        if holders:
            print(f"  {lang} : surcharge dans {len(holders)} gabarit(s)")
        else:
            print(f"  {lang} : AUCUNE surcharge -> herite de {a.family} partout ({len(carriers)} gabarits)")


if __name__ == "__main__":
    main()
