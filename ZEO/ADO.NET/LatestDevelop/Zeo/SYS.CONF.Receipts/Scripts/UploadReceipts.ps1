# Author: 			Bineesh Raghavan
# Description: 		To upload receipt templates to mongo server
# Date:				03/20/2014

# Input Parameters
# receiptList: 		Folder path contains receipts files	
# serverAddress:	Mongo server IP address
# bucketName:		Mongo server bucket name which contains receipts to be uploaded

# Sample:
# .\UploadReceipts.ps1 D:\Nexxo\mitra\SYS.CONF.Receipts\Receipts proxy.ic.local Dev-Alpha 

[CmdletBinding()]
Param( 
	[Parameter(Mandatory=$True,Position=1)]
	[string]$filePath,
	
	[Parameter(Mandatory=$True,Position=2)]
	[string]$serverAddress,
	
	[Parameter(Mandatory=$True,Position=3)]
	[string]$bucketName
)

$ErrorActionPreference= 'silentlycontinue'

$fileEntries = [IO.Directory]::GetFiles($filePath); 

for($i = 0; $i -lt $fileEntries.count; $i++)
{
	$fileName = $fileEntries[$i];
	
	$name = $fileName.Replace($filePath, "").Trim("\\");
	    	
	$webUri = "http://" + $serverAddress + "/receipts/gridfs?dbname=MGI_ReceiptsRepo&bucketname=" + $bucketName + "&filename=" + $name
	
	$status = "Uploading file " + $name
	
	Write-Progress -Activity "Uploading receipts from Mongo..." -status $status -percentComplete ($i / $fileEntries.count * 100)
	
	$result = Invoke-RestMethod -Uri $webUri -Method Delete
	
	if ($result.status -eq "success")
	{
	    $result = Invoke-RestMethod -Uri $webUri -Method Put -InFile $fileName -ContentType "text/html"
	}
	
	if ($result.status -eq "success")
	{
		Write-Host "The file $name uploaded to server $serverAddress successfully"
	}
	else
	{
		Write-Host "Upload of the file $name to server $serverAddress is failed" -foregroundcolor red
	}	
}
