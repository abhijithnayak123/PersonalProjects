-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/27/2015>
-- Description:	<Insert Visa Fee into tVisa_ShippingFee for Synovus and TCF>
-- Rally ID:	<AL-1639>
-- ==========================================================================
IF EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tVisa_ShippingFee' AND  COLUMN_NAME = 'VisaShippingFeeId'
	)
BEGIN

	ALTER TABLE tVisa_ShippingFee DROP COLUMN VisaShippingFeeId
	ALTER TABLE tVisa_ShippingFee ADD VisaShippingFeeId BIGINT IDENTITY(1000000000, 1) NOT NULL

END
