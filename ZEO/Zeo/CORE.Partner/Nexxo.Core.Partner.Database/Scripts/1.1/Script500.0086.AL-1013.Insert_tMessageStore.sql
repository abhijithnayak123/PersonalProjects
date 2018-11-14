--- ================================================================================
-- Author:		<Namit>
-- Create date: <08/24/2015>
-- Description:	<Incorrect processor name is displayed in the pop up error message while doing a send money transaction>
-- Jira ID:		<AL-1013>
-- ================================================================================
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1005.2003')
BEGIN
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),'1005.2003',1,0,'Error in Card lookup E9201 MISSING/INVALID PREFERRED CUSTOMER ACCOUNT NUMBER',GETDATE(),
  					'System.Exception','WesternUnion')
END
