IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Credential]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Credential]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tWUnion_Credential](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[WUServiceUrl] [nvarchar](max) NULL,
	[WUClientCertificateSubjectName] [nvarchar](200) NULL,
	[AccountIdentifier] [nvarchar](200) NULL,
	[CounterId] [nvarchar](100) NULL,
	[ChannelName] [nvarchar](100) NULL,
	[ChannelVersion] [nvarchar](100) NULL,
	[ChannelPartnerId] [bigint] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWU_Credential] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Credential]') AND type in (N'U'))
	INSERT INTO tWUnion_Credential (rowguid,WUServiceUrl,WUClientCertificateSubjectName,AccountIdentifier,CounterId,ChannelName,ChannelVersion,DTCreate,ChannelPartnerId)
		values (NEWID(),'https://wugateway2pi.westernunion.net','westernunionpartnerintegration','WGHH614900T','6149PT00001A','ESP','9500',GETDATE(),33)
GO


