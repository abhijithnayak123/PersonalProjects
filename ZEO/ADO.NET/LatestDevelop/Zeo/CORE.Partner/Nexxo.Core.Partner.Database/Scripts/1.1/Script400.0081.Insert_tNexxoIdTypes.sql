--===========================================================================================
-- Author:			<Divya Boddu>
-- Date Created:	04/15/2015
-- User Story:      AL-208
-- Description:		<Script for insert new updated state(U.S states) list  into [tNexxoIdTypes] >
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tNexxoIdTypes]') 
AND type in (N'U'))
BEGIN
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'AMERICAN SAMOA' AND NAME='DRIVER''S LICENSE' )
begin
  INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
  VALUES('3044275D-3534-47D6-A842-D8712E5D36DC',466,	'DRIVER''S LICENSE',	'^\w{4,15}$' ,	1,	'UNITED STATES' ,	'AMERICAN SAMOA','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','B52E82BD-37A1-4DBE-94C4-2AFA980D712D' ,1)
end
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'AMERICAN SAMOA' AND NAME='U.S. STATE IDENTITY CARD' )
begin
  INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
  VALUES('24EF7318-639B-4E29-8EE5-B17CCF76BBE2',465,	'U.S. STATE IDENTITY CARD',	'^\w{4,15}$' ,	1,	'UNITED STATES' ,	'AMERICAN SAMOA','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','B52E82BD-37A1-4DBE-94C4-2AFA980D712D' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'ARMED FORCES AMERICAS' AND NAME='DRIVER''S LICENSE' )
begin
  INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
  VALUES('54255DA6-B413-41C8-8CDB-E5C15F570FFD',467,	'DRIVER''S LICENSE',	'^\w{4,15}$' ,	1,	'UNITED STATES' ,	'ARMED FORCES AMERICAS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','AD5E6622-BD6B-4C99-8568-9CB9F2789469' ,1)
end
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'ARMED FORCES AMERICAS' AND NAME='U.S. STATE IDENTITY CARD' )
begin
 INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
 VALUES('9D985F4C-4AE8-44FD-83BE-C421E1814798',468,	'U.S. STATE IDENTITY CARD',	'^\w{4,15}$' ,	1,	'UNITED STATES' ,	'ARMED FORCES AMERICAS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','AD5E6622-BD6B-4C99-8568-9CB9F2789469' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'ARMED FORCES EUROPE' AND NAME='DRIVER''S LICENSE' )
begin
 INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
 VALUES('21876D73-1D7E-4BA9-BDEA-C042DE8B46D9',469,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'ARMED FORCES EUROPE','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','FA548A81-9A7D-4F38-BC05-6B9AA1A72577' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'ARMED FORCES EUROPE' AND NAME='U.S. STATE IDENTITY CARD' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('1672CD04-D11C-474D-BB56-A313CED65BF3',470,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'ARMED FORCES EUROPE','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','FA548A81-9A7D-4F38-BC05-6B9AA1A72577' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'ARMED FORCES PACIFIC' AND NAME='DRIVER''S LICENSE' )
begin
  INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
  VALUES('2ADDD854-5061-4008-8F6A-7687CD14E113',471,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'ARMED FORCES PACIFIC','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','2209294B-FFBE-4112-8F73-0D9D54DC8345' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'ARMED FORCES PACIFIC' AND NAME='U.S. STATE IDENTITY CARD' )
begin
  INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
  VALUES('69334505-2CD6-4A73-AAC5-823F19810EE5',472,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'ARMED FORCES PACIFIC','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','2209294B-FFBE-4112-8F73-0D9D54DC8345' ,1)
end 

IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'FEDERATED STATES OF MICRONESIA' AND NAME='DRIVER''S LICENSE' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('B6B79B6D-9457-4B37-A641-9E53E758CB38',473,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'FEDERATED STATES OF MICRONESIA','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','8440AF31-58C6-41F2-91A8-8EB4944C453A' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'FEDERATED STATES OF MICRONESIA' AND NAME='U.S. STATE IDENTITY CARD' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('491CCCA4-01EF-402A-8A2E-E2BD0F41E0C3',474,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'FEDERATED STATES OF MICRONESIA','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','8440AF31-58C6-41F2-91A8-8EB4944C453A' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'GUAM' AND NAME='DRIVER''S LICENSE' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('B6957D4F-0998-4196-AEC5-069521ACA252',475,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'GUAM','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','53D3A315-8107-447B-85D5-3694B4922978' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'GUAM' AND NAME='U.S. STATE IDENTITY CARD' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('51B13263-5C08-4F71-A13F-8052E3BC94D9',476,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'GUAM','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','53D3A315-8107-447B-85D5-3694B4922978' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'MARSHALL ISLANDS' AND NAME='DRIVER''S LICENSE' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('2A9D02ED-EC30-4330-8A90-6B286B26A1EE',477,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'MARSHALL ISLANDS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','442D8EEC-F822-4B1A-80F4-9EE9EED036CF' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'MARSHALL ISLANDS' AND NAME='U.S. STATE IDENTITY CARD' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('C9D5ADDC-B169-4E25-A74F-BF33F8CB2C99',478,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'MARSHALL ISLANDS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','442D8EEC-F822-4B1A-80F4-9EE9EED036CF' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'NORTHERN MARIANA ISLANDS' AND NAME='DRIVER''S LICENSE' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('20C32037-339D-44F0-826B-EA176FAFAF7D',479,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'NORTHERN MARIANA ISLANDS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','7A92F737-33C5-4F91-8E85-2E285886FCFF' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'NORTHERN MARIANA ISLANDS' AND NAME='U.S. STATE IDENTITY CARD' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('AB03D902-D078-4872-9C2D-EAE8AD71E876',480,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'NORTHERN MARIANA ISLANDS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','7A92F737-33C5-4F91-8E85-2E285886FCFF' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'PALAU' AND NAME='DRIVER''S LICENSE' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('463593E8-6E9C-428E-8429-7D2842DCE737',481,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'PALAU','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','8B4E87C6-B2FD-4BD9-876F-56A5C38F7461' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'PALAU' AND NAME='U.S. STATE IDENTITY CARD' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('F3B2E2F5-E774-4014-8E45-9196E5568DF6',482,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'PALAU','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','8B4E87C6-B2FD-4BD9-876F-56A5C38F7461' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'VIRGIN ISLANDS' AND NAME='DRIVER''S LICENSE' )
begin
 INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
 VALUES('7ACD018E-9586-4DF6-9C4E-14695AB9A8A7',483,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'VIRGIN ISLANDS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','E9229F3D-290E-43C6-851E-DF1AB4A1AA64' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'VIRGIN ISLANDS' AND NAME='U.S. STATE IDENTITY CARD' )
begin
   INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
   VALUES('1C37BDFF-F772-495B-8FA5-0A3F65424B90',484,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'VIRGIN ISLANDS','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','E9229F3D-290E-43C6-851E-DF1AB4A1AA64' ,1)
end
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'PUERTO RICO' AND NAME='DRIVER''S LICENSE' )
begin
INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
VALUES('D7161A34-A016-4C45-80D5-DD2C49F07AC3',485,	'DRIVER''S LICENSE', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'PUERTO RICO','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','4CD48882-F497-4FD8-8740-E5437A3C6272' ,1)
end 
IF NOT EXISTS  (select * from [dbo].[tNexxoIdTypes] where State = 'PUERTO RICO' AND NAME='U.S. STATE IDENTITY CARD' )
begin
   INSERT INTO [dbo].[tNexxoIdTypes] (rowguid,	Id,	Name,	Mask,	HasExpirationDate,	Country, State,	CountryId,	StateId,	IsActive) 
   VALUES('ABF097D7-7AEC-40BE-A305-5E7B8E798046',486,	'U.S. STATE IDENTITY CARD', '^\w{4,15}$' ,	1,	'UNITED STATES' ,	'PUERTO RICO','32D2E289-3319-49B0-B630-FB0A1E5FCEC0','4CD48882-F497-4FD8-8740-E5437A3C6272' ,1)
end
End
go
