-- ==========================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <11/27/2015>
-- Description:	<Insert Visa Fee into tVisa_ShippingFee for Synovus and TCF>
-- Rally ID:	<AL-1639>
-- ==========================================================================


IF NOT EXISTS (SELECT 1 FROM SYS.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[tVisa_Fee]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE tVisa_Fee(
	VisaFeePK UNIQUEIDENTIFIER NOT NULL,
	VisaFeeId BIGINT IDENTITY(1000000000,1) NOT NULL,
	Fee DECIMAL(18, 0) NOT NULL,
	ChannelPartnerFeeTypePK UNIQUEIDENTIFIER NOT NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL
	
) ON [PRIMARY]




ALTER TABLE [dbo].[tVisa_Fee]  WITH CHECK ADD  CONSTRAINT [FK_tVisa_Fee_tVisa_ChannelPartnerFeeTypeMapping] FOREIGN KEY([ChannelPartnerFeeTypePK])
REFERENCES [dbo].[tVisa_ChannelPartnerFeeTypeMapping] ([ChannelPartnerFeeTypeMappingPK])


ALTER TABLE [dbo].[tVisa_Fee] CHECK CONSTRAINT [FK_tVisa_Fee_tVisa_ChannelPartnerFeeTypeMapping]

END
