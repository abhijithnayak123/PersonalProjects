--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Modified By :<Pushkal>
-- Create date: <08-3-2016>
-- Modified date: <12-01-2017>
-- Description:	 Get all the check types
-- Modified reason : Getting the provider Id also with the check types as part of story B-10860
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
			CheckTypeId as Id,
			ProductProviderCode AS ProductProviderCode 
		FROM 
		    tCheckTypes WITH (NOLOCK)

	END TRY

	BEGIN CATCH

	  EXECUTE usp_CreateErrorInfo

	END CATCH
END
GO


