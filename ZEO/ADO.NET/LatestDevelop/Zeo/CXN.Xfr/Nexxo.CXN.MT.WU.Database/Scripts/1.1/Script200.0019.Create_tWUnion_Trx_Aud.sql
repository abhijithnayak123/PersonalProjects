/****** Object:  Table [dbo].[tWUnion_Trx_Aud]    Script Date: 12/10/2013 18:34:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Trx_Aud]
GO

/****** Object:  Table [dbo].[tWUnion_Trx_Aud]    Script Date: 12/10/2013 18:34:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tWUnion_Trx_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[WUnionAccountPK] [uniqueidentifier] NOT NULL,
	[WUnionRecipientAccountPK] [uniqueidentifier] NULL,
	[OriginatorsPrincipalAmount] [decimal](18, 2) NULL,
	[OriginatingCountryCode] [varchar](20) NULL,
	[OriginatingCurrencyCode] [varchar](20) NULL,
	[TranascationType] [varchar](20) NULL,
	[PromotionsCode] [varchar](50) NULL,
	[ExchangeRate] [decimal](18, 2) NULL,
	[DestinationPrincipalAmount] [decimal](18, 2) NULL,
	[GrossTotalAmount] [bigint] NULL,
	[Charges] [bigint] NULL,
	[TaxAmount] [bigint] NULL,
	[Mtcn] [varchar](50) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[PromotionDiscount] [decimal](18, 2) NULL,
	[OtherCharges] [decimal](18, 2) NULL,
	[MoneyTransferKey] [varchar](100) NULL,
	[AdditionalCharges] [decimal](18, 2) NULL,
	[DestinationCountryCode] [varchar](100) NULL,
	[DestinationCurrencyCode] [varchar](100) NULL,
	[DestinationState] [varchar](100) NULL,
	[IsDomesticTransfer] [bit] NULL,
	[IsFixedOnSend] [bit] NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Url] [varchar](100) NULL,
	[AgencyName] [varchar](200) NULL,
	[ChannelPartnerId] [bigint] NULL,
	[ProviderId] [int] NULL,
	[TestQuestion] [varchar](100) NULL,
	[TempMTCN] [varchar](100) NULL,
	[ExpectedPayoutStateCode] [varchar](100) NULL,
	[ExpectedPayoutCityName] [varchar](100) NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL,
	[RevisionNo] [bigint] NULL,
	[TestAnswer] [varchar](100) NULL,
	[TestQuestionAvaliable] [varchar](5) NULL,
	[GCNumber] [varchar](20) NULL,
	[SenderName] [varchar](50) NULL,
	[PdsRequiredFlag] [bit] NULL,
	[DfTransactionFlag] [bit] NULL,
	[DeliveryServiceName] [varchar](100) NULL,
	[DTAvailableForPickup] [datetime] NULL,
	[RecieverFirstName] [varchar](100) NULL,
	[RecieverLastName] [varchar](100) NULL,
	[RecieverSecondLastName] [varchar](100) NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


