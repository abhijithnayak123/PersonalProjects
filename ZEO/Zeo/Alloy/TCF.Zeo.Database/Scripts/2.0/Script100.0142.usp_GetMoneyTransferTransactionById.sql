--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-04-2016>
-- Description:	This SP is used to get money transfer transaction.
-- Jira ID:		<AL-8324>

-- EXEC usp_GetMoneyTransferTransactionById 1000000001
-- ================================================================================

IF OBJECT_ID(N'usp_GetMoneyTransferTransactionById', N'P') IS NOT NULL
DROP PROC usp_GetMoneyTransferTransactionById
GO


CREATE PROCEDURE usp_GetMoneyTransferTransactionById
(
    @transactionId BIGINT
)
AS
BEGIN
	
BEGIN TRY
	
	SELECT
		Amount AS Amount
		,CXNId AS CXNId
		, Fee AS Fee
		, [Description] AS [Description]
		, ConfirmationNumber AS ConfirmationNumber
		, RecipientId AS RecipientId
		, ExchangeRate AS ExchangeRate
		, TransferType AS TransferType
		, (
			CASE 
				WHEN LOWER(TransactionSubType) = 'modify'
				THEN 2
				WHEN LOWER(TransactionSubType) = 'refund'
				THEN 3
			END
		) AS TransactionSubType
		, OriginalTransactionID AS OriginalTransactionID
		, CustomerSessionId AS CustomerSessionId
		, ProviderId AS ProviderId
		, ProviderAccountId AS ProviderAccountId
		, Destination AS Destination
		, [State] AS [State]
	FROM tTxn_MoneyTransfer
	WHERE TransactionID = @transactionId

END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
