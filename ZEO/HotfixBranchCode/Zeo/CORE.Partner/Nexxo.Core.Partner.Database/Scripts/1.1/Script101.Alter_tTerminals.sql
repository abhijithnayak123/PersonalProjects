--===========================================================================================
-- Auther:			Bineesh Raghavan
-- Date Created:	10/02/2013
-- Description:		Introducing ChannelPartnerPK column in tTerminals table to fix 
--					multitenancy issue
--===========================================================================================
ALTER TABLE tTerminals
ADD ChannelPartnerPK UNIQUEIDENTIFIER NULL
CONSTRAINT FK_tTerminal_tChannelPartner FOREIGN KEY(ChannelPartnerPK) REFERENCES tChannelPartners(ROWGUID)
GO

UPDATE 
	tTerminals
SET 
	ChannelPartnerPK = chl.rowguid
FROM 
	tTerminals ter
INNER JOIN tLocations loc ON loc.rowguid = ter.LocationPK 
INNER JOIN tChannelPartners chl ON chl.id = loc.ChannelPartnerId
GO

ALTER TABLE tTerminals
ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NOT NULL
GO
--===========================================================================================