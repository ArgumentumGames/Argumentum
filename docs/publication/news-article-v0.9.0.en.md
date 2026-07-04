# Announcement article — Argumentum v0.9.0 (EN mirror draft)

> **Status: DRAFT (#135 prep).** Public-facing announcement article for the v0.9.0 release, to be
> published on the DNN portal **News5** module. **English mirror** of the FR canonical
> ([news-article-v0.9.0.fr.md](news-article-v0.9.0.fr.md)). The DNN portal serves FR as primary; EN
> is the bilingual mirror per `docs/publication/` convention.
> **Publication is GATED** on #134 (GitHub Release tag), #132 (prod deployment) and #131 (DNN 10.3.2
> live) — see "Publish checklist" below. This file prepares the copy; it publishes nothing.
>
> **Source of truth:** [RELEASE-NOTES-v0.9.0.md](../../RELEASE-NOTES-v0.9.0.md),
> [docs/release-dossier/README.md](../release-dossier/README.md),
> [cards-catalog.en.md](cards-catalog.en.md). Every figure is verified there as of 2026-06-29;
> `[PLACEHOLDER]` fields are filled at tag time.

---

## SEO / CMS metadata (fill at publish)

| Field | Value |
|-------|-------|
| **SEO title** (`<title>`) | Argumentum v0.9.0 — the fallacy card game, now in 8 languages |
| **Meta description** (<160 char.) | Argumentum v0.9.0: 8 languages, 4 new game variants, updated mind maps and ontology. Free Print & Play materials to download. |
| **URL slug** | `argumentum-v0-9-0-8-languages` |
| **og:image** | `[PLACEHOLDER — A0 EN thumbnail or 4-variants mosaic, ~1200×630]` |
| **twitter:card** | `summary_large_image` |
| **og:locale** | `en_US` (FR canonical: `fr_FR`) |
| **Publish date** | `[PLACEHOLDER — day of the v0.9.0 tag]` |
| **CMS author** | Argumentum Games |

> **Sitemap:** add the canonical URL to the DNN sitemap at publish; declare `hreflang` alternates
> for translated variants (FR/RU/PT/ES/AR/FA/ZH) once they exist.

---

## Article body (EN)

### Argumentum v0.9.0 — the fallacy card game, now in 8 languages

**Argumentum**, the educational card game that teaches you to spot logical fallacies and build
rigorous arguments, releases version **0.9.0**. Its most ambitious update to date expands language
coverage from 4 to **8 languages**, adds **four new game variants**, and refreshes the cards, mind
maps and taxonomy ontology.

The full set of materials — printable cards, mind maps and ontology — is available as a free
download under an open license.

#### 🌍 Eight languages, one corpus

Argumentum is now generated end-to-end in **8 languages**: French (source language), English,
Russian, Portuguese, Spanish, Arabic, Persian and Chinese. Every piece of game data — the **1408
nodes** of the fallacy taxonomy, the **223 nodes** of argumentation virtues, the **167 game
scenarios** and the rules — is 100% translated into each of these languages, including non-Latin
scripts (Cyrillic, Arabic, Persian, Chinese).

| Language | Script | |
|----------|--------|---|
| Français | Latin | source language |
| English · Português · Español | Latin | |
| Русский | Cyrillic | |
| العربية · فارسی | RTL (right-to-left) | |
| 中文 | CJK | |

#### 🃏 Four new game variants

The **Rules** deck gains four brand-new game modes that freshen up play sessions:

- **Bingo mixologie argumentative**
- **Dernier Beau Parleur**
- **Moulin à Baratin**
- **Parlote Coinchée**

Each variant ships as a rule card inside the Tarot deck (and the Print & Play A4 booklet).

#### 📚 Enriched taxonomy, mind maps and ontology

The fallacy taxonomy has been consolidated: the 7 FR family roots were reviewed cell by cell, and
translation consistency is now deterministic (no machine-translation artefacts, correct scripts for
non-Latin languages). The **argumentation virtues** and the **167 scenarios** (previously 54%
translated) now reach 100% coverage.

**Mind maps** (Fallacies + Virtues) were regenerated as FreeMind SVG, and the **OWL ontology**
(with SKOS alignments and AIF references) documents the formal structure of the taxonomy — a
foundation for computational-argumentation research.

#### 🖨 Print & Play

All materials are available as **A4 Print & Play**: duplex printing on heavy paper (160–250 g/m²),
cut, and play. Two booklets:

- `TarotCards_Print&Play_A4` — Rules + Memo + Fallacies
- `PokerCards_Print&Play_A4` — Scenarios

#### 📦 Downloads

Packages are hosted on the [GitHub Releases page](https://github.com/ArgumentumGames/Argumentum/releases)
`[PLACEHOLDER — link to the v0.9.0 release once tagged]`.

| Package | Contents | Languages |
|---------|----------|-----------|
| **Full** | All materials (Tarot, Poker, Print & Play, FallaciesWeb A0/A4, Thumbnails) | all 8 |
| **Print & Play** | Print & Play A4 PDFs only (home printing, duplex) | all 8 |
| **Per language** | Complete materials for one language | pick one |
| **Mind maps** | Fallacies + Virtues SVG | `[PLACEHOLDER — 4 or 8 languages per MindMap scope decision]` |
| **Ontology** | `argumentum.owl` + documentation | FR |

Per-format detail (Tarot, Poker, Print & Play, FallaciesWeb A0/A4/Thumbnails) and printing
instructions: see the [card catalog](cards-catalog.en.md) and the
[release dossier downloads snippet](../release-dossier/README.md#5-readme-download-section-snippet-ready-to-paste--issue-134-asks-for-it).

> **80 PDFs in total** = 8 languages × 10 document types (incl. Print&Play Standard + Print&Play Light, #648-650), parity verified. All 80 converted to DeviceCMYK + SWOP OutputIntent via the Ghostscript post-process (#632/#652).

#### 💬 Join the community

`[PLACEHOLDER — community link / Discord / GitHub Discussions per decision]`

---

## Publish checklist (gates #134 / #132 / #131)

Tick at tag time — **do not publish until all are green**:

- [ ] **#134** — Tag `v0.9.0` set + GitHub Release created (assets uploaded).
- [ ] **#131** — DNN **10.3.2 + 2sxc 21** live in production (release coupling validated by jsboige).
- [ ] **#132** — Full prod deployment (runbook Phase 5).
- [ ] Replace every `[PLACEHOLDER]`: v0.9.0 release URL, date, og:image, MindMap scope (4 or 8), community link.
- [ ] Upload the `og:image` to DNN media and reference its final URL.
- [ ] Create the post in the **News5** module (DNN), paste the EN body, set slug + meta.
- [ ] Add the canonical URL to the DNN sitemap; declare `hreflang` alternates for translated variants.
- [ ] **Final visual verdict** = jsboige / ai-01 (worker signals, does not declare PASS).
- [ ] Update the site **Downloads** page with v0.9.0 links (issue #135 §Downloads).

## Translations

Per `docs/publication/` convention (FR canonical + EN mirror in the same PR), this English mirror
accompanies the canonical French file: [news-article-v0.9.0.fr.md](news-article-v0.9.0.fr.md).

The **6 other languages** (RU/PT/ES/AR/FA/ZH) follow at publish time via the `DatasetUpdater`
pipeline (same discipline as #192 native-ratification: translate then human-validate non-Latin
scripts, especially RTL/CJK). Planned post-tag, non-blocking for FR+EN publication.

## Sources

- [RELEASE-NOTES-v0.9.0.md](../../RELEASE-NOTES-v0.9.0.md) — canonical figures (8 languages, 64 PDFs, 4 variants, ~9834 images, 5.3 MB OWL).
- [docs/release-dossier/README.md](../release-dossier/README.md) — validation dossier + README downloads snippet (§5) + gate checklist (§4).
- [cards-catalog.en.md](cards-catalog.en.md) — formats and physical dimensions.
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — brief (original body stale: "4 languages"; live scope = 8).
- Dependency issues: #134 (release), #131 (DNN), #132 (deploy).
