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
	SELECT 
		twc.Name AS CountryName,
		twc.ISOCountryCode
	FROM 
		tWUnion_Countries twc WITH (NOLOCK)
		LEFT JOIN tBlockedCountries tbc WITH (NOLOCK) ON tbc.ISOCountryCode = twc.ISOCountryCode
	WHERE 
		tbc.ISOCountryCode IS NULL
	ORDER BY
		CASE 
		WHEN twc.ISOCountryCode = 'US' THEN 0
		WHEN twc.ISOCountryCode = 'CA' THEN 1
		WHEN twc.ISOCountryCode = 'MX' THEN 2
		ELSE 3 END
END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END