--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-16-2016>
-- Description:	Get the data for WuCardEnrollment. 
-- Jira ID:		<AL-8325>
-- ================================================================================
IF OBJECT_ID(N'usp_GetEnrollmentRequest', N'P') IS NOT NULL
DROP PROC usp_GetEnrollmentRequest
GO

CREATE PROCEDURE usp_GetEnrollmentRequest
(
	@customerSessionId BIGINT
)
AS
BEGIN
	BEGIN TRY

		-- This was hardcoded in the web layer of Enrollment method. So moved this to SP.
		DECLARE @destinationCountryCode VARCHAR(20) = 'US' 
		DECLARE @destinationCurrencyCode VARCHAR(20) = 'USD' 

		SELECT 
			wc.ISOCountryCode AS originatingCountryCode
			,@destinationCountryCode AS destinationCountryCode
			,@destinationCurrencyCode AS destinationCurrencyCode
			,@destinationCountryCode AS recordingcountrycurrencyCountryCode
			,@destinationCurrencyCode AS recordingcountrycurrencyCurrencyCode
			,c.FirstName AS FirstName
			,c.LastName AS LastName
			,c.Address1 AS [Address]
			,c.City AS City
			,c.[State] AS [State]
			,c.ZipCode AS PostalCode
			,c.Phone1 AS ContactPhone
			,c.Email AS Email
			,CASE
			  WHEN c.Phone1 IS NOT NULL
					 AND c.Phone1Type = 'Cell'
			  THEN c.Phone1
			  WHEN c.Phone2 IS NOT NULL
					 AND c.Phone2Type = 'Cell'
			  THEN c.Phone2
			  ELSE ''
			 END AS MobilePhone
		FROM tCustomerSessions cs
			INNER JOIN tCustomers c ON cs.CustomerID = c.CustomerID
			INNER JOIN tNexxoIdTypes nt ON c.GovtIdTypeId = nt.NexxoIdTypeID 
			INNER JOIN tMasterCountries mc ON nt.MasterCountriesID = mc.MasterCountriesID
			INNER JOIN tWUnion_Countries wc ON wc.Name = mc.Name 
		WHERE cs.CustomerSessionID = @customerSessionId

	END TRY
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO


