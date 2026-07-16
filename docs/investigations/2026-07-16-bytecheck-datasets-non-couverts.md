# Byte-check — Rules + Scenarii (read-only audit, 0 write)

**Date** : 2026-07-16
**Auteur** : po-2024 (tick 24, dispatch ai-01 `07kpoq` idle secours)
**Posture** : read-only rapport. Aucun write prod. Aucun micro-fix.

## Scope

Datasets **non couverts** par les sweeps de qualité antérieurs (tick 24 #812/#804 étaient sur Fallacies + audit templates ; tick précédents ont couvert les Scenarii à 76/167 EN+RU+PT mais le présent sweep inclut les **8 langues** sur les cellules existantes).

- `Cards/Rules/Argumentum Rules - Cards.csv` — 15 rows × 10 cols
- `Cards/Rules/Argumentum Rules - Cards Print and Play.csv` — 6 rows × 10 cols
- `Cards/Scenarii/Argumentum Scenarii - Cards.csv` — 167 rows × 70 cols

## Tool

`tools/bytecheck-datasets.py` — script Python standalone, lit chaque CSV, calcule :

1. **Encoding header** : BOM (UTF-8 +BOM) ou no-BOM, CRLF vs LF
2. **Couverture par langue** : combien de cells populées vs vides
3. **Couverture de script** : %age de caractères dans chaque bloc Unicode (latin / latin_ext / cyrillic / arabic / cjk / other / ctrl) par colonne `_xx`
4. **Détection de contamination FR** : présence de caractères français spécifiques (à, é, è, ç, etc.) dans une colonne attendue cyrillique / arabe / CJK

## Résultats

### Encoding

| Dataset | BOM | EOL | Note |
|---|---|---|---|
| `Rules/Argumentum Rules - Cards.csv` | **BOM** | LF | ⚠ BOM sur Rules |
| `Rules/Argumentum Rules - Cards Print and Play.csv` | **BOM** | LF | ⚠ BOM sur Rules PnP |
| `Scenarii/Argumentum Scenarii - Cards.csv` | no-BOM | LF | ✅ canonique |

BOM détecté sur les 2 fichiers Rules. À vérifier si intentionnel (commodité Excel?) ou accidentel. **Pas de write dans cette PR — investigation seule.**

### Couverture par langue

| Dataset | \_en | \_ru | \_pt | \_es | \_ar | \_fa | \_zh |
|---|---|---|---|---|---|---|---|
| Rules | 15/15 | 15/15 | 15/15 | 15/15 | 15/15 | 15/15 | 15/15 |
| Rules PnP | 6/6 | 6/6 | 6/6 | 6/6 | 6/6 | 6/6 | 6/6 |
| Scenarii | 167/167 | 1336/1336 | 1336/1336 | 1336/1336 | 1336/1336 | 1336/1336 | 1336/1336 |

**0 cellules vides** sur les colonnes `_xx` — la traduction est complète côté cellule.

### Couverture de script (signe que les traductions sont **effectives**, pas des fallbacks FR)

| Dataset | \_en (latin) | \_ru (cyrillic) | \_pt (latin) | \_es (latin) | \_ar (arabic) | \_fa (arabic) | \_zh (cjk) |
|---|---:|---:|---:|---:|---:|---:|---:|
| Rules | 77% | 79% | 76% | 76% | 77% | 75% | 75% |
| Rules PnP | 77% | 80% | 76% | 77% | 79% | 75% | 80% |
| Scenarii | 79% | 86% | 81% | 82% | 84% | 80% | 92% |

**Lecture.** Les pourcentages de script attendu sont dans la plage 75-92% — bien au-dessus du seuil de bruit. Aucune langue n'est dominée par du latin dans une colonne cyrillique / arabe / CJK. **Pas de fallback FR silencieux détecté.**

### Contamination FR (caractères à/é/è/ç/etc. dans colonnes cyrillique/arabe/CJK)

| Dataset | Lang | Cas | Verdict |
|---|---|---:|---|
| Scenarii | \_ru (8 cols) | 1 | **Faux positif** — `trompe-l'œil` (emprunt lexical FR en russe : technique de peinture, terme culturel standard) |
| Scenarii | \_ar (8 cols) | 1 | **Faux positif** — `Caméléa` (nom propre, à garder tel quel) |
| Scenarii | \_fa (8 cols) | 3 | **Faux positifs** — noms propres `Caméléa`, `Obélix` |

**0 contamination FR authentique.** Les 5 cas détectés sont des emprunts lexicaux ou des noms propres, ce qui est correct pour des traductions.

### Cellules `ctrl` (newline `\n` au sein des cells multi-lignes)

| Dataset | Ctrl % global | Lecture |
|---|---:|---|
| Rules | 1-5% | Markdown structurel (`## headings`, listes) |
| Rules PnP | 1-4% | idem |
| Scenarii | 7-21% | Description cells denses avec paragraphes |

Cohérent avec des contenus Markdown longs (headings, listes, paragraphes). Pas de contrôle de séquence dans la cellule qui indiquerait une corruption.

## Verdict

✅ **Datasets propres côté i18n.** Pas de contamination FR authentique. Pas de fallback FR silencieux. Couverture 100% sur les colonnes de langue. Scripts attendus dominent à 75-92%.

⚠ **Note encoding** : les deux fichiers `Rules` portent un BOM UTF-8. C'est atypique par rapport aux autres CSVs du projet (no-BOM). À investiguer si :
- intentionnel (commodité Excel)
- accidentel (un export avec BOM par défaut)
- breaking pour certains parsers

**Pas de write prod dans cette PR.** Le BOM sur Rules est tracked mais non-fix (gated sur jsboige — potentiellement une décision de cohérence projet-wide).

## Out of scope

- ⛔ Écriture CSV (gated par jsboige).
- ⛔ Decision BOM vs no-BOM sur Rules (gated — investigation nécessaire : quel outil a produit ces fichiers, est-ce que les lectures CsvHelper + Papaparse sont résilientes).
- ⛔ Sweep additionnel sur `Cards/Rules/Argumentum Rules - Cards.old.csv` (fichier de backup, à confirmer s'il est tracké en git ou obsolète).

## Re-run

```bash
python tools/bytecheck-datasets.py
```

Idempotent, 0 dépendance externe (Python stdlib only), runtime <1s.

## Refs

- #202 (CSV write governance)
- `mt-garbage-sweep-false-zero` (memory — le sweep 1D mot-clé est insuffisant)
- `byte-check-multilang` (memory — byte-check ALL lang columns not just FR)
- `csv-byte-exact-column-insertion` (memory — field-segment splitter not csv round-trip)

— po-2024 (tick 24, dispatch ai-01 `07kpoq` idle secours)