IF NOT EXISTS(
				SELECT 
					f.name AS ForeignKey,
					SCHEMA_NAME(f.SCHEMA_ID) SchemaName,
					OBJECT_NAME(f.parent_object_id) AS TableName,
					COL_NAME(fc.parent_object_id,fc.parent_column_id) AS ColumnName,
					SCHEMA_NAME(o.SCHEMA_ID) ReferenceSchemaName,
					OBJECT_NAME(f.referenced_object_id) AS ReferenceTableName,
					COL_NAME(fc.referenced_object_id,fc.referenced_column_id) AS ReferenceColumnName
				  FROM sys.foreign_keys AS f
				INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
				INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
				 WHERE OBJECT_NAME(f.parent_object_id) = 'tLimitConditions'
				   AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'LimitPK'
				   AND OBJECT_NAME(f.referenced_object_id) = 'tLimits'
				   AND COL_NAME(fc.referenced_object_id,fc.referenced_column_id) = 'rowguid'
			)
BEGIN
	ALTER TABLE tLimitConditions
		ADD CONSTRAINT FK_tLimits_LimitPK FOREIGN KEY (LimitPK)
			REFERENCES tLimits(rowguid);
END