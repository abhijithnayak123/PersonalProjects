-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <11/20/2015>
-- Description:	<Disabled the CANADIAN DRIVER''S LICENSE and PROVINCIAL/TERRITORIAL 
--				 IDENTITY CARD for CANADA country and TCF channel partner>
-- Jira ID:		<AL-2973>
-- ================================================================================


UPDATE 
	tChannelPartnerIDTypeMapping

SET 
	IsActive = 0

FROM 
	tChannelPartnerIDTypeMapping cpm
	INNER JOIN tChannelPartners cp ON cp.ChannelPartnerPK = cpm.ChannelPartnerId
	INNER JOIN tNexxoIdTypes nit ON nit.NexxoIdTypePK = cpm.NexxoIdTypeId
	INNER JOIN tMasterCountries mc ON mc.rowguid = nit.CountryPK
WHERE
 
	cp.Name = 'TCF'
	AND nit.Name = 'CANADIAN DRIVER''S LICENSE'
	AND mc.Name = 'CANADA'


UPDATE tChannelPartnerIDTypeMapping
	
	SET IsActive = 0

		FROM tChannelPartnerIDTypeMapping cpm
			INNER JOIN tChannelPartners cp ON cp.ChannelPartnerPK = cpm.ChannelPartnerId
			INNER JOIN tNexxoIdTypes nit ON nit.NexxoIdTypePK = cpm.NexxoIdTypeId
			INNER JOIN tMasterCountries mc ON mc.rowguid = nit.CountryPK
	WHERE cp.Name = 'TCF'

 AND nit.Name = 'PROVINCIAL/TERRITORIAL IDENTITY CARD'
 AND mc.Name = 'CANADA'