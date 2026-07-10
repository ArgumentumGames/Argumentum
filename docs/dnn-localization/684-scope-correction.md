# 2026-07-10 — #684 Rule prose translation — SCOPE CORRECTION (live 2sxc v21 export #681)

**Scope**: scope correction of [`684-translation-manifest.md`](684-translation-manifest.md) (PR #696,
2026-07-05) in light of the **live 2sxc v21 export** (#681, delivered 2026-07-08, read-only Method B SQL).
This is an **addendum / supersede-note**, not a rewrite — the manifest's *structure* (6 prose fields × 7
langs, Scenarii-style chunking, gpt-5.5, re-import plan) **stands and is confirmed**; this doc corrects
the *volume estimate* (~30 / ~210 → **23 populated / 161**) by answering the manifest's own deferred
question ("is it 5 games? do all games have all 6 fields populated?") with export ground truth.

**Repo reference**: master `053257c7`. Issue: #684 (rule prose translation). Triggered by ai-01 dispatch
`1gmve4` (2026-07-10, primaire — #684 scope correction). Owner: jsboige (DB re-import) + worker (analysis =
this doc). **Analysis-only — no DB write, no translation executed** (export = read-only SELECT).

**Export source**: `.shared-state/attachments/DNN-Argumentum-export-2026-07-07/12-game-rule-content-items.json`
(Game Rule content-type, AttributeSet 377, app=60, `entityCount=5`).

---

## TL;DR — 3 corrections, structure unchanged

The manifest's *structure* is confirmed. What the export resolves:

| Delta | Manifest #696 (estimate) | Revised (live export #681) |
|---|---|---|
| **Game count** | "~5 games" (gated, uncertain) | **5 games confirmed** (`entityCount=5`, all `IsPublished=True`) — matches #662's "5/5 jeux FR présents" |
| **Prose cells populated** | "~30 entities" (6 fields × ~5 games, assumed full) | **23 / 30 populated** — 7 cells are **structurally empty** (games with no Variants/Memo), not data gaps |
| **Translation volume** | "~210 units" (30 × 7 langs) | **161 units** (23 populated × 7 langs) — **−23 %** vs estimate |
| **DB seed/migration needed?** | (implicit — "DB in seed/pre-migration state", per #682 revision §4) | **No** — the 5 published games are the **complete intended set** (#662: "0 rule missing"); no seeding before translation |

➡️ **The #684 translation task is ~23 % smaller than estimated, with no migration prerequisite.** It
proceeds against the **5 published entities directly** (FR dimensionless source, net-new translation).

---

## §1 — The 5 published rules (code=truth, from `12-game-rule-content-items.json`)

| # | EntityID | IsPublished | Title (FR canonical) | Source of `Variants`/`Memo` |
|---|----------|-------------|----------------------|-----------------------------|
| 1 | 11378 | True | L'école des menteurs | both present |
| 2 | 11380 | True | Le Bingo mixologie argumentative | neither |
| 3 | 11387 | True | Le dernier beau parleur | neither |
| 4 | 11388 | True | Le moulin à baratin | Memo empty |
| 5 | 11389 | True | La parlote coinchée | neither |

- All 5 are **published** (`IsPublished=True`), owner `dnn:userid=1`.
- All values are **FR dimensionless** (`Lang=None`, `DimensionID=None`) — i.e. **0 translation exists yet**
  (the 7 target langs are net-new). This confirms #684 = translation of FR source, nothing reusable.
- The 5 titles match #662's scraped `/Règles` page (L'école des menteurs / Bingo / Dernier beau parleur /
  Moulin à baratin / Parlote coinchée) — **cross-validated**, export == live site.

---

## §2 — Field completeness matrix (23/30 — the 7 empty cells are STRUCTURAL)

Per-entity prose-field population (char count of populated cells):

| # | Game | Summary | Material | Installation | Content | Variants | Memo |
|---|------|--------|----------|--------------|---------|----------|------|
| 1 | L'école des menteurs | 774 | 336 | 1070 | **15653** | 1112 | 278 |
| 2 | Le Bingo mixologie… | 425 | 340 | 307 | 1545 | _empty_ | _—_ |
| 3 | Le dernier beau parleur | 387 | 336 | 781 | 2231 | _empty_ | _—_ |
| 4 | Le moulin à baratin | 178 | 336 | 676 | 1014 | 369 | _empty_ |
| 5 | La parlote coinchée | 598 | 271 | 366 | **7790** | _empty_ | _—_ |
| | **Populated** | **5/5** | **5/5** | **5/5** | **5/5** | **2/5** | **1/5** |

**23 / 30 prose cells populated.** The 7 empty cells:

- **Variants empty (3 games)**: Bingo / Dernier beau parleur / Parlote — these 3 games legitimately have
  **no game variants** (not a translation gap — the field is blank in FR too).
- **Memo missing/empty (4 games)**: only "L'école des menteurs" has a Memo card (278 chars); the other 4
  have **no memo card** (Memo field absent or blank in FR).

➡️ These 7 are **structural variation** (some games have no variants / no memo card), **NOT missing
translations**. The #684 translation task therefore covers **only the 23 populated FR cells** — translating
the 7 empty ones would fabricate content. Per `[[csv-byte-exact-column-insertion]]` discipline (no
fabrication), the empty cells stay empty across all langs.

> **Note on `Content` (the bulk field)**: 5/5 populated, ranging 1014–15653 chars. This is the dominant
> per-entity cost (the manifest's "minutes-to-tens-of-minutes gpt-5.5 per lang" estimate holds for
> `Content`; the other 5 fields are short). L'école des menteurs (15653) + La parlote coinchée (7790) are
> the two heaviest entities.

---

## §3 — Corrected volume estimate (161 translation units, not ~210)

| Metric | Manifest #696 | Revised (export) | Δ |
|--------|---------------|------------------|---|
| Game entities | ~5 | **5** | confirmed |
| Prose cells (populated) | ~30 (assumed full) | **23** | −7 (structural) |
| × 7 target langs | ~210 | **161** | **−23 %** |
| + EntityTitle short strings | ~5 × 7 = 35 | **5 × 7 = 35** | unchanged (all 5 titles present) |
| **Total translation units** | **~245** | **~196** (161 prose + 35 title) | **−20 %** |

➡️ The dominant-cost estimate (#669 §3) drops ~20 %. Still the largest single portage item, but smaller
than planned. **No re-baseline of the chunking strategy** needed (Scenarii-style gpt-5.5, per-language
pass — the manifest §2 holds; just fewer units per pass).

---

## §4 — "DB seed/pre-migration" — CLARIFIED: no migration needed

The #682 revision §4 note ("DB in seed/pre-migration state") and the dispatch's "documenter le DB
seed/pre-migration nécessaire" warranted a definitive answer. The export + #662 resolve it:

- **#662 audit verdict** (2026-07-03, read-only prod scrape): *"0 rule missing. The restructure #438/#250
  (re-pagination 24→15 cards) did not drop any game on prod: the 4 variants are all published alongside the
  main game."* → all 5 games are present and published.
- **Export confirms**: `entityCount=5`, all `IsPublished=True`, FR dimensionless.
- **The "24" / "15" were CARD counts, not game counts**: the Rules CSV (`Argumentum Rules - Cards.csv`)
  has **15 records** after the #438/#250 re-pagination (was 24 before). These 15 **cards** map to **5
  games** (some games span multiple cards — e.g. "Rules_09–10 Le dernier beau parleur (variant)",
  "Rules_13–15 La parlote coinchée (variant)"). The manifest's "~30 entities" conflated card-level and
  game-level; the translation scope is at the **game-entity level = 5 games**.

➡️ **No DB seed or migration is needed before #684 translation.** The 5 published games are the complete
intended set. If jsboige later wants to publish MORE games, that is a separate **content-authoring** step
(write new FR rule prose → publish entity → then translate) — out of #684 scope (#684 = translate what
exists, not author new). The translation proceeds against the 5 existing entities directly.

---

## §5 — Reconciliation with the manifest (#696)

[ `684-translation-manifest.md`](684-translation-manifest.md) sections:

| § | Status | Action |
|---|--------|--------|
| TL;DR volume (~30 / ~210) | ⚠ **revised** → 23 populated / 161 units (this doc §3) | update on next manifest touch |
| §1 Entity inventory ("~5 games, gated") | ✅ **confirmed** → 5 games (§1) + 23/30 completeness matrix (§2) | this doc is the resolution |
| §1 "Honest uncertainty" block | ✅ **resolved** — the deferred question ("5 games? all 6 fields?") is answered: 5 games, 23/30 populated | — |
| §2 Chunking strategy (Scenarii-style, gpt-5.5, per-lang pass) | ✅ **unchanged** — holds (fewer units per pass) | — |
| §3 DatasetUpdater task sketch | ✅ **unchanged** — `SourceFilePath` = export file (now confirmed available) | unblock: source path no longer "jsboige-gated", export #681 delivered |
| §4 Re-import plan | ✅ **unchanged** — holds (entity-count parity = 5; verify no field loss) | — |
| §5 Effort estimate | ⚠ **revised down ~20 %** | update on next touch |

➡️ **The manifest does NOT need a rewrite** — this addendum supersedes its volume figures. A future touch
can fold §3/§5 numbers into the manifest; until then, this doc is the authoritative scope.

---

## §6 — DoD status (dispatch `1gmve4` primaire)

| DoD item | Status |
|----------|--------|
| Confirm 5 rules published (not 24-30) | ✅ `entityCount=5`, all `IsPublished=True` (§1); "24-30" was card-level count, game-level = 5 (§4) |
| Document DB seed/pre-migration mechanism | ✅ **no migration needed** — 5 games = complete set (#662 "0 rule missing", §4) |
| Doc `docs/dnn-684-scope.md` | ✅ this doc (`684-scope-correction.md`, addendum to manifest) |
| PR docs-only | ✅ this PR |
| 0 write DNN prod | ✅ export = Method B read-only SELECT |

**Bonus finding**: corrected translation volume **~196 units (161 prose + 35 title), −20 % vs estimate**.
No fabrication: the 7 structurally-empty cells stay empty across langs.

---

## §7 — Gate boundaries (HARD — analysis only)

- ❌ No DB write, no translation executed, no prod interaction (export = read-only Method B SELECT).
- ❌ No mutation of the manifest (#696) — this is an addendum; folding numbers in is a future touch.
- ❌ No content fabrication — the 7 empty cells (no Variants/Memo for some games) stay empty.
- ✅ Scope confirmed by live export #681 (ground truth) + cross-validated vs #662 prod scrape (5/5).
- ✅ Honest about the "24-30" origin (card-level repagination #438/#250, not a game undercount) — the
  manifest's own "~5 estimate" was right; the volume figure was the loose part.

Relates: dispatch `1gmve4` (primary), #684 (this issue), #681 (export, ground truth), #696 (manifest,
addendum'd), #692 (Rules coverage CSV-vs-site prep), #662 (prod coverage audit — "0 rule missing"),
#438/#250 (card re-pagination 24→15), #682 (field-model revision — "5 published" note, clarified here),
#669 (mechanism), #458 (epic Track 2).
