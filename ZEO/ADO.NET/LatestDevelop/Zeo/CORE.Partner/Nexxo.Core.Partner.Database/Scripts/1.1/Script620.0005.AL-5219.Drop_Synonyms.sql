-- =======================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <02/26/2016>
-- Description:	<As Alloy we need a consolidated database>
-- Rally ID:	<AL-5219>
-- =====================================================================

 IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sCustomer')
 DROP SYNONYM [dbo].[sCustomer]
 GO

 IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'stxn_Check_Stage')
 DROP SYNONYM [dbo].[stxn_Check_Stage]
 GO