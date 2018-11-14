--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <02-27-2018>
-- Description:	 SP to get the state name and id based on the channel partner Id
-- VersionOne:	<B-13221>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_GetStateNameAndIdByChannelPartnerId')
BEGIN
	DROP PROCEDURE usp_GetStateNameAndIdByChannelPartnerId
END 
GO

CREATE PROCEDURE usp_GetStateNameAndIdByChannelPartnerId
(
	@channelPartnerId smallint
)
AS
BEGIN
	SELECT  
		ts.Name AS stateName,
		tl.LocationName,
		tl.LocationID 
	FROM 
		tLocations tl
	INNER JOIN tStates ts WITH(NOLOCK) ON ts.Abbr = tl.State AND tl.IsActive = 1 AND ts.CountryCode = 840
	WHERE 
		tl.ChannelPartnerId = @channelPartnerId
END 
