# #498 — AIF two-layer reconciliation, P1 tranche-1b (PREC-TIE + first SUFFIX-ONLY sub-lot)

**Worker** po-2024 · **Date** 2026-07-11 · **Base** master `f34ac77c` (post-#771, tranche-1 en prod) · **Statut** proposition **gated** (0 write prod CSV dans ce PR) · **Track** GO ai-01 « suite tiering (autonome) » (`msg-20260710T230135-kpea48`), couvert par le pilote GO #498 (jsboige 2026-06-17).

> Scope de ce PR : **docs + apply-script (dry-run)**. Aucune cellule du CSV de production n'est modifiée dans le diff. La sérialisation prod suit le flow #753/#760 (gate ai-01), après revue de cette proposition.

---

## 0. TL;DR

Suite de la réconciliation P1 (back-fill de la couche attack pour les 52 lignes skos-only). La **tranche-1** (#769/#771) a livré les **14 PRECEDENT** (token exact d'un fully-modeled). Ce PR livre la **tranche-1b = 7 lignes** :

- **2 PREC-TIE** (777 Inconsistance, 633 Relation infondée) — tokens votant pour des types **divergents**, arbitrés par co-token + `desc_fr` (règle de tie-break **durcie par l'audit #770** : jamais un token partagé isolé).
- **5 SUFFIX-ONLY** (premier sous-lot : Influence 337/432/356 + Obstruction 1280/1360) — **aucun** token à précédent in-set ; modélisation Walton **au cas par cas** depuis le scheme propre + `desc_fr` (« que défait le CQ »). Le **prior de suffixe est proscrit** (contre-preuve in-set pk804 `_Conflict`→undercut, audit #770).

Distribution : **4 undermine / 2 undercut / 1 rebut**. `attackType` = **jugement neuf** (0-risque token, pas 0-risque modélisation) ; **0 fabrication de token** (#677) — on type des lignes déjà skos-vettées. `attackedNode` déterministe (#707§4 a).

Compléter la tranche-1b porte le fully-modeled **+7** (**32 → 39** ; #771 mergé, voir §6). Reste après : **31 SUFFIX-ONLY** (sous-lots suivants).

---

## 1. Fondation (héritée de #769 + #770)

- **Méthode** (#769 §2) : l'`attackType` de chaque ligne skos-only se dérive de **sa propre** signature skos, ancrée sur les **18 lignes fully-modeled** (ground truth token→type). Pas d'héritage mécanique du sous-sous anchor (0/19 alignées, #769 §1).
- **Audit des 18 anchors** (#770 §2bis) : **16 CLEAN / 2 SOFT / 0 erreur franche, node-map 18/18 propre**. L'ancrage est validé et **outillé** de 3 trouvailles réutilisées ici :
  1. **suffixe non décisif** (pk804 `PropertyNotExistant_Conflict`→undercut) → le prior de suffixe est proscrit pour les 5 SUFFIX-ONLY ;
  2. **slippery-slope penche undercut** (pk858) — non mobilisé dans ce sous-lot mais confirme la méthode « scheme propre » ;
  3. **token partagé non décisif** (`CircumstantialAdHominem_Inference` dans 1361 rebut ET 1371 undermine) → **règle de tie-break** appliquée à 777/633 : co-token + `desc_fr`, jamais un token isolé.

Carte token→type (extraite des 18, `tools/498-p1b-apply.py` la re-dérive) confirmée code=truth. Node déterministe :

| attackType | attackedNode | composant attaqué |
|---|---|---|
| undercut | `RA-node` | inférence (la règle ne s'applique pas) |
| undermine | `I-node` | prémisse (acceptabilité contestée) |
| rebut | `CA-node` | conclusion (contre-conclusion / blocage du dialogue) |

---

## 2. PREC-TIE — arbitrage documenté (2 lignes)

Les deux lignes portent des tokens à précédent qui **votent différemment**. On tranche par la règle durcie #770 (co-token + `desc_fr` + famille), pas par un token isolé.

### pk777 « Inconsistance » (Erreur de raisonnement) → **undermine / I-node** · MED

- Tokens (Direct) : `OpposedCommitment_Conflict` → **undermine** (pk1297 Preuve par assertion, CLEAN) · `InconsistentCommitment_Inference` → **rebut** (pk1361 Procès en incohérence, **SOFT**).
- `desc_fr` : « Votre raisonnement repose sur des affirmations qui se contredisent entre elles. »
- **Arbitrage** : pk777 est une **erreur de raisonnement** (jeu de prémisses mutuellement contradictoires), **pas** un coup d'Obstruction. La contradiction dit « au moins une prémisse est inacceptable » → **I-node** (undermine), lecture ASPIC+ standard de l'inconsistance de prémisses (elle ne présente pas de contre-conclusion indépendante → pas un rebut canonique). Le co-token **CLEAN** OpposedCommitment (undermine) l'emporte sur le précédent rebut **SOFT** de pk1361 — ce dernier vise le coup dialectique « procès en incohérence » en contexte **Obstruction** (accusation d'incohérence), distinct du défaut structurel de pk777.
- **Alternative signalée** : rebut/CA (si l'on suit strictement pk1361). Flag MED — à confirmer ai-01.

### pk633 « Relation infondée » (Erreur mathématique) → **undercut / RA-node** · MED

- Tokens : `PropertyNotExistant_Conflict` (**Direct**, CQ primaire) → **undercut** (pk804, CLEAN) · `Sign_Inference` (**Exception**, secondaire) → **undermine** (pk357).
- `desc_fr` : « Vous attribuez une relation significative à ce qui n'est qu'une simple coïncidence. »
- **Arbitrage** : le **CQ primaire direct** l'emporte. La « relation infondée » infère une relation réelle depuis une coïncidence → le **pas inférentiel** (coïncidence ⇒ relation) est le défaut → **RA-node** (undercut). Parallèle **quasi-exact** avec **pk621 Transfert illicite** (tranche-1) : même famille Erreur mathématique, même token Direct `PropertyNotExistant_Conflict`, typé undercut via le même anchor pk804. `Sign_Inference` (Exception) reste secondaire.
- Flag MED (tie arbitré) — le parallèle pk621 le rapproche de HIGH.

---

## 3. SUFFIX-ONLY — premier sous-lot, modélisation Walton (5 lignes)

Aucun token de ces lignes n'a de précédent in-set (vérifié `498-p1b-apply.py` / précédent-check). On modélise **depuis le scheme propre + `desc_fr`** (« que défait le CQ »). **Prior de suffixe proscrit.**

| pk | sophisme (famille) | scheme propre | `desc_fr` (abrégé) | → type | node | conf |
|---:|---|---|---|---|---|---|
| 337 | Appel à la terreur (Influence/Repoussoir) | `FearAppeal_Inference` + `IrrationalFearAppeal_Conflict` | « provoquez la peur… en délaissant les arguments raisonnables » | **undermine** | I | HIGH |
| 432 | Engagement (Influence/Jeu de pouvoir) | `Waste_Inference` (argument-from-waste / sunk cost) | « l'invitant à assumer une responsabilité induite » | **undercut** | RA | MED |
| 356 | Manipulation mentale (Influence) | `ConflictingGoals_Conflict` | « orientent subtilement le point de vue… sans qu'ils s'en rendent compte » | **undermine** | I | MED |
| 1280 | Obstruction (parente) | `ResolvingInconsistency_Conflict` | « la discussion ne se déroule pas comme prévu » | **rebut** | CA | MED |
| 1360 | Ad hominem (Obstruction) | `Ethotic_Inference` (ethos/caractère) | « visez votre adversaire lui-même plutôt que… ses arguments » | **undermine** | I | HIGH |

- **337** — l'appel à la peur injecte une **prémisse émotionnelle** (la peur) en lieu de raison → I. Dérivé du scheme propre (FearAppeal), **PAS** hérité de pk322 (scheme différent) ; la convergence avec le cluster émotionnel Influence (300/322/357 undermine) est un contrôle, pas la base.
- **432** — `Waste_Inference` (coût irrécupérable) : l'engagement passé est réel mais **non pertinent** pour la décision à venir ; le pas « déjà engagé ⇒ doit continuer » ne tient pas → **RA** (undercut). Flag MED (sunk-cost classiquement inférentiel, mais cadrage « jeu de pouvoir » lisible premise-level).
- **356** — manipulation mentale = **façonnage covert des prémisses/cadrage** de l'interlocuteur (mécanisme Influence, cf. pk177) → I ; **pas** un déraillement dialogue-level de l'Obstruction. Flag MED (parente générique, pas de sous-sous).
- **1280** — parente Obstruction : `desc_fr` = **déraillement du dialogue lui-même**, même nucleus dialogue-block que pk1281 Refus du débat (tranche-1 rebut) et l'anchor pk1313 Évasion (rebut) → **CA**. Flag MED : rebut = **cas-queue structurel rare (~1-5%)**, à confirmer ai-01.
- **1360** — ad hominem = attaque de la **prémisse de crédibilité** (ethos) → I, textbook undermine ; parallèle pk1398 Attaque personnelle (CLEAN) + pk1371 Sophisme génétique. **NB** : le typage Virtues « détournement=undercut » (mirror #499) **ne s'applique PAS** aux Fallacies (dataset/structure différents ; audit #770 : 1398/1371 = undermine en prod).

Détail complet en colonne `justification` de [`498-reconciliation-p1b-annotations.csv`](498-reconciliation-p1b-annotations.csv).

---

## 4. Distribution & discipline rebut-rarity

**4 undermine / 2 undercut / 1 rebut.** Le seul **rebut** (pk1280) est un **cas-queue structurel** (dialogue-block, cohérent avec le nucleus Obstruction Évasion/Refus). Conforme au pattern « rebut ~1-5%, localisé aux blocages de dialogue / appels aux conséquences » (dashboard). Les **2 MED-flag load-bearing** à revoir en priorité : **777** (alternative rebut) et **1280** (rebut tail-case).

---

## 5. Justification par ligne

Voir colonne `justification` du CSV machine-readable. Synthèse des mécanismes :

- **Undermine — prémisse déformée/inacceptable** : 777 (prémisses contradictoires), 337 (prémisse émotionnelle/peur), 356 (cadrage manipulé), 1360 (prémisse de crédibilité).
- **Undercut — la règle d'inférence ne tient pas** : 633 (coïncidence ⇏ relation), 432 (engagement passé ⇏ poursuite).
- **Rebut — blocage de la conclusion/du dialogue** : 1280 (déraillement de la discussion).

---

## 6. Sérialisation (flow #753/#760)

`tools/498-p1b-apply.py` — **gated, dry-run par défaut**, mirroir de `tools/498-p1-apply.py` (#769) :

- lit `498-reconciliation-p1b-annotations.csv` et **re-vérifie** que sa carte interne concorde 7/7 (assertion load-bearing) ;
- splitters byte-exact (guillemets doublés + LF encadrés), cell-fill des seules colonnes `AIF_attackType`/`AIF_attackedNode` des 7 PK ;
- preuve de **byte-preservation** (0 mismatch hors les 2 cellules × 7 lignes), well-formedness 104 cols, BOM+CRLF préservés ;
- `--write` **gaté** (ai-01), backup `tmp/Fallacies-backup-pre-p1b.csv` avant écriture pour vérif indépendante.

**Baseline dynamique (dépendance #771).** Les 7 PK de cette tranche sont **disjoints** des 14 de la tranche-1 (#771). Le script lit le compte **réel** et rapporte `total → total+7` sans jamais coder en dur la baseline :

- si #771 **déjà mergé** : `107 → 114` ;
- si #771 **pas encore mergé** : `93 → 100`.

L'ordre de merge #771 vs ce PR est donc sans impact sur la correction (cellules disjointes). #771 a été **mergé** (master `f34ac77c`, tranche-1 en prod) ; dry-run actuel : `107 → 114`, delta +102 B, 0 mismatch, 1409×104, CRLF+BOM préservés.

```bash
python tools/498-p1b-apply.py            # dry-run (0 write prod) — ce PR
python tools/498-p1b-apply.py --write    # APPLY 7 cellules (GATÉ — relais ai-01)
```

---

## 7. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (docs + apply-script dry-run uniquement — `git diff f34ac77c -- Cards/…/Taxonomy.csv` vide).
- ✅ **0 fabrication token #677** — aucun token AIF créé ; on type des lignes déjà skos-vettées.
- ✅ Code=truth : tokens/labels/`desc_fr` lus du CSV master `f34ac77c` ; précédents = les 18 fully-modeled (audités CLEAN, #770).
- ✅ Prior de suffixe **proscrit** ; PREC-TIE arbitrés par co-token + `desc_fr` (règle durcie #770), pas mécaniquement.
- ❌ #674/#666/#596 non touchés (HOLD). Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante gated (relais ai-01), pas dans ce PR.

🤖 Worker po-2024 — réconciliation P1 tranche-1b (2 PREC-TIE + 1er sous-lot SUFFIX-ONLY).
