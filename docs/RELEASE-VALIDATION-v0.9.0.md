# Argumentum v0.9.0 — Dossier de validation release

**Date** : 2026-06-23
**Statut** : DRAFT — pour revue jsboige (go-live v0.9.0)
**Branche** : `docs/release-v0.9.0-validation`
**Master de référence** : `22eb5f34` (build zéro-warning CS+NU, 533/0/5 tests)

---

## 1. Objectif

Établir l'état vérifiable de la release v0.9.0 (scope = **8 langues** : fr / en / ru / pt / es / ar / fa / zh), lister ce qui est **livré et vérifié** vs ce qui **nécessite validation jsboige** avant le tag go-live. Ce dossier est le GATE de publication (#134).

> **Note de méthode** : ce dossier est bâti sur les **assets committés** + le **rapport de régén release mémorisé** (12 juin 2026 : 64/64 PDFs, 9 834 images, exit 0). Le `bin/Target/` local est vide (post-crash / nettoyage Docker) — aucune régén fraîche n'a été lancée pour ce dossier (pas de GO RDP, pas ma lane sans coordination). Les compteurs « régén 12 juin » sont donc **RAPPORTÉS** (mémoire + dashboard), pas **re-vérifiés** ce jour.

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

### 3.3 PDFs — 8 langues (RAPPORTÉ régén 12 juin)

- **64/64 PDFs**, 9 834 images, exit 0 — **RAPPORTÉ** (régén release 12 juin 2026).
- CardSets concernés : Tarot, Poker, A0 posters, Print&Play, Memo, Rules.
- ⚠️ **Non re-vérifié ce jour** (bin/ vide). Si go-live exige une régén fraîche post-#590/#569, **prévoir un run Release coordonné** (GO jsboige + RDP).

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
| Tests | **533 pass / 0 fail / 5 skip** | dashboard ai-01, master `22eb5f34` ; skip = GUI/Freeplane (session interactive) |
| SkipConfigFile | `true` (C# defaults = source unique) | règle HARD projet |
| Dépendances stables | QuestPDF 2022.12.12, Magick.NET 13.5.0, Playwright 1.43.0 | — |

---

## 5. Points nécessitant décision jsboige avant tag

1. **Validation pixel RTL/CJK des SVGs** (#3.2) — eyeball ou GO sur verdict source-level ai-01.
2. **Régén PDF fraîche ?** — le bin/ est vide. Soit go-live sur la régén 12 juin (rapportée), soit run Release coordonné post-#590/#569.
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
- **Régén release 12 juin non reproduite** : si une régression s'est glissée dans les 16 commits master suivants (dont #590 prod-write Virtues additif, prouvé 0-drift), elle ne serait visible que sur une régén fraîche.
- **DNN couplé** : si la release doit attendre la migration DNN (10.3.2 + 2sxc 21), le go-live glisse.

---

## 8. Recommandation po-2023

1. **Correction CHANGELOG ligne 16** (§6) — trivial, non-bloquant, à merger avec ce dossier.
2. **GO jsboige sur verdict source-level SVGs** (ai-01 a validé technique) → débloque le volet MindMap sans attente screenshot.
3. **Décision régén fraîche** : go-live sur régén 12 juin (accepter risque §7) OU run Release coordonné.
4. **Décision couplage DNN** : attendre #131 ou dé-coupler pour tag v0.9.0 assets-only.
5. **Tag v0.9.0** après (3) et (4).

---

*Ce dossier est un draft pour revue. Il sera commit + PR après validation du contenu par jsboige. Pas de code prod touché.*
