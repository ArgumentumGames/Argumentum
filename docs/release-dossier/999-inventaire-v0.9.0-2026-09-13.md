# #999 — Inventaire daté des mentions `v0.9.0` (numérotation vivante : **v2.0.0**)

**Status:** INVENTAIRE + bandeaux posés (méthode ③ : bandeau daté + barré, ⛔ jamais supprimer). Aucune mention supprimée.
**Author:** po-2024 — grain **secondaire** du dispatch ai-01 `msg-20260913T105836-kmhyzw` (issue #999, DoD 1).
**Base:** master `35acac04` (2026-09-13) pour l'état de référence ; bandeaux posés le même jour.
**Décision source:** [DECISION-v2.0.0-jsboige.md](DECISION-v2.0.0-jsboige.md) — jsboige 2026-08-06, « on passera en v2 plutôt que v0.9 ».

---

## §0 Repro de la mesure

Commande du DoD 1, reproduite à la racine du dépôt sur master `35acac04` (pré-grain) :

```bash
grep -ril "v0\.9\.0" --include="*.md" --include="*.ps1" .
```

- **76 fichiers** matchent dans l'arbre de travail local.
- Dont **7 non trackés** (`tmp/_pr_body_*.md`, `_415_comment.md` — scratch de sessions passées, jamais commités) : sur un clone frais, le grep voit **69 fichiers**.
- Comptes par fichier mesurés par `grep -oi "v0\.9\.0" <fichier> | wc -l` (recopiés dans les tableaux ci-dessous).
- ⚠️ Aucun des fichiers listés n'a été modifié par le sweep autrement que par **ajout de bandeau/note datée ou mutation datée d'une ligne vivante** (détail §3). Le compte brut de mentions **augmente** donc mécaniquement (chaque bandeau nomme l'ancien numéro pour l'expliquer) — c'est la méthode ③, pas une régression.

## §1 Catégories

| Cat. | Définition | Effectif |
|---|---|---|
| **A** | Document **vivant** qui annonçait encore v0.9.0 comme version courante → traité ce jour (bandeau/note/mutation datée) | 16 fichiers |
| **B** | Document portant **déjà** une note de version ou un bandeau superseded (posés avant ce grain, 2026-06→09) | 7 fichiers |
| **C** | **Trace datée** : rapport/plan/audit nommé ou daté, la release y est citée comme contexte sous son nom d'alors | 51 fichiers (dont 7 non trackés `tmp/`) |
| **D** | **Trace protégée** : le paquet `release-review-v0.9.0/` et le tag `v0.9.0-review` — §2 du corps de #999 les déclare intouchables (soumis à Thomas & Adeline le 14/07 sous ce nom) | 2 fichiers + le tag |

Contrôle : 16 + 7 + 51 + 2 = **76** (arbre local) · 76 − 7 non trackés = **69** trackés.

## §2 Justification, fichier par fichier

### A. Vivants, traités ce jour (16)

| Fichier | Mentions | Traitement |
|---|---:|---|
| `README.md` | 1 | Titre `### Packages (v0.9.0)` → `### Packages (v2.0.0)` + note datée avec ancien nom barré |
| `CHANGELOG.md` | 2 | En-tête de section `## [~~0.9.0~~ → 2.0.0]` (barré, pas supprimé) + note datée ; corps de section conservé tel quel (reprise = DoD 3) |
| `CLAUDE.md` | 2 | L.551 checklist vivante mutée : `#134 — GitHub Release v2.0.0 (ex-v0.9.0, re-scopé 2026-08-06, #999)` ; la 2ᵉ mention (l.157, « GO v0.9.0 bundle ») = trace (le bundle GO porte ce nom) |
| `RELEASE-NOTES-v0.9.0.md` (racine) | 2 | Bandeau EN après le titre |
| `docs/RELEASE-NOTES-v0.9.0.md` | 7 | Bandeau FR après le titre |
| `docs/RELEASE-NOTES-v0.9.0-DRAFT-consolidated.md` | 12 | Bandeau EN après le titre |
| `docs/RELEASE-VALIDATION-v0.9.0.md` | 23 | Bandeau FR après le titre (le bloc 🔻 RÉCONCILIATION 2026-09-13 existant portait sur le couplage DNN, pas le numéro) |
| `docs/RELEASE-VERIFICATION-INDEX-v0.9.0.md` | 16 | Bandeau FR après le titre |
| `docs/RELEASE-ENTRY-POINT-v0.9.0.md` | 12 | Bandeau FR après le titre (distinct du bandeau « MISE À JOUR CHIFFRES » 2026-09-13 déjà présent) |
| `docs/RELEASE-VISUAL-GO-SESSION-v0.9.0.md` | 16 | Bandeau FR : si la session GO est rejouée post-régén (12/09), c'est v2.0.0 qui s'applique |
| `docs/publication/news-article-v0.9.0.fr.md` | 16 | Bandeau FR « ⛔ ne pas publier tel quel » — le corps annonce encore v0.9.0 |
| `docs/publication/news-article-v0.9.0.en.md` | 18 | Bandeau EN miroir |
| `docs/publication/README.fr.md` | 5 | Note datée sous « §3 Article d'annonce » : noms de fichiers conservés (traces), contenu à repasser en v2.0.0 |
| `docs/publication/README.en.md` | 5 | Note EN miroir |
| `docs/dnn-localization/release-validation/README.md` | 4 | Note FR (même motif que les runbooks go-live notés le 2026-09-11) |
| `docs/ontology/README.md` | 4 | L.115 (limitation `versionInfo`) mutée : « bump to the release tag (**v2.0.0**, ex-v0.9.0) » ; les 3 autres mentions décrivent le claim/l'artefact sous son nom = traces |

### B. Déjà notés avant ce grain (7)

| Fichier | Mentions | Note existante |
|---|---:|---|
| `docs/dnn/go-live-turnkey-checklist.md` | 4 | « Version note (2026-09-11) : re-scoped v0.9.0 → v2.0.0 » |
| `docs/dnn/go-live-gate-runbook.md` | 2 | idem 2026-09-11 |
| `docs/dnn/go-live-smoke-test.md` | 2 | idem 2026-09-11 |
| `docs/release-dossier/README.md` | 14 | « ⚠️ Superseded (2026-06-19 snapshot) » |
| `docs/release-v0.9.0-validation-brief.md` | 10 | « Document superseded (snapshot 2026-06-13) » |
| `docs/v0.9.0-readiness-and-disk-recovery.md` | 7 | « Part 1 superseded (snapshot 2026-06-13) » |
| `docs/release-dossier/DECISION-v2.0.0-jsboige.md` | 9 | C'est l'**enregistrement** de la décision v2.0.0 — cite l'ancien nom par nature |

### C. Traces datées, intouchables (45)

**docs/investigations/ (16)** — rapports de session/archéologie, nom de fichier `AAAA-MM-JJ-*` :
`2026-06-14-140-qa-verification-matrix` (1) · `2026-06-14-188-rules-cover-extension-scoping` (6) · `2026-06-14-189-print-play-complete-scoping` (6) · `2026-06-14-415-git-repo-slimming-scoping` (4) · `2026-06-25-virtues-mindmap-fr-frozen-mechanism` (6) · `2026-07-02-scanner-fp-arbitration` (1) · `2026-07-03-629-cardpen-pages-cards-404-diagnostic` (1) · `2026-07-03-dnn-i18n-porting-mechanism` (1) · `2026-07-03-dnn-prod-rules-coverage` (2) · `2026-07-03-polish-trad-sweep-v090-confirm` (1) · `2026-07-04-regen-readiness-checklist-8lang` (2) · `2026-07-05-654-remark-dryrun` (1) · `2026-07-11-regen-readiness-refresh-c1ed77d2` (7) · `2026-07-16-mindmap-html-wrapper-encoding` (1) · `2026-07-19-aif-attack-graph-export` (1) · `2026-07-28-release-validation-v0.9.0-audit` (5).

**tmp/ (7, non trackés)** — corps de PR de sessions passées (`_pr_body_134.md` (3), `_pr_body_relnotes.md` (2), `_pr_body_457verify.md` (2), `_415_comment.md` (2), `_pr_body_owl_v2.md` (1), `_pr_body_layerc.md` (1), `_pr_body_202.md` (1)). Scratch local, invisible d'un clone frais.

**docs/dnn/ (7)** :
- `UPGRADE-ASSESSMENT.md` (6) — mentions dans la section datée « §10 … (2026-06-24) », recommandation passée.
- `skin-tabid138-diagnostic-runbook.md` (2) — « non-bloquant v0.9.0 / fix reporté v0.9.1 », décision datée d'alors (voir résiduels : le fast-follow « v0.9.x » porte aussi l'ancienne numérotation).
- `machinekey-rotation-scrub-runbook.md` (1) — contexte « go-live pending v0.9.0 » d'alors.
- `dnn10-migration-readiness.md` (1) — état daté du chantier.
- Les 3 runbooks go-live sont en catégorie B ci-dessus.

