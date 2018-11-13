--===========================================================================================
-- Author:		<Adwait Ullal>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names and foreign key relationships>           
-- Jira ID:	<AL-241>
--===========================================================================================

-- Drop tTargetCatalog, since the table is not used any more
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tTargetCatalog')
BEGIN
	DROP TABLE tTargetCatalog
END
GO


-- Changes to tMasterCatalog
IF EXISTS
	(
		SELECT 1
		  FROM INFORMATION_SCHEMA.COLUMNS
		 WHERE TABLE_NAME = 'tMasterCatalog'
		   AND COLUMN_NAME = 'rowguid'
	)
BEGIN
	exec sp_rename @objname = 'tMasterCatalog.rowguid', @newname = 'MasterCatalogPK', @objtype = 'COLUMN'
END
GO

IF EXISTS
	(
		SELECT 1
		  FROM INFORMATION_SCHEMA.COLUMNS
		 WHERE TABLE_NAME = 'tMasterCatalog'
		   AND COLUMN_NAME = 'id'
	)
BEGIN
	exec sp_rename @objname = 'tMasterCatalog.id', @newname = 'MasterCatalogID', @objtype = 'COLUMN'
END
GO

-- Changes to tPartnerCatalog
-- rename primary key
IF EXISTS
	(
		SELECT 1
		  FROM INFORMATION_SCHEMA.COLUMNS
		 WHERE TABLE_NAME = 'tPartnerCatalog'
		   AND COLUMN_NAME = 'rowguid'
	)
BEGIN
	exec sp_rename @objname = 'tPartnerCatalog.rowguid', @newname = 'tPartnerCatalogPK', @objtype = 'COLUMN'
END
GO
-- Add MasterCatalogPK column as NULL first
IF NOT EXISTS
	(
		SELECT 1
		  FROM INFORMATION_SCHEMA.COLUMNS
		 WHERE TABLE_NAME = 'tPartnerCatalog'
		   AND COLUMN_NAME = 'MasterCatalogPK'
	)
BEGIN
	ALTER TABLE tPartnerCatalog
		ADD MasterCatalogPK UNIQUEIDENTIFIER NULL
END
GO

UPDATE p
   SET p.MasterCatalogPK = m.MasterCatalogPK
  FROM tPartnerCatalog p WITH (NOLOCK)
  JOIN tMasterCatalog m WITH (NOLOCK) ON m.MasterCatalogID = p.Id
GO

IF EXISTS
	(
		SELECT 1
		  FROM INFORMATION_SCHEMA.COLUMNS
		 WHERE TABLE_NAME = 'tPartnerCatalog'
		   AND COLUMN_NAME = 'MasterCatalogPK'
	)
BEGIN
	ALTER TABLE tPartnerCatalog
		ALTER COLUMN MasterCatalogPK UNIQUEIDENTIFIER NOT NULL
END
GO

-- Drop the primary key from ID and add 'xCatalogPK' as PK for both tables
-- tMasterCatalog first
IF EXISTS
	(SELECT 1 
	   FROM sys.key_constraints 
	  WHERE name = 'PK_MasterCatalog' 
	    AND OBJECT_NAME(parent_object_id) = 'tMasterCatalog'
	)
BEGIN
	ALTER TABLE tMasterCatalog
		DROP CONSTRAINT PK_MasterCatalog
END
GO

IF NOT EXISTS
	(SELECT 1 
	   FROM sys.key_constraints 
	  WHERE name = 'PK_MasterCatalog' 
	    AND OBJECT_NAME(parent_object_id) = 'tMasterCatalog'
	)
BEGIN
	ALTER TABLE tMasterCatalog
		ADD PRIMARY KEY (MasterCatalogPK)
END
GO

-- Unique index on MasterCatalogID
IF NOT EXISTS
	(SELECT 1 
	   FROM sys.key_constraints 
	  WHERE name = 'UX_MasterCatalogID' 
	    AND OBJECT_NAME(parent_object_id) = 'tMasterCatalog'
	)
BEGIN
	ALTER TABLE tMasterCatalog
		ADD CONSTRAINT UX_MasterCatalogID UNIQUE NONCLUSTERED (MasterCatalogID)
END
GO

-- tPartnerCatalog
IF EXISTS
	(SELECT 1 
	   FROM sys.key_constraints 
	  WHERE name = 'PK_PartnerCatalog' 
	    AND OBJECT_NAME(parent_object_id) = 'tPartnerCatalog'
	)
BEGIN
	ALTER TABLE tPartnerCatalog
		DROP CONSTRAINT PK_PartnerCatalog
END
GO

IF NOT EXISTS
	(SELECT 1 
	   FROM sys.key_constraints 
	  WHERE name = 'PK_PartnerCatalog' 
	    AND OBJECT_NAME(parent_object_id) = 'tPartnerCatalog'
	)
BEGIN
	ALTER TABLE tPartnerCatalog
		ADD PRIMARY KEY (tPartnerCatalogPK)
END
GO

 --Lastly, add the FK constraint to tPartnerCatalog
IF NOT EXISTS
	(
		SELECT 1
			-- f.name AS ForeignKey,
			-- SCHEMA_NAME(f.SCHEMA_ID) SchemaName,
			-- OBJECT_NAME(f.parent_object_id) AS TableName,
			-- COL_NAME(fc.parent_object_id,fc.parent_column_id) AS ColumnName,
			-- SCHEMA_NAME(o.SCHEMA_ID) ReferenceSchemaName,
			-- OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
			-- COL_NAME(fc.referenced_object_id,fc.referenced_column_id) AS ReferenceColumnName
		  FROM sys.foreign_keys AS f
		INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
		INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
		 WHERE OBJECT_NAME(f.parent_object_id) = 'tPartnerCatalog'
		   AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'MasterCatalogPK'
		   AND OBJECT_NAME(f.referenced_object_id) = 'tMasterCatalog'
		   AND COL_NAME(fc.referenced_object_id,fc.referenced_column_id) = 'MasterCatalogPK'
	)
BEGIN
	ALTER TABLE tPartnerCatalog
		ADD CONSTRAINT FK_tPartnerCatalog_MasterCatalogPK_tMasterCatalog_MasterCatalogPK
			FOREIGN KEY (MasterCatalogPK)
			REFERENCES tMasterCatalog(MasterCatalogPK)
END
GO

IF EXISTS
	(
		SELECT 1
		  FROM INFORMATION_SCHEMA.COLUMNS
		 WHERE COLUMN_NAME = 'Id'
		   AND TABLE_NAME = 'tPartnerCatalog'
	)
BEGIN
	ALTER TABLE tPartnerCatalog
		DROP COLUMN Id
END
GO

