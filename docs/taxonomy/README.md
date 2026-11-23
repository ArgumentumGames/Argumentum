# docs/taxonomy — constats datés des chantiers de taxonomie

Ce dossier ne fait que grossir : chaque chantier y dépose ses rapports datés, ses
annotations CSV et ses scripts compagnons. Rien ne s'y supprime — chaque fichier
est une pièce. Ce README indexe **146 des 168 fichiers** du dossier : les tranches
**#498** (65 fichiers), **#141** (21), **#499** (37) et **#847** (15), puis les
tranches d'un seul fichier **#202**, **#299**, **#986** et les cinq documents non
numérotés (8 fichiers). Il n'est **pas encore** un index de record : la marque d'index
viendra avec la dernière tranche, quand les 168 fichiers du dossier seront tous
indexés, conformément à
[`docs/agents/consolidation.md`](../agents/consolidation.md) (§ *Procedure* :
« Add the marker last »). En attendant, **rien ne tient cet index en CI** — une
ligne oubliée ne rougira rien ; la complétude ci-dessous est celle de sa PR.

Tranches restantes : #192 (8 fichiers), #497 (9) et #600 (5) — 22 fichiers.

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

## La tranche #847 — 15 fichiers

Compte établi par `git ls-files docs/taxonomy | grep -c '^docs/taxonomy/847-'` → 15.

#847 (« AIF two-layer structural homogeneity audit ») est **clos** (COMPLETED le
7 septembre 2026). La tranche est la **chaîne complète d'un audit contradictoire** :
une mesure des couches (a)+(b), **deux** instruments de couche (c) — chacun conçu
pour *disculper* la structure plutôt que pour la condamner —, une synthèse-verdict
d'ai-01, une liste de réparation, puis **une re-passe de release** qui refait la
mesure sur Fallacies et la tente pour la première fois sur Virtues. **7 `.md`,
7 `.csv`, 1 `.py`.** Toutes les pièces portent « MEASURE, not verdict » en tête
**sauf** la synthèse, qui est le verdict et le dit.

Aucune ligne n'est `current`, et la raison est **mesurée**, pas de principe : la
tranche est close, et le seul fichier exécutable ne sert qu'à réécrire ses propres
sidecars. `git grep -n "847-virtues-two-layer" -- ':!docs/taxonomy'` rend **0
hit**, et il n'existe aucun `tools/847-*` : rien dans `tools/` ni en CI ne
l'appelle. Il reste rejouable — il lit les deux CSV de prod et écrit les deux
sidecars datés du 28/08 —, ce qui ne fait pas de lui un organe : c'est le
**compagnon d'une passe datée**. Il n'a **pas** été rejoué pour écrire cet index
(c'est un générateur : le relancer réécrirait des fichiers commités, geste hors
du périmètre d'un index).

### Mesure des couches (a) et (b) — juillet 2026

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`847-acompte-homogeneity.md`](847-acompte-homogeneity.md) | 2026-07-22 | `historical` | Acompte des couches (a)+(b) sur les **145 feuilles entièrement modélisées** — `87 undercut / 53 undermine / 5 rebut` ; en-tête « MEASURE ONLY, not a verdict », 0 écriture CSV prod |
| [`847-acompte.csv`](847-acompte.csv) | 2026-07-22 | `historical` | Sidecar de l'acompte — **41** rangées (nœuds mesurés, 145 feuilles en prod) |
| [`847-acompte-projected.csv`](847-acompte-projected.csv) | 2026-07-22 | `historical` | Variante **illustrative** : les 145 feuilles de prod **plus 8 propositions** de feuilles du cluster Layer-C **qui ne sont pas dans le CSV de prod** — **50** rangées ; à ne pas lire comme un état |

