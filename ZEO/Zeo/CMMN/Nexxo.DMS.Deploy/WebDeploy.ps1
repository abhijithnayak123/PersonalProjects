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

# we need to build the msbuild command
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
	$cmdArgs +='/p:Platform=x64' 
    return $cmdArgs
}

function Get-MSBuildPath(){
    return "C:\Program Files (x86)\MSBuild\14.0\Bin\amd64\MSBuild.exe"
}
#==================================================================================

#Declaration Section
#==================================================================================
#$scriptDir = (Get-ScriptDirectory)
$scriptDir = "$PSScriptRoot\"
Write-Host "Script Dir is " $scriptDir
$DMSConfigFile= ("{0}DMS.Config" -f $scriptDir)
Write-Host "DMS Config is " $DMSConfigFile
[xml]$DMSConfig = Get-Content $DMSConfigFile
$msbuildCmd = (Get-MSBuildPath)
$workspace='..\..\'

$Datasource=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DataSource' }).Value
$DBPrifix = ($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DBPrifix' }).Value
$UserId=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBUserName' }).Value
$Password=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBPassword' }).Value
$ServiceBaseUrl=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'ServiceBaseUrl' }).Value
#==================================================================================

#begin script
#==================================================================================
if(!([environment]::GetEnvironmentVariable("EnableNuGetPackageRestore","Machine")))
{
	Write-Host "Creating Environment variable EnableNuGetPackageRestore"
	[Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Machine")
}

#Build and Deploy web applications
foreach($app in $DMSConfig.DMS.WebApps.WebApp)
{
	Write-Host "--------------------------------------------------------"
	"Started Building {0} " -f $app.name | Write-Host
	Write-Host "--------------------------------------------------------"
	$OutputDirectory =('{0}DMS\' -f $scriptDir)
	$allArgs  = (Get-MSBuildCommandArgs  -ProjectRootPath  $app.RootPath  -Project $app.name -OutputDirectory $OutputDirectory) 
	"Calling msbuild.exe with the following parameters. [$msbuildCmd] [$allArgs] " | Write-Host
	& $msbuildCmd $allArgs 
	if ($LastExitCode -ne 0) {
	 exit $LastExitCode
	}
	"Build {0} Completed" | Write-Host

	Write-Host "--------------------------------------------------------"
	Write-Host "WEB Config transformation Started"
	Write-Host "--------------------------------------------------------"
	
	$allArgs  = (Get-MSBuildCommandArgs -ProjectRootPath  $app.RootPath  -Project $app.name -OutputDirectory $OutputDirectory) 
	#$allArgs +='/t:TransformWebConfig'
	#$allArgs +='/p:Env=Dev'
	"Calling msbuild.exe with the following parameters. [$msbuildCmd] [$allArgs] " | Write-Host
	& $msbuildCmd $allArgs 
	if ($LastExitCode -ne 0) {
	 exit $LastExitCode
	}
	Write-Host "XML Config transformation Completed"
	Write-Host "--------------------------------------------------------"
	Write-Host "Parameterized Config transformation Started"
	Write-Host "--------------------------------------------------------"
	
	$configFile =('{0}DMS\{1}\Web.config' -f $scriptDir ,$app.name)
	Write-Host $configFile
	$xmlConfig = [xml](Get-Content $configFile)
	# transfer service database connections
	if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXNDBConnection']")){
		$xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXNDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_CXN;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	}
	if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXEDBConnection']")){
		$xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXEDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_CXE;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	}
	if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PTNRDBConnection']")){
		$xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PTNRDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_PTNR;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	}
	if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PCATDBConnection']")){
		$xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PCATDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_PCAT;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	}
	if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='ComplianceDBConnection']")){
		$xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='ComplianceDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_COMPLIANCE;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	}
	# transfer service database connections
	# transfer service endpoint
	if($xmlConfig.SelectSingleNode("configuration/system.serviceModel/client/endpoint[@contract='DMSService.IDesktopService']")){
		$xmlConfig.SelectSingleNode("configuration/system.serviceModel/client/endpoint[@contract='DMSService.IDesktopService']").address = ('{0}/DesktopWSImpl.svc' -f $ServiceBaseUrl ) 
	}
	# transfer service endpoint

    if($IsIntegerationTest -eq "Y")
    {	
		#update log file path
		$obj = $xmlConfig.configuration.appSettings.add | where {$_.Key -eq 'LogPath'}
		$obj.value = 'C:\Logs\DEV-CI\'
		# end update log file path
	}
	$xmlConfig.Save($configFile)
	
	Write-Host "Parameterized config transformation for $configFile is Completed.."	
	
	
	Write-Host "--------------------------------------------------------"
	"Hosting {0} with Application Name : {1} " -f $app.name , $app.ApplicationName | Write-Host 
	Write-Host "--------------------------------------------------------"
	$HostDirectory=('{0}DMS\{1}\' -f $scriptDir,$app.name)
	$PackageName= $app.ApplicationName
 
    Write-Host "PackageName $PackageName"
    Write-Host "FolderName $FolderName"

    Write-Host "IsIntegerationTest $IsIntegerationTest"
	$result = "Nothing"
    if($IsIntegerationTest -eq "Y")
    {
        Write-Host "This is integeration test deployment"
        $FolderName = ('D:\Deployments\DEV-CI\{0}' -f $app.name)
        $copycmd = "Copy-Item ""Microsoft.PowerShell.Core\FileSystem::$HostDirectory"" ""Microsoft.PowerShell.Core\FileSystem::\\OrdDevWeb1\D$\Deployments\DEV-CI"" -Recurse -Force"
        Write-Host $copycmd
        Invoke-Expression $copycmd
        
        $result = Invoke-Command -ComputerName OrdDevWeb1  -File $scriptDir\Deploy.ps1 -ArgumentList $FolderName, $PackageName #-ErrorAction Stop, for some reason this step through error even if the above line was sucessful. Need time to analyse and fix.
		Write-Host "OrdDevWeb1 Response was " $result

		$copycmd2 = "Copy-Item ""Microsoft.PowerShell.Core\FileSystem::$HostDirectory"" ""Microsoft.PowerShell.Core\FileSystem::\\OrdDevWeb2\D$\Deployments\DEV-CI"" -Recurse -Force"
        Write-Host $copycmd2
        Invoke-Expression $copycmd2
				
		$result = Invoke-Command -ComputerName OrdDevWeb2  -File $scriptDir\Deploy.ps1 -ArgumentList $FolderName, $PackageName #-ErrorAction Stop, for some reason this step through error even if the above line was sucessful. Need time to analyse and fix.
		Write-Host "OrdDevWeb2 Response was " $result
	}
    else
    {
        Write-Host "This is not integeration test deployment"
	    $command="$scriptDir\Deploy.ps1 -DMSHostDirectory $HostDirectory -PackageName $PackageName -ErrorAction Stop"
	    Write-Host "Start Deploy with following parameter : $command"
	    Invoke-Expression $command | Write-Host
    }

	if ($LastExitCode -ne 0) {
	 exit $LastExitCode
	}

	Write-Host "--------------------------------------------------------"
	"Hosting {0} Completed" -f $app.name | Write-Host 
	Write-Host "--------------------------------------------------------"
}
cd $scriptDir
#==================================================================================