--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	12/17/2013
-- Description:		Script for insert record into [tChxr_Partner] 
--===========================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChxr_Partner]') 
AND type in (N'U'))
BEGIN
	IF EXISTS(SELECT * FROM [dbo].[tChxr_Partner] WHERE [Id] = 28)
	BEGIN
		UPDATE [dbo].[tChxr_Partner] SET [Name] = N'Carver', 
		URL = N'http://beta.chexar.net/webservice/', DTLastMod = GETDATE()
		WHERE [Id]=28
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tChxr_Partner]
			([rowguid], [Id], [Name], [URL], [DTCreate], [DTLastMod]) 
		VALUES
			(NEWID(), 28, N'Carver','http://beta.chexar.net/webservice/',GETDATE(),NULL)
	END
END
Go
--===========================================================================================