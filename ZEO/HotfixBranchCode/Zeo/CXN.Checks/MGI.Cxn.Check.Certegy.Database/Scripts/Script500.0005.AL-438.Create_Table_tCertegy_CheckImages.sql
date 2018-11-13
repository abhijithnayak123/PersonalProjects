-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_CheckImages table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_CheckImages]') AND type in (N'U'))
DROP TABLE [dbo].[tCertegy_CheckImages]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tCertegy_CheckImages](
	[CertegyCheckImagePK] [uniqueidentifier] NOT NULL,
	[Front] [varbinary](max) NOT NULL,
	[Back] [varbinary](max) NOT NULL,
	[Format] [nvarchar](10) NOT NULL,
	[FrontTIF] [varbinary](max) NOT NULL,
	[BackTIF] [varbinary](max) NOT NULL,
	[CertegyTrxPK] [uniqueidentifier] NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
 CONSTRAINT [PK_tCertegy_CheckImages] PRIMARY KEY CLUSTERED 
(
	[CertegyCheckImagePK] ASC
)WITH (FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tCertegy_CheckImages]  WITH CHECK ADD  CONSTRAINT [FK_tCertegy_CheckImages_tCertegy_Trx] FOREIGN KEY([CertegyTrxPK])
REFERENCES [dbo].[tCertegy_Trx] ([CertegyTrxPK])
GO

ALTER TABLE [dbo].[tCertegy_CheckImages] CHECK CONSTRAINT [FK_tCertegy_CheckImages_tCertegy_Trx]
GO


