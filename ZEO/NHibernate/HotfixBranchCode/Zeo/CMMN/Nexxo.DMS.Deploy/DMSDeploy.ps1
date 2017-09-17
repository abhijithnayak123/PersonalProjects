#Parameters - This is used only when we want to deploy integeration test, we can further fine tune the script to deploy anywhere.
param(
        [Parameter(Mandatory=$false)]
        [string]$IsIntegerationTest
    )

#Functions 
#==================================================================================
function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    $path = Split-Path $Invocation.MyCommand.Path
    return ("{0}\" -f $path)
}
#==================================================================================

#Declaration Section
#==================================================================================
$scriptDir = (Get-ScriptDirectory)
#==================================================================================

#begin script
#==================================================================================
& "$scriptDir\DBDeploy.ps1"| Write-Host
if ($LastExitCode -ne 0) {
     exit $LastExitCode
}

& "$scriptDir\MonitorDeploy.ps1" -IsIntegerationTest $IsIntegerationTest | Write-Host
if ($LastExitCode -ne 0) {
     exit $LastExitCode
}

& "$scriptDir\WebDeploy.ps1" -IsIntegerationTest $IsIntegerationTest | Write-Host

if ($LastExitCode -ne 0) {
     exit $LastExitCode
}
#==================================================================================