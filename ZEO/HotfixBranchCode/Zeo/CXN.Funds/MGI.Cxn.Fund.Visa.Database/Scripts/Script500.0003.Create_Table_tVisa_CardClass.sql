-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <06/25/2014>
-- Description:	<DDL script to create tVisa_CardClass table>
-- Rally ID:	<AL-327>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tVisa_CardClass]') AND type in (N'U'))
DROP TABLE [dbo].[tVisa_CardClass]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tVisa_CardClass]
(
	[VisaCardClassPK] [uniqueidentifier] NOT NULL,
	[VisaCardClassId] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[StateCode] VARCHAR(5) NOT NULL ,
	[CardClass] INT  NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastMod] [datetime] NULL,
	CONSTRAINT IX_tVisa_CardClass_StateCode UNIQUE NONCLUSTERED(StateCode)
)
GO
