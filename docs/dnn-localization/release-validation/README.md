# DNN Site — Dossier de validation release (Epic #131/#132)

**Auteur :** Claude Code @ myia-po-2023 (worker) — 2026-06-16
**Objet :** Ossature du dossier que jsboige utilisera pour valider la release du **site web DNN**
(DNN + 2sxc, 8 langues). C'est l'artefact de validation **site**, distinct du dossier de validation
**cartes** (`docs/release-v0.9.0-validation-brief.md`).
**Statut :** **Skeleton** — le site n'est pas encore déployé publiquement (Phase A = boot local IIS
Express, DNN 9.11.1 + 2sxc 15.02, port 8090). Ce dossier est **préparé à l'avance** ; il est utilisable
dès maintenant sur le site local et, sans changement, sur un staging/prod futur.
**Gate release :** ⛔ TOUJOURS ACTIF. Ce dossier **prépare** la validation, il ne la remplace pas et ne
lève rien. Verdict visuel/contenu = jsboige. Le worker signale, il ne déclare pas PASS.

---

## 1. Pourquoi un dossier séparé « site »

La release v0.9.0 a **deux tracks** validés indépendamment :

| Track | Artefacts | Dossier de validation |
|-------|-----------|----------------------|
| **Cartes** (PDF/PNG/mindmaps/OWL) | 64 PDFs × 8 langues, ~9 834 images, 20 SVGs | `docs/release-v0.9.0-validation-brief.md` + `docs/publication/qa-scenario-8langues-release.md` |
| **Site DNN** (ce dossier) | UI strings 8 langues, Fallacy/Rules Explorers, rendu RTL/CJK | **`docs/dnn-localization/release-validation/`** (ici) |

Le track cartes est couvert. Ce dossier couvre le **track site**, qui n'existait pas.

## 2. Structure du dossier

| Fichier | Rôle | Quand jsboige l'utilise |
|---------|------|--------------------------|
| `README.md` (ce fichier) | Index + manifeste + relations | Point d'entrée — lire en premier |
| `validation-checklist.md` | Checklist par langue + par feature (Explorers, labels, nav) | Pendant la validation site — cocher au fur et à mesure |
| `non-latin-verification-guide.md` | Guide pour les langues que jsboige ne lit pas (ar/fa/zh, +ru) — chaînes attendues + signatures visuelles | Pour valider ar/fa/zh sans lire la langue |
| `2sxc-export-spec.md` | Spec d'export 2sxc pour vérifier les 7 FR inférés + extraire le contenu DB-only | **Avant** la validation — débloque #490 |

## 3. Workflow de validation (jsboige)

1. **Préalable — export 2sxc** (voir `2sxc-export-spec.md`) : vérifier les 7 FR inférés `res.*` vs le
   dictionnaire Resources live. Si un FR inféré diffère, le corriger dans `dnn-ui-strings.csv` puis
   re-run #457 (OpenAI direct, clé rechargée). Tant que c'est ouvert, la PR #490 reste **HELD**.
2. **Lever le hold #490** une fois le FR vérifié + les langues validées.
3. **Validation site** (sur local IIS Express `http://localhost:8090` maintenant, staging/prod plus
   tard) : parcourir chaque langue avec `validation-checklist.md`.
4. **Langues non lues** (ar/fa/zh) : s'appuyer sur `non-latin-verification-guide.md` (chaînes
   attendues + contrôles de direction/police).
5. **Bugs connus à vérifier** : FallacyExplorer pas culture-aware (pin `_en`) — voir §4 de
   `../PHASE1-content-audit.md`. À corriger en Phase 2/4.

## 4. Prérequis / dépendances

- **2sxc export** (étape 1 ci-dessus) — gate réel pour la fiabilité du FR source.
- **Fix FallacyExplorer culture** (audit §4) — sans lui, l'Explorer affiche `text_en` quelle que soit
  la langue → invalide toute validation ar/fa/zh/ru/pt/es de l'Explorer. À planifier Phase 2/4.
- **Déploiement staging** — pour une validation représentative du prod (le local IIS suffit pour
  valider le rendu, pas la perf/SSL).

## 5. Relations avec les autres docs

- `../PHASE1-content-audit.md` — audit de localisation (source des 10 chaînes + bug §4).
- `../dnn-ui-strings.csv` — données source (#490, HELD).
- `../../dnn/UPGRADE-ASSESSMENT.md` — assessment migration DNN 9.13→10.x (track déploiement).
- `../../release-v0.9.0-validation-brief.md` — validation release **cartes** (l'autre track).

---

*Worker signals; visual/content verdict and merge are jsboige's. Release gate untouched.*
