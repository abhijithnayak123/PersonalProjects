	--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get ALL Chexar transactions
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_GetAllChexarPendingChecks', N'P') IS NOT NULL
DROP PROC usp_GetAllChexarPendingChecks
GO

CREATE PROCEDURE usp_GetAllChexarPendingChecks	

AS
BEGIN

  BEGIN TRY

     SELECT 
	     tc.TransactionId,
		 lp.UserName,
		 lp.Password,
		 cp.ChannelPartnerId,
		 l.TimezoneID as LocationTimeZone,
		 cs.CustomerSessionId,
		 tc.DiscountName,
		 tc.IsSystemApplied
      FROM 
		 tChxr_Trx ct 
		 INNER JOIN tTxn_Check tc WITH (NOLOCK) ON ct.TransactionId = tc.TransactionId				 
		 INNER JOIN tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerSessionID = tc.CustomerSessionId
		 INNER JOIN tAgentsessions a WITH (NOLOCK) ON cs.AgentSessionPK = a.AgentSessionPK
		 INNER JOIN tTerminals t WITH (NOLOCK) ON a.TerminalPK = t.TerminalPK
		 INNER JOIN tLocations l WITH (NOLOCK) ON t.LocationPK = l.LocationPK
		 INNER JOIN tLocationProcessorCredentials lp WITH (NOLOCK) ON l.LocationPK = lp.LocationId AND lp.ProviderId = 200	 
		 INNER JOIN tChannelPartners cp WITH (NOLOCK) ON t.ChannelPartnerPK = cp.ChannelPartnerPK
	 
	  WHERE ct.Status = 1  -- Pending status
       

  END TRY
  BEGIN CATCH
    EXECUTE usp_CreateErrorInfo
  END CATCH
  
  	
END
GO

