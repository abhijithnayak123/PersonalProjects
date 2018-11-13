-- ============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <09/07/2015>
-- Description:	<Script to alter transaction history to get the transaction details.>
-- Jira ID:	<AL-798>
-- =============================================================================

ALTER VIEW [dbo].[vTransactionHistory]  
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
	(tC.Amount - tC.Fee) AS TotalAmount,   
	CASE tA.ProviderId
		WHEN 201 THEN
		  (
		     CASE sCC.CheckNumber WHEN '' THEN 'Not Available' ELSE ISNULL('[' + tCCT.Name + '] [' + sCC.CheckNumber + ']','Not Available') END
	      )
		WHEN 200 THEN
		  (
		     CASE sCC.CheckNumber WHEN '' THEN 'Not Available' ELSE ISNULL('[' + tCT.Name  + '] [' + sCH.CheckNumber + ']','Not Available') END	
	 	  )  
		ELSE 'Not Available'
	END AS TransactionDetail, 
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
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
	LEFT JOIN sChxr_Trx sCH ON sCH.ChxrTrxID = tC.CXNId AND tA.ProviderId = 200
	LEFT JOIN stxn_Check_Stage	sCT ON sCT.CheckID = tC.CXEId 
	LEFT JOIN tCheckTypes tCT ON tCT.CheckTypePK = sCT.CheckType		
	LEFT JOIN sCertegy_Trx sCC ON sCC.CertegyTrxID = tC.CXNId AND tA.ProviderId = 201
	LEFT JOIN stxn_Check_Stage sCS ON sCS.CheckID = tC.CXEId 
	LEFT JOIN tCheckTypes tCCT ON tCCT.CheckTypePK = sCS.CheckType
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
		WHEN 1 THEN (tF.Amount - tF.Fee)  
		ELSE (tF.Amount + tF.Fee)  
	END AS TotalAmount,  
	CASE tA.ProviderId
	     WHEN 102 THEN ISNULL('Card #[' + right(sSA.CardNumber,4) + '] ','Not Available')
		 WHEN 103 THEN ISNULL('Card #[' + right(sVA.CardNumber,4) + '] ','Not Available')
		 ELSE 'Not Available'
    END AS TransactionDetail,    
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
	LEFT JOIN sTSys_Trx sT ON sT.TSysTrxID=tF.CXNId AND tA.ProviderId = 102
	LEFT JOIN sTSys_Account sSA ON sSA.TSysAccountPK = sT.TSysAccountPK
	LEFT JOIN sVisa_Trx sV ON sV.Id = tF.CXNId AND tA.ProviderId = 103
	LEFT JOIN sVisa_Account sVA ON sVA.rowguid = sV.AccountPK
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
		    CASE tA.ProviderId 
				WHEN 301 THEN 'to [' + sW.RecieverFirstName + ' ' + ISNULL(sW.RecieverLastName,'') + ' ' + ISNULL(sW.RecieverSecondLastName,'') + '] [' + tCW.Name + ']'
				WHEN 302 THEN 'to [' + sM.ReceiverFirstName + ' ' + ISNULL(sM.ReceiverMiddleName,'') + ' ' + ISNULL(sM.ReceiverLastName,'') + '] [' + tCM.Name + ']'
				ELSE 'Not Available'
		    END
	    )
		WHEN '2' THEN 'from [' + sC.FirstName + ' ' + sC.MiddleName + ' ' + sC.LastName + ']'
		END AS TransactionDetail,		   
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
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
	LEFT JOIN sMGram_Transfer_Trx sM on tC.CXNId = sM.MGTransferTrxID AND tA.ProviderId = 302 
	LEFT JOIN sMGram_Countries tCM on tCM.Code = sM.DestinationCountry                                        
	LEFT JOIN sWUnion_Trx sW on tC.CXNId = sW.WUTrxID AND tA.ProviderId = 301 
	LEFT JOIN sWUnion_Countries tCW on tCW.ISOCountryCode = sW.DestinationCountryCode                          
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
	'Money Order' AS TransactionDetail,  
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
	CASE tA.ProviderId
		WHEN 401 THEN
		  (
			  CASE sBW.BillerName WHEN '' THEN 'Not Available' ELSE '[' + sBW.BillerName+'] Acct # [' + sBW.Customer_AccountNumber + ']' END 
	      )        
	    WHEN 402 THEN
		    (
			  CASE sBM.BillerName WHEN '' THEN 'Not Available' ELSE '[' + sBM.BillerName+'] Acct # [' + sBM.AccountNumber + ']' END 
			)                  
		ELSE 'Not Available'                                               
	END AS TransactionDetail, 
	sC.FirstName + ' ' + sC.LastName AS CustomerName,  
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
	LEFT JOIN sWUnion_BillPay_Trx sBW ON sBW.WUBillPayTrxID = tC.CXNId AND tA.ProviderId = 401 
	LEFT JOIN sMGram_BillPay_Trx sBM ON sBM.MGBillPayTrxID = tC.CXNId AND tA.ProviderId = 405  
WHERE   
	tC.DTTerminalCreate > DATEADD(d, -90, GETDATE())
GO


