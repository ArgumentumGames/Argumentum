# Argumentum v0.9.0 — Release Notes (DRAFT)

**Statut** : DRAFT pour validation jsboige (pré-tag juillet 2026)
**Auteur** : po-2023 (draft), verdict jsboige
**Scope** : 8 langues (fr / en / ru / pt / es / ar / fa / zh)

> Draft auto-construit depuis `CHANGELOG.md` + `docs/RELEASE-VALIDATION-v0.9.0.md`. À coller dans le GitHub Release une fois jsboige validé.

---

## 🎉 v0.9.0 — Internationalisation complète (8 langues)

Argumentum v0.9.0 étend l'intégralité du pipeline de génération à **8 langues** : Français, Anglais, Russe, Portugais, Espagnol, Arabe, Farsi et Chinois — incluant le rendu **RTL** (arabe, farsi) et **CJK** (chinois).

### ✨ Nouveautés

**Données (8 langues, 100% couverture)**
- Fallacies (1408 nœuds), Virtues (223), Scenarii (167), Rules : traduction complète (titre, description, exemple, hiérarchie des familles, liens) sur les 8 langues.
- Couche relationnelle/AIF ajoutée aux Virtues : 12 colonnes (66→78), cross-links Virtue↔Fallacy (`crossLink_Opposes`) et références AIF (`AIF_skosDirectRef`, `AIF_skosMappingType`).

**Assets générés (8 langues)**
- **PDFs** : CardSets (Tarot, Poker, posters A0, Print&Play Standard/Light, Mémo, Rules) localisés pour les 8 langues. **Bundle v3 (2026-07-03) : 80 PDFs (10 types × 8 langues, expansion P&P #648-650)**, post-process Ghostscript CMYK (#632/#652) = 80/80 DeviceCMYK + OutputIntent SWOP (cf dossier v4 §3.3 pour le détail).
- **MindMap SVGs** : 8 langues via FreeMind/Batik, incluant RTL (ar/fa) et CJK (zh, 5.45 MB — glyphes denses). Polices arabes-capables (Tahoma).
- **OWL** : ontologie **bilingue EN/FR uniquement** (Fallacies 5.07 MB + Virtues 842 KB, #592/#499 Phase 2), SKOS + AIF. ⚠ **Scope honnête** : l'OWL n'embarque PAS les 6 autres langues de la release (générateur mono-`DefaultLanguage`) — le claim 8 langues s'applique à CSV/PDF/SVG, pas à l'OWL.
- **Print&Play numérique gratuit** : le jeu numérique est entièrement gratuit — **Light** (colonne `print_and_play` + Virtues overview subset) et **Standard** (toutes les cartes), #645/#648-650.

**Infrastructure pipeline**
- DatasetUpdater : migration SDK OpenAI .NET v2.10.0, multi-provider, function calling + structured output.
- Sync GSheet ↔ CSV bidirectionnel (6 couches de protection upload).
- Harness QA visuel : détecteur de collision footer + Playwright visual regression.
- Build **zéro-warning** (CS + NuGet audit). AutoMapper reste MIT pur (14.0.0 + `MaxDepth(1)`, CVE NU1903 non-exploitable justifiée).

### 🔧 Corrections (recovery Oct 2025 → Jun 2026)

Restauration complète du pipeline depuis l'état Golden Master (avril 2024, `0087f0ec`) :
- HarvestManager (timeout 120s, generateImages explicite, race CardPen Release)
- PdfManager (lock global QuestPDF non thread-safe, positionnement Rules #119)
- CardPen templates (classe CSS `argumentsVertueux`, auto-shrink overflow, chemins assets GitHub absolus)
- CSV injection Golden Master restauré (PapaParse gère les newlines)
- MindMap : SVG Batik haute-fidélité (fallback XSLT retiré #184), automatisation FreeMind SendKeys
- Hygiène CSV : corruption encodage `%C3→A13` systémique réparée dans les templates JSON

### 🧪 Qualité

- **578 tests** passent (5 skips GUI/infrastructure, 1 known-fail = OWLSharp `rdf:type`/`inScheme` round-trip pré-existant, tracké #133 — n'affecte pas les assets générés) — montée depuis 0 en avril 2024.
- Build Debug + Release verts, GitGuardian clean.
- Couverture tests : CsvDiffEngine, SyncSafetyChecker, PdfAssembly, MindMap, ClassMap matrix (Fallacies/Virtues/Scenarii), localisation.

### 📦 Migration / notes techniques

- `SkipConfigFile = true` (les tuples cassent la sérialisation JSON des `Translations`).
- CSV en lecture seule avant injection CardPen (jamais de `.Replace("\n","\\n")`).
- Lock global QuestPDF : ne pas paralléliser la génération PDF.
- Timeout Playwright : ≥120s harvesting, 300s CardSets lourds.

### 🔒 Sécurité (DNN site — piste post-release, non-bloquante pour v0.9.0)

Le site DNN (`DNNPlatform/`) reste sur DNN 9.11.1 + 2sxc 21.07. Deux CVE critiques (CVE-2025-64095 RCE CVSS 10.0, CVE-2025-52488 NTLM CVSS 8.6) sont ouvertes — leur patch nécessite DNN **10.1.2+** (9.13.x ferme 0 CVE). Cible actée : **DNN 10.3.2 + 2sxc 21** (décision #458), runtime .NET Framework 4.8 (pas .NET 8). Recommandation : upgrade DNN en jalon ops post-release séparé (~4-6h migration 12 templates RazorComponent + ops VPS). Détails : `docs/dnn/UPGRADE-ASSESSMENT.md`.

---

## ℹ️ Connaître l'état complet

- Dossier de validation release : [`docs/RELEASE-VALIDATION-v0.9.0.md`](RELEASE-VALIDATION-v0.9.0.md)
- Changelog technique : [`CHANGELOG.md`](../CHANGELOG.md)
- Historique de recovery (37 rapports) : `docs/investigations/`

---

*Draft po-2023. Décisions jsboige en attente (cf dossier §5 : pixel RTL/CJK, régén fraîche, coupling DNN, tag).*
