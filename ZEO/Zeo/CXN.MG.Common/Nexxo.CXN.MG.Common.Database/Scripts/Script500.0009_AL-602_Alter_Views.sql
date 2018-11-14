-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- vPastTransaction
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vPastTransaction]'))
	DROP VIEW [dbo].[vPastTransaction]
GO

CREATE VIEW [dbo].[vPastTransaction]  
	AS  
	SELECT 
	tM.MGTransferTrxID AS Id,
	tR.FirstName As ReceiverFirstName,
	tR.LastName As ReceiverLastName,
	tS.Name AS StateName,
	tC.Name AS CountryName,
	NULL AS BillerName,
	NULL AS BillerCode,
	NULL AS AccountNumber,
	'MoneyTransfer' as TransactionType 
	FROM  tMGram_Transfer_Trx tM
	INNER JOIN dbo.tMGram_Receiver tR ON tM.ReceiverId = tR.MGReceiverID
	LEFT JOIN dbo.tMGram_Countries tC ON tM.DestinationCountry = tC.Code
	LEFT JOIN dbo.tMGram_States tS ON tM.DestinationCountry = tS.CountryCode and tM.DestinationState=tS.Code
	WHERE   
		tM.DTTerminalCreate > DATEADD(d, -180, GETDATE()) 
	
	UNION ALL

	SELECT 
	tB.MGBillPayTrxID AS Id,
	NULL As ReceiverFirstName,
	NULL As ReceiverLastName,
	NULL AS StateName,
	NULL AS CountryName,
	tB.BillerName AS BillerName,
	tB.ReceiveCode AS BillerCode,
	tB.AccountNumber AS AccountNumber,
	'BillPay' as TransactionType 
	FROM  tMGram_BillPay_Trx tB WHERE   
		tB.DTTerminalCreate > DATEADD(d, -180, GETDATE())
GO

-- vTransactionHistory
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vTransactionHistory]'))
DROP VIEW [dbo].[vTransactionHistory]
GO

CREATE VIEW [dbo].[vTransactionHistory]
AS

SELECT	tCT.ChxrTrxPK AS rowguid, 
		tCT.ChxrTrxID AS CxnId,
		2 AS TxnType,
		tCTM.Name AS AddlInfo1,
		tca.ChxrAccountID AS CXNAccountId,
		200 as ProviderId,
		tCT.ChannelPartnerID AS ChannelPartnerId
FROM	tChxr_Trx tCT
		INNER JOIN tChxr_CheckTypeMapping tCTM ON tCT.ReturnType = tCTM.ChexarTypePK
		INNER JOIN tChxr_Account tCA ON tca.ChxrAccountPK = tct.ChxrAccountPK
WHERE	tCT.DTTerminalCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT	tFT.FViewTrxPK AS rowguid,
		tFT.FViewTrxID AS CxnId,
		3 AS TxnType, 
		'**** ' + tFC.CardNumber AddlInfo1,
		tFC.FViewCardID AS CXNAccountId,
		101 as ProviderId,
		tFT.ChannelPartnerID AS ChannelPartnerId
FROM	tFView_Trx tFT
		JOIN tFView_Card tFC ON tFT.FViewCardPK = tFC.FViewCardPK 
		WHERE	tFT.DTTerminalCreate > DATEADD(d, -90, GETDATE())
		
UNION ALL

SELECT  tFT.TSysTrxPK AS rowguid, 
		tFT.TSysTrxID AS CxnId, 
		3 AS TxnType,
		'****' + substring(tFC.CardNumber,len(tFC.CardNumber)-3,len(ltrim(rtrim(tFC.CardNumber)))) AddlInfo1,
		tfc.TSysAccountID AS CXNAccountId,
		600 as ProviderId,
		tFT.ChannelPartnerID AS ChannelPartnerId
FROM    tTSys_Trx tFT JOIN
        tTSys_Account tFC ON tFT.TSysAccountPK = tFC.TSysAccountPK
		WHERE  tFT.DTTerminalCreate > DATEADD(d, - 90, GETDATE())
GO
