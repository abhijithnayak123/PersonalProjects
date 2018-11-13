#Functions
#==================================================================================
function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    $path = Split-Path $Invocation.MyCommand.Path
    return ("{0}\" -f $path)
}

# prepare the msbuild command
function Get-MSBuildCommandArgs(){
    param(	
		[Parameter(Mandatory=$true)]
        [string]
        $ProjectRootPath,
		
		[Parameter(Mandatory=$true)]
        [string]
        $Project
    )

    $cmdArgs = @()
    $cmdArgs += ("{0}NugetPackAndPush.proj" -f $scriptDir)
	$cmdArgs +=('/p:WORKSPACE={0}' -f $workspace)
	$cmdArgs +=('/p:ProjectRootPath={0}' -f $ProjectRootPath)
	$cmdArgs +=('/p:Project={0}' -f $Project)
    return $cmdArgs
}

function Get-MSBuildPath(){
    return "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
}
#==================================================================================

#Declaration Section
#==================================================================================
$scriptDir = (Get-ScriptDirectory)

$msbuildCmd = (Get-MSBuildPath)
$DMSConfigFile= ("{0}DMS.Config" -f $scriptDir)
[xml]$DMSConfig = Get-Content $DMSConfigFile
$workspace= '..\..\'
#==================================================================================

#begin script
#==================================================================================
#pack and push Database/exe application
foreach($proj in $DMSConfig.DMS.DBProjects.Project)
{
	$nodeExists = $proj.Nugetpush 
    if (($nodeExists -eq $null) -or ($nodeExists.tolower() -eq 'true')) 
	{
		"Nuget Pack and PUSH {0} is Started.." -f $proj.name | Write-Host
		Write-Host "--------------------------------------------------------"
		$allArgs  = (Get-MSBuildCommandArgs -ProjectRootPath $proj.RootPath -Project $proj.name) 
		$allArgs +=('/p:OutputDirectory={0}\DMS\Database\' -f $scriptDir)
		"Calling msbuild.exe with the following parameters. [$msbuildCmd] [$allArgs] " | Write-Host
		& $msbuildCmd $allArgs
		if ($LastExitCode -ne 0) {
		 exit $LastExitCode
		}
		"Nuget Pack and PUSH {0} is completed.." -f $proj.name | Write-Host 
		Write-Host "--------------------------------------------------------"
    }
    else {
		Write-Host "--------------------------------------------------------"
		"The package {0} is not pushed to nuget.." -f $proj.name | Write-Host 
		Write-Host "--------------------------------------------------------"
    }
}
#pack and push web application
foreach($app in $DMSConfig.DMS.WebApps.WebApp)
{
	$nodeExists = $app.Nugetpush 
    if (($nodeExists -eq $null) -or ($nodeExists.tolower() -eq 'true')) 
	{
		"Nuget Pack and PUSH {0} is Started.." -f $app.name | Write-Host
		Write-Host "--------------------------------------------------------"
		$allArgs  = (Get-MSBuildCommandArgs -ProjectRootPath $app.RootPath -Project $app.name) 
		$allArgs +=('/p:OutputDirectory={0}\DMS\' -f $scriptDir) 
			
		"Calling msbuild.exe with the following parameters. [$msbuildCmd] [$allArgs] " | Write-Host
		& $msbuildCmd $allArgs
		if ($LastExitCode -ne 0) {
		 exit $LastExitCode
		}
		"Nuget Pack and PUSH {0} is completed.." -f $app.name | Write-Host 
		Write-Host "--------------------------------------------------------"
    }
    else {
		Write-Host "--------------------------------------------------------"
		"The package {0} is not pushed to nuget.." -f $proj.name | Write-Host 
		Write-Host "--------------------------------------------------------"
    }
}
#get release note
$ReleaseNote =("{0}\DMS\ReleaseNote.out" -f $scriptDir)
if ((Test-Path $ReleaseNote) -eq 'True')
{
	Write-Host "Release note"
	Write-Host "......................................................................................."
	Get-Content $ReleaseNote
	Write-Host "......................................................................................."
	Remove-Item $ReleaseNote
}
#==================================================================================


