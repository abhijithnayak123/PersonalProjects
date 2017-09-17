--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-05-2017>
-- Description:	 Pending check were not displaying in the message center.
-- Jira ID:		<AL-7705>
-- ================================================================================

-- exec usp_GetMessagesByAgentId 500001, '04/05/2017 14:40:33'

IF OBJECT_ID(N'usp_GetMessagesByAgentId', N'P') IS NOT NULL
DROP PROC usp_GetMessagesByAgentId
GO

CREATE PROCEDURE usp_GetMessagesByAgentId
(
   @agentId BIGINT,
   @dtServerLastModified DATETIME
)
AS
BEGIN	
    BEGIN TRY

		SELECT
		  c.FirstName,
		  c.LastName,
		  ms.TransactionId,
		  ct.Amount,
		  ct.State as State,
		  cxt.TicketId,
		  cxt.Message
		FROM 
		  tMessageCenter ms WITH (NOLOCK)
			INNER JOIN tTxn_Check ct WITH (NOLOCK) ON ms.TransactionId = ct.TransactionID
			INNER JOIN  tChxr_Trx cxt WITH (NOLOCK) ON cxt.ChxrTrxID = ct.CXNId
			INNER JOIN tCustomerSessions cs WITH (NOLOCK) ON ct.CustomerSessionId = cs.CustomerSessionID
			INNER JOIN tCustomers c WITH (NOLOCK) ON cs.CustomerID = c.CustomerID
		WHERE
		    ms.AgentId = @agentId AND IsActive = 1
		    AND (ms.dtServerLastModified >= @dtServerLastModified OR ct.State = 1) 

	END TRY

	BEGIN CATCH

	  EXECUTE usp_CreateErrorInfo

	END CATCH
END	
GO



