# Argumentum v0.9.0 — Release Notes (DRAFT)

**Statut** : DRAFT pour validation jsboige (pré-tag juillet 2026)
**Auteur** : po-2023 (draft initial), po-2024 (refresh #134 tick 24, alignement 80 PDFs / 596 tests)
**Scope** : 8 langues (fr / en / ru / pt / es / ar / fa / zh)

> Draft auto-construit depuis `CHANGELOG.md` + `docs/RELEASE-VALIDATION-v0.9.0.md`. À coller dans le GitHub Release une fois jsboige validé.
>
> **Refresh tick 24 (po-2024).** Sync des chiffres canoniques : 80 PDFs (10 types × 8 langues) — pas 64. Print&Play Standard + Light ajoutés post-#648-650. 596 tests (post-#807 alignement) — pas 578. AIF attack layer : 145 fallacies + 222 virtues typés. crossLink layer : 1985 relations inter-fallacies across 8 predicates (#763). Bundle v3 CMYK : 80/80 DeviceCMYK + OutputIntent SWOP.

---

## 🎉 v0.9.0 — Internationalisation complète (8 langues)

Argumentum v0.9.0 étend l'intégralité du pipeline de génération à **8 langues** : Français, Anglais, Russe, Portugais, Espagnol, Arabe, Farsi et Chinois — incluant le rendu **RTL** (arabe, farsi) et **CJK** (chinois).

### ✨ Nouveautés

**Données (8 langues, 100% couverture)**
- Fallacies (1408 nœuds), Virtues (223), Scenarii (167), Rules : traduction complète (titre, description, exemple, hiérarchie des familles, liens) sur les 8 langues.
- Couche relationnelle/AIF ajoutée aux Virtues : 12 colonnes (66→78), cross-links Virtue↔Fallacy (`crossLink_Opposes`) et références AIF (`AIF_skosDirectRef`, `AIF_skosMappingType`).
- **AIF attack layer (Fallacies + Virtues, #498/#499)** : 145/1408 fallacies (10.3%) et 222/223 vertus portent une sémantique d'attaque formelle ASPIC+ (undercut/RA-node, undermine/I-node, rebut/CA-node), déterministe via node map. Tiering par confiance : 14 PRECEDENT + 2 PREC-TIE + 36 SUFFIX-ONLY, 0 résiduel skos-only, 0 token inventé (anchor audit #770 : 16 CLEAN / 2 SOFT / 0 erreur).
- **CrossLink layer inter-fallacies (#763)** : **1985 relations émises** dans `argumentum.owl`, portées par **844 fallacies (59,9 % de 1408)**, 8 prédicats relationnels (`predatesOn`, `denounces`, `leverages`, `allows`, `opposes`, `inverts`, `mirrors`, `isRelatedTo`). Côté source CSV cela fait **1081 cellules** non vides (dont 137 multi-valuées, `;`) — les deux chiffres sont justes et comptent des choses différentes : les verbes symétriques (`mirrors`, `isRelatedTo`, `inverts`, `opposes`) sont émis dans les deux sens. Réconciliation détaillée : [`docs/ontology/aif-export/README.md`](ontology/aif-export/README.md) §*Count reconciliation note*.

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

- **596 tests** passent (5 skips GUI/infrastructure, 0 known-fail — le dernier #133 OWLSharp `skos:inScheme` round-trip a été résolu en #793 : 1408 assertions survivent au round-trip ; seule la chute de `rdf:type` au reload reste et est assertée-comme-attendue + contournée par le survivor-fallback read path). Montée depuis 0 en avril 2024.
- Build Debug + Release verts, GitGuardian clean, build zéro-warning (CS + NuGet audit #587).
- Couverture tests : CsvDiffEngine, SyncSafetyChecker, PdfAssembly, MindMap, ClassMap matrix (Fallacies/Virtues/Scenarii), FallaciesLocalizationTests, TaxonomyValidationTests, Memo_Back localization, HarvestManager `RetryAsync` contract (#678), Virtues mindmap wrapper localization (#738), Playwright visual tests.

### 📦 Migration / notes techniques

- `SkipConfigFile = true` (les tuples cassent la sérialisation JSON des `Translations`).
- CSV en lecture seule avant injection CardPen (jamais de `.Replace("\n","\\n")`).
- Lock global QuestPDF : ne pas paralléliser la génération PDF.
- Timeout Playwright : ≥120s harvesting, 300s CardSets lourds.
- Timeout Ghostscript : ≥900s par PDF (#670 — 23 PDFs denses A0 timeout-taient à 180s).
- Post-process CMYK : `Mode=PdfCmykPostProcess` (#632) applique le DeviceCMYK + OutputIntent SWOP sur le bundle final. Per-image `ConvertToCmyk` dans `DocumentCardSet.cs` est un no-op effectif (PNG ne porte pas CMYK) — l'autorité CMYK = GS post-process.

### 🔒 Sécurité (DNN site — piste post-release, non-bloquante pour v0.9.0)

Le site DNN (`DNNPlatform/`) reste sur DNN 9.11.1 + 2sxc 21.07. Deux CVE critiques (CVE-2025-64095 RCE CVSS 10.0, CVE-2025-52488 NTLM CVSS 8.6) sont ouvertes — leur patch nécessite DNN **10.1.2+** (9.13.x ferme 0 CVE). Cible actée : **DNN 10.3.2 + 2sxc 21** (décision #458), runtime .NET Framework 4.8 (pas .NET 8). Recommandation : upgrade DNN en jalon ops post-release séparé (~4-6h migration 12 templates RazorComponent + ops VPS). Détails : `docs/dnn/UPGRADE-ASSESSMENT.md`.

---

## ℹ️ Connaître l'état complet

- Dossier de validation release : [`docs/RELEASE-VALIDATION-v0.9.0.md`](RELEASE-VALIDATION-v0.9.0.md)
- Changelog technique : [`CHANGELOG.md`](../CHANGELOG.md)
- Historique de recovery (37 rapports) : `docs/investigations/`

---

*Draft po-2023. Décisions jsboige en attente (cf dossier §5 : pixel RTL/CJK, régén fraîche, coupling DNN, tag).*
