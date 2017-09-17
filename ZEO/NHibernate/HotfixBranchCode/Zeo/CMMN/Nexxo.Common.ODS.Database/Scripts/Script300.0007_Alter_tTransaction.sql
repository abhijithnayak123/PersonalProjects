-- ============================================================
-- Author:        <Rogy Eapen>
-- Create date:   <12/15/2014>
-- Description:   <Alter TellerUserName Column in 'tTransaction' for ODS> 
-- Rally ID:      <DE3430 – TA6489>
-- ============================================================
IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'TellerUserName' AND OBJECT_ID = OBJECT_ID(N'tTransaction'))
BEGIN
	ALTER TABLE tTransaction ALTER COLUMN
	TellerUserName nvarchar(255)
END
GO