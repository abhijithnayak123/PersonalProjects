--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <10-11-2016>
-- Description:	 Alter PK and FK constraints for BillPay related tables
-- Jira ID:		<AL-8320>
-- ================================================================================


--==============================================================================================================================
--Drop foreign key constraints from all BillPay related tables
--==============================================================================================================================

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_BillPay_tAccounts]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tTxn_BillPay]'))
BEGIN
    ALTER TABLE [dbo].[tTxn_BillPay] DROP CONSTRAINT FK_tTxn_BillPay_tAccounts;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_BillPay_Commit_tCustomerAccounts]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tTxn_BillPay_Commit]'))
BEGIN
    ALTER TABLE [dbo].[tTxn_BillPay_Commit] DROP CONSTRAINT FK_tTxn_BillPay_Commit_tCustomerAccounts;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_BillPay_Stage_tCustomerAccounts]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tTxn_BillPay_Stage]'))
BEGIN
    ALTER TABLE [dbo].[tTxn_BillPay_Stage] DROP CONSTRAINT FK_tTxn_BillPay_Stage_tCustomerAccounts;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_BillPay_Trx_tWUnion_BillPay_AccountPK]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_BillPay_Trx] DROP CONSTRAINT FK_tWUnion_BillPay_Trx_tWUnion_BillPay_AccountPK;
END;

DECLARE @FKName VARCHAR(500);
SELECT @FKName = f.name
FROM SYS.FOREIGN_KEYS AS f
     INNER JOIN SYS.FOREIGN_KEY_COLUMNS AS fc ON f.OBJECT_ID = fc.CONSTRAINT_OBJECT_ID
     INNER JOIN SYS.OBJECTS AS o ON o.OBJECT_ID = fc.REFERENCED_OBJECT_ID
WHERE OBJECT_NAME(f.PARENT_OBJECT_ID) = 'tWUnion_ImportBillers'
      AND COL_NAME(fc.PARENT_OBJECT_ID, fc.parent_column_id) = 'WUBillPayAccountPK'
      AND OBJECT_NAME(f.REFERENCED_OBJECT_ID) = 'tWUnion_BillPay_Account'
      AND COL_NAME(fc.REFERENCED_OBJECT_ID, fc.REFERENCED_COLUMN_ID) = 'WUBillPayAccountPK';

IF(@FKName IS NOT NULL)
    BEGIN
        EXEC ('ALTER TABLE  tWUnion_ImportBillers DROP CONSTRAINT '+@FKName);
    END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tPartnerCatalog_MasterCatalogPK_tMasterCatalog_MasterCatalogPK]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tPartnerCatalog]'))
BEGIN
    ALTER TABLE [dbo].[tPartnerCatalog] DROP CONSTRAINT FK_tPartnerCatalog_MasterCatalogPK_tMasterCatalog_MasterCatalogPK;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_BillPay_Commit_tCustomerAccounts]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tTxn_BillPay_Commit]'))
BEGIN
    ALTER TABLE [dbo].[tTxn_BillPay_Commit] DROP CONSTRAINT FK_tTxn_BillPay_Commit_tCustomerAccounts;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_BillPay_Stage_tCustomerAccounts]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tTxn_BillPay_Stage]'))
BEGIN
    ALTER TABLE [dbo].[tTxn_BillPay_Stage] DROP CONSTRAINT FK_tTxn_BillPay_Stage_tCustomerAccounts;
END;

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomerPreferedProducts_tCustomers]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tCustomerPreferedProducts]'))
BEGIN
    ALTER TABLE [dbo].[tCustomerPreferedProducts] DROP CONSTRAINT FK_tCustomerPreferedProducts_tCustomers;
END;


--==============================================================================================================================
--Drop Primary key constraints from all BillPay related tables
--==============================================================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tTxn_BillPay' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_BillPay')
BEGIN
	ALTER TABLE [dbo].[tTxn_BillPay] DROP CONSTRAINT PK_tTxn_BillPay
END

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tTxn_BillPay_Commit' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_BillPay_Commit')
BEGIN
	ALTER TABLE [dbo].[tTxn_BillPay_Commit] DROP CONSTRAINT PK_tTxn_BillPay_Commit
