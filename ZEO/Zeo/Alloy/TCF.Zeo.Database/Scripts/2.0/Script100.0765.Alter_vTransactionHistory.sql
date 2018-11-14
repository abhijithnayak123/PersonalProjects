-- ==========================================================================================
-- Author:		<Manikandan Govindraj>
-- Modified By: <M.Purna Pushkal>
-- Modified Date: <22/11/2017>
-- Create date: <12/07/2016>
-- Description:	<Modified  the SP to display Seder name for receive money>
-- Modify reason: Removing the days condition so that we'll get the transaction based on the date picker range as part of story B-06218.
-- Jira ID:		<AL-8869>
-- Modified By: <M.Purna Pushkal>
-- Modified Date: <06/11/2018>
-- Modify reason :  Left join with tchxr_transaction to support OnUs also.
-- ===========================================================================================

IF EXISTS(SELECT 1 FROM sys.views WHERE NAME = 'vTransactionHistory')
DROP VIEW vTransactionHistory
GO

CREATE VIEW vTransactionHistory
AS  
-- Check Processing Transactions
SELECT   
	C.CustomerID AS CustomerId,   
	ISNULL(tC.DTTerminalLastModified, tC.DTTerminalCreate) AS TransactionDate,   
	tAD.FirstName +' '+ tAD.LastName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tC.TransactionId,
	tL.LocationName AS Location,   
	'Check Processing' AS TransactionType,  
	(tC.Amount - tC.Fee) AS TotalAmount,   
	CASE tC.ProviderId
		WHEN 200 THEN
		  (
		     CASE sCH.CheckNumber WHEN '' THEN 'Not Available' ELSE ISNULL('[' + tCT.DisplayName  + '] [' + sCH.CheckNumber + ']','Not Available') END	
	 	  )
		  WHEN 202 THEN
		  (
		     CASE ttt.CheckNumber WHEN '' THEN 'Not Available' ELSE ISNULL('[' + tCT.DisplayName  + '] [' + ttt.CheckNumber + ']','Not Available') END	
	 	  )  
		ELSE 'Not Available'
	END AS TransactionDetail, 
	C.FirstName + ' ' + C.LastName AS CustomerName,  
	CASE tC.State   
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
	INNER JOIN tCustomerSessions tCS WITH (NOLOCK) ON tCS.CustomerSessionID = tC.CustomerSessionId
	INNER JOIN tAgentSessions tAS WITH (NOLOCK) ON tAS.AgentSessionID = tCS.AgentSessionId
	INNER JOIN tAgentDetails tAD WITH (NOLOCK) ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT WITH (NOLOCK) ON tAS.TerminalId = tT.TerminalID
	INNER JOIN tLocations tL WITH (NOLOCK) ON tT.LocationId = tL.LocationID  
	INNER JOIN tCustomers C WITH (NOLOCK) ON C.CustomerID = tCS.CustomerID 
	LEFT JOIN tChxr_Trx sCH WITH (NOLOCK) ON sCH.ChxrTrxID = tC.CXNId AND tC.ProviderId = 200 --INGO
	LEFT JOIN tTCFOnus_Trx ttt WITH (NOLOCK) ON ttt.TCFOnusTrxID = tC.CXNId AND tC.ProviderId = 202 --OnUs
	INNER JOIN tCheckTypes tCT WITH (NOLOCK) ON tCT.CheckTypeId = tC.CheckType	   
   
UNION ALL  
   
-- Fund Transactions
SELECT   
	sc.CustomerID AS CustomerId,   
	ISNULL(tF.DTTerminalLastModified, tF.DTTerminalCreate) AS TransactionDate,   
	tAD.FirstName +' '+ tAD.LastName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tF.TransactionID AS TransactionId,  
	tL.LocationName AS Location,   
	CASE tF.FundType   
	WHEN 0 THEN 'Prepaid-Withdraw'   
	WHEN 1 THEN 'Prepaid-Load'
	ELSE 'Prepaid-Activate'   
	END AS TransactionType,  
	CASE tF.FundType   
		WHEN 1 THEN (tF.Amount - tF.Fee)  
		ELSE (tF.Amount + tF.Fee)  
	END AS TotalAmount,  
	CASE tF.ProviderId
		 WHEN 103 THEN 
		    (
			   --CASE tF.FundType
				 -- WHEN 3 THEN 'Companion Card -{' + sCC.FirstName + ' ' + sCC.LastName + '}'
				  --ELSE 
				  ISNULL('Card #[' + right(sVA.CardNumber,4) + '] ', 'Not Available')
			   --END
			)		 
		 ELSE 'Not Available'
    END AS TransactionDetail,    
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
	CASE tF.State     
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
	INNER JOIN tCustomerSessions tCS WITH (NOLOCK) ON tCS.CustomerSessionId = tF.CustomerSessionId 
	INNER JOIN tAgentSessions tAS WITH (NOLOCK) ON tAS.AgentSessionID = tCS.AgentSessionId
	INNER JOIN tAgentDetails tAD WITH (NOLOCK) ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT WITH (NOLOCK) ON tAS.TerminalId = tT.TerminalID
	INNER JOIN tLocations tL WITH (NOLOCK) ON tT.LocationId = tL.LocationID 	
	INNER JOIN tCustomers sC WITH (NOLOCK) ON sC.CustomerID = tCS.CustomerID   
	INNER JOIN tVisa_Trx sV WITH (NOLOCK) ON sV.VisaTrxID = tF.CXNId AND tF.ProviderId = 103
	INNER JOIN tVisa_Account sVA WITH (NOLOCK) ON sVA.VisaAccountID = sV.VisaAccountId
	--INNER JOIN tCustomers sCC WITH (NOLOCK) ON tF.AddOnCustomerId = sCC.CustomerID	

UNION ALL

--Money Order Transactions
SELECT   
	sC.CustomerID AS CustomerId,   
	ISNULL(tM.DTTerminalLastModified, tM.DTTerminalCreate) AS TransactionDate,   
	tAD.FirstName +' '+ tAD.LastName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tM.TransactionID AS TransactionId,  
	tL.LocationName AS Location,   
	'MoneyOrder' AS TransactionType,  
	(Amount + Fee) AS TotalAmount,  
	'Money Order [' + ISNULL(tM.CheckNumber,'Not Available') + ']' AS TransactionDetail,  
	sC.FirstName + ' '+ sC.LastName AS CustomerName,  
	CASE tM.State   
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
	INNER JOIN tCustomerSessions tCS WITH (NOLOCK) ON tCS.CustomerSessionID = tM.CustomerSessionId 
	INNER JOIN tAgentSessions tAS WITH (NOLOCK) ON tAS.AgentSessionID = tCS.AgentSessionId
	INNER JOIN tAgentDetails tAD WITH (NOLOCK) ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT WITH (NOLOCK) ON tAS.TerminalId = tT.TerminalID  
	INNER JOIN tLocations tL WITH (NOLOCK) ON tT.LocationId = tL.LocationID 
	INNER JOIN tCustomers sC WITH (NOLOCK) ON sC.CustomerID = tCS.CustomerID

UNION ALL

--BillPay Transactions 
SELECT   
	sC.CustomerID AS CustomerId,   
	ISNULL(tC.DTTerminalLastModified, tC.DTTerminalCreate) AS TransactionDate,   
	tAD.FirstName +' '+ tAD.LastName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tC.TransactionID AS TransactionId,  
	tL.LocationName AS Location, 
	'BillPay' AS  TransactionType,  	
	Amount + Fee AS TotalAmount,  
	CASE tC.ProviderId
		WHEN 401 THEN
		  (
			  CASE sBW.BillerName WHEN '' THEN 'Not Available' ELSE '[' + sBW.BillerName+'] Acct # [' + sBW.Customer_AccountNumber + ']' END 
	      )                   
		ELSE 'Not Available'                                               
	END AS TransactionDetail, 
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
	CASE tC.State   
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
	INNER JOIN tCustomerSessions tCS WITH (NOLOCK) ON tCS.CustomerSessionID = tC.CustomerSessionId
	INNER JOIN tAgentSessions tAS WITH (NOLOCK) ON tAS.AgentSessionID = tCS.AgentSessionId  
	INNER JOIN tAgentDetails tAD WITH (NOLOCK) ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT WITH (NOLOCK) ON tAS.TerminalId = tT.TerminalID
	INNER JOIN tLocations tL WITH (NOLOCK) ON tT.LocationId = tL.LocationID
	INNER JOIN tCustomers sC WITH (NOLOCK) ON sC.CustomerID = tCS.CustomerID  
	INNER JOIN tWUnion_BillPay_Trx sBW WITH (NOLOCK) ON sBW.WUBillPayTrxID= tC.CXNId AND tC.ProviderId = 401

