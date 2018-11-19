SET NOCOUNT ON
-- TBL_OF_CountryLookup
SET IDENTITY_INSERT TBL_OF_CountryLookup ON

INSERT INTO TBL_OF_CountryLookup (
	SL_NO
	,TIAA_Country_Code
	,TIAA_Country_Name
	,InnoCountryCode
	,InnoCountryName
	)
SELECT SL_NO
	,TIAA_Country_Code
	,TIAA_Country_Name
	,EX_Country_Code
	,EX_Country_Name
FROM $(ExcelsiorDB)..TBL_OFAC_COUNTRY_LOOKUP

SET IDENTITY_INSERT TBL_OF_CountryLookup OFF

SET NOCOUNT OFF