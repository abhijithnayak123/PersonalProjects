# Web application deployment script
# Octopus 2.0 compatible

# General settings
$siteName = "Default Web Site"
$appPoolFrameworkVersion = "v4.0"

# Octopus settings
$environment = $OctopusParameters["Octopus.Environment.Name"]
$packageName = $OctopusParameters["Octopus.Action.Package.NuGetPackageId"]
$packageVersion = $OctopusParameters["Octopus.Action.Package.NuGetPackageVersion"]

Write-Host "==> Preparing to deploy web application ..."

$versionTokens = $packageVersion.Split(".")
If (!$WebAppName) {
	$WebAppName = ($packageName + "." + $versionTokens[0] + "." + $versionTokens[1] + "." + $versionTokens[2] + "-" + $environment).trimend("-")
}

Write-Host "==> Initializing required parameters ..."
$webRoot = ($OctopusParameters["Octopus.Action.Package.CustomInstallationDirectory"])
$appPoolName = ($OctopusParameters["Octopus.Project.Name"])
$appPoolPath = ("IIS:\AppPools\" + $appPoolName)

Write-Host "Environment : $environment"
Write-Host "Web root : $webRoot"
Write-Host "App pool : $appPoolName"
Write-Host "Site name : $siteName"
Write-Host "Webapp name : $WebAppName"

Write-Host "==> If the above parameters do not match the values expected, please check your variables in Octopus Deploy or the package name being installed."

Import-Module WebAdministration

cd IIS:\

$appPool = Get-Item $appPoolPath -ErrorAction SilentlyContinue

If (!$appPool) { 
    Write-Host "==> App pool does not exist. Creating ..." 
    New-Item $appPoolPath
} Else {
    Write-Host "==> App pool already exists. Nothing to do." 
}

Write-Host "==> Setting .NET framework version to $appPoolFrameworkVersion ..."
Set-ItemProperty $appPoolPath managedRuntimeVersion $appPoolFrameworkVersion

Write-Host "==> Setting identity ..."
Set-ItemProperty $appPoolPath -name processModel -value @{identitytype="NetworkService"}

Write-Host "==> Checking web application..."
$webApp = Get-WebApplication -Site $siteName -Name $WebAppName -ErrorAction SilentlyContinue
If ($webApp) {
    Write-Host "==> Web application already exists. Removing ..."
    Remove-WebApplication -Site $siteName -Name $WebAppName 
}

Write-Host "==> Creating web application $webAppName ..." 
New-WebApplication -Site $siteName -Name $WebAppName -PhysicalPath $webRoot -ApplicationPool $appPoolName

Write-Host "==> IIS configuration complete!"