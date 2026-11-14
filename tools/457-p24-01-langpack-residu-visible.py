#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""P24-01 (#457) — traduire la part VISIBLE du résidu des langpacks, mesurée servie.

Source de la mesure : `docs/dnn-localization/458-langpack-residu-visible-2026-11-05.md`
(#2154, commit 43b922a3) — 222 hits servis, trois surfaces seulement :

1. Skip-link du skin (« Skip to main content », `SkipLink.MainContent`) — toutes
   pages, 8 langues y compris fr. Le skin servi est 2shineBS5 (href
   `#to-shine-page-main`) ; ses 8 jumeaux de culture EXISTENT mais AUCUN ne porte
   la clé (fr-FR : 4 clés, les autres : 15) — DNN retombe sur le neutre anglais.
   Re-mesuré au parseur avant d'écrire (fonction `skin_state`).
2. Page 404 : ~5 chaînes (Privacy Statement ×2, search ×3). Re-mesuré : les
   jumeaux du dépôt portent DÉJÀ les traductions (Privacy.ascx ×2 fichiers ×7
   cultures, Search.ascx ×7 cultures) — rien à écrire, SAUF une coquille pt-BR
   (« Não foin encontrado resultados ») corrigée ici.
3. Boutons sociaux Login/Register — pt-BR et ru-RU seulement : les jumeaux
   `Login.ascx.<culture>.resx` des 3 fournisseurs (Facebook/Google/Live)
   existent pour fr/es/ar/fa/zh mais PAS pour pt-BR ni ru-RU. Créés ici depuis
   le squelette du jumeau fr-FR (BOM UTF-8 + LF, comme lui).

L'anglais reste la valeur du neutre : en-US retombe dessus à raison — ce grain
n'écrit donc que 7 cultures (fr, ru, pt, es, ar, fa, zh), jamais en-US.

Modes :
  (défaut)  sans option : vérifie le contrat (clé présente, valeur exacte,
                      aucune valeur anglaise dans un jumeau non-en) ; exit 1 si
                      rouge — aucune option `--check` n'existe.
  --write            applique les 3 familles de gestes (garde anti-écrasement :
                      refuse si la clé existe déjà avec une autre valeur, refuse
                      d'insérer deux fois).
  --self-test        contrôle inverse sur copies temporaires : (a) clé retirée
                      → check échoue en nommant fichier+clé ; (b) valeur
                      anglaise remise → check échoue sur la fuite ; (c) double
                      insertion → refusée.

Sortie : une ligne par fichier ciblé, MESURÉ, jamais déduit.
"""

import argparse
import os
import re
import shutil
import sys
import tempfile

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

SKIN_DIR = "DNNPlatform/Portals/_default/Skins/2shineBS5/App_LocalResources"
AUTH_DIR = "DNNPlatform/DesktopModules/AuthenticationServices/%s/App_LocalResources"
SEARCH_PT = "DNNPlatform/admin/Skins/App_LocalResources/Search.ascx.pt-BR.resx"

# --- 1. Skip-link : SkipLink.MainContent, par culture (jamais en-US) ---------
SKIN = {
    "fr-FR": "Aller au contenu principal",
    "ru-RU": "Перейти к основному содержимому",
    "pt-BR": "Ir para o conteúdo principal",
    "es-ES": "Ir al contenido principal",
    "ar-SA": "الانتقال إلى المحتوى الرئيسي",
    "fa-IR": "پرش به محتوای اصلی",
    "zh-CN": "跳转到主要内容",
}
SKIN_KEY = "SkipLink.MainContent"
SKIN_EN = "Skip to main content"

# --- 3. Boutons sociaux : jumeaux manquants pt-BR / ru-RU --------------------
# (provider, culture) -> {clé: valeur}. Title.Text suit le jumeau fr-FR du
# fournisseur (Facebook/Google gardent la marque ; Live : fr-FR et zh-CN
# modernisent en « Microsoft », es-ES garde « Windows Live » — on suit la
# lecture moderne, déjà majorité des jumeaux existants).
SOCIAL = {
    ("Facebook", "pt-BR"): {
        "LoginFacebook.Text": "Entrar com o Facebook",
        "RegisterFacebook.Text": "Registrar-se com o Facebook",
        "Title.Text": "Facebook",
    },
    ("Facebook", "ru-RU"): {
        "LoginFacebook.Text": "Войти через Facebook",
        "RegisterFacebook.Text": "Регистрация через Facebook",
        "Title.Text": "Facebook",
    },
    ("Google", "pt-BR"): {
        "LoginGoogle.Text": "Entrar com sua conta Google",
        "RegisterGoogle.Text": "Registrar-se com o Google+",
        "Title.Text": "Google",
    },
    ("Google", "ru-RU"): {
        "LoginGoogle.Text": "Войти через аккаунт Google",
        "RegisterGoogle.Text": "Регистрация через Google+",
        "Title.Text": "Google",
    },
    ("Live", "pt-BR"): {
        "SignInLive.Text": "Entrar com a conta Microsoft",
        "RegisterLive.Text": "Registrar-se com a conta Microsoft",
        "Title.Text": "Microsoft",
    },
    ("Live", "ru-RU"): {
        "SignInLive.Text": "Войти через учетную запись Microsoft",
        "RegisterLive.Text": "Зарегистрироваться через учетную запись Microsoft",
        "Title.Text": "Microsoft",
    },
}
SOCIAL_SIBLING = "fr-FR"  # squelette source (BOM + LF, 3 blocs data)

# --- 2. Coquille pt-BR de la page 404 (search) -------------------------------
TYPO = {
    "file": SEARCH_PT,
    "key": "NoResult.Text",
    "old": "Não foin encontrado resultados",
    "new": "Nenhum resultado encontrado",
}


def path(rel):
    return os.path.join(REPO, rel.replace("/", os.sep))


def parse_values(text):
    """{clé: valeur} au parseur — jamais au grep (les valeurs sont du XML)."""
    out = {}
    for m in re.finditer(
        r'<data name="([^"]+)"[^>]*>\s*<value(?: xml:space="preserve")?>(.*?)</value>',
        text, re.S,
    ):
        out[m.group(1)] = m.group(2)
    return out


BOM = b"\xef\xbb\xbf"


def strip_boms(blob):
    """Retire TOUS les BOM de tête (un double BOM d'une réécriture naïve en
    utf-8-sig sur un texte déjà décodé en utf-8 doit devenir zéro)."""
    while blob.startswith(BOM):
        blob = blob[len(BOM):]
    return blob


def read(rel):
    with open(path(rel), "rb") as f:
        return f.read()


def write_bytes(rel, blob):
    with open(path(rel), "wb") as f:
        f.write(blob)


def skin_file(culture):
    return "%s/2shine.%s.resx" % (SKIN_DIR, culture)


def login_file(provider, culture):
    return "%s/Login.ascx.%s.resx" % (AUTH_DIR % provider, culture)


def skin_state():
    """Re-mesure : quels jumeaux portent la clé, et avec quoi (avant/après)."""
    rows = []
    for culture in SKIN:
        rel = skin_file(culture)
        vals = parse_values(read(rel).decode("utf-8"))
        rows.append((rel, SKIN_KEY in vals, vals.get(SKIN_KEY)))
    return rows


def check(verbose=True):
    """Le contrat. Exit code via la liste `failures`."""
    failures = []

    # 1. Skip-link : présent et exact dans les 7 jumeaux, jamais l'anglais.
    for culture, expected in SKIN.items():
        rel = skin_file(culture)
        blob = read(rel)
        if blob[:3] == BOM or b"\r\n" in blob:
            failures.append("%s : enveloppe attendue sans BOM / LF seuls" % rel)
        vals = parse_values(blob.decode("utf-8"))
        got = vals.get(SKIN_KEY)
        if got is None:
            failures.append("%s : clé %s ABSENTE (fuite EN par repli neutre)" % (rel, SKIN_KEY))
        elif got == SKIN_EN:
            failures.append("%s : %s porte l'ANGLAIS neutre — fuite non fermée" % (rel, SKIN_KEY))
        elif got != expected:
            failures.append("%s : %s = %r, attendu %r" % (rel, SKIN_KEY, got, expected))

    # 2. Boutons sociaux : les 6 jumeaux existent, clés exactes, pas d'anglais.
    en_neutral = {
        "Facebook": {"LoginFacebook.Text", "RegisterFacebook.Text"},
        "Google": {"LoginGoogle.Text", "RegisterGoogle.Text"},
        "Live": {"SignInLive.Text", "RegisterLive.Text"},
    }
    for (provider, culture), expected in SOCIAL.items():
        rel = login_file(provider, culture)
        if not os.path.exists(path(rel)):
            failures.append("%s : jumeau ABSENT" % rel)
            continue
        blob = read(rel)
        if blob[:3] != BOM or blob[3:6] == BOM or b"\r\n" in blob:
            failures.append("%s : enveloppe attendue EXACTEMENT un BOM UTF-8 / LF seuls" % rel)
        vals = parse_values(blob.decode("utf-8"))
        for key, want in expected.items():
            got = vals.get(key)
            if got is None:
                failures.append("%s : clé %s ABSENTE" % (rel, key))
            elif key in en_neutral[provider] and got in (
                "Login with Facebook", "Register with Facebook",
                "Sign with your Google Account", "Register with Google+",
                "Register with Windows Live", "Sign In with Windows Live",
            ):
                failures.append("%s : %s porte l'ANGLAIS neutre" % (rel, key))
            elif got != want:
                failures.append("%s : %s = %r, attendu %r" % (rel, key, got, want))

    # 3. Coquille pt-BR : la nouvelle valeur, plus l'ancienne nulle part.
    blob = read(TYPO["file"]).decode("utf-8")
    vals = parse_values(blob)
    got = vals.get(TYPO["key"])
    if got != TYPO["new"]:
        failures.append("%s : %s = %r, attendu %r" % (TYPO["file"], TYPO["key"], got, TYPO["new"]))
    if TYPO["old"] in blob:
        failures.append("%s : la coquille %r subsiste" % (TYPO["file"], TYPO["old"]))

    if verbose:
        for rel, present, value in skin_state():
            print("  %-72s %s=%r" % (rel.split("DNNPlatform/")[-1], SKIN_KEY, value))
        for (provider, culture) in sorted(SOCIAL):
            rel = login_file(provider, culture)
            exists = os.path.exists(path(rel))
            print("  %-72s %s" % (rel.split("DNNPlatform/")[-1], "présent" if exists else "ABSENT"))
        print("  %-72s %s=%r" % (TYPO["file"].split("DNNPlatform/")[-1], TYPO["key"],
                                 parse_values(read(TYPO["file"]).decode("utf-8")).get(TYPO["key"])))
        if failures:
            print("CHECK: %d échec(s)" % len(failures))
            for f in failures:
                print("  FAIL " + f)
        else:
            print("CHECK: %d/%d fichiers au contrat" % (len(SKIN) + len(SOCIAL) + 1, len(SKIN) + len(SOCIAL) + 1))
    return failures


def do_write():
    # 1. Skip-link : insertion avant </root>, refus si déjà présent.
    for culture, value in SKIN.items():
        rel = skin_file(culture)
        text = read(rel).decode("utf-8")
        if SKIN_KEY in parse_values(text):
            print("SKIP %s : %s déjà présent" % (rel, SKIN_KEY))
            continue
        if text.count("</root>") != 1 or not text.endswith("</root>"):
            sys.exit("REFUS %s : fin de fichier inattendue" % rel)
        block = (
            '  <!-- #457 P24-01: skip-link — part visible du résidu langpack, mesurée servie (#2154). -->\n'
            '  <data name="%s" xml:space="preserve">\n'
            "    <value>%s</value>\n"
            "  </data>\n" % (SKIN_KEY, value)
        )
        write_bytes(rel, (text[: -len("</root>")] + block + "</root>").encode("utf-8"))
        print("WRITE %s : %s += %r" % (rel, SKIN_KEY, value))

    # 2. Boutons sociaux : nouveau jumeau depuis le squelette fr-FR du fournisseur.
    for (provider, culture), expected in SOCIAL.items():
        rel = login_file(provider, culture)
        if os.path.exists(path(rel)):
            print("SKIP %s : existe déjà" % rel)
            continue
        sib = login_file(provider, SOCIAL_SIBLING)
        text = strip_boms(read(sib)).decode("utf-8")
        sib_vals = parse_values(text)
        missing = [k for k in expected if k not in sib_vals]
        if missing:
            sys.exit("REFUS %s : le squelette %s ne porte pas %r" % (rel, sib, missing))
        for key, want in expected.items():
            old_val = sib_vals[key]
            text, n = re.subn(r"(<value[^>]*>)%s(</value>)" % re.escape(old_val),
                              lambda m: m.group(1) + want + m.group(2), text)
            if n != 1:
                sys.exit("REFUS %s : valeur de %s non unique dans le squelette (%d)" % (rel, key, n))
        write_bytes(rel, BOM + text.encode("utf-8"))
        print("WRITE %s (depuis %s)" % (rel, sib))

    # 3. Coquille pt-BR : fragment unique, sinon refus (la balise <value> peut
    # porter xml:space="preserve" — la regex la préserve telle quelle).
    # BOM : on décode SANS BOM et on en remet exactement un (garde anti double-BOM).
    # Idempotent : si la nouvelle valeur est déjà en place, on SKIP (un second
    # --write est un no-op, pas un échec) ; REFUS si l'ancienne a disparu sans
    # être remplacée par la nôtre (quelqu'un a corrigé autrement).
    text = strip_boms(read(TYPO["file"])).decode("utf-8")
    if re.search(r"<value[^>]*>%s</value>" % re.escape(TYPO["new"]), text):
        print("SKIP %s : %s porte déjà %r" % (TYPO["file"], TYPO["key"], TYPO["new"]))
        return
    text, n = re.subn(r"(<value[^>]*>)%s(</value>)" % re.escape(TYPO["old"]),
                      lambda m: m.group(1) + TYPO["new"] + m.group(2), text)
    if n != 1:
        sys.exit("REFUS %s : coquille %r non unique (%d occurrence(s))"
                 % (TYPO["file"], TYPO["old"], n))
    write_bytes(TYPO["file"], BOM + text.encode("utf-8"))
    print("WRITE %s : %s %r -> %r" % (TYPO["file"], TYPO["key"], TYPO["old"], TYPO["new"]))


def self_test():
    """Contrôle inverse sur copies : le check doit VOIR chaque défaut qu'on injecte."""
    global REPO
    saved = REPO
    tmp = tempfile.mkdtemp(prefix="p24-01-selftest-")
    try:
        # Construire un arbre réduit avec les fichiers du dépôt, appliquer, puis muter.
        rels = [skin_file(c) for c in SKIN] + [TYPO["file"]]
        rels += [login_file(p, c) for (p, c) in SOCIAL]
        rels += [login_file(p, SOCIAL_SIBLING) for p in ("Facebook", "Google", "Live")]
        for rel in rels:
            if not os.path.exists(path(rel)):
                continue  # jumeaux pt-BR/ru-RU : créés par do_write() ci-dessous
            dest = os.path.join(tmp, rel.replace("/", os.sep))
            os.makedirs(os.path.dirname(dest), exist_ok=True)
            shutil.copy(path(rel), dest)
        REPO = tmp
        # Reconstruire l'état d'AVANT le geste, même si le dépôt est déjà écrit :
        # retirer le bloc inséré, remettre la coquille, supprimer les jumeaux neufs.
        for culture in SKIN:
            rel = skin_file(culture)
            f = os.path.join(tmp, rel.replace("/", os.sep))
            text = open(f, encoding="utf-8").read()
            text2, n = re.subn(
                r'  <!-- #457 P24-01:[^\n]*-->\n  <data name="%s"[^>]*>\n    <value>[^<]*</value>\n  </data>\n' % SKIN_KEY,
                "", text)
            if n:
                open(f, "w", encoding="utf-8", newline="").write(text2)
        typo_f = os.path.join(tmp, TYPO["file"].replace("/", os.sep))
        text = open(typo_f, encoding="utf-8").read()
        text2, n = re.subn(r"(<value[^>]*>)%s(</value>)" % re.escape(TYPO["new"]),
                           lambda m: m.group(1) + TYPO["old"] + m.group(2), text)
        if n:
            open(typo_f, "w", encoding="utf-8", newline="").write(text2)
        for (provider, culture) in SOCIAL:
            f = os.path.join(tmp, login_file(provider, culture).replace("/", os.sep))
            if os.path.exists(f):
                os.remove(f)
        # Base AVANT le geste : le contrat doit être ROUGE (c'est le contrôle
        # « échoue sans le changement ») — les 3 familles de défauts attendues.
        base_failures = check(verbose=False)
        need = {fam for fam in base_failures for pat in (SKIN_KEY, "jumeau ABSENT", "coquille") if pat in fam}
        if len(base_failures) < 15 or not need:
            sys.exit("SELF-TEST: base inattendue avant le geste (%d échecs) — instrument cassé"
                     % len(base_failures))
        do_write()
        if check(verbose=False):
            sys.exit("SELF-TEST: rouge juste après --write — geste cassé")

        # (a) clé retirée -> nommée
        rel = skin_file("fr-FR")
        pristine = open(os.path.join(tmp, rel.replace("/", os.sep)), "rb").read()
        text = pristine.decode("utf-8")
        pattern = r'  <data name="%s"[^>]*>\n    <value>[^<]*</value>\n  </data>\n' % SKIN_KEY
        mutated, n = re.subn(pattern, "", text)
        assert n == 1, "self-test: bloc skip-link non retiré (n=%d)" % n
        open(os.path.join(tmp, rel.replace("/", os.sep)), "w", encoding="utf-8", newline="").write(mutated)
        fails = check(verbose=False)
        named = any(SKIN_KEY in f and "fr-FR" in f for f in fails)
        assert fails and named, "self-test (a): retrait de clé non vu"

        # (b) la valeur française remplacée par l'anglais neutre -> fuite vue
        open(os.path.join(tmp, rel.replace("/", os.sep)), "wb").write(pristine)
        text = pristine.decode("utf-8")
        good = "<value>%s</value>" % SKIN["fr-FR"]
        assert text.count(good) == 1, "self-test: valeur fr non unique"
        mutated = text.replace(good, "<value>%s</value>" % SKIN_EN)
        open(os.path.join(tmp, rel.replace("/", os.sep)), "w", encoding="utf-8", newline="").write(mutated)
        fails = check(verbose=False)
        assert any("ANGLAIS" in f for f in fails), "self-test (b): fuite anglaise non vue"

        # (c) double insertion -> refusée (le --write sur un arbre déjà écrit skippe)
        before = open(os.path.join(tmp, rel.replace("/", os.sep)), "rb").read()
        do_write()  # doit SKIPper skin_file, pas réinsérer
        after = open(os.path.join(tmp, rel.replace("/", os.sep)), "rb").read()
        assert before == after, "self-test (c): double insertion non refusée"
        print("SELF-TEST: 3 contrôles verts (retrait vu, fuite vue, double insertion refusée)")
    finally:
        REPO = saved
        shutil.rmtree(tmp, ignore_errors=True)


def main():
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--write", action="store_true", help="applique (défaut: check seul)")
    ap.add_argument("--self-test", action="store_true", help="contrôle inverse sur copies")
    args = ap.parse_args()
    if args.self_test:
        self_test()
        return
    if args.write:
        do_write()
    failures = check()
    sys.exit(1 if failures else 0)


if __name__ == "__main__":
    main()
