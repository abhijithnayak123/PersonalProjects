
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartners]') AND type in (N'U'))
DROP TABLE [dbo].[tChannelPartners]
GO


CREATE TABLE [dbo].[tChannelPartners](
	[id] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[FeesFollowCustomer] [bit] NOT NULL,
	[CashFeeDescriptionEN] [nvarchar](500) NULL,
	[CashFeeDescriptionES] [nvarchar](500) NULL,
	[DebitFeeDescriptionEN] [nvarchar](500) NULL,
	[DebitFeeDescriptionES] [nvarchar](500) NULL,
	[ConvenienceFeeCash] [money] NULL,
	[ConvenienceFeeDebit] [money] NULL,
	[ConvenienceFeeDescriptionEN] [nvarchar](500) NULL,
	[ConvenienceFeeDescriptionES] [nvarchar](500) NULL,
	[CanCashCheckWOGovtId] [bit] NULL,
	[LogoFileName] [nvarchar](50) NULL,
	[IsEFSPartner] [bit] NULL,
	[EFSClientId] [int] NULL,
	[UsePINForNonGPR] [bit] NULL,
	[IsCUPartner] [bit] NULL,
	[HasNonGPRCard] [bit] NULL,
	[ManagesCash] [bit] NULL,
	[AllowPhoneNumberAuthentication] [bit] NULL,
 CONSTRAINT [PK_tChannelPartners] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChannelPartners] ADD  CONSTRAINT [DF_tChannelPartners_FeesFollowCustomer]  DEFAULT ((0)) FOR [FeesFollowCustomer]
GO