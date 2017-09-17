-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<User defined type for countries> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'CountryTableType' AND ss.name = N'dbo')
DROP TYPE [dbo].[CountryTableType]
GO

CREATE TYPE [dbo].[CountryTableType] AS TABLE(
	CountryCode VARCHAR(10) NOT NULL,
	CountryName VARCHAR(255) NOT NULL,
	CountryLegacyCode varchar(10) NOT NULL,
	SendActive bit NULL,
	ReceiveActive BIT NULL,
	DirectedSendCountry BIT NULL,
	MGDirectedSendCountry BIT NULL
)
GO