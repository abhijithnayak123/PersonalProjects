-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2016>
-- Modified date: <06/22/2017>
-- Description:	<Fetching the customer with IsRCIFSuccess flag as true.>
-- Jira ID:		<AL-7630>
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
				C.CustomerID
			    ,C.FirstName 
			    ,C.LastName 
			    ,C.DOB
			    ,C.SSN
			    ,C.ProfileStatus
				,C.Phone1
				,C.GovtIdentification
				,VA.CardNumber
			FROM 
				tCustomers C WITH (NOLOCK)
				LEFT JOIN tVisa_Account VA WITH (NOLOCK) ON VA.CustomerId = C.CustomerID AND VA.Activated = 1
			WHERE 
				ChannelPartnerId = @channelPartnerId
				AND (@lastName IS NULL OR C.LastName= @lastName) 
				AND (@ssn IS NULL OR C.SSN = @ssn) 
				AND (@dob IS NULL OR C.DOB = @dob) 
				AND (@phoneNumber IS NULL OR C.Phone1 = @phoneNumber) 
				AND (@idNumber IS NULL OR C.GovtIdentification = @idNumber)
				AND C.ProfileStatus = 1 AND IsRCIFSuccess = 1
		ELSE 
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
			FROM 
				tCustomers C WITH (NOLOCK)
				LEFT JOIN tVisa_Account VA WITH (NOLOCK) ON VA.CustomerId = C.CustomerID AND VA.Activated = 1
			WHERE 
				ChannelPartnerId = @channelPartnerId
				AND (@lastName IS NULL OR C.LastName= @lastName) 
				AND (@ssn IS NULL OR C.SSN = @ssn) 
				AND (@dob IS NULL OR C.DOB = @dob) 
				AND (@phoneNumber IS NULL OR C.Phone1 = @phoneNumber) 
				AND (@idNumber IS NULL OR C.GovtIdentification = @idNumber)
				AND (ProfileStatus = 1 OR C.ProfileStatus = 2) 
				AND IsRCIFSuccess = 1
	END TRY
	BEGIN CATCH 
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

	