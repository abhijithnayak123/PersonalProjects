-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To Update Visa Account
-- =============================================


IF OBJECT_ID(N'usp_UpdateVisaTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateVisaTransaction
GO

CREATE PROCEDURE usp_UpdateVisaTransaction
	(
		@transactionID BIGINT,
		@confirmationId VARCHAR(100),
		@status INT,
		@dtTransmission DATETIME,
		@DTServerLastModified DATETIME,
		@DTTerminalLastModified DATETIME,
		@locationNodeId BIGINT
	)
AS
BEGIN
BEGIN TRY
	UPDATE
		tVisa_Trx
	SET
		 ConfirmationId = @confirmationId,
		 DTTransmission = @dtTransmission,
		 Status = @status,
		 LocationNodeId = @locationNodeId,
		 DTTerminalLastModified = @DTTerminalLastModified,
		 DTServerLastModified	= @DTServerLastModified
	Where 
		 VisaTrxID = @transactionID
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
