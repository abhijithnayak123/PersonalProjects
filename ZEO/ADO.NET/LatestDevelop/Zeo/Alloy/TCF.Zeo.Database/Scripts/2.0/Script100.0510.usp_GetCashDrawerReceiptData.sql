-- ================================================================================
-- Author:		Ashok Kumar G
-- Create date: 2/8/2017
-- Description: Get Cash Drawer Receipt Data
-- Jira ID:		<8613>
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetCashDrawerReceiptData')
BEGIN
	DROP PROCEDURE usp_GetCashDrawerReceiptData
END
GO

CREATE PROCEDURE usp_GetCashDrawerReceiptData
(
       @agentId    BIGINT,
	   @locationId BIGINT
)
AS
BEGIN
    BEGIN TRY	
             DECLARE   @cashIn				 money
             DECLARE   @cashOut              money

						SELECT @cashIn = SUM(Amount) 
						FROM dbo.tTxn_Cash ttcn_Cash WITH (NOLOCK)
						     JOIN dbo.tCustomerSessions tcs WITH (NOLOCK)    ON ttcn_Cash.CustomerSessionId = tcs.CustomerSessionID
						     JOIN dbo.tAgentSessions tas WITH (NOLOCK)       ON tas.AgentSessionID = tcs.AgentSessionId
						     JOIN dbo.tTerminals tt WITH (NOLOCK)            ON tt.TerminalId =tas.TerminalId
						     JOIN dbo.tLocations tl WITH (NOLOCK)            ON tt.LocationId = tl.LocationID
						WHERE tt.LocationId = @locationId AND tas.AgentId = @agentId AND ttcn_Cash.CashType=1
        AND ttcn_Cash.DTTerminalCreate >= DATEADD(dd, DATEDIFF(dd, 0, GETDATE()) - 0, 0)
		AND ttcn_Cash.DTTerminalCreate < DATEADD(dd, DATEDIFF(dd, 0, GETDATE())+1, 0)
							 AND ttcn_Cash.[State] = 4 
							  
						SELECT @cashOut = SUM(Amount) 
						FROM dbo.tTxn_Cash ttcn_Cash WITH (NOLOCK)
							JOIN dbo.tCustomerSessions tcs WITH (NOLOCK)     ON ttcn_Cash.CustomerSessionId = tcs.CustomerSessionID
							JOIN dbo.tAgentSessions tas WITH (NOLOCK)        ON tas.AgentSessionID = tcs.AgentSessionId
							JOIN dbo.tTerminals tt WITH (NOLOCK)             ON tt.TerminalId =tas.TerminalId
							JOIN dbo.tLocations tl WITH (NOLOCK)             ON tt.LocationId = tl.LocationID
						WHERE tt.LocationId = @locationId AND tas.AgentId = @agentId AND ttcn_Cash.CashType=2
        AND ttcn_Cash.DTTerminalCreate >= DATEADD(dd, DATEDIFF(dd, 0, GETDATE()) - 0, 0)
		AND ttcn_Cash.DTTerminalCreate < DATEADD(dd, DATEDIFF(dd, 0, GETDATE())+1, 0)
							AND ttcn_Cash.[State] = 4 
                        SELECT
							@cashIn                    AS CashIn ,			
							@cashOut				   AS CashOut
      END TRY
	  BEGIN CATCH
	            EXECUTE usp_CreateErrorInfo
	  END CATCH
END

