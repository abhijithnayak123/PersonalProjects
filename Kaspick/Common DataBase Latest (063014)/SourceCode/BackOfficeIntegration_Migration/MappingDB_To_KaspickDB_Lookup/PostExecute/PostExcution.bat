
REM Set Runtime Parameters
REM ----------------------

@echo on

set DB_USER=EDW_Users

set DB_PASSWORD=K@spick123

set DB_NAME=KaspickDB_Lookup

set DB_SERVER=10.10.9.71



Rem ******** MappingDB Scripts *****************

SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%   -i Delete_Duplicate_Lookup_TableData.sql  -o Log\Delete_Duplicate_Lookup_TableData.log

SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME%   -i Enable_Lookup_Constraints.sql  -o Log\Enable_Lookup_Constraints.log




Rem ======================================================


Pause

