--- ================================================================================
-- Author:		<Divya>
-- Create date: <10/26/2015>
-- Description:	<As InGo processor, I want to treat designated error codes as declines>
-- Jira ID:		<AL-2013>
-- ================================================================================
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1002.-1')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1002.-1',1,0,'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer.',GETDATE(),'','Chexar')
  					
END

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1002.-2')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1002.-2',1,0,'THE CHECK CANNOT BE APPROVED.The check is stale dated and cannot be cashed. Please return the check to the customer.',GETDATE(),'','Chexar')
  					
END

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1002.-3')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1002.-3',1,0,'YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check is post dated. Please return the check to the person who issued it to correct the date and resubmit.',GETDATE(),'','Chexar')
  					
END
GO