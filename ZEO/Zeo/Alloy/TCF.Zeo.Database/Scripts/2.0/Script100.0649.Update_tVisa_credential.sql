--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <11-20-2017>
-- Description:	Updating Url in credentials Table. 
--Jira ID:	    <B-10524 - Visa Upgrade>
-- ================================================================================


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Credential' AND COLUMN_NAME = 'ServiceUrl')
BEGIN
	UPDATE 
		[dbo].[tVisa_Credential] 
	SET 
		ServiceUrl = 'https://zeo-nonprod.tcfbank.com/visa/websrv_prepaid/v17_10/prepaidservices' 
END