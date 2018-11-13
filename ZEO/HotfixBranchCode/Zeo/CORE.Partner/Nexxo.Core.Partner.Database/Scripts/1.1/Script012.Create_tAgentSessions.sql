

CREATE TABLE [dbo].[tAgentSessions](
    [rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
	[AgentId] [nvarchar](50) NOT NULL,
	[TerminalPK] [uniqueidentifier] NOT NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tAgentSessions] PRIMARY KEY CLUSTERED 
(
    [rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tAgentSessions]  WITH CHECK ADD CONSTRAINT [FK_tAgentSessions_tTerminals] FOREIGN KEY([TerminalPK])
REFERENCES [dbo].[tTerminals] ([rowguid])
GO
 
ALTER TABLE [dbo].[tAgentSessions] CHECK CONSTRAINT [FK_tAgentSessions_tTerminals]
GO
