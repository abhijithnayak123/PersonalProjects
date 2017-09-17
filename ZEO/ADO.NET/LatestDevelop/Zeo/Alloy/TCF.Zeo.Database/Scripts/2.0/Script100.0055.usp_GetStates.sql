-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetStates'
)
BEGIN
	DROP PROCEDURE usp_GetStates
END
GO

CREATE PROCEDURE usp_GetStates
	@countryCode VARCHAR(50)
AS
BEGIN
	BEGIN TRY
		SELECT 
			DISTINCT abbr 
		FROM 
			tstates WITH (NOLOCK)
		WHERE 
			CountryCode = @countryCode
		ORDER BY 
			Abbr ASC
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END