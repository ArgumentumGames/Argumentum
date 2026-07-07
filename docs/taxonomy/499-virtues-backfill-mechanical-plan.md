# #499 Virtues AIF — Plan mécanique back-fill 222-rows (Option A, HOLD prod)

> **Status : PLAN PRÊT, exécution GATED.** Ce doc spécifie la dérivation déterministe des 2 nouvelles colonnes
> `AIF_attackType` + `AIF_attackedNode` (schéma ratifié #707§4 Option A) pour les **222 nœuds** de la taxonomie
> Virtues, à partir du grounding relationnel 12-col existant (phase-2, CLEAN 222/222, validator #518).
>
> **⚠️ HOLD exécution** (ordre ai-01 2026-07-07) : **0 write** du CSV prod `Argumentum Virtues - Taxonomy.csv`
> jusqu'à ce que **po-2023 pose le contrat de colonnes côté Fallacies #498** (noms d'en-têtes exacts + valeurs
> canoniques) sur master ET que ai-01 le reviewe. **Anti-drift** : on mirror le format exact de po-2023, pas
> l'inverse. Ce plan est prêt à devenir 1 PR programmatique dès que le contrat est posé.
>
> **Option A ratifiée** (ai-01 sous délégation jsboige 2026-07-07) : colonnes = **attack-type prévenu** +
> **node maintenu**. Une vertu = bonne tenue d'un scheme → la fallacy tente de défaire un composant AIF
> que la vertu maintient.

---

## 1. Règle de dérivation déterministe (2 étapes)

La valeur de chaque nœud est dérivée de **(a) son scheme Walton** + **(b) le mécanisme de la/les fallacy(s) opposée(s)**.
La règle est déterministe et programmable. Elle généralise la méthode validée sur 3 clusters (PR #741/#742/#743,
9 feuilles, 14 fallacy-instances).

### Étape 1 — défaut = `undercut` / `RA-node`

Le défaut pour **tous** les nœuds est `undercut` / `RA-node`. Justification empirique (cf. §4) : sur les
60 fallacies distinctes opposées aux 222 nœuds, la quasi-totalité casse l'**inférence** (scheme non tenu) —
crédibilité, engagement, biais, lien causal, lien analogique, usage d'outil logique/probabiliste,
classification verbale mal appliquée — sans assertion d'une proposition factuellement fausse.

`RA-node` = le nœud d'inférence (Rule Application) est la cible : la vertu maintient l'inférence valide,
la fallacy la défait.

### Étape 2 — overrides (rares)

#### 2a. `undermine` / `I-node` — assertion d'une proposition connue-fausse ou fabriquée

Override **uniquement** si la fallacy opposée asserte une **proposition fausse** (le contenu factuel I-node
est faux), pas un usage d'outil ni une crédibilité. Discriminateur :

| La fallacy… | → attack-type |
|---|---|
| asserte un fait qu'elle sait faux, ou fabrique une définition/donnée (la **proposition elle-même** est fausse) | **undermine / I-node** |
| casse le raisonnement (crédibilité source, biais, saut causal, outil mal utilisé, engagement court-circuité) | undercut / RA-node (défaut) |

**Set undermine validé** (scan des 60 fallacies opposées, keywords `savez faux` / `inventez une déf` /
`mensonge`) :

| Fallacy | desc_fr | Nodes l'opposant | Verdict |
|---|---|---|---|
| **889 Mensonge** | « Vous affirmez quelque chose que vous savez faux » | **9 nœuds** | **undermine / I-node** (clean — la proposition I-node est factuellement fausse) |
| **804 Acception arbitraire** | « Vous inventez une définition sur mesure pour vos termes » | 4 nœuds | **BORDERLINE** — la définition-fabrication = prémisse fausse (undermine/I) MAIS peut se lire comme classification-inference cassée (undercut/RA). **Défaut conservateur = undercut/RA** sauf override per-case documenté. |

**Faux positifs écartés** (tool-misuse / credibility → undercut/RA, PAS undermine) :
- 644 Probabilités faussées (« fausse votre RAISONNEMENT ») → undercut/RA
- 727/735/750 Erreurs logiques (propositionnel/quantification/modalité) → undercut/RA
- 1287 Pseudo-explication (« feignant d'expliquer ») → undercut/RA
- 942 Fausse attribution (source fabriquée = crédibilité testimony → undercut/RA, cf. PR-2)
- 974 Exigence renforcée (moving-goalposts = engagement/burden → undercut/RA)

#### 2b. `rebut` / `CA-node` — contre-conclusion indépendante

Override si la fallacy présente une **contre-conclusion indépendante** soutenue par son propre argument
(CA-node ↔ conclusion I-node adverse).

**Set rebut : VIDE.** Scan des 60 fallacies opposées (keywords contre-argument / position adverse / conclusion opposée / réfut) = **0 occurrence**. Corrobore le finding rebut-rarity (PR #743 §3) à pleine échelle.
PK 1398 *Attaque personnelle* est défini dans la taxonomie elle-même comme *« au lieu de réfuter »* = négation
explicite du rebuttal.

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
| Argument from Verbal Classification | 21 | undercut/RA (sauf oppose-804 → borderline) |
| Argument from Cause to Effect | 11 | undercut/RA |
| Argument from Witness Testimony | 10 | undercut/RA |
| Argument from Position to Know | 8 | undercut/RA |
| Argument from Values | 8 | undercut/RA |
| Argument from Analogy | 7 | undercut/RA |
| Argument from Expert Opinion | 6 | undercut/RA |
| Argument from Example | 4 | undercut/RA |
| Argument from Consequences | 3 | undercut/RA |
| Argument from Danger | 1 | undercut/RA |

**Note** : le scheme ne pilote PAS l'attack-type (tous défaut = undercut/RA). C'est le **mécanisme de la fallacy
opposée** qui pilote l'override undermine. Le scheme reste load-bearing pour la colonne existante `AIF_skosDirectRef`
et pour la CQ (`AIF_skosMappingType`), pas pour l'attack-type.

---

## 3. Distribution de sortie attendue (post-back-fill)

| attack-type | nœuds | % | Source |
|---|---|---|---|
| **undercut / RA-node** | **~209-213** | ~94-96% | défaut (reste après overrides) |
| **undermine / I-node** | **9** (889) + 0-4 (804 borderline) | ~4-6% | override §2a |
| **rebut / CA-node** | **0** | 0% | set rebut vide §2b |

**Total** : 222 nœuds. L'estimation exacte dépend du verdict borderline 804 (4 nœuds). Si 804 → undermine :
13 nœuds undermine / 209 undercut. Si 804 → undercut (défaut conservateur) : 9 undermine / 213 undercut.

**Implication tooling** (load-bearing pour ai-01/jsboige) : la colonne `AIF_attackType` sera
**undercut-dominée à ~95%**. Ne pas sur-investir la couverture rebut/undermine dans le validateur —
leur rareté est la structure réelle du domaine, pas un gap de modélisation.

---

## 4. Preuve de généralisation (3 clusters validés → règle pleine échelle)

La règle ci-dessus est la **généralisation déterministe** de la méthode validée sur 3 clusters empiriques :

| Cluster (PR) | Subfamily | Scheme(s) | Distribution observée | Règle dérivée |
|---|---|---|---|---|
| #741 PR-1 | 4.1 Causalités | Cause to Effect | 6/6 undercut/RA | défaut tient |
| #742 PR-2 | 6.1 Fidélité aux faits | Witness Testimony, Bias | 1 undermine (889) + 2 undercut | override undermine pour 889 |
| #743 PR-3 | 7.3 Respect | Argument from Commitment | 6/6 undercut/RA | défaut tient |

**Cumul** : 14 undercut + 1 undermine + 0 rebut sur 3 familles / 9 feuilles. Le scan pleine échelle (§2a/§2b)
étend ce finding à 60 fallacies : 2 undermine (889, 804-borderline) + 0 rebut. **La règle est stable.**

---

## 5. Spécification programmatique (prête à exécuter post-contrat)

Script `tools/499-virtues-backfill-apply.py` (à créer dès contrat #498 posé). Pseudo-code :

```python
# GATING : 0 write tant que contrat_colonnes_po2023 == False
import csv

UNDERMINE_FALLACIES = {"889"}                     # clean undermine (proposition factuellement fausse)
UNDERMINE_BORDERLINE = {"804"}                    # définition-fabrication, verdict conservateur undercut
                                                          # sauf override per-case documenté
VIRTUES_CSV = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv"
# CONTRAT po-2023 (TBD — valeurs canoniques exactes à mirror) :
ATTACK_HEADER   = "AIF_attackType"      # valeurs: "undercut" | "undermine" | "rebut"  (po-2023 à confirmer)
NODE_HEADER     = "AIF_attackedNode"    # valeurs: "RA-node" | "I-node" | "CA-node"   (po-2023 à confirmer)

rows = read_csv(VIRTUES_CSV)
for row in rows:
    if not row["AIF_skosDirectRef"].strip():
        continue                                  # nœud racine sans scheme → skip (ou valeur vide)
    opposed = {p.strip() for p in row["crossLink_Opposes"].split(";") if p.strip()}
    # Étape 1 — défaut
    attack_type, node = "undercut", "RA-node"
    # Étape 2a — override undermine
    if opposed & UNDERMINE_FALLACIES:
        attack_type, node = "undermine", "I-node"
    elif opposed & UNDERMINE_BORDERLINE:
        pass  # défaut conservateur undercut/RA ; override per-case à documenter si jugé undermine
    # Étape 2b — override rebut : set vide, pas de cas
    row[ATTACK_HEADER] = attack_type
    row[NODE_HEADER]   = node

write_csv(VIRTUES_CSV, rows)                      # GATED — cf. §6
```

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
- [ ] Distribution conforme §3 (undercut ~95%, undermine ~4-6%, rebut 0%).
- [ ] Les 9 nœuds opposant 889 = undermine/I-node (clean).
- [ ] Les 4 nœuds opposant 804 = verdict borderline documenté (undercut défaut ou undermine override per-case).
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
| **Validé par cluster doc** (PR #741/#742/#743) | 4.1 Causalités, 6.1 Fidélité, 7.3 Respect (3/14+ scheme-families) | décomposition I/RA/CA détaillée per-leaf |
| **À dériver mécaniquement** (ce plan) | 222 nœuds restants via règle §1 | apply script post-contrat |

La règle déterministe (§1) couvre **tous** les nœuds. Les clusters docs (PR #741/#742/#743) restent la
**trace de raisonnement** pour 3 subfamilies pilotes ; le back-fill mécanique étend la même logique au reste.
Aucun nœud n'échappe à la règle (default undercut/RA couvre le cas général ; overrides 889/804 couvrent
les exceptions).

---

## 9. Honnêteté de modélisation (discipline #677)

- **0 fabrication de tokens** : undercut/undermine/rebut, RA-node/I-node/CA-node = tous natifs AIF.
- **Rareté empirique documentée** : undermine (~4-6%) et rebut (0%) ne sont PAS forcés à zéro par idéologie —
  ce sont les valeurs réelles dérivées du scan des 60 fallacies opposées. Si un nœud future révèle un rebut
  propre, la règle §2b l'accueille (le set rebut est vide aujourd'hui, ouvert à l'extension).
- **Borderline 804 documenté** : verdict conservateur (undercut/RA) avec override per-case possible, pas
  forcé dans une catégorie pour coller à la distribution attendue.
- **Anti-drift** : ce plan ne spécifie PAS les valeurs canoniques finales — il mirror celles de po-2023 #498
  quand elles atterrissent. 0 write prod tant que.
- **Anti-duplication** : réutilise le grounding 12-col CLEAN (phase-2, #518) — ne redérive pas scheme/CQ/opposes.
