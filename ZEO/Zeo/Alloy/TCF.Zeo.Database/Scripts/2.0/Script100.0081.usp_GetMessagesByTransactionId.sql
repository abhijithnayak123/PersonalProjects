--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get Agent message details by TransactionId
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_GetMessagesByTransactionId', N'P') IS NOT NULL
DROP PROC usp_GetMessagesByTransactionId
GO

CREATE PROCEDURE usp_GetMessagesByTransactionId
(
   @transactionId BIGINT
)
AS
BEGIN	
    BEGIN TRY

		SELECT
		  c.FirstName,
		  c.LastName,
		  ct.Amount,
		  ct.State as State,
		  cxt.TicketId
		FROM 
		  tMessageCenter ms WITH (NOLOCK)
		INNER JOIN 
		  tTxn_Check ct WITH (NOLOCK)
		ON
		  ms.TransactionId = ct.TransactionID
		INNER JOIN 
		  tChxr_Trx cxt WITH (NOLOCK)
		ON
		  cxt.ChxrTrxID = ct.CXNId
		INNER JOIN 
		  tCustomerSessions cs WITH (NOLOCK)
		ON
		  ct.CustomerSessionId = cs.CustomerSessionID
		INNER JOIN 
		  tCustomers c WITH (NOLOCK)
		ON 
		  cs.CustomerID = c.CustomerID
		WHERE
		  ms.TransactionId = @transactionId

	END TRY

	BEGIN CATCH

	  EXECUTE usp_CreateErrorInfo

	END CATCH
END
GO
