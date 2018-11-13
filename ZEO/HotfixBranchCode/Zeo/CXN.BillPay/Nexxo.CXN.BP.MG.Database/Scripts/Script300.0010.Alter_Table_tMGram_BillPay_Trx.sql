-- ============================================================
-- Author:		<Raviraja>
-- Create date: <17/09/2014>
-- Description:	<Added additional dump columns for billpay> 
-- Rally ID:	<NA>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'BillerName')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	[BillerName] [varchar](65) NULL,
	[TextTranslation] [varchar](MAX) NULL
END
GO