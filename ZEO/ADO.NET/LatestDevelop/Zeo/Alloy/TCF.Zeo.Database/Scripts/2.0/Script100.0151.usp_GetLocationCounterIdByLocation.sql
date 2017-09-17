--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-02-2016>
-- Description:	Get the Counter Id from location. 
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_GetLocationCounterIdByLocation', N'P') IS NOT NULL
DROP PROC usp_GetLocationCounterIdByLocation
GO


CREATE PROCEDURE usp_GetLocationCounterIdByLocation
(
    @locationId BIGINT
	,@providerId BIGINT
)
AS
BEGIN
	
BEGIN TRY
	
	--TODO - Abhi - Need to work on this when location module is changed. Now we are joining on LocationId which is a
	-- unique identifier in table - "tLocationCounterIdDetails". Once after location module is implemented, no need to 
	-- join to the tLocations table, directly we can filter "tLocationCounterIdDetails" table on LocationId.
	SELECT TOP 1 lc.CounterId
	FROM tLocationCounterIdDetails lc
		INNER JOIN tLocations l ON lc.LocationId = l.LocationPK
	WHERE l.LocationID = @locationId AND lc.ProviderId = @providerId AND IsAvailable = 1 
	ORDER BY lc.DTServerLastModified
	
    
END TRY
BEGIN CATCH
    
	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
