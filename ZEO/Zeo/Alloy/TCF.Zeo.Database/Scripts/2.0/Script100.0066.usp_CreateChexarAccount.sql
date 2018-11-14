-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Create chexar Sim Account
-- Jira ID:		<AL-7705>
-- ================================================================================

-- EXEC usp_CreateChexarSimAccount

-- DISABLE TRIGGER tr_Chxr_Account_Aud ON tChxr_Account

IF OBJECT_ID(N'usp_CreateChexarAccount', N'P') IS NOT NULL
DROP PROC usp_CreateChexarAccount
GO

CREATE PROCEDURE usp_CreateChexarAccount
(
    @chxrAccountId BIGINT OUT,
	@badgeId BIGINT,
	@customerSessionId BIGINT,
	@customerId BIGINT,
	@dTServerCreate DATETIME,
	@dTTerminalCreate DATETIME
)

AS
BEGIN
	BEGIN TRY
	 
	 DECLARE @customerRevisionNo BIGINT

	 SELECT 
	   @customerRevisionNo = MAX(RevisionNo) 
	 FROM
	   tCustomers_Aud 
	 WHERE 
	   CustomerId = @customerId


	 INSERT INTO 
		 tChxr_Account
		 (
		    Badge,
			CustomerId,
			CustomerSessionId,
			CustomerRevisionNo,
			DTServerCreate,
			DTTerminalCreate
		 )
		 VALUES 
		 ( 
		    @badgeId,
		    @customerId,
		    @customerSessionId,
			@customerRevisionNo,
			@dTServerCreate,
			@dTTerminalCreate
		 )

		 SELECT @chxrAccountId = CAST(SCOPE_IDENTITY() AS BIGINT)
 
	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END


