IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tNpsTerminals]') AND type in (N'U'))
	DROP TABLE [dbo].[tNpsTerminals]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tNpsTerminals](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (10000000, 1),
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[IPAddress] [varchar](20) NOT NULL,
	[Port] [varchar](10) NOT NULL,
	[LocationPK] [uniqueidentifier] NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	 CONSTRAINT [PK_tNpsTerminals] PRIMARY KEY CLUSTERED 
	(
		[rowguid] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tNpsTerminals]
ADD CONSTRAINT [DF_tNpsTerminals_Port] DEFAULT 8732 FOR Port
GO

ALTER TABLE [dbo].[tNpsTerminals]
ADD CONSTRAINT [FK_tNpsTerminals_tLocations_RowGuid] FOREIGN KEY(LocationPK) REFERENCES [dbo].[tLocations](RowGuid) 
GO

ALTER TABLE [dbo].[tNpsTerminals] 
ADD CONSTRAINT [IX_tNpsTerminals_IPAddress] UNIQUE NONCLUSTERED ([IPAddress]) 
GO

