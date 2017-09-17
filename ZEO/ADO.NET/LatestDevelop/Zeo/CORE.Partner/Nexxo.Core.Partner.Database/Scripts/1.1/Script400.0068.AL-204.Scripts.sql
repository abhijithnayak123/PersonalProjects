--=========================================================
-- Author: <Kaushik S>
-- Date Created: <March 31 2015>
-- Description: <As a Carver user, 
--				 I want to see updated Country names, 
--				 so that I can perform New Customer registration operation>
-- User Story ID: <AL-204>
--===========================================================================
DECLARE @channelPartnerId UNIQUEIDENTIFIER  

SELECT @channelPartnerId = rowguid FROM tChannelPartners WHERE Name ='Carver'

--Update Statement for Mexico
UPDATE tChannelPartnerIDTypeMapping
SET IsActive = 0 
WHERE NexxoIdTypeId IN 
( SELECT rowguid FROM tNexxoIdTypes WHERE Name IN( 'LICENCIA DE CONDUCIR','INSTITUTO FEDERAL ELECTORAL') AND Country = 'MEXICO' ) 
AND ChannelPartnerId = @channelPartnerId

--Update Statement for United States
UPDATE tChannelPartnerIDTypeMapping
SET IsActive = 0
WHERE NexxoIdTypeId  IN 
( SELECT rowguid FROM tNexxoIdTypes WHERE Name IN ('GREEN CARD / PERMANENT RESIDENT CARD','EMPLOYMENT AUTHORIZATION CARD (EAD0' ) AND Country = 'UNITED STATES' )
AND ChannelPartnerId = @channelPartnerId
