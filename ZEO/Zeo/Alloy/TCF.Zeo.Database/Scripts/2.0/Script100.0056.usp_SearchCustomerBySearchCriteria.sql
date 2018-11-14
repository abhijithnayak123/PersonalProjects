-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>

-- EXEC usp_SearchCustomerBySearchCriteria 34, NULL, 'sakala', '10/10/1950', NULL, NULL, 0
-- EXEC usp_SearchCustomerBySearchCriteria 34, '344222', NULL, NULL, '345435888', NULL, 0
-- ================================================================================


IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_SearchCustomerBySearchCriteria'
)

BEGIN
	DROP PROCEDURE usp_SearchCustomerBySearchCriteria
END
GO

CREATE PROCEDURE usp_SearchCustomerBySearchCriteria
	@channelPartnerId BIGINT = NULL,
	@ssn NVARCHAR(255) = NULL,
	@lastName VARCHAR(200) = NULL,
	@dob DATETIME = NULL,
	@phoneNumber NVARCHAR(255) = NULL,
	@idNumber NVARCHAR(255) = NULL,
	@isCloseCustomer BIT = 0
AS
BEGIN
	BEGIN TRY
		IF(@isCloseCustomer = 0)
			SELECT 
				CustomerID
			    ,FirstName 
			    ,LastName 
			    ,DOB
			    ,SSN
			    ,ProfileStatus
				,Phone1
				,GovtIdentification
			FROM 
				tCustomers WITH (NOLOCK)
			WHERE 
				ChannelPartnerId = @channelPartnerId
				AND (@lastName IS NULL OR LastName= @lastName) 
				AND (@ssn IS NULL OR SSN = @ssn) 
				AND (@dob IS NULL OR DOB = @dob) 
				AND (@phoneNumber IS NULL OR Phone1 = @phoneNumber) 
				AND (@idNumber IS NULL OR GovtIdentification = @idNumber)
				AND (ProfileStatus = 0 OR ProfileStatus = 1)
		ELSE 
			SELECT 
				CustomerID
			    ,FirstName 
			    ,LastName 
			    ,DOB
			    ,SSN
			    ,ProfileStatus
				,Phone1
				,GovtIdentification
			FROM 
				tCustomers WITH (NOLOCK)
			WHERE 
				ChannelPartnerId = @channelPartnerId
				AND (@lastName IS NULL OR LastName= @lastName) 
				AND (@ssn IS NULL OR SSN = @ssn) 
				AND (@dob IS NULL OR DOB = @dob) 
				AND (@phoneNumber IS NULL OR Phone1 = @phoneNumber) 
				AND (@idNumber IS NULL OR GovtIdentification = @idNumber)
	END TRY
	BEGIN CATCH 
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

	