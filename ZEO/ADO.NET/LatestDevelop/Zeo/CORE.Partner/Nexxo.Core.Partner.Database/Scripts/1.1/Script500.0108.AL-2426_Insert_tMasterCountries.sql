--===========================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <Oct 26 2015>
-- Description:	<Script to insert countries to tMasterCountries table>
-- Jira ID:	    <AL-2426>
--===========================================================================================


IF NOT EXISTS(SELECT 1 FROM  tMasterCountries WHERE Name = 'EAST TIMOR')
BEGIN
     INSERT INTO [dbo].[tMasterCountries] ([rowguid], [Name], [Abbr2], [Abbr3], [DTServerCreate], [DTServerLastModified])
     VALUES ('C485A3A3-0E28-47F6-8FEC-C87CD06BCA67', 'EAST TIMOR', 'TP', 'TMP', GETDATE(), GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM  tMasterCountries WHERE Name = 'PALESTINIAN TERRITORY')
BEGIN
     INSERT INTO [dbo].[tMasterCountries] ([rowguid], [Name], [Abbr2], [Abbr3], [DTServerCreate], [DTServerLastModified])
     VALUES ('DFF1DC42-2725-48DE-B78E-212A5AB1F3A3', 'PALESTINIAN TERRITORY', 'PS', 'PSE', GETDATE(), GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM  tMasterCountries WHERE Name = 'REUNION')
BEGIN
     INSERT INTO [dbo].[tMasterCountries] ([rowguid], [Name], [Abbr2], [Abbr3], [DTServerCreate], [DTServerLastModified])
     VALUES ('A6E9B895-55A5-4649-A1DA-32E1094301DF', 'REUNION', 'RE', 'REU', GETDATE(), GETDATE())
END