
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] DROP CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account]
GO


/****** Object:  Table [dbo].[tWUnion_Trx]    Script Date: 10/28/2013 19:35:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Trx]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tWUnion_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
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
	[AgencyName] [varchar](20) NULL,
	[ChannelPartnerId] [bigint] NULL,
	[ProviderId] [int] NULL,
	[TestQuestion] [varchar](100) NULL,
	[TempMTCN] [varchar](100) NULL,
	[ExpectedPayoutStateCode] [varchar](100) NULL,
	[ExpectedPayoutCityName] [varchar](100) NULL,
	[TestAnswer] [varchar](100) NULL,
	[TestQuestionAvaliable] [varchar](5) NULL,
 CONSTRAINT [PK_tWUnion_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account] FOREIGN KEY([WUnionAccountPK])
REFERENCES [dbo].[tWUnion_Account] ([rowguid])
GO

ALTER TABLE [dbo].[tWUnion_Trx] CHECK CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account]