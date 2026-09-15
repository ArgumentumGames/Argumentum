# #499 Virtues AIF — Cluster PR-10 « Interprétation juste » (famille 1.3)

> **Schéma ratifié** : #707§4 Option (a) + **Option A « attaque résistée »** (ai-01 sous délégation jsboige).
> **Méthode** : décomposition I/RA/CA par feuille (cluster anchor + feuilles), fail-loud si pas de CQ natif.
> **Discipline** : docs-only, 0 fabrication de tokens (#677), couche **additive** (ne redérive pas le relationnel phase-2).
> Suit PR #741-#750 (9 clusters + plan back-fill v2 ; #741-#749 merged `01721c39`, #750-#751 OPEN).

---

## Scope du cluster — famille 1 « Argument pertinent » / subfamily 1.3 « Interprétation juste »

| pk | path | depth | titre | scheme Walton | opposes (Fallacy PK) |
|----|------|-------|-------|---------------|----------------------|
| **28** | 1.3 | 2 | **Interprétation juste** *(anchor)* | Argument from Sign | 953 ; 153 |
| 29 | 1.3.1 | 3 | Complexité adaptée | Argument from Verbal Classification | 1345 ; 33 |
| 30 | 1.3.2 | 3 | Interprétation non partisane | Argument from Bias | 953 ; 1242 |
| 31 | 1.3.3 | 3 | Représentation parcimonieuse | Argument from Cause to Effect | 165 ; 1345 |
| 32 | 1.3.3.1 | 4 | Rasoir d'Ockham | Argument from Cause to Effect | 165 ; 1287 |
| 33 | 1.3.3.2 | 4 | Rasoir de Hanlon | Argument from Cause to Effect | 707 ; 1371 |

**6 nœuds** (anchor 28 + feuilles 29/30/31 + sous-feuilles 32/33), **4 schemes** :
Argument from Sign (28) + Verbal Classification (29) + Bias (30) + **Cause to Effect (31/32/33)**.

> **⚠️ Disambiguation load-bearing** : la Fallacy **pk 165 « Manque de parcimonie »** (Insuffisance / Surinterprétation)
> **n'est PAS** la Virtue pk 165 « Identifier les points de vigilance » (Argument from Danger, famille 6.2, qui oppose
> la Fallacy 340 et est inférée `rebut`/CA dans le plan #750). Les espaces de PK Fallacies et Virtues sont **distincts**.
> Ici, 165 désigne toujours la **Fallacy** « Manque de parcimonie ».

---

## 🔑 KEY CONTRIBUTION 1 — complète la famille 1 (3/3 subfamilies) + valide Cause to Effect sur la sous-espèce « parcimonie / rasoirs »

La famille 1 « Argument pertinent » est désormais couverte sur **ses 3 subfamilies** (1.1 Argument fondé ✅PR-7,
1.2 Prémisses fiables ✅PR-8, **1.3 Interprétation juste ✅PR-10**). 33 nœuds de la famille 1 sont désormais
décomposés I/RA/CA.

De plus, **PR-10 valide Argument from Cause to Effect sur la sous-espèce « parcimonie causale »** (rasoirs
d'Ockham et de Hanlon) — un sous-mécanisme de Cause to Effect non encore observé :
- PR-1 (4.1 Causalités) couvrait Cause to Effect sur le **lien causal** de base (pk 31 Représentation parcimonieuse
  + 32 Rasoir d'Ockham + 33 Rasoir de Hanlon couvrent la **parcimonie de l'explication causale**).
- Les fallacies opposées aux rasoirs (165 Manque de parcimonie, 1287 Pseudo-explication, 707 Inversion de causalité)
  cassent toutes l'**inférence causale parcimonieuse** → `undercut` / `RA-node`.

**Confirme** (avec PR-1, PR-5, PR-7, PR-8, PR-9) : la règle déterministe du plan #750 tient sur **10 clusters /
13 schemes**. Aucune feuille de 1.3 n'oppose 889/804 (mine) ni 340 (rebut) → pur défaut mécanique `undercut`/`RA-node`.

## 🔑 KEY CONTRIBUTION 2 — 1371 Sophisme génétique = parallèle de 1398 Attaque personnelle (fallacy relationnelle = détournement, renforce rebut-rarity)

PR-9 a documenté **1398 Attaque personnelle** comme **négation native du rebuttal** (*« au lieu de réfuter »*) :
preuve native que les fallacies relationnelles sont des **détournements** (undercut/RA), pas des réfutations (rebut/CA).

**PR-10 documente le parallèle structurel sur 1371 Sophisme génétique** :
- *« Vous rejetez une idée en critiquant son origine plutôt que son contenu »*.
- Comme 1398, la fallacy **rejette** la conclusion — mais **sans présenter de contre-conclusion indépendante** :
  elle détourne l'attention de l'argument vers **l'origine** (attaque de crédibilité/génétique).
- → **`undercut` / `RA-node`, explicitement PAS rebut.**

**Double confirmation native** (1398 sur la personne + 1371 sur l'origine) : les fallacies relationnelles (ad hominem,
génétique) **rejettent sans réfuter** → la définition taxonomy elle-même encode que le rejet passe par le **détournement**
(sous-cut), pas la réfutation (rebut). Renforce le finding rebut-rarity avec une 2e preuve native (texte taxonomy),
en plus de l'empirique (scan/cluster).

---

## Distribution cluster 1.3 — 12/12 `undercut`/`RA-node` (uniforme), 0 mine, 0 rebut

Les fallacies d'interprétation (sélection biaisée 953, mauvaises raisons 153, complication 1345, justification
triviale 33, biais théorique 1242, manque de parcimonie 165, pseudo-explication 1287, inversion causale 707,
sophisme génétique 1371) sont toutes des **défaillances d'inférence** : aucune n'asserte une prémisse fabriquée,
aucune ne présente de contre-conclusion (1371 le nie structurellement, comme 1398).

| Virtue pk | Fallacy | attackType | attackedNode |
|-----------|---------|------------|--------------|
| 28 (anchor) | 953 Attention sélective | undercut | RA-node |
| 28 (anchor) | 153 Argument des mauvaises raisons | undercut | RA-node |
| 29 | 1345 Complication exagérée | undercut | RA-node |
| 29 | 33 Justification triviale | undercut | RA-node |
| 30 | 953 Attention sélective | undercut | RA-node |
| 30 | 1242 Biais théoriques | undercut | RA-node |
| 31 | 165 Manque de parcimonie | undercut | RA-node |
| 31 | 1345 Complication exagérée | undercut | RA-node |
| 32 | 165 Manque de parcimonie | undercut | RA-node |
| 32 | 1287 Pseudo-explication | undercut | RA-node |
| 33 | 707 Inversion de causalité | undercut | RA-node |
| 33 | 1371 Sophisme génétique | undercut | RA-node |

**Classification honnête par mécanisme** (discriminator #750 §2a) :
- **953** (cherry-picking) = sélection biaisée → undercut/RA (confirmé PR-7, double-confirmé ici).
- **1242** (biais théorique) = cadre déformé → undercut/RA (confirmé PR-7).
- **1287** (pseudo-explication) = feinte d'explication → defeat de l'inférence (tool-misuse) → undercut/RA (déjà
  noté #750 §2a comme faux-positif mine).
- **33** (justification triviale) = preuve insuffisante → Sign Application failure (comme 3/1297 PR-7).
- **165** (manque de parcimonie), **1345** (complication) = sur-complexification → defeat de l'inférence parcimonieuse.
- **707** (inversion causale) = relation causale inversée → Cause to Effect Application failure.
- **153** (mauvaises raisons), **1371** (génétique) = rejet via attaque du raisonnement/origine (credibility) →
  defeat de l'inférence, PAS contre-conclusion.

---

## Finding cumulatif rebut-rarity (10 clusters, load-bearing)

| Cluster | Subfamily | Famille | Schemes | mine | undercut | rebut |
|---------|-----------|---------|---------|------|----------|-------|
| PR-1 | 4.1 | Raisonnement | Cause to Effect | 0 | 6 | 0 |
| PR-2 | 6.1 | Honnêteté | Witness/Bias | 1 | 2 | 0 |
| PR-3 | 7.3 | Échange | Commitment | 0 | 6 | 0 |
| PR-4 | 5.1 | Langage | Verbal Cl./Expert | 3 | 7 | 0 |
| PR-5 | 3.1 | Rigueur math | Example/Rule/Analogy | 0 | 6 | 0 |
| PR-6 | 2.2 | Présentation | Bias/**Consequences** | 0 | 4 | **2** |
| PR-7 | 1.1 | Argument pertinent | Sign/Bias | 0 | 6 | 0 |
| PR-8 | 1.2 | Argument pertinent | Position to Know/Bias | 0 | 6 | 0 |
| PR-9 | 2.1.2 | Présentation | Values | 0 | 8 | 0 |
| **PR-10** | **1.3** | **Argument pertinent** | **Sign/Verbal Cl./Bias/Cause to Effect** | **0** | **12** | **0** |
| **Cumul** | **10 subfamilies** | **7 familles / 13 schemes** | | **4** | **63** | **2** |

**rebut/CA-node = 2 sur 10 familles / 13 scheme-types / ~69 fallacy-instances.** rebut **reste absent** sur la
famille 1 complète (1.1 + 1.2 + 1.3) — les fallacies d'interprétation ne *nient* pas la conclusion, elles
*defeated* l'inférence (sélection biaisée, sur-complexification, inversion causale, rejet génétique). rebut reste
**localisé + prédictible** : Consequences (PK 340) only. L'override rebut {340} du plan #750 tient. **1371 =
2e preuve native** que les fallacies relationnelles = détournements (undercut), pas réfutations (rebut).

---

## Coverage schemes — 13/14 maintenus (tous les schemes majeurs validés), **famille 1 COMPLÈTE**

Rule (50) ✅PR-5, Commitment (40) ✅PR-3, Bias (27) ✅PR-2, Sign (26) ✅PR-7, Verbal Classification (21) ✅PR-4,
Cause to Effect (11) ✅PR-1+**PR-10 (parcimonie)**, Witness Testimony (10) ✅PR-2, Position to Know (8) ✅PR-8,
Values ✅PR-9 = **~199 nœuds (~90%)**. Reste : **Danger (1 nœud, pk 165 — verdict rebut/CA déjà inféré du
mécanisme 340 dans plan #750)**.

**Jalon** : **famille 1 « Argument pertinent » complète** (3/3 subfamilies : 1.1 + 1.2 + 1.3, 33 nœuds décomposés).
PR-10 n'ajoute pas de NOUVEAU scheme (Sign/Verbal Cl./Bias/Cause to Effect déjà validés) — il **étend Cause to
Effect à la sous-espèce parcimonie** et confirme le défaut sur la dernière subfamily non couverte de la famille 1.

---

## Discipline

- ✅ **Read body before action** : structure famille 1 énumérée (33 nœuds, 3 subfamilies), annotation phase-2
  `argument-annotations.csv` lue (6 nœuds 1.3 schématisés : scheme + CQ + opposes + justification), 9 descriptions
  Fallacies lues (953/153/1345/33/1242/165/1287/707/1371) AVANT dérivation ; anti-duplication grep confirmé
  (les 9 docs matchant `RA-node/attackType` dans `docs/taxonomy/499-*.md` = mes propres clusters merged + plan ;
  `argument.md` phase-2 = 0 token I/RA/CA → couche additive).
- ✅ **0 fabrication** (#677) : Sign/Verbal Cl./Bias/Cause to Effect/undercut/RA-node = tous natifs AIF/Walton.
- ✅ **Disambiguation Fallacy 165 ≠ Virtue 165** documentée load-bearing (espaces de PK distincts).
- ✅ **1371 documenté comme parallèle natif de 1398** — 2e preuve native de rebut-rarity (rejet sans réfutation =
  détournement), pas forçage idéologique du set rebut.
- ✅ **953/1242/1287 classés honnêtement** undercut (sélection/cadre/tool-misuse = traitement inférence), cohérents
  PR-7 + #750 §2a.
- ✅ **Cause to Effect parcimonie = RA-node paradigmatique** documenté (rasoirs = Cause to Effect Application
  parcimonieuse ; 165/1287/707 = failure archetypes), pas forcé.
- ✅ **0 modif code**, 0 write `Cards/`, 0 régén, 0 CSV/DB/OWL. Docs-only (+160 lignes).
- ❌ Does NOT touch #674/#666/#596 (HOLD). Back-fill exécution **toujours GATED** (po-2023 contrat colonnes #498
  non posé, stall détecté).

---

## Test impact

None — docs-only.
