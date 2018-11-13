-- ============================================================
-- Author:        <Ashok Kumar G>
-- Create date:   <4/17/2015>
-- Description:   <Alter vODS_AgentLocationInfo to add ClientAgentIdentifier, AlloyAgentIdentifier for ODS> 
-- Rally ID:      <DE3430 – TA6489>
-- ============================================================
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE NAME = N'vODS_AgentLocationInfo' AND TYPE = N'V')
BEGIN
DROP VIEW vODS_AgentLocationInfo
END
GO

CREATE VIEW vODS_AgentLocationInfo
AS
SELECT 
	tLocations.LocationName, 
	tAgentSessions.AgentId AS AlloyAgentIdentifier, 
	tAgentDetails.UserName AS TellerUserName, 
	tAgentDetails.ClientAgentIdentifier AS ClientAgentIdentifier,
	tPartnerCustomers.CXEId AS PartnerCustomer_CXEId, 
	tLocations.Id AS LocationId, 
	tPartnerCustomers.rowguid AS PartnerCustomers_rowguid
FROM tPartnerCustomers
LEFT OUTER JOIN tAgentSessions ON tPartnerCustomers.AgentSessionId = tAgentSessions.rowguid 
LEFT OUTER JOIN tAgentDetails ON tAgentSessions.AgentId = tAgentDetails.Id 
LEFT OUTER JOIN tTerminals ON tAgentSessions.TerminalPK = tTerminals.rowguid 
LEFT OUTER JOIN tLocations ON tTerminals.LocationPK = tLocations.rowguid
GO