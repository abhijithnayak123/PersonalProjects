--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <05-04-2016>
-- Description:	 Alter PK constraint for tIdentificationConfirmation table
-- Jira ID:		<>
-- ================================================================================


--============ DROP PK constraints from tIdentificationConfirmation table======================

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tIdentificationConfirmation' AND OBJECT_NAME(OBJECT_ID) = 'PK_tRecordIdentityConfirmed')
BEGIN
	ALTER TABLE [dbo].[tIdentificationConfirmation] DROP CONSTRAINT [PK_tRecordIdentityConfirmed]
END
GO

--============ ADD PK constraints to tIdentificationConfirmation table======================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tIdentificationConfirmation' AND OBJECT_NAME(OBJECT_ID) = 'PK_tRecordIdentityConfirmed')
BEGIN
	ALTER TABLE [dbo].[tIdentificationConfirmation] ADD CONSTRAINT [PK_tRecordIdentityConfirmed] PRIMARY KEY CLUSTERED (IdConfirmID)
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tIdentificationConfirmation' AND COLUMN_NAME = 'IdConfirmPK' )
BEGIN
	ALTER TABLE tIdentificationConfirmation 
	ALTER COLUMN IdConfirmPK UNIQUEIDENTIFIER NULL
END
GO

--=======================================================================================