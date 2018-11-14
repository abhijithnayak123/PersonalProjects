--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-20-2016>
-- Description:	Get Check transaction by TransactionId
-- Jira ID:		<AL-7705>
-- ================================================================================

-- EXEC usp_GetCheckTransactionById 1000000047

IF OBJECT_ID(N'usp_GetCheckTransactionById', N'P') IS NOT NULL
DROP PROC usp_GetCheckTransactionById
GO

CREATE PROCEDURE usp_GetCheckTransactionById	
(
	@transactionId BIGINT
)
AS
BEGIN

	BEGIN TRY
	 SELECT     
		tc.Amount,
		tc.Fee,
		tc.Description,
		tc.ConfirmationNumber,
		tc.DiscountApplied,
		tc.DiscountName,
		ct.Message,
		ct.CheckNumber,
		tc.BaseFee,
		tc.CheckType,
		ct.DeclineCode,
		tc.ProviderId,
		ct.ChexarStatus,
		tc.DiscountDescription,
		tc.AdditionalFee,
		tc.Description,
		tc.IsSystemApplied,
		tc.MICR,
		tc.ProviderAccountId,
		tc.ProviderId,
		tc.State as State,
		cpc.FrankData
	 FROM
		tTxn_Check tc WITH (NOLOCK)	

	 INNER JOIN	tChxr_Trx AS ct WITH (NOLOCK) ON ct.TransactionId = tc.TransactionId
	 INNER JOIN tCustomerSessions cs WITH (NOLOCK) ON tc.CustomerSessionId = cs.CustomerSessionID
	 INNER JOIN tCustomers c WITH (NOLOCK) ON c.CustomerID = cs.CustomerID
	 INNER JOIN tChannelPartners cp WITH (NOLOCK) ON c.ChannelPartnerId = cp.ChannelPartnerId
	 INNER JOIN tChannelPartnerConfig cpc WITH (NOLOCK) ON cp.ChannelPartnerPK = cpc.ChannelPartnerPK
	
	 WHERE
	    tc.TransactionId = @transactionId
	 
	END TRY
	BEGIN CATCH

		EXECUTE usp_CreateErrorInfo

	END CATCH
END
GO


