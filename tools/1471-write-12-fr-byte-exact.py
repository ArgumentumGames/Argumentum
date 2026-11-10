# -*- coding: utf-8 -*-
# #1471 lot fr (sous-lot 1) — écriture byte-exact de 12 cellules link_fr vides.
# Pattern #1554 : splice par offsets absolus (des cellules vides ne sont pas uniques),
# machine à états CSV pour les spans de champs (quotes + LF embarqués sûrs),
# BOM + CRLF préservés, re-parse complet, anti-overwrite, --write gated.
#
# Candidats vérifiés le 2026-11-10 (instrument .tmp/1471-lot1/verify-urls.py + curl ×2 stacks) :
#   HTTP 200 + langue constatée dans le contenu + référent constaté (titre/extrait/mots-clés).
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = r"D:\Dev\Argumentum\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv"
TARGETS = {
    299:  "https://yourlogicalfallacyis.com/fr/appel-a-l-emotion",
    633:  "https://fr.wikipedia.org/wiki/Corr%C3%A9lation_illusoire",
    953:  "https://fr.wikipedia.org/wiki/Cherry_picking",
    1092: "https://fr.wikipedia.org/wiki/Biais_de_n%C3%A9gativit%C3%A9",
    1174: "https://fr.wikipedia.org/wiki/Biais_culturel",
    1282: "https://fr.wikipedia.org/wiki/Relativisme",
    1288: "https://fr.wikipedia.org/wiki/R%C3%A9futabilit%C3%A9",
    1297: "https://fr.wikipedia.org/wiki/Argumentum_ad_nauseam",
    153:  "https://yourlogicalfallacyis.com/fr/l-erreur-sur-l-erreur",
    956:  "https://yourlogicalfallacyis.com/fr/deux-poids-deux-mesures",
    1361: "https://yourlogicalfallacyis.com/fr/tu-quoque",
    653:  "https://thedecisionlab.com/fr/biases/hot-hand-fallacy",
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
    """Yield (rec_start, [(field_start, field_end, value), ...]) — offsets absolus, quote-aware."""
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
header = [v for _, vs, _ in [r] for v in vs] if False else [v for v in recs[0][1]]
col = header.index("link_fr")
pk_col = header.index("PK")
assert header[pk_col] == "PK"

def unquote(v):
    if len(v) >= 2 and v[0] == '"' and v[-1] == '"':
        return v[1:-1].replace('""', '"')
    return v

rows_before = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:]]
assert len(rows_before) == 1408, f"attendu 1408 lignes, lu {len(rows_before)}"

edits = []  # (abs_start, abs_end, url, pk)
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
    assert cur == "", f"PK {pk} link_fr NON VIDE ({cur[:40]!r}) — anti-overwrite, worklist à re-mesurer"
    edits.append((a, b, TARGETS[pk], pk))
assert len(edits) == len(TARGETS), f"{len(edits)} edits != {len(TARGETS)} cibles"

# garde : les 10 justifiés restent vides et intacts
just_rows = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:] if unquote(vs[pk_col]).strip().isdigit() and int(unquote(vs[pk_col]).strip()) in JUSTIFIED]
assert len(just_rows) == 10 and all((r.get("link_fr") or "") == "" for r in just_rows)

# splice offsets décroissants
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
            assert k == "link_fr" and int(rb["PK"]) in TARGETS, f"champ inattendu changé PK {rb['PK']} {k}"
            assert ra[k] == TARGETS[int(rb["PK"])]
            diffs.append(int(rb["PK"]))
assert sorted(diffs) == sorted(TARGETS), "diff != cibles"

print(f"DRY-RUN OK — {len(TARGETS)} cellules link_fr : " + ", ".join(f"{pk}->{'…' + u[-28:]}" for pk, u in sorted(TARGETS.items())))
if WRITE:
    tmp = CSV_PATH + ".tmp1471"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")
