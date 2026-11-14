# -*- coding: utf-8 -*-
# #1471 lot es — écriture byte-exact des cellules link_es constatables de ce lot : 12 cellules.
# Pattern #1554/#2228 : splice par offsets absolus, machine à états CSV, BOM + CRLF préservés,
# re-parse complet, anti-overwrite, --write gated.
#
# Worklist re-mesurée le 2026-11-14 (jamais recopiée de l'issue) : deck 175, link_es natif 100,
# 10 vides justifiés (#1435), worklist = 65 cellules. Trois gisements déterministes sondés :
#
#  1) fallaeasies.com/es — sitemap 559 URLs, 42 es dont 40 fiches (2 hors fiche) ; 40/40
#     HTTP 200 + lang="es" constaté dans le HTML servi. Titre de fiche comparé aux 1408
#     libellés text_es/text_en : 3 nœuds du worklist, exactement ceux que le lot ru avait
#     déjà servis avec la MÊME fiche en /ru (précédent inter-langues, #2228) —
#     112 (moralistic-fallacy, aussi link_fr déjà posé), 598 (hasty-generalization),
#     846 (ambiguity). 953 (Atención selectiva) a un desc_es de cherry-picking mais part sur
#     l'article wikipédia du gisement 3, qui nomme « cherry picking » dans sa première phrase.
#
#  2) es.wikipedia, titre exact — 65 sondes de libellé, 16 pages existantes, 5 retenues après
#     lecture de l'extrait : 175 (Influencia), 511 (Comunicación no verbal), 713 (Dos errores
#     hacen un acierto, Catégorie:Falacias, extrait = desc_es), 847 (Anfibología,
#     Catégorie:Falacias informales), 1092 (Sesgo de negatividad). Rejets nommés :
#     845 Confusión, 1313 Evasión, 1280 Obstrucción = pages de désambiguación ;
#     953 Atención selectiva = article de psychologie (pas la falacia) ;
#     598 et 846 = titre exact mais déjà servis par la fiche fallaeasies (gisement 1).
#
#  3) es.wikipedia, interwiki inverse — TOUS les liens wikipedia déjà portés par les 65 cartes
#     (28 titres en, 20 fr, 19 ar, 26 zh, 11 pt, 4 ru, 11 fa interrogés) : 7 cartes résolues
#     vers un article es du MÊME item Wikidata (liens symétriques vérifiés). 4 apports propres :
#     313 (fa:پاچه‌خاری -> es Adulación, dont l'extrait dit « La adulación o lisonja… », soit le
#     libellé text_es de la carte), 953 (fr:Cherry picking -> Falacia de evidencia incompleta),
#     1282 (fr:Relativisme -> Relativismo), 1288 (fr:Réfutabilité -> Falsabilidad). 511, 847 et
#     1092 recoupent le gisement 2. Union des trois gisements = 12 cellules distinctes.
#
# Règle du titre (#2225) : 847 et 1092 = libellé d'un seul nœud (vérifié sur les 1408) ; 713 a
# un homonyme hors deck (1325) ; 511 et 175 sont uniques. Pour 313/953/1282/1288 le référent
# n'est pas un titre mais l'item Wikidata du lien déjà porté par la carte — contrôle plus fort.
# DoD : HTTP 200 + lang="es" CONSTATÉ dans le HTML servi pour les 12 URLs (12/12, 2026-11-14).
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
TARGETS = {
    112: "https://www.fallaeasies.com/es/fallacies/moralistic-fallacy",
    175: "https://es.wikipedia.org/wiki/Influencia",
    313: "https://es.wikipedia.org/wiki/Adulaci%C3%B3n",
    511: "https://es.wikipedia.org/wiki/Comunicaci%C3%B3n_no_verbal",
    598: "https://www.fallaeasies.com/es/fallacies/hasty-generalization",
    713: "https://es.wikipedia.org/wiki/Dos_errores_hacen_un_acierto",
    846: "https://www.fallaeasies.com/es/fallacies/ambiguity",
    847: "https://es.wikipedia.org/wiki/Anfibolog%C3%ADa",
    953: "https://es.wikipedia.org/wiki/Falacia_de_evidencia_incompleta",
    1092: "https://es.wikipedia.org/wiki/Sesgo_de_negatividad",
    1282: "https://es.wikipedia.org/wiki/Relativismo",
    1288: "https://es.wikipedia.org/wiki/Falsabilidad",
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
col = header.index("link_es")
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
    assert cur == "", f"PK {pk} link_es NON VIDE ({cur[:40]!r}) — anti-overwrite, worklist à re-mesurer"
    edits.append((a, b, TARGETS[pk], pk))
assert len(edits) == len(TARGETS), f"{len(edits)} edits != {len(TARGETS)} cibles"

just_rows = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:] if unquote(vs[pk_col]).strip().isdigit() and int(unquote(vs[pk_col]).strip()) in JUSTIFIED]
assert len(just_rows) == 10 and all((r.get("link_es") or "") == "" for r in just_rows)

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
            assert k == "link_es" and int(rb["PK"]) in TARGETS, f"champ inattendu changé PK {rb['PK']} {k}"
            assert ra[k] == TARGETS[int(rb["PK"])]
            diffs.append(int(rb["PK"]))
assert sorted(diffs) == sorted(TARGETS), "diff != cibles"

print(f"DRY-RUN OK — {len(TARGETS)} cellule link_es : " + ", ".join(f"{pk}->{'…' + u[-24:]}" for pk, u in sorted(TARGETS.items())))
if WRITE:
    tmp = CSV_PATH + ".tmp1471"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")