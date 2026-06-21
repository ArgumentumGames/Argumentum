<#
.SYNOPSIS
    Make the current Windows session a persistent interactive desktop so that
    FreeMind SVG export (driven by SendKeys) keeps working even when the RDP
    client is minimized or disconnected.

.DESCRIPTION
    Root cause: FreeMind has no automation API; its SVG export is triggered by
    keyboard macros that require a real foreground window on an interactive
    desktop (WinSta0\Default). When the RDP client is minimized/idle, the
    desktop parks its focus, GetForegroundWindow() returns null, and the
    keystrokes drop -> "FreeMind SVG not detected".

    This script applies the SAFE, reversible OS-level hardening (disable
    screensaver / inactivity lock / monitor & system sleep) so the desktop
    stays awake and keeps a foreground. It then PRINTS the two options to keep
    the desktop interactive after an RDP disconnect:

      C1 (recommended, no credentials): park the session to the physical
         console with `tscon <id> /dest:console`. Use -ParkToConsole to run it
         (WARNING: it disconnects your RDP client immediately).
      C2 (needs stored credentials -> jsboige consent): autologon + auto-start.

    See docs/investigations/2026-06-21-mindmap-reliability-without-rdp.md.

.PARAMETER ParkToConsole
    Redirect the current session to the physical console (tscon /dest:console).
    WARNING: this DISCONNECTS your current RDP client. The session keeps running
    on the console with a valid interactive desktop. Reconnect later to collect
    the generated SVGs.

.PARAMETER Revert
    Re-enable the screensaver and restore reasonable sleep timeouts.

.EXAMPLE
    pwsh -File Enable-MindmapInteractiveSession.ps1
    # Apply safe hardening and print the C1/C2 instructions.

.EXAMPLE
    pwsh -File Enable-MindmapInteractiveSession.ps1 -ParkToConsole
    # Apply hardening, then park the session to the console (disconnects RDP).

.EXAMPLE
    pwsh -File Enable-MindmapInteractiveSession.ps1 -Revert
    # Restore screensaver + sleep timeouts.
#>
[CmdletBinding()]
param(
    [switch]$ParkToConsole,
    [switch]$Revert
)

$ErrorActionPreference = 'Stop'

function Test-Admin {
    $id = [System.Security.Principal.WindowsIdentity]::GetCurrent()
    (New-Object System.Security.Principal.WindowsPrincipal($id)).IsInRole(
        [System.Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Set-RegValue {
    param($Path, $Name, $Value, $Type = 'DWord')
    if (-not (Test-Path $Path)) { New-Item -Path $Path -Force | Out-Null }
    New-ItemProperty -Path $Path -Name $Name -Value $Value -PropertyType $Type -Force | Out-Null
    Write-Host ("  [reg] {0}\{1} = {2}" -f $Path, $Name, $Value)
}

$sid = (Get-Process -Id $PID).SessionId

# ---------------------------------------------------------------------------
# REVERT
# ---------------------------------------------------------------------------
if ($Revert) {
    Write-Host "== Revert: restoring screensaver + sleep timeouts ==" -ForegroundColor Cyan
    Set-RegValue 'HKCU:\Control Panel\Desktop' 'ScreenSaveActive' '1' 'String'
    try { powercfg /change monitor-timeout-ac 10 | Out-Null; Write-Host "  [powercfg] monitor-timeout-ac = 10" } catch { Write-Warning "powercfg monitor failed: $_" }
    try { powercfg /change standby-timeout-ac 30 | Out-Null; Write-Host "  [powercfg] standby-timeout-ac = 30" } catch { Write-Warning "powercfg standby failed: $_" }
    if (Test-Admin) {
        try { Set-RegValue 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System' 'InactivityTimeoutSecs' 900 } catch { Write-Warning $_ }
    } else {
        Write-Warning "Not admin: skipped InactivityTimeoutSecs (machine lock policy)."
    }
    Write-Host "Done. Screensaver re-enabled; sleep timeouts restored." -ForegroundColor Green
    return
}

# ---------------------------------------------------------------------------
# APPLY — safe, reversible hardening
# ---------------------------------------------------------------------------
Write-Host "== Step A: disable screensaver / inactivity lock ==" -ForegroundColor Cyan
Set-RegValue 'HKCU:\Control Panel\Desktop' 'ScreenSaveActive'   '0' 'String'
Set-RegValue 'HKCU:\Control Panel\Desktop' 'ScreenSaverIsSecure' '0' 'String'
if (Test-Admin) {
    Set-RegValue 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System' 'InactivityTimeoutSecs' 0
} else {
    Write-Warning "Not admin: skipped InactivityTimeoutSecs=0 (machine inactivity lock). Re-run elevated if the console still auto-locks."
}

Write-Host "== Step B: disable monitor & system sleep (AC) ==" -ForegroundColor Cyan
try { powercfg /change monitor-timeout-ac 0 | Out-Null; Write-Host "  [powercfg] monitor-timeout-ac = 0" } catch { Write-Warning "powercfg monitor failed: $_" }
try { powercfg /change standby-timeout-ac 0 | Out-Null; Write-Host "  [powercfg] standby-timeout-ac = 0" } catch { Write-Warning "powercfg standby failed: $_" }

Write-Host ""
Write-Host "Safe hardening applied. Current session id = $sid" -ForegroundColor Green

# ---------------------------------------------------------------------------
# Step C — keep the desktop interactive after RDP disconnect
# ---------------------------------------------------------------------------
if ($ParkToConsole) {
    Write-Host ""
    Write-Warning "About to run: tscon $sid /dest:console"
    Write-Warning "This DISCONNECTS your RDP client NOW. The session keeps running on"
    Write-Warning "the physical console with a valid interactive desktop. Reconnect later."
    $ans = Read-Host "Type YES to proceed (anything else aborts)"
    if ($ans -eq 'YES') {
        tscon $sid /dest:console
        # If tscon succeeded, the RDP link is already gone; nothing else runs here.
    } else {
        Write-Host "Aborted. No session redirect performed." -ForegroundColor Yellow
    }
    return
}

Write-Host ""
Write-Host "== Step C (manual) — keep the desktop interactive after disconnect ==" -ForegroundColor Cyan
Write-Host @"
  C1 (recommended, no credentials):
      Start the regen, then park this session to the console BEFORE disconnecting:
          tscon $sid /dest:console
      (or re-run this script with -ParkToConsole). It disconnects your RDP client;
      the session keeps running with a real foreground so FreeMind keystrokes land.

  C2 (needs stored credentials -> jsboige consent):
      Autologon + a 'run at logon' scheduled task that launches Mode=Mindmapper.
      Prefer Sysinternals Autologon.exe (LSA-encrypted) over plaintext
      HKLM\...\Winlogon\DefaultPassword. DO NOT apply unilaterally.
"@
Write-Host "Next: set ARGUMENTUM_FREEMIND_PATH, run ONE 'dotnet run' (Mode=Mindmapper) at a time." -ForegroundColor Green
