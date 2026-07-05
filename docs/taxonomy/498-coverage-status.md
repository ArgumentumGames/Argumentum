# 2026-07-05 — #498 AIF chantier : coverage status & méthode (synthèse pour ratification jsboige)

**Objet** : vue d'ensemble du chantier #498 (« exceptions défaisables Walton/AIF ») après 4 PR,
pour ratification jsboige de la **méthode** + décision sur le **schéma CSV I/RA/CA** (sérialisation
du DoD enrichi). Synthèse read-only — aucun write prod.

**Repo reference**: master `70bd1605`. Issue: #498 (reformulated, GO jsboige 2026-06-17 verified).
DoD enrichi (I-node/RA-node/CA-node) : dernier comment jsboige sur #498.

---

## TL;DR

- **Coverage master (code=truth)** : **70/1408 fallacies mappés (5.0%)** sur 8 families.
- **Chantier #498 (4 PR)** : 14 leaves modélisés, **+11 fully-modeled (DoD)** → 70→81 projected
  (si toutes merged). 1 subfamily fully addressed (Fallacious comparison), 1 ouverte (Inexact
  definition).
- **Méthode** : grappe-à-grappe (1 PR = 1 sub-sub cohérent), 2 cluster shapes (in-sub-sub anchor /
  borrowed-root anchor), 2 patterns (exception / direct-conflict), fail-loud si pas de token AIF
  natif. **0 fabrication** (validé programmatisation sur chaque PR).
- **DoD enrichi (I/RA/CA)** : décomposition enregistrée dans §7 de chaque PR, **pas encore
  sérialisée** dans le CSV → **décision jsboige** requise sur nouvelles colonnes
  (`AIF_attackType`, `AIF_attackedNode`).
- **Trajectoire** : 1408 leaves, ~87 sub-subs. À ~3-4 leaves/PR, chantier long. Recommandation :
  ratifier méthode + schéma I/RA/CA maintenant, puis exécuter en autonomie.

---

## 1. Coverage code=truth (master `70bd1605`)

### Per family

| Family | Mapped | Total | % |
|--------|-------:|------:|---:|
| Cheating | 14 | 390 | 3.6% |
| Faulty logics | 11 | 102 | 10.8% |
| Influence | 8 | 378 | 2.1% |
| Insufficiency | 11 | 174 | 6.3% |
| Mathematical error | 7 | 102 | 6.9% |
| **Misleading language** | **11** | **87** | **12.6%** (highest) |
| Obstruction | 8 | 126 | 6.3% |
| *(empty family name)* | 0 | 48 | 0.0% ⚠ |
| Fallacy | 0 | 1 | 0.0% |
| **TOTAL** | **70** | **1408** | **5.0%** |

> ⚠ **Data-quality note** : **48 rows have an empty `Family`** (and 1 empty subfamily). These are
> likely depth-1/depth-2 root rows or mis-classified leaves — a separate data-hygiene item, not an
> AIF-mapping gap. Flagged for jsboige; the chantier treats them as out-of-scope until classified.

### Per subfamily (Misleading language — chantier focus)

| Subfamily | Mapped | Total | Chantier status |
|-----------|-------:|------:|-----------------|
| Ambiguity | 4 | 39 | not started |
| **Fallacious comparison** | **2** | **13** | **✅ complete (PR-1/2/3)** |
| Inexact definition | 5 | 34 | 🚧 opened (PR-4 Vague definition) |

---

## 2. Chantier progress (4 PR)

| PR | Cluster (subfamily / sub-sub) | Leaves | Fully-modeled (DoD) | DirectRef-loose | FAIL-LOUD | State |
|----|-------------------------------|-------:|--------------------:|----------------:|----------:|-------|
| baseline | — | — | 70 | 70 | — | master |
| **PR-1 #699** | Fallacious comparison / False analogy | 4 | 74 (+4) | 74 (+4) | 1 (840 CA-missing) | merged |
| **PR-2 #701** | Fallacious comparison / Faulty comparison | 5 | 76 (+2) | 79 (+5) | 3 (834/835/837 RA-missing) | merged |
| **PR-3 #703** | Fallacious comparison / Association fallacy | 2 | 78 (+2) | 81 (+2) | 0 | OPEN CLEAN |
| **PR-4 #705** | Inexact definition / Vague definition | 3 | 81 (+3) | 84 (+3) | 0 | OPEN CLEAN |
| **cumulative** | **2 subfamilies touched** | **14** | **81 (+11)** | **84 (+14)** | **4** | — |

**Fallacious comparison subfamily : ✅ fully addressed** (3/3 sub-subs, 13/13 leaves).
**Inexact definition subfamily : 🚧 opened** (1/3 sub-subs — next: Arbitrary definition, Inconsistent definition).

### FAIL-LOUD accounting (honest gaps, 0 fabrication)

| PR | pk | Layer missing | Honest reason |
|----|----|---------------|---------------|
| PR-1 | 840 | CA-node (Conflict) | no native AIF circularity CQ for analogy |
| PR-2 | 834/835/837 | RA-node (scheme) | no native `Comparison_Inference` in the 36-scheme vocabulary |

These 4 leaves take a DirectRef (direct-conflict pattern) but document the absent scheme/CQ in
`AIF_skosOther` rather than fabricating a token. **Two distinct FAIL-LOUD layers** (CA-missing vs
RA-missing) documented across the chantier.

---

## 3. Méthode (validée ai-01 cycle précédent « rigoureuse, 0 fabrication »)

### Cluster selection
1. **Unit of work = 1 sub-sub** (`Soussousfamille`), code=truth depuis la CSV.
2. **Préférer les sub-subs avec in-sub-sub anchor mappé** (cluster shape PR-1/PR-4) — pattern le
   plus propre. Sinon, **borrow-root shape** (PR-2/PR-3) : emprunter l'anchor d2/d3 parent.
3. **Taille cible** : 2-5 leaves/PR (au-delà, découper par depth ou mécanisme).

### Modeling (2 patterns)
- **Exception pattern** (PR-1 anchor 839) : scheme légitime nommé en `ExceptionRef`, CQ violé en
  `DirectRef`. La fallacy *defeated* un scheme légitime.
- **Direct-conflict pattern** (PR-2/PR-3/PR-4 anchors 833/800) : scheme en `DirectRef` seul, pas
  d'`ExceptionRef`. La fallacy *est* un scheme défectueux.

### Leaves
- **Réutiliser le scheme+CQ de l'anchor** où la leaf est une spécialisation honnête ; varier seulement
  `MappingType` (`narrowMatch` = plus spécifique, `broadMatch` = plus large, `closeMatch` = variante
  directe).
- **FAIL-LOUD** si aucun token AIF natif ne capture le défaitur (CQ ou scheme) — **jamais fabriquer**
  de `*_Conflict` ou `*_Inference`. Documenter le gap dans `AIF_skosOther`.
- **Honest scheme-divergence** : si une leaf dans sub-sub X cible en fait un scheme d'une autre
  famille (ex. 836/838 classification leaves en sub-sub comparison), dire et utiliser le bon scheme.

### Vocabulaire natif (discipline #677)
- Restriction aux **26 Conflict nodes + 36 Inference schemes** confirmés par l'usage existant (70
  mapped rows). Aucun token inventé.
- Validation programmatisation sur chaque PR (grep des tokens backticked vs whitelist native).

---

## 4. DoD enrichi jsboige (I-node / RA-node / CA-node) — statut sérialisation

Le dernier comment jsboige sur #498 enrichit le DoD : modéliser AIF-style = décomposer l'argument
en **I-nodes** (prémisses + conclusion) + **RA-node** (scheme, inférence attaquable) + **CA-node**
(conflict, typé undermine/undercut/rebut).

### Statut chantier
- **§7 ajoutée à chaque PR** (PR-1 à PR-4) : mappe chaque leaf à la décomposition + attack-type.
- **Distribution attack-types observée** : majorité d'**undercuts** (cohérent jsboige « most
  fallacies live in the undercut »), quelques undermines, 0 rebut jusqu'ici.

### ⚠ Pas encore sérialisé — décision jsboige requise
La décomposition I/RA/CA est **enregistrée dans les docs de proposition**, pas dans le CSV. La
sérialiser nécessite de **nouvelles colonnes** :

| Colonne proposée | Contenu | Exemple |
|------------------|---------|---------|
| `AIF_attackType` | `undermine` / `undercut` / `rebut` / *(vide si FAIL-LOUD)* | `undercut` |
| `AIF_attackedNode` | `I-node` / `RA-node` / `CA-node` (le composant attaqué) | `RA-node` |

**Décision jsboige** :
- (a) **Ratifier les 2 nouvelles colonnes** → les 4 PR (back-fill §7 dans le CSV) + PRs suivantes
  les remplissent dès le départ.
- (b) **Différer** la sérialisation → les PR continuent à enregistrer I/RA/CA en prose (§7), CSV
  inchangé, décision reportée post-chantier.
- (c) **Schéma alternatif** (ex. colonne unique `AIF_decomposition` JSON) → à spécifier.

Reco chantier : **(a)** — sérialiser tôt évite un back-fill massif tardif et donne au consommateur
(EPITA harness) la donnée structurée dès le début.

---

## 5. Trajectoire & recommandation

- **1408 leaves, ~87 sub-subs** (estimation ; à confirmer par scan depth). À ~3-4 leaves/PR →
  chantier long (~30-40 PR pour couvrir les sub-subs à anchor mappé).
- **Priorité** : sub-subs avec anchor mappé d'abord (cluster shape propre, productivité max), puis
  borrow-root shapes. Families **Misleading language** (12.6%) et **Faulty logics** (10.8%) = meilleur
  ratio anchor/leaves.
- **Prochaines cibles worker-able** (post-batch ai-01 sur #703/#705) :
  - Inexact definition / Arbitrary definition (3 mapped anchors + 19 unmapped — gros, à découper).
  - Inexact definition / Inconsistent definition (0/7, borrow-root).
  - Ambiguity / Narrative ambiguity (1/8), Semantic ambiguity (2/19), Syntactic ambiguity (0/8).

### Recommandation pour jsboige
1. **Ratifier la méthode** (§3) — déjà validée ai-01, confirmation jsboige clôt le gate méthodologique.
2. **Décider le schéma I/RA/CA** (§4 — reco : (a) sérialiser tôt).
3. **Priorisation families** : confirmer Misleading language + Faulty logics en focus, ou rediriger.

---

## 6. Gate boundaries (HARD — synthèse read-only)

- ❌ No prod CSV write, no DB write, no OWL regen.
- ✅ Synthèse dérivée code=truth (CSV scan master `70bd1605`) + chantier PRs (#699/#701 merged,
  #703/#705 OPEN).
- ✅ Aucun token AIF fabriqué dans la synthèse (référence uniquement les tokens natifs existants).

Relates: #498 (chantier), PR-1 #699, PR-2 #701, PR-3 #703, PR-4 #705, #133/#130 (OWL), #499
(inverse: virtue = good tenor), #677 (0 fabrication discipline), #192 (terminology), #458.
