# #1499 grain G1 — Dossier d'arbitrage Fallacies : 50 renommages text_fr + 1 collision PK 598/2

**Date** : 2026-09-23 (post-compaction 22:09 UTC) · **Lane** : po-2024 (worker) · **Base** : `origin/master` `7337b72a`
**Instrument** : [`docs/corpus/fallacies-renommages-diff.py`](../corpus/fallacies-renommages-diff.py) —
exécuté **tel que livré** avant commit (rc=0, témoins a–d PASS).
**Périmètre dispatché** : #1499 c.`5777638672` + c.`5777917978` — « G1 (Fallacies, 32 renommages +
collision PK 598/PK 2) + 2 nouveaux PK 603/680 ».

**⛔ Garde centrale respectée** : aucune écriture dans le CSV. Ce document est un **dossier
d'arbitrage** — la décision de valider un renommage revient à l'owner, carte par carte.

---

## Réponse en une ligne

Le périmètre dispatché (**32 renommages**) sous-estime l'ampleur réelle. L'instrument passe 1) **47
SUBST par jointure `path`** archive v3 → HEAD, plus 2) **3 SUBST par bridge** (cartes déplacées ET
renommées, invisibles aux jointures simples) = **50 renommages réels text_fr**, plus **3 corrections
typographiques** (accents/casse/ponctuation) qui ne sont pas des renommages éditoriaux. La **collision
PK 598 / PK 2** est confirmée (« Généralisation hâtive » a migré de PK 598 vers PK 2, qui en HEAD
est la racine de l'arbre `path=1.1`).

L'écart **50 vs 32** a une cause arithmétique : la spec mesurait contre une baseline non explicitée ;
les 18 supplémentaires sortent quand (a) on ne filtre pas sur `carte ≠ vide` (le ledger #1503 filtre
là et rate 11 cas) et (b) on instrumente les **déplacés + renommés** par bridge `archive --nom-->
baseline 2024 --PK--> HEAD` (3 cas : 598/603/680).

---

## 1. Ce que la mesure établit (instrument livré, rc=0)

Périmètre `path` archive v3 → HEAD (153 cartes imprimées) + bridge baseline 2024 (3 cas invisibles).

| Catégorie | Compte | Statut |
|---|---:|---|
| **Renommages SUBST (réels)** | **50** | objet d'arbitrage |
| Corrections typographiques (C) | 3 | **à garder**, pas d'arbitrage |
| **Total `text_fr` changés** | **53** | |

Les 50 se répartissent en :

| Méthode de détection | Compte |
|---|---:|
| Jointure `path` archive v3 → HEAD | **47** |
| Bridge `archive --nom--> baseline 2024 --PK--> HEAD` | **3** (598, 603, 680) |
| **Total** | **50** |

### Témoins

| # | Contrôle | Mesure | Verdict |
|---|---|---|---|
| a | Archive v3 lisible = 169 cartes | 169 | PASS |
| b | Jointure `path` archive v3 ∩ HEAD | 163/169 (96 %) | PASS (≥ 153) |
| c | PK 598/603/680 visibles en SUBST (bridge requis) | 3/3 | PASS |
| d | Collision PK 598 ≠ PK 2 HEAD | « Induction hâtive » ≠ « Généralisation hâtive » | PASS |

---

## 2. Pourquoi 50 et pas 32 (réconciliation)

| Source | Renommages `text_fr` SUBST | Méthode |
|---|---:|---|
| Spec dispatch (c.`5777638672`) | 32 | inconnue — base d'origine non documentée |
| Ledger #1503 (`ai-01`, `carte ≠ vide`) | 36-37 | jointure `path` + filtre cartes non vides |
| **G1 instrument** | **50** | jointure `path` + bridge, sans filtre `carte` |

L'écart 18 se décompose en :

- **+10** : le ledger filtre `carte ≠ vide` ; G1 instrumente **toutes** les cartes de l'archive v3
  même celles avec `carte = ''`. Quand une carte perd son `carte` entre l'archive et HEAD mais
  conserve son `text_fr` modifié, le ledger la traite comme « non appariée » et rate le renommage.
- **+3** : PK 598, 603, 680 sont déplacés ET renommés — `path` change entre archive v3 et HEAD, donc
  invisibles aux jointures par `path`. Le bridge `archive --nom--> baseline 2024 --PK--> HEAD`
  (introduit par PR #1507) les fait sortir.
- **+5** : vrais renommages que la spec dispatch n'avait pas capturés (cf. tableau §3 ci-dessous ;
  aucun match exact avec le 32 — la base de mesure est différente, pas un sous-ensemble).

⇒ **L'arbitrage porte sur 50, pas sur 32.** Le 32 du dispatch a expiré dès que la spec a été rédigée.

---

## 3. Tableau d'arbitrage — 50 renommages `text_fr` (archive v3 → HEAD)

Format : `PK | path | ancien (v3) | nouveau (HEAD) | famille`. Lignes triées par PK.

### 3.1 — Jointure `path` directe (47 cas)

| PK | path | ancien (archive v3) | nouveau (HEAD) |
|---:|---|---|---|
| 2 | 1.1 | Argument bâclé | Généralisation hâtive |
| 3 | 1.1.1 | Justification triviale | Argument vide |
| 4 | 1.1.1.1 | Preuve anecdotique | Appel à l'ignorance |
| 20 | 1.1.1.2 | Pratique courante | Appel à l'urgence pragmatique |
| 22 | 1.1.1.3 | Sophisme du psychologue | Acte de foi |
| 33 | 1.1.2 | Sauvetage ad hoc | Justification triviale |
| 55 | 1.1.3 | Argument vide | Sauvetage ad hoc |
| 96 | 1.2.2 | Sophisme naturaliste | Appel à la nature |
| 98 | 1.2.2.2 | Raison de la majorité | Appel à la majorité |
| 134 | 1.3.1 | Sophisme ludique | Sophisme du jeu |
| 153 | 1.3.2 | Mauvaises raisons | Argument des mauvaises raisons |
| 154 | 1.3.2.1 | Argument du Sophisme | Sophisme du sophisme |
| 176 | 2.1 | Procédé rhétorique | Technique rhétorique |
| 185 | 2.1.1.4 | Poncif anti critique | Poncif anticritique |
| 332 | 2.2.2.3 | Appel à la terreur | Fâcher l'adversaire |
| 361 | 2.3.1.1.1.2 | Leurre | Appât et substitution |
| 511 | 2.3.3 | Influence non verbale | Communication non verbale |
| 600 | 3.1.1.1.1.2 | Généralisation hâtive | Insensibilité à la taille de l'échantillon |
| 602 | 3.1.1.2 | Cueillette de cerises | Biais de confirmation |
| 632 | 3.2 | Mauvaise interprétation | Interprétation quantitative erronée |
| 658 | 3.2.3 | Justification infinie | Infini fallacieux |
| 666 | 3.3 | Résultat invalide | Conclusion mathématique invalide |
| 679 | 3.3.1.3.3 | Hypothèse non plausible | Appel à l'abus censeur |
| 713 | 4.1.2.5 | Sophisme de la double faute | Deux torts font un droit |
| 726 | 4.2 | Mauvaise composition | Composition fautive |
| 730 | 4.2.1.2 | Négation de l'antécédent | Commutation des conditionnelles |
| 740 | 4.2.2.2 | Inférence immédiate erronnée | Inférence immédiate erronée |
| 758 | 4.3 | Mauvaise déduction | Déduction invalide |
| 799 | 5.1 | Définition imprécise | Définition biaisée |
| 808 | 5.1.2.2 | Sophisme de corrélation | Sophisme des corrélatifs |
| 826 | 5.1.3 | Définition inconsistante | Définition incohérente |
| 837 | 5.2.1.3 | Comparaison inconsistante | Comparaison incohérente |
| 847 | 5.3.1 | Ambiguïté syntaxique | Amphibologie |
| 855 | 5.3.2 | Ambiguïté sémantique | Équivoque |
| 888 | 6.1 | Arranger les faits | Présentation trompeuse des faits |
| 973 | 6.2 | Changement de cap | Déplacement des critères |
| 992 | 6.2.2 | Beurre et argent du beurre | Vouloir le beurre et l'argent du beurre |
| 1023 | 6.3 | Pensée biaisée | Raisonnement biaisé |
| 1024 | 6.3.1 | Anthropocentrisme | Biais naturels |
| 1174 | 6.3.2 | Ethnocentrisme | Biais culturels |
| 1242 | 6.3.3 | Dogmatisme | Biais théoriques |
| 1287 | 7.1.2 | Sophisme d'Explication | Pseudo-explication |
| 1312 | 7.2 | Saboter le débat | Sabotage du débat |
| 1313 | 7.2.1 | Fausse piste | Évasion |
| 1352 | 7.2.3 | Empoisonner le puits | Empoisonnement du puits |
| 1361 | 7.3.1 | Procès en inconstance | Procès en incohérence |
| 1372 | 7.3.2.1 | Sophisme par association | Culpabilité par association |

### 3.2 — Bridge (déplacé + renommé, 3 cas invisibles)

| PK | path (archive → HEAD) | ancien (archive v3) | nouveau (HEAD) | famille HEAD |
|---:|---|---|---|---|
| 598 | 3.1.1.1.1.2 → **3.1.1.1.1** | Généralisation hâtive | Induction hâtive | Mathematical error / Généralisation abusive / Échantillon biaisé |
| 603 | 3.1.1.2 → **3.1.1.2.1** | Cueillette de cerises | Picorage de données | Mathematical error / Généralisation abusive / Échantillon biaisé |
| 680 | 3.3.1.3.3 → **3.3.1.3.4** | Hypothèse non plausible | Hypothèse peu plausible | Mathematical error / Résultat invalide / Imprécision |

### 3.3 — Corrections typographiques (3 cas, à GARDER sans arbitrage)

| PK | path | ancien | nouveau |
|---:|---|---|---|
| 813 | 5.1.2.2.3 | Sophisme du vrai écossais | Sophisme du vrai Écossais |
| 1362 | 7.3.1.1 | Tu Quoque | Tu quoque |
| 1388 | 7.3.2.3.2 | Attaque de la Confiance en soi | Attaque de la confiance en soi |

(`C` = ne diffère que par accents/casse/ponctuation, conforme à `classify()` dans `ledger-instrument.py`.)

---

## 4. La collision PK 598 / PK 2 — le défaut que G1 sert à détecter

La collision est **confirmée**. En HEAD courant :

| PK | path HEAD | `text_fr` HEAD |
|---:|---|---|
| **598** | 3.1.1.1.1 | **Induction hâtive** |
| **2** | 1.1 | **Généralisation hâtive** |

Le **titre de l'archive v3** « Généralisation hâtive » a quitté PK 598 (qui désormais s'appelle
« Induction hâtive » et est passé en `path 3.1.1.1.1.2 → 3.1.1.1.1`) pour atterrir sur PK 2 (la
racine `path=1.1`).

Trois lectures possibles :

1. **L'agent a vu juste mais a re-clés PK 2** : « Induction hâtive » (PK 598) et « Généralisation
   hâtive » (PK 2) sont deux sophismes distincts mais l'ancien nom de PK 598 a été re-attribué à
   PK 2 par erreur de migration. ⇒ **Arbitrage owner requis** : le nom PK 2 « Généralisation
   hâtive » est-il justifié, ou faut-il restaurer un autre nom ?
2. **PK 2 est devenu la racine thématique « Argument bâclé → Généralisation hâtive »** : c'est ce
   que l'agent semble avoir fait (PK 2 a perdu son titre d'origine « Argument bâclé » pour adopter
   l'ancien nom de PK 598). ⇒ L'arbitrage porte sur le fait de **fusionner** PK 2 et PK 598 sous
   un même nom.
3. **PK 598 a été créé nouveau** : PK 598 « Induction hâtive » est un nouveau sophisme, distinct
   de l'ancien PK 598 « Généralisation hâtive » qui a migré vers PK 2. ⇒ Le tri est **sémantique**
   (l'agent distingue « induction hâtive » et « généralisation hâtive »), pas un raté.

⛔ Les trois lectures ont des conséquences éditoriales opposées. **L'arbitrage owner est requis.**

---

## 5. Ce que ce dossier **n'établit pas**

- ⛔ **Il ne dit pas qu'un `SUBST` est mauvais.** `SUBST` est une mesure de **forme** : le titre a
  été remplacé. Beaucoup sont probablement des clarifications utiles. Le tri est éditorial.
- ⛔ **Il ne couvre que le `text_fr`** sur l'arc Fallacies (les 3 champs `desc_fr` et `example_fr`
  sont mesurés par le ledger #1503 — 135 + 111 cellules — sur le même arc).
- ⛔ **Il ne couvre pas Scenarii / Virtues / Rules.** Même méthode applicable aux Scenarii **avec
  une autre clé** (le titre normalisé, cf. ledger #1503 §8), Virtues / Rules sont couverts par G4.
- ⚠️ **Pour 90 des 153 cartes, la référence est l'archive v3, pas l'imprimé.** Une couche de dérive
  reste invisible pour ces 90.
- ⚠️ **L'arc d'origine reste à dater.** L'instrument compare archive v3 et HEAD mais ne situe pas
  temporellement les renommages ; le ledger #1503 §4 attribue 220 cellules desc_fr/example_fr à
  `9d45b4f9` (PR #369, 28/05/2026, agentique) — **probablement la même vague pour text_fr** mais à
  confirmer par `git log -S` cellule par cellule si l'owner veut la chronologie.

---

## 6. Ce qui est attendu

1. **Arbitrer PK 2 vs PK 598** (collision) en premier — c'est le défaut de gouvernance, pas un
   renommage neutre.
2. **Arbitrer la tête du tableau 3.1**, pas les 50 lignes. Les **47 renommages** sont tous des
   SUBST et l'amplitude varie ; un seuil suffit.
3. **Trancher la règle par défaut.** Le mandat owner du 22/09 pose *le retour à l'imprimé* comme
   défaut, la charge de la preuve pesant sur le changement. ⛔ Appliqué littéralement à 50 titres de
   cartes déjà imprimées ⇒ cascade des 8 langues (régénération ≈ 3 h 25, barème #1469) ⇒ c'est
   l'owner qui choisit l'amplitude de l'arbitrage.

⚠️ **Contrainte politique, ⛔ pas décorative** — verbatim owner : *« Thomas est assez susceptible
et dubitatif sur notre usage de l'IA […] on a déjà joué des centaines de fois avec ces cartes […]
trop de changement gratuit, et erroné de surcroît, aucune chance que ça passe à la relecture. »*
⇒ **Le volume non trié est lui-même le risque** : 50 renommages défendables passent ; 50 non triés
font rejeter l'ensemble, y compris les bons.

---

*Mesuré par `myia-po-2024` (worker lane) le 2026-09-23. Instrument et contrôles inverses
reproductibles — la mesure se refait en une passe sur `path` + bridge.*
