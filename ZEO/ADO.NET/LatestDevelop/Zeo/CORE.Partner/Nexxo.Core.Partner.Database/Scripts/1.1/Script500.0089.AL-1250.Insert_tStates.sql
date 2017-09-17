--===========================================================================================
-- Author:			<Manikandan Govindraj>
-- Date Created:	01/09/2015
-- User Story:      AL-1250
-- Description:		<Script for inserting States for Canada country>
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tStates]') 
AND type in (N'U'))
BEGIN

	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'AB' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'9e81d4a9-71d7-482a-a96a-690ce5833f2d', N'ALBERTA', N'AB', 124, N'CA.AB', NULL, NULL, N'CA01', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'BC' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'f31b2117-c801-49ae-b6dc-aeb12a10ceba', N'BRITISH COLUMBIA', N'BC', 124, N'CA.BC', NULL, NULL, N'CA02', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'MB' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'a2d5beb7-dd07-47ed-b04c-0ced93ca3ce5', N'MANITOBA', N'MB', 124, N'CA.MB', NULL, NULL, N'CA03', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'NB' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'ea575d65-ccbf-4d75-9288-7b5eaa504562', N'NEW BRUNSWICK', N'NB', 124, N'CA.NB', NULL, NULL, N'CA04', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'NL' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'82cd2094-6b43-4b7c-98ac-3164bd362dc8', N'NEWFOUNDLAND AND LABRADOR', N'NL', 124, N'CA.NL', NULL, NULL, N'CA05', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'NS' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'cdee0642-54ac-4208-bd1d-be02d570cd77', N'NOVA SCOTIA', N'NS', 124, N'CA.NS', NULL, NULL, N'CA06', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'NT' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'c831dcee-f841-40c0-a625-7403533601b3', N'NORTHWEST TERRITORIES', N'NT', 124, N'CA.NT', NULL, NULL, N'CA07', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'NU' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'efc57eb0-58b4-44b1-83d5-8cb9375a828d', N'NUNAVUT', N'NU', 124, N'CA.NU', NULL, NULL, N'CA08', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'ON' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'e003d419-8e29-4513-b830-df455115951c', N'ONTARIO', N'ON', 124, N'CA.ON', NULL, NULL, N'CA09', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'PE' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'805a5c19-2fa1-485d-83ed-bf2453dcd6ce', N'PRINCE EDWARD ISLAND', N'PE', 124, N'CA.PE', NULL, NULL, N'CA10', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'QC' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'985516f4-a9d0-4887-920f-76af9a39355c', N'QUEBEC', N'QC', 124, N'CA.QC', NULL, NULL, N'CA11', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'SK' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'22aa0896-f8f2-42c7-8a3f-84ecd0296237', N'SASKATCHEWAN', N'SK', 124, N'CA.SK', NULL, NULL, N'CA12', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END
	IF NOT EXISTS  (select * from [dbo].[tStates] where Abbr = 'YT' AND CountryCode =124 )
	BEGIN 
		INSERT [dbo].[tStates] ([StatePK], [Name], [Abbr], [CountryCode], [HASC1], [Region], [Label], [FIPS], [DTServerCreate], [DTServerLastModified], [XLicenseId], [TimeZone], [CountryPK]) 
		VALUES (N'beb5b418-1256-49b5-8954-472333bf0dec', N'YUKON', N'YT', 124, N'CA.YT', NULL, NULL, N'CA13', CAST(0x00009C6F0109D359 AS DateTime), CAST(0x00009D0600BAFEFB AS DateTime), NULL, N'Central Standard Time (canada)', N'02e9f23e-de8a-4b71-aa3c-1c2dde671eb4')
	END

END
