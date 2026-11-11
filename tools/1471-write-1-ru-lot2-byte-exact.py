# -*- coding: utf-8 -*-
# #1471 lot ru (2) — écriture byte-exact de LA cellule link_ru constatable de ce lot : PK 511.
# Pattern #1554/#2228 : splice par offsets absolus, machine à états CSV, BOM + CRLF préservés,
# re-parse complet, anti-overwrite, --write gated.
#
# Adjudication 2026-11-11 — sondage intégral des gisements déterministes ru sur le worklist
# re-mesuré (81 cellules deck vides hors justifiés ; le lot 1 annonçait ~79, écart dit dans le
# commentaire #1471) :
#   - fallaeasies.com/ru : épuisé par le lot 1 (#2228, 14 écrites post-révision ai-01).
#   - yourlogicalfallacyis.com/ru et thedecisionlab.com/ru : HTTP 404 — n'existent pas.
#   - ru.wikipedia recherche «libellé exact» (81 sondes) : 6 titres-exacts, TOUS hors-sujet
#     constaté au snippet (300 droit pénal · 845 chimie du mercure · 1280 médical · 511/432/1174
#     parapluies génériques du concept de la carte) — aucun retenu.
#   - ru.wikipedia interwiki inverse (131 candidats des recherches + 288 de la passe libre) :
#     exactement 1 match — PK 511 : l'article ru «Невербальное общение» a pour interwiki
#     en:'Nonverbal communication' / fr:'Communication non verbale' ; or la carte 511 porte
#     DÉJÀ link_fr = fr.wikipedia.org/wiki/Communication_non_verbale. Le référent est donc
#     confirmé mécaniquement : l'article ru est l'équivalent interwiki du lien fr de SA carte.
#   - ru.wiktionary (81 sondes) : 12 «titres-exacts» de search sont des fantômes d'index
#     (matchs de contenu) ; les 4 pages inspectées rendent missingtitle — gisement vide.
#
# Règle du titre (#2225, étendue par révision ai-01 #2228 à TOUT l'arbre, ru ET en) :
#   «Невербальное общение» = libellé text_ru du SEUL nœud 511 (vérifié sur les 1408) ;
#   'Nonverbal communication' = libellé text_en du seul nœud 511. 0 collision.
# DoD : HTTP 200 (174 Ko) + lang="ru" CONSTATÉ dans le HTML servi + référent interwiki exact.
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
TARGETS = {
    511: "https://ru.wikipedia.org/wiki/%D0%9D%D0%B5%D0%B2%D0%B5%D1%80%D0%B1%D0%B0%D0%BB%D1%8C%D0%BD%D0%BE%D0%B5_%D0%BE%D0%B1%D1%89%D0%B5%D0%BD%D0%B8%D0%B5",
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
col = header.index("link_ru")
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
    assert cur == "", f"PK {pk} link_ru NON VIDE ({cur[:40]!r}) — anti-overwrite, worklist à re-mesurer"
    edits.append((a, b, TARGETS[pk], pk))
assert len(edits) == len(TARGETS), f"{len(edits)} edits != {len(TARGETS)} cibles"

just_rows = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:] if unquote(vs[pk_col]).strip().isdigit() and int(unquote(vs[pk_col]).strip()) in JUSTIFIED]
assert len(just_rows) == 10 and all((r.get("link_ru") or "") == "" for r in just_rows)

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
            assert k == "link_ru" and int(rb["PK"]) in TARGETS, f"champ inattendu changé PK {rb['PK']} {k}"
            assert ra[k] == TARGETS[int(rb["PK"])]
            diffs.append(int(rb["PK"]))
assert sorted(diffs) == sorted(TARGETS), "diff != cibles"

print(f"DRY-RUN OK — {len(TARGETS)} cellule link_ru : " + ", ".join(f"{pk}->{'…' + u[-24:]}" for pk, u in sorted(TARGETS.items())))
if WRITE:
    tmp = CSV_PATH + ".tmp1471"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")
