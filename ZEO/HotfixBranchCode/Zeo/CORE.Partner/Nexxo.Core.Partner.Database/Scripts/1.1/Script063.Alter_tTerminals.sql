IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tAgentSessions_tTerminals]') AND parent_object_id = OBJECT_ID(N'[dbo].[tAgentSessions]'))
ALTER TABLE [dbo].[tAgentSessions] DROP CONSTRAINT [FK_tAgentSessions_tTerminals]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tNpsTerminalMapping_tTerminals_TerminalPK]') AND parent_object_id = OBJECT_ID(N'[dbo].[tNpsTerminalMapping]'))
ALTER TABLE [dbo].[tNpsTerminalMapping] DROP CONSTRAINT [FK_tNpsTerminalMapping_tTerminals_TerminalPK]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTerminals_tLocations]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTerminals]'))
ALTER TABLE [dbo].[tTerminals] DROP CONSTRAINT [FK_tTerminals_tLocations]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTerminals]') AND type in (N'U'))
DROP TABLE [dbo].[tTerminals]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tTerminals](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[MacAddress] [varchar](50) NULL,
	[IpAddress] [varchar](20) NOT NULL,
	[LocationPK] [uniqueidentifier] NOT NULL,
	[NpsTerminalPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	 CONSTRAINT [PK_tTerminals] PRIMARY KEY CLUSTERED 
	(
		[rowguid] ASC
	),
	 CONSTRAINT [IX_tTerminals_Name] UNIQUE NONCLUSTERED 
	(
		[Name] ASC
	)
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tTerminals] 
ADD CONSTRAINT [FK_tTerminals_tLocations] FOREIGN KEY([LocationPK]) REFERENCES [dbo].[tLocations] ([rowguid])
GO

ALTER TABLE [dbo].[tTerminals] 
ADD CONSTRAINT [FK_tTerminals_tNpsTerminals] FOREIGN KEY([NpsTerminalPK]) REFERENCES [dbo].[tNpsTerminals] ([rowguid])
GO
