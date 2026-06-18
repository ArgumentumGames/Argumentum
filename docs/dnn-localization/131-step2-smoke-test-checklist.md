# #131 — Step 2 Consolidated Smoke-Test Checklist (post-upgrade sandbox gate)

**Issue:** [#131 — DNN platform upgrade (sandbox → prod)](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-18
**Base:** master `fc2013fc`
**Status:** **DOC / non-gated.** The consolidated, tickable smoke-test gate to run **after** the Step-1
sandbox upgrade (9.11.1.19 → **10.3.2**), **before** any production go-live. Pulls the smoke items scattered
across the runbook arc into one go/no-go list, and **adds the cliff-specific checks** that the 10.1.2-era
scenarios did not need. Execution stays GATED — this is the checklist; running it on prod is jsboige's call.

> **Target = 10.3.2 (decided).** jsboige interactive decision #2 (issue #458, 2026-06-18) chose the full
> upgrade → the sandbox crosses the 2sxc compatibility cliff at **10.2.0** (#520 §3 / [revision
> doc](131-target-revision-10.3.2-full-upgrade.md) §2.2). That makes two smoke families **mandatory** that a
> 10.1.2 stop would not have required: the **`DnnJsInclude` IIS-crash check** (§3) and the **25 stock 2sxc
> apps load** check (§4). A 10.1.2 stop could skip both; on 10.3.2 they are the gate.

---

## 1. When to run this checklist

