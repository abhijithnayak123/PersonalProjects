--- ===============================================================================
-- Author:		<Ashok Kumatr>
-- Create date: <28-03-2017>
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

	SELECT 
		twc.Name AS Name,
		twc.ISOCountryCode AS Code
	FROM 
		tWUnion_Countries twc WITH (NOLOCK)
	ORDER BY
		CASE 
		WHEN twc.ISOCountryCode = 'US' THEN 0
		WHEN twc.ISOCountryCode = 'CA' THEN 1
		WHEN twc.ISOCountryCode = 'MX' THEN 2
		ELSE 3 END, twc.Name ASC

	END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
