/****** Object:  Table [dbo].[TBL_AddressLookup]    Script Date: 04/11/2014 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_AddressLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_AddressLookup](
	[ContactID] [int] NOT NULL,
	[AddressID] [int] NOT NULL,
	[ExcelsiorAddressID] [int] NOT NULL
) ON [PRIMARY]
END
GO
