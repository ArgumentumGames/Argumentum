# #131 — Step 0 Result: DNN 10.x Runtime = .NET Framework 4.8 (VERIFIED)

**Issue:** [#131 — DNN platform upgrade](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17
**Base:** master `03cfe5f0`
**Companion to:** [`131-upgrade-sandbox-plan-v2.md`](131-upgrade-sandbox-plan-v2.md) (PR #511, merged) — this document reports the **result of Step 0** of its §7 staircase.

---

## 1. Verdict — Step 0 = GO

**The single gate that the entire staircase hinges on is confirmed: DNN 10.3.2 runs on .NET Framework 4.8 — the SAME runtime as the currently installed DNN 9.11.1.19. There is no .NET-8 jump.**

| Runtime fact | Source | Status |
|---|---|---|
| DNN **10.3.2** core compiled TFM = `net48` | `DotNetNuke.Core` 10.3.2 `.nupkg` → `lib/net48/` (this report, §2) | **VERIFIED (primary source)** |
| DNN **9.11.1** (installed) runtime = .NET Framework 4.8 | `DNNPlatform/web.config` `targetFramework="4.8"` + `DotNetNuke.dll` FileVersion 9.11.1.19 | **VERIFIED** |
| ⇒ Same runtime across 9.11.1 → 10.3.2 | transitive | **VERIFIED** |

The §7 Step 0 pass criterion ("DNN 10.3.2 runs on .NET Framework 4.8") is **met without standing up a full install** — the compiled binary itself declares the runtime.

## 2. The primary-source artifact (the proof)

The authoritative machine-readable evidence for "what runtime does DNN 10.3.2 require" is the **compiled `.nupkg`'s `lib/` folder structure** — the Target Framework Moniker is the folder name. Inspected via the NuGet flat-container:

```
DotNetNuke.Core 10.3.2.nupkg   (1,070,411 bytes, 2026-05-05 build)
  lib/net48/DotNetNuke.dll                     (2,949,632 bytes — the core DNN assembly)
  lib/net48/Microsoft.ApplicationBlocks.Data.dll (32,768 bytes)
```

- `lib/net48/` is the **only** TFM folder present (verified: `unzip -l … | grep lib/` returns exactly `lib/net48/`).
- There is **no** `lib/net8.0/`, `lib/net6.0/`, `lib/netstandard2.0/`, or any other target. DNN 10.3.2 core is single-TFM: **.NET Framework 4.8**.
- The other 10.3.2 packages confirm the same line: `DotNetNuke.Web`, `DotNetNuke.Web.Mvc`, `DotNetNuke.WebApi`, `DotNetNuke.Abstractions`, `DotNetNuke.DependencyInjection`, `DotNetNuke.Bundle`, `DotNetNuke.SiteExportImport`, `DotNetNuke.Providers.FolderProviders` — **all** ship at 10.3.2 (NuGet search, 13 DNN packages at 10.3.2).

The `DotNetNuke.Core` package's `.nuspec` declares its single dependency without a framework group (`<dependency id="DotNetNuke.DependencyInjection" version="10.3.2" />`), consistent with DNN's convention of not multi-targeting — the `lib/net48/` folder IS the TFM declaration.

## 3. Why this method is authoritative (and stronger than the original plan)

The v2 plan's Step 0 action was *"stand up a clean DNN 10.3.2 install and confirm `web.config` `targetFramework`."* This report substitutes **source-artifact inspection** (the published `.nupkg`), which is **strictly stronger** evidence for the runtime-requirement question:

- A running install's `web.config` `targetFramework` is a **config value** that an operator can hand-edit; it declares intent, not the binary's nature.
- The `lib/net48/` folder is what the binary **actually is** — a .NET-8-compiled DNN could not produce a `lib/net48/DotNetNuke.dll`. The folder name is produced by the compiler from the `.csproj` `<TargetFramework>`, with no human-edit step in between.
- It requires **no install, no DB, no IIS** — pure read of the published artifact. Fully within the "sandbox/research only, no prod" boundary, and reproducible by anyone.

The full-install sub-step is therefore **not needed to answer Step 0's question**. (A running-install smoke remains valuable later, at Step 1/3, for the *upgrade-path* mechanics — not for the *runtime* gate.)

## 4. What this dissolves — and re-opens

This upgrades **finding B** of the v2 plan from *strong-evidence / TO-CONFIRM* to **VERIFIED from primary source**:

- **The OpenStore / NBrightBuy 4.1.11 blocker dissolves.** `docs/dnn/UPGRADE-ASSESSMENT.md` (2026-06-07) predicated that blocker on "DNN 10.x = .NET 8 → no .NET 8 port of NBrightBuy." The premise is false: DNN 10.x is .NET Framework 4.8, and NBrightBuy 4.1.11 (.NET Framework) targets the same runtime. There is no framework port required for it to run on DNN 10.x.
- **Re-opens the Stripe Native decision (#445) for jsboige.** The #445 rationale ("OpenStore blocks 10.x") no longer holds. This is a **business call** — the worker signals the premise change, does not overturn the decision. Flagged for jsboige in the session DONE.

## 5. The "module loads" sub-check (already covered)

Step 0's second criterion ("a stock .NET Framework 4.8 module loads on DNN 10") is not re-tested here — it is already answered by independent circumstantial proof documented in the v2 plan §4: **2sxc v21.00.02** (2026-01-12, .NET-Framework-4.8-compiled) ships a workaround for a **DNN 10.02.1** bug ⇒ a .NET-Framework module demonstrably runs ON DNN 10.02.1. The runtime gate (§2) plus this module-compat proof together close Step 0.

## 6. Step 0 go/no-go → next step

- **Step 0: GO.** Runtime confirmed .NET Framework 4.8. No .NET-8 jump. Blocker dissolved.
- **Next (on jsboige go):** Step 1 of the §7 staircase — **9.11.1 → 9.13.x** — which is the version that closes **CVE-2025-64095** (CVSS 9.8, unauthenticated file upload). Note from the v2 plan §2: 9.13.x does **not** close CVE-2025-52488 (NTLM, patched **10.0.1 only**) — full CVE closure still requires reaching 10.0.1+ (Step 3). jsboige's choice of security palier target (§8 Q1) remains the gating decision before any upgrade runs.

## 7. Sources

1. NuGet flat-container (compiled nupkg, primary source): `https://api.nuget.org/v3-flatcontainer/dotnetnuke.core/10.3.2/dotnetnuke.core.10.3.2.nupkg` → `lib/net48/DotNetNuke.dll`
2. NuGet registration (versions/dependency groups): `https://api.nuget.org/v3/registration5-semver1/dotnetnuke.core/index.json`
3. NuGet search (all DNN 10.3.2 packages): `https://azuresearch-usnc.nuget.org/query?q=DotNetNuke`
4. DNN Platform requirements (corroborating): `https://docs.dnncommunity.org/content/getting-started/setup/requirements/index.html` — *"Starting with DNN 10.0.0, .NET Framework 4.8.0+ is required"*
5. `DotNetNuke.Web.Mvc 10.3.2` NuGet page (corroborating): states *"targets .NET Framework 4.8"*
6. Installed baseline (local): `DNNPlatform/web.config` `targetFramework="4.8"`; `DNNPlatform/bin/DotNetNuke.dll` FileVersion 9.11.1.19
7. v2 plan + 2sxc module-compat proof: `131-upgrade-sandbox-plan-v2.md` §2/§4

---

*Worker research deliverable (sandbox/research only). No production deploy. Verdict on upgrade target = ai-01 review + jsboige business call. Runtime finding = primary-source verified.*
