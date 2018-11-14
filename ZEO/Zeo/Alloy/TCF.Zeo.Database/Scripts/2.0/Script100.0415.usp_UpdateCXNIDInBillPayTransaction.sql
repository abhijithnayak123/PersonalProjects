--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <16-03-2017>
-- Description:	Updating the CXN ID of the PTNR transaction.
-- Jira ID:		<AL-8320>

--  EXEC usp_UpdateCXNIDInBillPayTransaction 1000000011 
-- ================================================================================

IF OBJECT_ID('usp_UpdateCXNIDInBillPayTransaction') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_UpdateCXNIDInBillPayTransaction
END
GO

CREATE PROCEDURE usp_UpdateCXNIDInBillPayTransaction
(
	  @transactionId          BIGINT,
	  @wuBillPayTrxID		  BIGINT,  
	  @dtTerminalLastModified DATETIME,
	  @dtServerLastModified   DATETIME
)
AS
BEGIN

	BEGIN TRY
		
		UPDATE 
			dbo.tTxn_BillPay
		SET
			CXNId = @wuBillPayTrxID,
			DTTerminalLastModified = @dtTerminalLastModified,
			DTServerLastModified = @dtServerLastModified
		WHERE 
			TransactionID = @transactionId

	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO