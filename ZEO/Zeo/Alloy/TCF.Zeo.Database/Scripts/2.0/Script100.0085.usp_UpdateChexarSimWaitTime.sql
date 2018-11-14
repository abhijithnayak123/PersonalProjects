--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Update Chexar wait time.
-- Jira ID:		<AL-7705>
-- ================================================================================
--EXEC usp_GetCheckAccount '1234567890'

IF OBJECT_ID(N'usp_UpdateChexarSimWaitTime', N'P') IS NOT NULL
DROP PROC usp_UpdateChexarSimWaitTime
GO

CREATE PROCEDURE usp_UpdateChexarSimWaitTime
(
	@ticketId INT,
	@dTServerLastModified DATETIME
)

AS
BEGIN
	BEGIN TRY
	  
	    -- Update Wait time in Chexar invoice
		UPDATE
		  tChxrSim_Invoice
		SET
		  WaitTime = Convert(INT,WaitTime) - 1,
		  DTServerLastModified =  @dTServerLastModified
		WHERE 
		  TicketId = @ticketId

        -- select wait time and status from chexar sim invoice
        SELECT
		   Status,
		   WaitTime 
		FROM 
		   tChxrSim_Invoice WITH (NOLOCK)
		WHERE 
		  TicketId = @ticketId

	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END



