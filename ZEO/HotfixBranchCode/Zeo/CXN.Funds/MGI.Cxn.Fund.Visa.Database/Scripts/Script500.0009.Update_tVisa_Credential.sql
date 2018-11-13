-- ================================================================================
-- Author:		<Namit khandelwal>
-- Create date: <10/02//2015>
-- Description:	<As VisaDPS processor, I want to update to V15_04 version>
-- Jira ID:		<AL-1703>
-- ================================================================================

update tVisa_Credential
set ServiceUrl = 'https://certservicesgateway.visaonline.com/websrv_prepaid/v15_04/prepaidservices' 
where ChannelPartnerId = 34