-- ============================================================
-- Author:		<Pamila>
-- Create date: <24/09/2014>
-- Description:	<Added additional dump columns for billpay> 
-- Rally ID:	<NA>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'BillerAccountnumber')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	[ReceiverMiddleName] [varchar](40) NULL,
	[ReceiverLastName2] [varchar](40) NULL,
	[PurposeOfFund] [varchar](6) NULL,
	[TotalSendAmount] decimal(18,2) NULL,
	[MgiRewardsNumber] [varchar](20) NULL,
	[ValidateAccountNumber] [varchar](30) NULL
END
GO