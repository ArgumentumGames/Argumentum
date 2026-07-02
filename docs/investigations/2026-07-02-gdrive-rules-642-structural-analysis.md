# #642 — GDrive Rules ↔ repo master : analyse structurelle + inventaire garbage MT

**Auteur** : po-2024 (deep-queue PRIMAIRE, dispatch ai-01 msg-…c60w1b)
**Date** : 2026-07-02
**Base repo** : master `99145fab` (post-#640 Rules i18n refonte)
**Source GDrive** : feuille publique `gid=0` exportée 2026-07-02 (24 rangées × 5 cols)

> Ce document remplace l'artefact « paste-ready » initialement demandé : **un CSV
> row-level paste-ready est structurellement impossible** (voir §1). La livraison est
> donc une analyse décisionnelle + un inventaire garbage actionnable, destinée à
> éclairer la décision A/B/C/D de jsboige avant toute écriture GDrive.

---

## 1. Trouvaille bloquante — les rangées GDrive traversent les frontières de records repo

La structure GDrive (24 rangées fines) et la structure repo (15 records main + 6 PP,
coarser) **ne sont pas alignées rangée-à-rangée**. Pire : les frontières de rangées
GDrive **coupent au milieu des records repo**.

### Preuve (V1, « École des menteurs », base 4-8 joueurs)

| Repo record | Contenu FR (frontière) |
|---|---|
| `Rules_04` (fin) | `…Le baratineur peut écourter la saynète quand il le souhaite.` |
| `Rules_05` (début) | `### 4. Le jury — Tous les joueurs, sauf le baratineur, constituent le jury…` |

| GDrive rangée | Contenu FR |
|---|---|
| `row 3` (fin) | `…le type d'argument indiqué par la carte qu'il a choisie.` *(= Rules_04 step 3, incomplet)* |
| `row 4` (début) | `Le baratineur expose ses arguments et le piocheur lui donne la réplique…` *(= DERNIER paragraphe de Rules_04)* puis `### 4. Le jury…` *(= Rules_05)* |

→ **GDrive row 4 chevauche `Rules_04` (fin) ET `Rules_05` (début).** Le découpage
GDrive est indépendant du découpage repo. `row 5` « En cas d'égalité (🥈👇👇≟🥈👇👇) »
confirme : ce paragraphe vit dans **`Rules_05`** (main), pas dans `RulesPP_06`
(« Il garde alors sa carte… ») — le mapping naïf V1→PP était donc faux aussi.

### Conséquence

Un artefact « paste-ready » construit en remplaçant chaque cellule GDrive par le
contenu d'un record repo serait **incorrect** : il (a) dupliquerait le contenu du
record dans la mauvaise rangée, (b) perdrait les paragraphes coupés, (c) misalignerait
les langues (FR d'une rangée ↔ EN/RU/PT d'un autre record). **Aucun mapping
rangée→record n'existe, pour V1 comme pour V2-V5.**

Le contenu « propre » que jsboige attend vit dans le repo, mais **dans un découpage
différent**. Mettre le GDrive à jour nécessite donc soit de changer la structure du
GDrive, soit du fixing paragraphe-par-paragraphe (chirurgical).

---

## 2. Carte structurelle — 24 rangées GDrive → records repo (avec chevauchements)

Légende : `→` = correspondance propre · `↔` = chevauchement (frontière coupée) ·
`∅` = pas d'équivalent direct repo (paragraphe interne).

| GDrive | Variante | Record repo lié | Note granularité |
|---|---|---|---|
| row 0 | V1 École des menteurs (4-8j) | `Rules_01` / `RulesPP_01` | cover `# Argumentum` (+ sous-titre GDrive « ## L'école des menteurs » absent du repo) |
| row 1 | V1 | `Rules_02` | Règles + Matériel + Résumé |
| row 2 | V1 | `Rules_03` | Installation |
| row 3 | V1 | `Rules_04` ↔ | Déroulé steps 1-3 (manque le dernier paragraphe, parti en row 4) |
| row 4 | V1 | `Rules_04`(fin) ↔ `Rules_05` | « Le baratineur expose… » + « ### 4. Le jury » + « ### 5. Le décompte » — **chevauche 2 records** |
| row 5 | V1 | `Rules_05` ∅ | « En cas d'égalité (🥈👇👇)… » — paragraphe interne à Rules_05 |
| row 6 | V2 Bingo (1-20j) | `Rules_07` | cover + nom variante |
| row 7 | V2 | `Rules_07` ∅ | « *Règles du jeu : 1-20* » + Matériel + Résumé (fragment = mi-fin de Rules_07) |
| row 8 | V2 | `Rules_08` | Pendant le débat |
| row 9 | V3 Dernier beau parleur (1-8j) | `Rules_09` | cover + nom variante |
| row 10 | V3 | `Rules_09` ∅ | Règles + Matériel (fragment de Rules_09) |
| row 11 | V3 | `Rules_09`/`10` ↔ | Installation (header partagé) |
| row 12 | V3 | `Rules_10` | Déroulé de la manche |
| row 13 | V3 | `Rules_10` ∅ | Fin de partie et décompte (fragment) |
| row 14 | V4 Moulin à baratin (2-8j) | `Rules_11` | cover + nom variante |
| row 15 | V4 | `Rules_11` ∅ | Règles + Matériel (fragment) |
| row 16 | V4 | `Rules_11`/`12` ↔ | Installation |
| row 17 | V4 | `Rules_12` | Déroulé de la manche |
| row 18 | V4 | `Rules_12` ∅ | Fin de partie (fragment) |
| row 19 | V5 Parlote coinchée (4j) | `Rules_13` | cover + nom variante |
| row 20 | V5 | `Rules_13` ∅ | Règles + Matériel (fragment) |
| row 21 | V5 | `Rules_13`/`14` ↔ | Installation (variante-spécifique : « On sélectionne 28… ») |
| row 22 | V5 | `Rules_14` | Début de la manche |
| row 23 | V5 | `Rules_15` ∅ | Décompte (fragment) |

**17 rangées sur 24 (∅ ou ↔) n'ont pas de correspondance rangée-à-rangée propre.**

---

## 3. Inventaire garbage MT résiduel dans le GDrive (à écraser)

27 cellules garbage confirmées (sondage `scan_translations.py` #640 + probes
manuels). Le propre équivalent vit dans le record repo indiqué (colonne
`Text_en`/`Text_ru`/`Text_pt` post-#640). Glossaire verrouillé #640 :
piocheur→drawer/Ведущий/comprador, baratineur→smooth talker/Болтун/embromador,
manche→round/раунд/rodada, atout→trump/козырь/trunfo.

### Défauts MT par pattern (source propre = record repo)

| GDrive cellules | Garbage MT | Clean équivalent (repo) | Source record |
|---|---|---|---|
| row 3 `Text_pt`, row 12 `Text_en`/`pt`, row 17 `Text_en`/`pt` | `Roll of the English Channel` / `# Ход партии` (manche→English Channel) | `## Round sequence` (EN), `## Ход раунда` (RU), `## Decurso da rodada` / `## Desenvolvimento da rodada` (PT) | `Rules_04` / `Rules_10` / `Rules_12` |
| row 7, 11, 16, 21 `Text_en` | `## Facility` (Installation→Facility) | `## Setup` | `Rules_03` / `Rules_07` / `_11` / `_13` |
| row 12, 17, 22 `Text_en` | `the peacher` (piocheur→peacher) | `the drawer` / `the comprador` | `Rules_04` / `_10` / `_12` / `_14` |
| row 12, 17, 22 `Text_pt` | `o pêssego` (piocheur→pêssego/peach) | `o comprador` | idem |
| row 12, 17 `Text_pt` | `a gaveta desenha` (drawer+draw littéral) | `o comprador retira` / `puxa` | idem |
| row 16, 21 `Text_pt` | `picareta` (piocheur→picareta/pickaxe) | `o comprador` | `Rules_11` / `_13` zone |
| row 11, 13, 17, 20, 23 `Text_pt` | `manga` (manche→manga/sleeve) | `rodada` | Déroulé records |
| row 18 `Text_en`/`pt` | `baratiner` / `baratinerer` (baratineur non traduit) | `the smooth talker` (EN), `o embromador` (PT) | `Rules_11`/`_12` |
| row 4 `Text_pt` | `O baratiner exibe… o picador` | `O embromador expõe… o comprador` | `RulesPP_05` / `Rules_05` |
| row 5 `Text_pt` | `baratineur` non traduit | `embromador` | `Rules_05` |

**Toutes ces cellules ont un clean équivalent identifiable dans le repo** — le fixing
est paragraphe-level (extraire le bon paragraphe du record repo), pas row-level.

---

## 4. Décisions attendues (jsboige) — re-cadrage A/B/C/D

La trouvaille §1 re-cadre la décision **A** : ce n'est plus « mapping cell-by-cell vs
remplacement » — c'est le niveau de **restructuration** du GDrive.

### A — Niveau de restructuration GDrive (décision nouvelle, remplace l'ancien A)

- **A1 — Collapser le GDrive vers la structure repo.** Remplacer la feuille 24 rangées
  par le layout repo (15 records main, 5 variantes en colonnes ou feuilles séparées).
  Paste propre et durable, mais **perd la granularité fine GDrive** (covers par
  variante, split paragraphe). Recommandé si le GDrive n'est plus la source d'édition.
- **A2 — Garder la structure GDrive 24×5, fixing in-place paragraphe-par-paragraphe.**
  Préserve la granularité. jsboige (ou po-2024 sur GO) corrige les 27 cellules garbage
  en extrayant le paragraphe propre du record repo. Labour-intensif mais non-destructeur.
- **A3 — Re-splitter les records repo pour reconstruire les rangées GDrive.** Je tente
  le découpage par headers markdown (`##`/`###`) sur les 4 langues pour reconstruire
  un artefact 24×5 aligné. **Fragile** (les headers MT garbage sont cassés, mais je
  remplace par le propre qui a la structure) — confiance moyenne, diff obligatoire.

**Reco po-2024** : **A2** (préserve la source, fixing ciblé 27 cellules, risk minime).
A1 si jsboige veut simplifier la maintenance. A3 seulement si la granularité GDrive
doit être préservée ET qu'on veut un artefact automatisé (alors je le livre avec diff
complet pour relecture).

### B — OAuth creds vs paste manuel (inchangé)

OAuth GSheetSync toujours absent du repo (`GSheetAuthManager.cs:25-37` attend
client_id/client_secret, conforme #193). Donc soit :
- **B1** jsboige fournit creds → upload programmatique possible (mais voir A pour la
  forme de l'artefact).
- **B2** jsboige fait le paste manuel (je fournis le CSV/le worklist, jsboige importe).
  **B2 est actionnable immédiatement** dès que A est tranché.

### C — Colonnes AR/ES/ZH/FA (inchangé)

Le GDrive n'a que 4 colonnes langue (`Text/Text_en/Text_ru/Text_pt`). Les 4 cols
AR/ES/ZH/FA du repo ne sont pas synchronisables telles quelles. Décision : ajouter
ces 4 colonnes au GDrive (en-têtes + données), ou les laisser hors-scope (le repo
reste la source pour ces langues) ?

### D — Feuille PP (inchangé)

Le GSheet n'expose qu'1 feuille publique (`gid=0`, 24 rangées, main+PP concaténés).
Les `gid=1/2/3` = login wall. Le PP vit-il dans cette même feuille (concaténé, ce que
suggère V1 rows 4-5 type PP) ou dans une 2e feuille privée ? (Si A1, ce point devient
moot : on adopte le layout repo main+PP séparés.)

---

## 5. Statut & prochaines étapes

- **Aucun upload GDrive** (conforme mandate jsboige + B non tranché).
- **Aucun artefact paste-ready row-level** (structurellement impossible, §1).
- **Worklist garbage §3 actionnable** dès décision A (A2 = je fixe les 27 cellules ;
  A1 = je produis le CSV collapser ; A3 = je tente le split avec diff).
- Colonnes AR/ES/ZH/FA (C) et forme de la feuille PP (D) en attente.

**Recommandation po-2024** : jsboige tranche **A2 + B2** → je livre le fixing
paragraphe-level des 27 cellules garbage (CSV 24×5 preserving structure, diff
cell-by-cell, 0 cellule non-ciblée). C et D peuvent rester différés (non-bloquants
pour nettoyer le garbage FR/EN/RU/PT qui est le cœur du clobber #211).

Relates #193, #211, #633, #640. Code = truth (base `99145fab`, GDrive export 2026-07-02).
