# `docs/release/` — préparer le verdict visuel et la communication aux associés

Ce dossier porte les **deux livrables de sortie du run de régénération** : le comparateur de paquets
(ce que le verdict visuel doit regarder) et le brouillon « quoi de neuf » (ce que les associés lisent).
Les **deux outils existaient déjà** au dépôt ; ce qui manquait, c'était cette documentation — l'item
demandait explicitement « **Docs plus script**, en PR ».

**État vérifié le 2026-10-08** (master `edb39554`) : les deux outils passent leur `--self-test`
(`SELF-TEST PASS`, sortie 0). Rien à construire — à **faire tourner au bon moment**.

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
self-test rouge ; `2` = erreur (réf git inconnue, CSV illisible). **Self-test PASS le 08/10.**

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

## 5. Point ouvert — la réconciliation census

Le status du dashboard déclare un census de régénération de **273 cellules** (F:123 · V:42 · S:78 ·
R:30) et **se déclare lui-même « à re-mesurer »**. Re-mesuré le **08/10** par
`what-changed-associates.py` de `89f78bcd` à `edb39554` : **418 cellules · 195 cartes**. Les deux
instruments **ne s'accordent pas** au-delà des Sophismes (123 = 123 ✔) — V 42 vs 122, S 78 vs 145,
R 30 vs 28.

**Je ne tranche pas lequel est juste**, et aucune source au dépôt ne définit le 273. C'est exactement
ce que la table de réconciliation du § « Vue d'ensemble » sert à établir — **à faire avant que le
census ne serve à décider quelles pages le verdict doit couvrir.**

---

*po-2023 (worker lane, `myia-po-2023`). Outils des grains 2 et 3 du pool c.5993735448 ; cette
documentation est le volet manquant, écrite le 2026-10-08.*
