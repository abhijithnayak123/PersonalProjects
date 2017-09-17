-- =============================================
-- Author:		<Kaushik Sakala>
-- Create date: <16/11/2016>
-- Description:	Update fund details by transaction Id
-- Jira ID : <AL-8323>
-- =============================================

IF OBJECT_ID(N'usp_UpdateFundTransactionById', N'P') IS NOT NULL
	DROP PROC usp_UpdateFundTransactionById
GO


CREATE PROCEDURE usp_UpdateFundTransactionById
(
	@transactionId	BIGINT,
	@amount MONEY,
	@fee MONEY,
	@description NVARCHAR(255),
	@state INT,
	@confirmationNumber VARCHAR(50),
	@baseFee MONEY,
	@additionalFee MONEY,
	@discountName VARCHAR(50),
	@isSystemApplied BIT,
	@discountApplied MONEY,
	@fundsTypeId INT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME
)
AS
BEGIN
BEGIN TRY
	UPDATE tTxn_Funds
	SET
		Amount = @amount,
		Fee = @fee,
		Description = @description,
		[State] =	@state,
		ConfirmationNumber = @confirmationNumber,
		baseFee = @baseFee,
		AdditionalFee = @additionalFee,
		DiscountName = @discountName,
		IsSystemApplied = @isSystemApplied,
		discountApplied = @discountApplied,
		FundType = @fundsTypeId,
		dTTerminalLastModified = @dTTerminalLastModified,
		dTServerLastModified = @dTServerLastModified 
	WHERE 
		Transactionid = @transactionId
END TRY

	BEGIN CATCH
	  EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO
