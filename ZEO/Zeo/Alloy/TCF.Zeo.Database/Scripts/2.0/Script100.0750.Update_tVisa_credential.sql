--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-10-2018>
-- Description:	Updating Url in credentials Table. 
-- ================================================================================
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Credential' AND COLUMN_NAME = 'ServiceUrl')
BEGIN
	UPDATE 
		[dbo].[tVisa_Credential] 
	SET 
		ServiceUrl = 'https://zeo-nonprod.tcfbank.com/visa/websrv_prepaid/v18_04/prepaidservices' 
END