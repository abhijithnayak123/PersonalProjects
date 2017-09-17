
IF EXISTS(SELECT * FROM [tChxr_Partner] WHERE Id = 33)
	BEGIN
		UPDATE tChxr_Partner SET [Name] = N'Synovus', URL = N'http://beta.chexar.net/webservice/', DTLastMod = GETDATE()
	END
ELSE
	BEGIN
		INSERT INTO tChxr_Partner ([rowguid], [Id], [Name], [URL], [DTCreate], [DTLastMod]) VALUES
			(NEWID(), 33, N'Synovus','http://beta.chexar.net/webservice/',GETDATE(),NULL)
	END
