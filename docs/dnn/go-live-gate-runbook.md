# DNN Go-Live Gate — Consolidated Runbook (capstone)

> **Purpose**: the single top-to-bottom procedure jsboige executes on go-live day. Consolidates the **scattered staged docs** (#854 machineKey, #442 web.config hardening, dnn10-migration-readiness, skin diagnostic) into one ordered, tagged sequence.
> **Tags** on every step:
> - `[jsboige-DECISION]` — arbitration required from jsboige (blocks progression)
> - `[po-2023-EXEC]` — worker execution (repo/PR only, never prod mutation)
> - `[ai-01-VERDICT]` — visual/live validation gate (only ai-01 declares go-live)
> **Secret-safe HARD**: this runbook references secrets **by location only**. Zero key value, zero SQL password, zero connection string. It *says* "rotate the key" / "verify the connection string in backup file X before restore"; it never *contains* a secret. GitGuardian must pass.
> **Author**: po-2023 (dispatch ai-01 `msg-20260723T024927-zwcvck`, primaire).
> **Cross-ref**: #131, #132 (deployment, CLOSED), #811 (dependabot assessment §below).

---

## 0. Arbitrages jsboige requis AVANT go-live (blocking checklist)

These 4 decisions gate the release. None is resolvable by a worker.

| # | Arbitration | Recommendation | Where |
|---|-------------|----------------|-------|
| 1 | **machineKey rotation** (server-side, the ONLY real fix for the public exposure) | **Do FIRST, before any public traffic.** Pre-go-live = near-zero disruption cost. | [`machinekey-rotation-scrub-runbook.md`](machinekey-rotation-scrub-runbook.md) (#854) |
| 2 | **skin `tabid=138`** — Opt 1 (gate tag, fix in v0.9.1) vs Opt 2 (scope creep now) | **Opt 1** — the HTTP 500 is platform-version-independent and non-blocking for the tag. | [`skin-tabid138-diagnostic-runbook.md`](skin-tabid138-diagnostic-runbook.md) (#851) |
| 3 | **#681 2sxc App export** — hard-unblocker for DNN i18n | jsboige (content lives in live 2sxc DB; no worker can do it). | #681 (OPEN) |
| 4 | **T&A return #802** + release dossier validation | Gate for the v0.9.0 tag. | release dossier [ASK] (po-2023) |

> **Cluster state (ai-01, 2026-07-23 04:52)**: the autonomous runway is quasi-exhausted — the release + go-live cannot advance without these decisions. The 3 runbooks are pre-armed for copy-paste execution at GO.

---

## 1. Canonical go-live sequence (top-to-bottom)

Each step **points to** its detailed staged doc (does not duplicate content).

### Step A — `[jsboige-DECISION]` + server-ops: rotate machineKey (FIRST)
**Why first**: deleting the branch (B) does NOT purge the secret (GitHub retains orphan commits server-side; forks/caches keep it). Rotation invalidates the exposed key everywhere.
- **Do**: `machinekey-rotation-scrub-runbook.md` §A (generate fresh material on the server via `Get-Random`; apply to live `web.config`; recycle app pool → invalidates old cookies/ViewState; pre-go-live cost ~0).
- **Gate**: smoke homepage 200 + login flow works + 0 ViewState-MAC errors.
- `ai-01-VERDICT`: confirm site signs with the new key.

### Step B — `[po-2023-EXEC]` + `[jsboige-DECISION]`: scrub the repo (AFTER rotation)
- **B1 (recommended) bleed-stop**: `git push origin --delete dnn/sandbox-runtime-1032` — removes the ref. **BUT** the branch is **load-bearing** (migration reference) → delete only AFTER the migration analysis is captured (`dnn10-migration-readiness.md`, done in #854) AND after rotation (A) is confirmed live.
- **B2 (gated) history-rewrite**: force-push rewrite — **gated #415** (shared-repo force-push requires jsboige arbitration). Low marginal value given server-side retention; B1 + rotation is sufficient.
- **Verify C**: `machinekey-rotation-scrub-runbook.md` §C — master placeholder unchanged, ref gone, live on new keys, `.githooks/pre-commit` (#842) active.

### Step C — `[ai-01-VERDICT]`: web.config hardening (#442, already MERGED)
The security headers (CSP, HSTS commented, X-Frame-Options, nosniff, Permissions-Policy) from PR **#442 are already on master** (merged). This is hardening **already applied** — verify presence, not apply.
- **Verify**: `DNNPlatform/web.config` `<system.webServer>/<httpProtocol>/<customHeaders>` carries the #442 headers. Note: HSTS `<add name="Strict-Transport-Security"...>` is **commented** in #442 — activate ONLY when the site is fully HTTPS in production (`[jsboige-DECISION]`).

### Step D — `[ai-01-VERDICT]` + `[jsboige-DECISION]`: DNN 10.3.2 readiness
- **Code-side = low-risk & essentially complete** in repo-runtime: see [`dnn10-migration-readiness.md`](dnn10-migration-readiness.md). Telerik migration surface = **ZERO** (0/206 DLLs, `Telerik.Web.UI.dll` absent, 0 `.ascx`); skin objects DNN 10 standard; Razor APIs stable; OpenStore Telerik-free.
- **Remaining = runtime/ops**: runtime smoke-test at GO + skin `tabid=138` (Arbitration 2, Opt 1 = v0.9.1).

### Step E — `[jsboige-DECISION]`: 2sxc / #681 (DNN i18n hard-unblocker)
- The localization content lives in the **live 2sxc DB**, not the repo. The repo-side inventory (`docs/dnn-localization/457-site-content-type-inventory.md`) covers 5 content-types A-E; only A+B (10 UI-string keys) + C (2 HTML pages) are repo-extractable — the bulk (D+E) is DB-only and needs the portal/2sxc export = **#681 (jsboige)**.
- Not blocking the v0.9.0 tag (site can launch FR+partial EN); blocks the 8-language site localization.

### Step F — `[ai-01-VERDICT]`: deployment + live verdict (#132, CLOSED as the ops vehicle)
- Deploy per #132 runbook (ops VPS, jsboige only).
- **`ai-01-VERDICT`**: live site `dnn.argumentum.myia.io` — homepage 200, `/Argumentum` + `/Règles` render 2sxc content, 0 `JsonOptions`/conn-string error, `tabid=138` (post-fix Opt 1) renders.
- **Only ai-01 declares go-live**; po-2023 prepares/executes repo-side only.

---

## 2. Cross-cutting hardening (already in place, verify-only)

| Guard | Status | Ref |
|-------|--------|-----|
| Pre-commit hook blocks staged `web.config` with real secrets | MERGED #842 | `.githooks/pre-commit` |
| web.config security headers (CSP/nosniff/X-Frame) | MERGED #442 | `DNNPlatform/web.config` |
| machineKey rotation runbook | staged #854 (MERGED) | `machinekey-rotation-scrub-runbook.md` |
| DNN10 migration readiness | staged #854 (MERGED) | `dnn10-migration-readiness.md` |
| Skin `tabid=138` diagnostic | staged #851 (MERGED) | `skin-tabid138-diagnostic-runbook.md` |
| Gated-ops runbooks index | staged #851 (MERGED) | `RUNBOOKS-GATED-OPS.md` |

---

## 3. Dependabot #811 (skin `Bootstrap 4 Instant` — websocket-driver 0.7.4→0.7.5)

`[po-2023-EXEC]` assessment (read-only, 2026-07-22):
- **`websocket-driver` is transitive** (pulled via `ws`, itself dev-tooling for webpack/dev-server hot-reload). Absent from the skin's direct `package.json`.
- Patch bump (semver patch, no API change). **Zero prod runtime path** — the served skin never loads it.
- **Recommendation: merge** (low-risk hygiene). Not blocking v0.9.0 / go-live.
- **Not merging as a worker** — verdict merge = ai-01/jsboige (DNN-gated territory). Comment on #811 already posted (2026-07-22).

---

## 4. Ordering rule (non-negotiable)

**A (rotation, server) → B1 (bleed-stop, repo) → C-F (verify/deploy).** Never scrub (B) before rotation (A): scrubbing a key still live on the server does nothing for security. Rotation is the remediation; scrub is hygiene. Go-live verdict is ai-01's alone.
