--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update vComplianceTransactions view>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  View [dbo].[vComplianceTransactions]    Script Date: 3/27/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP VIEW [dbo].[vComplianceTransactions]
GO
CREATE VIEW [dbo].[vComplianceTransactions]
AS
SELECT	tBP.TxnPK rowguid, tBP.TransactionID AS TransactionId, tPC.CustomerID CustomerId, tBP.DTCreate, 4 AS TransactionType,
		tBP.Amount, tBP.Fee, tBP.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, NULL AS xRecipientId, NULL AS xRate, 
		tBP.ProductId AS bpProductId, tBP.AccountNumber AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_BillPay tBP
		INNER JOIN tAccounts tA ON tBP.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tBP.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tBP.TxnPK=scT.TxnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartPK=sc.CartPK

UNION ALL

SELECT	tC.txnPK rowguid, tC.TransactionID AS TransactionId, tPC.CustomerID CustomerId, tC.DTCreate, 2 AS TransactionType,
		tC.Amount, tC.Fee, tC.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Check tC
		INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tC.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tC.TxnPK=scT.TxnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartPK=sc.CartPK

UNION ALL

SELECT	tMO.txnPK rowguid, tMO.TransactionID AS TransactionId, tPC.CustomerID CustomerId, tMO.DTCreate, 5 AS TransactionType,
		tMO.Amount, tMO.Fee, tMO.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyOrder tMO
		INNER JOIN tAccounts tA ON tMO.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tMO.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMO.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK

UNION ALL

SELECT	tF.txnPK rowguid, tF.TransactionID AS TransactionId, tPC.CustomerID CustomerId, tF.DTCreate, 
		CASE tF.FundType WHEN 0 THEN 3 
						 WHEN 1 THEN 8 
						 ELSE 9 END AS TransactionType,
		tF.Amount, tF.Fee, tF.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Funds tF
		INNER JOIN tAccounts tA ON tF.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tF.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tF.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK

--UNION ALL

--SELECT	tCSH.txnRowguid rowguid, tCSH.Id AS TransactionId, tPC.Id CustomerId, tCSH.DTCreate, 
--		CASE tCSH.CashType WHEN 1 THEN 1
--							ELSE 7 END AS TransactionType,
--		tCSH.Amount, tCSH.Fee, tCSH.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, null as xRecipientId, null as xRate, 
--		null AS bpProductId, null AS bpAccountNumber,sc.Id AS ShoppingCartId
--FROM	tTxn_Cash tCSH
--		INNER JOIN tAccounts tA ON tCSH.AccountPK = tA.rowguid
--		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
--		LEFT OUTER JOIN tLedgerEntries le ON tCSH.LedgerEntryPK=le.rowguid
--		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid
--		LEFT OUTER JOIN tShoppingCartTransactions scT on tCSH.txnRowguid=scT.txnRowguid
--		LEFT OUTER JOIN tShoppingCarts sc on scT.cartRowguid=sc.cartRowguid


UNION ALL

SELECT	tMT.txnPK rowguid, tMT.TransactionID AS TransactionId, tPC.CustomerID CustomerId, tMT.DTCreate, 6 AS TransactionType,
		tMT.Amount, tMT.Fee, tMT.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, tMT.RecipientId AS xRecipientId, 
		tMT.ExchangeRate AS xRate, NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyTransfer tMT
		INNER JOIN tAccounts tA ON tMT.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tMT.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMT.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK
GO


