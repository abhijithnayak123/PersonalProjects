-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- Drop and recreate vCashDrawerReport
DROP VIEW [dbo].[vCashDrawerReport]
GO

CREATE VIEW [dbo].[vCashDrawerReport]
AS
SELECT	NEWID() AS rowguid, tAS.AgentId AgentId, tCA.CashType CashType, 
		tL.LocationID LocationId, tL.LocationName Location, SUM(Amount) Amount
FROM	tTxn_Cash tCA
		INNER JOIN tAccounts tA ON tCA.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tCA.CustomerSessionPK
		INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK
		INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK
		INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK
WHERE	tCA.DTTerminalCreate >= DATEADD(dd, DATEDIFF(dd, 0, GETDATE()) - 0, 0)
		AND tCA.DTTerminalCreate < DATEADD(dd, DATEDIFF(dd, 0, GETDATE())+1, 0)
		AND tCA.CXEState = 4 
GROUP BY tAS.AgentId, tCA.CashType, tL.LocationID,tL.LocationName
GO

-- Drop and recreate vComplianceTransactions
DROP VIEW [dbo].[vComplianceTransactions]
GO

CREATE VIEW [dbo].[vComplianceTransactions]
AS
SELECT	tBP.TxnPK rowguid, tBP.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tBP.DTTerminalCreate, 4 AS TransactionType,
		tBP.Amount, tBP.Fee, tBP.CXEState AS [State], tA.ProviderId, lt.LedgerTransactionsID, NULL AS xRecipientId, NULL AS xRate, 
		tBP.ProductId AS bpProductId, tBP.AccountNumber AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_BillPay tBP
		INNER JOIN tAccounts tA ON tBP.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tBP.LedgerEntryPK=le.LedgerEntriesPK
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionPK=lt.LedgerTransactionsPK
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tBP.TxnPK=scT.TxnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartPK=sc.CartPK

UNION ALL
SELECT	tC.txnPK rowguid, tC.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tC.DTTerminalCreate, 2 AS TransactionType,
		tC.Amount, tC.Fee, tC.CXEState AS [State], tA.ProviderId, lt.LedgerTransactionsID , NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Check tC
		INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tC.LedgerEntryPK=le.LedgerEntriesPK
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionPK=lt.LedgerTransactionsPK
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tC.TxnPK=scT.TxnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.CartPK=sc.CartPK

UNION ALL

SELECT	tMO.txnPK rowguid, tMO.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tMO.DTTerminalCreate, 5 AS TransactionType,
		tMO.Amount, tMO.Fee, tMO.CXEState AS [State], tA.ProviderId, lt.LedgerTransactionsID, NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyOrder tMO
		INNER JOIN tAccounts tA ON tMO.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tMO.LedgerEntryPK=le.LedgerEntriesPK
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionPK=lt.LedgerTransactionsPK
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMO.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK

UNION ALL

SELECT	tF.txnPK rowguid, tF.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tF.DTTerminalCreate, 
		CASE tF.FundType WHEN 0 THEN 3 
						 WHEN 1 THEN 8 
						 ELSE 9 END AS TransactionType,
		tF.Amount, tF.Fee, tF.CXEState AS [State], tA.ProviderId, lt.LedgerTransactionsId , NULL AS xRecipientId, NULL AS xRate, 
		NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_Funds tF
		INNER JOIN tAccounts tA ON tF.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tF.LedgerEntryPK=le.LedgerEntriesPK
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionPK=lt.LedgerTransactionsPK
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tF.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK

UNION ALL

SELECT	tMT.txnPK rowguid, tMT.TransactionID AS TransactionId, tPC.CustomerID as CustomerId, tMT.DTTerminalCreate, 6 AS TransactionType,
		tMT.Amount, tMT.Fee, tMT.CXEState AS [State], tA.ProviderId, lt.LedgerTransactionsID, tMT.RecipientId AS xRecipientId, 
		tMT.ExchangeRate AS xRate, NULL AS bpProductId, NULL AS bpAccountNumber, sc.CartID AS ShoppingCartId
