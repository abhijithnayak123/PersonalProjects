--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <12-13-2016>
-- Description:	 Alter PK and FK constraints for Agent related tables
-- Jira ID:		<AL-7581>
-- ================================================================================


--==============================================================================================================================
--Drop foreign key constraints from all Agent related tables
--==============================================================================================================================

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentSessions_tAgentDetails_AgentPK]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tAgentSessions]'))
BEGIN
    ALTER TABLE [dbo].[tAgentSessions] DROP CONSTRAINT FK_tAgentSessions_tAgentDetails_AgentPK;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentAuthentication_tAgentDetails_AgentPK]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tAgentAuthentication]'))
BEGIN
    ALTER TABLE [dbo].[tAgentAuthentication] DROP CONSTRAINT FK_tAgentAuthentication_tAgentDetails_AgentPK;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomerSessions_tAgentSessions]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tCustomerSessions]'))
BEGIN
    ALTER TABLE [dbo].[tCustomerSessions] DROP CONSTRAINT FK_tCustomerSessions_tAgentSessions;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentLocationMapping_tAgentDetails]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tAgentLocationMapping]'))
BEGIN
    ALTER TABLE [dbo].[tAgentLocationMapping] DROP CONSTRAINT FK_tAgentLocationMapping_tAgentDetails;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentSessions_tAgentDetails]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tAgentSessions]'))
BEGIN
    ALTER TABLE [dbo].[tAgentSessions] DROP CONSTRAINT FK_tAgentSessions_tAgentDetails;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentSessions_tTerminals]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tAgentSessions]'))
BEGIN
    ALTER TABLE [dbo].[tAgentSessions] DROP CONSTRAINT FK_tAgentSessions_tTerminals;
END;

--==============================================================================================================================
--Drop Primary key constraints from all Agent related tables
--==============================================================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tAgentDetails' AND OBJECT_NAME(OBJECT_ID) = 'PK_tAgentDetails')
BEGIN
	ALTER TABLE [dbo].[tAgentDetails] DROP CONSTRAINT PK_tAgentDetails
END

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tAgentSessions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tAgentSessions')
BEGIN
	ALTER TABLE [dbo].[tAgentSessions] DROP CONSTRAINT PK_tAgentSessions
END

--IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tTerminals' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTerminals')
--BEGIN
--	ALTER TABLE [dbo].[tTerminals] DROP CONSTRAINT PK_tTerminals
--END

--=========================================================================================================
-- Add new columns in Agent related tables
--=========================================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentSessions' AND COLUMN_NAME = 'TerminalId')
BEGIN
	ALTER TABLE tAgentSessions 
	ADD TerminalId  BIGINT NULL  	
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentLocationMapping' AND COLUMN_NAME = 'AgentLocationId')
BEGIN
	ALTER TABLE tAgentLocationMapping 
	ADD AgentLocationId  BIGINT NOT NULL  identity(100000000,1)	
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'AgentSessionId')
BEGIN
	ALTER TABLE tCustomerSessions 
	ADD AgentSessionId  BIGINT NULL  	
END

--=========================================================================================================
--changing the columns datatype Agent relatetd tables
--=========================================================================================================
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentDetails' AND COLUMN_NAME = 'AgentID')
BEGIN
    ALTER TABLE [dbo].[tAgentDetails] Alter Column  
     [AgentID] BIGINT NOT NULL
End
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentLocationMapping' AND COLUMN_NAME = 'AgentId')
BEGIN
    ALTER TABLE [dbo].[tAgentLocationMapping] Alter Column  
     [AgentId] BIGINT NOT NULL
End
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentSessions' AND COLUMN_NAME = 'AgentId')
BEGIN
    ALTER TABLE [dbo].[tAgentSessions] Alter Column  
     [AgentId] BIGINT NOT NULL
End
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentSessions' AND COLUMN_NAME = 'AgentPK')
BEGIN
    ALTER TABLE [dbo].[tAgentSessions] Alter Column  
     [AgentPK] uniqueidentifier NULL
End
--=========================================================================================================
--Adding PK constraints to the Agent relatetd tables
--=========================================================================================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tAgentDetails' AND OBJECT_NAME(OBJECT_ID) = 'PK_tAgentDetails')
BEGIN
	ALTER TABLE [dbo].[tAgentDetails] ADD CONSTRAINT [PK_tAgentDetails] PRIMARY KEY CLUSTERED (AgentID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tAgentSessions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tAgentSessions')
BEGIN
	ALTER TABLE [dbo].[tAgentSessions] ADD CONSTRAINT [PK_tAgentSessions] PRIMARY KEY CLUSTERED (AgentSessionID)
END

--IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTerminals' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTerminals')
--BEGIN
--	ALTER TABLE [dbo].[tTerminals] ADD CONSTRAINT [PK_tTerminals] PRIMARY KEY CLUSTERED (TerminalID)
--END


IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tAgentLocationMapping' AND OBJECT_NAME(OBJECT_ID) = 'PK_tAgentLocationMapping')
BEGIN
	ALTER TABLE [dbo].[tAgentLocationMapping] ADD CONSTRAINT [PK_tAgentLocationMapping] PRIMARY KEY CLUSTERED (AgentLocationId)
END

--=========================================================================================================
--Adding FK constraints to the Agent related tables
--=========================================================================================================

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentSessions_tAgentDetails]') AND parent_object_id = OBJECT_ID(N'[dbo].[tAgentSessions]'))
BEGIN
    ALTER TABLE [dbo].[tAgentSessions]  WITH CHECK ADD  CONSTRAINT [FK_tAgentSessions_tAgentDetails] FOREIGN KEY(AgentId)
	REFERENCES [dbo].[tAgentDetails] (AgentID)
END

--IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentSessions_tTerminals]') AND parent_object_id = OBJECT_ID(N'[dbo].[tAgentSessions]'))
--BEGIN
--    ALTER TABLE [dbo].[tAgentSessions]  WITH CHECK ADD  CONSTRAINT [FK_tAgentSessions_tTerminals] FOREIGN KEY(TerminalId)
--	REFERENCES [dbo].[tTerminals] (TerminalID)
--END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentLocationMapping_tAgentDetails]') AND parent_object_id = OBJECT_ID(N'[dbo].[tAgentLocationMapping]'))
BEGIN
    ALTER TABLE [dbo].[tAgentLocationMapping]  WITH CHECK ADD  CONSTRAINT [FK_tAgentLocationMapping_tAgentDetails] FOREIGN KEY(AgentId)
	REFERENCES [dbo].[tAgentDetails] (AgentID)
END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomerSessions_tAgentSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomerSessions]'))
BEGIN
    ALTER TABLE [dbo].[tCustomerSessions]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerSessions_tAgentSessions] FOREIGN KEY(AgentSessionId)
	REFERENCES [dbo].[tAgentSessions] (AgentSessionID)
END

--================================================================================================================
--Adding default values to the PK columns 
--================================================================================================================

--IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tAgentDetails_AgentPK' )
--BEGIN
--	ALTER TABLE tAgentDetails ADD CONSTRAINT DF_tAgentDetails_AgentPK DEFAULT NEWID() FOR AgentPK
--END
--GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tAgentSessions_AgentSessionPK' )
BEGIN
	ALTER TABLE tAgentSessions ADD CONSTRAINT DF_tAgentSessions_AgentSessionPK DEFAULT NEWID() FOR AgentSessionPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tAgentLocationMapping_AgentLocationPK' )
BEGIN
	ALTER TABLE tAgentLocationMapping ADD CONSTRAINT DF_tAgentLocationMapping_AgentLocationPK DEFAULT NEWID() FOR AgentLocationPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTerminals_TerminalPK' )
BEGIN
	ALTER TABLE tTerminals ADD CONSTRAINT DF_tTerminals_TerminalPK DEFAULT NEWID() FOR TerminalPK
END
GO