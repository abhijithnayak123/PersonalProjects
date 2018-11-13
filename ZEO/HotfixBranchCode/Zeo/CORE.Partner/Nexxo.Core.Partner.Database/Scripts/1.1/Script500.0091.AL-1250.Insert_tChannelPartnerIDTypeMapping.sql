
--===========================================================================================
-- Author:			<Manikandan Govindraj>
-- Date Created:	01/09/2015
-- User Story:      AL-1250
-- Description:		<Script for inserting channel partner mapping for Canada and ID types (TCF,Synovus,Carver,MGI,Redstone>
--===========================================================================================

-- Channel Partner: Synovus

IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='86FC16BD-8C46-441E-8C0D-2C3804A8EF59')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','86FC16BD-8C46-441E-8C0D-2C3804A8EF59',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='3289F313-3B2F-45C8-B490-29F8C6B13F81')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','3289F313-3B2F-45C8-B490-29F8C6B13F81',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='C252BF05-984F-4F61-9C9B-E32FFCE860A5')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','C252BF05-984F-4F61-9C9B-E32FFCE860A5',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='47431F97-022B-4395-B492-ECB2D2C70B15')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','47431F97-022B-4395-B492-ECB2D2C70B15',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='70CE30C9-B2ED-4667-AEBA-FE0A5588D720')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','70CE30C9-B2ED-4667-AEBA-FE0A5588D720',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='4076CFEE-F7AE-4183-BA48-3CDE88230940')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','4076CFEE-F7AE-4183-BA48-3CDE88230940',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='A79B07EB-2887-4C4A-8F97-A734DA313621')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','A79B07EB-2887-4C4A-8F97-A734DA313621',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='364CC0C5-3984-4854-81A5-8DC5CFC95A70')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','364CC0C5-3984-4854-81A5-8DC5CFC95A70',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='727B129B-BADB-4370-982D-A3D6730A8769')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','727B129B-BADB-4370-982D-A3D6730A8769',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='04579BDB-4A46-4788-A1FA-E28E135E93E4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','04579BDB-4A46-4788-A1FA-E28E135E93E4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='F916FA84-C5C1-4439-BD1C-871DA08495D4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','F916FA84-C5C1-4439-BD1C-871DA08495D4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='CAFD9513-6FD6-4F3B-8833-83A7915ED019')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','CAFD9513-6FD6-4F3B-8833-83A7915ED019',1)
END 
		   
		
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='E9B2F359-5AF0-47D3-943E-EA53059CBA44')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','E9B2F359-5AF0-47D3-943E-EA53059CBA44',1)
END	  
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='011A5FE3-95ED-430E-B806-BAA0577BAD38')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','011A5FE3-95ED-430E-B806-BAA0577BAD38',1)
END	  
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='F26E08BE-842A-4153-B5E0-015BF845FAF8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','F26E08BE-842A-4153-B5E0-015BF845FAF8',1)
END	 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='7EBD3CE8-10B4-4255-A091-23552529FDAA')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','7EBD3CE8-10B4-4255-A091-23552529FDAA',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='580EC2EA-8E4D-4AEC-981A-C8DC7691754A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','580EC2EA-8E4D-4AEC-981A-C8DC7691754A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='5A46AEC7-2BF6-49C8-8047-148464D828FF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','5A46AEC7-2BF6-49C8-8047-148464D828FF',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='437F8F5F-1F02-4EDF-9EC3-5879C07809F8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','437F8F5F-1F02-4EDF-9EC3-5879C07809F8',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='DDC70954-60D1-4DCD-BA3B-FBADCEFB334A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','DDC70954-60D1-4DCD-BA3B-FBADCEFB334A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='7BFB407E-BF04-4930-A39F-10E2D9721482')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','7BFB407E-BF04-4930-A39F-10E2D9721482',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='58FBC2FD-EA56-4F34-B814-541453BC06A7')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','58FBC2FD-EA56-4F34-B814-541453BC06A7',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='009CEEDA-28AB-4654-90EF-A7338ECF9AA4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','009CEEDA-28AB-4654-90EF-A7338ECF9AA4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and  NexxoIdTypeId='F3B7621B-5EC6-407D-8256-2ADEE9434504')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17','F3B7621B-5EC6-407D-8256-2ADEE9434504',1)
END
		  
-- Channel Partner: TCF
  	  
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='86FC16BD-8C46-441E-8C0D-2C3804A8EF59')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','86FC16BD-8C46-441E-8C0D-2C3804A8EF59',1)
END		   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='3289F313-3B2F-45C8-B490-29F8C6B13F81')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','3289F313-3B2F-45C8-B490-29F8C6B13F81',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='C252BF05-984F-4F61-9C9B-E32FFCE860A5')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','C252BF05-984F-4F61-9C9B-E32FFCE860A5',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='47431F97-022B-4395-B492-ECB2D2C70B15')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','47431F97-022B-4395-B492-ECB2D2C70B15',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='70CE30C9-B2ED-4667-AEBA-FE0A5588D720')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','70CE30C9-B2ED-4667-AEBA-FE0A5588D720',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='4076CFEE-F7AE-4183-BA48-3CDE88230940')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','4076CFEE-F7AE-4183-BA48-3CDE88230940',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='A79B07EB-2887-4C4A-8F97-A734DA313621')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','A79B07EB-2887-4C4A-8F97-A734DA313621',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='364CC0C5-3984-4854-81A5-8DC5CFC95A70')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','364CC0C5-3984-4854-81A5-8DC5CFC95A70',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='727B129B-BADB-4370-982D-A3D6730A8769')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','727B129B-BADB-4370-982D-A3D6730A8769',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='04579BDB-4A46-4788-A1FA-E28E135E93E4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','04579BDB-4A46-4788-A1FA-E28E135E93E4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='F916FA84-C5C1-4439-BD1C-871DA08495D4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','F916FA84-C5C1-4439-BD1C-871DA08495D4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='CAFD9513-6FD6-4F3B-8833-83A7915ED019')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','CAFD9513-6FD6-4F3B-8833-83A7915ED019',1)
END
		   
		   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='E9B2F359-5AF0-47D3-943E-EA53059CBA44')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','E9B2F359-5AF0-47D3-943E-EA53059CBA44',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='011A5FE3-95ED-430E-B806-BAA0577BAD38')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','011A5FE3-95ED-430E-B806-BAA0577BAD38',1)
END	   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='F26E08BE-842A-4153-B5E0-015BF845FAF8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','F26E08BE-842A-4153-B5E0-015BF845FAF8',1)
END	 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='7EBD3CE8-10B4-4255-A091-23552529FDAA')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','7EBD3CE8-10B4-4255-A091-23552529FDAA',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='580EC2EA-8E4D-4AEC-981A-C8DC7691754A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','580EC2EA-8E4D-4AEC-981A-C8DC7691754A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='5A46AEC7-2BF6-49C8-8047-148464D828FF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','5A46AEC7-2BF6-49C8-8047-148464D828FF',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='437F8F5F-1F02-4EDF-9EC3-5879C07809F8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','437F8F5F-1F02-4EDF-9EC3-5879C07809F8',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='DDC70954-60D1-4DCD-BA3B-FBADCEFB334A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','DDC70954-60D1-4DCD-BA3B-FBADCEFB334A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='7BFB407E-BF04-4930-A39F-10E2D9721482')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','7BFB407E-BF04-4930-A39F-10E2D9721482',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='58FBC2FD-EA56-4F34-B814-541453BC06A7')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','58FBC2FD-EA56-4F34-B814-541453BC06A7',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='009CEEDA-28AB-4654-90EF-A7338ECF9AA4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','009CEEDA-28AB-4654-90EF-A7338ECF9AA4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='6D7E785F-7BDD-42C8-BC49-44536A1885FC' and  NexxoIdTypeId='F3B7621B-5EC6-407D-8256-2ADEE9434504')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'6D7E785F-7BDD-42C8-BC49-44536A1885FC','F3B7621B-5EC6-407D-8256-2ADEE9434504',1)
END

-- Channel Partner: CARVER		 
		  
		 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='86FC16BD-8C46-441E-8C0D-2C3804A8EF59')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','86FC16BD-8C46-441E-8C0D-2C3804A8EF59',1)
END		
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='3289F313-3B2F-45C8-B490-29F8C6B13F81')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','3289F313-3B2F-45C8-B490-29F8C6B13F81',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='C252BF05-984F-4F61-9C9B-E32FFCE860A5')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','C252BF05-984F-4F61-9C9B-E32FFCE860A5',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='47431F97-022B-4395-B492-ECB2D2C70B15')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','47431F97-022B-4395-B492-ECB2D2C70B15',1)
END		
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3',1)
END		   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='70CE30C9-B2ED-4667-AEBA-FE0A5588D720')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','70CE30C9-B2ED-4667-AEBA-FE0A5588D720',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='4076CFEE-F7AE-4183-BA48-3CDE88230940')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','4076CFEE-F7AE-4183-BA48-3CDE88230940',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='A79B07EB-2887-4C4A-8F97-A734DA313621')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','A79B07EB-2887-4C4A-8F97-A734DA313621',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='364CC0C5-3984-4854-81A5-8DC5CFC95A70')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','364CC0C5-3984-4854-81A5-8DC5CFC95A70',1)
END		
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='727B129B-BADB-4370-982D-A3D6730A8769')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','727B129B-BADB-4370-982D-A3D6730A8769',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='04579BDB-4A46-4788-A1FA-E28E135E93E4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','04579BDB-4A46-4788-A1FA-E28E135E93E4',1)
END		
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='F916FA84-C5C1-4439-BD1C-871DA08495D4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','F916FA84-C5C1-4439-BD1C-871DA08495D4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='CAFD9513-6FD6-4F3B-8833-83A7915ED019')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','CAFD9513-6FD6-4F3B-8833-83A7915ED019',1)
END			


IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='E9B2F359-5AF0-47D3-943E-EA53059CBA44')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','E9B2F359-5AF0-47D3-943E-EA53059CBA44',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='011A5FE3-95ED-430E-B806-BAA0577BAD38')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','011A5FE3-95ED-430E-B806-BAA0577BAD38',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='F26E08BE-842A-4153-B5E0-015BF845FAF8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','F26E08BE-842A-4153-B5E0-015BF845FAF8',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='7EBD3CE8-10B4-4255-A091-23552529FDAA')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','7EBD3CE8-10B4-4255-A091-23552529FDAA',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='580EC2EA-8E4D-4AEC-981A-C8DC7691754A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','580EC2EA-8E4D-4AEC-981A-C8DC7691754A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='5A46AEC7-2BF6-49C8-8047-148464D828FF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','5A46AEC7-2BF6-49C8-8047-148464D828FF',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='437F8F5F-1F02-4EDF-9EC3-5879C07809F8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','437F8F5F-1F02-4EDF-9EC3-5879C07809F8',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='DDC70954-60D1-4DCD-BA3B-FBADCEFB334A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','DDC70954-60D1-4DCD-BA3B-FBADCEFB334A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='7BFB407E-BF04-4930-A39F-10E2D9721482')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','7BFB407E-BF04-4930-A39F-10E2D9721482',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='58FBC2FD-EA56-4F34-B814-541453BC06A7')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','58FBC2FD-EA56-4F34-B814-541453BC06A7',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='009CEEDA-28AB-4654-90EF-A7338ECF9AA4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','009CEEDA-28AB-4654-90EF-A7338ECF9AA4',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and  NexxoIdTypeId='F3B7621B-5EC6-407D-8256-2ADEE9434504')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','F3B7621B-5EC6-407D-8256-2ADEE9434504',1)
END	

-- Channel Partner: MGI


IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='86FC16BD-8C46-441E-8C0D-2C3804A8EF59')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','86FC16BD-8C46-441E-8C0D-2C3804A8EF59',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='3289F313-3B2F-45C8-B490-29F8C6B13F81')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','3289F313-3B2F-45C8-B490-29F8C6B13F81',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='C252BF05-984F-4F61-9C9B-E32FFCE860A5')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','C252BF05-984F-4F61-9C9B-E32FFCE860A5',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='47431F97-022B-4395-B492-ECB2D2C70B15')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','47431F97-022B-4395-B492-ECB2D2C70B15',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='70CE30C9-B2ED-4667-AEBA-FE0A5588D720')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','70CE30C9-B2ED-4667-AEBA-FE0A5588D720',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='4076CFEE-F7AE-4183-BA48-3CDE88230940')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','4076CFEE-F7AE-4183-BA48-3CDE88230940',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='A79B07EB-2887-4C4A-8F97-A734DA313621')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','A79B07EB-2887-4C4A-8F97-A734DA313621',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='364CC0C5-3984-4854-81A5-8DC5CFC95A70')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','364CC0C5-3984-4854-81A5-8DC5CFC95A70',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='727B129B-BADB-4370-982D-A3D6730A8769')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','727B129B-BADB-4370-982D-A3D6730A8769',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='04579BDB-4A46-4788-A1FA-E28E135E93E4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','04579BDB-4A46-4788-A1FA-E28E135E93E4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='F916FA84-C5C1-4439-BD1C-871DA08495D4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','F916FA84-C5C1-4439-BD1C-871DA08495D4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='CAFD9513-6FD6-4F3B-8833-83A7915ED019')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','CAFD9513-6FD6-4F3B-8833-83A7915ED019',1)
END
		   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='E9B2F359-5AF0-47D3-943E-EA53059CBA44')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','E9B2F359-5AF0-47D3-943E-EA53059CBA44',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='011A5FE3-95ED-430E-B806-BAA0577BAD38')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','011A5FE3-95ED-430E-B806-BAA0577BAD38',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='F26E08BE-842A-4153-B5E0-015BF845FAF8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','F26E08BE-842A-4153-B5E0-015BF845FAF8',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='7EBD3CE8-10B4-4255-A091-23552529FDAA')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','7EBD3CE8-10B4-4255-A091-23552529FDAA',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='580EC2EA-8E4D-4AEC-981A-C8DC7691754A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','580EC2EA-8E4D-4AEC-981A-C8DC7691754A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='5A46AEC7-2BF6-49C8-8047-148464D828FF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','5A46AEC7-2BF6-49C8-8047-148464D828FF',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='437F8F5F-1F02-4EDF-9EC3-5879C07809F8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','437F8F5F-1F02-4EDF-9EC3-5879C07809F8',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='DDC70954-60D1-4DCD-BA3B-FBADCEFB334A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','DDC70954-60D1-4DCD-BA3B-FBADCEFB334A',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='7BFB407E-BF04-4930-A39F-10E2D9721482')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','7BFB407E-BF04-4930-A39F-10E2D9721482',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='58FBC2FD-EA56-4F34-B814-541453BC06A7')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','58FBC2FD-EA56-4F34-B814-541453BC06A7',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='009CEEDA-28AB-4654-90EF-A7338ECF9AA4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','009CEEDA-28AB-4654-90EF-A7338ECF9AA4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='10F2865B-DBC5-4A0B-983C-62E0A0574354' and  NexxoIdTypeId='F3B7621B-5EC6-407D-8256-2ADEE9434504')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'10F2865B-DBC5-4A0B-983C-62E0A0574354','F3B7621B-5EC6-407D-8256-2ADEE9434504',1)
