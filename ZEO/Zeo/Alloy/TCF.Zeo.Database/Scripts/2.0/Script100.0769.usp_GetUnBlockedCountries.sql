--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06-25-2018>
-- Description:	 Getting the list of UnBlocked Countries.
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetUnBlockedCountries'
)
BEGIN
	DROP PROCEDURE usp_GetUnBlockedCountries
END
GO

CREATE PROCEDURE usp_GetUnBlockedCountries
AS
BEGIN
BEGIN TRY
		DECLARE @countriesOrderList table(ISOCountryCode nvarchar(20), Name nvarchar(250))
				
		INSERT INTO 
			@countriesOrderList (ISOCountryCode, Name) 
		VALUES 
			('US', 'United States'),('CA', 'Canada'),('MX', 'Mexico')

		INSERT INTO @countriesOrderList (ISOCountryCode, Name) 
		SELECT 
			DISTINCT twc.ISOCountryCode,twc.Name
		FROM 
			tWUnion_Countries twc WITH (NOLOCK)
			LEFT JOIN tBlockedCountries tbc WITH (NOLOCK) ON tbc.ISOCountryCode = twc.ISOCountryCode
		WHERE
			 tbc.ISOCountryCode IS NULL AND
			 twc.Name NOT IN(SELECT Name FROM @countriesOrderList)
			 AND twc.ISOCountryCode NOT IN(SELECT ISOCountryCode FROM @countriesOrderList)
		ORDER BY 
			Name


		SELECT 
			Name AS CountryName, ISOCountryCode AS ISOCountryCode
		FROM 
			@countriesOrderList
END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END