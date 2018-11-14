--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-04-2016>
-- Description:	This SP is used to insert a money transfer transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_CreateMoneyTransferTransaction', N'P') IS NOT NULL
DROP PROC usp_CreateMoneyTransferTransaction
GO


CREATE PROCEDURE usp_CreateMoneyTransferTransaction
(
	@customerSessionId BIGINT,
	@providerAccountId BIGINT,
	@amount MONEY,
	@fee MONEY,
	@description NVARCHAR(255),
	@confirmationNumber VARCHAR(50),
	@recipientId BIGINT,
	@exchangeRate MONEY,
	@transferType INT,
	@transactionSubType VARCHAR(20),
	@originalTransactionId BIGINT,
	@providerId BIGINT,
	@destination NVARCHAR(200),
	@state INT,
	@dtServerCreate DATETIME,
	@dtTerminalCreate DATETIME
)
AS
BEGIN
	
BEGIN TRY

	DECLARE @IsExist BIGINT
	DECLARE @Status INT

	SELECT 
		@IsExist = COUNT(1)
	FROM 
		tTxn_MoneyTransfer 
	WHERE 
		ConfirmationNumber = @confirmationNumber AND TransferType = '2' AND [State] = 2 


	IF @IsExist = 0
	BEGIN
		DECLARE @tranSubType VARCHAR(20) =
		(
			SELECT
				CASE 
					WHEN @transactionSubType = ' '
						THEN NULL
						ELSE @transactionSubType
				END
		)
		
		 -- Get latest customer revision number
		 DECLARE @customerRevisionNo BIGINT =
		 (
			SELECT 
				ISNULL(MAX(ca.RevisionNo),0) 
			FROM tCustomers_Aud ca
				INNER JOIN tCustomerSessions cs ON ca.CustomerID = cs.CustomerID
			WHERE cs.CustomerSessionID = @customerSessionId

		 )
		 
		 -- Insert the money transfer 
		 INSERT INTO [dbo].[tTxn_MoneyTransfer]
		       ([Amount],
		       [Fee],
		       [Description],
		       [DTTerminalCreate],
		       [ConfirmationNumber],
		       [RecipientId],
		       [ExchangeRate],
		       [DTServerCreate],
		       [TransferType],
		       [TransactionSubType],
		       [OriginalTransactionID],
		       [CustomerSessionId],
		       [CustomerRevisionNo],
		       [ProviderId],
		       [ProviderAccountId],
		       [Destination],
		       [State])
		 VALUES
		       (@amount,
		       @fee,
		       @description,
		       @dtTerminalCreate,
		       @confirmationNumber,
		       @recipientId,
		       @exchangeRate,
		       @dtServerCreate,
		       @transferType,
		       @tranSubType,
		       @originalTransactionId,
		       @customerSessionId,
		       @customerRevisionNo,
		       @providerId,
		       @providerAccountId,
		       @destination,
		       @state)

			 SELECT CAST (SCOPE_IDENTITY() AS BIGINT) AS TransactionId, CAST(0 AS BIT) AS IsExist, @Status AS State
		END
	
	ELSE
		BEGIN

			SELECT 
				@Status = [State] 
			FROM  
				tTxn_MoneyTransfer 
			WHERE 
				ConfirmationNumber = @confirmationNumber AND TransferType = '2' AND [State] = 2 

			SELECT 0 AS TransactionId, CAST(1 AS BIT) AS IsExist, @Status AS State
		END

END TRY
BEGIN CATCH

  	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
