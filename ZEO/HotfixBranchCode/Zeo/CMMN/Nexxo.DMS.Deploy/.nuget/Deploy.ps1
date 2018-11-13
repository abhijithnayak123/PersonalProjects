Write-Host "Preparing the web site name and web application name..."

Write-Host "OctopusPackageDirectoryPath is " + $OctopusPackageDirectoryPath
Write-Host "OctopusActionPackageCustomInstallationDirectory is " + $OctopusActionPackageCustomInstallationDirectory
Write-Host "Octopus.Action.Package.CustomInstallationDirectory is " + $Octopus.Action.Package.CustomInstallationDirectory
Write-Host "Octopus Environment Variable Value is " + $OctopusEnvironmentName
Write-Host "Custom Environment Variable(DeploymentEnviornment) Value is " + $DeploymentEnviornment 

$VersionSplit = $OctopusPackageVersion.Split(".")
if(!$HostedWebSite)
{
	$HostedWebSite = "Default Web Site"
	#$OctopusWebSiteName = $HostedWebSite + "/" + $OctopusPackageName + "." + $VersionSplit[0] + "." + $VersionSplit[1] + "." + $VersionSplit[2]
	$OctopusWebSiteName = $HostedWebSite + "/" + $OctopusPackageName 
	Write-Host "OctopusWebSiteName is :" + $OctopusWebSiteName
}
if(!$WebAppName)
{
	#$WebAppName = ($OctopusPackageName + "." + $VersionSplit[0] + "." + $VersionSplit[1] + "." + $VersionSplit[2] + "-" + $DeploymentEnviornment).trimend("-")
	$WebAppName = ($OctopusPackageName + "-" + $DeploymentEnviornment).trimend("-")
	Write-Host "WebAppName is :" + $WebAppName
}

Write-Host "Parameter Values are : OctopusWebSiteName = " + $OctopusWebSiteName + "; WebAppName = " + $WebAppName

Write-Host "If above parameters does not match the values expected, then please check your variables in Octopus Portal or you package name being installed"

Write-Host "Initializing required parameters..."

$appPoolName = ($OctopusProjectName)
$siteName = ($OctopusWebSiteName) 
$appPoolFrameworkVersion = "v4.0"
$webRoot = ($OctopusActionPackageCustomInstallationDirectory)
$siteWebSite = $HostedWebSite

Import-Module WebAdministration

cd IIS:\

if($webRoot -eq "")
{
	$webRoot = ($OctopusActionPackageCustomInstallationDirectory)	
}
if($webRoot -eq "")
{
	$webRoot = ($Octopus.Action.Package.CustomInstallationDirectory)	
}

Write-Host "Web Root is  " + $webRoot

$appPoolPath = ("IIS:\AppPools\" + $appPoolName)
$pool = Get-Item $appPoolPath -ErrorAction SilentlyContinue

if (!$pool) { 
    Write-Host "App pool does not exist, creating..." 
    new-item $appPoolPath
    $pool = Get-Item $appPoolPath
} else {
    Write-Host "App pool exists." 
}

Write-Host "Set .NET framework version:" $appPoolFrameworkVersion
Set-ItemProperty $appPoolPath managedRuntimeVersion $appPoolFrameworkVersion

Write-Host "Set identity..."
Set-ItemProperty $appPoolPath -name processModel -value @{identitytype="NetworkService"}

Write-Host "Checking web application..."
$site = Get-WebApplication $siteName -ErrorAction SilentlyContinue
if (!$site) { 
    Write-Host "Site does not exist, creating..." 
	New-WebApplication -Name $WebAppName -Site $siteWebSite -PhysicalPath $webRoot -ApplicationPool $appPoolName -Force
} else {
    Write-Host "Web application already exists. Complete"
}

Write-Host "IIS configuration complete!"