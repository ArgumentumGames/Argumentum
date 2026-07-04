# v0.9.0 Release — Validation Dossier (for jsboige)

> ⚠️ **Superseded (2026-06-19 snapshot).** The counts and facts below predate **bundle v3** (80 PDFs DeviceCMYK + SWOP, Print&Play Standard/Light #648-650, Ghostscript post-process #632/#652, OWL EN+FR only, tests 578 pass). **Current reference: [`docs/RELEASE-VALIDATION-v0.9.0.md`](../RELEASE-VALIDATION-v0.9.0.md) (dossier v4, refreshed 2026-07-04).** This framework dossier stays as the archive of the 17–19 June validation; its body is intentionally not rewritten.

**Issue:** [#134 — publish new GitHub release with updated game materials](https://github.com/ArgumentumGames/Argumentum/issues/134)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17 (refreshed 2026-06-19)
**Base:** master `4dd3c6bd`
**Status:** **DOSSIER / FRAMEWORK (docs, non-gated).** This is the consolidated entry-point + gate
checklist + artifact inventory jsboige asked the worker to build. **The release itself stays
GATED** — no tag, no GitHub Release, no regen — until jsboige says go. This dossier *prepares*
jsboige's validation; it does not replace it.

> **2026-06-19 refresh:** (a) DNN target confirmed **10.3.2** (decision #2, #531 — was "10.1.2 vs
> 10.3.2 undecided" in §4); (b) the DNN upgrade-arc docs (#543 2sxc migration plan, #545 regen
> staging, #548 Phase 2 exec+rollback) are now indexed in §2; (c) artifact inventory re-verified
> against disk (64 PDFs still present, mindmaps es/ar/fa/zh still 0). No structural change — this
> is a staleness fix, not a rewrite.

> Issue #134's body is **stale** (it lists "4 languages FR/EN/RU/PT" and dependencies #127-#130/#116).
> The live scope is **8 languages** (FR/EN/RU/PT + AR/ES/FA/ZH, merged via #359/#360/#361) and all
> those dependencies are closed. This dossier reflects the **current** scope.

> **Path note:** placed in `docs/release-dossier/` (not `docs/release/`) because `.gitignore:6`
> rule `[Rr]elease/` excludes any directory named `release`/`Release` (build-output guard).

---

## 1. What this dossier is (and isn't)

The release documentation is **already substantial but scattered** across 4 locations. This file is
the **single index** so a reader (jsboige) finds everything in one place, **plus** the two net-new
pieces that were missing: the **release-tag gate checklist** (§4) and the **README download-section
snippet** (§5).

**This dossier does NOT duplicate** the detailed docs it indexes — it points to them.

## 2. The release-doc map (where everything lives)

| Doc | What it covers | Status |
|-----|----------------|--------|
| [CHANGELOG.md](../../CHANGELOG.md) | v0.9.0 changelog (8-lang, pipeline recovery, data quality, test coverage, migration notes) | ✅ scaffolded (root) |
| [docs/release-v0.9.0-validation-brief.md](../release-v0.9.0-validation-brief.md) | "Sur pièce" presentation of the changelog claims, with worker-verified facts (64 PDFs = 8 types × 8 langs, ~9 834 images) | ✅ po-2023, 2026-06-13 |
| [docs/publication/qa-scenario-8langues-release.md](../publication/qa-scenario-8langues-release.md) | Per-PDF-type QA scenario: the 8 doc types × risk + page count, setup instructions | ✅ the per-language verification guide |
| [docs/dnn-localization/release-validation/](../dnn-localization/release-validation/) | DNN-site validation (non-latin guide, checklist, 2sxc export spec) | ✅ DNN-scoped (separate from the cards release) |
| [docs/dnn-localization/131-target-revision-10.3.2-full-upgrade.md](../dnn-localization/131-target-revision-10.3.2-full-upgrade.md) (#531) | DNN target = **10.3.2 + 2sxc 21** (decision #2), templates already Razor14 | ✅ the DNN-coupling context |
| [docs/dnn-localization/131-2sxc-migration-plan.md](../dnn-localization/131-2sxc-migration-plan.md) (#543) | Phase 1.5: 2sxc 15.02→21.07 LTS sequence + 25-app verify | ✅ merged `4dd3c6bd` |
| [docs/dnn-localization/131-dnn-phase2-exec-rollback.md](../dnn-localization/131-dnn-phase2-exec-rollback.md) (#548) | Phase 2: DNN exec 9.11→10.3.2 + rollback runbook | ✅ merged `4dd3c6bd` |
| **This dossier** | Consolidated index + **gate checklist** + **README download snippet** | 🆕 net-new |

**Known correction tracked:** the validation-brief flags that CHANGELOG said "155 tests" but the
real count was 159 (now ~359/0/5 after the #204 contract wave). The CHANGELOG test figure should be
refreshed at tag time (jsboige gate).

## 3. Per-language artifact inventory (grounded, not asserted)

Verified against the built Release artifacts (`bin/Release/net9.0-windows/Target/<lang>/Documents/density-0/`).
**8 languages × 8 document types = 64 PDFs.** Parity confirmed (same 8 filenames in `fr` and `zh`).

### The 8 PDF types (per language, `xx` = lang suffix `fr|en|ru|pt|es|ar|fa|zh`)

| # | File | Role | Main risk |
|---|------|------|-----------|
| 1 | `Argumentum_TarotCards_xx.pdf` | Rules + Memo + Fallacies (largest) | content density |
| 2 | `Argumentum_TarotCards_Virtues_xx.pdf` | Virtues | body overflow (#190, most fragile) |
| 3 | `Argumentum_PokerCards_xx.pdf` | Scenarii | long paragraphs |
| 4 | `Argumentum_Fallacies_Web_A4_xx.pdf` | A4 grid 66×66 mm | dense grid |
| 5 | `Argumentum_Fallacies_Web_A0_xx.pdf` | A0 poster 12 cols | 1 page |
| 6 | `Argumentum_Fallacies_Web_Thumbnails_A4_xx.pdf` | 50×50 vignettes | ~4 pages |
| 7 | `Argumentum_TarotCards_Print&Play_A4_xx.pdf` | recto-verso home print | alignment |
| 8 | `Argumentum_PokerCards_Print&Play_A4_xx.pdf` | Scenarii home print | ~12 pages |

(Per-PDF risk detail + page counts live in [qa-scenario-8langues-release.md](../publication/qa-scenario-8langues-release.md).)

### Non-PDF artifacts

| Artifact | Scope | Status |
|----------|-------|--------|
| Card images (PNG) | ~9 834 images, 8 langs | ✅ regen 2026-06-12, exit 0, ~5.0 GB |
| MindMap SVGs (Fallacies) | fr/en/ru/pt committed (content/links/cards variants, 21 total) | ✅ 4 langs |
| MindMap SVGs (Fallacies) es/ar/fa/zh | RTL/CJK variants | ⛔ **BLOCKED** (Windows foreground-lock — needs attended RDP run, jsboige gate) |
| MindMap SVGs (Virtues) | content/links variants | ✅ partial |
| OWL ontology | `docs/ontology/argumentum.owl` (FR) | ✅ committed (#161, #130 closed) |

**⚠️ Release-blocking gap:** MindMap SVGs for es/ar/fa/zh are NOT generated (foreground-lock). If
v0.9.0 must ship mindmaps for all 8 languages, this is the blocker; if 4-lang mindmaps are
acceptable for v0.9.0 (es/ar/fa/zh deferred), it is not. **jsboige decision** (gate).

## 4. Release-tag gate checklist (net-new — what must pass before `git tag v0.9.0`)

This is the checklist jsboige (or ai-01 on visual QA) ticks off at tag time. The regen that fills
the dossier stays gated; this is the contract that says "ready to tag".

- [ ] **Artifacts built**: 64 PDFs (8 langs × 8 types) present in Release bin, exit 0.
- [ ] **Tests green**: `dotnet test` → 0 fail (baseline ~359/0/5; refresh the CHANGELOG figure).
- [ ] **CSV data 100%**: Fallacies/Virtues/Scenarii/Rules ×8 langs, no FR-contamination in
      EN/RU/PT/AR/ES/FA/ZH (per data-gate #303, verified 2026-05-23/24).
- [ ] **Visual QA** (ai-01 verdict, NOT worker): spot-check ≥1 PDF per language per the
      [QA scenario](../publication/qa-scenario-8langues-release.md) — especially Virtues body
      overflow (#190) and RTL/CJK rendering (ar/fa/zh).
- [ ] **MindMap scope decided**: 4-lang (committed) or 8-lang (needs es/ar/fa/zh attended run).
- [ ] **CHANGELOG finalized**: v0.9.0 date filled (`2026-06-XX`), test count corrected.
- [ ] **README download section** added (§5 snippet) + tag link.
- [ ] **Packaging plan** (issue #134): Full / Print&Play / Per-language / MindMaps / Ontology
      bundles — decide bundling tooling (GitHub Release assets vs git-lfs vs external).
- [ ] **DNN #131 status**: release is "coupled DNN" per jsboige decision. DNN target is now
      **confirmed 10.3.2 + 2sxc 21** (decision #2, [#531](../dnn-localization/131-target-revision-10.3.2-full-upgrade.md));
      the upgrade procedure is documented ([#543](../dnn-localization/131-2sxc-migration-plan.md) Phase 1.5 2sxc +
      [#548](../dnn-localization/131-dnn-phase2-exec-rollback.md) Phase 2 DNN). Confirm whether the GitHub release
      **gates on the DNN upgrade being live** or ships independently (jsboige call).

## 5. README download-section snippet (ready to paste — issue #134 asks for it)

`README.md` currently has **no download/release section** (issue #134 explicitly requests one).
Below is a gate-ready snippet jsboige can paste once v0.9.0 is tagged. Placeholder tag/URL.

```markdown
## Download

Pre-built game materials are available on the [releases page](https://github.com/ArgumentumGames/Argumentum/releases).

### Packages (v0.9.0)

| Package | Contents | Languages |
|---------|----------|-----------|
| **Full Package** | All materials (Tarot, Poker, Print&Play, FallaciesWeb A0/A4) | FR · EN · RU · PT · ES · AR · FA · ZH |
| **Print & Play** | Print&Play A4 PDFs only (home printing, recto-verso) | all 8 |
| **Per Language** | Complete materials for one language | pick one |
| **Mind Maps** | Fallacies + Virtues SVG mind maps | FR · EN · RU · PT (es/ar/fa/zh: follow-up) |
| **Ontology** | `argumentum.owl` + documentation | FR |

### Printing instructions (Print & Play)

- Print **recto-verso** (duplex, flip on long edge) on A4 heavy paper (160–250 g/m²).
- `TarotCards_Print&Play_A4` = Rules + Memo + Fallacies; `PokerCards_Print&Play_A4` = Scenarii.
- Cut along the card edges after printing.
```

*(This snippet is **not** pasted into README.md yet — that edit is jsboige's gate call at tag time.
Provided here so the release-ready README is one paste away.)*

## 6. What this dossier does NOT do

- ❌ Does **not** tag or publish a release (gated jsboige).
- ❌ Does **not** regenerate artifacts (the regen is gated; this inventories what's already built).
- ❌ Does **not** edit README.md, CHANGELOG.md, or any consumer file (snippet provided, not applied).
- ❌ Does **not** decide the MindMap 4-vs-8-lang scope, the packaging tooling, or the DNN coupling —
  those are jsboige gates surfaced in §3/§4.
- ❌ Does **not** declare a visual-QA verdict (ai-01 only).

## Sources

- Built artifacts: `Generation/Converters/Argumentum.AssetConverter/bin/Release/net9.0-windows/Target/<lang>/Documents/density-0/` (64 PDFs, 8 langs × 8 types, parity verified fr↔zh).
- [CHANGELOG.md](../../CHANGELOG.md) (root), [release-v0.9.0-validation-brief.md](../release-v0.9.0-validation-brief.md), [qa-scenario-8langues-release.md](../publication/qa-scenario-8langues-release.md).
- MindMaps: `Cards/Fallacies/Mindmaps/{fr,en,ru,pt}/` (committed); es/ar/fa/zh blocked (foreground-lock, dashboard decision #13).
- OWL: `docs/ontology/argumentum.owl` (#161 merged, #130 closed).
- Issue #134 (release epic, scope stale vs current 8-lang reality).
