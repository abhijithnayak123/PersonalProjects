
/****** Object:  Table [dbo].[tJobs]    Script Date: 08/19/2013 18:25:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tJobs](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](20) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
	[Status] [char](20) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

