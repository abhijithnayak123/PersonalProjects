--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03-21-2017>
-- Description:	Updating Url in credentials Table. 
-- ================================================================================


UPDATE 
	[dbo].[tVisa_Credential] 
SET 
	ServiceUrl = 'https://proxy.ic.local/visa/websrv_prepaid/v16_10/prepaidservices' 