**docs/dnn-localization/ (7)** : `136-2sxc-collaborative-scoping` (5, scoping daté 2026-06-27) · `457-document-tier-translation-workflow` (3) · `131-upgrade-9.13-sandbox-plan` (2, plan daté) · `457-site-content-type-inventory` (2) · `132-deployment-runbook` (2) · `PHASE1-content-audit` (2, audit daté) · `README` (1). La 8ᵉ entrée du répertoire (`release-validation/README.md`) est traitée en §2.A.

**docs/taxonomy/ (6)** : `498-layerC-pilot-design` (10) · `i18n-hierarchy-apply-spec` (2) · `498-reconciliation-p1-closure` (2) · `192-native-ratification-checklist` (2) · `taxonomy-coherence-scan` (1) · `192-terminology-glossary-register` (1) — chantiers taxonomy, la release y est un jalon de contexte sous son nom d'alors.

**docs/repo/ (3)** : `415-phase2-proposal` (4) · `415-git-reduction-execution-plan` (4) · `tests-nullable-cleanup-plan` (3) — plans datés.

**docs/translation/ (2)** : `202-qa-gate-protocol` (2) · `202-en-campaign-plan` (2) — campagne traduction d'alors.

**docs/quality/ (1)** : `192-link-i18n-scoping` (3) — scoping daté.

**docs/licensing/ (1)** : `dependency-license-inventory` (4) — audit « prepared ahead of public release v0.9.0 », snapshot.

**docs/publication/ (3)** : `release-v090-bundle-v3-8lang-verification-guide` (3) — vérifie le bundle GDrive **nommé** `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/` (nom d'artefact, trace) · `validation-guide.fr` (1) et `qa-scenario-8langues-release` (1) — références au gate « #134 — GitHub Release v0.9.0 » sous son intitulé d'alors.

**.claude/skills/coordinate/SKILL.md (1)** — citation d'un dispatch passé (« roadmap release v0.9.0 ») dans un exemple ; fichier du domaine ai-01, laissé tel quel.

### D. Traces protégées par le corps de #999 (2 fichiers + le tag)

`docs/release-review-v0.9.0/README.md` (5) et `CHECKLIST.md` (1) — le paquet effectivement soumis à Thomas & Adeline le 14/07 sous ce nom ; le renommer casserait les références de #134/#458/#802. Le tag **`v0.9.0-review`** (commit `6fe0a84b`, 2026-07-14) reste tel quel. Un second tag `v0.9.0-review-refresh` existe (13/08) — même statut.

## §3 Ce que ce grain a fait (résumé)

16 fichiers vivants : 11 bandeaux/notes datées, 1 en-tête CHANGELOG barré, 1 titre README corrigé, 2 mutations de lignes vivantes (CLAUDE.md checklist, ontology README). **0 suppression, 0 renommage de fichier** — les noms `*-v0.9.0.md` restent (traces ; tout renommage casserait des liens croisés et relèverait de la reprise DoD 3, pas d'un sweep).

## §4 Résiduels #999 (hors périmètre de ce dispatch — décision ai-01/jsboige)

1. **DoD 2 — milestones** : `v0.9.0 — Release tag` (3 issues ouvertes) et `v0.9.x — Fast-follow post-tag` (4 issues) existent toujours (vérifié 2026-09-13). Renommage = opération GitHub partagée, non incluse dans ce dispatch worker.
2. **DoD 3 — notes v2.0.0** : rédiger les notes côté « ce qui change pour quelqu'un qui possède l'édition précédente » et reprendre le corps des news articles FR/EN (bandeaux « ⛔ ne pas publier tel quel » posés ce jour). Relecture jsboige requise.
3. **DoD 4 — packaging** : [`package-v2.0.0.ps1`](package-v2.0.0.ps1) existe (manifest 80 PDFs, paramétré `-Freshness|-BundleDir|-ManifestPath`, 0 `v0.9.0` codé en dur) ; il couvre le manifest, pas encore un éventuel `scripts/release/package-release.ps1` complet.
4. **Le tag `v2.0.0` n'est pas posé** (séquencement #999 : après régén — faite 12/09 — avant tag).
5. Les **fichiers** nommés `*-v0.9.0.md` garderont leur nom tant que la reprise DoD 3 n'a pas décidé d'éventuels renommages avec mise à jour des liens croisés.

## §5 Verdict

Le DoD 1 (« ne subsiste que des traces datées, justifiées une par une ») est satisfait **au sens de la méthode ③** : chaque occurrence restante vit soit dans un document portant une note de version datée (cat. A/B), soit dans une trace datée inventoriée ci-dessus (cat. C/D). L'inventaire est rejouable par la commande du §0 — sur un clone frais, 69 fichiers, tous couverts par les catégories A–D (les 7 `tmp/` ne sont pas trackés).
