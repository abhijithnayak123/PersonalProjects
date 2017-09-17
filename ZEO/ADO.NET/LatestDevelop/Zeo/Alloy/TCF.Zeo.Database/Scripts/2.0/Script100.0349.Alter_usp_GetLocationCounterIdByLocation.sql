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
	
	SELECT TOP 1 CounterId
	FROM tLocationCounterIdDetails 
	WHERE LocationID = @locationId AND ProviderId = @providerId AND IsAvailable = 1 
    
END TRY
BEGIN CATCH


	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
