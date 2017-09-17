--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <09-19-2016>
-- Description:	 Alter PK and FK constraints for Compliance/Limit
-- Jira ID:		<AL-8195>
-- ================================================================================

-- Drop the FK constraint (LimitTypePK - RowGuid) create the FK constraint for LimitTypeID

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimits_tLimitTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLimits]'))
BEGIN
	ALTER TABLE [dbo].[tLimits] DROP CONSTRAINT [FK_tLimits_tLimitTypes]
END
GO
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimitFailures_ComplianceProgramPK_tCompliancePrograms_ComplianceProgramPK]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitFailures]'))
BEGIN
	ALTER TABLE [dbo].[tLimitFailures] DROP CONSTRAINT [FK_tLimitFailures_ComplianceProgramPK_tCompliancePrograms_ComplianceProgramPK]
END
GO
GO
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimitTypes_tCompliancePrograms]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitTypes]'))
BEGIN
	ALTER TABLE [dbo].[tLimitTypes] DROP CONSTRAINT [FK_tLimitTypes_tCompliancePrograms]
END
GO

--==============================ADD NEW COLUMN IN tLimitTypes TABLE ================================================

-- Add new columns(ComplianceProgramId) in tLimitTypes table

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitTypes' AND COLUMN_NAME = 'ComplianceProgramID')
BEGIN
	ALTER TABLE tLimitTypes 
	ADD ComplianceProgramID BIGINT NULL 
END
GO

-- Add new columns(LimitTypeID) in tLimits table

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimits' AND COLUMN_NAME = 'LimitTypeID')
BEGIN
	ALTER TABLE tLimits 
	ADD LimitTypeID BIGINT NULL 
END
GO

-- Add new columns(ComplianceProgramID) in tLimits table

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitFailures' AND COLUMN_NAME = 'ComplianceProgramID')
BEGIN
	ALTER TABLE tLimitFailures 
	ADD ComplianceProgramID BIGINT NULL 
END
GO
		

--========================================DROP AND ADD THE PK CONSTRAINTS IN tCompliancePrograms TABLE===========================================


-- Drop the PK constraint(ComplianceProgramPK - rowguid) and add PK constraint into ComplianceProgramID Column

IF EXISTS (SELECT 1 FROM sys.objects WHERE TYPE = 'PK' AND OBJECT_NAME(parent_object_id) = 'tCompliancePrograms' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCompliancePrograms')
BEGIN
	ALTER TABLE [dbo].[tCompliancePrograms] DROP CONSTRAINT [PK_tCompliancePrograms]
	ALTER TABLE [dbo].[tCompliancePrograms] ADD  CONSTRAINT [PK_tCompliancePrograms] PRIMARY KEY CLUSTERED (ComplianceProgramID)
END
GO

--========================================DROP AND ADD THE CONSTRAINTS IN limits TABLE===========================================


-- Drop the PK constraint(LimitTypePK - rowguid) and add PK constraint into LimitTypeID Column

IF EXISTS (SELECT 1 FROM sys.objects WHERE TYPE = 'PK' AND OBJECT_NAME(parent_object_id) = 'tLimitTypes' AND OBJECT_NAME(OBJECT_ID) = 'PK_tLimitTypes')
BEGIN
	ALTER TABLE [dbo].[tLimitTypes] DROP CONSTRAINT [PK_tLimitTypes]
	ALTER TABLE [dbo].[tLimitTypes] ADD  CONSTRAINT [PK_tLimitTypes] PRIMARY KEY CLUSTERED (LimitTypeID)
END
GO

-- Drop the PK constraint(LimitPK - rowguid) and add PK constraint into LimitID Column


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'PK' AND OBJECT_NAME(parent_object_id) = 'tLimits' AND OBJECT_NAME(OBJECT_ID) = 'PK_tLimits')
BEGIN

	ALTER TABLE [dbo].[tLimits] DROP CONSTRAINT [PK_tLimits]
	ALTER TABLE [dbo].[tLimits] ADD  CONSTRAINT [PK_tLimits] PRIMARY KEY CLUSTERED (LimitID)

END
GO

-- Drop the PK constraint(LimitFailurePK - rowguid) and add PK constraint into LimitFailureID Column

IF EXISTS (SELECT 1 FROM sys.objects WHERE TYPE = 'PK' AND OBJECT_NAME(parent_object_id) = 'tLimitFailures' AND OBJECT_NAME(OBJECT_ID) = 'PK_tLimitFailures')
BEGIN

	ALTER TABLE [dbo].[tLimitFailures] DROP CONSTRAINT [PK_tLimitFailures]
	ALTER TABLE [dbo].[tLimitFailures] ADD  CONSTRAINT [PK_tLimitFailures] PRIMARY KEY CLUSTERED (LimitFailureID)

END
GO

--========================================DROP AND ADD THE CONSTRAINTS IN tLimitTypes TABLE===========================================

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimitTypes_tCompliancePrograms]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitTypes]'))
BEGIN

	ALTER TABLE [dbo].[tLimitTypes]  WITH CHECK ADD  CONSTRAINT [FK_tLimitTypes_tCompliancePrograms] FOREIGN KEY([ComplianceProgramID])
	REFERENCES [dbo].[tCompliancePrograms] ([ComplianceProgramID])
	
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimits_tLimitTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLimits]'))
BEGIN

	ALTER TABLE [dbo].[tLimits]  WITH CHECK ADD  CONSTRAINT [FK_tLimits_tLimitTypes] FOREIGN KEY([LimitTypeID])
    REFERENCES [dbo].[tLimitTypes] ([LimitTypeID])
	
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimitFailures_tCompliancePrograms]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitFailures]'))
BEGIN

	ALTER TABLE [dbo].[tLimitFailures]  WITH CHECK ADD  CONSTRAINT [FK_tLimitFailures_tCompliancePrograms] FOREIGN KEY([ComplianceProgramID])
	REFERENCES [dbo].[tCompliancePrograms] ([ComplianceProgramID])
	
END
GO

--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLimitFailures_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLimitFailures]'))
--BEGIN

--	ALTER TABLE [dbo].[tLimitFailures]  WITH CHECK ADD  CONSTRAINT [FK_tLimitFailures_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
--	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionId])
	
--END
--GO




