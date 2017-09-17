-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Update Chexar invoice status
-- Jira ID:		<AL-7705>
-- ================================================================================

-- EXEC usp_UpdateChexarSimInvoiceStatus

IF OBJECT_ID(N'usp_UpdateChexarSimInvoiceStatus', N'P') IS NOT NULL
DROP PROC usp_UpdateChexarSimInvoiceStatus
GO

CREATE PROCEDURE usp_UpdateChexarSimInvoiceStatus
(
	@chexarSimInvoiceId INT,
	@status NVARCHAR(50),
	@dTServerLastModified DATETIME
)

AS
BEGIN
	BEGIN TRY

		UPDATE
		  tChxrSim_Invoice
		SET
		  Status = @status,
		  DTServerLastModified =  @dTServerLastModified

		WHERE 
		  ChxrSimInvoiceID = @chexarSimInvoiceId


	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END


