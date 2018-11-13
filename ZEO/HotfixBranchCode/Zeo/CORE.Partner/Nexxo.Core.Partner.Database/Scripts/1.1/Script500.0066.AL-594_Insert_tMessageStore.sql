--===========================================================================================
-- Auther:			<Kaushik Sakala>
-- Date Created:	<07-July-2015>
-- Description:		<Script for Insert limit exception messages in tMessageStore>
-- Jira ID:			<AL-594>
--===========================================================================================
--Shopping Cart Checkout
INSERT INTO tMessageStore
([MessageStorePK],[MessageKey],[PartnerPK],[Language],[Content],[DTServerCreate],[AddlDetails],[Processor])
  VALUES(NEWID(),1008.6013,1,0,'Exceeded MGiAlloy Limit',GETDATE(),
  					'Please remove {1} transaction with transaction amount {0} to checkout shopping cart','MGiAlloy')