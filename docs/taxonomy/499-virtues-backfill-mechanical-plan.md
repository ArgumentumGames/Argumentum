# #499 Virtues AIF — Plan mécanique back-fill 222-rows (Option A, HOLD prod)

> **Status : PLAN PRÊT, exécution GATED.** Ce doc spécifie la dérivation déterministe des 2 nouvelles colonnes
> `AIF_attackType` + `AIF_attackedNode` (schéma ratifié #707§4 Option A) pour les **222 nœuds** de la taxonomie
> Virtues, à partir du grounding relationnel 12-col existant (phase-2, CLEAN 222/222, validator #518).
>
> **⚠️ HOLD exécution** (ordre ai-01 2026-07-07, reconfirmé 2026-07-08) : **0 write** du CSV prod
> `Argumentum Virtues - Taxonomy.csv` jusqu'à ce que **po-2023 pose le contrat de colonnes côté Fallacies #498**
> (noms d'en-têtes exacts + valeurs canoniques) sur master ET que ai-01 le reviewe. **Anti-drift** : on mirror
> le format exact de po-2023, pas l'inverse. Ce plan est prêt à devenir 1 PR programmatique dès que le contrat
> est posé.
>
> **Option A ratifiée** (ai-01 sous délégation jsboige 2026-07-07) : colonnes = **attack-type prévenu** +
> **node maintenu**. Une vertu = bonne tenue d'un scheme → la fallacy tente de défaire un composant AIF
> que la vertu maintient.
>
> **Raffinement v2** (ce doc, PR post-#749) : intègre 2 overrides concrets révélés par les clusters PR-4 et PR-6 :
> **804 Acception arbitraire** borderline → **clean undermine/I-node** (triade 800/804/826, PR #745 mergée) ;
> **rebut/CA-node** set vide → **{340 Appel aux conséquences}** (argumentum ad consequentiam = contre-conclusion,
> PR #747 mergée). La règle de dérivation est maintenant **exécutable telle quelle** (3 overrides concrets).
> Généralise la méthode validée sur **8 clusters** (PR #741-#749, ~26 feuilles, 49 fallacy-instances, 87% coverage).

---

## 1. Règle de dérivation déterministe (2 étapes)

La valeur de chaque nœud est dérivée de **(a) son scheme Walton** + **(b) le mécanisme de la/les fallacy(s) opposée(s)**.
La règle est déterministe et programmable. Elle généralise la méthode validée sur **8 clusters** (PR #741-#749,
~26 feuilles, 49 fallacy-instances).

### Étape 1 — défaut = `undercut` / `RA-node`

Le défaut pour **tous** les nœuds est `undercut` / `RA-node`. Justification empirique (cf. §4) : sur les
60 fallacies distinctes opposées aux 222 nœuds, la quasi-totalité casse l'**inférence** (scheme non tenu) —
crédibilité, engagement, biais, lien causal, lien analogique, usage d'outil logique/probabiliste,
classification verbale mal appliquée — sans assertion d'une proposition factuellement fausse.

`RA-node` = le nœud d'inférence (Rule Application) est la cible : la vertu maintient l'inférence valide,
la fallacy la défait.

### Étape 2 — overrides (3 sets concrets)

#### 2a. `undermine` / `I-node` — assertion d'une proposition connue-fausse ou fabriquée

Override **uniquement** si la fallacy opposée asserte une **proposition fausse** (le contenu factuel I-node
est faux), pas un usage d'outil ni une crédibilité. Discriminateur :

| La fallacy… | → attack-type |
|---|---|
| asserte un fait qu'elle sait faux, ou fabrique une définition/donnée (la **proposition elle-même** est fausse) | **undermine / I-node** |
| casse le raisonnement (crédibilité source, biais, saut causal, outil mal utilisé, engagement court-circuité) | undercut / RA-node (défaut) |

**Set undermine validé** (scan des 60 fallacies opposées + résolution per-case PR-4) :

| Fallacy | desc_fr | Nodes l'opposant | Verdict |
|---|---|---|---|
| **889 Mensonge** | « Vous affirmez quelque chose que vous savez faux » | **9 nœuds** | **undermine / I-node** (clean — la proposition I-node est factuellement fausse) |
| **804 Acception arbitraire** | « Vous inventez une définition sur mesure pour vos termes » | **4 nœuds** | **undermine / I-node (CLEAN, raffiné PR-4)** — la définition-fabrication = prémisse I-node fausse. **N'est plus borderline** : la triade 800/804/826 (PR #745) discrimine — 800 vague = undercut, **804 fabriquée = undermine**, 826 contradictoire = undercut. |

**Discriminateur undermine raffiné** (PR-4 §3.1 + PR-5 §2.1 + PR-7 §2.1 + PR-8 §2.1, quadruple-confirmé) :
`undermine`/I = **assertion d'une prémisse de fond fabriquée/fausse** (889, 804). À distinguer de :
- *sélection biaisée* (596 Échantillon biaisé PR-5, 953 Attention sélective PR-7/PR-8) → les données peuvent
  être vraies, c'est la sélection qui fait défaut → **undercut/RA** ;
- *biais cognitif / cadre distordu* (1242 PR-7, 1024 PR-8) → traitement de l'inférence → **undercut/RA** ;
- *crédibilité source* (942 Fausse attribution PR-2/PR-8) → la falsification porte sur la *qualification de
  source*, pas sur la proposition de fond P → défait l'inférence → **undercut/RA**.

**Faux positifs écartés** (tool-misuse / credibility → undercut/RA, PAS undermine) :
- 644 Probabilités faussées (« fausse votre RAISONNEMENT ») → undercut/RA
- 727/735/750 Erreurs logiques (propositionnel/quantification/modalité) → undercut/RA
- 1287 Pseudo-explication (« feignant d'expliquer ») → undercut/RA
- 942 Fausse attribution (source fabriquée = crédibilité testimony → undercut/RA, cf. PR-2/PR-8)
- 974 Exigence renforcée (moving-goalposts = engagement/burden → undercut/RA)

#### 2b. `rebut` / `CA-node` — contre-conclusion indépendante

Override si la fallacy présente une **contre-conclusion indépendante** soutenue par son propre argument
(CA-node ↔ conclusion I-node adverse).

**Set rebut : {340}.** **NON-VIDE** (raffinement PR-6, PR #747 mergée) — le keyword-scan initial
(contre-argument / position adverse / conclusion opposée / réfut) donnait 0 occurrence car trop littéral ;
l'analyse per-case du mécanisme AIF révèle 340.

| Fallacy | desc_fr | Nodes l'opposant | Verdict |
|---|---|---|---|
| **340 Appel aux conséquences** | « fondez votre argumentation sur les conséquences d'une idée plutôt que sur sa justesse » (= argumentum ad consequentiam) | **3 nœuds** (53, 54 Consequences ; 165 Danger) | **rebut / CA-node (clean — PR-6)** |

**Mécanisme AIF** : la fallacy substitue l'évaluation des conséquences à l'évaluation de la vérité, assertant
une **contre-conclusion** (rejeter P / non-P) qui **conflit** avec la conclusion ciblée (P). L'attaque porte sur
la **conclusion** (I-node conclusion contredit), médiée par un **CA-node** (Conflict Application). → `rebut`/`CA-node`.
Natif AIF : Argument from Consequences = scheme Walton listé ; CA-node natif.

**🔑 Leçon load-bearing (keyword-scan incomplétude)** : le set rebut a été manqué par le keyword-scan initial
(§2b v1) car 340 ne dit pas « conclusion opposée » mot pour mot — elle **opère** la contre-conclusion en fondant
l'argument sur les conséquences. **Les overrides rebut doivent venir de l'analyse per-case du mécanisme AIF**,
pas d'un keyword-set figé. La règle déterministe encode 340 comme override concret ; le set reste **ouvert à
extension** si une future fallacy révèle un mécanisme de contre-conclusion (analyse per-case, pas keyword).

**Note pk 165 (Argument from Danger)** : seul nœud du scheme Danger (1 à pleine échelle), il oppose 340.
Verdict rebut/CA **inféré du mécanisme 340 stable** (intrinsèque à la fallacy, indépendant du scheme de la
vertu) — **pas observé per-case en cluster doc** (famille 6.2 non décomposée). À confirmer si cluster 6.2 est
traité ; en attendant, la règle déterministe l'affecte à rebut/CA de façon fiable (340 → rebut quel que soit le
scheme opposant).

PK 1398 *Attaque personnelle* (Obstruction) est défini dans la taxonomie comme *« au lieu de réfuter »* =
négation explicite du rebuttal — confirme que rebut est rare et localisé, et que la plupart des fallacies
relationnelles (ad hominem, etc.) restent en undercut/RA.

---

## 2. Inputs pleine échelle (222 nœuds)

| Métrique | Valeur |
|---|---|
| Nœuds Virtues avec scheme + opposes (CLEAN #518) | **222 / 223** (1 nœud racine sans scheme) |
| Familles de scheme Walton distinctes | **14** |
| Fallacies distinctes opposées | **60** |
| Profondeur max | 6 (path `x.y.z.w.v.u`) |

### Distribution schemes (222 nœuds)

| Scheme | nœuds | attack-type défaut |
|---|---|---|
| Argument from Rule | 50 | undercut/RA |
| Argument from Commitment | 40 | undercut/RA |
| Argument from Bias | 27 | undercut/RA |
| Argument from Sign | 26 | undercut/RA |
| Argument from Verbal Classification | 21 | undercut/RA (sauf oppose-804 → undermine/I) |
| Argument from Cause to Effect | 11 | undercut/RA |
| Argument from Witness Testimony | 10 | undercut/RA |
| Argument from Position to Know | 8 | undercut/RA |
| Argument from Values | 8 | undercut/RA |
| Argument from Analogy | 7 | undercut/RA |
| Argument from Expert Opinion | 6 | undercut/RA |
| Argument from Example | 4 | undercut/RA |
| Argument from Consequences | 3 | **undercut/RA sauf oppose-340 → rebut/CA** (53, 54) |
| Argument from Danger | 1 | **rebut/CA** (165 oppose 340 — seul nœud du scheme) |

**Note** : le scheme ne pilote PAS l'attack-type (tous défaut = undercut/RA). C'est le **mécanisme de la fallacy
opposée** qui pilote l'override (undermine 889/804, rebut 340). Le scheme reste load-bearing pour la colonne
existante `AIF_skosDirectRef` et pour la CQ (`AIF_skosMappingType`), pas pour l'attack-type. **Validé sur les
3 top schemes** (Rule 50 PR-5, Sign 26 PR-7, Position to Know 8 PR-8 = 84 nœuds / ~38% de la taxonomy en pur
défaut mécanique, 0 scheme-override observé).

---

## 3. Distribution de sortie attendue (post-back-fill, raffinée)

| attack-type | nœuds | % | Source |
|---|---|---|---|
| **undercut / RA-node** | **206** | **92.8%** | défaut (reste après overrides) |
| **undermine / I-node** | **13** | **5.9%** | override §2a (9×889 + 4×804) |
| **rebut / CA-node** | **3** | **1.4%** | override §2b (3×340 : pks 53, 54, 165) |

**Total** : 222 nœuds. **Distribution exacte** (calculée par scan programmatique du CSV, 0 overlap entre sets
340∩889∩804). `undermine_pks = {34, 36, 49, 55, 56, 73, 137, 139, 140, 153, 154, 155, 158}`,
`rebut_pks = {53, 54, 165}`, le reste (206) = undercut/RA.

**Implication tooling** (load-bearing pour ai-01/jsboige) : la colonne `AIF_attackType` sera
**undercut-dominée à ~93%**. Ne pas sur-investir la couverture rebut/undermine dans le validateur —
leur rareté (mine ~6%, rebut ~1%) est la structure réelle du domaine, pas un gap de modélisation. rebut est
**non-nul mais localisé** au mécanisme contre-conclusion (340 uniquement).

---

## 4. Preuve de généralisation (8 clusters validés → règle pleine échelle)

La règle ci-dessus est la **généralisation déterministe** de la méthode validée sur **8 clusters empiriques** :

| Cluster (PR) | Subfamily | Scheme(s) | Distribution observée | Règle dérivée |
|---|---|---|---|---|
| #741 PR-1 | 4.1 Causalités | Cause to Effect | 6/6 undercut/RA | défaut tient |
| #742 PR-2 | 6.1 Fidélité | Witness, Bias | 1 undermine (889) + 2 undercut | override undermine pour 889 + credibility-attack 942 |
| #743 PR-3 | 7.3 Respect | Commitment | 6/6 undercut/RA | défaut tient (finding rebut-rarity directionnel) |
| #745 PR-4 | 5.1 Définitions | Verbal Classification, Expert | 3 undermine (804) + 7 undercut | **804 borderline → clean undermine/I** (triade 800/804/826) |
| #746 PR-5 | 3.1 Généralisations | Example, Rule, Analogy | 6/6 undercut/RA | scheme Rule ≠ axe d'override ; discriminateur mine sur 596 |
| #747 PR-6 | 2.2 Équilibre émotionnel | Bias, **Consequences** | 4 undercut + **2 rebut (340)** | **1er rebut/CA-node (PK 340) → set rebut {340}** |
| #748 PR-7 | 1.1 Argument fondé | **Sign**, Bias | 6/6 undercut/RA | scheme Sign ≠ axe d'override ; discriminateur mine sur 953 |
| #749 PR-8 | 1.2 Prémisses fiables | **Position to Know**, Bias | 6/6 undercut/RA | scheme PtK ≠ axe d'override ; credibility-attack 942 confirmé |

**Cumul** : 43 undercut + 4 undermine + 2 rebut sur 8 familles / ~26 feuilles / 49 fallacy-instances (rebut
observé en cluster sur 53/54 seulement ; pk 165 inféré du mécanisme 340 au scan pleine échelle). Le scan pleine
échelle (§2a/§2b) étend ce finding à 60 fallacies : **13 undermine (889×9 + 804×4) + 3 rebut (340×3) + 206
undercut**. **La règle est stable et exécutable.**

---

## 5. Spécification programmatique (prête à exécuter post-contrat)

Script `tools/499-virtues-backfill-apply.py` (à créer dès contrat #498 posé). Pseudo-code :

```python
# GATING : 0 write tant que contrat_colonnes_po2023 == False
import csv

UNDERMINE_FALLACIES = {"889", "804"}               # prémisses de fond fabriquées/fausses
                                                   # 804 raffiné clean mine (triade 800/804/826, PR-4)
REBUT_FALLACIES     = {"340"}                      # contre-conclusion via conséquences (argumentum ad
                                                   #   consequentiam, PR-6) — set ouvert à extension per-case
VIRTUES_CSV = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
# CONTRAT po-2023 (TBD — valeurs canoniques exactes à mirror) :
ATTACK_HEADER   = "AIF_attackType"      # valeurs: "undercut" | "undermine" | "rebut"  (po-2023 à confirmer)
NODE_HEADER     = "AIF_attackedNode"    # valeurs: "RA-node" | "I-node" | "CA-node"   (po-2023 à confirmer)

rows = read_csv(VIRTUES_CSV)
mine = rebut = undercut = 0
for row in rows:
    if not row["AIF_skosDirectRef"].strip():
        continue                                  # nœud racine sans scheme → skip (ou valeur vide)
    opposed = {p.strip() for p in row["crossLink_Opposes"].split(";") if p.strip()}
    # Étape 1 — défaut
    attack_type, node = "undercut", "RA-node"
    # Étape 2a — override undermine (précédence : 889/804 avant 340 ; 0 overlap vérifié)
    if opposed & UNDERMINE_FALLACIES:
        attack_type, node = "undermine", "I-node"
    # Étape 2b — override rebut (contre-conclusion)
    elif opposed & REBUT_FALLACIES:
        attack_type, node = "rebut", "CA-node"
    row[ATTACK_HEADER] = attack_type
    row[NODE_HEADER]   = node

write_csv(VIRTUES_CSV, rows)                      # GATED — cf. §6
```

**Distribution attendue** (vérification post-exécution) : 206 undercut/RA + 13 undermine/I + 3 rebut/CA = 222.
**0 overlap** entre les sets (vérifié : aucun nœud n'oppose à la fois 340 et 889/804), donc la précédence
`undermine > rebut` est sans ambiguïté.

**Note hygiène CSV (CLAUDE.md)** : le back-fill **ajoute 2 colonnes** au CSV prod, ne modifie aucune cellule
existante. Respecte la règle « ne JAMAIS modifier le CSV avant injection CardPen » (les 2 colonnes sont
additives, hors périmètre CardPen qui consomme `text_fr`/`desc_fr`/etc.). Néanmoins, exécution GATED ordre ai-01.

---

## 6. Anti-drift gating (séquence obligatoire)

1. **po-2023 pose le contrat colonnes** sur master côté Fallacies #498 (dispatch `knodyx`) : noms d'en-têtes
   exacts + valeurs canoniques (`undercut`/`undermine`/`rebut`, `RA-node`/`I-node`/`CA-node` — ou variantes).
2. **ai-01 reviewe** le contrat Fallacies.
3. **po-2024 mirror** : ce plan est ajusté aux valeurs canoniques exactes de po-2023 (variables
   `ATTACK_HEADER`/`NODE_HEADER`/valeurs dans le script).
4. **po-2024 exécute** le back-fill sur une branche `feat/499-virtues-aif-backfill`, 1 PR, **0 write Cards/ régén DB OWL**.
5. **Validator #518** + tests de régression (mirror `MindMapLocalizationRegressionTests`) passent.
6. **ai-01 review + merge**.

**Jusqu'à l'étape 3** : ce plan reste un doc, 0 write.

---

## 7. Checklist de validation (fail-loud)

Avant/après exécution, vérifier :

- [ ] Contrat colonnes po-2023 présent sur master + reviewé ai-01 (prérequis GATING).
- [ ] Header names + canonical values mirror exactement po-2023 (anti-drift).
- [ ] 222 nœuds reçoivent une valeur (1 nœud racine sans scheme = valeur vide ou skip, documenté).
- [ ] `AIF_attackType` ∈ {undercut, undermine, rebut} — 0 valeur hors-enum.
- [ ] `AIF_attackedNode` ∈ {RA-node, I-node, CA-node} — 0 valeur hors-enum.
- [ ] Distribution conforme §3 : **206 undercut/RA (~93%) + 13 undermine/I (~6%) + 3 rebut/CA (~1%)**.
- [ ] Les 9 nœuds opposant 889 = undermine/I-node (clean).
- [ ] Les 4 nœuds opposant 804 = **undermine/I-node (clean, raffiné PR-4)** — pks 137/139/140 (+1 à confirmer).
- [ ] Les 3 nœuds opposant 340 = **rebut/CA-node** — pks 53, 54 (Consequences, PR-6) + 165 (Danger, inféré mécanisme).
- [ ] Aucune cellule existante modifiée (colonnes additives uniquement).
- [ ] Validator #518 12/12 CLEAN + tests régression verts.
- [ ] `dotnet test` empirique : compteur conforme baseline (587 pass / 1 fail #133 / 5 skip post-#739, ou à jour).

**Fail-loud si** : un nœud avec scheme reçoit une valeur vide ; une valeur hors-enum apparaît ; la distribution
dévie de §3 de >5pts (signaler pour investigation, pas bloquer mécaniquement) ; le contrat po-2023 diverge
des valeurs attendues.

---

## 8. Coverage cluster vs reste du chantier

| Statut | Subfamilies | Méthode |
|---|---|---|
| **Validé par cluster doc** (PR #741-#749) | 4.1 Causalités, 6.1 Fidélité, 7.3 Respect, 5.1 Définitions, 3.1 Généralisations, 2.2 Équilibre émotionnel, 1.1 Argument fondé, 1.2 Prémisses fiables (**8 subfamilies / 7 familles / 12 schemes**) | décomposition I/RA/CA détaillée per-leaf |
| **À dériver mécaniquement** (ce plan) | 222 nœuds restants via règle §1 | apply script post-contrat |

La règle déterministe (§1) couvre **tous** les nœuds. Les clusters docs (PR #741-#749) restent la
**trace de raisonnement** pour 8 subfamilies pilotes ; le back-fill mécanique étend la même logique au reste.
Aucun nœud n'échappe à la règle (default undercut/RA couvre le cas général ; overrides 889/804 mine + 340 rebut
couvrent les exceptions). **3 attack-types AIF tous observés en cluster** (undermine PR-2/PR-4, undercut
dominant, rebut PR-6) — couverture sémantique complète.

**Reste à couvrir en cluster doc** (vers ~100% coverage scheme) : 1.3 Interprétation juste, 2.x Values (8 nœuds,
dernier scheme majeur), Danger (1 nœud, pk 165 — verdict rebut déjà inféré). Ces clusters confirmeront la règle
sur les derniers schemes ; aucun n'invalidéra les overrides (le mécanisme pilote l'override, pas le scheme).

---

## 9. Honnêteté de modélisation (discipline #677)

- **0 fabrication de tokens** : undercut/undermine/rebut, RA-node/I-node/CA-node = tous natifs AIF.
- **Rareté empirique documentée** : undermine (~6%) et rebut (~1%) ne sont PAS forcés à zéro par idéologie —
  ce sont les valeurs réelles dérivées du scan des 60 fallacies opposées + résolution per-case (PR-4 pour 804,
  PR-6 pour 340). Le set rebut {340} est **ouvert à extension** si une future fallacy révèle un mécanisme de
  contre-conclusion (analyse per-case du mécanisme AIF, pas keyword-scan).
- **804 résolu per-case** (PR-4, #745 mergée) : borderline → clean undermine/I-node via la triade 800/804/826.
  Plus de conservatisme par défaut.
- **pk 165 (Danger) documenté** : verdict rebut/CA inféré du mécanisme 340 stable (intrinsèque à la fallacy),
  pas observé per-case en cluster doc — à confirmer si cluster 6.2 est décomposé. La règle déterministe
  l'affecte fiable (340 → rebut quel que soit le scheme opposant).
- **Leçon keyword-scan documentée** (pas cachée) : le scan §2b v1 a manqué 340 car trop littéral ; la v2
  encode l'override rebut via mécanisme per-case, pas keyword-set figé.
- **Anti-drift** : ce plan ne spécifie PAS les valeurs canoniques finales — il mirror celles de po-2023 #498
  quand elles atterrissent. 0 write prod tant que.
- **Anti-duplication** : réutilise le grounding 12-col CLEAN (phase-2, #518) — ne redérive pas scheme/CQ/opposes.
