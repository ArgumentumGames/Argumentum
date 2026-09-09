<#
.SYNOPSIS
    Build/verify the v2.0.0 PDF bundle manifest (read-only on the bundle; writes only the manifest file).

.DESCRIPTION
    Validates that a release bundle directory contains EXACTLY the expected 8 languages x 10 PDF types
    (80 PDFs): every *.pdf under the bundle is enumerated recursively and compared against the expected
    path set — BOTH missing and unexpected files fail the run (exit 1, both lists printed). Computes
    size and (optionally) sha256 per file, and emits manifest-v2.0.0.md (provenance, explicit A0
    section, langue x type inventory, fingerprints).

    A freshness attestation is MANDATORY (-Freshness final|stale): the script refuses to run without
    it, so a manifest titled v2.0.0 can never be produced without an explicit freshness statement.
    -Freshness stale marks the manifest NON FINAL / STALE with a staleness section (-StaleNote
    documents the delta; a generic default note is used otherwise). -Freshness final is the
    operator's attestation that the bundle is the final release material (normal manifest).

    The script NEVER mutates the bundle: it only reads it and writes the manifest file at
    -ManifestPath. The manifest is written UTF-8 without BOM.

.PARAMETER Freshness
    Mandatory freshness attestation. 'stale' = bundle known non-final (manifest titled NON FINAL /
    STALE + staleness section). 'final' = operator attests this is the final release bundle.

.PARAMETER BundleDir
    Root of the bundle to inventory. Default: the 06/09 run E bundle on the Drive mount (known
    STALE: 24 Scenarii PDFs precede #1297/#1300/#1308/#1310).

.PARAMETER ManifestPath
    Where to write the manifest. Default: <BundleDir>\manifest-v2.0.0.md.

.PARAMETER SkipHashes
    Skip sha256 computation (useful on slow/network mounts). Sizes are always computed.
    The manifest then carries 'n/a' hashes and is marked HASH-IMPERFECT.

.PARAMETER StaleNote
    Optional markdown note (single line) recorded in the staleness section when -Freshness stale
    (generic default note when omitted). Ignored with -Freshness final.

.PARAMETER BaseCommit
    Provenance commit that generated the bundle (default 2a2e7b32 = run E base).

.EXAMPLE
    .\package-v2.0.0.ps1 -Freshness stale -SkipHashes
    # fast size-only inventory of the historical (run E) bundle - manifest marked NON FINAL / STALE

.EXAMPLE
    .\package-v2.0.0.ps1 -Freshness final
    # final assembly pass: full sha256 manifest, no staleness section
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('final', 'stale')]
    [string]$Freshness,
    [string]$BundleDir   = "G:\Mon Drive\Argumentum\review-v2.0.0-regen-20260906",
    [string]$ManifestPath = "",
    [switch]$SkipHashes  = $false,
    [string]$StaleNote   = "",
    [string]$BaseCommit  = "2a2e7b32"
)

$ErrorActionPreference = 'Stop'
$langs = @('ar','en','es','fa','fr','pt','ru','zh')
$types = @(
    'TarotCards',
    'TarotCards_Virtues',
    'PokerCards',
    'TarotCards_Print&Play_A4',
    'TarotCards_Print&Play_Light_A4',
    'PokerCards_Print&Play_A4',
    'PokerCards_Print&Play_Light_A4',
    'Fallacies_Web_A0',
    'Fallacies_Web_A4',
    'Fallacies_Web_Thumbnails_A4'
)

if (-not (Test-Path $BundleDir)) { Write-Error "BundleDir not found: $BundleDir"; exit 2 }
if (-not $ManifestPath) { $ManifestPath = Join-Path $BundleDir 'manifest-v2.0.0.md' }

$missing = @()
$inventory = [ordered]@{}
foreach ($l in $langs) {
    $langDir = Join-Path $BundleDir $l
    if (-not (Test-Path $langDir)) { $missing += "dir $l"; continue }
    foreach ($t in $types) {
        $file = Join-Path $langDir "Argumentum_$t`_$l.pdf"
        if (-not (Test-Path $file)) { $missing += "$l/$t"; continue }
        $f = Get-Item $file
        $hash = 'n/a'
        if (-not $SkipHashes) { $hash = (Get-FileHash $file -Algorithm SHA256).Hash }
        $inventory["$l/$t"] = [pscustomobject]@{ Lang=$l; Type=$t; Size=$f.Length; Hash=$hash }
    }
}

# exact set: unexpected PDFs fail the run just like missing ones
$rootLen = (Get-Item $BundleDir).FullName.Length
$foundRel = @(Get-ChildItem -Path $BundleDir -Recurse -Filter *.pdf | ForEach-Object {
    ($_.FullName.Substring($rootLen) -replace '^[\\/]+', '') -replace '\\', '/'
})
$expectedRel = @()
foreach ($l in $langs) { foreach ($t in $types) { $expectedRel += "$l/Argumentum_$t`_$l.pdf" } }
$unexpected = @($foundRel | Where-Object { $expectedRel -notcontains $_ })

$expected = $langs.Count * $types.Count
$found = $inventory.Count
Write-Host "[package-v2.0.0] files: $found / $expected (pdf on disk: $($foundRel.Count))"
if ($missing.Count -gt 0 -or $unexpected.Count -gt 0) {
    if ($missing.Count -gt 0)    { Write-Host "[package-v2.0.0] MISSING $($missing.Count): $($missing -join ', ')" }
    if ($unexpected.Count -gt 0) { Write-Host "[package-v2.0.0] UNEXPECTED $($unexpected.Count): $($unexpected -join ', ')" }
    exit 1
}

# ---- render manifest ----
$total = ($inventory.Values | Measure-Object -Property Size -Sum).Sum
$sb = New-Object System.Text.StringBuilder
$title = if ($Freshness -eq 'stale') { 'v2.0.0 bundle - NON FINAL / STALE' } else { 'v2.0.0 bundle' }
[void]$sb.AppendLine("# Manifest - $title (generated by docs/release-dossier/package-v2.0.0.ps1)")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("- **Generated**: $((Get-Date -Format 'yyyy-MM-dd HH:mm:ss'))")
[void]$sb.AppendLine("- **BundleDir**: $BundleDir")
[void]$sb.AppendLine("- **Base commit (provenance)**: $BaseCommit")
[void]$sb.AppendLine("- **Files**: $found / $expected (8 langs x 10 types) · **Total size**: $([math]::Round($total/1MB,1)) Mo")
[void]$sb.AppendLine("- **Hashes**: $(if ($SkipHashes) {'n/a (HASH-IMPERFECT — run without -SkipHashes for the final pass)'} else {'sha256 per file'})")
if ($Freshness -eq 'final') {
    [void]$sb.AppendLine("- **Status**: FINAL - attested by operator (-Freshness final)")
} else {
    [void]$sb.AppendLine("- **Status**: NON FINAL / STALE - attested by operator (-Freshness stale); NOT a release package")
    $note = if ($StaleNote) { $StaleNote } else { 'declared non-final by operator - freshness delta not documented' }
    [void]$sb.AppendLine("- **Staleness delta**: $note")
}
[void]$sb.AppendLine("")
[void]$sb.AppendLine("## Affiches A0 (explicit part of the release material)")
[void]$sb.AppendLine("| Fichier | Taille | sha256 |")
[void]$sb.AppendLine("|---|---|---|")
foreach ($l in $langs) {
    $k = "$l/Fallacies_Web_A0"
    $it = $inventory[$k]
    if (-not $it) { continue }
    $short = if ($it.Hash -ne 'n/a') { $it.Hash.Substring(0,16) + '…' } else { 'n/a' }
    [void]$sb.AppendLine("| Argumentum_Fallacies_Web_A0_$l.pdf | $([math]::Round($it.Size/1KB,1)) Ko | $short |")
}
[void]$sb.AppendLine("")
[void]$sb.AppendLine("## Inventaire langue x type (taille en Mo)")
[void]$sb.AppendLine("| | $($langs -join ' | ') | Total |")
[void]$sb.AppendLine(("|---" * ($langs.Count + 2)) + "|")
foreach ($t in $types) {
    $row = @($t)
    $sum = 0
    foreach ($l in $langs) {
        $it = $inventory["$l/$t"]
        if ($it) { $sum += $it.Size; $row += [math]::Round($it.Size/1MB,1) } else { $row += '-' }
    }
    [void]$sb.AppendLine("| $($row -join ' | ') | $([math]::Round($sum/1MB,1)) |")
}
[void]$sb.AppendLine("| **TOTAL** | $(' | ' * ($langs.Count - 1)) | **$([math]::Round($total/1MB,1))** |")
[void]$sb.AppendLine("")

if (-not $SkipHashes) {
    [void]$sb.AppendLine("## Empreintes sha256 (per file)")
    foreach ($k in $inventory.Keys) { [void]$sb.AppendLine("- $k $($inventory[$k].Hash)") }
}

[System.IO.File]::WriteAllText($ManifestPath, $sb.ToString(), [System.Text.UTF8Encoding]::new($false))
Write-Host "[package-v2.0.0] manifest written: $ManifestPath ($((Get-Item $ManifestPath).Length) o)"
exit 0
