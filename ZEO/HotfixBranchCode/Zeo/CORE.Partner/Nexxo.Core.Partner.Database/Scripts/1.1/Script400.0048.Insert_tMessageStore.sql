--===========================================================================================
-- Author:		SwarnaLakshmi S
-- Create date: Feb 27th 2015
-- Description:	<Script for insert message for Customer Multiple Account Found >
-- Jira ID:	
--===========================================================================================
--
IF NOT Exists( select 1 from TMessageStore where MessageKey = '1001.3015' and PartnerPK=1)
BEGIN
	INSERT INTO [dbo].[TMEssageStore]
		([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],
		[DTCreate],[AddlDetails],[Processor]) 
	VALUES 
		(NewID(),'1001.3015',1,'0','Customer Multiple Account Found.',
		getdate(),'Customer Multiple Account Found.','')
END
Go


--===========================================================================================