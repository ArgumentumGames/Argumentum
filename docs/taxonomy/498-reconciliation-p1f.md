# #498 — AIF two-layer reconciliation, P1 tranche-1f (SUFFIX-ONLY, famille « Abus de langage », MIXTE)

**Worker** po-2024 · **Date** 2026-07-11 · **Base** master `c1ed77d2` (post-#777 ; tranches 1+1b écrites en prod → attack 114 ; 1c #779 + 1d #778 + 1e #780 en attente de review/write) · **Statut** proposition **gated** (0 write prod CSV dans ce PR) · **Track** GO ai-01 « suite SUFFIX-ONLY par sous-lots famille jusqu'à épuisement » (tertiaire GO autonome), couvert par le pilote GO #498 (jsboige 2026-06-17).

> Scope de ce PR : **docs + apply-script (dry-run)**. Aucune cellule du CSV de production n'est modifiée dans le diff. La sérialisation prod suit le flow #753/#760 (gate ai-01), après revue de cette proposition.

---

## 0. TL;DR

Suite de la réconciliation P1. Tranches **1** (14 PRECEDENT) + **1b** (2 PREC-TIE + 5 SUFFIX-ONLY) + **1c** (7 SUFFIX-ONLY « Erreur de raisonnement ») + **1d** (4 SUFFIX-ONLY « Erreur mathématique ») + **1e** (5 SUFFIX-ONLY « Insuffisance ») déjà livrées. Ce PR livre la **tranche-1f = 7 SUFFIX-ONLY, famille « Abus de langage » complète** (Définition inexacte + Comparaison fallacieuse + Ambiguïté) :

| pk | sophisme (text_fr) | sous-famille |
|---:|---|---|
| 800 | Acception vague | Définition inexacte / Acception vague |
| 808 | Sophisme des corrélatifs | Définition inexacte / Acception arbitraire |
| 814 | Faux dilemme | Définition inexacte / Acception arbitraire |
| 833 | Comparaison fallacieuse | Comparaison fallacieuse |
| 839 | Fausse analogie | Comparaison fallacieuse / Fausse analogie |
| 856 | Expression vague | Ambiguïté / Equivoque |
| 876 | Ambiguïté narrative | Ambiguïté / Ambiguïté narrative |

**Contrairement aux familles inference-uniformes (1c/1d/1e toutes 100% undercut), cette famille est HÉTÉROGÈNE** : les manipulations définitionnelles/classificatoires attaquent la **prémisse** (le terme s'applique / l'exhaustivité / la comparaison est biaisée = contestable) → **undermine/I-node** ; seul le scheme analogique attaque la **règle d'inférence** → **undercut/RA-node** (pk839). Distribution : **6 undermine / 1 undercut / 0 rebut**. C'est la distribution MIXTE annoncée dans les transparences 1d/1e.

**5 tokens sur 7 sont novel** (aucun precedent typé in-set) ; 2 ont un precedent undermine (BiasedClassification_Conflict→pk799 pour pk833 ; ConflictingGoals_Conflict→pk356 pour pk876). Modélisation Walton **au cas par cas** depuis le scheme propre + `desc_fr`. Le **prior de suffixe est proscrit** (contre-preuve in-set pk804 `_Conflict`→undercut, audit #770). `attackType` = **jugement neuf** ; **0 fabrication de token** (#677). `attackedNode` déterministe (#707§4 a).

Back-fill **+7** : attack-typed **114 → 121** (baseline dynamique, §5). Reste après ce sous-lot : **8 SUFFIX-ONLY** (Tricherie 8 — dernière famille, distribution MIXTE attendue).

**4 flags MED** (800, 808, 856, 876) — coût honnête d'une famille hétérogène. **3 HIGH** (814, 833, 839). Le point le plus délicat : **pk808** (same-token `AlternativeMeans_Conflict` que tranche-1e pk33, verdict divergent justifié par la desc — §3).

---

## 1. Fondation (héritée de #769 + #770)

- **Méthode** (#769 §2) : l'`attackType` de chaque ligne skos-only se dérive de **sa propre** signature skos + `desc_fr`, ancrée sur les lignes fully-modeled. Pas d'héritage mécanique d'anchor (0/19 alignées, #769 §1).
- **Audit des 18 anchors** (#770) : **16 CLEAN / 2 SOFT / 0 erreur franche**. Trouvaille ancrante : suffixe non décisif (pk804 `PropertyNotExistant_Conflict`→undercut) → prior de suffixe proscrit.
- **Precedent-check par token** (cette tranche) : 5 tokens novel, 2 avec precedent undermine. Aucun precedent ne force un verdict — il confirme seulement la lean undermine des schemes de classification/verbal-classification.

Node déterministe :

| attackType | attackedNode | composant attaqué |
|---|---|---|
| undercut | `RA-node` | inférence (la règle ne s'applique pas) |
| undermine | `I-node` | prémisse (acceptabilité contestée) |
| rebut | `CA-node` | conclusion (contre-conclusion / blocage du dialogue) |

---

## 2. SUFFIX-ONLY — famille « Abus de langage », modélisation Walton (7 lignes, MIXTE)

Modélisation **depuis le scheme propre + `desc_fr`** (« que défait le CQ »). **Prior de suffixe proscrit.**

| pk | sophisme | scheme propre (signature skos) | `desc_fr` (abrégé) | → type | node | conf | precedent |
|---:|---|---|---|---|---|---|---|
| 800 | Acception vague | `VagueVerbalClassification_Inference` | « termes imprécis pour rester évasif, modifier en cours de route » | **undermine** | I | MED | sibling ArbitraryVerbal→undermine |
| 808 | Sophisme des corrélatifs | `AlternativeMeans_Conflict` | « manipule des termes mutuellement exclusifs de façon trompeuse » | **undermine** | I | MED | ⚠️ same-token pk33→undercut |
| 814 | Faux dilemme | `Dilemma_Inference` | « choix limité à deux options, alors qu'il existe d'autres possibilités » | **undermine** | I | HIGH | — |
| 833 | Comparaison fallacieuse | `BiasedClassification_Conflict` + `ExceptionSimilarityCase_Conflict` | « comparaison trompeuse pour appuyer votre point de vue » | **undermine** | I | HIGH | BiasedClassification→undermine pk799 |
| 839 | Fausse analogie | `DifferencesUndermineSimilarity_Conflict` + `Analogy_Inference` | « parallèle sur un seul point commun, négligeant les différences » | **undercut** | RA | HIGH | — |
| 856 | Expression vague | `VagueVerbalClassification_Inference` | « termes si vagues qu'ils ne permettent pas de comprendre » | **undermine** | I | MED | même token pk800 |
| 876 | Ambiguïté narrative | `ConflictingGoals_Conflict` | « récit qui peut être interprété de plusieurs manières » | **undermine** | I | MED | ConflictingGoals→undermine pk356 |

- **800** — termes imprécis pour rester évasif. La classification/application du terme est contestable → prémisse inacceptable → I. Sibling d'`ArbitraryVerbalClassification_Inference` (→undermine 3/3). **Flag MED** : alt undercut (« modifier en cours de route » = glissement équivoque) ; écartée car le défaut central est l'imprécision (prémisse mal définie).
- **808** — sophisme des corrélatifs : manipule des termes mutuellement exclusifs. La prémisse (exclusivité rigoureuse des termes) est manipulée/fausse → I. **⚠️ Flag MED + DISCONNECT skos/desc + SAME-TOKEN DIVERGENCE** : token `AlternativeMeans_Conflict` (suggère undercutter means-end) vs desc définitionnelle. **Même token que tranche-1e pk33** (typé UNDERCUT, desc inférentielle d'induction). Verdicts opposés justifiés par la divergence de desc (définitionnel ici vs inférentiel en pk33), NON par le token (mémoire aif-no-inherit). **Point le plus délicat de la tranche — voir §3.**
- **814** — faux dilemme : la prémisse « il n'y a que deux options » est fausse (d'autres existent) → I. Token novel mais sémantique non ambiguë (attaque la prémisse d'exhaustivité). Cohérent avec la transparence tranche-1d §3 (814→undermine annoncé).
- **833** — comparaison trompeuse : la classification/comparaison est biaisée → prémisse fausse → I. Precedent in-set `BiasedClassification_Conflict`→undermine (pk799). **Distinction explicite avec pk839** : 833 = classification biaisée (prémisse, undermine) ; 839 = inférence analogique (règle, undercut).
- **839** — fausse analogie : l'argument-from-analogy est un **scheme** (règle d'inférence). Le CQ « différences pertinentes ? » (= `DifferencesUndermineSimilarity_Conflict`) attaque l'applicabilité de la règle analogique → l'inférence (transfert analogique) est défectueuse → RA. Textbook undercutter du scheme analogique. Clean (cadrage scheme propre).
- **856** — expression vague : vagueness pure, la prémisse (terme applicable/compréhensible) est inacceptable → I. Même token que pk800 → aligné undermine. **Flag MED** : le sous-sous « Equivoque » suggère équivocation (undercut), mais desc porte sur l'incompréhension par vagueur (prémisse) → undermine. Plus clean que pk800 (pas de glissement).
- **876** — ambiguïté narrative : les engagements/goals du récit sont conflictuels → la prémisse (sens unique) est inacceptable → I. Precedent in-set `ConflictingGoals_Conflict`→undermine (pk356). **Flag MED** : « plusieurs interprétations » = ambiguïté → alt undercut (bascule d'interprétation) ; écartée car le token + desc ciblent le conflit d'engagements (prémisse).

Détail complet en colonne `justification` de [`498-reconciliation-p1f-annotations.csv`](498-reconciliation-p1f-annotations.csv).

---

## 3. Pourquoi MIXTE (6 undermine / 1 undercut) — et la divergence same-token pk808/pk33

Cette famille est hétérogène, contrairement aux familles inference-uniformes précédentes (1c/1d/1e toutes 100% undercut). La raison est sémantique :

- **Manipulation définitionnelle/classificatoire** (800, 808, 814, 833, 856, 876) : le défaut porte sur la **prémisse** — l'application du terme, l'exhaustivité des options, l'aptitude de la comparaison, la compréhension du récit sont **contestables/fausses**. Attaquer une prémisse = **undermine/I-node**.
- **Scheme analogique** (839) : l'argument-from-analogy est une **règle d'inférence** ; le CQ des différences attaque son applicabilité = **undercut/RA-node**.

La frontière premise-vs-inference est nette et documentée par ligne (ex : 833 vs 839, même sous-famille « comparaison », verdicts différents par la nature du défaut : classification biaisée vs transfert analogique).

**⚠️ Divergence same-token pk808 / pk33 (le point le plus délicat).** `AlternativeMeans_Conflict` apparaît deux fois :
- **tranche-1e pk33** (Justification triviale) → **undercut** : desc_fr inférentielle (« accorder à un exemple la valeur d'une preuve » = induction faible = undercutter).
- **tranche-1f pk808** (Sophisme des corrélatifs) → **undermine** : desc_fr définitionnelle (« manipule des termes mutuellement exclusifs » = prémisse fausse).

Les verdicts opposés sont **justifiés par la divergence de desc**, pas par le token. Ceci illustre la leçon mémoire `aif-no-inherit` : **le token seul n'est pas décisif** ; l'`attackType` est un jugement basé sur la signature skos + la sémantique propre (desc_fr). ai-01 devrait vérifier spécifiquement cette paire — si le principe « desc-driven, token non-décisif » est rejeté, alors pk33 et pk808 doivent être alignées (et les deux tranches ajustées).

**Les 4 MED-flag load-bearing à revoir en priorité par ai-01 : 800, 808, 856, 876.**

---

## 4. Justification par ligne

Voir colonne `justification` du CSV machine-readable. Mécanismes :

- **prémisse définitionnelle inacceptable** (terme vague/exclusif/faux) : 800, 808, 856 (vagueness/correlatifs), 876 (engagements conflictuels) ;
- **prémisse d'exhaustivité fausse** : 814 (faux dilemme) ;
- **prémisse de classification biaisée** : 833 (comparaison fallacieuse) ;
- **règle d'inférence analogique inapplicable** : 839 (fausse analogie).

---

## 5. Sérialisation (flow #753/#760)

`tools/498-p1f-apply.py` — **gated, dry-run par défaut**, miroir de `tools/498-p1e-apply.py` :

- lit `498-reconciliation-p1f-annotations.csv` et **re-vérifie** que sa carte interne concorde 7/7 (assertion load-bearing) ;
- splitters byte-exact, cell-fill des seules colonnes `AIF_attackType`/`AIF_attackedNode` des 7 PK ;
- pre-state : les 7 PK **vides** (fill, pas overwrite) + **portent une skos** (back-fill skos-only) ;
- preuve de **byte-preservation** (0 mismatch hors les 2 cellules × 7 lignes), well-formedness 104 cols, BOM+CRLF préservés ;
- `--write` **gaté** (ai-01), backup `tmp/Fallacies-backup-pre-p1f.csv`.

**Baseline dynamique (write-driven).** Les 7 PK sont **disjoints** des 14 (1) + 7 (1b) + 7 (1c) + 4 (1d) + 5 (1e). Le script lit le compte réel :

- master actuel `c1ed77d2` (1+1b écrits) : `114 → 121` ;
- si 1c #779 écrit : `121 → 128` ;
- si 1c+1d écrits : `125 → 132` ;
- si 1c+1d+1e écrits : `130 → 137`.

Dry-run actuel (baseline 114) : `114 → 121`, delta **+105 B**, **0 mismatch**, **1409×104**, CRLF+BOM préservés, distribution 6 undermine / 1 undercut.

```bash
python tools/498-p1f-apply.py            # dry-run (0 write prod) — ce PR
python tools/498-p1f-apply.py --write    # APPLY 7 cellules (GATÉ — relais ai-01)
```

---

## 6. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (docs + apply-script dry-run uniquement — `git diff c1ed77d2 -- Cards/…/Taxonomy.csv` vide).
- ✅ **0 fabrication token #677** — aucun token AIF créé ; on type des lignes déjà skos-vettées.
- ✅ Code=truth : tokens/labels/`desc_fr` lus du CSV master `c1ed77d2` ; SUFFIX-ONLY confirmé par precedent-check (5/7 novel, 2/7 precedent undermine).
- ✅ Prior de suffixe **proscrit** ; distribution MIXTE = signature de famille hétérogène vérifiée (manipulation définitionnelle = undermine ; scheme analogique = undercut), chaque ligne modélisée, 4 MED flaguées avec alternatives explicites.
- ⚠️ **Divergence same-token pk808/pk33 documentée** (§3) — à vérifier par ai-01 (test du principe desc-driven).
- ❌ #674/#666/#596 non touchés (HOLD). Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante gated (relais ai-01), pas dans ce PR.

🤖 Worker po-2024 — réconciliation P1 tranche-1f (famille « Abus de langage », 7 SUFFIX-ONLY, MIXTE).
