--- ===============================================================================
-- Author:		<Purna pushkal>
-- Create date: <18-11-2016>
-- Description:	 Get WU bill pay transactions
-- Jira ID:		<AL-8320>
-- ================================================================================
IF OBJECT_ID('usp_GetWUBillPayTransaction') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_GetWUBillPayTransaction
END
GO

CREATE PROCEDURE usp_GetWUBillPayTransaction
(
	@WUTransactionID BIGINT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			tb.Amount,
			tb.Fee,
			tb.ConfirmationNumber,
			tbt.Customer_AccountNumber AS AccountNumber,
			tbt.BillerName,
			tbt.MTCN,
			tbt.NewMTCN,
			tbt.WUCard_TotalPointsEarned,
			tbt.financials_UndiscountedCharges,
			tbt.financials_TotalDiscount,
			tbt.WesternUnionCardNumber,
			tbt.FillingDate,
			tbt.FillingTime,
			tbt.promotions_sender_promo_code,
			tbt.promotions_promo_message,
			tbt.DeliveryCode,
			tbt.MessageArea
		FROM 
			dbo.tWUnion_BillPay_Trx tbt 
			INNER JOIN dbo.tTxn_BillPay tb ON tb.CXNId = tbt.WUBillPayTrxID
		WHERE 
			tbt.WUBillPayTrxID = @WUTransactionId
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO