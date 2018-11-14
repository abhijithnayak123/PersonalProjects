-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<Table to hold supported DoddFrank state regulators by MoneyGram> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.tMGram_StateRegulators') AND type in (N'U'))
DROP TABLE dbo.tMGram_StateRegulators
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO	

CREATE TABLE dbo.tMGram_StateRegulators(
	rowguid UNIQUEIDENTIFIER NOT NULL,
	Id bigint IDENTITY(1000000000,1) NOT NULL,
	DFJurisdiction VARCHAR(10) NOT NULL,
	StateRegulatorURL VARCHAR(255) NOT NULL,
	StateRegulatorPhone VARCHAR(30) NOT NULL,
	LanguageCode VARCHAR(10) NOT NULL,
	Translation VARCHAR(255) NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	CONSTRAINT PK_tMGram_StateRegulators PRIMARY KEY CLUSTERED 
	(
		rowguid ASC
	)
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO