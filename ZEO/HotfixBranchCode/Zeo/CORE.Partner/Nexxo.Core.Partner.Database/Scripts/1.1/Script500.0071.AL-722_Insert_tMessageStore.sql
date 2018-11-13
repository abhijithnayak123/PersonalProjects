--- ================================================================================
-- Author:		<Divya>
-- Create date: <07/22/2015>
-- Description:	<Attempt to print the same money order twice and confirm the error message is displayed>
-- Jira ID:		<AL-722>
-- ================================================================================
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1006.6003')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1006.6003',1,0,'Money Order has already been issued. Please use a different Money Order. Also validate previously printed Money Orders have matching check numbers on the MICR line and as printed',GETDATE(),
  					'MoneyOrder already been Issued','MGiAlloy')
END
