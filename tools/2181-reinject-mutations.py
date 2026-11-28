#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#2181 / P24-51, P24-55 et P24-54 - batterie de mutations du controle nomme, INDEPENDANTE des gardes.

    python tools/2181-reinject-mutations.py [--work-dir <dossier>]

Chaque mutation est une reecriture textuelle de `tools/458-eav-reinject.py` appliquee a
une COPIE (outil + module d'export + suite de gardes) ; la suite ENTIERE (93 cas) est
rejouee sur la copie et doit rougir sur le cas nomme d'avance. La copie NON mutee
(temoin) doit rester 93/93 : sans ce temoin, un rouge ne prouverait rien.

TROIS SERIES. N1-N13 : l'ecrasement nomme `--force` (P24-51). N14-N29 : le mode borne
`--only-cell` (P24-55) — d'abord jouees en batterie de session non commise, elles sont
ici pour etre rejouables par qui relit ; N30-N34 : les cinq mutations de la contre-revue
po-2023 de ce mode. N35-N59 : le levier nomme de la base fr du bloc, `--fr-force`
(P24-54) — parseur, classification, perimetre, transmission, writer, garde « #2181
identique », post-contrat, rapport et fichier de resultat.

POURQUOI une batterie a part. Les mutations vivantes de la suite (ForceNomme.mutation,
WriterForce.check/block) eprouvent les HELPERS de la suite : elles etablissent que
`check()` sait voir une forme absente. Elles ne disent pas que les gardes couvrent
l'OUTIL — un cas peut passer sans rien garder, et personne ne le voit en relisant la
suite. Ici c'est l'outil qui est mute, et le cas qui doit le voir est nomme d'avance :
l'auteur des gardes ne peut pas etre le seul juge de leur pouvoir de detection.

CE QUE CELA N'ETABLIT PAS. Le script writer n'est jamais execute contre une base
(aucune base, aucune transaction) : une mutation du script est vue par les gardes de
FORME, et — depuis P24-54 — par les lignes de DECISION de la garde « #2181 identique »,
jouees sous PowerShell sur un payload factice (il faut un shell sur la machine : sans
lui, ce cas est saute et N47, N48 et N51 ne sont plus vus que de forme). Et les
aiguilles sont textuelles : si l'outil change, une aiguille absente ARRETE la
batterie en la nommant — jamais un vert par disparition.

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
    ("N20 le dry-run ne rend plus 1 sur une cellule nommee ecartee",
     [("        return 1 if (hard or noop) else 0", "        return 1 if hard else 0")],
     "test_une_cellule_nommee_ecartee_par_defaut_refuse_l_apply"),
    ("N21 l'apply ne refuse plus sur une cellule nommee ecartee",
     [('    if noop:\n        # Meme distinction qu\'au dry-run', '    if False:\n        # Meme distinction qu\'au dry-run')],
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
    # P24-55 (contre-revue po-2023 c.6093174887, §3.1 à §3.3) : les cinq mutations
    # que cette revue nommait « non vues » sur la tête d2c59224. Chacune retire UNE
    # garde du mode borne ; le cas qui la fixe est nomme d'avance.
    ("N30 le plan d'ecriture n'est plus borne (plan complet)",
     [("    writes = planned_writes(actions)",
       "    writes = planned_writes(compute_actions(CURRENT_PLAN, target, "
       "app_map_info[\"map\"], allow_preserved=args.include_preserved_2181, "
       "force=force_keys))")],
     "test_l_apply_borne_ne_transmet_au_writer_que_les_cellules_nommees"),
    ("N31 les bases fr ecrites ne sont plus bornees (plan complet)",
     [('    fr_writes = [{**a, "op": "FR_UPDATE"} for a in fr_actions if a["op"] == "FR_DRIFT"]',
       '    fr_writes = [{**a, "op": "FR_UPDATE"} for a in compute_fr_actions(CURRENT_FR_PLAN, '
       'target, app_map_info["map"], excludes) if a["op"] == "FR_DRIFT"]')],
     "test_l_apply_borne_ne_transmet_au_writer_que_les_cellules_nommees"),
    ("N32 le reglage ecarte bloque meme hors perimetre (noop)",
     [("    noop = named_cells_not_written(actions, fr_actions) if only else []",
       "    noop = named_cells_not_written(actions, fr_actions)")],
     "test_sans_only_cell_les_reglages_hors_perimetre_ne_bloquent_pas"),
    ("N33 la politique de base fr bloque meme hors perimetre (policy)",
     [("    policy = named_cells_policy(actions, fr_actions) if only else []",
       "    policy = named_cells_policy(actions, fr_actions)")],
     "test_sans_only_cell_les_reglages_hors_perimetre_ne_bloquent_pas"),
    ("N34 --force hors perimetre n'est plus refuse",
     [("    if unknown:", "    if False:")],
     "test_force_hors_perimetre_est_refuse"),
    # P24-54 : le levier nomme de la base fr du bloc, `--fr-force GUID:CHAMP`. Chaque
    # mutation retire UNE decision ; le cas qui la fixe est nomme d'avance.
    ("N35 --fr-force accepte n'importe quel nom",
     [("        if key != levier:", "        if False:")],
     "test_le_nom_faux_est_refuse_avant_toute_lecture"),
    ("N36 --fr-force mal forme accepte (GUID incomplet)",
     [("""    if len(parts) != 2 or not parts[1]:
        return None
    if not FORCE_GUID_RE.match(parts[0].upper()):
        return None""", """    if len(parts) != 2 or not parts[1]:
        return None""")],
     "test_le_nom_faux_est_refuse_avant_toute_lecture"),
    ("N37 le levier ne leve pas l'exclusion",
     [("        excludes.discard(levier)", "        pass")],
     "test_le_nom_complet_reecrit_la_base"),
    ("N38 une base nommee reste classee FR_DRIFT",
     [('            op = "FR_FORCE" if (g.upper(), field) in named else "FR_DRIFT"',
       '            op = "FR_DRIFT"')],
     "test_le_nom_complet_reecrit_la_base"),
    ("N39 toute base qui derive devient FR_FORCE",
     [('(g.upper(), field) in named else "FR_DRIFT"', 'True else "FR_DRIFT"')],
     "test_une_base_fr_nommee_hors_bloc_se_reecrit"),
    ("N40 exclue et nommee sur la meme base : plus de refus",
     [("        if key in excluded_by_name:", "        if False:")],
     "test_exclue_et_nommee_sur_la_meme_base_refuse"),
    ("N41 --fr-force hors perimetre accepte",
     [('    if named and not any((p["entity_guid"].upper(), p["field"]) in named for p in fr_plan):',
       "    if False:")],
     "test_le_levier_hors_perimetre_est_refuse"),
    ("N42 la base nommee n'est pas transmise au writer",
     [('    fr_writes += [{**a, "op": "FR_FORCE"} for a in fr_actions if a["op"] == "FR_FORCE"]',
       "    pass")],
     "test_l_apply_transmet_un_fr_force_et_sauvegarde_l_avant_avant_le_writer"),
    ("N43 fichier des valeurs d'avant d'une base nommee absent",
     [("reinject-fr-force-prior-{stamp}.json", "reinject-fr-force-aucun-{stamp}.json")],
     "test_l_apply_transmet_un_fr_force_et_sauvegarde_l_avant_avant_le_writer"),
    ("N44 la branche FR_FORCE tombe dans FR_UPDATE (sans continue)",
     [("            $frforced++\n            continue\n", "            $frforced++\n")],
     "test_une_seule_branche_fr_force_et_conforme"),
    ("N45 FR_FORCE : la valeur d'avant n'est plus verifiee (writer)",
     [('throw "FR_FORCE $guid.$field : la valeur en base n\'est plus celle du dry-run - ROLLBACK"',
       'Write-Host "FR_FORCE : valeur d\'avant differente, on ecrit quand meme"')],
     "test_une_seule_branche_fr_force_et_conforme"),
    ("N46 FR_FORCE : plusieurs rangees acceptees (writer)",
     [('if ($n -ne 1) { throw "FR_FORCE', 'if ($false) { throw "FR_FORCE')],
     "test_une_seule_branche_fr_force_et_conforme"),
    ("N47 la garde accepte n'importe quel nom de FR_FORCE",
     [('([string]$ff[0].entity_guid) -ieq "8A8636A4-069A-4692-A139-4773B161931B"',
       '([string]$ff[0].entity_guid) -ieq ([string]$ff[0].entity_guid)')],
     "test_la_garde_est_jouee_sous_powershell"),
    ("N48 la garde accepte le nom plusieurs fois",
     [("$ff.Count -eq 1 -and ", "$ff.Count -ge 1 -and ")],
     "test_la_garde_est_jouee_sous_powershell"),
    ("N49 la garde ne verifie plus la rangee reecrite",
     [("if ($vidfr -ne [int]$payload.expected_2181_vid) { throw", "if ($false) { throw")],
     "test_la_garde_est_jouee_sous_powershell"),
    ("N50 la garde ne verifie plus que la base vaut le registre",
     [('if ($null -eq $v2181 -or -not ([string]$v2181 -ceq [string]$ff[0].value)) { throw',
       "if ($false) { throw")],
     "test_la_garde_est_jouee_sous_powershell"),
    ("N51 le refus d'avant ne depend plus du nom (garde sans condition)",
     [("if (-not $named2181 -and (", "if ((")],
     "test_la_garde_est_jouee_sous_powershell"),
    ("N52 le post-contrat accepte toute base changee",
     [("    conforme = nommee if nomme else same", "    conforme = True")],
     "test_sans_fr_force_une_base_changee_est_un_echec"),
    ("N53 le post-contrat suit un FR_FORCE d'un autre nom",
     [("""    nomme = (len(ff) == 1 and str(ff[0].get("entity_guid", "")).upper() == PRESERVED_2181_GUID
             and ff[0].get("field") == PRESERVED_2181_FIELD)""",
       "    nomme = (len(ff) == 1)")],
     "test_un_fr_force_d_un_autre_nom_ne_relache_rien"),
    ("N54 le post-contrat ne compare plus la base au registre",
     [('                  and apres[0]["value"] == ff[0]["value"]\n', "\n")],
     "test_base_apres_differente_du_registre_est_un_echec"),
    ("N55 le post-contrat ne verifie plus la rangee mesuree",
     [('                  and apres[0]["value_id"] == avant[0]["value_id"]\n', "\n")],
     "test_autre_rangee_est_un_echec"),
    ("N56 le post-contrat ne verifie plus les News #2182",
     [("nommee = bool(nomme and same_2182 and len(avant) == 1", "nommee = bool(nomme and len(avant) == 1")],
     "test_une_news_2182_disparue_est_un_echec"),
    ("N57 le post-contrat accepte un plan nomme dont la base n'a pas bouge",
     [('                  and apres[0]["value"] != avant[0]["value"])', "                  and True)")],
     "test_le_nom_une_base_deja_egale_au_registre_n_est_pas_une_reecriture"),
    ("N58 le rapport porte la section du levier meme sans levier",
     [('                   | ({"fr_named": named_report} if named_report else {}),',
       '                   | ({"fr_named": named_report}),')],
     "test_sans_le_levier_le_bloc_reste_exclu"),
    ("N59 le fichier de resultat porte la cle du levier meme sans levier",
     [("    if nomme:\n", "    if True:\n")],
     "test_sans_fr_force_une_base_intacte_est_conforme"),
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