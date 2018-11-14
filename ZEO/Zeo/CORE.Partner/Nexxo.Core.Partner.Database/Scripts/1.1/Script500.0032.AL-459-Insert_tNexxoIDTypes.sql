--================================================================
-- Author:		 <KAUSHIK S>
-- Date Created: <MAY 26 2015>
-- Description:  <As Carver,  I need NY State ID and Benefits ID 
--               added to list of IDs supported.>
-- Jira ID:		 <AL-459>
--=================================================================

--Adding new ID Types to tNexxoIdTypes 

DECLARE @masterCountryPK UNIQUEIDENTIFIER
SELECT @masterCountryPK = rowguid FROM tMasterCountries WHERE Name = 'UNITED STATES'

DECLARE @statePK UNIQUEIDENTIFIER
SELECT @statePK = StatePK FROM tStates 
WHERE CountryPK =(SELECT CountryPK FROM tCountries WHERE Name = 'UNITED STATES')
 AND NAME = 'NEW YORK'

IF NOT EXISTS(SELECT 1 FROM tNexxoIdTypes WHERE NexxoIdTypeID in( N'487',N'488'))
BEGIN 
	INSERT INTO tNexxoIdTypes VALUES
	(NEWID(), 487, 'NEW YORK CITY ID', '^[a-zA-Z0-9]{15}$', 1, 'UNITED STATES', 'NEW YORK', @masterCountryPK, @statePK, 1), 
	(NEWID(), 488, 'NEW YORK BENEFITS ID', '^[a-zA-Z0-9]{15}$', 0, 'UNITED STATES', 'NEW YORK', @masterCountryPK, @statePK, 1)

	--Mapping the new ID types to Channel Partner Carver.

	DECLARE @channelPartnerId UNIQUEIDENTIFIER 
	SELECT @channelPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name ='Carver'

	DECLARE @stateID UNIQUEIDENTIFIER
	DECLARE @benefitsID UNIQUEIDENTIFIER

	SELECT @stateID = NexxoIdTypePK FROM tNexxoIdTypes where Name = 'NEW YORK CITY ID'
	SELECT @benefitsID = NexxoIdTypePK FROM tNexxoIdTypes where Name = 'NEW YORK BENEFITS ID'

	INSERT INTO tChannelPartnerIDTypeMapping VALUES
	(NEWID(), @channelPartnerId, @stateID, 1), 
	(NEWID(), @channelPartnerId, @benefitsID, 1)
END
GO
