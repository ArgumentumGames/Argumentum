# -*- coding: utf-8 -*-
"""
READ-ONLY translation quality audit for Argumentum CSV datasets.
Scans for: cross-language contamination, known mistranslations (homonyms),
garbled/mojibake cells, intra-dataset term inconsistency.

Usage:
    python 2026-07-02-scan-translations.py [--json out.json] [--repo PATH]

Baseline: 22 raw findings -> 7 after FR-canon cross-check + loanword whitelist
+ short-cell exemption (post-#640 Rules refonte, master 99145fab). The residual 7
were arbitrated in docs/investigations/2026-07-02-scanner-fp-arbitration.md (#644):
all KEEP (proper nouns / Latin mnemonics / loanwords the FR canon itself keeps
untranslated).

Three FP-reduction heuristics (vs the pre-#644 baseline scanner):
  1. LOANWORD/MNEMONIC WHITELIST  — LEGIT_TOKENS extended with conserved loanwords
     (Gish gallop, Creepypasta, Whataboutism, Credo quia absurdum) and the medieval
     syllogism mnemonics (Barbara, Celarent, Darii, ... Bamalip). These tokens are
     stripped before script-ratio scoring, so a ZH title "Celarent 三段论" no longer
     reads as "only 50% CJK".
  2. SHORT-CELL EXEMPTION (MIN_SCRIPT_CHARS=12) — cells shorter than 12 letters are
     almost always proper nouns / short labels; the <30% script-ratio threshold is
     inapplicable to them. They are skipped (mojibake/probe checks still run).
  3. FR-CANON CROSS-CHECK — before flagging a _ru/_ar/_fa/_zh cell as "contaminated"
     (low script ratio), we look at the sibling FR cell: if the FR source ITSELF keeps
     the same Latin/loanword token untranslated, the translation is COHERENT (mirroring
     the canon), not contaminated -> alert suppressed. This is the single biggest FP
     killer: it encodes the rule "the FR canon is allowed to keep loanwords, so the
     translations that do the same are correct".
"""
import csv, json, re, sys, os
from collections import defaultdict

REPO = os.environ.get("ARGUMENTUM_REPO", os.getcwd())

DATASETS = {
    "Rules": [
        r"Cards\Rules\Argumentum Rules - Cards.csv",
        r"Cards\Rules\Argumentum Rules - Cards Print and Play.csv",
    ],
    "Fallacies": [r"Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv"],
    "Virtues": [r"Cards\Fallacies\Argumentum Virtues - Taxonomy.csv"],
    "Scenarii": [r"Cards\Scenarii\Argumentum Scenarii - Cards.csv"],
}

# ---------------------------------------------------------------- column maps
# severity_role: HIGH = card title / rendered header, MEDIUM = rendered body, LOW = not rendered

def rules_colmap():
    m = {}
    for lang, col in [("fr","Text"),("en","Text_en"),("ru","Text_ru"),("pt","Text_pt"),
                      ("ar","Text_ar"),("es","Text_es"),("zh","Text_zh"),("fa","Text_fa")]:
        m[col] = (lang, "MEDIUM")
    return m

def fallacies_colmap():
    m = {}
    fr_high = ["Famille","Sous-Famille","Soussousfamille","text_fr","nom_vulgarisé"]
    fr_med  = ["desc_fr","example_fr"]
    for c in fr_high: m[c] = ("fr","HIGH")
    for c in fr_med:  m[c] = ("fr","MEDIUM")
    m["link_fr"] = ("fr","LOW")
    en_high = ["Family","Subfamily","Subsubfamily","text_en","Simple_name_en"]
    en_med  = ["desc_en","example_en","example_en_bis","political_example_en"]
    for c in en_high: m[c] = ("en","HIGH")
    for c in en_med:  m[c] = ("en","MEDIUM")
    m["link_en"] = ("en","LOW")
    for lang in ["ru","pt","ar","es","zh","fa"]:
        for c in [f"Family_{lang}",f"Subfamily_{lang}",f"Subsubfamily_{lang}",f"text_{lang}"]:
            m[c] = (lang,"HIGH")
        for c in [f"desc_{lang}",f"example_{lang}"]:
            m[c] = (lang,"MEDIUM")
        m[f"link_{lang}"] = (lang,"LOW")
    return m

def virtues_colmap():
    m = {}
    for lang in ["fr","en","ru","pt","ar","es","zh","fa"]:
        for c in [f"family_{lang}",f"subfamily_{lang}",f"subsubfamily_{lang}",f"title_{lang}"]:
            m[c] = (lang,"HIGH")
        for c in [f"description_{lang}",f"remark_{lang}"]:
            m[c] = (lang,"MEDIUM")
        m[f"link_{lang}"] = (lang,"LOW")
    return m

def scenarii_colmap():
    m = {}
    m["catégorie"]=("fr","MEDIUM"); m["sous-catégorie"]=("fr","MEDIUM")
    m["titre"]=("fr","HIGH"); m["baratineur"]=("fr","MEDIUM"); m["piocheur"]=("fr","MEDIUM")
    m["contexte"]=("fr","MEDIUM"); m["enjeu"]=("fr","MEDIUM"); m["suggestion"]=("fr","MEDIUM")
    m["category"]=("en","MEDIUM"); m["subcategory"]=("en","MEDIUM")
    m["title"]=("en","HIGH"); m["smoothTalker"]=("en","MEDIUM"); m["drawer"]=("en","MEDIUM")
    m["context"]=("en","MEDIUM"); m["issue"]=("en","MEDIUM"); m["suggestion_en"]=("en","MEDIUM")
    for lang in ["ru","pt","ar","es","zh","fa"]:
        m[f"title_{lang}"]=(lang,"HIGH")
        for c in [f"category_{lang}",f"subcategory_{lang}",f"smoothTalker_{lang}",
                  f"drawer_{lang}",f"context_{lang}",f"issue_{lang}",f"suggestion_{lang}"]:
            m[c]=(lang,"MEDIUM")
    return m

COLMAPS = {
    r"Cards\Rules\Argumentum Rules - Cards.csv": rules_colmap(),
    r"Cards\Rules\Argumentum Rules - Cards Print and Play.csv": rules_colmap(),
    r"Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv": fallacies_colmap(),
    r"Cards\Fallacies\Argumentum Virtues - Taxonomy.csv": virtues_colmap(),
    r"Cards\Scenarii\Argumentum Scenarii - Cards.csv": scenarii_colmap(),
}

# FR-canon sibling column per (file, lang) — used by the cross-check (#3).
# Maps a translated column -> the FR source column of the same record/field.
def fr_sibling(relpath):
    """Return dict: translated_col -> fr_col, for the given file."""
    if "Rules" in relpath:
        return {c: "Text" for c in ["Text_en","Text_ru","Text_pt","Text_ar","Text_es","Text_zh","Text_fa"]}
    if "Fallacies - Taxonomy" in relpath:
        d = {}
        for fr, pref in [("Famille","Family"),("Sous-Famille","Subfamily"),
                         ("Soussousfamille","Subsubfamily"),("text_fr","text"),("desc_fr","desc"),
                         ("example_fr","example")]:
            for lang in ["en","ru","pt","ar","es","zh","fa"]:
                d[f"{pref}_{lang}" if pref in ("text","desc") else (pref if lang=="en" else f"{pref}_{lang}")] = fr
        return d
    if "Virtues - Taxonomy" in relpath:
        d = {}
        for pref in ["family","subfamily","subsubfamily","title","description","remark"]:
            for lang in ["en","ru","pt","ar","es","zh","fa"]:
                d[f"{pref}_{lang}"] = f"{pref}_fr"
        return d
    if "Scenarii" in relpath:
        return {"title":"titre","title_ru":"titre","title_pt":"titre","title_ar":"titre",
                "title_es":"titre","title_zh":"titre","title_fa":"titre"}
    return {}

# ------------------------------------------------------------ language models
SW = {
    "en": {"the","of","and","is","are","you","your","with","this","that","from","they",
           "have","has","will","was","were","which","when","must","not","be","it","its",
           "their","would","can","should","who","what","by","an","but","then",
           "there","them","his","her","we","our","us","these","those","because","about",
           "to","in","for","at","into","during","against","without","while","other"},
    "fr": {"les","des","une","est","dans","avec","qui","vous","votre","vos","pour",
           "sur","ne","pas","du","au","aux","cette","être","ses","leurs","donc",
           "c'est","d'un","d'une","elle","nous","lui","cela","ça","très",
           "où","dès","chaque","tout","toute","tous","toutes","fait",
           "été","sont","ont","doit","peut","je","ils","elles","aussi","afin",
           "vôtre","celui","celle","ceux","lorsque","ainsi","alors","dont"},
    "pt": {"não","é","uma","você","vocês","com","da","dos","das","em","são","muito",
           "deve","ele","ela","seu","sua","suas","seus","também","já","ao",
           "pelo","pela","isso","esse","essa","um","às","até",
           "foi","tem","têm","quando","pode","fazer","depois","então","onde",
           "mesmo","outro","outra","apenas","assim","estão"},
    "es": {"el","los","las","y","es","una","usted","pero","más","también","con",
           "del","según","debe","cuando","puede","hacer","muy",
           "su","sus","al","lo","esto","eso","fue","tiene","tienen",
           "hay","ya","mientras","desde","después","aquí","cómo","qué","están"},
}
_inter_pt_es = SW["pt"] & SW["es"]
SW["pt"] -= _inter_pt_es
SW["es"] -= _inter_pt_es
_inter_fr_pt = SW["fr"] & SW["pt"]
SW["fr"] -= _inter_fr_pt
SW["pt"] -= _inter_fr_pt

WORD_RE = re.compile(r"[a-zA-ZÀ-ÿА-я']+", re.UNICODE)

def latin_scores(text):
    words = [w.lower() for w in WORD_RE.findall(text)]
    n = len(words)
    if n == 0: return {}, 0
    sc = {}
    for lang, sws in SW.items():
        sc[lang] = sum(1 for w in words if w in sws) / n
    return sc, n

def script_ratio(text, predicate):
    letters = [c for c in text if c.isalpha()]
    if not letters: return None, 0
    return sum(1 for c in letters if predicate(c)) / len(letters), len(letters)

def is_cyr(c): return 'Ѐ' <= c <= 'ӿ'
def is_arabic(c): return '؀' <= c <= 'ۿ' or 'ݐ' <= c <= 'ݿ' or 'ﭐ' <= c <= '﷿' or 'ﹰ' <= c <= '﻿'
def is_cjk(c):
    o = ord(c)
    return (0x4E00 <= o <= 0x9FFF) or (0x3400 <= o <= 0x4DBF) or (0xF900 <= o <= 0xFAFF) or (0x3000 <= o <= 0x303F)

