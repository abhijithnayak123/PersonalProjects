--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Update Chexar Invoice details.
-- Jira ID:		<AL-7705>
-- ================================================================================

--EXEC usp_GetCheckAccount '1234567890'

IF OBJECT_ID(N'usp_UpdateChexarSimInvoice', N'P') IS NOT NULL
DROP PROC usp_UpdateChexarSimInvoice
GO

CREATE PROCEDURE usp_UpdateChexarSimInvoice
(
	@chexarSimInvoiceId BIGINT,
	@status NVARCHAR(50),
	@waitTime NVARCHAR(100),
	@declineCode INT,
	@declineReason NVARCHAR(300),
	@dTServerLastModified DATETIME
)

AS
BEGIN
	BEGIN TRY


		UPDATE
		  tChxrSim_Invoice
		SET
		  Status = @status,
		  WaitTime = @waitTime,
		  DeclineId  = @declineCode,
		  DeclineReason = @declineReason,
		  DTServerLastModified =  @dTServerLastModified
		WHERE
		  TicketId = @chexarSimInvoiceId        

	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END