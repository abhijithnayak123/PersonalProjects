

CREATE TABLE [dbo].[tBridgerFailures](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY(1000000000,1),
	[CustomerId] [bigint] NOT NULL,
	[ServerName] [nvarchar](20) NULL,
	[ExitCode] [int] NOT NULL,
	[DTAlertSent] [datetime] NULL,
	[Cleared] [bit] NULL,
	[DTCleared] [datetime] NULL,
	[ClearedBy] [nvarchar](20) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tBridgerFailures] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

