SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_Credential_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Credential] DROP CONSTRAINT [DF_tFView_Credential_DTCreate]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_Credential_DTLastMod]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Credential] DROP CONSTRAINT [DF_tFView_Credential_DTLastMod]
END
GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[PK_tFView_Credential]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Credential] DROP CONSTRAINT [PK_tFView_Credential]
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFView_Credential]') AND type in (N'U'))
DROP TABLE [dbo].[tFView_Credential]
GO

CREATE TABLE [dbo].[tFView_Credential](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[ServiceUrl] [varchar](500) NULL,
	[User] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Application] [varchar](50) NULL,
	[TerminalID] [uniqueidentifier] NULL,
	[ProcessorID] [int] NULL,
	[CIAClientID] [varchar](10) NULL,
	[SystemExtLogin] [int] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tFView_Credential] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tFView_Credential] ADD  CONSTRAINT [DF_tFView_Credential_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tFView_Credential] ADD  CONSTRAINT [DF_tFView_Credential_DTLastMod]  DEFAULT (getdate()) FOR [DTLastMod]
GO

--This is sample data for Test
insert into tFView_Credential select NEWID(),1,'https://firstviewcorp.com/dbbApplications/CoreIssue.aspx','CentrisTestAPI','First2013!View','appScale4630','F7F17776-E886-4AE2-93BD-A339102D6720',14,64034,1,GETDATE(),GETDATE()