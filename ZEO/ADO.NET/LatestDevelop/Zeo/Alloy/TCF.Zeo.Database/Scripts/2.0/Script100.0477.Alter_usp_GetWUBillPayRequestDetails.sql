--- ===============================================================================
-- Author:		<KARUN>
-- Create date: <07-APR-2017>
-- Description:	Get WURequestDetails
-- Jira ID:		<ALM-5233>

-- EXEC usp_GetWUBillPayRequestDetails 1000000011, 10000122
-- ================================================================================

IF OBJECT_ID('usp_GetWUBillPayRequestDetails') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_GetWUBillPayRequestDetails
END
GO

CREATE PROCEDURE usp_GetWUBillPayRequestDetails
(
	@customerId BIGINT,
	@wuBillPayTrxID BIGINT
)
AS
BEGIN

	BEGIN TRY
		DECLARE @referenceNo VARCHAR(50) = ''

		IF(@wuBillPayTrxID != 0)
		BEGIN
			SELECT 
				@referenceNo = ForeignRemoteSystem_Reference_no 
			FROM 
				tWUnion_BillPay_Trx 
			WHERE 
				WUBillPayTrxID = @wuBillPayTrxID
		END

		SELECT 
			tc.FirstName,
			tc.LastName,
			tc.Address1,
			tc.Address2,
			tc.City,
			tc.State,
			tc.ZipCode as Zip,
			tc.DOB,
			tc.Phone1 AS CustomerPhoneNumber,
			tc.Email,
			tc.GovtIdentification AS PrimaryIdNumber,
			tc.SSN AS SecondIdNumber,
			tc.CountryOfBirth,
			CASE WHEN ISNULL(tc.OccupationDescription, '') = '' THEN o.Name
				 ELSE tc.OccupationDescription 
			END AS Occupation,
			tn.Name AS PrimaryIdType,
			CASE
				WHEN tc.Phone1 IS NOT NULL AND tc.Phone1Type = 'Cell'
				THEN tc.Phone1
				WHEN tc.Phone2 IS NOT NULL AND tc.Phone2Type = 'Cell'
				THEN tc.Phone2
				ELSE ''
			END AS CustomerMobileNumber,
			ISNULL(tm.Name, '') AS PrimaryIdCountryOfIssue,		
			ISNULL(tn.Country,'') AS PrimaryCountryOfIssue,
			ISNULL(ts.Name,'') PrimaryIdPlaceOfIssue,
			ISNULL(tm.Abbr2,'') PrimaryIdCountryOfIssueCode,			
			ISNULL(ts.Abbr,'') PrimaryIdPlaceOfIssueCode,
			ISNULL(tm.Abbr2,'') PrimaryIdCountryOfIssueCode,
			ISNULL(twc.Name,'') PrimaryIdCountryNameOfIssue,
			'SSN' AS SecondIdType,
			'US' AS SecondIdCountryOfIssue,
			ISNULL(tmc.Abbr3,'') CountryOfBirthAbbr3,
			@referenceNo as ForeignRemoteSystemReferenceNo,
			wbc.CardNumber
		FROM 		
			dbo.tCustomers tc 
			INNER JOIN dbo.tNexxoIdTypes tn WITH (NOLOCK) ON tn.NexxoIdTypeID = tc.GovtIdTypeId
			INNER JOIN dbo.tOccupations o WITH (NOLOCK) ON o.Code = tc.Occupation
			LEFT JOIN dbo.tMasterCountries tm WITH (NOLOCK) ON tm.MasterCountriesID = tn.MasterCountriesID
			LEFT JOIN dbo.tStates ts WITH (NOLOCK) ON ts.StateId = tn.StateId
			INNER JOIN dbo.tMasterCountries tmc WITH (NOLOCK) ON tmc.Abbr2 = tc.CountryOfBirth
			INNER JOIN tWUnion_BillPay_Account wbc WITH (NOLOCK) ON wbc.CustomerId = tc.CustomerID
			INNER JOIN tWUnion_Countries twc WITH (NOLOCK) ON twc.ISOCountryCode = tm.Abbr2 
		WHERE
			tc.CustomerId = @customerId

	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO