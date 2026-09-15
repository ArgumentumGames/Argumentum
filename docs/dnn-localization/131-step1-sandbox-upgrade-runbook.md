# #131 — Step 1 Sandbox Upgrade Runbook (DNN 9.11.1.19 → 10.1.2)

**Issue:** [#131 — DNN platform upgrade (sandbox → prod)](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17
**Base:** master `87a02c19`
**Status:** **SANDBOX-PREP (research/runbook)** — execution GATED on jsboige's target decision (10.1.2 vs 10.3.2, see #520). **No prod deploy, no live sandbox mutation** until jsboige says go.

> ## ✅ POST-EXECUTION REALITY (refreshed 2026-07-12) — this prep doc's purpose was fulfilled
>
> The sandbox upgrade this runbook **prepared for** has been **executed and delivered**: DNN **10.3.2** + 2sxc
> **21.07** are live (full-IIS, `dnn.argumentum.myia.io`, HTTP 200/~85 KB, HTTPS SAN exp 2026-09-27). The
> 4-step staircase (9.11.1 → 9.13.10 → 10.2.0 → **10.3.2**) was completed on IIS Express :8090 (Phase B,
> 2026-06-28, jsboige GO), wizard automated via `wiz_runupgrade.ps1`, `dbo.[Version]` top row = 10.3.2.
> ACME bypass is **active in live** (`.well-known/acme-challenge/web.config`, renew win-acme 2026-08-23).
>
> **This doc is now a HISTORICAL prep record.** Read it for the staircase structure, pre-flight §3, and
> risk-register §5 reasoning. The **current source of truth** for state is:
> - [`131-dnn-phase2-exec-rollback.md`](131-dnn-phase2-exec-rollback.md) — the actual Phase B execution + rollback.
> - `MEMORY.md` §DNN — live state (runtime branch `dnn/sandbox-runtime-1032`, bin/ 330 files, B2.5 smoke GREEN).
> - [`../dnn/UPGRADE-ASSESSMENT.md`](../dnn/UPGRADE-ASSESSMENT.md) (#593) — corrected CVE + .NET 4.8 floor.
>
> **Two staleness items in the body below, now resolved** (do not act on them as written):
> - **§2 "2sxc 15.02"** — was STALE memory; 2sxc was **already 21.07.00** at Phase A boot. The 15.02 figure was
>   a pre-export inference, corrected by the live manifest (`13-app60-resources.json`).
> - **§5 "2sxc stays 15.02 → templates untouched" (10.1.2 path)** — moot; target is 10.3.2, the cliff was crossed,
>   and the 12-template audit + 2sxc-21 **BCL-stack alignment** were real work (see `132-deployment` §5.5 + the
>   B1-inversion lesson: 2sxc-21 REQUIRES .NET 8/9 BCL Json 9.0.0.0/SCI 9.0.0.0/Bcl 8.0.0.0 — NOT the 6.0.0.0
>   reversion the early `repair-bin-net48.ps1` encoded; that thesis was inverted).
>
> **Remaining gated work** = prod go-live on the VPS (jsboige ops task), described in
> [`132-deployment-runbook.md`](132-deployment-runbook.md).

> ⚠️ **TARGET SUPERSEDED — jsboige decided 10.3.2 (decision #2, issue #458, 2026-06-18).** The 10.1.2
> target below is superseded; read
> [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) as the source of
> truth. This sandbox procedure is **reusable for the 10.3.2 path** (the 4-step staircase + pre-flight +
> risk-register structure hold); the delta is the 2sxc upgrade + template audit the revision doc §4 adds.

**Depends on (all merged):**
- [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) — the 4-step staircase + 2sxc compat matrix.
- [131-step0-runtime-verification.md](131-step0-runtime-verification.md) (#514) — Step 0 result: DNN 10.3.2 runtime **VERIFIED .NET Framework 4.8** (no .NET 8 jump; OpenStore blocker dissolved).
- [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) — CVE map correction (9.13.x closes **0** CVEs; target refined to **10.1.2**).

---

## 1. What Step 1 is (scope)

Step 1 of the §7 staircase = the **security upgrade jump**: lift the running sandbox from the installed **DNN 9.11.1.19** to the CVE-closure target, **on the sandbox only**, documenting a per-stage go/no-go.

**The target is NOT yet chosen** (jsboige's gate, #520 §3):

| Option | CVEs closed | 2sxc work | Risk |
|--------|-------------|-----------|------|
| **10.1.2 (recommended)** | both (CVE-2025-64095 patched **10.1.1**; CVE-2025-52488 patched **10.0.1**) | none (before the 2sxc cliff at **10.2.0+**, issue #6902) | lowest |
| 10.3.2 (latest) | both | upgrade 2sxc 15.02 → ≥21 + template audit | higher (Argumentum's 4 bespoke `.cshtml`) |

This runbook covers the **10.1.2** path (lowest disruption, closes both CVEs). A 10.3.2 execution prepends a **2sxc upgrade sub-step** before the DNN jump — out of scope here until jsboige picks it.

**Authoritative version ladder (NuGet `DotNetNuke.Core`, verified 2026-06-17):** the 9.13.x line tops out at **9.13.9** (closes **0** CVEs — ruled out as a security palier). The 10.x line: 10.0.0, 10.0.1, 10.1.0, 10.1.1, 10.1.2, 10.2.0 … 10.3.2. The CVE-closure minimum is **10.0.1** (NTLM) → **10.1.1** (file-upload). **10.1.2** is the first release that has *both* and precedes the 2sxc cliff.

---

## 2. Sandbox baseline (repo-grounded)

| Fact | Value | Source |
|------|-------|--------|
| Installed DNN | **9.11.1.19** | `DNNPlatform/bin/DotNetNuke.dll` FileVersion |
| Runtime | .NET Framework **4.8** | `DNNPlatform/web.config` `targetFramework="4.8"` |
| 2sxc (Argumentum app host) | **15.02** | MEMORY.md (Phase A boot, 2026-06-02) |
| Argumentum custom 2sxc app | 4 bespoke `.cshtml` | `_Album List.cshtml`, `_FallacyExplorer_Root.cshtml`, `_RulesExplorer_RuleDetail.cshtml`, `_RulesExplorer_RuleList.cshtml` (`DNNPlatform/Portals/1/2sxc/Argumentum/`) |
| Glossary3 app | version history → **14.09.00** | `…/Glossary3/App_Data/app.xml` |
| Sandbox host | IIS Express, port **8090** | MEMORY.md Phase A (config outside repo: `.vs` gitignored) |

**Step 0 carried-over fact (the gate this depends on):** installed 9.11.1 (net48) and target 10.1.2 (net48) are the **same runtime** → no .NET jump, no module recompile for runtime reasons. (Module *API* surface is a separate question — see §5.)

---

## 3. Pre-flight (BEFORE any sandbox mutation) — do these now, gate-safe

These are non-destructive and *should* be done regardless of target choice:

1. **Full sandbox backup** (the safety net for every later step):
   - File-system copy of `DNNPlatform/` (the whole site root).
   - Database backup (`BACKUP DATABASE` on the sandbox DB).
   - Export the 2sxc apps (Argumentum + Glossary3) via 2sxc export, so a rollback restores app data even if the DB restore is partial.
2. **Capture the "before" golden state:**
   - Screenshot / note the loaded Argumentum pages (FallacyExplorer, RulesExplorer, Album List) on 8090 — the *visual* regression baseline.
   - Record `web.config` checksum + `DotNetNuke.dll` version.
3. **Inventory installed modules/extensions** (Host → Extensions) so a post-upgrade diff shows exactly what changed.
4. **Confirm the .NET 4.8 runtime** is what the Windows host serves (matches Step 0). No action if already 4.8.

---

## 4. The upgrade execution (sandbox only) — generic DNN mechanics

> ⚠️ **Honesty boundary:** the *generic* DNN upgrade-wizard mechanics below are the standard community-documented flow, but the **authoritative steps must be confirmed against the official DNN upgrade guide** (`docs.dnncommunity.org` → Upgrading) before execution. The web doc fetch was unavailable while writing this runbook (tooling outage); do not treat the bullet list below as verified procedure — confirm, then execute. Everything in §3 and §5 is repo-grounded and stands on its own.

Standard community flow (to confirm vs. official guide):
1. Take the §3 backups.
2. Download the **Upgrade package** for the target version (10.1.2) from the DNN community release archive — *not* the Install package (Install is for greenfield; Upgrade preserves the DB).
3. Stop the IIS Express sandbox site (free file locks).
4. Copy the Upgrade package's files/bin over the sandbox site root (preserves `web.config`, `App_Data`, `Portals`, `DesktopModules`).
5. Browse to `http://localhost:8090/Install/Upgrade.aspx` to run the DB-migration wizard.
6. Watch the upgrade log; the wizard reports schema/data migration + any skipped extensions.
7. Restart the site; smoke-test the §2 pages.

---

## 5. Argumentum-specific risk register (repo-grounded)

These are the places a 9.11→10.1 jump could bite *this* site specifically:

| Risk | Evidence | Mitigation |
|------|----------|------------|
| **Argumentum bespoke `.cshtml` (4)** use 2sxc v15-era APIs. | v20 "Moment-of-Truth" renamed module path (`/DesktopModules/ToSic_SexyContent/` → `…/ToSic.Sxc/`) + removed `SexyContentWebPage`/`GetBestValue`. | **Audit the 4 templates** against the v15→v20 changelog. But: target 10.1.2 does **not** force a 2sxc bump (we stay ≤ 10.1.x, before the cliff) → 2sxc stays 15.02 → templates likely untouched. This risk activates **only if target = 10.3.2** (forces 2sxc ≥21). |
| **Glossary3 at 14.09.00** (v14-era) | `app.xml` version history. | Same logic: no 2sxc bump on the 10.1.2 path → no action. Bump to typed v3 is a separate, later hardening. |
| **News5 / Blog5** (current-generation apps) | repo structure (`bs3/4/5`, `DnnSearch`, `api`). | LOW. Re-verify they load post-upgrade (smoke-test). |
| **DNN 9.12/9.13/10.0/10.1 breaking changes** (CDF control constructors, #6871 in Dnn.Platform releases) | GitHub release notes. | Confirm via the 9.11→10.1 release-note diff; the empty-ctor fix (#6871) suggests DNN *re-added* compatibility — favorable signal. |
| **Social-auth connectors** (PR #506, open) | existing branch. | Decide #506 fate *as part of* this upgrade or keep separate — don't bundle. |

---

## 6. Go/No-go per stage

| Stage | Pass criterion | Gate |
|-------|----------------|------|
| Pre-flight (§3) | backups exist; before-state captured | **GO** to proceed to execution |
| Upgrade execution (§4) | Upgrade wizard completes on sandbox; log clean | worker documents; **ai-01 + jsboige** review log |
| Post-upgrade smoke | the 4 Argumentum pages render correctly on 8090; modules load | **visual verdict = ai-01** (worker signals, does not declare PASS) |
| Rollback readiness | a tested path to restore 9.11.1.19 from §3 backup | **mandatory before** declaring Step 1 done |

**Hard gate:** Step 1 stays **sandbox-only**. No prod deploy without explicit jsboige go (Step 4 of the v2 plan). The CVE-closure target itself (10.1.2 vs 10.3.2) is jsboige's call (#520).

---

## 7. What this runbook does NOT do

- ❌ Does **not** execute the upgrade — execution is gated on jsboige's target decision.
- ❌ Does **not** verify the generic DNN upgrade-wizard steps against the official guide (tooling outage this session — flagged in §4 for confirmation).
- ❌ Does **not** touch prod, the CSV taxonomy, `dnn-ui-strings.csv`, or any consumer.
- ❌ Does **not** decide #445 (Stripe Native) — that stays jsboige's business call.

---

## Sources

- NuGet `DotNetNuke.Core` registration — 9.13.x (tops 9.13.9) and 10.x (→10.3.2) version lines (verified 2026-06-17).
- #520 (#131 CVE correction) — CVE-2025-64095 patched **10.1.1**; CVE-2025-52488 patched **10.0.1**; 2sxc cliff at **10.2.0+** (issue #6902).
- #514 (Step 0) — DNN 10.3.2 runtime = .NET Framework 4.8 (no .NET 8 jump).
- Repo: `DNNPlatform/bin/DotNetNuke.dll`, `DNNPlatform/web.config`, `DNNPlatform/Portals/1/2sxc/{Argumentum,Glossary3}/`.
- MEMORY.md — Phase A sandbox boot (IIS Express :8090, DNN 9.11.1 + 2sxc 15.02, 2026-06-02).
