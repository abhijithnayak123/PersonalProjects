# Author: 			Bineesh Raghavan
# Description: 		To delete receipt templates from mongo server
# Date:				10/13/2014

# Input Parameters
# receiptList: 		Text file which contains the list of receipt templates to be deleted	
# serverAddress:	Mongo server IP address
# bucketName:		Mongo server bucket name which contains receipts to be deleted

# Sample:
# .\DeleteReceipts.ps1 E:\Temp\Receipts.txt proxy.ic.local Dev-Alpha

[CmdletBinding()]
Param( 
	[Parameter(Mandatory=$True,Position=1)]
	[string]$receiptList,
	
	[Parameter(Mandatory=$True,Position=2)]
	[string]$serverAddress,
	
	[Parameter(Mandatory=$True,Position=3)]
	[string]$bucketName
)

$fileEntries = Get-Content $receiptList

for($i = 0; $i -lt $fileEntries.count; $i++)
{
	$filename = $fileEntries[$i]
	$webUri = "http://" + $serverAddress + "/receipts/gridfs?dbname=MGI_ReceiptsRepo&bucketname=" + $bucketName + "&filename=" + $filename
	$outfile = $receiptFolder + "\" + $filename
	$status = "Deleting file " + $filename
	
	Write-Progress -Activity "Deleting receipts from Mongo." -status $status -percentComplete ($i / $fileEntries.count * 100)

	$result = Invoke-RestMethod -Uri $webUri -Method Delete 
		
	if ($result.status -eq "success")
	{
		Write-Host "The file $filename deleted from server $serverAddress successfully"
	}
	else
	{
		Write-Host "The file $filename deletion from server $serverAddress is failed" -foregroundcolor red
	}
}
