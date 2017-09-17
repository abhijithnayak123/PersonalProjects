param(
        [Parameter(Mandatory=$true)]
        $DMSHostDirectory,
		
		[Parameter(Mandatory=$true)]
        $PackageName
    )

# Since every one is on 64 bit machine this may not be required anymore.	
#function Get-ScriptDirectory
#{
#    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
#    $path = Split-Path $Invocation.MyCommand.Path
#    return ("{0}\" -f $path)
#}
#$scriptDir = (Get-ScriptDirectory)
#Write-Host "Current System Architecture : $env:Processor_Architecture" 
# am I running in 32 bit shell?
#if ($pshome -like "*syswow64*") {
#	
#	write-warning "Restarting script under 64 bit powershell"
#	# relaunch this script under 64 bit shell
#	# if you want powershell 2.0, add -version 2 *before* -file parameter
#	& (join-path ($pshome -replace "syswow64", "sysnative") powershell.exe) -file `
#		(join-path $scriptDir $myinvocation.mycommand) -DMSHostDirectory $DMSHostDirectory -PackageName $PackageName
#	# exit 32 bit script
#	exit
#}
# start of script for 64 bit powershell
#write-warning "current pshome : $pshome"
Write-Host "Preparing the web site name and web application name..."
Write-Host "DMS Hosting Directory is "  $DMSHostDirectory
if(!$HostedWebSite)
{
	$HostedWebSite = "Default Web Site"
	$WebSiteName = $HostedWebSite + "/" + $PackageName 
	Write-Host "WebSiteName is :" $WebSiteName 
}
if(!$WebAppName)
{
	$WebAppName = $PackageName
	Write-Host "WebAppName is :" $WebAppName 
}

Write-Host "Parameter Values are : WebSiteName =  $WebSiteName ; WebAppName = $WebAppName"

Write-Host "Initializing required parameters..."
$appPoolName = 'DMS-Web-CI'
$siteName = ($WebSiteName) 
$appPoolFrameworkVersion = "v4.0"
$webRoot = ($DMSHostDirectory)
$siteWebSite = $HostedWebSite

Import-Module WebAdministration

cd IIS:\

if($webRoot -eq "")
{
	$webRoot = ($DMSHostDirectory)	
}

Write-Host "Web Root is  " + $webRoot

$appPoolPath = ("IIS:\AppPools\" + $appPoolName)

try
{
    $pool = Get-Item $appPoolPath #-ErrorAction SilentlyContinue
}
catch [Exception]
{
    echo $_.Exception.GetType().FullName, $_.Exception.Message
}   


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
    Write-Host "Creating Site..." 
	New-WebApplication -Name $WebAppName -Site $siteWebSite -PhysicalPath $webRoot -ApplicationPool $appPoolName -Force
} 

Write-Host "IIS configuration complete!"