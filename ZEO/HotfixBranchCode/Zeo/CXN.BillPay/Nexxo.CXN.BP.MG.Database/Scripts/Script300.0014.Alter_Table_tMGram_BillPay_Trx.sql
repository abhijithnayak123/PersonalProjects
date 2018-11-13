-- ============================================================
-- Author:		<Pamila>
-- Create date: <24/09/2014>
-- Description:	<Added additional dump columns for billpay> 
-- Rally ID:	<NA>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'cardExpirationMonth')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	[CardExpirationMonth] [varchar](2) NULL,
	[CardExpirationYear] [varchar](4) NULL

END
GO