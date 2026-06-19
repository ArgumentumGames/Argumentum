# DNN Localization & Upgrade Arc — Navigation Index

> **Entry point for the `#131` / `#132` / `#457` DNN documentation arc.**
> This is a **navigation index only** — it sequences the 15 documents in this folder in **go-live execution order** and flags what is superseded. Every line below points to a detailed doc; none rewrites its content. If a doc answers your question, open it.

**Current target:** **DNN 10.3.2 + 2sxc 21.07 LTS** (jsboige decision #2, issue #458, verified interactive channel 2026-06-18). The earlier **10.1.2** and **9.13.x** targets are **superseded** (see §3 below).

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
| [457-site-content-type-inventory.md](457-site-content-type-inventory.md) | Localization scope map (extends the Phase 1 audit, feeds the extractor). |
| [PHASE1-content-audit.md](PHASE1-content-audit.md) | Phase 1 content extraction audit (#457). Repo-side, complete. |

---

## §3 — Superseded (read for history, do NOT execute)

| Doc | Superseded by | Why |
|-----|---------------|-----|
| [131-upgrade-9.13-sandbox-plan.md](131-upgrade-9.13-sandbox-plan.md) | #1 (target revision) | 9.13.x closes **0** CVEs (NVD/GHSA correction); target is now 10.3.2. |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) §target | #1 + #4 | 10.1.2 target abandoned → 10.3.2. Runbook mechanics still valid; re-point the version. |
| [132-deployment-runbook.md](132-deployment-runbook.md) title | #1 | Header reads "10.1.2 go-live"; actual target is **10.3.2**. Content applies, version label is stale. |

---

## §4 — jsboige gates still pending (block prod go-live)

These are **business / content decisions only jsboige can make**. Doc execution stops here until resolved.

1. **#445 — Stripe vs OpenStore** (keep OpenStore vs Stripe managed). Business decision. See [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) § "unblocked items".
2. **#525 — "Materiel" typo fix** (DB / 2sxc content). Gated to the DNN upgrade session (jsboige decision #7).

---

## §5 — Release validation (separate dossier)

The `release-validation/` subfolder has its **own** [README](release-validation/README.md) covering: 2sxc export spec, 8-language checklist, non-Latin (ar/fa/zh/+ru) verification guide.

---

## §6 — Hard constraints (apply to every doc here)

- **DNN = sandbox / research / doc only.** NO deploy, NO content mutation, NO live sandbox upgrade without **explicit jsboige GO**.
- **NEVER touch** `dnn-ui-strings.csv` (merged via #490; gpt-5.5 only).
- Doc execution is **gated step-by-step** on jsboige go/no-go.
