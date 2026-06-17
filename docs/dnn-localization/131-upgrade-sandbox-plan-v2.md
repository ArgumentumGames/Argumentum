# #131 — DNN 9.11.1 → 9.13.x → 10.3.2 Upgrade: Sandbox Palier v2 (go/no-go matrix)

**Issue:** [#131 — DNN platform + déploiement](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17
**Base:** master `fc8313b3` (clean, in sync with origin)
**Status:** **SANDBOX/RESEARCH document. No production deploy. No CSV touch. Awaits jsboige go/no-go per step.**

This document **EXTENDS** (does not duplicate) two prior analyses:
- [`131-upgrade-9.13-sandbox-plan.md`](./131-upgrade-9.13-sandbox-plan.md) — the #479 9.11.1→9.13.x security-palier plan (CVE table, disk blocker, sandbox sketch).
- [`../dnn/UPGRADE-ASSESSMENT.md`](../dnn/UPGRADE-ASSESSMENT.md) — the comprehensive 2026-06-07 assessment (current-state inventory, Option A staircase, dependency CVEs, jsboige = Stripe Native decision).

It adds three things those docs do **not** carry: (1) a **CVE reconciliation** that flags one factual error in UPGRADE-ASSESSMENT §3, (2) a **decisive finding on the DNN 10 runtime** that reverses a core premise of the assessment, and (3) a **2sxc app compatibility matrix** + **go/no-go per staircase step**.

---

## 1. Executive summary — two reversals

Two findings from authoritative sources (DNN core contributors + 2sxc official changelog + CVE advisories) change the risk picture materially versus the 2026-06-07 assessment:

| # | Prior assumption (UPGRADE-ASSESSMENT / #479) | Verified finding (this doc) | Impact |
|---|----------------------------------------------|------------------------------|--------|
| A | CVE-2025-52488 (NTLM) patched in 9.13.x (ASSESSMENT §3) | **Patched in DNN 10.0.1+ ONLY.** 9.13.x does **not** close it. (#479 §1 was right; ASSESSMENT §3 is wrong.) | The 9.13.x "security palier" closes **only 1 of 2** critical CVEs. Closing both requires reaching 10.0.1+. |
| B | DNN 10.x = .NET 8 (major breaking change) → OpenStore/NBrightBuy 4.1.11 has no .NET 8 port = blocker | **DNN 10.x runtime is .NET Framework 4.8**, not .NET 8. Proven by DNN core contributors (forum) AND circumstantially by 2sxc: 2sxc is .NET-Framework-compiled and demonstrably runs **on DNN 10.02.01** (2sxc v21.00.02 ships a workaround for a DNN 10.02.01 bug). | The `.NET 8 jump` that motivated the OpenStore blocker (and fed the Stripe Native #445 decision) **likely does not apply on DNN**. NBrightBuy/OpenStore (.NET Framework) probably still runs on DNN 10.x. **Dissolves the blocker — needs sandbox confirmation + re-asks the Stripe Native decision.** |

**Bottom line for #131:** the 9.11.1 → 10.x staircase is **more viable than the assessment concluded** (2sxc and the .NET-Framework commerce stack appear to move together to DNN 10). The single largest residual risk is the **2sxc v20 "Moment-of-Truth" breaking changes** (module path rename, SQL reorg, SexyContent→2sxc) on the installed apps — not a .NET runtime jump. The full CVE closure still requires DNN 10.0.1+.

> ⚠️ **Calibration:** finding A (CVE patch version) is **VERIFIED** from the SystemVll/CVE-2025-52488 advisory + NVD description. Finding B (DNN 10 runtime) is **strong-evidence / TO-CONFIRM-IN-SANDBOX** — it rests on (a) DNN core-contributor forum statements from 2024-05-30 and (b) the 2sxc-runs-on-DNN-10.02.01 circumstantial proof. Step 0 of the sandbox protocol below exists to confirm B empirically.

---

## 2. CVE reconciliation (VERIFIED)

| CVE | Type | CVSS | Patched in | DNN 9.11.1 vulnerable? | Source |
|-----|------|------|------------|------------------------|--------|
| **CVE-2025-64095** | Unauthenticated arbitrary file upload | 9.8 (CRITICAL) | **DNN 9.13.x** | ✅ Yes (9.11.1 < 9.13.x) | #479 §1 + ASSESSMENT §3 (agree) |
| **CVE-2025-52488** | NTLM hash exposure via SMB (`…/Install/…` UNC) | 8.6 (HIGH) | **DNN 10.0.1 ONLY** | ✅ Yes (9.11.1 < 10.0.1) | SystemVll/CVE-2025-52488 GitHub advisory: "DNN **6.0.0 to before 10.0.1**"; NVD description |

**Correction to flag in UPGRADE-ASSESSMENT §3:** that section states both CVEs are patched in 9.13.x+. That is **wrong for CVE-2025-52488** — it is patched in **10.0.1 only**. #479 §1 is correct. **Recommended follow-up (not done here):** edit ASSESSMENT §3 to split the two CVEs once this reconciliation is accepted. (I did not edit the existing assessment in place — it is jsboige's doc and the correction should land after review, per the "read-before-action" discipline.)

**Consequence for the staircase:** stopping at 9.13.x closes the file-upload RCE but **leaves the NTLM-exposure CVE open**. To close **both**, the path must reach **DNN 10.0.1+**.

---

## 3. The decisive finding — DNN 10.x runtime is .NET Framework 4.8 (not .NET 8)

This is the single fact that most changes the #131 risk picture, because the UPGRADE-ASSESSMENT treated "DNN 10.x = .NET 8" as the source of the OpenStore/NBrightBuy blocker.

### 3.1 Evidence

**Primary — DNN core contributors, dnncommunity.org forum "DNN 9 & DNN 10?" (2024-05-30):**
- **Timo Breumelhof** (DNN MVP): *"No, DNN 10 will not be .NET Core [...] See: The technical Future of DNN."*
- **Daniel Valadas** (DNN core): *"You will see .NET Framework code, .NET Standard code and .NET Core code in the repository. Everything runtime is .NET Framework except in some places that use .NET Standard like for dependency injection [...]. The few places where you will see .NET Core code is build-time only (we use Cake [...]). But this does not get into the runtime and 3rd party extensions still need to target .NET Framework."*
- The OP confirms the confusion: *"I saw 'dotnet 8' in the repos and figured the base code was changing"* — the `.NET 8` in the DNN GitHub repo is **build tooling (Cake/Nuke), not the web runtime.**

**Circumstantial proof — 2sxc official changelog (`docs.2sxc.org/abyss/releases/history/changes-all.html`):**
- **2sxc v21.00.02 (2026-01-12):** *"☢️ Dnn: Workaround for bug in DNN 10.02.01 related to assets (js/css) because of Dnn changes in Client Dependency Management."* → For 2sxc to need (and ship) a workaround for a **DNN 10.02.1** bug, 2sxc v21 must be **running on DNN 10.02.1**.
- **2sxc v20.00.05 (2025-08-26):** *"Formula: Introduce `context.user.isContentEditor` to match latest DNN 10 features."* → 2sxc actively tracks DNN 10 features.
- **2sxc v20.00.00 (2025-06-25):** *"☢️ Minimum DNN version is now v9.11.02 (previously v9.6.2)."* → the current 2sxc floor (9.11.02) matches Argumentum's installed DNN (9.11.1.19).

2sxc is a **.NET Framework 4.8** module (its own docs: *"For DNN: Requires .NET Framework 4.8"*). If DNN 10.02.01 required .NET 8, a .NET-Framework-compiled 2sxc **could not load into it**. The fact that it does is itself the proof: **DNN 10.x loads .NET Framework 4.8 modules.**

### 3.2 What this dissolves

- **OpenStore / NBrightBuy 4.1.11 blocker (ASSESSMENT):** that blocker was predicated on "no .NET 8 port". If DNN 10.x is .NET Framework 4.8, **NBrightBuy/OpenStore (.NET Framework) should still run on DNN 10.x.** The blocker likely **does not exist on DNN** — it was a real blocker only for an Oqtane/.NET-8 migration path.
- This **re-opens** the jsboige "Stripe Native (#445)" decision: its stated rationale (OpenStore blocks 10.x) may no longer hold. This is a **business decision for jsboige**, not a finding to overturn here — but it must be surfaced (see §8).

### 3.3 The real residual risk shifts to 2sxc v20 breaking changes

With the .NET-8 jump off the table, the dominant upgrade risk becomes **2sxc v20 "Moment-of-Truth" breaking changes** (the version that ships on DNN 10), which restructure things Argumentum's installed apps may touch:

- **Module path rename** (v20.00.00): `/DesktopModules/ToSic_SexyContent/` → `/DesktopModules/ToSic.Sxc/`. Affects any template linking the old `$2sxc.min.js` path or the old `Thumbnailer.aspx`.
- **SQL reorg** (v20.00.00): all SQL tables restructured. Breaking only for code hitting the DB directly.
- **`SexyContentWebPage` deprecation** (v20.00.00): 2sxc stops auto-creating `web.config` in `/2sxc/`, so old Razor files no longer default to `SexyContentWebPage`. **Affects the installed RazorComponent templates** (the ASSESSMENT lists 12 deprecated RazorComponent templates — Content/News5 vintage).
- **Old `IEntity` / Razor APIs removed** (v20.00.00): `GetBestValue(...)`, old `IEntity` variants, etc. Affects any custom Razor using exotic APIs.

Argumentum's 4 custom templates (`_FallacyExplorer_Root.cshtml`, `_RulesExplorer_*`, `_Album List.cshtml`) must be checked against these — they are the highest-risk custom surface.

---

## 4. 2sxc ↔ DNN 10 compatibility — VERIFIED compatible

- 2sxc **v21** (LTS line) runs on DNN 10.02.01 (§3.1 proof).
- 2sxc **v20.00.00** minimum DNN = **9.11.02** — Argumentum's installed 9.11.1 clears it.
- 2sxc tracks DNN 10 features actively (`context.user.isContentEditor` "to match latest DNN 10", v20.00.05).
- **No 2sxc/DNN-10 hard blocker exists.** (The earlier hypothesis that "2sxc on DNN 10 = Oqtane only" conflated the .NET-8/Oqtane path with DNN; DNN 10 stays .NET Framework, so the DNN build of 2sxc applies.)

---

## 5. 2sxc app compatibility matrix (the 4 dispatch-named apps + Argumentum)

Installed DNN: **9.11.1.19**. Installed 2sxc: **21.07** (per ASSESSMENT). Source: repo `DNNPlatform/Portals/1/2sxc/`.

| App | Type | Installed version (repo evidence) | 2sxc v20/v21 risk | DNN 10 risk | Go/No-go |
|-----|------|-----------------------------------|-------------------|-------------|----------|
| **Argumentum** | **Custom content app** (4 bespoke Razor views: `_FallacyExplorer_Root`, `_RulesExplorer_RuleDetail/List`, `_Album List`) | `src/`+`dist/`, no 2sxc `app.json` → bespoke | **HIGH** — must audit the 4 `.cshtml` for removed APIs (`SexyContentWebPage`, `GetBestValue`, old `IEntity`), path refs to old module folder. | LOW (if 2sxc-audit clean) | **GO with mandatory template audit (§7 Step 2c)** |
| **Glossary3** | 2sinc catalog app | `App_Data/app.xml` version history: 07.00.00 → 07.04.02 → **14.09.00** (installed = **v14-era**, pre-typed-mode) | **MEDIUM** — installed version predates the v17.07 "all apps typed" re-release. May trip `GetBestValue`/`SexyContentWebPage` removals. | LOW | **GO; plan a Glossary3 update to the current typed v3 build as part of Step 2** |
| **News5** | 2sinc catalog app | `bs3/bs4/bs5` editions, `DnnSearch` (current generation structure) | LOW-MEDIUM — current generation, but verify no `$2sxc.min.js` old-path refs and `app-` WebApi prefix removal (v20) | LOW | **GO** |
| **Blog5** | 2sinc catalog app | `bs3/bs4/bs5`, `api/`, `DnnSearch` (current generation) | LOW-MEDIUM — same as News5 | LOW | **GO** |

**Repo-grounding note:** the other 16 installed 2sxc apps (Accordion4, AddSearch3, CTA3, Content, Counter2, EventsAndCourses6, Faq4, Gallery7, IFrame3, ImageCompare2, ImageHotspots3, Jobs2, MobiusForms5, PeopleDirectory4, PodCast2, PopupMessage3) are not in the dispatch scope; the **Content** app's deprecated RazorComponent templates are the ASSESSMENT's known 12-template legacy surface.

**App-matrix verdict:** all 4 named apps are **GO**, with the Argumentum custom templates as the single mandatory-audit item and Glossary3 as a recommended version bump.

---

## 6. 9.x → 10.x breaking changes (documented)

Consolidated from the 2sxc changelog + DNN release line + ASSESSMENT:

**DNN-side (9.13.x → 10.x):**
- Client Dependency Management change (DNN 10.02.01) broke 2sxc asset loading → needs **2sxc v21.00.02+** (already satisfied: installed 21.07).
- Community-reported friction: admin menu control rendering, dependency resolution (anecdotal; verify in sandbox).
- CVE-2025-52488 closed at 10.0.1 (§2).

**2sxc-side (v20 "Moment-of-Truth", the version floor for DNN 10):**
- Module moved to `/DesktopModules/ToSic.Sxc/`, renamed `2sxc` (was `SexyContent`) — breaks old direct asset links.
- SQL tables fully restructured — breaks direct-DB code only.
- `SexyContentWebPage` auto-base removed — old Razor templates must declare `@inherits` explicitly.
- Old `IEntity`/`GetBestValue(...)` APIs removed — breaks exotic custom Razor.
- Old `app-content`/`app-query`/`app-api` WebApi routes removed — use unprefixed routes.
- jQuery no longer auto-loaded for old Razor base classes.

**Dependency-side (ASSESSMENT §2, unchanged):**
- RazorEngine 3.10.0 → CVE-2021-46703 (unrelated to DNN version; independent fix).
- Stripe.net 41.8.0 (no known CVE).
- OpenStore/NBrightBuy 4.1.11 — **re-assessed: blocker likely dissolves** (§3.2).

---

## 7. The staircase — go/no-go per step

Each step is **sandbox-only**. No step touches production without an explicit jsboige GO.

### Step 0 — Confirm the DNN 10 runtime (THE gate)
- **Action (sandbox):** stand up a clean DNN **10.3.2** install (fresh DB), confirm `web.config` `targetFramework` and that a stock .NET Framework 4.8 module loads.
- **Pass criterion:** DNN 10.3.2 runs on .NET Framework 4.8 (finding B confirmed empirically).
- **Go/No-go:** **GO** if confirmed. **NO-GO + escalate to jsboige** if DNN 10.3.2 turns out to be .NET 8 (would revive the OpenStore blocker + Oqtane question).
- *Why first:* every downstream go/no-go depends on this single fact.

### Step 1 — 9.11.1 → 9.13.x (security palier, partial)
- **Action:** upgrade a sandbox clone of the live site to DNN **9.13.x** (latest 9.13). Keep 2sxc at 21.07.
- **Pass criterion:** site boots, all 4 Argumentum custom views render, Glossary3/News5/Blog5 render, no white-screen.
- **CVE result:** closes **CVE-2025-64095** only. **CVE-2025-52488 still open.**
- **Go/No-go:** **GO** (low-risk incremental). **Decision point for jsboige:** is closing only 1 of 2 CVEs acceptable, or is the full path to 10.0.1 required? If full closure is required, Step 1 is a stepping stone, not a destination.

### Step 2 — Argumentum template audit + Glossary3 bump (prerequisite to 10.x)
- **Action (no DNN change):**
  - (a) Audit the 4 Argumentum `.cshtml` for `SexyContentWebPage`, `GetBestValue`, old `IEntity`, old `app-` routes, old module-folder asset links.
  - (b) Update Glossary3 from v14.09.00 to the current typed v3 build.
  - (c) Verify News5/Blog5 have no old `$2sxc.min.js` / `Thumbnailer.aspx` refs.
- **Pass criterion:** all templates pass the 2sxc v20 deprecation scanner / load clean on 2sxc v21.
- **Go/No-go:** **GO** required before Step 3. This is the highest-risk custom surface.

### Step 3 — 9.13.x → 10.0.1+ → 10.3.2 (full CVE closure)
- **Action:** upgrade the sandbox (post Step 1+2) through **10.0.1** then to **10.3.2**. 2sxc stays v21.07 (≥ v21.00.02, satisfies the DNN 10.02.01 asset-workaround).
- **Pass criterion:** site boots on DNN 10.3.2, all 4 Argumentum views + 3 catalog apps render, `web.config` targetFramework = 4.8, commerce stack (if retained) loads.
- **CVE result:** closes **both** CVE-2025-64095 and CVE-2025-52488. ✅
- **Go/No-go:** **GO** if Step 0 confirmed .NET Framework + Step 2 audit clean. This is the destination if full CVE closure is required.

### Step 4 — Production deployment decision (jsboige-only gate)
- **Action:** none by worker. jsboige reviews sandbox evidence from Steps 0-3.
- **Go/No-go:** **EXCLUSIVELY jsboige.** No worker (po-2023/po-2024/ai-01) deploys to production.

---

## 8. Open questions for jsboige

1. **CVE closure target:** stop at 9.13.x (1/2 CVEs) or require 10.0.1+ (both)? (Step 1 vs Step 3 destination.)
2. **Stripe Native (#445) re-decision:** given finding B (DNN 10.x is likely .NET Framework 4.8 → OpenStore blocker likely dissolves), is Stripe Native still the chosen path, or does retaining OpenStore become viable on DNN 10? This is a business decision; the technical blocker may no longer apply.
3. **Argumentum custom templates:** confirm the 4 bespoke `.cshtml` are the only custom 2sxc surface (so the Step 2 audit scope is complete).
4. **Sandbox provision:** the disk blocker (ASSESSMENT §5) is resolved (Docker migration, ~15 GB free). Is a sandbox VM/container provisioned, or does Step 0 run on jsboige's dev box?

---

## 9. Sandbox validation protocol (NO production)

- All steps run on an **isolated clone** of the live DB + `DNNPlatform/` files. Never against the production DB or the live IIS site.
- **Rollback:** snapshot the sandbox DB + filesystem before each step; restore on any white-screen or render failure.
- **Evidence captured per step:** (a) `web.config` targetFramework, (b) DNN version string from `DotNetNuke.dll`, (c) screenshots of the 4 Argumentum views + 3 catalog apps rendering, (d) 2sxc deprecation-scanner output.
- **What the worker does NOT do:** deploy to prod, touch `docs/dnn-localization/dnn-ui-strings.csv` (po-2024 lane, #490), modify any production CSV, or take the Stripe Native decision.

---

## 10. Gate boundaries (this document)

- ✅ Documents the upgrade path with go/no-go per step.
- ✅ Reconciles the CVE facts and flags the ASSESSMENT §3 error.
- ✅ Surfaces the .NET-Framework-4.8 finding that reverses the OpenStore blocker premise.
- ✅ Provides the 2sxc app compatibility matrix for the 4 named apps.
- ❌ Does **not** deploy anything to production.
- ❌ Does **not** edit the production CSVs or `dnn-ui-strings.csv`.
- ❌ Does **not** overturn the Stripe Native #445 decision — only surfaces that its rationale may need re-examination.
- ❌ Does **not** edit UPGRADE-ASSESSMENT.md in place — proposes the §3 correction as a follow-up (§2).

---

## Sources

- DNN forum "DNN 9 & DNN 10?" — `dnncommunity.org/forums/Getting-Started/new-to-dnn/dnn-9-dnn-10/` (Valadas, Breumelhof; 2024-05-30).
- 2sxc changelog (all versions) — `docs.2sxc.org/abyss/releases/history/changes-all.html` (v21.00.02 2026-01-12; v20.00.05 2025-08-26; v20.00.00 2025-06-25).
- CVE-2025-52488 — SystemVll/CVE-2025-52488 GitHub advisory ("DNN 6.0.0 to before 10.0.1") + NVD.
- Repo: `DNNPlatform/web.config` (`targetFramework="4.8"`), `DNNPlatform/bin/DotNetNuke.dll` (9.11.1.19), `DNNPlatform/Portals/1/2sxc/Glossary3/App_Data/app.xml` (14.09.00), `DNNPlatform/Portals/1/2sxc/Argumentum/` (4 bespoke `.cshtml`).

---

*Worker research document. po-2023 signals findings + go/no-go matrix; ai-01 reviews; jsboige decides CVE target, Stripe Native re-decision, and production deployment. No production system changed.*
