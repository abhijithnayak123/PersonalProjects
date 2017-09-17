--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <April 24 2015>
-- Description:	<Script to insert ChannelPartnerMasterCountries list in the table>
-- Jira ID:	    <AL-419>
--===========================================================================================

------------------------------ Insert script for  TCF ------------------------------------

IF NOT EXISTS(SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = '2B60C245-978F-4825-8BFC-8F74F67F2C59' and ChannelPartnerId = '6D7E785F-7BDD-42C8-BC49-44536A1885FC')
  BEGIN
	INSERT INTO tChannelPartnerMasterCountryMapping(rowguid, ChannelPartnerId, MasterCountryId) VALUES	(NEWID(), '6D7E785F-7BDD-42C8-BC49-44536A1885FC', '2B60C245-978F-4825-8BFC-8F74F67F2C59'),	(NEWID(), '6D7E785F-7BDD-42C8-BC49-44536A1885FC', '52AC1FB4-5E6F-4246-8AE4-3B9E31189329')  END

------------------------------ Insert script for  MGI ------------------------------------

IF NOT EXISTS(SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = '2B60C245-978F-4825-8BFC-8F74F67F2C59' and ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354')
  BEGIN
	INSERT INTO tChannelPartnerMasterCountryMapping(rowguid, ChannelPartnerId, MasterCountryId) VALUES	(NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', '2B60C245-978F-4825-8BFC-8F74F67F2C59'),	(NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', '52AC1FB4-5E6F-4246-8AE4-3B9E31189329')  END

 ------------------------------ Insert script for  SYNOVUS ------------------------------------

 IF NOT EXISTS(SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = '2B60C245-978F-4825-8BFC-8F74F67F2C59' and ChannelPartnerId = 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17')
  BEGIN
	INSERT INTO tChannelPartnerMasterCountryMapping(rowguid, ChannelPartnerId, MasterCountryId) VALUES	(NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '2B60C245-978F-4825-8BFC-8F74F67F2C59'),	(NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '52AC1FB4-5E6F-4246-8AE4-3B9E31189329'),	(NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '6E64D274-5A68-4961-9E09-317C5223253F')	  END

------------------------------ Insert script for  CARVER ------------------------------------
IF NOT EXISTS(SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = '2B60C245-978F-4825-8BFC-8F74F67F2C59' and ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6')
  BEGIN
	INSERT INTO tChannelPartnerMasterCountryMapping(rowguid, ChannelPartnerId, MasterCountryId) VALUES	(NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '2B60C245-978F-4825-8BFC-8F74F67F2C59'),	(NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '52AC1FB4-5E6F-4246-8AE4-3B9E31189329')  END