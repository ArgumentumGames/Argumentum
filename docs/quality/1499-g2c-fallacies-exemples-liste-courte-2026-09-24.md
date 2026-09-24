# #1499 grain G2-C — Liste courte des pertes `example_fr` (baseline 2024 → HEAD)

**Date** : 2026-09-24 · **Lane** : po-2024 (worker) · **Base** : `origin/master` `c51037b6`
**Instrument** : [`docs/corpus/fallacies-exemples-g2c.py`](../corpus/fallacies-exemples-g2c.py) (paires entières baseline→HEAD, similarité difflib, drapeaux tu/vous) sur la sortie régénérée de [`fallacies-exemples-diff.py`](../corpus/fallacies-exemples-diff.py) — 4 témoins a–d PASS, surface **111/111**.
**Mandat** : dossier G2 §9.2 + dispatch [#458 c.5796151802](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5796151802). « L'ancien » = **baseline `62b561e75`** (version d'avant la couche agentique — consigne [#458 c.5801633624](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5801633624)), ⛔ pas l'imprimé 2022.

**⛔ Aucune écriture CSV** — dossier d'arbitrage seul, règle C : **défaut = garder HEAD**.

---

## Réponse en une ligne

Les **111 cellules** ont été lues une à une (deux textes entiers chacune) : **93 sont des reformulations sur place** (même scène, même fond, parfois meilleures) et **18 des remplacements totaux** (similarité < 0,5). Parmi les 18 : **11 emportent une « glose » didactique** — une leçon collée après un tiret, qui dédouble `desc_fr` et parfois nomme le sophisme dans l'exemple même ; **3 corrigent la visée** (l'ancien n'illustrait pas le sophisme de la carte) ; **1 déplace le mécanisme** vers une autre carte (PK 598) ; 2 remplacent une scène valable par une autre scène valable ; 1 n'est qu'une mise en chiffres (644). **Liste courte : 14 items** — 13 « garder l'actuel » (dont 11 : retirer la glose, pas l'exemple), **1 « revenir à l'ancien » : PK 598**.

## 1. Ce que la lecture a compté (VERIFIÉ, cellule par cellule)

| Catégorie | # | PK |
|---|---:|---|
| Reformulation sur place (sim ≥ 0,5) | 93 | le reste de la surface |
| Remplacement total (sim < 0,5) **avec glose didactique** | **11** | 33, 322, 362, 432, 658, 796, 834, 848, 908, 1092, 1120 |
| Remplacement total, **visée corrigée** (l'ancien n'illustrait pas le sophisme) | 3 | 653, 888, 973 |
| Remplacement total, **mécanisme déplacé** | **1** | **598** |
| Remplacement total, scène équivalente | 2 | 176, 340 |
| Basse similarité trompeuse (chiffres en mots → symboles) | 1 | 644 |

Similarités : min 0,01 · médiane 0,88 · max 0,99. Longueurs : 74 allongés / 34 raccourcis / 3 égaux — les 5 plus forts allongements (1092 : 41 → 364 c. ; 848 : 57 → 306 ; 796 : 104 → 295 ; 658 : 145 → 307 ; 834 : 99 → 277) sont **tous** des items à glose : c'est la glose qui fait la longueur, pas l'exemple.

## 2. La question du tutoiement → vouvoiement (posée UNE fois)

Le passage est **systématique** : le drapeau lexical (tu/ton/ta/te/t' dans l'ancien ET vous/votre/vos dans l'actuel) touche **23 des 111** cellules, et le contrôle inverse (vous dans l'ancien ET tu dans l'actuel) rend **0 cas**. C'est un changement de registre global, à sens unique, pas une décision cellule par cellule.

> **Question unique à l'owner** : le vouvoiement est-il le registre voulu pour toutes les cartes ? Si oui, ces 23 suivent la règle C (garder) sans item ; si non, c'est un geste d'écriture global à dispatcher séparément — pas 23 retours individuels.

## 3. Liste courte — 14 items, deux textes entiers, verdict en mots courants

### 3.1 Les gloses didactiques (11 items) — garder l'exemple, la glose est en cause

Dans les 11 cas, le nouvel exemple **vise mieux le sophisme** que l'ancien, mais une leçon est collée après un tiret : elle répète la définition (`desc_fr`), et 3 items **nomment le sophisme dans l'exemple** (322, 1092, 1120) — une carte qui révèle sa propre réponse. Verdict proposé : **garder l'actuel, retirer la phrase après le tiret** (le retrait est un geste d'écriture à part, soumis à décision).

**PK 33 · Justification triviale**
- Ancien : « J'ai entendu dire que cette personne était sexiste ; il m'est donc impossible d'être en accord avec elle sur quoi que ce soit. »
- Actuel : « Je conviens que votre calcul est correct. Mais votre raisonnement est insignifiant : ce détail ne change rien au problème de fond. — Rejet d'un argument correct sur la base de son importance perçue, pas de sa validité. »
- Raison : l'ancien illustrait l'amalgame, pas la justification triviale ; le nouveau vise juste, mais la phrase après le tiret fait la leçon à la place de la définition.

**PK 322 · Repoussoir**
- Ancien : « Ce n'est tout de même pas une poignée de jeunes imbéciles qui va vous apprendre votre métier ! »
- Actuel : « Les collapsologues veulent nous ramener à l'âge de pierre et abolir toute industrie. — Repoussoir : caricature volontairement excessive d'un mouvement réel pour rendre sa réfutation plus facile et plus applaudie. »
- Raison : le nouveau montre mieux le procédé, mais la glose **nomme le sophisme** dans l'exemple même.

**PK 362 · Sandwich de louanges**
- Ancien : « Ton travail sur le projet a été impressionnant, vraiment. Par contre, il y a eu quelques erreurs dans tes calculs. Mais dans l'ensemble, tu as montré un grand engagement, et c'est ce qui compte le plus. »
- Actuel : « Votre travail sur le projet a été impressionnant. En revanche, il y a eu quelques erreurs dans vos calculs. Mais, dans l'ensemble, vous avez montré un grand engagement. — Technique managériale : la critique centrale est encadrée par deux compliments, ce qui augmente le taux d'acceptation de la critique tout en diminuant la perception de son importance. »
- Raison : même structure en sandwich, passage au vouvoiement, puis une glose de manuel qui double la définition.

**PK 432 · Engagement**
- Ancien : « Puisque les bonnes ménagères que vous êtes avez été convaincues par nos recettes alléchantes et leurs bienfaits diététiques, que celles qui vont cuisiner des abats à leur mari la semaine prochaine lèvent la main. »
- Actuel : « Puisque vous êtes favorables à la transition écologique, que ceux d'entre vous qui accepteront de prendre le train plutôt que l'avion pour leurs prochains déplacements lèvent la main. — Lewin (1947) : l'engagement public active l'acceptation privée, indépendamment du sujet. »
- Raison : le nouveau garde le mécanisme (levée de main publique) avec une scène plus actuelle ; la référence académique appartient à la définition, pas à l'exemple.

**PK 658 · Infini trompeur**
- Ancien : « Si vous commencez à questionner la moralité de consommer des produits animaux, vous finirez par remettre en question toute forme de consommation. »
- Actuel : « Cette affirmation est vraie. — Mais comment le savez-vous ? Je l'ai vérifiée. — Mais comment avez-vous vérifié cette vérification ? Et la vérification de cette vérification, comment l'avez-vous vérifiée ? … — Régression à l'infini de la justification épistémique : chaque niveau de preuve en exige un autre. »
- Raison : l'ancien illustrait la pente glissante, pas la régression à l'infini ; le dialogue nouveau vise juste, la glose finale est de trop (et l'item passe de 145 à 307 caractères).

**PK 796 · Quaternio terminorum**
- Ancien : « Seuls les Hommes naissent libres. Les femmes ne sont pas des hommes. Donc les femmes ne sont pas libres. »
- Actuel : « Tous les avocats défendent des clients au tribunal. Ce fruit est un avocat. Donc ce fruit défend des clients au tribunal. — « avocat » change de sens : profession juridique dans la première prémisse, fruit dans la seconde. Le raisonnement contient donc en réalité quatre termes au lieu de trois. »
- Raison : le jeu sur « avocat » est plus limpide que l'ancien (qui jouait sur « Hommes/hommes ») ; la glose explique le mécanisme que la définition doit déjà porter.

**PK 834 · Comparaison abusive**
- Ancien : « Les lycéens sont comme une meute de chiens féroces : il convient de leur montrer qui est le maître. »
- Actuel : « Gérer une classe, c'est comme piloter une machine : si une pièce ralentit le système, il suffit de la remplacer. — Comparaison abusive : une classe n'est pas une machine, et les élèves ne sont pas des pièces interchangeables. La comparaison efface des différences essentielles. »
- Raison : le nouveau est plus proche de la vie de classe et montre mieux l'effacement des différences ; la glose répète la définition.

**PK 848 · Ponctuation ambiguë**
- Ancien : « Mes élèves qui sont en surpoids boivent beaucoup de soda. »
- Actuel : « Les membres du comité qui ont validé ce dossier seront convoqués. — Restrictive : seuls les membres ayant validé ce dossier seront convoqués. Les membres du comité, qui ont validé ce dossier, seront convoqués. — Explicative : tous les membres seront convoqués, et l'on précise qu'ils ont validé ce dossier. »
- Raison : la phrase seule illustre l'ambiguïté ; la mini-leçon (deux lectures expliquées) double la définition et triple la longueur (57 → 306 c.).

**PK 908 · Factoïde**
- Ancien : « Les élites dirigeatntes ont voulu toutes ces calamités pour mieux contrôler les masses. »
- Actuel : « On sait tous que les pyramides ont été construites par des esclaves. — Largement cru, historiquement faux : les travailleurs de Gizeh étaient des ouvriers rémunérés, logés et nourris sur le site. »
- Raison : l'ancien portait une faute d'orthographe et une thèse complotiste ; le nouveau est un factoïde canonique (cru mais faux) — la glose donne la réfutation, rôle de la définition.

**PK 1092 · Biais de négativité**
- Ancien : « Je déteste perdre plus que j'aime gagner. »
- Actuel : « Le rapport indique que la nouvelle politique a amélioré neuf indicateurs sur dix : moins d'accidents, moins de retards, moins de coûts et une meilleure satisfaction des usagers. Mais je la refuse, parce qu'un indicateur s'est légèrement dégradé. — Biais de négativité : un élément négatif limité pèse plus lourd que plusieurs éléments positifs pourtant importants. »
- Raison : l'ancien tenait en une phrase timide ; le nouveau montre le mécanisme — mais la glose **nomme le sophisme**, et l'item fait 9 fois la longueur de l'ancien (41 → 364 c.).

**PK 1120 · Pensée dichotomique**
- Ancien : « Si je n'ai pas eu un A à mon examen, alors je suis complètement nul en cette matière. »
- Actuel : « Soit j'ai la moyenne, soit je suis complètement nul. — Dichotomie : il n'existe aucune autre note entre « moyenne » et « complètement nul », alors que la notation sur /20 offre une granularité bien plus fine. »
- Raison : les deux versions montrent la dichotomie ; la glose nomme le mécanisme et souligne ce que la définition doit dire.

### 3.2 Les remplacements de scène (3 items)

**PK 598 · Généralisation hâtive — ⭐ le seul « revenir » proposé**
- Ancien : « Avez-vous vu l'augmentation des cas de fraude chez les nouveaux employés? La nouvelle génération n'est pas fiable. »
- Actuel : « Le sondage a été réalisé à la sortie du congrès : 90 % des personnes interrogées soutiennent la motion. L'opinion du pays est donc faite. »
- Raison : le nouvel exemple illustre **l'échantillon biaisé** — c'est la carte PK 596, sa voisine, qui porte ce sophisme ; l'ancien (quelques fraudes → « toute une génération ») est la généralisation hâtive elle-même. La carte porte le titre « Généralisation hâtive » depuis G1-W et reçoit l'IRI `hastyGeneralization` (Q-11) : son exemple devrait montrer CE sophisme-là. **Revenir à l'ancien.**

**PK 176 · Procédé rhétorique — garder, perte signalée**
- Ancien : « Si votre ramage se rapporte à votre plumage, vous êtes le phénix des hôtes de ces bois. »
- Actuel : « Votre éloquence est à la hauteur de votre prestance : vous êtes vraiment exceptionnel. »
- Raison : même flatterie, mais la **citation du Renard et du Corbeau** (La Fontaine) est perdue — c'était la référence culturelle la plus reconnaissable du corpus. La paraphrase est autonome et lisible ; à garder sauf si l'owner tient au clin d'œil littéraire.

**PK 340 · Appel aux conséquences — garder**
- Ancien : « Je ne donne jamais d'argent aux mendiants car je crains qu'ils n'en profitent pour acheter de l'alcool. »
- Actuel : « Admettons que ce diagnostic soit exact : il faudrait tout arrêter, prévenir les familles, et reconnaître publiquement notre erreur. Vous voyez bien que ce n'est pas possible. »
- Raison : deux scènes valables pour le même sophisme (refuser une conclusion parce que ses conséquences déplairaient) ; la nouvelle montre le mécanisme d'irréalisme plus franchement. Pas de perte nette.

## 4. Hors liste courte — ce que l'agent a amélioré (mesuré en lisant)

Candidats au retour examinés et écartés, parce que l'ancien n'illustrait **pas** le sophisme de la carte :

- **PK 653 · Main chaude** — ancien : « Mieux vaut ne rien entreprendre : rien ne me réussit ces temps-ci ! » (superstition du renoncement) ; actuel : trois paniers marqués → le prochain rentré forcément (la main chaude canonique).
- **PK 888 · Arranger les faits** — ancien : déresponsabilisation (« tu as pris la mauvaise décision ») ; actuel : omission volontaire de tests échoués (l'arrangement des faits lui-même).
- **PK 973 · Changement de cap** — ancien : « offrir un beau spectacle au public » (diversion) ; actuel : une étude exigée, puis trois études et une méta-analyse quand elle arrive (le drapeau qu'on déplace).

Corrections et modernisations au fil des 93 reformulations (liste non exhaustive, aucune perte) : « averré » → « avéré » (632), « dirigeatntes » corrigé par remplacement (908), fautes de la baseline réparées (247 : « lui-seul », « le secrets »), « taxes » → « impôts » et « géner » → « gêner » (713), « bon » → « bons » (1301), « les hommes » → « les êtres humains » (108), précision du biais dans l'exemple (596 : « interrogés dans notre magasin » ; 942 : « dont personne ne connaît les membres » ; 953 : « Inutile de parler des risques »).

## 5. Ce que ce dossier n'établit pas

- ⛔ La qualité littéraire des reformulations — jugement de goût hors mandat ; règle C : défaut = garder.
- ⛔ Le tri des 23 tu→vous item par item (§2 : une question, pas 23 lignes).
- ⛔ La chronologie fine : la surface est datée « post-baseline » en bloc ; chaque cellule daterait par `git log -S` individuellement (G2 §8).
- ⛔ Ne couvre pas `desc_fr` (G3 v2 : « rien »), ni `text_fr` (G1-W), ni Virtues (G4-C à venir), ni Rules/Scenarii.
- ⚠️ La similarité difflib sous-estime les remplacements de vocabulaire (leçon #1516) : le seuil 0,5 est un **rideau de lecture**, pas une frontière — les 93 « reformulations » ont été lues une à une, pas extrapolées.

## DoD

- [x] 111/111 lues cellule par cellule (fichier de paires entières, instrument committé).
- [x] Liste courte : une ligne par item, les deux textes cités **en entier**, verdict et raison en mots courants.
- [x] tu→vous compté (23) et signalé **une seule fois**, sous forme de question (§2).
- [x] ⛔ Zéro écriture CSV (dossier + instrument seulement).
- [x] Règle C appliquée : 13 garder / 1 revenir.

---

*Mesuré et lu par `myia-po-2024` (worker lane) le 2026-09-24. Instruments stdlib, lecture seule stricte. La mesure se refait en deux passes (`fallacies-exemples-diff.py` puis `fallacies-exemples-g2c.py`).*
