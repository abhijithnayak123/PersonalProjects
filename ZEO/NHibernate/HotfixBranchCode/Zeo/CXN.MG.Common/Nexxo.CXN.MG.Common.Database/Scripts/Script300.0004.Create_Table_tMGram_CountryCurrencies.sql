-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/01/2014>
-- Description:	<Table to hold countryCurrencies supported by MoneyGram> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.tMGram_CountryCurrencies') AND type in (N'U'))
DROP TABLE dbo.tMGram_CountryCurrencies
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO	

CREATE TABLE dbo.tMGram_CountryCurrencies(
	rowguid UNIQUEIDENTIFIER NOT NULL,
	Id bigint IDENTITY(1000000000,1) NOT NULL,
	CountryCode VARCHAR(10) NOT NULL,
	BaseCurrency VARCHAR(10) NOT NULL,
	LocalCurrency VARCHAR(10) NULL,
	ReceiveCurrency VARCHAR(10) NOT NULL,
	IndicativeRateAvailable BIT NULL,	
	DeliveryOption VARCHAR(255) NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	CONSTRAINT PK_tMGram_CountryCurrencies PRIMARY KEY CLUSTERED 
	(
		rowguid ASC
	)
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO