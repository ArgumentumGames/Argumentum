# -*- coding: utf-8 -*-
"""Série (20)-(25) -- fidélité des DÉFINITIONS du deck : instrument de mesure, 0 écriture.

POURQUOI CE DOCUMENT EXISTE
---------------------------
Les TITRES ont eu leurs passes de fidélité (grains (8)-(17), 7 langues). Les DÉFINITIONS
imprimées (`desc_<lang>`) n'ont jamais eu la leur. Cet instrument prépare la lecture
rangée par rangée : il apparie `desc_fr` et `desc_<lang>`, attache la référence imprimée du
dépôt, et lève des drapeaux MÉCANIQUES.

⛔ CE QUE L'INSTRUMENT N'EST PAS. Les drapeaux sont des PRIORITÉS DE LECTURE, pas des
verdicts. Mesuré sur le deck ru (grain (20)) : 26 drapeaux, 0 défaut réel. Sur ce corpus,
les écrans mécaniques ne savent pas voir un contresens -- c'est la lecture humaine qui
porte le verdict, et l'instrument ne fait que garantir que chaque rangée est présentée.

⛔ LA JOINTURE IMPRIMÉE N'EST PAS `path` SEUL. `path` apparie 153/175 et fait passer les
22 orphelines pour des « cartes nouvelles ». C'est FAUX : la taxonomie a été restructurée
entre l'archive v3 et la baseline 2024 (34 lignes d'archive changent de `path` à nom
constant), et la couverture réelle est 168/175. L'erreur a été publiée le 22/09 puis
corrigée -- voir docs/corpus/archive-coverage-2026-09-22.md. Cet instrument réutilise donc
le pont du dépôt (`archive-bridge-instrument.py`) : archive --(nom)--> baseline 2024
--(PK)--> HEAD.

⛔ MÊME `path` N'EST PAS UN ARBITRE SANS LE NOM (grain 3, 03/10). Un `path` qui tombe sur
une ligne d'archive ne vaut rattachement CONFIRMÉ que si l'archive porte le même nom --
directement, ou via le nom de la même PK dans la baseline 2024. Sinon : pont, et à défaut
le rattachement est gardé mais marqué POSITION SEULE (compté à part, contenu non affiché :
un DIFFERE lu contre la mauvaise carte est le défaut, pas la preuve). Mesure ai-01 du
03/10 : le triplet path 1.1.1-1.1.3 (PK 3/33/55, trois sœurs permutées depuis l'archive
v3) était rattaché aux MAUVAISES cartes -- PK 55 sortait DIFFERE contre « Argument vide ».

USAGE
-----
    python docs/corpus/definitions-fidelity-instrument.py --lang ru --out /tmp/ru.txt
    python docs/corpus/definitions-fidelity-instrument.py --self-test
    python docs/corpus/definitions-fidelity-instrument.py --lang ru --csv <copie> --out ...

Le corpus du dépôt n'est JAMAIS écrit : le contrôle inverse travaille sur une copie.
"""
import argparse
import csv
import importlib.util
import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.abspath(os.path.join(HERE, "..", ".."))
MAIN = os.path.join(ROOT, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
BRIDGE_PY = os.path.join(HERE, "archive-bridge-instrument.py")

# Marqueurs de négation, PAR LANGUE. L'écran n'est pas neutre entre les langues : appliquer
# le jeu russe à du chinois ne lève rien (aucun `\bне\b` dans « 不 »), donc CHAQUE rangée dont
# le français nie sort en POLARITY -- 43 faux positifs mesurés sur `zh`. Un écran de
# polarité n'a de sens que dans la langue qu'il lit.
#
# ⚠️ Même dans sa propre langue, `\bне\b` ne voit pas la négation PRÉFIXÉE russe
# (недостаточны, необоснованными, неприменим, невозможность...) : il mesure l'absence d'une
# PARTICULE, pas d'une NÉGATION. Mesuré sur (20) : 21/21 drapeaux POLARITY faux positifs.
#
# ⚠️ Arabe (grain ㉒, 03/10) : le jeu SANS frontières de mot matchait des SOUS-CHAÎNES
# -- ما à l'intérieur d'أمام (« devant ») a levé POLARITY sur la cellule du contrôle
# inverse, par accident. Frontières posées + لن (négation du futur, présente sur 12
# cellules) ajouté : « négation détectée » passe de 130/175 à 78/175, POLARITY de 95
# à 45. Limite qui RESTE : ما homophone (négation vs pronom indéfini « سلوكًا ما »)
# est indécidable par frontières -- l'écran reste une priorité de lecture, pas un verdict.
#
# ⚠️ Farsi (grain ㉓, 03/10) : même défaut de sous-chaînes (نه à l'intérieur de خانه,
# 42 occurrences) -- frontières posées : négation « détectée » 64/175 → 28/175,
# POLARITY 57 → 29. Le ZWNJ (U+200C, non-\w) CRÉE la frontière de \bنمی\b : les 8
# نمی du corpus sont toutes suivies de ZWNJ (mesuré, 0 attachée) -- une forme
# attachée sans ZWNJ (نمیخواهم) serait ratée, limite nommée.
NEG_MARKERS = {
    "fr": r"(?:\bne\b|\bn'|\bpas\b|\bjamais\b|\baucun|\bsans\b|\brien\b|\bnon\b)",
    "en": r"(?:\bnot\b|\bno\b|\bnever\b|\bnone\b|\bwithout\b|\bnothing\b|\bnor\b)",
    "ru": r"(?:\bне\b|\bни\b|\bникогда\b|\bбез\b|\bнет\b|\bничего\b|\bни один)",
    "pt": r"(?:\bnão\b|\bnem\b|\bnunca\b|\bsem\b|\bnada\b|\bnenhum)",
    "es": r"(?:\bno\b|\bnunca\b|\bsin\b|\bnada\b|\bning[uú]n)",
    # Écritures sans séparateur de mot : recherche de sous-chaîne, pas de `\b`.
    "zh": r"(?:不|没|無|无|非|未|別|别)",
    "ar": r"(?:\bلا\b|\bلن\b|\bما\b|\bليس(?:ت)?\b|\bغير\b|\bبدون\b|\bدون\b)",
    "fa": r"(?:\bنه\b|\bنیست(?:یم)?\b|\bبدون\b|\bهیچ\b|\bنمی\b)",
}
NEG_FR = re.compile(NEG_MARKERS["fr"], re.I)


def neg_hits(lang, text):
    """None quand la langue n'a pas de jeu configuré -- à NOMMER, pas à feindre."""
    pat = NEG_MARKERS.get(lang)
    if pat is None:
        return None
    return bool(re.search(pat, text or "", re.I))


def read_csv_guarded(path, required):
    """Garde d'en-tête : une colonne absente rend `r.get(col)` -> None, qui se lit comme
    « contenu absent ». On échoue bruyamment plutôt que de compter des zéros trompeurs."""
    with open(path, encoding="utf-8-sig", newline="") as f:
        hdr = [h.strip() for h in next(csv.reader(f))]
    missing = [c for c in required if c not in hdr]
    if missing:
        raise SystemExit("HEADER GUARD: %s missing %s" % (path, missing))
    with open(path, encoding="utf-8-sig", newline="") as f:
        return list(csv.DictReader(f))


def load_bridge():
    spec = importlib.util.spec_from_file_location("archbridge", BRIDGE_PY)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)      # main() ne tourne que sous __main__
    return mod


