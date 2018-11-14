-- =======================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <02/26/2016>
-- Description:	<As Alloy we need a consolidated database>
-- Rally ID:	<AL-5219>
-- =====================================================================

IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sODS_AgentLocationInfo')
DROP SYNONYM [dbo].[sODS_AgentLocationInfo]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sODS_CustomerIdInfo')
DROP SYNONYM [dbo].[sODS_CustomerIdInfo]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sODS_tAccounts')
DROP SYNONYM [dbo].[sODS_tAccounts]
GO