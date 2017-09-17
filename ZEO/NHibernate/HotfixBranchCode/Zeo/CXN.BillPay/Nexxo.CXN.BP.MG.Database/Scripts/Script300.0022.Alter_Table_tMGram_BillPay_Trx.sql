-- ============================================================
-- Author:		<Pamila>
-- Create date: <08/12/2014>
-- Description:	<Added Missing fields of Receipts> 
-- Rally ID:	<2046>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'ExpectedPostingTimeFrame')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	ExpectedPostingTimeFrame [varchar](150) NULL,
	ExpectedPostingTimeFrameSecondary [varchar](150) NULL,
	CustomerTipTextTranslation [varchar](2000) NULL
END
GO