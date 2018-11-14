--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <12-14-2016>
-- Description:	This SP is used to update the state for SM Modify and SM Refund transactions.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateStatesForTransactions', N'P') IS NOT NULL
DROP PROC usp_UpdateStatesForTransactions
GO


CREATE PROCEDURE usp_UpdateStatesForTransactions
(
	 @modifiedorRefundTransactionId BIGINT
	,@state                         INT
	,@dtTerminalDate                DATETIME
	,@dtServerDate                  DATETIME
)
AS
BEGIN
BEGIN TRY
	

		UPDATE mtc		   
		SET
			mtc.State = @state,
			mtc.DTServerLastModified = @dtServerDate,
			mtc.DTTerminalLastModified = @dtTerminalDate
		FROM 
		   tTxn_MoneyTransfer mt
		INNER JOIN 
		   tTxn_MoneyTransfer mtc
		   ON  mt.TransactionId = @modifiedorRefundTransactionId
		WHERE 
		   mt.OriginalTransactionId = mtc.OriginalTransactionId
	  
	
END TRY
BEGIN CATCH


	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
