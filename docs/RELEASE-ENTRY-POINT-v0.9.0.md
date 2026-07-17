# Argumentum v0.9.0 — Entry Point (où tout commence)

**Date** : 2026-07-17
**Statut** : Tag différé — gate (b) review T&A + gate (d) DNN ops
**Pour** : jsboige, Thomas, Adeline, et tout nouvel arrivant sur la release
**Scope** : 8 langues (fr / en / ru / pt / es / ar / fa / zh)

> **TL;DR** — v0.9.0 d'Argumentum est **prête sur les assets** (verdict visuel ai-01 = PASS). Le **tag** reste différé jusqu'à 2 validations externes : **(b)** retour Thomas & Adeline (véhicule review live), **(d)** ops DNN complètes (Manifests #490/#682 + connection-string tuning, partiellement appliquée — voir `option-c-connection-string-runbook.md`). Ce fichier est l'**index de démarrage** : il pointe vers chaque livrable vérifié et chaque gate en attente.

---

## 1. Vous êtes pressé ? (30 secondes)

| Question | Réponse | Source |
|----------|---------|--------|
| **Combien de PDFs ?** | **80** (10 types × 8 langues), post-process Ghostscript CMYK (80/80 DeviceCMYK + OutputIntent SWOP) | `docs/release-dossier/` §3.3 |
| **Combien de langues ?** | **8** (fr / en / ru / pt / es / ar / fa / zh) — incluant RTL (ar, fa) et CJK (zh) | décision #4 / #7 |
| **Combien de nœuds Fallacies ?** | **1408** (8 langues, ~100% couverture) | issue #335 |
| **Combien de tests ?** | **596 pass / 0 fail / 5 skip / 601 total** | master `fccb5b3d` (round-trip #133 fixé #793) |
| **Mind maps ?** | **20** (8 Fallacies + 8 Virtues + 4 interm.), FreeMind Batik, RTL/CJK OK | PRs #565, #819, #822 |
| **OWL ontology ?** | **Bilingue EN/FR** (5.07 MB Fallacies + 842 KB Virtues), SKOS + AIF | issue #499 Phase 2 #592 |
| **DNN site ?** | **UP & CERTIFIÉ** (idle-hang fixé 2026-07-17 13:38), reste Manifests Δ #490/#682 (gate d jsboige) | `option-c-connection-string-runbook.md` |

---

## 2. Les 3 portes d'entrée par audience

### 2.1 Pour jsboige (Product Owner)

**Décisions en attente** (gates bloquant le tag) :
- **(b)** Retour Thomas & Adeline sur le **véhicule review live** (64 PDFs + 16 mindmaps interactifs en prod IIS). Issue de tracking : **#802**.
- **(d)** Ops DNN complètes — Manifests Δ #490/#682 + tuning connection-string complémentaire. Backup `DNNPlatform/web.config.bak-20260717` disponible.

**Décisions post-tag** (peuvent attendre) :
- `#202 A/B` (registre `political_example_en`)
- `#415 shrink` (git history rewrite, 2.2 GB — interdit autonome)
- `#654 mnémoniques Virtues` (script #695 livré, --apply gated post-tag)
- `#812 _csv_note marker` (PR #814)
- `#818 BOM A/B/C` (encoding)
- `#804 résidus` mindmap (3 fixés, Phase 4 regen SVG HOLD post-tag)

→ Voir `docs/release-dossier/` (tome v4/v5).

### 2.2 Pour Thomas & Adeline (Relecteurs T&A)

**Véhicule review** : https://github.com/ArgumentumGames/Argumentum/releases/tag/v0.9.0-review (LIVE)
- 64 PDFs en lecture directe (HTTP 200 vérifié).
- 16 mindmaps HTML interactifs (8 Fallacies, 8 Vertues, click-to-define).
- Bundle complet sur GDrive : `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/` (6.5 GB).

**Issues de feedback** : utiliser le véhicule + commentaires par carte (pas un dump massif).
- Mindmap click-to-define : **1397-1408 nœuds** par Fallacies, **219-223** par Vertues, click sur un nœud → définition dans la langue active.
- Print&Play Standard + Light : 2 versions (Light = colonne `print_and_play` + Virtues overview subset).

→ Voir `docs/release-dossier/RELEASE-VISUAL-GO-SESSION-v0.9.0.md` (méthode de relecture pas-à-pas).

### 2.3 Pour un nouveau contributeur / mainteneur

**Parcours d'onboarding** :
1. **CLAUDE.md** (racine) — règles du projet (CSV injection, QuestPDF lock, `SkipConfigFile`, etc.).
2. **Ce fichier** (entry-point) — vous y êtes.
3. **`docs/release-dossier/RELEASE-VALIDATION-v0.9.0.md`** — état technique exhaustif (v5.1).
4. **`docs/dnn/sandbox-bootstrap-runbook.md`** — si vous touchez au DNN.
5. **`docs/investigations/`** — 37 rapports d'archéologie si vous débuggez.

**Build & test** :
```bash
dotnet build "Argumentum Converters.sln"           # 0 warning CS+NU attendu
dotnet test "Generation/Converters/Argumentum.AssetConverter.Tests/"  # 596+ pass attendu
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/"  # pipeline complet (Debug : ~15min, Release : ~45min)
```

**Régénération bundle Release** :
```bash
git checkout master && git pull --ff-only
dotnet run --project "Generation/Converters/Argumentum.AssetConverter/" -c Release
# Bundle dans bin/Release/net9.0/Target/{fr,en,ru,pt,es,ar,fa,zh}/Documents/density-0/
```

---

## 3. Couverture assets (80 PDFs, scope honnête)

| Type | fr | en | ru | pt | es | ar | fa | zh | Total | Notes |
|------|----|----|----|----|----|----|----|----|-------|-------|
| Fallacies Tarot | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Recto-verso 1 dos + N faces par famille |
| Fallacies Web (A0) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Poster 841×1189mm, 12 colonnes |
| Fallacies Web (A4) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Print compact |
| Fallacies Web (Thumbnails) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Aperçu rapide |
| Virtues Tarot | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Faces-only |
| Scenarii Poker | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Recto-verso |
| Rules Tarot | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Inclus dans Tarot |
| Memo Tarot | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | 1 carte (rscount=200) |
| Print&Play Standard | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Toutes cartes |
| Print&Play Light | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **8** | Col `print_and_play` + Virtues overview |
| | | | | | | | | **Total** | **80** | 10 types × 8 langues |

**Colorimétrie** : 80/80 PDFs en **DeviceCMYK + OutputIntent SWOP** (post-process Ghostscript #632, entry --pdf-cmyk #652).
**Encodage** : UTF-8 (pas UTF-16 legacy). Mindmaps `content.svg` 8 langues : 1 résidu encoding #804 #3 fixé (post-tag).

**Scope honnête** : OWL ontology **bilingue EN/FR uniquement** (générateur mono-`DefaultLanguage`). Les 6 autres langues sont dans CSV/PDF/SVG, **pas** dans OWL. Issue #133 publication différée post-tag.

---

## 4. Validations techniques (verdicts rendus)

### 4.1 Verdict visuel ai-01 (Release = PASS)

| Vérification | Résultat | Date | Source |
|--------------|----------|------|--------|
| Géométrie (recto-verso, dimensions) | **PASS** | 2026-07-01 20:55 | bundle Release |
| Contenu (#216 pas de fuite FR EN/RU/PT/ES/AR/FA/ZH) | **PASS** | 2026-07-01 (post-#216 fix #592 + cascades #765-#770) | bundle Release |
| Micro-RU PK79 garble `чшск-то` | **RÉSOLU** | 2026-07-01 (harvest frais post-fix encoding) | bundle Release |
| CMYK (80/80 DeviceCMYK + SWOP) | **PASS** | 2026-07-04 (post-process Ghostscript #632) | bundle v3 |
| RTL (ar/fa) rendu | **PASS** | 2026-07-04 | bundle Release |
| CJK (zh) rendu | **PASS** | 2026-07-04 | bundle Release |
| Mindmap interactivité (Fallacies 8 langues) | **PASS** (1397-1408 nœuds) | 2026-07-17 #822 | release v0.9.0-review |

### 4.2 Tests .NET

```
596 pass / 0 fail / 5 skip / 601 total
- xUnit : 0 fail
- CS compiler warnings : 0
- NuGet audit : 0
- OWL2XML round-trip #133 : fixé (#793 — assertion `BeGreaterThan(0)` corrige le faux-négatif obsolète)
```

### 4.3 DNN prod (live sur `dnn.argumentum.myia.io`)

```
Cold-start post-idle 26min : 200 5.2s (pre-fix: 60-95s) — vérifié 2026-07-17 13:38
Warm-up : <1s
```

---

## 5. Pipeline qualité (rappels pour contributeurs)

| Règle | Raison | Source |
|-------|--------|--------|
| `SkipConfigFile = true` (délibéré) | Tuples cassent la sérialisation JSON des Translations | `AssetConverterConfig.cs:34` |
| Ne JAMAIS modifier le CSV avant injection CardPen | PapaParse gère les newlines | `HarvestManager.cs` |
| Ne JAMAIS forcer `rscount=0` | Préserver la valeur du template JSON | docs/CLAUDE.md |
| `CsvType` obligatoire par DataSet | Sans ça, harvest vide | `AssetConverterConfig.cs` |
| QuestPDF : lock global | Non thread-safe | `PdfManager.cs` |
| Pas de `RowsetNb` quand template `rscount` suffit | Casse le template | `WebBasedGeneratorConfig.cs` |

**Hygiène post-régénération** :
- Avant toute régénération localisation → **clobber le harvest cache** (stale harvest = bug silencieux).
- Avant tout byte-check multilingue → **couvrir TOUTES les colonnes langue** (zh/ru/ar, pas seulement FR/EN).
- Mindmap automation : **ONE `dotnet run` + kill `javaw`** (pas de XSLT fallback).

---

## 6. Roadmap post-tag (vue d'ensemble)

| # | Issue | Description | Status |
|---|-------|-------------|--------|
| #134 | Tag GitHub v0.9.0 | **GATED** par (b) T&A + (d) DNN ops | EN ATTENTE |
| #133 | Publication OWL | Bilingue EN/FR, certifié publication-safe #790 | DIFFÉRÉ post-tag |
| #131/#132 | DNN go-live complet | Manifests Δ #490/#682 + ops | GATED jsboige |
| #202 | Campagne EN post-release | Plan livré #809 | DIFFÉRÉ |
| #654 | Mnémoniques Virtues --apply | Script #695 livré | DIFFÉRÉ |
| #804 | Mindmap regen SVG (Phase 4) | Code-fix fusionné #808 | HOLD post-tag |
| #812 | `_csv_note` markers templates | PR #814 | HOLD post-tag |
| #815 | #804 PREP link mapping | PR #815 | HOLD post-tag |
| #818 | BOM A/B/C encoding | Décision requise | DIFFÉRÉ |

---

## 7. Références profondes

| Fichier | Contenu |
|---------|---------|
| `docs/release-dossier/RELEASE-VALIDATION-v0.9.0.md` | Dossier exhaustif v5.1 (596 tests, AIF, comptages) |
| `docs/release-dossier/RELEASE-NOTES-v0.9.0.md` | Notes de release (collable GitHub Release) |
| `docs/release-dossier/RELEASE-VISUAL-GO-SESSION-v0.9.0.md` | Méthode relecture T&A pas-à-pas |
| `docs/release-dossier/RELEASE-VERIFICATION-INDEX-v0.9.0.md` | Index de tous les verdicts rendus |
| `docs/dnn/option-c-connection-string-runbook.md` | Runbook Partie C fix DNN idle-hang |
| `docs/dnn/sandbox-bootstrap-runbook.md` | Runbook bootstrap sandbox local |
| `docs/dnn/UPGRADE-ASSESSMENT.md` | Cibles 10.3.2 + 2sxc 21 + CVE analysis |
| `docs/dnn/go-live-smoke-test.md` | Smoke test post-déploiement |
| `docs/dnn/go-live-turnkey-checklist.md` | Checklist go-live jsboige |
| `CHANGELOG.md` | Changements techniques versionnés |
| `docs/investigations/` | 37 rapports d'archéologie |

---

## 8. Contacts & escalation

- **jsboige** : Product Owner, décisions gate (b) + (d).
- **ai-01 (myia-ai-01)** : Coordinateur, verdict QA visuelle, merges, dispatch.
- **po-2023 (myia-po-2023)** : Lane DNN ops + driver Argumentum.
- **po-2024 (myia-po-2024)** : Lane Cards/game-content + backlog traduction.

**Canal principal** : dashboard workspace Argumentum (MCP `roo-state-manager`). Le canal **débordement/inbox privé** : `roosync_messages`.

---

*Entry-point rédigé par po-2023, 2026-07-17. À mettre à jour à chaque livraison majeure.*
