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
#
########################################################## 
 
param(
    [string]$run
    )
    
If ("1","2","3", "test1", "test2", "test3" -NotContains $run) 
        { 
        Throw "OOPS! -- $($run) is not a valid run parameter! Please use -run [1, 2, 3, test1, test2, or test3]"
        } 

$job = "tcfX9_$run"

#$exedir = 'D:\Deployments\PI\2.0.17\CSVFileGenerator'
#$workdir = 'D:\Deployments\PI\2.0.17\CSVFileGenerator\Scripts'

$invocation = (Get-Variable MyInvocation).Value
$workdir = Split-Path $invocation.MyCommand.Path
$exedir = (Get-Item $workdir).Parent.FullName


#$date = "2016-01-28"
$date = (Get-Date -format "yyyy-MM-dd")
#$date2 = "20160128"
$date2 = (Get-Date -format "yyyyMMdd")
#$date3 = "01/28/2016"
$date3 = (Get-Date -format MM/dd/yyyy)
$time = (Get-Date -Format "HH:mm:ss")
$log = "c:\X9Reporting\logs\$Date.$run.txt"
$outputdir = "c:\X9Reporting\Output\$date2\TCF"
$archivedir = "$outputdir\$run"
$putty = '"C:\Program Files (x86)\PuTTY\psftp.exe"'
#production specific variables
#$dest = "transferhub.tcfbank.com"
#$destport = "22"
#$logon = "tp00074"
#$pw = 'Hopi389fox^!^!'
#$destdir = "/Test"
#test specific variables
#$destTEST = "12.227.206.77"
#$destportTEST = "1226"
#$logonTEST = 'mgi'
#$pwTEST = 'qa@ynHT4u7'
#$destdirTEST = "transfer"

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
Elseif ($run -eq "test1")
    {
    $mode = "1"
    }
Elseif ($run -eq "test2")
    {
    $mode = "2"
    }
Elseif ($run -eq "test3")
    {
    $mode = "3"
    }
Else {exit}

Set-Location $workdir
Add-Content $log "Begin X9 run $run on $date mode $mode"
Add-Content $log "Generating CSV file - launch CSVFileGenerator.exe"
#&$exedir\CSVFileGenerator.exe --mode $mode --rundate $date2 --ignorepreviousrun true --partner TCF >> $log
&$exedir\CSVFileGenerator.exe --mode $mode >> $log
Write-Host "Sleeping for 30 seconds following CSVFileGenerator" -ForegroundColor Magenta
Start-Sleep -seconds 30

Add-Content $log "Running merge process - launch GenerateAndMergeX9Files.ps1"
#PowerShell -file $workdir\GenerateAndMergeX9Files.ps1 -rundate $date3 >> $log
PowerShell -file $workdir\GenerateAndMergeX9Files.ps1 >> $log

#Get rid of nul character being mysteriously added to our log
    (Get-Content $log) | 
    Foreach-Object {$_ -replace "\x00", ""} | 
    Set-Content $log

#Transfer the files
#Add-Content  $log "Begin $job sFTP on $date at $time"

#Generate the batch file
#Create the psftp job instruction file (what psftp will do once connected)
#Note the back-tick ` to escape the double quotes needed to deal with spaces in the path name
#Spaces in paths cause MANY issues that need to be dealt with, adding to complexity
Remove-Item -force $workdir\$job.bat
Remove-Item -force $workdir\$job.txt
Add-Content $workdir\$job.txt "lcd `"$outputdir`""

#If ("1","2","3" -NotContains $run) 
#     {
#     Add-Content $workdir\$job.txt "cd $destdirTEST"
#     }
#Else {
#     Add-Content $workdir\$job.txt "cd $destdir"
#     }

#Get the file list and add the put command lines
#$items = Get-ChildItem -Path $outputdir\* -include *.txt, *.x937
#foreach ($item in $items)
#    {
#    $fname = $item.PSChildName
#    Add-Content $workdir\$job.txt "put $fname"
#    }

# Create the psftp bat file we will run
#Add-Content $log "-Creating transfer job at $Time"
#Add-Content $log "--Transfer job commands begin--"
#Add-Content $log (Get-Content $workdir\$job.txt)
#Add-Content $log "--Transfer job commands end--"
#Add-Content $log "-Begin sFTP log:"

#If ("1","2","3" -NotContains $run) 
#     {
#     Add-Content $workdir\$job.bat  "$putty $destTEST -P $destportTEST -l $logonTEST -pw $pwTEST -bc -b $workdir\$job.txt >> $log"
#     }
#Else {
#     Add-Content $workdir\$job.bat  "$putty $dest -P $destport -l $logon -pw $pw -bc -b $workdir\$job.txt >> $log"
#     }

#Write-Host "Sleeping for 5 seconds for file save to release file lock" -ForegroundColor Magenta
#Start-Sleep -Seconds 5
#Start-Process $workdir\$job.bat
#Write-Host "Sleeping for 120 seconds for files to transfer" -ForegroundColor Magenta
#Start-Sleep -seconds 120
#Clean up *.sem files created by job
#Remove-Item $outputdir\*.sem

#Verify the files all transferred and email failure if not
#$verify = 1
#foreach ($item in $items)
#    {
#    $fname = $item.PSChildName
#    $text = "local:$fname => remote:$destdir/$fname"
#    If (-Not (Get-Content -raw $log | Select-String -Pattern $text -SimpleMatch))
#        {
#            $verify = $verify++
#            $k = "$text NOT FOUND Verify = $verify"
#            Send-MailMessage -To burlingamedevops@moneygram.com -From Devops@monitor.ic.local -Subject "Job $job $run Fail" -SmtpServer 10.200.52.9 -body "$k"
#            break
#        }
#    }
#If the log shows transfer for each item, send a success email with the log and archive
#If ($verify -le 2)
#        {
#            Add-Content $log "verifying"
#            $k = $log
#            Send-MailMessage -To burlingamedevops@moneygram.com -From Devops@monitor.ic.local -Subject "Job $job $run Success" -SmtpServer 10.200.52.9 -body "X9 Log $run" -Attachments $k
#            New-Item -ItemType directory $archivedir
#            foreach ($item in $items)
#                {
#                Start-Sleep -Seconds 2
#                Move-Item $item $archivedir
#                }
#        }