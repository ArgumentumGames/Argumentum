# `docs/release/` — préparer le verdict visuel et la communication aux associés

Ce dossier porte les **deux livrables de sortie du run de régénération** : le comparateur de paquets
(ce que le verdict visuel doit regarder) et le brouillon « quoi de neuf » (ce que les associés lisent).
Les **deux outils existaient déjà** au dépôt ; ce qui manquait, c'était cette documentation — l'item
demandait explicitement « **Docs plus script**, en PR ».

**État vérifié le 2026-10-08** (master `b7e89c32`) : les deux outils passent leur `--self-test`
(`SELF-TEST PASS`, sortie 0). Rien à construire — à **faire tourner au bon moment**.
⚠️ Pour `what-changed-associates.py`, ce self-test **ne couvre pas le chemin de production** —
voir le §5.

---

## 1. La chaîne, dans l'ordre

```text
PDF de l'ancien paquet ─┐
                        ├─► tools/pdf-page-signature-batch.py  ─► ancien.json ─┐
PDF du nouveau paquet  ─┘                                                      ├─► tools/bundle-compare.py ─► rapport.md
                        └─► tools/pdf-page-signature-batch.py  ─► nouveau.json ┘        (où regarder, page par page)

CSV au commit relu ─┐
                    ├─► tools/what-changed-associates.py ─► quoi-de-neuf-<date>.md
CSV à la tête     ─┘        (diff git, par pk, colonne → langue)
```

Les deux branches sont **indépendantes** : le comparateur regarde des **PDF**, le « quoi de neuf » regarde
des **CSV**. Ils répondent à deux questions différentes et ne se remplacent pas.

## 2. `tools/bundle-compare.py` — comparateur de paquets (grain 2)

**Ce qu'il rend** — un rapport lisible, dans cet ordre :

1. **Complétude** : les mêmes 80 fichiers (8 langues × 10 documents), rien en plus, rien en moins.
2. **Pagination** : même nombre de pages par document. Les corrections de la campagne sont
   **textuelles** ⇒ **toute dérive de pagination est rouge**.
3. **Périmètre** : par document, les pages dont la **signature** diffère (contenu des images
   embarquées + géométrie), avec les **zones de deck** pour le Tarot (p. 1-15 Rules, 16-29 Memo,
   30-379 Fallacies).
4. **Témoin inverse** : `--expect-deck lang=préfixe` — un deck **attendu touché** dont **0 page bouge**
   est une **défaillance silencieuse** (le run a « réussi » en réutilisant l'existant). Chaque entrée
   attendue doit avoir ≥ 1 page différente.

**Il ne re-signe rien.** Il diff deux manifestes. C'est ce qui lui permet de tourner en secondes là où
re-signer 80 PDF prendrait une heure — et c'est ce qui rend le verdict d'ai-01 tenable en un tick.

**Usage**

```bash
python tools/bundle-compare.py ANCIEN.json NOUVEAU.json --out rapport.md [--json diff.json] \
    [--expect-deck fr=TarotCards] [--expect-deck es=TarotCards ...] \
    [--baseline-release "v2.0.0-review"] [--max-listed 80]
```

**Codes de sortie** : `0` = rapport écrit, complétude **et** pagination tenues — *des pages différentes
ne sont pas un échec, c'est le produit* ; `1` = anomalie structurelle (fichier manquant/surnuméraire,
pagination qui dérive, ou **témoin inverse mort**) ; `2` = erreur.

**Contrôles, et ce qu'ils valent**

- `--self-test` : 6 contrôles sur manifestes de fixture, **sans PDF** — témoin (0 page), mutation
  (le fichier/page muté est nommé), complétude (fichier retiré), pagination (dérive), témoin inverse
  (doit rendre ROUGE), bornes des zones Tarot. **PASS le 08/10.**
- **Étalonnage réel (05/10)** : baseline v2.0.0 **contre elle-même** → **80/80 identiques, 0 page
  différente, sortie 0**. C'est le contrôle positif sur données réelles — le seul qui prouve que
  « 0 page » veut dire « rien n'a bougé » et non « l'instrument est aveugle ».
  ⚠️ **Consigné dans l'en-tête de l'outil, non rejoué le 08/10** : le rejouer demande les **deux
  manifestes** du paquet, que je n'ai pas sur cette machine. Ce que j'ai rejoué, c'est le
  `--self-test` (§ ci-dessus) ; je ne présente pas l'étalonnage du 05/10 comme ma mesure.

## 3. `tools/what-changed-associates.py` — brouillon « quoi de neuf » (grain 3)

**Ce qu'il rend** — un Markdown **par langue** (une section par associé), deck par deck, avec pour
chaque carte touchée : **pk + ancre FR + champs modifiés**, plus une **table de réconciliation census**
en tête.

**Pourquoi il re-mesure à chaque appel** : le census dérive de ~40 cellules/jour tant que les pools
vivent. Un document à chiffres gelés serait **périmé avant d'être lu** ⇒ l'outil **diff des CSV git**,
il ne lit aucun chiffre écrit.

