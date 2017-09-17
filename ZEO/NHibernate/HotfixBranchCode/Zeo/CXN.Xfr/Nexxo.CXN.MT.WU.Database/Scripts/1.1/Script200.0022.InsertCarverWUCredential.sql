--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	12/17/2013
-- Description:		Script for insert record into [tWUnion_Credential]  
--===========================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Credential]') 
AND type in (N'U'))
BEGIN
	IF EXISTS(SELECT * FROM [dbo].[tWUnion_Credential] WHERE [ChannelPartnerId] = 28)
	BEGIN
		UPDATE [dbo].[tWUnion_Credential] 
		SET [WUServiceUrl] = 'https://wugateway2pi.westernunion.net', 
			[WUClientCertificateSubjectName] = 'westernunionpartnerintegration',
			[AccountIdentifier]='WGHH614900T',
			[CounterId]='6149PT00001A',
			[ChannelName]='ESP',
			[ChannelVersion]='9500',
			DTLastMod = GETDATE()
		WHERE [ChannelPartnerId] = 28
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tWUnion_Credential] 
			([rowguid],[WUServiceUrl],[WUClientCertificateSubjectName],[AccountIdentifier],
			[CounterId],[ChannelName],[ChannelVersion],[ChannelPartnerId],[DTCreate],[DTLastMod])
		VALUES 
			(NEWID(),'https://wugateway2pi.westernunion.net','westernunionpartnerintegration',
			'WGHH614900T','6149PT00001A','ESP','9500',28,GETDATE(),NULL)
	END
END
GO
--===========================================================================================