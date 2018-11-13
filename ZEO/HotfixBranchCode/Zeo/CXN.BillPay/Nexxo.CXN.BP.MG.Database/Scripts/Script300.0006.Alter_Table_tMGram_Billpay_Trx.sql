-- ============================================================
-- Author:		<Ratheesh PK>
-- Create date: <08/09/2014>
-- Description:	<Added additional dump columns for billpay> 
-- Rally ID:	<NA>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'ReferenceNumber')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	[ReferenceNumber] [varchar](50) NULL,
	[PartnerConfirmationNumber] [varchar](100) NULL,
	[PartnerName] [varchar](250) NULL,
	[FreePhoneCallPin] [varchar](250) NULL,
	[TollFreePhoneNumber] [varchar](250) NULL,
	[ExpectedDateOfDelivery] [datetime] NULL,
	[TransactionDateTime] [datetime] NULL,
    [AccountNumber] [varchar](100) NULL
	

END
GO