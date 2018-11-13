-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/27/2015>
-- Description:	<Insert Visa Fee into tVisa_ShippingFee for Synovus and TCF>
-- Rally ID:	<AL-1639>
-- ==========================================================================
--tVisa_ShippingFee
DECLARE @channelPartnerFeeTypePK UNIQUEIDENTIFIER
DECLARE @channelPartnerShippingTypePK UNIQUEIDENTIFIER
DECLARE @channelPartnerID BIGINT = 33    -- Synovus Channel partner ID


-- INSERT Express Shipping fee as '20' for Synovus

SELECT @channelPartnerShippingTypePK = ChannelPartnerShippingTypePK 
FROM tVisa_ChannelPartnerShippingTypeMapping CS INNER JOIN tVisa_CardShippingTypes CP ON CS.ShippingTypePK = CP.ShippingTypePK
WHERE  CP.Name = 'Instant Issue Replace\Lost' AND CS.ChannelPartnerId = @channelPartnerID


IF NOT EXISTS ( SELECT 1 FROM tVisa_ShippingFee WHERE ChannelPartnerShippingTypePK = @channelPartnerShippingTypePK)
BEGIN

	INSERT INTO tVisa_ShippingFee
	(VisaShippingFeePK, Fee, DTServerCreate, DTServerLastModified, ChannelPartnerShippingTypePK)
	VALUES ( NEWID(), 10, GETDATE(), NULL, @channelPartnerShippingTypePK)

END
