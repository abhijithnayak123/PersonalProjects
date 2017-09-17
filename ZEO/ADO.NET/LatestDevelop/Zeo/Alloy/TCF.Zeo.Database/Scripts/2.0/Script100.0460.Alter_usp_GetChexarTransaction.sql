	--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get Chexar transactions
-- Jira ID:		<AL-7705>
-- ================================================================================

 -- EXEC usp_GetChexarTransaction 1000000002

IF OBJECT_ID(N'usp_GetChexarTransaction', N'P') IS NOT NULL
DROP PROC usp_GetChexarTransaction
GO

CREATE PROCEDURE usp_GetChexarTransaction	
(
	@transactionId BIGINT
)
AS
BEGIN		
		
	BEGIN TRY   
		
	SELECT 
		ct.Amount,
		ChexarAmount,
		ChexarFee,
		CheckDate,
		CheckNumber,
		RoutingNumber,
		AccountNumber,
		ct.Micr,
		Latitude,
		Longitude,
		InvoiceId,
		TicketId,
		WaitTime,
		Status,
		ChexarStatus,
		SubmitType,
		ReturnType,
		DeclineCode,
		Message,
		Location,
		ChannelPartnerID,
		IsCheckFranked,
		ChxrAccountId,
		tc.DiscountDescription,
		tc.DiscountApplied,
		tc.DiscountName,
		tcctm.CheckType AS DmsReturnType,
		tc.BaseFee,
		tci.Front AS FrontImage
	FROM 
	    tChxr_Trx ct WITH (NOLOCK)
		INNER JOIN tTxn_Check tc WITH (NOLOCK)
		  ON ct.ChxrTrxID = tc.CXNId
		INNER JOIN tChxr_CheckTypeMapping tcctm WITH (NOLOCK)
		  ON ct.ReturnType = tcctm.ChexarTypeId
		INNER JOIN tCheckImages tci WITH (NOLOCK)
		  ON tci.transactionId = tc.TransactionID
	WHERE
		ct.ChxrTrxID = @transactionId

  END TRY

  BEGIN CATCH			
		EXECUTE usp_CreateErrorInfo;  
  END CATCH

END
GO

