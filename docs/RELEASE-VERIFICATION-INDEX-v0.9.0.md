# Argumentum v0.9.0 — Index de vérification docs release (pre-tag gate)

**Objet** : index unique pour que **jsboige** puisse reviewer en un seul passage **tous les docs
release** avant de poser le tag v0.9.0 (#134). Un seul point d'entrée → un lien + une ligne
« à vérifier » par doc.

**Master de référence** : `84a529bf` (refresh v5.1 2026-07-12 : tests empiriques **596/601 (0 rouge, round-trip #133 corrigé #793)**). Précédent : `81a9e4e6` (v5 2026-07-09 : intégration colonnes AIF #753/#754/#755). Tous
les docs listés ci-dessous sont **sur master** (vérifiables maintenant). Le tag est techniquement
débloqué (verdicts #140 contenu + #632 CMYK = PASS rendus) — il attend les arbitrages jsboige
(§« Décisions restantes »).

> **Comment utiliser cet index** : pour chaque ligne, ouvrir le lien, eyeball-vérifier le point
> « à vérifier », puis cocher. Les docs stale (framework historique) sont regroupés en §8 avec un
> avertissement — ils ne sont PAS le tag-gate (le dossier §1 l'est).
>
> **🎯 Pour la session GO visuel des ASSETS** (PDFs/SVGs du bundle GDrive, ~45 min) : voir
> [RELEASE-VISUAL-GO-SESSION-v0.9.0.md](RELEASE-VISUAL-GO-SESSION-v0.9.0.md) — parcours guidé des 6
> PDFs prioritaires + guide vérif non-latin (AR/FA RTL, ZH CJK, RU) + les 4 arbitrages GO/no-GO.
> Cet index couvre la review des **docs** ; le parcours couvre la review des **assets** — complémentaires.

---

## §1 — TAG GATE (le doc qui contrôle la publication)

| Doc | À vérifier |
|-----|------------|
| [RELEASE-VALIDATION-v0.9.0.md](RELEASE-VALIDATION-v0.9.0.md) | **Dossier de validation v5 (refresh 2026-07-12)** — 80 PDFs CMYK, verdicts #140/#632 RENDUS PASS, **+ §3.1bis : colonnes AIF relationnelles Fallacies+Virtues (#753/#754/#755, metadata-only, 0 impact rendu)**. Master `84a529bf`, tests **596/601 (0 rouge, round-trip #133 corrigé #793)** empiriques. **2 appels à décision** : (a) SVG Virtues `.content.svg` FR-frozen (#636/#654, non-bloquant géométrie), (b) titre PT « Roll of the English Channel » (§3.6, fix prep po-2024 gated). Confirme si l'un ou l'autre bloque le tag. |

---

## §2 — Release notes (coller dans la GitHub Release)

| Doc | À vérifier |
|-----|------------|
| [RELEASE-NOTES-v0.9.0-DRAFT-consolidated.md](RELEASE-NOTES-v0.9.0-DRAFT-consolidated.md) | **Corps paste-ready consolidé (#659, EN)** — 80 PDFs, CMYK #632/#652, P&P Standard/Light #645/#648-650, Rules i18n #633→#640, harvest deadlock #651, logger #630/#655, Scenarii #653, GSheet #642. ⚠️ Le compteur tests dans ce draft (~566 pass / 1 known-fail) est **stale** — la valeur empirique courante est **596 pass / 0 fail / 5 skip** (#793 a clos le known-fail #133). Le draft lui-même est owned po-2024 (lane release-notes) ; l'action de mise à jour du compteur dans le corps est à porter par po-2024. |
| [RELEASE-NOTES-v0.9.0.md](RELEASE-NOTES-v0.9.0.md) | Draft po-2023 (FR) — dit encore « 64 PDFs », pas de CMYK. Conservé pour contexte historique. **Ne PAS coller** dans la GitHub Release ; utiliser le consolidé ci-dessus. |
| [CHANGELOG.md](../CHANGELOG.md) | ⚠️ **GAP — à mettre à jour avant tag** : l'entrée v0.9.0 **ne mentionne pas** CMYK post-process (#632/#652), bundle v3 80 PDFs, P&P Standard/Light (#645/#648-650), stage Ghostscript, harvest deadlock (#651), logger (#630/#655). Prédate tout le travail bundle-v3. |

---

## §3 — CMYK / colorimétrie

| Doc | À vérifier |
|-----|------------|
| [PdfCmykPostProcess/README.md](../Generation/Converters/Argumentum.AssetConverter/PdfCmykPostProcess/README.md) | #632 CMYK+OutputIntent via post-pass Ghostscript. **Scope claim** : « CMYK + OutputIntent, PAS PDF/X-3 formel » (pas de trim/bleed boxes). Confirme que ça matche l'exigence de l'imprimeur (pas de certif PDF/X-3 requise). |
| `CMYK_COLOR_PROOF.txt` (GDrive only) | **Pas dans le repo** — vit dans le bundle GDrive `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/`. Si tu veux ce proof tracké en repo, il faut l'ajouter ; sinon confirme review depuis GDrive. |

---

## §4 — Résilience pipeline (harvest retry / deadlock / logger)

| Doc | À vérifier |
|-----|------------|
| [2026-07-04-retry-serial-smoke-test.md](investigations/2026-07-04-retry-serial-smoke-test.md) | #676/#613 — analyse statique du chemin retry + rationale HOLD d'un smoke runtime pre-tag. **Accepte** que la validation runtime du retry soit post-tag (seule la couverture unitaire `RetryAsync` #678 est pre-tag). |

(Les fixes deadlock #651 et logger #630/#655 sont documentés dans les release notes consolidées §2, pas en investigation standalone.)

---

## §5 — DNN i18n / prod go-live (#131/#132 — couplé au tag)

| Doc | À vérifier |
|-----|------------|
| [2026-07-03-dnn-i18n-porting-mechanism.md](investigations/2026-07-03-dnn-i18n-porting-mechanism.md) | #669 — mécanisme de portage i18n. **Claim dure** : RulesExplorer n'a aucun plumbing i18n, contenu DB-only (pas en repo, pas dérivable du CSV Rules). L'unblocker = export 2sxc (gate jsboige). |
| [2026-07-04-dnn-2sxc-rule-schema-export.md](investigations/2026-07-04-dnn-2sxc-rule-schema-export.md) | #681 — export read-only du schema Rule depuis backup. **Finding clé** : content-type Rule = champs génériques (0 champ suffixé), EAV dimensionné FR+EN seulement. Force la décision #682 vers Path A (lang-suffixed + `loc()`). |
| [2026-07-03-dnn-prod-rules-coverage.md](investigations/2026-07-03-dnn-prod-rules-coverage.md) | #662 — audit prod : 5/5 jeux FR présents, 7 langs non publiées. **Découplage** : aucune donnée perdue, le gap = publication FR-only (portage = separate fast-follow). |
| [dnn-hang-option-c-connection-string-prep.md](dnn/dnn-hang-option-c-connection-string-prep.md) | Option C tuning connection-string SQL (prep, non appliqué). **Root-cause = hypothèse** (staleness pool post-idle) — à confirmer côté VPS. Ne pas traiter les valeurs comme un fix confirmé. |
| [dnn/go-live-smoke-test.md](dnn/go-live-smoke-test.md) | Checklist smoke go-live 10.3.2. **Checks obligatoires cliff** : `DnnJsInclude` IIS-crash (A3) + 25 apps 2sxc stock chargées. |

---

## §6 — Couverture i18n (qualité données)

| Doc | À vérifier |
|-----|------------|
| [2026-07-03-rules-i18n-coverage-audit.md](investigations/2026-07-03-rules-i18n-coverage-audit.md) | #661 — couverture cell-by-cell Rules (15×8 = 120/120 OK). **Spot-check** : PK 371 & 607 `desc_en` (citées vides = déjà peuplées 120/79 chars). |
| [2026-07-03-polish-trad-sweep-v090-confirm.md](investigations/2026-07-03-polish-trad-sweep-v090-confirm.md) | #667 — sweep scanner #647, 4 datasets, TOTAL = 0 finding (1813 rows). Confirme que le scanner #647 est la version re-run sur `27442add`. |
| [taxonomy/taxonomy-coherence-scan.md](taxonomy/taxonomy-coherence-scan.md) | Cohérence terminologique Fallacies (intra-lang), 0 finding. Les 3 axes (contamination / harmonization FR / cohérence) tous verts ensemble. |
| [2026-07-02-scanner-fp-arbitration.md](investigations/2026-07-02-scanner-fp-arbitration.md) | Arbitrage 7 faux-positifs scanner (post-#640) → tous KEEP. **PK 475 & 927** : FR source garde des termes non traduits (Gish gallop, Creepypasta) → miroir autres langs = consistant. |
| [2026-07-02-gdrive-rules-642-structural-analysis.md](investigations/2026-07-02-gdrive-rules-642-structural-analysis.md) | #642 — analyse structurale GDrive Rules vs repo (24 rows cross boundaries). A mené à la décision Option-3 two-clean-sheets. |
| [2026-07-03-gdrive-rules-642-migration-table.md](investigations/2026-07-03-gdrive-rules-642-migration-table.md) | #642 audit trail 24 rows → verdicts migration. **Correction** : le claim antérieur « 12 NO-MATCH = variants absentes » était ERRONÉ — les 4 variants sont présentes. |
| [2026-06-23-prod-csv-hygiene-audit.md](investigations/2026-06-23-prod-csv-hygiene-audit.md) | Hygiène CSV prod — Memo `%C3→A13` fixé, autres clean. **1 finding** : 9 cellules RU « Mathematical accuracy » (pk 71-78) = anglais non traduit « Valid results » — confirmé résolu ou documenté comme limitation. |

---

## §7 — Mindmaps / OWL

| Doc | À vérifier |
|-----|------------|
| [2026-07-04-499-virtues-owl-phase2-confirm.md](investigations/2026-07-04-499-virtues-owl-phase2-confirm.md) | #499 Virtues OWL Phase 2 CONFIRMED DONE + freshness-verified (pas stale comme #634). Logique column-diff (seul `print_and_play` changé, colonne non lue par OWL gen). |
| [2026-06-25-virtues-mindmap-fr-frozen-mechanism.md](investigations/2026-06-25-virtues-mindmap-fr-frozen-mechanism.md) | Virtues `.content.svg` FR-frozen gap — mécanisme tracé. **Non-bloquant v0.9.0**, deferred post-release (ship-as-is + known-limitation). |
| `docs/ontology/argumentum.owl` (5.13 MB) | OWL **bilingue EN+FR only** (pas 8-langues). Le dossier §3.5 downgrade explicitement tout claim « 8 langs » pour l'OWL. Vérifier que cette décimation honnête est dans les release notes. |

---

## §8 — Polish / misc (taxonomy, crosslinks, mnemonics, repo)

| Doc | À vérifier |
|-----|------------|
| [2026-07-03-654-mnemonics-celltable.md](investigations/2026-07-03-654-mnemonics-celltable.md) | #668 — cell table apply-ready pour #654 (pks par langue). Surface d'inconsistance 53 cellules (RU14+AR16+ZH6+FA17). **Staged post-tag** (0 write CSV). |
| [2026-07-03-654-virtues-mnemonics-inventory.md](investigations/2026-07-03-654-virtues-mnemonics-inventory.md) | #660/#654 — inventaire 20 rows mnémoniques Latin (pks 106-127). Remplace l'estimé « ~40-60 » par 53 cellules vérifiées. |
| [2026-07-03-629-cardpen-pages-cards-404-diagnostic.md](investigations/2026-07-03-629-cardpen-pages-cards-404-diagnostic.md) | #629 — diagnostic 404 CardPen Pages. 6 paths relatifs `/Cards` restants (Fallacies Face ×4, Backs ×2). **Post-tag fast-follow** (PR #666 `[HOLD post-tag]`). |
| [repo/415-phase2-proposal.md](repo/415-phase2-proposal.md) | #663 — réduction `.git` (2.05 GiB pack), **read-only, non exécuté**. **HORS scope v0.9.0** (history rewrite avant tag = disruptif). |
| [taxonomy/141-crosslink-datasetupdater-adaptation.md](taxonomy/141-crosslink-datasetupdater-adaptation.md) | #141 — adaptation crossLink DatasetUpdater, **gated post-tag**. Le text-enrichment + AIF cross-ref = DONE (1232/1232, 0 fab) ; seul le write-task `crossLink` reste. |
| [taxonomy/141-crosslink-sample-run.md](taxonomy/141-crosslink-sample-run.md) | #141 — sample 7-node curé depuis output #626 (pas un re-run frais). Transparency : curation, pas nouvel appel API. |

---

## §9 — Docs stale (framework historique — NE PAS lire pour le tag)

Ces docs sont **superseded** par le dossier v4 §1. Conservés pour trace, ils peuvent induire en
erreur si lus à froid (affirment « 64 PDFs », « mindmaps 4 langs », etc.). **Aucun n'est le
tag-gate.**

- `docs/release-dossier/README.md` (2026-06-19) — « 64 PDFs, es/ar/fa/zh 0 »
- `docs/release-v0.9.0-validation-brief.md` (2026-06-13) — « 64 PDFs = 8 types × 8 langues »
- `docs/v0.9.0-readiness-and-disk-recovery.md` (2026-06-13) — « 64 PDFs, 155 pass »
- `docs/publication/qa-scenario-8langues-release.md` — « 64 (8 langues × 8 documents) », vieux commit `3aade349`
- `docs/dnn-localization/release-validation/README.md` — « 64 PDFs × 8 langues », site track maintenant post-go-live

---

## Décisions restantes avant tag (arbitrages jsboige)

1. **#636 §1 assets SVG Virtues** (FreeMind GUI-interactif vs defer post-tag).
2. **#654 mnémoniques** — Scope A (no-op) / B (19 cellules min-churn) / global (53 Latin). ⚠ Contradiction dashboard-#42 vs mémoire ai-01 à trancher.
3. **Titre PT « Roll of the English Channel »** (dossier §3.6) — block le tag ou fast-follow post-tag ?
4. **Couplage go-live DNN** — tag immédiat vs aligné au go-live DNN multilingue (portage #669, unblock #681).
5. **Export portal 2sxc** — action jsboige (voie 1 Playwright / grant sysadmin / sa pwd) pour compléter le schema doc #681 et débloquer #682.

---

*Index créé par po-2023 (dispatch `lev5ct` secondaire, 2026-07-04). Tous les chemins vérifiés sur
master `7590dfb8`. CHANGELOG gap (§2) est le seul point nécessitant une édition avant tag. Les
docs §9 stale sont signalés pour éviter lecture froide confuse.*
