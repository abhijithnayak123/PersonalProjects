--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Update Check transaction state
-- Jira ID:		<AL-7705>
-- ================================================================================
--Exec usp_UpdateChexarTransactionState
  
IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateChexarTransactionStatus]') AND TYPE IN (N'P'))
DROP PROCEDURE usp_UpdateChexarTransactionStatus
GO

CREATE PROCEDURE usp_UpdateChexarTransactionStatus
	@transactionId BIGINT,
	@status INT,
	@chexarStatus NVARCHAR(50),
	@dTServerLastModified DATETIME,
	@dTTerminalLastModified DATETIME
AS
BEGIN
	
	BEGIN TRY

		    UPDATE
		        tChxr_Trx
			SET
				Status = @status,
				ChexarStatus = @chexarStatus,
				DTServerLastModified = @dTServerLastModified,
				DTTerminalLastModified = @dTTerminalLastModified
			WHERE 
				ChxrTrxId = @transactionId
	END TRY
	BEGIN CATCH

		EXEC usp_CreateErrorInfo

	END CATCH

END
GO
