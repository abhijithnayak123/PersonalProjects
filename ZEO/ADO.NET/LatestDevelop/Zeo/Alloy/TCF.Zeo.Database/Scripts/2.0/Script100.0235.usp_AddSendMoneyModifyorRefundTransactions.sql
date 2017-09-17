--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <12-14-2016>
-- Description:	This SP is used to update the Money Transfer transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_AddSendMoneyModifyorRefundTransactions', N'P') IS NOT NULL
DROP PROC usp_AddSendMoneyModifyorRefundTransactions
GO


CREATE PROCEDURE usp_AddSendMoneyModifyorRefundTransactions
(
	@mtTransactionId BIGINT
	,@customerSessionId BIGINT
	,@state INT
	,@transactionSubType VARCHAR(20) -- Either modify or refund. Pass it from the application. //2-Modify, 3-Refund
	,@recipientId BIGINT
	,@originalTransactionID BIGINT
	,@dtTerminalDate DATETIME
	,@dtServerDate DATETIME
)
AS
BEGIN
BEGIN TRY
	
	 
	 
	 DECLARE @cancelTranSubType VARCHAR(20) = '1' 
	 DECLARE @modifyorRefundTrxId BIGINT
	 DECLARE @cancelTrxId BIGINT
	 DECLARE @providerId BIGINT = 301
	 DECLARE @transferType INT = 1
	 DECLARE @amount MONEY
	 DECLARE @fee MONEY
	 
	 SELECT	
			@amount = Amount
			,@fee = Fee
	 FROM tTxn_MoneyTransfer
	 WHERE TransactionID = @mtTransactionId


	 DECLARE @providerAccountId BIGINT =
	 (
		SELECT wa.WUAccountID
		FROM tWUnion_Account wa
			INNER JOIN tCustomers c ON c.CustomerId = wa.CustomerId
			INNER JOIN tCustomerSessions cs ON cs.CustomerId = c.CustomerId
		WHERE cs.CustomerSessionID = @customerSessionId
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
	 
	 -- Insert the money transfer record for cancel subtype for PTNR transaction. 
	 INSERT INTO [dbo].[tTxn_MoneyTransfer]
           ([Amount]
           ,[Fee]
           --,[Description]
          -- ,[ConfirmationNumber]
           ,[RecipientId]
          -- ,[ExchangeRate]
           ,[TransferType]
           ,[TransactionSubType]
           ,[OriginalTransactionID]
           ,[CustomerSessionId]
           ,[CustomerRevisionNo]
           ,[ProviderId]
           ,[ProviderAccountId]
           --,[Destination]
           ,[State]
		   ,[DTServerCreate]
		   ,[DTTerminalCreate]
		  )
     VALUES
           (@amount
           ,@fee
          -- ,@description
          -- ,@confirmationNumber
           ,@recipientId
          -- ,@exchangeRate
           ,@transferType
           ,@cancelTranSubType
           ,@originalTransactionId
           ,@customerSessionId
           ,@customerRevisionNo
           ,@providerId
           ,@providerAccountId
          -- ,@destination
           ,@state
		   ,@dtServerDate
		   ,@dtTerminalDate
		 )

	 SET @cancelTrxId = CAST(SCOPE_IDENTITY() AS BIGINT)
	 

	 
	 INSERT INTO [dbo].[tTxn_MoneyTransfer]
           ([Amount]
           ,[Fee]
           --,[Description]
          -- ,[ConfirmationNumber]
           ,[RecipientId]
          -- ,[ExchangeRate]
           ,[TransferType]
           ,[TransactionSubType]
           ,[OriginalTransactionID]
           ,[CustomerSessionId]
           ,[CustomerRevisionNo]
           ,[ProviderId]
           ,[ProviderAccountId]
           --,[Destination]
           ,[State]
		   ,[DTServerCreate]
		   ,[DTTerminalCreate]
		  )
     VALUES
           (@amount
           ,@fee
          -- ,@description
          -- ,@confirmationNumber
           ,@recipientId
          -- ,@exchangeRate
           ,@transferType
           ,@transactionSubType
           ,@originalTransactionId
           ,@customerSessionId
           ,@customerRevisionNo
           ,@providerId
           ,@providerAccountId
          -- ,@destination
           ,@state
		   ,@dtServerDate
		   ,@dtTerminalDate
		 )
	 
	  SET @modifyorRefundTrxId = CAST(SCOPE_IDENTITY() AS BIGINT)

	  SELECT
			@cancelTrxId AS CancelTransactionId
			,@modifyorRefundTrxId AS ModifyorRefundTransactionId 

END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO
