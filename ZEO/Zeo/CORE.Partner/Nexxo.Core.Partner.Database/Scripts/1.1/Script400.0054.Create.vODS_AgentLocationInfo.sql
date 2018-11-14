--===========================================================================================
-- Author:		Rogy Eapen
-- Create date: Mar 04 2015
-- Description:	<Script for vODS_AgentLocationInfo>
-- Jira ID:	AL-123
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vODS_AgentLocationInfo]'))
DROP VIEW [dbo].[vODS_AgentLocationInfo]
GO
CREATE VIEW [dbo].[vODS_AgentLocationInfo]
AS
SELECT     tLocations.LocationName, tAgentSessions.AgentId AS TellerUserId, tAgentDetails.UserName AS TellerUserName, 
tPartnerCustomers.CXEId AS PartnerCustomer_CXEId, tLocations.Id AS LocationId, tPartnerCustomers.rowguid AS PartnerCustomers_rowguid
FROM tPartnerCustomers
LEFT OUTER JOIN tAgentSessions ON tPartnerCustomers.AgentSessionId = tAgentSessions.rowguid 
LEFT OUTER JOIN tAgentDetails ON tAgentSessions.AgentId = tAgentDetails.Id 
LEFT OUTER JOIN tTerminals ON tAgentSessions.TerminalPK = tTerminals.rowguid 
LEFT OUTER JOIN tLocations ON tTerminals.LocationPK = tLocations.rowguid
GO
