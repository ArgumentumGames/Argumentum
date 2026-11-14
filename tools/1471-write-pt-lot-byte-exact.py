# -*- coding: utf-8 -*-
# #1471 lot pt — écriture byte-exact des cellules link_pt constatables de ce lot : 12 cellules.
# Pattern #1554/#2228 : splice par offsets absolus, machine à états CSV, BOM + CRLF préservés,
# re-parse complet, anti-overwrite, --write gated.
#
# Worklist re-mesurée le 2026-11-14 (jamais recopiée de l'issue) : deck 175, link_pt natif 91,
# 10 vides justifiés (#1435), worklist = 74 cellules actionnables.
#
#  1) fallaeasies.com/pt — sitemap 559 URLs, 42 pt dont 40 fiches (2 hors fiche) ; 40/40
#     HTTP 200 + lang="pt" constaté dans le HTML servi. Les h1 des 40 fiches comparés aux 1408
#     libellés text_pt/text_en : 12 correspondances exactes, dont 3 sur la worklist —
#     598 (Generalização Apressada), 846 (Ambiguidade), 956 (Pleito Especial). Les 9 autres
#     (299, 104, 814, 1314, 1372, 4, 1395, 611, 843) ne sont pas dans la worklist (link_pt déjà
#     posé, ou hors deck).
#
#  2) pt.wikipedia, titre exact — 74 sondes de libellé, 8 pages existantes, 1 retenue :
#     511 (Comunicação não verbal, aussi par gisement 3). Rejets nommés :
#     322 Repulsão -> redirige vers « Repulsão elétrica » (physique, pas la fable) ;
#     598 -> « Generalização precipitada » (la fiche fallaeasies prime, même règle qu'en es) ;
#     846 -> déjà servi par la fiche ; 887 Trapaça, 1280 Obstrução, 1313 Evasão = pages de
#     désambiguïsation (pageprops.disambiguation constaté) ; 1398 « Ataque pessoal » ->
#     redirige vers « Argumentum ad hominem » (la carte est ad personam, sophisme distinct).
#
#  3) pt.wikipedia, interwiki inverse — TOUS les liens wikipedia déjà portés par les 74 cartes
#     (39 titres en, 31 fr, 25 es, 22 ar, 32 zh, 7 ru, 13 fa interrogés, redirects résolus) :
#     12 cartes résolues vers un article pt du MÊME item Wikidata (liens symétriques vérifiés
#     par la contre-requête pt->fr/en/es). Retenues après lecture de l'extrait :
#     511 (3 sources fr+es+ru), 758 (Non sequitur, fr+es), 833 (fr Fausse analogie -> redirige
#     vers Fausse équivalence -> pt « Falsa equivalência », dont l'extrait nomme « falsa
#     analogia » ; URL déjà portée par 768, doublon toléré — 6 autres URLs pt dupliquées
#     existent dans le deck), 847 (Anfibologia, fr+es), 953 (Evidência suprimida, fr+es ; son
#     extrait nomme « cherry picking »), 1024 et 1174 (es Novum organum, dont la section
#     « Os ídolos » couvre ídolos da tribo/caverna = Idola tribus/specus des cartes),
#     1282 (Relativismo), 1288 (Falseabilidade).
#     Rejets nommés : 595 « Generalização » = l'opération logique (199 chars, aucune section
#     sur le sophisme, alors que la carte est « Generalização abusiva ») ;
#     1297 Argumentum ad nauseam = sophisme distinct de « Prova por afirmação » (la carte),
#     et aucun article pt pour « Prova por afirmação » (sonde absente).
#
# Règle du titre (#2225) : 511, 598, 846, 956 = libellé du seul nœud (vérifié sur les 1408) ;
# 847 et 953 = homonymes de libellés hors worklist, référent confirmé par l'item du lien déjà
# porté ; 758, 833, 1024, 1174, 1282, 1288 = référent = item Wikidata du lien déjà porté par la
# carte (contrôle plus fort qu'un titre). DoD : HTTP 200 + lang="pt" CONSTATÉ dans le HTML
# servi pour les 12 URLs (12/12, 2026-11-14).
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
TARGETS = {
    511: "https://pt.wikipedia.org/wiki/Comunica%C3%A7%C3%A3o_n%C3%A3o_verbal",
    598: "https://www.fallaeasies.com/pt/fallacies/hasty-generalization",
    758: "https://pt.wikipedia.org/wiki/Non_sequitur",
    833: "https://pt.wikipedia.org/wiki/Falsa_equival%C3%AAncia",
    846: "https://www.fallaeasies.com/pt/fallacies/ambiguity",
    847: "https://pt.wikipedia.org/wiki/Anfibologia",
    953: "https://pt.wikipedia.org/wiki/Evid%C3%AAncia_suprimida",
    956: "https://www.fallaeasies.com/pt/fallacies/special-pleading",
    1024: "https://pt.wikipedia.org/wiki/Novum_Organum",
    1174: "https://pt.wikipedia.org/wiki/Novum_Organum",
    1282: "https://pt.wikipedia.org/wiki/Relativismo",
    1288: "https://pt.wikipedia.org/wiki/Falseabilidade",
}
JUSTIFIED = {1, 33, 658, 681, 690, 798, 826, 876, 1004, 1281}
WRITE = "--write" in sys.argv

