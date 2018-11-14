# Steps to run this script
# 1. Find the Receipts.config file in the SYS.CONF.Receipts/Scripts/ directory and change the database connection string appropriately.
# 2. Create a folder and keep all the receipts which are to be uploaded and use that folder path in the Receipts.config against the key "ReceiptsToBeUploaded".
# 3. Run the powershell script.

Try
{
    # get the directory of this script file
    $currentDirectory = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Path)

    # get the full path and file name of the App.config file in the same directory as this script
    $appConfigFile = [IO.Path]::Combine($currentDirectory, 'Receipts.config')

    Write-Host 'Reading the Receipts.config file..'

    # initialize the xml object
    $appConfig = New-Object XML

    # load the config file as an xml object
    $appConfig.Load($appConfigFile)

    Write-Host 'Reading the connection string from config file..'

    # Storing the Connection string
    $DBConnectionString = $appConfig.configuration.connectionStrings.add.connectionString

    #FilePath where in the receipts are stored which are to be uploaded
    $FilePath = $appConfig.configuration.appSettings.add | where {$_.key -eq 'ReceiptsToBeUploaded' }

    Write-Host 'getting the receipts from '$FilePath.value 

    $files = Get-ChildItem -Path $FilePath.value
    Write-Host 'Uploading the receipts to the database..'
    Write-Host 'Please wait..'
    

    #Creating the SQL connection object
    $SqlConnection = new-object System.Data.SqlClient.SqlConnection

    #assigning the connection string to the sql connection object
    $SqlConnection.ConnectionString = $DBConnectionString


	$SqlConnection.Open()

	ForEach($file in $files)
	 {
		Write-Host 'Uploading receipt :' $file.Name

		$date = Get-Date

		$TemplateContent = [System.IO.File]::ReadAllBytes($FilePath.value+ "/" +$file.Name)   

		$SqlCommand = $SqlConnection.CreateCommand()

		$SqlCommand.CommandText = "EXEC dbo.usp_UploadReceiptTemplate @templateName, @template,@dtServerDate"
	 
		# Add parameters to pass values to the stored procedure
		$SqlCommand.Parameters.AddWithValue("@templateName",$file.Name ) | Out-Null

		$SqlCommand.Parameters.AddWithValue("@template", $TemplateContent) | Out-Null

		$SqlCommand.Parameters.AddWithValue("@dtServerDate",$date ) | Out-Null

		$SqlCommand.ExecuteNonQuery()| Out-Null
	}

	$SqlConnection.Close()
	
	Write-Host 'Receipts upload to the database success..'
}
Catch
{
	write-host 'Receipts upload failed due to the below error..' -ForegroundColor "Red"
	write-host 'Exception Message :'$_.Exception.Message -ForegroundColor "Red"
}

Read-Host -Prompt "Press Enter to exit"
