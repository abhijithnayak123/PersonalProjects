-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetMasterCountryByAbbr2'
)
BEGIN
	DROP PROCEDURE usp_GetMasterCountryByAbbr2
END
GO

CREATE PROCEDURE usp_GetMasterCountryByAbbr2
	@Abbr2 CHAR(2)
AS
BEGIN
	BEGIN TRY
		SELECT 
			MC.Name,MC.Abbr2,MC.Abbr3 
		FROM 
			tMasterCountries MC 
		WHERE 
			MC.Abbr2 = @Abbr2
		 
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END