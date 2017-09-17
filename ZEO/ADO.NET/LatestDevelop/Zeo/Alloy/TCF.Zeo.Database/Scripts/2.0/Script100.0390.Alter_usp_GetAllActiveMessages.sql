	--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get all Active Messages from message center
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_GetAllActiveMessages', N'P') IS NOT NULL
DROP PROC usp_GetAllActiveMessages
GO

CREATE PROCEDURE usp_GetAllActiveMessages	
AS
BEGIN

  BEGIN TRY

     SELECT 
	     tc.TransactionId,
		 l.TimezoneID as LocationTimeZone
      FROM 
		 tMessageCenter ct 
		 INNER JOIN tTxn_Check tc WITH (NOLOCK) ON ct.TransactionId = tc.TransactionId				 
		 INNER JOIN tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerSessionID = tc.CustomerSessionId
		 INNER JOIN tAgentsessions a WITH (NOLOCK) ON cs.AgentSessionId = a.AgentSessionID
		 INNER JOIN tTerminals t WITH (NOLOCK) ON a.TerminalId = t.TerminalId
		 INNER JOIN tLocations l WITH (NOLOCK) ON t.LocationId = l.LocationId
	  WHERE ct.IsActive = 1  
       

  END TRY
  BEGIN CATCH
    EXECUTE usp_CreateErrorInfo
  END CATCH
  
  	
END
GO