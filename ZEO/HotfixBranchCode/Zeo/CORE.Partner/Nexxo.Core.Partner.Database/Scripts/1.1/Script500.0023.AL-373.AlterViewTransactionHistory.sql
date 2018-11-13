--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

/****** Object:  View [dbo].[vTransactionHistory]    Script Date: 3/27/2015 1:59:14 PM ******/


/****** Object:  View [dbo].[vTransactionHistory]    Script Date: 3/27/2015 1:59:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vTransactionHistory]  
AS  
SELECT   
	tC.TxnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tC.DTCreate AS TransactionDate,   
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
	tC.DTCreate > DATEADD(d, -90, GETDATE())  
	   
UNION ALL  
  
SELECT   
 tF.TxnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tF.DTCreate AS TransactionDate,   
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
	tF.DTCreate > DATEADD(d, -90, GETDATE())  
   
UNION ALL  
  
SELECT   
	tCA.txnPK AS rowguid,  
	tPC.CustomerID AS CustomerId,   
	tCA.DTCreate AS TransactionDate,   
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
	tCA.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT   
	tC.txnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tC.DTCreate AS TransactionDate,   
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
	tC.DTCreate > DATEADD(d, -90, GETDATE())  

UNION ALL  

SELECT   
	tM.txnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tM.DTCreate AS TransactionDate,   
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
	tM.DTCreate > DATEADD(d, -90, GETDATE())  


UNION ALL

SELECT   
	tC.txnPK AS rowguid,   
	tPC.CustomerID AS CustomerId,   
	tC.DTCreate AS TransactionDate,   
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
	tC.DTCreate > DATEADD(d, -90, GETDATE())
GO


