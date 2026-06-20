# #506 — Social-Authentication Connectors Inventory + Update Procedure (Facebook / Google / Microsoft / Twitter)

**Status:** **DOC / research / non-gated.** Repo-grounded inventory of the social-authentication providers shipped with the DNN 9.11.1 export, plus the update + secret-rotation procedure. **No prod mutation, no live DNN touch, no secret stored in repo.** Everything requiring access to the developer consoles (Facebook for Developers, Google Cloud Console, Microsoft Entra) is flagged as a **jsboige live step** — this doc structures it but cannot execute it.

**Context:** Filed by ai-01 from a jsboige finding during the live verification of #490 (admin login on https://www.argumentum.games, 2026-06-16): the social-auth connectors are probably outdated and risk silently breaking social login. To be coordinated with the #131 platform upgrade (the admin banner already advertises DNN 10.3.2).

---

## §1 — What ships in the DNN 9.11.1 export (repo-grounded)

Verified directly against `DNNPlatform/` in this repo (no live DB access needed for this part):

| Provider | Source dir (`DesktopModules/AuthenticationServices/`) | Assembly (`bin/`) | Notes |
|----------|--------------------------------------------------------|-------------------|-------|
| **DNN** (built-in) | `DNN/` | `DotNetNuke.dll` (core) | Default credential auth. Not a social provider. |
| **Facebook** | `Facebook/` | `DotNetNuke.Authentication.Facebook.dll` | OAuth 2.0 Graph API. |
| **Google** | `Google/` | `DotNetNuke.Authentication.Google.dll` | Pre-Identity-Services SDK risk (see §3). |
| **Microsoft** | **`Live/`** | **`DotNetNuke.Authentication.LiveConnect.dll`** | ⚠ **"Live Connect" = the legacy pre-Entra name.** This is the most likely to break — see §3. |
| **Twitter** | `Twitter/` | `DotNetNuke.Authentication.Twitter.dll` | OAuth 1.0a-era connector. |

**JWT auth layer** (separate from social providers): `Install/Provider/DnnJwtAuth_09.11.01_Install.resources` — version **09.11.01**, aligned with the DNN 9.11.1 platform.

**Assemblies are uniform**: all four social DLLs are 9,216 bytes with the same timestamp (DNN build artifact), i.e. the **stock DNN 9.11.1 provider versions** — no custom/updated social-auth extension is installed.

---

## §2 — The highest-risk finding: "Live Connect" (Microsoft) is the legacy name

The Microsoft provider ships as **`LiveConnect`** (`Live/`, `DotNetNuke.Authentication.LiveConnect.dll`). "Live Connect" was Microsoft's consumer-auth brand (Live ID / Outlook.com era), **deprecated in favour of Microsoft Account / Entra ID (Azure AD) v2.0 endpoints**.

