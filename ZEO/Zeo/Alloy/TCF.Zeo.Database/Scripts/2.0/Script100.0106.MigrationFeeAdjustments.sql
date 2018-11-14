--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Migration script update id columns
-- Jira ID:		<AL-7926>
-- ================================================================================

BEGIN TRY
  BEGIN TRAN

    -- UPDATE ChannelpartnerId in tChannelPartnerFeeAdjustments table
	UPDATE CFA SET CFA.ChannelPartnerId = CP.ChannelPartnerId FROM tChannelPartnerFeeAdjustments AS CFA
	   INNER JOIN tChannelPartners AS CP ON CP.ChannelPartnerPK = CFA.ChannelPartnerPK
	   
	-- UPDATE TransactionId in tTxn_FeeAdjustments table for check processing
	 
 	UPDATE TF SET TF.TransactionId = TC.TransactionID FROM tTxn_FeeAdjustments AS TF
		INNER JOIN tTxn_Check AS TC ON TF.TxnPK = TC.TxnPK

	   -- UPDATE TransactionId in tTxn_FeeAdjustments table money order
	 
 	UPDATE TF SET TF.TransactionId = TM.TransactionID FROM tTxn_FeeAdjustments AS TF 
		INNER JOIN tTxn_MoneyOrder AS TM ON TF.TxnPK = TM.TxnPK


   -- UPDATE FeeAdjustmentId in tFeeAdjustmentConditions table

	UPDATE FAC SET FAC.FeeAdjustmentId = CFA.FeeAdjustmentId FROM tFeeAdjustmentConditions AS FAC
	   INNER JOIN tChannelPartnerFeeAdjustments AS CFA ON FAC.FeeAdjustmentPK = CFA.FeeAdjustmentPK

    -- UPDATE FeeAdjustmentId in tTxn_FeeAdjustments table
	UPDATE TF SET TF.FeeAdjustmentId = CFA.FeeAdjustmentId FROM tTxn_FeeAdjustments AS TF
	   INNER JOIN tChannelPartnerFeeAdjustments AS CFA ON TF.FeeAdjustmentPK = CFA.FeeAdjustmentPK

     -- UPDATE FeeAdjustmentId in tCustomerFeeAdjustments table
	UPDATE CF SET CF.FeeAdjustmentId = CFA.FeeAdjustmentId FROM tCustomerFeeAdjustments AS CF
	   INNER JOIN tChannelPartnerFeeAdjustments AS CFA ON CF.FeeAdjustmentPK = CFA.FeeAdjustmentPK

	COMMIT TRAN
END TRY

BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;