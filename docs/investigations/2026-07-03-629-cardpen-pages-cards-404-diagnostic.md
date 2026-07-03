# #629 — CardPen GitHub Pages `/Cards/` 404: Diagnostic & Option Analysis

**Date:** 2026-07-03 (po-2024, base master `cb989051`)
**Milestone:** v0.9.x — Fast-follow post-tag (explicitly **post-tag**)
**Companion:** corrects/refines the root cause in the #629 issue body (code=truth).

## TL;DR

The #629 issue body states "templates Scenarii référencent `/Cards/Scenarii/Assets/...`" and cites Fallacies as "déjà fait". **The code shows the opposite and a broader scope:**

- **Scenarii Face** is **already converted** to absolute raw-master URLs (commit `09b427ef`, Feb 2026) ✅
- **6 relative `/Cards` paths remain unconverted** across **3 card types** (Fallacies Face ×4, Fallacies Back ×1, Scenarii Back ×1) ❌
- The bug is **not Scenarii-specific** — any card set with a relative `/Cards` path will 404 on GitHub Pages identically.

**Recommended fix (post-tag):** Option 1 — convert the 6 remaining relative paths to absolute raw-master URLs, exactly as `09b427ef` did for Scenarii Face. Verdict = ai-01 / jsboige.

## Verified state (code=truth)

Audited every `*_Face_*.json` / `*_Back_*.json` template in `Cards/`. Relative `/Cards` paths found:

| Template | Relative path (non-http) |
|---|---|
| `Cards/Fallacies/Argumentum_Fallacies_Face_fr.json` | `../../Cards/Fallacies/Assets/Fallacy-front/{{path}}.png` |
| `Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json` | `../../Cards/Fallacies/Assets/Fallacy-front/{{path}}.png` |
| `Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json` | `../../Cards/Fallacies/Assets/Fallacy-front/{{path}}.png` |
| `Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json` | `../../Cards/Fallacies/Assets/Fallacy-front/{{path}}.png` |
| `Cards/Fallacies/Argumentum_Fallacies_Back_fr.json` | `../../Cards/Fallacies/Assets/Fallacy-back/birds.png` |
| `Cards/Scenarii/Argumentum_Scenarii_Back_fr.json` | `../../Cards/Scenarii/Assets/Scenario-back/img-{{rowset.[0].catégorie}}.png` |

**Already absolute (good):** `Cards/Scenarii/Argumentum_Scenarii_Face_fr.json` — 7 img src, all `https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/...` (converted by `09b427ef`).

### Why local works and Pages doesn't

- **Local IIS** (`UseLocalCardpen=true`, site rooted at repo root): `../../Cards/` resolves to `/Cards/` on disk → HTTP 200 (Golden Master, Debug PASS).
- **GitHub Pages** (`UseLocalCardpen=false`): Pages publishes only the CardPen site, **not** the repo `/Cards/` tree. Both `/Cards/...` (absolute) and `../../Cards/...` (relative) resolve to a path Pages doesn't serve → HTTP 404 → 0 images → Mismatch → set failure.

### Dynamic assets verified to exist on master (conversion-safe)

The relative paths are Mustache-templated (dynamic), but the underlying files all exist on `master`:

- `Cards/Fallacies/Assets/Fallacy-front/` → **188** `{{path}}.png` files (e.g. `1.1.1.png`, `1.1.2.png`) ✅
- `Cards/Fallacies/Assets/Fallacy-back/birds.png` → 149 KB ✅
- `Cards/Scenarii/Assets/Scenario-back/img-*.png` → **7** files (`img-histoire.png`, `img-mythologie.png`, `img-politique.png`, `img-pop culture.png`, `img-relation intime.png`, `img-vie personnelle.png`, `img-vie professionnelle.png`) ✅

Mustache fills `{{path}}` / `{{rowset.[0].catégorie}}` client-side before the browser issues the HTTP request, so absolute raw-master URLs with templated fragments resolve correctly.

## Option analysis

| Option | Effort | Risk | Notes |
|---|---|---|---|
| **1. Convert 6 relative paths → absolute raw-master URLs** | **Low** (6 string edits, mechanical, mirrors `09b427ef`) | **Low** (identical proven pattern; assets verified on master) | ✅ **Recommended.** Consistent with the Scenarii Face precedent. Pins card images to `master` HEAD (acceptable: assets are stable, not versioned per-release). |
| 2. Publish `/Cards/` on Pages via `static.yml` | High | Medium | Must configure a Pages workflow to copy `Cards/` into the published site + keep path parity. Fragile coupling between repo layout and Pages site structure. |
| 3. Document local-only Release regen | **Trivial** | **None** (doc-only) | Codifies the current workaround. No Pages capability gained. Loses the ability to run a Pages-hosted Release regen. |

### Recommendation (analysis, not verdict)

**Option 1** — it is the lowest-effort path to a real fix, consistent with the already-merged `09b427ef` precedent, and unblocks Pages-hosted Release regen. Option 3 is a valid fallback if Pages-hosted regen is not a goal. **Final verdict = ai-01 / jsboige** (release-scope decision, post-tag).

### Concrete Option 1 edit (illustrative — not applied)

For each of the 6 templates, replace the relative prefix:

```
../../Cards/  →  https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/
```

e.g. Scenarii Back:
```
img-{{rowset.[0].catégorie}}.png
../../Cards/Scenarii/Assets/Scenario-back/img-{{rowset.[0].catégorie}}.png
→ https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Assets/Scenario-back/img-{{rowset.[0].catégorie}}.png
```

Caveat: pinning to `master` means a release regen always reflects the latest asset state. If asset-pinning per-release tag is later desired, the URL would need to embed the tag (e.g. `/v0.9.0/Cards/...`) — out of scope for #629, defer.

## What is NOT in scope here

- Applying any edit (post-tag decision).
- The Fallacies Back `birds.png` asset purpose (decorative; confirmed present).
- Non-FR language templates (the `_fr` files are the harvested ones; if other langs have separate templates with relative paths, they'd need the same audit — but the pipeline localizes via `{{text_xx}}` field swaps, not separate template files, so `_fr` is representative).

## Verification plan (when the fix is applied post-tag)

1. Apply Option 1 to the 6 templates on a feature branch.
2. Run a Release regen with `UseLocalCardpen=false` (Pages) on a small subset (Scenarii + Fallacies, FR only) → confirm no 404s, image count matches.
3. Spot-check 2 cards (one Fallacies with `{{path}}`, one Scenarii with `{{catégorie}}`) render the image.
4. Confirm local IIS regen still PASS (no regression).

Relates #629, #134, #190, #613. Base master `cb989051`.
