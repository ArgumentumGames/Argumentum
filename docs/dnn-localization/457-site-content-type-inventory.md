# #457 — Site Content-Type Inventory (localization scope map)

**Issue:** [#457 — DNN site localization epic](https://github.com/ArgumentumGames/Argumentum/issues/457)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17
**Base:** master `010ea589`
**Status:** SCOPE-MAP (docs, non-gated). Extends [PHASE1-content-audit.md](PHASE1-content-audit.md) and feeds the [extractor](../../tools/dnn_i18n/extract_dnn_ui_strings.py) (PR #524, merged). **No prod mutation, no DB/RDP.**

---

## 1. Purpose

A complete map of **what DNN site content is localizable, where it lives (repo vs DB), and the
status of each**. This closes the "inventory content-types" idle task (ai-01 dispatch
`msg-…141408`) and tells jsboige exactly what the extractor can reach today vs what needs the
portal export (the true unblocker for bulk translation).

The scope split is **Argumentum-app content only** — the DNN framework (PersonaBar admin HTML,
`admin/Modules/App_LocalResources/*.resx`, skin objects) ships its own official language packs
and is explicitly out of scope (PHASE1 §TL;DR).

## 2. The inventory (5 content-types)

| # | Content-type | Where | In repo? | Volume | Extractor status |
|---|--------------|-------|----------|--------|------------------|
| **A** | Custom template **hardcoded UI strings** | `Portals/1/2sxc/Argumentum/*.cshtml` | ✅ Yes | 2 keys (`ui.*`) | ✅ Done — `extract_dnn_ui_strings.py` (`ui.*` branch) |
| **B** | Custom template **`@Resources.*` keys** | `Portals/1/2sxc/Argumentum/_RulesExplorer_RuleDetail.cshtml` (+1) | ⚠️ Keys yes, **values DB-only** | 8 keys (`res.*`) | ✅ Keys done — extractor emits key + flags value DB-only |
| **C** | **Static HTML content pages** | `DNNPlatform/fallacies/*.html` (2 files) | ✅ Yes | ~24K + ~13K visible chars (FR prose) | 🆕 **NOT extracted** — see §3 |
| **D** | 2sxc **content items** (glossary, FAQ, homepage, per-rule content, App resource VALUES) | SQL (2sxc App Resources) | ❌ DB-only | unknown (the bulk) | ❌ Blocked — needs portal/2sxc export (jsboige, gated) |
| **E** | DNN **page settings** (titles, SEO meta, nav labels) | SQL | ❌ DB-only | unknown | ❌ Blocked — needs portal export |
| — | Framework `.resx` / skin objects | `admin/Modules/`, `Portals/_default/Skins/`, `Portals/1-System/Skins/` | ✅ Yes (202 HTML + many .resx) | framework | OUT OF SCOPE — DNN official language packs |

**Net repo-extractable today: A + B (10 keys) + C (2 HTML pages).** D + E are the bulk and
require the portal export — the single unblocker for Phases 2-3 of the epic.

## 3. Content-type C — the 2 static HTML pages (the repo-extractable gap)

PHASE1 §TL;DR deferred these ("2 files, full extraction deferred pending live-nav confirmation").
They ARE in the repo, ARE Argumentum-specific FR content, and ARE the largest repo-extractable
chunk:

| File | Bytes | Visible text (FR) | Structure | Role |
|------|-------|-------------------|-----------|------|
| `DNNPlatform/fallacies/fallacies.html` | 81 KB | ~24,200 chars | 2 `<h2>` + 2 `<h3>` + 10 `<li>` (a structured **integration charte** — "Charte Html pour l'identification des arguments fallacieux") | Educational integration doc with worked fallacy examples |
| `DNNPlatform/fallacies/MariagePourTous.html` | 38 KB | ~13,100 chars | prose, no semantic headings | Standalone analysis article ("L'analyse rhétologique pour tous") |

These are **standalone FR content**, not CSV-driven (no `fetch()`/data-src). Translating them is
a prose task (gpt-5.5), not a key-value dictionary task — so they'd use a **different
DatasetUpdater task shape** (chunked document translation, like Scenarii) rather than the
`ui.*/res.*` string task (#487). They are NOT wired today.

**Recommendation (for jsboige):** treat C as a **separate translation lane** (document-tier, not
string-tier). The extractor (PR #524) is scoped to A/B; extending it to C is a follow-up that
depends on a content-extraction approach (HTML→text segment), not the `.cshtml` anchor method.

## 4. The 2sxc app landscape (26 apps, 1 custom)

The portal runs **26 2sxc apps** (`Portals/1/2sxc/`): **Argumentum** (custom, the 4 bespoke
`.cshtml`) + **25 stock 2sxc apps** (Accordion4, Blog5, News5, Glossary3, Faq4, etc.).

- Only **Argumentum** carries bespoke translatable templates (A + B above).
- **Glossary3** has an `app.xml` (version history → 14.09.00) — its glossary term VALUES are
  content-type D (DB-only), not in the repo.
- The 24 other stock apps are 2sxc-bundled (their templates ship in the 2sxc distribution, not
  Argumentum's localization scope).

This confirms PHASE1: the repo holds **application templates**, not portal content.

## 5. The "Materiel" coquille (jsboige flag) — located?

Dashboard tracks a live coquille « Materiel » (missing accent, should be « Matériel »).
**Searched the Argumentum-app scope (`Portals/1/2sxc/Argumentum/`) + the 2 fallacies HTML → not
found.** The coquille is therefore almost certainly in **content-type D** (2sxc App Resources /
DB content) — consistent with the pattern appris « DNN i18n »: canonical FR values live in SQL,
not in repo templates. **Fixing it requires the portal/DB export (jsboige, gated).** This is a
data-point that the DB-only content bucket (D) is where live-site text corrections must happen.

## 6. What this changes

- The extractor (PR #524) already covers A + B (the repo-extractable *string-tier* content).
- **C (HTML pages) is a real, separate repo-extractable lane** — not wired, recommend a
  document-tier translation task for it (follow-up, gated on jsboige deciding whether these
  long-form FR articles are in v0.9.0 scope).
- **D + E remain the bulk unblocker** — every live-site text correction (incl. the « Materiel »
  coquille) and the bulk of translation surface lives in SQL and needs the portal export.
- The **« Materiel » coquille is DB-only** (not in any repo template) — confirmed by negative
  grep across the Argumentum-app scope.

## 7. Gate boundaries

- ❌ No prod mutation, no DB/RDP, no portal export attempted.
- ❌ Does not touch `dnn-ui-strings.csv` (po-2024 lane, #490) or enable the DatasetUpdater task.
- ❌ Does not declare a QA verdict (ai-01 only). Pure scope-map doc.

## Sources

- Repo: `DNNPlatform/Portals/1/2sxc/` (26 apps), `DNNPlatform/fallacies/*.html` (2 pages),
  `DNNPlatform/Portals/1/2sxc/Argumentum/*.cshtml` (4 templates).
- [PHASE1-content-audit.md](PHASE1-content-audit.md) (#461, merged `5550d294`) — the manual audit
  this inventory extends.
- PR #524 (merged `010ea589`) — the extractor (bricks 1 + 3) covering content-types A + B.
- #487 (merged `ca9a8640`) — the Option-C DatasetUpdater config (`Enabled=false`) for A/B strings.
