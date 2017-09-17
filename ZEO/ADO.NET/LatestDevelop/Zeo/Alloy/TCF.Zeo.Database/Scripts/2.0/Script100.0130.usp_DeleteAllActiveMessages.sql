--- ===============================================================================
-- Author:		 Manikandan Govindraj
-- Description:	 Soft delete for All active messages
-- Jira ID:		<AL-8428>
-- ================================================================================

IF OBJECT_ID(N'usp_DeleteAllActiveMessages', N'P') IS NOT NULL
DROP PROC usp_DeleteAllActiveMessages
GO

CREATE PROCEDURE usp_DeleteAllActiveMessages	
	@ActiveMessages XML
AS
BEGIN

 BEGIN TRY
        
        SET NOCOUNT ON;
		
		IF OBJECT_ID('#TempAgentMessages') IS NOT NULL
	    DROP TABLE #TempAgentMessages

		SELECT
		[Table].[Column].value('TransactionId[1]', 'BIGINT') as 'TransactionId',
		[Table].[Column].value('DTServerLastModified[1]', 'DATETIME') as 'DTServerLastModified',
		[Table].[Column].value('DTTerminalLastModified[1]', 'DATETIME') as 'DTTerminalLastModified'
		INTO #TempAgentMessages
		FROM @ActiveMessages.nodes('/DocumentElement/ActiveMessages') as [Table]([Column])

		UPDATE mc 
		SET
		  mc.IsActive = 0,
		  mc.DTServerLastModified = tam.DTServerLastModified,
		  mc.DTTerminalLastModified = tam.DTTerminalLastModified
		FROM
		  tMessageCenter mc
		INNER JOIN
		  #TempAgentMessages tam 
		ON 
		   mc.TransactionId = tam.TransactionId

  END TRY
  BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

  END CATCH
END
GO



