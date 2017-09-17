--- ================================================================================
-- Author:		<RAJKUMAR>
-- Create date: <09/11/2015>
-- Description:	<Ingo Unhandled decline code to display generic message in mouse hover on Decline check>
-- Jira ID:		<AL-1578>
-- ================================================================================
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1002.0' and Processor = 'Chexar')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1002.0',1,0,'THE CHECK CANNOT BE APPROVED AT THIS TIME. Please contact INGO for resolution',GETDATE(),
  					'','Chexar')
END
