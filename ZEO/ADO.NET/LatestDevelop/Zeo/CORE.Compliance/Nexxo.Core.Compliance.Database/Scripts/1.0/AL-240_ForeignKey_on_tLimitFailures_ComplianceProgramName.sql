-- First, create a unique index on the Name column of the tComplianccePrograms table
-- since an index is a pre-requisite for an FK to occur
IF NOT EXISTS(
				SELECT 1 
				  FROM
					 sys.indexes ind 
				INNER JOIN 
					 sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
				INNER JOIN 
					 sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
				INNER JOIN 
					 sys.tables t ON ind.object_id = t.object_id 
				  WHERE 
					 ind.is_primary_key = 0 
					 AND ind.is_unique = 1
					 AND ind.is_unique_constraint = 0 
					 AND t.is_ms_shipped = 0 
					 AND t.Name = 'tCompliancePrograms'
					 AND col.Name = 'Name'
			)
BEGIN
		CREATE UNIQUE INDEX UX_tCompliancePrograms_Name ON tCompliancePrograms(Name)
END
-- Next, add a Foreign Key relationship on tLimitFailures table
IF NOT EXISTS(
				SELECT 
					f.name AS ForeignKey,
					SCHEMA_NAME(f.SCHEMA_ID) SchemaName,
					OBJECT_NAME(f.parent_object_id) AS TableName,
					COL_NAME(fc.parent_object_id,fc.parent_column_id) AS ColumnName,
					SCHEMA_NAME(o.SCHEMA_ID) ReferenceSchemaName,
					OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
					COL_NAME(fc.referenced_object_id,fc.referenced_column_id) AS ReferenceColumnName
				  FROM sys.foreign_keys AS f
				INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
				INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
				 WHERE OBJECT_NAME(f.parent_object_id) = 'tLimitFailures'
				   AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'ComplianceProgramName'
				   AND OBJECT_NAME(f.referenced_object_id) = 'tCompliancePrograms'
				   AND COL_NAME(fc.referenced_object_id,fc.referenced_column_id) = 'Name'
			)
BEGIN
	ALTER TABLE tLimitFailures
		ADD CONSTRAINT FK_tCompliancePrograms_Name FOREIGN KEY (ComplianceProgramName)
			REFERENCES tCompliancePrograms(Name);
END