**Colonnes → langue** (mesuré sur les en-têtes master) : `Fallacies` et `Virtues` par suffixe
(`text_fr`, `description_pt`…), sans suffixe = structurel ; `Rules` : `Text` = fr, `Text_xx` = langue ;
`Scenarii` : colonnes FR nommées en français = fr, colonnes anglaises sans suffixe = en.

**Usage**

```bash
python tools/what-changed-associates.py BASE_REF HEAD_REF [--out f.md]
python tools/what-changed-associates.py --self-test
```

**Codes** : `0` = rapport écrit — **un périmètre vide est un résultat, pas une erreur** ; `1` =
self-test rouge ; `2` = erreur (réf git inconnue, CSV illisible). **Self-test PASS le 08/10**
(rejoué à `b7e89c32`). ⚠️ **Il ne valide pas `diff_deck`** : il en recopie la logique, et cette copie
a divergé — détail et citations au §5.

## 4. Ce qui tourne au lancement, et par qui

| Quand | Quoi | Qui |
|---|---|---|
| avant le run | re-mesurer le census et re-générer `quoi-de-neuf-<date>.md` à la tête | po-2023 |
| après le run | `pdf-page-signature-batch.py` sur l'ancien **et** le nouveau paquet → 2 manifestes | po-2023 |
| après le run | `bundle-compare.py` avec un `--expect-deck` par deck touché | po-2023 |
| au tick suivant | ouvrir **les pages listées**, pas les 80 PDF | **ai-01** |

⛔ **Le verdict visuel reste ai-01.** Ces outils **délimitent** ce qu'il faut regarder ; ils ne
remplacent pas le regard, et aucun des deux ne rend un verdict de justesse — ils détectent un
**changement**, jamais une erreur.

## 5. Réconciliation census — **close le 08/10** (grain S1)

Le status du dashboard déclarait un census de **273 cellules** (F:123 · V:42 · S:78 · R:30) et
s'auto-déclarait « à re-mesurer » ; le relevé de ce dossier en donnait **418** à `edb39554`. La
version précédente de ce paragraphe les présentait comme **deux instruments en désaccord** et se
refusait à trancher. **C'était une erreur de lecture** : les deux comptent le **même objet à deux
dates** — même unité, même base (`89f78bcd`), mêmes discriminants.

- Le **273** est le census manuel n°7 du **soir du 05/10** : `F123/V42/S78` sont exacts dans la
  fenêtre `8103baa6` (05/10 12:49) → `2969f4b3` (06/10 01:08), et son `R:30` = **19 cellules
  mesurées + les « 11 » de #1778 en vol**, dont **2 cellules `Rules_13` (`Text_ar`, `Text_es`)
  comptées deux fois** (changées par #1768, rechangées par #1778). `R:30` n'existe à aucune tête
  (plafond outil : 28) ⇒ valeur exacte à cet instant : **271**.
- Le **418** est le même diff re-mesuré à `edb39554`.
- Rejoué pour ce correctif à `b7e89c32` : **429 cellules · 196 cartes** — **+11, toutes sur les
  Vertus**, soit **exactement** les 11 cellules du lot de merges ai-01 de 12:17 (#1807 = 3,
  #1808 = 2, #1814 = 6 ; ar +1 · es +3 · pt +4 · ru +3). **Contrôle inverse satisfait : la dérive
  du census égale le diff mergé, au chiffre près.**

⇒ **Aucun des trois n'est « le » census : c'est un instantané vivant** (~40 cellules/jour tant que
les pools vivent). Au lancement du run, re-mesurer à la tête —
`python tools/what-changed-associates.py 89f78bcd <tête-du-run>` — et **couvrir les pages du listing
rendu à cette tête**, jamais un total. Relevés et détail : en-tête de
[`quoi-de-neuf-associes-2026-10-08.md`](quoi-de-neuf-associes-2026-10-08.md).

### ⚠️ Un point d'instrument reste ouvert — il ne change pas les chiffres ci-dessus

Le `--self-test` de `what-changed-associates.py` **n'exécute pas** `diff_deck` : il en recopie la
logique (l.202, « même logique que `diff_deck`, sans git ») — or cette copie a **dérivé sur deux
discriminants** :

| | `diff_deck` (production) | self-test (copie) |
|---|---|---|
| « carte » | `is_card(h) or is_card(b)` — **l.134** | tête seule — **l.208** |
| comptage des cellules | **sous** la garde `if card:` — **l.143** | **hors** de la garde — **l.215** |

Conséquence : son assertion **l.223** (« rangée sans carte comptée en cellules mais pas en cartes »)
**passe grâce à** la divergence au lieu de la détecter ⇒ le self-test **valide une copie, pas le
chemin de production**. Aucun effet sur les chiffres de ce dossier, tous mesurés par `diff_deck`
(un seul chemin) ; mais un self-test vert ne prouve ici **rien** sur la production.
**À trancher par le propriétaire de l'outil** : soit la copie se recale sur `diff_deck` (et
l'assertion s'inverse), soit c'est `diff_deck` qui doit changer — le commentaire **l.141-142**
tranche aujourd'hui en faveur de « les cellules se comptent sur les rangées cartes **uniquement** ».

---

*po-2023 (worker lane, `myia-po-2023`). Outils des grains 2 et 3 du pool c.5993735448 ; cette
documentation est le volet manquant, écrite le 2026-10-08.*
