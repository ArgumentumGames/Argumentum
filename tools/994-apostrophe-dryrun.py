#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
#994 — dry-run instrument for the apostrophe branches A/B/C/D.

Reference: docs/release-dossier/994-apostrophes-8langues-2026-09-14.md (§1 convention,
§2 rendered columns, §3 reverse sweep, §6 decision sheet). Every scope rule below is
QUOTED from that dossier and cites its section; when the dossier and this script
disagree, the dossier wins and the script is wrong.

ZERO-CORPUS-WRITE BY CONSTRUCTION: this script has NO in-place mode. It reads a
corpus tree, derives the exact worklist per branch, and can `--emit` transformed
COPIES to a directory of your choice (which it refuses to place inside the corpus
tree). `Cards/` stays byte-identical; the caller proves it with hashes.

Convention (acquired, #1073): FR = ’ (curved) / EN = ' (straight).
Intra-word = letter · apostrophe · letter (Python isalpha — the predicate that
reproduces every figure of the dossier §2 table, including the zh/ar quote rows).

Exclusions (§1, encoded as a RENDERED whitelist + a link_* prefix ban, self-tested):
  - link_* : URLs — `…#Types_d'arguments` — normalising breaks the link;
  - no-reader columns (§5 FP#2): Remarques, proverbe, `exemple politique`,
    political_example_en, example_en_bis, Simple_name_en, KIDZ, image, …
    (anything not in the rendered sets is out by construction);
  - non-intra-word apostrophes: single-quote guillemets (`'自由'`, `'علم المناخ'`)
    are hors périmètre (§1) — only intra-word positions are ever touched.

Branches (§6):
  A  FR seule      : FR rendered columns of Scenarii/Rules/RulesPnP +
                     Virtues AIF_criticalQuestion (emitted to OWL). Fallacies FR and
                     Virtues FR title/description/remark stay in scope ON PURPOSE:
                     they derive 0, which is the falsifiable closure proof of §2.1.
  B  A + EN reverse: + curved →straight in EN rendered columns (the 22 of §3).
  C  8 langues     : + all other languages →curved. ⚠️ dossier: "ne se justifie
                     pas comme un nettoyage" — ru/pt/es change a LANGUAGE convention
                     (`d'água` is correct pt), zh/ar touches quote-class characters.
                     The instrument flags those rows instead of hiding them.
  D  ne rien faire : no worklist.

Usage:
    python tools/994-apostrophe-dryrun.py                       # report A/B/C/D (repo root)
    python tools/994-apostrophe-dryrun.py --corpus DIR          # alternate repo root
    python tools/994-apostrophe-dryrun.py --worklist B          # cell-level TSV on stdout
    python tools/994-apostrophe-dryrun.py --emit A --to DIR     # transformed COPIES to DIR
    python tools/994-apostrophe-dryrun.py --self-test           # throwaway-fixture suite
"""
import argparse, os, sys, tempfile

STRAIGHT = "'"
CURVED = "’"

CORPORA = [
    # (key, path, id-column)  — id columns verified against the headers at d7013df1
    ("Scenarii",  "Cards/Scenarii/Argumentum Scenarii - Cards.csv",              "coordonnées"),
    ("Fallacies", "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv",          "PK"),
    ("Virtues",   "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv",            "pk"),
    ("Rules",     "Cards/Rules/Argumentum Rules - Cards.csv",                     "pk"),
    ("RulesPnP",  "Cards/Rules/Argumentum Rules - Cards Print and Play.csv",      "pk"),
]

OTHER = ["ru", "pt", "es", "ar", "zh", "fa"]
SCEN_FIELDS = ["category", "subcategory", "title", "smoothTalker", "drawer", "context", "issue", "suggestion"]
FAL_FIELDS = ["text", "desc", "example"]
VIR_FIELDS = ["title", "description", "remark"]

# Rendered/emitted columns per corpus × language (dossier §2 "R" rows, §2.1, §2.2, §5 FP#2).
# AIF_criticalQuestion is not mustache-rendered but IS emitted to the OWL (§1).
RENDERED = {
    "Scenarii": {
        "fr": ["titre", "baratineur", "piocheur", "contexte", "enjeu", "suggestion"],
        "en": SCEN_FIELDS[:7] + ["suggestion_en"],
        **{lg: [f"{c}_{lg}" for c in SCEN_FIELDS] for lg in OTHER},
    },
    "Fallacies": {
        "fr": ["Famille", "Sous-Famille", "Soussousfamille", "text_fr", "desc_fr", "example_fr"],
        "en": ["text_en", "desc_en", "example_en"],
        **{lg: [f"{c}_{lg}" for c in FAL_FIELDS] for lg in OTHER},
    },
    "Virtues": {
        "fr": ["family_fr", "subfamily_fr", "subsubfamily_fr",
               "title_fr", "description_fr", "remark_fr", "AIF_criticalQuestion"],
        "en": ["title_en", "description_en", "remark_en"],
        **{lg: [f"{c}_{lg}" for c in VIR_FIELDS] for lg in OTHER},
    },
    "Rules":    {"fr": ["Text"], "en": ["Text_en"], **{lg: [f"Text_{lg}"] for lg in OTHER}},
    "RulesPnP": {"fr": ["Text"], "en": ["Text_en"], **{lg: [f"Text_{lg}"] for lg in OTHER}},
}

BRANCH_LANGS = {"A": ["fr"], "B": ["fr", "en"], "C": ["fr", "en"] + OTHER, "D": []}
TARGET = {"fr": CURVED, "en": STRAIGHT, **{lg: CURVED for lg in OTHER}}

# Dossier §6 figures (measured on 21a72385, pre-71) — carried as RAPPORTÉ, never asserted.
EXPECTED = {"A_cartes": 146, "B_delta_cartes": 18, "C_cartes": 183}
# "Cartes dont le pixel change" (§4.1/§4.2) : toutes les rangées de corpus-cartes comptent,
# mais PAS les surfaces OWL-only — AIF_criticalQuestion alimente l'ontologie, aucun pixel.
OWL_ONLY = {("Virtues", "fr", "AIF_criticalQuestion")}


def cartes_per_lang(res):
    """Dossier §4.2: « le total se calcule par langue, pas par union de PK » —
    union des ids par (langue × corpus) sur les colonnes rendues CARTE, puis somme
    sur les langues (une même coordonnée désigne une carte FR ET une carte EN)."""
    per_lc = {}
    for key in res:
        for (lg, col), h in res[key]["hits"].items():
            if (key, lg, col) in OWL_ONLY:
                continue
            per_lc.setdefault((lg, key), set()).update(h[2])
    per_lang = {}
    for (lg, _key), ids in per_lc.items():
        per_lang[lg] = per_lang.get(lg, 0) + len(ids)
    return per_lang

# ── byte-exact CSV splitters (house pattern: doubled quotes + embedded newlines safe) ──
def split_logical_rows(text):
    rows, cur, in_q = [], [], False
    i, n = 0, len(text)
    while i < n:
        ch = text[i]
        if ch == '"':
            if in_q and i + 1 < n and text[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch in "\r\n" and not in_q:
            if ch == "\r" and i + 1 < n and text[i + 1] == "\n":
                rows.append(("".join(cur), "\r\n")); cur = []; i += 2
            else:
                rows.append(("".join(cur), "\n")); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    if cur:
        rows.append(("".join(cur), ""))
    return rows


def split_fields(row):
    fields, cur, in_q = [], [], False
    i, n = 0, len(row)
    while i < n:
        ch = row[i]
        if ch == '"':
            if in_q and i + 1 < n and row[i + 1] == '"':
                cur.append('""'); i += 2
            else:
                in_q = not in_q; cur.append(ch); i += 1
        elif ch == "," and not in_q:
            fields.append("".join(cur)); cur = []; i += 1
        else:
            cur.append(ch); i += 1
    fields.append("".join(cur))
    return fields


def unquote(field):
    if len(field) >= 2 and field[0] == '"' and field[-1] == '"':
        return field[1:-1].replace('""', '"')
    return field


# ── the transformation ─────────────────────────────────────────────────────────
def wrong_for(lang):
    return STRAIGHT if TARGET[lang] == CURVED else CURVED


def convert_field(raw_field, lang):
    """Rewrite intra-word apostrophes of the wrong kind inside ONE raw field span.
    Returns (new_raw, n_changed). Works on the raw span: '"' and ',' are not
    isalpha(), so quoted/escaped material can never turn an apostrophe intra-word,
    and quoting state is preserved without re-serialising."""
    w, r = wrong_for(lang), TARGET[lang]
    out, changed, i, n = [], 0, 0, len(raw_field)
    while i < n:
        ch = raw_field[i]
        if ch == w:
            prev = raw_field[i - 1] if i else ""
            nxt = raw_field[i + 1] if i + 1 < n else ""
            if prev.isalpha() and nxt.isalpha():
                out.append(r); changed += 1; i += 1
                continue
        out.append(ch); i += 1
    return "".join(out), changed


def count_wrong(value, lang):
    w = wrong_for(lang)
    return sum(1 for i, ch in enumerate(value)
               if ch == w and (value[i - 1] if i else "").isalpha()
               and (value[i + 1] if i + 1 < len(value) else "").isalpha())


# ── corpus loading ─────────────────────────────────────────────────────────────
def load(root, rel):
    data = open(os.path.join(root, rel), "rb").read()
    bom = data.startswith(b"\xef\xbb\xbf")
    text = data.decode("utf-8-sig")
    rows = split_logical_rows(text)
    header = [unquote(f) for f in split_fields(rows[0][0])]
    return {"bom": bom, "rows": rows, "header": header, "raw": data}


def branch_scope(branch, corpus_key, header):
    """{(lang, column): index} in scope for this branch, from the RENDERED whitelist."""
    scope = {}
    if branch not in BRANCH_LANGS:
        return scope
    cols = {c: i for i, c in enumerate(header)}
    for lang in BRANCH_LANGS[branch]:
        for c in RENDERED[corpus_key].get(lang, []):
            if c in cols and not c.startswith("link_"):
                scope[(lang, c)] = cols[c]
    return scope


# ── analysis ───────────────────────────────────────────────────────────────────
def analyse(root, branch):
    """Returns {corpus: {...}} with per-column hits, cartes, excluded counts."""
    result = {}
    for key, rel, idcol in CORPORA:
        corp = load(root, rel)
        scope = branch_scope(branch, key, corp["header"])
        hits = {}          # (lang, col) -> [occ, cells, set(ids)]
        excluded = {}      # lang -> occ in out-of-scope columns
        inscope_cols = {c for (lg, c) in scope}
        for rowno, (row, _term) in enumerate(corp["rows"][1:], start=2):
            fields = [unquote(f) for f in split_fields(row)]
            rid = (fields[corp["header"].index(idcol)] if idcol in corp["header"] and corp["header"].index(idcol) < len(fields) else "").strip() or f"L{rowno}"
            for (lang, col), idx in scope.items():
                if idx >= len(fields):
                    continue
                occ = count_wrong(fields[idx], lang)
                if occ:
                    h = hits.setdefault((lang, col), [0, 0, set()])
                    h[0] += occ; h[1] += 1; h[2].add(rid)
            # exclusions: wrong-char intra-word in columns NOT in scope, per language
            for i, c in enumerate(corp["header"]):
                if c in inscope_cols or i >= len(fields):
                    continue
                lang = lang_of_column(key, c)
                if lang:
                    occ = count_wrong(fields[i], lang)
                else:
                    # colonne sans lecteur ni langue (Remarques, KIDZ, …) : les deux
                    # formes intra-mot comptent (fr et en visent des caractères
                    # disjoints) — bac « n/a », jamais converties.
                    occ = count_wrong(fields[i], "fr") + count_wrong(fields[i], "en")
                if occ:
                    excluded[lang or "n/a"] = excluded.get(lang or "n/a", 0) + occ
        result[key] = {"hits": hits, "excluded": excluded, "nrows": len(corp["rows"]) - 1, "rel": rel}
    return result


def lang_of_column(corpus_key, col):
    low = col.lower()
    for lg in ["fr", "en"] + OTHER:
        if low.endswith("_" + lg):
            return lg
    if corpus_key == "Scenarii":
        if col in ("titre", "baratineur", "piocheur", "contexte", "enjeu", "suggestion", "catégorie", "sous-catégorie"):
            return "fr"
        if col in SCEN_FIELDS[:7]:
            return "en"
    if corpus_key in ("Rules", "RulesPnP") and col == "Text":
        return "fr"
    if corpus_key == "Fallacies" and col in ("Famille", "Sous-Famille", "Soussousfamille"):
        return "fr"
    if corpus_key == "Virtues" and col in ("family_fr", "subfamily_fr", "subsubfamily_fr"):
        return "fr"
    if corpus_key == "Virtues" and col == "AIF_criticalQuestion":
        return "fr"
    return None


def report(root, branch, verbose=False):
    res = analyse(root, branch)
    print(f"\n════════ BRANCHE {branch} ════════")
    if branch == "D":
        print("  Ne rien faire — aucune écriture (dossier §6 D).")
        return res
    for key in res:
        hits, excluded = res[key]["hits"], res[key]["excluded"]
        tot_occ = sum(h[0] for h in hits.values())
        tot_cells = sum(h[1] for h in hits.values())
        ids = set().union(*[h[2] for h in hits.values()]) if hits else set()
        print(f"  {key:10s} {tot_occ:5d} occ. · {tot_cells:4d} cellules · {len(ids):4d} cartes/lignes"
              f"   ({res[key]['nrows']} lignes)")
        for (lang, col), h in sorted(hits.items()):
            flag = ""
            if branch == "C" and lang in ("ru", "pt", "es"):
                flag = "   ⚠ changement de convention de langue (dossier §6 C)"
            if branch == "C" and lang in ("zh", "ar"):
                flag = "   ⚠ classe guillemet, pas élision (dossier §6 C)"
            if (key, lang, col) in OWL_ONLY:
                flag = "   ⇢ surface OWL (§4.1 : aucun pixel de carte)"
            print(f"      {lang:3s} {col:22s} {h[0]:4d} occ · {h[1]:4d} cell · {len(h[2]):4d} cartes{flag}")
        if excluded:
            print(f"      (exclu, non touché : " + ", ".join(f"{lg} {n}" for lg, n in sorted(excluded.items())) + ")")
    per_lang = cartes_per_lang(res)
    cartes = sum(per_lang.values())
    detail = " + ".join(f"{lg} {n}" for lg, n in sorted(per_lang.items())) or "0"
    print(f"  ⇒ cartes dont le pixel change (§4.2 : par langue, surfaces OWL exclues) = {cartes}  [{detail}]")
    if branch == "A":
        print(f"    dossier §6 RAPPORTÉ : {EXPECTED['A_cartes']} → {'✔ reproduit' if cartes == EXPECTED['A_cartes'] else '✘ ÉCART — à dériver, pas à masquer'}")
    if branch == "B":
        resA = analyse(root, "A")
        plA = cartes_per_lang(resA)
        delta = cartes - sum(plA.values())
        print(f"    Δ vs A = {delta} cartes nouvelles (la ligne « en » de §4.2 : Fallacies 5 + Virtues 2 + Scenarii 11)"
              f" · dossier §6 RAPPORTÉ : +{EXPECTED['B_delta_cartes']} → {'✔' if delta == EXPECTED['B_delta_cartes'] else '✘ ÉCART'}")
    if branch == "C":
        print(f"    dossier §6 RAPPORTÉ : {EXPECTED['C_cartes']} cartes → {'✔' if cartes == EXPECTED['C_cartes'] else '✘ ÉCART'}")
    return res


# ── emit (transformed COPIES — never in-place) ────────────────────────────────
def emit(root, branch, out_root):
    corp_root = os.path.abspath(root)
    out_root = os.path.abspath(out_root)
    if out_root == corp_root or out_root.startswith(corp_root + os.sep):
        sys.exit(f"REFUS: --to {out_root} est DANS le corpus {corp_root} — émission refusée.")
    total_cells = 0
    for key, rel, _id in CORPORA:
        corp = load(root, rel)
        scope = branch_scope(branch, key, corp["header"])
        new_rows = [corp["rows"][0]]
        touched = 0
        for row, term in corp["rows"][1:]:
            fields = split_fields(row)
            new_fields = list(fields)
            for (lang, col), idx in scope.items():
                if idx < len(fields):
                    nf, ch = convert_field(fields[idx], lang)
                    if ch:
                        new_fields[idx] = nf
                        touched += 1
            if new_fields != fields:
                row = ",".join(new_fields)
            new_rows.append((row, term))
        text = "".join(r + t for r, t in new_rows)
        data = text.encode("utf-8-sig" if corp["bom"] else "utf-8")
        dest = os.path.join(out_root, rel)
        os.makedirs(os.path.dirname(dest), exist_ok=True)
        fd, tmp = tempfile.mkstemp(dir=os.path.dirname(dest), prefix=".994tmp")
        with os.fdopen(fd, "wb") as fh:
            fh.write(data)
        os.replace(tmp, dest)  # atomic promote; target never partially written
        total_cells += touched
        print(f"  émis {rel}  ({touched} cellules, BOM={'oui' if corp['bom'] else 'non'})")
    print(f"Émission {branch} → {out_root} : {total_cells} cellules. Le corpus source est intact (aucun mode in-place).")


# ── self-test on a throwaway fixture ──────────────────────────────────────────
def self_test():
    failures = []

    def check(name, cond, detail=""):
        print(f"  [{'PASS' if cond else 'FAIL'}] {name}" + (f" — {detail}" if detail and not cond else ""))
        if not cond:
            failures.append(name)

    with tempfile.TemporaryDirectory(prefix="994-fixture-") as fx, \
         tempfile.TemporaryDirectory(prefix="994-fixture-out-") as outd:
        scen = os.path.join(fx, "scen.csv")
        header_s = "coordonnées,titre,contexte,link_fr,Remarques,title,context,issue,suggestion_en,title_ru"
        r1 = '"1,0101",l\'été,"le roi d\'Angleterre,\r\nil parle d\'or",https://fr.wiki#Types_d\'arguments,l\'ordre,don’t stay,ain’t,it’s d’or,d’Arc,plain'
        r2 = '"2,0102",«Eden»,' + "'liberté'" + ',l,x,y,z,plain,plain,plain'
        body_s = "\r\n".join([header_s, r1, r2]) + "\r\n"
        open(scen, "wb").write(body_s.encode("utf-8"))  # noBOM + CRLF, embedded newline in quoted cell

        fal = os.path.join(fx, "fal.csv")
        header_f = "PK,text_fr,desc_fr,example_fr,Famille,Sous-Famille,Soussousfamille,text_en,desc_en,example_en,Simple_name_en,example_en_bis,link_fr,Remarques"
        rf1 = '1,qu’il,d’nulle,«l’exemple»,Appel,Ad,Hominem,it’s,the baker’s,isn’t,don’t,can’t,https://x#Biais_d\'attribution,l’ordre'
        open(fal, "wb").write(b"\xef\xbb\xbf" + ("\r\n".join([header_f, rf1, "2,a,b,c,d,e,f,g,h,i,j,k,l,m"]) + "\r\n").encode("utf-8"))

        vir = os.path.join(fx, "vir.csv")
        header_v = "pk,title_fr,description_fr,remark_fr,AIF_criticalQuestion,title_en,description_en,remark_en,link_fr,KIDZ"
        # FR title/description/remark already curved (Virtues FR closure, §2);
        # AIF_criticalQuestion still STRAIGHT (the real residual, 139 occ).
        rv1 = '1,qu’est-ce,d’abord,l’oiseau,qu\'est-ce que c\'est,don’t,baker’s,it’s,https://y#Jeanne_d\'Arc,l\'oiseau'
        open(vir, "wb").write(b"\xef\xbb\xbf" + ("\r\n".join([header_v, rv1]) + "\r\n").encode("utf-8"))

        rules = os.path.join(fx, "rules.csv")
        open(rules, "wb").write(b"\xef\xbb\xbf" + ("\r\n".join([
            "pk,Text,Text_en,Text_ru,print_and_play",
            "1,d\'abord,don’t,д\'Арк,pp"]) + "\r\n").encode("utf-8"))
        rulespnp = os.path.join(fx, "rulespnp.csv")
        open(rulespnp, "wb").write(b"\xef\xbb\xbf" + ("\r\n".join([
            "pk,Text,Text_en,Text_ru,print_and_play",
            "1,d\'accord,don’t begin,д\'Арк,pp"]) + "\r\n").encode("utf-8"))

        # The analyse()/emit() plumbing addresses corpora by fixed repo-relative
        # paths — point the machinery at the fixture by monkey-mapping CORPORA.
        global CORPORA
        real_corpora = CORPORA
        CORPORA = [
            ("Scenarii", "scen.csv", "coordonnées"),
            ("Fallacies", "fal.csv", "PK"),
            ("Virtues", "vir.csv", "pk"),
            ("Rules", "rules.csv", "pk"),
            ("RulesPnP", "rulespnp.csv", "pk"),
        ]
        try:
            resA = analyse(fx, "A")
            h = resA["Scenarii"]["hits"]
            check("A touche titre FR (1 occ)", h.get(("fr", "titre"), [0])[0] == 1)
            check("A touche contexte FR (d'Angleterre + d'or = 2, guillemets CSV + newline embarqué)",
                  h.get(("fr", "contexte"), [0])[0] == 2, str(h))
            check("A ne touche PAS title EN courbe (en hors A)", ("en", "title") not in h)
            check("A touche Rules Text FR (d'abord)", resA["Rules"]["hits"].get(("fr", "Text"), [0])[0] == 1)
            check("A exclut link_fr (d'arguments dans l'URL)", resA["Scenarii"]["excluded"].get("fr", 0) >= 1)
            check("A exclut Remarques sans-suffixe (bac n/a)", resA["Scenarii"]["excluded"].get("n/a", 0) >= 1)
            check("A touche AIF_criticalQuestion (émis OWL, 2 occ)", resA["Virtues"]["hits"].get(("fr", "AIF_criticalQuestion"), [0])[0] == 2)
            check("A exclut KIDZ sans-suffixe (bac n/a)", resA["Virtues"]["excluded"].get("n/a", 0) >= 1)
            check("Fallacies FR dérive 0 (fermeture §2.1)", not resA["Fallacies"]["hits"])

            resB = analyse(fx, "B")
            hb = resB["Scenarii"]["hits"]
            check("B converse Rules Text_en don’t → droit", resB["Rules"]["hits"].get(("en", "Text_en"), [0])[0] == 1)
            check("B converse RulesPnP Text_en", resB["RulesPnP"]["hits"].get(("en", "Text_en"), [0])[0] == 1)
            check("B converse title EN (don’t stay)", hb.get(("en", "title"), [0])[0] == 1)
            check("B converse context EN (ain’t)", hb.get(("en", "context"), [0])[0] == 1)
            check("B converse issue EN (it’s + d’or = 2)", hb.get(("en", "issue"), [0])[0] == 2)
            check("B converse suggestion_en (d’Arc = 1)", hb.get(("en", "suggestion_en"), [0])[0] == 1)
            check("B ne touche pas Text_ru (ru hors B)", all(("ru", c) not in resB[k]["hits"] for k in resB for c in ["Text_ru", "title_ru"]))
            check("B exclut Simple_name_en (don’t, sans lecteur)", resB["Fallacies"]["excluded"].get("en", 0) >= 1)
            # example_en_bis ne finit pas par _en : invisible au décompte d'exclusions par
            # suffixe — sa preuve est l'assertion INTACT de l'émission ci-dessous.

            resC = analyse(fx, "C")
            check("C touche Rules Text_ru д’Арк → courbe", resC["Rules"]["hits"].get(("ru", "Text_ru"), [0])[0] == 1)

            outA = outd
            import contextlib, io as _io
            with contextlib.redirect_stdout(_io.StringIO()):
                emit(fx, "A", outA)
            new_scen = open(os.path.join(outA, "scen.csv"), "rb").read()
            old_scen = open(scen, "rb").read()
            check("émission Scenarii : toujours sans BOM", not new_scen.startswith(b"\xef\xbb\xbf"))
            check("émission Scenarii : CRLF identique à la source (terminaisons + newline embarqué)",
                  new_scen.count(b"\r\n") == old_scen.count(b"\r\n"),
                  f"{new_scen.count(b'\r\n')} vs {old_scen.count(b'\r\n')}")
            check("émission Scenarii : newline embarqué préservé", b",\r\nil parle" in new_scen)
            check("émission Scenarii : l’été courbé", "l’été".encode() in new_scen and "l'été".encode() not in new_scen)
            check("émission : link_fr INTACT (URL d’arguments)", b"#Types_d'arguments" in new_scen)
            check("émission : Remarques INTACT", b"l'ordre" in new_scen)
            check("émission : Text_en courbe INTACT en A", "don’t".encode() in new_scen)
            check("émission : guillemets simples 'liberté' INTACTS (hors périmètre §1)", "'liberté'".encode() in new_scen)
            new_fal = open(os.path.join(outA, "fal.csv"), "rb").read()
            check("émission Fallacies : BOM préservé", new_fal.startswith(b"\xef\xbb\xbf"))
            check("émission : Simple_name_en INTACT (courbe jamais redressée, sans lecteur)",
                  "don’t".encode() in new_fal)
            check("émission : example_en_bis INTACT", "can’t".encode() in new_fal)

            # round-trip : re-run detection on the emitted tree → residual 0 for A's targets
            resA2 = analyse(outA, "A")
            resid = sum(v[0] for k in resA2 for v in resA2[k]["hits"].values())
            check("aller-retour : plus aucun apostrophe FR fautif après émission A", resid == 0, str(resid))

            # D dérive vide
            resD = analyse(fx, "D")
            check("D : aucune hit", all(not resD[k]["hits"] for k in resD))

            # refus d'émission dans le corpus
            refused = False
            try:
                with contextlib.redirect_stdout(_io.StringIO()):
                    emit(fx, "A", os.path.join(fx, "sub"))
            except SystemExit:
                refused = True
            check("émission refusée DANS le corpus", refused)
        finally:
            CORPORA = real_corpora

    print(f"\n{'SELF-TEST OK' if not failures else 'SELF-TEST ÉCHOUÉ : ' + ', '.join(failures)}")
    return 0 if not failures else 1


# ── CLI ────────────────────────────────────────────────────────────────────────
def main():
    ap = argparse.ArgumentParser(description="#994 apostrophe dry-run (zéro écriture corpus)")
    ap.add_argument("--corpus", default=".", help="racine du dépôt contenant Cards/ (défaut .)")
    ap.add_argument("--branch", default=None, choices=list(BRANCH_LANGS), help="branche unique à rapporter")
    ap.add_argument("--worklist", default=None, choices=list(BRANCH_LANGS), help="worklist cellulaire TSV sur stdout")
    ap.add_argument("--emit", default=None, choices=list(BRANCH_LANGS), help="émettre des COPIES transformées")
    ap.add_argument("--to", default=None, help="répertoire de destination des copies (jamais dans le corpus)")
    ap.add_argument("--self-test", action="store_true", help="suite de tests sur fixture jetable")
    args = ap.parse_args()

    if args.self_test:
        sys.exit(self_test())

    if not os.path.isdir(args.corpus):
        sys.exit(f"corpus introuvable : {args.corpus}")

    if args.worklist:
        res = analyse(args.corpus, args.worklist)
        print("corpus\trowid\tlang\tcolonne\toccurrences")
        for key in res:
            for (lg, col), h in sorted(res[key]["hits"].items()):
                for rid in sorted(h[2]):
                    print(f"{key}\t{rid}\t{lg}\t{col}\t{h[0] if len(h[2]) == 1 else '≥1'}")
        return

    branches = [args.branch] if args.branch else ["A", "B", "C", "D"]
    for b in branches:
        report(args.corpus, b)

    if args.emit:
        if not args.to:
            sys.exit("--emit exige --to")
        print(f"\n════════ ÉMISSION {args.emit} ════════")
        emit(args.corpus, args.emit, args.to)
    print("\nCORPUS INTACT — ce script n'a aucun mode d'écriture in-place.")


if __name__ == "__main__":
    main()
