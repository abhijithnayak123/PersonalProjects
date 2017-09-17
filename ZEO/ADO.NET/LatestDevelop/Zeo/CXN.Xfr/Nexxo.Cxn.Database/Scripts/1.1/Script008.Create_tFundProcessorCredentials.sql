/****** Object:  Table [dbo].[tFundProcessorCredentials]    Script Date: 05/14/2013 17:32:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tFundProcessorCredentials') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tFundProcessorCredentials](
	[Id] [uniqueidentifier] NOT NULL,
	[ServiceUrl] [varchar](500) NULL,
	[User] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Application] [varchar](50) NULL,
	[TerminalID] [varchar](20) NULL,
	[ProcessorID] [int] NULL,
	[CIAClientID] [varchar](10) NULL,
	[SystemExtLogin] [int] NULL,
	[DTCreate] [datetime] NULL,
 CONSTRAINT [PK_tFundProcessorCredentials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

END

