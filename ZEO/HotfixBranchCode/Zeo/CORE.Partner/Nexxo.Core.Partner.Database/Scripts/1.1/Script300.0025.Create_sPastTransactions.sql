--===========================================================================================
-- Auther:			Rahul k
-- Date Created:	09/24/2014
-- Description:		Create Synonyms sPastTransactions 
--===========================================================================================
 
 
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sPastTransactions')
DROP SYNONYM [dbo].[sPastTransactions]
GO
CREATE SYNONYM [dbo].[sPastTransactions] FOR [$CXNDATABASE$].[dbo].[vPastTransaction]
GO