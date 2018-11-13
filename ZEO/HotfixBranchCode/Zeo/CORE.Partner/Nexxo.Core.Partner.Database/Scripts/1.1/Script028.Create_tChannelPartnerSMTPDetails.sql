
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartnerSMTPDetails]') AND type in (N'U'))
DROP TABLE [dbo].[tChannelPartnerSMTPDetails]
GO

CREATE TABLE [dbo].[tChannelPartnerSMTPDetails](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[ChannelPartnerId] [int] NOT NULL,
	[SmtpHost] [nvarchar](50) NULL,
	[SmtpPort] [int] NULL,
	[SenderEmail] [nvarchar](50) NULL,
	[SenderPwd] [nvarchar](50) NULL,
	[Subject] [nvarchar](255) NULL,
	[Body] [nvarchar](1000) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChannelPartnerSMTPDetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
