# docs/taxonomy — constats datés des chantiers de taxonomie

Ce dossier ne fait que grossir : chaque chantier y dépose ses rapports datés, ses
annotations CSV et ses scripts compagnons. Rien ne s'y supprime — chaque fichier
est une pièce. Ce README indexe les tranches **#498** (65 fichiers), **#141**
(21 fichiers) et **#499** (37 fichiers). Il n'est **pas encore** un index de record : la marque d'index
viendra avec la dernière tranche, quand les 168 fichiers du dossier seront tous
indexés, conformément à
[`docs/agents/consolidation.md`](../agents/consolidation.md) (§ *Procedure* :
« Add the marker last »). En attendant, **rien ne tient cet index en CI** — une
ligne oubliée ne rougira rien ; la complétude ci-dessous est celle de sa PR.

Tranches restantes : #192, #202, #299, #497, #600, #847, #986 et les
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

## Où en est #141 — chantier clos, module de hiérarchie toujours vivant

Indexé sur master `6b7afbb9`. #141 (« Iterate on non-card taxonomy nodes ») est
**clos** (COMPLETED). Deux choses y cohabitent et ne doivent pas être confondues :

- **Le chantier AIF de renvois croisés (juin–juillet 2026)** — recensement des
  nœuds non-carte, pilote génératif dry-run (Stage-1 gpt-5.5), tri des WARN
  `bad_map:*`, adjudication d'expert, adaptation CrossLink/DatasetUpdater. Tout
  ceci est un **instantané daté d'un chantier fermé** : rapports et sidecars CSV
  au statut `historical`. Leur régénération serait un nouvel instrument, pas une
  reproduction — ils gardent la date et le commit auxquels ils ont été mesurés.
- **Le module de hiérarchie `141-hierarchy.py`** — **code vivant** (`current`),
  repris par le grain 1b de #498 (06/11). Il est chargé (par
  `importlib.import_module("141-hierarchy")`, le nom à tiret n'étant pas
  importable par un `import` simple) par ses jumeaux `141-aif-explore.py`,
  `141-aif-fullscale.py`, `141-aif-stage1.py` et par son propre
  [`141-hierarchy-self-test.py`](141-hierarchy-self-test.py), qui tourne et
  passe (**25/25**) ; le self-test n'est **pas câblé en CI** — le relancer à la
  main. ⚠️ **Ne pas confondre** : [`tools/aif-attack-graph-export.py`](../../tools/aif-attack-graph-export.py)
  n'importe **pas** ce module — il **réimplémente** la même logique `path` en
  ligne (`to_comma`, l. 179) et ne fait que **citer** le rapport en commentaire.
  Le statut `current` du module tient donc à ce qu'il est **exécutable et
  vérifié** (self-test vert), non à un usage par un outil de `tools/`.
  Sa règle unique : la hiérarchie se lit dans la
  colonne **`path`** (pointée, un segment par niveau, 1 408 uniques, 0 orphelin) ;
  la forme coma reste la clé de **rendu** (`dot2comma`), jamais l'inverse — la
  reconstruction chiffre par chiffre depuis `decimal_path` fabriquait un parent
  inexistant (`5,123224`, rang 24).

## La tranche #141 — 21 fichiers

Compte établi par `git ls-files docs/taxonomy | grep -c '^docs/taxonomy/141-'` → 21.

### Code vivant — hiérarchie lue dans `path`

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`141-hierarchy.py`](141-hierarchy.py) | 2026-11-06 | `current` | **Module partagé** des primitives de hiérarchie (`path_parent`, `dot2comma`, `comma_parent_DEFECTUEUX` pour contrôle inverse) — chargé par `importlib` par ses jumeaux `141-aif-*.py` et son self-test ; **exécutable et vérifié** (self-test 25/25). NB : `tools/aif-attack-graph-export.py` réimplémente la logique inline, ne l'importe pas |
| [`141-hierarchy-self-test.py`](141-hierarchy-self-test.py) | 2026-11-06 | `current` | Contrôle du module ci-dessus — **25/25** (mesuré 23/11), non câblé en CI ; se lance par `python -I docs/taxonomy/141-hierarchy-self-test.py` |
| [`141-hierarchy-path-2026-11-06.md`](141-hierarchy-path-2026-11-06.md) | 2026-11-06 | `historical` | Rapport du grain 1b : la mesure qui a imposé la lecture par `path` (écart rangée 824, `depth` faux sur 2 rangées) — pièce datée, le module est la référence vivante |

