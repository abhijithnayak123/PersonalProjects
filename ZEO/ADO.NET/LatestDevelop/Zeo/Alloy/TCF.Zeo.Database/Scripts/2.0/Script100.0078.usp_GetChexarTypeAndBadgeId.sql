--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get Chexar batch Id and Chexar type id 
-- Jira ID:		<AL-7705>
-- ================================================================================

--Exec usp_GetChexarTypeAndBatchId 10029374665890,1

IF OBJECT_ID(N'usp_GetChexarTypeAndBadgeId', N'P') IS NOT NULL
DROP PROC usp_GetChexarTypeAndBadgeId 
GO


CREATE PROCEDURE usp_GetChexarTypeAndBadgeId 
	@chxrAccountId BIGINT,
	@checkType INT
AS
BEGIN
	BEGIN TRY
	
		DECLARE @badgeId INT 
		DECLARE @chexarTypeId INT 
		
		SELECT 
		   @badgeId = Badge
	    FROM
		   tChxr_Account WITH (NOLOCK)
		WHERE 
		   ChxrAccountID = @chxrAccountId 


	    SELECT TOP 1 
		   @chexarTypeId = ChexarTypeId 
	    FROM 
		   tChxr_CheckTypeMapping WITH (NOLOCK) 
		WHERE 
		   CheckType = @checkType

		
		SELECT  
		  @badgeId as BadgeId,
		  @chexarTypeId as ChexarTypeId

	END TRY

	BEGIN CATCH

	    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
    END CATCH
   
END
GO


