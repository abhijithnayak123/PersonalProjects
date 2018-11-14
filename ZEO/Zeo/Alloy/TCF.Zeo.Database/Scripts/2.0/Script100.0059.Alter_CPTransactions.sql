--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-20-2016>
-- Description:	 Alter PK and FK constraints for Check related tables
-- Jira ID:		<AL-7582>
-- ================================================================================

--============ DROP FK constraints in Check Processing transaction related tables ==

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Trx_tChxr_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]'))
BEGIN
	ALTER TABLE [dbo].[tChxr_Trx] DROP CONSTRAINT [FK_tChxr_Trx_tChxr_Account]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Session_tChxr_Partner]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Session]'))
BEGIN
	ALTER TABLE [dbo].[tChxr_Session] DROP CONSTRAINT [FK_tChxr_Session_tChxr_Partner]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxrSim_Invoice_tChxrSim_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxrSim_Invoice]'))
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Invoice] DROP CONSTRAINT [FK_tChxrSim_Invoice_tChxrSim_Account]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCheckImages_tTxn_Check_Stage]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCheckImages]'))
BEGIN
	ALTER TABLE [dbo].[tCheckImages] DROP CONSTRAINT [FK_tCheckImages_tTxn_Check_Stage]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCheckImages_tTxn_Check]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCheckImages]'))
BEGIN
	ALTER TABLE [dbo].[tCheckImages] DROP CONSTRAINT [FK_tCheckImages_tTxn_Check]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Check_tAccounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Check]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_Check] DROP CONSTRAINT [FK_tTxn_Check_tAccounts]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Check_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Check]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_Check] DROP CONSTRAINT [FK_tTxn_Check_tCustomerSessions]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Check_Commit_tCustomerAccounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Check_Commit]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_Check_Commit] DROP CONSTRAINT [FK_tTxn_Check_Commit_tCustomerAccounts]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Check_Stage_tCustomerAccounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Check_Stage]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_Check_Stage] DROP CONSTRAINT [FK_tTxn_Check_Stage_tCustomerAccounts]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_FeeAdjustments]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_FeeAdjustments] DROP CONSTRAINT [FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments]
END
GO

IF EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tMessageCenter_AgentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[tMessageCenter]'))
BEGIN
	ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [FK_tMessageCenter_AgentId]
END
GO

IF EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tMessageCenter_TxnId]') AND parent_object_id = OBJECT_ID(N'[dbo].[tMessageCenter]'))
BEGIN
	ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [FK_tMessageCenter_TxnId]
END
GO


IF EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Trx_tTxn_Check]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]'))
BEGIN
	ALTER TABLE [dbo].[tChxr_Trx] DROP CONSTRAINT [FK_tChxr_Trx_tTxn_Check]
END
GO

--============ DROP PK constraints in Check Processing transaction related tables======================

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tCheckImages' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCheckImages')
BEGIN
	ALTER TABLE [dbo].[tCheckImages] DROP CONSTRAINT [PK_tCheckImages]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Account')
BEGIN
	ALTER TABLE [dbo].[tChxr_Account] DROP CONSTRAINT [PK_tChxr_Account]
END
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Partner' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Partner')
BEGIN
	ALTER TABLE [dbo].[tChxr_Partner] DROP CONSTRAINT [PK_tChxr_Partner]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Partner' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Partner')
BEGIN
	ALTER TABLE [dbo].[tChxr_Partner] DROP CONSTRAINT [PK_tChxr_Partner]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Session' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Session')
BEGIN
	ALTER TABLE [dbo].[tChxr_Session] DROP CONSTRAINT [PK_tChxr_Session]
END
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Trx' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Trx')
BEGIN
	ALTER TABLE [dbo].[tChxr_Trx] DROP CONSTRAINT [PK_tChxr_Trx]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxrSim_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxrSim_Account')
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Account] DROP CONSTRAINT [PK_tChxrSim_Account]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxrSim_Invoice' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxrSim_Invoice')
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Invoice] DROP CONSTRAINT [PK_tChxrSim_Invoice]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tMessageCenter' AND OBJECT_NAME(OBJECT_ID) = 'PK_tMessageCenter')
BEGIN
	ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [PK_tMessageCenter]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_Check' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_Check')
