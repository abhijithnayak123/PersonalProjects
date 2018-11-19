REM --  This gets the Environments Variable set in the nested batch bat using SET
REM ----------------------

@echo off

set DB_USER=%DB_USER%
set DB_PASSWORD=%DB_PASSWORD%
set DB_NAME=%DB_NAME%
set DB_SERVER=%DB_SERVER%



Rem ******** Function *****************

for /f "delims=" %%f in ('dir /b ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\Functions\') do (SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\Functions\%%f -o ..\..\DeploymentBatchScripts\BackOfficeIntegration\Log\ClientOutlookContact\%%f.log)

Rem ******** Migration *****************

for /f "delims=" %%f in ('dir /b ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\Migration\') do (SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\Migration\%%f -o ..\..\DeploymentBatchScripts\BackOfficeIntegration\Log\ClientOutlookContact\%%f.log)

Rem ******** StoredProcedures*****************

for /f "delims=" %%f in ('dir /b ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\StoredProcedures\') do (SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\StoredProcedures\%%f -o ..\..\DeploymentBatchScripts\BackOfficeIntegration\Log\ClientOutlookContact\%%f.log)

Rem ******** Triggers*****************

for /f "delims=" %%f in ('dir /b ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\Triggers\') do (SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\Triggers\%%f -o ..\..\DeploymentBatchScripts\BackOfficeIntegration\Log\ClientOutlookContact\%%f.log)
