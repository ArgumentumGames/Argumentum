# #994 — Arbitrages A / B re-estampés sur master courant (`21a72385`)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : master `21a72385`
*(`origin/master` a avancé de 3 commits depuis — `874a5d98`, `836d9274`, `26aeaedd` — tous
**docs-only** : le recensement CSV est donc inchangé **par construction**.)*
**Statut** : **RE-MESURE + FEUILLE DE DÉCISION** — lecture seule. ⛔ `0` écriture CSV, ⛔ aucune décision éditoriale prise ici.
**Révision** : `2` — corrections de la revue ai-01 du 14/09 (6 points, tous vérifiés puis appliqués ;
détail par point en §A.2, §A.3, §B.1, §B.2, §B.3, §D).
**Dispatch** : ai-01, deep-queue #458 c.5656689863, grain **②** (`[secondaire] #994 dossiers A/B`).
**Sources amont** : dossier **A** = commentaire #994 du 2026-09-06 (master `2a2e7b32`) · dossier **B** = commentaire #994 du 2026-09-06 (master `1ab6d861`).
**Décision aval** : commentaire owner **#994 `5657115517`** (14/09) — **les deux feuilles sont tranchées** (§D).
**Instrument** : `Cards/Scenarii/Argumentum Scenarii - Cards.csv`, 167 rangées, 70 colonnes ; FR **et EN nus** (`contexte`, `context`), autres langues suffixées.

> La re-mesure n'est pas une formalité : elle **écarte trois chiffres du dossier A** (§A.2 — deux
> pannes d'instrument réelles, `fr` et `fa`) et **confirme intégralement le dossier B** (§B.1), à
> **une cellule près** (§B.1, cas 2 : 3 cellules PT, pas 2). Un arbitrage présenté sur un chiffre
> faux se tranche mal.
>
> ⚠️ Elle a aussi produit **une accusation fausse de ma part**, retirée en §A.2 : j'avais déclaré la
> liste « hors cause » non reproductible en la sondant sur la **mauvaise colonne**. La revue l'a
> relevé. Les deux feuilles §A.5/§B.3 **ne sont plus des questions ouvertes** — l'owner a tranché
> le 14/09 (§D), elles sont conservées comme archives.

---

## §A — Dossier A : la voix de l'anglais dans Scenarii

### A.1 Ce qui tient, au chiffre près

| dimension | dossier (06/09, `2a2e7b32`) | re-mesure (14/09, `21a72385`) | verdict |
|---|---:|---:|---|
| `contexte` 2ᵉ pers. — **fr** | 1 | **0** | ❌ **faux positif d'instrument** (§A.2) |
| `context` 2ᵉ pers. — **en** | 25 | **25** | ✅ |
| — ru · pt · es · ar · zh | 0 | **0** | ✅ |
| — **fa** | **3** | **0** | ❌ **faux positif d'instrument** (§A.2) |
| `smoothTalker` EN 2ᵉ pers. | 12 / 167 | **12** | ✅ |
| `drawer` EN 2ᵉ pers. | 11 / 167 | **10** | ↘ **delta −1, pinné** (§A.3) |
| rangées incohérentes (la carte change de personne) | 15 | **16** | ❌ **`4,0101` manquait** |
| rangées « hors cause » — colonnes de **rôle** | 8 | **8 / 8 reproduites** | ✅ *(requalifiée, §A.2)* |

⇒ **`25 + 12 + 10 = 47`** : la volumétrie de la branche (A) annoncée par ai-01 est **exacte**.

### A.2 Deux faux positifs réels (tous deux du dossier) — et deux accusations retirées (les miennes)

