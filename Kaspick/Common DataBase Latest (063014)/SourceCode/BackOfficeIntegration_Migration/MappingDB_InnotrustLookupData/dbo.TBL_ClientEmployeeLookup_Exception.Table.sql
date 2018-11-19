/****** Object:  Table [dbo].[TBL_ClientEmployeeLookup_Exception]    Script Date: 04/11/2014 11:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_ClientEmployeeLookup_Exception]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_ClientEmployeeLookup_Exception](
	[SubContactID] [int] NULL,
	[ContactID] [int] NULL,
	[RoleCode] [int] NULL,
	[EmployeeID] [int] NULL,
	[ClientID] [int] NULL
) ON [PRIMARY]
END
GO
