# 2026-07-10 — #498 AIF chantier : coverage status & trajectoire (refresh code=truth)

**Objet** : vue d'ensemble à jour du chantier #498 (« exceptions défaisables Walton/AIF »)
après la sérialisation des 2 colonnes d'attaque (#753/#760) et 12 cluster-doc PRs. Corrige le
snapshot précédent (2026-07-05, master `70bd1605`), désormais périmé sur la structure du chantier.
Synthèse **read-only, dérivée code=truth** — aucun write prod dans ce document (proposition gated).

**Repo reference** : master `7796c127`. Issue : #498 (reformulée, GO jsboige 2026-06-17 vérifié).
Schéma I/RA/CA **ratifié + sérialisé** (#707 Option (a) → colonnes créées #753, remplies #760).

---

## TL;DR — la couverture a **deux couches**, jamais réconciliées

Le chiffre « couverture » dépend de **quelle couche AIF** on mesure. Code=truth master `7796c127` :

| Métrique | Rows | % de 1408 | Sérialisée par |
|----------|-----:|----------:|----------------|
| **Couche attaque** (`AIF_attackType`+`AIF_attackedNode`) | **93** | 6.6% | #753 (back-fill §7) + #760 (scaleup phase 1-3) |
| **Couche skos** (`AIF_skosDirectRef`/`ExceptionRef`/`MappingType`) | **70** | 5.0% | pilote AIF pré-chantier (baseline) |
| **Union** (au moins un signal AIF) | **145** | 10.3% | — |
| **Pleinement modélisée** (les 2 couches) | **18** | **1.3%** | intersection |

- Les deux couches **ne se recouvrent qu'à 18 rows**. Il en résulte **52 rows skos-only** (tokens
  natifs vérifiés, **mais pas de colonne d'attaque**) et **75 rows attack-only** (attaque typée,
  **mais pas de token skos**). C'est un **écart de back-fill bidirectionnel**, pas une régression.
- Le snapshot précédent « 70/1408 (5.0%) » **n'était pas faux** : il mesurait la couche **skos**,
  qui précède les colonnes d'attaque. « 93 » mesure la couche **attaque**. Deux compteurs distincts.
- **La vraie complétude AIF (les 2 couches) = 18 leaves (1.3%)**. Le reste est mono-couche.
- **Conséquence stratégique** : les deux plus hauts ROI ne sont **pas** de nouveaux cluster docs
  ex-nihilo, mais **réconcilier les couches** — voir §5.

---

## 1. Coverage code=truth (master `7796c127`)

### Couche attaque — per family (93 rows)

| Family | Attack-typed | Total | % |
|--------|-------------:|------:|---:|
| **Misleading language** | **46** | 87 | **52.9%** (focus historique) |
| Obstruction | 9 | 126 | 7.1% |
| Mathematical error | 6 | 102 | 5.9% |
| Faulty logics | 6 | 102 | 5.9% |
| Insufficiency | 6 | 174 | 3.4% |
| Influence | 9 | 378 | 2.4% |
| Cheating | 9 | 390 | 2.3% |
| *(empty family name)* | 2 | 48 | 4.2% ⚠ |
| Fallacy | 0 | 1 | 0.0% |
| **TOTAL** | **93** | **1408** | **6.6%** |

> ⚠ **Data-quality note (inchangé)** : **48 rows ont un `Family` vide**. Probables racines
> depth-1/2 ou leaves mal classées — item d'hygiène de données séparé, hors-scope AIF jusqu'à
> classification. Flaggé jsboige.

### Distribution attack-type / attacked-node (93 mapped)

