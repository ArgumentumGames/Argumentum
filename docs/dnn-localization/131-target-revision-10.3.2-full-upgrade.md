# #131 — Target Revision: 10.1.2 → 10.3.2 FULL upgrade (+ 2sxc 15→≥21)

**Issue:** [#131 — DNN platform + déploiement](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-18
**Base:** master `cae93dc8`
**Status:** DECISION-SPEC (docs, non-gated). **Supersedes the 10.1.2 target** of [#520](131-cve-correction-and-target-refinement.md)
/ [#522](131-step1-sandbox-upgrade-runbook.md) / [#527](132-deployment-runbook.md) / [#528 errata](131-upgrade-sandbox-plan-v2.md),
per jsboige interactive decision #2 (issue [#458](https://github.com/ArgumentumGames/Argumentum/issues/458) comment
2026-06-18). **Execution stays GATED** — this revises the *target*, it does not deploy.

> **jsboige decision #2 (VÉRIFIÉ, canal interactif):** *"Je suis pour l'upgrade complète, même si c'est plus
> lourde."* → target = **10.3.2-latest + 2sxc 15→≥21 + template audit**. This crosses the 2sxc compatibility
> cliff at 10.2.0 (#520 §3) that the 10.1.2 target deliberately avoided. **#445 (Stripe Native) is unblocked**
> by this decision (the OpenStore/.NET-8 blocker is dissolved — DNN 10 = .NET Framework 4.8, #514).

---

## 1. What changed (why the 10.1.2 runbooks are now superseded)

The earlier target refinement (#520) chose **10.1.2** specifically to *avoid* the 2sxc compatibility cliff:
10.1.2 closes both CVEs while staying pre-10.2.0, so 2sxc 15.02 and the Argumentum templates need no rework.
jsboige has now overridden that trade-off — accepting the heavier path to reach **10.3.2** (latest), which
*does* cross the cliff and *requires* the 2sxc upgrade + template audit. The motivation: latest platform +
modernized 2sxc + resolved Stripe decision, accepting the extra work.

**The 10.1.2 runbooks are not wrong, they are superseded for the chosen target.** Their procedure (backup →
staging → restore → schema analysis → migration plan → sandbox test → go-live → rollback), the rollback
contract, and the runtime finding (DNN 10 = .NET Framework 4.8, #514) all remain valid. Only the *target
version* and the *2sxc/template scope* change.

| Doc | Status after this revision |
|-----|----------------------------|
| [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) | ⚠️ "Recommended target = 10.1.2" superseded; CVE facts (64095→10.1.1, 52488→10.0.1, 9.13.x closes 0) still correct |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) (#522) | ⚠️ Target 10.1.2 superseded; sandbox procedure reusable for 10.3.2 path |
| [132-deployment-runbook.md](132-deployment-runbook.md) (#527) | ⚠️ Target 10.1.2 superseded (§4 "no 2sxc migration" NO LONGER HOLDS for 10.3.2); go-live procedure + rollback contract reusable |
| [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511) | Already under errata (#528); 2sxc-compat matrix §5-6 now *central* (was a caveat) |
| **This doc** | 🆕 net-new decision-spec: target 10.3.2, 2sxc upgrade path, template-audit finding |

## 2. The new target — 10.3.2 + 2sxc ≥21 (what crosses the cliff)

### 2.1 Platform: 9.11.1.19 → 10.3.2

| Step | Version | CVEs closed | 2sxc cliff? | Notes |
|------|---------|-------------|-------------|-------|
| (optional stepping-stone) | 9.13.9 | 0 | safe (pre-10.x) | migration aid only |
| 1 | 10.0.1 | 1 (CVE-2025-52488) | safe | first 10.x |
| 2 | 10.1.1 | +1 (CVE-2025-64095) | safe | both CVEs closed here |
| **3 (new target)** | **10.3.2 (latest)** | **2** | **⚠️ crossed** | jsboige decision #2 |

The DNN upgrade wizard handles the platform schema migration 9.11.1 → 10.3.2 (the 6-step strategy of
[#132](132-deployment-runbook.md) Phase 2-4 applies unchanged). The cliff consequence is a **2sxc** problem,
not a DNN-schema problem.

### 2.2 2sxc: 15.02 → ≥21.07 LTS (the cliff work)

Per #511 §6 + #520 §3, the cliff is a **Client Dependency Management change in DNN 10.2.0+** that breaks
2sxc 15.02's asset loading (`DnnJsInclude` → IIS crash). Resolution:

1. **Upgrade 2sxc 15.02 → 21.07 LTS** (min DNN 9.11.02 — our 9.11.1 is sufficient; LTS line runs on
   DNN 10.3.2, ships the `DnnJsInclude` workaround from 2sxc 21.00.02).
2. **2sxc v20 breaking changes** (the "Moment-of-Truth" flagged in #511 §6): `SexyContentWebPage`
   deprecation, module-path rename, SQL reorg. **See §3 — the Argumentum templates are already on the
   surviving path.**

## 3. 🔑 Grounded finding: the template audit is LARGELY ALREADY DONE

The body of issue #131 flags `_FallacyExplorer_Root.cshtml` as the migration risk: *"using deprecated
`@inherits ToSic.Sxc.Dnn.RazorComponent` → migrate to `Custom.Hybrid.Razor14`."* **That migration is
already complete in the repo.** Verified against the source (not asserted):

```
$ grep -rhoE '@inherits [A-Za-z0-9._]+' DNNPlatform/Portals/1/2sxc/Argumentum/*.cshtml | sort | uniq -c
      4 @inherits Custom.Hybrid.Razor14
```

All **4** custom Argumentum templates (`_FallacyExplorer_Root`, `_RulesExplorer_RuleDetail`,
`_RulesExplorer_RuleList`, `_Album List`) already inherit `Custom.Hybrid.Razor14` — the modern base that
**survives 2sxc 21**. None use the deprecated `ToSic.Sxc.Dnn.RazorComponent`. The APIs they call are all in
the *maintained-in-2sxc-21* list from issue #131 itself:

| API | Occurrences in Argumentum templates | Status in 2sxc 21 |
|-----|-------------------------------------|-------------------|
| `App.Query` | 1 | ✅ maintained |
| `AsList` | 4 | ✅ maintained |
| `CmsContext` | 4 | ✅ maintained |
| `Link.To` | 3 | ✅ maintained |
| `Edit.TagToolbar` | 5 | ✅ maintained |

**Implication:** the template-audit work that made the 10.3.2 target "heavier" is **substantially
pre-completed**. The residual 2sxc upgrade risk is the v20 **infra** breaking changes
(`SexyContentWebPage` auto-`web.config` deprecation, module-path rename, SQL reorg) on the **stock 2sxc
apps** (the other 25 of 26 — Accordion4, Blog5, Glossary3, etc.), not the custom Argumentum app.

> §3 honesty boundary: this is verified against the *repo* templates. The 2sxc v20 infra changes touch
> **installed-app data and web.config**, which live in the portal DB / runtime, not the repo (#525). The
> §3 finding covers the **Argumentum custom templates** (repo-grounded); the stock-app infra risk is a
> **staging-test** item (Phase 4 of #132), not a repo-audit item.

## 4. Revised upgrade procedure (delta vs the 10.1.2 runbooks)

On top of the 6-phase go-live procedure in [#132](132-deployment-runbook.md), the 10.3.2 target adds:

- **Phase 1.5 (NEW) — 2sxc upgrade on the current DNN.** Before crossing 10.2.0, upgrade 2sxc 15.02 →
  21.07 LTS **on DNN 9.11.1** (the 2sxc-first path from #131 research findings). Verify the Argumentum
  app still serves (it's already Razor14 — §3). This isolates 2sxc risk from DNN risk.
- **Phase 4 (EXPANDED) — sandbox test must cover the cliff.** The staging test (restored prod DB) must
  confirm: (a) no `DnnJsInclude` IIS crash after 10.2.0, (b) the 25 stock 2sxc apps still load, (c) the
  Argumentum app templates bind. **Go/no-go gate → jsboige** before prod.
- **Phase 3 (EXPANDED) — migration plan includes 2sxc content-types/entities/metadata.** Unlike the 10.1.2
  no-op (old #527 §4), 10.3.2 crosses the 2sxc v20 boundary → the 2sxc content migration is a real step
  (run by the 2sxc 21 upgrade wizard on the staging-restored prod DB first).
- **#445 (Stripe Native) unblocked.** With OpenStore's .NET-8 blocker dissolved (#514), jsboige can
  re-decide keep-OpenStore vs migrate-to-Stripe-managed-products. This is a **separate decision** (Phase B
  of #131), not blocking the platform upgrade — but the 10.3.2 upgrade window is the natural moment to
  act on it.

## 5. Gate boundaries

- ❌ Does **not** deploy, upgrade, back up, or restore anything on production (execution GATED jsboige).
- ❌ Does **not** touch the live DB / RDP / portal / 2sxc runtime (all steps *described*, not *performed*).
- ❌ Does **not** rewrite the merged 10.1.2 runbooks — adds superseded banners pointing here (separate edits).
- ❌ Does **not** decide #445 (Stripe) — surfaces it as unblocked, jsboige decides.
- ❌ Does **not** declare a QA verdict (ai-01 only).

## 6. What this does NOT cover (still gated / separate)

- The **stock 2sxc apps' v20 infra risk** (25 of 26 apps) — staging-test item, not repo-audit.
- The **#445 Stripe decision** content (keep OpenStore vs Stripe managed) — jsboige business decision.
- The **live DB export** needed to fill the #132 runbook's "à confirmer" sections — jsboige portal export.
- The **"Materiel" typo** fix — DB/2sxc content (#525), gated to the DNN upgrade session (jsboige decision #7).

## Sources

- jsboige decision #2 (issue #458 comment 2026-06-18, VÉRIFIÉ canal interactif) — target 10.3.2 + 2sxc upgrade + #445 unblocked.
- Repo templates: `DNNPlatform/Portals/1/2sxc/Argumentum/*.cshtml` (4 files, all `@inherits Custom.Hybrid.Razor14`).
- [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) (#520) — CVE facts + cliff @10.2.0.
- [131-step0-runtime-verification.md](131-step0-runtime-verification.md) (#514) — DNN 10 = .NET Framework 4.8.
- [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) (#511 §6) — 2sxc v20 breaking changes inventory.
- [132-deployment-runbook.md](132-deployment-runbook.md) (#527) — 6-phase go-live procedure + rollback contract.
- Issue #131 body (topology, 2sxc APIs maintained in v21, eshop scope).
