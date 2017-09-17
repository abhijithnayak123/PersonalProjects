-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_Account table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tCertegy_Account]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tCertegy_Account](
	[CertegyAccountPK] [uniqueidentifier] NOT NULL,
	[CertegyAccountId] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[SecondLastName] [nvarchar](50) NULL,
	[SSN] [nvarchar](20) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Address1] [nvarchar](100) NOT NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](2) NOT NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[IDType] [nvarchar](50) NOT NULL,
	[IdState] [char](2) NULL,
	[IDCardNumber] [nvarchar](50) NULL,	
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	CONSTRAINT [PK_tCertegy_Account] PRIMARY KEY CLUSTERED 
	(
		[CertegyAccountPK] ASC
	) WITH (FILLFACTOR = 90)
) ON [PRIMARY]
GO
