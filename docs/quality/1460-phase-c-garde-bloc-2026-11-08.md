# #1460 phase C — garde de séparation par bloc règle (08/11/2026, po-2023)

**Suite demandée par ai-01** à la revue de l'organe 3 ([#2196 c.6714912295](https://github.com/ArgumentumGames/Argumentum/issues/1460#issuecomment-6714912295), grain READY po-2023) : en groupant par **couleur distincte**, la garde de séparation de `FamilyColorIntegrity` ne voyait plus une collision **exacte** entre deux familles **différentes**.

## 1. La faille (mesurée sur témoin, pas sur gabarit réel)

Un copier-coller qui donnerait à `inférenceMaîtrisée` la couleur de `justesseLexicale` (`#0054a4` dans deux blocs distincts) :

- l'ancienne garde (paires sur les **noms**) la signalait : distance 0,0 < 60 ;
- la garde par **couleurs distinctes** (organe 3, 07/11) ne la voit plus : les deux noms groupés en **une** couleur = une seule unité de garde ;
- le contrôle **par carte** ne peut pas la voir : les cartes des deux familles sont rendues dans la couleur partagée et tombent exactement sur l'attendu ;
- le témoin existant `Two_Template_Families_Too_Close_Is_Red_Even_When_Every_Card_Is_Exact` éprouvait ~13, jamais 0.

La garde rougissait donc **à 0,1 mais plus à 0,0** — et la phrase « comportement inchangé sur le gabarit Fallacies » de l'organe 3 était vraie du gabarit **actuel**, pas d'un gabarit défectueux.

## 2. Le correctif — l'identité de bloc

Le renommage additif prescrit que **tous les noms successifs d'une famille cohabitent dans le même bloc** CSS. Le bloc règle EST donc l'unité de famille ; deux blocs distincts = deux familles, quelle que soit leur couleur.

- `FamilyColorIntegrity.FamilyBlock(Names, Color)` — le record qui porte l'identité perdue par la vue aplatie.
- `ParseFamilyBlocks(css)` — le **parseur de référence** (même itération de blocs que l'aplatissement) ; `ParseFamilyColors` devient une **vue aplatie** de la jumelle (dernier bloc gagnant, déterminisme inchangé, témoin dédié inchangé). **Le dictionnaire des organes 2 et 3 ne change pas de forme.**
- `Check(…, blocks = null)` — paramètre optionnel : fourni, la garde de séparation court sur les **paires de blocs** (label d'un bloc = ses noms joints par `|`) ; absent, repli sur les couleurs distinctes (comportement d'origine, chemin couvert par les témoins existants). Cas `d == 0` entre deux blocs distincts : message dédié (« portent la MÊME couleur — deux blocs règle distincts pour deux familles différentes ? ») — ce cas ne peut naître **que** de la voie blocs, le repli groupé ne produit jamais deux unités de même couleur.
- Les **deux gates** (organes 2 et 3, `VisualTests/Structure`) parsent désormais les blocs et les passent à `Check` — c'est la voie que la régénération arme.

La voie blocs est **strictement plus forte** que les deux précédentes : elle attrape la collision exacte (0,0) comme le rapproché (~13), sans rougir sur les alias légitimes d'un même bloc.

## 3. Témoins (3 nouveaux, `FamilyColorIntegrityTests` — 18 au total, CI sans artefact)

| Témoin | Attendu |
|---|---|
| `Two_Distinct_Blocks_With_The_Same_Color_Are_Red_Even_Though_Every_Card_Matches` | **rouge**, nomme `inférenceMaîtrisée` et `justesseLexicale`, « MÊME couleur » — cartes exactes : seule la garde de blocs voit la faille |
| `Aliases_In_One_Block_Do_Not_Trip_The_Block_Guard` | **vert** — une liste `card.exactitudeMathématique, card.sensQuantitatif` = 1 bloc = 1 famille |
| `Real_Virtues_Template_Gives_Eight_Family_Blocks_And_Passes_The_Block_Guard` | le gabarit réel : **8 blocs / 15 noms / 8 couleurs distinctes**, garde verte — un renommage qui écrirait le nouveau nom dans un bloc séparé rend ce témoin rouge |

Les témoins existants (proche ~13, alias par le repli, déterminisme, contrôle inverse du contrôle) passent inchangés — la voie repli garde son comportement.

## 4. Ce que ce correctif n'établit pas

- Il ne dit rien de plus de la **justesse** des couleurs (charpente §2) — il ferme un angle mort de l'instrument, pas un défaut des gabarits réels : les deux gabarits actuels (Fallacies 8 blocs, Virtues 8 blocs / 15 noms, mesurés) n'ont aucune collision.
- Un nom réapparaissant dans **deux blocs** de couleurs différentes : le parseur reste déterministe (dernier gagne, témoin dédié), mais la voie blocs voit alors deux unités de garde — un gabarit qui redéfinit une famille ailleurs qu'à sa couleur d'origine rougira. C'est voulu : une famille définie deux fois à deux teintes est ambiguë, l'instrument la nomme au lieu de choisir en silence.

*po-2023*
