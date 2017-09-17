--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

/****** Object:  View [dbo].[vCustomerSessions]    Script Date: 3/27/2015 1:47:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vCustomerSessions]
AS
SELECT cp.Name AS ChannelPartner, cp.ChannelPartnerId AS ChannelPartnerId, c.CustomerSessionID AS CustomerSessionId, p.CXEId AS CustomerId, cxe.FirstName AS CustomerFirstName, cxe.LastName AS CustomerLastName, c.DTStart, c.DTEnd, s.AgentSessionID AS AgentSessionId,
s.AgentId, a.UserName AS AgentUserName, a.FullName AS AgentName, t.Name AS TerminalName, t.TerminalID AS TerminalId, l.LocationName, l.LocationID AS LocationId
FROM tCustomerSessions c
JOIN tPartnerCustomers p ON c.CustomerPK = p.CustomerPK
JOIN sCustomer cxe ON p.CXEId = cxe.CustomerId
JOIN tAgentSessions s ON c.AgentSessionPK = s.AgentSessionPK
JOIN tAgentDetails a ON s.AgentId = a.AgentID
JOIN tTerminals t ON s.TerminalPK = t.TerminalPK
JOIN tChannelPartners cp ON t.ChannelPartnerPK = cp.ChannelPartnerPK
JOIN tLocations l ON t.LocationPK = l.LocationPK
GO


