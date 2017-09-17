-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetCountries'
)
BEGIN
	DROP PROCEDURE usp_GetCountries
END
GO

CREATE PROCEDURE usp_GetCountries
AS
BEGIN
	BEGIN TRY

		DECLARE @countriesOrderList table(Name nvarchar(250))
		
		INSERT INTO 
			@countriesOrderList (Name) 
		VALUES 
			('UNITED STATES'),('CANADA'),('MEXICO')
		
		 
		INSERT INTO @countriesOrderList (Name) 
			SELECT 
				DISTINCT Name
			FROM 
				tCountries c WITH (NOLOCK)
			WHERE 
				c.Name NOT IN(SELECT * FROM @countriesOrderList)
			ORDER BY 
				Name

		SELECT 
			Name 
		FROM 
			@countriesOrderList
		
	END TRY
	BEGIN CATCH	        

      EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END