END
		   
 --Channel Partner: REDSTONE
		   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='86FC16BD-8C46-441E-8C0D-2C3804A8EF59')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','86FC16BD-8C46-441E-8C0D-2C3804A8EF59',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='3289F313-3B2F-45C8-B490-29F8C6B13F81')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','3289F313-3B2F-45C8-B490-29F8C6B13F81',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='C252BF05-984F-4F61-9C9B-E32FFCE860A5')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','C252BF05-984F-4F61-9C9B-E32FFCE860A5',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='47431F97-022B-4395-B492-ECB2D2C70B15')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','47431F97-022B-4395-B492-ECB2D2C70B15',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','E565E46A-EBE7-4B8C-89B8-D8ADD42B53D3',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='70CE30C9-B2ED-4667-AEBA-FE0A5588D720')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','70CE30C9-B2ED-4667-AEBA-FE0A5588D720',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='4076CFEE-F7AE-4183-BA48-3CDE88230940')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','4076CFEE-F7AE-4183-BA48-3CDE88230940',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='A79B07EB-2887-4C4A-8F97-A734DA313621')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','A79B07EB-2887-4C4A-8F97-A734DA313621',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='364CC0C5-3984-4854-81A5-8DC5CFC95A70')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','364CC0C5-3984-4854-81A5-8DC5CFC95A70',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='727B129B-BADB-4370-982D-A3D6730A8769')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','727B129B-BADB-4370-982D-A3D6730A8769',1)
END	   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='04579BDB-4A46-4788-A1FA-E28E135E93E4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','04579BDB-4A46-4788-A1FA-E28E135E93E4',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='F916FA84-C5C1-4439-BD1C-871DA08495D4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','F916FA84-C5C1-4439-BD1C-871DA08495D4',1)
END	   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='CAFD9513-6FD6-4F3B-8833-83A7915ED019')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','CAFD9513-6FD6-4F3B-8833-83A7915ED019',1)
END	    


IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='E9B2F359-5AF0-47D3-943E-EA53059CBA44')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','E9B2F359-5AF0-47D3-943E-EA53059CBA44',1)
END	
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='011A5FE3-95ED-430E-B806-BAA0577BAD38')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','011A5FE3-95ED-430E-B806-BAA0577BAD38',1)
END	 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='F26E08BE-842A-4153-B5E0-015BF845FAF8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','F26E08BE-842A-4153-B5E0-015BF845FAF8',1)
END	 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='7EBD3CE8-10B4-4255-A091-23552529FDAA')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','7EBD3CE8-10B4-4255-A091-23552529FDAA',1)
END	  
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='580EC2EA-8E4D-4AEC-981A-C8DC7691754A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','580EC2EA-8E4D-4AEC-981A-C8DC7691754A',1)
END	   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='5A46AEC7-2BF6-49C8-8047-148464D828FF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','5A46AEC7-2BF6-49C8-8047-148464D828FF',1)
END	    
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='437F8F5F-1F02-4EDF-9EC3-5879C07809F8')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','437F8F5F-1F02-4EDF-9EC3-5879C07809F8',1)
END	   
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='DDC70954-60D1-4DCD-BA3B-FBADCEFB334A')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','DDC70954-60D1-4DCD-BA3B-FBADCEFB334A',1)
END	 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','0E9DD564-E84D-4DC1-9F7A-5D8175ED7ADF',1)
END	 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='7BFB407E-BF04-4930-A39F-10E2D9721482')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','7BFB407E-BF04-4930-A39F-10E2D9721482',1)
END	 
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='58FBC2FD-EA56-4F34-B814-541453BC06A7')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES  (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','58FBC2FD-EA56-4F34-B814-541453BC06A7',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='009CEEDA-28AB-4654-90EF-A7338ECF9AA4')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','009CEEDA-28AB-4654-90EF-A7338ECF9AA4',1)
END
IF NOT EXISTS  (select 1 from [dbo].[tChannelPartnerIDTypeMapping] where ChannelPartnerId='F8365E3B-FDD5-439A-BE9E-9B4444E17ED6' and  NexxoIdTypeId='F3B7621B-5EC6-407D-8256-2ADEE9434504')
BEGIN
	INSERT INTO [dbo].[tChannelPartnerIDTypeMapping] ([rowguid],[ChannelPartnerId],[NexxoIdTypeId],[IsActive])
	VALUES (NEWID(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6','F3B7621B-5EC6-407D-8256-2ADEE9434504',1)
END
		   
		
		   
		   
		  
		   
		   













