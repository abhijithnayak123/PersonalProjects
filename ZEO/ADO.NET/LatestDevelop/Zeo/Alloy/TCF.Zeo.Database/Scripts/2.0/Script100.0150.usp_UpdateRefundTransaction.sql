--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU Refund transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateRefundTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateRefundTransaction
GO


CREATE PROCEDURE usp_UpdateRefundTransaction
(
     @WUTrxId BIGINT
	,@originatorsPrincipalAmount DECIMAL(18,2)
	,@destinationPrincipalAmount DECIMAL(18,2)
	,@grossTotalAmount DECIMAL(18,2)
	,@charges DECIMAL(18,2)
	,@dtTerminalLastModified DATETIME
	,@dtServerLastModified DATETIME
)
AS
BEGIN
	
BEGIN TRY
	
	UPDATE [dbo].[tWUnion_Trx]
	SET 
	  [DTServerLastModified] = @dtServerLastModified
	  ,[DTTerminalLastModified] = @dtTerminalLastModified
	  ,[OriginatorsPrincipalAmount] = @originatorsPrincipalAmount
	  ,[DestinationPrincipalAmount] = @destinationPrincipalAmount
	  ,[GrossTotalAmount] = @grossTotalAmount
	  ,[Charges] = @charges
	WHERE [WUTrxID] = @WUTrxId
	
END TRY
BEGIN CATCH
    
	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
