#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""#1497-W — ecrire les cellules link_* validees par l'audit 261W, splice byte-exact.

Spec ai-01 (#1497 c.5831302892) : au plus 261 ecritures (256 interwiki + 5
sections), APRES audit d'extraits cellule par cellule ; les defectueuses sont
retenues et nommees (worklist), jamais ecrites. ⛔ Aucune URL d'une autre langue.

Worklist : tools/1497-write-261W-worklist.json — verdicts de relecture humaine
des extraits de tools/1497-audit-261W.json. Chaque entree ecrite doit exister
dans l'audit avec empty_on_master + q_match + non-disambiguation.

GARDES (meme famille que tools/1471-write-14E-byte-exact.py) :
  (1) COLLISION : chaque cible vise une cellule vide, verifiee a l'octet ;
  (2) LANGUE : le sous-domaine de l'URL == la colonne visee (garde ⛔ mechanicale) ;
  (3) 1408 lignes / CRLF / BOM invariants ; splice par offsets absolus ;
  (4) RE-PARSE COMPLET : les cellules changees == exactement la worklist ;
  (5) RETENUES : verifiees VIDES apres ecriture ;
  (6) MUTATION : --mutation-test injecte une retenue dans une copie en memoire
      et doit faire ROUGIR la garde (2)/(5) — la preuve que l'instrument mesure.

Usage :
    python tools/1497-write-261W-byte-exact.py            # --check : plan + preuve, 0 ecriture
    python tools/1497-write-261W-byte-exact.py --apply    # ecrit + backup .BEFORE-1497W
    python tools/1497-write-261W-byte-exact.py --head-check   # HEAD 200/404 sur les URL ecrites
    python tools/1497-write-261W-byte-exact.py --mutation-test
"""

import argparse
import csv
import io
import json
import os
import sys
import time
import urllib.parse
import urllib.request

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_PATH = os.path.join(REPO, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv")
WORKLIST = os.path.join(REPO, "tools", "1497-write-261W-worklist.json")
AUDIT = os.path.join(REPO, "tools", "1497-audit-261W.json")
BACKUP_SUFFIX = ".BEFORE-1497W"
BOM = b"\xef\xbb\xbf"
UA = "Argumentum-pipeline/1497-write (contact: jsboige@gmail.com)"
N_ROWS = 1408


def load_worklist():
    w = json.load(open(WORKLIST, encoding="utf-8"))
    ecrit = [(e["pk"], e["lang"], e["url"]) for e in w["ecritures"]]
    reten = [(r["pk"], r["lang"], r["raison"]) for r in w["retenues"]]
    return ecrit, reten, w


def check_against_audit(ecrit):
    """Chaque ecriture doit etre portee par l'audit avec les conditions de la spec."""
    aud = json.load(open(AUDIT, encoding="utf-8"))
    facts = {(e["pk"], e["lang"]): e for e in aud["details"]}
    for pk, lang, url in ecrit:
        e = facts.get((pk, lang))
        assert e, f"({pk},{lang}) absente de l'audit"
        assert e["empty_on_master"], f"({pk},{lang}) non vide a la re-mesure"
        assert e.get("q_match"), f"({pk},{lang}) Q ancre != Q cible"
        assert not e.get("target_missing"), f"({pk},{lang}) page cible absente"
        assert not e.get("target_disambiguation"), f"({pk},{lang}) page desambiguation"
        assert e["url"] == url, f"({pk},{lang}) url worklist {url} != audit {e['url']}"
    # borne de la spec
    assert len(ecrit) <= 261, f"borne 261 depassee : {len(ecrit)}"


def field_spans(record):
    spans, i, start, quoted = [], 0, 0, False
    while i < len(record):
        ch = record[i]
        if ch == '"':
            if quoted and i + 1 < len(record) and record[i + 1] == '"':
                i += 2
                continue
            quoted = not quoted
        elif ch == "," and not quoted:
            spans.append((start, i))
            start = i + 1
        i += 1
    spans.append((start, len(record)))
    return spans


def unquote(field):
    if len(field) >= 2 and field.startswith('"') and field.endswith('"'):
        return field[1:-1].replace('""', '"')
    return field


def split_raw(rec):
    return [rec[s:e] for s, e in field_spans(rec)]


def load_raw():
    with open(CSV_PATH, "rb") as fh:
        return fh.read()


def url_lang_ok(url, lang):
    pre = f"https://{lang}.wikipedia.org/wiki/"
    return url.startswith(pre)


def build(raw, ecrit, reten):
    bom = raw.startswith(BOM)
    text = raw[len(BOM):].decode("utf-8") if bom else raw.decode("utf-8")
    assert text.count("\r\n") == N_ROWS + 1, f"CRLF: {text.count(chr(13) + chr(10))} != {N_ROWS + 1}"
    records = text.split("\r\n")
    if records and records[-1] == "":
        records = records[:-1]
    header_fields = [unquote(f) for f in split_raw(records[0])]
    pk_idx = header_fields.index("PK")

    by_pk = {}
    for rec in records[1:]:
        fields = split_raw(rec)
        by_pk.setdefault(unquote(fields[pk_idx]).strip(), []).append(rec)
    for pk in {w[0] for w in ecrit} | {r[0] for r in reten}:
        assert len(by_pk.get(pk, [])) == 1, f"PK {pk}: {len(by_pk.get(pk, []))} enregistrements"

    # garde (2) ⛔ langue : AVANT toute ecriture
    for pk, lang, url in ecrit:
        assert url_lang_ok(url, lang), f"({pk},{lang}) URL d'une autre langue : {url}"

    edits = []
    for pk, lang, url in ecrit:
        col = f"link_{lang}"
        idx = header_fields.index(col)
        rec = by_pk[pk][0]
        spans = field_spans(rec)
        s, e = spans[idx]
        current = unquote(rec[s:e])
        assert current.strip() == "", f"COLLISION pk={pk} {col} non vide: {current[:60]!r}"
        rec_offset = text.index(rec)
        edits.append((rec_offset + s, rec_offset + e, url, pk, col))

    edits.sort(key=lambda t: t[0], reverse=True)
    new_text = text
    for s, e, url, pk, col in edits:
        new_text = new_text[:s] + url + new_text[e:]

    return (BOM if bom else b"") + new_text.encode("utf-8"), edits


def verify(old_raw, new_raw, ecrit, reten):
    def rows(raw):
        text = raw[len(BOM):].decode("utf-8") if raw.startswith(BOM) else raw.decode("utf-8")
        return list(csv.DictReader(io.StringIO(text)))

    old_rows, new_rows = rows(old_raw), rows(new_raw)
    assert len(old_rows) == len(new_rows) == N_ROWS, f"lignes {len(old_rows)}/{len(new_rows)}"
    expected = {(pk, f"link_{lang}"): url for pk, lang, url in ecrit}
    changed = []
    for o, n in zip(old_rows, new_rows):
        assert o["PK"] == n["PK"]
        for key in o:
            if o[key] != n[key]:
                changed.append((n["PK"].strip(), key))
    assert set(changed) == set(expected.keys()), f"cellules changees != worklist : {set(changed) ^ set(expected.keys())}"
    for (pk, col), url in expected.items():
        row = next(r for r in new_rows if r["PK"].strip() == pk)
        assert row[col] == url, f"{pk}.{col}: {row[col]!r} != {url}"
    check_forbidden(new_rows, ecrit, reten)
    return changed


def check_forbidden(rows, ecrit, reten):
    """Gardes (2) et (5) : URL ecrites dans la bonne langue, retenues VIDES."""
    by_pk = {r["PK"].strip(): r for r in rows}
    for pk, lang, url in ecrit:
        assert url_lang_ok(by_pk[pk][f"link_{lang}"], lang), f"({pk},{lang}) URL ecrite hors langue"
    for pk, lang, raison in reten:
        v = (by_pk[pk].get(f"link_{lang}") or "").strip()
        assert v == "", f"RETENUE NON VIDE ({pk},{lang}) [{raison}] : {v[:60]!r}"


