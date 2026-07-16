# Byte-check — encoding inventory across all production Cards CSVs (read-only, 0 write)

**Date** : 2026-07-16
**Auteur** : po-2024 (tick 24 idle secours `07kpoq`, corrigé + étendu tick 28 sur demande jsboige)
**Posture** : read-only rapport. Aucun write prod. Aucun micro-fix CSV.

## Historique

- **tick 24** : sweep initial sur Rules + Rules PnP + Scenarii (3 datasets).
- **tick 28** : correction d'un bug de l'outil (voir ci-dessous) + extension aux **2 taxonomies de production** (Fallacies + Virtues) qui n'avaient jamais été scannées sur la dimension encoding — or ce sont les fichiers qui rendent l'essentiel des cartes (1408 + 223 lignes).

### Correction du bug tick 24 (EOL faussement rapporté « LF »)

Le tick 24 déclarait Rules/Rules PnP/Scenarii en **LF**. **C'était faux.** L'ancien `detect_encoding()` ne lisait que les **64 premiers octets** (`f.read(64)`) et testait `b"\r\n" in raw`. Or le premier saut de ligne de chaque CSV de production tombe **au-delà de l'octet 64** (première `\r\n` aux octets 82 → 1235 selon le fichier, le header étant très large). Résultat : aucun `\r\n` dans les 64 premiers octets → faux « LF ». Corrigé tick 28 : lecture du fichier entier, avec distinction **terminateur de ligne** (CRLF) vs **LF intra-cellule** (retours à la ligne à l'intérieur des cellules Markdown multi-lignes de Rules/Fallacies — normal, PapaParse/CsvHelper-compatible).

## Scope

| Dataset | Path | Taille |
|---|---|---|
| Fallacies Taxonomy | `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` | 1408 × 104 · 3.9M |
| Virtues Taxonomy | `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` | 223 × 81 · 979K |
| Rules | `Cards/Rules/Argumentum Rules - Cards.csv` | 15 × 10 · 152K |
| Rules PnP | `Cards/Rules/Argumentum Rules - Cards Print and Play.csv` | 6 × 10 · 49K |
| Scenarii | `Cards/Scenarii/Argumentum Scenarii - Cards.csv` | 167 × 70 · 529K |

## Résultats

### Encoding (corrigé + complété)

| Dataset | BOM | EOL (terminateur) | Note |
|---|---|---|---|
| Fallacies Taxonomy | **BOM** | CRLF | + LF intra-cellule (Markdown multi-lignes) |
| Virtues Taxonomy | **BOM** | CRLF | — |
| Rules | **BOM** | CRLF | + LF intra-cellule (contenu Markdown dense) |
| Rules PnP | **BOM** | CRLF | + LF intra-cellule |
| Scenarii | **no-BOM** | CRLF | **seul fichier sans BOM** |

**Le cadrage tick 24 était inversé.** Tick 24 ne voyait que Rules (BOM) parmi Rules/Scenarii et suggérait implicitement « Rules est l'anomalie ». En réalité, une fois les 2 taxonomies incluses : **le BOM est la convention majoritaire (4/5)** et c'est **Scenarii** le fichier atypique (no-BOM). Le terminateur de ligne est **CRLF pour les 5** (tick 24 avait faux sur ce point).

Le fichier de backup `Cards/Rules/Argumentum Rules - Cards.old.csv` (6 × 3, no-BOM/LF) n'est pas un render CSV — hors scope.

### Couverture par langue (cellules populées / vides)

| Dataset | \_en | \_ru | \_pt | \_es | \_ar | \_fa | \_zh |
|---|---|---|---|---|---|---|---|
| Fallacies (6-7 cols/lang) | 5646/2802 | 8523/1333 | 8506/1350 | 8509/1347 | 8506/1350 | 8495/1361 | 8492/1364 |
| Virtues (7 cols/lang) | 1495/66 | 1497/64 | 1487/74 | 1454/107 | 1393/168 | 1401/160 | 1406/155 |
| Rules (1 col/lang) | 15/15 | 15/15 | 15/15 | 15/15 | 15/15 | 15/15 | 15/15 |
| Rules PnP (1 col/lang) | 6/6 | 6/6 | 6/6 | 6/6 | 6/6 | 6/6 | 6/6 |
| Scenarii (1-8 cols/lang) | 167/167 | 1336/1336 | 1336/1336 | 1336/1336 | 1336/1336 | 1336/1336 | 1336/1336 |

**Note sur les « empty » Fallacies/Virtues** : les colonnes `_xx` scannées incluent des champs **optionnels** (exemples secondaires, notes, `link_*`). Le taux de vide reflète cette sparsité de champs optionnels, **pas** un déficit de traduction : les champs de texte cœur (`text`, `desc`, `example`) sont déjà vérifiés 100% couverts par les audits #192/#795. Le présent sweep ne rouvre pas ce constat — il confirme seulement l'absence de fallback FR silencieux (voir script coverage).

### Couverture de script (les traductions sont **effectives**, pas des fallbacks FR)

| Dataset | \_en (latin) | \_ru (cyrillic) | \_pt (latin) | \_es (latin) | \_ar (arabic) | \_fa (arabic) | \_zh (cjk) |
|---|---:|---:|---:|---:|---:|---:|---:|
| Fallacies | 83% | 84% | 81% | 83% | 82% | 79% | 89% |
| Virtues | 85% | 82% | 81% | 83% | 80% | 77% | 80% |
| Rules | 77% | 79% | 76% | 76% | 77% | 75% | 75% |
| Rules PnP | 77% | 80% | 76% | 77% | 79% | 75% | 80% |
| Scenarii | 79% | 86% | 81% | 82% | 84% | 80% | 92% |

