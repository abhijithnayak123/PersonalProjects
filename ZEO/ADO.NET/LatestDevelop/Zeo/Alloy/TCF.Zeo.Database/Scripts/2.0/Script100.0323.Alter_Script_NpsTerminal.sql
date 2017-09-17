-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <07/21/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Terminal module>
-- Jira ID:		<AL-7583>
-- ================================================================================

-- Dropping the ForeignKey constraint on NpsTerminalId
IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tTerminals'
			AND CONSTRAINT_NAME = 'FK_tTerminals_tNpsTerminals')
BEGIN
	ALTER TABLE tTerminals 
	DROP CONSTRAINT FK_tTerminals_tNpsTerminals
END
GO


-- Dropping the Primarykey constraint on NpsTerminalPK column
DECLARE @SQL NVARCHAR(MAX) = N'';
IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tNpsTerminals'
			AND Constraint_Type = 'PRIMARY KEY')
BEGIN
	SELECT @SQL = CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	WHERE TABLE_NAME = 'tNpsTerminals'
	AND Constraint_Type = 'PRIMARY KEY'
	
	EXEC ('ALTER TABLE tNpsTerminals DROP CONSTRAINT ' + @SQL)
END
GO

-- Adding the Primarykey constraint on NpsTerminalId column
IF NOT EXISTS ( SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tNpsTerminals'
				AND Constraint_Type = 'PRIMARY KEY')
BEGIN
	ALTER TABLE tNpsTerminals
	ADD CONSTRAINT PK_tNpsTerminals PRIMARY KEY CLUSTERED (NpsTerminalId)
END 
GO


-- Dropping the ForeignKey constraint on ChannelpartnerPk
IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tNpsTerminals'
			AND CONSTRAINT_NAME = 'FK_tChannelPartners_tNpsTerminals')
BEGIN
	ALTER TABLE tNpsTerminals 
	DROP CONSTRAINT FK_tChannelPartners_tNpsTerminals
END
GO

-- Adding the ChannelPartnerId column
IF NOT EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNpsTerminals' 
				AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tNpsTerminals 
	ADD ChannelPartnerId SMALLINT NULL 
END
GO

-- ADD ChannelPartnerId as FK in tNpsTerminals
IF NOT EXISTS ( SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartners_tNpsTerminals]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tNpsTerminals]'))
BEGIN
	ALTER TABLE [dbo].tNpsTerminals  
	WITH CHECK ADD CONSTRAINT [FK_tChannelPartners_tNpsTerminals] 
	FOREIGN KEY(ChannelPartnerId)
	REFERENCES [dbo].[tChannelPartners] (ChannelPartnerId)	
END
GO

-- Adding the LocationId column
IF NOT EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNpsTerminals' 
				AND COLUMN_NAME = 'LocationId')
BEGIN
	ALTER TABLE tNpsTerminals 
	ADD LocationId BIGINT NULL 
END
GO

-- ADD LocationId as FK in tTerminals
IF NOT EXISTS (	SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLocations_tNpsTerminals]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tNpsTerminals]'))
BEGIN
	ALTER TABLE [dbo].tNpsTerminals  
	WITH CHECK ADD CONSTRAINT [FK_tLocations_tNpsTerminals] 
	FOREIGN KEY([LocationId])
	REFERENCES [dbo].[tLocations] ([LocationID])	
END
GO


IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNpsTerminals' 
				AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
	ALTER TABLE tNpsTerminals 
	ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NULL 
END
GO

IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNpsTerminals' 
				AND COLUMN_NAME = 'NpsTerminalPK')
BEGIN
	ALTER TABLE tNpsTerminals 
	ALTER COLUMN NpsTerminalPK UNIQUEIDENTIFIER NULL 
END
GO
