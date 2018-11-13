--===========================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <May 11 2015>
-- Description:	<Data Migration for LocationIdentifier>
-- Jira ID:	    <AL-360>
--===========================================================================================

UPDATE loc
SET LocationIdentifier = locProc.UserName
FROM tLocations loc
	INNER JOIN tLocationProcessorCredentials locProc ON loc.rowguid = locProc.LocationId
WHERE locProc.ProviderId = 200 --Provider ID of INGO

GO
--===========================================================================================

