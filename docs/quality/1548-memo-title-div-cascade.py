# -*- coding: utf-8 -*-
"""#1548 — instrument de cascade CSS pour la face Mémo (ré-affirmation #1225).

Mesure SANS pipeline (JSON parsé, DOM squelette, getComputedStyle) quelle règle
de font-family gagne la cascade sur les lettres « M E M O » (`.title > div`)
sous ar/fa/zh, après la réécriture de marqueur de langue du pipeline
(`argu-lang-fr` -> `argu-lang-<lang>`, AssetConverterConfig.cs ARGU_LANG_MARKER).

Pourquoi un navigateur et pas un parseur CSS écrit main : la question posée est
précisément « quelle règle gagne » — spécificité + !important + ordre des règles
sont exactement ce que getComputedStyle résout et qu'un re-implémenteur se
trompe. Le réseau est bloqué : la cascade ne dépend d'aucune ressource externe
(le @import reset peut échouer sans effet sur font-family) et le verdict VISUEL
(les glyphes) reste à ai-01 dans la fenêtre de régénération.

Modes :
  (défaut)      mesure le gabarit Memo Face : 4 lettres x {ar,fa,zh} -> Bebas
                attendu ; contrôle inverse .familyName/.familyDesc -> fonte de
                langue attendue (Vazirmatn / Noto Sans SC).
  --scan        balaye tous les gabarits portant une ré-affirmation #1225 et
                mesure, pour chaque langue de la règle, un descendant porteur
                de texte de l'élément ré-affirmé. Rapporte le compte.
  --self-test   mutation falsifiante : la règle #1225 réduite à sa forme
                d'avant #1548 (sans `> div`) doit faire ÉCHOUER la mesure des
                lettres ; rc attendu [0, 2].

Exit code : 0 = PASS, 2 = FAIL (contrat des organes du corpus).
"""
import argparse
import json
import re
import sys
from pathlib import Path

REPO = Path(__file__).resolve().parents[2]
MARKER = "argu-lang-fr"
LANGS = ("ar", "fa", "zh")  # les écritures non latines servies
LANG_FONT = {
    "ar": "Vazirmatn",
    "fa": "Vazirmatn",
    "zh": "Noto Sans SC",
}
BEBAS = "Bebas Neue"

# Le squelette reproduit le mustache réel (bloc .title, classes de famille
# factices issues du css du gabarit) + les deux témoins du contrôle inverse.
SKELETON = """
<div class="cardContainer {marker}" lang="fr">
  <div class="titleContainer" dir="auto">
    <div class="title" dir="auto">
      <div class="insuffisance">M</div>
      <div class="insuffisance">E</div>
      <div class="insuffisance">M</div>
      <div class="insuffisance">O</div>
    </div>
  </div>
  <div class="familyContainer">
    <div class="familyName">TEMOIN NOM</div>
    <div class="familyDesc">Temoir description</div>
  </div>
</div>
"""

PAGE = """<!doctype html><html><head><meta charset="utf-8">
<style>{css}</style></head><body>{body}</body></html>"""

RULE_1225 = re.compile(
    r"/\*\s*#1225[^*]*\*/\s*([^{}]+)\{([^{}]*)\}", re.S)


def load_template(path):
    d = json.loads(Path(path).read_text(encoding="utf-8-sig"))
    return d.get("css", "")


def rewrite_marker(text, lang):
    return text.replace(MARKER, f"argu-lang-{lang}")


def measure(page, css, body):
    page.set_content(PAGE.format(css=css, body=body))
    return page.evaluate(
        """() => {
            const pick = sel => Array.from(document.querySelectorAll(sel)).map(
                el => getComputedStyle(el).fontFamily);
            return {
                letters: pick('.title > div'),
                familyName: pick('.familyName'),
                familyDesc: pick('.familyDesc'),
                scanChild: pick('.scanTextChild'),
            };
        }""")


def first_family(value):
    return re.match(r"\s*['\"]?([^'\",]+)", value).group(1)


def check_memo(page, css, verbose=True):
    """Mesure le gabarit Memo Face. Retourne (ok, detail).

    Le squelette doit porter le marqueur : sans la classe de langue sur le
    conteneur, les lettres rendent Bebas par simple héritage de la règle de
    base `.title` — un vert de coïncidence, pas une preuve du fix (#1046).
    """
    ok = True
    lines = []
    skeleton = SKELETON.format(marker=MARKER)
    for lang in LANGS:
        res = measure(page, rewrite_marker(css, lang),
                      rewrite_marker(skeleton, lang))
        for i, fam in enumerate(res["letters"]):
            good = first_family(fam) == BEBAS
            ok &= good
            if verbose or not good:
                lines.append(f"[{lang}] lettre {i}: {fam!r}"
                             f" {'OK' if good else 'FAIL (attendu ' + BEBAS + ')'}")
        for name, values in (("familyName", res["familyName"]),
                             ("familyDesc", res["familyDesc"])):
            good = first_family(values[0]) == LANG_FONT[lang]
            ok &= good
            if verbose or not good:
                lines.append(f"[{lang}] .{name}: {values[0]!r}"
                             f" {'OK' if good else 'FAIL (attendu ' + LANG_FONT[lang] + ')'}")
    return ok, lines


