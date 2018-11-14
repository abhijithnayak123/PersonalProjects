--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Get Chexar sim invoice details by InvoiceId.
-- Jira ID:		<AL-7705>
-- ================================================================================

--EXEC usp_GetChexarSimInvoice

IF OBJECT_ID(N'usp_GetChexarSimInvoice', N'P') IS NOT NULL
DROP PROC usp_GetChexarSimInvoice
GO

CREATE PROCEDURE usp_GetChexarSimInvoice
(	
	@chexarSimInvoiceId INT
)

AS
BEGIN
	BEGIN TRY

		SELECT
		   csi.Amount,
		   csi.Fee,
		   csi.CheckType,
		   csi.ChxrSimAccountId,
		   csi.DeclineId,
		   csi.DeclineReason,
		   csi.Status,
		   csi.TicketId,
		   csi.WaitTime,
		   csa.Badge as BadgeId
		FROM 
		  tChxrSim_Invoice csi
		  INNER JOIN
		  tChxrSim_Account csa
		  ON csi.ChxrSimAccountId = csa.ChxrSimAccountId
		WHERE 
		  ChxrSimInvoiceID = @chexarSimInvoiceId
        
	END TRY
	
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END