Script attendu dominant à 75-92% dans chaque colonne (le reste = ponctuation/chiffres/espaces = `other`). **Aucun fallback FR silencieux** : pas de colonne cyrillique/arabe/CJK dominée par du latin.

### Contamination FR (marqueurs à/é/è/ç dans colonnes cyrillique/arabe/CJK)

| Dataset | Lang | Cas | Colonne | Verdict |
|---|---|---:|---|---|
| Fallacies | toutes | **0** | — | ✅ **1408 lignes cleanes** |
| Virtues | \_ru | 1 | **`link_ru`** | Gap link-URL #192 (URL, pas texte MT) |
| Virtues | \_fa | 1 | **`link_fa`** | URL FR `transparency.org/fr/...` — gap link-URL #192 |
| Scenarii | \_ru | 1 | `context_ru` | Faux positif — `trompe-l'œil` (emprunt lexical FR, terme de peinture) |
| Scenarii | \_ar | 1 | `suggestion_ar` | Faux positif — `Caméléa` (nom propre) |
| Scenarii | \_fa | 3 | `suggestion_fa`/`smoothTalker_fa` | Faux positifs — noms propres `Caméléa`, `Obélix` |

**0 contamination FR authentique dans le texte traduit.** Les 2 hits Virtues sont dans des colonnes **`link_*` (URLs)** — c'est le **gap connu de recherche humaine des liens localisés** (audit #192, mémoire `i18n-coverage-gap-is-link-urls`), pas de la MT contaminée : ce ne sont pas des champs traduisibles par gpt-5.5 mais des URLs à rechercher à la main. Les hits Scenarii sont des emprunts/noms propres, corrects en traduction.

## Read-path / write-path (impact pipeline du BOM)

Vérifié côté code C# — le BOM actuel **n'impacte pas** la lecture, mais **impacterait** une future écriture GSheet-sync :

- **Read-path SÛR** : le pipeline lit les 4 fichiers BOM sans souci (80 PDFs générés, 596 tests verts). `CsvBase.LoadFromContent` ([CsvBase.cs:57](../../Generation/Converters/Argumentum.AssetConverter/Entities/CsvBase.cs#L57)) travaille sur une string déjà décodée (BOM strippé en amont par `File.ReadAllText` par défaut) ; `UtilityExtensions.cs:268` fait un `.TrimStart('﻿')` explicite ; le diff engine a un test dédié `BOM_Prefixed_Header_Does_Not_Break_Primary_Key_Lookup`. CsvHelper + PapaParse tolèrent tous deux le BOM (`utf-8-sig`).
- **Write-path = piège de churn latent** : `GSheetSyncRunner.cs:98` écrit `File.WriteAllTextAsync(localPath, downloadedCsv)` **sans argument d'encoding** → défaut .NET = **UTF-8 no-BOM**. Donc si le GSheet-sync (#193, actuellement `Enabled=false` partout, en attente OAuth) est un jour lancé en pull sur les 4 fichiers BOM, il les **réécrirait no-BOM** → diff git plein-fichier (strip du BOM) + risque de re-normalisation d'EOL. Ce n'est pas un bug aujourd'hui (sync désactivé) mais un **point de décision** à trancher avant d'activer #193.

## Verdict

✅ **Datasets propres côté i18n.** 0 contamination FR authentique (Fallacies 1408 lignes cleanes incluses). 0 fallback FR silencieux. Scripts attendus 75-92%. Les seuls hits « FR » restants sont soit des URLs (`link_*`, gap #192 connu), soit des emprunts/noms propres.

⚠ **Décision encoding pour jsboige (decision-ready)** : le BOM est la **convention majoritaire** (4/5) et le read-path le tolère. Trois directions cohérentes avant d'activer le GSheet-sync #193 :
- **(A) Normaliser tout en no-BOM** — aligne les fichiers sur ce que le sync produira de toute façon (`GSheetSyncRunner.cs:98`), au prix d'un diff plein-fichier ponctuel sur les 4 BOM.
- **(B) Rendre le writer du sync BOM-preserving** — préserve la convention majoritaire, au prix d'une modif dans `GSheetSyncRunner`.
- **(C) Statu quo** — acceptable tant que #193 reste désactivé ; le read-path est sûr.

**Pas de write prod dans cette PR.** La décision A/B/C est gated jsboige (cohérence projet-wide × interaction #193).

## Out of scope

- ⛔ Écriture CSV (gated par #202 governance).
- ⛔ Décision BOM vs no-BOM (gated jsboige — voir 3 options ci-dessus).
- ⛔ Modif de `GSheetSyncRunner.cs` (lane po-2024 = Cards/data ; toucher le writer relève d'un mandat #193 explicite).
- ⛔ Recherche des `link_*` localisés (gap #192, tâche de recherche humaine, pas MT — post-tag).

## Re-run

```bash
python tools/bytecheck-datasets.py
```

Idempotent, 0 dépendance externe (Python stdlib only), runtime ~1.5s.

## Refs

- #202 (CSV write governance)
- #193 (GSheet ↔ CSV sync — writer no-BOM, `Enabled=false`)
- #192 (link-URL i18n gap — `i18n-coverage-gap-is-link-urls`)
- `mt-garbage-sweep-false-zero` (mémoire — le sweep 1D mot-clé est insuffisant)
- `csv-byte-exact-column-insertion` (mémoire — field-segment splitter not csv round-trip)

— po-2024 (tick 24 dispatch `07kpoq`, corrigé + étendu tick 28)
