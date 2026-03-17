#Requires -Version 5.1
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

try {
    # Déterminer les chemins dynamiquement
    $scriptPath = $PSScriptRoot
    $basePath = (Resolve-Path (Join-Path $scriptPath "..\..\..\")).Path
    $inputFile = Join-Path $basePath "Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv"
    $tempFile = Join-Path $basePath "Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv.temp"
    
    Write-Host "Chemin de base détecté : $basePath"
    Write-Host "Fichier d'entrée : $inputFile"

    if (-not (Test-Path $inputFile)) {
        throw "Le fichier d'entrée '$inputFile' n'a pas été trouvé."
    }

    # Lire le fichier CSV avec Import-Csv et le réécrire immédiatement.
    # Cela forcera la correction de la structure selon la norme RFC 4180.
    # L'option -UseCulture est importante pour s'assurer que le délimiteur est bien la virgule.
    Write-Host "Lecture et normalisation du fichier CSV..."
    $csvData = Import-Csv -Path $inputFile -UseCulture ([System.Globalization.CultureInfo]::InvariantCulture)
    
    Write-Host "Écriture du fichier CSV normalisé dans un fichier temporaire..."
    $csvData | Export-Csv -Path $tempFile -NoTypeInformation -Encoding UTF8 -UseCulture ([System.Globalization.CultureInfo]::InvariantCulture)

    Write-Host "Remplacement de l'ancien fichier par le fichier nettoyé."
    Move-Item -Path $tempFile -Destination $inputFile -Force
    
    Write-Host "Opération terminée avec succès."

} catch {
    Write-Error "Une erreur est survenue : $_"
    # Afficher plus de détails sur l'erreur de parsing si possible
    if ($_.Exception -is [Microsoft.PowerShell.Commands.ImportCsvCommand.ImportCsvException]) {
        Write-Error "Détails de l'erreur CSV : $($_.Exception.Message)"
    }
    exit 1
}