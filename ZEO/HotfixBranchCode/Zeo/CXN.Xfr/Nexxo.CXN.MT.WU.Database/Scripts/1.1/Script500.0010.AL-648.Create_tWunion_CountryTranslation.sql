-- ============================================================
-- Author:		<Shwetha Mohan>
-- Create date: <08/11/2015>
-- Description:	<Creating [tWunion_CountryTranslation] TABLE TO STORE COUNTRY NAME IN SPANISH
-- Jira ID:		<AL-648>
-- ============================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWunion_CountryTranslation]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[tWunion_CountryTranslation]
END 

CREATE TABLE [dbo].[tWunion_CountryTranslation]
(
	[WUCountryTranslationPK] [uniqueidentifier] NOT NULL,
	[WUCountryID] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[LanguageCode][varchar](20) NULL,
	[ISOCountryCode][varchar](20) NULL,
	[TranslationName][varchar](200) NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL
)
GO