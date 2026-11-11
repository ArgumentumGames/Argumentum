#!/usr/bin/env python3
"""#135 — construction des zips de téléchargement, avec essai à blanc obligatoire.

Table de correspondance DÉCIDÉE (#135 c.6650903268, ai-01 05/11) — ce script l'exécute :

- **Cartes numériques** : un zip par langue des PNG de ``density-0/Fallacies``.
  Le nombre de faces attendues est DÉRIVÉ du CSV (colonne ``carte`` non vide) —
  jamais codé en dur (#1288 : le compte fait partie du contrat, il bouge avec le CSV).
  **Identité dérivée (11/11, pool #458 c.6788623855)** : chaque fichier PNG est
  joint à une rangée du deck via la colonne ``path`` (code hiérarchique) +
  séparateur **double point** + slug (``text_<lang>`` normalisé). Un PNG qui ne
  correspond à aucune rangée du deck est signalé **nominativement avec son PK
  CSV et son état ``carte``** (arbre pré-#1288 : ``1.2.2..`` = PK 96, retiré du
  deck le 05/09). Un compte juste ne suffit pas : un arbre de 175 dont le deck
  est faux doit rougir.
  Le dos partagé (``card_001.png``) est requis.
- **Manifeste** (``--build``) : ``manifest.json`` + ``manifest.md`` avec tailles
  et sha256 de chaque zip et de chaque fichier, deck dérivé, HEAD du dépôt.
- **Cartes légères** : uniquement si l'arbre de la release porte une ``density-1``.
  Sinon le paquet DISPARAÎT et la planche ``Fallacies_Web_Thumbnails_A4`` de chaque
  langue le remplace (décision de la table) — le signal « absent » est un résultat,
  pas une erreur de l'instrument.
- **Cartes mentales** : les SVG du DÉPÔT (``Cards/Fallacies/Mindmaps/<lang>/``),
  5 par langue : 3 Fallacies (main + content + links) + 2 Virtues (content + links).

Par défaut : **dry-run** (aucune écriture). ``--build`` écrit les zips et REFUSE
(exit 2) si une entrée manque ou si un compte dévie du CSV — le build réel a lieu
au tag, sur l'arbre de la release finale. ``--self-test`` plante un corpus synthétique
avec des trous connus et exige que CHACUN soit signalé : un instrument qui ne sait pas
rougir ne mesure rien.

Sortie : table lisible (ou ``--json``), exit 0 en dry-run même avec avertissements
(un avertissement du dry-run est une information sur l'arbre, pas un échec).
"""

from __future__ import annotations

import argparse
import csv
import hashlib
import json
import re
import sys
import tempfile
import zipfile
from pathlib import Path

LANGS = ["ar", "en", "es", "fa", "fr", "pt", "ru", "zh"]

# 5 SVG par langue, sur le modèle mesuré du dépôt (CLAUDE.md §Mindmaps) :
# Fallaces = triplet, Virtues = paire, variante FR-only « cards » hors paquet
# (décision de packaging à part, cf. essai à blanc 2026-11-05).
MINDMAP_PATTERNS = [
    "Fallacies_{lang}.svg",
    "Fallacies_{lang}.content.svg",
    "Fallacies_{lang}.links.svg",
    "Argumentum_Virtues_MindMap_{lang}.content.svg",
    "Argumentum_Virtues_MindMap_{lang}.links.svg",
]

TAXONOMY_CSV = Path("Cards/Fallacies") / "Argumentum Fallacies - Taxonomy.csv"


def expected_faces(repo: Path) -> int:
    """Faces attendues = lignes du CSV avec 'carte' non vide. Dérivé, jamais en dur."""
    with open(repo / TAXONOMY_CSV, encoding="utf-8-sig") as f:
        return sum(1 for r in csv.DictReader(f) if (r.get("carte") or "").strip())


FACE_RE = re.compile(r"^argumentum_fallacies_(?P<rest>.+)_face\.png$")


def sha256_file(p: Path) -> str:
    h = hashlib.sha256()
    with p.open("rb") as f:
        for chunk in iter(lambda: f.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def derive_deck(repo: Path) -> dict:
    """{path_code: row} pour les lignes 'carte' non vide. Abort sur path dupliqué."""
    deck = {}
    with open(repo / TAXONOMY_CSV, encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f)
        for row in reader:
            if not (row.get("carte") or "").strip():
                continue
            if "path" not in (reader.fieldnames or []):
                # corpus minimal (compte-seul) : pas de colonne path -> identité non vérifiable
                deck[f"#{row.get('pk') or row.get('PK')}"] = row
                continue
            code = (row.get("path") or "").strip()
            if not code:
                raise SystemExit(f"ABORT: ligne deck PK={row.get('PK')} a carte mais path vide")
            if code in deck:
                raise SystemExit(f"ABORT: deux lignes deck partagent path {code!r} "
                                 f"(PK {deck[code]['PK']} et {row.get('PK')})")
            deck[code] = row
    if not deck:
        raise SystemExit("ABORT: dérivation du deck vide — l'instrument est cassé, pas la donnée")
    return deck


def derive_full_map(repo: Path) -> dict:
    """{path: (PK, carte)} sur TOUTES les lignes — annotation ; le deck reste l'autorité."""
    fm = {}
    with open(repo / TAXONOMY_CSV, encoding="utf-8-sig", newline="") as f:
        for row in csv.DictReader(f):
            p = (row.get("path") or "").strip()
            if p and p not in fm:
                fm[p] = (row.get("PK") or row.get("pk") or "?", (row.get("carte") or "").strip())
    return fm


def _annotate(rest: str, full_map) -> str:
    """Identifie le code propre d'un nom de fichier dans le CSV complet (deck ou non)."""
    cands = [p for p in full_map if rest.startswith(p + "..")]
    if not cands:
        return "code CSV inconnu"
    p = max(cands, key=len)
    pk, carte = full_map[p]
    return f"PK {pk} · carte={'vide' if not carte else carte}"


def match_faces(deck: dict, face_names: list, full_map=None, want_slug: dict = None):
    """Joint les noms de fichiers aux codes ``path`` du deck (plus long préfixe ``..``).

    Format mesuré : ``argumentum_fallacies_<path>..<slug>_face.png`` (double point).
    Retourne (matched, extra, missing, collisions) — extra = [(nom, annotation PK)] ;
    collisions indécidables = le fichier doit rougir, jamais deviner.
    """
    want_slug = want_slug or {}
    full_map = full_map or {}
    homes, extra = {}, []
    for name in face_names:
        rest = FACE_RE.match(name).group("rest")
        cands = [p for p in deck if rest.startswith(p + "..")]
        if not cands:
            extra.append((name, _annotate(rest, full_map)))
            continue
        homes.setdefault(max(cands, key=len), []).append(name)

    matched, collisions = {}, {}
    for p, names in sorted(homes.items()):
        if len(names) == 1:
            matched[p] = names[0]
            continue
        want = want_slug.get(p)

        def _slug_of(n, p=p):
            r = FACE_RE.match(n).group("rest")
            return r[len(p) + 2:] if r.startswith(p + "..") else None
        hit = [n for n in names if want and _slug_of(n) == want]
        if len(hit) == 1:
            matched[p] = hit[0]
            extra.extend((n, _annotate(FACE_RE.match(n).group("rest"), full_map)
                          + " · collision gagnée par le slug")
                         for n in names if n != hit[0])
        else:
            collisions[p] = sorted(names)
    missing = sorted(set(deck) - set(matched))
    return matched, extra, missing, collisions


def survey_cards(tree: Path, lang: str, density: str) -> dict:
    """Inventorie density-N/Fallacies d'une langue. Signale, ne devine pas."""
    d = tree / lang / "Images" / density / "Fallacies"
    pngs = sorted(d.glob("*.png")) if d.is_dir() else []
    faces = [p for p in pngs if p.name != "card_001.png"]
    back = d / "card_001.png"
    named = [p.name for p in faces if FACE_RE.match(p.name)]
    return {
        "dir": str(d),
        "exists": d.is_dir(),
        "faces": len(faces),
        "back": back.is_file(),
        "files": len(pngs),
        "bytes": sum(p.stat().st_size for p in pngs),
        "named_faces": named,
        "unnamed": len(faces) - len(named),
    }


def survey_mindmaps(repo: Path, lang: str) -> dict:
    base = repo / "Cards/Fallacies/Mindmaps" / lang
    present, holes = [], []
    for pat in MINDMAP_PATTERNS:
        p = base / pat.format(lang=lang)
        (present if p.is_file() else holes).append(p.name)
    files = [base / n for n in present]
    return {
        "dir": str(base),
        "present": present,
        "holes": holes,
        "bytes": sum(p.stat().st_size for p in files),
    }


def survey(tree: Path, repo: Path, langs: list[str]) -> dict:
    want = expected_faces(repo)
    deck = derive_deck(repo)
    full_map = derive_full_map(repo)
    cards, light, mind = {}, {}, {}
    for lang in langs:
        c = survey_cards(tree, lang, "density-0")
        c["expected_faces"] = want
        c["face_delta"] = c["faces"] - want
        cards[lang] = c
        # densité légère : SA présence est une mesure, pas une évidence
        light[lang] = survey_cards(tree, lang, "density-1")
        light[lang]["expected_faces"] = want
        light[lang]["face_delta"] = light[lang]["faces"] - want
        mind[lang] = survey_mindmaps(repo, lang)
    return {
        "expected_faces_from_csv": want,
        "deck": deck,
        "full_map": full_map,
        "cards_png": cards,
        "cards_light": light,
        "mindmaps": mind,
    }


def light_verdict(light: dict, langs: list[str]) -> str:
    """Absence de density-1 = résultat tranché par la table, pas une panne."""
    carrying = [l for l in langs if light[l]["exists"]]
    if not carrying:
        return "ABSENT de l'arbre -> le paquet disparaît, Fallacies_Web_Thumbnails_A4 le remplace (décision table)"
    if len(carrying) != len(langs):
        return f"PARTIEL ({len(carrying)}/{len(langs)}) -> incohérent, à trancher avant build"
    return "PRÉSENT sur toutes les langues"


def hard_failures(s: dict, langs: list[str]) -> list[str]:
    """Ce qui bloque --build (exit 2). Le dry-run n'échoue pas sur ces points."""
    out = []
    for lang in langs:
        c = s["cards_png"][lang]
        if not c["exists"]:
            out.append(f"{lang}: density-0/Fallacies absente")
            continue
        if c["face_delta"] != 0:
            out.append(f"{lang}: {c['faces']} faces != {c['expected_faces']} attendues (CSV)")
        if not c["back"]:
            out.append(f"{lang}: dos partagé card_001.png manquant")
        # Identité dérivée (11/11) : jointure path→PNG, annotation PK.
        # Tollérance : si AUCUN fichier ne suit le motif canonique (corpus synthétique,
        # arbre historique), on retombe sur les gardes de compte — le self-test les couvre.
        named = c.get("named_faces", [])
        if named:
            want_slug = {p: (row.get("text_" + lang) or "").strip().lower().replace(" ", "_")
                         for p, row in s["deck"].items()}
            _m, extra, missing, collisions = match_faces(
                s["deck"], named, s["full_map"], want_slug)
            for name, why in extra:
                out.append(f"{lang}: PNG hors deck : {name} ({why})")
            if missing:
                out.append(f"{lang}: rangées deck sans PNG : "
                           + ", ".join(missing[:8]) + ("…" if len(missing) > 8 else ""))
            for p, names in sorted(collisions.items()):
                out.append(f"{lang}: collision indécidable sur path {p} : {names}")
        holes = s["mindmaps"][lang]["holes"]
        if holes:
            out.append(f"{lang}: SVG mindmap manquants : {', '.join(holes)}")
        lt = s["cards_light"][lang]
        if lt["exists"] and lt["face_delta"] != 0:
            out.append(f"{lang}: density-1 {lt['faces']} faces != {lt['expected_faces']} (CSV)")
        if lt["exists"] and lt.get("named_faces"):
            want_slug_l = {p: (row.get("text_" + lang) or "").strip().lower().replace(" ", "_")
                           for p, row in s["deck"].items()}
            _ml, extra_l, missing_l, coll_l = match_faces(
                s["deck"], lt["named_faces"], s["full_map"], want_slug_l)
            for name, why in extra_l:
                out.append(f"{lang}: density-1 PNG hors deck : {name} ({why})")
            if missing_l:
                out.append(f"{lang}: density-1 deck sans PNG : "
                           + ", ".join(missing_l[:8]) + ("…" if len(missing_l) > 8 else ""))
            for p, names in sorted(coll_l.items()):
                out.append(f"{lang}: density-1 collision sur path {p} : {names}")
    return out


def write_manifest(written: list[Path], s: dict, out: Path, repo: Path) -> None:
    """Tailles + sha256 par zip et par fichier ; comptes dérivés, jamais en dur."""
    import subprocess
    head = subprocess.run(["git", "rev-parse", "--short", "HEAD"],
                          capture_output=True, text=True, cwd=repo).stdout.strip()
    manifest = {
        "repo_head": head,
        "tool": "tools/135-build-download-zips.py",
        "deck_derived": {"faces": s["expected_faces_from_csv"],
                         "join": "CSV colonne path (code hiérarchique) + '..' + slug text_<lang>"},
        "zips": [],
    }
    lines = [f"# Manifeste #135 — HEAD {head}", "",
             f"- faces dérivées du CSV : {s['expected_faces_from_csv']}"]
    for zpath in written:
        per_file = {}
        with zipfile.ZipFile(zpath) as z:
            names = z.namelist()
        # les sources sont les fichiers originaux (zips déjà écrits) : empreintes du zip
        # et de chaque entrée relue depuis le zip (empreinte du contenu compressé = entrée).
        with zipfile.ZipFile(zpath) as z:
            for n in sorted(names):
                per_file[n] = hashlib.sha256(z.read(n)).hexdigest()
        entry = {"zip": zpath.name, "bytes": zpath.stat().st_size,
                 "sha256": sha256_file(zpath), "entries": len(names),
                 "per_file_sha256": per_file}
        manifest["zips"].append(entry)
        lines.append(f"- {zpath.name} : {entry['entries']} fichiers, "
                     f"{entry['bytes']:,} o, sha256 `{entry['sha256'][:16]}…`")
    out.mkdir(parents=True, exist_ok=True)
    (out / "manifest.json").write_text(json.dumps(manifest, ensure_ascii=False, indent=2),
                                       encoding="utf-8")
    (out / "manifest.md").write_text("\n".join(lines) + "\n", encoding="utf-8")


def build_zips(s: dict, tree: Path, repo: Path, langs: list[str], out: Path) -> list[Path]:
    """Écrit les zips. À n'appeler qu'après hard_failures() == []."""
    out.mkdir(parents=True, exist_ok=True)
    written = []
    for lang in langs:
        zpath = out / f"Argumentum_Cards_PNG_{lang}.zip"
        with zipfile.ZipFile(zpath, "w", zipfile.ZIP_DEFLATED) as z:
            for p in sorted((tree / lang / "Images/density-0/Fallacies").glob("*.png")):
                z.write(p, arcname=f"{lang}/{p.name}")
        written.append(zpath)
        if s["cards_light"][lang]["exists"]:
            zpath = out / f"Argumentum_Cards_PNG_light_{lang}.zip"
            with zipfile.ZipFile(zpath, "w", zipfile.ZIP_DEFLATED) as z:
                for p in sorted((tree / lang / "Images/density-1/Fallacies").glob("*.png")):
                    z.write(p, arcname=f"{lang}/{p.name}")
            written.append(zpath)
        zpath = out / f"Argumentum_MindMaps_{lang}.zip"
        with zipfile.ZipFile(zpath, "w", zipfile.ZIP_DEFLATED) as z:
            base = repo / "Cards/Fallacies/Mindmaps" / lang
            for pat in MINDMAP_PATTERNS:
                p = base / pat.format(lang=lang)
                if p.is_file():
                    z.write(p, arcname=f"{lang}/{p.name}")
        written.append(zpath)
    return written


def render(s: dict, langs: list[str]) -> str:
    lines = []
    lines.append(f"Faces attendues (CSV 'carte' non vide) : {s['expected_faces_from_csv']}")
    lines.append("")
    lines.append("## Cartes numériques (density-0/Fallacies)")
    lines.append("| langue | faces | attendu | écart | dos | taille |")
    lines.append("|---|---:|---:|---:|---|---:|")
    for lang in langs:
        c = s["cards_png"][lang]
        lines.append(
            f"| {lang} | {c['faces']} | {c['expected_faces']} | {c['face_delta']:+d} "
            f"| {'oui' if c['back'] else 'MANQUANT'} | {c['bytes']:,} o |"
        )
    lines.append("")
    lines.append(f"## Cartes légères (density-1) — {light_verdict(s['cards_light'], langs)}")
    lines.append("| langue | présente | faces | écart |")
    lines.append("|---|---|---:|---:|")
    for lang in langs:
        lt = s["cards_light"][lang]
        lines.append(
            f"| {lang} | {'oui' if lt['exists'] else 'non'} | {lt['faces']} | {lt['face_delta']:+d} |"
        )
    lines.append("")
    lines.append("## Cartes mentales (SVG du dépôt, 5/langue)")
    lines.append("| langue | présents | manquants | taille |")
    lines.append("|---|---:|---|---:|")
    for lang in langs:
        m = s["mindmaps"][lang]
        lines.append(
            f"| {lang} | {len(m['present'])}/5 | {', '.join(m['holes']) or '—'} | {m['bytes']:,} o |"
        )
    return "\n".join(lines)


def self_test() -> int:
    """Corpus synthétiques avec trous connus — chaque trou DOIT être signalé."""
    import shutil

    tmp = Path(tempfile.mkdtemp(prefix="135-selftest-"))
    fails = []
    try:
        # ── corpus A : sain côté cartes, trou côté mindmap + densité légère absente
        repo = tmp / "repo"
        tax = repo / TAXONOMY_CSV
        tax.parent.mkdir(parents=True)
        tax.write_text("pk,carte\n1,oui\n2,oui\n3,oui\n", encoding="utf-8")
        tree = tmp / "tree"
        for lang in ["fr", "en"]:
            d = tree / lang / "Images/density-0/Fallacies"
            d.mkdir(parents=True)
            for i in (1, 2, 3):
                (d / f"f{i:03d}_face.png").write_bytes(b"x" * 10)
            (d / "card_001.png").write_bytes(b"x" * 10)
            mdir = repo / "Cards/Fallacies/Mindmaps" / lang
            mdir.mkdir(parents=True)
            for pat in MINDMAP_PATTERNS:
                (mdir / pat.format(lang=lang)).write_text("<svg/>")
        # trou planté : un SVG manquant en 'en'
        (repo / "Cards/Fallacies/Mindmaps" / "en" / "Fallacies_en.links.svg").unlink()
        s = survey(tree, repo, ["fr", "en"])
        hf = hard_failures(s, ["fr", "en"])
        if not any("Fallacies_en.links.svg" in h for h in hf):
            fails.append("A: le SVG manquant n'est PAS signalé")
        if any("fr" in h for h in hf):
            fails.append("A: langue saine signalée à tort")

        # ── corpus B : dos manquant + écart de compte (doit être signalé, pas construit)
        (tree / "fr/Images/density-0/Fallacies/card_001.png").unlink()
        s = survey(tree, repo, ["fr", "en"])
        hf = hard_failures(s, ["fr", "en"])
        if not any("card_001.png" in h for h in hf):
            fails.append("B: le dos manquant n'est PAS signalé")
        (tree / "fr/Images/density-0/Fallacies/f004_face.png").write_bytes(b"x" * 10)
        s = survey(tree, repo, ["fr", "en"])
        hf = hard_failures(s, ["fr", "en"])
        if not any("faces != 3" in h for h in hf):
            fails.append("B: l'écart 4 faces / 3 attendues n'est PAS signalé")

        # ── corpus C : density-1 présente mais incohérente -> signalée
        d1 = tree / "fr/Images/density-1/Fallacies"
        d1.mkdir(parents=True)
        (d1 / "f001_face.png").write_bytes(b"x")
        s = survey(tree, repo, ["fr", "en"])
        hf = hard_failures(s, ["fr", "en"])
        if not any("density-1" in h for h in hf):
            fails.append("C: la density-1 incohérente n'est PAS signalée")

        # ── corpus D : SAIN -> build_zips() écrit des zips complets, lisibles, bien nommés
        #    (remarque relecture #2154 : la fonction d'écriture n'était jamais exécutée par
        #    l'essai à blanc — seuls survey/hard_failures l'étaient.)
        repo2 = tmp / "repo2"
        (repo2 / TAXONOMY_CSV).parent.mkdir(parents=True)
        (repo2 / TAXONOMY_CSV).write_text("pk,carte\n1,oui\n2,oui\n3,oui\n", encoding="utf-8")
        tree2 = tmp / "tree2"
        d = tree2 / "fr/Images/density-0/Fallacies"
        d.mkdir(parents=True)
        for i in (1, 2, 3):
            (d / f"f{i:03d}_face.png").write_bytes(b"x" * 10)
        (d / "card_001.png").write_bytes(b"x" * 10)
        d1 = tree2 / "fr/Images/density-1/Fallacies"
        d1.mkdir(parents=True)
        for i in (1, 2, 3):
            (d1 / f"f{i:03d}_face.png").write_bytes(b"x" * 10)
        (d1 / "card_001.png").write_bytes(b"x" * 10)
        mdir = repo2 / "Cards/Fallacies/Mindmaps/fr"
        mdir.mkdir(parents=True)
        for pat in MINDMAP_PATTERNS:
            (mdir / pat.format(lang="fr")).write_text("<svg/>")
        s = survey(tree2, repo2, ["fr"])
        hf = hard_failures(s, ["fr"])
        if hf:
            fails.append(f"D: corpus sain signalé à tort : {hf}")
        else:
            out = tmp / "zips"
            written = build_zips(s, tree2, repo2, ["fr"], out)
            names = sorted(p.name for p in written)
            want_names = sorted([
                "Argumentum_Cards_PNG_fr.zip",
                "Argumentum_Cards_PNG_light_fr.zip",
                "Argumentum_MindMaps_fr.zip",
            ])
            if names != want_names:
                fails.append(f"D: zips écrits {names} != {want_names} (le zip léger n'existe que si density-1 existe)")
            want_cards = sorted(f"fr/{n}" for n in [
                "f001_face.png", "f002_face.png", "f003_face.png", "card_001.png"])
            want_mind = sorted(f"fr/{pat.format(lang='fr')}" for pat in MINDMAP_PATTERNS)
            for zname, want_entries in (
                ("Argumentum_Cards_PNG_fr.zip", want_cards),
                ("Argumentum_Cards_PNG_light_fr.zip", want_cards),
                ("Argumentum_MindMaps_fr.zip", want_mind),
            ):
                with zipfile.ZipFile(out / zname) as z:
                    bad = z.testzip()
                    if bad:
                        fails.append(f"D: {zname} entrée corrompue : {bad}")
                        continue
                    got = sorted(z.namelist())
                    if got != want_entries:
                        fails.append(f"D: {zname} contenu {got} != {want_entries}")
        # ── corpus E : identité dérivée (11/11) — même COMPTE que le deck mais
        #    un PNG hors deck doit être nommé avec son PK ; retrait d'un PNG du
        #    deck doit être nommé comme manquant. Aucune langue saine signalée.
        repo3 = tmp / "repo3"
        tax3 = repo3 / TAXONOMY_CSV
        tax3.parent.mkdir(parents=True)
        tax3.write_text(
            "pk,path,carte,text_fr\n"
            "70,1.2,1,Préjugé\n"
            "96,1.2.2,,\n"
            "108,1.2.2.4,1,Appel à la nature\n"
            "200,1.3,1,Faux dilemme\n",
            encoding="utf-8-sig",
        )
        tree3 = tmp / "tree3"
        d3 = tree3 / "fr/Images/density-0/Fallacies"
        d3.mkdir(parents=True)
        for fn in ["argumentum_fallacies_1.2..préjugé_face.png",
                   "argumentum_fallacies_1.2.2..appel_à_la_nature_face.png",
                   "argumentum_fallacies_1.2.2.4..appel_à_la_nature_face.png",
                   "argumentum_fallacies_1.3..faux_dilemme_face.png",
                   "card_001.png"]:
            (d3 / fn).write_bytes(b"x" * 10)
        mdir3 = repo3 / "Cards/Fallacies/Mindmaps/fr"
        mdir3.mkdir(parents=True)
        for pat in MINDMAP_PATTERNS:
            (mdir3 / pat.format(lang="fr")).write_text("<svg/>")
        s3 = survey(tree3, repo3, ["fr"])
        hf3 = hard_failures(s3, ["fr"])
        # le compte est 4 faces vs 3 attendues ET l'extra doit être nommé PK 96
        if not any("PK 96" in h for h in hf3):
            fails.append(f"E: le PNG hors deck (arbre pré-#1288) n'est pas nommé PK 96 : {hf3}")
        if not any("hors deck" in h for h in hf3):
            fails.append("E: aucun 'PNG hors deck' signalé")
        # contrôle inverse : retirer un PNG du deck -> 'sans PNG' nommé
        (d3 / "argumentum_fallacies_1.3..faux_dilemme_face.png").unlink()
        (d3 / "argumentum_fallacies_1.2.2..appel_à_la_nature_face.png").unlink()
        s3 = survey(tree3, repo3, ["fr"])
        hf3 = hard_failures(s3, ["fr"])
        if not any("sans PNG" in h and "1.3" in h for h in hf3):
            fails.append(f"E: le retrait d'un PNG du deck n'est pas signalé 'sans PNG 1.3' : {hf3}")
        if any("hors deck" in h for h in hf3):
            fails.append(f"E: extra signalé alors qu'il a été retiré : {hf3}")
    finally:
        shutil.rmtree(tmp, ignore_errors=True)

    for f in fails:
        print(f"SELF-TEST FAIL — {f}")
    if fails:
        return 1
    print("SELF-TEST PASS — trous plantés (SVG, dos, écart de faces, density-1 incohérente) tous signalés, langue saine jamais signalée ; build_zips() vérifié sur corpus sain (zips complets, lisibles, bien nommés)")
    return 0


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--tree", help="répertoire Target de l'arbre de génération (ex: bin/Release/net9.0-windows/Target)")
    ap.add_argument("--repo", default=".", help="racine du dépôt (CSV + Mindmaps)")
    ap.add_argument("--langs", default=",".join(LANGS))
    ap.add_argument("--out", default=None, help="répertoire de sortie des zips (build)")
    ap.add_argument("--build", action="store_true", help="écrire les zips (refuse sur entrée manquante)")
    ap.add_argument("--json", action="store_true")
    ap.add_argument("--self-test", action="store_true", help="corpus synthétiques à trous connus")
    args = ap.parse_args()

    if args.self_test:
        return self_test()

    langs = [l.strip() for l in args.langs.split(",") if l.strip()]
    repo = Path(args.repo).resolve()
    tax = repo / TAXONOMY_CSV
    if not tax.is_file():
        # Remarque relecture #2154 : un --repo invalide tombait en FileNotFoundError brut
        # dans expected_faces(). Sortie propre, code 2, message qui nomme l'attendu.
        print(
            f"--repo invalide : {tax} introuvable.\n"
            f"  --repo attend la RACINE du dépôt (celle qui contient {TAXONOMY_CSV.as_posix()}), "
            f"reçu : {args.repo}",
            file=sys.stderr,
        )
        return 2
    if not args.tree:
        print("--tree requis (ou --self-test)", file=sys.stderr)
        return 2
    tree = Path(args.tree).resolve()

    s = survey(tree, repo, langs)
    if args.json:
        print(json.dumps(s, ensure_ascii=False, indent=2))
    else:
        print(render(s, langs))

    hf = hard_failures(s, langs)
    if hf:
        print()
        print("## Bloque un --build (le dry-run, lui, rend son verdict) :")
        for h in hf:
            print(f"- {h}")
    if args.build:
        if hf:
            print("BUILD REFUSÉ — entrées manquantes ou comptes en écart (ci-dessus).", file=sys.stderr)
            return 2
        out = Path(args.out or tree.parent / "zips-135").resolve()
        written = build_zips(s, tree, repo, langs, out)
        print()
        for p in written:
            print(f"écrit : {p.name} — {p.stat().st_size:,} o")
        write_manifest(written, s, out, repo)
        print(f"manifeste : {out / 'manifest.json'} (+ manifest.md)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
