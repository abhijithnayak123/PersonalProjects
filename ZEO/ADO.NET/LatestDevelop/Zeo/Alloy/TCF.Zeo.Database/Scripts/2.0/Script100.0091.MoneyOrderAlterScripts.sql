--- ===============================================================================
-- Author:		<Nitish Biradar/Nishad Varghese>
-- Create date: <01-Oct-2016>
-- Description:	Alter PK and FK constraints, addition of table for MoneyOrder related tables
-- Jira ID:		<AL-7706>
-- ================================================================================


-- DROP FK constraints in MoneyOrder related tables

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_MoneyOrder_tAccounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyOrder]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_MoneyOrder] DROP CONSTRAINT [FK_tTxn_MoneyOrder_tAccounts]
END
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_MoneyOrder_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyOrder]'))
BEGIN
	ALTER TABLE [dbo].[tTxn_MoneyOrder] DROP CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions]
END
GO

DECLARE @FKName VARCHAR(500)
SELECT @FKName = f.name 
             FROM sys.foreign_keys AS f
             INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
             INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
             WHERE OBJECT_NAME(f.parent_object_id) = 'tMoneyOrderImage'
                    AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'TrxId'
                    AND OBJECT_NAME(f.referenced_object_id) = 'tTxn_MoneyOrder'
                    AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'TxnPK'
IF (@FKName != '')
BEGIN
   EXEC('ALTER TABLE  tMoneyOrderImage DROP CONSTRAINT '+ @FKName)     
END


--Drop PK constraints in MoneyOrder related tables
IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_MoneyOrder' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_MoneyOrder')
BEGIN
	ALTER TABLE [dbo].[tTxn_MoneyOrder] DROP CONSTRAINT [PK_tTxn_MoneyOrder]
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tMoneyOrderImage')
BEGIN
DECLARE @PKName VARCHAR(MAX)
SELECT @PKName=name FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tMoneyOrderImage'
EXEC('ALTER TABLE [dbo].[tMoneyOrderImage] DROP CONSTRAINT ' + @PKName)
END
GO

-- Adding column to maintain revision
IF NOT EXISTS (SELECT
                     COLUMN_NAME
               FROM
                     INFORMATION_SCHEMA.COLUMNS
               WHERE
                     TABLE_NAME = 'tTxn_MoneyOrder'
                     AND COLUMN_NAME = 'CustomerRevisionNo')
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD CustomerRevisionNo INT
END
GO


--Adding columns from tTxn_MoneyOrder_Stage since that table will be removed
IF NOT EXISTS (SELECT
                     COLUMN_NAME
               FROM
                     INFORMATION_SCHEMA.COLUMNS
               WHERE
                     TABLE_NAME = 'tTxn_MoneyOrder'
                     AND COLUMN_NAME = 'MICR')
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD MICR VARCHAR(500)
END
GO

IF NOT EXISTS (SELECT
                     COLUMN_NAME
               FROM
                     INFORMATION_SCHEMA.COLUMNS
               WHERE
                     TABLE_NAME = 'tTxn_MoneyOrder'
                     AND COLUMN_NAME = 'PurchaseDate')
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD PurchaseDate DATETIME
END
GO

--ID column inclusion since PK to be removed
IF NOT EXISTS (SELECT
                     COLUMN_NAME
               FROM
                     INFORMATION_SCHEMA.COLUMNS
               WHERE
                     TABLE_NAME = 'tTxn_MoneyOrder'
                     AND COLUMN_NAME = 'CustomerSessionId')
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD CustomerSessionId BIGINT NULL;
END
GO

--ID column inclusion since PK to be removed
IF NOT EXISTS (SELECT
                     COLUMN_NAME
               FROM
                     INFORMATION_SCHEMA.COLUMNS
               WHERE
                     TABLE_NAME = 'tMoneyOrderImage'
                     AND COLUMN_NAME = 'TransactionId')
