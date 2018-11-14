-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_Credential table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_Credential]') AND type in (N'U'))
DROP TABLE [dbo].[tCertegy_Credential]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tCertegy_Credential](
	[CertegyCredentialPK] [uniqueidentifier] NOT NULL,
	[CertegyCredentialId] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ServiceUrl] [varchar](500) NOT NULL,
	[CertificateName] [varchar](255) NOT NULL,
	[Version] [varchar](6) NOT NULL,
	[ChannelPartnerId] [bigint] NOT NULL,
	[DeviceType] [char] (1) NOT NULL,	
	[DeviceIP] [nvarchar] (15) NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
 CONSTRAINT [PK_tCertegy_Credential] PRIMARY KEY CLUSTERED 
(
	[CertegyCredentialPK] ASC
)WITH (FILLFACTOR = 90) ON [PRIMARY],
 CONSTRAINT [IX_tCertegy_Credential_ChannelPartnerId] UNIQUE NONCLUSTERED 
(
	[ChannelPartnerId] ASC
)WITH (FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


