--===========================================================================================
-- Author: 
-- Created date: 
-- Description:	
-- Jira ID: 
--===========================================================================================
IF EXISTS(SELECT 1 FROM sys.views WHERE NAME = 'vComplianceTransactions')
	DROP VIEW vComplianceTransactions
GO

CREATE VIEW [dbo].[vComplianceTransactions]
AS
SELECT	tBP.TransactionID AS TransactionId, cs.CustomerID as CustomerId, tBP.DTTerminalCreate, 4 AS TransactionType,
		tBP.Amount, tBP.Fee, tBP.[State], tBP.ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		tBP.ProductId AS bpProductId, tBP.AccountNumber AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_BillPay tBP
		INNER JOIN tCustomerSessions cs ON tBP.CustomerSessionId = cs.CustomerSessionID
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tBP.TransactionID=scT.TransactionId
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartId=sc.CartID

UNION ALL
SELECT	tC.TransactionID AS TransactionId, cs.CustomerID as CustomerId, tC.DTTerminalCreate, 2 AS TransactionType,
		tC.Amount, tC.Fee, tC.[State], tC.ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Check tC
		INNER JOIN tCustomerSessions cs ON tC.CustomerSessionId = cs.CustomerSessionID
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tC.TransactionID = scT.TransactionId
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartId = sc.CartID

UNION ALL

SELECT	tMO.TransactionID AS TransactionId, cs.CustomerID as CustomerId, tMO.DTTerminalCreate, 5 AS TransactionType,
		tMO.Amount, tMO.Fee, tMO.[State], 504 as ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyOrder tMO
		INNER JOIN tCustomerSessions cs ON tMO.CustomerSessionId = cs.CustomerSessionID
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMO.TransactionID = scT.TransactionId
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartId = sc.CartID

UNION ALL

SELECT	tF.TransactionID AS TransactionId, cs.CustomerID as CustomerId, tF.DTTerminalCreate, 
		CASE tF.FundType WHEN 0 THEN 3 
						 WHEN 1 THEN 8 
						 ELSE 9 END AS TransactionType,
		tF.Amount, tF.Fee, tF.[State], tF.ProviderId, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Funds tF
		INNER JOIN tCustomerSessions cs ON tF.CustomerSessionId = cs.CustomerSessionID
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tF.TransactionID=scT.TransactionId
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartId=sc.CartID

UNION ALL

SELECT	tMT.TransactionID AS TransactionId, cs.CustomerID as CustomerId, tMT.DTTerminalCreate, 6 AS TransactionType,
		tMT.Amount, tMT.Fee, tMT.[State] AS State, tMT.ProviderId, tMT.RecipientId AS xRecipientId, 
		tMT.ExchangeRate AS xRate, NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyTransfer tMT
		INNER JOIN tCustomerSessions cs ON tMT.CustomerSessionId = cs.CustomerSessionID
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMT.TransactionID=scT.TransactionId
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartId=sc.CartID
GO