--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <02-22-2018>
-- Description:	 SP to save the Qulaifier details
-- Jira ID:		<B-13218>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_SaveQualifier')
BEGIN
	DROP PROCEDURE usp_SaveQualifier
END
GO

CREATE PROCEDURE usp_SaveQualifier
(
	 @promoQualifierId BIGINT
	,@promotionId BIGINT
	,@startDate DATE
	,@endDate DATE
	,@amount MONEY
	,@minTransactionCount INT
	,@productId INT
	,@transactionStates NVARCHAR(50)
	,@isPaidFee BIT
	,@isParked BIT
	,@dTServerDate DATETIME
	,@dTTerminalDate DATETIME
	,@dbQualifierId BIGINT OUTPUT
)
AS 
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION

	IF @promoQualifierId = 0
	BEGIN
		INSERT INTO tPromoQualifiers
		(
		    PromotionId,
		    StartDate,
		    EndDate,
		    Amount,
		    MinTransactionCount,
		    ProductId,
		    IsPaidFee,
		    TransactionStates,
		    IsParked,
		    DTServerCreate,
		    DTTerminalCreate
		)
		VALUES
		(
		    @promotionId,
		    @startDate,
		    @endDate,
		    @amount,
		    @minTransactionCount,
		    @productId,
		    @isPaidFee,
		    @transactionStates,
		    @isParked,
		    @dTServerDate,
		    @dTTerminalDate
		)

		SELECT @dbQualifierId = CAST(SCOPE_IDENTITY() AS BIGINT)

	END 
	ELSE
	BEGIN
		UPDATE tPromoQualifiers
		SET
			StartDate = @startDate,
			EndDate = @endDate,
			Amount = @amount,
			MinTransactionCount = @minTransactionCount,
			ProductId = @productId,
			IsPaidFee = @isPaidFee,
			TransactionStates = @transactionStates,
			IsParked = @isParked,
			DTServerLastModified = @dTServerDate, 
			DTTerminalLastModified = @dTTerminalDate
		WHERE PromoQualifierId = @promoQualifierId

		SELECT @dbQualifierId = @promoQualifierId

	END
	COMMIT
	END TRY

BEGIN CATCH
		ROLLBACK
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
END CATCH
END 