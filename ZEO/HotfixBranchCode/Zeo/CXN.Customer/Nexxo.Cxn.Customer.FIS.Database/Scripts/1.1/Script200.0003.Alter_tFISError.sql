-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <01/22/2014>
-- Description:	<Adding additional columns as per the review 
--				 comments received from Neeraj>
-- Rallly ID:	<US1702>
-- ============================================================

IF NOT EXISTS(SELECT * FROM sys.columns where Name = N'DTCreate' and Object_ID = Object_ID(N'tFIS_Error'))
BEGIN
	ALTER TABLE 
		dbo.tFIS_Error 
	ADD 
		[BranchId] [varchar](20) NULL,
		[NxoEvent] [varchar](30) NOT NULL,
		[DTCreate] [datetime] NOT NULL,
		[DTLastMod] [datetime] NULL,	
		[DTServerCreate] [datetime] NULL,
		[DTServerLastMod] [datetime] NULL
END
GO

IF EXISTS(SELECT * FROM sys.columns where Name = N'ErrorMessage' and Object_ID = Object_ID(N'tFIS_Error'))
BEGIN
	ALTER TABLE dbo.tFIS_Error 
	ALTER COLUMN [ErrorMessage] [varchar](255) NOT NULL	
END
GO