def nums(s):
    return set(re.findall(r"\d+", s or ""))


def is_cjk_dominant(s):
    """Un ratio de LONGUEUR EN CARACTÈRES n'est pas comparable entre écritures.

    Mesuré sur le deck (desc_zh contre desc_fr) : ratio 0,146-0,417, médiane 0,255 --
    le chinois dit la même chose en ~4 fois moins de caractères. Les bornes calibrées
    pour le russe (0,60-1,65) lèvent donc 175/175 drapeaux SHORT sur `zh` : un écran qui
    crie toujours ne trie rien. Aucune borne ne rattrape ça -- en dessous de 0,146 il ne
    reste rien à lever, au-dessus de 0,417 non plus. On le DIT au lieu de le feindre.
    """
    s = s or ""
    if not s:
        return False
    cjk = sum(1 for c in s if u"一" <= c <= u"鿿" or u"぀" <= c <= u"ヿ")
    return cjk / len(s) > 0.3


def is_arabic_dominant(s):
    """Même famille, autre écriture : l'arabe est une ABIJAD -- les voyelles brèves
    ne s'écrivent pas, les clitiques s'agglutinent (و، ال، بـ). Ratio mesuré
    desc_ar/desc_fr sur le deck : médiane 0,618, p10 0,505, min 0,389 -- la moitié
    du corpus saine tombe sous la borne SHORT(0,60) calibrée pour le russe : 76/175
    drapeaux sur des cellules saines, un écran qui hurle ne trie rien. (Le farsi ㉓
    s'écrit dans le même bloc avec des lettres supplémentaires : même écran.)
    Mesure du 03/10, grain ㉒."""
    s = s or ""
    if not s:
        return False
    ar = sum(1 for c in s if u"؀" <= c <= u"ۿ")
    return ar / len(s) > 0.3


def segments(s):
    """Découpe en phrases. ⚠️ La ponctuation CJK (。！？；) n'est pas ASCII : sans elle,
    une phrase chinoise compte pour 1 et l'écran CLAUSES devient muet par construction."""
    s = (s or "").strip()
    if not s:
        return 0
    return len([x for x in re.split(r"[.;:!?。！？；：]\s*|\n", s) if x.strip()])


def calibrate_script(pairs):
    """Etalonne un ecran de CONTENU sur la langue elle-meme, au lieu de la declarer NA.

    Un ratio brut contre le francais ne veut rien dire en chinois (0,146-0,417 mesure)
    ni en arabe (0,389-1,015, médiane 0,618 -- abjad : voyelles brèves non écrites,
    mesure grain ㉒). Mais la COMPRESSION est stable dans les deux écritures :
    `len ~ a * fr_len + b` explique la moitié de la variance en chinois (R2=0,498)
    et les deux tiers en arabe (R2=0,681, mesure sur les 175 cartes). Une cellule qui
    s'ecarte du residu attendu est donc un VRAI signal, calibre sur l'ecriture
    mesuree et non sur une autre. Au seuil de 2,5 ecarts-types : 1 carte levee sur le
    corpus reel zh (PK 112, compression reelle), 2 sur ar ; controle inverse --
    troncations a 50 % attrapees : 104/175 (zh), 137/175 (ar) ; a 35 % : 167/175 (zh),
    168/175 (ar). Declarer l'ecran NA laissait 0 % de pouvoir de detection.
    """
    n = len(pairs)
    if n < 30:
        return None
    mx = sum(p[0] for p in pairs) / n
    my = sum(p[1] for p in pairs) / n
    sxx = sum((p[0] - mx) ** 2 for p in pairs)
    if not sxx:
        return None
    a = sum((p[0] - mx) * (p[1] - my) for p in pairs) / sxx
    b = my - a * mx
    res = [p[1] - (a * p[0] + b) for p in pairs]
    sd = (sum(x * x for x in res) / (n - 1)) ** 0.5
    if not sd:
        return None
    return a, b, sd


def flags_for(fr, tg, lang="ru", script_cal=None):
    """Drapeaux mécaniques, bornes calibrées sur ce corpus (phrases uniques, 48-187 car.).
    Chacun est une PRIORITÉ DE LECTURE, jamais un verdict. Les notices (LEN-NA, POL-NA)
    disent qu'un écran n'a PAS tourné -- elles ne comptent pas comme des priorités."""
    out = []
    if not (tg or "").strip():
        out.append("EMPTY-target")
        return out
    if not (fr or "").strip():
        out.append("EMPTY-source")
    lf, lt = len((fr or "").strip()), len((tg or "").strip())
    if lf:
        if is_cjk_dominant(tg) or is_arabic_dominant(tg):
            if script_cal is None:
                # Nommé, pas silencieux : le lecteur doit savoir que l'écran de longueur
                # n'a PAS tourné sur cette rangée, et pourquoi.
                out.append("LEN-NA(CJK/AR)")
            else:
                a, b, sd = script_cal
                z = (lt - (a * lf + b)) / sd
                if abs(z) > 2.5:
                    out.append("LENDEV(%+.1f)" % z)
        else:
            ratio = lt / lf
            if ratio < 0.60:
                out.append("SHORT(%.2f)" % ratio)
            elif ratio > 1.65:
                out.append("LONG(%.2f)" % ratio)
    only = nums(fr) - nums(tg)
    if only:
        out.append("NUM-only-FR:" + ",".join(sorted(only)[:4]))
    pt = neg_hits(lang, tg)
    if pt is None:
        out.append("POL-NA(%s)" % lang)      # pas de jeu de marqueurs : on le dit
    elif bool(NEG_FR.search(fr)) != pt:
        out.append("POLARITY")
    sf, st = segments(fr), segments(tg)
    if sf - st >= 2:
        out.append("CLAUSES(%d<%d)" % (st, sf))
    if sf and st > sf + 2:
        out.append("CLAUSES(%d>%d)" % (st, sf))
    return out


def archive_headers(bridge):
    """L'en-tete de CHAQUE archive. ⛔ Sans lui, `arow.get("desc_zh")` rend None sur une
    archive qui n'a PAS la colonne, et l'instrument l'affiche comme « cellule vide ».
    C'est le piege nomme dans la memoire du depot (garde d'en-tete) : un None se lit
    comme une absence de contenu alors qu'il peut signifier une absence de COLONNE.
    Mesure du 03/10 sur `zh` : les archives ne portent que desc_fr/en/ru/pt -- le
    chinois n'y existe pas, donc 168/168 rendaient « <pas de desc_zh> » et le dossier
    aurait pu conclure « l'imprime n'a jamais eu de definition chinoise » a partir d'un
    artefact d'instrument."""
    out = {}
    for tag, path in bridge.ARCHIVES:
        with open(path, encoding="utf-8-sig", newline="") as f:
            out[tag] = [h.strip() for h in next(csv.reader(f))]
    return out


def build(deck, bridge, lang):
    arch = [(t, r) for t, p in bridge.ARCHIVES for r in bridge.rows(p)]
    hdrs = archive_headers(bridge)
    desc_col = "desc_%s" % lang
    col_present = {t: (desc_col in hdrs.get(t, [])) for t in hdrs}
    by_path, by_name = {}, {}
    for t, r in arch:
        p = bridge.n(r.get("path"))
        if p:
            by_path.setdefault(p, (t, r))
        k = bridge.key(r.get("text_fr"))
        if k:
            by_name.setdefault(k, []).append((t, r))
    base_by_pk = {bridge.n(r.get("PK")): r
                  for r in bridge.rows(bridge.DECK, bridge.BASELINE_REF)
                  if bridge.n(r.get("PK"))}

    def confirmed_by_name(arow, drow, base):
        """Un rattachement par `path` ne vaut CONFIRMÉ que si l'archive porte le
        même nom -- directement, ou via le nom de la même PK dans la baseline
        2024 (la carte a pu être re-titrée entre la baseline et HEAD).
        ⛔ Mesure ai-01 du 03/10 (pool c.5964435596) : sur 153 rattachements par
        `path`, 3 pointaient la MAUVAISE carte -- le triplet path 1.1.1 à 1.1.3
        (PK 3/33/55), dont les trois sœurs ont permuté leurs positions depuis
        l'archive v3. `path` est aveugle aux permutations de sœurs : il attache
        une POSITION, pas une carte."""
        ka = bridge.key(arow.get("text_fr"))
        if not ka:
            return False
        if ka == bridge.key(drow.get("text_fr")):
            return True
        return base is not None and ka == bridge.key(base.get("text_fr"))

    def reference_for(drow):
        """(etage, tag, ligne, colonne-presente, confirmee-par-nom) -- 1 = `path`,
        2 = le pont. None si aucune archive. Le 4e champ distingue une archive
        SANS la colonne d'une archive dont la cellule est vide : sans lui, les
        deux se rendent pareil. Le 5e : un `path` NON confirmé cède la place au
        pont ; si le pont échoue, il est gardé mais marqué POSITION SEULE -- une
        position sans nom n'est pas un arbitre imprimé."""
        p = bridge.n(drow.get("path"))
        base = base_by_pk.get(bridge.n(drow.get("PK")))
        hit = by_path.get(p) if p else None
        if hit:
            t, r = hit
            if confirmed_by_name(r, drow, base):
                return 1, t, r, col_present.get(t, False), True
        if base:
            hits = by_name.get(bridge.key(base.get("text_fr"))) or []
            if hits:
                return 2, hits[0][0], hits[0][1], col_present.get(hits[0][0], False), True
        if hit:
            t, r = hit
            return 1, t, r, col_present.get(t, False), False
        return None, None, None, False, False

    return reference_for


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--lang", default="ru")
    ap.add_argument("--out")
    ap.add_argument("--csv", default=None, help="copie de travail (contrôle inverse)")
    ap.add_argument("--self-test", action="store_true")
    a = ap.parse_args()
    lang = a.lang

    rows = read_csv_guarded(a.csv or MAIN,
                            ["path", "carte", "text_fr", "text_%s" % lang,
                             "desc_fr", "desc_%s" % lang])
    deck = [r for r in rows if (r.get("carte") or "").strip()]
    if len(deck) != 175:
        raise SystemExit("ATTENDU 175 cartes imprimees, mesure %d -- instrument suspect"
                         % len(deck))

    if a.self_test:
        # CONTRÔLE INVERSE. Une cellule FAUSSE plantée doit être visible. Les écrans
        # mécaniques seuls ne la voient pas (mesuré) : le contrôle établit donc (a) que
        # l'instrument ne casse pas sur une cellule anormale, et (b) que la lecture
        # humaine est la charge utile -- il ne prétend pas détecter le contresens.
        lens = sorted(len((r.get("desc_fr") or "").strip()) for r in deck)
        med = lens[len(lens) // 2]
        victim = dict(deck[min(range(len(deck)),
                               key=lambda i: abs(len(deck[i]["desc_fr"]) - med))])
        print("SELF-TEST -- calibré sur la distribution réelle (mediane desc_fr=%d)" % med)
        victim["desc_%s" % lang] = "Полная противоположность: правило всегда соблюдается."
        fa = flags_for(victim["desc_fr"], victim["desc_%s" % lang], lang)
        print("  (a) cellule tronquée -> %s" % fa)
        assert any(x.startswith("SHORT") for x in fa), "instrument AVEUGLE a une cellule tronquee"
        assert lang in NEG_MARKERS, "langue sans jeu de marqueurs : ecran POLARITY non testable"
        neg = [r for r in deck if NEG_FR.search(r.get("desc_fr") or "")]
        assert neg, "aucune carte a marqueur de negation FR -- ecran POLARITY non testable"
        v2 = dict(neg[0])
        stripped = re.sub(NEG_MARKERS[lang], "", v2["desc_%s" % lang] or "", flags=re.I)
        fb = flags_for(v2["desc_fr"], stripped, lang)
        print("  (b) polarité inversée -> %s" % fb)
        assert "POLARITY" in fb, "instrument AVEUGLE a un desaccord de polarite"

        # (c) Ecran de CONTENU par ECRITURE (zh : CJK ; ar/fa : abjad). La version
        # d'avant ㉑ declarait l'ecran NA (LEN-NA(CJK)) -- pouvoir de detection NUL
        # par construction ; ㉒ etend l'etalonnage a l'abjad. On exige que l'ecran
        # attrape, pour CHAQUE ecriture, une cellule tronquee a 50 %, et ne crie
        # pas sur un temoin sain.
        for col in ("zh", "ar", "fa"):
            dom = is_cjk_dominant if col == "zh" else is_arabic_dominant
            cells = [r for r in deck if (r.get("desc_%s" % col) or "").strip()
                     and dom(r["desc_%s" % col])]
            if not cells:
                continue
            cal = calibrate_script([(len(r["desc_fr"].strip()),
                                     len(r["desc_%s" % col].strip()))
                                    for r in cells])
            assert cal, "calibration %s impossible -- ecran NON testable" % col
            vict = max(cells, key=lambda r: len(r["desc_%s" % col].strip()))
            full = vict["desc_%s" % col].strip()
            tronq = full[: max(1, len(full) // 2)]
            fc = flags_for(vict["desc_fr"], tronq, col, cal)
            print("  (c) cellule %s tronquee a 50%% (PK=%s) -> %s"
                  % (col, vict.get("PK"), fc))
            assert any(x.startswith("LENDEV") for x in fc), \
                "ecran %s AVEUGLE a une cellule tronquee" % col
            # Temoin negatif : une cellule SAINE ne doit pas lever LENDEV.
            sane = [r for r in cells if len(r["desc_%s" % col].strip()) > 30][0]
            fz = flags_for(sane["desc_fr"], sane["desc_%s" % col].strip(), col, cal)
            assert not any(x.startswith("LENDEV") for x in fz), \
                "ecran %s crie sur une cellule saine -- borne trop serree" % col
            print("  (c') temoin sain %s (PK=%s) -> aucun LENDEV" % (col, sane.get("PK")))

        # (d) JOINTURE PAR NOM. `path` seul attache une POSITION, pas une carte :
        # les trois soeurs du triplet path 1.1.1-1.1.3 (PK 3/33/55) ont permute
        # leurs positions depuis l'archive v3, et l'instrument d'avant-réparation
        # rattachait chacune a la MAUVAISE (mesure ai-01 03/10, pool c.5964435596).
        # Exigeance : chacune doit desormais etre attachee CONFIRMEE PAR LE NOM.
        bridge = load_bridge()
        ref = build(deck, bridge, lang)
        tri = [r for r in deck if (r.get("path") or "").strip() in
               ("1.1.1", "1.1.2", "1.1.3")]
        assert len(tri) == 3, \
            "triplet 1.1.1-1.1.3 introuvable -- controle de jointure non testable"
        for r in sorted(tri, key=lambda x: x.get("PK", "")):
            tier, tag, arow, has_col, ok_nom = ref(r)
            assert tier is not None, "PK %s : plus aucun rattachement" % r.get("PK")
            assert ok_nom, ("PK %s rattachee SANS confirmation de nom (archive « %s » "
                            "vs deck « %s »)" % (r.get("PK"),
                                                 (arow.get("text_fr") or "").strip(),
                                                 (r.get("text_fr") or "").strip()))
            print("  (d) PK %-3s %-28s -> etage%d(%s) CONFIRME sur « %s »"
                  % (r.get("PK"), (r.get("text_fr") or "").strip()[:28], tier, tag,
                     (arow.get("text_fr") or "").strip()))
        # (d') PK 55 : c'est la carte qui a DEMONTRE le defaut -- lue contre
        # « Argument vide » par la jointure path, elle sortait DIFFERE. Le pont doit
        # la rattacher a SA carte archive, et l'imprime russe doit etre IDENTIQUE.
        r55 = [r for r in deck if r.get("PK") == "55"]
        assert len(r55) == 1, "PK 55 introuvable -- controle de jointure non testable"
        t55, g55, a55, hc55, ok55 = ref(r55[0])
        assert t55 == 2 and ok55, \
            "PK 55 doit venir du PONT, confirmee par nom (etage=%s)" % t55
        pl55 = (a55.get("desc_ru") or "").strip()
        assert pl55 and pl55 == (r55[0].get("desc_ru") or "").strip(), \
            "PK 55 : l'imprime russe du pont n'est pas IDENTIQUE -- jointure suspecte"
        print("  (d') PK 55 -> pont, archive « %s », desc_ru IDENTIQUE (%d car.)"
              % ((a55.get("text_fr") or "").strip(), len(pl55)))
        print("  PASS. ⚠️ Ce sont des priorités de lecture : le contrôle prouve que")
        print("  l'instrument n'est pas structurellement aveugle, pas qu'il juge.")
        return

    if not a.out:
        raise SystemExit("--out requis hors --self-test")

    bridge = load_bridge()
    reference_for = build(deck, bridge, lang)
    n_e1nom = n_e1pos = n_pont = 0
    n_flag = n_notice = 0
    usable = 0
    col_missing = set()
    script_cal = calibrate_script([(len((r.get("desc_fr") or "").strip()),
                                   len((r.get("desc_%s" % lang) or "").strip()))
                                  for r in deck if (r.get("desc_%s" % lang) or "").strip()
                                  and (is_cjk_dominant(r.get("desc_%s" % lang) or "")
                                        or is_arabic_dominant(r.get("desc_%s" % lang) or ""))])
    if script_cal:
        print("CALIBRATION script: pente=%.3f ordonnee=%.1f ecart-type=%.1f car."
              % (script_cal[0], script_cal[1], script_cal[2]))
    with open(a.out, "w", encoding="utf-8", newline="\n") as out:
        for i, r in enumerate(deck, 1):
            fr = (r.get("desc_fr") or "").strip()
            tg = (r.get("desc_%s" % lang) or "").strip()
            f = flags_for(fr, tg, lang, script_cal)
            prio = [x for x in f if not x.startswith(("LEN-NA", "POL-NA"))]
            if prio:
                n_flag += 1
            elif f:
                n_notice += 1
            tier, tag, arow, has_col, ok_nom = reference_for(r)
            if tier == 1:
                if ok_nom:
                    n_e1nom += 1
                else:
                    n_e1pos += 1
            elif tier == 2:
                n_pont += 1
            # EXPLOITABLE ne compte que les rattachements CONFIRMÉS PAR LE NOM
            # (étage1-nom + pont) dont l'archive porte la colonne. Une POSITION
            # SEULE n'est pas un arbitre imprimé : la compter ici contredit la
            # ligne du dessus (ru : 157, pas 168 — c.5965769047).
            if tier and ok_nom:
                if has_col:
                    usable += 1
                else:
                    col_missing.add(tag)
            out.write("### [%03d] PK=%s path=%s\n" % (i, r.get("PK"), (r.get("path") or "").strip()))
            out.write("TITRE fr: %s\n" % (r.get("text_fr") or "").strip())
            out.write("TITRE %s: %s\n" % (lang, (r.get("text_%s" % lang) or "").strip()))
            out.write("FR: %s\n" % fr)
            out.write("%s: %s\n" % (lang.upper(), tg))
            if arow is not None:
                via = ("etage1-nom" if tier == 1 and ok_nom else
                       "etage1-pos" if tier == 1 else "pont")
                if tier == 1 and not ok_nom:
                    # ⚠ POSITION SEULE : le `path` a attaché une POSITION, pas une
                    # carte. Le contenu de l'archive n'est volontairement PAS affiché :
                    # un DIFFERE lu contre la mauvaise carte est exactement le défaut
                    # que cette réparation ferme (PK 55 lue contre « Argument vide »,
                    # mesure ai-01 03/10, pool c.5964435596).
                    out.write("IMPRIME[%s](%s): ⚠ POSITION SEULE -- l'archive %s porte "
                              "« %s » a ce path, le deck y lit « %s », et le pont n'a "
                              "rien trouve sous ce nom : PAS un arbitre imprime\n"
                              % (via, tag, tag,
                                 (arow.get("text_fr") or "").strip(),
                                 (r.get("text_fr") or "").strip()))
                elif not has_col:
                    # ⛔ DISTINCT de « cellule vide ». L'archive n'a pas la colonne :
                    # aucun arbitrage imprime n'est possible pour cette langue.
                    out.write("IMPRIME[%s](%s): <archive SANS colonne desc_%s -- "
                              "aucun arbitrage imprime>\n" % (via, tag, lang))
                else:
                    pl = (arow.get("desc_%s" % lang) or "").strip()
                    if pl:
                        out.write("IMPRIME[%s](%s) [%s]: %s\n"
                                  % (via, tag, "IDENTIQUE" if pl == tg else "DIFFERE", pl))
                    else:
                        out.write("IMPRIME[%s](%s): <cellule desc_%s vide>\n"
                                  % (via, tag, lang))
            else:
                out.write("IMPRIME: <aucune reference, sous aucun nom>\n")
            out.write("FLAGS: %s\n\n" % (", ".join(f) if f else "-"))
        out.write("---\nDENOMINATEUR: %d cartes lues, %d affichees\n" % (len(deck), len(deck)))
        out.write("FLAGS mecaniques: %d/%d lignes portent au moins une PRIORITE de lecture "
                  "(+ %d lignes ne portent qu'une NOTICE : ecran non applicable)\n"
                  % (n_flag, len(deck), n_notice))
        n_att = n_e1nom + n_e1pos + n_pont
        out.write("REFERENCE IMPRIMEE: %d/%d rattachees -- CONFIRMEES PAR LE NOM %d "
                  "(etage1-nom %d + pont %d) + POSITION SEULE %d (le `path` a attache "
                  "une position, l'archive porte un AUTRE nom : pas un arbitre imprime) "
                  "+ aucune %d -- cf. docs/corpus/archive-coverage-2026-09-22.md\n"
                  % (n_att, len(deck), n_e1nom + n_pont, n_e1nom, n_pont, n_e1pos,
                     len(deck) - n_att))
        out.write("REFERENCE EXPLOITABLE: %d/%d -- %s\n"
                  % (usable, len(deck),
                     "colonne desc_%s presente dans toutes les archives" % lang
                     if not col_missing else
                     "⛔ AUCUN arbitrage imprime : desc_%s ABSENTE des archives %s"
                     % (lang, sorted(col_missing))))
    print("written %s: %d cards, %d flagged, reference %d/%d "
          "(confirmees par le nom %d, position seule %d)"
          % (a.out, len(deck), n_flag, n_att, len(deck), n_e1nom + n_pont, n_e1pos))


if __name__ == "__main__":
    main()
