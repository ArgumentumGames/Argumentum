#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Contrat des documents d'entrée pour agents — stdlib seulement, aucun réseau, aucun git.

    python tools/test_agent-docs-contract.py

POURQUOI. `AGENTS.md` et `CLAUDE.md` sont chargés à CHAQUE session ; le détail vit dans
`docs/agents/`, lu à la demande. Ce partage ne tient que si trois choses restent vraies,
et aucune ne se voit à la relecture d'une PR :

  - un renvoi (« lis `regeneration.md` § *Regeneration runs* ») mène quelque part. Une
    section renommée laisse un renvoi mort, et un renvoi mort se lit « rien à lire » ;
  - le détail ne revient pas dans les fichiers chargés par défaut — c'est ainsi que
    `CLAUDE.md` avait atteint 61 Ko ;
  - `docs/agents/` n'est pas importé : un `@docs/agents/…` le rechargerait à chaque session.

CE QUI EST ÉPINGLÉ
   1. tout lien markdown relatif des documents d'entrée et de `docs/agents/` (sous-dossiers
      compris) désigne un chemin qui existe, À LA CASSE EXACTE — un lien à la mauvaise
      casse s'ouvre sous Windows et casse sous Linux. Formes lues : `[a](cible)`,
      `[a](cible "titre")`, `[a](<cible avec espaces>)`, et la définition `[a]: cible`
   2. tout renvoi `fichier.md` § *Titre* désigne UN titre de ce fichier, et un seul.
      `Titre` est le titre entier ou son DÉBUT, coupé sur une frontière de mot : les
      titres portent des numéros d'issue et des dates qu'un renvoi n'a pas à recopier.
      Un début que deux titres partagent est un renvoi ambigu, donc mort. Un nom de
      fichier nu qui existe à deux endroits l'est aussi : on écrit alors son chemin
   3. `CLAUDE.md` importe `AGENTS.md` une fois, et rien d'autre n'est importé — ni par une
      ligne `@chemin`, ni par un `@chemin` glissé dans une phrase
   4. `AGENTS.md` + `CLAUDE.md` restent sous BUDGET octets (fins de ligne comptées en LF)
   5-13. contrôles inverses, sur un arbre synthétique : chaque défaut ci-dessus EST vu, et
        ce qui n'en est pas un ne l'est pas (un titre, un lien, un renvoi ou un `@chemin`
        dans un bloc de code n'en sont pas ; une adresse e-mail n'est pas un import)

CE QUI N'EST PAS ÉPINGLÉ : que le texte soit juste ; les liens http(s) ; les ancres `#…` ;
une cible de lien qui contient une parenthèse ; les liens par référence `[a][b]` dont la
définition manque ; les renvois en prose sans la forme `fichier.md` § *Titre*. Seuls les
blocs de code CLÔTURÉS (``` ou ~~~) sont écartés : un bloc indenté de 4 espaces est lu
comme du texte. La conservation ligne à ligne d'un déplacement se prouve ailleurs
(`tools/doc-move-preservation.py`).

CHANGER LE BUDGET est légitime : c'est une décision, à prendre dans la PR qui le change,
pas un chiffre à relever pour faire passer un ajout. Le détail va dans `docs/agents/`.
"""
import io
import os
import re
import shutil
import tempfile
import unittest
from urllib.parse import unquote

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ENTRY = ("AGENTS.md", "CLAUDE.md")
DETAIL_DIR = "docs/agents"
BUDGET = 32000      # octets, AGENTS.md + CLAUDE.md, fins de ligne en LF (= la taille des blobs).
                    # Mesuré 2026-11-13 : 22 984 (60 960 avant, pour CLAUDE.md seul).

FENCE = re.compile(r"^ {0,3}(`{3,}|~{3,})")
LINK = re.compile(r"""\]\(\s*(<[^<>\n]*>|[^()\s]+)(?:\s+(?:"[^"\n]*"|'[^'\n]*'))?\s*\)""")
REFDEF = re.compile(r"(?m)^ {0,3}\[(?!\^)[^\]\n]+\]:[ \t]*(<[^<>\n]*>|\S+)")
POINTER = re.compile(r"`([^`]+\.md)`|§ \*([^*]+)\*")
HEADING = re.compile(r"(?m)^ {0,3}#{1,6}[ \t]+(.+?)[ \t]*#*[ \t]*$")
CODE_SPAN = re.compile(r"`[^`\n]*`")
IMPORT = re.compile(r"(?<![\w@/.\-])@([\w~./\\\-]+)")


