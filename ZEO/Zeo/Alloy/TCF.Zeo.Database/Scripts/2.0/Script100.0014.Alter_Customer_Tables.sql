--==============================ADD new column for Customer Audit table ================================================
--==============================Starts Here ============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'Occupation')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD Occupation NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'Employer')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD Employer NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'EmployerPhone')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD EmployerPhone NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'OccupationDescription')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD OccupationDescription NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'GovtIdTypeId')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD GovtIdTypeId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'GovtIdentification')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD GovtIdentification NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'GovtIDExpirationDate')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD GovtIDExpirationDate DATE NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'GovtIdIssueDate')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD GovtIdIssueDate DATE NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'Group1')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD Group1 INT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'Group2')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD Group2 INT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'IsPartnerAccountHolder')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD IsPartnerAccountHolder BIT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'ReferralCode')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD ReferralCode NVARCHAR(16) NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'AgentSessionID')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD AgentSessionID BIGINT NOT NULL DEFAULT 0
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'LastUpdatedAgentSessionID')
BEGIN
    ALTER TABLE tCustomers_Aud 
	ADD LastUpdatedAgentSessionID BIGINT NOT NULL DEFAULT 0
END
GO
--==============================Ends Here ============================================================================


--==============================ADD new column for Customer table ====================================================
--==============================Starts Here ==========================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'Occupation')
BEGIN
    ALTER TABLE tCustomers 
	ADD Occupation NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'Employer')
BEGIN
    ALTER TABLE tCustomers 
	ADD Employer NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'EmployerPhone')
BEGIN
    ALTER TABLE tCustomers 
	ADD EmployerPhone NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'OccupationDescription')
BEGIN
    ALTER TABLE tCustomers 
	ADD OccupationDescription NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'GovtIdTypeId')
BEGIN
    ALTER TABLE tCustomers 
	ADD GovtIdTypeId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'GovtIdentification')
BEGIN
    ALTER TABLE tCustomers 
	ADD GovtIdentification NVARCHAR(255) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'GovtIDExpirationDate')
BEGIN
    ALTER TABLE tCustomers 
	ADD GovtIDExpirationDate DATE NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'GovtIdIssueDate')
BEGIN
    ALTER TABLE tCustomers 
	ADD GovtIdIssueDate DATE NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'Group1')
BEGIN
    ALTER TABLE tCustomers 
	ADD Group1 INT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'Group2')
BEGIN
    ALTER TABLE tCustomers 
	ADD Group2 INT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'IsPartnerAccountHolder')
BEGIN
    ALTER TABLE tCustomers 
	ADD IsPartnerAccountHolder BIT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'ReferralCode')
BEGIN
    ALTER TABLE tCustomers 
	ADD ReferralCode NVARCHAR(16) NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'AgentSessionID')
BEGIN
    ALTER TABLE tCustomers 
	ADD AgentSessionID BIGINT NOT NULL DEFAULT 0
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'LastUpdatedAgentSessionID')
BEGIN
    ALTER TABLE tCustomers 
	ADD LastUpdatedAgentSessionID BIGINT NOT NULL DEFAULT 0
END
GO

--==============================Ends Here ===========================================================================

--==============================Disable the Triggers for Customer related tables ====================================
--==============================Starts Here =========================================================================

DISABLE TRIGGER [trCustomerEmploymentDetailsAudit] on tCustomerEmploymentDetails;
DISABLE TRIGGER [trCustomerGovernmentIdDetailsAudit] on tCustomerGovernmentIdDetails;
DISABLE TRIGGER [tPartnerCustomers_Audit] on tPartnerCustomers;
DISABLE TRIGGER [tPartnerCustomerGroupSettings_Delete] on tPartnerCustomerGroupSettings;
DISABLE TRIGGER [tPartnerCustomerGroupSettings_Insert_Update] on tPartnerCustomerGroupSettings;

--==============================Ends Here =========================================================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type='UQ' AND name = 'UX_CustomerID')
BEGIN
	ALTER TABLE [tCustomers]
	ADD CONSTRAINT UX_CustomerID UNIQUE ([CustomerID])  -- needed for creating below FK relationship
END
GO


----- Rename PAN to CustomerId
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerPreferedProducts' AND COLUMN_NAME = 'PAN' )
BEGIN
	 EXEC SP_RENAME 'tCustomerPreferedProducts.PAN','CustomerID','COLUMN'
END
GO

----- Rename ContactTypePK to ContactTypeId
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tContactTypes' AND COLUMN_NAME = 'ContactTypePK' )
BEGIN
	 EXEC SP_RENAME 'tContactTypes.ContactTypePK','ContactTypeId','COLUMN'
END
GO