END

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tTxn_BillPay_Stage' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_BillPay_Stage')
BEGIN
	ALTER TABLE [dbo].[tTxn_BillPay_Stage] DROP CONSTRAINT PK_tTxn_BillPay_Stage
END

DECLARE @pkName VARCHAR(50);
SELECT @pkName = name
FROM SYS.KEY_CONSTRAINTS
WHERE type = 'PK'
      AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tMasterCatalog';

IF @pkName IS NOT NULL
BEGIN
	EXEC ('ALTER TABLE tMasterCatalog DROP CONSTRAINT '+@pkName);
END;

SELECT @pkName = name
FROM SYS.KEY_CONSTRAINTS
WHERE TYPE = 'PK'
      AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tPartnerCatalog';

IF @pkName IS NOT NULL
BEGIN
	EXEC ('ALTER TABLE tPartnerCatalog DROP CONSTRAINT '+@pkName);
END;

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tWUnion_Catalog' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Catalog')
BEGIN
	ALTER TABLE [dbo].[tWUnion_Catalog] DROP CONSTRAINT PK_tWUnion_Catalog
END;

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tWUnion_BillPay_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_BillPay_Account')
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Account] DROP CONSTRAINT PK_tWUnion_BillPay_Account
END;

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tWUnion_BillPay_Trx' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_BillPay_Trx')
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Trx] DROP CONSTRAINT PK_tWUnion_BillPay_Trx
END;

SELECT @pkName = name
FROM SYS.KEY_CONSTRAINTS
WHERE type = 'PK'
      AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tWUnion_ImportBillers';

IF @pkName IS NOT NULL
BEGIN
    EXEC ('ALTER TABLE tWUnion_ImportBillers DROP CONSTRAINT '+ @pkName);
END;

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tCustomerPreferedProducts' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCustomerPreferedProducts')
BEGIN
	ALTER TABLE [dbo].[tCustomerPreferedProducts] DROP CONSTRAINT PK_tCustomerPreferedProducts
END

---Deleting the unique key constraints
IF OBJECT_ID('UX_WUBPTrxConstraint') IS  NOT NULL
BEGIN
	ALTER TABLE dbo.tWUnion_BillPay_Trx DROP CONSTRAINT UX_WUBPTrxConstraint
END
--=========================================================================================================
-- Add new columns in Billpay related tables
--=========================================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME = 'PartnerCatalogID')
BEGIN
	ALTER TABLE tPartnerCatalog 
	ADD PartnerCatalogId  BIGINT NOT NULL IDENTITY(1000000,1)   	
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME = 'MasterCatalogID')
BEGIN
	ALTER TABLE tPartnerCatalog 
	ADD MasterCatalogId  BIGINT NULL  	
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME in ('CustomerSessionID','ProviderAccountID','ProviderID','CustomerRevisionNo'))
BEGIN
	ALTER TABLE tTxn_BillPay 
	ADD CustomerSessionId  BIGINT NULL;	

	ALTER TABLE tTxn_BillPay 
	ADD ProviderAccountId  BIGINT NULL;  	

	ALTER TABLE tTxn_BillPay 
	ADD ProviderId  INT NULL;  	

	ALTER TABLE tTxn_BillPay 
	ADD CustomerRevisionNo INT NULL;  	

