# -*- coding: utf-8 -*-
"""Source-word sweep: untranslated FR/EN words in translated columns of live CSVs.

Built for the #994 series (dispatch 8sor1n tertiaire). Reads the 6 live CSV
sources referenced by AssetConverterConfig.cs (no Archive/ path is live) and
flags source-language words left in translated cells.

Detector classes (per target language):
  - LATIN  : any Latin/accented-Latin token in ru/ar/fa/zh columns
  - MIXED  : single token mixing Cyrillic and Latin letters (homoglyphs, #1080 class)
  - FR-CHAR/FR-LEX : FR-only accented chars (è ù œ û ï; +ç for es) or FR lexicon
    tokens in pt/es columns
  - EN-LEX : unambiguous EN function/game words in pt/es columns

Design constraints paid for in blood during the #1079-#1085 series:
  - case-insensitive everywhere (#1083: lowercase 'baratineur' was invisible to
    a case-sensitive probe)
  - NO \\b against CJK — Python classes ideograms as word chars, so \\b never
    matches at a Latin<->CJK frontier (#1083 round 1 missed 2 ZH cells)
  - link_* columns are excluded: URLs are Latin by construction
  - pt/es share the Latin script with FR/EN, so detection there is
    lexicon-based only — coverage is intentionally weaker than ru/ar/fa/zh
  - inverse control: before believing a "clean" result, the detector must find
    known witnesses on pre-fix states (see --selftest note below)

Counts are OCCURRENCES; the unit must always be stated with the number
(ai-01, cycle 8: physical lines != records != cells != occurrences on these
multi-line markdown-cell CSVs).

Usage: python tools/994-source-word-sweep.py   (from repo root)

Witness validation used at creation (2026-08-15, all PASS — reproduce with
`git show <parent>:<file>` into temp files and run detect() on them):
  W1 'plausible' in text_ar  pk 3.3.1.3.4   pre-#1079  (d7bf928a)
  W2 charabia tokens in desc_ar pk 2.3.2.4.1.4 (L503)  still on master
  W3 'Картa' MIXED in text_ru pk 7.3.2.1.1   pre-#1080  (bcfe6d12)
  W4 lowercase 'baratineur' in context_zh/issue_zh pk 2.2.6/2.2.7 pre-#1083 (fcf33c36)
"""
import csv, re, sys, collections

sys.stdout.reconfigure(encoding="utf-8")

RULES_CARDS = r"Cards/Rules/Argumentum Rules - Cards.csv"
RULES_PNP   = r"Cards/Rules/Argumentum Rules - Cards Print and Play.csv"
SCENARII    = r"Cards/Scenarii/Argumentum Scenarii - Cards.csv"
FALLACIES   = r"Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
VIRTUES     = r"Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
DNN_UI      = r"docs/dnn-localization/dnn-ui-strings.csv"

LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
NONLATIN = {"ru", "ar", "fa", "zh"}

SCEN_EN_BARE = {"category", "subcategory", "title", "smoothTalker", "drawer",
                "context", "issue", "suggestion_en"}
SCEN_FR_BARE = {"catégorie", "sous-catégorie", "titre", "baratineur", "piocheur",
                "contexte", "enjeu", "suggestion"}
META_COLS = {"pk", "path", "coordonnées", "CCby", "print_and_play",
             "édition février 2022", "print_&_play_fevrier_2022", "depth",
             "decimal_path_padded", "depth_max4", "card", "update", "locked",
             "KIDZ", "family_fr_camelcase", "source_file", "key", "notes"}

FILES = [
    ("rules", RULES_CARDS), ("rulespp", RULES_PNP), ("scen", SCENARII),
    ("fall", FALLACIES), ("virt", VIRTUES), ("dnn", DNN_UI),
]

def classify(col, filekey):
    if filekey == "dnn":
        return col if col in ("fr", *LANGS) else "meta"
    if col in META_COLS:
        return "meta"
    if col.startswith("link_") or col == "link":
        return "meta"  # URL columns: Latin by construction (#804 aside)
    m = re.search(r"_(en|ru|pt|es|ar|fa|zh|fr)$", col)
    if m:
        return m.group(1)
    if filekey == "scen" and col in SCEN_EN_BARE:
        return "en"
    if filekey == "scen" and col in SCEN_FR_BARE:
        return "fr"
    if filekey in ("rules", "fallacies"):
        return "fr"  # Text, text, desc, example, Famille...
    return "meta"

LATIN_TOKEN = re.compile(r"[A-Za-zÀ-ÖØ-öø-ÿŒœÆæ]{2,}")
CYR = re.compile(r"[А-ЯЁа-яё]")
LAT1 = re.compile(r"[A-Za-z]")