def scan(page, verbose=True):
    """Balaye les gabarits portant une ré-affirmation #1225.

    Pour chaque règle #1225 d'un gabarit, SI son mustache emballe réellement le
    texte dans une balise enfant de l'élément ré-affirmé (le motif #1548 —
    p.ex. les lettres M E M O dans `.title > div`), instancie cet élément avec
    un descendant porteur de texte et mesure si la déclaration protégée descend
    à l'enfant, ou si la règle universelle `.argu-lang-XX *` le frappe
    directement. Les gabarits dont le texte est DIRECT dans l'élément ré-affirmé
    (p.ex. Memo_Back `ARGUMENTUM`) sont NON APPLICABLES par construction : la
    ré-affirmation couvre déjà leur texte.
    """
    total = measured = protected = broken = 0
    report = []
    for path in sorted(REPO.glob("Cards/**/*.json")):
        if "Archive" in path.parts:
            continue
        try:
            css = load_template(path)
            must = json.loads(path.read_text(encoding="utf-8-sig")).get("mustache", "")
        except Exception:
            continue
        if "#1225" not in css:
            continue
        total += 1
        status = {}
        for m in RULE_1225.finditer(css):
            selectors = [s.strip() for s in m.group(1).split(",")]
            decl = m.group(2)
            protected_font = re.search(
                r"font-family:\s*'([^']+)'", decl)
            if not protected_font:
                continue
            font = protected_font.group(1)
            for sel in selectors:
                lang_m = re.search(r"argu-lang-([a-z]+)", sel)
                cls_m = re.search(r"\.([A-Za-z_][\w-]*)\s*$", sel)
                if not lang_m or not cls_m:
                    continue
                lang, cls = lang_m.group(1), cls_m.group(1)
                # le mustache réel emballe-t-il le texte dans un enfant de cls ?
                wrapped = re.search(
                    r'class="[^"]*\b' + re.escape(cls) + r'\b[^"]*"[^>]*>\s*<[a-z][^>]*>\s*[^<\s]',
                    must)
                if not wrapped:
                    status.setdefault(lang, f"{cls}: texte direct (non applicable)")
                    continue
                measured += 1
                body = (f'<div class="argu-lang-{lang}">'
                        f'<div class="{cls}">'
                        f'<div class="scanTextChild">A</div></div></div>')
                res = measure(page, css, body)
                if not res["scanChild"]:
                    status.setdefault(lang, f"selecteur {cls}: enfant introuvable")
                    continue
                got = first_family(res["scanChild"][0])
                if got == font:
                    status.setdefault(lang, f"{cls}: DESCEND ({font})")
                else:
                    status.setdefault(lang, f"{cls}: CASSE ({got} != {font})")
        if any("CASSE" in v for v in status.values()):
            broken += 1
        elif any("DESCEND" in v for v in status.values()):
            protected += 1
        tag = ("CASSE   " if any("CASSE" in v for v in status.values())
               else "PROTEGE" if any("DESCEND" in v for v in status.values())
               else "N/A    ")
        report.append(f"{tag} {path.relative_to(REPO)}"
                      + (f"  {sorted(status.items())}" if verbose and status else ""))
    print(f"[scan] gabarits portant #1225 : {total} — motif enfant-texte mesure : "
          f"{protected} protege(s), {broken} casse(s) ; "
          f"{total - protected - broken} non applicables (texte direct)")
    for line in report:
        print("   ", line)
    return broken == 0, report


def self_test(browser):
    """Mutation falsifiante : sans `> div`, la mesure doit echouer."""
    css = load_template(REPO / "Cards/Memo/Argumentum_Memo_Face_fr.json")
    reverted = css.replace(
        ".argu-lang-ar .title, .argu-lang-ar .title > div, "
        ".argu-lang-fa .title, .argu-lang-fa .title > div, "
        ".argu-lang-zh .title, .argu-lang-zh .title > div {",
        ".argu-lang-ar .title, .argu-lang-fa .title, .argu-lang-zh .title {")
    results = []
    ok_fixed, _ = check_memo(browser, css, verbose=False)
    results.append(("forme fixee -> PASS attendu", ok_fixed is True))
    ok_reverted, lines = check_memo(browser, reverted, verbose=False)
    results.append(("forme d'avant #1548 -> FAIL attendu", ok_reverted is False))
    # temoin : la mutation change bien la mesure des lettres (pas un vert fige)
    for name, good in results:
        print(f"[self-test] {name}: {'OK' if good else 'FAIL'}")
    return all(g for _, g in results), results


def main():
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("--template", default="Cards/Memo/Argumentum_Memo_Face_fr.json")
    ap.add_argument("--scan", action="store_true")
    ap.add_argument("--self-test", action="store_true")
    args = ap.parse_args()

    from playwright.sync_api import sync_playwright

    ok_all = True
    with sync_playwright() as p:
        browser = p.chromium.launch()
        page = browser.new_page()
        page.route("**/*", lambda route: route.abort())  # cascade seule, hors reseau
        if args.self_test:
            ok, _ = self_test(page)
            ok_all &= ok
        elif args.scan:
            ok, _ = scan(page)
            ok_all &= ok
        else:
            css = load_template(REPO / args.template)
            ok, lines = check_memo(page, css)
            ok_all &= ok
            for line in lines:
                print(line)
            print(f"[memo-face] {'PASS' if ok else 'FAIL'} "
                  f"({args.template}, lettres + temoins {', '.join(LANGS)})")
        browser.close()
    print("VERDICT:", "PASS" if ok_all else "FAIL")
    sys.exit(0 if ok_all else 2)


if __name__ == "__main__":
    main()