### Recensement et exploration (juin 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`141-noncard-enrichment-census.md`](141-noncard-enrichment-census.md) | 2026-06-29 | `historical` | Recensement des nœuds non-carte + proposition d'enrichissement — le point de départ du chantier |
| [`141-noncard-census.py`](141-noncard-census.py) | 2026-06 | `historical` | Script compagnon du recensement (one-shot) |
| [`141-aif-explore.py`](141-aif-explore.py) | 2026-07 | `historical` | Exploration structurelle read-only des candidats AIF (résumé sur stdout, 0 écriture `Cards/`) |

### Stage-1 — pilote génératif dry-run et montée à l'échelle (juillet 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`141-aif-stage1-report.md`](141-aif-stage1-report.md) | 2026-07-01 | `historical` | Pilote Stage-1 (gpt-5.5, dry-run) — candidats de renvois croisés sur nœuds non-carte |
| [`141-aif-stage1.py`](141-aif-stage1.py) | 2026-07 | `historical` | Script du pilote dry-run — n'écrit que le sidecar `docs/taxonomy/`, 0 écriture `Cards/` |
| [`141-aif-fullscale-report.md`](141-aif-fullscale-report.md) | 2026-07-01 | `historical` | Rapport full-scale : couverture et WARN sur l'ensemble des nœuds non-carte |
| [`141-aif-fullscale.py`](141-aif-fullscale.py) | 2026-07 | `historical` | Passage du pilote (28 nœuds) à tous les non-carte (1 232) — dry-run |
| [`141-aif-candidates-sample.csv`](141-aif-candidates-sample.csv) | — | `historical` | Sidecar : échantillon de 148 candidats Stage-1 |
| [`141-aif-candidates-fullscale.csv`](141-aif-candidates-fullscale.csv) | — | `historical` | Sidecar : 6 160 candidats Stage-1 à l'échelle |

### Stage-2/3 — diff, tri des WARN, adjudication (juillet 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`141-aif-stage2-diff.py`](141-aif-stage2-diff.py) | 2026-07 | `historical` | Compare le sidecar Stage-1 full-scale aux référents (contrôle de diff) |
| [`141-aif-stage2-diff.csv`](141-aif-stage2-diff.csv) | — | `historical` | Sortie du diff Stage-2 (16 lignes) |
| [`141-aif-warn-triage.md`](141-aif-warn-triage.md) | 2026-07-01 | `historical` | Tri des WARN `bad_map:*` (dry-run, no auto-apply) |
| [`141-aif-warn-triage.py`](141-aif-warn-triage.py) | 2026-07 | `historical` | Helper de tri des 87 nœuds non-carte dont la proposition Stage-1 utilisait un schéma AIF |
| [`141-aif-warn-triage.csv`](141-aif-warn-triage.csv) | — | `historical` | Sidecar du tri (87 lignes) |
| [`141-aif-stage3-adjudication.md`](141-aif-stage3-adjudication.md) | 2026-07-01 | `historical` | Paquet d'adjudication d'expert Stage-3 (dry-run, no auto-apply) |

### CrossLink, adaptation et clôture (juillet 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`141-crosslink-datasetupdater-adaptation.md`](141-crosslink-datasetupdater-adaptation.md) | 2026-07-04 | `historical` | Proposition additive gated d'adaptation CrossLink/DatasetUpdater |
| [`141-crosslink-sample-run.md`](141-crosslink-sample-run.md) | 2026-07-04 | `historical` | Exécution d'échantillon CrossLink (sortie réelle, curatée depuis #626) |
| [`141-closure-recommendation.md`](141-closure-recommendation.md) | 2026-07-01 | `historical` | Recommandation de clôture du chantier #141 |

## Où en est #499 — parité relationnel/AIF des Virtues, chantier clos

Indexé sur master `93a218e5`. #499 (« amener la taxonomie Virtues à parité
relationnel + AIF ») est **clos** (COMPLETED). La tranche entière est un
**instantané daté d'un chantier fermé** : les 37 fichiers sont des rapports de
batch, des sidecars d'annotation et des specs gated, tous de **juin–juillet
2026**. **Rien d'exécutable dans la tranche** (25 `.md` + 12 `.csv`, aucun `.py`) ;
**aucun outil ne lit ces sidecars en entrée** — l'apply (`tools/499-virtues-*-apply.py`,
`tools/989-*`) lit le CSV `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`
directement, et [`tools/validate_taxonomy_annotations.md`](../../tools/validate_taxonomy_annotations.md)
les cite comme **exemples mesurés**, pas comme entrées. Les deux références
externes (`docs/RELEASE-VALIDATION-v0.9.0.md` → `499-virtues-prod-write-spec.md` ;
`docs/investigations/2026-07-03-654-*.md` → `499-virtues-scaleup-phase2-raisonnement-b.md`
§86) pointent un **daté**, elles ne le rendent pas gouvernant.

## La tranche #499 — 37 fichiers

Compte établi par `git ls-files docs/taxonomy | grep -c '^docs/taxonomy/499-'` → 37.
Découpage mesuré : `…499-*.md` → 25, `…499-*.csv` → 12.

### Pilote, plan, spec et clôture (juin 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`499-virtues-parity-pilot.md`](499-virtues-parity-pilot.md) | 2026-06-16 | `historical` | Pilote de parité relationnel/AIF des Virtues (proposition gated) — la graine du chantier |
| [`499-virtues-scaleup-phase1.md`](499-virtues-scaleup-phase1.md) | 2026-06-17 | `historical` | Scale-up phase 1 : les 18 sous-familles de profondeur 2 (proposition gated) |
| [`499-virtues-parity-closure.md`](499-virtues-parity-closure.md) | 2026-06-23 | `historical` | Évaluation de clôture de la parité (couverture + validation) |
| [`499-virtues-prod-write-spec.md`](499-virtues-prod-write-spec.md) | 2026-06-23 | `historical` | Spec d'écriture prod (brouillon, non-prod, gated sur le contenu owner) — **ground-truth 223 rangées × 66 colonnes** cité par `RELEASE-VALIDATION-v0.9.0` |
| [`499-virtues-backfill-mechanical-plan.md`](499-virtues-backfill-mechanical-plan.md) | 2026-07-07 | `historical` | Plan mécanique de back-fill 222 rangées (Option A, HOLD prod) |

### Scale-up phase 2 — un rapport par batch (juin 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`499-virtues-scaleup-phase2.md`](499-virtues-scaleup-phase2.md) | 2026-06-18 | `historical` | Batch 1 : Langage exact, feuilles de profondeur 3-4 |
| [`499-virtues-scaleup-phase2-rigmath.md`](499-virtues-scaleup-phase2-rigmath.md) | 2026-06-18 | `historical` | Batch 2 : Rigueur mathématique, profondeur 3… |
| [`499-virtues-scaleup-phase2-honnetete.md`](499-virtues-scaleup-phase2-honnetete.md) | 2026-06-18 | `historical` | Batch 3 : Honnêteté intellectuelle, profondeur 3… |
| [`499-virtues-scaleup-phase2-presentation.md`](499-virtues-scaleup-phase2-presentation.md) | 2026-06-18 | `historical` | Batch 4 : Présentation intègre, profondeur 3-4 |
| [`499-virtues-scaleup-phase2-argument.md`](499-virtues-scaleup-phase2-argument.md) | 2026-06-18 | `historical` | Batch 5 : Argument pertinent, profondeur 3…6 |
| [`499-virtues-scaleup-phase2-echange-a.md`](499-virtues-scaleup-phase2-echange-a.md) | 2026-06-18 | `historical` | Batch 6a : Échange enrichissant, profondeur 3… |
| [`499-virtues-scaleup-phase2-echange-b.md`](499-virtues-scaleup-phase2-echange-b.md) | 2026-06-19 | `historical` | Batch 6b : Échange enrichissant, profondeur 3… |
| [`499-virtues-scaleup-phase2-raisonnement-a.md`](499-virtues-scaleup-phase2-raisonnement-a.md) | 2026-06-19 | `historical` | Batch 7a : Raisonnement valide, profondeur 3…6 |
| [`499-virtues-scaleup-phase2-raisonnement-b.md`](499-virtues-scaleup-phase2-raisonnement-b.md) | 2026-06-19 | `historical` | Batch 7b : Raisonnement valide, profondeur 6…7 — **§86 : définition des mnémoniques syllogistiques** (Barbara, Celarent…), citée par `docs/investigations/2026-07-03-654-*.md` |
| [`499-virtues-scaleup-phase2-raisonnement-c.md`](499-virtues-scaleup-phase2-raisonnement-c.md) | 2026-06-19 | `historical` | Batch 7c : Raisonnement valide, profondeur 4…5 |

### Clusters AIF des Virtues (juillet 2026)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`499-aif-raisonnement-causalite-cluster.md`](499-aif-raisonnement-causalite-cluster.md) | 2026-07-07 | `historical` | Cluster PR-1 : Raisonnement valide / Causalités bien établies |
| [`499-aif-honnetete-fidelite-aux-faits-cluster.md`](499-aif-honnetete-fidelite-aux-faits-cluster.md) | 2026-07-07 | `historical` | Cluster PR-2 : Honnêteté intellectuelle / Fidélité aux faits |
| [`499-aif-echange-respect-de-la-personne-cluster.md`](499-aif-echange-respect-de-la-personne-cluster.md) | 2026-07-07 | `historical` | Cluster PR-3 : Échange enrichissant / Respect de la personne |
| [`499-aif-langage-definitions-claires-cluster.md`](499-aif-langage-definitions-claires-cluster.md) | 2026-07-07 | `historical` | Cluster PR-4 : Langage exact / Définitions claires |
| [`499-aif-rigmath-generalisations-justifiees-cluster.md`](499-aif-rigmath-generalisations-justifiees-cluster.md) | 2026-07-07 | `historical` | Cluster PR-5 : Rigueur mathématique / Généralisations justifiées |
| [`499-aif-presentation-equilibre-emotionnel-cluster.md`](499-aif-presentation-equilibre-emotionnel-cluster.md) | 2026-07-07 | `historical` | Cluster PR-6 : Présentation intègre / Équilibre émotionnel |
| [`499-aif-argument-fonde-cluster.md`](499-aif-argument-fonde-cluster.md) | 2026-07-07 | `historical` | Cluster PR-7 : Argument pertinent / Argument fondé |
| [`499-aif-premisses-fiables-cluster.md`](499-aif-premisses-fiables-cluster.md) | 2026-07-07 | `historical` | Cluster PR-8 : Argument pertinent / Prémisses fiables |
| [`499-aif-presentation-humour-cluster.md`](499-aif-presentation-humour-cluster.md) | 2026-07-07 | `historical` | Cluster PR-9 : Présentation intègre / Rhétorique et humour |
| [`499-aif-interpretation-juste-cluster.md`](499-aif-interpretation-juste-cluster.md) | 2026-07-08 | `historical` | Cluster PR-10 : Interprétation juste (famille 1.3) |

### Sidecars d'annotation machine (12 CSV)

| Fichier | Lignes | Statut | Ce que c'est |
|---|---:|---|---|
| [`499-pilot-annotations.csv`](499-pilot-annotations.csv) | 10 | `historical` | Annotations du pilote (10 rangées) |
| [`499-scaleup-phase1-annotations.csv`](499-scaleup-phase1-annotations.csv) | 18 | `historical` | Annotations de la phase 1 (18 rangées, #510) |
| [`499-scaleup-phase2-annotations.csv`](499-scaleup-phase2-annotations.csv) | 14 | `historical` | Annotations du batch 1 |
| [`499-scaleup-phase2-rigmath-annotations.csv`](499-scaleup-phase2-rigmath-annotations.csv) | 16 | `historical` | Annotations du batch 2 |
| [`499-scaleup-phase2-honnetete-annotations.csv`](499-scaleup-phase2-honnetete-annotations.csv) | 23 | `historical` | Annotations du batch 3 |
| [`499-scaleup-phase2-presentation-annotations.csv`](499-scaleup-phase2-presentation-annotations.csv) | 21 | `historical` | Annotations du batch 4 |
| [`499-scaleup-phase2-argument-annotations.csv`](499-scaleup-phase2-argument-annotations.csv) | 29 | `historical` | Annotations du batch 5 |
| [`499-scaleup-phase2-echange-a-annotations.csv`](499-scaleup-phase2-echange-a-annotations.csv) | 18 | `historical` | Annotations du batch 6a |
| [`499-scaleup-phase2-echange-b-annotations.csv`](499-scaleup-phase2-echange-b-annotations.csv) | 22 | `historical` | Annotations du batch 6b |
| [`499-scaleup-phase2-raisonnement-a-annotations.csv`](499-scaleup-phase2-raisonnement-a-annotations.csv) | 22 | `historical` | Annotations du batch 7a |
| [`499-scaleup-phase2-raisonnement-b-annotations.csv`](499-scaleup-phase2-raisonnement-b-annotations.csv) | 23 | `historical` | Annotations du batch 7b |
| [`499-scaleup-phase2-raisonnement-c-annotations.csv`](499-scaleup-phase2-raisonnement-c-annotations.csv) | 6 | `historical` | Annotations du batch 7c |
