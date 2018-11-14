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

function Get-MSBuildPath(){
    return "C:\Program Files (x86)\MSBuild\14.0\Bin\amd64\MSBuild.exe"
}
#==================================================================================

#Declaration Section
#==================================================================================
$scriptDir = (Get-ScriptDirectory)

$msbuildCmd = (Get-MSBuildPath)
$DMSConfigFile= ("{0}DMS.Config" -f $scriptDir)
[xml]$DMSConfig = Get-Content $DMSConfigFile
$workspace= '..\..\' 

$Datasource=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DataSource' }).Value
$DBPrifix = ($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DBPrifix' }).Value
$UserId=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBUserName' }).Value
$Password=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBPassword' }).Value
$RestoreDB = ($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'RestoreDB' }).Value
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
foreach($proj in $DMSConfig.DMS.MonitorProjects.Project)
{
	Write-Host "--------------------------------------------------------"
	Write-Host "Building Monitors"
	Write-Host "--------------------------------------------------------"

	Write-Host "--------------------------------------------------------"
	"Started Building {0} " -f $proj.name | Write-Host
	Write-Host "--------------------------------------------------------"
	
    #====================================================================
    # Modified code for building monitors, source was DBDeploy.ps1
    #====================================================================

	$OutputDirectory =('{0}DMS\Monitors\' -f $scriptDir)
	$allArgs  = (Get-MSBuildCommandArgs -ProjectRootPath $proj.RootPath -Project $proj.name  -OutputDirectory $OutputDirectory )
	"Calling msbuild.exe with the following parameters. [$msbuildCmd] [$allArgs] " | Write-Host
	& $msbuildCmd $allArgs
	if ($LastExitCode -ne 0) {
	exit $LastExitCode
	}
	"Build {0} is is completed.." -f $proj.name | Write-Host
	
	Write-Host "--------------------------------------------------------"
	Write-Host "XML Config transformation  Started"
	Write-Host "--------------------------------------------------------"
	
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
	$configFile =('{0}DMS\Monitors\{1}\{2}.exe.config' -f $scriptDir ,$proj.name,$proj.name)
	Write-Host "config file to be transformed : $configFile"
	Write-Host $configFile
	$xmlConfig = [xml](Get-Content $configFile)
	# if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXNDBConnection']")){
		# $xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXNDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_CXN;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	# }
	# if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXEDBConnection']")){
		# $xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='CXEDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_CXE;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	# }
	if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PTNRDBConnection']")){
		$xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PTNRDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_PTNR;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	}
	# if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PCATDBConnection']")){
		# $xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='PCATDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_PCAT;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	# }
	# if($xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='ComplianceDBConnection']")){
		# $xmlConfig.SelectSingleNode("configuration/connectionStrings/add[@name='ComplianceDBConnection']").connectionString = ('Data Source={0};Initial Catalog={1}_COMPLIANCE;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )
	# }
	
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
	"Deploying {0}" -f $proj.name | Write-Host 
	Write-Host "--------------------------------------------------------"
	
    $HostDirectory=('{0}DMS\Monitors\{1}' -f $scriptDir,$proj.name)	

    Write-Host "HostDirectory $HostDirectory"

    $exeFile =('{0}DMS\Monitors\{1}\{2}.exe' -f $scriptDir ,$proj.name,$proj.name)
    $nodeExists = $proj.CommandLineArguments 
    
    Write-Host "IsIntegerationTest $IsIntegerationTest"

	Write-Host "Exe File Path is  " $exeFile


    if($IsIntegerationTest -eq "Y")
    {
        Write-Host "This is integeration test deployment"
#        $FolderName = ('D:\Deployments\DEV-CI\{0}' -f $app.name)
        $copycmd = "Copy-Item ""Microsoft.PowerShell.Core\FileSystem::$HostDirectory"" ""Microsoft.PowerShell.Core\FileSystem::\\OrdDevWeb1\D$\Deployments\DEV-CI\"" -Recurse -Force"
        Write-Host $copycmd
        Invoke-Expression $copycmd

		$exeFile =('\\OrdDevWeb1\D$\Deployments\DEV-CI\{0}\{1}.exe' -f $proj.name,$proj.name)

        $ScriptBlockContent = { 
            param ($MessageToWrite)
            Invoke-Expression $MessageToWrite }

        Invoke-Command -ComputerName OrdDevWeb1 -ScriptBlock $ScriptBlockContent -ArgumentList $exeFile
		if ($LastExitCode -ne 0) {
		    exit $LastExitCode
		}
    }
    else
    {
	    if (($nodeExists -eq $null) -or ($nodeExists.tolower() -eq '')) 
	    {
		    "Deploying {0} to server with connectionString {1} " -f $exeFile, $ConnectionString | Write-Host 
		    & "$exeFile" | Write-Host
		    if ($LastExitCode -ne 0) {
		    exit $LastExitCode
		    }
	    }
	    else
	    {
		    "Deploying {0} to server with connectionString {1} , CommandLineArguments : {2} " -f $exeFile, $ConnectionString,$proj.CommandLineArguments | Write-Host 		
		    & "$exeFile" $proj.CommandLineArguments  | Write-Host
		    if ($LastExitCode -ne 0) {
		    exit $LastExitCode
		    }
	    }
    }
}
#==================================================================================