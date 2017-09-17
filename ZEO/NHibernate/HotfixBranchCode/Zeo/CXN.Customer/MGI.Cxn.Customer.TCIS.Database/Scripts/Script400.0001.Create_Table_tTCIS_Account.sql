-- ============================================================
-- Author:		<Lokesh M N>
-- Create date: <11/04/2014>
-- Description:	<script for creating tTCIS_Account table.>
-- Rally ID:	<US2099>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTCIS_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tTCIS_Account]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTCIS_Account](
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
	[BankId] [nvarchar](40) NULL,
	[BranchId] [nvarchar](40) NULL,
 CONSTRAINT [PK_tTCIS_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


