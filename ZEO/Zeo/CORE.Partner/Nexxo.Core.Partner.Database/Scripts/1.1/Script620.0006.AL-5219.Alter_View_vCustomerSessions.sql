-- =======================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <02/26/2016>
-- Description:	<As Alloy we need a consolidated database>
-- Rally ID:	<AL-5219>
-- =====================================================================
/****** Object:  View [dbo].[vCustomerSessions]    Script Date: 3/02/2016 16:32:27 ******/
IF EXISTS (select 1 from sys.views where name = N'vCustomerSessions')
	DROP VIEW [dbo].[vCustomerSessions]
GO

/****** Object:  View [dbo].[vCustomerSessions]    Script Date: 3/02/2016 16:32:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vCustomerSessions]
AS
SELECT cp.Name AS ChannelPartner, cp.ChannelPartnerId AS ChannelPartnerId, c.CustomerSessionID AS CustomerSessionId, p.CXEId AS CustomerId, cxe.FirstName AS CustomerFirstName, cxe.LastName AS CustomerLastName, c.DTStart, c.DTEnd, s.AgentSessionID AS AgentSessionId,
s.AgentId, a.UserName AS AgentUserName, a.FullName AS AgentName, t.Name AS TerminalName, t.TerminalID AS TerminalId, l.LocationName, l.LocationID AS LocationId
FROM tCustomerSessions c
JOIN tPartnerCustomers p ON c.CustomerPK = p.CustomerPK
JOIN tCustomers cxe ON p.CXEId = cxe.CustomerId
JOIN tAgentSessions s ON c.AgentSessionPK = s.AgentSessionPK
JOIN tAgentDetails a ON s.AgentId = a.AgentID
JOIN tTerminals t ON s.TerminalPK = t.TerminalPK
JOIN tChannelPartners cp ON t.ChannelPartnerPK = cp.ChannelPartnerPK
JOIN tLocations l ON t.LocationPK = l.LocationPK
GO


