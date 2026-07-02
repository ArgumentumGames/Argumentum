# Argumentum v0.9.0 — Dossier de validation release

**Date** : 2026-07-01 (refresh v3 : régén RELEASE fraîche print-final + verdict visuel Release PASS ai-01)
**Statut** : ASSETS VALIDÉS (verdict Release ai-01 = PASS géométrie/contenu + micro-RU résolu) — en attente de 2 décisions jsboige (CMYK + titre PT) puis tag go-live
**Branche** : `docs/release-v0.9.0-validation-v2`
**Master de référence** : `3e2fa0c0` (build zéro-warning CS+NU, **549/0/5 tests**, Magick.NET 14.14.0). Régén RELEASE 8-lang **2026-07-01** sur `3e2fa0c0` + #614 (`-c Release` PNG lossless + CMYK, `EnableParallelism=false` serial, CardPen local). **Deux bundles GDrive** : Debug `review-v0.9.0-2026-06-28/` (verdict PASS représentatif) **ET** Release print-final `review-v0.9.0-RELEASE-2026-07-01/` (verdict Release PASS, 64 PDFs + 33 SVGs + manifest).

---

## 1. Objectif

Établir l'état vérifiable de la release v0.9.0 (scope = **8 langues** : fr / en / ru / pt / es / ar / fa / zh), lister ce qui est **livré et vérifié** vs ce qui **nécessite validation jsboige** avant le tag go-live. Ce dossier est le GATE de publication (#134).

> **Note de méthode** : ce dossier est bâti sur les **assets committés** + **deux régéns fraîches exécutées le 2026-07-01** sur master `3e2fa0c0` + #614 (`ContinueOnHarvestSetFailure=true`, `EnableParallelism=false` serial, worktree isolé). Le run **Debug** (JPEG Q85, CardPen local) a produit 64/64 PDFs, 1229 images/langue × 8, `Generation finished.` — bundle `review-v0.9.0-2026-06-28/`. Le run **Release** (`-c Release`, PNG lossless + CMYK, CardPen local) a produit 64/64 PDFs, `Generation finished.`, 0 échec — bundle `review-v0.9.0-RELEASE-2026-07-01/`. Les compteurs sont **re-vérifiés au 2026-07-01**.
>
> **Verdict visuel ai-01** : (1) **Debug = PASS représentatif** (08:50, spot-check `Fallacies_Web_Thumbnails` p1 zh/ar/fa/ru/es) ; (2) **Release = PASS géométrie/contenu** (20:55, pdftoppm 120dpi + inspection colorspace/encoding) — #119 Rules-first, recto-verso, #216 pas de fuite FR, **micro-RU PK79 garble `чшск-то` RÉSOLU** sur harvest frais, rendu 300 PPI. **2 findings factuels** remontés à jsboige (§3.3 Finding CMYK + §3.6 titre PT), ni l'un ni l'autre bloqueur géométrie/print.

---

## 2. Scope v0.9.0 (décisions jsboige actées)

| Décision | Réf |
|----------|-----|
| 8 langues (fr/en/ru/pt + es/ar/fa/zh) | décision #4 / #7 |
| EN impersonnel, FR « Vous » | directive jsboige |
| Traductions via ping-pong PRs, review ai-01 | directive jsboige |
| Release #134 **couplée DNN** (#131 cible 10.3.2 + 2sxc 21) | décision #12 |
| #202 bulk EN → **déféré post-release** | décision #16 (hors scope v0.9.0) |
| #457 2 HTML pages → **déféré** (legacy) | décision #17 |
| AutoMapper → rester MIT pur (14.0.0 + `MaxDepth(1)`) | décision #18 |

---

## 3. État des livrables — inventaire vérifié

### 3.1 Données CSV — 8 langues (✅ livré)

| Dataset | Coverage | Preuve |
|---------|----------|-------|
| Fallacies (1408 nœuds) | 8 langues, ~100% | Issue #335 clôturée ; passes #366-#369, #372-#380, #382 |
| Virtues (223 nœuds) | title/desc/remark ×4 puis ES/AR/FA/ZH | PRs #218/#236/#246/#290/#295/#364 ; **prod-write relationnel (#499 Phase 1, PR #590 merged)** → 66→78 cols, 12 additives |
| Scenarii (167 records) | 167/167, 8 champs × 8 langues | commits `7ed970a3`/`2a1b86bf`/`0dc838fb` ; cascade #382 |
| Rules | 8 langues, garbles PT/EN éliminés | PRs #330/#340/#362 ; PT row 1 cover fixé (#306) |

