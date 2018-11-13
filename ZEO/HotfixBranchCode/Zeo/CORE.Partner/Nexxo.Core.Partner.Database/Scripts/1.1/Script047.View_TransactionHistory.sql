IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vTransactionHistory]'))
DROP VIEW [dbo].[vTransactionHistory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vTransactionHistory]
AS
SELECT	tBP.txnRowguid rowguid, tPC.Id PAN, tBP.DTCreate TransactionDate, tBP.CXEId TransactionId,
		'Bill Pay' Product,Amount,Fee, 
		CASE tBP.CXEState WHEN 1 THEN 'Pending' 
						  WHEN 2 THEN 'Authorized' 
						  WHEN 3 THEN 'AuthorizationFailed' 
						  WHEN 4 THEN 'Complete' 
						  WHEN 5 THEN 'Failed' 
						  ELSE 'Unknown' END [Status],
		ISNULL(tBP.ConfirmationNumber,'') ConfirmationNumber, tL.LocationName Location, '4' TxnType
FROM	tTxn_BillPay tBP
		INNER JOIN tAccounts tA ON tBP.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tBP.CustomerSessionPK
		INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
		INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
		INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
WHERE	tBP.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT	tC.txnRowguid rowguid, tPC.Id PAN, tC.DTCreate TransactionDate, tC.CXEId TransactionId,
		'Check to Card' Product,Amount,Fee, 
		CASE tC.CXEState WHEN 1 THEN 'Pending' 
						  WHEN 2 THEN 'Authorized' 
						  WHEN 3 THEN 'AuthorizationFailed' 
						  WHEN 4 THEN 'Complete' 
						  WHEN 5 THEN 'Failed' 
						  ELSE 'Unknown' END [Status],
		ISNULL(tC.ConfirmationNumber,'') ConfirmationNumber, tL.LocationName Location, '3' TxnType
FROM	tTxn_Check tC
		INNER JOIN tAccounts tA ON tC.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tC.CustomerSessionPK
		INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
		INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
		INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
WHERE	tC.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT	tMT.txnRowguid rowguid, tPC.Id PAN, tMT.DTCreate TransactionDate, tMT.CXEId TransactionId,
		'Wire Transfer' Product,Amount,Fee, 
		CASE tMT.CXEState WHEN 1 THEN 'Pending' 
						  WHEN 2 THEN 'Authorized' 
						  WHEN 3 THEN 'AuthorizationFailed' 
						  WHEN 4 THEN 'Complete' 
						  WHEN 5 THEN 'Failed' 
						  ELSE 'Unknown' END [Status],
		ISNULL(tMT.ConfirmationNumber,'') ConfirmationNumber, tL.LocationName Location, '6' TxnType
FROM	tTxn_MoneyTransfer tMT
		INNER JOIN tAccounts tA ON tMT.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tMT.CustomerSessionPK
		INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
		INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
		INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
WHERE	tMT.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT	tF.txnRowguid rowguid, tPC.Id PAN, tF.DTCreate TransactionDate, tF.CXEId TransactionId,
		CASE tF.FundType WHEN 0 THEN 'Cash Withdrawal' 
						 WHEN 1 THEN 'Cash to Card' 
						 ELSE 'Card Activate' END Product,
		Amount,Fee, 
		CASE tF.CXEState WHEN 1 THEN 'Pending' 
						 WHEN 2 THEN 'Authorized' 
						 WHEN 3 THEN 'AuthorizationFailed' 
						 WHEN 4 THEN 'Complete' 
						 WHEN 5 THEN 'Failed' 
						 ELSE 'Unknown' END [Status],
		ISNULL(tF.ConfirmationNumber,'') ConfirmationNumber, tL.LocationName Location, '2' TxnType
FROM	tTxn_Funds tF
		INNER JOIN tAccounts tA ON tF.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tF.CustomerSessionPK
		INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
		INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
		INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
WHERE	tF.DTCreate > DATEADD(d, -90, GETDATE())