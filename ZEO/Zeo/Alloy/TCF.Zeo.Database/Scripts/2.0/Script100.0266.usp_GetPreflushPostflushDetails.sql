IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetPreflushPostflushDetails')
DROP PROCEDURE usp_GetPreflushPostflushDetails
GO

CREATE PROCEDURE usp_GetPreflushPostflushDetails
	@CustomerSessionId BIGINT,
	@CustomerId BIGINT,
	@State INT
AS
BEGIN

DECLARE @CartId BIGINT = 
	(
		SELECT 
			TOP 1 CartId 
		FROM 
			tShoppingCarts WITH (NOLOCK)
		WHERE 
			State != 2
			AND State = @State
			AND	CustomerSessionId = @customerSessionId
		ORDER BY
			DTTerminalCreate DESC
	)

EXEC usp_GetCustomerByCustomerId @CustomerId 

SELECT   
	tC.TransactionId AS Id
	,'' AS AccountNumber 
	,'' AS AliasId 
	,tC.Amount
	,'' AS CardNumber 
	,'' AS CashType 
	,sCH.CheckNumber
	,tCT.Name AS CheckType
	,tc.ConfirmationNumber
	,tC.Fee
	,(tC.Amount - tC.Fee) AS GrossTotalAmount
	,CAST(0 AS BIT) AS isActivate
	,'' AS MTCN 
	,'' AS Payee 
	,tC.State  AS [Status] 
	,'' AS ToAddress 
	,'' AS ToCity 
	,'' AS ToCountry 
	,'' AS ToCountryOfBirth 
	,'' AS ToDeliveryMethod 
	,'' AS ToDeliveryOption 
	,'' AS ToFirstName 
	,'' AS ToLastName 
	,'' AS ToPhoneNumber 
	,'' AS ToPickUpCity 
	,'' AS ToPickUpCountry 
	,'' AS ToPickUpState_Province 
	,'' AS ToSecondLastName 
	,'' AS ToState_Province 
	,'' AS ToZipCode 
	,'' AS TransferType 
	,'Check' AS [Type]
FROM   
	tTxn_Check tC
	LEFT JOIN tChxr_Trx sCH ON sCH.ChxrTrxID = tC.CXNId AND tC.ProviderId = 200
	LEFT JOIN tCheckTypes tCT ON tCT.CheckTypeId = tC.CheckType
	INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = tC.TransactionId
	INNER JOIN tshoppingcarts sc on sc.CartId = sct.CartId
WHERE   
	 sct.CartId = @CartId AND sct.CartItemStatus = 0
   
UNION ALL  

SELECT   
	tF.TransactionID AS Id
	,'' AS AccountNumber 
	,sVA.CardAliasId AS AliasId
	,tF.Amount
	,CASE tF.FundType 
		WHEN 2 THEN sVA.CardNumber
		ELSE SUBSTRING(sVA.CardNumber,LEN(sVA.CardNumber)-6,LEN(sVA.CardNumber))
		END AS CardNumber
	,'' AS CashType 
	,'' AS CheckNumber
	,'' AS CheckType 
	,sV.ConfirmationId AS ConfirmationNumber 
	,tF.Fee
	,(tF.Amount - tF.Fee) AS GrossTotalAmount
	,CASE tF.FundType 
		WHEN 2 THEN CAST(1 AS BIT)
		ELSE CAST(0 AS BIT)
		END AS isActivate 
	,'' AS MTCN 
	,'' AS Payee 
	,tF.State  AS [Status]
	,'' AS ToAddress 
	,'' AS ToCity 
	,'' AS ToCountry 
	,'' AS ToCountryOfBirth 
	,'' AS ToDeliveryMethod 
	,'' AS ToDeliveryOption 
	,'' AS ToFirstName 
	,'' AS ToLastName 
	,'' AS ToPhoneNumber 
	,'' AS ToPickUpCity 
	,'' AS ToPickUpCountry 
	,'' AS ToPickUpState_Province 
	,'' AS ToSecondLastName 
	,'' AS ToState_Province 
	,'' AS ToZipCode 
	,CASE tF.FundType   
		WHEN 0 THEN 'Debit'   
		ELSE 'Credit'
		END AS TransferType
	,'Funds' AS [Type]