BEGIN
	ALTER TABLE tMoneyOrderImage ADD TransactionId BIGINT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME IN ('CXEState'))
BEGIN
	 EXEC sp_RENAME 'tTxn_MoneyOrder.CXEState' , 'State' , 'COLUMN'	 
END
GO

--New table
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyOrder_Aud]') AND type in (N'U'))
BEGIN
CREATE TABLE tTxn_MoneyOrder_Aud
(
	TransactionId BIGINT NULL,
	CustomerSessionId BIGINT NULL,
	CustomerRevisionNo INT NULL,
	MICR VARCHAR(500) NULL,
	PurchaseDate DATETIME NULL,
	Amount MONEY NULL,
	Fee MONEY NULL,
	Description NVARCHAR(255) NULL,
	State INT NULL,
	BaseFee MONEY NULL,
	DiscountApplied MONEY NULL,
	AdditionalFee MONEY NULL,
	DiscountName VARCHAR(50) NULL,
	DiscountDescription VARCHAR(100) NULL,
	IsSystemApplied bit NULL,
	CheckNumber VARCHAR(50) NULL,
	AccountNumber VARCHAR(20) NULL,
	RoutingNumber VARCHAR(20) NULL,
	DTAudit DATETIME NOT NULL,
	RevisionNo INT NOT NULL,
	AuditEvent INT NOT NULL,
	DTTerminalCreate DATETIME NOT NULL,
	DTTerminalLastModified DATETIME NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL
)
END
GO



--Add PK
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tMoneyOrderImage' AND OBJECT_NAME(OBJECT_ID) = 'PK_tMoneyOrderImage')
BEGIN
	ALTER TABLE [dbo].[tMoneyOrderImage] ADD CONSTRAINT [PK_tMoneyOrderImage] PRIMARY KEY CLUSTERED (MoneyOrderImageID)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_MoneyOrder' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTrxn_MoneyOrder')
BEGIN
	ALTER TABLE [dbo].[tTxn_MoneyOrder] ADD CONSTRAINT [PK_tTrxn_MoneyOrder] PRIMARY KEY CLUSTERED (TransactionId)
END
GO


--Add FK
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tMoneyOrderImage_tTxn_MoneyOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[tMoneyOrderImage]'))
BEGIN
    ALTER TABLE [dbo].[tMoneyOrderImage]  WITH CHECK ADD  CONSTRAINT [FK_tMoneyOrderImage_tTxn_MoneyOrder] FOREIGN KEY(TransactionId)
	REFERENCES [dbo].[tTxn_MoneyOrder] ([TransactionId])
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_MoneyOrder_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyOrder]'))
BEGIN
    ALTER TABLE [dbo].[tTxn_MoneyOrder]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions] FOREIGN KEY(CustomerSessionID)
	REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])
END
GO

--Adding default values to the PK columns

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_MoneyOrder_txnPK' )
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD CONSTRAINT DF_tTxn_MoneyOrder_txnPK DEFAULT NEWID() FOR TxnPK
END
GO

IF NOT EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tMoneyOrderImage_MoneyOrderImagePK' )
BEGIN
	ALTER TABLE tMoneyOrderImage ADD CONSTRAINT DF_tMoneyOrderImage_MoneyOrderImagePK DEFAULT NEWID() FOR MoneyOrderImagePK
END
GO


--Making the columns nullable which we are not using presently in the new design

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'CXEId' )
BEGIN
	ALTER TABLE tTxn_MoneyOrder ALTER COLUMN CXEId BIGINT NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'AccountPK' )
BEGIN
	ALTER TABLE tTxn_MoneyOrder ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL;
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'CustomerSessionPK' )
BEGIN
	ALTER TABLE tTxn_MoneyOrder ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL;
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMoneyOrderImage' AND COLUMN_NAME = 'TrxId' )
BEGIN
	ALTER TABLE tMoneyOrderImage ALTER COLUMN TrxId UNIQUEIDENTIFIER NULL;
END
GO


