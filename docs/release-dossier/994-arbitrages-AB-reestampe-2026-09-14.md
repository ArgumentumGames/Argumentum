# #994 — Arbitrages A / B re-estampés sur master courant (`21a72385`)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : master `21a72385`
**Statut** : **RE-MESURE + FEUILLE DE DÉCISION** — lecture seule. ⛔ `0` écriture CSV, ⛔ aucune décision éditoriale prise ici.
**Dispatch** : ai-01, deep-queue #458 c.5656689863, grain **②** (`[secondaire] #994 dossiers A/B`).
**Sources amont** : dossier **A** = commentaire #994 du 2026-09-06 (master `2a2e7b32`) · dossier **B** = commentaire #994 du 2026-09-06 (master `1ab6d861`).
**Instrument** : `Cards/Scenarii/Argumentum Scenarii - Cards.csv`, 167 rangées, 70 colonnes ; FR **et EN nus** (`contexte`, `context`), autres langues suffixées.

> La re-mesure n'est pas une formalité : elle **écarte deux résultats du dossier A** (§A.2) et **confirme intégralement le dossier B** (§B.1). Un arbitrage présenté sur un chiffre faux se tranche mal.

---

## §A — Dossier A : la voix de l'anglais dans Scenarii

### A.1 Ce qui tient, au chiffre près

| dimension | dossier (06/09, `2a2e7b32`) | re-mesure (14/09, `21a72385`) | verdict |
|---|---:|---:|---|
| `contexte` 2ᵉ pers. — **fr** | 1 | **1** (`3,0106`) | ✅ |
| `context` 2ᵉ pers. — **en** | 25 | **25** | ✅ |
| — ru · pt · es · ar · zh | 0 | **0** | ✅ |
| — **fa** | **3** | **0** | ❌ **faux positif d'instrument** (§A.2) |
| `smoothTalker` EN 2ᵉ pers. | 12 / 167 | **12** | ✅ |
| `drawer` EN 2ᵉ pers. | 11 / 167 | **10** | ↘ **delta −1, pinné** (§A.3) |
| rangées incohérentes (la carte change de personne) | 15 | **16** | ❌ **`4,0101` manquait** |
| rangées « hors cause » | 8 | **non reproduite** | ❌ **liste non reproductible** (§A.2) |

⇒ **`25 + 12 + 10 = 47`** : la volumétrie de la branche (A) annoncée par ai-01 est **exacte**.

### A.2 Deux faux positifs d'instrument, nommés

**FP #1 — les deux listes du dossier ne sont pas disjointes.** `3,0306`, `3,0311` et `4,0302`
figurent **à la fois** dans les « 15 rangées incohérentes » **et** dans les « 8 rangées hors cause ».
Une rangée ne peut pas être les deux : le dossier se contredit sur 3 rangées, et son total
`15 + 8 = 23` ne décrit que **20** rangées distinctes.

**FP #2 — la liste « hors cause » ne décrit pas les rangées qu'elle cite.** Lecture des 8 rangées
citées, `context` EN au HEAD :

| rangée | `context` EN réel | ce que le dossier en dit |
|---|---|---|
| `3,0102` | `The smooth talker wakes up beside a conquest…` | « You / Your conquest » — **3ᵉ personne, pas 2ᵉ** |
| `3,0103` | `The Smooth Talker removed his condom…` | idem — **3ᵉ personne** |
| `3,0203` | `The smooth talker has met the perfect person…` | idem — **3ᵉ personne** |
| `4,0208` | `The Smooth Talker wakes up after a night…` | idem — **3ᵉ personne** |
| `7,0207` | `The Smooth Talker and their friend have been planning…` | idem — **3ᵉ personne** |
| `3,0306` | `You get caught in adultery.` | citée « hors cause » **et** incohérente |
| `3,0311` | `During the act, you called your partner by your mother's first name.` | idem |
| `4,0302` | `You're not sick, but you want to go on holiday.` | idem |

5 des 8 rangées citées **ne sont pas en 2ᵉ personne** dans `context` (leur `issue` est à l'impératif,
ce qui a dû les faire remonter) ; les 3 autres sont **incohérentes**. La catégorie « hors cause »
n'a donc **aucun membre reproductible**.

**FP #3 — le `fa = 3`.** La sonde du dossier a compté 3 rangées persanes. Sonde par **token exact**
(`تو` / `شما` isolés) : **0 aux deux révisions**. Et `context_fa` n'a bougé que sur **2 rangées**
(`3.2.12`, `4.3.7`) entre `2a2e7b32` et `21a72385` — hors sujet. Le chiffre ne décrit aucun état du
fichier : c'est un artefact de sonde (le `تو` persan est une **sous-chaîne** fréquente — `توجه`,
`کوتاه`, `دستور`). ⚠️ *Reproduit ici même* : ma première sonde large a rendu `fa = 27` et `pt = 52`
avant resserrement — `seu/sua` sont des possessifs **3ᵉ** personne en portugais, pas une adresse au
joueur. **Une sonde à sous-chaîne sur une langue non latine produit un chiffre plausible et faux.**