FROM  
	tTxn_Funds tF  
	LEFT JOIN tVisa_Trx sV ON sV.VisaTrxID = tF.CXNId AND tF.ProviderId = 103
	LEFT JOIN tVisa_Account sVA ON sVA.VisaAccountID = sV.VisaAccountId
	INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = tF.TransactionId
	INNER JOIN tshoppingcarts sc on sc.CartId = sct.CartId
WHERE   
	 sct.CartId = @CartId AND sct.CartItemStatus = 0

UNION ALL

SELECT   
	tM.TransactionID AS Id
	,'' AS AccountNumber 
	,'' AS AliasId 
	,tM.Amount
	,'' AS CardNumber 
	,'' AS CashType 
	,tM.CheckNumber
	,'' AS CheckType 
	,'' AS ConfirmationNumber 
	,tM.Fee  
	,(Amount + Fee) AS GrossTotalAmount
	,CAST(0 AS BIT) AS isActivate
	,'' AS MTCN 
	,'' AS Payee 
	,tM.State AS [Status]
	,'' AS ToAddress 
	,'' AS ToCity 
	,'' AS ToCountry 
	,'' AS ToCountryOfBirth 
	,'' AS ToDeliveryMethod 
	,'' AS ToDeliveryOption 
	,'' AS ToFirstName 
	,'' AS ToLastName 
	,'' AS ToPhoneNumber 
	,'' AS ToPickUpCity 
	,'' AS ToPickUpCountry 
	,'' AS ToPickUpState_Province 
	,'' AS ToSecondLastName 
	,'' AS ToState_Province 
	,'' AS ToZipCode 
	,'' AS TransferType 
	,'MoneyOrder' AS [Type]
FROM   
	tTxn_MoneyOrder tM  
	INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = tM.TransactionId
	INNER JOIN tshoppingcarts sc on sc.CartId = sct.CartId
WHERE   
	 sct.CartId = @CartId AND sct.CartItemStatus = 0

UNION ALL

SELECT   
	tB.TransactionID AS Id
	 ,tB.AccountNumber
	,'' AS AliasId 
	 ,Amount
	,'' AS CardNumber 
	,'' AS CashType 
	,'' AS CheckNumber 
	,'' AS CheckType 
	,'' AS ConfirmationNumber 
	,Fee
	,(Amount + Fee) AS GrossTotalAmount
	,CAST(0 AS BIT) AS isActivate
	,sBW.MTCN
	,sBW.BillerName AS Payee
	,tB.State AS [Status] 
	,'' AS ToAddress 
	,'' AS ToCity 
	,'' AS ToCountry 
	,'' AS ToCountryOfBirth 
	,'' AS ToDeliveryMethod 
	,'' AS ToDeliveryOption 
	,'' AS ToFirstName 
	,'' AS ToLastName 
	,'' AS ToPhoneNumber 
	,'' AS ToPickUpCity 
	,'' AS ToPickUpCountry 
	,'' AS ToPickUpState_Province 
	,'' AS ToSecondLastName 
	,'' AS ToState_Province 
	,'' AS ToZipCode 
	,'' AS TransferType 
	,'BillPay' AS [Type]
FROM   
	tTxn_BillPay tB  
	LEFT JOIN tWUnion_BillPay_Trx sBW ON sBW.WUBillPayTrxID= tB.CXNId AND tB.ProviderId = 401 
	INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = tB.TransactionId
	INNER JOIN tshoppingcarts sc on sc.CartId = sct.CartId
WHERE   
	 sct.CartId = @CartId AND sct.CartItemStatus = 0

UNION ALL

