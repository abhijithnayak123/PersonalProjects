--- ===============================================================================
-- Author:		<Nishad Varghese>
-- Create date: <08-04-2016>
-- Description:	Migration scripts
-- Jira ID:		<AL-7706>
-- ================================================================================

BEGIN TRY
BEGIN TRAN

	---- Update the TransactionId in tMoneyOrderImage
	UPDATE CP SET CP.TransactionId = C.TransactionId
	FROM tMoneyOrderImage AS CP
	INNER JOIN tTxn_MoneyOrder AS C
	ON CP.TrxId = C.TxnPK

	---- Update the CustomerSessionId in tTxn_MoneyOrder
	UPDATE CP SET CP.CustomerSessionId = C.CustomerSessionId
	FROM tTxn_MoneyOrder AS CP
	INNER JOIN tCustomerSessions AS C
	ON CP.CustomerSessionPK = C.CustomerSessionPK


	---- Update the MICR in tTxn_MoneyOrder
	UPDATE CP SET CP.MICR = C.MICR
	FROM tTxn_MoneyOrder AS CP
	INNER JOIN tTxn_MoneyOrder_Stage AS C
	ON CP.CXEId = C.MoneyOrderID


	---- Update the PurchaseDate in tTxn_MoneyOrder
	UPDATE CP SET CP.PurchaseDate = C.PurchaseDate
	FROM tTxn_MoneyOrder AS CP
	INNER JOIN tTxn_MoneyOrder_Stage AS C
	ON CP.CXEId = C.MoneyOrderID

	
	-------------Alter new columns as not null------------------------

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'CustomerSessionId')
	BEGIN
		ALTER TABLE tTxn_MoneyOrder 
		ALTER COLUMN CustomerSessionId BIGINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMoneyOrderImage' AND COLUMN_NAME = 'TransactionId')
	BEGIN
		ALTER TABLE tMoneyOrderImage 
		ALTER COLUMN TransactionId BIGINT NOT NULL
	END

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



