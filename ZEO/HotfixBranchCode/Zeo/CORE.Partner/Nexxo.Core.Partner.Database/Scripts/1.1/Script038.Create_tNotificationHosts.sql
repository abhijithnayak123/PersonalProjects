
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tNotificationHosts]') AND type in (N'U'))
DROP TABLE [dbo].[tNotificationHosts]
GO

CREATE TABLE [dbo].[tNotificationHosts](
	[id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MailGateway] [nvarchar](200) NOT NULL,
	[MaxMessageLen] [int] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tNotificationHosts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO