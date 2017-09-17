-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <11/26/2016>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
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
			FROM 
				tCustomers C WITH (NOLOCK)
				LEFT JOIN tVisa_Account VA WITH (NOLOCK) ON VA.CustomerId = C.CustomerID
			WHERE 
				VA.CardNumber = @cardNumber
	END TRY
	BEGIN CATCH 
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

	