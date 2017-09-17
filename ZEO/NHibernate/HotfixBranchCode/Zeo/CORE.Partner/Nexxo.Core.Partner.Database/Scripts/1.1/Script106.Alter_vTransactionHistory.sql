SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vTransactionHistory]  
AS  
SELECT 
txn.txnRowguid AS rowguid,
customers.Id AS CustomerId,
txn.DTCreate AS TransactionDate,
agents.FullName AS Teller,
agents.Id AS TellerId,
customersession.Id AS SessionId,
txn.CXEId AS TransactionId,
locations.LocationName AS Location,
'Check Processing' AS TransactionType,  
(Amount - Fee) AS TotalAmount,  
txn.Description AS TransactionDetail,
cxecustomers.FirstName + ' ' + cxecustomers.LastName AS CustomerName,
CASE txn.CXEState   
	WHEN 1 THEN 'Pending'   
	WHEN 2 THEN 'Authorized'   
	WHEN 3 THEN 'AuthorizationFailed'   
	WHEN 4 THEN 'Committed'   
	WHEN 5 THEN 'Failed'   
	ELSE 'Unknown'  
END TransactionStatus
FROM 
tTxn_Check txn 
INNER JOIN tAccounts accts ON txn.AccountPK = accts.rowguid 
INNER JOIN tPartnerCustomers customers ON accts.CustomerPK = customers.rowguid
INNER JOIN tCustomerSessions customersession ON txn.CustomerSessionPK = customersession.customerSessionRowguid
INNER JOIN tAgentSessions agentsession ON agentsession.rowguid = customersession.AgentSessionPK
INNER JOIN tAgentDetails agents ON agentsession.AgentId = agents.Id
INNER JOIN tTerminals terminals ON terminals.rowguid = agentsession.TerminalPK
INNER JOIN tLocations locations ON locations.rowguid = terminals.LocationPK
INNER JOIN sCustomer cxecustomers ON cxecustomers.Id = customers.CXEId
INNER JOIN sTransHistory cxntransactions ON 
cxntransactions.CxnId = txn.CXNId 
AND cxntransactions.ProviderId = accts.ProviderId 
AND cxntransactions.CXNAccountId = accts.CXNId
WHERE   
	txn.DTCreate > DATEADD(d, -90, GETDATE()) 

UNION ALL

SELECT 
txn.txnRowguid AS rowguid,
customers.Id AS CustomerId,
txn.DTCreate AS TransactionDate,
agents.FullName AS Teller,
agents.Id AS TellerId,
customersession.Id AS SessionId,
txn.CXEId AS TransactionId,
locations.LocationName AS Location,
CASE txn.FundType   
	WHEN 0 THEN 'Prepaid-Withdraw'   
	WHEN 1 THEN 'Prepaid-Load'   
	ELSE 'Prepaid-Activate'   
	END AS TransactionType,
(Amount - Fee) AS TotalAmount,  
cxntransactions.AddlInfo1 AS TransactionDetail,
cxecustomers.FirstName + ' ' + cxecustomers.LastName AS CustomerName,
CASE txn.CXEState     
		WHEN 1 THEN 'Pending'   
		WHEN 2 THEN 'Authorized'   
		WHEN 3 THEN 'AuthorizationFailed'   
		WHEN 4 THEN 'Committed'   
		WHEN 5 THEN 'Failed'   
		ELSE 'Unknown'   
	END TransactionStatus 
FROM 
tTxn_Funds txn 
INNER JOIN tAccounts accts ON txn.AccountPK = accts.rowguid
INNER JOIN tPartnerCustomers customers ON accts.CustomerPK = customers.rowguid
INNER JOIN tCustomerSessions customersession ON txn.CustomerSessionPK = customersession.customerSessionRowguid
INNER JOIN tAgentSessions agentsession ON agentsession.rowguid = customersession.AgentSessionPK
INNER JOIN tAgentDetails agents ON agentsession.AgentId = agents.Id
INNER JOIN tTerminals terminals ON terminals.rowguid = agentsession.TerminalPK
INNER JOIN tLocations locations ON locations.rowguid = terminals.LocationPK
INNER JOIN sCustomer cxecustomers ON cxecustomers.Id = customers.CXEId
INNER JOIN sTransHistory cxntransactions ON 
cxntransactions.CxnId = txn.CXNId 
AND cxntransactions.CXNAccountId = accts.CXNId
AND cxntransactions.TxnType = 3
WHERE   
	txn.DTCreate > DATEADD(d, -90, GETDATE()) 

UNION ALL

SELECT 
txn.txnRowguid AS rowguid,
customers.Id AS CustomerId,
txn.DTCreate AS TransactionDate,
agents.FullName AS Teller,
agents.Id AS TellerId,
customersession.Id AS SessionId,
txn.CXEId AS TransactionId,
locations.LocationName AS Location,
CASE txn.CashType   
	WHEN 1 THEN 'Cash In'   
	WHEN 2 THEN 'Cash Out'   
	END AS TransactionType,
(Amount - Fee) AS TotalAmount,  
txn.Description AS TransactionDetail,
cxecustomers.FirstName + ' ' + cxecustomers.LastName AS CustomerName,
CASE txn.CXEState   
		WHEN 1 THEN 'Pending'   
		WHEN 2 THEN 'Authorized'   
		WHEN 3 THEN 'AuthorizationFailed'   
		WHEN 4 THEN 'Committed'   
		WHEN 5 THEN 'Failed'   
		ELSE 'Unknown'   
	END TransactionStatus 
FROM 
tTxn_Cash txn 
INNER JOIN tAccounts accts ON txn.AccountPK = accts.rowguid
INNER JOIN tPartnerCustomers customers ON accts.CustomerPK = customers.rowguid
INNER JOIN tCustomerSessions customersession ON txn.CustomerSessionPK = customersession.customerSessionRowguid
INNER JOIN tAgentSessions agentsession ON agentsession.rowguid = customersession.AgentSessionPK
INNER JOIN tAgentDetails agents ON agentsession.AgentId = agents.Id
INNER JOIN tTerminals terminals ON terminals.rowguid = agentsession.TerminalPK
INNER JOIN tLocations locations ON locations.rowguid = terminals.LocationPK
INNER JOIN sCustomer cxecustomers ON cxecustomers.Id = customers.CXEId
WHERE   
	txn.DTCreate > DATEADD(d, -90, GETDATE()) 
GO