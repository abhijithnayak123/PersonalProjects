IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vTransactionHistory]'))
DROP VIEW [dbo].[vTransactionHistory]
GO

CREATE VIEW [dbo].[vTransactionHistory]  
AS  
SELECT   
	tC.txnRowguid AS rowguid,   
	tPC.Id AS CustomerId,   
	tC.DTCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.Id AS 'TellerId',  
	tAS.Id AS 'SessionID',  
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
		ELSE 'Unknown'   
	END TransactionStatus  
FROM   
	tTxn_Check tC  
	INNER JOIN tAccounts tA ON tC.AccountPK = tA.rowguid  
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tC.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid  
	INNER JOIN sCustomer sC ON sC.Id = tPC.Id  
WHERE   
	tC.DTCreate > DATEADD(d, -90, GETDATE())  
	   
UNION ALL  
  
SELECT   
 tF.txnRowguid AS rowguid,   
	tPC.Id AS CustomerId,   
	tF.DTCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.Id AS 'TellerId',  
	tAS.Id AS 'SessionID',  
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
		ELSE 'Unknown'   
	END TransactionStatus  
FROM  
	tTxn_Funds tF  
	INNER JOIN tAccounts tA ON tF.AccountPK = tA.rowguid  
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tF.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid  	
	INNER JOIN sCustomer sC ON sC.Id = tPC.Id  
WHERE   
	tF.DTCreate > DATEADD(d, -90, GETDATE())  
   
UNION ALL  
  
SELECT   
	tCA.txnRowguid AS rowguid,  
	tPC.Id AS CustomerId,   
	tCA.DTCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.Id AS 'TellerId',  
	tAS.Id AS 'SessionID',  
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
		ELSE 'Unknown'   
	END TransactionStatus  
FROM   
	tTxn_Cash tCA  
	INNER JOIN tAccounts tA ON tCA.AccountPK = tA.rowguid  
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tCA.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid  
	INNER JOIN sCustomer sC ON sC.Id = tPC.Id  
WHERE   
	tCA.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT   
	tC.txnRowguid AS rowguid,   
	tPC.Id AS CustomerId,   
	tC.DTCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.Id AS 'TellerId',  
	tAS.Id AS 'SessionID',  
	tC.CXEId AS TransactionId,  
	tL.LocationName AS Location, 
	case  tC.TransferType 
	when '1' then 'SendMoney'
	when '2' then 'ReceiveMoney'
	End as  TransactionType,  	
	(case  tC.TransferType 
	when '1' then Amount+Fee
	when '2' then Amount-Fee
	else Amount+Fee
	End  ) as TotalAmount,  
	tC.Description AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tC.CXEState   
		WHEN 1 THEN 'Pending'   
		WHEN 2 THEN 'Authorized'   
		WHEN 3 THEN 'AuthorizationFailed'   
		WHEN 4 THEN 'Committed'   
		WHEN 5 THEN 'Failed'   
		ELSE 'Unknown'   
	END TransactionStatus  
FROM   
	tTxn_MoneyTransfer tC  
	INNER JOIN tAccounts tA ON tC.AccountPK = tA.rowguid  
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tC.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid  
	INNER JOIN sCustomer sC ON sC.Id = tPC.Id  
WHERE   
	tC.DTCreate > DATEADD(d, -90, GETDATE())  

UNION ALL  

SELECT   
	tM.txnRowguid AS rowguid,   
	tPC.Id AS CustomerId,   
	tM.DTCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.Id AS 'TellerId',  
	tAS.Id AS 'SessionID',  
	tM.CXEId AS TransactionId,  
	tL.LocationName AS Location,   
	'MoneyOrder' AS TransactionType,  
	(Amount + Fee) AS TotalAmount,  
	'' AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tM.CXEState   
		WHEN 1 THEN 'Initiated'   
		WHEN 2 THEN 'Approved'   
		WHEN 4 THEN 'Committed'   
		WHEN 6 THEN 'Removed' 
		WHEN 12 THEN 'Processing' 
		ELSE 'Unknown'   
	END TransactionStatus  
FROM   
	tTxn_MoneyOrder tM  
	INNER JOIN tAccounts tA ON tM.AccountPK = tA.rowguid  
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tM.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid  
	INNER JOIN sCustomer sC ON sC.Id = tPC.Id  
WHERE   
	tM.DTCreate > DATEADD(d, -90, GETDATE())  


UNION ALL

SELECT   
	tC.txnRowguid AS rowguid,   
	tPC.Id AS CustomerId,   
	tC.DTCreate AS TransactionDate,   
	tAD.FullName AS 'Teller',  
	tAD.Id AS 'TellerId',  
	tAS.Id AS 'SessionID',  
	tC.CXEId AS TransactionId,  
	tL.LocationName AS Location, 
	'BillPay' as  TransactionType,  	
	Amount + Fee as TotalAmount,  
	'' AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tC.CXEState   
		WHEN 1 THEN 'Pending'   
		WHEN 2 THEN 'Authorized'   
		WHEN 3 THEN 'AuthorizationFailed'   
		WHEN 4 THEN 'Committed'   
		WHEN 5 THEN 'Failed'   
		ELSE 'Unknown'   
	END TransactionStatus  
FROM   
	tTxn_BillPay tC  
	INNER JOIN tAccounts tA ON tC.AccountPK = tA.rowguid  
	INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
	INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tC.CustomerSessionPK  
	INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK  
	INNER JOIN tAgentDetails tAD ON tAD.Id = tAS.AgentId  
	INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid  
	INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid  
	INNER JOIN sCustomer sC ON sC.Id = tPC.Id  
WHERE   
	tC.DTCreate > DATEADD(d, -90, GETDATE())  


GO


