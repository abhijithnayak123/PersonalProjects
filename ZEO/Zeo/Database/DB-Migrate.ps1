#REQUIRES -Version 3.0

<#
.SYNOPSIS
    Database migration tool
.DESCRIPTION  
    Applies schema migrations and optionally bootstraps DB + user creation
.LINK  
    https://github.com/NexxoFinancial/mitra.git
.SYNTAX
    # bootstrap databases and apply migrations
    DB-Migrate.ps1 -Server <server> -Creator <DB Creator> -CreatorPassword <DB Creator Password> -Owner <DB Owner> -OwnerPassword <DB Owner Password> -Prefix <Database Prefix> Database1 Database2 ... DatabaseN
    
    # apply migrations only
    DB-Migrate.ps1 -Server <server> -Prefix <Database Prefix> Database1 Database2 ... DatabaseN
.EXAMPLE    
    # bootstrap databases and apply migrations
    DB-Migrate.ps1 -Bootstrap -Server localhost\instance -Creator creator -CreatorPassword secret -Owner owner -OwnerPassword secret -Prefix DB USERS PRODUCTS

    # apply migrations only
    DB-Migrate.ps1 -Server localhost\instance -Prefix DB USERS PRODUCTS
#>

[CmdletBinding(DefaultParameterSetName="migrate")]
Param (
    [Parameter(ParameterSetName="bootstrap")]
    [Switch] $Bootstrap,
    
    [Parameter(Mandatory=$true)]
    [String] $Server,

    [Parameter(ParameterSetName="bootstrap", Mandatory=$true)]
    [String] $Creator,
    
    [Parameter(ParameterSetName="bootstrap", Mandatory=$true)]
    [String] $CreatorPassword,

    [Parameter(ParameterSetName="bootstrap", Mandatory=$true)]
    [String] $Owner,
    
    [Parameter(ParameterSetName="bootstrap", Mandatory=$true)]
    [String] $OwnerPassword,
    
    [Parameter(Mandatory=$true)]
    [String] $Prefix,

    [Parameter(ValueFromRemainingArguments=$true)]
    [String[]] $Databases
)

# General settings
$liquibaseCmd = "${Env:LIQUIBASE_HOME}\liquibase.bat"
$jdbcUrlPrefix = "jdbc:sqlserver"
$changelogPath = "changelogs/changelog-master.xml"

# Import the SQL Server module
Push-Location
Import-Module “sqlps” -DisableNameChecking
Pop-Location

# Write colorized log messages
Function Write-Log {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory=$true)]
        [String] $Message,
        [String] $Detail
    )

    Write-Host "=> " -ForegroundColor Blue -NoNewline
    Write-Host "$Message" -ForegroundColor White -NoNewline
    
    If ($Detail) {
        Write-Host "$Detail" -ForegroundColor Green -NoNewline
    }
    
    Write-Host
}

If ($Bootstrap) {
    # Create database owner
    $createUserQuery = @"
    IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = N'$Owner')
    CREATE LOGIN [$Owner] WITH PASSWORD = N'$OwnerPassword';
"@

    Write-Log -Message "Creating database owner: " -Detail "${Owner}"
    Invoke-Sqlcmd -ServerInstance $Server -Username $Creator -Password $CreatorPassword -Query $createUserQuery
}

## Create databases
ForEach ($database in $Databases) {
    # add prefix
    $fullDbName = "${Prefix}_${database}"

    If ($Bootstrap) {
        $createDbQuery = @"
        IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'$fullDbName')
        CREATE DATABASE [$fullDbName];
        GO

        USE $fullDbName;

        IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = N'$Owner')
        BEGIN
            CREATE USER [$Owner] FOR LOGIN [$Owner];
            EXEC sp_addrolemember 'db_owner', '$Owner';
        END
"@

        Write-Log -Message "Creating database: " -Detail "$fullDbName"
        Invoke-Sqlcmd -ServerInstance $Server -Username $Creator -Password $CreatorPassword -Query $createDbQuery
    }
    
    $jdbcUrl = "${jdbcUrlPrefix}://${Server};databaseName=${fullDbName}"
    $changeLogFile = "${database}/${changeLogPath}"
    
    # run migrations (update) on database
    Write-Log -Message "Migrating database: " -Detail "$fullDbName"
    & "$liquibaseCmd" "--url=`"${jdbcUrl}`"" --changeLogFile=${changeLogFile} update
}

$result = if ($LASTEXITCODE -eq 0) { "SUCCESS" } else { "FAILURE"}

Write-Log -Message "Result: " -Detail "$result"

exit $LASTEXITCODE