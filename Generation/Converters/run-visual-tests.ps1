<#
.SYNOPSIS
    Runs the .NET visual tests for the converter projects.
.DESCRIPTION
    This script builds and runs tests for the Argumentum.AssetConverter.VisualTests project.
    It includes safeguards against process deadlocks and file locking issues.
.PARAMETER TestProject
    The path to the .csproj file of the test project to run.
.EXAMPLE
    .\run-visual-tests.ps1
    Runs the tests for Argumentum.AssetConverter.VisualTests.
#>
[CmdletBinding()]
param (
    [string]$TestProjectRelative = "Argumentum.AssetConverter.VisualTests/Argumentum.AssetConverter.VisualTests.csproj"
)

$TestProject = Join-Path -Path $PSScriptRoot -ChildPath $TestProjectRelative

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# Forcefully stop any lingering testhost processes to prevent file locking issues.
Write-Host "Checking for lingering test host processes..."
$lingeringTesthosts = Get-Process testhost -ErrorAction SilentlyContinue
if ($lingeringTesthosts) {
    Write-Warning "Found and stopping lingering testhost processes to prevent file locks..."
    $lingeringTesthosts | Stop-Process -Force
    Write-Host "Lingering processes stopped."
}

# Create a directory for test results if it doesn't exist
$testResultDir = Join-Path -Path $PSScriptRoot -ChildPath "TestResults"
if (-not (Test-Path -Path $testResultDir)) {
    New-Item -ItemType Directory -Path $testResultDir | Out-Null
}

$logFileName = "VisualTestResults-$(Get-Date -Format 'yyyyMMdd_HHmmss').trx"
$logFilePath = Join-Path -Path $testResultDir -ChildPath $logFileName
$loggerArg = "--logger ""trx;LogFileName=$logFilePath"""

# Build the project first
Write-Host "Building project: $TestProject"
dotnet build $TestProject

# Run tests directly
Write-Host "Running tests for project: $TestProject (no build)..."
$arguments = "test ""$TestProject"" --no-build $loggerArg"
$testProcess = Start-Process dotnet -ArgumentList $arguments -PassThru
$timeoutSeconds = 600
$completed = $testProcess.WaitForExit($timeoutSeconds * 1000)

if (-not $completed) {
    Write-Error "Test execution timed out after $timeoutSeconds seconds. Terminating process."
    Stop-Process -Id $testProcess.Id -Force
    exit 1
} else {
    Write-Host "Test execution completed within the time limit."
    Write-Host "Test results logged to $testResultDir\$logFileName"
    if ($testProcess.ExitCode -ne 0) {
        Write-Error "Tests failed with exit code $($testProcess.ExitCode)."
        exit $testProcess.ExitCode
    }
}