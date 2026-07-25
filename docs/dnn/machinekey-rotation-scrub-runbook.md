# DNN machineKey — Rotation + Scrub Runbook (gated jsboige, secret-safe)

> **Severity**: HIGH — the DNN `machineKey` is **exposed publicly on GitHub** (branch `dnn/sandbox-runtime-1032`, commit `4b0297ee`). Treat as compromised. **Rotation is the only real fix.**
> **Secret-safe policy**: this document references secrets **by location only** (file path + attribute name). **No key value is ever written here.** GitGuardian must pass on this file. Matches discipline of PR #842 / #851.
> **Scope**: staged runbook — **ZERO execution** without explicit GO from jsboige (ops server, `#415` git-rewrite gated). Copy-paste procedure for jsboige.
> **Author**: po-2023 (dispatch ai-01 `msg-20260722T224852-6cz88c`, primaire).
> **Cross-ref**: #131 Phase D, `.githooks/pre-commit` (#842), `RUNBOOKS-GATED-OPS.md`.

---

## 0. Where the secret lives (emplacement, NOT value)

| Location | Content | Status |
|----------|---------|--------|
| `DNNPlatform/web.config` → `<system.web>` → `<machineKey validationKey="…" decryptionKey="…" decryption="AES" validation="HMACSHA256" />` | on **master**: placeholder `validationKey="REPLACE" decryptionKey="REPLACE"` (7-char, **not a secret**) | ✅ master clean |
| Same element, branch `dnn/sandbox-runtime-1032` (commit `4b0297ee`, **public on origin**) | **real** keys (128-hex `validationKey` + 64-hex `decryptionKey`, no `AutoGenerate`) | ⛔ **EXPOSED** |
| Same element, local working tree (runtime-expanded) | real keys (DNN runtime writes them on startup) | local-only, never committed |

> The exposed commit `4b0297ee` = sandbox runtime snapshot of DNN 10.3.2 + 2sxc 21.07 (bin/ 330 files). Git author = jsboige (auth routing, not provably human-vs-agent).

**Why this matters** (threat model): a leaked `machineKey` lets an attacker forge DNN authentication cookies + tamper ViewState → **unauthenticated account impersonation / privilege escalation** on any DNN site using the same key. The prod site (`dnn.argumentum.myia.io`) is the target if it runs the exposed key.

### 0.1 That conditional is now RESOLVED — prod does **not** run the exposed key (2026-07-25)

The clause above ("*…if it runs the exposed key*") was left open when this runbook was written. It is now closed on
evidence. `myia-web1` classified the **live prod** `web.config` (classification only — lengths + algorithm names,
**zero values transmitted**, per the secret-safe policy at the top of this file) and the result is categorical:

| attribute | **exposed** key (sandbox, `4b0297ee`) | **prod web1** key (measured) |
|-----------|---------------------------------------|------------------------------|
| `validationKey` length | **128 hex chars** | **40 chars** |
| `decryptionKey` length | **64 hex chars** | **48 chars** |
| `validation` | **HMACSHA256** | **SHA1** |
| `decryption` | **AES** | **3DES** |

**All four attributes differ.** These are not two instances sharing a key — they are keys of two different
generations. The 40/48 + 3DES/SHA1 shape is the **legacy ASP.NET machineKey format** (validationKey 40 hex = 20
bytes for SHA1; decryptionKey 48 hex = 24 bytes for 3DES); the sandbox used the modern AES/HMACSHA256 shape.
Prod cannot be running the exposed key: it has neither its length nor its algorithms.

**Consequence — prod is OUT OF SCOPE of this compromise remediation.** No emergency rotation, no maintenance
window, no session invalidation on `argumentum.games`. The jsboige GO of 2026-07-24 covered the **sandbox**
(the po-2023 box, whose key was genuinely leaked live); extending a sandbox-scoped GO to an outward-facing,
hard-to-reverse prod action would have been unjustified — and, as measured, factually unnecessary.

> **Separate item, deliberately NOT merged into the above:** SHA1 and 3DES are legacy algorithms. That is a
> **hardening** observation, not an incident response. It belongs in a normal maintenance window with explicit
> jsboige sign-off — **not** in a compromise-remediation action, and not while he is unreachable. Tracked as
> backlog, not executed.

**Method note worth keeping:** this was settled *without any secret crossing a wire*. Comparing lengths and
algorithm names was sufficient to prove non-identity. When you need to answer "is this the same key?", compare
fingerprints — never values.

---

## A. Rotation (server-side — the ONLY real fix) — do FIRST

**Why first**: deleting the branch (B) does NOT purge the secret — GitHub retains orphaned commits server-side, and any fork/cache still has it. **Rotation invalidates the exposed key everywhere**; scrub (B) is then repo hygiene.

**Good timing**: the site is **not yet in public prod** (go-live pending v0.9.0) → rotation cost is near-zero (no active user sessions to disrupt). Do it **before** go-live.

### A.1 Generate fresh key material (run ON THE SERVER; output is NEVER committed)

```powershell
# Run on the production server. Output is a NEW key — do NOT paste it into git, this repo, or any dashboard.
$bytes = { param($n) -join (1..$n | ForEach-Object { '{0:X2}' -f (Get-Random -Maximum 256) }) }
$validationKey = & $bytes 64   # 128 hex chars (SHA256/HMACSHA256)
$decryptionKey = & $bytes 32   # 64 hex chars (AES)
"validationKey=$validationKey"
"decryptionKey=$decryptionKey"
```

