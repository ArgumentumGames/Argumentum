# Script d'exécution complète du pipeline PDF Argumentum
# Date: 2025-10-16
# Objectif: Générer les 4 PDFs critiques et capturer tous les logs

$ErrorActionPreference = "Continue"

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "   GÉNÉRATION COMPLÈTE DES PDFs ARGUMENTUM" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

# Timestamp de démarrage
$startTime = Get-Date
$logFileName = "generation-complete-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"

Write-Host "📅 Heure de démarrage: $($startTime.ToString('yyyy-MM-dd HH:mm:ss'))" -ForegroundColor Yellow
Write-Host "📝 Fichier de log: $logFileName" -ForegroundColor Yellow
Write-Host ""

Write-Host "🎯 PDFs cibles:" -ForegroundColor Green
Write-Host "   1. Argumentum_TarotCards_fr.pdf" -ForegroundColor White
Write-Host "   2. Argumentum_TarotCards_Print&Play_A4_fr.pdf" -ForegroundColor White
Write-Host "   3. Argumentum_PokerCards_fr.pdf" -ForegroundColor White
Write-Host "   4. Argumentum_PokerCards_Print&Play_A4_fr.pdf" -ForegroundColor White
Write-Host ""

Write-Host "⚙️ Configuration:" -ForegroundColor Yellow
Write-Host "   - Mode: WebBasedImageGeneration + QuestPdfGeneration" -ForegroundColor White
Write-Host "   - Browser: Headless (mode silencieux)" -ForegroundColor White
Write-Host "   - Serveur CardPen: http://localhost:5258" -ForegroundColor White
Write-Host ""

Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "   DÉBUT DE L'EXÉCUTION" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

# Lancer la génération avec capture complète des logs
try {
    Write-Host "▶️ Lancement de dotnet run..." -ForegroundColor Green
    Write-Host ""
    
    # Exécuter et capturer tous les streams
    $process = Start-Process -FilePath "dotnet" -ArgumentList "run" -NoNewWindow -PassThru -Wait -RedirectStandardOutput "stdout.tmp" -RedirectStandardError "stderr.tmp"
    
    # Lire et afficher la sortie
    if (Test-Path "stdout.tmp") {
        $stdout = Get-Content "stdout.tmp" -Raw
        Write-Host $stdout
        $stdout | Out-File -FilePath $logFileName -Encoding UTF8
        Remove-Item "stdout.tmp" -Force
    }
    
    # Lire et afficher les erreurs
    if (Test-Path "stderr.tmp") {
        $stderr = Get-Content "stderr.tmp" -Raw
        if ($stderr) {
            Write-Host $stderr -ForegroundColor Red
            $stderr | Out-File -FilePath $logFileName -Append -Encoding UTF8
        }
        Remove-Item "stderr.tmp" -Force
    }
    
    $exitCode = $process.ExitCode
    
} catch {
    Write-Host "❌ ERREUR lors de l'exécution:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    $exitCode = 1
}

Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "   FIN DE L'EXÉCUTION" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

# Timestamp de fin
$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host "📅 Heure de fin: $($endTime.ToString('yyyy-MM-dd HH:mm:ss'))" -ForegroundColor Yellow
Write-Host "⏱️ Durée totale: $([math]::Floor($duration.TotalMinutes)) minutes $($duration.Seconds) secondes" -ForegroundColor Yellow
Write-Host "🔢 Code de sortie: $exitCode" -ForegroundColor $(if ($exitCode -eq 0) { "Green" } else { "Red" })
Write-Host ""

# Résumé dans le fichier log
$summary = @"

================================================================
RÉSUMÉ DE L'EXÉCUTION
================================================================
Heure de début: $($startTime.ToString('yyyy-MM-dd HH:mm:ss'))
Heure de fin: $($endTime.ToString('yyyy-MM-dd HH:mm:ss'))
Durée totale: $([math]::Floor($duration.TotalMinutes)) minutes $($duration.Seconds) secondes
Code de sortie: $exitCode
================================================================
"@

$summary | Out-File -FilePath $logFileName -Append -Encoding UTF8

Write-Host "✅ Logs sauvegardés dans: $logFileName" -ForegroundColor Green
Write-Host ""

if ($exitCode -eq 0) {
    Write-Host "🎉 GÉNÉRATION TERMINÉE AVEC SUCCÈS!" -ForegroundColor Green
} else {
    Write-Host "⚠️ GÉNÉRATION TERMINÉE AVEC DES ERREURS (Code: $exitCode)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Fin du script." -ForegroundColor Gray