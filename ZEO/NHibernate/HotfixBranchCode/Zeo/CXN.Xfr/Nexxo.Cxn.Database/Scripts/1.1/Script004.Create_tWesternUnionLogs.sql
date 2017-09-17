
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tWesternUnionLogs_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tWesternUnionLogs] DROP CONSTRAINT [DF_tWesternUnionLogs_DTCreate]
END

GO


/****** Object:  Table [dbo].[tWesternUnionLogs]    Script Date: 05/28/2013 11:11:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWesternUnionLogs]') AND type in (N'U'))
DROP TABLE [dbo].[tWesternUnionLogs]
GO


CREATE TABLE [dbo].[tWesternUnionLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReceiverNameType] [nvarchar](50) NULL,
	[ReceiverFirstName] [nvarchar](50) NULL,
	[ReceiverLastName] [nvarchar](50) NULL,
	[ReceiverAddressAddrLine1] [nvarchar](50) NULL,
	[ReceiverAddressCity] [nvarchar](50) NULL,
	[ReceiverAddressState] [nvarchar](50) NULL,
	[ReceiverAddressPostalCode] [nvarchar](50) NULL,
	[ReceiverPreferredCustomerAccountNumber] [nvarchar](50) NULL,
	[ReceiverPreferredCustomerLevelCode] [nvarchar](50) NULL,
	[ReceiverEmail] [nvarchar](30) NULL,
	[ReceiverContactPhone] [nvarchar](20) NULL,
	[ReceiverMobilePhone] [nvarchar](20) NULL,
	[ReceiverSmsNotificationFlag] [nvarchar](50) NULL,
	[SenderNameType] [nvarchar](50) NULL,
	[SenderFirstName] [nvarchar](50) NULL,
	[SenderLastName] [nvarchar](50) NULL,
	[SenderAddressAddrLine1] [nvarchar](50) NULL,
	[SenderAddressCity] [nvarchar](50) NULL,
	[SenderAddressState] [nvarchar](50) NULL,
	[SenderAddressPostalCode] [nvarchar](50) NULL,
	[SenderPreferredCustomerAccountNumber] [nvarchar](50) NULL,
	[SenderPreferredCustomerLevelCode] [nvarchar](50) NULL,
	[SenderEmail] [nvarchar](30) NULL,
	[SenderContactPhone] [nvarchar](20) NULL,
	[SenderMobilePhone] [nvarchar](20) NULL,
	[SenderSmsNotificationFlag] [nvarchar](50) NULL,
	[OriginatorsPrincipalAmount] [bigint] NULL,
	[DestinationCountryCode] [varchar](20) NULL,
	[DestinationCurrencyCode] [varchar](20) NULL,
	[OriginatingCountryCode] [varchar](20) NULL,
	[OriginatingCurrencyCode] [varchar](20) NULL,
	[ExpectedPayoutStateCode] [varchar](20) NULL,
	[ExpectedPayoutCityCode] [varchar](20) NULL,
	[TranascationType] [varchar](20) NULL,
	[PaymentType] [varchar](20) NULL,
	[PromotionsCode] [varchar](50) NULL,
	[DTCreate] [datetime] NOT NULL,
	[ExchangeRate] [float] NULL,
	[DestinationPrincipalAmount] [bigint] NULL,
	[GrossTotalAmount] [bigint] NULL,
	[Charges] [bigint] NULL,
	[TaxAmount] [bigint] NULL,
	[Mtcn] [varchar](50) NULL,
	[MoneyTransferId] [bigint] NOT NULL,
 CONSTRAINT [PK_tWesternUnionLogs_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



ALTER TABLE [dbo].[tWesternUnionLogs] ADD  CONSTRAINT [DF_tWesternUnionLogs_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO


