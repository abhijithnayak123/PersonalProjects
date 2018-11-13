-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<User defined type for countries> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'StateTableType' AND ss.name = N'dbo')
DROP TYPE [dbo].[StateTableType]
GO

CREATE TYPE [dbo].[StateTableType] AS TABLE
(
	CountryCode VARCHAR(10) NOT NULL,
	StateProvinceCode VARCHAR(10) NOT NULL,
	StateProvinceName VARCHAR(255) NOT NULL
)
GO

