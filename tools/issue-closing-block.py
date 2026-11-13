#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Le bloc « chemin de clôture » en tête d'un corps d'issue — std-lib + `gh`.

    python tools/issue-closing-block.py list
    python tools/issue-closing-block.py show N
    python tools/issue-closing-block.py set N --block-file F                      # dry-run
    python tools/issue-closing-block.py set N --block-file F --apply --expect-login LOGIN

POURQUOI. Une issue ouverte porte en tête de son corps son chemin de clôture (fait, reste,
critère, blocage, prochaine étape) ; qui merge un changement qui la touche le met à jour
(docs/agents/method.md § Closing issues). Fait à la main, ce geste finit par manger un
corps d'origine, ou par poster un chemin de fichier au lieu de son contenu.

FORME DU CORPS

    <!-- etat-cloture:debut -->

    (le bloc)

    <!-- etat-cloture:fin -->

    ---

    (le texte d'origine, qui ne change jamais)

Le texte d'origine commence après le premier trait `---` qui suit le marqueur de fin, et
après la ligne vide qui suit ce trait. Entre le marqueur et le trait : des blancs, rien
d'autre. Le bloc et les séparateurs prennent les fins de ligne du texte d'origine (CRLF
s'il en porte : GitHub rend `\\r\\n` pour un corps édité dans le navigateur).

COMMANDES
    list  un seul appel `gh issue list` : chaque issue ouverte a un bloc, n'en a pas, ou est
          malformée. Sortie 1 si une au moins est malformée.
    show  imprime le bloc de l'issue N. Sortie 1 si elle n'en a pas.
    set   dry-run par défaut : diff unifié de la TÊTE seule (ce qui précède le texte
          d'origine) et taille en octets du texte d'origine avant / après, qui doivent être
          égales. Avec --apply : --expect-login exigé et comparé à `gh api user` ; le corps
          est relu juste avant l'écriture (refus s'il a changé depuis la première lecture) ;
          écriture par `gh issue edit --body-file` (jamais --body, qui poste le chemin) ;
          relecture comparée au corps envoyé, fins de ligne normalisées.

REFUS — sortie 2, jamais un vert silencieux : un seul des deux marqueurs ; un marqueur
dupliqué ; fin avant début ; du texte avant le marqueur de début (il serait perdu) ; pas de
trait `---` juste après le marqueur de fin ; un bloc vide ; un bloc qui contient un
marqueur ; un bloc qui porte une mention `@login` hors code (elle notifie une personne) ; un
texte d'origine qui porte un marqueur ; un corps composé de plus de 65 536 caractères. Un
`gh` en échec sort en 2 avec son stderr : jamais un résultat vide.

CE QUI N'EST PAS VÉRIFIÉ : que le bloc soit juste ou à jour ; qu'il ne nomme ni personne, ni
chemin, ni montant (seules les mentions `@` sont refusées : relire le diff) ; les mentions
en bloc de code indenté (refusées comme les autres) ; la longueur comptée par GitHub
(comptée ici en points de code) ; la fenêtre entre la relecture et l'écriture ; les
commentaires de l'issue ; les issues fermées (`list` ne lit que les ouvertes). Un marqueur
cité dans le texte d'origine rend le corps malformé : refusé, pas contourné.
"""
import argparse
import difflib
import io
import json
import os
import re
import subprocess
import sys
import tempfile

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
REPO = "ArgumentumGames/Argumentum"
BEGIN, END = "<!-- etat-cloture:debut -->", "<!-- etat-cloture:fin -->"
LIMIT = 65536            # longueur maximale d'un corps d'issue sur GitHub
LIST_LIMIT = 1000        # `gh issue list` coupe à --limit sans le dire
RULE = re.compile(r"(?:[ \t]*\r?\n)+---[ \t]*(?:\r?\n|\Z)(?:\r?\n)?")
FENCE = re.compile(r"(?ms)^ {0,3}(`{3,}|~{3,}).*?(?:^ {0,3}\1|\Z)")
SPAN = re.compile(r"(?s)(`+)(?!`).+?(?<!`)\1(?!`)")
MENTION = re.compile(r"(?<![\w`/])@[A-Za-z0-9][A-Za-z0-9-]*")


class Refusal(Exception):
    def __init__(self, code, message):
        Exception.__init__(self, message)
        self.code = code


class GhError(Exception):
    pass


def eol(text):
    return text.replace("\r\n", "\n")


def mentions(text):
    """Les `@login` hors code : un bloc clôturé ou un span entre accents graves ne notifie pas."""
    return sorted(set(MENTION.findall(SPAN.sub(" ", FENCE.sub(" ", text)))))


def split(body):
    """(bloc ou None, texte d'origine). Sans marqueur : (None, body)."""
    starts, ends = body.count(BEGIN), body.count(END)
    if not starts and not ends:
        return None, body
    if not starts or not ends:
        raise Refusal("unpaired-marker", "un seul des deux marqueurs est présent : %s"
                      % (BEGIN if starts else END))
    if starts > 1 or ends > 1:
        raise Refusal("duplicate-marker", "marqueurs dupliqués : %d début, %d fin" % (starts, ends))
    start, end = body.index(BEGIN), body.index(END)
    if end < start:
        raise Refusal("marker-order", "le marqueur de fin précède le marqueur de début")
    if body[:start].strip():
        raise Refusal("text-before-block", "du texte précède le marqueur de début : il serait perdu")
    tail = body[end + len(END):]
    rule = RULE.match(tail)
    if not rule:
        raise Refusal("no-rule", "pas de trait `---` juste après le marqueur de fin "
                                 "(seuls des blancs peuvent les séparer)")
    return body[start + len(BEGIN):end].strip(), tail[rule.end():]


def compose(block, original):
    """Le corps neuf : le bloc entre les marqueurs, le trait, puis le texte d'origine tel quel."""
    text = eol(block).strip()
    if not text:
        raise Refusal("empty-block", "bloc vide")
    if BEGIN in text or END in text:
        raise Refusal("marker-in-block", "le bloc contient lui-même un marqueur")
    found = mentions(text)
    if found:
        raise Refusal("mention", "mention hors code dans le bloc (%s) : elle notifierait une personne ; "
                                 "l'écrire entre accents graves" % ", ".join(found))
    if BEGIN in original or END in original:
        raise Refusal("marker-in-original", "le texte d'origine porte un marqueur : le passer par split()")
    nl = "\r\n" if "\r\n" in original else "\n"
    body = nl.join([BEGIN, "", text.replace("\n", nl), "", END, "", "---", "", ""]) + original
    if len(body) > LIMIT:
        raise Refusal("too-long", "corps de %d caractères, limite GitHub %d" % (len(body), LIMIT))
    return body


def head(body, original):
    return body[:len(body) - len(original)]


def run_gh(args):
    """(code, stdout, stderr) de `gh` — le seul point du module qui touche le réseau."""
    try:
        done = subprocess.run(["gh"] + list(args), capture_output=True, cwd=ROOT)
    except OSError as exc:
        return 127, "", "gh introuvable : %s" % exc
    err = done.stderr.decode("utf-8", "replace")
    try:
        return done.returncode, done.stdout.decode("utf-8"), err
    except UnicodeDecodeError as exc:
        return 2, "", "sortie de gh non UTF-8 (%s)\n%s" % (exc, err)


def gh_out(gh, args):
    code, out, err = gh(args)
    if code != 0:
        raise GhError("gh %s : code %d\n%s" % (" ".join(args), code, err.strip()))
    return out


def gh_json(gh, args):
    out = gh_out(gh, args)
    try:
        return json.loads(out)
    except ValueError:
        raise GhError("gh %s : réponse illisible (%d caractères) — un vide n'est pas une réponse"
                      % (" ".join(args), len(out)))


def live_body(gh, repo, number):
    data = gh_json(gh, ["issue", "view", str(number), "-R", repo, "--json", "body"])
    body = data.get("body") if isinstance(data, dict) else None
    if not isinstance(body, str):
        raise GhError("#%d : pas de champ body dans la réponse de gh" % number)
    return body


def cmd_show(args, gh, out, err):
    block, _ = split(live_body(gh, args.repo, args.number))
    if not block:
        print("#%d : aucun bloc de clôture" % args.number, file=err)
        return 1
    print(eol(block), file=out)
    return 0


def cmd_list(args, gh, out, err):
    issues = gh_json(gh, ["issue", "list", "-R", args.repo, "--state", "open",
                          "--limit", str(LIST_LIMIT), "--json", "number,title,body"])
    if not isinstance(issues, list):
        raise GhError("gh issue list : la réponse n'est pas une liste")
    if len(issues) >= LIST_LIMIT:
        raise GhError("gh issue list : %d issues = la limite demandée, liste peut-être tronquée" % len(issues))
    counts = {"bloc": 0, "aucun": 0, "malformé": 0}
    for issue in sorted(issues, key=lambda item: item["number"]):
        if not isinstance(issue.get("body"), str):
            raise GhError("#%s : pas de champ body dans la réponse de gh" % issue.get("number"))
        note = ""
        try:
            state = "bloc" if split(issue["body"])[0] is not None else "aucun"
        except Refusal as refusal:
            state, note = "malformé", " — %s" % refusal
        counts[state] += 1
        print("#%-5d %-9s %s%s" % (issue["number"], state, issue["title"], note), file=out)
    print("%d issues ouvertes : %d avec bloc, %d sans, %d malformées"
          % (len(issues), counts["bloc"], counts["aucun"], counts["malformé"]), file=out)
    return 1 if counts["malformé"] else 0


def cmd_set(args, gh, out, err):
    with io.open(args.block_file, encoding="utf-8-sig", newline="") as handle:
        block = handle.read()
    before = live_body(gh, args.repo, args.number)
    original = split(before)[1]
    after = compose(block, original)
    kept = split(after)[1]
    out.write("".join(difflib.unified_diff(eol(head(before, original)).splitlines(True),
                                           eol(head(after, kept)).splitlines(True),
                                           "#%d tête avant" % args.number, "#%d tête après" % args.number)))
    print("texte d'origine : %d octets avant, %d octets après"
          % (len(original.encode("utf-8")), len(kept.encode("utf-8"))), file=out)
    if kept != original:
        raise Refusal("original-altered", "le texte d'origine changerait : rien n'est écrit")
    if after == before:
        print("#%d : corps inchangé, rien à écrire" % args.number, file=out)
        return 0
    if not args.apply:
        print("dry-run : rien n'est écrit (--apply --expect-login LOGIN pour écrire)", file=out)
        return 0
    login = gh_out(gh, ["api", "user", "--jq", ".login"]).strip()
    if login != args.expect_login:
        raise Refusal("identity", "compte actif « %s », attendu « %s » : rien n'est écrit"
                      % (login, args.expect_login))
    if live_body(gh, args.repo, args.number) != before:
        raise Refusal("body-changed", "le corps de #%d a changé depuis la première lecture : "
                                      "rien n'est écrit, relancer le dry-run" % args.number)
    folder = tempfile.mkdtemp(prefix="issue-closing-block-")
    path = os.path.join(folder, "body.md")
    try:
        with io.open(path, "w", encoding="utf-8", newline="") as handle:
            handle.write(after)
        gh_out(gh, ["issue", "edit", str(args.number), "-R", args.repo, "--body-file", path])
    finally:
        if os.path.exists(path):
            os.remove(path)
        os.rmdir(folder)
    back = live_body(gh, args.repo, args.number)
    if eol(back) != eol(after):
        raise Refusal("read-back", "#%d écrit mais relu différent du corps envoyé (%d contre %d caractères, "
                                   "fins de ligne normalisées) : relire l'issue" % (args.number, len(back), len(after)))
    print("#%d : écrit ; relu identique au corps envoyé (comparaison après normalisation des fins "
          "de ligne CRLF -> LF)" % args.number, file=out)
    return 0


def positive(text):
    value = int(text)
    if value <= 0:
        raise argparse.ArgumentTypeError("numéro d'issue attendu")
    return value


def parser():
    common = argparse.ArgumentParser(add_help=False)
    common.add_argument("--repo", default=REPO)
    top = argparse.ArgumentParser(description="Bloc « chemin de clôture » en tête d'un corps d'issue.")
    sub = top.add_subparsers(dest="command")
    sub.required = True
    sub.add_parser("list", parents=[common], help="les issues ouvertes : bloc / aucun / malformé")
    show = sub.add_parser("show", parents=[common], help="le bloc de l'issue N")
    show.add_argument("number", type=positive)
    put = sub.add_parser("set", parents=[common], help="poser le bloc de l'issue N (dry-run par défaut)")
    put.add_argument("number", type=positive)
    put.add_argument("--block-file", required=True)
    put.add_argument("--apply", action="store_true")
    put.add_argument("--expect-login")
    return top


COMMANDS = {"list": cmd_list, "show": cmd_show, "set": cmd_set}


def main(argv=None, gh=run_gh, out=None, err=None):
    out = out if out is not None else sys.stdout
    err = err if err is not None else sys.stderr
    try:
        args = parser().parse_args(argv)
    except SystemExit as exc:
        return exc.code if isinstance(exc.code, int) else 2
    if args.command == "set" and args.apply and not args.expect_login:
        print("REFUS : --apply exige --expect-login LOGIN (le compte qui doit écrire)", file=err)
        return 2
    try:
        return COMMANDS[args.command](args, gh, out, err)
    except Refusal as refusal:
        print("REFUS (%s) : %s" % (refusal.code, refusal), file=err)
    except GhError as exc:
        print("ÉCHEC gh : %s" % exc, file=err)
    except (OSError, UnicodeDecodeError) as exc:
        print("ÉCHEC : %s" % exc, file=err)
    return 2


if __name__ == "__main__":
    for stream in (sys.stdout, sys.stderr):
        if hasattr(stream, "reconfigure"):
            stream.reconfigure(encoding="utf-8")
    sys.exit(main())
