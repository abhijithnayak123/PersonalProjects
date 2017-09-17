If '$RestoreDB$' = 'true'
BEGIN
----------------------------------------------------------------------------------------------------------------
--Declaretion section
----------------------------------------------------------------------------------------------------------------
CREATE TABLE #tmpFilelist (
    LogicalName varchar(64), PhysicalName varchar(130), [Type] varchar(1), FileGroupName varchar(64), Size decimal(20, 0)
    ,MaxSize decimal(25, 0), FileID bigint, CreateLSN decimal(25,0), DropLSN decimal(25,0), UniqueID uniqueidentifier
    ,ReadOnlyLSN decimal(25,0), ReadWriteLSN decimal(25,0), BackSizeInBytes decimal(25,0), SourceBlockSize int
    ,filegroupid int, loggroupguid uniqueidentifier, differentialbaseLSN decimal(25,0), differentialbaseGUID uniqueidentifier
    ,isreadonly bit, ispresent bit, TDEThumbpr decimal
)

Declare @PhysicalNameMdf varchar(250)
Declare @PhysicalNameLdf varchar(250)
Declare @LogicalNameMdf varchar(250)
Declare @LogicalNameLdf varchar(250)
----------------------------------------------------------------------------------------------------------------
--Catalog Restore
----------------------------------------------------------------------------------------------------------------
USE $DBPrifix$_PCAT
TRUNCATE TABLE #tmpFilelist
INSERT INTO #tmpFilelist
exec('RESTORE FILELISTONLY FROM DISK = ''$WorkingDirectory$\DBBackup\Dev_PCAT.bak'' WITH  FILE = 1 ')
SET @PhysicalNameMdf=(SELECT physical_name from sys.database_files where type=0) -- 0 Stands for MDF
SET @PhysicalNameLdf=(SELECT physical_name from sys.database_files where type=1) -- 1 Stands for LDF
SET @LogicalNameMdf= (SELECT LogicalName from #tmpFilelist where Type='D')--D = SQL Server data file
SET @LogicalNameLdf= (SELECT LogicalName from #tmpFilelist where Type='L')--L = Microsoft SQL Server log file

USE MASTER
RESTORE DATABASE $DBPrifix$_PCAT  
FROM DISK='$WorkingDirectory$\DBBackup\Dev_PCAT.bak'
WITH 
REPLACE,FILE = 1,
MOVE @LogicalNameMdf TO @PhysicalNameMdf,
MOVE @LogicalNameLdf TO @PhysicalNameLdf

----------------------------------------------------------------------------------------------------------------
--COMPLIANCE Restore
----------------------------------------------------------------------------------------------------------------
USE $DBPrifix$_COMPLIANCE
TRUNCATE TABLE #tmpFilelist
INSERT INTO #tmpFilelist
exec('RESTORE FILELISTONLY FROM DISK = ''$WorkingDirectory$\DBBackup\Dev_COMPLIANCE.bak'' WITH  FILE = 1 ')
SET @PhysicalNameMdf=(SELECT physical_name from sys.database_files where type=0) -- 0 Stands for MDF
SET @PhysicalNameLdf=(SELECT physical_name from sys.database_files where type=1) -- 1 Stands for LDF
SET @LogicalNameMdf= (SELECT LogicalName from #tmpFilelist where Type='D')--D = SQL Server data file
SET @LogicalNameLdf= (SELECT LogicalName from #tmpFilelist where Type='L')--L = Microsoft SQL Server log file

USE MASTER
RESTORE DATABASE $DBPrifix$_COMPLIANCE  
FROM DISK='$WorkingDirectory$\DBBackup\Dev_COMPLIANCE.bak'
WITH 
REPLACE,FILE = 1,
MOVE @LogicalNameMdf TO @PhysicalNameMdf,
MOVE @LogicalNameLdf TO @PhysicalNameLdf

----------------------------------------------------------------------------------------------------------------
--CXE Restore
----------------------------------------------------------------------------------------------------------------
USE $DBPrifix$_CXE
TRUNCATE TABLE #tmpFilelist
INSERT INTO #tmpFilelist
exec('RESTORE FILELISTONLY FROM DISK = ''$WorkingDirectory$\DBBackup\Dev_CXE.bak'' WITH  FILE = 1 ')
SET @PhysicalNameMdf=(SELECT physical_name from sys.database_files where type=0) -- 0 Stands for MDF
SET @PhysicalNameLdf=(SELECT physical_name from sys.database_files where type=1) -- 1 Stands for LDF
SET @LogicalNameMdf= (SELECT LogicalName from #tmpFilelist where Type='D')--D = SQL Server data file
SET @LogicalNameLdf= (SELECT LogicalName from #tmpFilelist where Type='L')--L = Microsoft SQL Server log file

USE MASTER
RESTORE DATABASE $DBPrifix$_CXE  
FROM DISK='$WorkingDirectory$\DBBackup\Dev_CXE.bak'
WITH 
REPLACE,FILE = 1,
MOVE @LogicalNameMdf TO @PhysicalNameMdf,
MOVE @LogicalNameLdf TO @PhysicalNameLdf

----------------------------------------------------------------------------------------------------------------
--CXN Restore
----------------------------------------------------------------------------------------------------------------
USE $DBPrifix$_CXN
TRUNCATE TABLE #tmpFilelist
INSERT INTO #tmpFilelist
exec('RESTORE FILELISTONLY FROM DISK = ''$WorkingDirectory$\DBBackup\Dev_CXN.bak'' WITH  FILE = 1 ')
SET @PhysicalNameMdf=(SELECT physical_name from sys.database_files where type=0) -- 0 Stands for MDF
SET @PhysicalNameLdf=(SELECT physical_name from sys.database_files where type=1) -- 1 Stands for LDF
SET @LogicalNameMdf= (SELECT LogicalName from #tmpFilelist where Type='D')--D = SQL Server data file
SET @LogicalNameLdf= (SELECT LogicalName from #tmpFilelist where Type='L')--L = Microsoft SQL Server log file

USE MASTER
RESTORE DATABASE $DBPrifix$_CXN  
FROM DISK='$WorkingDirectory$\DBBackup\Dev_CXN.bak'
WITH 
REPLACE,FILE = 1,
MOVE @LogicalNameMdf TO @PhysicalNameMdf,
MOVE @LogicalNameLdf TO @PhysicalNameLdf

----------------------------------------------------------------------------------------------------------------
--PTNR Restore
----------------------------------------------------------------------------------------------------------------
USE $DBPrifix$_PTNR
TRUNCATE TABLE #tmpFilelist
INSERT INTO #tmpFilelist
exec('RESTORE FILELISTONLY FROM DISK = ''$WorkingDirectory$\DBBackup\Dev_PTNR.bak'' WITH  FILE = 1 ')
SET @PhysicalNameMdf=(SELECT physical_name from sys.database_files where type=0) -- 0 Stands for MDF
SET @PhysicalNameLdf=(SELECT physical_name from sys.database_files where type=1) -- 1 Stands for LDF
SET @LogicalNameMdf= (SELECT LogicalName from #tmpFilelist where Type='D')--D = SQL Server data file
SET @LogicalNameLdf= (SELECT LogicalName from #tmpFilelist where Type='L')--L = Microsoft SQL Server log file

USE MASTER
RESTORE DATABASE $DBPrifix$_PTNR  
FROM DISK='$WorkingDirectory$\DBBackup\Dev_PTNR.bak'
WITH 
REPLACE,FILE = 1,
MOVE @LogicalNameMdf TO @PhysicalNameMdf,
MOVE @LogicalNameLdf TO @PhysicalNameLdf

--Clean
BEGIN TRY
    DROP TABLE #tmpFilelist
END TRY
BEGIN CATCH
END CATCH
----------------------------------------------------------------------------------------------------------------
End