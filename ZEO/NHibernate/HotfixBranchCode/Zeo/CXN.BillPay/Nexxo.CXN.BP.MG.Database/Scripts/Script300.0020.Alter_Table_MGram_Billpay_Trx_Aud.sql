-- ============================================================
-- Author:		<Pamila>
-- Create date: <24/09/2014>
-- Description:	<Added IsValidateAccNumberRequired> 
-- Rally ID:	<2046>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = '[tMGram_Billpay_Trx_Aud]' AND COLUMN_NAME = 'IsValidateAccNumberRequired')
BEGIN
	ALTER TABLE [tMGram_Billpay_Trx_Aud] 
	ADD 
	IsValidateAccNumberRequired [bit] NULL

END
GO