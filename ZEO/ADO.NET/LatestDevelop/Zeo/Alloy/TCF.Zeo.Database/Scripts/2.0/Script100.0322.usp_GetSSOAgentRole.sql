--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <12-13-2016>
-- Description:	Create procedure for Get SSO Agent Role
-- Jira ID:		<AL-7581>
-- ================================================================================


IF OBJECT_ID(N'usp_GetSSOAgentRole', N'P') IS NOT NULL
DROP PROCEDURE usp_GetSSOAgentRole   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_GetSSOAgentRole]
(
@AgentId       BIGINT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
		  UserRoleId
    FROM 
		tAgentDetails ta 
	WHERE
		ta.AgentID = @AgentId 
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END