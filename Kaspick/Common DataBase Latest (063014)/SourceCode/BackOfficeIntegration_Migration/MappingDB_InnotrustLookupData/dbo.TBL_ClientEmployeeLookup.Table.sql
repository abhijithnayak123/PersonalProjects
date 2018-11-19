/****** Object:  Table [dbo].[TBL_ClientEmployeeLookup]    Script Date: 04/11/2014 11:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_ClientEmployeeLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_ClientEmployeeLookup](
	[SubContactId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
	[RoleCode] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[ClientID] [int] NOT NULL
) ON [PRIMARY]
END
GO
