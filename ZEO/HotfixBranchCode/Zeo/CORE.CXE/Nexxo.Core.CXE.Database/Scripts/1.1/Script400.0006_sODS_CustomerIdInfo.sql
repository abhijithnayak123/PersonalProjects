--===========================================================================================
-- Author:		Rogy Eapen
-- Create date: Mar 05 2015
-- Description:	<Script for sODS_CustomerIdInfo>
-- Jira ID:	AL-123
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sODS_CustomerIdInfo')
DROP SYNONYM [dbo].[sODS_CustomerIdInfo]
GO
CREATE SYNONYM [dbo].[sODS_CustomerIdInfo] FOR [$PTNRDATABASE$].[dbo].[vODS_CustomerIdInfo]
GO
