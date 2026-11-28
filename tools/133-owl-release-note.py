#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Note de version de l'ontologie — brouillon dérivé du delta (P24-47, #133).

    python tools/133-owl-release-note.py --repo . \
        --old docs/ontology/v1.0.0/argumentum_virtues.owl --csv-old c1491de9 \
        --new docs/ontology/argumentum_virtues.owl --csv-new HEAD \
        --other-old docs/ontology/v1.0.0/argumentum.owl --other-new docs/ontology/argumentum.owl \
        --date 2026-11-28 --out docs/ontology/133-owl-note-de-version-brouillon-2026-11-28.md

    # la note est-elle à jour ? LA MÊME COMMANDE, --check en plus. Sans
    # --other-old/--other-new, la section « ce qui n'a pas changé » perd sa ligne
    # sur l'autre artefact, et --check répond « perime » sur un arbre correct.
    python tools/133-owl-release-note.py --repo . \
        --old docs/ontology/v1.0.0/argumentum_virtues.owl --csv-old c1491de9 \
        --new docs/ontology/argumentum_virtues.owl --csv-new HEAD \
        --other-old docs/ontology/v1.0.0/argumentum.owl --other-new docs/ontology/argumentum.owl \
        --date 2026-11-28 --out docs/ontology/133-owl-note-de-version-brouillon-2026-11-28.md --check

    À lancer depuis la racine du dépôt : `--old` et `--new` sont ouverts relativement
    au répertoire courant, pas à `--repo`.

CE QUE CE FICHIER FAIT. Il rédige le **brouillon de note de version** de l'ontologie :
les changements de sens entre le blob servi et le blob de master, **classés par IRI**, et
chaque changement **rattaché à son commit**.

D'OUIN VIENT CHAQUE LIGNE.

  · la liste des changements vient de `tools/133-owl-regen-delta.py` (l'instrument de
    P24-37), appelé en sous-processus : il compare les deux `.owl` et **projette le CSV**
    pour dire quelle clé a bougé et pourquoi. Le sous-processus écrit son rapport JSON ;
    ce script ne le relit pas autrement.
  · le rattachement à un commit vient de la **chaîne de la cellule CSV** : pour chaque
    pk et chaque colonne en cause, le script relit le CSV à **chaque commit** qui a
    touché le fichier dans la plage, et note les transitions. Le commit d'une
    transition est donc le commit qui a changé CETTE cellule — pas un pickaxe qui
    tombe sur la même chaîne dans une autre cellule.
  · l'IRI vient du registre (`OwlIriRegistry.cs`), pas d'une convention de nom.

CE QUI EMPÊCHE LE BROUILLON DE MENTIR. Le script **échoue plutôt que de produire une note
qui n'explique pas un changement**. L'autorité sur les causes est l'instrument de delta :
**son code de sortie est propagé tel quel** — 2 : l'instrument a lui-même échoué ; 3 : une
IRI perdue ; 4 : un changement d'OWL sans cause dans le CSV ; 5 : un changement de CSV
sans effet sur l'OWL ; 6 : une clé hors oracle. Deux échecs sont en propre à ce script : un
changement que le registre ne rattache à aucune cellule — ni pk ni colonne — sort en **7**,
et une cellule porteuse dont aucun commit de la plage n'explique la transition sort en
**8**. Tous les motifs sont écrits sur stderr.

