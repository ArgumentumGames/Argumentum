# Setup-CardPenHost.ps1
# Configure l'entrée hosts pour argumentum.myia.io
# REQUIERT: Exécution en tant qu'Administrateur

param(
    [switch]$Remove,
    [switch]$Check
)

$hostsPath = "C:\Windows\System32\drivers\etc\hosts"
$hostEntry = "127.0.0.1 argumentum.myia.io"

function Test-Admin {
    $currentUser = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object Security.Principal.WindowsPrincipal($currentUser)
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Get-HostEntry {
    $content = Get-Content $hostsPath -ErrorAction SilentlyContinue
    return $content | Where-Object { $_ -match "argumentum\.myia\.io" }
}

# Mode vérification
if ($Check) {
    $existing = Get-HostEntry
    if ($existing) {
        Write-Host "[OK] Entrée existante: $existing" -ForegroundColor Green
    } else {
        Write-Host "[!] Aucune entrée pour argumentum.myia.io" -ForegroundColor Yellow
    }
    exit 0
}

# Vérifier les droits admin
if (-not (Test-Admin)) {
    Write-Host "[ERREUR] Ce script nécessite des droits Administrateur." -ForegroundColor Red
    Write-Host "Relancez PowerShell en tant qu'Administrateur puis exécutez ce script." -ForegroundColor Yellow
    exit 1
}

# Mode suppression
if ($Remove) {
    $content = Get-Content $hostsPath
    $newContent = $content | Where-Object { $_ -notmatch "argumentum\.myia\.io" }
    Set-Content -Path $hostsPath -Value $newContent
    Write-Host "[OK] Entrée argumentum.myia.io supprimée." -ForegroundColor Green
    exit 0
}

# Mode ajout
$existing = Get-HostEntry
if ($existing) {
    Write-Host "[OK] Entrée déjà présente: $existing" -ForegroundColor Green
    exit 0
}

# Ajouter l'entrée
Add-Content -Path $hostsPath -Value "`n$hostEntry"
Write-Host "[OK] Entrée ajoutée: $hostEntry" -ForegroundColor Green
Write-Host ""
Write-Host "Pour démarrer le serveur CardPen:" -ForegroundColor Cyan
Write-Host "  cd Generation/CardPen/server" -ForegroundColor White
Write-Host "  dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "CardPen sera accessible sur: http://argumentum.myia.io:5258" -ForegroundColor Cyan
