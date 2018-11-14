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
$DMSConfigFile= ("{0}DMS.Config" -f $scriptDir)
[xml]$DMSConfig = Get-Content $DMSConfigFile
$SancusRepoPath =($DMSConfig.DMS.SystemTest.add | Where-Object { $_.Key -eq 'SancusRepoPath' }).Value
$StartHubCmd = ("START /MIN  CMD /k java -Dwebdriver.chrome.driver=`"{0}Drivers\chromedriver.exe`" -jar {0}Drivers\selenium-server-standalone-2.42.2.jar -role hub" -f $SancusRepoPath)
$StartNodeCmd =("START /MIN  CMD /k java -Dwebdriver.chrome.driver=`"{0}Drivers\chromedriver.exe`" -jar {0}Drivers\selenium-server-standalone-2.42.2.jar -role node -hub http://localhost:4444/grid/register" -f $SancusRepoPath)
$gradlewDir = ("{0}slingshot" -f $SancusRepoPath)  
#==================================================================================

#begin script
#==================================================================================
Write-Host "--------------------------------------------------------" 
Write-Host "Start Hub : $StartHubCmd"

& CMD /C $StartHubCmd

Write-Host "--------------------------------------------------------"
Write-Host "Start Node : $StartNodeCmd"
& CMD /C  $StartNodeCmd

Write-Host "--------------------------------------------------------"
Write-Host "gradlew Run : $gradlewDir"
Set-Location $gradlewDir
& CMD /C "gradlew run"

if ($LastExitCode -ne 0) {
	Set-Location $scriptDir
     exit $LastExitCode
}

Set-Location $scriptDir
Write-Host "--------------------------------------------------------"
#==================================================================================