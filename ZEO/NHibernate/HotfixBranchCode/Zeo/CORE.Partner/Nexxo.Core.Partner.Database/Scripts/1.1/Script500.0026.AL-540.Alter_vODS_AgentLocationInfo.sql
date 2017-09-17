--===========================================================================================
-- Author:		<Ashok Kumar G>
-- Created date: <May 21th 2015>
-- Description:	<Script to alter vODS_AgentLocationInfo>           
-- Jira ID:	<AL-540>
--===========================================================================================

DROP VIEW [dbo].[vODS_AgentLocationInfo]
GO

CREATE VIEW [dbo].[vODS_AgentLocationInfo]
AS
SELECT 
	tLocations.LocationName, 
	tAgentSessions.AgentId AS AlloyAgentIdentifier, 
	tAgentDetails.UserName AS TellerUserName, 
	tAgentDetails.ClientAgentIdentifier AS ClientAgentIdentifier,
	tPartnerCustomers.CXEId AS PartnerCustomer_CXEId, 
	tLocations.LocationID AS LocationId, 
	tPartnerCustomers.CustomerPK AS PartnerCustomers_rowguid
FROM tPartnerCustomers
LEFT OUTER JOIN tAgentSessions ON tPartnerCustomers.AgentSessionPK = tAgentSessions.AgentSessionPK 
LEFT OUTER JOIN tAgentDetails ON tAgentSessions.AgentId = tAgentDetails.AgentID 
LEFT OUTER JOIN tTerminals ON tAgentSessions.TerminalPK = tTerminals.TerminalPK 
LEFT OUTER JOIN tLocations ON tTerminals.LocationPK = tLocations.LocationPK
GO


