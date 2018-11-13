-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <01/21/2014>
-- Description:	<Created new table to log FIS miscellaneous account creation errors>
-- Rallly ID:	<US1702>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFIS_Error]') AND type in (N'U'))
	DROP TABLE [dbo].[tFIS_Error]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tFIS_Error](
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ErrorNumber] [varchar](30) NOT NULL,
	[ErrorMessage] [varchar](30) NOT NULL,
	[BankID] [varchar](30) NOT NULL,
	[NexxoCustomerId] [varchar](50) NOT NULL,
	[AppID] [varchar](10) NULL,
	[FISRelationshipIndicator] [varchar](10) NULL,
	[FISAddressLineCode1] [varchar](10) NULL,
	[FISAddressLineCode2] [varchar](10) NULL,
	[FISAddressLineCode3] [varchar](10) NULL,
	[FISCurrentNameAddressLine1] [varchar](100) NULL,
	[FISCurrentNameAddressLine2] [varchar](100) NULL,
	[FISCurrentNameAddressLine3] [varchar](100) NULL,
	[FISAccountNumber] [varchar](50) NULL,
	[FISAcountType] [varchar](10) NULL,
	CONSTRAINT [PK_tFIS_Error_Id] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO
