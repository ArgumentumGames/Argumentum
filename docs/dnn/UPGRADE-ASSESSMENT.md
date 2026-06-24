# DNN Upgrade Assessment — Argumentum Platform

**Date**: 2026-06-07 (original) · **refreshed 2026-06-24** by po-2023
**Author**: po-2023 (assessment), reviewed ai-01
**Status**: Draft — awaiting jsboige validation
**Related issues**: #131 (DNN security/upgrade), #132 (DNN deployment), #134 (release v0.9.0)

> ## ⚠️ 2026-06-24 CORRECTION (2 material factual errors)
>
> The original 2026-06-07 assessment (below) contained two errors that materially change the upgrade path. Both are **VERIFIED** (NVD + official GHSA + DNN `DotNetNuke.Core` 10.3.2 `.nupkg` `lib/net48/`, 2026-06-17). Full correction: `docs/dnn-localization/131-cve-correction-and-target-refinement.md`.
>
> 1. **CVE patch level — 9.13.x closes ZERO of the 2 CVEs.** The original §3 / §4 Option B claimed the CVEs are "patched in DNN 9.13.x+". **WRONG.** CVE-2025-64095 (CVSS **10.0 CRITICAL**, unauth file upload → RCE) is patched at **10.1.1** ([GHSA-3m8r-w7xg-jqvw](https://github.com/dnnsoftware/Dnn.Platform/security/advisories/GHSA-3m8r-w7xg-jqvw)); CVE-2025-52488 (CVSS 8.6 HIGH, NTLM hash leak) at **10.0.1** ([GHSA-mgfv-2362-jq96](https://github.com/dnnsoftware/Dnn.Platform/security/advisories/GHSA-mgfv-2362-jq96)). The pragmatic security stop is **10.1.2** (closes both, sits before the 2sxc compat cliff @10.2.0). 9.13.x is **not** a security patch — stopping there leaves both critical CVEs open.
> 2. **DNN 10.x runtime is .NET Framework 4.8, NOT .NET 8.** The original §4 Option A assumed "9.13.x → 10.3.2 = .NET 4.8 → .NET 8" and derived an OpenStore blocker from it. **WRONG.** DNN 10.3.2 `.nupkg` ships `lib/net48/` as its only TFM folder → DNN 10.x runs on .NET Framework 4.8. The OpenStore/NBrightBuy (.NET Framework) blocker **dissolves** — NBrightBuy runs on DNN 10.x. The Stripe Native (#445) decision (§5) stands on its own merits (RazorEngine CVE, modernization), not on a .NET-8 blocker.
>
> **Actée target (decision #458): DNN 10.3.2 + 2sxc 21.** Argumentum already runs 2sxc **21.07** (§1), so the 2sxc compat cliff @10.2.0 is already crossed. See corrected path in §4.

---

## 1. Current State (Verified bin/ DLLs)

| Component | Installed Version | Source | Notes |
|-----------|------------------|--------|-------|
| **DNN Platform** | 9.11.1.19 | `bin/DotNetNuke.dll` FileVersion | .NET Framework 4.8 |
| **2sxc (ToSic.Sxc)** | 21.07.00 | `bin/ToSic.Eav.Apps.dll` + install pkg | LTS, current ✅ |
| **ToSic.Razor (RazorBlade)** | 4.4.1.0 | `bin/ToSic.Razor.dll` | Very old (upstream ~16.x) |
| **Connect.Razor** | 2.0.0.0 | `bin/Connect.Razor.dll` | 2sxc Razor helper |
| **NBrightBuy (OpenStore)** | 4.1.11.0 | `bin/NBrightBuy.dll` | E-commerce module |
| **RazorEngine** | 3.10.0 | `bin/RazorEngine.dll` | ⚠️ **CVE-2021-46703** (unfixable, used only by NBrightBuy) |
| **Stripe.net** | 41.8.0.0 | `bin/Stripe.net.dll` | Payment integration |
| **Imageflow.Net** | 0.14.0.0 | `bin/Imageflow.Net.dll` | Image optimization |

### Database

- **Connection**: `(localdb)\MSSQLLocalDB` → `ArgumentumGames` (Integrated Security)
- **Schema scripts**: up to `09.11.02.SqlDataProvider`
- **machineKey**: `REPLACE` placeholder (never committed, GDrive authoritative)

### Custom Templates (Argumentum 2sxc App)

Located at `DNNPlatform/Portals/1/2sxc/Argumentum/` — **4 custom templates, all on modern Razor14**:

| Template | `@inherits` | Function |
|----------|-------------|----------|
| `_FallacyExplorer_Root.cshtml` | `Custom.Hybrid.Razor14` | Fallacy taxonomy browser |
| `_RulesExplorer_RuleDetail.cshtml` | `Custom.Hybrid.Razor14` | Rule detail view |
| `_RulesExplorer_RuleList.cshtml` | `Custom.Hybrid.Razor14` | Rule list view |
| `_Album List.cshtml` | `Custom.Hybrid.Razor14` | Image gallery |

**No migration needed** for the Argumentum app itself — already on the latest template base.

---

## 2. Template Migration Inventory

| `@inherits` Type | Count | Migration Needed | Priority |
|------------------|-------|------------------|----------|
| `Custom.Hybrid.Razor14` | 16 | None | N/A |
| `Custom.Hybrid.Razor12` | 225 | Cosmetic bump to Razor14 | Low |
| `ToSic.Sxc.Dnn.RazorComponent` | **12** | **Required** (deprecated) | **High** |

### The 12 Deprecated RazorComponent Templates

All are generic Bootstrap-3 templates (Content + News5 apps), **none in the Argumentum app**:

```
Content/bs3/Layout/_Line.cshtml
Content/bs3/Link/_Large emphasized link.cshtml
Content/bs3/Link/_List of document-links.cshtml
Content/bs3/Link/_List of icon-links.cshtml
Content/bs3/Link/_List of image-links with overlay.cshtml
Content/bs3/Link/_List of image-links.cshtml
Content/bs3/Link/_List of links.cshtml
News5/bs3/_Details.cshtml
News5/bs3/_List archive.cshtml
News5/bs3/_List Columns without images.cshtml
News5/bs3/_List Columns.cshtml
News5/bs3/_List.cshtml
```

**Blocking dependency**: `Content/bs3/_Parts.cshtml` contains 12 `@helper` methods used by the templates above. Must migrate `_Parts.cshtml` first (convert `@helper` → `@functions` or partials).

**Estimated effort**: 3-5 hours for the full 12-file migration.

---

## 3. Security Posture

### Active CVEs on DNN 9.11.1

| CVE | Severity | Description | CVSS | Patched at | GHSA |
|-----|----------|-------------|------|------------|------|
| **CVE-2025-64095** | CRITICAL | Arbitrary file upload → RCE | **10.0** | **10.1.1** | [GHSA-3m8r-w7xg-jqvw](https://github.com/dnnsoftware/Dnn.Platform/security/advisories/GHSA-3m8r-w7xg-jqvw) |
| **CVE-2025-52488** | HIGH | NTLM hash disclosure via authenticated SSRF | 8.6 | **10.0.1** | [GHSA-mgfv-2362-jq96](https://github.com/dnnsoftware/Dnn.Platform/security/advisories/GHSA-mgfv-2362-jq96) |

> **CORRECTION (2026-06-24, VERIFIED NVD/GHSA):** the original version said "both patched in DNN 9.13.x+". That is **wrong** — **9.13.x (latest 9.13.9) closes ZERO of the 2 CVEs.** To close both you must reach at least **10.1.2** (NTLM fix @10.0.1 + file-upload fix @10.1.1). The 9.13.x palier is only a migration stepping-stone, not a security fix.

### Other Security Items

| Item | Risk | Status |
|------|------|--------|
| **RazorEngine 3.10.0** | CVE-2021-46703 (unfixable sandbox escape) | Used only by NBrightBuy — remove NBrightBuy → eliminate |
| **Dependabot 368 alerts** | All npm transitive (DNN vendor skins) | Low priority — not on .NET pipeline |
| **httpCookies requireSSL="false"** | Session hijacking on HTTP | Fix in web.config (PR #442 staged) |
| **cookieProtection="None"** (anonymous auth) | Tampering | Fix in web.config |
| **minRequiredPasswordLength="7"** | Brute force | Increase to 12+ |
| **customErrors mode="RemoteOnly"** | Information leak on local | Acceptable for dev, change for prod |

### Web.config Hardening (PR #442 — staged, jsboige applies on VPS)

- CSP headers, HSTS, secure cookies, crypto algorithm update
- **Independent of DNN upgrade** — can be applied immediately on production

---

## 4. Upgrade Path (CORRECTED 2026-06-24)

> **Two premises of the original 2026-06-07 path were wrong** (see header correction): (1) 9.13.x closes the CVEs — it closes **zero**; (2) DNN 10.x = .NET 8 — it is **.NET Framework 4.8**, so the OpenStore blocker dissolves. The path below reflects both corrections.

### Runtime fact: DNN 10.x stays on .NET Framework 4.8

VERIFIED 2026-06-17: DNN 10.3.2 `DotNetNuke.Core` `.nupkg` ships `lib/net48/` as its only TFM folder (no `lib/net8.0/`). The `.NET 8` in the DNN repo is build tooling (Cake), not the web runtime. **Consequence:** NBrightBuy/OpenStore (.NET Framework) runs on DNN 10.x — no eshop blocker. The 2sxc compat cliff @10.2.0 (issue #6902) is already crossed because Argumentum runs 2sxc **21.07** (§1).

### Corrected target: 10.1.2 (pragmatic security stop) or 10.3.2 (latest, actée)

| Stop | Closes CVEs? | 2sxc rework? | Notes |
|------|-------------|--------------|-------|
| **9.13.9** | ❌ none | No | Migration stepping-stone only — **not** a security fix |
| **10.1.2** | ✅ both (NTLM@10.0.1 + upload@10.1.1) | No | **Pragmatic security stop** — before the 10.2.0 cliff, least disruption |
| **10.3.2** | ✅ both | No (2sxc 21.07 in place) | **Actée target (decision #458)** — latest, full security |

```
9.11.1 → (optional 9.13.9 stepping-stone) → 10.1.2 (security floor) → 10.3.2 (actée latest)
```
All steps stay on **.NET Framework 4.8** — no runtime jump, no new hosting model.

| Step | What | Risk | Blockers |
|------|------|------|----------|
| **9.11.1 → 10.1.2** | Security floor (closes both CVEs) | Low–Med — same .NET 4.8 runtime, SQL migration across majors | None (eshop stays; 2sxc 21.07 OK) |
| **10.1.2 → 10.3.2** | Latest (decision #458) | Low — incremental within 10.x, pre-cliff | None |

**No eshop blocker** — the original "OpenStore has no .NET 8 build → gates 10.x" line is retracted (premise was .NET 8, which is false). Stripe Native (#445) is a separate modernization decision, not an upgrade gate.

### Option B (retracted): "Security Patch Only → 9.13.x, stop"

The original Option B recommended stopping at 9.13.x "to fix both CVEs without the .NET 8 jump". **Retracted** — 9.13.x fixes **zero** CVEs. Stopping at 9.13.x leaves both critical CVEs open. The security floor is **10.1.2**, not 9.13.x.

---

## 5. Eshop Strategy

| Option | Description | Pros | Cons | DNN 10.x Compatible? |
|--------|-------------|------|------|---------------------|
| **1. Keep OpenStore** | Stay on NBrightBuy 4.1.11 | No migration work, existing config | RazorEngine CVE (CVE-2021-46703) | ✅ (runs on 10.x — .NET Framework 4.8) |
| **2. Stripe Native** | Remove NBrightBuy, implement Stripe Checkout/Products | Eliminates RazorEngine CVE, modern API | Rewrite e-shop views, Stripe account setup needed | ✅ |
| **3. Remove Eshop** | Remove NBrightBuy, no replacement | Simplest, eliminates CVE | No e-commerce capability | ✅ |

**Decision jsboige (2026-06-07)**: **Option 2 (Stripe Native)** — validé. L'eshop actuel (OpenStore) sera remplacé par Stripe. Épic créée (#445) pour tracker la mise en œuvre. Le compte revendeur existant dans OpenStore sera évalué dans la conception Stripe (Stripe Connect marketplace ou modèle hors-ligne).

---

## 6. i18n Strategy for the DNN Site

### Current State
- The Argumentum 2sxc app hardcodes `text_en` for English content
- Glossary3 template is the multilingual reference
- FR content is the primary language

### Options

| Approach | Description | Effort | Maintenance |
|----------|-------------|--------|-------------|
| **A. 2sxc EAV Dimensions** | Use 2sxc's built-in language dimension system | Medium (restructure content types) | Low (native 2sxc) |
| **B. Field Suffixes** | Mirror CSV pattern (`Title`, `Title_en`, `Title_ru`, etc.) | Low (add fields) | Medium (manual sync) |
| **C. DNN Core Localization** | Use DNN's language packs + resx files | High (DNN native but rigid) | High |

**Recommendation**: **Option A (2sxc EAV Dimensions)** — native to the platform, supports the 8-language scope, and aligns with how 2sxc is designed to handle multilingual content. This should be scoped as a separate workstream after the security upgrade.

---

## 7. Risk Matrix

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| SQL migration failure (9.11→10.3.2) | Low-Med | High | Full DB backup before upgrade, test on LocalDB first; the 9.x→10.x jump is bigger than a minor |
| 12 RazorComponent templates break | Medium | Medium | Migration is code-only, no data impact; test each template |
| OpenStore incompatibility with 9.13.x | Low | Medium | 9.13.x stays on .NET 4.8 — OpenStore should be compatible; Phase 2 removes it entirely |
| ~~.NET 8 migration breaks custom modules~~ | ~~High~~ | ~~High~~ | **N/A — retracted:** DNN 10.x is .NET Framework 4.8 (§4), no .NET-8 runtime jump. Custom modules (.NET Framework) continue to run. |
| Downtime during upgrade | Medium | Medium | Plan maintenance window, test upgrade on clone first |
| Stripe integration complexity | Medium | Medium | Epic #445 — scope reseller marketplace vs. simple checkout early |

---

## 8. Proposed Execution Order

### Phase 1: Security Upgrade 9.11.1 → 10.1.2 (security floor) or 10.3.2 (actée #458)

**Objective**: Patch BOTH critical CVEs (requires reaching 10.x — 9.13.x closes zero, see §3 correction). Stay on .NET Framework 4.8 (no eshop blocker). Est. 1-2h downtime. **Target: 10.3.2 + 2sxc 21 (actée decision #458)**; 10.1.2 is the minimum security floor if a smaller jump is preferred.

#### Step 1: Pre-flight (on VPS, ~15 min)

1. **Full DB backup**: `BACKUP DATABASE [ArgumentumGames] TO DISK = N'...'` (SQL Server, not LocalDB on prod)
2. **Filesystem snapshot**: zip `DNNPlatform/` (or VPS snapshot)
3. **Verify current state**: `SELECT * FROM {databaseOwner}{objectQualifier}Version` → should show 9.11.1
4. **Export 2sxc app**: via 2sxc Admin UI → export Argumentum app (safety net)

#### Step 2: DNN Upgrade 9.11.1 → 10.3.2 (~30-45 min)

1. Download DNN 10.3.2 install package from dnncommunity.org
2. **Stop IIS** (or IIS Express if dev)
3. **Backup `bin/`**, `web.config`, `DotNetNuke.config`
4. Extract upgrade package over existing install (do NOT delete `App_Data/`, `Portals/`)
5. **Merge `web.config`**: keep connection string, machineKey (GDrive authoritative), custom modules. DNN upgrade may add new sections.
6. **Start IIS**
7. Navigate to site → DNN auto-runs upgrade wizard → SQL migration scripts execute
8. Verify: `SELECT * FROM {databaseOwner}{objectQualifier}Version` → 10.3.2 (actée target)
9. **Check 2sxc**: Admin → 2sxc should still show 21.07 (2sxc is independent of DNN version on .NET 4.8)

**Key concern**: `web.config` merge. DNN 9.12+ may add new assembly bindings or security settings. Manual merge required — never accept the default overwrite.

#### Step 3: RazorComponent Migration (~3-5h, can run in parallel)

1. Create backup of `Portals/1/2sxc/` (all apps)
2. **Migrate `_Parts.cshtml`** first (12 `@helper` → `@functions` with `static` methods, or split into partials)
3. Migrate each of the 12 templates: `@inherits ToSic.Sxc.Dnn.RazorComponent` → `@inherits Custom.Hybrid.Razor14`
4. Replace `@helper` calls with the new pattern
5. **Test each template** in 2sxc preview mode
6. Verify the 4 Argumentum templates (`_FallacyExplorer_*`, `_RulesExplorer_*`, `_Album List`) are **untouched** (already Razor14)

**2sxc compatibility note**: RazorComponent is deprecated since 2sxc 12. Razor14 is the current stable base. The migration is mechanical (find/replace + helper refactoring).

#### Step 4: Verification Checklist

- [ ] Site loads (homepage, admin panel)
- [ ] Argumentum app: Fallacy Explorer renders correctly
- [ ] Argumentum app: Rules Explorer renders correctly
- [ ] Argumentum app: Album List renders correctly
- [ ] 2sxc Admin → manages content types, views, data
- [ ] OpenStore/NBrightBuy: admin accessible (still on .NET 4.8, should work)
- [ ] No JavaScript console errors on public pages
- [ ] SQL: no orphaned schema objects
- [ ] Login works (admin + test user)
- [ ] SEO URLs still resolve (SiteUrls.config intact)

### Phase 2: Eshop Migration — Stripe Native (jsboige decision: Option 2 ✅)

**Decision made 2026-06-07**: Stripe Native replaces OpenStore/NBrightBuy. Epic #445 tracks implementation.

1. Design Stripe integration (Connect marketplace vs. offline model for existing reseller account)
2. Implement Stripe Checkout/Products for Argumentum game sales
3. Remove NBrightBuy + RazorEngine dependency → eliminates CVE-2021-46703
4. Evaluate manufacturing/distribution partners (EU-based, languages we support)
5. ~~This phase **unblocks DNN 10.x** (.NET 8)~~ — **RETRACTED:** DNN 10.x is .NET Framework 4.8 (§4 correction), so the eshop never blocked 10.x. Stripe Native stands on modernization + CVE elimination (RazorEngine), not on an upgrade gate.

### Phase 3: DNN 10.3.2 (actée target, decision #458)
1. Upgrade DNN 9.11.1 → 10.3.2 (all on .NET Framework 4.8 — no runtime jump)
2. Apply SQL migration across the major (full DB backup first — §9 prerequisites)
3. Update connection strings / `web.config` merge (keep machineKey, custom modules)
4. Full regression test on production clone (incl. 4 Argumentum Razor14 templates, 2sxc 21.07, OpenStore if kept)
5. Security floor is reached at 10.1.2; 10.3.2 is the latest actée target.

### Phase 4: i18n (After Phase 3, separate workstream)
1. Implement 2sxc EAV Dimensions for multilingual content
2. Migrate hardcoded `text_en` to proper language dimensions
3. Align with pipeline's 8-language scope

---

## 9. Prerequisites (jsboige action needed)

- [ ] **VPS access** — for production upgrade and backup
- [x] **Eshop decision** — ~~Option 1/2/3~~ → **Option 2 (Stripe Native)** validé 2026-06-07
- [ ] **Maintenance window** — for DNN upgrade (est. 1-2h downtime)
- [ ] **Production DB backup** — before any schema migration
- [ ] **Stripe account** — for payment integration (Epic #445)

## 10. v0.9.0 release coupling recommendation (2026-06-24)

**Question (dispatch ai-01 / dossier release #591 point #3):** should the v0.9.0 tag wait for the DNN #131 upgrade, or de-couple (tag assets-only)?

**Recommendation: DE-COUPLE. Tag v0.9.0 assets-only NOW; DNN upgrade as a separate post-release milestone.**

Rationale:
1. **Assets are complete & verified.** 8-language CSV + MindMap SVGs (#565) + PDFs (reported 12-Jun regen 64/64) + OWL 5.13 MB are the release payload — none depend on the DNN site.
2. **DNN upgrade is a VPS-side operational task**, not a code deliverable. It needs jsboige on the production server (full DB backup, IIS stop, `web.config` merge, maintenance window — §9 prerequisites). It cannot be automated by the worker cluster. Coupling the tag to it makes the release hostage to an ops calendar.
3. **The DNN site is currently functional** (DNN 9.11.1 + 2sxc 21.07, 4 Argumentum templates on Razor14). The 2 critical CVEs are a **security-debt** concern, not a blocker for shipping the game assets. The site can be upgraded in a maintenance window post-release.
4. **Decision #458 target (10.3.2 + 2sxc 21) is compatible** — 2sxc 21.07 is already installed (the only rework that *could* be needed is the 12 RazorComponent templates below).

**If jsboige prefers coupling** (release = assets + site on 10.3.2), the gating work is:
- DNN upgrade on VPS (§8 Phase 1, ~1-2h ops) — jsboige only.
- The 12 RazorComponent template migration (below) — worker can prepare, but must be tested live on the upgraded site.

### 12 RazorComponent template migration — effort estimate

Per §2: 12 generic Bootstrap-3 templates (Content/News5 apps), **none in the Argumentum app** (the 4 Argumentum templates are already Razor14). Blocking dependency: `Content/bs3/_Parts.cshtml` (12 `@helper` methods) must migrate first.

| Sub-task | Effort | Notes |
|----------|--------|-------|
| `_Parts.cshtml`: 12 `@helper` → `@functions` (static) or partials | ~1h | Mechanical, but each `@helper` signature must be preserved |
| 12 templates: `@inherits` swap + `@helper` call refactor | ~2-3h | Find/replace + verify each call-site |
| Test each of 12 in 2sxc preview | ~1-2h | Live 2sxc on upgraded site |
| **Total** | **~4-6h** | Code-only, no data impact; the original 3-5h estimate (§2) was tight |

**Risk level: Medium.** Pure code migration (no DB/data), but the `_Parts.cshtml` `@helper`→`@functions` refactor is the one non-trivial step (helpers become static methods with explicit args). If a template isn't used in production, it can be skipped (verify usage first).

**Conclusion:** the 12-template migration is a **self-contained ~4-6h workstream**, independent of the v0.9.0 assets. It can ship after the release tag without blocking it. Recommend tagging v0.9.0 assets-only and running the DNN upgrade + template migration as a post-release ops milestone (tracked in #131/#132).

---

## Appendix: Version Ground Truth

All versions verified from `bin/` DLL FileVersion attributes on 2026-06-07:

```
DotNetNuke.dll           = 9.11.1.19
ToSic.Eav.Apps.dll       = 21.07.00
ToSic.Razor.dll          = 4.4.1.0
Connect.Razor.dll        = 2.0.0.0
NBrightBuy.dll           = 4.1.11.0
RazorEngine.dll          = 3.10.0
Stripe.net.dll           = 41.8.0.0
Imageflow.Net.dll        = 0.14.0.0
```

Connection string: `Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArgumentumGames;Integrated Security=True`
