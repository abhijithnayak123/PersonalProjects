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
# prepare the msbuild command
function Get-MSBuildCommandArgs(){
    param(
		
		[Parameter(Mandatory=$true)]
        [string]
        $ProjectRootPath,
		
		[Parameter(Mandatory=$true)]
        [string]
        $Project,
		
		[Parameter(Mandatory=$true)]
        [string]
        $OutputDirectory
    )

    $cmdArgs = @()
    $cmdArgs += ("{0}BuildAndPublish.proj" -f $scriptDir)
	$cmdArgs +=('/p:WORKSPACE={0}' -f $workspace)
	$cmdArgs +=('/p:OutputDirectory={0}' -f $OutputDirectory)
	$cmdArgs +=('/p:ProjectRootPath={0}' -f $ProjectRootPath)
	$cmdArgs +=('/p:Project={0}' -f $Project)
	$cmdArgs +='/p:ReleaseMode=Release' 
    return $cmdArgs
}

function Copy-AppConfig-Source(){
	param(
	
		[Parameter(Mandatory=$true)]
        [string]
        $ProjectBasePath,
		
		[Parameter(Mandatory=$true)]
        [string]
        $ProjectRootPath,
		
		[Parameter(Mandatory=$true)]
        [string]
        $Project,
		
		[Parameter(Mandatory=$true)]
        [string]
        $OutputDirectory
	)
	
	$cmdArgs = @("{0}{1}app.config" -f $ProjectBasePath, $ProjectRootPath)
	return $cmdArgs
}
function Copy-AppConfig-Destination(){
	param(
	
		[Parameter(Mandatory=$true)]
        [string]
        $ProjectBasePath,
		
		[Parameter(Mandatory=$true)]
        [string]
        $ProjectRootPath,
		
		[Parameter(Mandatory=$true)]
        [string]
        $Project,
		
		[Parameter(Mandatory=$true)]
        [string]
        $OutputDirectory
	)
	
	$cmdArgs = @("{0}{1}\bin\{2}.dll.config" -f $OutputDirectory, $Project, $Project)
	return $cmdArgs
}


function Get-MSBuildPath(){
    return "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
}
#==================================================================================

#Declaration Section
#==================================================================================
$scriptDir = (Get-ScriptDirectory)
$baseDir = (Get-BaseDirectory)
$msbuildCmd = (Get-MSBuildPath)
$DMSConfigFile= ("{0}DMS.Config" -f $scriptDir)
[xml]$DMSConfig = Get-Content $DMSConfigFile
$workspace= '..\..\' 

#==================================================================================

#begin script
#==================================================================================
#add EnableNuGetPackageRestore env variable if not exists
if(!([environment]::GetEnvironmentVariable("EnableNuGetPackageRestore","Machine")))
{
	Write-Host "Creating Environment variable EnableNuGetPackageRestore"
	[Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Machine")
}
#build DB projects

$DeploymentServer=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DeploymentServer' }).Value

foreach($proj in $DMSConfig.DMS.TestProjects.TestProject)
{
	Write-Host "--------------------------------------------------------"
	"Started Building {0} " -f $proj.name | Write-Host
	Write-Host "--------------------------------------------------------"

	$OutputDirectory =('{0}DMS\' -f $scriptDir)
	$allArgs  = (Get-MSBuildCommandArgs -ProjectRootPath $proj.RootPath -Project $proj.name  -OutputDirectory $OutputDirectory )
	"Calling msbuild.exe with the following parameters. [$msbuildCmd] [$allArgs] " | Write-Host
	Write-Host $allArgs
	& $msbuildCmd $allArgs
	$copyConfigSource = (Copy-AppConfig-Source -ProjectBasePath $baseDir -ProjectRootPath $proj.RootPath -Project $proj.name -OutputDirectory $OutputDirectory );
	$copyConfigDestination = (Copy-AppConfig-Destination -ProjectBasePath $baseDir -ProjectRootPath $proj.RootPath -Project $proj.name -OutputDirectory $OutputDirectory );
	Write-Host $copyConfigSource;
	Write-Host $copyConfigDestination;
	copy $copyConfigSource $copyConfigDestination
	if ($LastExitCode -ne 0) {
	exit $LastExitCode
	}
	"Build {0} is is completed.." -f $proj.name | Write-Host
	
	$configFile =('{0}DMS\{1}\bin\{2}.dll.config' -f $scriptDir ,$proj.name,$proj.name)
	# transfer service endpoint
	
	Write-Host $configFile
	$xmlConfig = [xml](Get-Content $configFile)
	if($xmlConfig.SelectSingleNode("configuration/system.serviceModel/client/endpoint[@contract='DMSService.IDesktopService']")){
		$xmlConfig.SelectSingleNode("configuration/system.serviceModel/client/endpoint[@contract='DMSService.IDesktopService']").address = ('http://{0}/{1}/DesktopWSImpl.svc' -f $DeploymentServer,$proj.ApplicationName)
	}
	$xmlConfig.Save($configFile)
	# transfer service endpoint
}
#==================================================================================


