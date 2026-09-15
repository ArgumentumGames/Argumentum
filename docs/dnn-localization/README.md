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
