#!/usr/bin/env python3
"""133 / P24-37 — delta mesuré entre deux générations de l'OWL Vertus, oracle dérivé du CSV.

Le DoD de P24-37 (#458) demande trois choses :

  1. **0 IRI disparue** entre l'ancien blob committé et la sortie de `--generate-owl` ;
  2. **chaque `rdfs:seeAlso` retiré rattaché à un changement du CSV**, ou rétabli ;
  3. l'empreinte du nouveau blob, notée pour W2-03.

Cet outil répond aux deux premières et imprime la troisième. Il ne se contrôle jamais avec la
sortie qu'il vient de lire : l'attendu est **calculé depuis le CSV** (`--csv-old`, `--csv-new`)
par la règle d'émission de `VirtueOwlDocumentConfig`, puis comparé aux deux fichiers OWL.

  · surface  : les classes déclarées du namespace Argumentum — une IRI présente à l'ancien
               et absente du nouveau est une **perte** (sortie 3) ;
  · oracle   : pour chaque vertu (pk → fragment via `OwlIriRegistry`), chaque clé OWL que le
               générateur émet (prefLabel, definition, seeAlso, comment, aifAttackType,
               provenance, arêtes d'objet, hiérarchie) reçoit un attendu CSV, sur les deux
               faces. Un observable qui bouge sans que son entrée CSV bouge est **sans cause**
               (sortie 4) ; une entrée CSV qui bouge sans que l'observable bouge est **sans
               effet** (sortie 5). La projection est aussi rejouée face à l'ANCIEN couple
               (OWL committé, CSV de son commit) : c'est la calibration — un écart là est
               pré-existant, il est listé et jamais fondu dans le delta ;
  · hors-oracle : toute clé OWL qui change sans être couverte par l'oracle est nommée ;
  · seeAlso  : chaque valeur retirée est listée (pk, langue, URL), puis rattachée par
               `git log -S<url> <old>..<new>` au commit du CSV qui l'a fait disparaître.

Usage :
    python -I tools/133-owl-regen-delta.py --old <old.owl> --new <new.owl> \
        --csv-old <git-ref> --csv-new <git-ref> [--repo .] [--attribute] [--json out]

Codes : 0 cohérent · 2 instrument (fichier, ref ou colonne illisible) · 3 IRI perdue
       · 4 OWL changé sans cause CSV · 5 CSV changé sans effet OWL · 6 clé hors oracle.
"""
import argparse
import csv
import hashlib
import io
import json
import os
import re
import subprocess
import sys
import xml.etree.ElementTree as ET

XML_LANG = "{http://www.w3.org/XML/1998/namespace}lang"
SKOS = "http://www.w3.org/2004/02/skos/core#"
RDFS = "http://www.w3.org/2000/01/rdf-schema#"
AIF = "http://www.arg.dundee.ac.uk/aif#"
CSV_REL = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
REGISTRY_REL = "Generation/Converters/Argumentum.AssetConverter/Ontology/OwlIriRegistry.cs"
INPUT_COLUMNS = ["title_fr", "title_en", "description_fr", "description_en", "link_fr", "link_en",
                 "AIF_skosDirectRef", "AIF_criticalQuestion", "AIF_attackType", "AIF_attackedNode",
                 "AIF_skosOther", "path", "depth"]
LANG_KEYS = (("prefLabel", "fr"), ("prefLabel", "en"), ("definition", "fr"), ("definition", "en"),
             ("seeAlso", "fr"), ("seeAlso", "en"), ("comment", "fr"))
UNKNOWN = object()  # entrée CSV absente de la ref : ni cause ni absence, aveugle


def local(tag):
    return tag.split("}")[-1]


def die(msg, code=2):
    print(f"INSTRUMENT: {msg}", file=sys.stderr)
    sys.exit(code)


def git(repo, *args):
    p = subprocess.run(["git", "-C", repo, *args], capture_output=True)
    if p.returncode != 0:
        die(f"git {' '.join(args[:2])}… -> code {p.returncode}: {p.stderr.decode('utf-8', 'replace')[:300]}")
    return p.stdout


def parse_owl(path):
    try:
        root = ET.parse(path).getroot()
    except (OSError, ET.ParseError) as exc:
        die(f"{path}: {exc}")
    ns = (root.get("ontologyIRI") or "").rstrip("#")
    index, declared, upper_tags = {}, set(), []
    for e in root:
        name = local(e.tag)
        if name == "Annotation" or name == "AnnotationAssertion":
            prop = next((c.get("IRI") for c in e if local(c.tag) == "AnnotationProperty"), None)
            if name == "Annotation":
                subject, rest = ns, list(e)
            else:
                subject, rest = None, list(e)
            kind = value = lang = None
            for c in rest:
                cu = local(c.tag)
                if cu == "AnnotationProperty":
                    continue
                if cu == "IRI":
                    if subject is None:
                        subject = c.text or ""
                        continue
                    kind, value, lang = "IRI", c.text or "", ""
                elif cu == "Literal":
                    kind, value, lang = "L", c.text or "", (c.get(XML_LANG) or "").lower()
                    if c.get(XML_LANG) and c.get(XML_LANG) != c.get(XML_LANG).lower():
                        upper_tags.append(c.get(XML_LANG))
            if prop is not None and subject is not None and kind is not None:
                index.setdefault(("ann", subject, prop, lang), []).append(value)
        elif name == "ObjectPropertyAssertion":
            prop = next((c.get("IRI") for c in e if local(c.tag) == "ObjectProperty"), None)
            inds = [c.get("IRI") for c in e if local(c.tag) == "NamedIndividual"]
            if prop and len(inds) == 2:
                index.setdefault(("obj", inds[0], prop, ""), []).append(inds[1])
        elif name == "Declaration":
            for c in e:
                if local(c.tag) == "Class" and c.get("IRI"):
                    declared.add(c.get("IRI"))
    index = {k: tuple(sorted(v)) for k, v in index.items()}
    with open(path, "rb") as fh:
        digest = hashlib.sha256(fh.read()).hexdigest()
    return {"ns": ns, "index": index, "declared": declared, "sha256": digest,
            "size": os.path.getsize(path), "upper_lang_tags": len(upper_tags)}


def load_csv(repo, ref):
    text = git(repo, "show", f"{ref}:{CSV_REL}").decode("utf-8-sig")
    reader = csv.DictReader(io.StringIO(text, newline=""))
    order, rows = [], {}
    for r in reader:
        pk = (r.get("pk") or "").strip()
        if not pk:
            continue
        order.append(pk)
        rows[pk] = {(k or ""): ("" if v is None else v) for k, v in r.items()}
    if not order:
        die(f"{ref}:{CSV_REL} : aucune ligne")
    return rows, order, set(reader.fieldnames or [])


def load_registry(repo):
    try:
        with open(os.path.join(repo, REGISTRY_REL), encoding="utf-8") as fh:
            text = fh.read()
    except OSError as exc:
        die(f"registre illisible : {exc}")
    m = re.search(r"Virtues\s*=\s*new Dictionary<string, string>\s*\{(.*?)\n\s*\};", text, re.S)
    if not m:
        die("bloc « Virtues » introuvable dans OwlIriRegistry.cs")
    table = dict(re.findall(r'\["(\d+)"\]\s*=\s*"([^"]+)"', m.group(1)))
    if not table:
        die("registre Virtues vide")
    return table


def get_id(text):
    """VirtueOwlDocumentConfig.GetId sur les 14 noms de schemes du CSV (mots simples, pas de
    ponctuation) : étiquette en minuscule, suivantes en majuscule, espaces retirés. La calibration
    prouve cette reprise sur TOUTES les arêtes goodTenorOf du fichier ancien."""
    words = text.strip().split()
    return "".join(w[:1].lower() + w[1:] if i == 0 else w[:1].upper() + w[1:]
                   for i, w in enumerate(words)).replace(" ", "")


def derive_provenance(row):
    stored = (row.get("AIF_attackType") or "").strip()
    node = (row.get("AIF_attackedNode") or "").strip()
    if not stored or (row.get("AIF_skosOther") or "").strip():
        return "human-reviewed"
    expected = {"undercut": "RA-node", "undermine": "I-node", "rebut": "CA-node"}.get(stored)
    return "script-derived" if node == expected else "human-reviewed"


def expected_keys(rows, order, columns, registry, ns):
    """La projection CSV → OWL du générateur : {clé OWL: (attendu, entrées)}.
    attendu = None (rien d'émis) ou tuple triée de valeurs ; entrées = noms des colonnes lues."""
    by_path = {(r.get("path") or "").strip(): pk for pk, r in rows.items()}
    root = order[0]
    parent = {}
    for pk in order:
        r = rows[pk]
        path, depth = (r.get("path") or "").strip(), (r.get("depth") or "").strip()
        if depth.isdigit() and int(depth) <= 1:
            parent[pk] = root if pk != root else None
        else:
            parent[pk] = by_path.get(path.rsplit(".", 1)[0]) if "." in path else None
    children = {}
    for pk, par in parent.items():
        if par is not None:
            children.setdefault(par, []).append(pk)
    scheme = ns + "#" + registry[root] + "Scheme"
    out = {}

    def put(key, expected, inputs):
        out[key] = (UNKNOWN if any(c not in columns for c in inputs) else expected, tuple(inputs))

    for pk in order:
        row = rows[pk]
        subject = ns + "#" + registry[pk]
        known = lambda col: col in columns  # noqa: E731 — colonne absente de CE CSV
        val = lambda col: (row.get(col) or "") if known(col) else None  # noqa: E731
        for col, pred in (("title_fr", "prefLabel"), ("title_en", "prefLabel"),
                          ("description_fr", "definition"), ("description_en", "definition")):
            put(("ann", subject, SKOS + pred, col[-2:]), None if val(col) is None else (val(col),), (col,))
        for col in ("link_fr", "link_en"):
            v = val(col)
            put(("ann", subject, RDFS + "seeAlso", col[-2:]),
                None if (v is None or not v) else (v,), (col,))
        direct, cq = val("AIF_skosDirectRef"), val("AIF_criticalQuestion")
        put(("ann", subject, RDFS + "comment", "fr"),
            None if (direct is None or cq is None or not direct.strip() or not cq) else (cq,),
            ("AIF_skosDirectRef", "AIF_criticalQuestion"))
        attack, node = val("AIF_attackType"), val("AIF_attackedNode")
        has_attack = attack is not None and bool(attack.strip())
        put(("ann", subject, ns + "#aifAttackType", ""), (attack.strip(),) if has_attack else None, ("AIF_attackType",))
        put(("ann", subject, ns + "#aifAttackTypeProvenance", ""),
            (derive_provenance(row),) if has_attack else None,
            ("AIF_attackType", "AIF_attackedNode", "AIF_skosOther"))
        put(("obj", subject, ns + "#aifAttackedNode", ""),
            (AIF + node.strip(),) if (has_attack and node is not None and node.strip()) else None,
            ("AIF_attackType", "AIF_attackedNode"))
        tokens = [t.strip() for t in direct.split(",") if t.strip()] if direct else []
        put(("obj", subject, AIF + "goodTenorOf", ""),
            tuple(sorted(AIF + get_id(t) for t in tokens)) if tokens else None, ("AIF_skosDirectRef",))
        put(("ann", subject, SKOS + "broader", ""),
            (ns + "#" + registry[parent[pk]],) if parent.get(pk) else None, ("path", "depth"))
        kids = tuple(sorted(ns + "#" + registry[k] for k in children.get(pk, ())))
        put(("ann", subject, SKOS + "narrower", ""), kids if kids else None, ("path", "depth"))
        if pk == root:
            put(("ann", subject, SKOS + "topConceptOf", ""), (scheme,), ("path", "depth"))
            put(("ann", scheme, SKOS + "hasTopConcept", ""), (subject,), ("path", "depth"))
    return out


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--old", required=True, help="OWL/XML de référence (blob committé)")
    ap.add_argument("--new", required=True, help="OWL/XML régénéré")
    ap.add_argument("--csv-old", required=True, help="ref git du CSV qui a produit --old")
    ap.add_argument("--csv-new", required=True, help="ref git du CSV qui a produit --new")
    ap.add_argument("--repo", default=".", help="racine du dépôt (défaut : répertoire courant)")
    ap.add_argument("--attribute", action="store_true", help="rattacher chaque seeAlso retiré à son commit")
    ap.add_argument("--json", help="écrire le rapport complet")
    args = ap.parse_args()

    old_owl, new_owl = parse_owl(args.old), parse_owl(args.new)
    if old_owl["ns"] != new_owl["ns"]:
        die(f"namespaces différents : {old_owl['ns']} / {new_owl['ns']}")
    ns = old_owl["ns"]
    registry = load_registry(args.repo)
    old_csv, old_order, old_cols = load_csv(args.repo, args.csv_old)
    new_csv, new_order, new_cols = load_csv(args.repo, args.csv_new)
    missing = [c for c in INPUT_COLUMNS if c not in new_cols]
    if missing:
        die(f"colonnes absentes du CSV {args.csv_new} : {missing}")
    absent_old = [c for c in INPUT_COLUMNS if c not in old_cols]
    for pk in new_order:
        if pk not in registry:
            die(f"pk {pk} absent du registre (le générateur refuserait)")
    if set(old_order) - set(registry):
        die(f"pks du CSV {args.csv_old} absents du registre : {sorted(set(old_order) - set(registry))[:5]}")

    # ── 1. surface ──
    s_old = {i for i in old_owl["declared"] if i.startswith(ns + "#")}
    s_new = {i for i in new_owl["declared"] if i.startswith(ns + "#")}
    lost, gained = sorted(s_old - s_new), sorted(s_new - s_old)

    # ── 2. oracle, deux faces ──
    exp_old = expected_keys(old_csv, old_order, old_cols, registry, ns)
    exp_new = expected_keys(new_csv, new_order, new_cols, registry, ns)

    def mismatches(exp, owl):
        out = []
        for key, (expected, inputs) in exp.items():
            if expected is UNKNOWN:
                continue
            actual = owl["index"].get(key)
            if actual != expected:
                out.append({"key": [key[0], key[1].replace(ns + "#", ""), key[2].rsplit("#", 1)[-1].rsplit("/", 1)[-1], key[3]],
                            "csv": None if expected is None else [v[:80] for v in expected],
                            "owl": None if actual is None else [v[:80] for v in actual],
                            "inputs": list(inputs)})
        return out

    calib_old, calib_new = mismatches(exp_old, old_owl), mismatches(exp_new, new_owl)

    # ── 3. delta : l'OWL a bougé ⇔ son entrée CSV a bougé ──
    rows, spurious, missing_effect, unknown_cause = [], [], [], []
    for key in exp_new:
        exp_a, inputs_a = exp_old.get(key, (UNKNOWN, ()))
        exp_b, inputs_b = exp_new[key]
        act_a, act_b = old_owl["index"].get(key), new_owl["index"].get(key)
        owl_changed = act_a != act_b
        blind = exp_a is UNKNOWN
        input_changed = (not blind) and exp_a != exp_b
        if not (owl_changed or input_changed):
            continue
        rec = {"subject": key[1].replace(ns + "#", ""), "predicate": key[2].rsplit("#", 1)[-1].rsplit("/", 1)[-1],
               "lang": key[3], "owl_changed": owl_changed, "input_changed": input_changed,
               "owl_old": act_a, "owl_new": act_b, "csv_old": exp_a, "csv_new": exp_b, "inputs": list(inputs_b)}
        rows.append(rec)
        if owl_changed and blind:
            unknown_cause.append(rec)
        elif owl_changed and not input_changed:
            spurious.append(rec)
        elif input_changed and not owl_changed:
            missing_effect.append(rec)

    # ── 4. hors-oracle : clés OWL changées que la projection ne couvre pas ──
    keys = set(old_owl["index"]) | set(new_owl["index"])
    uncovered = []
    for key in sorted(keys, key=str):
        if key in exp_new and key in exp_old:
            continue
        a, b = old_owl["index"].get(key), new_owl["index"].get(key)
        if a != b:
            uncovered.append({"key": [key[0], key[1], key[2], key[3]], "old": a, "new": b})

    # ── 5. seeAlso nommés + rattachement ──
    reverse = {frag: pk for pk, frag in registry.items()}

    def see_also(index):
        out = set()
        for (channel, subject, prop, lang), vals in index.items():
            if channel == "ann" and prop == RDFS + "seeAlso" and subject.startswith(ns + "#"):
                pk = reverse.get(subject[len(ns) + 1:], subject[len(ns) + 1:])
                for v in vals:
                    out.add((pk, lang, v))
        return out

    def cells(rows_map):
        return {(pk, col[-2:], v) for pk, r in rows_map.items() for col in ("link_fr", "link_en")
                if (v := (r.get(col) or ""))}

    sa_old, sa_new = see_also(old_owl["index"]), see_also(new_owl["index"])
    removed, added = sorted(sa_old - sa_new), sorted(sa_new - sa_old)
    csv_removed = cells(old_csv) - cells(new_csv)
    csv_added = cells(new_csv) - cells(old_csv)
    attribution = {}
    if args.attribute:
        for pk, lang, url in removed:
            log = git(args.repo, "log", "--reverse", "--format=%h%x09%ad%x09%s", "--date=short",
                      f"-S{url}", f"{args.csv_old}..{args.csv_new}", "--", CSV_REL).decode("utf-8", "replace")
            attribution[f"{pk}|{lang}"] = [line[:140] for line in log.splitlines() if line.strip()][:3]

    # ── 6. littéraux remplacés, par langue ──
    replaced = {"fr": {"replaced": 0, "removed": 0, "added": 0}, "en": {"replaced": 0, "removed": 0, "added": 0}}
    for r in rows:
        lang = r["lang"]
        if (r["predicate"], lang) in LANG_KEYS and r["owl_changed"]:
            bucket = "replaced" if (r["owl_old"] is not None and r["owl_new"] is not None) else \
                     ("removed" if r["owl_old"] is not None else "added")
            replaced[lang][bucket] += 1

    by_pred = {}
    for r in rows:
        d = by_pred.setdefault(f"{r['predicate']}[{r['lang']}]", {"owl": 0, "input": 0})
        d["owl"] += int(r["owl_changed"])
        d["input"] += int(r["input_changed"])

    report = {"old": {"file": args.old, "sha256": old_owl["sha256"], "bytes": old_owl["size"], "csv_ref": args.csv_old},
              "new": {"file": args.new, "sha256": new_owl["sha256"], "bytes": new_owl["size"], "csv_ref": args.csv_new},
              "surface": {"old": len(s_old), "new": len(s_new), "lost": lost, "gained": gained},
              "registry": len(registry), "csv_rows": {"old": len(old_order), "new": len(new_order)},
              "columns_absent_from_old_csv": absent_old,
              "oracle_keys": len(exp_new),
              "calibration": {"old": calib_old, "new": calib_new},
              "by_predicate": by_pred, "literals": replaced,
              "owl_change_without_cause": spurious, "owl_change_cause_blind": unknown_cause,
              "csv_change_without_effect": missing_effect, "uncovered_changed_keys": uncovered,
              "see_also": {"removed": [list(x) for x in removed], "added": [list(x) for x in added],
                           "csv_cells_removed": sorted(list(x) for x in csv_removed),
                           "csv_cells_added": sorted(list(x) for x in csv_added),
                           "attribution": attribution},
              "rows": rows}
    if args.json:
        with open(args.json, "w", encoding="utf-8", newline="\n") as fh:
            json.dump(report, fh, ensure_ascii=False, indent=1)

    p = print
    p(f"ANCIEN sha256 {old_owl['sha256']}  {old_owl['size']} o  (CSV @{args.csv_old}, {len(old_order)} lignes)")
    p(f"NOUVEAU sha256 {new_owl['sha256']}  {new_owl['size']} o  (CSV @{args.csv_new}, {len(new_order)} lignes)")
    p(f"surface {ns}# : {len(s_old)} -> {len(s_new)} classes déclarées ; perdues={len(lost)} gagnées={len(gained)}")
    for i in lost:
        p(f"  PERDUE {i}")
    for i in gained:
        p(f"  GAGNEE {i}")
    if absent_old:
        p(f"colonnes absentes du CSV ancien (entrées aveugles) : {absent_old}")
    p(f"oracle : {len(exp_new)} clés OWL projetées depuis le CSV ; calibration ancienne face={len(calib_old)} écart(s), "
      f"nouvelle face={len(calib_new)} écart(s) ; xml:lang non minuscule : ancien={old_owl['upper_lang_tags']} "
      f"nouveau={new_owl['upper_lang_tags']}")
    for m in (calib_old + calib_new)[:12]:
        p(f"  CALIB {m}")
    p(f"clés OWL changées : {len(rows)} ; hors oracle : {len(uncovered)}")
    for u in uncovered[:12]:
        p(f"  HORS-ORACLE {u}")
    p("par prédicat[langue] — OWL changé / entrée CSV changée :")
    for k in sorted(by_pred):
        p(f"  {k:34s} {by_pred[k]['owl']:4d} / {by_pred[k]['input']:4d}")
    p(f"littéraux fr : remplacés={replaced['fr']['replaced']} retirés={replaced['fr']['removed']} ajoutés={replaced['fr']['added']}")
    p(f"littéraux en : remplacés={replaced['en']['replaced']} retirés={replaced['en']['removed']} ajoutés={replaced['en']['added']}")
    p(f"seeAlso : OWL retirés={len(removed)} ajoutés={len(added)} ; cellules link_fr/en du CSV retirées={len(csv_removed)} "
      f"ajoutées={len(csv_added)}")
    p(f"  les seeAlso OWL retirés sont exactement les cellules CSV retirées : {set(removed) == csv_removed}")
    p(f"  les seeAlso OWL ajoutés sont exactement les cellules CSV ajoutées   : {set(added) == csv_added}")
    for pk, lang, url in removed:
        p(f"  RETIRE pk={pk} [{lang}] {url}")
        for line in attribution.get(f"{pk}|{lang}", []):
            p(f"        {line}")
    for pk, lang, url in added:
        p(f"  AJOUTE pk={pk} [{lang}] {url}")
    p(f"OWL changé sans cause CSV : {len(spurious)} (dont entrée aveugle : {len(unknown_cause)})")
    for r in (spurious + unknown_cause)[:12]:
        p(f"  SANS-CAUSE {r['subject']} {r['predicate']}[{r['lang']}]")
    p(f"CSV changé sans effet OWL : {len(missing_effect)}")
    for r in missing_effect[:12]:
        p(f"  SANS-EFFET {r['subject']} {r['predicate']}[{r['lang']}] entrées={r['inputs']}")

    if lost:
        sys.exit(3)
    if spurious or unknown_cause:
        sys.exit(4)
    if missing_effect:
        sys.exit(5)
    if uncovered:
        sys.exit(6)
    p("VERDICT: coherent — 0 IRI perdue ; chaque changement OWL a sa cause CSV et réciproquement ; rien hors oracle")
    sys.exit(0)


if __name__ == "__main__":
    main()