# Improvement #1 — extended whitelist: Latin fallacy names + loanwords the FR canon
# keeps untranslated + medieval syllogism mnemonics (Barbara..Bamalip). Stripped
# before script-ratio scoring so they no longer read as "foreign script gaps".
SYLLOGISM_MNEMONICS = (
    r"barbara|celarent|darii|ferio|cesare|camestres|festino|baroco|"
    r"darapti|felapton|disamis|datisi|bocardo|ferison|camenes|dimatis|fesapo|fresison|bamalip"
)
LEGIT_TOKENS = re.compile(
    r"\b(argumentum|ad\s+\w+|post\s+hoc|ergo\s+(propter\s+hoc|sum|decedo)|ipse\s+dixit|"
    r"non\s+sequitur|petitio\s+principii|reductio\s+ad\s+absurdum|tu\s+quoque|"
    r"cum\s+hoc|a\s+priori|a\s+posteriori|ad\s+hominem|ad\s+populum|ad\s+ignorantiam|"
    r"sherlock|cleopatra|cléopâtre|jeanne\s+d'arc|caesar|césar|wikipedia|wikipédia|"
    r"secundum\s+quid|idem|sic|de\s+facto|modus\s+(ponens|tollens)|"
    r"straw\s?man|red\s+herring|"
    r"gish\s?gallop|creepypasta|whataboutism|credo\s+quia\s+absurdum|"
    + SYLLOGISM_MNEMONICS + r")\b", re.I)

# Improvement #2 — short cells (proper nouns / short labels) are exempt from the
# script-ratio contamination check. The <30% threshold is meaningless on 2-word titles.
MIN_SCRIPT_CHARS = 12

URL_RE = re.compile(r"https?://\S+")
MD_STRIP = re.compile(r"[#*_>\-|`\[\]()]+")

MOJIBAKE_PATTERNS = [
    ("�", "U+FFFD replacement char"),
    ("Ã©", "UTF8-as-latin1 'é'"), ("Ã¨", "UTF8-as-latin1 'è'"), ("Ã :", "UTF8-as-latin1 'à'"),
    ("Ã§", "UTF8-as-latin1 'ç'"), ("Ãª", "UTF8-as-latin1 'ê'"),
    ("â€™", "UTF8-as-latin1 right quote"), ("â€œ", "UTF8-as-latin1 left dquote"),
    ("â€", "UTF8-as-latin1 right dquote"), ("â€“", "UTF8-as-latin1 en-dash"),
    ("Ã°", "UTF8-as-latin1 misc"), ("Ð ", "double-encoded cyrillic"),
]

LITERAL_PROBES = [
    "english channel", "sleeve", "round of the", "the channel",
    "canal da mancha", "canal de la mancha", "ла-манш", "la manche",
]

findings = []
CUR_ID = {"id": ""}

def add(f, rec, line, col, cell, klass, sev, note):
    excerpt = re.sub(r"\s+", " ", cell.strip())[:80]
    findings.append({
        "file": f, "record": rec, "line": line, "id": CUR_ID["id"], "column": col,
        "excerpt": excerpt, "class": klass, "severity": sev, "note": note,
    })

HEADER_RE = re.compile(r"^\s*#{1,3}\s+(.+)$", re.M)

def fr_keeps_same_latin(fr_cell, translated_cell):
    """Improvement #3 — FR-canon cross-check (SAFE variant).
    True ONLY when the FR source and the translation share a KNOWN loanword / proper
    noun / Latin mnemonic (LEGIT_TOKENS). Restricted on purpose: a broad Latin-token
    overlap would mask the #211 clobber case where a _ru/_pt cell is literally
    untranslated French (FR and the garbage share ordinary French words like
    'baratineur'). By limiting the overlap to the curated loanword/mnemonic set, we
    suppress only the coherent-loanword FPs (Gish gallop, Celarent, ...) while still
    flagging real French-contamination."""
    if not fr_cell:
        return False
    fr_legit = set(m.group(0).lower() for m in LEGIT_TOKENS.finditer(fr_cell))
    tr_legit = set(m.group(0).lower() for m in LEGIT_TOKENS.finditer(translated_cell))
    return bool(fr_legit & tr_legit)

