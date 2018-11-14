if exists(select * from sys.objects where name = 'vAgentSessions')
	drop view [dbo].[vAgentSessions]
	GO

create view [dbo].[vAgentSessions]
as
select cp.Name as ChannelPartner, cp.Id as ChannelPartnerId, s.Id as AgentSessionId, s.AgentId, a.UserName as AgentUserName, 
a.FullName as AgentName, t.Name as TerminalName, t.Id as TerminalId, l.LocationName, l.Id as LocationId
from tAgentSessions s
join tAgentDetails a on s.AgentId = a.Id
join tChannelPartners cp on a.ChannelPartnerId = cp.Id
left outer join tTerminals t
	join tLocations l on t.LocationPK = l.rowguid
 on s.TerminalPK = t.rowguid
GO
