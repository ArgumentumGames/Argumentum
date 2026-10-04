# Campagne de fidélité Scénarios — **en** (0 écriture, lecture à deux voies FR | EN)

**Mandat** : pool #458, renvoi c.5977925408 (sur la PR #1747) + dispatch c.5977929362 — les
six dossiers de la première passe étaient des **écrans mécaniques, pas des lectures** ; reprise
en **une PR par langue** avec lecture intégrale. Le présent dossier livre **en**, la **septième
et dernière** passe (ordre : ar → fa → es → ru → pt → en).
**Objet** : les 167 cartes du deck Scénarios, champs rendus par le gabarit de carte.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

---

## 1. Méthode

Corpus **deux voies** FR | EN généré depuis le CSV (`dump2way_en.py`, scratchpad de session,
régénérable en une passe) : 167 cartes × 6 champs rendus × **FR | EN** = **2338 lignes**, lu
intégralement en 3 passes. Chaque écart est relu contre la cellule brute du CSV avant d'être
écrit (aucune citation de mémoire ; les cellules citées au §3 ont toutes été ré-affichées
depuis le CSV).

**Pourquoi deux voies et non trois.** Les six passes précédentes lisaient `FR | EN | langue
cible`, l'EN servant de **seconde source** pour juger la langue cible. Ici l'EN **est** la
langue cible — et c'est la même colonne qui servait de référence aux six autres. La passe en a
donc une fonction qu'aucune autre n'a : elle **note la source EN elle-même**, et convertit
l'énoncé « N langues suivent l'EN » en « l'EN dévie à la cellule X ; N−k langues en ont
hérité » (§4).

**Garde d'instrument.** Le premier relevé a été jeté : mes indices de colonnes étaient décalés
d'une position, ce qui faisait lire `category_ru` comme colonne de suggestion et rendait
« 167 suggestions sur 167 sans ponctuation finale ». Le script a été réécrit **piloté par
l'en-tête**, avec `HEADER GUARD` (échec bruyant si une colonne manque) et **self-test** sur une
cellule connue à la main (1.1.1). *Un `167/167` sur un champ qui devrait être quasi ponctué
était le symptôme ; la cause était une colonne, pas un corpus.*

---

## 2. Écran (mécanique — mesuré, restitué)

| Mesure | **EN** | Contrôle **FR** |
|---|---|---|
| Couverture | **1002 / 1002 cellules, 0 vide** (167 × 6) | 1002 / 1002, 0 vide |
| Ponctuation finale absente (501 cellules ctxt/enjeu/sugg) | **2** — 2.2.1 sugg, 6.3.2 sugg | **0** |
| Famille « - » (espace avant trait d'union) | **1** — 5.3.2 bara `An anti -vaccin` | 0 |
| bara/pioch à initiale minuscule (334 cellules) | **32** | **4** |
| Casse du terme récurrent (ctxt/enjeu/sugg) | **4 variantes** : `The smooth talker` 140 · `the smooth talker` 21 · `The Smooth Talker` 13 · `the Smooth Talker` 1 | **2 variantes** : `Le baratineur` 154 · `le baratineur` 20 |
| Orthographe GB vs US | **GB 3** (neighbour, favour, labour) / **US ≈ 49** (neighbor 28, organiz 7, favor 4, skeptical 4, labor 3, honor 1, behavior 1, defense 1) | — |
| Tiret cadratin (U+2014) | **5 cellules** | **0** |

**Instrument écarté** (et pourquoi). Le filtre « titre EN ne partage aucun mot de contenu avec
le FR » rend **plus de 60 cartes**, dont `Jeanne d'Arc / Joan of Arc`, `Pain d'épices /
Gingerbread`, `Le Joueur de flûte / The Pied Piper`, `La Belle au bois dormant / Sleeping
Beauty` — toutes **traductions justes**. L'instrument mesure *traduit*, pas *défaillant* : il a
été **écarté**, pas resserré. Seul compteur de titres retenu : l'**écart de longueur** (> +12
caractères), qui isole 4 cartes — dont **une seule** défaillante (« Lendemain difficile »), les
trois autres étant un idiome rendu par un idiome (3.1.4) ou une explicitation (4.1.11, 6.2.6).
Les cinq autres titres substitués (§3 A) ont la **même longueur** que le FR : aucun compteur de
taille ne les voit.

⚠️ **Ce que l'écran ne dit pas.** Il rendrait l'EN quasi propre : 2 points manquants sur 501
cellules, 1 « - », 3 orthographes GB. La lecture établit **37 écarts**, dont 5 titres
substitués, 5 contenus substitués, 2 amputations et 3 contresens. Aucun des gestes de §3 A,
§3 B, §3 C et §3 D n'est visible à l'écran.

---

## 3. Défauts EN établis (lecture FR | EN)

### A. Titres substitués (6)

| Carte | FR | EN | Nature |
|---|---|---|---|
| **3.1.2** | « Lendemain difficile » | `Did you see yourself when you drank` | le plus grave : un titre **remplacé par une phrase** — qui n'est ni la traduction du FR, ni un titre (interrogative, sans point d'interrogation). Le pioch de la même carte dit `His conquest`, le ctxt décrit le réveil : rien ne rattache ce titre à la carte |
| **3.2.15** | « Le t-shirt taché » | `The Spaghetti T-Shirt` | le titre **déplace le sujet** du vêtement taché vers l'aliment. Même longueur que le FR — invisible à tout compteur de taille |
| **4.1.1** | « Rouler des mécaniques » | `Rolling mechanics` | l'idiome (frimer, crâner) **traduit mot à mot** : le titre EN ne signifie rien |
| **5.3.1** | « Débat avec un terraplaniste » | `Flat Earth Society` | le titre remplacé par le **nom d'une organisation** (qui n'apparaît pas dans le titre FR) |
| **5.3.4** | « La conspiration de la 5G » | `5g` | titre **réduit à un sigle minuscule** — sans article, sans mot |
| **7.1.5** | « La kermesse » | `Kermesse` | mot **français non traduit** dans le titre — alors que la **même carte** traduit `fair` dans le ctxt et l'enjeu (§3 G) |

### B. Contenus substitués (5)

| Carte | Champ | FR | EN | Nature |
|---|---|---|---|---|
| **3.1.1** | sugg | « Laissez-moi tenter ma chance ce soir ; si j'échoue, je vous laisse le champ libre. » | `We can both try, we'll see who gets picked.` | la **supplique** devient une **proposition de compétition** : le baratineur ne renonce plus à rien. **Source** des langues qui « suivent l'EN » (§4) |
| **6.1.1** | sugg | « Pour restaurer la confiance, il faut des règles claires : le délai doit courir à partir des faits, comme en droit commun. » | `It is absolutely necessary to rebuild the people's confidence in the political class.` | substitution complète : perdus le **mécanisme légal** (le délai, les faits, le droit commun) — c'est-à-dire l'objet même de la carte |
| **5.2.5** | enjeu | « Il doit convaincre Rachel qu'il ne l'a pas trompée. » | `She considers it an infidelity: try to prove her wrong.` | l'enjeu **change de sujet** (l'état de Rachel au lieu de la tâche du baratineur) et devient un impératif ; **« Rachel » disparaît** |
| **1.3.2** | bara | « Le président des États-Unis » | `Truman` | le **rôle** remplacé par le **nom propre** — geste inverse de celui de 1.1.1 |
| **2.2.5** | bara | « Un juge des affaires familiales » | `Salomon` | le rôle remplacé par le nom — **qui est déjà le titre de la carte** : le bara ne dit plus rien de la fonction |

### C. Amputations (2)

| Carte | Champ | FR | EN | Nature |
|---|---|---|---|---|
| **4.2.1** | sugg | « …ce poulet rôti dans votre assiette ? **Je croyais qu'on était en rationnement.** » | `…what's this spit-roasted chicken on your plate?` | la **dernière phrase est coupée** : le rappel du rationnement — le motif de la carte — disparaît. (« rôti » devient `spit-roasted`, sur-spécifié) |
| **5.1.2** | **bara + sugg** | « Le **shtroumphissime** » / « ne serait **schtroumpfement** pas d'accord » | `the smurf` / `But Papa Smurf would disagree...` | **les deux créations lexicales tombent** : le superlatif du bara et l'adverbe du sugg. L'EN garde les noms canoniques (Smurf, Papa Smurf, Hefty Smurf) mais **ne forge rien**. **Défaut-signal de la campagne** : les 6 langues lues **recréent** le jeu de mots, l'EN le perd |

### D. Contresens (3)

| Carte | Champ | FR | EN | Nature |
|---|---|---|---|---|
| **4.3.1** | sugg | « voilà que j'ai **la berlue** ! » | `My God, I'm so **dizzy**!` | « avoir la berlue » = *voir ce qui n'existe pas* ; `dizzy` = *avoir la tête qui tourne*. es («viendo visiones»), ru («затмение»), pt («alucinações») réparent par leur idiome |
| **7.2.8** | sugg | « vous ne croyez pas que vous **allez un peu vite en besogne** ? » | `don't you think **you're going to work a bit**?` | contresens complet — et la phrase EN est **agrammaticale** (`Me too it makes me happy…`, §3 H). ar · fa · es · ru réparaient |
| **2.2.1** | sugg | « Vade retro**,** Satanas**.** » | `Vade retro Satanas` | la formule latine est conservée mais **la virgule et le point tombent** — 1re des **2 seules** omissions de ponctuation finale de tout le bloc EN |

### E. Dilutions (3)

- **5.3.2** enjeu — « de **refuser le vaccin** » → `to refuse to benefit from it` : l'objet
  (le vaccin) disparaît derrière un pronom.
- **3.2.2** sugg — « mes **affaires** » → `my clothes` : le générique devient spécifique.
- **5.3.1** enjeu — « le **cosmonaute** est un imposteur » → `the latter is an impostor` : le
  référent devient opaque.

### F. Ajouts (5)

- **3.2.8** ctxt — + `all-important` : qualificatif absent du FR. **Source** de la famille
  suivie par fa · ru · pt.
- **4.1.1** ctxt — + une phrase entière (`The smooth talker is a car dealer.`) absente du FR.
- **7.1.2** ctxt — + une phrase entière (`The smooth talker is a kid at school.`).
- **6.3.1** ctxt — + `is a candidate in a major national election` ; et **perd** « pendant la
  campagne d'une élection majeure » du FR (déplacement, pas seulement ajout).
- **3.1.3** ctxt — « sans consentement » → `without **his partner's** consent` : explicitation.

### G. Incohérences internes à la carte (3)

| Carte | Constat |
|---|---|
| **2.3.5** | pioch `the statue of the governor` contre son **propre** ctxt et son **propre** enjeu (`the specter`) — le FR dit « Le spectre » partout. **Et** le ctxt écrit `spect**er**` (US) tandis que l'enjeu écrit `spect**re**` (GB) : **deux orthographes dans la même carte** |
| **7.3.2** | pioch `A relative` contre son **propre** enjeu (`the parents`) — le FR dit « Un parent », et le ctxt FR parle « des parents » |
| **7.1.5** | titre `Kermesse` (mot FR) contre ctxt/enjeu `fair` (mot EN) : le titre et le corps de la même carte ne sont pas dans la même langue |

### H. Grammaire et orthographe (6)

- **4.2.2** ctxt — `It's five o'clock in the morning, although already drunk, the smooth talker
  decides…` : proposition détachée sans sujet (calque de la construction absolue FR).
- **7.2.8** sugg — `Me too it makes me happy to see you again` : agrammatical.
- **6.1.4** sugg — `Our country had prepared.` : calque de « était préparé » ; `had prepared`
  attend un objet.
- **4.3.3** ctxt — `the World Cup where it was given favorite` : calque de « donnée favorite ».
- **7.1.6** titre — `The inheritence` : « inheritance » mal orthographié.
- **7.3.4** titre — `Gooal!` : « Goal! » ; et le titre FR est « Buuut ! », autre interjection.

### I. Registre, modalité et formes (5)

- **7.1.2** sugg — « Alors ? » → `What's up?` : registre familier américain là où le FR est neutre.
- **6.2.2** ctxt — « un pot-de-vin » → `an occult payment` : `occult` = *occulte, surnaturel* ;
  le mot anglais est *bribe*.
- **3.1.6** pioch — « Une personne rencontrée en ligne » → `A person **I** met online` : bascule
  de personne grammaticale.
- **7.2.5** enjeu — « Il **tente** de convaincre » → `He **must** convince` : la modalité se
  durcit (ar · fa · es durciront encore, §5).
- **5.2.1** sugg — « mon nom est **l'Éternel** » → `my name is **eternal**` : le nom divin
  (capitale) devient un adjectif.

---

## 4. Conséquence structurante — l'EN n'est pas une référence neutre

Les six passes précédentes mesuraient la langue cible contre **deux** sources, en traitant l'EN
comme la seconde référence. Le tableau ci-dessous retourne l'instrument : à chaque cellule où
une passe a constaté « suit l'EN contre le FR », il donne **la cellule EN d'origine**.

| Carte | Champ | Cellule **EN** (origine) | Cellule **FR** | Suivent l'EN | Réparent |
|---|---|---|---|---|---|
| **3.1.1** | sugg | `We can both try, we'll see who gets picked.` | « Laissez-moi tenter ma chance… » | zh · ar · fa · es | **ru · pt** |
| **5.2.5** | enjeu | `try to prove her wrong` | « …qu'il ne l'a pas trompée » | zh · ar · fa · es | **ru · pt** |
| **5.3.2** | enjeu | `refuse to benefit from it` | « refuser le vaccin » | zh · ar · fa · es | **ru · pt** |
| **6.1.1** | sugg | `rebuild the people's confidence…` | « …comme en droit commun » | zh · ar · fa · es | **ru · pt** |
| **6.2.3** | sugg | `claims your head` | « réclame votre démission » | ar · es | fa · ru · pt |
| **3.2.2** | sugg | `clothes` | « affaires » | zh · ar · fa · es · pt | **ru** |
| **3.2.15** | titre | `The Spaghetti T-Shirt` | « Le t-shirt taché » | ar · fa · es · ru · pt | zh |
| **3.3.9** | sugg | ordre inversé (affirmation puis question) | question puis exclamation | ar · fa · es · ru · pt | zh |
| **4.1.11** | titre | `Mars, the Next Must-See Destination` | « Destination Mars » | ar · fa · es · ru · pt | zh |
| **3.2.8** | ctxt | `all-important` | (rien) | fa · ru · pt | zh · ar · es |
| **7.2.8** | sugg | `you're going to work a bit` (contresens) | « vous allez un peu vite en besogne » | zh · pt | **ar · fa · es · ru** |
| **6.3.2** | sugg | point final omis | point final présent | zh · pt | **ar · fa · es · ru** |

*(Les colonnes « suivent » / « réparent » recopient les mesures publiées par les passes
zh #1746, ar #1749, fa #1750, es #1751, ru #1752 et pt #1753 ; elles ne sont pas re-mesurées
ici. La colonne EN, elle, est mesurée par la présente passe.)*

**Ce que ce tableau établit.**

1. **L'hypothèse « passe sur le bloc EN » est confirmée à la source.** Les écarts des langues
   cibles ne sont pas inventés par elles : ils sont **hérités** d'un EN qui dévie lui-même. La
   passe en ne découvre donc pas douze défauts de plus dans les langues — elle découvre que
   **la seconde référence n'était pas une référence**.
