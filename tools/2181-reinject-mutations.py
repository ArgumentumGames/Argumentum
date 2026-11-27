#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#2181 / P24-51 - batterie de mutations du controle nomme, INDEPENDANTE des gardes.

    python tools/2181-reinject-mutations.py [--work-dir <dossier>]

Chaque mutation est une reecriture textuelle de `tools/458-eav-reinject.py` appliquee a
une COPIE (outil + module d'export + suite de gardes) ; la suite ENTIERE (38 cas) est
rejouee sur la copie et doit rougir sur le cas nomme d'avance. La copie NON mutee
(temoin) doit rester 38/38 : sans ce temoin, un rouge ne prouverait rien.

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