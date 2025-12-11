# Script de nettoyage temporaire
Write-Host "=== Nettoyage des fichiers PDFs et Harvest ==="

$pdfs = Get-ChildItem -Path . -Filter "*.pdf" -ErrorAction SilentlyContinue
$harvests = Get-ChildItem -Path . -Filter "*.harvest.json" -ErrorAction SilentlyContinue

if ($pdfs.Count -gt 0) {
    Write-Host "PDFs trouvés: $($pdfs.Count)"
    $pdfs | ForEach-Object { 
        Write-Host "  Suppression: $($_.Name)"
        Remove-Item $_.FullName 
    }
} else {
    Write-Host "Aucun PDF trouvé"
}

if ($harvests.Count -gt 0) {
    Write-Host "Harvests trouvés: $($harvests.Count)"
    $harvests | ForEach-Object { 
        Write-Host "  Suppression: $($_.Name)"
        Remove-Item $_.FullName 
    }
} else {
    Write-Host "Aucun harvest trouvé"
}

Write-Host "✅ Nettoyage terminé"