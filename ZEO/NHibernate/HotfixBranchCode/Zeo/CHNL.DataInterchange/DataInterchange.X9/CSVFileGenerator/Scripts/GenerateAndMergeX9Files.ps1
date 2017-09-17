
Param
(
	[Parameter(Mandatory=$False)]
	[DateTime]
	$runDate,
	[Parameter(Mandatory=$False)]
	[String]
	[ValidateSet("Carver", "Synovus", "TCF")]
	$partnerName,
	[Parameter(Mandatory=$False)]
	[ValidateSet("OnUs", "OffUs", "MoneyOrder")]
	[String]
	$fileType
)

<#
.SYNOPSIS
	X937 File generation and merge
	only once in each topic.
.DESCRIPTION
	This script iterates thru an input directory for a specific run date, and for each partner that has a .CSV
	file, generates a x937 file. If multiple x937 files exist, will merge those files to create a consolidated
	x937 file for transmission to the client
.NOTES
	File Name      : GenerateAndMergeX9Files.ps1
	Author         : Adwait Ullal (aullal@moneygram.com)
	Prerequisite   : PowerShell V2 over Vista and upper.
	Copyright 2015 - Adwait Ullal/Moneygram
.LINK
	Script posted over:
	http://github.com/
.EXAMPLE
	(Without any input parameters) GenerateAndMergeX9Files.ps1
.EXAMPLE
	(With input parameters) GenerateAndMergeX9Files.ps1 -RunDate "1/12/2015 14:45:00" -PartnerNames "TCF, Carver", -FileTypes "OnUs"
#>
Function GetScriptInvocationPath
{
	# Due to a quirk in PS, Split-Path for MyInvocation needs to called within a function :(
	return (Split-Path -parent $global:MyInvocation.MyCommand.Path)
}

Function CheckParams
{
	if (($partnerName -ne $null) -and ($partnerList.Contains($partnerName)))
	{
		$partnerList = @($partnerName)
	}
	if ($runDate -eq $null)
	{
		$runDate = [DateTime]::Now;
	}
	elseif ($runDate.ToString("HHmmss") -eq "000000")
	{
		$time = [DateTime]::Now.TimeOfDay
		$runDate = $runDate.AddTicks($time.Ticks)
	}

	return $partnerList, $runDate
}

Function LoadConfig
{
	$configFile = $ScriptInvocationPath + "\GenerateAndMergeX9Files.config.xml"
	$config = [xml](gc $configFile)

	try
	{
		#$connectionString = $config.configuration.connectionStrings.connectionString.value
		$connectionString = "Not Used"
		$inputDirectory = $config.configuration.appSettings.inputDirectory.value
		$outputDirectory = $config.configuration.appSettings.outputDirectory.value
		$amcLocation = $config.configuration.appSettings.amcLocation.value
		$amcConfigDirectory = $ScriptInvocationPath + $config.configuration.appSettings.amcConfigDirectory.value
		$amcIniTemplateFile = $ScriptInvocationPath + $config.configuration.appSettings.amcIniTemplateFile.value
		$archiveDirectory = $config.configuration.appSettings.archiveDirectory.value
	}
	catch
	{
		Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception) "
	}

	return $connectionString, $inputDirectory, $outputDirectory, $amcLocation, $amcConfigDirectory, $amcIniTemplateFile, $archiveDirectory

}

