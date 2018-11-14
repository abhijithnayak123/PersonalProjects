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
function Get-BaseDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    $path = Split-Path $Invocation.MyCommand.Path
	$basepath = Split-Path -parent $path
	$basepath = Split-Path -parent $basepath
    return ("{0}\" -f $basepath)
}
#==================================================================================

#Declaration Section
#==================================================================================
$scriptDir = (Get-ScriptDirectory)
$base_path = (Get-BaseDirectory);
#$nunit_console_path = $base_path + 'Packages\NUnit.2.5.10.11092\tools\nunit-console.exe';
$nunit_console_path = 'C:\Program` Files\NUnit` 2.6.4\bin\nunit-console.exe';
$nunit_dll_path = $base_path + 'CMMN\Nexxo.DMS.Deploy\DMS\MGI.Unit.Test\bin\MGI.Unit.Test.dll';
#==================================================================================

#begin script
#==================================================================================
& "$scriptDir\TestDeploy.ps1"| Write-Host
if ($LastExitCode -ne 0) {
     exit $LastExitCode
}
#==================================================================================

#Run Integration test cases, these will still run only on OrdDevWeb1 only.
#We need a better way for this, while the idea is great, we cannot keep adding the test cases as a list in the ps file. May be a sequencing might help? 
#==================================================================================
	$testcasecmd = $nunit_console_path + ' ' + $nunit_dll_path
	Invoke-Expression $testcasecmd
#==================================================================================