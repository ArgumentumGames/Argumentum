# -*- coding: utf-8 -*-
# #1471 lot ar (P24-11) — écriture byte-exact des cellules link_ar constatables de ce lot : 7 cellules.
# Pattern #1554/#2228/#2269/#2273 : splice par offsets absolus, machine à états CSV, BOM + CRLF préservés,
# re-parse complet, anti-overwrite, --write gated.
#
# Worklist re-mesurée le 2026-11-15 (jamais recopiée de l'issue) : deck 175, link_ar natif 110,
# 10 vides justifiés (#1435), worklist = 55 cellules. Trois gisements déterministes sondés :
#
#  1) fallaeasies.com/ar — sitemap 42 URLs dont 40 fiches ; fiche comparée aux libellés
#     text_en/text_ar/text_fr des 1408 nœuds : UNE carte du worklist correspond, 846
#     (ambiguity) — même fiche que le lot es et que le lot ru avant lui (précédent
#     inter-langues #2228). Toutes les autres correspondances sont des cartes déjà servies
#     ou des nœuds hors deck.
#
#  2) ar.wikipedia, titre exact — 55 sondes de libellé text_ar, 7 pages existantes.
#     Retenues après lecture de l'extrait : 614 (مغالطة الحادث, première phrase =
#     « المغالطة غير الصورية للحادث… تدمير الاستثناء », soit le Sophisme de l'accident
#     de la carte) et 1398 (شخصنة, « صنف شائع من المغالطات… بسبب معلومات متعلقة
#     بالكاتب », soit l'attaque personnelle). Recoupement : 511 (تواصل غير كلامي).
#     Rejets nommés : 357 التكييف -> تكييف الهواء = article CLIMATISATION (pas le
#     conditionnement psychologique) ; 845 خلط -> خلط (توضيح) = page de désambiguïsation ;
#     322 التشويه -> تشويه = mutilation physique (le « Foil » de la carte est une
#     distorsion d'argument).
#
#  3) interwiki inverse — TOUS les liens wikipedia déjà portés par les 55 cartes
#     (19 titres en, 17 fr, 16 zh, 15 es, 10 pt, 5 ru, 5 fa interrogés, lots de 50) :
#     5 cartes résolues vers un article ar du MÊME item Wikidata : 511 (en Nonverbal
#     communication -> تواصل غير كلامي), 953 (fr Cherry picking -> التقاطية), 1092
#     (es Sesgo de negatividad -> انحياز للسلبية), 1282 (fr Relativisme -> النسبية
#     (مذهب فلسفي), page qui distingue elle-même le concept philosophique du concept
#     physique), 1288 (fr Réfutabilité -> دحوضية).
#
# Garde « aucune collision ajoutée » (dispatch du 15/11) : les arrivées des candidats sont
# comparées aux 110 link_ar existants du deck. 953 التقاطية est déjà l'arrivée de 603
# (3.1.1.2.1 « Picorage de données ») et 953 (6.1.3) n'a AUCUNE parenté avec lui — le
# groupe serait un « autres » à juger : 953 EXCLU du lot, laissé à l'arbitrage. 1398
# شخصنة est l'arrivée de 1360 (7.3 « Ad hominem ») et 1362 (7.3.1.1 « Tu quoque ») :
# 1398 (7.3.3) est descendant de 1360 et frère de 1362, le groupe existant reste un bloc
# carte-ancetre accepté et AUCUN nouveau groupe n'apparaît (15/15 -> 15/15 sur --lang ar,
# groupe {1360,1362} -> {1360,1362,1398}) : 1398 GARDÉ.
#
# Union des trois gisements moins l'exclu = 7 cellules distinctes.
# DoD : HTTP 200 + lang="ar" CONSTATÉ dans le HTML servi pour les 7 URLs (7/7, 2026-11-15).
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
TARGETS = {
    511: "https://ar.wikipedia.org/wiki/%D8%AA%D9%88%D8%A7%D8%B5%D9%84_%D8%BA%D9%8A%D8%B1_%D9%83%D9%84%D8%A7%D9%85%D9%8A",
    614: "https://ar.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D8%A9_%D8%A7%D9%84%D8%AD%D8%A7%D8%AF%D8%AB",
    846: "https://www.fallaeasies.com/ar/fallacies/ambiguity",
    1092: "https://ar.wikipedia.org/wiki/%D8%A7%D9%86%D8%AD%D9%8A%D8%A7%D8%B2_%D9%84%D9%84%D8%B3%D9%84%D8%A8%D9%8A%D8%A9",
    1282: "https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D9%86%D8%B3%D8%A8%D9%8A%D8%A9_%28%D9%85%D8%B0%D9%87%D8%A8_%D9%81%D9%84%D8%B3%D9%81%D9%8A%29",
    1288: "https://ar.wikipedia.org/wiki/%D8%AF%D8%AD%D9%88%D8%B6%D9%8A%D8%A9",
    1398: "https://ar.wikipedia.org/wiki/%D8%B4%D8%AE%D8%B5%D9%86%D8%A9",
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
col = header.index("link_ar")
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
    assert cur == "", f"PK {pk} link_ar NON VIDE ({cur[:40]!r}) — anti-overwrite, worklist à re-mesurer"
    edits.append((a, b, TARGETS[pk], pk))
assert len(edits) == len(TARGETS), f"{len(edits)} edits != {len(TARGETS)} cibles"

just_rows = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:] if unquote(vs[pk_col]).strip().isdigit() and int(unquote(vs[pk_col]).strip()) in JUSTIFIED]
assert len(just_rows) == 10 and all((r.get("link_ar") or "") == "" for r in just_rows)

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
            assert k == "link_ar" and int(rb["PK"]) in TARGETS, f"champ inattendu changé PK {rb['PK']} {k}"
            assert ra[k] == TARGETS[int(rb["PK"])]
            diffs.append(int(rb["PK"]))
assert sorted(diffs) == sorted(TARGETS), "diff != cibles"

print(f"DRY-RUN OK — {len(TARGETS)} cellule link_ar : " + ", ".join(f"{pk}" for pk in sorted(TARGETS)))
if WRITE:
    tmp = CSV_PATH + ".tmp1471"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")
