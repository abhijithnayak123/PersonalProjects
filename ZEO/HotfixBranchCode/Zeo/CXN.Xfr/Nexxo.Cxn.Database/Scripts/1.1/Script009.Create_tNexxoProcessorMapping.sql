/****** Object:  Table [dbo].[tNexxoProcessorMapping]    Script Date: 05/14/2013 17:33:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tNexxoProcessorMapping') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tNexxoProcessorMapping](
	[ID] [uniqueidentifier] NOT NULL,
	[NexxoAccountNo] [bigint] NULL,
	[NexxoCardNumber] [bigint] NOT NULL,
	[ProcessorAccountNo] [bigint] NULL,
	[DTCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_tNexxoProcessorMapping] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[tNexxoProcessorMapping] ADD  CONSTRAINT [DF_tNexxoProcessorMapping_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]

END


