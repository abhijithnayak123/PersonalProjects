/****** Object:  Table [dbo].[tApplicationLog]    Script Date: 05/14/2013 17:30:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tApplicationLog') and TYPE in ('U'))
BEGIN

CREATE TABLE [dbo].[tApplicationLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Source] [varchar](50) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[StackTrace] [varchar](max) NULL,
	[LoggedBy] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_ApplicationLog_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


SET ANSI_PADDING OFF
END


