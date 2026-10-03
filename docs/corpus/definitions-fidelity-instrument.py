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
NEG_MARKERS = {
    "fr": r"(?:\bne\b|\bn'|\bpas\b|\bjamais\b|\baucun|\bsans\b|\brien\b|\bnon\b)",
    "en": r"(?:\bnot\b|\bno\b|\bnever\b|\bnone\b|\bwithout\b|\bnothing\b|\bnor\b)",
    "ru": r"(?:\bне\b|\bни\b|\bникогда\b|\bбез\b|\bнет\b|\bничего\b|\bни один)",
    "pt": r"(?:\bnão\b|\bnem\b|\bnunca\b|\bsem\b|\bnada\b|\bnenhum)",
    "es": r"(?:\bno\b|\bnunca\b|\bsin\b|\bnada\b|\bning[uú]n)",
    # Écritures sans séparateur de mot : recherche de sous-chaîne, pas de `\b`.
    "zh": r"(?:不|没|無|无|非|未|別|别)",
    "ar": r"(?:لا|ما|ليس|ليست|غير|بدون|دون)",
    "fa": r"(?:نه|نیست|نیستم|بدون|هیچ|نمی)",
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


def segments(s):
    """Découpe en phrases. ⚠️ La ponctuation CJK (。！？；) n'est pas ASCII : sans elle,
    une phrase chinoise compte pour 1 et l'écran CLAUSES devient muet par construction."""
    s = (s or "").strip()
    if not s:
        return 0
    return len([x for x in re.split(r"[.;:!?。！？；：]\s*|\n", s) if x.strip()])


def calibrate_cjk(pairs):
    """Etalonne un ecran de CONTENU sur la langue elle-meme, au lieu de la declarer NA.

    Un ratio brut contre le francais ne veut rien dire en chinois (0,146-0,417 mesure).
    Mais la COMPRESSION du chinois est stable : `zh_len ~ a * fr_len + b` explique la
    moitie de la variance (R2=0,498 mesure sur les 175 cartes). Une cellule qui s'ecarte
    du residu attendu est donc un VRAI signal, calibre sur le chinois et non sur une
    autre ecriture. Le seuil de 2,5 ecarts-types ne leve qu'UNE carte sur le corpus reel
    (PK 112, une compression reelle), et attrape 104/175 troncatures a 50 %, 167/175 a
    35 % (controle inverse). Declarer l'ecran NA laissait 0 % de pouvoir de detection.
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


def flags_for(fr, tg, lang="ru", cjk_cal=None):
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
        if is_cjk_dominant(tg):
            if cjk_cal is None:
                # Nommé, pas silencieux : le lecteur doit savoir que l'écran de longueur
                # n'a PAS tourné sur cette rangée, et pourquoi.
                out.append("LEN-NA(CJK)")
            else:
                a, b, sd = cjk_cal
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

    def reference_for(drow):
        """(etage, tag, ligne, colonne-presente) -- 1 = `path`, 2 = le pont.
        None si aucune archive. Le 4e champ distingue une archive SANS la colonne
        d'une archive dont la cellule est vide : sans lui, les deux se rendent pareil."""
        p = bridge.n(drow.get("path"))
        if p in by_path:
            t, r = by_path[p]
            return 1, t, r, col_present.get(t, False)
        b = base_by_pk.get(bridge.n(drow.get("PK")))
        if b:
            hits = by_name.get(bridge.key(b.get("text_fr"))) or []
            if hits:
                return 2, hits[0][0], hits[0][1], col_present.get(hits[0][0], False)
        return None, None, None, False

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

        # (c) Ecran de CONTENU sur une ecriture CJK. La version precedente declarait
        # l'ecran NA (LEN-NA(CJK)) : pouvoir de detection NUL par construction. On exige
        # ici que l'ecran etalonne attrape une cellule chinoise tronquee.
        zh = [r for r in deck if is_cjk_dominant(r.get("desc_zh") or "")]
        if zh:
            cal = calibrate_cjk([(len(r["desc_fr"].strip()), len(r["desc_zh"].strip()))
                                 for r in zh])
            assert cal, "calibration CJK impossible -- ecran de contenu NON testable"
            a2, b2, sd2 = cal
            vict = zh[0]
            tronq = vict["desc_zh"].strip()[: max(1, len(vict["desc_zh"].strip()) // 2)]
            fc = flags_for(vict["desc_fr"], tronq, "zh", cal)
            print("  (c) cellule zh tronquee a 50%% -> %s" % fc)
            assert any(x.startswith("LENDEV") for x in fc), \
                "ecran CJK AVEUGLE a une cellule tronquee"
            # Temoin negatif : une cellule zh SAINE ne doit pas lever LENDEV.
            sane = [r for r in zh if len(r["desc_zh"].strip()) > 30][0]
            fz = flags_for(sane["desc_fr"], sane["desc_zh"].strip(), "zh", cal)
            assert not any(x.startswith("LENDEV") for x in fz), \
                "ecran CJK crie sur une cellule saine -- borne trop serree"
            print("  (c') temoin sain (PK=%s) -> aucun LENDEV" % sane.get("PK"))
        print("  PASS. ⚠️ Ce sont des priorités de lecture : le contrôle prouve que")
        print("  l'instrument n'est pas structurellement aveugle, pas qu'il juge.")
        return

    if not a.out:
        raise SystemExit("--out requis hors --self-test")

    bridge = load_bridge()
    reference_for = build(deck, bridge, lang)
    matched = {1: 0, 2: 0}
    n_flag = n_notice = 0
    usable = 0
    col_missing = set()
    cjk_cal = calibrate_cjk([(len((r.get("desc_fr") or "").strip()),
                              len((r.get("desc_%s" % lang) or "").strip()))
                             for r in deck if (r.get("desc_%s" % lang) or "").strip()
                             and is_cjk_dominant(r.get("desc_%s" % lang) or "")])
    if cjk_cal:
        print("CALIBRATION CJK: pente=%.3f ordonnee=%.1f ecart-type=%.1f car."
              % (cjk_cal[0], cjk_cal[1], cjk_cal[2]))
    with open(a.out, "w", encoding="utf-8", newline="\n") as out:
        for i, r in enumerate(deck, 1):
            fr = (r.get("desc_fr") or "").strip()
            tg = (r.get("desc_%s" % lang) or "").strip()
            f = flags_for(fr, tg, lang, cjk_cal)
            prio = [x for x in f if not x.startswith(("LEN-NA", "POL-NA"))]
            if prio:
                n_flag += 1
            elif f:
                n_notice += 1
            tier, tag, arow, has_col = reference_for(r)
            if tier:
                matched[tier] += 1
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
                via = "etage%d" % tier if tier == 1 else "pont"
                if not has_col:
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
        out.write("REFERENCE IMPRIMEE: %d/%d (etage1 `path` %d + pont %d; ⛔ pas %d -- "
                  "cf. docs/corpus/archive-coverage-2026-09-22.md)\n"
                  % (matched[1] + matched[2], len(deck), matched[1], matched[2], matched[1]))
        out.write("REFERENCE EXPLOITABLE: %d/%d -- %s\n"
                  % (usable, len(deck),
                     "colonne desc_%s presente dans toutes les archives" % lang
                     if not col_missing else
                     "⛔ AUCUN arbitrage imprime : desc_%s ABSENTE des archives %s"
                     % (lang, sorted(col_missing))))
    print("written %s: %d cards, %d flagged, reference %d/%d"
          % (a.out, len(deck), n_flag, matched[1] + matched[2], len(deck)))


if __name__ == "__main__":
    main()
