--===========================================================================================
-- Author:			<Divya Boddu>
-- Date Created:	04/16/2015
-- User Story:      AL-208
-- Description:		<Script for inserting  nexxoIdTypeId's along with channelPartnerId's into [tChannelPartnerIDTypeMapping] >
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartnerIDTypeMapping]') 
AND type in (N'U'))
BEGIN
IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='3044275D-3534-47D6-A842-D8712E5D36DC')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','3044275D-3534-47D6-A842-D8712E5D36DC',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='24EF7318-639B-4E29-8EE5-B17CCF76BBE2')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','24EF7318-639B-4E29-8EE5-B17CCF76BBE2',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='54255DA6-B413-41C8-8CDB-E5C15F570FFD')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','54255DA6-B413-41C8-8CDB-E5C15F570FFD',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='9D985F4C-4AE8-44FD-83BE-C421E1814798')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','9D985F4C-4AE8-44FD-83BE-C421E1814798',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='21876D73-1D7E-4BA9-BDEA-C042DE8B46D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','21876D73-1D7E-4BA9-BDEA-C042DE8B46D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='1672CD04-D11C-474D-BB56-A313CED65BF3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','1672CD04-D11C-474D-BB56-A313CED65BF3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='2ADDD854-5061-4008-8F6A-7687CD14E113')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','2ADDD854-5061-4008-8F6A-7687CD14E113',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='69334505-2CD6-4A73-AAC5-823F19810EE5')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','69334505-2CD6-4A73-AAC5-823F19810EE5',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='B6B79B6D-9457-4B37-A641-9E53E758CB38')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','B6B79B6D-9457-4B37-A641-9E53E758CB38',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='B6957D4F-0998-4196-AEC5-069521ACA252')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','B6957D4F-0998-4196-AEC5-069521ACA252',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='51B13263-5C08-4F71-A13F-8052E3BC94D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','51B13263-5C08-4F71-A13F-8052E3BC94D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='2A9D02ED-EC30-4330-8A90-6B286B26A1EE')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','2A9D02ED-EC30-4330-8A90-6B286B26A1EE',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='20C32037-339D-44F0-826B-EA176FAFAF7D')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','20C32037-339D-44F0-826B-EA176FAFAF7D',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='AB03D902-D078-4872-9C2D-EAE8AD71E876')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','AB03D902-D078-4872-9C2D-EAE8AD71E876',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='463593E8-6E9C-428E-8429-7D2842DCE737')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','463593E8-6E9C-428E-8429-7D2842DCE737',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='F3B2E2F5-E774-4014-8E45-9196E5568DF6')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','F3B2E2F5-E774-4014-8E45-9196E5568DF6',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='7ACD018E-9586-4DF6-9C4E-14695AB9A8A7')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','7ACD018E-9586-4DF6-9C4E-14695AB9A8A7',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='1C37BDFF-F772-495B-8FA5-0A3F65424B90')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','1C37BDFF-F772-495B-8FA5-0A3F65424B90',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='D7161A34-A016-4C45-80D5-DD2C49F07AC3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','D7161A34-A016-4C45-80D5-DD2C49F07AC3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='ABF097D7-7AEC-40BE-A305-5E7B8E798046')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','ABF097D7-7AEC-40BE-A305-5E7B8E798046',1)
end
--=======synovus end
IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='3044275D-3534-47D6-A842-D8712E5D36DC')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','3044275D-3534-47D6-A842-D8712E5D36DC',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='24EF7318-639B-4E29-8EE5-B17CCF76BBE2')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','24EF7318-639B-4E29-8EE5-B17CCF76BBE2',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='54255DA6-B413-41C8-8CDB-E5C15F570FFD')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','54255DA6-B413-41C8-8CDB-E5C15F570FFD',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='9D985F4C-4AE8-44FD-83BE-C421E1814798')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','9D985F4C-4AE8-44FD-83BE-C421E1814798',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='21876D73-1D7E-4BA9-BDEA-C042DE8B46D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','21876D73-1D7E-4BA9-BDEA-C042DE8B46D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='1672CD04-D11C-474D-BB56-A313CED65BF3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','1672CD04-D11C-474D-BB56-A313CED65BF3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='2ADDD854-5061-4008-8F6A-7687CD14E113')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','2ADDD854-5061-4008-8F6A-7687CD14E113',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='69334505-2CD6-4A73-AAC5-823F19810EE5')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','69334505-2CD6-4A73-AAC5-823F19810EE5',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='B6B79B6D-9457-4B37-A641-9E53E758CB38')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','B6B79B6D-9457-4B37-A641-9E53E758CB38',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='B6957D4F-0998-4196-AEC5-069521ACA252')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','B6957D4F-0998-4196-AEC5-069521ACA252',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='51B13263-5C08-4F71-A13F-8052E3BC94D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','51B13263-5C08-4F71-A13F-8052E3BC94D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='2A9D02ED-EC30-4330-8A90-6B286B26A1EE')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','2A9D02ED-EC30-4330-8A90-6B286B26A1EE',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='20C32037-339D-44F0-826B-EA176FAFAF7D')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','20C32037-339D-44F0-826B-EA176FAFAF7D',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='AB03D902-D078-4872-9C2D-EAE8AD71E876')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','AB03D902-D078-4872-9C2D-EAE8AD71E876',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='463593E8-6E9C-428E-8429-7D2842DCE737')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','463593E8-6E9C-428E-8429-7D2842DCE737',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='F3B2E2F5-E774-4014-8E45-9196E5568DF6')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','F3B2E2F5-E774-4014-8E45-9196E5568DF6',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='7ACD018E-9586-4DF6-9C4E-14695AB9A8A7')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','7ACD018E-9586-4DF6-9C4E-14695AB9A8A7',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='1C37BDFF-F772-495B-8FA5-0A3F65424B90')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','1C37BDFF-F772-495B-8FA5-0A3F65424B90',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='D7161A34-A016-4C45-80D5-DD2C49F07AC3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','D7161A34-A016-4C45-80D5-DD2C49F07AC3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='ABF097D7-7AEC-40BE-A305-5E7B8E798046')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','ABF097D7-7AEC-40BE-A305-5E7B8E798046',1)
end
--===============tcf end
IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='3044275D-3534-47D6-A842-D8712E5D36DC')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','3044275D-3534-47D6-A842-D8712E5D36DC',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='24EF7318-639B-4E29-8EE5-B17CCF76BBE2')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','24EF7318-639B-4E29-8EE5-B17CCF76BBE2',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='54255DA6-B413-41C8-8CDB-E5C15F570FFD')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','54255DA6-B413-41C8-8CDB-E5C15F570FFD',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='9D985F4C-4AE8-44FD-83BE-C421E1814798')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','9D985F4C-4AE8-44FD-83BE-C421E1814798',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='21876D73-1D7E-4BA9-BDEA-C042DE8B46D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','21876D73-1D7E-4BA9-BDEA-C042DE8B46D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='1672CD04-D11C-474D-BB56-A313CED65BF3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','1672CD04-D11C-474D-BB56-A313CED65BF3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='2ADDD854-5061-4008-8F6A-7687CD14E113')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','2ADDD854-5061-4008-8F6A-7687CD14E113',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='69334505-2CD6-4A73-AAC5-823F19810EE5')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','69334505-2CD6-4A73-AAC5-823F19810EE5',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='B6B79B6D-9457-4B37-A641-9E53E758CB38')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','B6B79B6D-9457-4B37-A641-9E53E758CB38',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='B6957D4F-0998-4196-AEC5-069521ACA252')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','B6957D4F-0998-4196-AEC5-069521ACA252',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='51B13263-5C08-4F71-A13F-8052E3BC94D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','51B13263-5C08-4F71-A13F-8052E3BC94D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='2A9D02ED-EC30-4330-8A90-6B286B26A1EE')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','2A9D02ED-EC30-4330-8A90-6B286B26A1EE',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='20C32037-339D-44F0-826B-EA176FAFAF7D')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','20C32037-339D-44F0-826B-EA176FAFAF7D',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='AB03D902-D078-4872-9C2D-EAE8AD71E876')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','AB03D902-D078-4872-9C2D-EAE8AD71E876',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='463593E8-6E9C-428E-8429-7D2842DCE737')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','463593E8-6E9C-428E-8429-7D2842DCE737',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='F3B2E2F5-E774-4014-8E45-9196E5568DF6')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','F3B2E2F5-E774-4014-8E45-9196E5568DF6',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='7ACD018E-9586-4DF6-9C4E-14695AB9A8A7')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','7ACD018E-9586-4DF6-9C4E-14695AB9A8A7',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='1C37BDFF-F772-495B-8FA5-0A3F65424B90')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','1C37BDFF-F772-495B-8FA5-0A3F65424B90',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='D7161A34-A016-4C45-80D5-DD2C49F07AC3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','D7161A34-A016-4C45-80D5-DD2C49F07AC3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='ABF097D7-7AEC-40BE-A305-5E7B8E798046')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','ABF097D7-7AEC-40BE-A305-5E7B8E798046',1)
end
--=======carver end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='3044275D-3534-47D6-A842-D8712E5D36DC')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','3044275D-3534-47D6-A842-D8712E5D36DC',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='24EF7318-639B-4E29-8EE5-B17CCF76BBE2')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','24EF7318-639B-4E29-8EE5-B17CCF76BBE2',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='54255DA6-B413-41C8-8CDB-E5C15F570FFD')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','54255DA6-B413-41C8-8CDB-E5C15F570FFD',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='9D985F4C-4AE8-44FD-83BE-C421E1814798')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','9D985F4C-4AE8-44FD-83BE-C421E1814798',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='21876D73-1D7E-4BA9-BDEA-C042DE8B46D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','21876D73-1D7E-4BA9-BDEA-C042DE8B46D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='1672CD04-D11C-474D-BB56-A313CED65BF3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','1672CD04-D11C-474D-BB56-A313CED65BF3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='2ADDD854-5061-4008-8F6A-7687CD14E113')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','2ADDD854-5061-4008-8F6A-7687CD14E113',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='69334505-2CD6-4A73-AAC5-823F19810EE5')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','69334505-2CD6-4A73-AAC5-823F19810EE5',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='B6B79B6D-9457-4B37-A641-9E53E758CB38')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','B6B79B6D-9457-4B37-A641-9E53E758CB38',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='B6957D4F-0998-4196-AEC5-069521ACA252')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','B6957D4F-0998-4196-AEC5-069521ACA252',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='51B13263-5C08-4F71-A13F-8052E3BC94D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','51B13263-5C08-4F71-A13F-8052E3BC94D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='2A9D02ED-EC30-4330-8A90-6B286B26A1EE')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','2A9D02ED-EC30-4330-8A90-6B286B26A1EE',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='20C32037-339D-44F0-826B-EA176FAFAF7D')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','20C32037-339D-44F0-826B-EA176FAFAF7D',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='AB03D902-D078-4872-9C2D-EAE8AD71E876')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','AB03D902-D078-4872-9C2D-EAE8AD71E876',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='463593E8-6E9C-428E-8429-7D2842DCE737')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','463593E8-6E9C-428E-8429-7D2842DCE737',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='F3B2E2F5-E774-4014-8E45-9196E5568DF6')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','F3B2E2F5-E774-4014-8E45-9196E5568DF6',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='7ACD018E-9586-4DF6-9C4E-14695AB9A8A7')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','7ACD018E-9586-4DF6-9C4E-14695AB9A8A7',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='1C37BDFF-F772-495B-8FA5-0A3F65424B90')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','1C37BDFF-F772-495B-8FA5-0A3F65424B90',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='D7161A34-A016-4C45-80D5-DD2C49F07AC3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','D7161A34-A016-4C45-80D5-DD2C49F07AC3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='ABF097D7-7AEC-40BE-A305-5E7B8E798046')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','ABF097D7-7AEC-40BE-A305-5E7B8E798046',1)
end

