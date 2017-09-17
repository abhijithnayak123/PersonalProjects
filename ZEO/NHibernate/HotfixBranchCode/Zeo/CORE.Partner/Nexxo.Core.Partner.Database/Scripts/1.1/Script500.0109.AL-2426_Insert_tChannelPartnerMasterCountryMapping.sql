--===========================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <Oct 26 2015>
-- Description:	<Script to insert data into tChannelPartnerMasterCountryMapping table>
-- Jira ID:	    <AL-2426>
--===========================================================================================

-- Synovus 
IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67' AND ChannelPartnerId = 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67', 1)
END

IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3' AND ChannelPartnerId = 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3', 1)
END

IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'A6E9B895-55A5-4649-A1DA-32E1094301DF' AND ChannelPartnerId = 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','A6E9B895-55A5-4649-A1DA-32E1094301DF',1)
END


-- Carver
IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67' AND ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67', 1)
END

IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3' AND ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3', 1)
END

IF NOT EXISTS (SELECT * FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'A6E9B895-55A5-4649-A1DA-32E1094301DF' AND ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', 'A6E9B895-55A5-4649-A1DA-32E1094301DF', 1)
END


-- MGI
IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67' AND ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','C485A3A3-0E28-47F6-8FEC-C87CD06BCA67',1)
END

IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3' AND ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3', 1)
END

IF NOT EXISTS (SELECT * FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'A6E9B895-55A5-4649-A1DA-32E1094301DF' AND ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', 'A6E9B895-55A5-4649-A1DA-32E1094301DF', 1)
END


-- Redstone 
IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67' AND ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67', 1)
END

IF NOT EXISTS (SELECT 1 FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3' AND ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3', 1)
END

IF NOT EXISTS (SELECT * FROM  tChannelPartnerMasterCountryMapping WHERE MasterCountryId = 'A6E9B895-55A5-4649-A1DA-32E1094301DF' AND ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6')
BEGIN
     INSERT INTO [dbo].[tChannelPartnerMasterCountryMapping] ([rowguid], [ChannelPartnerId], [MasterCountryId], [IsActive])
     VALUES (NEWID(), 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', 'A6E9B895-55A5-4649-A1DA-32E1094301DF', 1)
END






