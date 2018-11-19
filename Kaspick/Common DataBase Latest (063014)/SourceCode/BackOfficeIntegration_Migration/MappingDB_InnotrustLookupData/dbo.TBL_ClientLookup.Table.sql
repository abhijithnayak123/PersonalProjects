/****** Object:  Table [dbo].[TBL_ClientLookup]    Script Date: 04/11/2014 11:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_ClientLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_ClientLookup](
	[AccountManagerCode] [char](4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContactId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[PartyID] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
