--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06/25/2018>
-- Description:	 Create a new table for Blocked Countries.
-- ================================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tBlockedCountries]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[tBlockedCountries]
END 

CREATE TABLE [dbo].[tBlockedCountries]
(
	[BlockCountryID] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ISOCountryCode][varchar](20) NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL
)
GO

