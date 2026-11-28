#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""La page lisible des ontologies — `docs/ontology/index.html` (#133, P24-46).

    python tools/133-ontology-index-page.py             # ecrit la page
    python tools/133-ontology-index-page.py --check     # la page est-elle a jour ?
    python tools/133-ontology-index-page.py --root <dossier> --check

POURQUOI CE FICHIER EXISTE. #133 demande, pour clore, « une page lisible en ligne ».
Jusqu'ici, le dossier `docs/ontology/` n'avait qu'un README : un fichier OWL de 6 Mo
n'est pas lisible par un humain, et le README est un document de travail (mesures,
dates, arbitrages) autant qu'une page publiee. Cette page est l'index publie du
dossier : elle dit ce qui est servi, sous quel IRI, en quelle version, et avec
quels octets.

CE QUE LA PAGE DIT, ET D'OUIN. Tout est lu dans les FICHIERS DU DOSSIER : l'IRI de
l'ontologie, son titre, sa version, son auteur, sa licence et sa description sont lus
dans l'en-tete XML du `.owl` ; la taille et le sha256 sont calcules sur les octets lus.
Aucun nombre n'est recopie d'un document : une regeneration de l'OWL change la page,
et `--check` la voit. Une annotation absente de l'en-tete n'est pas inventee : la ligne
correspondante n'apparait pas.

DETERMINISME. Aucune date, aucun horodatage : les memes octets donnent la meme page,
octet pour octet. C'est ce qui permet d'exiger `diff 0` au rejeu — sans quoi la page
ne serait qu'un fichier ecrit une fois.

LE CONTROLE. `--check` regenere la page et la compare a celle du disque : sortie 0
si identiques, sortie 1 (et le diff) sinon. Il ne se fie jamais au fichier de la
page : il le compare a une regeneration.

LIMITES. La page decrit ce que le dossier contient, pas ce que l'hote sert ; le
service effectif se verifie par `tools/133-pages-owl-smoke.sh`. Les IRI d'espace de
noms ne resolvent pas vers un document (fait mesure, dit au §6 du README).
"""
import argparse
import difflib
import hashlib
import html
import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.dirname(HERE)
FOLDER = "docs/ontology"
PAGE = "index.html"

# L'annotation est cherchee par son IRI complet : un nom local separe dans le
# texte n'est pas une reponse (le fichier en contient beaucoup).
ANNOTATION = {
    "title": "http://purl.org/dc/terms/title",
    "version": "http://www.w3.org/2002/07/owl#versionInfo",
    "creator": "http://purl.org/dc/terms/creator",
    "description": "http://www.w3.org/2000/01/rdf-schema#comment",
    "license": "http://purl.org/dc/terms/license",
}

# Un meme sens peut etre porte par deux IRIs (l'autour est en `dc:terms` dans un
# fichier, en `dc:elements` dans l'autre) : on les essaie dans l'ordre.
FALLBACK = {
    "creator": ("http://purl.org/dc/elements/1.1/creator",),
}


def _annotation(text, iri):
    """La valeur d'une annotation de l'en-tete — vide si elle n'y est pas.

    Deux formes coexistent dans les `.owl` :

      - `<Annotation>…<AnnotationProperty IRI="…"/><Literal>v</Literal></Annotation>`
        — l'en-tete de l'ontologie : une seule valeur ;
      - `<AnnotationAssertion>…<AnnotationProperty IRI="…"/><IRI>sujet</IRI>
        <IRI>objet</IRI></AnnotationAssertion>` — l'annotation porte sur un sujet ;
        la valeur est alors l'**objet**, le dernier element, pas le premier.

    On prend donc la DERNIERE valeur de la portee de l'annotation : le premier
    element serait le sujet d'une `AnnotationAssertion` (mesure : la licence
    sortait l'IRI de l'ontologie au lieu de son URL).
    """
    marker = '<AnnotationProperty IRI="%s"' % iri
    start = text.find(marker)
    if start < 0:
        return ""
    fins = [p for p in (text.find("</Annotation>", start),
                        text.find("</AnnotationAssertion>", start)) if p > 0]
    if not fins:
        return ""
    portee = text[start:min(fins)]
    valeurs = re.findall(r"<(?:Literal|IRI)(?:\s[^>]*)?>([^<]*)</", portee)
    return valeurs[-1].strip() if valeurs else ""


def read_artefact(path):
    """Les metadonnees d'un `.owl` : ce que la page affiche, lu dans le fichier."""
    with open(path, "rb") as fh:
        raw = fh.read()
    text = raw.decode("utf-8", "replace")
    m = re.search(r'ontologyIRI="([^"]+)"', text)
    description = _annotation(text, ANNOTATION["description"])
    # La description retenue est la PREMIERE phrase : le fichier en porte une
    # seconde, technique, qui n'a rien a faire sur une page d'accueil.
    sentence = description.split(". ")[0].strip()
    if sentence and not sentence.endswith("."):
        sentence += "."
    return {
        "iri": m.group(1) if m else "",
        "title": _annotation(text, ANNOTATION["title"]),
        "version": _annotation(text, ANNOTATION["version"]),
        "creator": _creator(text),
        "license": _annotation(text, ANNOTATION["license"]),
        "description": sentence,
        "size": len(raw),
        "sha256": hashlib.sha256(raw).hexdigest(),
    }


