# Fidélité Scénarios — **en, l'axe imprimé** (complément au dossier `en`)

**Mandat** : pool #458 (c.5980960236), **grain 1** — « 1. en — l'axe imprimé (complément du
dossier en, une PR). Pour chacun des 39 défauts, une classe : **imprimé et inchangé** → à
**soumettre** ; **dérivé depuis l'impression** → **correction** ; **jamais imprimé en EN**
(9 cartes de l'édition n'ont aucun texte EN). Méthode : jointure par titre FR normalisé, puis par
contexte pour les 21 titres renommés. Chaque appariement est confirmé à la lecture, ⛔ jamais par
`path`. »
**Objet** : confronter les **39 défauts EN** du dossier `458-fidelite-scenarii-en-2026-10-04.md`
(§3) à l'**édition imprimée de février 2022**
(`Cards/Scenarii/Archive/2022/Argumentum Scenarii - Cards fevrier 2022.csv`, 77 cartes).
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée. Ce document est un
**complément** au dossier `en` : il ne le remplace pas et ne le re-mesure pas.

---

## 1. Méthode — et pourquoi le `path` était interdit

La jointure s'est faite en trois passes, dans cet ordre :

| Passe | Règle | Paires |
|---|---|---|
| 1 | **titre FR normalisé** (NFKD, sans casse, sans ponctuation) — appariement 1-1 exigé | **56** |
| 2 | **contexte** (ratio ≥ 0,6 sur l'agrégat `titre+contexte+enjeu+suggestion+baratineur+piocheur`) | **9** |
| 3 | **titre imprimé** pour les 12 restants, chacun **confirmé à la lecture** | **12** |
| | | **77** |

La passe 1 seule laisse **21 cartes non appariées** — exactement le chiffre que le pool annonçait
(« les 21 titres renommés »).

⚠️ **Le `path` n'est pas une clé : il a bougé sur 26 des 77 paires (34 %).** Mesure :

| | Paires |
|---|---|
| `path` identique entre les deux éditions | **51 / 77** |
| `path` **différent** | **26 / 77** |

Et il porte une **collision** : l'édition imprimée numérote **deux** cartes `3.2.1`
(`coordonnées` `3,21`) — « emménager ensemble » / `move in together` **et** « les bibelots » /
`Trinkets`. Le CSV courant les a séparées en `3.2.1` (coord. `3,0201`) et `3.2.2` (coord.
`3,0202`) : `path` et `coordonnées` sont **uniques** côté courant, **pas** côté imprimé.

⭐ Une jointure par `path` aurait donc **mal apparié un tiers du deck** et **fusionné deux cartes en
une** — en silence, les deux erreurs produisant un résultat plausible. Le titre FR, lui, départage
les deux `3.2.1` sans ambiguïté.

**Contrôle du drapeau.** Le CSV courant porte une colonne `édition février 2022` = `1` sur **77**
rangées (les 90 autres sont vides). Le drapeau n'a pas servi à joindre : il a été **testé**.
Balayage des 90 cartes hors-drapeau contre les 77 rangées imprimées, par le même agrégat de
contexte, **meilleur appariement par carte** : **une seule** carte franchit le seuil 0,5 — `3.3.7`
(« La photo de trop »), dont les deux meilleures paires sont `3.4.2` à **0,556** et `3.2.5` à
**0,506**, toutes deux des **faux positifs** (mots génériques communs : « compromettante »,
« surprendre »). Les **89** autres cartes plafonnent en dessous. ⇒ Le drapeau et la jointure par
contenu **concordent**.

---

## 2. Le corpus imprimé, en chiffres

| Mesure | Valeur |
|---|---|
| Rangées imprimées | **77** (+ en-tête), 47 583 octets, **sans BOM** |
| `path` distincts | **76** (collision `3.2.1` ×2) |
| Colonnes | 20 — FR (`titre`…`suggestion`) **et** EN (`title`…`suggestion_en`), **aucune** autre langue |
| Cartes imprimées **sans aucun texte EN** | **9** — `2.1.8` `4.1.5` `5.1.3` `5.3.1` `5.3.3` `6.1.4` `6.2.2` `6.3.2` `6.4.3` |
| Cellules EN non vides (imprimé) | **195** |
| Cellules EN non vides (courant) | **501** |

**L'EN imprimé s'adressait au joueur.** Sur les 195 cellules EN imprimées, **125 (64 %)** contiennent
`you`/`your` et **0** contiennent `The smooth talker` ; le courant est à **86** `you`/`your` contre
**173** `The smooth talker` (restreint aux 77 cartes de l'édition : **35** contre **93**).
⇒ Une **passe de standardisation 2ᵉ → 3ᵉ personne** a eu lieu entre les deux états. Elle explique
à elle seule toute la classe « dérivé » (§5).

---

## 3. Les classes, telles que mesurées

Le pool demandait trois classes. La mesure en établit **quatre**, parce que « imprimé et inchangé »
recouvre deux situations que le geste de correction ne traite pas pareil : quand la cellule EN est
identique **et** que le FR a été **réécrit** depuis, l'EN n'est pas le fautif — il est **fidèle à un
FR qui n'existe plus**.

| Classe | Cellules (§3) | Ce qu'elle demande |
|---|---:|---|
| **A. `FR-INTACT`** — imprimé et inchangé, FR inchangé | **5** | **soumettre** : défaut EN établi |
| **B. `FR-COSMETIQUE`** — imprimé et inchangé, FR retouché (casse, espace, article, accent) | **10** | **soumettre** : le FR dit la même chose |
| **C. `EN-FIDELE-A-UN-FR-PERIME`** — imprimé et inchangé, **FR réécrit** | **4** | **ré-attribuer** : ce n'est pas un défaut EN |
| **D. `FR-REPARE-SEUL`** — le FR s'est ponctué après impression, l'EN n'a pas suivi | **1** | **correction EN** (le FR a montré la cible) |
| **E. `DERIVE`** — la cellule EN a changé depuis l'impression | **5** | **correction**, motivée (§5) |
| **F. `JAMAIS-IMPRIME-EN`** — carte imprimée, cellule EN **vide** à l'impression | **5** | libre : rien n'a été imprimé |
| **G. `CARTE-HORS-EDITION`** — la carte n'est pas dans l'édition | **10** | libre : rien n'a été imprimé |
| | **40** | *(39 entrées ; `5.1.2` en groupe C porte 2 cellules)* |

⭐ **Un fait que le pool n'avait pas** : **19 des 40 cellules (47,5 %) ne sont pas imputables à
l'EN** — **4** sont la traduction fidèle d'un FR périmé, **5** n'ont jamais été imprimées en EN,
**10** portent sur des cartes jamais imprimées. Et **5 seulement** sont un défaut EN établi contre
un FR intact (classe A).

---

## 4. Table des 39 entrées (40 cellules)

`imprimé` = `path` de la rangée imprimée appariée (`—` = carte absente de l'édition). L'entrée
C-`5.1.2` porte deux champs (`bara` **et** `sugg`) : elle produit deux lignes.

| Gr. | Carte | Champ | Imprimé | Verdict |
|---|---|---|---|---|
| A | 3.1.2 | titre | 3.1.2 | **C** · EN fidèle à un FR périmé |
| A | 3.2.15 | titre | — | **G** · carte hors édition |
| A | 4.1.1 | titre | 4.1.1 | **B** · FR cosmétique |
| A | 5.3.1 | titre | 5.3.1 | **F** · jamais imprimé en EN |
| A | 5.3.4 | titre | 5.3.3 | **F** · jamais imprimé en EN |
| A | 7.1.5 | titre | 7.1.5 | **B** · FR cosmétique |
| A | 5.1.2 | titre | 5.1.2 | **B** · FR cosmétique |
| B | 3.1.1 | sugg | 3.1.1 | **C** · EN fidèle à un FR périmé |
| B | 6.1.1 | sugg | — | **G** · carte hors édition |
| B | 5.2.5 | enjeu | — | **G** · carte hors édition |
| B | 1.3.2 | bara | 1.4.2 | **A** · FR intact |
| B | 2.2.5 | bara | 2.2.5 | **B** · FR cosmétique |
| C | 4.2.1 | sugg | 4.2.1 | **C** · EN fidèle à un FR périmé |
| C | 5.1.2 | bara | 5.1.2 | **A** · FR intact |
| C | 5.1.2 | sugg | 5.1.2 | **B** · FR cosmétique |
| D | 4.3.1 | sugg | 4.4.1 | **B** · FR cosmétique |
| D | 7.2.8 | sugg | — | **G** · carte hors édition |
| D | 2.2.1 | sugg | 2.2.1 | **D** · FR réparé seul |
| E | 5.3.2 | enjeu | — | **G** · carte hors édition |
| E | 3.2.2 | sugg | 3.2.1 | **C** · EN fidèle à un FR périmé |
| E | 5.3.1 | enjeu | 5.3.1 | **F** · jamais imprimé en EN |
| F | 3.2.8 | ctxt | — | **G** · carte hors édition |
| F | 4.1.1 | ctxt | 4.1.1 | **E** · dérivé (2ᵉ→3ᵉ personne) |
| F | 7.1.2 | ctxt | 7.1.2 | **E** · dérivé (2ᵉ→3ᵉ personne) |
| F | 6.3.1 | ctxt | 6.3.1 | **E** · dérivé (2ᵉ→3ᵉ personne) |
| F | 3.1.3 | ctxt | 3.1.3 | **E** · dérivé (FR a gagné « sans consentement ») |
| G | 2.3.5 | pioch | 2.1.2 | **A** · FR intact |
| G | 7.3.2 | pioch | 7.3.2 | **A** · FR intact |
| G | 7.1.5 | titre | 7.1.5 | **B** · FR cosmétique |
| H | 4.2.2 | ctxt | 4.2.2 | **E** · dérivé (2ᵉ→3ᵉ personne) |
| H | 7.2.8 | sugg | — | **G** · carte hors édition |
| H | 6.1.4 | sugg | 6.1.4 | **F** · jamais imprimé en EN |
| H | 4.3.3 | ctxt | — | **G** · carte hors édition |
| H | 7.1.6 | titre | 7.2.2 | **B** · FR cosmétique |
| H | 7.3.4 | titre | — | **G** · carte hors édition |
| I | 7.1.2 | sugg | 7.1.2 | **B** · FR cosmétique |
| I | 6.2.2 | ctxt | 6.2.2 | **F** · jamais imprimé en EN |
| I | 3.1.6 | pioch | 3.1.6 | **A** · FR intact |
| I | 7.2.5 | enjeu | — | **G** · carte hors édition |
| I | 5.2.1 | sugg | 5.2.1 | **B** · FR cosmétique |

⚠️ Les **39 entrées** couvrent **38 cellules distinctes** (33 cartes) : `7.2.8 sugg` est citée par D **et** par H
(contresens **et** agrammaticalité), `7.1.5 titre` par A **et** par G (titre substitué **et**
incohérence avec le corps de sa propre carte). Le décompte de 39 du §9 du dossier `en` compte des
**entrées**, pas des cellules — les deux chiffres sont vrais.

### Classe A — les 5 défauts EN établis (FR intact)

| Carte | Champ | FR (identique aux deux états) | EN (identique aux deux états) |
|---|---|---|---|
| 1.3.2 | bara | « Le président des États-Unis » | `Truman` |
| 2.3.5 | pioch | « Le spectre » | `the statue of the governor` |
| 3.1.6 | pioch | « Une personne rencontrée en ligne » | `A person I met online` |
| 5.1.2 | bara | « Le shtroumphissime » | `the smurf` |
| 7.3.2 | pioch | « Un parent » | `A relative` |

Ce sont les seules cellules où **rien n'a bougé de part et d'autre** : le FR imprimé est le FR
courant, l'EN imprimé est l'EN courant, et l'EN ne dit pas le FR. **À soumettre**, en sachant que
corriger change un texte **imprimé**.

---

## 5. Classe E — les 5 « dérivés » : une seule cause

Les cinq cellules dérivées portent **toutes** sur `ctxt`, et **quatre** sur cinq sont le même geste :

| Carte | EN imprimé | EN courant |
|---|---|---|
| 4.1.1 | `You're a car dealer. A customer comes into your shop with a modest budget but great aspirations.` | `The smooth talker is a car dealer. A customer comes into his shop with a modest budget but great aspirations.` |
| 7.1.2 | `You're a kid at school. You pushed your classmate down the stairs when he fell and complained to the teacher.` | `The smooth talker is a kid at school. They pushed their classmate down the stairs. He fell and complained to the teacher.` |
| 6.3.1 | `You're a candidate in a major national election. You paid your spouse in fictitious employment and it's come out.` | `The smooth talker is a candidate in a major national election. He paid his spouse in fictitious employment and it's come out.` |
| 4.2.2 | `It's five o'clock in the morning, although already drunk, you decide to have a nightcap while you're working in two hours.` | `It's five o'clock in the morning, although already drunk, the smooth talker decides to have a nightcap while he is working in two hours.` |

⭐ **Conséquence directe sur le dossier `en`.** Ces quatre cellules sont classées au §3 F (« ajouts »)
comme « + une phrase entière absente du FR ». Mesure : la phrase **n'est pas ajoutée** — elle est
**imprimée**, à la 2ᵉ personne, et la passe de standardisation l'a convertie. Pour 4.2.2 (§3 H,
« proposition détachée sans sujet, calque de la construction absolue FR ») : le calque est **aussi
imprimé** (`although already drunk, you decide`) — l'EN ne l'a pas fabriqué, il l'a **conservé**.
⚠️ Le FR, lui, a **avancé** sur 4.2.2 : imprimé « Il est cinq heures du matin, **bien que** déjà
éméché, le baratineur décide… » → courant « Il est cinq heures du matin. **Déjà éméché**, le
baratineur décide… ». Le FR s'est partiellement dégagé du calque ; l'EN est resté.

La cinquième est d'une autre nature :

| Carte | EN imprimé | EN courant | FR imprimé → courant |
|---|---|---|---|
| 3.1.3 | `You removed your condom during sex. Your partner noticed.` | `The Smooth Talker removed his condom during sex **without his partner's consent**. His partner realizes what happened.` | « …pendant l'acte. » → « …pendant l'acte **sans consentement**. » |

Ici l'EN **suit un FR qui a lui-même gagné la précision** : ce n'est pas une explicitation inventée
par l'EN (§3 F), c'est une **mise à jour fidèle**. ⇒ **conserver**.

---

## 6. Classe C — quatre entrées ré-attribuées : l'EN traduit un FR qui n'existe plus

**Le résultat le plus lourd de ce complément.** Quatre cellules que le dossier `en` impute à l'EN
sont, à la mesure, la **traduction exacte du FR imprimé** — un FR que l'édition courante a
**réécrit** sans propager la réécriture à l'EN.

| Carte · champ | FR imprimé | FR courant | EN (identique aux deux états) | §3 du dossier `en` |
|---|---|---|---|---|
| **3.1.1** sugg | « On peut essayer tous les deux, on verra qui sera choisi. » | « Laissez-moi tenter ma chance ce soir ; si j'échoue, je vous laisse le champ libre. » | `We can both try, we'll see who gets picked.` | B « contenu substitué : la supplique devient une proposition de compétition » |
| **3.1.2** titre | « tu t'es vu quand t'as bu » | « Lendemain difficile » | `Did you see yourself when you drank` | A « titre remplacé par une phrase » |
| **3.2.2** sugg | « Bon, où est-ce que je vais mettre mes **habits**? » | « …mes **affaires** ? » | `Well, where am I going to put my clothes?` | E « dilution : le générique devient spécifique » |
| **4.2.1** sugg | « …ce poulet **à la broche** dans votre assiette? » *(la phrase s'arrête là)* | « …ce poulet **rôti** dans votre assiette ? **Je croyais qu'on était en rationnement.** » | `…what's this **spit-roasted** chicken on your plate?` | C « la dernière phrase est coupée ; rôti devient spit-roasted, sur-spécifié » |

⭐ **Ce que la mesure renverse, cas par cas :**

1. **3.1.1** — l'EN n'a rien substitué : il traduit le FR imprimé **mot pour mot**. C'est le FR qui
   a changé de nature (supplique au lieu de compétition). Or c'est **cette cellule EN qui est
   `SOURCE`** de la famille suivie par zh · ar · fa · es (§4 et §5 du dossier `en`) : les quatre
   langues qui « suivent l'EN » suivent en réalité **l'ancien FR**, par l'EN. ⚠️ Cela **ne disculpe
   pas** les quatre langues — elles devaient lire le FR courant — mais cela **déplace la cause**.
2. **3.1.2** — `Did you see yourself when you drank` n'est pas « une phrase mise à la place d'un
   titre » : c'est la **traduction du titre FR imprimé**. L'EN est resté sur l'ancien titre.
3. **3.2.2** — « affaires » est le mot **courant** ; c'est « habits » qui était imprimé. L'EN
   `clothes` traduit `habits`. La « dilution » est dans le **FR**, pas dans l'EN.
4. **4.2.1** — la phrase du rationnement **n'était pas dans le FR imprimé** : elle a été **ajoutée**
   au FR après l'impression. L'EN n'a **rien coupé**. Et `spit-roasted` n'est pas une
   sur-spécification : c'est la traduction de « à la broche », le mot **imprimé**.

⚠️ **Une cinquième, d'une autre espèce (`FR-REPARE-SEUL`)** — **2.2.1 `sugg`** : le FR imprimé
disait `Vade retro Satanas` (sans virgule ni point) ; le FR courant dit « Vade retro**,** Satanas**.** ».
L'EN n'a jamais bougé. ⇒ Le défaut EN du §3 D est **exact** (« la virgule et le point tombent »),
mais sa cause est nommable : **le FR s'est ponctué seul** et l'EN ne l'a pas suivi. La cible de
correction est donc **donnée par le FR courant** — c'est le cas le plus facile des 39.

**Les deux cartes du §5 absentes du §3 appartiennent à la même classe.** Le §5 du dossier `en`
liste dix « familles propres à l'EN », dont **1.1.1 bara** (nom propre perdu) et **2.1.3 sugg**
(question → affirmation) — qui **ne figurent pas** dans les 39 du §3. Mesure : **toutes deux**
relèvent de la classe C.

| Carte · champ | FR imprimé | FR courant | EN |
|---|---|---|---|
| **1.1.1** bara | « la mère de César » | « **Aurelia Cotta**, mère de César » | `Caesar's mother` |
| **2.1.3** sugg | « Alors toi aussi tu me trahis, car te me refuses tes récits. A mort! » | « Alors, vous aussi, vous voulez me trahir en me refusant vos histoires ? À mort ! » | `Then you betray me too, because you refuse me your stories. Death!` |

Le §6 du dossier `en` écrit de 1.1.1 : « le bara EN perd le nom propre que porte le FR » — mesuré :
le FR a **gagné** « Aurelia Cotta » **après** l'impression, l'EN `Caesar's mother` traduit exactement
« la mère de César ». Même bascule pour 2.1.3.

---

## 7. Classe F — les 9 cartes imprimées sans EN

Les neuf cartes que l'édition a imprimées **sans aucun texte EN** portent aujourd'hui **6 champs EN
remplis sur 6** — soit **54 cellules EN écrites après l'impression** :

| Imprimé | Courant | Titre courant | EN remplis |
|---|---|---|---|
| 2.1.8 | 2.1.8 | Le loup et l'agneau | 6/6 |
| 4.1.5 | 4.1.4 | Le professeur | 6/6 |
| 5.1.3 | 5.1.3 | La potion de trop | 6/6 |
| 5.3.1 | 5.3.1 | Débat avec un terraplaniste | 6/6 |
| 5.3.3 | 5.3.4 | La conspiration de la 5G | 6/6 |
| 6.1.4 | 6.1.4 | Bas les masques | 6/6 |
| 6.2.2 | 6.2.2 | Juste un doigt | 6/6 |
| 6.3.2 | 6.3.2 | Ami de vingt ans | 6/6 |
| 6.4.3 | 6.2.3 | Alliance douteuse | 6/6 |

⚠️ **Deux de ces cartes portent un `path` différent** aujourd'hui (`4.1.5→4.1.4`,
`6.4.3→6.2.3`) — un fait de plus contre la jointure par `path`.

⇒ Sur ces cartes, **aucun texte EN n'a jamais été imprimé** : toute correction EN est **libre**, et
les 5 cellules du §3 qui les touchent (`5.3.1` titre + enjeu, `5.3.4` titre, `6.1.4` sugg,
`6.2.2` ctxt) n'ont **rien à préserver**.

---

## 8. Classe G — les 10 cellules hors édition

Neuf cartes du §3 (`3.2.15`, `3.2.8`, `4.3.3`, `5.2.5`, `5.3.2`, `6.1.1`, `7.2.5`, `7.2.8`,
`7.3.4`) **ne sont pas dans l'édition** — 10 cellules. Vérifié deux fois : par le drapeau
`édition février 2022` (vide) **et** par recherche de contenu dans toute l'édition.

⭐ **Le cas le plus important est un défaut-signal.** `3.2.15` (« Le t-shirt taché » /
`The Spaghetti T-Shirt`) est **absent de l'édition** : aucune occurrence de *spaghetti*, *t-shirt*,
*tach*, *shirt* dans les 77 rangées imprimées. Or le §5 du dossier `en` en fait la carte dont
**cinq langues** (ar · fa · es · ru · pt) suivent le titre EN. ⇒ La famille `The Spaghetti T-Shirt`
est **entièrement postérieure à l'impression** : elle n'a **rien à voir** avec l'édition papier, et
corriger ce titre ne touche aucun produit imprimé.

---

## 9. Ce que ce complément change dans le dossier `en`

⚠️ **Aucune ligne du dossier `en` n'est fausse** : ses 39 défauts sont bien **dans le CSV courant**,
et ses citations sont exactes. Ce qui change est leur **attribution** — à qui revient l'écart.

| §3 du dossier `en` | Entrée | Lecture du dossier | Lecture de l'axe imprimé |
|---|---|---|---|
| B | 3.1.1 sugg | « contenu substitué » par l'EN | **EN = traduction du FR imprimé** ; FR réécrit depuis |
| A | 3.1.2 titre | « titre remplacé par une phrase » | **EN = traduction du titre FR imprimé** |
| E | 3.2.2 sugg | « dilution » de l'EN | FR imprimé « habits » ; **EN fidèle** ; FR passé à « affaires » |
| C | 4.2.1 sugg | « la dernière phrase est coupée » | **la phrase n'était pas dans le FR imprimé** ; FR l'a ajoutée |
| F | 4.1.1 ctxt · 7.1.2 ctxt · 6.3.1 ctxt | « ajouts » de l'EN | phrase **imprimée** ; convertie 2ᵉ→3ᵉ personne |
| F | 3.1.3 ctxt | « explicitation » de l'EN | **FR a gagné « sans consentement »** ; EN à jour |
| H | 4.2.2 ctxt | « calque » de l'EN | calque **imprimé**, conservé par l'EN ; FR a avancé |
| D | 2.2.1 sugg | l'EN perd virgule et point | exact — **le FR s'est ponctué seul** après impression |
| §6 | 1.1.1 bara | « l'EN perd le nom propre » | **le FR a gagné « Aurelia Cotta »** après impression |

⛔ **Aucun commentaire d'erratum n'est posté sur la PR #1754.** Le dossier `en` reste publié tel
quel ; c'est le présent complément qui porte la lecture corrigée — même régime que le §10 du
dossier `en` lui-même, qui ne s'est pas erraté sur les six dossiers antérieurs.

---

## 10. Ce que ce complément n'établit pas

- **Les classes A/B/C ne disent rien de la justesse.** Qu'une cellule soit « imprimée et inchangée »
  n'établit pas qu'elle soit **fautive** : c'est le §3 du dossier `en` qui l'établit, par lecture, et
  ce complément **ne le re-juge pas**. Il dit seulement **qui a bougé**.
- **La direction de la réécriture FR.** Que le FR courant diffère de l'imprimé est mesuré ; **qui**
  l'a réécrit, **quand** et **pourquoi** ne l'est pas. « FR réécrit après impression » est une
  inférence d'ordre (le CSV courant est postérieur au fichier d'archive), pas une datation.
