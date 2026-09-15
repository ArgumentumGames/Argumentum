# #994 — Arbitrage apostrophes A/B/C/D : comptes joués sur master, feuille tranchable

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `c089d526`
**Instrument** : `tools/994-apostrophe-dryrun.py` — **sur master** depuis #1377 (`12f07409`).
**Statut** : **MESURE + FEUILLE DE DÉCISION** — lecture seule. ⛔ `0` écriture CSV/OWL, ⛔ aucune régénération, ⛔ aucune branche choisie ici.
**Dispatch** : pool #458 c.5666260217 (grain ①, actualisé 15/09) + DM `4gt4tz`.
**Amont** : `994-apostrophes-8langues-2026-09-14.md` (mesure, rév. 2) · GO voix EN écrit le 14/09 (`52ff57e4` #1367 + `29ce5d03` #1375).

---

## §0 Le résultat en une phrase

**La contrainte d'ordre du dossier §6 est satisfaite** — le GO voix EN (71 cellules) est écrit, et le
solde EN d'apostrophes **re-dérivé après lui** est **inchangé : 22** ; les quatre branches sont
mesurées à jour sur `c089d526` (**A 146 · B 164 · C 183 · D 0**) ; **l'owner peut trancher** (§3).

## §1 Ce qui a changé depuis le dossier du 14/09 (`21a72385` → `c089d526`)

| événement | commit | effet apostrophes (MESURÉ, pas recopié) |
|---|---|---|
| **GO voix EN écrit** — 71 cellules Scenarii à la 3ᵉ pers. | `52ff57e4` (#1367, delta déclaré) + `29ce5d03` (#1375) | **Solde EN re-dérivé post-GO = 22, inchangé** (Scenarii 11 + Fallacies 9 + Virtues 2). L'inquiétude du dossier §6 (« réintroduire des courbes dans le texte normalisé ») est **levée par la mesure** : la réécriture à la 3ᵉ personne n'a ni créé ni détruit de courbe. |
| Tournesol — 2 cellules (B/C du GO noms officiels) | `52ff57e4` (#1367) | Aucune apostrophe en jeu (`Cuthbert Calculus` / `профессор Лакмус`). |
| OWL régénéré | `2b572787` (#1379) | `argumentum.owl` frais (Fallacies FR = **0** droite rendue ; seules 3 `link_fr` URLs). **`argumentum_virtues.owl` inchangé** (`git diff 21a72385..c089d526 -- docs/ontology/` : seul `argumentum.owl` bouge) ⇒ la mesure du 14/09 tient par construction : **143 droites FR = 139 `AIF_criticalQuestion` + 4 `link_fr`** partent toujours en `rdfs:comment`. |

**Comptes A/B/C du dossier reproduits à l'unité près sur `c089d526`** : 146 ✔ · ΔB +18 ✔ · 183 ✔
(trois « dossier §6 RAPPORTÉ → ✔ » émis par l'instrument lui-même).

## §2 Les quatre branches, telles que l'instrument les joue sur `c089d526`

| branche | écritures | cartes pixel | PDF / 80 | OWL | régén |
|---|---|---:|---:|---|---|
| **(A) FR seule** — Scenarii+Rules+RulesPnP FR + `AIF_criticalQuestion` | **558 occurrences** droites → courbes (Scenarii 392 · Virtues AIF 139 · Rules 19 · RulesPnP 8 ; Fallacies FR et Virtues FR titre/description/remark **dérivent 0** — preuve de fermeture vivante) | **146** (Scenarii 138 · Rules 4 · RulesPnP 4) | **6 = 7,5 %** (Poker ×3 + Tarot ×3) | **1** — les 102 cellules AIF deviennent courbes en `rdfs:comment` | **`fr/` seule** (1/8) |
| **(B) A + inverse EN** | A + **22 courbes → droites** EN | **164** = A + **18** EN (Scenarii 11 · Fallacies 5 · Virtues 2) | **16** (A + 10 : Fallacies 6 + Virtues 1 + Poker ×3) | idem A | `fr/` + `en/` |
| **(C) 8 langues** | B + **29 droites → courbes** pt/ru/es/zh/ar | **183** (fr 146 · en 18 · pt 8 · zh 6 · ru 3 · es 1 · ar 1) | **52 = 65 %** | idem A | complète |
| **(D) ne rien faire** | 0 | 0 | 0 | 0 | aucune |

⚠️ L'instrument **marque** chaque rangée pt/ru/es de (C) « changement de convention de langue » et
chaque rangée zh/ar « classe guillemet, pas élision » — le dossier §6 C : (C) **ne se justifie pas
comme un nettoyage** (`d'água` est un portugais correct ; `'自由'` est un guillemet).

## §3 Feuille de décision (un écran — l'owner coche)

| | |
|---|---|
| **La question** | La convention **FR = `’` / EN = `'`** étant acquise (#1073), **jusqu'où l'appliquer** aux quatre surfaces jamais couvertes : Scenarii FR (392 occ., 138/167 cartes), Rules FR (19), RulesPnP FR (8), `AIF_criticalQuestion` (139 occ./102 cellules → OWL) ? |
| **(A) FR seule** *(recommandation du dossier §6, reportée)* | Corrige le seul défaut **visible sur une carte** + l'OWL FR. Même langue, même règle, **aucune convention étrangère touchée**, aucune mindmap invalidée (libellés de famille FR à 0). 146 cartes · 6 PDF · 1 OWL · régén `fr/` seule. |
| **(B) A + inverse EN (22 courbes)** | Achève #1030 : il avait fait 87 cellules sur les 6 colonnes EN des deux taxonomies, il en reste **9** là-bas, **plus 11 Scenarii jamais dans son périmètre**. Le lot le moins cher par unité (+18 cartes pour +10 PDF). |
| **(C) 8 langues** | ⚠️ **Changement éditorial, pas un nettoyage** : pt/ru/es = conventions de langue légitimes (`d'água`, `д'Арк`), zh/ar = **guillemets** (autre classe de caractère, autre règle). 52 PDF/80. |
| **(D) Statu quo** | 0 écriture. Le défaut visible reste : **138 des 167 cartes FR de Scenarii** portent au moins une droite (392 occ., ≈ 2,8/carte touchée) ; l'OWL FR reste mixte (143 droites / 184 courbes). |
| **Ordre d'exécution** | **Sans objet désormais** côté voix EN : le GO (71 cellules) est écrit et le solde re-dérivé après lui est **stable à 22** (§1). L'écriture apostrophes peut suivre **immédiatement** le choix. |
| **Mécanique post-choix** | Worklist dérivable à la cellule (`--worklist A\|B\|C`) · application par splice byte-exact (BOM/CRLF préservés par champ, house pattern) · régén du périmètre + passe `--pdf-cmyk` · manifeste + **verdict visuel ai-01** (le worker signale, il ne déclare pas). |
| **Invariants toutes branches** | `link_*` **exempt** (URLs — normaliser casse le lien) · colonnes sans lecteur **exclues** par construction (l'instrument ne balaye que le rendu/émis) · guillemets simples zh/ar **hors périmètre**. |

## §4 Annexe — le lot EN de (B), nominatif (18 rangées, 22 occurrences)

| corpus | rangée/PK | colonne | occurrences |
|---|---|---|---|
| Scenarii | `1,0302` `1,0305` `2,0101` `3,0208` `3,0209` `3,0301` | `context` | 6 |
| Scenarii | `6,0103` | `issue` | 1 |
| Scenarii | `5,0103` | `suggestion_en` | 1 |
| Scenarii | `1,0101` `1,0204` `5,0204` | `title` | 3 |
| Fallacies | PK `412` | `desc_en` | 1 |
| Fallacies | PK `254` `653` `1120` `1179` | `example_en` | 8 |
| Virtues | pk `208` | `description_en` | 1 |
| Virtues | pk `34` | `remark_en` | 1 |

## §5 Preuves du jour (base `c089d526`, 15/09)

- Sortie intégrale de l'instrument A/B/C/D conservée ; trois contrôles « dossier §6 RAPPORTÉ → ✔ reproduit » (146 / Δ+18 / 183).
- **`Cards/` byte-identique** : les 5 CSV du périmètre ont `git hash-object` disque == blob `HEAD` (Scenarii, Fallacies, Virtues, Rules, RulesPnP).
- `docs/ontology/argumentum_virtues.owl` **inchangé** depuis `21a72385` (seul `argumentum.owl` a bougé — #1379).
- L'instrument n'a **aucun mode d'écriture in-place** (émis en copies uniquement, refus de `--to` dans le corpus).

## §6 Ce que ce grain n'établit pas

⛔ Aucune cellule écrite · ⛔ aucune branche choisie · ⛔ aucune régénération lancée (fenêtre gated) ·
⛔ aucun verdict visuel (ai-01) · ⛔ le solde 22 **n'est pas un GO** — l'écriture ne démarre que sur
choix A/B/C/D explicite de jsboige (pool #458, candidats bloqués).

---

*po-2024 — pool #458, grain ① (actualisé 15/09). Le worker mesure et signale ; la décision est à l'owner.*
