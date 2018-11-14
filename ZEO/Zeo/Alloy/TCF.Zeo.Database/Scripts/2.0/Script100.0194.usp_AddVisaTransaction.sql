-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To Add Visa Transaction
-- =============================================


IF OBJECT_ID(N'usp_AddVisaTransaction', N'P') IS NOT NULL
DROP PROC usp_AddVisaTransaction
GO

CREATE PROCEDURE usp_AddVisaTransaction 
	(
		@amount MONEY,
		@transactiontype INT,
		@status INT,
		@balance MONEY,
		@promoCode VARCHAR(50),
		@locationNodeId BIGINT,
		@dTServerCreate DATETIME,
		@dTTerminalCreate DATETIME,
		@visaaccountid BIGINT,
		@dtTransmission DATETIME
	)
AS
BEGIN

	BEGIN TRY
		INSERT INTO tVisa_Trx
		(			 
			 Amount,
			 TransactionType,
			 Status,
			 Balance,
			 PromoCode,
			 LocationNodeId,
			 VisaAccountID,
			 DTTransmission,
			 DTServerCreate,
			 DTTerminalCreate
		)
		VALUES
		(			
			@amount,
			@transactiontype,
			@status,
			@balance,
			@promocode,
			@locationNodeId,
			@visaaccountid,
			@dtTransmission,
			@dTServerCreate,
			@dTTerminalCreate
		)

		SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS VisaTrxID
END TRY

BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
