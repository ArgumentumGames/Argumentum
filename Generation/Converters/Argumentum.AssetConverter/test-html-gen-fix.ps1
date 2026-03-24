#!/usr/bin/env pwsh
#Requires -Version 7.0

Write-Host "`n=== TEST CORRECTION BUG GÉNÉRATION HTML ===" -ForegroundColor Cyan
Write-Host "Objectif: Vérifier que CardPen génère TOUTES les cartes, pas seulement la première`n" -ForegroundColor Yellow

Set-Location "Generation/Converters/Argumentum.AssetConverter"

# Nettoyer les anciens harvests
Write-Host "[NETTOYAGE] Suppression des anciens fichiers harvest..." -ForegroundColor Gray
Remove-Item "*.harvest.json" -ErrorAction SilentlyContinue
Write-Host "✓ Nettoyage effectué`n" -ForegroundColor Green

# Tester avec 3 cartes Rules
Write-Host "[TEST MINIMAL] Génération avec 3 cartes Rules..." -ForegroundColor Cyan
$result = dotnet run --config AssetConverterConfig.test-js-fix.json 2>&1 | Tee-Object -FilePath "test-html-gen-fix.log"

# Analyser les résultats
Write-Host "`n[ANALYSE] Vérification des résultats..." -ForegroundColor Cyan

$harvests = Get-ChildItem "*.harvest.json" -ErrorAction SilentlyContinue
if ($harvests.Count -gt 0) {
    Write-Host "✅ SUCCESS: $($harvests.Count) fichier(s) harvest généré(s)" -ForegroundColor Green
    
    foreach ($harvest in $harvests) {
        $content = Get-Content $harvest.FullName | ConvertFrom-Json
        $imageCount = $content.Count
        
        Write-Host "`n📦 Harvest: $($harvest.Name)" -ForegroundColor White
        Write-Host "   Nombre d'images: $imageCount" -ForegroundColor $(if ($imageCount -eq 3) { "Green" } else { "Red" })
        
        if ($imageCount -eq 3) {
            Write-Host "   ✅ CONFORME (attendu: 3 images)" -ForegroundColor Green
        } else {
            Write-Host "   ❌ NON CONFORME (attendu: 3, obtenu: $imageCount)" -ForegroundColor Red
        }
    }
} else {
    Write-Host "❌ ÉCHEC: Aucun fichier harvest généré" -ForegroundColor Red
    Write-Host "`nVoir les logs dans: test-html-gen-fix.log" -ForegroundColor Yellow
}

# Rechercher erreurs spécifiques dans les logs
Write-Host "`n[LOGS] Recherche d'erreurs critiques..." -ForegroundColor Cyan
$logContent = Get-Content "test-html-gen-fix.log" -Raw

if ($logContent -match "Mismatch between generated image count") {
    Write-Host "❌ ERREUR: Bug de mismatch encore présent!" -ForegroundColor Red
    $logContent -split "`n" | Where-Object { $_ -match "Mismatch|Expecting" } | ForEach-Object {
        Write-Host "   $_" -ForegroundColor Red
    }
} elseif ($logContent -match "undefined\.replace") {
    Write-Host "❌ ERREUR: Bug markdown réintroduit!" -ForegroundColor Red
} elseif ($logContent -match "ApplicationException|Error:") {
    Write-Host "⚠️ WARNING: Erreurs détectées dans les logs" -ForegroundColor Yellow
} else {
    Write-Host "✅ Aucune erreur critique détectée" -ForegroundColor Green
}

Write-Host "`n=== FIN DU TEST ===" -ForegroundColor Cyan