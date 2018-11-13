
/****** Object:  Table [dbo].[tWesternUnionReceiverProfiles]    Script Date: 05/14/2013 17:17:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tWesternUnionReceiverProfiles') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tWesternUnionReceiverProfiles](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[XRecipientProfilesId] [uniqueidentifier] NOT NULL,
	[PrimaryPhoneNumber] [nvarchar](20) NULL,
	[PhoneType] [varchar](50) NULL,
	[MobileProvider] [varchar](50) NULL,
	[PaymentMethodId] [int] NULL,
	[AmountType] [nvarchar](20) NULL,
	[PaymentType] [nvarchar](20) NULL,
	[DeliveryMethodId] [varchar](50) NULL,
	[DeliveryOptionId] [int] NULL,
	[DestinationCurrencyCode] [nvarchar](20) NULL,
	[DestinationCountryCode] [nvarchar](20) NULL,
	[DestinationStateCode] [nvarchar](20) NULL,
	[DestinationCity] [nvarchar](200) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [dbo].[tWesternUnionReceiverProfiles] ADD  CONSTRAINT [DF_tWesternUnionReceiverProfiles_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
END