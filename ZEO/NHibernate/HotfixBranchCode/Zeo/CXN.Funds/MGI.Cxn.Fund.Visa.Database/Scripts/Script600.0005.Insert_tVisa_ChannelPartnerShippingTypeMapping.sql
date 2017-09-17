-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/27/2015>
-- Description:	<DML script to insert Channelpartner and shippingtype Mapping data into tVisa_ChannelPartnerShippingTypeMapping table for Synovus Visa DPS>
-- Rally ID:	<AL-1642>
-- ============================================================
	DECLARE @StandardShippingType UNIQUEIDENTIFIER
	DECLARE @ExpressShippingType UNIQUEIDENTIFIER
	DECLARE @InstantIssueShippingType UNIQUEIDENTIFIER

	DECLARE @SynovusChannelPartnerId BIGINT = 33

	SELECT @StandardShippingType = ShippingTypePK FROM tVisa_CardShippingTypes WHERE Name = 'Standard Mail'
	SELECT @ExpressShippingType = ShippingTypePK FROM tVisa_CardShippingTypes WHERE Name = 'Express Shipping'
	SELECT @InstantIssueShippingType = ShippingTypePK FROM tVisa_CardShippingTypes WHERE Name = 'Instant Issue'
	
BEGIN
	IF NOT EXISTS (SELECT 1 FROM tVisa_ChannelPartnerShippingTypeMapping WHERE ChannelPartnerId = @SynovusChannelPartnerId 	and ShippingTypePK = @StandardShippingType)
	BEGIN
	
	-- Inserting Records for Synovus ChannelPartner	
	INSERT INTO tVisa_ChannelPartnerShippingTypeMapping(ChannelPartnerShippingTypePK, ShippingTypePK, ChannelPartnerId)
	VALUES
		  (NEWID(), @StandardShippingType, @SynovusChannelPartnerId)
	END
	
	IF NOT EXISTS (SELECT 1 FROM tVisa_ChannelPartnerShippingTypeMapping WHERE ChannelPartnerId = @SynovusChannelPartnerId 	and ShippingTypePK = @ExpressShippingType)
	BEGIN
	
	-- Inserting Records for Synovus ChannelPartner
	INSERT INTO tVisa_ChannelPartnerShippingTypeMapping(ChannelPartnerShippingTypePK, ShippingTypePK, ChannelPartnerId)
	VALUES
		  (NEWID(), @ExpressShippingType, @SynovusChannelPartnerId)
	END

	
	IF NOT EXISTS (SELECT 1 FROM tVisa_ChannelPartnerShippingTypeMapping WHERE ChannelPartnerId = @SynovusChannelPartnerId 	and ShippingTypePK = @InstantIssueShippingType)
	BEGIN

	-- Inserting Records for Synovus ChannelPartner
	INSERT INTO tVisa_ChannelPartnerShippingTypeMapping(ChannelPartnerShippingTypePK, ShippingTypePK, ChannelPartnerId)
	VALUES
		  (NEWID(), @InstantIssueShippingType, @SynovusChannelPartnerId)
	END

END

