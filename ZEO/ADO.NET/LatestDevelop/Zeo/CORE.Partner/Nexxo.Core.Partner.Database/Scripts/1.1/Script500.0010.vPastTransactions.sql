--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update vPastTransactions view>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  View [dbo].[vPastTransactions]    Script Date: 3/27/2015 1:54:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
Drop VIEW [dbo].[vPastTransactions]
GO
CREATE VIEW [dbo].[vPastTransactions] AS
SELECT
tPC.CustomerID AS CustomerId,
tC.CXEId AS CXEId,
tC.CXNId AS CXNId,
tC.CustomerSessionPK AS CustomerSessionPK,
tC.Amount AS Amount,
tC.Fee AS Fee,
tC.CXEState AS CXEState,
tC.DTLastMod AS DTLastMod,
tC.ProductId AS ProductId,
tC.TransactionID AS Id,
sP.ReceiverFirstName AS ReceiverFirstName,
sP.ReceiverLastName AS ReceiverLastName,
sP.StateName AS StateName,
sP.CountryName AS CountryName,
sP.BillerName AS BillerName,
sP.BillerCode AS BillerCode,
sP.AccountNumber AS AccountNumber,
sP.TransactionType AS TransactionType
FROM 
tTxn_BillPay tC  
INNER JOIN sPastTransactions sP ON sP.Id = tC.CXNId AND sP.TransactionType='BillPay'
INNER JOIN tAccounts tA ON tC.AccountPK = tA.AccountPK
INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tC.CustomerSessionPK
WHERE tC.CXEState= 4

UNION ALL

SELECT
tPC.CustomerID AS CustomerId,
tM.CXEId AS CXEId,
tM.CXNId AS CXNId,
tM.CustomerSessionPK AS CustomerSessionPK,
tM.Amount AS Amount,
tM.Fee AS Fee,
tM.CXEState AS CXEState,
tM.DTLastMod AS DTLastMod,
NULL AS ProductId,
tM.TransactionID AS Id,
sP.ReceiverFirstName AS ReceiverFirstName,
sP.ReceiverLastName AS ReceiverLastName,
sP.StateName AS StateName,
sP.CountryName AS CountryName,
sP.BillerName AS BillerName,
sP.BillerCode AS BillerCode,
sP.AccountNumber AS AccountNumber,
sP.TransactionType AS TransactionType
FROM 
tTxn_MoneyTransfer tM  
INNER JOIN sPastTransactions sP ON sP.Id = tM.CXNId AND sP.TransactionType='MoneyTransfer'
INNER JOIN tAccounts tA ON tM.AccountPK = tA.AccountPK
INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK   
INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tM.CustomerSessionPK
WHERE tM.CXEState= 4
AND tM.TransactionID NOT IN(SELECT OriginalTransactionID FROM tTxn_MoneyTransfer WHERE TransferType=1)
AND COALESCE(tM.TransactionSubType, 0) <> 1
GO


