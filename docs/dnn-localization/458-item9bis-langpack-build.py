#!/usr/bin/env python3
"""Construit un pack de langue DNN (App_GlobalResources) pour #458 grain 9-bis.

Principe : on ne RECREE pas les .resx. On decoupe le fichier NEUTRE en blocs
<data>, on ne garde que les cles effectivement traduites (plus celles deja
presentes dans le fichier culturel, qui sont reportees telles quelles), et on
remplace le contenu de leur <value>. En-tete boilerplate, schema xsd, resheaders,
ordre et fins de ligne (LF) sont preserves.

Une cle NON traduite est donc ABSENTE du fichier produit : DNN retombe alors sur
le neutre anglais, cle par cle. C'est le comportement voulu pour les textes
juridiques, les pages d'erreur hote et les listes techniques (stopwords).

Usage :
  python -I langpack-build.py --lang zh-CN --translations zh.json [--root <webroot>] [--dry-run]

Controles bloquants :
  - une cle traduite absente du neutre => refus
  - une valeur vide => refus
  - multiset de placeholders {0}/{1} different du neutre => refus
  - identite des jetons DNN differente ([Portal:URL], [Custom:4|...]) => refus
  - le fichier produit doit se reparser et rendre exactement les cles attendues
  - un fichier culturel existant ne perd aucune de ses cles
"""
import argparse
import json
import os
import re
import sys
import xml.etree.ElementTree as ET

PLACEHOLDER = re.compile(r"\{\d+\}")
# jetons DNN : on ne compare que l'IDENTITE (namespace + chemin avant le '|'),
# jamais la partie affichable — dans [Custom:4|Number of errors:{0}<br />] la
# prose apres '|' EST a traduire. Une regex trop large capture aussi la prose
# entre crochets ([NOTE: ...]) et fabrique un faux echec.
TOKEN = re.compile(r"\[(?:Portal|User|Host|Profile|Membership|Custom|Date|Module|Tab|Site|Page|App):([^\]\|]*)")
DATA_BLOCK = re.compile(r'<data\s+name="([^"]+)"[^>]*>.*?</data>\n?', re.DOTALL)


def neutral_root(explicit=None):
    if explicit:
        return explicit
    env = os.environ.get("ARGUMENTUM_AGR_ROOT")
    if env:
        return env
    here = os.path.dirname(os.path.abspath(__file__))
    for _ in range(8):
        cand = os.path.join(here, "DNNPlatform", "App_GlobalResources")
        if os.path.isdir(cand):
            return cand
        here = os.path.dirname(here)
    sys.exit("ROOT introuvable : passer --root")


def read_resx(path):
    tree = ET.parse(path)
    out = {}
    for d in tree.getroot().findall("data"):
        v = d.find("value")
        out[d.get("name")] = "" if v is None or v.text is None else v.text
    return out


def esc(s):
    return s.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")


