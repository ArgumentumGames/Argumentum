<#
.SYNOPSIS
  DNN sandbox bin/ repair — replace .NET 9 SDK contaminants with net48-compatible versions.

  Step B1 of docs/dnn/go-live-turnkey-checklist.md. Run interactively in jsboige's
  RDP/sandbox session (the only place the runtime blocker can be cleared).

.DESCRIPTION
  The 2026-06-25 boot attempt (PR #596 issuecomment-4804068740) characterized: bin/ root
  carries .NET 9 SDK contract assemblies (asmVer 9.0.0.0) that EF Core 2.1.1 / DNN net48
  cannot bind -> HTTP 500 0x80131040 cascade. This script is the bounded recipe from
  sandbox-bootstrap-runbook.md §3, made executable in one command.

  -DryRun is the DEFAULT: prints the plan + current->target version map, changes nothing.
  Pass -Apply to execute. Backs up bin/ to bin.contaminated.bak before any change.

  bin/ is git-tracked. NEVER commit the result — revert after validation:
    git checkout -- DNNPlatform/bin DNNPlatform/web.config

.PARAMETER BinRoot
  Path to DNNPlatform/bin. Default D:\Dev\Argumentum\DNNPlatform\bin.

.PARAMETER Apply
  Execute the repair (backup + copy + download + extract). Without it: dry-run plan only.

.EXAMPLE
  .\repair-bin-net48.ps1                    # dry-run: see the plan
  .\repair-bin-net48.ps1 -Apply             # execute the bin/ repair
#>
param(
  [string]$BinRoot = 'D:\Dev\Argumentum\DNNPlatform\bin',
  [switch]$Apply
)
$ErrorActionPreference = 'Stop'
$DryRun = -not $Apply
$mode = if ($DryRun) { 'DRY-RUN (no changes — pass -Apply to execute)' } else { 'APPLY' }

# 5 .NET 9 contaminants -> fetch 6.0.0 NuGet (last line shipping a net4x lib/ folder,
# predates the .NET 8+ TFM-only cliff; satisfies EF Core 2.1.1 + DNN binding redirects).
$nugets = @(
  @{ Id='System.Collections.Immutable';      Asm='6.0.0.0' }
  @{ Id='System.Text.Json';                  Asm='6.0.0.0' }
  @{ Id='System.IO.Pipelines';               Asm='6.0.0.0' }
  @{ Id='System.Diagnostics.DiagnosticSource'; Asm='6.0.0.0' }
  @{ Id='System.Text.Encodings.Web';         Asm='6.0.0.0' }
)
$nugetVer = '6.0.0'

# 2 .NET 6 markers -> local clean source already in bin/Imageflow/ (correct older versions).
$local = @(
  @{ Id='System.Buffers'; Target='4.0.3.0'; Src='Imageflow\System.Buffers.dll' }
  @{ Id='System.Memory';  Target='4.0.1.1'; Src='Imageflow\System.Memory.dll' }
)

function Get-AsmVer([string]$dllPath) {
  if (-not (Test-Path $dllPath)) { return '(missing)' }
  try { return [System.Reflection.AssemblyName]::GetAssemblyName($dllPath).Version.ToString() }
  catch { return '(unreadable)' }
}

Write-Host "=== DNN bin/ repair ($mode) ===" -ForegroundColor Cyan
Write-Host "BinRoot: $BinRoot`n"

Write-Host "--- Current contaminant versions (current -> target) ---" -ForegroundColor Yellow
foreach ($n in $nugets) {
  $p = Join-Path $BinRoot "$($n.Id).dll"
  $cur = Get-AsmVer $p
  Write-Host ("  {0,-42} {1,-14} -> {2}  (NuGet $nugetVer)" -f $n.Id, $cur, $n.Asm)
}
foreach ($n in $local) {
  $p = Join-Path $BinRoot "$($n.Id).dll"
  $cur = Get-AsmVer $p
  Write-Host ("  {0,-42} {1,-14} -> {2}  (from {3})" -f $n.Id, $cur, $n.Target, $n.Src)
}

if ($DryRun) {
  Write-Host "`n[DRY-RUN] Would:" -ForegroundColor Magenta
  Write-Host "  1. Backup bin\ -> bin.contaminated.bak (if not present)"
  Write-Host "  2. Copy 2 clean DLLs from bin\Imageflow\ (Buffers 4.0.3.0, Memory 4.0.1.1)"
  Write-Host "  3. Download + extract 5 NuGet $nugetVer from nuget.org v3-flatcontainer (lib/net4x)"
  Write-Host "  4. Drop the 5 net48 DLLs into bin\"
  Write-Host "`n[DRY-RUN] web.config binding-redirect alignment is NOT auto-applied" -ForegroundColor Magenta
  Write-Host "  (see sandbox-bootstrap-runbook.md section 3.3 for the newVersion map)."
  Write-Host "  After -Apply: align newVersion -> 6.0.0.0 for the 5 + 4.0.3.0/4.0.1.1 for the 2."
  Write-Host "`n[DRY-RUN] Pass -Apply to execute the bin/ repair."
  return
}

# --- APPLY ---
if (-not (Test-Path $BinRoot)) { throw "BinRoot not found: $BinRoot" }

$bak = "$BinRoot.contaminated.bak"
if (-not (Test-Path $bak)) {
  Write-Host "`nBacking up bin\ -> $bak" -ForegroundColor Green
  Copy-Item $BinRoot $bak -Recurse
} else {
  Write-Host "`nBackup $bak already exists — skipping backup." -ForegroundColor DarkGray
}

# 1. Local copies (Buffers + Memory from bin/Imageflow/)
foreach ($n in $local) {
  $src = Join-Path $BinRoot $n.Src
  $dst = Join-Path $BinRoot "$($n.Id).dll"
  if (-not (Test-Path $src)) { throw "Clean source not found: $src" }
  Copy-Item $src $dst -Force
  Write-Host ("  Copied {0} -> {1}.dll (asm {2})" -f $n.Src, $n.Id, (Get-AsmVer $dst)) -ForegroundColor Green
}

# 2. NuGet fetch + extract (pick the highest net4x TFM folder, fallback netstandard2.0)
$tmp = Join-Path $env:TEMP "dnn-binrepair"
if (Test-Path $tmp) { Remove-Item $tmp -Recurse -Force }
New-Item -ItemType Directory -Force -Path $tmp | Out-Null

foreach ($n in $nugets) {
  $idLower = $n.Id.ToLower()
  $url = "https://api.nuget.org/v3-flatcontainer/$idLower/$nugetVer/$idLower.$nugetVer.nupkg"
  $pkg = Join-Path $tmp "$idLower.$nugetVer.zip"
  Write-Host "`n  Downloading $n.Id $nugetVer ..." -ForegroundColor Cyan
  try {
    Invoke-WebRequest $url -OutFile $pkg -UseBasicParsing
  } catch { throw "Download failed for $n.Id : $_" }

  $extract = Join-Path $tmp $n.Id
  Expand-Archive $pkg -DestinationPath $extract -Force

  # Prefer the highest .NET Framework folder (net462 > net461 > net46), else netstandard2.0
  $tfmDir = Get-ChildItem -Path "$extract\lib" -Directory -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -match '^net4' } |
    Sort-Object Name -Descending |
    Select-Object -First 1
  if (-not $tfmDir) {
    $tfmDir = Get-Item "$extract\lib\netstandard2.0" -ErrorAction SilentlyContinue
  }
  if (-not $tfmDir) { throw "No net4x/netstandard2.0 lib folder in $n.Id $nugetVer nupkg" }

  $dll = Join-Path $tfmDir.FullName "$($n.Id).dll"
  if (-not (Test-Path $dll)) { throw "$($n.Id).dll not found in $($tfmDir.Name)" }

  Copy-Item $dll (Join-Path $BinRoot "$($n.Id).dll") -Force
  Write-Host ("    Installed {0} from {1} -> asm {2}" -f $n.Id, $tfmDir.Name, (Get-AsmVer (Join-Path $BinRoot "$($n.Id).dll"))) -ForegroundColor Green
}
Remove-Item $tmp -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "`n=== bin/ repair DONE ===" -ForegroundColor Cyan
Write-Host "Verify (asmVer should be 6.0.0.0 for the 5 NuGet, 4.0.3.0/4.0.1.1 for the 2):"
foreach ($n in $nugets + $local) {
  $target = if ($n.Asm) { $n.Asm } else { $n.Target }
  $p = Join-Path $BinRoot "$($n.Id).dll"
  Write-Host ("  {0,-42} {1}  (target {2})" -f $n.Id, (Get-AsmVer $p), $target)
}
Write-Host "`nNEXT (see go-live-turnkey-checklist.md B1, sandbox-bootstrap-runbook.md section 3.3/3.4):" -ForegroundColor Yellow
Write-Host "  - Align web.config binding redirects newVersion -> 6.0.0.0 (5) + 4.0.3.0/4.0.1.1 (2)"
Write-Host "  - Edit web.config local: LocalDB conn string + throwaway machineKey (never commit)"
Write-Host "  - Boot: iisexpress /config:applicationhost.config /site:'"'"'DNN Argumentum'"'"'"
Write-Host "  - Expect HTTP 200 (not 0x80131040). Probe: Invoke-WebRequest http://localhost:8090/ -SkipHttpErrorCheck"
Write-Host "`nREVERT after validation (bin/ + web.config are git-tracked):" -ForegroundColor Yellow
Write-Host "  git checkout -- DNNPlatform\bin DNNPlatform\web.config"
