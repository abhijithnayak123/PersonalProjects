	-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<As an engineer, I want to implement ADO.Net for Terminal module>
-- Jira ID:		<AL-7583>
-- ================================================================================
IF EXISTS (	SELECT  1 	FROM sys.objects WHERE NAME = 'usp_UpdateAgentSession')
BEGIN DROP PROCEDURE usp_UpdateAgentSession END
GO

CREATE PROCEDURE usp_UpdateAgentSession
	@agentSessionId BIGINT,
	@terminalId BIGINT,
	@terminalName VARCHAR(100),
	@dTServerCreate DATETIME
AS   
BEGIN

   BEGIN TRY
			Update dbo.tAgentSessions 
			SET
				TerminalID = @terminalId,
				DTServerLastModified = 	@dTServerCreate
			Where 
			AgentSessionID = @agentSessionId 
  END TRY
  BEGIN CATCH
        
	EXECUTE usp_CreateErrorInfo  

  END CATCH
	
END
GO