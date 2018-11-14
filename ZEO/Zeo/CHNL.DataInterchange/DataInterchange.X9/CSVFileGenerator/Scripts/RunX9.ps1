##########################################################
# Powershell to process X-9 and send to sFTP server
# 2016-01-22 Shawn added parameters
# 
# NOTES:
#
# parameter is required: RunX9.ps1 -run 1 or RunX9.ps1 -run test2
# supported parameters are 1, 2, 3, test1, test2, test3
#
# to specify a date for CSVGenerator:
# &$exedir\CSVFileGenerator.exe --mode 1 --rundate 20150915 --ignorepreviousrun true --partner TCF >> $log
# &$exedir\CSVFileGenerator.exe --mode 1 --ignorepreviousrun true  >> $log
#
# to specify a date:
# PowerShell -file $workdir\GenerateAndMergeX9Files.ps1 -rundate 09/15/2015 >> $log
# 6/14/2016
# Added to the timeouts for transmissions
# Added sleep 20 on archive step and log a line that says we tried to move it
# working on adding try/catch to the archive step and proper error loggging with a transcript
# 6/16/2016 
# -Added sleep prior to verify step and added count to log
# -Added removal of any files from previous run
# 12/1/2016
# -Installed AMP on webnode2 for TCF Production cotover on 12-11-16
# 12/8/2016 Shawn Westerhoff
# -Changed to xfer transfer tool from putty psftp, simplify script
# 09/19/2017
# Cleaned the script to accomodate TCF datacenter move. Since the jobs will run via MoveIT File watcher
# Removed unwanted code based on new MoveIt approach, added parameters to support rerun the X9 generation.
########################################################## 

# Default execution is RunX9 -run <value>
# For rerun of the script it should be RunX9 -run <value> -destinationfolder<value> -ignorepreviousrun <$true,$false,1,0> -$rundate <date in MM/dd/yyyy format>
 
param(
    [Parameter(Mandatory=$true,Position=0,HelpMessage="Indicates the current run, valid values are 1, 2, 3")]
    [ValidateRange(1,3)]
    [string]$run,
    [Parameter(Mandatory=$true)]
    [string] $destinationfolder,
    [bool] $ignorepreviousrun = $false,
    [datetime] $rundate = (Get-Date) 
    )

$ErrorActionPreference = "Stop"
    
$job = "tcfX9_$run"

$invocation = (Get-Variable MyInvocation).Value
$workdir = Split-Path $invocation.MyCommand.Path
$exedir = (Get-Item $workdir).Parent.FullName

#Date Settings
#$date = "2016-11-15"
$date = $rundate.ToString('yyyy-MM-dd')
#$date2 = "20161115"
$date2 = $rundate.ToString('yyyyMMdd')
#$date3 = "11/15/2016"
$date3 = $rundate.ToString('MM/dd/yyyy')
$time = (Get-Date -Format "HH:mm:ss")

$log = "c:\X9Reporting\logs\$Date.$run.txt"
$outputdir = "c:\X9Reporting\Output\$date2\TCF"
$archivedir = "$outputdir\$run"
#production specific variables

#test specific variables

# set mode

If ($run -eq "1")
    {
    $mode = "1"
    }
Elseif ($run -eq "2")
    {
    $mode = "2"
    }
Elseif ($run -eq "3")
    {
    $mode = "3"
    }
Else {exit}

Set-Location $workdir
Add-Content $log "Begin X9 run $run on $date mode $mode"
Add-Content $log "Generating CSV file - launch CSVFileGenerator.exe"
#&$exedir\CSVFileGenerator.exe --mode $mode --rundate $date2 --ignorepreviousrun true --partner TCF >> $log
&$exedir\CSVFileGenerator.exe --mode $mode --rundate $date2 --ignorepreviousrun $ignorepreviousrun >> $log
if($LASTEXITCODE -ne 0)
{
	Throw ("CSVFileGenerator exited with error")
}
Write-Host "Sleeping for 60 seconds following CSVFileGenerator" -ForegroundColor Magenta
Start-Sleep -seconds 60

Write-Host "Running merge process - launch GenerateAndMergeX9Files.ps1"
Add-Content $log "Running merge process - launch GenerateAndMergeX9Files.ps1"
#PowerShell -file $workdir\GenerateAndMergeX9Files.ps1 -rundate $date3 >> $log
PowerShell -file $workdir\GenerateAndMergeX9Files.ps1 -rundate $date3 >> $log

Write-Host "Cleaning of null character in log files"
Add-Content $log "Cleaning of null character in log files"
#Get rid of nul character being mysteriously added to our log
    (Get-Content $log) | 
    Foreach-Object {$_ -replace "\x00", ""} | 
    Set-Content $log

#Transfer the files to shared location
Write-Host "Begin file copy to shared folder on $date at $time"
Add-Content  $log "Begin file copy to shared folder on $date at $time"

Get-ChildItem -Path $outputdir\* -Include *.txt, *.x937 | ForEach-Object {
	$filename = $_.FullName
	Copy-Item $filename $destinationfolder
    Write-Host "Sleeping for 10 seconds for saving $filename to $destinationfolder folder" -ForegroundColor Magenta
	Start-Sleep -Seconds 10
}

Write-Host "Cleanup .sem files"
Add-Content $log "Cleanup .sem files"
#Clean up *.sem files created by job
Remove-Item $outputdir\*.sem

#Archive
Write-Host "Archiving files..."
Add-Content $log "Archiving files..."
New-Item -ItemType directory $archivedir
Get-ChildItem -Path $outputdir\* -Include *.txt, *.x937 | ForEach-Object {
	$filename = $_.FullName
    Add-Content $log "moving $filename..."
    Move-Item $filename $archivedir
    Write-Host "Sleeping for 10 seconds for archiving $filename to $archivedir folder" -ForegroundColor Magenta
	Start-Sleep -Seconds 10
    }
Add-Content $log "Job Complete."