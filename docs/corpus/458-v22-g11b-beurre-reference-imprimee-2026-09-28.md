# Pool v22 — grain ⑪b : « Vouloir le beurre et l'argent du beurre » contre la référence imprimée (mesure 0-écriture)

**Base** : `f6e41174` (28/09, post-⑪/#1629). **Commande** : ai-01 — « mesure sans écriture de 6.2.2 : la description parle du refus de prendre position. À comparer à la référence imprimée (`Cards/Fallacies/Archive/`, jointure sur `path`) ». **Nature** : mesure, aucune écriture. Résout la C-note 5 du grain ⑪.

## Verdict — P (gardé) : la discordance titre/description est un héritage du jeu imprimé, pas une régression du corpus

La carte physique v1 portait **déjà** le titre-idiome « beurre » apparié à la description de refus de position. La rangée actuelle en descend fidèlement (reformulations datées, sémantique constante à chaque étape). Il n'y a rien à corriger contre la référence ; renommer serait une **nouvelle** décision éditoriale divergeant de l'imprimé, à arbitrer seulement si la lane owner le souhaite.

## Chaîne de preuve (jointure sur `path` = `6.2.2`)

### 1. v1 imprimé (`Cards/Fallacies/Archive/v1/Deck2/6.2.2.svg.png`, OCR WinRT fr-FR)

| Champ | Texte imprimé lu |
|---|---|
| Titre | « Beurre et argent du beurre » *(l'OCR tronque le dernier « e » — bords de carte)* |
| Description | « Vous n'adoptez pas clairement une position, afin qu'il soit impossible de vous prendre en défaut. » |
| Exemple | « Si j'ai par mégarde heurté quelqu'un dans cette assistance, je tiens à lui présenter mes excuses ; j'ai cependant agi en mon âme et conscience. » |
| Famille (pied de carte) | « Tricherie » |

⇒ L'appariement idiom-titre × description-hedging **existe dans la carte physique d'origine**. Le concept imprimé : garder une sortie de secours = vouloir le beurre ET l'argent du beurre.

### 2. Édition février 2022 (`Archive/2022/…fevrier 2022.csv`, 70 cartes) — la carte a disparu

Aucune rangée `6.2.2`. « Beurre et argent du beurre » n'y survit que comme **étiquette de sous-sous-famille** (EN : « Versatility ») de l'unique carte imprimée du groupe : `6.2.2.1.1` « Langue de bois ». Entre v1 et 2022, la carte 6.2.2 a été retirée du deck imprimé ; l'étiquette catégorielle, elle, est restée.

### 3. Taxonomie pré-#369 (rangée PK 992, lue dans le diff `9d45b4f9`)

La carte a été réintégrée avec le triplet v1 (titre « Beurre et argent du beurre », exemple **verbatim** v1, desc reformulée une fois : « Vous évitez de prendre position ferme pour ne jamais être pris en défaut. ») — et le champ `nom_vulgarisé` portait la note de l'éditeur, en capitales :

> « Versatilité : ON INVERSERAIT PAS LE TEXT_FR eT LE NOM VULGARISé? »

⇒ La question de l'inversion titre/nom vulgarisé **a déjà été posée par l'éditeur** — la note demande s'il ne faudrait PAS inverser ; la résolution retenue (voir #369) garde le titre-idiome et « Versatilité » comme nom vulgarisé.

### 4. #369 (`9d45b4f9`, 28/05/2026, passe « FR clarity » gpt-5.5) — la seule réécriture de fond

| Champ | Avant | Après (#369) |
|---|---|---|
| `text_fr` | Beurre et argent du beurre | **Vouloir le beurre et l'argent du beurre** |
| `desc_fr` | Vous évitez de prendre position ferme pour ne jamais être pris en défaut. | Vous évitez de prendre une position claire afin de ne pas pouvoir être mis en défaut. |
| `example_fr` | Si j'ai par mégarde heurté quelqu'un dans cette assistance… | Si j'ai offensé quelqu'un dans cette assemblée, même par inadvertance… |
| `nom_vulgarisé` | Versatilité : ON INVERSERAIT PAS… ? | Versatilité (note résolue/retirée) |

Sémantique inchangée sur les trois champs — le titre étend l'idiome, la desc et l'exemple restent le hedging/apologie v1.

### 5. #1522 (`789afcd8`, 23/09/2026) — alignement du bandeau uniquement

`Soussousfamille` « Beurre et argent du beurre » → « Vouloir le beurre et l'argent du beurre » (les 482 bandeaux alignés sur les titres). Le titre et la desc ne bougent pas.

## Cohérence inter-langues (état courant)

Toutes les cellules traduisent l'**idiome-titre** d'un côté (en « Having your cake and eating it too », ru « И на елку влезть… », pt « Ter o bolo e comê-lo também », es « Tener la mantequilla y el dinero de la mantequilla », zh « 鱼与熊掌兼得 », fa « هم کیک و هم خوردن آن », ar « أخذ الزبدة وثمنها ») et la **description-hedging** de l'autre — la discorde titre/desc est donc systémique et héritée, pas un accident fr. Le champ `proverbe` de la rangée conserve d'ailleurs les alternatives de l'éditeur (« retourner sa veste ? / ménager la chèvre et le chou ? » — cette dernière existe désormais comme nœud enfant `6.2.2.2`).

## Ce que cette mesure n'établit pas

- La troncature du proverbe ru « И на елку влезть… » (complet : « …и штанов не изорвать ») reste une question éditoriale indépendante (⑪ C-note 5, 2e moitié).
- Le sort de la carte entre v1 et 2022 (pourquoi retirée, pourquoi réintégrée) n'est pas documenté dans le dépôt — hors périmètre de cette mesure.
- Aucune écriture : un éventuel renommage (« Versatilité » en titre, ou desc réalignée sur l'idiome) serait une décision nouvelle **contre** la référence imprimée — à la main d'ai-01.

*po-2024*
