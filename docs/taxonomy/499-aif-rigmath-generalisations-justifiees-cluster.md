# AIF #499 — Virtues Cluster PR-5 : Rigueur mathématique (3) / Généralisations justifiées (3.1)

> **Schéma I/RA/CA ratifié #707§4(a), Option A ratifiée** (ai-01 sous délégation jsboige 2026-07-07). 2 colonnes
> additives `AIF_attackType` (undermine/undercut/rebut) + `AIF_attackedNode` (I-node/RA-node/CA-node). Couche
> additive I/RA/CA sur le grounding relationnel 12-col (phase-2 batch 2 `rigmath.md`, CLEAN #518).
>
> **PR-1** 4.1 Causalités (Cause to Effect, 4/4 undercut) · **PR-2** 6.1 Fidélité (Witness/Bias, mixte) ·
> **PR-3** 7.3 Respect (Commitment, 6/6 undercut) · **PR-4** 5.1 Définitions (Verbal Classification, mixte,
> résout borderline 804) · **PR-5 (ce doc)** 3.1 Généralisations (Example + **Rule** + Analogy) — **valide le
> scheme dominant Argument from Rule + confirme le discriminateur undermine raffiné sur un cas statistique**.

---

## 1. Cluster — Généralisations justifiées (path 3.1)

Family 3 *Rigueur mathématique* / subfamily 3.1 *Généralisations justifiées* — anchor pk 60 + leaves 61 / 62 / 63.
3 schemes distincts en 3 feuilles : **Argument from Example** (61), **Argument from Rule** (62), **Argument from Analogy** (63).

| Virtue leaf (path) | pk | Titre FR | Scheme Walton | CQ restaurée | Fallacies opposées |
|---|---|---|---|---|---|
| 3.1.1 | 61 | Échantillonnage représentatif | Argument from Example | L'échantillon est-il **représentatif** de la population cible ? | 596 Échantillon biaisé · 759 Conclusion hâtive |
| 3.1.2 | 62 | Prise en compte des exceptions | **Argument from Rule** | La règle générale tient-elle compte des **exceptions** pertinentes au cas particulier ? | 614 Sophisme de l'accident · 759 Conclusion hâtive |
| 3.1.3 | 63 | Transfert licite | Argument from Analogy | Le transfert d'une caractéristique est-il **licite** (groupe↔éléments) ? | 621 Transfert illicite · 839 Fausse analogie |

### 1.1 Miroir Fallacies — path-prefix EXACT (4e fois)

La subfamily Fallacies *Erreur mathématique / Généralisations* est au **path 3.1.x** (596/614/621), la subfamily
Virtues est au **path 3.1** — miroir 1:1 exact. Corroboration path-prefix cumulative : PR-1 4.1, PR-2 6.1,
PR-4 5.1, PR-5 3.1 = **4 miroirs exacts** ; PR-3 7.3 = partiel. La structure path-prefix Virtues↔Fallacies
est la règle, la transversalité l'exception.

Cross-family documenté : 759 Conclusion hâtive (Erreur raisonnement 4.3.1), 839 Fausse analogie (Abus de langage 5.2.2).
La vertu *généralisation justifiée* est violée par la hâte (759, raisonnement) et l'analogie forcée (839, langage),
pas seulement par l'erreur mathématique directe.

---

## 2. Décomposition I/RA/CA — attack-type + attacked-node (Option A)

### 2.1 🔑 Test du discriminateur undermine raffiné (PR-4 §3.1) sur 596 Échantillon biaisé

PR-4 a raffiné le discriminateur : `undermine`/I-node = **assertion d'une prémisse fabriquée/fausse** (889 Mensonge,
804 Acception arbitraire), par opposition aux tool-misuse et credibility-attacks (`undercut`/RA). 596 est le cas
frontière statistique idéal pour tester ce discriminateur :

**596 Échantillon biaisé** — *« Vous tirez des conclusions à partir d'un échantillon qui n'est pas représentatif
de la population cible »*.
- L'échantillon est **biaisé** (sélection défectueuse), **pas fabriqué**. Les données de l'échantillon peuvent
  être factuellement exactes (chacune mesurée correctement) — c'est leur **représentativité** qui fait défaut.
- L'inférence (échantillon → population) ne tient pas : la règle « l'échantillon représente la population »
  est invalide pour ce tirage.