FROM	tTxn_MoneyTransfer tMT
		INNER JOIN tAccounts tA ON tMT.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tMT.LedgerEntryPK=le.LedgerEntriesPK
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionPK=lt.LedgerTransactionsPK
		LEFT OUTER JOIN tShoppingCartTransactions scT ON tMT.txnPK=scT.txnPK
		LEFT OUTER JOIN tShoppingCarts sc ON scT.cartPK=sc.cartPK
GO

-- Drop and recreate vPastTransactions
DROP VIEW [dbo].[vPastTransactions]
GO

CREATE VIEW [dbo].[vPastTransactions] AS
SELECT
tPC.CustomerID AS CustomerId,
tC.CXEId AS CXEId,
tC.CXNId AS CXNId,
tC.CustomerSessionPK AS CustomerSessionPK,
tC.Amount AS Amount,
tC.Fee AS Fee,
tC.CXEState AS CXEState,
tC.DTTerminalLastModified AS DTTerminalLastModified,
tC.ProductId AS ProductId,
tC.TransactionID AS Id,
sP.ReceiverFirstName AS ReceiverFirstName,
sP.ReceiverLastName AS ReceiverLastName,
sP.StateName AS StateName,
sP.CountryName AS CountryName,
sP.BillerName AS BillerName,
sP.BillerCode AS BillerCode,
sP.AccountNumber AS AccountNumber,
sP.TransactionType AS TransactionType
FROM 
tTxn_BillPay tC  
INNER JOIN sPastTransactions sP ON sP.Id = tC.CXNId AND sP.TransactionType='BillPay'
INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK
INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tC.CustomerSessionPK
WHERE tC.CXEState= 4

UNION ALL

SELECT
tPC.CustomerID AS CustomerId,
tM.CXEId AS CXEId,
tM.CXNId AS CXNId,
tM.CustomerSessionPK AS CustomerSessionPK,
tM.Amount AS Amount,
tM.Fee AS Fee,
tM.CXEState AS CXEState,
tM.DTTerminalLastModified AS DTTerminalLastModified,
NULL AS ProductId,
tM.TransactionID AS Id,
sP.ReceiverFirstName AS ReceiverFirstName,
sP.ReceiverLastName AS ReceiverLastName,
sP.StateName AS StateName,
sP.CountryName AS CountryName,
sP.BillerName AS BillerName,
sP.BillerCode AS BillerCode,
sP.AccountNumber AS AccountNumber,
sP.TransactionType AS TransactionType
FROM 
tTxn_MoneyTransfer tM  
INNER JOIN sPastTransactions sP ON sP.Id = tM.CXNId AND sP.TransactionType='MoneyTransfer'
INNER JOIN tAccounts tA ON tM.AccountPK = tA.AccountPK
INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tM.CustomerSessionPK
WHERE tM.CXEState= 4
AND tM.TransactionID NOT IN(SELECT OriginalTransactionID FROM tTxn_MoneyTransfer WHERE TransferType=1)
AND COALESCE(tM.TransactionSubType, 0) <> 1
GO

-- Drop and recreate vTransactionHistory
DROP VIEW [dbo].[vTransactionHistory]
GO

CREATE VIEW [dbo].[vTransactionHistory]  
AS  
SELECT   
	tC.TxnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tC.DTTerminalCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tC.CXEId AS TransactionId,  
	tL.LocationName AS Location,   
	'Check Processing' AS TransactionType,  
	(Amount - Fee) AS TotalAmount,   
	tC.Description AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tC.CXEState   
		WHEN 1 THEN 'Pending'     
		WHEN 2 THEN 'Authorized'     
		WHEN 3 THEN 'AuthorizationFailed'     
		WHEN 4 THEN 'Committed'     
		WHEN 5 THEN 'Failed' 
		WHEN 6 THEN 'Cancelled'
		WHEN 7 THEN 'Expired'
		WHEN 8 THEN 'Declined'
		WHEN 9 THEN 'Initiated'
		WHEN 10 THEN 'Hold'
		WHEN 11 THEN 'Priced'
		WHEN 12 THEN 'Processing'    
		ELSE 'Unknown'  
	END TransactionStatus  