UNION ALL

-- Money Transfer Transactions
SELECT   
	C.CustomerID AS CustomerId,   
	ISNULL(tC.DTTerminalLastModified, tC.DTTerminalCreate) AS TransactionDate,   
	tAD.FirstName +' '+ tAD.LastName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tC.TransactionID AS TransactionId,  
	tL.LocationName AS Location, 
	CASE  tC.TransferType 
		WHEN '1' THEN 
			(
				CASE tC.TransactionSubType 
					WHEN '3' THEN 'SendMoneyRefund' 
					WHEN '1' THEN 'SendMoneyCancel'
					ELSE 'SendMoney' 
				END
			)
		WHEN '2' THEN 'ReceiveMoney'
		END AS  TransactionType,  	
		(
			CASE  tC.TransferType 
				WHEN '1' THEN tC.Amount + Fee
				WHEN '2' THEN tC.Amount - Fee
				ELSE tC.Amount + Fee
			END  
		) AS TotalAmount,  
				CASE  tC.TransferType              
		WHEN '1' THEN 
		(
		    CASE tC.ProviderId 
				WHEN 301 THEN 'to [' + sW.RecieverFirstName + ' ' + ISNULL(sW.RecieverLastName,'') + ' ' + ISNULL(sW.RecieverSecondLastName,'') + '] [' + tCW.Name + ']'
				ELSE 'Not Available'
		    END
	    )
		WHEN '2' THEN 'from [' + sW.SenderName + ']'
		END AS TransactionDetail,		   
	C.FirstName + ' ' + C.LastName AS CustomerName,  
	CASE tC.State   
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
	INNER JOIN tCustomerSessions tCS WITH (NOLOCK) ON tCS.CustomerSessionID = tC.CustomerSessionId
	INNER JOIN tAgentSessions tAS WITH (NOLOCK) ON tAS.AgentSessionID = tCS.AgentSessionId
	INNER JOIN tAgentDetails tAD WITH (NOLOCK) ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT WITH (NOLOCK) ON tAS.TerminalId = tT.TerminalID
	INNER JOIN tLocations tL WITH (NOLOCK) ON tT.LocationId = tL.LocationID 
	INNER JOIN tCustomers C WITH (NOLOCK) ON C.CustomerID = tCS.CustomerID                                       
	INNER JOIN tWUnion_Trx sW WITH (NOLOCK) ON tC.CXNId = sW.WUtrxId AND tC.ProviderId = 301 
	LEFT JOIN tWUnion_Countries tCW WITH (NOLOCK) ON tCW.ISOCountryCode = sW.DestinationCountryCode

UNION ALL  
  
--Cash Transactions
SELECT   
	tCS.CustomerID AS CustomerId,   
	ISNULL(tCA.DTTerminalLastModified, tCA.DTTerminalCreate) AS TransactionDate,   
	tAD.FirstName +' '+ tAD.LastName AS 'Teller',  
	tAD.AgentID AS 'TellerId',  
	tCS.CustomerSessionID AS 'SessionID',  
	tCA.TransactionID TransactionId,  
	tL.LocationName AS Location,   
	CASE tCA.CashType   
	WHEN 1 THEN 'Cash In'   
	WHEN 2 THEN 'Cash Out'   
	END AS TransactionType,  
	Amount TotalAmount,  
	'' AS TransactionDetail,  
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
	CASE tCA.State   
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
	INNER JOIN tCustomerSessions tCS WITH (NOLOCK) ON tCS.CustomerSessionID = tCA.CustomerSessionId
	INNER JOIN tAgentSessions tAS WITH (NOLOCK) ON tAS.AgentSessionID = tCS.AgentSessionId  
	INNER JOIN tAgentDetails tAD WITH (NOLOCK) ON tAD.AgentID = tAS.AgentId  
	INNER JOIN tTerminals tT WITH (NOLOCK) ON tAS.TerminalId = tT.TerminalID
	INNER JOIN tLocations tL WITH (NOLOCK) ON tT.LocationId = tL.LocationID 
	INNER JOIN tCustomers sC WITH (NOLOCK) ON sC.CustomerID = tCS.CustomerID
GO
