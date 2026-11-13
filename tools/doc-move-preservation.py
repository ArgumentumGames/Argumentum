#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Preuve de préservation d'un déplacement de documentation — stdlib seulement.

    python tools/doc-move-preservation.py --base <ref> --source CLAUDE.md \\
        --into CLAUDE.md --into AGENTS.md --into docs/agents \\
        [--manifest docs/agents/move-manifest.tsv] [--head <ref>]

POURQUOI CET ORGANE EXISTE. « Consolider » un document, c'est déplacer ses lignes sans en
perdre. Une PR qui retire 500 lignes d'un fichier et en ajoute 500 ailleurs ne se relit
pas à l'œil : le relecteur doit croire que rien n'a disparu en route. Cet organe remplace
la croyance par un décompte : chaque ligne non vide du fichier d'origine, pris à une ref
git, doit se retrouver MOT POUR MOT dans les fichiers d'arrivée — ou être déclarée, avec
sa raison, dans un manifeste.

CE QU'IL PROUVE
  - aucune ligne non vide de la source n'a disparu (comparaison en multi-ensemble : une
    ligne présente 3 fois à l'origine doit l'être au moins 3 fois à l'arrivée) ;
  - toute ligne absente est DÉCLARÉE au manifeste, et toute déclaration du manifeste
    désigne bien une ligne absente (une déclaration devenue inutile est une erreur :
    elle masquerait la prochaine perte au même numéro).

CE QU'IL NE PROUVE PAS, et le dit
  - que l'ORDRE est conservé : les « suites » affichées (lignes d'origine retrouvées
    d'affilée dans un même fichier) le montrent, elles ne sont pas le verdict ;
  - que le texte déplacé est encore JUSTE, ni que les liens relatifs tiennent toujours
    depuis le nouvel emplacement ;
  - la comparaison ignore les espaces de début et de fin de ligne, et les fins de ligne
    (CRLF/LF) : un changement d'indentation seul n'est pas vu.

SORTIES
  0  PRÉSERVÉ — aucune ligne perdue hors manifeste, aucun manifeste périmé
  1  PERTE — au moins une ligne absente non déclarée, ou une déclaration inutile
  2  ORGANE AVEUGLE — source introuvable à la ref, aucun fichier d'arrivée, git absent :
     l'organe ne peut pas mesurer et refuse de rendre un vert.

MANIFESTE (TSV, `#` = commentaire) :  <chemin source> TAB <numéro de ligne> TAB <raison>
  Un chemin RELATIF se lit depuis la racine du dépôt, jamais depuis le répertoire courant ;
  avec `--head`, il est lu DANS LE COMMIT examiné, comme les fichiers d'arrivée — la preuve
  à un commit n'emprunte rien au disque. Un chemin absolu est toujours lu sur le disque.

PLUSIEURS SOURCES : le décompte est commun. Une ligne présente dans deux sources doit se
retrouver deux fois à l'arrivée.
"""
import argparse
import collections
import difflib
import os
import subprocess
import sys


def git(repo, *args):
    done = subprocess.run(["git", "-C", repo] + list(args), capture_output=True)
    return done.returncode, done.stdout, done.stderr.decode("utf-8", "replace").strip()


def blind(message):
    print("ORGANE AVEUGLE : " + message)
    sys.exit(2)


def norm(raw):
    """Octets -> lignes, sans fin de ligne ni espaces de bord."""
    text = raw.decode("utf-8", "replace").replace("\r\n", "\n").replace("\r", "\n")
    if text.startswith("\ufeff"):
        text = text[1:]
    lines = text.split("\n")
    if lines and lines[-1] == "":
        lines.pop()
    return [line.strip() for line in lines]


def read_source(repo, base, path):
    code, out, err = git(repo, "show", "%s:%s" % (base, path))
    if code != 0:
        blind("source `%s` introuvable à la ref `%s` (%s)" % (path, base, err))
    return norm(out)


def list_targets(repo, head, specs):
    """Fichiers d'arrivée : chaque spec est un fichier, ou un dossier (tous ses *.md)."""
    found = []
    for spec in specs:
        spec = spec.replace("\\", "/").rstrip("/")
        if head is None:
            full = os.path.join(repo, *spec.split("/"))
            if os.path.isdir(full):
                for root, _, names in os.walk(full):
                    for name in sorted(names):
                        if name.lower().endswith(".md"):
                            rel = os.path.relpath(os.path.join(root, name), repo)
                            found.append(rel.replace(os.sep, "/"))
            elif os.path.isfile(full):
                found.append(spec)
        else:
            code, out, _ = git(repo, "ls-tree", "-r", "--name-only", head, "--", spec)
            if code == 0:
                for rel in out.decode("utf-8", "replace").splitlines():
                    if rel == spec or rel.lower().endswith(".md"):
                        found.append(rel)
    return sorted(set(found))