END


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'CXEState' )
BEGIN
	 EXEC sp_RENAME '[tTxn_BillPay].[CXEState]' , 'State' , 'COLUMN'	 
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_BillPay_Aud]') AND type in (N'U'))
BEGIN
CREATE TABLE tTxn_BillPay_Aud
  (	
	TransactionId BIGINT NULL,
    CustomerSessionId BIGINT NULL,	
	Amount MONEY NULL,
	Fee MONEY NULL,
	Description NVARCHAR(255) NULL,
	State INT NULL,
	DTTerminalCreate DATETIME NOT NULL,
	DTTerminalLastModified DATETIME NULL,
	ConfirmationNumber VARCHAR(50) NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL,
	CustomerRevisionNo BIGINT NULL,
	ProviderId INT NULL,
	ProviderAccountId BIGINT NULL,
	DTAudit DATETIME NOT NULL,
	AuditEvent SMALLINT NOT NULL,
	RevisionNo BIGINT NOT NULL,
	AccountNumber VARCHAR(50) NULL
 )
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'WUBillPayAccountID')
BEGIN
	ALTER TABLE tWUnion_BillPay_Trx 
	ADD WUBillPayAccountId  BIGINT NULL; 
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx_Aud]') AND type in (N'U'))
BEGIN
CREATE TABLE tWUnion_BillPay_Trx_Aud
(
	WUBillPayTrxId BIGINT  NOT NULL,
	WUBillPayAccountId BIGINT NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL,
	ProviderId int NOT NULL,
	Channel_Type VARCHAR(10) NOT NULL,
	Channel_Name VARCHAR(20) NOT NULL,
	Channel_Version VARCHAR(10) NOT NULL,
	Sender_FirstName VARCHAR(50) NOT NULL,
	Sender_Lastname VARCHAR(50) NOT NULL,
	Sender_AddressLine1 VARCHAR(50) NOT NULL,
	Sender_City VARCHAR(50) NOT NULL,
	Sender_State VARCHAR(50) NOT NULL,
	Sender_PostalCode VARCHAR(50) NULL,
	Sender_CountryCode VARCHAR(50) NULL,
	Sender_CurrencyCode VARCHAR(50) NULL,
	Sender_AddressLine2 VARCHAR(50) NULL,
	Sender_Street VARCHAR(50) NULL,
	WesternUnionCardNumber VARCHAR(15) NULL,
	LevelCode VARCHAR(50) NULL,
	Sender_Email VARCHAR(50) NULL,
	Sender_ContactPhone VARCHAR(50) NULL,
	Sender_DateOfBirth VARCHAR(50) NULL,
	BillerName VARCHAR(50) NOT NULL,
	Biller_CityCode VARCHAR(50) NULL,
	Customer_AccountNumber VARCHAR(50) NOT NULL,
	CountryCode VARCHAR(50) NULL,
	CurrencyCode VARCHAR(50) NULL,
	Financials_MunicipalTax VARCHAR(50) NULL,
	Financials_StateTax VARCHAR(50) NULL,
	Financials_CountTax VARCHAR(50) NULL,
	Financials_OriginatorsPrincipalAmount DECIMAL(18, 2) NULL,
	Financials_DestinationPrincipalAmount DECIMAL(18, 2) NULL,
	Financials_Fee DECIMAL(18, 2) NULL,
	Financials_GrossTotalAmount DECIMAL(18, 2) NULL,
	Financials_Total DECIMAL(18, 2) NULL,
	Financials_UndiscountedCharges DECIMAL(18, 2) NULL,
	Financials_DiscountedCharges DECIMAL(18, 2) NULL,
	PaymentDetails_Recording_CountryCode VARCHAR(50) NULL,
	PaymentDetails_Recording_CountryCurrency VARCHAR(50) NULL,
	PaymentDetails_Destination_CountryCode VARCHAR(20) NULL,
	PaymentDetails_Destination_CountryCurrency VARCHAR(20) NULL,
	PaymentDetails_Originating_CountryCode VARCHAR(20) NULL,
	PaymentDetails_Originating_CountryCurrency VARCHAR(20) NULL,
	PaymentDetails_Originating_City VARCHAR(50) NULL,
	PaymentDetails_Originating_State VARCHAR(50) NULL,
	PaymentDetails_TransactionType VARCHAR(50) NULL,
	PaymentDetails_PaymentType VARCHAR(50) NULL,
	PaymentDetails_ExchangeRate DECIMAL(18, 2) NULL,
	PaymentDetails_FixOnSend VARCHAR(50) NULL,
	PaymentDetails_ReceiptOptOut VARCHAR(50) NULL,
	PaymentDetails_AuthStatus VARCHAR(50) NULL,
	FillingDate VARCHAR(50) NULL,
	FillingTime VARCHAR(50) NULL,
	MTCN VARCHAR(50) NULL,
	NewMTCN VARCHAR(50) NULL,
	DfFields_PDSRequiredFlag VARCHAR(50) NULL,
	DfFields_TransactionFlag VARCHAR(50) NULL,
	DfFields_DeliveryServiceName VARCHAR(50) NULL,
	DeliveryCode VARCHAR(50) NULL,
	FusionScreen VARCHAR(50) NULL,
	ConvSessionCookie VARCHAR(200) NULL,
	ForeignRemoteSystem_Identifier VARCHAR(20) NULL,
	ForeignRemoteSystem_Reference_no VARCHAR(50) NULL,
	ForeignRemoteSystem_CounterId VARCHAR(20) NULL,
	InstantNotification_AddlServiceCharges VARCHAR(200) NULL,
	InstantNotification_AddlServiceLength VARCHAR(50) NULL,
	promotions_coupons_promotions VARCHAR(30) NULL,
	promotions_promo_code_description VARCHAR(250) NULL,
	promotions_promo_sequence_no VARCHAR(15) NULL,
	promotions_promo_name VARCHAR(50) NULL,
	promotions_promo_message VARCHAR(250) NULL,
	promotions_promo_discount_amount BIGINT NULL,
	promotions_promotion_error VARCHAR(250) NULL,
	promotions_sender_promo_code VARCHAR(30) NULL,
	Sender_ComplianceDetails_TemplateID VARCHAR(20) NULL,
	Sender_ComplianceDetails_IdDetails_IdType VARCHAR(10) NULL,
	Sender_ComplianceDetails_IdDetails_IdCountryOfIssue VARCHAR(50) NULL,
	Sender_ComplianceDetails_IdDetails_IdPlaceOfIssue VARCHAR(50) NULL,
	Sender_ComplianceDetails_IdDetails_IdNumber VARCHAR(50) NULL,
	Sender_ComplianceDetails_SecondID_IdType VARCHAR(10) NULL,
	Sender_ComplianceDetails_SecondID_IdCountryOfIssue VARCHAR(50) NULL,
	Sender_ComplianceDetails_SecondID_IdNumber VARCHAR(50) NULL,
	Sender_ComplianceDetails_DateOfBirth VARCHAR(20) NULL,
	Sender_ComplianceDetails_Occupation VARCHAR(50) NULL,
	Sender_ComplianceDetails_CurrentAddress_AddrLine1 VARCHAR(255) NULL,
	Sender_ComplianceDetails_CurrentAddress_AddrLine2 VARCHAR(255) NULL,
	Sender_ComplianceDetails_CurrentAddress_City VARCHAR(50) NULL,
	Sender_ComplianceDetails_CurrentAddress_StateCode VARCHAR(20) NULL,
	Sender_ComplianceDetails_CurrentAddress_PostalCode VARCHAR(20) NULL,
	Sender_ComplianceDetails_ContactPhone VARCHAR(20) NULL,
	Sender_ComplianceDetails_I_ActOnMyBehalf VARCHAR(10) NULL,
	Sender_ComplianceDetails_Ack_Flag VARCHAR(10) NULL,
	Sender_ComplianceDetails_ComplianceData_Buffer VARCHAR(500) NULL,
	Financials_PlusChargesAmount DECIMAL(18, 2) NULL,
	Financials_TotalDiscount DECIMAL(18, 2) NULL,
	Request_XML VARCHAR(max) NULL,
	Response_XML VARCHAR(max) NULL,
	WUCard_TotalPointsEarned VARCHAR(50) NULL,
	QPCompany_Department VARCHAR(100) NULL,
	DTTerminalCreate DATETIME NOT NULL,
	DTTerminalLastModified DATETIME NULL,
	MessageArea nVARCHAR(max) NULL,
	DTAudit DATETIME NOT NULL,
	AuditEvent SMALLINT NOT NULL,
	RevisionNo BIGINT NOT NULL
)
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_ImportBillers' AND COLUMN_NAME = 'WUBillPayAccountID')
BEGIN
	ALTER TABLE tWUnion_ImportBillers 
	ADD WUBillPayAccountId  BIGINT NULL  	
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME IN('CustomerID','CustomerSessionID','CustomerRevisionNo'))
BEGIN
	ALTER TABLE tWUnion_BillPay_Account 
	ADD CustomerId  BIGINT NULL  	

	ALTER TABLE tWUnion_BillPay_Account 
	ADD CustomerSessionId  BIGINT NULL  	

	ALTER TABLE tWUnion_BillPay_Account 
	ADD CustomerRevisionNo INT NULL  	
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Account_Aud]') AND type in (N'U'))
BEGIN
CREATE TABLE tWUnion_BillPay_Account_Aud
  (	
	WUBillPayAccountId BIGINT NOT NULL,
	DTTerminalCreate DATETIME NOT NULL,
	DTTerminalLastModified DATETIME NULL,
	CardNumber VARCHAR(50) NULL,
	PreferredCustomerLevelCode VARCHAR(250) NULL,
	SmsNotificationFlag VARCHAR(250) NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL,
	DTAudit DATETIME NOT NULL,
	AuditEvent SMALLINT NOT NULL,
	RevisionNo BIGINT NOT NULL,
	CustomerRevisionNo BIGINT NULL,
	CustomerSessionId BIGINT NULL,
	CustomerId BIGINT NULL
 )
END

--=========================================================================================================
--Adding PK constraints to the Billpay relatetd tables
--=========================================================================================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tMasterCatalog' AND OBJECT_NAME(OBJECT_ID) = 'PK_tMasterCatalog')
BEGIN
	ALTER TABLE [dbo].[tMasterCatalog] ADD CONSTRAINT [PK_tMasterCatalog] PRIMARY KEY CLUSTERED (MasterCatalogID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tPartnerCatalog' AND OBJECT_NAME(OBJECT_ID) = 'PK_tPartnerCatalog')
BEGIN
	ALTER TABLE [dbo].[tPartnerCatalog] ADD CONSTRAINT [PK_tPartnerCatalog] PRIMARY KEY CLUSTERED (PartnerCatalogID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_BillPay' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTrx_BillPay')
BEGIN
	ALTER TABLE [dbo].[tTxn_BillPay] ADD CONSTRAINT [PK_tTrx_BillPay] PRIMARY KEY CLUSTERED (TransactionID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Catalog' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_Catalog')
BEGIN
	ALTER TABLE [dbo].[tWUnion_Catalog] ADD CONSTRAINT [PK_tWUnion_Catalog] PRIMARY KEY CLUSTERED (WUCatalogID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_BillPay_Trx' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_BillPay_Trx')
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Trx] ADD CONSTRAINT [PK_tWUnion_BillPay_Trx] PRIMARY KEY CLUSTERED (WUBillPayTrxID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_BillPay_Account' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWU_BillPay_Account')
BEGIN
	ALTER TABLE [dbo].[tWUnion_BillPay_Account] ADD CONSTRAINT [PK_tWU_BillPay_Account] PRIMARY KEY CLUSTERED (WUBillPayAccountID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_ImportBillers' AND OBJECT_NAME(OBJECT_ID) = 'PK_tWUnion_ImportBillers')
BEGIN
	ALTER TABLE [dbo].[tWUnion_ImportBillers] ADD CONSTRAINT [PK_tWUnion_ImportBillers] PRIMARY KEY CLUSTERED (WUBillersID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tCustomerPreferedProducts' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCustomerPreferedProducts')
BEGIN
	ALTER TABLE [dbo].[tCustomerPreferedProducts] ADD CONSTRAINT [PK_tCustomerPreferedProducts] PRIMARY KEY CLUSTERED (CustProductID)
END

--=========================================================================================================
--Adding FK constraints to the Billpay related tables
--=========================================================================================================

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tPartnerCatalog_tMasterCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[tPartnerCatalog]'))
BEGIN
    ALTER TABLE [dbo].[tPartnerCatalog]  WITH CHECK ADD  CONSTRAINT [FK_tPartnerCatalog_tMasterCatalog] FOREIGN KEY(MasterCatalogID)
	REFERENCES [dbo].[tMasterCatalog] (MasterCatalogID)
END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_BillPay_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_BillPay]'))
BEGIN
    ALTER TABLE [dbo].[tTxn_BillPay]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_BillPay_tCustomerSessions] FOREIGN KEY(CustomerSessionID)
	REFERENCES [dbo].[tCustomerSessions] (CustomerSessionID)
