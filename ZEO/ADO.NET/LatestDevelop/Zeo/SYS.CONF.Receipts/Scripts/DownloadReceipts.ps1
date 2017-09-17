# Author: 			Bineesh Raghavan
# Description: 		To download receipt templates from mongo server
# Date:				03/20/2014

# Input Parameters
# receiptList: 		Text file which contains the list of receipt file names to be downloaded	
# serverAddress:	Mongo server IP address
# bucketName:		Mongo server bucket name which contains receipts to be downloaded
# receiptFolder:	Folder path where receipts to be downloaded

# Sample:
# .\DownloadReceipts.ps1 E:\Temp\Receipts.txt proxy.ic.local Dev-Alpha D:\Temp\Receipts 

[CmdletBinding()]
Param( 
	[Parameter(Mandatory=$True,Position=1)]
	[string]$receiptList,
	
	[Parameter(Mandatory=$True,Position=2)]
	[string]$serverAddress,
	
	[Parameter(Mandatory=$True,Position=3)]
	[string]$bucketName,
	
	[Parameter(Mandatory=$True,Position=4)]
	[string]$receiptFolder
)

$fileEntries = Get-Content $receiptList

for($i = 0; $i -lt $fileEntries.count; $i++)
{
	$webUri = "http://" + $serverAddress + "/receipts/gridfs?dbname=MGI_ReceiptsRepo&bucketname=" + $bucketName + "&filename=" + $fileEntries[$i]
	$outfile = $receiptFolder + "\" + $fileEntries[$i]
	$status = "Downloading file " + $fileEntries[$i]
	
	Write-Progress -Activity "Downloading receipts from Mongo." -status $status -percentComplete ($i / $fileEntries.count * 100)

	Invoke-RestMethod -Uri $webUri -Method Get -OutFile $outfile
}
