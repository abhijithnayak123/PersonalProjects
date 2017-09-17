-- ===============================================================================
-- Author & Modified by:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Modified date: <06-02-2017>
-- Description:	 Modifying Get Chexar Account to return default date for customers with Id exp date null.
-- ================================================================================

-- EXEC usp_GetChexarAccount '1000000020'

IF OBJECT_ID(N'usp_GetChexarAccount', N'P') IS NOT NULL
DROP PROC usp_GetChexarAccount
GO

CREATE PROCEDURE usp_GetChexarAccount
(
	@customerSessionId BIGINT
)
AS
BEGIN
	BEGIN TRY

		SELECT
		    tca.ChxrAccountID, 
			tca.Badge,
			tc.FirstName,
			tc.LastName,
			tc.Address1,
			tc.Address2,
			tc.City,
			tc.State,
			tc.ZipCode as Zip,
			tc.Phone1 as Phone,
			tc.SSN,
			tc.IDCode,
			tc.DOB,
			tc.GovtIdentification as GovernmentId,
			ISNULL(tc.GovtIDExpirationDate, CONVERT(DATETIME,'1900-01-01')) as IDCardExpireDate,
			nt.Name as IDCardType,
			tc.GovtIdIssueDate as IDCardIssuedDate,
			c.Name as IDCardIssuedCountry,
			NULL as Occupation,
			NULL as Employer,
			NULL as EmployerPhone,
			tc.CustomerID

		FROM 
			tChxr_Account tca WITH (NOLOCK)
		RIGHT JOIN 
		    tCustomers tc  WITH (NOLOCK)
        ON 
			tc.CustomerID = tca.CustomerId 
        LEFT JOIN
		   tNexxoIdTypes as nt WITH (NOLOCK)
		ON 
		   nt.NexxoIdTypeID = tc.GovtIdTypeId
		LEFT JOIN
		   tStates as s WITH (NOLOCK)
		ON 
		   s.StateId = nt.StateId
		LEFT JOIN
		   tMasterCountries c WITH (NOLOCK)
		ON 
		   c.MasterCountriesID = nt.MasterCountriesID		
		INNER JOIN 
		    tCustomerSessions cs WITH (NOLOCK)
		ON 
		    cs.CustomerId = tc.CustomerId      
		WHERE 
			cs.CustomerSessionId = @customerSessionId

	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END

