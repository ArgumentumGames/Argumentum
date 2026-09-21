#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1460 — charpente de la suite de regression structurelle : GARDE D'ARBRE.

    python tools/1460-structure-charpente-pilot.py [--repo .] [--json]

LECTURE SEULE. N'ecrit rien, ne genere rien, ne reclame aucun artefact.

POURQUOI CET ORGANE EXISTE, ET POURQUOI IL EST LE PREMIER.
Les trois phases de #1460 (A dimensions/page counts, B presence de contenu,
C structure) assertent toutes sur les PDF **produits**. Or un arbre `Target/` peut
etre : absent · present mais sans PDF · present, peuple, et **anterieur au corpus**.
Dans les trois cas un organe naif rendrait du vert sur un objet qui ne peut pas
porter l'etat courant — c'est la famille « regen sans clobber = stale trap »
(arbres Debug et Release INDEPENDANTS : mesurer sur celui dont la date correspond,
jamais « le premier trouve »).

C'est aussi la raison pour laquelle ce pilot tourne sur une machine **sans bundle** :
la garde doit etre executable la ou il n'y a rien a mesurer. C'est son cas nominal.

LES TROIS ETATS DE SORTIE (un verdict gradue par CONSEQUENCE, pas par nombre) :

    0  MESURABLE      au moins un arbre frais porte des PDF -> les phases peuvent asserter
    2  NON MESURABLE  aucun arbre, ou arbre sans PDF, ou arbre anterieur au corpus
    1  DIVERGENT      un arbre frais existe mais son inventaire contredit l'attendu

Un exit 2 n'est PAS un echec : c'est « je n'ai pas d'objet a mesurer, et je le dis ».
Confondre 2 et 0 est exactement le defaut que cette garde ferme. Un organe qui
n'aurait pas d'arbre doit donc ETRE ROUGE quand son gate est arme (fail-loud),
et non silencieusement vert.

CE QUE CET ORGANE N'ETABLIT PAS.
  - Il ne derive AUCUN compte attendu : c'est le metier des organes de contrat
    existants (`PdfDeckCountContractTests`, `CardSetExpectedCardCountContractTests`),
    qui gardent les comptes au CSV. ⛔ Le dupliquer ici fabriquerait un second
    referentiel qui deriverait du premier (motif de defaut connu).
  - Il ne dit rien de la JUSTESSE du contenu : fraicheur n'est pas conformite.
  - Il ne voit pas la surface de rendu : un arbre frais peut etre faux.
  - La borne de balayage est NOMMEE (profondeur 1 sous `Target/`), pas supposee
    exhaustive : un scan borne se lit sinon comme une enumeration.
"""
import argparse
import json
import os
import sys
from datetime import datetime

LANGS = ("ar", "en", "es", "fa", "fr", "pt", "ru", "zh")
TREES = (
    ("Debug", os.path.join("Generation", "Converters", "Argumentum.AssetConverter",
                           "bin", "Debug", "net9.0-windows", "Target")),
    ("Release", os.path.join("Generation", "Converters", "Argumentum.AssetConverter",
                             "bin", "Release", "net9.0-windows", "Target")),
)
DEFAULT_REPO = r"D:\Dev\Argumentum"
# borne NOMMEE : les CSV de corpus sont sous Cards/, a la racine de la taxonomie.
# `Archive/` est exclu (instantanés historiques : ils datent le passe, pas le present).
CORPUS_ROOT = "Cards"
CORPUS_EXCLUDE = ("Archive",)


def scan(root, skip_dirs=()):
    """(nb_fichiers, nb_pdf, mtime_max, extensions) — profondeur NON bornee ici,
    mais le perimetre est nomme par l'appelant (voir `inventory`)."""
    n = pdf = 0
    tmax = 0.0
    ext = {}
    for dp, dns, fns in os.walk(root):
        dns[:] = [d for d in dns if d not in skip_dirs]
        for fn in fns:
            p = os.path.join(dp, fn)
            n += 1
            e = os.path.splitext(fn)[1].lower()
            ext[e] = ext.get(e, 0) + 1
            if e == ".pdf":
                pdf += 1
            try:
                m = os.path.getmtime(p)
            except OSError:
                continue
            if m > tmax:
                tmax = m
    return n, pdf, tmax, ext


def pdf_by_lang(tree):
    out = {}
    for lg in LANGS:
        d = os.path.join(tree, lg)
        if not os.path.isdir(d):
            out[lg] = None          # langue non presente du tout
            continue
        out[lg] = scan(d)[1]
    return out


