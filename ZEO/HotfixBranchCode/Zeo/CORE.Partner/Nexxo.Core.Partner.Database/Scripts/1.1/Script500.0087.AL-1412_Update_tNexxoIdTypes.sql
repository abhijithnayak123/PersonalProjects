-- ================================================================================
-- Author:		<Namit Khandelwal>
-- Create date: <08/27/2015>
-- Description:	<Update the validation for NEW YORK BENEFITS ID and NEW YORK CITY ID>
-- Jira ID:		<AL-1412>
-- ================================================================================


 update tNexxoIdTypes
 set Mask = '^\w{4,20}$' 
 where NexxoIdTypeID = 488 and Name = 'NEW YORK BENEFITS ID'
 Go

 update tNexxoIdTypes
 set Mask = '^\w{4,20}$' 
 where NexxoIdTypeID = 487 and Name = 'NEW YORK CITY ID'
 Go
