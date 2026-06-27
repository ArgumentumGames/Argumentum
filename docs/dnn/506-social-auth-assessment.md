# #506 — Social Authentication Connectors Assessment

**DNN 10.3.2 + 2sxc 21 upgrade arc — Phase D (security)**
**Date:** 2026-06-25 · **Author:** po-2023 (dispatched by ai-01, 2026-06-25 14:05)
**Status:** Assessment only (read-only inventory + risk evaluation). **No code changes.**
**Related:** #131 (DNN upgrade arc), #458, #593 (upgrade assessment audit)
**Runtime note:** functional validation of these connectors is **deferred to the sandbox** post-upgrade.

---

## 1. Inventory (VERIFIED)

Four DNN-native social authentication connectors ship as **distributed binaries** — they are NOT compiled from repo source. There is no `.cs`/`.vb` for these under `DNNPlatform/DesktopModules/AuthenticationServices/{Facebook,Google,Live,Twitter}/` (only `Login.ascx`, `Settings.ascx`, `App_LocalResources/`, `Images/`, `license.txt`, `releasenotes.txt`).

| Connector DLL | Assembly ver | File ver | Build SHA | Size |
|---|---|---|---|---|
| `DotNetNuke.Authentication.Facebook.dll` | 9.11.1.0 | 9.11.1.19 | `d2f44d2` | 9 KB |
| `DotNetNuke.Authentication.Google.dll` | 9.11.1.0 | 9.11.1.19 | `d2f44d2` | 9 KB |
| `DotNetNuke.Authentication.LiveConnect.dll` | 9.11.1.0 | 9.11.1.19 | `d2f44d2` | 9 KB |
| `DotNetNuke.Authentication.Twitter.dll` | 9.11.1.0 | 9.11.1.19 | `d2f44d2` | 9 KB |

All 4 share the **same build SHA as DNN core 9.11.1** → they are **official DNN modules**, versioned and released with the platform (not third-party extensions). `LiveConnect` is the **Microsoft-account** connector (legacy Live/Hotmail/Microsoft-account API) — there is **no separate "Microsoft" connector**.

Two additional Google **analytics** connectors (not auth) are present: `DNN.Connectors.GoogleAnalytics.dll`, `DNN.Connectors.GoogleTagManager.dll` — out of scope for this auth assessment.

Source layout per connector (`DNNPlatform/DesktopModules/AuthenticationServices/<Name>/`): `Login.ascx`, `Settings.ascx`, `App_LocalResources/`, `Images/`, `license.txt`, `releasenotes.txt`. No `releasenotes.txt` content was parsed for this assessment (candidate follow-up if per-connector changelog matters).

## 2. DNN 10.3.2 compatibility (VERIFIED + REPORTED)

- These connectors are **DNN platform modules** → upgraded automatically **with the DNN core package** (9.11.1 → 10.3.2). They are not independently versioned extension packages requiring separate upgrade steps.
- `DotNetNuke.Authentication.LiveConnect` is **still present in the official Dnn.Platform repository** (confirmed via DNN-21505 PR touching `Live_Auth.dnn`) → not removed in the 10.x line.
- **REPORTED functional issues** (community forum, dnnsoftware.com): *"Facebook, Twitter, Google, Live Authentication Providers do not work in DNN CE"* — this indicates known fragility / misconfiguration patterns, **not necessarily formal deprecation**. Status: **REPORTÉ**, to validate on sandbox.
- Runtime = **.NET Framework 4.8** (same as DNN core, per `project-dnn10-net-framework-not-net8`) → no .NET 8 migration concern for these assemblies.

## 3. CVEs (VERIFIED via NVD / cvedetails)

**No CVE found specific to `DotNetNuke.Authentication.{Facebook,Google,LiveConnect,Twitter}`.** DNN CVEs are DNN-core-level:

| CVE | Description | Affects DNN range | Relevance here |
|---|---|---|---|
| **CVE-2017-9822** | Cookie deserialization → RCE | 5.0.0 – **9.3.0** | DNN **9.11.1 is above** the range → already patched. Not auth-provider-specific. |
| **CVE-2025-64095** | Unauthenticated file upload | pre-**10.1.1** | Patched in **10.1.1**. Covered by the 9.11.1 → 10.3.2 upgrade (pragmatic target **10.1.2+**). Not auth-provider-specific. |

Auth-relevant risk = **OAuth library surface**: these thin wrappers delegate to `Microsoft.Owin.Security.*` / OAuth2 flows living in DNN core. No standalone auth-connector CVE surfaced.

## 4. Per-connector risk assessment

| Connector | External API status | Risk | Qualification |
|---|---|---|---|
| **Facebook** | OAuth2 — live | Low (config-dependent) | REPORTÉ functional issues; validate sandbox |
| **Google** | OAuth2 — live | Low (config-dependent) | REPORTÉ functional issues; validate sandbox |
| **LiveConnect (Microsoft)** | Legacy Live Connect API — superseded by Microsoft Graph / Azure AD | **Medium** — legacy surface | SUPPOSÉ deprecated path; still in DNN repo; validate whether login still functions post-upgrade |
| **Twitter** | X/Twitter closed free OAuth tier (2023), paid tiers required | **Medium-High** — API-side breakage (external) | REPORTÉ API change (not a DNN issue); login likely broken for non-paying X API tier **regardless of DNN version** |

## 5. Recommendations

1. **Carry all 4 connectors through the DNN 10.3.2 upgrade** (they upgrade with the core). No independent package action required.
2. **CVE exposure is already closed by the DNN core upgrade** (CVE-2017-9822 range-clear, CVE-2025-64095 patched @10.1.1). **No auth-connector-specific patching needed.**
3. **Sandbox validation (post-upgrade)**: test each connector's login flow with fresh OAuth credentials — confirm Facebook/Google still bind, probe whether LiveConnect still authenticates, and confirm Twitter status (likely needs a paid X API tier → may warrant **disabling** if unused).
4. **Follow-up (post-validation)**: if LiveConnect/Twitter prove non-functional, **disable the unused connectors** (Admin → Extensions → Authentication Systems) rather than maintain dead attack surface.
5. **No code change in this PR** (assessment-only, per dispatch). Any enable/disable action is an operational sandbox step, tracked separately.

## 6. Validation deferred to sandbox

- Functional login test per connector (Facebook, Google, LiveConnect, Twitter) post-DNN-10.3.2-upgrade.
- Confirm no auth-connector assembly load error after the 2sxc 21 + DNN 10.3.2 upgrade (these are .NET Framework 4.8 assemblies, same runtime as DNN core).

## Sources

- [CVE-2025-64095 (NVD)](https://nvd.nist.gov/vuln/detail/cve-2025-64095) — DNN file upload, patched 10.1.1
- [CVE-2017-9822 (GitHub advisory)](https://github.com/murataydemir/CVE-2017-9822) — DNN cookie deserialization RCE (5.0.0–9.3.0)
- [Dotnetnuke CVE list (cvedetails)](https://www.cvedetails.com/vulnerability-list/vendor_id-2486/Dotnetnuke.html)
- [DNN Security Center (dnncommunity.org)](https://dnncommunity.org/security)
- [DNN answers — auth providers functional issues (REPORTED)](https://www.dnnsoftware.com/answers/facebook-twitter-google-live-authentication-providers-do-not-work-in-dnn-ce)
- [DNN 10.00.00 release](https://dnncommunity.org/blogs/Post/20257/DNN-10-00-00-Released)
