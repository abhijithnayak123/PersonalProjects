-- ================================================================================
-- Author:		<kiranmaie Munagapati>
-- Create date: <11/29/2016>
-- Description:	<As an engineer, I want to implement ADO.Net for SendMoney module>
-- Jira ID:		<AL-8325>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetWUcities'
)
BEGIN
	DROP PROCEDURE usp_GetWUcities
END
GO

CREATE PROCEDURE usp_GetWUcities
	@stateCode VARCHAR(50)
AS
BEGIN
	BEGIN TRY
		SELECT 
			tc.WUCityID AS Id
			,tc.Name AS Name
		FROM 
			tWUnion_Cities  tc WITH (NOLOCK)
		WHERE 
			StateCode = @stateCode
	
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END