# ---- whitelists (all entries inspected by hand on 2026-08-15) --------------
LATIN_MAXIM = set("""ad hoc ergo propter post reductio absurdum credo quia ipse
dixit mutatis mutandis priori posteriori nemo auditur propriam turpitudinem allegans
versus vs etc et al sic non sequitur cum sum""".split())
SYLLOGISTIC = set("""barbara celarent cesare darii ferio datisi disamis bocardo
camenes camestres dimatis ferison fesapo darapti modus ponens tollens""".split())
EN_FALLACY_BARE = set("""whataboutism gaslighting darvo gish gallop creepypasta
clickbait deepfake fake news boomer fallacies fallacy slippery slope strawman
straw man moving goalposts red herring no true scotsman motte bailey sea lioning
sealioning""".split())
# fallacy/effect names legitimately kept in Latin/EN inside ru/ar/fa/zh name cells
FALLACY_LATIN = set("""hominem fori contrario silentio idola jingle jangle type
token swish""".split())
PROPER = set("""sherlock holmes watson poirot marple batman superman gandalf
pulp fiction tarantino jean luc picard spock mccoy marcellus wallace mia vincent
jules churchill napoleon caesar cesar cleopatra galilée galileo newton descartes
kant hegel nietzsche socrate socrates platon platon aristote aristotle diogene
senèque tintin asterix obelix mickey tarzan zorro king kong godzilla cyclope thor
zeus ulysse odysseus penelope circe calypso wonderland grimm disney shakespeare
moliere racine corneille hugo zola balzac camus sartre freud jung einstein tesla
edison ford chewbacca kashyyyk wookiee endor rachel ross neo frodon gollum johnny
hallyday halliday panoramix obélix obelix bilou victor google casper aurelia
cotta nanine restorff caméléa camelea caméléon""".split())
UNITS_MISC = set("""cm mm km kg ms ok dnn pdf html url http https www isbn tv cd
dvd sms gsm gps led lcd usb api ai vip argumentum""".split())
LOANWORDS = set("""déjà vu gruyère pizzaïolo je coinche bug trompe l'œil oeil œil
high five mind mapping""".split())

def whitelisted(tok):
    t = tok.lower().strip(".,;:!?()'\"«»„“”…—–")
    if not t:
        return True
    if (t in LATIN_MAXIM or t in SYLLOGISTIC or t in EN_FALLACY_BARE
            or t in FALLACY_LATIN or t in PROPER or t in UNITS_MISC
            or t in LOANWORDS):
        return True
    if re.fullmatch(r"[IVXLC]+", tok):          # Roman numerals
        return True
    if re.fullmatch(r"[A-ZÀ-ÖØ-Þ]{2,8}", tok):  # sigles/acronyms (incl. WYSIWYG)
        return True
    if re.match(r"^\d", tok):                   # starts with digit (3.ª, 2e)
        return True
    return False

# FR markers for pt/es columns
FR_CHARS = {"pt": "èùœûï", "es": "èùœûïç"}
FR_LEX = set("""des une dans avec qui dont donc sont été était chaque leur
lorsqu puisqu très déjà après être avoir fait faire joueurs carte cartes
scénario sophisme exemple définition réserve défausse baratineur piocheur
saynète vertus joueur énoncé règle règles fallacieux fallacieuse
c'est n'est qu'il qu'elle d'un d'une l'appel jusqu' afin""".split())
# words valid in BOTH FR and the target script-sharing language -> not FR markers
AMBIG = {"es": {"les", "mais", "pas", "plus", "est", "son", "sur", "une", "sin"},
         "pt": {"une", "les", "mais", "pas", "plus", "est", "son", "sur", "onde"}}
EN_LEX = set("""the and with must then which when while from that this these
those they their player players card cards scenario fallacy drawer reader round
trick supply discard smooth talker""".split())

def detect(lang, cell):
    """yield (kind, token) suspects for one cell of target lang"""
    if not cell:
        return
    if lang in NONLATIN:
        for m in LATIN_TOKEN.finditer(cell):
            tok = m.group(0)
            if not whitelisted(tok):
                yield ("LATIN", tok)
        if lang == "ru":
            for m in re.finditer(r"[А-ЯЁа-яёA-Za-z]+", cell):
                w = m.group(0)
                if CYR.search(w) and LAT1.search(w):
                    yield ("MIXED", w)
    elif lang in ("pt", "es"):
        for m in LATIN_TOKEN.finditer(cell):
            tok = m.group(0)
            tl = tok.lower().strip(".,;:!?()'\"«»…—–")
            if tl in AMBIG[lang] or whitelisted(tok):
                continue
            if any(c in FR_CHARS[lang] for c in tl):
                yield ("FR-CHAR", tok)
            elif tl in FR_LEX:
                yield ("FR-LEX", tok)
            elif tl in EN_LEX:
                yield ("EN-LEX", tok)

def main():
    findings = collections.defaultdict(list)   # (file,col,lang,kind,token) -> [(row,pk,ctx)]
    for filekey, path in FILES:
        rows = list(csv.DictReader(open(path, encoding="utf-8-sig", newline="")))
        for i, r in enumerate(rows, 1):
            pk = r.get("pk") or r.get("path") or r.get("key") or str(i)
            for col in r:
                lang = classify(col, filekey)
                if lang in ("meta", "fr", "en"):
                    continue
                cell = r[col] or ""
                for kind, tok in detect(lang, cell):
                    pos = cell.find(tok)
                    ctx = cell[max(0, pos - 25):pos + len(tok) + 25].replace("\n", " ")
                    findings[(filekey, col, lang, kind, tok)].append((i, pk, ctx))
    total = 0
    for (filekey, col, lang, kind, tok), hits in sorted(
            findings.items(), key=lambda kv: (-len(kv[1]), kv[0])):
        n = len(hits)
        total += n
        print(f"[{filekey}] {col} ({lang}) {kind} {tok!r} x{n}")
        for i, pk, ctx in hits[:3]:
            print(f"    row {i} pk={pk}: ...{ctx}...")
        if n > 3:
            print(f"    ... +{n - 3} more")
    print(f"\nTOTAL occurrences: {total}")
    print(f"DISTINCT (file,col,lang,kind,token) groups: {len(findings)}")

if __name__ == "__main__":
    main()