> ⚠️ **Correction du 14/09 (revue ai-01).** La première version de cette section accusait le
> **dossier** de deux fautes supplémentaires (FP #1, FP #2). **Les deux accusations sont retirées** :
> elles venaient d'une sonde de ma part sur la **mauvaise colonne**. Détail ci-dessous, parce que
> l'erreur est la même que celle que la section dénonce.

**FP #1 — les deux listes ne sont pas en conflit : elles ne portent pas sur la même colonne.**
`3,0306`, `3,0311` et `4,0302` figurent bien dans les deux listes du dossier — mais la liste des
**16 incohérentes** décrit la colonne **`context`** (le récit adresse au joueur, puis l'`issue`
retombe à la 3ᵉ personne), tandis que la liste des **8 « hors cause »** décrit les colonnes de
**rôle** `smoothTalker` / `drawer`. Une rangée **peut** appartenir aux deux : l'une parle de la
voix du récit, l'autre du libellé de rôle. Le recouvrement est **légitime**, pas contradictoire.

**FP #2 — la liste « hors cause » est parfaitement reproductible, 8/8.** J'ai lu `context` pour la
vérifier — la **mauvaise colonne**. Sur les colonnes qu'elle décrit réellement :

| rangée | `smoothTalker` | `drawer` | `context` (pour mémoire) |
|---|---|---|---|
| `3,0102` | `You` | `Your conquest` | 3ᵉ pers. |
| `3,0103` | `You` | `Your partner` | 3ᵉ pers. |
| `3,0203` | `You` | `Your partner who has a child` | 3ᵉ pers. |
| `4,0208` | `You` | `Your Boss` | 3ᵉ pers. |
| `7,0207` | `You` | `Your friend` | 3ᵉ pers. |
| `3,0306` | `You` | `Your partner` | **2ᵉ pers.** |
| `3,0311` | `You` | `Your partner` | **2ᵉ pers.** |
| `4,0302` | `You` | `Your doctor` | **2ᵉ pers.** |

**8/8** portent `You` en `smoothTalker` et `Your …` en `drawer` — exactement le motif « You / Your X »
que le dossier citait. La catégorie a donc **8 membres reproductibles**, et sa jonction avec les 16
est un fait mesuré, pas un défaut. ⚠️ **Leçon** : j'avais dénoncé une sonde à **sous-chaîne** (le
`fa` ci-dessous), puis produit une sonde sur la **mauvaise colonne** — deux pannes d'instrument
distinctes, et la seconde est plus difficile à voir parce qu'elle rend un chiffre *exact* sur un
objet qui n'est pas le sujet.

**FP #3 — le `fa = 3` (celui-ci tient).** La sonde du dossier a compté 3 rangées persanes. Sonde par
**token exact** (`تو` / `شما` isolés) : **0 aux deux révisions**. Et `context_fa` n'a bougé que sur
**2 rangées** (`3.2.12`, `4.3.7`) entre `2a2e7b32` et `21a72385` — hors sujet. Le chiffre ne décrit
aucun état du fichier : c'est un artefact de sonde (le `تو` persan est une **sous-chaîne** fréquente —
`توجه`, `کوتاه`, `دستور`). ⚠️ *Reproduit ici même* : ma première sonde large a rendu `fa = 27` et
`pt = 52` avant resserrement — `seu/sua` sont des possessifs **3ᵉ** personne en portugais, pas une
adresse au joueur. **Une sonde à sous-chaîne sur une langue non latine produit un chiffre plausible
et faux.**

**FP #4 — le `fr = 1` (même famille, et le dossier avait raison de le compter… non).** Le dossier
comptait **1** rangée FR en 2ᵉ personne, `3,0106`. Lecture de la cellule :

> *« Lors de son inscription sur un site de rencontres en ligne, le baratineur a utilisé une photo de
> profil mensongère… Au premier **rendez-vous**, son rencard s'apprête à rebrousser chemin. »*

Le `vous` détecté est une **sous-chaîne de `rendez-vous`** — un nom composé, pas une adresse au
joueur. **Valeur correcte : 0.** C'est le **même piège** que le `fa`, cette fois sur une langue
latine : la sonde à sous-chaîne ne distingue pas un mot d'un fragment de mot. ⇒ Le FR canonique
n'adresse **jamais** le joueur, comme les 6 autres traductions : l'EN est **seule** (25/167).

### A.3 Le delta pinné

Un seul changement dans le dossier A entre la mesure et aujourd'hui :

