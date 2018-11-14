-- ============================================================
-- Author:		<Ratheesh PK>
-- Create date: <08/08/2014>
-- Description:	<Table for MoneyGram BillPay Transaction> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tMGram_BillPay_Trx_tMGram_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tMGram_BillPay_Trx]'))
ALTER TABLE [dbo].[tMGram_BillPay_Trx] DROP CONSTRAINT [FK_tMGram_BillPay_Trx_tMGram_Account]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMGram_BillPay_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tMGram_BillPay_Trx]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tMGram_BillPay_Trx](
		[rowguid] [uniqueidentifier] NOT NULL,
	    [Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
		[AgentID] VARCHAR(50) NULL,
	    [AgentSequence]  VARCHAR(50) NULL,
	    [Token]  VARCHAR(50) NULL,
	    [ApiVersion]  VARCHAR(50) NULL,
	    [ClientSoftwareVersion]  VARCHAR(50) NULL,
		[RequestResponseType] [tinyint] NULL,
		[ProductVariant] [tinyint] NULL,

	    [ReceiveCountry]  VARCHAR(50) NULL,	
		[ReceiveCode]  VARCHAR(50) NULL,	
		[ReceiveAgentID]  VARCHAR(50) NULL,	
		[ReceiveCurrency]  VARCHAR(50) NULL,	
		[SendCurrency]  VARCHAR(50) NULL,		
		[PromoCodeValuesPromoCode] VARCHAR(50) NULL,

	    [DoCheckIn] bit NULL,
	    [TimeStamp] datetime NULL,	    
	    [Flags]  int NULL,

		[ValidReceiveAmount] decimal(18,2) NULL,
	    [ValidReceiveCurrency]  VARCHAR(20) NULL,
		[ValidExchangeRate] decimal(18,2) NULL,

		[TotalAmount] decimal(18,2) NULL,
	    [ReceiveAmountAltered]  bit NULL,
	    [RevisedInformationalFee]  bit NULL,
	    [DeliveryOptId]  VARCHAR(20) NULL,
	    [DeliveryOptDisplayName]  VARCHAR(50) NULL,
		[ReceiveAgentName]  VARCHAR(100) NULL,
		[MgiTransactionSessionID]  VARCHAR(50) NULL,
		[SendAmountAltered]  bit NULL,

		[SendAmount] decimal(18,2) NULL,
		[TotalSendFees] decimal(18,2) NULL,
		[TotalDiscountAmount] decimal(18,2) NULL,
		[TotalSendTaxes] decimal(18,2) NULL,
		[TotalAmountToCollect] decimal(18,2) NULL,
		
		[ReceiveAmount] decimal(18,2) NULL,		
		[ValidCurrencyIndicator]  bit NULL,
		[PayoutCurrency] VARCHAR(50) NULL,
		[TotalReceiveFees] decimal(18,2) NULL,	
		[TotalReceiveTaxes] decimal(18,2) NULL,	
		[TotalReceiveAmount] decimal(18,2) NULL,	
		[ReceiveFeesAreEstimated] bit NULL,	
		[ReceiveTaxesAreEstimated] bit NULL,	

		[DTCreate] datetime NOT NULL,
	    [DTLastMod] datetime NULL,
		[DTServerCreate] datetime NOT NULL,
		[DTServerLastMod] datetime NULL,
 CONSTRAINT [PK_tMGram_BillPay_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

