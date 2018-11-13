--===========================================================================================
-- Author:	<Rogy Eapen>
-- Created date: <July 02 2015>
-- Description:	<Script to remove tLedgerEntries and tLedgerTransactions table>           
-- Jira ID:	<AL-639>
--===========================================================================================

ALTER VIEW [dbo].[vComplianceTransactions]
AS
SELECT	tBP.TxnPK rowguid, tBP.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tBP.DTTerminalCreate, 4 AS TransactionType,
		tBP.Amount, tBP.Fee, tBP.CXEState AS [State], tA.ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		tBP.ProductId AS bpProductId, tBP.AccountNumber AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_BillPay tBP
		INNER JOIN tAccounts tA ON tBP.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 		
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tBP.TxnPK=scT.TxnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartPK=sc.CartPK

UNION ALL
SELECT	tC.txnPK rowguid, tC.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tC.DTTerminalCreate, 2 AS TransactionType,
		tC.Amount, tC.Fee, tC.CXEState AS [State], tA.ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Check tC
		INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 		
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tC.TxnPK=scT.TxnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartPK=sc.CartPK

UNION ALL

SELECT	tMO.txnPK rowguid, tMO.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tMO.DTTerminalCreate, 5 AS TransactionType,
		tMO.Amount, tMO.Fee, tMO.CXEState AS [State], tA.ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyOrder tMO
		INNER JOIN tAccounts tA ON tMO.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 		
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMO.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK

UNION ALL

SELECT	tF.txnPK rowguid, tF.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tF.DTTerminalCreate, 
		CASE tF.FundType WHEN 0 THEN 3 
						 WHEN 1 THEN 8 
						 ELSE 9 END AS TransactionType,
		tF.Amount, tF.Fee, tF.CXEState AS [State], tA.ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Funds tF
		INNER JOIN tAccounts tA ON tF.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tF.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK

UNION ALL

SELECT	tMT.txnPK rowguid, tMT.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tMT.DTTerminalCreate, 6 AS TransactionType,
		tMT.Amount, tMT.Fee, tMT.CXEState AS [State], tA.ProviderId, tMT.RecipientId AS xRecipientId, 
		tMT.ExchangeRate AS xRate, NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyTransfer tMT
		INNER JOIN tAccounts tA ON tMT.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMT.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK
GO