--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <05-21-2017>
-- Description:	Create the non clustered index for required columns
-- Jira ID:		<>
-- ================================================================================



--==========================tShoppingCarts=============================

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tShoppingCarts' AND Name = 'IX_tShoppingCarts_CartID')
BEGIN
  DROP INDEX IX_tShoppingCarts_CartID ON tShoppingCarts
END
GO

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tShoppingCarts' AND Name = 'ix_tShoppingCartS_CustomerPK')
BEGIN
  DROP INDEX ix_tShoppingCartS_CustomerPK ON tShoppingCarts
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tShoppingCarts' AND Name = 'IX_tShoppingCarts_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tShoppingCarts_CustomerSessionId ON tShoppingCarts
    (
		CustomerSessionId
    )
	include
	(
		Status,
		IsReferral
	)
END
GO


--==========================tShoppingCartTransactions=============================


IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tShoppingCartTransactions' AND Name = 'Ix_tShoppingCartTransactions_CartPk')
BEGIN
  DROP INDEX Ix_tShoppingCartTransactions_CartPk ON tShoppingCartTransactions
END
GO

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tShoppingCartTransactions' AND Name = 'IX_TxnPK')
BEGIN
  DROP INDEX IX_TxnPK ON tShoppingCartTransactions
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tShoppingCartTransactions' AND Name = 'IX_tShoppingCartTransactions_TransactionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tShoppingCartTransactions_TransactionId ON tShoppingCartTransactions
    (
    	CartId,		
		ProductId,
		CartItemStatus,
		TransactionId
    )
END
GO


--==========================tTCIS_Account=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTCIS_Account' AND Name = 'IX_tTCIS_Account_CustomerId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTCIS_Account_CustomerId ON tTCIS_Account
    (
		CustomerID
    )
END
GO


IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTCIS_Account' AND Name = 'IX_tTCIS_Account_CustomerSessionID')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTCIS_Account_CustomerSessionID ON tTCIS_Account
    (
		CustomerID,
		CustomerSessionID
    )
END
GO


--==========================tTxn_BillPay=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_BillPay' AND Name = 'IX_tTxn_BillPay_CXNId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_BillPay_CXNId ON tTxn_BillPay
    (
		CXNId
    )
END
GO


IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_BillPay' AND Name = 'IX_tTxn_BillPay_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_BillPay_CustomerSessionId ON tTxn_BillPay
    (
		CustomerSessionId
    )
END
GO


--==========================tTxn_Cash=============================


----IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_Cash' AND Name = 'IX_tTxn_Cash_CXNId')
----BEGIN
----    CREATE NONCLUSTERED INDEX IX_tTxn_Cash_CXNId ON tTxn_Cash
----    (
----		CXNId
----    )
----END
----GO


IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_Cash' AND Name = 'IX_tTxn_Cash_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_Cash_CustomerSessionId ON tTxn_Cash
    (
		CustomerSessionId
    )
END
GO


--==========================tTxn_Check=============================


IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_Check' AND Name = 'IX_tTxn_Check_CXNId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_Check_CXNId ON tTxn_Check
    (
		CXNId
    )
END
GO


IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_Check' AND Name = 'IX_tTxn_Check_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_Check_CustomerSessionId ON tTxn_Check
    (
		CustomerSessionId
    )
END
GO

--==========================tTxn_Funds=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_Funds' AND Name = 'IX_tTxn_Funds_CXNId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_Funds_CXNId ON tTxn_Funds
    (
		CXNId
    )
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_Funds' AND Name = 'IX_tTxn_Funds_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_Funds_CustomerSessionId ON tTxn_Funds
    (
		CustomerSessionId
    )
	INCLUDE
	(
	ProviderAccountId,
	ProviderId
	)
END
GO


--==========================tTxn_MoneyOrder=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_MoneyOrder' AND Name = 'IX_tTxn_MoneyOrder_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_MoneyOrder_CustomerSessionId ON tTxn_MoneyOrder
    (
		CustomerSessionId
    )
END
GO


