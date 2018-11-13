-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <19/10/2015>
-- Description:	<As Alloy, Allow other product transactions if/when GPR processor is down>
-- Jira ID:		<AL-2193>
-- ================================================================================

INSERT INTO [tMessageStore]
           ([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
     VALUES
           (NEWID(),'1003.2107',1,0,'{0}',GETDATE(),'You can continue to perform all transactions except Prepaid Card',NULL)
GO


