
REM Set Runtime Parameters
REM ----------------------

@echo off

set DB_USER=DBUser

set DB_PASSWORD=Password

set DB_NAME=MappingDB Name

set DB_SERVER=10.10.9.71

set ExcelsiorDB=Excelsior_CBR_DB

Set InnoTrustDB=innotrust_repl


Rem ******** MappingDB Scripts *****************

SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% InnoTrustDB=%InnoTrustDB% -i PostExecute\InsertAccountLookup.sql  -o Log\InsertAccountLookup.log

SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v MappingDB = %DB_NAME%  -v ExcelsiorDB = %ExcelsiorDB% -i PostExecute\UpdateTrustAdvisorID.sql -o Log\UpdateTrustAdvisorID.log

SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v MappingDB = %DB_NAME%  -v InnoTrustDB=%InnoTrustDB%   -i PostExecute\Update_TBL_BeneficiaryLookup.sql -o Log\Update_TBL_BeneficiaryLookup.log


Rem ======================================================


Pause

