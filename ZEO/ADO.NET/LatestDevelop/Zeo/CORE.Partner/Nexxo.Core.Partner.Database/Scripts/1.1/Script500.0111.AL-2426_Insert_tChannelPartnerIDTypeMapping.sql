--===========================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <Oct 26 2015>
-- Description:	<Script for inserting channel partner ID type mapping for Countries(EAST TIMOR,PALESTINIAN TERRITORY,REUNION) and ID type(PASSPORT) 
--               for channel partners (Carver,MGI,Redstone)>
-- Jira ID:	    <AL-2426>
--===========================================================================================


-- CARVER 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6' AND  NexxoIdTypeId = '0A927247-D011-4611-BF3B-7C3EF30E15C4')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '0A927247-D011-4611-BF3B-7C3EF30E15C4', 1)
END

IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6' AND  NexxoIdTypeId = 'F2881A88-E178-4B9D-84E0-51295A65557A')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', 'F2881A88-E178-4B9D-84E0-51295A65557A', 1)
END

IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6' AND  NexxoIdTypeId = '41F25534-4B28-4387-8530-497AFA6456D2')
BEGIN
 	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '41F25534-4B28-4387-8530-497AFA6456D2', 1)
END

-- MGI 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354' AND  NexxoIdTypeId = '0A927247-D011-4611-BF3B-7C3EF30E15C4')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', '0A927247-D011-4611-BF3B-7C3EF30E15C4',1 )
END

IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354' AND  NexxoIdTypeId = 'F2881A88-E178-4B9D-84E0-51295A65557A')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', 'F2881A88-E178-4B9D-84E0-51295A65557A', 1)
END

IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354' AND  NexxoIdTypeId = '41F25534-4B28-4387-8530-497AFA6456D2')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', '41F25534-4B28-4387-8530-497AFA6456D2', 1)
END

-- REDSTONE 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' AND  NexxoIdTypeId = '0A927247-D011-4611-BF3B-7C3EF30E15C4')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', '0A927247-D011-4611-BF3B-7C3EF30E15C4', 1)
END

IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' AND  NexxoIdTypeId = 'F2881A88-E178-4B9D-84E0-51295A65557A')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
	 VALUES (NEWID(), 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', 'F2881A88-E178-4B9D-84E0-51295A65557A', 1)
END

IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' AND  NexxoIdTypeId = '41F25534-4B28-4387-8530-497AFA6456D2')
BEGIN
	 INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid], [ChannelPartnerId], [NexxoIdTypeId], [IsActive])
 	 VALUES (NEWID(), 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', '41F25534-4B28-4387-8530-497AFA6456D2', 1)
END


