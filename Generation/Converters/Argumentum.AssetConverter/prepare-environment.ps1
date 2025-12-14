# Script de préparation de l'environnement pour la génération complète des PDFs
# Date: 2025-10-16

Write-Host "=== PRÉPARATION DE L'ENVIRONNEMENT ===" -ForegroundColor Cyan
Write-Host ""

# 1. Vérifier le serveur CardPen
Write-Host "1. Vérification du serveur CardPen..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5258/index.html" -UseBasicParsing -TimeoutSec 5 -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host "   ✅ Serveur CardPen actif et répond correctement" -ForegroundColor Green
    }
} catch {
    Write-Host "   ❌ Serveur CardPen inactif ou ne répond pas" -ForegroundColor Red
    Write-Host "   Veuillez démarrer le serveur manuellement:" -ForegroundColor Yellow
    Write-Host "   cd Generation/CardPen/server" -ForegroundColor White
    Write-Host "   dotnet run" -ForegroundColor White
    Write-Host ""
    $continue = Read-Host "   Appuyez sur Entrée quand le serveur est démarré (ou Q pour quitter)"
    if ($continue -eq "Q") {
        exit 1
    }
}

Write-Host ""

# 2. Nettoyer les anciens fichiers
Write-Host "2. Nettoyage des anciens fichiers..." -ForegroundColor Yellow

# PDFs
$pdfs = Get-ChildItem "*.pdf" -ErrorAction SilentlyContinue
if ($pdfs.Count -gt 0) {
    Write-Host "   Suppression de $($pdfs.Count) fichiers PDF..." -ForegroundColor Gray
    Remove-Item "*.pdf" -ErrorAction SilentlyContinue
    Write-Host "   ✅ PDFs supprimés" -ForegroundColor Green
} else {
    Write-Host "   ℹ️ Aucun PDF à supprimer" -ForegroundColor Gray
}

# Fichiers Harvest
$harvests = Get-ChildItem "*.harvest.json" -Recurse -ErrorAction SilentlyContinue
if ($harvests.Count -gt 0) {
    Write-Host "   Suppression de $($harvests.Count) fichiers harvest..." -ForegroundColor Gray
    Remove-Item "*.harvest.json" -Recurse -ErrorAction SilentlyContinue
    Write-Host "   ✅ Fichiers harvest supprimés" -ForegroundColor Green
} else {
    Write-Host "   ℹ️ Aucun fichier harvest à supprimer" -ForegroundColor Gray
}

# Répertoire Harvest
if (Test-Path "Harvest") {
    Write-Host "   Nettoyage du répertoire Harvest..." -ForegroundColor Gray
    Remove-Item "Harvest\*" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "   ✅ Répertoire Harvest nettoyé" -ForegroundColor Green
}

# Répertoire Images
if (Test-Path "Images") {
    Write-Host "   Nettoyage du répertoire Images..." -ForegroundColor Gray
    Remove-Item "Images\*" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "   ✅ Répertoire Images nettoyé" -ForegroundColor Green
}

# Répertoire Documents
if (Test-Path "Documents") {
    Write-Host "   Nettoyage du répertoire Documents..." -ForegroundColor Gray
    Remove-Item "Documents\*" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "   ✅ Répertoire Documents nettoyé" -ForegroundColor Green
}

Write-Host ""
Write-Host "=== ENVIRONNEMENT PRÊT ===" -ForegroundColor Green
Write-Host ""