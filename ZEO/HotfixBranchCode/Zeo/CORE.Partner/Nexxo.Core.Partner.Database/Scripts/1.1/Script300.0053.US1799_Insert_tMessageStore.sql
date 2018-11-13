--===========================================================================================
-- Author:		SwarnaLakshmi S
-- Create date: Feb 4th 2015
-- Description:	<Script for insert message for Invalid Promo Code >
-- Rally ID:	<US1799>
--===========================================================================================
--
IF NOT Exists( select 1 from TMessageStore where MessageKey = '1010.3017' and PartnerPK=1)
BEGIN
	INSERT INTO [dbo].[TMEssageStore]
		([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],
		[DTCreate],[AddlDetails],[Processor]) 
	VALUES 
		(NewID(),'1010.3017',1,'0','Invalid Promotion Code.',
		getdate(),'Invalid Promotion Code.','')
END
Go
IF NOT Exists( select 1 from TMessageStore where MessageKey = '1001.6044' and PartnerPK=1)
BEGIN
	INSERT INTO [dbo].[TMEssageStore]
		([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],
		[DTCreate],[AddlDetails],[Processor]) 
	VALUES 
		(NewID(),'1001.6044',1,'0','Invalid Referral Code.',
		getdate(),'Invalid Referral Code.','')
END
Go

--===========================================================================================