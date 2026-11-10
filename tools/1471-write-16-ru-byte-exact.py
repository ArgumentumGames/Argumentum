# -*- coding: utf-8 -*-
# #1471 lot ru (1) — écriture byte-exact de cellules link_ru vides (16 proposées, 14 retenues : voir révision ai-01).
# Pattern #1554 : splice par offsets absolus, machine à états CSV, BOM + CRLF préservés,
# re-parse complet, anti-overwrite, --write gated.
#
# Adjudication 2026-11-10 (fiches fallaeasies.com/ru/fallacies/, 40 fiches sondées,
# TOUTES HTTP 200 + lang="ru" constatés dans le HTML servi) :
#   16 retenues sur 40 fiches — les autres couvrent des cartes dont link_ru est déjà rempli
#   (passe #1487 : ad hominem, straw man, appel à l'autorité...) ou aucun répondant du deck.
#
#   Référents : titre + première phrase de la fiche lus et confrontés au concept de la carte.
#   Règle du titre (#2225, appliquée mécaniquement) : le titre ru de la fiche ne doit pas être
#   le libellé (text_ru) exact d'une AUTRE carte du deck. Mesuré : 0 collision. Les 3 titres
#   qui égalent un libellé sont ceux de LEUR cible (598, 846, 768) — confirmation de référent.
#   - 677 Скользкий склон (pente glissante) · 598 Поспешное обобщение (général. hâtive)
#   - 299 Аргумент к эмоциям · 697 Ложная причина (causalité douteuse : post hoc)
#   - 846 Двусмысленность · 953 Выборочное использование фактов (précédent fr #2224)
#   - 768 Ложная эквивалентность · 989 Переложение бремени доказательства
#   - 974 Передвижение ворот (précédent fr #2225 : la fiche va à 974, pas à 973/1011)
#   - 977 Ошибка нирваны · 758 Не следствие · 1371 Генетическая ошибка
#   - 844 Вина по ассоциации · 112 Моралистическая ошибка (précédent fr #2225)
#   - 622 Ошибка композиции · 625 Ошибка деления
#   Écartés pour collision de référent avec une AUTRE carte (mêmes motifs que la révision
#   ai-01 du lot fr 2) : PK 2 et 759 (fiche hasty-generalization appartient à 598, libellé
#   exact), PK 726 (composition fautive ≠ composition), PK 973/1011 (la paire buts mobiles
#   va à 974, cf. révision ai-01 #2225).
#
# Révision ai-01 (2026-11-10 soir, avant merge) : 14 cellules sur 16 — règle du titre étendue
#   à TOUS les nœuds de l'arbre (pas seulement le deck), en russe ET en anglais (le slug est anglais) :
#   - 844 retirée : le titre « Вина по ассоциации » est le libellé ru exact du nœud 1372
#     « Culpabilité par association » (7.3.2.1, sous Sophisme génétique) ; 844 « Sophisme
#     d'association » (5.2.3) est un autre concept (association indue de caractéristiques).
#   - 953 retirée : « cherry-picking » = libellé anglais exact de la carte 603 « Picorage de
#     données » (deck), qui porte déjà ru.wikipedia Cherry_picking.
#   - 974 → 973 : « moving-the-goalposts » = libellé anglais exact de la carte 973 « Changement
#     de cap » (6.2), parent de 974 (6.2.1) et 1011 (6.2.3). Le précédent fr #2225 était faux
#     sur ce point ; corrigé dans la même PR (974 link_fr vidée).
#   Les jumeaux hors deck (705, 843, 1350, 761) portent le même libellé que leur cible-carte :
#   la carte garde la fiche.
# ⛔ Ne pas ré-ajouter 844, 953, 974.
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
BASE = "https://www.fallaeasies.com/ru/fallacies/"
TARGETS = {
    112:  BASE + "moralistic-fallacy",
    299:  BASE + "appeal-to-emotion",
    598:  BASE + "hasty-generalization",
    622:  BASE + "composition-fallacy",
    625:  BASE + "division-fallacy",
    677:  BASE + "slippery-slope",
    697:  BASE + "false-cause",
    758:  BASE + "non-sequitur",
    768:  BASE + "false-equivalence",
    846:  BASE + "ambiguity",
    973:  BASE + "moving-the-goalposts",
    977:  BASE + "nirvana-fallacy",
    989:  BASE + "burden-of-proof",
    1371: BASE + "genetic-fallacy",
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

print(f"DRY-RUN OK — {len(TARGETS)} cellules link_ru : " + ", ".join(f"{pk}->{'…' + u[-24:]}" for pk, u in sorted(TARGETS.items())))
if WRITE:
    tmp = CSV_PATH + ".tmp1471"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")
