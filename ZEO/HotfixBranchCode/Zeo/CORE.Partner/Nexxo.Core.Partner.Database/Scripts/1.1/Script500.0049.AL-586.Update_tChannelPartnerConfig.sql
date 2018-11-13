-- ================================================================
-- Author:		Kaushik Sakala
-- Create date: <07/07/2015>
-- Description:	<TCF: Updating CanEnableProfileStatus value for TCF channel partner as false.
-- JIRA ID:	<AL-586>
-- =================================================================


BEGIN 
	Declare @ChannelPartnerId UNIQUEIDENTIFIER

	SELECT @ChannelPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'TCF'

	UPDATE 
		tChannelPartnerConfig
	SET 
		CanEnableProfileStatus = 0 
	WHERE 
	ChannelPartnerPK = @ChannelPartnerId
	END
GO
