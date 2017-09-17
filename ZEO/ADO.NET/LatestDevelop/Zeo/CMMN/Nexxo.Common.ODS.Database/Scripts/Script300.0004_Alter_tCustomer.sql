-- ============================================================
-- Author:        <Rogy Eapen>
-- Create date:   <12/03/2014>
-- Description:   <Added Notes Column in 'tCustomer' for ODS> 
-- Rally ID:      <US2276 – TA6258>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'Notes' AND OBJECT_ID = OBJECT_ID(N'tCustomer'))
BEGIN
	ALTER TABLE tCustomer ADD
	Notes nvarchar(250)
END
GO
