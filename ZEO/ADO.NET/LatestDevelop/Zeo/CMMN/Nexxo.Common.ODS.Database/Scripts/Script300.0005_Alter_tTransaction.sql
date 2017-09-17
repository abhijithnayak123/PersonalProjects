-- ============================================================
-- Author:        <Rogy Eapen>
-- Create date:   <12/03/2014>
-- Description:   <Added Surcharge Column in 'tTransaction' for ODS> 
-- Rally ID:      <DE3352 – TA6259>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'Surcharge' AND OBJECT_ID = OBJECT_ID(N'tTransaction'))
BEGIN
	ALTER TABLE tTransaction ADD
	Surcharge money
END
GO