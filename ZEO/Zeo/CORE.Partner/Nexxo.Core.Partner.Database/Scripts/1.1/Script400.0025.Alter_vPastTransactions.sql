--===========================================================================================
-- Auther:			Rahul K
-- Date Created:	12/1/2015
-- Description:		Alter View vPastTransactions 
--===========================================================================================


IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vPastTransactions]'))
DROP VIEW [dbo].[vPastTransactions]
GO



CREATE VIEW [dbo].[vPastTransactions] AS
SELECT
tPC.id AS CustomerId,
tC.CXEId AS CXEId,
tC.CXNId AS CXNId,
tC.CustomerSessionPK AS CustomerSessionPK,
tC.Amount AS Amount,
tC.Fee AS Fee,
tC.CXEState AS CXEState,
tC.DTLastMod AS DTLastMod,
tC.ProductId AS ProductId,
tC.Id AS Id,
sP.ReceiverFirstName As ReceiverFirstName,
sP.ReceiverLastName As ReceiverLastName,
sP.StateName AS StateName,
sP.CountryName AS CountryName,
sP.BillerName As BillerName,
sP.BillerCode AS BillerCode,
sP.AccountNumber AS AccountNumber,
sP.TransactionType as TransactionType
FROM 
tTxn_BillPay tC  
INNER JOIN sPastTransactions sP ON sP.Id = tC.CXNId and sP.TransactionType='BillPay'
INNER JOIN tAccounts tA ON tC.AccountPK = tA.rowguid
INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tC.CustomerSessionPK
WHERE tC.CXEState= 4

UNION ALL

SELECT
tPC.id AS CustomerId,
tM.CXEId AS CXEId,
tM.CXNId AS CXNId,
tM.CustomerSessionPK AS CustomerSessionPK,
tM.Amount AS Amount,
tM.Fee AS Fee,
tM.CXEState AS CXEState,
tM.DTLastMod AS DTLastMod,
NULL AS ProductId,
tM.Id AS Id,
sP.ReceiverFirstName As ReceiverFirstName,
sP.ReceiverLastName As ReceiverLastName,
sP.StateName AS StateName,
sP.CountryName AS CountryName,
sP.BillerName As BillerName,
sP.BillerCode AS BillerCode,
sP.AccountNumber AS AccountNumber,
sP.TransactionType as TransactionType
FROM 
tTxn_MoneyTransfer tM  
INNER JOIN sPastTransactions sP ON sP.Id = tM.CXNId and sP.TransactionType='MoneyTransfer'
INNER JOIN tAccounts tA ON tM.AccountPK = tA.rowguid
INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK   
INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tM.CustomerSessionPK
WHERE tM.CXEState= 4
and tM.id not in(select OriginalTransactionID from tTxn_MoneyTransfer where TransferType=1)
and COALESCE(tM.TransactionSubType, 0) <> 1
GO