### A.3 Le delta pinné

Un seul changement dans le dossier A entre la mesure et aujourd'hui :

| rangée | colonne | `2a2e7b32` | `21a72385` |
|---|---|---|---|
| **`4,0202`** | `drawer` (EN) | `Yourself` | **`A serious trainee`** |

C'est **exactement** le défaut mécanique que le dossier signalait comme « déjà dispatché à
l'écriture » (les deux joueurs fusionnés, `issue` parlant pourtant du *trainee*). Le fix est arrivé.
`context` (25) et `smoothTalker` (12) sont inchangés. Deltas CSV de la fenêtre : #1295, #1297
(06/09), #1305 (07/09), #1308, #1310 (08/09).

### A.4 Le classement re-dérivé **par lecture des 25 rangées**

| | rangées | n |
|---|---|---:|
| **Incohérentes** — le `context` adresse au joueur, l'`issue` repasse à la 3ᵉ (He/They/The) | `3,0305` · `3,0306` · `3,0311` · **`4,0101`** · `4,0103` · `4,0202` · `4,0203` · `4,0302` · `5,0101` · `5,0102` · `5,0201` · `6,0101` · `6,0201` · `6,0301` · `7,0301` · `7,0302` | **16** |
| **Cohérentes** — les deux phrases restent en 2ᵉ personne (impératif ou « You must… ») | `3,0104` · `4,0301` · `7,0102` · `7,0103` · `7,0104` · `7,0105` · `7,0107` · `7,0201` · `7,0206` | **9** |

`4,0101` est le seul écart avec la liste du dossier : `context` « You're a car dealer. » /
`issue` « The smooth talker must sell them… ». Il appartient bien à la classe.

### A.5 Feuille de décision — dossier A (une feuille)

