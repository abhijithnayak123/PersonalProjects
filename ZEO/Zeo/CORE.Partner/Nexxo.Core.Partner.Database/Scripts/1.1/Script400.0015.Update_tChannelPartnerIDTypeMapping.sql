-- ============================================================
-- Author:		<RAJKUMAR>
-- Create date: <14/11/2014>
-- Description:	<Script to restrict GovId types for TCF channel partner>
-- Rally ID:	<US1610>
-- ============================================================


UPDATE tChannelPartnerIDTypeMapping 
	SET ISACTIVE = 0  
FROM
	tChannelPartnerIDTypeMapping CPID
	INNER JOIN tChannelPartners CP ON CPID.ChannelPartnerId=CP.rowguid
	INNER JOIN tNexxoIdTypes NID ON NID.rowguid = CPID.NexxoIdTypeId
WHERE CP.Name = 'TCF' 
	AND CPID.IsActive = 1
	AND Country = 'UNITED STATES'
	AND NID.Name IN ('EMPLOYMENT AUTHORIZATION CARD (EAD0', 'GREEN CARD / PERMANENT RESIDENT CARD')
GO

UPDATE tChannelPartnerIDTypeMapping 
	SET ISACTIVE = 0 
FROM
	tChannelPartnerIDTypeMapping CPID 
	INNER JOIN tChannelPartners CP ON CPID.ChannelPartnerId=CP.rowguid 
	INNER JOIN tNexxoIdTypes NID ON NID.rowguid = CPID.NexxoIdTypeId
WHERE  CP.Name='TCF' 
	AND CPID.IsActive = 1
	AND Country = 'MEXICO'
	AND NID.Name NOT IN('MATRICULA CONSULAR')

GO