-- ============================================================
-- Author:		<Pamila>
-- Create date: <24/09/2014>
-- Description:	<Added IsValidateAccNumberRequired> 
-- Rally ID:	<2046>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'IsValidateAccNumberRequired')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	IsValidateAccNumberRequired [bit] NULL

END
GO