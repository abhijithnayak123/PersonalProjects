--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Create TCF Onus transactions
-- Jira ID:		<AL-7705>
-- ================================================================================


IF OBJECT_ID(N'usp_CreateTCFOnusTransaction', N'P') IS NOT NULL
DROP PROC usp_CreateTCFOnusTransaction
GO

CREATE PROCEDURE usp_CreateTCFOnusTransaction
		
	@tcfonusTransactionId BIGINT OUTPUT,
	@amount MONEY,
	@checkDate DATETIME,
	@micr NVARCHAR(50),
	@latitude FLOAT,
	@longitude FLOAT,
	--@status INT,
	@tcfonusStatus NVARCHAR(50),
	@location NVARCHAR(50),
	@submitType INT,
	@tcfonusAccountId BIGINT,
	@dTServerCreate DATETIME,
	@dTTerminalCreate DATETIME
AS
BEGIN

 BEGIN TRY

	
	   INSERT INTO tTCFOnus_Trx
         (
			Amount
           ,CheckDate
           --,CheckNumber
           --,RoutingNumber
           --,AccountNumber
           ,Micr
           ,Latitude
           ,Longitude
           --,Status
           ,TCFOnusStatus
           ,Location
          ,TCFOnusAccountId
		  ,DTTerminalCreate
           ,DTServerCreate
		  )
		Values
		(
			@amount,
			@checkDate,
			--@checkNumber,
			--@routingNumber,
			--@accountNumber,
			@micr,
			@latitude,
			@longitude,
			--@status,
			@tcfOnusStatus,
			@location,
			@tcfonusAccountId,
			@dTTerminalCreate,
			@dTServerCreate
		)

      SELECT @tcfonusTransactionId = CAST(SCOPE_IDENTITY() AS BIGINT)
		
  END TRY
  BEGIN CATCH
    EXECUTE usp_CreateErrorInfo
  END CATCH
END
GO



