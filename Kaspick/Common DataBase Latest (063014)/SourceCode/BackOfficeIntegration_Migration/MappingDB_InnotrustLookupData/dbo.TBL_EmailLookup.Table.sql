/****** Object:  Table [dbo].[TBL_EmailLookup]    Script Date: 04/11/2014 11:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_EmailLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_EmailLookup](
	[ContactID] [int] NOT NULL,
	[AddressID] [int] NOT NULL,
	[ExcelsiorEmailID] [int] NOT NULL
) ON [PRIMARY]
END
GO
