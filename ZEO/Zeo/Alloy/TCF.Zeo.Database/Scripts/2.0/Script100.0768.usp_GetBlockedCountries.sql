--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06-25-2018>
-- Description:	 Getting the list of Blocked Countries.
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetBlockedCountries'
)
BEGIN
	DROP PROCEDURE usp_GetBlockedCountries
END
GO

CREATE PROCEDURE [dbo].[usp_GetBlockedCountries]
AS
BEGIN
	BEGIN TRY

	   SELECT 
			twc.Name AS CountryName,
			twc.ISOCountryCode
	   FROM tBlockedCountries tbc WITH (NOLOCK)
	   INNER JOIN tWUnion_Countries twc WITH (NOLOCK) ON tbc.ISOCountryCode = twc.ISOCountryCode

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END