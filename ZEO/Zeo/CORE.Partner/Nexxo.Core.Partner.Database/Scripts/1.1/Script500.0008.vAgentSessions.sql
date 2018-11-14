--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update vAgentSessions view>           
-- Jira ID:	<AL-242>
--===========================================================================================

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP VIEW [dbo].[vAgentSessions]
GO

Create VIEW [dbo].[vAgentSessions]
AS
SELECT cp.Name AS ChannelPartner, cp.ChannelPartnerId AS ChannelPartnerId, s.AgentSessionID AS AgentSessionId, s.AgentId, a.UserName AS AgentUserName, 
a.FullName AS AgentName, t.Name AS TerminalName, t.TerminalID AS TerminalId, l.LocationName, l.LocationID AS LocationId
FROM tAgentSessions s
JOIN tAgentDetails a ON s.AgentId = a.AgentID
JOIN tChannelPartners cp ON a.ChannelPartnerId = cp.ChannelPartnerId
LEFT OUTER JOIN tTerminals t
	JOIN tLocations l ON t.LocationPK = l.LocationPK
 ON s.TerminalPK = t.TerminalPK
GO
