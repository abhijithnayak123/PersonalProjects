/****** Object:  Table [dbo].[tWUnion_ErrorMessages]    Script Date: 10/10/2013 17:26:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_ErrorMessages]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_ErrorMessages]
GO

/****** Object:  Table [dbo].[tWUnion_ErrorMessages]    Script Date: 10/10/2013 17:26:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tWUnion_ErrorMessages](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ErrorCode] [nvarchar](100) NULL,
	[ErrorDesc] [nvarchar](max) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_ErrorMessages] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


