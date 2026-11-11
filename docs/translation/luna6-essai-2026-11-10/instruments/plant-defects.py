# #1520 phase 0 — générateur de défauts plantés (copie de travail, JAMAIS Cards/**).
# ~30 défauts, <=1/nœud, répartis sur 5 grappes et 7 langues, 7 classes, graine déterministe 1520.
# Sorties : planted-clusters.json (corpus muté, même structure que clusters.json)
#           planted-defects.json (manifeste-témoin : chaque défaut avec original + planté)
import json, os, random, re, sys, hashlib, copy, io

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding="utf-8", errors="replace")

SEED = 1520
# Repertoire de travail (clusters.json en entree, sorties a cote) : argument optionnel,
# sinon le dossier de ce script — aucun chemin machine en dur.
BASE = sys.argv[1] if len(sys.argv) > 1 else os.path.dirname(os.path.abspath(__file__))
CL = json.load(open(os.path.join(BASE, "clusters.json"), encoding="utf-8"))
LANGS = ["en", "ru", "pt", "es", "ar", "zh", "fa"]
QUOTA = {"5.2": 2, "4.1": 3, "6.1": 5, "2.1": 8, "6.3": 12}   # total 30
CLASSES = {1: "contresens (négation inversée)", 2: "exemple échangé avec un nœud frère",
           3: "texte FR laissé dans la cellule cible", 4: "proposition amputée",
           5: "mauvais registre (tutoiement / « You »)", 6: "nom ou nombre changé",
           7: "exemple hors-sujet (gré d'une autre grappe)"}

rng = random.Random(SEED)

# ---------- transformations par classe ----------
NEG_PREFIX = {"en": "It is not the case that ", "ru": "Неверно, что ", "pt": "Não é o caso que ",
              "es": "No es el caso de que ", "ar": "ليس صحيحًا أن ", "zh": "情况并非如此：",
              "fa": "چنین نیست که "}

def t_negation(l, s):
    pre = NEG_PREFIX[l]
    return pre + (s[0].lower() + s[1:] if l in ("en", "pt", "es", "ru") and s[:1].isupper() else s)

def t_amputee(l, s):
    m = re.search(r"[.!?。！？]", s)
    if m and m.end() < len(s) - 10:
        return s[:m.end()]
    cut = max(20, int(len(s) * 0.4))
    head = s[:cut]
    if " " in head:
        head = head.rsplit(" ", 1)[0]
    return head + "…"

REGISTRE = {"ru": [("Вы ", "Ты "), ("вы ", "ты "), ("Вас ", "Тебя "), ("вас ", "тебя "),
                   ("вам ", "тебе "), ("Вами ", "Тобой ")],
            "pt": [("você ", "tu "), ("Você ", "Tu ")],
            "es": [("usted ", "tú "), ("Usted ", "Tú ")],
            "zh": [("您", "你")]}

def t_registre(l, s):
    if l == "en":
        return "You " + (s[0].lower() + s[1:] if s[:1].isupper() else s)
    for a, b in REGISTRE.get(l, []):
        if a in s:
            return s.replace(a, b, 1)
    return None

NUM_WORDS = {"en": [("one ", "two "), ("two ", "three "), ("first ", "second ")],
             "ru": [("один ", "два "), ("два ", "три "), ("первый ", "второй ")],
             "pt": [("um ", "dois "), ("dois ", "três "), ("primeiro ", "segundo ")],
             "es": [("uno ", "dos "), ("dos ", "tres "), ("primero ", "segundo ")],
             "ar": [("واحد ", "اثنان ")],
             "zh": [("第一", "第二")],
             "fa": [("یک ", "دو ")]}

def t_nom_nombre(l, s, names=None):
    m = re.search(r"\d+", s)
    if m:
        d = m.group(0)
        new = str(int(d) + 3)
        if new == d:
            new = str(int(d) + 7)
        return (s[:m.start()] + new + s[m.end():], f"chiffre {d}->{new}")
    for a, b in NUM_WORDS.get(l, []):
        if a in s:
            return (s.replace(a, b, 1), f"mot-nombre {a.strip()}->{b.strip()}")
    # repli : nom propre remplacé par un autre nom propre de la grappe (même langue)
    if names:
        for m in re.finditer(r"[A-ZÀ-ÝЁ][a-zà-ÿа-яё]{2,}", s):
            if m.start() == 0 or s[max(0, m.start() - 2):m.start()].endswith((". ", "! ", "? ")):
                continue  # initial de phrase : ce serait un mot courant, pas un nom
            others = [n for n in names if n != m.group(0)]
            if others:
                b = rng.choice(others)
                return (s[:m.start()] + b + s[m.end():], f"nom {m.group(0)}->{b}")
    return None

