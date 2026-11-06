# #1460 phase B — organe 1 : aucun placeholder littéral ne survit

**Date :** 2026-11-06 · **Lane :** po-2023 · **Base :** master `33f25e82` · **Rattachement :** #1460 (phase B), commentaire `c.6688584824`

## 1. Ce que l'organe assert

`Generation/Converters/Argumentum.AssetConverter.Tests/Structure/PlaceholderResidueGuardTests.cs` — 6 cas, aucun artefact requis, aucun réseau, inerte en CI comme le reste de la suite structurelle.

| Cas | Ce qu'il épingle |
|---|---|
| `Every_Card_Template_Variable_Resolves_Against_Its_Data` | chaque `{{variable}}` d'un gabarit vivant se résout contre le CSV du DataSet, le `csv` embarqué du gabarit, ou un built-in CardPen nommé |
| `Card_Template_Mustache_Blocks_Are_Balanced` | `{{#if}}`/`{{/if}}` et `{{#each}}`/`{{/each}}` équilibrés, par gabarit |
| `The_Template_Scan_Is_Not_Vacuous` | plancher anti-effondrement : ≥ 8 gabarits et ≥ 80 références **effectivement lues** (inventaire imprimé) |
| `A_Renamed_Column_Is_Detected_Mutation_Of_A_Real_Template` | **contrôle inverse** : renommer une variable réelle en colonne inexistante doit être signalé |
| `Wrapper_Templates_Leave_No_Token_After_The_Production_Formatter` | `[LANGUAGE] [DIRECTION] [TITLE] [SVGPATH] [SVGCONTENT]` tous consommés par `FormatWrapper`, sur les 2 gabarits racine |
| `An_Unconsumed_Token_Is_Detected_Mutation_Of_The_Wrapper_Formatter` | **contrôle inverse** : un token inconnu doit survivre au formateur et être vu |

## 2. Pourquoi cette source, et pas une autre

