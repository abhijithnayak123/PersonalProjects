-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/12/2017>
-- Description:	<As an engineer, I want to implement ADO.Net for Terminal module>
-- Jira ID:		<AL-7583>
-- ================================================================================

-- Dropping the ForeignKey constraint on tAgentSessions
IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tAgentSessions'
			AND CONSTRAINT_NAME = 'FK_tTerminals_AgentSession')
BEGIN
	ALTER TABLE tAgentSessions 
	DROP CONSTRAINT FK_tTerminals_AgentSession
END
GO

-- Dropping the Primarykey constraint on TerminalPK column
DECLARE @SQL NVARCHAR(MAX) = N'';
IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tTerminals' AND Constraint_Type = 'PRIMARY KEY')
BEGIN
	SELECT @SQL = CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	WHERE TABLE_NAME = 'tTerminals'
	AND Constraint_Type = 'PRIMARY KEY'
	
	EXEC ('ALTER TABLE tTerminals DROP CONSTRAINT ' + @SQL)
END
GO

-- Adding the Primarykey constraint on TerminalId column
IF NOT EXISTS ( SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tTerminals'
				AND Constraint_Type = 'PRIMARY KEY')
BEGIN
	ALTER TABLE tTerminals
	ADD CONSTRAINT PK_Terminals PRIMARY KEY CLUSTERED (TerminalID)
END 
GO

--Droppping NONCLUSTERED Constraint IX_tTerminals_NameChannelPartner
IF EXISTS ( SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tTerminals'
		    AND CONSTRAINT_NAME = 'IX_tTerminals_NameChannelPartner')
BEGIN
	ALTER TABLE tTerminals 
	DROP CONSTRAINT IX_tTerminals_NameChannelPartner
END
GO

-- Dropping the ForeignKey constraint on NpsTerminalPK
IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tTerminals'
			AND CONSTRAINT_NAME = 'FK_tTerminals_tNpsTerminals')
BEGIN
	ALTER TABLE tTerminals 
	DROP CONSTRAINT FK_tTerminals_tNpsTerminals
END
GO

-- Adding the NpsTerminalId column
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals'
		AND COLUMN_NAME = 'NpsTerminalId')
BEGIN
	ALTER TABLE tTerminals 
	ADD NpsTerminalId BIGINT NULL
END
GO

--Adding Contraint on NpsTerminalId
IF NOT EXISTS ( SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tNpsTerminal_tTerminals]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTerminals]'))
BEGIN
	ALTER TABLE [dbo].[tTerminals]  
	WITH CHECK ADD CONSTRAINT [FK_tNpsTerminal_tTerminals] 
	FOREIGN KEY([NpsTerminalId])
	REFERENCES [dbo].[tNpsTerminals] ([NpsTerminalID])	
END
GO


-- Adding the LocationId column
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals' AND COLUMN_NAME = 'LocationId')
BEGIN
	ALTER TABLE tTerminals 
	ADD LocationId BIGINT  NULL
END
GO

-- ADD LocationId as FK in tTerminals
IF NOT EXISTS ( SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLocations_tTerminals]') 
			    AND parent_object_id = OBJECT_ID(N'[dbo].[tTerminals]'))
BEGIN
	ALTER TABLE [dbo].[tTerminals]  
	WITH CHECK ADD CONSTRAINT [FK_tLocations_tTerminals] 
	FOREIGN KEY([LocationId])
	REFERENCES [dbo].[tLocations] ([LocationID])	
END
GO


--Adding Contraint on tAgentSessions
IF NOT EXISTS ( SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTerminals_AgentSession]') AND parent_object_id = OBJECT_ID(N'[dbo].[tAgentSessions]'))
BEGIN
	ALTER TABLE [dbo].[tAgentSessions]  
	WITH CHECK ADD CONSTRAINT FK_tTerminals_AgentSession 
	FOREIGN KEY([TerminalId])
	REFERENCES [dbo].[tTerminals] ([TerminalId])	
END
GO


-- Adding the TerminalId column
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentSessions'
		AND COLUMN_NAME IN ('TerminalId', 'TerminalName'))
BEGIN
	ALTER TABLE tAgentSessions 
	ADD TerminalId BIGINT NULL 

	ALTER TABLE tAgentSessions 
	ADD TerminalName VARCHAR(100) NULL
END
GO


-- Dropping the ForeignKey constraint on ChannelpartnerPk
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tTerminals'
		   AND CONSTRAINT_NAME = 'FK_tTerminal_tChannelPartner'
)
BEGIN
	ALTER TABLE tTerminals 
	DROP CONSTRAINT FK_tTerminal_tChannelPartner
END
GO

-- Adding the ChannelPartnerId column
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals'
		AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tTerminals 
	ADD ChannelPartnerId SMALLINT NULL
END
GO

-- ADD ChannelPartnerId as FK in tTerminals
IF NOT EXISTS ( SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTerminal_tChannelPartner]') 
			    AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartners]'))
BEGIN
	ALTER TABLE [dbo].[tTerminals]  
	WITH CHECK ADD CONSTRAINT FK_tTerminal_tChannelPartner 
	FOREIGN KEY(ChannelPartnerId)
	REFERENCES [dbo].[tChannelPartners] (ChannelPartnerId)	
END
GO


IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals' 
				AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
	ALTER TABLE tTerminals 
	ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NULL 
END
GO


IF EXISTS (	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals' 
				AND COLUMN_NAME = 'TerminalPK')
BEGIN
	ALTER TABLE tTerminals 
	ALTER COLUMN TerminalPK UNIQUEIDENTIFIER NULL 
END
GO
