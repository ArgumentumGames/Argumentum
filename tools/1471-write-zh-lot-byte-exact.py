# -*- coding: utf-8 -*-
# #1471 lot zh (P24-13) — écriture byte-exact du lot zh : 8 remplissages + 1 re-cible.
# RÉVISION du 16/11 (arbitrage ai-01, c.6925175950) : les 2 vidages initiaux (973, 2) sont
# ANNULÉS — les cellules et le pin de FallaciesLanguageLinkParityTests restent tels que sur
# master. Le lot final = master + 8 remplissages + 1 re-cible = 9 cellules link_zh.
# Pattern #1554/#2228/#2269/#2273/#2283 : splice par offsets absolus, machine à états CSV,
# BOM + CRLF préservés, re-parse complet, anti-overwrite, --write gated.
#
# Worklist re-mesurée le 2026-11-16 sur a8df0843 (jamais recopiée de l'issue) : deck 175,
# link_zh wikipedia remplis 120, 10 vides justifiés (#1435), worklist = 45 cellules.
# Trois gisements déterministes sondés (.tmp/zh-probe1.json, zh-probe2.json — non commités) :
#
#  1) fallaeasies.com/zh — sitemap racine (le /zh/sitemap.xml est 404) : 40 fiches sous
#     /zh/fallacies/, comparées aux libellés text_en/text_fr/text_zh des 1408 nœuds :
#     UNE seule carte de la worklist correspond, 846 (ambiguity) — même fiche que les lots
#     es/pt/ar avant lui. L'article natif 歧義 existant sur zh.wikipedia est retenu à la
#     place de la fiche (l'article natif prime quand il existe ; la fiche servait de repli
#     pour ar, qui n'a pas d'article correspondant).
#  2) zh.wikipedia, titre exact — 45 libellés text_zh sondés : 3 existantes, toutes les
#     trois via redirection, lues puis tranchées :
#     ACCEPTÉES : 846 歧义→歧義 (extrait : « 未定義或定義不清楚而沒有明確涵義 » — c'est
#     l'ambiguïté de la carte 5.3) ; 421 个人魅力→魅力 (article du concept charisme — la
#     carte 2.3.2.1 en est l'usage rhétorique, même classe que 511 admis au lot ar) ;
#     777 不一致→一致性 (邏輯) (concept inverse, comme 1288 postulat infalsifiable→
#     réfutabilité au lot ar).
#  3) interwiki inverse — tous les liens wikipedia déjà portés par les 45 cartes interrogés
#     (langlinks zh, lots de 40) : 6 cartes résolues vers l'article zh du MÊME item :
#     511 非言语交际 (×5 langues), 953 單方論證 (fr Cherry picking), 1092 負面偏誤 (×3),
#     1282 相對主義 (×4), 1288 可证伪性 (×4), 1398 訴諸人身 (ar شخصنة).
#
# Garde « aucune collision ajoutée » (décision du 15/11, arrivals résolus par redirection,
# leçon du lot es) : les 8 arrivées comparées aux 120 link_zh existants du deck.
# 7 arrivées sont NOUVELVES. 1398 訴諸人身 est l'arrivée de 1360 (7.3 « Ad hominem ») et
# 1362 (7.3.1.1.1 « Tu quoque ») : 1398 (7.3.3) est descendant de 1360, le groupe
# {1360,1362,1398} reste un bloc carte-ancêtre accepté — PRÉCÉDENT EXACT du lot ar
# (1398 شخصنة gardé, c.6903455484). 953 單方論證 est l'arrivée de 603 (3.1.1.2.1
# « Picorage de données ») et 953 (6.1.3) n'a AUCUNE parenté avec lui : EXCLU du lot,
# laissé à l'arbitrage — même jugement que le lot ar (953 التقاطية exclu).
#
# Groupes zh de la liste #2282 jugés dans ce lot (docs/translation/1471-classes-collisions-
# 2026-11-15.md §3) — aucune cellule de ces groupes n'est dans la worklist, ce sont des
# corrections séparées :
#  - Groupe 19 (323/844 → 關聯謬誤) : 844 « Sophisme d'association » est exact ; 323 « Appel
#    au mépris » (fr Argumentum_ad_odium) pointait sur l'article d'association, hors concept.
#    Article exact trouvé et lu : 訴諸仇恨 (« appeal to spite, appeal to hatred, argumentum
#    ad odium — 訴諸情感謬誤 »). RE-CIBLÉ, aucune nouvelle collision (0 porteur avant).
#  - Groupe 27 (809/973, zh seul → 歧義謬誤) : les deux cellules redirigent vers l'article
#    générique 歧義謬誤. 809 « Contraste perdu » (5.1.2.2.1, branche ambiguïté) : 打壓對立
#    est le terme zh du suppressed correlative, famille correcte — GARDÉ. 973 « Changement
#    de cap » : VIDÉ au premier jet (挪動門柱 atterrit sur 歧義謬誤, aucun article dédié
#    mesuré), puis GARDÉ sur arbitrage ai-01 (c.6925175950, 16/11) : la redirection
#    porte un fragment (歧義謬誤#挪動門柱), l'article a une section « 挪動門柱 », le lien zh
#    de Moving the goalposts (le link_en du nœud) est précisément 挪動門柱, et le corpus
#    accepte déjà les liens vers une section (précédent 1362 #Tu_quoque). Cellule et pin
#    laissés tels que sur master.
#  - Groupe 28 (2/595/598 → 草率歸納) : {595,598} paire carte-ancêtre acceptée (3.1 ⊃
#    3.1.1.1.1), inchangée. 2 « Argument bâclé » : VIDÉ au premier jet (reste d'ancien
#    titre pré-#1522), puis GARDÉ sur arbitrage ai-01 (c.6925175950, 16/11) : la
#    description reste la généralisation hâtive, le changement de titre #1522 n'a pas
#    changé le concept. Cellule laissée telle que sur master ; le signalement
#    link_en/link_fr est CLOS par ai-01.
#  - Groupes 3 (182/814 → 假兩難推理) et 26 (43/98 → 訴諸群眾) : GARDÉS TELS QUELS.
#    182 « Fausse alternative » et 814 « Faux dilemme » sont deux mécanismes de la famille
#    faux-dilemne (descs lus), la colonne fr fait le même choix (les deux → Faux_dilemme) ;
#    43 et 98 sont deux variantes du même sophisme ad populum (les DEUX link_en =
#    Argumentum_ad_populum) — doublon accepté par la décision du 15/11.
#
# Union = 8 remplissages + 1 re-cible = 9 rangées, 9 cellules link_zh (état final révisé).
# DoD : HTTP 200 + lang=\"zh\" CONSTATÉ dans le HTML servi pour les 9 URLs écrites
# (9/9, 2026-11-16, avant commit).
# ⛔ 10 vides justifiés (#1435) non touchés : PK 1,33,658,681,690,798,826,876,1004,1281.
import csv, io, os, sys

