# DNN Site — Checklist de validation 8 langues

**Usage :** à parcourir pour **chaque langue** sur le site (local IIS Express `http://localhost:8090`
maintenant, staging/prod plus tard). Cocher chaque case. Pour ar/fa/zh, s'appuyer sur
`non-latin-verification-guide.md`.

**Langues :** FR (source) · EN · RU · PT · ES · AR · FA · ZH.

---

## 1. Setup langue

Pour chaque langue, basculer la culture du portail (DNN language selector ou `?language=xx-XX`).
Vérifier :

- [ ] Le sélecteur de langue existe et liste les 8 langues.
- [ ] Le changement de langue recharge la page dans la bonne culture (pas de cache FR résiduel).

## 2. Rendu par langue (tableau de signatures)

| Langue | Script | Sens | Police attendue | Red flag (fallback FR) |
|--------|--------|------|-----------------|------------------------|
| FR | Latin | LTR | défaut | (source — référence) |
| EN | Latin | LTR | défaut | texte qui reste en français |
| RU | Cyrillique | LTR | défaut (Cyrillique) | caractères latin à la place du cyrillique |
| PT | Latin | LTR | défaut | texte en français |
| ES | Latin | LTR | défaut | texte en français |
| AR | Arabe | **RTL** | Noto Naskh Arabic | sens LTR / glyphes manquants (tofu □) |
| FA | Persan | **RTL** | Vazirmatn | sens LTR / glyphes manquants |
| ZH | CJK | LTR | Noto Sans SC | glyphes manquants (tofu) / texte latin |

**Pour chaque langue, vérifier :**

- [ ] Aucun **glyphe manquant** (pas de □/tofu).
- [ ] Aucun **fallback FR** (texte français qui apparaît dans une page non-FR).
- [ ] Aucun **débordement** (texte coupé ou qui sort de son conteneur).
- [ ] **Sens de lecture correct** (RTL pour ar/fa — voir contrôle direction §3).

## 3. Features du site (par langue)

### 3.1 Fallacy Explorer (`_FallacyExplorer_Root.cshtml`)

- [ ] La liste des fallacies s'affiche.
- [ ] **⚠️ BUG CONNU (audit §4)** : le template **pin `text_en`/`desc_en`/`link_en`** quelle que soit
      la culture + hardcode le label `"find out more"` (EN). → Pour toute langue ≠ EN, l'Explorer
      affiche actuellement de l'anglais. **C'est un bug à corriger (Phase 2/4), pas un échec de
      traduction.** Le signaler ; ne pas bloquer la validation des autres features dessus.
- [ ] Une fois le fix appliqué : le libellé du lien = chaîne localisée (`ui.fallacy.find_out_more`,
      voir `non-latin-verification-guide.md`).

### 3.2 Rules Explorer — liste (`_RulesExplorer_RuleList.cshtml`)

- [ ] La plage de joueurs s'affiche : `de {0} à {1} joueurs` (FR) → traduit (voir guide).
- [ ] Les placeholders `{0}`/`{1}` sont **préservés** (nombres de joueurs insérés).

### 3.3 Rules Explorer — détail (`_RulesExplorer_RuleDetail.cshtml`)

Pour chaque règle, vérifier les 8 sections `res.*` (FR → traduit) :

- [ ] `res.RuleSummary` (Résumé)
- [ ] `res.RuleMaterial` (Matériel)
- [ ] `res.RuleInstallation` (Installation)
- [ ] `res.RuleVariants` (Variantes)
- [ ] `res.RuleMemoCard` (Carte mémo)
- [ ] `res.RuleMemoInstructions` — **⚠️ PAS DE SOURCE FR** (DB-only multi-sentence). Actuellement
      **non traduit** (7 cellules vides). Nécessite un export DB FR avant traduction (voir
      `2sxc-export-spec.md`).
- [ ] `res.RuleMemoCardFileNamePrefix` (Mémo)
- [ ] `res.RuleMemoCardDownload` (Télécharger la carte — bouton)

> **Note :** les 7 `res.*` (hors MemoInstructions) ont un **FR inféré** (issu du contexte `.cshtml`,
> pas du DB live). Leurs traductions dépendent de ce FR. **À vérifier vs export 2sxc avant de s'y
> fier** (c'est le motif du HOLD #490).

### 3.4 Navigation & meta

- [ ] Labels de navigation localisés (DB-only — nécessite export DNN tabs, voir spec §3).
- [ ] Titres/meta SEO localisés (DB-only — export page settings).

## 4. Contrôles transverses

- [ ] **Direction RTL** : pour ar/fa, `html[dir="rtl"]` (ou `dir="auto"` résout à RTL). Le layout
      miroir (sidebar, alignement) suit.
- [ ] **Polices RTL/CJK chargées** : Noto Naskh Arabic (ar), Vazirmatn (fa), Noto Sans SC (zh) —
      pas de fallback système qui déformerait les glyphes.
- [ ] **Encodage UTF-8** : caractères accentués/arabes/CJK intacts (pas de `?` ou mojibake).
- [ ] **Placeholders** : `{0}`/`{1}` préservés dans toutes les langues (cf. `ui.rules.players_range`).

## 5. Issue tracking

Tout écart → issue GitHub (label `dnn`/`i18n`) ou signalement dashboard. Distinguer :
- **Bug template** (ex. FallacyExplorer §4) → fix code Phase 2/4.
- **FR source faux** (inféré ≠ DB) → corriger CSV + re-run #457.
- **Traduction faible** → ajuster cellule ciblée.

---

*Checklist exhaustive mais non exhaustive des contenus DB-only (Glossary/FAQ/homepage) — ceux-ci
attendent l'export 2sxc (voir `2sxc-export-spec.md`).*
