--===========================================================================================
-- Author:		Rogy Eapen
-- Create date: Mar 04 2015
-- Description:	<Script for sODS_tAccounts>
-- Jira ID:	AL-123
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sODS_tAccounts')
DROP SYNONYM [dbo].[sODS_tAccounts]
GO
CREATE SYNONYM [dbo].[sODS_tAccounts] FOR [$PTNRDATABASE$].[dbo].[tAccounts]
GO