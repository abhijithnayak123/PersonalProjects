-- ============================================================
-- Author:        <Rogy Eapen>
-- Create date:   <12/03/2014>
-- Description:   <Added CheckDeclineReason Column in 'tTransaction' for ODS> 
-- Rally ID:      <US2016 – TA6256>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'CheckDeclineReason' AND OBJECT_ID = OBJECT_ID(N'tTransaction'))
BEGIN
	ALTER TABLE tTransaction ADD
	CheckDeclineReason nvarchar(250)
END
GO