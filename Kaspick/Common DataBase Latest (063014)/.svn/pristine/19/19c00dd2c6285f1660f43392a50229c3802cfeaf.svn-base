
REM Set Runtime Parameters
REM ----------------------

@echo on

set DB_USER=EDW_Users

set DB_PASSWORD=K@spick123

set DB_NAME=KaspickDB_Lookup

set DB_SERVER=10.10.9.71



Rem ******** MappingDB Scripts *****************

SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i Disable_Lookup_Constraints.sql  -o Log\Disable_Lookup_Constraints.log


SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%  -i TBL_Lookup_AccountType.sql  -o Log\TBL_Lookup_AccountType.log


Rem ======================================================


Pause

