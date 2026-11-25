# DNN Localization & Upgrade Arc — Index of record (tranches 1–4/4, measured 2026-11-25)

<!-- index-of-record -->

> **Entry point for the `#131` / `#132` / `#457` DNN documentation arc.**
> The folder holds **217 tracked files** (`git ls-files docs/dnn-localization`, measured 2026-11-24). This
> README is an **index of record, in four tranches**: tranche 1 (§1–§7) indexes the **49 root files whose
> name is not `458-*`** — the 48 documents plus this README — and the two subfolders; tranche 2 (§8) the
> **first 49 of the 134 `458-*` files**, tranche 3 (§9) the **next 50**, tranche 4 (§10) the **35 that
> remain**. All 134 are indexed.
> ⚠️ The mark above submits this file to `python tools/test_docs-index-contract.py` — completeness, one
> status per row, no dead link (§12). Until tranche 4 this file was a navigation index that happened to
> carry statuses; it is now a contract.

**Status vocabulary** (`docs/agents/consolidation.md`): `current` = says today's state, or governs a
gesture still to come — re-read against master on 2026-11-24; `superseded` = a later document replaces it
(the successor is named on the same line); `historical` = dated record, not governing. Doubt between
current and historical resolves to **historical**. For data files (CSV/JSON) the date column is the last
commit that touched them; the status says whether they are still consumed.

---

## §0 — Site state, 2026-11-24 (supersedes the former index kept at the end of this file)

**The production swap happened on 2026-11-24 at 04:56Z.** Three independent sources: web2 posted
"jour J terminé" at 05:04Z; the external recette P23-32 ran read-only against the served site (32/32
routes, legal EN 2/2, 301s, 7 language links per homepage, `hreflang` 0/8); ai-01 measured on its own
instrument. Nothing in this folder was written by that pass — this index records it, it does not
certify it.

> ⚠️ The **former navigation index** is kept **byte for byte at the end of this file**, under *Archive*
> (last changed on master 2026-07-13; its status block is dated 2026-07-12). Three of its lines are
> statements of that date, not of today. Its "Prod go-live — PENDING" line is **false since 24/11**. Its
> "Current target: DNN 10.3.2 + 2sxc 21.07 LTS" predates the cutover runbook, whose validated generation
> is **DNN 10.4.1 + 2sxc 22.01** ([`../quality/1066-cutover-runbook-2026-11-04.md`](../quality/1066-cutover-runbook-2026-11-04.md)).
> Its §4 says the gates it lists "block prod go-live": the go-live happened (above), and the state of each
> gate lives on its issue, not in this file. The B1-inversion sweep note and the version matrix it points
> to remain the record of *how* the runtime was made to work.

---

## §1 — The go-live spine (executed; read for history, not as a TODO)

In the order it was executed. Every row below is `historical` or `superseded` today: the upgrade the
spine planned is the one that went live on 24/11 (§0).

