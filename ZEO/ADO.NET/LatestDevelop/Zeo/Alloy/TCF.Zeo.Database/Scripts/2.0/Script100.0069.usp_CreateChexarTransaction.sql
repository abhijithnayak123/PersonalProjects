--- ===============================================================================
-- Author:		 Manikandan Govindraj
-- Description:	 create chexar transactions
-- Jira ID:		<AL-7705>
-- ================================================================================


IF OBJECT_ID(N'usp_CreateChexarTransaction', N'P') IS NOT NULL
DROP PROC usp_CreateChexarTransaction
GO

CREATE PROCEDURE usp_CreateChexarTransaction
		
	@transactionId BIGINT,
	@amount MONEY,
	@checkDate DATETIME,
	@micr NVARCHAR(50),
	@latitude FLOAT,
	@longitude FLOAT,
	@status INT,
	@chexarStatus NVARCHAR(50),
	@location NVARCHAR(50),
	@submitType INT,
	@chexarAccountId BIGINT,
	@dTServerCreate DATETIME,
	@dTTerminalCreate DATETIME
AS
BEGIN

 BEGIN TRY

       DECLARE @chxarTransactionId BIGINT	
	
	   INSERT INTO tChxr_Trx
		(
			TransactionId,
			Amount,		
			CheckDate,
			Micr,
			Latitude,
			Longitude,
			Status,
			ChexarStatus, 
			Location,
			SubmitType,
			ChxrAccountId,
			DTServerCreate,	
			DTTerminalCreate
		)
		Values
		(			
			@transactionId,
			@amount,
			@checkDate,
			@micr,
			@latitude,
			@longitude,
			@status,
			@chexarStatus,
			@location,
			@submitType,
			@chexarAccountId,
			@dTServerCreate,
			@dTTerminalCreate
		)

		 SELECT @chxarTransactionId = CAST(SCOPE_IDENTITY() AS BIGINT)

  END TRY
  BEGIN CATCH
    EXECUTE usp_CreateErrorInfo
  END CATCH
END
GO



