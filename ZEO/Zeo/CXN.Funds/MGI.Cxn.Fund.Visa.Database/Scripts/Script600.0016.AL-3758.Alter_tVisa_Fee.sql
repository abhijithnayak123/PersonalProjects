-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <12/18/2015>
-- Description:	<Alter tVisa_Fee to add new column>
-- Rally ID:	<AL-3758>
-- ==========================================================================

-- Add a column called FeeCode to tVisa_Fee, this FeeCode will be sent as part of Visa Api request

IF NOT EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tVisa_Fee' AND  COLUMN_NAME = 'FeeCode'
	)
BEGIN
	ALTER TABLE tVISA_Fee
	ADD FeeCode BIGINT
END
GO

-- Add a column called StockId to tVisa_Fee, this StockId will be sent as part of Visa Api request

IF NOT EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tVisa_Fee' AND  COLUMN_NAME = 'StockId'
	)
BEGIN
	ALTER TABLE tVISA_Fee
	ADD StockId VARCHAR(20)
END
GO

