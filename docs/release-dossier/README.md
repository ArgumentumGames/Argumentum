# docs/release-dossier — index de record

<!-- index-of-record -->

Ce dossier rassemble les pièces du dossier de release v2.0.0 (#134) et les feuilles de décision que les grains du pool #458 y ont déposées de juin à septembre 2026. Rien ne s'y supprime : chaque fichier est une pièce. Ce README **est** l'index de record du dossier depuis le 24/11/2026 : il cite les **31 fichiers** suivis (lui-même non compris), conformément à [`docs/agents/consolidation.md`](../agents/consolidation.md), et la marque d'index a été posée en dernier. À partir de maintenant la CI tient la complétude : `python -I tools/test_docs-index-contract.py` rougit un fichier ajouté au dossier sans ligne d'index, un lien mort, ou une ligne citante sans statut.

**L'index ne change aucun constat.** Aucune pièce n'est modifiée, déplacée ni corrigée ici.

**Ce que devenait ce fichier.** Jusqu'ici ce README était le dossier de validation de la v0.9.0 (dossier du 17/06, rafraîchi le 19/06). Il porte en tête son propre bandeau de remplacement et désigne [`RELEASE-VALIDATION-v0.9.0.md`](../RELEASE-VALIDATION-v0.9.0.md) comme référence. Il est conservé **tel quel** sous l'index, en bas de page : deux articles d'annonce v0.9.0 et quatre pièces de ce dossier pointent encore vers lui par un lien. Ses chiffres ne décrivent aucun bundle publié.

## Où en est #134 — ouvert, tag non posé

Indexé sur master `48dc101b`. [#134](https://github.com/ArgumentumGames/Argumentum/issues/134) est **ouvert** et le tag `v2.0.0` n'est pas posé. L'ordre en vigueur est celui du corps de [#458](https://github.com/ArgumentumGames/Argumentum/issues/458) : régénération groupée après « PDF validés » sur #2132, verdict visuel d'ai-01 sur les fichiers finaux, date du tag décidée par l'owner, puis le tag posé par ai-01 le jour de la bascule, avec les fichiers et les zips. Aucune pièce de ce dossier ne décrit cet état : la plus récente date du 18/09. Seules les trois pièces marquées current servent encore ; tout le reste se lit comme un constat daté. Les notes de version et la page de téléchargement v2.0.0 sont hors de ce dossier ([`RELEASE-NOTES-v2.0.0.md`](../RELEASE-NOTES-v2.0.0.md), [`downloads-v2.0.0.fr.md`](../publication/downloads-v2.0.0.fr.md)) : cet index ne les couvre pas.

## Les statuts

| Statut | Sens | Pièces |
|---|---|---|
| `current` | Gouverne encore un geste à venir. Relu contre master le 24/11 : un chiffre qu'il cite se re-mesure avant d'être répété | 3 |
| `superseded` | Remplacé : la ligne nomme ce qui le remplace | 0 |
| `historical` | Pièce d'un travail clos : vraie à sa date, à ne pas exécuter ni corriger | 28 |

## La release v2.0.0 et #134 — 10 fichiers

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`134-bundle-verification-recipes.md`](134-bundle-verification-recipes.md) | 2026-09-13 | `current` | Recettes pour refaire les mesures d'un bundle : dimensions des objets image, CMYK par le log de la passe, empreintes contre le manifeste, et quatre pièges d'instrument. À rejouer sur chaque bundle à publier ; ses limites sont plus bas |
| [`134-guide-validation-v2.0.0.md`](134-guide-validation-v2.0.0.md) | 2026-09-15 | `current` | Guide de validation, édition 3 : six contrôles C1 à C6, chacun avec sa commande, son attendu et son constat. Dernière édition en date, source des critères cités par les notes de version v2.0.0 ; ses limites sont plus bas |
| [`package-v2.0.0.ps1`](package-v2.0.0.ps1) | 2026-09-09 | `current` | Script qui vérifie qu'un bundle contient exactement 8 langues × 10 types de PDF, ni un de plus ni un de moins, et écrit le manifeste `manifest-v2.0.0.md`. Ses 10 types sont, au 24/11, les 10 documents PDF activés de la configuration ; ses valeurs par défaut datent de septembre |
| [`DECISION-v2.0.0-jsboige.md`](DECISION-v2.0.0-jsboige.md) | 2026-08-07 | `historical` | Brief de décision de la release, étampé le 13/09 : compte rendu des quatre réponses données par l'owner le 08/08 (assets GitHub, tag après la mise en prod du site, scope 8 langues). Le reste décrit le 07/08 et n'est plus vrai |
| [`134-dossier-validation-mesures.md`](134-dossier-validation-mesures.md) | 2026-09-16 | `historical` | Déroulé complet des contrôles C1 à C6 sur le bundle du 12/09 : 80 PDF sur 80, 3 745,6 Mo |
| [`134-fraicheur-post-65dd4742-2026-09-16.md`](134-fraicheur-post-65dd4742-2026-09-16.md) | 2026-09-16 | `historical` | Fraîcheur du bundle du 12/09 : 6 PDF Poker (en, ru) touchés par les commits postérieurs à `65dd4742`, régénération ciblée ou complète |
| [`134-fenetre-regen-reservation-2026-09-16.md`](134-fenetre-regen-reservation-2026-09-16.md) | 2026-09-16 | `historical` | Dossier de réservation de la fenêtre de régénération : périmètre selon la branche retenue pour #994, durée, critères de sortie. Aucune réservation exécutée ; il ne décrit plus la régénération à venir |
| [`134-entree-validation-release-2026-09-16.md`](134-entree-validation-release-2026-09-16.md) | 2026-09-16 | `historical` | Page d'entrée de la relecture du week-end du 16/09 : table des matières du dossier et décisions alors ouvertes. Sa liste de décisions n'est pas l'état des arbitrages d'aujourd'hui |
| [`134-release-assets-drift-2026-09-15.md`](134-release-assets-drift-2026-09-15.md) | 2026-09-15 | `historical` | Mesure du 15/09 : 64 fichiers servis par la pré-release pour 80 attendus |
| [`134-dependabot-pretag-tri-2026-09-15.md`](134-dependabot-pretag-tri-2026-09-15.md) | 2026-09-15 | `historical` | Tri des 5 PR Dependabot ouvertes au 15/09 avant le tag |

## #994 — apostrophes : 5 fichiers

#994 est fermée depuis le 19/10 (complétée). Les cinq pièces disent l'état des apostrophes entre le 11/08 et le 15/09 ; la branche A est notée cochée depuis le 17/09 dans l'index des arbitrages du 18/09.

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`994-pk992-text-vs-nom-vulgarise.md`](994-pk992-text-vs-nom-vulgarise.md) | 2026-08-11 | `historical` | Constat : `nom_vulgarisé` est une dérogation rare (40 nœuds sur 1 408), rendue dans aucun artefact de production |
| [`994-apostrophes-8langues-2026-09-14.md`](994-apostrophes-8langues-2026-09-14.md) | 2026-09-14 | `historical` | Recensement des apostrophes dans les 8 langues, coût de relecture et convention cible (révision 2, après la revue d'ai-01) |
| [`994-arbitrages-AB-reestampe-2026-09-14.md`](994-arbitrages-AB-reestampe-2026-09-14.md) | 2026-09-14 | `historical` | Branches A et B re-mesurées sur master (révision 2) |
| [`994-tournesol-et-delta-branche-A-2026-09-14.md`](994-tournesol-et-delta-branche-A-2026-09-14.md) | 2026-09-14 | `historical` | Compte rendu de l'écriture des 2 cellules « Tournesol » et de l'écart 47 → 71 de la branche A, déclaré avant écriture (révision 2) |
| [`994-arbitrage-apostrophes-ABCD-2026-09-15.md`](994-arbitrage-apostrophes-ABCD-2026-09-15.md) | 2026-09-15 | `historical` | Branches A à D jouées à blanc par `tools/994-apostrophe-dryrun.py` : feuille tranchable |

## Les grains du pool #458 — 5 fichiers

#458 est ouvert ; sa file vit dans le corps de l'issue, pas dans ces pièces. Ce sont les traces de quatre grains des 15 et 16/09 et la photographie des arbitrages du 18/09.

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`458-grain3-sweep-volumes-2026-09-15.md`](458-grain3-sweep-volumes-2026-09-15.md) | 2026-09-15 | `historical` | Trace du balayage des volumes erronés 191/358/489 : un porteur réel, corrigé |
| [`458-grain3-nombres-rules-15-18-24-2026-09-15.md`](458-grain3-nombres-rules-15-18-24-2026-09-15.md) | 2026-09-15 | `historical` | Balayage des « 18 » et « 24 » règles : zéro porteur, chaque occurrence restante est historique ou annotée |
| [`458-grain9-sweep-references-2026-09-15.md`](458-grain9-sweep-references-2026-09-15.md) | 2026-09-15 | `historical` | Balayage des références périmées (#1189, #1190, anciens noms de familles) : 4 erratum d'en-tête posés |
| [`458-grain8-contre-review-1400-1406-2026-09-16.md`](458-grain8-contre-review-1400-1406-2026-09-16.md) | 2026-09-16 | `historical` | Contre-revue par po-2023 de 7 PR de po-2024 (#1400 à #1406) : 7 conformes sur les affirmations re-jouées |
| [`INDEX-arbitrages-owner-2026-09-17.md`](INDEX-arbitrages-owner-2026-09-17.md) | 2026-09-18 | `historical` | Photographie au 18/09 des arbitrages owner ouverts (15 lignes) et carte de `docs/`. Elle se donnait pour point d'entrée de ce dossier et sa liste n'est plus celle d'aujourd'hui : voir les contradictions plus bas |

## #1369 — couvertures par variante de jeu : 2 fichiers

#1369 est ouverte ; l'état vit sur l'issue. L'index des arbitrages du 18/09 y notait le mécanisme et la fiche de style soldés depuis le 17/09 ; selon lui restaient les images muettes et la validation visuelle.

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`1369-cout-mecanisme-cardclass-2026-09-15.md`](1369-cout-mecanisme-cardclass-2026-09-15.md) | 2026-09-15 | `historical` | Chiffrage des 3 options de mécanisme en fichiers touchés et régressions possibles |
| [`1369-dossier-owner-cover-variantes-2026-09-15.md`](1369-dossier-owner-cover-variantes-2026-09-15.md) | 2026-09-15 | `historical` | Dossier tranchable : couverture illustrée par variante de jeu, re-mesure du CSV Rules vivant (15 lignes), du gabarit et des assets |

## #802 — libellés de rôles RU et ES : 2 fichiers

#802 est fermée depuis le 03/11, comme doublon.

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`802-libelles-roles-2026-09-15.md`](802-libelles-roles-2026-09-15.md) | 2026-09-15 | `historical` | Proposition de libellés de rôles : inventaire mesuré des CSV Scenarii et Rules, cible par rôle et par langue |
| [`802-feuille-decision-ru-es-2026-09-15.md`](802-feuille-decision-ru-es-2026-09-15.md) | 2026-09-15 | `historical` | Feuille de décision à 2 coches (libellés RU et ES), extraite de la proposition |

## Pièces isolées — 7 fichiers

| Fichier | Daté | Statut | Ce que c'est |
|---|---|---|---|
| [`1058-family-en-regen-manifest.md`](1058-family-en-regen-manifest.md) | 2026-08-11 | `historical` | Manifeste d'impact des 48 cellules `family_en` remplies (PR #1058, mergée le 11/08) : seuls les PNG et les PDF de cartes EN sont touchés, ni les cartes mentales ni l'OWL |
| [`1064-csp-whitelist-derivation.md`](1064-csp-whitelist-derivation.md) | 2026-08-11 | `historical` | Liste blanche CSP dérivée du HTML servi sur 7 pages (#1064, close le 09/09) |
| [`999-inventaire-v0.9.0-2026-09-13.md`](999-inventaire-v0.9.0-2026-09-13.md) | 2026-09-13 | `historical` | Inventaire daté des mentions `v0.9.0` et des bandeaux posés (#999, close le 13/09) |
| [`942-dependabot-dnn-gelees-inventaire-2026-09-15.md`](942-dependabot-dnn-gelees-inventaire-2026-09-15.md) | 2026-09-15 | `historical` | Inventaire des 3 PR Dependabot gelées sous `DNNPlatform/` au 15/09 (#942, close le 06/09) |
| [`192-feuille-decision-link-i18n-2026-09-15.md`](192-feuille-decision-link-i18n-2026-09-15.md) | 2026-09-15 | `historical` | Feuille à 2 coches sur les liens `link_*` (#192, close le 28/06) |
| [`scenarii-es-romance-feuille-2026-09-15.md`](scenarii-es-romance-feuille-2026-09-15.md) | 2026-09-15 | `historical` | Feuille à 1 coche : « romance » ou « Romance » dans `subcategory_es`. Question éditoriale laissée en l'état |
| [`131-feuille-decision-d1-d6-2026-09-16.md`](131-feuille-decision-d1-d6-2026-09-16.md) | 2026-09-16 | `historical` | Feuille de décision de la montée DNN, vague 1 (6 coches) au 16/09. Ses cases ne sont plus tenues : l'état vit sur #131 |

## Limites des trois pièces current

| Pièce | Statut | Ce qui est daté ou démenti |
|---|---|---|
| [`134-guide-validation-v2.0.0.md`](134-guide-validation-v2.0.0.md) | `current` | Son en-tête et son contrôle C3 affirment que la chaîne `SWOP` est absente du bundle et qu'un grep nul est une panne d'instrument. La page d'entrée du 16/09 dit l'inverse (§4) : les 80 PDF portent une occurrence `SWOP` échappée, et l'attendu de l'édition 2 était vrai. Écart consigné, non corrigé : la correction revient à l'auteur de l'édition. Son ancre (§0) est le bundle du 12/09 sur la base `65dd4742`, à re-vérifier à chaque déroulé, et ses chiffres de fraîcheur (C6) datent du 15/09 |
| [`134-bundle-verification-recipes.md`](134-bundle-verification-recipes.md) | `current` | Son §4.3 répète l'affirmation `SWOP` démentie ci-dessus. Ses artefacts de référence (§1) sont le bundle du 12/09 sur le disque `G:`, hors dépôt. Sa sonde `/SMask` portait sur l'arbre de génération d'avant la conversion CMYK, recyclé depuis (§2) : elle n'y est plus rejouable |
| [`package-v2.0.0.ps1`](package-v2.0.0.ps1) | `current` | Ses valeurs par défaut datent de septembre : le dossier du bundle du 06/09, déclaré périmé dans son propre en-tête, et le commit de provenance `2a2e7b32`. Les deux se passent explicitement pour tout bundle ultérieur. Vérifié le 24/11 : ses 10 types sont les 10 documents PDF activés de [`WebBasedGeneratorConfig.cs`](../../Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs) ; les 2 autres documents PDF y sont désactivés |

## Contradictions entre pièces — lues, non tranchées

| Pièce | Statut | Ce qu'une autre pièce en dit |
|---|---|---|
| [`131-feuille-decision-d1-d6-2026-09-16.md`](131-feuille-decision-d1-d6-2026-09-16.md) | `historical` | Elle laisse ses 6 cases vides au 16/09. L'index des arbitrages du 18/09 note, à sa ligne « Montée DNN Vague 1 », la décision D1 prise le 17/09 (Stripe) |
| [`INDEX-arbitrages-owner-2026-09-17.md`](INDEX-arbitrages-owner-2026-09-17.md) | `historical` | Ses 15 lignes « ouvertes » ne sont plus la liste ouverte : #994 est close depuis le 19/10, #802 depuis le 03/11 (doublon), #192 depuis le 28/06. Sa carte de `docs/` compte 31 fichiers dans ce dossier et 42 dans `dnn-localization/` : des comptes du 18/09 |
| [`INDEX-arbitrages-owner-2026-09-17.md`](INDEX-arbitrages-owner-2026-09-17.md) | `historical` | Elle se donne (ses lignes 12 et 46) pour point d'entrée de ce dossier, à la place du README d'alors. Ce rôle est tenu par cet index depuis le 24/11 |
| [`134-fenetre-regen-reservation-2026-09-16.md`](134-fenetre-regen-reservation-2026-09-16.md) | `historical` | Son périmètre dépend de la branche retenue pour #994. La régénération annoncée dans le corps de #458 est groupée, sur la base #1460 : ce dossier ne la décrit plus |

## Ce que cet index n'établit pas

⛔ Aucune re-mesure des chiffres cités : ils sont rapportés de l'en-tête de chaque pièce. ⛔ Un statut n'est pas un verdict : sa justesse est un jugement, que la contre-revue relit. ⛔ Rien ici ne corrige une pièce : une pièce fausse a sa propre PR. ⛔ Le dossier v0.9.0 conservé en bas de page n'est pas indexé ligne à ligne : il fait partie de ce fichier.

*po-2023 — pool #458, ligne « Index de référence restants » · 24/11/2026.*

---

## Archive — dossier de validation v0.9.0 du 17/06 (remplacé, conservé tel quel)

# v0.9.0 Release — Validation Dossier (for jsboige)

> ⚠️ **Superseded (2026-06-19 snapshot).** The counts and facts below predate **bundle v3** (80 PDFs DeviceCMYK + SWOP, Print&Play Standard/Light #648-650, Ghostscript post-process #632/#652, OWL EN+FR only, tests 578 pass). **Current reference: [`docs/RELEASE-VALIDATION-v0.9.0.md`](../RELEASE-VALIDATION-v0.9.0.md) (dossier v4, refreshed 2026-07-04).** This framework dossier stays as the archive of the 17–19 June validation; its body is intentionally not rewritten.

**Issue:** [#134 — publish new GitHub release with updated game materials](https://github.com/ArgumentumGames/Argumentum/issues/134)
**Author:** Claude Code @ myia-po-2023 (worker)
**Date:** 2026-06-17 (refreshed 2026-06-19)
**Base:** master `4dd3c6bd`
**Status:** **DOSSIER / FRAMEWORK (docs, non-gated).** This is the consolidated entry-point + gate
checklist + artifact inventory jsboige asked the worker to build. **The release itself stays
GATED** — no tag, no GitHub Release, no regen — until jsboige says go. This dossier *prepares*
jsboige's validation; it does not replace it.

> **2026-06-19 refresh:** (a) DNN target confirmed **10.3.2** (decision #2, #531 — was "10.1.2 vs
> 10.3.2 undecided" in §4); (b) the DNN upgrade-arc docs (#543 2sxc migration plan, #545 regen
> staging, #548 Phase 2 exec+rollback) are now indexed in §2; (c) artifact inventory re-verified
> against disk (64 PDFs still present, mindmaps es/ar/fa/zh still 0). No structural change — this
> is a staleness fix, not a rewrite.

> Issue #134's body is **stale** (it lists "4 languages FR/EN/RU/PT" and dependencies #127-#130/#116).
> The live scope is **8 languages** (FR/EN/RU/PT + AR/ES/FA/ZH, merged via #359/#360/#361) and all
> those dependencies are closed. This dossier reflects the **current** scope.

> **Path note:** placed in `docs/release-dossier/` (not `docs/release/`) because `.gitignore:6`
> rule `[Rr]elease/` excludes any directory named `release`/`Release` (build-output guard).

---

## 1. What this dossier is (and isn't)

The release documentation is **already substantial but scattered** across 4 locations. This file is
the **single index** so a reader (jsboige) finds everything in one place, **plus** the two net-new
pieces that were missing: the **release-tag gate checklist** (§4) and the **README download-section
snippet** (§5).

**This dossier does NOT duplicate** the detailed docs it indexes — it points to them.

## 2. The release-doc map (where everything lives)

| Doc | What it covers | Status |
|-----|----------------|--------|
| [CHANGELOG.md](../../CHANGELOG.md) | v0.9.0 changelog (8-lang, pipeline recovery, data quality, test coverage, migration notes) | ✅ scaffolded (root) |
| [docs/release-v0.9.0-validation-brief.md](../release-v0.9.0-validation-brief.md) | "Sur pièce" presentation of the changelog claims, with worker-verified facts (64 PDFs = 8 types × 8 langs, ~9 834 images) | ✅ po-2023, 2026-06-13 |
| [docs/publication/qa-scenario-8langues-release.md](../publication/qa-scenario-8langues-release.md) | Per-PDF-type QA scenario: the 8 doc types × risk + page count, setup instructions | ✅ the per-language verification guide |
| [docs/dnn-localization/release-validation/](../dnn-localization/release-validation/) | DNN-site validation (non-latin guide, checklist, 2sxc export spec) | ✅ DNN-scoped (separate from the cards release) |
| [docs/dnn-localization/131-target-revision-10.3.2-full-upgrade.md](../dnn-localization/131-target-revision-10.3.2-full-upgrade.md) (#531) | DNN target = **10.3.2 + 2sxc 21** (decision #2), templates already Razor14 | ✅ the DNN-coupling context |
| [docs/dnn-localization/131-2sxc-migration-plan.md](../dnn-localization/131-2sxc-migration-plan.md) (#543) | Phase 1.5: 2sxc 15.02→21.07 LTS sequence + 25-app verify | ✅ merged `4dd3c6bd` |
| [docs/dnn-localization/131-dnn-phase2-exec-rollback.md](../dnn-localization/131-dnn-phase2-exec-rollback.md) (#548) | Phase 2: DNN exec 9.11→10.3.2 + rollback runbook | ✅ merged `4dd3c6bd` |
| **This dossier** | Consolidated index + **gate checklist** + **README download snippet** | 🆕 net-new |

**Known correction tracked:** the validation-brief flags that CHANGELOG said "155 tests" but the
real count was 159 (now ~359/0/5 after the #204 contract wave). The CHANGELOG test figure should be
refreshed at tag time (jsboige gate).

## 3. Per-language artifact inventory (grounded, not asserted)

Verified against the built Release artifacts (`bin/Release/net9.0-windows/Target/<lang>/Documents/density-0/`).
**8 languages × 8 document types = 64 PDFs.** Parity confirmed (same 8 filenames in `fr` and `zh`).

### The 8 PDF types (per language, `xx` = lang suffix `fr|en|ru|pt|es|ar|fa|zh`)

| # | File | Role | Main risk |
|---|------|------|-----------|
| 1 | `Argumentum_TarotCards_xx.pdf` | Rules + Memo + Fallacies (largest) | content density |
| 2 | `Argumentum_TarotCards_Virtues_xx.pdf` | Virtues | body overflow (#190, most fragile) |
| 3 | `Argumentum_PokerCards_xx.pdf` | Scenarii | long paragraphs |
| 4 | `Argumentum_Fallacies_Web_A4_xx.pdf` | A4 grid 66×66 mm | dense grid |
| 5 | `Argumentum_Fallacies_Web_A0_xx.pdf` | A0 poster 12 cols | 1 page |
| 6 | `Argumentum_Fallacies_Web_Thumbnails_A4_xx.pdf` | 50×50 vignettes | ~4 pages |
| 7 | `Argumentum_TarotCards_Print&Play_A4_xx.pdf` | recto-verso home print | alignment |
| 8 | `Argumentum_PokerCards_Print&Play_A4_xx.pdf` | Scenarii home print | ~12 pages |

(Per-PDF risk detail + page counts live in [qa-scenario-8langues-release.md](../publication/qa-scenario-8langues-release.md).)

### Non-PDF artifacts

| Artifact | Scope | Status |
|----------|-------|--------|
| Card images (PNG) | ~9 834 images, 8 langs | ✅ regen 2026-06-12, exit 0, ~5.0 GB |
| MindMap SVGs (Fallacies) | fr/en/ru/pt committed (content/links/cards variants, 21 total) | ✅ 4 langs |
| MindMap SVGs (Fallacies) es/ar/fa/zh | RTL/CJK variants | ⛔ **BLOCKED** (Windows foreground-lock — needs attended RDP run, jsboige gate) |
| MindMap SVGs (Virtues) | content/links variants | ✅ partial |
| OWL ontology | `docs/ontology/argumentum.owl` (FR) | ✅ committed (#161, #130 closed) |

**⚠️ Release-blocking gap:** MindMap SVGs for es/ar/fa/zh are NOT generated (foreground-lock). If
v0.9.0 must ship mindmaps for all 8 languages, this is the blocker; if 4-lang mindmaps are
acceptable for v0.9.0 (es/ar/fa/zh deferred), it is not. **jsboige decision** (gate).

## 4. Release-tag gate checklist (net-new — what must pass before `git tag v0.9.0`)

This is the checklist jsboige (or ai-01 on visual QA) ticks off at tag time. The regen that fills
the dossier stays gated; this is the contract that says "ready to tag".

- [ ] **Artifacts built**: 64 PDFs (8 langs × 8 types) present in Release bin, exit 0.
- [ ] **Tests green**: `dotnet test` → 0 fail (baseline ~359/0/5; refresh the CHANGELOG figure).
- [ ] **CSV data 100%**: Fallacies/Virtues/Scenarii/Rules ×8 langs, no FR-contamination in
      EN/RU/PT/AR/ES/FA/ZH (per data-gate #303, verified 2026-05-23/24).
- [ ] **Visual QA** (ai-01 verdict, NOT worker): spot-check ≥1 PDF per language per the
      [QA scenario](../publication/qa-scenario-8langues-release.md) — especially Virtues body
      overflow (#190) and RTL/CJK rendering (ar/fa/zh).
- [ ] **MindMap scope decided**: 4-lang (committed) or 8-lang (needs es/ar/fa/zh attended run).
- [ ] **CHANGELOG finalized**: v0.9.0 date filled (`2026-06-XX`), test count corrected.
- [ ] **README download section** added (§5 snippet) + tag link.
- [ ] **Packaging plan** (issue #134): Full / Print&Play / Per-language / MindMaps / Ontology
      bundles — decide bundling tooling (GitHub Release assets vs git-lfs vs external).
- [ ] **DNN #131 status**: release is "coupled DNN" per jsboige decision. DNN target is now
      **confirmed 10.3.2 + 2sxc 21** (decision #2, [#531](../dnn-localization/131-target-revision-10.3.2-full-upgrade.md));
      the upgrade procedure is documented ([#543](../dnn-localization/131-2sxc-migration-plan.md) Phase 1.5 2sxc +
      [#548](../dnn-localization/131-dnn-phase2-exec-rollback.md) Phase 2 DNN). Confirm whether the GitHub release
      **gates on the DNN upgrade being live** or ships independently (jsboige call).

## 5. README download-section snippet (ready to paste — issue #134 asks for it)

`README.md` currently has **no download/release section** (issue #134 explicitly requests one).
Below is a gate-ready snippet jsboige can paste once v0.9.0 is tagged. Placeholder tag/URL.

```markdown
## Download

Pre-built game materials are available on the [releases page](https://github.com/ArgumentumGames/Argumentum/releases).

### Packages (v0.9.0)

| Package | Contents | Languages |
|---------|----------|-----------|
| **Full Package** | All materials (Tarot, Poker, Print&Play, FallaciesWeb A0/A4) | FR · EN · RU · PT · ES · AR · FA · ZH |
| **Print & Play** | Print&Play A4 PDFs only (home printing, recto-verso) | all 8 |
| **Per Language** | Complete materials for one language | pick one |
| **Mind Maps** | Fallacies + Virtues SVG mind maps | FR · EN · RU · PT (es/ar/fa/zh: follow-up) |
| **Ontology** | `argumentum.owl` + documentation | FR |

### Printing instructions (Print & Play)

- Print **recto-verso** (duplex, flip on long edge) on A4 heavy paper (160–250 g/m²).
- `TarotCards_Print&Play_A4` = Rules + Memo + Fallacies; `PokerCards_Print&Play_A4` = Scenarii.
- Cut along the card edges after printing.
```

*(This snippet is **not** pasted into README.md yet — that edit is jsboige's gate call at tag time.
Provided here so the release-ready README is one paste away.)*

## 6. What this dossier does NOT do

- ❌ Does **not** tag or publish a release (gated jsboige).
- ❌ Does **not** regenerate artifacts (the regen is gated; this inventories what's already built).
- ❌ Does **not** edit README.md, CHANGELOG.md, or any consumer file (snippet provided, not applied).
- ❌ Does **not** decide the MindMap 4-vs-8-lang scope, the packaging tooling, or the DNN coupling —
  those are jsboige gates surfaced in §3/§4.
- ❌ Does **not** declare a visual-QA verdict (ai-01 only).

## Sources

- Built artifacts: `Generation/Converters/Argumentum.AssetConverter/bin/Release/net9.0-windows/Target/<lang>/Documents/density-0/` (64 PDFs, 8 langs × 8 types, parity verified fr↔zh).
- [CHANGELOG.md](../../CHANGELOG.md) (root), [release-v0.9.0-validation-brief.md](../release-v0.9.0-validation-brief.md), [qa-scenario-8langues-release.md](../publication/qa-scenario-8langues-release.md).
- MindMaps: `Cards/Fallacies/Mindmaps/{fr,en,ru,pt}/` (committed); es/ar/fa/zh blocked (foreground-lock, dashboard decision #13).
- OWL: `docs/ontology/argumentum.owl` (#161 merged, #130 closed).
- Issue #134 (release epic, scope stale vs current 8-lang reality).
