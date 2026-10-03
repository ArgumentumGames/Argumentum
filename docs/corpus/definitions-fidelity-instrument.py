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

# Marqueurs de négation. ⚠️ `\bне\b` ne voit PAS la négation préfixée russe
# (недостаточны, необоснованными, неприменим, невозможность...) : l'écran mesure
# l'absence d'une PARTICULE, pas l'absence d'une NÉGATION. Mesuré sur (20) : 21/21
# drapeaux POLARITY sont des faux positifs pour cette raison.
NEG_FR = re.compile(r"(?:\bne\b|\bn'|\bpas\b|\bjamais\b|\baucun|\bsans\b|\brien\b|\bnon\b)", re.I)
NEG_RU = re.compile(r"(?:\bне\b|\bни\b|\bникогда\b|\bбез\b|\bнет\b|\bничего\b|\bни один)", re.I)


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


def segments(s):
    s = (s or "").strip()
    if not s:
        return 0
    return len([x for x in re.split(r"[.;:!?]\s+|\n", s) if x.strip()])


def flags_for(fr, tg):
    """Drapeaux mécaniques, bornes calibrées sur ce corpus (phrases uniques, 48-187 car.).
    Chacun est une PRIORITÉ DE LECTURE, jamais un verdict."""
    out = []
    if not (tg or "").strip():
        out.append("EMPTY-target")
        return out
    if not (fr or "").strip():
        out.append("EMPTY-source")
    lf, lt = len((fr or "").strip()), len((tg or "").strip())
    if lf:
        ratio = lt / lf
        if ratio < 0.60:
            out.append("SHORT(%.2f)" % ratio)
        elif ratio > 1.65:
            out.append("LONG(%.2f)" % ratio)
    only = nums(fr) - nums(tg)
    if only:
        out.append("NUM-only-FR:" + ",".join(sorted(only)[:4]))
    if bool(NEG_FR.search(fr)) != bool(NEG_RU.search(tg)):
        out.append("POLARITY")
    sf, st = segments(fr), segments(tg)
    if sf - st >= 2:
        out.append("CLAUSES(%d<%d)" % (st, sf))
    if sf and st > sf + 2:
        out.append("CLAUSES(%d>%d)" % (st, sf))
    return out


def build(deck, bridge):
    arch = [(t, r) for t, p in bridge.ARCHIVES for r in bridge.rows(p)]
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
        """(etage, tag, ligne) -- 1 = `path`, 2 = le pont. None si aucune archive."""
        p = bridge.n(drow.get("path"))
        if p in by_path:
            t, r = by_path[p]
            return 1, t, r
        b = base_by_pk.get(bridge.n(drow.get("PK")))
        if b:
            hits = by_name.get(bridge.key(b.get("text_fr"))) or []
            if hits:
                return 2, hits[0][0], hits[0][1]
        return None, None, None

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
        fa = flags_for(victim["desc_fr"], victim["desc_%s" % lang])
        print("  (a) cellule tronquée -> %s" % fa)
        assert any(x.startswith("SHORT") for x in fa), "instrument AVEUGLE a une cellule tronquee"
        neg = [r for r in deck if NEG_FR.search(r.get("desc_fr") or "")]
        assert neg, "aucune carte a marqueur de negation FR -- ecran POLARITY non testable"
        v2 = dict(neg[0])
        fb = flags_for(v2["desc_fr"], NEG_RU.sub("", v2["desc_%s" % lang]))
        print("  (b) polarité inversée -> %s" % fb)
        assert "POLARITY" in fb, "instrument AVEUGLE a un desaccord de polarite"
        print("  PASS. ⚠️ Ce sont des priorités de lecture : le contrôle prouve que")
        print("  l'instrument n'est pas structurellement aveugle, pas qu'il juge.")
        return

    if not a.out:
        raise SystemExit("--out requis hors --self-test")

    bridge = load_bridge()
    reference_for = build(deck, bridge)
    matched = {1: 0, 2: 0}
    n_flag = 0
    with open(a.out, "w", encoding="utf-8", newline="\n") as out:
        for i, r in enumerate(deck, 1):
            fr = (r.get("desc_fr") or "").strip()
            tg = (r.get("desc_%s" % lang) or "").strip()
            f = flags_for(fr, tg)
            if f:
                n_flag += 1
            tier, tag, arow = reference_for(r)
            if tier:
                matched[tier] += 1
            out.write("### [%03d] PK=%s path=%s\n" % (i, r.get("PK"), (r.get("path") or "").strip()))
            out.write("TITRE fr: %s\n" % (r.get("text_fr") or "").strip())
            out.write("TITRE %s: %s\n" % (lang, (r.get("text_%s" % lang) or "").strip()))
            out.write("FR: %s\n" % fr)
            out.write("%s: %s\n" % (lang.upper(), tg))
            if arow is not None:
                pl = (arow.get("desc_%s" % lang) or "").strip()
                via = "etage%d" % tier if tier == 1 else "pont"
                if pl:
                    out.write("IMPRIME[%s](%s) [%s]: %s\n"
                              % (via, tag, "IDENTIQUE" if pl == tg else "DIFFERE", pl))
                else:
                    out.write("IMPRIME[%s](%s): <pas de desc_%s>\n" % (via, tag, lang))
            else:
                out.write("IMPRIME: <aucune reference, sous aucun nom>\n")
            out.write("FLAGS: %s\n\n" % (", ".join(f) if f else "-"))
        out.write("---\nDENOMINATEUR: %d cartes lues, %d affichees\n" % (len(deck), len(deck)))
        out.write("FLAGS mecaniques: %d/%d lignes portent au moins un drapeau\n"
                  % (n_flag, len(deck)))
        out.write("REFERENCE IMPRIMEE: %d/%d (etage1 `path` %d + pont %d; ⛔ pas %d -- "
                  "cf. docs/corpus/archive-coverage-2026-09-22.md)\n"
                  % (matched[1] + matched[2], len(deck), matched[1], matched[2], matched[1]))
    print("written %s: %d cards, %d flagged, reference %d/%d"
          % (a.out, len(deck), n_flag, matched[1] + matched[2], len(deck)))


if __name__ == "__main__":
    main()
