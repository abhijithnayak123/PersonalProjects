-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <12/18/2015>
-- Description:	<Insert the value into tVisa_ChannelPartnerFeeTypeMapping table>
-- Rally ID:	<AL-3758>
-- ==========================================================================

DECLARE @visaFeeTypePk UNIQUEIDENTIFIER
SELECT @visaFeeTypePk = VisaFeeTypePK FROM tVisa_FeeTypes WHERE Name = 'Mail Order Fee'
DECLARE @synovusChannelPartnerID BIGINT = 33
DECLARE @tcfChannelPartnerID BIGINT = 34

-- INSERT  Mail Order Fee mapping data to Synovus Channel partner

IF NOT EXISTS ( SELECT 1 FROM tVisa_ChannelPartnerFeeTypeMapping WHERE VisaFeeTypePk = @visaFeeTypePk AND ChannelPartnerID = @synovusChannelPartnerID)
BEGIN
	
	INSERT INTO tVisa_ChannelPartnerFeeTypeMapping 
	(ChannelPartnerFeeTypeMappingPK, VisaFeeTypePK, ChannelPartnerID,DTServerCreate)

	VALUES (NEWID() , @visaFeeTypePk, @synovusChannelPartnerID, GETDATE())

END

-- INSERT  Mail Order Fee mapping data to TCF Channel partner

IF NOT EXISTS ( SELECT 1 FROM tVisa_ChannelPartnerFeeTypeMapping WHERE VisaFeeTypePK = @visaFeeTypePk AND ChannelPartnerID = @tcfChannelPartnerID)
BEGIN

	INSERT INTO tVisa_ChannelPartnerFeeTypeMapping 
	(ChannelPartnerFeeTypeMappingPK, VisaFeeTypePK, ChannelPartnerID, DTServerCreate)

	VALUES (NEWID() , @visaFeeTypePk, @tcfChannelPartnerID,GETDATE())

END