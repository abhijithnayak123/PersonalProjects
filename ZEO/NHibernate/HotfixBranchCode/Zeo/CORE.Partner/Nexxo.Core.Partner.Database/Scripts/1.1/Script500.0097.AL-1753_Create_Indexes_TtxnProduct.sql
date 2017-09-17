/****** Object:  Index [IX_tMoneyTransferTransaction_ID]    Script Date: 09/14/2015 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tMoneyTransfer_TransactionID' AND object_id = OBJECT_ID('tTxn_MoneyOrder'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tMoneyTransfer_TransactionID] ON tTxn_MoneyOrder
(
	[TransactionID] ASC
)
INCLUDE ( [CustomerSessionPK],
[AccountPK],
[DTServerCreate],
[DTServerLastModified])
END
GO

/****** Object:  Index [IX_tAccount_ID]  ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tAccount_ID' AND object_id = OBJECT_ID('tAccounts'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tAccount_ID] ON [dbo].[tAccounts] 
(
	[AccountID] ASC
)
INCLUDE ( [CustomerPK],
[DTServerCreate],
[DTServerLastModified])
END
GO


IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_TxnPK' AND object_id = OBJECT_ID('tShoppingCartTransactions'))
BEGIN
/****** Object:  Index [IX_TxnPK]  ******/
CREATE NONCLUSTERED INDEX [IX_TxnPK] ON tShoppingCartTransactions 
(
	[TxnPK] ASC
)
INCLUDE ( [CartPK])
END
GO

/****** Object:  Index [IX_tTxnBillPay_TransactionID]  ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tTxnBillPay_TransactionID' AND object_id = OBJECT_ID('tTxn_BillPay'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tTxnBillPay_TransactionID] ON tTxn_BillPay
(
	[TransactionID] ASC
)
INCLUDE ( [CustomerSessionPK],
[AccountPK],
[DTServerCreate],
[DTServerLastModified])
END
GO

/****** Object:  Index [IX_tTxnCheck_TransactionID]  ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tTxnBillPay_TransactionID' AND object_id = OBJECT_ID('tTxn_Check'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tTxnBillPay_TransactionID] ON tTxn_Check
(
	[TransactionID] ASC
)
INCLUDE ( [CustomerSessionPK],
[AccountPK],
[DTServerCreate],
[DTServerLastModified])
END
GO

/****** Object:  Index [IX_tTxnFunds_TransactionID]  ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tTxnFunds_TransactionID' AND object_id = OBJECT_ID('tTxn_Funds'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tTxnFunds_TransactionID] ON tTxn_Funds
(
	[TransactionID] ASC
)
INCLUDE ( [CustomerSessionPK],
[AccountPK],
[DTServerCreate],
[DTServerLastModified])
END
GO

/****** Object:  Index [IX_tTxnMoneyTransfer_TransactionID]  ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tTxnMoneyTransfer_TransactionID' AND object_id = OBJECT_ID('tTxn_MoneyTransfer'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tTxnMoneyTransfer_TransactionID] ON tTxn_MoneyTransfer
(
	[TransactionID] ASC
)
INCLUDE ( [CustomerSessionPK],
[AccountPK],
[DTServerCreate],
[DTServerLastModified])
END
GO
