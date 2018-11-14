-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<User defined type for StateRegulator> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'StateRegulatorTableType' AND ss.name = N'dbo')
DROP TYPE [dbo].[StateRegulatorTableType]
GO

CREATE TYPE [dbo].[StateRegulatorTableType] AS TABLE
(
	DFJurisdiction VARCHAR(10) NOT NULL,
	StateRegulatorURL VARCHAR(255) NOT NULL,
	StateRegulatorPhone VARCHAR(30) NOT NULL,
	LanguageCode VARCHAR(10) NOT NULL,
	Translation VARCHAR(255) NOT NULL
)
GO
