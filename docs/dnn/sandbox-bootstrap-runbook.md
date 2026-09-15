# DNN Sandbox Bootstrap Runbook — DNN 9.11.1 + 2sxc 21 (.NET Framework 4.8)

**Date**: 2026-06-26 · **Author**: po-2023 (dispatched by ai-01 v3, idle track)
**Status**: Reproducible ops runbook (capitalizes the 2026-06-25 boot-attempt characterization). Read-only doc, no code.
**Purpose**: Bootstrap a local DNN 9.11.1 + 2sxc 21.07 sandbox for runtime validation (e.g. the 12 Razor14 templates of #596) without touching production.
**Related**: #131 (DNN upgrade arc), #596 (Razor14 migration, runtime-pending), [UPGRADE-ASSESSMENT.md](UPGRADE-ASSESSMENT.md) §1 (verified bin/ state), [feedback-dnn-iis-express](../../) (IIS Express `/config:` gotcha).

---

> ## ⛔ §2 / §3 SUPERSEDED (2026-07-10) — the B1 "revert to 6.0.0.0" thesis is INVERTED for 2sxc-21
>
> This runbook was written 2026-06-26 from the 2026-06-25 boot-attempt characterization. The §2 root-cause
> framing (".NET 9 SDK **contamination**") and the §3 recipe (fetch NuGet **6.0.0**, align redirects to
> **6.0.0.0**) pre-date the B1-inversion being understood. For a **2sxc 21.07** site that diagnosis is
> **backwards and harmful**:
>
> - 2sxc 21.07 is **compiled against .NET 9** and **ships** the .NET 9 BCL stack in its own Install pkg. It
>   **REQUIRES** `System.Text.Json` 9.0.0.0, `System.Collections.Immutable` 9.0.0.0,
>   `Microsoft.Bcl.AsyncInterfaces` 8.0.0.0, etc.
> - Reverting them to 6.0.0.0 breaks 2sxc's `JsonOptions` type-init (`MissingMethodException`) →
>   `StartupDnnWebApi.Configure()` aborts **before** `SetConnectionString` → every 2sxc module renders
>   *"Something went really wrong in view.ascx"* (mimics a connection-string bug; isn't one).
>
> **What was actually done (2026-06-28):** deployed the matched BCL stack from the **2sxc 21.07 Install
> package** and aligned redirects **to 9.0.0.0** (not 6.0.0.0). Result: homepage 200/86 KB, 0
> JsonOptions / conn-string errors. Canonical snapshot: `tmp/dnn-backups/bin_post_2sxc_realign` (330 files).
> Authoritative version matrix: `reference-dnn-2sxc-net48-bcl-stack` (per-machine memory) +
> [README §0.5](../dnn-localization/README.md). The executable form of the (also-superseded) 6.0.0 recipe —
> [`repair-bin-net48.ps1`](repair-bin-net48.ps1) — was deprecated in **#624** (its `-Apply` now refuses).
>
> **Still valid here** (verified, reusable): §1 (LocalDB / IIS Express / DB-163-tables facts), §3.1
> (Imageflow local source — but at the 9.x versions, not 6.x), §3.3 binding-redirect **line numbers**
> (~446/454/596/600 — the `newVersion` *targets* are wrong, the line *locations* are right), §4 (web.config
> local edits), §6 gotchas (IIS Express `/config:`), §7 (why redirect-only patching fails).
> **Stale / do-not-execute:** §2 root-cause framing, §3.2 NuGet 6.0.0 table, §3.3 `newVersion → 6.0.0.0`.

## TL;DR

The sandbox **boot infrastructure is proven** on po-2023 (LocalDB + IIS Express + DB present). DNN itself **cannot start** due to a **pre-existing `bin/` SDK-assembly contamination**: ~5 contract assemblies shipped at `.NET 9` versions (asmVer `9.0.0.0`) into a site that runs **EF Core 2.1.1** (netstandard2.0) on **.NET Framework 4.8**. EF Core 2.1.1 cannot bind `.NET 9` assemblies → HTTP 500 cascade. The fix is a **bounded clean `bin/` re-deploy** (replace ~8 net48-compatible assemblies) — an ops task, **not a config tweak**. This runbook gives the exact recipe.

> ⚠️ **Local-only edits.** `DNNPlatform/bin/` and `web.config` are **git-tracked**. All edits in this runbook are **local, reverted after** — never commit a machineKey, connection string, or `bin/` binary.

---

## 1. Verified current state (po-2023, 2026-06-25)

| Component | State | Source |
|---|---|---|
| **LocalDB `MSSQLLocalDB`** | ✅ present, starts clean | `sqllocaldb start MSSQLLocalDB` |
| **DB `ArgumentumGames`** | ✅ real DNN 9.11.1 (163 tables, `Version`=9.11.1, full `aspnet_*`) | `sqlcmd` |
| **IIS Express** | ✅ x64 + x86 present | `C:\Program Files\IIS Express\` |
| **IIS site "DNN Argumentum"** | ✅ pre-configured (`:8090`, Clr4IntegratedAppPool v4.0, physicalPath `DNNPlatform`) | `Documents/IISExpress/config/applicationhost.config` |
| **.NET Framework 4.8 runtime** | ✅ present | `Windows/Microsoft.NET/Framework64/v4.0.30319/` |
| **`DNNPlatform/bin/`** | ❌ **CONTAMINATED** — see §2 | git-tracked |
| **`web.config`** | ❌ placeholders (`Data Source=REPLACE`, `machineKey=REPLACE`) | git-tracked |

## 2. The blocker — `bin/` SDK-assembly contamination (characterized)

DNN returns **HTTP 500**, FR-locale title:
> *« Impossible de charger le fichier ou l'assembly '<System.X>' … La définition trouvée du manifeste de l'assembly ne correspond pas à la référence de l'assembly. (Exception de HRESULT : 0x80131040) »*

The failing assembly **advances with each binding-redirect patch** = a cascade, not a single fix.

### Root cause (verified 2026-06-26)

**EF Core = 2.1.1.0** (`Microsoft.EntityFrameworkCore*.dll`, netstandard2.0 — old, NOT .NET 9). But `bin/` root contains contract assemblies at **.NET 9 SDK versions** that EF Core 2.1.1 / DNN net48 **cannot bind**:

| Assembly (physical `bin/`) | asmVer | fileVer (= SDK marker) | EF Core 2.1.1 needs |
|---|---|---|---|
| `System.Collections.Immutable` | **9.0.0.0** | 9.0.24.52809 | ~1.5/5.0/6.0 |
| `System.Text.Json` | **9.0.0.0** | 9.0.24.52809 | ~4.7/6.0 |
| `System.IO.Pipelines` | **9.0.0.0** | 9.0.24.52809 | ~4.x/6.0 |
| `System.Diagnostics.DiagnosticSource` | **9.0.0.11** | 9.0.1125.51716 | ~4.x/6.0 |
| `System.Text.Encodings.Web` | **9.0.0.0** | 9.0.24.52809 | ~4.7/6.0 |
| `System.Buffers` | 4.0.4.0 | **4.600.24.56208** (.NET 6 SDK) | 4.0.3.0 |
| `System.Memory` | 4.0.1.2 | 4.6.31308.01 | 4.0.1.1 |
| `System.Numerics.Vectors` | 4.1.4.0 | 4.6.26515.06 | 4.1.3.0/4.1.4.0 |
| `System.Runtime.CompilerServices.Unsafe` | 6.0.0.0 | 6.0.21.52210 | 4.0.4.1/4.5.x |
| `System.Threading.Tasks.Extensions` | 4.2.0.1 | 4.6.28619.01 | 4.2.0/4.5.x |

The fileVersions `4.600.x` / `9.0.x` are **.NET 6/9 SDK forwarder/contract assemblies** — dropped by a global-tool / SDK NuGet restore polluting `bin/`. A DNN 9.11.1 site on .NET Framework 4.8 cannot bind them.

### Partial clean source exists — but insufficient

`bin/Imageflow/` carries the **correct** older versions for 2 of these (the `codeBase`-commented redirects pointed here):

| `bin/Imageflow/` assembly | asmVer (correct) |
|---|---|
| `System.Buffers.dll` | **4.0.3.0** ✅ |
| `System.Memory.dll` | **4.0.1.1** ✅ |
| `Microsoft.Extensions.*` (7 dlls) | netstandard2.0 ✅ |

**But none of the 5 `.NET 9` contaminants has a clean local source** in the repo. Resolving them requires fetching net48-compatible NuGet packages (see §3).

## 3. Clean `bin/` re-deploy recipe (the remaining ops work)

> This is a **bounded, reproducible ops task** (~1 focused tick). It is NOT whack-a-mole redirect patching — the versions are known.

### 3.1 Replace from the local clean source (free)

```powershell
# Backup first (local — never commit bin/)
Copy-Item DNNPlatform\bin DNNPlatform\bin.contaminated.bak -Recurse
# Buffers + Memory: use the correct bin/Imageflow/ versions
Copy-Item DNNPlatform\bin\Imageflow\System.Buffers.dll DNNPlatform\bin\System.Buffers.dll -Force
Copy-Item DNNPlatform\bin\Imageflow\System.Memory.dll DNNPlatform\bin\System.Memory.dll -Force
```

### 3.2 Fetch the 5 .NET 9 contaminants at net48-compatible versions (6.0.x)

Pull these NuGet packages and extract the `lib/net462` (or `lib/netstandard2.0`) assemblies into `DNNPlatform/bin/`:

| Package | Version | Provides |
|---|---|---|
| `System.Collections.Immutable` | **6.0.0** | `System.Collections.Immutable.dll` (asm 6.0.0.0) |
| `System.Text.Json` | **6.0.0** | `System.Text.Json.dll` (asm 6.0.0.0) |
| `System.IO.Pipelines` | **6.0.0** | `System.IO.Pipelines.dll` (asm 6.0.0.0) |
| `System.Diagnostics.DiagnosticSource` | **6.0.0** | `System.Diagnostics.DiagnosticSource.dll` (asm 6.0.0.0) |
| `System.Text.Encodings.Web` | **6.0.0** | `System.Text.Encodings.Web.dll` (asm 6.0.0.0) |

Why 6.0: last line shipping `lib/net462` (net48-compatible), predates the `.NET 8+ TFM-only` cliff. These satisfy EF Core 2.1.1 + the DNN binding redirects.

> Verify after extraction: `Get-Item bin\System.Collections.Immutable.dll` → asmVer `6.0.0.0` (not 9.0.0.0).

### 3.3 Align the binding redirects (web.config, local edit)

The active (uncommented) redirects at `web.config` lines ~446/454/596/600 use `oldVersion="0.0.0.0-32767...newVersion=<X>"`. Set `newVersion` to the **6.0.0.0** (or the matching physical asmVer) for the 5 replaced assemblies + `System.Buffers`→`4.0.3.0`, `System.Memory`→`4.0.1.1`. The existing `0.0.0.0-32767` oldVersion range already covers any requestor version.

### 3.4 Boot + probe

```powershell
sqllocaldb start MSSQLLocalDB
# web.config local edits (see §4)
& "C:\Program Files\IIS Express\iisexpress.exe" /config:"...\applicationhost.config" /site:"DNN Argumentum"
# probe (PS7 SkipHttpErrorCheck captures 500 bodies):
Invoke-WebRequest http://localhost:8090/ -UseBasicParsing -SkipHttpErrorCheck
```

Expectation: HTTP 200 with DNN home page (not the `0x80131040` FileLoadException). If a **new** assembly mismatch surfaces, repeat §3.2-3.3 for it (bounded set).

## 4. web.config local edits (revert after — never commit)

```xml
<!-- connectionString: line ~40 -->
<add name="SiteSqlServer"
     connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ArgumentumGames;Integrated Security=True;"
     providerName="System.Data.SqlClient" />

<!-- machineKey: line ~151 — THROWAWAY DEV key (prod machineKey is GDrive-authoritative) -->
<!-- Generate: [byte[]] (1..32 | %{ Get-Random -Max 256 }) → hex64 for decryption (AES-256) -->
<machineKey validationKey="<throwaway-64+hex>" decryptionKey="<throwaway-64hex-AES>"
            decryption="AES" validation="HMACSHA256" />
```

**Revert**: `git checkout -- DNNPlatform/web.config` (and `DNNPlatform/bin` from the backup or `git checkout`) after the validation session. Verify `git status` shows only `?? tmp/`.

## 5. Runtime validation (the DoD for #596)

Once DNN boots (§3-4), to validate the 12 Razor14 templates:

1. **Assign** each migrated template (`Portals/1/2sxc/{News5/bs3, Content/bs3/{Link,Layout}}/`) to a 2sxc App module on a DNN page in `ArgumentumGames` (DB rows control page/module wiring — verify which pages exist).
2. **Browser-navigate** each page → screenshot the render.
3. **Record OK/KO** per template. KO = capture the 2sxc runtime error (yellow-screen).
4. Post the OK/KO report on #596 → that **un-gates the merge**.

> po-2023 signals the runtime report (factual OK/KO); the visual PASS verdict is ai-01/jsboige's.

---

## 6. Known gotchas (applied)

- **IIS Express `/path:` is broken** on this machine (404s silently) — **must** use `/config:` + the named "DNN Argumentum" site. ([feedback-dnn-iis-express])
- **Do NOT whack-a-mole** binding redirects without replacing the physical assemblies — patching `System.Buffers`→4.0.4.0 just advanced the cascade to `System.Collections.Immutable`. Replace the DLLs (§3.2), then align redirects (§3.3).
- **One `dotnet run` / IIS site at a time** for any MindMap-related work (unrelated here, but the machine shares the desktop).
- `bin/` + `web.config` are **git-tracked** — local edits show in `git status`; revert before any commit/PR.

## 7. Why not just patch redirects?

Tested 2026-06-25: patching the `System.Buffers` redirect `4.0.3.0`→`4.0.4.0` advanced the error to `System.Collections.Immutable` (the next contaminated assembly). With ~5 `.NET 9` assemblies all mismatched, redirect-patching is an indefinite chase, and even if it bound, the `.NET 9` assemblies are ABI-incompatible with EF Core 2.1.1 / Imageflow compiled against older versions → runtime errors after binding. **The DLLs must be replaced, not just redirected.**
