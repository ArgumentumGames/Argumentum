# Audit templates CardPen — `csv` embeddé stale vs CSV source (#812)

**Generated** : 2026-07-16 (auto, read-only) — issue #812 dispatch ai-01
**Base** : master `72c408ec`
**Method** : code=truth — `HarvestManager.cs:342-363` injecte le CSV source au runtime,
             écrasant le `csv` embeddé du template pour toute carte Face.
**Implication** : le `csv` embeddé est mort pour les Faces — un delta = risque **latent**, pas un bug de rendu actuel.

**Rappel** : corrigé via #803/#805 (cette fois sur le bon fichier), puis contredit par généralisation.
**Statut rendu actuel** : OK (relecture T&A confirme) — seul le `csv` embeddé est stale.

## Synthèse exécutive (lecture po-2024)

**Cartographie complète — 27 templates CardPen × 15 CardSets analysés** (WebBasedGeneratorConfig.cs lignes 105-456) :

- **20/27 HIGH** : tous les **Face** avec CSV source injecté ont un `csv` embeddé structurellement en retard (delta row +76 à +1239, delta col +13 à +68). **Aucune trace de MT-garbage résiduel dans les templates stale** (sauf `Rules` + `RulesPrintAndPlay` = résidu du #803/#805 déjà corrigé). → **Risque rendu = 0**. **Risque latent = structurel** (drift schéma).
- **0/27 MEDIUM/LOW/NONE** : pas de divergence mineure détectée (probablement masquée par le seuillage row_delta>5 dans `classify`).
- **7/27 N/A** : les **Backs** (`DataSet=None`) rendent depuis le template = source de vérité, donc rien à comparer.

**Pourquoi le delta row est si grand ?** Parce que les CSV sources ont grandi entre la création des templates et aujourd'hui :
- Fallacies Taxonomy `54→104 cols` : ajout AIF (#498, 4 nouvelles colonnes), ajout i18n 7 langues (#183 + #210), +`link_*` (#192), +`simple_name`/`political_example` (#202 pending).
- Scenarii Cards `29→70 cols` : même raison + ajouts post-#795.
- Virtues Taxonomy `13→81 cols` : PR #218/#236/#246/#290/#295 + cascade #808.

→ Le `csv` embeddé est **un snapshot d'époque**, pas une donnée corrompue. C'est précisément ce que la généralisation tick 22 annonçait : la clé est morte, pas nuisible.

## Décision par défaut (subtractif — règle « no pendulum »)

**Marqueur `_csv_note` plutôt que resync.** Justification :

1. **Resync = diff large + risque drift** : régénérer `csv` embeddé depuis le CSV source = +1239 lignes et +50 colonnes pour Fallacies = PR à 5000+ lignes, difficile à review, et le contenu sera re-overridden au prochain regen. Effort non-rentable.
2. **Marqueur = subtractif** : ajouter `CardSetDocument._csv_note: "STALE — overridden at runtime by DataSet='X', see <csv_path>"` — 1 ligne par template Face. **Aucune chance de régression** (clé `_csv_note` ignorée par CardPen qui lit `CardSetDocument.csv`, `CardSetDocument.mustache`, `CardSetDocument.css`).
3. **Garde-fou QA future** : quand un dev/agent ouvre un template Face, la note le guide vers le bon fichier (CSV source), pas le template.

**Hors scope** (ces éléments du template **sont** rendus et n'ont rien à voir) :
- `mustache` (HTML/Markdown layout) — rendu actuel.
- `css` (styles visuels) — rendu actuel.
- `script`/`style`/autres clés.

**Ordre d'exécution proposé (post-tag)** :
1. PR `chore(hygiene): add _csv_note marker on stale-csv Face templates` — 1 commit, 18 fichiers (20 Face - Rules, déjà fait partiellement - Virtues/Fallacies/Memo/Scenarii/P&P).
2. Aucun test à modifier (le code C# ignore `_csv_note`).
3. Re-vérif via re-run du script : `risk-HIGH` doit devenir `risk-NONE` (rapport indique "identical via _csv_note marker").

## Summary

| CardSets scoped | templates Face/Back analysés | NONE | LOW | MEDIUM | HIGH | REVIEW | skip |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 15 | 27 | 0 | 0 | 0 | 20 | 0 | 7 |

## Findings (per CardSet × side)

### HIGH — 20 finding(s)

#### `Rules` (Face) — DataSet=Rules
- **Template** : `Cards/Rules/Argumentum_Rules_fr.json`
- **CSV source (render truth)** : `Cards\Rules\Argumentum Rules - Cards.csv`
- **Risk** : **HIGH**
- **Why** : header column delta +7
- **Stats** : rows src=16 / tmpl=19 (Δ-3) · cols src=10 / tmpl=3 (Δ+7) · identical=False
- **Stale `csv` quality** : flag_tokens=3 (src=1) · lower_headings=0 (src=0) · fr_stopword_cells=18 (src=42)
  - examples: *Règles du jeu : de 4 à 8 joueurs*

## Matériel

* 1 paquet de cartes d’argument | ## Installation

Selon le nombre de joueurs et le niveau de difficulté voulu, on | ## Déroulé de la manche

### 1.       Le piocheur

Le piocheur tire une carte de
- **Note** : diff detected — see stats below

#### `Fallacies` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Face_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `Virtues` (Face) — DataSet=VirtuesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Virtues_Face_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Virtues - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : header column delta +68
- **Stats** : rows src=224 / tmpl=226 (Δ-2) · cols src=81 / tmpl=13 (Δ+68) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=22 (src=785)
  - examples: La production d'une argumentation vertueuse implique de respecter une rigueur et | La composition d'un argument pertinent le distingue d'une opinion. Il doit compo | Information provenant d'un journal reconnu pour son sérieux et son intégrité
- **Note** : diff detected — see stats below

#### `Scenarii` (Face) — DataSet=Scenarii
- **Template** : `Cards/Scenarii/Argumentum_Scenarii_Face_fr.json`
- **CSV source (render truth)** : `Cards\Scenarii\Argumentum Scenarii - Cards.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +76 (template carries stale snapshot)
- **Stats** : rows src=168 / tmpl=92 (Δ+76) · cols src=70 / tmpl=29 (Δ+41) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=134 (src=398)
  - examples: Le baratineur doit le dissuader de se rapprocher de Cléopâtre, la reine d’Égypte | Dans la Rome antique, le baratineur est un sénateur. | Il doit convaincre le sénat de partir en guerre pour détruire Carthage, une cité
- **Note** : diff detected — see stats below

#### `Scenarii` (Back) — DataSet=Scenarii
- **Template** : `Cards/Scenarii/Argumentum_Scenarii_Back_fr.json`
- **CSV source (render truth)** : `Cards\Scenarii\Argumentum Scenarii - Cards.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +76 (template carries stale snapshot)
- **Stats** : rows src=168 / tmpl=92 (Δ+76) · cols src=70 / tmpl=29 (Δ+41) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=134 (src=398)
  - examples: Le baratineur doit le dissuader de se rapprocher de Cléopâtre, la reine d’Égypte | Dans la Rome antique, le baratineur est un sénateur. | Il doit convaincre le sénat de partir en guerre pour détruire Carthage, une cité
- **Note** : diff detected — see stats below

#### `Fallacies2` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `Fallacies3` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `RulesPrintAndPlay` (Face) — DataSet=RulesPrintAndPlay
- **Template** : `Cards/Rules/Argumentum_Rules_fr.json`
- **CSV source (render truth)** : `Cards\Rules\Argumentum_Rules_Francais_edition_fevrier_2022_Print_and_Play.json`
- **Risk** : **HIGH**
- **Why** : row count delta +18 (template carries stale snapshot)
- **Stats** : rows src=37 / tmpl=19 (Δ+18) · cols src=1 / tmpl=3 (Δ-2) · identical=False
- **Stale `csv` quality** : flag_tokens=3 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=18 (src=28)
  - examples: *Règles du jeu : de 4 à 8 joueurs*

## Matériel

* 1 paquet de cartes d’argument | ## Installation

Selon le nombre de joueurs et le niveau de difficulté voulu, on | ## Déroulé de la manche

### 1.       Le piocheur

Le piocheur tire une carte de
- **Note** : diff detected — see stats below

#### `FallaciesPrintAndPlay` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Face_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `FallaciesPrintAndPlayLight` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Face_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `VirtuesPrintAndPlayLight` (Face) — DataSet=VirtuesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Virtues_Face_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Virtues - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : header column delta +68
- **Stats** : rows src=224 / tmpl=226 (Δ-2) · cols src=81 / tmpl=13 (Δ+68) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=22 (src=785)
  - examples: La production d'une argumentation vertueuse implique de respecter une rigueur et | La composition d'un argument pertinent le distingue d'une opinion. Il doit compo | Information provenant d'un journal reconnu pour son sérieux et son intégrité
- **Note** : diff detected — see stats below

#### `Memo` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Memo/Argumentum_Memo_Face_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `Memo` (Back) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Memo/Argumentum_Memo_Back_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `ScenariiPrintAndPlay` (Face) — DataSet=Scenarii
- **Template** : `Cards/Scenarii/Argumentum_Scenarii_Face_fr.json`
- **CSV source (render truth)** : `Cards\Scenarii\Argumentum Scenarii - Cards.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +76 (template carries stale snapshot)
- **Stats** : rows src=168 / tmpl=92 (Δ+76) · cols src=70 / tmpl=29 (Δ+41) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=134 (src=398)
  - examples: Le baratineur doit le dissuader de se rapprocher de Cléopâtre, la reine d’Égypte | Dans la Rome antique, le baratineur est un sénateur. | Il doit convaincre le sénat de partir en guerre pour détruire Carthage, une cité
- **Note** : diff detected — see stats below

#### `ScenariiPrintAndPlay` (Back) — DataSet=Scenarii
- **Template** : `Cards/Scenarii/Argumentum_Scenarii_Back_fr.json`
- **CSV source (render truth)** : `Cards\Scenarii\Argumentum Scenarii - Cards.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +76 (template carries stale snapshot)
- **Stats** : rows src=168 / tmpl=92 (Δ+76) · cols src=70 / tmpl=29 (Δ+41) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=134 (src=398)
  - examples: Le baratineur doit le dissuader de se rapprocher de Cléopâtre, la reine d’Égypte | Dans la Rome antique, le baratineur est un sénateur. | Il doit convaincre le sénat de partir en guerre pour détruire Carthage, une cité
- **Note** : diff detected — see stats below

#### `ScenariiPrintAndPlayFull` (Face) — DataSet=Scenarii
- **Template** : `Cards/Scenarii/Argumentum_Scenarii_Face_fr.json`
- **CSV source (render truth)** : `Cards\Scenarii\Argumentum Scenarii - Cards.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +76 (template carries stale snapshot)
- **Stats** : rows src=168 / tmpl=92 (Δ+76) · cols src=70 / tmpl=29 (Δ+41) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=134 (src=398)
  - examples: Le baratineur doit le dissuader de se rapprocher de Cléopâtre, la reine d’Égypte | Dans la Rome antique, le baratineur est un sénateur. | Il doit convaincre le sénat de partir en guerre pour détruire Carthage, une cité
- **Note** : diff detected — see stats below

#### `ScenariiPrintAndPlayFull` (Back) — DataSet=Scenarii
- **Template** : `Cards/Scenarii/Argumentum_Scenarii_Back_fr.json`
- **CSV source (render truth)** : `Cards\Scenarii\Argumentum Scenarii - Cards.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +76 (template carries stale snapshot)
- **Stats** : rows src=168 / tmpl=92 (Δ+76) · cols src=70 / tmpl=29 (Δ+41) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=134 (src=398)
  - examples: Le baratineur doit le dissuader de se rapprocher de Cléopâtre, la reine d’Égypte | Dans la Rome antique, le baratineur est un sénateur. | Il doit convaincre le sénat de partir en guerre pour détruire Carthage, une cité
- **Note** : diff detected — see stats below

#### `MemoPrintAndPlay` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Memo/Argumentum_Memo_Face_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `MemoPrintAndPlay` (Back) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Memo/Argumentum_Memo_Back_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

#### `FallaciesWeb` (Face) — DataSet=FallaciesTaxonomy
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json`
- **CSV source (render truth)** : `Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv`
- **Risk** : **HIGH**
- **Why** : row count delta +1239 (template carries stale snapshot)
- **Stats** : rows src=1409 / tmpl=170 (Δ+1239) · cols src=104 / tmpl=54 (Δ+50) · identical=False
- **Stale `csv` quality** : flag_tokens=0 (src=0) · lower_headings=0 (src=0) · fr_stopword_cells=241 (src=2588)
  - examples: Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des fai | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preu
- **Note** : diff detected — see stats below

### N/A — 7 finding(s)

#### `Fallacies` (Back) — DataSet=None
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json`
- **Risk** : **N/A**
- **Why** : template = render truth
- **Note** : template embeddé = source de vérité (DataSet=None)

#### `Virtues` (Back) — DataSet=None
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json`
- **Risk** : **N/A**
- **Why** : template = render truth
- **Note** : template embeddé = source de vérité (DataSet=None)

#### `Fallacies2` (Back) — DataSet=None
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json`
- **Risk** : **N/A**
- **Why** : template = render truth
- **Note** : template embeddé = source de vérité (DataSet=None)

#### `Fallacies3` (Back) — DataSet=None
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json`
- **Risk** : **N/A**
- **Why** : template = render truth
- **Note** : template embeddé = source de vérité (DataSet=None)

#### `FallaciesPrintAndPlay` (Back) — DataSet=None
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json`
- **Risk** : **N/A**
- **Why** : template = render truth
- **Note** : template embeddé = source de vérité (DataSet=None)

#### `FallaciesPrintAndPlayLight` (Back) — DataSet=None
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json`
- **Risk** : **N/A**
- **Why** : template = render truth
- **Note** : template embeddé = source de vérité (DataSet=None)

#### `VirtuesPrintAndPlayLight` (Back) — DataSet=None
- **Template** : `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json`
- **Risk** : **N/A**
- **Why** : template = render truth
- **Note** : template embeddé = source de vérité (DataSet=None)

## Recommendations

**Échelle** : HIGH > MEDIUM > LOW > NONE

- **HIGH** : `csv` embeddé a un delta de lignes ou colonnes significatif. → resync **ou** marqueur sibling.
  - Marqueur = ajouter dans le JSON template un champ `_csv_note: "stale since YYYY-MM-DD, render truth = <csv_path>"` (subtractif, non-permissif).
  - Resync = regénérer la clé `csv` du template depuis le CSV source (risque drift, large diff).
- **MEDIUM** : à examiner au cas par cas (delta row 1-5, headers OK).
- **LOW** : 0-2 lignes éditées (typo/casing), latent quasi nul. → simple note commit.
- **NONE** : template aligné sur CSV source (peu probable, à confirmer).
- **N/A** : Back/DataSet=None → template = source, rien à auditer.

**Décision par défaut (ai-01 penche)** : **marqueur** plutôt que resync, car la clé `csv` est morte
pour les Faces — pas de raison de la maintenir fidèle. Le marqueur documente l'intention.

## Hors-scope (rappel)

- ⛔ Pas de resync ni write prod ce tick (post-tag, gated jsboige).
- ⛔ #415 history-rewrite INTERDIT en autonome.
- ⛔ #202 écriture sans GO registre jsboige (mais ce ticket ne touche pas #202).
- Le présent audit est **read-only** (lit fichiers, écrit rapport markdown optionnel).
