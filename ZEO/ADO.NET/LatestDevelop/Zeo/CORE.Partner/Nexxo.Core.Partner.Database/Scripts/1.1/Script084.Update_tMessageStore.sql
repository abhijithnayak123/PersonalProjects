update tMessageStore 
set Content = 'Customer Profile is Under review',
AddlDetails='Customer Profile is Under review and should be complete within 2 minutes'
where MessageKey='1008.5000'
go