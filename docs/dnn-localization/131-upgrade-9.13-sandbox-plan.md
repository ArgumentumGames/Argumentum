# #131 — DNN 9.11.1 → 9.13.x Security Palier (sandbox plan)

**Status:** Plan (sandbox prep) — no prod touch, no tag, no release artifact
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-15
**Scope:** Security patch palier 9.11.1 → 9.13.x on the DNN export checked into this
repo. This is the **secondary** task of ai-01 dispatch `slfy5i` (2026-06-15 03:05). It
extends the worker's Phase A migration plan (2026-06-05) and builds on the security
hardenings already merged in **#442** (master `092bb8f3`).
**Gate:** Release v0.9.0 gate is **unaffected** — DNN migration is a separate, non-gated
track. The 10.x jump (eshop/OpenStore + go-live) is explicitly **out of scope** (gated by
jsboige). This plan is the intermediate security palier only.

---

## 1. Why a 9.13.x palier (and not 10.x, not stay on 9.11.1)

Two disclosed CVEs affect the current 9.11.1 install:

| CVE | CVSS | Description | Patched in |
|-----|------|-------------|------------|
| **CVE-2025-64095** | **9.8 (critical)** | Unauthenticated file upload / replace via the default HTML editor provider → website defacing + XSS payload injection. | **9.13.x** (and 10.0.x) |
| **CVE-2025-52488** | (med-high) | NTLM hash exposure to a third-party SMB server via crafted interaction. Affects 6.0.0 → before 10.0.1. | **10.0.1 only** — *NOT patched in 9.13.x* |

**Honest scoping note (important):** the 9.13.x palier patches the **critical 9.8**
(CVE-2025-64095) but does **NOT** patch CVE-2025-52488 (NTLM), which requires the 10.x
jump. CVE-2025-52488's practical risk is lower than the 9.8 (requires crafted SMB
interaction + an authenticated-ish vector), so the 9.13.x palier is a **legitimate
intermediate** that closes the critical hole without the gated 10.x commitment. The NTLM
exposure stays as a **documented residual risk** until the 10.x jump, mitigated by the
#442 hardenings (AES/HMACSHA256 machineKey, requireSSL cookies, CSP).

