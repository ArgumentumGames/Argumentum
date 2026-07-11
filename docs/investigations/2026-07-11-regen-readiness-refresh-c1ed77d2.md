# 2026-07-11 — Régén-readiness REFRESH vs `c1ed77d2` (delta over the 07-04 checklist)

**Scope**: a **freshness refresh** of the regen-readiness harness
([`2026-07-04-regen-readiness-checklist-8lang.md`](2026-07-04-regen-readiness-checklist-8lang.md)),
re-anchored to master **`c1ed77d2`**. The 07-04 checklist remains the **operational harness**
(languages, ConverterMode flags, regen sequence, cache-clobber recipe, FreeMind GUI, OWL caveat,
verification, entry points, pre-regen gate) — this doc only corrects what has **drifted since** and
adds **post-07-04 operational lessons**. Read the 07-04 doc first; this is its addendum.

**Repo reference**: master `c1ed77d2` (was `21e2c666` on 07-04). Triggered by ai-01 dispatch `28xdu9`
(HIGH, deep-queue primary: "Readiness régén release — repro propre de l'état de départ pour la régén
post-GO-visuel sem. 13/07"). Worker: po-2023 (heavy-regen lane).

> **⚠ STATUS = PREP / REFRESH ONLY.** No regen is launched here. The regen remains **HOLD until
> jsboige GO visuel** (sem. 13/07). This doc verifies master `c1ed77d2` is regen-ready and documents
> the live fire-time prerequisites, so the regen is a one-shot when the GO window opens.

---

## 1. Base drift `21e2c666` → `c1ed77d2` (what landed between 07-04 and 07-11)

~30 commits. For **regen readiness**, only the **content-input** and **code-path** changes matter
(docs-only merges do not):

| Merge | Impact on regen | Lane |
|-------|-----------------|------|
| **AIF #498 tranches 1a/1b/1c** (#771/#776/#779 + propositions) | **CONTENT INPUT CHANGED** — 6 new columns in `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (`AIF_skosDirectRef`, `AIF_skosExceptionRef`, `AIF_skosOther`, `AIF_skosMappingType`, `AIF_attackType`, `AIF_attackedNode`); 121 fallacies now fully-modeled. The regen harvests this richer CSV → cards + OWL carry the AIF wiring. **Verified on master** (header grep). | po-2024 (Cards/) |
| **DNN i18n #767/#772/#774/#777** | **No pipeline impact** — docs-only (`docs/dnn-localization/`) + DNN-side artifacts. The AssetConverter does not read these. | po-2023 (docs) |
| **AIF Ontology Integration #763** | OWL crosslinks 59% + AIF wiring already in the committed `docs/ontology/argumentum.owl`. The regen **re-produces** OWL from the CSV (now AIF-enriched). | ai-01 |
| **Playwright deadlock #651 (RESOLVED)** | **Code-path fix** — see §4. Stall-watcher now measures chromium child CPU. | ai-01 |

**Net**: the one regen-relevant content delta is the **AIF attack columns in the Fallacies CSV**. The
regen from `c1ed77d2` will produce AIF-aware cards + ontology. This is **expected and desired** (not a
regression) — the 07-04 doc predates the AIF work and does not mention it.

---

## 2. Corrections to the 07-04 checklist (staleness fixed)

### 2a. Mindmap 8-lang coverage — **CLOSED, not "still need RDP"**

The 07-04 doc §5 says: *"historically 4 langs (FR/EN/RU/PT, 21 SVGs)... ar/fa/zh SVGs still need a
live RDP foreground window."* **This is STALE on `c1ed77d2`.** Verified via `git ls-tree`:

| Lang | SVGs committed | Lang | SVGs committed |
|------|---------------:|------|---------------:|
| fr | **6** | es | 5 |
| en | 5 | ar | 5 |
| ru | 5 | fa | 5 |
| pt | 5 | zh | 5 |

**Total: 41 SVGs, 8/8 languages shipped** (PR #565 + the 2026-06-24 byte-proven regen
`Fallacies_zh.svg` cmp IDENTICAL). The ar/fa/zh gap the 07-04 doc flagged is **closed**. FR has 6
(one `.content.svg` variant — the Virtues FR-frozen gap noted in MEMORY, deferred jsboige,
non-blocking v0.9.0).

**Implication for regen**: the mindmap stage will **re-produce** these 41 SVGs (not create them from
scratch). The XSLT-proscription check (#184) still applies — reject any non-Batik SVG after regen.
The RDP-foreground prerequisite (07-04 §5) remains a **fire-time condition** for the GUI automation,
but there is no longer a coverage gap to close first.

### 2b. Test baseline — 578 → **595 pass**

The 07-04 doc §9 cites `578 pass / 1 fail / 5 skip / 584 total`. **Current (REPORTED ai-01
2026-07-11): `595 pass / 1 fail (#133 OWLSharp round-trip) / 5 skip / 600 total`**, build zero-warning
(CS + NuGet audit). The 1 known-fail is pre-existing/tracked (#133), not a regen blocker. **Re-confirm
empirically at fire-time** (test counts are `dotnet test` empirical, never a copied figure
[[test-counter-empirical]]).

### 2c. AIF columns — **new regen input (not in 07-04 doc)**

See §1. The 07-04 doc's TL;DR ("80 PDFs, ~4500 images, 20 mind-map SVGs, OWL 5 MB") does not mention
AIF. The CSV now carries 6 AIF columns + 121 fully-modeled fallacies. **No action** — the pipeline
reads the CSV as-is — but release notes should reflect that the OWL + cards are AIF-enriched since
the 07-04 bundle.

---

## 3. Post-07-04 operational lessons to honor at fire-time

These lessons were learned **after** the 07-04 checklist was written and are NOT in it. Add them to
the pre-regen gate (07-04 §10):

### 3a. `dotnet run` build-server deadlock [[feedback-dotnet-run-buildserver-deadlock]]

`dotnet run` can **silently hang** (0 MSBuild child, flat CPU) on build-server negotiation after
repeated cross-worktree builds. Symptom: no output, no error, no progress.

**Mitigation** (add to gate): if `dotnet run` produces no output within ~60s, **do not wait** —
`dotnet build-server shutdown`, then explicit `dotnet build -c Release`, then
`dotnet run --no-build -c Release --project ...`.

### 3b. Playwright deadlock — **RESOLVED #651**, but stall-watcher caveat

The CardPen-local harvest deadlock is **fixed** (#651 — console-flood sync I/O on the Playwright
event-thread moved to a transport queue). The stall-watcher is now reliable **provided it measures
chromium **child** CPU, not parent-only** (parent-only false-kills legitimate heavy renders like
Fallacies). No action beyond running on the fixed code path (master `c1ed77d2` has it).

### 3c. CardPen local — **validated by the 07-01 Release regen**

The 07-04 doc §10 lists "CardPen local IIS up" as a prerequisite. **Confirmed in production**: the
2026-07-01 Release regen pivoted to CardPen local after the GitHub Pages `/Cards/` 404 (#629) and
completed 64 PDFs PNG-lossless, 0 failures, serial. CardPen local is the **only viable Release path**
(Pages cannot serve `/Cards/`, structural #629 — post-tag bug). Ensure `UseLocalCardpen=true`,
`LocalCardpenUrl="http://argumentum.myia.io"`, and the IIS site is up before firing.

### 3d. AssetConverter `Console.ReadKey` headless crash [[feedback-assetconverter-background-runkey]]

`dotnet run` headless **crashes on `Console.ReadKey`** (`Program.cs:381`) when the config file is
freshly created (the `newConfig` prompt). **Mitigation**: a pre-existing `AssetConverterConfig.json`
on disk → `newConfig=false` → prompt skipped. Since `SkipConfigFile=true` (C# defaults win), ensure
the JSON file exists (auto-generated on first run) so the prompt is never reached. Piping stdin is
futile.

### 3e. Harvest clobber — **still MANDATORY** (unchanged, re-emphasized)

`dotnet clean` ≠ clean regen. The 07-04 §4 recipe stands: clobber `*.harvest.json` in
`Target/<lang>/Harvest/` before any fresh harvest, else stale `.harvest.json` is reused as
authoritative and the AIF-enriched CSV changes will **not** propagate to card PNGs
([[feedback-stale-harvest-regen]]).

---

## 4. Readiness verdict — master `c1ed77d2`

**VERDICT (po-2023, prep — NOT a PASS declaration): master `c1ed77d2` is regen-ready.**

No code blocker. The remaining items are **fire-time conditions** (live state at the moment jsboige
opens the GO window), not master defects:

- [ ] jsboige GO visuel rendered (sem. 13/07) — **the gate**, not a code item.
- [ ] RDP live foreground window (FreeMind GUI) — `tscon /dest:console` or jsboige present (07-04 §5).
- [ ] `FreeMindPath` / `ARGUMENTUM_FREEMIND_PATH` set to a working FreeMind install.
- [ ] Harvest cache clobbered (§3e + 07-04 §4).
- [ ] CardPen local IIS up (§3c).
- [ ] `AssetConverterConfig.json` present on disk (§3d — avoid `ReadKey` prompt).
- [ ] `Mode` includes `Mindmapper | OwlGenerator` (07-04 §3); CMYK via `--pdf-cmyk` after.
- [ ] Unit tests green (§2b) — empirical re-confirm.
- [ ] ONE `dotnet run` at a time; `kill javaw` between runs; build-server-shutdown ready (§3a).

**Owner split at fire-time**: po-2023 executes the regen (heavy lane); ai-01 renders the visual
verdict (geometry/content/RTL/CJK — project rule: po-2023 signals counts/hashes, never declares PASS).

---

## 5. What is NOT in scope (gate boundaries)

- ❌ **No regen launched here** — HOLD until jsboige GO visuel.
- ❌ **No `Cards/` CSV edit** — game-content/AIF is po-2024's lane (the AIF columns are an input to
  the regen, not something this refresh touches).
- ❌ **No visual verdict** — ai-01 lane only.
- ❌ **No merge of #596/#674/#666** (HOLD set — sandbox garde-fou / runtime-pending / post-tag).
- ❌ **No DB write** (DNN-side provisioning #682 + Δ1/Δ2 #490 = gated jsboige/ops, separate lane).

---

## 6. Relationship to the release-doc ecosystem

This refresh sits in the **worker-side harness** layer, alongside the 07-04 checklist. The
jsboige-facing release docs are separate and unaffected:

- [`RELEASE-VALIDATION-v0.9.0.md`](../RELEASE-VALIDATION-v0.9.0.md) — technical dossier (source of truth).
- [`RELEASE-VERIFICATION-INDEX-v0.9.0.md`](../RELEASE-VERIFICATION-INDEX-v0.9.0.md) — doc review parcours.
- [`RELEASE-VISUAL-GO-SESSION-v0.9.0.md`](../RELEASE-VISUAL-GO-SESSION-v0.9.0.md) — jsboige visual session parcours (bundle v3, 80 PDFs CMYK).

Relates: dispatch `28xdu9`, [`2026-07-04-regen-readiness-checklist-8lang.md`](2026-07-04-regen-readiness-checklist-8lang.md)
(the harness this refreshes), #134 (release), #632 (CMYK), #651 (Playwright fix), #184 (XSLT
proscribed), #498 (AIF — the content delta), #458 (epic).
