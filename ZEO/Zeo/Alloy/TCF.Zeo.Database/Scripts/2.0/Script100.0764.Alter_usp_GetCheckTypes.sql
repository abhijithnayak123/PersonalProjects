--- ===============================================================================
-- Author:		<M.Pushkal>
-- Create date: <11-06-2018>
-- Description:	Getting the checktype and getting the display name instaed of Name
-- ================================================================================

IF OBJECT_ID(N'usp_GetCheckTypes', N'P') IS NOT NULL
DROP PROC usp_GetCheckTypes
GO

CREATE PROCEDURE usp_GetCheckTypes
AS
BEGIN	
    BEGIN TRY

		SELECT 
			DisplayName AS Name,
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


