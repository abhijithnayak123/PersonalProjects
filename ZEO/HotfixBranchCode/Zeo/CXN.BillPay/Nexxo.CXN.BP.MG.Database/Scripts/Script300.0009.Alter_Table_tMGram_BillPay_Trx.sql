-- ============================================================
-- Author:		<Pamila Jose>
-- Create date: <17/09/2014>
-- Description:	<Added additional dump columns for billpay> 
-- Rally ID:	<NA>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'SenderMiddleName')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	[SenderMiddleName] [varchar](250) NULL,
	[SenderLastName2] [varchar](250) NULL,
	[MessageField1] [varchar](50) NULL,
	[MessageField2] [varchar](50) NULL,
	[SenderDOB] [Datetime] NULL,
	[SenderOccupation] [varchar](30) NULL,
	[SenderLegalIdNumber] [varchar](14) NULL,
	[SenderLegalIdType][varchar](5) NULL,
	[SenderPhotoIdCountry][varchar](3) NULL,
	[SenderPhotoIdState][varchar](2) NULL,
	[SenderPhotoIdNumber][varchar](20) NULL,
	[SenderPhotoIdType][varchar](3) NULL
END
GO