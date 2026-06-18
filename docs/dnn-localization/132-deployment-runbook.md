# #132 — Production Deployment Runbook (DNN 9.11.1 → 10.1.2 go-live)

**Issue:** [#132 — Production deployment](https://github.com/ArgumentumGames/Argumentum/issues/132)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-18
**Base:** master `132ffea3`
**Status:** RUNBOOK / PLAN (docs, non-gated). **Execution stays GATED** — no backup, no restore, no
upgrade, no go-live on production without jsboige's explicit go. This is the **go-live procedure**,
distinct from the sandbox-upgrade procedure ([#522](131-step1-sandbox-upgrade-runbook.md)).

> **Migration strategy (jsboige, issue #132 body):** recover the production database → restore to
> dev/staging → analyze schema & data → migration plan (dev↔prod delta) → test on the backup →
> go-live. This runbook follows that 6-step shape and flags every section that can only be completed
> against the **real prod DB export**.

---

## 1. Purpose

The deploy runbook for taking the DNN platform from **9.11.1.19 → 10.1.2** on production. It is the
**go-live** counterpart to the prep arc (#511 plan → #514 runtime → #520 CVE/target → #522 sandbox
procedure) and closes #132 (Production deployment), which until now had only a validation dossier
(#492) and **no runbook**.

**Why 10.1.2 (not 10.3.2):** per [#520](131-cve-correction-and-target-refinement.md) the 9.13.x
palier closes **0** of the 2 critical CVEs (CVE-2025-64095 CVSS 10.0, CVE-2025-52488 CVSS 8.6);
**10.1.2** is the first target closing **both**, and it sits **before** the 2sxc compatibility cliff
at **10.2.0+** (which would crash 2sxc 15.02 via `DnnJsInclude` and force a 2sxc ≥21 upgrade + template
audit). The 10.3.2 alternative is documented in §9 for if jsboige chooses to cross the cliff.

## 2. What this runbook does NOT duplicate

| Doc | Covers | Role here |
|-----|--------|-----------|
| [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) | Go/no-go plan, 2 reversals (CVE + .NET) | the *why* — referenced, not repeated |
| [131-step0-runtime-verification.md](131-step0-runtime-verification.md) (#514) | Runtime = .NET Framework 4.8 (VERIFIED) | prerequisite fact |
| [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) | Target = 10.1.2, CVE correction | target rationale |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) (#522) | **Sandbox** upgrade procedure (9.11.1.19 → 10.1.2) | the *how* for the staging test (Phase 4) |
| **This runbook** | **Production go-live** (backup → restore → migrate → go-live → rollback) | 🆕 net-new |

## 3. Known production topology (repo/memory-grounded)

| Component | Value | Grounding |
|-----------|-------|-----------|
| DNN Platform | **9.11.1.19** | VERIFIED (#514, `DotNetNuke.dll` FileVersion) |
| Runtime | .NET Framework **4.8** | VERIFIED (`web.config targetFramework="4.8"`) |
| DB backend | **SQL Server** (`System.Data.SqlClient`) | repo `web.config` (connectionString sanitized to `Data Source=REPLACE` — real value prod-only) |
| 2sxc | **15.02**, **26 apps** installed (only `Argumentum` custom) | VERIFIED (#525, `Portals/1/2sxc/`) |
| Eshop | **NBrightBuy/OpenStore** + `OS_Stripe.dll` + `Stripe.net.dll` | repo `DNNPlatform/bin/`, `DesktopModules/NBright/OS_Stripe` |
| Custom-template migration risk | `_FallacyExplorer_Root.cshtml` (legacy `RazorComponent`) | issue #131 body |
| Host | VPS **myia-web1**, IIS | RAPPORTÉ (memory — Phase A booted IIS Express :8090 2026-06-02) |

> **§3 honesty note:** the real production connection string, DB name, DB size, and row counts are
> **not in the repo** (the export sanitizes them). Everything in §3 that is not marked VERIFIED is
> RAPPORTÉ and must be confirmed against the prod export at Phase 2.

## 4. The 2sxc-migration simplification (key grounded insight)

**For target 10.1.2, no 2sxc entity migration is required.** 2sxc 15.02 survives a DNN upgrade to
10.1.2 because 10.1.2 is **before** the 10.2.0 cliff. The DNN upgrade wizard migrates DNN's own SQL
schema; 2sxc's content (`ToSic_EAV_*` + app tables) is untouched and remains valid on 2sxc 15.02.

- ✅ **In scope for 10.1.2:** DNN platform schema migration (wizard-handled) + **data preservation**
  (all 26 apps' content rows, all portal/page settings, all users).
- ❌ **NOT in scope for 10.1.2:** 2sxc content-type/entity/metadata migration (that is a 2sxc-version
  upgrade task, only triggered if jsboige later targets 10.3.2 — see §9).
- ⚠️ **Still verify** post-upgrade that 2sxc 15.02 still serves the `Argumentum` app templates (the
  `_FallacyExplorer_Root.cshtml` legacy RazorComponent must still bind). This is a smoke-test item
  (Phase 5), not a migration step.

## 5. Phase-by-phase go-live procedure

Each phase has a **go/no-go gate**. Do not start the next phase until the current gate is green and
jsboige has signed off (execution is GATED throughout).

### Phase 0 — Pre-flight (repo-grounded, can be prepared now)

- [ ] Confirm rollback-readiness prerequisites: enough disk on prod for a full files+DB backup, and an
      off-host copy target.
- [ ] Inventory the deployable artifacts: the 602 MB git-tracked export (RAPPORTÉ issue #131) +
      `DNNPlatform/bin/` (DotNetNuke 9.11.1.19, OS_Stripe, Stripe.net).
- [ ] Confirm the sandbox test (Phase 4 below / [Step-1 runbook #522](131-step1-sandbox-upgrade-runbook.md))
      has **passed** before touching prod.
- [ ] Snapshot the current prod versions (DNN, 2sxc, OpenStore) into the deploy log.

### Phase 1 — Production backup (files + DB)  — *jsboige step 1*

**Two independent backups; both must succeed and be checksum-verified before Phase 2.**

1. **Files backup** — copy the entire DNN web root (`DNNPlatform/` equivalent on prod) off-host:
   `Portals/`, `bin/`, `web.config`, `DesktopModules/`. Tar + checksum (`sha256sum`).
2. **DB backup** — SQL Server native: `BACKUP DATABASE [DotNetNuke] TO DISK = '…\pre-upgrade.bak'
   WITH FORMAT, INIT, CHECKSUM, STOP_ON_ERROR`. Verify `RESTORE VERIFYONLY`.
3. **Store** both backups + checksums in **two** locations (on-host + off-host). Record paths in the
   deploy log. **This is the rollback anchor** — losing it means no rollback.

> ⚠️ The exact DB name / connection is **prod-only** (sanitized in repo). Phase 1 is DB/RDP-gated and
> must be run by jsboige against the live VPS.

### Phase 2 — Staging restore + schema analysis  — *jsboige steps 2-3*

1. Restore the Phase-1 DB backup to the **staging** SQL instance (not prod).
2. Restore the files backup to the staging web root.
3. **Analyze the prod schema & data** — table inventory, 2sxc app content row counts (especially
   `Argumentum` + `Glossary3`), portal/page settings, user counts.

> 🚧 **HONESTY BOUNDARY — DB/RDP-gated:** the actual prod schema, the 2sxc content-type/entity/metadata
> row counts, and the dev↔prod delta **cannot be produced from the repo** (the export is sanitized and
> the repo holds templates, not portal content — confirmed by [inventory #525]). These sections are
> **to be filled in against the real prod DB export**. The runbook provides the *procedure* and the
> *topology* (§3); the *measurements* are jsboige's, gated.

### Phase 3 — Migration plan (dev↔prod delta)  — *jsboige step 4*

- [ ] **Schema delta** — diff staging-restored-prod-schema vs the dev sandbox schema. Identify any
      DNN customizations (custom modules, extra columns) that the upgrade wizard must preserve.
- [ ] **Data-preservation rules** — which tables are user-content (preserve verbatim) vs
      framework-config (wizard-managed). For 10.1.2 the split is: 2sxc content = preserve; DNN core
      tables = wizard-managed (no manual edit).
- [ ] **2sxc scope** — confirm §4: no 2sxc migration for 10.1.2; only smoke-test the app post-upgrade.
- [ ] **Eshop scope** — confirm OpenStore 4.1.11 + OS_Stripe survive the DNN 10.1.2 upgrade (Phase B
      eshop decision, #131, is **separate** — Stripe Native #445 is re-opened but not blocking the
      platform upgrade).

### Phase 4 — Sandbox test migration  — *jsboige step 5*

Run the [Step-1 sandbox runbook (#522)](131-step1-sandbox-upgrade-runbook.md) on the staging copy
(**not** the empty sandbox — on the prod-restored data). Validate:
- [ ] Upgrade wizard completes 9.11.1.19 → 10.1.2, no fatal errors.
- [ ] Site boots, homepage renders, language switcher works (FR + en-US baseline).
- [ ] 2sxc `Argumentum` app templates still bind (`_FallacyExplorer_Root.cshtml`).
- [ ] OpenStore + Stripe checkout path loads (no IIS crash from `DnnJsInclude`).
- [ ] **Go/no-go gate → jsboige.** Only on green do we proceed to Phase 5.

### Phase 5 — Production go-live  — *jsboige step 6*

1. **Maintenance mode** — put the prod site in DNN maintenance/under-construction (visitors see the
   holding page; admin stays functional).
2. **Re-backup prod** (Phase 1 again) — captures any data written between the Phase-1 backup and now.
   This second backup is the **true rollback anchor**.
3. **Apply the upgrade** — same wizard path proven in Phase 4, on prod, to 10.1.2.
4. **Smoke test on prod** — homepage, language switcher, 2sxc Argumentum app, OpenStore/Stripe path.
5. **Exit maintenance mode** — only after the smoke test is green.
6. **Record** final versions + timestamps in the deploy log.

### Phase 6 — Rollback strategy (the contract that makes go-live reversible)

Rollback-readiness is **per phase**; if any phase fails, roll back to the last green anchor:

| Failed phase | Rollback action | Anchor |
|--------------|-----------------|--------|
| Phase 1 (backup fails) | **Do not proceed** — nothing changed yet | (none needed) |
| Phase 4 (sandbox test fails) | Discard staging; prod untouched | prod (unchanged) |
| Phase 5 wizard fails | `RESTORE DATABASE` from Phase-5 re-backup + restore files backup | Phase-5 re-backup |
| Phase 5 smoke-test fails post-wizard | Same `RESTORE` + files restore; keep maintenance mode | Phase-5 re-backup |

**Rollback contract:** the Phase-5 re-backup must exist, be checksum-verified, and be tested by a
`RESTORE VERIFYONLY` **before** exiting maintenance mode. If rollback is not possible, do not exit
maintenance mode — escalate to jsboige.

## 6. Go/no-go checklist (jsboige ticks at go-live)

- [ ] Phase 0 pre-flight complete; sandbox test (Phase 4) **passed**.
- [ ] Phase 1 prod backup done, **two copies**, checksum-verified, `RESTORE VERIFYONLY` green.
- [ ] Phase 2 staging restore + schema analysis done (against **real** prod DB export).
- [ ] Phase 3 migration plan signed off (schema delta, data-preservation, 2sxc = no-op for 10.1.2).
- [ ] Phase 4 sandbox test on prod-restored data **passed** (2sxc app + OpenStore smoke green).
- [ ] Phase 5 re-backup taken; `RESTORE VERIFYONLY` green; rollback path confirmed.
- [ ] Maintenance window scheduled; jsboige **go** for go-live.

## 7. Honesty boundary — sections gated on the real prod DB export

These cannot be completed from the repo and are left as **procedure**, to be filled against the prod
export (the export in-repo is sanitized — `Data Source=REPLACE`, templates-only per #525):

- §3 real connection string / DB name / DB size / row counts (RAPPORTÉ, not VERIFIED).
- §5 Phase 2 actual prod schema + 2sxc content row counts.
- §5 Phase 3 actual dev↔prod schema delta.
- §5 Phase 4 sandbox-test outcome on prod-restored data.

The **procedure, rollback strategy, topology, and the 2sxc no-op insight (§4)** are repo/memory-grounded
and hold on their own.

## 8. Gate boundaries

- ❌ Does **not** deploy, back up, restore, or upgrade anything on production (execution GATED jsboige).
- ❌ Does **not** touch the live DB / RDP / portal (all DB steps are described, not performed).
- ❌ Does **not** duplicate the sandbox procedure (#522) or the plan (#511) — it references them.
- ❌ Does **not** decide the eshop (Phase B) or the 10.1.2-vs-10.3.2 target — those are jsboige gates.
- ❌ Does **not** declare a QA verdict (ai-01 only).

## 9. Alternative target: 10.3.2 (if jsboige crosses the 2sxc cliff)

If jsboige chooses **10.3.2** instead of 10.1.2, the runbook gains two gated phases:

- **2sxc upgrade 15.02 → 21.07 LTS** before the DNN upgrade past 10.2.0 (else `DnnJsInclude` crashes
  IIS — the cliff).
- **2sxc template audit** — `_FallacyExplorer_Root.cshtml` (`@inherits ToSic.Sxc.Dnn.RazorComponent`)
  → migrate to `Custom.Hybrid.Razor14` (issue #131 body lists the APIs that survive in 2sxc 21:
  `App.Query`, `AsList`, `CmsContext`, `Link.To`, `Edit.TagToolbar`).
- The §4 2sxc-no-op insight **no longer holds** — full 2sxc content-type/entity/metadata migration
  enters scope (run on the staging-restored prod DB first, Phase 4).

10.1.2 is recommended because it avoids all of §9.

## Sources

- Repo: `DNNPlatform/web.config` (SQL Server `System.Data.SqlClient`, `targetFramework="4.8"`),
  `DNNPlatform/Portals/1/2sxc/` (26 apps), `DNNPlatform/bin/` (DotNetNuke, OS_Stripe, Stripe.net),
  `DNNPlatform/DesktopModules/NBright/OS_Stripe`.
- [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) ·
  [131-step0-runtime-verification.md](131-step0-runtime-verification.md) (#514) ·
  [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) ·
  [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) (#522) ·
  [457-site-content-type-inventory.md](457-site-content-type-inventory.md) (#525 — repo = templates,
  not portal content).
- Issue #132 body (jsboige 6-step migration strategy) · Issue #131 body (topology, eshop, i18n scope).
- Memory: DNN Phase A IIS Express boot (2026-06-02), VPS myia-web1, 2sxc cliff @10.2.0 (#6902).
