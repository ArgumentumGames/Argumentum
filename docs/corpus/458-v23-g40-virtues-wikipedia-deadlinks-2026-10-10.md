# #458 — Virtues: Wikipedia link census and dead-cell repair (grain ③)

**Date:** 2026-10-10 · **Lane:** po-2024 (worker, `myia-po-2024`) · **Dispatch:** ai-01, #458 c.6090687351
**Scope:** `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (`link_{fr,en,ru,pt,ar,es,zh,fa}`)

---

## 1. The dispatched question

The dispatch asked to treat **8 cells** — `link_fr`/`link_en`/`link_ru`/`link_es` on PK 203 and 204,
whose URLs (`Méthode_PRIOR`, `PRIOR_method`, `Метод_PRIOR`, `Método_PRIOR`, `METHOD_PRIOR`) return 404 —
and to **say which of two explanations holds**: the pages died since they were written, or the probe that
cleared them had a blind spot. If it was a blind spot, the dispatch ordered the census to be replayed on
`wikipedia.org`, with witnesses.

**Answer, measured: it is a blind spot.** The explanation is written in the body of the very PR that
performed the earlier clearing (#1442):

> *"The full non-Wikipedia tail of the Virtues taxonomy (58 URLs) was probed exhaustively; only the 25 dead
> ones are touched. **Wikipedia URLs were covered by the earlier deck-wide probe (200s).**"*

Wikipedia URLs were excluded as a class on the strength of one earlier probe. That premise is false, and the
census below measures how false.

## 2. Instruments, and the three defects found in them

A census is only worth what its probe is worth. Three defects were found and fixed **before** any verdict was
used — each one would have produced a false result:

| # | Defect | Symptom it fabricates | Fix |
|---|---|---|---|
| 1 | `urllib` cannot request a URL holding raw non-ASCII (`ru`, `ar`, `zh`, `fa` literal paths) | `UnicodeEncodeError` on 437 of 774 URLs — **no verdict**, not "dead" | percent-encode before the request; the 437 were re-probed, **0 remain without a verdict** |
| 2 | The Wayback availability API is **scheme-sensitive** | `https://fr.wikipedia.org/wiki/Sophisme` → *no snapshot*; `http://…` (same page) → **snapshot found**. A one-scheme probe invents `NONE` for everything archived over `http` | probe **both** schemes |
| 3 | `list=logevents` with `letype=delete\|move\|create` returns an API error (no `query` key) | an empty list that reads as "no such event" | **one `letype` per call**; calibrated on a deletion read from the log one instant earlier |

A fourth, subtler point: the witness must be **proven capable of answering the positive case**. A first runner
used `fr:Sophisme` as its witness — a live page that happens to have **no snapshot**, so `witness_ok()` was
always false and the run aborted itself at the first call. It was reported as an archive.org outage; it was an
instrument defect. A witness that can only ever say "no" is not a witness.

An earlier conclusion recorded during this grain — "the 5 PRIOR titles have NO LOG EVENTS, therefore they never
existed" — rested on that uncalibrated instrument and is **withdrawn as stated**; the census below re-establishes
it on a calibrated one.

## 3. Census — 774 distinct Wikipedia URLs, 1 139 cells

Every distinct `wikipedia.org` URL in the eight `link_*` columns, probed with retries, redirect-following,
a witness every 40 URLs, and 0.35 s throttle. `incomplete_at: None` — the run finished; no verdict is missing.

| | URLs | Cells |
|---|---:|---:|
| Alive (200) | 570 | 893 |
| **Dead (404)** | **204** | **246** |
| No verdict | 0 | 0 |
| **Total** | **774** | **1 139** |

Dead-cell rate per column — **every language is affected, none is clean**:

| `link_fr` | `link_pt` | `link_ru` | `link_ar` | `link_es` | `link_zh` | `link_fa` | `link_en` |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 59/161 = **36.6 %** | 49/186 = 26.3 % | 45/172 = 26.2 % | 20/92 = 21.7 % | 26/146 = 17.8 % | 17/105 = 16.2 % | 13/92 = 14.1 % | 17/185 = 9.2 % |

**246 dead cells sit on 92 of the 223 nodes.** On **8 nodes every filled link is dead** — the node was written
whole, in 1 to 5 languages, pointing at articles that do not exist:

| PK | Node | Dead / filled links |
|---|---|---:|
| 59 | Sens quantitatif | 5/5 (fr, en, ru, pt, es) |
| 70 | Support fini | 5/5 (fr, en, ru, pt, es) |
| 171 | Conscience interculturelle | 5/5 (fr, en, ru, pt, ar) |
| 119 | Syllogisme Datisi | 4/4 (fr, en, ru, pt) |
| 203 | Concentration sur l'essentiel | 4/4 (fr, en, ru, es) |
| 204 | Gérer le temps imparti | 4/4 (fr, en, ru, es) |
| 63 | Transfert licite | 3/3 (fr, ru, es) |
| 23 | Preuves empiriques | 1/1 (pt) |

## 4. Root cause — an LLM was asked for URLs, and a fill rate was read as a validity rate

The values are not rot; they were **generated**. Two commits name the mechanism:

- `04a30838` — *chore(dataset-updater): refine Virtues EN/PT/RU prompts + complete translations* (#236):
  introduces `PRIOR_method`, `Открытость_культуре`, `Mathematical_method` into the CSV.
- `43941040` — **feat(dataset-updater): add Virtues PT Wikipedia links task (186/223 = 83.4 %)** (#246):
  a DatasetUpdater task whose **only job is to write `link_pt`**, plus `PromptVirtuesLinksPtUser.txt`.

PR #246's own body states the instruction given to the model and the verdict passed on its output:

> *"New prompt pair … instructs model to use Wikipedia interlanguage links and **leave empty if PT article
> doesn't exist**"* … *"**186/223 Virtues entries now have valid `link_pt`** (83.4 % coverage, up from ~0 %)"*

Two failures compose here. The model **ignored an explicit "leave empty" instruction** and produced plausible
titles instead — the classic hallucinated-citation mode. And the receiving lane called the result **"valid"**
without resolving a single URL: the only test applied was *"the model returned a non-empty string"*. Measured
today, **49 of those 186 "valid" links (26.3 %) do not resolve**.

> **A fill rate is not a validity rate. A generated URL is a claim until it resolves.**

The generator is dormant — every DatasetUpdater task is `Enabled = false` today. The cause is therefore not
live, but nothing in the tree prevents a future run from re-fabricating (see §9).

## 5. Why it stayed invisible — a blind spot inside the guard itself

Two independent mechanisms hid this, and both are measurable on the current tree:

1. **The class was excluded by an unverified claim** (§1): Wikipedia URLs were skipped because an earlier probe
   had found 200s.
2. **The mindmap guard's dictionary is frozen.** `MindmapDeadLinkGateTests.ClearedVirtuesLinks` is a literal
   list written once, from the repair commits of #1442/#1621. A guard whose expected set is a hand-written
   constant covers exactly that constant — **and nothing else**. Measured on the committed mindmaps:

   | | |
   |---|---:|
   | Dead URLs from this census present in the shipped mindmaps | **204 / 204** |
   | Bare occurrences (no wayback prefix) | **444** |
   | Occurrences the always-on gate can see | **0** — none of these URLs is in its list |

   The gate is green while 444 dead links ship. This is not a defect the gate *failed* to catch; it is the
   gate's **coverage boundary**, which only its literal list defines.

3. **The `Link<Lang>Fallback` cascade amplifies a single dead cell.** The link rendered for a language is
   `link_<lang> ?? link_en ?? link_fr`, so a fabricated `link_fr` or `link_en` value propagates into every
   language that has no link of its own — measured up to **18 files** for one URL (`Transfert_(logique)`)
   and **27** for `Constructive_discussion`.

## 6. Dead on arrival — no dead link in this set was ever alive

"Link rot" implies a link that worked and later decayed. The dates refute that reading:

| | |
|---|---|
| Link values written | **2026-04-26** (`04a30838`, #236) and **2026-05-04** (`43941040`, #246) |
| Pages that demonstrably existed, and when they were removed (11) | 2007-08-26 · 2011-09-05 · 2013-11-18 · 2014-04-10 · 2014-11-25 · 2016-05-09 · 2017-07-08 · 2021-03-20 · 2022-12-26 · 2023-01-24 · 2025-05-30 |

**Every one of those eleven removals predates the writing of the link** — the latest falls thirteen months before
the first link value was committed. Of the whole dead set, not a single cell was a working link at the moment it
was created: the URLs were **dead on arrival**. The defect is not decay, it is **fabrication at generation
time** — which is also why no amount of waiting or re-checking would have revealed anything, and why the only
remedy is to resolve every generated URL.

The deletion comments also dispose of the mirror question: vandalism and test pages (`es:Dramatización` 2007,
`ru:С1` short-articles 2014), copyright violations (`ru:Аналитическое_мышление` 2014, `fa:کرامت_انسان` 2017), pages
created by a banned editor (`pt:Argumento_válido` 2023), a no-context stub (`pt:Terminologia_técnica` 2022),
unsourced or unverifiable pages (`fr:Proposition_(logique)` 2025, `pt:Compromisso` 2021). One of the eleven keeps a
link (PK 198) — not because the page was a good reference, but because a snapshot survives AND serves the article
body; `es:Dramatización` has a 2008 capture too, and it is **not** mirrored, because that capture carries no
article body (the page it froze was itself delete-tagged).

## 7. Treatment

Doctrine applied is #1442's, in the order that serves the reader — **a live page beats a snapshot of a dead
one**, so the live-equivalent branch is tried first and the doctrine's stated order is deliberately inverted
here (declared deviation):

1. **Live equivalent**, accepted only when produced by a **mechanical name transformation** of the dead title
   (unfold or drop a parenthetical), returning 200, with **identical token sets** after accent-folding, and not
   a disambiguation page. Anything requiring judgement — a synonym, a neighbouring concept, a search hit — is
   **rejected**: substituting an approximate page is the anti-pattern #1438 exists to remove. An equivalence
   finder that used a search-based rule was measured rejecting the *correct* candidate and accepting a
   disambiguation page, and was discarded for that reason.
2. **Wayback snapshot**, mirroring the archived URL — probed under **both schemes** (§2 defect 2).
3. **Otherwise the cell is emptied**, and the removed value is recorded below. This is the same action #1442
   took for its 25 URLs, and "empty" is the representation already used by **589 `link_*` cells** in this file.

### What each class received

| Class | URLs | Cells | Treatment |
|---|---:|---:|---|
| **Never existed** — no `create`, `delete` or `move` event in that wiki's logs; the title is an artefact of the generating model | 193 | 234 | **5** rewritten to a live title · **229** emptied |
| **Existed, then removed** — deletion logged and dated, 2007-08-26 … 2025-05-30, always *before* the link was written (§6) | 11 | 12 | **1** mirrored to a snapshot that serves the article body · **11** emptied |
| | **204** | **246** | **6 rewritten · 240 emptied** |

The classes partition the census exactly — 234 + 12 = 246 cells, 5 + 1 = 6 rewritten, 229 + 11 = 240 emptied —
so no cell is counted in two classes and none is unaccounted for.

The six rewritten cells are the only ones in the whole set where the substitute is demonstrably the same subject:

| PK | Cell | Was (404) | Now |
|---|---|---|---|
| 98 | `link_fr` | `Conjonction_(logique)` | `Conjonction_logique` |
| 98 | `link_pt` | `Conjunção_(lógica)` | `Conjun%C3%A7%C3%A3o_l%C3%B3gica` |
| 99 | `link_fr` | `Disjonction_(logique)` | `Disjonction_logique` |
| 99 | `link_pt` | `Disjunção_(lógica)` | `Disjun%C3%A7%C3%A3o_l%C3%B3gica` |
| 100 | `link_fr` | `Négation_(logique)#Double_négation` | `Négation_logique` |
| 198 | `link_pt` | `Compromisso` | `web.archive.org/web/20201225225451/https://pt.wikipedia.org/wiki/Compromisso` |

Five are the same page under its current title (a parenthetical dropped, or an anchor replaced by the article that now
carries the content): each returns 200, each is token-identical to the dead title after accent folding, and none is a
disambiguation page. The sixth is the single usable mirror (§6).

### How the classes were established

A 404 alone does not say whether a page died or never existed, and the two demand opposite treatment — so all 204 were
put to a second instrument: the wiki's own log (`list=logevents`, one `letype` per call, §2 defect 3) read together with
the Wayback availability API **under both schemes** (§2 defect 2). **204 of 204 settled, zero instrument errors.** The
run's HTTP 429s were retried at a slower cadence and resolved; a transient error is retried, never frozen into a verdict.

Cross-instrument control, on the 87 URLs where both instruments spoke: `snapshot ∧ never-existed = 0`. No URL is
archived without its page having existed, which is what two instruments measuring the same world must produce — a
non-zero here would have meant one of them was wrong.

### Why only 1 of the 11 removals is mirrored

A snapshot is a reference only if it *serves the article*. Every candidate snapshot was fetched and its body inspected:
10 of the 11 hold none — redirects, deletion notices, vandalism-tagged pages — and only `pt:Compromisso` serves the
article. `es:Dramatización` is the instructive case: a 2008 capture **exists**, but what it froze was a page already
tagged for deletion, so it carries no body and was not used. Mirroring on the mere existence of a snapshot would have
shipped eleven references to deletion notices.

### Verified on the committed artefact

| | |
|---|---|
| Size | 930 469 → **916 964** bytes |
| Preserved | BOM · 224 CRLF (0 bare LF) · 224 rows · **82 fields on every row** |
| Cells | 246 link cells changed (6 rewritten, 240 emptied) on **92 rows**; **0 non-`link_*` cells changed** |
| Emptied cells | 589 → **829** (of 1 784), the representation already used by the file for a link it does not have |
| Dead URLs left in any `link_*` cell | **0** |

## 8. Impact

Measured through the `Link<Lang>Fallback` cascade, i.e. on what a reader would actually get:

| | Before | After |
|---|---:|---:|
| **Nodes a reader could follow a link from** (≥ 1 resolving, any language) | 210/223 = 94.2 % | 202/223 = 90.6 % |
| Nodes with **no link at all** (no cell filled in any language) | 13 | 21 |

Per language — nodes whose *rendered* link resolves, out of 223 (`Link<Lang>Fallback` applied):

| `link_fr` | `link_en` | `link_ru` | `link_pt` | `link_ar` | `link_es` | `link_zh` | `link_fa` |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 205 → 187 | 205 → 187 | 207 → 194 | 208 → 190 | 205 → 188 | 206 → 191 | 205 → 190 | 205 → 188 |

**8 nodes newly keep no link** (23, 59, 63, 70, 119, 171, 203, 204) — seven of them because every link they
carried was fabricated (§3), one (23) because its single link was. Three figures here answer three different
questions and must not be added or substituted for one another:

- **210 → 202** counts nodes with a *resolving* link;
- **13 → 21** counts nodes with *no cell filled at all*;
- #1442's body lists PK 23 among nodes *"already linkless"* — a **live**-link count, under which a filled-but-dead
  cell reads as absent. PK 23 was filled (one dead `link_pt`), so it appears here as newly empty, not as
  previously empty. All three are true; none of them is the other.

## 9. What this does not establish, and what must follow

- **The mindmaps are not re-derived here.** The committed SVGs and inlining HTML wrappers still carry the dead
  URLs (444 bare occurrences) until a re-derivation runs — that step needs FreeMind's GUI and is ai-01's lane.
- **`ClearedVirtuesLinks` is deliberately NOT updated in this PR.** Adding these URLs to the always-on gate
  **before** the mindmaps are re-derived turns CI red on a correct tree: the gate asserts the URLs' *absence
  from the shipped mindmaps*, and the shipped mindmaps still contain them (§5.2). The guard patch and the
  re-derivation must land **together**. The exact list to add is the dead-URL set recorded in this doc.
- **No prevention organ ships here either.** The hole §5.2 measures is structural: a guard whose expected set
  is a hand-written constant is blind by construction to everything added later. The offline remedy is a
  **census-derived** organ — commit the URL→verdict map, and assert that every `link_*` value in the taxonomy
  appears in it with a resolving verdict, so that a new link cannot land un-probed and the census must be
  refreshed deliberately. This is a code change with its own proof and is left to a dedicated grain.
- **Editorial replacement is not attempted** beyond mechanical name transformations. The 8 nodes that become
  linkless, and the 246 emptied cells, are recorded so that a human can source better links deliberately.

## 10. Reproduction

```bash
# census (774 URLs, witness every 40, both-scheme wayback, calibrated logevents)
python -I <scratchpad>/wiki_census.py && python -I <scratchpad>/wiki_reprobe.py
# discrimination of the dead set
python -I <scratchpad>/discriminate.py
# byte-exact cell clearing by field span (self-test + dry-run before --apply)
python -I <scratchpad>/clear_cells.py --csv "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv" --plan <plan>.json
```

The clearing tool walks the CSV as **fields with spans**, never by string replacement: two rows can hold the
same URL (measured: `Fiabilité_des_sources` on PK 13 and PK 14), so a text-level edit contaminates a
neighbouring cell. Before writing it asserts, per cell, that the span holds the expected value byte-for-byte,
and afterwards that every row still parses to the same number of fields.