DETERMINISME. Aucune date n'est lue dans l'horloge : la date vient de `--date`, les
commits sont lus dans le dépôt. Les mêmes entrées donnent le même brouillon, octet pour
octet — c'est ce qui permet `--check` (et le contrat qui le rejoue).
"""
import argparse
import contextlib
import csv
import hashlib
import importlib.util
import io
import json
import os
import subprocess
import sys
import tempfile

HERE = os.path.dirname(os.path.abspath(__file__))
REPO = os.path.dirname(HERE)
CSV_REL = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
DELTA_TOOL = os.path.join(HERE, "133-owl-regen-delta.py")

# Le champ OWL, son nom lisible et ce qu'un changement veut dire.
PREDICATE = {
    ("prefLabel", "fr"): ("skos:prefLabel@fr", "libellé (français)"),
    ("prefLabel", "en"): ("skos:prefLabel@en", "libellé (anglais)"),
    ("definition", "fr"): ("skos:definition@fr", "définition (française)"),
    ("definition", "en"): ("skos:definition@en", "définition (anglaise)"),
    ("seeAlso", "fr"): ("rdfs:seeAlso@fr", "lien de référence (français)"),
    ("seeAlso", "en"): ("rdfs:seeAlso@en", "lien de référence (anglais)"),
}


def load_delta_module():
    spec = importlib.util.spec_from_file_location("owl_regen_delta", DELTA_TOOL)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


DELTA = load_delta_module()


def git(repo, *args):
    done = subprocess.run(["git", "-C", repo] + list(args), capture_output=True)
    if done.returncode != 0:
        raise RuntimeError("git %s a échoué : %s" % (" ".join(args),
                                                    done.stderr.decode("utf-8", "replace").strip()))
    return done.stdout.decode("utf-8", "replace")


def cell_history(repo, cells, csv_old, csv_new):
    """Les transitions de chaque cellule demandée, avec leur commit.

    `cells` est un ensemble de (pk, colonne). Les commits qui ont touché le CSV
    dans la plage sont lus **une fois chacun** : le CSV est parsé une fois par
    commit, et toutes les cellules demandées y sont lues dans la même passe —
    relire le CSV par cellule coûterait des centaines de `git show`.

    Renvoie {(pk, colonne) : [(commit, date, sujet, avant, apres)]} — une transition
    par commit qui a changé la cellule. Le premier état est celui de `csv_old`.
    """
    commits = git(repo, "rev-list", "--reverse", "%s..%s" % (csv_old, csv_new),
                  "--", CSV_REL).split()
    chains = {cell: [] for cell in cells}
    previous = {}
    for ref in commits:
        rows, _order, _cols = DELTA.load_csv(repo, ref)
        touched = False
        for (pk, column) in cells:
            value = (rows.get(pk) or {}).get(column, "")
            if (pk, column) not in previous:
                previous[(pk, column)] = value
                continue
            if value != previous[(pk, column)]:
                meta = git(repo, "show", "-s", "--format=%h\t%ad\t%s", "--date=short",
                           ref).strip().split("\t")
                chains[(pk, column)].append((meta[0], meta[1], meta[2],
                                             previous[(pk, column)], value))
                previous[(pk, column)] = value
                touched = True
        if not touched:
            continue
    return chains


def transitions_of(row):
    """Les transitions de la cellule responsable d'un changement OWL."""
    return row["_chain"]


def diff_values(old, new):
    """Ce qui part, ce qui vient, ce qui reste — pour l'affichage d'une ligne."""
    old = list(old or [])
    new = list(new or [])
    removed = [v for v in old if v not in new]
    added = [v for v in new if v not in old]
    kept = [v for v in new if v in old]
    return removed, added, kept


def escape(text):
    return text.replace("|", "\\|")


def render(ctx):
    """Le brouillon, dérivé de tout ce qui précède."""
    rows = ctx["rows"]
    registry = ctx["registry"]
    ns = ctx["ns"]
    date = ctx["date"]
    out = []
    w = out.append

    # La ligne « l'autre artefact » : sans elle, la commande rejouee perd la
    # section « ce qui n'a pas changé » et `--check` dit « perime » sur un arbre
    # correct. Elle n'est ecrite que si l'appelant a fourni les deux faces.
    other_line = ""
    if ctx.get("other_old") and ctx.get("other_new"):
        other_line = ("  --other-old %s --other-new %s \\"
                      % (ctx["other_old"], ctx["other_new"]))

    w("# Note de version de l'ontologie — BROUILLON (%s)" % date)
    w("")
    w("> **BROUILLON — aucune version n'est publiée par ce document.** Il n'y a ni tag, ni")
    w("> release, ni annonce dans une note de version publique : c'est un brouillon de")
    w("> travail, committé dans le dossier du dépôt pour être relu. Le dossier")
    w("> `docs/ontology/` est monté en entier par `static.yml` : ce fichier est donc servi")
    w("> comme fichier statique — ce n'est pas une publication de version.")
    w("> Issue : #133 · file : #458 (P24-47, reprend CL-06) · lane : po-2024.")
    w("> Instrument : [`tools/133-owl-release-note.py`](../../tools/133-owl-release-note.py).")
    w("")
    w("## 1. Ce que ce brouillon compare")
    w("")
    w("| | Blob servi (avant) | Blob de master (après) |")
    w("|---|---|---|")
    w("| Fichier | `%s` | `%s` |" % (ctx["old_file"], ctx["new_file"]))
    w("| sha256 | `%s` | `%s` |" % (ctx["old_sha"], ctx["new_sha"]))
    w("| Octets | %s | %s |" % (ctx["old_bytes"], ctx["new_bytes"]))
    w("| CSV source | `%s` | `%s` |" % (ctx["csv_old"], ctx["csv_new"]))
    w("")
    w("Les deux blobs sont **ceux du dépôt** : aucun artefact de build n'est requis, la")
    w("comparaison est rejouable hors ligne.")
    w("")
    w("## 2. Le changement en une ligne")
    w("")
    w("**%d clés de l'OWL ont changé, sur %d IRI.** Chaque changement porte son commit ;" %
      (ctx["n_rows"], ctx["n_subjects"]))
    w("aucun n'est sans cause : le script échoue plutôt que de produire une note qui")
    w("n'explique pas un changement — il **propage le code de sortie de l'instrument de")
    w("delta**, 4 pour un changement d'OWL sans cause dans le CSV.")
    w("")
    w("| Champ | Changements | IRI touchées |")
    w("|---|---:|---:|")
    for key in sorted(ctx["by_predicate"]):
        nom, _sens = PREDICATE[key]
        w("| `%s` | %d | %d |" % (nom, ctx["by_predicate"][key]["n"],
                                  len(ctx["by_predicate"][key]["iris"])))
    w("")
    w("## 3. Les changements par IRI")
    w("")
    w("Une IRI par section ; une ligne par champ modifié ; le commit est celui qui a")
    w("changé la cellule du CSV responsable.")
    w("")
    for subject in ctx["order"]:
        iri = ns + "#" + subject
        w("### `%s`" % iri)
        w("")
        w("| Champ | Avant | Après | Commit |")
        w("|---|---|---|---|")
        for row in ctx["by_subject"][subject]:
            nom, sens = PREDICATE[(row["predicate"], row["lang"])]
            removed, added, kept = diff_values(row["owl_old"], row["owl_new"])
            avant = " ; ".join(removed + kept) if (removed or kept) else "—"
            apres = " ; ".join(added + kept) if (added or kept) else "—"
            w("| `%s` | %s | %s | %s |" % (nom, escape(avant), escape(apres),
                                           ctx["commit_cell"][(row["subject"], row["predicate"],
                                                              row["lang"])]))
        w("")
    w("## 4. Ce qui n'a pas changé")
    w("")
    for line in ctx["unchanged"]:
        w("- %s" % line)
    w("")
    w("## 5. Comment le rejouer")
    w("")
    w("```bash")
    w("# le delta, seul (l'instrument écrit son rapport JSON)")
    w("python tools/133-owl-regen-delta.py --repo . \\")
    w("  --old %s --csv-old %s \\" % (ctx["old_file"], ctx["csv_old"]))
    w("  --new %s --csv-new %s \\" % (ctx["new_file"], ctx["csv_new"]))
    w("  --attribute --json <delta.json>")
    w("")
    w("# le brouillon (appelle le delta ci-dessus, puis rattache chaque changement à son commit)")
    w("python tools/133-owl-release-note.py --repo . \\")
    w("  --old %s --csv-old %s \\" % (ctx["old_file"], ctx["csv_old"]))
    w("  --new %s --csv-new %s \\" % (ctx["new_file"], ctx["csv_new"]))
    if other_line:
        w(other_line)
    w("  --date %s --out %s" % (date, ctx["out_file"]))
    w("")
    w("# le brouillon est-il à jour ? LA MÊME COMMANDE, --check en plus.")
    w("python tools/133-owl-release-note.py --repo . \\")
    w("  --old %s --csv-old %s \\" % (ctx["old_file"], ctx["csv_old"]))
    w("  --new %s --csv-new %s \\" % (ctx["new_file"], ctx["csv_new"]))
    if other_line:
        w(other_line)
    w("  --date %s --out %s --check" % (date, ctx["out_file"]))
    w("```")
    w("")
    w("Ces commandes se lancent **depuis la racine du dépôt** : `--old` et `--new` sont")
    w("ouverts relativement au répertoire courant, pas à `--repo`. Sans")
    w("`--other-old`/`--other-new`, la section « ce qui n'a pas changé » perd sa ligne sur")
    w("l'autre artefact — et `--check` répond alors « brouillon perime » sur un arbre")
    w("correct.")
    w("")
    w("Le delta vient de [`tools/133-owl-regen-delta.py`](../../tools/133-owl-regen-delta.py) :")
    w("l'attendu est **calculé depuis le CSV**, la valeur constatée est lue dans le XML —")
    w("l'instrument ne peut pas se donner raison à lui-même. Le rattachement à un commit")
    w("relit le CSV à **chaque commit** qui l'a touché dans la plage : le commit d'une")
    w("transition est celui qui a changé cette cellule, pas une recherche de chaîne qui")
    w("tomberait sur la même valeur dans une autre cellule.")
    w("")
    w("**Ce contrôle n'est pas câblé en CI** : il lit l'historique du CSV (`%s`), qu'un"
      % ctx["csv_old"])
    w("checkout superficiel ne contient pas. Le câbler suppose un job à `fetch-depth: 0` —")
    w("à faire, ce n'est pas fait ici.")
    w("")
    return "\n".join(out)


def build(ctx):
    """Rassemble tout : delta, registre, chaînes de cellules, puis le texte."""
    tmp = tempfile.NamedTemporaryFile(suffix=".json", delete=False)
    tmp.close()
    try:
        argv = [sys.executable, "-I", DELTA_TOOL, "--repo", ctx["repo"],
                "--old", ctx["old_file"], "--csv-old", ctx["csv_old"],
                "--new", ctx["new_file"], "--csv-new", ctx["csv_new"],
                "--json", tmp.name]
        out = io.StringIO()
        with contextlib.redirect_stdout(out):
            done = subprocess.run(argv, capture_output=True, text=True,
                                  encoding="utf-8", errors="replace")
        if done.returncode != 0:
            # Le code de sortie de l'instrument EST le verdict : 3 IRI perdue,
            # 4 changement d'OWL sans cause CSV, 5 changement de CSV sans effet,
            # 6 clé hors oracle, 2 instrument en erreur. On le **propage** tel quel
            # au lieu de l'enfermer dans une RuntimeError (sortie 1) : la RuntimeError
            # disait « l'outil a planté » là où l'outil avait raison, et la note
            # annonçait un « code 4 » que le script ne rendait jamais.
            sys.stderr.write(done.stdout[-4000:] or "")
            sys.stderr.write(done.stderr[-2000:] or "")
            sys.stderr.write("\ninstrument de delta : sortie %d\n" % done.returncode)
            raise SystemExit(done.returncode)
        with open(tmp.name, encoding="utf-8") as fh:
            report = json.load(fh)
    finally:
        os.unlink(tmp.name)

    rows = report["rows"]
    registry = DELTA.load_registry(ctx["repo"])
    fragment_to_pk = {}
    for pk, fragment in registry.items():
        fragment_to_pk.setdefault(fragment, pk)

    ns = DELTA.parse_owl(os.path.join(ctx["repo"], ctx["old_file"]))["ns"]

    # La chaîne de chaque cellule responsable, une fois par (pk, colonne).
    cells = set()
    for row in rows:
        pk = fragment_to_pk.get(row["subject"])
        column = (row["inputs"] or [None])[0]
        if pk is None or column is None:
            print("changement sans pk ni colonne : %r" % (row,), file=sys.stderr)
            raise SystemExit(7)
        row["_pk"] = pk
        row["_column"] = column
        cells.add((pk, column))
    chains = cell_history(ctx["repo"], cells, ctx["csv_old"], ctx["csv_new"])

    # Chaque changement doit être expliqué par la chaîne de sa cellule.
    ctx["commit_cell"] = {}
    for row in rows:
        chain = chains[(row["_pk"], row["_column"])]
        if not chain:
            print("changement sans cause CSV : pk=%s %s" % (row["_pk"], row["_column"]),
                  file=sys.stderr)
            raise SystemExit(8)
        names = []
        for commit, when, subject, avant, apres in chain:
            names.append("`%s` (%s) %s" % (commit, when, subject))
        ctx["commit_cell"][(row["subject"], row["predicate"], row["lang"])] = " → ".join(names)

    by_subject = {}
    for row in rows:
        by_subject.setdefault(row["subject"], []).append(row)
    order = sorted(by_subject, key=lambda s: int(fragment_to_pk.get(s, 0) or 0))

    by_predicate = {}
    for row in rows:
        key = (row["predicate"], row["lang"])
        entry = by_predicate.setdefault(key, {"n": 0, "iris": set()})
        entry["n"] += 1
        entry["iris"].add(row["subject"])

    ctx["rows"] = rows
    ctx["registry"] = registry
    ctx["ns"] = ns
    ctx["by_subject"] = by_subject
    ctx["order"] = order
    ctx["by_predicate"] = by_predicate
    ctx["n_rows"] = len(rows)
    ctx["n_subjects"] = len(by_subject)
    ctx["unchanged"] = unchanged_lines(report, rows, ctx)
    ctx["unchanged"] += list(ctx.get("extra_unchanged", []))
    return ctx


def unchanged_lines(report, rows, ctx):
    """Ce qui n'a pas changé — lu dans le rapport, pas asserted par le script."""
    lines = []
    surface = report["surface"]
    lines.append("**Aucune IRI perdue** : la surface d'IRI passe de %d à %d (0 perdue, 0 gagnée)."
                 % (surface["old"], surface["new"]))
    lines.append("**Aucun changement d'OWL sans cause dans le CSV** (%d), aucune cause sans effet (%d), "
                 "aucune clé hors oracle (%d)."
                 % (len(report["owl_change_without_cause"]),
                    len(report["csv_change_without_effect"]),
                    len(report["uncovered_changed_keys"])))
    fr_non_lien = [r for r in rows if r["lang"] == "fr" and r["predicate"] != "seeAlso"]
    if not fr_non_lien:
        lines.append("**Aucun libellé, définition ni commentaire en français n'a changé** : "
                     "les changements français sont tous des liens (`rdfs:seeAlso`).")
    else:
        lines.append("**%d changements français portent sur autre chose qu'un lien** : %s."
                     % (len(fr_non_lien),
                        ", ".join(sorted({"`%s@%s`" % (r["predicate"], r["lang"]) for r in fr_non_lien}))))
    if ctx.get("other_old") and ctx.get("other_new"):
        old_sha, new_sha = ctx["other_old_sha"], ctx["other_new_sha"]
        if old_sha == new_sha:
            lines.append("**L'autre artefact du dossier est identique octet pour octet** (`%s`, "
                         "sha256 `%s…`) : rien à y raconter." % (ctx["other_new"], old_sha[:8]))
        else:
            lines.append("**L'autre artefact du dossier DIFFÈRE** (`%s` : `%s…` → `%s…`) : "
                         "ce brouillon ne porte que sur l'ontologie des vertus ; la comparaison de "
                         "l'autre face n'est pas faite ici."
                         % (ctx["other_new"], old_sha[:8], new_sha[:8]))
    return lines


def main(argv=None):
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--repo", default=REPO, help="racine du dépôt")
    ap.add_argument("--old", required=True, help="OWL/XML d'avant (le blob servi)")
    ap.add_argument("--new", required=True, help="OWL/XML d'après (master)")
    ap.add_argument("--csv-old", required=True, help="ref git du CSV qui a produit --old")
    ap.add_argument("--csv-new", required=True, help="ref git du CSV qui a produit --new")
    ap.add_argument("--date", required=True, help="date du brouillon (AAAA-MM-JJ)")
    ap.add_argument("--out", required=True, help="fichier du brouillon")
    ap.add_argument("--unchanged", action="append", default=[],
                    help="ligne à écrire dans la section « ce qui n'a pas changé »")
    ap.add_argument("--other-old", help="l'autre artefact avant (comparé octet pour octet)")
    ap.add_argument("--other-new", help="l'autre artefact après")
    ap.add_argument("--check", action="store_true", help="comparer au fichier du disque")
    args = ap.parse_args(argv)

    ctx = {"repo": args.repo, "old_file": args.old, "new_file": args.new,
           "csv_old": args.csv_old, "csv_new": args.csv_new, "date": args.date,
           "out_file": args.out, "extra_unchanged": list(args.unchanged)}

    # Les identités des blobs, lues sur les octets.
    for face, path in (("old", args.old), ("new", args.new)):
        full = os.path.join(args.repo, path)
        with open(full, "rb") as fh:
            raw = fh.read()
        ctx["%s_sha" % face] = hashlib.sha256(raw).hexdigest()
        ctx["%s_bytes" % face] = len(raw)
    if args.other_old and args.other_new:
        for face, path in (("other_old", args.other_old), ("other_new", args.other_new)):
            full = os.path.join(args.repo, path)
            with open(full, "rb") as fh:
                raw = fh.read()
            ctx["%s_sha" % face] = hashlib.sha256(raw).hexdigest()
            ctx["%s_bytes" % face] = len(raw)
        ctx["other_old"] = args.other_old
        ctx["other_new"] = args.other_new

    ctx = build(ctx)
    text = render(ctx)
    if args.check:
        full = os.path.join(args.repo, args.out)
        if not os.path.exists(full):
            print("brouillon absent : %s" % full, file=sys.stderr)
            return 1
        with open(full, encoding="utf-8", newline="") as fh:
            current = fh.read()
        if current == text:
            print("identique : %d octets" % len(text.encode("utf-8")))
            return 0
        print("brouillon perime : il ne decrit plus les deux blobs", file=sys.stderr)
        return 1
    full = os.path.join(args.repo, args.out)
    with open(full, "w", encoding="utf-8", newline="\n") as fh:
        fh.write(text)
    print("[ecrit] %s (%d octets)" % (full, len(text.encode("utf-8"))))
    return 0


if __name__ == "__main__":
    sys.exit(main())
