# 2026-07-03 — DNN prod rules coverage audit (read-only)

**Scope**: Read-only audit of the **production DNN site** (`https://dnn.argumentum.myia.io`) to answer jsboige's concern (2026-07-03, verified interactive via ai-01): *"it is possible that some rules were not ported to the prod site."*

**Dispatch**: ai-01 → po-2023 (`msg-20260703T095500-5wdug6`, TASK). DoD: coverage doc + clear verdict "the 4 variants ARE / ARE NOT on prod", per language.

**Repo reference**: `Cards/Rules/Argumentum Rules - Cards.csv` @ master `9c19e51a` — 15 records, 10 columns (`pk`, `Text`, `Text_en`, `Text_ru`, `Text_pt`, `Text_ar`, `Text_es`, `Text_zh`, `Text_fa`, `print_and_play`).

---

## TL;DR — Verdict

> **The 4 variants + the main game ARE on prod (FR). No rule was lost.**
> **But the prod site publishes the rules in FRENCH ONLY.** The 7 translated languages present in the repo (en/ru/pt/ar/es/zh/fa) are **NOT published** on the site.

jsboige's concern (rules missing on prod) is **refuted for content**: all 5 games are present. A **separate, real gap** exists on the multilingual axis (FR-only publication), which is a content-portage task, not a data-loss issue.

---

## 1. Production site — rules page located

| Probe | Result |
|-------|--------|
| `https://dnn.argumentum.myia.io/Règles` | **HTTP 200** (canonical rules page) |
| `/Argumentum/Règles` | HTTP 301 → `/Règles` |
| `/Argumentum/Rules` | HTTP 200 (alias) |
| `/Regles`, `/rules`, `/Le-jeu` | HTTP 404 (not separate pages) |

**Page size**: ~52 KB of rendered HTML (fully populated, not a placeholder/error).

## 2. Content — 5/5 games PRESENT (French)

Scraped from `/Règles` (WebFetch + raw-HTML title grep). All 5 rule sets from the repo are published, in French:

| Records (repo) | Game | On prod |
|----------------|------|---------|
| Rules_01–06 | **L'école des menteurs** (main game) | ✅ present |
| Rules_07–08 | **Le Bingo mixologie argumentative** (variant) | ✅ present |
| Rules_09–10 | **Le dernier beau parleur** (variant) | ✅ present |
| Rules_11–12 | **Le moulin à baratin** (variant) | ✅ present |
| Rules_13–15 | **La parlote coinchée** (variant) | ✅ present |

→ **0 rule missing.** The restructure #438/#250 (re-pagination 24→15 cards) did not drop any game on prod: the 4 variants are all published alongside the main game.

## 3. Multilingual — FR ONLY (7 languages NOT published)

The repo carries 8 languages (FR source + 7 translations). The prod site serves **only FR**. Evidence:

| Check | FR (`/Règles`) | `?language=en-US` | `?language=ru-RU` | Conclusion |
|-------|----------------|-------------------|-------------------|------------|
| `<html lang="...">` | `fr-FR` | **`fr-FR`** | **`fr-FR`** | locale param ignored — page is hardcoded FR |
| Page size (bytes) | 41 605 | 41 639 (+34) | ~same | identical content (translation would change size) |
| Game titles in body | FR (`Bingo mixologie`, `dernier beau parleur`) | **same FR titles** | **same FR titles** | no EN/RU text — FR fallback |
| `&#233;` (FR accents) present | yes | yes | yes | FR content served regardless of locale |

- **Path-based locale routing**: `/en-US/Règles`, `/ru-RU/Règles`, … `/fa-IR/Règles` → all **404** (no localized page copies exist).
- **Language switcher**: the skin contains an empty `<div class="language">` / `language-object` block — the DNN language skin object is present but **not populated** (no flags, no `href` with `language=`).

### Coverage matrix — prod vs repo (per game × language)

| Game | FR | EN | RU | PT | AR | ES | ZH | FA |
|------|----|----|----|----|----|----|----|----|
| L'école des menteurs | ✅ prod | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only |
| Bingo mixologie arg. | ✅ prod | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only |
| Le dernier beau parleur | ✅ prod | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only |
| Le moulin à baratin | ✅ prod | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only |
| La parlote coinchée | ✅ prod | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only | ❌ repo only |

**Prod multilingual coverage: 1/8 languages (FR).** The 7 translations exist in the repo + ship in the print/PDF bundle (v3) but are **not ported to the DNN site**.

---

## 4. Context — why FR-only on prod is consistent with project state

- **DNN go-live (full-IIS, 2026-07-01)**: migration completed, stopgap retired. The site is live and stable (modulo the intermittent idle-hang — separate ops issue, see ASK IIS lane).
- **#490 (DNN i18n) MERGED**: this delivered the **i18n string-tier / infrastructure** (skin object, culture plumbing), **not the localized rules content**. The empty `<div class="language">` confirms the infra shell is in place but no content locales are wired.
- **Bundle v3 (print)**: 8 languages × 10 docs ship correctly (PNG-lossless + CMYK) — the multilingual assets exist; the gap is purely **DNN content portage**.
- **DNN prod go-live = ops VPS task (jsboige only)** per project memory — content portage decisions are jsboige's lane.

## 5. Recommendation (NOT executed — read-only audit)

This audit **only reports**. No prod change was made. If jsboige decides to publish the 7 translated languages on the site, the work is a **DNN/2sxc content-portage dispatch** (separate from this audit):
1. Enable the missing content locales in DNN Admin > Languages.
2. Create localized page copies of `/Règles` per culture (or wire the 2sxc module's per-language content from the CSV `Text_xx` columns).
3. Populate the `<div class="language">` switcher.

This is a **post-tag** candidate (not blocking v0.9.0 print release), consistent with decision #27 (RTL/CJK + BR/PT deferred to native validation) and #16 (DNN go-live decoupled from print assets).

---

## 6. Method / reproducibility

All probes performed read-only via `curl` + `WebFetch` against `https://dnn.argumentum.myia.io/Règles` and locale variants (2026-07-03 ~15:00 UTC). No write requests, no DNN admin access, no content modification. Repo reference: `Cards/Rules/Argumentum Rules - Cards.csv` @ master `9c19e51a`.

Note: the site exhibited an **intermittent idle-hang** during the audit (first probes timed out HTTP 000, then recovered to 200/9.5s HTTPS + 200/0.5s HTTP). This hang is tracked separately by the IIS lane (connection-pool post-idle, mitigation TBD). It does not affect the audit verdict (content was captured during a healthy window).

Relates: #636 (SVGs stale, separate DNN concern), #490 (DNN i18n infra), #134/#140 (v0.9.0 release QA), ai-01 dispatch `msg-20260703T095500-5wdug6`.
