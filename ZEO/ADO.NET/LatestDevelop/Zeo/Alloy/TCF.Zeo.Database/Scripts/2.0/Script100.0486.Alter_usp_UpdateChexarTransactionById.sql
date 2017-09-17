-- =============================================
-- Author:		<Author,,Manikandan Govindraj>
-- Create date: <09/23/2016>
-- Description:	<AL-7837 : Update Chexar transactions>
-- =============================================

--  exec usp_UpdateChexarTransactionById 

IF OBJECT_ID(N'usp_UpdateChexarTransactionById', N'P') IS NOT NULL
DROP PROC usp_UpdateChexarTransactionById
GO

CREATE PROCEDURE usp_UpdateChexarTransactionById
	@chxrTransactionId BIGINT,	
	@amount MONEY,
	@chexarAmount MONEY,
	@chexarFee MONEY,
	@checkNumber  NVARCHAR(20),
	@routingNumber NVARCHAR(20),
	@accountNumber NVARCHAR(20),
	@latitude FLOAT,
	@longitude FLOAT,
	@ticketId INT,
	@waitTime NVARCHAR(50),
	@status INT,
	@chexarStatus NVARCHAR(50),
	@declineCode INT,
	@submitType INT,
	@returnType INT,
	@invoiceId INT,
	@declineMessagekey NVARCHAR(20),
	@dTServerLastModified DATETIME,
	@dTTerminalLastModified DATETIME,
	@channelPartnerId BIGINT,
	@lang INT

AS
BEGIN

  BEGIN TRY


        DECLARE @declineMessage NVARCHAR(1000)  
		--Get DMS Decline message 

		if(@status = 10) -- Declined
		BEGIN
			SET @declineMessage = dbo.ufn_GetDeclineMessageByKey(@declineMessagekey, @channelPartnerId, @lang)

			IF( @declineMessage IS NULL)
			BEGIN
			   SET @declineMessagekey = '1002.200.0' --Unhandled error decline code while doing the check processing
			   SET @declineMessage = dbo.ufn_GetDeclineMessageByKey(@declineMessagekey, @channelPartnerId, @lang)
			END
	    END

		UPDATE 
		  tChxr_Trx
		SET	
			Amount = @amount,
			InvoiceId = @invoiceId,
			ChexarAmount = @chexarAmount,
			ChexarFee =  @chexarFee,
			CheckNumber = @checkNumber,
			RoutingNumber = @routingNumber,
			AccountNumber = @accountNumber,
			Latitude = @latitude,
			Longitude = @longitude,             
			TicketId = @ticketId,
			WaitTime = @waitTime,
			Status = @status,
			ChexarStatus = @chexarStatus,
			DeclineCode = @declineCode, 
			SubmitType = @submitType,
			ReturnType = @returnType,
			Message = @declineMessage,
			DTServerLastModified = @dTServerLastModified,	
			DTTerminalLastModified = @dTTerminalLastModified		
		 WHERE
		    ChxrTrxId = @chxrTransactionId

        -- Get DMS Check Submit type and return type by Chexar Type id 

	    EXEC usp_GetCheckSubmitAndReturnTypeByChexarTypeId @submitType, @returnType, @submitType OUTPUT, @returnType OUTPUT

		SELECT @declineMessage as DeclineMessage, @submitType as SubmitType, @returnType as ReturnType


  END TRY
  BEGIN CATCH
    EXECUTE usp_CreateErrorInfo
  END CATCH
  
  	
END
GO


