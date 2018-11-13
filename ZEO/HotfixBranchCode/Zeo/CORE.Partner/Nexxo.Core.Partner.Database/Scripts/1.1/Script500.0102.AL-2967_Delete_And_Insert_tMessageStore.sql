
--- ================================================================================
-- Author:		<Divya>
-- Create date: <11/16/2015>
-- Description:	<As Alloy, add a translation for WU error code T0425>
-- Jira ID:		<AL-2967>
-- ================================================================================

-- As per karun's decision, added Errormessage for WU error code T0425 for synovus,carver and Tcf.

delete from tMessageStore where MessageKey='1004.425' and PartnerPK=33 and AddlDetails is Null

delete from tMessageStore where MessageKey='1004.425' and PartnerPK=28 and AddlDetails is Null

delete from tMessageStore where MessageKey='1004.425' and PartnerPK=34 and AddlDetails is Null

--For carver Billpay
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425'and PartnerPK=28)
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1004.425',28,0,'This transaction requires additional customer information',GETDATE(),
  					'Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer','Western Union')
END
GO
--For synovus Billpay
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425'and PartnerPK=33)
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1004.425',33,0,'This transaction requires additional customer information',GETDATE(),
  					'Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer','Western Union')
END
GO
--For Tcf Billpay
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425'and PartnerPK=34)
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1004.425',34,0,'This transaction requires additional customer information',GETDATE(),
  					'Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer','Western Union')
END
GO