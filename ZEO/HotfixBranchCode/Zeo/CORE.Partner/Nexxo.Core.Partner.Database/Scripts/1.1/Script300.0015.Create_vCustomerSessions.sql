if exists(select * from sys.objects where name = 'vCustomerSessions')
	drop view [dbo].[vCustomerSessions]
	GO

create view [dbo].[vCustomerSessions]
as
select cp.Name as ChannelPartner, cp.Id as ChannelPartnerId, c.Id as CustomerSessionId, p.CXEId as CustomerId, cxe.FirstName as CustomerFirstName, cxe.LastName as CustomerLastName, c.DTStart, c.DTEnd, s.Id as AgentSessionId,
s.AgentId, a.UserName as AgentUserName, a.FullName as AgentName, t.Name as TerminalName, t.Id as TerminalId, l.LocationName, l.Id as LocationId
from tCustomerSessions c
join tPartnerCustomers p on c.CustomerPK = p.rowguid
join sCustomer cxe on p.CXEId = cxe.Id
join tAgentSessions s on c.AgentSessionPK = s.rowguid
join tAgentDetails a on s.AgentId = a.Id
join tTerminals t on s.TerminalPK = t.rowguid
join tChannelPartners cp on t.ChannelPartnerPK = cp.rowguid
join tLocations l on t.LocationPK = l.rowguid
GO
