--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Checking for the Fraud Limit based on the Country Code.
-- Story Id   :  B-13688
-- ================================================================================


IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_GetFraudLimit')
BEGIN
	DROP PROCEDURE usp_GetFraudLimit
END 
GO

CREATE PROCEDURE usp_GetFraudLimit
(
	@countryCode VARCHAR(20)
)
AS
BEGIN

	SELECT
		FraudLimit AS FraudLimit
	FROM
		tWUnion_CountryCurrencies WITH(NOLOCK)
	WHERE 
		CountryCode = @countryCode

END