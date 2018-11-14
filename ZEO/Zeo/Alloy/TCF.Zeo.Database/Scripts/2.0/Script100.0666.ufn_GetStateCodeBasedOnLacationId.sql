--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <03-27-2018>
-- Description:	 GetCustomerId by Customer Session Id
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('ufn_GetStateCodeBasedOnLacationId') IS NOT NULL
	 BEGIN
		  DROP FUNCTION dbo.ufn_GetStateCodeBasedOnLacationId
	 END
GO

CREATE FUNCTION ufn_GetStateCodeBasedOnLacationId(@locationId BIGINT)
RETURNS NVARCHAR(2)
AS
BEGIN
	 DECLARE @stateCode NVARCHAR(2);

	 SELECT 
		@stateCode = State  
	 FROM 
	 	tLocations 
	 WHERE 
	 	LocationID = @locationId

	 RETURN @stateCode
END

