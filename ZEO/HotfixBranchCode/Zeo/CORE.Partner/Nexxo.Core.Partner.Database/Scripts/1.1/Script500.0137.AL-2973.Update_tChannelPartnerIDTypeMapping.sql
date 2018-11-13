-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <11/25/2015>
-- Description:	<Disabled the CANADIAN DRIVER''S LICENSE and PROVINCIAL/TERRITORIAL 
--				 IDENTITY CARD and mapping PASSPORT id types for CANADA country and 
--				 Synovus channel partner>
-- Jira ID:		<AL-2973>
-- ================================================================================

----Script to map PAssport Id Type
DECLARE @NexxoIdTypeId UNIQUEIDENTIFIER 
DECLARE @ChannelPartnerId UNIQUEIDENTIFIER

SELECT @NexxoIdTypeId = NexxoIdTypePK FROM tNexxoIdTypes WHERE Name ='PASSPORT' AND Country ='CANADA'
SELECT @ChannelPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'Synovus'

IF NOT EXISTS ( Select 1 FROM tChannelPartnerIDTypeMapping
WHERE ChannelPartnerId =  @ChannelPartnerId  AND NexxoIdTypeId =  @NexxoIdTypeId )
BEGIN
	INSERT INTO tChannelPartnerIDTypeMapping (rowguid, ChannelPartnerId, NexxoIdTypeId, IsActive)
	VALUES 	( NEWID(), @ChannelPartnerId, @NexxoIdTypeId, 1)
END



--Script to remove mapping for CANADIAN DRIVER''S LICENSE and PROVINCIAL/TERRITORIAL IDENTITY CARD Id Type 
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
	cp.Name = 'Synovus'
	AND nit.Name = 'CANADIAN DRIVER''S LICENSE'
	AND mc.Name = 'CANADA'
GO

UPDATE 
	tChannelPartnerIDTypeMapping
SET 
	IsActive = 0
FROM tChannelPartnerIDTypeMapping cpm
	INNER JOIN tChannelPartners cp ON cp.ChannelPartnerPK = cpm.ChannelPartnerId
	INNER JOIN tNexxoIdTypes nit ON nit.NexxoIdTypePK = cpm.NexxoIdTypeId
	INNER JOIN tMasterCountries mc ON mc.rowguid = nit.CountryPK
WHERE 
	cp.Name = 'Synovus'
	AND nit.Name = 'PROVINCIAL/TERRITORIAL IDENTITY CARD'
	AND mc.Name = 'CANADA'
GO