--============ ADD FK constraints in customer related tables ========================================================
--==============================Starts Here =========================================================================

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
		AND CONSTRAINT_NAME = 'FK_tCustomerPreferedProducts_tCustomers'
)
BEGIN
	ALTER TABLE [dbo].[tCustomerPreferedProducts]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerPreferedProducts_tCustomers] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[tCustomers] ([CustomerID])
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomerFeeAdjustments'
		AND CONSTRAINT_NAME = 'FK_tCustomerFeeAdjustments_tCustomers'
)
BEGIN
	ALTER TABLE [dbo].[tCustomerFeeAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerFeeAdjustments_tCustomers] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[tCustomers] ([CustomerID])
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tAccounts'
		AND CONSTRAINT_NAME = 'FK_tAccounts_tCustomers'
)
BEGIN
	ALTER TABLE tAccounts 
	DROP CONSTRAINT FK_tAccounts_tCustomers
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomerAccounts'
		AND CONSTRAINT_NAME = 'FK_tCustomerAccounts_tCustomer'
)
BEGIN
	ALTER TABLE tCustomerAccounts 
	DROP CONSTRAINT FK_tCustomerAccounts_tCustomer
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomerSessions'
		AND CONSTRAINT_NAME = 'FK_tCustomerSessions_tCustomers'
)
BEGIN
	ALTER TABLE tCustomerSessions 
	DROP CONSTRAINT FK_tCustomerSessions_tCustomers
END
GO

DECLARE @FkConstraint NVARCHAR(MAX) = N''
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomerEmploymentDetails'
		AND CONSTRAINT_TYPE = 'FOREIGN KEY'
)
BEGIN

	SELECT @FkConstraint = CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	WHERE TABLE_NAME = 'tCustomerEmploymentDetails'
	AND Constraint_Type = 'FOREIGN KEY'
	
	EXEC ('ALTER TABLE tCustomerEmploymentDetails DROP CONSTRAINT ' + @FkConstraint)
	
END
GO

DECLARE @Constraint NVARCHAR(MAX) = N''
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails'
		AND CONSTRAINT_TYPE = 'FOREIGN KEY'
)
BEGIN
	SELECT @Constraint = CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	WHERE TABLE_NAME = 'tCustomerGovernmentIdDetails'
	AND Constraint_Type = 'FOREIGN KEY'
	
	EXEC ('ALTER TABLE tCustomerGovernmentIdDetails DROP CONSTRAINT ' + @Constraint)	
END
GO

DECLARE @Constraint NVARCHAR(MAX) = N''
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tStates'
		AND CONSTRAINT_TYPE = 'FOREIGN KEY'
)
BEGIN
	SELECT @Constraint = CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	WHERE TABLE_NAME = 'tStates'
	AND Constraint_Type = 'FOREIGN KEY'
	
	EXEC ('ALTER TABLE tStates DROP CONSTRAINT ' + @Constraint)	
END
GO

--IF EXISTS (
--		SELECT 1
--		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
--		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
--		AND CONSTRAINT_NAME = 'FK_tCustomerPreferedProducts_tCustomers'
--)
--BEGIN
--	ALTER TABLE tCustomerPreferedProducts 
--	DROP CONSTRAINT FK_tCustomerPreferedProducts_tCustomers
--END
--GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tCustomers'
		AND CONSTRAINT_NAME = 'PK_tCustomers' 
		AND COLUMN_NAME = 'CustomerPK'
)
BEGIN
	ALTER TABLE tCustomers 
	DROP CONSTRAINT PK_tCustomers
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomers'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tCustomers 
	ADD CONSTRAINT PK_Customers PRIMARY KEY CLUSTERED (CustomerID)
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_BillPay'
		AND CONSTRAINT_NAME = 'FK_tTxn_BillPay_tCustomerSessions'
)
BEGIN
	ALTER TABLE tTxn_BillPay 
	DROP CONSTRAINT FK_tTxn_BillPay_tCustomerSessions
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_Cash'
		AND CONSTRAINT_NAME = 'FK_tTxn_Cash_tCustomerSessions'
)
BEGIN
	ALTER TABLE tTxn_Cash 
	DROP CONSTRAINT FK_tTxn_Cash_tCustomerSessions
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_Check'
		AND CONSTRAINT_NAME = 'FK_tTxn_Check_tCustomerSessions'
)
BEGIN
	ALTER TABLE tTxn_Check 
	DROP CONSTRAINT FK_tTxn_Check_tCustomerSessions
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_Funds'
		AND CONSTRAINT_NAME = 'FK_tTxn_Funds_tCustomerSessions'
)
BEGIN
	ALTER TABLE tTxn_Funds 
	DROP CONSTRAINT FK_tTxn_Funds_tCustomerSessions
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_MoneyOrder'
		AND CONSTRAINT_NAME = 'FK_tTxn_MoneyOrder_tCustomerSessions'
)
BEGIN
	ALTER TABLE tTxn_MoneyOrder 
	DROP CONSTRAINT FK_tTxn_MoneyOrder_tCustomerSessions
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTxn_MoneyTransfer'
		AND CONSTRAINT_NAME = 'FK_tTxn_MoneyTransfer_tCustomerSessions'
)
BEGIN
	ALTER TABLE tTxn_MoneyTransfer 
	DROP CONSTRAINT FK_tTxn_MoneyTransfer_tCustomerSessions
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tCustomerSessions'
		AND CONSTRAINT_NAME = 'PK_tCustomerSessions' 
		AND COLUMN_NAME = 'CustomerSessionPK'
)
BEGIN
	ALTER TABLE tCustomerSessions 
	DROP CONSTRAINT PK_tCustomerSessions
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tCustomerSessions'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tCustomerSessions 
	ADD CONSTRAINT PK_tCustomerSessions PRIMARY KEY CLUSTERED (CustomerSessionID)
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tCustomers_Aud'
		AND CONSTRAINT_NAME = 'PK_tCustomers_Aud'
		AND COLUMN_NAME IN ('CustomerPK','RevisionNo')
)
BEGIN
	ALTER TABLE tCustomers_Aud 
	DROP CONSTRAINT PK_tCustomers_Aud