def harvest_names(nodes, lang):
    """Noms propres (majuscule initiale, script latin/cyrillique) récoltés dans la grappe, même langue."""
    names = set()
    for n in nodes:
        for ch in ("desc", "example"):
            for m in re.finditer(r"[A-ZÀ-ÝЁ][a-zà-ÿа-яё]{2,}", n.get(f"{ch}_{lang}", "")):
                names.add(m.group(0))
    return sorted(names)

def parent(p):
    return p.rsplit(".", 1)[0] if "." in p else ""

def cell(node, champ, l):
    return node.get(f"{champ}_{l}", "")

def set_cell(node, champ, l, v):
    node[f"{champ}_{l}"] = v

# ---------- éligibilité ----------
def applicable(cid, nodes, node, l, champ):
    s = cell(node, champ, l)
    if cid in (1, 3, 4, 5, 6) and not s:
        return False
    if cid == 3 and not node.get(f"{champ}_fr"):
        return False
    if cid == 4 and len(s) < 40:
        return False
    if cid == 5:
        if l == "en":
            return bool(s)
        if l not in REGISTRE:
            return False
        return any(a in s for a, _ in REGISTRE[l])
    if cid == 6:
        if re.search(r"\d+", s):
            return True
        if any(a in s for a, _ in NUM_WORDS.get(l, [])):
            return True
        names = harvest_names(nodes, l)
        for m in re.finditer(r"[A-ZÀ-ÝЁ][a-zà-ÿа-яё]{2,}", s):
            if m.start() == 0 or s[max(0, m.start() - 2):m.start()].endswith((". ", "! ", "? ")):
                continue
            if [n for n in names if n != m.group(0)]:
                return True
        return False
    return True  # 2 et 7 vérifiés par le fournisseur de partenaire/donneur

# ---------- affectation ----------
planted = copy.deepcopy(CL)
defects = []
touched = set()   # pk déjà défectés
global_idx = 0

for cid in ["5.2", "4.1", "6.1", "2.1", "6.3"]:
    nodes = planted["clusters"][cid]
    remaining = QUOTA[cid]
    order = list(nodes)
    rng.shuffle(order)
    pos = 0
    while remaining > 0 and pos < len(order):
        node = order[pos]
        if node["pk"] in touched:
            pos += 1
            continue
        # langue indexée sur les défauts POSÉS (len(defects)), pas sur l'idx de classe :
        # sinon classe c ⟺ idx≡c-1 (mod 7) fixe aussi la langue (couplage mesuré : classe 6 → toujours ru)
        lang = LANGS[len(defects) % 7]
        want_class = (global_idx % 7) + 1
        champ = "desc" if global_idx % 2 == 0 else "example"
        # chercher une classe applicable en commençant par want_class
        for off in range(7):
            c = ((want_class - 1 + off) % 7) + 1
            if c == 2:
                # échange fraternel : partenaire disponible ?
                par = parent(node["path"])
                if not par:
                    continue
                sibs = [n for n in nodes if n is not node and n["pk"] not in touched
                        and parent(n["path"]) == par and cell(node, "example", lang)
                        and cell(n, "example", lang)]
                if not sibs or remaining < 2:
                    continue
                sib = sibs[0]
                a, b = cell(node, "example", lang), cell(sib, "example", lang)
                set_cell(node, "example", lang, b)
                set_cell(sib, "example", lang, a)
                defects.append({"pk": node["pk"], "path": node["path"], "langue": lang,
                                "champ": "example", "classe": 2, "label": CLASSES[2],
                                "original": a, "plante": b, "note": f"échangé avec PK {sib['pk']}"})
                defects.append({"pk": sib["pk"], "path": sib["path"], "langue": lang,
                                "champ": "example", "classe": 2, "label": CLASSES[2],
                                "original": b, "plante": a, "note": f"échangé avec PK {node['pk']}"})
                touched.add(node["pk"]); touched.add(sib["pk"])
                remaining -= 2; global_idx += 1
                break
            elif c == 7:
                # exemple hors-sujet : donneur d'une AUTRE grappe
                donors = [d for cl2, ns in planted["clusters"].items() if cl2 != cid
                          for d in ns if d["pk"] not in touched and cell(d, "example", lang)
                          and d["pk"] != node["pk"]]
                if not donors or not cell(node, "example", lang):
                    continue
                don = rng.choice(donors)
                orig = cell(node, "example", lang)
                new = cell(don, "example", lang)
                set_cell(node, "example", lang, new)
                defects.append({"pk": node["pk"], "path": node["path"], "langue": lang,
                                "champ": "example", "classe": 7, "label": CLASSES[7],
                                "original": orig, "plante": new,
                                "note": f"remplacé par l'exemple de PK {don['pk']} (grappe {don['path'].split('.')[0]}.{don['path'].split('.')[1] if '.' in don['path'] else ''})"})
                touched.add(node["pk"]); remaining -= 1; global_idx += 1
                break
            else:
                if not applicable(c, nodes, node, lang, champ):
                    # essayer l'autre champ pour 1/3/4/5/6
                    alt = "example" if champ == "desc" else "desc"
                    if applicable(c, nodes, node, lang, alt):
                        champ_eff = alt
                    else:
                        continue
                else:
                    champ_eff = champ
                s = cell(node, champ_eff, lang)
                if c == 1:
                    new = t_negation(lang, s)
                elif c == 3:
                    new = node[f"{champ_eff}_fr"]
                elif c == 4:
                    new = t_amputee(lang, s)
                elif c == 5:
                    new = t_registre(lang, s)
                elif c == 6:
                    new, note6 = t_nom_nombre(lang, s, harvest_names(nodes, lang))
                if new is None or new == s:
                    continue
                set_cell(node, champ_eff, lang, new)
                note = note6 if c == 6 else ""
                defects.append({"pk": node["pk"], "path": node["path"], "langue": lang,
                                "champ": champ_eff, "classe": c, "label": CLASSES[c],
                                "original": s, "plante": new, "note": note})
                touched.add(node["pk"]); remaining -= 1; global_idx += 1
                break
        pos += 1
    if remaining:
        print(f"WARN grappe {cid}: {remaining} défaut(s) non posés (épuisement candidats)")

