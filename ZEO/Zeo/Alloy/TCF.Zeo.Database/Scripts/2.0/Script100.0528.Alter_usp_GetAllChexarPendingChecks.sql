--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <05-05-2017>
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
		 cp.Name AS ChannelPartnerName,
		 l.TimezoneID as LocationTimeZone,
		 cs.CustomerSessionId,
		 tc.DiscountName,
		 tc.IsSystemApplied,
		 tc.CXNId as CxnTrasactionId,
		 cs.CustomerID,
		 l.LocationIdentifier 
      FROM 
		 tChxr_Trx ct 
		 INNER JOIN tTxn_Check tc WITH (NOLOCK) ON ct.ChxrTrxID = tc.CXNId				 
		 INNER JOIN tCustomerSessions cs WITH (NOLOCK) ON cs.CustomerSessionID = tc.CustomerSessionId
		 INNER JOIN tAgentsessions a WITH (NOLOCK) ON cs.AgentSessionId = a.AgentSessionID
		 INNER JOIN tTerminals t WITH (NOLOCK) ON a.TerminalId = t.TerminalID
		 INNER JOIN tLocations l WITH (NOLOCK) ON t.LocationId = l.LocationID
		 INNER JOIN tLocationProcessorCredentials lp WITH (NOLOCK) ON l.LocationID = lp.LocationId AND lp.ProviderId = 200	 -- Chxr Provider Id
		 INNER JOIN tChannelPartners cp WITH (NOLOCK) ON t.ChannelPartnerId = cp.ChannelPartnerId
	  WHERE 
		 ct.Status = 1  AND tc.State = 1    -- Pending status
  END TRY
  BEGIN CATCH
    EXECUTE usp_CreateErrorInfo
  END CATCH
  
  	
END
GO

