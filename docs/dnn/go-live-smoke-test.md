# DNN 10.3.2 Go-Live — Smoke-Test Checklist

**Date**: 2026-06-26 · **Author**: po-2023 (dispatched by ai-01 v3, secondary track)
**Status**: Actionable checklist — **complements** the [#132 production deployment runbook](../dnn-localization/132-deployment-runbook.md) (PR #594), does NOT duplicate it. Read-only doc, no code.
**Purpose**: The #132 runbook covers the upgrade wizard + rollback contract. This doc is the **detailed smoke test** run immediately after the wizard completes (Phase 5, step 5 of #132) before exiting maintenance mode — the gate that says "10.3.2 is live and serving".
**Related**: #132 (deployment runbook), #131 (upgrade arc), #596 (12 Razor14 templates), #597 (4 social-auth connectors), [UPGRADE-ASSESSMENT.md](UPGRADE-ASSESSMENT.md) §5 (eshop), [sandbox-bootstrap-runbook.md](sandbox-bootstrap-runbook.md).

---

## TL;DR

After the DNN 10.3.2 wizard + 2sxc 21 upgrade + 12-template deploy (all detailed in [#132](../dnn-localization/132-deployment-runbook.md) §5/§5.5), run **this checklist** on production before exiting maintenance mode. Three sections: **(A) Platform core**, **(B) Argumentum 2sxc app + 12 Razor14 templates** (#596), **(C) Social auth connectors** (#597) + **eshop path**. Every item is a green/red signal. **All green → exit maintenance. Any red → rollback** (#132 §6).

> **Scope note**: #132 already sequences the upgrade. This doc exists because (1) #132 treats smoke as one line per phase, (2) it predates #596 (Razor14 templates) and #597 (auth connectors) so doesn't smoke-test them, and (3) a focused, URL-level checklist is what an operator runs in the maintenance window. It references #132 rather than repeating it.

## A. Platform core (post-wizard)

| # | Check | How | Green | Red flag |
|---|-------|-----|-------|----------|
| A1 | DNN version | `SELECT Major,Minor,Build FROM {db}.{obj}Version` | `10.3.2` | `9.x` (wizard didn't run) |
| A2 | Homepage loads | `GET /` (anon) | HTTP 200, site skin renders | 500 / yellow-screen / `0x80131040` (binding) |
| A3 | Admin panel | login as admin → PersonaBar | loads, no JS console errors | `DnnJsInclude` crash = 2sxc cliff not crossed (re-do #132 §5.5.a) |
| A4 | Language switcher | toggle FR ⇄ en-US | content switches, no 404 | FR-leak into EN (cf. #216 regression class) |
| A5 | Login (local admin) | DNN native login | succeeds, session cookie set | `aspnet_Membership` schema mismatch |
| A6 | SEO URLs | a known friendly URL resolves | 200, correct page | SiteUrls.config lost in web.config merge |

## B. Argumentum 2sxc app + 12 Razor14 templates (#596)

> The 12 templates migrated in #596 are file-based Razor, interpreted by 2sxc 21 at runtime. Each must render without a 2sxc yellow-screen.

| # | Template (path under `Portals/1/2sxc/`) | Check | Green | Red flag |
|---|---|---|---|---|
| B1 | `Argumentum/_FallacyExplorer_Root.cshtml` | load Fallacy Explorer page | taxonomy renders | (already Razor14 — if red, 2sxc 21 upgrade regressed) |
| B2 | `Argumentum/_RulesExplorer_*` | Rules list + detail | renders | same |
| B3 | `Argumentum/_Album List.cshtml` | gallery page | renders | same |
| B4 | `News5/bs3/_Details.cshtml` | a News5 detail page | renders, `CmsContext.Page.Parameters` resolves | `Request.QueryString` NRE (RazorComponent API missed in migration) |
| B5 | `News5/bs3/_List.cshtml` + `_List archive` + `_List Columns*` | News5 list pages | render | `IContainer`/`IModule` compile error |
| B6 | `Content/bs3/Link/_List of *.cshtml` (6) + `_Large emphasized link` | Content Link views | render, `CmsContext.Module.Id` resolves | `Dnn.Module.ModuleID` NRE (legacy API missed) |
| B7 | `Content/bs3/Layout/_Line.cshtml` | layout partial | renders | — |

**Red on B4-B7** = a RazorComponent→Razor14 API migration was incomplete in #596; capture the 2sxc error, fix the template, redeploy. (These were source-level migrated; runtime binding is what this smoke validates.)

## C. Social auth connectors (#597) + eshop path

> The 4 DNN-native auth connectors (Facebook, Google, LiveConnect=Microsoft, Twitter) are at v9.11.1.0 distributed binaries (per #597). They upgrade **with the DNN core package**. Validate each OAuth flow still initiates.

| # | Connector | Check | Green | Red flag |
|---|-----------|-------|-------|----------|
| C1 | Facebook | click "Login with Facebook" | redirects to FB OAuth consent | redirect-loop / 401 (client secret / callback URL) |
| C2 | Google | "Login with Google" | redirects to Google OAuth | same |
| C3 | LiveConnect (Microsoft) | "Login with Microsoft" | redirects to MS OAuth | API deprecation (legacy Live API) |
| C4 | Twitter | "Login with Twitter" | redirects to Twitter OAuth | same |
| C5 | OpenStore admin | `/DesktopModules/NBright/...` admin | loads on .NET 4.8 (no `DnnJsInclude` crash) | IIS crash = 2sxc cliff (#132 §5.5) |
| C6 | Stripe checkout path | add item → checkout | OS_Stripe + Stripe.net path loads | RazorEngine/CVE surface (pre-#445 removal) |

> **#597 caveat**: these connectors have known community-reported fragility ("do not work in DNN CE") — a red here may be pre-existing misconfig, **not** a regression from the 10.3.2 upgrade. Compare against the **pre-upgrade** baseline (snapshot in #132 Phase 0) to distinguish regression from pre-existing.

## Sign-off gate

```
[ ] Section A: all green (A1-A6)
[ ] Section B: all 12 templates render (B1-B7)
[ ] Section C: C5 (OpenStore) + C6 (Stripe) green; C1-C4 either green OR confirmed pre-existing (vs Phase-0 baseline)
[ ] Rollback anchor (#132 Phase-5 re-backup) verified: RESTORE VERIFYONLY green
→ If all green: exit maintenance mode. Record versions (DNN 10.3.2, 2sxc 21.07, OpenStore) in deploy log.
→ If any RED that is a regression: do NOT exit maintenance → execute #132 §6 rollback.
```

## Runtime validation note (#596 un-gate)

Sections B4-B7 are the **same checks** that un-gate the #596 merge when run on the **sandbox** (see [sandbox-bootstrap-runbook.md](sandbox-bootstrap-runbook.md) §5). The sandbox run produces the OK/KO report on #596; this go-live run confirms them on production post-upgrade. po-2023 signals factual OK/KO; the PASS verdict is ai-01/jsboige's.
