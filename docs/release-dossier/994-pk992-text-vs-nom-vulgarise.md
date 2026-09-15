# PK 992 — `text_fr` vs `nom_vulgarisé` convention constat

**Issue:** [#994 — editorial / semantic arbitration (Thomas)](https://github.com/ArgumentumGames/Argumentum/issues/994) (PK 1024, and now PK 992)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-08-11
**Base:** master `2db33902`
**Dispatch:** ai-01 `mbs53t` [primaire]
**Status:** **CONSTAT (docs-only), decision = Thomas / jsboige.** This document measures the `text_fr` / `nom_vulgarisé` convention across the 1408 taxonomy nodes and characterises PK 992 against it. It does **not** decide the swap — that is a semantic arbitration. Measured, not deduced from examples.

> **Headline.** (1) `nom_vulgarisé` is a **rare override** filled on only **40/1408** nodes (2.8 %), not a field parallel to `text_fr`. (2) It is **rendered nowhere** in the production artefacts (every Fallacies card template uses `{{text_fr}}`, none uses `{{nom_vulgarisé}}`; the mindmap title expression is `{item.TextFr}`; OWL uses `TextEn`/`TextFr`; the entity alias `Title => NomVulgarisé` has no production consumer). The editorial note is therefore **dormant**, not a live rendering defect. (3) Among the 40 populated nodes, the dominant convention is `nom_vulgarisé` = popular/colloquial form, `text_fr` = technical/formal term — and PK 992 is **inverted** relative to it. (4) PK 992 is **not isolated as a residue**: PK 41 carries a second editorial brainstorm note in the same column (outside the printed deck). A signalled defect is a probe, not a perimeter.

---

## 1. Method (triple grounding + an instrument self-correction)

- **Technical (code = truth):** `Fallacy.cs:18-19` entity aliases (`Title => NomVulgarisé`, `Text => TextFr`); `FallacyClassMap` CSV indices; grep of `nom_vulgarisé`/`NomVulgarisé`/`.Title` consumers; a Python field-segment splitter (reuses the #753/#1058 idiom — respects doubled-quote escapes + bare LF inside quoted cells; the file has 144 bare LFs in multi-line cells) over `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` on master `2db33902`.
- **Conversational:** ai-01 dispatch `mbs53t` (the PK 992 flag) + `kkieqj` (the inverse-control lesson that caught a false-positive series on the mindmap).
- **Semantic:** `codebase_search` confirmed the CardPen template surface.

**Instrument self-correction (before publishing).** My first measure printed `desc_fr = '26'` for PK 992 — which contradicted ai-01's `desc_fr = "Vous évitez de prendre une position claire…"`. Cause: I had indexed `desc_fr` at col **14**, but col 14 is **`LTfr`** (a length counter = 26); `desc_fr` is col **15** (there is an `LTfr` length column between `text_fr` and `desc_fr`). ai-01 was right. Re-ran with the corrected index; all length/convention stats below use `nom_vulgarisé`@11 and `text_fr`@13, which were always correct. (This is the same "accuse the instrument before the data" reflex ai-01 documented in `kkieqj` — applied here to my own script.)

## 2. Reframe — `nom_vulgarisé` is a rare override, not a parallel field

| field | fill rate (1408 nodes) |
|---|---|
| `text_fr` | **1408 / 1408** (100 %) |
| `nom_vulgarisé` | **40 / 1408** (2.8 %) |
| both populated | **40** (the comparable set) |

`text_fr` is the universal primary name. `nom_vulgarisé` is an **exception override** present on 40 nodes only. The editor's question — "should `text_fr` and `nom_vulgarisé` be swapped?" — therefore applies to **40 nodes**, not 1408: on the other 1368, `text_fr` is the only name and there is nothing to swap. This reframes ai-01's "convention sur l'ensemble des 1408 nœuds" — the convention lives in the 40.

## 3. The convention among the 40 populated nodes

**Length classification** (on the 40 both-populated nodes, delta > 3 chars):

| class | count | reading |
|---|---|---|
| `similar` (within 3 chars) | 20 | near-synonyms / singular-plural / identical |
| `text_fr` longer | 14 | `text_fr` carries the longer/explanatory form |
| `nom_vulgarisé` longer | 6 | `nom_vulgarisé` carries the longer form |

**Semantic pattern** (reading the pairs, not just lengths). When the two differ in register, the **dominant** convention is:

> **`nom_vulgarisé` = the popular / colloquial / idiomatic form · `text_fr` = the technical / formal term.**

Representative pairs (the convention at its clearest):

| PK | `nom_vulgarisé` (popular) | `text_fr` (formal) |
|---|---|---|
| 943 | Citation hors contexte | Décontextualisation |
| 956 | Plaider l'exception | Plaidoirie spéciale |
| 1362 | Traiter d'hypocrite | Tu quoque |
| 1365 | Caricature | Homme de paille |
| 1373 | Point Godwin | Reductio ad Hitlerum |
| 814 | Pensée binaire | Faux dilemme |
| 74 | Parce que c'est comme ça ! | Argument d'autorité personnelle |

(The `similar` set is near-synonyms or singular/plural — e.g. PK 1024 "Biais naturel"/"Biais naturels", PK 855 "Équivoque"/"Équivoque" — where the popular/formal distinction does not apply. The convention is a tendency, not a hard rule, on a 40-node set.)

## 4. PK 992 against the convention

```
PK 992 · path 6.2.2 · Famille "Tricherie" · Sous-Famille "Changement de cap"
        Sous-Sous-Famille "Beurre et argent du beurre" · carte=2 · niveau=7

nom_vulgarisé (63) : "Versatilité: ON INVERSERAIT PAS LE TEXT_FR eT LE NOM VULGARISé?"
                     ─────────────────  ──────────────────────────────────────────────
                     intended value      editorial question (the residue)
text_fr       (39) : "Vouloir le beurre et l'argent du beurre"   (a proverb / popular)
desc_fr           : "Vous évitez de prendre une position claire afin de ne pas pouvoir
                     être mis en défaut."   (definition of versatilité)
```

**Inversion.** Stripping the editorial note, the intended `nom_vulgarisé` = "Versatilité" (a **formal/technical** abstract noun) and `text_fr` = a **popular proverb**. This is the **opposite** of the §3 convention (`nom_vulgarisé` = popular). **The editor's flag is founded.** PK 992 is the **only** node among the 40 where `nom_vulgarisé` is unambiguously a formal term and `text_fr` a popular proverb.

**But there is a counter-argument the decision must weigh.** PK 992 is the **parent** of the "Beurre et argent du beurre" sub-sub-family — its `text_fr` (the proverb) **names the whole cluster** (PK 993–1003, 1407 all sit under that sub-sub-family, with colloquial `text_fr` like "Ménager la chèvre et le chou", "Langue de bois"). So the proverb in `text_fr` is the cluster's canonical name; "Versatilité" is the formal alias. Swapping would put a formal term as the parent's primary name while the cluster is named by the proverb. This is why it is an arbitration, not a fix.

## 5. Is PK 992 isolated? — a second editorial residue (probe ≠ perimeter)

A systematic scan of `nom_vulgarisé` for editorial contamination (all-caps shouty text, `?`, editorial verbs) finds **two** genuine residues, not one:

| PK | `carte` | in printed deck? | `nom_vulgarisé` content |
|---|---|---|---|
| **992** | `2` | **YES** (90 `carte=2` nodes total) | `"Versatilité: ON INVERSERAIT PAS LE TEXT_FR eT LE NOM VULGARISé?"` (the editor's swap question) |
| **41** | *(empty)* | no | `"Je n'aime aucun des titres. Chercher vers Clairvoyance subjective, discernement infus, y a pas un perso grec qui a le jugement parfait?"` (a 136-char brainstorm note — no term at all) |

(PK 74 also matched the heuristic on its `!`, but "Parce que c'est comme ça !" is a **legitimate** colloquial fallacy name — false positive, kept.)

ai-01 measured "1 seul cas sur 52 colonnes rendues". That count is correct **for the printed deck** (PK 41 is `carte` empty). But the column itself carries **2** editorial residues — PK 41 is dormant data-hygiene debt in the same column. **A signalled defect is a probe, not a perimeter**: scanning the whole column found a sibling.

## 6. Rendering — the note is dormant, the swap has no visual effect today

Where is `nom_vulgarisé` consumed?

| consumer | uses `nom_vulgarisé`? | proof |
|---|---|---|
| Fallacies card templates (Face 1/2/3, Web) | **no** — all use `{{text_fr}}` | `Argumentum_Fallacies_Face{,_2,_3,_Web}_fr.json` mustache: `{{text_fr}}`=YES, `{{nom_vulgarisé}}`=**no** (all 4) |
| Mindmap node title | **no** — uses `{item.TextFr}` | `FallacyMindMapDocumentConfig.cs:41` `DefaultTitleExpression = "{item.TextFr}"` |
| OWL ontology | **no** | `OwlGeneratorConfig.cs:317-318` prefLabel = `TextFr`/`TextEn`; `Ontology/` has **0** occurrence of `Family` or `nom_vulgarisé` |
| Entity alias `Title => NomVulgarisé` | **orphan** — no production consumer | grep of `.Title` returns only test assertions (`c.Title.Should().Be(...)`) |

**Consequence:** the editorial note in `nom_vulgarisé` is **not rendered on any current artefact**. It would only surface if a template switched to `{{nom_vulgarisé}}` or the `Title` alias gained a consumer. The residue is data-hygiene debt, not a live card defect. **Corollary for the swap decision:** since only `text_fr` renders, swapping `text_fr` ↔ `nom_vulgarisé` would change the **card's displayed name** (proverb → "Versatilité" under Option B) but the note itself never appears either way.

## 7. The two options (decision = Thomas / jsboige)

| | Option A — keep | Option B — swap |
|---|---|---|
| `text_fr` (rendered) | proverb "Vouloir le beurre…" (current; names the cluster) | "Versatilité" (formal term) |
| `nom_vulgarisé` (dormant) | "Versatilité" (cleaned of note) | proverb "Vouloir le beurre…" |
| matches §3 convention? | no (inverted) | yes |
| matches cluster-naming? | yes (proverb names the sub-sub-family) | no (formal term as parent name) |
| visible on card | proverb (unchanged) | "Versatilité" (card title changes) |

Either way, the **editorial note must be removed** from `nom_vulgarisé` (and PK 41 cleaned) — that is independent of the swap.

## 8. Where PK 992 appears

- **Printed deck:** `carte=2` → yes (90 `carte=2` "new card" nodes total). It renders on the Fallacies Tarot/Web PDFs **via `text_fr`** = the proverb (Option A, current behaviour).
- **Mindmap:** node title = `{item.TextFr}` = the proverb.
- **OWL:** concept prefLabel (FR) = `TextFr` = the proverb; hierarchy via `path` 6.2.2.

## 9. Scope / gel

- **0 CSV mutation · 0 code change · 0 artefact regen · 0 CardPen · 0 dependabot.** This is a constat (docs-only). The CSV edit (note removal + optional swap) is **gated on Thomas/jsboige's semantic decision** — a separate PR after arbitration, class #994.
- 1 net-new doc + the measurement tool (committed as the reproducible proof, as #753/#1058 did). Branch + PR, no direct push to master.

## Sources

- `Generation/Converters/Argumentum.AssetConverter/Entities/Fallacy.cs:15-19,222-223` (Title/Text aliases; CSV map)
- `Generation/Converters/Argumentum.AssetConverter/Mindmapper/FallacyMindMapDocumentConfig.cs:41` (`DefaultTitleExpression = "{item.TextFr}"`)
- `Generation/Converters/Argumentum.AssetConverter/Ontology/OwlGeneratorConfig.cs:317-318` (prefLabel = TextFr/TextEn)
- `Cards/Fallacies/Argumentum_Fallacies_Face{,_2,_3,_Web}_fr.json` mustache (4 templates: `{{text_fr}}` yes, `{{nom_vulgarisé}}` no)
- `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` on master `2db33902` (40/1408 `nom_vulgarisé` populated; PK 992 + PK 41 residues)
- `tools/1060-pk992-convention-measure.py` (the reproducible measure)

🤖 po-2024 — ai-01 dispatch `mbs53t` [primaire], base `2db33902`, docs-only
