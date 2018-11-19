/****** Object:  Table [dbo].[TBL_TrusteeLookup_Exception]    Script Date: 04/11/2014 11:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_TrusteeLookup_Exception]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_TrusteeLookup_Exception](
	[CustomerAccountNumber] [char](14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContactID] [int] NULL,
	[RoleCode] [int] NULL,
	[TrusteeId] [int] NULL,
	[PartyID] [int] NULL,
	[ClientBriefName] [varchar](6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
