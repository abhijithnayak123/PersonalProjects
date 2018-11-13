-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/27/2015>
-- Description:	<Alter tVisa_ShippingFee to add new columns>
-- Rally ID:	<AL-1639>
-- ==========================================================================

-- DROP the unique constraint of ShippingType column in tVisa_ShippingFee table
-- DROP the ShippingType column from tVisa_ShippingFee table

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME='IX_tVisa_ShippingFee_ShippingType')
BEGIN
  ALTER TABLE tVisa_ShippingFee DROP CONSTRAINT IX_tVisa_ShippingFee_ShippingType
END

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_ShippingFee'
			AND COLUMN_NAME = 'ShippingType'
		)
BEGIN

   ALTER TABLE tVisa_ShippingFee DROP COLUMN  ShippingType

END

-- ADD ChannelPartnerShippingTypePK column in tVisa_ShippingFee

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_ShippingFee'
			AND COLUMN_NAME = 'ChannelPartnerShippingTypePK'
		)
BEGIN
	ALTER TABLE tVisa_ShippingFee ADD ChannelPartnerShippingTypePK UNIQUEIDENTIFIER NULL
END

-- ADD foreign key reference ChannelPartnerShippingTypePK column in tVisa_ShippingFee

IF NOT EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE Name = 'FK_tVisa_Fee_tVisa_ChannelPartnerShippingTypeMapping' AND TYPE in (N'F'))
BEGIN

	ALTER TABLE tVisa_ShippingFee  WITH CHECK ADD  CONSTRAINT FK_tVisa_Fee_tVisa_ChannelPartnerShippingTypeMapping 
	FOREIGN KEY(ChannelPartnerShippingTypePK)
	REFERENCES tVisa_ChannelPartnerShippingTypeMapping (ChannelPartnerShippingTypePK)

END
GO
