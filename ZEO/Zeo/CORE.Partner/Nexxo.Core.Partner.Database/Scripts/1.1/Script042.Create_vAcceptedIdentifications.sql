

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vAcceptedIdentifications]') )
DROP VIEW [dbo].[vAcceptedIdentifications]

GO

create view [dbo].[vAcceptedIdentifications]
as
select a.Id, a.CountryCode, a.StateId, a.Name, a.Mask, a.HasExpirationDate, c.Name as Country, s.Name as State
from tAcceptedIdentifications a
join tCountries c on a.CountryCode = c.Code
left outer join tStates s on a.StateId = s.id

GO
