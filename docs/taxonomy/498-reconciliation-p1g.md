# #498 — AIF two-layer reconciliation, P1 tranche-1g (SUFFIX-ONLY, famille « Tricherie », MIXTE) — DERNIÈRE FAMILLE

**Worker** po-2024 · **Date** 2026-07-11 · **Base** master `cbf9b7c8` (post-#782 ; tranches 1+1b+1c écrites en prod → attack 121 ; write batché 1d+1e+1f #783 en attente de merge → 137 ; cette proposition gated) · **Statut** proposition **gated** (0 write prod CSV dans ce PR) · **Track** GO ai-01 `msg-ftqy51` « propose 1g Tricherie (dernière SUFFIX-ONLY) ». **⚠️ Dernière famille SUFFIX-ONLY → clôture des 36-row back-fill** (5 1b + 7 1c + 4 1d + 5 1e + 7 1f + 8 1g = 36).

> Scope de ce PR : **docs + apply-script (dry-run)**. 0 write prod. Sérialisation gated (relais ai-01).

---

## 0. TL;DR

Suite de la réconciliation P1. Tranches 1 + 1b + 1c (prod) + 1d/1e/1f (write #783 pending) déjà livrées. Ce PR livre la **tranche-1g = 8 SUFFIX-ONLY, famille « Tricherie » complète** (Arranger les faits + Changement de cap + Pensée biaisée) — **dernière famille SUFFIX-ONLY** :

| pk | sophisme (text_fr) | sous-famille |
|---:|---|---|
| 888 | Présentation trompeuse des faits | Arranger les faits |
| 973 | Déplacement des critères | Changement de cap |
| 1020 | Sophisme des coûts irrécupérables | Changement de cap / Exigence relâchée |
| 1023 | Raisonnement biaisé | Pensée biaisée |
| 1066 | Effet de simple exposition | Pensée biaisée / Biais naturels |
| 1087 | Preuve anecdotique | Pensée biaisée / Biais naturels |
| 1148 | Justification de l'effort | Pensée biaisée / Biais naturels |
| 1175 | Biais d'attribution | Pensée biaisée / Biais culturels |

**Famille HÉTÉROGÈNE (MIXTE)** : distribution **3 undermine / 5 undercut**. Trois mécanismes distincts :
- **manipulation relationnelle** (arrangement trompeur, déplacement de critères, raisonnement biaisé) → la **prémisse** est biaisée/violée → **undermine/I** (888, 973, 1023) ;
- **sunk cost / effort justification** → la **règle d'inférence** practical-reasoning/waste est défectueuse → **undercut/RA** (1020, 1148) ;
- **biais cognitifs d'induction** (mere exposure, preuve anecdotique, attribution) → l'**inférence** est faible → **undercut/RA** (1066, 1087, 1175).

**6/8 tokens ont un precedent typé** (PositionToKnow pk70, ConflictingGoals pk356, Waste_Inference pk432 ×2, Bias pk70, PresumptiveInference pk70) ; 2/8 novel (Example_Inference ×2, InductiveInference_Scheme). Prior de suffixe **proscrit** (mémoire aif-no-inherit ; pk804 `_Conflict`→undercut). `attackType` = jugement neuf ; **0 fabrication token** (#677). `attackedNode` déterministe (#707§4a).

Back-fill **+8** : attack-typed **121 → 129** sur master actuel (ou 137 → 145 si #783 mergeé avant le write 1g — baseline dynamique, §5). **Clôture des 36 SUFFIX-ONLY** : après write 1g, la couche attack couvre les 52 skos-only (36 SUFFIX-ONLY + 16 tranche-1 PRECEDENT... en fait 14 tranche-1 + 2 PREC-TIE 1b = 16 non-suffix + 36 suffix = 52 ✅).

**3 MED flags** (888, 1066, 1175) — dont **1 divergence same-token** (pk888 `LackOfPTKReliability_Scheme` vs tranche-1e pk2). **5 HIGH** (973, 1020, 1023, 1087, 1148 — 4 avec precedent direct + 1 desc-twin pk34).

---

## 1. Fondation (héritée de #769 + #770)

- **Méthode** (#769 §2) : `attackType` dérivé de **sa propre** signature skos + `desc_fr`. Pas d'héritage d'anchor (0/19 alignées, #769 §1).
- **Audit 18 anchors** (#770) : 16 CLEAN / 2 SOFT / 0 erreur. Suffixe non décisif (pk804) → prior proscrit.
- **Precedent-check** (cette tranche) : 6/8 tokens ont un precedent typé in-set — forte ancre empirique. Aucun precedent ne force un verdict (confirmation seulement).
- **Mémoire rebut-rarity** : les fallacies relationnelles de manipulation (déplacement, arrangement trompeux) sont **undermine/undercut**, PAS rebut (pas de contre-conclusion indépendante). Cohérent avec 973/888.

Node déterministe :

| attackType | attackedNode | composant attaqué |
|---|---|---|
| undercut | `RA-node` | inférence (la règle ne s'applique pas) |
| undermine | `I-node` | prémisse (acceptabilité contestée) |
| rebut | `CA-node` | conclusion (contre-conclusion / blocage du dialogue) |

---

## 2. SUFFIX-ONLY — famille « Tricherie », modélisation Walton (8 lignes, MIXTE)

Modélisation **depuis le scheme propre + `desc_fr`**. Prior de suffixe **proscrit**.

| pk | sophisme | scheme propre | `desc_fr` (abrégé) | → type | node | conf | precedent |
|---:|---|---|---|---|---|---|---|
| 888 | Présentation trompeuse des faits | `LackOfPTKReliability_Scheme` + `PositionToKnow_Inference` | « présentez les faits d'une manière susceptible d'induire en erreur » | **undermine** | I | MED | ⚠️ same-token pk2→undercut ; PositionToKnow pk70→undermine |
| 973 | Déplacement des critères | `ConflictingGoals_Conflict` | « changez les critères du débat sans le dire » | **undermine** | I | HIGH | ConflictingGoals pk356 |
| 1020 | Sophisme des coûts irrécupérables | `Waste_Inference` | « persistez parce que vous y avez consacré beaucoup de ressources » | **undercut** | RA | HIGH | Waste_Inference pk432 |
| 1023 | Raisonnement biaisé | `Bias_Inference` + `PresumptiveInference_Scheme` | « vos biais orientent vos arguments » | **undermine** | I | HIGH | Bias pk70 + Presumptive pk70 |
| 1066 | Effet de simple exposition | `Example_Inference` | « acceptez parce que c'est devenu familier » | **undercut** | RA | MED | token novel ; parallèle pk34/pk595 |
| 1087 | Preuve anecdotique | `InductiveInference_Scheme` | « événement isolé qui ne suffit pas à démontrer une règle » | **undercut** | RA | HIGH | **desc TWIN pk34** (undercut) |
| 1148 | Justification de l'effort | `Waste_Inference` | « plus de valeur parce que vous avez beaucoup travaillé » | **undercut** | RA | HIGH | Waste_Inference pk432 |
| 1175 | Biais d'attribution | `Example_Inference` | « attribuez à tort à certaines causes, sous-estimant les circonstances » | **undercut** | RA | MED | token novel ; parallèle pk34/pk595 |

- **888** — arrangement/cherry-picking trompeur des faits. La prémisse (les faits présentés sont représentatifs/fidèles) est biaisée → I. Precedent `PositionToKnow_Inference`→undermine (pk70). **⚠️ Flag MED + SAME-TOKEN DIVERGENCE** : `LackOfPTKReliability_Scheme` apparaît aussi en tranche-1e pk2 (Généralisation hâtive → UNDERCUT, desc inférentielle d'induction). Ici desc déférentielle (présentation trompeuse = prémisse) → undermine. Verdicts divergent par la desc, NON par le token (mémoire aif-no-inherit). **Voir §3.**
- **973** — moving the goalposts : critères déplacés secrètement. La prémisse (critères fixes/consistants) est violée → I. Precedent direct `ConflictingGoals_Conflict`→undermine (pk356). Fallacy relationnelle de manipulation = undermine (mémoire rebut-rarity).
- **1020** — sunk cost : la règle [beaucoup investi] → [faut continuer] est défectueuse → RA. Precedent direct `Waste_Inference`→undercut (pk432). Textbook undercutter du practical-reasoning/waste scheme.
- **1023** — biais orientent les arguments : l'acceptabilité des prémisses est biaisée → I. **Double precedent** `Bias_Inference`+`PresumptiveInference_Scheme`→undermine (pk70 ×2). Verdict HIGH confiant.
- **1066** — mere exposure : la règle [familier] → [acceptable] est défectueuse → RA. Token `Example_Inference` novel, parallèle sémantique pk34/pk595 (induction → undercut). **Flag MED** : alt undermine (prémisse « familier » surévaluée) ; écartée car le biais porte sur l'inférence d'acceptation.
- **1087** — **desc_fr IDENTIQUE à tranche-1e pk34** (Preuve anecdotique, déjà undercut/RA) = hasty generalization / undercutter de l'induction → RA. Token `InductiveInference_Scheme` (seul, vs pk34 qui a Example+Inductive). ⚠️ Nœud **distinct** de pk34 (PK + sous-famille différents : 1087 sous Pensée biaisée vs 34 sous Insuffisance). Verdict cohérent undercut.
- **1148** — effort justification : même mécanisme que sunk cost, la règle [beaucoup d'effort] → [plus de valeur] est défectueuse → RA. Precedent direct `Waste_Inference`→undercut (pk432). Parallèle pk1020.
- **1175** — fundamental attribution error : la règle d'attribution [comportement] → [cause interne] est trop systématique → RA. Token `Example_Inference` novel, parallèle pk34/pk595. **Flag MED** : alt undermine (prémisse causale fausse) ; écartée car le biais porte sur l'inférence d'attribution.

Détail complet en colonne `justification` de [`498-reconciliation-p1g-annotations.csv`](498-reconciliation-p1g-annotations.csv).

---

## 3. Pourquoi MIXTE (3 undermine / 5 undercut) + divergence same-token pk888/pk2

La famille « Tricherie » regroupe des stratagèmes intentionnels (arranger, déplacer, biaisér) ET des biais cognitifs (exposition, attribution). Deux natures de défaut :

- **Manipulation relationnelle** (888, 973, 1023) : le stratagème attaque une **prémisse** (fiabilité de la présentation, consistance des critères, objectivité du jugement) = **undermine/I**. Fallacies relationnelles → undermine (mémoire rebut-rarity, pas de contre-conclusion).
- **Sunk cost / effort** (1020, 1148) : la **règle d'inférence** practical-reasoning/waste ([investi] → [continuer/valeur]) est défectueuse = **undercut/RA**. Waste_Inference precedent pk432.
- **Biais d'induction** (1066, 1087, 1175) : l'**inférence** inductive (familier→acceptable, exemple→règle, comportement→cause) est faible = **undercut/RA**. Parallèles pk34/pk595.

**⚠️ Divergence same-token pk888 / pk2** (2ᵉ du genre après pk808/pk33 en 1f). `LackOfPTKReliability_Scheme` :
- **tranche-1e pk2** (Généralisation hâtive) → **undercut** : desc inférentielle (induction depuis impressions/anecdotes).
- **tranche-1g pk888** (Présentation trompeuse) → **undermine** : desc déférentielle (arrangement trompeur des faits = prémisse biaisée), renforcée par le 2ᵉ token `PositionToKnow_Inference`→undermine (pk70).

Verdicts opposés **justifiés par la desc**, pas par le token (mémoire `aif-no-inherit`). Ceci confirme et renforce le principe desc-driven testé en tranche-1f (pk808/pk33). ai-01 devrait vérifier cette 2ᵉ occurrence — si le principe est rejeté, pk2 et pk888 doivent être alignées.

**Les 3 MED-flag load-bearing à revoir en priorité par ai-01 : 888, 1066, 1175.**

---

## 4. Justification par ligne

Voir colonne `justification` du CSV machine-readable. Mécanismes :

- **prémisse biaisée/violée** (manipulation relationnelle) : 888 (arrangement trompeux), 973 (déplacement critères), 1023 (biais) ;
- **règle practical-reasoning/waste défectueuse** : 1020 (sunk cost), 1148 (effort justification) ;
- **inférence inductive faible** : 1066 (mere exposure), 1087 (preuve anecdotique), 1175 (biais attribution).

---

## 5. Sérialisation (flow #753/#760)

`tools/498-p1g-apply.py` — **gated, dry-run par défaut**, miroir de `tools/498-p1f-apply.py` :

- lit `498-reconciliation-p1g-annotations.csv` et **re-vérifie** MAP 8/8 ;
- splitters byte-exact, cell-fill des seules colonnes `AIF_attackType`/`AIF_attackedNode` des 8 PK ;
- pre-state : les 8 PK **vides** + **portent une skos** ;
- preuve **byte-preservation** (0 mismatch), well-formedness 104 cols, BOM+CRLF ;
- `--write` **gaté** (ai-01), backup `tmp/Fallacies-backup-pre-p1g.csv`.

**Baseline dynamique (write-driven).** Les 8 PK sont **disjoints** de toutes tranches précédentes. Le script lit le compte réel :

- master actuel `cbf9b7c8` (1+1b+1c écrits) : `121 → 129` ;
- si write batché 1d+1e+1f (#783) mergeé : `137 → 145`.

Dry-run actuel (baseline 121) : `121 → 129`, delta **+120 B**, **0 mismatch**, **1409×104**, CRLF+BOM préservés, distribution 3 undermine / 5 undercut.

```bash
python tools/498-p1g-apply.py            # dry-run (0 write prod) — ce PR
python tools/498-p1g-apply.py --write    # APPLY 8 cellules (GATÉ — relais ai-01)
```

---

## 6. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (`git diff cbf9b7c8 -- Cards/…/Taxonomy.csv` vide).
- ✅ **0 fabrication token #677** — on type des lignes déjà skos-vettées.
- ✅ Code=truth : tokens/labels/`desc_fr` lus du CSV master `cbf9b7c8` ; SUFFIX-ONLY confirmé (6/8 precedent typé, 2/8 novel).
- ✅ Prior de suffixe **proscrit** ; distribution MIXTE = 3 natures de défaut vérifiées (manipulation = undermine ; waste/induction = undercut), chaque ligne modélisée, 3 MED flaguées avec alternatives.
- ⚠️ **Divergence same-token pk888/pk2 documentée** (§3) — 2ᵉ occurrence après pk808/pk33 (1f), renforce le principe desc-driven. À vérifier par ai-01.
- ✅ **Dernière famille SUFFIX-ONLY** → write 1g clôturera les 36-row back-fill.
- ❌ #674/#666/#596 non touchés (HOLD). Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante gated (relais ai-01).

🤖 Worker po-2024 — réconciliation P1 tranche-1g (famille « Tricherie », 8 SUFFIX-ONLY, MIXTE, DERNIÈRE FAMILLE).
