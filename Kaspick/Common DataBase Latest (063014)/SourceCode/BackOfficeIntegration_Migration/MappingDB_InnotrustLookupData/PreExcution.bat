
REM Set Runtime Parameters
REM ----------------------

@echo off

set DB_USER=DBUser

set DB_PASSWORD=DBUser5

set DB_NAME=MappingDB9

set DB_SERVER=10.82.26.136

set ExcelsiorDB=Excelsior_CBR_DB

Set InnoTrustDB=innotrust_repl



Rem ******** MappingDB Scripts *****************

for /f "delims=" %%f in ('dir /b *.sql') do (SQLCMD -S %DB_SERVER% -U %DB_USER% -P "%DB_PASSWORD%" -d %DB_NAME% -v ExcelsiorDB = %ExcelsiorDB% InnoTrustDB=%InnoTrustDB% -i %%f -o Log\%%f.log)


Rem ======================================================


Pause

