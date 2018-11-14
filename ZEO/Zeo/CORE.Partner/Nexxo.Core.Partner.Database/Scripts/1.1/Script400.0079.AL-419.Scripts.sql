--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <April 24 2015>
-- Description:	<Script to insert missing Countrymapping list in 
--              tNexxoIdTypes and tChannelPartnerIDTypeMapping table>
-- Jira ID:	    <AL-419>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM tNexxoIdTypes WHERE Id = 465 and CountryId = '2B60C245-978F-4825-8BFC-8F74F67F2C59')
BEGIN
	INSERT INTO tNexxoIdTypes (rowguid, Id, Name, Mask, HasExpirationDate, Country, CountryId, IsActive) VALUES
	('6B024196-7BBA-486B-B561-D6FCBDAB0C61', 465, 'PASSPORT', '^\w{4,15}$', 1, 'LAOS', '2B60C245-978F-4825-8BFC-8F74F67F2C59', 1),
	('82A110C2-864C-4C4F-87B8-603F18CC7C59', 466, 'PASSPORT', '^\w{4,15}$', 1, 'RUSSIA', '52AC1FB4-5E6F-4246-8AE4-3B9E31189329', 1)	
END
GO

-- TCF
IF NOT EXISTS(SELECT 1 FROM  tChannelPartnerIDTypeMapping WHERE NexxoIdTypeId = '6B024196-7BBA-486B-B561-D6FCBDAB0C61' and ChannelPartnerId = '6D7E785F-7BDD-42C8-BC49-44536A1885FC')
BEGIN
	INSERT INTO tChannelPartnerIDTypeMapping (ChannelPartnerId, NexxoIdTypeId, IsActive) VALUES
	('6D7E785F-7BDD-42C8-BC49-44536A1885FC','6B024196-7BBA-486B-B561-D6FCBDAB0C61',1),
	('6D7E785F-7BDD-42C8-BC49-44536A1885FC','82A110C2-864C-4C4F-87B8-603F18CC7C59',1)
END
GO

-- Carver
IF NOT EXISTS(SELECT 1 FROM  tChannelPartnerIDTypeMapping WHERE NexxoIdTypeId = '6B024196-7BBA-486B-B561-D6FCBDAB0C61' and ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6')
BEGIN
    INSERT INTO tChannelPartnerIDTypeMapping (ChannelPartnerId, NexxoIdTypeId, IsActive) VALUES
	('578AC8FB-F69C-4DBD-A502-57B1EECD41D6','6B024196-7BBA-486B-B561-D6FCBDAB0C61',1),
	('578AC8FB-F69C-4DBD-A502-57B1EECD41D6','82A110C2-864C-4C4F-87B8-603F18CC7C59',1)
END
GO

-- MGI
IF NOT EXISTS(SELECT 1 FROM  tChannelPartnerIDTypeMapping WHERE NexxoIdTypeId = '6B024196-7BBA-486B-B561-D6FCBDAB0C61' and ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354')
BEGIN
    INSERT INTO tChannelPartnerIDTypeMapping (ChannelPartnerId, NexxoIdTypeId, IsActive) VALUES
	('10F2865B-DBC5-4A0B-983C-62E0A0574354','6B024196-7BBA-486B-B561-D6FCBDAB0C61',1),
	('10F2865B-DBC5-4A0B-983C-62E0A0574354','82A110C2-864C-4C4F-87B8-603F18CC7C59',1)
END
GO