--==========================tTxn_MoneyTransfer=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_MoneyTransfer' AND Name = 'IX_tTxn_MoneyTransfer_CXNId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_MoneyTransfer_CXNId ON tTxn_MoneyTransfer
    (
		CXNId
    )
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_MoneyTransfer' AND Name = 'IX_tTxn_MoneyTransfer_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tTxn_MoneyTransfer_CustomerSessionId ON tTxn_MoneyTransfer
    (
		CustomerSessionId
    )
END
GO

--==========================tVisa_Account=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tVisa_Account' AND Name = 'IX_tVisa_Account_CardAliasId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tVisa_Account_CardAliasId ON tVisa_Account
    (
		CardAliasId
    )
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tVisa_Account' AND Name = 'IX_tVisa_Account_CardNumber')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tVisa_Account_CardNumber ON tVisa_Account
    (
		CardNumber
    )
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tVisa_Account' AND Name = 'IX_tVisa_Account_CustomerId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tVisa_Account_CustomerId ON tVisa_Account
    (
		CustomerId
    )
END
GO

--==========================tWUnion_Account=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tWUnion_Account' AND Name = 'IX_tWUnion_Account_CustomerId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tWUnion_Account_CustomerId ON tWUnion_Account
    (
		CustomerId
    )
END
GO

--==========================tWUnion_BillPay_Account=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tWUnion_BillPay_Account' AND Name = 'IX_tWUnion_BillPay_Account_CustomerId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tWUnion_BillPay_Account_CustomerId ON tWUnion_BillPay_Account
    (
		CustomerId
    )
END
GO

--==========================tWUnion_BillPay_Trx=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tWUnion_BillPay_Trx' AND Name = 'IX_tWUnion_BillPay_Trx_WUBillPayAccountId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tWUnion_BillPay_Trx_WUBillPayAccountId ON tWUnion_BillPay_Trx
    (
		WUBillPayAccountId
    )
END
GO

--==========================tWUnion_Trx=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tWUnion_Trx' AND Name = 'IX_tWUnion_Trx_Mtcn')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tWUnion_Trx_Mtcn ON tWUnion_Trx
    (
		Mtcn
    )
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tWUnion_Trx' AND Name = 'IX_tWUnion_Trx_WUAccountID')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tWUnion_Trx_WUAccountID ON tWUnion_Trx
    (
		WUAccountID
    )
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tWUnion_Trx' AND Name = 'IX_tWUnion_Trx_WUReceiverID')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tWUnion_Trx_WUReceiverID ON tWUnion_Trx
    (
		WUReceiverID
    )
END
GO


IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tWUnion_Trx' AND Name = 'IX_tWUnion_Trx_WUTrxID')
BEGIN
  DROP INDEX IX_tWUnion_Trx_WUTrxID ON tWUnion_Trx
END
GO


--==========================[dbo].[tCheckImages]=============================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tCheckImages' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCheckImages')
BEGIN
	ALTER TABLE [dbo].[tCheckImages] DROP CONSTRAINT PK_tCheckImages
END

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tCheckImages' AND OBJECT_NAME(OBJECT_ID) = 'PK_CheckImages')
BEGIN
	ALTER TABLE [dbo].[tCheckImages] ADD CONSTRAINT PK_CheckImages PRIMARY KEY CLUSTERED (TransactionId)
END


--==========================[dbo].[tChxr_Account]=============================

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tChxr_Trx_tChxr_Account') AND PARENT_OBJECT_ID = OBJECT_ID(N'tChxr_Trx'))
BEGIN
    ALTER TABLE [dbo].[tChxr_Trx] DROP CONSTRAINT FK_tChxr_Trx_tChxr_Account
END
GO 

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tChxr_Account' AND Name = 'IX_tChxr_Account_Id')
BEGIN
    DROP INDEX [IX_tChxr_Account_Id] ON [dbo].[tChxr_Account]
END
GO 

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Trx_tChxr_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]'))
BEGIN
    ALTER TABLE [dbo].[tChxr_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Trx_tChxr_Account] FOREIGN KEY([ChxrAccountId])
	REFERENCES [dbo].[tChxr_Account] ([ChxrAccountId])
END
GO

IF NOT EXISTS (SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tChxr_Account' AND Name = 'XI_tChxr_Account_CustomerId')
BEGIN
    CREATE NONCLUSTERED INDEX XI_tChxr_Account_CustomerId ON [dbo].[tChxr_Account]
    (
    	CustomerId 
    )
END


IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tChxr_Account' AND Name = 'IX_tChxr_Account_CustomerSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tChxr_Account_CustomerSessionId ON [dbo].[tChxr_Account]
    (
    	CustomerSessionId 
    )
END
GO 

--==========================[dbo].[tChxr_Trx]=============================
 
IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tChxr_Trx' AND Name = 'IX_tChxr_Trx_ChxrAccountId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tChxr_Trx_ChxrAccountId ON [dbo].[tChxr_Trx]
    (
    	ChxrAccountId 
    )
END
GO 


--==========================[dbo].[tCustomerSessionCounterIdDetails]=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tCustomerSessionCounterIdDetails' AND Name = 'IX_tCustomerSessionCounterIdDetails_CustomersessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tCustomerSessionCounterIdDetails_CustomersessionId ON [dbo].[tCustomerSessionCounterIdDetails]
    (
    	CustomerSessionID 
    )
END
GO 

--==========================[dbo].[tCustomerSessions]=============================

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tCustomerSessions' AND Name = 'IX_tCustomerSessions_customerPK')
BEGIN
    DROP INDEX [IX_tCustomerSessions_customerPK] ON [dbo].[tCustomerSessions]
END
GO 

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tCustomerSessions' AND Name = 'IX_tCustomerSessions_ID')
BEGIN
    DROP INDEX [IX_tCustomerSessions_ID] ON [dbo].[tCustomerSessions]
END
GO 

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tCustomerSessions' AND Name = 'IX_tCustomerSessions_AgentSessionId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tCustomerSessions_AgentSessionId ON [dbo].[tCustomerSessions]
    (
    	[AgentSessionId] 
    )
END
GO 

--==========================[dbo].[tMasterCatalog]=============================

IF  EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tPartnerCatalog_tMasterCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[tPartnerCatalog]'))
BEGIN
    ALTER TABLE [dbo].[tPartnerCatalog] DROP CONSTRAINT FK_tPartnerCatalog_tMasterCatalog
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='UNIQUE'  AND CONSTRAINT_NAME = 'UX_MasterCatalogID' )
BEGIN
	 ALTER TABLE [dbo].[tMasterCatalog] DROP CONSTRAINT UX_MasterCatalogID
END 
GO

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tMasterCatalog' AND Name = 'UX_MasterCatalogID')
BEGIN
    DROP INDEX UX_MasterCatalogID ON [dbo].[tMasterCatalog]
END
GO 

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tPartnerCatalog_tMasterCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[tPartnerCatalog]'))
BEGIN
    ALTER TABLE [dbo].[tPartnerCatalog]  WITH CHECK ADD  CONSTRAINT [FK_tPartnerCatalog_tMasterCatalog] FOREIGN KEY(MasterCatalogID)
	REFERENCES [dbo].[tMasterCatalog] (MasterCatalogID)
END
GO
--==========================[dbo].[tPartnerCatalog]=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tPartnerCatalog' AND Name = 'IX_tPartnerCatalog_MasterCatalogId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tPartnerCatalog_MasterCatalogId ON [dbo].[tPartnerCatalog]
    (
    	[MasterCatalogId] 
    )
END
GO 

--==========================[dbo].[tLocationProcessorCredentials]=============================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tLocationProcessorCredentials' AND Name = 'IX_tLocationProcessorCredentials_ProviderId_LocationId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tLocationProcessorCredentials_ProviderId_LocationId ON [dbo].[tLocationProcessorCredentials]
    (
    	ProviderId,
		LocationId
    )
END
GO 

--==========================[dbo].[tCustomers]=============================

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tCustomerPreferedProducts_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tCustomerPreferedProducts'))
BEGIN
    ALTER TABLE [dbo].[tCustomerPreferedProducts] DROP CONSTRAINT FK_tCustomerPreferedProducts_tCustomers
END
GO 

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tWUBillPay_Acc_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tWUnion_BillPay_Account'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_BillPay_Account] DROP CONSTRAINT FK_tWUBillPay_Acc_tCustomers
END
GO

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tCustomer_tCustomerSessions') AND PARENT_OBJECT_ID = OBJECT_ID(N'tCustomerSessions'))
BEGIN
    ALTER TABLE [dbo].[tCustomerSessions] DROP CONSTRAINT FK_tCustomer_tCustomerSessions
END
GO 

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tVisa_Account_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tVisa_Account'))
BEGIN
    ALTER TABLE [dbo].[tVisa_Account] DROP CONSTRAINT FK_tVisa_Account_tCustomers
END
GO 

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tTCIS_Account_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tTCIS_Account'))
BEGIN
    ALTER TABLE [dbo].[tTCIS_Account] DROP CONSTRAINT FK_tTCIS_Account_tCustomers
END
GO 

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tCustomerFeeAdjustments_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tCustomerFeeAdjustments'))
BEGIN
    ALTER TABLE [dbo].[tCustomerFeeAdjustments] DROP CONSTRAINT FK_tCustomerFeeAdjustments_tCustomers
END
GO 

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tChxrSim_Account_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tChxrSim_Account'))
BEGIN
    ALTER TABLE [dbo].[tChxrSim_Account] DROP CONSTRAINT FK_tChxrSim_Account_tCustomers
END
GO 

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tChxr_Account_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tChxr_Account'))
BEGIN
    ALTER TABLE [dbo].[tChxr_Account] DROP CONSTRAINT FK_tChxr_Account_tCustomers
END
GO 

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tWUnion_Account_tCustomers') AND PARENT_OBJECT_ID = OBJECT_ID(N'tWUnion_Account'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_Account] DROP CONSTRAINT FK_tWUnion_Account_tCustomers
END
GO 

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='UNIQUE'  AND CONSTRAINT_NAME = 'UX_CustomerID' )
BEGIN
	 ALTER TABLE [dbo].[tCustomers] DROP CONSTRAINT UX_CustomerID
END 
GO

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tCustomers' AND Name = 'IX_tCustomers_CustomerID')
BEGIN
    DROP INDEX [IX_tCustomers_CustomerID] ON [dbo].[tCustomers]
END
GO 


IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomer_tCustomerSessions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomerSessions]'))
BEGIN
    ALTER TABLE [dbo].[tCustomerSessions]  WITH CHECK ADD  CONSTRAINT [FK_tCustomer_tCustomerSessions] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_Account]'))
BEGIN
    ALTER TABLE [dbo].[tVisa_Account]  WITH CHECK ADD  CONSTRAINT [FK_tVisa_Account_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTCIS_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTCIS_Account]'))
BEGIN
    ALTER TABLE [dbo].[tTCIS_Account]  WITH CHECK ADD  CONSTRAINT [FK_tTCIS_Account_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomerFeeAdjustments_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomerFeeAdjustments]'))
BEGIN
    ALTER TABLE [dbo].[tCustomerFeeAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerFeeAdjustments_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxrSim_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxrSim_Account]'))
BEGIN
    ALTER TABLE [dbo].[tChxrSim_Account]  WITH CHECK ADD  CONSTRAINT [FK_tChxrSim_Account_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChxr_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tChxr_Account]'))
BEGIN
    ALTER TABLE [dbo].[tChxr_Account]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Account_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Account_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Account]'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_Account]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Account_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomerPreferedProducts_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomerPreferedProducts]'))
BEGIN
    ALTER TABLE [dbo].[tCustomerPreferedProducts]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerPreferedProducts_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END
GO

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUBillPay_Acc_tCustomers]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Account]'))
BEGIN
    ALTER TABLE [dbo].[tWUnion_BillPay_Account]  WITH CHECK ADD  CONSTRAINT [FK_tWUBillPay_Acc_tCustomers] FOREIGN KEY(CustomerID)
	REFERENCES [dbo].[tCustomers] (CustomerID)
END 
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tAgentSessions' AND Name = 'IX_tAgentSessions_AgentId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tAgentSessions_AgentId ON tAgentSessions
    (
    	AgentId
    )
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tAgentSessions' AND Name = 'IX_tAgentSessions_TerminalId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_tAgentSessions_TerminalId ON tAgentSessions
    (
    	TerminalId
    )
END
GO