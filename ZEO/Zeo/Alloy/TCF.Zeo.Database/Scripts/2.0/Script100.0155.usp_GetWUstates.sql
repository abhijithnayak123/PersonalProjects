--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to get the WU states.
-- Jira ID:		<AL-8324>
-- ================================================================================
IF OBJECT_ID(N'usp_GetWUstates', N'P') IS NOT NULL
DROP PROC usp_GetWUstates
GO


CREATE PROCEDURE usp_GetWUstates
(
	@countryCode VARCHAR(50)
)
	
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY

    -- Insert statements for procedure here
	SELECT 
		WUStateID AS Id
		,StateCode AS Code
		,Name AS Name 
	FROM 
		tWUnion_States WITH (NOLOCK)
	WHERE 
		ISOCountryCode = @countryCode
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
