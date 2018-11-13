-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <11/11/2014>
-- Description:	<DDL script to create tVisa_Account table>
-- Rally ID:	<US2154>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tVisa_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tVisa_Account]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tVisa_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ProxyId] [varchar](25) NULL,
	[PseudoDDA] [varchar](50) NULL,
	[CardNumber] [varchar](50) NULL,
	[CardAliasId] [varchar](50) NULL,
	[FirstName] [varchar](50) NOT NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[SSN] [varchar](50) NULL,
	[Phone] [varchar](20) NULL,
	[PhoneType] [varchar](20) NULL,
	[Address1] [varchar](100) NULL,
	[Address2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](20) NULL,
	[Country] [varchar](50) NULL,
	[Activated] [bit] NOT NULL,
	[FraudScore] [int] NULL,
	[ExpirationMonth] [int] NULL,
	[ExpirationYear] [int] NULL,
	[SubClientNodeId] BIGINT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	CONSTRAINT [PK_tVisa_Account] PRIMARY KEY CLUSTERED 
	(
		[rowguid] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
