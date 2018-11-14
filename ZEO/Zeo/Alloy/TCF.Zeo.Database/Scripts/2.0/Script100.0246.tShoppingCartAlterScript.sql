-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Alter scripts for tShopping cart
-- Jira ID:		AL-8047
-- =================================================================================

--================== Dropt Foreign Key constraints in Shopping related tables ===============================================================

IF EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE OBJECT_ID = OBJECT_ID(N'FK_tShoppingCarts_tCustomer') AND PARENT_OBJECT_ID = OBJECT_ID(N'tShoppingCarts'))
BEGIN
	ALTER TABLE tShoppingCarts 
	DROP CONSTRAINT FK_tShoppingCarts_tCustomer
END
GO


IF EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE OBJECT_ID = OBJECT_ID(N'FK_tShoppingCartTransactions_tShoppingCart') AND PARENT_OBJECT_ID = OBJECT_ID(N'tShoppingCartTransactions'))
BEGIN
	ALTER TABLE tShoppingCartTransactions 
	DROP CONSTRAINT FK_tShoppingCartTransactions_tShoppingCart
END
GO


IF EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE OBJECT_ID = OBJECT_ID(N'FK_tTxn_Cash_tAccounts') AND PARENT_OBJECT_ID = OBJECT_ID(N'tTxn_Cash'))
BEGIN
	ALTER TABLE tTxn_Cash 
	DROP CONSTRAINT FK_tTxn_Cash_tAccounts
END
GO

--======================Drop the index on the tTxn_Cash Table======================================================================

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tTxnCash_TransactionID' AND object_id = OBJECT_ID('tTxn_Cash'))
BEGIN
	DROP INDEX tTxn_Cash.IX_tTxnCash_TransactionID
END


--- ====================DROP PK constraints in Shopping cart related tables=========================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE_DESC = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tShoppingCarts' AND OBJECT_NAME(OBJECT_ID) = 'PK_tShoppingCarts')
BEGIN
	ALTER TABLE tShoppingCarts 
	DROP CONSTRAINT PK_tShoppingCarts
END
GO

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE_DESC = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tShoppingCartTransactions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tShoppingCartTransactions_cartTxnRowguid')
BEGIN
	ALTER TABLE tShoppingCartTransactions 
	DROP CONSTRAINT PK_tShoppingCartTransactions_cartTxnRowguid
END
GO


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE_DESC = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tTxn_Cash' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_Cash')
BEGIN
	ALTER TABLE tTxn_Cash 
	DROP CONSTRAINT PK_tTxn_Cash
END
GO
--===================Dropping the existing shopping cart trigger================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE Name = 'trShoppingCartTransactionsAudit')
BEGIN
	DROP TRIGGER trShoppingCartTransactionsAudit
END
GO

--=================== Add new columns to tShopping cart related tables =========================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'CustomerSessionId')
BEGIN
	ALTER TABLE tShoppingCarts
	ADD CustomerSessionId BIGINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'State')
BEGIN
	ALTER TABLE tShoppingCarts
	ADD State INT NULL
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME = 'CustomerSessionId')
BEGIN
	ALTER TABLE tShoppingCarts_Aud
	ADD CustomerSessionId BIGINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME = 'State')
BEGIN
	ALTER TABLE tShoppingCarts_Aud
	ADD State INT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'CartId')
BEGIN
	ALTER TABLE tShoppingCartTransactions
	ADD CartId BIGINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'TransactionId')
BEGIN
	ALTER TABLE tShoppingCartTransactions
	ADD TransactionId BIGINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'ShoppingCartTransactionId')
BEGIN
	ALTER TABLE tShoppingCartTransactions
	ADD ShoppingCartTransactionId BIGINT NOT NULL IDENTITY(1000000000, 1)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'ProductId')
BEGIN
	ALTER TABLE tShoppingCartTransactions
	ADD ProductId INT NULL
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME = 'CartId')
BEGIN
	ALTER TABLE tShoppingCartTransactions_Aud
	ADD CartId BIGINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME = 'TransactionId')
BEGIN
	ALTER TABLE tShoppingCartTransactions_Aud
	ADD TransactionId BIGINT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME = 'ShoppingCartTransactionId')
BEGIN
	ALTER TABLE tShoppingCartTransactions_Aud
	ADD ShoppingCartTransactionId BIGINT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME = 'ProductId')
BEGIN
	ALTER TABLE tShoppingCartTransactions_Aud
	ADD ProductId INT NULL
END
GO



---===========================Add New columns into  tTxn_Cash table==================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'CustomerSessionId')
BEGIN
	ALTER TABLE tTxn_Cash
	ADD CustomerSessionId BIGINT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME IN ('TxnPK', 'CXEId', 'AccountPK', 'CustomerSessionPK'))
BEGIN

	ALTER TABLE tTxn_Cash
	ALTER COLUMN TxnPK UNIQUEIDENTIFIER NULL

	ALTER TABLE tTxn_Cash
	ALTER COLUMN CXEId BIGINT NULL

	ALTER TABLE tTxn_Cash
	ALTER COLUMN CustomerSessionPK UNIQUEIDENTIFIER NULL

	ALTER TABLE tTxn_Cash
	ALTER COLUMN AccountPK UNIQUEIDENTIFIER NULL

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME IN ('CXEState'))
BEGIN
	 EXEC sp_RENAME 'tTxn_Cash.CXEState' , 'State' , 'COLUMN'	 
END
GO



--================= Add Default constraint for PK columns =====================================

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'D' AND NAME = 'DF_tShoppingCarts_CartPK')
BEGIN
	ALTER TABLE tShoppingCarts ADD CONSTRAINT DF_tShoppingCarts_CartPK
	DEFAULT NEWID() FOR CartPK
END
GO



-- === alter columns as nullable to tShoppingCarts table =======================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME ='Active' )
BEGIN

	ALTER TABLE tShoppingCarts
	ALTER COLUMN Active BIT NULL

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME ='Active' )
BEGIN

	ALTER TABLE tShoppingCarts_Aud
	ALTER COLUMN Active BIT NULL

END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME ='CartPK' )
BEGIN

	ALTER TABLE tShoppingCartTransactions
	ALTER COLUMN CartPK UNIQUEIDENTIFIER NULL

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME ='TxnPK' )
BEGIN

	ALTER TABLE tShoppingCartTransactions
	ALTER COLUMN TxnPK UNIQUEIDENTIFIER NULL

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME ='CartPK' )
BEGIN

	ALTER TABLE tShoppingCartTransactions_Aud
	ALTER COLUMN CartPK UNIQUEIDENTIFIER NULL

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME ='TxnPK' )
BEGIN

	ALTER TABLE tShoppingCartTransactions_Aud
	ALTER COLUMN TxnPK UNIQUEIDENTIFIER NULL

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME ='CartPK' )
BEGIN

	ALTER TABLE tShoppingCarts_Aud
	ALTER COLUMN CartPK UNIQUEIDENTIFIER NULL

END
GO

--- =================== Add PK Constraints to Shopping cart related tables ========================================

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE_DESC = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tShoppingCarts' AND OBJECT_NAME(OBJECT_ID) = 'PK_tShoppingCarts')
BEGIN
	ALTER TABLE tShoppingCarts ADD CONSTRAINT PK_tShoppingCarts PRIMARY KEY CLUSTERED (CartId)
END
GO

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE_DESC = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tShoppingCartTransactions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tShoppingCartTransactions')
BEGIN
	ALTER TABLE tShoppingCartTransactions ADD CONSTRAINT PK_tShoppingCartTransactions PRIMARY KEY CLUSTERED (ShoppingCartTransactionId)
END
GO

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE_DESC = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tTxn_Cash' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_Cash')
BEGIN
	ALTER TABLE tTxn_Cash ADD CONSTRAINT PK_tTxn_Cash PRIMARY KEY CLUSTERED (TransactionId)
END
GO



---- =================== Add FK constraints to Shopping cart related tables =======================================

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Cash_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Cash]'))
BEGIN
	ALTER TABLE tTxn_Cash WITH CHECK ADD CONSTRAINT FK_tTxn_Cash_tCustomerSessions FOREIGN KEY (CustomerSessionId)
	REFERENCES tCustomerSessions (CustomerSessionId)
END
GO

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tShoppingCarts_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tShoppingCarts]'))
BEGIN
	ALTER TABLE tShoppingCarts WITH CHECK ADD CONSTRAINT FK_tShoppingCarts_tCustomerSessions FOREIGN KEY (CustomerSessionId)
	REFERENCES tCustomerSessions (CustomerSessionId)
END
GO

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tShoppingCartTransactions_tShoppingCart]') AND parent_object_id = OBJECT_ID(N'[dbo].[tShoppingCartTransactions]'))
BEGIN
	ALTER TABLE tShoppingCartTransactions WITH CHECK ADD CONSTRAINT FK_tShoppingCartTransactions_tShoppingCart FOREIGN KEY (CartId)
	REFERENCES tShoppingCarts (CartId)
END
GO



-- ==================== Create the Audit table for tTxn_Cash table =================================================================

IF NOT EXISTS(
	SELECT 1
	FROM SYS.TABLES 
	WHERE NAME = 'tTxn_Cash_Aud'
)
BEGIN

   CREATE TABLE tTxn_Cash_Aud
				(
				TransactionID           BIGINT NOT NULL,
				Amount                  MONEY NULL,
				Fee                     MONEY NULL,
				Description             NVARCHAR(255) NULL,
				DTTerminalCreate        DATETIME NOT NULL,
				DTTerminalLastModified  DATETIME NULL,
				ConfirmationNumber      VARCHAR(50) NULL,
				CashType                INT NULL,
				DTServerCreate          DATETIME NULL,
				DTServerLastModified    DATETIME NULL,
				CustomerSessionId       BIGINT NULL,
				RevisionNo              BIGINT NOT NULL,
				AuditEvent              SMALLINT NOT NULL,
				DTAudit                 DATETIME NOT NULL
				)

END
GO