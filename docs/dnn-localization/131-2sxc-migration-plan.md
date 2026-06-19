# #131 — 2sxc Migration Plan: 15.02 → 21.07 LTS (the Phase 1.5 detail)

**Issue:** [#131 — DNN platform upgrade (sandbox → prod)](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-19
**Base:** master `909d04c3`
**Status:** **DOC / non-gated.** The detailed, step-by-step **2sxc upgrade procedure** that
[target-revision #531](131-target-revision-10.3.2-full-upgrade.md) §4 **Phase 1.5** names in one sentence but
does not expand. This doc is that expansion: the **sequence, the order, the rollback points, and the 25-stock-app
verification checklist**. Execution stays GATED — this is the plan; running it is jsboige's call.

> **Relationship to the arc** (anti-dup): the *why* (cliff #6902, target 10.3.2, templates already Razor14) lives in
> [#531](131-target-revision-10.3.2-full-upgrade.md) and is **not repeated here**. The v20 breaking-changes *inventory*
> lives in [#511 §6](131-upgrade-sandbox-plan-v2.md) and is **not repeated**. The post-upgrade *smoke-test gate* lives in
> [#533](131-step2-smoke-test-checklist.md) and is **referenced, not duplicated**. This doc adds the **how** that those
> docs only name: the concrete 2sxc upgrade sequence, the framework-DNN-puis-2sxc ordering rationale, per-step rollback
> anchors, and the named-25-app verification table.

---

## 1. The ordering decision — 2sxc-first, then DNN (why Phase 1.5 is a separate step)

The 10.3.2 upgrade crosses the 2sxc cliff at 10.2.0 ([#520 §3](131-cve-correction-and-target-refinement.md):
`DnnJsInclude` ClientDependency change → IIS worker-process crash). The decisive choice is **which to upgrade first**.

**Decision: upgrade 2sxc 15.02 → 21.07 LTS on the CURRENT DNN 9.11.1, before touching DNN.** Rationale:

| Order | What happens at the cliff (10.2.0+) | Verdict |
|-------|--------------------------------------|---------|
| **2sxc-first (chosen)** | 2sxc 21.07 carries the `DnnJsInclude` workaround (since 2sxc 21.00.02). Upgrade on DNN 9.11.1 is **safe** (2sxc 21 min DNN = 9.11.02, our 9.11.1 clears it). Then the DNN 9.11→10.3.2 jump crosses the cliff with a workaround-bearing 2sxc already in place → **no IIS crash**. | ✅ **isolates risk** (2sxc failure ≠ DNN failure, debuggable separately) |
| DNN-first (rejected) | DNN 9.11→10.3.2 crosses 10.2.0 with **2sxc 15.02 still installed** (no workaround) → **IIS crash on first 2sxc page hit**. The site is then half-upgraded (DNN 10.3.2, 2sxc 15.02 broken) — a recovery-required state. | ❌ **couples failures** |

**Implication:** Phase 1.5 (2sxc upgrade) is **not optional** on the 10.3.2 path and **must complete + verify before Phase 2
(the DNN upgrade)**. This is the single sequencing constraint the whole plan hinges on.

---

## 2. The 2sxc upgrade sequence (Phase 1.5, sandbox/staging only)

> ⚠️ **Honesty boundary:** the *generic* 2sxc upgrade mechanics below are the standard community flow (2sxc install
> package → DNN Extensions → restart → 2sxc migration wizard). **Confirm the exact steps against the official 2sxc
> upgrade guide** (`docs.2sxc.org` → upgrade) before execution — the same caveat #522 §4 carries for the DNN wizard.
> Everything in §1, §3, §4, §5 is repo/arc-grounded and stands on its own.

Each step has a **rollback anchor**. Do not start the next until the current verifies.

### Step 1.5.0 — Pre-flight (the safety net for every later step)
- [ ] **Full backup** (files + DB) — reuse the [#527](132-deployment-runbook.md) Phase 1 procedure (files tar+checksum,
      DB `BACKUP DATABASE … WITH CHECKSUM` + `RESTORE VERIFYONLY`). **Two copies, off-host.** This is the master
      rollback anchor for the whole 2sxc upgrade.
- [ ] **Export the 2sxc apps** (Argumentum + the 25 stock) via 2sxc admin → export, so app data is restorable even if the
      DB restore is partial.
- [ ] **Record the "before" state**: 2sxc version (15.02, MEMORY.md Phase A), the 26 installed apps (repo-grounded list
      in §4), DNN 9.11.1.19, `web.config` checksum.

### Step 1.5.1 — Download + stage the 2sxc 21.07 LTS install package
- [ ] Download the **2sxc 21.07 LTS** (or latest 21.x LTS) **install package** from the 2sxc release archive (NOT the
      upgrade-only delta if the 2sxc installer offers both — verify which the DNN Extensions installer expects for a
      15→21 jump).
- [ ] **Verify** the package version clears the cliff workaround floor (≥ 21.00.02 per #531 §2.2).
- [ ] Stage the package on the sandbox; **do not install yet**.

### Step 1.5.2 — Stop IIS, install 2sxc 21 via DNN Extensions
- [ ] **Stop the IIS site** (sandbox: stop IIS Express :8090 / staging: stop the app pool) — frees file locks.
- [ ] DNN **Host → Extensions → Install Extension** → upload the 2sxc 21 package → run the installer.
- [ ] Watch the install log for the 2sxc schema-migration step (the installer restructures the `ToSic_EAV_*` + app
      tables — the v20 SQL reorg from #511 §6). **No fatal errors expected**; flag any skipped/reported extension.

### Step 1.5.3 — Run the 2sxc migration wizard, restart
- [ ] Browse to the site → 2sxc prompts its **migration/upgrade wizard** (content-types/entities/metadata — the "real
      2sxc content migration" #531 §4 Phase 3 flags). Run it to completion.
- [ ] **Restart IIS** (full app-pool recycle, not just the site) — 2sxc caches must clear.
- [ ] **Go/no-go gate A → jsboige**: wizard completes, no fatal. If fatal → **rollback to Step 1.5.0 backup** (anchor).

### Step 1.5.4 — Verify on DNN 9.11.1 (the isolation check)
The point of doing this *before* the DNN upgrade: confirm 2sxc 21 runs **on the current DNN**, so a later failure can
only be DNN-side, not 2sxc-side.
- [ ] Site boots on DNN 9.11.1 + 2sxc 21.
- [ ] **Argumentum app** still serves (4 templates — already Razor14 per #531 §3, so they should bind unchanged).
- [ ] 2sxc admin → System Info reports 21.x.
- [ ] **Go/no-go gate B → jsboige**: 2sxc 21 healthy on DNN 9.11.1. Only then proceed to Phase 2 (DNN 9.11→10.3.2).

---

## 3. Rollback points (2sxc-specific, complements #527 §6)

#527 §6 has the per-phase rollback table for the **DNN** phases. The 2sxc upgrade adds its own anchors:

| Failed step | Rollback action | Anchor |
|-------------|-----------------|--------|
| 1.5.2 (2sxc install fails) | `RESTORE DATABASE` + files from Step 1.5.0 | Step 1.5.0 backup |
| 1.5.3 (2sxc migration wizard fails) | Same `RESTORE` + files; **or** re-import the 2sxc app exports from 1.5.0 if only app-data is corrupt | Step 1.5.0 backup + app exports |
| 1.5.4 (2sxc 21 unhealthy on DNN 9.11.1) | Same `RESTORE` (back to 2sxc 15.02 + DNN 9.11.1) — **do NOT proceed to DNN upgrade** | Step 1.5.0 backup |
| Phase 2 DNN wizard fails (after 1.5 green) | `RESTORE` to Step 1.5.0 (resets both 2sxc and DNN) — the 2sxc-first order means a DNN failure never leaves 2sxc in a half-state | Step 1.5.0 backup |

**Rollback contract:** the Step 1.5.0 backup must exist, be checksum-verified, and pass `RESTORE VERIFYONLY` **before**
Step 1.5.2. If rollback is not possible, **do not start the 2sxc upgrade** — escalate to jsboige.

---

## 4. The 26 installed 2sxc apps — verification checklist (repo-grounded)

The repo `DNNPlatform/Portals/1/2sxc/` holds **26 apps** (verified directory count this date). The custom
**Argumentum** app is 1 of 26 (pre-cleared, #531 §3). The other **25 are stock 2sxc apps** that face the v20 infra
breaking changes (#511 §6: `SexyContentWebPage` deprecation, module-path rename, SQL reorg). They live in the portal
DB/runtime, **not the repo** ([#525](457-site-content-type-inventory.md)) → they are a **staging-test** item, run after
Step 1.5.4 and again after Phase 2.

The 25 stock apps (actual repo listing, supersedes the partial list in #511 §5):

`Accordion4` · `AddSearch3` · `Blog5` · `CTA3` · `Content` · `Counter2` · `EventsAndCourses6` · `Faq4` · `Gallery7` ·
`Glossary3` · `IFrame3` · `ImageCompare2` · `ImageHotspots3` · `Jobs2` · `MobiusForms5` · `News5` · `PeopleDirectory4` ·
`PodCast2` · `PopupMessage3` · `QrCode2` · `SnippetInject3` · `Swiper2` · `Timeline3` · `TimelineJs2` · `disqus3`

Post-2sxc-upgrade verification (spot-check ≥ the dispatch-named 4 + Content, then sweep):
- [ ] **Content** app views render (the ASSESSMENT's known 12-template legacy RazorComponent surface — highest legacy risk).
- [ ] **Glossary3** renders (installed = 14.09.00, pre-typed-mode — #511 §5 flags it for a typed-v3 bump).
- [ ] **Blog5**, **News5** render (current-generation; verify no old `$2sxc.min.js` / `Thumbnailer.aspx` path refs).
- [ ] **MobiusForms5** renders (forms = live data path, not just display).
- [ ] Remaining 20 apps spot-load (admin → each app default view renders, no white-screen).
- [ ] No `SexyContentWebPage`-deprecation error on any stock-app view.
- [ ] Asset links resolve on the **new** module path `/DesktopModules/ToSic.Sxc/` (old `ToSic_SexyContent` → 404 for
      directly-linked assets).

> The full 8-language smoke (homepage, language switcher, commerce stack) is the **[#533](131-step2-smoke-test-checklist.md)
> gate**, run after Phase 2. This §4 checklist is the **2sxc-apps-specific** subset, run after Step 1.5.4.

---

## 5. The full 10.3.2 sequence (how Phase 1.5 slots in)

Consolidated view — Phase 1.5 (this doc) is the new step the 10.3.2 target inserts into the [#527](132-deployment-runbook.md)
6-phase go-live:

```
Phase 0   pre-flight (#527)
Phase 1   prod backup files+DB (#527)               ← the master anchor
Phase 1.5 2sxc 15.02 → 21.07 LTS on DNN 9.11.1      ← THIS DOC (§2), gates A+B
Phase 2   staging restore + schema analysis (#527)
Phase 3   migration plan incl. 2sxc content-types (#531 §4 expanded)
Phase 4   sandbox test on prod-restored data (#527) + cliff smoke (#533 §3/§4) + 25-app verify (§4 here)
Phase 5   prod go-live (#527)
Phase 6   rollback strategy (#527 §6 + §3 here)
```

**Hard gate:** Phase 1.5 must be **green (gates A+B)** before Phase 2. Skipping it = the DNN-first rejected order (§1).

---

## 6. Gate boundaries (this document)

- ✅ Expands the one-sentence Phase 1.5 from #531 §4 into a step-by-step plan with rollback anchors.
- ✅ Grounds the 25-stock-app list from the actual repo (26 apps, supersedes #511 §5's partial list).
- ✅ Documents the 2sxc-first ordering rationale and the 2sxc-specific rollback table.
- ❌ Does **not** execute anything — 2sxc upgrade, migration wizard, DB restore are all GATED jsboige.
- ❌ Does **not** duplicate the cliff rationale (#520/#531), the v20 breaking-changes inventory (#511 §6), or the
      smoke-test gate (#533) — references them.
- ❌ Does **not** decide #445 (Stripe) — flags the 10.3.2 window as the natural moment (#531 §4).
- ❌ Does **not** declare a QA verdict (ai-01 + jsboige only).

## Sources

- [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) (#531) — §2.2 2sxc upgrade path, §4 Phase 1.5 (the one-sentence this doc expands), §3 templates already Razor14.
- [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) — §3 the cliff at 10.2.0 (issue #6902, `DnnJsInclude` win32 crash).
- [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) — §5 2sxc app matrix (partial list), §6 v20 breaking-changes inventory.
- [131-step2-smoke-test-checklist.md](131-step2-smoke-test-checklist.md) (#533) — §3 cliff smoke (DnnJsInclude), §4 stock-apps smoke.
- [132-deployment-runbook.md](132-deployment-runbook.md) (#527) — §6 per-phase DNN rollback table; Phase 1 backup = the master anchor reused in §2 Step 1.5.0.
- [457-site-content-type-inventory.md](457-site-content-type-inventory.md) (#525) — stock apps live in portal DB/runtime, not repo (why §4 is a staging-test item).
- Repo: `DNNPlatform/Portals/1/2sxc/` (26 apps, verified directory count 2026-06-19), `DNNPlatform/Portals/1/2sxc/Argumentum/*.cshtml` (4 templates, all Razor14).

---

*Worker migration plan (doc/non-gated). The 2sxc upgrade execution is GATED jsboige; the visual PASS verdict is ai-01 +
jsboige. No production system touched.*