CSV_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Cards", "Fallacies",
                        "Argumentum Fallacies - Taxonomy.csv")
FILLS = {
    421: "https://zh.wikipedia.org/wiki/%E9%AD%85%E5%8A%9B",
    511: "https://zh.wikipedia.org/wiki/%E9%9D%9E%E8%A8%80%E8%AF%AD%E4%BA%A4%E9%99%85",
    777: "https://zh.wikipedia.org/wiki/%E4%B8%80%E8%87%B4%E6%80%A7_%28%E9%82%8F%E8%BC%AF%29",
    846: "https://zh.wikipedia.org/wiki/%E6%AD%A7%E7%BE%A9",
    1092: "https://zh.wikipedia.org/wiki/%E8%B2%A0%E9%9D%A2%E5%81%8F%E8%AA%A4",
    1282: "https://zh.wikipedia.org/wiki/%E7%9B%B8%E5%B0%8D%E4%B8%BB%E7%BE%A9",
    1288: "https://zh.wikipedia.org/wiki/%E5%8F%AF%E8%AF%81%E4%BC%AA%E6%80%A7",
    1398: "https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E4%BA%BA%E8%BA%AB",
}
# PK -> (valeur actuelle attendue byte-exact, nouvelle valeur)
RETARGET = {
    323: ("https://zh.wikipedia.org/wiki/%E9%97%9C%E8%81%AF%E8%AC%AC%E8%AA%A4",
          "https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E4%BB%87%E6%81%A8"),
}
# Vidages ANNULÉS le 16/11 (arbitrage ai-01 c.6925175950) : 973 et 2 restent tels que
# sur master — ils ne sont ni remplis, ni re-ciblés, ni vidés. La garde ci-dessous exige
# juste qu'ils soient INTACTS (non vides) après écriture, pour prêter main-forte au pin.
KEPT = {
    973: "https://zh.wikipedia.org/wiki/%E6%8C%AA%E5%8B%95%E9%96%80%E6%9F%B1",
    2: "https://zh.wikipedia.org/wiki/%E8%8D%89%E7%8E%87%E6%AD%B8%E7%B4%8D",
}
VOIDS = {}
JUSTIFIED = {1, 33, 658, 681, 690, 798, 826, 876, 1004, 1281}
WRITE = "--write" in sys.argv
ALL = dict(FILLS)
ALL.update({pk: new for pk, (old, new) in RETARGET.items()})
ALL.update({pk: "" for pk in VOIDS})