### Couche (c) — les deux moitiés de l'instrument de disculpe — juillet 2026

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`847-layerc-reference-reachability.md`](847-layerc-reference-reachability.md) | 2026-07-23 | `historical` | Couche (c) §2.2 — accessibilité des références : la moitié qui devait séparer un **défaut de groupement** (axe A, à arbitrer) d'un **artefact de tradition** (axe C, la faute de personne) |
| [`847-layerc-reference-reachability.csv`](847-layerc-reference-reachability.csv) | 2026-07-23 | `historical` | Sidecar de la couche (c) §2.2 — **90** rangées |
| [`847-layerc-reference-reachability-refs.csv`](847-layerc-reference-reachability-refs.csv) | 2026-07-23 | `historical` | Le détail **par référence** — **2 580** rangées, compartimentées en `{wikipedia-stable, archive-repointed, dictionary-known, alive, dead, unknown, unparseable}` (liste blanche déterministe + sonde HEAD en lecture seule pour la longue traîne) |
| [`847-layerc-label-distortion.md`](847-layerc-label-distortion.md) | 2026-07-23 | `historical` | Couche (c) §2.1 — la **contrainte d'étiquetage** des 39 nœuds internes ; deux proxys sémantiques testés et **non discriminants** (`dispersion_fr` saturée 0,94-1,00, `n_en_groupings = 1` partout) ⇒ `stretched`/`narrowed` restent non mesurés |
| [`847-layerc-label-distortion.csv`](847-layerc-label-distortion.csv) | 2026-07-23 | `historical` | Sidecar de la couche (c) §2.1 — **41** rangées, `label_fit ∈ {borrowed, unflagged}` (`unflagged` = aucun drapeau mécanique, **pas** une validation de portée) |

### Verdict et backlog de réparation — juillet 2026

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`847-synthesis-verdict.md`](847-synthesis-verdict.md) | 2026-07-25 | `historical` | La **synthèse-verdict d'ai-01** : « l'arbre survit à l'audit, aucune restructuration proposée ». Sur les **39** nœuds de groupement : 4 cohérents · 1 **pont** (*Ad hominem*, à annoter, à ne pas aplatir) · **28** résistances de matière · **6** candidats axe A dont **2 seulement** passent une porte de confiance. Constat dominant : **65,5 %** des 145 feuilles n'ont aucun jeton de schéma AIF natif |
| [`847-reference-repair-worklist.md`](847-reference-repair-worklist.md) | 2026-07-26 | `historical` | La moitié **actionnable** du §5 — re-sonde en **GET**, qui réfute la réserve de la synthèse : la sonde HEAD sur-comptait (rationalwiki.org rend 503 en HEAD, 200 en GET) |
| [`847-reference-repair-worklist.csv`](847-reference-repair-worklist.csv) | 2026-07-26 | `historical` | Le backlog machine — **134** rangées, la borne haute des références mortes (5,2 %) |