FROM   
	tTxn_Check tC  
	INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK  
	INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tC.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK  
	INNER JOIN sCustomer sC ON sC.CustomerID = tPC.CustomerID 
WHERE   
	tC.DTTerminalCreate > DATEADD(d, -90, GETDATE())  
	   
UNION ALL  
  
SELECT   
 tF.TxnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tF.DTTerminalCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tF.CXEId AS TransactionId,  
	tL.LocationName AS Location,   
	CASE tF.FundType   
	WHEN 0 THEN 'Prepaid-Withdraw'   
	WHEN 1 THEN 'Prepaid-Load'   
	ELSE 'Prepaid-Activate'   
	END AS TransactionType,  
	CASE tF.FundType   
	WHEN 1 THEN (Amount - Fee)  
	ELSE (Amount + Fee)  
	END AS TotalAmount,  
	''  AS TransactionDetail,  
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
	CASE tF.CXEState     
		WHEN 1 THEN 'Pending'     
		WHEN 2 THEN 'Authorized'     
		WHEN 3 THEN 'AuthorizationFailed'     
		WHEN 4 THEN 'Committed'     
		WHEN 5 THEN 'Failed' 
		WHEN 6 THEN 'Cancelled'
		WHEN 7 THEN 'Expired'
		WHEN 8 THEN 'Declined'
		WHEN 9 THEN 'Initiated'
		WHEN 10 THEN 'Hold'
		WHEN 11 THEN 'Priced'
		WHEN 12 THEN 'Processing'    
		ELSE 'Unknown'  
	END TransactionStatus  
FROM  
	tTxn_Funds tF  
	INNER JOIN tAccounts tA ON tF.AccountPK = tA.AccountPK  
	INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tF.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK 
	INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK 	
	INNER JOIN sCustomer sC ON sC.CustomerID = tPC.CustomerID  
WHERE   
	tF.DTTerminalCreate > DATEADD(d, -90, GETDATE())  
   
UNION ALL  
  
SELECT   
	tCA.txnPK AS rowguid,  
	tPC.CustomerID AS CustomerId,   
	tCA.DTTerminalCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tCA.CXEId TransactionId,  
	tL.LocationName AS Location,   
	CASE tCA.CashType   
	WHEN 1 THEN 'Cash In'   
	WHEN 2 THEN 'Cash Out'   
	END AS TransactionType,  
	CASE tCA.CashType   
	WHEN 1 THEN (Amount - Fee)  
	ELSE (Amount + Fee)  
	END AS TotalAmount,  
	'' AS TransactionDetail,  
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
	CASE tCA.CXEState   
		WHEN 1 THEN 'Pending'     
		WHEN 2 THEN 'Authorized'     
		WHEN 3 THEN 'AuthorizationFailed'     
		WHEN 4 THEN 'Committed'     
		WHEN 5 THEN 'Failed' 
		WHEN 6 THEN 'Cancelled'
		WHEN 7 THEN 'Expired'
		WHEN 8 THEN 'Declined'
		WHEN 9 THEN 'Initiated'
		WHEN 10 THEN 'Hold'
		WHEN 11 THEN 'Priced'
		WHEN 12 THEN 'Processing'    
		ELSE 'Unknown' 
	END TransactionStatus  
FROM   
	tTxn_Cash tCA  
	INNER JOIN tAccounts tA ON tCA.AccountPK = tA.AccountPK
	INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tCA.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK 
	INNER JOIN sCustomer sC ON sC.CustomerID = tPC.CustomerID
WHERE   
	tCA.DTTerminalCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT   
	tC.txnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tC.DTTerminalCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tC.CXEId AS TransactionId,  
	tL.LocationName AS Location, 
	CASE  tC.TransferType 
	WHEN '1' THEN 'SendMoney'
	WHEN '2' THEN 'ReceiveMoney'
	WHEN '3' THEN 'SendMoneyRefund'
	END AS  TransactionType,  	
	(CASE  tC.TransferType 
	WHEN '1' THEN Amount+Fee
	WHEN '2' THEN Amount-Fee

	ELSE Amount+Fee
	END  ) AS TotalAmount,  
	tC.Description AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tC.CXEState   
		WHEN 1 THEN 'Pending'     
		WHEN 2 THEN 'Authorized'     
		WHEN 3 THEN 'AuthorizationFailed'     
		WHEN 4 THEN 'Committed'     
		WHEN 5 THEN 'Failed' 
		WHEN 6 THEN 'Cancelled'
		WHEN 7 THEN 'Expired'
		WHEN 8 THEN 'Declined'
		WHEN 9 THEN 'Initiated'
		WHEN 10 THEN 'Hold'
		WHEN 11 THEN 'Priced'
		WHEN 12 THEN 'Processing'    
		ELSE 'Unknown'   
	END TransactionStatus  
FROM   
	tTxn_MoneyTransfer tC  
	INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK  
	INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tC.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK 
	INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK 
	INNER JOIN sCustomer sC ON sC.CustomerID = tPC.CustomerID 
WHERE   
	tC.DTTerminalCreate > DATEADD(d, -90, GETDATE())  

UNION ALL  

SELECT   
	tM.txnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tM.DTTerminalCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tM.CXEId AS TransactionId,  
	tL.LocationName AS Location,   
	'MoneyOrder' AS TransactionType,  
	(Amount + Fee) AS TotalAmount,  
	'' AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tM.CXEState   
		WHEN 1 THEN 'Pending'     
		WHEN 2 THEN 'Approved'     
		WHEN 3 THEN 'AuthorizationFailed'     
		WHEN 4 THEN 'Committed'     
		WHEN 5 THEN 'Failed' 
		WHEN 6 THEN 'Removed'
		WHEN 7 THEN 'Expired'
		WHEN 8 THEN 'Declined'
		WHEN 9 THEN 'Initiated'
		WHEN 10 THEN 'Hold'
		WHEN 11 THEN 'Priced'
		WHEN 12 THEN 'Processing'    
		ELSE 'Unknown'  
	END TransactionStatus  
FROM   
	tTxn_MoneyOrder tM  
	INNER JOIN tAccounts tA ON tM.AccountPK = tA.AccountPK
	INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tM.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK 
	INNER JOIN sCustomer sC ON sC.CustomerID = tPC.CustomerID 
WHERE   
	tM.DTTerminalCreate > DATEADD(d, -90, GETDATE())  


UNION ALL

SELECT   
	tC.txnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tC.DTTerminalCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tC.CXEId AS TransactionId,  
	tL.LocationName AS Location, 
	'BillPay' AS  TransactionType,  	
	Amount + Fee AS TotalAmount,  
	'' AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tC.CXEState   
		WHEN 1 THEN 'Pending'     
		WHEN 2 THEN 'Authorized'     
		WHEN 3 THEN 'AuthorizationFailed'     
		WHEN 4 THEN 'Committed'     
		WHEN 5 THEN 'Failed' 
		WHEN 6 THEN 'Cancelled'
		WHEN 7 THEN 'Expired'
		WHEN 8 THEN 'Declined'
		WHEN 9 THEN 'Initiated'
		WHEN 10 THEN 'Hold'
		WHEN 11 THEN 'Priced'
		WHEN 12 THEN 'Processing'    
		ELSE 'Unknown'
	END TransactionStatus  
FROM   
	tTxn_BillPay tC  
	INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK
	INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tC.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK
	INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK
	INNER JOIN sCustomer sC ON sC.CustomerID = tPC.CustomerID  
WHERE   
	tC.DTTerminalCreate > DATEADD(d, -90, GETDATE())
GO