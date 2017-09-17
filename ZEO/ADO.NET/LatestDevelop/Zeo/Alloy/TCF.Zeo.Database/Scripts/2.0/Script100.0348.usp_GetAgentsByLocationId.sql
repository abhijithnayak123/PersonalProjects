--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <06-02-2017>
-- Description: Get the agents by location Id
-- Jira ID:		<AL-7581>
-- ================================================================================


IF OBJECT_ID(N'usp_GetAgentsByLocationId', N'P') IS NOT NULL
DROP PROCEDURE usp_GetAgentsByLocationId   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_GetAgentsByLocationId]
(
	@LocationId  BIGINT
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
		WHERE
			ta.PrimaryLocationId = @LocationId
	END TRY

	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END