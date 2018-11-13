--===========================================================================================
-- Auther:			<Kaushik Sakala>
-- Date Created:	<07-July-2015>
-- Description:		<Script for Update limits related messages in tMessageStore>
-- Jira ID:			<AL-594>
--===========================================================================================

--Bill Pay
update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6000'

update tMessageStore
set tMessageStore.AddlDetails = 'Minimum Amount required for this transaction {0}',
tMessageStore.Content = 'Bill Pay Amount is less than Minimum Amount',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6011'

--Money Transfer

update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6006'

update tMessageStore
set tMessageStore.AddlDetails = 'Minimum Amount required for this transaction {0}',
tMessageStore.Content = 'Money Transfer Amount is less than Minimum Amount',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6009'

--Money Order

update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6005'

update tMessageStore
set tMessageStore.AddlDetails = 'Minimum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Money Order Amount is less than Minimum Amount',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6010'

--Check Processing

update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6003'

update tMessageStore
set tMessageStore.AddlDetails = 'Minimum Amount required for this transaction {0}',
tMessageStore.Content = 'Check Transaction Amount is less than Minimum Amount',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6008'

-- Cash Transaction

update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6001'

update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6002'

update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6004'

update tMessageStore
set tMessageStore.AddlDetails = 'Minimum Amount required for this transaction {0}',
tMessageStore.Content = 'Money Transfer Amount is less than Minimum Amount',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6007'

update tMessageStore
set tMessageStore.AddlDetails = 'Maximum Amount allowed for this transaction {0}',
tMessageStore.Content = 'Exceeded MGiAlloy Limit Check',tMessageStore.Processor = 'MGiAlloy'
where MessageKey like '1008.6012'
