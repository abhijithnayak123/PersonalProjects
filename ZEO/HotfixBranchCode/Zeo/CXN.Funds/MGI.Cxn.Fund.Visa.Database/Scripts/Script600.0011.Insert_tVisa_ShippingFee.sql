-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/27/2015>
-- Description:	<Insert Visa Fee into tVisa_ShippingFee for Synovus and TCF>
-- Rally ID:	<AL-1639>
-- ==========================================================================
--tVisa_ShippingFee
DECLARE @channelPartnerFeeTypePK UNIQUEIDENTIFIER
DECLARE @channelPartnerShippingTypePK UNIQUEIDENTIFIER
DECLARE @synovusChannelPartnerID BIGINT = 33
DECLARE @tcfChannelPartnerID BIGINT = 34


-- INSERT Express Shipping fee as '20' for Synovus

SELECT @channelPartnerShippingTypePK = ChannelPartnerShippingTypePK 
FROM tVisa_ChannelPartnerShippingTypeMapping CS INNER JOIN tVisa_CardShippingTypes CP ON CS.ShippingTypePK = CP.ShippingTypePK
WHERE  CP.Name = 'Express Shipping' AND CS.ChannelPartnerId = @synovusChannelPartnerID


IF NOT EXISTS ( SELECT 1 FROM tVisa_ShippingFee WHERE ChannelPartnerShippingTypePK = @channelPartnerShippingTypePK)
BEGIN

	INSERT INTO tVisa_ShippingFee
	(VisaShippingFeePK, Fee, DTServerCreate, DTServerLastModified, ChannelPartnerShippingTypePK)
	VALUES ( NEWID(), 20, GETDATE(), NULL, @channelPartnerShippingTypePK)

END


-- INSERT Instant Issue Replacement fee as '10' for Synovus

SELECT @channelPartnerShippingTypePK = ChannelPartnerShippingTypePK 
FROM tVisa_ChannelPartnerShippingTypeMapping CS INNER JOIN tVisa_CardShippingTypes CP ON CS.ShippingTypePK = CP.ShippingTypePK
WHERE  CP.Name = 'Instant Issue' AND CS.ChannelPartnerId = @synovusChannelPartnerID

IF NOT EXISTS (SELECT 1 FROM tVisa_ShippingFee WHERE ChannelPartnerShippingTypePK = @channelPartnerShippingTypePK )
BEGIN

	INSERT INTO tVisa_ShippingFee
	(VisaShippingFeePK, Fee, DTServerCreate, DTServerLastModified, ChannelPartnerShippingTypePK)
	VALUES ( NEWID(), 10, GETDATE(), NULL, @channelPartnerShippingTypePK)

END

-- INSERT Standard Mail shipping fee as '0' for Synovus

SELECT @channelPartnerShippingTypePK = ChannelPartnerShippingTypePK 
FROM tVisa_ChannelPartnerShippingTypeMapping CS INNER JOIN tVisa_CardShippingTypes CP ON CS.ShippingTypePK = CP.ShippingTypePK
WHERE  CP.Name = 'Standard Mail' AND CS.ChannelPartnerId = @synovusChannelPartnerID

IF NOT EXISTS ( SELECT 1 FROM tVisa_ShippingFee WHERE ChannelPartnerShippingTypePK = @channelPartnerShippingTypePK )
BEGIN

	INSERT INTO tVisa_ShippingFee
	(VisaShippingFeePK, Fee, DTServerCreate, DTServerLastModified, ChannelPartnerShippingTypePK)
	VALUES ( NEWID(), 0, GETDATE(), NULL, @channelPartnerShippingTypePK)

END

-- Update channel partner shipping type PK for Standard Mail shipping Type to TCF

SELECT @channelPartnerShippingTypePK = ChannelPartnerShippingTypePK 
FROM tVisa_ChannelPartnerShippingTypeMapping CS INNER JOIN tVisa_CardShippingTypes CP ON CS.ShippingTypePK = CP.ShippingTypePK
WHERE  CP.Name = 'Express Shipping' AND CS.ChannelPartnerId = @tcfChannelPartnerID


UPDATE tVisa_ShippingFee SET ChannelPartnerShippingTypePK = @channelPartnerShippingTypePK WHERE VisaShippingFeeId = 1000



-- Update channel partner shipping type PK for Standard Mail shipping Type to TCF

SELECT @channelPartnerShippingTypePK = ChannelPartnerShippingTypePK 
FROM tVisa_ChannelPartnerShippingTypeMapping CS INNER JOIN tVisa_CardShippingTypes CP ON CS.ShippingTypePK = CP.ShippingTypePK
WHERE  CP.Name = 'Standard Mail' AND CS.ChannelPartnerId = @tcfChannelPartnerID


UPDATE tVisa_ShippingFee SET ChannelPartnerShippingTypePK = @channelPartnerShippingTypePK WHERE VisaShippingFeeId = 1001




