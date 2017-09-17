-- ================================================================
-- Author:		<Sunil Shetty>
-- Create date: <06/22/2015>
-- Description:	<Configuring the mailing address for TCF>
-- Jira ID:		<AL-533>
-- ================================================================

BEGIN 
	DECLARE @ChannelPartnerId UNIQUEIDENTIFIER

	SELECT @ChannelPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'TCF'

	UPDATE 
		tChannelPartnerConfig
	SET 
		IsMailingAddressEnable = 0 
	WHERE 
		ChannelPartnerPK = @ChannelPartnerId
END
GO