### Re-passe de release — 28 août 2026 (Fallacies + première passe Virtues)

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`847-virtues-two-layer.md`](847-virtues-two-layer.md) | 2026-08-28 | `historical` | Re-passe déclenchée par la release du 25/08 : contrôle de dérive Fallacies — **0 dérive**, 145 feuilles et `87/53/5` **identiques** aux chiffres de juillet, tables et nominations du verdict valides telles quelles — plus la **première** passe sur Virtues (222 feuilles `206/13/3`, entropie normalisée 0,267, homogénéité 0,928 ; **la couche (b) ne se transpose pas** aux Virtues, les tags y sont dégénérés dans les deux lectures) |
| [`847-virtues-two-layer.py`](847-virtues-two-layer.py) | 2026-08-28 | `historical` | Le script compagnon de la re-passe — lit les deux CSV de prod, réimprime les mêmes normalisations que juillet et réécrit ses deux sidecars (une ligne de commentaire en tête, puis l'en-tête) |
| [`847-fallacies-rerun.csv`](847-fallacies-rerun.csv) | 2026-08-28 | `historical` | Sortie machine du script — Fallacies, **91** rangées |
| [`847-virtues-pass.csv`](847-virtues-pass.csv) | 2026-08-28 | `historical` | Sortie machine du script — Virtues, **91** rangées |

La méthode que ces six pièces appliquent — 3 axes A/B/C, table de décision §4,
règle de séparation mesure / verdict — vit dans
[`aif-structural-audit-method.md`](aif-structural-audit-method.md), qui relève de
la tranche des fichiers non numérotés ; le chantier porteur est #498.

### Non établi

- **Les 134 références mortes ont-elles été réparées ?** L'index ne le dit pas et
  ne l'établit pas. Aucune pièce du dossier ne porte de lot de réparation
  postérieur au 26/07, mais une réparation a pu se faire sous un autre nom :
  l'instrument employé (`git grep` sur « 134 dead », « dead references ») ne
  mesure que l'absence de ce **motif**, pas l'absence du travail.
- Le contenu des sidecars au-delà de leur compte de rangées : la tranche est
  indexée, pas ré-auditée. Un chantier clos ne se re-mesure pas pour l'indexer.
- Le marqueur `<!-- index-of-record -->` reste absent (dernière tranche
  seulement) : la complétude ci-dessus est celle de cette PR.

## Les tranches d'un fichier (#202, #299, #986) et les cinq documents non numérotés — 8 fichiers

Compte établi par
`git ls-files docs/taxonomy | grep -E '^docs/taxonomy/(202|299|986)-|^docs/taxonomy/(aif-structural-audit-method|family-gap-inventory|i18n-hierarchy-apply-spec|subsubfamily-gap-inventory|taxonomy-coherence-scan)\.md$' | wc -l`
→ 8.

Ce lot rassemble deux familles que rien ne rapprochait jusqu'ici : **trois tranches
d'un seul fichier** (#202, #299, #986), et **cinq documents non numérotés** qui
n'appartiennent à aucun chantier — le dossier de méthode de l'audit #847, deux
inventaires de qualité de données et leur spec d'application consolidée, et une
passe de cohérence terminologique de la sign-off de release.

### Le gap des 48 rangées est fermé — et c'est mesurable

Les trois documents `family-gap-inventory` / `subsubfamily-gap-inventory` /
`i18n-hierarchy-apply-spec` décrivent **le même défaut** : 48 rangées du CSV
Fallacies dont une colonne de groupement anglaise était vide (propagation i18n
manquante). Un lecteur de l'index doit pouvoir savoir s'il est encore ouvert.
Mesure du 23/11 sur le blob `origin/master` (`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`,
**1 408** rangées ; profondeur = nombre de segments de `path`) :

| Colonne EN | Rangées vides | Profondeur des rangées vides |
|---|---:|---|
| `Family` | **0** | — |
| `Subfamily` | **8** | **8** à profondeur 1 |
| `Subsubfamily` | **29** | **8** à profondeur 1 + **21** à profondeur 2 |

Le CSV porte par ailleurs exactement **8** nœuds de profondeur 1 et **21** de
profondeur 2. Un nœud de profondeur 1 n'a pas de `Subfamily` ; un nœud de
profondeur ≤ 2 n'a pas de `Subsubfamily` : les populations **coïncident
exactement** avec les cellules vides. Il ne reste donc **aucune cellule vide qui
soit un défaut** — le gap des 48 rangées a été **appliqué**, et les trois
documents sont des pièces datées, pas un travail en attente.

> Ce que cette mesure n'établit pas : elle ne dit pas *quand* ni par quelle PR les
> 144 cellules ont été remplies. Elle établit l'état, pas l'historique.

### Les trois tranches d'un fichier

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`202-bulk-en-scoping.md`](202-bulk-en-scoping.md) | 2026-07-01 | `historical` | Recensement **read-only** du « bulk EN » de #202 (master `18b4d023`) — conclusion : c'est un **dispatch périmé**. Les items de la phase 2 sont faits ou ne sont pas des trous de traduction (`Simple_name_en` 61/1408 remplis contre 40 sources FR : la source est vide, pas la traduction ; Scenarii 167/167 ; Virtues 8 langues ; Rules PT corrigé). #202 est **CLOSED/COMPLETED** |
| [`299-llm-quality-delta-investigation.md`](299-llm-quality-delta-investigation.md) | 2026-07-23 | `historical` | Investigation **read-only** du **delta** entre le banc #299 (clos le 17/05) et l'état de juillet (pivot gpt-5.5, chemin `/v1/responses` de #852 câblé mais dormant) — **0 run live** (`DatasetUpdater.Enabled=false` sur les 48 configs, gel T&A). Entrée de la décision d'activation post-T&A. #299 est **CLOSED/COMPLETED** |
| [`986-c1-note-cadrage-pre-draft-3-postures.md`](986-c1-note-cadrage-pre-draft-3-postures.md) | 2026-09-21 | `current` | **Pré-draft** du principe générateur du deck Vertus : la mesure (les titres des 131 cartes, le miroir qui atteint 33 % du deck Sophismes sans orphelin) puis **trois postures** — dispositions (A), standards (B), deux couches déclarées (C) — et un test d'inclusion applicable aux trois. ⚠️ **Aucune posture n'y est retenue** : #986 est **OPEN**, et son dernier commentaire (ai-01, 08/11) demande l'avis de Thomas. Ce statut dit « seule pièce écrite de la question, rien ne la remplace » — **pas** « décision prise » |

