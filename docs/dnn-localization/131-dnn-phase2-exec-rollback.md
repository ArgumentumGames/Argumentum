# #131 — DNN Phase 2 Execution + Rollback Runbook (9.11.1 → 10.3.2, post-2sxc-21)

**Issue:** [#131 — DNN platform upgrade (sandbox → prod)](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-19
**Base:** master `4dd3c6bd`
**Status:** **DOC / non-gated.** The **DNN platform upgrade execution sequence** (Phase 2 of the 10.3.2 path) **and its
rollback runbook**, expanding the one-line "Phase 2 — staging restore + schema analysis (#527)" that
[#531 §4](131-target-revision-10.3.2-full-upgrade.md) names but does not detail for the post-cliff path. This is the
**DNN-side** counterpart to the [2sxc migration plan #543](131-2sxc-migration-plan.md) (Phase 1.5). Execution stays
GATED — this is the plan; running it is jsboige's call.

> **Relationship to the arc** (anti-dup): the *why* (cliff #6902, target 10.3.2, templates already Razor14) lives in
> [#531](131-target-revision-10.3.2-full-upgrade.md). The **2sxc upgrade sequence** (Phase 1.5, must complete first)
> lives in [#543](131-2sxc-migration-plan.md). The **generic DNN upgrade mechanics** + the **sandbox go/no-go per stage**
> live in [#522 §4-§6](131-step1-sandbox-upgrade-runbook.md) (target 10.1.2-superseded, but mechanics reusable). The
> **6-phase go-live + the DNN-phase rollback table** live in [#527 §5-§6](132-deployment-runbook.md). **This doc adds the
> how for the DNN execution after Phase 1.5 is green + a 10.3.2-specific rollback runbook** that the others only name.

---

## 1. Preconditions — Phase 1.5 (2sxc) must be GREEN first

The whole 10.3.2 path hinges on [#543 §1](131-2sxc-migration-plan.md): **2sxc 21.07 LTS is installed and verified on
DNN 9.11.1 BEFORE the DNN upgrade crosses 10.2.0.** Do not start Phase 2 (this doc) unless:

- [ ] [Phase 1.5](131-2sxc-migration-plan.md) **gate B is green** — 2sxc 21 healthy on DNN 9.11.1.
- [ ] The **Step 1.5.0 master backup** exists, is checksum-verified, and passes `RESTORE VERIFYONLY` (the rollback
      anchor reused in §4 below).
- [ ] The site is currently **DNN 9.11.1.19 + 2sxc 21.x** (the only safe pre-crossing state).

If Phase 1.5 is not green, **stop** — proceeding would cross the cliff with a workaround-less 2sxc (the rejected
DNN-first order, #543 §1).

---

## 2. The DNN execution sequence (Phase 2 — the platform upgrade)

> ⚠️ **Honesty boundary:** the *generic* DNN upgrade-wizard mechanics below are the standard community-documented flow
> and are reused from [#522 §4](131-step1-sandbox-upgrade-runbook.md). The **authoritative steps must be confirmed
> against the official DNN upgrade guide** (`docs.dnncommunity.org` → Upgrading) before execution — the same caveat #522
> §4 carries. Everything about *ordering, validation gates, and rollback* in §3/§4 here is arc-grounded and stands alone.

### §2.1 Single-jump or stepping-stone? (the path question)

[#531 §2.1](131-target-revision-10.3.2-full-upgrade.md) confirms the DNN upgrade wizard handles the schema migration
**9.11.1 → 10.3.2 directly** — the cliff is a 2sxc problem (now solved by Phase 1.5), **not** a DNN-schema problem.

| Path | What | Verdict |
|------|------|---------|
| **Direct jump (chosen)** | Run the 10.3.2 Upgrade package wizard over 9.11.1. The wizard migrates schema 9.11→10.3 in one pass. | ✅ the documented path (#531 §2.1) |
| Stepping-stone via 9.13.9 | Upgrade 9.11→9.13.9 first, then 9.13.9→10.3.2. | ⚠️ "#531 §2.1: migration aid only" — optional, adds a step + a backup cycle for no CVE gain (9.13.x closes 0 CVEs) |

**Decision: direct jump 9.11.1 → 10.3.2**, because the wizard supports it and the cliff is already neutralized by
Phase 1.5. Reserve the stepping-stone only if the direct wizard fails on this specific DB (fallback, §4).

### §2.2 Execution steps (sandbox/staging first, then prod)

Each step has a **validation gate**. Do not start the next until the current is green.

#### Step 2.0 — Pre-execution snapshot (the Phase-2 rollback anchor)
- [ ] **Re-snapshot** the current state (DNN 9.11.1 + 2sxc 21) — files tar+checksum + `BACKUP DATABASE … WITH CHECKSUM` +
      `RESTORE VERIFYONLY`. **Two copies, off-host.** This is distinct from the Step 1.5.0 backup: it captures the
      **post-2sxc-upgrade, pre-DNN-upgrade** state, which is the cleanest rollback target if the DNN wizard fails.
- [ ] Record: DNN 9.11.1.19, 2sxc 21.x, `DotNetNuke.dll` FileVersion, `web.config` checksum.

#### Step 2.1 — Download + stage the 10.3.2 Upgrade package
- [ ] Download the **Upgrade package** for 10.3.2 from the DNN community release archive — **not** the Install package
      (Install is greenfield; Upgrade preserves the DB, #522 §4 step 2).
- [ ] **Verify** the package version is 10.3.2 (not 10.1.x — the superseded target).
- [ ] Stage on the sandbox/staging host; **do not apply yet**.

#### Step 2.2 — Stop IIS, apply the Upgrade package files
- [ ] **Stop the IIS site** (sandbox: stop IIS Express :8090 / staging: stop the app pool) — frees file locks (#522 §4 step 3).
- [ ] Copy the Upgrade package's files/bin over the site root — **preserves** `web.config`, `App_Data`, `Portals`,
      `DesktopModules`, `bin\ToSic.*` (2sxc 21 binaries stay in place, #522 §4 step 4).
- [ ] **Do NOT run the wizard yet** — files only at this step.

#### Step 2.3 — Run the upgrade wizard (schema migration)
- [ ] Browse to `http://localhost:8090/Install/Upgrade.aspx` (sandbox) — the wizard auto-detects the version gap and
      runs the **schema + data migration** 9.11.1 → 10.3.2 (#522 §4 step 5).
- [ ] Watch the upgrade log: schema migration + any **skipped/reported extension**. **No fatal expected**; flag any.
- [ ] **Validation gate C** (below, §3.1): wizard completes, log clean, site boots. If fatal → **rollback to Step 2.0
      snapshot** (§4).

#### Step 2.4 — Restart, full smoke
- [ ] **Restart IIS** (full app-pool recycle) — DNN caches must clear post-schema-migration.
- [ ] Run the **[#533](131-step2-smoke-test-checklist.md) consolidated smoke gate** (§3 cliff check, §4 stock apps,
      §5 Argumentum, §6 8-language surface, §7 commerce).
- [ ] **Validation gate D** (§3.2): smoke green. If red → rollback to Step 2.0 snapshot (§4).

---

## 3. Intermediate validation gates (the go/no-go checkpoints)

These are the **between-steps** checkpoints the dispatch asks for. Each is a hard gate.

### §3.1 Gate C — wizard completion (post-Step 2.3)
The point: catch a failed schema migration **before** investing in the full smoke.
- [ ] Upgrade-wizard log: **no `FATAL`**, no skipped-extensions errors.
- [ ] `DotNetNuke.dll` FileVersion = **10.3.2.x** (the wizard applied the new binaries).
- [ ] Site **boots to the homepage** (no yellow-screen / no 500) — a boot failure here means the schema migration left
      the DB inconsistent → rollback.
- [ ] 2sxc admin → System Info still reports **21.x** (the DNN upgrade did not regress 2sxc).

### §3.2 Gate D — smoke green (post-Step 2.4)
The [#533](131-step2-smoke-test-checklist.md) gate, in full. The two 10.3.2-mandatory families:
- [ ] **§3 cliff check** — no `DnnJsInclude` IIS worker-process crash (the whole reason Phase 1.5 ran first; this confirms
      the workaround held across the DNN upgrade).
- [ ] **§4 stock apps** — the 25 stock 2sxc apps load (v20 infra: `SexyContentWebPage` deprecation, module-path rename).
- [ ] **§5 Argumentum** — 4 custom templates still bind (pre-cleared Razor14, #531 §3).
- [ ] **§6 platform** — 8-language switcher, Admin/PersonaBar.
- [ ] **§7 commerce** — OpenStore + Stripe checkout path (the #445 decision window).

**Gate D green → eligible for [#527](132-deployment-runbook.md) Phase 5 (prod go-live).** Gate D red → rollback (§4).

---

## 4. Rollback runbook (10.3.2-specific)

This complements [#527 §6](132-deployment-runbook.md) (DNN-phase rollback table, 10.1.2-oriented) and
[#543 §3](131-2sxc-migration-plan.md) (2sxc-specific rollback). The 10.3.2 path adds its own anchors:

### §4.1 The rollback anchors (two distinct snapshots)

| Anchor | Captured at | Contains | Use when |
|--------|-------------|----------|----------|
| **Step 1.5.0 backup** | before the 2sxc upgrade | DNN 9.11.1 + 2sxc **15.02** | full reset to the original pre-anything state |
| **Step 2.0 snapshot** (this doc) | after Phase 1.5 green, **before** the DNN upgrade | DNN 9.11.1 + 2sxc **21** | DNN wizard failed but 2sxc 21 was healthy — restores the 2sxc work without redoing Phase 1.5 |

**Choose the right anchor:** if the DNN wizard fails, prefer the **Step 2.0 snapshot** (preserves the verified 2sxc 21
upgrade). Use the **Step 1.5.0 backup** only if you want to reset everything (including redoing Phase 1.5).

### §4.2 Rollback procedure (DB + webroot)

```
1. STOP the IIS site (free all file locks).
2. DB RESTORE:
     ALTER DATABASE [DotNetNuke] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
     RESTORE DATABASE [DotNetNuke]
       FROM DISK = '…\<anchor>.bak'
       WITH REPLACE, RECOVERY, CHECKSUM;
     -- VERIFYONLY first, always (the rollback contract, #527 §6):
     RESTORE VERIFYONLY FROM DISK = '…\<anchor>.bak' WITH CHECKSUM;
3. WEBROOT RESTORE:
     - delete the upgraded webroot's bin/ + the wizard-touched files
     - restore bin/, web.config, Portals/, DesktopModules/, App_Data/ from the anchor's files backup
     - verify file count + checksums match the anchor manifest
4. START the IIS site; confirm it boots to DNN <pre-upgrade version> + 2sxc <anchor's 2sxc version>.
5. Record the rollback in the deploy log (timestamp, anchor used, reason).
```

> ⚠️ **Honesty boundary:** the exact DB name / connection string is **prod-only** (sanitized in repo, #527 §3). The SQL
> shape above is the standard SQL Server restore; confirm against the real prod instance at execution.

### §4.3 Go/no-go criteria for rollback

Roll back immediately if **any** of:
- Gate C red (wizard fatal / boot failure post-schema-migration).
- Gate D §3 red — a `DnnJsInclude` win32 crash appears (means the 2sxc workaround did not survive the DNN upgrade —
  serious; rollback + re-verify Phase 1.5).
- Gate D §7 red AND #445 is undecided (commerce failure blocks prod, #533 §9).
- The rollback path itself is **not verified** (`RESTORE VERIFYONLY` on the anchor must be green **before** starting
  Step 2.2 — the rollback contract).

**Do NOT roll back silently:** every rollback is a deploy-log event + an ai-01 + jsboige notification (the worker signals,
does not decide, #533 §1).

### §4.4 The "wizard failed, what now" branch

If the direct 9.11→10.3.2 wizard fails (gate C red) on this specific DB:
1. Roll back to the **Step 2.0 snapshot** (DNN 9.11.1 + 2sxc 21).
2. **Try the stepping-stone** ([#531 §2.1](131-target-revision-10.3.2-full-upgrade.md) "migration aid"): upgrade
   9.11→9.13.9 first, then 9.13.9→10.3.2. Two wizard passes, two backup cycles.
3. If the stepping-stone also fails → **escalate to jsboige**; the DB may have a customization the wizard cannot migrate.
   This is the §7 honesty-boundary item (real prod schema is prod-only).

---

## 5. Where this slots in (the 10.3.2 phase map, updated)

Consolidated view — Phase 2 (this doc) is the DNN execution after Phase 1.5 (2sxc) is green:

```
Phase 0   pre-flight (#527)
Phase 1   prod backup files+DB (#527)               ← the master anchor
Phase 1.5 2sxc 15.02 → 21.07 LTS on DNN 9.11.1      ← #543 (gates A+B)
Phase 2   DNN 9.11.1 → 10.3.2 + rollback            ← THIS DOC (§2 exec, §3 gates C+D, §4 rollback)
Phase 3   migration plan incl. 2sxc content-types (#531 §4 expanded)
Phase 4   sandbox test on prod-restored data (#527) + cliff smoke (#533 §3/§4) + 25-app verify
Phase 5   prod go-live (#527)
Phase 6   rollback strategy (#527 §6 + #543 §3 + §4 here)
```

**Hard gate:** Phase 2 (this doc) requires Phase 1.5 green. Phase 5 (prod go-live) requires Phase 2 gate D green.

---

## 6. Gate boundaries (this document)

- ✅ Details the DNN execution sequence (single-jump 9.11→10.3.2, post-2sxc-21) with 4 steps + 2 validation gates.
- ✅ Adds a **10.3.2-specific rollback runbook** (two-anchor model, DB+webroot restore procedure, go/no-go criteria,
  stepping-stone fallback) — complements #527 §6 (generic DNN) and #543 §3 (2sxc).
- ✅ Documents the **intermediate validation gates** (C: wizard completion, D: smoke green) the dispatch asks for.
- ❌ Does **not** execute anything — DNN upgrade, wizard, DB restore are all GATED jsboige.
- ❌ Does **not** duplicate the 2sxc upgrade sequence (#543), the generic wizard mechanics (#522 §4), the cliff rationale
      (#531/#520), the v20 breaking-changes inventory (#511 §6), or the smoke-test gate (#533) — references them.
- ❌ Does **not** decide #445 (Stripe) — flags §3.2 Gate D §7 as the window.
- ❌ Does **not** declare a QA verdict (ai-01 + jsboige only).

## Sources

- [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) (#531) — §2.1 single-jump path (wizard handles 9.11→10.3.2 directly), §2.2 2sxc upgrade path, §3 templates already Razor14, §4 Phase 2 expansion.
- [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) (#543) — Phase 1.5 (2sxc, must be green first), §1 2sxc-first ordering, §3 2sxc-specific rollback (complemented by §4 here).
- [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) (#522) — §4 generic wizard mechanics (reused), §5 risk register, §6 go/no-go per stage.
- [131-step2-smoke-test-checklist.md](131-step2-smoke-test-checklist.md) (#533) — §3 cliff check, §4 stock apps, §5 Argumentum, §6 8-language, §7 commerce — the Gate D smoke.
- [132-deployment-runbook.md](132-deployment-runbook.md) (#527) — §5 6-phase go-live, §6 DNN-phase rollback table (complemented by §4 here), Phase 1 backup = master anchor.
- [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) — §3 the cliff at 10.2.0 (issue #6902).
- ai-01 dispatch `msg-20260619T110913` — Phase 2 exec + rollback doc.

---

*Worker Phase 2 execution + rollback runbook (doc/non-gated). The DNN upgrade execution is GATED jsboige; the visual
PASS verdict is ai-01 + jsboige. No production system touched.*
