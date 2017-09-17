-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/27/2015>
-- Description:	<Insert Visa Fee into tVisa_ShippingFee for Synovus and TCF>
-- Rally ID:	<AL-1639>
-- ==========================================================================

-- Update Express shipping fee code for Synovus and TCF

UPDATE tVisa_ShippingFee 
SET FeeCode = '1015'
WHERE VisaShippingFeeId = '1000000000' OR VisaShippingFeeId = '1000000002'
GO

-- Update Instant Issue shipping Fee code for Synovus 

UPDATE tVisa_ShippingFee
SET FeeCode = '1110'
WHERE VisaShippingFeeId = '1000000003' 
GO

---- Update Instant Issue(lost/stolen) shipping Fee code for Synovus

UPDATE tVisa_ShippingFee 
SET FeeCode = '1111'
WHERE VisaShippingFeeId = '1000000005'
GO