def scan_rules_headers(f, rec, line, col, lang, cell, fr_cell):
    for m in HEADER_RE.finditer(cell):
        htext = clean_for_lang(m.group(1)).strip()
        if not htext:
            continue
        plain = MD_STRIP.sub(" ", htext)
        if lang == "ru":
            ratio, nlet = script_ratio(plain, is_cyr)
            if ratio is not None and nlet >= 8 and ratio < 0.30 \
               and not fr_keeps_same_latin(fr_cell, m.group(1)):
                add(f, rec, line, col, m.group(0), "contamination", "HIGH",
                    f"RU card title only {ratio:.0%} cyrillic")
        elif lang in ("ar","fa"):
            ratio, nlet = script_ratio(plain, is_arabic)
            if ratio is not None and nlet >= 8 and ratio < 0.30 \
               and not fr_keeps_same_latin(fr_cell, m.group(1)):
                add(f, rec, line, col, m.group(0), "contamination", "HIGH",
                    f"{lang.upper()} card title only {ratio:.0%} arabic-script")
        elif lang == "zh":
            letters = [c for c in plain if c.isalpha() or is_cjk(c)]
            if letters and len(letters) >= 6:
                cjk = sum(1 for c in letters if is_cjk(c))
                if cjk/len(letters) < 0.30 and not fr_keeps_same_latin(fr_cell, m.group(1)):
                    add(f, rec, line, col, m.group(0), "contamination", "HIGH",
                        f"ZH card title only {cjk/len(letters):.0%} CJK")
        else:
            sc, n = latin_scores(plain)
            if n < 3:
                continue
            own = sc.get(lang, 0)
            for other, s in sc.items():
                if other == lang: continue
                if s >= 0.25 and own == 0 and s * n >= 2:
                    add(f, rec, line, col, m.group(0), "contamination", "HIGH",
                        f"{lang.upper()} card title looks {other.upper()} (sw={s:.0%}, {n} words)")
                    break

IL_Y_A = re.compile(r"\bil\s+y\s+a\b|\by\s+a\b", re.I)

def clean_for_lang(text):
    t = URL_RE.sub(" ", text)
    t = LEGIT_TOKENS.sub(" ", t)
    t = IL_Y_A.sub(" ", t)
    return t

def scan_cell(f, rec, line, col, lang, base_sev, cell, fr_cell):
    raw = cell
    if not raw or not raw.strip():
        return
    for pat, desc in MOJIBAKE_PATTERNS:
        if pat in raw:
            add(f, rec, line, col, raw, "garbled", "HIGH" if base_sev=="HIGH" else "MEDIUM",
                f"mojibake: {desc} ({pat!r})")
            break
    low = raw.lower()
    for probe in LITERAL_PROBES:
        if probe in low:
            if probe == "la manche" and lang == "fr":
                continue
            if probe == "sleeve" and lang == "en" and "card sleeve" in low:
                continue
            add(f, rec, line, col, raw, "known-mistranslation", "HIGH",
                f"literal probe {probe!r} found")
            break

    if base_sev == "LOW":
        return

    txt = clean_for_lang(raw)
    stripped = MD_STRIP.sub(" ", txt)

    # Improvement #2 — short-cell exemption for script-ratio checks
    nletters = sum(1 for c in stripped if c.isalpha() or is_cjk(c))

    if lang == "ru":
        ratio, nlet = script_ratio(stripped, is_cyr)
        if ratio is not None and nlet >= 15 and ratio < 0.30 \
           and nletters >= MIN_SCRIPT_CHARS \
           and not fr_keeps_same_latin(fr_cell, raw):
            add(f, rec, line, col, raw, "contamination", base_sev,
                f"_ru cell only {ratio:.0%} cyrillic ({nlet} letters)")
        return
    if lang in ("ar","fa"):
        ratio, nlet = script_ratio(stripped, is_arabic)
        if ratio is not None and nlet >= 15 and ratio < 0.30 \
           and nletters >= MIN_SCRIPT_CHARS \
           and not fr_keeps_same_latin(fr_cell, raw):
            add(f, rec, line, col, raw, "contamination", base_sev,
                f"_{lang} cell only {ratio:.0%} arabic-script ({nlet} letters)")
        return
    if lang == "zh":
        letters = [c for c in stripped if c.isalpha() or is_cjk(c)]
        if letters:
            cjk = sum(1 for c in letters if is_cjk(c))
            ratio = cjk/len(letters)
            if len(letters) >= 10 and ratio < 0.30 \
               and not fr_keeps_same_latin(fr_cell, raw):
                add(f, rec, line, col, raw, "contamination", base_sev,
                    f"_zh cell only {ratio:.0%} CJK ({len(letters)} chars)")
        return

    sc, n = latin_scores(stripped)
    if n < 6:
        return
    own = sc.get(lang, 0)
    for other, s in sc.items():
        if other == lang: continue
        if s >= 0.15 and own <= 0.04 and s > own * 3 and s * n >= 2:
            add(f, rec, line, col, raw, "contamination", base_sev,
                f"_{lang} cell looks {other.upper()} (sw {other}={s:.0%}, {lang}={own:.0%}, {n} words)")
            break

