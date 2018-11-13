IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sTransHistory')
 DROP SYNONYM [dbo].[sTransHistory]
 GO
 CREATE SYNONYM [dbo].[sTransHistory] FOR [$CXNDATABASE$].[dbo].[vTransactionHistory]
 GO

 IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sCustomer')
 DROP SYNONYM [dbo].[sCustomer]
 GO
 CREATE SYNONYM [dbo].[sCustomer]  FOR [$CXEDATABASE$].[dbo].[tCustomers]
 GO
