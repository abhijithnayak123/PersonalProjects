--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06-25-2018>
-- Description:	 Getting the list of Blocked Countries.
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetBlockedUnBlockedCountries'
)
BEGIN
	DROP PROCEDURE usp_GetBlockedUnBlockedCountries
END
GO

CREATE PROCEDURE usp_GetBlockedUnBlockedCountries
AS
BEGIN
BEGIN TRY
	--Getting the UnBlocked Countries
	EXECUTE usp_GetUnBlockedCountries

	--Getting the Blocked Countries
	EXECUTE usp_GetBlockedCountries
END TRY
BEGIN CATCH
	-- Execute error retrieval routine.  
	EXECUTE usp_CreateErrorInfo;  
END CATCH
END