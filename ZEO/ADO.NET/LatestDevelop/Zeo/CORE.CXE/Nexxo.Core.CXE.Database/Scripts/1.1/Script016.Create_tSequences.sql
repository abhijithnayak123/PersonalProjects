/****** Object:  Table [dbo].[tSequences]    Script Date: 05/03/2013 10:10:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tSequences](
	[seqName] [nvarchar](25) NOT NULL,
	[seqNum] [bigint] NOT NULL
) ON [PRIMARY]

GO

INSERT tSequences(seqName,seqNum) values('PAN',0)
