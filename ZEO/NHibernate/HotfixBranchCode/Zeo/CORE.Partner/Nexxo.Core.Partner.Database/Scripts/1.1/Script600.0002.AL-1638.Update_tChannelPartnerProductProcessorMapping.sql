-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/20/2015>
-- Description:	<Update CardExpiryPeriod as 36 months for Synovus Visa>
-- Jira ID:		<AL-1638>
-- ================================================================================

DECLARE @synovusChannelPartnerPk UNIQUEIDENTIFIER	
DECLARE @visaProductProcessorId UNIQUEIDENTIFIER
DECLARE @cardExpiryPeriod INT = 36

	
SELECT @synovusChannelPartnerPk = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'Synovus'

SELECT @visaProductProcessorId = PP.rowguid  FROM tProductProcessorsMapping AS PP INNER JOIN 
tProcessors P ON P.rowguid = PP.ProcessorId WHERE P.Name = 'VISA'

-- Update CardExpiryPeriod 36 months for Synovus visa


UPDATE tChannelPartnerProductProcessorsMapping SET CardExpiryPeriod = @cardExpiryPeriod
WHERE ChannelPartnerId = @synovusChannelPartnerPk and ProductProcessorId = @visaProductProcessorId


 