| rangée | colonne | `2a2e7b32` | `21a72385` |
|---|---|---|---|
| **`4,0202`** | `drawer` (EN) | `Yourself` | **`A serious trainee`** |

C'est **exactement** le défaut mécanique que le dossier signalait comme « déjà dispatché à
l'écriture » (les deux joueurs fusionnés, `issue` parlant pourtant du *trainee*). Le fix est arrivé.
`context` (25) et `smoothTalker` (12) sont inchangés.

**Deltas CSV de la fenêtre** — re-dérivés par `git log 2a2e7b32..21a72385 -- "Cards/Scenarii/Argumentum Scenarii - Cards.csv"` :

| commit | PR | date | sujet |
|---|---|---|---|
| `281ffe41` | #1297 | 06/09 | 15 cellules EN défectueuses (lot non corroboré) |
| `bab289c0` | **#1300** | 07/09 | **noms propres — 8 cellules** (Haddock, Cheshire, Poucet PT, Joana d'Arc) |
| `7c40caa6` | #1305 | 07/09 | reliquat hors EN — 31 cellules mécaniques |
| `b7cd6ed0` | #1308 | 08/09 | 11 cellules MINEUR arbitrées |
| `8f370985` | #1310 | 08/09 | 32 cellules MINEUR restantes |

⚠️ **Correction du 14/09.** La première version écrivait « #1295, #1297 (06/09), #1305 (07/09),
#1308, #1310 (08/09) » : elle listait **`#1295`** — qui est `2a2e7b32`, c'est-à-dire **la base de la
fenêtre elle-même**, pas un delta (vérifié : `git merge-base --is-ancestor 2a2e7b32 21a72385` →
vrai) — et **omettait `#1300`**, qui est pourtant le commit qui a soldé le lot §B.2. **5 commits, pas 5
numéros au hasard.**

### A.4 Le classement re-dérivé **par lecture des 25 rangées**

| | rangées | n |
|---|---|---:|
| **Incohérentes** — le `context` adresse au joueur, l'`issue` repasse à la 3ᵉ (He/They/The) | `3,0305` · `3,0306` · `3,0311` · **`4,0101`** · `4,0103` · `4,0202` · `4,0203` · `4,0302` · `5,0101` · `5,0102` · `5,0201` · `6,0101` · `6,0201` · `6,0301` · `7,0301` · `7,0302` | **16** |
| **Cohérentes** — les deux phrases restent en 2ᵉ personne (impératif ou « You must… ») | `3,0104` · `4,0301` · `7,0102` · `7,0103` · `7,0104` · `7,0105` · `7,0107` · `7,0201` · `7,0206` | **9** |

`4,0101` est le seul écart avec la liste du dossier : `context` « You're a car dealer. » /
`issue` « The smooth talker must sell them… ». Il appartient bien à la classe.

### A.5 Feuille de décision — dossier A *(archive pré-GO — tranchée le 14/09)*

> ⚠️ **Cette feuille n'est plus ouverte.** L'owner a tranché le **2026-09-14** (commentaire #994
> `5657115517`), **après** l'ouverture de cette PR : **branche (A) retenue**. Elle est conservée
> comme **archive de la décision**, pas comme question pendante. Le chiffre de volumétrie
> (`25 + 12 + 10 = 47`) est celui que le GO cite — il est **exact** sur la re-mesure courante.

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

### B.1 Ce qui est confirmé — **cas 1 intégralement, cas 2 à une cellule près**

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
| `suggestion_pt` | `Mas o **Grande Smurf** não estaria nada smurfamente de acordo.` | en `Papa Smurf` · es `Papá Pitufo` · ar `بابا سنفور` · zh `蓝爸爸` · fa `اسمورف بزرگ` · ru `Великий Смурф` |
| `context_pt` | `O embromador é um Smurf, um pequeno duende azul das florestas liderado pelo **Grande Smurf**.` | officiel dans les 6 autres |

⇒ **3 cellules PT** (non 2 — correction du 14/09) porteuses d'une **forme française brute**
(`Shtroumphissime`, mal orthographiée même en français) ou d'un **calque** (`Grande Smurf`, présent
dans **deux** cellules : `suggestion_pt` **et** `context_pt`), là où 6 langues portent l'officiel.
`context_pt` compte : le calque y est **rendu** sur la carte, pas seulement dans la suggestion.
Bloqué par une question jamais tranchée : **le deck vise pt-BR ou pt-PT ?** (`Papai Smurf` vs
`Papá Smurf`). Le compteur du dossier B (et de sa feuille) portait **2** — il en manquait une.

### B.2 Les 8 cellules « mécaniques » du même dossier — **toutes soldées**

| attendu | au HEAD |
|---|---|
| `Hadock` → `Haddock` | `Hadock` **0** · `Haddock` **4** ✅ |
| `Cheschire` → `Cheshire` | `Cheschire` **0** · `Cheshire` **8** ✅ |
| `Jeanne d'Arc` → `Joana d'Arc` (pt) | EN `Joan of Arc` ×2 · PT `Joana d'Arc` ×2 ✅ — les 2 `Jeanne d'` restants sont les cellules **FR** de `1,0201` (`titre`, `baratineur`), c'est-à-dire la **VO** : corrects |

⚠️ **Correction du 14/09 — l'attribution était fausse.** La première version créditait
« #1305 (07/09) et #1308/#1310 (08/09) ». Le lot a été soldé par **#1300 seul** —
commit **`bab289c0`**, 07/09, *« fix(scenarii): #1294 proper-noun fixes — 8 cells (Haddock, Cheshire,
Poucet PT, Joana d'Arc) »*, PR **mergée le 2026-09-07T04:08:11Z**, `8 ++++----` sur le CSV.
Vérification : `git log -S'Haddock' -- <csv>` ne rend **que** `bab289c0` sur la fenêtre ; #1305 et
#1308/#1310 portent d'autres lots (31 cellules mécaniques « hors EN », puis les échantillons
MINEUR 11/32). **Rien ne remonte de ce lot** — mais le crédit va au bon commit, et c'est
exactement le commit que le §A.3 omettait.

### B.3 Feuille de décision — dossier B *(archive pré-GO — tranchée le 14/09)*

> ⚠️ **Cette feuille n'est plus ouverte non plus.** L'owner a tranché le **2026-09-14** : **branche
> minimale (C)** — les 2 cellules Tournesol certaines, EN et RU. Les **3 cellules Schtroumpfs PT
> restent hors périmètre** (choix pt-BR / pt-PT non tranché). Chiffres corrigés ci-dessous : le
> compteur PT portait **2** au lieu de **3**, ce qui décalait les options (A) et (B).