def scan_file(relpath):
    path = os.path.join(REPO, relpath)
    colmap = COLMAPS[relpath]
    sibling = fr_sibling(relpath)
    with open(path, encoding="utf-8-sig", newline="") as fh:
        reader = csv.reader(fh)
        header = next(reader)
        header = [h.strip().lstrip("﻿") for h in header]
        idx = {h: i for i, h in enumerate(header)}
        unknown = [h for h in header if h not in colmap]
        rec = 1
        prev_line = reader.line_num
        is_rules = "Rules" in relpath
        for row in reader:
            rec += 1
            start_line = prev_line + 1
            prev_line = reader.line_num
            CUR_ID["id"] = row[0] if row else ""
            for col, (lang, sev) in colmap.items():
                i = idx.get(col)
                if i is None or i >= len(row): continue
                fr_col = sibling.get(col)
                fr_cell = row[idx[fr_col]] if (fr_col and fr_col in idx and idx[fr_col] < len(row)) else ""
                scan_cell(relpath, rec, start_line, col, lang, sev, row[i], fr_cell)
                if is_rules and row[i].strip():
                    scan_rules_headers(relpath, rec, start_line, col, lang, row[i], fr_cell)
    return unknown

def main():
    if "--repo" in sys.argv:
        global REPO
        REPO = sys.argv[sys.argv.index("--repo")+1]
    unmapped = {}
    for ds, files in DATASETS.items():
        for f in files:
            unmapped[f] = scan_file(f)
    by = defaultdict(int)
    for fd in findings:
        ds = next(d for d, fs in DATASETS.items() if fd["file"] in fs)
        fd["dataset"] = ds
        by[(ds, fd["class"], fd["severity"])] += 1
    print("=== SUMMARY (dataset / class / severity : count) ===")
    for k in sorted(by):
        print(f"  {k[0]:10s} {k[1]:22s} {k[2]:6s} : {by[k]}")
    print(f"TOTAL: {len(findings)}")
    print()
    print("=== FINDINGS ===")
    for fd in sorted(findings, key=lambda x: ({"HIGH":0,"MEDIUM":1,"LOW":2}[x["severity"]], x["file"], x["record"])):
        print(f"[{fd['severity']}] {fd['class']} | {fd['file']} | rec {fd['record']} (line ~{fd['line']}, id={fd['id']}) | {fd['column']}")
        print(f"    {fd['note']}")
        print(f"    >> {fd['excerpt']}")
    if "--json" in sys.argv:
        out = sys.argv[sys.argv.index("--json")+1]
        with open(out, "w", encoding="utf-8") as fh:
            json.dump(findings, fh, ensure_ascii=False, indent=1)
        print(f"\nJSON written to {out}")
    print("\n=== UNMAPPED COLUMNS (not scanned) ===")
    for f, cols in unmapped.items():
        print(f"  {f}: {cols}")

if __name__ == "__main__":
    main()
