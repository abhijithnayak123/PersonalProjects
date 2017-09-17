-- ================================================================================
-- Author:		Nishad Varghese
-- Create date: 01/Sep/2016
-- Description:	Update MO fee
-- Jira ID:		AL-7706
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE TYPE = 'P' and NAME = 'usp_UpdateMoneyOrderFee'
)
BEGIN
	DROP PROCEDURE usp_UpdateMoneyOrderFee
END
GO


CREATE PROCEDURE [dbo].[usp_UpdateMoneyOrderFee]
(
	@transactionId BIGINT,
	@amount MONEY,
	@baseFee MONEY,
	@fee MONEY,
	@additionalFee MONEY,
	@discountName VARCHAR(50),
	@discountDescription VARCHAR(100),
	@isSystemApplied BIT,
	@discountApplied MONEY,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME
)
AS
BEGIN
	BEGIN TRY
		UPDATE tTxn_MoneyOrder
		SET
			Fee = @fee,
			BaseFee = @baseFee,
			AdditionalFee = @additionalFee,
			DiscountName = @discountName,
			DiscountDescription = @discountDescription,
			IsSystemApplied = @isSystemApplied,
			DiscountApplied = @discountApplied,
			DTServerLastModified = @dTServerLastModified,
			DTTerminalLastModified = @dTTerminalLastModified
		WHERE
			TransactionId = @transactionId
	END TRY
	BEGIN CATCH	        
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

GO

