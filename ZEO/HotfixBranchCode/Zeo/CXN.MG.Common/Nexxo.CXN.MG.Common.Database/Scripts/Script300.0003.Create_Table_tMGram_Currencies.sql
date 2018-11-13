-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/01/2014>
-- Description:	<Table to hold MoneyGram currencies> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.tMGram_Currencies') AND type in (N'U'))
DROP TABLE dbo.tMGram_Currencies
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO	

CREATE TABLE dbo.tMGram_Currencies(
	rowguid UNIQUEIDENTIFIER NOT NULL,
	Id bigint IDENTITY(1000000000,1) NOT NULL,
	Code VARCHAR(10) NOT NULL,
	Name VARCHAR(255) NOT NULL,
	CurrencyPrecision VARCHAR(10) NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	CONSTRAINT PK_tMGram_Currencies PRIMARY KEY CLUSTERED 
	(
		rowguid ASC
	)
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO