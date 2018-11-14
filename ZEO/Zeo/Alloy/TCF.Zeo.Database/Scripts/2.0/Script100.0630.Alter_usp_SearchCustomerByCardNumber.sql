-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <11/26/2016>
-- Updated Date: <10/12/2017> 
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Updated desc: we have to get the customers only if the profile status is Active 
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_SearchCustomerByCardNumber'
)

BEGIN
	DROP PROCEDURE usp_SearchCustomerByCardNumber
END
GO

CREATE PROCEDURE usp_SearchCustomerByCardNumber
	@cardNumber NVARCHAR(255) 
AS
BEGIN
	BEGIN TRY
			SELECT 
				C.CustomerID
			    ,C.FirstName 
			    ,C.LastName 
			    ,C.DOB
			    ,C.SSN
			    ,C.ProfileStatus
				,C.Phone1
				,C.GovtIdentification
				,VA.CardNumber
				,C.Address1
				,C.GovtIDExpirationDate
			FROM 
				tCustomers C WITH (NOLOCK)
				INNER JOIN tVisa_Account VA WITH (NOLOCK) ON  VA.CustomerId = C.CustomerID AND VA.CardNumber = @cardNumber 
				INNER JOIN tTCIS_Account tca WITH (NOLOCK) ON tca.CustomerID = C.CustomerID
			WHERE 
				C.ProfileStatus = 1 AND --Active
				VA.Activated = 1 AND
				tca.ProfileStatus = 1 
	END TRY
	BEGIN CATCH 
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

	