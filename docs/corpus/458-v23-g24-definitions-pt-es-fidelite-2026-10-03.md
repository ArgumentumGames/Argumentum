# Pool v23 — série ㉔ : fidélité des **définitions** portugaises et espagnoles du deck (mesure 0-écriture)

**Base** : branche `docs/458-g23-definitions-fa-fidelite` (série ㉓, chaîne #1713 → #1715 → #1717 → #1718 → ce grain). **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py) — rejouable, `--self-test` (6 contrôles). **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Cinquième grain de la série ⑳-㉕, les deux langues ibériques en un dossier. Ce grain est le premier depuis le ⑳ (`ru`) où une colonne d'archive **locale** existe : `desc_pt` est présent dans toutes les archives (**168/175 exploitable**) — l'arbitrage imprimé pt↔pt devient mécanique. L'espagnol, lui, n'a aucune colonne (`0/175`, comme zh/ar/fa). Et le grain livre une trouvaille : **l'archive `desc_pt` elle-même est un artefact MT de faible qualité** — la référence « imprimée » portugaise n'est PAS un étalon de fidélité.

## Synthèse — 0 A / 0 M / 55 rangées observées / 295 ✓ (2×175)

> **Erratum d'arbitrage (03/10, ai-01, [c.5966971153](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5966971153)).** Les frontières de ce dossier sont tranchées : **pt 154, pt 594 et es 989 sont gardées — C-note définitives** (154 : conclure que la conclusion est *fausse* est la définition classique de l'*argument from fallacy*, le « rejetez » français est la version adoucie ; 594 : redondant, pas faux — un changement de style sur une langue imprimée ne vaut pas le coût ; 989 : la forme « présomption faute de preuve contraire » appartient à la même famille que le renversement de charge, l'espagnol est la traduction fidèle de l'anglais imprimé). **es 1357 (Invéntas) confirmée coquille et corrigée** par la PR d'arbitrage. Règle actée : une langue imprimée (pt) garde son texte sauf contresens ; les coquilles se corrigent toujours. Aucun compte ne change.

| Langue | A | M | C-note | ✓ |
|---|---:|---:|---:|---:|
| **pt** | **0** | **0** | **31** (5 catégories) | **144** |
| **es** | **0** | **0** | **24** (6 catégories) | **151** |

**Les 2×175 rangées ont été lues, aucun échantillonnage.**

⚠️ Faiblesses structurelles, dites avant le verdict : lecture **unique** par un **lecteur non natif** ; `desc_es` absent des archives (**0/175**) ; et pour le portugais, l'archive exploitable mécaniquement est **elle-même dégradée** (voir ci-dessous) — l'arbitrage pt↔pt n'a valeur d'étalon nulle part où l'archive est fautive.

## L'archive `desc_pt` est un artefact MT — la référence imprimée pt n'est pas un étalon

Mesuré à la lecture des 157 références rattachées : l'archive v3 porte des phrases **cassées en plein vol**, pas des variantes :

