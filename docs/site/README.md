# docs/site — measurements and corrections of the website content

<!-- index-of-record -->

This folder is an **index of record**: every tracked file in it is listed below with what it is worth today. Add a file here and you add its line — `python tools/test_docs-index-contract.py` goes red otherwise. The three statuses and the method are in [`docs/agents/consolidation.md`](../agents/consolidation.md).

Almost everything here is **evidence of closed work**. What governs go-live day is the consolidated cutover runbook [`docs/quality/1066-cutover-runbook-2026-11-04.md`](../quality/1066-cutover-runbook-2026-11-04.md) (#2148); the procedures of the website itself are indexed in [`docs/dnn/README.md`](../dnn/README.md).

## Where the subjects stand

Measured 2026-11-13 on `47d70f15`; issue states read in bulk the same day. Re-measure before quoting; the issue says what is open.

- **The gap between the served site and the preprod — #1066, closed 2026-10-06.** Three read-only sweeps (06/10, then 08/10). The analytics-tag item is closed: the cutover runbook lists « GTM/GA à rejouer (R4 — clos 08/10) » among its permanent prohibitions. The sweep itself is replayed on go-live day as step 20 of the cutover runbook.
- **Content corrections on the preprod — #1781, closed 2026-11-03.** The manifest and the grain notes are the trace of what was written there in October. One divergence is still open: five typos corrected on the preprod rules pages are still in the Rules card template on master (measured: each of the five strings found once in `Cards/Rules/Argumentum_Rules_fr.json` at `origin/master`; witness string found 6 times). It is carried by #1502 (open). A temporary « Bientôt de retour » banner posed on the shop article is to come off when the shop reopens — decision D1, #445 (open).
- **The site in other languages — #457, open.** The English-site measurement of 08/10 predates the multilingual work; the cutover runbook's #457 row governs it now (language packs and per-language probes on go-live day).
- **The ontology presentation page.** `ontologie_fr.html` is the page posed on the preprod in October (#1786). Publishing the ontology files themselves is #133 (open), step 16 of the cutover runbook.

## The files

Dates are the first and last commits touching the file (`git log`).

| File | Dated | Status | What it is |
|---|---|---|---|
| [`ontologie_fr.html`](ontologie_fr.html) | 2026-10-07 | `current` | The French presentation page of the ontology, posed on the preprod as grain K of #1781 (#1786) — the source of that page. No English counterpart is tracked (measured: `ontologie_fr.html` is the only `ontologie_*` file on master) |
| [`1066-balayage-ecart-prod-preprod.md`](1066-balayage-ecart-prod-preprod.md) | 2026-10-07 | `historical` | The control of 2026-10-06: gate 2 of #1066 measured entirely green. It opens with a warning that its first version misread the issue — read that warning |
| [`1066-rebalayage-2026-10-08.md`](1066-rebalayage-2026-10-08.md) | 2026-10-08 → 10-09 | `historical` | The re-run of that sweep on 2026-10-08 ("it replays the control, it does not replace it"). The method lives on in step 20 of the cutover runbook |
| [`1066-2a-ga4-remesure-2026-10-08.md`](1066-2a-ga4-remesure-2026-10-08.md) | 2026-10-08 | `historical` | The re-measure showing the last analytics-tag item was already closed. The earlier cutover runbook carries a correction pointing here |
| [`1781-corrections-contenu-preprod.md`](1781-corrections-contenu-preprod.md) | 2026-10-07 | `historical` | Manifest of every content correction written on the preprod for #1781 (v3.2), with backups and served checks. Its open-items table still holds one live divergence, carried by #1502 — see above |
| [`1781-grains-DEF-recherches.md`](1781-grains-DEF-recherches.md) | 2026-10-07 | `historical` | Measurements and recommendations for grains D, E and F (nothing written for those), then the grains posed afterwards, with a summary of every write of the pass |
| [`1781-mesure-site-en-anglais.md`](1781-mesure-site-en-anglais.md) | 2026-10-08 | `historical` | What an English version of the site would cost, measured on 2026-10-08 (pages, words, mechanisms). Overtaken by the multilingual site of #457 — see the contradictions |

## Contradictions between documents

One line each: what disagrees, and which one holds.

- **Analytics tags on two pages.** [`docs/quality/1066-cutover-runbook-2026-09-22.md`](../quality/1066-cutover-runbook-2026-09-22.md) said « gap résiduel = 2 pages »; `1066-2a-ga4-remesure-2026-10-08.md` measured both pages tagged → **the re-measure holds**, and that runbook already carries the correction (its l.38).
- **Languages served.** `1781-mesure-site-en-anglais.md` §8 ends « Aucune de ces quatre actions n'est engagée »; §0 of the [cutover runbook](../quality/1066-cutover-runbook-2026-11-04.md) records the preprod serving 8 languages → **the cutover runbook holds**.
- **Language of the mind-map pages.** `1781-mesure-site-en-anglais.md` §6.1 measured the deployed mind-map pages declaring `lang="en"` in every language; on master the pages now declare their own language (measured on four of them: `lang="fr"` ×3, `lang="ru"` ×1) → **the repository holds**; what is served was not re-measured.