2. **Le choix suivre/réparer est une décision par langue, pas un lot.** Sur les **cinq** mêmes
   cellules (3.1.1 · 5.2.5 · 5.3.2 · 6.1.1 · 6.2.3), zh · ar · fa · es suivent et ru · pt
   réparent — quatre contre deux, sur la même donnée d'entrée. Aucune « vague » unique ne
   décrit le corpus : il y a des **faisceaux par langue**.
3. **ru et pt ne réparent pas par principe** : ils réparent là où l'EN est inintelligible
   (contresens, ponctuation) et suivent l'EN ailleurs (3.2.2, 3.2.15, 3.3.9 pour pt). Un
   correcteur qui « suit le FR » est un correcteur qui **a lu l'EN** et l'a jugé.
4. **EN devait donc être lue en dernier** : c'est la seule passe qui change la nature du
   verdict sur les six autres. Les dossiers zh → pt mesuraient des langues ; celui-ci mesure
   **la référence qui les jugeait**.

---

## 5. Matrice inter-langues complète (7 langues + la colonne **en** comme origine)

Colonne **en** : `SOURCE` = l'écart existe d'abord dans l'EN ; `fidèle` = l'EN suit le FR, donc
l'écart est un ajout propre à la langue cible.

| Carte | Nature | zh | ar | fa | es | ru | pt | **en** |
|---|---|---|---|---|---|---|---|---|
| **2.2.9** ctxt | ajout « mythologique » | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | *fidèle* |
| **3.3.2** enjeu | ajout « engagement mutuel » | ✓ | ✓ | ✓ | ✓ | ✓ | — | *fidèle* |
| **3.2.16** enjeu | modalité affaiblie | ✓ | ✓ | ✓ | ✓ | ✓ | — | *fidèle* |
| **1.1.3** sugg | lions → félidé domestique | ✓ | ✓ | ✓ | ✓ | — | — | *fidèle* |
| **4.2.8** titre | « inattendu » ajouté, jeu perdu | ✓ | ✓ | ✓ | ✓ | — | — | *fidèle* |
| **1.2.3** ctxt+enjeu | ponctuation finale absente | ✓ | ✓ | ✓ | ✓ | — | — | *ponctue* |
| **7.3.5** sugg | punchline réécrite, « Venise » perdu | — | ✓ | ✓ | ✓ | ✓ | — | *garde Venise* |
| **4.3.4** ctxt | enjeu recopié dans le contexte | ✓ | — | ✓ | ✓ | ✓ | — | *fidèle* |
| **4.1.11** enjeu | superlatif idiomatique | — | ✓ | ✓ | ✓ | ✓ | — | *fidèle* |
| **4.1.11** titre | titre étendu | — | ✓ | ✓ | ✓ | ✓ | ✓ | **SOURCE** |
| **3.3.9** sugg | structure EN suivie | — | ✓ | ✓ | ✓ | ✓ | ✓ | **SOURCE** |
| **3.2.15** titre | « spaghetti » suivi | — | ✓ | ✓ | ✓ | ✓ | ✓ | **SOURCE** |
| **3.2.8** ctxt | ajout EN « all-important » | — | — | ✓ | — | ✓ | ✓ | **SOURCE** |
| **3.2.8** | anniversaire d'événement | ✓ (incoh.) | — | ✓ | ✓ | — | — | *fidèle* |
| **1.3.1** sugg | modificateur « sombres » | — | ✓ | ✓ | ✓ | — | — | *fidèle* |
| **3.3.5** enjeu | explicitation « et lui-même » | — | ✓ | ✓ | ✓ | — | — | *fidèle* |
| **4.2.2** ctxt | diminutif « un peu » | — | ✓ | ✓ | ✓ | — | ✓ | *fidèle* |
| **7.2.5** enjeu | modalité « pousse à » | — | ✓ | ✓ | ✓ | — | — | *durcit déjà* |
| **3.1.5** pioch | « conquête » adoucie | — | ✓ | ✓ | ✓ | ✓ | — | *fidèle* |
| **4.1.12** enjeu | modalité affaiblie | — | ✓ | ✓ | ✓ | ✓ | ✓ | *fidèle* |
| **6.2.1** titre | titre inventé | ✓ | — | — | — | ✓ | — | *fidèle* |
| **4.3.3** enjeu | rôle ajouté | ✓ | — | — | — | — | — | *fidèle* |
| **2.1.8** sugg | ponctuation finale absente | ✓ | ✓ | — | ✓ | — | — | *ponctue* |
| **3.1.1 · 5.2.5 · 5.3.2 · 6.1.1 · 6.2.3** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | — | — | **SOURCE** |
| **3.2.2** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | — | ✓ | **SOURCE** |
| **7.2.8** sugg | contresens | ✓ | — | — | — | — | ✓ | **SOURCE** |
| **6.3.2** sugg | point final omis | ✓ | — | — | — | — | ✓ | **SOURCE** |