| | |
|---|---|
| **La question** | Quelle **voix** pour l'anglais de Scenarii ? L'EN est la seule à tutoyer le joueur (`context` 2ᵉ personne 25/167, contre 0 dans 6 langues) — et **16 de ces 25 cartes se contredisent en leur milieu**. |
| **(A) Normaliser à la 3ᵉ personne** *(la recommandation d'ai-01)* | **47 cellules** (25 `context` + 12 `smoothTalker` + 10 `drawer`). L'EN rejoint le FR canonique et les 6 traductions ; mécanique, corroborable ligne à ligne contre le FR. **Régén EN.** |
| **(B) Assumer la 2ᵉ personne** | Propager aux **16** rangées qui décrochent **puis** trancher les ~142 rangées EN déjà en 3ᵉ : soit tout basculer (~500 cellules, l'EN diverge alors délibérément des 6 langues), soit acter que l'EN mélange les voix — c'est-à-dire **l'état actuel, le défaut**. |
| **(C) Ne corriger que les 16 incohérentes** | **16 cellules**. Supprime ce qui se voit sur une carte, laisse l'EN seule contre 6 langues sur la voix. |
| **Nature** | Convention éditoriale — **aucune branche ne corrige un contresens**. |
| **Coût** | (A) 47 · (B) 16 à ~500 · (C) 16. Dans les trois cas : **régén EN seule**, les 7 autres langues ne bougent pas. |
| **Ce qu'il faut savoir avant de trancher** | L'EN n'est pas seulement « seule » : elle est **incohérente avec elle-même sur 16 cartes sur 25**. (C) suffit donc à supprimer le défaut *visible sur une carte* ; (A) est le seul choix qui rende l'EN homogène avec les 6 autres langues. |

---

## §B — Dossier B : calque littéral vs nom officiel publié

### B.1 Ce qui est confirmé — intégralement

**Cas 1 — Tournesol, rangée `5,0101`, colonne `piocheur`/`drawer`** — identique au dossier, cellule
par cellule :

| fr | es | pt | zh | ar | fa | **en** | **ru** |
|---|---|---|---|---|---|---|---|
| `Tournesol` | `Tornasol` | `Girassol` | `向日葵教授` | `تورنسول` | `پروفسور تورنسل` | **`Sunflower`** | **`Подсолнух`** |

⇒ **6 langues sur 8 portent le nom officiel** de la traduction publiée ; EN et RU traduisent
littéralement « tournesol ».

**Le faux positif du dossier antérieur — confirmé, et c'est le plus important du dossier B.** Un
rapport antérieur justifiait ces deux cellules en écrivant qu'elles « contredisent la VO
`Calculus`/`Калькулюс` » de la même rangée. Mesure sur **tout le CSV** :

| chaîne | occurrences |
|---|---:|
| `Calculus` | **0** |
| `Калькул` | **0** |
| `Sunflower` · `Подсолнух` · `Tournesol` · `Tornasol` · `Girassol` · `向日葵` | 1 chacune |

⇒ La preuve annoncée **n'existait pas dans le corpus** : la justification était **externe** (les
traductions publiées d'Hergé). Le fait tient, mais il devait être déclaré comme tel — *un
référentiel externe ne s'écrit jamais « le corpus le contredit »*.

**Cas 2 — Schtroumpfs, rangée `5,0102`, colonnes PT** — confirmé :

| colonne PT | valeur | ce que portent les autres |
|---|---|---|
| `smoothTalker_pt` | **`Shtroumphissime`** | fr `Le shtroumphissime` (déjà non canonique en FR : minuscule) · es `El Pitufísimo` · ru `Смурфик-бунтарь` |
| `suggestion_pt` | `Mas o **Grande Smurf** não estaria…` | en `Papa Smurf` · es `Papá Pitufo` · ar `بابا سنفور` · zh `蓝爸爸` · fa `اسمورف بزرگ` · ru `Великий Смурф` |
| `context_pt` | `O embromador é um Smurf…` | officiel dans les 6 autres |

⇒ 2 cellules PT porteuses d'une **forme française brute** (`Shtroumphissime`, mal orthographiée
même en français) ou d'un **calque** (`Grande Smurf`), là où 6 langues portent l'officiel. Bloqué
par une question jamais tranchée : **le deck vise pt-BR ou pt-PT ?** (`Papai Smurf` vs `Papá Smurf`).

### B.2 Les 8 cellules « mécaniques » du même dossier — **toutes soldées**

| attendu | au HEAD |
|---|---|
| `Hadock` → `Haddock` | `Hadock` **0** · `Haddock` **4** ✅ |
| `Cheschire` → `Cheshire` | `Cheschire` **0** · `Cheshire` **8** ✅ |
| `Jeanne d'Arc` → `Joana d'Arc` (pt) | EN `Joan of Arc` ×2 · PT `Joana d'Arc` ×2 ✅ — les 2 `Jeanne d'` restants sont les cellules **FR** de `1,0201` (`titre`, `baratineur`), c'est-à-dire la **VO** : corrects |

Soldées par #1305 (07/09) et #1308/#1310 (08/09). **Rien ne remonte de ce lot.**

### B.3 Feuille de décision — dossier B (une feuille)

| | |
|---|---|
| **La question** | Quand une œuvre a une **traduction publiée** dont le nom de personnage n'est *pas* le calque du français, le deck suit-il le nom officiel, ou garde-t-il le calque ? |
| **Le fait mesuré** | Sur les 8 langues, **6 suivent déjà l'officiel**. Seules **2 cellules** (EN `Sunflower`, RU `Подсолнух`) sont des calques — et les 2 cellules PT du cas 2 sont, elles, des **formes françaises brutes**, pas des calques assumés. |
| **(A) Suivre l'officiel** *(recommandation d'ai-01)* | **4 cellules** (EN, RU + 2 PT). Cohérent avec les 6/8 déjà à l'officiel. **Exige de trancher pt-BR/pt-PT** pour le cas 2. |
| **(B) Garder le calque** | **0 écriture.** Le deck assume une nomenclature dérivée du FR — mais alors ce sont **ES/PT/ZH** qui sont incohérents, pas EN/RU, et c'est **6 cellules** qu'il faudrait aligner **en sens inverse** (+ les 2 PT, qui resteraient des formes françaises brutes : (B) ne les règle pas). |
| **(C) Cas 1 seul** | **2 cellules** (EN, RU). Débloque le certain, laisse le cas 2 en attente de la variante PT. |
| **Nature** | Convention de nommage — pas de goût, pas de contresens. |
| **Coût si GO** | **Édition CSV pure, aucune régénération** (porte groupée fermée : les corrections s'accumulent sur master et paient la régén une fois). |

---

## §C — Ce que ce grain n'a pas fait

⛔ Aucune cellule CSV écrite · ⛔ aucune branche d'arbitrage choisie · ⛔ aucune recommandation
ajoutée à celles d'ai-01 (elles sont **reportées**, pas re-faites) · ⛔ aucune régénération.

Les deux dossiers sont **re-estampés** : leurs chiffres tiennent sur `21a72385` **sauf** les quatre
écarts du §A.1, dont trois sont des **pannes d'instrument du dossier**, pas des dérives du corpus.

---

*po-2024 — grain ② du dispatch #458 c.5656689863. Le worker mesure et signale ; la décision reste
à l'owner.*
