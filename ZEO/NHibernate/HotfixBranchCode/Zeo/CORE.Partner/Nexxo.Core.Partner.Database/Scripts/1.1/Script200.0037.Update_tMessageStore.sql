--===========================================================================================
-- Author:		<Pamila Jose>
-- Create date: <05/23/2014>
-- Description:	<Changed the Message Key since it was conflicting with the Provider_Error constant>
-- Rally ID:	<DE2808>
--===========================================================================================
UPDATE [dbo].[tMessageStore] 
	SET 
		[MessageKey]='1011.2005',
		[DTLastMod]=getdate()
	WHERE 
		[MessageKey]='1011.2004'
	AND [PartnerPK]=1
	AND [Language]='0'

GO
