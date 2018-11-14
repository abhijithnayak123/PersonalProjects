-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <12/18/2015>
-- Description:	<Alter tVisa_ShippingFee to add new columns>
-- Rally ID:	<AL-3758>
-- ==========================================================================

-- Add a column called FeeCode to tVisa_ShippingFee, this FeeCode will be sent as part of Visa Api request

IF NOT EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tVisa_ShippingFee' AND  COLUMN_NAME = 'FeeCode'
	)
BEGIN
	ALTER TABLE tVisa_ShippingFee
	ADD FeeCode BIGINT
END
GO