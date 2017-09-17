-- ================================================================================
-- Author:		Nishad Varghese
-- Create date: 01/Sep/2016
-- Description:	Update MO transaction
-- Jira ID:		AL-7706

/*

EXECUTE usp_UpdateMoneyOrderTransaction
1000000004,
'o039559ot121000248t161226120629o',
'1612261206',
'161226120629',
'161226120629',
1,
'12/20/2016',
'12/20/2016'

*/ 
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE TYPE = 'P' and NAME = 'usp_UpdateMoneyOrderTransaction'
)
BEGIN
	DROP PROCEDURE usp_UpdateMoneyOrderTransaction
END
GO


CREATE PROCEDURE [dbo].[usp_UpdateMoneyOrderTransaction]
(
	@transactionId            BIGINT,
	@MICR                     VARCHAR(500),
	@checkNumber              VARCHAR(50),
	@accountNumber            VARCHAR(20),
	@routingNumber            VARCHAR(20),
	@checkFrontImage          VARBINARY(MAX),
	@checkBackImage           VARBINARY(MAX),
	@AllowDuplicateMoneyOrder BIT,
	@dTTerminalDate           DATETIME,
	@dTServerDate             DATETIME
)
AS
BEGIN
	BEGIN TRY
	
	DECLARE @isUpdated BIT = 0

	IF @AllowDuplicateMoneyOrder = 1
	BEGIN
		--Updating the tTxn_MoneyOrder table
		UPDATE tTxn_MoneyOrder
		SET
			AccountNumber = @accountNumber,
			RoutingNumber = @routingNumber,
			MICR = @MICR,
			CheckNumber = @checkNumber,
			DTServerLastModified = @dTServerDate,
			DTTerminalLastModified = @dTTerminalDate
		WHERE
			TransactionID = @transactionId

		  --Creating/Updating the Image
        EXECUTE usp_CreateOrUpdateMoneyOrderImage
                @transactionId,
                @checkFrontImage,
                @checkBackImage,
                @dTServerDate,
                @dTTerminalDate

		  SET @isUpdated = 1
   END

	IF @AllowDuplicateMoneyOrder = 0 AND NOT EXISTS (SELECT 1 FROM tTxn_MoneyOrder WHERE MICR = @MICR AND State = 4)
	BEGIN
		--Updating the tTxn_MoneyOrder table
		UPDATE tTxn_MoneyOrder
		SET
			AccountNumber = @accountNumber,
			RoutingNumber = @routingNumber,
			MICR = @MICR,
			CheckNumber = @checkNumber,
			DTServerLastModified = @dTServerDate,
			DTTerminalLastModified = @dTTerminalDate
		WHERE
			TransactionID = @transactionId

		  --Creating/Updating the Image
        EXECUTE usp_CreateOrUpdateMoneyOrderImage
                @transactionId,
                @checkFrontImage,
                @checkBackImage,
                @dTServerDate,
                @dTTerminalDate

		  SET @isUpdated = 1
   END

	SELECT @isUpdated AS IsUpdated
	
	END TRY

	BEGIN CATCH	 
        EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END

GO