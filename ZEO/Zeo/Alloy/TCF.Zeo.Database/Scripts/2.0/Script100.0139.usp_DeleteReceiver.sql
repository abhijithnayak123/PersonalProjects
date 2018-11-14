--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to delete a faviourate receiver through UI of the application.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_DeleteReceiver', N'P') IS NOT NULL
DROP PROC usp_DeleteReceiver
GO


CREATE PROCEDURE usp_DeleteReceiver
(
    @receiverId BIGINT
	,@status VARCHAR(20)
	,@dtServerLastModified DATETIME = NULL
	,@dtTerminalLastModified DATETIME = NULL
)
AS
BEGIN
	
BEGIN TRY
	
	UPDATE tWUnion_Receiver
	SET [Status] = @status
		, DTTerminalLastModified = @dtTerminalLastModified
		, DTServerLastModified = @dtServerLastModified
	WHERE WUReceiverID = @receiverId
	
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
