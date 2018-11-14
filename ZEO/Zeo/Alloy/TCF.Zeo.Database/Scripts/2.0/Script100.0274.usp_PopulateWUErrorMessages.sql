--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU Error Messages
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateWUErrorMessages', N'P') IS NOT NULL
DROP PROC usp_PopulateWUErrorMessages
GO

CREATE PROCEDURE usp_PopulateWUErrorMessages	
	@Messages XML
AS
BEGIN

 BEGIN TRY
        BEGIN TRANSACTION
        SET NOCOUNT ON;
		
		IF OBJECT_ID('#TempMessages') IS NOT NULL
			DROP TABLE #TempMessages
		
		
		IF((SELECT COUNT(1) FROM tWUnion_ErrorMessages)> 0)
		BEGIN
			DELETE FROM tWUnion_ErrorMessages
		END

		SELECT
			[Table].[Column].value('ErrorCode[1]', 'VARCHAR(20)') as 'ErrorCode',
			[Table].[Column].value('ErrorDesc[1]', 'VARCHAR(200)') as 'ErrorDesc',
			[Table].[Column].value('DTServerCreate[1]', 'DATETIME') as 'DTServerCreate'
		INTO #TempMessages
		FROM @Messages.nodes('/DocumentElement/Messages') as [Table]([Column])

		INSERT INTO 
			tWUnion_ErrorMessages ([ErrorCode],[ErrorDesc],[DTServerCreate]) 
			(SELECT ts.ErrorCode,ts.ErrorDesc,ts.DTServerCreate FROM #TempMessages ts)
		
        COMMIT TRANSACTION;	          
  END TRY
  BEGIN CATCH

    IF @@TRANCOUNT > 0 
    BEGIN   	  
        ROLLBACK TRANSACTION 		
    END

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

  END CATCH
END
GO



