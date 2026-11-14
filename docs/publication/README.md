# docs/publication — what the release and the website publish

<!-- index-of-record -->

This folder is an **index of record**: every tracked file in it is listed below with what it is worth today. Add a file here and you add its line — `python tools/test_docs-index-contract.py` goes red otherwise. The three statuses and the method are in [`docs/agents/consolidation.md`](../agents/consolidation.md). The two older inventories of this folder, `README.fr.md` and `README.en.md`, are ordinary files listed below; their conventions still apply.

Nothing in this folder publishes anything: these are drafts and references. When and how they go out is set by the consolidated cutover runbook [`docs/quality/1066-cutover-runbook-2026-11-04.md`](../quality/1066-cutover-runbook-2026-11-04.md) (#2148).

## Where the subjects stand

Measured 2026-11-13 on `47d70f15`; issue states read in bulk the same day. Re-measure before quoting; the issue says what is open.

- **The GitHub release — #134, open.** The public preview is `v2.1.0-review` (measured: `gh release list`); the final release is still to be created from it, and the preview is not overwritten. The cutover runbook's #134 row governs.
- **The news article and the Downloads page — #135, open.** Published on go-live day (step 12 of the cutover runbook). The drafts are `news-article-v2.0.0.*` and `downloads-v2.0.0.*`; the list of assets is `135-downloads-assets-table.md` (#2234), and the mapping of today's downloads to the new ones was decided on [#135](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268) (2026-11-05). Owner review is required before anything is published.
- **Seven site articles and the site body — epic #2173, open.** The French drafts are in `articles/`, one per issue #2174–#2180 (all open); the extension of the Argumentation block is `argumentation-bloc-fr.md` (#2181, open). Translation follows the French reading, through #2182 (open).
- **Card catalogue and visual validation.** The catalogue was last refreshed on 2026-09-15. The multilingual QA issue #140 closed on 2026-09-13; the validation guide still serves the release gate #134.

## The files

Dates are the first and last commits touching the file (`git log`).

| File | Dated | Status | What it is |
|---|---|---|---|
| [`135-downloads-assets-table.md`](135-downloads-assets-table.md) | 2026-11-11 | `current` | The 80 PDF assets of the Downloads page (10 documents × 8 languages), derived from the generator config and checked against `v2.1.0-review` (0 absent), plus the 24 zip names the packaging tool will build (#2234) |
| [`downloads-v2.0.0.fr.md`](downloads-v2.0.0.fr.md) | 2026-09-16 → 11-05 | `current` | Draft of the Downloads page, French canonical (#135): per-language material, sizes measured on 2026-09-16, publication checklist. Its zip placeholder and its gates are out of date — see the contradictions |
| [`downloads-v2.0.0.en.md`](downloads-v2.0.0.en.md) | 2026-09-16 → 11-05 | `current` | English mirror of the draft above |
| [`news-article-v2.0.0.fr.md`](news-article-v2.0.0.fr.md) | 2026-09-15 | `current` | Draft of the release announcement for the site's news module, French canonical (#135, figures re-measured in #1386) |
| [`news-article-v2.0.0.en.md`](news-article-v2.0.0.en.md) | 2026-09-15 | `current` | English mirror of the draft above |
| [`news-article-v0.9.0.fr.md`](news-article-v0.9.0.fr.md) | 2026-06-29 → 09-15 | `superseded` | The first announcement draft, for the version number abandoned on 2026-08-06. It declares itself « Superseded 15/09/2026 »; replaced by [`news-article-v2.0.0.fr.md`](news-article-v2.0.0.fr.md) |
| [`news-article-v0.9.0.en.md`](news-article-v0.9.0.en.md) | 2026-06-29 → 09-15 | `superseded` | English mirror of the draft above, same banner; replaced by [`news-article-v2.0.0.en.md`](news-article-v2.0.0.en.md) |
| [`articles/README.md`](articles/README.md) | 2026-11-14 | `current` | The rule the seven articles are held to, in the owner's words of 2026-11-14: the narrative first, the numbers second, and illustrations that answer the article's subject (#2173, #2263) |
| [`articles/1-depot.fr.md`](articles/1-depot.fr.md) | 2026-11-07 | `current` | Article 1/7 — from the card game to a reproducible repository (#2174) |
| [`articles/2-aif-liens-transverses.fr.md`](articles/2-aif-liens-transverses.fr.md) | 2026-11-08 | `current` | Article 2/7 — the ontology, the AIF format and the cross-links (#2175) |
| [`articles/3-epita-descente-guidee.fr.md`](articles/3-epita-descente-guidee.fr.md) | 2026-11-08 | `current` | Article 3/7 — a student project's plugin that walks down the taxonomy level by level (#2176) |
| [`articles/4-coursia-carnets.fr.md`](articles/4-coursia-carnets.fr.md) | 2026-11-08 | `current` | Article 4/7 — course notebooks that read the game's corpus (#2177) |
| [`articles/5-produit-cartesien.fr.md`](articles/5-produit-cartesien.fr.md) | 2026-11-07 | `current` | Article 5/7 — fallacies × scenarios, the game as a dataset (#2178) |
| [`articles/6-ict-strate-discours.fr.md`](articles/6-ict-strate-discours.fr.md) | 2026-11-08 | `current` | Article 6/7 — a research series' prediction set on the corpus (#2179) |
| [`articles/7-site-multilingue.fr.md`](articles/7-site-multilingue.fr.md) | 2026-11-07 | `current` | Article 7/7 — the site in eight languages, the launch article (#2180) |
| [`argumentation-bloc-fr.md`](argumentation-bloc-fr.md) | 2026-11-10 | `current` | French draft extending the "L'argumentation" block of the site, for the owner's reading; nothing injected (#2181) |
| [`cards-catalog.fr.md`](cards-catalog.fr.md) | 2026-05-30 → 09-15 | `current` | Catalogue of the publishable card formats: deliverables, dimensions, colour profile, audience, volumes (fabrication counts re-measured 2026-09-15). Its list of deliverables is incomplete — see the contradictions |
| [`cards-catalog.en.md`](cards-catalog.en.md) | 2026-05-30 → 09-15 | `current` | English mirror of the catalogue, same gap |
| [`validation-guide.fr.md`](validation-guide.fr.md) | 2026-05-31 → 09-12 | `current` | How a final reviewer validates the generated PDFs by eye, including in languages they do not read. Written for #140 (closed) and the release gate #134 (open) |
| [`README.fr.md`](README.fr.md) | 2026-05-30 → 09-13 | `current` | The folder's first inventory and its conventions (language suffix, millimetres, edit the C# config, never the generated JSON). The conventions hold; its inventory stops at the v0.9.0 article |
| [`README.en.md`](README.en.md) | 2026-05-30 → 09-13 | `current` | English mirror of the file above |
| [`qa-scenario-8langues-release.md`](qa-scenario-8langues-release.md) | 2026-06-02 | `historical` | The QA walk-through of the 2026-06-02 build (64 PDFs, 8 languages × 8 documents) for #140, closed on 2026-09-13 |
| [`release-v090-bundle-v3-8lang-verification-guide.md`](release-v090-bundle-v3-8lang-verification-guide.md) | 2026-07-05 | `historical` | What to look at, per language, in the July review bundle of the abandoned v0.9.0 number. Later bundles replaced that one |

## Contradictions between documents

One line each: what disagrees, and which one holds.

- **Zip names on the Downloads page.** `downloads-v2.0.0.fr.md` (l.31) and its mirror (l.32) hold a placeholder `argumentum-{lang}-v2.0.0.zip`; `135-downloads-assets-table.md` lists the only zips the tool builds (`Argumentum_Cards_PNG[_light]_<lang>.zip`, `Argumentum_MindMaps_<lang>.zip`), and the [#135 decision of 2026-11-05](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268) links the PDFs one by one from the final release → **the table and the decision hold**.
- **How many PDF deliverables.** `cards-catalog.fr.md` and its mirror say « huit livrables PDF » and never mention the Light print-and-play variants (measured: 0 occurrence of "Light" in either, 4 in the Downloads draft); `135-downloads-assets-table.md` derives 10 documents per language from the config, the Light variants coming from #645 (closed 2026-07-02) → **the table holds**.
- **Publication gates.** `news-article-v2.0.0.*` and `downloads-v2.0.0.*` say publication waits on #132 « prod deployment » and on #131 « DNN 10.3.2 live »; #132 closed on 2026-06-18, and the [cutover runbook](../quality/1066-cutover-runbook-2026-11-04.md) targets 10.4.1 and places #135 at its step 12 → **the cutover runbook holds**.
- **Which article draft is current.** `README.fr.md` and `README.en.md` §3 list the v0.9.0 article as the draft; that draft declares itself « Superseded 15/09/2026 » → **the v2.0.0 articles hold**.
- **The final tag.** The drafts announce a tag `v2.0.0`; the public preview is `v2.1.0-review`, and the [cutover runbook](../quality/1066-cutover-runbook-2026-11-04.md) says the final release is created from it without naming its tag → **the tag follows #134** (owning lane, 2026-11-13): it is cut when #134 closes; the drafts that write `v2.0.0` as given are ahead of the facts.
