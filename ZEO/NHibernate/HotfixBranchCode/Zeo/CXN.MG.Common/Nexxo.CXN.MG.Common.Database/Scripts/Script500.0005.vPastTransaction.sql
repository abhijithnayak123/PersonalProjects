--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
		tM.DTCreate > DATEADD(d, -180, GETDATE()) 
	
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
		tB.DTCreate > DATEADD(d, -180, GETDATE())
GO


