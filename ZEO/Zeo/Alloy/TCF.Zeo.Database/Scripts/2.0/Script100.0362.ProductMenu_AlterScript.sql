--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <01-23-2017>
-- Description:	As an engineer, I want to implement ADO.Net for Products
-- Jira ID:		<AL->
-- ================================================================================

-- =============== Changes realted to tProductProcessorsMapping ====================
DECLARE @FKName VARCHAR(500);
SELECT @FKName = f.name
FROM SYS.FOREIGN_KEYS AS f
     INNER JOIN SYS.FOREIGN_KEY_COLUMNS AS fc ON f.OBJECT_ID = fc.CONSTRAINT_OBJECT_ID
     INNER JOIN SYS.OBJECTS AS o ON o.OBJECT_ID = fc.REFERENCED_OBJECT_ID
WHERE OBJECT_NAME(f.PARENT_OBJECT_ID) = 'tProductProcessorsMapping'
      AND COL_NAME(fc.PARENT_OBJECT_ID, fc.parent_column_id) = 'ProductId'

IF(@FKName IS NOT NULL)
BEGIN
    EXEC ('ALTER TABLE  tProductProcessorsMapping DROP CONSTRAINT '+ @FKName);
END;

SELECT @FKName = f.name
FROM SYS.FOREIGN_KEYS AS f
     INNER JOIN SYS.FOREIGN_KEY_COLUMNS AS fc ON f.OBJECT_ID = fc.CONSTRAINT_OBJECT_ID
     INNER JOIN SYS.OBJECTS AS o ON o.OBJECT_ID = fc.REFERENCED_OBJECT_ID
WHERE OBJECT_NAME(f.PARENT_OBJECT_ID) = 'tProductProcessorsMapping'
      AND COL_NAME(fc.PARENT_OBJECT_ID, fc.parent_column_id) = 'ProcessorId'

IF(@FKName IS NOT NULL)
BEGIN
    EXEC ('ALTER TABLE  tProductProcessorsMapping DROP CONSTRAINT '+ @FKName);
END;


IF EXISTS (
	SELECT 1 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'tProductProcessorsMapping' 
	AND COLUMN_NAME = 'ProcessorId'
)
BEGIN	
	EXEC sp_rename 'tProductProcessorsMapping.ProcessorId', 'ProcessorPK', 'COLUMN';
END
GO


IF EXISTS (
	SELECT 1 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'tProductProcessorsMapping' 
	AND COLUMN_NAME = 'ProductId'
)
BEGIN	
	EXEC sp_rename 'tProductProcessorsMapping.ProductId', 'ProductPK', 'COLUMN';
END
GO

IF NOT EXISTS (
	SELECT 1 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'tProductProcessorsMapping' 
	AND COLUMN_NAME = 'ProductId'
)
BEGIN
	ALTER TABLE tProductProcessorsMapping 
	ADD ProductId BIGINT NULL
END
GO

IF NOT EXISTS (
	SELECT 1 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'tProductProcessorsMapping' 
	AND COLUMN_NAME = 'ProcessorId'
)
BEGIN
	ALTER TABLE tProductProcessorsMapping 
	ADD ProcessorId BIGINT NULL
END
GO

DECLARE @PKName VARCHAR(500);

SELECT 
	@PKName = f.CONSTRAINT_NAME
FROM 
	INFORMATION_SCHEMA.TABLE_CONSTRAINTS  AS f
WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'tProductProcessorsMapping'

IF(@PKName IS NOT NULL)
BEGIN
    EXEC ('ALTER TABLE tProductProcessorsMapping DROP CONSTRAINT '+ @PKName);
END;

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tProductProcessorsMapping'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tProductProcessorsMapping 
	ADD CONSTRAINT PK_tProductProcessorsMapping PRIMARY KEY CLUSTERED (ProductProcessorsMappingID)
END
GO
-- =============== Changes realted to tProducts ====================
DECLARE @PKName VARCHAR(500);

SELECT 
	@PKName = f.CONSTRAINT_NAME
FROM 
	INFORMATION_SCHEMA.TABLE_CONSTRAINTS  AS f
WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'tProducts'

IF(@PKName IS NOT NULL)
BEGIN
    EXEC ('ALTER TABLE tProducts DROP CONSTRAINT '+ @PKName);
END;

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tProducts'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tProducts 
	ADD CONSTRAINT PK_tProducts PRIMARY KEY CLUSTERED (ProductsID)
END
GO


-- ================= Changes related to tProcessors ====================
DECLARE @PKName VARCHAR(500);

SELECT 
	@PKName = f.CONSTRAINT_NAME
FROM 
	INFORMATION_SCHEMA.TABLE_CONSTRAINTS  AS f
WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'tProcessors'


IF(@PKName IS NOT NULL)
BEGIN
    EXEC ('ALTER TABLE tProcessors DROP CONSTRAINT '+ @PKName);
END;

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tProcessors'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tProcessors 
	ADD CONSTRAINT PK_tProcessors PRIMARY KEY CLUSTERED (ProcessorsID)
END
GO

--Making PK column NULL
IF EXISTS(
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= 'tProducts'
		AND COLUMN_NAME = 'ProductsPK'
)
BEGIN 
	ALTER TABLE tProducts
	ALTER COLUMN ProductsPK UNIQUEIDENTIFIER NULL
END
GO