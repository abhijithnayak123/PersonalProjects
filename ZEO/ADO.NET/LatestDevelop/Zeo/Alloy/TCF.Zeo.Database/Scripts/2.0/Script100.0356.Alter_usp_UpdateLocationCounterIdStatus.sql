--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-02-2016>
-- Description:	Update the Counter Id Status. 
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'Alter_usp_UpdateLocationCounterIdStatus', N'P') IS NOT NULL
DROP PROC Alter_usp_UpdateLocationCounterIdStatus
GO


CREATE PROCEDURE Alter_usp_UpdateLocationCounterIdStatus
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
	
UPDATE lc
	SET lc.IsAvailable = @isAvailable, lc.DTTerminalLastModified = @dtTerminalLastModified
	FROM tLocationCounterIdDetails lc
		INNER JOIN tLocations l ON lc.LocationId = l.LocationID
	WHERE l.LocationID = @locationId AND lc.ProviderId = @providerId AND lc.CounterId = @counterId
    
END TRY
BEGIN CATCH
   
	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
