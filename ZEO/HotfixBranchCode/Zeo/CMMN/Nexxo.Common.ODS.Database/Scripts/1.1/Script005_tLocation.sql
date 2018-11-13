IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tLocation]') AND type in (N'U'))
DROP TABLE [dbo].[tLocation]
GO

/****** Object:  Table [dbo].[tLocation]    Script Date: 10/10/2013 20:21:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tLocation](
 [ClientID] [bigint] NULL,
 [LocationID] [bigint] NULL,
 [InternalLocationID] [bigint] NULL,
 [LocationName] [nvarchar](100) NULL,
 [LocationStatus] [nvarchar](20) NULL,
 [Address1] [nvarchar](100) NULL,
 [Address2] [nvarchar](100) NULL,
 [City] [nvarchar](50) NULL,
 [State] [nvarchar](2) NULL,
 [ZipCode] [nvarchar](10) NULL,
 [PhoneNumber] [nvarchar](20) NULL,
 [BankID] [varchar](40) NULL,
 [BranchID] [varchar](40) NULL,
 [TimezoneID] [varchar](100) NULL,
 [LastUpdate] [datetime] NULL,
 [Revision] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
