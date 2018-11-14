--- ===============================================================================
-- Author     :	 M.Purna Pushkal
-- Modified By : Abhijith
-- Description:  SP to fetch the RBS contact details
-- Modified Description : Get the RBS, IT Support and Provider contact details from the database for Help Section.
-- Creatd Date:  12-02-2018
-- Modified Date : 03/29/2018
-- Story Id   :  B-12630

-- EXEC usp_GetSupportInformation 1000000003
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
       ts.ContactType,
       CASE
           WHEN ts.ContactType = 'RBS' THEN tst.Name
           WHEN ts.ContactType = 'WU' THEN 'Western Union Contact Number'
           WHEN ts.ContactType = 'VISA' THEN 'Visa Customer Contact Number'
           WHEN ts.ContactType = 'ITServiceDesk' THEN 'TCF IT Service Desk'
           WHEN ts.ContactType = 'INGO' THEN 'Ingo'
           WHEN ts.ContactType = 'BSA' THEN 'BSA Contact number'
           ELSE ts.ContactType
        END AS Name
	FROM
		tSupportInformation ts WITH (NOLOCK)
		INNER JOIN tLocations tl WITH (NOLOCK) ON (tl.State = ts.StateCode OR ts.StateCode IS NULL) AND tl.LocationID = @locationId
		INNER JOIN tStates tst WITH (NOLOCK) ON tst.Abbr = tl.State 
	WHERE tst.CountryCode = 840 --Country code of US
		
END 