END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_ImportBillers_tWUnion_BillPay_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_ImportBillers]'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_ImportBillers]  WITH CHECK ADD CONSTRAINT [FK_tWUnion_ImportBillers_tWUnion_BillPay_Account] FOREIGN KEY(WUBillPayAccountID)
    REFERENCES [dbo].[tWUnion_BillPay_Account] (WUBillPayAccountID)
END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_BillPay_Account_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Account]'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_BillPay_Account]  WITH CHECK ADD CONSTRAINT [FK_tWUnion_BillPay_Account_tCustomerSessions] FOREIGN KEY(CustomerSessionID)
    REFERENCES [dbo].[tCustomerSessions] (CustomerSessionID)
END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomerPreferedProducts_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomerPreferedProducts]'))
BEGIN
    ALTER TABLE [dbo].[tCustomerPreferedProducts]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerPreferedProducts_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUBillPay_tWUBillPayAccount]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_BillPay_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tWUBillPay_tWUBillPayAccount] FOREIGN KEY(WUBillPayAccountId)
	REFERENCES [dbo].[tWUnion_BillPay_Account] (WUBillPayAccountID)
END
--================================================================================================================
--Adding default values to the PK columns 
--================================================================================================================

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_Billpay_txnPK' )
BEGIN
	ALTER TABLE tTxn_BillPay ADD CONSTRAINT DF_tTxn_Billpay_txnPK DEFAULT NEWID() FOR TxnPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tMasterCatalog_masterCatalogPK' )
BEGIN
	ALTER TABLE tMasterCatalog ADD CONSTRAINT DF_tMasterCatalog_masterCatalogPK DEFAULT NEWID() FOR MasterCatalogPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tPartnerCatalog_catalogPK' )
BEGIN
	ALTER TABLE tPartnerCatalog ADD CONSTRAINT DF_tPartnerCatalog_catalogPK DEFAULT NEWID() FOR tPartnerCatalogPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_Catalog_catalogPK' )
BEGIN
	ALTER TABLE tWUnion_Catalog ADD CONSTRAINT DF_tWUnion_Catalog_catalogPK DEFAULT NEWID() FOR WUCatalogPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_BillPay_Trx_WUtrxPK' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Trx ADD CONSTRAINT DF_tWUnion_BillPay_Trx_WUtrxPK DEFAULT NEWID() FOR WUBillPayTrxPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tCustomerPreferedProducts_PK' )
BEGIN
	ALTER TABLE tCustomerPreferedProducts ADD CONSTRAINT DF_tCustomerPreferedProducts_PK DEFAULT NEWID() FOR CustProductPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_BillPay_Account_PK' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Account ADD CONSTRAINT DF_tWUnion_BillPay_Account_PK DEFAULT NEWID() FOR WUBillPayAccountPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_ImportBillers_PK' )
BEGIN
	ALTER TABLE tWUnion_ImportBillers ADD CONSTRAINT DF_tWUnion_ImportBillers_PK DEFAULT NEWID() FOR WUBillersPK
END
GO

--================================================================================================================
--Making the columns nullable which we are not using presently in the new design
--================================================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'CXEId' )
BEGIN
	ALTER TABLE tTxn_BillPay ALTER COLUMN CXEId BIGINT NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'CustomerSessionPK' )
BEGIN
	ALTER TABLE tTxn_BillPay ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'AccountPK' )
BEGIN
	ALTER TABLE tTxn_BillPay ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'ChannelParterId' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Trx ALTER COLUMN ChannelParterId INT NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'WUBillPayAccountPK' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Trx ALTER COLUMN WUBillPayAccountPK UNIQUEIDENTIFIER NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'FirstName' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Account ALTER COLUMN FirstName VARCHAR(50) NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'LastName' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Account ALTER COLUMN LastName VARCHAR(50) NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'Address1' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Account ALTER COLUMN Address1 VARCHAR(250) NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_ImportBillers' AND COLUMN_NAME = 'WUBillPayAccountPK' )
BEGIN
	ALTER TABLE tWUnion_ImportBillers ALTER COLUMN WUBillPayAccountPK UNIQUEIDENTIFIER NULL;
END
GO

------------------------------------------------DROPING FK'S FROM Tables-------------------------------------------------------------
--drop FK in tNexxoIdtypes referring tmastercountries
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tNexxoIdTypes_tMasterCountries]') AND parent_object_id = OBJECT_ID(N'[dbo].[tNexxoIdTypes]'))
BEGIN
ALTER TABLE dbo.tNexxoIdTypes DROP CONSTRAINT FK_tNexxoIdTypes_tMasterCountries
END

--drop FK in tNexxoIdtypes referring tstates
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tNexxoIdTypes_tStates]') AND parent_object_id = OBJECT_ID(N'[dbo].[tNexxoIdTypes]'))
BEGIN
ALTER TABLE dbo.tNexxoIdTypes DROP CONSTRAINT FK_tNexxoIdTypes_tStates
END

--drop Pk from tNexxoIdTypes
IF OBJECT_ID('UX_NexxoIdTypeID') IS NOT NULL
BEGIN
  ALTER TABLE dbo.tNexxoIdTypes DROP CONSTRAINT UX_NexxoIdTypeID
END


--drop foreign key from tProspectGovernmentIdDetails referring tNexxoIdTypes
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tProspectGovernmentIdDetails_tNexxoIdTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[tProspectGovernmentIdDetails]'))
BEGIN
ALTER TABLE dbo.tProspectGovernmentIdDetails DROP CONSTRAINT FK_tProspectGovernmentIdDetails_tNexxoIdTypes
END

--drop foreign key from tChannelPartnerMasterCountryMapping referring tMasterCountries
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MasterCountry_tChannelPartnerMasterCountryMapping]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerMasterCountryMapping]'))
BEGIN
ALTER TABLE dbo.tChannelPartnerMasterCountryMapping DROP CONSTRAINT FK_MasterCountry_tChannelPartnerMasterCountryMapping
END

--drop foreign key from tStates referring tCountries
DECLARE @FKName VARCHAR(500);
SELECT @FKName = f.name
FROM SYS.FOREIGN_KEYS AS f
     INNER JOIN SYS.FOREIGN_KEY_COLUMNS AS fc ON f.OBJECT_ID = fc.CONSTRAINT_OBJECT_ID
     INNER JOIN SYS.OBJECTS AS o ON o.OBJECT_ID = fc.REFERENCED_OBJECT_ID
WHERE OBJECT_NAME(f.PARENT_OBJECT_ID) = 'tStates'
      AND COL_NAME(fc.PARENT_OBJECT_ID, fc.parent_column_id) = 'CountryPK'
      AND OBJECT_NAME(f.REFERENCED_OBJECT_ID) = 'tCountries'
      AND COL_NAME(fc.REFERENCED_OBJECT_ID, fc.REFERENCED_COLUMN_ID) = 'CountryPK';

IF(@FKName IS NOT NULL)
BEGIN
	 EXEC ('ALTER TABLE tStates DROP CONSTRAINT '+@FKName);
END;
------------------------------------------------DROPING PK'S FROM Tables-------------------------------------------------------------

--drop Pk from tNexxoIdTypes
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tNexxoIdTypes' AND OBJECT_NAME(OBJECT_ID) = 'PK_tNexxoIdTypes')
BEGIN
ALTER TABLE dbo.tNexxoIdTypes DROP CONSTRAINT PK_tNexxoIdTypes
END

--Getting Pk from tMasterCountries
DECLARE @pkName VARCHAR(50);
SELECT @pkName = name
FROM SYS.KEY_CONSTRAINTS
WHERE type = 'PK'
      AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tMasterCountries';

--drop Pk from tMasterCountries
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = 'tMasterCountries' AND CONSTRAINT_NAME = @pkName AND COLUMN_NAME = 'MasterCountriesPK')
BEGIN
	 EXEC ('ALTER TABLE tMasterCountries DROP CONSTRAINT '+@pkName);
END 

--drop Pk from tStates
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tStates' AND OBJECT_NAME(OBJECT_ID) = 'PK_tStates')
BEGIN
ALTER TABLE dbo.tStates DROP CONSTRAINT PK_tStates
END

----------------------------------ALTERING THE REQUIRED TABLES------------------------------------------------------------

--add columns in tNexxoIdTypes table
-----------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'tNexxoIdTypes' AND COLUMN_NAME = 'StateId')
BEGIN
ALTER TABLE dbo.tNexxoIdTypes ADD StateId BIGINT NULL
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'tNexxoIdTypes' AND COLUMN_NAME = 'MasterCountriesID')
BEGIN
ALTER TABLE dbo.tNexxoIdTypes ADD MasterCountriesID BIGINT NULL
END
-----------------------------------------------------------------------------

--add column in tStates table
-----------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'tStates' AND COLUMN_NAME = 'StateId')
BEGIN
ALTER TABLE dbo.tStates ADD StateId bigint NOT NULL IDENTITY(100000000,1)
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'tStates' AND COLUMN_NAME = 'MasterCountriesID')
BEGIN
ALTER TABLE dbo.tStates ADD MasterCountriesID bigint NULL
END
-----------------------------------------------------------------------------
--Making the NexxoIdTypeID column not nullable
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'tNexxoIdTypes' AND COLUMN_NAME = 'NexxoIdTypeID')
BEGIN
ALTER TABLE tNexxoIdTypes ALTER COLUMN NexxoIdTypeID BIGINT NOT NULL
END

--Adding the masterCountriesId to tChannelPartnerMasterCountryMapping
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'MasterCountriesId')
BEGIN
ALTER TABLE dbo.tChannelPartnerMasterCountryMapping ADD MasterCountriesId BIGINT NULL
END

------------------------------------------ADDING PK'S FOR THE TABLES-------------------------------------------------------
--add primary key constraint in tMasterCountries 
IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tMasterCountries' AND OBJECT_NAME(OBJECT_ID) = 'PK_tMasterCountries')
BEGIN
ALTER TABLE dbo.tMasterCountries ADD CONSTRAINT PK_tMasterCountries PRIMARY KEY CLUSTERED (MasterCountriesID)
END

--add primary key constraint in tStates 
IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tStates' AND OBJECT_NAME(OBJECT_ID) = 'PK_tStates')
BEGIN
ALTER TABLE dbo.tStates ADD CONSTRAINT PK_tStates PRIMARY KEY CLUSTERED (StateId)
END

--add primary key constraint in tNexxoIdTypes 
IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tNexxoIdTypes' AND OBJECT_NAME(OBJECT_ID) = 'PK_tNexxoIdTypes')
BEGIN
ALTER TABLE dbo.tNexxoIdTypes ADD CONSTRAINT PK_tNexxoIdTypes PRIMARY KEY CLUSTERED (NexxoIdTypeID)
END
-------------------------------------ADDING FK'S FOR THE TABLES----------------------------------------------------------------
--add foreign key constraints for tStates
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tStates_tMasterCountries]') AND parent_object_id = OBJECT_ID(N'[dbo].[tStates]'))
BEGIN
ALTER TABLE [dbo].[tStates]  WITH CHECK ADD  CONSTRAINT [FK_tStates_tMasterCountries] FOREIGN KEY(MasterCountriesID)
REFERENCES [dbo].[tMasterCountries] (MasterCountriesID)
END

--add foreign key constraints for tNexxoIdTypes
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tNexxoIdTypes_tMasterCountries]') AND parent_object_id = OBJECT_ID(N'[dbo].[tNexxoIdTypes]'))
BEGIN
ALTER TABLE [dbo].[tNexxoIdTypes]  WITH CHECK ADD  CONSTRAINT [FK_tNexxoIdTypes_tMasterCountries] FOREIGN KEY(MasterCountriesID)
REFERENCES [dbo].[tMasterCountries] (MasterCountriesID)
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tNexxoIdTypes_tStates]') AND parent_object_id = OBJECT_ID(N'[dbo].[tNexxoIdTypes]'))
BEGIN
ALTER TABLE [dbo].[tNexxoIdTypes]  WITH CHECK ADD  CONSTRAINT [FK_tNexxoIdTypes_tStates] FOREIGN KEY(StateId)
REFERENCES [dbo].[tStates] (StateId)
END

--add foreign key constraints for tChannelPartnerMasterCountryMapping
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MasterCountries_tChannelPartnerMasterCountryMapping]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartnerMasterCountryMapping]'))
BEGIN
ALTER TABLE [dbo].tChannelPartnerMasterCountryMapping  WITH CHECK ADD  CONSTRAINT FK_MasterCountries_tChannelPartnerMasterCountryMapping FOREIGN KEY(MasterCountriesID)
REFERENCES [dbo].[tMasterCountries] (MasterCountriesID)
END