Function GenerateX9File([string] $partnerName, [string] $fileType, [string] $inputWorkingDirectory)
{

	if ( ($ValidFileTypes -contains $fileType) -ne $true)
	{
		Write-Host("GenerateX9File::InvalidFileType : $fileType for directory: $inputWorkingDirectory for partner: $partnerName")
		return
	}

	if ((Test-Path -PathType Container $inputWorkingDirectory) -ne $true)
	{
		Write-Host("GenerateX9File::InvalidWorkingDirectory : $inputWorkingDirectory for partner: $partnerName")
		return
	}
	
	$files = Get-ChildItem $inputWorkingDirectory -Filter *$fileType*.csv | Select-Object FullName, Name, Length

	foreach($file in $files)
	{
		$name = $file.Name
		$FullName = $file.FullName

		if ($file.Length -lt 1)
		{
			Write-Host("GenerateX9Files::ZeroFileLength : File $file.FullName for partner: $partnerName is null or empty")
			continue
		}

		$configFile = $amcConfigDirectory + "\" + $partnerName + "\" + ($file.Name -replace "_\d{14}.csv", "_Config.xml")
		if ((Test-Path -PathType Leaf $configFile) -ne $true)
		{
			Write-Host("Configuration file: $configFile for $FullName does not exist. Omitted from x937 file processing")
			continue
		}

		$iniFile = $file.FullName.Replace(".csv", ".ini.xml")

		try
		{
			$amcInitXML = [xml](Get-Content $amcIniTemplateFile)
			foreach($var in $amcInitXML.AllMyChecksInitializationFile.var)
			{
				#Write-Host("var: $var")
				if ($var.HasAttribute("configfile") -eq $true)
				{
					$an = $var.GetAttributeNode("configfile")
					$an.Value = $configFile
				}
				if ($var.HasAttribute("ImageDir") -eq $true)
				{
					$an = $var.GetAttributeNode("ImageDir")
					$an.Value = $FullName
				}
				if ($var.HasAttribute("outputDir") -eq $true)
				{
					$an = $var.GetAttributeNode("outputDir")
					$an.Value = $inputWorkingDirectory
				}
			}
		}
		catch
		{
			Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception) "
		}

		try
		{
			$amcInitXML.InnerXml | Out-File -Encoding utf8 $iniFile
		}
		catch
		{
			Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception) "
		}

		Write-Host("Name: $name, Full Name: $FullName, config file: $configFile, ini File: $iniFile")

		try
		{
			$amcArgumentList = $FullName + " " + $configFile + " " + $iniFile
			Write-Host("amcLocation: $amcLocation Argument List: $amcArgumentList") 
			Start-Process -Wait -FilePath $amcLocation -ArgumentList $amcArgumentList
		}
		catch
		{
			Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception) "
		}

	}
}

Function MergeX9Files([string] $partnerName, [string] $fileType, [string] $inputWorkingDirectory, [string] $outputDirectory)
{
	if ( ($ValidFileTypes -contains $fileType) -ne $true)
	{
		Write-Host("MergeX9Files::InvalidFileType : $fileType for directory: $inputWorkingDirectory for partner: $partnerName")
		return
	}

	if ((Test-Path -PathType Container $inputWorkingDirectory) -ne $true)
	{
		Write-Host("MergeX9Files::InvalidWorkingDirectory : $inputWorkingDirectory for partner: $partnerName")
		return
	}

    if ($fileType -ieq "moneyorder")
    {
        $fileTypeInFileName = "mo"
    }
    else
    {
        $fileTypeInFileName = $fileType.ToLower()
    }
	
	$listOfFilesToMerge = (Get-Item $inputWorkingDirectory).Parent.FullName + "\" + [string]::Format("{0}_{1}_{2}_MergeFiles.lst", $partnerName, $fileType, $instanceRunDate.ToString("yyyyMMddHHmmss"))
	$outputFile = $outputDirectory + "\" + [string]::Format("mgi_{0}.{1}.{2}.{3}.x937", $partnerName.Replace(" ", "").ToLower(), $fileTypeInFileName, $instanceRunDate.ToString("yyyyMMdd"), $instanceRunDate.ToString("HHmmss"))

	# Note "encoding" must be ASCII
	Get-ChildItem $inputWorkingDirectory -Filter *.937 | Select-Object -ExpandProperty FullName | Out-File -Encoding ascii $listOfFilesToMerge

	# If the resulting file is zero length, no point in doing a merge
	If (((Test-Path -PathType Leaf $listOfFilesToMerge) -ne $true) -or (((Get-ChildItem $listOfFilesToMerge).Length) -lt 1))
	{
		Write-Host("MergeX9Files::ZeroFileLength : Merge List $listOfFilesToMerge for partner: $partnerName is null or does not exist")
		return
	}

	# If output directory doesn't exist, create it
	if ((Test-Path -PathType Container $outputDirectory) -ne $true)
	{
		New-Item -ItemType Directory -Path $outputDirectory | Out-Null
	}

	# Special case when there is only 1 x9 file, we just copy it, rather than merge, 
	# since AMC can't single file merges :(

	if (((Get-ChildItem $inputWorkingDirectory -Filter *.937 | Select-Object -ExpandProperty FullName | Measure-Object).Count) -lt 2)
	{
		$fileToCopy = (Get-ChildItem $inputWorkingDirectory -Filter *.937 | Select-Object -ExpandProperty FullName)
		Write-Host("MergeX9Files::CopyFile : Only x9 file in directory $inputWorkingDirectory so copying $fileToCopy to destination $outputFile")
		Copy-Item -Path $fileToCopy -Destination $outputFile
		return
	}

	try
	{
		# Invoke AMC to merge the files (note " -M " switch to merge)
		$amcArgumentList = " -M " + $listOfFilesToMerge + " " + $outputFile
		Write-Host("amcLocation: $amcLocation Argument List: $amcArgumentList") 
		Start-Process -Wait -FilePath $amcLocation -ArgumentList $amcArgumentList
	}
	catch
	{
		Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception)"
	}
}


# Main

	try
	{
		Set-StrictMode -Version Latest
	}
	catch
	{
		Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception) "
		return
	}

	#$PartnerList = @("TCF", "Carver", "Synovus")
	$PartnerList = @("TCF")
	$ValidFileTypes = @("OffUs", "OnUs", "MoneyOrder")

	$instanceRunDate = [DateTime]::MinValue
	$RunDateString = [String]::Empty
	$connectionString = [String]::Empty
	$dbInstance = [String]::Empty
	$inputDirectory = [String]::Empty
	$outputDirectory = [String]::Empty
	$amcLocation = [String]::Empty
	$amcConfigDirectory = [String]::Empty
	$archiveDirectory = [String]::Empty


	try
	{
		$ScriptInvocationPath = GetScriptInvocationPath

		# Check input params, if any
		$returnValues = CheckParams
		$partnerList = $returnValues[0]
		$instanceRunDate = $returnValues[1]
		$runDateString = $instanceRunDate.ToString("yyyyMMdd")

		<#
		Write-Host("run date param: $runDate")
		Write-Host("Partner Name param: $partnerName")
		Write-Host("File Type param: $fileType")
		Write-Host("Run Date string is $RunDateString")
		Write-Host("Partner List: $partnerList")
		#>

		# Load values from configuration file
		$returnValues = LoadConfig
		$connectionString = $returnValues[0]
		$inputDirectory = $returnValues[1]
		$outputDirectory = $returnValues[2]
		$amcLocation = "`"" + $returnValues[3] + "`""
		$amcConfigDirectory = $returnValues[4]
		$amcIniTemplateFile = $returnValues[5]
		$archiveDirectory = $returnValues[6]

		<#
		Write-Host("connection string: $connectionString")
		Write-Host("Input Directory: $inputDirectory")
		Write-Host("Output Directory: $outputDirectory")
		Write-Host("AMC location: $amcLocation")
		Write-Host("AMC Config Directory: $amcConfigDirectory")
		#>

		# start (individual) X9 processing
		foreach ($partner in $PartnerList)
		{
			foreach ($fileType in $ValidFileTypes)
			{
				$inputWorkingDirectory = $inputDirectory + "\" + $RunDateString + "\" + $partner + "\" + $fileType
				#Write-Host("Working Input Directory: $inputWorkingDirectory")
				GenerateX9File -partnerName $partner -fileType $fileType -inputWorkingDirectory $inputWorkingDirectory
			}        
		}
		# Once the individual files are generated, merge them first, then archive the input files
		foreach ($partner in $PartnerList)
		{
			# Do the merges for all file types first
			# then archive the entire input directory
			foreach ($fileType in $ValidFileTypes)
			{
				# Note: the generated files are in the input directory,
				# so the start point for Merge is the input directory
				$inputWorkingDirectory = $inputDirectory + "\" + $RunDateString + "\" + $partner + "\" + $fileType
				$outputWorkingDirectory = $outputDirectory + "\" + $RunDateString + "\" + $partner
				#Write-Host("Working Input Directory: $inputWorkingDirectory")
				MergeX9Files -partnerName $partner -fileType $fileType -inputWorkingDirectory $inputWorkingDirectory -outputDirectory $outputWorkingDirectory
			}
            # Copy the notification text file whose format is mgi_$partner_yyyyMMddhhmmss.txt
            # Normally there should be only one file in this directory
            $outputWorkingDirectory = $outputDirectory + "\" + $RunDateString + "\" + $partner
            if ((Test-Path -PathType Container $outputWorkingDirectory) -ne $true)
            {
                New-Item -ItemType Directory -Path $outputWorkingDirectory | Out-Null
            }

            $outputNotificationFile = $outputWorkingDirectory + "\" + [string]::Format("mgi_{0}.{1}.{2}.txt", $partner.Replace(" ", "").ToLower(), $instanceRunDate.ToString("yyyyMMdd"), $instanceRunDate.ToString("HHmmss"))
            $inputWorkingDirectory = $inputDirectory + "\" + $RunDateString + "\" + $partner
            if (((Get-ChildItem $inputWorkingDirectory -Filter mgi_$partner*.txt | Select-Object -ExpandProperty FullName | Measure-Object).Count) -lt 2)
            {
		        $fileToCopy = (Get-ChildItem $inputWorkingDirectory -Filter mgi_$partner*.txt | Select-Object -ExpandProperty FullName)
		        Write-Host("Main::CopyNotificationFile : Copying $fileToCopy to destination $outputNotificationFile")
		        Copy-Item -Path $fileToCopy -Destination $outputNotificationFile
            }
            else
            {
                $notificationFiles = (Get-ChildItem $inputWorkingDirectory -Filter mgi_$partner*.txt | Select-Object -ExpandProperty FullName)
                $fileCount = 0
                foreach ($notificationFile in $notificationFiles)
                {
                    $outputNotificationFile = $outputWorkingDirectory + "\" + [string]::Format("mgi_{0}.{1}.{2}.{3}.txt", $partner.Replace(" ", "").ToLower(), $instanceRunDate.ToString("yyyyMMdd"), $instanceRunDate.ToString("HHmmss"), $fileCount)
                    Copy-Item -Path $notificationFile -Destination $outputNotificationFile
                    $fileCount++
                }
            }
            

			# Archive the input files to prevent the files from being included in subsequent runs of this script
			try
			{
				$inputWorkingDirectory = $inputDirectory + "\" + $RunDateString + "\" + $partner
				if ((Get-ChildItem -Recurse $inputWorkingDirectory) -eq $null)
				{
					Write-Host("ArchiveX9Files::InvalidWorkingDirectory : $inputWorkingDirectory for partner: $partner is empty or does not exist, no action taken")
					continue
				}
				$iWDNoQualifier = (Split-Path $inputWorkingDirectory -NoQualifier)
				$archiveWorkingDirectory = $archiveDirectory + $iWDNoQualifier + "\" + [DateTime]::Now.ToString("hhmmss")
				if ((Test-Path -PathType Container $archiveWorkingDirectory) -ne $true)
				{
					New-Item -ItemType Directory -Force -Path $archiveWorkingDirectory | Out-Null
				}
				$allItemsinIWD = $inputWorkingDirectory + "\*"
				Write-Host("Moving $allItemsinIWD to $archiveWorkingDirectory")
				Move-Item -Force -Path $allItemsinIWD -Destination $archiveWorkingDirectory

			}
			catch
			{
				Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception)"
			}
		}
	}
	catch
	{
		Write-Host "$($_.InvocationInfo.ScriptName)($($_.InvocationInfo.ScriptLineNumber)): $($_.InvocationInfo.Line) Exception: $($_.Exception) "
	}
# End Main