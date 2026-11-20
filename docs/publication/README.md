# docs/publication — what the release and the website publish

<!-- index-of-record -->

This folder is an **index of record**: every tracked file in it is listed below with what it is worth today. Add a file here and you add its line — `python tools/test_docs-index-contract.py` goes red otherwise. The three statuses and the method are in [`docs/agents/consolidation.md`](../agents/consolidation.md). The two older inventories of this folder, `README.fr.md` and `README.en.md`, are ordinary files listed below; their conventions still apply.

Nothing in this folder publishes anything: these are drafts and references. When and how they go out is set by the consolidated cutover runbook [`docs/quality/1066-cutover-runbook-2026-11-04.md`](../quality/1066-cutover-runbook-2026-11-04.md) (#2148).

## Where the subjects stand

Measured 2026-11-13 on `47d70f15`; issue states read in bulk the same day. Re-measure before quoting; the issue says what is open.

- **The GitHub release — #134, open.** The public preview is `v2.1.0-review` (measured: `gh release list`); the final release is still to be created from it, and the preview is not overwritten. The cutover runbook's #134 row governs.
- **The news article and the Downloads page — #135, open.** Published on go-live day (step 12 of the cutover runbook). The drafts are `news-article-v2.0.0.*` and `downloads-v2.0.0.*`; the list of assets is `135-downloads-assets-table.md` (#2234), and the mapping of today's downloads to the new ones was decided on [#135](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268) (2026-11-05). Owner review is required before anything is published.
- **Seven site articles and the site body — epic #2173, open.** The French drafts are in `articles/`, one per issue #2174–#2180 (all open); each now carries its illustration, chosen by the owner from planche #2277 and wired on 2026-11-17; the extension of the Argumentation block is `argumentation-bloc-fr.md` (#2181, open). Translation follows the French reading, through #2182 (open).
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
| [`articles/illustrations/1a-du-texte-a-la-carte.png`](articles/illustrations/1a-du-texte-a-la-carte.png) | 2026-11-17 | `current` | Illustration of article 1/7 — montage: nineteen real taxonomy lines beside the card they produce, « Argument bâclé » (the owner's choice 1a from planche #2277) |
| [`articles/illustrations/2a-liens-transverses.png`](articles/illustrations/2a-liens-transverses.png) | 2026-11-17 | `current` | Illustration of article 2/7 — ×2 crop of the cross-links mind map: the arrows cross branches instead of following the tree (2a) |
| [`articles/illustrations/3b-du-chemin-a-la-carte.png`](articles/illustrations/3b-du-chemin-a-la-carte.png) | 2026-11-17 | `current` | Illustration of article 3/7 — the four levels of the guided descent, one repository engraving per level (3b) |
| [`articles/illustrations/4a-cinq-carnets.png`](articles/illustrations/4a-cinq-carnets.png) | 2026-11-17 | `current` | Illustration of article 4/7 — the five CoursIA notebooks, their folder and their commit (4a) |
| [`articles/illustrations/5b-une-manche-deux-cartes.png`](articles/illustrations/5b-une-manche-deux-cartes.png) | 2026-11-17 | `current` | Illustration of article 5/7 — a scenario back « Histoire » and a fallacy card: the game's cross product in one image (5b) |
| [`articles/illustrations/6c-deux-cartes-un-miroir.png`](articles/illustrations/6c-deux-cartes-un-miroir.png) | 2026-11-17 | `current` | Illustration of article 6/7 — « Faux dilemme » and « Juste milieu » joined by the reciprocal `Mirrors` link; row-number captions and footer cropped out (verdict c.6899162608) |
| [`articles/illustrations/7b-accueil-arabe.png`](articles/illustrations/7b-accueil-arabe.png) | 2026-11-17 | `current` | Illustration of article 7/7 — the Arabic home page, read right-to-left, captured 15/11/2026 (7b) |
| [`argumentation-bloc-fr.md`](argumentation-bloc-fr.md) | 2026-11-10 → 11-20 | `current` | French extension of the site's "L'argumentation" block, read by the owner and partially published on 2026-11-19. P24-31: qualified AIF critical-question clause and the same Virtues OWL exclusion as the served value; the remaining retouch goes out with the seven translations after the cutover (#2181) |
| [`argumentation-bloc-en.md`](argumentation-bloc-en.md) | 2026-11-20 | `current` | English translation of the Argumentation extension; no native-speaker review claimed (#2181, P24-31) |
| [`argumentation-bloc-ru.md`](argumentation-bloc-ru.md) | 2026-11-20 | `current` | Russian translation of the Argumentation extension; no native-speaker review claimed (#2181, P24-31) |
| [`argumentation-bloc-pt.md`](argumentation-bloc-pt.md) | 2026-11-20 | `current` | Portuguese translation of the Argumentation extension; no native-speaker review claimed (#2181, P24-31) |
| [`argumentation-bloc-es.md`](argumentation-bloc-es.md) | 2026-11-20 | `current` | Spanish translation of the Argumentation extension; no native-speaker review claimed (#2181, P24-31) |
| [`argumentation-bloc-ar.md`](argumentation-bloc-ar.md) | 2026-11-20 | `current` | Arabic translation of the Argumentation extension; no native-speaker review claimed (#2181, P24-31) |
| [`argumentation-bloc-fa.md`](argumentation-bloc-fa.md) | 2026-11-20 | `current` | Persian translation of the Argumentation extension; no native-speaker review claimed (#2181, P24-31) |
| [`argumentation-bloc-zh.md`](argumentation-bloc-zh.md) | 2026-11-20 | `current` | Chinese translation of the Argumentation extension; no native-speaker review claimed (#2181, P24-31) |
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
