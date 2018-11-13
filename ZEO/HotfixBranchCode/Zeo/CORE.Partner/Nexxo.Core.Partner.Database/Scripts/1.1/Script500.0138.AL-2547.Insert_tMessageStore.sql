--- ================================================================================
-- Author:		<Divya>
-- Create date: <12/01/2015>
-- Description:	<As WU processor, I want to translate the error message for unable to modify>
-- Jira ID:		<AL-2547>
-- ================================================================================
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1005.3343' and Processor = 'Western Union')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1005.3343',1,0,'This transaction can only be modified by Western Union Customer Service',GETDATE(),
  					'Please have the customer contact Western Union directly at the number referenced on the receipt',' Western Union')
END