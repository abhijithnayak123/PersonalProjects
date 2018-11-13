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

#Build and deploy DMS
& "$scriptDir\DMSDeploy.ps1"| Write-Host
if ($LastExitCode -ne 0) {
     exit $LastExitCode
}

#System Test
& "$scriptDir\SystemTest.ps1"
if ($LastExitCode -ne 0) {
     exit $LastExitCode
}


#==================================================================================