| PK (deck) | Ce que dit l'archive `desc_pt` |
|---|---|
| 175 Influence | « Você lida com seu público para **convencê-lo em vez de convencê-lo** » — convaincre au lieu de convaincre |
| 247 Poésie | « Você usa **flores e flores** para tornar seu argumento mais atraente » — des *fleurs* et des *fleurs* (l'anglais « flowers of speech » traduit littéralement) |
| 128 Raison du plus riche | « pois seu autor é **fornecido com bom** » — débris non grammatical |
| 185 Poncif anticritique | « Você usa **fotos cativantes** » — des *photos* (mistraduction) |

Conséquence : le `DIFFERE` **157/157** (aucune carte IDENTIQUE) ne se lit pas comme « le deck a dérivé de l'imprimé » mais comme « le deck a **remplacé** une colonne fautive ». La corrélation de longueur le confirme : deck pt ~ archive pt **0,320** — la colonne deck n'est **pas** dérivée de l'archive pt ; elle suit le français courant (**0,838**).

## T2 — d'où viennent les deux colonnes ? (mesuré)

| Comparaison | pt | es |
|---|---:|---:|
| vs **français courant** | **0,838** (n=175) | **0,854** (n=175) |
| vs **anglais imprimé** (157 rattachées) | 0,415 | 0,273 |
| vs **archive `desc_pt`** (157 non vides) | **0,320** | — (colonne inexistante) |

Deux traductions fraîches du français **courant**, plus fidèles à leur source que zh (0,667), ar (0,825) ou fa (0,715) — avec des **survivants d'époque** qui, cette fois, se vérifient **mécaniquement contre l'EN d'archive** (et non plus contre la seule longueur).

## Le grain livre le premier survivant d'un imprimé DÉFECTUEUX — es PK 989

Le dossier ㉑ a arbitré la cellule **zh PK 989** (inversion, corrigée par #1714) ; les grains ㉒ ㉓ ont vérifié la cellule **saine** en ar et fa. La cellule **es** est un troisième cas, mécaniquement vérifié :

- **Deck es** : *« Asumes que una proposición es verdadera hasta que se demuestre lo contrario **o viceversa** »* — « vous tenez une proposition pour vraie jusqu'à preuve du contraire ou inversement ».
- **Archive v3 EN** : *« Assuming a proposition to be true until proved false **or vice-versa** »* — **traduction quasi-verbatim**.
- **Archive v3 PT** (la même carte) : *« Você acredita que cabe à parte oposta demonstrar a deficiência de sua posição »* — **correcte** (renversement de charge).
- **Français courant** : « c'est à la partie adverse de démontrer que votre position est fausse, au lieu de la justifier vous-même » — correct.

La cellule es ne dérive donc pas du français courant **ni** ne s'invente : elle **conserve un imprimé EN qui définissait un autre sophisme** (la présomption / argument d'ignorance, pas le renversement de charge), pendant que l'archive PT de la même carte était juste. Le « ou viceversa » incohérent vient avec. ⛔ **Consignée avec frontière, arbitrage ai-01** (motif zh-989) : si un locuteur juge que définir la présomption au lieu du renversement rend la carte fausse → **A** ; si l'écart est tenu pour un héritage d'époque visible → C-note définitive. La cellule **pt 989 est saine** (« demonstrar a invalidez da sua posição », quatrième vérification directe : l'inversion n'existe qu'en chinois).

## C-notes pt — 31 rangées, 5 catégories, aucune écriture proposée

### 1. Sérieux, consignés AVEC frontière (2)

- **PK 594 « Erreur mathématique »** — `LONG(2,10)`, **deux phrases** : « O raciocínio inclui imprecisões no uso de informações numéricas. **Você emprega dados numéricos de forma incorreta ou imprecisa.** » La première phrase est la **traduction quasi-verbatim de l'EN d'archive** (« The reasoning includes inaccuracies in in the use of numerical information » — l'archive porte même un « in in » de débris MT) ; la seconde est **ajoutée par-dessus** et redit la première. Frontière M : si le doublon nuit à la carte → M (contenu ajouté) ; s'il se lit comme un développement → C-note.
- **PK 154 « Sophisme du sophisme »** — FR « Vous **rejetez** une conclusion uniquement parce que l'argument… est fallacieux » → PT « Argumentando que uma conclusão **é falsa** porque sua prova envolve uma falácia ». *Rejeter* ≠ *déclarer fausse* — c'est la nuance qui définit ce sophisme même. L'archive PT porte la même formulation (« Você diz que uma conclusão é falsa… ») : le décalage est **hérité**, pas inventé. Frontière : si un locuteur juge que « é falsa » détruit la mécanique → A. (L'es, lui, garde « Rechazar » — sain.)

### 2. Survivants d'époque, vérifiés MÉCANIQUEMENT contre l'EN d'archive (2)

| PK | Titre | Deck pt | EN d'archive (v3) |
|---|---|---|---|
| 343 | Argument du bâton | « Usar a ameaça para **forçar a concordância de alguém com uma proposição** » | « Using threat to **force someone's agreement with a proposition** » |
| 699 | Argument circulaire | « proposições que **só são verdadeiras se a conclusão for verdadeira** » | « propositions that are **only true if the conclusion is true** » |

Dans les deux cas le français courant dit autre chose (« remporter un débat au lieu de défendre par la raison » / « acceptation préalable de la conclusion ») — même famille que les survivants zh/ar/fa des grains précédents, mais ici **prouvée au mot près**, pas par longueur.