--================MGI end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='3044275D-3534-47D6-A842-D8712E5D36DC')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','3044275D-3534-47D6-A842-D8712E5D36DC',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='24EF7318-639B-4E29-8EE5-B17CCF76BBE2')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','24EF7318-639B-4E29-8EE5-B17CCF76BBE2',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='54255DA6-B413-41C8-8CDB-E5C15F570FFD')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','54255DA6-B413-41C8-8CDB-E5C15F570FFD',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='9D985F4C-4AE8-44FD-83BE-C421E1814798')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','9D985F4C-4AE8-44FD-83BE-C421E1814798',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='21876D73-1D7E-4BA9-BDEA-C042DE8B46D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','21876D73-1D7E-4BA9-BDEA-C042DE8B46D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='1672CD04-D11C-474D-BB56-A313CED65BF3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','1672CD04-D11C-474D-BB56-A313CED65BF3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='2ADDD854-5061-4008-8F6A-7687CD14E113')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','2ADDD854-5061-4008-8F6A-7687CD14E113',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='69334505-2CD6-4A73-AAC5-823F19810EE5')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','69334505-2CD6-4A73-AAC5-823F19810EE5',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='B6B79B6D-9457-4B37-A641-9E53E758CB38')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','B6B79B6D-9457-4B37-A641-9E53E758CB38',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='B6957D4F-0998-4196-AEC5-069521ACA252')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','B6957D4F-0998-4196-AEC5-069521ACA252',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='51B13263-5C08-4F71-A13F-8052E3BC94D9')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','51B13263-5C08-4F71-A13F-8052E3BC94D9',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='2A9D02ED-EC30-4330-8A90-6B286B26A1EE')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','2A9D02ED-EC30-4330-8A90-6B286B26A1EE',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='20C32037-339D-44F0-826B-EA176FAFAF7D')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','20C32037-339D-44F0-826B-EA176FAFAF7D',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='AB03D902-D078-4872-9C2D-EAE8AD71E876')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','AB03D902-D078-4872-9C2D-EAE8AD71E876',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='463593E8-6E9C-428E-8429-7D2842DCE737')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','463593E8-6E9C-428E-8429-7D2842DCE737',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='F3B2E2F5-E774-4014-8E45-9196E5568DF6')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','F3B2E2F5-E774-4014-8E45-9196E5568DF6',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='7ACD018E-9586-4DF6-9C4E-14695AB9A8A7')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','7ACD018E-9586-4DF6-9C4E-14695AB9A8A7',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='1C37BDFF-F772-495B-8FA5-0A3F65424B90')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','1C37BDFF-F772-495B-8FA5-0A3F65424B90',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='D7161A34-A016-4C45-80D5-DD2C49F07AC3')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','D7161A34-A016-4C45-80D5-DD2C49F07AC3',1)
end

IF NOT EXISTS  (select * from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='C5725730-0D46-4429-B8F1-6B75C1B274F2' and  NexxoIdTypeId='ABF097D7-7AEC-40BE-A305-5E7B8E798046')
begin
INSERT INTO [dbo].[tChannelPartnerIDTypeMapping](rowguid ,ChannelPartnerId, NexxoIdTypeId , IsActive)
VALUES(NEWID(),'C5725730-0D46-4429-B8F1-6B75C1B274F2','ABF097D7-7AEC-40BE-A305-5E7B8E798046',1)
end
--====Centris records end
End
