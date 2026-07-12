# Argumentum v0.9.0 — Dossier de validation release

**Date** : 2026-07-09 (refresh **v5** : intégration colonnes AIF relationnelles #753/#754/#755 + master `81a9e4e6`). Refresh v5.1 (2026-07-12) : master `84a529bf`, **tests 596/601 (0 rouge — round-trip #133 corrigé #793)**. Refresh v4.1 (2026-07-05) : master `d90ce613`, aligné CHANGELOG #689. Refresh v4 (2026-07-04) : bundle v3 80 PDFs CMYK + verdicts #140/#632 RENDUS PASS.
**Statut** : ASSETS VALIDÉS (verdict Release ai-01 = PASS géométrie/contenu + verdict #140 multilingue 8 langues RENDU + verdict #632 colorimétrique CMYK RENDU) — en attente d'arbitrages jsboige (SVG #636, mnémoniques #654, couplage go-live DNN) puis tag. **Tag toujours non posé** (`git tag` vide au 2026-07-09).
**Branche** : `docs/release-validation-v5-aif-refresh`
**Master de référence** : `84a529bf` (build zéro-warning CS+NU, **tests 596 pass / 0 fail / 5 skip / 601 total** [empirique `dotnet test` 2026-07-12, po-2023 — suite au fix round-trip #793], Magick.NET 14.14.0). **0 rouge** : le test OWL2XML round-trip #133 n'est plus un known-fail depuis #793 — l'assertion `inScheme.Be(0)` (faux-négatif obsolète : inScheme **survit** au round-trip, empirique 1408) a été corrigée en `BeGreaterThan(0)` ; le résidu réel (`rdf:type` dropped au reload, contourné par le survivor-fallback reader) reste asserté-comme-attendu, documenté, pas une régression. Précédent master : `81a9e4e6` (v5 2026-07-09). **Refresh v4** : bundle **v3** régénéré 2026-07-03 sur `27442add` = **80 PDFs** (10 types × 8 langues, expansion P&P #648-650) PNG-lossless puis **80/80 convertis CMYK** via post-process Ghostscript (#632/#652). Bundle GDrive `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/` (6.5 GB : 80 PDFs CMYK + 7 samples + `CMYK_COLOR_PROOF.txt`).

> **Delta v5 (post-05/07)** : 4 PRs merged sur master entre `d90ce613` et `81a9e4e6` — **#753** (Fallacies AIF : 2 colonnes relationnelles `AIF_attackType`/`AIF_attackedNode`, 46/1408 remplis), **#754** (mirror Virtues script), **#755** (mirror Virtues exécuté, 222/223 remplis), **#756** (audit sérialisation read-only). Détail inventaire §3.1bis. Aucun impact assets rendus (CSV metadata-only, 0 impact CardPen/harvest/PDF) → verdicts #140/#632 inchangés, bundle v3 reste le bundle de référence.

---

## 1. Objectif

Établir l'état vérifiable de la release v0.9.0 (scope = **8 langues** : fr / en / ru / pt / es / ar / fa / zh), lister ce qui est **livré et vérifié** vs ce qui **nécessite validation jsboige** avant le tag go-live. Ce dossier est le GATE de publication (#134).

> **Note de méthode** : ce dossier est bâti sur les **assets committés** + **3 cycles de régén** : (1) Debug 2026-06-28 (JPEG Q85) → bundle `review-v0.9.0-2026-06-28/` ; (2) Release 2026-07-01 (`-c Release` PNG lossless, `3e2fa0c0`+#614) → bundle `review-v0.9.0-RELEASE-2026-07-01/` ; (3) **Bundle v3 2026-07-03** (`27442add`, régén PNG-lossless + post-process Ghostscript CMYK #632/#652) → bundle `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/` = **80/80 PDFs CMYK** (expansion P&P #648-650 : 10 types × 8 langues). Les compteurs sont **re-vérifiés au 2026-07-04**.
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

### 3.1bis Colonnes AIF relationnelles — Fallacies + Virtues (✅ livré v5, PRs #753/#754/#755)

**Nouvel artefact de données (delta v5)** : le chantier AIF #498/#499 a sérialisé la décomposition argumentative I/RA/CA dans **2 nouvelles colonnes CSV** (`AIF_attackType` + `AIF_attackedNode`) sur les **deux taxonomies**, avec un **contrat partagé anti-drift**. Metadata-only (couche additive, 0 impact rendu carte / CardPen / harvest / PDF → verdicts #140/#632 inchangés).

| Taxonomie | Colonnes | Coverage | PR | Master commit | Byte-check ai-01 |
|-----------|----------|----------|----|---------------|-------------------|
| **Fallacies** (1408 nœuds) | `AIF_attackType` (idx 96) + `AIF_attackedNode` (idx 97) | **46/1408** remplis : 44 undercut/RA + 2 undermine/I, **0 rebut**, 17 fail-loud vides | #753 | `d4fde74d` | ✅ 143 616 checks → 0 mismatch |
| **Virtues** (223 nœuds) | `AIF_attackType` (idx 79) + `AIF_attackedNode` (idx 80) | **222/223** remplis : 206 undercut/RA + 13 undermine/I + 3 rebut/CA ; 1 root (pk 0) vide | #755 | `3b68393a` | ✅ 17 617 checks → 0 mismatch |

- **Discipline #677 tenue** : 0 fabrication de tokens. Cellules vides = fail-loud si pas de CQ (corpus question) natif (17 Fallacies).
- **Règle déterministe** (plan #750 v2) : défaut `undercut`/`RA-node` ; override `undermine`/`I-node` si oppose {889,804} ; override `rebut`/`CA-node` si oppose {340}. Distribution Virtues = 206/13/3 (exacte, re-confirmée programmatiquement).
- **Contrat partagé** : les 2 taxonomies utilisent les mêmes noms d'en-têtes + vocab canonique → mirror anti-drift.
- **Impact release** : aucun sur les assets rendus (CSV = métadonnée OWL/EPITA, pas consommée par le pipeline de rendu). L'OWL peut être régénéré pour embarquer ces colonnes (post-tag, #133 scope).
- ⚠️ **#756** (audit sérialisation read-only, `81a9e4e6`) : diagnostic qui **backs l'ASK ai-01** sur la couverture Fill Fallacies. Ne modifie aucune donnée — documente le delta entre 46 modelés et la cible de couverture.

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

### 3.3 PDFs — 8 langues (✅ bundle v3 80 PDFs CMYK + verdicts #140/#632 RENDUS PASS)

- **80/80 PDFs CMYK** — bundle **v3** régénéré 2026-07-03 sur `27442add`. Deux phases : (a) régén QuestPDF PNG-lossless = 80 PDFs (10 types × 8 langues, 6.18 GB), exit propre, 0 échec (pivot CardPen local après crash Pages, serial, deadlock #651 résolu) ; (b) **post-process Ghostscript CMYK** (#632, entry-point `--pdf-cmyk` #652) = **80/80 convertis DeviceCMYK + OutputIntent SWOP** (5.30 GB). Preuve : `TarotCards_fr` = DeviceCMYK 195 / DeviceRGB 0 / OutputIntent 3 / ICC SWOP 2 (23 PDFs initialement timeout à 180s, résolus par bump 180→900s #670).
- **Expansion P&P #648-650** : 64→80 PDFs (+2 doc types = **Print&Play Standard** + **Print&Play Light**, ce dernier = colonne `print_and_play` + Virtues overview). 10 types × 8 langues.
- CardSets concernés : Fallacies Web A0/A4/Thumbnails, Tarot, Tarot Virtues, Poker, P&P Standard, P&P Light (10 types × 8 langues).
- **Bug #216 (contamination FR) TENU** : image count invariant (le multilingue n'a pas cassé la structure).
- ✅ **Verdict contenu #140 = PASS (ai-01, RENDU 2026-07-03)** : 8 langues validées — carte dense p51 rendue EN/ES/RU/AR/FA/ZH (géométrie identique, taxonomie localisée, **RTL AR+FA correct, CJK propre**), covers, **PT #306 fixé** (« A Escola dos Mentirosos »), FR Rules 5 jeux. #119 Rules-first, recto-verso, #216 pas de fuite FR, micro-RU PK79 résolu (confirme diagnostic stale-harvest), rendu 300 PPI.
- ✅ **Verdict colorimétrique #632 = PASS (ai-01, RENDU 2026-07-03)** : indépendamment vérifié sur fr+ar+zh — images cmyk 4-composantes + GTS_PDFX/OutputIntent présents. Le path CMYK Ghostscript (#632/#652) matérialise ce que le path `DocumentCardSet.cs` ne pouvait pas (PNG détruit le CMYK — oxymore documenté, résolu par le post-process GS).
- Bundle GDrive **v3** `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/` (6.5 GB : 80 PDFs CMYK + 7 samples + `CMYK_COLOR_PROOF.txt`). Bundles précédents (Debug `review-v0.9.0-2026-06-28/`, Release v1 `review-v0.9.0-RELEASE-2026-07-01/`) préservés.
- ⚠️ **Finding 1 (CMYK) — ✅ RÉSOLU via #632/#652** : le finding original du 2026-07-01 (« 198 DeviceRGB / 0 DeviceCMYK ») est **résolu** par le post-process Ghostscript (ci-dessus). Le bundle v3 = **80/80 DeviceCMYK + OutputIntent SWOP**. Plus d'appel à décision sur le CMYK.

### 3.4 OWL Ontologie — BILINGUE EN/FR (régén fraîche 2026-07-02 sur `c2a9b761`, #634)

- `docs/ontology/argumentum.owl` — **5,314,381 B (5.07 MB)**, SKOS + AIF, **1 408 fallacies**, littéraux **bilingues EN/FR** (5 558 EN + 4 861 FR), 2 816 `prefLabel`, 1 408 `broader` (hiérarchie complète). **Régén fraîche 2026-07-02** sur `c2a9b761` (précédent commit `d206e59c` datait du 2026-03-28, stale ~3 mois — était BLOQUEUR TAG ; taille -64 KB vs stale car le contenu EN/FR CSV a évolué).
- `docs/ontology/argumentum_virtues.owl` — **862,709 B (842 KB), NOUVEAU** (#592) : 223 Virtues, 223 `aif:goodTenorOf`, 7 familles, littéraux bilingues FR/EN (884 FR + 641 EN). Absent du commit `d206e59c` (pré-#592) — désormais committé.
- ⚠️ **Finding scope (downgrade claims honnête)** : le générateur OWL n'embarque **QUE EN+FR** — les 6 autres langues de la release (RU/PT/ES/AR/FA/ZH) **ne sont PAS** dans l'OWL. L'OWL est une ontologie de référence bilingue (FR canonical + EN secondary), **pas** multilingue 8-langues. Les claims docs « 8 langues » ne s'appliquent **pas** à l'OWL : CSV/PDF/SVG = 8 langues, OWL = EN+FR bilingue (par construction du générateur, `OwlGeneratorConfig` mono-`DefaultLanguage`).
- #133 (publication OWL) reste ouvert (publication infra-stage) ; bug round-trip OWLSharp **partiellement résolu** depuis #793 : `skos:inScheme` **survit** au round-trip (empirique 1408) ; seul `rdf:type` reste droppé au reload, contourné en scoping readers sur annotations survivantes (`prefLabel`, `DeclarationAxioms`). Le test round-trip est désormais **vert** (assertion `inScheme > 0`), le résidu `rdf:type` est asserté-comme-attendu.
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
| Tests | **596 pass / 0 fail / 5 skip / 601 total** | empirique `dotnet test` master `84a529bf` 2026-07-12 (po-2023, post-fix round-trip #793) ; skip = GUI/Freeplane/GSheet (session/creds interactifs). **0 rouge** : #133 round-trip corrigé (inScheme survit, résidu rdf:type asserté-comme-attendu) |
| SkipConfigFile | `true` (C# defaults = source unique) | règle HARD projet |
| Dépendances stables | QuestPDF 2022.12.12, Magick.NET **14.14.0** (bump 2026-07-01, `dotnet test` GREEN), Playwright 1.43.0 | — |

---

## 5. Points nécessitant décision jsboige avant tag

1. **Validation pixel RTL/CJK des SVGs** (#3.2) — ✅ **VERDICT SOURCE-LEVEL ai-01 = PASS** (technique). Pixel eyeball jsboige optionnel (le pixel-RTL est figé en coordonnées absolues ; un screenshot n'ajouterait que la détection tofu = défaut viewer-font, pas défaut asset).
2. **Verdicts visuels PDFs** — ✅ **RENDUS = PASS** (ai-01). (a) Verdict contenu **#140** 8 langues (2026-07-03) : carte dense p51 EN/ES/RU/AR/FA/ZH, RTL/CJK propre, covers, PT #306 fixé, FR Rules 5 jeux. (b) Verdict colorimétrique **#632** (2026-07-03) : DeviceCMYK 4-comp + GTS_PDFX/OutputIntent sur fr+ar+zh. **Bundle v3** = 80/80 PDFs CMYK (Ghostscript #632/#652), 6.5 GB GDrive. #119, recto-verso, #216, micro-RU résolu, 300 PPI. Multilingue/RTL/CJK/CMYK validés.
3. **DNN #131 couplé** — ✅ **MIGRATION FULL-IIS FERMÉE (2026-07-01)** : `dnn.argumentum.myia.io` LIVE full-IIS direct (HTTP 200/85 KB, 0× « Something went wrong », HTTPS SAN 9D80D4CC), DB SQL Express + PortalAlias table clean, stopgap `dnn.myia.io` retiré. **Verdict visuel site = jsboige (RDP)**. Le couplage n'est plus un bloqueur assets — po-2023 recommande toujours de **tagger v0.9.0 assets-only** (DNN prod go-live = ops VPS jsboige, séparé).
4. **Tag v0.9.0** — pas encore posé (`git tag` vide). **Techniquement débloqué** : verdicts #140 (contenu) + #632 (CMYK) = PASS, bundle v3 80/80 CMYK livré. Reste les **arbitrages jsboige** : (a) #636 §1 assets SVG Virtues (FreeMind GUI-interactif vs defer post-tag), (b) #654 mnémoniques (scope A/B/global), (c) décision couplage go-live DNN (immédiat vs après portage). Plus le finding titre PT (§3.6).
5. **CHANGELOG.md** — **✅ corrigé** (ligne 16, patch cf §6, merged via #591). **`docs/RELEASE-NOTES-v0.9.0.md` créé** — la release est documentée par CHANGELOG.md + RELEASE-NOTES.
6. **#499 Phase 2 OWL** — ✅ **livré** (PR #592 merged `8d5d275b`) **avant** ce dossier. Mentionné dans release notes. Fait acquis.
7. **~~Finding CMYK absent~~ — ✅ RÉSOLU** (post-process Ghostscript #632/#652). Le finding original du 2026-07-01 (bundle RGB-300-lossless, 0 DeviceCMYK) est **résolu** : le bundle v3 (2026-07-03) = **80/80 DeviceCMYK + OutputIntent SWOP** (§3.3). Plus d'appel à décision sur le CMYK — le verdict #632 = PASS (ai-01). Le path `DocumentCardSet.cs` (oxymore PNG) est remplacé par le post-process GS sur le PDF final.
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
- ~~**Params Debug vs Release**~~ — ✅ **RÉSOLU** : jsboige a validé Release (GO interactif 2026-07-01). Bundle v3 `-c Release` (PNG lossless) **+ post-process Ghostscript CMYK** (#632/#652) produit, verdict #632 PASS. **Le CMYK visé est désormais matérialisé** : 80/80 DeviceCMYK + OutputIntent SWOP (§3.3).
- **Finding titre PT cassé** : 1 carte Rules PT affiche « Roll of the English Channel » (homonyme, §3.6). Fix prep po-2024 gated. Non-bloqueur géométrie/print mais décision jsboige (block vs fast-follow).
- **Note de procédure (stale-harvest + parallélisme + CardPen host)** : la régên Release 2026-07-01 a required `EnableParallelism=false` (serial) après diagnostic parallélisme=6 → timeout 300s → `Mismatch` throw (résolu par #614 résilience + serial). **CardPen Pages = échec structurel** (404 `/Cards/`, #629) → pivot CardPen local (Golden Master, #629 workaround). **Bug Spectre `[HARVEST-FAILURE]`** (#630) court-circuite #614 sur set-failure → 2 bugs tracés post-tag. Documenté en mémoire.

---

## 8. Recommandation po-2023

1. **GO jsboige sur verdicts PASS ai-01** (§3.3 — #140 contenu 8 langues + #632 CMYK, géométrie #119, recto-verso, #216, micro-RU résolu, 300 PPI) → assets validés côté technique.
2. **Arbitrages jsboige restants** : (a) **#636 §1 SVG Virtues** (FreeMind GUI-interactif vs defer post-tag), (b) **#654 mnémoniques** (scope A/B/global), (c) **titre PT** (§3.6 — block vs fast-follow ; note : #640 a résolu le bulk « English Channel » 23 occurrences HIGH, le titre Tarot PT spécifique est possiblement inclus, à confirmer visuellement au tag), (d) **couplage go-live DNN** (immédiat vs après portage i18n #669/#674).
3. **Régén fraîche** : ✅ **FAITE** (bundle v3 2026-07-03, §3.3, 80/80 CMYK). Go-live sur bundle v3.
4. **Décision couplage DNN** : dé-coupler — tagger v0.9.0 assets-only (DNN prod = ops VPS jsboige, migration déjà LIVE en recette). Le portage i18n site (#669 mécanisme, #674 refactor Rules) est post-tag.
5. **Tag v0.9.0** après (1)+(2).

---

*Ce dossier est un draft pour revue. Il sera commit + PR après validation du contenu par jsboige. Pas de code prod touché.*
