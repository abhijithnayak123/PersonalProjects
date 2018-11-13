--===========================================================================================
-- Author:			<Chinar Kulkarni>
-- Date Created:	05/07/2015
-- User Story:      AL-208
-- Description:		<Script for inserting new ID Types and channel partner mappings for new channel partner - Redstone >
--===========================================================================================

-- 1. tNexxoIdTypes table

IF NOT EXISTS (SELECT 1 FROM tNexxoIdTypes WHERE Country = 'UNITED STATES' AND Name = 'FEDERAL EMPLOYEE ID')
BEGIN
	INSERT INTO tNexxoIdTypes (NexxoIdTypePK, Name, Mask, HasExpirationDate, Country, State, CountryPK, StatePK, IsActive)
	VALUES ('0f2ca0ec-7268-4a51-bfa1-19a0825fc4da', 'FEDERAL EMPLOYEE ID', '^\w{4,15}$', 0, 'UNITED STATES', NULL, '32D2E289-3319-49B0-B630-FB0A1E5FCEC0', NULL, 1)
END

IF NOT EXISTS (SELECT 1 FROM tNexxoIdTypes WHERE Country = 'UNITED STATES' AND Name = 'NYC ID/BENEFITS ID')
BEGIN
	INSERT INTO tNexxoIdTypes (NexxoIdTypePK, Name, Mask, HasExpirationDate, Country, State, CountryPK, StatePK, IsActive)
	VALUES ('65c9d477-8615-4694-b6d4-4838f3473bb2', 'NYC ID/BENEFITS ID', '^\w{4,15}$', 0, 'UNITED STATES', NULL, '32D2E289-3319-49B0-B630-FB0A1E5FCEC0', NULL, 1)
END


-- 2. tChannelPartnerIDTypeMapping table

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerIDTypeMapping WHERE ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' AND NexxoIdTypeId = '0f2ca0ec-7268-4a51-bfa1-19a0825fc4da')
BEGIN
	INSERT INTO tChannelPartnerIDTypeMapping (ChannelPartnerId, NexxoIdTypeId, IsActive)
	VALUES('F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', '0f2ca0ec-7268-4a51-bfa1-19a0825fc4da', 1)
END	

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerIDTypeMapping WHERE ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' AND NexxoIdTypeId = '65c9d477-8615-4694-b6d4-4838f3473bb2')
BEGIN
	INSERT INTO tChannelPartnerIDTypeMapping (ChannelPartnerId, NexxoIdTypeId, IsActive)
	VALUES('F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', '65c9d477-8615-4694-b6d4-4838f3473bb2', 1)
END	