### 3. Jumeaux des dossiers précédents (11)

PK 3 (« apportent rien » → « sem peso argumentatif », gérondif — jumeau fa) · 43 (« un comportement » → « escolhas, métodos ou ações », jumeau fa) · 121 (« une partie de l'auditoire » → « ofensa **social ou institucional** », jumeau ar/fa) · 175 (ajoute « por meio de raciocínio lógico », jumeau zh/ar/fa) · 322 (discréditez → **critica**, jumeau ar/fa/es) · 729 (**« uma causa possível » + la glose « confundindo assim causa e condição necessária »** — jumeau EXACT zh/ar/fa/es, **cinquième langue**) · 833 (+ « e induz ao erro », jumeau fa/es) · 834 (« excessive ou inappropriée » → « não é aceitável », jumeau fa/es) · 1281 (« arguments rationnels » → « lógica », jumeau ar/fa/es) · 1352 (triplet « argumentação, debate ou oponente », jumeau fa) · 1362 (« pas toujours » → « falha em agir de forma consistente », jumeau fa/es).

### 4. Décalages propres au portugais, sens tenu (9)

| PK | Titre | Ce qui bouge |
|---|---|---|
| 98 | Appel à la majorité | « une position » → « a **validade de uma proposição** » ; « le plus grand nombre » → « muitas **ou** a maioria » |
| 108 | Appel à la nature | universalisé : « **tudo o que é** natural é bom » (FR : « une chose ») |
| 340 | Appel aux conséquences | « sa justesse » → « a própria proposta » (la chose, pas sa justesse) |
| 697 | Causalité douteuse | compression : « supposez… pas démontré ou incorrect » → « afirma relações questionáveis » |
| 802 | Indéfinissabilité | réification : « repose sur un concept » → « repousa sobre **o fato de que** » (jumeau es) |
| 855 | Équivoque | par le terme technique : « os diferentes sentidos d'un même mot » → « a **polissemia** » |
| 1011 | Exigence relâchée | « difficilement défendable » → « parece falhar em atendê-los » (plus doux que le jumeau ar/fa « indéfendable ») |
| 1373 | Reductio ad Hitlerum | rétréci : « dizendo que Hitler teria apoiado » — « ou au nazisme » et « injustement » perdus |
| 1388 | Attaque de la confiance en soi | « manque de confiance en soi » → « confiança **sobre seu argumento** » ; « ou evidência » ajouté |

### 5. Compressions et ajouts mineurs, sens tenu (7)

PK 826 (« contradictoires ou incohérentes » → « inconsistentes, logicamente falhas », gérondif) · 887 (`SHORT 0,57` — « règles **tacites** » perdu : « viola as normas ») · 888 (`SHORT 0,58` — « susceptible d'induire en erreur » → « enganosa », bénin) · 992 (intensificateur « nunca » ajouté, jumeau es) · 1024 (« reflètent » → « deixam que reflitam », trivial) · 1242 (« trop rigide » → « estrita e exclusiva » — le « trop » perdu, jumeau es) · 1330 (« sans rapport » → « principalmente irrelevantes », adouci — cousin ar).

### Observation de colonne (hors décompte) — le portugais mélange cinq registres de personne

Mesuré sur les 175 : **você** (singulier brésilien, majoritaire), **vocês** (pluriel), formes **vós** archaïques (PK 362 « Enquadrais », 511 « Procurais », 621 « Atribuís », 1282 « Afirmais »), **Os senhores** (formel européen : PK 358, 421, 633, 636, 1287, 1360, 1361), et ~26 cartes au **gérondif impersonnel** sans sujet (PK 3, 43, 98, 108, 121, 154, 176, 182, 313, 340, 361, 598, 690, 696, 826, 847, 848, 855, 900, 994, 1011, 1281, 1330, 1352, 1362, 1388). Ce n'est pas un défaut de fidélité au FR — c'est une **hétérogénéité de lots de traduction** (même famille que la signature « Scenarii 2 classes »), consignée pour arbitrage éditorial : un jeu publié ne mélange normalement pas vós, você et Os senhores.

## C-notes es — 24 rangées, 6 catégories, aucune écriture proposée

### 1. Sérieux, consigné AVEC frontière (1)

- **PK 989 « Renverser la charge de la preuve »** — voir la section dédiée ci-dessus : survivant vérifié d'un **EN d'époque défectueux** (définissait la présomption ; l'archive PT de la même carte était correcte). Frontière écrite, arbitrage ai-01.

### 2. Survivants d'époque, vérifiés MÉCANIQUEMENT contre l'EN d'archive (2)

| PK | Titre | Deck es | EN d'archive (v3) |
|---|---|---|---|
| 837 | Comparaison incohérente | « Comparar **parcialmente** varias cosas **para pretender** hacer una comparación general » | « **Partially comparing** several things **to pretend** to draw a general comparison » |
| 594 | Erreur mathématique | « Su razonamiento incluye inexactitudes en el uso de **información numérica** » | « The reasoning includes inaccuracies in in the use of **numerical information** » |

Le contraste 837 est instructif : **l'es est survivant là où le pt ne l'est pas** (pt 837 suit le français courant, « distorcendo a comparação geral »). Et le cas 594 explique rétroactivement une « addition » des grains précédents : le « informations **numériques** » qu'ar (notion shift ㉒), fa, pt et es portent tous là où le français courant dit juste « quantitatives » — c'est **l'imprimé EN d'époque qui le portait**, chaque langue l'a hérité en traduisant l'EN, le français seul l'a perdu en étant réécrit.

### 3. Jumeaux des dossiers précédents (12)

PK 300 (« infléchir son jugement » → « ganar su apoyo », jumeau ar/fa) · 658 (« la notion d'infini » → « nociones infinitas », jumeau ar/fa/pt) · 729 (« una causa posible » + **la même glose** « confundiendo así causa y condición necesaria » — jumeau EXACT zh/ar/fa/pt, **cinquième langue**) · 735 (« fausse l'argument » → « impacta el argumento », jumeau pt) · 740 (« implications » → « conclusiones », jumeau pt) · 802 (« el hecho de que », jumeau pt) · 809 (« termes distincts » → « términos **del debate** », jumeau fa) · 833 (+ « e induce a error », jumeau fa/pt) · 834 (« excessive ou inappropriée » → « incorrecta », `SHORT 0,57`, jumeau fa/pt) · 1011 (« difficilement défendable » → « parece no cumplirlas », jumeau pt) · 1281 (« arguments rationnels » → « la lógica », jumeau ar/fa/pt) · 1362 (« pas toujours » → « no actúa de acuerdo », jumeau fa/pt).

### 4. Décalages propres à l'espagnol, sens tenu (3)

| PK | Titre | Ce qui bouge |
|---|---|---|
| 638 | Tireur d'élite texan | restructuré : « **Asignar una causa común a un patrón.** Esto resulta en forzar el resultado…, moldeando los hechos » — « repérez une tendance **après coup** » perdu, « moldeando los hechos » ajouté |
| 673 | Juste milieu | « la **solution** réside dans le compromis » → « la **verdad** se encuentra en un compromiso » — la vérité, pas la solution |
| 1024 | Biais naturels | « limites et réflexes liés à la perspective humaine » → « perdiendo así **la objetividad científica** » — éditorial ajouté |

### 5. Compressions et ajouts mineurs, sens tenu (5)

PK 165 (« l'explication la plus simple » → « la simplicidad ») · 697 (« pas démontré » → « no es evidente ») · 888 (`SHORT 0,53` — « engañosa », bénin, jumeau pt) · 992 (intensificateur « nunca », jumeau pt) · 1242 (« trop rigide » → « estricta », jumeau pt).

### 6. Typo probable — lisible, sens intact (1)

- **PK 1357 « Arguments factices »** : « **Invéntas** » — accent fautif (forme standard : « inventas »). Même statut que les typos fa du ㉓ (PK 323, 362) : confirmation par un locuteur, aucune écriture proposée.

### Observation de colonne (hors décompte) — l'espagnol mélange quatre registres de personne

**usted** (PK 108 « Supone », 653 « Cree », 727 « Utiliza », 735, 740, 796, 808 « Usa », 942), **ustedes** (PK 70, 176, 184, 299, 356, 358, 420, 511, 594, 633, 636, 889, 908, 1023, 1282, 1287, 1312, 1360, 1361), **tú** (PK 71 « Crees », 79, 105, 112, 165, 177, 247, 304, 337, 340, 376, 596, 614…), **vosotros** (PK 51 « Confundís », 55, 121, 128, 133, 134, 219, 421, 432, 603, 621, 622, 625, 707, 708, 713, 729, 733, 759, 784, 837, 845, 887, 888, 900, 953, 956, 973, 974, 977, 989, 992, 1023, 1024, 1092, 1120, 1174, 1242, 1281, 1291, 1301, 1313, 1330, 1352, 1362, 1388). Même conclusion que pour le pt : hétérogénéité de lots, arbitrage éditorial, pas un défaut de fidélité.

## Contrôle inverse

Copie du corpus en scratchpad, **jamais le dépôt** — vérifié après la passe : `git status --porcelain Cards/` **vide**.

1. **Cellule inversée plantée** (PK 814, le faux dilemme dit à l'envers, sans mot de négation) : **pt `FLAGS: -`** ; **es `SHORT(0,57)`** — mais ce drapeau est un **artefact de longueur** (la phrase plantée est plus courte que le FR), pas une détection de sens : `POLARITY` n'a pas levé. Dans les deux langues, **aucun écran ne voit le contresens ; la lecture le voit immédiatement**. L'incident es est consigné pour ce qu'il est : la phrase témoin doit être calibrée en longueur pour que « aucun drapeau » veuille dire « aveugle », leçon du ⑳ réappliquée.
2. **Cellules tronquées à 50 %** : pt PK 43 → `SHORT(0,49)` ✓ ; es PK 51 → `SHORT(0,44)` ✓ — l'écran de longueur latin attrape la troncature dans les deux langues.

## N'établit pas

- **Que les définitions pt/es soient sans défaut.** Lecture **unique**, par un **non-locuteur natif** — l'établi est plus étroit : *aucun écart n'a été trouvé en lisant les 2×175 rangées* hors les 55 consignées.
- **Que l'arbitrage pt↔pt éclaire quoi que ce soit là où l'archive est fautive** — elle l'est visiblement sur plusieurs cartes (section dédiée) ; la référence « exploitable » 168/175 est exploitable **mécaniquement**, pas qualitativement.
- **Que les trois cellules à frontière (pt 594, pt 154, es 989) soient des A ou des M** — la frontière est écrite, l'arbitrage appartient à ai-01 (motif zh-989).
- **Que la typo probable (es 1357) en soit une** — un locuteur natif confirme ou infirme.
- **Que les écrans corroborent quoi que ce soit** : `POLARITY` pt 10 / es 8 drapeaux, tous faux positifs lus (parité de négation respectée dans chaque cas) ; le contresens planté sort sans `POLARITY` dans les deux langues.
- **Rien sur** les 7 cartes sans archive (PK 105, 362, 492, 1020, 1092, 1120, 1357), la langue ㉕ (`en`), les champs `example_<lang>`/`link_<lang>`, ni les **1233 rangées hors deck**.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.** Les 55 observations attendent l'arbitrage d'ai-01.

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --lang pt --out <sortie>
python docs/corpus/definitions-fidelity-instrument.py --lang es --out <sortie>
```

- Dénominateur **auto-vérifié** (arrêt bruyant si ≠ 175, par langue) ; corpus du dépôt **jamais écrit** (contrôle inverse sur copie).
- **Aucune réparation d'instrument ce grain** — les jeux pt/es portaient déjà des frontières de mot (écriture latine) et l'écran de longueur brut leur est applicable : rien à étalonner, rien à re-mesurer. Les comptes des langues précédentes (ru 26 / zh 48 / ar 47 / fa) sont inchangés **par construction** (aucun diff sur l'instrument).
- Comptes de jointure inchangés (157 nom / 11 position seule / 7 aucune — mêmes que ⑳-㉓, mesurés au grain 3).
- ⚠️ Piège d'instrument rencontré et éliminé pendant le grain : **rapprocher les PK du deck des PK d'archive est une jointure morte** (l'archive « PK 343 » est la carte PNL, PK 376 du deck — deux numérotations). Les vérifications « verbatim » de ce dossier passent toutes par la jointure **par nom** de l'instrument (pont/étage-1), jamais par PK.
