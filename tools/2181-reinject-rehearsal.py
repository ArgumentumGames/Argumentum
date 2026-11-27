#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#2181 / P24-51 - repeter HORS BASE un plan de re-injection, entre deux registres.

    python tools/2181-reinject-rehearsal.py --before <registre d'avant.csv> --after <registre d'apres.csv>

POURQUOI. `tools/458-eav-reinject.py` ne tourne en vrai que depuis un poste qui lit la
base servie. Ici la base est tenue par un registre : la cible synthetique porte les
valeurs de --before (ce que la base est censee porter ENCORE), le plan est construit sur
--after. Trois passes de la vraie main() ; seuls la base (EXP.run_query,
read_connection_string) et le writer (un temoin : aucun PowerShell n'est lance) sont
remplaces.

  1. sans --force  : la classification ; les CONFLICT sont listes.
  2. --force sur chaque conflit ATTENDU (ensemble derive des deux registres, pas du
     resultat de la passe 1, pas tape) : tous doivent passer en FORCE, rien d'autre bouge.
  3. --apply nomme : le writer recoit exactement les FORCE ; le fichier des valeurs
     d'avant dit, pour chacune, la cellule de --before puis celle de --after.

L'ORACLE EST LA PAIRE DE REGISTRES, jamais la passe 1 : le conflit attendu est compte
sur les CSV (non-vide des deux cotes, differents), et la classification de l'outil est
comparee a ce compte. Un outil qui classerait mal est vu, pas recopie.

CE QUE CELA N'ETABLIT PAS. L'etat reel de la base : si elle porte autre chose que
--before, le vrai dry-run classera autrement — le mode a blanc sur la base servie
reste la mesure (W2-11). Le script writer : il est remplace, ses formes sont gardees
par tools/test_458-eav-reinject-guards.py.

Sortie : code 0 si les trois passes sont coherentes avec l'oracle, 1 sinon.
"""
from __future__ import annotations

import argparse
import contextlib
import csv
import importlib.util
import io
import json
import pathlib
import sys

HERE = pathlib.Path(__file__).resolve().parent
TOOL = HERE / "458-eav-reinject.py"


def load_tool():
    spec = importlib.util.spec_from_file_location("eav_reinject", TOOL)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def read_registry(path: pathlib.Path) -> list[dict]:
    with path.open("r", encoding="utf-8-sig", newline="") as h:
        return list(csv.DictReader(h))


def expected_conflicts(before: list[dict], after: list[dict], langs: list[str]) -> dict:
    """L'oracle : (GUID, champ, langue) -> (valeur d'avant, valeur d'apres) pour chaque
    cellule NON VIDE des deux cotes et DIFFERENTE. Une cellule vide d'un cote n'est pas
    un conflit (INSERT ou rien) : l'outil n'a rien a ecraser."""
    old = {(r["entity_guid"].upper(), r["field"]): r for r in before}
    out = {}
    for r in after:
        for lang in langs:
            new_val = r.get(lang) or ""
            old_val = (old.get((r["entity_guid"].upper(), r["field"])) or {}).get(lang) or ""
            if new_val and old_val and new_val != old_val:
                out[(r["entity_guid"].upper(), r["field"], lang)] = (old_val, new_val)
    return out


def synthetic_rows(rows: list[dict], langs: list[str]) -> tuple[list[dict], list[dict]]:
    """Le registre comme la base le tiendrait : une rangee de valeur par cellule non
    vide (dim 0 pour le fr, 1..n pour les langues), et les seules rangees dim-0 a part."""
    eid, vid, values, base_fr = {}, 0, [], []
    for r in rows:
        app, g = int(r["app_id"]), r["entity_guid"].upper()
        eid.setdefault((app, g), 100 + len(eid))
        for lang in ["fr"] + list(langs):
            val = r.get(lang) or ""
            if val == "":
                continue
            vid += 1
            values.append({"app": app, "entity_guid": g, "entity_id": eid[(app, g)],
                           "field": r["field"],
                           "dimension_id": None if lang == "fr" else 1 + langs.index(lang),
                           "vid": vid, "val": val})
            if lang == "fr":
                base_fr.append({"app": app, "g": g, "f": r["field"], "val": val})
    values.sort(key=lambda x: (x["app"], x["entity_guid"], x["field"],
                               x["dimension_id"] if x["dimension_id"] is not None else 0,
                               x["vid"]))
    return values, base_fr


def main(argv=None) -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--before", required=True,
                    help="registre d'AVANT (ce que la cible est censee porter encore)")
    ap.add_argument("--after", required=True, help="registre d'APRES (le plan a injecter)")
    ap.add_argument("--work-dir", default=None,
                    help="ou ecrire les sorties (defaut: un dossier temporaire, hors depot)")
    args = ap.parse_args(argv)

    mod = load_tool()
    langs = list(mod.EXP.LANGS)
    cultures = dict(mod.EXP.DIM_EXTERNAL_KEY)
    before, after = read_registry(pathlib.Path(args.before)), read_registry(pathlib.Path(args.after))
    expected = expected_conflicts(before, after, langs)
    values, base_fr = synthetic_rows(before, langs)
    apps = sorted({int(r["app_id"]) for r in before})

    def fake_query(conn, sql):
        """La base tenue par le registre d'avant. Tout SQL inattendu leve : jamais de
        rangees fausses servies en silence."""
        if "HAVING COUNT(DISTINCT e.EntityId) > 1" in sql:
            return []
        if "e.EntityId AS eid" in sql or "e.EntityId AS id" in sql:
            return []
        if "FROM TsDynDataApp" in sql:
            return [{"app": a, "zone": 3} for a in apps]
        if "FROM TsDynDataDimension" in sql:
            return [{"id": 1 + i, "ext": cultures[lang], "zone": 3} for i, lang in enumerate(langs)]
        if "ORDER BY app, entity_guid, field, dimension_id, vid" in sql:
            return values
        if "v.ValueId AS vid" in sql:      # sonde #2181 (le grand SELECT est pris plus haut)
            return [{"vid": 900001, "val": "bloc #2181 (synthetique)"}]
        if "ct.Name IN (" in sql and "vd.ValueId IS NULL" in sql:
            return base_fr
        raise AssertionError("SQL inattendu sous faux run_query:\n" + sql[:240])

    mod.EXP.run_query = fake_query
    mod.EXP.read_connection_string = lambda p: "synthetique"
    writer_calls: list[dict] = []

    def fake_writer(conn, writes, fr_writes, pres_before, backup_dir, stamp, app_map):
        writer_calls.append({"writes": writes, "backup_dir": pathlib.Path(backup_dir)})
        return 0

    mod.run_writer = fake_writer
    # Les actions classées sont lues à la source — autour de compute_actions, que main()
    # appelle — et JAMAIS dépouillées dans le rapport : celui-ci ne liste que les 15
    # premiers conflits, un plan qui en aurait plus serait lu tronqué.
    seen: dict = {}
    real_compute = mod.compute_actions

    def recording_compute(plan, target, app_map, allow_preserved=False, force=None):
        acts = real_compute(plan, target, app_map, allow_preserved=allow_preserved, force=force)
        seen["actions"] = acts
        return acts

    mod.compute_actions = recording_compute
    import tempfile
    work = pathlib.Path(args.work_dir or tempfile.mkdtemp(prefix="reinject-rehearsal-"))
    work.mkdir(parents=True, exist_ok=True)

    def run(label, extra):
        writer_calls.clear()
        out = io.StringIO()
        with contextlib.redirect_stdout(out):
            rc = mod.main(["--registry", args.after, "--fr-report", str(work / "fr.json")] + extra)
        text = out.getvalue()
        (work / f"passe-{label}.txt").write_text(text, encoding="utf-8")
        counts = {}
        for line in text.splitlines():
            head, sep, tail = line[2:].partition(": ")
            if line.startswith("  ") and not line.startswith("   ") and sep and tail.isdigit():
                counts[head] = int(tail)
        return rc, text, counts

    def by_op(op: str) -> dict:
        return {(a["entity_guid"].upper(), a["field"], a["lang"]): a
                for a in seen.get("actions", []) if a["op"] == op}

    fails: list[str] = []

    def check(name, cond):
        if not cond:
            fails.append(name)
        print("  [%s] %s" % ("OK" if cond else "ECART", name))

    keys = sorted(expected)
    print(f"registres : avant {len(before)} lignes, apres {len(after)} lignes | "
          f"cible synthetique : {len(values)} rangees de valeur, {len(apps)} app(s)")
    print(f"oracle (les deux CSV) : {len(keys)} conflit(s) attendu(s)")
    for g, f, lang in keys:
        print(f"  - {g} {f} [{lang}] avant={len(expected[(g, f, lang)][0])}c "
              f"apres={len(expected[(g, f, lang)][1])}c")

    print("\nPasse 1 — sans --force")
    rc1, text1, c1 = run("1-sans-force", [])
    print("  " + " | ".join(f"{k} {v}" for k, v in sorted(c1.items())))
    conflicts = by_op("CONFLICT")
    check("passe 1 : les CONFLICT de l'outil sont exactement ceux de l'oracle",
          set(conflicts) == set(keys) and c1.get("CONFLICT", 0) == len(keys))
    check("passe 1 : pour chacun, valeur cible = avant, valeur du registre = apres (octets)",
          all((a["db_value"], a["value"]) == expected[k] for k, a in conflicts.items()))
    check("passe 1 : code 1 et ECHEC s'il y a des conflits, sinon code 0",
          (rc1 == 1 and "ECHEC" in text1) if keys else rc1 == 0)

    force_args = [x for (g, f, lang) in keys for x in ("--force", f"{g}:{f}:{lang}")]
    print("\nPasse 2 — chaque conflit attendu, nomme")
    rc2, text2, c2 = run("2-force", force_args)
    print("  " + " | ".join(f"{k} {v}" for k, v in sorted(c2.items())))
    forced = by_op("FORCE")
    check("passe 2 : un FORCE par conflit attendu, aucun autre, plus aucun CONFLICT",
          set(forced) == set(keys) and c2.get("FORCE", 0) == len(keys) and "CONFLICT" not in c2)
    check("passe 2 : chaque FORCE porte avant -> apres de l'oracle (octets)",
          all((a["db_value"], a["value"]) == expected[k] for k, a in forced.items()))
    check("passe 2 : rien d'autre ne bouge (meme comptes hors CONFLICT/FORCE)",
          {k: v for k, v in c1.items() if k != "CONFLICT"} == {k: v for k, v in c2.items() if k != "FORCE"})
    check("passe 2 : plan coherent, code 0", rc2 == 0 and "plan coherent" in text2)

    print("\nPasse 3 — --apply nomme (writer remplace, aucune base)")
    backup = work / "backup"
    rc3, text3, c3 = run("3-apply", force_args + ["--apply", "--backup-dir", str(backup)])
    writes = writer_calls[0]["writes"] if writer_calls else []
    check("passe 3 : le writer est appele une fois, code 0", len(writer_calls) == 1 and rc3 == 0)
    check("passe 3 : il ne recoit QUE les conflit attendus, tous en FORCE",
          len(writes) == len(keys) and all(w["op"] == "FORCE" for w in writes)
          and {(w["entity_guid"].upper(), w["field"], w["lang"]) for w in writes} == set(keys))
    priors = sorted(backup.glob("reinject-force-prior-*.json")) if keys else []
    if keys:
        doc = json.loads(priors[0].read_text(encoding="utf-8")) if priors else []
        check("passe 3 : le fichier des valeurs d'avant dit avant -> apres, aux octets",
              len(doc) == len(keys) and all(
                  (d["entity_guid"].upper(), d["field"], d["lang"]) in expected
                  and (d["prior_value"], d["new_value"])
                  == expected[(d["entity_guid"].upper(), d["field"], d["lang"])]
                  for d in doc))
    else:
        check("passe 3 : aucun fichier d'avant sans ecrasement", not list(backup.glob("reinject-force-prior-*.json")))

    print(f"\nsorties : {work}")
    print("VERDICT: " + ("coherent avec l'oracle" if not fails else
                         f"{len(fails)} ecart(s) : " + " ; ".join(fails)))
    return 1 if fails else 0


if __name__ == "__main__":
    sys.exit(main())
