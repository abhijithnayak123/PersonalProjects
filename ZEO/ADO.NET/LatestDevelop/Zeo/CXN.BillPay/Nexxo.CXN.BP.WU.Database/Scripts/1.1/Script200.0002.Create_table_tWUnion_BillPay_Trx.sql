IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_BillPay_Trx]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tWUnion_BillPay_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[ChannelParterId] [int] NOT NULL,
	[ProviderId] [int] NOT NULL,
	[Channel_Type] [varchar](10) NOT NULL,
	[Channel_Name] [varchar](20) NOT NULL,
	[Channel_Version] [varchar](10) NOT NULL,
	[Sender_FirstName] [varchar](50) NOT NULL,
	[Sender_Lastname] [varchar](50) NOT NULL,
	[Sender_AddressLine1] [varchar](50) NOT NULL,
	[Sender_City] [varchar](50) NOT NULL,
	[Sender_State] [varchar](50) NOT NULL,
	[Sender_PostalCode] [varchar](50) NULL,
	[Sender_CountryCode] [varchar](50) NULL,
	[Sender_CurrencyCode] [varchar](50) NULL,
	[Sender_AddressLine2] [varchar](50) NULL,
	[Sender_Street] [varchar](50) NULL,
	[WesternUnionCardNumber] [varchar](15) NOT NULL,
	[LevelCode] [varchar](50) NULL,
	[Sender_Email] [varchar](50) NULL,
	[Sender_ContactPhone] [varchar](50) NULL,
	[Sender_DateOfBirth] [varchar](50) NOT NULL,
	[BillerName] [varchar](50) NOT NULL,
	[Biller_CityCode] [varchar](50) NULL,
	[Customer_AccountNumber] [varchar](50) NOT NULL,
	[CountryCode] [varchar](50) NULL,
	[CurrencyCode] [varchar](50) NULL,
	[Financials_MunicipalTax] [varchar](50) NULL,
	[Financials_StateTax] [varchar](50) NULL,
	[Financials_CountTax] [varchar](50) NULL,
	[Financials_OriginatorsPrincipalAmount] [bigint] NOT NULL,
	[Financials_DestinationPrincipalAmount] [bigint] NULL,
	[Financials_Fee] [bigint] NOT NULL,
	[Financials_GrossTotalAmount] [bigint] NOT NULL,
	[Financials_Total] [bigint] NULL,
	[Financials_UndiscountedCharges] [bigint] NULL,
	[Financials_DiscountedCharges] [bigint] NULL,
	[PaymentDetails_Recording_CountryCode] [varchar](50) NULL,
	[PaymentDetails_Recording_CountryCurrency] [varchar](50) NULL,
	[PaymentDetails_Destination_CountryCode] [varchar](50) NULL,
	[PaymentDetails_Destination_CountryCurrency] [varchar](50) NULL,
	[PaymentDetails_Originating_CountryCode] [varchar](50) NULL,
	[PaymentDetails_Originating_CountryCurrency] [varchar](50) NULL,
	[PaymentDetails_Originating_City] [varchar](50) NULL,
	[PaymentDetails_Originating_State] [varchar](50) NULL,
	[PaymentDetails_TransactionType] [varchar](50) NULL,
	[PaymentDetails_PaymentType] [varchar](50) NULL,
	[PaymentDetails_ExchangeRate] [decimal] NULL,
	[PaymentDetails_FixOnSend] [varchar](50) NULL,
	[PaymentDetails_ReceiptOptOut] [varchar](50) NULL,
	[PaymentDetails_AuthStatus] [varchar](50) NULL,
	[PromotionMessage] [varchar](50) NULL,
	[Promotion_DiscountAmount] [varchar](50) NULL,
	[Promotion_Error] [varchar](50) NULL,
	[Promotions_SenderCode] [varchar](50) NULL,
	[FillingDate] [varchar](50) NULL,
	[FillingTime] [varchar](50) NULL,
	[MTCN] [varchar](50) NULL,
	[NewMTCN] [varchar](50) NULL,
	[DfFields_PDSRequiredFlag] [varchar](50) NULL,
	[DfFields_TransactionFlag] [varchar](50) NULL,
	[DfFields_DeliveryServiceName] [varchar](50) NULL,
	[DeliveryCode] [varchar](50) NULL,
	[FusionScreen] [varchar](50) NULL,
	[ConvSessionCookie] [varchar](200) NULL,
	[ForeignRemoteSystem_Identifier] [varchar](20) NULL,
	[ForeignRemoteSystem_Reference_no] [varchar](50) NULL,
	[ForeignRemoteSystem_CounterId] [varchar](20) NULL,
	[InstantNotification_AddlServiceCharges] [varchar](200) NULL,
	[InstantNotification_AddlServiceLength] [varchar](50) NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_tWUnion_BillPay_Trx] PRIMARY KEY CLUSTERED 
	(
	[rowguid] ASC
	),
	CONSTRAINT [FK_tWUnion_BillPay_Trx_tWUnion_BillPay_AccountPK] 
	FOREIGN KEY ([AccountPK]) REFERENCES [tWUnion_BillPay_Account](rowguid)
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