- **La date des passes EN.** Rien ici ne date la standardisation 2ᵉ→3ᵉ personne, ni ne dit combien
  de passes successives ont produit l'EN courant.
- **Le jugement natif** sur les 5 défauts de la classe A et sur les 10 `FR-COSMETIQUE` : dire qu'un
  défaut « tient » malgré une retouche FR cosmétique est un jugement de lecteur, pas une mesure.
- **Ce que l'imprimé vaut comme cible.** Que le texte imprimé soit *meilleur* n'est établi nulle
  part. Pour 4.2.1, par exemple, c'est le **FR courant** qui porte l'information manquante
  (« rationnement ») et l'imprimé qui ne l'a pas.
- **L'exhaustivité de la jointure.** 77 paires sur 77 rangées, chaque appariement confirmé — mais
  trois paires de la passe 3 reposent sur **ma lecture**, pas sur une clé. Un lecteur qui conteste
  `1.2.5 ↔ 1.3.3`, `3.3.10 ↔ 3.2.3` ou `5.3.4 ↔ 5.3.3` doit rouvrir ces trois-là en priorité.
- **Les 90 cartes hors édition** ne sont couvertes que par le test du drapeau (§1) : elles n'ont pas
  été lues une par une contre l'édition.

---

## 11. Verdict

