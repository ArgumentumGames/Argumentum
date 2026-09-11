# #506 — Social Authentication Connectors — consolidated assessment (CANONICAL)

**DNN 10.3.2 + 2sxc 21 upgrade arc — Phase D (security)**
**Dates:** 2026-06-25 initial assessment (#597) · 2026-06-20 inventory doc (#555) · **2026-09-11 consolidated + re-measured on the live 10.3.2 préprod** (po-2023, multi-grain mandate `bm9tf6` + anti-duplication amendment `gqckb5`).
**Status:** Assessment + live-measured inventory, read-only. No code change, no provider update, no enable/disable, no secret read/displayed/committed.
**Canonical target:** this file supersedes `docs/dnn-localization/506-social-auth-connectors-inventory.md` (#555) — its rotation procedure and console checklists are merged here (§5–§7); that file is now a renvoi stub with a preservation map.
**Runtime note:** functional validation of these connectors is **deferred to the sandbox/recette** — credentials and consent are jsboige live steps (§6–§7).

---

## 1. Inventory (MEASURED on the préprod live webroot, 2026-09-11)

Runtime: DNN **10.3.2.0** (.NET Framework 4.8), webroot `D:\Dev\Argumentum\DNNPlatform`, all provider files dated 02/07/26 (platform-realign wave). Version info read via file metadata — no secret involved.

| Assembly (`bin/`) | Assembly ver | Build SHA | Size | Status |
|---|---|---|---|---|
| `DotNetNuke.Authentication.Facebook.dll` | 10.3.2.0 | `4b0214b8` | 9 728 o | upgraded with core |
| `DotNetNuke.Authentication.Google.dll` | 10.3.2.0 | `4b0214b8` | 9 216 o | upgraded with core |
| `DotNetNuke.Authentication.LiveConnect.dll` | 10.3.2.0 | `4b0214b8` | 9 216 o | upgraded with core (legacy surface remains) |
| `DotNetNuke.Authentication.Twitter.dll` | 10.3.2.0 | `4b0214b8` | 9 728 o | upgraded with core |
| **`Dnn.ExchangeOnlineAuthProvider.dll`** | 10.3.2.0 | `4b0214b8` | 24 576 o | **NEW vs 9.11.1 — modern Entra/Exchange Online path** |
| **`Dnn.GoogleMailAuthProvider.dll`** | 10.3.2.0 | `4b0214b8` | 25 600 o | **NEW vs 9.11.1 — modern Google path** |
| `Dnn.AuthServices.Jwt.dll` | 1.0.0.0 | — | 31 744 o | JWT auth service assembly |
| `Google.Apis.Auth.dll` | 1.69.0.0 | `04e8051` | 232 960 o | modern Google SDK surface |

- The four legacy social DLLs now share the **same build SHA as DNN core 10.3.2** (`4b0214b8dad…`, identical to `DotNetNuke.dll`) → official platform modules, upgraded **with** the core package. The #555-era claim "9.11.1.0 / 9 216 o / SHA `d2f44d2`" is **périmée**.
- The two NEW providers were absent from 9.11.1 and from both original docs. They have **no `DesktopModules/` dir and no `.dnn` manifest** in this install (bin-only assemblies shipped with the platform).
- `DesktopModules/AuthenticationServices/`: `DNN, Facebook, Google, JWTAuth, Live, Twitter` — 6 dirs (`JWTAuth` new vs the 5 listed in #555).
- Stale artifact: `Install/Provider/DnnJwtAuth_09.11.01_Install.resources` (9.11.1-era package staging — not runtime; candidate cleanup in a future #131 pass).

## 2. DNN 10.3.2 compatibility — now MEASURED, no longer projected

- The connectors are platform modules → they were carried automatically by the 9.11.1 → 10.3.2 upgrade (measured: same build SHA as core, §1).
- The #555 §2 prediction ("10.3.2 ships a modernized Microsoft provider") is **CONFIRMED**, with the actual assembly name `Dnn.ExchangeOnlineAuthProvider` (plus `Dnn.GoogleMailAuthProvider` on the Google side).
- `DotNetNuke.Authentication.LiveConnect` is still present in the platform line (dnnsoftware repo, `Live_Auth.dnn`) — both the legacy and the modern Microsoft paths are installed side by side.
- Runtime = .NET Framework 4.8 (same as core) → no runtime-migration concern for these assemblies.
- Community-reported fragility ("Facebook, Twitter, Google, Live Authentication Providers do not work in DNN CE", dnnsoftware.com answers) remains **REPORTÉ** — config-dependent, to validate on sandbox.

## 3. CVEs (unchanged since #597 — verified via NVD / cvedetails)

No CVE specific to `DotNetNuke.Authentication.*`. DNN CVEs are core-level:

| CVE | Description | Affects DNN range | Relevance here |
|---|---|---|---|
| CVE-2017-9822 | Cookie deserialization → RCE | 5.0.0 – 9.3.0 | 10.3.2 above range → patched. Not provider-specific. |
| CVE-2025-64095 | Unauthenticated file upload | pre-10.1.1 | Patched 10.1.1 → covered by 10.3.2. Not provider-specific. |

Auth-relevant surface = OAuth flows delegated to core (`Microsoft.Owin.Security.*`) — no standalone connector CVE.

## 4. Per-connector risk (updated 2026-09-11)

| Connector | External API status | Risk | Qualification |
|---|---|---|---|
| Facebook (legacy) | OAuth2 live | Low (config-dependent) | REPORTÉ fragility; sandbox-validate |
| Google (legacy) | OAuth2 live | Low (config-dependent) | REPORTÉ fragility; sandbox-validate |
| LiveConnect (legacy Microsoft) | Legacy Live Connect API — superseded | **Medium** — legacy endpoint surface | SUPPOSÉ deprecated path; **modern alternative now installed (ExchangeOnline)** → retirement candidate |
| Twitter | X free OAuth tier closed (2023) | **Medium-High** — external breakage | Likely broken regardless of DNN version unless paid X API tier |
| **ExchangeOnlineAuthProvider (new)** | Microsoft Entra / Exchange Online modern endpoints | **Unknown — unconfigured** | Installed but no console app registration visible from repo; enable + Entra app = jsboige live step (§6) |
| **GoogleMailAuthProvider (new)** | Modern Google surface (`Google.Apis.Auth` 1.69) | **Unknown — unconfigured** | Same: enable + console credentials = jsboige live step |

## 5. Secret-rotation procedure (merged verbatim from #555 §5 — still authoritative)

**Hard rule: NO secret is ever stored in this repo.** Secrets live only in the DNN database (the provider's App ID/secret are entered through the DNN admin UI → Settings → Security → Authentication and persisted server-side).

For each provider that stays:
1. **Generate** a new Client Secret in the provider's developer console.
2. **Enter** the new App ID + secret in **DNN → Settings → Security → Authentication → {Provider}** (admin UI, server-side).
3. **Test** one end-to-end social login with a real account (not just "save succeeds").
4. **Record** (password manager / jsboige secure store, **NOT in this repo**): provider name, App ID (not secret), creation date, next rotation date, console URL.
5. **Revoke** the old secret in the console only **after** the new one is confirmed working end-to-end.

Rotation cadence (recommendation, jsboige to confirm): every 12 months, or immediately after admin turnover.

## 6. Per-provider console checklist (merged from #555 §3 — jsboige live steps)

| Provider | Deprecation risk to verify | Console | Healthy looks like |
|---|---|---|---|
| Facebook | Graph API version pinned to a deprecated version; app stuck in "development mode" | developers.facebook.com | App **Live**, valid App ID + secret, current Graph version, redirect URI = prod login endpoint |
| Google | Legacy `gapi.auth2` flow vs Google Identity Services | console.cloud.google.com → Credentials | OAuth 2.0 Client ID valid, authorized redirect URIs include the Argumentum login endpoint, project not quota-capped |
| Microsoft | Legacy `login.live.com` endpoints (LiveConnect) vs Entra v2.0 (ExchangeOnline provider) | entra.microsoft.com (App registrations) | Entra v2.0 app registration with redirect URI — **or retire** |
| Twitter/X | OAuth 1.0a + API tier (free tier no longer grants sign-in) | developer.twitter.com | App exists, tier allows OAuth sign-in — **or retire** |

## 7. Open questions for jsboige (updated 2026-09-11)

1. **Which providers are ENABLED in the préprod DB** now that six social paths are installed (4 legacy + 2 new)? Enabled-state lives in the DB/admin UI — not readable from the repo.
2. **Microsoft**: switch to the installed `Dnn.ExchangeOnlineAuthProvider` (Entra app registration required) and retire `LiveConnect`?
3. **Twitter/X**: retire (likely broken on free tier) or fund a paid tier?
4. **Recette timing**: fold connector validation into the post-upgrade sandbox pass (§2) — before prod cutover.

## 8. Consolidation changelog (proof of preservation)

| Source section | Disposition |
|---|---|
| #555 §1 (9.11.1 stock inventory table) | Replaced by measured §1 (delta: 4 DLLs now 10.3.2.0 + 2 new providers + `JWTAuth` dir + stale `Install/Provider` artifact) |
| #555 §2 (Live Connect legacy finding) | Preserved in §2/§4 — prediction confirmed, modern path now installed |
| #555 §3 (console checklist) | Merged verbatim → §6 |
| #555 §4 (upgrade-sequence recommendation) | Historical — the upgrade is done; outcome recorded in §1 |
| #555 §5 (rotation procedure) | Merged verbatim → §5 |
| #555 §6 (open questions) | Merged + updated → §7 |
| #555 §7 (out-of-scope) | Folded into header (status line) |
| #597 §1–§6 (this file, previous revision) | §1 re-measured; §2 projected→measured; §3 preserved; §4 updated (2 new rows); §5–§6 preserved |

## Sources

- Live measurement 2026-09-11: `DNNPlatform/bin/*.dll` file version info (read-only), `DesktopModules/AuthenticationServices/`, `Install/Provider/` — préprod 10.3.2.
- [CVE-2025-64095 (NVD)](https://nvd.nist.gov/vuln/detail/cve-2025-64095) · [CVE-2017-9822 advisory](https://github.com/murataydemir/CVE-2017-9822) · [Dotnetnuke CVE list](https://www.cvedetails.com/vulnerability-list/vendor_id-2486/Dotnetnuke.html)
- [DNN answers — auth providers functional issues (REPORTED)](https://www.dnnsoftware.com/answers/facebook-twitter-google-live-authentication-providers-do-not-work-in-dnn-ce)
- Issue #506 (filed by ai-01 from jsboige finding 2026-06-16) · related #131, #490, #593.
