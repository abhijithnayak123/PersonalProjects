--===========================================================================================
-- Author:		Pamila Jose
-- Create date: <21/05/2014>
-- Description:	<Script for creating tCCIS_Account table  >
-- Rally ID:	<US1983>
--===========================================================================================

IF NOT EXISTS
(
	SELECT 1  FROM  dbo.sysobjects 	WHERE  name = N'tCCIS_Account'
)
BEGIN

	CREATE TABLE [dbo].[tCCIS_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
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
 CONSTRAINT [PK_tCCIS_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO



