-- ============================================================
-- Author:		<Shwetha Mohan>
-- Create date: <08/11/2015>
-- Description:	<Creating [tWunion_DeliveryTranslations] TABLE TO STORE DELIVERY SERVICE NAME IN SPANISH
-- Jira ID:		<AL-648>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWunion_DeliveryTranslations]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[tWunion_DeliveryTranslations]
END 

CREATE TABLE [dbo].[tWunion_DeliveryTranslations]
(
	[WUDeliveryTranslationsPK] [uniqueidentifier] NOT NULL,
	[EnglishName] [varchar] (200) NULL,
	[TanslatedDeliveryServiceName][varchar](200) NULL,
	[LanguageCode][varchar](20) NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL
)
GO