# docs/quality — quality and release evidence: dated findings, and what governs today

<!-- index-of-record -->

This folder is an **index of record**, as defined in [`docs/agents/consolidation.md`](../agents/consolidation.md): every tracked file in it is listed below with what it is worth today. Add a file here and you add its line — `python tools/test_docs-index-contract.py` goes red otherwise. A file marked `current` was re-read against `master` on the date below; a `superseded` one names its successor on its line; a `historical` one was true at its date and is neither executed nor "corrected" now.

Measured 2026-11-25 on `6b101880`, before this README: **61 tracked files** (`git ls-files docs/quality | wc -l`) — 57 Markdown, 1 Python, 2 text, 1 JSON. No README existed; this one indexes all 61 in a single tranche (the guide's bound is "about sixty", and no pull request is stacked on this file). Issue states were read the same day in one bulk call. Re-measure before quoting a count; the issue says what is open. Statuses: **6 `current`, 1 `superseded`, 54 `historical`.**

Several files are cited **by path** from guard tests, workflows and other indexes — moving one breaks those references: `1049-webroot-guard.md` and `1244a-dnn-bin-untrack-provenance.md` from `DnnWebrootGuardTests.cs` / `DnnBinUntrackGuardTests.cs` (and `.gitignore`); `visual-tests-release-gate.md` from `build.yml`, `mindmap-wrapper-behaviour.yml`, `AGENTS.md` §8 and the licence inventory; `1460-charpente-suite-regression.md` from `python-tools-tests.yml` and `AGENTS.md` §8; `415-filter-repo-runbook-post-tag.md` from `docs/repo/README.md`; `1066-cutover-runbook-2026-11-04.md` from the `docs/dnn`, `docs/publication` and `docs/site` indexes; `1502-g7-regles-site-vs-cartes-2026-09-23.md` from `docs/corpus/rules-site-vs-cards.py`.

## Where the subjects stand

- **Webroot, tracked DNN binaries, the DNN upgrade (#1049 open, #1244 closed, #131 open).** Re-measured today: `git ls-files DNNPlatform/bin` returns 0 files (the de-track of `1244a` still holds) and both guards are present at `master`: `DnnWebrootGuardTests.cs`, which cites `1049`, and the `.githooks/pre-commit` hook. The upgrade itself was executed — preprod runs 10.4.1 + 2sxc 22.01 since 02/11 (runbook §0) — so the preparation dossier and the Stripe audit are dated records; the shop decision (D1) is still open, with its decision sheet in `docs/release-dossier/`.
- **The 2026-11-24 cutover (#1066 closed, #1180 closed).** The swap happened on 2026-11-24 at 04:56Z (`docs/dnn-localization/README.md` §0, three independent sources). The consolidated runbook stays `current`: what it still governs is what its §3 lists as open — the #2132 human gate, the #134 final release, the module-level packs, the #135 grains (all open at the date of this index) — and its revision of 24/11 is #2375, open. Its predecessor of 22/09 is superseded by it. The four `1180-*` files and the preprod baseline are the dated evidence under it.
- **Site content (#135 open, #1502 open, #133 open, #1091 open, #2317 open).** All dated measurements, each pointing at its open issue for the live state. The rules comparison dossier is still the writing reference cited by `docs/corpus/rules-site-vs-cards.py`; it is `historical` because the divergence it measured was a fact of 2026-09-23, not an instruction.
- **Repository weight (#415 open).** Phase 1 is done at `HEAD`; phase 2 is prepared and **has never been run**. Re-measured today: the tag `v2.0.0` is still absent from GitHub (`gh api …/git/ref/tags/v2.0.0` → 404), so the execution gate (published tag + owner GO) is closed. The runbook is the one live file; the mirror dry-run of 19/11 is its rehearsal record, the two `.txt` are its data extracts.
- **Mind maps, visual validation, regeneration (#830 closed, #802 closed as duplicate, #1048 closed).** `visual-tests-release-gate.md` is the live register: the build workflow compiles VisualTests but never runs them, the documented replacement is a local release-validation run, and the case count is recorded there one dated line per change — 38 at the 11/09 baseline, 68 at the last recorded change (16/11). The five `830-*`/`802-*` files are the dated measurements of the 9 capabilities, committed and served layer; the `regen-baseline-v2.0.0` pair froze page signatures of the 22/09 recette bundle (nothing outside this folder cites it today: `git grep regen-baseline -- ':!docs/quality'` returns 0 hit — a search can miss).
- **Structural regression suite #1460 (open; phase D gated on #2132).** The charpente is `current` — it governs the suite's contracts and is cited by `AGENTS.md` §8 and the workflow header; the eight phase reports are dated records of the organs delivered between 05/11 and 08/11.
- **Corpus arbitration #1499 (closed epic), #994 (closed), #192 (closed), #1548 (closed), and the July drift audit.** Read-only dossiers under rule C (default: keep `HEAD`); the writes that followed are counted in their own dossiers (8 cells for PK 636, 3 cells for PK 492, 7 template sites). The `1548` cascade instrument is kept as the evidence script — nothing outside this index references it (searched).

## Known contradictions (dated statements, not corrected here — the index changes no finding)

- `1066-cutover-runbook-2026-11-04.md`, state block (l. 3–20): "la production est **gelée** — DNN 9.11.1.19 … `/en/` = 404". That is the state of 2026-11-04; production was swapped on 2026-11-24 at 04:56Z (`docs/dnn-localization/README.md` §0). The runbook stays `current` because its §3 still governs the remaining day-J steps; the block needs a pull request of its own.
- `415-p24-14-mirror-dry-run-2026-11-19.md` states that the `v2.0.0` tag is not published (19/11). Re-measured 2026-11-25: still 404 — consistent, no drift.
- The `docs/dnn`, `docs/publication` and `docs/site` indexes introduce the consolidated runbook as "what governs go-live day". That day has happened (24/11 04:56Z); the sentence remains true of the procedure that still governs the gestures of the runbook's §3, not of a day to come.

## The files

### Webroot, tracked DNN binaries, the DNN upgrade — #1049, #1244, #131

| File | Dated | Status | What it is |
|---|---|---|---|
| [`1049-webroot-guard.md`](1049-webroot-guard.md) | 2026-09-16 | `current` | The guard organ for the root `web.config`: why the tracked copy was a total outage waiting for a webroot, and the test organ that holds it out — `DnnWebrootGuardTests.cs`, which cites this document; the `.githooks/pre-commit` hook guards the same file a second time (both present at master). #1049 still open |
| [`1244-webroot-equals-checkout-chronology.md`](1244-webroot-equals-checkout-chronology.md) | 2026-09-14 | `historical` | The chronology proving the served webroot was a live checkout with tracked binaries older than the installed DNN — the measurement that made the de-track decidable. #1244 closed |
| [`1244a-dnn-bin-untrack-provenance.md`](1244a-dnn-bin-untrack-provenance.md) | 2026-09-14 | `current` | What the tracked `DNNPlatform/bin/` layer was and was not, and the de-track of its 195 files (45.7 MB). Re-measured 2026-11-25: 0 files tracked there; `DnnBinUntrackGuardTests.cs` cites its §2 and §7 |
| [`131-montee-dnn-2026-09-16.md`](131-montee-dnn-2026-09-16.md) | 2026-09-16 | `historical` | Preparation dossier for the DNN upgrade: measured state, extensions, order of operations, rollback. The upgrade was executed (10.4.1 on preprod, 02/11) |
| [`131-d1-stripe-poc-2026-09-17.md`](131-d1-stripe-poc-2026-09-17.md) | 2026-09-17 | `historical` | The Stripe audit that preceded the D1 shop decision; it decides nothing. Live state: #131 / #445, decision sheet in `docs/release-dossier/` |

### The 2026-11-24 cutover — runbooks, route parity, day-J baselines — #1066, #1180, #131

| File | Dated | Status | What it is |
|---|---|---|---|
| [`1066-balayage-servi-depot-2026-09-15.md`](1066-balayage-servi-depot-2026-09-15.md) | 2026-09-15 | `historical` | Served↔repository sweep (gate 2c) and the location of the GTM/GA carrier: read-only HTTP, disk greps, SQL. #1066 closed |
| [`1066-cutover-runbook-2026-09-22.md`](1066-cutover-runbook-2026-09-22.md) | 2026-09-22 | `superseded` | The first cutover runbook (R1–R8), replaced by [`1066-cutover-runbook-2026-11-04.md`](1066-cutover-runbook-2026-11-04.md) — its own banner says so; kept for the measured detail it alone carries |
| [`1066-cutover-runbook-2026-11-04.md`](1066-cutover-runbook-2026-11-04.md) | 2026-11-04 | `current` | The consolidated cutover runbook: master table (gesture, lane, control, rollback), day-J sequence, post-swap probes, and what remains open in §3. Cited as the governing procedure by three folder indexes. Its state block is stale (see contradictions) |
| [`1180-1066-route-parity-prod-preprod-blob.md`](1180-1066-route-parity-prod-preprod-blob.md) | 2026-09-14 | `historical` | Route parity prod × preprod × blob: the 16 `_ext` wrappers were 404 on preprod then. Dated measurement. #1180 closed |
| [`1180-dossier-cutover-2026-09-16.md`](1180-dossier-cutover-2026-09-16.md) | 2026-09-16 | `historical` | Cutover dossier of gaps E1–E11 with a proposal per gap — none decided here; the consolidated runbook is what came of it |
| [`1180-parite-remesure-2026-11-02.md`](1180-parite-remesure-2026-11-02.md) | 2026-11-02 | `historical` | The same parity measure replayed after the 10.4.1 upgrade: the wrappers are no longer absent; it gives the delta, not a replacement |
| [`1180-recette-preprod-2026-09-17.md`](1180-recette-preprod-2026-09-17.md) | 2026-09-17 | `historical` | Preprod acceptance of 17/09: gaps E4/E6/E7 closed, "preprod nickel" state of that date |
| [`controles-jour-j-baseline-preprod-2026-11-05.md`](controles-jour-j-baseline-preprod-2026-11-05.md) | 2026-11-05 | `historical` | Day-J control baseline on preprod: 8 cultures × 4 surfaces, statuses, `<html lang>`, bytes. The reference the post-swap probes are compared against |
| [`131-p2-sonde-accueils-2026-11-24.md`](131-p2-sonde-accueils-2026-11-24.md) | 2026-11-24 | `historical` | P24-45: first reading of probe P2 (analytics marker on the 8 homepages) now rendered by `tools/1105-cutover-day-controls.py`, and its bounds |

### Site content — rules, drafts, downloads, ontology, database, accordion — #135, #1502, #133, #1091, #2317

| File | Dated | Status | What it is |
|---|---|---|---|
| [`1502-g7-regles-site-vs-cartes-2026-09-23.md`](1502-g7-regles-site-vs-cartes-2026-09-23.md) | 2026-09-23 | `historical` | The published rules page against the repository cards: per-section comparison, every verdict computed by `docs/corpus/rules-site-vs-cards.py` (which still cites this dossier). #1502 open |
| [`135-draft-links-vs-published-assets-2026-09-22.md`](135-draft-links-vs-published-assets-2026-09-22.md) | 2026-09-22 | `historical` | Every link of the 4 news/download drafts resolved against the v2.0.0-review assets (HTTP code and size). #135 open |
| [`135-remesure-brouillons-et-liens-servis-2026-11-04.md`](135-remesure-brouillons-et-liens-servis-2026-11-04.md) | 2026-11-04 | `historical` | The drafts re-measured unchanged and the served download links relevés — the target moved under fixed drafts |
| [`135-zips-contenu-et-correspondance-2026-11-05.md`](135-zips-contenu-et-correspondance-2026-11-05.md) | 2026-11-05 | `historical` | What is behind the 4 served zips (no extraction): all FR, 18/08 generation, plus the proposed correspondence to build at tag time |
| [`135-zips-essai-a-blanc-2026-11-05.md`](135-zips-essai-a-blanc-2026-11-05.md) | 2026-11-05 | `historical` | `tools/135-build-download-zips.py` and its dry run: faces derived from the CSV (175), `--build` refuses on any count gap. The build itself happens at the tag |
| [`133-endpoint-vs-ticket-gap-2026-09-22.md`](133-endpoint-vs-ticket-gap-2026-09-22.md) | 2026-09-22 | `historical` | Served endpoint against the ticket's scope, item by item, with HTTP codes and content types — "measuring the gap is not filling it". #133 open |
| [`133-iri-impact-before-owl-publication.md`](133-iri-impact-before-owl-publication.md) | 2026-09-14 | `historical` | The IRI surface was already unstable before publication; commits the v1's oracle mistake (a checkout 62 commits behind). #133 open |
| [`1091-preprod-db-provenance-2026-09-22.md`](1091-preprod-db-provenance-2026-09-22.md) | 2026-09-22 | `historical` | Is the preprod database a production backup? Read-only SQL; no credential, hash or `XMLData` reproduced. #1091 open |
| [`1091-shop-go-live-dating-2026-09-22.md`](1091-shop-go-live-dating-2026-09-22.md) | 2026-09-22 | `historical` | Dating the shop's go-live to size the risk of the 2023 archive — a date suffices; the archive was never opened. #1091 open |
| [`2317-accordion-idempotent-init-2026-11-19.md`](2317-accordion-idempotent-init-2026-11-19.md) | 2026-11-19 | `historical` | The BS5 accordion's double `init()`: measured cause and the idempotent fix on the served variant (BS3/BS4 untouched). Live state: #2317 |

### Repository weight — `git filter-repo` phase 2 — #415

| File | Dated | Status | What it is |
|---|---|---|---|
| [`415-filter-repo-runbook-post-tag.md`](415-filter-repo-runbook-post-tag.md) | 2026-09-21 | `current` | The phase-2 history rewrite, post-tag and owner-gated: it governs the gesture and has never been run. Re-measured 2026-11-25: the `v2.0.0` tag is still absent (404), preflight tool present. `docs/repo/README.md` delegates to it |
| [`415-objets-vises-phase2-2026-09-21.txt`](415-objets-vises-phase2-2026-09-21.txt) | 2026-09-21 | `historical` | Data extract: the 416 objects phase 2 targets, one line each (sha, type, packed size, family, path), perimeter = the real globs |
| [`415-refs-pre-phase2-2026-09-21.txt`](415-refs-pre-phase2-2026-09-21.txt) | 2026-09-21 | `historical` | Data extract: every ref (heads and tags) before phase 2, with its sha — the "before" side of the runbook's post-gesture control |
| [`415-p24-14-mirror-dry-run-2026-11-19.md`](415-p24-14-mirror-dry-run-2026-11-19.md) | 2026-11-19 | `historical` | The rehearsal of phase 2 on a throwaway mirror: four runbook corrections, nothing pushed, nothing mutated. Its "tag not published" was re-measured today — still true |

### Mind maps, visual validation, regeneration — #830, #802, #1048

| File | Dated | Status | What it is |
|---|---|---|---|
| [`visual-tests-release-gate.md`](visual-tests-release-gate.md) | 2026-08-10 | `current` | The live register of release validation outside CI: the build never runs VisualTests, the replacement is a local run before the tag, the fixture split, and the case count recorded one dated line per change (38 at the 11/09 baseline, 68 at the last recorded change of 16/11). Cited by two workflows, `AGENTS.md` §8 and the licence inventory |
| [`802-830-cards-fr-wrapper-provenance.md`](802-830-cards-fr-wrapper-provenance.md) | 2026-09-14 | `historical` | Provenance of the `fallacies_cards_fr.html` route: it belongs to the #830/#802 deliverable, not an orphan file. #802 closed as duplicate |
| [`830-etat-9-capacites-2026-09-15.md`](830-etat-9-capacites-2026-09-15.md) | 2026-09-15 | `historical` | Which of the 9 behavioural capabilities is asserted by what, and the coverage — the base the regeneration bar was built on. #830 closed |
| [`830-served-wrapper-revalidation.md`](830-served-wrapper-revalidation.md) | 2026-09-13 | `historical` | The served layer re-validated where CI is structurally blind: what the user receives, not what `HEAD` renders |
| [`830-reference-numerique-9-capacites-2026-09-21.md`](830-reference-numerique-9-capacites-2026-09-21.md) | 2026-09-21 | `historical` | The numeric reference values of the 9 capabilities, frozen on the re-derived tree via the existing VisualTests instrument |
| [`830-mindmaps-servi-etat-2026-10-09.md`](830-mindmaps-servi-etat-2026-10-09.md) | 2026-10-09 | `historical` | Served state of the mind maps measured on production bytes: the redeployment had been applied while three documents said otherwise. The redeploy runbook cites it twice |
| [`1123-race-assemblage-2026-09-16.md`](1123-race-assemblage-2026-09-16.md) | 2026-09-16 | `historical` | The image-assembly race (root cause #1121): verified code path, reproducible sequence, irreproducibility bound, proposed guard. The durable fix was a post-tag decision; #1123 closed |
| [`regen-baseline-v2.0.0-2026-09-22.md`](regen-baseline-v2.0.0-2026-09-22.md) | 2026-09-22 | `historical` | Regeneration baseline v2.0.0: page-by-page signatures of the recette bundle, one sha256 per source PDF, with the replay control |
| [`regen-baseline-v2.0.0.json`](regen-baseline-v2.0.0.json) | 2026-09-22 | `historical` | The data companion: signatures above, produced by `tools/pdf-page-signature.py` (instrument sha pinned in the file) |

### Structural regression suite — #1460

| File | Dated | Status | What it is |
|---|---|---|---|
| [`1460-charpente-suite-regression.md`](1460-charpente-suite-regression.md) | 2026-09-21 | `current` | The suite's contracts and measurement points for stages 1/3/4, and what is decidable without a human verdict; status updated 11/11 (A delivered, B and C organs delivered, D gated on #2132). Cited by `AGENTS.md` §8 and the workflow header |
| [`1460-phase-a-geometrie-2026-11-05.md`](1460-phase-a-geometrie-2026-11-05.md) | 2026-11-05 | `historical` | Phase A: page geometry and page counts, the pure verifier plus the 8-witness inverse control, with an erratum of the same evening |
| [`1460-phase-b-placeholders-2026-11-06.md`](1460-phase-b-placeholders-2026-11-06.md) | 2026-11-06 | `historical` | Phase B organ 1: no literal placeholder survives — 6 cases including a mutation control on a real template |
| [`1460-phase-b-fr-contamination-2026-11-08.md`](1460-phase-b-fr-contamination-2026-11-08.md) | 2026-11-08 | `historical` | Phase B organ 2: no French tool-word survives in a non-French column — zero tolerated, one named test per language plus anti-vacuity |
| [`1460-phase-b-script-proportions-2026-11-08.md`](1460-phase-b-script-proportions-2026-11-08.md) | 2026-11-08 | `historical` | Phase B organ 3: Cyrillic, CJK and Arabic script proportions with two named, justified thresholds — the organ that catches empty cards |
| [`1460-phase-c-rules-first-2026-11-06.md`](1460-phase-c-rules-first-2026-11-06.md) | 2026-11-06 | `historical` | Phase C organ 1: the Tarot deck starts with the 15 Rules pages, in order, in all 8 languages |
| [`1460-phase-c-famille-2026-11-07.md`](1460-phase-c-famille-2026-11-07.md) | 2026-11-07 | `historical` | Phase C organ 2: every Fallacies card carries its family colour — the three anchors CSV × template × pixel |
| [`1460-phase-c-virtues-2026-11-07.md`](1460-phase-c-virtues-2026-11-07.md) | 2026-11-07 | `historical` | Phase C organ 3: the same family-colour organ transposed to Virtues, and the two template facts that changed the shared engine |
| [`1460-phase-c-garde-bloc-2026-11-08.md`](1460-phase-c-garde-bloc-2026-11-08.md) | 2026-11-08 | `historical` | Phase C: the block guard that closed the "red at 0.1, green at 0.0" hole of the colour-distinct grouping |

### Corpus arbitration and dated audits — #1499, #994, #192, #795, #1548

| File | Dated | Status | What it is |
|---|---|---|---|
| [`1499-636-desc-2022-2026-09-25.md`](1499-636-desc-2022-2026-09-25.md) | 2026-09-25 | `historical` | Grain ② of #1499: PK 636 restored to its 2022 definition in the 8 `desc_*` cells — 1 CSV row, −106 bytes, measured before/after. #1499 closed |
| [`1499-g1-fallacies-dossier-2026-09-23.md`](1499-g1-fallacies-dossier-2026-09-23.md) | 2026-09-23 | `historical` | G1 v3: Fallacies renaming arbitration dossier, instrument run as delivered, no CSV write — the owner decides card by card |
| [`1499-g2-fallacies-exemples-2026-09-23.md`](1499-g2-fallacies-exemples-2026-09-23.md) | 2026-09-23 | `historical` | G2: the `example_fr` field, 132 non-IDENT cells measured, 111 forming the arbitration surface — no write |
| [`1499-g2c-fallacies-exemples-liste-courte-2026-09-24.md`](1499-g2c-fallacies-exemples-liste-courte-2026-09-24.md) | 2026-09-24 | `historical` | G2-C: the short list of `example_fr` losses, baseline 2024 → HEAD: 93 on-the-spot reformulations, 18 full replacements |
| [`1499-g3-fallacies-desc-2026-09-23.md`](1499-g3-fallacies-desc-2026-09-23.md) | 2026-09-23 | `historical` | G3 v2: the `desc_fr` field and its short loss list, comparison against the 2024 version as the ai-01 review required |
| [`1499-g4-virtues-dossier-2026-09-22.md`](1499-g4-virtues-dossier-2026-09-22.md) | 2026-09-22 | `historical` | G4: the 312 French cells of the 131 printable Virtues cards — arbitration dossier, no CSV or template write |
| [`1499-g4bis-planche-arbitrage-virtues-2026-09-24.md`](1499-g4bis-planche-arbitrage-virtues-2026-09-24.md) | 2026-09-24 | `historical` | G4bis: stratified sample of 10 `remark_fr` pairs out of 45 below 0.3 similarity, plus 3 high-similarity witnesses |
| [`1499-g4c-virtues-liste-courte-2026-09-24.md`](1499-g4c-virtues-liste-courte-2026-09-24.md) | 2026-09-24 | `historical` | G4-C: the 45 most-rewritten `remark_fr` pairs read one by one — 42 condensations without loss under rule C |
| [`1499-g5-typo-gabarits-2026-09-26.md`](1499-g5-typo-gabarits-2026-09-26.md) | 2026-09-26 | `historical` | Grain 5: hard-coded template typography — 2 defects found (intra-word straight apostrophe, fr), 7 sites written in 4 files (#1571) |
| [`1499-g8-memo-inventaire-2026-09-23.md`](1499-g8-memo-inventaire-2026-09-23.md) | 2026-09-23 | `historical` | G8: the Memo content embedded in the template's internal `csv` key, parsed with `json.loads`, never with a raw grep |
| [`1499-g9aw-ecritures-2026-09-26.md`](1499-g9aw-ecritures-2026-09-26.md) | 2026-09-26 | `historical` | G9-A-W: the 3 actionable misaligned cells written (PK 492, `text_{es,fa,zh}`) — 1 CSV row, +5 bytes |
| [`994-residuel-apostrophes-fr-2026-09-21.md`](994-residuel-apostrophes-fr-2026-09-21.md) | 2026-09-21 | `historical` | The full nominative worklist of straight apostrophes in FR columns, measure only. #994 closed |
| [`192-link-i18n-scoping.md`](192-link-i18n-scoping.md) | 2026-07-13 | `historical` | July 2026: the `link_*` URL coverage gap isolated as the only material residue of the drift audit. Read-only. #192 closed |
| [`multilingual-drift-audit-2026-07.md`](multilingual-drift-audit-2026-07.md) | 2026-07-12 | `historical` | The July drift audit across the 4 CSV × 7 languages ("real translatable content is clean"), with a 15/09 erratum voiding its IP-name examples |
| [`1548-memo-title-div-cascade.py`](1548-memo-title-div-cascade.py) | 2026-09-25 | `historical` | Evidence script: measures without the pipeline which `font-family` rule wins the cascade on the four MEMO letters under ar/fa/zh. #1548 closed |