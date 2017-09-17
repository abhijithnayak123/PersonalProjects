-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Create chexar Sim Account
-- Jira ID:		<AL-7705>
-- ================================================================================
-- EXEC usp_CreateChexarSimAccount

IF OBJECT_ID(N'usp_CreateChexarSimAccount', N'P') IS NOT NULL
DROP PROC usp_CreateChexarSimAccount
GO

CREATE PROCEDURE usp_CreateChexarSimAccount
(
	@badgeId BIGINT OUTPUT,
	@CustomerSessionId BIGINT,
	@CustomerId BIGINT,
	@dTServerCreate DATETIME
)

AS
BEGIN
	BEGIN TRY

	DECLARE @chexarSimAccountId BIGINT

	SELECT
	  @chexarSimAccountId = ISNULL(MAX(ChxrSimAccountId), 100000) + 1
	FROM 
	  tChxrSim_Account



	 INSERT INTO 
		 tChxrSim_Account
		 (
			ChxrSimAccountId,
			CustomerId,
			CustomerSessionId,
			DTServerCreate
		 )
		 VALUES 
		 (
		    @chexarSimAccountId,
		    @CustomerId,
		    @CustomerSessionId,
			@dTServerCreate
		 )

		 SELECT @badgeId = SCOPE_IDENTITY() 
 
	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END


