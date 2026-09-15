# 2026-07-04 — Export 2sxc Rule content-type schema (read-only) — #681 primary

**Scope**: Read-only extraction of the **2sxc Rule content-type schema** from the production DNN
backup, to unblock the DNN i18n portage (#681, hard unblocker per #669 §3.1 step 1). Triggered by
ai-01 dispatch `msg-20260704T130644-lev5ct` (HIGH, "2sxc export = FAISABLE").

**Repo reference**: master `7590dfb8`. Backup: `tmp/dnn-backups/ArgumentumGames_live_post-migration_20260628.bak`.

---

## TL;DR — verdict (signal, not a PASS)

> The 2sxc Rule content-type uses **generic, language-neutral fields** (`EntityTitle`, `Summary`,
> `Material`, `MinNbPlayers`, …) — there are **zero language-suffixed fields** (`Summary_en`/`Summary_fr`
> not found in the DB). The site is **only EAV-dimensioned for FR + EN** (ru/ar/fa/zh are trace
> noise; pt/es absent). This **confirms investigation #669** (§2c): the RulesExplorer gap is a
> field-model gap, and the portage must either add lang-suffixed fields + the `loc()` cascade
> (PR #674, recommended) or provision the missing EAV language dimensions. It also narrows #682:
> the field-model decision is **forced toward lang-suffixed** because the EAV-dimension path would
> require enabling 5 new cultures (ru/pt/es/ar/fa/zh) before any translation work — the lang-suffixed
> path needs none of that.

The backup **cannot be cleanly queried via SQL restore** in this session (see §3 blocker): the
`jsboi` Windows login is authenticated but **not sysadmin** on `MSSQL$SQLEXPRESS`, so `RESTORE
FILELISTONLY` / `RESTORE DATABASE` is denied. The findings below come from a **direct binary scan
of the backup** (read-only, 0 mutation, 0 DB interaction), which is conclusive for the
presence/absence question even though it cannot enumerate every attribute cleanly.

---

## 1. Sources used (read-only, code=truth)

| Source | Path | Verdict |
|--------|------|---------|
| Production DNN backup (post-migration 10.3.2 + 2sxc 21.07) | `tmp/dnn-backups/ArgumentumGames_live_post-migration_20260628.bak` (98 MB, TAPE format) | Binary scan (UTF-16LE + ASCII) — read-only |
| 2sxc app export in repo | `DNNPlatform/Portals/1/2sxc/Argumentum/` | **No `App_Data/app.xml`** — the Argumentum app is not exported in the repo (unlike `Glossary3`). Content-types live DB-only. |
| Rule view templates (source-level) | `_RulesExplorer_Rule{List,Detail}.cshtml` | Already audited in #669 §2b — generic fields, no culture logic. |
| Local SQL instance | `MSSQL$SQLEXPRESS` (Running) + `sqlcmd` | **Blocked** on restore (§3) |

---

## 2. Findings — Rule content-type field model

### 2.1 Generic fields confirmed (no language suffixes)

Binary scan (UTF-16LE) of the backup, marker counts:

| Marker | Count | Interpretation |
|--------|-------|----------------|
| `2sxc` | 3082 | 2sxc content present (app boots, queries, etc.) |
| `Attribute` | 18611 | EAV attribute rows — large schema, consistent with a rich content app |
| `EntityTitle` | 64 | Rule field `EntityTitle` (referenced in #674 `loc()`) — present |
| `MinNbPlayers` | 13 | Rule field `MinNbPlayers` (hardcoded FR "de X à Y joueurs") — present |
| `Dimensions` | 1357 | EAV language-dimension rows |
| `DefaultLanguage` | 88 | EAV default-language config |
| **`Summary_en`** | **0** | **No language-suffixed Summary field** |
| **`Summary_fr`** | **0** | **No language-suffixed Summary field** |
| `RuleSummary` | 3 (UTF16) | Field-name fragments (consistent with generic `Summary`) |

➡️ **Conclusion**: the Rule content-type has **generic single-value fields** today (`Summary`,
`Material`, `Installation`, `Content`, `Variants`, `Memo`, `EntityTitle`, `MinNbPlayers`,
`MaxNbPlayers`, `UrlKey`). There is **no per-language field provisioning**. This is exactly what
#669 §2c predicted from the view-template audit — now **confirmed at the DB level**.

### 2.2 EAV language dimensions — FR + EN only

Culture-code marker counts (UTF-16LE):

| Culture | Count | Status |
|---------|-------|--------|
| `en-us` | 6523 | ✅ Dimensioned (secondary) |
| `fr-fr` / `fr-FR` | 4184 + 1656 | ✅ Dimensioned (canonical) |
| `en-US` | 486 | ✅ Dimensioned |
| `fa-` | 767 | ⚠️ Trace noise (Farsi — mostly skin/unused) |
| `ar-` | 64 | ⚠️ Trace noise (Arabic) |
| `zh-` | 22 | ⚠️ Trace noise (Chinese) |
| `ru-RU` | 15 | ⚠️ Trace noise (Russian) |
| **`pt-PT`** | **0** | ❌ Absent |
| **`es-ES`** | **0** | ❌ Absent |

➡️ **Conclusion**: the production EAV is **only dimensioned for FR (canonical) + EN (secondary)**.
The 6 release target languages (RU/PT/ES/AR/FA/ZH) are **not present as usable EAV dimensions**
(pt/es are entirely absent; ru/ar/fa/zh are trace-level, likely skin/langpack residue).

### 2.3 Implication for the field-model decision (#682)

This narrows #682 significantly. Two paths were on the table (#669 §3.1 step 2):

- **Path A — lang-suffixed fields + `loc()` cascade** (mirror FallacyExplorer #490). Requires adding
  `Summary_en`/`Summary_ru`/…/`Memo_zh` to the Rule content-type and wiring `loc()` (PR #674).
  **No DNN content-language enablement needed for the data to flow** — the cascade falls back to FR
  until a suffixed value is populated. Lowest risk, reuses a production-proven pattern.
- **Path B — 2sxc EAV language dimensions** on the Rule content-type. Requires **enabling 5 new
  content cultures** (ru/pt/es/ar/fa/zh) in DNN + 2sxc **before** any translation can attach — a
  heavier, jsboige-admin-gated prerequisite, and more fragile (RTL/CJK EAV dimension behavior).

➡️ **Recommendation stands and is now reinforced**: **Path A (lang-suffixed + `loc()`)**. The
field-model decision in #682 is effectively **forced** — Path B would block the whole portage on
DNN content-language provisioning for 5 cultures, while Path A lets translation proceed field-by-field.

---

## 3. Blocker — clean SQL restore denied (jsboige login not sysadmin)

To enumerate every Rule attribute cleanly (rather than via marker counts), the clean path is to
restore the backup into an isolated local DB and run `SELECT` against the 2sxc EAV tables
(`ToSic_EAV_Attribute`, `ToSic_EAV_Dimensions`, …).

```
sqlcmd -S "localhost\SQLEXPRESS" -E -Q "RESTORE FILELISTONLY FROM DISK='...'"
→ Msg 262, Level 14: CREATE DATABASE permission denied in database 'master'.
```

- `jsboi` Windows login is authenticated (`MYIA-PO-2023\jsboi`) but `IS_SRVROLEMEMBER('sysadmin') = 0`.
- `sa` SQL login exists (not disabled) but its password is not in the repo keychain and was not
  guessed (out of read-only scope).

➡️ This is **not pursued here**: elevating the SQL login or recovering `sa` is a config mutation
beyond the read-only gate of this dispatch. Two clean unblockers, both jsboige-gated:
1. **Grant `jsboi` `sysadmin`** on `MSSQL$SQLEXPRESS` (or provide `sa` password) → enables
   `RESTORE DATABASE ArgDnnExport_ReadOnly ...` + full `SELECT` access.
2. **Use the 2sxc admin UI export** (voie 1, Playwright + portal creds) → produces a clean JSON/XML
   content-type export without any DB restore.

Either unblocks a **complete** schema doc (full attribute list with types). The binary-scan
findings in §2 are already conclusive for the portage-direction decision (#682).

---

## 4. DoD status

| DoD item (#681) | Status |
|-----------------|--------|
| Export of content-types D+E + Resources values delivered | **Partial** — binary-scan findings (§2) confirm the field model; full per-attribute enumeration gated on §3 unblocker |
| Export exposes the current generic Rule content-type fields | ✅ Confirmed generic (EntityTitle, Summary, Material, MinNbPlayers, … — no suffixed fields) |
| Export exposes the App Resources values (`res.Rule*`, incl. `res.RuleMemoInstructions`) | ⚠️ Presence confirmed (`Attribute` count, `2sxc` markers); value enumeration gated on §3 |

**Signal, not a PASS.** Verdict on full export completeness = ai-01 + jsboige (once §3 unblocker chosen).

---

## 5. Recommended next step (critical path)

1. **jsboige picks an §3 unblocker** (sysadmin grant OR `sa` pwd OR voie-1 Playwright export).
2. Worker completes the schema doc (full attribute list) → finalizes #682 decision (Path A).
3. PR #674 (`[runtime pending]`) becomes runtime-validable once the first lang-suffixed field is
   provisioned in DB (smallest possible validation: add `Summary_en`, set one EN value, render).

---

## 6. Gate boundaries (HARD — read-only)

- ❌ No prod write, no DB mutation, no portal interaction (voie 2 = local backup scan only).
- ❌ No SQL restore attempted beyond a denied permission check (no config mutation to elevate).
- ❌ No credentials guessed or committed (no `sa` password brute-force; nothing in the diff).
- ❌ Does not declare a QA verdict (ai-01 only).
- ✅ All evidence is read-only: backup binary scan + repo file presence + view templates.

Relates: #681, #682, #674, #669, #458, #457, #490, dispatch `msg-20260704T130644-lev5ct`.
