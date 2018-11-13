--===========================================================================================
-- Author:		Rogy Eapen
-- Create date: Mar 05 2015
-- Description:	<Script for sODS_AgentLocationInfo>
-- Jira ID:	AL-123
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sODS_AgentLocationInfo')
DROP SYNONYM [dbo].[sODS_AgentLocationInfo]
GO
CREATE SYNONYM [dbo].[sODS_AgentLocationInfo] FOR [$PTNRDATABASE$].[dbo].[vODS_AgentLocationInfo]
GO