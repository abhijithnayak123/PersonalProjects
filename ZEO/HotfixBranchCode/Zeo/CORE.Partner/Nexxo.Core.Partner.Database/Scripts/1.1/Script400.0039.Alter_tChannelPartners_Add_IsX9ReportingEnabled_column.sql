-- ============================================================
-- Author:		Adwait Ullal
-- Create date: <01/09/2015>
-- Description:	<Added a New Column IsX9ReportingEnabled, 
--				in 'tChannelPartners' to persist X9 reporting attribute
--				for a channel partner>
-- Rally ID:	<US1685>
-- ============================================================

-- PTNR database
IF NOT EXISTS 
(
	SELECT 
		*
	FROM 
		sys.columns 
	WHERE name = N'IsX9ReportingEnabled' 
	  AND object_id = OBJECT_ID(N'[dbo].[tChannelPartners]')
)
BEGIN
	ALTER TABLE tChannelPartners 
		ADD IsX9ReportingEnabled BIT NOT NULL
			CONSTRAINT DF_tChannelPartners_IsX9ReportingEnabled DEFAULT (0) WITH VALUES
END
GO