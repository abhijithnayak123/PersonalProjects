
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Table_1_FailureCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tAgentAuthentication] DROP CONSTRAINT [DF_Table_1_FailureCount]
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tAgentAuthentication]') AND type in (N'U'))
DROP TABLE [dbo].[tAgentAuthentication]
GO

CREATE TABLE [dbo].[tAgentAuthentication](
	[AgentId] [int] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](100) NULL,
	[Salt] [nvarchar](100) NULL,
	[AuthenticationFailures] [int] NOT NULL,
	[TemporaryPassword] [bit] NOT NULL,
	[DTLastPasswordUpdate] [datetime] NULL,
	[LastPasswordUpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_tAgentAuthentication] PRIMARY KEY CLUSTERED 
(
	[AgentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tAgentAuthentication] ADD  CONSTRAINT [DF_Table_1_FailureCount]  DEFAULT ((0)) FOR [AuthenticationFailures]
GO