#!/usr/bin/env python3
"""#1539 — Règles : la liste de matériel ne doit annoncer AUCUN nombre de cartes mémo.

Décision owner du 24/09/2026 (interactif, VÉRIFIÉ, verbatim « Q-14 ok pour a ») :
le chiffre part, la ligne reste. Aucune valeur n'est juste partout — la boîte
imprime 7 cartes mémo (décision #1187), le P&P A4 en imprime 5, le P&P Light 1.
La liste de matériel annonçait « 5 » sur 5 cartes de règles (Rules_02/09/11/13
du deck + RulesPP_02), dans les 8 langues, soit 40 cellules.

DEUX invariants, parce qu'un seul serait satisfait par disparition :
  1. aucun item de liste « mémo » ne commence par un chiffre ;
  2. les 40 cellules portent toujours leur ligne (plancher de présence) — sans
     lui, supprimer la ligne éteindrait l'invariant 1 sans rien corriger.

Gardes d'instrument :
- le mot « mémo » N'EST PAS le même dans les 8 langues — ajuda (pt) n'est pas
  ayuda (es), l'arabe écrit تذكير, le persan یادآور, le chinois 提示卡 ;
  une sonde qui ne cherche que « mémo » rend 39 sur un corpus qui en porte 40
  (écart mesuré lors de la reconnaissance de ce grain) ;
- les chiffres arabo-indiens (U+0660..) et étendus (U+06F0..) sont des chiffres
  pour tous les moteurs, les idéogrammes chinois 一二三四五六七 non — d'où
  l'énumération en clair des quatre systèmes au lieu d'un `isdigit()` ;
- l'apostrophe du corpus est typographique (U+2019) là où elle apparaît.

Usage :
  python docs/corpus/rules-memo-material-1539.py                 # lecture seule
  python docs/corpus/rules-memo-material-1539.py --self-test     # témoins
  python docs/corpus/rules-memo-material-1539.py --csv COPIE.csv # CONTROLE INVERSE
  python docs/corpus/rules-memo-material-1539.py --apply         # chirurgie

Codes de sortie :
  0  invariant tenu (ou chirurgie appliquée conforme au plan) ;
  2  invariant violé, ou témoin d'instrument en échec, ou plan d'édition
     non conforme (cellule manquante, ligne fautive absente ou dédoublée,
     compte de cellules différent de 40), ou structure de fichier inattendue.
"""
import csv
import io
import os
import re
import sys

BASE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "..", "Cards", "Rules")
DECK = "Argumentum Rules - Cards.csv"
PP = "Argumentum Rules - Cards Print and Play.csv"

LANGS = ["Text", "Text_en", "Text_ru", "Text_pt", "Text_ar", "Text_es", "Text_zh", "Text_fa"]

# ⚠ NE JAMAIS deduire la position des colonnes de la liste des langues : l'en-tete
# reel intercale `print_and_play` entre `Text_pt` et `Text_ar`. Un index construit
# « pk + les 8 langues » decale donc ar/es/zh d'un cran et perd fa — mesure fausse
# rendue ici en 35 au lieu de 40, alors que la garde C# en comptait 40. Les deux
# instruments se sont contredits : c'est ce qui a rendu le defaut visible. Le
# lecteur ci-dessous passe par l'EN-TETE et refuse une colonne attendue absente.

# Les 5 cartes qui portent une liste de matériel annoncant les cartes memo.
CARTES = ["Rules_02", "Rules_09", "Rules_11", "Rules_13", "RulesPP_02"]
PLANCHER = 40  # 5 cartes x 8 langues — egale a la mesure, pas en dessous

# Systemes de chiffres nommes par l'issue. `str.isdigit()` couvre les trois
# premiers ; les ideogrammes chinois sont dans la table CJK, en clair.
PLAGES = [("arabe occidental", "0", "9"),
          ("arabo-indien", "٠", "٩"),
          ("arabo-indien etendu (persan)", "۰", "۹")]
CHIFFRES_CJK = "〇一二三四五六七八九十百千"

# Mots « memo » par langue, MESURES sur le corpus. ayuda (es) et ajuda (pt) sont
# deux entrees distinctes : les confondre rend 39 au lieu de 40.
RX_MEMO = re.compile(
    "mémo|memo|мемо|ajuda|ayuda|recordatorio"
    "|تذكير|提示卡|备忘卡|یادآور",
    re.I | re.U)

# Cible declaree, par carte et par langue (decision editoriale, pas derivation).
# fr/en/pt/es : le chiffre part, majuscule en tete d'item.
# ru : le genitif pluriel « kart » est porte par le nombre -> nominatif « Karty ».
# zh : le classificateur zhang appartient au nombre -> il part avec lui.
# ar : le pluriel nu est la forme correcte sans numeral.
# fa : sans numeral le nom se met au pluriel, comme dans les quatre langues latines.
CIBLES = {
    "Rules_02": {
        "Text": "* Cartes mémo.", "Text_en": "* Memo cards.",
        "Text_ru": "* Карты Мемо",
        "Text_pt": "* Cartas de ajuda.", "Text_ar": "* بطاقات تذكير",
        "Text_es": "* Cartas recordatorio", "Text_zh": "* 提示卡",
        "Text_fa": "* کارت‌های یادآور",
    },
    "Rules_09": {
        "Text": "* Cartes mémo", "Text_en": "* Memo cards",
        "Text_ru": "* Карты Мемо",
        "Text_pt": "* Cartas de ajuda", "Text_ar": "* بطاقات تذكير",
        "Text_es": "* Cartas recordatorio", "Text_zh": "* 备忘卡",
        "Text_fa": "* کارت‌های یادآور",
    },
    "Rules_11": {
        "Text": "* Cartes mémo.", "Text_en": "* Memo cards",
        "Text_ru": "* Карты Мемо",
        "Text_pt": "* Cartas de ajuda-memória.", "Text_ar": "* بطاقات تذكير",
        "Text_es": "* Cartas de ayuda", "Text_zh": "* 备忘卡",
        "Text_fa": "* کارت‌های یادآور",
    },
    "Rules_13": {
        "Text": "* Cartes mémo", "Text_en": "* Memo cards",
        "Text_ru": "* Карты Мемо",
        "Text_pt": "* Cartas de ajuda", "Text_ar": "* بطاقات تذكير",
        "Text_es": "* Cartas de memo", "Text_zh": "* 备忘卡",
        "Text_fa": "* کارت‌های یادآور",
    },
    "RulesPP_02": {
        "Text": "* Cartes mémo.", "Text_en": "* Memo cards.",
        "Text_ru": "* Карты Мемо",
        "Text_pt": "* Cartas de ajuda.", "Text_ar": "* بطاقات تذكير",
        "Text_es": "* Cartas recordatorio", "Text_zh": "* 提示卡",
        "Text_fa": "* کارت‌های یادآور",
    },
}


# ─────────────────────────── detecteur ───────────────────────────
def ligne_memo(cellule):
    """Items de liste (ouvrant par « * ») qui mentionnent le mot memo de la langue."""
    return [ln for ln in (cellule or "").split("\n")
            if ln.strip().startswith("*") and RX_MEMO.search(ln)]


def chiffre_ouvrant(ligne):
    """(caractere, systeme) du chiffre qui ouvre l'item — ou None."""
    reste = ligne.strip()
    if not reste.startswith("*"):
        return None
    reste = reste[1:].lstrip()
    if not reste:
        return None
    c = reste[0]
    for nom, debut, fin in PLAGES:
        if debut <= c <= fin:
            return (c, nom)
    if c in CHIFFRES_CJK:
        return (c, "chiffre chinois")
    return None


