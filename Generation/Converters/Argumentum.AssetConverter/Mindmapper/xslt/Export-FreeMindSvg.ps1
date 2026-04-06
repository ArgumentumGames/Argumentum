param(
    [Parameter(Mandatory=$true)]
    [string]$MmFilesDir,

    [string]$FreeMindPath = "C:\Program Files (x86)\FreeMind\FreeMind.exe",
    [int]$LoadWaitSeconds = 12,
    [int]$ExportWaitSeconds = 8
)

Add-Type @"
using System;
using System.Runtime.InteropServices;
public class Win32 {
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}
"@

function Clean-FreeMind {
    Get-Process javaw -ErrorAction SilentlyContinue | Where-Object { $_.MainWindowTitle -match 'FreeMind' } | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep 2
    Get-ChildItem "$env:USERPROFILE\.freemind" -Filter 'FM_*.mm' -ErrorAction SilentlyContinue | Remove-Item -Force
    Get-ChildItem "$env:USERPROFILE\.freemind" -Filter '*.lck' -ErrorAction SilentlyContinue | Remove-Item -Force
}

function Export-MmToSvg {
    param([string]$MmFile)

    $svgFile = [System.IO.Path]::ChangeExtension($MmFile, ".svg")
    $svgBefore = if (Test-Path $svgFile) { (Get-Item $svgFile).LastWriteTimeUtc } else { [DateTime]::MinValue }

    Write-Host "[$([DateTime]::Now.ToString('HH:mm:ss'))] Exporting: $(Split-Path $MmFile -Leaf)" -ForegroundColor Cyan

    # 1. Clean
    Clean-FreeMind

    # 2. Launch FreeMind
    Start-Process -FilePath $FreeMindPath -ArgumentList "`"$MmFile`""

    # 3. Wait for window — must contain the .mm FILENAME in title (not just "FreeMind")
    # FreeMind shows just "FreeMind" during loading, then "filename.mm - FreeMind - ..." once loaded
    $mmBaseName = [System.IO.Path]::GetFileName($MmFile)
    $proc = $null
    for ($i = 0; $i -lt $LoadWaitSeconds; $i++) {
        Start-Sleep 1
        $candidates = Get-Process javaw -ErrorAction SilentlyContinue | Where-Object { $_.SessionId -gt 0 }
        foreach ($c in $candidates) {
            try { $c.Refresh() } catch {}
            if ($c.MainWindowTitle -match [regex]::Escape($mmBaseName)) {
                $proc = $c
                break
            }
        }
        if ($proc) { break }
    }
    if (-not $proc) {
        Write-Host "  FAILED: FreeMind window with '$mmBaseName' not found after ${LoadWaitSeconds}s" -ForegroundColor Red
        # Kill any FreeMind that may have started
        Clean-FreeMind
        return $false
    }
    Write-Host "  FreeMind ready: $($proc.MainWindowTitle)" -ForegroundColor Gray
    Start-Sleep 5

    # 4. Focus
    [Win32]::ShowWindow($proc.MainWindowHandle, 9) | Out-Null
    [Win32]::SetForegroundWindow($proc.MainWindowHandle) | Out-Null
    Start-Sleep 1

    # 5. Send keystrokes: Alt+F, 8xDOWN, RIGHT, 12xDOWN, ENTER, ENTER, ENTER
    # SendKeys throws Win32Exception "L'opération a réussi" (error code 0) — false negative, ignore it
    function Send-Key([string]$key) {
        try { [System.Windows.Forms.SendKeys]::SendWait($key) }
        catch [System.ComponentModel.Win32Exception] { <# ignore error code 0 #> }
    }

    Send-Key "{ESC}"
    Start-Sleep -Milliseconds 500

    Send-Key "%f"
    Start-Sleep 2

    for ($i = 0; $i -lt 8; $i++) {
        Send-Key "{DOWN}"
        Start-Sleep -Milliseconds 300
    }

    Send-Key "{RIGHT}"
    Start-Sleep -Milliseconds 1500

    for ($i = 0; $i -lt 12; $i++) {
        Send-Key "{DOWN}"
        Start-Sleep -Milliseconds 300
    }

    Send-Key "{ENTER}"
    Start-Sleep 3
    Send-Key "{ENTER}"
    Start-Sleep 3
    Send-Key "{ENTER}"
    Start-Sleep $ExportWaitSeconds

    # 6. Check result
    $success = (Test-Path $svgFile) -and ((Get-Item $svgFile).LastWriteTimeUtc -gt $svgBefore) -and ((Get-Item $svgFile).Length -gt 500000)

    if ($success) {
        $sizeKB = [math]::Round((Get-Item $svgFile).Length / 1024)
        Write-Host "  SUCCESS: $(Split-Path $svgFile -Leaf) (${sizeKB} KB)" -ForegroundColor Green
    } else {
        Write-Host "  FAILED: SVG not generated or too small" -ForegroundColor Red
    }

    # 7. Kill FreeMind
    Clean-FreeMind

    return $success
}

# Load Windows Forms assembly for SendKeys
Add-Type -AssemblyName System.Windows.Forms

# Find all .mm files
$mmFiles = Get-ChildItem $MmFilesDir -Recurse -Filter "*.mm" | Where-Object { $_.Name -notmatch '_fr\.mm$' -or $_.DirectoryName -match '\\fr\\' }
# Actually, just get all .mm files
$mmFiles = Get-ChildItem $MmFilesDir -Recurse -Filter "*.mm"

Write-Host "`n=== FreeMind SVG Export ===" -ForegroundColor Yellow
Write-Host "Found $($mmFiles.Count) .mm files to convert`n"

$results = @()
foreach ($mm in $mmFiles) {
    $success = Export-MmToSvg -MmFile $mm.FullName
    $results += @{ File = $mm.Name; Success = $success }
}

Write-Host "`n=== Results ===" -ForegroundColor Yellow
foreach ($r in $results) {
    $status = if ($r.Success) { "OK" } else { "FAIL" }
    Write-Host "  [$status] $($r.File)"
}

# Final cleanup
Clean-FreeMind
