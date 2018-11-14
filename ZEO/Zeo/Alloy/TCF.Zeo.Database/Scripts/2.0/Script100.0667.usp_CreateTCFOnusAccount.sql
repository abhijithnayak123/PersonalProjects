-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-23-2018>
-- Description:	 Create TCF On US Account
-- ================================================================================

IF OBJECT_ID(N'usp_CreateTCFOnusAccount', N'P') IS NOT NULL
DROP PROC usp_CreateTCFOnusAccount
GO

CREATE PROCEDURE usp_CreateTCFOnusAccount
(
    @tcfOnusAccountId BIGINT OUT,
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
	   tCustomers_Aud (NOLOCK)
	 WHERE 
	   CustomerId = @customerId


	 INSERT INTO 
		 tTCFOnus_Account
		 (
			CustomerId,
			CustomerSessionId,
			CustomerRevisionNo,
			DTServerCreate,
			DTTerminalCreate
		 )
		 VALUES 
		 ( 
		    @customerId,
		    @customerSessionId,
			@customerRevisionNo,
			@dTServerCreate,
			@dTTerminalCreate
		 )

		 SELECT @tcfOnusAccountId = CAST(SCOPE_IDENTITY() AS BIGINT)
 
	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END