TEMOINS = [
    ("fr chiffre ouest", "* 5 cartes mémo.", True, "arabe occidental"),
    ("ar arabo-indien", "* ٥ بطاقات تذكير", True, "arabo-indien"),
    ("fa etendu", "* ۵ کارتِ یادآور", True, "arabo-indien etendu (persan)"),
    ("zh + classificateur", "* 5张提示卡", True, "arabe occidental"),
    ("zh ideogramme seul", "* 五张备忘卡", True, "chiffre chinois"),
    ("TEMOIN corrige fr", "* Cartes mémo.", False, None),
    ("TEMOIN corrige ru", "* Карты Мемо", False, None),
    ("TEMOIN corrige zh", "* 备忘卡", False, None),
    ("TEMOIN corrige fa", "* کارت‌های یادآور", False, None),
    ("TEMOIN autre item", "* 1 paquet de cartes de scénario.", False, None),
]


def self_test():
    ok = True
    print("[SELF-TEST] 5 chiffres vus (4 systemes), 5 temoins corriges NON vus :")
    for label, ligne, attendu, systeme in TEMOINS:
        vu = bool(ligne_memo(ligne)) and chiffre_ouvrant(ligne) is not None
        bon = vu == attendu and (systeme is None or (chiffre_ouvrant(ligne) or ("", ""))[1] == systeme)
        ok &= bon
        print("  [%s] %-22s attendu=%-5s vu=%-5s systeme=%s"
              % ("PASS" if bon else "FAIL", label, attendu, vu,
                 (chiffre_ouvrant(ligne) or ("-", "-"))[1]))
    print("[SELF-TEST] %s" % ("PASS" if ok else "FAIL"))
    return 0 if ok else 2


# ─────────────────── lecture au dialecte du fichier ───────────────────
def lire(path):
    raw = open(path, "rb").read()
    return raw, raw.decode("utf-8-sig")


def spans_champs(ligne):
    """Spans BRUTS (guillemets compris) des champs d'une ligne, quote-aware."""
    spans, i, n, debut, inq = [], 0, len(ligne), 0, False
    while i < n:
        ch = ligne[i]
        if inq:
            if ch == '"':
                if i + 1 < n and ligne[i + 1] == '"':
                    i += 2
                    continue
                inq = False
                i += 1
                continue
            i += 1
            continue
        if ch == '"':
            inq = True
            i += 1
            continue
        if ch == ",":
            spans.append((debut, i))
            debut = i + 1
            i += 1
            continue
        i += 1
    spans.append((debut, n))
    return spans


def sans_guillemets(raw):
    if len(raw) >= 2 and raw[0] == '"' and raw[-1] == '"':
        return raw[1:-1].replace('""', '"')
    return raw


def rangees(texte):
    if not texte.endswith("\r\n"):
        raise ValueError("derniere rangee sans CRLF — dialecte inattendu")
    parts = texte[:-2].split("\r\n")
    if "" in parts:
        raise ValueError("rangee vide au milieu — dialecte inattendu")
    return parts


def table(path):
    """pk -> {colonne: valeur}, indexe par l'EN-TETE (jamais par position).

    Une colonne attendue absente est une ERREUR, pas un vide : `r.get(col)` sur
    une colonne renommee rend None, donc un texte vide, donc un compte a 0 qui se
    lit comme « rien a signaler ».
    """
    _, texte = lire(path)
    lignes = list(csv.reader(io.StringIO(texte, newline="")))
    entete = lignes[0]
    ix = {c: i for i, c in enumerate(entete)}
    manquantes = [c for c in ["pk"] + LANGS if c not in ix]
    if manquantes:
        raise ValueError("%s : colonne(s) absente(s) de l'en-tete : %s"
                         % (os.path.basename(path), ", ".join(manquantes)))
    out = {}
    for r in lignes[1:]:
        if not r or not r[0]:
            continue
        out[r[0]] = {c: (r[ix[c]] if ix[c] < len(r) else "") for c in ["pk"] + LANGS}
    return out


