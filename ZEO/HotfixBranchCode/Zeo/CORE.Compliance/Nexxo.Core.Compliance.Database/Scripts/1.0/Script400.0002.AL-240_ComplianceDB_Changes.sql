--===========================================================================================
-- Author:		<Adwait Ullal>
-- Create date: <Apr 20th 2015>
-- Description:	<Script to rename Compliance Database column names and FK relationships>              
-- Jira ID:	<AL-240>
--===========================================================================================

-- tBridgerFailures table
EXEC sp_rename @objname='tBridgerFailures.rowguid', @newname='BridgerFailurePK', @objtype='COLUMN'
GO
EXEC sp_rename @objname='tBridgerFailures.Id', @newname='BridgerFailureID', @objtype='COLUMN'
GO

-- tLimits table
EXEC sp_rename @objname='tLimits.rowguid', @newname='LimitPK', @objtype='COLUMN'
GO
EXEC sp_rename @objname='tLimits.Id', @newname='LimitID', @objtype='COLUMN'
GO

-- tLimitTypes table
EXEC sp_rename @objname='tLimitTypes.rowguid', @newname='LimitTypePK', @objtype='COLUMN'
GO
EXEC sp_rename @objname='tLimitTypes.Id', @newname='LimitTypeID', @objtype='COLUMN'
GO

-- tLimitConditions table
EXEC sp_rename @objname='tLimitConditions.rowguid', @newname='LimitConditionPK', @objtype='COLUMN'
GO
EXEC sp_rename @objname='tLimitConditions.Id', @newname='LimitConditionID', @objtype='COLUMN'
GO

-- tTransactionMinimums table
EXEC sp_rename @objname='tTransactionMinimums.rowguid', @newname='TransactionMinimumPK', @objtype='COLUMN'
GO
EXEC sp_rename @objname='tTransactionMinimums.Id', @newname='TransactionMinimumID', @objtype='COLUMN'
GO

-- tCompliancePrograms table
EXEC sp_rename @objname='tCompliancePrograms.rowguid', @newname='ComplianceProgramPK', @objtype='COLUMN'
GO
EXEC sp_rename @objname='tCompliancePrograms.Id', @newname='ComplianceProgramID', @objtype='COLUMN'
GO

-- tLimitFailures table
EXEC sp_rename @objname='tLimitFailures.rowguid', @newname='LimitFailurePK', @objtype='COLUMN'
GO
EXEC sp_rename @objname='tLimitFailures.Id', @newname='LimitFailureID', @objtype='COLUMN'
GO

/*
	The tLimitFailures currently has the ComplianceProgramName as column,
	which should've been a foreign key into the tCompliancePrograms table
	However, since the standard is to have Primary Keys as foreign key candidates
	a ComplianceProgramPK column needs to be added, updated to the right value
	Finally, make it 'not nullable' and enable the FK constraint
*/
ALTER TABLE tLimitFailures
	ADD ComplianceProgramPK UNIQUEIDENTIFIER NULL
GO

UPDATE lf
   SET lf.ComplianceProgramPK = cp.ComplianceProgramPK
  FROM tLimitFailures lf WITH (NOLOCK)
  INNER JOIN tCompliancePrograms cp WITH (NOLOCK) ON cp.Name = lf.ComplianceProgramName
GO

-- Make it not nullable and add Foreign Key Constraint
ALTER TABLE tLimitFailures
	ALTER COLUMN ComplianceProgramPK UNIQUEIDENTIFIER NOT NULL
GO

ALTER TABLE tLimitFailures
	ADD CONSTRAINT FK_tLimitFailures_ComplianceProgramPK_tCompliancePrograms_ComplianceProgramPK FOREIGN KEY(ComplianceProgramPK)
		REFERENCES tCompliancePrograms(ComplianceProgramPK)
GO

-- Rename the FK relationships if they exist
IF EXISTS (SELECT 1
			 FROM sys.foreign_keys 
			WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimits_tLimitTypes]') 
			  AND parent_object_id = OBJECT_ID(N'[dbo].[tLimits]'))
BEGIN
    -- do stuff
	EXEC sp_rename @objname='FK_tLimits_tLimitTypes', @newname='FK_tLimits_LimitTypePK_tLimitTypes_LimitTypePK'
END
GO

IF EXISTS (SELECT 1
			 FROM sys.foreign_keys 
			WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimits_LimitPK]') 
			  AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitConditions]'))
BEGIN
	EXEC sp_rename @objname='FK_tLimits_LimitPK', @newname='FK_tLimitConditions_LimitPK_tLimits_LimitPK'
END
GO

IF EXISTS (SELECT 1
			 FROM sys.foreign_keys 
			WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimitTypes_tCompliancePrograms]') 
			  AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitTypes]'))
BEGIN
	EXEC sp_rename @objname='FK_tLimitTypes_tCompliancePrograms', @newname='FK_tLimitTypes_ComplianceProgramPK_tCompliancePrograms_ComplianceProgramPK'
END
GO

IF EXISTS (SELECT 1
			 FROM sys.foreign_keys 
			WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTransactionMinimums_tCompliancePrograms]') 
			  AND parent_object_id = OBJECT_ID(N'[dbo].[tTransactionMinimums]'))
BEGIN
	EXEC sp_rename @objname='FK_tTransactionMinimums_tCompliancePrograms', @newname='FK_tTransactionMinimums_ComplianceProgramPK_tCompliancePrograms_ComplianceProgramPK'
END
GO

IF EXISTS (SELECT 1
			 FROM sys.foreign_keys 
			WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCompliancePrograms_Name]') 
			  AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitFailures]'))
BEGIN
	ALTER TABLE tLimitFailures
		DROP CONSTRAINT FK_tCompliancePrograms_Name
END
GO