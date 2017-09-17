--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-04-2016>
-- Description:	This SP is used to update the gold card number in WU account.
-- Jira ID:		<AL-8324>

--EXEC usp_SaveWUAccount 1000000020,0,'194667259','N','10/10/2016','10/20/2016'
-- ================================================================================

IF OBJECT_ID(N'usp_SaveWUAccount', N'P') IS NOT NULL
DROP PROC usp_SaveWUAccount
GO

CREATE PROCEDURE usp_SaveWUAccount
(
   	@customerSessionId BIGINT
	,@wuAccountId BIGINT
	,@preferredCustomerAccountNumber VARCHAR(250) = NULL
	,@smsNotificationFlag VARCHAR(250) = NULL
	,@dtServerDate DATETIME
	,@dtTerminalDate DATETIME = NULL
)
AS
BEGIN
	
BEGIN TRY
	
	DECLARE @customerId BIGINT =
	(
		SELECT CustomerId 
		FROM tCustomerSessions
		WHERE CustomerSessionId = @customerSessionId
	)

	DECLARE @preferredCustomerLevelCode VARCHAR(250) = ''
	DECLARE @nameType VARCHAR(200) = '' 

	IF NOT EXISTS 
	(
		SELECT 1 
		FROM tWUnion_Account
		WHERE CustomerId = @customerId
	)
	BEGIN

		 DECLARE @customerRevisionNo BIGINT =
		 (
			 SELECT 
			   MAX(RevisionNo) 
			 FROM
			   tCustomers_Aud 
			 WHERE 
			   CustomerId = @customerId
		  )

		 SET @nameType = 'D'	
		
		 

		 INSERT INTO [dbo].[tWUnion_Account]
			   ([DTTerminalCreate]
			   ,[NameType]
			   ,[PreferredCustomerAccountNumber]
			   ,[PreferredCustomerLevelCode]
			   ,[SmsNotificationFlag]
			   ,[DTServerCreate]
			   ,[CustomerId]
			   ,[CustomerRevisionNo]
			   ,[CustomerSessionId])
		 VALUES
			   (@dtTerminalDate
			   ,@nameType
			   ,@preferredCustomerAccountNumber
			   ,@preferredCustomerLevelCode
			   ,@smsNotificationFlag
			   ,@dtServerDate
			   ,@customerId
			   ,@customerRevisionNo
			   ,@customerSessionId)

		SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS AccountId
	END
	ELSE
	BEGIN

		UPDATE tWUnion_Account
		SET NameType = ISNULL(@nameType, NameType)
			, PreferredCustomerAccountNumber = ISNULL(@preferredCustomerAccountNumber, PreferredCustomerAccountNumber)
			, PreferredCustomerLevelCode = ISNULL(@preferredCustomerLevelCode, PreferredCustomerLevelCode)
			, SmsNotificationFlag = ISNULL(@smsNotificationFlag, SmsNotificationFlag)
			, DTTerminalLastModified = @dtTerminalDate
			, DTServerLastModified = @dtServerDate
		WHERE WUAccountID = @wuAccountId

	END


END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
