-- ==========================================================================
-- Author:		<Sunil Shetty>
-- Create date: <12/18/2015>
-- Description:	<Add CardReplacement and Mail Order Fee Code in Synovus and TCF>
-- Rally ID:	<AL-3758>
-- ==========================================================================

--  VisaFeeId is a Identity column of tVisa_Fee table.
--  Update the card replacement Fee code.
  
UPDATE tVisa_Fee 
SET FeeCode = '1008'
WHERE VisaFeeId in ('1000000000' ,'1000000001')
GO
-- Update the mail order Fee code

UPDATE tVisa_Fee 
SET FeeCode = '1013'
WHERE VisaFeeId in ('1000000002', '1000000003')
GO


--Updating the StockId for card replacement Fee code.

UPDATE tVisa_Fee 
SET StockId ='127CS202'
FROM tvisa_feetypes ft
INNER JOIN tVisa_ChannelPartnerFeeTypeMapping cp ON ft.VisaFeeTypePK = cp.VisaFeeTypePK
INNER JOIN tVisa_Fee f ON cp.ChannelPartnerFeeTypeMappingPK = f.ChannelPartnerFeeTypePK
WHERE ChannelPartnerID = 34 AND FeeCode = '1008'

UPDATE tVisa_Fee 
SET StockId ='967CS002'
FROM tvisa_feetypes ft
INNER JOIN tVisa_ChannelPartnerFeeTypeMapping cp ON ft.VisaFeeTypePK = cp.VisaFeeTypePK
INNER JOIN tVisa_Fee f ON cp.ChannelPartnerFeeTypeMappingPK = f.ChannelPartnerFeeTypePK
WHERE ChannelPartnerID = 33 AND FeeCode = '1008'


--Updating the StockId for mail order Fee code.
UPDATE tVisa_Fee 
SET StockId ='127CS202'
FROM tvisa_feetypes ft
INNER JOIN tVisa_ChannelPartnerFeeTypeMapping cp ON ft.VisaFeeTypePK = cp.VisaFeeTypePK
INNER JOIN tVisa_Fee f ON cp.ChannelPartnerFeeTypeMappingPK = f.ChannelPartnerFeeTypePK
WHERE ChannelPartnerID = 34 AND FeeCode = '1013'


UPDATE tVisa_Fee 
SET StockId ='967CS002'
FROM tvisa_feetypes ft
INNER JOIN tVisa_ChannelPartnerFeeTypeMapping cp ON ft.VisaFeeTypePK = cp.VisaFeeTypePK
INNER JOIN tVisa_Fee f ON cp.ChannelPartnerFeeTypeMappingPK = f.ChannelPartnerFeeTypePK
WHERE ChannelPartnerID = 33 AND FeeCode = '1013' 