> Alternative: IIS Manager → site → **Machine Key** feature → uncheck "Automatically generate at runtime" → **Generate Keys** → Apply (writes new values to the site `web.config`).

### A.2 Apply the new keys to the live site `web.config`

Edit the live site's `web.config` (NOT the repo's tracked template) `<system.web>` `<machineKey>` element with the new values, keeping `decryption="AES" validation="HMACSHA256"`.

### A.3 Recycle the app pool

```powershell
# Force DNN to pick up the new machineKey — invalidates ALL old cookies/ViewState/sessions
Restart-WebAppPool -Name "<DNN-app-pool-name>"
```

**Side effect (expected)**: every existing auth cookie / ViewState / session is invalidated → all users must re-authenticate. Acceptable for a compromise + pre-go-live (no active public users yet).

### A.4 Smoke

- `dnn.argumentum.myia.io` homepage → HTTP 200.
- Login flow works (cookie signed with new key).
- 0 "Validation of viewstate MAC failed" errors (Event Viewer → Application log).

---

## B. Scrub the repository (repo hygiene, AFTER rotation A)

> **Load-bearing warning**: branch `dnn/sandbox-runtime-1032` is the **migration reference** for po-2023 (bin/ 330 files, clean net48 runtime). Before **any** delete (B1), ensure its migration analysis is captured durably → see `dnn10-migration-readiness.md` (item 2 of the dispatch). **Do NOT delete before that capture + before rotation (A) is confirmed live.**
>
> **⚠ Second load-bearing item — found at execution time (2026-07-25), missed by the warning above.** The branch
> carried **two** unique commits, not one:
>
> | Commit | Content | Disposition before B1 |
> |--------|---------|----------------------|
> | `4b0297ee` | sandbox runtime snapshot 10.3.2 + 2sxc 21.07 (3338 files) — **carries the exposed machineKey** | must NOT be preserved (that is the point of the scrub); analysis captured in `dnn10-migration-readiness.md`, canonical bin/ kept locally |
> | `78cd1aab` | `DNNPlatform/.well-known/acme-challenge/web.config` (12 lines, **zero secrets**) — the win-acme HTTP-01 bypass, **load-bearing for cert renewal due 2026-08-23** | **cherry-picked to master before deletion** + mechanism documented in `go-live-turnkey-checklist.md` |
>
> **Generalisation**: `git log --oneline origin/master..<branch>` before any branch delete — enumerate **every** unique
> commit and give each an explicit disposition (preserved / deliberately discarded). A branch created for one purpose
> accumulates unrelated operational commits; "the branch is no longer needed for X" does not license discarding Y.
> (Project rule «Consolider ≠ Archiver»: no delete without proof of preservation.)

### B.1 Bleed-stop (quick, recommended) — delete the exposed remote ref

```bash
# Removes the ref from origin. The commit becomes unreachable from any branch.
git push origin --delete dnn/sandbox-runtime-1032
```

**What this does / does NOT do**:
- ✅ Stops the secret from being trivially discoverable via `git ls-remote` / branch listing.
- ❌ Does **NOT** purge the commit: GitHub retains orphaned commits server-side (accessible by SHA via the API for an extended period), and any local clone/fork that fetched it still has it. **Hence rotation (A) is the only true fix.**

### B.2 History-rewrite + force-push (heavier, gated) — OPTIONAL

Rewriting `4b0297ee` out of history (`git filter-repo` / BFG on the key) + force-push would scrub the literal from the repo's history. **Gated by `#415`** (force-push on a shared repo requires explicit jsboige arbitration). And even then, it does not purge GitHub server-side retention or external forks.

**Recommendation**: B1 (bleed-stop) + rotation (A) is sufficient. B2 only if jsboige wants the literal gone from visible history (low marginal value given server-side retention).

---

## C. Post-action verification (after A + B1)

1. **master clean**: `git show origin/master:DNNPlatform/web.config | grep machineKey` → `validationKey="REPLACE"` (placeholder, unchanged).
2. **ref gone**: `git ls-remote origin dnn/sandbox-runtime-1032` → empty (post B1).
3. **live site on new keys**: login works, 0 ViewState-MAC errors (A.4).
4. **local scan (informational)**: `git log --all -S '<key-fragment>'` — should return only the (now-deleted) branch's commits if any local ref still points there; this confirms no *other* branch carried the secret. (Does NOT reflect GitHub server-side state.)
5. **pre-commit guard active**: `.githooks/pre-commit` (#842) blocks any future staged `web.config` carrying a real key → defense-in-depth against re-introduction.

---

## D. Optional hardening (post-rotation, separate decision)

- `.gitignore` should cover `DNNPlatform/web.config` so the runtime-expanded copy can't be accidentally staged. Currently it is git-tracked but the pre-commit hook (#842) is the active guard. A `.gitignore` entry + `git rm --cached` would be belt-and-suspenders (jsboige-gated — don't break the master template `092bb8f3` / #442).
- Rotate the SQL password (`dnn_user`) too — it was verified CLEAN (never committed) but shares the threat surface with the sandbox snapshot.

---

## Ordering (mandatory)

**A (rotation, server) → B1 (bleed-stop, repo) → C (verify).** Never scrub (B) before rotation (A): scrubbing a key that is still live on the server does nothing for security. Rotation is the remediation; scrub is hygiene.
