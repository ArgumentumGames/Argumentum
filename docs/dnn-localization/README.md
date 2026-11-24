# DNN Localization & Upgrade Arc — Index of record (tranche 1/4, measured 2026-11-24)

> **Entry point for the `#131` / `#132` / `#457` DNN documentation arc.**
> The folder holds **217 tracked files** (`git ls-files docs/dnn-localization`, measured 2026-11-24). This
> README is being rebuilt as an **index of record, by tranches**: tranche 1 (this file) indexes the **49
> root files whose name is not `458-*`** — the 48 documents below plus this README — and the two
> subfolders; tranches 2–4 index the **134 `458-*` files** (the injection records). ⚠️ The
> `<!-- index-of-record -->` mark is posed at the **last** tranche: this file does not carry it yet, and
> the contract `tools/test_docs-index-contract.py` holds only a marked README to completeness.

**Status vocabulary** (`docs/agents/consolidation.md`): `current` = says today's state, or governs a
gesture still to come — re-read against master on 2026-11-24; `superseded` = a later document replaces it
(the successor is named on the same line); `historical` = dated record, not governing. Doubt between
current and historical resolves to **historical**. For data files (CSV/JSON) the date column is the last
commit that touched them; the status says whether they are still consumed.

---

## §0 — Site state, 2026-11-24 (supersedes the former index kept at the end of this file)

**The production swap happened on 2026-11-24 at 04:56Z.** Three independent sources: web2 posted
"jour J terminé" at 05:04Z; the external recette P23-32 ran read-only against the served site (32/32
routes, legal EN 2/2, 301s, 7 language links per homepage, `hreflang` 0/8); ai-01 measured on its own
instrument. Nothing in this folder was written by that pass — this index records it, it does not
certify it.

> ⚠️ The **former navigation index** is kept **byte for byte at the end of this file**, under *Archive*
> (last changed on master 2026-07-13; its status block is dated 2026-07-12). Three of its lines are
> statements of that date, not of today. Its "Prod go-live — PENDING" line is **false since 24/11**. Its
> "Current target: DNN 10.3.2 + 2sxc 21.07 LTS" predates the cutover runbook, whose validated generation
> is **DNN 10.4.1 + 2sxc 22.01** ([`../quality/1066-cutover-runbook-2026-11-04.md`](../quality/1066-cutover-runbook-2026-11-04.md)).
> Its §4 says the gates it lists "block prod go-live": the go-live happened (above), and the state of each
> gate lives on its issue, not in this file. The B1-inversion sweep note and the version matrix it points
> to remain the record of *how* the runtime was made to work.

---

## §1 — The go-live spine (executed; read for history, not as a TODO)

In the order it was executed. Every row below is `historical` or `superseded` today: the upgrade the
spine planned is the one that went live on 24/11 (§0).

| File | Date | Status | What it is |
|---|---|---|---|
| [131-step0-runtime-verification.md](131-step0-runtime-verification.md) | 2026-06-17 | `historical` | Verified fact that founded the plan: DNN 10.x runs on **.NET Framework 4.8**, not .NET 8. |
| [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) | 2026-06-17 | `superseded` → [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | CORRIGENDUM of a CVE-patched-version error in the merged v2 plan and Step 0; refined the target. |
| [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | 2026-06-18 | `historical` | DECISION-SPEC: 10.1.2 → **10.3.2 full**, 2sxc-cliff ordering. The target that was executed. |
| [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) | 2026-06-19 | `historical` | Step-by-step **2sxc 15.02 → 21.07 LTS** before crossing 10.2.0. Executed as planned. |
| [131-dnn-phase2-exec-rollback.md](131-dnn-phase2-exec-rollback.md) | 2026-06-19 | `historical` | DNN **9.11.1 → 10.3.2** execution sequence + two-anchor rollback runbook (§4.2 restore order fixed in #552). |
| [131-step2-smoke-test-checklist.md](131-step2-smoke-test-checklist.md) | 2026-06-18 | `historical` | Consolidated smoke-test gate to run after the sandbox upgrade, before go-live. |
| [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) | 2026-06-17 | `historical` | Merged "v2" sandbox plan + go/no-go matrix; the palier logic that was followed. |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) | 2026-06-17 | `superseded` → [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | Sandbox runbook written for the **10.1.2** target; mechanics still readable, target abandoned. |
| [131-upgrade-9.13-sandbox-plan.md](131-upgrade-9.13-sandbox-plan.md) | 2026-06-15 | `superseded` → [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) | First 9.13.x palier plan. 9.13.x closes **0** CVEs (NVD/GHSA correction) — replaced by v2. |
| [131-regen-staging-runbook.md](131-regen-staging-runbook.md) | 2026-06-19 | `historical` | Stages every prerequisite for an attended mindmap/PDF regen, **no launch**. No regen was launched from it. |
| [132-deployment-runbook.md](132-deployment-runbook.md) | 2026-11-04 | `superseded` → [../quality/1066-cutover-runbook-2026-11-04.md](../quality/1066-cutover-runbook-2026-11-04.md) | Former prod deployment runbook; its own header names the #1066 cutover runbook as the replacement. |

---

## §2 — Site content, localization pipeline (#457)

| File | Date | Status | What it is |
|---|---|---|---|
| [PHASE1-content-audit.md](PHASE1-content-audit.md) | 2026-06-13 | `historical` | Phase 1 content-extraction audit (#457): what the site content is made of, repo-side. Complete. |
| [457-site-content-type-inventory.md](457-site-content-type-inventory.md) | 2026-06-17 | `historical` | Localization scope map of the site's content types, extending Phase 1. Pre-upgrade pictures; the zone-3 measurement of 09-10/10 is the later picture. |
| [457-document-tier-translation-workflow.md](457-document-tier-translation-workflow.md) | 2026-06-22 | `historical` | Design of the document tier (2 static FR HTML pages): extraction → DatasetUpdater → re-import. String-tier CSV dialect does not fit prose. |
| [457-t1-site-content-pipeline.md](457-t1-site-content-pipeline.md) | 2026-10-08 | `historical` | T1, first brick: the **2sxc export ⇄ CSV ⇄ import XML** pipeline and its offline proof. |
| [457-t2-app33-first-lot.md](457-t2-app33-first-lot.md) | 2026-10-09 | `historical` | T2 first real lot on app 33: pipeline adapted, then blocked on brick 1 (the export). |
| [457-t2-pivot-worklist-2026-10-10.md](457-t2-pivot-worklist-2026-10-10.md) | 2026-10-10 | `historical` | T2 brick 2: CSV pivots of app 33 proven (one entity), measured worklist delivered. |
| [457-t3-globalresources-arbitrage-dossier-2026-10-10.md](457-t3-globalresources-arbitrage-dossier-2026-10-10.md) | 2026-10-10 | `historical` | Arbitration dossier for `GlobalResources.<culture>.resx`: composition, real surface, upstream. Reco: upstream es+pt installs are a lane gesture on preprod; production needs the owner GO. |
| [457-t3-ui-labels-measured.md](457-t3-ui-labels-measured.md) | 2026-10-10 | `historical` | T3 measure-only: which UI labels are done, which are a no-op, which remain. Erratum of 10/10 after counter-review. |
| [457-p24-01-residu-visible-traductions-2026-11-14.md](457-p24-01-residu-visible-traductions-2026-11-14.md) | 2026-11-14 | `historical` | P24-01: the visible part of the langpack residue, translated. |

---

## §3 — Field models, provisioning, DNN inventories (#490 / #506 / #682 / #683 / #684)

Four of these still **govern a gesture to come**: the owner window that provisions the field model.
Everything else is the record that led to it.

| File | Date | Status | What it is |
|---|---|---|---|
| [dossier-groupe-682-685-fenetre-owner.md](dossier-groupe-682-685-fenetre-owner.md) | 2026-09-14 | `current` | Grouped dossier for the owner window (i18n portage, steps 2–6): the sequencing to execute. |
| [682-685-dryrun-fenetre-owner.md](682-685-dryrun-fenetre-owner.md) | 2026-09-15 | `current` | Dry-run of that window (provision `Title_<lang>` + runtime control). Code (#674) and dossier (#1373) merged: **only the window remains**. |
| [682-path-a-provisioning-manifest.md](682-path-a-provisioning-manifest.md) | 2026-07-11 | `current` | Manifest of the **49 suffixed fields** (7 translatable × 7 non-FR languages) for the 2sxc editor. Gated ops. |
| [682-field-model-revision-2sxc21.md](682-field-model-revision-2sxc21.md) | 2026-07-09 | `current` | Revision of the field model against the 2sxc v21 export (#681): confirms Path A, 3 deltas corrected. The model the window applies. |
| [682-field-model-decision-input.md](682-field-model-decision-input.md) | 2026-07-05 | `superseded` → [682-field-model-revision-2sxc21.md](682-field-model-revision-2sxc21.md) | Decision input that chose Path A (lang-suffixed + `loc()` cascade); the revision carries the corrected model. |
| [490-res-rule-provisioning-manifest.md](490-res-rule-provisioning-manifest.md) | 2026-07-11 | `current` | Manifest of the `res.Rule*` DB writes (Δ1/Δ2). **Manifest only** — no write is executed from the repo; gated ops. |
| [683-content-language-enablement-design.md](683-content-language-enablement-design.md) | 2026-07-04 | `historical` | Design for enabling 7 cultures + routing + switcher. Measured on prod on 24/11: 7 language links per homepage, `hreflang` 0/8 (P23-32). |
| [684-rule-prose-translation-harness.md](684-rule-prose-translation-harness.md) | — | `historical` | Harness design (Option C, worker-side): 0 run, 0 prod CSV, 0 DB mutation. |
| [684-translation-manifest.md](684-translation-manifest.md) | 2026-07-05 | `historical` | Manifest/prep of the Rule prose translations (~30 entities × 7 languages). No translation executed at that date. |
| [684-scope-correction.md](684-scope-correction.md) | 2026-07-10 | `historical` | Addendum that corrects the manifest against the live export (#681): game count confirmed at 5, entity list. |
| [684-translation-run-report.md](684-translation-run-report.md) | 2026-07-10 | `historical` | Run report: 196/196 cell-languages translated (28 cells × 7 languages), prep delivered, 0 prod write. Re-import remains gated. |
| [684-translations.json](684-translations.json) | 2026-07-10 | `historical` | The 196 translated cells, as data. Consumed by the gated re-import, not by the site. |
| [490-res-rule-reconciliation.md](490-res-rule-reconciliation.md) | 2026-07-09 | `historical` | Reconciliation of the 7 `res.Rule*` FR values against live 2sxc v21 export #681; feeds the manifest above. |
| [506-social-auth-connectors-inventory.md](506-social-auth-connectors-inventory.md) | 2026-09-11 | `superseded` → [../dnn/506-social-auth-assessment.md](../dnn/506-social-auth-assessment.md) | Former social-auth connectors inventory. Its header names the canonical, re-measured assessment. |

---

## §4 — Measurements and records (#136 / #1180 / #1502 / #1781 / #2182)

| File | Date | Status | What it is |
|---|---|---|---|
| [136-2sxc-collaborative-scoping.md](136-2sxc-collaborative-scoping.md) | 2026-06-27 | `historical` | Read-only scoping of #136 (2sxc taxonomy entities + collaborative editing): what exists, what is missing, what is gated. |
| [1180-portallocalization-enus-culturename-zh-2026-10-25.md](1180-portallocalization-enus-culturename-zh-2026-10-25.md) | 2026-10-25 | `historical` | #1180 arbitration 7: the `PortalLocalization` en-US row and the zh-CN `CultureName` corrected. No production write (prod frozen at that date). |
| [1502-r-site-mesure-et-plan.md](1502-r-site-mesure-et-plan.md) | 2026-10-09 | `historical` | Measure of the Rules store and the write plan for #1502. |
| [1502-r-site-ecriture-2026-10-09.md](1502-r-site-ecriture-2026-10-09.md) | 2026-10-09 | `historical` | Write phase: the 6 Rules pages of the preprod synchronized to the cards. |
| [1781-i2-zone3-inventory.md](1781-i2-zone3-inventory.md) | 2026-10-09 | `historical` | Measured inventory of what the live portal's visible text is made of (zone 3). |
| [1781-i3-en-seed-verification-2026-10-09.md](1781-i3-en-seed-verification-2026-10-09.md) | 2026-10-09 | `historical` | Verification of the zone-2 EN seed. |
| [1781-i3-a-sans-consommateur-2026-10-09.md](1781-i3-a-sans-consommateur-2026-10-09.md) | 2026-10-09 | `historical` | Stop-before-write: the EN seed of the 1 013 fallacies has no consumer on the site. |
| [1781-i1b-culture-dimensions-2026-10-10.md](1781-i1b-culture-dimensions-2026-10-10.md) | 2026-10-10 | `historical` | The 6 culture dimensions provisioned in zone 3. |
| [1781-i3-en-regles-en-2026-10-11.md](1781-i3-en-regles-en-2026-10-11.md) | 2026-10-11 | `historical` | 27 en-US values written for the 5 Rules sheets. |
| [2182-articles-actus-et-liens-mindmap-2026-11-09.md](2182-articles-actus-et-liens-mindmap-2026-11-09.md) | 2026-11-09 | `historical` | 7 Actus articles and 7 mind-map links, measured in the window of 09/11. |
| [2182-plan-injection-147-cellules-2026-11-11.md](2182-plan-injection-147-cellules-2026-11-11.md) | 2026-11-11 | `current` | Injection plan for the 147 cells, replayable from the repo. Source = the `eav-content.csv` registry; the apply is still to come (option (c) at the apply). |

---

## §5 — Data files

| File | Date | Status | What it is |
|---|---|---|---|
| [dnn-ui-strings.csv](dnn-ui-strings.csv) | 2026-10-26 | `current` | The 11-row UI-strings artifact of #490 (2 `ui.*` + 9 `res.*`), 8 languages. ⛔ Never touch — see §7. |
| [eav-content.csv](eav-content.csv) | 2026-11-19 | `current` | The **injection registry** (2sxc EAV pivot, 8 language columns). Read by the 147-cell plan and by `tools/458-eav-content-export.py verify`. |
| [download-cards-canon.csv](download-cards-canon.csv) | 2026-10-28 | `historical` | Canonical download labels (Tarot/Poker, full/light) in 8 languages. No wired consumer measured on 2026-11-24 (no reference in `.md`/`.py`/`.cs`/`.json`). |

---

## §6 — Subfolders

| Folder | Status | What it is |
|---|---|---|
| [release-validation/](release-validation/) | `current` | The site-validation dossier: **30 tracked files** (README + 3 guides + 26 exports). Release gate still active — the dossier prepares validation, it does not replace it. ⚠️ Its own README still carries a 2026-06-16 "Skeleton / site not publicly deployed" status that is **stale since the 24/11 swap**; the files themselves remain the support of the validation to come. |
| [preuves/](preuves/) | `historical` | 4 PNG evidence captures (ar RTL, EN/FR navbar at 1440 px, K2 ontology tree), 09–10/10. |

---

## §7 — Hard constraints (apply to every doc here)

- **No production content mutation without jsboige GO** — no DB write, no 2sxc content change, no live-site edit without explicit authorization. The runbooks were the *plan*; re-running them now is only for a re-deploy / rollback.
- **NEVER touch** `dnn-ui-strings.csv` (merged via #490; gpt-5.5 only).
- **Worker signals, does not declare PASS** — visual site verdict = jsboige/ai-01; prod operations = jsboige's ops task.
- **No production write by a worker lane** — production execution is web2's; a worker lane prepares, measures and hands over.

---

## §8 — Remaining tranches

Tranche 1 (this file) covers the 49 root files that are not `458-*`, plus the two subfolders. Tranches 2–4
index the **134 `458-*` root files** (≈45 each), one PR at a time, each branched from master **after** the
previous one merged — never stacked (a chain on the same README re-conflicts at every squash, measured
23/11). The `<!-- index-of-record -->` mark is posed at the **last** tranche, and the DoD mutant —
"a row of a file cited only once removed, contract red and named" — is played on **that** PR, not on this
one. Until then this file is a navigation index that happens to carry statuses, not a contract.

The former navigation index is kept **byte for byte** at the end of this file, under *Archive*. It is not
indexed row by row: it is part of this file. It sits in a fenced block on purpose: the contract holds
every table row that links to a file of this folder to a status, and the rows of the former index carry
none — fenced, the record stays exact and the contract stays readable.

*po-2023 — pool #458, P23-34, tranche 1/4 · 24/11/2026.*

---

## Archive — former navigation index (replaced 2026-11-24, kept byte for byte as a dated record)

Last changed on master on 2026-07-13 (`b75df8326`, #798); blob `f569d598`. Nothing below this line is
edited: read it as the statement of that date.

````markdown
# DNN Localization & Upgrade Arc — Navigation Index

> **Entry point for the `#131` / `#132` / `#457` DNN documentation arc.**
> This is a **navigation index only** — it sequences the 15 documents in this folder in **go-live execution order** and flags what is superseded. Every line below points to a detailed doc; none rewrites its content. If a doc answers your question, open it.

**Current target:** **DNN 10.3.2 + 2sxc 21.07 LTS** (jsboige decision #2, issue #458, verified interactive channel 2026-06-18). The earlier **10.1.2** and **9.13.x** targets are **superseded** (see §3 below).

---

## §0.5 — Execution status (refresh 2026-07-12: the upgrade is DONE, B1-doc sweep extended)

> **The plan below was EXECUTED, not just written.** This section records what's actually live, so readers
> don't treat the spine as a TODO. The detailed runbooks (§1) remain the source of truth for *how*; this
> records *that it ran*.

| Milestone | Status | Evidence |
|-----------|--------|----------|
| **Phase 1.5 — 2sxc upgrade** | ✅ **DONE** | DB already at **2sxc 21.07.00** at upgrade time (memory "15.02" was stale). |
| **Phase 2 — DNN exec** | ✅ **DONE** | Sandbox wizard automated (`wiz_runupgrade.ps1`, 9.11.1→9.13.10→10.2.0→**10.3.2** on IIS Express :8090, 2026-06-28). `dbo.[Version]` top row = **10.3.2**. |
| **Phase 4 — sandbox smoke** | ✅ **DONE (technical GREEN)** | Homepage 200/86 KB, `/Argumentum` + `/Règles` render 2sxc content, 0 JsonOptions/conn-string errors, no IIS crash (cliff clear). **Visual PASS = jsboige (pending)**. |
| **Full-IIS migration** | ✅ **DONE (2026-07-01)** | `dnn.argumentum.myia.io` LIVE full-IIS direct (HTTP 200/85 KB, 0× "Something went wrong"). Stopgap `dnn.myia.io` retired + PortalAlias 1010 dropped. HTTPS SAN 9D80D4CC (exp 2026-09-27). **ACME bypass active** (`.well-known/acme-challenge/web.config`, functional for win-acme renew 2026-08-23). |
| **B1 inversion fix** | ✅ **DONE** | `repair-bin-net48.ps1` had reverted BCL DLLs to 6.0.0.0 (treating .NET 8/9 as "contamination" — **wrong for 2sxc-21**); fixed by deploying the matched BCL stack (Json 9.0.0.0/SCI 9.0.0.0/Bcl 8.0.0.0) from the 2sxc 21.07 Install pkg. See `[[reference-dnn-2sxc-net48-bcl-stack]]`. |
| **Canonical working `bin/`** | ✅ | `tmp/dnn-backups/bin_post_2sxc_realign` (330 files). Runtime branch: `dnn/sandbox-runtime-1032`. |
| **Prod go-live** | ⏸ **PENDING (ops, jsboige VPS)** | Migration is complete; remaining = **visual site verdict (jsboige/ai-01)** + prod go-live (ops VPS task). NOT blocking v0.9.0 assets (reco: de-couple). |

> ✅ **B1-inversion correction — sweep complete across 3 docs (refresh 2026-07-12):** the inverted B1
> thesis (revert BCL DLLs to 6.0.0.0 treating .NET 8/9 as "contamination" — **wrong for 2sxc-21**) was
> encoded in 3 docs; all now carry the correct fix inline (deploy the matched BCL stack — Json 9.0.0.0 /
> SCI 9.0.0.0 / Bcl 8.0.0.0 — from the 2sxc 21.07 Install pkg):
>
> - [`docs/dnn/sandbox-bootstrap-runbook.md`](../dnn/sandbox-bootstrap-runbook.md) §2/§3 — ⛔ SUPERSEDED callout (#765).
> - [`docs/dnn/go-live-turnkey-checklist.md`](../dnn/go-live-turnkey-checklist.md) B1 — ⛔ CRITICAL CORRECTION (#765, extended #794).
> - [`132-deployment-runbook.md`](132-deployment-runbook.md) §5.5(a) — B1-inversion lesson added as a MANDATORY sub-step (#792).
>
> The executable [`repair-bin-net48.ps1`](../dnn/repair-bin-net48.ps1) was **deprecated in #624** (2026-07-01,
> `-Apply` refuses). The `132-deployment-runbook.md` title was retargeted to 10.3.2 (2026-06-24).
> (`131-step1-sandbox-upgrade-runbook.md` carries no BCL versions — DNN-version-scoped, own TARGET-SUPERSEDED header.)
> See `[[reference-dnn-2sxc-net48-bcl-stack]]` for the authoritative version matrix.

---

## §1 — Read this first (the spine)

Follow these in order. Each is the authoritative doc for one execution phase.

| # | Phase | Doc | What it decides / does |
|---|-------|-----|------------------------|
| 0 | **Runtime fact** | [131-step0-runtime-verification.md](131-step0-runtime-verification.md) | Verified: DNN 10.x runtime = **.NET Framework 4.8** (not .NET 8). Foundation of the whole plan. |
| 1 | **Target decision** | [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | **DECISION-SPEC.** Picks 10.3.2 over 10.1.2. Names the 2sxc cliff + the 2sxc-first ordering. Start here after Step 0. |
| 2 | **CVE reconciliation** | [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) | Corrects CVE-patched-version errors; refines target using the 2sxc cliff. Feeds #1. |
| 3 | **Phase 1.5 — 2sxc upgrade** | [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) | Step-by-step **2sxc 15.02 → 21.07** BEFORE crossing 10.2.0. De-risks the cliff. |
| 4 | **Phase 2 — DNN exec + rollback** | [131-dnn-phase2-exec-rollback.md](131-dnn-phase2-exec-rollback.md) | DNN 9.11.1 → 10.3.2 execution sequence + 2-anchor rollback runbook. **(§4.2 RESTORE order fixed in PR #552.)** |
| 5 | **Phase 4 — sandbox smoke gate** | [131-step2-smoke-test-checklist.md](131-step2-smoke-test-checklist.md) | Consolidated, tickable smoke-test to run **after** Step-1 upgrade, **before** prod go-live. |
| 6 | **Phase 5 — prod go-live** | [132-deployment-runbook.md](132-deployment-runbook.md) | Production deployment runbook. ⚠ **Title still says "10.1.2"** — apply with target 10.3.2 (see §3). |

---

## §2 — Supporting / scope docs

| Doc | Role |
|-----|------|
| [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) | The merged "v2" sandbox plan + go/no-go matrix. Reference for the palier logic. |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) | Sandbox upgrade runbook (was 10.1.2-scoped; exec gated on target decision). |
| [131-regen-staging-runbook.md](131-regen-staging-runbook.md) | Stages prerequisites for an attended mindmap/PDF regen — **no launch**. |
| [682-field-model-decision-input.md](682-field-model-decision-input.md) | **#682** Rule content-type field-model decision → **Path A** (lang-suffixed + `loc()` cascade). Ratified. Feeds #674 runtime + #684 translation. |
| [682-field-model-revision-2sxc21.md](682-field-model-revision-2sxc21.md) | **#682** revision against 2sxc v21 export (#681) — confirms Path A, 3 deltas corrected. |
| [682-path-a-provisioning-manifest.md](682-path-a-provisioning-manifest.md) | **#682** provisioning manifest: **49 new suffixed fields** (7 translatable × 7 non-FR langs) for jsboige to provision via 2sxc content-type editor. GATED ops. |
| [457-site-content-type-inventory.md](457-site-content-type-inventory.md) | Localization scope map (extends the Phase 1 audit, feeds the extractor). |
| [457-document-tier-translation-workflow.md](457-document-tier-translation-workflow.md) | **#457 Phase 2–4** for the **document tier** (2 static FR HTML pages): extraction → DatasetUpdater → re-import workflow design. Complements the string-tier toolchain; extractor = critical path. |
| [PHASE1-content-audit.md](PHASE1-content-audit.md) | Phase 1 content extraction audit (#457). Repo-side, complete. |
| [506-social-auth-connectors-inventory.md](506-social-auth-connectors-inventory.md) | **#506** social-auth (Facebook/Google/Microsoft/Twitter) inventory + secret-rotation procedure. Repo-grounded; live console checks = jsboige. Recommend folding into #131. |

---

## §3 — Superseded (read for history, do NOT execute)

| Doc | Superseded by | Why |
|-----|---------------|-----|
| [131-upgrade-9.13-sandbox-plan.md](131-upgrade-9.13-sandbox-plan.md) | #1 (target revision) | 9.13.x closes **0** CVEs (NVD/GHSA correction); target is now 10.3.2. |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) §target | #1 + #4 | 10.1.2 target abandoned → 10.3.2. Runbook mechanics still valid; re-point the version. |
| [132-deployment-runbook.md](132-deployment-runbook.md) title | #1 | Header reads "10.1.2 go-live"; actual target is **10.3.2**. Content applies, version label is stale. |

---

## §4 — jsboige gates still pending (block prod go-live)

These are **business / content decisions only jsboige can make**. (The **upgrade itself is done** — see §0.5; these are the residual gates.)

1. **#445 — Stripe vs OpenStore** (keep OpenStore vs Stripe managed). Business decision. See [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) § "unblocked items".
2. **#525 — "Materiel" typo fix** (DB / 2sxc content). **= `res.RuleMaterial` Δ1** flagged in [490-res-rule-reconciliation.md](490-res-rule-reconciliation.md) (DB `Materiel` typo → `Matériel`). jsboige arbitration. Part of the 4 DNN res.Rule* deltas (Material typo · MemoCard case · FileNamePrefix brand · RuleContent view-bug).
3. **#682 — Rule field-model provisioning** (2sxc content-type editor). Provision the **49 lang-suffixed fields** listed in [682-path-a-provisioning-manifest.md](682-path-a-provisioning-manifest.md) (Path A, ratified). DB-side ops, gated jsboige. Unblocks #674 runtime validation + #684 translation population.
4. **#683 — DNN content-language enablement + URL routing + switcher** (jsboige). The i18n portage prerequisite that makes the 49 provisioned fields render per-culture. Gates #684 re-import.
5. **#684 — Rule prose translation + re-import** (7 langs × ~30 entities). Prep DONE (PR #767, 0 prod write); the **re-import to 2sxc is GATED** on #682 provisioning + #683 enablement + jsboige GO. See [457-document-tier-translation-workflow.md](457-document-tier-translation-workflow.md).
6. **Visual site verdict** (jsboige/ai-01) + **prod go-live** (ops VPS). The last non-calendaire gate.

---

## §5 — Release validation (separate dossier)

The `release-validation/` subfolder has its **own** [README](release-validation/README.md) covering: 2sxc export spec, 8-language checklist, non-Latin (ar/fa/zh/+ru) verification guide.

---

## §6 — Hard constraints (apply to every doc here)

- **The upgrade is EXECUTED** (§0.5) but **prod content mutation still requires jsboige GO** — no DB write, no 2sxc content change, no live-site edit without explicit authorization. The runbooks here were the *plan*; re-running them now is only for a re-deploy / rollback.
- **NEVER touch** `dnn-ui-strings.csv` (merged via #490; gpt-5.5 only).
- **Worker signals, does not declare PASS** — visual site verdict = jsboige/ai-01. DNN prod go-live = jsboige ops task.
````
