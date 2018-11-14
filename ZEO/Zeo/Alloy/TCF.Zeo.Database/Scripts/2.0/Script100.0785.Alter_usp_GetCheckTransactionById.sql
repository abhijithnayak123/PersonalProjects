--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <07-12-2018>
-- Description:	Get Check transaction by TransactionId
-- ================================================================================

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
		tc.ShoppingCartDescription,
		tc.ConfirmationNumber,
		tc.DiscountApplied,
		tc.DiscountName,
		tc.BaseFee,
		tc.CheckType,
		tc.ProviderId,
		tc.DiscountDescription,
		tc.AdditionalFee,
		tc.IsSystemApplied,
		tc.MICR,
		tc.ProviderAccountId,
		tc.State as State,
		cpc.FrankData,
		tc.CXNId as CxnTransactionId
	 FROM
		tTxn_Check tc WITH (NOLOCK)	
	 INNER JOIN tCustomerSessions cs WITH (NOLOCK) ON tc.CustomerSessionId = cs.CustomerSessionID
	 INNER JOIN tCustomers c WITH (NOLOCK) ON c.CustomerID = cs.CustomerID
	 INNER JOIN tChannelPartners cp WITH (NOLOCK) ON c.ChannelPartnerId = cp.ChannelPartnerId
	 INNER JOIN tChannelPartnerConfig cpc WITH (NOLOCK) ON cp.ChannelPartnerId = cpc.ChannelPartnerID
	 WHERE
	    tc.TransactionId = @transactionId
	 
	END TRY
	BEGIN CATCH

		EXECUTE usp_CreateErrorInfo

	END CATCH
END
GO