**Sources:**
- [ZeroPath — CVE-2025-64095 analysis](https://zeropath.com/blog/cve-2025-64095-dnn-unauthenticated-file-upload)
- [Tenable — CVE-2025-64095](https://www.tenable.com/cve/CVE-2025-64095)
- [akaoma — CVE-2025-52488](https://cve.akaoma.com/cve-2025-52488)

---

## 2. Current state (verified on master `092bb8f3`)

| Item | Value | Source |
|------|-------|--------|
| DNN runtime version | **9.11.1** | `DotNetNuke.dll` ProductVersion `9.11.1+Branch.master.Sha.d2f44d2…` |
| `web.config InstallVersion` | `09.06.02` | **Legacy sticker, NOT the runtime version** — never updated through past installs; do not trust it as the source of truth. The DLL version is authoritative. |
| .NET target | `4.8` | `web.config` `<httpRuntime targetFramework="4.8">` — DNN 9.13.x stays on .NET 4.8 ✅ (no .NET 8+ jump required for this palier) |
| 2sxc | (TBD vs CHANGELOG target 21.07) | CHANGELOG §Changed says "Upgraded 2sxc 15→21.07 with Razor14 migration + IRenderService fix (#418)". Verify the on-disk 2sxc DLL version is compatible with DNN 9.13.x before upgrade (2sxc 21.x supports DNN 9.13). |
| #442 hardenings already merged | machineKey `decryption=3DES→AES`, `validation=SHA1→HMACSHA256`; `httpCookies requireSSL=false→true`; `anonymousIdentification cookieRequireSSL=false→true`; CSP; `enableVersionHeader=false` | commit `092bb8f3` (PR #442) |
| Telerik removal module | `Dnn.Modules.TelerikRemoval.dll` present | DNN 9.x removed Telerik — the module is in place, supports the 9.13.x path |
| Upgrade wizard assets | `Install/UpgradeWizard.aspx.*` present (de/es/fr/it/nl) | Standard DNN in-place upgrade path is available |

---

## 3. Sandbox plan (prep only — no prod touch)

The palier must be validated in a **sandbox copy of prod** before any prod consideration.
jsboige holds prod access; this plan is what the worker prepares.

### 3.1 Sandbox setup (jsboige-owned step, worker documents)

1. **Copy prod** (≈602 MB DNN export + DB backup) to an isolated IIS site + isolated DB
   on a non-prod port (the worker's local IIS Express `/config:` pattern from
   `feedback-dnn-iis-express`, site `DNN Argumentum` id:2, can host the sandbox).
2. **Snapshot DB** before any upgrade step (rollback anchor).
3. **Confirm `.NET 4.8`** runtime on the sandbox app pool (9.13.x requirement — already
   met by the current `targetFramework`).

### 3.2 Upgrade procedure (standard DNN in-place)

1. **Backup `web.config`** (carries the #442 hardenings — must survive the upgrade; the
   DNN upgrader rewrites some sections, so a 3-way merge of #442 hardenings back in is the
   post-upgrade step).
2. **Stop the app pool**, drop the 9.13.x upgrade package (`Install/` overlay + `/Install/Upgrade.aspx` run).
3. **Run the Upgrade Wizard** (`Install/UpgradeWizard.aspx`) — browser-driven, applies
   SQL migrations + config transforms.
4. **Verify version**: `DotNetNuke.dll` ProductVersion → `9.13.x`; `web.config
   InstallVersion` sticker updates to `09.13.x` (this is the one place the sticker is
   meaningful — post-upgrade).
5. **Re-apply #442 hardenings** via 3-way diff vs the pre-upgrade `web.config` backup:
   - machineKey `AES` + `HMACSHA256` (upgrader may reset to defaults)
   - `httpCookies requireSSL=true`, `anonymousIdentification cookieRequireSSL=true`
   - CSP header, `enableVersionHeader=false`
6. **Verify 2sxc 21.x** still resolves Razor14 templates (`_FallacyExplorer_Root.cshtml`
   etc.) post-upgrade — this is the integration risk (Razor14 + IRenderService, #418).
7. **Regression smoke**: site boots, Argumentum app queries (`FallaciesFromCSV`) resolve,
   Rules/Glossary pages render.

### 3.3 Acceptance for the palier

- ✅ `DotNetNuke.dll` = 9.13.x
- ✅ #442 hardenings re-applied (diff-clean vs backup)
- ✅ 2sxc 21.x + Argumentum templates render without IRenderService errors
- ✅ HTML editor provider no longer allows unauthenticated upload (CVE-2025-64095
  closed) — manual verify: unauthenticated upload attempt rejected
- ⚠️ CVE-2025-52488 (NTLM) **remains open** — documented residual, deferred to 10.x jump

### 3.4 Rollback

DB snapshot restore + `web.config` backup restore + revert `bin/` overlay. Sandbox-only,
zero prod blast radius.

---

## 4. What this plan does NOT do (gated / out of scope)

- ❌ **10.x jump** (CVE-2025-52488 NTLM full fix, eshop/OpenStore, Stripe-native) — gated
  by jsboige go-live decision + #444 assessment.
- ❌ **Any prod touch** — sandbox only until jsboige sign-off.
- ❌ **2sxc upgrade** beyond verifying 21.x compatibility — 2sxc 15→21.07 already done
  (#418); this palier only confirms it survives the DNN 9.13.x bump.
- ❌ **Release v0.9.0 coupling** — DNN migration is a separate track; release gate intact.

---

## 5. Worker next step (disk-light, awaits jsboige sandbox access)

This document is the deliverable for the secondary dispatch. The worker cannot execute
§3.1 (sandbox copy) without jsboige prod access and the disque-headroom to host a sandbox
copy (currently C: = ~9 GB free / 100% — **blocked until the disque prune jsboige owns**).
Until then, this plan is complete as prep; execution resumes when the sandbox is provisioned.

---

*Worker signals; sandbox execution + prod upgrade + visual/QA verdict are jsboige's.
Release gate untouched.*
