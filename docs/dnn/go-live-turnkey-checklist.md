# DNN 10.3.2 Go-Live — Turnkey Session Checklist (jsboige's RDP window)

**Date**: 2026-06-26 · **Author**: po-2023 (dispatched by ai-01, secondary track — release COUPLÉE au site, jsboige input 26/06)
**Status**: **Turnkey navigator.** Organizes the entire #131 arc by **what an RDP session is needed for**, not by technical phase (the [dnn-localization README](../dnn-localization/README.md) already does phase-order). The shortest path from "open RDP" to "site live on 10.3.2". Read-only doc, no code.
**Purpose**: jsboige coupled v0.9.0 to the DNN go-live (#131/#132). Most of the arc is already done by agents without touching the runtime; a bounded set **requires jsboige's interactive RDP/sandbox session**. This is the checklist that says — when you open that window, here is the exact turnkey sequence and nothing redundant.
**Related**: [sandbox-bootstrap-runbook.md](sandbox-bootstrap-runbook.md), [go-live-smoke-test.md](go-live-smoke-test.md), [../dnn-localization/README.md](../dnn-localization/README.md) (arc index), #596 (Razor14), #597 (auth), #131/#132.

---

## TL;DR — the RDP-time split

```
[A] DONE by agents (no RDP) ────── verify only (no runtime action)
[B] REQUIRES jsboige RDP session ─ the runtime work, turnkey sequence below
```

| Block | What | Needs RDP? | Status |
|-------|------|------------|--------|
| Templates Razor14 migration (#596) | 12 templates source-level migrated | ❌ no (repo) | ✅ done — runtime verify only (B3) |
| CVE + target docs (#593) | 9.13.x closes 0 CVE; target = 10.3.2 | ❌ no | ✅ done |
| Full doc arc + checklists | README index, sandbox smoke (#131-step2), prod smoke (#603), deployment (#132) | ❌ no | ✅ done |
| **Sandbox `bin/` repair** | 5 .NET-9 contaminants → net48 re-deploy | ✅ **RDP** | ⛔ characterized, recipe ready (B1) |
| Sandbox upgrade 9.11.1→10.3.2 + 2sxc 15.02→21.07 | wizard + cliff cross | ✅ **RDP** | ⛔ gated (B2) |
| Browser-verify 12 templates (#596 runtime un-gate) | assign + screenshot | ✅ **RDP** | ⛔ gated (B3) |
| Prod go-live 10.3.2 | wizard on prod + Phase-5 smoke | ✅ **RDP (prod)** | ⛔ gated (B4) |

## [A] Already done without RDP (agent-delivered — verify only, no action)

These landed via PRs while `master` stayed frozen at `bef3bc6c`:

- **#596** — 12 RazorComponent→Razor14 templates migrated at source level (`Portals/1/2sxc/News5/`, `Content/`). Runtime binding is what B3 verifies; the source migration is complete.
- **#593** (`131-cve-correction-and-target-refinement.md`) — CVE reconciliation: 9.13.x closes **0** of the 2 relevant CVEs; floor = 10.1.2; actée target = **10.3.2 + 2sxc 21**.
- **#597** (`506-social-auth-connectors-inventory.md`) — Facebook/Google/Microsoft/Twitter connector inventory + secret-rotation procedure. Repo-grounded; live console checks fold into B4.
- **#603** — [sandbox-bootstrap-runbook.md](sandbox-bootstrap-runbook.md) (the `bin/` repair recipe) + [go-live-smoke-test.md](go-live-smoke-test.md) (prod smoke checklist).
- **#132** (`132-deployment-runbook.md`, #594) — 6-phase production go-live + rollback contract. Retargeted to 10.3.2.
- **#131-step2-smoke-test-checklist.md** — consolidated, tickable sandbox smoke gate (9 sections, cliff-specific checks).
- **Arc navigation** — [../dnn-localization/README.md](../dnn-localization/README.md) sequences the 15-doc arc in execution order.

## [B] Requires jsboige's RDP session — turnkey sequence

> Run in order. Each step references the authoritative doc (don't re-derive here). The whole of [B] is one focused session if B1 goes smoothly.

### B1 — Sandbox `bin/` repair (FIRST — blocks ALL DNN boot) ⛔

This is the characterized blocker from the 2026-06-25 boot attempt ([#596 `issuecomment-4804068740`](https://github.com/ArgumentumGames/Argumentum/pull/596#issuecomment-4804068740)). The sandbox **cannot start** until the `bin/` SDK contamination is cleaned. **Recipe is turnkey**: [sandbox-bootstrap-runbook.md §3](sandbox-bootstrap-runbook.md).

1. Copy `System.Buffers`/`System.Memory` from `bin/Imageflow/` (clean 4.0.3.0/4.0.1.1).
2. Fetch the **5 .NET-9 contaminants** at **6.0.x** NuGet (`lib/net462`, last net48 line): `System.Collections.Immutable`, `System.Text.Json`, `System.IO.Pipelines`, `System.Diagnostics.DiagnosticSource`, `System.Text.Encodings.Web`.
3. Align binding redirects `newVersion` → 6.0.0.0.
4. Edit `web.config` locally (LocalDB conn string + throwaway machineKey — **never commit**, revert after).
5. Boot IIS Express `:8090` via `/config:` + named "DNN Argumentum" site. Expect HTTP 200 (not `0x80131040`).

### B2 — Sandbox upgrade + smoke gate

Per [../dnn-localization/131-2sxc-migration-plan.md](../dnn-localization/131-2sxc-migration-plan.md) (2sxc-first) then [../dnn-localization/131-step1-sandbox-upgrade-runbook.md](../dnn-localization/131-step1-sandbox-upgrade-runbook.md):

1. **2sxc 15.02 → 21.07 LTS** first (carries the `DnnJsInclude` cliff workaround — mandatory on the 10.3.2 path).
2. **DNN 9.11.1 → 10.3.2** upgrade wizard.
3. Run [../dnn-localization/131-step2-smoke-test-checklist.md](../dnn-localization/131-step2-smoke-test-checklist.md) — all green (esp. §3 `DnnJsInclude` crash, §4 stock 2sxc apps).

### B3 — Browser-verify 12 Razor14 templates (#596 runtime un-gate)

Per [sandbox-bootstrap-runbook.md §5](sandbox-bootstrap-runbook.md): assign each migrated template to a 2sxc App module, browser-navigate, screenshot. **OK/KO per template**. KO = capture the 2sxc yellow-screen, fix, redeploy. Red on B4-B7 of [go-live-smoke-test.md](go-live-smoke-test.md) = incomplete RazorComponent→Razor14 API migration.

> po-2023 signals the OK/KO report; the visual PASS verdict is ai-01/jsboige's.

### B4 — Production go-live

Per [../dnn-localization/132-deployment-runbook.md](../dnn-localization/132-deployment-runbook.md) Phase 5: prod wizard on prod-restored data → [go-live-smoke-test.md](go-live-smoke-test.md) (A platform core + B 12 templates + C auth/eshop) → sign-off gate → exit maintenance. Any regression-red → #132 §6 rollback.

## Reference map (which doc for what)

| You need | Open |
|----------|------|
| The `bin/` repair recipe | [sandbox-bootstrap-runbook.md](sandbox-bootstrap-runbook.md) §3 |
| Sandbox smoke gate (tickable) | [../dnn-localization/131-step2-smoke-test-checklist.md](../dnn-localization/131-step2-smoke-test-checklist.md) |
| Prod smoke (post-wizard) | [go-live-smoke-test.md](go-live-smoke-test.md) |
| Full prod go-live + rollback | [../dnn-localization/132-deployment-runbook.md](../dnn-localization/132-deployment-runbook.md) |
| The whole arc in phase order | [../dnn-localization/README.md](../dnn-localization/README.md) |
| Target decision + cliff rationale | [../dnn-localization/131-target-revision-10.3.2-full-upgrade.md](../dnn-localization/131-target-revision-10.3.2-full-upgrade.md) |
| Auth connectors (#597) | [../dnn-localization/506-social-auth-connectors-inventory.md](../dnn-localization/506-social-auth-connectors-inventory.md) |
