-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetOccupations'
)
BEGIN
	DROP PROCEDURE usp_GetOccupations
END
GO

CREATE PROCEDURE usp_GetOccupations
AS
BEGIN
	BEGIN TRY
		SELECT 
			OccupationsID,Code,Name 
		FROM 
			tOccupations
		WHERE 
			IsActive = 1 
		ORDER BY 
			NAME ASC		
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END