def head_check(ecrit):
    """HEAD 200 sur chaque URL ecrite + controle 404 VU."""
    codes = {}
    for pk, lang, url in ecrit:
        base = url.split("#")[0]
        req = urllib.request.Request(base, headers={"User-Agent": UA}, method="HEAD")
        try:
            with urllib.request.urlopen(req, timeout=30) as r:
                codes[(pk, lang)] = r.status
        except urllib.error.HTTPError as e:
            codes[(pk, lang)] = e.code
        except Exception as e:  # noqa: BLE001
            codes[(pk, lang)] = f"ERR {e!r}"
        time.sleep(0.05)
    bad = {k: v for k, v in codes.items() if v != 200}
    n200 = sum(1 for v in codes.values() if v == 200)
    print(f"HEAD : {n200}/{len(ecrit)} x 200 ; non-200 : {len(bad)}")
    for k, v in sorted(bad.items(), key=lambda x: (int(x[0][0]), x[0][1]))[:20]:
        print(f"  {k}: {v}")
    # controle 404 : un titre fabrique DOIT rendre 404, sinon l'instrument est aveugle
    ctl = "https://fr.wikipedia.org/wiki/Zorblaxion_infundibulaire_inexistant_1497"
    req = urllib.request.Request(ctl, headers={"User-Agent": UA}, method="HEAD")
    try:
        with urllib.request.urlopen(req, timeout=30) as r:
            ctl_code = r.status
    except urllib.error.HTTPError as e:
        ctl_code = e.code
    print(f"controle 404 : {ctl_code}")
    assert ctl_code == 404, f"controle 404 non vu ({ctl_code}) : instrument non discriminant"
    assert not bad, f"{len(bad)} URL non-200"
    return codes


def mutation_test(ecrit, reten):
    """Injecte une retenue dans une COPIE en memoire : la garde doit rougir."""
    assert reten, "aucune retenue : mutation-test sans objet"
    old_raw = load_raw()
    new_raw, _ = build(old_raw, ecrit, reten)
    text = new_raw.decode("utf-8-sig")
    new_rows = list(csv.DictReader(io.StringIO(text)))
    pk, lang, raison = reten[0]
    for r in new_rows:
        if r["PK"].strip() == pk:
            r[f"link_{lang}"] = "https://fr.wikipedia.org/wiki/Mutation_test_1497"
            break
    try:
        check_forbidden(new_rows, ecrit, reten)
    except AssertionError as e:
        print(f"MUTATION VUE (garde rougit) : {e}")
        return 0
    print("MUTATION NON VUE : garde aveugle")
    return 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    ap.add_argument("--head-check", action="store_true")
    ap.add_argument("--mutation-test", action="store_true")
    args = ap.parse_args()

    ecrit, reten, w = load_worklist()
    print(f"worklist : {len(ecrit)} ecritures, {len(reten)} retenues nommees, "
          f"{len(w.get('retenues_sans_section', []))} sans section (non ecrites)")

    if args.mutation_test:
        return mutation_test(ecrit, reten)

    if args.head_check:
        head_check(ecrit)
        return 0

    check_against_audit(ecrit)

    old_raw = load_raw()
    new_raw, edits = build(old_raw, ecrit, reten)
    changed = verify(old_raw, new_raw, ecrit, reten)

    print(f"plan : {len(edits)} cellules")
    for pk, lang, url in sorted(ecrit, key=lambda x: (int(x[0]), x[1])):
        print(f"  pk={pk:>5} link_{lang:<4} -> {url}")
    print(f"verification : {len(changed)} cellules changees, exactement la worklist ; "
          f"{N_ROWS} lignes ; BOM/CRLF invariants ; {len(reten)} retenues vides")
    print(f"octets : {len(old_raw)} -> {len(new_raw)} (delta {len(new_raw) - len(old_raw):+d})")

    if not args.apply:
        print("MODE --check : aucune ecriture.")
        return 0

    backup = CSV_PATH + BACKUP_SUFFIX
    with open(backup, "wb") as fh:
        fh.write(old_raw)
    with open(CSV_PATH, "wb") as fh:
        fh.write(new_raw)
    reread = load_raw()
    assert reread == new_raw, "relecture != ce qui a ete ecrit"
    print(f"APPLIQUE. backup: {backup}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
