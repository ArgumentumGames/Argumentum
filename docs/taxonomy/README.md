# docs/taxonomy — constats datés des chantiers de taxonomie

Ce dossier ne fait que grossir : chaque chantier y dépose ses rapports datés, ses
annotations CSV et ses scripts compagnons. Rien ne s'y supprime — chaque fichier
est une pièce. Ce README indexe la tranche **#498** (65 fichiers suivis). Il n'est
**pas encore** un index de record : la marque d'index viendra avec la dernière
tranche, quand les 168 fichiers du dossier seront tous indexés, conformément à
[`docs/agents/consolidation.md`](../agents/consolidation.md) (§ *Procedure* :
« Add the marker last »). En attendant, **rien ne tient cet index en CI** — une
ligne oubliée ne rougira rien ; la complétude ci-dessous est celle de sa PR.

Tranches restantes : #141, #192, #202, #299, #497, #499, #600, #847, #986 et les
fichiers non numérotés (`aif-structural-audit-method.md`, `family-gap-inventory.md`,
`i18n-hierarchy-apply-spec.md`, `subsubfamily-gap-inventory.md`,
`taxonomy-coherence-scan.md`).

## Où en est #498 — fermé le 17 novembre 2026

Indexé sur master `4d187340`. #498 est **clos** (ai-01, 17/11) : schéma imité (a)
complet — 1 207/1 408 nœuds décrits, 152/175 cartes — et OWL régénéré **sans IRI
changée** (PR #2295). Les tranches P3 (3.3.3, 6.2, 7.2.2, 7.2.3) sont mergées du
14 au 16/11 (#2270, #2279, #2285, #2290). La **couche générative « Layer C »**
(les ~1 259 feuilles sans signature skos ni attackType) n'a **jamais été entamée** :
le design de juillet 2026 ci-dessous est une pièce de décision, pas un plan en
attente d'exécution. La méthode de la réconciliation P1 (back-fill skos-only,
7 tranches, 0 token fabriqué, carte de nœuds ASPIC+ déterministe) reste **citée**
comme référence par [`docs/ontology/README.md`](../ontology/README.md) et les
notes de version v0.9.0 — en tant que pièce datée, ce qui est le rôle exact du
statut `historical` ici.

## La tranche #498 — 65 fichiers

Compte établi par `git ls-files docs/taxonomy | grep -c '^docs/taxonomy/498-'` → 65.

### Pilotes et phases du scale-up génératif (juin 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`498-aif-generative-pilot.md`](498-aif-generative-pilot.md) | 2026-06-16 | `historical` | Pilote génératif 18 sophismes (scheme + CQ + mécanisme) — la proposition gated qui a validé la méthode, source de tout le chantier |
| [`498-aif-scaleup-phase1.md`](498-aif-scaleup-phase1.md) | 2026-06-17 | `historical` | Phase 1 du scale-up (proposition gated, TRIPLE AIF) |
| [`498-aif-scaleup-phase2.md`](498-aif-scaleup-phase2.md) | 2026-06-19 | `historical` | Phase 2 du scale-up (proposition gated) |
| [`498-aif-scaleup-phase3.md`](498-aif-scaleup-phase3.md) | 2026-06-19 | `historical` | Phase 3 : Influence + Tricherie + Obstruction (proposition gated) |
| [`498-scaleup-phase1-annotations.csv`](498-scaleup-phase1-annotations.csv) | — | `historical` | Annotations machine de la phase 1 (compagnon du doc ci-dessus) |
| [`498-scaleup-phase2-annotations.csv`](498-scaleup-phase2-annotations.csv) | — | `historical` | Annotations machine de la phase 2 |
| [`498-scaleup-phase3-annotations.csv`](498-scaleup-phase3-annotations.csv) | — | `historical` | Annotations machine de la phase 3 |
| [`498-pilot-annotations.csv`](498-pilot-annotations.csv) | — | `historical` | Annotations machine du pilote génératif (4 champs par sophisme) |

### Cluster docs et annotations (juillet 2026)

Rapports cluster par cluster du chantier AIF (12 PR numérotées + 7 clusters d'observation). Chaque `.md` documente les familles traitées et leurs verdicts ; chaque `.csv` est la version machine lue par les scripts `tools/498-*` du même grain.

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`498-aif-fallacious-comparison-cluster.md`](498-aif-fallacious-comparison-cluster.md) | 2026-07-05 | `historical` | Cluster PR-1 « Fallacious comparison » |
| [`498-aif-faulty-comparison-cluster.md`](498-aif-faulty-comparison-cluster.md) | 2026-07-05 | `historical` | Cluster PR-2 « Faulty comparison » |
| [`498-aif-association-fallacy-cluster.md`](498-aif-association-fallacy-cluster.md) | 2026-07-05 | `historical` | Cluster PR-3 « Association fallacy » |
| [`498-aif-vague-definition-cluster.md`](498-aif-vague-definition-cluster.md) | 2026-07-05 | `historical` | Cluster PR-4 « Vague definition » |
| [`498-aif-arbitrary-definition-cluster.md`](498-aif-arbitrary-definition-cluster.md) | 2026-07-06 | `historical` | Cluster PR-5 « Acception arbitraire » |
| [`498-aif-inconsistent-definition-cluster.md`](498-aif-inconsistent-definition-cluster.md) | 2026-07-05 | `historical` | Cluster PR-6 « Inconsistent definition » |
| [`498-aif-amphibologie-cluster.md`](498-aif-amphibologie-cluster.md) | 2026-07-05 | `historical` | Cluster PR-7 « Amphibologie » |
| [`498-aif-narrative-ambiguity-insinuation-cluster.md`](498-aif-narrative-ambiguity-insinuation-cluster.md) | 2026-07-05 | `historical` | Cluster PR-8 « Ambiguïté narrative / insinuation » |
| [`498-aif-narrative-ambiguity-deception-cluster.md`](498-aif-narrative-ambiguity-deception-cluster.md) | 2026-07-05 | `historical` | Cluster PR-9 « Ambiguïté narrative / tromperie » |
| [`498-aif-equivoque-polysemie-cluster.md`](498-aif-equivoque-polysemie-cluster.md) | 2026-07-06 | `historical` | Cluster PR-10 « Équivoque / polysémie » |
| [`498-aif-equivoque-residual-cluster.md`](498-aif-equivoque-residual-cluster.md) | 2026-07-06 | `historical` | Cluster PR-11 « Équivoque / mécanismes résiduels » |
| [`498-aif-equivoque-reification-cluster.md`](498-aif-equivoque-reification-cluster.md) | 2026-07-06 | `historical` | Cluster PR-12 « Équivoque / réification » |
| [`498-aif-relativisme-abusif-cluster.md`](498-aif-relativisme-abusif-cluster.md) | 2026-07-10 | `historical` | Cluster d'observation « Relativisme abusif » |
| [`498-relativisme-abusif-annotations.csv`](498-relativisme-abusif-annotations.csv) | — | `historical` | Annotations du cluster relativisme |
| [`498-aif-complication-exageree-cluster.md`](498-aif-complication-exageree-cluster.md) | 2026-07-21 | `historical` | Cluster d'observation « Complication exagérée » (jumelle de 977, traitée par #2285) |
| [`498-complication-exageree-annotations.csv`](498-complication-exageree-annotations.csv) | — | `historical` | Annotations du cluster complication |
| [`498-aif-attaque-personnelle-cluster.md`](498-aif-attaque-personnelle-cluster.md) | 2026-07-22 | `historical` | Cluster d'observation « Attaque personnelle » |
| [`498-aif-attaque-personnelle-annotations.csv`](498-aif-attaque-personnelle-annotations.csv) | — | `historical` | Annotations du cluster attaque personnelle |
| [`498-aif-effet-cigogne-cluster.md`](498-aif-effet-cigogne-cluster.md) | 2026-07-22 | `historical` | Cluster d'observation « Effet cigogne » |
| [`498-aif-effet-cigogne-annotations.csv`](498-aif-effet-cigogne-annotations.csv) | — | `historical` | Annotations du cluster effet cigogne |
| [`498-aif-empoisonner-le-puits-cluster.md`](498-aif-empoisonner-le-puits-cluster.md) | 2026-07-22 | `historical` | Cluster d'observation « Empoisonner le puits » (ratifie 1352 sans scheme ; repris par #2290) |
| [`498-aif-empoisonner-le-puits-annotations.csv`](498-aif-empoisonner-le-puits-annotations.csv) | — | `historical` | Annotations du cluster empoisonner le puits |
| [`498-aif-inconsistance-cluster.md`](498-aif-inconsistance-cluster.md) | 2026-07-22 | `historical` | Cluster d'observation « Inconsistance » |
| [`498-aif-inconsistance-annotations.csv`](498-aif-inconsistance-annotations.csv) | — | `historical` | Annotations du cluster inconsistance |
| [`498-aif-operation-inappropriee-cluster.md`](498-aif-operation-inappropriee-cluster.md) | 2026-07-22 | `historical` | Cluster d'observation « Opération inappropriée » (sépare un sous-arbre en deux, repris par #2290) |
| [`498-aif-operation-inappropriee-annotations.csv`](498-aif-operation-inappropriee-annotations.csv) | — | `historical` | Annotations du cluster opération inappropriée |
| [`498-aif-proces-inconsistance-cluster.md`](498-aif-proces-inconsistance-cluster.md) | 2026-07-22 | `historical` | Cluster d'observation « Procès en incohérence » |
| [`498-aif-proces-inconsistance-annotations.csv`](498-aif-proces-inconsistance-annotations.csv) | — | `historical` | Annotations du cluster procès en incohérence |
| [`498-aif-sophisme-accident-cluster.md`](498-aif-sophisme-accident-cluster.md) | 2026-07-22 | `historical` | Cluster d'observation « Sophisme de l'accident » |
| [`498-aif-sophisme-accident-annotations.csv`](498-aif-sophisme-accident-annotations.csv) | — | `historical` | Annotations du cluster accident |

### Réconciliation P1 — back-fill skos-only, 7 tranches (juillet 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`498-reconciliation-p1.md`](498-reconciliation-p1.md) | 2026-07-10 | `historical` | Proposition P1 : skos déjà présent → colonnes d'attaque dérivées |
| [`498-reconciliation-p1-annotations.csv`](498-reconciliation-p1-annotations.csv) | — | `historical` | Annotations machine de P1 (lues par `tools/498-p1-apply.py`) |
| [`498-reconciliation-p1b.md`](498-reconciliation-p1b.md) | 2026-07-11 | `historical` | Tranche 1b (PREC-TIE + premières familles) |
| [`498-reconciliation-p1b-annotations.csv`](498-reconciliation-p1b-annotations.csv) | — | `historical` | Annotations de la tranche 1b |
| [`498-reconciliation-p1c.md`](498-reconciliation-p1c.md) | 2026-07-11 | `historical` | Tranche 1c (suffix-only) |
| [`498-reconciliation-p1c-annotations.csv`](498-reconciliation-p1c-annotations.csv) | — | `historical` | Annotations de la tranche 1c |
| [`498-reconciliation-p1d.md`](498-reconciliation-p1d.md) | 2026-07-11 | `historical` | Tranche 1d (suffix-only, familles suivantes) |
| [`498-reconciliation-p1d-annotations.csv`](498-reconciliation-p1d-annotations.csv) | — | `historical` | Annotations de la tranche 1d |
| [`498-reconciliation-p1e.md`](498-reconciliation-p1e.md) | 2026-07-11 | `historical` | Tranche 1e (suffix-only) |
| [`498-reconciliation-p1e-annotations.csv`](498-reconciliation-p1e-annotations.csv) | — | `historical` | Annotations de la tranche 1e |
| [`498-reconciliation-p1f.md`](498-reconciliation-p1f.md) | 2026-07-11 | `historical` | Tranche 1f (suffix-only) |
| [`498-reconciliation-p1f-annotations.csv`](498-reconciliation-p1f-annotations.csv) | — | `historical` | Annotations de la tranche 1f |
| [`498-reconciliation-p1g.md`](498-reconciliation-p1g.md) | 2026-07-11 | `historical` | Tranche 1g (dernière de P1) |
| [`498-reconciliation-p1g-annotations.csv`](498-reconciliation-p1g-annotations.csv) | — | `historical` | Annotations de la tranche 1g |
| [`498-reconciliation-p1-closure.md`](498-reconciliation-p1-closure.md) | 2026-07-12 | `historical` | Clôture P1 : 93 → 145, 0 token fabriqué, audit d'ancres #770, carte de nœuds ASPIC+ déterministe (#707 §4) — **la méthodologie citée par `docs/ontology/README.md` et les notes v0.9.0** |

### Réconciliation P2 — « Abus de langage » (août 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`498-reconciliation-p2.md`](498-reconciliation-p2.md) | 2026-08-29 | `historical` | P2 : sérialisation SKOS des 46 rangées attack-only « Abus de langage » |
| [`498-reconciliation-p2-annotations.csv`](498-reconciliation-p2-annotations.csv) | — | `historical` | Annotations machine de P2 |
| [`498-reconciliation-p2-apply.py`](498-reconciliation-p2-apply.py) | — | `historical` | Script one-shot d'application P2 (le verrou de vocabulaire qu'il portait est cité par `FallacyAifVocabularyLockTests.cs` comme précédent one-shot) |
| [`498-reconciliation-p2-apply-test.py`](498-reconciliation-p2-apply-test.py) | — | `historical` | Contrôle du script P2 (fixtures) |

### P3, état de tranche et audits (août–septembre 2026, juillet)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`498-reconciliation-p3a-palier1.md`](498-reconciliation-p3a-palier1.md) | 2026-08-30 | `historical` | P3 palier 1 : les 6 têtes à ancre de famille |
| [`498-reconciliation-p3a-palier1-annotations.csv`](498-reconciliation-p3a-palier1-annotations.csv) | — | `historical` | Annotations du palier 1 |
| [`498-p3-abd-tranche-etat-2026-09-14.md`](498-p3-abd-tranche-etat-2026-09-14.md) | 2026-09-14 | `historical` | Établit : tranche « Abus de langage » **faite et appliquée** (PR #1234 : skos 70 → 116, attack-only 75 → 29) ; P3 gelé à sa date — le gel a sauté ensuite, tranches mergées 14-16/11 |
| [`498-audit-18-anchors.csv`](498-audit-18-anchors.csv) | — | `historical` | Audit des 18 ancres (verdicts + notes) — cible de `tools/498-audit-anchors.py` |
| [`498-verification-audit.md`](498-verification-audit.md) | 2026-07-05 | `historical` | Audit de vérification du chantier (code=truth, adverse) |
| [`498-coverage-status.md`](498-coverage-status.md) | 2026-07-10 | `historical` | Snapshot à deux couches (attaque 93 · skos 70 · union 145 · intersection 18) — l'état final (schéma (a), 1 207/1 408) vit sur #498 |
| [`498-layerC-pilot-design.md`](498-layerC-pilot-design.md) | 2026-07-12 | `historical` | Design gated de la passe générative Layer C (~1 259 feuilles) — **jamais exécutée**, GO/NO-GO tranché par la clôture de #498 sans elle |
| [`498-aif-closure.md`](498-aif-closure.md) | 2026-06-19 | `superseded` | Évaluation de clôture de juin — recommandait déjà de fermer ; le chantier a continué jusqu'en novembre, et l'état de clôture réel vit sur [#498](https://github.com/ArgumentumGames/Argumentum/issues/498) (clos le 17/11/2026) |