raw = open(CSV_PATH, "rb").read()
bom = b"\xef\xbb\xbf"
assert raw.startswith(bom), "BOM absent"
text = raw[len(bom):].decode("utf-8")
crlf_before = text.count("\r\n")
assert crlf_before > 0 and "\r\n" not in ALL.values()

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
col = header.index("link_zh")
pk_col = header.index("PK")
assert header[pk_col] == "PK"

def unquote(v):
    if len(v) >= 2 and v[0] == '"' and v[-1] == '"':
        return v[1:-1].replace('""', '"')
    return v

rows_before = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:]]
assert len(rows_before) == 1408, f"attendu 1408 lignes, lu {len(rows_before)}"

# garde de cohérence du groupe 1398 : la nouvelle URL est byte-identique à celle de 1360
r1360 = next(r for r in rows_before if r["PK"] == "1360")
assert r1360["link_zh"] == FILLS[1398], "1398 doit rejoindre byte-identique l'URL de 1360"

edits = []
for rec_start, vs, fields in recs[1:]:
    pk_raw = vs[pk_col]
    try:
        pk = int(pk_raw)
    except ValueError:
        continue
    if pk not in ALL:
        continue
    deck = (dict(zip(header, vs)).get("carte") or "").strip()
    assert deck, f"PK {pk} hors deck ?!"
    a, b = fields[col]
    cur = text[a:b]
    if pk in FILLS:
        assert cur == "", f"PK {pk} link_zh NON VIDE ({cur[:40]!r}) — anti-overwrite, worklist à re-mesurer"
    elif pk in RETARGET:
        old, _ = RETARGET[pk]
        assert cur == old, f"PK {pk} link_zh != attendu ({cur[:60]!r} != {old[:60]!r})"
    else:
        assert cur == VOIDS[pk], f"PK {pk} link_zh != attendu pour vidage ({cur[:60]!r})"
    edits.append((a, b, ALL[pk], pk))
assert len(edits) == len(ALL) == 9, f"{len(edits)} edits != 9 cibles"

just_rows = [dict(zip(header, [unquote(v) for v in vs])) for _, vs, _ in recs[1:]
             if unquote(vs[pk_col]).strip().isdigit() and int(unquote(vs[pk_col]).strip()) in JUSTIFIED]
assert len(just_rows) == 10 and all((r.get("link_zh") or "") == "" for r in just_rows)

new_text = text
for a, b, url, pk in sorted(edits, key=lambda e: -e[0]):
    new_text = new_text[:a] + url + new_text[b:]

assert new_text.count("\r\n") == crlf_before, "CRLF altéré"
delta = (sum(len(u) for u in FILLS.values())
         + sum(len(new) - len(old) for old, new in RETARGET.values()))
assert len(new_text) - len(text) == delta, "longueur inattendue"

rows_after = list(csv.DictReader(io.StringIO(new_text)))
assert len(rows_after) == 1408
# Arbitrage ai-01 (c.6925175950) : 973 et 2 ne doivent PAS bouger — cellules = master.
for kept_pk, kept_url in KEPT.items():
    r = next(r for r in rows_after if r["PK"] == str(kept_pk))
    assert r["link_zh"] == kept_url, f"PK {kept_pk} ne doit pas bouger ({r['link_zh'][:50]!r})"
diffs = []
for rb, ra in zip(rows_before, rows_after):
    assert rb["PK"] == ra["PK"]
    for k in header:
        if rb[k] != ra[k]:
            assert k == "link_zh" and int(rb["PK"]) in ALL, f"champ inattendu changé PK {rb['PK']} {k}"
            assert ra[k] == ALL[int(rb["PK"])]
            diffs.append(int(rb["PK"]))
assert sorted(diffs) == sorted(ALL), "diff != cibles"

filled = sum(1 for r in rows_after if (r.get("carte") or "").strip() and (r.get("link_zh") or "").strip())
print(f"DRY-RUN OK — 9 cellules link_zh : 8 remplies ({', '.join(str(p) for p in sorted(FILLS))}) + "
      f"1 re-ciblée (323) ; 973 et 2 intacts (arbitrage ai-01 c.6925175950)")
print(f"link_zh wikipedia sur deck : 120 -> {filled}")
if WRITE:
    tmp = CSV_PATH + ".tmp1471zh"
    with open(tmp, "wb") as f:
        f.write(bom + new_text.encode("utf-8"))
    os.replace(tmp, CSV_PATH)
    print("ÉCRIT (os.replace atomique).")
else:
    print("Pas d'écriture — passer --write.")