- **Bijection 77 ↔ 77 établie** par titre FR normalisé (56), contexte (9) et lecture (12) — le
  `path` **écarté** : 26 paires sur 77 l'ont changé, et l'édition porte une **collision** `3.2.1`.
- **40 cellules classées** : **5** défauts EN établis contre un FR intact · **10** contre un FR
  retouché cosmétiquement · **4** EN fidèles à un **FR périmé** · **1** FR réparé seul · **5**
  dérivés · **5** jamais imprimés en EN · **10** sur des cartes hors édition.
- **19 des 40 cellules (47,5 %) ne sont pas imputables à l'EN** : 4 relèvent d'un FR réécrit sous
  elles, 15 d'un texte jamais imprimé (5 cellules sur des cartes imprimées sans EN, 10 sur des
  cartes hors édition).
- **Une cause unique** pour 4 des 5 « dérivés » : une passe **2ᵉ → 3ᵉ personne** (imprimé : 125/195
  cellules en `you`/`your`, 0 `The smooth talker` ; courant : 86 contre 173).
- **Les 5 défauts EN à soumettre** (classe A) portent sur un texte **imprimé** : les corriger change
  un produit papier — c'est une décision éditoriale, pas une correction de fidélité.
- **0 écriture** : aucune cellule CSV n'a été modifiée par ce grain.

*po-2024*
