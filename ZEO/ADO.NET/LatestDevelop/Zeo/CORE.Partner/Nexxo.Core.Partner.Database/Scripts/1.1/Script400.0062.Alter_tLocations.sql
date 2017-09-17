--===========================================================================================
-- Author:		<Abhijith>
-- Create date: <Mar 26 2015>
-- Description:	<Adding Location Identifier column>
-- Jira ID:	    <AL-217>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = N'LocationIdentifier' AND OBJECT_ID = OBJECT_ID(N'tLocations'))
BEGIN
	ALTER TABLE [dbo].[tLocations]
	ADD [LocationIdentifier] [varchar](50) NULL
END
GO