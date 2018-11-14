IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vTransactionHistory]'))
DROP VIEW [dbo].[vTransactionHistory]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vTransactionHistory]
AS
SELECT	
	tC.txnRowguid AS rowguid, 
	tPC.Id AS CustomerId, 
	tC.DTCreate AS	TransactionDate, 
	tAD.FullName AS 'Teller',
	tAS.Id AS 'SessionID',
	tC.CXEId AS TransactionId,
	tL.LocationName AS Location, 
	'Check Processing' AS TransactionType,
	(Amount + Fee) AS TotalAmount,
	sTH.AddlInfo1  AS TransactionDetail
FROM	
	tTxn_Check tC
	INNER JOIN tAccounts tA ON tC.AccountPK = tA.rowguid
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tC.CustomerSessionPK
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
	INNER JOIN sTransHistory sTH ON sTH.CxnId = tC.CXNId AND sTH.TxnType = 2
WHERE	
	tC.DTCreate > DATEADD(d, -90, GETDATE())
	AND tC.CXEState = 4 
	
UNION ALL

SELECT	
	tF.txnRowguid AS rowguid, 
	tPC.Id AS CustomerId, 
	tF.DTCreate AS TransactionDate, 
	tAD.FullName AS 'Teller',
	tAS.Id AS 'SessionID',
	tF.CXEId AS TransactionId,
	tL.LocationName AS Location, 
	'Prepaid' AS TransactionType,
	(Amount + Fee) AS TotalAmount,
	sTH.AddlInfo1  AS TransactionDetail
FROM
	tTxn_Funds tF
	INNER JOIN tAccounts tA ON tF.AccountPK = tA.rowguid
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tF.CustomerSessionPK
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
	INNER JOIN sTransHistory sTH ON sTH.CxnId = tF.CXNId AND sTH.TxnType = 3
WHERE	
	tF.DTCreate > DATEADD(d, -90, GETDATE())
	AND tF.CXEState = 4 
	
UNION ALL

SELECT	
	tCA.txnRowguid AS rowguid,
	tCA.Id AS CustomerId, 
	tCA.DTCreate AS TransactionDate, 
	tAD.FullName AS 'Teller',
	tAS.Id AS 'SessionID',
	tCA.CXEId TransactionId,
	tL.LocationName AS Location, 
	CASE tCA.CashType 
		WHEN 1 THEN 'Cash In' 
		WHEN 2 THEN 'Cash Out' 
	END AS TransactionType,
	(Amount + Fee) AS TotalAmount,
	'' AS TransactionDetail
FROM	
	tTxn_Cash tCA
	INNER JOIN tAccounts tA ON tCA.AccountPK = tA.rowguid
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tCA.CustomerSessionPK
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
WHERE	
	tCA.DTCreate > DATEADD(d, -90, GETDATE())
	AND tCA.CXEState = 4 
