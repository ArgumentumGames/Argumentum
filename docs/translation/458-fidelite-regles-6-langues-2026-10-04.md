# #458 — Fidélité des **Règles** : les 6 langues cibles, lues contre la référence (0 écriture)

**Objet** : `Cards/Rules/Argumentum Rules - Cards.csv` — **15 documents**, **8 langues**.
**Portée de ce dossier** : les **6 langues cibles restantes** (ru, pt, ar, es, zh, fa), lues
**contre** la paire source FR | EN auditée la veille (`458-fidelite-regles-sources-fr-en-2026-10-04.md`,
PR #1757).
**Statut** : **0 écriture** — aucune cellule CSV modifiée. Les défauts sont **localisés**, pas réparés.

⚠️ **Ce dossier est un relevé, pas un verdict éditorial.** Il dit *où* une langue rompt sa propre
continuité et *où* elle s'écarte de la source. Il ne dit pas quel variant est le bon : pour
« مَغلوطة » contre « مَغالِطة », pour « 序列 » contre « 阶层 », c'est un jugement de locuteur natif.

**Le corpus est désormais lu en entier** : 15 × 2 sources (30 cellules, dossier précédent) +
15 × 6 langues cibles (90 cellules, ici) = **120 / 120**.

---

## 1. Méthode

Trois instruments, du plus large au plus fin :

| Axe | Ce qu'il compare | Indépendant de la langue ? |
|---|---|---|
| **Structure** | titres `#`, puces `*`, **emoji** — par cellule, 8 langues | **oui** — un emoji est un emoji |
| **Blocs** | découpage en paragraphes (lignes vides) et **ponctuation finale de chaque bloc** | oui |
| **Termes** | le mot retenu pour un objet donné, cellule par cellule | non — demande la lecture |

⭐ **L'emoji est le seul marqueur structurel que la traduction ne touche pas.** C'est donc le
meilleur détecteur d'omission : si le FR porte 30 emoji et la langue en porte 12, rien ne l'explique.

⚠️ Les trois axes sont restés **mécaniques** ; le verdict vient de la lecture, langue par langue,
des 90 cellules.

---

## 2. Ce que la structure **établit** (et un soupçon qu'elle tue)

```
                          fr   en   ru   pt   ar   es   zh   fa
titres / puces / emoji    49   49   49   49   49   49   49   49
                          32   32   32   32   32   32   32   32
                          52   52   52   52   52   52   52   52
```

**Identiques dans les huit langues, à l'unité près.** ⇒ **Aucune omission structurelle dans tout
le corpus des Règles** : pas un titre, pas une puce, pas un emoji perdu — dans aucune langue.

### Le soupçon « zh tronqué » — levé

Le chinois pèse **5 069 caractères contre 16 499 en FR** : un tiers. Un écran qui compare des
**longueurs** crie à la troncature. Il a tort, et la structure le prouve :

- zh porte **18 lignes non vides** dans Rules_15, exactement comme les sept autres ;
- zh porte **49 / 32 / 52** titres, puces et emoji, comme tout le monde ;
- zh porte **97 nombres contre 96** en FR — il en a **un de plus**, pas sept de moins.

⭐ *Une langue dense n'est pas une langue courte.* L'écart de longueur est une propriété du
système d'écriture, pas un signal de perte — et seule une mesure **indépendante de la langue**
peut trancher.

---

## 3. Deux **faux positifs** que la lecture a évités — et ils comptent plus que les vrais

Cette section est la plus utile du dossier. Deux fois, un écran mécanique a produit une conclusion
fausse, et deux fois c'est la lecture du **contexte** qui l'a désamorcée. Les deux mécanismes se
reproduiront sur d'autres corpus.

### 3.1 `zh` : « deux mots pour un concept » — **faux**

**Mesure** : le terme central du jeu se partage **33 / 27** entre `谬误` et `诡辩`.

| Cellules | Terme |
|---|---|
| Rules_02, 03, 07, 09, 11, 13, 14, 15 | **谬误** (miùwù — *erreur, sophisme*) |
| Rules_04, 05, 06 (et Rules_12, mélangé) | **诡辩** (guǐbiàn — *sophistique*) |

Un partage de 33 / 27 sur le mot le plus important du jeu : de quoi écrire « incohérence majeure ».

**Lecture** : les deux mots ne désignent **pas le même référent**.
`谬误论证` = « argument fallacieux » — **l'objet**. `诡辩者` = « baratineur » — **le rôle**.

**Vérification** : sur **27 occurrences** de `诡辩`, **27 sont suivies de `者`**. Zéro exception.

⇒ **Ce n'est pas une incohérence, c'est une distinction juste** — et elle tombe exactement là où
les deux concepts coexistent. `Rules_04` porte les deux (`谬误论证` pour l'objet, `诡辩者` pour le
rôle) parce que la cellule parle des deux.

⛔ **Le contre-exemple qui rend la leçon opérante** : le même genre de partage, en **farsi**, est
un **vrai** défaut — parce que là, les deux mots visent le **même** référent (§6.5).

⭐ *Un partage de fréquence n'est pas un défaut de terminologie tant que les référents n'ont pas
été lus.* Compter les mots ne dit rien ; compter les **choses** qu'ils nomment, si.

### 3.2 Les paragraphes « fusionnés » de `ar/es/zh/fa` — **faux**

**Mesure** : sur les 15 cellules, `ar`, `es`, `zh` et `fa` ont **exactement les mêmes comptes de
blocs** — et ils diffèrent du FR sur trois cellules :

| Cellule | fr / en / ru / pt | ar / es / zh / fa |
|---|---|---|
| Rules_10 | 11 blocs | **9** |
| Rules_14 | 9 blocs | **6** |
| Rules_15 | 18 blocs | **11** |

`Rules_15` : 18 lignes non vides réparties en **18 blocs** d'un côté, **11 blocs** de l'autre —
7 lignes ont perdu leur ligne vide de séparation. En markdown, sept règles indépendantes
deviendraient **un seul paragraphe** : un bloc de règles agglutiné. Le défaut serait visible à
l'impression.

**La vérification qui l'annule** — le rendu réel. Le gabarit Rules passe le texte par le helper
`{{markdown Text}}` (`Argumentum_Rules_fr.json`), et ce helper fait :

```javascript
Handlebars.registerHelper("markdown", function (md) {
   marked.setOptions({ breaks: true });      // Generation/CardPen/js/main.js:1408
```

`breaks: true` = **chaque retour à la ligne devient un `<br/>`**. Une ligne seule suffit donc à
produire un saut de ligne visuel : les **18 lignes** de `Rules_15` s'affichent sur **18 lignes**
dans les huit langues. Ni contenu perdu, ni texte agglutiné.

Ce qui diverge est la **structure HTML** (`<br>` dans un `<p>` contre des `<p>` séparés), ce qui
peut déplacer l'interlignage — ⛔ et c'est **précisément ce que ce dossier n'établit pas** (§8).

⭐ **C'est l'option du moteur de rendu qui décide si un écart de structure est un écart de
contenu.** Sans lire `main.js`, j'écrivais « sept règles fusionnées en un paragraphe » : faux, et
invérifiable sur le CSV seul.

---

## 4. La famille `PONCT` est **deux fois plus large** que l'écran ne le voit

L'instrument (PR #1755) teste la ponctuation finale du **dernier caractère de la cellule** : il
rend **8 cellules** (fin de Rules_08 et Rules_15, en ar/es/zh/fa).

En testant la ponctuation finale **de chaque bloc** — la mesure que l'écran ne fait pas :

| Langue | Blocs sans ponctuation finale | Cellules |
|---|---:|---|
| fr / en / ru / pt | **1 / 1 / 1 / 1** | le **même** bloc (Rules_06) — artefact de source partagé |
| ar / fa | 4 | Rules_06 †, **Rules_08**, **Rules_12**, Rules_15 |
| es | 4 | idem |
| zh | 5 | idem + un second bloc dans Rules_15 |

† le bloc commun aux huit langues.

⇒ **12 cellules, pas 8** — et la cellule manquante est **Rules_12**, que l'écran ne pouvait pas
voir. La famille est **strictement** le bloc `ar/es/zh/fa`.

⭐ *Un écran qui ne regarde que le dernier caractère mesure une cellule sur quinze par cellule.*
La généralisation est gratuite — il suffisait de déplacer la sonde.

---

## 5. Le bloc `ar · es · zh · fa`

Deux axes **indépendants** convergent sur la même partition :

- la **ponctuation finale manquante** : 12 cellules, toutes dans ces 4 langues (fr/en/ru/pt : 1) ;
- les **comptes de blocs** : identiques entre ces 4 langues, dans les 15 cellules.

Et l'écran avait déjà trouvé la même famille seul (`PONCT`, 8 cellules). Trois mesures, une
partition. ⚠️ Les quatre langues ont donc été traitées **ensemble** — c'est une **signature de
lot**, pas quatre coïncidences. À traiter comme telle : une correction qui n'en viserait qu'une
laisserait les trois autres.

---

## 6. Défauts, langue par langue

Chaque entrée est **mesurée** et **localisée**. Sauf mention contraire, la cellule fautive est
nommée et le reste du corpus ne la porte pas.

### 6.1 `ru` — le plus chargé

| # | Cellule | Défaut | Mesure |
|---|---|---|---|
| 1 | **Rules_11** | **Fragment orphelin.** `…для добора. ые в середине стола.` — la queue d'une phrase dont la tête a disparu, **au milieu de la cellule** | index 965 sur 1 071 caractères |
| 2 | **Rules_12** | **Deux coquilles dans une phrase** : `вытазить` (→ *вытянуть*) et `слудующую` (→ *следующую*) | forme correcte : **absente** |
| 3 | **Rules_12** | `набравший **на** 20 очков` — le `на` fait lire « gagné **de** 20 points » | `набравший 20` : absent |
| 4 | **Rules_09, 11** | `3 семей` là où Rules_02 et Rules_13 écrivent `3 семейства` | frontière de mot ; Rules_02 : `семейств` ×2 |
| 5 | **Rules_14** | `28 карт с **ложными аргументами**` — seule cellule du corpus à ne pas dire `софизмы` | `софизм` ×32 ailleurs |
| 6 | **Rules_02** | En-tête `В комплект входит:` où les quatre autres disent `Потребуется:` | voir §6.4 |
| 7 | **Rules_02** | Deux noms pour le même objet **dans un document** : `Карты Мемо` (liste) / `Карты-памятки` (prose) | `памятк` : 1 occurrence, Rules_02 seul |
| 8 | **Rules_13** | `1 колода **из** 32 карт` **fusionne** les deux énoncés du FR (« un paquet » / « une sélection de 32 ») ⇒ **efface la trace de la contradiction de la source** (32 vs 28) | Rules_13 seul |
| 9 | **Rules_06** | Deux séquences emoji gagnent un `+` : `✅+1🎴` contre `✅ 1🎴` chez les 7 autres langues | **7 langues identiques au FR** |
| 10 | **Rules_14** | 4 paragraphes du FR fusionnés en 1 (9 blocs → **5**) | cf. §3.2 — non concluant seul |

⚠️ **(4) est mesuré, non tranché** : `семей` est le génitif pluriel de `семья`, `семейство` le terme
taxinomique employé ailleurs. Abréviation ou autre nom — **les deux lectures sont nommées, aucune
n'est retenue ici**.

### 6.2 `pt`

| # | Cellule | Défaut |
|---|---|---|
| 1 | **Rules_09** | Seule cellule au **tutoiement singulier** (`forme`, `escolha`, `embaralhe` ×6) là où Rules_03/11/12/13 sont au **pluriel** (`formem`, `escolham` ×16) |
| 2 | **Rules_05** | `do esquete` — **masculin**, alors que Rules_02/04/06 disent `uma esquete` / `a esquete` : le mot est féminin |
| 3 | **Rules_11** | `Cartas de ajuda-**memória**` où Rules_02/09/13 disent `Cartas de ajuda` |

### 6.3 `es`

| # | Cellule | Défaut |
|---|---|---|
| 1 | **Rules_11 et Rules_13** | **Trois termes** pour les cartes mémo : `Cartas recordatorio` (02, 09) · `Cartas de ayuda` (11) · `Cartas de memo` (13) |
| 2 | **Rules_07** | Seule cellule à dire `falacias argumentativas` (`×4`) où les autres disent `argumento falaz` — **îlot lexical** |
| 3 | **Rules_15** | `palos de triunfo` (`×2`) alors que **Rules_14**, document immédiatement précédent, dit `colores de triunfo` (`×2`) |
| 4 | **Rules_12** | **Acteur supprimé** : `Empieza a robar cartas…` **sans sujet**. FR/EN/PT nomment le baratineur (`Le baratineur` / `The smooth talker` / `O embromador`) |

### 6.4 `ar`

| # | Cellule | Défaut |
|---|---|---|
| 1 | **Rules_09, 13** | `الحجج المغالِطة` où les **huit autres** cellules disent `المغلوطة` (×**22** — compte re-mesuré, erratum §10) |
| 2 | **Rules_06** | `الرصيد` (**solde, crédit**) pour « la réserve », où Rules_03 dit `المخزون` — et `الرصيد` ×3 dans la même cellule |
| 3 | **Rules_09** | `حزمة` (×2) et `رزمة` (×2) nomment **le même paquet de cartes dans le même document** — *même famille que `pack`/`package` en EN* |
| 4 | **Rules_11** | `كومتان` pour les pioches de scénario, où Rules_03/09/13 disent `رُزمتان` |
| 5 | **Rules_02** | En-tête `المواد` où les quatre autres disent `المكوّنات` |

### 6.5 `fa` — le défaut le plus net du corpus

| # | Cellule | Défaut |
|---|---|---|
| 1 | **Rules_14 vs Rules_15** | **Deux mots pour « atout »**, dans deux documents consécutifs : `اَتو` (×5, l'emprunt au français) en Rules_14, `حکم` (×6, le terme natif) en Rules_15. **Zéro recouvrement** — ⚠️ citation corrigée, voir §10 |
| 2 | **Rules_02, 13** | `مغالطی` là où les **dix autres** cellules disent `مغالطه‌آمیز` — même référent, deux adjectifs |
| 3 | **Rules_05, 06** | **Chiffres ASCII** là où les **dix autres** cellules écrivent en **chiffres persans** : `3 یا 4` (×2) contre `۳۲`, `۲۰`… — ⚠️ citation corrigée, voir §10 |
| 4 | **Rules_02** | En-tête `تجهیزات` où les quatre autres disent `محتویات` |
| 5 | **Rules_11** | `بسته` pour le paquet, où Rules_02/09/13 disent `دسته` |
| 6 | **Rules_13** | **Rupture de registre dans la cellule** : passif/impersonnel puis **impératif** (`تشکیل دهید`, `قرار دهید` — ×2) là où Rules_03/09/11 emploient le « nous » (`تشکیل می‌دهیم` — ×15) |

⚠️ **(1)** est le défaut le plus saillant : le mécanisme central du jeu (l'atout) change de nom
entre deux pages de règles consécutives. **(3)** est un **angle mort de l'instrument** — voir §7.

### 6.6 `zh`

| # | Cellule | Défaut |
|---|---|---|
| 1 | **Rules_09** | `序列` pour « ordre » là où Rules_02 et Rules_13 disent `阶层` |
| 2 | **Rules_11** | `目` / `科` pour les deux niveaux de taxinomie, là où les autres disent `阶层` / `家族` |
| 3 | **Rules_11** | **Composé inversé** : `论证谬误` là où tout le corpus dit `谬误论证` — et **classificateur** `套` (×2) là où Rules_02/09/13 disent `副` (×2 chacun) |
| 4 | **Rules_12** | **hérite** du composé inversé, via le nom de pioche cité (`"论证谬误"卡`) |

⇒ `Rules_11` porte à lui seul **quatre** ruptures en chinois (2, 3 et l'en-tête `材料` est correct,
lui). C'est la cellule la plus divergente du corpus, toutes langues confondues.

---

## 7. Deux angles morts de l'instrument, découverts ici

1. **La ponctuation de fin de bloc** (§4) — l'écran ne teste que le dernier caractère de la
   cellule : famille sous-estimée de 8 à 12 cellules.
2. **Le script des chiffres** — l'instrument **normalise** les chiffres arabo-indiens et persans
   vers l'ASCII (défaut 2 de la PR #1755, réparation **juste** : sans elle, un nombre en chiffres
   natifs était lu absent). Mais cette réparation rend l'écran **structurellement aveugle** à une
   cellule qui change de script : `fa` Rules_05 et Rules_06 écrivent en ASCII, les dix autres en
   persan, et l'écran ne peut plus le voir.

⭐ *Un instrument réparé pour comparer peut perdre la faculté de voir ce qu'il compare.* La
normalisation était le bon geste pour l'**égalité** ; elle ne l'est pas pour la **forme**. Les deux
mesures doivent coexister.

---

## 8. Ce que ce dossier **n'établit pas**

- ⛔ **Quel variant est le bon.** Pour `مغلوطة` / `مغالِطة`, `序列` / `阶层`, `اَتو` / `حکم` :
  c'est un jugement de locuteur natif. Ce dossier dit *où* la continuité rompt, pas *comment* la
  rétablir.
- ⛔ **La conséquence visuelle de l'écart de structure des blocs** (§3.2). Le rendu conserve les
  18 lignes, mais la différence `<p>` / `<br>` peut jouer sur l'interlignage — **non mesuré ici**,
  et non mesurable sur le CSV seul.
- ⛔ **L'existence d'un contresens.** Les trois axes sont mécaniques. Un contresens qui n'altère ni
  le nombre, ni la structure, ni un terme inventorié **échappe à ce dossier** : la lecture des 90
  cellules en a relevé (§6), elle ne les a pas épuisés.
- ⛔ **Le partage d'en-tête `Matériel` de `ru` / `ar` / `fa`** (§6.1-6, §6.4-5, §6.5-4). Les trois
  langues séparent `Rules_02` des quatre autres cellules au **même endroit**, chacune avec deux
  mots valides. **Deux lectures, aucune tranchée** : signature d'une passe de traduction commune,
  ou traitement naturel de « la règle principale » contre « les variantes ». Le parallélisme de
  trois langues indépendantes est le fait ; son explication ne l'est pas.
- ⛔ **La fidélité de `en`.** Il a été audité dans le dossier précédent (#1757), pas ici ; et
  `Rules_09`/`Rules_11` y portent `pack` **et** `package` dans la **même cellule** — c'est-à-dire
  exactement le défaut relevé en `ar` (§6.4-3), **deux langues, deux fois**.
- ⛔ **Le statut de `tied` (EN Rules_05)** — toujours non tranché, cf. #1757 §3.5.

---

## 9. Reproductibilité

```bash
# l'ecran (self-test 17 temoins, PUIS l'ecran, PUIS le controle de citation) -- PR #1755 + grain 9
python docs/corpus/regles-fidelity-instrument.py

# les axes de ce dossier : structure (titres/puces/emoji), blocs, ponctuation par bloc,
# script des chiffres, termes par cellule. Le CSV est lu par NOM DE COLONNE, jamais par position.
```

⚠️ **Deux pièges de mesure rencontrés dans ce dossier même**, tous deux à garder :

1. **La sous-chaîne qui matche dans le mot voisin.** `3 семей` apparaît dans `3 семейства` : mon
   premier comptage donnait 4 cellules fautives, le vrai en donne **2**. Et `目` (taxinomie) matche
   dans `目标` (« objectif ») : une cellule saine était signalée.
2. **Le mauvais périmètre.** Compter les chiffres **titre inclus** faisait apparaître un
   « mélange » de scripts en `fa` que le corps seul dément — les numéros `### 1.` sont en ASCII
   dans **toutes** les langues. Sur le corps seul : `fa` n'a **aucun** mélange intra-cellule, il a
   **deux groupes** de cellules.

⭐ *Avant de conclure sur un comptage, mesurer que la sonde atteint la bonne chose* — et le
re-mesurer quand le premier résultat est spectaculaire.

---

## 10. Erratum — trois citations remises sur la cellule, et un compte (ajouté le 2026-10-04)

La première est relevée par ai-01 au merge (`49fdb213`, c.5980906972) ; les deux autres sont
sorties **de la re-mesure même** qui corrigeait la première — le défaut cité n'était pas seul
de sa classe. Chaque correction est **mesurée sur le CSV** avant d'être écrite.

1. **L'atout fa (§6.5-1).** Ce dossier citait `اتو` — forme **nue**. La cellule écrit `اَتو`,
   **avec la fatha** (U+064E), **5 fois** en Rules_14 et nulle part ailleurs ; la forme nue
   apparaît **0 fois** dans tout le CSV. **Le défaut tient intégralement** : `حکم` (keheh)
   ×6 en Rules_15, `اَتو` ×0 en Rules_15, zéro recouvrement — seule la citation était fausse.
2. **Le contraste des chiffres fa (§6.5-3).** La forme de droite « `۳ یا ۴` » était une forme
   **idéale**, pas une cellule : **0 occurrence** dans le CSV. Le contraste réel : `3 یا 4`
   (×2, Rules_05 et 06, ASCII) contre les chiffres persans du reste du corpus (`۳۲`, `۲۰`…).
   Le défaut tient — Rules_05 et 06 restent les seules cellules au corps ASCII.
3. **Le compte `المغلوطة` (§6.4-1).** « ×24 » → **×22**, re-mesuré cellule par cellule :
   Rules_02 ×4, 03 ×3, 04 ×2, 07 ×5, 11 ×5, 12 ×1, 14 ×1, 15 ×1. Les **huit cellules**
   annoncées sont justes ; le total ne l'était pas.

**Le contrôle de citation, dit.** Le contrôle d'alors (corps de la PR #1758) annonçait
« 56 fragments présents, 0 absent, quatre témoins inverses ». Son script n'a pas survécu à la
session — le scratchpad de reprise n'en porte aucune trace (mesuré) — et on ne peut donc plus
établir **s'il retirait les voyelles avant de chercher, ou si la forme nue n'y figurait pas**.
Les deux hypothèses restent ouvertes ; aucune n'est retenue. Ce qui est mesuré : la forme nue
= 0 dans le CSV — « 0 absent » ne pouvait donc pas être le résultat d'une comparaison
mot pour mot qui aurait porté cette forme.

Le contrôle est désormais **committé** dans l'instrument (`docs/corpus/regles-fidelity-instrument.py`,
grain 9) : **61 fragments** comparés **mot pour mot** — aucune normalisation (harakat, ZWNJ,
keheh/kaf, script des chiffres). La forme porteuse de fatha et sa forme nue y sont épinglées
par un témoin dédié, et **deux mutations d'attente** (la forme nue inversée ; `вытазить`
attendu absent) ont rougi le contrôle **en nommant leur fragment**, code de sortie 1, avant
restauration vérifiée (sha256).

⭐ *Une citation de cellule se copie depuis la mesure, jamais depuis la mémoire — une fatha
est invisible à l'œil et fait pourtant toute la différence entre « cité » et « inventé ».*

*po-2024*
