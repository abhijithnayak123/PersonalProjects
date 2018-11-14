--- ===============================================================================
-- Author     :	 M.Purna Pushkal
-- Description:  SP to fetch the RBS contact details
-- Creatd Date:  12-02-2018
-- Story Id   :  B-12630
-- ================================================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE name = 'usp_GetSupportInformation')
BEGIN
	DROP PROCEDURE usp_GetSupportInformation 
END 
GO

CREATE PROCEDURE usp_GetSupportInformation
(
	@locationId BIGINT
)
AS 
BEGIN
	
	SELECT 
		ts.Email,
	    ts.Phone1,
		ts.Phone2,
		tst.Name
	FROM
		tSupportInformation ts WITH (NOLOCK)
		INNER JOIN tLocations tl WITH (NOLOCK) ON tl.State = ts.StateCode AND tl.LocationID = @locationId
		INNER JOIN tStates tst WITH (NOLOCK) ON tst.Abbr = ts.StateCode 
	WHERE tst.CountryCode = 840 --Country code of US

END 