def _creator(text):
    """L'auteur, sous l'IRI qu'il porte reellement — les deux senses sont essayes."""
    for iri in (ANNOTATION["creator"],) + FALLBACK.get("creator", ()):
        value = _annotation(text, iri)
        if value:
            return value
    return ""



def artefacts(folder):
    """Les `.owl` du dossier, tries — les canoniques d'abord, puis l'instantane."""
    found = []
    for dirpath, dirnames, filenames in os.walk(folder):
        dirnames.sort()
        for name in sorted(filenames):
            if name.endswith(".owl"):
                full = os.path.join(dirpath, name)
                rel = os.path.relpath(full, folder).replace(os.sep, "/")
                found.append((rel, full))
    return sorted(found)


def _size_h(size):
    for unit in ("o", "Ko", "Mo", "Go"):
        if size < 1024 or unit == "Go":
            return ("%d %s" % (size, unit)) if unit == "o" else ("%.1f %s" % (size, unit))
        size /= 1024.0
    return "%d o" % size


def _artefact_html(rel, meta, level=3):
    e = html.escape
    out = [
        '<section class="artefact">',
        "  <h%d>%s</h%d>" % (level, e(meta["title"] or rel), level),
        "  <p class=\"desc\">%s</p>" % e(meta["description"]),
        "  <dl>",
        "    <dt>File</dt><dd><code>%s</code></dd>" % e(rel),
        "    <dt>Namespace IRI</dt><dd><code>%s</code></dd>" % e(meta["iri"]),
        "    <dt>Version</dt><dd><code>%s</code></dd>" % e(meta["version"]),
    ]
    # Une annotation absente de l'en-tete n'est pas inventee : pas de ligne.
    if meta["creator"]:
        out.append("    <dt>Creator</dt><dd>%s</dd>" % e(meta["creator"]))
    if meta["license"]:
        out.append("    <dt>Licence</dt><dd><code>%s</code></dd>" % e(meta["license"]))
    out += [
        "    <dt>Size</dt><dd>%s (%d bytes)</dd>" % (_size_h(meta["size"]), meta["size"]),
        "    <dt>SHA-256</dt><dd><code>%s</code></dd>" % e(meta["sha256"]),
        "    <dt>Download</dt><dd><a href=\"%s\">%s</a></dd>" % (e(rel), e(rel)),
        "  </dl>",
        "</section>",
    ]
    return out


