--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	 Update bill pay transaction fee and confirmation number
-- Jira ID:		<AL-8321>
-- ================================================================================

IF OBJECT_ID('usp_UpdateBillPayTransactionFee') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_UpdateBillPayTransactionFee
END
GO

CREATE PROCEDURE usp_UpdateBillPayTransactionFee
(
      @transactionId          BIGINT,
	  @state                  INT,
	  @amount                 MONEY,
	  @fee                    MONEY,
	  @confirmationNumber     NVARCHAR(50),
	  @dtTerminalLastModified DATETIME,
	  @dtServerLastModified   DATETIME
 )
AS
BEGIN
	BEGIN TRY
		UPDATE 
			tTxn_BillPay
		SET
			Amount = @amount,				
			ConfirmationNumber = @confirmationNumber,				
			fee = @fee,
			[State] = @state,
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
