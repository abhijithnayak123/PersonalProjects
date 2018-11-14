--- ===============================================================================
-- Author:		<Kiranmaie>
-- Create date: <11-16-2016>
-- Description:	Get the WU Country Currencies. 
-- Jira ID:		<AL-8325>
-- ================================================================================

IF OBJECT_ID(N'usp_GetWUnionCountryCurrencies', N'P') IS NOT NULL
DROP PROC usp_GetWUnionCountryCurrencies
GO


CREATE PROCEDURE usp_GetWUnionCountryCurrencies
(
    @countryCode VARCHAR(20)
)

AS
BEGIN
	BEGIN TRY

		SELECT 
			[WUCountryCurrencyID] As Id
			,[CountryName]
			,[CountryCode]
			,[CountryNumCode]
			,[CurrencyCode]
			,[CurrencyNumCode]
			,[CurrencyName]	
		FROM 
			tWUnion_CountryCurrencies 
		WHERE 
			CountryCode = @countryCode

	END TRY
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO


