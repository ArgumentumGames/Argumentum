# Script de diagnostic Git Status
# Date: 2025-12-14
# Objectif: Lister les fichiers modifiés/non suivis et aider à l'analyse pour le nettoyage Git

$reportPath = "docs/investigations/git_status_report.txt"
$rootPath = Get-Location

Write-Host "Analyse de l'état Git dans $rootPath..."

# Capture du git status brut
$gitStatus = git status -u
$gitStatus | Out-File -FilePath $reportPath -Encoding utf8

# Analyse détaillée des fichiers
$files = git status --porcelain -u | ForEach-Object {
    $line = $_
    $status = $line.Substring(0, 2).Trim()
    $file = $line.Substring(3).Trim()
    
    # Gestion des chemins avec des guillemets (si espaces)
    if ($file.StartsWith('"') -and $file.EndsWith('"')) {
        $file = $file.Substring(1, $file.Length - 2)
    }

    [PSCustomObject]@{
        Status = $status
        Path = $file
        Extension = [System.IO.Path]::GetExtension($file)
        IsBinary = $false # Placeholder, detection naive par extension ci-dessous
    }
}

# Détection naïve des binaires/artefacts courants à ignorer
$artifactsExtensions = @(".pdf", ".dll", ".pdb", ".exe", ".log", ".tmp", ".cache", ".suo", ".user")
$files | ForEach-Object {
    if ($artifactsExtensions -contains $_.Extension) {
        $_.IsBinary = $true
    }
}

# Ajout de l'analyse structurée au rapport
Add-Content -Path $reportPath "`n`n=========================================="
Add-Content -Path $reportPath "ANALYSE DÉTAILLÉE"
Add-Content -Path $reportPath "=========================================="

# Groupement par extension
$grouped = $files | Group-Object Extension | Sort-Object Count -Descending

Add-Content -Path $reportPath "`n--- Fichiers par extension ---"
$grouped | ForEach-Object {
    Add-Content -Path $reportPath "$($_.Count.ToString().PadLeft(4)) fichiers $($_.Name)"
}

Add-Content -Path $reportPath "`n--- Fichiers potentiellement artefacts/générés (à vérifier) ---"
$files | Where-Object { $_.IsBinary -eq $true } | ForEach-Object {
    Add-Content -Path $reportPath "[$($_.Status)] $($_.Path)"
}

Add-Content -Path $reportPath "`n--- Liste complète des modifications ---"
$files | Sort-Object Path | ForEach-Object {
    Add-Content -Path $reportPath "[$($_.Status)] $($_.Path)"
}

Write-Host "Rapport généré : $reportPath"