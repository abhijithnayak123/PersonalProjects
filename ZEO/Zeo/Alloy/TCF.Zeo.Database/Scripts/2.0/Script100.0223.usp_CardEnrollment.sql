--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-04-2016>
-- Description:	This SP is used for Card Enrollment.
-- Jira ID:		<AL-8324>

--EXEC usp_CardEnrollment 1000000010, '50058570703', '10/20/2016', '10/22/2016'
-- ================================================================================

IF OBJECT_ID(N'usp_CardEnrollment', N'P') IS NOT NULL
DROP PROC usp_CardEnrollment
GO

CREATE PROCEDURE usp_CardEnrollment
(
   	@customerSessionId BIGINT
	,@preferredCustomerAccountNumber VARCHAR(250) = NULL
	,@dtServerDate DATETIME = NULL
	,@dtTerminalDate DATETIME = NULL
)
AS
BEGIN

BEGIN TRY
	

	SET NOCOUNT ON;

	DECLARE @customerId BIGINT =
	(
		SELECT c.CustomerId 
		FROM tCustomers c
			INNER JOIN tCustomerSessions cs ON cs.CustomerId = c.CustomerId
		WHERE cs.CustomerSessionId = @customerSessionId
	)

	DECLARE @wuAccountId BIGINT =
	(
		SELECT WUAccountID
		FROM tWUnion_Account wa
		WHERE CustomerId = @customerId
	)

	DECLARE @smsNotificationFlag VARCHAR(250) =
	(
		SELECT	
			CASE 
				WHEN SMSEnabled = 1
				THEN 'Y'
				ELSE 'N'
			END
		FROM tCustomers
		WHERE CustomerId = @customerId			
	)

	EXEC usp_SaveWUAccount 
		  @customerSessionId
		  ,@wuAccountId
		  ,@preferredCustomerAccountNumber 
		  ,@smsNotificationFlag
		  ,@dtServerDate
		  ,@dtTerminalDate


	EXEC usp_CreateOrUpdateWUBillPayAccount
		 @customerSessionId
		 ,@preferredCustomerAccountNumber	
		 ,@dtTerminalDate																	 
		 ,@dtServerDate														 

END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

END CATCH
END