def read_target(repo, head, rel):
    if head is None:
        with open(os.path.join(repo, *rel.split("/")), "rb") as handle:
            return norm(handle.read())
    code, out, err = git(repo, "show", "%s:%s" % (head, rel))
    if code != 0:
        blind("fichier d'arrivée `%s` illisible à la ref `%s` (%s)" % (rel, head, err))
    return norm(out)


def read_manifest(repo, head, path):
    """-> (déclarations, origine lue). Relatif = racine du dépôt ; avec --head = le commit."""
    declared = {}
    if not path:
        return declared, None
    if os.path.isabs(path):
        origin = path
        if not os.path.isfile(path):
            blind("manifeste `%s` introuvable" % path)
        with open(path, "rb") as handle:
            raw = handle.read()
    elif head is not None:
        rel = path.replace("\\", "/")
        origin = "%s @ %s" % (rel, head)
        code, raw, err = git(repo, "show", "%s:%s" % (head, rel))
        if code != 0:
            blind("manifeste `%s` absent du commit `%s` : la preuve à un commit lit le "
                  "manifeste de ce commit (%s)" % (rel, head, err))
    else:
        full = os.path.join(repo, *path.replace("\\", "/").split("/"))
        origin = "%s (arbre de travail)" % path.replace("\\", "/")
        if not os.path.isfile(full):
            blind("manifeste `%s` introuvable depuis la racine du dépôt" % path)
        with open(full, "rb") as handle:
            raw = handle.read()
    text = raw.decode("utf-8", "replace")
    if text.startswith("\ufeff"):
        text = text[1:]
    for number, line in enumerate(text.split("\n"), 1):
        line = line.rstrip("\r")
        if not line.strip() or line.lstrip().startswith("#"):
            continue
        parts = line.split("\t")
        if len(parts) < 3 or not parts[1].strip().isdigit() or not parts[2].strip():
            blind("manifeste `%s` ligne %d : attendu <source> TAB <ligne> TAB <raison>"
                  % (origin, number))
        key = (parts[0].strip().replace("\\", "/"), int(parts[1]))
        if key in declared:
            blind("manifeste `%s` ligne %d : %s L%d déclarée deux fois" % (origin, number, key[0], key[1]))
        declared[key] = parts[2].strip()
    return declared, origin


def find_runs(source, targets, min_run):
    """Suites de lignes d'origine retrouvées d'affilée dans un même fichier d'arrivée."""
    blocks = []
    for rel, lines in targets.items():
        matcher = difflib.SequenceMatcher(None, source, lines, autojunk=False)
        for i, j, size in matcher.get_matching_blocks():
            if size >= min_run:
                blocks.append((size, i, j, rel))
    blocks.sort(key=lambda b: (-b[0], b[1]))
    taken = [False] * len(source)
    runs = []
    for size, i, j, rel in blocks:
        k = 0
        while k < size:
            if taken[i + k]:
                k += 1
                continue
            start = k
            while k < size and not taken[i + k]:
                taken[i + k] = True
                k += 1
            if k - start >= min_run:
                runs.append((i + start + 1, i + k, rel, j + start + 1, j + k))
    runs.sort()
    return runs, taken


def main(argv=None):
    parser = argparse.ArgumentParser(description="Preuve de préservation d'un déplacement de doc.")
    parser.add_argument("--base", required=True, help="ref git de l'état d'origine")
    parser.add_argument("--source", action="append", required=True,
                        help="fichier d'origine, lu à --base (répétable)")
    parser.add_argument("--into", action="append", required=True,
                        help="fichier ou dossier d'arrivée (répétable) ; dossier = ses *.md")
    parser.add_argument("--head", default=None,
                        help="ref git de l'état d'arrivée (défaut : l'arbre de travail)")
    parser.add_argument("--manifest", default=None, help="TSV des lignes déclarées non reprises")
    parser.add_argument("--min-run", type=int, default=3, help="longueur minimale d'une suite")
    parser.add_argument("--repo", default=None, help="racine du dépôt (défaut : celui de l'outil)")
    args = parser.parse_args(argv)
    for stream in (sys.stdout, sys.stderr):
        if hasattr(stream, "reconfigure"):      # console Windows cp1252 : « ≥ » n'y existe pas
            stream.reconfigure(encoding="utf-8", errors="replace")

    repo = args.repo or os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    try:
        code, _, err = git(repo, "rev-parse", "--verify", "--quiet", args.base + "^{commit}")
    except OSError as exc:
        blind("git indisponible (%s)" % exc)
    if code != 0:
        blind("ref d'origine `%s` inconnue dans ce dépôt" % args.base)

    rels = list_targets(repo, args.head, args.into)
    if not rels:
        blind("aucun fichier d'arrivée trouvé pour %s" % ", ".join(args.into))
    targets = {rel: read_target(repo, args.head, rel) for rel in rels}
    have = collections.Counter(line for lines in targets.values() for line in lines if line)
    declared, manifest_origin = read_manifest(repo, args.head, args.manifest)
    used = set()
    lost_total = 0
    budget = dict(have)     # commun à toutes les sources : une ligne ne sert qu'une fois

    print("ARRIVÉE (%s) : %d fichier(s), %d lignes non vides"
          % (args.head or "arbre de travail", len(targets), sum(have.values())))
    for rel in rels:
        print("  %s (%d lignes)" % (rel, len(targets[rel])))
    if manifest_origin:
        print("MANIFESTE : %s, %d déclaration(s)" % (manifest_origin, len(declared)))

    for path in args.source:
        path = path.replace("\\", "/")
        source = read_source(repo, args.base, path)
        nonblank = [(n, line) for n, line in enumerate(source, 1) if line]
        if not nonblank:
            blind("source `%s` vide à la ref `%s`" % (path, args.base))

        # Verdict : multi-ensemble. Les DERNIÈRES occurrences excédentaires sont les absentes.
        absent = []
        for n, line in nonblank:
            if budget.get(line, 0) > 0:
                budget[line] -= 1
            else:
                absent.append((n, line))

        runs, taken = find_runs(source, targets, args.min_run)
        absent_numbers = set(n for n, _ in absent)
        in_runs = sum(1 for n, _ in nonblank if taken[n - 1])
        isolated = [n for n, _ in nonblank if not taken[n - 1] and n not in absent_numbers]

        print("")
        print("SOURCE %s @ %s : %d lignes, %d non vides" % (path, args.base, len(source), len(nonblank)))
        print("SUITES (≥ %d lignes d'affilée, dans l'ordre) : %d, couvrant %d lignes non vides"
              % (args.min_run, len(runs), in_runs))
        for first, last, rel, t_first, t_last in runs:
            print("  L%d-L%d -> %s L%d-L%d" % (first, last, rel, t_first, t_last))
        print("ISOLÉES (retrouvées mot pour mot, hors suite) : %d" % len(isolated))
        if isolated:
            print("  " + ", ".join("L%d" % n for n in isolated))

        stated = [(n, line) for n, line in absent if (path, n) in declared]
        lost = [(n, line) for n, line in absent if (path, n) not in declared]
        used.update((path, n) for n, _ in stated)
        print("DÉCLARÉES au manifeste : %d" % len(stated))
        for n, line in stated:
            print("  L%d  %s\n        raison : %s" % (n, line[:100], declared[(path, n)]))
        print("PERDUES : %d" % len(lost))
        for n, line in lost:
            print("  L%d  %s" % (n, line[:140]))
        lost_total += len(lost)

    sources = set(p.replace("\\", "/") for p in args.source)
    stale = sorted(key for key in declared if key not in used)
    foreign = [key for key in stale if key[0] not in sources]
    if foreign:
        blind("le manifeste déclare une source non examinée : %s" % foreign[0][0])
    print("")
    if stale:
        print("MANIFESTE PÉRIMÉ : %d déclaration(s) désignent une ligne qui n'est PAS absente" % len(stale))
        for path, n in stale:
            print("  %s L%d" % (path, n))
    if lost_total or stale:
        print("VERDICT : PERTE — %d ligne(s) perdue(s), %d déclaration(s) périmée(s)"
              % (lost_total, len(stale)))
        return 1
    print("VERDICT : PRÉSERVÉ — aucune ligne non vide perdue hors manifeste")
    return 0


if __name__ == "__main__":
    sys.exit(main())
