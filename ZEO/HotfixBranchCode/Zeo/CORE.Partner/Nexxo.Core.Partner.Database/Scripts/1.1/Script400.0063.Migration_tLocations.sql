--===========================================================================================
-- Author:		<Abhijith>
-- Create date: <Mar 26 2015>
-- Description:	<Data Migration for LocationIdentifier>
-- Jira ID:	    <AL-217>
--===========================================================================================

UPDATE loc
SET LocationIdentifier = locProc.Identifier
FROM tLocations loc
	INNER JOIN tLocationProcessorCredentials locProc ON loc.rowguid = locProc.LocationId
WHERE locProc.ProviderId = 200 --Provider ID of INGO

GO
--===========================================================================================
