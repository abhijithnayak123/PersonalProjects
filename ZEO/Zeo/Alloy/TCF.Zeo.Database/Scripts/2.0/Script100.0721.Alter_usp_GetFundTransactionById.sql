-- ========================================================
-- Author:		<Kaushik Sakala>
-- Modified By : Pushkal
-- Create date: <11/15/2016>
-- Modified Date : 03/19/2018
-- Description:	<Stored Procedure to get fund transaction>
-- Modification : Getting the promo code joining the partner table
-- ========================================================

IF OBJECT_ID(N'usp_GetFundTransactionById', N'P') IS NOT NULL
DROP PROC usp_GetFundTransactionById
GO

CREATE PROCEDURE usp_GetFundTransactionById
@transactionId	BIGINT
AS
BEGIN
BEGIN TRY
	SELECT 
		tf.ProviderId,
		tf.ProviderAccountId,
		tf.Amount,
		tf.Fee,
		tf.[Description],
		tf.[State],
		tf.ConfirmationNumber,
		tf.FundType,
		tf.BaseFee,
		tf.AdditionalFee,
		tvt.PromoCode,
		tf.IsSystemApplied,
		tf.AddOnCustomerId
	FROM 
		tTxn_Funds tf 
		LEFT JOIN tVisa_Trx tvt WITH (NOLOCK) ON tf.CXNId = tvt.VisaTrxID
	WHERE
		tf.TransactionID = @transactionId
END TRY

	BEGIN CATCH
	  EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO
