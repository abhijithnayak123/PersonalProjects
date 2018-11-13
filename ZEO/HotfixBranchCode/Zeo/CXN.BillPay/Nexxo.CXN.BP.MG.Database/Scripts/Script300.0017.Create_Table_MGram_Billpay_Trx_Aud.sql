-- ============================================================
-- Author:		<Ratheesh PK>
-- Create date: <09/29/2014>
-- Description:	<CREATE Table for MoneyGram Billpay Transaction Audit Table> 
-- Rally ID:	<NA>
-- ============================================================


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMGram_Billpay_Trx_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tMGram_Billpay_Trx_Aud]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [tMGram_BillPay_Trx_Aud](
	[rowguid] [uniqueidentifier] NULL,
	[Id] [bigint] NULL,
	[AgentID] [varchar](50) NULL,
	[AgentSequence] [varchar](50) NULL,
	[Token] [varchar](50) NULL,
	[ApiVersion] [varchar](50) NULL,
	[ClientSoftwareVersion] [varchar](50) NULL,
	[RequestResponseType] [tinyint] NULL,
	[ProductVariant] [tinyint] NULL,
	[ReceiveCountry] [varchar](50) NULL,
	[ReceiveCode] [varchar](50) NULL,
	[ReceiveAgentID] [varchar](50) NULL,
	[ReceiveCurrency] [varchar](50) NULL,
	[SendCurrency] [varchar](50) NULL,
	[PromoCodeValuesPromoCode] [varchar](50) NULL,
	[DoCheckIn] [bit] NULL,
	[TimeStamp] [datetime] NULL,
	[Flags] [int] NULL,
	[ValidReceiveAmount] [decimal](18, 2) NULL,
	[ValidReceiveCurrency] [varchar](20) NULL,
	[ValidExchangeRate] [decimal](18, 2) NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[ReceiveAmountAltered] [bit] NULL,
	[RevisedInformationalFee] [bit] NULL,
	[DeliveryOptId] [varchar](20) NULL,
	[DeliveryOptDisplayName] [varchar](50) NULL,
	[ReceiveAgentName] [varchar](100) NULL,
	[MgiTransactionSessionID] [varchar](50) NULL,
	[SendAmountAltered] [bit] NULL,
	[SendAmount] [decimal](18, 2) NULL,
	[TotalSendFees] [decimal](18, 2) NULL,
	[TotalDiscountAmount] [decimal](18, 2) NULL,
	[TotalSendTaxes] [decimal](18, 2) NULL,
	[TotalAmountToCollect] [decimal](18, 2) NULL,
	[ReceiveAmount] [decimal](18, 2) NULL,
	[ValidCurrencyIndicator] [bit] NULL,
	[PayoutCurrency] [varchar](50) NULL,
	[TotalReceiveFees] [decimal](18, 2) NULL,
	[TotalReceiveTaxes] [decimal](18, 2) NULL,
	[TotalReceiveAmount] [decimal](18, 2) NULL,
	[ReceiveFeesAreEstimated] [bit] NULL,
	[ReceiveTaxesAreEstimated] [bit] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastMod] [datetime] NULL,
	[AccountPK] [uniqueidentifier] NULL,
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
	[ExchangeRateApplied] [decimal](18, 2) NULL,
	[ReferenceNumber] [varchar](50) NULL,
	[PartnerConfirmationNumber] [varchar](100) NULL,
	[PartnerName] [varchar](250) NULL,
	[FreePhoneCallPin] [varchar](250) NULL,
	[TollFreePhoneNumber] [varchar](250) NULL,
	[ExpectedDateOfDelivery] [datetime] NULL,
	[TransactionDateTime] [datetime] NULL,
	[AccountNumber] [varchar](100) NULL,
	[SenderMiddleName] [varchar](250) NULL,
	[SenderLastName2] [varchar](250) NULL,
	[MessageField1] [varchar](50) NULL,
	[MessageField2] [varchar](50) NULL,
	[SenderDOB] [datetime] NULL,
	[SenderOccupation] [varchar](30) NULL,
	[SenderLegalIdNumber] [varchar](14) NULL,
	[SenderLegalIdType] [varchar](5) NULL,
	[SenderPhotoIdCountry] [varchar](3) NULL,
	[SenderPhotoIdState] [varchar](2) NULL,
	[SenderPhotoIdNumber] [varchar](20) NULL,
	[SenderPhotoIdType] [varchar](3) NULL,
	[BillerName] [varchar](65) NULL,
	[TextTranslation] [varchar](max) NULL,
	[ReceiverMiddleName] [varchar](40) NULL,
	[ReceiverLastName2] [varchar](40) NULL,
	[PurposeOfFund] [varchar](6) NULL,
	[TotalSendAmount] [decimal](18, 2) NULL,
	[MgiRewardsNumber] [varchar](20) NULL,
	[ValidateAccountNumber] [varchar](30) NULL,
	[CardExpirationMonth] [varchar](2) NULL,
	[CardExpirationYear] [varchar](4) NULL,	
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL,
	[RevisionNo] [bigint] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

 