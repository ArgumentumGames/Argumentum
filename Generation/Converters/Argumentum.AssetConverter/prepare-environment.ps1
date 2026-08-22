# Script de préparation de l'environnement pour la génération complète des PDFs
# Date: 2025-10-16 — revu 2026-08-22 (#1133 suite)
#
# Deux pannes muettes corrigées ici :
#  - le filtre harvest "*.harvest.json" ne matchait AUCUN fichier (nom reel :
#    "<CardSet>_harvest_<lang>.json") et affichait "Aucun fichier harvest a supprimer" ;
#  - la sonde CardPen visait http://localhost:5258, que le pipeline n'utilise jamais :
#    CardpenUrl => UseLocalCardpen ? LocalCardpenUrl : ReleaseCardpenUrl
#    (WebBasedGeneratorConfig.cs:90), avec UseLocalCardpen = true en dur (:84).
# Ce script echoue maintenant bruyamment plutot que de rassurer a tort.

param(
    # Doit correspondre au build qui sera lance ensuite (Debug ou Release).
    [ValidateSet("Debug", "Release")]
    [string]$Config = "Release"
)

$ErrorActionPreference = "Stop"
$cardpenUrl = "https://argumentum.myia.io"                              # ce que le pipeline consomme
$masterUrl  = "https://argumentumgames.github.io/Argumentum"            # deploye depuis master par static.yml

function Get-Sha256([string]$text) {
    $stream = [IO.MemoryStream]::new([Text.Encoding]::UTF8.GetBytes($text))
    return (Get-FileHash -InputStream $stream -Algorithm SHA256).Hash
}

Write-Host "=== PRÉPARATION DE L'ENVIRONNEMENT ($Config) ===" -ForegroundColor Cyan
Write-Host ""

# 1. Vérifier le moteur CardPen — joignable ET a jour vis-a-vis de MASTER
#
# ⚠️ La reference est DISTANTE (GitHub Pages = master), pas l'arbre local.
# Sur la machine qui heberge l'IIS, celui-ci sert ce meme checkout : comparer
# "servi" a "local" y serait toujours vrai — un garde-fou incapable d'echouer,
# c'est-a-dire exactement la panne corrigee par #1133.
Write-Host "1. Vérification du moteur CardPen ($cardpenUrl)..." -ForegroundColor Yellow
try {
    $served = (Invoke-WebRequest -Uri "$cardpenUrl/js/frame.js" -UseBasicParsing -TimeoutSec 20).Content
    Write-Host "   ✅ Moteur CardPen joignable" -ForegroundColor Green
} catch {
    Write-Host "   ❌ FATAL: moteur CardPen injoignable — $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "      Le pipeline ne peut pas moissonner sans lui. STOP." -ForegroundColor Red
    exit 1
}

try {
    $master = (Invoke-WebRequest -Uri "$masterUrl/js/frame.js" -UseBasicParsing -TimeoutSec 20).Content
} catch {
    Write-Host "   ⚠️ Reference master (GitHub Pages) injoignable — fraicheur NON verifiee." -ForegroundColor Yellow
    Write-Host "      Ne pas conclure 'a jour' : verifier a la main avant une regen de release." -ForegroundColor Yellow
    $master = $null
}

if ($master) {
    $servedHash = Get-Sha256 $served
    $masterHash = Get-Sha256 $master
    if ($servedHash -ne $masterHash) {
        Write-Host "   ❌ FATAL: le frame.js servi est PERIME par rapport a master." -ForegroundColor Red
        Write-Host "      servi=$($servedHash.Substring(0,16))  master=$($masterHash.Substring(0,16))" -ForegroundColor Red
        Write-Host "      Un template a jour rendu par un moteur perime produit une carte fausse" -ForegroundColor Red
        Write-Host "      SANS aucune erreur (incident #1127/#1130, 2026-08-22)." -ForegroundColor Red
        Write-Host "      => 'git pull' sur le checkout servi par l'IIS, puis relancer. STOP." -ForegroundColor Red
        exit 1
    }
    Write-Host "   ✅ Moteur a jour vs master (sha $($masterHash.Substring(0,16)))" -ForegroundColor Green
}

Write-Host ""

# 2. Nettoyer les anciens fichiers
#
# ⚠️ Racine EXPLICITE. L'ancienne version utilisait des chemins relatifs au repertoire
# courant ("*.pdf", Test-Path "Harvest") : lancee d'ailleurs que le dossier de sortie,
# elle ne nettoyait rien tout en affichant des ✅.
$target = Join-Path $PSScriptRoot "bin/$Config/net9.0-windows/Target"
Write-Host "2. Nettoyage des anciens fichiers ($target)..." -ForegroundColor Yellow

if (-not (Test-Path $target)) {
    Write-Host "   ℹ️ Arbre de sortie absent — rien a nettoyer (premier run, ou mauvais -Config)." -ForegroundColor Gray
} else {
    # Le nom reel des harvests est "<CardSet>_harvest_<lang>.json"
    # (CardSetConfig.GetHarvestSerializationName, CardSetConfig.cs:26-29).
    $pdfs     = @(Get-ChildItem -Path $target -Recurse -Filter "*.pdf"             -ErrorAction SilentlyContinue)
    $harvests = @(Get-ChildItem -Path $target -Recurse -Filter "*_harvest_*.json"  -ErrorAction SilentlyContinue)
    $images   = @(Get-ChildItem -Path $target -Recurse -Filter "*.png"             -ErrorAction SilentlyContinue)

    Write-Host "   Trouves — PDFs: $($pdfs.Count) | harvests: $($harvests.Count) | images: $($images.Count)" -ForegroundColor Gray

    $pdfs     | ForEach-Object { Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue }
    $harvests | ForEach-Object { Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue }
    $images   | ForEach-Object { Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue }

    # Assertion sur l'EFFET : un filtre mort supprime 0 fichier sans lever d'erreur.
    $afterH = @(Get-ChildItem -Path $target -Recurse -Filter "*_harvest_*.json" -ErrorAction SilentlyContinue).Count
    if ($harvests.Count -gt 0 -and $afterH -eq $harvests.Count) {
        Write-Host "   ❌ FATAL: $($harvests.Count) harvests trouves, 0 supprime. STOP." -ForegroundColor Red
        exit 1
    }
    Write-Host "   ✅ Nettoyage effectif — harvests $($harvests.Count) -> $afterH" -ForegroundColor Green
}

Write-Host ""
Write-Host "=== ENVIRONNEMENT PRÊT ===" -ForegroundColor Green
Write-Host ""