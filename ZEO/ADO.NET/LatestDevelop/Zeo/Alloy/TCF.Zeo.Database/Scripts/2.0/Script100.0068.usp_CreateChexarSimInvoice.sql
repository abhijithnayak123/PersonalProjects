-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Update Chexar invoice status
-- Jira ID:		<AL-7705>
-- ================================================================================
-- EXEC usp_CreateChexarSimInvoice

IF OBJECT_ID(N'usp_CreateChexarSimInvoice', N'P') IS NOT NULL
DROP PROC usp_CreateChexarSimInvoice
GO

CREATE PROCEDURE usp_CreateChexarSimInvoice
(
	@chxrSimInvoiceId BIGINT OUTPUT,
	@badgeId INT,
	@declineId INT,
	@declineReason NVARCHAR(300),
	@amount MONEY,
	@checkType INT,
	@fee MONEY,
	@status NVARCHAR(50),
	@ticketId INT,
	@waitTime NVARCHAR(100),	
	@dTServerCreate DATETIME
)

AS
BEGIN
  BEGIN TRY
	
	 DECLARE @chxrSimAccountId BIGINT

	 SELECT 
	    @chxrSimAccountId = chxrSimAccountId 
	 FROM 
	    tChxrSim_Account WITH (NOLOCK)
	 WHERE
	    badge = @badgeId

	
	 INSERT INTO 
		 tChxrSim_Invoice
		 (
			TicketId,
			Amount,
			Fee,
			CheckType,
			Status,
			WaitTime,
			DeclineId,
			DeclineReason,
			ChxrSimAccountId,
			DTServerCreate
		 )
		 VALUES 
		 (
		    @ticketId,
		    @amount,
		    @fee,
			@checkType,
			@status,
			@waitTime,
			@declineId,
			@declineReason,
			@chxrSimAccountId,
			@dTServerCreate
		 )

	SELECT @chxrSimInvoiceId = SCOPE_IDENTITY() 
 
  END TRY
	
  BEGIN CATCH
	 EXECUTE usp_CreateErrorInfo
  END CATCH
END