def set_block_value(block, key, value):
    pat = re.compile(r'(<data\s+name="' + re.escape(key) + r'"[^>]*>\s*)(<value[^>]*>)(.*?)(</value>)', re.DOTALL)
    m = pat.search(block)
    if not m:
        return None
    return block[:m.start(2)] + m.group(2) + esc(value) + m.group(4) + block[m.end(4):]


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--lang", required=True)
    ap.add_argument("--translations", required=True)
    ap.add_argument("--root")
    ap.add_argument("--dry-run", action="store_true")
    # Convention de BOM du fichier PRODUIT. `sig` (defaut, comportement
    # historique) : App_GlobalResources porte le BOM partout. `mirror` : le
    # fichier produit prend l'etat de BOM de SON neutre — indispensable au
    # module-level, ou le dossier melange les deux (controls/App_LocalResources :
    # Address.ascx.resx sans BOM, URLControl.ascx.resx avec). `plain` : jamais.
    ap.add_argument("--bom", choices=("sig", "plain", "mirror"), default="sig")
    a = ap.parse_args()

    root = neutral_root(a.root)
    data = json.load(open(a.translations, encoding="utf-8"))
    files = data.get("files", data)

    errors, report = [], []
    for base, tr in files.items():
        neutral_path = os.path.join(root, f"{base}.resx")
        if not os.path.isfile(neutral_path):
            sys.exit(f"CONTROLE POSITIF : neutre absent ({neutral_path}) — arret, rien n'est conclu.")
        neutral = read_resx(neutral_path)
        if len(neutral) < 2:
            sys.exit(f"CONTROLE POSITIF : {base}.resx rend {len(neutral)} cle(s) — l'instrument ne voit rien, arret.")
        neutral_shell = all(not v.strip() for v in neutral.values())

        unknown = [k for k in tr if k not in neutral]
        if unknown:
            errors.append(f"{base}: {len(unknown)} cle(s) hors neutre (ex. {unknown[:3]})")
            continue
        for k, v in tr.items():
            if not v.strip():
                errors.append(f"{base}: valeur vide pour {k}")
            if not neutral_shell and not neutral[k].strip():
                errors.append(f"{base}: {k} a un neutre vide hors coquille — rien a traduire")
            if sorted(PLACEHOLDER.findall(neutral[k])) != sorted(PLACEHOLDER.findall(v)):
                errors.append(f"{base}: placeholders differents pour {k} "
                              f"({sorted(PLACEHOLDER.findall(neutral[k]))} -> {sorted(PLACEHOLDER.findall(v))})")
            if sorted(TOKEN.findall(neutral[k])) != sorted(TOKEN.findall(v)):
                errors.append(f"{base}: jetons DNN differents pour {k} "
                              f"({sorted(TOKEN.findall(neutral[k]))} -> {sorted(TOKEN.findall(v))})")

        target = os.path.join(root, f"{base}.{a.lang}.resx")
        preserved = {}
        if os.path.isfile(target):
            preserved = read_resx(target)
            lost = [k for k in preserved if k not in neutral]
            if lost:
                errors.append(f"{base}: le fichier culturel existant porte des cles hors neutre ({lost[:3]})")

        # Precedence : la TRADUCTION gagne toujours. Une cle presente dans le
        # fichier culturel mais ABSENTE du dictionnaire de traduction est
        # preservee telle quelle (cas des textes juridiques deja traduits).
        # (Inverser cette precedence laisserait un fichier perime ecraser une
        # traduction neuve — un 0 qui ne prouve rien.)
        wanted = dict(tr)
        for k, v in preserved.items():
            if k not in wanted:
                wanted[k] = v

        raw = open(neutral_path, encoding="utf-8-sig").read()
        # L'en-tete boilerplate du format resx contient QUATRE exemples <data> dans
        # un commentaire XML (Name1, Color1, Bitmap1, Icon1). Les compter comme de
        # vraies cles double le denombrement -> on ne decoupe qu'apres le dernier
        # </resheader>, ou commencent les blocs reels.
        anchor = raw.rfind("</resheader>")
        if anchor < 0:
            errors.append(f"{base}: aucun </resheader> — format inattendu, arret")
            continue
        body_start = anchor + len("</resheader>")
        body = raw[body_start:]
        blocks = [m for m in DATA_BLOCK.finditer(body)]
        if len(blocks) != len(neutral):
            errors.append(f"{base}: {len(blocks)} blocs <data> decoupes, {len(neutral)} cles lues — decoupage non fiable")
            continue

        head = raw[:body_start] + body[:blocks[0].start()]
        tail = body[blocks[-1].end():]
        out, kept = [head], []
        for m in blocks:
            key = m.group(1)
            if key not in wanted:
                continue                      # non traduite => absente => repli neutre DNN
            blk = set_block_value(m.group(0), key, wanted[key])
            if blk is None:
                errors.append(f"{base}: bloc <value> introuvable pour {key}")
                break
            out.append(blk)
            kept.append(key)
        else:
            out.append(tail)
            produced = "".join(out)
            try:
                got = {d.get("name") for d in ET.fromstring(produced.encode("utf-8")).findall("data")}
            except ET.ParseError as e:
                errors.append(f"{base}: XML produit invalide ({e})")
                continue
            if got != set(wanted):
                errors.append(f"{base}: cles produites != cles attendues "
                              f"(manquantes {sorted(set(wanted) - got)[:3]}, en trop {sorted(got - set(wanted))[:3]})")
                continue
            report.append((base, len(kept), len(preserved), len(produced.encode("utf-8")), target))
            if not a.dry_run:
                # utf-8-sig : le BOM UTF-8 est la convention du dossier
                # App_GlobalResources (tous les .resx du neutre et des cultures
                # en portent un) ; sans lui la reecriture fait disparaitre la
                # premiere ligne du fichier. newline="" : les fins de ligne LF
                # du depot sont preservees (CRLF = bruit de diff).
                # `--bom mirror` : le BOM suit le neutre du fichier traite, pour
                # les dossiers heterogenes (module-level).
                neutral_bytes = open(neutral_path, "rb").read(3)
                neutral_has_bom = neutral_bytes == b"\xef\xbb\xbf"
                enc = {
                    "sig": "utf-8-sig",
                    "plain": "utf-8",
                    "mirror": "utf-8-sig" if neutral_has_bom else "utf-8",
                }[a.bom]
                with open(target, "w", encoding=enc, newline="") as f:
                    f.write(produced)

    for base, n, pres, size, target in report:
        print(f"{base}: {n} cles ecrites ({pres} preexistantes preservees), {size} o -> {target}")
    if errors:
        print("\nECHECS :", file=sys.stderr)
        for e in errors:
            print(" - " + e, file=sys.stderr)
        sys.exit(1)
    print(f"\nOK — {sum(r[1] for r in report)} cles ecrites pour {a.lang}")


if __name__ == "__main__":
    main()
