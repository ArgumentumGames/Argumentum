# -*- coding: utf-8 -*-
# #1471 lot fr (sous-lot 2) — écriture byte-exact de cellules link_fr vides (5 proposées, 1 retenue : voir révisions ai-01).
# Pattern #1554 : splice par offsets absolus, machine à états CSV, BOM + CRLF préservés,
# re-parse complet, anti-overwrite, --write gated.
#
# Vérifiés le 2026-11-10 (instrument .tmp/1471-lot1/fetch-adjudicate.py, texte intégral constaté) :
#   7 fiches sondées, TOUTES HTTP 200 + lang="fr" + explication/exemple extraits.
#   Adjudication strate E : 112/759/834 retenus ; 974+1011 = PAIRE « Déplacer les buts »
#   (explication direction-agnostique « modifier les standards de preuve une fois atteints »,
#    exemple = monter la barre ; baisser la barre = même procédé vers le bas — paire nommée).
#   Rejets : 105 (miroir tradition vs précédent négatif), 726 (parties→tout ≠ propositions),
#            800 (équivoque = double sens ≠ évasivité).
#
# Révision ai-01 (2026-11-10, avant merge) : 2 cellules retenues sur 5 — 112 et 974.
#   Retirées, chacune pour collision ou miroir avec une AUTRE carte du corpus :
#   - 759 « Conclusion hâtive » : la fiche s'intitule « Généralisation hâtive » = libellé exact
#     de la carte 598 (deck) ; référent de 598, pas de 759 (link_en 759 = Jumping_to_conclusions).
#   - 834 « Comparaison abusive » : la fiche s'intitule « Fausse équivalence » = libellé exact
#     de la carte 768 (deck), qui porte déjà fr.wikipedia Fausse_équivalence.
#   - 1011 « Exigence relâchée » : la fiche définit « modifier les standards de preuve une fois
#     atteints POUR REJETER un argument » = relever la barre (974) ; abaisser est le miroir.
# Seconde révision ai-01 (2026-11-10 soir, #2228) : 974 retirée à son tour — 1 cellule sur 5 (112).
#   La fiche « moving-the-goalposts » porte le libellé anglais EXACT de la carte 973 « Changement de cap »
#   (path 6.2, text_en « Moving the goalposts », link_fr fr.wikipedia Changer_les_règles_du_jeu) :
#   974 (6.2.1) et 1011 (6.2.3) sont ses deux enfants (relever / abaisser), et une fiche
#   « direction-agnostique » est celle du parent. La première révision n'avait comparé que les
#   libellés français des cartes ; la règle compare désormais tous les nœuds, en langue visée ET en anglais.
# ⛔ Ne pas ré-ajouter ces 4 entrées : le script réécrirait des cellules vides à dessein.
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
TARGETS = {
    112:  "https://www.fallaeasies.com/fr/fallacies/moralistic-fallacy",
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

just_rows = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:] if unquote(vs[pk_col]).strip().isdigit() and int(unquote(vs[pk_col]).strip()) in JUSTIFIED]
assert len(just_rows) == 10 and all((r.get("link_fr") or "") == "" for r in just_rows)

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
