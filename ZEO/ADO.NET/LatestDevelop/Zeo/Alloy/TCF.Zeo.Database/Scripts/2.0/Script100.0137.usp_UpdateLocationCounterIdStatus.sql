--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-02-2016>
-- Description:	Update the Counter Id Status. 
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateLocationCounterIdStatus', N'P') IS NOT NULL
DROP PROC usp_UpdateLocationCounterIdStatus
GO


CREATE PROCEDURE usp_UpdateLocationCounterIdStatus
(
    @locationId BIGINT
	,@providerId BIGINT
	,@counterId VARCHAR(50)
	,@isAvailable BIT = 0
	,@dtTerminalLastModified DATETIME
)
AS
BEGIN
	
BEGIN TRY
	
	--TODO - Abhi - Need to work on this when location module is changed. Now we are joining on LocationId which is a
	-- unique identifier in table - "tLocationCounterIdDetails". Once after location module is implemented, no need to 
	-- join to the tLocations table, directly we can filter "tLocationCounterIdDetails" table on LocationId.
	UPDATE lc
	SET lc.IsAvailable = @isAvailable, lc.DTTerminalLastModified = @dtTerminalLastModified
	FROM tLocationCounterIdDetails lc
		INNER JOIN tLocations l ON lc.LocationId = l.LocationPK
	WHERE l.LocationID = @locationId AND lc.ProviderId = @providerId AND lc.CounterId = @counterId
    
END TRY
BEGIN CATCH
    
	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
