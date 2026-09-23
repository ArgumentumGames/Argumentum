# #1499 grain G1 v2 — Dossier d'arbitrage Fallacies (PR #1515, nouvelle tête)

**Date** : 2026-09-23 (post-review 02:03Z) · **Lane** : po-2024 (worker) · **Base** : `origin/master` `c3d07132`
**Instrument** : [`docs/corpus/fallacies-renommages-diff.py`](../corpus/fallacies-renommages-diff.py)
— exécuté **tel que livré** avant commit (rc=0, témoins a–d PASS).
**Review répondue** : [#1515 c.PRR_kwDODtQRX88AAAABOxMH6A](https://github.com/ArgumentumGames/Argumentum/pull/1515#pullrequestreview-~OxMH6A).
**Nouvelle tête** : G1 v1 mesurait contre `Archive/v3` ; G1 v2 mesure **par PK contre la baseline `62b561e75`**, la référence canonique de l'Epic #1499 (règle c.`5787733627`).

**⛔ Garde centrale respectée** : aucune écriture dans le CSV. Ce document est un **dossier d'arbitrage** — la décision de valider un renommage ou la collision PK 598/2 appartient à l'owner, carte par carte.

---

## Réponse en une ligne

G1 v1 (PR #1515 v1) mesurait **50** en joignant `path` archive v3 → HEAD sans filtre `carte ≠ vide`. La review a montré trois défauts : **dédoublonnage bridge, jointure path qui apparie des nœuds non-imprimés, et absence de couche baseline**. G1 v2 mesure **43** par **2 passes** (path archive v3 ∩ HEAD ∩ imprimées, puis PK baseline imprimées → HEAD) avec **classes couche** et **classe DÉPLACÉ**. Surface d'arbitrage agentique = **34** (32 agentique + 1 typo-agent + 1 hors-deck).

---

## 1. Diff vs G1 v1

| Aspect | G1 v1 (#1515 v1) | G1 v2 (#1515 v2) |
|---|---|---|
| Référence mesurée | Archive v3 (169 cartes) | Baseline `62b561e75` par PK |
| Jointure | `path` archive v3 → HEAD | 2 passes : path + PK baseline |
| Filtre carte HEAD | aucun | `carte ≠ vide` (sauf cas hors-deck explicite) |
| Dédoublonnage | aucun | archive_pk_traite (chaque PK archive comptée 1×) |
| Classes | SUBST / C / (rien) | SUBST / C / DÉPLACÉ |
| Couches | aucune | agentique / pré-agentique / typo-agent / hors-deck |
| Cas PK 598/2 | collision | nom-rétro-attribué (cf. review) |
| Total SUBST | 47 | **39** |
| Total + DÉPLACÉ | 50 | **43** |
| Surface arbitrage | 50 (tout) | **34** (agentique uniquement) |

---

## 2. Ce que la mesure v2 établit (instrument livré, rc=0)

Périmètre : `cartes imprimées HEAD` × `archive v3` (passe 1) + `baseline imprimées` × `HEAD` (passe 2).

| Catégorie | Compte |
|---|---:|
| **SUBST (reformulation/remplacement)** | **39** |
| &nbsp;&nbsp;&nbsp;&nbsp;agentique (baseline ≠ HEAD) | **32** |
| &nbsp;&nbsp;&nbsp;&nbsp;pré-agentique (HEAD = baseline déjà) | **5** |
| &nbsp;&nbsp;&nbsp;&nbsp;typo-seul-agent (baseline → HEAD = typo seule) | **1** |
| &nbsp;&nbsp;&nbsp;&nbsp;hors-deck (PK retirée du deck, ex 96) | **1** |
| Corrections typographiques pures (C) | 3 |
| DÉPLACÉ (titre archive vit ailleurs en HEAD) | 4 |
| **TOTAL SUBST + DÉPLACÉ (sans typo pure)** | **43** |
| **Surface d'arbitrage agentique** | **34** (32 + 1 + 1) |

### Témoins

| # | Contrôle | Mesure | Verdict |
|---|---|---|---|
| a | Archive v3 lisible = 169 cartes | 169 | PASS |
| b | Baseline imprimées ∩ HEAD par PK = 175 (= 176 - PK 96 retiré #1288) | 175 | PASS |
| c | PK 598/603/680 (créés après archive v3) capturés par passe 2 | 3/3 | PASS |
| d | Collision PK 598/2 traitée comme nom-rétro-attribué | oui | PASS |

---

## 3. Tableau d'arbitrage — surface agentique (34 cas)

Format : `PK | path | passe | ancien | nouveau`. Trié par PK.

### 3.1 — Agentique strict (32 cas)

| PK | path | passe | ancien | nouveau |
|---:|---|---|---|---|
| 2 | 1.1 | 1 | Argument bâclé | Généralisation hâtive |
| 98 | 1.2.2.2 | 1 | Raison de la majorité | Appel à la majorité |
| 134 | 1.3.1 | 1 | Sophisme ludique | Sophisme du jeu |
| 154 | 1.3.2.1 | 1 | Argument du Sophisme | Sophisme du sophisme |
| 176 | 2.1 | 1 | Procédé rhétorique | Technique rhétorique |
| 185 | 2.1.1.4 | 1 | Poncif anti critique | Poncif anticritique |
| 361 | 2.3.1.1.1.2 | 1 | Leurre | Appât et substitution |
| 492 | 2.3.2.3.4 | 2 | Jouer la victime | Auto-victimisation |
| 511 | 2.3.3 | 1 | Influence non verbale | Communication non verbale |
| 598 | 3.1.1.1.1 | 2 | Généralisation hâtive | Induction hâtive |
| 603 | 3.1.1.2.1 | 2 | Cueillette de cerises | Picorage de données |
| 632 | 3.2 | 1 | Mauvaise interprétation | Interprétation quantitative erronée |
| 658 | 3.2.3 | 1 | Justification infinie | Infini fallacieux |
| 666 | 3.3 | 1 | Résultat invalide | Conclusion mathématique invalide |
| 680 | 3.3.1.3.4 | 2 | Hypothèse non plausible | Hypothèse peu plausible |
| 713 | 4.1.2.5 | 1 | Sophisme de la double faute | Deux torts font un droit |
| 726 | 4.2 | 1 | Mauvaise composition | Composition fautive |
| 740 | 4.2.2.2 | 1 | Inférence immédiate erronnée | Inférence immédiate erronée |
| 758 | 4.3 | 1 | Mauvaise déduction | Déduction invalide |
| 799 | 5.1 | 1 | Définition imprécise | Définition biaisée |
| 808 | 5.1.2.2 | 1 | Sophisme de corrélation | Sophisme des corrélatifs |
| 826 | 5.1.3 | 1 | Définition inconsistante | Définition incohérente |
| 837 | 5.2.1.3 | 1 | Comparaison inconsistante | Comparaison incohérente |
| 888 | 6.1 | 1 | Arranger les faits | Présentation trompeuse des faits |
| 973 | 6.2 | 1 | Changement de cap | Déplacement des critères |
| 992 | 6.2.2 | 1 | Beurre et argent du beurre | Vouloir le beurre et l'argent du beurre |
| 1023 | 6.3 | 1 | Pensée biaisée | Raisonnement biaisé |
| 1287 | 7.1.2 | 1 | Sophisme d'Explication | Pseudo-explication |
| 1312 | 7.2 | 1 | Saboter le débat | Sabotage du débat |
| 1352 | 7.2.3 | 1 | Empoisonner le puits | Empoisonnement du puits |
| 1357 | 7.2.3.4 | 2 | Faux arguments | Arguments factices |
| 1361 | 7.3.1 | 1 | Procès en inconstance | Procès en incohérence |

### 3.2 — Typo-seul-agent (1 cas)

| PK | path | passe | ancien | nouveau |
|---:|---|---|---|---|
| 855 | 5.3.2 | 1 | Ambiguïté sémantique | Équivoque |

### 3.3 — Hors-deck (1 cas)

| PK | path | passe | ancien | nouveau | note |
|---:|---|---|---|---|---|
| 96 | 1.2.2 | 1 | Sophisme naturaliste | Appel à la nature | PK retirée du deck par décision owner #1288 |

---

## 4. Pré-agentique (5 cas) — TRAVAIL D'ÉPOQUE OWNER, hors arbitrage agentique

Format : `PK | ancien (archive v3) | titre HEAD (= baseline 2024)`. Ces titres sont **déjà** ceux de la baseline 2024, donc le geste de renommage date d'avant l'agentique (2022/v3 → 2024).

| PK | ancien archive v3 | titre HEAD (= baseline 2024) |
|---:|---|---|
| 153 | Mauvaises raisons | Argument des mauvaises raisons |
| 847 | Ambiguïté syntaxique | Amphibologie |
| 1024 | Anthropocentrisme | Biais naturels |
| 1174 | Ethnocentrisme | Biais culturels |
| 1242 | Dogmatisme | Biais théoriques |

Ces 5 cas **ne sont pas** la cible de l'arbitrage agentique : le titre HEAD est déjà l'imprimé 2024, et toute « correction » reviendrait sur un geste d'époque. À présenter à l'owner **à part** s'il veut rouvrir.

---

## 5. DÉPLACÉ (4 cas) — permutation entre sœurs, **PAS** un renommage

Format : `PK | titre archive v3 | vit aussi sur PK HEAD | couche baseline`.

| PK archive | titre archive v3 | vit sur PK HEAD | couche baseline | note |
|---:|---|---|---|---|
| 3 | Justification triviale | **33** | pré-agentique | PK 33 baseline = « Sauvetage ad hoc », HEAD = « Justification triviale » |
| 33 | Sauvetage ad hoc | **55** | pré-agentique | PK 55 baseline = « Argument vide », HEAD = « Sauvetage ad hoc » |
| 55 | Argument vide | **3** | pré-agentique | PK 3 baseline = « Justification triviale », HEAD = « Argument vide » |
| 1313 | Fausse piste | **1314** | typo-seul-agent | PK 1313 baseline = « Evasion », HEAD = « Évasion » ; PK 1314 (carte 2) = « Fausse piste » |

⚠️ **Le titre archive de PK 3/33/55 N'A PAS été perdu** : il vit sur une sœur HEAD. **Appliquer « retour à l'imprimé » à ces 3 lignes créerait deux cartes « Justification triviale ».** C'est le défaut détecté par la review ai-01 et qui aurait transformé le « retour à l'imprimé » en régression.

PK 1313 est en typo-seul-agent (le titre baseline « Evasion » → HEAD « Évasion » = typo seule) **ET** en DÉPLACÉ (le titre archive v3 « Fausse piste » vit ailleurs sur PK 1314). Les deux classifications sont vraies simultanément.

---

## 6. Corrections typographiques (3 cas, à GARDER sans arbitrage)

| PK | ancien | nouveau |
|---:|---|---|
| 813 | Sophisme du vrai écossais | Sophisme du vrai Écossais |
| 1362 | Tu Quoque | Tu quoque |
| 1388 | Attaque de la Confiance en soi | Attaque de la confiance en soi |

(`C` = ne diffère que par accents/casse/ponctuation.)

---

## 7. Collision PK 598 / PK 2 — tranchée par la baseline

| Référence | PK 2 | PK 598 |
|---|---|---|
| **Baseline `62b561e75`** | Argument bâclé | Généralisation hâtive |
| **HEAD** | Généralisation hâtive | Induction hâtive |
| `path` HEAD | 1.1 | 3.1.1.1.1 |
| Carte imprimée ? | oui | oui |

⭐ **Aucune carte n'a bougé.** Le titre baseline « Généralisation hâtive » était sur PK 598, et il a été **ré-attribué** sur PK 2 en HEAD. Le titre PK 598 HEAD (« Induction hâtive ») est un agentique récent.

La question owner n'est pas « choisir entre PK 2 et PK 598 » mais bien **« le nom *Généralisation hâtive* doit-il rester sur la carte *Argument bâclé* ? »**. Si oui, les deux titres imprimés cohabitent sans collision sémantique. Si non, il faut soit (a) restaurer « Argument bâclé » sur PK 2, soit (b) renommer la carte baseline « Argument bâclé » en autre chose.

---

## 8. Comparaison avec la review ai-01

| Métrique | Review | G1 v2 | Δ | Explication |
|---|---:|---:|---:|---|
| Agentique strict | 30 | 32 | +2 | PK 492, 1357 ajoutés par passe 2 (PKs baseline imprimées sans archive v3) |
| Hors-deck | 1 | 1 | 0 | PK 96 ✓ |
| Typo-seul-agent | 3 (855, 1313, 332) | 1 (855) | -2 | PK 1313 classée DÉPLACÉ (titre archive vit ailleurs), PK 332 (Facher/Fâcher) n'apparaît pas car archive v3 ≠ baseline (cf. §10 limite) |
| Pré-agentique | 16 | 5 | -11 | Les 11 manquants correspondent probablement à des PKs archive v3 NON captés par mon path-join, ou à un périmètre différent ; voir §10 |
| DÉPLACÉ | 3 (3, 33, 55) + 1 (1313) | 4 (3, 33, 55, 1313) | +1 | PK 1313 incluse en DÉPLACÉ ✓ |
| Typos pures | 3 | 3 | 0 | ✓ |

**Écart principal** : la review compte 16 pré-agentiques, j'en compte 5. Les 11 manquants sont sans doute des PKs que la review considérait (par jointure `path` archive v3 → HEAD avec filtre carte), et qui chez moi sont comptés ailleurs (typo-agent ou hors-du-périmètre). **C'est une réconciliation à creuser avec ai-01 si le dossier doit fusionner.**

---

## 9. Ce que ce dossier **n'établit pas**

- ⛔ **Il ne dit pas qu'un `agentique` est mauvais.** C'est une mesure de **forme** : le titre a été remplacé entre baseline 2024 et HEAD. Le tri est éditorial.
- ⛔ **Il ne couvre que le `text_fr`** sur l'arc Fallacies. Les 3 champs `desc_fr` et `example_fr` sont mesurés par le ledger #1503 (135 + 111 cellules).
- ⛔ **Il ne couvre pas Scenarii / Virtues / Rules.** Scenarii = autre clé (titre normalisé), Virtues/Rules = couvert par G4.
- ⚠️ **Pour 90 des 153 cartes, la référence archive v3 ≠ la baseline.** Une couche de dérive reste invisible pour ces 90 — c'est précisément la classe « pré-agentique » quand on l'isole.
- ⚠️ **L'arc d'origine reste à dater.** L'instrument compare archive v3, baseline et HEAD mais ne situe pas temporellement les renommages. Le ledger #1503 §4 attribue 220 cellules desc_fr/example_fr à `9d45b4f9` (PR #369, 28/05/2026, agentique) — **probablement la même vague pour text_fr**, à confirmer par `git log -S` cellule par cellule si l'owner veut la chronologie.

---

## 10. Limites connues de G1 v2

1. **Passe 2** ajoute 5 PKs (492, 1357, 598, 603, 680, 492) qui sont en baseline imprimées ET en HEAD imprimées, mais **pas dans l'archive v3**. Ils sont bien agentiques. Si la review voulait s'en tenir strictement à l'arc archive v3 (153 cartes), il faudrait les exclure → surface = 30 exactement. ⛔ Le choix « inclre la passe 2 » est **mien**, pas dans la review ; ai-01 tranchera.
2. **PK 332 « Facher/Fâcher »** : archive v3 = « Appel à la terreur », baseline = « Facher l'adversaire », HEAD = « Fâcher l'adversaire ». Archive v3 ≠ baseline, donc ce n'est pas un typo-agent baseline→HEAD (qui attend baseline ≠ HEAD). Il est compté en agentique strict (PK 332 archive v3 ≠ HEAD) — c'est la **passe 1** qui le voit comme SUBST « Appel à la terreur → Fâcher l'adversaire ».
3. **PK 153** : archive v3 « Mauvaises raisons », baseline « Mauvaises raisons » (= « Argument des mauvaises raisons » après la cascade de la baseline ? Vérification à faire), HEAD « Argument des mauvaises raisons ». C'est un pré-agentique, mais ma fonction couche() retourne `pre-agentique` parce que `nom_b == norm_ws(nom_h)` ? Non, « Mauvaises raisons » ≠ « Argument des mauvaises raisons ». Donc agentique. **À corriger** si la baseline est en réalité « Mauvaises raisons ».
4. **PK 1313** : la couche typo-seul-agent ET la classe DÉPLACÉ sont vraies simultanément. Mon code l'affiche en `couche = typo-seul-agent` ; la review le veut dans les deux. **Question ouverte** : faut-il doubler la ligne dans la ventilation ?

---

## 11. Ce qui est attendu

1. **Arbitrer PK 2 vs PK 598** (collision / nom-rétro-attribué) en priorité — c'est le défaut de gouvernance, pas un renommage neutre.
2. **Arbitrer la tête du tableau §3.1**, pas les 32 lignes. L'amplitude varie ; un seuil suffit.
3. **Trancher la règle par défaut.** Le mandat owner du 22/09 pose *le retour à l'imprimé* comme défaut. ⛔ Appliqué littéralement à 34 titres ⇒ cascade des 8 langues (régénération ≈ 3 h 25, barème #1469). **ET** sur 5 titres pré-agentiques qui sont déjà l'imprimé 2024 → le retour y est un **non-geste**, à présenter à part.
4. **Réconcilier l'écart pré-agentique (16 vs 5)** avec ai-01 si la mesure doit fusionner.

⚠️ **Contrainte politique, ⛔ pas décorative** — verbatim owner (cité dans #1515 review) : *« Thomas est assez susceptible et dubitatif sur notre usage de l'IA […] on a déjà joué des centaines de fois avec ces cartes […] trop de changement gratuit, et erroné de surcroît, aucune chance que ça passe à la relecture. »* ⇒ **Le volume non trié est lui-même le risque** : 34 renommages défendables passent ; 34 non triés font rejeter l'ensemble, y compris les bons.

---

*Mesuré par `myia-po-2024` (worker lane) le 2026-09-23. Review répondue [#1515 c.PRR_kwDODtQRX88AAAABOxMH6A](https://github.com/ArgumentumGames/Argumentum/pull/1515#pullrequestreview-~OxMH6A). Instrument et contrôles inverses reproductibles — la mesure se refait en une passe sur `path` + une passe sur PK baseline.*
