# #188 — Rules cover extension (variantes de jeu) : scoping & options

**Issue**: [#188 — feat(rules): restore École des Menteurs cover layout + generalize to other rules with AI illustrations](https://github.com/ArgumentumGames/Argumentum/issues/188)
**Track**: #458 Track 4 (éditorial / features cartes)
**Author**: po-2024 (worker backlog)
**Date**: 2026-06-14
**Base**: master `2e4f6735`
**Status**: Scoping (key-free analysis) — **Phase 1 already resolved (PR #194); Phase 2 = AI illustrations, key-gated**

> Scope of this document: **analysis only**. No template/asset/pipeline code
> touched. Phase 2 illustrations require an AI/LLM image provider (decision +
> key from jsboige); the template-mechanics part is key-free but gated on the
> v0.9.0 release.

---

## 0. What's already done (do not redo)

**Phase 1 of #188 (the white-band regression) is RESOLVED.**

- **PR #194** (merged 2026-04-08, `74557ea6`) restored the École des Menteurs
  cover: scoped the CSS `:not([class~="1"])` selectors back to `card:` prefix
  (removed the parasitic grey band) + changed the subtitle from beige `#d9c1b4`
  to chalk yellow `#ffc307`. 1-line CSS edit in `Cards/Rules/Argumentum_Rules_fr.json`.
- **Diagnostic confirmed stable** (po-2024, #188 comment 2026-06-02): the cover
  CSS (`[class~="1"]`) is **identical across `9cdba475`, `d287b085`, `acc776ba`,
  and master** — the regression has not returned.
- **Visual confirmation** (ai-01, #188 comment 2026-06-02): the white band does
  **not** reproduce on master renders; clean black chalkboard + yellow subtitle.

→ **Phase 1 is closed.** This doc scopes only **Phase 2** (generalize the cover
to the other game variants + AI illustrations).

## 1. The variants that need a cover (Phase 2 scope)

From `Cards/Rules/Argumentum Rules - Cards.csv` (`Text` field, FR), the Tarot
Rules deck contains a cover card (#1 = École des menteurs, already styled) plus
**~6-8 distinct game variants** that are currently plain-text rule cards with no
dedicated cover art:

| # | Variant (FR title) | EN title | Has cover art today? |
|---|---|---|---|
| 1 | **L'École des menteurs** (deck cover) | The school of liars | ✅ (PR #194) |
| 2 | Le Bingo mixologie argumentative | Argumentative mixology bingo | ❌ |
| 3 | Le dernier beau parleur | The last beautiful speaker | ❌ |
| 4 | Le moulin à baratin | The Smooth-Talk Mill | ❌ |
| 5 | La parlote coinchée | The Coinched Chat | ❌ |
| 6 | (7-tour variant) | (7-turn variant) | ❌ |
| … | (a few more rule cards) | | ❌ |

(Exact count to confirm at execution time — the CSV mixes cover, setup, round,
jury and variant cards; a `## ` markdown heading parse yields the canonical list.)

## 2. Current cover mechanics (what a "generalize" would reuse)

The École des Menteurs cover (`Cards/Rules/Argumentum_Rules_fr.json`, card
`class~="1"`) is built from **CSS + 2 background images**, not from a per-variant
template:

```css
[class~="1"]            { background: url('.../bg-rules.jpg'); background-size: 100% 100%; }
[class~="1"] safe       { background: url('.../rules-kids.png') no-repeat; background-size:100%; background-position:50% 93%; }
[class~="1"] h2         { /* chalk yellow title */ }
```

- **`bg-rules.jpg`** = the black chalkboard background.
- **`rules-kids.png`** = the illustration (kids at a chalkboard) sitting in the
  `<safe>` zone.
- **Title/subtitle** = styled text on top.

**To generalize**: the structure is already a reusable "chalkboard cover"
gabarit. Each variant needs only (a) its own illustration image replacing
`rules-kids.png`, and (b) a per-variant CSS class (`class~="2"`, `="3"`…) so the
right illustration binds to the right card. The chalkboard background + title
styling can be shared.

## 3. Two sub-tracks (different gating)

### Phase 2a — Template mechanics (KEY-FREE, pipeline-side)
Wire a per-variant cover class system into the Rules template so each variant
card can carry its own illustration:

- Add `class~="2"`, `="3"`, … CSS blocks mirroring `[class~="1"]` (same
  chalkboard bg, different illustration url, same title style).
- Decide the illustration-naming convention (`rules-bingo.png`,
  `rules-moulin.png`, …) and the asset path.
- This is **pure CSS/template work** — no LLM, no image generation. Key-free.

**Apply-risk**: LOW (CSS scoped per class, no render logic change). Output
changes **only** for variant cards that get a class + illustration; cards
without stay as today.

### Phase 2b — AI illustrations (KEY-GATED, decision jsboige)
Generate one illustration per variant (~6-8 images) in the chalkboard/kids
style of `rules-kids.png`.

- **Requires**: an AI image-generation provider (decision + API key from
  jsboige). Not available on po-2024 (no image-gen key in env).
- **Style consistency** is the hard part: all illustrations must match the
  existing chalkboard aesthetic or the deck looks inconsistent.
- **Gating**: blocked on (a) jsboige picking a provider, (b) v0.9.0 release
  (Track 4 is low priority; don't add new cover art mid-release-gate).

## 4. Options

### Option A — Full Phase 2a + 2b (the issue's stated goal)
Generalize the template **and** generate AI illustrations for every variant.

- **Pro**: complete feature; consistent cover art across the deck.
- **Con**: 2b is key-gated + stylistically risky + Track 4 (low priority).
  **Not for v0.9.0.**

### Option B — Phase 2a only now (template mechanics), illustrations later
Wire the per-variant class system + naming convention now (key-free), with
**placeholder/same** illustrations, so adding AI art later is a drop-in asset
swap.

- **Pro**: unblocks the structural work key-free; illustrations become a pure
  asset-replacement PR later (no template re-touch).
- **Con**: ships "partial" covers (template-ready, art not yet) — acceptable for
  a dev milestone, not for a polished release.

### Option C — Defer entirely to v1.0
Track 4 is explicitly low priority vs release + DNN. Keep only the École des
Menteurs cover (Phase 1, done) for v0.9.0; revisit Phase 2 post-release.

- **Pro**: zero risk to the gated release; no Track 4 distraction.
- **Con**: defers the feature.

## 5. Recommendation

**Option C for v0.9.0 → Option A for post-v1.0**, because:
1. Track 4 (#188/#189) is **low priority**; the critical path is release
   validation (today) + DNN go-live. Adding new cover art mid-gate is a
   distraction and a visual-regression risk.
2. Phase 2b (AI illustrations) is **key-gated on jsboige** (provider choice +
   style consistency decision) — can't proceed autonomously anyway.
3. If jsboige wants the structural head-start, **Option B** (Phase 2a template
   mechanics, key-free) is a safe, small PR — but still best done **after** the
   release validation, not during.

## 6. Decisions needed from jsboige

1. **Phase 2 in or out of v0.9.0?** (recommendation: out — Option C now)
2. If in/post-release: **Option A (full)** or **Option B (template now, art
   later)?**
3. **AI illustration provider** for 2b? (OpenAI gpt-image / DALL·E, Midjourney,
   Stable Diffusion self-hosted, …) — style must match `rules-kids.png`
   chalkboard aesthetic.
4. **Confirm the variant list** (§1) — exact set of cards that should get a
   dedicated cover.

---

_Generated by po-2024 (worker backlog), key-free scoping. No template, asset, or
pipeline code modified. Phase 2b (AI illustrations) gated on jsboige provider
decision + key. Phase 1 referenced as resolved (PR #194) — not redone._