def fmt_t(ts):
    return datetime.fromtimestamp(ts).strftime("%Y-%m-%d %H:%M") if ts else "(aucun fichier)"


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--repo", default=DEFAULT_REPO)
    ap.add_argument("--json", action="store_true")
    a = ap.parse_args()
    repo = os.path.abspath(a.repo)

    # --- 1) dater le CORPUS (le referentiel de fraicheur) -------------------
    croot = os.path.join(repo, CORPUS_ROOT)
    if not os.path.isdir(croot):
        sys.exit(f"REPO INVALIDE : {croot} absent — --repo pointe sur quoi ?")
    cn, cpdf, cmax, cext = scan(croot, skip_dirs=CORPUS_EXCLUDE)
    n_csv_src = cext.get(".csv", 0)

    # --- 2) inventorier les ARBRES (les deux, ils sont independants) --------
    report, verdicts = [], []
    for label, rel in TREES:
        tree = os.path.join(repo, rel)
        if not os.path.isdir(tree):
            report.append({"cfg": label, "path": rel, "present": False,
                           "files": 0, "pdf": 0, "newest": 0.0, "languages": {}})
            verdicts.append((label, 2, "arbre ABSENT"))
            continue
        n, pdf, tmax, ext = scan(tree)
        by = pdf_by_lang(tree)
        report.append({"cfg": label, "path": rel, "present": True, "files": n,
                       "pdf": pdf, "newest": tmax, "languages": by,
                       "extensions": ext})
        if pdf == 0:
            verdicts.append((label, 2, f"arbre present, {n} fichiers, "
                                       f"MAIS 0 PDF — rien a mesurer"))
        elif tmax < cmax:
            jours = (cmax - tmax) / 86400.0
            verdicts.append((label, 2, f"arbre ANTERIEUR au corpus de {jours:.1f} j "
                                       f"({fmt_t(tmax)} < {fmt_t(cmax)}) — STALE"))
        else:
            verdicts.append((label, 0, f"frais : {pdf} PDF, {fmt_t(tmax)}"))

    # un arbre exploitable suffit ; sinon l'etat global est NON MESURABLE
    best = min((v[1] for v in verdicts), default=2)

    if a.json:
        print(json.dumps({"repo": repo, "corpus": {"files": cn, "csv": n_csv_src,
                          "newest": cmax, "newest_str": fmt_t(cmax)},
                          "trees": report,
                          "verdicts": [{"cfg": c, "code": k, "motif": m}
                                       for c, k, m in verdicts],
                          "exit": best}, ensure_ascii=False, indent=2))
        return best

    print("=== #1460 — garde d'arbre (charpente de la suite de regression) ===")
    print(f"  depot : {repo}")
    print()
    print("  CORPUS (referentiel de fraicheur)")
    print(f"    {n_csv_src} CSV sous {CORPUS_ROOT}/ (hors {', '.join(CORPUS_EXCLUDE)}) · "
          f"{cn} fichiers · plus recent {fmt_t(cmax)}")
    print()
    print("  ARBRES D'ARTEFACTS (Debug et Release sont INDEPENDANTS)")
    print(f"    {'cfg':<9}{'present':<9}{'fichiers':>9}{'pdf':>6}  {'plus recent':<18}verdict")
    for (c, k, m), inf in zip(verdicts, report):
        pres = "oui" if inf["present"] else "NON"
        print(f"    {c:<9}{pres:<9}{inf['files']:>9}{inf['pdf']:>6}  "
              f"{fmt_t(inf['newest']):<18}{m}")
    for (c, k, m), inf in zip(verdicts, report):
        if inf["present"] and inf["pdf"] == 0:
            by = inf["languages"]
            vides = [lg for lg in LANGS if by.get(lg) == 0]
            absentes = [lg for lg in LANGS if by.get(lg) is None]
            print(f"      {c} : langues presentes sans PDF = {len(vides)}"
                  f" ({', '.join(vides) or '—'}) · langues absentes = {len(absentes)}"
                  f" ({', '.join(absentes) or '—'})")
            print(f"      {c} : extensions = "
                  f"{', '.join(f'{e or '(sans)'}x{n}' for e, n in sorted(inf['extensions'].items(), key=lambda x: -x[1]))}")
    print()
    print("  ⛔ Un arbre absent ou perime ne rend PAS un organe vert : il le rend")
    print("     NON MESURABLE. Un gate ARME sans arbre frais doit ETRE ROUGE.")
    print()
    if best == 0:
        print("  VERDICT : MESURABLE — au moins un arbre frais porte des PDF.")
        print("            ⚠️ L'etat 1 (DIVERGENT) reste INATTEIGNABLE ici : aucune source")
        print("            d'attendu n'est branchee, volontairement — deriver un compte")
        print("            ici dupliquerait les organes de contrat (voir l'en-tete). Il")
        print("            s'activera quand la phase A branchera la sienne.")
    else:
        print("  VERDICT : NON MESURABLE — aucun arbre frais porteur de PDF sur cette")
        print("            machine. Les phases A/B/C ne peuvent rien asserter ici ;")
        print("            elles restent POST-TAG et inexecutables en CI (pas de Target/).")
    return best


if __name__ == "__main__":
    sys.exit(main())