### Le dossier de méthode de l'audit AIF

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`aif-structural-audit-method.md`](aif-structural-audit-method.md) | 2026-07-22 | `historical` | Le **document de méthode et de gouvernance** de l'audit #847 : conversation de conception jsboige × ai-01, les **trois axes d'arbitraire** (A groupement, B matière, C tradition), la table de décision §4 et le **principe de gouvernance §5** (mesure ≠ verdict, tout changement structurel gaté par la ratification). Committé « exprès pour que le raisonnement survive à la condensation ». Il est cité par les six pièces de la [tranche #847](#la-tranche-847--15-fichiers) ; `git grep` hors `docs/taxonomy` → **0** référence, et #847 est clos |

### Les inventaires de qualité de données et leur spec

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`family-gap-inventory.md`](family-gap-inventory.md) | 2026-07-06 | `historical` | Inventaire du gap #712 : **48 rangées** à colonne `Family` (EN) vide, méthode code=truth, **0 écriture** au CSV de prod (gel `Cards/`) — proposition prête pour une application post-tag |
| [`subsubfamily-gap-inventory.md`](subsubfamily-gap-inventory.md) | 2026-07-06 | `historical` | Le **même défaut, une troisième colonne** (#716) : `Subsubfamily` vide sur « les exactement mêmes 48 rangées » — trouvé en appliquant la méthode #712 à tout le CSV |
| [`i18n-hierarchy-apply-spec.md`](i18n-hierarchy-apply-spec.md) | 2026-07-06 | `historical` | La **spec d'application unifiée** qui consolide les deux inventaires en un seul geste gaté, byte-targeted, sur les 48 rangées × 3 colonnes EN — c'est elle qui porte le mode opératoire, les deux inventaires portant le constat |

### La passe de cohérence terminologique

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`taxonomy-coherence-scan.md`](taxonomy-coherence-scan.md) | 2026-07-04 | `historical` | Passe de cohérence terminologique **cross-langue** (master `d5913862`, dispatch `lofjtd`, secondaire) : **read-only, 0 constat**, sign-off de release — 0 écriture CSV, 0 appel gpt-5.5. Citée par `docs/RELEASE-VERIFICATION-INDEX-v0.9.0.md` et le dossier de release |

### Non établi

- **Quand** les cellules du gap des 48 rangées ont été remplies, et par quelle PR :
  la mesure ci-dessus établit l'état, pas l'historique.
- Le contenu des trois postures de #986 au-delà de leur existence : l'index les
  nomme, il ne les arbitre pas — c'est l'objet de #986, qui attend un humain.
