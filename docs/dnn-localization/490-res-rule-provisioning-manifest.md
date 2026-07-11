# 2026-07-11 — #490 `res.Rule*` provisioning manifest (Δ1/Δ2 DB writes, gated ops)

**Scope**: the 2 DB-only deltas from the [reconciliation](490-res-rule-reconciliation.md) that
**jsboige arbitrated in interactive session** (2026-07-11 ~01:15, consigned on #458). These are
**2sxc App-Resource value fixes on eid=10340** (app=60, FR dimensionless default) — they live in the
DB, **not** the repo, so they cannot be PR'd. This manifest is the **prep artifact** the prod operator
(jsboige / ops) applies via the 2sxc admin or a Method B write script on the live DNN instance.

**Repo reference**: branch `fix/490-res-rule-deltas`, base master `daa71763`. Issue: #490. Owner:
jsboige (DB write). Triggered by ai-01 dispatch `70iit0` (2026-07-11, 4 arbitrages tranchés).

> ⚠ **Status = manifest only.** No DB write is executed from this repo. The 2 operations below are
> **staged for prod apply** (gated jsboige). The repo-side deltas (Δ3 CSV + Δ4 view) ship in the
> companion PR on this same branch — they are independent of these DB writes.

---

## The 2 DB writes (eid=10340, FR dimensionless default)

Both target the **App-Resource entity eid=10340** (app=60), the `res.*` rail consumed by
`_RulesExplorer_RuleDetail.cshtml` via `@Resources.X`. The FR value is the **dimensionless default**
(the value 2sxc returns when no culture-specific value is attached).

### Δ1 — `res.RuleMaterial`: fix FR typo (missing accent)

| | |
|---|---|
| **Resource key** | `RuleMaterial` |
| **Entity** | eid=10340 (app=60) |
| **Dimension** | FR dimensionless default |
| **Current DB value** | `Materiel` (**typo** — no accent) |
| **Target DB value** | `Matériel` (correct French) |
| **Rationale** | jsboige arbitration 2026-07-11: the rendered `<h2>` currently shows "Materiel" (wrong). CSV already correct (`Matériel`), so no repo change — DB-only fix. |
| **Blast radius** | **None.** The 7 non-FR translations (`Materials`/`Материалы`/`Material`/`Material`/`المواد`/`محتویات بازی`/`材料`) were translated from the *correct* FR and remain valid. |

### Δ2 — `res.RuleMemoCard`: fix FR case (title → sentence)

| | |
|---|---|
| **Resource key** | `RuleMemoCard` |
| **Entity** | eid=10340 (app=60) |
| **Dimension** | FR dimensionless default |
| **Current DB value** | `Carte Mémo` (title case — capital `M`) |
| **Target DB value** | `Carte mémo` (sentence case — lowercase `m`) |
| **Rationale** | jsboige arbitration 2026-07-11: sentence case is more conventional for FR headings. CSV keeps `Carte mémo` as canonical (unchanged). |
| **Blast radius** | **None.** The 7 non-FR translations (`Memo card`/`Памятка`/`Cartão de resumo`/`Tarjeta de resumen`/`بطاقة تذكيرية`/`کارت یادآوری`/`备忘卡`) are case-insensitive equivalents and remain valid. |

---

## Apply procedure (jsboige / ops, gated)

Two equivalent paths — pick per ops preference:

### Path A — 2sxc admin UI (lowest risk, visual)
1. Log into the DNN instance as host/admin.
2. Open the 2sxc app 60 (Argumentum) → App Resources (entity eid=10340).
3. For `RuleMaterial` (FR default): edit value `Materiel` → `Matériel`. Save.
4. For `RuleMemoCard` (FR default): edit value `Carte Mémo` → `Carte mémo`. Save.

### Path B — Method B write script (on-box, gated)
A SQL/PowerShell write against the live DNN DB, run from `myia-web1` (the same box that delivered
export #681 via Method B read-only). **Requires write credentials** (the export was read-only SELECT).

```sql
-- Δ1: RuleMaterial FR typo fix (eid=10340, FR dimensionless default = no DimensionID / dimension 0)
UPDATE [dbo].[ToSIC_EAV_Values]
SET [Value] = N'Matériel'
WHERE [AttributeId] = (SELECT [AttributeId] FROM [dbo].[ToSIC_EAV_Attributes]
                       WHERE [StaticName] = 'RuleMaterial')
  AND [EntityId] = 10340
  AND [DimensionID] IS NULL;  -- FR dimensionless default

-- Δ2: RuleMemoCard FR case fix (eid=10340, FR dimensionless default)
UPDATE [dbo].[ToSIC_EAV_Values]
SET [Value] = N'Carte mémo'
WHERE [AttributeId] = (SELECT [AttributeId] FROM [dbo].[ToSIC_EAV_Attributes]
                       WHERE [StaticName] = 'RuleMemoCard')
  AND [EntityId] = 10340
  AND [DimensionID] IS NULL;  -- FR dimensionless default
```

> ⚠ **The SQL above is a draft from the EAV schema model — verify column/table names against the live
> 2sxc v21 schema before executing.** The 2sxc EAV table names (`ToSIC_EAV_Values`,
> `ToSIC_EAV_Attributes`) and the FR-dimensionless convention (`DimensionID IS NULL`) match the export
> #681 shape; confirm on-box. Path A (2sxc admin UI) avoids this risk entirely.

### Post-apply verification
After either path, re-export eid=10340 (Method B read-only) and confirm:
- `RuleMaterial` FR default = `Matériel` (accent present)
- `RuleMemoCard` FR default = `Carte mémo` (lowercase `m`)

The re-export diff vs #681 should show **exactly these 2 FR cells changed**, nothing else.

---

## Gate boundaries (HARD)

- ❌ **Zero DB write from this repo** — the manifest is a staging artifact; apply is gated jsboige/ops.
- ❌ Does not modify `Cards/` CSV (game-content is po-2024's lane).
- ❌ Does not provision the 5 non-FR/non-EN culture dimensions on eid=10340 (that is #682 Path A,
  separate lane) — Δ1/Δ2 fix the **FR dimensionless default only**, which already exists.
- ❌ Does not declare a QA verdict — that's ai-01.

## What is NOT here (companion repo PR, this branch)

The 2 **repo-side** deltas (Δ3 `RuleMemoCardFileNamePrefix` CSV fix + Δ4 `RuleContent` view+CSV) ship
in the companion commits on this same branch — they are independent of these DB writes and land via
normal PR review. See the branch diff for `dnn-ui-strings.csv` (Δ3 row + Δ4 new row) and
`_RulesExplorer_RuleDetail.cshtml:47` (Δ4 heading wire).

Relates: dispatch `70iit0`, #490 (res.* rail), #681 (export), #458 (epic), #682 (dimension
provisioning — separate lane), #669 (mechanism), reconciliation doc
[490-res-rule-reconciliation.md](490-res-rule-reconciliation.md).
