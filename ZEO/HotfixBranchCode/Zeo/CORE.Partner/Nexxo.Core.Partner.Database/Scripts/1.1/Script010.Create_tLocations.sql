
--CREATE TABLE [dbo].[tLocations](
--    [rowguid] [uniqueidentifier] NOT NULL,
--	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
--	[Name] [nvarchar](100) NULL,
--	[Address] [nvarchar](100) NULL,
--	[Phone] [nvarchar](100) NULL,
--	[ChannelPartnerId] [int] NOT NULL,
--    [DTCreate] [datetime] NOT NULL,
--	[DTLastMod] [datetime] NULL,
--CONSTRAINT [PK_tLocations] PRIMARY KEY CLUSTERED 
--(
--    [rowguid] ASC
--)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
--) ON [PRIMARY]
 
--GO
 

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tLocations_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tLocations] DROP CONSTRAINT [DF_tLocations_DTCreate]
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tLocations_DTLastMod]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tLocations] DROP CONSTRAINT [DF_tLocations_DTLastMod]
END
GO
/****** Object:  Table [dbo].[tLocations]    Script Date: 04/26/2013 14:38:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tLocations]') AND type in (N'U'))
DROP TABLE [dbo].[tLocations]
GO

CREATE TABLE [dbo].[tLocations](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LocationName] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Address1] [nvarchar](100) NULL,
	[Address2] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](2) NULL,
	[ZipCode] [nvarchar](10) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NOT NULL,
	[ChannelPartnerId] [smallint] NULL,
 CONSTRAINT [PK_tLocations] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tLocations] ADD  CONSTRAINT [DF_tLocations_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tLocations] ADD  CONSTRAINT [DF_tLocations_DTLastMod]  DEFAULT (getdate()) FOR [DTLastMod]
GO