| File | Date | Status | What it is |
|---|---|---|---|
| [131-step0-runtime-verification.md](131-step0-runtime-verification.md) | 2026-06-17 | `historical` | Verified fact that founded the plan: DNN 10.x runs on **.NET Framework 4.8**, not .NET 8. |
| [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) | 2026-06-17 | `superseded` → [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | CORRIGENDUM of a CVE-patched-version error in the merged v2 plan and Step 0; refined the target. |
| [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | 2026-06-18 | `historical` | DECISION-SPEC: 10.1.2 → **10.3.2 full**, 2sxc-cliff ordering. The target that was executed. |
| [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) | 2026-06-19 | `historical` | Step-by-step **2sxc 15.02 → 21.07 LTS** before crossing 10.2.0. Executed as planned. |
| [131-dnn-phase2-exec-rollback.md](131-dnn-phase2-exec-rollback.md) | 2026-06-19 | `historical` | DNN **9.11.1 → 10.3.2** execution sequence + two-anchor rollback runbook (§4.2 restore order fixed in #552). |
| [131-step2-smoke-test-checklist.md](131-step2-smoke-test-checklist.md) | 2026-06-18 | `historical` | Consolidated smoke-test gate to run after the sandbox upgrade, before go-live. |
| [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) | 2026-06-17 | `historical` | Merged "v2" sandbox plan + go/no-go matrix; the palier logic that was followed. |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) | 2026-06-17 | `superseded` → [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | Sandbox runbook written for the **10.1.2** target; mechanics still readable, target abandoned. |
| [131-upgrade-9.13-sandbox-plan.md](131-upgrade-9.13-sandbox-plan.md) | 2026-06-15 | `superseded` → [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) | First 9.13.x palier plan. 9.13.x closes **0** CVEs (NVD/GHSA correction) — replaced by v2. |
| [131-regen-staging-runbook.md](131-regen-staging-runbook.md) | 2026-06-19 | `historical` | Stages every prerequisite for an attended mindmap/PDF regen, **no launch**. No regen was launched from it. |
| [132-deployment-runbook.md](132-deployment-runbook.md) | 2026-11-04 | `superseded` → [../quality/1066-cutover-runbook-2026-11-04.md](../quality/1066-cutover-runbook-2026-11-04.md) | Former prod deployment runbook; its own header names the #1066 cutover runbook as the replacement. |

---

## §2 — Site content, localization pipeline (#457)

| File | Date | Status | What it is |
|---|---|---|---|
| [PHASE1-content-audit.md](PHASE1-content-audit.md) | 2026-06-13 | `historical` | Phase 1 content-extraction audit (#457): what the site content is made of, repo-side. Complete. |
| [457-site-content-type-inventory.md](457-site-content-type-inventory.md) | 2026-06-17 | `historical` | Localization scope map of the site's content types, extending Phase 1. Pre-upgrade pictures; the zone-3 measurement of 09-10/10 is the later picture. |
| [457-document-tier-translation-workflow.md](457-document-tier-translation-workflow.md) | 2026-06-22 | `historical` | Design of the document tier (2 static FR HTML pages): extraction → DatasetUpdater → re-import. String-tier CSV dialect does not fit prose. |
| [457-t1-site-content-pipeline.md](457-t1-site-content-pipeline.md) | 2026-10-08 | `historical` | T1, first brick: the **2sxc export ⇄ CSV ⇄ import XML** pipeline and its offline proof. |
| [457-t2-app33-first-lot.md](457-t2-app33-first-lot.md) | 2026-10-09 | `historical` | T2 first real lot on app 33: pipeline adapted, then blocked on brick 1 (the export). |
| [457-t2-pivot-worklist-2026-10-10.md](457-t2-pivot-worklist-2026-10-10.md) | 2026-10-10 | `historical` | T2 brick 2: CSV pivots of app 33 proven (one entity), measured worklist delivered. |
| [457-t3-globalresources-arbitrage-dossier-2026-10-10.md](457-t3-globalresources-arbitrage-dossier-2026-10-10.md) | 2026-10-10 | `historical` | Arbitration dossier for `GlobalResources.<culture>.resx`: composition, real surface, upstream. Reco: upstream es+pt installs are a lane gesture on preprod; production needs the owner GO. |
| [457-t3-ui-labels-measured.md](457-t3-ui-labels-measured.md) | 2026-10-10 | `historical` | T3 measure-only: which UI labels are done, which are a no-op, which remain. Erratum of 10/10 after counter-review. |
| [457-p24-01-residu-visible-traductions-2026-11-14.md](457-p24-01-residu-visible-traductions-2026-11-14.md) | 2026-11-14 | `historical` | P24-01: the visible part of the langpack residue, translated. |

---

## §3 — Field models, provisioning, DNN inventories (#490 / #506 / #682 / #683 / #684)

Four of these still **govern a gesture to come**: the owner window that provisions the field model.
Everything else is the record that led to it.

| File | Date | Status | What it is |
|---|---|---|---|
| [dossier-groupe-682-685-fenetre-owner.md](dossier-groupe-682-685-fenetre-owner.md) | 2026-09-14 | `current` | Grouped dossier for the owner window (i18n portage, steps 2–6): the sequencing to execute. |
| [682-685-dryrun-fenetre-owner.md](682-685-dryrun-fenetre-owner.md) | 2026-09-15 | `current` | Dry-run of that window (provision `Title_<lang>` + runtime control). Code (#674) and dossier (#1373) merged: **only the window remains**. |
| [682-path-a-provisioning-manifest.md](682-path-a-provisioning-manifest.md) | 2026-07-11 | `current` | Manifest of the **49 suffixed fields** (7 translatable × 7 non-FR languages) for the 2sxc editor. Gated ops. |
| [682-field-model-revision-2sxc21.md](682-field-model-revision-2sxc21.md) | 2026-07-09 | `current` | Revision of the field model against the 2sxc v21 export (#681): confirms Path A, 3 deltas corrected. The model the window applies. |
| [682-field-model-decision-input.md](682-field-model-decision-input.md) | 2026-07-05 | `superseded` → [682-field-model-revision-2sxc21.md](682-field-model-revision-2sxc21.md) | Decision input that chose Path A (lang-suffixed + `loc()` cascade); the revision carries the corrected model. |
| [490-res-rule-provisioning-manifest.md](490-res-rule-provisioning-manifest.md) | 2026-07-11 | `current` | Manifest of the `res.Rule*` DB writes (Δ1/Δ2). **Manifest only** — no write is executed from the repo; gated ops. |
| [683-content-language-enablement-design.md](683-content-language-enablement-design.md) | 2026-07-04 | `historical` | Design for enabling 7 cultures + routing + switcher. Measured on prod on 24/11: 7 language links per homepage, `hreflang` 0/8 (P23-32). |
| [684-rule-prose-translation-harness.md](684-rule-prose-translation-harness.md) | — | `historical` | Harness design (Option C, worker-side): 0 run, 0 prod CSV, 0 DB mutation. |
| [684-translation-manifest.md](684-translation-manifest.md) | 2026-07-05 | `historical` | Manifest/prep of the Rule prose translations (~30 entities × 7 languages). No translation executed at that date. |
| [684-scope-correction.md](684-scope-correction.md) | 2026-07-10 | `historical` | Addendum that corrects the manifest against the live export (#681): game count confirmed at 5, entity list. |
| [684-translation-run-report.md](684-translation-run-report.md) | 2026-07-10 | `historical` | Run report: 196/196 cell-languages translated (28 cells × 7 languages), prep delivered, 0 prod write. Re-import remains gated. |
| [684-translations.json](684-translations.json) | 2026-07-10 | `historical` | The 196 translated cells, as data. Consumed by the gated re-import, not by the site. |
| [490-res-rule-reconciliation.md](490-res-rule-reconciliation.md) | 2026-07-09 | `historical` | Reconciliation of the 7 `res.Rule*` FR values against live 2sxc v21 export #681; feeds the manifest above. |
| [506-social-auth-connectors-inventory.md](506-social-auth-connectors-inventory.md) | 2026-09-11 | `superseded` → [../dnn/506-social-auth-assessment.md](../dnn/506-social-auth-assessment.md) | Former social-auth connectors inventory. Its header names the canonical, re-measured assessment. |

---

## §4 — Measurements and records (#136 / #1180 / #1502 / #1781 / #2182)

| File | Date | Status | What it is |
|---|---|---|---|
| [136-2sxc-collaborative-scoping.md](136-2sxc-collaborative-scoping.md) | 2026-06-27 | `historical` | Read-only scoping of #136 (2sxc taxonomy entities + collaborative editing): what exists, what is missing, what is gated. |
| [1180-portallocalization-enus-culturename-zh-2026-10-25.md](1180-portallocalization-enus-culturename-zh-2026-10-25.md) | 2026-10-25 | `historical` | #1180 arbitration 7: the `PortalLocalization` en-US row and the zh-CN `CultureName` corrected. No production write (prod frozen at that date). |
| [1502-r-site-mesure-et-plan.md](1502-r-site-mesure-et-plan.md) | 2026-10-09 | `historical` | Measure of the Rules store and the write plan for #1502. |
| [1502-r-site-ecriture-2026-10-09.md](1502-r-site-ecriture-2026-10-09.md) | 2026-10-09 | `historical` | Write phase: the 6 Rules pages of the preprod synchronized to the cards. |
| [1781-i2-zone3-inventory.md](1781-i2-zone3-inventory.md) | 2026-10-09 | `historical` | Measured inventory of what the live portal's visible text is made of (zone 3). |
| [1781-i3-en-seed-verification-2026-10-09.md](1781-i3-en-seed-verification-2026-10-09.md) | 2026-10-09 | `historical` | Verification of the zone-2 EN seed. |
| [1781-i3-a-sans-consommateur-2026-10-09.md](1781-i3-a-sans-consommateur-2026-10-09.md) | 2026-10-09 | `historical` | Stop-before-write: the EN seed of the 1 013 fallacies has no consumer on the site. |
| [1781-i1b-culture-dimensions-2026-10-10.md](1781-i1b-culture-dimensions-2026-10-10.md) | 2026-10-10 | `historical` | The 6 culture dimensions provisioned in zone 3. |
| [1781-i3-en-regles-en-2026-10-11.md](1781-i3-en-regles-en-2026-10-11.md) | 2026-10-11 | `historical` | 27 en-US values written for the 5 Rules sheets. |
| [2182-articles-actus-et-liens-mindmap-2026-11-09.md](2182-articles-actus-et-liens-mindmap-2026-11-09.md) | 2026-11-09 | `historical` | 7 Actus articles and 7 mind-map links, measured in the window of 09/11. |
| [2182-plan-injection-147-cellules-2026-11-11.md](2182-plan-injection-147-cellules-2026-11-11.md) | 2026-11-11 | `current` | Injection plan for the 147 cells, replayable from the repo. Source = the `eav-content.csv` registry; the apply is still to come (option (c) at the apply). |

---

## §5 — Data files

| File | Date | Status | What it is |
|---|---|---|---|
| [dnn-ui-strings.csv](dnn-ui-strings.csv) | 2026-10-26 | `current` | The 11-row UI-strings artifact of #490 (2 `ui.*` + 9 `res.*`), 8 languages. ⛔ Never touch — see §7. |
| [eav-content.csv](eav-content.csv) | 2026-11-19 | `current` | The **injection registry** (2sxc EAV pivot, 8 language columns). Read by the 147-cell plan and by `tools/458-eav-content-export.py verify`. |
| [download-cards-canon.csv](download-cards-canon.csv) | 2026-10-28 | `historical` | Canonical download labels (Tarot/Poker, full/light) in 8 languages. No wired consumer measured on 2026-11-24 (no reference in `.md`/`.py`/`.cs`/`.json`). |

---

## §6 — Subfolders

| Folder | Status | What it is |
|---|---|---|
| [release-validation/](release-validation/) | `current` | The site-validation dossier: **30 tracked files** (README + 3 guides + 26 exports). Release gate still active — the dossier prepares validation, it does not replace it. ⚠️ Its own README still carries a 2026-06-16 "Skeleton / site not publicly deployed" status that is **stale since the 24/11 swap**; the files themselves remain the support of the validation to come. |
| [preuves/](preuves/) | `historical` | 4 PNG evidence captures (ar RTL, EN/FR navbar at 1440 px, K2 ontology tree), 09–10/10. |

---

## §7 — Hard constraints (apply to every doc here)

- **No production content mutation without jsboige GO** — no DB write, no 2sxc content change, no live-site edit without explicit authorization. The runbooks were the *plan*; re-running them now is only for a re-deploy / rollback.
- **NEVER touch** `dnn-ui-strings.csv` (merged via #490; gpt-5.5 only).
- **Worker signals, does not declare PASS** — visual site verdict = jsboige/ai-01; prod operations = jsboige's ops task.
- **No production write by a worker lane** — production execution is web2's; a worker lane prepares, measures and hands over.

---

## §8 — `458-*` records, tranche 2/4 (49 files)

The **134 `458-*` files** of this folder are the working records of pool #458: 73 `.md` reports, 55
`.translations.json` fragments, 1 other `.json`, 4 `.py` builders and 1 `.csv` pivot (counted 2026-11-24). Tranche 1 called them
"the injection records"; most are not. They are dated measurements, notes of preprod writes, and the payloads
of the language packs. They are indexed in tranches cut on `git ls-files` order, so that the membership of a
tranche is a fact to re-derive rather than a judgement to review:

```text
git ls-files docs/dnn-localization | grep -E '^docs/dnn-localization/458-[^/]+$'
```

This tranche takes the **first 49** — everything before the first builder, `458-item9bis-agr-core-extract.py`:
**30 `.md`, 18 `.json`, 1 `.csv`**. Tranches 3 and 4 take the **other 85**.

Each row reads its file's own header. The **Date** column is the date the record bears — its header, which
for these documents is also in the file name — so it can precede the commit that carried it; the two data
files bear no date of their own and are dated by their last commit. A figure followed by a date of
2026-11-24 was taken for this index, on master (`git ls-files`, a parse of the file) or on the served
production site (a GET); nothing else was replayed, on no base. A pull request number is read from the
subject of the commit that carried the file on master, never from another report's citation.

### §8.1 — Content written on preprod (10 files)

**Where it stands on 2026-11-24.** Every write recorded here was made on the **preprod** base, between
2026-10-24 and 2026-11-06, before the production swap of 2026-11-24 (§0). Two findings outlive their grain
and are carried in their rows: the registry is not a mirror of the base — `verify` measures a gap, never a
stale artefact — and a content read is taken on a fresh render, never on the DNN output cache.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-24b-4-app59-injection-2026-10-24.md](458-24b-4-app59-injection-2026-10-24.md) | 2026-10-24 | `historical` | App 59 (Glossaire) written on preprod: 98 cells × 7 languages, dry run 98/98 then apply (PR #1967) — the five translations had been merged by #1924 and never written to the base. §6 leaves three points open; the first says not to purge the 27 `Game Rule` rows on the strength of the pool — the base already carries their ar/fa titles, byte for byte equal to the cards' headings (the arbitration is ai-01's, not a pool item's). |
| [458-app35-injection-2026-10-25.md](458-app35-injection-2026-10-25.md) | 2026-10-25 | `historical` | App 35 (persona accordions) written on preprod: 105 cells × 7 languages, dry run 105/105 then apply (PR #1971). Four demo titles stay English in the 8 cultures — demo content, rendered on no public page. |
| [458-item8-memo-card-name-prefix-2026-10-25.md](458-item8-memo-card-name-prefix-2026-10-25.md) | 2026-10-25 | `historical` | Read-only finding: the memo-card name prefix is served in French on ru/ar/fa/zh, and it is the registry that gets injected, not the templates (PR #1968). Its correctif was ready, not executed; the repository half landed the same day in #1979 — the registry row filled (4 cells) and the tool's comment rewritten — while the field stays in the tool's `NEVER` list (re-read 2026-11-24). The base half was not written from that file. |
| [458-app49-injection-2026-10-25.md](458-app49-injection-2026-10-25.md) | 2026-10-25 | `historical` | App 49 (presets of the content slider) written on preprod: 70 cells × 7 languages, pure addition (PR #1980; last touched by #1994). §5 records the instrument trap: the DNN output cache answered a false negative right after the recycle — a content read is taken on a fresh render. |
| [458-app52-actus-h1-2026-10-25.md](458-app52-actus-h1-2026-10-25.md) | 2026-10-25 | `historical` | Two findings (PR #1987). The `h1` of Actus is a `ViewConfiguration.Title`, not a `Content` row: the export filter is corrected, self-tested (585 lines, 0 gap; a mutated cell seen exactly once). And §5: the registry is not a mirror of the base — 129 cells differ, in both directions — so `verify` measures a registry↔base gap, never a stale artefact. |
| [458-cultname-ru-2026-10-25.md](458-cultname-ru-2026-10-25.md) | 2026-10-25 | `historical` | One preprod transaction: `Languages.CultureName` for ru-RU goes from the raw code to the native name (PR #1982), after an INFO post and a green dry run. |
| [458-contact-newsletter-injection-2026-11-04.md](458-contact-newsletter-injection-2026-11-04.md) | 2026-11-04 | `historical` | Contact form and newsletter written on preprod: 24 labels of app 50 (entity E9169) + 1 `Html` of app 48, × 7 cultures = 175 cells, method E1, served proof (PR #2139; erratum #2145 on the identity range a cancelled Rehearse had consumed). The French error messages it left for a later grain are the next row. |
| [458-e9169-fr8-base-2026-11-04.md](458-e9169-fr8-base-2026-11-04.md) | 2026-11-04 | `historical` | The 8 French error messages of the contact form (base values, dimension 0, entity E9169) aligned on the `fr` column of the registry — backup, dry run, transaction (PR #2142). Closes the residue of the previous row. |
| [458-eav-70-cellules-2026-11-05.md](458-eav-70-cellules-2026-11-05.md) | 2026-11-05 | `historical` | The 70 registry cells of apps 52/33/49 triaged: 42 translated for the write, 28 declined by construction (2sxc defaults — titles, `UrlKey`, demo placeholders); no base write in that grain (PR #2164, completed by #2210). The write of the 42 is the next row. |
| [458-inject42-resultat-2026-11-06.md](458-inject42-resultat-2026-11-06.md) | 2026-11-06 | `historical` | Run record of the 42-cell write on preprod (06/11, method E1): before/after proofs, rollback, and a guard on the identity of the rows — a count is not an identity — validated by four mutation passes (PR #2171, completed by #2213). |

### §8.2 — Files written in the repository: resx (9 files)

**Where it stands on 2026-11-24.** The nine changes are on master. Each of the six `GlobalResources.<culture>.resx`
carries `Administrators.RoleDescription`, one of the 30 keys the 10.4.1 upgrade added to the neutral (six files
parsed), and `security.es-ES.resx` has no `fitro` left (read that day).

| File | Date | Status | What it is |
|---|---|---|---|
| [458-eses-security-fitro-2026-11-02.md](458-eses-security-fitro-2026-11-02.md) | 2026-11-02 | `historical` | One typo in `security.es-ES.resx`: `fitro` → `filtro` (PR #2120; erratum on a witness name, PR #2122). Re-measured 2026-11-24 on master: 0 `fitro`. |
| [458-adminresethelp-resync-2026-11-03.md](458-adminresethelp-resync-2026-11-03.md) | 2026-11-03 | `historical` | `Password.ascx.es-ES.resx` and `.ru-RU.resx` resynchronised on the neutral for `AdminResetHelp`, with a guard (PR #2129). |
| [458-globalresources-frfr-30-cles-2026-11-03.md](458-globalresources-frfr-30-cles-2026-11-03.md) | 2026-11-03 | `historical` | First of the lot: what the DNN 10.4.1 upgrade added to the neutral `GlobalResources.resx` — 30 keys, in three families — and their French translation, 30 cells added, none touched (PR #2133). |
| [458-email-4th-phrase-ar-fa-zh-2026-11-03.md](458-email-4th-phrase-ar-fa-zh-2026-11-03.md) | 2026-11-03 | `historical` | The sentence DNN 10.4.1 added to `EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY` translated in the ar-SA, fa-IR and zh-CN packs, and the drift declaration retired; 52/52 green (commit `d4b2ac3f`). Its "remaining" note — the 30 keys in the five other packs — is closed by the five rows below. |
| [458-globalresources-ptbr-30-cles-2026-11-04.md](458-globalresources-ptbr-30-cles-2026-11-04.md) | 2026-11-04 | `historical` | pt-BR twin of the fr-FR grain: the same 30 keys, 30 cells added, none touched (PR #2143). |
| [458-globalresources-ruru-30-cles-2026-11-04.md](458-globalresources-ruru-30-cles-2026-11-04.md) | 2026-11-04 | `historical` | ru-RU twin of the fr-FR and pt-BR grains: the same 30 keys, 30 cells added, none touched (PR #2147). |
| [458-globalresources-arsa-30-cles-2026-11-04.md](458-globalresources-arsa-30-cles-2026-11-04.md) | 2026-11-04 | `historical` | ar-SA twin of the fr-FR, pt-BR and ru-RU grains: the same 30 keys, 30 cells added, none touched (PR #2149). |
| [458-globalresources-fair-30-cles-2026-11-05.md](458-globalresources-fair-30-cles-2026-11-05.md) | 2026-11-05 | `historical` | fa-IR: the same 30 keys, 30 cells added, none touched (PR #2151). |
| [458-globalresources-zhcn-30-cles-2026-11-05.md](458-globalresources-zhcn-30-cles-2026-11-05.md) | 2026-11-05 | `historical` | zh-CN: the same 30 keys, 30 cells added, none touched — last pack of the lot (PR #2152). |

### §8.3 — Site state: webroot, language lever, chrome, shop (9 files)

**Where it stands on 2026-11-24.** These files measure a site that has since moved: production was swapped on
2026-11-24 (§0), and today's state is read there, not here. Seven rows are the record of a date.
Two are `current`, and they belong to one dossier — the « Buy the game » product sheet: prepared, pivot
versioned, not written; the write waits for an arbitration. Two rows carry a measurement taken on the served
production that day, marked ⚠️.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-etape3-webroot-sync-simulation-2026-10-12.md](458-etape3-webroot-sync-simulation-2026-10-12.md) | 2026-10-12 | `historical` | Step 3 of the preprod webroot recovery, rehearsed on a copy: the naïve test, refuted by execution; the protected sequence and its checks (PR #1873). Its object is the clone of that date — 506 commits behind, 226 tracked files modified — and the real execution stayed frozen until ai-01's verdict. |
| [458-item9-langpack-ar-fa-2026-10-25.md](458-item9-langpack-ar-fa-2026-10-25.md) | 2026-10-25 | `historical` | Read-only state of the ar/fa base chrome: no upstream pack to install, English fallback, zero chrome resx for zh/ar/fa outside the skin rail (PR #1970). §6 offers options and executes none; the fallback chrome it scoped is the next row. |
| [458-chrome-minimal-zh-ar-fa-2026-10-25.md](458-chrome-minimal-zh-ar-fa-2026-10-25.md) | 2026-10-25 | `historical` | The 13 keys that made zh/ar/fa fall back to English on the login path — 4 carrier files, 39 triples — proved by the rendered page, not deduced from the module (PR #1972). Read-only by itself; executed the next day by PR #1995: the 13 keys are on master in the three cultures (four carriers parsed in each, 2026-11-24). |
| [458-hreflang-skin-2026-10-25.md](458-hreflang-skin-2026-10-25.md) | 2026-10-25 | `historical` | The 2shineBS5 skin emits `hreflang` alternates, canonical and x-default from the portal's languages (PR #1989 — preprod only at the time). ⚠️ Measured 2026-11-24 on the served production: the three home pages probed (`/`, `/en/`, `/ar/`) each carry one `rel="canonical"` and **no `hreflang`**, although `EmitLanguageAlternates()` is on master (`controls/theme-body.ascx`). |
| [458-cl-flag-origin-2026-10-26.md](458-cl-flag-origin-2026-10-26.md) | 2026-10-26 | `historical` | Origin of the `ContentLocalizationEnabled=True` write of 11/10: the residue of a lane experiment, never reverted — not an intention (PR #2002). Established by four read-only instruments, as the preliminary of the go-live arbitration « Option B » (switching back to `False`); whether that switch was made is not measured by this index. |
| [458-grain-crlf-accueil-acheter-2026-11-03.md](458-grain-crlf-accueil-acheter-2026-11-03.md) | 2026-11-03 | `historical` | Two preprod gestures done — the webroot clone switched to `core.autocrlf=true`, 1 751 status lines brought back to 22; and the home-page typo, base and pivot — and the « Buy the game » dossier measured without a gesture (PR #2134). The dossier continues in the two rows below. |
| [458-buy-the-game-item131-enus-2026-11-03.md](458-buy-the-game-item131-enus-2026-11-03.md) | 2026-11-03 | `current` | **Preparation only** — no base write (PR #2136). The product sheet of the « Buy the game » link renders in French whatever the culture asked. It stops before the write: three objects carry the product, the planned chain cannot run as dispatched, and three questions are put to ai-01. It governs a gesture still to come. ⚠️ Measured 2026-11-24 on the served production: `/en/Acheter-le-jeu/…` still answers **301** to the same URL without `/en/`; the arbitration has no line in the #458 pool body read that day. |
| [458-buy-the-game-item131-pivot.csv](458-buy-the-game-item131-pivot.csv) | 2026-11-04 | `current` | The payload of the previous row: 5 records of item 131 (`PRDLANG`, `fr-FR`) — name, summary, HTML description, two image captions — French against en-US, in 23 physical lines because the description cell spans several (parsed 2026-11-24), versioned before any gesture on the base. No file on master cites it by name (`git grep`): it waits for the write the previous row gates. |
| [458-ff-preprod-66b28511-essai-a-blanc-2026-11-05.md](458-ff-preprod-66b28511-essai-a-blanc-2026-11-05.md) | 2026-11-05 | `historical` | Read-only rehearsal of the preprod webroot advance to `66b28511`, before the daytime window (committed with PR #2148). It is the rehearsal of one advance; its last section says to re-measure before the `merge --ff-only`, never on the faith of that document. |

### §8.4 — Admin language packs, module-level lots 2, 4 and 6 (21 files)

**Where it stands on 2026-11-24.** The three packs are on master, in the parity of the other cultures: each of
the seven cultures has 28 `<culture>.resx` under the six `DNNPlatform/admin/*/App_LocalResources/`
directories, and the pack covers 27 of them — `Skins/Login.ascx` is excluded on purpose, its zh/ar/fa files
existed already (`git log --diff-filter=A`: added 2026-10-26). Four file names exist in two directories each — `Export`
and `Import` in `Modules` and `Tabs`, `Privacy` and `Terms` in `Portal` and `Skins` — so a name identifies
a file only with its directory. The 18 JSON fragments are the **source** of the merged `.resx`: the guards
`DnnModuleLevelAdmin{Zh,Ar,Fa}ResxGuardTests` read the `.resx`, and no file under `tools/` or `Generation/`
cites a fragment — the only `.translations.json` cited there is the older `684-translations.json`
(`git grep`).

| File | Date | Status | What it is |
|---|---|---|---|
| [458-item9bis-admin-zh-2026-10-31.md](458-item9bis-admin-zh-2026-10-31.md) | 2026-10-31 | `historical` | Lot 2: the zh-CN pack of the six admin directories — 27 files, 328 keys of the 338 neutrals, 10 declared not delivered (neutral fallback) (PR #2088). |
| [458-item9bis-admin-zh-CN.modules.translations.json](458-item9bis-admin-zh-CN.modules.translations.json) | 2026-10-31 | `historical` | Source fragment of the previous row — `Modules`: 6 files, 165 keys. |
| [458-item9bis-admin-zh-CN.portal.translations.json](458-item9bis-admin-zh-CN.portal.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Portal`: 2 files, 2 keys. |
| [458-item9bis-admin-zh-CN.security.translations.json](458-item9bis-admin-zh-CN.security.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Security`: 3 files, 54 keys. |
| [458-item9bis-admin-zh-CN.skins.translations.json](458-item9bis-admin-zh-CN.skins.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Skins`: 13 files, 52 keys. |
| [458-item9bis-admin-zh-CN.tabs.translations.json](458-item9bis-admin-zh-CN.tabs.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Tabs`: 2 files, 53 keys. |
| [458-item9bis-admin-zh-CN.users.translations.json](458-item9bis-admin-zh-CN.users.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Users`: 1 file, 2 keys. |
| [458-item9bis-admin-ar-2026-10-31.md](458-item9bis-admin-ar-2026-10-31.md) | 2026-10-31 | `historical` | Lot 4: the same perimeter for ar-SA — 27 files, 328 keys, key sets equal to the zh-CN lot stem by stem (PR #2093). |
| [458-item9bis-admin-ar-SA.modules.translations.json](458-item9bis-admin-ar-SA.modules.translations.json) | 2026-10-31 | `historical` | Source fragment of the previous row — `Modules`: 6 files, 165 keys. |
| [458-item9bis-admin-ar-SA.portal.translations.json](458-item9bis-admin-ar-SA.portal.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Portal`: 2 files, 2 keys. |
| [458-item9bis-admin-ar-SA.security.translations.json](458-item9bis-admin-ar-SA.security.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Security`: 3 files, 54 keys. |
| [458-item9bis-admin-ar-SA.skins.translations.json](458-item9bis-admin-ar-SA.skins.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Skins`: 13 files, 52 keys. |
| [458-item9bis-admin-ar-SA.tabs.translations.json](458-item9bis-admin-ar-SA.tabs.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Tabs`: 2 files, 53 keys. |
| [458-item9bis-admin-ar-SA.users.translations.json](458-item9bis-admin-ar-SA.users.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Users`: 1 file, 2 keys. |
| [458-item9bis-admin-fa-2026-10-31.md](458-item9bis-admin-fa-2026-10-31.md) | 2026-10-31 | `historical` | Lot 6: the same perimeter for fa-IR — 27 files, 328 keys, translations authored by the lane, 0 $ of API (PR #2097). |
| [458-item9bis-admin-fa-IR.modules.translations.json](458-item9bis-admin-fa-IR.modules.translations.json) | 2026-10-31 | `historical` | Source fragment of the previous row — `Modules`: 6 files, 165 keys. |
| [458-item9bis-admin-fa-IR.portal.translations.json](458-item9bis-admin-fa-IR.portal.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Portal`: 2 files, 2 keys. |
| [458-item9bis-admin-fa-IR.security.translations.json](458-item9bis-admin-fa-IR.security.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Security`: 3 files, 54 keys. |
| [458-item9bis-admin-fa-IR.skins.translations.json](458-item9bis-admin-fa-IR.skins.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Skins`: 13 files, 52 keys. |
| [458-item9bis-admin-fa-IR.tabs.translations.json](458-item9bis-admin-fa-IR.tabs.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Tabs`: 2 files, 53 keys. |
| [458-item9bis-admin-fa-IR.users.translations.json](458-item9bis-admin-fa-IR.users.translations.json) | 2026-10-31 | `historical` | Source fragment of the same pack — `Users`: 1 file, 2 keys. |

---

## §9 — `458-*` records, tranche 3/4 (50 files)

This tranche takes the **50 that follow** the first 49 of §8, cut the same way on `git ls-files` order:
from the first builder, `458-item9bis-agr-core-extract.py`, to the last `dtm-zh-CN` fragment. It is
**14 `.md`, 35 `.translations.json`, 1 `.py`**. Tranche 4 takes the **35 that remain** and begins at
`458-item9bis-fa-IR.translations.json` — the ar-SA core fragment is here, its fa-IR and zh-CN sisters are
there: the cut is mechanical, never thematic.

The row conventions of §8 apply unchanged. Two families of figures were re-measured for this index on
**2026-11-25**, on master `6b101880`, with no base replayed: the per-culture key counts of the perimeters
these lots touch (`App_GlobalResources`, `controls`, and `DesktopModules` minus PersonaBar, NBright and
`Install/`), and the fidelity of **each of the 35 fragments to the `.resx` it fed** — 34 are exact (every key
identical, none absent, none added on master since), the ar-SA core fragment is the 35th and its single
difference is named in §9.1. A fragment's Date is the **last commit that touched it**; the one report with
neither a date in its name nor in its header, `458-item9bis-ar-sa-pack.md`, is dated by the commit that
carried it. No file under `tools/` or `Generation/` cites a fragment — the guards read the `.resx` — and the
only names of this tranche cited outside `docs/` are the three `controls` reports, each by its own guard.

### §9.1 — The AGR core family, grain ⑨-bis (5 files)

**Where it stands on 2026-11-25.** The six families of `DNNPlatform/App_GlobalResources` carry **6 cultural
files and 1 254 keys per culture** — SharedResources 610, List_Country 256, GlobalResources 202,
Exceptions 132, WebControls 50, FileUpload 4 — against a neutral of 1 270 keys (617, 256, 211, 132, 50, 4;
`List_Country.resx` is a **shell**: 256 keys, not one value). Two movements since the measure below: the
neutral `GlobalResources.resx` went from 182 to 211 keys around the 10.4.1 mount, whose key lot §8.2
translated — ar-SA's 202 decompose as the fragment's 170 + those 30 + the two arbitrated legal values
(`MESSAGE_PORTAL_PRIVACY`, `MESSAGE_PORTAL_TERMS`) — and the pack is **10 neutral keys short**, exactly the
set the AGR guard declares (10 of 10 names found in its declaration, 2 of them empty in the neutral), while
it still carries one legacy key the neutral no longer has (`ActivatingLicense.Text`). That is why 202 is
not 211 minus 9: a count of keys is not a difference of sets. The `List_Country` 256 keys came from the
separate CLDR grain; its builder and guard sit in tranche 4.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-item9bis-agr-core-extract.py](458-item9bis-agr-core-extract.py) | 2026-10-30 | `historical` | Read-only measure and worklist instrument of the AGR core family (ar-SA, fa-IR, zh-CN): six families, `{0}`/`{1}` placeholders pinned, a positive control, and the two rules the measure had to state — a neutral may be a **shell** (`List_Country.resx`), and an empty neutral key in a non-shell file is not material. It writes only the path given to `--write`, outside `DNNPlatform`. Carried by the squashed commit of #2067–#2070 (`efb82ae0`); cited by the row below and by this index, by no code. |
| [458-item9bis-agr-core-mesure-2026-10-30.md](458-item9bis-agr-core-mesure-2026-10-30.md) | 2026-10-30 | `historical` | The denominator, measured before the packs: **1 227 keys per language, not 1 261** — SharedResources 607 to produce + List_Country 256 + GlobalResources 178 + Exceptions 132 + WebControls 50 + FileUpload 4 (PR #2065). It reconciles with the previously published 1 241, names the four systematic gaps, gives in §4 the facts its DoD uses and in §5 the reproduction. Re-measured 2026-11-25: the six families now read **1 254 keys per culture**, the neutral's `GlobalResources` having grown with the 10.4.1 mount. |
| [458-item9bis-allowed-missing-by-couple-2026-10-30.md](458-item9bis-allowed-missing-by-couple-2026-10-30.md) | 2026-10-30 | `historical` | The guard change, no `.resx` touched (PR #2086, carried on 2026-10-31): the declared-missing set moves from per file to per **(file, culture)** couple — web2's hand-off on the counter-review of #2084 — with two declarations, one resolution, a failure message that names the applied perimeter, and three guards on the declaration itself. Live on master (re-read 2026-11-25: `AllowedMissingByFileAndCulture`, `AllowedMissingDefaultByStem`, one `Resolve(...)`, 8 tests). |
| [458-item9bis-ar-SA.translations.json](458-item9bis-ar-SA.translations.json) | 2026-10-30 | `historical` | Source fragment of the ar-SA core packs — 5 files, **966 keys** (FileUpload 4, WebControls 50, Exceptions 132, GlobalResources 170, SharedResources 610 added on 30/10 by #2079–#2081). Against master 2026-11-25: **965 keys byte-identical, 1 differs** — `GlobalResources::EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY.Text`, re-translated by the §8.2 grain (`d4b2ac3f`) — 0 absent. |
| [458-item9bis-ar-sa-pack.md](458-item9bis-ar-sa-pack.md) | 2026-10-30 | `historical` | Lot 2, the ar-SA pack, **partial as designed: 358 keys** of the 1 241 then counted, preprod and webroot untouched (PR #2068, in the squashed commit `efb82ae0`; last touched by the #2084 errata on 2026-10-31). Its status block is a statement of 30/10 — "SharedResources untouched, List_Country not created": both landed the same day and the next, and ar-SA carries 1 254 keys in the six families today. Its two legal values stay the arbitrated ones. |

### §9.2 — The `controls` grain, three cultures (6 files)

**Where it stands on 2026-11-25.** The eleven `controls` families carry **11 cultural files and 133 keys in
each of the three cultures, with no key short of the neutral** (11 of 11, écart 0, parsed). The three guards
`DnnModuleLevelControls{Zh,Ar,Fa}ResxGuardTests` each cite their lot's report — the only citations of a
tranche-3 file outside `docs/`. The grain's denominator was #2072's CORE without `Install/` (94 files /
2 168 keys per language); these eleven families are one brick of it, and `Install/` (2 files / 187 keys)
stays recommended to exclusion.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-item9bis-controls-ar-2026-10-31.md](458-item9bis-controls-ar-2026-10-31.md) | 2026-10-31 | `historical` | Lot 3, the ar-SA `controls` pack: 11 files / 133 keys, ar-SA only; none of the eleven families had ar-SA before (master `991e7816`); two values are Latin by nature and declared; the guard was factorised, not copied (PR #2092). |
| [458-item9bis-controls-ar-SA.translations.json](458-item9bis-controls-ar-SA.translations.json) | 2026-10-31 | `historical` | Source fragment of the row above — 11 files, 133 keys (Address 34, URLControl 27, User 24, TextEditor 13, UrlTrackingControl 9, SkinControl 7, DualListControl 6, filepickeruploader 4, Help 4, ModuleAuditControl 3, LocaleSelectorControl 2). |
| [458-item9bis-controls-fa-2026-10-31.md](458-item9bis-controls-fa-2026-10-31.md) | 2026-10-31 | `historical` | Lot 3, third culture after zh-CN (#2087) and ar-SA (#2092): 11 files / 133 keys, source = the fragment below, 0 API call; builder `--lang fa-IR --root DNNPlatform/controls/App_LocalResources --bom mirror`; five inverse controls (PR #2096). Branch `feat/458-modulelevel-controls-fa`. |
| [458-item9bis-controls-fa-IR.translations.json](458-item9bis-controls-fa-IR.translations.json) | 2026-10-31 | `historical` | Source fragment of the row above — the same 11 files, 133 keys. |
| [458-item9bis-controls-zh-2026-10-30.md](458-item9bis-controls-zh-2026-10-30.md) | 2026-10-30 | `historical` | Lot 3, first brick of the grain: zh-CN, 11 files / 133 keys, on ai-01's order of 30/10 (rang ③); the builder was extended by a flag, never doubled; the guard is not backed onto `ru-RU`; BOM and EOL measured on the committed blob (#2078); three mutations; `Install/` recommended to exclusion by #2072 (PR #2087). |
| [458-item9bis-controls-zh-CN.translations.json](458-item9bis-controls-zh-CN.translations.json) | 2026-10-30 | `historical` | Source fragment of the row above — the same 11 files, 133 keys. |

### §9.3 — `DesktopModules/Admin`, lot 3b and its two twins (9 files)

**Where it stands on 2026-11-25.** The lot delivered **20 files / 640 keys per culture** of 645 neutral
ones, the 5 short being declared and pinned: `Security/ProfileDefinitions.ascx` 4 (the grid headers are
empty in the neutral) and `ViewProfile/SharedResources` 1 (pure tokens). The perimeter has moved since: the
three cultures now hold **21 files / 647 keys** in these two directories, because
`Security/DataConsent.ascx` (7 keys) was outside the lot and was delivered by the residual grain (#2106,
#2115, #2141) — the fa report below says it "stays out of lot here, and it stays so" as of 2026-11-01,
which today's tree refutes for all three cultures. `Membership.ascx` carries two keys the neutral does not
(30 against 28) — a legacy residue, not a gap. And the zh report's §8 announces lot 3c as "9 files /
381 keys": a pool label, corrected to **37 / 689** by the lot that delivered it (row 41).

| File | Date | Status | What it is |
|---|---|---|---|
| [458-item9bis-dtm-admin-ar-2026-11-01.md](458-item9bis-dtm-admin-ar-2026-11-01.md) | 2026-11-01 | `historical` | Lot 3b, Arabic twin of the zh report below: 20 files / 640 keys of 645 neutrals, 5 declared not delivered, 6 verbatim; the tree `DesktopModules/Admin` counts 65 files / 4 395 keys and the lot is the sister guard's `Covered` table (20 / 645); the 45 others belonged to no delivered lot at that date; it adds the « Persian ≠ Arabic » direction to the check (PR #2109). |
| [458-item9bis-dtm-admin-ar-SA.security.translations.json](458-item9bis-dtm-admin-ar-SA.security.translations.json) | 2026-11-01 | `historical` | Source fragment of the row above — `Security`: 17 files, 622 keys. |
| [458-item9bis-dtm-admin-ar-SA.viewprofile.translations.json](458-item9bis-dtm-admin-ar-SA.viewprofile.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `ViewProfile`: 3 files, 18 keys (Settings 7, ViewProfile 11, SharedResources 0). |
| [458-item9bis-dtm-admin-fa-2026-11-01.md](458-item9bis-dtm-admin-fa-2026-11-01.md) | 2026-11-01 | `historical` | Lot 3b, Persian twin, the same numbers. It names the out-of-lot `Security/DataConsent.ascx` (7 neutral keys, sisters es-ES/ru-RU/zh-CN from the original DNN packs) and concludes it "stays out of lot here, and it stays so" — delivered for the three cultures on 2026-11-01/04 (see the heading) (PR #2111). |
| [458-item9bis-dtm-admin-fa-IR.security.translations.json](458-item9bis-dtm-admin-fa-IR.security.translations.json) | 2026-11-01 | `historical` | Source fragment of the row above — `Security`: 17 files, 622 keys. |
| [458-item9bis-dtm-admin-fa-IR.viewprofile.translations.json](458-item9bis-dtm-admin-fa-IR.viewprofile.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `ViewProfile`: 3 files, 18 keys. |
| [458-item9bis-dtm-admin-zh-2026-10-31.md](458-item9bis-dtm-admin-zh-2026-10-31.md) | 2026-10-31 | `historical` | Lot 3b, zh-CN: 20 files / 640 keys; the denominator re-measured with `458-item9bis-modulelevel-extract.py --json` on `991e7816` (Security 17 / 626, ViewProfile 3 / 19); the `SharedResources` stem collides in two directories, so the guard identifies a file by (directory, stem); builder `--bom mirror`, 17 of the 20 neutrals without BOM; the five undelivered are named (four empty grid headers of `ProfileDefinitions`, the pure tokens of `ViewProfile/SharedResources`), six verbatim; its §8 announces lot 3c as "9 files / 381 keys" — a pool label, re-measured 37 / 689 by row 41 (PR #2095). |
| [458-item9bis-dtm-admin-zh-CN.security.translations.json](458-item9bis-dtm-admin-zh-CN.security.translations.json) | 2026-10-31 | `historical` | Source fragment of the row above — `Security`: 17 files, 622 keys. |
| [458-item9bis-dtm-admin-zh-CN.viewprofile.translations.json](458-item9bis-dtm-admin-zh-CN.viewprofile.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `ViewProfile`: 3 files, 18 keys. |

### §9.4 — `DesktopModules`: the « modules » lot and the residual (30 files)

**Where it stands on 2026-11-25.** Two blocks, one rule. The « modules » lot (3a) carries **27 files /
668 keys per culture** over the eight `DesktopModules/{…}/App_LocalResources` directories — 678 neutral keys
minus the 10 declared not delivered, and the measured écart is exactly those 10, nothing else. The residual
lot (4a for ar-SA, 3c for zh-CN) carries **37 files / 689 keys** in zh-CN and ar-SA and **696 in fa-IR**,
the seven extra keys being `Dnn.EditBar` `AddExistingModule` and `AddModule` (1 + 1) and five
`ResourceManager` keys (#2141). The rows below follow the `git ls-files` order, which groups by name: the zh
« modules » lot comes last although it opened the sequence on 2026-10-31.

Two measured limits, inside the same perimeters. **(a)** Two files are outside every lot **by
construction**, each having had a twin before the lots — the module-level measure excluded the three
`Login.ascx` as "already in Tier 1", which had delivered only a few keys: `Admin/Authentication/…/Login.ascx`
holds **3 keys of 40** in each culture — `cmdForgotPassword`, `cmdCreate`, `cmdPassword`, `AssociateHelp`,
`AuthenticatedTitle` are among the 37 missing — and `AuthenticationServices/DNN/…/Login.ascx` **5 of 9** (`Title`,
`InvalidVerificationCode`, `UserAlreadyVerified`, `UserDoesNotExist` missing) — **41 neutral keys with no
zh/ar/fa value**, named by no record of this folder and held by no guard
(`DnnVisibleResidualLangpackGuardTests` covers only the six social-button twins; each lot's guard keys on a
declared (directory, stem) list). **(b)** One neutral entered the residual perimeter **after** the lots:
`Connectors/GoogleAnalytics4/App_LocalResources/SharedResources.resx`, **5 keys, added by #2127 on
2026-11-03**, has no zh/ar/fa twin — re-derived today the residual rule selects **38 files, not 37**, and
the guards' closed lists cannot see it. Both are leads for the pool; this index executes neither.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-item9bis-dtm-ar-SA-2026-10-31.md](458-item9bis-dtm-ar-SA-2026-10-31.md) | 2026-10-31 | `historical` | Lot 3a, Arabic twin of the zh « modules » lot: 27 resx / 668 keys over the eight directories, translated by the lane (0 $), guard, independent verification and measured inverse controls (PR #2103). PersonaBar and NBright stay outside (#2072); the same 10 keys are declared not delivered as in the zh lot. Branch `feat/458-modulelevel-dtm-modules-ar`. |
| [458-item9bis-dtm-ar-SA.authservices.translations.json](458-item9bis-dtm-ar-SA.authservices.translations.json) | 2026-10-31 | `historical` | Source fragment of the row above — `AuthenticationServices/DNN`: 1 file (`Settings.ascx`), 5 keys. |
| [458-item9bis-dtm-ar-SA.coremessaging.translations.json](458-item9bis-dtm-ar-SA.coremessaging.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `CoreMessaging`: 2 files (`SharedResources` 26, `View.ascx` 114), 140 keys. |
| [458-item9bis-dtm-ar-SA.ddrmenu.translations.json](458-item9bis-dtm-ar-SA.ddrmenu.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `DDRMenu`: 2 files, 5 keys. |
| [458-item9bis-dtm-ar-SA.html.translations.json](458-item9bis-dtm-ar-SA.html.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `HTML`: 6 files (EditHtml 56, HtmlModule 21, MyWork 5, Settings 18, SharedResources 5, Workflow 31), 136 keys. |
| [458-item9bis-dtm-ar-SA.journal.translations.json](458-item9bis-dtm-ar-SA.journal.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `Journal`: 4 files (Edit 1, Settings 16, View 24, SharedResources 90), 131 keys. |
| [458-item9bis-dtm-ar-SA.memberdirectory.translations.json](458-item9bis-dtm-ar-SA.memberdirectory.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `MemberDirectory`: 2 files (Settings 70, View 43), 113 keys. |
| [458-item9bis-dtm-ar-SA.razorhost.translations.json](458-item9bis-dtm-ar-SA.razorhost.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `RazorModules/RazorHost`: 5 files (AddScript 9, CreateModule 23, EditScript 12, RazorHost 4, Settings 3), 51 keys. |
| [458-item9bis-dtm-ar-SA.socialgroups.translations.json](458-item9bis-dtm-ar-SA.socialgroups.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `SocialGroups`: 5 files (CreateSettings 5, Edit 3, ListSettings 23, SharedResources 55, View 1), 87 keys. |
| [458-item9bis-dtm-fa-2026-11-01.md](458-item9bis-dtm-fa-2026-11-01.md) | 2026-11-01 | `historical` | Lot 3a, Persian twin of the zh (#2089) and ar-SA lots: the same 27 files / 668 keys and the same 10 declared; what it adds to the guard is the **Persian ≠ Arabic** direction, with a mutation bench (PR #2108). Branch `feat/458-modulelevel-dtm-modules-fa`. |
| [458-item9bis-dtm-fa-IR.authservices.translations.json](458-item9bis-dtm-fa-IR.authservices.translations.json) | 2026-11-01 | `historical` | Source fragment of the row above — `AuthenticationServices/DNN`: 1 file, 5 keys. |
| [458-item9bis-dtm-fa-IR.coremessaging.translations.json](458-item9bis-dtm-fa-IR.coremessaging.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `CoreMessaging`: 2 files, 140 keys. |
| [458-item9bis-dtm-fa-IR.ddrmenu.translations.json](458-item9bis-dtm-fa-IR.ddrmenu.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `DDRMenu`: 2 files, 5 keys. |
| [458-item9bis-dtm-fa-IR.html.translations.json](458-item9bis-dtm-fa-IR.html.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `HTML`: 6 files, 136 keys. |
| [458-item9bis-dtm-fa-IR.journal.translations.json](458-item9bis-dtm-fa-IR.journal.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `Journal`: 4 files, 131 keys. |
| [458-item9bis-dtm-fa-IR.memberdirectory.translations.json](458-item9bis-dtm-fa-IR.memberdirectory.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `MemberDirectory`: 2 files, 113 keys. |
| [458-item9bis-dtm-fa-IR.razorhost.translations.json](458-item9bis-dtm-fa-IR.razorhost.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `RazorHost`: 5 files, 51 keys. |
| [458-item9bis-dtm-fa-IR.socialgroups.translations.json](458-item9bis-dtm-fa-IR.socialgroups.translations.json) | 2026-11-01 | `historical` | Source fragment of the same lot — `SocialGroups`: 5 files, 87 keys. |
| [458-item9bis-dtm-residual-ar-2026-11-01.md](458-item9bis-dtm-residual-ar-2026-11-01.md) | 2026-11-01 | `historical` | Lot 4a, the residual of `DesktopModules` for ar-SA: 37 resx / 689 keys, Arabic twin of the zh lot delivered the same day — same files, same keys, same reference (the neutral) — so the report describes only the deltas (PR #2115). Base `3554741b`, 0 $ of API; seven files had no cultural sister at all (never localised, not an ar gap); `InvalidConfiguration.ascx` carries 0 key and stays out of delivery, declared; the write-direction control is inverted (décision 33) and ten verbatim values are pinned; §4 records two delivery defects, both in the tool. |
| [458-item9bis-dtm-residual-ar-SA.translations.json](458-item9bis-dtm-residual-ar-SA.translations.json) | 2026-11-01 | `historical` | Source fragment of the row above — a top-level `__verbatim__` (10 pinned pairs) and **37 repository paths, written without the `.resx` extension**, each a `{key: value}` map: 689 keys over Admin 19 / 446, AuthenticationServices 8 / 16, Connectors 3 / 28, ResourceManager 4 / 130, SiteExportImport 1 / 10, TelerikRemoval 1 / 45, ToSic.Sxc 1 / 14. Identical to master (689 of 689 keys, the 10 pairs equal to the neutral). |
| [458-item9bis-dtm-residual-zh-2026-11-01.md](458-item9bis-dtm-residual-zh-2026-11-01.md) | 2026-11-01 | `historical` | Lot 3c: the residual defined **mechanically** — every neutral `*.resx` under `DNNPlatform/DesktopModules/**/App_LocalResources/` without a `*.zh-CN.resx` twin, outside PersonaBar, NBright and `Install/` (PR #2106). The pool label said **9 files / 381 keys**; re-measured on the tree: **37 files / 689 keys** — recorded here and in the guard, never executed blind (décision 18). Base `d60b9471`. |
| [458-item9bis-dtm-zh-2026-10-31.md](458-item9bis-dtm-zh-2026-10-31.md) | 2026-10-31 | `historical` | Lot 3a, zh-CN « modules »: 27 resx / 668 keys (678 neutrals − 10 declared); the denominator re-measured with `458-item9bis-modulelevel-extract.py --json`; four stems collide massively (`Settings.ascx` ×5, `View.ascx` ×4, `SharedResources` ×4, `Edit.ascx` ×2), so the mono-`--root` builder ran eight times; BOM mirrors each neutral (3 of 27 without); the guard measures BOM and EOL on the committed blob; its §8 announces lot 3c as "9 files / 381 keys" — a pool label, see row 41 (PR #2089). |
| [458-item9bis-dtm-zh-CN.authservices.translations.json](458-item9bis-dtm-zh-CN.authservices.translations.json) | 2026-10-31 | `historical` | Source fragment of the row above — `AuthenticationServices/DNN`: 1 file, 5 keys. |
| [458-item9bis-dtm-zh-CN.coremessaging.translations.json](458-item9bis-dtm-zh-CN.coremessaging.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `CoreMessaging`: 2 files, 140 keys. |
| [458-item9bis-dtm-zh-CN.ddrmenu.translations.json](458-item9bis-dtm-zh-CN.ddrmenu.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `DDRMenu`: 2 files, 5 keys. |
| [458-item9bis-dtm-zh-CN.html.translations.json](458-item9bis-dtm-zh-CN.html.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `HTML`: 6 files, 136 keys. |
| [458-item9bis-dtm-zh-CN.journal.translations.json](458-item9bis-dtm-zh-CN.journal.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `Journal`: 4 files, 131 keys. |
| [458-item9bis-dtm-zh-CN.memberdirectory.translations.json](458-item9bis-dtm-zh-CN.memberdirectory.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `MemberDirectory`: 2 files, 113 keys. |
| [458-item9bis-dtm-zh-CN.razorhost.translations.json](458-item9bis-dtm-zh-CN.razorhost.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `RazorHost`: 5 files, 51 keys. |
| [458-item9bis-dtm-zh-CN.socialgroups.translations.json](458-item9bis-dtm-zh-CN.socialgroups.translations.json) | 2026-10-31 | `historical` | Source fragment of the same lot — `SocialGroups`: 5 files, 87 keys. |

---

## §10 — `458-*` records, tranche 4/4 (35 files)

This tranche takes the **35 that remain** after the 50 of §9, cut the same mechanical way on `git ls-files`
order: from `458-item9bis-fa-IR.translations.json` to `458-tier3-mobius5-brevo-2026-10-25.md`. It is
**29 `.md`, 3 data files** (the two fa-IR / zh-CN translation fragments and the CLDR table) **and 3 `.py`**
(the two pack builders and the module-level measure instrument). With it the **134 `458-*` files are all
indexed** — §8 49, §9 50, §10 35 — and §1–§6 carry the 48 root documents that are not `458-*`.

The row conventions of §8 apply unchanged. Three families of figures were re-measured for this index on
**2026-11-25**, on master `64a81a5d`, with no base replayed. The **two translation fragments against the
packs they fed**: 966 keys each, **965 byte-identical, 1 differs** in each — the same key,
`GlobalResources::EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY.Text`, re-translated after them by the
§8.2 grain (`d4b2ac3f`) — **0 absent**, while the pack holds **32 keys the fragment never had** (the 30 of
the 10.4.1 lot plus the two arbitrated legal values of §9.1). The **CLDR table against the three committed
`List_Country` packs**: 256 values per culture, **0 differing, 0 absent on either side**. And every guard
or instrument a row names was found on master — the **eleven** test classes and the one instrument
(`DnnPersianScriptMarkers`).

Unlike §9's fragments, **four files of this tranche are named outside the folder**, and three of them are
read by a test or a live tool — the build script excepted, its own row saying why it still counts:
`458-item9bis-list-country-cldr-47.json` and `458-item9bis-list-country-build.py` at
`DnnListCountryLocalizationGuardTests.cs:19-20` (the reproducible source of truth and its byte-exact
constructor), `458-langpack-residu-visible-2026-11-05.md` at
`DnnVisibleResidualLangpackGuardTests.cs:13` and `tools/457-p24-01-langpack-residu-visible.py:5` (source of
the measure a live tool replays), and `458-newsletter-store-chain-2026-10-26.md` at
`tools/458-eav-content-export.py:114` (the attribution the export relies on). A `.md` is dated by its own
date; a data file or a builder by the last commit that touched it; the two pack notes with neither a date
in their name nor in their header (`458-item9bis-fa-ir-pack.md`, `458-item9bis-zh-cn-pack.md`) are dated by
the commit that carried them.

Statuses of this tranche: **3 `current`**, **2 `superseded`**, **30 `historical`**.

### §10.1 — The AGR core family: the fa-IR and zh-CN packs, and the `List_Country` grain (11 files)

**Where it stands on 2026-11-25.** The core family is complete in the three cultures of the grain (ar-SA in
§8.2 and §9.1, fa-IR and zh-CN here): `SharedResources` 610, `GlobalResources` 202, `Exceptions` 132,
`WebControls` 50, `FileUpload` 4 — **998 keys**, the AGR core of the 1 254-key denominator §9.1 measured
on `6b101880`, which adds the 256 of `List_Country`. The two translation fragments in this list are the
inputs of the two packs, not copies of their output: each holds the 966 keys of its date, and the packs
have moved by 32 keys since (measured above). `List_Country` is the separate CLDR grain: three packs of
256 keys each, byte-identical to the table below, which the guard names as its source of truth — that is
why the table is the one `current` data file of the pair.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-item9bis-fa-IR.translations.json](458-item9bis-fa-IR.translations.json) | 2026-10-30 | `historical` | Source fragment of the fa-IR core packs — 5 files, **966 keys** (FileUpload 4, WebControls 50, Exceptions 132, GlobalResources 170, SharedResources 610). Against master 2026-11-25: **965 byte-identical, 1 differs** (`GlobalResources::EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY.Text`, re-translated by the §8.2 grain `d4b2ac3f`), 0 absent, and 32 pack keys that post-date it. Fed `458-item9bis-langpack-build.py`; carried by the squashed commit of #2067–#2070 (`efb82ae0`). |
| [458-item9bis-fa-ir-pack.md](458-item9bis-fa-ir-pack.md) | 2026-10-30 | `historical` | The fa-IR pack note (Tier 2, lot 3): the **same perimeter** as lots 1–2, the key set identical to zh-CN and ar-SA (measured at merge: the same 356 translated keys). Dated by the commit that carried it — no date in its name or header (#2067–#2070). |
| [458-item9bis-langpack-build.py](458-item9bis-langpack-build.py) | 2026-10-31 | `historical` | Builder of the AGR core packs: reads the neutral, writes `<family>.<culture>.resx` in `utf-8-sig`, `{0}`/`{1}` placeholders pinned. A one-shot authoring instrument — executed by no test, cited by no code; the guards read the `.resx` (#2067–#2070). |
| [458-item9bis-zh-CN.translations.json](458-item9bis-zh-CN.translations.json) | 2026-10-30 | `historical` | Source fragment of the zh-CN core packs — the same 5 files, **966 keys**; against master 2026-11-25 **965 byte-identical, 1 differs** (the same key as the fa-IR fragment), 0 absent, 32 pack keys post-dating it (#2067–#2070). |
| [458-item9bis-zh-cn-pack.md](458-item9bis-zh-cn-pack.md) | 2026-10-30 | `historical` | The zh-CN pack note: DNN resource files for the `App_GlobalResources` chrome — buttons, messages, validation, authentication mails: **static admin/portal content, no impact on the game cards or the PDFs**. Dated by the commit that carried it (#2067–#2070). |
| [458-item9bis-sharedresources-zh-2026-10-30.md](458-item9bis-sharedresources-zh-2026-10-30.md) | 2026-10-30 | `historical` | `SharedResources` zh-CN — the **first complete** pack of the family. Its errata of 30/10 (NanoClaw review): the UTF-8 **BOM is re-emitted on write**, not "preserved" — the builder reads the neutral in `utf-8-sig` and writes in `utf-8-sig` (#2079). |
| [458-item9bis-sharedresources-ar-2026-10-30.md](458-item9bis-sharedresources-ar-2026-10-30.md) | 2026-10-30 | `historical` | `SharedResources` ar-SA, the **second complete** pack: **3 keys → 610**; branch stacked on the zh-CN one (§6 of the report) (#2080). |
| [458-item9bis-sharedresources-fa-2026-10-30.md](458-item9bis-sharedresources-fa-2026-10-30.md) | 2026-10-30 | `historical` | `SharedResources` fa-IR, the **third complete** pack, with its errata: the BOM is re-emitted (§3), and the un-reproducible key figure of §5.1.3 is replaced by the measured count delivered at the guard's closure (2 297) and after merge (2 904) (#2081). |
| [458-item9bis-list-country-ar-fa-zh-2026-10-30.md](458-item9bis-list-country-ar-fa-zh-2026-10-30.md) | 2026-10-30 | `historical` | `List_Country` ar-SA / fa-IR / zh-CN: **3 × 256 keys from CLDR release-47**; the entry measure was 256 keys to produce per language with **no file for ar/fa/zh** (#2082). |
| [458-item9bis-list-country-build.py](458-item9bis-list-country-build.py) | 2026-10-30 | `current` | The **byte-exact constructor** of the three packs, from the neutral plus the CLDR table. Still load-bearing: `DnnListCountryLocalizationGuardTests.cs:20` names it as the reference constructor whose output the guard pins — deleting it breaks what "reproducible source of truth" means, even though no test executes it (#2082). |
| [458-item9bis-list-country-cldr-47.json](458-item9bis-list-country-cldr-47.json) | 2026-10-30 | `current` | The CLDR release-47 table: a `provenance` block (source, download date, version check against the ru-RU precedent, the regional-locale note) plus **256 values per culture** for ar-SA / fa-IR / zh-CN. **Still consumed** — the reproducible source of truth of the three committed packs, named at `DnnListCountryLocalizationGuardTests.cs:19`; re-measured 2026-11-25: **256/256 identical per culture**, 0 absent either way (#2082). |

### §10.2 — The `module-level` family: measure, residual perimeter and the guards' socle (7 files)

**Where it stands on 2026-11-25.** Every guard this family named exists on master and runs: the two fa-IR
`Controls`/`Admin` classes, the four zh-CN ones (`Controls`, `Admin`, `DtmAdmin`, `DtmModules`), and the
`DnnPersianScriptMarkers` instrument that keeps a Persian pack from passing the Arabic guards. The residual
perimeter's **v3** is the one that stands: **99 files / 5 664 keys** for ar-SA and fa-IR, with the state
after the lot 4a delivery (`623f8fd8`) recorded in the same report. The socle report leaves one declared
follow-up, a one-line conversion of the 15th hand-copied read-helper after the merge of #2111.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-item9bis-modulelevel-extract.py](458-item9bis-modulelevel-extract.py) | 2026-10-30 | `historical` | Read-only measure of the **MODULE-LEVEL** family (`App_LocalResources`): how many files, how many keys, and what is missing for ar-SA / fa-IR / zh-CN — the measure that gave **96 files, 2 355 keys per language** (#2072). |
| [458-item9bis-modulelevel-mesure-2026-10-30.md](458-item9bis-modulelevel-mesure-2026-10-30.md) | 2026-10-30 | `historical` | The real measure of the module-level family for the three cultures — 96 files, 2 355 keys per language — taken on the dispatch of ai-01 of 29/10 (§5 and §7 of #458) (#2072). |
| [458-item9bis-fa-ar-distinction-2026-10-31.md](458-item9bis-fa-ar-distinction-2026-10-31.md) | 2026-10-31 | `historical` | Distinguishing **Persian from Arabic** in the fa-IR guards (4 tests): `DnnModuleLevelControlsFaResxGuardTests`, `DnnModuleLevelAdminFaResxGuardTests`, plus the new `DnnPersianScriptMarkers` instrument — **0 `.resx` touched**. Born of the question « would a full copy of ar-SA pass the fa guards at 100 % ? » (NanoClaw review of #2097, item 6 of the po-2023 pool) (#2101). |
| [458-item9bis-zh-script-2026-10-31.md](458-item9bis-zh-script-2026-10-31.md) | 2026-10-31 | `historical` | Chinese-script control on the **three** zh-CN guards (**60 tests**): `DnnModuleLevelControlsZhResxGuardTests`, `DnnModuleLevelAdminZhResxGuardTests`, `DnnModuleLevelDtmAdminZhResxGuardTests` — 3 test files, 0 `.resx` touched (#2098). |
| [458-item9bis-zh-script-dtm-modules-2026-10-31.md](458-item9bis-zh-script-dtm-modules-2026-10-31.md) | 2026-10-31 | `historical` | The same control on the **fourth** zh-CN guard (**28 tests**): `DnnModuleLevelDtmModulesZhResxGuardTests`, unblocked by the merge of #2098 (#2105). |
| [458-item9bis-guard-socle-2026-11-01.md](458-item9bis-guard-socle-2026-11-01.md) | 2026-11-01 | `historical` | The guards' **socle**: the `CommittedBlob` primitive written once (`Dnn/DnnResxGuardSocle.cs`, with 4 facts that measure the socle itself) and **14 guards migrated** to a one-line delegation — −298 / +14 lines, no caller touched. Its error paths made explicit: a timeout that kills the process tree and names the command, a non-zero exit that says « commit **before** measuring », git absent from the PATH, an empty path refused before calling git. Plus the two 🟡 notes of #2105; one declared follow-up — converting the 15th copy after the merge of #2111 (#2112). |
| [458-item9bis-residual-perimeter-2026-11-01.md](458-item9bis-residual-perimeter-2026-11-01.md) | 2026-11-01 | `historical` | The **residual perimeter** ar-SA / fa-IR, v3: **99 files / 5 664 keys**. Its section on the 7 files of the « Reste » bucket names the fifth and costliest of five instrument errors — each time the *definition of the object* was wrong, never the tool applying it; a later section records the state after the lot 4a delivery (`623f8fd8`). Completed on 02/11 (#2113). |

### §10.3 — The langpack residual, two measures of 2026-11-05 (2 files)

**Where it stands on 2026-11-25.** The residual is carried by three families — NBrightBuy ×25, the admin
console, and pt-BR 105 families — and the dry measure's §4 holds a table of three go-live decisions with a
recommended answer each. That table is a **record of the recommendations**, not a live checklist: the swap
happened on 24/11 (§0). The *visible* measure is the one still replayed — it is the source of the measure of
`tools/457-p24-01-langpack-residu-visible.py` and was cited at the birth of
`DnnVisibleResidualLangpackGuardTests` (#2154), which is why it, and not its dry twin, carries `current`.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-langpack-residu-2026-11-05.md](458-langpack-residu-2026-11-05.md) | 2026-11-05 | `historical` | The residual, dry: per-culture counts, the three families carrying almost all of it, what it is **not**, and §4's table of the three decisions to take before go-live with a recommended answer each. Cross-check: es-ES "suivies" = **192** here and "master resx: 192" in the build (#2148). |
| [458-langpack-residu-visible-2026-11-05.md](458-langpack-residu-visible-2026-11-05.md) | 2026-11-05 | `current` | The **visible** part of the residual, measured *served* on preprod (7 pack cultures × 6 visitor surfaces, 05/11 ≈15:20Z; a hit = the exact English value of a residual neutral family present in the served body). Still the source of the measure replayed by `tools/457-p24-01-langpack-residu-visible.py:5` and cited by `DnnVisibleResidualLangpackGuardTests.cs:13` (#2154). |

### §10.4 — pt-BR and the fa-IR lot 4a base (4 files)

**Where it stands on 2026-11-25.** The three gestures this group measured are guarded on master:
`DnnPtBrContinentalResxGuardTests`, `DnnSecurityDeadKeysPtBrGuardTests` (the 13 dead keys),
`DnnSecurityPtBrAuditScannerGuardTests` (the 66 cells and 10 created keys). The lot 4a base note is the
arithmetic that closed the fa-IR perimeter **before** any guard was written — 37 neutral files with an ar-SA
sibling and no fa-IR sibling, "37, and not one more", measured on blobs rather than on disk.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-lot4a-fa-ir-base-arithmetique-2026-11-03.md](458-lot4a-fa-ir-base-arithmetique-2026-11-03.md) | 2026-11-03 | `historical` | The **arithmetic of the fa-IR base**, closed before any guard: the perimeter is **37** neutral `DesktopModules/**/*.resx` files with an ar-SA sibling and **no fa-IR** sibling — derived in §2, measured on **blobs** in §3, confirmed against the pool in §4; §5 names the two figures that do not reproduce and what was set aside; §7 states what the note authorizes — the whole lot, since the guard requires it whole (#2131). |
| [458-ptbr-pt-continental-measure-2026-11-02.md](458-ptbr-pt-continental-measure-2026-11-02.md) | 2026-11-02 | `historical` | The pt-PT → pt-BR grain: **the nominative list first** — 13 cells in 2 forms over 140 `*.pt-BR.resx` files, with two instrument traps paid and named (an unaccented class inventing 136 defects; a one-eyed plural marker returning 0 by blindness); §6 the guard `DnnPtBrContinentalResxGuardTests` with its red measured, not asserted (#2126). |
| [458-ptbr-security-13-cles-mortes-2026-11-02.md](458-ptbr-security-13-cles-mortes-2026-11-02.md) | 2026-11-02 | `historical` | `security.pt-BR.resx`: **the 13 dead keys**, named and pinned by `DnnSecurityDeadKeysPtBrGuardTests.cs` (#2118). |
| [458-ptbr-security-audit-scanner-2026-11-02.md](458-ptbr-security-audit-scanner-2026-11-02.md) | 2026-11-02 | `historical` | `security.pt-BR.resx`, Audit/Scanner volet: **66 cells rewritten + 10 keys created**, pinned by `DnnSecurityPtBrAuditScannerGuardTests.cs` (#2125). |

### §10.5 — Site surfaces, measured on preprod (11 files)

**Where it stands on 2026-11-25.** These are preprod measures of surfaces — footer, sitemap lever, login
link, `/terms` `/privacy`, the newsletter chain, the Tier 3 forms, site→cards, and the two RuleMemo
injections. The RuleMemo gestures were **preprod content injections only** (4 then 22 cells), which §0 now
records as part of the state the swap carried. The newsletter group is a chain of three measures of the same
day: the third one corrects both of its predecessors on a **named point each** (which app renders the
newsletter), so the two earlier ones are `superseded` and the third survives — it is the attribution
`tools/458-eav-content-export.py:114` relies on for the app 48 export.

| File | Date | Status | What it is |
|---|---|---|---|
| [458-site-vs-cards-exhaustivite-2026-10-12.md](458-site-vs-cards-exhaustivite-2026-10-12.md) | 2026-10-12 | `historical` | Site → cards exhaustiveness: the **8 site-only sections classified, 0 missing rule**; classification, 0 write; instrument `docs/corpus/rules-site-vs-cards.py`, replayed from its committed location (#1868). |
| [458-tier3-mobius5-brevo-2026-10-25.md](458-tier3-mobius5-brevo-2026-10-25.md) | 2026-10-25 | `historical` | Tier 3 measures, read-only: the **Mobius5 contact form** and the **Brevo newsletter** — no DB write, no code, no deployment; preprod `dnn.argumentum.myia.io`, master `10aeb190` served (#1983). |
| [458-landingpage-footer-2026-10-25.md](458-landingpage-footer-2026-10-25.md) | 2026-10-25 | `historical` | The `Landingpage.ascx` footer: **no page selects it**; the 8/8 served `href` measured under each label; the label/target inversion is **real in the file and not served**. The correctif is in the repository and takes effect the day a page selects the template, or on a skin-pack reinstall (#1984). |
| [458-loginlink-attribution-2026-10-25.md](458-loginlink-attribution-2026-10-25.md) | 2026-10-25 | `historical` | The core login link (`dnn_DnnLogin_loginLink`, 5/8): the resx named, the missing cultures named, and the finding that it is **already covered** by the chrome Tier 1 file of po-2024 (#1993). |
| [458-sitemap-cl-lever-2026-10-25.md](458-sitemap-cl-lever-2026-10-25.md) | 2026-10-25 | `historical` | Empty preprod sitemap: the lever is the portal row **`ContentLocalizationEnabled`**, measured both ways and restored; §3 the culture servitude does **not** depend on the portal flag; §4 the measured interaction — a populated sitemap **XOR** the skin's `hreflang`; §5 the instrument traps met (#1991). |
| [458-terms-privacy-attribution-2026-10-25.md](458-terms-privacy-attribution-2026-10-25.md) | 2026-10-25 | `historical` | `/terms` `/privacy`: the text lives in `GlobalResources.<culture>.resx`, keys `MESSAGE_PORTAL_*.Text`; the state per culture, and §3 the grain handed to po-2024 — an **attribution, not an execution** (#1992). |
| [458-newsletter-form-unserved-translations-2026-10-26.md](458-newsletter-form-unserved-translations-2026-10-26.md) | 2026-10-26 | `superseded` by [458-newsletter-store-chain-2026-10-26.md](458-newsletter-store-chain-2026-10-26.md) | The morning measure: the form is translated but not served — its §3 attributed the rendering to **app 33 / entity 10176**, which has no consumer. What the successor keeps as true: §1 (the served text identical on the 8 cultures) and §2 (the app 48 `HtmlFragment` lead — **the right one**) (#2014). |
| [458-newsletter-rendered-store-identified-2026-10-26.md](458-newsletter-rendered-store-identified-2026-10-26.md) | 2026-10-26 | `superseded` by [458-newsletter-store-chain-2026-10-26.md](458-newsletter-store-chain-2026-10-26.md) | The afternoon measure: it identified the rendered store as **app 50 MobiusForms5** — that is the **contact form's** store, not the newsletter's. Corrected by the successor, whose §4 names the cause (an instrument reading the wrong table). What it keeps as true: the measure on 10176 (no consumer) and the elimination of the shared flag (#2014). |
| [458-newsletter-store-chain-2026-10-26.md](458-newsletter-store-chain-2026-10-26.md) | 2026-10-26 | `historical` | The chain, measured end to end: the served newsletter is the **Sendinblue HTML fragment of app 48** (E10181, `Html`, 13 314 characters, **no dimension row**), reached by `item 10172 → ContentGroupReference 10179 → app 48`; `_SnippetInject.cshtml` does `@Html.Raw(Content.Html)`, and `Content` resolves dimensions — so dims 7–13 on E10181 **would** be served: that is the path of a correctif, none posed. This is the surviving attribution, used at `tools/458-eav-content-export.py:114` (#2014). |
| [458-rulememo-injection-2026-10-25.md](458-rulememo-injection-2026-10-25.md) | 2026-10-25 | `historical` | Injection of the rule card-name prefix (`RuleMemoCardFileNamePrefix`): **4 cells**, preprod only, pure addition; base `dc42437c`, gate = the translation of #1979 (#1988). |
| [458-rulememo-harmonisation-injection-2026-10-26.md](458-rulememo-harmonisation-injection-2026-10-26.md) | 2026-10-26 | `historical` | The **harmonised** RuleMemo set injected: **22 UPDATE cells**, preprod only, gap 51 → 29; gate = the merge of #2011 (`7e53347b`); direct successor of the 4-cell note above (#2015). |

---

## §11 — How to read this folder

These are **dated measures**, not living references. A number quoted here is the number of its date: the
history of this folder is full of counts that were true when written and are not today (key totals, file
perimeters, families of residuals). Before quoting one in an issue or a devis, re-measure it against the
tree — and prefer the latest report of the same subject, not the most detailed one. `current` means the
file was re-read against master on the date of this index; `historical` means it is evidence of a closed
work; `superseded` means a named successor replaces it. For a **data file or a builder**, `current` says
it is still consumed (by a guard, a tool, or a pack whose content it defines) — not that its numbers are
today's.

## §12 — The contract, from this tranche on

This README carries the `<!-- index-of-record -->` mark since tranche 4. `python tools/test_docs-index-contract.py`
holds it to three things: every tracked file of `docs/dnn-localization` is cited, directly or through a
folder link; every row that links a file of this folder carries **exactly one** status in backticks; no link
is dead, case included. A file added under this folder without its row turns the pull request red, naming
the file. The counts this file accounts for move when a file is added — `git ls-files docs/dnn-localization`
is the instrument, and the figures above are dated: **217 tracked files** = 134 `458-*` + 49 root files
(48 documents and this README) + 34 files behind the two folder links of §6.

A subfolder row links the **folder** (`name/`), never its `README.md`: a link to a subfolder's README covers
only that README, and the other files of the subfolder would read as uncited. That is why the rows of §6
use `name/`.

The mark was posed with the proofs the DoD asks for, and the rule born of #2380 was applied in the same
pull request: `docs/dnn-localization` joined `REQUIRED_INDEXES` in `tools/test_docs-index-contract.py`, so
losing the mark now reddens `test_02` and names the folder instead of silently leaving the contract. Two
mutants were played on a copy of this branch: a row of a file cited only once removed ⇒ `test_01` red,
naming the file; the mark line removed ⇒ `test_02` red, naming `['docs/dnn-localization']`, with the
inverse control — the same mutant tree against master's list — clean, which is what proves the new entry
is the cause. Both were restored by `cp` and verified at `sha256`.

The former navigation index is kept **byte for byte** at the end of this file, under *Archive*. It is part
of this file, not indexed row by row: it sits in a fenced block on purpose — the contract holds every table
row that links to a file of this folder to a status, and the rows of the former index carry none. Fenced,
the record stays exact and the contract stays readable.

*po-2023 — pool #458, P23-34, tranches 1–4 · 25/11/2026.*

---

## Archive — former navigation index (replaced 2026-11-24, kept byte for byte as a dated record)

Last changed on master on 2026-07-13 (`b75df8326`, #798); blob `f569d598`. Nothing below this line is
edited: read it as the statement of that date.

````markdown
# DNN Localization & Upgrade Arc — Navigation Index

> **Entry point for the `#131` / `#132` / `#457` DNN documentation arc.**
> This is a **navigation index only** — it sequences the 15 documents in this folder in **go-live execution order** and flags what is superseded. Every line below points to a detailed doc; none rewrites its content. If a doc answers your question, open it.

**Current target:** **DNN 10.3.2 + 2sxc 21.07 LTS** (jsboige decision #2, issue #458, verified interactive channel 2026-06-18). The earlier **10.1.2** and **9.13.x** targets are **superseded** (see §3 below).

---

## §0.5 — Execution status (refresh 2026-07-12: the upgrade is DONE, B1-doc sweep extended)

> **The plan below was EXECUTED, not just written.** This section records what's actually live, so readers
> don't treat the spine as a TODO. The detailed runbooks (§1) remain the source of truth for *how*; this
> records *that it ran*.

| Milestone | Status | Evidence |
|-----------|--------|----------|
| **Phase 1.5 — 2sxc upgrade** | ✅ **DONE** | DB already at **2sxc 21.07.00** at upgrade time (memory "15.02" was stale). |
| **Phase 2 — DNN exec** | ✅ **DONE** | Sandbox wizard automated (`wiz_runupgrade.ps1`, 9.11.1→9.13.10→10.2.0→**10.3.2** on IIS Express :8090, 2026-06-28). `dbo.[Version]` top row = **10.3.2**. |
| **Phase 4 — sandbox smoke** | ✅ **DONE (technical GREEN)** | Homepage 200/86 KB, `/Argumentum` + `/Règles` render 2sxc content, 0 JsonOptions/conn-string errors, no IIS crash (cliff clear). **Visual PASS = jsboige (pending)**. |
| **Full-IIS migration** | ✅ **DONE (2026-07-01)** | `dnn.argumentum.myia.io` LIVE full-IIS direct (HTTP 200/85 KB, 0× "Something went wrong"). Stopgap `dnn.myia.io` retired + PortalAlias 1010 dropped. HTTPS SAN 9D80D4CC (exp 2026-09-27). **ACME bypass active** (`.well-known/acme-challenge/web.config`, functional for win-acme renew 2026-08-23). |
| **B1 inversion fix** | ✅ **DONE** | `repair-bin-net48.ps1` had reverted BCL DLLs to 6.0.0.0 (treating .NET 8/9 as "contamination" — **wrong for 2sxc-21**); fixed by deploying the matched BCL stack (Json 9.0.0.0/SCI 9.0.0.0/Bcl 8.0.0.0) from the 2sxc 21.07 Install pkg. See `[[reference-dnn-2sxc-net48-bcl-stack]]`. |
| **Canonical working `bin/`** | ✅ | `tmp/dnn-backups/bin_post_2sxc_realign` (330 files). Runtime branch: `dnn/sandbox-runtime-1032`. |
| **Prod go-live** | ⏸ **PENDING (ops, jsboige VPS)** | Migration is complete; remaining = **visual site verdict (jsboige/ai-01)** + prod go-live (ops VPS task). NOT blocking v0.9.0 assets (reco: de-couple). |

> ✅ **B1-inversion correction — sweep complete across 3 docs (refresh 2026-07-12):** the inverted B1
> thesis (revert BCL DLLs to 6.0.0.0 treating .NET 8/9 as "contamination" — **wrong for 2sxc-21**) was
> encoded in 3 docs; all now carry the correct fix inline (deploy the matched BCL stack — Json 9.0.0.0 /
> SCI 9.0.0.0 / Bcl 8.0.0.0 — from the 2sxc 21.07 Install pkg):
>
> - [`docs/dnn/sandbox-bootstrap-runbook.md`](../dnn/sandbox-bootstrap-runbook.md) §2/§3 — ⛔ SUPERSEDED callout (#765).
> - [`docs/dnn/go-live-turnkey-checklist.md`](../dnn/go-live-turnkey-checklist.md) B1 — ⛔ CRITICAL CORRECTION (#765, extended #794).
> - [`132-deployment-runbook.md`](132-deployment-runbook.md) §5.5(a) — B1-inversion lesson added as a MANDATORY sub-step (#792).
>
> The executable [`repair-bin-net48.ps1`](../dnn/repair-bin-net48.ps1) was **deprecated in #624** (2026-07-01,
> `-Apply` refuses). The `132-deployment-runbook.md` title was retargeted to 10.3.2 (2026-06-24).
> (`131-step1-sandbox-upgrade-runbook.md` carries no BCL versions — DNN-version-scoped, own TARGET-SUPERSEDED header.)
> See `[[reference-dnn-2sxc-net48-bcl-stack]]` for the authoritative version matrix.

---

## §1 — Read this first (the spine)

Follow these in order. Each is the authoritative doc for one execution phase.

| # | Phase | Doc | What it decides / does |
|---|-------|-----|------------------------|
| 0 | **Runtime fact** | [131-step0-runtime-verification.md](131-step0-runtime-verification.md) | Verified: DNN 10.x runtime = **.NET Framework 4.8** (not .NET 8). Foundation of the whole plan. |
| 1 | **Target decision** | [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) | **DECISION-SPEC.** Picks 10.3.2 over 10.1.2. Names the 2sxc cliff + the 2sxc-first ordering. Start here after Step 0. |
| 2 | **CVE reconciliation** | [131-cve-correction-and-target-refinement.md](131-cve-correction-and-target-refinement.md) | Corrects CVE-patched-version errors; refines target using the 2sxc cliff. Feeds #1. |
| 3 | **Phase 1.5 — 2sxc upgrade** | [131-2sxc-migration-plan.md](131-2sxc-migration-plan.md) | Step-by-step **2sxc 15.02 → 21.07** BEFORE crossing 10.2.0. De-risks the cliff. |
| 4 | **Phase 2 — DNN exec + rollback** | [131-dnn-phase2-exec-rollback.md](131-dnn-phase2-exec-rollback.md) | DNN 9.11.1 → 10.3.2 execution sequence + 2-anchor rollback runbook. **(§4.2 RESTORE order fixed in PR #552.)** |
| 5 | **Phase 4 — sandbox smoke gate** | [131-step2-smoke-test-checklist.md](131-step2-smoke-test-checklist.md) | Consolidated, tickable smoke-test to run **after** Step-1 upgrade, **before** prod go-live. |
| 6 | **Phase 5 — prod go-live** | [132-deployment-runbook.md](132-deployment-runbook.md) | Production deployment runbook. ⚠ **Title still says "10.1.2"** — apply with target 10.3.2 (see §3). |

---

## §2 — Supporting / scope docs

| Doc | Role |
|-----|------|
| [131-upgrade-sandbox-plan-v2.md](131-upgrade-sandbox-plan-v2.md) | The merged "v2" sandbox plan + go/no-go matrix. Reference for the palier logic. |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) | Sandbox upgrade runbook (was 10.1.2-scoped; exec gated on target decision). |
| [131-regen-staging-runbook.md](131-regen-staging-runbook.md) | Stages prerequisites for an attended mindmap/PDF regen — **no launch**. |
| [682-field-model-decision-input.md](682-field-model-decision-input.md) | **#682** Rule content-type field-model decision → **Path A** (lang-suffixed + `loc()` cascade). Ratified. Feeds #674 runtime + #684 translation. |
| [682-field-model-revision-2sxc21.md](682-field-model-revision-2sxc21.md) | **#682** revision against 2sxc v21 export (#681) — confirms Path A, 3 deltas corrected. |
| [682-path-a-provisioning-manifest.md](682-path-a-provisioning-manifest.md) | **#682** provisioning manifest: **49 new suffixed fields** (7 translatable × 7 non-FR langs) for jsboige to provision via 2sxc content-type editor. GATED ops. |
| [457-site-content-type-inventory.md](457-site-content-type-inventory.md) | Localization scope map (extends the Phase 1 audit, feeds the extractor). |
| [457-document-tier-translation-workflow.md](457-document-tier-translation-workflow.md) | **#457 Phase 2–4** for the **document tier** (2 static FR HTML pages): extraction → DatasetUpdater → re-import workflow design. Complements the string-tier toolchain; extractor = critical path. |
| [PHASE1-content-audit.md](PHASE1-content-audit.md) | Phase 1 content extraction audit (#457). Repo-side, complete. |
| [506-social-auth-connectors-inventory.md](506-social-auth-connectors-inventory.md) | **#506** social-auth (Facebook/Google/Microsoft/Twitter) inventory + secret-rotation procedure. Repo-grounded; live console checks = jsboige. Recommend folding into #131. |

---

## §3 — Superseded (read for history, do NOT execute)

| Doc | Superseded by | Why |
|-----|---------------|-----|
| [131-upgrade-9.13-sandbox-plan.md](131-upgrade-9.13-sandbox-plan.md) | #1 (target revision) | 9.13.x closes **0** CVEs (NVD/GHSA correction); target is now 10.3.2. |
| [131-step1-sandbox-upgrade-runbook.md](131-step1-sandbox-upgrade-runbook.md) §target | #1 + #4 | 10.1.2 target abandoned → 10.3.2. Runbook mechanics still valid; re-point the version. |
| [132-deployment-runbook.md](132-deployment-runbook.md) title | #1 | Header reads "10.1.2 go-live"; actual target is **10.3.2**. Content applies, version label is stale. |

---

## §4 — jsboige gates still pending (block prod go-live)

These are **business / content decisions only jsboige can make**. (The **upgrade itself is done** — see §0.5; these are the residual gates.)

1. **#445 — Stripe vs OpenStore** (keep OpenStore vs Stripe managed). Business decision. See [131-target-revision-10.3.2-full-upgrade.md](131-target-revision-10.3.2-full-upgrade.md) § "unblocked items".
2. **#525 — "Materiel" typo fix** (DB / 2sxc content). **= `res.RuleMaterial` Δ1** flagged in [490-res-rule-reconciliation.md](490-res-rule-reconciliation.md) (DB `Materiel` typo → `Matériel`). jsboige arbitration. Part of the 4 DNN res.Rule* deltas (Material typo · MemoCard case · FileNamePrefix brand · RuleContent view-bug).
3. **#682 — Rule field-model provisioning** (2sxc content-type editor). Provision the **49 lang-suffixed fields** listed in [682-path-a-provisioning-manifest.md](682-path-a-provisioning-manifest.md) (Path A, ratified). DB-side ops, gated jsboige. Unblocks #674 runtime validation + #684 translation population.
4. **#683 — DNN content-language enablement + URL routing + switcher** (jsboige). The i18n portage prerequisite that makes the 49 provisioned fields render per-culture. Gates #684 re-import.
5. **#684 — Rule prose translation + re-import** (7 langs × ~30 entities). Prep DONE (PR #767, 0 prod write); the **re-import to 2sxc is GATED** on #682 provisioning + #683 enablement + jsboige GO. See [457-document-tier-translation-workflow.md](457-document-tier-translation-workflow.md).
6. **Visual site verdict** (jsboige/ai-01) + **prod go-live** (ops VPS). The last non-calendaire gate.

---

## §5 — Release validation (separate dossier)

The `release-validation/` subfolder has its **own** [README](release-validation/README.md) covering: 2sxc export spec, 8-language checklist, non-Latin (ar/fa/zh/+ru) verification guide.

---

## §6 — Hard constraints (apply to every doc here)

- **The upgrade is EXECUTED** (§0.5) but **prod content mutation still requires jsboige GO** — no DB write, no 2sxc content change, no live-site edit without explicit authorization. The runbooks here were the *plan*; re-running them now is only for a re-deploy / rollback.
- **NEVER touch** `dnn-ui-strings.csv` (merged via #490; gpt-5.5 only).
- **Worker signals, does not declare PASS** — visual site verdict = jsboige/ai-01. DNN prod go-live = jsboige ops task.
````
