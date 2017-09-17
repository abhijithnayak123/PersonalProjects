-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetLegalCodes'
)
BEGIN
	DROP PROCEDURE usp_GetLegalCodes
END
GO

CREATE PROCEDURE usp_GetLegalCodes
AS
BEGIN
	BEGIN TRY
		SELECT 
			LegalCodesID,Code,Name 
		FROM 
			tLegalCodes
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END