BEGIN
	ALTER TABLE [dbo].[tTxn_Check] DROP CONSTRAINT [PK_tTxn_Check]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_Check_Commit' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_Check_Commit')
BEGIN
	ALTER TABLE [dbo].[tTxn_Check_Commit] DROP CONSTRAINT [PK_tTxn_Check_Commit]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_Check_Stage' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_Check_Stage')
BEGIN
	ALTER TABLE [dbo].[tTxn_Check_Stage] DROP CONSTRAINT [PK_tTxn_Check_Stage]
END
GO


------=================================================ReName the PK colum to ID column===============================================
----- ReName CheckTypePK to CheckTypeId
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCheckTypes' AND COLUMN_NAME = 'CheckTypePK')
BEGIN
	 EXEC sp_RENAME '[tCheckTypes].[CheckTypePK]' , 'CheckTypeId' , 'COLUMN'	 
END
GO


----IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Partner' AND COLUMN_NAME != 'ChannelPartnerId' AND COLUMN_NAME = 'ChxrPartnerID' )
----BEGIN
----	 EXEC sp_RENAME '[tChxr_Partner].[ChxrPartnerID]' , 'ChannelPartnerId' , 'COLUMN'	 
----END
----GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'CXEState')
BEGIN
	 EXEC sp_RENAME '[tTxn_Check].[CXEState]' , 'State' , 'COLUMN'	 
END
GO

----- ReName ChexarTypePK to ChexarTypeId
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_CheckTypeMapping' AND COLUMN_NAME = 'ChexarTypePK' )
BEGIN
	 EXEC sp_RENAME '[tChxr_CheckTypeMapping].[ChexarTypePK]' , 'ChexarTypeId' , 'COLUMN'	 
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageCenter' AND COLUMN_NAME = 'AgentId' AND DATA_TYPE = 'uniqueidentifier')
BEGIN
	 EXEC sp_RENAME '[tMessageCenter].[AgentId]' , 'AgentPK' , 'COLUMN'	 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'ChxrSimAccountID' and DATA_TYPE = 'nvarchar') 
BEGIN
	 EXEC sp_RENAME '[tChxrSim_Account].[ChxrSimAccountID]' , 'GovernmentId' , 'COLUMN'	 
END
GO



--==============================ADD new column for CP transaction related table ================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCheckImages' AND COLUMN_NAME IN ('CheckImageId', 'TransactionId'))
BEGIN
    ALTER TABLE tCheckImages 
	ADD CheckImageId BIGINT NOT NULL IDENTITY(10000000,1) 

	ALTER TABLE tCheckImages 
	ADD TransactionId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Partner' AND COLUMN_NAME = 'ChxrPartnerId')
BEGIN
	ALTER TABLE tChxr_Partner 
	ADD ChxrPartnerId BIGINT NOT NULL IDENTITY(10000000,1) 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Session' AND COLUMN_NAME IN ('ChxrSessionId', 'ChxrPartnerId'))
BEGIN
	ALTER TABLE tChxr_Session 
	ADD ChxrSessionId BIGINT NOT NULL IDENTITY(10000000,1) 
	
	ALTER TABLE tChxr_Session 
	ADD ChxrPartnerId BIGINT NULL 
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME IN ('CustomerId', 'CustomerSessionId', 'CustomerRevisionNo'))
BEGIN
	ALTER TABLE tChxr_Account 
	ADD CustomerId BIGINT NULL

	ALTER TABLE tChxr_Account 
	ADD CustomerRevisionNo BIGINT NULL 
		
	ALTER TABLE tChxr_Account 
	ADD CustomerSessionId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME IN ('CustomerId', 'CustomerSessionId', 'CustomerRevisionNo'))
BEGIN
	ALTER TABLE tChxr_Account_Aud 
	ADD CustomerId BIGINT NULL

	ALTER TABLE tChxr_Account_Aud 
	ADD CustomerRevisionNo BIGINT NULL 
		
	ALTER TABLE tChxr_Account_Aud 
	ADD CustomerSessionId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx' AND COLUMN_NAME IN ('ChxrAccountId'))