Mesuré le 06/11 (détail : [#1460 c.6688584824](https://github.com/ArgumentumGames/Argumentum/issues/1460#issuecomment-6688584824)) :

- le **PDF** ne porte **aucune couche texte** (0 caractère sur deux instruments, 0 police ; contrôle positif 17 616 caractères) ;
- le `.harvest.json` ne porte que des **pixels** (176 `data:image/png;base64`, 0 champ non-`dataUrl`, 0 occurrence du corps `desc_fr`).

Un test de « présence de contenu » écrit contre l'un ou l'autre aurait été **vert sur un bundle correct comme sur un bundle cassé**. Le texte vit un cran en amont : **`mustache` du gabarit × ligne du CSV du DataSet**.

## 3. Calibration — mesurée, pas supposée

Le classifieur devait distinguer trois choses : variable de données, helper, built-in d'exécution. Le vocabulaire a été **mesuré** avant d'être codé, sinon le test aurait inventé un second référentiel.

**Inventaire (master `33f25e82`, 11 gabarits vivants, 101 références) :**

| Gabarit | Colonnes | Références |
|---|---:|---:|
| `Cards/Rules/Argumentum_Rules_fr.json` | 11 | 2 |
| `Cards/Fallacies/Argumentum_Fallacies_Face_fr.json` | 105 | 13 |
| `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json` | 8 | 1 |
| `Cards/Fallacies/Argumentum_Virtues_Face_fr.json` | 82 | 11 |
| `Cards/Scenarii/Argumentum_Scenarii_Face_fr.json` | 70 | 14 |
| `Cards/Scenarii/Argumentum_Scenarii_Back_fr.json` | 70 | 4 |
| `Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json` | 105 | 12 |
| `Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json` | 105 | 14 |
| `Cards/Memo/Argumentum_Memo_Face_fr.json` | 105 | 10 |
| `Cards/Memo/Argumentum_Memo_Back_fr.json` | 105 | 7 |
| `Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json` | 105 | 13 |

**Built-ins d'exécution, nommés et sourcés :**

| Nom | Où il naît |
|---|---|
| `cardIndex` | `Generation/CardPen/js/main.js:1473` (injecté par carte) |
| `rowset`, `rowset.N.champ`, `rowset.[N].champ` | objet de regroupement construit par CardPen |
| `ifFamilyHeader`, `ifLeafFirst`, `ifSubfamilyHeader` | `main.js:1455-1470` (helpers enregistrés) |
| `ifCond`, `breaklines`, `markdown`, `each`, `if` | helpers Handlebars de CardPen |

⚠️ **Découverte de calibration** — `{{tagline_fr}}` (dos Fallacies et dos Virtues) n'existe dans **aucun** CSV du corpus : les 7 CSV n'ont aucune colonne `tagline*` (mesuré). Il se résout parce que ces CardSets déclarent `DataSet = KnownDataSets.None`, donc le **`csv` embarqué du gabarit** est leur seule source — et celui de `Argumentum_Fallacies_Back_fr.json` porte bien `tagline_fr,…,tagline_zh` (8 colonnes, 2 lignes). Un organe qui n'aurait lu que les CSV du corpus aurait rendu ce champ **faux rouge** ; un organe qui aurait lu l'embarqué pour tous aurait mesuré l'instantané `STALE` (`_csv_note`, #813/#812). Le contrat retenu est l'**union** des deux — et c'est la seule lecture qui ne se trompe sur aucun des deux cas.

⚠️ **`{{#ifCond Famille "==" text_fr }}`** — forme **bloc** d'un helper inline, mesurée sur `Memo_Face_fr`. Le classifieur y lit deux opérandes de données et un opérateur entre guillemets ; il a d'abord rendu `ifCond` comme variable (faux rouge), corrigé puis re-mesuré.

## 4. Ce que les contrôles inverses prouvent

- **Mutation du classifieur sur un gabarit réel** : la plus grande template est prise, sa première variable *résoluble* est renommée `zz_colonne_absente_zz`, et le classifieur doit le signaler. La mutation tourne **dans la suite**, donc elle ne peut pas pourrir en transcription — la leçon du pilote de #1460, dont la mutation du premier jet vivait dans un scratchpad.
- **Mutation du formateur** : `[ZZ_UNKNOWN_TOKEN]` ajouté à un gabarit synthétique doit **survivre** à `FormatWrapper` et être vu. Sans ce cas, le test « 0 résidu » passerait aussi bien si le formateur supprimait tout aveuglément.
- **Contrôle de visibilité du zéro** : sur les gabarits racine **bruts**, le test exige ≥ 1 token. Un zéro n'est une absence que si l'instrument pouvait voir un un — mesuré : `included.html` porte `[DIRECTION] [LANGUAGE] [SVGCONTENT] [TITLE]`, `external.html` porte `[DIRECTION] [LANGUAGE] [SVGPATH] [TITLE]`.

## 5. Ce que cet organe n'établit pas

⛔ Il lit la **résolution** des variables, pas la **substitution** : il ne prouve pas qu'une valeur de CSV donnée produit le bon pixel. ⛔ Il ne voit **ni la mise en page, ni le débordement, ni une police manquante, ni un texte présent au CSV mais masqué par le CSS** — ces défauts restent du ressort de la validation visuelle. ⛔ Il ne dit **rien de la justesse** d'une traduction : un `{{text_en}}` qui pointerait sur du français se résout parfaitement. ⛔ Le plancher (`≥ 8 gabarits`, `≥ 80 références`) est une **borne basse mesurée**, pas un compte exact : il ne doit pas devenir un oracle de cardinalité, les organes de contrat (§4 de la charpente) gardent les comptes. ⛔ Il ne couvre pas la **phase C** (structure : Rules d'abord, couleurs de famille).

## 6. Mesure de la suite

`dotnet test` sur `Argumentum.AssetConverter.Tests` (worktree `wt-1460-b1` depuis `33f25e82`, 06/11) : **4686 total · 4680 réussis · 0 échec · 6 ignorés**.

*po-2023*
