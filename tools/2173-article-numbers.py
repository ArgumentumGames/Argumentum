#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Compte les chiffres de la PROSE des articles de la page Actus (#2173).

Instrument de la passe « les chiffres au service du récit » : il donne le compte
avant/après par article, et la liste des chiffres gardés avec leur contexte, pour
que chaque nombre retenu soit justifié un par un.

Périmètre mesuré
----------------
Le corps de l'article : tout ce qui précède le séparateur `\\n---\\n`. Le
paragraphe `*Sources…*` est donc exclu par construction. Sont exclus en plus :
la ligne de légende `*Illustration…*`, les blocs de code clôturés, et les cibles
de liens markdown `](…)` (une URL, un hash de commit dans une URL).

Un « chiffre » est un jeton numérique tel qu'un lecteur le voit : les groupes de
milliers séparés par une espace fine (U+00A0, U+202F) ou une espace ordinaire
comptent POUR UN SEUL nombre — `1 408` vaut 1, pas 2. Les identifiants de commit
(7 à 40 caractères hexadécimaux contenant au moins une lettre) sont comptés à
part : ce ne sont pas des chiffres de récit.

Limites assumées (elles jouent dans les deux sens, avant comme après) : un
nombre écrit en mots (`douze questions`) n'est pas compté ; un identifiant en
pointillés (`7.3.1.2`) compte pour ses quatre chiffres ; une version
(`LGPL-3.0`) compte pour deux.

Usage
-----
  python tools/2173-article-numbers.py                  # état du worktree
  python tools/2173-article-numbers.py --rev origin/master   # état d'une révision
  python tools/2173-article-numbers.py --list           # les chiffres gardés
  python tools/2173-article-numbers.py --self-test      # éprouve l'instrument
"""
import argparse
import glob
import io
import os
import re
import subprocess
import sys
import tempfile

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ART = os.path.join(ROOT, "docs", "publication", "articles")

SEP = "[   ]"
HASH = re.compile(r"\b(?=[0-9a-f]*[a-f])[0-9a-f]{7,40}\b")
# Forme groupée d'abord (1 à 3 chiffres, puis des groupes d'exactement 3 séparés
# par une espace fine ou ordinaire) : c'est la forme la plus longue qui gagne.
NUM = re.compile(r"\d{1,3}(?:" + SEP + r"\d{3})+(?!\d)|\d+")


def body_of(text):
    body = text.split("\n---\n")[0]
    body = "\n".join(l for l in body.splitlines() if not l.startswith("*Illustration"))
    body = re.sub(r"```.*?```", "", body, flags=re.S)
    body = re.sub(r"\]\([^)]*\)", "]", body)
    return body


def measure_text(text):
    """Rend (liste des chiffres, liste des hashs) du corps d'un article."""
    body = body_of(text)
    hashes = HASH.findall(body)
    return NUM.findall(HASH.sub(" ", body)), hashes


def measure_file(path):
    with io.open(path, encoding="utf-8") as fh:
        return measure_text(fh.read())


def articles_at(rev):
    """[(nom, texte)] des 7 articles tels qu'ils sont à `rev`, sans toucher au worktree."""
    names = [os.path.basename(p) for p in
             sorted(glob.glob(os.path.join(ART, "*.fr.md")))]
    out = []
    for name in names:
        rel = "docs/publication/articles/" + name
        text = subprocess.check_output(
            ["git", "-C", ROOT, "show", "%s:%s" % (rev, rel)]).decode("utf-8")
        out.append((name, text))
    return out


def articles_local():
    out = []
    for path in sorted(glob.glob(os.path.join(ART, "*.fr.md"))):
        with io.open(path, encoding="utf-8") as fh:
            out.append((os.path.basename(path), fh.read()))
    return out


def report(pairs, label):
    total_n = total_h = 0
    for name, text in pairs:
        nums, hashes = measure_text(text)
        total_n += len(nums)
        total_h += len(hashes)
        print("%-40s chiffres %3d   hashes %2d" % (name, len(nums), len(hashes)))
    print("%-40s chiffres %3d   hashes %2d" % (label, total_n, total_h))
    return total_n, total_h


def listing(pairs):
    """Les numéros de ligne sont ceux du FICHIER : on saute les lignes exclues
    (légende d'illustration, blocs de code) sans renuméroter le reste."""
    for name, text in pairs:
        print("\n#### " + name)
        in_fence = False
        for i, line in enumerate(text.split("\n---\n")[0].split("\n"), 1):
            if line.lstrip().startswith("```"):
                in_fence = not in_fence
                continue
            if in_fence or line.startswith("*Illustration"):
                continue
            clean = HASH.sub(" ", re.sub(r"\]\([^)]*\)", "]", line))
            for m in NUM.finditer(clean):
                a = max(0, m.start() - 34)
                b = min(len(clean), m.end() + 34)
                print("  L%-4d %-10s … %s …"
                      % (i, m.group(0), clean[a:b].replace(" ", " ").replace(" ", " ")))


CASES = [
    ("1 408", 1), ("29 225", 1), ("272 043", 1),
    ("20 mars 2020", 2), ("167 × 175", 2),
    ("1 064 nœuds sur 1 408", 2), ("7.3.1.2", 4),
    ("LGPL-3.0", 2), ("167 et 175", 2), ("123", 1),
]


def self_test():
    ok = True
    for text, expected in CASES:
        got = len(measure_text(text + "\n")[0])
        if got != expected:
            ok = False
        print("%s attendu %d obtenu %d  %r"
              % ("OK " if got == expected else "ROUGE", expected, got, text))
    # contrôle inverse : le compte doit bouger quand on ajoute un chiffre
    base = len(measure_text("Un paragraphe sans nombre.\n")[0])
    plus = len(measure_text("Un paragraphe sans nombre, plus 12.\n")[0])
    if base != 0 or plus != 1:
        ok = False
    print("%s contrôle inverse 0 -> 1  (%d -> %d)"
          % ("OK " if (base, plus) == (0, 1) else "ROUGE", base, plus))
    print("INSTRUMENT", "sain" if ok else "CASSE")
    return 0 if ok else 1


def main():
    ap = argparse.ArgumentParser(description=__doc__.split("\n")[0])
    ap.add_argument("--rev", help="mesurer une révision (ex. origin/master) au lieu du worktree")
    ap.add_argument("--list", action="store_true", help="lister les chiffres gardés")
    ap.add_argument("--self-test", action="store_true", help="éprouver l'instrument")
    args = ap.parse_args()
    if args.self_test:
        return self_test()
    pairs = articles_at(args.rev) if args.rev else articles_local()
    label = "TOTAL (%s)" % args.rev if args.rev else "TOTAL (worktree)"
    report(pairs, label)
    if args.list:
        listing(pairs)
    return 0


if __name__ == "__main__":
    sys.exit(main())
