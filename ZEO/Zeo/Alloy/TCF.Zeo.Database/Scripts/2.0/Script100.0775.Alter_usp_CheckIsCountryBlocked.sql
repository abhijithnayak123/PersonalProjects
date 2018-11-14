-- =============================================
-- Author:		<Abhijith>
-- Create date: <06/25/2018>
-- Description:	Checking the Country Code is Blocked in ZEO.
-- =============================================

IF OBJECT_ID(N'usp_CheckIsCountryBlocked', N'P') IS NOT NULL
DROP PROC usp_CheckIsCountryBlocked
GO

CREATE PROCEDURE usp_CheckIsCountryBlocked
	@countryCode VARCHAR(15)
AS
BEGIN

  BEGIN TRY

		SELECT 
			CASE 
				WHEN COUNT(1) > 0 THEN CAST(1 AS BIT)
				ELSE CAST(0 AS BIT)
			END AS IsBlocked
		FROM tBlockedCountries
		WHERE ISOCountryCode = @countryCode

  END TRY
  BEGIN CATCH
    EXECUTE usp_CreateErrorInfo
  END CATCH
  
  	
END
GO


