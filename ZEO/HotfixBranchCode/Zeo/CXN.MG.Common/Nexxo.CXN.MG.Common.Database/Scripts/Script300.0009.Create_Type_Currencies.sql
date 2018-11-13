-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<User defined type for countries> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'CurrencyTableType' AND ss.name = N'dbo')
DROP TYPE [dbo].[CurrencyTableType]
GO

CREATE TYPE [dbo].[CurrencyTableType] AS TABLE
(
	CurrencyCode VARCHAR(10) NOT NULL,
	CurrencyName VARCHAR(255) NOT NULL,
	CurrencyPrecision VARCHAR(10) NOT NULL
)
GO

