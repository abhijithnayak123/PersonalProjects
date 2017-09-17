--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <12-13-2016>
-- Description:	Create procedure for get AgentBy SessionId
-- Jira ID:		<AL-7581>
-- ================================================================================


IF OBJECT_ID(N'usp_GetAgentBySessionId', N'P') IS NOT NULL
DROP PROCEDURE usp_GetAgentBySessionId   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_GetAgentBySessionId]
(
	@AgentsessionId  BIGINT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			ta.AgentID,
			ta.FirstName,
			ta.LastName,
			ta.UserRoleId,
			ta.PrimaryLocationId,
			ta.FullName,
			ta.UserStatusId
		FROM 
			tAgentDetails ta 
				INNER JOIN tAgentSessions ts ON ta.AgentID = ts.AgentId
		WHERE
			ts.AgentSessionID = @AgentsessionId
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END