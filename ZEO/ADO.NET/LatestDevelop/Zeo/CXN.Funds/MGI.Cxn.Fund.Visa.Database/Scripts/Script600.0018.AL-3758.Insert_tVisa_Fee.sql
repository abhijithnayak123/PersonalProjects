-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/27/2015>
-- Description:	<Insert Visa Fee into tVisa_Fee for Synovus and TCF>
-- Rally ID:	<AL-1639>
-- ==========================================================================

--tVisa_Fee
DECLARE @channelPartnerFeeTypePK UNIQUEIDENTIFIER
DECLARE @channelPartnerShippingTypePK UNIQUEIDENTIFIER
DECLARE @synovusChannelPartnerID BIGINT = 33
DECLARE @tcfChannelPartnerID BIGINT = 34

 -- INSERT Mail Order Fee as '5' for Synovus

SELECT @channelPartnerFeeTypePK = ChannelPartnerFeeTypeMappingPK 
FROM tVisa_ChannelPartnerFeeTypeMapping FM INNER JOIN tVisa_FeeTypes FT ON FM.VisaFeeTypePK = FT.VisaFeeTypePK
WHERE FT.Name = 'Mail Order Fee' AND FM.ChannelPartnerID = @synovusChannelPartnerID


IF NOT EXISTS ( SELECT 1 FROM tVisa_Fee WHERE ChannelPartnerFeeTypePK = @channelPartnerFeeTypePK )
BEGIN

	INSERT INTO tVisa_Fee
	(VisaFeePK, Fee, DTServerCreate, DTServerLastModified, ChannelPartnerFeeTypePK)
	VALUES ( NEWID(), 5, GETDATE(), NULL, @channelPartnerFeeTypePK)

END

 -- INSERT Mail Order Fee as '0' for TCF

SELECT @channelPartnerFeeTypePK = ChannelPartnerFeeTypeMappingPK 
FROM tVisa_ChannelPartnerFeeTypeMapping FM INNER JOIN tVisa_FeeTypes FT ON FM.VisaFeeTypePK = FT.VisaFeeTypePK
WHERE FT.Name = 'Mail Order Fee' AND FM.ChannelPartnerID = @tcfChannelPartnerID


IF NOT EXISTS ( SELECT 1 FROM tVisa_Fee WHERE ChannelPartnerFeeTypePK = @channelPartnerFeeTypePK )
BEGIN

	INSERT INTO tVisa_Fee
	(VisaFeePK, Fee, DTServerCreate, DTServerLastModified, ChannelPartnerFeeTypePK)
	VALUES ( NEWID(), 0, GETDATE(), NULL, @channelPartnerFeeTypePK)

END