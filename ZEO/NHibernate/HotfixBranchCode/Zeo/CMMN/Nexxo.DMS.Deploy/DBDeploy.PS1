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

$Datasource=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DataSource' }).Value
$DBPrifix = ($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DBPrifix' }).Value
$RestoreDB = ($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'RestoreDB' }).Value
$DBBackupPath = ($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DBBackupPath' }).Value
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
foreach($proj in $DMSConfig.DMS.DBProjects.Project)
{
	Write-Host "--------------------------------------------------------"
	"Started Building {0} " -f $proj.name | Write-Host
	Write-Host "--------------------------------------------------------"
	
	#During restore DB 	Seed Data is not required
	if ( !( ($RestoreDB -eq 'true') -and ($proj.name -eq 'Nexxo.CI.Seed.Database' ) ) )
	{
	
		$OutputDirectory =('{0}DMS\DataBase\' -f $scriptDir)
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
	
#		$allArgs +='/t:TransformWebConfig'
#		$allArgs +='/p:Env=Dev'
		"Calling msbuild.exe with the following parameters. [$msbuildCmd] [$allArgs] " | Write-Host
		& $msbuildCmd $allArgs 
		if ($LastExitCode -ne 0) {
		  exit $LastExitCode
		}
		Write-Host "XML Config transformation Completed"

		Write-Host "--------------------------------------------------------"
		Write-Host "Parameterized Config transformation Started"
		Write-Host "--------------------------------------------------------"
		$configFile =('{0}DMS\DataBase\{1}\{2}.exe.config' -f $scriptDir ,$proj.name,$proj.name)
		Write-Host "config file to be transformed : $configFile"
		
		if( $proj.InitialCatalog  -eq 'master')
		{
			$InitialCatalog ='master'
			$UserId=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DBOwnerUserName' }).Value
			$Password=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DBOwnerPassword' }).Value
		}
		else
		{
			$InitialCatalog =('{0}_{1}' -f $DBPrifix,$proj.InitialCatalog )
			$UserId=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBUserName' }).Value
			$Password=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBPassword' }).Value
		}
		$ConnectionString= ('Data Source={0};Initial Catalog={1};User ID={2};pwd={3}' -f $Datasource,$InitialCatalog,$UserId,$Password )

		
		(Get-Content $configFile)`
			| Foreach-Object {$_ `
			-replace "{OutputDirectory}", ('{0}{1}\' -f $OutputDirectory,$proj.name)`
			-replace "{CXNConnectionString}",  ('Data Source={0};Initial Catalog={1}_CXN;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )`
			-replace "{CXEConnectionString}", ('Data Source={0};Initial Catalog={1}_CXE;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )`
			-replace "{PTNRConnectionString}", ('Data Source={0};Initial Catalog={1}_PTNR;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )`
			-replace "{PCATConnectionString}", ('Data Source={0};Initial Catalog={1}_PCAT;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )`
			-replace "{COMPLIANCEConnectionString}", ('Data Source={0};Initial Catalog={1}_COMPLIANCE;User ID={2};pwd={3}' -f $Datasource,$DBPrifix,$UserId,$Password )`
			-replace "{CXNDatabaseName}", ('{0}_CXN' -f $DBPrifix )`
			-replace "{CXEDatabaseName}", ('{0}_CXE' -f $DBPrifix )`
			-replace "{PTNRDatabaseName}", ('{0}_PTNR' -f $DBPrifix )`
			-replace "{PCATDatabaseName}", ('{0}_PCAT' -f $DBPrifix )`
			-replace "{COMPLIANCEDatabaseName}", ('{0}_COMPLIANCE' -f $DBPrifix )`
			-replace "{Datasource}", ('{0}' -f $Datasource )`
			-replace "{DatabaseUserName}", ('{0}' -f $UserId )`
			-replace "{DataBasePassword}", ('{0}' -f $Password )}`
			| Set-Content $configFile
		
		[xml]$DBConfig = Get-Content $configFile  
		

		if ( ! (($proj.InitialCatalog -eq $null) -or ($proj.InitialCatalog -eq '')) )
		{
			$DBConfig.configuration.connectionStrings.Add.connectionString=$ConnectionString
		}
		Try
		{
			#CXNDATABASE Synonym
			$CXNDATABASE = ('{0}_CXN' -f $DBPrifix)
			($DBConfig.configuration.appSettings.add | Where-Object { $_.Key -eq 'CXNDATABASE' }).Value= $CXNDATABASE 
	
			#CXEDATABASE Synonym
			$CXEDATABASE =('{0}_CXE' -f $DBPrifix)
			($DBConfig.configuration.appSettings.add | Where-Object { $_.Key -eq 'CXEDATABASE' }).Value= $CXEDATABASE
			Write-Host "Synonym Transformation Completed" 
		}
		Catch
		{
			Write-Host "Synonym does not exist in $configFile" 
		}
			
		Try
		{ 
			#CI Data base package config
			($DBConfig.configuration.appSettings.add | Where-Object { $_.Key -eq 'DBUserName' }).Value=($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBUserName' }).Value

			($DBConfig.configuration.appSettings.add | Where-Object { $_.Key -eq 'DBUserPassword' }).Value= ($DMSConfig.DMS.DatabaseConfig.add | Where-Object { $_.Key -eq 'DMSDBPassword' }).Value
			
			($DBConfig.configuration.appSettings.add | Where-Object { $_.Key -eq 'DBPrifix' }).Value= $DBPrifix
			
			($DBConfig.configuration.appSettings.add | Where-Object { $_.Key -eq 'RestoreDB' }).Value= $RestoreDB
			
			($DBConfig.configuration.appSettings.add | Where-Object { $_.Key -eq 'DBBackupPath' }).Value= $DBBackupPath			
			
		}
		Catch
		{
		}
		$DBConfig.Save($configFile)
	
		Write-Host "Parameterized config transformation for $configFile is Completed.."
		
	
		Write-Host "--------------------------------------------------------"
		"Deploying {0}" -f $proj.name | Write-Host 
		Write-Host "--------------------------------------------------------"
	
		$exeFile =('{0}DMS\DataBase\{1}\{2}.exe' -f $scriptDir ,$proj.name,$proj.name)
	
		$nodeExists = $proj.CommandLineArguments 
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


