<#
.SYNOPSIS
    Matrice de mesure equilibree du delta runtime .NET 9 / 10 / 11 sur le banc
    Argumentum.AssetConverter.Benchmarks.

.DESCRIPTION
    Rejoue le controle decrit dans
    benchmarks/MULTI-RUNTIME-net9-net10-net11-RECHECK.md.

    Un ordre de passage fixe confond la POSITION avec le runtime : dans
    "net9, net10, net11 x3", net9 est toujours en 1re position et net11 toujours
    en 3e -- toute derive de la machine d'une position a l'autre est alors
    indiscernable d'un effet de runtime (mesure : l'ecart entre positions atteint
    +-38,6 % sur l'axe timestamping, du meme ordre que l'ecart net9 -> net10).

    Ce script retire le confound :
      1. CARRE LATIN -- trois ordres cycliques (9,10,11 / 10,11,9 / 11,9,10)
         repetes deux fois : chaque runtime occupe chaque position exactement
         deux fois ;
      2. ESPACEMENT -- une pause entre deux jambes consecutivees, pour qu'une
         jambe ne soit pas mesuree sur l'etat thermique/cache laisse par la
         precedente ;
      3. WARM-UP ECARTE -- une jambe de warm-up est jouee puis jetee avant la
         premiere jambe mesuree ;
      4. ATTESTATION -- la ligne [Host] du rapport BDN de chaque jambe est lue
         et comparee au runtime attendu.

    Le runtime est epingle par l'environnement (DOTNET_ROOT + DOTNET_ROLL_FORWARD)
    et le binaire est lance directement (apphost) : c'est la seule voie qui
    honore DOTNET_ROOT, cf. MULTI-RUNTIME-net9-net10-net11.md pour les trois
    murs mesures (NU1201, process enfant, DotNetSdkValidator de BDN 0.15.8).

.PARAMETER OutRoot
    Dossier de sortie. Chaque jambe y ecrit son rapport BDN et son journal.

.PARAMETER BenchDir
    Dossier du projet de banc (contient bin/Release/net9.0-windows/...).

.PARAMETER DotNet11Root
    Root .NET 11 isole. Doit contenir Microsoft.NETCore.App ET
    Microsoft.WindowsDesktop.App : un root incomplet fait retomber le probe
    silencieusement sur le root systeme (le TFM -windows exige les deux).

.PARAMETER SpacingSeconds
    Pause entre deux jambes consecutivees.

.EXAMPLE
    pwsh -File benchmarks/recheck-multiruntime.ps1 -OutRoot C:\temp\recheck
#>
[CmdletBinding()]
param(
    [string]$OutRoot = (Join-Path ([System.IO.Path]::GetTempPath()) "argumentum-multiruntime-recheck"),
    [string]$BenchDir = "Generation/Converters/Argumentum.AssetConverter.Benchmarks",
    [string]$DotNet11Root = "D:\dev\tools\dotnet11rc",
    [int]$SpacingSeconds = 60
)

$ErrorActionPreference = "Continue"

$exe = Join-Path $BenchDir "bin/Release/net9.0-windows/Argumentum.AssetConverter.Benchmarks.exe"
if (-not (Test-Path $exe)) {
    throw "Binaire introuvable : $exe -- lancer d'abord `dotnet build $BenchDir -c Release`."
}
if (-not (Test-Path $DotNet11Root)) {
    throw "Root .NET 11 isole introuvable : $DotNet11Root"
}

$null = New-Item -ItemType Directory -Force -Path $OutRoot

# --- configuration par runtime -------------------------------------------------
# net9  : runtime systeme (le TFM natif du binaire)
# net10 : roll-forward majeur sur le root systeme
# net11 : root isole + roll-forward Major
$legs = @{
    net9  = @{ root = $null;                    roll = $null }
    net10 = @{ root = $null;                    roll = "LatestMajor" }
    net11 = @{ root = $DotNet11Root;            roll = "Major" }
}

# --- carre latin, cycle deux fois ------------------------------------------------
$orders = @(
    @("net9",  "net10", "net11"),
    @("net10", "net11", "net9"),
    @("net11", "net9",  "net10")
)

function Invoke-Leg {
    param([string]$rt, [string]$tag, [switch]$Discard)

    $cfg = $legs[$rt]
    if ($cfg.root) { $env:DOTNET_ROOT = $cfg.root }
    else { Remove-Item Env:DOTNET_ROOT -ErrorAction SilentlyContinue }
    if ($cfg.roll) { $env:DOTNET_ROLL_FORWARD = $cfg.roll }
    else { Remove-Item Env:DOTNET_ROLL_FORWARD -ErrorAction SilentlyContinue }

    $legDir = Join-Path $OutRoot $tag
    $null = New-Item -ItemType Directory -Force -Path $legDir

    # -- probe : la ligne [Host] est l'attestation du runtime reellement employe
    $probe = & $exe --info 2>&1 | Out-String
    $hostLine = ($probe -split "`r?`n" | Where-Object { $_ -match "\[Host\]" } | Select-Object -First 1)
    $hostLine = if ($hostLine) { $hostLine.Trim() } else { "<absente>" }

    $sw = [Diagnostics.Stopwatch]::StartNew()
    & $exe -f "*" --artifacts (Join-Path $legDir "artifacts") 2>&1 |
        Out-File -FilePath (Join-Path $legDir "bench.log") -Encoding utf8
    $sw.Stop()

    Write-Host ("[{0}] {1} -> {2} ({3:N1}s)" -f $tag, $rt, $hostLine, $sw.Elapsed.TotalSeconds)

    if ($Discard) {
        Write-Host ("[{0}] warm-up ecarte" -f $tag)
        return
    }

    $rows += [pscustomobject]@{
        tag      = $tag
        runtime  = $rt
        host     = $hostLine
        seconds  = [math]::Round($sw.Elapsed.TotalSeconds, 1)
    }
}

$rows = @()

Write-Host "=== warm-up (ecarte) ==="
Invoke-Leg -rt "net9" -tag "warmup" -Discard

for ($rep = 1; $rep -le 6; $rep++) {
    $order = $orders[($rep - 1) % $orders.Count]
    Write-Host ("=== repetition {0} : {1} ===" -f $rep, ($order -join " -> "))
    for ($pos = 0; $pos -lt $order.Count; $pos++) {
        $rt = $order[$pos]
        Invoke-Leg -rt $rt -tag ("rep{0}-{1}-p{2}" -f $rep, $rt, ($pos + 1))
        if ($pos -lt $order.Count - 1 -and $SpacingSeconds -gt 0) {
            Start-Sleep -Seconds $SpacingSeconds
        }
    }
}

Write-Host "=== DONE ==="
$rows | Format-Table -AutoSize | Out-String | Write-Host