# ---------- invariants ----------
assert len(defects) == 30, f"total défauts {len(defects)} != 30"
pks = [d["pk"] for d in defects]
assert len(pks) == len(set(pks)), "un nœud porte >1 défaut"
langs_seen = {d["langue"] for d in defects}
classes_seen = {d["classe"] for d in defects}
assert langs_seen == set(LANGS), f"langues couvertes {sorted(langs_seen)}"
assert classes_seen == set(range(1, 8)), f"classes couvertes {sorted(classes_seen)}"
# FR jamais touché
for cid, nodes in planted["clusters"].items():
    for n in nodes:
        for k, v in n.items():
            if k.endswith("_fr"):
                assert v == CL["clusters"][cid][[x["pk"] for x in CL["clusters"][cid]].index(n["pk"])][k], f"FR muté PK {n['pk']}"
# cellules non ciblées inchangées : re-diff complet
targeted = {(d["pk"], f"{d['champ']}_{d['langue']}") for d in defects}
for cid, nodes in planted["clusters"].items():
    src = {x["pk"]: x for x in CL["clusters"][cid]}
    for n in nodes:
        for k, v in n.items():
            if k in ("pk", "path", "titre_fr", "carte") or k.endswith("_fr"):
                continue
            if (n["pk"], k) not in targeted:
                assert v == src[n["pk"]][k], f"cellule non ciblée mutée: {n['pk']}.{k}"

# ---------- écriture ----------
json.dump(planted, open(os.path.join(BASE, "planted-clusters.json"), "w", encoding="utf-8"),
          ensure_ascii=False, indent=1)
json.dump({"seed": SEED, "sha_corpus_figé": CL["sha"], "total": len(defects),
           "défauts": defects},
          open(os.path.join(BASE, "planted-defects.json"), "w", encoding="utf-8"),
          ensure_ascii=False, indent=1)
print(f"OK — {len(defects)} défauts plantés, graine {SEED}")
from collections import Counter
print("par classe:", dict(sorted(Counter(d['classe'] for d in defects).items())))
print("par langue:", dict(sorted(Counter(d['langue'] for d in defects).items(), key=lambda x: LANGS.index(x[0]))))
print("par grappe:", dict(sorted(Counter(d['path'].split('.')[0] + '.' + d['path'].split('.')[1] if d['path'].count('.') >= 1 else d['path'] for d in defects).items())))
