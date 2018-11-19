/****** Object:  Table [dbo].[TBL_AccountLookup]    Script Date: 04/11/2014 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_AccountLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_AccountLookup](
	[ManagerCode] [char](4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ClientID] [int] NOT NULL,
	[AllianceNumber] [char](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ProgramID] [int] NOT NULL,
	[CustomerAccountNumber] [char](14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AccountID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
