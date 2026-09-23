# #1499 grain G2 — Dossier d'arbitrage Fallacies : champ `example_fr` (PR autonome)

**Date** : 2026-09-23 · **Lane** : po-2024 (worker) · **Base** : `origin/master` `9fe92572`
**Instrument** : [`docs/corpus/fallacies-exemples-diff.py`](../corpus/fallacies-exemples-diff.py)
— exécuté **tel que livré** avant commit (rc=0, témoins a–d PASS, sortie `fallacies-exemples-diff.json` non committée).
**Dispatch** : pool #458 c.5784688699 — « G2 (145 `example_fr`) ».

**⛔ Garde centrale respectée** : aucune écriture dans le CSV. Dossier d'arbitrage seulement — la décision appartient à l'owner, cellule par cellule.

---

## Réponse en une ligne

G2 mesure **132 cellules `example_fr` non-IDENT** sur les cartes imprimées HEAD, dont **111 forment la surface d'arbitrage agentique** (geste postérieur à la baseline 2024, classe SUBST). Les 21 autres : 12 pré-agentiques (geste d'époque owner, déjà en HEAD en 2024) et 9 typographiques. Le « 145 » du dispatch était une mesure **pré-erratum #1516** ; le ledger post-erratum dit **116** ; la différence 132 − 116 s'explique intégralement (passe 2 = +7, contrôle 5 élargi = +9, cf. §4).

---

## 1. Références et méthode

| Référence | Rôle |
|---|---|
| `Archive/2022/...edition fevrier 2022.csv` (70 cartes) | **l'imprimé** — autorité maximale |
| `Archive/v3/...Cards.csv` (169 cartes) | deck pré-agentique, étage 2 de l'imprimé |
| `62b561e75` (22/04/2024, 1408 lignes) | baseline canonique — référence des **couches** (⛔ pas l'imprimé) |
| HEAD (1408 lignes) | corpus courant |

- **Passe 1** (125 lignes) : imprimé (étage 2022 > v3) → HEAD par `path`, filtre `carte ≠ vide`, avec le **contrôle 5** (ré-appariement des sœurs renumérotées par titre, sens archive → HEAD uniquement — motif #1516). **16 jointures** ré-appariées.
- **Passe 2** (7 lignes) : PK baseline imprimées → HEAD pour les PK non couvertes par `path` — les **sans-référence archive** (#1507 : 105, 362, 1020, 1092, 1120) et les cartes v3 sous un autre `path` (598, 603).
- **Couche par PK contre la baseline** : `agentique` (baseline ≠ HEAD) / `pre-agentique` (baseline = HEAD, geste d'époque) / `typo-seul-agent` (baseline → HEAD = typographique seul) / `hors-deck`.
- **Contrôle 6** (motif ledger) : un SUBST dont le texte vit verbatim ailleurs est un **déplacement**, pas une réécriture — **3 cas** : PK 633, 636, 844.

## 2. Ventilation (132 cellules)

| classe × couche | agentique | pré-agentique | typo-seul-agent | TOTAL |
|---|---:|---:|---:|---:|
| SUBST | **111** ⭐ | 12 | 3 | 126 |
| C (typographique) | 0 | 0 | 6 | 6 |
| TRONQ / VIDE / AJOUT / FILL | 0 | 0 | 0 | 0 |
| **TOTAL** | **111** | 12 | 9 | **132** |

⭐ **Surface d'arbitrage = 111** (couche agentique × SUBST). Aucun TRONQ (texte amputé), aucun VIDE (cellule vidée), aucun AJOUT/FILL : l'intégralité de la surface est de la **reformulation ou du remplacement**.

## 3. Ce que la mesure révèle qualitativement (lecture humaine, SUPPOSÉ en granularité)

La classe mécanique SUBST mélange au moins **trois régimes** très différents, que seul l'œil sépare :

1. **Modernisation du registre** (tutoiement → vouvoiement, resserrement) — l'exemple dit la même chose : PK 79 (« *Ton* opinion… quand *tu* auras » → « *Votre* opinion… quand *vous* aurez »), PK 337 (« Ne *t*'éloigne pas… le loup *te* dévorera » → « Ne *vous* éloignez pas… *vous* dévorera »), PK 182 (« Pile je gagne. Face tu perds. » → « Pile, je gagne. Face, vous perdez. »).
2. **Remplacement total** de l'exemple — la carte illustre le même sophisme avec une autre scène : PK 2 (permis de conduire → crotte de chien), PK 112 (couple homosexuel → enfants innocents), PK 247 (appel de De Gaulle → vallée fleurie).
3. **Réécriture didactique** avec explication incorporée : PK 796, PK 834 (cf. ledger #1503 §6.1).

Un « retour à l'imprimé » uniforme traiterait les trois régimes pareil — c'est précisément le tri que la contrainte politique (cf. G1 §11) interdit de présenter en bloc.

## 4. Réconciliation des trois chiffres (MESURÉ)

| Chiffre | Source | Définition | Écart vs 132 |
|---|---|---|---|
| **145** | dispatch v12 (22/09 13:44Z) | mesure **pré-erratum #1516** du ledger (jointure `path` sans ré-appariement sœurs) | pré-erratum, superseded |
| **116** | ledger #1503 post-erratum (`ledger-cartes-existantes-2026-09-22.md` : 110 SUBST + 6 C) | jointure `path` + contrôle 5 du ledger, **sans passe 2** | −16 |
| **132** | **cette mesure** | passe 1 (contrôle 5 élargi : 16 réparations) + passe 2 (PK baseline) | — |

Décomposition du 132 − 116 = **+16** : **+7** lignes de passe 2 (PK 105, 362, 598, 603, 1020, 1092, 1120 — invisibles d'une jointure `path` pure), **+9** lignes de passe 1 créées par le contrôle 5 élargi (l'imprimé ré-attaché par titre à une sœur renumérotée diffère de celui que la jointure `path` naïve comparait). ⛔ Aucune de ces différences n'est un défaut : ce sont **trois définitions de périmètre**, chacune cohérente avec sa référence.

---

## 5. Tableau d'arbitrage — surface agentique (111 cellules, extraits)

Format : `PK | path | p | référence (p1 = imprimé 2022/v3, p2 = baseline) | HEAD | étage`. Extraits tronqués à 72 caractères ; textes complets dans la sortie JSON de l'instrument (non committée, reproductible en une passe).

| PK | path | p | référence (p1 = imprimé, p2 = baseline) | HEAD | étage |
|---:|---|---:|---|---|---|
| 2 | 1.1 | 1 | Depuis l'obtention de mon permis de conduire, j'ai eu trois accidents ;  | Hier, j'ai marché dans une crotte de chien après avoir salué un passant  | v3 |
| 33 | 1.1.2 | 1 | J'ai entendu dire que cette personne était sexiste ; il m'est donc impos | Je conviens que votre calcul est correct. Mais votre raisonnement est in | v3 |
| 43 | 1.1.2.2 | 1 | Tout le monde dépasse les limites de vitesse; cela ne devrait donc pas ê | Tout le monde dépasse les limites de vitesse ; cela ne devrait donc pas  | 2022 |
| 55 | 1.1.3 | 1 | - Si les extraterrestres existaient, on en aurait vus ! - On ne les voit | Si on ne voit pas les extraterrestres, c'est parce qu'ils rendent amnési | v3 |
| 70 | 1.2 | 1 | Laisse donc ton père ouvrir ce pot de cornichons: c'est une affaire d'ho | Laissez donc votre père ouvrir ce pot de cornichons : c'est une affaire  | v3 |
| 71 | 1.2.1 | 1 | Bien qu'ayant trop chaud, je n'enlève pas mon manteau, car maman a dit q | Même si j'ai trop chaud, je n'enlève pas mon manteau, car maman a dit qu | 2022 |
| 79 | 1.2.1.2.1 | 1 | Ton opinion sur la musique m'intéressera quand tu auras sorti un album ! | Votre opinion sur la musique m'intéressera quand vous aurez sorti un alb | 2022 |
| 98 | 1.2.2.2 | 1 | La plupart des enfants réclament tous les jours des frites à la cantine; | La plupart des enfants réclament tous les jours des frites à la cantine  | 2022 |
| 104 | 1.2.2.3 | 1 | La corrida a toujours existé en France; il serait donc stupide d'interdi | La corrida a toujours existé en France ; il serait donc stupide d'interd | 2022 |
| 105 | 1.2.2.3.1 | 2 | Aucun bâtiment de ce type n'a jamais résisté à un tremblement de terre d | Aucun bâtiment de ce type n'a jamais résisté à un tremblement de terre d | baseline |
| 108 | 1.2.2.4 | 1 | Il est normal que chez les hommes, comme chez la plupart des animaux, le | Il est normal que, chez les êtres humains comme chez la plupart des anim | 2022 |
| 112 | 1.2.3 | 1 | Il est choquant de voir un couple d'hommes ou de femmes s'embrasser dans | Ce sont des enfants innocents. Ils ne pourraient pas causer autant de pr | v3 |
| 121 | 1.2.3.3.2 | 1 | Ne cherchez pas d'explication du côté des croyances; la religion est aff | Ne cherchez pas d'explication du côté des croyances ; la religion relève | 2022 |
| 128 | 1.2.3.5.1 | 1 | S'il avait tort, il n'en serait pas parvenu là aujourd'hui. | S'il avait tort, il ne serait pas arrivé là où il en est aujourd'hui. | v3 |
| 134 | 1.3.1 | 1 | Il s'est beaucoup entraîné sur l'ordinateur, il ne peut pas rater son pe | Gérer une entreprise n'est pas si différent d'un jeu vidéo de stratégie  | v3 |
| 153 | 1.3.2 | 1 | Cet homme a tout intérêt me vendre sa voiture, sa description du véhicul | Cet homme a tout intérêt à me vendre sa voiture. Sa description du véhic | v3 |
| 154 | 1.3.2.1 | 1 | Il m'a dit d'emprunter la route qui descend sur la droite; or cette rout | Il m'a dit d'emprunter la route qui descend sur la droite ; or cette rou | 2022 |
| 165 | 1.3.3 | 1 | Où sont mes clefs ? Quelqu'un a dû me les voler. C'est une conspiration  | Où sont mes clés ? Quelqu'un a dû me les voler. Il y a sûrement un compl | 2022 |
| 176 | 2.1 | 1 | "Si votre ramage se rapporte à votre plumage, vous êtes le phénix des hô | Votre éloquence est à la hauteur de votre prestance : vous êtes vraiment | v3 |
| 182 | 2.1.1.1.3 | 1 | Pile je gagne. Face tu perds. | Pile, je gagne. Face, vous perdez. | 2022 |
| 184 | 2.1.1.3 | 1 | Cet homme est un athée, quelqu'un qui n'a pas été touché par la grâce de | Cet homme est athée : il n'a pas été touché par la grâce de Dieu. | v3 |
| 185 | 2.1.1.4 | 1 | Ne cherche pas plus loin: c'est la vie avec ses joies et ses peines. | Nous devons aussi appliquer une politique de tolérance zéro aux enfants  | 2022 |
| 219 | 2.1.2 | 1 | Je ne suis pas vieux, voyons ! Je suis tout simplement jeune depuis plus | Je ne suis pas vieux, voyons ! Je suis simplement jeune depuis plus long | v3 |
| 247 | 2.1.3 | 1 | Paris ! Paris outragé ! Paris brisé ! Paris martyrisé ! Mais Paris libér | Dans la vallée, où chaque fleur exhale un soupir d'amour, notre candidat | v3 |
| 299 | 2.2 | 1 | Rapporte-nous un bon bulletin de notes, et ton père sera fier de toi. | Si vous avez de bonnes notes, votre père sera très fier de vous. | 2022 |
| 319 | 2.2.1.4 | 1 | Mesdames et messieurs du jury, regardez cet homme misérable, dans un fau | Mesdames et messieurs les jurés, regardez cet homme misérable, en fauteu | 2022 |
| 322 | 2.2.2 | 1 | Ce n'est tout de même pas une poignée de jeunes imbéciles qui va vous ap | Les collapsologues veulent nous ramener à l'âge de pierre et abolir tout | v3 |
| 323 | 2.2.2.1 | 1 | Mon fils, tu ne feras pas tes études en fac de Lettres, ramassis notoire | Mon fils, vous ne ferez pas vos études en faculté de lettres, ce ramassi | v3 |
| 337 | 2.2.2.4 | 1 | Ne t'éloigne pas du chemin ou le loup te dévorera ! | Ne vous éloignez pas du chemin, ou le loup vous dévorera ! | 2022 |
| 340 | 2.2.3 | 1 | Je ne donne jamais d'argent aux mendiants car je crains qu'ils n'en prof | Admettons que ce diagnostic soit exact : il faudrait tout arrêter, préve | v3 |
| 343 | 2.2.3.1.2 | 1 | Si tu ne me laisses pas la dernière tranche de jambon, tu le paieras trè | Si vous ne me laissez pas la dernière tranche de jambon, vous le paierez | 2022 |
| 356 | 2.3 | 1 | Pour des raisons de sécurité, veuillez nous communiquer l'identifiant et | N'oubliez pas : il y a ceux qui ont le privilège de choisir et ceux qui  | v3 |
| 357 | 2.3.1 | 1 | Donner le pouvoir à un banquier, c'est comme guérir un alcoolique avec d | Quand vous voyez notre logo, vous pensez aussitôt à la liberté et à l'av | v3 |
| 361 | 2.3.1.1.1.2 | 1 | Les parents ont le devoir d'éduquer leurs enfants et de subvenir à leurs | Les parents ont le devoir d'éduquer leurs enfants et de subvenir à leurs | v3 |
| 362 | 2.3.1.1.1.2.1 | 2 | Ton travail sur le projet a été impressionnant, vraiment. Par contre, il | Votre travail sur le projet a été impressionnant. En revanche, il y a eu | baseline |
| 420 | 2.3.2 | 1 | Si ton amour pour moi est sincère, alors tu comprendras ce que je vais t | Si votre amour pour moi est sincère, alors vous comprendrez ce que je va | v3 |
| 421 | 2.3.2.1 | 1 | Après une si belle entrée en matière, je suis sûr que nous parviendrons  | Après une si belle entrée en matière, je suis sûr que nous parviendrons  | v3 |
| 432 | 2.3.2.2.1 | 1 | Puisque les bonnes ménagères que vous êtes avez été convaincues par nos  | Puisque vous êtes favorables à la transition écologique, que ceux d'entr | v3 |
| 596 | 3.1.1 | 1 | Ce yaourt est le meilleur, puisqu'il a été plébiscité par quatre-vingts  | Ce yaourt est le meilleur, puisqu'il a été plébiscité par 80 % des conso | v3 |
| 598 | 3.1.1.1.1 | 2 | Avez-vous vu l'augmentation des cas de fraude chez les nouveaux employés | Le sondage a été réalisé à la sortie du congrès : 90 % des personnes int | baseline |
| 603 | 3.1.1.2.1 | 2 | Maman n'a pas dit non, j'ai eu une bonne note et ce bonbon est rose, ma  | Maman n'a pas dit non, j'ai eu une bonne note, et ce bonbon est rose, ma | baseline |
| 614 | 3.1.2 | 1 | Ma voiture peut rouler à cent trente kilomètres à l'heure ; traverser la | Ma voiture peut rouler à 130 kilomètres à l'heure ; traverser la ville n | 2022 |
| 621 | 3.1.3 | 1 | Un camion consomme plus de carburant qu'une voiture ; on peut en déduire | Un camion consomme plus de carburant qu'une voiture ; on peut donc en dé | v3 |
| 625 | 3.1.3.2 | 1 | Les tigres royaux sont voués à disparaître prématurément; ce tigre royal | Les tigres royaux sont voués à disparaître prématurément ; ce tigre roya | v3 |
| 632 | 3.2 | 1 | Les apparitions de Nicolas Cage au cinéma ces dix dernières années coïnc | La courbe du réchauffement climatique suit de très près la baisse de la  | v3 |
| 644 | 3.2.2 | 1 | L'ADN laissé par l'auteur de ce crime présente une particularité que l'o | L'ADN laissé par l'auteur de ce crime présente une particularité que l'o | v3 |
| 653 | 3.2.2.3.1 | 1 | Mieux vaut ne rien entreprendre : rien ne me réussit ces temps-ci ! | J'ai marqué mes trois derniers paniers : je suis en réussite, donc je va | 2022 |
| 658 | 3.2.3 | 1 | Admettons que Dieu ait créé l'homme ; dans ce cas, qui a créé Dieu ? | Cette affirmation est vraie. - Mais comment le savez-vous ? Je l'ai véri | 2022 |
| 666 | 3.3 | 1 | L'accélération moyenne de la voiture A est supérieure à celle de la voit | L'accélération moyenne de la voiture A est supérieure à celle de la voit | v3 |
| 673 | 3.3.1.2.2 | 1 | Tu préfères le lundi et moi le mercredi ? Coupons la poire en deux et vo | Vous préférez le lundi et moi le mercredi ? Coupons la poire en deux et  | 2022 |
| 677 | 3.3.1.3.1 | 1 | Si nous le laissons se resservir de ce plat, il prendra l'habitude de ma | Si nous le laissons se resservir de ce plat, il prendra l'habitude de tr | 2022 |
| 681 | 3.3.2 | 1 | Sachant qu'un croissant coûte un euro et cinquante centimes, avec dix eu | Sachant qu'un croissant coûte un euro et cinquante centimes, avec dix eu | 2022 |
| 690 | 3.3.3 | 1 | Dans la fable de La Fontaine, chaque fois que le lièvre avance, la tortu | Dans la fable de La Fontaine, chaque fois que le lièvre avance, la tortu | v3 |
| 707 | 4.1.2 | 1 | Il est dangereux d'utiliser un fauteuil roulant, puisque la plupart des  | Il est dangereux d'utiliser un fauteuil roulant, puisque la plupart des  | 2022 |
| 713 | 4.1.2.5 | 1 | - Pourquoi refuses-tu de me rendre ma trottinette ? - Parce que tu m'as  | Le patron ne paie pas ses impôts, alors je ne vais pas me gêner pour me  | v3 |
| 726 | 4.2 | 1 | Plus il y a de gruyère, plus il y a de trous ; or plus il y a de trous,  | Plus il y a de fromage, plus il y a de trous ; or plus il y a de trous,  | v3 |
| 727 | 4.2.1 | 1 | Il ne peut pas pleuvoir et neiger en même temps ; du coup, s'il ne pleut | Il ne peut pas pleuvoir et neiger en même temps ; donc, s'il ne pleut pa | v3 |
| 733 | 4.2.1.3.1 | 1 | Prendrez-vous du fromage ou un dessert ? | Pour réussir dans ce milieu, il faut être très riche ou très beau. Je su | 2022 |
| 735 | 4.2.2 | 1 | Tous les philosophes sont sages ; cependant quelques-uns sont idiots. | Tous les philosophes sont sages ; pourtant, certains philosophes sont id | 2022 |
| 740 | 4.2.2.2 | 1 | Il est faux que tous les chiens sont blancs, donc il n'y a pas de chiens | Il est faux que tous les chiens soient blancs ; donc il n'existe aucun c | v3 |
| 750 | 4.2.3 | 1 | Maman prétend que je ne peux pas manger des bonbons après m'être brossé  | Maman prétend que je ne peux pas manger de bonbons après m'être brossé l | 2022 |
| 768 | 4.3.1.2 | 1 | Tu me demandes de faire mes devoirs après l'école; mais je ne te demande | Vous me demandez de faire mes devoirs après l'école ; mais moi, je ne vo | v3 |
| 781 | 4.3.2.2.1 | 1 | Premièrement je n'ai absolument pas emprunté de chaudron à B ; deuxièmem | Premièrement, je n'ai absolument pas emprunté de chaudron ; deuxièmement | v3 |
| 796 | 4.3.3.3.1 | 1 | Socrate est un homme ; les hommes sont mortels ; les chats sont mortels  | Tous les avocats défendent des clients au tribunal. Ce fruit est un avoc | v3 |
| 800 | 5.1.1 | 1 | Ma liberté d'expression m'autorise à menacer cet homme. | Quand j'ai dit que je serais là dans cinq minutes, cela pouvait aussi vo | v3 |
| 802 | 5.1.1.2 | 1 | Ce foulard n'est pas violet, il est mauve. | Le concept de " qualité " est si vaste qu'il est difficile à définir pré | 2022 |
| 804 | 5.1.2 | 1 | L'amour, c'est savoir pardonner. Du coup, comment oses-tu me reprocher m | L'amour, c'est savoir pardonner. Alors, comment osez-vous me reprocher m | v3 |
| 808 | 5.1.2.2 | 1 | Je t'adore, donc je ne t'aime pas | Je vous adore, donc je ne vous aime pas. | v3 |
| 809 | 5.1.2.2.1 | 1 | Les atomes font partie de la nature ; cette poudre chimique est composée | Les atomes font partie de la nature ; cette substance chimique est compo | 2022 |
| 813 | 5.1.2.2.3 | 1 | - Tous les Écossais sont roux. - Angus est écossais mais il n'est pas ro | - Tous les Écossais sont roux. - Angus est écossais, mais il n'est pas r | 2022 |
| 814 | 5.1.2.2.4 | 1 | Si les nouvelles technologies ne t'intéressent pas, rien ne t'empêche de | Si les nouvelles technologies ne vous intéressent pas, rien ne vous empê | 2022 |
| 826 | 5.1.3 | 1 | Une bonne éducation se doit d'éveiller l'esprit critique ; or un individ | Une bonne éducation doit éveiller l'esprit critique ; or une personne do | v3 |
| 833 | 5.2 | 1 | L'école, c'est comme la prison. On n'y entre pas comme on veut. | L'école, c'est comme la prison : on n'y entre pas et on n'en sort pas co | v3 |
| 834 | 5.2.1 | 1 | Les lycéens sont comme une meute de chiens féroces : il convient de leur | Gérer une classe, c'est comme piloter une machine : si une pièce ralenti | 2022 |
| 839 | 5.2.2 | 1 | Les cerneaux de noix ressemblent à de petits cerveaux ; un noyer doit do | Personne n'empêche un chirurgien de consulter ses notes pendant une inte | 2022 |
| 845 | 5.2.3.1 | 1 | Son prénom est Yannick : il doit être breton ; aucune chance donc qu'il  | Vous êtes végétarien ? Nous n'avons pas besoin de militant écologiste ic | 2022 |
| 848 | 5.3.1.1 | 1 | Mes élèves qui sont en surpoids boivent beaucoup de soda. | Les membres du comité qui ont validé ce dossier seront convoqués. - Rest | v3 |
| 876 | 5.3.3 | 1 | Lundi dernier, tu as rendu ton devoir à temps ; aujourd'hui, il faut qu' | Lundi dernier, vous avez rendu votre devoir à temps ; aujourd'hui, il fa | v3 |
| 878 | 5.3.3.1.1 | 1 | Tu étais seul dans la pièce, et je ne trouve plus mon portefeuille. | Vous étiez seul dans la pièce, et je ne trouve plus mon portefeuille. | v3 |
| 888 | 6.1 | 1 | Je ne suis pas responsable de cette situation : je t'ai dit ce que je sa | Je ne suis pas responsable de votre décision : je vous ai dit que le pro | v3 |
| 908 | 6.1.1.2 | 1 | Les élites dirigeatntes ont voulu toutes ces calamités pour mieux contrô | On sait tous que les pyramides ont été construites par des esclaves. - L | v3 |
| 942 | 6.1.2 | 1 | Un comité scientifique trié sur le volet a déclaré que cette crème pour  | Un " comité scientifique " dont personne ne connaît les membres affirme  | 2022 |
| 943 | 6.1.2.1 | 1 | - Tu m'as pourtant dit avoir trouvé ce film éblouissant... - Éblouissant | - Vous m'avez pourtant dit avoir trouvé ce film éblouissant... - Éblouis | 2022 |
| 953 | 6.1.3 | 1 | Le vin est un produit excellent pour la santé car il contient des antiox | Le vin est excellent pour la santé : il contient des antioxydants. Inuti | 2022 |
| 973 | 6.2 | 1 | Ce qui me tenait le plus à cœur n'était pas de gagner ce match mais d'of | Vous aviez demandé une étude publiée pour être convaincu. Lorsqu'on vous | 2022 |
| 974 | 6.2.1 | 1 | - Tu ne sais pas conduire. - Mais voyons, j'ai mon permis de conduire !  | - Vous ne savez pas conduire. - Mais voyons, j'ai mon permis de conduire | v3 |
| 989 | 6.2.1.4 | 1 | Si vous ne pouvez pas me prouver que le monstre du Loch Ness n'existe pa | Moi, prouver que mon produit est sans danger ? C'est plutôt à vous de mo | 2022 |
| 992 | 6.2.2 | 1 | Si j'ai par mégarde heurté quelqu'un dans cette assistance, je tiens à l | Si j'ai offensé quelqu'un dans cette assemblée, même par inadvertance, j | v3 |
| 1004 | 6.2.2.5 | 1 | Je n'ai pas dit que je voulais abandonner ce projet, voyons, mais simple | Je n'ai pas dit que je voulais abandonner ce projet, voyons ; j'ai simpl | v3 |
| 1020 | 6.2.3.4 | 2 | Nous ne pouvons pas abandonner le chantier maintenant, réfléchissez à to | Nous ne pouvons pas abandonner le chantier maintenant : pensez à toutes  | baseline |
| 1023 | 6.3 | 1 | Ces personnes-là se ressemblent toutes : comment font-elles pour se reco | Pour moi, tous les membres de ce groupe se ressemblent ; comment font-il | v3 |
| 1024 | 6.3.1 | 1 | La nature est à ce point pleine de sagesse qu'elle a fait traverser chaq | La nature est si sage qu'elle a fait passer une rivière dans chaque vill | v3 |
| 1092 | 6.3.1.2.1.1 | 2 | Je déteste perdre plus que j'aime gagner. | Le rapport indique que la nouvelle politique a amélioré neuf indicateurs | baseline |
| 1120 | 6.3.1.2.2.1 | 2 | Si je n'ai pas eu un A à mon examen, alors je suis complètement nul en c | Soit j'ai la moyenne, soit je suis complètement nul. - Dichotomie : il n | baseline |
| 1174 | 6.3.2 | 1 | Ces personnes sont bien à plaindre, sans télévision ni voiture... | Ces personnes sont bien à plaindre : elles n'ont ni télévision ni voitur | v3 |
| 1242 | 6.3.3 | 1 | En cuisine, il n'y a que l'huile d'olive qui vaille. Je fais donc ma vie | En cuisine, seule l'huile d'olive vaut vraiment la peine d'être utilisée | 2022 |
| 1281 | 7.1 | 1 | Mes positions sont claires, connues de tous, et elles ne changeront pas. | Mes positions sont claires, connues de tous, et je n'ai pas à en débattr | v3 |
| 1282 | 7.1.1 | 1 | C'est peut-être vrai pour toi, mais ça ne l'est pas pour moi. | C'est peut-être vrai pour vous, mais cela ne l'est pas pour moi. | 2022 |
| 1287 | 7.1.2 | 1 | Pourquoi la mer est bleue ? Parce que les dauphins pleurent. | Peu importe vos arguments : la vraie explication, c'est que nous n'avons | v3 |
| 1297 | 7.1.3 | 1 | Vous pouvez me croire sur parole : en deux semaines, j'enseignerai la ph | Il est évident que notre futur produit sera un succès mondial ; nul beso | v3 |
| 1301 | 7.1.3.3 | 1 | Les produits laitiers sont nos amis pour la vie. Ils sont excellents pou | Les produits laitiers sont nos amis pour la vie. Ils sont excellents pou | 2022 |
| 1312 | 7.2 | 1 | Allons donc dans mon bureau pour discuter de la stupidité de votre deman | Allons plutôt dans mon bureau discuter de l'absurdité de votre demande d | v3 |
| 1313 | 7.2.1 | 1 | Avant de me reprocher mon retard, peux-tu m'expliquer pourquoi tu te pro | C'est une question intéressante, mais examinons d'abord le contexte plus | 2022 |
| 1314 | 7.2.1.1 | 1 | Avant de me reprocher mon retard, peux-tu m'expliquer pourquoi tu te pro | Avant de me reprocher mon retard, pouvez-vous m'expliquer pourquoi vous  | 2022 |
| 1330 | 7.2.1.2.2 | 1 | J'ai eu une mauvaise note, mais on a volé mes billes: Jean a gagné la pr | J'ai eu une mauvaise note, mais on a volé mes billes : Jean a gagné la p | 2022 |
| 1355 | 7.2.3.3 | 1 | Je ne lui fais pas confiance car il me déteste. | Il n'est pas surprenant que vous m'accusiez de fuir le sujet : après tou | v3 |
| 1362 | 7.3.1.1 | 1 | Comment peut-on lire ce que Jean-Jacques Rousseau peut écrire sur l'éduc | Comment peut-on prendre au sérieux ce que Jean-Jacques Rousseau écrit su | 2022 |
| 1365 | 7.3.1.2 | 1 | La cuisine bretonne, ce n'est que du beurre. Je ne mange pas du beurre à | Ah, donc selon vous, nous devrions passer toutes nos journées à fouiller | 2022 |
| 1371 | 7.3.2 | 1 | Je ne vais pas prendre les conseils d'un américain pour choisir mon vin  | Je ne vais tout de même pas suivre les conseils d'un Américain pour choi | 2022 |
| 1388 | 7.3.2.3.2 | 1 | Si tu refuses de parier ta maison pour soutenir qu'il faut tourner à dro | Vous semblez nerveux en présentant votre projet ; vous n'y croyez peut-ê | v3 |
| 1398 | 7.3.3 | 1 | Vous ne pouvez prétendre que la Terre tourne autour du Soleil, vous n'êt | Ne me dites pas comment hacher l'ail. Un toxicomane ne va pas m'apprendr | 2022 |

---

## 6. Hors surface (21 cellules)

### Pré-agentiques (12) — geste d'époque owner, HEAD = baseline 2024 déjà

PK 633, 636, 698, 729, 787, 844, 889, 900, 1015, 1345, 1360, 1373. Un « retour à l'imprimé » y reviendrait sur un geste owner d'époque (2022→2024) — ⛔ pas un non-geste (leçon G1 v3). HEAD = baseline 2024 pour ces 12, **modulo l'apostrophe typographique (`'` → `’`) pour 8/12** — PK 636, 698, 729, 787, 900, 1015, 1360, 1373 (re-mesure ai-01, review #1518) ; les 4 autres (633, 844, 889, 1345) sont identiques octet à octet. À présenter à part seulement sur demande.

### Typo-seul-agent (9) — à garder sans arbitrage

PK 3, 667, 708, 994, 1288, 1291 (C purs) et PK 300, 304, 1361 (SUBST nominal dont baseline → HEAD est typographique seul).

### Contrôle 6 — déplacements verbatim (3)

PK **633, 636, 844** : le texte HEAD vit verbatim sur une autre carte d'archive (motif `3.2.1`↔`3.2.1.3`, `5.2.3` du ledger) — déplacement pré-agentique, **pas une réécriture**. Restent classées SUBST et signalées (motif ledger inchangé). ⚠️ 633 et 844 figurent AUSSI en pré-agentiques ci-dessus : les deux classifications sont vraies simultanément, DÉPLACÉ prime à la lecture.

## 7. Témoins instrument (4 PASS, rc=0)

| # | Contrôle | Mesure | Verdict |
|---|---|---|---|
| a | Archives lisibles (2022 = 70, v3 = 169) | 70 / 169 | PASS |
| b | Baseline imprimées ∩ HEAD par PK = 175 (= 176 − PK 96 #1288) | 175 | PASS |
| c | PK 598/603/680 couverts (écart mesuré ou IDENT baseline) | 3/3 couverts (598, 603 en écart ; 680 IDENT baseline) | PASS |
| d | Témoin positif path 2.1 (PK 176) : example_fr non-IDENT | rendu (SUBST) | PASS |

Contrôle 5 : 16 jointures sœurs ré-appariées. Contrôle 6 : 3 déplacements verbatim.

## 8. Ce que ce dossier **n'établit pas**

- ⛔ La valeur éditoriale d'aucune cellule — SUBST est une mesure de forme (leçon ledger #148 : « beaucoup sont probablement des améliorations »).
- ⛔ La qualification fine des 111 en trois régimes (§3) : nominative, **SUPPOSÉE** en granularité — seul un passage cellule par cellule les compte.
- ⚠️ La chronologie agentique : la vague dominante est probablement `9d45b4f9` (PR #369, 28/05/2026, « 744/1408 example_fr ») mais chaque cellule daterait par `git log -S` individuellement.
- ⛔ Ne couvre pas `desc_fr` (G3), ni `text_fr` (G1, mergé), ni Virtues/Rules/Scenarii.

## 9. Ce qui est attendu — rien : la règle est posée, la suite est G2-C

**Règle générale owner (23/09 12:35Z, [#1499 c.5794911968](https://github.com/ArgumentumGames/Argumentum/issues/1499#issuecomment-5794911968), reprise [#458 c.5794922231](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5794922231))** : *« Si des choses ont progressé on les garde meilleures donc C on fait du cas par cas, pas que pour les vertues »* — valable pour tous les dossiers G, G2 compris, tranchée **avant** l'ouverture de ce dossier.

1. **Défaut = garder HEAD** : les 111 cellules restent telles quelles sauf décision contraire, item par item.
2. **La suite est G2-C** (calqué sur G4-C) : parmi les 111, relever uniquement ce qui mérite de revenir — perte d'un fait, d'une nuance ou d'un exemple mieux ciblé ; erreur introduite (contresens, exemple qui n'illustre plus le sophisme de la carte) ; exemple devenu trop long. Une ligne par item, lisible sans ouvrir l'issue. Le passage tutoiement → vouvoiement (§3.1) est un changement **systématique**, à compter et signaler une seule fois, pas item par item. ⛔ Aucune écriture CSV : G1-W écrit dans le même fichier.
3. **Les 3 déplacements verbatim** (633/636/844) et les **12 pré-agentiques** sortent de tout geste par construction.
4. G9 (clé Scenarii) reste gaté sur l'arbitrage de G2 + G3 — ce dossier couvre la moitié Fallacies-exemples de ce gate.

---

*Mesuré par `myia-po-2024` (worker lane) le 2026-09-23. Instrument stdlib, lecture seule stricte, rc=0 exécuté tel que livré avant commit. La mesure se refait en une passe (`path` + contrôle 5) + une passe (PK baseline).*
