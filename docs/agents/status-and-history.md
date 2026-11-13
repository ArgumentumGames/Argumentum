# Status and history — dated snapshots

> **Read on demand — not loaded by default.** Read this for what was true when each section was last re-measured. **It is documentation, not state**: before acting on any item, read the issue itself (`AGENTS.md` §5); before quoting a count, re-measure it.
> Entry point and invariants: [`AGENTS.md`](../../AGENTS.md) · silent failures and the map of section titles: [`CLAUDE.md`](../../CLAUDE.md) · about this folder: [`README.md`](README.md).
> The sections below were moved here **verbatim** from `CLAUDE.md` on 2026-11-13 — headings and wording unchanged, so a citation such as *CLAUDE.md § Prochaines étapes* names a heading that exists here.
> In the text below, *ce fichier* means `CLAUDE.md` as it was; the mind-map section it points to is now in [`pipeline.md`](pipeline.md) § *Pipeline Stages*.

## Historical Context & Known Issues

### Golden Master: April 2024 (Commit `0087f0ec`)

The pipeline worked correctly before May 2025. A series of "vibecodés" commits introduced regressions.

### Applied Corrections (Oct-Dec 2025)

1. **HarvestManager.cs - Timeout**: Restored from 60s to 120s (CardPen needs 90-120s)
2. **HarvestManager.cs - frame.js**: Removed manual injection (CardPen loads it)
3. **HarvestManager.cs - generateImages()**: Added explicit call (no UI click in automation)
4. **PdfManager.cs - Lock**: Global lock on QuestPDF (NOT thread-safe)
5. **WebBasedGeneratorConfig.cs - CardSet Memo**: Reintegrated (critical for Print&Play Tarot)
6. **PdfManager.cs - Issue #119**: Rules cards positioning (lines 43-72) - preserves CardSet order so Rules appear first in TarotCards PDFs

### Known CSV Mapping Issues — ALL RESOLVED ✅

| File | Original Issue | Resolution |
|------|---------------|------------|
| `Rule.cs` | `GetId()` returned `string.Empty` | Now returns `Rules_01`, `Rules_02` etc. via RowIndex |
| `RuleClassMap.cs` | `print_and_play` not mapped | Mapped with `.Optional()` to `PrintAndPlay` property |
| `ArgumentVirtueClassMap.cs` | `Id` not mapped | Uses `Pk` property mapped to CSV `pk` column |
| `CsvBase.cs` | `MissingFieldFound = null` | Now logs warnings via `Logger.Log()` instead of silent |

## Diagnostic Resources

### Investigation Reports

Located in `docs/investigations/`:
- `2025-10-15-investigation-historique-pipeline-pdf.md` - Complete regression history
- `2025-10-21-rapport-archeologie-git-final.md` - Git archaeology (conclusion: C# code is intact)
- `2025-12-13-rapport-cloture-mission.md` - Final mission report with validation metrics

### PowerShell Diagnostic Scripts

Located in `docs/investigations/scripts/`:
- `2025-10-16-XX-test-*.ps1` - Pipeline validation tests
- `2025-10-21-XX-*.ps1` - Git archaeology scripts
- `2025-10-23-01-validation-memo-restaure.ps1` - Memo CardSet validation

### Reference Files (Git Archaeology)

Located in `docs/investigations/archeologie-git/`:
- `WebBasedGeneratorConfig_REFERENCE_FUNCTIONAL_*.cs` - Known working config (20KB)
- 15 historical versions for comparison

## Pipeline Recovery Status (Mars 2026)

### Validation Multilingue - 17 Mars 2026 (dimensions OK, contenu CASSÉ)

**P1 - Pipeline multilingue validé en DIMENSIONS uniquement** : 79 PDFs générés, 4209 images, dimensions correctes.

| Langue | PDFs | Images | Status |
|--------|------|--------|--------|
| FR (Français) | 18 | 620 | ✅ Golden Master |
| EN (English) | 22 | 1781 | ⚠ Structure OK, contenu FR (voir #216) |
| RU (Русский) | 17 | 1270 | ⚠ Structure OK, contenu FR (voir #216) |
| PT (Português) | 22 | 538 | ⚠ Structure OK, contenu FR (voir #216) |

**Issue #119 validée** : Rules cards apparaissent en premier dans tous les TarotCards multilingues.

**Bug #216 découvert 2026-04-22 + corrigé 2026-04-23** : `LocalizationConfig.FrontFieldConversions` pour Fallacies référençait des noms de champs (`Titre`, `Definition`, `Exemple`, `Contre-Exemple`) qui n'existaient PAS dans les templates Mustache (qui utilisent `{{text_fr}}`, `{{desc_fr}}`, `{{example_fr}}`, `{{Famille}}`, `{{Sous-Famille}}`, `{{Soussousfamille}}`). Résultat : `template.Replace()` ne trouvait rien à remplacer → tous les PDFs EN/RU/PT contenaient du contenu français. Corrigé en restaurant le mapping Golden Master (avril 2024) + ajout de Rules (absent) + tests de régression `FallaciesLocalizationTests`. **Regénération pipeline complète requise** pour produire les PDFs réellement multilingues.

**Mise à jour cycle 56+59 (mai 2026)** : Bug #216 corrigé avril 2026, pipeline régen post-merges #301/#302 auditée cycle 56 (Stratégie D recommandée : spot-check 5 cartes pixel diff ~15 min avant décision régen complète A ou aucune C). Tableau ci-dessus reste snapshot 17 mars 2026 (pré-fix).

### État actuel par CardSet — **mesuré 2026-08-28 sur l'arbre Release** (régén du 22/08), identique sur **les 8 langues**

⚠️ Les chiffres barrés ci-dessous étaient périmés dans ce fichier et auraient produit des devis d'impression faux (#1187, #1288). Un devis se chiffre **à la carte** : ne pas citer ce tableau sans re-mesurer.

| CardSet | Faces | Dos | Images | PDFs |
|---------|------:|----:|-------:|------|
| Fallacies Tarot | **175** ~~176~~ | 1 † | **176** ~~177~~ | Argumentum_TarotCards_{lang}.pdf |
| FallaciesWeb | **175** ~~176~~ | — | **175** ~~176~~ | A0, A4, Thumbnails |
| Rules Tarot | **15** ~~24~~ | — | 15 | dans TarotCards — réduit par #438 |
| Memo Tarot | 1 | 1 † | 2 | dans TarotCards |
| Scenarii Poker | **167** ~~97~~ | **7** | 174 | Argumentum_PokerCards_{lang}.pdf |
| Virtues Tarot | **131** ~~113~~ | 1 | 132 | Argumentum_TarotCards_Virtues_{lang}.pdf |

⚠️ La colonne « PDFs » nomme les **assets publiés** (mesurés sur les 80 PDF du bundle v2.0.0-regen du 21/09 : 10 documents × 8 langues, aucun suffixe). Les formes historiques `…_{lang}-1/2.pdf` / `…_{lang}-1.pdf` / `…-FacesOnly.pdf` décrivent d'anciennes sorties de pipeline et **ne correspondent à aucun asset** — c'est la famille de défauts qui a fait envoyer les relecteurs #802 sur un fichier inexistant (corrigé 21/09, c.5755612517).

† « 1 dos » s'entend **par langue** — les dos du Tarot (famille Fallacies, famille Memo) sont **localisés**, pas partagés entre langues. Mesuré au `sha256` des pixels décodés sur les 8 langues (balayage intégral du bundle v2.0.0, [#134 c. 5643251291](https://github.com/ArgumentumGames/Argumentum/issues/134#issuecomment-5643251291), 2026-09-12) : **0 dos commun aux 8 langues** — chaque dos existe en 8 exemplaires distincts (Fallacies ×175 pages/langue, Memo ×7, 1982×3401 CMYK, 8/8 pixels distincts). Le dos Fallacies porte le **titre du jeu traduit** (bande ~82 % de large à ~5 % du sommet : « L'ART DE NE JAMAIS AVOIR TORT » / « THE ART OF NEVER BEING WRONG » / « EL ARTE DE NUNCA ESTAR EQUIVOCADO » / « A ARTE DE NUNCA ESTAR ERRADO »…). Deux conséquences : un **devis d'impression** compte **un dos par langue** (8 exemplaires de chaque), jamais « un dos partagé sur toute la série » ; et un **contrôle inter-langues** qui attend un dos commun rendra « 0 dos commun » — c'est le comportement correct, pas une anomalie.

**Totaux fabrication** (#1187, décision retail du 14/09 c.`5665864605`, une boîte par langue) : deck **Tarot = 197 cartes** (175 Fallacies + 15 Rules + memo ×7 exemplaires imprimés) · deck **Scenarii = 167 cartes** · boîte **sans** Virtues = **364 cartes**, **avec** = **495**. *(Erratum 15/09 : les volumes 191/358/489 publiés le 05/09 — qui comptaient une seule memo — sont superseded pour la fabrication physique ; P&P inchangé, memo ×5/×1.)*

⚠️ **#1288 (décision owner du 05/09/2026, épinglée en CI par `PdfDeckCountContractTests`)** : PK 96 est un vrai doublon de PK 108 « Appel à la nature » et sort du deck ⇒ Fallacies 176→**175**, Tarot 192→**191**, boîte 359→**358** / 490→**489** (Scenarii 167 inchangé). Comptes re-mesurés le 15/09 sur `c089d526` depuis les CSV (Fallacies `carte` non vide = 175 PK uniques ; Rules 15 ; Scenarii 167 ; memo ×7 = décision #1187 ; Virtues 131 rapporté, arbre 22/08) ; le bundle mesuré du 11/09 les conforte (175 faces, 197 instances, 379 pages). Le compte de PNG de l'**arbre de régénération** (`bin/*/Target/`) reste **à re-mesurer** à la prochaine régén (lane po-2023). ⚠️ **En revanche, les mindmaps committés sont déjà à jour** — l'affirmation « ils embarquent encore les 176 thumbnails d'avant #1288 » est **fausse, mesurée le 19/09** : `Cards/Fallacies/Mindmaps/fr/Argumentum_Fallacies_MindMap_cards_fr.content.svg` porte **exactement 175 balises `<image>`**, toutes `width="60"`, et a été re-dérivé le 09/09 (`4bd53c81`, relabel PK 598) donc **après** la décision #1288 du 05/09. Ce sont **deux objets distincts** : l'arbre de build n'est pas le dépôt.
> 🔧 **Instrument** — les vignettes sont **encodées en base64 inline**, pas référencées par chemin : `grep -c '\.png'` rend **0** et se lit à tort comme « aucune vignette ». Compter `grep -o 'xlink:href="data:image/png;base64' <fichier> | wc -l`. Et ⛔ **ne pas compter un libellé** pour compter des cartes : le texte SVG est découpé en `tspan` au retour à la ligne, si bien qu'un libellé connu comme unique rend 1, 2 **ou** 3 occurrences (mesuré : « Faux dilemme » 1, « Appel à la nature » 2, « Homme de paille » 3). Le compte de libellés n'établit rien, **dans aucun sens**.

⚠️ **Le deck Scenarii a 7 dos distincts**, un par catégorie (`histoire`, `mythologie`, `politique`, `pop_culture`, `relation_intime`, `vie_personnelle`, `vie_professionnelle`) — contrairement au Tarot qui partage **un dos par langue** (voir † ci-dessus : « un » ne traverse pas les langues). C'est une contrainte de façonnage, pas un détail d'asset.

⚠️ **L'arbre Debug est périmé** (Rules 24 = pré-#438, Virtues 1) : Debug et Release sont indépendants — mesurer sur celui dont la date correspond, jamais « le premier trouvé ».

### Mind Maps & SVGs — April 2026 ✅ COMPLETE

- FreeMind Batik SVGs generated and committed across all 8 languages (FR/EN/RU/PT + ES/AR/FA/ZH, PR #565)
- FreeMind GUI automation via `SendKeys.SendWait` — VALIDATED (commit `46d6cd9b`)
- Issues #127, #128, #129 closed
- OWL ontology with SKOS — committed (PR #161), issue #130 closed

### Translation Pipeline (DatasetUpdater)

- **SDK**: Official OpenAI .NET SDK **v2.14.0** (csproj; migrated to the official SDK in PR #210, bumped via #1136 then #1627 — licence MIT re-read on the 2.14.0 nuspec 2026-10-02)
- **Models**: **`gpt-5.6-sol`** — le modèle **décidé** (GO owner 14/08) est désormais aussi celui que la **config compile** ; `gpt-4.1`/`gpt-4.1-mini` restent le fallback RU/PT (13 occurrences, inchangé). ⚠️ **L'erratum du 2026-09-15 est SUPERSEDED** — il chiffrait la config comme encore majoritairement `gpt-5.5`, sans aucune occurrence de 5.6, et donnait la migration pour gatée sur un GO à nommer (⛔ **chiffres volontairement non recopiés ici** : un remède qui **cite** le défaut le ré-indexe, et le prochain `grep` le relit comme courant — décrire, jamais citer) : la migration a été **mergée par #1426** (`0a79d217`), commentaires périmés historisés par #1431. **Re-mesuré le 2026-09-21 sur `22486d4a`** (`DatasetUpdaterRootConfig.cs`) : **48 littéraux `"gpt-5.6-sol"`, 0× `gpt-5.5`, 0× `gpt-5.4`** (87 occurrences toutes formes — les deux chiffres mesurent des objets différents, littéraux quotés vs commentaires inclus). ⛔ **Ne plus dispatcher « bumper la config » : c'est fait.** Ce qui reste gaté est **l'exécution** (GO owner + clé nommée), jamais le bump — voir [`docs/translation/202-smoke-datasetupdater-protocole-2026-09-15.md`](../translation/202-smoke-datasetupdater-protocole-2026-09-15.md). ⚠️ Deux `.cs` nomment encore `gpt-5.5` (`RulesSectionLexiconTests.cs:74`, `DnnUiString.cs:11`) : ce sont des **constats historiques datés** (« arbitré par… le 2026-08-28 »), pas des états courants — ⛔ ne pas les « corriger », ce serait falsifier un journal.
- **Multi-provider**: API support added via PR #302 (OpenAI + alternative providers configurable)
- **Config**: `DatasetUpdater/DatasetUpdaterRootConfig.cs` — 7 task configs (all `Enabled = false`)
- **Prompts**: 101 files in `DatasetUpdater/Resources/` (re-measured 2026-10-22 at `2662c795`, G-AIF; the historical "29" described an early state)
- **Function calling**: Manual `FunctionToolDef` + JSON schema + `BinaryData.FromString()`
- **Virtues CSV**: 100% translated (title/description/remark × fr/en/ru/pt), via PRs #218, #236, #246, #290, #295
- **Re-mesure i18n `es`/`ar`/`fa`/`zh`** — mesurée 2026-09-13 sur master `35acac04`, 5 corpus (Fallacies, Virtues, Scenarii, Rules, Rules PP), 12 774 cellules par langue : **couverture 100 %** sur tous les champs substantiels (les seuls vides sont **structurels**, identiques dans les 8 langues) et **0 contamination FR**. La seule lacune apparente est `link` — ⚠️ **mais ce chiffre mesure la colonne brute, pas ce qui est livré** (re-mesuré 19/09, `ea7a0a99`, **issue #1435**). `Entities/Fallacy.cs` déclare une **cascade de repli** (`Link<Lang>Fallback` → `link_<lang>` ?? `link_en` ?? `link_fr`), et ce sont ces propriétés-là que consomme le générateur de mindmaps (`MmGeneratorTests` asserte `LinkExpression` ⊇ `LinkFrFallback`). Sur **le deck imprimé** (175 cartes) : brut `fr` 49,7 % · `en` 90,3 % · `ru` 15,4 % · les 5 autres 4–5 % ⇒ **après repli, 161/175 = 92,0 % uniformes sur les 8 langues**, puis **165/175 = 94,3 %** depuis #1437. ⚠️ **Le tri des 14 cartes sans lien est SOLDÉ et #1435 est CLOSE** (19/09, `COMPLETED`) : #1437 a rempli **4 PK** (**2, 666, 758, 833**) et les **10 restants sont des vides JUSTIFIÉS, consignés nommément** (PK **1, 33, 658, 681, 690, 798, 826, 876, 1004, 1281**) — têtes de famille abstraites sans article dédié. ⛔ Ne pas re-dispatcher « trancher carte par carte » sur la foi de cette ligne : c'est fait. ⚠️ Les deux bases diffèrent : les pourcentages de 2026-09-13 portent sur les **1408 nœuds**, ceux-ci sur les **175 cartes** — vrais tous les deux, mais seul le second décide d'un livrable. Et `link` n'est rendu par **aucun gabarit de carte** (zéro `{{link}}` dans `Cards/**/*.json`) : le remplir n'impose **aucune régénération de PDF**. 6 groupes ont exigé une lecture ligne à ligne (tous des locutions latines, un emprunt accentué, une citation, deux formes espagnoles autonomes). Deux **questions éditoriales** étaient signalées : `Scenarii.<lang>.subcategory` « campagne » — ✅ **corrigé** par #1434 (`Campaign` rendu au sens *rural* en `es`/`ar`/`zh`/`fa`, 16 cellules) — et « romance » (`es`) vs `Romance` (`en`/`pt`), **laissée en l'état** (voisinage désambiguïsant, à ne changer que sur jugement natif). Instrument, tables et relevé nominatif : [`docs/translation/i18n-remesure-es-ar-fa-zh.md`](../translation/i18n-remesure-es-ar-fa-zh.md)
- **Issue #183** DONE — merged via PR #210

### GSheet ↔ CSV Sync (PR #200 merged)

- **Module**: `GSheetSync/` (9 files: CsvDiffEngine, DiffReport, SyncSafetyChecker, Auth, Service, Runner, configs)
- **Mode flag**: `ConverterMode.GSheetSync = 1 << 14` (16384)
- **Safety**: 6-layer upload protection (dry-run, diff, thresholds, confirmation, backup, verify)
- **4 spreadsheet configs**: Fallacies, Scenarii, Virtues, Rules (all `Enabled = false`)
- **Pending**: OAuth credentials for end-to-end testing
- **Tests**: 77 pass / 0 fail / 1 skip (includes CsvDiffEngine, SyncSafetyChecker, DiffReport, CsvToGrid tests)

### Test Coverage (refreshed 2026-10-02)

- **1615 tests pass** (`dotnet test` on `Argumentum.AssetConverter.Tests`, 2026-10-22, .NET 9 — **1621 total: 1615 pass / 0 fail / 6 skip**, mesuré localement par ai-01 sur la tête de #1905 `56a3a070`, Magick.NET 15), 6 skips (GUI/infrastructure). ⚠️ Les **5 portes mindmaps** ne font **pas** partie des skips : env-gatées jusqu'au 30/09, elles étaient comptées **réussies à vide** ; **PROMUES toujours-actives le 01/10** par #1688 (pool #458, oracles d'acceptation : `MindmapDeadLinkGateTests` ×1 et `MindmapWrapperFreshnessGateTests` ×4 tournaient à vide sans `ARGUMENTUM_DEADLINK_GATE` ; mesurées 5/5 vertes porte ouverte sur `48618fc5` post-#1681, early-returns retirés, `build.yml` passé en `fetch-depth: 0` pour les dates de commit). **0 known-fail** : le round-trip OWL (#133) n'est plus rouge depuis #793 — `skos:inScheme` survit au round-trip (l'ancienne formule « 1 fail `OwlE2E…DroppedByOwl2XmlRoundTrip` » datait d'avant) ; le résidu `rdf:type` reload-drop reste asserté-comme-attendu, sans impact sur les assets.
- Historique du compteur (l'instrument a changé en route) : 578/584 le 05/07 (local) → 596/601 le 14/07 (local) → 643 total/638 pass/0 fail/5 skip le 27/07 **mesuré en CI, legs Debug+Release** (#911 a rendu l'étape Test signifiante — elle exécutait le build sans lancer les tests ; run `30280070312`) → 1100/1095/0/5 le 12/09 (local) → **1135/1129/0/6 le 21/09** (local, `68e8d3d5` — les +6 : les organes de fraîcheur #1446/#1449 comptent désormais dans le total CI-inerte). → **1304/1298/0/6 le 29/09** (local, `ba15f238`) → 1358/1352/0/6 le 30/09 matin (arbre fusionné #1673/#1665/#1672, mesure ai-01 au tick 04:23Z) → 1359/1353/0/6 le 30/09 (local, `c1491de9`) → **1367/1361/0/6 le 01/10** (local `48618fc5` + CI #1688 ; la promotion des 5 portes ne change pas le compte : elles passaient déjà, à vide). → **1369/1363/0/6 le 01/10 soir** (local, tête de #1691 ; +2 = `InverseTitleCollisionGuardTests`). → **1393/1387/0/6 le 02/10** (local po-2024, master `da8a49a2` ; +24 vs la tête de #1691 = cycle K mergé #1700/#1702). → **1621/1615/0/6 le 22/10** (local ai-01, tête de #1905 `56a3a070` — bumps dotnet + Magick.NET 15 ; ⚠️ sur ai-01 la sortie de `dotnet test` est **en français** — `Réussi!`, `réussite`, `Ignoré` — un filtre `grep "Passed!"` y rend vide). Un compteur de tests dans une doc se **re-mesure**, jamais ne se recopie d'un rapport.
- Coverage includes: CsvDiffEngine, SyncSafetyChecker, DiffReport, CsvToGrid, MindMapHtmlWrapper, FallaciesLocalizationTests, TaxonomyValidationTests, Memo_Back localization, Playwright visual tests, `PdfDeckCountContractTests` (#1187/#1288), `CardSetExpectedCardCountContractTests` (#1212)
- Build is zero-warning (CS compiler warnings + NuGet audit, #587) — **épinglé par organe depuis #1676 (30/09)** : `<WarningsAsErrors>nullable</WarningsAsErrors>` dans les DEUX csproj de test rend un avertissement CS86xx bloquant. Périmètre : le csproj principal reste **hors contexte nullable** (aucun `<Nullable>enable</Nullable>` ; l'activer mesure 1228 CS8xxx — recompilation forcée `--no-incremental` obligatoire, le build incrémental rend 0), NU190x restent hors organe (advisories dérivant contre les versions épinglées).
- ~~Issue #212 tracks Playwright visual regression tests for generated PDFs~~ ✅ **CLOSED/COMPLETED le 2026-04-23** (barré 21/09, sweep état) — le cadrage du successeur vit dans ce fichier (§ mindmaps + pool #458 v6 grain ④) ; la ligne de « Prochaines étapes » porte le même barré.

### Prochaines étapes

> ⚠️ **Cette liste est une DOC, pas un ÉTAT.** Elle dit ce qui *était* vrai à sa dernière relecture ; l'issue dit ce qui *est* ouvert. ⛔ **Ne jamais dispatcher un item d'ici sans le confronter à `gh issue view N --json state,stateReason`** — deux lignes non barrées ci-dessous pointaient sur du clos (items 11 et 16, mesurés le 20/09), et l'une d'elles a produit un faux dispatch. Même famille que la ligne #1435 corrigée par #1448 : **une ligne de doc périmée ne dort pas, elle se fait exécuter.**

1. ~~Valider génération images après corrections CSV/CSS~~ FAIT
2. ~~Tester génération PDFs (QuestPDF)~~ FAIT
3. ~~Activer et tester génération multilingue~~ FAIT (17 Mars 2026)
4. ~~Valider formats: Tarot, Poker, A0, Print&Play~~ FAIT
5. ~~Mind Maps + SVGs (Batik)~~ FAIT (6 Avril 2026)
6. ~~#183 — Upgrade SDK traduction~~ FAIT (PR #210 merged, avril 2026)
7. ~~#193 — GSheet ↔ CSV sync~~ FAIT (PR #200 merged, avril 2026)
8. ~~#202 Phase 1 — CSV text micro-fixes~~ FAIT (PR #203 + #213 merged)
9. Valider DatasetUpdater round-trip avec OpenAI API (3-5 records, `Enabled=true`) — **protocole rédigé, jamais exécuté** ([`202-smoke-datasetupdater-protocole`](../translation/202-smoke-datasetupdater-protocole-2026-09-15.md)). ⚠️ « smoke test gpt-5.5 en cours (po-2023, cycle 61bis) » était **doublement périmé** : ni en cours, ni gpt-5.5. Le prérequis **P1 (bump de config) est SOLDÉ** par #1426 ; restent **P2 (clé nommée, hors dépôt)** et P3-P5 (sonde crédit, API Responses + effort low, copie fraîche du corpus) — à poser à l'exécution. Le gate est le **GO owner + la clé**, ⛔ plus le bump.
10. ~~#211 — Retraduction PT Rules~~ ✅ DONE (closed 2026-05-17), Rules PT 100% sauf row 1 cover (fix PR #306 cycle 47)
11. ~~#212 — Playwright visual regression tests pour PDFs générés~~ ✅ **CLOSED/COMPLETED le 2026-04-23** — le projet `Generation/Converters/Argumentum.AssetConverter.VisualTests` porte les tests (`FallacyCardTests`, `MemoBackPaletteGutterTests`, `MindmapWrapperTests`, `MindmapWrapperCapabilitiesTests`, `PdfBundleIntegrityTests`). ⚠️ **Laissée non barrée jusqu'au 20/09, cette ligne a produit un faux dispatch** (pool #458 v5 grain ⑥ : cadrage demandé sur un ticket clos depuis cinq mois). Le cadrage livré ([#212 c.5752110201](https://github.com/ArgumentumGames/Argumentum/issues/212#issuecomment-5752110201)) partitionne les 4 stages en *mesurable-sans-verdict* (1/3/4) vs *verdict humain* (2, snapshots — qui ne détectent qu'un **changement**, jamais une justesse) : il reste **valable comme note de conception post-tag**, ⛔ mais #212 ne se rouvre pas sur cette base.
12. ~~Virtues i18n — ajouter colonnes _en/_ru/_pt~~ ✅ DONE (April-May 2026, PRs #218/#236/#246/#290/#295) — 100% coverage title/description/remark
13. ~~Scenarii EN/RU/PT — 76/167 records missing (~46%)~~ ✅ DONE — 167/167 records 100% covered EN/RU/PT (verified cell-by-cell on master `7206f2f9`, 2026-05-24) across all 8 translatable fields; filled via commits `7ed970a3` (EN), `2a1b86bf` (RU), `0dc838fb` (PT) + contamination/BOM fixes
14. #134 — GitHub Release v2.0.0 (ex-v0.9.0, re-scopé jsboige 2026-08-06, #999 — en attente validation docs)
15. #133 — Publication OWL
16. #131 — DNN site + déploiement. ⚠️ **#132 est CLOSED/COMPLETED depuis le 2026-06-18** : ⛔ ne plus citer la paire « #131/#132 » comme un seul item ouvert — c'est un compte de travail restant faux de moitié.

### Commits clés de la recovery

| Commit | Description |
|--------|-------------|
| `37600e4a` | fix(harvest): restore Golden Master CSV injection |
| `f0b1cd35` | fix(templates): add argumentsVertueux CSS class |
| `09b427ef` | fix(templates): Scenarii asset paths to GitHub URLs |
| `30483257` | fix(templates): Virtues CSS and Rules naming |
| `9b19d5e8` | fix(config): remove RowsetNb=14 for Scenarii CardSet |
| `75a049d3` | fix(mindmap): restore validated FreeMind SendKeys automation |
| `55c6774e` | feat(assets): replace XSLT SVGs with FreeMind Batik SVGs |
| `fd2aef10` | feat(dataset-updater): migrate to official OpenAI SDK v2.10.0 (#183) |
| `e24cbd17` | fix(prompt): enable #nullable context and guard null param.Name |
| `092d4639` | Merge PR #200 — bidirectional GSheet ↔ CSV sync (#193) |

### Data Quality Issues (April-May 2026)

| Issue | Description | Status |
| ------- | ----------- | ------ |
| ~~Fallacies duplicate PKs 520, 1000~~ | Was reported during GSheet sync; **not reproducible on master** — 1408/1408 PKs unique, PK 520 & 1000 appear once each (verified `7206f2f9`, 2026-05-24). Stale warning or GSheet-view artefact | ✅ N/A |
| ~~Scenarii 54% translated~~ | 167/167 records now 100% covered EN/RU/PT, all 8 fields; substantive fields (context/issue) 0% FR-contaminated, RU 165/167 Cyrillic. Title=FR overlaps (21 EN/11 PT) = legitimate proper nouns (Sherlock, Ergo sum…). Verified `7206f2f9`, 2026-05-24. ⚠️ « Jeanne d'Arc » retiré de cette liste (mesure 8 langues sur la rangée `1,0201`, master `1ab6d861`, 2026-09-07) : 7/8 localisent (Joan of Arc, Жанна д'Арк, Juana de Arco, جان دارك, 圣女贞德, ژان دارک) — seul `title_pt` portait « Jeanne d'Arc » ; lacune corrigée en « Joana d'Arc », l'ancienne vérif ne balayait que « FR dans une cellule PT » sans mesurer les 6 autres langues | ✅ DONE |
| ~~Virtues 0% translated~~ | ✅ Resolved via PRs #218, #236, #246, #290, #295 (April-May 2026) — 100% coverage title/description/remark × 4 languages | DONE |
| PT Rules row 1 EN contamination | Rules cover showed "Liars 'School" instead of "A Escola dos Mentirosos" | ✅ Fix PR #306 cycle 47 (1 cell CSV, native PT validated po-2023) |
