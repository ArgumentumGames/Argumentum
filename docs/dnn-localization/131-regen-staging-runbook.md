# #131 — Regen Staging Runbook (prereqs, NO launch)

**Issue:** [#131 — DNN platform upgrade (sandbox → prod)](https://github.com/ArgumentumGames/Argumentum/issues/131)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-19
**Base:** master `909d04c3`
**Status:** **DOC / non-gated.** This stages every **prerequisite** for an attended mindmap/PDF regen so that the
run itself is a one-command affair — but it **launches nothing**. Per ai-01 dispatch (`msg-20260619T050902`) the
SECONDAIRE lane: "stager prérequis régén (sans lancer)". The **actual regen is GATED jsboige** (scope arbitration
4-vs-8 lang + Windows foreground-lock → run = jsboige RDP / ai-01 Opus, not the worker).

> ## 🔄 REFRESH (2026-07-12) — scope resolved, prereqs largely proven, see #782 for the live harness
>
> Two things this runbook **deferred as open** are now **closed**:
> - **§5 "scope DEFERRED — 4-lang vs 8-lang"** → **RESOLVED: 8 languages shipped** (PR #565, 41 SVGs across
>   8/8 langs via `git ls-tree`; byte-proven regen `Fallacies_zh.svg` 2026-06-24). When the print-bundle regen
>   fires (post-GO-visuel jsboige), clobber **all 8** (`fr en ru pt es ar fa zh`) — scope option B is the live one.
> - **Foreground-lock blocker (§2)** → **mitigated**, not eliminated: PR #569 added the OS persistent-session
>   recipe (`tscon <session> /dest:console`) to keep the desktop foreground alive unattended. Freeplane headless
>   is tracked (#568, open). The foreground-lock is a **fire-time condition**, not a code gap.
>
> **Two operational advances this doc predates** (honor at fire-time):
> - **Playwright/CardPen deadlock = RESOLVED (#651)** — console-flood sync I/O moved to a transport queue. The
>   stall-watcher is reliable **provided it measures chromium **child** CPU, not parent-only** (parent-only
>   false-kills legitimate heavy renders like Fallacies).
> - **`dotnet run` build-server deadlock** — can silently hang after cross-worktree builds; mitigation =
>   `dotnet build-server shutdown` + explicit `dotnet build` + `dotnet run --no-build`.
>
> **This runbook remains valid for its staging structure** (FreeMind env var §2, Mode-isolation §3, harvest
> clobber §4, pre-flight §7). The **live, refreshed fire-time harness** — re-anchored to master `c1ed77d2` with
> the post-07-04 lessons — is
> [`../investigations/2026-07-11-regen-readiness-refresh-c1ed77d2.md`](../investigations/2026-07-11-regen-readiness-refresh-c1ed77d2.md)
> (PR #782 merged). Read that doc as the authoritative pre-regen gate; this runbook is its predecessor.

> **Scope-neutrality (HARD):** two scope options are on the table and **jsboige has not published the choice**
> (ai-01: "4 cycles sans publication"). This doc describes prereqs that are **identical for both scopes** and
> explicitly defers the one scope-dependent step (which language directories to clobber/regen) to the decision.
> Do **not** treat anything here as picking 4 or 8 languages.

---

## 1. What this runbook stages (and what it deliberately does not)

| Prereq | Staged here? | Notes |
|--------|:---:|-------|
| FreeMind binary discoverable at runtime | ✅ via env var (already merged [#536](https://github.com/ArgumentumGames/Argumentum/pull/536)) | `ARGUMENTUM_FREEMIND_PATH` — no config edit, no hardcoded path |
| `Mode` isolated to `Mindmapper` (for the mindmap-only pass) | ⏳ documented, set at run time | default stays `WebBasedImageGeneration \| QuestPdfGeneration` — **never** commit a permanent default change (lesson from #536 review) |
| Harvest cache clobber (anti-stale) | ⏳ documented, run at execution | the recurring stale-harvest failure mode (memory `feedback-stale-harvest-regen`) |
| Order: mindmaps BEFORE PDFs | ✅ documented | mindmaps feed no PDF directly, but isolating Mindmapper first proves the foreground-lock path before the long PDF pass |
| **Which languages** to clobber/regen | ❌ **DEFERRED — jsboige scope** | 4-lang (preserve es/ar/fa/zh) vs 8-lang (uniformise). The clobber target set differs. |

The pattern: **everything that is scope-neutral is staged; the one scope-dependent variable is left as a placeholder.**

---

## 2. FreeMind path — the env-var contract (already in place)

The runtime plumbing merged in [#536](https://github.com/ArgumentumGames/Argumentum/pull/536) (commit `f17d8e6e`,
`FallacyMindMapDocumentConfig.cs:554-567`) resolves the FreeMind binary in this order:

1. `config.FreeMindPath` (default `""` — **never hardcoded**, see `AssetConverterConfig.cs:278-280`)
2. `Environment.GetEnvironmentVariable("ARGUMENTUM_FREEMIND_PATH")`
3. if both empty/missing → `Logger.LogWarning("… Skipping GUI export.")` and the SVG conversion returns `false`
   (fail-loud, no silent XSLT fallback — jsboige explicitly rejected the XSLT fallback: "On ne veut pas le fallback
   xslt qui ne fonctionne pas bien").

**Staging action (run time, on the attended host):**
```powershell
# Verify FreeMind is installed (memory: installed at this path on the attended host)
Test-Path "C:\Program Files (x86)\FreeMind\FreeMind.exe"   # → True expected

# Set the env var for the regen session (process-scoped, no system change)
$env:ARGUMENTUM_FREEMIND_PATH = "C:\Program Files (x86)\FreeMind\FreeMind.exe"
```

> ⚠️ **Foreground-lock (HARD blocker, memory `feedback-mindmap-freemind-unattended-foreground`):** FreeMind's `SendKeys`
> SVG export **fails unattended** — Windows foreground-lock blocks the synthetic input. The export **requires an
> attended desktop session** (jsboige RDP, or ai-01 under Opus with a "hands-off" GO). This is why the regen is not a
> worker task. Staging the env var does not bypass this — it only ensures the binary is found once the session is
> attended.

---

## 3. Isolating Mindmapper (the order rationale: mindmaps before PDFs)

The default `Mode` (`AssetConverterConfig.cs:37`) is `WebBasedImageGeneration | QuestPdfGeneration` — **not** Mindmapper.
For the mindmap regen pass, isolate to Mindmapper only:

**Why isolate first:**
- The mindmap pass is the **foreground-lock-gated** step. Proving it works (attended) before the long PDF pass means a
  foreground-lock failure surfaces in ~minutes, not after a multi-hour PDF build.
- Mindmaps and PDFs are independent outputs — neither feeds the other in the pipeline — so the order is a **debugging
  optimization**, not a data dependency.

**Staging action (run time, on the attended host, AFTER §2 env var):**
```powershell
# Set Mode via JSON config override (SkipConfigFile stays true — C# defaults are the source of truth,
# but Mode can be overridden at the property level). OR set in the launch command.
# PREFERRED: temporary JSON override in the session, NOT a C# edit (lesson from #536 review — never
# commit a permanent Mode default change).
```

> **Discipline note:** the #536 review (NanoClaw COMMENT_WITH_CONCERNS) caught an earlier attempt that hardcoded a
> permanent `Mode = Mindmapper` default. That would make **every future** `dotnet run` only do mindmaps. The correct
> pattern is a **session-scoped override** (env var, temp JSON, or launch flag), reverted after the pass. This runbook
> documents that discipline; it does not encode a default change.

---

## 4. Harvest cache clobber — the anti-stale contract (MANDATORY before any regen)

**Recurring failure mode** (memory `feedback-stale-harvest-regen`, occurred 2026-06-07 and 2026-06-08 on Mémo Back):
`dotnet clean` only clears `bin/`/`obj/`. Harvest caches (`.harvest.json` + images) live under
`Target/{lang}/Harvest/` and `Target/{lang}/Images/` and are **NOT cleaned by MSBuild**. A "clean build + full regen"
silently reuses stale renders — the fix is in the binary but never executes for cached CardSets.

**Staging action (run time, scope-DEFERRED — replace `<langs>` per §5):**
```bash
# 1. Clobber the harvests for the target languages (scope-dependent — see §5)
for lang in <langs>; do
  find "Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0-windows/Target/$lang/Harvest/" \
    -name "*.harvest.json" -delete 2>/dev/null
  find "Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0-windows/Target/$lang/Images/" \
    -name "*.png" -delete 2>/dev/null
done

# 2. THEN clean + rebuild + regen
dotnet clean "Argumentum Converters.sln"
dotnet build "Argumentum Converters.sln"
# (regen command — gated, §6)
```

> ⚠️ **Path gotcha (memory):** the target directory is `net9.0-windows` (NOT `net9.0`). Mindmap SVGs, however, are
> committed to `Cards/Fallacies/Mindmaps/{lang}/` and `Cards/Virtues/Mindmaps/{lang}/` — **not** under
> `bin/.../Target/`. The mindmap pass writes to the committed `Cards/` tree directly; the PDF pass writes to
> `bin/.../Target/`. Clobber the right tree for the right pass.

---

## 5. The scope-dependent variable (DEFERRED — jsboige choice)

The **only** step that differs between the two scopes is the language set to clobber + regen:

| Scope option | Languages clobbered/regen'd | Rationale (from dashboard decision #12 + interactive session) |
|--------------|------------------------------|----------------------------------------------------------------|
| **A) es/ar/fa/zh only (preserve)** | `es ar fa zh` | preserves the 20 validated FR/EN/RU/PT SVGs; only the 4 new-language mindmaps need generation |
| **B) 8 languages (uniformise)** | `fr en ru pt es ar fa zh` | uniformises all 8 — interactive jsboige request, but contradicts dashboard decision #12 |

**This runbook does NOT choose.** ai-01 submitted both options to jsboige in interactive (4 cycles unpublished as of
2026-06-19). When jsboige publishes the choice, substitute the language set into the §4 clobber loop and the §6 regen
target. Until then, this stays a placeholder.

> The [#536](https://github.com/ArgumentumGames/Argumentum/pull/536) PR body documents both paths explicitly (it was
> merged with the contradiction flagged, not resolved).

---

## 6. The run command (GATED — not executed by this runbook)

For completeness, the regen command shape (attended host only, after §2 + §3 + §4):

```powershell
# Mindmap pass (foreground-lock-gated — attended session required):
$env:ARGUMENTUM_FREEMIND_PATH = "C:\Program Files (x86)\FreeMind\FreeMind.exe"
# Mode = Mindmapper (session override, not committed)
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"

# PDF pass (after mindmap pass green):
# Mode = WebBasedImageGeneration | QuestPdfGeneration (default)
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj"
```

**This runbook does not run these.** Execution is GATED jsboige (scope + attended host).

---

## 7. Pre-flight checklist (what "staged" means — tick at run time)

- [ ] FreeMind binary present on the attended host (`Test-Path` → True).
- [ ] `ARGUMENTUM_FREEMIND_PATH` set for the session (§2).
- [ ] Attended desktop session active (foreground-lock — §2 blocker).
- [ ] **jsboige scope decision published** (§5 — 4-lang vs 8-lang).
- [ ] Harvest cache clobbered for the chosen language set (§4).
- [ ] `dotnet clean` + `dotnet build` green (§4).
- [ ] Mindmap pass isolated first (§3) — prove foreground-lock before the long PDF pass.

All seven must be green before the regen. The first six are staged-by-this-doc; the seventh (scope decision) is the
gating human input.

---

## 8. Gate boundaries (this document)

- ✅ Stages every **scope-neutral** prereq (FreeMind env var, Mode-isolation discipline, harvest clobber, order).
- ✅ Documents the recurring stale-harvest failure mode + the foreground-lock blocker (memory-grounded).
- ✅ Flags the scope decision as the single gating variable (defers to jsboige, does not choose).
- ❌ Does **not** launch any regen — execution is GATED jsboige (scope + attended host).
- ❌ Does **not** commit a permanent `Mode` or `FreeMindPath` default change (lesson from #536 review).
- ❌ Does **not** pick the scope (4-lang vs 8-lang) — ai-01's interactive arbitration is unresolved.
- ❌ Does **not** declare a QA verdict (ai-01 + jsboige only).

## Sources

- [#536](https://github.com/ArgumentumGames/Argumentum/pull/536) `chore(regen-mindmap-freemind-8lang)` (merged `f17d8e6e`) — `ARGUMENTUM_FREEMIND_PATH` env-var fallback in `FallacyMindMapDocumentConfig.cs:554-567`, `FreeMindPath` default `""` at `AssetConverterConfig.cs:278-280`.
- Memory `feedback-stale-harvest-regen` — harvest cache clobber contract, `net9.0-windows` path, recurring Mémo Back failures (2026-06-07/08).
- Memory `feedback-mindmap-freemind-unattended-foreground` — FreeMind `SendKeys` fails unattended, needs attended desktop.
- Dashboard decision #12 (contradiction) + interactive jsboige session — the unresolved 4-lang vs 8-lang scope.
- ai-01 dispatch `msg-20260619T050902` — SECONDAIRE: "stager prérequis régén (sans lancer)".
- Repo: `AssetConverterConfig.cs:37` (Mode default), `:278-280` (FreeMindPath default), `FallacyMindMapDocumentConfig.cs:554-567` (env-var resolution).

---

*Worker regen staging runbook (doc/non-gated, NO launch). Execution is GATED jsboige; the visual PASS verdict is ai-01 +
jsboige. No production system touched, no regen launched.*
