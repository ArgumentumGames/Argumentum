# -*- coding: utf-8 -*-
# #1471 correctif lot pt — retour de contre-revue ai-01 du 15/11 (c.6886533628, tête 35fb93c5).
# 3 vidages + 2 ancres, byte-exact (pattern #1554/#2228 : splice par offsets absolus,
# machine à états CSV, BOM + CRLF préservés, re-parse complet, --write gated).
#
#  1) VIDÉES (l'article servi est celui d'une AUTRE carte, après redirection) :
#     - 833 link_pt (Falsa_equivalência = libellé/article de 768 « Fausse équivalence » ;
#       son propre lien fr Fausse_analogie redirige aussi vers l'article de 768 — défaut
#       antérieur, la carte garde en+fr) ;
#     - 953 link_pt (Evidência_suprimida = article aussi servi par le lien pt de 603
#       « Cata de cerejas », pt:Cherry_picking -> redirection) ;
#     - 953 link_es (Falacia_de_evidencia_incompleta = article aussi servi par le lien es
#       de 603, es:Cherry_picking -> redirection ; décision de coordination ai-01 du 15/11,
#       révisable par l'owner ; le fr n'est PAS touché : fr:Cherry_picking (953) et
#       fr:Cueillette_de_cerises (603) sont deux articles distincts).
#     Lot : 12 cellules -> 10 cellules.
#  2) ANCRES (suggestion non bloquante de la même contre-revue, mesuré ce jour) :
#     - 1024 link_pt + #Os_%C3%ADdolos_da_tribo  (link_en = Idola_tribus) ;
#     - 1174 link_pt + #Os_%C3%ADdolos_da_caverna (link_en = Idola_specus).
#     Sections constatées dans l'API de parse pt (2026-11-15) : « Os ídolos da tribo » /
#     « Os ídolos da caverna », niveau 2 sous « Os ídolos » — le portugais n'a pas
#     d'article séparé par idole, l'ancre donne à chaque carte son passage.
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
# (pk, colonne) -> (valeur attendue AVANT, valeur APRÈS)
EDITS = {
    (833, "link_pt"): ("https://pt.wikipedia.org/wiki/Falsa_equival%C3%AAncia", ""),
    (953, "link_pt"): ("https://pt.wikipedia.org/wiki/Evid%C3%AAncia_suprimida", ""),
    (953, "link_es"): ("https://es.wikipedia.org/wiki/Falacia_de_evidencia_incompleta", ""),
    (1024, "link_pt"): ("https://pt.wikipedia.org/wiki/Novum_Organum",
                        "https://pt.wikipedia.org/wiki/Novum_Organum#Os_%C3%ADdolos_da_tribo"),
    (1174, "link_pt"): ("https://pt.wikipedia.org/wiki/Novum_Organum",
                        "https://pt.wikipedia.org/wiki/Novum_Organum#Os_%C3%ADdolos_da_caverna"),
}
JUSTIFIED = {1, 33, 658, 681, 690, 798, 826, 876, 1004, 1281}
WRITE = "--write" in sys.argv

raw = open(CSV_PATH, "rb").read()
bom = b"\xef\xbb\xbf"
assert raw.startswith(bom), "BOM absent"
text = raw[len(bom):].decode("utf-8")
crlf_before = text.count("\r\n")
assert crlf_before > 0


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
    pk_raw = unquote(vs[pk_col]).strip()
    if not pk_raw.isdigit() or int(pk_raw) not in {pk for pk, _ in EDITS}:
        continue
    pk = int(pk_raw)
    for col, (old, new) in ((c, v) for (p, c), v in EDITS.items() if p == pk):
        cc = header.index(col)
        a, b = fields[cc]
        cur = text[a:b]
        assert cur == old, f"PK {pk} {col} : attendu {old!r}, lu {cur[:60]!r} — cible décalée, tout stopper"
        edits.append((a, b, new, pk, col))
assert len(edits) == len(EDITS), f"{len(edits)} edits != {len(EDITS)} cibles"

just_rows = [r for r in rows_before if r['PK'].strip().isdigit() and int(r['PK'].strip()) in JUSTIFIED]
assert len(just_rows) == 10

new_text = text
for a, b, new, pk, col in sorted(edits, key=lambda e: -e[0]):
    new_text = new_text[:a] + new + new_text[b:]

assert new_text.count("\r\n") == crlf_before, "CRLF altéré"
assert len(new_text) - len(text) == sum(len(n) - len(o) for o, n in EDITS.values()), "longueur inattendue"

rows_after = list(csv.DictReader(io.StringIO(new_text)))
assert len(rows_after) == 1408
diffs = []
for rb, ra in zip(rows_before, rows_after):
    assert rb["PK"] == ra["PK"]
    for k in header:
        if rb[k] != ra[k]:
            key = (int(rb["PK"]), k)
            assert key in EDITS, f"champ inattendu changé PK {rb['PK']} {k}"
            assert ra[k] == EDITS[key][1], f"PK {rb['PK']} {k} : valeur après != attendue"
            diffs.append(key)
assert sorted(diffs) == sorted(EDITS), "diff != cibles"

print(f"DRY-RUN OK — {len(EDITS)} cellules :")
for (pk, col), (old, new) in sorted(EDITS.items()):
    tail = new[-44:] if new else "(vide)"
    print(f"  {pk} {col}: {old[-40:]} -> {tail}")
if WRITE:
    tmp = CSV_PATH + ".tmp1471fix"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")