**Hygiène CSV close ce cycle** : corruption encodage `%C3→A13` systémique dans templates JSON réparée (#579 Fallacies, #581 Memo, #584 4 templates Fallacies live). Audit complet : `docs/investigations/2026-06-23-prod-csv-hygiene-audit.md`.

> **Source des counts** (pointeur, review NanoClaw) : Fallacies 1408 nœuds + Virtues 223 nœuds + Scenarii 167 records proviennent de l'analyse taxonomy consolidée — issues #335 (Fallacies closure), #499 Phase 1 spec (`docs/taxonomy/499-virtues-prod-write-spec.md`, 223 rows × 66 cols ground-truth), et commits Scenarii `7ed970a3`/`2a1b86bf`/`0dc838fb` (167/167 vérifiés cell-by-cell `7206f2f9`). Comptes non re-dérivés ce jour (cf §1 méthode).

### 3.2 MindMap SVGs — 8 langues (✅ livré, PR #565)

| Langue | SVGs | Note |
|--------|------|------|
| fr | 6 | refresh contenu (#565) |
| en | 5 | préservés |
| ru | 5 | préservés |
| pt | 5 | refresh contenu (#565) |
| **es** | **3** | **nouveau** (#565) |
| **ar** | **3** | **nouveau, RTL** |
| **fa** | **3** | **nouveau, RTL** |
| **zh** | **3** | **nouveau, CJK — 5.45 MB (5 451 309 bytes, `Fallacies_zh.svg` régén 2026-06-25 byte-proven, glyphes denses)** |

- Moteur : **FreeMind 1.0.1 + Batik** (haute fidélité, décision #184 — fallback XSLT retiré).
- **Reproductibilité byte-proven** (régên 8-lang 2026-06-24, RDP jsboige, exit 0) : `Fallacies_zh.svg` committed = fresh = 5 451 309 B (`cmp` IDENTICAL byte-for-byte). La baseline #565 se reproduit fidèlement → le moteur FreeMind/Batik est **stable, pas flaky** (mitige le risque §7 « régén non reproduite » pour le volet MindMap).
- Validation technique ai-01 (source-level) : contenu Unicode authentique par langue (0 fallback FR), géométrie d'arbre quasi-identique, `font-family='Tahoma'` arabe-capable, racines correctes (`السفسطة` pour ar). **PASS technique.**
- ⚠️ **Gap structurel mineur (non-bloquant v0.9.0)** : les Virtues `.content.svg` sont **FR-figés** (le post-processing localise Fallacies mais fige le contenu Virtues en FR — même comportement que la baseline). Les 8 langues Fallacies sont localisées ; les Virtues mindmaps ne le sont pas. Corriger = toucher la config post-processing (jugement jsboige, deferred).
- ⚠️ **Validation pixel RTL/CJK = À CONFIRMER jsboige** (eyeball `Fallacies_ar.svg` / `Fallacies_zh.svg`). Le pixel-RTL est figé en coordonnées absolues dans le SVG ; un screenshot n'ajouterait que la détection tofu = défaut viewer-font, pas défaut asset.

### 3.3 PDFs — 8 langues (✅ régén RELEASE fraîche 2026-07-01 + verdict Release PASS)

- **64/64 PDFs**, exit 0 — **régén RELEASE fraîche 2026-07-01** (`3e2fa0c0` + #614, Mode `WebBasedImageGeneration | QuestPdfGeneration`, `-c Release` PNG lossless + CMYK, `EnableParallelism=false` serial, CardPen local). 0 échec, 0 HARVEST-FAILURE/timeout/Mismatch. Bundle GDrive **Release** `review-v0.9.0-RELEASE-2026-07-01/` (3.5 GB) + manifest sha256×64 (`regen-1032-RELEASE-manifest.txt`). **Bundle Debug `review-v0.9.0-2026-06-28/` préservé** (comparaison possible).
- CardSets concernés : Fallacies Web A0/A4/Thumbnails, Tarot, Tarot Virtues, Poker, Print&Play (8 types × 8 langues).
- **Bug #216 (contamination FR) TENU** : image count invariant (le multilingue n'a pas cassé la structure).
- **i18n distinct (anti-leak #216 OK)** : 426-444 MB/langue Release, tailles distinctes (pas de FR leaké) ; ar/fa légèrement plus légers (RTL shaping) = attendu.
- ✅ **Verdict visuel Release ai-01 = PASS** (2026-07-01 20:55, pdftoppm 120dpi + pdfimages/pdfinfo colorspace) : **#119 Rules-first** (Print&Play FR p1 = livret Règles en premier), **recto-verso** propre (faces p1 / 6 dos p2), **#216 pas de fuite FR** (FR/EN/RU/PT dans la bonne langue), **micro-RU PK79 RÉSOLU** (garble `чшск-то` disparu sur harvest frais, confirme diagnostic stale-harvest), rendu 300 PPI net 0 artefact.
- ⚠️ **Finding 1 — CMYK absent (appel à décision jsboige)** : ai-01 a vérifié le colorspace au niveau image **et** document sur FR Tarot : **198 DeviceRGB / 0 DeviceCMYK / 0 OutputIntent / 0 ICCBased**. Toutes les images = RGB, encoding **FlateDecode lossless** (0 DCTDecode). Le bundle Debug est **identique** (RGB + Flate + 300 PPI). → Le différentiateur `-c Release` est la **losslessness (Flate vs JPEG DCT)**, **PAS le CMYK** : le path CMYK Release (`DocumentCardSet.cs`) ne s'est pas matérialisé. RGB-300-lossless est imprimable (conversion imprimeur), donc non-bloqueur print, mais **si CMYK embarqué est requis pour l'imprimeur → investigation du path Release** (§5.7).

### 3.4 OWL Ontologie — BILINGUE EN/FR (régén fraîche 2026-07-02 sur `c2a9b761`, #634)

- `docs/ontology/argumentum.owl` — **5,314,381 B (5.07 MB)**, SKOS + AIF, **1 408 fallacies**, littéraux **bilingues EN/FR** (5 558 EN + 4 861 FR), 2 816 `prefLabel`, 1 408 `broader` (hiérarchie complète). **Régén fraîche 2026-07-02** sur `c2a9b761` (précédent commit `d206e59c` datait du 2026-03-28, stale ~3 mois — était BLOQUEUR TAG ; taille -64 KB vs stale car le contenu EN/FR CSV a évolué).
- `docs/ontology/argumentum_virtues.owl` — **862,709 B (842 KB), NOUVEAU** (#592) : 223 Virtues, 223 `aif:goodTenorOf`, 7 familles, littéraux bilingues FR/EN (884 FR + 641 EN). Absent du commit `d206e59c` (pré-#592) — désormais committé.
- ⚠️ **Finding scope (downgrade claims honnête)** : le générateur OWL n'embarque **QUE EN+FR** — les 6 autres langues de la release (RU/PT/ES/AR/FA/ZH) **ne sont PAS** dans l'OWL. L'OWL est une ontologie de référence bilingue (FR canonical + EN secondary), **pas** multilingue 8-langues. Les claims docs « 8 langues » ne s'appliquent **pas** à l'OWL : CSV/PDF/SVG = 8 langues, OWL = EN+FR bilingue (par construction du générateur, `OwlGeneratorConfig` mono-`DefaultLanguage`).
- #133 (publication OWL) reste ouvert ; bug round-trip OWLSharp (`rdf:type`/`skos:inScheme` droppés) contourné en scoping readers sur annotations survivantes (`prefLabel`, `DeclarationAxioms`).
- **#499 Phase 2 OWL** — ✅ **MERGED** (PR #592 → master `8d5d275b`) : `VirtueOwlGeneratorConfig` + `VirtueOwlDocumentConfig` + `aif:goodTenorOf`, mono-corpus. L'OWL inclut désormais les métadonnées relationnelles Virtues (fichier `argumentum_virtues.owl` committé via cette régén #634).

### 3.5 DNN (#131)

- Cible : **DNN 10.3.2 + 2sxc 21** (décision #458 #2).
- CVE : 9.13.x ferme **0** CVE ; cible minimale pour les 2 CVE = 10.1.2 (pragmatique 10.3.2 avant falaise 2sxc @10.2.0).
- ⚠️ **Couplé à la release** (décision #4) → statut DNN = **gate de publication**. Audit prep non-destructif po-2023 (idle lane) — pas d'upgrade destructif sans GO jsboige.

### 3.6 Finding — Titre Rules PT mistranslé (⚠️ appel à décision jsboige)

- **Symptôme** (ai-01, 2026-07-01 20:55) : PT Tarot page 4 affiche le titre **« Roll of the English Channel »** (anglais + faux) au-dessus d'un corps PT correct.
- **Root cause** : homonyme « Manche » (un *round* de jeu → La Manche géographie). **Contenu CSV source** (pas lane harvest) : 5 occurrences dans les 2 Rules CSV ; « Round Sequence » (correct) = 0×. EN/RU rendent correctement, **PT surface l'anglais cassé**.
- **Statut** : fix prep dispatché à po-2024 (traduction PT native + clean orphelins EN/RU), **PR GATED jsboige**. Non-bloqueur géométrie/print (1 titre, 1 carte, 1 langue).
- **→ appel à décision jsboige** : block le tag v0.9.0 (attendre le fix PT) ou fast-follow post-tag ?

---

## 4. Toolchain & qualité

| Critère | État | Preuve |
|---------|------|-------|
| Build solution zéro-warning CS | ✅ | PR #587 (master `6caf5833`) |
| Build zéro-warning NU (NuGet audit) | ✅ | PR #588 (NU1903 clos MIT-pur) |
| Tests | **549 pass / 0 fail / 5 skip** | test run `3e2fa0c0` ; skip = GUI/Freeplane (session interactive) |
| SkipConfigFile | `true` (C# defaults = source unique) | règle HARD projet |
| Dépendances stables | QuestPDF 2022.12.12, Magick.NET **14.14.0** (bump 2026-07-01, `dotnet test` GREEN), Playwright 1.43.0 | — |

---

## 5. Points nécessitant décision jsboige avant tag

1. **Validation pixel RTL/CJK des SVGs** (#3.2) — ✅ **VERDICT SOURCE-LEVEL ai-01 = PASS** (technique). Pixel eyeball jsboige optionnel (le pixel-RTL est figé en coordonnées absolues ; un screenshot n'ajouterait que la détection tofu = défaut viewer-font, pas défaut asset).
2. **Verdict visuel PDFs** — ✅ **FAIT = PASS** (ai-01, 2026-07-01 20:55 verdict Release). Régén RELEASE fraîche 2026-07-01 sur `3e2fa0c0`+#614, 64/64 PDFs PNG lossless + CMYK, exit 0, `Generation finished.`. #119 Rules-first, recto-verso, #216 pas de fuite FR, **micro-RU PK79 résolu**, rendu 300 PPI. Multilingue/RTL/CJK validés. Go-live sur régén Release fraîche 2026-07-01.
3. **DNN #131 couplé** — ✅ **MIGRATION FULL-IIS FERMÉE (2026-07-01)** : `dnn.argumentum.myia.io` LIVE full-IIS direct (HTTP 200/85 KB, 0× « Something went wrong », HTTPS SAN 9D80D4CC), DB SQL Express + PortalAlias table clean, stopgap `dnn.myia.io` retiré. **Verdict visuel site = jsboige (RDP)**. Le couplage n'est plus un bloqueur assets — po-2023 recommande toujours de **tagger v0.9.0 assets-only** (DNN prod go-live = ops VPS jsboige, séparé).
4. **Tag v0.9.0** — pas encore posé (`git tag` vide). Débloqué côté géométrie/contenu. À poser après les 2 findings ci-dessous (§5.7 + §3.6) + validation visuelle jsboige.
5. **CHANGELOG.md** — **✅ corrigé** (ligne 16, patch cf §6, merged via #591). **`docs/RELEASE-NOTES-v0.9.0.md` créé** — la release est documentée par CHANGELOG.md + RELEASE-NOTES.
6. **#499 Phase 2 OWL** — ✅ **livré** (PR #592 merged `8d5d275b`) **avant** ce dossier. Mentionné dans release notes. Fait acquis.
7. **Finding CMYK absent (NOUVEAU, ai-01 20:55)** — le bundle Release est **RGB-300-lossless** (FlateDecode), **0 DeviceCMYK/ICC** — le path CMYK `DocumentCardSet.cs` ne s'est pas matérialisé. **→ appel à décision jsboige** : CMYK embarqué requis pour l'imprimeur (→ investigation path Release) ou RGB-300-lossless OK (conversion imprimeur) ? Non-bloqueur print (RGB imprimable).
8. **Finding titre PT « Roll of the English Channel »** (NOUVEAU, ai-01 20:55, cf §3.6) — homonyme « Manche » (round→géographie), contenu CSV (5 occurrences), PT surface l'anglais cassé. Fix prep po-2024, **PR GATED jsboige**. **→ appel à décision jsboige** : block le tag ou fast-follow post-tag ?

---

## 6. Patch CHANGELOG proposé (ligne 16)

**Avant** :
```
- **MindMap SVGs**: FreeMind mind maps committed for FR/EN/RU/PT (21 SVGs) with localized node text, family hierarchy, and descriptions; the pipeline is extended (`StaticConversions` + `MindMapLocalization`) for ES/AR/FA/ZH with SVG regeneration pending (#458 Track 1a)
```

**Après** :
```
- **MindMap SVGs**: FreeMind/Batik mind maps committed for all 8 languages (FR/EN/RU/PT refreshed + ES/AR/FA/ZH added, PR #565) with localized node text, family hierarchy, and descriptions, including RTL (ar/fa) and CJK (zh) rendering via Tahoma-capable fonts
```

---

## 7. Risques résiduels (honnête)

- **Validation pixel** : spot-check Playwright **FAIT = PASS** par ai-01 (2026-07-01) sur `Fallacies_Web_Thumbnails` p1 × 5 langues (zh/ar/fa/ru/es — CJK + RTL + cyrillique + latin) + verdict Release (pdftoppm/pdfimages, #119 + recto-verso + micro-RU résolu). Couverture complète pixel sur l'ensemble non faite (Playwright cale sur le poids SVG/PDF — mur d'outillage documenté). Validation donc **source-level + spot-check + colorspace/encoding inspection**, pas pixel exhaustif.
- ~~**Régén release non reproduite**~~ — ✅ **LEVÉ** : régén **Release** fraîche **2026-07-01** exécutée (§3.3, `3e2fa0c0`+#614, serial `-c Release`), 0 échec, 64/64 PDFs, verdict visuel PASS. 0 régression.
- ~~**DNN couplé**~~ — ✅ **LEVÉ** : migration full-IIS **fermée** (2026-07-01), `dnn.argumentum.myia.io` LIVE. Le couplage n'est plus un bloqueur assets (DNN prod go-live = ops VPS jsboige, séparé).
- ~~**Params Debug vs Release**~~ — ✅ **RÉSOLU** : jsboige a validé Release (GO interactif 2026-07-01). Bundle Release `-c Release` (PNG lossless + CMYK visé) produit, verdict PASS. **Caveat honnête** : le CMYK visé ne s'est pas matérialisé (bundle = RGB-300-lossless, §3.3 Finding 1) — appel à décision jsboige si CMYK embarqué requis.
- **Finding titre PT cassé** : 1 carte Rules PT affiche « Roll of the English Channel » (homonyme, §3.6). Fix prep po-2024 gated. Non-bloqueur géométrie/print mais décision jsboige (block vs fast-follow).
- **Note de procédure (stale-harvest + parallélisme + CardPen host)** : la régên Release 2026-07-01 a required `EnableParallelism=false` (serial) après diagnostic parallélisme=6 → timeout 300s → `Mismatch` throw (résolu par #614 résilience + serial). **CardPen Pages = échec structurel** (404 `/Cards/`, #629) → pivot CardPen local (Golden Master, #629 workaround). **Bug Spectre `[HARVEST-FAILURE]`** (#630) court-circuite #614 sur set-failure → 2 bugs tracés post-tag. Documenté en mémoire.

---

## 8. Recommandation po-2023

1. **GO jsboige sur verdict Release PASS ai-01** (§3.3 — géométrie #119, recto-verso, #216, micro-RU résolu, 300 PPI) → assets validés côté technique.
2. **2 findings = 2 calls jsboige** : (a) **CMYK** (§5.7) — RGB-300-lossless suffit ou CMYK embarqué requis (investigation path Release) ? (b) **titre PT** (§3.6) — block le tag ou fast-follow post-tag ?
3. **Régén fraîche** : ✅ **FAITE** (Release 2026-07-01, §3.3). Go-live sur cette régén (ou Debug bundle préservé).
4. **Décision couplage DNN** : dé-coupler — tagger v0.9.0 assets-only (DNN prod = ops VPS jsboige, migration déjà LIVE en recette).
5. **Tag v0.9.0** après (1)+(2)+(4).

---

*Ce dossier est un draft pour revue. Il sera commit + PR après validation du contenu par jsboige. Pas de code prod touché.*
