--===========================================================================================
-- Author:		Rogy Eapen
-- Create date: Mar 04 2015
-- Description:	<Script for insert message for Western Union counter Id is not available or 
--- has not been correctly setup >
-- Jira ID:	AL-90
--===========================================================================================

IF NOT Exists( select 1 from TMessageStore where MessageKey = '1005.2023' and PartnerPK=1)
BEGIN
INSERT INTO TMEssageStore(Rowguid,MessageKey,PartnerPK,Language,Content,DTCreate,AddlDetails,Processor) 
VALUES(NewID(),'1005.2023 ',1,'0','Western Union counter Id is not available or has not been correctly setup',getdate(),'Please contact the System Administrator','')
END
GO

IF NOT Exists( select 1 from TMessageStore where MessageKey = '1005.2416' and PartnerPK=1)
BEGIN
INSERT INTO TMEssageStore(Rowguid,MessageKey,PartnerPK,Language,Content,DTCreate,AddlDetails,Processor) 
VALUES(NewID(),'1005.2416 ',1,'0','Western Union counter Id is not available or has not been correctly setup',getdate(),'Please contact the System Administrator','')
END
GO