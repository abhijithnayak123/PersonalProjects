-- ================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/20/2015>
-- Description:	<Updated Visa Service URL to V15_10 version>
-- Jira ID:		<AL-2190>
-- ================================================================================

UPDATE tVisa_Credential
SET ServiceUrl = 'https://certservicesgateway.visaonline.com/websrv_prepaid/v15_10/prepaidservices' 
WHERE ChannelPartnerId = 34