def render(folder):
    """La page, derivee des fichiers du dossier. Memes octets ⇒ meme page."""
    e = html.escape
    items = artefacts(folder)
    current = [(rel, full) for rel, full in items if "/" not in rel]
    snapshot = [(rel, full) for rel, full in items if "/" in rel]
    out = [
        "<!DOCTYPE html>",
        '<html lang="en">',
        "<head>",
        '  <meta charset="utf-8">',
        '  <meta name="viewport" content="width=device-width, initial-scale=1">',
        "  <title>Argumentum — OWL ontologies</title>",
        "  <style>",
        "    :root { color-scheme: light dark; }",
        "    body { font-family: system-ui, sans-serif; margin: 2rem auto; max-width: 46rem;",
        "           line-height: 1.5; padding: 0 1rem; }",
        "    h1 { font-size: 1.6rem; } h2 { font-size: 1.2rem; margin-top: 2rem; }",
        "    h3 { font-size: 1rem; margin-bottom: 0.25rem; }",
        "    .desc { margin-top: 0; color: #666; }",
        "    dl { display: grid; grid-template-columns: 9rem 1fr; gap: 0.15rem 1rem; }",
        "    dt { font-weight: 600; } dd { margin: 0; overflow-wrap: anywhere; }",
        "    code { font-size: 0.85em; }",
        "    footer { margin-top: 3rem; font-size: 0.8rem; color: #666; }",
        "  </style>",
        "</head>",
        "<body>",
        "  <h1>Argumentum — OWL ontologies</h1>",
        "  <p>The ontologies of the Argumentum fallacy and virtue taxonomy, published as",
        "  OWL/XML. This page is the human-readable index of this folder: which file,",
        "  under which namespace IRI, at which version, and with which bytes.</p>",
        '  <p lang="fr">Les ontologies de la taxonomie des sophismes et des vertus',
        "  d'Argumentum, publiees en OWL/XML. Cette page est l'index lisible de ce qui",
        "  est servi : quel fichier, sous quel IRI d'espace de noms, en quelle version,",
        "  et avec quels octets.</p>",
    ]
    out.append("  <h2>Current artefacts</h2>")
    if not current:
        out.append("  <p>No OWL file in this folder.</p>")
    for rel, full in current:
        out += _artefact_html(rel, read_artefact(full))
    if snapshot:
        out.append("  <h2>Versioned snapshot — %s</h2>" % e(snapshot[0][0].split("/")[0]))
        out.append("  <p>Files kept under the version they were generated with. The")
        out.append("  current generation is the one above; the snapshot is kept for")
        out.append("  comparison, not as a reference.</p>")
        for rel, full in snapshot:
            out += _artefact_html(rel, read_artefact(full))
    out += [
        "  <h2>Notes</h2>",
        "  <ul>",
        "    <li>The namespace IRI identifies the ontology; it does not resolve to a",
        "    document. The files are fetched by URL — the served URLs and their",
        '    <code>Content-Type</code> are in the <a href="README.md">README</a>, §6.</li>',
        "    <li>The licence of the corpus files is CC BY-SA 4.0; the code that produces",
        '    them is LGPL-3.0 (see <a href="../../LICENSE-CONTENT.md">LICENSE-CONTENT.md</a>).</li>',
        "    <li>This page is generated from the files of the folder by",
        "    <code>tools/133-ontology-index-page.py</code>. It carries no date, so that",
        "    replaying the script yields the same bytes;",
        "    <code>python tools/133-ontology-index-page.py --check</code> fails when the",
        "    page no longer matches the files it describes.</li>",
        "  </ul>",
        "  <footer>Generated by <code>tools/133-ontology-index-page.py</code> — do not",
        "  edit by hand.</footer>",
        "</body>",
        "</html>",
        "",
    ]
    return "\n".join(out)


def main(argv=None):
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--root", default=ROOT,
                    help="racine du depot (ou de l'arbre de controle)")
    ap.add_argument("--check", action="store_true",
                    help="ne rien ecrire : comparer la page du disque a sa regeneration")
    args = ap.parse_args(argv)
    folder = os.path.join(args.root, *FOLDER.split("/"))
    page = os.path.join(folder, PAGE)
    if not os.path.isdir(folder):
        print("dossier absent : %s" % folder, file=sys.stderr)
        return 2
    content = render(folder)
    if not args.check:
        with open(page, "w", encoding="utf-8", newline="\n") as fh:
            fh.write(content)
        print("[ecrit] %s (%d octets)" % (page, len(content.encode("utf-8"))))
        return 0
    if not os.path.exists(page):
        print("page absente : %s" % page, file=sys.stderr)
        return 1
    with open(page, encoding="utf-8", newline="") as fh:
        current = fh.read()
    if current == content:
        print("identique : %d octets" % len(content.encode("utf-8")))
        return 0
    diff = difflib.unified_diff(
        current.splitlines(True), content.splitlines(True),
        fromfile="disque", tofile="regeneré")
    sys.stderr.writelines(diff)
    print("page perimee : elle ne decrit plus les fichiers du dossier", file=sys.stderr)
    return 1


if __name__ == "__main__":
    sys.exit(main())
