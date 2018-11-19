@echo off


REM Set Runtime Parameters
REM ----------------------

set DB_USER=<dbname>
set DB_PASSWORD=<dbpassword>
set DB_NAME=KaspickDB
set DB_SERVER=10.10.9.71
set ExcelsiorDB=ExcelsiorDB
Set InnoTrustDB=innotrust_repl
Set MappingDB=ExcelsiorInnotrust_MappingDB
Set GiftwrapDB=GiftwrapMask
Set ProfileStageDB=ProfileStageDB

call:SQLExecTime
Echo %datetimef% -- KaspickDB Execution Started.. 
Echo %datetimef% -- KaspickDB Execution Started.. >> log\TimeStamp.txt

call:SQLExecTime
Echo %datetimef% -- Executing KaspickDB schema Scripts 
Echo %datetimef% -- Executing KaspickDB schema Scripts >> log\TimeStamp.txt

Rem ******** Schema Changes*****************

SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -i ..\..\KaspickDB_ModelScripts\Table.sql -o Log\Table.log
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -i ..\..\KaspickDB_ModelScripts\MVD_Table.sql -o Log\MVD_Table.log
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v MappingDB = %MappingDB% -i ..\..\KaspickDB_ModelScripts\MappingDB_Table.sql -o Log\MappingDB_Table.log
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v GiftwrapDB = %GiftwrapDB% InnoTrustDB = %InnoTrustDB% ExcelsiorDB = %ExcelsiorDB% ProfileStageDB = %ProfileStageDB% -i ..\..\KaspickDB_ModelScripts\Synonym.sql -o Log\Synonym.log
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -i ..\..\KaspickDB_ModelScripts\View.sql -o Log\View.log


call:SQLExecTime
Echo %datetimef% -- Executing KaspickDB common Functions.. 
Echo %datetimef% -- Executing KaspickDB common Functions.. >> log\TimeStamp.txt

Rem ******** Function *****************

for /f "delims=" %%f in ('dir /b ..\..\KaspickDB_CommonScripts\Functions') do (SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i ..\..\KaspickDB_CommonScripts\Functions\%%f -o Log\%%f.log)


call:SQLExecTime
Echo %datetimef% -- Executing Excelsior to KaspickDB Migration Scripts.. 
Echo %datetimef% -- Executing Excelsior to KaspickDB Migration Scripts.. >> log\TimeStamp.txt

Rem ******** Data Migration ************
call:SQLExecTime
Echo %datetimef% -- 1.1.KasperMigrationScript.sql 
Echo %datetimef% -- 1.1.KasperMigrationScript.sql  >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% MappingDB=%MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.1.KasperMigrationScript.sql -o Log\1.1.KasperMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 1.2.AccountPayountManualClearScript.sql 
Echo %datetimef% -- 1.2.AccountPayountManualClearScript.sql %>> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  MappingDB = %MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.2.AccountPayountManualClearScript.sql -o Log\1.2.AccountPayountManualClearScript.log

call:SQLExecTime
Echo %datetimef% -- 1.3.LookUpMigrationScript.sql 
Echo %datetimef% -- 1.3.LookUpMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  MappingDB = %MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.3.LookUpMigrationScript.sql -o Log\1.3.LookUpMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 1.3.1ListTypeListItemInsertScript.sql 
Echo %datetimef% -- 1.3.1ListTypeListItemInsertScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.3.1ListTypeListItemInsertScript.sql -o Log\1.3.1ListTypeListItemInsertScript.log

call:SQLExecTime
Echo %datetimef% -- 1.4.PolicyMigrationScript.sql 
Echo %datetimef% -- 1.4.PolicyMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.4.PolicyMigrationScript.sql -o Log\1.4.PolicyMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 1.5.MiddlewareMigrationScript.sql 
Echo %datetimef% -- 1.5.MiddlewareMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  MappingDB = %MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.5.MiddlewareMigrationScript.sql -o Log\1.5.MiddlewareMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 1.6.ParagonMigrationScript.sql 
Echo %datetimef% -- 1.6.ParagonMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  MappingDB = %MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.6.ParagonMigrationScript.sql -o Log\1.6.ParagonMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 1.7.ParagonDataLogScript.sql 
Echo %datetimef% -- 1.7.ParagonDataLogScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v MappingDB = %MappingDB% ExcelsiorDB = %ExcelsiorDB%  -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.7.ParagonDataLogScript.sql -o Log\1.7.ParagonDataLogScript.log


call:SQLExecTime
Echo %datetimef% -- 1.8.PolicyItemMigrationScript.sql 
Echo %datetimef% -- 1.8.PolicyItemMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% InnoTrustDB = %InnoTrustDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.8.PolicyItemMigrationScript.sql -o Log\1.8.PolicyItemMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 1.9.MiddlewareAuthorizationMatrixScripts.sql 
Echo %datetimef% -- 1.9.MiddlewareAuthorizationMatrixScripts.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.9.MiddlewareAuthorizationMatrixScripts.sql -o Log\1.9.MiddlewareAuthorizationMatrixScripts.log

call:SQLExecTime
Echo %datetimef% -- 1.91.PaymentProfileAuthorizationScripts.sql 
Echo %datetimef% -- 1.91.PaymentProfileAuthorizationScripts.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\1.91.PaymentProfileAuthorizationScripts.sql -o Log\1.91.PaymentProfileAuthorizationScripts.log

call:SQLExecTime
Echo %datetimef% -- 2.1.InvMgmt_MigrationScripts.sql 
Echo %datetimef% -- 2.1.InvMgmt_MigrationScripts.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\2.1.InvMgmt_MigrationScripts.sql -o Log\2.1.InvMgmt_MigrationScripts.log

call:SQLExecTime
Echo %datetimef% -- 3.1.TRex_MigrationRules_v1.sql 
Echo %datetimef% -- 3.1.TRex_MigrationRules_v1.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\3.1.TRex_MigrationRules_v1.sql -o Log\3.1.TRex_MigrationRules_v1.log

call:SQLExecTime
Echo %datetimef% -- 4.1.Reportoire_MigrationScripts.sql 
Echo %datetimef% -- 4.1.Reportoire_MigrationScripts.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  MappingDB = %MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\4.1.Reportoire_MigrationScripts.sql -o Log\4.1.Reportoire_MigrationScripts.log

call:SQLExecTime
Echo %datetimef% -- 5.1.EmailEngine_MigrationScript.sql 
Echo %datetimef% -- 5.1.EmailEngine_MigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\5.1.EmailEngine_MigrationScript.sql -o Log\5.1.EmailEngine_MigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 6.1.WFT_MigrationScript.sql 
Echo %datetimef% -- 6.1.WFT_MigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  MappingDB = %MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\6.1.WFT_MigrationScript.sql -o Log\6.1.WFT_MigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 7.1.MVD_MigrationScript.sql 
Echo %datetimef% -- 7.1.MVD_MigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\7.1.MVD_MigrationScript.sql -o Log\7.1.MVD_MigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 8.1.PevaMigrationScript.sql 
Echo %datetimef% -- 8.1.PevaMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\8.1.PevaMigrationScript.sql -o Log\8.1.PevaMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 9.1.3.ApplicationInnoTrustKeyInsterScript.sql 
Echo %datetimef% -- 9.1.3.ApplicationInnoTrustKeyInsterScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.1.3.ApplicationInnoTrustKeyInsterScript.sql -o Log\9.19.1.3.ApplicationInnoTrustKeyInsterScript.log

call:SQLExecTime
Echo %datetimef% -- 9.1.4.IncomeEstimateMigrationScript.sql 
Echo %datetimef% -- 9.1.4.IncomeEstimateMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% InnoTrustDB = %InnoTrustDB%  -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.1.4.IncomeEstimateMigrationScript.sql -o Log\9.1.4.IncomeEstimateMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 9.1.5.KatanaMigrationScript.sql 
Echo %datetimef% -- 9.1.5.KatanaMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.1.5.KatanaMigrationScript.sql -o Log\9.1.5.KatanaMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 9.1.6.PGCalcInnotrustContactImportScript.sql 
Echo %datetimef% -- 9.1.6.PGCalcInnotrustContactImportScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v MappingDB = %MappingDB% GiftwrapDB = %GiftwrapDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.1.6.PGCalcInnotrustContactImportScript.sql -o Log\9.1.6.PGCalcInnotrustContactImportScript.log

call:SQLExecTime
Echo %datetimef% -- 9.1.7.OFACLookupMigrationScript.sql 
Echo %datetimef% -- 9.1.7.OFACLookupMigrationScript.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v  ExcelsiorDB = %ExcelsiorDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.1.7.OFACLookupMigrationScript.sql -o Log\9.1.7.OFACLookupMigrationScript.log

call:SQLExecTime
Echo %datetimef% -- 9.9.1.HistoricalGifts1.sql 
Echo %datetimef% -- 9.9.1.HistoricalGifts1.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.9.1.HistoricalGifts1.sql -o Log\9.9.1.HistoricalGifts1.log

call:SQLExecTime
Echo %datetimef% -- 9.9.2.HistoricalGifts2.sql 
Echo %datetimef% -- 9.9.2.HistoricalGifts2.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.9.2.HistoricalGifts2.sql -o Log\9.9.2.HistoricalGifts2.log

call:SQLExecTime
Echo %datetimef% -- 9.9.3.1.HistoricalGifts3.sql 
Echo %datetimef% -- 9.9.3.1.HistoricalGifts3.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB%  MappingDB = %MappingDB% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.9.3.1.HistoricalGifts3.sql -o Log\9.9.3.1.HistoricalGifts3.log


call:SQLExecTime
Echo %datetimef% -- 9.9.3.TBL_Lookup_DataRefreshJobControl.sql 
Echo %datetimef% -- 9.9.3.TBL_Lookup_DataRefreshJobControl.sql >> log\TimeStamp.txt
SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -i ..\..\BackOfficeIntegration_Migration\ExcelsiorDB_To_KaspickDB\9.9.3.TBL_Lookup_DataRefreshJobControl.sql -o Log\9.9.3.TBL_Lookup_DataRefreshJobControl.log




call:SQLExecTime
Echo %datetimef% -- Executing common Stored procedure Scripts.. 
Echo %datetimef% -- Executing common Stored procedure Scripts.. >> log\TimeStamp.txt

Rem ******** StoredProcedures *****************

for /f "delims=" %%f in ('dir /b ..\..\KaspickDB_CommonScripts\StoredProcedures') do (SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i ..\..\KaspickDB_CommonScripts\StoredProcedures\%%f -o Log\%%f.log)


call:SQLExecTime
Echo %datetimef% -- Executing Application Scripts.. 
Echo %datetimef% -- Executing Application Scripts.. >> log\TimeStamp.txt

call:SQLExecTime
Echo %datetimef% -- Executing Kasper Application Scripts.. 
Echo %datetimef% -- Executing Kasper Application Scripts.. >> log\TimeStamp.txt

call ..\..\KaspickDB_ApplicationScripts\Kasper\Kasper.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%
call:SQLExecTime
Echo %datetimef% -- Executing Paragon Application Scripts.. 
Echo %datetimef% -- Executing Paragon Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\Paragon\paragon.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing KCoEmailEngine Application Scripts.. 
Echo %datetimef% -- Executing KCoEmailEngine Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\KCoEmailEngine\KCoEmailEngine.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing KCoEmailServices Application Scripts.. 
Echo %datetimef% -- Executing KCoEmailServices Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\KCoEmailServices\KCoEmailServices.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing PEVA Application Scripts.. 
Echo %datetimef% -- Executing PEVA Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\PEVA\PEVA.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing Reportoire Application Scripts.. 
Echo %datetimef% -- Executing Reportoire Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\Reportoire\Reportoire.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing SalesForceService Application Scripts.. 
Echo %datetimef% -- Executing SalesForceService Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\SalesForceService\SalesForceService.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing Trex Application Scripts.. 
Echo %datetimef% -- Executing Trex Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\Trex\Trex.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing TradeRebalance Application Scripts.. 
Echo %datetimef% -- Executing TradeRebalance Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\TradeRebalance\TradeRebalance.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing IncomeEstimation Application Scripts.. 
Echo %datetimef% -- Executing IncomeEstimation Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\IncomeEstimation\IncomeEstimation.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing Excelsior Prime Application Scripts.. 
Echo %datetimef% -- Executing Excelsior Prime Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\Excelsior\Excelsior.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER%

call:SQLExecTime
Echo %datetimef% -- Executing ClientOutlookContact Application Scripts.. 
Echo %datetimef% -- Executing ClientOutlookContact Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\ClientOutlookContact\ClientOutlookContact.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER% 

call:SQLExecTime
Echo %datetimef% -- Executing Katana Application Scripts.. 
Echo %datetimef% -- Executing Katana Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\Katana\Katana.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER% 


call:SQLExecTime
Echo %datetimef% -- Executing KCoReports Application Scripts.. 
Echo %datetimef% -- Executing KCoReports Application Scripts.. >> log\TimeStamp.txt
call ..\..\KaspickDB_ApplicationScripts\KCoReports\KCoReports.bat %DB_USER% %DB_PASSWORD% %DB_NAME% %DB_SERVER% 


call:SQLExecTime
Echo %datetimef% -- KaspickDB Execution completed.. 
Echo %datetimef% -- KaspickDB Execution completed.. >> log\TimeStamp.txt


Echo Verifying the Log file for errors 

Rem ******** Execute Errorlog ************

findstr /s /i /n /g:FindText.txt Log/*.* > Error.log

Echo Verificaton completed.. Please verify the Error.log file for any errors. Thank you!!


Rem ======Authorization Matrix for Excelsior DB==========================



Rem ======================================================

echo.&pause&goto:eof


:SQLExecTime
set hour=%time:~0,2%
if "%hour:~0,1%" == " " set datetimef=%time:~1,1%:%time:~3,2%:%time:~6,2%
if "%hour:~0,1%" NEQ " " set datetimef=%time:~0,2%:%time:~3,2%:%time:~6,2%
goto:eof



