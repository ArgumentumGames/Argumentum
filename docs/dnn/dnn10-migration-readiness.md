# DNN 10 Migration — Code Readiness Assessment (read-only, consolidated)

> **Purpose**: consolidate the read-only findings (ticks 29-31) into a **durable** document that survives dashboard condensation. Establishes that the **code-side migration surface for DNN 10.3.2 is low-risk**; the dominant unknowns (Telerik removal, deprecated APIs) are cleared.
> **Scope**: repo-runtime state (branch `dnn/sandbox-runtime-1032`, commit `4b0297ee`, DNN 10.3.2.0 + 2sxc 21.07.00, .NET Framework 4.8). NOT public prod (`dnn.argumentum.myia.io` = HTTP 500 skin `tabid=138`, version-independent).
> **Author**: po-2023 (dispatch ai-01 `msg-20260722T224852-6cz88c`, secondaire).
> **Cross-ref**: #131, `skin-tabid138-diagnostic-runbook.md` (PR #851 merged), `RUNBOOKS-GATED-OPS.md`, `UPGRADE-ASSESSMENT.md`.

---

## 1. Versions (measured, DLL-level)

| Component | Installed (repo-runtime) | Target | Method |
|-----------|--------------------------|--------|--------|
| DNN Platform | **10.3.2.0** ✅ | 10.3.2 | `DotNetNuke.dll` FileVersion |
| 2sxc | **21.07.00** ✅ | 21.07 LTS | `ToSic.Sxc.*.dll` |
| .NET runtime | Framework 4.8 | 4.8 | verified (not .NET 8/9) |
| OpenStore (NBrightBuy) | 4.1.11.0 | 4.2.4 or replace | `NBrightBuy.dll` FileVersion |

> `web.config` `InstallVersion=09.06.02` is a **legacy initial-install appSetting**, NOT the runtime version. See #131 body refresh (PR comment, 2026-07-22).

---

## 2. Telerik removal — migration surface = ZERO ✅

DNN 10 removes Telerik. Historically the dominant pain-point of DNN upgrades. For this codebase it is **already done**:

- **DLL-level**: `0/206` DLLs in `bin/` reference Telerik (`Assembly.GetReferencedAssemblies()` scan). `Telerik.Web.UI.dll` is **absent** from `bin/`. Only `Dnn.Modules.TelerikRemoval.dll` (the removal shim) is present.
- **OpenStore / NBrightBuy**: **Telerik-free** (6 DLLs checked: NBrightBuy 4.1.11, NBrightCore/NBrightDNN 8.7.2, OS_Stripe/OS_Reports/OS_Chronopost2 — 0 Telerik AssemblyRefs). Comment on #131 (`issuecomment-5049252377`).
- **Skin markup**: `0` `.ascx` files reference Telerik (3 CSS files in `_default/WebControlSkin/` are cosmetic styles for Telerik controls — never loaded without a Telerik control).

**Conclusion**: Telerik is not a blocker for OpenStore or any repo module/skin on DNN 10.

---

## 3. Skin objects — DNN 10 standard, compatible ✅

- **26 `.ascx`** across all skins (Xcillion, nvQuickTheme, DnnContra, DnnBootsterV2, Bootstrap 4 Instant, 2shineBS5) use **standard DNN skin objects** (`dnn:BREADCRUMB` / `dnn:MENU` / `dnn:LOGIN` / `dnn:SEARCH` / `dnn:USER` / etc.) — core controls, compatible with DNN 10.
- **6 Razor skins** (DnnContra, DnnBootsterV2, Bootstrap 4 Instant, 2shineBS5) use `TabInfo` + `PortalSettings.ActiveTab` — **stable APIs, not deprecated** in DNN 10.

---

## 4. OpenStore secondary refs (documented, non-blocking)

From `NBrightBuy.dll` AssemblyRefs:
- `→ DotNetNuke 9.7.1.0` (runtime is 10.3.2.0 — handled by standard DNN binding redirects).
- `→ RazorEngine 3.10.0.0` (legacy Razor engine; OpenStore ships its own — watch-point for the 2sxc Razor14 migration #596 interaction, separate concern).

---

## 5. Residual open items (gated, not code-blocking)

| Item | Status | Gated by |
|------|--------|----------|
| **Runtime smoke-test** on DNN 10.3.2 sandbox | not yet re-verified live | sandbox blocked by HTTP 500 skin `tabid=138` (≠ platform version); smoke at GO jsboige |
| **Skin `tabid=138` HTTP 500** (`lblBreadCrumb` HttpParseException) | diagnostic runbook ready | Opt 1 (fix v0.9.1, tag not blocked) — `skin-tabid138-diagnostic-runbook.md` |
| **OpenStore runtime re-verify** (Phase A checkbox) | gated | same sandbox block; risk Telerik cleared → smoke-test, not porting effort |
| **machineKey rotation** | runbook ready | jsboige ops server (HIGH exposure) — `machinekey-rotation-scrub-runbook.md` |
| **2sxc App export #681** | hard-unblocker DNN i18n | jsboige (content lives in live 2sxc DB) |

---

## 6. Bottom line

The **code-side** migration to DNN 10.3.2 is **low-risk and essentially complete** in the repo-runtime state:
- Telerik surface = 0 (the usual DNN 10 blocker) ✅
- Skin objects + Razor APIs = DNN 10 standard ✅
- OpenStore = Telerik-free ✅

The remaining work is **runtime/ops** (smoke-test at GO, skin `tabid=138` fix, machineKey rotation), not code porting. None of it blocks the v0.9.0 tag (Opt 1: skin fix deferred to v0.9.1).
