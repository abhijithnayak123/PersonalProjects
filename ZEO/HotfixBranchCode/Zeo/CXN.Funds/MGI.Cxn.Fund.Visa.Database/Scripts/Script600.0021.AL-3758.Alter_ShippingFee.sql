-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <12/18/2015>
-- Description:	<Alter tVisa_ShippingFee to add new columns>
-- Rally ID:	<AL-3758>
-- ==========================================================================

-- Add a column called Active to tVisa_ShippingFee

IF NOT EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tVisa_CardShippingTypes' AND  COLUMN_NAME = 'Active'
	)
BEGIN
	ALTER TABLE tVisa_CardShippingTypes
	ADD Active BIT NOT NULL DEFAULT(1)
END
GO

