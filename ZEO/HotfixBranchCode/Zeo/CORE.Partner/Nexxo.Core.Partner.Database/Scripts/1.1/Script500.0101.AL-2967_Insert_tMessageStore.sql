--- ================================================================================
-- Author:		<Divya>
-- Create date: <11/13/2015>
-- Description:	<As Alloy, add a translation for WU error code T0425>
-- Jira ID:		<AL-2967>
-- ================================================================================

--MoneyTransfer
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1005.425')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1005.425',1,0,'This transaction requires additional customer information',GETDATE(),
  					'Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer','Western Union')
END
Go
--Billpay
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1004.425',1,0,'This transaction requires additional customer information',GETDATE(),
  					'Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer','Western Union')
END
GO
