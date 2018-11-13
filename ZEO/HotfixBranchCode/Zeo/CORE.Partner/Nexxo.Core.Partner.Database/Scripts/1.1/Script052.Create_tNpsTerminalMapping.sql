IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tNpsTerminalMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tNpsTerminalMapping]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tNpsTerminalMapping](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (10000000, 1),
	[TerminalPK] [uniqueidentifier] NOT NULL,
	[NpsTerminalPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	CONSTRAINT [PK_tNpsTerminalMapping] PRIMARY KEY CLUSTERED 
	(
		[rowguid] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tNpsTerminalMapping]
ADD CONSTRAINT [FK_tNpsTerminalMapping_tTerminals_TerminalPK] FOREIGN KEY(TerminalPK) REFERENCES [dbo].[tTerminals](rowguid) 
GO

ALTER TABLE [dbo].[tNpsTerminalMapping]
ADD CONSTRAINT [FK_tNpsTerminalMapping_tNpsTerminals_NpsTerminalPK] FOREIGN KEY(NpsTerminalPK) REFERENCES [dbo].[tNpsTerminals](RowGuid) 
GO

ALTER TABLE [dbo].[tNpsTerminalMapping]
ADD CONSTRAINT [IX_tNpsTerminalMapping_TerminalPK] UNIQUE NONCLUSTERED ([TerminalPK]) 
GO