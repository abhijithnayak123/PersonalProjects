--- ===============================================================================
-- Author:		<Pushkal>
-- Create date: <12-01-2017>
-- Description:	 Update the display message for the key 1002.200.7
-- ================================================================================

IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1002.200.7' AND ChannelPartnerId IN (1, 34))
BEGIN
	UPDATE tMessageStore 
	SET DisplayMessage = N'Resubmit if you have more info-Duplicate Check. Call Retail Banking Support (RBS) if assistance is needed.'
	WHERE MessageKey = '1002.200.7' AND ChannelPartnerId IN (1, 34)
END 