| | |
|---|---|
| **La question** | Quand une œuvre a une **traduction publiée** dont le nom de personnage n'est *pas* le calque du français, le deck suit-il le nom officiel, ou garde-t-il le calque ? |
| **Le fait mesuré** | Sur les 8 langues, **6 suivent déjà l'officiel**. Seules **2 cellules** (EN `Sunflower`, RU `Подсолнух`) sont des calques — et les **3 cellules PT** du cas 2 sont, elles, des **formes françaises brutes** ou des calques non assumés. |
| **(A) Suivre l'officiel** *(la recommandation d'ai-01)* | **5 cellules** (EN, RU + **3** PT). Cohérent avec les 6/8 déjà à l'officiel. **Exige de trancher pt-BR/pt-PT** pour le cas 2. |
| **(B) Garder le calque** | **0 écriture.** Le deck assume une nomenclature dérivée du FR — mais alors ce sont **ES/PT/ZH** qui sont incohérents, pas EN/RU, et c'est **7 cellules** qu'il faudrait aligner **en sens inverse** (+ les 3 PT, qui resteraient des formes françaises brutes : (B) ne les règle pas). |
| **(C) Cas 1 seul** | **2 cellules** (EN, RU). Débloque le certain, laisse le cas 2 en attente de la variante PT. ← **choix owner** |
| **Nature** | Convention de nommage — pas de goût, pas de contresens. |
| **Coût si GO** | **Édition CSV pure, aucune régénération** (porte groupée fermée : les corrections s'accumulent sur master et paient la régén une fois). |

---

## §C — Ce que ce grain n'a pas fait

⛔ Aucune cellule CSV écrite · ⛔ aucune régénération · ⛔ aucune recommandation ajoutée à celles
d'ai-01 (elles sont **reportées**, pas re-faites). Les deux feuilles §A.5/§B.3 étaient ouvertes à
l'ouverture de cette PR ; elles sont **requalifiées en archives** depuis le GO du 14/09 (§D).

**Bilan de la re-mesure** : les deux dossiers tiennent sur `21a72385` **sauf** —

| écart | nature |
|---|---|
| `contexte` FR 1 → **0** (`3,0106`) | **panne d'instrument du dossier** — sonde à sous-chaîne (`vous` ⊂ `rendez-vous`) |
| `context_fa` 3 → **0** | **panne d'instrument du dossier** — même famille, langue non latine |
| rangées « hors cause » « non reproductible » → **8/8** | **deux accusations retirées** — *mes* sondes portaient sur `context` au lieu des colonnes de rôle (§A.2, FP #1 et FP #2) |
| `drawer` 11 → **10** (`4,0202`) | **delta réel du corpus** — le défaut signalé a été corrigé entre les deux mesures |
| incohérentes 15 → **16** | **omission du dossier** — `4,0101` manquait à sa liste |
| PT cas 2 : 2 → **3** cellules | **sous-compte du dossier B** — `context_pt` porte le calque, pas seulement `smoothTalker_pt`/`suggestion_pt` |
| fenêtre : `#1295` en trop, `#1300` en moins | **relevé non re-dérivé** — `#1295` **est** la base, `#1300` est le commit du lot §B.2 |
| attrib. §B.2 « #1305/#1308/#1310 » | **fausse** — le lot est `bab289c0` = **#1300** seul |

Soit **deux** pannes d'instrument du dossier (`fr`, `fa`), **deux** accusations de ma part à
retirer, **deux** sous-comptes/erreurs d'attribution du dossier, **un** delta de corpus et **une**
omission. Les trois derniers points de la table relèvent du même défaut d'instrument : un relevé
non re-dérivé.

---

## §D — Les décisions owner, enregistrées (2026-09-14)

Source : commentaire owner **#994 `5657115517`** (14/09, publié par ai-01 sous le token partagé,
**décisions reçues en interactif** : « OK pour toutes tes recos »). Enregistrées ici pour que le
dossier porte l'état, et non plus seulement la question.

### A — voix anglaise des Scenarii → **branche (A)**

Normaliser l'anglais à la **troisième personne**, comme le français canonique et les 6 autres
langues. Périmètre autorisé : **~47 cellules** (`context` 25 + `smoothTalker` 12 + `drawer` 10),
**plus** la correction des **16 cartes** qui basculent de personne en leur milieu.
⚠️ Clauses d'exécution du GO, à respecter telles quelles :
- **re-dériver les cellules cibles depuis le CSV courant avant mutation** ;
- si le chiffre diffère de 47 **avant écriture**, **publier le delta et s'arrêter** — ne pas forcer
  l'attendu ;
- mutation CSV puis régénération **EN seule** = **deux grains séparés** ; verdict visuel = ai-01.

### B — noms officiels publiés → **branche minimale (C)**

Deux cellules certains, corrigées immédiatement :

| langue | colonne | de | vers |
|---|---|---|---|
| **en** | `drawer` (rangée `5,0101`) | `Sunflower` | **`Cuthbert Calculus`** |
| **ru** | `drawer_ru` (rangée `5,0101`) | `Подсолнух` | **`профессор Лакмус`** |

**Les 3 cellules Schtroumpfs PT restent hors périmètre** — aucune décision prise sur elles, elles
dépendent du choix pt-BR / pt-PT. *(Le GO les compte « trois » : c'est la mesure corrigée du §B.1,
et c'est le chiffre que la feuille aurait dû porter dès l'origine.)*

---

*po-2024 — grain ② du dispatch #458 c.5656689863. Le worker mesure et signale ; la décision reste
à l'owner.*
