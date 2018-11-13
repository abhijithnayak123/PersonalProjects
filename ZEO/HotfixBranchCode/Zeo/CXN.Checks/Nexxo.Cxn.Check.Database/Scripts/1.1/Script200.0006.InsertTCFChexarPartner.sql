--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	2/10/2014
-- Description:		Script for insert record into [tChxr_Partner] for TCF
--===========================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChxr_Partner]') 
AND type in (N'U'))
BEGIN
	IF EXISTS(SELECT * FROM [dbo].[tChxr_Partner] WHERE [Id] = 34)
	BEGIN
		UPDATE [dbo].[tChxr_Partner] SET [Name] = N'TCF', 
		URL = N'http://beta.chexar.net/webservice/', DTLastMod = GETDATE()
		WHERE [Id]=34
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tChxr_Partner]
			([rowguid], [Id], [Name], [URL], [DTCreate], [DTLastMod]) 
		VALUES
			(NEWID(), 34, N'TCF','http://beta.chexar.net/webservice/',GETDATE(),NULL)
	END
END
Go
--===========================================================================================