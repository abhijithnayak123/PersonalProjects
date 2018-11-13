
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tAgentLocationMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tAgentLocationMapping]
GO

CREATE TABLE [dbo].[tAgentLocationMapping](
	[AgentId] [int] NOT NULL,
	[LocationId] [bigint] NOT NULL,
 CONSTRAINT [PK_tAgentLocationMapping] PRIMARY KEY CLUSTERED 
(
	[AgentId] ASC,
	[LocationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


