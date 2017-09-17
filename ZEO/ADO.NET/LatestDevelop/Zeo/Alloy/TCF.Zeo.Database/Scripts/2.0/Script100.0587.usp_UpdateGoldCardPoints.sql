-- ========================================================
-- Author:		Abjijith Nayak
-- Create date: <07/24/2017>
-- Description:	<Stored Procedure to update the gold card points for WU>
-- ========================================================

IF OBJECT_ID(N'usp_UpdateGoldCardPoints', N'P') IS NOT NULL
DROP PROC usp_UpdateGoldCardPoints
GO

CREATE PROCEDURE usp_UpdateGoldCardPoints
 @transactionId	BIGINT,
 @productCode  INT,
 @totalPointsEarned VARCHAR(50)
AS
BEGIN
BEGIN TRY
	
	IF @productCode = 1005
	BEGIN
		UPDATE WUtrx		
		SET
		    WUtrx.WUCard_TotalPointsEarned = @totalPointsEarned
		FROM tWUnion_Trx WUtrx INNER JOIN 
		tTxn_MoneyTransfer mt ON mt.CXNId = WUtrx.WUTrxID 

		WHERE
			mt.TransactionID = @transactionId
	END
	ELSE
	BEGIN
		UPDATE WUBPtrx 
		SET
		    WUBPtrx.WUCard_TotalPointsEarned = @totalPointsEarned
		FROM tWUnion_BillPay_Trx WUBPtrx
		INNER JOIN tTxn_BillPay bp on bp.CXNId = WUBPtrx.WUBillPayTrxID
		WHERE
			bp.TransactionID = @transactionId
	END

	
END TRY

	BEGIN CATCH
	  EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO
