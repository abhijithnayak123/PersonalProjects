SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_IdTypes_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_IdTypes] DROP CONSTRAINT [DF_tFView_IdTypes_DTCreate]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_IdTypes_DTLastMod]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_IdTypes] DROP CONSTRAINT [DF_tFView_IdTypes_DTLastMod]
END
GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[PK_tFView_IdTypes]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_IdTypes] DROP CONSTRAINT [PK_tFView_IdTypes]
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFView_IdTypes]') AND type in (N'U'))
DROP TABLE [dbo].[tFView_IdTypes]
GO

CREATE TABLE [dbo].[tFView_IdTypes](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [int] IDENTITY(1000000000,1) NOT NULL,
	[NexxoIdTypeId] [int] NOT NULL,
	[IdCode] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[CountryCode] [varchar](2) NOT NULL,
	[StateCode] [varchar](2) NOT NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tFView_IdTypes] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tFView_IdTypes] ADD  CONSTRAINT [DF_tFView_IdTypes_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tFView_IdTypes] ADD  CONSTRAINT [DF_tFView_IdTypes_DTLastMod]  DEFAULT (getdate()) FOR [DTLastMod]
GO

--insert into tFView_IdTypes select NEWID(),110,'02','02 - Drivers License #','US','CA',getdate(),getdate()
--insert into tFView_IdTypes select NEWID(),222,'04','04 - Passport','US','CA',getdate(),getdate()
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'18d9b9f9-9aac-4e8c-a9cc-004e6ba6ca56',  146, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'SC', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'00b92a57-2f81-48e7-9455-017264955b62',  213, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'WY', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'7f1716fa-33e2-4953-97c3-01815458f30d',  179, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'KS', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'bdcb92e1-5439-4ad6-b878-01e663f51e20',  180, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'KY', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'36f986cf-92e5-4b0a-9aa4-021857a1c762',  138, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'NY', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'05729ebc-3d1a-4239-bd42-0362ef06386e',  192, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'NH', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'2ceefd11-3335-4d2d-8f39-05882f6b1bc1',  157, N'', N' - WORK PERMIT', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'445054c0-1388-4a44-8c52-063738d894dd',  142, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'OK', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'606abf2c-8c21-4204-bb9e-0639ef41013b',  174, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'HI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'a8834cc9-12d4-4747-903b-066d039fcd12',  188, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'MO', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'd72cd126-ff60-40cb-aced-0738dc99ee0d',  156, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'WY', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'fb58947b-9ba6-4cb8-af82-08cc1bbdb82c',  190, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'NE', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'54d5a4e6-5ebe-4e6f-b077-0c7f98683e59',  222, N'04', N'04 - PASSPORT', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'f1d2329e-a035-4a75-aa73-11a996ff0ce4',  184, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'MA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'a456a48f-198f-46d6-935c-145aa1d6097a',  170, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'DE', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'0204f1b3-1742-49bd-a0bc-1d1b74e25db5',  106, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'AL', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'1b442fe3-6d7f-44c2-97b1-1d72ac63c188',  177, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'IN', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'f9770580-05ab-4b61-91c0-1d8702d0fbf3',  206, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'TX', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'090ff666-554d-4777-b11e-1dff6ec53955',  136, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'NJ', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'e9db3a5d-6a8b-46ec-87eb-200b32a24d53',  208, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'VT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'49f04376-2072-4012-ae48-20325941ee28',  195, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'NY', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'c7477b84-e9c5-4133-8557-236c34eae3ea',  207, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'UT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'e6d8aab2-247e-4ed5-9ef4-2673f60d2b1d',  205, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'TN', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'33bd71db-737d-4901-8e66-276c2de6a3c5',  201, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'PA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'27d9eacc-3167-41a9-926c-295af97eba64',  178, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'IA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'9ae0ddcc-1d56-4309-93c2-2a880797622b',  168, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'CO', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'75a8b83e-3375-411e-bd00-2d3ee353bf04',  223, N'', N' - SAN FRANCISCO CITY ID', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'65cb6669-1c30-42ba-a9fa-2fd1970e1b3f',  123, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'KY', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'904c2844-aa10-4882-adaa-30813abc8873',  151, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'VT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'2e230e29-a1ff-41ce-a0c4-31c417e45283',  169, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'CT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'23cc9314-acbd-418a-82ae-36c49051a781',  175, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'ID', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'efbe2492-5d44-4dbe-9cd8-36f99e6efb2c',  161, N'', N' - RE-ENTRY PERMIT', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'338a7d44-d41e-4640-900e-3c1a31eb249d',  158, N'06', N'06 - GREEN CARD / PERMANENT RESIDENT CARD', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'7e76e9d0-8be9-48db-af7e-3dc8a3efab86',  186, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'MN', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'9044fa7b-69a8-438d-a633-3f613c7b7fe8',  200, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'OR', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'1a32b1be-52b1-4955-b0de-48665fdbde7b',  185, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'MI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'5e71c2d4-2a17-4884-bfde-48724a932ce9',  164, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'AK', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'ac5cd73f-5235-43cc-bfd6-4a330046fdd5',  110, N'02', N'02 - DRIVER''S LICENSE # ', N'US', N'CA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'e1522315-4e78-4690-be3b-4c640ca84ddd',  108, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'AZ', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'00e103a5-61fc-412f-a458-511244b9b81f',  112, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'CT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'3f3dee8e-a55d-45d2-b724-51510a53f327',  143, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'OR', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'f0d057fe-b810-49ba-8f3e-54cb35a0a992',  187, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'MS', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'b21b0073-3aca-4ace-b6e7-5ca063ecc6d8',  139, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'NC', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'448b55be-91bf-40b4-aa3f-5d5a261dc57c',  128, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'MI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'e5c3f0d1-0434-4a40-953f-5fb75864ebf9',  196, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'NC', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'71bab396-5955-4700-9d07-630dbac9234c',  130, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'MS', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'8ec8f1c4-d12e-42f8-8e9b-643cbe3cc83f',  126, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'MD', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'bddb26f6-0aa5-4ebc-a1ae-64c680bc12fe',  147, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'SD', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'8381e172-c0a7-4910-adcc-64d4e49b3f93',  202, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'RI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'54531dd7-8cd9-4328-beef-650ff3dcfffa',  210, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'WA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'16a0aa74-8135-4c13-886d-6950ee01af27',  167, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'CA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'4f29eb69-4229-4ea4-bd38-6e6871570846',  116, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'GA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'c7f6d52f-1cfe-4157-856a-71d5dae4cf69',  122, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'KS', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'1906c104-5606-4446-8850-736af5b89bcd',  121, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'IA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'a8f11a97-8589-4c98-9492-79f372cfc2ec',  141, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'OH', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'2ea3cc66-c151-45d4-a4b9-7b0b25cef2e2',  111, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'CO', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'916de42e-6a3c-4be2-bac4-7b5ba2c2e500',  171, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'DC', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'c17112e1-b957-4147-9ebf-7ccfae4ca6fb',  150, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'UT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'eba3c0b8-c6fd-4939-ba5c-7e6772ac091d',  154, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'WV', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'55d341f2-53a6-459a-86d8-7f4ba9a273e3',  176, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'IL', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'0211026c-a59b-40c7-8503-813bcbc1c3b5',  152, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'VA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'18bb682a-03e9-4b63-83f3-8261acbf266a',  135, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'NH', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'2c018f4e-f917-4e19-91fe-84e1f703d319',  117, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'HI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'39707023-ffa3-4eee-89a7-8bab829bff7c',  166, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'AR', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'39854ee1-a2df-4a4e-8b8e-8efe4714f016',  204, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'SD', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'd713b674-594d-4c69-965a-90c3cb0522ff',  181, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'LA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'7cc0296f-560a-4990-aa18-90f20a1c61cb',  118, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'ID', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'db51a358-4658-42c7-95d2-946688105201',  129, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'MN', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'838b1e75-5fbc-4895-9fe1-95cd6a1c99be',  148, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'TN', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'e8c4f1e2-78b2-4ff7-83d2-9b05572bf7b8',  194, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'NM', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'2467b721-64d9-428e-b2c6-9dfaec075318',  119, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'IL', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'1c76240c-6cec-4c14-baf4-9f9a29dbd82a',  173, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'GA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'958745e1-11d7-4039-ae54-a1dc21e8dfe7',  162, N'', N' - REFUGEE TRAVEL DOCUMENT', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'cfd773cf-be7b-4d1c-81ed-a820273d4504',  193, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'NJ', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'a7cd8cd7-8c72-4af2-99cd-aa0b7806a3c7',  197, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'ND', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'2921f3ee-5c4a-47d1-a0c6-aab5c230d768',  189, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'MT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'b6d25a3d-f8a3-4705-b9ff-ab9acbe8a899',  131, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'MO', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'e77c6cda-f542-4f54-99c7-ac837c2727a5',  153, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'WA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'cea742d6-172c-4d13-9ebe-b1a663ad36a2',  133, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'NE', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'92f89578-e8b1-49fb-a5ef-b1cd8f753e29',  199, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'OK', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'295a5101-c9ee-462e-87d4-b3806aa70b42',  140, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'ND', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'c80a9746-334b-4cb9-83b5-b4a0458eff18',  191, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'NV', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'71086c1e-7f6e-4804-861c-bc5ac1390b23',  182, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'ME', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'c4fca384-d803-473c-b7fd-bd7543c8700e',  212, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'WI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'9cac9e6d-be59-46b3-b3ba-c0ad6076c02c',  115, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'FL', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'7a33fb15-c69a-4a0e-9901-c1006ef85860',  163, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'AL', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'd0a87975-050f-4184-ba31-c130bfe47050',  183, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'MD', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'06435f5c-e85f-41d0-9114-c439e708d9e6',  160, N'', N' - EMPLOYMENT AUTHORIZATION CARD (EAD0', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'43d4b57a-1363-4fa2-ae32-c488807b5e9e',  125, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'ME', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'04b101c8-9143-4c4a-a3fc-c8971717e5f7',  144, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'PA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'3639a235-7101-4261-8a96-cdd2a6ab9ea1',  172, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'FD', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'06b32cfa-1523-41f9-b690-cefefbe7a8f5',  132, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'MT', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'9f98b33f-6b94-4ed6-bc21-d129970c5ad2',  159, N'', N' - TEMPORARY RESIDENT CARD', N'US', N'', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'0459fe0e-09e6-4488-8865-d1974b2dce20',  198, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'OH', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'112a4360-55ac-4509-9510-d8fcd61c1f6c',  127, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'MA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'f16cff11-6c6d-4005-bba5-dad5519f2d1e',  165, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'AZ', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'58a30a11-580b-40ce-ab94-df3c379b58ec',  109, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'AR', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'c3e0392a-940b-4504-b610-e0da70dd7677',  120, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'IN', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'a6a39ade-6e76-4ba4-a70d-e1a26020c44f',  145, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'RI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'91934a74-ecdd-451b-9fbd-e7bde2c0e749',  124, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'LA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'ca5e34a6-c281-42e8-967c-e8ba8f9101e3',  155, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'WI', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'2dbd6ec2-8720-4df2-bc7a-edf36e950fba',  203, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'SC', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'd5fa42ab-b01f-4cdc-9aa0-ee13838921ba',  113, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'DE', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'db420d25-547d-4aff-93a8-eef9ccfac076',  209, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'VA', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'96921162-2700-408f-ba89-f105c964527d',  137, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'NM', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'5b3a9118-ffea-4f77-bb9f-f10fa989454c',  134, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'NV', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'328a4aa2-6587-4563-9fab-f2175c49b8fc',  114, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'DC', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'83d79fd8-9882-4d3d-8503-f680986afb16',  211, N'07', N'07 - U.S. STATE IDENTITY CARD', N'US', N'WV', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'740ffb0c-45d0-4817-a656-f723c6de6b30',  149, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'TX', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'd1e1da7c-d05c-4894-98c4-f81e05419b9a',  107, N'02', N'02 - DRIVER''S LICENSE #', N'US', N'AK', GETDATE(), GETDATE())
INSERT [dbo].[tFView_IdTypes] ([rowguid], [NexxoIdTypeId], [IdCode], [Name], [CountryCode], [StateCode], [DTCreate], [DTLastMod]) VALUES (N'e054b0db-31d2-4cbf-bc92-fa41a5afb2ab',  224, N'08', N'08 - MILITARY ID', N'US', N'', GETDATE(), GETDATE())
