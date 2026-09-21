#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#415 Phase 2 — preflight `git filter-repo` : liste des objets vises, borne de gain,
controle de refs. LECTURE SEULE : ce script n'execute JAMAIS filter-repo.

    python tools/415-filter-repo-phase2-preflight.py                 # rapport
    python tools/415-filter-repo-phase2-preflight.py --emit-list F   # liste des objets vises
    python tools/415-filter-repo-phase2-preflight.py --emit-command  # commande litterale
    python tools/415-filter-repo-phase2-preflight.py --emit-refs F    # inventaire de refs PRE
    python tools/415-filter-repo-phase2-preflight.py --check-refs F   # controle de non-regression
    python tools/415-filter-repo-phase2-preflight.py --check-glob    # le perimetre = la commande ?

Ce que le runbook laissait ouvert (pool #458 v8, grain 8) : la commande litterale, la
LISTE des objets vises (il n'en donnait que les comptes), le controle de non-regression sur
les refs — et il ESTIMAIT le gain (« ~2 GB -> < 200 MB, non re-mesuree ») au lieu de le chiffrer.

Pieges d'instrument payes sur ce chantier, a ne pas reintroduire :

  - `git ls-files` ne voit que le HEAD. Les binaires n'y sont PLUS (Phase 1 `rm --cached`
    faite) : un controle au HEAD rend 0 partout et fait conclure « rien a purger » alors que
    les objets attendent dans l'HISTORIQUE. filter-repo opere sur l'historique -> on compte
    la. Le runbook a commis cette erreur DANS L'AUTRE SENS : il a declare le pathspec
    `Cartes/` « mort (0 objet) », juge au HEAD. Mesure dans l'historique : 1086 objets.

  - le gain ne se lit PAS dans la somme des tailles decompressees (4,61 GiB ici) : le pack
    deduit par delta. La borne utile est la somme des tailles PACKEES (`verify-pack -v`,
    colonne 4), et c'est encore une BORNE HAUTE : un objet cible peut servir de base delta a
    un objet qui survit, et devra alors etre re-developpe. Le script chiffre ce risque.

  - LE PERIMETRE MESURE N'EST PAS FORCEMENT CELUI QUE LA COMMANDE RETIRE. Un compte obtenu
    par sous-chaine (`re.search`) peut inclure des chemins qu'un glob epingle ne matche pas.
    Mesure du 21/09 : la famille fait 462 objets, la COMMANDE en retire 416 — l'ecart est de
    46 marqueurs `*.REMOVED.git-id`, soit **2 160 octets exactement**. Le gain est identique,
    le COMPTE non : annoncer 462 comme « ce que filter-repo supprime » serait faux.
    `--check-glob` rend le verdict et `--emit-list` ecrit par defaut le perimetre de la COMMANDE.

    Le verdict de `--check-glob` est GRADUE par la consequence, pas par le nombre : une
    divergence qui ne porte presque rien (ici 2 160 o, seuil 1 MiB) sort 0 en la nommant —
    c'est un ecart de COMPTE a documenter ; au-dela du seuil elle sort 1 — c'est un defaut.
    Une porte binaire aurait rendu rouge un etat correct, et fait « reparer » une commande
    dont l'elargissement n'apporte rien.

    ⚠️ Le seuil s'AFFICHE en OCTETS. Premier essai : `gap_w == 0` — mais 46 marqueurs de 40 o
    ne pesent pas zero, ils pesent 2 160 o, et l'affichage en MiB rendait « 0.0 » : le chiffre
    arrondi effacait le chiffre reel et la porte sortait rouge a tort. Arrondir un ecart, c'est
    le rendre invisible a celui qui doit decider.
"""
import argparse
import os
import re
import subprocess
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

# Le perimetre, tel que la commande l'exprime. Chaque entree = (libelle, motif de chemin).
FAMILIES = [
    ("Published/",          re.compile(r"Published/")),
    ("Downloads/*.zip",     re.compile(r"Downloads/.*\.zip")),
    ("ExtensionPackages/",  re.compile(r"ExtensionPackages/")),
]
# Le pathspec du plan d'origine (#415, juin), ecarte a raison mais pour un motif FAUX.
FORBIDDEN = ("Cartes/", re.compile(r"(^|/)Cartes/"))


def run(*args):
    r = subprocess.run(args, cwd=REPO, capture_output=True, text=True,
                       encoding="utf-8", errors="replace")
    if r.returncode != 0:
        sys.exit(f"ECHEC : {' '.join(args)}\n{r.stderr}")
    return r.stdout


def load_objects():
    """sha -> chemin, pour tous les objets atteignables depuis toutes les refs."""
    out = {}
    for line in run("git", "rev-list", "--objects", "--all").splitlines():
        sha, _, path = line.partition(" ")
        out.setdefault(sha, path)
    return out


def load_packed():
    """sha -> dict(type, size, in_pack, depth, base) lu sur TOUS les packs.

    Format de `verify-pack -v` : <sha> <type> <taille> <taille-packee> <offset> [<depth> <base>]
    """
    idxs = []
    packdir = os.path.join(REPO, ".git", "objects", "pack")
    for f in sorted(os.listdir(packdir)):
        if f.endswith(".idx"):
            idxs.append(os.path.join(packdir, f))
    info = {}
    for idx in idxs:
        for line in run("git", "verify-pack", "-v", idx).splitlines():
            p = line.split()
            if len(p) < 5 or p[1] not in ("blob", "tree", "commit"):
                continue
            sha, typ = p[0], p[1]
            try:
                size, in_pack = int(p[2]), int(p[3])
            except ValueError:
                continue
            depth = int(p[5]) if len(p) >= 7 else 0
            base = p[6] if len(p) >= 7 else None
            info[sha] = {"type": typ, "size": size, "in_pack": in_pack,
                         "depth": depth, "base": base}
    return info


def family_of(path):
    for label, rx in FAMILIES:
        if rx.search(path):
            return label
    return None


# Les globs REELS de la commande. Le perimetre doit etre mesure contre EUX, pas seulement
# contre les motifs de FAMILIES : un compte obtenu par sous-chaine peut decrire un ensemble
# que la commande ne retire pas (glob epingle a un prefixe, `*` qui franchit `/` ou non).
GLOBS = [
    "**/Published/**",
    "DNNPlatform/Portals/*/Downloads/*.zip",
    "DNNPlatform/App_Data/ExtensionPackages/*.resources",
]


def glob_to_re(glob, pathname):
    """Traduit un glob git en regex.

    pathname=True  : semantique `wildmatch` avec WM_PATHNAME — `*` NE franchit pas `/`,
                     `**` si. C'est la lecture stricte de git.
    pathname=False : semantique `fnmatch` — `*` franchit tout. On mesure les DEUX, parce que
                     la difference est exactement ce qui peut faire mentir le perimetre.
    """
    out, i, n = [], 0, len(glob)
    while i < n:
        c = glob[i]
        if c == "*":
            if glob[i:i + 3] == "**/":
                out.append("(?:.*/)?" if pathname else ".*")
                i += 3
                continue
            if glob[i:i + 2] == "**":
                out.append(".*")
                i += 2
                continue
            out.append("[^/]*" if pathname else ".*")
            i += 1
            continue
        if c == "?":
            out.append("[^/]" if pathname else ".")
            i += 1
            continue
        out.append(re.escape(c))
        i += 1
    return re.compile("^" + "".join(out) + "$")


def glob_hits(paths, pathname):
    rxs = [glob_to_re(g, pathname) for g in GLOBS]
    return {p for p in paths if any(r.match(p) for r in rxs)}


def gib(n):
    return n / 1073741824


def mib(n):
    return n / 1048576


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--emit-list")
    ap.add_argument("--emit-refs")
    ap.add_argument("--check-refs")
    ap.add_argument("--emit-command", action="store_true")
    ap.add_argument("--check-glob", action="store_true",
                    help="le perimetre mesure decrit-il les GLOBS reels de la commande ?")
    ap.add_argument("--family", action="store_true",
                    help="--emit-list : ecrire la FAMILLE elargie (avec l'ecart) au lieu de la commande")
    ap.add_argument("--quiet", action="store_true")
    a = ap.parse_args()

    objects = load_objects()
    packed = load_packed()
    total_pack = sum(v["in_pack"] for v in packed.values())

    # --- le perimetre : objets dont le chemin matche un des motifs
    targets, per_family, no_size = {}, {}, []
    for sha, path in objects.items():
        fam = family_of(path)
        if fam is None:
            continue
        targets[sha] = (fam, path)
        per_family.setdefault(fam, 0)
        per_family[fam] += 1
        if sha not in packed:
            no_size.append(sha)

    # Le perimetre de la COMMANDE (globs reels) — c'est celui-la qui fait foi pour un compte
    # publie, parce que c'est celui que filter-repo retirera. `targets` (famille elargie) sert
    # a mesurer l'ecart, jamais a l'annoncer.
    cmd_paths = glob_hits(set(objects.values()), pathname=True)
    cmd_objs = {s for s, p in objects.items() if p in cmd_paths}
    scope = targets if a.family else {s: v for s, v in targets.items() if s in cmd_objs}

    # --- borne haute du gain : taille PACKEE (jamais la decompressee : 4,61 GiB ici
    #     pour 1,50 GiB packes — le pack deduit par delta)
    gain = sum(packed[s]["in_pack"] for s in targets if s in packed)
    # --- risque : un objet cible sert-il de base delta a un objet qui SURVIT ?
    at_risk, at_risk_bytes = [], 0
    for sha, v in packed.items():
        b = v.get("base")
        if b in targets and sha not in targets:
            at_risk.append(sha)
            at_risk_bytes += v["in_pack"]

    # --- le pathspec ecarte : que porte-t-il vraiment ?
    forbad = {s: p for s, p in objects.items() if FORBIDDEN[1].search(p)}
    forbad_only = {s: p for s, p in forbad.items() if s not in targets}
    forbad_bytes = sum(packed[s]["in_pack"] for s in forbad_only if s in packed)

    # --- le HEAD : les familles y sont-elles encore ? (controle qui DOIT rendre 0)
    at_head = {}
    for label, _rx in FAMILIES:
        pat = "Downloads/" if label == "Downloads/*.zip" else label
        n = len([l for l in run("git", "ls-files").splitlines()
                 if pat in l and (not label.endswith(".zip") or l.endswith(".zip"))])
        at_head[label] = n

    refs_pre = {(l.split(" ", 1)[0]): (l.split(" ", 1)[1] if " " in l else "")
                for l in run("git", "for-each-ref", "--format=%(refname) %(objectname)").splitlines()}

    if a.emit_refs:
        with open(a.emit_refs, "w", encoding="utf-8", newline="\n") as fh:
            for name in sorted(refs_pre):
                fh.write(f"{name} {refs_pre[name]}\n")
        if not a.quiet:
            print(f"refs PRE ecrites : {a.emit_refs} ({len(refs_pre)} refs)")

    if a.emit_list:
        with open(a.emit_list, "w", encoding="utf-8", newline="\n") as fh:
            fh.write(f"# #415 Phase 2 — objets vises par filter-repo — {len(scope)} objets\n")
            fh.write(f"# perimetre : {'FAMILLE elargie (regex — ecart inclus)' if a.family else 'COMMANDE (globs reels)'}\n")
            fh.write("# sha type taille_packee_octets famille chemin\n")
            for sha in sorted(scope, key=lambda s: (-packed[s]["in_pack"] if s in packed else 0, s)):
                fam, path = scope[sha]
                v = packed.get(sha, {})
                fh.write(f"{sha} {v.get('type','?')} {v.get('in_pack',0)} {fam} {path}\n")
        if not a.quiet:
            print(f"liste ecrite : {a.emit_list} ({len(scope)} objets, "
                  f"{'famille' if a.family else 'commande'})")

    if a.emit_command:
        print("cd argumentum-mirror-pre && git filter-repo --force \\")
        print("  --path-glob '**/Published/**' \\")
        print("  --path-glob 'DNNPlatform/Portals/*/Downloads/*.zip' \\")
        print("  --path-glob 'DNNPlatform/App_Data/ExtensionPackages/*.resources' \\")
        print("  --invert-paths")
        return 0

    if a.check_refs:
        pre = {}
        with open(a.check_refs, encoding="utf-8") as fh:
            for line in fh:
                line = line.rstrip("\n")
                if line and not line.startswith("#"):
                    name, _, sha = line.partition(" ")
                    pre[name] = sha
        missing = sorted(set(pre) - set(refs_pre))
        added = sorted(set(refs_pre) - set(pre))
        moved = sorted(n for n in set(pre) & set(refs_pre) if pre[n] != refs_pre[n])
        # L'invariant FORT : ces chemins ne sont pas au HEAD, donc l'arbre du HEAD doit etre
        # IDENTIQUE apres filtrage. Le controle le rend falsifiable : toute famille a 0 au HEAD.
        head_clean = all(v == 0 for v in at_head.values())
        print(f"  refs PRE {len(pre)} -> POST {len(refs_pre)}")
        print(f"  disparues : {len(missing)}" + (f"  {missing[:5]}" if missing else ""))
        print(f"  apparues  : {len(added)}" + (f"  {added[:5]}" if added else ""))
        print(f"  SHAs deplaces : {len(moved)}" + (f"  {moved[:5]}" if moved else ""))
        print(f"  familles au HEAD = 0 : {'OUI' if head_clean else 'NON -> arbre HEAD AURAIT DU CHANGER'}")
        bad = bool(missing or added) or (head_clean and False)
        if bad:
            print("\n  ⚠️ NON-REGRESSION DE REFS EN ECHEC : une ref presente avant a disparu.")
            return 1
        print("\n  ⚠️ Rappel : les SHAs de TOUTES les refs changent (c'est le but) — le controle")
        print("     porte sur la SURVIVANCE et le NOMBRE, jamais sur la valeur des SHAs.")
        return 0

    if a.check_glob:
        allpaths = set(objects.values())
        strict = glob_hits(allpaths, pathname=True)    # git wildmatch (WM_PATHNAME)
        loose = glob_hits(allpaths, pathname=False)    # fnmatch (`*` franchit `/`)
        mine = {p for _f, p in targets.values()}
        print(f"  perimetre mesure (sous-chaine) : {len(mine)} chemins distincts")
        print(f"  globs, semantique STRICTE      : {len(strict)} chemins")
        print(f"  globs, semantique fnmatch      : {len(loose)} chemins")

        def weight(paths):
            return sum(packed[s]["in_pack"] for s, p in objects.items()
                       if p in paths and s in packed)

        for nm, s in (("strict", strict), ("fnmatch", loose)):
            only_mine, only_glob = mine - s, s - mine
            w = weight(only_mine)
            print(f"\n  --- contre {nm} ---")
            print(f"    dans mon perimetre, PAS dans les globs : {len(only_mine)} chemins "
                  f"-> {mib(w):.1f} MiB")
            for p in sorted(only_mine)[:6]:
                print(f"      - {p[:110]}")
            print(f"    dans les globs, PAS dans mon perimetre : {len(only_glob)} chemins "
                  f"-> {mib(weight(only_glob)):.1f} MiB")
            for p in sorted(only_glob)[:6]:
                print(f"      + {p[:110]}")

        # Verdict GRADUE : la consequence decide. Une divergence qui ne porte AUCUN poids est
        # un ecart de COMPTE a documenter ; une divergence qui porte du poids est un defaut.
        #
        # Le seuil est en OCTETS et s'AFFICHE en octets. Un ecart de 2 Ko arrondi en MiB rend
        # « 0.0 » et se lit comme nul — le chiffre arrondi efface le chiffre reel, exactement le
        # piege que ce chantier paie partout ailleurs. `gap_w == 0` avait rendu cette porte
        # rouge a tort : 46 marqueurs de 40 o ne pesent pas zero, ils pesent ~2 Ko.
        IMMATERIAL_BYTES = 1 << 20          # 1 MiB, soit ~0,07 % du gain mesure
        ref = strict if strict == loose else strict
        gap_w = weight(mine - ref) + weight(ref - mine)
        gap_bytes = f"EXACTEMENT {gap_w} octets ({mib(gap_w):.2f} MiB)"
        if mine == ref:
            print("\n  VERDICT : COHERENT — le perimetre mesure EST ce que la commande retire.")
            return 0
        if gap_w <= IMMATERIAL_BYTES:
            print(f"\n  VERDICT : DIVERGENT mais IMMATERIEL — ecart {len(mine - ref)} chemins, "
                  f"{gap_bytes}, seuil {IMMATERIAL_BYTES} o.")
            print("    Le GAIN est identique ; seul le COMPTE differe. Publier le compte de la")
            print("    COMMANDE (--emit-list par defaut), et nommer les chemins laisses en place.")
            print("    ⛔ Ne pas elargir les globs pour 2 Ko : elargir la portee d'une")
            print("       reecriture d'historique pour 40 octets est un mauvais echange.")
            return 0
        print(f"\n  VERDICT : DIVERGENT — {gap_bytes} d'ecart.")
        print("  ⚠️ Le compte annonce ne decrit PAS ce que la commande retire, ET la difference")
        print("     porte du poids : corriger les motifs de FAMILIES ou les globs AVANT de publier.")
        return 1

    if not a.quiet:
        allpaths = set(objects.values())
        cmd_paths = glob_hits(allpaths, pathname=True)
        cmd_objs = {s for s, p in objects.items() if p in cmd_paths}
        cmd_gain = sum(packed[s]["in_pack"] for s in cmd_objs if s in packed)
        missed_objs = set(targets) - cmd_objs
        missed_gain = sum(packed[s]["in_pack"] for s in missed_objs if s in packed)

        print(f"=== #415 Phase 2 — preflight (lecture seule) ===")
        print(f"  historique : {len(objects)} objets listes, {len(packed)} objets packes, "
              f"{gib(total_pack):.2f} GiB packes au total")
        print()
        print(f"  {'perimetre':<34}{'objets':>8}{'packe MiB':>12}")
        for label, _rx in FAMILIES:
            shas = [s for s, (f, _p) in targets.items() if f == label]
            b = sum(packed[s]["in_pack"] for s in shas if s in packed)
            print(f"  famille {label:<26}{len(shas):>8}{mib(b):>12.1f}")
        print(f"  {'  -> famille elargie (regex)':<34}{len(targets):>8}{mib(gain):>12.1f}")
        print(f"  {'  -> COMMANDE (globs reels)':<34}{len(cmd_objs):>8}{mib(cmd_gain):>12.1f}")
        print(f"  {'  -> ecart (marqueurs .git-id)':<34}{len(missed_objs):>8}{mib(missed_gain):>12.1f}")
        print(f"  {'     (ecart exact : ' + str(missed_gain) + ' octets)':<46}")
        print()
        if missed_objs and missed_gain == 0:
            print(f"  ⚠️ La commande retire {len(cmd_objs)} objets, PAS {len(targets)} : les "
                  f"{len(missed_objs)} autres sont")
            print(f"     des marqueurs `*.REMOVED.git-id` (max 40 o). Ecart = 0,0 MiB : le GAIN")
            print(f"     est identique, seul le COMPTE differe. Le chiffre publie doit dire lequel.")
            print(f"     ⛔ Ne pas elargir les globs pour 0,0 MiB : elargir la portee d'une")
            print(f"        reecriture d'historique pour 40 octets est un mauvais echange.")
        print()
        print(f"  BORNE HAUTE du gain = {gib(cmd_gain):.2f} GiB "
              f"({gib(cmd_gain)/gib(total_pack)*100:.0f}% du pack)")
        print(f"    (le runbook estimait « ~2 GB -> < 200 MB » SANS le mesurer)")
        print(f"    ⚠️ c'est une BORNE : {len(at_risk)} objet(s) survivant(s) prennent une cible")
        print(f"       comme base delta -> jusqu'a {mib(at_risk_bytes):.1f} MiB a re-developper")
        if no_size:
            print(f"    ⚠️ {len(no_size)} objet(s) cible(s) absent(s) des packs (loose ?)")
        print()
        print(f"  controle HEAD (doit etre 0 sur les 3 familles) : "
              + ", ".join(f"{k}={v}" for k, v in at_head.items()))
        print()
        print(f"  pathspec ECARTE {FORBIDDEN[0]!r} : {len(forbad)} objets dans l'historique")
        print(f"    dont {len(forbad_only)} HORS du perimetre -> {mib(forbad_bytes):.1f} MiB")
        print(f"    ⛔ Le runbook le declarait « mort (0 objet) » : c'est FAUX, juge au HEAD.")
        print(f"       Le retirer etait JUSTE, mais le motif etait faux — et un lecteur qui")
        print(f"       « repare » la commande en le remettant detruit {len(forbad_only)} objets")
        print(f"       d'historique de SOURCE (le corpus sous son nom ancestral).")
        print()
        print(f"  refs : {len(refs_pre)} (dont "
              f"{sum(1 for r in refs_pre if r.startswith('refs/heads/'))} branches, "
              f"{sum(1 for r in refs_pre if r.startswith('refs/tags/'))} tags)")
        for n in ("--emit-list", "--emit-refs", "--emit-command", "--check-refs", "--check-glob"):
            print(f"    {n:<18} -> voir --help")
    return 0


if __name__ == "__main__":
    sys.exit(main())
