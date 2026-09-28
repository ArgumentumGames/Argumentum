# Pool v22 — grain ⑱ : bandeaux hors deck ≠ titre de l'ancêtre (tri, mesure 0-écriture)

**Base** : `b646c8fd` (post-⑪c). **Commande** : ai-01 (#458 c.5877439841) — « trier les bandeaux hors deck qui diffèrent du titre de leur ancêtre, sans écriture ». **Méthode** : invariant bandeau (niveaux 1-3) == `text_<lang>` de l'ancêtre, restreint aux rangées **hors deck** (`carte` vide), 7 langues (fr exclu : 0 par construction, c'est la référence).

## Synthèse — 422 cellules en 29 clusters : 94 vides, 8 casse, 320 alias

Instrument validé contre le tableau ai-01 : **7/8 langues exactes** (ru 99, fa 109, zh 66, pt 16, ar 12, es 8, fr 0). L'écart « en » (ai-01 114, mesure 112) = les 2 cellules deck de `2.3.2.3.4` (« Playing the victim »), comptées par son balayage et **corrigées depuis** (#1634, grain ⑰) — les 94 vides hors deck restent.

**Chaque divergence appartient à un cluster systémique** (même ancêtre, même écart, toutes les rangées descendantes) — jamais de divergence isolée :

| Verdict | Cellules | Clusters | Lecture |
|---|---:|---:|---|
| **V — vide (en seulement)** | 94 | 14 | remplissage mécanique par le titre de l'ancêtre (même geste que ⑰, aucun choix éditorial) |
| **C — casse (pt)** | 8 | 1 | « Transferência Ilícita » → minuscule (règle typographique : toujours corriger) |
| **A — alias périmé** | 320 | 14 | le bandeau porte l'**ancienne** traduction du titre d'un ancêtre renommé depuis |

## A — la signature du renommage non suivi

La preuve interne que les alias sont du stale : l'ancêtre `3.2.1` (fr « Relation infondée ») a son cluster d'alias dans **les 7 langues** — « Spurious relationship » (en), « Необоснованная связь » porte « Безосновательное отношение » (ru), « Relación espuria » (es), « علاقة وهمية » (ar), « Relacionamento Espúrio » (pt), « رابطه‌ی کاذب » (fa), « 虚假关系 » (zh). Sept traductions indépendantes du même concept ont gardé **l'ancien** libellé pendant que les titres bougeient : c'est un renommage d'ancêtre dont les bandeaux hors deck n'ont pas suivi (les bandeaux du deck, eux, sont tenus par ④c depuis v20).

Clusters dominants :

| Langue | Ancêtre | Titre actuel | Bandeau (alias) | Rangées |
|---|---|---|---|---:|
| ru | `2.3.2` | Игра власти | **Игра престолов** *(Game of Thrones !)* | 87 |
| zh | `2.3.1` | 条件作用 | 条件反射 | 58 |
| fa | `2.1.3` | شعر | الشعر *(article défini arabe)* | 51 |
| fa | `7.3` | حمله شخصی | حمله به شخص | 38 |
| ×7 | `3.2.1` | (voir ci-dessus) | « spurious » par langue | 8/langue |
| en | `4.1.3` | Stork effect | Causalation | 6 |
| en/ru/ar/fa | `7.1.1` | (relativisme abusif) | « Relativism »/« чрезмерная »/… | 4/langue |

## V — les 94 vides anglais, concentrés

| Sous-arbre | Rangées vides | Colonnes touchées |
|---|---:|---|
| `2.3.2` | 64 | Subfamily (« Psychological manipulation ») + Subsubfamily (« Power games ») — même famille que ⑰ |
| `2.3.1` | 12 | Subsubfamily (« Conditioning ») |
| `6.1.1` | 8 | Family + Subfamily (« Spin doctoring »/« Lying ») |
| `2.1.1` / `5.3.3` / `2.2.2` | 10 | paires L2+L3 éparses |

## Proposition (à arbitrer — Règle C)

1. **V + C (102 cellules) : corriger mécaniquement** — remplir avec le titre courant de l'ancêtre, comme ⑰ et comme ④c l'exige déjà sur le deck. Aucun choix.
2. **A (320 cellules) : aligner sur le titre de l'ancêtre**, même règle que le deck — l'alias est un vestige pré-renommage, pas une alternative délibérée (aucune note éditoriale dans les cellules). Deux exceptions candidates à trancher nommément avant écriture :
   - ru `2.3.2` « Игра престолов » : le bandeau cite la série TV, le titre dit « jeu de pouvoir » — l'alignement retire une référence pop assumée ? (rien dans le dépôt ne documente l'intention) ;
   - en `4.1.3` « Causalation » : néologisme portemanteau actif dans la littérature (correlation-implies-causation), le titre « Stork effect » est plus étroit — l'alias est peut-être le MEILLEUR libellé, le problème serait alors le titre.
3. **Surface consommatrice VÉRIFIÉE — ce n'est pas cosmétique** : les colonnes `Family/Subfamily/Subsubfamily` (+ suffixes par langue) alimentent la localisation des mindmaps (`AssetConverterConfig.cs:330`, table `MindMapLocalization`). L'alias périmé est **vivant dans le livré** : « Игра престолов » apparaît **91 fois** dans le SVG committé `Cards/Fallacies/Mindmaps/ru/Fallacies_ru.content.svg` (mesuré). Corriger les bandeaux hors deck = corriger les mindmaps publiées à la régénération suivante — un correctif de livrable, pas de simple cohérence interne.

## Ce que cette mesure n'établit pas

- Aucune écriture : les 422 cellules attendent l'arbitrage ai-01 (le grain d'écriture suivra en série CSV).
- Les Vertus n'ont pas été balayées (issue #1632 : « non mesurées de la même façon ») — même instrument applicable.
- La date du renommage fauteur (quel commit a changé les titres sans les bandeaux) n'a pas été cherchée — inutile au tri, l'état courant suffit.

*po-2024*