def read(root, rel):
    with io.open(os.path.join(root, *rel.split("/")), encoding="utf-8") as handle:
        return handle.read()


def prose(text):
    """Le texte hors blocs de code clôturés, ligne pour ligne (les lignes d'un bloc deviennent
    vides) : dans un bloc, `# x` est un commentaire shell, `@x` un décorateur, pas du markdown."""
    kept, fence = [], None
    for line in text.replace("\r\n", "\n").split("\n"):
        mark = FENCE.match(line)
        if fence is None:
            if mark:
                fence = (mark.group(1)[0], len(mark.group(1)))
                kept.append("")
            else:
                kept.append(line)
        else:
            if (mark and mark.group(1)[0] == fence[0] and len(mark.group(1)) >= fence[1]
                    and line.strip() == mark.group(1)):
                fence = None
            kept.append("")
    return "\n".join(kept)


def documents(root):
    """Les documents d'entrée et les fichiers de détail présents, sous-dossiers compris."""
    found = [rel for rel in ENTRY if os.path.isfile(os.path.join(root, rel))]
    detail = os.path.join(root, *DETAIL_DIR.split("/"))
    for base, dirs, names in os.walk(detail):
        dirs.sort()
        for name in sorted(names):
            if name.lower().endswith(".md"):
                found.append(os.path.relpath(os.path.join(base, name), root).replace(os.sep, "/"))
    return found


def exists_exact(root, rel):
    """Le chemin existe, composant par composant, à la casse exacte (comme sous Linux)."""
    rel = os.path.normpath(rel).replace(os.sep, "/")
    if rel == ".":
        return True
    current = root      # un `..` qui sort du dépôt n'est dans aucun listdir : il est mort ci-dessous
    for part in rel.split("/"):
        try:
            names = os.listdir(current)
        except OSError:
            return False
        if part not in names:
            return False
        current = os.path.join(current, part)
    return True


def link_targets(text):
    for match in LINK.finditer(text):
        yield match.group(1)
    for match in REFDEF.finditer(text):
        yield match.group(1)


def local_path(target):
    """La partie « chemin » d'une cible locale, ou None (lien externe, ancre seule)."""
    if target.startswith("<") and target.endswith(">"):
        target = target[1:-1]
    if re.match(r"^[A-Za-z][A-Za-z0-9+.\-]*:", target) or target.startswith("//"):
        return None
    return unquote(target.split("#")[0].split("?")[0]) or None      # `#ancre` seule -> None


def dead_links(root):
    """[(document, cible)] — liens markdown relatifs dont la cible n'existe pas."""
    dead = []
    for rel in documents(root):
        for target in link_targets(prose(read(root, rel))):
            path = local_path(target)
            if path is None:
                continue
            if path.startswith("/"):                     # GitHub : depuis la racine du dépôt
                where = path.lstrip("/")
            else:
                where = os.path.join(os.path.dirname(rel), path)
            if not exists_exact(root, where):
                dead.append((rel, target))
    return dead


def links_seen(root):
    return sum(1 for rel in documents(root) for _ in link_targets(prose(read(root, rel))))


def resolve(root, citing, name):
    """-> (fichier, None) ou (None, raison). Un fichier cité se cherche à côté du document
    citant, sous docs/agents/, puis depuis la racine ; trouvé à deux endroits, il est ambigu."""
    name = name.replace("\\", "/")
    beside = os.path.normpath(os.path.join(os.path.dirname(citing), name)).replace(os.sep, "/")
    found = []
    for candidate in (beside, "%s/%s" % (DETAIL_DIR, name), name):
        candidate = os.path.normpath(candidate).replace(os.sep, "/")
        if (candidate not in found and exists_exact(root, candidate)
                and os.path.isfile(os.path.join(root, *candidate.split("/")))):
            found.append(candidate)
    if not found:
        return None, "fichier absent"
    if len(found) > 1:
        return None, "fichier ambigu"
    return found[0], None


def pointers(root):
    """[(document, fichier cité, titre)] — la forme `fichier.md` § *Titre* [, § *Titre*…]."""
    found = []
    for rel in documents(root):
        for line in prose(read(root, rel)).split("\n"):
            current = None
            for match in POINTER.finditer(line):
                if match.group(1):
                    current = match.group(1)
                elif current:
                    found.append((rel, current, clean(match.group(2).rstrip("…"))))
    return found


def clean(title):
    """Un titre sans son emphase : `## **Titre**` et § *Titre* désignent la même chose."""
    return re.sub(r"[*`]", "", title).strip()


def headings(text):
    return [clean(match.group(1)) for match in HEADING.finditer(prose(text))]


def designates(heading, title):
    """Le titre entier, ou son début coupé sur une frontière de mot."""
    if heading == title:
        return True
    if not title or not heading.startswith(title):
        return False
    following = heading[len(title)]
    return not (following.isalnum() or following == "_")


def dead_pointers(root):
    dead = []
    for rel, name, title in pointers(root):
        target, reason = resolve(root, rel, name)
        if target is None:
            dead.append((rel, name, title, reason))
            continue
        titles = headings(read(root, target))
        if titles.count(title) == 1:
            continue
        hits = [t for t in titles if designates(t, title)]
        if not hits:
            dead.append((rel, target, title, "titre absent"))
        elif len(hits) > 1:
            dead.append((rel, target, title, "titre ambigu"))
    return dead


def imports(root):
    """[(document, cible)] — tout `@chemin` des documents d'entrée, en ligne seule ou dans une
    phrase, hors blocs et hors `code`. Un chemin porte un `.` ou un `/` : `@nom` seul n'en est pas un."""
    found = []
    for rel in ENTRY:
        if not os.path.isfile(os.path.join(root, rel)):
            continue
        for line in prose(read(root, rel)).split("\n"):
            for match in IMPORT.finditer(CODE_SPAN.sub("", line)):
                target = match.group(1).rstrip(".,;:!?)")      # la ponctuation de la phrase
                if re.search(r"[./\\]", target):
                    found.append((rel, target))
    return found


def loaded_bytes(root):
    """{document: octets} — fins de ligne ramenées à LF, pour mesurer le blob et non le checkout."""
    sizes = {}
    for rel in ENTRY:
        with open(os.path.join(root, rel), "rb") as handle:
            sizes[rel] = len(handle.read().replace(b"\r\n", b"\n"))
    return sizes


class AgentDocsContract(unittest.TestCase):
    def test_01_relative_links_resolve(self):
        self.assertEqual(dead_links(ROOT), [])
        # Plancher : un zéro n'est une absence que si l'instrument a vu des liens.
        self.assertGreaterEqual(links_seen(ROOT), 20, "trop peu de liens vus : l'instrument est-il aveugle ?")

    def test_02_section_pointers_resolve(self):
        self.assertEqual(dead_pointers(ROOT), [])
        self.assertGreaterEqual(len(pointers(ROOT)), 10,
                                "trop peu de renvois vus : la forme `fichier.md` § *Titre* a-t-elle changé ?")

    def test_03_only_agents_md_is_imported(self):
        self.assertEqual(imports(ROOT), [("CLAUDE.md", "AGENTS.md")],
                         "CLAUDE.md importe AGENTS.md, une fois ; rien d'autre ne se charge par défaut")

    def test_04_default_loaded_documents_stay_under_budget(self):
        sizes = loaded_bytes(ROOT)
        self.assertLessEqual(
            sum(sizes.values()), BUDGET,
            "%s = %d octets > %d. Ces deux fichiers sont chargés à chaque session : le détail va "
            "dans docs/agents/ (règle : docs/agents/README.md)." % (sizes, sum(sizes.values()), BUDGET))
        # Plancher : un fichier vidé ou tronqué passerait sous le budget sans rien prouver.
        self.assertGreaterEqual(min(sizes.values()), 2000, "un document d'entrée est presque vide : %s" % sizes)


