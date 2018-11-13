--==========================================================================
-- Author: <Ashok Kumar G>
-- Date Created: <April 1 2015>
-- Description: <As a TCF user,
--				 I want to see updated Country names, 
--				 so that I can register a new customer>
-- User Story ID: <AL-108>
--===========================================================================
DECLARE @channelPartnerId UNIQUEIDENTIFIER  

SELECT @channelPartnerId = rowguid FROM tChannelPartners WHERE Name = 'TCF'

--Update Statement for Mexico
UPDATE tChannelPartnerIDTypeMapping
SET IsActive = 0 
WHERE NexxoIdTypeId IN 
( SELECT rowguid FROM tNexxoIdTypes WHERE Name IN( 'LICENCIA DE CONDUCIR','INSTITUTO FEDERAL ELECTORAL') AND Country = 'MEXICO' ) 
AND ChannelPartnerId = @channelPartnerId


UPDATE tChannelPartnerIDTypeMapping
SET IsActive = 1 
WHERE NexxoIdTypeId IN 
( SELECT rowguid FROM tNexxoIdTypes WHERE Name IN( 'PASAPORTE') AND Country = 'MEXICO' ) 
AND ChannelPartnerId = @channelPartnerId

--Update Statement for United States
UPDATE tChannelPartnerIDTypeMapping
SET IsActive = 0
WHERE NexxoIdTypeId  IN 
( SELECT rowguid FROM tNexxoIdTypes WHERE Name IN ('GREEN CARD / PERMANENT RESIDENT CARD','EMPLOYMENT AUTHORIZATION CARD (EAD0' ) 
AND Country = 'UNITED STATES' )
AND ChannelPartnerId = @channelPartnerId