- → **undercut / RA-node**, **PAS undermine**. La prémisse (les données de l'échantillon) n'est pas fausse ;
  c'est le saut inférentiel qui est cassé.

**Confirmation du discriminateur** : `undermine` reste réservé aux **prémisses fabriquées/fausses** (889, 804).
Une sélection biaisée (596), une imprécision (667), un outil mal utilisé (644) = tous `undercut`/RA. La
frontière tient : le défaut porte sur l'**inférence**, pas sur la **vérité de la prémisse**. Ce raffinement
renforce la prévisibilité du back-fill plan #744 §2a.

### 2.2 Dérivation par leaf

**3.1.1 pk 61 — Échantillonnage représentatif** (Argument from Example : l'exemple échantillonne la population)
- 596 Échantillon biaisé → **undercut / RA-node** (§2.1, sélection biaisée = inference sample→population cassée).
- 759 Conclusion hâtive (*« conclusions trop vite, sans assez de preuves »*) : preuves insuffisantes →
  l'inférence example→généralisation n'est pas supportée → **undercut / RA-node**. (Pas un rebut : pas de
  contre-conclusion, un déficit de support.)

**3.1.2 pk 62 — Prise en compte des exceptions** (Argument from Rule : la règle se transfère au cas)
- 614 Sophisme de l'accident (*« applique une règle générale à un cas particulier, sans tenir compte des
  exceptions »*) : la règle est appliquée là où une exception la disqualifie → l'inférence règle→cas est
  incorrecte → **undercut / RA-node**. **Cas paradigmatique du RA-node** : c'est littéralement la Rule
  Application (Accident = l'archétype de la défaillance de transfert de règle). Scheme Argument from Rule
  parfaitement aligné.
- 759 Conclusion hâtive → **undercut / RA-node** (§62, déficit de support).

**3.1.3 pk 63 — Transfert licite** (Argument from Analogy : l'analogie autorise le transfert)
- 621 Transfert illicite (*« prêtez à un groupe les caractéristiques de ses éléments, ou l'inverse »*) :
  composition/division — le transfert groupe↔élément est illicite → l'inférence analogique ne tient pas →
  **undercut / RA-node**.
- 839 Fausse analogie (*« parallèle basé sur un seul point commun, négligeant les différences importantes »*) :
  l'analogie est superficielle → l'inférence transfert est invalide → **undercut / RA-node**.

### 2.3 Distribution attack-type — cluster 3.1

| Leaf | Fallacy | attackType | attackedNode |
|---|---|---|---|
| 61 | 596 Échantillon biaisé | undercut | RA-node |
| 61 | 759 Conclusion hâtive | undercut | RA-node |
| 62 | 614 Sophisme de l'accident | undercut | RA-node |
| 62 | 759 Conclusion hâtive | undercut | RA-node |
| 63 | 621 Transfert illicite | undercut | RA-node |
| 63 | 839 Fausse analogie | undercut | RA-node |

**= 6/6 `undercut` / `RA-node`** — uniforme comme PR-1 (4.1) et PR-3 (7.3). **0 undermine, 0 rebut.** Les
fallacies de généralisation sont toutes des **défaillances d'inférence** (échantillon→population, règle→cas,
analogie→transfert) : aucune n'asserte une prémisse fabriquée, aucune ne présente de contre-conclusion.

---

## 3. 🔑 Validation du scheme dominant Argument from Rule (50 nœuds pleine échelle)

PR-5 couvre **Argument from Rule** (pk 62) pour la première fois dans le chantier I/RA/CA. C'est le scheme
**le plus représenté** à pleine échelle (50 nœuds / 222 = ~22% de la taxonomie). Sa validation sur le cas
paradigmatique 614 (Sophisme de l'accident = archétype de la défaillance RA-node) confirme :

- **Argument from Rule → défaut `undercut` / `RA-node`**. Une vertu « règle bien appliquée » est résistée
  par la mauvaise application de règle (614), qui défait le Rule Application node. Logique identique au
  défaut général, cohérent avec les 50 nœuds Rule attendus en `undercut`/`RA-node` dans le back-fill plan.
- **Pas d'override** attendu pour les nœuds Rule : le scheme Rule ne génère pas de prémisse fabriquée (pas
  d'undermine) ni de contre-conclusion (pas de rebut). Le back-fill plan peut traiter les 50 nœuds Rule en
  pur défaut mécanique.

**Impact back-fill plan #744** : confirme que le scheme n'est PAS un axe d'override (l'override vient du
**mécanisme de la fallacy opposée**, pas du scheme de la vertu). Les 50 nœuds Rule = `undercut`/`RA-node`
sauf si la fallacy opposée est 889/804 (mine). Renforce la simplicité du script apply.

---

## 4. Finding cumulatif rebut-rarity (5 clusters) — toujours 0 rebut

| Cluster | Subfamily | Famille | Schemes | mine | undercut | rebut |
|---|---|---|---|---|---|---|
| PR-1 | 4.1 Causalités | Raisonnement | Cause to Effect | 0 | 6 | 0 |
| PR-2 | 6.1 Fidélité | Honnêteté | Witness/Bias | 1 | 2 | 0 |
| PR-3 | 7.3 Respect | Échange | Commitment | 0 | 6 | 0 |
| PR-4 | 5.1 Définitions | Langage | Verbal Classification/Expert | 3 | 7 | 0 |
| **PR-5** | **3.1 Généralisations** | **Rigueur math** | **Example/Rule/Analogy** | **0** | **6** | **0** |
| **Cumul** | **5 subfamilies** | **5 familles / 8 schemes** | | **4** | **27** | **0** |

**rebut/CA-node = 0 sur 5 familles / 8 scheme-types distincts / ~31 fallacy-instances.** Le finding rebut-rarity
(PR-3 §3, back-fill plan #744 §2b) se renforce encore : même sur les schemes de généralisation (Example, Rule,
Analogy) qui sont les candidats naturels pour abriter des contre-conclusions (règle contraire, exemple
contraire), rebut reste absent. La dérivation deterministe du back-fill plan tient sans aucun cas rebut ajouté.

### 4.1 Coverage des 14 schemes — état après PR-5

| Scheme | nœuds pleine échelle | Couvert en cluster ? |
|---|---|---|
| Argument from Rule | 50 | **✅ PR-5 (pk 62)** |
| Argument from Commitment | 40 | ✅ PR-3 |
| Argument from Bias | 27 | ✅ PR-2 |
| Argument from Sign | 26 | ⏳ (famille 3/1, à venir) |
| Argument from Verbal Classification | 21 | ✅ PR-4 |
| Argument from Cause to Effect | 11 | ✅ PR-1 |
| Argument from Witness Testimony | 10 | ✅ PR-2 |
| Argument from Position to Know | 8 | ⏳ (famille 1) |
| Argument from Values | 8 | ⏳ (famille 2) |
| Argument from Analogy | 7 | **✅ PR-5 (pk 63)** |
| Argument from Expert Opinion | 6 | ✅ PR-4 |
| Argument from Example | 4 | **✅ PR-5 (pk 61)** |
| Argument from Consequences | 3 | ⏳ (famille 2) |
| Argument from Danger | 1 | ⏳ |

**10/14 schemes couverts** (dont les 6 plus représentés : Rule, Commitment, Bias, Verbal Classification,
Cause to Effect, Witness Testimony = 165 nœuds / 222 = 74% de la taxonomie). Restent : Sign (26, famille 1/3),
Position to Know (8, famille 1), Values (8, famille 2), Consequences (3, famille 2), Danger (1).

---

## 5. Honnêteté de modélisation (discipline #677)

- **0 fabrication** : undercut/undermine/RA-node/I-node/Argument from Example/Rule/Analogy = tous natifs AIF/Walton.
- **596 testé honnêtement** : classé `undercut`/RA (pas undermine) malgré la tentation « data fallacy → mine ».
  Preuve : l'échantillon biaisé n'est pas une prémisse fabriquée, c'est une sélection défaillante → inference
  cassée. Le discriminateur PR-4 tient et se confirme.
- **614 = RA-node paradigmatique** documenté (Accident = archétype Rule Application failure), pas forcé.
- **Miroir cross-family documenté** (§1.1) : 759 (Erreur raisonnement) et 839 (Abus de langage) atteints
  légitimement hors-3.1.
- **0 rebut ni forcé ni écarté par idéologie** : set rebut vide par structure du domaine.
- **Anti-duplication** : `rigmath.md` (phase-2 batch 2) couvre le grounding relationnel 12-col des 16 nœuds
  famille 3 ; ma couche I/RA/CA est additive (grep global confirmé : aucun doc phase-2 ne décompose I/RA/CA).

---

## 6. État chantier Virtues I/RA/CA — cumul PR-1 + PR-2 + PR-3 + PR-4 + PR-5

- **5 subfamilies** (4.1 Causalités, 6.1 Fidélité, 7.3 Respect, 5.1 Définitions, 3.1 Généralisations).
- **5 familles** sur 8 (Raisonnement, Honnêteté, Échange, Langage, Rigueur math). Restent : 1 Argument pertinent,
  2 Présentation intègre, (0 Argument valable = racine).
- **~17 feuilles, ~31 fallacy-instances**.
- **Attack-types** : undercut (27) + undermine (4) + rebut (0).
- **10/14 schemes couverts** (74% de la taxonomie en nœuds), dont les 6 plus représentés.
- **Miroirs path-prefix** : 4 exacts + 1 partiel.
- Restant : ~205 nœuds / ~10-13 subfamilies. Back-fill post-contrat po-2023 largement mécanique
  (undercut dominant ~94%, override mine pour 889/804, 0 rebut).
