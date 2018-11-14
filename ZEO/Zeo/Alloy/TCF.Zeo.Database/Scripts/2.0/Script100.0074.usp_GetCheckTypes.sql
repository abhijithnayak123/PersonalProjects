--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get all the check types
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_GetCheckTypes', N'P') IS NOT NULL
DROP PROC usp_GetCheckTypes
GO

CREATE PROCEDURE usp_GetCheckTypes
AS
BEGIN	
    BEGIN TRY

		SELECT 
			Name,
			CheckTypeId as Id 
		FROM 
		    tCheckTypes WITH (NOLOCK)

	END TRY

	BEGIN CATCH

	  EXECUTE usp_CreateErrorInfo

	END CATCH
END
GO


