# Script de nettoyage des PDFs et harvests generes
#
# ATTENTION (#1133) — les harvests s'appellent "<CardSet>_harvest_<lang>.json",
# PAS "*.harvest.json". L'ancien filtre "*.harvest.json" ne matchait AUCUN fichier
# et affichait "Aucun harvest trouve" + "Nettoyage termine" : un no-op qui rend vert.
# Le nom reel vient de CardSetConfig.GetHarvestSerializationName (CardSetConfig.cs:26-29).
# Ce script echoue maintenant bruyamment si la suppression ne supprime rien.

param(
    # Doit correspondre au build qui a genere les fichiers (Debug ou Release).
    [ValidateSet("Debug", "Release")]
    [string]$Config = "Release",
    # Racine explicite ; par defaut l'arbre Target du build choisi.
    [string]$Root
)

if (-not $Root) {
    $Root = Join-Path $PSScriptRoot "bin/$Config/net9.0-windows/Target"
}

Write-Host "=== Nettoyage des fichiers PDFs et Harvest ==="
Write-Host "Racine: $Root"

if (-not (Test-Path $Root)) {
    Write-Host "FATAL: racine introuvable — mauvais -Config ($Config) ou build jamais lance." -ForegroundColor Red
    exit 1
}

$pdfs     = @(Get-ChildItem -Path $Root -Recurse -Filter "*.pdf" -ErrorAction SilentlyContinue)
$harvests = @(Get-ChildItem -Path $Root -Recurse -Filter "*_harvest_*.json" -ErrorAction SilentlyContinue)

Write-Host "PDFs trouves: $($pdfs.Count) | Harvests trouves: $($harvests.Count)"

$pdfs     | ForEach-Object { Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue }
$harvests | ForEach-Object { Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue }

# Assertion sur l'EFFET, pas sur l'execution : un filtre mort retourne 0 sans erreur.
$afterPdfs     = @(Get-ChildItem -Path $Root -Recurse -Filter "*.pdf" -ErrorAction SilentlyContinue).Count
$afterHarvests = @(Get-ChildItem -Path $Root -Recurse -Filter "*_harvest_*.json" -ErrorAction SilentlyContinue).Count

if ($harvests.Count -gt 0 -and $afterHarvests -eq $harvests.Count) {
    Write-Host "FATAL: $($harvests.Count) harvests trouves, 0 supprime — filtre ou droits casses. STOP." -ForegroundColor Red
    exit 1
}
if ($pdfs.Count -gt 0 -and $afterPdfs -eq $pdfs.Count) {
    Write-Host "FATAL: $($pdfs.Count) PDFs trouves, 0 supprime — filtre ou droits casses. STOP." -ForegroundColor Red
    exit 1
}

Write-Host "OK: harvests $($harvests.Count) -> $afterHarvests | PDFs $($pdfs.Count) -> $afterPdfs" -ForegroundColor Green
