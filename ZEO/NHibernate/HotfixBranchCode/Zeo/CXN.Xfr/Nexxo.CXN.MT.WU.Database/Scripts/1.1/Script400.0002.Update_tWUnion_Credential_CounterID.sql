--===========================================================================================
-- Auther:			<RAJKUMAR>
-- Date Created:	<28/01/2015>
-- Description:		<Script to update Western Union credential for channel partner 'TCF'>
--===========================================================================================
BEGIN
	UPDATE    
		tWUnion_Credential
	SET 
		CounterId = '990000402'
	WHERE
		(ChannelPartnerId = 34)
END
GO

