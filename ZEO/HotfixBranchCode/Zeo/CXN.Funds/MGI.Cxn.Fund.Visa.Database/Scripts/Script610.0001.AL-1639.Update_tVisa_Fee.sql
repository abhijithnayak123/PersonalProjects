--==================================================================================================
-- Author:		<KAUSHIK SAKALA>
-- Created date: <18/01/2016>
-- Description:	<As Synovus, Configure limits and fees for VISA DPS Prepaid cards>           
-- Jira ID:	<AL-1639>
--===================================================================================================

UPDATE tVisa_Fee
SET Fee = 4
FROM tVisa_Fee vf
JOIN tVisa_ChannelPartnerFeeTypeMapping cpft
 ON
 	 vf.ChannelPartnerFeeTypePK = cpft.ChannelPartnerFeeTypeMappingPK
JOIN tVisa_FeeTypes vft 
 ON
 	vft.VisaFeeTypePK = cpft.VisaFeeTypePK
WHERE  
   ChannelPartnerID = 34 AND Name ='Card Replacement Fee'
