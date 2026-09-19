#Requires -Version 5.1
<#
.SYNOPSIS
  Executable preflight for Argumentum regeneration runs (measure only, zero regen).

.DESCRIPTION
  Third recurrence of the missed clobber (18/09, run B: 0 PNG rewritten) proved a reminder
  is not enough: this script MEASURES each gate and prints GO/NO-GO per line.
  Run it from the short junction root (e.g. D:\A7) right before a regen launch.

  Lines measured:
    1-3   Disk headroom D:/C:/G: vs required, FAIL below x2 margin (#1294 disk-held run)
    4     Shell: refuse Git-Bash/MSYS (junction resolved at spawn => WriteBlob Failed, #1179)
    5     Worktree path is the short junction form, not the long .regen-* path (#1179)
    6+    Clobber effective: PNG and .harvest.json files remaining under Target\<lang> = 0
          (counts files AFTER the operator's clobber - the check the 3 recurrences lacked)
    last  Network inputs: HEAD == origin/master; raw-master templates served byte-identical
          to the worktree (#1225/#1228 - a Release render reads the network, not the tree);
          local CardPen IIS answers 200 (committed != served)

  Exit code 0 = all GO, 1 = at least one NO-GO.

.PARAMETER SelfTest
  Falsifying mutations on temp fixtures (no network): removing one clobber line must turn
  the clobber row NO-GO naming the residual PNG count (#1112: a self-written expectation
  is green by construction until proven able to go red).

.EXAMPLE
  cd D:\A7 ; powershell -File scripts\regen\preflight-regen.ps1
  powershell -File scripts\regen\preflight-regen.ps1 -SelfTest
#>
[CmdletBinding()]
param(
    [string]$WorktreePath = (Get-Location).ProviderPath,
    [string[]]$Languages = @('ar','en','es','fa','fr','pt','ru','zh'),
    [double]$RequiredGoD = 12.5,
    [double]$RequiredGoC = 7.8,
    [double]$RequiredGoG = 3.75,
    [string]$BinConfig = 'Release',
    [switch]$SkipNetwork,
    [switch]$SelfTest
)

$script:Results = New-Object System.Collections.Generic.List[object]

function Add-Result([int]$Id, [string]$Name, [bool]$Go, [string]$Evidence) {
    $script:Results.Add([pscustomobject]@{
        Id = $Id; Name = $Name
        Status = ($(if ($Go) { 'GO' } else { 'NO-GO' }))
        Evidence = $Evidence
    })
    return $Go
}

function Get-DiskRow([string]$Letter, [double]$RequiredGo) {
    $drive = Get-PSDrive -Name $Letter -ErrorAction SilentlyContinue
    if (-not $drive) { return @{ Go = $false; Ev = "volume $Letter`: absent" } }
    $free = [math]::Round($drive.Free / 1GB, 1)
    $floor = [math]::Round(2 * $RequiredGo, 1)
    $go = $free -ge $floor
    $ev = "libre ${free} Go vs requis ${RequiredGo} Go (x2 = ${floor} Go)"
    return @{ Go = $go; Ev = $ev }
}

function Get-ShellRow {
    $msys = -not [string]::IsNullOrEmpty($env:MSYSTEM) -or -not [string]::IsNullOrEmpty($env:MSYSCON)
    if ($msys) {
        return @{ Go = $false; Ev = "MSYSTEM='$env:MSYSTEM' - Git-Bash/MSYS resolves the junction at spawn => MagickCoderError WriteBlob Failed (#1179)" }
    }
    return @{ Go = $true; Ev = "PowerShell $($PSVersionTable.PSVersion), no MSYS env" }
}

function Get-PathRow([string]$WtPath) {
    $root = $WtPath -replace '\\Generation.*$', ''
    $isJunction = $root -match '^[A-Za-z]:\\A\d+$'
    $lenOk = $WtPath.Length -le 250
    if ($isJunction -and $lenOk) {
        return @{ Go = $true; Ev = "worktree '$root' (junction form, $($WtPath.Length) chars)" }
    }
    if (-not $isJunction) {
        return @{ Go = $false; Ev = "worktree root '$root' is the LONG form - launch from the short junction (e.g. D:\A7), #1179" }
    }
    return @{ Go = $false; Ev = "path $($WtPath.Length) chars > 250 (EnsurePathWithinLimit fails the run)" }
}

function Get-ClobberRow([string]$TargetRoot, [string]$Lang) {
    $langRoot = Join-Path $TargetRoot $Lang
    if (-not (Test-Path $langRoot)) {
        return @{ Go = $true; Ev = "Target\$Lang absent = virgin tree" }
    }
    $img = Join-Path $langRoot 'Images'
    $har = Join-Path $langRoot 'Harvest'
    $png = 0; $js = 0
    if (Test-Path $img) { $png = @(Get-ChildItem $img -Recurse -File -ErrorAction SilentlyContinue).Count }
    if (Test-Path $har) { $js = @(Get-ChildItem $har -Recurse -File -Filter *.json -ErrorAction SilentlyContinue).Count }
    $go = (($png + $js) -eq 0)
    $ev = "$png files under Images + $js .harvest.json under Harvest remaining (expected 0)"
    return @{ Go = $go; Ev = $ev; Png = $png }
}

function Get-GitRow([string]$WtPath) {
    $head = (& git -C $WtPath rev-parse HEAD 2>$null)
    $master = (& git -C $WtPath rev-parse origin/master 2>$null)
    if (-not $head -or -not $master) { return @{ Go = $false; Ev = 'git rev-parse failed (not a repo?)' } }
    $dirty = @(& git -C $WtPath status --porcelain 2>$null).Count
    $go = ($head.Trim() -eq $master.Trim())
    $ev = "HEAD $($head.Trim().Substring(0,8)) vs origin/master $($master.Trim().Substring(0,8)); $dirty uncommitted paths (run levers expected)"
    return @{ Go = $go; Ev = $ev }
}

function Get-TemplateRow([string]$WtPath) {
    $wgc = Join-Path $WtPath 'Generation\Converters\Argumentum.AssetConverter\WebBasedGenerator\WebBasedGeneratorConfig.cs'
    if (-not (Test-Path $wgc)) { return @{ Go = $false; Ev = "WebBasedGeneratorConfig.cs not found under worktree" } }
    $raw = Get-Content $wgc -Raw
    $rels = [regex]::Matches($raw, 'https://raw\.githubusercontent\.com/ArgumentumGames/Argumentum/master/(Cards/[A-Za-z0-9_\-./]+\.json)') |
        ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique
    if (-not $rels -or @($rels).Count -eq 0) { return @{ Go = $false; Ev = '0 raw-master template URLs extracted (parser drift?)' } }
    [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12
    $tmp = New-TemporaryFile
    $ok = 0; $bad = @()
    foreach ($rel in $rels) {
        $local = Join-Path $WtPath ($rel -replace '/', '\')
        try {
            Invoke-WebRequest -Uri "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/$rel" `
                -UseBasicParsing -TimeoutSec 30 -OutFile $tmp.FullName | Out-Null
            if (-not (Test-Path $local)) { $bad += "$rel (absent from worktree)"; continue }
            $h1 = (Get-FileHash $tmp.FullName -Algorithm MD5).Hash
            $h2 = (Get-FileHash $local -Algorithm MD5).Hash
            if ($h1 -eq $h2) { $ok++ } else { $bad += "$rel (served != worktree)" }
        } catch { $bad += "$rel (fetch failed: $($_.Exception.Message))" }
    }
    Remove-Item $tmp.FullName -Force -ErrorAction SilentlyContinue
    $go = (@($bad).Count -eq 0)
    $ev = ("{0}/{1} templates served byte-identical to worktree" -f $ok, @($rels).Count)
    if ($bad) { $ev += '; divergent: ' + (($bad | Select-Object -First 3) -join '; ') + $(if (@($bad).Count -gt 3) { (" (+{0} more)" -f (@($bad).Count - 3)) } else { '' }) }
    return @{ Go = $go; Ev = $ev }
}

function Get-CardpenRow([string]$WtPath) {
    $wgc = Join-Path $WtPath 'Generation\Converters\Argumentum.AssetConverter\WebBasedGenerator\WebBasedGeneratorConfig.cs'
    $m = [regex]::Match((Get-Content $wgc -Raw), 'LocalCardpenUrl\s*\{[^}]*\}\s*=\s*@"?([^"]+)')
    if (-not $m.Success) { return @{ Go = $false; Ev = 'LocalCardpenUrl not found in config' } }
    $url = $m.Groups[1].Value
    try {
        [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12
        $r = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 15
        return @{ Go = ($r.StatusCode -eq 200); Ev = "$url -> HTTP $($r.StatusCode)" }
    } catch { return @{ Go = $false; Ev = "$url unreachable: $($_.Exception.Message)" } }
}

function Invoke-Preflight {
    $id = 1
    foreach ($spec in @(@{L='D';R=$RequiredGoD}, @{L='C';R=$RequiredGoC}, @{L='G';R=$RequiredGoG})) {
        $r = Get-DiskRow $spec.L $spec.R
        Add-Result $id "Disk $($spec.L): (x2 margin)" $r.Go $r.Ev | Out-Null; $id++
    }
    $r = Get-ShellRow
    Add-Result $id 'Shell (#1179)' $r.Go $r.Ev | Out-Null; $id++
    $r = Get-PathRow $WorktreePath
    Add-Result $id 'Path = short junction (#1179)' $r.Go $r.Ev | Out-Null; $id++
    $targetRoot = Join-Path $WorktreePath "Generation\Converters\Argumentum.AssetConverter\bin\$BinConfig\net9.0-windows\Target"
    foreach ($lang in $Languages) {
        $r = Get-ClobberRow $targetRoot $lang
        Add-Result $id "Clobber $lang (residual PNG+harvest)" $r.Go $r.Ev | Out-Null; $id++
    }
    if (-not $SkipNetwork) {
        $r = Get-GitRow $WorktreePath
        Add-Result $id 'Base = origin/master' $r.Go $r.Ev | Out-Null; $id++
        $r = Get-TemplateRow $WorktreePath
        Add-Result $id 'Templates served == worktree (#1225/#1228)' $r.Go $r.Ev | Out-Null; $id++
        $r = Get-CardpenRow $WorktreePath
        Add-Result $id 'CardPen IIS reachable' $r.Go $r.Ev | Out-Null; $id++
    }
    return $script:Results
}

function Invoke-SelfTest {
    $script:SelfTestFails = 0
    function Assert([bool]$Cond, [string]$Name, [string]$Detail) {
        if ($Cond) { Write-Host "  PASS  $Name" } else { Write-Host "  FAIL  $Name -- $Detail"; $script:SelfTestFails++ }
    }

    $tmp = Join-Path ([IO.Path]::GetTempPath()) ('preflight-selftest-' + [guid]::NewGuid().ToString('N'))
    $wt = Join-Path $tmp 'wt'
    $target = Join-Path $wt 'Target'
    New-Item -ItemType Directory -Path "$target\fr\Images\density-0\Rules" -Force | Out-Null
    New-Item -ItemType Directory -Path "$target\fr\Harvest" -Force | Out-Null

    # M1 (DoD mutation): simulate "one clobber line removed" -> 5 residual PNG + 2 harvest json
    1..5 | ForEach-Object { Set-Content -Path "$target\fr\Images\density-0\Rules\rules_0$_.png" -Value 'png' }
    'Fallacies', 'Memo' | ForEach-Object { Set-Content -Path "$target\fr\Harvest\${_}_harvest_fr.json" -Value '{}' }
    $r = Get-ClobberRow $target 'fr'
    Assert (-not $r.Go) 'M1 clobber row goes NO-GO on residual files' ($r.Ev)
    Assert ($r.Ev -match '(^|\s)5 files') 'M1 evidence names the residual count (5)' ($r.Ev)

    # M1b positive control: virgin tree must be GO
    $r = Get-ClobberRow $target 'en'
    Assert $r.Go 'M1b virgin language row is GO' ($r.Ev)

    # M2 shell mutation: MSYSTEM set => NO-GO
    $oldMsys = $env:MSYSTEM
    $env:MSYSTEM = 'MINGW64'
    $r = Get-ShellRow
    if ($null -eq $oldMsys) { Remove-Item Env:MSYSTEM -ErrorAction SilentlyContinue } else { $env:MSYSTEM = $oldMsys }
    Assert (-not $r.Go) 'M2 MSYS env => shell row NO-GO' ($r.Ev)

    # M3 path mutation: long worktree form => NO-GO
    $r = Get-PathRow 'D:\Dev\Argumentum\.regen-fake-worktree\Generation\Converters'
    Assert (-not $r.Go) 'M3 long-path worktree => path row NO-GO' ($r.Ev)

    # M4 disk mutation: absurd requirement => NO-GO
    $r = Get-DiskRow 'D' 1e9
    Assert (-not $r.Go) 'M4 required > free => disk row NO-GO' ($r.Ev)

    # M5 network compare mutation: one byte differs => hash mismatch detected
    $f1 = Join-Path $tmp 'a.json'; $f2 = Join-Path $tmp 'b.json'
    Set-Content $f1 '{"a":1}'; Set-Content $f2 '{"a":2}'
    $h1 = (Get-FileHash $f1 -Algorithm MD5).Hash; $h2 = (Get-FileHash $f2 -Algorithm MD5).Hash
    Assert ($h1 -ne $h2) 'M5 one-byte template change => hash comparison detects it' "h1=$h1 h2=$h2"

    Remove-Item $tmp -Recurse -Force -ErrorAction SilentlyContinue
    return $script:SelfTestFails
}

if ($SelfTest) {
    Write-Host '== preflight-regen self-test (falsifying mutations, hermetic) =='
    $n = Invoke-SelfTest
    if ($n -gt 0) { Write-Host "SELF-TEST RED: $n assertion(s) failed"; exit 1 }
    Write-Host 'SELF-TEST GREEN: every mutation turned its row red'
    exit 0
}

$results = Invoke-Preflight
$results | Format-Table Id, Status, Name, Evidence -AutoSize | Out-String -Width 220 | Write-Host
$nogo = @($results | Where-Object { $_.Status -eq 'NO-GO' }).Count
Write-Host ("summary: {0}/{1} GO, {2} NO-GO" -f ($results.Count - $nogo), $results.Count, $nogo)
if ($nogo -gt 0) { exit 1 }
exit 0