END
GO


--IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tCountries' AND OBJECT_NAME(OBJECT_ID) != 'PK_tCountries')
--BEGIN
--     -- PK constraint Name generated by server which was not common for all the DB server, So fetch the PK constraint Name and DROP
--    DECLARE @PKName varchar(max)
--	SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tCountries'	
--	EXEC('ALTER TABLE [dbo].[tCountries] DROP CONSTRAINT ' + @PKName) 
--END
--GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tMessageStore'
		AND CONSTRAINT_NAME = 'PK_tMessageStore'
		AND COLUMN_NAME = 'MessageStorePK'
)
BEGIN
	ALTER TABLE tMessageStore 
	DROP CONSTRAINT PK_tMessageStore
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tLegalCodes'
		AND CONSTRAINT_NAME = 'PK_tLegalCodes'
		AND COLUMN_NAME = 'LegalCodesPK'
)
BEGIN
	ALTER TABLE tLegalCodes 
	DROP CONSTRAINT PK_tLegalCodes
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tOccupations'
		AND CONSTRAINT_NAME = 'PK_tOccupation'
		AND COLUMN_NAME = 'OccupationsPK'
)
BEGIN
	ALTER TABLE tOccupations 
	DROP CONSTRAINT PK_tOccupation
END
GO


IF NOT EXISTS(
			SELECT 1 
			FROM INFORMATION_SCHEMA.COLUMNS 
			WHERE TABLE_NAME = 'tCustomerSessions' 
			AND COLUMN_NAME IN ('AgentSessionId')
)
BEGIN
    ALTER TABLE tCustomerSessions 
	ADD AgentSessionId BIGINT NULL
END
GO

------------------------------------ TCIS_AAcount changes --------------------------------------------

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE TABLE_NAME = 'tTCIS_Account'
		AND CONSTRAINT_NAME = 'PK_tTCIS_Account'
		AND COLUMN_NAME = 'TCISAccountPK'
)
BEGIN
	ALTER TABLE tTCIS_Account 
	DROP CONSTRAINT PK_tTCIS_Account
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTCIS_Account'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tTCIS_Account 
	ADD CONSTRAINT PK_tTCIS_Account PRIMARY KEY CLUSTERED (TCISAccountID)
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTCIS_Account' 
		AND COLUMN_NAME = 'CustomerID'
)
BEGIN
	ALTER TABLE tTCIS_Account 
	ADD CustomerID BIGINT NULL
END
GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTCIS_Account' 
		AND COLUMN_NAME = 'CustomerSessionID'
)
BEGIN
	ALTER TABLE tTCIS_Account 
	ADD CustomerSessionID BIGINT NULL
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tTCIS_Account'
		AND CONSTRAINT_NAME = 'FK_tTCIS_Account_tCustomers'
)
BEGIN
	ALTER TABLE [dbo].[tTCIS_Account]  WITH CHECK ADD  CONSTRAINT [FK_tTCIS_Account_tCustomers] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[tCustomers] ([CustomerID])
END
GO

--IF NOT EXISTS (
--		SELECT 1
--		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
--		WHERE TABLE_NAME = 'tTCIS_Account'
--		AND CONSTRAINT_NAME = 'FK_tTCIS_Account_tCustomerSessions'
--)
--BEGIN
--	ALTER TABLE [dbo].[tTCIS_Account]  WITH CHECK ADD  CONSTRAINT [FK_tTCIS_Account_tCustomerSessions] FOREIGN KEY([CustomerSessionID])
--	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])
--END
--GO

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tTCIS_Account' 
		AND COLUMN_NAME = 'CustomerRevisionNo'
)
BEGIN
	ALTER TABLE tTCIS_Account 
	ADD CustomerRevisionNo BIGINT NULL
END
GO


IF NOT EXISTS(
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= 'tCustomerSessionCounterIdDetails'
		AND COLUMN_NAME = 'CustomerSessionID'
)
BEGIN 
	ALTER TABLE tCustomerSessionCounterIdDetails
	ADD CustomerSessionID BIGINT NULL
END
GO

IF NOT EXISTS(
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= 'tCustomerSessionShoppingCarts'
		AND COLUMN_NAME = 'CustomerSessionID'
)
BEGIN 
	ALTER TABLE tCustomerSessionShoppingCarts
	ADD CustomerSessionID BIGINT NULL
END
GO
-------------------------------Audit table for tTCIS_Account -------------------------------

IF NOT EXISTS (
	SELECT 1	
	FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[tTCIS_Account_Aud]') 
	AND type in (N'U')
)
BEGIN
CREATE TABLE [dbo].[tTCIS_Account_Aud](
	[TCISAccountID] [BIGINT] NOT NULL,
	[PartnerAccountNumber] [NVARCHAR](100) NULL,
	[RelationshipAccountNumber] [NVARCHAR](100) NULL,
	[ProfileStatus] [SMALLINT] NULL,
	[DTTerminalCreate] [DATETIME] NULL,
	[DTTerminalLastModified] [DATETIME] NULL,
	[BankId] [NVARCHAR](40) NULL,
	[BranchId] [NVARCHAR](40) NULL,
	[TcfCustInd] [BIT] NULL,
	[DTServerCreate] [DATETIME] NOT NULL,
	[DTServerLastModified] [DATETIME] NULL,
	[CustomerID] [BIGINT] NULL,
	[CustomerSessionID] [BIGINT] NULL,
	[CustomerRevisionNo] [INT] NULL,
	[RevisionNo] [BIGINT] not null,
	[AuditEvent] [SMALLINT] not null,
	[DTAudit] [DATETIME] NOT NULL
)
END
GO

--IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tCountries' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCountries')
--BEGIN
--ALTER TABLE [dbo].[tCountries] ADD  CONSTRAINT [PK_tCountries] PRIMARY KEY CLUSTERED (Code)
--END
--GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tMessageStore' AND OBJECT_NAME(OBJECT_ID) = 'PK_tMessageStore')
BEGIN
ALTER TABLE [dbo].[tMessageStore] ADD  CONSTRAINT [PK_tMessageStore] PRIMARY KEY CLUSTERED (MessageStoreID)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tLegalCodes'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tLegalCodes 
	ADD CONSTRAINT PK_tLegalCodes PRIMARY KEY CLUSTERED (LegalCodesID)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tOccupations'
		AND Constraint_Type = 'PRIMARY KEY'
)
BEGIN
	ALTER TABLE tOccupations 
	ADD CONSTRAINT PK_tOccupation PRIMARY KEY CLUSTERED (OccupationsID)
END
GO

------------------------------------ tChannelPartnerIDTypeMapping changes -----------------------------------------

IF NOT EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS 
		WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping'
		AND COLUMN_NAME = 'NexxoIdTypeID'
)
BEGIN
	ALTER TABLE tChannelPartnerIDTypeMapping 
	ADD NexxoIdTypeID BIGINT NULL
END
GO


------Making the customerPk in the tCustomers table nullable
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tCustomers
	ALTER COLUMN CustomerPK UNIQUEIDENTIFIER NULL 
END
GO

--Making PK column NULL
IF EXISTS(
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= 'tCustomerSessionCounterIdDetails'
		AND COLUMN_NAME = 'CustomerSessionPK'
)
BEGIN 
	ALTER TABLE tCustomerSessionCounterIdDetails
	ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL
END
GO

------Making the customerPk in the tCustomers table nullable
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tCustomerSessions
	ALTER COLUMN CustomerPK UNIQUEIDENTIFIER NULL 
END
GO

------Making the customerPk in the tCustomers table nullable
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tCustomerSessions
	ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL 
END
GO
------Making the customerPk in the tCustomers table nullable
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tCustomers_Aud
	ALTER COLUMN CustomerPK UNIQUEIDENTIFIER NULL 
END
GO

------Making the TCISAccountPK in the tTCIS_Account_Aud table nullable
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account_Aud' AND COLUMN_NAME = 'TCISAccountPK')
BEGIN
    ALTER TABLE tTCIS_Account_Aud
	ALTER COLUMN TCISAccountPK UNIQUEIDENTIFIER NULL 
END
GO

------Making the TCISAccountPK in the tTCIS_Account table nullable
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'TCISAccountPK')
BEGIN
    ALTER TABLE tTCIS_Account
	ALTER COLUMN TCISAccountPK UNIQUEIDENTIFIER NULL 
END
GO