class InverseControls(unittest.TestCase):
    """Chaque détecteur doit VOIR le défaut qu'il prétend exclure — et rien que lui."""

    AGENTS = "# Entry\n\nSee [detail](docs/agents/topic.md) and `topic.md` § *Real title*.\n"

    def setUp(self):
        self.root = tempfile.mkdtemp(prefix="agent-docs-")
        os.makedirs(os.path.join(self.root, "docs", "agents"))
        self.write("AGENTS.md", self.AGENTS)
        self.write("CLAUDE.md", "# Claude\n\n@AGENTS.md\n")
        self.write("docs/agents/topic.md", "# Topic\n\n## Real title (dated)\n\ntext\n")

    def tearDown(self):
        shutil.rmtree(self.root, ignore_errors=True)

    def write(self, rel, text, newline="\n"):
        full = os.path.join(self.root, *rel.split("/"))
        if not os.path.isdir(os.path.dirname(full)):
            os.makedirs(os.path.dirname(full))
        with io.open(full, "w", encoding="utf-8", newline=newline) as handle:
            handle.write(text)

    def healthy(self):
        self.assertEqual((dead_links(self.root), dead_pointers(self.root), imports(self.root)),
                         ([], [], [("CLAUDE.md", "AGENTS.md")]))

    def test_05_healthy_synthetic_tree_is_clean_then_a_dead_link_is_seen(self):
        self.healthy()
        self.write("AGENTS.md", "# Entry\n\nSee [detail](docs/agents/renamed.md).\n")
        self.assertEqual(dead_links(self.root), [("AGENTS.md", "docs/agents/renamed.md")])

    def test_06_a_pointer_to_a_renamed_heading_is_seen(self):
        self.write("docs/agents/topic.md", "# Topic\n\n## Another title\n\ntext\n")
        self.assertEqual(dead_pointers(self.root),
                         [("AGENTS.md", "docs/agents/topic.md", "Real title", "titre absent")])
        self.write("AGENTS.md", "# Entry\n\nSee `gone.md` § *Real title*.\n")
        self.assertEqual(dead_pointers(self.root), [("AGENTS.md", "gone.md", "Real title", "fichier absent")])

    def test_07_an_added_import_is_seen_on_its_own_line_or_inside_a_sentence(self):
        self.write("CLAUDE.md", "# Claude\n\n@AGENTS.md\n@docs/agents/topic.md\n")
        self.assertEqual(imports(self.root),
                         [("CLAUDE.md", "AGENTS.md"), ("CLAUDE.md", "docs/agents/topic.md")])
        self.write("CLAUDE.md", "# Claude\n\n@AGENTS.md\n\nRead @docs/agents/topic.md, then go.\n")
        self.assertEqual(imports(self.root),
                         [("CLAUDE.md", "AGENTS.md"), ("CLAUDE.md", "docs/agents/topic.md")])
        # AGENTS.md est importé par CLAUDE.md : un import qu'il porte se charge aussi.
        self.write("CLAUDE.md", "# Claude\n\n@AGENTS.md\n")
        self.write("AGENTS.md", self.AGENTS + "\n@docs/agents/topic.md\n")
        self.assertEqual(imports(self.root),
                         [("AGENTS.md", "docs/agents/topic.md"), ("CLAUDE.md", "AGENTS.md")])

    def test_08_what_sits_in_code_is_neither_a_heading_nor_an_import_nor_a_link(self):
        # Un `# titre` dans un bloc est un commentaire shell : un renvoi vers lui est mort.
        self.write("docs/agents/topic.md", "# Topic\n\n```bash\n# Real title\n```\n\ntext\n")
        self.assertEqual(dead_pointers(self.root),
                         [("AGENTS.md", "docs/agents/topic.md", "Real title", "titre absent")])
        # Un `@chemin` dans un bloc, dans du `code`, ou une adresse e-mail : pas des imports.
        self.write("docs/agents/topic.md", "# Topic\n\n## Real title (dated)\n\ntext\n")
        self.write("CLAUDE.md", "# Claude\n\n@AGENTS.md\n\n```python\n@docs/agents/topic.md\n```\n\n"
                                "Never write `@docs/agents/topic.md`. Mail noreply@example.invalid, ask @someone.\n"
                                "~~~\n[dead](docs/agents/absent.md)\n~~~\n")
        self.healthy()
        # Un renvoi cité en exemple dans un bloc n'est pas un renvoi : il n'a pas à mener quelque part.
        self.write("AGENTS.md", self.AGENTS + "\n```\n`topic.md` § *No such title*\n```\n")
        self.healthy()
        # Un bloc ne se referme que sur SA clôture : même signe, au moins aussi longue, seule sur
        # sa ligne. Ce qui suit une fausse clôture est encore du code.
        for inner in ("~~~~", "```", "```` python"):
            self.write("AGENTS.md", self.AGENTS + "\n````\n%s\n[dead](docs/agents/absent.md)\n````\n" % inner)
            self.assertEqual(dead_links(self.root), [], inner)

    def test_09_a_pointer_designates_exactly_one_heading(self):
        # Un début que deux titres partagent : ambigu, donc mort.
        self.write("docs/agents/topic.md", "# Topic\n\n## Real title (dated)\n\n## Real title, again\n")
        self.assertEqual(dead_pointers(self.root),
                         [("AGENTS.md", "docs/agents/topic.md", "Real title", "titre ambigu")])
        # Le titre exact existe une fois : il l'emporte sur ceux qui ne font que commencer pareil.
        self.write("docs/agents/topic.md", "# Topic\n\n## Real title\n\n## Real title, again\n")
        self.assertEqual(dead_pointers(self.root), [])
        # Un début coupé au milieu d'un mot ne désigne rien.
        self.write("docs/agents/topic.md", "# Topic\n\n## Real titles and more\n")
        self.assertEqual(dead_pointers(self.root),
                         [("AGENTS.md", "docs/agents/topic.md", "Real title", "titre absent")])
        # L'emphase ne compte pas : `## **Real title** (dated)` est le même titre.
        self.write("docs/agents/topic.md", "# Topic\n\n## **Real title** (dated)\n")
        self.assertEqual(dead_pointers(self.root), [])

    def test_10_every_link_form_is_read(self):
        for form in ('[a](docs/agents/absent.md "a title")', "[a](<docs/agents/absent file.md>)",
                     "[a][ref]\n\n[ref]: docs/agents/absent.md", "[a](docs/agents/absent.md#section)",
                     "[a](docs/agents/Topic.md)"):            # mauvaise casse : vert sous Windows sans la garde
            self.write("AGENTS.md", "# Entry\n\n%s\n" % form)
            self.assertEqual(len(dead_links(self.root)), 1, form)
        # Et ce qui existe n'est pas rougi : espace encodé, chevrons, ancre, titre, note de bas de page.
        self.write("docs/agents/two words.md", "# Two\n")
        self.write("AGENTS.md", "# Entry\n\n[a](docs/agents/two%20words.md) [b](<docs/agents/two words.md>) "
                                "[c](docs/agents/topic.md#real-title 'a title') [d](https://example.invalid/x.md) "
                                "[e](#entry) [f](//example.invalid/x.md) [g](/docs/agents/topic.md) [h](./) "
                                "note[^1]\n\n[^1]: not a link definition\n")
        self.assertEqual(dead_links(self.root), [])
        # Une cible qui commence par `/` se lit depuis la racine du dépôt, d'où qu'elle soit citée.
        self.write("docs/agents/deep/more.md", "# More\n\n[a](/docs/agents/topic.md) [b](/docs/agents/absent.md)\n")
        self.assertEqual(dead_links(self.root), [("docs/agents/deep/more.md", "/docs/agents/absent.md")])

    def test_11_detail_files_in_subfolders_are_read(self):
        self.write("docs/agents/deep/more.md", "# More\n\n[a](../absent.md) and `topic.md` § *No such title*\n")
        self.assertEqual(dead_links(self.root), [("docs/agents/deep/more.md", "../absent.md")])
        self.assertEqual(dead_pointers(self.root),
                         [("docs/agents/deep/more.md", "docs/agents/topic.md", "No such title", "titre absent")])

    def test_12_a_bare_file_name_found_in_two_places_is_ambiguous(self):
        self.write("topic.md", "# Root topic\n\n## Real title\n")
        self.assertEqual(dead_pointers(self.root), [("AGENTS.md", "topic.md", "Real title", "fichier ambigu")])
        self.write("AGENTS.md", "# Entry\n\nSee `docs/agents/topic.md` § *Real title*.\n")
        self.assertEqual(dead_pointers(self.root), [])

    def test_13_the_budget_counts_blob_bytes_not_checkout_bytes(self):
        self.write("AGENTS.md", "one\ntwo\n", newline="\r\n")
        self.assertEqual(loaded_bytes(self.root)["AGENTS.md"], 8)


if __name__ == "__main__":
    unittest.main()
