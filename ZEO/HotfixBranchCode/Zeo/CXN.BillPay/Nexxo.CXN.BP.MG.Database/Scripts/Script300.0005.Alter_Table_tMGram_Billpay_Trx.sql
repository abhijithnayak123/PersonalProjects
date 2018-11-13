-- ============================================================
-- Author:		<Ratheesh PK>
-- Create date: <02/09/2014>
-- Description:	<Added additional dump columns for billpay> 
-- Rally ID:	<NA>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_BillPay_Trx' AND COLUMN_NAME = 'AccountNumberRetryCount')
BEGIN
	ALTER TABLE tMGram_BillPay_Trx 
	ADD 
	[AccountNumberRetryCount] [varchar](10) NULL,
	[SenderFirstName] [varchar](250) NULL,
	[SenderLastName] [varchar](250) NULL,
	[SenderAddress] [varchar](250) NULL,
	[SenderCity] [varchar](100) NULL,
	[SenderState] [varchar](100) NULL,
	[SenderZipCode] [varchar](100) NULL,
	[SenderCountry] [varchar](100) NULL,
	[SenderHomePhone] [varchar](100) NULL,
	[ReceiverFirstName] [varchar](250) NULL,
	[ReceiverLastName] [varchar](250) NULL,
	[ServiceOfferingID] [varchar](50) NULL,
	[BillerWebsite] [varchar](100) NULL,
	[BillerPhone] [varchar](50) NULL,
	[BillerCutoffTime] [varchar](50) NULL,
	[BillerAddress] [varchar](250) NULL,
	[BillerAddress2] [varchar](250) NULL,
	[BillerAddress3] [varchar](250) NULL,
	[BillerCity] [varchar](250) NULL,
	[BillerState] [varchar](250) NULL,
	[BillerZip] [varchar](50) NULL,
	[PrintMGICustomerServiceNumber] [bit] NULL,
	[AgentTransactionId] [varchar](100) NULL,
	[ReadyForCommit] [bit] NULL,
	[ProcessingFee] [decimal](18, 2) NULL,
	[InfoFeeIndicator] [bit] NULL,
	[ExchangeRateApplied] [decimal](18, 2) NULL
END
GO