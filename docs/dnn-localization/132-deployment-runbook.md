# #132 — Production Deployment Runbook (DNN 9.11.1 → 10.3.2 go-live)

**Issue:** [#132 — Production deployment](https://github.com/ArgumentumGames/Argumentum/issues/132)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-18 (refreshed 2026-06-24 — target retarget 10.1.2 → 10.3.2)
**Base:** master `fa231121`
**Status:** RUNBOOK / PLAN (docs, non-gated). **Execution stays GATED** — no backup, no restore, no
upgrade, no go-live on production without jsboige's explicit go. This is the **go-live procedure**,
distinct from the sandbox-upgrade procedure ([#522](131-step1-sandbox-upgrade-runbook.md)).

> ## ⚠️ Target retarget — 10.1.2 → 10.3.2 (refreshed 2026-06-24)
>
> jsboige interactive decision **#2** (issue #458, 2026-06-18) chose the **full upgrade to
> 10.3.2-latest + 2sxc 15→≥21 + template audit**. **This runbook now reflects that decision.** The
> earlier draft framed **10.1.2** as the primary target and 10.3.2 as a speculative alternative —
> that framing is **retracted** (it was written before the #458 decision landed in the body).
>
> **What changed in this refresh:**
> - The **target is 10.3.2** throughout (§1, §5, §6).
> - **§4 "2sxc no-op for 10.1.2" is RETRACTED** — 10.3.2 crosses the 2sxc compatibility cliff at
>   10.2.0, so the **2sxc 15.02 → 21.07 upgrade + the 12-template audit are real steps** (now §5.5),
>   not a deferred alternative.
> - CVE facts corrected against NVD/GHSA (see [131-cve-correction-and-target-refinement.md]
>   (#520) + the [UPGRADE-ASSESSMENT](../dnn/UPGRADE-ASSESSMENT.md) §3, PR #593): 9.13.x closes
>   **0** of the 2 critical CVEs; the security floor is 10.1.2; 10.3.2 is above it.
> - The **6-phase go-live procedure, the rollback contract, the runtime finding (.NET 4.8), and the
>   honesty boundary (DB-gated sections) remain valid** and unchanged in structure.
>
> **Migration strategy (jsboige, issue #132 body):** recover the production database → restore to
> dev/staging → analyze schema & data → migration plan (dev↔prod delta) → test on the backup →
> go-live. This runbook follows that 6-step shape and flags every section that can only be completed
> against the **real prod DB export**.

---

## 1. Purpose

The deploy runbook for taking the DNN platform from **9.11.1.19 → 10.3.2** on production, including
the mandatory **2sxc 15.02 → 21.07** upgrade (crossing the 10.2.0 cliff) and the **12 custom-template
audit**. It is the **go-live** counterpart to the prep arc (#511 plan → #514 runtime → #520 CVE/target
→ #522 sandbox procedure) and closes #132 (Production deployment), which until now had only a
validation dossier (#492) and **no runbook**.

**Why 10.3.2 (actée #458), not 10.1.2:** per [#520](131-cve-correction-and-target-refinement.md) +
[UPGRADE-ASSESSMENT §3](../dnn/UPGRADE-ASSESSMENT.md) (PR #593), the 9.13.x palier closes **0** of the
2 critical CVEs (CVE-2025-64095 RCE CVSS **10.0**, patched 10.1.1 ; CVE-2025-52488 NTLM CVSS **8.6**,
patched 10.0.1) — the security floor is **10.1.2**. jsboige chose **10.3.2** (decision #458): it is
above the floor, is the latest, and the accepted cost is crossing the **2sxc compatibility cliff at
10.2.0+** (which crashes 2sxc 15.02 via `DnnJsInclude`). That cost is paid **upfront** here: a 2sxc
15→21 upgrade + a 12-template audit **before** the DNN upgrade (§5.5). 10.1.2 was the "avoid the cliff"
option; jsboige chose to cross it deliberately.

## 2. What this runbook does NOT duplicate

| Doc | Covers | Role here |
|-----|--------|-----------|
| [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) | Go/no-go plan, 2 reversals (CVE + .NET) | the *why* — referenced, not repeated |
| [131-step0-runtime-verification.md](131-step0-runtime-verification.md) (#514) | Runtime = .NET Framework 4.8 (VERIFIED) | prerequisite fact |
| [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) | CVE correction, target refinement | CVE/target rationale |
| [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) | **2sxc 15.02 → 21.07** migration (the cliff de-risk) | the *how* for §5.5 |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) (#522) | **Sandbox** upgrade procedure | the *how* for the staging test (Phase 4) |
| [../dnn/UPGRADE-ASSESSMENT.md](../dnn/UPGRADE-ASSESSMENT.md) (#593) | CVE patch levels + .NET 4.8 + coupling reco | corrected target + floor |
| **This runbook** | **Production go-live** (backup → restore → 2sxc upgrade → migrate → go-live → rollback) | 🆕 net-new |

## 3. Known production topology (repo/memory-grounded)

| Component | Value | Grounding |
|-----------|-------|-----------|
| DNN Platform | **9.11.1.19** | VERIFIED (#514, `DotNetNuke.dll` FileVersion) |
| Runtime | .NET Framework **4.8** | VERIFIED (`web.config targetFramework="4.8"`; DNN 10.x nupkg = `lib/net48/`, #593) |
| DB backend | **SQL Server** (`System.Data.SqlClient`) | repo `web.config` (connectionString sanitized to `Data Source=REPLACE` — real value prod-only) |
| 2sxc | **15.02**, **26 apps** installed (only `Argumentum` custom) | VERIFIED (#525, `Portals/1/2sxc/`) — **prod**; the §5.5 upgrade raises this to 21.07 |
| Eshop | **NBrightBuy/OpenStore** + `OS_Stripe.dll` + `Stripe.net.dll` | repo `DNNPlatform/bin/`; **runs on 10.x** (.NET 4.8, #593 — blocker dissolved) |
| Custom-template migration risk | 12 Razor templates incl. `_FallacyExplorer_Root.cshtml` (legacy `RazorComponent`) | issue #131 body; ~4-6h code-only (UPGRADE-ASSESSMENT §10) |
| Host | VPS **myia-web1**, IIS | RAPPORTÉ (memory — Phase A booted IIS Express :8090 2026-06-02) |

> **§3 honesty note:** the real production connection string, DB name, DB size, and row counts are
> **not in the repo** (the export sanitizes them). Everything in §3 that is not marked VERIFIED is
> RAPPORTÉ and must be confirmed against the prod export at Phase 2.

## 4. The 2sxc-migration scope (RETRACTED no-op, now IN scope)

> **RETRACTION (2026-06-24):** the earlier draft of this section claimed *“for target 10.1.2, no 2sxc
> entity migration is required — 10.1.2 is before the cliff.”* That was correct **only for 10.1.2**,
> the rejected option. For the actée target **10.3.2**, the opposite holds.

**For target 10.3.2, the 2sxc 15.02 → 21.07 upgrade IS required** because 10.3.2 is **past** the
10.2.0 cliff. A DNN upgrade wizard to 10.3.2 with 2sxc 15.02 still installed will crash IIS via the
`DnnJsInclude` incompatibility. The mitigations are real steps, done **in this order**, on staging
first (Phase 4) then prod (Phase 5):

- ✅ **In scope for 10.3.2:** DNN platform schema migration (wizard-handled) + **2sxc upgrade 15.02 → 21.07** + **12 custom-template audit** (`_FallacyExplorer_Root.cshtml` `RazorComponent` → `Custom.Hybrid.Razor14`, `_Parts.cshtml` `@helper` → `@functions`) + **data preservation** (all 26 apps' content rows, portal/page settings, users).
- ⚠️ **Still verify** post-upgrade that 2sxc 21.07 serves the migrated `Argumentum` app templates. This is a smoke-test item (Phase 5/6), not a separate migration.

The detailed 2sxc migration procedure lives in [131-2sxc-migration-plan.md](131-2sxc-migration.md);
this runbook sequences it as §5.5 and does not repeat its internals.

## 5. Phase-by-phase go-live procedure

Each phase has a **go/no-go gate**. Do not start the next phase until the current gate is green and
jsboige has signed off (execution is GATED throughout).

### Phase 0 — Pre-flight (repo-grounded, can be prepared now)

- [ ] Confirm rollback-readiness prerequisites: enough disk on prod for a full files+DB backup, and an
      off-host copy target.
- [ ] Inventory the deployable artifacts: the 602 MB git-tracked export (RAPPORTÉ issue #131) +
      `DNNPlatform/bin/` (DotNetNuke 9.11.1.19, OS_Stripe, Stripe.net).
- [ ] Confirm the sandbox test (Phase 4 below / [Step-1 runbook #522](131-step1-sandbox-upgrade-runbook.md))
      has **passed** — including the §5.5 2sxc upgrade + template audit — before touching prod.
- [ ] Snapshot the current prod versions (DNN 9.11.1.19, 2sxc 15.02, OpenStore) into the deploy log.

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
   `Argumentum` + `Glossary3`), portal/page settings, user counts. Confirm 2sxc is at 15.02 (the
   pre-upgrade baseline before §5.5).

> 🚧 **HONESTY BOUNDARY — DB/RDP-gated:** the actual prod schema, the 2sxc content-type/entity/metadata
> row counts, and the dev↔prod delta **cannot be produced from the repo** (the export is sanitized and
> the repo holds templates, not portal content — confirmed by [inventory #525]). These sections are
> **to be filled in against the real prod DB export**. The runbook provides the *procedure* and the
> *topology* (§3); the *measurements* are jsboige's, gated.

### Phase 3 — Migration plan (dev↔prod delta)  — *jsboige step 4*

- [ ] **Schema delta** — diff staging-restored-prod-schema vs the dev sandbox schema. Identify any
      DNN customizations (custom modules, extra columns) that the upgrade wizard must preserve.
- [ ] **Data-preservation rules** — which tables are user-content (preserve verbatim) vs
      framework-config (wizard-managed). Split: 2sxc content = preserve; DNN core tables = wizard-managed
      (no manual edit).
- [ ] **2sxc scope** — confirm §4: the 2sxc 15.02 → 21.07 upgrade + 12-template audit ARE in scope
      (§5.5), sequenced before the DNN 10.3.2 wizard.
- [ ] **Eshop scope** — confirm OpenStore + OS_Stripe survive the DNN 10.3.2 upgrade (OpenStore runs
      on 10.x, #593 — the earlier “.NET 8 breaks it” concern is retracted). Stripe Native #445 is
      re-opened but **not blocking** the platform upgrade (separate decision).

### Phase 4 — Sandbox test migration  — *jsboige step 5*

Run the [Step-1 sandbox runbook (#522)](131-step1-sandbox-upgrade-runbook.md) **plus §5.5** on the
staging copy (**not** the empty sandbox — on the prod-restored data). Validate:
- [ ] **§5.5.a 2sxc 15.02 → 21.07** completes, no fatal errors (cliff de-risk, done FIRST).
- [ ] **§5.5.b 12-template audit** — `_FallacyExplorer_Root.cshtml` → `Custom.Hybrid.Razor14`,
      `_Parts.cshtml` `@helper` → `@functions`; remaining 10 templates re-bind under 2sxc 21.
- [ ] Upgrade wizard completes 9.11.1.19 → 10.3.2, no fatal errors.
- [ ] Site boots, homepage renders, language switcher works (FR + en-US baseline).
- [ ] 2sxc 21.07 `Argumentum` app templates still bind.
- [ ] OpenStore + Stripe checkout path loads (no IIS crash from `DnnJsInclude`).
- [ ] **Go/no-go gate → jsboige.** Only on green do we proceed to Phase 5.

### §5.5 — The cliff crossing (2sxc upgrade + template audit)  — referenced procedure

This is the **net-new work** that 10.3.2 requires and 10.1.2 avoided. It is done on staging (Phase 4)
then repeated on prod (Phase 5). The internals are in [131-2sxc-migration-plan.md]:

- **(a) 2sxc 15.02 → 21.07 LTS** — install 2sxc 21 **before** the DNN upgrade crosses 10.2.0
  (else `DnnJsInclude` crashes IIS — the cliff). 2sxc content (`ToSic_EAV_*` + app tables) migrates
  with the 2sxc installer, not the DNN wizard.
- **(b) 12-template audit** — migrate the custom Razor templates that 2sxc 21 no longer tolerates:
  `_FallacyExplorer_Root.cshtml` (`@inherits ToSic.Sxc.Dnn.RazorComponent` → `Custom.Hybrid.Razor14`),
  `_Parts.cshtml` (`@helper` → `@functions`, the one non-trivial step), + 10 others re-bind. Issue
  #131 body lists the APIs that survive in 2sxc 21: `App.Query`, `AsList`, `CmsContext`, `Link.To`,
  `Edit.TagToolbar`. ~4-6h code-only (UPGRADE-ASSESSMENT §10).

### Phase 5 — Production go-live  — *jsboige step 6*

1. **Maintenance mode** — put the prod site in DNN maintenance/under-construction (visitors see the
   holding page; admin stays functional).
2. **Re-backup prod** (Phase 1 again) — captures any data written between the Phase-1 backup and now.
   This second backup is the **true rollback anchor**.
3. **Apply §5.5 on prod** — 2sxc 15.02 → 21.07 (a) then the 12-template audit (b), same path proven
   in Phase 4.
4. **Apply the DNN upgrade** — same wizard path proven in Phase 4, on prod, to 10.3.2.
5. **Smoke test on prod** — homepage, language switcher, 2sxc 21 Argumentum app, OpenStore/Stripe path.
6. **Exit maintenance mode** — only after the smoke test is green.
7. **Record** final versions + timestamps in the deploy log (DNN 10.3.2, 2sxc 21.07, OpenStore).

### Phase 6 — Rollback strategy (the contract that makes go-live reversible)

Rollback-readiness is **per phase**; if any phase fails, roll back to the last green anchor:

| Failed phase | Rollback action | Anchor |
|--------------|-----------------|--------|
| Phase 1 (backup fails) | **Do not proceed** — nothing changed yet | (none needed) |
| §5.5 on staging (2sxc/template fails) | Discard staging; prod untouched | prod (unchanged) |
| Phase 4 (sandbox test fails) | Discard staging; prod untouched | prod (unchanged) |
| Phase 5 §5.5 fails (2sxc/template on prod) | `RESTORE DATABASE` from Phase-5 re-backup + restore files backup | Phase-5 re-backup |
| Phase 5 wizard fails | Same `RESTORE` + files restore; keep maintenance mode | Phase-5 re-backup |
| Phase 5 smoke-test fails post-wizard | Same `RESTORE` + files restore; keep maintenance mode | Phase-5 re-backup |

**Rollback contract:** the Phase-5 re-backup must exist, be checksum-verified, and be tested by a
`RESTORE VERIFYONLY` **before** exiting maintenance mode. If rollback is not possible, do not exit
maintenance mode — escalate to jsboige.

## 6. Go/no-go checklist (jsboige ticks at go-live)

- [ ] Phase 0 pre-flight complete; sandbox test (Phase 4) **passed** — including §5.5.
- [ ] Phase 1 prod backup done, **two copies**, checksum-verified, `RESTORE VERIFYONLY` green.
- [ ] Phase 2 staging restore + schema analysis done (against **real** prod DB export).
- [ ] Phase 3 migration plan signed off (schema delta, data-preservation, 2sxc upgrade + template
      audit scoped per §5.5).
- [ ] Phase 4 sandbox test on prod-restored data **passed** (2sxc 21 app + OpenStore smoke green).
- [ ] Phase 5 re-backup taken; `RESTORE VERIFYONLY` green; rollback path confirmed.
- [ ] Maintenance window scheduled; jsboige **go** for go-live.

## 7. Honesty boundary — sections gated on the real prod DB export

These cannot be completed from the repo and are left as **procedure**, to be filled against the prod
export (the export in-repo is sanitized — `Data Source=REPLACE`, templates-only per #525):

- §3 real connection string / DB name / DB size / row counts (RAPPORTÉ, not VERIFIED).
- §5 Phase 2 actual prod schema + 2sxc content row counts.
- §5 Phase 3 actual dev↔prod schema delta.
- §5 Phase 4 / §5.5 sandbox-test outcome on prod-restored data.

The **procedure, rollback strategy, topology, and the cliff-crossing sequence (§4/§5.5)** are
repo/memory-grounded and hold on their own.

## 8. Gate boundaries

- ❌ Does **not** deploy, back up, restore, or upgrade anything on production (execution GATED jsboige).
- ❌ Does **not** touch the live DB / RDP / portal (all DB steps are described, not performed).
- ❌ Does **not** duplicate the sandbox procedure (#522), the plan (#511), or the 2sxc migration plan
      ([131-2sxc-migration-plan.md]) — it references them.
- ❌ Does **not** decide the eshop (Stripe Native #445) or the target — **target = 10.3.2 is actée
      (#458)**; the eshop is a separate jsboige gate.
- ❌ Does **not** declare a QA verdict (ai-01 only).

## 9. Decision history — why 10.1.2 was considered and rejected

The earlier draft recommended **10.1.2** because it is the first target closing **both** critical
CVEs (CVE-2025-64095 patched 10.1.1, CVE-2025-52488 patched 10.0.1) **and** sits **before** the 2sxc
compatibility cliff at 10.2.0 — i.e. it avoids the entire §5.5 cliff-crossing cost. That tradeoff
(“security floor, no 2sxc work”) was real. jsboige rejected it in decision **#458** in favour of
**10.3.2**, deliberately accepting the 2sxc upgrade + template audit to reach the latest version and
clear the cliff for good. 10.1.2 is documented here for history; **do not execute against 10.1.2.**

## Sources

- Repo: `DNNPlatform/web.config` (SQL Server `System.Data.SqlClient`, `targetFramework="4.8"`),
  `DNNPlatform/Portals/1/2sxc/` (26 apps), `DNNPlatform/bin/` (DotNetNuke, OS_Stripe, Stripe.net),
  `DNNPlatform/DesktopModules/NBright/OS_Stripe`.
- [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) ·
  [131-step0-runtime-verification.md](131-step0-runtime-verification.md) (#514) ·
  [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) ·
  [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) ·
  [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) (#522) ·
  [../dnn/UPGRADE-ASSESSMENT.md](../dnn/UPGRADE-ASSESSMENT.md) (#593) ·
  [457-site-content-type-inventory.md](457-site-content-type-inventory.md) (#525 — repo = templates,
  not portal content).
- Issue #132 body (jsboige 6-step migration strategy) · Issue #131 body (topology, eshop, i18n scope).
- Memory: DNN Phase A IIS Express boot (2026-06-02), VPS myia-web1, 2sxc cliff @10.2.0 (#6902).
