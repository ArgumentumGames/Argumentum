# DNN Upgrade Assessment — Argumentum Platform

**Date**: 2026-06-07
**Author**: po-2023 (assessment), reviewed ai-01
**Status**: Draft — awaiting jsboige validation
**Related issues**: #131 (DNN security/upgrade), #132 (DNN deployment), #134 (release v0.9.0)

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

| CVE | Severity | Description | CVSS |
|-----|----------|-------------|------|
| **CVE-2025-52488** | CRITICAL | NTLM hash disclosure via authenticated SSRF | 9.1 |
| **CVE-2025-64095** | CRITICAL | Arbitrary file upload → RCE | 9.8 |

Both are patched in DNN 9.13.x+.

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

## 4. Upgrade Path

### Option A: Staircase (9.11.1 → 9.13.x → 10.3.2)

```
9.11.1 (.NET 4.8) → 9.13.x (.NET 4.8 safe) → 10.3.2 (.NET 8+)
```

| Step | What | Risk | Blockers |
|------|------|------|----------|
| **9.11.1 → 9.13.x** | Security patches (fixes both CVEs) | Low — same .NET 4.8 base, incremental SQL migration | None |
| **9.13.x → 10.3.2** | Major platform upgrade (.NET 4.8 → .NET 8) | **High** — breaking changes, new hosting model, SQL schema jump | **OpenStore has no .NET 8 build** |

**Blocker**: NBrightBuy/OpenStore 4.1.11 targets .NET Framework. No .NET 8 port exists. **DNN 10.x upgrade is gated on resolving the eshop.**

### Option B: Security Patch Only (9.11.1 → 9.13.x, stop)

- Fixes both critical CVEs
- Stays on .NET 4.8 (no OpenStore blocker)
- 12 RazorComponent templates still need migration for 2sxc compatibility
- **Recommended as Phase 1** — unblocks security without the .NET 8 jump

---

## 5. Eshop Strategy

| Option | Description | Pros | Cons | DNN 10.x Compatible? |
|--------|-------------|------|------|---------------------|
| **1. Keep OpenStore** | Stay on NBrightBuy 4.1.11 | No migration work, existing config | RazorEngine CVE, **blocks DNN 10.x** | ❌ |
| **2. Stripe Native** | Remove NBrightBuy, implement Stripe Checkout/Products | Eliminates RazorEngine CVE, .NET 8 compatible, modern API | Rewrite e-shop views, Stripe account setup needed | ✅ |
| **3. Remove Eshop** | Remove NBrightBuy, no replacement | Simplest, eliminates CVE, unblocks upgrade | No e-commerce capability | ✅ |

**Recommendation**: **Option 2 (Stripe Native)** — eliminates the unfixable CVE, future-proofs for DNN 10.x, and Stripe Products API provides equivalent functionality. Option 3 is acceptable if e-commerce is not a current business requirement.

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
| SQL migration failure (9.11→9.13) | Low | High | Full DB backup before upgrade, test on LocalDB first |
| 12 RazorComponent templates break | Medium | Medium | Migration is code-only, no data impact; test each template |
| OpenStore incompatibility with 9.13.x | Low | Medium | 9.13.x stays on .NET 4.8 — OpenStore should be compatible |
| .NET 8 migration breaks custom modules | High | High | Phase 2 only — after eshop decision, full test environment |
| Downtime during upgrade | Medium | Medium | Plan maintenance window, test upgrade on clone first |

---

## 8. Proposed Execution Order

### Phase 1: Security (Immediate — no blockers)
1. ✅ Apply web.config hardening (PR #442 — jsboige applies on VPS)
2. Upgrade DNN 9.11.1 → 9.13.x (fixes CVE-2025-52488 + CVE-2025-64095)
3. Migrate 12 RazorComponent → Razor14 templates
4. Verify all 4 Argumentum custom templates still work
5. Smoke test on LocalDB clone before production

### Phase 2: Eshop Decision (jsboige call needed)
1. Evaluate business need for e-commerce
2. If yes → implement Stripe Native (eliminates RazorEngine CVE)
3. If no → remove NBrightBuy (cleanest path to DNN 10.x)
4. Either way → eliminate RazorEngine dependency

### Phase 3: DNN 10.x (After Phase 2)
1. Upgrade DNN 9.13.x → 10.3.2
2. Migrate hosting from .NET Framework 4.8 → .NET 8
3. Update connection strings, verify SQL compatibility
4. Full regression test on production clone

### Phase 4: i18n (After Phase 3, separate workstream)
1. Implement 2sxc EAV Dimensions for multilingual content
2. Migrate hardcoded `text_en` to proper language dimensions
3. Align with pipeline's 8-language scope

---

## 9. Prerequisites (jsboige action needed)

- [ ] **VPS access** — for production upgrade and backup
- [ ] **Eshop decision** — Option 1/2/3 (blocks Phase 2+)
- [ ] **Maintenance window** — for DNN upgrade (est. 1-2h downtime)
- [ ] **Production DB backup** — before any schema migration
- [ ] **Stripe account** — if Option 2 chosen (for payment integration)

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
