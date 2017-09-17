--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-02-2016>
-- Description:	Get the WU countries. 
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_GetWUCountries', N'P') IS NOT NULL
DROP PROC usp_GetWUCountries
GO


CREATE PROCEDURE usp_GetWUCountries
AS
BEGIN

	BEGIN TRY
		DECLARE @countriesOrderList table(ISOCountryCode nvarchar(20), Name nvarchar(250))
		
		INSERT INTO 
			@countriesOrderList (ISOCountryCode, Name) 
		VALUES 
			('US', 'UNITED STATES'),('CA', 'CANADA'),('MX', 'MEXICO')
		
		 
		INSERT INTO @countriesOrderList (ISOCountryCode, Name) 
			SELECT 
				DISTINCT ISOCountryCode,Name
			FROM 
				tWUnion_Countries c WITH (NOLOCK)
			WHERE 
				c.Name NOT IN(SELECT Name FROM @countriesOrderList)
				 and c.ISOCountryCode NOT IN(SELECT ISOCountryCode FROM @countriesOrderList)
			ORDER BY 
				Name

		SELECT 
			Name AS Name, ISOCountryCode AS Code
		FROM 
			@countriesOrderList
	END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
