--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	 Update bill pay transactions
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_UpdateBillPayTransaction') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_UpdateBillPayTransaction
END
GO

CREATE PROCEDURE usp_UpdateBillPayTransaction
(
      @transactionId          BIGINT,
	  @amount                 MONEY,
	  @accountNumber          NVARCHAR(50),
	  @billerNameOrCode       NVARCHAR(255),	  
	  @channelPartnerId       BIGINT,	  
	  @dtTerminalLastModified DATETIME,
	  @dtServerLastModified   DATETIME
 )
AS
BEGIN
	BEGIN TRY

		DECLARE @productID BIGINT;

		--getting the productId from the tMasterCatalog table using the billerCode
		SELECT @productID = dbo.ufn_GetProductIdByBillerInfo(@billerNameOrCode, @channelPartnerId);

		UPDATE 
			dbo.tTxn_BillPay
		SET
			Amount = @amount,				
			AccountNumber = @accountNumber,				
			ProductId = @productID,
			Description = @productID,
			DTTerminalLastModified = @dtTerminalLastModified,
			DTServerLastModified = @dtServerLastModified
		WHERE 
			TransactionID = @transactionId
	END TRY

	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO
