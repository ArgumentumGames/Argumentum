#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#2181 / P24-51 et P24-55 - batterie de mutations du controle nomme, INDEPENDANTE des gardes.

    python tools/2181-reinject-mutations.py [--work-dir <dossier>]

Chaque mutation est une reecriture textuelle de `tools/458-eav-reinject.py` appliquee a
une COPIE (outil + module d'export + suite de gardes) ; la suite ENTIERE (53 cas) est
rejouee sur la copie et doit rougir sur le cas nomme d'avance. La copie NON mutee
(temoin) doit rester 53/53 : sans ce temoin, un rouge ne prouverait rien.

DEUX SERIES. N1-N13 : l'ecrasement nomme `--force` (P24-51). N14-N29 : le mode borne
`--only-cell` (P24-55) — d'abord jouees en batterie de session non commise, elles sont
ici pour etre rejouables par qui relit.

POURQUOI une batterie a part. Les mutations vivantes de la suite (ForceNomme.mutation,
WriterForce.check/block) eprouvent les HELPERS de la suite : elles etablissent que
`check()` sait voir une forme absente. Elles ne disent pas que les gardes couvrent
l'OUTIL — un cas peut passer sans rien garder, et personne ne le voit en relisant la
suite. Ici c'est l'outil qui est mute, et le cas qui doit le voir est nomme d'avance :
l'auteur des gardes ne peut pas etre le seul juge de leur pouvoir de detection.

CE QUE CELA N'ETABLIT PAS. Le script writer n'est jamais execute (aucune base, aucun
PowerShell) : une mutation du script est vue par les gardes de FORME, pas par un vrai
run. Et les aiguilles sont textuelles : si l'outil change, une aiguille absente ARRETE
la batterie en la nommant — jamais un vert par disparition.

Sortie : code 0 si le temoin est vert et chaque mutation vue par son cas, 1 sinon.
"""
from __future__ import annotations

import argparse
import pathlib
import re
import subprocess
import sys
import tempfile

HERE = pathlib.Path(__file__).resolve().parent
TOOL = "458-eav-reinject.py"
TRIO = (TOOL, "458-eav-content-export.py", "test_458-eav-reinject-guards.py")

# (nom, [(aiguille, remplacement)], cas qui DOIT rougir)
MUTATIONS = [
    ("N1  tout conflit force (elif True)",
     [("elif (g.upper(), field, lang) in force:", "elif True:")],
     "test_sans_nom_le_conflit_refuse"),
    ("N2  jamais force (elif False)",
     [("elif (g.upper(), field, lang) in force:", "elif False:")],
     "test_nommee_la_cellule_passe_en_force"),
    ("N3  prefixe de GUID accepte",
     [("    if not FORCE_GUID_RE.match(parts[0].upper()):", "    if False:")],
     "test_parse_force"),
    ("N4  langue inconnue acceptee",
     [("parts[2] not in LANGS", "False")], "test_parse_force"),
    ("N5  FORCE hors du plan d'ecriture",
     [('WRITE_OPS = ("INSERT", "UPDATE", "FORCE")', 'WRITE_OPS = ("INSERT", "UPDATE")')],
     "test_nommee_la_cellule_passe_en_force"),
    ("N6  comparaison non ordinale (writer)",
     [(", [System.StringComparison]::Ordinal)", ")")],
     "test_une_seule_branche_et_conforme"),
    ("N7  UPDATE FORCE non epinglee (writer)",
     [('UPDATE TsDynDataValue SET [Value]=@v WHERE ValueId=@vid',
       'UPDATE TsDynDataValue SET [Value]=@v WHERE 1=1')],
     "test_une_seule_branche_et_conforme"),
    ("N8  lecture FORCE sans verrou (writer)",
     [("v.[Value]) FROM TsDynDataValue v WITH (UPDLOCK,HOLDLOCK)",
       "v.[Value]) FROM TsDynDataValue v")],
     "test_une_seule_branche_et_conforme"),
    ("N9  plusieurs rangees acceptees (writer)",
     [('if ($n -ne 1) { throw "FORCE', 'if ($false) { throw "FORCE')],
     "test_une_seule_branche_et_conforme"),
    ("N10 fichier des valeurs d'avant absent",
     [("reinject-force-prior-{stamp}.json", "reinject-plan-{stamp}.json")],
     "test_prior_force_sauvegarde_avant_le_writer"),
    ("N11 nom inconnu non signale",
     [("    return sorted(k for k in force_keys if k not in seen)", "    return []")],
     "test_un_autre_nom_ne_force_rien"),
    ("N12 --force mal forme accepte",
     [("""f"(langue parmi {', '.join(LANGS)})")
            return 1""", """f"(langue parmi {', '.join(LANGS)})")
            return 0""")],
     "test_cli_refuse_un_force_mal_forme_avant_toute_lecture"),
    # M1 de la verification du merger de #2373 (ai-01, c.7218159963) : ce refus retire
    # laissait 38/38 VERTS. Le comportement restait sur (catch + ROLLBACK), mais rien
    # ne fixait le refus : la suite le fixe depuis, et cette batterie le re-eprouve
    # sur une copie, independamment de l'auteur du cas.
    ("N13 refus de rangee FORCE retire (M1)",
     [('if (-not $r.Read()) { $r.Close(); throw "FORCE $guid.$field dim$dim : '
       'aucune rangee a cette dimension (base modifiee depuis le dry-run?)" }',
       "# M1 : refus de rangee absente retire")],
     "test_une_seule_branche_et_conforme"),
    # P24-55 : les deux validateurs de nom sont le MEME (FORCE_GUID_RE). Un second
    # analyseur pour la meme forme de cle derive : ce mutant le montre.
    ("N14 --only-cell sans le validateur de --force",
     [("""    if parts[2] not in LANGS and parts[2] != "fr":
        return None
    if not FORCE_GUID_RE.match(parts[0].upper()):""",
       """    if parts[2] not in LANGS and parts[2] != "fr":
        return None
    if not parts[0]:""")],
     "test_parse_only_cell_formes"),
    # P24-55 (suite) : les 15 mutations du mode borne `--only-cell`, d'abord jouees en
    # batterie de session (non commise, #2405) puis rabattues ici pour etre rejouables.
    # Chacune retire UNE garde ; le cas nomme est celui qui la fixe.
    ("N15 --only-cell ne borne plus les traductions",
     [("    plan, fr_plan = CURRENT_PLAN, CURRENT_FR_PLAN\n    if only:",
       "    plan, fr_plan = CURRENT_PLAN, CURRENT_FR_PLAN\n    if False:")],
     "test_le_perimetre_borne_leve_le_blocage_et_ne_garde_que_ses_cellules"),
    ("N16 scope_plan garde toutes les traductions",
     [('    kept = [p for p in plan if (p["entity_guid"].upper(), p["field"], p["lang"]) in only]',
       "    kept = list(plan)")],
     "test_scope_plan_borne_et_garde_les_bases_fr_nommees"),
    ("N17 --only-cell ne borne plus les bases fr",
     [('    kept_fr = [p for p in fr_plan if (p["entity_guid"].upper(), p["field"], "fr") in only]',
       "    kept_fr = list(fr_plan)")],
     "test_une_base_fr_hors_perimetre_n_est_pas_comparee"),
    ("N18 un nom inconnu n'est plus refuse",
     [("        if inconnues:", "        if False:")],
     "test_un_nom_absent_du_plan_est_refuse_avant_toute_lecture"),
    ("N19 un nom --only-cell mal forme n'est plus refuse",
     [("        key = parse_only_cell(item)\n        if key is None:",
       "        key = parse_only_cell(item)\n        if False:")],
     "test_un_nom_mal_forme_est_refuse"),
    ("N20 le dry-run ne bloque plus sur une cellule nommee ecartee",
     [("        blocked = conflicts or missing or fr_missing or noop",
       "        blocked = conflicts or missing or fr_missing")],
     "test_une_cellule_nommee_ecartee_par_defaut_refuse_l_apply"),
    ("N21 l'apply ne bloque plus sur une cellule nommee ecartee",
     [('    if conflicts or missing or fr_missing or noop:\n        print("REFUS d\'appliquer',
       '    if conflicts or missing or fr_missing:\n        print("REFUS d\'appliquer')],
     "test_une_cellule_nommee_ecartee_par_defaut_refuse_l_apply"),
    ("N22 FR_EXCLUDED redevient un refus du dry-run",
     [('SCOPED_NO_WRITE_OPS = ("SKIP_PRESERVED_2181", "FR_DB_LOCAL")',
       'SCOPED_NO_WRITE_OPS = ("SKIP_PRESERVED_2181", "FR_DB_LOCAL", "FR_EXCLUDED")')],
     "test_une_base_fr_nommee_exclue_est_montree_a_blanc_et_refusee_a_l_apply"),
    ("N23 FR_DB_LOCAL n'est plus une op sans ecriture",
     [('SCOPED_NO_WRITE_OPS = ("SKIP_PRESERVED_2181", "FR_DB_LOCAL")',
       'SCOPED_NO_WRITE_OPS = ("SKIP_PRESERVED_2181",)')],
     "test_une_reference_file_nommee_est_refusee"),
    ("N24 SKIP_PRESERVED_2181 n'est plus une op sans ecriture",
     [('SCOPED_NO_WRITE_OPS = ("SKIP_PRESERVED_2181", "FR_DB_LOCAL")',
       'SCOPED_NO_WRITE_OPS = ("FR_DB_LOCAL",)')],
     "test_une_cellule_nommee_ecartee_par_defaut_refuse_l_apply"),
    ("N25 la politique de base fr n'est plus montree",
     [('SCOPED_POLICY_OPS = ("FR_EXCLUDED",)', "SCOPED_POLICY_OPS = ()")],
     "test_une_base_fr_nommee_exclue_est_montree_a_blanc_et_refusee_a_l_apply"),
    ("N26 la politique de base fr ne refuse plus l'apply",
     [('    if policy:\n        print("REFUS d\'appliquer: cellule(s) nommee(s)',
       '    if False:\n        print("REFUS d\'appliquer: cellule(s) nommee(s)')],
     "test_la_passe_de_onze_cellules_de_w2_09_se_joue_a_blanc"),
    ("N27 le verdict annonce pret a appliquer malgre une exclusion",
     [("        elif policy:", "        elif False:")],
     "test_la_passe_de_onze_cellules_de_w2_09_se_joue_a_blanc"),
    ("N28 --only-cell accepte une langue inconnue",
     [('    if parts[2] not in LANGS and parts[2] != "fr":', "    if False:")],
     "test_parse_only_cell_formes"),
    ("N29 --only-cell ne normalise pas le GUID en majuscules",
     [("""    if parts[2] not in LANGS and parts[2] != "fr":
        return None
    if not FORCE_GUID_RE.match(parts[0].upper()):
        return None
    return (parts[0].upper(), parts[1], parts[2])""",
       """    if parts[2] not in LANGS and parts[2] != "fr":
        return None
    if not FORCE_GUID_RE.match(parts[0].upper()):
        return None
    return (parts[0], parts[1], parts[2])""")],
     "test_parse_only_cell_formes"),
]


def build(work: pathlib.Path, tag: str, replacements) -> pathlib.Path:
    d = work / tag
    if d.exists():
        raise SystemExit(f"REFUS: {d} existe deja — donner un --work-dir neuf")
    d.mkdir(parents=True)
    for f in TRIO:
        (d / f).write_bytes((HERE / f).read_bytes())
    src = (d / TOOL).read_text(encoding="utf-8")
    for needle, repl in replacements:
        if src.count(needle) < 1:
            # Aiguille disparue = l'outil a change depuis l'ecriture de la mutation :
            # on s'arrete en la nommant plutot que de rendre une batterie muette.
            raise SystemExit(f"REFUS: aiguille absente dans {tag}: {needle[:60]!r}")
        src = src.replace(needle, repl)
    (d / TOOL).write_text(src, encoding="utf-8")
    return d


def run(d: pathlib.Path):
    p = subprocess.run([sys.executable, "-I", str(d / TRIO[2])],
                       capture_output=True, text=True, encoding="utf-8", errors="replace")
    out = p.stdout + p.stderr
    m = re.search(r"Ran (\d+) tests", out)
    failed = re.findall(r"^(?:FAIL|ERROR): (\S+)", out, re.M)
    return p.returncode, (m.group(1) if m else "?"), failed


def main(argv=None) -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--work-dir", default=None,
                    help="ou ecrire les copies (defaut: un dossier temporaire, hors depot)")
    args = ap.parse_args(argv)
    work = pathlib.Path(args.work_dir or tempfile.mkdtemp(prefix="reinject-mutations-"))
    work.mkdir(parents=True, exist_ok=True)

    rc, n, failed = run(build(work, "temoin", []))
    print(f"TEMOIN (copie non mutee) : rc={rc}, {n} cas, rouges={failed or 'aucun'}")
    if rc != 0 or failed:
        print("VERDICT: temoin rouge — la batterie ne prouve rien, on s'arrete")
        return 1
    unseen = []
    for name, repl, expected in MUTATIONS:
        rc, n, failed = run(build(work, name.split()[0], repl))
        seen = expected in failed
        if not seen:
            unseen.append(name)
        print("%-46s rc=%d rouges=%d %s%s" % (
            name, rc, len(failed),
            "VU par " + expected if seen else "NON VU (attendu %s)" % expected,
            "" if seen else " | rouges: %s" % failed))
    print(f"\n{len(MUTATIONS) - len(unseen)}/{len(MUTATIONS)} mutations vues par leur cas")
    print("sorties : " + str(work))
    print("VERDICT: " + ("chaque mutation est vue par son cas" if not unseen else
                         f"{len(unseen)} mutation(s) non vue(s) : " + " ; ".join(unseen)))
    return 1 if unseen else 0


if __name__ == "__main__":
    sys.exit(main())