-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<User defined type for countries> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'CountryCurrencyTableType' AND ss.name = N'dbo')
DROP TYPE [dbo].[CountryCurrencyTableType]
GO

CREATE TYPE [dbo].[CountryCurrencyTableType] AS TABLE
(
	CountryCode VARCHAR(10) NOT NULL,
	BaseCurrency VARCHAR(10) NOT NULL,
	LocalCurrency VARCHAR(10) NULL,
	ReceiveCurrency VARCHAR(10) NOT NULL,
	IndicativeRateAvailable BIT NULL,	
	DeliveryOption VARCHAR(255) NOT NULL
)
GO