BEGIN
	ALTER TABLE tChxr_Trx 
	ADD ChxrAccountId BIGINT NULL    
	
	--ALTER TABLE tChxr_Trx 
	--ADD TransactionId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx_Aud' AND COLUMN_NAME IN ('ChxrAccountId'))
BEGIN
	ALTER TABLE tChxr_Trx_Aud 
	ADD ChxrAccountId BIGINT NULL    
	
	--ALTER TABLE tChxr_Trx_Aud 
	--ADD TransactionId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME IN ('ChxrSimAccountId', 'CustomerSessionId', 'CustomerId'))
BEGIN
	ALTER TABLE tChxrSim_Account 
	ADD ChxrSimAccountId  BIGINT NOT NULL DEFAULT(0)    
	
	ALTER TABLE tChxrSim_Account 
	ADD CustomerId BIGINT NULL	    
	
	ALTER TABLE tChxrSim_Account 
	ADD CustomerSessionId BIGINT NULL

END
GO

IF EXISTS(SELECT 1 FROM tChxrSim_Account where ChxrSimAccountId = 0)
BEGIN
	
	DECLARE @id BIGINT 
    SET @id = 10000000 
    UPDATE tChxrSim_Account 
    SET @id = ChxrSimAccountId = @id + 1

END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Invoice' AND COLUMN_NAME = 'ChxrSimAccountId' )
BEGIN
	ALTER TABLE tChxrSim_Invoice 
	ADD ChxrSimAccountId BIGINT NULL	
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageCenter' AND COLUMN_NAME IN ('AgentId','TransactionId'))
BEGIN
	ALTER TABLE tMessageCenter 
	ADD AgentId BIGINT NULL	

	ALTER TABLE tMessageCenter 
	ADD TransactionId BIGINT NULL	
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME IN ('CustomerSessionId', 'CustomerRevisionNo', 'ProviderId', 'ProviderAccountId', 'CheckType', 'MICR'))
BEGIN
	ALTER TABLE tTxn_Check 
	ADD CustomerSessionId BIGINT NULL	

	ALTER TABLE tTxn_Check 
	ADD CustomerRevisionNo BIGINT NULL	

	ALTER TABLE tTxn_Check 
	ADD ProviderId INT NULL	

	ALTER TABLE tTxn_Check 
	ADD ProviderAccountId BIGINT NULL	

	ALTER TABLE tTxn_Check 
	ADD CheckType INT NULL	

	ALTER TABLE tTxn_Check 
	ADD MICR NVARCHAR(100) NULL	
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_Check_Aud]') AND type in (N'U'))
BEGIN
CREATE TABLE tTxn_Check_Aud
  (	
	TransactionID BIGINT NOT NULL,	
	Amount MONEY NULL,
	Fee MONEY NULL,
	Description NVARCHAR(255) NULL,
	State INT NULL,
	DTTerminalCreate DATETIME NOT NULL,
	DTTerminalLastModified DATETIME NULL,
	ConfirmationNumber VARCHAR(50) NULL,
	DTServerCreate DATETIME NULL,
	DTServerLastModified DATETIME NULL,
	BaseFee MONEY NULL,
	DiscountApplied MONEY NULL,
	AdditionalFee MONEY NULL,
	DiscountName VARCHAR(50) NULL,
	DiscountDescription VARCHAR(100) NULL,
	IsSystemApplied BIT NOT NULL,
	CustomerSessionId BIGINT NULL,
	CustomerRevisionNo BIGINT NULL,
	ProviderId INT NULL,
	ProviderAccountId BIGINT NULL,
	CheckType INT NULL,
	MICR NVARCHAR(100) NULL,
	DTAudit DATETIME NOT NULL,
	AuditEvent SMALLINT NOT NULL,
	RevisionNo BIGINT NOT NULL
 )

END
--============ ADD PK constraints in Check Processing transaction related tables ======================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tCheckImages' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCheckImages')
BEGIN
	ALTER TABLE [dbo].[tCheckImages] ADD CONSTRAINT [PK_tCheckImages] PRIMARY KEY CLUSTERED (CheckImageId)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Account')
BEGIN
	ALTER TABLE [dbo].[tChxr_Account] ADD CONSTRAINT [PK_tChxr_Account] PRIMARY KEY CLUSTERED (ChxrAccountID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Partner' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Partner')
BEGIN
	ALTER TABLE [dbo].[tChxr_Partner] ADD CONSTRAINT [PK_tChxr_Partner] PRIMARY KEY CLUSTERED (ChxrPartnerId)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Partner' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Partner')
BEGIN
	ALTER TABLE [dbo].[tChxr_Partner] ADD CONSTRAINT [PK_tChxr_Partner] PRIMARY KEY CLUSTERED (ChxrPartnerId)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Session' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Session')
BEGIN
   ALTER TABLE [dbo].[tChxr_Session] ADD CONSTRAINT [PK_tChxr_Session] PRIMARY KEY CLUSTERED (ChxrSessionId)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxr_Trx' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxr_Trx')
BEGIN
	ALTER TABLE [dbo].[tChxr_Trx] ADD CONSTRAINT [PK_tChxr_Trx] PRIMARY KEY CLUSTERED (ChxrTrxId)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxrSim_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxrSim_Account')
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Account] ADD CONSTRAINT [PK_tChxrSim_Account]  PRIMARY KEY CLUSTERED (ChxrSimAccountId)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChxrSim_Invoice' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChxrSim_Invoice')
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Invoice] ADD CONSTRAINT [PK_tChxrSim_Invoice] PRIMARY KEY CLUSTERED (ChxrSimInvoiceID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tMessageCenter' AND OBJECT_NAME(OBJECT_ID) = 'PK_tMessageCenter')
BEGIN
	ALTER TABLE [dbo].[tMessageCenter] ADD CONSTRAINT [PK_tMessageCenter] PRIMARY KEY CLUSTERED (MessageCenterID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_Check' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_Check')
BEGIN
	ALTER TABLE [dbo].[tTxn_Check] ADD CONSTRAINT [PK_tTxn_Check] PRIMARY KEY CLUSTERED (TransactionID)
END
GO

--============ ADD FK constraints in Check Processing transaction related tables ======================


IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Trx_tChxr_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]'))
BEGIN
    ALTER TABLE [dbo].[tChxr_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Trx_tChxr_Account] FOREIGN KEY([ChxrAccountId])
	REFERENCES [dbo].[tChxr_Account] ([ChxrAccountId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Session_tChxr_Partner]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Session]'))
BEGIN
    ALTER TABLE [dbo].[tChxr_Session]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Session_tChxr_Partner] FOREIGN KEY([ChxrPartnerId])
	REFERENCES [dbo].[tChxr_Partner] ([ChxrPartnerId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxrSim_Invoice_tChxrSim_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxrSim_Invoice]'))
BEGIN
    ALTER TABLE [dbo].[tChxrSim_Invoice]  WITH CHECK ADD  CONSTRAINT [FK_tChxrSim_Invoice_tChxrSim_Account] FOREIGN KEY([ChxrSimAccountId])
	REFERENCES [dbo].[tChxrSim_Account] ([ChxrSimAccountId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCheckImages_tTxn_Check]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCheckImages]'))
BEGIN
    ALTER TABLE [dbo].[tCheckImages]  WITH CHECK ADD  CONSTRAINT [FK_tCheckImages_tTxn_Check] FOREIGN KEY([TransactionId])
	REFERENCES [dbo].[tTxn_Check] ([TransactionId])
END
GO

----IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Trx_tTxn_Check]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]'))
----BEGIN
----    ALTER TABLE [dbo].[tChxr_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Trx_tTxn_Check] FOREIGN KEY([TransactionId])
----	REFERENCES [dbo].[tTxn_Check] ([TransactionId])
----END
----GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Check_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Check]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_Check]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_Check_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Account_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Account]'))
BEGIN
	ALTER TABLE [dbo].[tChxr_Account]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Account_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Account]'))
BEGIN
	ALTER TABLE [dbo].[tChxr_Account]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Account_tCustomers] FOREIGN KEY([CustomerId])
	REFERENCES [dbo].[tCustomers] ([CustomerId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxrSim_Account_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxrSim_Account]'))
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Account]  WITH CHECK ADD  CONSTRAINT [FK_tChxrSim_Account_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxrSim_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxrSim_Account]'))
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Account]  WITH CHECK ADD  CONSTRAINT [FK_tChxrSim_Account_tCustomers] FOREIGN KEY([CustomerId])
	REFERENCES [dbo].[tCustomers] ([CustomerId])
END
GO

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tMessageCenter_TxnId]') AND parent_object_id = OBJECT_ID(N'[dbo].[tMessageCenter]'))
BEGIN
    ALTER TABLE [dbo].[tMessageCenter]  WITH CHECK ADD  CONSTRAINT [FK_tMessageCenter_TxnId] FOREIGN KEY([TransactionId])
	REFERENCES [dbo].[tTxn_Check] ([TransactionId])
END
GO


--============ Alter RowGuid column as Nullable ======================


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCheckImages' AND COLUMN_NAME = 'CheckPK' )
BEGIN
	ALTER TABLE tCheckImages 
	ALTER COLUMN CheckPK UNIQUEIDENTIFIER NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Partner' AND COLUMN_NAME = 'ChxrPartnerPK')
BEGIN
	ALTER TABLE tChxr_Partner 
	ALTER COLUMN ChxrPartnerPK UNIQUEIDENTIFIER  NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Session' AND COLUMN_NAME IN ('ChxrPartnerPK'))
BEGIN
	ALTER TABLE tChxr_Session 
	ALTER COLUMN ChxrPartnerPK UNIQUEIDENTIFIER NULL  
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'ChxrAccountPK' )
BEGIN
	ALTER TABLE tChxr_Account 
	ALTER COLUMN ChxrAccountPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'ChxrAccountPK' )
BEGIN
	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN ChxrAccountPK UNIQUEIDENTIFIER NULL	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx' AND COLUMN_NAME IN ('ChxrAccountPK', 'ChannelPartnerId'))
BEGIN
	ALTER TABLE tChxr_Trx 
	ALTER COLUMN ChxrAccountPK UNIQUEIDENTIFIER NULL  

	ALTER TABLE tChxr_Trx 
	ALTER COLUMN ChannelPartnerId INT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx_Aud' AND COLUMN_NAME IN ('ChxrTrxPK', 'ChxrAccountPK', 'ChannelPartnerId'))
BEGIN
	ALTER TABLE tChxr_Trx_Aud 
	ALTER COLUMN ChxrTrxPK UNIQUEIDENTIFIER NULL    
	
	ALTER TABLE tChxr_Trx_Aud 
	ALTER COLUMN ChxrAccountPK UNIQUEIDENTIFIER NULL 
	
	ALTER TABLE tChxr_Trx_Aud 
	ALTER COLUMN ChannelPartnerId INT NULL  
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'ChxrSimAccountPK' )
BEGIN
	ALTER TABLE tChxrSim_Account 
	ALTER COLUMN ChxrSimAccountPK  UNIQUEIDENTIFIER NULL  
END 
GO


IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Invoice' AND COLUMN_NAME IN ('ChxrSimAccountPK'))
BEGIN
	ALTER TABLE tChxrSim_Invoice 
	ALTER COLUMN ChxrSimAccountPK UNIQUEIDENTIFIER NULL  	
END
GO


IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageCenter' AND COLUMN_NAME IN ('AgentPK', 'TxnId'))
BEGIN
	ALTER TABLE tMessageCenter 
	ALTER COLUMN AgentPK UNIQUEIDENTIFIER NULL	

	ALTER TABLE tMessageCenter 
	ALTER COLUMN TxnId UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME IN ('CustomerSessionPK', 'AccountPK', 'CXNState', 'CXEId', 'CXNId'))
BEGIN

	ALTER TABLE tTxn_Check 
	ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL

	ALTER TABLE tTxn_Check 
	ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL	
	
	ALTER TABLE tTxn_Check 
	ALTER COLUMN CXNState INT NULL	
	
	ALTER TABLE tTxn_Check 
	ALTER COLUMN CXEId BIGINT NULL			

	ALTER TABLE tTxn_Check 
	ALTER COLUMN CXNId BIGINT NULL	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'CustomerScore' )
BEGIN
	ALTER TABLE tChxr_Account 
	ALTER COLUMN CustomerScore INT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME IN ('Phone', 'City', 'State', 'Zip', 'Address1', 'LastName', 'FirstName') )
BEGIN
	ALTER TABLE tChxr_Account 
	ALTER COLUMN Phone NVARCHAR(20) NULL

	ALTER TABLE tChxr_Account 
	ALTER COLUMN City NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account 
	ALTER COLUMN State NVARCHAR(2) NULL

	ALTER TABLE tChxr_Account 
	ALTER COLUMN Zip NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account 
	ALTER COLUMN FirstName NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account 
	ALTER COLUMN LastName NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account 
	ALTER COLUMN Address1 NVARCHAR(100) NULL

	
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'CustomerScore' )
BEGIN
	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN CustomerScore INT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME IN ('Phone', 'City', 'State', 'Zip', 'Address1', 'LastName', 'FirstName') )
BEGIN
	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN Phone NVARCHAR(20) NULL

	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN City NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN State NVARCHAR(2) NULL

	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN Zip NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN FirstName NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN LastName NVARCHAR(50) NULL

	ALTER TABLE tChxr_Account_Aud 
	ALTER COLUMN Address1 NVARCHAR(100) NULL

	
END
GO

--============ Add Default constraint to RowGuid column ======================


IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tCheckImages_CheckPK' )
BEGIN
	ALTER TABLE tCheckImages ADD CONSTRAINT DF_tCheckImages_CheckPK DEFAULT NEWID() FOR CheckPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Partner_ChxrPartnerPK')
BEGIN
	ALTER TABLE tChxr_Partner ADD CONSTRAINT DF_tChxr_Partner_ChxrPartnerPK DEFAULT NEWID() FOR ChxrPartnerPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Session_ChxrSessionPK')
BEGIN
	ALTER TABLE tChxr_Session ADD CONSTRAINT DF_tChxr_Session_ChxrSessionPK DEFAULT NEWID() FOR ChxrSessionPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Account_ChxrAccountPK')
BEGIN
	ALTER TABLE tChxr_Account ADD CONSTRAINT DF_tChxr_Account_ChxrAccountPK DEFAULT NEWID() FOR ChxrAccountPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Account_Aud_ChxrAccountPK')
BEGIN
	ALTER TABLE tChxr_Account_Aud ADD CONSTRAINT DF_tChxr_Account_Aud_ChxrAccountPK DEFAULT NEWID() FOR ChxrAccountPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Trx_ChxrTrxPK')
BEGIN
	ALTER TABLE tChxr_Trx ADD CONSTRAINT DF_tChxr_Trx_ChxrTrxPK DEFAULT NEWID() FOR ChxrTrxPK
END
GO


IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Trx_Aud_ChxrTrxPK')
BEGIN
	ALTER TABLE tChxr_Trx_Aud ADD CONSTRAINT DF_tChxr_Trx_Aud_ChxrTrxPK DEFAULT NEWID() FOR ChxrTrxPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxrSim_Account_ChxrSimAccountPK')
BEGIN
	ALTER TABLE tChxrSim_Account ADD CONSTRAINT DF_tChxrSim_Account_ChxrSimAccountPK DEFAULT NEWID() FOR ChxrSimAccountPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxrSim_Invoice_ChxrSimInvoicePK')
BEGIN
	ALTER TABLE tChxrSim_Invoice ADD CONSTRAINT DF_tChxrSim_Invoice_ChxrSimInvoicePK DEFAULT NEWID() FOR ChxrSimInvoicePK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tMessageCenter_MessageCenterPK')
BEGIN
	ALTER TABLE tMessageCenter ADD CONSTRAINT DF_tMessageCenter_MessageCenterPK DEFAULT NEWID() FOR MessageCenterPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_Check_TxnPK')
BEGIN
	ALTER TABLE tTxn_Check ADD CONSTRAINT DF_tTxn_Check_TxnPK DEFAULT NEWID() FOR TxnPK
END
GO
