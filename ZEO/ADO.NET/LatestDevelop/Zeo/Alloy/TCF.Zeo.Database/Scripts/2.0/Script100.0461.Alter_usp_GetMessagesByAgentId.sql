--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Modified the Script to get approved check upto 30 mins
-- Jira ID:		<AL-7705>
-- ================================================================================

-- exec usp_GetMessagesByAgentId 500001, '02/02/2017 14:50:33'



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
		  (
		    ms.AgentId = @agentId
		    AND
		    ms.dtServerLastModified > @dtServerLastModified
		   )
		AND 
		IsActive = 1 

	END TRY

	BEGIN CATCH

	  EXECUTE usp_CreateErrorInfo

	END CATCH
END	
GO



