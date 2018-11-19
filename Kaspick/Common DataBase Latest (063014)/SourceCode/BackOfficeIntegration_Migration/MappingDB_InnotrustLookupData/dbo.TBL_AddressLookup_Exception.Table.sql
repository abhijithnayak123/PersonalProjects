/****** Object:  Table [dbo].[TBL_AddressLookup_Exception]    Script Date: 04/11/2014 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_AddressLookup_Exception]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_AddressLookup_Exception](
	[ContactID] [int] NULL,
	[AddressID] [int] NULL,
	[ExcelsiorAddressID] [int] NULL
) ON [PRIMARY]
END
GO
