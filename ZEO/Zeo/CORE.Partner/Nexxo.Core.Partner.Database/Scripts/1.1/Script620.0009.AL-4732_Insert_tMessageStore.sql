--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <04/05/2016>
-- Description:	 Check cashing transaction did not appear on the Ingo settlement file.
--				 Had un-printed status at Ingo
-- Jira ID:		<AL-4732>
-- ================================================================================

INSERT INTO tMessageStore 

	(MessageStorePK,MessageKey,PartnerPK,Language,Content,DTServerCreate,AddlDetails,Processor)

VALUES
	
	(NEWID(),1002.2013,1,0,'{0}',GETDATE(),'Check with Invoice number {0} did not complete. Please contact Ingo support for further steps','Chexar')

GO