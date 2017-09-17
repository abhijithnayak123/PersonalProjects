/****** Object:  Table [dbo].[tFIS_Account]    Script Date: 09/26/2013 19:27:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFIS_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tFIS_Account]
GO

/****** Object:  Table [dbo].[tFIS_Account]    Script Date: 09/26/2013 19:27:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tFIS_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(2000000000,1) NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[LastName2] [nvarchar](255) NULL,
	[MothersMaidenName] [nvarchar](255) NULL,
	[DOB] [datetime] NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](255) NULL,
	[Phone1] [nvarchar](255) NULL,
	[Phone2] [nvarchar](255) NULL,
	[SSN] [nvarchar](255) NULL,
	[Gender] [nvarchar](6) NULL,
	[PartnerAccountNumber] [nvarchar](100) NULL,
	[RelationshipAccountNumber] [nvarchar](100) NULL,
	[ProfileStatus] [bit] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tFIS_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


