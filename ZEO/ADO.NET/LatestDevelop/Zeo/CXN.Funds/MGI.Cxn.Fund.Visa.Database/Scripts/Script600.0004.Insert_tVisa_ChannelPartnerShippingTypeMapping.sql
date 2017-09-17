-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/23/2015>
-- Description:	<DML script to insert Channelpartner and shippingtype Mapping data into tVisa_ChannelPartnerShippingTypeMapping table>
-- Rally ID:	<AL-1647>
-- ============================================================
	DECLARE @StandardShippingType UNIQUEIDENTIFIER
	DECLARE @ExpressShippingType UNIQUEIDENTIFIER
	DECLARE @TCFChannelPartnerId BIGINT = 34

	select @StandardShippingType = ShippingTypePK from tVisa_CardShippingTypes where Name = 'Standard Mail'
	select @ExpressShippingType = ShippingTypePK from tVisa_CardShippingTypes where Name = 'Express Shipping'
	IF NOT EXISTS (SELECT 1 FROM tVisa_ChannelPartnerShippingTypeMapping WHERE ChannelPartnerId = @TCFChannelPartnerId and ShippingTypePK = @StandardShippingType )
	BEGIN
	-- Inserting Records for TCF ChannelPartner
	INSERT INTO tVisa_ChannelPartnerShippingTypeMapping(ChannelPartnerShippingTypePK, ShippingTypePK, ChannelPartnerId)
	VALUES
		  (NEWID(), @ExpressShippingType, 34),
		  (NEWID(), @StandardShippingType, 34)
	END
GO