Run on the **sandbox** (IIS Express `:8090`, or staging), **after** the Step-1 upgrade wizard completes
([Step-1 runbook #522](131-step1-sandbox-upgrade-runbook.md) §4), on the **prod-restored data** (not the empty
sandbox — per [#132](132-deployment-runbook.md) Phase 4). Every box must be ticked green before the go/no-go
gate to prod (Phase 5). **Visual verdict = ai-01 + jsboige** — the worker signals, does not declare PASS.

## 2. Pre-smoke (confirm the upgrade landed)

- [ ] `web.config` `targetFramework="4.8"` (unchanged — DNN 10.3.2 stays .NET Framework 4.8, #514).
- [ ] `DotNetNuke.dll` FileVersion = **10.3.2.x** (wizard applied the upgrade).
- [ ] Upgrade-wizard log: no `FATAL` / no skipped-extensions errors (Step-1 runbook §4 step 6).
- [ ] Site boots to the homepage (no yellow-screen / no 500).

## 3. 🆕 Cliff check — no `DnnJsInclude` IIS worker-process crash (10.3.2-specific)

This is the single most important check on the 10.3.2 path. The cliff (#520 §3, issue #6902): starting in
**DNN 10.2.0**, a `ClientDependency` change crashes 2sxc when it creates a `DnnJsInclude` — a full
**`unhandled win32 exception` / IIS worker-process recycle**. 2sxc ≥21.00.02 ships a code-side workaround
([revision doc](131-target-revision-10.3.2-full-upgrade.md) §2.2 step 1 — the 15.02→21.07 upgrade is what
carries it).

- [ ] **2sxc upgraded to ≥ 21.07 LTS** (the workaround-bearing line) — confirm in 2sxc admin → System Info.
- [ ] Open any **2sxc-rendered page** (FallacyExplorer, RulesExplorer, Album List) → page renders, **no IIS
      worker-process recycle** in the Windows event log (`WAS` / `IIS Express` crash entries) around the hit.
- [ ] Open a 2sxc **App-level admin** screen (uses `DnnJsInclude` for asset loading) → loads without crash.
- [ ] **NO-GO + escalate** if a `DnnJsInclude`-driven win32 crash appears — means the 2sxc upgrade did not
      land (still 15.02) or the workaround is insufficient. Do not proceed past this point.

## 4. 🆕 Stock 2sxc apps load (25 of 26 — the v20 infra risk)

The custom **Argumentum** app is the 1 of 26 and is pre-cleared (§5). The other **25 stock apps** (Accordion4,
Blog5, News5, Glossary3, Faq4, Gallery7, etc. — full list in [#525 inventory](457-site-content-type-inventory.md))
face the 2sxc **v20 "Moment-of-Truth" infra** changes: `SexyContentWebPage` auto-`web.config` deprecation,
module-path rename, SQL reorg ([v2 plan #511](131-upgrade-sandbox-plan-v2.md) §6). These live in the portal
DB / runtime, **not the repo** (#525), so they are a **staging-test** item, not a repo-audit item.

- [ ] In 2sxc admin, each installed stock app **appears** and its default view **renders** (spot-check ≥ the
      dispatch-named 4 + Content: Glossary3, News5, Blog5, Content).
- [ ] No `SexyContentWebPage`-deprecation white-screen on any stock-app view.
- [ ] **Glossary3** specifically renders (installed version = 14.09.00, pre-typed-mode — highest stock risk;
      [#511 §5](131-upgrade-sandbox-plan-v2.md) flags it for a typed-v3 bump).
- [ ] Asset links resolve on the **new** module path `/DesktopModules/ToSic.Sxc/` (old `ToSic_SexyContent`
      path returns 404 for any directly-linked `$2sxc.min.js` / `Thumbnailer.aspx`).

## 5. Argumentum custom app (1 of 26 — pre-cleared, verify only)

The grounded finding ([revision doc](131-target-revision-10.3.2-full-upgrade.md) §3): all 4 bespoke templates
already `@inherits Custom.Hybrid.Razor14` (verified by grep, not asserted) — the `RazorComponent`→`Razor14`
migration #131 flagged is **already complete**. APIs in use (`App.Query`, `AsList`, `CmsContext`, `Link.To`,
`Edit.TagToolbar`) are all maintained in 2sxc 21. So this section is **verify-still-binds**, not migrate.

- [ ] **`_FallacyExplorer_Root.cshtml`** — FallacyExplorer page renders, fallacy list loads, detail view binds.
- [ ] **`_RulesExplorer_RuleList.cshtml`** — Rules index renders, `ui.players_range` resolves.
- [ ] **`_RulesExplorer_RuleDetail.cshtml`** — a rule detail renders; `res.*` headings (RuleSummary / Material /
      Installation / Variants / MemoCard / MemoInstructions) resolve from 2sxc App Resources.
- [ ] **`_Album List.cshtml`** — Album List page renders.
- [ ] No `RazorComponent`-deprecation / `GetBestValue`-removal error in any of the 4 (would mean a template
      regressed off `Razor14` — contradicts the §3 finding).

## 6. DNN platform surface (carry-over from #522 / #527)

- [ ] Homepage renders.
- [ ] Language switcher works — **all 8 languages** cycle (FR + en-US baseline + the 6 non-Latin: ru, pt, es,
      ar, fa, zh). For ar/fa/zh, validate via the signatures in
      [`release-validation/non-latin-verification-guide.md`](release-validation/non-latin-verification-guide.md)
      (jsboige does not read ar/fa/zh).
- [ ] Host → Extensions: installed-modules diff vs the Step-1 §3 pre-flight inventory — no unexpected removals.
- [ ] Admin/PersonaBar loads (framework surface — separate from the #490 app strings).

## 7. Commerce stack (if retained — Phase B decision pending)

OpenStore/NBrightBuy 4.1.11 + `OS_Stripe.dll` + `Stripe.net.dll` (repo `DNNPlatform/bin/`,
`DesktopModules/NBright/OS_Stripe`). The .NET-8 blocker is dissolved (#514 — DNN 10 is .NET Framework 4.8), so
the commerce stack **should** load; this confirms it. The keep-OpenStore-vs-Stripe-Native decision (#445) is
jsboige's separate Phase B call.

- [ ] OpenStore module loads on a catalog/checkout page (no bin-load failure).
- [ ] Stripe checkout path initializes (`OS_Stripe.dll` loads — no `DnnJsInclude` crash here either).
- [ ] **NO-GO + escalate** if the commerce stack fails to load — flags the #445 decision as blocking before
      prod (it is currently unblocked but not decided).

## 8. Rollback readiness (mandatory before declaring Step 2 green)

- [ ] A tested restore path exists to **9.11.1.19 + 2sxc 15.02** from the Step-1 §3 pre-flight backup
      (sandbox DB + filesystem + 2sxc app export).
- [ ] Restore was **verified** (not just taken) — `RESTORE VERIFYONLY` on the DB, file-count check on the
      filesystem copy, 2sxc import dry-run on the app export.

## 9. Go/no-go gate → jsboige

| Outcome | Action |
|---------|--------|
| **All green** | Worker signals READY; **ai-01 + jsboige** declare PASS → eligible to move to [#132](132-deployment-runbook.md) Phase 5 (prod go-live). |
| **Any §3 (cliff) red** | NO-GO. The 2sxc upgrade did not land correctly. Roll back sandbox, re-do the 15.02→21.07 upgrade ([revision doc](131-target-revision-10.3.2-full-upgrade.md) §2.2), re-run. |
| **§4 stock-app red** | NO-GO. Identify the failing app; bump (Glossary3 → typed v3 first) or pin; re-run. |
| **§5 Argumentum red** | NO-GO + investigate — would contradict the §3 repo finding (templates are Razor14). Most likely a 2sxc App Resources data issue, not a template issue. |
| **§7 commerce red** | Escalate to jsboige — #445 becomes a blocking decision before prod. |

## 10. Gate boundaries (this document)

- ✅ Consolidates the scattered smoke items (#522 §5, #527 Phase 4/5, revision §4) into one tickable gate.
- ✅ Adds the **cliff-specific checks** (§3 `DnnJsInclude`, §4 stock apps) that only matter on the 10.3.2 path.
- ✅ Encodes the grounded §5 finding (templates pre-cleared) so the checker knows the expected result.
- ❌ Does **not** execute the checklist — running it on prod is GATED jsboige; on sandbox it follows Step-1.
- ❌ Does **not** decide #445 (Stripe) — only flags it as blocking if §7 fails.
- ❌ Does **not** declare a QA verdict (ai-01 + jsboige only).

## Sources

- [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) (#531) — target 10.3.2, §3 templates pre-cleared (Razor14), §2.2 2sxc upgrade path, §4 cliff smoke families.
- [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) — §3 the cliff at 10.2.0 (issue #6902), `DnnJsInclude` win32 crash symptom.
- [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) (#522) — §3 pre-flight inventory, §4 upgrade mechanics, §5 risk register, smoke "4 Argumentum pages render" gate.
- [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) — §5 2sxc app matrix (Glossary3 v14 risk), §6 v20 "Moment-of-Truth" breaking changes.
- [132-deployment-runbook.md](132-deployment-runbook.md) (#527) — Phase 4 sandbox-on-prod-data, Phase 5 prod smoke items.
- [457-site-content-type-inventory.md](457-site-content-type-inventory.md) (#525) — the 26 apps (1 custom + 25 stock), stock apps live in portal DB not repo.
- [release-validation/non-latin-verification-guide.md](release-validation/non-latin-verification-guide.md) — ar/fa/zh visual signatures for the §6 language-switcher check.
- Repo: `DNNPlatform/Portals/1/2sxc/Argumentum/*.cshtml` (4 files, all `@inherits Custom.Hybrid.Razor14`), `DNNPlatform/bin/` (OS_Stripe, Stripe.net), `DNNPlatform/DesktopModules/NBright/OS_Stripe`.

---

*Worker smoke-test checklist (doc/non-gated). The worker ticks the boxes and signals READY/NO-GO; the visual
PASS verdict is ai-01 + jsboige. No production system touched.*
