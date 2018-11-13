--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

/****** Object:  View [dbo].[vAcceptedIdentifications]    Script Date: 5/4/2015 10:40:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER view [dbo].[vAcceptedIdentifications]
as
select a.AcceptedIdentificationID, a.CountryCode, a.StatePK, a.Name, a.Mask, a.HasExpirationDate, c.Name as Country, s.Name as State
from tAcceptedIdentifications a
join tCountries c on a.CountryCode = c.Code
left outer join tStates s on a.StatePK = s.StatePK
GO


