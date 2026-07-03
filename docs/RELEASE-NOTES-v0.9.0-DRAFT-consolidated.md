# Argumentum v0.9.0 — Release Notes (DRAFT, consolidated)

**Status:** DRAFT — for jsboige validation before the GitHub Release.
**Author:** po-2024 (consolidation), builds on the po-2023 draft ([`docs/RELEASE-NOTES-v0.9.0.md`](RELEASE-NOTES-v0.9.0.md)).
**Base:** master `ca5db818` (2026-07-03).
**Scope:** 8 languages (fr / en / ru / pt / es / ar / fa / zh).

> Paste-ready body for the GitHub Release once jsboige has validated. This draft consolidates the po-2023 draft with the post-bundle-v3 work (CMYK, P&P Standard/Light, Rules i18n refonte, harvest/logger hardening). The earlier draft remains as historical context.

---

## 🎉 v0.9.0 — Full internationalisation (8 languages)

Argumentum v0.9.0 extends the entire generation pipeline to **8 languages** — French, English, Russian, Portuguese, Spanish, Arabic, Farsi, Chinese — including **RTL** (Arabic, Farsi) and **CJK** (Chinese) rendering.

### ✨ Highlights

- **8 languages × 10 documents, print-ready.** Fresh Release regen (bundle v3, 2026-07-03): 80 PDFs across the 8 languages, PNG-300-lossless, covering Tarot, Poker, A0 posters, Mémo, Rules, and Print&Play. ([dossier §3](release-v0.9.0-validation-brief.md))
- **Print & Play, free and complete (#645, #648–650).** Two tiers, both free:
  - **Standard** — the full game (all cards).
  - **Light** — a historical sample, incl. a Virtues "families overview" subset (depth ≤ 2, 24 cards) so new players can print a playable taster.
- **Print-ready CMYK + SWOP OutputIntent (#632, #652).** A standalone Ghostscript post-pass (`--pdf-cmyk`) converts the final PDFs to DeviceCMYK and embeds the SWOP OutputIntent (GTS_PDFX / CGATS TR 001), verified end-to-end on a real A0. The pipeline ships PNG-300-lossless RGB, with CMYK applied as a deterministic post-process.
- **Rules i18n cleanup (#633 → #640).** Machine-translation garbage in the Rules (e.g. "English Channel" surfacing in PT/RU) was fully refactored via gpt-5.5; 0 HIGH findings residual. A committed anti-false-positive scanner now guards the translations.

### 🔧 Notable fixes since the validation dossier (#591)

- **Reproducible harvest deadlock resolved (#651).** A Playwright deadlock that froze generation at 0% CPU was root-caused to synchronous logging flooding the event-dispatch thread; fixed with an async drain queue. Validated PASS 3/3.
- **Logger hardening (#630, #655).** The console logger could crash a run on markup in card data; all console paths now escape Spectre markup, with a non-throwing render fallback.
- **Scenarii 6.1.3 title duplication fixed (#653).** A title duplicated across 6 languages was corrected via gpt-5.5 ("Johnny Johnny" → proper, scanner total now 0).
- **Google Sheet Rules sync (#642).** The shared "Argumentum Rules" sheet was migrated to two clean sheets mirroring the repo (byte-perfect), with the legacy partner tab archived non-destructively.

### 🧪 Quality

- Build Debug + Release green, **zero-warning** (CS compiler + NuGet audit). AutoMapper MIT-pure (14.0.0 + `MaxDepth(1)`).
- Test suite: ~566 pass / 5 skip (GUI/infrastructure) / 1 known-fail (OWLSharp `rdf:type`/`inScheme` round-trip, pre-existing, tracked #133 — does not affect generated assets). *(Count from dashboard baseline; will be confirmed empirically at tag.)*
- Coverage: CsvDiffEngine, SyncSafetyChecker, PdfAssembly, MindMap, localisation regression tests.

### 📦 Technical notes

- `SkipConfigFile = true` — the C# property initializers are the single source of truth (tuple `Translations` would break JSON round-trip).
- CSV is read-only before CardPen injection (no `.Replace("\n","\\n")`).
- QuestPDF PDF generation uses a global lock (not thread-safe); generation is serial.

### ⚠️ Known limitations

- **SVG mind maps (#636):** if not regenerated in an RDP/foreground session before tag, the committed SVGs may be stale in some languages. Tracked separately; does not block the print/PDF deliverables.
- **OWL ontology round-trip (#133):** a pre-existing OWLSharp bug drops `rdf:type`/`inScheme` on XML round-trip; the generated ontology is otherwise complete (SKOS + AIF).
- **DNN site CVEs:** the DNN site (DNN 9.11.1 + 2sxc 21.07) carries two open CVEs; the DNN 10.3.2 upgrade is scoped as a separate ops milestone post-release. Not a v0.9.0 blocker.

---

## 🎉 v0.9.0 — Internationalisation complète (8 langues) — FR

Argumentum v0.9.0 étend l'intégralité du pipeline à **8 langues** : Français, Anglais, Russe, Portugais, Espagnol, Arabe, Farsi, Chinois — incluant le rendu **RTL** (arabe, farsi) et **CJK** (chinois).

### ✨ Points forts

- **8 langues × 10 documents, prêts à imprimer.** Régén Release fraîche (bundle v3, 03/07/2026) : 80 PDFs sur les 8 langues, PNG-300-lossless (Tarot, Poker, posters A0, Mémo, Rules, Print&Play).
- **Print & Play, gratuit et complet (#645, #648–650).** Deux niveaux : **Standard** (jeu complet) et **Light** (échantillon historique + subset Virtues « families overview », depth ≤ 2, 24 cartes).
- **CMYK prêt à l'impression + OutputIntent SWOP (#632, #652).** Post-pass Ghostscript standalone (`--pdf-cmyk`) : conversion DeviceCMYK + OutputIntent SWOP (GTS_PDFX / CGATS TR 001), vérifié sur A0 réel.
- **Assainissement Rules i18n (#633 → #640).** Refonte complète du garbage MT (ex. « English Channel » en PT/RU) via gpt-5.5 ; 0 finding HIGH résiduel. Scanner anti-faux-positifs committé.

### 🔧 Corrections notables depuis le dossier #591

- **Deadlock harvest résolu (#651).** Gel reproductible à 0% CPU root-causé (flood de logging synchrone sur le thread d'événements Playwright) ; fix par drain queue asynchrone. PASS 3/3.
- **Durcissement Logger (#630, #655).** Échappement Spectre markup sur toutes les console paths + fallback non-throwing.
- **Titre Scenarii 6.1.3 (#653).** Duplication sur 6 langues corrigée via gpt-5.5 (scanner : 0).
- **Sync Google Sheet Rules (#642).** Migration vers 2 feuilles propres (miroir repo, byte-perfect) + onglet partenaire legacy archivé non-destructivement.

### 🧪 Qualité

- Build Debug + Release verts, **zéro-warning** (CS + audit NuGet). AutoMapper MIT pur (14.0.0 + `MaxDepth(1)`).
- Suite de tests : ~566 pass / 5 skip / 1 known-fail OWLSharp #133 (pré-existant, sans impact sur les assets). *(Compteur issu du dashboard baseline ; confirmé empiriquement au tag.)*

### ⚠️ Limitations connues

- **SVG mind maps (#636) :** potentiellement stale dans certaines langues si non régénérés en session RDP/foreground avant le tag. Suit séparément ; ne bloque pas les livrables print/PDF.
- **Round-trip OWL (#133) :** bug OWLSharp pré-existant (perte `rdf:type`/`inScheme` au round-trip XML) ; l'ontologie générée reste complète (SKOS + AIF).
- **CVEs site DNN :** le site (DNN 9.11.1 + 2sxc 21.07) porte deux CVE ouvertes ; l'upgrade DNN 10.3.2 est un jalon ops séparé post-release. Non-bloquant pour v0.9.0.

---

## ℹ️ Pour aller plus loin

- Dossier de validation release : [`docs/RELEASE-VALIDATION-v0.9.0.md`](../RELEASE-VALIDATION-v0.9.0.md)
- Changelog technique : [`CHANGELOG.md`](../CHANGELOG.md)
- Historique de recovery (37+ rapports) : `docs/investigations/`

---

*DRAFT po-2024 (consolidation). Décisions jsboige en attente (cf dossier #591 : pixel RTL/CJK, coupling DNN, tag).*
