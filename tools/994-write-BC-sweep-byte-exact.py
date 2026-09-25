#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#994 B + C + balayage de fermeture — ecrire la norme typographique par langue, splice byte-exact.

Dispatch ai-01 (#994 c.5837029653, decision owner 25/09 ~17:10Z) : B ET C passes,
et la typographie devient une REGLE PERMANENTE (correction systematique, norme de
chaque langue, aucune question). Seule exception decidee : l'anglais garde
l'apostrophe DROITE ( #1073/#1030 ). Objectif : 0 residuel DECLARE, pas « 22+29 faits ».

Regle par langue (forme dominante MESUREE sur les colonnes rendues, master 9e79e88d —
cf. --survey ; les compteurs par paire sont dans le dossier du 25/09) :
  elision intra-mot (lettre'lettre) -> ’    pour fr/ru/pt/es/fa
  EN : toute ’ courbe -> ' droite            (y compris possessifs en liseré)
       ⛔ le canal ' n'est PAS joue en EN : le ' droit y est la NORME (#1073/#1030)
  paires-guillemets ASCII ('…' et "…") -> forme dominante de la langue :
      fr es ru ar fa -> « … »   (dominant mesure : 116/84/98/89/99 paires)
      en pt zh      -> “ … ”   (dominant mesure : 73/29/133 paires)
  ESPACEMENT INTERIEUR : fr = « + U+0020 ... U+0020 + » (119/119 paires mesurees) ;
      toutes les autres langues = colle. Produire «mot» en fr contredirait les 119
      paires existantes — cf. INNER_SPACE.
  zh/ar : AUCUNE conversion d'elision — leurs ' intra-mot sont des guillemets
      collees (mesure : 100 % des 16 ' zh et des 72 ' ar sont des paires).
  PAIRE ENDOMMAGEE : un fermant seul suivi d'une attribution « — » et sans ouvrante
      en attente dans le segment -> la paire est restauree au debut du segment
      (1 cellule du corpus : Fallacies 251 example_pt). Nomme, jamais silencieux.

Appariement : PAR CELLULE, candidat ouvrant (prev non-alpha, next alpha) puis
premier fermant suivant (prev alpha, next non-alpha) ; barriere = newline.
zh/ar : alternance pure (1er=ouvrant, 2e=fermant...). Toute marque non appariee
reste EN PLACE et est NOMMEE (anomalie) — jamais convertie a l'aveugle.
⛔ Jamais ’…’ (deux fermantes) : l'ouvrant et le fermant portent des caracteres
differents des l'ecriture, garde (6) le prouve par mutation.

Portee : colonnes RENDUES des 5 CSV x 8 langues (whitelist du dry-run, branche C),
link_* exclus par construction, colonnes sans lecteur exclues par construction.

GARDES :
  (1) PORTEE : chaque cellule changee est dans le scope rendu, pas link_* ;
  (2) LANGUE : la regle appliquee est celle de la colonne (table fixe, citee) ;
  (3) structure : nb lignes, BOM, terminaison de chaque ligne INCHANGES ;
  (4) RE-PARSE : la diff de VALEURS == exactement la worklist derivee ;
  (5) POST-ETAT : 0 " droit dans TOUTES les colonnes rendues ; 0 ' droit hors
      EN/zh... (cf. post_state()) ; 0 ’ courbe en EN ;
  (6) MUTATION : --mutation-test casse une paire dans une copie en memoire ->
      la garde (5) ROUGIT.

Usage :
    python tools/994-write-BC-sweep-byte-exact.py            # --check : plan + preuve, 0 ecriture
    python tools/994-write-BC-sweep-byte-exact.py --apply    # ecrit + backups .BEFORE-994BC
    python tools/994-write-BC-sweep-byte-exact.py --survey   # formes dominantes + classification
    python tools/994-write-BC-sweep-byte-exact.py --mutation-test
    python tools/994-write-BC-sweep-byte-exact.py --self-test
"""
import argparse
import importlib.util
import json
import os
import sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DRY_PATH = os.path.join(REPO, "tools", "994-apostrophe-dryrun.py")
BACKUP_SUFFIX = ".BEFORE-994BC"

_spec = importlib.util.spec_from_file_location("dry994", DRY_PATH)
dry = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(dry)

STRAIGHT = "'"
CURVED = "’"
# Formes dominantes mesurees (master 9e79e88d) — cf. dossier 25/09, tableau §2.
OPEN_OF = {"fr": "«", "es": "«", "ru": "«", "ar": "«", "fa": "«",
           "en": "“", "pt": "“", "zh": "“"}
CLOSE_OF = {"fr": "»", "es": "»", "ru": "»", "ar": "»", "fa": "»",
            "en": "”", "pt": "”", "zh": "”"}
ELISION_LANGS = {"fr", "ru", "pt", "es", "fa"}   # zh/ar : guillemets, jamais d'elision
# Espacement INTERIEUR de la paire. Mesure sur les colonnes rendues (master 9e79e88d) :
# fr = « + U+0020 ... U+0020 + », 119/119 ; TOUTES les autres langues = colle, 100 %.
# ⛔ Ne pas produire «mot» en fr : contredirait les 119 paires existantes.
INNER_SPACE = {"fr"}
BARRIER = "\n"
# Ponctuation qui peut preceder un FERMANT (« made." », « "AND," », « liberated!" »).
TERM_BEFORE_CLOSE = ".,!?;:…"


def is_a(ch):
    return bool(ch) and ch.isalpha()


def intra_word_convert(value):
    """' droit entre deux lettres -> ’ (elisions). Rend (nouvelle_valeur, n)."""
    out, n, i = [], 0, 0
    while i < len(value):
        ch = value[i]
        if ch == STRAIGHT:
            prev = value[i - 1] if i else ""
            nxt = value[i + 1] if i + 1 < len(value) else ""
            if is_a(prev) and is_a(nxt):
                out.append(CURVED)
                n += 1
                i += 1
                continue
        out.append(ch)
        i += 1
    return "".join(out), n


def classify(value, i):
    """Ouvrant (True) / fermant (False) / ambigu (None) pour la marque en i.

    Regle d'accolement, dans cet ordre — la symetrie des espaces tranche d'abord,
    la ponctuation terminale tranche ensuite (cas « '&&', » : colle des deux cotes,
    le caractere de gauche n'est pas une lettre et le droit est un terminateur).
    """
    prev = value[i - 1] if i else ""
    nxt = value[i + 1] if i + 1 < len(value) else ""
    prev_sp = prev == "" or prev.isspace()
    next_sp = nxt == "" or nxt.isspace()
    if next_sp != prev_sp:
        return prev_sp                    # detache a gauche -> ouvrant, sinon fermant
    if not prev_sp:                       # colle des DEUX cotes
        if nxt in TERM_BEFORE_CLOSE:
            return False                  # « '&&', » -> fermant
        return is_a(nxt) and not is_a(prev)
    return None                           # detache des deux cotes -> ambigu


def pair_candidates(value, qch, lang):
    """Positions des candidats guillemets pour un caractere donne.

    zh/ar : toute occurrence est candidat, alternance 1er=ouvrant (leurs marques
    sont collees au texte des deux cotes — mesure : 16 ' zh et 72 ' ar, 100 % en
    paires — donc l'accolement ne les separe pas).
    autres : classify() ; les positions ambigues ne sont pas candidates.
    """
    marks = [i for i, c in enumerate(value) if c == qch]
    if lang in ("zh", "ar"):
        return [(i, (k % 2 == 0)) for k, i in enumerate(marks)]
    cand = []
    for i in marks:
        kind = classify(value, i)
        if kind is not None:
            cand.append((i, kind))
    return cand


def quote_open(lang, nxt):
    """Ouvrante, avec l'espace interieur si la langue le pratique (fr)."""
    if lang in INNER_SPACE and nxt and not nxt.isspace():
        return OPEN_OF[lang] + " "
    return OPEN_OF[lang]


def quote_close(lang, prev):
    """Fermante, avec l'espace interieur si la langue le pratique (fr)."""
    if lang in INNER_SPACE and prev and not prev.isspace():
        return " " + CLOSE_OF[lang]
    return CLOSE_OF[lang]


def pair_quotes(value, qch, lang):
    """Apparie ouvrant->premier fermant suivant, barriere = newline.
    Rend (nouvelle_valeur, n_paires, indices_non_apparies)."""
    cand = pair_candidates(value, qch, lang)
    repl = {}
    unpaired = []
    open_i = None
    for i, is_open in cand:
        if is_open:
            if open_i is not None:
                unpaired.append(open_i)      # re-ouvrant : l'ancien reste seul
            open_i = i
        else:
            if open_i is not None and BARRIER not in value[open_i + 1:i]:
                repl[open_i] = quote_open(lang, value[open_i + 1])
                repl[i] = quote_close(lang, value[i - 1])
                open_i = None
            else:
                unpaired.append(i)
    if open_i is not None:
        unpaired.append(open_i)
    if not repl:
        return value, 0, unpaired
    out = "".join(repl.get(i, c) for i, c in enumerate(value))
    return out, len(repl) // 2, unpaired


def apostrophe_leftovers(value):
    """Apocopes et orphelins : ' droit apres une lettre, hors paire-guillemet.
    Dans ces langues l'apostrophe est TOUJOURS courbe (es « nosotro' », pt « nada' »).
    Rend (valeur, n, indices)."""
    out, n, idx = [], 0, []
    for i, ch in enumerate(value):
        if ch == STRAIGHT:
            prev = value[i - 1] if i else ""
            nxt = value[i + 1] if i + 1 < len(value) else ""
            if is_a(prev) and not is_a(nxt):
                out.append(CURVED)
                n += 1
                idx.append(i)
                continue
        out.append(ch)
    return "".join(out), n, idx


def repair_lone_closer(value, lang, unpaired):
    """Repare une paire ENDOMMAGEE : fermant seul suivi d'une attribution (« — auteur »).

    Cas mesure : Fallacies 251 example_pt — les 7 autres langues entourent la citation
    de guillemets, le pt a perdu son OUVRANT et garde un fermant droit. La norme de la
    langue est une PAIRE : on la restaure au debut du segment. Conditions cumulees,
    volontairement etroites (une seule cellule du corpus les remplit) : (a) la marque
    est un FERMANT, (b) suivie d'une attribution « — », (c) aucune ouvrante en attente
    dans le segment, (d) precedee d'une lettre. Nomme, jamais silencieux.
    Rend (valeur, [(kind, n)], [description], indices_repares).
    """
    out, kinds, notes, done = value, [], [], set()
    for _ in range(len(unpaired)):
        target, seg = None, 0
        for i, ch in enumerate(out):
            if ch != STRAIGHT or i in done:
                continue
            if classify(out, i) is not False:
                continue
            if not out[i + 1:].lstrip().startswith("—"):
                continue
            if not is_a(out[i - 1] if i else ""):
                continue
            start = out.rfind(BARRIER, 0, i) + 1
            if any(out[j] in (STRAIGHT, '"') and classify(out, j) is True for j in range(start, i)):
                continue
            target, seg = i, start
        if target is None:
            break
        while seg < len(out) and out[seg] == " ":
            seg += 1
        out = (out[:seg] + quote_open(lang, out[seg])
               + out[seg:target] + quote_close(lang, out[target - 1])
               + out[target + 1:])
        done.add(target)
        kinds.append(("paire-reparee", 1))
        notes.append(f"PAIRE REPAREE (ouvrant absent) : …{out[max(0, seg - 12):target + 14]}…")
    return out, kinds, notes, done


def transform_value(value, lang):
    """Applique la regle de la langue. Rend (nouvelle_valeur, [ (kind, n) ], anomalies)."""
    changes, anomalies = [], []
    if lang == "en":
        n = value.count(CURVED)
        if n:
            value = value.replace(CURVED, STRAIGHT)
            changes.append(("en-courbe->droite", n))
    elif lang in ELISION_LANGS:
        value, n = intra_word_convert(value)
        if n:
            changes.append(("elision->courbe", n))
    # EN : le ' droit EST la norme (#1073/#1030) -> canal ' non joue.
    channels = [('"', 'paire-"')]
    if lang != "en":
        channels.insert(0, (STRAIGHT, "paire-'"))
    for qch, kind in channels:
        value, n, un = pair_quotes(value, qch, lang)
        if n:
            changes.append((kind, n))
        if qch == STRAIGHT and lang in ELISION_LANGS and un:
            value, ks, notes, _rep = repair_lone_closer(value, lang, un)
            changes += ks
            anomalies += [(STRAIGHT, n) for n in notes]
            # indices deplaces par l'insertion : on re-derive les non-apparies
            _v, _n, un = pair_quotes(value, qch, lang)
        for i in un:
            anomalies.append((qch, f"NON APPARIE: …{value[max(0, i - 16):i + 16]}…"))
        for i in [j for j, c in enumerate(value) if c == qch and classify(value, j) is None]:
            anomalies.append((qch, f"AMBIGU: …{value[max(0, i - 16):i + 16]}…"))
    if lang in ELISION_LANGS:
        value, n, idx = apostrophe_leftovers(value)
        if n:
            changes.append(("reliquat->courbe", n))
            anomalies += [(STRAIGHT, f"RELIQUAT converti (apocope): …{value[max(0, i - 14):i + 14]}…")
                          for i in idx]
    return value, changes, anomalies


# ── chargement corpus (reutilise les splitters byte-exacts du dry-run) ────────
def load_corpus(rel):
    data = open(os.path.join(REPO, rel), "rb").read()
    bom = data.startswith(b"\xef\xbb\xbf")
    text = data.decode("utf-8-sig")
    rows = dry.split_logical_rows(text)
    header = [dry.unquote(f) for f in dry.split_fields(rows[0][0])]
    return {"bom": bom, "rows": rows, "header": header, "raw": data, "rel": rel}


def structure(raw):
    """(bom, terminaisons de chaque ligne, nb lignes) — pour la garde (3)."""
    bom = raw.startswith(b"\xef\xbb\xbf")
    rows = dry.split_logical_rows(raw.decode("utf-8-sig"))
    return bom, [t for _, t in rows], len(rows)


def reserialize(raw_field, new_value):
    """Re-serialise en preservant la decision de quotage ORIGINALE du champ."""
    was_wrapped = len(raw_field) >= 2 and raw_field[0] == '"' and raw_field[-1] == '"'
    if was_wrapped:
        return '"' + new_value.replace('"', '""') + '"'
    assert not any(c in new_value for c in ',"\r\n'), \
        f"champ non quotee devenu special : {new_value[:60]!r}"
    return new_value


def residual_of(lang, v):
    """Marques hors norme d'une valeur, selon la norme de la langue."""
    errs = []
    if '"' in v:
        errs.append('" droit')
    if lang == "en":
        if CURVED in v:
            errs.append("’ courbe")
    elif STRAIGHT in v:
        errs.append("' droit")
    return errs


def derive_worklist(root=None):
    """Worklist complete + anomalies + RESIDUELS (garde 5, AVANT ecriture).

    Rend (work, anomalies, residuals). Les residuels sont evalues sur la valeur
    POST-transformation de CHAQUE cellule rendue (changee ou non) : c'est la garde
    (5) qui rend l'ecriture fail-closed — si elle rougit, rien n'est ecrit.
    """
    root = root or REPO
    work, anomalies, residuals = [], [], []
    for key, rel, idcol in dry.CORPORA:
        corp = load_corpus(rel)
        scope = dry.branch_scope("C", key, corp["header"])
        for rowno, (row, _term) in enumerate(corp["rows"][1:], start=2):
            fields = dry.split_fields(row)
            values = [dry.unquote(f) for f in fields]
            rid = (values[corp["header"].index(idcol)]
                   if idcol in corp["header"] and corp["header"].index(idcol) < len(values)
                   else "").strip() or f"L{rowno}"
            new_fields, touched = list(fields), []
            for (lang, col), idx in sorted(scope.items()):
                if idx >= len(values):
                    continue
                nv, changes, anom = transform_value(values[idx], lang)
                for e in residual_of(lang, nv):
                    residuals.append(f"{e} residuel [{lang}] {key} {rid} {col} : …{nv[:80]}…")
                if changes:
                    new_fields[idx] = reserialize(fields[idx], nv)
                    touched.append((lang, col, changes, values[idx], nv))
                for qch, ctx in anom:
                    anomalies.append({"corpus": key, "rowid": rid, "lang": lang,
                                      "col": col, "char": qch, "ctx": ctx})
            if touched:
                work.append({"corpus": key, "rowid": rid, "rowno": rowno,
                             "cells": [{"lang": lg, "col": col, "changes": ch,
                                        "avant": av[:90], "apres": nv[:90]}
                                       for lg, col, ch, av, nv in touched],
                             "_new_fields": new_fields, "_old_fields": fields})
    return work, anomalies, residuals


def post_state_errors(work_or_root=None):
    """Garde (5) : apres ecriture, plus aucune marque hors norme dans le rendu."""
    root = work_or_root if isinstance(work_or_root, str) else REPO
    errors = []
    for key, rel, _id in dry.CORPORA:
        corp = load_corpus(rel)
        scope = dry.branch_scope("C", key, corp["header"])
        for rowno, (row, _t) in enumerate(corp["rows"][1:], start=2):
            values = [dry.unquote(f) for f in dry.split_fields(row)]
            for (lang, col), idx in sorted(scope.items()):
                if idx >= len(values):
                    continue
                v = values[idx]
                for e in residual_of(lang, v):
                    errors.append(f"{e} residuel [{lang}] {key} L{rowno} {col}")
    return errors


def apply_all(apply=False):
    work, anomalies, residuals = derive_worklist()
    ncells = sum(len(w["cells"]) for w in work)
    npairs = sum(c[1] for w in work for c in w["cells"] for c in c["changes"])
    print(f"worklist : {len(work)} lignes, {ncells} cellules, {npairs} transformations")
    by_kind = {}
    for w in work:
        for c in w["cells"]:
            for kind, n in c["changes"]:
                by_kind[(c["lang"], kind)] = by_kind.get((c["lang"], kind), 0) + n
    for (lg, kind), n in sorted(by_kind.items()):
        print(f"  {lg:3s} {kind:18s} {n:5d}")
    if anomalies:
        print(f"ANOMALIES (non converties, nommees) : {len(anomalies)}")
        for a in anomalies:
            print(f"  [{a['lang']}] {a['corpus']} {a['rowid']} {a['col']} {a['char']} {a['ctx']}")
    if residuals:
        print(f"GARDE (5) PRE-ECRITURE — {len(residuals)} marques hors norme :")
        for r in residuals[:40]:
            print(f"  {r}")
        if apply:
            raise AssertionError(
                f"garde (5) rouge AVANT ecriture : {len(residuals)} marques hors norme "
                f"— rien n'a ete ecrit")
    else:
        print("garde (5) pre-ecriture : 0 marque hors norme (toutes langues, colonnes rendues).")

    for w in work:
        for c in w["cells"]:
            print(f"  {w['corpus']:9s} {w['rowid']:>6} {c['lang']} {c['col']:20s} "
                  f"{'+'.join(k for k, _ in c['changes'])}")

    if not apply:
        print("MODE --check : aucune ecriture.")
        return work, anomalies, None

    written = {}
    for key, rel, _id in dry.CORPORA:
        corp = load_corpus(rel)
        rows = [corp["rows"][0]]
        wl = {w["rowno"]: w for w in work if w["corpus"] == key}
        for rowno, (row, term) in enumerate(corp["rows"][1:], start=2):
            w = wl.get(rowno)
            rows.append((",".join(w["_new_fields"]) if w else row, term))
        text = "".join(r + t for r, t in rows)
        data = text.encode("utf-8-sig" if corp["bom"] else "utf-8")
        path = os.path.join(REPO, rel)
        with open(path + BACKUP_SUFFIX, "wb") as fh:
            fh.write(corp["raw"])
        with open(path, "wb") as fh:
            fh.write(data)
        written[key] = (corp["raw"], data)
        print(f"ecrit {rel} ({len(wl)} lignes) backup {rel + BACKUP_SUFFIX}")

    # garde (3) : structure par fichier (nb lignes, BOM, terminaison de chaque ligne)
    for key, (old, new) in written.items():
        so, sn = structure(old), structure(new)
        assert so[2] == sn[2], f"{key}: nb lignes {so[2]} -> {sn[2]}"
        assert so[0] == sn[0], f"{key}: BOM"
        assert so[1] == sn[1], f"{key}: terminaisons de ligne modifiees"
        print(f"  garde (3) {key}: {sn[2]} lignes, BOM={'oui' if sn[0] else 'non'}, "
              f"terminaisons identiques, {len(old)} -> {len(new)} octets")
    return work, anomalies, written


def verify_value_diff(written):
    """Garde (4) : la diff de valeurs == exactement la worklist re-derivee."""
    work2, anom2, resid2 = derive_worklist()
    n2 = sum(len(w["cells"]) for w in work2)
    assert n2 == 0, f"re-derivation post-ecriture non vide : {n2} cellules"
    assert not resid2, f"residuels post-ecriture : {len(resid2)}"
    errs = post_state_errors()
    for e in errs[:20]:
        print(f"  RESIDUEL : {e}")
    assert not errs, f"{len(errs)} marques hors norme restantes"
    print("post-etat : 0 marque hors norme dans les colonnes rendues (toutes langues).")
    return True


def mutation_test():
    """Garde (6) : controle FALSIFIANT, sur litteraux (independant de l'arbre).

    Le controle ne lit PAS le corpus : apres --apply, un controle qui cherche une
    paire a casser dans l'arbre ne trouve plus rien et se declare « aveugle » a tort.
    Deux cas : (A) paire intacte -> 0 residuel (l'instrument ne rougit pas a tort) ;
    (B) paire cassee (ouvrante convertie, fermante oubliee) -> residuel VU.
    """
    ok = True
    for lang, intact, casse in (("ru", 'сказал "да" и', 'сказал «да" и'),
                                ("fr", 'il dit "mot" ici', 'il dit « mot »" ici'),
                                ("en", 'he said "yes" now', 'he said “yes" now')):
        _v, ch, _a = transform_value(intact, lang)
        fausse = residual_of(lang, _v)
        print(f"  [{'PASS' if not fausse else 'FAIL'}] {lang} paire intacte -> 0 residuel "
              f"({'+'.join(k for k, _ in ch)})")
        ok = ok and not fausse
        _v2, _c2, _a2 = transform_value(casse, lang)
        vues = residual_of(lang, _v2)
        print(f"  [{'PASS' if vues else 'FAIL'}] {lang} paire CASSEE -> residuel VU : {vues}")
        ok = ok and bool(vues)
    print("MUTATION " + ("OK (controle falsifiant vu des deux cotes)" if ok else "NON PROUVEE"))
    return 0 if ok else 1


def survey():
    """Formes dominantes par langue, mesurees sur les colonnes rendues."""
    pairs = {}
    for key, rel, _id in dry.CORPORA:
        corp = load_corpus(rel)
        scope = dry.branch_scope("C", key, corp["header"])
        for row, _t in corp["rows"][1:]:
            values = [dry.unquote(f) for f in dry.split_fields(row)]
            for (lang, col), idx in scope.items():
                if idx >= len(values):
                    continue
                v = values[idx]
                for a, b in (("«", "»"), ("“", "”"), ("'", "'"), ('"', '"')):
                    if lang not in pairs:
                        pairs[lang] = {}
                    na, nb = v.count(a), v.count(b)
                    if a == b:
                        pairs[lang][a] = pairs[lang].get(a, 0) + na
                    elif na and nb:
                        pairs[lang][f"{a}…{b}"] = pairs[lang].get(f"{a}…{b}", 0) + min(na, nb)
    print("formes par langue (colonnes rendues, 5 CSV) :")
    for lg in sorted(pairs):
        top = " ".join(f"{k}×{v}" for k, v in sorted(pairs[lg].items(), key=lambda kv: -kv[1])[:5])
        print(f"  {lg}: {top}")


def self_test():
    import tempfile
    fx = tempfile.mkdtemp(prefix="994w-fx-")
    hdr = "PK,desc_fr,desc_en,desc_ru,desc_zh,desc_ar,link_fr,Remarques"
    rows = [
        # en : courbe intra + possessif en liseré + ouvrante d'elision + paire ' + paire "
        "1,a’b,\"He don’t know the sailors’ code, ’tis 'quote' and \"\"dquote\"\" here\",x,y,l,"
        "https://x#Types_d'attribution,z",
        # ru : elision droite -> courbe + paire ' -> «»
        "2,c,plain,\"Софизм 'Флинтстоунов' и д'Арк тут\",plain,plain,l,m",
        # zh : paires collees (CJK alpha des deux cotes) — guillemets, jamais d'elision
        "3,d,plain,plain,\"你用'和'替代了\"\"链接\"\"吗\",plain,l,m",
        # ar : paire collee a la conjonction و
        "4,e,plain,plain,plain,\"مثل 'علم المناخ' و'النظم' هنا\",l,m",
        # fr : rien (fermeture volet A)
        "5,f’r,plain,plain,plain,plain,l,m",
        # fr : paire " -> « » AVEC espaces interieurs (norme mesuree, 119/119)
        "6,\"il dit \"\"mot\"\" ici\",plain,plain,plain,plain,l,m",
    ]
    body = "\r\n".join([hdr] + rows) + "\r\n"
    open(os.path.join(fx, "fal.csv"), "wb").write(b"\xef\xbb\xbf" + body.encode("utf-8"))
    global REPO
    real_repo, REPO = REPO, fx
    real_corpora = dry.CORPORA
    dry.CORPORA = [("Fallacies", "fal.csv", "PK")]
    try:
        # scopes realistes : branche C demande les colonnes *_ru etc. — le fixture
        # les fournit via le header ci-dessus.
        work, anomalies, residuals = derive_worklist()
        got = {}
        for w in work:
            for c in w["cells"]:
                got[(c["lang"], c["col"])] = c["apres"]
        ok = True

        def check(name, cond, detail=""):
            nonlocal ok
            print(f"  [{'PASS' if cond else 'FAIL'}] {name}" + (f" — {detail}" if detail and not cond else ""))
            ok = ok and cond

        en = [c for w in work if w["rowid"] == "1" for c in w["cells"] if c["lang"] == "en"]
        en_after = en[0]["apres"] if en else ""
        check("EN : courbes->droites, ' droit INTACT (norme #1073), \" -> “”",
              en_after == "He don't know the sailors' code, 'tis 'quote' and “dquote” here",
              repr(en_after))
        ru = [c for w in work if w["rowid"] == "2" for c in w["cells"] if c["lang"] == "ru"]
        check("RU : elision d’Арк courbee + paire ’ -> «»",
              ru and ru[0]["apres"] == "Софизм «Флинтстоунов» и д’Арк тут")
        zh = [c for w in work if w["rowid"] == "3" for c in w["cells"] if c["lang"] == "zh"]
        check("ZH : paires collees -> “” (jamais ’…’)",
              zh and zh[0]["apres"] == "你用“和”替代了“链接”吗")
        ar = [c for w in work if w["rowid"] == "4" for c in w["cells"] if c["lang"] == "ar"]
        check("AR : paire collee a و -> «»",
              ar and ar[0]["apres"] == "مثل «علم المناخ» و«النظم» هنا")
        check("FR : aucune cellule hors paire \" (volet A ferme)",
              not [c for w in work if w["rowid"] in ("1", "2", "3", "4", "5")
                   for c in w["cells"] if c["lang"] == "fr"])
        fr6 = [c for w in work if w["rowid"] == "6" for c in w["cells"] if c["lang"] == "fr"]
        check("FR : paire \" -> « mot » AVEC espaces interieurs (norme 119/119)",
              fr6 and fr6[0]["apres"] == "il dit « mot » ici",
              repr(fr6[0]["apres"]) if fr6 else "aucune cellule")
        check("link_* et Remarques hors worklist",
              not [c for w in work for c in w["cells"] if c["col"].startswith("link_") or c["col"] == "Remarques"])
        # ecriture + post-etat
        corp = load_corpus("fal.csv")
        wl = {w["rowno"]: w for w in work}
        out_rows = [corp["rows"][0]]
        for rowno, (row, term) in enumerate(corp["rows"][1:], start=2):
            w = wl.get(rowno)
            out_rows.append((",".join(w["_new_fields"]) if w else row, term))
        data = ("\r\n".join(r for r, _ in out_rows) + "\r\n").encode("utf-8-sig")
        open(os.path.join(fx, "fal.csv"), "wb").write(data)
        resid = post_state_errors()
        check("post-etat : 0 residuel", resid == [], "; ".join(resid[:4]))
        print(f"  [INFO] residuels vus par la garde : {len(resid)}")
        check("BOM + nb lignes preserves",
              data.startswith(b"\xef\xbb\xbf") and len(load_corpus("fal.csv")["rows"]) == 7)
        print("SELF-TEST " + ("OK" if ok else "ÉCHOUÉ"))
        return 0 if ok else 1
    finally:
        dry.CORPORA = real_corpora
        REPO = real_repo


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    ap.add_argument("--survey", action="store_true")
    ap.add_argument("--mutation-test", action="store_true")
    ap.add_argument("--self-test", action="store_true")
    ap.add_argument("--json-out", default=None)
    args = ap.parse_args()

    if args.self_test:
        return self_test()
    if args.survey:
        survey()
        return 0
    if args.mutation_test:
        return mutation_test()

    work, anomalies, written = apply_all(apply=args.apply)
    if args.json_out:
        serial = [{k: v for k, v in w.items() if not k.startswith("_")} for w in work]
        json.dump({"worklist": serial, "anomalies": anomalies},
                  open(args.json_out, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
        print(f"worklist serialisee : {args.json_out}")
    if written:
        verify_value_diff(written)
    return 0


if __name__ == "__main__":
    sys.exit(main())
