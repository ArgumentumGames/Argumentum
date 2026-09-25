# #994 B + C + balayage de fermeture — norme typographique par langue

**Date :** 2026-09-25 · **Lane :** po-2024 (worker) · **Dispatch :** [#994 c.5837029653](https://github.com/ArgumentumGames/Argumentum/issues/994#issuecomment-5837029653)
**Base :** `9e79e88d` · **Portée :** 5 CSV, colonnes **rendues** × 8 langues · **⛔ Aucune régénération.**

## 0. Résultat

| | Mesure |
|---|---|
| Lignes touchées | **119** |
| Cellules touchées | **314** |
| Marques transformées | **737** |
| Marques hors norme **après** (colonnes rendues, 8 langues) | **0** |
| Lignes / champs / BOM / terminaisons | **inchangés** (contrôle `csv` stdlib, 5/5 fichiers) |

| Fichier | Lignes | Cellules | sha256 avant → après |
|---|---:|---:|---|
| `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` | 1409 | 271 | `5ad17709314d8cb4` → `02ed2e5b7dac5891` |
| `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` | 224 | 17 | `b1df7eebfe069be3` → `061540ea18a95fe9` |
| `Cards/Scenarii/Argumentum Scenarii - Cards.csv` | 168 | 20 | `b82f681d7be6359a` → `cb708f5464dd8eee` |
| `Cards/Rules/Argumentum Rules - Cards.csv` | 16 | 6 | `51fddaf0bb7f4f6d` → `252aef6e9bf4af03` |
| `Cards/Rules/Argumentum Rules - Cards Print and Play.csv` | 7 | **0** | `9de09ac8f5073f2a` → **identique** |

⚠️ Le fichier P&P est un **témoin naturel** : il a été lu, rien n'y était hors norme, son sha256 est inchangé. Un instrument qui aurait écrit partout l'aurait muté.

## 1. La norme, mesurée (`9e79e88d`, colonnes rendues)

| Langue | Forme dominante | Espacement intérieur |
|---|---|---|
| fr | `« … »` | **U+0020 des deux côtés, 119/119** |
| es, ru, ar, fa | `« … »` | collé, 84 / 98 / 89 / 99 sur 99+ |
| en, pt, zh | `“ … ”` | collé, 73 / 29 / 133 |

Deux enseignements qui ont chacun changé le code :

1. **Le fr est la SEULE langue à espacer l'intérieur de la paire** — et il le fait sans exception (119 paires, U+0020 ordinaire, pas d'espace fine). Produire `«mot»` en fr aurait introduit 12 marques contredisant les 119 existantes, en croyant « corriger ».
2. **L'anglais garde l'apostrophe droite** (#1073/#1030) : la cible B n'est pas « tout courber », c'est « rendre droites les courbes ». Le canal `'` **n'est pas joué** en EN — sinon les 1515 apostrophes droites du corpus (dont les possessifs `sailors' code`) entraient dans l'appariement.

## 2. Règles appliquées, par langue

| Traitement | Langues |
|---|---|
| `’` courbe → `'` droite (toute position) | en |
| éllision/apocope intra-mot `'` → `’` | fr, ru, pt, es, fa |
| paire `'…'` et paire `"…"` → forme dominante | les 8 |
| **alternance pure** (1ʳᵉ = ouvrante, 2ᵉ = fermante…) | zh, ar |

zh et ar sont en alternance **parce que leurs marques sont collées au texte des deux côtés** : la règle d'accolement ne les sépare pas. Mesure de contrôle : **16 `'` en zh et 72 en ar, 100 % en paires** — exactement les chiffres du dispatch, reproduits ici indépendamment.

⛔ Aucune langue ne produit `’…’` (deux fermantes) : ouvrantes et fermantes sont deux caractères distincts dès l'écriture.

## 3. Écarts au cadrage — déclarés, chiffrés

### 3.1 B : **25**, pas 22 — la classe était comptée à l'intra-mot près

| | Scenarii | Fallacies | Virtues | total |
|---|---:|---:|---:|---:|
| intra-mot (le compte du dispatch) | 11 | 9 | 2 | **22** |
| **non intra-mot, mesurées ici** | +1 | +1 | +1 | **+3** |
| **total traité** | 12 | 10 | 3 | **25** |

Les trois cellules que l'instrument du cadrage ne pouvait pas voir, nommées :

| Cellule | Marque | Forme |
|---|---|---|
| Scenarii `1,0301` `context` | finale | `workers’ conditions` |
| Fallacies `254` `example_en` | initiale | `’s no need t’make` |
| Virtues `196` `remark_en` | finale | `others’ arguments` |

Le prédicat du cadrage exige une lettre **des deux côtés** ; une apostrophe finale ou initiale n'est intra-mot dans aucune des deux. **Un par corpus** — signature d'un instrument, pas d'un contenu.

### 3.2 C : **29** était un sous-compte d'un facteur 16

Le compte C du cadrage est reproduit à l'identique par l'instrument pré-existant (`994-apostrophe-dryrun.py`, branche C, non-EN) : **29** — pt 11 · ru 4 · zh 10 · es 3 · **ar 1**.

Mesure brute des apostrophes droites réellement présentes dans ces mêmes colonnes : **487**. L'écart est presque entièrement l'arabe : **1 vue / 72 réelles** — les marques arabes sont collées à des espaces ou à la virgule arabe, jamais lettre'lettre, donc le prédicat intra-mot les rate.

⇒ Le balayage de fermeture n'était pas un supplément : c'était **la seule mesure qui atteint la classe**.

### 3.3 fr : 12 marques que ni A, ni B, ni C n'avaient couvertes

Le canal `"` (guillemet droit) **n'avait jamais été balayé** par les volets A/B/C, qui ne traitaient que les apostrophes. Il restait 6 paires droites en fr (3 cellules, ex. `"comme tout le monde"`), désormais `« comme tout le monde »`. Idem pour les 224 marques `"` des 8 langues.

### 3.4 Une paire **endommagée** réparée — 1 cellule, contenu touché

| Cellule | Avant | Après |
|---|---|---|
| Fallacies `251` `example_pt` | `Não, Nanine não honra nada' — Voltaire, Nanine, III, 8` | `“Não, Nanine não honra nada” — Voltaire, Nanine, III, 8` |

Les **7 autres langues entourent la citation** de guillemets (fr `«…»`, en `“…”`, ar `«…»`, zh `“…”`, fa `«…»`) ; le pt avait perdu son **ouvrante** et gardait un fermant droit. La règle de la maison étant « une paire reste une paire », la paire est restaurée — et l'événement est **nommé** dans la sortie de l'instrument, jamais silencieux. Conditions volontairement étroites : fermant + attribution `— ` + aucune ouvrante en attente dans le segment + précédé d'une lettre. **Une seule cellule du corpus les remplit.** ⛔ C'est le seul contenu ajouté par ce grain : un veto dessus est trivial (retirer 2 caractères).

## 4. Faits d'apostrophe laissés tels quels, nommés

4 marques de Fallacies `254` `example_es` (`nosotro'`, `remilgo'`, `estamo'`, `to'`) sont des **apocopes de registre** — traitées comme éllisions (`’`), pas comme des guillemets. Les 1515 apostrophes droites de l'anglais sont la **norme** de l'anglais et sortent intactes.

## 5. Preuves

| Garde | Ce qu'elle établit | Résultat |
|---|---|---|
| (1) portée | 0 cellule `link_*`, 0 colonne sans lecteur | ✔ |
| (2) langue | règle fixe par colonne, table citée §2 | ✔ |
| (3) structure | lignes, BOM, **terminaison de chaque ligne** | 168/1409/224/16/7 inchangés |
| (4) re-parse | diff de valeurs == worklist dérivée | 314 cellules, exact |
| (5) post-état | 0 hors norme, **avant** écriture (fail-closed) | 0 → écriture autorisée |
| (6) mutation | paire cassée → garde rouge ; paire intacte → pas de faux rouge | ru/fr/en, les deux sens |

Contrôles **indépendants** (aucune ligne du writeur réutilisée) :

| Instrument | Avant | Après |
|---|---|---|
| `994-apostrophe-dryrun.py` (tiers, celui du cadrage), branche C | **51** | **0** |
| ↳ dont B (EN courbe) / C (non-EN) | 22 / 29 | 0 / 0 |
| comptage brut par canal (`994-verifie-BC-independant.py`) | P `"` **224** · A `'` hors EN **487** · C `’` EN **25** | **0 · 0 · 0** |
| `csv` stdlib (structure + cellules) | — | 5/5 fichiers, **314** cellules |

Et le contrôle qui compte : **les trois canaux étaient non nuls avant**. Un `0` n'est une absence que si l'instrument pouvait voir un `1` — ici il voyait 224/487/25.

Reconciliation **plan ↔ réel, cellule par cellule** (`--worklist`) : **0 écart** sur les 8 langues.

## 6. Échantillons (un par langue, tirés des backups)

| langue | cellule | avant | après |
|---|---|---|---|
| `fr` | Fallacies 47 `desc_fr` | `"comme tout le monde"` | `« comme tout le monde »` |
| `en` | Fallacies 1179 `example_en` | `didn’t steal it` | `didn't steal it` |
| `ru` | Fallacies 151 `example_ru` | `Я 'атлет' в этимологическом смысле` | `Я «атлет» в этимологическом смысле` |
| `pt` | Fallacies 251 `example_pt` | `não honra nada' — Voltaire` | `“Não, Nanine não honra nada” — Voltaire` |
| `es` | Fallacies 254 `example_es` | `Entre nosotro', no hace falta` | `Entre nosotro’, no hace falta` |
| `zh` | Fallacies 1236 `example_zh` | `例如'清晰的概念清晰地表达出来。'` | `例如“清晰的概念清晰地表达出来。”` |
| `ar` | Fallacies 1102 `desc_ar` | `مثل "يَجِب" أو "لا يَجِب"` | `مثل «يَجِب» أو «لا يَجِب»` |
| `fa` | Fallacies 1102 `desc_fa` | `مانند "باید" یا "نباید"` | `مانند «باید» یا «نباید»` |

## 7. Limites, nommées

- **`--mutation-test` a dû être réécrit sur littéraux.** Sa première version cherchait une paire à casser **dans l'arbre** : après `--apply`, l'arbre n'en contient plus, et le contrôle s'est déclaré « aveugle » **à tort** (rc=1 sur un instrument correct). Un contrôle qui dépend de l'état qu'il valide ne se rejoue pas. Il ne lit plus le corpus.
- Le writeur n'est **pas** idempotent au sens « rejouable sur un arbre écrit » : une seconde passe ne trouve plus rien (worklist vide) — c'est le résultat attendu, pas un bug.
- La réparation §3.4 dépend d'un motif (`— `) : sur un corpus où une citation serait attribuée autrement, elle ne se déclenche pas et la marque resterait **nommée non appariée** — jamais convertie à l'aveugle.
- Les backups `.BEFORE-994BC` (5 fichiers) restent **hors commit** ; la préservation qui fait foi est l'historique (`9e79e88d`).
- **⛔ Aucune régénération n'a été lancée** : le rendu de ces corrections ouvre la prochaine fenêtre ×8, après la livraison de bundle en cours.

## 8. Exécutable

```bash
python tools/994-apostrophe-dryrun.py                      # instrument pré-existant (branche C)
python tools/994-write-BC-sweep-byte-exact.py              # plan + garde (5) pre-ecriture, 0 ecriture
python tools/994-write-BC-sweep-byte-exact.py --apply      # ecrit + backups
python tools/994-write-BC-sweep-byte-exact.py --mutation-test
python tools/994-write-BC-sweep-byte-exact.py --self-test
python tools/994-verifie-BC-independant.py --avant         # non-aveuglement (backups, doit etre non nul)
python tools/994-verifie-BC-independant.py --worklist <json>
```
