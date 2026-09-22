# Registre des changements sur les cartes **existantes** — Fallacies FR

> **Ce document est une MESURE, pas un verdict.** Il compare le corpus courant à une référence **présente dans le dépôt**, et propose une classe par cellule. ⛔ **Aucune classe n'est tranchée ici** : la classe *D — dérive sémantique* exige de savoir quel sophisme la carte illustre, et c'est précisément le jugement qui a manqué.
>
> Ouvert sur mandat owner du **2026-09-22** : *« nous présenter un tableau argumenté des changements qui ont été effectués sur des cartes existantes et la justification associée »*. Porté par **#1503**, enfant de l'Epic **#1499**.

---

## 1. La référence — et pourquoi elle rend l'OCR inutile

Le premier cadrage de #1503 annonçait qu'il faudrait **OCR-iser le tirage de février 2022**, faute de référence diffable : le dépôt commence en mai 2023, un an après l'impression.

⭐ **C'était faux, et la réfutation est dans le dépôt, sur HEAD.** `Cards/Fallacies/Archive/` porte deux corpus jamais touchés par la vague agentique :

| Archive | Cartes | Ce que c'est |
|---|---:|---|
| `Archive/2022/Argumentum Fallacies - Cards - edition fevrier 2022.csv` | **70** | **le tirage imprimé** — autorité maximale |
| `Archive/v3/Argumentum Fallacies - Cards.csv` | **169** | le deck pré-agentique, fr/en/ru/pt |

Contrôle décisif : la carte que l'owner a physiquement en main s'y lit **mot pour mot**.

| Champ | `Archive/v3`, `path` 2.1 | Corpus courant |
|---|---|---|
| `text_fr` | Procédé rhétorique | Technique rhétorique |
| `desc_fr` | **Vous faites preuve d'une éloquence persuasive.** | Vous utilisez des formules séduisantes et un style persuasif pour convaincre, sans nécessairement fonder votre propos sur un raisonnement logique. |
| `example_fr` | *« Si votre ramage se rapporte à votre plumage… »* — **Jean de La Fontaine** | Votre éloquence est à la hauteur de votre prestance : vous êtes vraiment exceptionnel. |

### ⚠️ Le PK n'est pas une clé — `path` l'est

Cette carte est **PK 156** dans la v3 et **PK 176** aujourd'hui. Mesuré sur les cartes imprimées :

| Clé candidate | Recouvrement v3 ∩ courant | Verdict |
|---|---:|---|
| `PK` | 29 / 169 | ⛔ inutilisable |
| **`path`** | **153 / 169 (90,5 %)** | ✅ retenue |
| `text_fr` | 118 / 169 | — |

⭐ **L'écart entre les deux dernières lignes est lui-même une mesure** : 153 cartes appariées dont 118 gardent leur titre ⇒ **35 titres de cartes imprimées ont changé**.

### ⚠️ Trois couches, pas deux

Le contrôle inverse ci-dessous mesure **3 désaccords entre 2022 et v3** : la v3 est donc **déjà** une révision de l'imprimé — elle y introduit même une faute (`perspectives individuels`). ⇒ L'instrument prend **2022 en priorité** (63 cartes) et **v3 en repli** (90 cartes). Pour ces 90, la référence est *une couche en retrait* de l'objet imprimé ; c'est une limite, elle est signalée colonne « réf. ».

---

## 2. Contrôles inverses

⭐ *Un instrument qui ne peut pas rendre « rien à signaler » ne prouve rien quand il signale.*

| # | Contrôle | Résultat |
|---|---|---|
| 1 | 2022 vs v3 sur 69 `path` communs (207 cellules) | **3 désaccords** ⇒ référence stable à 98,6 %, et les 3 sont **documentés** ci-dessus |
| 2 | Le témoin connu (`path` 2.1, le cas de l'owner) est-il détecté ? | **OUI** — 3 cellules, dont `example_fr` en `SUBST` |
| 3 | L'instrument rend-il *intact* là où rien n'a bougé ? | **OUI — 12 cartes** rendues sans aucune cellule signalée |
| **4** | **La jointure `path` apparie-t-elle bien les MÊMES cartes ?** | **OUI — `Famille` identique sur 153/153 (100 %)** |

⭐ **Le contrôle 4 est le plus important, et il a failli manquer.** `Famille` est un invariant qui **survit à une réécriture mais pas à une re-clé** : si les `path` avaient été réattribués, les familles se mélangeraient. 100 % ⇒ la jointure apparie les mêmes cartes, et les 36 titres divergents sont une **mesure**, ⛔ pas un artefact.

⚠️ **Ce contrôle échoue sur Scenarii** — voir §8. Il n'est donc pas décoratif : il **sépare** un corpus mesurable d'un corpus qui ne l'est pas avec cette clé.

---

## 3. Périmètre et ventilation

| | |
|---|---:|
| Cartes imprimées courantes | **175** |
| Appariées sur `path` (= cartes **existantes**) | **153** — dont 63 contre l'imprimé 2022, 90 contre v3 |
| Non appariées ⇒ **cartes nouvelles**, hors périmètre de ce registre | **22** |
| Cartes appariées **totalement intactes** | **12** |

### Cellules changées — 3 champs FR, cartes existantes uniquement

| champ | C | AJOUT | SUBST | total |
|---|---:|---:|---:|---:|
| `text_fr` | 3 | 1 | **36** | 40 |
| `desc_fr` | 0 | 0 | **135** | 135 |
| `example_fr` | 5 | 0 | **111** | 116 |
| **TOTAL** | **8** | **1** | **282** | **291** |

| Classe | Définition **mécanique** | Traitement proposé |
|---|---|---|
| **C** | ne diffère que par accents / casse / ponctuation | ⛔ pas d'arbitrage — garder |
| **AJOUT** | la référence est **incluse telle quelle** dans le texte actuel | garder — *c'est la forme que l'owner décrit comme acceptable* |
| **SUBST** | reformulation ou remplacement | ⇒ **surface d'arbitrage** |

> ⭐ **Le chiffre qui répond au nit de l'owner.** Sur 291 changements appliqués à des cartes déjà imprimées et jouées, **exactement 1** a pris la forme *« garder le texte existant et lui ajouter la précision »* — le geste qu'il décrit comme celui qu'il n'aurait « pas eu à se battre pour faire accepter ». Les **282** autres remplacent.

---

## 4. Attribution — qui a écrit ces changements

277 des 291 cellules sont attribuées au commit qui a **introduit leur valeur courante** (balayage des 92 états committés du CSV). Les **14 restantes** ne le sont pas : leur valeur courante n'apparaît dans aucun état intermédiaire parsable — ⚠️ absence d'attribution, ⛔ pas absence de changement.

| cellules | date | commit | PR | sujet | commentaires |
|---:|---|---|---|---|---:|
| **220** | 2026-05-28 | `9d45b4f9` | **#369** | data(fallacies): FR clarity + cascade drift infrastructure | **0** |
| 48 | 2025-07-02 | `e8482fe5` | — | ré-addition du CSV (**ère manuelle**) | — |
| 4 | 2023-06-14 | `7c575cdf` | — | ère manuelle | — |
| 1 | 2026-09-11 | `930ae523` | #1331 | PK 511 renommage 8 langues (GO #1294) | 1 |
| 1 | 2026-09-02 | `3eb08fc6` | #1265 | #994 exemples composites PK 340/598 (GO 02/09) | 0 |
| 1 | 2026-08-08 | `1e642cff` | #1029 | #994 (a) coquille `A`→`À` PK 1361 | 0 |
| 1 | 2026-05-24 | `42915d0e` | — | manual taxonomy-distinction rework | — |
| 1 | 2026-05-24 | `d21199a5` | — | simplify 27 desc_fr for younger audience | — |

### ⚠️ Deux lectures que ce tableau impose

**(a) 52 cellules sont antérieures à l'agentique.** Les 48 de `e8482fe5` (2025-07) et les 4 de 2023 relèvent de la **dérive manuelle / GSheet**, ⛔ pas du travail d'agent. Les imputer à l'IA serait faux — et se retournerait à la relecture.

**(b) 220 sur 277 (79 %) viennent d'une seule PR, et le défaut n'est pas son volume.** #369 **déclare honnêtement** son ampleur dans son propre corps :

> *« **955/1408 PKs modified (67.8 %)**, **1651/4224 cells changed (39.1 %)** — `desc_fr` 700/1408 · `example_fr` 744/1408 · `text_fr` 207/1408 — **minor name refinements only** »*

Ce qui manque n'est pas la transparence, ce sont **deux autres choses** :

1. ⛔ **La qualification est fausse là où elle décide.** *« minor name refinements only »* décrit **36 titres de cartes imprimées** réécrits, dont des renommages de fond. ⭐ **Le volume était honnête, la nature ne l'était pas — et c'est la nature qui déclenche un arbitrage.**
2. ⛔ **La preuve est hors d'échelle.** Sa section *Quality check* énonce : *« Sampled paths 1.1.1, 3.2.1 »* — **2 chemins** pour une campagne de 955 PK.

⭐ **Le contraste est la conclusion.** Les PR #1029 / #1265 / #1331 ont produit **1 cellule chacune**, avec issue nommée, GO cité verbatim, oracle indépendant, contrôle de non-recouvrement et diff borné. *Le processus qui a produit 1 cellule porte plus de preuve que celui qui en a produit 220.* Le défaut de gouvernance est là, ⛔ pas dans le nombre.

---

## 5. Ce que ce registre **n'établit pas**

- ⛔ **Il ne dit pas qu'un `SUBST` est mauvais.** `SUBST` est une mesure de **forme** : le texte a été remplacé. Beaucoup sont probablement des améliorations. Le tri est éditorial.
- ⛔ **Il ne couvre que le FR**, 3 champs, 153 cartes. Les 7 autres langues ont été **cascadées depuis le FR dérivé** : un retour FR **sans** re-cascade laisserait les traductions alignées sur une version abandonnée.
- ⛔ **Il ne couvre pas Scenarii / Rules / Virtues.** Même méthode applicable si une archive équivalente existe — **à vérifier, pas à supposer**.
- ⚠️ **Pour 90 des 153 cartes, la référence est la v3, pas l'imprimé.** Une couche de dérive y reste invisible.
- ⚠️ **La perte d'attribution de citation est mesurée 1 sur 1.** Le corpus de référence ne contient **qu'une seule** citation d'auteur nommé — celle de La Fontaine — et elle a été détruite. La perte est totale, mais la base est de un : ⛔ **ce n'est pas un taux**, et on ne peut rien en généraliser.

---

## 6. Le tableau

Trié par **ampleur du remplacement** (distance d'édition normalisée, décroissante) : les réécritures les plus lourdes viennent en premier, la queue est du détail. ⭐ **On peut s'arrêter de lire où on veut.**

Colonne **réf.** : `2022` = comparé au tirage imprimé · `v3` = comparé au deck pré-agentique.

### 6.1 — `SUBST` : la surface d'arbitrage (282 cellules)

| # | `path` | PK | réf. | champ | ampl. | PR | **Référence** | **Aujourd'hui** |
|---:|---|---:|:-:|---|--:|:-:|---|---|
| 1 | `4.3.3.3.1` | 796 | v3 | `example_fr` | 99% | — | Socrate est un homme ; les hommes sont mortels ; les chats sont mortels ; donc Socrate est un chat. | Tous les avocats défendent des clients au tribunal. Ce fruit est un avocat. Donc ce fruit défend des clients au tribunal. - " avocat " change de sens : profession juridique dans la première prémisse, fruit dans la seconde. Le raisonnement contient donc en réalité quatre termes au lieu de trois. |
| 2 | `5.2.1` | 834 | **2022** | `example_fr` | 98% | — | Les lycéens sont comme une meute de chiens féroces : il convient de leur montrer qui est le maître. | Gérer une classe, c'est comme piloter une machine : si une pièce ralentit le système, il suffit de la remplacer. - Comparaison abusive : une classe n'est pas une machine, et les élèves ne sont pas des pièces interchangeables. La comparaison efface des différences essentielles. |
| 3 | `2.2.2` | 322 | v3 | `example_fr` | 97% | — | Ce n'est tout de même pas une poignée de jeunes imbéciles qui va vous apprendre votre métier ! | Les collapsologues veulent nous ramener à l'âge de pierre et abolir toute industrie. - Repoussoir : caricature volontairement excessive d'un mouvement réel pour rendre sa réfutation plus facile et plus applaudie. |
| 4 | `1.1.2` | 33 | v3 | `example_fr` | 97% | — | - Si les extraterrestres existaient, on en aurait vus ! - On ne les voit pas parce qu'ils frappent d'amnésie tous ceux qui les aperçoivent. | Je conviens que votre calcul est correct. Mais votre raisonnement est insignifiant : ce détail ne change rien au problème de fond. - Rejet d'un argument correct sur la base de son importance perçue, pas de sa validité. |
| 5 | `2.1.3` | 247 | v3 | `example_fr` | 96% | #369 | Paris ! Paris outragé ! Paris brisé ! Paris martyrisé ! Mais Paris libéré ! | Dans la vallée, où chaque fleur exhale un soupir d'amour, notre candidat est le seul qui, grâce à sa longue expérience, connaisse le secret des mots capables de convaincre l'État de préserver ce morceau de paradis. |
| 6 | `3.2.3` | 658 | **2022** | `example_fr` | 96% | — | Admettons que Dieu ait créé l'homme ; dans ce cas, qui a créé Dieu ? | Cette affirmation est vraie. - Mais comment le savez-vous ? Je l'ai vérifiée. - Mais comment avez-vous vérifié cette vérification ? Et la vérification de cette vérification, comment l'avez-vous vérifiée ? … - Régression à l'infini de la justification épistémique : chaque niveau de preuve en exige un autre. |
| 7 | `2.3.2.2.1` | 432 | v3 | `example_fr` | 94% | — | Puisque les bonnes ménagères que vous êtes avez été convaincues par nos recettes alléchantes et leurs bienfaits diététiques, que celles qui vont cuisiner des abats à leur mari la semaine prochaine lèvent la main. | Puisque vous êtes favorables à la transition écologique, que ceux d'entre vous qui accepteront de prendre le train plutôt que l'avion pour leurs prochains déplacements lèvent la main. - Lewin (1947) : l'engagement public active l'acceptation privée, indépendamment du sujet. |
| 8 | `2.2.1.2.1` | 304 | v3 | `example_fr` | 91% | #369 | Cela me ferait tellement plaisir d'avoir un compagnon de balade que ce chien finira par m'obéir, j'en suis sûr. | Je suis convaincu que mon dispositif de téléportation fonctionnera un jour. Après tout, il faut croire en l'avenir ! |
| 9 | `1.1` | 2 | v3 | `text_fr` | 89% | #369 | Argument bâclé | Généralisation hâtive |
| 10 | `2.3.1.1.1.2` | 361 | v3 | `text_fr` | 85% | #369 | Leurre | Appât et substitution |
| 11 | `4.2.3` | 750 | **2022** | `desc_fr` | 85% | — | Votre argumentation nuance mal les modalités du vrai. | Vous confondez ce qui est possible, nécessaire, certain ou obligatoire, et cela fausse votre raisonnement. |
| 12 | `4.3.3.1.1.1` | 787 | v3 | `example_fr` | 85% | — | Il nous faut faire quelque chose. Ce que je propose, c'est quelque chose. Donc c'est ce que nous devons faire. | Pour qu'elle s'améliore, la situation doit changer. Nous changeons la situation, donc nous améliorons la situation. |
| 13 | `7.3.2.3.2` | 1388 | v3 | `example_fr` | 84% | #369 | Si tu refuses de parier ta maison pour soutenir qu'il faut tourner à droite à ce carrefour, c'est que tu sais que j'ai raison de vouloir aller à gauche. | Vous semblez nerveux en présentant votre projet ; vous n'y croyez peut-être pas vous-même ? |
| 14 | `1.1.2` | 33 | v3 | `text_fr` | 84% | — | Sauvetage ad hoc | Justification triviale |
| 15 | `2.2.3` | 340 | v3 | `example_fr` | 84% | #1265 | Je ne donne jamais d'argent aux mendiants car je crains qu'ils n'en profitent pour acheter de l'alcool. | Admettons que ce diagnostic soit exact : il faudrait tout arrêter, prévenir les familles, et reconnaître publiquement notre erreur. Vous voyez bien que ce n'est pas possible. |
| 16 | `2.3.1` | 357 | v3 | `example_fr` | 83% | #369 | Donner le pouvoir à un banquier, c'est comme guérir un alcoolique avec de l'alcool. | Quand vous voyez notre logo, vous pensez aussitôt à la liberté et à l'aventure. C'est ce qui rend notre marque si spéciale pour nos clients. |
| 17 | `3.2` | 632 | v3 | `example_fr` | 83% | #369 | Les apparitions de Nicolas Cage au cinéma ces dix dernières années coïncident avec des pics de noyades en piscine. | La courbe du réchauffement climatique suit de très près la baisse de la natalité. Le lien entre les deux est donc avéré. |
| 18 | `5.2.1` | 834 | **2022** | `desc_fr` | 83% | #369 | Votre argumentation repose sur une mise en parallèle impropre de plusieurs propositions. | Vous utilisez une comparaison excessive ou inappropriée pour appuyer votre argument. |
| 19 | `2.3` | 356 | v3 | `example_fr` | 81% | #369 | Pour des raisons de sécurité, veuillez nous communiquer l'identifiant et le mot de passe de votre compte. | N'oubliez pas : il y a ceux qui ont le privilège de choisir et ceux qui subissent les choix des autres. Agissez en conséquence. |
| 20 | `6.2` | 973 | **2022** | `example_fr` | 81% | #369 | Ce qui me tenait le plus à cœur n'était pas de gagner ce match mais d'offrir un beau spectacle au public. | Vous aviez demandé une étude publiée pour être convaincu. Lorsqu'on vous en présente une, vous exigez soudain trois études indépendantes et une méta-analyse. |
| 21 | `6.3.1` | 1024 | v3 | `text_fr` | 81% | — | Anthropocentrisme | Biais naturels |
| 22 | `3.2.1.3` | 636 | v3 | `example_fr` | 81% | — | Il y a peu, au casino, j'ai gagné trois fois le jackpot ; mais depuis que j'ai bu un jus de tomate, je perds sans discontinuer : maudit jus de tomate ! | Comme j'étais très malade, j'ai prié et cela m'a guéri. |
| 23 | `5.1.1.2` | 802 | **2022** | `desc_fr` | 79% | #369 | Vous vous appuyez sur l'impossibilité de définir certains termes de votre argumentation. | Votre argument repose sur un concept crucial qui n'est pas clairement défini. |
| 24 | `6.3.2` | 1174 | v3 | `text_fr` | 79% | — | Ethnocentrisme | Biais culturels |
| 25 | `2.3.1` | 357 | v3 | `desc_fr` | 79% | #369 | Vous influez sur votre auditoire pour qu'il réagisse d'une manière particulière à un stimulus donné. | Vous associez des idées à des émotions afin d'influencer les comportements, sans que cette influence soit clairement perçue. |
| 26 | `7.3.1.2` | 1365 | **2022** | `example_fr` | 78% | #369 | La cuisine bretonne, ce n'est que du beurre. Je ne mange pas du beurre à la petite cuillère, pas de cuisine bretonne chez moi. | Ah, donc selon vous, nous devrions passer toutes nos journées à fouiller dans les poubelles pour trier les déchets ? Ce n'est pas une vie ! |
| 27 | `3.1` | 595 | v3 | `desc_fr` | 78% | #369 | Vous élargissez exagérément la portée de votre raisonnement. | Vous formulez une généralisation qui n'est pas valable dans le cas considéré. |
| 28 | `2.1.1.1.1` | 179 | **2022** | `desc_fr` | 77% | — | Vous interrogez votre contradicteur en mentionnant des faits dont vous souhaitez que l'auditoire les tienne pour acquis. | Vous posez une question qui suppose subtilement une prémisse fausse ou controversée. |
| 29 | `3.2.2` | 644 | v3 | `desc_fr` | 77% | — | Votre argumentation s'appuie sur une utilisation erronée des lois mathématiques du calcul de l'aléatoire. | Vous utilisez mal les probabilités pour appuyer votre argument, ce qui fausse votre raisonnement. |
| 30 | `7.2.1` | 1313 | **2022** | `desc_fr` | 77% | #369 | Vous détournez le débat de son objet premier afin d'échapper aux arguments de votre contradicteur. | Vous utilisez un procédé qui vous permet d'éviter de répondre sur le fond. |
| 31 | `7.1.3` | 1297 | v3 | `example_fr` | 77% | #369 | Vous pouvez me croire sur parole : en deux semaines, j'enseignerai la physique quantique à votre enfant. | Il est évident que notre futur produit sera un succès mondial ; nul besoin d'étude de marché. |
| 32 | `7.2.2` | 1345 | v3 | `example_fr` | 77% | — | Ce soir-là, Il pleuvait ; or, l'eau contient des atomes d'oxygène et d'hydrogène dont les électrons se comportent de manière quantique ; êtes-vous donc certain que votre radar électromagnétique n'ait pas momentanément dysfonctionné ? | Je ne vois pas comment on pourrait discuter de ce sujet, il y a de toute façon beaucoup trop de variables à prendre en compte. |
| 33 | `4.1.2.5` | 713 | v3 | `text_fr` | 76% | #369 | Sophisme de la double faute | Deux torts font un droit |
| 34 | `7.3` | 1360 | v3 | `example_fr` | 76% | — | Je n'ai pas triché: il dit ça pour prendre ma place. | Comment pouvez-vous prétendre que votre politique de santé fonctionnera quand vous n'avez même pas réussi à arrêter de fumer ? |
| 35 | `7.3.3` | 1398 | **2022** | `desc_fr` | 76% | #369 | Consiste à attaquer l'adversaire personnellement, sans rapport avec le fond du débat, pour le discréditer afin de discréditer ses arguments du même coup. | Au lieu de réfuter les arguments, vous attaquez directement la personne qui les présente. |
| 36 | `7.2.1` | 1313 | **2022** | `example_fr` | 76% | #369 | Avant de me reprocher mon retard, peux-tu m'expliquer pourquoi tu te promènes dans cette tenue ? | C'est une question intéressante, mais examinons d'abord le contexte plus large… |
| 37 | `5.2.2` | 839 | **2022** | `example_fr` | 76% | #369 | Les cerneaux de noix ressemblent à de petits cerveaux ; un noyer doit donc faire preuve d'une grande intelligence collective. | Personne n'empêche un chirurgien de consulter ses notes pendant une intervention difficile. Alors pourquoi voudriez-vous m'en priver pendant cet examen ? |
| 38 | `1.1.3` | 55 | v3 | `example_fr` | 75% | #369 | Il est inutile que je poursuive mes études car l'avenir est incertain. | Si on ne voit pas les extraterrestres, c'est parce qu'ils rendent amnésiques tous ceux qui les aperçoivent. |
| 39 | `7.3.3` | 1398 | **2022** | `example_fr` | 75% | #369 | Vous ne pouvez prétendre que la Terre tourne autour du Soleil, vous n'êtes même pas astronome ! | Ne me dites pas comment hacher l'ail. Un toxicomane ne va pas m'apprendre à me nourrir. |
| 40 | `5.1.1` | 800 | v3 | `example_fr` | 74% | #369 | Ma liberté d'expression m'autorise à menacer cet homme. | Quand j'ai dit que je serais là dans cinq minutes, cela pouvait aussi vouloir dire dans une heure. C'est une expression : il ne fallait pas la prendre au pied de la lettre. |
| 41 | `4.1.2.5` | 713 | v3 | `example_fr` | 74% | #369 | - Pourquoi refuses-tu de me rendre ma trottinette ? - Parce que tu m'as menti. | Le patron ne paie pas ses impôts, alors je ne vais pas me gêner pour me servir dans la caisse. |
| 42 | `4.3` | 758 | v3 | `desc_fr` | 74% | #369 | Vos hypothèses de départ ne permettent pas de construire un raisonnement qui justifie vos conclusions. | Vous tirez des conclusions qui ne découlent pas logiquement de vos prémisses. |
| 43 | `2.1.1.4` | 185 | **2022** | `example_fr` | 74% | #369 | Ne cherche pas plus loin: c'est la vie avec ses joies et ses peines. | Nous devons aussi appliquer une politique de tolérance zéro aux enfants : qui vole un œuf vole un bœuf. |
| 44 | `1.2.3` | 112 | v3 | `example_fr` | 74% | #369 | Il est choquant de voir un couple d'hommes ou de femmes s'embrasser dans la rue : l'homosexualité est totalement contre nature. | Ce sont des enfants innocents. Ils ne pourraient pas causer autant de problèmes à leurs parents. |
| 45 | `4.2.2.2` | 740 | v3 | `desc_fr` | 74% | #369 | Votre argument fait une implication erronnée à partir d'une proposition quantifiée. | À partir d'une seule prémisse, vous tirez directement une conclusion incorrecte en lui attribuant des propriétés ou des implications qu'elle ne soutient pas logiquement. |
| 46 | `5.1.1.2` | 802 | **2022** | `example_fr` | 73% | — | Ce foulard n'est pas violet, il est mauve. | Le concept de " qualité " est si vaste qu'il est difficile à définir précisément ; mon argument repose donc sur mon interprétation personnelle. |
| 47 | `1.1.3` | 55 | v3 | `text_fr` | 72% | — | Argument vide | Sauvetage ad hoc |
| 48 | `5.3.2` | 855 | v3 | `text_fr` | 72% | #369 | Ambiguïté sémantique | Équivoque |
| 49 | `3.2.3.1` | 659 | **2022** | `desc_fr` | 72% | #369 | Vous exigez que certaines des notions énoncées soient clairement bornées alors que celles-ci peuvent être de nature progressive. | Vous niez le caractère continu ou progressif d'un phénomène et exigez des limites strictes là où il n'y en a pas. |
| 50 | `5.2.3.1` | 845 | **2022** | `example_fr` | 71% | #369 | Son prénom est Yannick : il doit être breton ; aucune chance donc qu'il n'étale du beurre doux sur ses tartines ! | Vous êtes végétarien ? Nous n'avons pas besoin de militant écologiste ici. |
| 51 | `6.3` | 1023 | v3 | `desc_fr` | 71% | #369 | Votre perception de la réalité est déformée par votre propre subjectivité. | Vos biais orientent vos arguments, ce qui vous empêche de juger la situation de manière équilibrée. |
| 52 | `6.1.1.2` | 908 | v3 | `example_fr` | 71% | — | Les élites dirigeatntes ont voulu toutes ces calamités pour mieux contrôler les masses. | On sait tous que les pyramides ont été construites par des esclaves. - Largement cru, historiquement faux : les travailleurs de Gizeh étaient des ouvriers rémunérés, logés et nourris sur le site. |
| 53 | `3.3.1.3.1` | 677 | **2022** | `desc_fr` | 70% | #369 | Vous attribuez à un événement anodin la valeur de déclencheur inévitable d'une série de réactions aboutissant à une conclusion catastrophique. | Vous rejetez une proposition en prédisant une série peu probable d'événements négatifs. |
| 54 | `3.2.1.4.1` | 638 | v3 | `desc_fr` | 70% | #369 | Vous prétendez déduire une cause commune à une série d'événements fortuits possédant des caractéristiques similaires. | Vous repérez une tendance après coup et vous inventez une cause pour faire correspondre le résultat à vos convictions. |
| 55 | `4.2.1` | 727 | v3 | `desc_fr` | 69% | — | Votre raisonnement repose sur une mauvaise articulation d'éléments dont vous considérez qu'ils ne peuvent être qu'entièrement vrais ou entièrement faux. | Vous utilisez incorrectement des propositions logiques élémentaires reliées par " et ", " ou " ou " si… alors ", ce qui conduit à des conclusions fausses. |
| 56 | `6.3.3` | 1242 | **2022** | `text_fr` | 69% | — | Dogmatisme | Biais théoriques |
| 57 | `1.3.1` | 134 | v3 | `example_fr` | 69% | #369 | Il s'est beaucoup entraîné sur l'ordinateur, il ne peut pas rater son permis de conduire. | Gérer une entreprise n'est pas si différent d'un jeu vidéo de stratégie : il suffit de choisir les bonnes options et la croissance suivra d'elle-même. |
| 58 | `3.2.3` | 658 | **2022** | `desc_fr` | 69% | #369 | Vous exigez que soit prouvée la véracité de la moindre des propositions avancées par votre contradicteur. | Votre argument repose sur une mauvaise compréhension de la notion d'infini. |
| 59 | `6.1` | 888 | v3 | `example_fr` | 69% | #369 | Je ne suis pas responsable de cette situation : je t'ai dit ce que je savais et tu as pris la mauvaise décision. | Je ne suis pas responsable de votre décision : je vous ai dit que le produit avait réussi certains tests, mais j'ai omis de préciser qu'il en avait échoué d'autres. |
| 60 | `2.1` | 176 | v3 | `desc_fr` | 69% | #369 | Vous faites preuve d'une éloquence persuasive. | Vous utilisez des formules séduisantes et un style persuasif pour convaincre, sans nécessairement fonder votre propos sur un raisonnement logique. |
| 61 | `5.3.1` | 847 | v3 | `text_fr` | 69% | — | Ambiguïté syntaxique | Amphibologie |
| 62 | `4.3.3` | 784 | **2022** | `desc_fr` | 69% | #369 | Vous construisez un discours en trois étapes - si, or, donc - sans prêter attention à la validité du raisonnement. | Vous construisez de façon incorrecte un raisonnement logique en trois parties : prémisse majeure, prémisse mineure et conclusion. |
| 63 | `3.2.1` | 633 | v3 | `example_fr` | 69% | — | Comme j'étais très malade, j'ai prié et cela m'a guéri. | La courbe de la productivité suit clairement celle de la consommation de café. Il doit en être la cause. |
| 64 | `6.2.3` | 1011 | v3 | `desc_fr` | 69% | #369 | Vous réduisez vos prétentions à mesure que votre argumentation se heurte à des difficultés. | Vous abaissez vos exigences pour défendre une position difficilement défendable, ce qui dénature le débat. |
| 65 | `3.2.3` | 658 | **2022** | `text_fr` | 68% | #369 | Justification infinie | Infini fallacieux |
| 66 | `7.2.1` | 1313 | **2022** | `text_fr` | 68% | #369 | Fausse piste | Évasion |
| 67 | `4.1.2.5` | 713 | v3 | `desc_fr` | 68% | — | Vous considérez que l'incohérence des actes ou des propositions antérieures de votre interlocuteur disqualifie ses arguments présent, sans qu'il soit besoin de les réfuter. | Vous croyez à tort que deux erreurs combinées peuvent produire un résultat juste. |
| 68 | `7.1.2` | 1287 | v3 | `example_fr` | 68% | #369 | Pourquoi la mer est bleue ? Parce que les dauphins pleurent. | Peu importe vos arguments : la vraie explication, c'est que nous n'avons pas besoin de ce produit. Tout le monde le sait. |
| 69 | `1.1.3` | 55 | v3 | `desc_fr` | 68% | #369 | Les affirmations qui composent votre raisonnement ne démontrent rien. | Face à des objections valables, vous improvisez des justifications sans fondement pour échapper à la critique. |
| 70 | `7.3.1` | 1361 | v3 | `desc_fr` | 68% | #369 | Vous accusez votre contradicteur d'avoir changé d'opinion au fil du temps. | Vous relevez les incohérences ou les contradictions de votre interlocuteur, ou vous lui opposez des arguments erronés qui peuvent néanmoins lui sembler valides. |
| 71 | `1.1.1` | 3 | v3 | `example_fr` | 68% | #369 | J'ai entendu dire que cette personne était sexiste ; il m'est donc impossible d'être en accord avec elle sur quoi que ce soit. | Il est inutile que je poursuive mes études, car l'avenir est incertain. |
| 72 | `1.2.3` | 112 | v3 | `desc_fr` | 67% | #369 | Vous considérez comme naturel ce que vous approuvez et comme contre nature ce que vous réprouvez. | Vous jugez que certains faits sont vrais ou faux en fonction de vos principes moraux, et non sur la base d'une démonstration objective. |
| 73 | `7.3.1` | 1361 | v3 | `example_fr` | 67% | #1029 | Bien que déplorant la surpopulation mondiale, te voilà père de deux enfants ! | À t'entendre, tout achat est immoral. Pourtant je t'ai vu faire les soldes l'autre jour, et ma morale le tolère. |
| 74 | `4.2.3.1.1` | 752 | v3 | `desc_fr` | 67% | — | Votre argument identifie les propriétés d'objets qui font l'objet de connaissances distinctes. | Vous supposez à tort que des connaissances différentes sur des objets correspondent à des propriétés différentes de ces objets. |
| 75 | `5.2.3` | 844 | v3 | `example_fr` | 67% | — | Ce garçon est ami avec les élèves les plus bavards de la classe ; il est donc nécessaire de le garder à l'œil. | Jeanne est bonne en mathématiques. Jeanne est dyslexique. Donc tous les dyslexiques sont bons en mathématiques. |
| 76 | `3.2.2.3.1` | 653 | **2022** | `example_fr` | 66% | #369 | Mieux vaut ne rien entreprendre : rien ne me réussit ces temps-ci ! | J'ai marqué mes trois derniers paniers : je suis en réussite, donc je vais forcément réussir le prochain tir. |
| 77 | `1.2.2.3` | 104 | **2022** | `desc_fr` | 66% | — | Vous établissez la validité d'un discours ou d'une pratique par le fait qu'ils auraient été consacrés par l'usage dans le temps. | Vous considérez une pratique comme correcte simplement parce qu'elle est traditionnelle. |
| 78 | `1.1.1` | 3 | v3 | `text_fr` | 66% | — | Justification triviale | Argument vide |
| 79 | `7.1.3` | 1297 | v3 | `desc_fr` | 66% | #369 | Vous affirmez une proposition de manière suffisamment forte et répétée pour lui donner une apparence de solidité. | Vous répétez votre point de vue avec insistance au lieu de fournir de véritables preuves ou arguments. |
| 80 | `4.1` | 697 | v3 | `desc_fr` | 65% | #369 | Vous affirmez qu'un fait en a provoqué un autre en établissant un enchaînement très discutable. | Vous supposez un lien de cause à effet qui n'est pas démontré ou qui est incorrect. |
| 81 | `6.2.1.4` | 989 | **2022** | `example_fr` | 65% | #369 | Si vous ne pouvez pas me prouver que le monstre du Loch Ness n'existe pas, c'est qu'il existe. | Moi, prouver que mon produit est sans danger ? C'est plutôt à vous de montrer qu'il est nocif ! |
| 82 | `2.3.1.1.1.2` | 361 | v3 | `desc_fr` | 65% | #369 | Vous introduisez un propos discutable par des déclarations consensuelles. | Vous commencez par des affirmations que tout le monde accepte, puis vous glissez subtilement vers vos idées contestables. |
| 83 | `2.2.1` | 300 | v3 | `example_fr` | 64% | #369 | Nous autres connaissons, n'est-ce pas, la valeur des employés qui se lèvent tôt. | En tant que parents, nous comprenons l'importance de protéger nos enfants, n'est-ce pas ? |
| 84 | `7.3.1.1` | 1362 | **2022** | `desc_fr` | 64% | #369 | Vous disqualifiez votre adversaire l'accusant d'avoir agi à l'encontre de son argumentation. | Vous rejetez ce que dit quelqu'un en affirmant qu'il n'agit pas toujours conformément à ses propres principes. |
| 85 | `4.2` | 726 | v3 | `desc_fr` | 64% | #369 | Votre raisonnement repose sur un enchaînement erroné de propositions logiques. | Vous combinez des propositions logiques de façon incorrecte, ce qui conduit à un raisonnement erroné. |
| 86 | `2.1.2` | 219 | v3 | `desc_fr` | 64% | #369 | Vous utilisez des ressorts humoristiques pour détourner l'attention, atténuer l'effet de vos propos ou emporter la bienveillance de l'auditoire. | Vous recourez à l'humour pour rendre votre argumentation plus sympathique, sans vous appuyer sur des faits. |
| 87 | `7.3.2.3.2` | 1388 | v3 | `desc_fr` | 64% | #369 | Vous rejetez une proposition sous prétexte que son auteur manque d'assurance. | Vous discréditez les idées de quelqu'un en mettant en avant son manque de confiance en soi, plutôt qu'en contestant son argumentation. |
| 88 | `6.1.2.1` | 943 | **2022** | `desc_fr` | 64% | #369 | Vous citez une phrase sans en préciser les circonstances utiles, de manière à en modifier la portée. | Vous détournez des citations de leur sens d'origine en les isolant de leur contexte, afin de les faire paraître favorables à votre propos. |
| 89 | `6.2.3.3` | 1015 | v3 | `example_fr` | 63% | — | Cette nouvelle organisation aura nécessité la cohésion de nos équipes autour de cet épineux problème | Peut-être que mon invention ne fonctionne pas encore, mais vous ne pouvez nier que j'y ai travaillé jour et nuit pendant des mois. |
| 90 | `7.3.2.1.1` | 1373 | v3 | `example_fr` | 63% | — | Adolf Hitler n'en aurait pas fait autant ! | Votre argument sur la régulation est exactement ce qu'Hitler aurait dit ! |
| 91 | `4.1.1` | 698 | v3 | `example_fr` | 63% | — | La peine de mort est un châtiment juste car certains crimes méritent d'être ainsi punis. | Mieux vaut avoir un chat plutôt qu'un chien, car les chats sont de meilleurs animaux de compagnie. |
| 92 | `5.2.2` | 839 | **2022** | `desc_fr` | 63% | — | Vous supposez des qualités communes à plusieurs éléments sous prétexte qu'ils présentent un aspect commun. | Vous établissez un parallèle entre deux choses en vous basant seulement sur un point commun, négligeant leurs différences importantes. |
| 93 | `7.3` | 1360 | v3 | `desc_fr` | 63% | — | Vous confrontez l'adversaire directement dans sa personne en lui opposant ses propres paroles, actes, intentions ou fréquentations. | Vous visez votre adversaire lui-même plutôt que de contester ses arguments. |
| 94 | `1.1.2` | 33 | v3 | `desc_fr` | 63% | #369 | Vous répondez à de justes critiques de vos arguments par des explications sans fondement qui rendent la proposition de plus en plus irréfutable. | Vous accordez à une habitude, à une impression ou à un exemple la valeur d'une preuve. |
| 95 | `7.2.3` | 1352 | **2022** | `desc_fr` | 62% | #369 | Vous exposez des informations visant à jeter le discrédit sur le débat ou sur des arguments. | Vous discréditez l'autre partie au moyen d'informations préjudiciables afin d'affaiblir sa position. |
| 96 | `6.2.3.3` | 1015 | v3 | `desc_fr` | 62% | #369 | Vous prétendez que votre démarche vaut au moins pour l'effort qui lui est consacré. | Vous estimez qu'une idée mérite d'être soutenue uniquement parce que quelqu'un y a beaucoup travaillé. |
| 97 | `1.2` | 70 | v3 | `desc_fr` | 62% | #369 | Votre thèse repose sur des opinions préconçues. | Vous fondez vos arguments sur des idées reçues, sans les examiner de façon critique. |
| 98 | `6.1.1` | 889 | **2022** | `example_fr` | 62% | — | Bien sûr que le père Noël existe ! | Le Père Noël vous surveille, alors comportez-vous bien. |
| 99 | `1.3.1` | 134 | v3 | `desc_fr` | 62% | #369 | Vous raisonnez sur la modélisation simpliste d'une réalité complexe, comme s'il s'agissait d'un jeu. | Vous abordez des questions complexes comme s'il s'agissait de simples jeux ou de problèmes faciles à résoudre, souvent à l'aide de modèles simplistes qui négligent leur complexité réelle. |
| 100 | `2.2.1.2.1` | 304 | v3 | `desc_fr` | 62% | #369 | En dépit de son invraisemblance, vous soutenez la conclusion qui correspond à ce que vous souhaitez. | Vous tenez une idée pour vraie uniquement parce que vous aimeriez qu'elle le soit. |
| 101 | `4.2.2` | 735 | **2022** | `desc_fr` | 61% | — | Vous utilisez à mauvais escient les déterminants indéfinis tels tous, certains, aucun, etc. | Vous commettez une erreur dans l'emploi de quantificateurs comme " tous ", " aucun " ou " quelques ", ce qui fausse l'argument. |
| 102 | `1.2.1` | 71 | **2022** | `desc_fr` | 61% | — | Vous justifiez votre propos par l'expertise de personnes influentes qui le soutiennent, plutôt que par son contenu. | Vous croyez qu'une affirmation est vraie uniquement parce qu'une autorité la soutient, sans examiner les faits. |
| 103 | `2.2` | 299 | **2022** | `desc_fr` | 61% | #369 | Vous provoquez un trouble affectif propre à influencer le jugement. | Vous suscitez de fortes émotions pour détourner votre auditoire d'une réflexion rationnelle. |
| 104 | `6.2` | 973 | **2022** | `desc_fr` | 61% | #369 | Vous modifiez tacitement et a posteriori l'objectif initial du débat pour ne pas reconnaître votre défaite. | Vous changez les critères du débat sans le dire, afin d'éviter d'admettre votre erreur. |
| 105 | `6.2.1.1.1.1` | 977 | v3 | `desc_fr` | 61% | — | Vous rejetez toute proposition qui ne suffirait pas à résoudre complètement le problème évoqué. | Vous rejetez les solutions réalistes en demandant un idéal inatteignable. |
| 106 | `4.1.2` | 707 | **2022** | `desc_fr` | 61% | #369 | Vous considérez les conséquences d'un fait comme l'ayant provoqué ou vice versa. | Vous prenez la cause pour la conséquence, ou inversement, ce qui inverse la relation causale. |
| 107 | `5.3.3` | 876 | v3 | `desc_fr` | 60% | #369 | Le message formulé par votre discours est sujet à interprétation. | Vous présentez votre argument sous la forme d'un récit qui peut être interprété de plusieurs manières. |
| 108 | `5.2.3` | 844 | v3 | `desc_fr` | 60% | #369 | Sous prétexte que deux éléments présentent un aspect commun, vous attribuez à l'un toutes les propriétés de l'autre dès lors que cela peut servir votre point de vue. | Vous associez à tort des éléments entre eux en vous fondant sur des caractéristiques communes sans rapport avec l'argument. |
| 109 | `7.1.2.1` | 1288 | **2022** | `desc_fr` | 60% | #369 | Vous avancez une thèse qui ne peut être ni corroborée ni infirmée par l'expérience ou le raisonnement. | Vous avancez une affirmation impossible à confirmer ou à réfuter, ce qui empêche tout échange rationnel. |
| 110 | `1.2.3.5.1` | 128 | v3 | `desc_fr` | 60% | — | Une proposition vous semble d'autant plus valide que son auteur est pourvu de bien. | Vous pensez qu'une idée est valide seulement parce que celui qui la présente est riche. |
| 111 | `1.3.2.1` | 154 | **2022** | `text_fr` | 60% | #369 | Argument du Sophisme | Sophisme du sophisme |
| 112 | `5.1.3` | 826 | v3 | `desc_fr` | 60% | #369 | Vous définissez certains termes de votre raisonnement de manière contradictoire ou absurde. | Vous donnez aux termes que vous utilisez des définitions contradictoires ou incohérentes. |
| 113 | `4.3.1` | 759 | **2022** | `desc_fr` | 59% | — | Votre déduction repose sur des éléments insuffisants. | Vous tirez des conclusions trop vite, sans disposer de suffisamment de preuves. |
| 114 | `6.2.2.1.1` | 994 | **2022** | `desc_fr` | 59% | #369 | Pour éviter de répondre, vous énoncez des banalités abstraites qui ne vous engagent en rien. | Vous noyez votre propos dans un flot de généralités, ce qui vous évite de vous engager sur quoi que ce soit de précis. |
| 115 | `1.1.1` | 3 | v3 | `desc_fr` | 59% | #369 | Vous attribuez à une habitude, une impression ou un exemple la valeur d'une preuve. | Vous utilisez des affirmations qui n'apportent rien à la construction d'un argument convaincant. |
| 116 | `5.3.3.1.1` | 878 | v3 | `desc_fr` | 59% | #369 | Vous argumentez à l'aide du contenu implicite de vos formulations. | Vous argumentez en insinuant des choses sans les dire clairement, et laissez au public le soin d'interpréter vos sous-entendus. |
| 117 | `7.1.2.2` | 1291 | v3 | `desc_fr` | 58% | #369 | Vous analysez un phénomène de manière simpliste, sans en rechercher les véritables causes. | Vous restez à la surface du sujet au lieu d'examiner suffisamment les causes profondes. |
| 118 | `4.1.3` | 719 | **2022** | `desc_fr` | 57% | #369 | Sous prétexte que deux phénomènes sont corrélés, vous établissez entre eux un lien de cause à effet. | Vous croyez à tort qu'une corrélation entre deux événements implique nécessairement un lien de causalité. |
| 119 | `3.2.2.3.1` | 653 | **2022** | `desc_fr` | 57% | #369 | Vous évaluez vos chances de succès à l'aune de la réussite de tentatives récentes. | Vous croyez que vos succès récents augmenteront vos chances de réussir à nouveau. |
| 120 | `7.3.2` | 1371 | **2022** | `desc_fr` | 57% | — | Vous infirmez une proposition du fait de sa provenance ou de l'identité de son origine. | Vous rejetez une idée en critiquant son origine plutôt que son contenu. |
| 121 | `5.1.1` | 800 | v3 | `desc_fr` | 56% | #369 | Vous définissez de manière imprécise les termes que vous employez, de manière à pouvoir les adapter à la défense de vos intérêts. | Vous utilisez des termes imprécis pour rester évasif et pouvoir modifier votre argumentation en cours de route. |
| 122 | `4.3` | 758 | v3 | `text_fr` | 56% | #369 | Mauvaise déduction | Déduction invalide |
| 123 | `3.2` | 632 | v3 | `text_fr` | 55% | #369 | Mauvaise interprétation | Interprétation quantitative erronée |
| 124 | `5.3.1` | 847 | v3 | `desc_fr` | 55% | — | Vous argumentez avec des phrases à la syntaxe équivoque. | Vous construisez votre argumentation sur des phrases dont la structure peut prêter à différentes interprétations. |
| 125 | `2.1` | 176 | v3 | `example_fr` | 55% | #369 | "Si votre ramage se rapporte à votre plumage, vous êtes le phénix des hôtes de ces bois." Jean de La Fontaine | Votre éloquence est à la hauteur de votre prestance : vous êtes vraiment exceptionnel. |
| 126 | `1.2.2.4` | 108 | **2022** | `desc_fr` | 54% | — | Vous basez votre argument sur le présupposé que ce qui naturel est bon et ce qui ne l'est pas est mauvais. | Vous supposez qu'une chose est bonne parce qu'elle est naturelle, ou mauvaise parce qu'elle n'est pas naturelle. |
| 127 | `6.3.1` | 1024 | v3 | `desc_fr` | 54% | #369 | Vous raisonnez dans un contexte biaisé par la nature humaine. | Vos arguments reflètent les limites et les réflexes liés à la perspective humaine. |
| 128 | `3.1.1` | 596 | v3 | `desc_fr` | 54% | #369 | Votre argumentation repose sur des statistiques erronées, partiales ou invérifiables établies à partir d'une enquête sur un ensemble d'individus censé être représentatif d'une population donnée. | Vous tirez des conclusions à partir d'un échantillon qui n'est pas représentatif de la population cible. |
| 129 | `6.1.3` | 953 | **2022** | `desc_fr` | 54% | — | Vous ne vous attachez qu'aux aspects de la situation qui servent votre propos. | Vous ne présentez que les faits qui soutiennent votre thèse, en occultant ceux qui la contredisent. |
| 130 | `6.3.2` | 1174 | v3 | `desc_fr` | 54% | — | Vous raisonnez dans un contexte biaisé par votre expérience sociétale. | Vos prises de position sont influencées par votre contexte culturel. |
| 131 | `2.3` | 356 | v3 | `desc_fr` | 53% | #369 | Vous employez la ruse pour fausser le point de vue de votre auditoire. | Vous utilisez des procédés qui orientent subtilement le point de vue de vos interlocuteurs, souvent sans qu'ils s'en rendent compte. |
| 132 | `5.1.2` | 804 | v3 | `desc_fr` | 53% | — | Votre argumentation repose sur une définition de votre invention. | Vous inventez une définition sur mesure pour vos termes, sans respecter leur sens commun. |
| 133 | `3.1.3` | 621 | v3 | `desc_fr` | 53% | — | Vous considérez qu'une propriété s'appliquant à un ensemble dans sa globalité s'applique également à chacun des éléments qui le composent ou inversement. | Vous prêtez à un groupe entier les caractéristiques de ses éléments, ou l'inverse. |
| 134 | `6.1.2` | 942 | **2022** | `desc_fr` | 53% | — | Vous citez des sources non pertinentes, mal identifiées, biaisées ou fabriquées de toutes pièces. | Vous donnez du poids à votre argument en citant une source fausse, mal identifiée, incompétente ou inventée. |
| 135 | `7.1.3.3` | 1301 | **2022** | `desc_fr` | 53% | #369 | Vous répétez inlassablement votre position pour venir à bout de toute velléité de contestation. | Vous répétez votre point de vue à l'excès pour tenter de l'imposer, sans répondre aux objections. |
| 136 | `2.2.3` | 340 | v3 | `desc_fr` | 53% | #369 | Vous invoquez les effets d'une proposition au lieu de vous pencher sur la proposition elle-même. | Vous fondez votre argumentation sur les conséquences d'une idée plutôt que sur sa justesse. |
| 137 | `1.3.3` | 165 | **2022** | `desc_fr` | 52% | #369 | Votre raisonnement est douteux car il ne va pas au plus simple ou au plus probable. | Vous compliquez excessivement votre raisonnement au lieu de retenir l'explication la plus simple. |
| 138 | `6.1` | 888 | v3 | `text_fr` | 52% | #369 | Arranger les faits | Présentation trompeuse des faits |
| 139 | `1.1` | 2 | v3 | `desc_fr` | 52% | #369 | Votre raisonnement manque de rigueur, il s'appuie sur des impressions ou des faits anecdotiques. | Vous construisez un argument à partir d'impressions ou d'anecdotes, sans preuve solide. |
| 140 | `2.2.1.4` | 319 | **2022** | `desc_fr` | 51% | — | Vous exploitez la compassion ou le sentiment de culpabilité de votre auditoire pour le rallier à votre cause. | Vous faites appel à la compassion de votre auditoire pour gagner son soutien sans argumenter sur le fond. |
| 141 | `7.2.2` | 1345 | v3 | `desc_fr` | 51% | #369 | Vous embrouillez le débat afin de rendre la discussion impossible. | Vous complexifiez volontairement le débat afin d'empêcher toute résolution claire. |
| 142 | `3.3` | 666 | v3 | `text_fr` | 51% | #369 | Résultat invalide | Conclusion mathématique invalide |
| 143 | `3.3.2` | 681 | **2022** | `desc_fr` | 51% | #369 | Vos conclusions reposent sur une faute d'arithmétique. | Votre conclusion résulte d'une erreur de calcul. |
| 144 | `3.3.1` | 667 | v3 | `desc_fr` | 51% | #369 | Votre argumentation se fonde sur des données trop vagues. | Vos arguments s'appuient sur des données imprécises, ce qui conduit à des conclusions incertaines. |
| 145 | `4.1.1` | 698 | v3 | `desc_fr` | 51% | — | Votre argumentation présuppose la véracité de sa conclusion. | Vous utilisez un argument qui suppose déjà ce que vous essayez de prouver. |
| 146 | `5.3.2.3.2.1` | 869 | **2022** | `desc_fr` | 51% | — | Vous considérez un élément abstrait comme s'il s'agissait d'une chose concrète. | Vous faites comme si une idée ou un terme abstrait existait physiquement, ce qui fausse votre raisonnement. |
| 147 | `5.2` | 833 | v3 | `desc_fr` | 50% | #369 | Vous basez votre raisonnement sur une comparaison qui induit en erreur. | Vous établissez une comparaison trompeuse pour appuyer votre point de vue. |
| 148 | `2.2.3.1.2` | 343 | **2022** | `desc_fr` | 50% | #369 | Vous employez la menace pour faire accepter vos conclusions. | Vous utilisez la menace pour remporter un débat au lieu de défendre votre position par la raison. |
| 149 | `6.1.1.2` | 908 | v3 | `desc_fr` | 50% | — | Votre argument s'appuie sur un fait généralement reconnu mais qui est faux. | Vous vous appuyez sur des faits largement acceptés mais inexacts pour soutenir vos dires. |
| 150 | `7.3.1.2` | 1365 | **2022** | `desc_fr` | 50% | #369 | Consiste à présenter la position de son adversaire de façon caricaturale pour la réfuter plus facilement. | Vous caricaturez l'argument de l'autre afin de pouvoir le réfuter plus facilement. |
| 151 | `4.2` | 726 | v3 | `text_fr` | 49% | #369 | Mauvaise composition | Composition fautive |
| 152 | `4.3.3.3.1` | 796 | v3 | `desc_fr` | 49% | — | Votre argumentation repose sur un syllogisme comptant quatre propositions au lieu de trois. | Votre raisonnement utilise quatre termes, alors qu'un syllogisme valide n'en contient que trois. |
| 153 | `6.2.2.5` | 1004 | v3 | `desc_fr` | 48% | — | Vous modifiez discrètement votre position pour parer des arguments ou des faits qui la réfutent. | Vous adaptez votre position pour esquiver des critiques tout en prétendant maintenir le même discours. |
| 154 | `4.1.1.1` | 699 | **2022** | `desc_fr` | 48% | #369 | Votre raisonnement repose sur des éléments qui présupposent que votre conclusion est vraie. | Vous raisonnez en cercle : chaque argument repose sur l'acceptation préalable de la conclusion. |
| 155 | `5.1` | 799 | **2022** | `desc_fr` | 48% | #369 | Vous faites varier la définition des termes que vous employez au gré de votre argumentation. | Vous définissez les termes de façon à favoriser votre argument, en écartant leur sens établi. |
| 156 | `2.1.1.3` | 184 | v3 | `desc_fr` | 48% | #369 | Vous déterminez de manière partiale certains des termes sur lesquels repose votre argumentation. | Vous définissez des termes de manière biaisée pour orienter le débat en votre faveur. |
| 157 | `2.2.2` | 322 | v3 | `desc_fr` | 47% | #369 | Vous cherchez à discréditer une proposition en la dénigrant au lieu d'argumenter pour convaincre. | Vous discréditez une idée en la rendant repoussante, au lieu de l'examiner rationnellement. |
| 158 | `6.2` | 973 | **2022** | `text_fr` | 46% | #369 | Changement de cap | Déplacement des critères |
| 159 | `6.1` | 888 | v3 | `desc_fr` | 45% | #369 | Vous présentez des événements ou des faits sous un jour trompeur. | Vous présentez les faits ou les événements d'une manière susceptible d'induire en erreur. |
| 160 | `6.3` | 1023 | v3 | `text_fr` | 45% | #369 | Pensée biaisée | Raisonnement biaisé |
| 161 | `3.1.3.1` | 622 | **2022** | `desc_fr` | 45% | #369 | Vous considérez qu'une propriété s'appliquant aux éléments distincts d'un ensemble s'applique automatiquement à l'ensemble tout entier. | Vous pensez à tort qu'une caractéristique commune aux membres d'un groupe s'applique au groupe dans son ensemble. |
| 162 | `7.1.1` | 1282 | **2022** | `desc_fr` | 45% | — | Vous considérez qu'il y a autant de vérités que de points de vue individuels. | Vous affirmez que la vérité est subjective et propre à chaque individu. |
| 163 | `6.2.2` | 992 | v3 | `desc_fr` | 44% | #369 | Vous n'adoptez pas clairement une position, afin qu'il soit impossible de vous prendre en défaut. | Vous évitez de prendre une position claire afin de ne pas pouvoir être mis en défaut. |
| 164 | `3.3.3` | 690 | v3 | `desc_fr` | 43% | — | Vous usez de raisonnements scientifiques à mauvais escient. | Vous utilisez un type de raisonnement mathématique qui n'est pas valide dans cette situation. |
| 165 | `2.3.2.1` | 421 | v3 | `desc_fr` | 43% | — | Vous usez de votre pouvoir de séduction pour influer sur votre auditoire. | Vous utilisez votre charisme pour obtenir l'adhésion de votre auditoire à vos idées. |
| 166 | `7.2` | 1312 | v3 | `desc_fr` | 42% | #369 | Vous détériorez le cadre de la discussion dans le but de l'orienter ou d'en limiter la portée. | Vous perturbez le débat pour l'orienter à votre avantage ou pour en limiter la portée. |
| 167 | `5.2.1.3` | 837 | v3 | `desc_fr` | 42% | #369 | Vous comparez plusieurs choses selon certains aspects seulement pour ensuite prétendre les avoir globalement comparées. | Vous comparez des choses en ne retenant que certains aspects, ce qui fausse la comparaison globale. |
| 168 | `3.2.2` | 644 | v3 | `example_fr` | 42% | #369 | L'ADN laissé par l'auteur de ce crime présente une particularité que l'on ne trouve que chez dix pour cent de la population, singularité également présente dans l'ADN de l'accusé que voici ; on peut donc affirmer à quatre-vingt-dix pour cent que cet homme est coupable. | L'ADN laissé par l'auteur de ce crime présente une particularité que l'on retrouve chez l'accusé et chez seulement 10 % de la population ; on peut donc affirmer avec 90 % de certitude que cet homme est coupable. |
| 169 | `6.1.1` | 889 | **2022** | `desc_fr` | 42% | #369 | Vous affirmez des choses contraires à la vérité. | Vous affirmez quelque chose que vous savez faux. |
| 170 | `2.2` | 299 | **2022** | `example_fr` | 41% | #369 | Rapporte-nous un bon bulletin de notes, et ton père sera fier de toi. | Si vous avez de bonnes notes, votre père sera très fier de vous. |
| 171 | `7.1.2` | 1287 | v3 | `desc_fr` | 41% | — | Vous trompez votre auditoire en prétendant expliquer des choses. | Vous faussez le débat en feignant d'expliquer des faits ou des concepts. |
| 172 | `3.3.1.2` | 670 | **2022** | `desc_fr` | 40% | — | Vous attribuez injustement la même valeur à deux points de vue différents. | Vous attribuez le même poids à deux avis divergents, de manière injustifiée. |
| 173 | `7.1.2` | 1287 | v3 | `text_fr` | 40% | #369 | Sophisme d'Explication | Pseudo-explication |
| 174 | `4.3.2.2.1` | 781 | v3 | `desc_fr` | 40% | — | Votre argument utilise des raisons qui, bien que valables une-à-une, se contredisent. | Vous multipliez les justifications qui semblent valables seules, mais qui, prises ensemble, se contredisent. |
| 175 | `3.2.1.3` | 636 | v3 | `desc_fr` | 39% | #369 | Vous attribuez une cause erronée à ce qui n'est que le résultat d'une fluctuation tout à fait normale. | Vous attribuez à tort un effet particulier à ce qui n'est qu'un retour à la normale après une fluctuation. |
| 176 | `2.3.3` | 511 | **2022** | `text_fr` | 39% | #1331 | Influence non verbale | Communication non verbale |
| 177 | `4.3.1.2` | 768 | v3 | `desc_fr` | 38% | #369 | Vous faites un parallèle absurde entre deux arguments distincts. | Vous établissez un parallèle injustifié entre deux arguments ou deux situations qui ne sont pas comparables. |
| 178 | `2.3.3` | 511 | **2022** | `desc_fr` | 38% | #369 | Vous cherchez à agir sur la volonté de votre auditoire en utilisant autre chose que des mots, à l'instar du langage corporel ou des inflexions vocales. | Vous cherchez à convaincre au-delà du sens des mots, notamment par le langage corporel ou les inflexions de la voix. |
| 179 | `7.1` | 1281 | v3 | `desc_fr` | 37% | #369 | Vous évitez la discussion pour empêcher toute forme d'argumentation. | Vous esquivez le débat pour empêcher tout échange constructif fondé sur des arguments rationnels. |
| 180 | `2.1` | 176 | v3 | `text_fr` | 37% | #369 | Procédé rhétorique | Technique rhétorique |
| 181 | `6.3.1` | 1024 | v3 | `example_fr` | 37% | #369 | La nature est à ce point pleine de sagesse qu'elle a fait traverser chaque ville par une rivière. | La nature est si sage qu'elle a fait passer une rivière dans chaque ville. |
| 182 | `7.3.2.1.1` | 1373 | v3 | `desc_fr` | 36% | — | Vous disqualifiez les arguments d'un adversaire en les associant à Adolf Hitler. | Vous disqualifiez un argument en l'associant injustement à Hitler ou au nazisme. |
| 183 | `5.2.3.1` | 845 | **2022** | `desc_fr` | 36% | #369 | Vous associez un élément à une catégorie notoire sous prétexte d'un seul aspect en commun. | Vous associez à tort une personne ou une chose à une catégorie négative en vous fondant sur un seul point commun. |
| 184 | `3.1.3.2` | 625 | v3 | `desc_fr` | 36% | #369 | Vous attribuez les qualités d'un ensemble à chacun de ses éléments. | Vous attribuez à tort certaines caractéristiques d'un groupe à chacun de ses membres. |
| 185 | `4.3.2` | 777 | **2022** | `desc_fr` | 35% | — | Votre raisonnement s'appuie sur des hypothèses contradictoires. | Votre raisonnement repose sur des affirmations qui se contredisent entre elles. |
| 186 | `1.2.2.2` | 98 | **2022** | `text_fr` | 35% | #369 | Raison de la majorité | Appel à la majorité |
| 187 | `4.3.3.1.1.1` | 787 | v3 | `desc_fr` | 34% | #369 | Votre argument établit qu'une chose est souhaitable car elle est conforme. | Votre argument affirme qu'une action est souhaitable simplement parce qu'elle constitue un changement. |
| 188 | `3.1.2` | 614 | **2022** | `desc_fr` | 33% | #369 | Vous appliquez un principe général à une situation sans tenir compte du caractère particulier de celle-ci. | Vous appliquez une règle générale à un cas particulier, sans tenir compte des exceptions pertinentes. |
| 189 | `1.3.2` | 153 | v3 | `desc_fr` | 33% | #369 | Vous affirmez qu'une position est fausse en supposant que la réflexion ou les intentions de son auteur sont sujettes à caution. | Vous rejetez une position en supposant que le raisonnement ou les intentions de son auteur sont douteux, plutôt qu'en examinant l'argument lui-même. |
| 190 | `6.3` | 1023 | v3 | `example_fr` | 32% | #369 | Ces personnes-là se ressemblent toutes : comment font-elles pour se reconnaître entre elles ? | Pour moi, tous les membres de ce groupe se ressemblent ; comment font-ils pour se reconnaître entre eux ? |
| 191 | `6.1.3` | 953 | **2022** | `example_fr` | 32% | #369 | Le vin est un produit excellent pour la santé car il contient des antioxydants. | Le vin est excellent pour la santé : il contient des antioxydants. Inutile de parler des risques liés à l'alcool. |
| 192 | `3.1.3` | 621 | v3 | `example_fr` | 32% | #369 | Un camion consomme plus de carburant qu'une voiture ; on peut en déduire que le volume global de carburant consommé par les camions est supérieur à celui que consomment les voitures. | Un camion consomme plus de carburant qu'une voiture ; on peut donc en déduire que les camions consomment, au total, plus de carburant que les voitures. |
| 193 | `5.3.2` | 855 | v3 | `desc_fr` | 30% | #369 | Vous jouez sur les acceptions multiples d'un mot pour rendre votre discours ambigu. | Vous exploitez les différents sens d'un même mot pour rendre votre propos ambigu. |
| 194 | `1.3.2.1` | 154 | **2022** | `desc_fr` | 30% | #369 | Vous affirmez qu'une conclusion est fausse car l'argumentation qui y mène est fallacieuse. | Vous rejetez une conclusion uniquement parce que l'argument qui la soutient est fallacieux. |
| 195 | `6.2.1.4` | 989 | **2022** | `desc_fr` | 30% | #369 | Vous estimez que c'est à la partie adverse de démontrer l'invalidité de votre position. | Vous estimez que c'est à la partie adverse de démontrer que votre position est fausse, au lieu de la justifier vous-même. |
| 196 | `3.2.1` | 633 | v3 | `desc_fr` | 30% | #369 | Vous attribuez un sens à une simple coïncidence. | Vous attribuez une relation significative à ce qui n'est qu'une simple coïncidence. |
| 197 | `5.1.2.2.4` | 814 | **2022** | `desc_fr` | 30% | #369 | Vous raisonnez sur la base d'une alternative binaire alors qu'il existe d'autres possibilités. | Vous raisonnez à partir d'un choix limité à deux options, alors qu'il existe d'autres possibilités. |
| 198 | `1.3.1` | 134 | v3 | `text_fr` | 29% | #369 | Sophisme ludique | Sophisme du jeu |
| 199 | `2.1.1.4` | 185 | **2022** | `desc_fr` | 28% | #369 | Vous utilisez des clichés accrocheurs pour court-circuiter toute réflexion argumentée. | Vous utilisez des clichés frappants pour court-circuiter l'esprit critique ou éviter un débat argumenté. |
| 200 | `3.3` | 666 | v3 | `desc_fr` | 28% | #369 | Vos conclusions reposent sur un raisonnement mathématique erroné. | Vous tirez une conclusion à partir d'un raisonnement mathématique incorrect. |
| 201 | `3.1.1` | 596 | v3 | `example_fr` | 26% | #369 | Ce yaourt est le meilleur, puisqu'il a été plébiscité par quatre-vingts pour cent des consommateurs. | Ce yaourt est le meilleur, puisqu'il a été plébiscité par 80 % des consommateurs interrogés dans notre magasin. |
| 202 | `1.3.3` | 165 | **2022** | `example_fr` | 26% | #369 | Où sont mes clefs ? Quelqu'un a dû me les voler. C'est une conspiration qui cherche à me rendre fou. | Où sont mes clés ? Quelqu'un a dû me les voler. Il y a sûrement un complot pour me rendre fou. |
| 203 | `3.2` | 632 | v3 | `desc_fr` | 24% | #369 | Vous établissez des relations inexistantes entre divers éléments. | Vous établissez des relations quantitatives inexactes entre des éléments ou des données. |
| 204 | `6.1.2` | 942 | **2022** | `example_fr` | 24% | — | Un comité scientifique trié sur le volet a déclaré que cette crème pour les mains était la vraie recette du bonheur. | Un " comité scientifique " dont personne ne connaît les membres affirme que cette crème pour les mains est la vraie recette du bonheur. |
| 205 | `4.2.2` | 735 | **2022** | `example_fr` | 23% | #369 | Tous les philosophes sont sages ; cependant quelques-uns sont idiots. | Tous les philosophes sont sages ; pourtant, certains philosophes sont idiots. |
| 206 | `6.3.3` | 1242 | **2022** | `example_fr` | 23% | #369 | En cuisine, il n'y a que l'huile d'olive qui vaille. Je fais donc ma viennoiserie sans beurre. | En cuisine, seule l'huile d'olive vaut vraiment la peine d'être utilisée. Je fais donc mes viennoiseries sans beurre. |
| 207 | `6.2.2` | 992 | v3 | `text_fr` | 23% | #369 | Beurre et argent du beurre | Vouloir le beurre et l'argent du beurre |
| 208 | `7.1` | 1281 | v3 | `example_fr` | 22% | #369 | Mes positions sont claires, connues de tous, et elles ne changeront pas. | Mes positions sont claires, connues de tous, et je n'ai pas à en débattre. |
| 209 | `7.3.2` | 1371 | **2022** | `example_fr` | 22% | #369 | Je ne vais pas prendre les conseils d'un américain pour choisir mon vin tout de même. | Je ne vais tout de même pas suivre les conseils d'un Américain pour choisir mon vin. |
| 210 | `5.1.3` | 826 | v3 | `text_fr` | 22% | #369 | Définition inconsistante | Définition incohérente |
| 211 | `5.1.2.2.1` | 809 | **2022** | `desc_fr` | 21% | #369 | Vous définissez des termes distincts de manière à ce qu'on ne les distingue plus les uns des autres. | Vous définissez des termes distincts de telle sorte qu'il devient impossible de les distinguer les uns des autres. |
| 212 | `7.2` | 1312 | v3 | `text_fr` | 21% | #369 | Saboter le débat | Sabotage du débat |
| 213 | `5.1` | 799 | **2022** | `text_fr` | 21% | #369 | Définition imprécise | Définition biaisée |
| 214 | `7.2.3` | 1352 | **2022** | `text_fr` | 21% | #369 | Empoisonner le puits | Empoisonnement du puits |
| 215 | `6.3.3` | 1242 | **2022** | `desc_fr` | 21% | #369 | Vous raisonnez dans un contexte axé sur l'application stricte de concepts théoriques. | Vous raisonnez dans un cadre déformé par une application trop rigide de concepts théoriques. |
| 216 | `5.2.1.3` | 837 | v3 | `text_fr` | 21% | #369 | Comparaison inconsistante | Comparaison incohérente |
| 217 | `5.1.2.2` | 808 | v3 | `example_fr` | 21% | #369 | Je t'adore, donc je ne t'aime pas | Je vous adore, donc je ne vous aime pas. |
| 218 | `7.3.1` | 1361 | v3 | `text_fr` | 19% | #369 | Procès en inconstance | Procès en incohérence |
| 219 | `6.2.2` | 992 | v3 | `example_fr` | 19% | #369 | Si j'ai par mégarde heurté quelqu'un dans cette assistance, je tiens à lui présenter mes excuses ; j'ai cependant agi en mon âme et conscience. | Si j'ai offensé quelqu'un dans cette assemblée, même par inadvertance, je lui présente mes excuses ; j'ai cependant agi en mon âme et conscience. |
| 220 | `2.3.1.1.1.2` | 361 | v3 | `example_fr` | 18% | #369 | Les parents ont le devoir d'éduquer leurs enfants et de subvenir à leurs besoins ; c'est aussi à eux de déterminer quelles études ils doivent faire. | Les parents ont le devoir d'éduquer leurs enfants et de subvenir à leurs besoins ; ils devraient donc aussi déterminer les études que leurs enfants doivent suivre. |
| 221 | `5.1.2.2.3` | 813 | **2022** | `desc_fr` | 18% | #369 | Afin de réfuter un contre-exemple, vous modifiez les contours de l'idée générale que vous soutenez. | Pour écarter un contre-exemple, vous redéfinissez les contours de l'idée générale que vous défendez. |
| 222 | `1.2.3.5.1` | 128 | v3 | `example_fr` | 17% | #369 | S'il avait tort, il n'en serait pas parvenu là aujourd'hui. | S'il avait tort, il ne serait pas arrivé là où il en est aujourd'hui. |
| 223 | `5.2` | 833 | v3 | `example_fr` | 17% | #369 | L'école, c'est comme la prison. On n'y entre pas comme on veut. | L'école, c'est comme la prison : on n'y entre pas et on n'en sort pas comme on veut. |
| 224 | `2.3.2` | 420 | v3 | `desc_fr` | 17% | #369 | Vous cherchez à influer sur votre auditoire en prenant sur lui un ascendant psychologique. | Vous cherchez à influencer votre auditoire en exerçant un ascendant psychologique sur lui. |
| 225 | `6.1.3.1.1.1` | 956 | **2022** | `desc_fr` | 17% | — | Vous prétendez faire exception à une règle générale sans raison valable. | Vous demandez une exception à une règle sans raison valable. |
| 226 | `6.2.1` | 974 | v3 | `desc_fr` | 17% | #369 | Vous augmentez vos prétentions à mesure que votre interlocuteur y accède. | Vous augmentez vos exigences à mesure que votre interlocuteur y répond. |
| 227 | `4.2.2.2` | 740 | v3 | `example_fr` | 15% | #369 | Il est faux que tous les chiens sont blancs, donc il n'y a pas de chiens blancs. | Il est faux que tous les chiens soient blancs ; donc il n'existe aucun chien blanc. |
| 228 | `1.1` | 2 | v3 | `example_fr` | 15% | #369 | Hier, j'ai marché dans une crotte de chien en saluant un passant dans la rue. En ville, mieux vaut éviter de s'adresser à des inconnus. | Hier, j'ai marché dans une crotte de chien après avoir salué un passant dans la rue. En ville, il vaut donc mieux éviter de parler à des inconnus. |
| 229 | `6.3.2` | 1174 | v3 | `example_fr` | 15% | #369 | Ces personnes sont bien à plaindre, sans télévision ni voiture... | Ces personnes sont bien à plaindre : elles n'ont ni télévision ni voiture… |
| 230 | `5.3.3` | 876 | v3 | `example_fr` | 15% | #369 | Lundi dernier, tu as rendu ton devoir à temps ; aujourd'hui, il faut qu'on parle. | Lundi dernier, vous avez rendu votre devoir à temps ; aujourd'hui, il faut que nous parlions. |
| 231 | `7.2` | 1312 | v3 | `example_fr` | 15% | #369 | Allons donc dans mon bureau pour discuter de la stupidité de votre demande d'augmentation... | Allons plutôt dans mon bureau discuter de l'absurdité de votre demande d'augmentation… |
| 232 | `2.1.1.1.3` | 182 | **2022** | `example_fr` | 14% | #369 | Pile je gagne. Face tu perds. | Pile, je gagne. Face, vous perdez. |
| 233 | `2.2.2.1` | 323 | v3 | `desc_fr` | 14% | #369 | Vous présentez un élément de façon à ce qu'il paraisse soit condamnable, soit indigne d'attention ou d'estime. | Vous présentez un élément de sorte qu'il paraisse condamnable, indigne d'attention ou indigne d'estime. |
| 234 | `2.1.3` | 247 | v3 | `desc_fr` | 13% | #369 | Vous usez d'images et de propos fleuris pour rendre votre argumentation plus séduisante. | Vous utilisez des images et un style fleuri pour rendre votre argumentation plus séduisante. |
| 235 | `2.1.1.3` | 184 | v3 | `example_fr` | 13% | #369 | Cet homme est un athée, quelqu'un qui n'a pas été touché par la grâce de Dieu. | Cet homme est athée : il n'a pas été touché par la grâce de Dieu. |
| 236 | `2.3.2` | 420 | v3 | `example_fr` | 12% | #369 | Si ton amour pour moi est sincère, alors tu comprendras ce que je vais t'expliquer. | Si votre amour pour moi est sincère, alors vous comprendrez ce que je vais vous expliquer. |
| 237 | `4.3.1.2` | 768 | v3 | `example_fr` | 12% | #369 | Tu me demandes de faire mes devoirs après l'école; mais je ne te demande pas, moi, de faire des heures supplémentaires une fois rentré du bureau. | Vous me demandez de faire mes devoirs après l'école ; mais moi, je ne vous demande pas de faire des heures supplémentaires quand vous rentrez du bureau. |
| 238 | `7.1.3.3` | 1301 | **2022** | `example_fr` | 12% | #369 | Les produits laitiers sont nos amis pour la vie. Ils sont excellents pour la santé. En plus ils sont vraiment bon pour nous. Ils permettent d'être en forme. | Les produits laitiers sont nos amis pour la vie. Ils sont excellents pour la santé. Ils sont vraiment bons pour nous. Ils nous aident à rester en forme. |
| 239 | `4.1.2.1` | 708 | **2022** | `desc_fr` | 12% | #369 | Vous déduisez une cause de son effet sans prendre en compte les autres causes possibles. | Vous déduisez une cause à partir de son effet, sans tenir compte des autres causes possibles. |
| 240 | `1.2.3.3.2` | 121 | **2022** | `desc_fr` | 11% | #369 | Vous rejetez un argument parce qu'il risque de heurter la sensibilité d'une partie de l'auditoire. | Vous rejetez ou détournez un argument au motif qu'il risque de heurter la sensibilité d'une partie de l'auditoire. |
| 241 | `3.3.1.3.1` | 677 | **2022** | `example_fr` | 11% | #369 | Si nous le laissons se resservir de ce plat, il prendra l'habitude de manger à l'excès et deviendra obèse. | Si nous le laissons se resservir de ce plat, il prendra l'habitude de trop manger et finira obèse. |
| 242 | `5.1.2.2` | 808 | v3 | `text_fr` | 11% | #369 | Sophisme de corrélation | Sophisme des corrélatifs |
| 243 | `4.2` | 726 | v3 | `example_fr` | 11% | #369 | Plus il y a de gruyère, plus il y a de trous ; or plus il y a de trous, moins il y a de gruyère. Donc plus il y a de gruyère, moins il y en a. | Plus il y a de fromage, plus il y a de trous ; or plus il y a de trous, moins il y a de fromage. Donc plus il y a de fromage, moins il y en a. |
| 244 | `7.3.1.1` | 1362 | **2022** | `example_fr` | 10% | #369 | Comment peut-on lire ce que Jean-Jacques Rousseau peut écrire sur l'éducation des enfants alors qu'il a abandonné les siens ? | Comment peut-on prendre au sérieux ce que Jean-Jacques Rousseau écrit sur l'éducation des enfants, alors qu'il a abandonné les siens ? |
| 245 | `5.1.2` | 804 | v3 | `example_fr` | 10% | #369 | L'amour, c'est savoir pardonner. Du coup, comment oses-tu me reprocher mon infidélité ? | L'amour, c'est savoir pardonner. Alors, comment osez-vous me reprocher mon infidélité ? |
| 246 | `2.2.2.1` | 323 | v3 | `example_fr` | 10% | #369 | Mon fils, tu ne feras pas tes études en fac de Lettres, ramassis notoire de paresseux et de névrosés ! | Mon fils, vous ne ferez pas vos études en faculté de lettres, ce ramassis notoire de paresseux et de névrosés ! |
| 247 | `6.2.1` | 974 | v3 | `example_fr` | 10% | — | - Tu ne sais pas conduire. - Mais voyons, j'ai mon permis de conduire ! - Oui, mais tu n'as jamais su faire un créneau convenablement... | - Vous ne savez pas conduire. - Mais voyons, j'ai mon permis de conduire ! - Oui, mais vous n'avez jamais su faire un créneau correctement... |
| 248 | `1.2.1.2.1` | 79 | **2022** | `example_fr` | 9% | #369 | Ton opinion sur la musique m'intéressera quand tu auras sorti un album ! | Votre opinion sur la musique m'intéressera quand vous aurez sorti un album ! |
| 249 | `1.2.1` | 71 | **2022** | `example_fr` | 9% | #369 | Bien qu'ayant trop chaud, je n'enlève pas mon manteau, car maman a dit qu'il faisait froid. | Même si j'ai trop chaud, je n'enlève pas mon manteau, car maman a dit qu'il faisait froid. |
| 250 | `2.2.3.1.2` | 343 | **2022** | `example_fr` | 9% | #369 | Si tu ne me laisses pas la dernière tranche de jambon, tu le paieras très cher. | Si vous ne me laissez pas la dernière tranche de jambon, vous le paierez très cher. |
| 251 | `2.1.2` | 219 | v3 | `example_fr` | 8% | #369 | Je ne suis pas vieux, voyons ! Je suis tout simplement jeune depuis plus longtemps que toi... | Je ne suis pas vieux, voyons ! Je suis simplement jeune depuis plus longtemps que vous… |
| 252 | `2.1.1` | 177 | v3 | `desc_fr` | 7% | #369 | Vous vous exprimez de manière à imposer implicitement les termes du débat. | Vous vous exprimez de manière à imposer implicitement le cadre et les termes du débat. |
| 253 | `4.1.2` | 707 | **2022** | `example_fr` | 7% | #369 | Il est dangereux d'utiliser un fauteuil roulant, puisque la plupart des personnes qui en sont équipées ont déjà été victimes d'accidents. | Il est dangereux d'utiliser un fauteuil roulant, puisque la plupart des personnes qui en utilisent un ont été victimes d'accidents. |
| 254 | `7.1.1` | 1282 | **2022** | `example_fr` | 7% | #369 | C'est peut-être vrai pour toi, mais ça ne l'est pas pour moi. | C'est peut-être vrai pour vous, mais cela ne l'est pas pour moi. |
| 255 | `5.1.3` | 826 | v3 | `example_fr` | 7% | #369 | Une bonne éducation se doit d'éveiller l'esprit critique ; or un individu doué d'esprit critique ne se laisse jamais influencer par qui que ce soit. | Une bonne éducation doit éveiller l'esprit critique ; or une personne douée d'esprit critique ne se laisse jamais influencer par qui que ce soit. |
| 256 | `3.3.1.2.2` | 673 | **2022** | `example_fr` | 7% | #369 | Tu préfères le lundi et moi le mercredi ? Coupons la poire en deux et voyons-nous donc mardi ! | Vous préférez le lundi et moi le mercredi ? Coupons la poire en deux et voyons-nous mardi ! |
| 257 | `3.3.3` | 690 | v3 | `example_fr` | 7% | #369 | Dans la fable de La Fontaine, chaque fois que le lièvre avance, la tortue avance aussi, empêchant ainsi le lièvre de combler son retard. | Dans la fable de La Fontaine, chaque fois que le lièvre avance, la tortue avance aussi ; cela empêcherait donc le lièvre de combler son retard. |
| 258 | `1.3.2.1` | 154 | **2022** | `example_fr` | 7% | #369 | Il m'a dit d'emprunter la route qui descend sur la droite; or cette route est une montée. Tournons plutôt à gauche. | Il m'a dit d'emprunter la route qui descend sur la droite ; or cette route monte. Prenons plutôt à gauche. |
| 259 | `1.2.2.4` | 108 | **2022** | `example_fr` | 7% | #369 | Il est normal que chez les hommes, comme chez la plupart des animaux, les mâles dominent car ils sont plus forts et plus violents. | Il est normal que, chez les êtres humains comme chez la plupart des animaux, les mâles dominent, car ils sont plus forts et plus violents. |
| 260 | `1.2.3.3.2` | 121 | **2022** | `example_fr` | 7% | #369 | Ne cherchez pas d'explication du côté des croyances; la religion est affaire de conscience et ne doit donc jamais être critiquée. | Ne cherchez pas d'explication du côté des croyances ; la religion relève de la conscience et ne doit donc jamais être critiquée. |
| 261 | `5.1.2.2.4` | 814 | **2022** | `example_fr` | 6% | #369 | Si les nouvelles technologies ne t'intéressent pas, rien ne t'empêche de retourner vivre dans une grotte. | Si les nouvelles technologies ne vous intéressent pas, rien ne vous empêche de retourner vivre dans une grotte. |
| 262 | `5.1.2.2.3` | 813 | **2022** | `example_fr` | 6% | — | - Tous les Écossais sont roux. - Angus est écossais mais il n'est pas roux. - Ce n'est donc pas un vrai Écossais. | - Tous les Écossais sont roux. - Angus est écossais, mais il n'est pas roux. - Alors ce n'est pas un vrai Écossais. |
| 263 | `3.1.2` | 614 | **2022** | `example_fr` | 6% | #369 | Ma voiture peut rouler à cent trente kilomètres à l'heure ; traverser la ville ne me prendra donc que quelques minutes. | Ma voiture peut rouler à 130 kilomètres à l'heure ; traverser la ville ne me prendra donc que quelques minutes. |
| 264 | `5.3.3.1.1` | 878 | v3 | `example_fr` | 6% | #369 | Tu étais seul dans la pièce, et je ne trouve plus mon portefeuille. | Vous étiez seul dans la pièce, et je ne trouve plus mon portefeuille. |
| 265 | `5.1.2.2.1` | 809 | **2022** | `example_fr` | 5% | #369 | Les atomes font partie de la nature ; cette poudre chimique est composée d'atomes donc c'est un produit naturel. | Les atomes font partie de la nature ; cette substance chimique est composée d'atomes, donc c'est un produit naturel. |
| 266 | `1.2` | 70 | v3 | `example_fr` | 5% | #369 | Laisse donc ton père ouvrir ce pot de cornichons: c'est une affaire d'hommes. | Laissez donc votre père ouvrir ce pot de cornichons : c'est une affaire d'hommes. |
| 267 | `3.3.2` | 681 | **2022** | `example_fr` | 5% | #369 | Sachant qu'un croissant coûte un euro et cinquante centimes, avec dix euros tu peux largement en acheter huit. | Sachant qu'un croissant coûte un euro et cinquante centimes, avec dix euros, vous pouvez largement en acheter huit. |
| 268 | `2.2.1` | 300 | v3 | `desc_fr` | 5% | #369 | Vous instaurez une alliance émotionnelle avec votre auditoire pour infléchir son jugement. | Vous créez une alliance émotionnelle avec votre auditoire pour infléchir son jugement. |
| 269 | `6.2.2.5` | 1004 | v3 | `example_fr` | 4% | #369 | Je n'ai pas dit que je voulais abandonner ce projet, voyons, mais simplement que j'envisageais d'y travailler différemment. | Je n'ai pas dit que je voulais abandonner ce projet, voyons ; j'ai simplement dit que j'envisageais d'y travailler différemment. |
| 270 | `6.1.2.1` | 943 | **2022** | `example_fr` | 4% | — | - Tu m'as pourtant dit avoir trouvé ce film éblouissant... - Éblouissant par sa stupidité ! | - Vous m'avez pourtant dit avoir trouvé ce film éblouissant... - Éblouissant par sa stupidité ! |
| 271 | `4.2.1` | 727 | v3 | `example_fr` | 4% | #369 | Il ne peut pas pleuvoir et neiger en même temps ; du coup, s'il ne pleut pas, il neige. | Il ne peut pas pleuvoir et neiger en même temps ; donc, s'il ne pleut pas, il neige. |
| 272 | `2.2.1.4` | 319 | **2022** | `example_fr` | 4% | #369 | Mesdames et messieurs du jury, regardez cet homme misérable, dans un fauteuil roulant, incapable d'utiliser ses jambes. Un tel homme peut-il vraiment être coupable de détournement de fonds ? | Mesdames et messieurs les jurés, regardez cet homme misérable, en fauteuil roulant, incapable d'utiliser ses jambes. Un tel homme peut-il vraiment être coupable de détournement de fonds ? |
| 273 | `4.3.2.2.1` | 781 | v3 | `example_fr` | 4% | #369 | Premièrement je n'ai absolument pas emprunté de chaudron à B ; deuxièmement le chaudron avait déjà un trou lorsque je l'ai reçu de B ; troisièmement je lui ai rendu le chaudron intact." | Premièrement, je n'ai absolument pas emprunté de chaudron ; deuxièmement, le chaudron avait déjà un trou lorsque je l'ai reçu ; troisièmement, je lui ai rendu le chaudron intact. |
| 274 | `3.3` | 666 | v3 | `example_fr` | 3% | #369 | L'accélération moyenne de la voiture A est supérieure à celle de la voiture B ; la voiture A roule donc plus vite que la voiture B. | L'accélération moyenne de la voiture A est supérieure à celle de la voiture B ; la voiture A va donc plus vite que la voiture B. |
| 275 | `2.1.1.4` | 185 | **2022** | `text_fr` | 3% | #369 | Poncif anti critique | Poncif anticritique |
| 276 | `1.3.2` | 153 | v3 | `example_fr` | 2% | #369 | Cet homme a tout intérêt me vendre sa voiture, sa description du véhicule manque nécessairement d'objectivité: mieux vaut ne pas l'acheter. | Cet homme a tout intérêt à me vendre sa voiture. Sa description du véhicule manque nécessairement d'objectivité : mieux vaut ne pas l'acheter. |
| 277 | `2.3.2.1` | 421 | v3 | `example_fr` | 2% | #369 | Après une si belle entrée en matière, je suis sûr que nous parviendrons à dépasser ce facheux différent. | Après une si belle entrée en matière, je suis sûr que nous parviendrons à dépasser ce fâcheux différend. |
| 278 | `4.2.2.2` | 740 | v3 | `text_fr` | 2% | #369 | Inférence immédiate erronnée | Inférence immédiate erronée |
| 279 | `3.1.3.2` | 625 | v3 | `example_fr` | 1% | #369 | Les tigres royaux sont voués à disparaître prématurément; ce tigre royal mourra donc jeune. | Les tigres royaux sont voués à disparaître prématurément ; ce tigre royal mourra donc jeune. |
| 280 | `1.2.2.3` | 104 | **2022** | `example_fr` | 0% | #369 | La corrida a toujours existé en France; il serait donc stupide d'interdire cette pratique dans notre pays. | La corrida a toujours existé en France ; il serait donc stupide d'interdire cette pratique dans notre pays. |
| 281 | `4.2.3` | 750 | **2022** | `example_fr` | 0% | #369 | Maman prétend que je ne peux pas manger des bonbons après m'être brossé les dents ; pourtant, j'y parviens sans difficulté ! | Maman prétend que je ne peux pas manger de bonbons après m'être brossé les dents ; pourtant, j'y parviens sans difficulté ! |
| 282 | `1.2.2.2` | 98 | **2022** | `example_fr` | 0% | #369 | La plupart des enfants réclament tous les jours des frites à la cantine; c'est donc ce qu'il faudrait leur servir quotidiennement. | La plupart des enfants réclament tous les jours des frites à la cantine ; c'est donc ce qu'il faudrait leur servir quotidiennement. |

### 6.2 — `C` et `AJOUT` : à garder sans arbitrage (9 cellules)

| `path` | PK | réf. | champ | classe | **Référence** | **Aujourd'hui** |
|---|---:|:-:|---|:-:|---|---|
| `1.3.2` | 153 | v3 | `text_fr` | **AJOUT** | Mauvaises raisons | Argument des mauvaises raisons |
| `3.3.1` | 667 | v3 | `example_fr` | **C** | Pour le même prix, offrez-vous notre nouvelle formule double fraicheur ! | Pour le même prix, offrez-vous notre nouvelle formule double fraîcheur ! |
| `4.1.2.1` | 708 | **2022** | `example_fr` | **C** | Après une averse, le sol est toujours mouillé. Le sol est mouillé donc il a plu. | Après une averse, le sol est toujours mouillé. Le sol est mouillé, donc il a plu. |
| `5.1.2.2.3` | 813 | **2022** | `text_fr` | **C** | Sophisme du vrai écossais | Sophisme du vrai Écossais |
| `6.2.2.1.1` | 994 | **2022** | `example_fr` | **C** | Votre colère est légitime. En ces temps troublés nous devons tous faire preuve d'un grand sens des responsabilités. | Votre colère est légitime. En ces temps troublés, nous devons tous faire preuve d'un grand sens des responsabilités. |
| `7.1.2.1` | 1288 | **2022** | `example_fr` | **C** | C'est une voix intérieure qui m'a ordonné de faire cela... | C'est une voix intérieure qui m'a ordonné de faire cela… |
| `7.1.2.2` | 1291 | v3 | `example_fr` | **C** | Mon chien fouille dans les poubelles parce que... c'est un chien ! | Mon chien fouille dans les poubelles parce que… c'est un chien ! |
| `7.3.1.1` | 1362 | **2022** | `text_fr` | **C** | Tu Quoque | Tu quoque |
| `7.3.2.3.2` | 1388 | v3 | `text_fr` | **C** | Attaque de la Confiance en soi | Attaque de la confiance en soi |

---

## 8. ⚠️ Les autres types de cartes — mesuré, ⛔ pas supposé

| Type | Archive fév. 2022 dans le dépôt | Clé `path` valide ? | Diffable contre l'imprimé ? |
|---|---:|---|---|
| **Fallacies** | 70 (+28 P&P) + v3 169 | ✅ `Famille` 153/153 | ✅ **fait** — ce document |
| **Scenarii** | **77** (+27 P&P) + v3 27 | ⛔ **NON** — `path` réattribué | ⚠️ oui, **avec une autre clé** |
| **Rules** | **6** — une seule variante (*The Smooth Talker*), `Text`/`Text_en` | — | ⚠️ **partiel** : 6 des 15 cartes |
| **Virtues** | **0** | — | ⛔ **aucune référence imprimée** |
| **Memo** | **0** | — | ⛔ **aucune référence imprimée** |

### Scenarii — pourquoi `path` ne marche pas, et ce que ça coûte

Mesuré : *« Napoléon et la campagne de Russie »* n'a pas été réécrit, il est **passé de `1.3.1` à `1.2.3`**. Sur 56 cartes appariées par titre normalisé, **38 seulement gardent leur `path`** ⇒ **32 % de réattribution**.

⛔ **Une jointure `path` sur Scenarii produit donc des « réécritures » qui sont en réalité des cartes différentes comparées entre elles.** Le chiffre qu'elle rend (308 cellules) est un **artefact**, il n'est pas reporté ici.

⭐ **La signature de la panne était dans la sortie de l'instrument lui-même** : `catégorie` concordait **63/63** pendant que `sous-catégorie` tombait à **16/63**. Un corpus ne réécrit pas 3 sous-catégories sur 4 en gardant 100 % des catégories — *c'était la clé qui cassait, pas le corpus qui dérivait.* ⇒ **Avant de publier un compte de divergences, exiger un invariant qui survit à une réécriture mais pas à une re-clé.**

⇒ Scenarii reste mesurable, mais la clé doit être **le titre normalisé** (56/77 appariées), avec sa propre limite : les cartes **renommées** y deviennent invisibles, exactement comme les cartes redéplacées le sont avec `path`.

---

## 7. Ce qui est attendu

1. **Arbitrer la tête du tableau 6.1**, pas les 282 lignes. L'ampleur décroît vite ; un seuil suffit.
2. **Trancher la règle par défaut.** Le mandat owner du 22/09 pose *le retour à l'imprimé* comme défaut, la charge de la preuve pesant sur le changement. ⚠️ Appliqué littéralement, cela touche 282 cellules FR **plus leur cascade** dans 7 langues ⇒ régénération des 8 langues (≈ 3 h 25, barème #1469).
3. **Dire si le périmètre s'étend.** Le §8 mesure ce qui est **possible** : Scenarii oui (autre clé), Rules partiellement (6 cartes sur 15), Virtues et Memo **pas du tout** — aucune référence imprimée n'existe pour eux.

⚠️ **Contrainte politique, ⛔ pas décorative** — verbatim owner : *« Thomas est assez susceptible et dubitatif sur notre usage de l'IA […] on a déjà joué des centaines de fois avec ces cartes […] trop de changement gratuit, et erroné de surcroît, aucune chance que ça passe à la relecture. »* ⇒ **Le volume non trié est lui-même le risque** : 20 changements défendables passent, 282 non triés font rejeter l'ensemble, y compris les bons.

---

*Mesuré par `myia-ai-01` (coordinateur) le 2026-09-22. Instrument et contrôles inverses reproductibles — la jointure se refait en une passe sur `path`.*
