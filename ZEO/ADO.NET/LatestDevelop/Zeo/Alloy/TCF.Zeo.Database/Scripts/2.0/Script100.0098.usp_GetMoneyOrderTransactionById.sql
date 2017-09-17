-- ================================================================================
-- Author:		Nishad Varghese
-- Create date: 01/Sep/2016
-- Description:	Get MO transaction by Id
-- Jira ID:		AL-7706
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE TYPE = 'P' and NAME = 'usp_GetMoneyOrderTransactionById'
)
BEGIN
	DROP PROCEDURE usp_GetMoneyOrderTransactionById
END
GO


CREATE PROCEDURE [dbo].[usp_GetMoneyOrderTransactionById]
(
	@transactionId BIGINT
)
AS 
BEGIN
	BEGIN TRY
		SELECT 
			[Amount],
			[Fee],
			[Description],
			[State],
			[BaseFee],
			[DiscountApplied],
			[AdditionalFee],
			[DiscountName],
			[DiscountDescription],
			[IsSystemApplied],
			[CheckNumber],
			[AccountNumber],
			[RoutingNumber],
			[MICR],
			[CustomerSessionId],
			[PurchaseDate],
			[DTTerminalCreate],
			[DTTerminalLastModified]	
		FROM [dbo].[tTxn_MoneyOrder]
		WHERE 
			[TransactionId] = @transactionId
	END TRY

	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

GO

