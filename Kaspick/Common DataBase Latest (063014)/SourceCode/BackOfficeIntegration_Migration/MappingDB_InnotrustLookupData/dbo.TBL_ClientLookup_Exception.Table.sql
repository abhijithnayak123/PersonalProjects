/****** Object:  Table [dbo].[TBL_ClientLookup_Exception]    Script Date: 04/11/2014 11:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_ClientLookup_Exception]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_ClientLookup_Exception](
	[AccountManagerCode] [char](4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContactId] [int] NULL,
	[ClientId] [int] NULL,
	[PartyID] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
