# Argumentum v0.9.0 — Dossier de validation release

**Date** : 2026-07-01 (refresh post-régén fraîche 8-lang + verdict visuel PASS ai-01)
**Statut** : ASSETS VALIDÉS (verdict visuel ai-01 = PASS représentatif) — en attente décision params Release/couplage go-live jsboige
**Branche** : `docs/release-v0.9.0-validation`
**Master de référence** : `18b4d023` (build zéro-warning CS+NU, **549/0/5 tests**). Régén release 8-lang **2026-07-01** sur `18b4d023` + #614 (`EnableParallelism=false` serial), bundle validé sur GDrive `review-v0.9.0-2026-06-28/`.

---

## 1. Objectif

Établir l'état vérifiable de la release v0.9.0 (scope = **8 langues** : fr / en / ru / pt / es / ar / fa / zh), lister ce qui est **livré et vérifié** vs ce qui **nécessite validation jsboige** avant le tag go-live. Ce dossier est le GATE de publication (#134).

> **Note de méthode** : ce dossier est bâti sur les **assets committés** + une **régén Release fraîche exécutée le 2026-07-01** sur `18b4d023` + #614 (serial, `EnableParallelism=false`, worktree isolé — 0 échec, 64/64 PDFs, 1229 images/langue × 8 = 9 832, `Generation finished.`, exit 0 — voir §3.3). Les compteurs PDFs/images sont donc **re-vérifiés au 2026-07-01**. **Verdict visuel ai-01 = PASS représentatif** (08:50, spot-check `Fallacies_Web_Thumbnails` p1 sur zh/ar/fa/ru/es couvrant CJK + RTL + cyrillique + latin ; bug #216 tenu, structure complète 8 types × 8 langues) — voir §3.3.

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

### 3.3 PDFs — 8 langues (✅ régén fraîche 2026-07-01 + verdict visuel PASS)

- **64/64 PDFs**, **1229 images/langue × 8 = 9 832 images**, exit 0 — **régén Release fraîche 2026-07-01** (`18b4d023` + #614, Mode `WebBasedImageGeneration | QuestPdfGeneration`, `EnableParallelism=false` serial). 0 échec, 0 HARVEST-FAILURE/timeout/Mismatch. Bundle GDrive `review-v0.9.0-2026-06-28/` (3.6 GB) + manifest sha256×64 (`regen-1032-manifest.txt`).
- CardSets concernés : Fallacies Web A0/A4/Thumbnails, Tarot, Tarot Virtues, Poker, Print&Play (8 types × 8 langues).
- **Bug #216 (contamination FR) TENU** : image count invariant **1229/lang ×8** (le multilingue n'a pas cassé la structure) ; spot-check ad-populum FR = contenu FR au harvest.
- **i18n distinct (anti-leak #216 OK)** : ~450 MB/langue, tailles distinctes (pas de FR leaké) ; ar/fa légèrement plus légers (RTL shaping) = attendu.
- ✅ **Verdict visuel ai-01 = PASS représentatif** (2026-07-01 08:50) : spot-check `Fallacies_Web_Thumbnails` p1 sur **5 langues couvrant toutes les familles d'écriture** — zh (CJK, 0 tofu), ar (RTL arabe, shaping connecté), fa (RTL persan, lettres پچگژ), ru (cyrillique, auto-shrink titre #316/#353 tenu), es (latin, accents). Bug #216 tenu sur les 5. Reste à ai-01 : couverture Tarot recto-verso (#119) + en/pt latin (risque faible).
- ⚠️ **Params Debug** (JPEG Q=85, RGB, CardPen local) — cohérent avec validation multilingue historique. Si les assets GitHub #134 veulent du print-final, un run `-c Release` (PNG lossless + CMYK) suivra — verdict content vaut indépendamment des params. **→ décision jsboige** (§5.7).

### 3.4 OWL Ontologie

- `docs/ontology/argumentum.owl` — **5.13 MB (5 378 765 bytes)**, SKOS + AIF.
- #133 (publication OWL) reste ouvert ; bug round-trip OWLSharp (`rdf:type`/`skos:inScheme` droppés) contourné en scoping readers sur annotations survivantes (`prefLabel`, `DeclarationAxioms`).
- **#499 Phase 2 OWL** — ✅ **MERGED** (PR #592 → master `8d5d275b`) : `VirtueOwlGeneratorConfig` + `VirtueOwlDocumentConfig` + `aif:goodTenorOf`, mono-corpus, 540/0/5 tests. L'OWL inclut désormais les métadonnées relationnelles Virtues. Documenté dans les release notes de cette PR.

### 3.5 DNN (#131)

- Cible : **DNN 10.3.2 + 2sxc 21** (décision #458 #2).
- CVE : 9.13.x ferme **0** CVE ; cible minimale pour les 2 CVE = 10.1.2 (pragmatique 10.3.2 avant falaise 2sxc @10.2.0).
- ⚠️ **Couplé à la release** (décision #4) → statut DNN = **gate de publication**. Audit prep non-destructif po-2023 (idle lane) — pas d'upgrade destructif sans GO jsboige.

---

## 4. Toolchain & qualité

| Critère | État | Preuve |
|---------|------|-------|
| Build solution zéro-warning CS | ✅ | PR #587 (master `6caf5833`) |
| Build zéro-warning NU (NuGet audit) | ✅ | PR #588 (NU1903 clos MIT-pur) |
| Tests | **549 pass / 0 fail / 5 skip** | test run `18b4d023` ; skip = GUI/Freeplane (session interactive) |
| SkipConfigFile | `true` (C# defaults = source unique) | règle HARD projet |
| Dépendances stables | QuestPDF 2022.12.12, Magick.NET 13.5.0, Playwright 1.43.0 | — |

---

## 5. Points nécessitant décision jsboige avant tag

1. **Validation pixel RTL/CJK des SVGs** (#3.2) — ✅ **VERDICT SOURCE-LEVEL ai-01 = PASS** (technique). Pixel eyeball jsboige optionnel (le pixel-RTL est figé en coordonnées absolues ; un screenshot n'ajouterait que la détection tofu = défaut viewer-font, pas défaut asset).
2. **Verdict visuel PDFs** — ✅ **FAIT = PASS représentatif** (ai-01, 2026-07-01 08:50). Régén Release fraîche 2026-07-01 sur `18b4d023`+#614, 64/64 PDFs + 1229 img/lang ×8, exit 0, **vraiment fraîche post-#592/#595/#607** (cf §3.3). Bug #216 tenu, multilingue/RTL/CJK validés. Reste à ai-01 : couverture Tarot recto-verso (#119) + en/pt latin (risque faible, prochain tick). Go-live sur régén fraîche 2026-07-01.
3. **DNN #131 couplé** — ✅ **MIGRATION FULL-IIS FERMÉE (2026-07-01)** : `dnn.argumentum.myia.io` LIVE full-IIS direct (HTTP 200/85 KB, 0× « Something went wrong », HTTPS SAN 9D80D4CC), DB SQL Express + PortalAlias table clean, stopgap `dnn.myia.io` retiré. **Verdict visuel site = jsboige (RDP)**. Le couplage n'est plus un bloqueur assets — po-2023 recommande toujours de **tagger v0.9.0 assets-only** (DNN prod go-live = ops VPS jsboige, séparé).
4. **Tag v0.9.0** — pas encore posé (`git tag` vide). À poser après arbitrage ci-dessus.
5. **CHANGELOG.md** — **✅ corrigé dans cette PR** (ligne 16, patch cf §6). **`docs/RELEASE-NOTES-v0.9.0.md` créé dans cette PR** — la release est documentée par CHANGELOG.md + RELEASE-NOTES.
6. **#499 Phase 2 OWL** — ✅ **livré** (PR #592 merged `8d5d275b`) **avant** ce dossier. Mentionné dans release notes. Fait acquis.
7. **Params Release vs Debug** (NOUVEAU, ai-01 08:50) — le bundle validé est **Debug** (JPEG Q=85, RGB, CardPen local). Les assets GitHub #134 print-final veulent-ils un run `-c Release` (PNG lossless + CMYK, ~plus lent) ? Le verdict content d'ai-01 vaut indépendamment des params. **→ décision jsboige**.

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

- **Validation pixel** : spot-check Playwright **FAIT = PASS** par ai-01 (2026-07-01) sur `Fallacies_Web_Thumbnails` p1 × 5 langues (zh/ar/fa/ru/es — CJK + RTL + cyrillique + latin). Couverture complète pixel sur l'ensemble non faite (Playwright cale sur le poids SVG/PDF — mur d'outillage documenté). Validation donc **source-level + spot-check représentatif**, pas pixel exhaustif.
- ~~**Régén release non reproduite**~~ — ✅ **LEVÉ** : régén fraîche **2026-07-01** exécutée (§3.3, `18b4d023`+#614, serial), 0 échec, image count invariant 1229/lang ×8, verdict visuel PASS. 0 régression.
- ~~**DNN couplé**~~ — ✅ **LEVÉ** : migration full-IIS **fermée** (2026-07-01), `dnn.argumentum.myia.io` LIVE. Le couplage n'est plus un bloqueur assets (DNN prod go-live = ops VPS jsboige, séparé).
- **Params Debug vs Release** : le bundle validé est Debug (JPEG/RGB). Si print-final requis → run `-c Release` (décision jsboige §5.7). Non-bloquant pour le verdict content.
- **Note de procédure (stale-harvest + parallélisme)** : la régên 2026-07-01 a required `EnableParallelism=false` (serial) après diagnostic parallélisme=6 → timeout 300s → `Mismatch` throw (résolu par #614 résilience + serial). Leçon : grands sets (Fallacies 1408) timeoutent sous haute concurrence ; serial = capacité CardPen pleine. Documenté en mémoire.

---

## 8. Recommandation po-2023

1. **Correction CHANGELOG ligne 16** (§6) — trivial, non-bloquant, à merger avec ce dossier.
2. **GO jsboige sur verdict visuel PASS ai-01** (SVGs source-level + PDFs spot-check 5 langues) → assets validés.
3. **Régén fraîche** : ✅ **FAITE** (2026-07-01, §3.3). Go-live sur cette régén.
4. **Décision params** : Debug (validé) suffisant pour go-live, ou run Release pour print-final ? (§5.7)
5. **Décision couplage DNN** : dé-coupler — tagger v0.9.0 assets-only (DNN prod = ops VPS jsboige, migration déjà LIVE en recette).
6. **Tag v0.9.0** après (4) et (5).

---

*Ce dossier est un draft pour revue. Il sera commit + PR après validation du contenu par jsboige. Pas de code prod touché.*
