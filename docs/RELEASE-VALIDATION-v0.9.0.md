# Argumentum v0.9.0 — Dossier de validation release

**Date** : 2026-06-23
**Statut** : DRAFT — pour revue jsboige (go-live v0.9.0)
**Branche** : `docs/release-v0.9.0-validation`
**Master de référence** : `22eb5f34` (build zéro-warning CS+NU, 533/0/5 tests)

---

## 1. Objectif

Établir l'état vérifiable de la release v0.9.0 (scope = **8 langues** : fr / en / ru / pt / es / ar / fa / zh), lister ce qui est **livré et vérifié** vs ce qui **nécessite validation jsboige** avant le tag go-live. Ce dossier est le GATE de publication (#134).

> **Note de méthode** : ce dossier est bâti sur les **assets committés** + une **régén Release fraîche exécutée le 2026-06-25** sur `bef3bc6c` (64/64 PDFs, 9 834 images, exit 0 — voir §3.3). Les compteurs PDFs/images sont donc **re-vérifiés ce jour** (régên fraîche post-#592/#595), sauf le verdict visuel qui reste à ai-01 (§3.3, règle HARD).

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
| **zh** | **3** | **nouveau, CJK — 17.2 MB (18 075 919 bytes, glyphes denses)** |

- Moteur : **FreeMind 1.0.1 + Batik** (haute fidélité, décision #184 — fallback XSLT retiré).
- **Reproductibilité byte-proven** (régên 8-lang 2026-06-24, RDP jsboige, exit 0) : `Fallacies_zh.svg` committed = fresh = 5 451 309 B (`cmp` IDENTICAL byte-for-byte). La baseline #565 se reproduit fidèlement → le moteur FreeMind/Batik est **stable, pas flaky** (mitige le risque §7 « régén non reproduite » pour le volet MindMap).
- Validation technique ai-01 (source-level) : contenu Unicode authentique par langue (0 fallback FR), géométrie d'arbre quasi-identique, `font-family='Tahoma'` arabe-capable, racines correctes (`السفسطة` pour ar). **PASS technique.**
- ⚠️ **Gap structurel mineur (non-bloquant v0.9.0)** : les Virtues `.content.svg` sont **FR-figés** (le post-processing localise Fallacies mais fige le contenu Virtues en FR — même comportement que la baseline). Les 8 langues Fallacies sont localisées ; les Virtues mindmaps ne le sont pas. Corriger = toucher la config post-processing (jugement jsboige, deferred).
- ⚠️ **Validation pixel RTL/CJK = À CONFIRMER jsboige** (eyeball `Fallacies_ar.svg` / `Fallacies_zh.svg`). Le pixel-RTL est figé en coordonnées absolues dans le SVG ; un screenshot n'ajouterait que la détection tofu = défaut viewer-font, pas défaut asset.

### 3.3 PDFs — 8 langues (✅ régén fraîche 2026-06-25)

- **64/64 PDFs**, **9 834 images**, exit 0 — **régén Release fraîche 2026-06-25** (`bef3bc6c`, Mode `WebBasedImageGeneration | QuestPdfGeneration`).
- CardSets concernés : Tarot, Poker, A0 posters, Print&Play, Memo, Rules (8 langues × 8).
- **Cohérent avec les derniers merges** : #592 (OWL Virtues `aif:goodTenorOf`) + **#595 (24 cells Virtues harmonisées RTL/CJK)** — PDFs Virtues re-rendered post-#595 (clobber targeted harvest, Playwright/Chromium invoqué, labels familiaux localisés vérifiés dans les noms PNG : zh `有效论证`, ar `حجة_معتبرة`, fa).
- **i18n distinct (anti-leak #216 OK)** : PokerCards 108-114M/langue, tailles distinctes (pas de FR leaké) ; ar/zh légèrement plus petits (glyphes) = attendu.
- ⚠️ **Régén = headless** (Playwright+QuestPDF) — le Mode actuel n'inclut pas Mindmapper, donc **pas besoin de fenêtre RDP** (seul Mindmapper nécessite RDP, voir §3.2, déjà fait byte-proven).
- ⚠️ **Verdict visuel = ai-01** (règle HARD) : spot-check Playwright 1 carte / langue à confirmer (contenu localisé + RTL ar/fa + CJK zh). po-2023 signale counts/preuves, ne déclare pas PASS.

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
| Tests | **540 pass / 0 fail / 5 skip** | test run Release `bef3bc6c` 2026-06-25 (AssetConverter.Tests, 22s) ; skip = GUI/Freeplane (session interactive) ; +7 vs `22eb5f34` = tests OWL Virtues #592 |
| SkipConfigFile | `true` (C# defaults = source unique) | règle HARD projet |
| Dépendances stables | QuestPDF 2022.12.12, Magick.NET 13.5.0, Playwright 1.43.0 | — |

---

## 5. Points nécessitant décision jsboige avant tag

1. **Validation pixel RTL/CJK des SVGs** (#3.2) — eyeball ou GO sur verdict source-level ai-01.
2. **Régén PDF fraîche ?** — ✅ **FAITE (2026-06-25)**. Régén Release 8-langues exécutée sur `bef3bc6c` (GO jsboige direct), 64/64 PDFs + 9834 images, exit 0, **vraiment fraîche post-#595** (cf §3.3). Le risque résiduel « 16 commits master depuis régén 12 juin » (ancien §7) est **levé**. Reste : verdict visuel ai-01 (point 1 analogue pour PDFs). Go-live sur régén fraîche 2026-06-25.
3. **DNN #131 couplé** — **po-2023 recommande : DÉ-COUPLER.** Tagger v0.9.0 assets-only maintenant ; upgrade DNN (cible 10.3.2 + 2sxc 21, actée #458) en jalon ops post-release séparé. Justification complète + chiffrement effort migration 12 templates (~4-6h, code-only) : `docs/dnn/UPGRADE-ASSESSMENT.md` §10 (PR #593). Résumé : assets complets & vérifiés ; upgrade DNN = tâche ops VPS (jsboige only, pas automatable) ; site actuel fonctionnel (9.11.1 + 2sxc 21.07) ; les 2 CVE critiques = dette sécu, pas bloqueur de livraison des assets.
4. **Tag v0.9.0** — pas encore posé (`git tag` vide). À poser après arbitrage ci-dessus.
5. **CHANGELOG.md** — **✅ corrigé dans cette PR** (ligne 16, patch cf §6). **`docs/RELEASE-NOTES-v0.9.0.md` créé dans cette PR** (n'existait pas sur master `22eb5f34`) — la release est documentée par CHANGELOG.md + RELEASE-NOTES.
6. **#499 Phase 2 OWL** — ✅ **livré** (PR #592 merged `8d5d275b`) **avant** ce dossier. Mentionné dans release notes. Conservé pour traçabilité — plus une décision ouverte, un fait acquis.

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

- **Validation pixel non faite** sur l'ensemble (Playwright cale systématiquement sur le poids SVG — mur d'outillage documenté par ai-01). La validation est donc **source-level**, pas pixel.
- ~~**Régén release 12 juin non reproduite**~~ — ✅ **LEVÉ** : régén fraîche 2026-06-25 exécutée (§3.3), reproduit fidèlement les counts 12 juin (64 PDFs, 9834 img) + intègre #592/#595. 0 régression détectée.
- **DNN couplé** : si la release doit attendre la migration DNN (10.3.2 + 2sxc 21), le go-live glisse.
- **Note de procédure (stale-harvest)** : la régên 2026-06-25 a initialement servi le cache harvest (10 juin, pré-#595) — détecté (log `"Skip existing image"` + 0 Chromium) et corrigé par **clobber targeted Virtues** + re-régên (Chromium invoqué, harvests frais 02:33). Leçon : clobber MANDATORY avant régên post-fix-localization (le count identique ne prouve pas la fraîcheur — seule l'invocation Chromium le prouve). Documenté en mémoire.

---

## 8. Recommandation po-2023

1. **Correction CHANGELOG ligne 16** (§6) — trivial, non-bloquant, à merger avec ce dossier.
2. **GO jsboige sur verdict source-level SVGs** (ai-01 a validé technique) → débloque le volet MindMap sans attente screenshot.
3. **Décision régén fraîche** : ✅ **FAITE** — régén fraîche 2026-06-25 exécutée (§3.3). Go-live sur cette régên. Reste verdict visuel ai-01.
4. **Décision couplage DNN** : attendre #131 ou dé-coupler pour tag v0.9.0 assets-only.
5. **Tag v0.9.0** après (3) et (4).

---

*Ce dossier est un draft pour revue. Il sera commit + PR après validation du contenu par jsboige. Pas de code prod touché.*
