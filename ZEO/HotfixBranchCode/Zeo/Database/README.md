Database - schema migrations with support for rollback using Liquibase
======================================================================

To Do
-----
* need to figure out how to handle synonyms referencing other DBs
* research automated rollback strategies; for example, when reinstalling an older DB package version than the one currently installed

Prerequisites
-------------
* PowerShell 3.0+
* Liquibase 3.0+
* Java 6+
* JDBC driver

Getting Started
---------------
* Download and install version 6+ of the Java runtime here: http://java.com
* Download and install the latest version of Liquibase here: http://www.liquibase.org/download/index.html
* Set the environment variable, **LIQUIBASE_HOME**, to the Liquibase install directory
* Add the Liquibase install directory to the **PATH** environment variable
* Confirm that the host's execution policy allows PowerShell scripts to be run using any of the following methods: https://blog.netspi.com/15-ways-to-bypass-the-powershell-execution-policy/

Migrations (Bootstrapped DB creation)
-------------------------------------

	# Creates 6 databases prefixed with 'ENV' and 'owner' login
	PS C:\> .\DB-Migrate.ps1 -Bootstrap -Server localhost -Creator creator -CreatorPassword secret -Owner owner -OwnerPassword secret -Prefix DEV COMPLIANCE CXE CXN ODS PCAT PTNR

Migrations (Existing DBs)
-------------------------
	# Runs migrations on only the DEV_COMPLIANCE database
	PS C:\> .\DB-Migrate.ps1 -Server localhost -Prefix DEV COMPLIANCE

Rollback
--------
	*Note: rollbacks should be done manually and with great care using the Liquibase script directly*
	
	# Roll back the last 3 change sets
	liquibase --url="jdbc:sqlserver://localhost;databaseName=DEV_COMPLIANCE" --changeLogFile=COMPLIANCE\changelogs\changelog-master.xml rollbackCount 3

	# Roll back database to the state tagged 'COMPLIANCE-3.7.0.38'
	liquibase --url="jdbc:sqlserver://localhost;databaseName=DEV_COMPLIANCE" --changeLogFile=COMPLIANCE\changelogs\changelog-master.xml rollback COMPLIANCE-3.7.0.38
	
Resources
---------
1. Liquibase - http://www.liquibase.org/index.html