**Familles propres à l'EN** (aucune autre langue lue ne les porte — elles n'apparaissaient donc
dans aucune matrice précédente) : **1.1.1** bara (nom propre perdu) · **2.2.5** bara (rôle →
nom, déjà le titre) · **2.3.5** pioch (personnage substitué + deux orthographes dans la même
carte) · **2.1.3** sugg (question → affirmation) · **4.2.1** sugg (dernière phrase coupée) ·
**7.3.2** pioch (rôle substitué, contredit son propre enjeu) · **7.1.5** titre/enjeu (mot FR non
traduit au titre, contenu de l'enjeu changé) · **7.1.6** titre (faute d'orthographe) ·
**7.3.4** titre (faute d'orthographe) · **6.2.2** ctxt (`occult payment`).

**Constat structurant** :

- **2.2.9 reste la seule carte à six langues** — et l'EN est *fidèle* : c'est la **seule famille
  de la campagne que l'EN n'explique pas**. Sept passes lues, aucune ne l'a démentie.
- **4.3.3 reste zh-seul** après cinq contrôles ; l'EN y est *fidèle* — la signature zh survit à
  la lecture de la source.
- **6.2.1 = zh · ru**, deux inventions distinctes ; l'EN est *fidèle*.
- **La ligne « suit l'EN » se scinde** : 4 langues suivent (zh · ar · fa · es), 2 réparent
  (ru · pt), et le choix n'est pas constant pour une même langue selon la cellule.
- **La colonne `en` est SOURCE sur 12 lignes et *fidèle* sur 15** : l'EN explique la majorité
  du passif, pas la totalité — et il porte sa propre famille de défauts, que personne n'avait
  mesurée parce que personne ne lisait l'EN.

---

## 6. Observations à trancher en relecture native (pas des défauts établis)

- **GB et US mêlés** : `neighbour` (7.2.5), `favour` (6.2.1), `labour` (4.2.3) contre
  `neighbor` ×28, `organiz` ×7, `favor` ×4, `skeptical` ×4, `labor` ×3, `honor`, `behavior`,
  `defense`. Le corpus est majoritairement US ; les 3 formes GB sont des **îlots**. Et
  **2.3.5** porte les deux dans la même carte (`specter` au ctxt, `spectre` à l'enjeu).
- **La casse du terme récurrent est un fait de corpus, pas un défaut EN** : le FR lui-même
  varie (`Le baratineur` 154 / `le baratineur` 20). L'EN **amplifie** (4 variantes au lieu de 2,
  dont 14 occurrences de `Smooth Talker` capitalisé que le FR n'a pas). À trancher comme
  convention éditoriale sur les deux langues, pas comme correction d'une seule.
- **Les 32 bara/pioch à initiale minuscule** ne sont pas non plus un défaut EN exclusif : le FR
  en porte **4** (2.2.5 pioch « le procureur », 4.1.1 bara « le vendeur », 5.1.5 bara
  « fantôme » et pioch « chasseur de fantôme »). L'EN en porte 8 fois plus ; le **fait** est
  partagé, l'**ampleur** ne l'est pas.
- **1.1.1** — le bara EN `Caesar's mother` perd le nom propre que porte le FR (« Aurelia
  Cotta »). Geste inverse de 1.3.2 (où l'EN **ajoute** un nom propre à la place d'un rôle) :
  les deux existent, dans le même bloc.
- **5.2.1** — l'EN écrit `Marsellus Wallace` (graphie du film), le FR « Marcellus ». Ici la
  graphie EN est la **canonique** : à consigner comme écart FR, pas EN.
- **Deux coquilles FR** repérées en relisant la source : **5.2.1** pioch « Le tueur à **G**ages »
  (capitale en milieu de syntagme) et **6.1.2** bara « Le porte parole » (trait d'union manquant
  — « porte-parole »). Le FR n'est pas la référence impeccable que le geste de correction
  suppose ; ce sont des faits à consigner, jamais à corriger dans cette campagne.
- **Le tiret cadratin** (U+2014) apparaît dans 5 cellules EN et **0** cellule FR : marqueur de
  rédaction EN, invisible au gabarit.
- **L'EN neutralise le genre** (`they`, `their`) de façon systématique et cohérente là où le FR
  accorde. C'est un **choix**, appliqué partout — pas une négligence.

---

## 7. Ce que la lecture n'établit pas

- **La date et l'ordre des passes EN** : le dossier constate des écarts dans l'état courant du
  CSV, il ne date pas les vagues qui les ont produits.
- **Le mécanisme de l'héritage** : « suit l'EN » est mesuré comme un fait de cellule ; rien
  n'établit ici si les langues cibles ont traduit **depuis** l'EN ou recopié une version
  intermédiaire.
- **Le nombre total d'écarts EN** : le §3 en établit 37 par lecture intégrale ; il ne prétend
  pas que 37 soit le total — aucune sonde automatique ne mesure « fidélité » sur ce corpus.
- **Le jugement natif** sur le registre, l'idiome et la fluidité de l'EN : ce dossier ouvre la
  matière, il ne tranche pas le style.
- **Que le FR soit la référence** : la passe a trouvé 4 initiales minuscules, 2 coquilles et une
  casse variable **dans le FR** (§6). Le FR décrit ce qu'il faut *rendre*, pas ce qui est
  *impeccable*.
- **Les 5 langues non lues à deux voies** (zh ar fa es ru pt) restent jugées sur leurs écarts à
  l'EN **et** au FR, sans que l'EN ait été noté au moment de leur passe : la matrice §5 corrige
  l'interprétation, elle ne refait pas ces lectures.

---

## 8. Fidélité élevée — à consigner aussi

- **1.3.4** — « Tous les hommes naissent libres et égaux en droits et en dignité » rendu
  `All human beings are born free and equal in dignity and rights` : **texte canonique** de
  l'Article 1 de la Déclaration universelle, dans sa version anglaise officielle.
- **2.2.7** — le FR dit « Ulysse », l'EN dit **`Odysseus`** : l'EN emploie le nom du canon
  anglais, comme il dit `Joan of Arc` (1.2.1), `Snow White` (2.1.4), `Bluebeard` (2.1.1),
  `Sleeping Beauty` (2.1.7), `The Pied Piper` (2.1.9), `Cato` (1.1.2), `Cain and Abel` (2.2.4).
- **5.1.2** — `Smurf`, `Papa Smurf`, `Hefty Smurf` : les **noms canoniques anglais** du
  Schtroumpf sont tous justes. La perte de cette carte est ailleurs (les créations lexicales,
  §3 C), pas dans la nomenclature.
- **1.2.4** — « La femme a le droit de monter à l'échafaud ; elle doit avoir également celui de
  monter à la tribune » (Olympe de Gouges) rendu `Woman has the right to mount the scaffold;
  she must also have the right to mount the rostrum` : la **double construction** est conservée.
- **5.2.1** — Ézéchiel en registre biblique (`the Almighty`, `the ungodly hordes`), et
  `Marsellus Wallace` à la graphie canonique.
- **3.1.3** — `Stealthing` : le titre emploie le **terme anglais** de l'acte décrit — le mot
  juste, là où « Retrait non consenti » est une périphrase.
- **7.2.12** — « trompe-l'œil » **naturalisé** en `optical-illusion` (là où pt garde
  « trompe-l'œil » tel quel).
- **7.1.1** — le FR dit `bac avec mention « Très bien »`, rendu `graduated from high school with
  highest honors` : équivalence de **système** (le baccalauréat n'existe pas, la mention n'a pas
  d'équivalent) et non calque.
- **4.3.5** — « Vous comprendrez… que dalle » rendu `You'll Understand... Absolutely Nothing` :
  l'idiome est perdu mais le **sens** est intact et la chute conservée.
- **5.3.4** — le FR écrit « Un touriste **S**ud-Coréen » (capitale fautive en milieu de
  syntagme) ; l'EN écrit `A South Korean tourist` — **l'EN corrige le FR** ici.
- **7.2.6** — `Foie gras vintage` : le millésime est conservé (comme pt « Safra », mieux que ru).
- **7.1.5** — le corps de la carte (`fair`) traduit bien « kermesse » : la faute est au titre,
  pas au ctxt (§3 G).

---

## 9. Verdict en

- **Couverture : complète** — 1002/1002 cellules, 0 vide, 167 cartes.
- **37 défauts EN établis** : 6 titres substitués, 5 contenus substitués, 2 amputations,
  3 contresens, 3 dilutions, 5 ajouts, 3 incohérences internes à la carte, 6 fautes de
  grammaire ou d'orthographe, 5 écarts de registre ou de modalité.
- **Le défaut-signal de la campagne est EN** : à **5.1.2**, l'EN est la **seule** langue des
  sept qui **ne recrée pas** le jeu de mots Schtroumpf — le superlatif du bara et l'adverbe du
  sugg tombent tous les deux. Les six langues cibles le recréent.
- **La matrice est complète** (7 langues + l'EN comme origine) : **2.2.9** seule carte à six
  langues et **seule famille que l'EN n'explique pas** · **4.3.3** zh-seul · **6.2.1** zh · ru ·
  la ligne « suit l'EN » se scinde 4 / 2.
- **Le renversement de la passe** : l'EN, traitée comme seconde source par les six passes
  précédentes, **dévie elle-même sur 12 lignes de la matrice** et porte une famille de défauts
  propre. *La seconde référence n'était pas une référence.*
- **0 écriture** : la relecture native décidera des corrections.

---

## 10. Errata — aucun déclenché ; une reformulation

**Aucune ligne des dossiers déjà publiés n'est périmée par cette passe.** Les sept pièces que le
renvoi demandait de confirmer sur l'EN le sont **toutes**, à la cellule près : 5.1.2 bara
`the smurf` (+ l'adverbe du sugg), 5.3.2 bara `An anti -vaccin`, 7.2.8 sugg `you're going to
work a bit`, 4.3.1 sugg `dizzy`, 3.2.8 ctxt `all-important`, 6.3.2 sugg (point omis),
6.2.3 sugg `claims your head`. Les rectifications de titres signalées par les passes
précédentes (3.1.2, 3.2.15, 4.1.11) sont confirmées **à la source**, et 3.3.9 (structure
inversée) également. **Aucun commentaire d'erratum n'est posté sur #1749, #1750, #1751, #1752
ou #1753.**

**Une reformulation, en revanche** — et elle porte sur les six dossiers à la fois, pas sur un
fait de cellule : les passes zh → pt présentaient l'EN comme la **seconde source** et
concluaient, quand une langue cible s'en écartait, à un écart **de la langue cible**. La passe
en établit que, sur **12 lignes de la matrice**, la cellule EN est elle-même déviante : ces
écarts sont **hérités**, non produits. Les dossiers antérieurs n'ont pas mesuré faux — ils ont
mesuré contre une référence qu'ils n'avaient pas notée. C'est le §4 du présent dossier qui
porte la lecture corrigée ; il ne s'agit pas d'un erratum à poster, mais de la raison pour
laquelle **l'EN devait être lue en dernier**.

*po-2024*
