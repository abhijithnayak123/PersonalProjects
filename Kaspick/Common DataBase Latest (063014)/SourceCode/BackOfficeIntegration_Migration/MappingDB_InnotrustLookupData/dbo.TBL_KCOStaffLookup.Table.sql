/****** Object:  Table [dbo].[TBL_KCOStaffLookup]    Script Date: 04/11/2014 11:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_KCOStaffLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_KCOStaffLookup](
	[ContactID] [int] NOT NULL,
	[UserID] [int] NOT NULL
) ON [PRIMARY]
END
GO