| attack-type | count | → attacked-node (map déterministe #707§4 (a)) |
|-------------|------:|-----------------------------------------------|
| undercut | 61 | RA-node (61) |
| undermine | 29 | I-node (29) |
| rebut | 3 | CA-node (3) |

Map déterministe **parfaitement respectée** (61/29/3 ↔ RA/I/CA). Les 3 rebut (1282 Relativisme
abusif, 1313 Évasion, 1361 Procès en incohérence) sont les **premiers rebut de la taxonomie
Fallacies**, tous famille Obstruction, tous → rebut/CA-node (contre-conclusion structurelle).

---

## 2. Les deux couches AIF — l'état de réconciliation (finding central)

La couche skos (I/RA/CA détaillé : quel CQ Walton, quel scheme) et la couche attaque
(undermine/undercut/rebut + composant attaqué) ont été sérialisées par **deux efforts distincts**
et **jamais croisées**. Cross-tab code=truth (1408 rows) :

| | skos ✓ | skos ✗ |
|---|---:|---:|
| **attaque ✓** | **18** (pleinement modélisées) | **75** (attack-only) |
| **attaque ✗** | **52** (skos-only) | 1263 (non mappées) |

### 2a. 52 rows skos-only — back-fill attaque (ROI le + haut, 0 risque fabrication)

Ce sont les rows du **baseline AIF pré-chantier** : elles portent des **tokens natifs vérifiés**
(`AIF_skosDirectRef` = `*_Conflict`/`*_Inference`, `AIF_skosExceptionRef` = scheme,
`AIF_skosMappingType` = broad/close/narrowMatch) **mais aucune colonne d'attaque**. Half-done.

| Family | skos-only rows |
|--------|---------------:|
| Cheating | 13 |
| Insufficiency | 10 |
| Misleading language | 9 |
| Faulty logics | 8 |
| Mathematical error | 6 |
| Influence | 3 |
| Obstruction | 3 |
| **total** | **52** |

Dériver `AIF_attackType`/`AIF_attackedNode` est une **étape de modélisation bornée par row** : le
`DirectRef` révèle souvent le composant attaqué (un `*_Conflict` visant le scheme → undercut/RA-node ;
visant une prémisse → undermine/I-node ; visant la conclusion → rebut/CA-node). **Aucune fabrication
de token** (les tokens existent déjà, vérifiés). C'est le candidat de réconciliation le plus tractable.

### 2b. 75 rows attack-only — deepen skos (risque #677, fail-loud)

Attaque typée (#760 scaleup + leaves de cluster docs), **pas de token skos en prod**.

| Family | attack-only rows |
|--------|-----------------:|
| Misleading language | 44 |
| Cheating | 8 |
| Insufficiency | 5 |
| Mathematical error | 5 |
| Influence | 4 |
| Obstruction | 4 |
| Faulty logics | 3 |
| *(empty)* | 2 |
| **total** | **75** |

Nuance importante : les **44 Misleading language** sont les leaves des cluster docs mergés
(PR-5…PR-12 : Equivoque, Amphibologie, Narrative ambiguity, définitions). Leur skos est **déjà
proposé + ratifié dans les docs**, en attente de **sérialisation profonde** (write gated distinct).
Les **31 non-ML** (scaleup #760) nécessitent une **modélisation skos** avec discipline #677 (fail-loud
si aucun CQ/scheme natif — jamais fabriquer). ROI moyen, risque plus élevé que 2a.

---

## 3. Chantier progress — ledger des PR mergées (code=truth git log)

### Sérialisation (writes prod, gated byte-check ai-01)

| PR | Commit | Effet |
|----|--------|-------|
| **#753** | `d4fde74d` | Crée 2 colonnes + sérialise §7 I/RA/CA (attack-type) des cluster docs → 46 attack-typed |
| **#760** | `5551000b` | Sérialise scaleup phase 1-3 (47 leaves, MODE 2 override) → 46→93 |
| **#763** | `053257c7` | Ontologie : câblage AIF attack (OWL consomme les colonnes) + crosslinks 59% |
| #755 (miroir) | — | #499 Virtues AIF mirror back-fill (222 rows, chantier jumeau) |

### Cluster docs (propositions, gated — aucun write prod, modélisation I/RA/CA détaillée)

| PR | Cluster (Misleading language sauf noté) | Leaves |
|----|------------------------------------------|-------:|
| #699/#701/#703 | Fallacious comparison (analogy/faulty/association) | 11 |
| #705/#708 | Vague + Inconsistent definition | 10 |
| #711 | Amphibologie (borrow-root) | 8 |
| #713/#714 | Narrative ambiguity (insinuation + deception) | 10 |
| #717/#718/#720 | Equivoque (lexical / residual / reification) | 11 |
| #723 | Acception arbitraire (breadth-defects) | 3 |
| #707/#709/#716 | coverage synthesis + audits adversariaux | — |

**En attente (gated review ai-01, NON merged)** :
- **#766** — Relativisme abusif (Obstruction, **1er cluster rebut dédié**). En code=truth, Relativisme
  abusif affiche encore **4 unmapped** ci-dessous car #766 n'est pas mergé (le doc = proposition).

---

## 4. Méthode (ratifiée ai-01 « rigoureuse, 0 fabrication » + jsboige GO)

### Cluster selection
1. **Unit of work = 1 sub-sub** (`Soussousfamille`), code=truth depuis la CSV.
2. **Préférer sub-subs avec anchor in-sub-sub mappé** (cluster shape le + propre). Sinon
   **borrow-root** (emprunter l'anchor d2/d3 parent).
3. **Taille cible** : 2-5 leaves/PR (au-delà, découper par depth ou mécanisme).

### Modeling (3 shapes)
- **Exception pattern** : scheme légitime en `ExceptionRef`, CQ violé en `DirectRef`. La fallacy
  *défait* un scheme légitime.
- **Direct-conflict pattern** : scheme en `DirectRef` seul. La fallacy *est* un scheme défectueux.
- **Rebut cluster = attack-columns-only** (établi #766) : un rebut relativiste nu est un conflit
  *structurel* de contre-conclusion, **pas** un conflit de CQ Walton. Sérialisé
  `attackType=rebut` + `attackedNode=CA-node` seuls ; `DirectRef` vide ; `ExceptionRef` = le
  scheme natif rebuté **ssi** un s'applique honnêtement. Fail-loud sur le token CA (pas de
  `*Conflict` fabriqué).

### Discipline vocabulaire natif (#677)
- Tokens natifs **en usage** (code=truth) : **42 `DirectRef` distincts** + **50 `ExceptionRef`
  distincts** sur les 70 rows skos-modélisées. Restreint au vocabulaire AIF/Walton confirmé.
- **FAIL-LOUD** si aucun token natif ne capture le défaiteur — **jamais fabriquer** de `*_Conflict`
  ou `*_Inference`. Documenter le gap dans `AIF_skosOther`.
- Validation programmatique sur chaque PR (grep tokens backticked vs whitelist native).

---

## 5. Trajectoire & priorisation (refresh)

Le finding §2 change la priorisation : **réconcilier les couches** avant d'ouvrir de nouveaux
clusters ex-nihilo. Ordre ROI décroissant (toutes exécutions = **writes prod gated**, GO requis) :

### Priorité 1 — Réconciliation-A : back-fill attaque des 52 skos-only
- **52 rows**, tokens natifs déjà vérifiés → **0 risque fabrication**, modélisation bornée par row.
- Découpable par family (Cheating 13, Insufficiency 10, ML 9, Faulty logics 8, Math 6…).
- Passe la complétude « 2 couches » de 18 → potentiellement 70. Le meilleur ratio effort/couverture.

### Priorité 2 — Réconciliation-B : deep-serialize skos des 44 ML attack-only
- skos **déjà proposé + ratifié** dans les cluster docs mergés (PR-5…12) → sérialisation d'un
  contenu vetté, pas de re-modélisation.

### Priorité 3 — Nouveaux cluster docs (sub-subs à anchor mappé + leaves non-mappées)
Reste **53 sub-subs** candidats (anchor attack-typé + leaves non mappées). Les petits propres
d'abord ; les gros (Influence/Cheating) à découper par mécanisme.

**Petits clusters propres restants (2-6 unmapped + anchor)** :

| unmapped | family / sub-sub | anchor (attack) | note |
|---------:|------------------|-----------------|------|
| 6 | Obstruction / Complication exagérée | 1345 undercut | **prochain propre** |
| 5 | Mathematical error / Opération inappropriée | 690 undercut | propre |
| 4 | Obstruction / Relativisme abusif | 1282 rebut | **couvert par #766 (pending)** |
| 2 | Misleading language / Amphibologie | undercut ×6 | résiduel (cluster doc #711) |
| 2 | Misleading language / Définition inconsistante | undercut ×5 | résiduel (cluster doc #708) |
| 2 | Misleading language / Fausse analogie | undercut/undermine | résiduel FAIL-LOUD (840) |
| 2 | Misleading language / Comparaison abusive | undercut/undermine | résiduel FAIL-LOUD (834/835/837) |

**Gros clusters (à découper par mécanisme)** — top par volume non-mappé :
Biais naturels (149), Influence non verbale (83), Biais culturels (67), Jeu de pouvoir (57),
Conditionnement (56), Poésie (51), Mensonge (48), Langage persuasif (39), Biais théoriques (37),
**Evasion (31, anchor rebut 1313)**, **Sophisme génétique (26, anchor undermine 1371)**…

### Nucléi Obstruction rebut (contexte #766)
Après Relativisme abusif (#766), les autres rebut Obstruction : **Évasion** (1313) et **Procès en
inconsistance** (1361, 9 unmapped) — même shape attack-columns-only, contexte déjà établi.

---

## 6. Recommandation

1. **Réconciliation d'abord** (§5 P1/P2) : le finding §2 montre que 52+44 rows sont *half-done*.
   Les compléter porte la complétude 2-couches de 1.3% → ~7-8% pour un effort borné et vetté,
   **avant** d'ouvrir de nouveaux fronts.
2. **Nouveaux clusters** en parallèle sur les petits propres restants (Complication exagérée,
   Opération inappropriée) + nucléi Obstruction rebut.
3. Toute exécution reste **write prod gated** (GO jsboige/ai-01, byte-check comme #753/#760).

---

## 7. Gate boundaries (HARD — synthèse read-only)

- ❌ No prod CSV write, no DB write, no OWL regen dans **ce** document.
- ✅ Tous les chiffres dérivés **code=truth** (CSV scan master `7796c127`, 2026-07-10) + git log
  (PR mergées) + dashboard (PRs pending).
- ✅ Aucun token AIF fabriqué (référence uniquement les tokens natifs en usage).
- ✅ VERIFIED : couches 93/70/18/145, distribution 61/29/3, gap-sets 52/75, per-family — tout scan.
  RAPPORTÉ : effets des PR (git log). PENDING : #766 (non mergé).

Relates : #498 (chantier), #753/#760/#763 (sérialisation), #766 (Relativisme abusif rebut, pending),
#707 (schéma ratifié), #709/#716 (audits), #677 (0 fabrication), #499 (miroir Virtues), #141 (OWL AIF),
#133/#130 (OWL), #192 (terminology).
