# Announcement article — Argumentum v2.0.0 (EN mirror draft)

> **Status: DRAFT (#135 prep, #999 rework).** Public-facing announcement article for the **v2.0.0**
> release (re-scoped v0.9.0 → v2.0.0, jsboige 2026-08-06), to be published on the DNN portal
> **News5** module. **English mirror** of the FR canonical
> ([news-article-v2.0.0.fr.md](news-article-v2.0.0.fr.md)) — the DNN portal serves FR as primary; EN
> is the bilingual mirror per `docs/publication/` convention. **Publication is GATED** on #134
> (GitHub Release tag), #132 (prod deployment) and #131 (DNN 10.3.2 live) — see "Publish checklist"
> below. This file prepares the copy; it publishes nothing.
>
> **Source of truth:** [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) (reader-facing, figures
> re-measured 15/09 + qualification annex), [cards-catalog.en.md](cards-catalog.en.md) (formats),
> [134-release-assets-drift-2026-09-15.md](../release-dossier/134-release-assets-drift-2026-09-15.md)
> (state of the served assets). `[PLACEHOLDER]` fields are filled at tag time.
>
> Supersedes the [news-article-v0.9.0.en.md](news-article-v0.9.0.en.md) draft (13/09) — body reworked
> to v2.0.0, figures re-measured, virtues mind maps corrected (localized in 8 languages since #665).

---

## SEO / CMS metadata (fill at publish)

| Field | Value |
|-------|-------|
| **SEO title** (`<title>`) | Argumentum v2.0.0 — the fallacy card game, now in 8 languages |
| **Meta description** (<160 char.) | Argumentum v2.0.0: 8 languages, 197 cards per box, new game variants, updated mind maps and ontology. Free Print & Play. |
| **URL slug** | `argumentum-v2-0-0-8-languages` |
| **og:image** | `[PLACEHOLDER — A0 FR thumbnail or 4-variant mosaic, ~1200×630]` |
| **twitter:card** | `summary_large_image` |
| **og:locale** | `en_US` (canonical post: `fr_FR`) |
| **Publish date** | `[PLACEHOLDER — v2.0.0 tag day]` |
| **CMS author** | Argumentum Games |

---

## Article body (EN)

### Argumentum v2.0.0 — the fallacy card game, now in 8 languages

**Argumentum**, the educational card game that teaches you to spot fallacies and defend sound
arguments, ships as version **2.0.0**. Its most ambitious update yet expands language coverage from
4 to **8 languages**, adds **four new game variants**, and refreshes the cards, mind maps and the
taxonomy ontology.

The complete material — printable cards, mind maps and ontology — is available as a free download
under an open licence.

#### 🌍 Eight languages, one deck

Argumentum is now generated entirely in **8 languages**: French (source language), English, Russian,
Portuguese, Spanish, Arabic, Farsi and Chinese. Every piece of game data — the fallacy taxonomy
(**1408 nodes**), the **223 nodes** of argumentative virtues, the **167 game scenarios** and the
rules — is 100 % translated in each language, non-Latin scripts included (Cyrillic, Arabic, Farsi,
Chinese).

| Language | Script | |
|----------|--------|---|
| French | Latin | source language |
| English · Português · Español | Latin | |
| Русский | Cyrillic | |
| العربية · فارسی | RTL | |
| 中文 | CJK | |

#### 🃏 What's in the box

The main deck counts **197 cards**: **175** fallacy cards (a true duplicate was removed in this
edition), **15** rule cards — including **four brand-new game variants**: *Argumentative Mixology
Bingo*, *Last Sweet Talker*, *Babble Mill* and *Bid Chatter* — and the memo card printed as **7
copies**. Add the **167-card** scenario deck (7 category backs) and the full box reaches **364
cards** — **495** with the 131-card **Virtues** expansion.

#### 📚 Richer taxonomy, mind maps and ontology

The fallacy taxonomy has been consolidated: the FR family roots were revised cell by cell, and
translation consistency is now deterministic (no machine-translation artefacts, correct scripts for
non-Latin languages). The **argumentative virtues** and the **167 scenarios** (previously 54 %
translated) are now fully covered.

The **mind maps** have been regenerated as FreeMind SVGs **in all 8 languages** — fallacies and
virtues alike — and the **OWL ontology** (with SKOS alignments and AIF references) documents the
formal structure of the taxonomy in French and English — a foundation for research in computational
argumentation.

#### 🖨 Print & Play

The whole material is available as **A4 Print & Play**: print double-sided on thick paper
(160–250 g/m²), cut, play. Four booklets, in every language:

- `TarotCards_Print&Play_A4` — Rules + Memo + Fallacies, plus its ink-saving **Light** edition
- `PokerCards_Print&Play_A4` — Scenarios, plus its **Light** edition

#### 📦 Downloads

Packages will be hosted on the [GitHub Releases](https://github.com/ArgumentumGames/Argumentum/releases)
page `[PLACEHOLDER — link to the v2.0.0 release once tagged]`.

| Package | Contents | Languages |
|---------|----------|-----------|
| **Complete** | Everything (Tarot, Poker, Print & Play, FallaciesWeb A0/A4, Thumbnails) | all 8 |
| **Print & Play** | A4 Print & Play PDFs (standard + Light) | all 8 |
| **Per language** | Complete material for one language | pick one |
| **Mind maps** | Fallacies + Virtues SVGs | all 8 (FR/EN/RU/PT/ES/AR/FA/ZH) |
| **Ontology** | `argumentum.owl` + `argumentum_virtues.owl` (SKOS + AIF) | FR · EN |

**80 PDFs in total** = 8 languages × 10 document types, parity verified; professional-print PDFs
are converted to DeviceCMYK with OutputIntent (press-ready), cards at native 300 dpi.

Per-format details and printing instructions: see the [card catalogue](cards-catalog.en.md).

> ⚠️ **Until the v2.0.0 tag**, do not redistribute the `v0.9.0-review` pre-release assets: they date
> from Aug 24-25 and predate the September corrections (detailed state in the
> [drift dossier](../release-dossier/134-release-assets-drift-2026-09-15.md)).

#### 💬 Join the community

`[PLACEHOLDER — community link / Discord / GitHub Discussions, pending decision]`

---

## Publish checklist (gates #134 / #132 / #131)

Tick at tag time — **do not publish until everything is green**:

- [ ] **#134** — `v2.0.0` tag posed + GitHub Release created (**80 fresh assets** re-uploaded — not
      the Aug 24 pre-release, see the drift dossier; `Print.Play` vs `Print&Play` naming settled).
- [ ] **jsboige review of the v2.0.0 release notes** (#999 DoD 3) — condition set by the CHANGELOG.
- [ ] **#131** — DNN **10.3.2 + 2sxc 21** live in production (release coupling validated by jsboige).
- [ ] **#132** — Full prod deployment (runbook Phase 5).
- [ ] Replace every `[PLACEHOLDER]`: v2.0.0 release URL, date, og:image, community link.
- [ ] Upload the `og:image` to the DNN media library and reference its final URL.
- [ ] Create the post in the **News5** module (DNN), paste the EN body, set slug + meta.
- [ ] Add the canonical URL to the DNN sitemap; declare `hreflang` alternates.
- [ ] **Final visual verdict** = jsboige / ai-01 (the worker reports, it does not declare PASS).
- [ ] Update the site **Downloads** page with the v2.0.0 links (issue #135 §Downloads).

## Translations (mirrors)

Per the `docs/publication/` convention (FR canonical + EN mirror in the same PR), this file mirrors
[news-article-v2.0.0.fr.md](news-article-v2.0.0.fr.md). The **6 other languages** (RU/PT/ES/AR/FA/ZH)
follow at publish time via the `DatasetUpdater` pipeline (same discipline as the #192
native-ratification: translation then human validation of non-Latin scripts, especially RTL/CJK).
To plan post-tag, non-blocking for the FR+EN publication.

## Sources

- [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) — reader-facing notes, figures re-measured 15/09 on `c089d526` (175/15/167 MEASURED, memo ×7 and 197/364/495 DECIDED #1187 c.5665864605, Virtues 131 REPORTED, 80 PDFs MEASURED).
- [cards-catalog.en.md](cards-catalog.en.md) — formats, physical dimensions, fabrication volumes (15/09 erratum).
- [134-release-assets-drift-2026-09-15.md](../release-dossier/134-release-assets-drift-2026-09-15.md) — why the pre-release is not distributable as-is (PR #1383).
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — brief (original body stale: "4 languages"; actual scope = 8).
- Dependency issues: #134 (release), #131 (DNN), #132 (deploy), #999 (v2.0.0 renumbering).
