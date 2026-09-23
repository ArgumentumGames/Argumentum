# #1499 grain G1 v3 — Dossier d'arbitrage Fallacies (PR #1515, 3ᵉ tête)

**Date** : 2026-09-23 (post-review 04:58Z) · **Lane** : po-2024 (worker) · **Base** : `origin/master` `c3d07132`
**Instrument** : [`docs/corpus/fallacies-renommages-diff.py`](../corpus/fallacies-renommages-diff.py)
— exécuté **tel que livré** avant commit (rc=0, témoins a–d PASS).
**Reviews répondue** : [v1 OxMH6A](https://github.com/ArgumentumGames/Argumentum/pull/1515#pullrequestreview-OxMH6A) (02:03Z) puis [v2 OyIThw](https://github.com/ArgumentumGames/Argumentum/pull/1515#pullrequestreview-OyIThw) (04:58Z).
**Ce que la v3 change** : la mesure v2 est **recoupée juste** par ai-01 (32 agentiques retrouvés indépendamment, sans l'archive). La v3 corrige la **prose** : colonne baseline dans tout le tableau d'arbitrage, surface ramenée de 34 à **32**, §4 « non-geste » rétracté (la baseline 2024 n'est pas l'imprimé), réconciliations **closed** par la review, §10.2 supprimé (PK 332 n'est pas une carte).

**⛔ Garde centrale respectée** : aucune écriture dans le CSV. Ce document est un **dossier d'arbitrage** — la décision de valider un renommage ou la collision PK 598/2 appartient à l'owner, carte par carte.

---

## Réponse en une ligne

G1 v3 mesure **32 renommages agentiques** (titre `text_fr` de carte imprimée, par PK contre la baseline canonique `62b561e75`) — le **même ensemble** que le 32 du dispatch, recoupé indépendamment par ai-01 (31 SUBST strictes + PK 185 soudure). Autour : 5 pré-agentiques (geste d'époque owner 2022→2024), 4 DÉPLACÉ (permutations entre sœurs), 4 typographiques à garder, 1 hors-deck (PK 96, n'est plus une carte). **Surface d'arbitrage = 32.**

---

## 1. Diff vs têtes précédentes

| Aspect | v1 (#1515) | v2 (#1515) | **v3 (cette tête)** |
|---|---|---|---|
| Référence | Archive v3 | Baseline `62b561e75` par PK | idem v2 |
| Jointure | `path` seule | 2 passes (path + PK baseline) | idem v2 |
| Colonne baseline | — | — | **3 colonnes : imprimé / baseline / HEAD** |
| Surface annoncée | 50 | 34 | **32** (PK 855 → typo, PK 96 → hors arbitrage) |
| §4 pré-agentiques | — | « non-geste » (**faux**) | **retour à l'imprimé = revenir sur un geste owner** |
| Réconciliations 30/16 | ouvertes | « à creuser » | **closed** par la review v2 |
| §10.2 (PK 332) | — | spéculation | **supprimé** (n'est pas une carte) |

---

## 2. Ce que la mesure établit (instrument livré, rc=0, 4 témoins PASS)

| Catégorie | Compte | Surface d'arbitrage ? |
|---|---:|---|
| **Agentique** (baseline ≠ HEAD, SUBST) | **32** | **OUI — c'est la surface** |
| &nbsp;&nbsp;dont soudure (PK 185, classable typo) | 1 | oui, choix éditorial noté |
| Typo-seul-agent (PK 855, `Equivoque` → `Équivoque`) | 1 | non — à garder |
| Hors-deck (PK 96, retirée #1288 — n'est plus une carte) | 1 | non — rien à imprimer, rien à arbitrer |
| Pré-agentique (HEAD = baseline, archive différente) | 5 | non — geste d'époque owner |
| Corrections typographiques pures (C) | 3 | non — à garder |
| DÉPLACÉ (titre archive vit sur une autre carte HEAD) | 4 | non — permutation, pas renommage |
| **TOTAL lignes mesurées** | **46** | 32 |

⚠️ Le TOTAL 46 compte toutes les lignes (39 SUBST + 3 C + 4 DÉPLACÉ). La v2 affichait « 43 » en excluant silencieusement les 3 C — une ligne TOTAL doit dire ce qu'elle exclut.

### Témoins

| # | Contrôle | Mesure | Verdict |
|---|---|---|---|
| a | Archive v3 lisible = 169 cartes | 169 | PASS |
| b | Baseline imprimées ∩ HEAD par PK = 175 (= 176 − PK 96 retiré #1288) | 175 | PASS |
| c | PK 598/603/680 capturés par la passe 2 | 3/3 | PASS |
| d | Collision PK 598/2 traitée comme nom rétro-attribué | oui | PASS |

---

## 3. Tableau d'arbitrage — surface agentique (32 cas)

⭐ **3 colonnes** (règle Epic #1499 c.5787733627) : **imprimé** (archive v3) / **baseline 2024** / **HEAD**. Sur 4 PK, « retour à l'imprimé » et « annulation de l'agentique » donnent **deux titres différents** (361, 658, 799, 1361) ; sur 2 PK le retour à l'imprimé **n'existe pas** (492, 1357, sans référence archive — #1507).

| PK | path | impr. | imprimé (archive v3) | baseline 2024 | HEAD |
|---:|---|---|---|---|---|
| 2 | 1.1 | v3 | Argument bâclé | Argument bâclé | Généralisation hâtive |
| 98 | 1.2.2.2 | v3 | Raison de la majorité | Raison de la majorité | Appel à la majorité |
| 134 | 1.3.1 | v3 | Sophisme ludique | Sophisme ludique | Sophisme du jeu |
| 154 | 1.3.2.1 | v3 | Argument du Sophisme | Argument du Sophisme | Sophisme du sophisme |
| 176 | 2.1 | v3 | Procédé rhétorique | Procédé rhétorique | Technique rhétorique |
| 185 | 2.1.1.4 | v3 | Poncif anti critique | Poncif anti critique | Poncif anticritique ⚠️ soudure |
| 361 | 2.3.1.1.1.2 | v3 | Leurre | **Appât et bascule** | Appât et substitution |
| 492 | 2.3.2.3.4 | — | *aucune réf. archive* (#1507) | Jouer la victime | Auto-victimisation |
| 511 | 2.3.3 | v3 | Influence non verbale | Influence non verbale | Communication non verbale |
| 598 | 3.1.1.1.1 | v3* | *carte v3 sous un autre path* | Généralisation hâtive | Induction hâtive |
| 603 | 3.1.1.2.1 | v3* | *carte v3 sous un autre path* | Cueillette de cerises | Picorage de données |
| 632 | 3.2 | v3 | Mauvaise interprétation | Mauvaise interprétation | Interprétation quantitative erronée |
| 658 | 3.2.3 | v3 | Justification infinie | **Infini trompeur** | Infini fallacieux |
| 666 | 3.3 | v3 | Résultat invalide | Résultat invalide | Conclusion mathématique invalide |
| 680 | 3.3.1.3.4 | v3* | *carte v3 sous un autre path* | Hypothèse non plausible | Hypothèse peu plausible |
| 713 | 4.1.2.5 | v3 | Sophisme de la double faute | Sophisme de la double faute | Deux torts font un droit |
| 726 | 4.2 | v3 | Mauvaise composition | Mauvaise composition | Composition fautive |
| 740 | 4.2.2.2 | v3 | Inférence immédiate erronnée | Inférence immédiate erronnée | Inférence immédiate erronée |
| 758 | 4.3 | v3 | Mauvaise déduction | Mauvaise déduction | Déduction invalide |
| 799 | 5.1 | v3 | Définition imprécise | **Définition inexacte** | Définition biaisée |
| 808 | 5.1.2.2 | v3 | Sophisme de corrélation | Sophisme de corrélation | Sophisme des corrélatifs |
| 826 | 5.1.3 | v3 | Définition inconsistante | Définition inconsistante | Définition incohérente |
| 837 | 5.2.1.3 | v3 | Comparaison inconsistante | Comparaison inconsistante | Comparaison incohérente |
| 888 | 6.1 | v3 | Arranger les faits | Arranger les faits | Présentation trompeuse des faits |
| 973 | 6.2 | v3 | Changement de cap | Changement de cap | Déplacement des critères |
| 992 | 6.2.2 | v3 | Beurre et argent du beurre | Beurre et argent du beurre | Vouloir le beurre et l'argent du beurre |
| 1023 | 6.3 | v3 | Pensée biaisée | Pensée biaisée | Raisonnement biaisé |
| 1287 | 7.1.2 | v3 | Sophisme d'Explication | Sophisme d'Explication | Pseudo-explication |
| 1312 | 7.2 | v3 | Saboter le débat | Saboter le débat | Sabotage du débat |
| 1352 | 7.2.3 | v3 | Empoisonner le puits | Empoisonner le puits | Empoisonnement du puits |
| 1357 | 7.2.3.4 | — | *aucune réf. archive* (#1507) | Faux arguments | Arguments factices |
| 1361 | 7.3.1 | v3 | Procès en inconstance | **Procès en inconsistance** | Procès en incohérence |

Lecture :
- **26 lignes** : baseline = imprimé ⇒ « annuler l'agentique » et « retour à l'imprimé » sont **le même geste**.
- **4 lignes en gras** (361, 658, 799, 1361) : les deux issues **diffèrent** — l'owner tranche trois colonnes, pas deux.
- **2 lignes** (492, 1357) : pas de référence imprimée ⇒ seule l'annulation de l'agentique existe.
- **PK 185** : soudure (« anti critique » → « anticritique ») — le squelette sans espaces est identique ; classable typographique. L'instrument la garde en SUBST (l'espace compte) et la marque ; le tri final est éditorial.
- `v3*` : la carte existait dans l'archive v3 sous un autre `path` que la jointure par chemin ne rejoint pas — la correspondance de titre imprimé n'est pas établie par cet instrument.

### 3.2 — Hors arbitrage, mêmes références

| PK | cas | Pourquoi hors arbitrage |
|---:|---|---|
| 96 | Sophisme naturaliste → Appel à la nature | **N'est plus une carte** (retirée #1288) — figure toujours en baseline ; rien à imprimer, rien à arbitrer |
| 855 | Ambiguïté sémantique → Équivoque (baseline : `Equivoque`) | Différence agentique **purement typographique** — même classe que les C, à garder |

---

## 4. Pré-agentique (5 cas) — geste d'époque owner, à présenter à part seulement sur demande

⚠️ **Rétractation v2** : la v2 disait « le titre HEAD est déjà l'imprimé 2024, le retour y est un non-geste ». **C'est faux.** La baseline `62b561e75` est l'état CSV du 22/04/2024, **pas** l'édition imprimée. L'imprimé, c'est l'archive v3 — et pour ces 5 cartes l'archive **diffère** de HEAD :

| PK | imprimé (archive v3) | baseline 2024 = HEAD |
|---:|---|---|
| 153 | Mauvaises raisons | Argument des mauvaises raisons |
| 847 | Ambiguïté syntaxique | Amphibologie |
| 1024 | Anthropocentrisme | Biais naturels |
| 1174 | Ethnocentrisme | Biais culturels |
| 1242 | Dogmatisme | Biais théoriques |

Un « retour à l'imprimé » appliqué ici **reviendrait sur un geste owner d'époque (2022→2024)** — ce n'est pas un non-geste. Hors surface agentique ; à présenter à part **seulement** si l'owner le demande.

---

## 5. DÉPLACÉ (4 cas) — permutation entre sœurs, **PAS** un renommage

Valeurs mesurées (baseline = HEAD pour les 3 premières : la permutation était **déjà en place en 2024**) :

| PK archive | titre imprimé (v3) | baseline 2024 | HEAD | le titre vit aussi sur |
|---:|---|---|---|---|
| 3 | Justification triviale | Argument vide | Argument vide | **PK 33** |
| 33 | Sauvetage ad hoc | Justification triviale | Justification triviale | **PK 55** |
| 55 | Argument vide | Sauvetage ad hoc | Sauvetage ad hoc | **PK 3** |
| 1313 | Fausse piste | Evasion | Évasion | **PK 1314** |

⚠️ Le titre imprimé de PK 3/33/55 **n'a pas été perdu** : il vit sur une sœur HEAD. Appliquer « retour à l'imprimé » à ces 3 lignes créerait **deux cartes « Justification triviale »** — le défaut détecté par la review v1. PK 1313 : la couche typo-seul-agent (`Evasion` → `Évasion`) et la classe DÉPLACÉ sont **vraies simultanément** ; **DÉPLACÉ prime** dans la ventilation, sans dédoubler la ligne — elle est hors surface dans les deux lectures.

---

## 6. Corrections typographiques (4 cas, à GARDER sans arbitrage)

| PK | référence | HEAD | couche |
|---:|---|---|---|
| 813 | Sophisme du vrai écossais | Sophisme du vrai Écossais | typo-seul-agent |
| 855 | Ambiguïté sémantique (imprimé) / `Equivoque` (baseline) | Équivoque | typo-seul-agent |
| 1362 | Tu Quoque | Tu quoque | **pré-agentique** (baseline = HEAD = « Tu quoque » — le geste typo date d'avant 2024) |
| 1388 | Attaque de la Confiance en soi | Attaque de la confiance en soi | typo-seul-agent |

(`C` = ne diffère que par accents/casse/ponctuation ; PK 855 compte ici depuis la v3 — review : « même classe que les 3 C à garder ».)

---

## 7. Collision PK 598 / PK 2 — tranchée par la baseline

| Référence | PK 2 | PK 598 |
|---|---|---|
| **Baseline `62b561e75`** | Argument bâclé | Généralisation hâtive |
| **HEAD** | Généralisation hâtive | Induction hâtive |
| `path` HEAD | 1.1 | 3.1.1.1.1 |
| Carte imprimée ? | oui | oui |

⭐ **Aucune carte n'a bougé.** Le titre baseline « Généralisation hâtive » était sur PK 598 ; il a été **ré-attribué** sur PK 2 en HEAD. La question owner n'est pas « choisir entre PK 2 et PK 598 » mais **« le nom *Généralisation hâtive* doit-il rester sur la carte *Argument bâclé* ? »**. Si non : (a) restaurer « Argument bâclé » sur PK 2, ou (b) renommer autrement.

---

## 8. Réconciliations avec la review — CLOSED (⛔ pas des questions owner)

La review v2 d'ai-01 a clos elle-même les deux écarts que la v2 laissait ouverts :

- **30 (review v1) contre 32** : 30 = 32 − {492, 1357}, les deux PK **sans ancêtre dans l'archive**. La passe 2 **reste** : la règle de l'Epic mesure contre la baseline. Exclure la passe 2 donnerait **27, pas 30** — 598/603/680 **sont** des cartes v3, sous d'autres `path` (#1507).
- **16 (review v1) contre 5** : rien n'était manqué. 16 = **5** pré-agentiques + **3** DÉPLACÉ (PK 3/33/55) + **8 nœuds non-cartes** (PK 4, 20, 22, 600, 602, 679, 730, 1372), sortis par le filtre `carte ≠ vide` que la review v1 avait demandé.

Recoupement indépendant d'ai-01 (sans l'archive, squelette NFKD sans diacritiques sur les 175 cartes HEAD) : **31 + PK 185 = exactement les 32** de cette mesure — le même ensemble que le 32 du dispatch.

---

## 9. Ce que ce dossier **n'établit pas**

- ⛔ **Il ne dit pas qu'un `agentique` est mauvais.** C'est une mesure de **forme** : le titre a été remplacé entre baseline 2024 et HEAD. Le tri est éditorial.
- ⛔ **Il ne couvre que le `text_fr`** sur l'arc Fallacies. `desc_fr` et `example_fr` sont mesurés par le ledger #1503 (erratum #1516 : 287/277/32).
- ⛔ **Il ne couvre pas Scenarii / Virtues / Rules.** Scenarii = autre clé, Virtues = couvert par G4 (#1513), Règles = G7 (#1517).
- ⚠️ **L'arc d'origine reste à dater** : le ledger #1503 §4 attribue 220 cellules desc/example à `9d45b4f9` (PR #369, 28/05/2026, agentique) — probablement la même vague pour `text_fr`, à confirmer par `git log -S` si l'owner veut la chronologie.

---

## 10. Limites connues de G1 v3

1. **Passe 2** ajoute 5 PK (492, 598, 603, 680, 1357) présentes en baseline imprimées et HEAD imprimées mais invisibles de la jointure `path`. 492 et 1357 n'ont **aucune** référence archive (#1507 : les 7 sans référence sont 105, 362, 492, 1020, 1092, 1120, 1357) ; 598/603/680 ont une carte v3 **sous un autre path**, que cet instrument ne rejoint pas — leurs colonnes imprimé sont donc qualitatives (`v3*`), pas des titres.
2. ~~PK 332 « Facher/Fâcher »~~ — **supprimé** (v2 faux) : PK 332 n'est **pas** une carte (`carte` vide en baseline et en HEAD), l'instrument ne le rend nulle part ; la carte « Appel à la terreur » est **PK 337**, inchangée.
3. **PK 153** : tranché par la mesure — baseline = « Argument des mauvaises raisons » = HEAD ⇒ pré-agentique (la v2 spéculait sur une cascade ; la mesure répond).
4. **PK 1313** : DEPLACE **prime** sur typo-seul-agent dans la ventilation, sans dédoubler la ligne (hors surface dans les deux lectures).

---

## 11. Ce qui est attendu (question owner Q-9)

1. **Arbitrer PK 2 vs PK 598** (nom rétro-attribué « Généralisation hâtive ») en priorité — c'est le défaut de gouvernance.
2. **Arbitrer la surface des 32 agentiques** — en **trois colonnes** (imprimé / baseline / HEAD) : sur 4 PK les deux issues diffèrent (361, 658, 799, 1361), sur 2 le retour à l'imprimé n'existe pas (492, 1357).
3. **Trancher la règle par défaut.** Le mandat owner du 22/09 pose *le retour à l'imprimé* comme défaut — appliqué aux **32** titres, la cascade des 8 langues coûte ≈ 3 h 25 (barème #1469). Les 5 pré-agentiques ne sont **pas** concernées (y revenir dessus serait rouvrir un geste d'époque owner, à ne faire que sur demande explicite).
4. ⛔ Réconciliations : **closed** par la review (§8) — plus rien à creuser.

⚠️ **Contrainte politique, ⛔ pas décorative** — verbatim owner : *« Thomas est assez susceptible et dubitatif sur notre usage de l'IA […] on a déjà joué des centaines de fois avec ces cartes […] trop de changement gratuit, et erroné de surcroît, aucune chance que ça passe à la relecture. »* ⇒ **Le volume non trié est lui-même le risque** : 32 renommages défendables passent ; 32 non triés font rejeter l'ensemble, y compris les bons.

---

*Mesuré par `myia-po-2024` (worker lane) le 2026-09-23. Reviews répondue : v1 OxMH6A + v2 OyIThw. Instrument reproductible (rc=0, 4 témoins PASS) — la mesure se refait en une passe sur `path` + une passe sur PK baseline.*
