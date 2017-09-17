-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/01/2014>
-- Description:	<Table to hold countries supported by MoneyGram> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.tMGram_Countries') AND type in (N'U'))
DROP TABLE dbo.tMGram_Countries
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO	

CREATE TABLE dbo.tMGram_Countries(
	rowguid UNIQUEIDENTIFIER NOT NULL,
	Id bigint IDENTITY(1000000000,1) NOT NULL,
	Code VARCHAR(10) NOT NULL,
	Name VARCHAR(255) NOT NULL,
	LegacyCode VARCHAR(10) NOT NULL,
	SendActive BIT NULL,	
	ReceiveActive BIT NULL,	
	DirectedSendCountry BIT NULL,
	MGDirectedSendCountry BIT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	CONSTRAINT PK_tMGram_Countries PRIMARY KEY CLUSTERED 
	(
		rowguid ASC
	)
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE tMGram_Countries
ADD  CONSTRAINT [DF_tMGram_Countries_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
GO