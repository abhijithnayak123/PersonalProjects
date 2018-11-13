--===========================================================================================
-- Author:			<Divya Boddu>
-- Date Created:	04/13/2015
-- User Story:      AL-208
-- Description:		<Script for insert new updated state list into [tStates]  >
--===========================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tStates]') 
AND type in (N'U'))
BEGIN

IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'AS' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id, Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('B52E82BD-37A1-4DBE-94C4-2AFA980D712D',	'AMERICAN SAMOA',	'AS',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end

IF NOT EXISTS  (select * from dbo.[tStates] where Abbr = 'FM' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id,	Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('8440AF31-58C6-41F2-91A8-8EB4944C453A',	'FEDERATED STATES OF MICRONESIA',	'FM',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end

IF NOT EXISTS  (select 1 from dbo.[tStates] where Abbr = 'GU' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id,	Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('53D3A315-8107-447B-85D5-3694B4922978',	'GUAM',	'GU',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end

IF NOT EXISTS  (select 1 from dbo.[tStates] where Abbr = 'MH' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id,	Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('442D8EEC-F822-4B1A-80F4-9EE9EED036CF',	'MARSHALL ISLANDS',	'MH',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end

IF NOT EXISTS  (select 1 from dbo.[tStates] where Abbr = 'PW' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id,	Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('8B4E87C6-B2FD-4BD9-876F-56A5C38F7461',	'PALAU',	'PW',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end

IF NOT EXISTS  (select 1 from dbo.[tStates] where Abbr = 'PR' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id,	Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('4CD48882-F497-4FD8-8740-E5437A3C6272',	'PUERTO RICO',	'PR',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end

IF NOT EXISTS  (select 1 from dbo.[tStates] where Abbr = 'VI' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id,	Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('E9229F3D-290E-43C6-851E-DF1AB4A1AA64',	'VIRGIN ISLANDS',	'VI',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end

IF NOT EXISTS  (select 1 from dbo.[tStates] where Abbr = 'MP' AND CountryCode =840 )
begin 
  insert into [dbo].[tStates]( id,	Name,	Abbr,	CountryCode,	HASC1,	Region,	Label,	FIPS,	DTCreate,	DTLastMod,	XLicenseId,	TimeZone,	CountryId)
  values('7A92F737-33C5-4F91-8E85-2E285886FCFF',	'NORTHERN MARIANA ISLANDS',	'MP',	840,	NULL,	NULL,	NULL,	NULL,	GETDATE(),	GETDATE(),	NULL,null,	'FEB2478D-A6E8-4117-A77C-2D75379F70EA')
end
end
Go
--=================================================================================================================================

