﻿IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REVINFO]') AND type in (N'U'))
CREATE TABLE [dbo].[REVINFO](
	[REV] [int] IDENTITY(1,1) NOT NULL,
	[REVTSTMP] [datetime] NULL,
 CONSTRAINT [PK_REVINFO] PRIMARY KEY CLUSTERED 
(
	[REV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