raw = open(CSV_PATH, "rb").read()
bom = b"\xef\xbb\xbf"
assert raw.startswith(bom), "BOM absent"
text = raw[len(bom):].decode("utf-8")
crlf_before = text.count("\r\n")
assert crlf_before > 0 and "\r\n" not in TARGETS.values()


def records(text):
    """Yield (rec_start, values, fields) — offsets absolus, quote-aware."""
    i, n = 0, len(text)
    while i < n:
        start = i
        fields, fs, in_q = [], i, False
        while i < n:
            c = text[i]
            if in_q:
                if c == '"':
                    if i + 1 < n and text[i + 1] == '"':
                        i += 2
                    else:
                        in_q = False
                        i += 1
                else:
                    i += 1
            else:
                if c == '"':
                    in_q = True
                    i += 1
                elif c == ",":
                    fields.append((fs, i)); i += 1; fs = i
                elif c == "\r" and i + 1 < n and text[i + 1] == "\n":
                    fields.append((fs, i)); i += 2; break
                elif c == "\n":
                    fields.append((fs, i)); i += 1; break
                else:
                    i += 1
        else:
            fields.append((fs, n))
        values = [text[a:b] for a, b in fields]
        yield start, values, fields
        if i >= n and not values == [""]:
            break


recs = list(records(text))
header = [v for v in recs[0][1]]
col = header.index("link_pt")
pk_col = header.index("PK")
assert header[pk_col] == "PK"


def unquote(v):
    if len(v) >= 2 and v[0] == '"' and v[-1] == '"':
        return v[1:-1].replace('""', '"')
    return v


rows_before = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:]]
assert len(rows_before) == 1408, f"attendu 1408 lignes, lu {len(rows_before)}"

edits = []
for rec_start, vs, fields in recs[1:]:
    pk_raw = vs[pk_col]
    try:
        pk = int(pk_raw)
    except ValueError:
        continue
    if pk not in TARGETS:
        continue
    deck = (dict(zip(header, vs)).get("carte") or "").strip()
    assert deck, f"PK {pk} hors deck ?!"
    a, b = fields[col]
    cur = text[a:b]
    assert cur == "", f"PK {pk} link_pt NON VIDE ({cur[:40]!r}) — anti-overwrite, worklist à re-mesurer"
    edits.append((a, b, TARGETS[pk], pk))
assert len(edits) == len(TARGETS), f"{len(edits)} edits != {len(TARGETS)} cibles"

just_rows = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:]
             if unquote(vs[pk_col]).strip().isdigit() and int(unquote(vs[pk_col]).strip()) in JUSTIFIED]
assert len(just_rows) == 10 and all((r.get("link_pt") or "") == "" for r in just_rows)

new_text = text
for a, b, url, pk in sorted(edits, key=lambda e: -e[0]):
    new_text = new_text[:a] + url + new_text[b:]

assert new_text.count("\r\n") == crlf_before, "CRLF altéré"
assert len(new_text) - len(text) == sum(len(u) for u in TARGETS.values()), "longueur inattendue"

rows_after = list(csv.DictReader(io.StringIO(new_text)))
assert len(rows_after) == 1408
diffs = []
for rb, ra in zip(rows_before, rows_after):
    assert rb["PK"] == ra["PK"]
    for k in header:
        if rb[k] != ra[k]:
            assert k == "link_pt" and int(rb["PK"]) in TARGETS, f"champ inattendu changé PK {rb['PK']} {k}"
            assert ra[k] == TARGETS[int(rb["PK"])]
            diffs.append(int(rb["PK"]))
assert sorted(diffs) == sorted(TARGETS), "diff != cibles"

print(f"DRY-RUN OK — {len(TARGETS)} cellule link_pt : " + ", ".join(
    f"{pk}->{'…' + u[-24:]}" for pk, u in sorted(TARGETS.items())))
if WRITE:
    tmp = CSV_PATH + ".tmp1471"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")