# ─────────────────────────── lecture seule ───────────────────────────
def rapport(paths, etiquette=""):
    """Compte les deux invariants. Retourne (rc, nb_cellules_avec_ligne)."""
    rc = 0
    total = 0
    for path in paths:
        t = table(path)
        nom = os.path.basename(path)
        print("=== %s ===" % nom)
        n_fautives = 0
        for pk, r in t.items():
            for col in LANGS:
                items = ligne_memo(r[col])
                if len(items) > 1:
                    print("  [doublon] %s.%s : %d items memo dans la meme cellule"
                          % (pk, col, len(items)))
                    rc = 2
                for it in items:
                    total += 1
                    ch = chiffre_ouvrant(it)
                    if ch is None:
                        continue
                    n_fautives += 1
                    print("  [chiffre] %s.%s : |%s| ouvre par « %s » (%s)"
                          % (pk, col, it, ch[0], ch[1]))
        print("  lignes de materiel fautives : %d" % n_fautives)
        if n_fautives:
            rc = 2
    print("\ncellules portant une ligne de materiel « memo » : %d (plancher %d)"
          % (total, PLANCHER))
    if total < PLANCHER:
        print("  !! plancher non atteint : l'invariant « pas de chiffre » serait tenu")
        print("     par disparition de la ligne, pas par correction")
        rc = 2
    print("VERDICT%s : %s" % (" " + etiquette if etiquette else "",
                              "OK" if rc == 0 else "INVARIANT VIOLE (rc=%d)" % rc))
    return rc, total


# ─────────────────────── controle inverse par mutation ───────────────────────
def controle_inverse():
    """Injecte le defaut d'avant #1539 dans une copie et exige que la sonde le voie.

    Un detecteur jamais vu rouge est un no-op silencieux (#1046) : ce controle
    rejoue le defaut sur les vrais octets, pas sur un temoin en memoire.
    """
    import tempfile
    src = os.path.join(BASE, DECK)
    _, texte = lire(src)
    avant = "* Cartes mémo."
    apres = "* 5 cartes mémo."
    if avant not in texte:
        print("[CONTROLE INVERSE] ligne corrigee introuvable dans %s — controle impossible" % DECK)
        return 2
    mute = texte.replace(avant, apres, 1)
    fd, tmp = tempfile.mkstemp(suffix=".csv")
    os.close(fd)
    try:
        open(tmp, "wb").write(b"\xef\xbb\xbf" + mute.encode("utf-8"))
        path_pp = os.path.join(BASE, PP)
        rc, total = rapport([tmp, path_pp], etiquette="(copie mutee injectee)")
        if rc != 2:
            print("[CONTROLE INVERSE] ECHEC : le defaut injecte n'a pas ete vu")
            return 2
        print("[CONTROLE INVERSE] PASS : le defaut injecte est vu (rc=2)")
        return 0
    finally:
        os.unlink(tmp)


# ─────────────────────────── chirurgie ───────────────────────────
def plan(path):
    """Editions a appliquer : (rangee, pk, colonne, search, cible, a, b, champ_brut)."""
    raw, texte = lire(path)
    lignes = rangees(texte)
    cols = [sans_guillemets(lignes[0][a:b]) for a, b in spans_champs(lignes[0])]
    if cols[0] != "pk":
        raise ValueError("%s : premiere colonne = %r" % (path, cols[0]))
    edits = []
    for li in range(1, len(lignes)):
        sp = spans_champs(lignes[li])
        # Dialecte mesure : 15 rangees du deck n'ont que 10 champs, la derniere
        # colonne (variant_class, vide) est absente. Sans consequence ici.
        if len(sp) == len(cols) - 1:
            if cols[-1] != "variant_class":
                raise ValueError("%s rangee %d : colonne absente inattendue" % (path, li))
        elif len(sp) != len(cols):
            raise ValueError("%s rangee %d : %d champs pour %d colonnes"
                             % (path, li, len(sp), len(cols)))
        pk = sans_guillemets(lignes[li][sp[0][0]:sp[0][1]])
        if pk not in CIBLES:
            continue
        for ci, col in enumerate(cols[:len(sp)]):
            if col not in CIBLES[pk]:
                continue
            a, b = sp[ci]
            champ = lignes[li][a:b]
            val = sans_guillemets(champ)
            cible = CIBLES[pk][col]
            items = [ln for ln in val.split("\n")
                     if chiffre_ouvrant(ln) is not None and RX_MEMO.search(ln)]
            if len(items) != 1:
                raise ValueError("%s %s.%s : %d ligne(s) fautive(s), attendu 1 : %r"
                                 % (path, pk, col, len(items), items))
            search = items[0]
            for s in (search, cible):
                if "," in s or '"' in s:
                    raise ValueError("%s %s.%s : , ou \" dans %r — remplacement brut non sur"
                                     % (path, pk, col, s))
            if val.count(search) != 1:
                raise ValueError("%s %s.%s : %d occurrence(s)" % (path, pk, col, val.count(search)))
            edits.append((li, pk, col, search, cible, a, b, champ))
    return edits, lignes


