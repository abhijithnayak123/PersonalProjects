--===========================================================================================
-- Auther:			<RAJKUMAR>
-- Date Created:	<20/11/2014>
-- Description:		<Script to update Western Union credential for channel partner 'TCF'>
--===========================================================================================
IF EXISTS
(
	SELECT 1 FROM tWUnion_Credential WHERE (ChannelPartnerId = 34)
)
BEGIN
	UPDATE    
		tWUnion_Credential
	SET 
		AccountIdentifier = 'WGHH673600T'
	WHERE
		(ChannelPartnerId = 34)
END
GO

