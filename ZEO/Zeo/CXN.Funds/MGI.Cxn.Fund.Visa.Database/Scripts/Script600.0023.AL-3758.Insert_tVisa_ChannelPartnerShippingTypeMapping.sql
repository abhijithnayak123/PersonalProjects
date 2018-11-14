-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <12/18/2015>
-- Description:	<DML script to insert Channelpartner and shippingtype Mapping data into tVisa_ChannelPartnerShippingTypeMapping table for Synovus Visa DPS>
-- Rally ID:	<AL-3758>
-- ============================================================
	
	DECLARE @InstantIssueLostORStolenShippingType UNIQUEIDENTIFIER

	DECLARE @SynovusChannelPartnerId BIGINT = 33

	SELECT @InstantIssueLostORStolenShippingType = ShippingTypePK FROM tVisa_CardShippingTypes WHERE Name = 'Instant Issue Replace\Lost'
	
BEGIN
	IF NOT EXISTS (SELECT 1 FROM tVisa_ChannelPartnerShippingTypeMapping WHERE ChannelPartnerId = @SynovusChannelPartnerId 	AND ShippingTypePK = @InstantIssueLostORStolenShippingType)
	BEGIN
	
	-- Inserting Records for Synovus ChannelPartner	
	INSERT INTO tVisa_ChannelPartnerShippingTypeMapping(ChannelPartnerShippingTypePK, ShippingTypePK, ChannelPartnerId)
	VALUES
		  (NEWID(), @InstantIssueLostORStolenShippingType, @SynovusChannelPartnerId)
	END
END