def chirurgie(apply_it):
    rc = 0
    for fn in (DECK, PP):
        path = os.path.join(BASE, fn)
        raw, texte = lire(path)
        if raw[:3] != b"\xef\xbb\xbf":
            print("  !! %s : BOM absent" % fn)
            return 2
        edits, lignes = plan(path)
        attendu = 32 if fn == DECK else 8
        print("=== %s : %d cellule(s) ===" % (fn, len(edits)))
        for li, pk, col, search, cible, a, b, champ in edits:
            print("  %-11s %-8s |%s|  ->  |%s|" % (pk, col, search, cible))
        if len(edits) != attendu:
            print("  !! %d cellule(s) pour %s, attendu %d" % (len(edits), fn, attendu))
            return 2
        if not apply_it:
            continue
        out = list(lignes)
        par_rangee = {}
        for e in edits:
            par_rangee.setdefault(e[0], []).append(e)
        for li, eds in par_rangee.items():
            # une meme rangee porte jusqu'a 8 editions : on applique de la FIN
            # vers le DEBUT, sinon les offsets suivants ne designent plus rien
            eds = sorted(eds, key=lambda e: -e[5])
            spans = sorted((e[5], e[6]) for e in eds)
            for k in range(1, len(spans)):
                if spans[k][0] < spans[k - 1][1]:
                    raise ValueError("rangee %d : editions qui se chevauchent" % li)
            cur = lignes[li]
            for (l2, pk, col, search, cible, a, b, champ) in eds:
                if cur[a:b] != champ:
                    raise ValueError("rangee %d col %s : span deplace" % (li, col))
                cur = cur[:a] + champ.replace(search, cible) + cur[b:]
            out[li] = cur
        newtexte = "\r\n".join(out) + "\r\n"
        octets = b"\xef\xbb\xbf" + newtexte.encode("utf-8")
        # controle structurel : le dialecte doit survivre a l'edition
        if octets.count(b"\r\n") != raw.count(b"\r\n"):
            print("  !! CRLF %d apres contre %d avant" % (octets.count(b"\r\n"), raw.count(b"\r\n")))
            return 2
        if (octets.count(b"\n") - octets.count(b"\r\n")) != (raw.count(b"\n") - raw.count(b"\r\n")):
            print("  !! LF internes modifies")
            return 2
        open(path, "wb").write(octets)
        print("  ECRIT %s : %d -> %d octets (CRLF %d=%d)"
              % (fn, len(raw), len(octets), raw.count(b"\r\n"), octets.count(b"\r\n")))
    if not apply_it:
        print("\n[dry-run] aucun fichier ecrit.")
    return rc


def main(argv):
    if "--self-test" in argv:
        return self_test()
    if "--csv" in argv:
        return controle_inverse()
    if "--apply" in argv:
        rc = chirurgie(True)
        if rc:
            return rc
        rc, _ = rapport([os.path.join(BASE, DECK), os.path.join(BASE, PP)], etiquette="(apres chirurgie)")
        return rc
    if "--dry-run" in argv:
        return chirurgie(False)
    rc, _ = rapport([os.path.join(BASE, DECK), os.path.join(BASE, PP)])
    return rc


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))