SELECT   
	tMT.TransactionID AS Id
	,'' AS AccountNumber 
	,'' AS AliasId 
	,CASE  tMT.TransferType 
			WHEN 1 THEN CAST( OriginatorsPrincipalAmount AS MONEY)
			ELSE CAST( DestinationPrincipalAmount AS MONEY)
		END AS Amount
	,'' AS CardNumber 
	,'' AS CashType 
	,'' AS CheckNumber 
	,'' AS CheckType 
	,'' AS ConfirmationNumber 
	,CASE  tMT.TransferType 
				WHEN 1 THEN Fee
				ELSE 0
			END AS Fee
	,CASE  tMT.TransferType 
				WHEN 1 THEN CAST( GrossTotalAmount AS MONEY)
				ELSE CAST( AmountToReceiver AS MONEY)
			END AS GrossTotalAmount
	,CAST(0 AS BIT) AS isActivate
	,sW.Mtcn
	,'' AS Payee 
	,tMT.State AS [Status] 
	,wr.Address AS ToAddress
	,wr.City AS ToCity
	,'' AS ToCountry 
	,'' AS ToCountryOfBirth 
	,DeliveryServiceName AS ToDeliveryMethod
	,DeliveryServiceName AS ToDeliveryOption
	,wr.FirstName AS ToFirstName
	,wr.LastName AS ToLastName
	,wr.PhoneNumber AS ToPhoneNumber
	,wr.PickupCity AS ToPickUpCity
	,WR.Country AS ToPickUpCountry
	,ISNULL(wr.[PickupState/Province],ExpectedPayoutStateCode) AS ToPickUpState_Province
	,wr.SecondLastName AS ToSecondLastName
	,wr.[State/Province] AS ToState_Province
	,wr.ZipCode AS ToZipCode
	,CASE  tMT.TransferType 
		WHEN 1 THEN 'Send'			
		ELSE 'Receive'
		END AS  TransferType
	,'MoneyTransfer' AS [Type]
FROM   
	tTxn_MoneyTransfer tMT  
	LEFT JOIN tWUnion_Trx sW on tMT.CXNId = sW.WUTrxId AND tMT.ProviderId = 301 
	LEFT JOIN tWunion_Receiver wr on wr.WUReceiverID = sW.WUReceiverID
	INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = tMT.TransactionId
	INNER JOIN tshoppingcarts sc on sc.CartId = sct.CartId
WHERE   
	 sct.CartId = @CartId AND sct.CartItemStatus = 0

UNION ALL  
  
SELECT   
	tCA.TransactionID AS Id
	,'' AS AccountNumber 
	,'' AS AliasId 
	,Amount
	,'' AS CardNumber 
	,CASE tCA.CashType   
		WHEN 1 THEN 'CashIn'   
		WHEN 2 THEN 'CashOut'   
	END AS CashType  
	,'' AS CheckNumber 
	,'' AS CheckType 
	,'' AS ConfirmationNumber 
	,ISNULL(FEE,0) AS Fee
	,CASE tCA.CashType   
		WHEN 1 THEN CAST((Amount - ISNULL(FEE,0)) AS MONEY) 
		ELSE CAST((Amount + ISNULL(FEE,0)) AS MONEY)
		END AS GrossTotalAmount
	,CAST(0 AS BIT) AS isActivate
	,'' AS MTCN 
	,'' AS Payee 
	,tCA.State AS [Status]
	,'' AS ToAddress 
	,'' AS ToCity 
	,'' AS ToCountry 
	,'' AS ToCountryOfBirth 
	,'' AS ToDeliveryMethod 
	,'' AS ToDeliveryOption 
	,'' AS ToFirstName 
	,'' AS ToLastName 
	,'' AS ToPhoneNumber 
	,'' AS ToPickUpCity 
	,'' AS ToPickUpCountry 
	,'' AS ToPickUpState_Province 
	,'' AS ToSecondLastName 
	,'' AS ToState_Province 
	,'' AS ToZipCode 
	,'' AS  TransferType
	,'Cash' AS [Type]
FROM   
	tTxn_Cash tCA  
	INNER JOIN tShoppingCartTransactions sct ON sct.TransactionId = tCA.TransactionId
	INNER JOIN tshoppingcarts sc on sc.CartId = sct.CartId
WHERE   
	 sct.CartId = @CartId AND sct.CartItemStatus = 0

END