- The DNN 9.11.1 `LiveConnect` provider talks to the **legacy `https://login.live.com/oauth20_*` endpoints**, not the modern `https://login.microsoftonline.com/{tenant}/v2.0/` endpoints.
- **Risk**: Microsoft can deprecate the old endpoints → silent login failure for any user using a Microsoft account. This is the connector most likely to already be broken or about to break.
- **Resolution path**: this is typically fixed by the **DNN platform upgrade itself** (#131 → 10.3.2 ships a modernized `Dnn.Auth.Microsoft` / Entra-aligned provider) OR by a third-party DNN 10 social-auth extension. **Do not patch the 9.11.1 `LiveConnect` DLL in place** — it is superseded by the upgrade.

---

## §3 — Per-provider deprecation risk (what to verify live)

These are **live checks jsboige must run in the developer consoles** (the repo cannot see App IDs, secrets, or endpoint responses):

| Provider | Deprecation risk to verify | Console | What "healthy" looks like |
|----------|----------------------------|---------|---------------------------|
| **Facebook** | Graph API version pinned to a deprecated version (FB deprecates old Graph versions yearly). App in "development mode" instead of "live". | [developers.facebook.com](https://developers.facebook.com) | App **Live**, valid App ID + secret, Graph version current, redirect URI = `https://www.argumentum.games/...`. |
| **Google** | Pre-**Google Identity Services** (GIS) OAuth flow (old `gapi.auth2`) — Google deprecated it in favor of GIS (GSI). Old DNN 9.11 Google provider may use the legacy flow. | [console.cloud.google.com](https://console.cloud.google.com) (APIs & Services → Credentials) | OAuth 2.0 Client ID valid, authorized redirect URIs include the Argumentum login endpoint, project not deleted/quota-capped. |
| **Microsoft (LiveConnect)** | **Legacy login.live.com endpoints** (see §2). This is the #1 risk. | [entra.microsoft.com](https://entra.microsoft.com) / [azure.microsoft.com](https://portal.azure.com) (App registrations) | If keeping social Microsoft login post-upgrade → an Entra **v2.0 app registration** with redirect URI; otherwise plan to retire it. |
| **Twitter** | OAuth 1.0a still works but Twitter/X API tiers changed (free tier no longer grants OAuth sign-in as of 2023). App may be on a now-deprecated tier. | [developer.twitter.com](https://developer.twitter.com) (developer portal) | App exists, API tier allows OAuth 1.0a sign-in (or decide to retire Twitter login). |

---

## §4 — Coordination with the #131 platform upgrade (the recommended sequence)

**Recommended: fold the connector updates INTO the #131 upgrade, don't do them standalone on 9.11.1.**

1. **Pre-upgrade (now, sandbox-safe):** inventory the live App IDs/secrets in each console (§3 table). Confirm which connectors still have valid credentials. Retire any provider nobody actually uses (fewer moving parts).
2. **During #131 upgrade (9.11.1 → 10.3.2):** the 10.3.2 platform ships refreshed auth providers. Re-test each social login end-to-end on the **sandbox** after the upgrade, before prod cutover.
3. **Post-upgrade (prod):** rotate all secrets (§5), run one end-to-end login per provider on `https://www.argumentum.games`.

**Why not patch 9.11.1 standalone:** the `LiveConnect` legacy provider on 9.11.1 cannot be made modern without a 3rd-party extension; doing it on 9.11.1 then redoing it on 10.3.2 is wasted work. The upgrade is the natural breakpoint.

---

## §5 — Secret-rotation procedure (the part that must be documented regardless of version)

**Hard rule: NO secret is ever stored in this repo.** Secrets live only in the DNN database (`web.config` references the provider; the actual App ID/secret are entered through the DNN admin UI → Settings → Authentication and persisted server-side).

For each provider that stays:

1. **Generate** a new Client Secret in the provider's developer console.
2. **Enter** the new App ID + secret in **DNN → Settings → Security → Authentication → {Provider}** (admin UI, server-side).
3. **Test** one end-to-end social login with a real account (not just "save succeeds").
4. **Record** (in a password manager / jsboige's secure store, **NOT in this repo**):
   - provider name, App ID (not secret), creation date, next rotation date, console URL.
5. **Revoke** the old secret in the console only **after** the new one is confirmed working end-to-end.

**Rotation cadence (recommendation, jsboige to confirm):** every 12 months, or immediately after any admin turnover.

---

## §6 — Open questions for jsboige (block the live portion)

These require jsboige's access and cannot be resolved from the repo:

1. **Which social providers are actually used** on the prod site today? (Retire the unused ones before the upgrade — less to test.)
2. **Microsoft**: keep social Microsoft login (→ needs Entra v2.0 setup) or retire it?
3. **Twitter/X**: keep it (→ verify API tier allows OAuth sign-in) or retire it?
4. **Timeline**: fold into #131 upgrade (recommended) or urgent standalone fix?

---

## §7 — Out of scope (explicit)

- Touching the live DNN database or admin settings.
- Storing any App ID / Client Secret in this repo.
- Patching the 9.11.1 `LiveConnect` DLL in place (superseded by #131).
- Anything that needs the developer consoles — those are jsboige live steps, structured here.

## Sources

- `DNNPlatform/DesktopModules/AuthenticationServices/{DNN,Facebook,Google,Live,Twitter}/` (5 provider source dirs)
- `DNNPlatform/bin/DotNetNuke.Authentication.{Facebook,Google,LiveConnect,Twitter}.dll` (4 assemblies, 9,216 bytes each, stock 9.11.1)
- `DNNPlatform/Install/Provider/DnnJwtAuth_09.11.01_Install.resources` (JWT auth version)
- Issue #506 (filed by ai-01 from jsboige finding 2026-06-16)
- Linked: #131 (platform upgrade), #490 (i18n UI strings — the verification that surfaced this)
