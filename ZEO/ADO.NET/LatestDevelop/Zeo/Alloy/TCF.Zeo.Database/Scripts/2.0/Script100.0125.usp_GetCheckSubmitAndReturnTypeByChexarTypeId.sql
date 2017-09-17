--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Get Check submit and return type by chexar type id
-- Jira ID:		<AL-7837>
-- ================================================================================

-- EXEC usp_GetCheckSubmitAndReturnTypeByChexarTypeId 18,19,1,2

IF OBJECT_ID(N'usp_GetCheckSubmitAndReturnTypeByChexarTypeId', N'P') IS NOT NULL
DROP PROC usp_GetCheckSubmitAndReturnTypeByChexarTypeId
GO


CREATE PROCEDURE usp_GetCheckSubmitAndReturnTypeByChexarTypeId
(
    @chxrSubmitType INT,
	@chxrReturnType INT,
	@submitType INT OUTPUT,
	@returnType INT OUTPUT
)
AS
BEGIN
	
BEGIN TRY
	    	    
		SELECT @submitType = CheckType FROM tChxr_CheckTypeMapping WITH (NOLOCK)
		WHERE ChexarTypeId = @chxrSubmitType

		SELECT @returnType = CheckType FROM tChxr_CheckTypeMapping WITH (NOLOCK)
		WHERE ChexarTypeId = @chxrReturnType		
    
END TRY

BEGIN CATCH
    
	EXECUTE usp_CreateErrorInfo
		
END CATCH

END
GO
